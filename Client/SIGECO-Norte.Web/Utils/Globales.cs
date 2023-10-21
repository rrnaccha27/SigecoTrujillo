using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Utils
{
    public class Globales
    {
        public static int parametroModoAutenticacion = 1;
        public static int parametroVencimientoReserva = 2;
        public static int parametroClavePorDefectoNuevoUsuario = 3;

        public static int parametroCantidadReservaPorVendedor =4;
        public static int parametroCodigoAutorizacionVenta = 5;

        public static int parametroMensajeCantidadMaximaReserva = 6;
        public static int parametroAnhoMinimoConversion = 7;

        public static int parametroPerfilUsuariosServidorDominio = 8;

        public static string llaveCifradoClave = "SIGEES_JARDINES_PASSWORD_@1234567";
        public static string llaveCifradoParametro = "SIGEES_JARDINES_PARAMETER_@1234567";

        public static int estadoLapidaPorConfeccionar = 1;
        public static int estadoLapidaEnConfeccion = 2;
        public static int estadoLapidaConfeccionada = 3;
        public static int estadoLapidaColocada = 4;

    }
}