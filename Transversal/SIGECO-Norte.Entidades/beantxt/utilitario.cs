using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
    class utilitario
    {
        public String quitarPuntoDecimal(Double monto)
        {
            String retorno = "00";
            try
            {
                string algo = monto.ToString("N2");
                retorno = monto.ToString("N2").Replace(",", "").Replace(".", "");
            }
            catch (Exception ex)
            {
                retorno = "0";
                Console.WriteLine("Error en quitar los decimales:" + ex.ToString());
            }

            return retorno;

        }
        public static String Left(String stringVal, Int32 lenght)
        {
            stringVal = stringVal == null ? "" : stringVal;
            Int32 _lenght = stringVal.Length;
            if (_lenght > lenght)
            {
                stringVal = stringVal.Substring(0, lenght);
            }
            return stringVal;
        }

        public static String Right(String stringVal, Int32 lenght)
        {
            stringVal = stringVal == null ? "" : stringVal;
            Int32 _lenght = stringVal.Length;

            if (_lenght > lenght)
            {
                Int32 startIndex = _lenght - lenght;
                stringVal = stringVal.Substring(startIndex);
            }
            return stringVal;
        }
    }
}
