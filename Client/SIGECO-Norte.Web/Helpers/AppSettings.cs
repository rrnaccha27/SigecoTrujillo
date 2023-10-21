using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;

namespace SIGEES.Web.Helpers
{
    public static class AppSettings
    {
        public static string AppName()
        {
            return ConfigurationManager.AppSettings["AppName"].ToString();
        }
        public static int AppContadorRedireccion()
        {
            
            try
            {
                return int.Parse( ConfigurationManager.AppSettings["AppContadorRedireccion"].ToString());
            }
            catch (Exception ex)
            {

                string str_contador = ex.Message;
                return 5;
            }
            
        }
        public static string AppFecha()
        {
            string resultado = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd, dd {} MMMM {} yyyy"));
            return resultado.Replace("{}","de");//&#100;&#101; = 'de'
            //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd, dd de; MMMM de; yyyy"));//&#100;&#101; = 'de'

            //return ConfigurationManager.AppSettings["AppName"].ToString();
        }
    }
}