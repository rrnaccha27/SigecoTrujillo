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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

using SIGEES.Entidades;
using SIGEES.Web.Areas.Comision.Services;
using System.Globalization;
using SIGEES.BusinessLogic;
using System.Configuration;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ReglaPagoComisionController : Controller
    {
        //
        private readonly EmpresaSIGECOService _EmpresaSigecoService;
        private readonly CampoSantoSigecoService _CampoSantoSigecoService;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly CanalGrupoService _CanalGrupoService;
        private readonly TipoVentaService _TipoVentaService;
        private readonly TipoPlanillaService _TipoPlanillaService;

        private readonly TipoPagoService _TipoPagoService;
        private readonly TipoArticuloService _TipoArticuloService;
        private readonly TipoComisionService _TipoComisionService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly IEventoUsuarioService _EventoUsuarioService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReglaPagoComisionController()
        {
            _EmpresaSigecoService = new EmpresaSIGECOService();
            _CampoSantoSigecoService = new CampoSantoSigecoService();
            _CanalGrupoService = new CanalGrupoService();
            _TipoVentaService = new TipoVentaService();
            _TipoPagoService = new TipoPagoService();
            _EventoUsuarioService = new EventoUsuarioService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _TipoArticuloService = new TipoArticuloService();
            _TipoPlanillaService = new TipoPlanillaService();
            _TipoComisionService = new TipoComisionService();
        }

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        [HttpPost]
        public ActionResult Listar(regla_pago_comision_search_dto parametros)
        {

            int sede = System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString());
            var query = new List<regla_pago_comision_dto>();
            int total = 0;

            AnularSeleccionados(parametros);
            parametros.codigo_sede = sede;
            try
            {
                query = ReglaComisionBL.Instance.Listar(parametros);
                total = query.Count;
                /*
                query = from order in lst.AsEnumerable()
                        select new
                        {
                            codigo_regla_pago = order.codigo_regla_pago,
                            nombre_regla_pago = order.nombre_regla_pago,
                            nombre_empresa = order.nombre_empresa,
                            nombre_campo_santo = order.nombre_campo_santo,
                            nombre_canal_grupo = order.nombre_canal_grupo,
                            nombre_tipo_venta = order.nombre_tipo_venta,
                            nombre_tipo_pago = order.nombre_tipo_pago,
                            nombre_articulo = order.nombre_articulo,
                            vigencia = order.vigencia,
                            estado_registro_nombre = order.estado_registro_nombre,
                            estado_registro = order.estado_registro,
                            indica_estado=order.estado_registro?1:0
                        };*/
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Registrar(regla_pago_comision_dto parametros)
        {
            int vResultado = 1;
            MensajeDTO v_mensaje;
            long codigo_regla_comision = 0;
            // AnularSeleccionados_old(parametros);
            string vMensaje = string.Empty;
            try
            {
                parametros.codigo_tipo_planilla = parametros.codigo_tipo_planilla == 0 ? null : parametros.codigo_tipo_planilla;
                parametros.codigo_articulo = parametros.codigo_articulo == 0 ? null : parametros.codigo_articulo;
                parametros.codigo_tipo_venta = parametros.codigo_tipo_venta == 0 ? null : parametros.codigo_tipo_venta;
                parametros.codigo_tipo_articulo = parametros.codigo_tipo_articulo == 0 ? null : parametros.codigo_tipo_articulo;
                parametros.codigo_canal_grupo = parametros.codigo_canal_grupo == 0 ? null : parametros.codigo_canal_grupo;

                parametros.tope_minimo_contrato = parametros.tope_minimo_contrato == 0 ? null : parametros.tope_minimo_contrato;
                parametros.tope_unidad = parametros.tope_unidad == 0 ? null : parametros.tope_unidad;
                parametros.meta_general = parametros.meta_general == 0 ? null : parametros.meta_general;


                int sede = System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString());
                parametros.codigo_sede = sede;
                //ValidarRegla(parametros);                                
                parametros.usuario_registra = beanSesionUsuario.codigoUsuario;
                if (parametros.codigo_regla_comision == 0)
                    v_mensaje = ReglaComisionBL.Instance.Insertar(parametros);
                else
                    v_mensaje = ReglaComisionBL.Instance.Actualizar(parametros);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se registr&oacute; satisfactoriamente.";
                codigo_regla_comision = v_mensaje.idRegistro;
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_regla_comision= codigo_regla_comision }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult Eliminar(regla_pago_comision_dto parametros)
        {
            int vResultado = 1;

            string vMensaje = string.Empty;
            try
            {
                parametros.estado_registro = false;

                var v_mensaje = ReglaComisionBL.Instance.Eliminar(parametros);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se desactiv&oacute; satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetCanalGrupoJSON()
        {
            string result = string.Empty;
            try
            {
                result = this._CanalGrupoService.GetCanalAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }

        public ActionResult GetTipoVentaJSON()
        {
            string result = string.Empty;
            try
            {
                result = this._TipoVentaService.GetAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }

        public ActionResult GetTipoPlanillaJSON()
        {
            string result = string.Empty;
            try
            {
                result = this._TipoPlanillaService.GetAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }


        public ActionResult GetTipoArticuloJson()
        {
            string result = string.Empty;
            try
            {
                result = this._TipoArticuloService.GetAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }

        [HttpPost]
        public ActionResult GetTipoComisionJson()
        {
            string result = string.Empty;
            try
            {
                result = this._TipoComisionService.GetAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }
        
        public ActionResult GetArticuloByTipoJson(int tipoArticulo)
        {
            List<JObject> jObjects = new List<JObject>();
            int sede = System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString());
            var lista = new ArticuloBL().ListarBySedeAndTipo(tipoArticulo, sede);

            foreach (var item in lista)
            {
                JObject root = new JObject
                    {
                        {"id", item.codigo_articulo.ToString()},
                        {"text",item.codigo_sku+": "+ item.nombre},
                    };
                jObjects.Add(root);
            }

            return Content(JsonConvert.SerializeObject(jObjects), "application/json");
        }





        private void ValidarRegla(old_regla_pago_comision_dto parametros)
        {
            string mensaje = string.Empty;
            AnularSeleccionados_old(parametros);
            int existeRegla = ReglaPagoComisionBL.Instance.Validar(parametros);

            if (existeRegla > 0)
            {
                mensaje = "Ya existe una regla con criterio de: ";
                mensaje = mensaje + (!parametros.codigo_campo_santo.Equals(null) ? "camposanto, " : "");
                mensaje = mensaje + (!parametros.codigo_empresa.Equals(null) ? "empresa, " : "");
                mensaje = mensaje + (!parametros.codigo_canal_grupo.Equals(null) ? "canal de venta, " : "");
                mensaje = mensaje + (!parametros.codigo_tipo_venta.Equals(null) ? "tipo de venta, " : "");
                mensaje = mensaje + (!parametros.codigo_tipo_pago.Equals(null) ? "tipo de pago, " : "");
                mensaje = mensaje + (!parametros.codigo_articulo.Equals(null) ? "articulo, " : "");
                mensaje = mensaje + (!parametros.evaluar_plan_integral.Equals(null) ? "Evaluar Plan Integral, " : "");
                mensaje = mensaje + (!parametros.evaluar_plan_integral.Equals(null) ? "Evaluar Anexado, " : "");

                mensaje = mensaje + ", en el rango [" + parametros.fecha_inicio_str + " - " + parametros.fecha_fin_str + "]";



                mensaje = mensaje.Substring(0, mensaje.Length - 2) + ".";

                throw new Exception(mensaje);
            }
        }
        private void AnularSeleccionados(regla_pago_comision_search_dto parametros)
        {

            parametros.codigo_sede = (parametros.codigo_sede.Equals(0) ? null : parametros.codigo_sede);
            parametros.codigo_canal_grupo = (parametros.codigo_canal_grupo.Equals(0) ? null : parametros.codigo_canal_grupo);
            parametros.codigo_tipo_venta = (parametros.codigo_tipo_venta.Equals(0) ? null : parametros.codigo_tipo_venta);
            parametros.codigo_tipo_articulo = (parametros.codigo_tipo_articulo.Equals(0) ? null : parametros.codigo_tipo_articulo);
            parametros.codigo_articulo = (parametros.codigo_articulo.Equals(0) ? null : parametros.codigo_articulo);
            parametros.codigo_tipo_planilla = (parametros.codigo_tipo_planilla.Equals(0) ? null : parametros.codigo_tipo_planilla);

        }
        private void AnularSeleccionados_old(old_regla_pago_comision_dto parametros)
        {
            parametros.codigo_campo_santo = (parametros.codigo_campo_santo.Equals(0) ? null : parametros.codigo_campo_santo);
            parametros.codigo_empresa = (parametros.codigo_empresa.Equals(0) ? null : parametros.codigo_empresa);
            parametros.codigo_canal_grupo = (parametros.codigo_canal_grupo.Equals(0) ? null : parametros.codigo_canal_grupo);
            parametros.codigo_tipo_venta = (parametros.codigo_tipo_venta.Equals(0) ? null : parametros.codigo_tipo_venta);
            parametros.codigo_tipo_pago = (parametros.codigo_tipo_pago.Equals(0) ? null : parametros.codigo_tipo_pago);
            parametros.codigo_articulo = (parametros.codigo_articulo.Equals(0) ? null : parametros.codigo_articulo);

        }


        #region POPUP

        [HttpPost]
        public ActionResult GetDetalleReglaComisionByIdRegla(regla_pago_comision_search_dto parametros)
        {
            var list = new List<detalle_regla_comision_dto>();
            string mensaje = string.Empty;
            if (!parametros.codigo_regla_comision.HasValue || parametros.codigo_regla_comision <= 0)
                return Json(list, JsonRequestBehavior.AllowGet);
            //return Json(new { Msg = mensaje, registro = regla_pago_comision_dto, total = 0 }, JsonRequestBehavior.AllowGet);



            try
            {
                list = DetalleReglaComisionBL.Instance.GetListByIdRegla(parametros.codigo_regla_comision.Value);

            }
            catch (Exception ex)
            {
                ex.ToString();
                mensaje = ex.Message;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
            //return Json(new { Msg = mensaje, registro = regla_pago_comision_dto, total = regla_pago_comision_dto.Count }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult GetMetaReglaComisionByIdRegla(regla_pago_comision_search_dto parametros)
        {
            var list = new List<meta_regla_comision_dto>();
            string mensaje = string.Empty;

            if (!parametros.codigo_regla_comision.HasValue || parametros.codigo_regla_comision <= 0)
                return Json(list, JsonRequestBehavior.AllowGet);

            try
            {
                list = MetaReglaComisionBL.Instance.GetListByIdRegla(parametros.codigo_regla_comision.Value);
            }
            catch (Exception ex)
            {
                ex.ToString();
                mensaje = ex.Message;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
            //return Json(new { Msg = mensaje, registro = list, total = list.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegistrarMetaReglaComision(meta_regla_comision_dto parametros)
        {
            int vResultado = 1;
            MensajeDTO v_mensaje;
            // AnularSeleccionados_old(parametros);
            string vMensaje = string.Empty;
            try
            {

                parametros.usuario_registra = beanSesionUsuario.codigoUsuario;
                if (parametros.codigo_meta_regla_comision <= 0)
                    v_mensaje = MetaReglaComisionBL.Instance.Insertar(parametros);
                else
                    v_mensaje = MetaReglaComisionBL.Instance.Actualizar(parametros);

                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se registr&oacute; satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EliminarMetaReglaComision(meta_regla_comision_dto parametros)
        {
            int vResultado = 1;
            MensajeDTO v_mensaje;
            // AnularSeleccionados_old(parametros);
            string vMensaje = string.Empty;
            try
            {
                parametros.estado_registro = false;
                parametros.usuario_registra = beanSesionUsuario.codigoUsuario;

                v_mensaje = MetaReglaComisionBL.Instance.Eliminar(parametros);


                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se registr&oacute; satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion
        /*
        [HttpPost]
        public ActionResult GetRegistro(int id)
        {
            regla_pago_comision_dto regla_pago_comision_dto = new regla_pago_comision_dto();
            bool existe = false;
            try
            {
        
                if (regla_pago_comision_dto != null)
                {
                    existe = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new { existe = existe, registro = regla_pago_comision_dto }, JsonRequestBehavior.AllowGet);
        }*/

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Registrar(int p_codigo_regla_comision, int tipo = 0)
        {
            regla_pago_comision_dto item = new regla_pago_comision_dto();
            try
            {
                if (p_codigo_regla_comision > 0)
                {
                    item = ReglaComisionBL.Instance.BuscarById(p_codigo_regla_comision);
                }
                else
                {
                    item.codigo_articulo = p_codigo_regla_comision;
                    item.estado_registro = true;
                }
                item.tipoOperacion = tipo;
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }
        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Detalle_regla(int p_codigo_detalle_regla_comision, int tipo = 0)
        {
            detalle_regla_comision_dto item = new detalle_regla_comision_dto();
            try
            {
                if (p_codigo_detalle_regla_comision > 0)
                {
                    item = DetalleReglaComisionBL.Instance.GetById(p_codigo_detalle_regla_comision);
                }
                else
                {
                    item.codigo_detalle_regla_comision = p_codigo_detalle_regla_comision;
                    item.estado_registro = true;
                }
                
                if (item == null) 
                    item = new detalle_regla_comision_dto();                                    
                else
                    item.tipoOperacion = tipo;


            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }
        [HttpPost]
        public ActionResult RegistrarDetalleReglaComision(detalle_regla_comision_dto parametros)
        {
            int vResultado = 1;
            MensajeDTO v_mensaje;
            // AnularSeleccionados_old(parametros);
            string vMensaje = string.Empty;
            try
            {

                parametros.usuario_registra = beanSesionUsuario.codigoUsuario;
                if (parametros.codigo_detalle_regla_comision <= 0)
                    v_mensaje = DetalleReglaComisionBL.Instance.Insertar(parametros);
                else
                    v_mensaje = DetalleReglaComisionBL.Instance.Actualizar(parametros);

                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se registr&oacute; satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
