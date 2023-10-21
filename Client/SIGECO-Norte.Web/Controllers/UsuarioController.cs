using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.ViewModel;
using SIGEES.Entidades;
using SIGEES.Web.Common;
using SIGEES.Web.Services;
using SIGEES.Web.Models.Bean;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SIGEES.Web.Models;
using SIGEES.Web.Core;
using SIGEES.Web.Utils;

namespace SIGEES.Web.Controllers
{
    [RequiresAuthentication]
    public class UsuarioController : Controller
    {

        private readonly IUsuarioService _service;
        private readonly IPersonaService _PersonaService;
        private readonly IPerfilUsuarioService _PerfilUsuarioService;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly IEventoUsuarioService _EventoUsuarioService;
        private readonly IPermisoEmpresaService _PermisoEmpresaService;
        private readonly IParametroSistemaService _ParametroSistemaService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public UsuarioController()
        {
            _service = new UsuarioService();
            _PersonaService = new PersonaService();
            _PerfilUsuarioService = new PerfilUsuarioService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _EventoUsuarioService = new EventoUsuarioService();
            _PermisoEmpresaService = new PermisoEmpresaService();
            _ParametroSistemaService = new ParametroSistemaService();
        }

        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult GetRegistrosJSON()
        {
            string result = this._service.GetAllJson();
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
                string jsonContent = this._service.GetSingleJSON(id);
                return Content(jsonContent, "application/json");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
        }

