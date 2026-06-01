using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLDdataAccess
{
    public static class ClsDataAccessSettings
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
    }
}
