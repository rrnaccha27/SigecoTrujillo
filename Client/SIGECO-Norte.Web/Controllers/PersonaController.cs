using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Core;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEES.Web.Areas.Comision.Services;

namespace SIGEES.Web.Controllers
{
    [RequiresAuthentication]
    public class PersonaController : Controller
    {

        private readonly IPersonaService _service;
        private readonly ICorporacionService _CorporacionService;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly IEventoUsuarioService _EventoUsuarioService;
        private readonly TipoDocumentoService _TipoDocumentoService;
        private readonly IEstadoCivilService _EstadoCivilService;
        private readonly ISexoService _SexoService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }

        public PersonaController()
        {
            _service = new PersonaService();
            _CorporacionService = new CorporacionService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _EventoUsuarioService = new EventoUsuarioService();
            _TipoDocumentoService = new TipoDocumentoService();
            _EstadoCivilService = new EstadoCivilService();
            _SexoService = new SexoService();
        }
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult GetRegistrosJSON()
        {
            string result = this._service.GetAllJson(false);
            return Content(result, "application/json");
        }

        [HttpPost]
        public ActionResult GetRegistro(string id)
        {

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo));
            }

            try
            {
                string jsonContent = this._service.GetSingleJSON(int.Parse(id));
                return Content(jsonContent, "application/json");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
        }

        [HttpPost]
        public ActionResult Registrar(string codigoCorporacion, string codigoTipoDocumento,
            string codigoEstadoCivil, string nombrePersona, string apellidoPaterno, string apellidoMaterno,
            string numeroDocumento, string fechaNacimiento, string codigoSexo, string esVendedor)
        {
            bool estadoEvento = false;
            string codigo = "NULL";

            JObject jo = new JObject();

            persona persona = new persona();

            try
            {
                

                if (string.IsNullOrWhiteSpace(codigoCorporacion))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE CORPORACION");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(nombrePersona))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE EL NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombrePersona.Length > 60)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 60");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(apellidoPaterno))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE APELLIDO PATERNO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (apellidoPaterno.Length > 30)
                {
                    jo.Add("Msg", "APELLIDO PATERNO, NUMERO MAXIMO DE CARACTERES 30");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(apellidoMaterno))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE APELLIDO MATERNO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (apellidoMaterno.Length > 30)
                {
                    jo.Add("Msg", "APELLIDO MATERNO, NUMERO MAXIMO DE CARACTERES 30");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(fechaNacimiento))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE FECHA NACIMIENTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoTipoDocumento))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE TIPO DOCUMENTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(numeroDocumento))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NUMERO DOCUMENTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (numeroDocumento.Length > 30)
                {
                    jo.Add("Msg", "NUMERO DOCUMENTO, NUMERO MAXIMO DE CARACTERES 25");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoEstadoCivil))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE ESTADO CIVIL");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoSexo))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE SEXO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(esVendedor))
                {
                    jo.Add("Msg", "ES VENDEDOR NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                DateTime fechaNac = DateTime.ParseExact(fechaNacimiento, "d/M/yyyy", CultureInfo.InvariantCulture);

                persona.es_vendedor = esVendedor == "true" ? true : false;
                persona.nombre_persona = nombrePersona;
                persona.apellido_paterno = apellidoPaterno;
                persona.apellido_materno = apellidoMaterno;
                persona.numero_documento = numeroDocumento;
                persona.fecha_nacimiento = fechaNac;
                persona.codigo_sexo = codigoSexo;
                persona.codigo_corporacion = int.Parse(codigoCorporacion);
                persona.codigo_tipo_documento = int.Parse(codigoTipoDocumento);
                persona.codigo_estado_civil = int.Parse(codigoEstadoCivil);
                persona.estado_registro = true;
                persona.fecha_registra = DateTime.Now;
                persona.usuario_registra = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.Create(persona);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    codigo = respuesta.IdRegistro;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al registrar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("nombre_persona", persona.nombre_persona));
                    listaBeanAtributo.Add(new BeanAtributo("apellido_paterno", persona.apellido_paterno));
                    listaBeanAtributo.Add(new BeanAtributo("apellido_materno", persona.apellido_materno));
                    listaBeanAtributo.Add(new BeanAtributo("numero_documento", persona.numero_documento));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_sexo", persona.codigo_sexo));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_nacimiento", Fechas.convertDateTimeToString(persona.fecha_nacimiento)));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_corporacion", persona.codigo_corporacion.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_tipo_documento", persona.codigo_tipo_documento.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_estado_civil", persona.codigo_estado_civil.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("es_vendedor", persona.es_vendedor.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", persona.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(persona.fecha_registra)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_registra", persona.usuario_registra));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "persona", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL REGISTRAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "persona", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 1, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(string codigoPersona, string codigoTipoDocumento,
            string codigoEstadoCivil, string nombrePersona, string apellidoPaterno, string apellidoMaterno,
            string numeroDocumento, string fechaNacimiento, string codigoSexo, string esVendedor)
        {
            bool estadoEvento = false;
            persona registro = null;

            JObject jo = new JObject();

            try
            {
                DateTime fechaNac = DateTime.ParseExact(fechaNacimiento, "d/M/yyyy", CultureInfo.InvariantCulture);

                if (string.IsNullOrWhiteSpace(codigoPersona))
                {
                    jo.Add("Msg", "CODIGO REGISTRO NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(nombrePersona))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE EL NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombrePersona.Length > 60)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 60");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(apellidoPaterno))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE APELLIDO PATERNO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (apellidoPaterno.Length > 30)
                {
                    jo.Add("Msg", "APELLIDO PATERNO, NUMERO MAXIMO DE CARACTERES 30");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(apellidoMaterno))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE APELLIDO MATERNO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (apellidoMaterno.Length > 30)
                {
                    jo.Add("Msg", "APELLIDO MATERNO, NUMERO MAXIMO DE CARACTERES 30");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(fechaNacimiento))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE FECHA NACIMIENTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoTipoDocumento))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE TIPO DOCUMENTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(numeroDocumento))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NUMERO DOCUMENTO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (numeroDocumento.Length > 30)
                {
                    jo.Add("Msg", "NUMERO DOCUMENTO, NUMERO MAXIMO DE CARACTERES 25");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoEstadoCivil))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE ESTADO CIVIL");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoSexo))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE SEXO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(esVendedor))
                {
                    jo.Add("Msg", "ES VENDEDOR NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                registro = this._service.GetSingle(int.Parse(codigoPersona));
                registro.es_vendedor = esVendedor == "true" ? true : false;
                registro.nombre_persona = nombrePersona;
                registro.apellido_paterno = apellidoPaterno;
                registro.apellido_materno = apellidoMaterno;
                registro.fecha_nacimiento = fechaNac;
                registro.numero_documento = numeroDocumento;
                registro.codigo_sexo = codigoSexo;
                registro.codigo_tipo_documento = int.Parse(codigoTipoDocumento);
                registro.codigo_estado_civil = int.Parse(codigoEstadoCivil);
                registro.estado_registro = true;
                registro.fecha_modifica = DateTime.Now;
                registro.usuario_modifica = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.Update(registro);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al modificar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("nombre_persona", registro.nombre_persona));
                    listaBeanAtributo.Add(new BeanAtributo("apellido_paterno", registro.apellido_paterno));
                    listaBeanAtributo.Add(new BeanAtributo("apellido_materno", registro.apellido_materno));
                    listaBeanAtributo.Add(new BeanAtributo("numero_documento", registro.numero_documento));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_sexo", registro.codigo_sexo));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_nacimiento", Fechas.convertDateTimeToString(registro.fecha_nacimiento)));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_tipo_documento", registro.codigo_tipo_documento.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_estado_civil", registro.codigo_estado_civil.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("es_vendedor", registro.es_vendedor.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigoPersona, "persona", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigoPersona, "persona", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 2, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        [HttpPost]
        public ActionResult Eliminar(string codigoPersona)
        {
            bool estadoEvento = false;
            persona registro = null;

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigoPersona))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                registro = this._service.GetSingle(int.Parse(codigoPersona));
                registro.estado_registro = false;
                registro.fecha_modifica = DateTime.Now;
                registro.usuario_modifica = beanSesionUsuario.codigoUsuario;
                IResult respuesta = this._service.Update(registro);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al eliminar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigoPersona, "persona", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL ELIMINAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigoPersona, "persona", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 3, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        public ActionResult GetCorporacionJSON()
        {
            string result = this._CorporacionService.GetComboJson(false);
            return Content(result, "application/json");
        }

        public ActionResult GetTipoDocumentoJSON()
        {
            string result = this._TipoDocumentoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetEstadoCivilJSON()
        {
            string result = this._EstadoCivilService.GetComboJson(false);
            return Content(result, "application/json");
        }

        public ActionResult GetSexoJSON()
        {
            string result = this._SexoService.GetComboJson(false);
            return Content(result, "application/json");
        }

    }
}
