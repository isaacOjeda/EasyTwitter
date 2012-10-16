using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTwitter.Config
{
    public static class EasyTwitterConfigHelper
    {
        public static Uri GetCallBackUrl
        {
            get
            {
                EasyTwitterConfiguration obj = (EasyTwitterConfiguration)
                    System.Configuration.ConfigurationManager.GetSection("EasyTwitterGroup/EasyTwitterConfig");
                return obj.CallBackUri;
            }
        }
    }
}
