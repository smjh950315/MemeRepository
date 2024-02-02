namespace Cyh
{
    public class MyFileHelper
    {
        static string[] _UnHandledFileList(bool onlyTop, string root, string searchPattern) {
            SearchOption option = onlyTop ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return Directory.GetFiles(root, searchPattern, option);
        }
        static IEnumerable<string> _GetFileList(bool onlyTop, string root, bool withRoot, string searchPattern = "*.*") {
            if (File.Exists(root))
                throw new FileNotFoundException("the input root path is a file, input a folder path instead!");
            if (Directory.Exists(root)) {
                string[] files = _UnHandledFileList(onlyTop, root, searchPattern);
                if (withRoot) {
                    return files;
                } else {
                    List<string> result = new List<string>();
                    int lengthOfRoot = root.Length;
                    foreach (string file in files) {
                        result.Add(file.Substring(lengthOfRoot));
                    }
                    return result;
                }
            } else {
                throw new FileNotFoundException($"the path {root} does not exist!");
            }
        }

        /// <summary>
        /// 取得檔案清單(不包含根目錄)
        /// </summary>
        /// <param name="onlyTop">是否只取第一層檔案目錄</param>
        /// <param name="root">要掃描的根目錄</param>
        /// <param name="searchPattern">搜尋的特徵</param>
        /// <returns>掃瞄出的檔案清單</returns>
        /// <exception cref="FileNotFoundException">不存在該目錄時擲回例外</exception>
        public static IEnumerable<string> GetFileList(bool onlyTop, string root, string searchPattern = "*.*") {
            return _GetFileList(onlyTop, root, false, searchPattern);
        }

        /// <summary>
        /// 取得檔案清單(包含根目錄)
        /// </summary>
        /// <param name="onlyTop">是否只取第一層檔案目錄</param>
        /// <param name="root">要掃描的根目錄</param>
        /// <param name="searchPattern">搜尋的特徵</param>
        /// <returns>掃瞄出的檔案清單</returns>
        /// <exception cref="FileNotFoundException">不存在該目錄時擲回例外</exception>
        public static IEnumerable<string> GetFileListWithRoot(bool onlyTop, string root, string searchPattern = "*.*") {
            return _GetFileList(onlyTop, root, true, searchPattern);
        }
    }
}
