using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Utils
{
    public class Calculo
    {

        public static int CalcularEdad(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;
            return age;
        }

        public static String[] CrearArrayLetras(int v_columna_total)
        {

            v_columna_total++;
            var v_leyenda_x = new string[v_columna_total];
            var contador_leyenda = 0;
            var contador_fila = 0;

            for (int j = 0; j < v_columna_total; j++)
            {
                var letter = "-";

                if ((65 + contador_leyenda) <= 90)
                {

                    char numero = Convert.ToChar(65 + contador_leyenda);

                    letter = new String(new char[] { numero });
                    if (contador_fila > 0)
                    {
                        letter = letter + "" + contador_fila;
                    }
                }
                else
                {
                    contador_leyenda = 0;
                    contador_fila++;

                    char numero = Convert.ToChar(65 + contador_leyenda);

                    letter = new String(new char[] { numero }) + "" + contador_fila;
                }

                contador_leyenda++;
                v_leyenda_x[j] = letter;
            }
            return v_leyenda_x;
        }

        public static String[] CrearArrayNumeros(int v_columna_total)
        {

            v_columna_total++;

            var v_leyenda_x = new string[v_columna_total];
            var contador_leyenda = 0;

            for (int j = 0; j < v_columna_total; j++)
            {
                contador_leyenda++;
                v_leyenda_x[j] = contador_leyenda.ToString();
            }
            return v_leyenda_x;
        }
    }
}