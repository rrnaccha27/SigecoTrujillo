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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Controllers
{
    [RequiresAuthentication]
    public class ParametroSistemaController : Controller
    {

        private readonly IParametroSistemaService _service;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly IEventoUsuarioService _EventoUsuarioService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ParametroSistemaController()
        {
            _service = new ParametroSistemaService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _EventoUsuarioService = new EventoUsuarioService();
        }
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

            System.Diagnostics.Debug.WriteLine("estadoPermisoTotal: " + bean.estadoPermisoTotal);
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
        public ActionResult Registrar(string codigo, string nombre, string valor, string tokenizar)
        {
            bool estadoEvento = false;

            JObject jo = new JObject();

            parametro_sistema tipo = new parametro_sistema();

            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE CODIGO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(nombre))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombre.Length > 100)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 100");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(valor))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE VALOR");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (valor.Length > 100)
                {
                    jo.Add("Msg", "VALOR, NUMERO MAXIMO DE CARACTERES 100");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(tokenizar))
                {
                    jo.Add("Msg", "TOKENIZAR NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                var parametroSistemaExiste = _service.GetSingle(int.Parse(codigo));

                if (parametroSistemaExiste != null)
                {
                    jo.Add("Msg", "EL CODIGO INGRESADO YA FUE REGISTRADO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                tipo.tokenizar = tokenizar == "true" ? true : false;
                tipo.valor = tipo.tokenizar ? CifradoAES.EncryptStringAES(valor, Globales.llaveCifradoParametro) : valor;

                tipo.codigo_parametro_sistema = int.Parse(codigo);
                tipo.nombre_parametro_sistema = nombre;
                tipo.estado_registro = true;
                tipo.fecha_registra = DateTime.Now;
                tipo.usuario_registra = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.Create(tipo);
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
                    listaBeanAtributo.Add(new BeanAtributo("nombre_parametro_sistema", tipo.nombre_parametro_sistema));
                    listaBeanAtributo.Add(new BeanAtributo("valor", tipo.valor));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", tipo.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(tipo.fecha_registra)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_registra", tipo.usuario_registra));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "parametro_sistema", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL REGISTRAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "parametro_sistema", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 1, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(string codigo, string nombre, string valor, string tokenizar)
        {
            bool estadoEvento = false;
            parametro_sistema registro = null;

            JObject jo = new JObject();

            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    jo.Add("Msg", "CODIGO REGISTRO NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(nombre))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombre.Length > 100)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 100");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(valor))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE VALOR");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (valor.Length > 100)
                {
                    jo.Add("Msg", "VALOR, NUMERO MAXIMO DE CARACTERES 100");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(tokenizar))
                {
                    jo.Add("Msg", "TOKENIZAR NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                registro = this._service.GetSingle(int.Parse(codigo));

                System.Diagnostics.Debug.WriteLine("tokenizar: " + tokenizar);

                registro.tokenizar = tokenizar == "true" ? true : false;
                registro.valor = registro.tokenizar ? CifradoAES.EncryptStringAES(valor, Globales.llaveCifradoParametro) : valor;
                registro.nombre_parametro_sistema = nombre;
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
                    listaBeanAtributo.Add(new BeanAtributo("nombre_parametro_sistema", registro.nombre_parametro_sistema));
                    listaBeanAtributo.Add(new BeanAtributo("valor", registro.valor));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "parametro_sistema", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "parametro_sistema", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 2, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        [HttpPost]
        public ActionResult Eliminar(string codigo)
        {
            bool estadoEvento = false;
            parametro_sistema registro = null;

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                registro = this._service.GetSingle(int.Parse(codigo));
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
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "parametro_sistema", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL ELIMINAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "parametro_sistema", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 3, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

    }
}
