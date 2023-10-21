using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ConsultaPlanillaBonoController : Controller
    {
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly IParametroSistemaService _IParametroSistemaService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        
        //
        // GET: /Comision/PlanillaBono/
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public ConsultaPlanillaBonoController() {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _IParametroSistemaService = new ParametroSistemaService();
        }

        /*
        public ActionResult GetComboReglaTipoPlanillaAllJson()
        {
            List<combo_regla_tipo_planilla_dto> result = ReglaTipoPlanillaBL.Instance.GetAllComboJson();
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }*/

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

                ex.ToString();
            }


            return View(bean);
        }
        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Planilla(int p_codigo_planilla)
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            PlanillaViewModel vm = new PlanillaViewModel();
            
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

            if (p_codigo_planilla > 0)
            {
                vm.planilla_bono = PlanillaSelBL.Instance.BuscarPlanillaBonoById(p_codigo_planilla);
            }
            else
            {
                
                var planilla = new Planilla_bono_dto();
                planilla.fecha_apertura = DateTime.Now;
                planilla.usuario_apertura = beanSesionUsuario.codigoUsuario;
                vm.planilla_bono = planilla;
            }

            vm.bean = bean;
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult GetPagosHabilitadosJson(int codigo_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarByIdPlanillaBono(codigo_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpPost]
        public ActionResult GetPagosExcluidosJson(int codigo_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarExcluidosByIdPlanillaBono(codigo_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult _Buscar()
        {
            int v_total = 0;
            var lista = new List<grilla_planilla_bono>();
            try
            {
                  lista = PlanillaSelBL.Instance.ListarPlanillaBono();
            }
            catch (Exception ex)
            {
               string mensaje=ex.ToString();                
            }

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = lista }), "application/json");
        }

        [HttpGet]
        public ActionResult _Reporte_Planilla(int p_codigo_planilla,int p_codigo_tipo_planilla,int? p_codigo_personal)
        {
            ReporteViewModel vm = new ReporteViewModel();
            string vUrl = string.Empty;
            try
            {
                if(p_codigo_tipo_planilla==1)
                {
                    vUrl = Url.Action("frm_reporte_bono_personal", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal; 
                }
                if (p_codigo_tipo_planilla == 2)
                {
                    vUrl = Url.Action("frm_reporte_bono_supervisor", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal; 
                }                
                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte",vm);
        }
        public ActionResult _Reporte_Liquidacion(int p_codigo_planilla, int p_codigo_tipo_planilla, int? p_codigo_personal) 
        {
            ReporteViewModel vm = new ReporteViewModel();
             string vUrl = string.Empty;
            try
            {
                if(p_codigo_tipo_planilla==1)
                {
                    vUrl = Url.Action("frm_liquidacion_bono_personal", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;
                }
                if(p_codigo_tipo_planilla==2)
                {
                    vUrl = Url.Action("frm_liquidacion_bono_supervisor_general", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;
                }
                
                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);
        
        }
    
        public ActionResult _Reporte_Liquidacion_individual(int p_codigo_planilla, int p_codigo_tipo_planilla, int? p_codigo_personal)
        {
            ReporteViewModel vm = new ReporteViewModel();
            string vUrl = string.Empty;
            try
            {
                vUrl = Url.Action("frm_liquidacion_bono_supervisor_individual", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;

                //if (p_codigo_tipo_planilla == 1)
                //{
                //    vUrl = Url.Action("frm_liquidacion_bono_personal", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;
                //}
                //if (p_codigo_tipo_planilla == 2)
                //{
                //    vUrl = Url.Action("frm_liquidacion_bono_supervisor_general", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;
                //}

                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);

        }

        public ActionResult GetCanalJson()
        {
            List<canal_grupo_combo_dto> lst = new List<canal_grupo_combo_dto>();
            try
            {
                lst = new CanalGrupoBL().Listar_Canal_Planilla_Bono();
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

    }
}
