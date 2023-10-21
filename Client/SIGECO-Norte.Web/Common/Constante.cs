using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Common
{
    public static class Constante
    {
        public class proceso_conversion_inicio
        {
            public const int flujo_normal = 1;
            public const int flujo_alterno = 2;
            public const int flujo_exhumacion = 3;
        }
        public class estado_conversion
        {
            public const int inicio = -1;
            public const int InicioConversion = 1;
            public const int exhumacion = 2;
            public const int encofrado = 3;
            public const int inhumado = 4;
            public const int finalizado = 5;
            public const int Convertido = 6;


           public const int ExhumacionEncofrado = 7;
        }

        public class estado_exhumacion
        {
            public const int planificacion = 1;
            public const int proceso = 2;
            public const int exhumado = 3;
        }

        public class tipo_tabla
        {
            public const int tabla_reserva = 1;
            public const int tabla_venta = 2;

        }

        public class tipo_espacio
        {
            
            public const int sin_piso = 1;
            public const int lote = 2;
            public const int lote_cinerario = 3;
            public const int con_pisos= 4;
            public const int columbario = 5;
            public const int cinerario = 6;
            public const int elite = 7;
            
        }
        public class estado_espacio
        {
            public const int libre = 1;
            public const int reservado = 2;
            public const int vendido = 3;
            public const int ocupado = 4;
            public const int anulado = 5;
            public const int conversion = 6;
        }
        public class estado_tabla_movimeinto
        {
            public const int reservado = 1;
            public const int reserva_anulada = 2;
            public const int reserva_renovada = 3;

            public const int vendido = 4;
            public const int venta_anulada = 5;

            public const int venta_actualizada = 6;
            public const int reserva_actualizada = 7;
        }
        public class estado_fallecido
        {
            public const int registrado = 1;
            public const int inhumado = 2;
            public const int exhumado = 3;
        }
        public class tipo_fallecido
        {
            public const int normal = 1;
            public const int incinerado = 2;

        }

        public class tipo_observacion
        {
            public const int anular_reserva = 1;
            public const int anular_venta = 2;

            public const int autorizar_reserva = 3;
            public const int autorizar_venta = 4;

            public const int autorizar_renovar_reserva = 5;
            public const int autorizar_actualizar_reserva = 6;

            public const int autorizar_cambio_vendedor = 7;

        }

        public class session_name
        {
            public const string sesionUsuario = "usuario";
            public const string sesionListaRutaMenu = "listaRutaMenu";
            public const string sesionMenuPerfil = "menuPerfil";

            public const string sesionListaMenu = "listaMenu";

            public const string sesionMenuPermiso = "menuPermiso";
        }

        /*
        public class operacion
        {
            public const int uno = 1;
            public const int dos = 2;
            public const int tres = 3;
            public const int cuatro = 4;

            
            public const int actualizar = 1;
            public const int anular = 2;
            
        }*/



        public class calcular
        {
            /****************************/
            public const int calcular_espacio_disponible = 1;
            public const int no_calcular = 2;


        }

        public class estado_lapida
        {
            /****************************/
            public const int apertura = 1;
            public const int reapertura = 2;
        }

        public class operacion_tabla
        {
            /****************************/
            public const int insertar = 1;
            public const int actualizar = 2;
            public const int eliminar = 3;
            public const int anular = 4;
            public const int renovar = 5;

        }
        public class IndicaField
        {

            public const int field_fecha = 1;
            public const int field_nro_orden = 2;

        }


        public static string obtenerTituloConversionPorVenta(int pCodigoEstadoConversion)
        {
            string v_field = "";
            switch (pCodigoEstadoConversion)
            {
                case estado_conversion.encofrado:
                    v_field = "PROGRAMAR ENCOFRADO";
                    break;
                case estado_conversion.finalizado:
                    v_field = "FINALIZAR CONVERSIÓN";
                    break;
                default:

                    break;
            }
            return v_field;
        }

        public static string obtenerTitulo(int pCodigoEstadoConversion)
        {
            string v_field = "";
            switch (pCodigoEstadoConversion)
            {
                case estado_conversion.InicioConversion:
                    v_field = "DATOS PARA LA CONVERSIÓN";
                    break;
                case estado_conversion.exhumacion:
                    v_field = "PROGRAMAR EXHUMACIÓN Y ENCOFRADO";
                    break;
                //case estado_conversion.exhumacion:
                //    v_field = "DATOS PARA EXHUMACIÓN";
                //    break;
                case estado_conversion.encofrado:
                    v_field = "PROGRAMAR ENCOFRADO";
                    break;
                case estado_conversion.inhumado:
                    v_field = "PROGRAMAR INHUMACIÓN";
                    break;
                case estado_conversion.finalizado:
                    v_field = "FINALIZAR CONVERSIÓN";
                    break;
                default:

                    break;
            }
            return v_field;
        }

        public static string obtenerField(int pCodigoEstadoConversion, int pIndicaField)
        {
            string v_field = "";

            switch (pCodigoEstadoConversion)
            {
                case estado_conversion.InicioConversion:

                    switch (pIndicaField)
                    {
                        case IndicaField.field_fecha:
                            v_field = "Fecha inicio conversión";
                            break;
                        case IndicaField.field_nro_orden:
                            v_field = "N° informe";
                            break;
                        default:
                            break;
                    }

                    break;
                case estado_conversion.ExhumacionEncofrado:

                    switch (pIndicaField)
                    {
                        case IndicaField.field_fecha:
                            v_field = "Fecha inicio exhumación";
                            break;
                        case IndicaField.field_nro_orden:
                            v_field = "N° informe";
                            break;
                        default:
                            break;
                    }

                    break;
                case estado_conversion.exhumacion:

                    switch (pIndicaField)
                    {
                        case IndicaField.field_fecha:
                            v_field = "Fecha exhumación";
                            break;
                        case IndicaField.field_nro_orden:
                            v_field = "N° informe";
                            break;
                        default:
                            break;
                    }

                    break;
                case estado_conversion.encofrado:
                    switch (pIndicaField)
                    {
                        case IndicaField.field_fecha:
                            v_field = "Fecha inicio encofrado";
                            break;
                        case IndicaField.field_nro_orden:
                            v_field = "N° orden";
                            break;
                        default:
                            break;
                    }
                    break;
                case estado_conversion.inhumado:
                    switch (pIndicaField)
                    {
                        case IndicaField.field_fecha:
                            v_field = "Fecha inhumación";
                            break;
                        case IndicaField.field_nro_orden:
                            v_field = "N° ordern";
                            break;
                        default:
                            break;
                    }
                    break;
                case estado_conversion.finalizado:
                    switch (pIndicaField)
                    {
                        case IndicaField.field_fecha:
                            v_field = "Fecha fin de ejecución";
                            break;
                        case IndicaField.field_nro_orden:
                            v_field = "N° ordern";
                            break;
                        default:
                            break;
                    }
                    break;
                default:

                    break;
            }




            return v_field;
        }

        public static object obtenerValorInt(int? p)
        {
            if (p == 0)
            {
                return null;
            }
            else
            {
                return p;
            }

        }

        public static object obtenerValorString(string p)
        {
            if (p == null)
            {
                return "";
            }
            else
            {
                return p.Trim();
            }

        }




    }
}