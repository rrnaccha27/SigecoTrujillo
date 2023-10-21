using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    [RequiresAuthentication]
    public class ReglaTipoPlanillaController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly EmpresaSIGECOService _empresaService;
        private readonly TipoVentaService _tipoVentaService;
        private readonly TipoPlanillaService _tipoPlanillaService;
        private readonly CanalGrupoService _canalService;
        private readonly CampoSantoSigecoService _campoSantoService;

        public ReglaTipoPlanillaController() {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _tipoPlanillaService = new TipoPlanillaService();
            _empresaService = new EmpresaSIGECOService();
            _tipoVentaService = new TipoVentaService();
            _canalService = new CanalGrupoService();
            _campoSantoService = new CampoSantoSigecoService();
        }
        
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            }
            catch (Exception ex)
            {

                string mens = ex.Message;
            }

            return View(bean);
        }


        public ActionResult GetAllJson()
        {
            int v_total = 0;
            var v_resultado = new List<grilla_regla_tipo_planilla_dto>();
            try
            {

                v_resultado = ReglaTipoPlanillaBL.Instance.Listar();
                v_total = v_resultado.Count();
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.ToString();
            }

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = v_resultado }), "application/json");

        }

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Nuevo(int p_codigo_regla_tipo_planilla)
        {
            regla_tipo_planilla_dto item = new regla_tipo_planilla_dto();
            try
            {
                if (p_codigo_regla_tipo_planilla > 0)
                    item = ReglaTipoPlanillaBL.Instance.BuscarById(p_codigo_regla_tipo_planilla);
                else
                {   
                    item.estado_registro = true;
                    item.fecha_registra = DateTime.Now;
                }

            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }
            return PartialView(item);
        }


        #region SECCION JSON COMBOS 
        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._tipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetEmpresaJson()
        {
            string result = this._empresaService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetCampoSantoJson()
        {
            string result = this._campoSantoService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetTipoVentaJson()
        {
            string result = this._tipoVentaService.GetAllComboAbreaviaturaJson();
            return Content(result, "application/json");
        }      

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetTipoReporteJson() {
            return Content(TipoReporteBL.Instance.ComboJson(), "application/json");
        }
        #endregion

        #region VALIDAR DUPLICIDAD
        public ActionResult ValidarDuplicidadRegla(grilla_detalle_regla_tipo_planilla_dto detalle_regla, List<grilla_detalle_regla_tipo_planilla_dto> lista_detalle_regla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            try
            {

                if (detalle_regla.codigo_canal <= 0)
                {
                    throw new Exception("Canal es campo obligatorio, vuelva intentar.");
                }
                if (string.IsNullOrWhiteSpace(detalle_regla.codigo_empresa))
                {
                    throw new Exception("Empresa es campo obligatorio, vuelva intentar.");
                }
                if (string.IsNullOrWhiteSpace(detalle_regla.codigo_tipo_venta))
                {
                    throw new Exception("Tipo Venta es campo obligatorio, vuelva intentar.");
                }
                if (string.IsNullOrWhiteSpace(detalle_regla.codigo_campo_santo))
                {
                    throw new Exception("Camposanto es campo obligatorio, vuelva intentar.");
                }



                if (lista_detalle_regla == null)
                {
                    lista_detalle_regla = new List<grilla_detalle_regla_tipo_planilla_dto>();
                }
                foreach (var item in lista_detalle_regla)
                {
                    item.codigo_campo_santo =string.Join(",",item.codigo_campo_santo.Split(',').OrderByDescending(x => x.ToString()));
                    item.codigo_tipo_venta = string.Join(",", item.codigo_tipo_venta.Split(',').OrderByDescending(x => x.ToString()) );
                    item.codigo_empresa = string.Join(",", item.codigo_empresa.Split(',').OrderByDescending(x => x.ToString()));

                }
                
                    detalle_regla.codigo_campo_santo = string.Join(",",detalle_regla.codigo_campo_santo.Split(',').OrderByDescending(x=>x.ToString()));
                    detalle_regla.codigo_tipo_venta = string.Join(",",detalle_regla.codigo_tipo_venta.Split(',').OrderByDescending(x => x.ToString()));
                    detalle_regla.codigo_empresa = string.Join(",", detalle_regla.codigo_empresa.Split(',').OrderByDescending(x => x.ToString()));
                    
                    foreach (var item in lista_detalle_regla)
                    {
                        if (item.codigo_canal==detalle_regla.codigo_canal &&
                            item.codigo_campo_santo.Contains(detalle_regla.codigo_campo_santo) &&
                            item.codigo_empresa.Contains(detalle_regla.codigo_empresa) &&
                            item.codigo_tipo_venta.Contains(detalle_regla.codigo_tipo_venta) 
                            )
                        {
                            throw new Exception("Regla ingresado se repite, vuelva intentar."); 
                        }
                        

                    }
                
                /*
                var resultado = lista_detalle_regla.FindAll(                    
                    x =>x.codigo_canal==detalle_regla.codigo_canal &&
                        x.codigo_tipo_venta.Contains(detalle_regla.codigo_tipo_venta)&&
                        x.codigo_empresa.Contains(detalle_regla.codigo_empresa)&&
                        x.codigo_campo_santo.Contains(detalle_regla.codigo_campo_santo)
                    
                    );
                if (resultado.Count > 0)
                {                
                     throw new Exception("Regla ingresado se repite, vuelva intentar.");               
                }
                */

                vMensaje = "Se registro satisfactoriamente.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region REGISTRAR
        [HttpPost]        
        public ActionResult Registrar(regla_tipo_planilla_dto v_entidad, List<grilla_detalle_regla_tipo_planilla_dto> lista_detalle_regla)
        {
            int vResultado = 1;
            Int64 v_codigo_regla_tipo_planilla;
            string vMensaje = string.Empty;
            try
            {
          
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                if (beanSesionUsuario != null)
                    v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                else
                    v_entidad.usuario_registra = "root";
                foreach (var item in lista_detalle_regla){               
                    item.usuario_registra = v_entidad.usuario_registra;                    
                }

                v_entidad.usuario_registra = v_entidad.usuario_registra;
                
                MensajeDTO v_mensaje = ReglaTipoPlanillaBL.Instance.Insertar(v_entidad, lista_detalle_regla);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                v_codigo_regla_tipo_planilla = v_mensaje.idRegistro;
                vMensaje = "Se registro satisfactoriamente.";


            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
                v_codigo_regla_tipo_planilla = 0;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_regla_tipo_planilla = v_codigo_regla_tipo_planilla }, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public ActionResult Eliminar(int p_codigo_regla_tipo_planilla)
        {
            int vResultado = 1;
            Int64 v_codigo_regla_tipo_planilla;
            string vMensaje = string.Empty;
            try
            {
                regla_tipo_planilla_dto v_entidad = new regla_tipo_planilla_dto();
                v_entidad.codigo_regla_tipo_planilla = p_codigo_regla_tipo_planilla;
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                if (beanSesionUsuario != null)
                    v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                else
                    v_entidad.usuario_registra = "root";
              
                v_entidad.usuario_registra = v_entidad.usuario_registra;

                MensajeDTO v_mensaje = ReglaTipoPlanillaBL.Instance.Eliminar(v_entidad);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                v_codigo_regla_tipo_planilla = v_mensaje.idRegistro;
                vMensaje = "Se registro satisfactoriamente.";


            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
                v_codigo_regla_tipo_planilla = 0;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_regla_tipo_planilla = v_codigo_regla_tipo_planilla }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        public ActionResult GetDetalleAllJson(int p_codigo_regla_tipo_planilla)
        {
            int v_total = 0;
            var v_resultado = new List<grilla_detalle_regla_tipo_planilla_dto>();
            try
            {

                v_resultado = ReglaTipoPlanillaBL.Instance.ListarDetalle(p_codigo_regla_tipo_planilla);
                v_total = v_resultado.Count();
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.ToString();
            }

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = v_resultado }), "application/json");

        }
    }
}
