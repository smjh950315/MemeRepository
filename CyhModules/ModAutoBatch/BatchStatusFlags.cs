using System.Collections;
using System.Runtime.InteropServices;

namespace Cyh.Modules.ModAutoBatch
{
    unsafe public struct StatusEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        T* _Ptr;
        int _Count;
        int _Pos;
        public T Current {
            get {
                if (this._Ptr == null)
                    return default;
                return this._Ptr[this._Pos];
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() { }

        public bool MoveNext() {
            return ++this._Pos < this._Count;
        }

        public void Reset() {
            this._Pos = -1;
        }

        public StatusEnumerator() {
            this._Ptr = default;
            this._Count = 0;
            this._Pos = 0;
        }
        public StatusEnumerator(T* ptr, int count) {
            this._Ptr = ptr;
            this._Count = count;
            this._Pos = 0;
        }
    }

    unsafe public struct BatchStatusFlags : IEnumerable<BATCH_STATUS>
    {
        private readonly int _Capacity;
        private readonly int* _PtrCount;
        private readonly uint* _PtrFlags;
        private readonly uint* _GetMyPtr(int index) {
            if (index < 0 || index >= *this._PtrCount)
                throw new ArgumentOutOfRangeException("index");
            return this._PtrFlags + index;
        }
        public readonly int Capacity => this._Capacity;
        public readonly int Count => *this._PtrCount;
        public readonly int Register() {
            if (this.Count >= this.Capacity)
                return -1;
            *(this._PtrCount) += 1;
            return this.Count - 1;
        }

        readonly IEnumerator IEnumerable.GetEnumerator() {
            return new StatusEnumerator<BATCH_STATUS>((BATCH_STATUS*)this._PtrFlags, this.Count);
        }
        public readonly IEnumerator<BATCH_STATUS> GetEnumerator() {
            return new StatusEnumerator<BATCH_STATUS>((BATCH_STATUS*)this._PtrFlags, this.Count);
        }
        public readonly IEnumerable<int> GetExecuted() {
            List<int> res = new List<int>();
            int current = 0;
            foreach (var item in this) {
                if (item.IsInvoked())
                    res.Add(current);
                current++;
            }
            return res;
        }
        public readonly BATCH_STATUS this[int index] {
            get => (BATCH_STATUS)(*this._GetMyPtr(index));
            set => *this._GetMyPtr(index) = (uint)value;
        }
        public BatchStatusFlags(int flagCapacity) {
            this._Capacity = flagCapacity;
            this._PtrCount = (int*)Marshal.AllocHGlobal(sizeof(int));
            *this._PtrCount = 0;
            this._PtrFlags = (uint*)Marshal.AllocHGlobal(flagCapacity * sizeof(uint));
        }
    }
}
