using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SysAidArchive
{
    static class Settings
    {
        public static string dbConnectionString = ConfigurationManager.ConnectionStrings["SysAidDatabase"].ConnectionString;
    }
}