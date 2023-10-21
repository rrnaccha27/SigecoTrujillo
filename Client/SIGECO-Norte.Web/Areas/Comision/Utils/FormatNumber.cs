using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace SIGEES.Web.Areas.Comision.Utils
{
    public static class Format
    {
        public static String quitarPuntoDecimal(Double monto)
        {
            String retorno = "00";
            try
            {
                //monto = monto * 10;
                retorno = monto.ToString("N2").Replace(",", "").Replace(".", "");
            }
            catch (Exception ex)
            {
                retorno = "0";
                Console.WriteLine("Error en quitar los decimales:" + ex.ToString());
            }

            return retorno;

        }

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