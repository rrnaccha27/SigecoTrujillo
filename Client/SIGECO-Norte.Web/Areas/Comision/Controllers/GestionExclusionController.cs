using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
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
    public class GestionExclusionController : Controller
    {

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public GestionExclusionController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
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
        [HttpPost]
        public ActionResult GetPagosExcluidoAllJson(listado_exclusion_grilla_dto v_planilla)
        {
            List<listado_exclusion_grilla_dto> lst = new List<listado_exclusion_grilla_dto>();
            try
            {
                lst = DetalleCronogramaPagoSelBL.Instance.ListarPagosExcluidosAll(v_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpPost]
        public ActionResult GetPlanillaAbierta()
        {
            List<grilla_planilla_exclusion_dto> lst = new List<grilla_planilla_exclusion_dto>();
            try
            {

                lst = PlanillaSelBL.Instance.ListarPlanillaAbiertaGestionExclusion();

            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            lst.Insert(0, new grilla_planilla_exclusion_dto() { codigo_planilla = 0, numero_planilla = "Sin Asignar" });
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpPost]
        public ActionResult GetDetallePagoComisionVsPlanillaAbierta(string  p_lst_id_exclusion)
        {            

            List<grilla_cuota_pago_planilla_dto> lst = new List<grilla_cuota_pago_planilla_dto>();
            try
            {
                var v_lst_id_exclusion =new List<collection_id_exclusion_dto>();
                string[] _array = p_lst_id_exclusion.Split(',');
                for (int i = 0; i < _array.Length; i++)
                {
                    var v_entidad = new collection_id_exclusion_dto() {codigo_exclusion=int.Parse(_array[i].ToString())};
                    v_lst_id_exclusion.Add(v_entidad);
                }

                
                lst = PlanillaSelBL.Instance.ListarPagoComisionVsPlanillaAbiertaGestionExclusion(v_lst_id_exclusion);

            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }

            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Detalle_Exclusion(int p_codigo_exclusion)
        {
            detalle_exclusion_dto _detalle_exclusion_dto = new detalle_exclusion_dto();
            try
            {
                _detalle_exclusion_dto = DetalleCronogramaPagoSelBL.Instance.GetDetalleExclusionCuotaPagoComision(p_codigo_exclusion);
            }
            catch (Exception ex)
            {
                string _mensaje = ex.Message;

            }
            return PartialView(_detalle_exclusion_dto);
        }
        [HttpGet]
        public ActionResult _Registrar_Exclusion()
        {

            return PartialView();

        }


        [HttpPost]
        [RequiresAuthentication]
        public ActionResult HabilitarCuotaPagoComision(List<grilla_cuota_pago_planilla_dto> lst_cuota_pago_comision)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                foreach (var item in lst_cuota_pago_comision)
                {
                    item.usuario_registra = beanSesionUsuario.codigoUsuario;
                }

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.GestionExclusionHabilitarPagoComision(lst_cuota_pago_comision);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                vMensaje = "Se registró satisfactoriamente.";

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
