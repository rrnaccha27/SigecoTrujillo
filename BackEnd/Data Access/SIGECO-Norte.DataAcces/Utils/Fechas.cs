using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace SIGEES.DataAcces.Helper.Utils
{
    public static class Fechas
    {
        public static String convertDateTimeToString(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static String convertDateTimeToString(Nullable<System.DateTime> fecha)
        {
            return fecha != null ? fecha.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
        }
        public static String convertDateToShortString(Nullable<System.DateTime> fecha)
        {
            return fecha != null ? fecha.Value.ToString("dd/MM/yyyy") : "";
        }
        public static String convertDateToString(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }
        public static String convertDateToString(Nullable<System.DateTime> fecha)
        {
            return fecha != null ? fecha.Value.ToString("dd/MM/yyyy") : "";
        }
    }
}