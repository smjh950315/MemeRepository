using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.WebServices.AppConfigs
{
    public class DefaultViewSettings : IViewSettings
    {
        public string Title_CurrentUser { get; set; }

        public string Title_Default { get; set; }

        public bool Display_LoginState { get; set; }

        public bool Display_AppTitle { get; set; }

        public bool Display_ActionName { get; set; }
    }
}
