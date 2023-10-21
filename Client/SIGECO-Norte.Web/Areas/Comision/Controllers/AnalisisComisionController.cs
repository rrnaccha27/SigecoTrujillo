using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.MemberShip.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Areas.Comision.Models;
using System.Text;
using SIGEES.Entidades.planilla;
using System.Configuration;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class AnalisisComisionController : Controller
    {
        private readonly EmpresaSIGECOService _empresaService;
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

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

        public AnalisisComisionController() {
            _empresaService = new EmpresaSIGECOService();
            _tipoAccesoItemService = new TipoAccesoItemService();
        }

        public ActionResult GetEmpresaJson()
        {
            string result = this._empresaService.GetAllComboJson(true);
            return Content(result, "application/json");
        }

        public ActionResult GetTipoPlanillaJson(int codigo_empresa, string nro_contrato)
        {
            filtro_contrato_dto busqueda = new filtro_contrato_dto {
                codigo_empresa = codigo_empresa
                ,numero_contrato = nro_contrato
            };
            List<analisis_contrato_combo_dto> result = ContratoSelBL.Instance.ListarTipoPlanillaByContrato(busqueda);
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpGet]
        public PartialViewResult _ModificarCuota(int codigo_detalle_cronograma)
        {
            detalle_cronograma_comision_dto v_entidad = new detalle_cronograma_comision_dto();
            try
            {

                v_entidad = DetalleCronogramaPagoSelBL.Instance.CuotaPagoById(codigo_detalle_cronograma);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

            }

            return PartialView(v_entidad);
        }

        [HttpGet]
        public ActionResult _DetalleCuota(int codigo_detalle_cronograma)
        {
            operacion_cuota_comision_listado_dto item = new operacion_cuota_comision_listado_dto();
            try
            {
                item.codigo_detalle_cronograma = codigo_detalle_cronograma;
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpGet]
        public PartialViewResult _ExclusionCuota(int codigo_detalle_cronograma)
        {
            ExclusionViewModel vm = new ExclusionViewModel();

            detalle_cronograma_comision_dto detalle_cronograma = DetalleCronogramaPagoSelBL.Instance.CuotaPagoById(codigo_detalle_cronograma);

            //vm.codigo_detalle_planilla = detalle_cronograma.codigo_detalle_planilla;
            //vm.codigo_planilla = detalle_cronograma.codigo_planilla;

            return PartialView(vm);
        }

        [HttpGet]
        public PartialViewResult _AdicionarCuota(int codigo_empresa, string nro_contrato, int codigo_articulo)
        {
            AdicionCuotaViewModel vm = null;

            try
            {
                articulo_dto articulo = ArticuloBL.Instance.BuscarById(codigo_articulo);

                vm = new AdicionCuotaViewModel {
                    codigo_empresa = codigo_empresa,
                    nro_contrato = nro_contrato,
                    codigo_articulo = codigo_articulo,
                    nombre_articulo = articulo.nombre
                };
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult GetContratoJson(int codigo_empresa, string numero_contrato)
        {
            analisis_contrato_dto v_entidad = new analisis_contrato_dto();
            try
            {
                int sede = System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString());
                v_entidad = ContratoSelBL.Instance.BuscarByEmpresaContrato(codigo_empresa, numero_contrato,sede);

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                v_entidad.existe_registro = -1;

            }
            return Content(JsonConvert.SerializeObject(v_entidad), "application/json");     

        }

        [HttpPost]
        public ActionResult ModificarCuota(detalle_cronograma_comision_dto detalle_cronograma)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                detalle_cronograma.usuario_modifica = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.Modificar(detalle_cronograma);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se realizó la operación satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetListadoOperacionJson(int codigo_detalle_cronograma)
        {
            List<operacion_cuota_comision_listado_dto> lst = new List<operacion_cuota_comision_listado_dto>();
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                operacion_cuota_comision_listado_dto busqueda = new operacion_cuota_comision_listado_dto { codigo_detalle_cronograma = codigo_detalle_cronograma, usuario = beanSesionUsuario.codigoUsuario };

                lst = DetalleCronogramaPagoSelBL.Instance.ListadoOperacion(busqueda);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        [HttpPost]
        public ActionResult AdicionarCuota(detalle_cronograma_adicionar_dto detalle_cronograma)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                detalle_cronograma.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.Adicionar(detalle_cronograma);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se realizó la operación satisfactoriamente. Nro de cuota " + Convert.ToString(mensaje.idRegistro) + ".";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excluir(detalle_planilla_exclusion_dto v_exclusion)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            List<detalle_planilla_inclusion_dto> lst_detalle = v_exclusion.lst_detalle_cronograma;
            StringBuilder xmlContratos = new StringBuilder();

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_exclusion.usuario_registra = beanSesionUsuario.codigoUsuario;

                xmlContratos.Append("<cronograma>");

                foreach (detalle_planilla_inclusion_dto detalle in lst_detalle)
                {
                    xmlContratos.Append("<detalle codigo_detalle_cronograma='" + detalle.codigo_detalle_cronograma.ToString() + "' />");
                }
                xmlContratos.Append("</cronograma>");
                v_exclusion.procesarXML = xmlContratos.ToString();

                MensajeDTO mensaje = DetallePlanillaBL.Instance.Excluir(v_exclusion);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se realizó la operación satisfactoriamente.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult BloquearReproceso(log_contrato_sap_bloqueo_dto contrato)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                contrato.usuario_bloqueo = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = LogContratoSAPBL.Instance.Bloquear(contrato);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = mensaje.mensaje;
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeshabilitarCuota(detalle_cronograma_deshabilitacion_dto v_deshabilitar)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            List<detalle_cronograma_elemento_dto> lst_detalle = v_deshabilitar.lst_detalle_cronograma_elemento;
            StringBuilder xmlContratos = new StringBuilder();

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_deshabilitar.usuario_registra = beanSesionUsuario.codigoUsuario;

                xmlContratos.Append("<cronograma>");

                foreach (detalle_cronograma_elemento_dto detalle in lst_detalle)
                {
                    xmlContratos.Append("<detalle codigo_detalle_cronograma='" + detalle.codigo_detalle_cronograma.ToString() + "' />");
                }
                xmlContratos.Append("</cronograma>");
                v_deshabilitar.procesarXML = xmlContratos.ToString();

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.Deshabilitar(v_deshabilitar);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se realizó la operación satisfactoriamente.";
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
