using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SIGEES.Web.WS
{
    /// <summary>
    /// Summary description for wsSIGEES
    /// </summary>
    [WebService(Namespace = "http://jardines.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wsSIGEES : System.Web.Services.WebService
    {
        /*
        [WebMethod]
        public string HelloWorld()
        {
            IUsuarioService _usuarioService = new UsuarioService();

            return _usuarioService.GetAllJson();
        }*/

        private string codigoUsuario = "root";

        [WebMethod]
        public string RegistrarVenta(BeanRegistrarVenta beanRegistrarVenta)
        {
            if (string.IsNullOrWhiteSpace(beanRegistrarVenta.codigoEspacio))
            {
                return "Codigo de espacio es requerido";
            }
            else if (string.IsNullOrWhiteSpace(beanRegistrarVenta.numeroContrato))
            {
                return "Numero de contrato es requerido";
            }
            else if (string.IsNullOrWhiteSpace(beanRegistrarVenta.fechaVenta))
            {
                return "Fecha de venta es requerido";
            }
            else if (string.IsNullOrWhiteSpace(beanRegistrarVenta.apellidoPaternoCliente))
            {
                return "Apellido Paterno del cliente es requerido";
            }
            else if (string.IsNullOrWhiteSpace(beanRegistrarVenta.nombreCliente))
            {
                return "Nombre del cliente es requerido";
            }

            informacion_venta_dto venta_deto = new informacion_venta_dto();

            String[] respuestaReserva = ReservaSelBL.Instance.BuscarReservaActivaById(beanRegistrarVenta.codigoEspacio);
            if (respuestaReserva == null)
            {
                return "No se encontro reserva activa para el codigo de espacio ingresado";
            }

            string codigo_reserva = respuestaReserva[0];
            string codigo_persona_vendedor = respuestaReserva[1];
            string codigo_venta = respuestaReserva[2];

            if (!string.IsNullOrWhiteSpace(codigo_venta))
            {
                return "El espacio ya tiene registrada una venta";
            }

            DateTime fechaVenta;

            try
            {
                fechaVenta = DateTime.ParseExact(beanRegistrarVenta.fechaVenta, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                return "Formato de fecha incorrecto, ingrese en este formato (yyyy-MM-dd)";
            }

            int codigoCatalogoBienContrato;
            int codigoCatalogoBienEspacio;
            int codigoCatacteristicaLote;

            if (!Int32.TryParse(beanRegistrarVenta.codigoCatalogoBienContrato, out codigoCatalogoBienContrato))
            {
                return "Formato de codigo catalogo bien contrato incorrecto, solo acepto números";
            }

            if (!Int32.TryParse(beanRegistrarVenta.codigoCatalogoBienEspacio, out codigoCatalogoBienEspacio))
            {
                return "Formato de codigo catalogo bien espacio incorrecto, solo acepto números";
            }

            if (!Int32.TryParse(beanRegistrarVenta.codigoCatacteristicaLote, out codigoCatacteristicaLote))
            {
                return "Formato de codigo característica lote incorrecto, solo acepto números";
            }

            venta_deto.codigo_persona_vendedor = int.Parse(codigo_persona_vendedor);
            venta_deto.codigo_reserva = int.Parse(codigo_reserva);
            venta_deto.usuario_registra = codigoUsuario;
            venta_deto.numero_contrato = beanRegistrarVenta.numeroContrato;
            venta_deto.codigo_espacio = beanRegistrarVenta.codigoEspacio;
            venta_deto.observacion = beanRegistrarVenta.observacion;
            venta_deto.fecha_venta = fechaVenta;
            venta_deto.apellido_paterno_cliente = beanRegistrarVenta.apellidoPaternoCliente;
            venta_deto.apellido_materno_cliente = string.IsNullOrWhiteSpace(beanRegistrarVenta.apellidoMaternoCliente) ? null : beanRegistrarVenta.apellidoMaternoCliente;
            venta_deto.nombre_persona_cliente = beanRegistrarVenta.nombreCliente;
            venta_deto.codigo_catalogo_bien_contrato = codigoCatalogoBienContrato;
            venta_deto.codigo_catalogo_bien_espacio = codigoCatalogoBienEspacio;
            venta_deto.codigo_caracteristica_lote = codigoCatacteristicaLote;

            venta_deto.det_estado_espacio.codigo_estado_espacio = Common.Constante.estado_espacio.vendido;
            venta_deto.det_estado_espacio.usuario_registro = codigoUsuario;
            venta_deto.det_estado_espacio.fecha_registro = DateTime.Now;
            venta_deto.det_estado_espacio.estado_registro = true;
            venta_deto.det_estado_espacio.codigo_espacio = beanRegistrarVenta.codigoEspacio;

            var v_mensaje = VentaBL.Instance.InsertarWS(venta_deto);

            return v_mensaje.mensaje;
        }

        [WebMethod]
        public string AnularVenta(BeanAnularVenta beanAnularVenta)
        {
            if (string.IsNullOrWhiteSpace(beanAnularVenta.codigoEspacio))
            {
                return "Codigo de espacio es requerido";
            }

            String[] respuestaReserva = ReservaSelBL.Instance.BuscarReservaActivaById(beanAnularVenta.codigoEspacio);
            if (respuestaReserva == null)
            {
                return "El espacio no tiene una venta activa para anular";
            }

            //string codigo_reserva = respuestaReserva[0];
            //string codigo_persona_vendedor = respuestaReserva[1];

            string codigo_venta = respuestaReserva[2];

            if (string.IsNullOrWhiteSpace(codigo_venta))
            {
                return "El espacio no tiene una venta activa para anular";
            }

            var v_mensaje = VentaBL.Instance.AnularWS(beanAnularVenta.codigoEspacio, beanAnularVenta.observacionAnulacion, codigoUsuario);

            return v_mensaje.mensaje;
        }


    }
}