        [HttpPost]
        public ActionResult Registrar(string codigo,string clave, string codigoPerfilUsuario, string codigoPersona, string listaCodigoEmpresa)
        {
            bool estadoEvento = false;

            JObject jo = new JObject();

            string[] splitEmpresa = listaCodigoEmpresa.Split(',');

            usuario usuario = new usuario();

            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE CODIGO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(clave))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE CLAVE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (codigo.Length < 5 || codigo.Length > 50)
                {
                    jo.Add("Msg", "CODIGO, NUMERO DE CARACTERES PERMITIDOS 5 al 50");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoPerfilUsuario))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE PERFIL USUARIO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoPersona))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE PERSONA");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                parametro_sistema parametro_sistema = _ParametroSistemaService.GetParametro(Globales.parametroClavePorDefectoNuevoUsuario);
                if (parametro_sistema == null)
                {
                    jo.Add("Msg", "PARAMETRO DE CLAVE POR DEFECTO NO SE ENCUENTRA DISPONIBLE, CODIGO: " +
                        Globales.parametroClavePorDefectoNuevoUsuario);

                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                string claveDefault = parametro_sistema.valor.Trim();

                usuario.codigo_usuario = codigo;
                usuario.codigo_perfil_usuario = int.Parse(codigoPerfilUsuario);
                usuario.codigo_persona = int.Parse(codigoPersona);
                usuario.estado_registro = "A";
                usuario.fecha_registra = DateTime.Now;
                usuario.usuario_registra = beanSesionUsuario.codigoUsuario;

                List<permiso_empresa> listaPermisoEmpresa = new List<permiso_empresa>();
                foreach (string codigoEmpresa in splitEmpresa)
                {
                    if (codigoEmpresa.Length > 0)
                    {
                        permiso_empresa permiso_empresa = new permiso_empresa();
                        permiso_empresa.codigo_empresa = int.Parse(codigoEmpresa);
                        permiso_empresa.codigo_usuario = usuario.codigo_usuario;
                        permiso_empresa.estado_registro = true;
                        permiso_empresa.fecha_registra = DateTime.Now;
                        permiso_empresa.usuario_registra = beanSesionUsuario.codigoUsuario;

                        listaPermisoEmpresa.Add(permiso_empresa);
                    }
                }

                List<clave_usuario> listaClaveUsuario = new List<clave_usuario>();
                clave_usuario clave_usuario = new clave_usuario();

                clave_usuario.codigo_usuario = usuario.codigo_usuario;
                //clave_usuario.clave = CifradoAES.EncryptStringAES(claveDefault, Globales.llaveCifradoClave);

                clave_usuario.clave = CifradoAES.EncryptStringAES(clave, Globales.llaveCifradoClave);
                clave_usuario.estado_registro = true;
                clave_usuario.fecha_registra = DateTime.Now;
                clave_usuario.usuario_registra = beanSesionUsuario.codigoUsuario;
                listaClaveUsuario.Add(clave_usuario);

                usuario.clave_usuario = listaClaveUsuario;
                usuario.permiso_empresa = listaPermisoEmpresa;

                IResult respuesta = this._service.Create(usuario);

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
                    listaBeanAtributo.Add(new BeanAtributo("codigo_perfil_usuario", usuario.codigo_perfil_usuario.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("codigo_persona", usuario.codigo_persona.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", usuario.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(usuario.fecha_registra)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_registra", usuario.usuario_registra));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL REGISTRAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "usuario", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 1, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(string codigo, string clave, string codigoPerfilUsuario, string listaCodigoEmpresa)
        {
            bool estadoEvento = false;
            usuario registro = null;

            string[] splitEmpresa = listaCodigoEmpresa.Split(',');

            JObject jo = new JObject();

            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    jo.Add("Msg", "CODIGO REGISTRO NULO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(clave))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE CLAVE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (string.IsNullOrWhiteSpace(codigoPerfilUsuario))
                {
                    jo.Add("Msg", "POR FAVOR SELECCIONE PERFIL USUARIO");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                registro = this._service.GetSingle(codigo);
                registro.estado_registro = "A";
                registro.codigo_perfil_usuario = int.Parse(codigoPerfilUsuario);
                registro.fecha_modifica = DateTime.Now;
                registro.usuario_modifica = beanSesionUsuario.codigoUsuario;

                List<permiso_empresa> listaPermisoEmpresa = new List<permiso_empresa>();
                foreach (string codigoEmpresa in splitEmpresa)
                {
                    if (codigoEmpresa.Length > 0)
                    {
                        permiso_empresa permiso_empresa = new permiso_empresa();
                        permiso_empresa.codigo_empresa = int.Parse(codigoEmpresa);
                        permiso_empresa.codigo_usuario = registro.codigo_usuario;
                        permiso_empresa.estado_registro = true;
                        permiso_empresa.fecha_registra = DateTime.Now;
                        permiso_empresa.usuario_registra = beanSesionUsuario.codigoUsuario;

                        listaPermisoEmpresa.Add(permiso_empresa);
                    }
                }

                registro.permiso_empresa = listaPermisoEmpresa;

                IResult respuesta = this._service.Update(registro);

                /**/

                clave_usuario clave_usuario = new clave_usuario();
                clave_usuario.codigo_usuario = codigo;
                clave_usuario.clave = CifradoAES.EncryptStringAES(clave, Globales.llaveCifradoClave);
                clave_usuario.estado_registro = true;
                clave_usuario.fecha_registra = DateTime.Now;
                clave_usuario.usuario_registra = beanSesionUsuario.codigoUsuario;
                IResult respuesta_clave = this._service.ResetPassword(clave_usuario);
                /**/


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
                    listaBeanAtributo.Add(new BeanAtributo("codigo_perfil_usuario", registro.codigo_perfil_usuario.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", registro.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(registro.fecha_modifica)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", registro.usuario_modifica));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "usuario", listaBeanAtributo));
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
            usuario registro = null;

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                registro = this._service.GetSingle(codigo);
                registro.estado_registro = "I";
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
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL ELIMINAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "usuario", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 3, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult ResetearClave(string codigo)
        {
            bool estadoEvento = false;
            string codigoClaveUsuario = "NULL";

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "CODIGO REGISTRO NULO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            clave_usuario clave_usuario = new clave_usuario();

            try
            {
                parametro_sistema parametro_sistema = _ParametroSistemaService.GetParametro(Globales.parametroClavePorDefectoNuevoUsuario);
                if (parametro_sistema == null)
                {
                    jo.Add("Msg", "PARAMETRO DE CLAVE POR DEFECTO NO SE ENCUENTRA DISPONIBLE, CODIGO: " +
                        Globales.parametroClavePorDefectoNuevoUsuario);

                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                string claveDefault = parametro_sistema.valor.Trim();

                clave_usuario.codigo_usuario = codigo;
                clave_usuario.clave = CifradoAES.EncryptStringAES(claveDefault, Globales.llaveCifradoClave);
                clave_usuario.estado_registro = true;
                clave_usuario.fecha_registra = DateTime.Now;
                clave_usuario.usuario_registra = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._service.ResetPassword(clave_usuario);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    codigoClaveUsuario = respuesta.IdRegistro;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al restaurar clave");
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
                    listaBeanAtributo.Add(new BeanAtributo("codigo_usuario", clave_usuario.codigo_usuario));
                    listaBeanAtributo.Add(new BeanAtributo("estado_registro", clave_usuario.estado_registro.ToString()));
                    listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(clave_usuario.fecha_registra)));
                    listaBeanAtributo.Add(new BeanAtributo("usuario_registra", clave_usuario.usuario_registra));
                    listaBeanEntidad.Add(new BeanEntidad(codigoClaveUsuario, "clave_usuario", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL MODIFICAR"));
                    listaBeanEntidad.Add(new BeanEntidad(codigoClaveUsuario, "clave_usuario", listaBeanAtributo));
                }

                _EventoUsuarioService.GenerarEvento(beanSesionUsuario, 2, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {

            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
        public ActionResult GetPerfilUsuarioJSON()
        {
            string result = this._PerfilUsuarioService.GetComboJson(false);
            return Content(result, "application/json");
        }

        public ActionResult GetAllPersonaByFiltroJson(string tipo, string valor)
        {
            JObject jo = new JObject();
            if (string.IsNullOrWhiteSpace(tipo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE FILTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            else if (string.IsNullOrWhiteSpace(valor))
            {
                jo.Add("Msg", "POR FAVOR INGRESE VALOR DE FILTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            string result = this._PersonaService.GetAllJsonByFiltro(tipo, valor);
            return Content(result, "application/json");
        }

        public ActionResult GetEmpresaJSON(string codigoUsuario)
        {
            string result = this._PermisoEmpresaService.GetAllEmpresaByUsuarioJson(codigoUsuario);
            return Content(result, "application/json");
        }
    }
}