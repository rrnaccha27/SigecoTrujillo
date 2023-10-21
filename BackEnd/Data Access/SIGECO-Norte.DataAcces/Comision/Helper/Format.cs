using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces.Comision.Helper
{
    public static class Format
    {
        public static string DecimalToString(decimal valor, int decimales)
        {
            string valor_string = valor.ToString(new System.Globalization.CultureInfo("en-US"));
            string valor_string_decimal = string.Empty;
            string valor_string_entero = string.Empty;
            int posicion_decimal = valor_string.IndexOf(Convert.ToChar("."), 0);

            valor_string_entero = valor_string.Substring(0, posicion_decimal);
            valor_string_decimal = valor_string.Substring(posicion_decimal + 1);

            if (valor_string_decimal.Length < decimales)
            {
                valor_string_decimal = valor_string_decimal + "00000";
                valor_string_decimal = valor_string_decimal.Substring(0, decimales);
            }

            valor_string = valor_string_entero + "." + valor_string_decimal;

            return valor_string;
        }

    }
}
