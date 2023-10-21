using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SIGEES.DataAcces
{
    public class ConexionData
    {
        public static string SIGEES()
        {
            return ConfigurationManager.ConnectionStrings["cnSIGEES"].ConnectionString;
        }        
    }
}
