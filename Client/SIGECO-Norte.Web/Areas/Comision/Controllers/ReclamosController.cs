using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGEES.BusinessLogic;
using SIGEES.Entidades;

using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.Areas.Comision.Utils;

using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SIGEES.Web.Helpers;
using SIGEES.Web.MemberShip.Filters;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ReclamosController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly EstadoReclamoService _EstadoReclamoService;
        private readonly EstadoResultadoService _EstadoResultadoService;
        private readonly reclamoBL _reclamoBL;
        private readonly ArticuloBL _ArticuloBL;
        private readonly EmpresaSIGECOService _EmpresaSIGECOService;
        private readonly PersonalService _personalService;
        private readonly ArticuloService _articuloService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReclamosController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _EstadoReclamoService = new EstadoReclamoService();
            _EstadoResultadoService = new EstadoResultadoService();
            _reclamoBL = new reclamoBL();
            _ArticuloBL = new ArticuloBL();
            _EmpresaSIGECOService = new EmpresaSIGECOService();
            _personalService = new PersonalService();
            _articuloService = new ArticuloService();
        }
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult GetAllEstadoReclamoJson()
        {
            string result = this._EstadoReclamoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetAllEstadoResultadoJson()
        {
            string result = this._EstadoResultadoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetAllEmpresasJson()
        {
            string result = this._EmpresaSIGECOService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetPersonalByNombreJson(string texto)
        {
            List<reclamo_personal_listado_dto> lista = new List<reclamo_personal_listado_dto>();

            if (texto != "-1")
            {
                lista =  new PersonalBL().ListarParaReclamos(beanSesionUsuario.codigoUsuario);
            }

            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetArticulosByNombreJson(string texto)
        {
            string result = _articuloService.GetAllArticulobyNombreJson(texto);
            return Content(result, "application/json");
        }

        [RequiresAuthentication]
        public ActionResult GetAllJson(string nro_contrato, string personal_ventas, string codigo_estado, string codigo_perfil)
        {
            reclamo_busqueda_dto busqueda = new reclamo_busqueda_dto();

            busqueda.nro_contrato = nro_contrato;
            busqueda.personal_ventas = personal_ventas;
            busqueda.codigo_estado = Convert.ToInt32(codigo_estado);
            busqueda.codigo_perfil = Convert.ToInt32(codigo_perfil);
            busqueda.usuario = beanSesionUsuario.codigoUsuario;

            var lista = _reclamoBL.Listar(busqueda);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult ModificarNotificacionPendientes()
        {
            int result = 0;

            try
            {
                result = new reclamoBL().ObtenerPendientes(beanSesionUsuario.codigoUsuario);
            }
            finally {

            }
            return Json(new { cantidad = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Registrar(reclamo_dto v_entidad)
        {
            JObject jo = new JObject();
            reclamo_dto oBE = new reclamo_dto();
            bool esNuevo = v_entidad.codigo_reclamo == -1 ? true : false;
            string[] respuesta = null;

            try
            {
                if (string.IsNullOrEmpty(v_entidad.NroContrato))
                {
                    jo.Add("Msg", "Nro Contrato, Campo requerido");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                v_entidad.codigo_personal = v_entidad.codigo_personal;
                v_entidad.NroContrato = string.IsNullOrEmpty(v_entidad.NroContrato) ? "" : v_entidad.NroContrato;
                v_entidad.codigo_articulo = v_entidad.codigo_articulo;
                v_entidad.codigo_empresa = v_entidad.codigo_empresa;
                v_entidad.Cuota = v_entidad.Cuota;
                v_entidad.Importe = v_entidad.Importe;
                v_entidad.codigo_estado_reclamo = 1;
                v_entidad.codigo_estado_resultado = 0;
                v_entidad.Observacion = string.IsNullOrEmpty(v_entidad.Observacion) ? "" : v_entidad.Observacion;
                v_entidad.Respuesta = "";
                v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                v_entidad.fecha_registra = DateTime.Now;
                respuesta = _reclamoBL.Registrar(v_entidad).Split('|');
                if (respuesta.Length == 1)
                {
                    jo.Add("Tipo", "ALERT");
                    jo.Add("Msg", respuesta[0]);
                }
                else if (respuesta.Length == 2)
                {
                    jo.Add("Tipo", respuesta[0]);
                    jo.Add("Msg", respuesta[1]);
                }
                else
                {
                    jo.Add("Tipo", respuesta[0]);
                    jo.Add("Msg", respuesta[1]);
                }
            }
            catch (Exception ex)
            {
                jo.Add("Tipo", "ERROR");
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Modificar(reclamo_dto v_entidad)
        {
            JObject jo = new JObject();
            reclamo_dto oBE = new reclamo_dto();
            bool esNuevo = v_entidad.codigo_reclamo == -1 ? true : false;
            int respuesta = -1;

            try
            {
                if (esNuevo)
                {
                    jo.Add("Msg", "Seleccione un registro");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                if (string.IsNullOrEmpty(v_entidad.NroContrato))
                {
                    jo.Add("Msg", "Nro Contrato, Campo requerido");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                v_entidad.codigo_reclamo = v_entidad.codigo_reclamo;
                v_entidad.codigo_personal = v_entidad.codigo_personal;
                v_entidad.NroContrato = string.IsNullOrEmpty(v_entidad.NroContrato) ? "" : v_entidad.NroContrato;

                v_entidad.codigo_articulo = v_entidad.codigo_articulo;
                v_entidad.codigo_empresa = v_entidad.codigo_empresa;
                v_entidad.Cuota = v_entidad.Cuota;
                v_entidad.Importe = v_entidad.Importe;

                v_entidad.atencion_codigo_articulo = v_entidad.atencion_codigo_articulo;
                v_entidad.atencion_codigo_empresa = v_entidad.atencion_codigo_empresa;
                v_entidad.atencion_Cuota = v_entidad.atencion_Cuota;
                v_entidad.atencion_Importe = v_entidad.atencion_Importe;

                v_entidad.codigo_estado_reclamo = Convert.ToInt32(EstadoReclamo.atendido);         //AppSettings.ReclamoCodResultadoAtendido();             //2: Atendiso
                v_entidad.codigo_estado_resultado = v_entidad.codigo_estado_resultado;
                v_entidad.Observacion = string.IsNullOrEmpty(v_entidad.Observacion) ? "" : v_entidad.Observacion;
                v_entidad.Respuesta = string.IsNullOrEmpty(v_entidad.Respuesta) ? "" : v_entidad.Respuesta;
                v_entidad.usuario_modifica = beanSesionUsuario.codigoUsuario;
                v_entidad.fecha_modifica = DateTime.Now;

                respuesta = _reclamoBL.Actualizar(v_entidad);

                if (respuesta > 0)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "NO SE PUDO GUARDAR EL REGISTRO.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Eliminar(string codigo)
        {
            JObject jo = new JObject();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                int codigo_reclamo = Convert.ToInt32(codigo);
                int respuesta = _reclamoBL.Eliminar(codigo_reclamo);
                if (respuesta > 0)
                {
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
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult GetRegistro(string codigo)
        {
            //_personalBL oBL = new _personalBL();
            reclamo_dto oBE = new reclamo_dto();
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "Codigo Vacio");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            int ID;
            if (!int.TryParse(codigo, out ID))
            {
                jo.Add("Msg", "Error al parsear codigo");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                int codigo_reclamo = Convert.ToInt32(codigo);
                oBE = _reclamoBL.GetReg(codigo_reclamo);
            }
            catch (Exception ex)
            {
            }
            return Json(oBE, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ValidarRegistro(reclamo_dto v_entidad)
        {
            JObject jo = new JObject();
            reclamo_dto oBE = new reclamo_dto();
            bool esNuevo = v_entidad.codigo_reclamo == -1 ? true : false;
            string[] respuesta = null;

            try
            {
                if (string.IsNullOrEmpty(v_entidad.NroContrato))
                {
                    jo.Add("Msg", "Nro Contrato, Campo requerido");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                v_entidad.codigo_personal = v_entidad.codigo_personal;
                v_entidad.NroContrato = string.IsNullOrEmpty(v_entidad.NroContrato) ? "" : v_entidad.NroContrato;
                v_entidad.codigo_articulo = v_entidad.codigo_articulo;
                v_entidad.codigo_empresa = v_entidad.codigo_empresa;
                v_entidad.Cuota = v_entidad.Cuota;
                v_entidad.Importe = v_entidad.Importe;
                respuesta = _reclamoBL.ValidarRegistro(v_entidad).Split('|');
                if (respuesta.Length == 1)
                {
                    jo.Add("Tipo", "ALERT");
                    jo.Add("Msg", respuesta[0]);
                }
                else if (respuesta.Length == 2)
                {
                    jo.Add("Tipo", respuesta[0]);
                    jo.Add("Msg", respuesta[1]);
                }
                else
                {
                    jo.Add("Tipo", respuesta[0]);
                    jo.Add("Msg", respuesta[1]);
                }
            }
            catch (Exception ex)
            {
                jo.Add("Tipo", "ERROR");
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult ValidarExisteContratoJson(string empresa, string contrato, string personal)
        {
            JObject jo = new JObject();
            try
            {
                if (string.IsNullOrEmpty(empresa))
                {
                    throw new Exception("La empresa es un valor requerido.");
                }
                
                if (string.IsNullOrEmpty(contrato))
                {
                    throw new Exception("El nro. contrato es un valor requerido.");
                }

                if (string.IsNullOrEmpty(personal))
                {
                    throw new Exception("El vendedor es un valor requerido.");
                }


                int respuesta = _reclamoBL.ValidarContrato(Convert.ToInt32(empresa), contrato, Convert.ToInt32(personal));
                if (respuesta > 0)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "El contrato ingresado no se encontró, de acuerdo a los filtros establecidos.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
        [HttpPost]
        public ActionResult ValidarExistePagoJson(reclamo_dto v_entidad)
        {
            JObject jo = new JObject();
            try
            {
                //if (v_entidad.codigo_personal == null)
                //{
                //    codigo_personal = 0;
                //}
                //if (codigo_articulo == null)
                //{
                //    codigo_articulo = 0;
                //}
                //if (nro_cuota == null)
                //{
                //    nro_cuota = 0;
                //}
                //if (importe == null)
                //{
                //    importe = "0";
                //}
                //if (string.IsNullOrEmpty(nro_contrato))
                //{
                //    nro_contrato = string.Empty;
                //}
                //if (string.IsNullOrEmpty(codigo_empresa))
                //{
                //    codigo_empresa = string.Empty;
                //}
                string respuesta = _reclamoBL.ValidarPago(v_entidad);
                jo.Add("Msg", respuesta);

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
        public ActionResult GetArticulosByFilterJson(string empresa, string contrato, string texto)
        {
            if (string.IsNullOrEmpty(empresa))
            {
                empresa = string.Empty;
            }
            if (string.IsNullOrEmpty(contrato))
            {
                contrato = string.Empty;
            }

            var lista = _ArticuloBL.ListarByFiltro(empresa, contrato, texto);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
        public ActionResult GetEstadoContrato(string codigo_empresa, string nro_contrato)
        {
            reclamo_estado_contrato_dto estado_contrato = new reclamo_estado_contrato_dto();
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo_empresa) || string.IsNullOrWhiteSpace(codigo_empresa)) 
            {
                jo.Add("Msg", "Error al procesar información.");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                estado_contrato = _reclamoBL.EstadoContrato(Convert.ToInt32(codigo_empresa), nro_contrato);
                return Content(JsonConvert.SerializeObject(estado_contrato), "application/json");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
        }

        [HttpPost]
        public ActionResult AtenderContratoMigrado(string codigo_reclamo, string respuesta_atencion)
        {
            JObject jo = new JObject();
            reclamo_dto reclamo = new reclamo_dto();
            bool respuesta = false;

            if (string.IsNullOrWhiteSpace(codigo_reclamo))
            {
                jo.Add("Msg", "ERROR CON CODIGO DE RECLAMO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                reclamo.codigo_reclamo = Convert.ToInt32(codigo_reclamo);
                reclamo.Respuesta = respuesta_atencion;
                reclamo.usuario_modifica = beanSesionUsuario.codigoUsuario.ToString();
                
                respuesta = _reclamoBL.AtenderContratoMigrado(reclamo);
                
                if (respuesta)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al atender el reclamo.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult AtenderN1(reclamo_atencion_n1_dto v_entidad)
        {
            JObject jo = new JObject();
            reclamo_dto oBE = new reclamo_dto();
            int respuesta = -1;

            try
            {

                v_entidad.usuario = beanSesionUsuario.codigoUsuario;

                respuesta = _reclamoBL.AtenderN1(v_entidad);

                if (respuesta > 0)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Fail.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


    }
}
