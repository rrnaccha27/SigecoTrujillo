using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class PlanillaComercialController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly CanalGrupoService _canalService;
        private readonly TipoPlanillaService _tipoPlanillaService;

        //
        // GET: /Comision/Planilla/
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public PlanillaComercialController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
            _tipoPlanillaService = new TipoPlanillaService();

        }

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._tipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
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

                ex.ToString();
            }


            return View(bean);
        }

        public ActionResult GetComboReglaTipoPlanillaAllJson()
        {
            List<combo_regla_tipo_planilla_dto> result = ReglaTipoPlanillaBL.Instance.GetAllComboJson();
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult GetPagosHabilitadoJson(int codigo_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                
                lst = DetallePlanillaSelBL.Instance.ListarPagoHabilitadoByIdPlanilla(codigo_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }



        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Planilla(int p_codigo_planilla)
        {
            PlanillaViewModel vm = new PlanillaViewModel();

            vm.planilla = PlanillaSelBL.Instance.BuscarById(p_codigo_planilla);


            return PartialView(vm);
        }


        public ActionResult _Liquidacion(string p_codigo_planilla, string p_codigo_personal)
        {

            ReporteViewModel vm = new ReporteViewModel();
            try
            {
                string urlReporte = Url.Action("frm_reporte_liquidacion", "Areas/Comision/Reporte/Planilla/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;
                vm.url = urlReporte;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);
        }

        public ActionResult _Reporte(int p_codigo_planilla,int p_codigo_personal)
        {
            ReporteViewModel vm = new ReporteViewModel();  
            
            try
            {
                string urlReporte = Url.Action("frm_reporte_liquidacion", "Areas/Comision/Reporte/Planilla/frm") + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "& codigo_personal=" + p_codigo_personal;
                vm.url = urlReporte;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);
        }


        public ActionResult _Exportar(int p_codigo_planilla)
        {
            ReporteViewModel vm = new ReporteViewModel();           
            try
            {
                string urlReporte = Url.Action("frm_reporte_planilla_comercial", "Areas/Comision/Reporte/PlanillaComercial/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla;
                vm.url = urlReporte;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte",vm);
        }



    }
}
