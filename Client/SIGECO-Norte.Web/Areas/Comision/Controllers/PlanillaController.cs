
using log4net;
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
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SIGEES.Web.Areas.Comision.Utils;
using ClosedXML.Excel;
using System.Web.Hosting;
using System.Configuration;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    //[SessionExpireFilter]
    //[SessionExpireFilterAttribute]

    [RequiresAuthenticationAttribute]
    public class PlanillaController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;

        private readonly CanalGrupoService _canalService;
        private readonly TipoPlanillaService _tipoPlanillaService;
        private readonly EmpresaSIGECOService _empresaSIGECOService;
        private readonly IParametroSistemaService _IParametroSistemaService;
        private readonly TipoVentaService _tipoVentaService;

        LocalReport rptLiquidacion;
        LocalReport rptResumenLiquidacion;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PlanillaController));

        //
        // GET: /Comision/Planilla/
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public PlanillaController()
        {
            _tipoVentaService = new TipoVentaService();
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
            _tipoPlanillaService = new TipoPlanillaService();
            _empresaSIGECOService = new EmpresaSIGECOService();
            _IParametroSistemaService = new ParametroSistemaService();
          //  GenerarComisionExcel(50);
        }

        [RequiresAuthentication]
        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetEmpresaByContratoJson(string p_numero_contrato)
        {
            filtro_contrato_dto busqueda = new filtro_contrato_dto { numero_contrato = p_numero_contrato };
            List<analisis_contrato_combo_dto> result = ContratoSelBL.Instance.ListarEmpresasByContrato(busqueda);
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._tipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ViewResult Index()
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

        public ActionResult GetFilterTipoVentaJson()
        {
            string result = this._tipoVentaService.GetComboFilterAbreaviaturaJson();
            return Content(result, "application/json");
        }
        public ActionResult GetComboReglaTipoPlanillaAllJson()
        {
            List<combo_regla_tipo_planilla_dto> result = ReglaTipoPlanillaBL.Instance.GetAllComboJson();
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult _Buscar()
        {

            var query = new object();
            int v_resultado = 1;
            string v_mensaje = "";
            int v_total = 0;
            try
            {
                var lst = PlanillaSelBL.Instance.Listar();
                v_total = lst.Count();
                query = from order in lst.AsEnumerable()
                        select new
                        {
                            codigo_planilla = order.codigo_planilla,
                            numero_planilla = order.numero_planilla,
                            nombre_regla_tipo_planilla = order.nombre_regla_tipo_planilla,
                            nombre_tipo_planilla = order.tipo_planilla.nombre,
                            fecha_inicio = order.fecha_inicio.ToShortDateString(),
                            fecha_fin = order.fecha_fin.ToShortDateString(),
                            nombre_estado_planilla = order.estado_planilla.nombre,
                            fecha_apertura = order.fecha_apertura.ToShortDateString(),
                            codigo_estado_planilla = order.codigo_estado_planilla,
                            fecha_cierre = order.fecha_cierre == null ? "" : DateTime.Parse(order.fecha_cierre.ToString()).ToShortDateString(),
                            fecha_anulacion = order.fecha_anulacion == null ? "" : DateTime.Parse(order.fecha_anulacion.ToString()).ToShortDateString(),
                            estilo = order.estilo,
                            envio_liquidacion = (order.envio_liquidacion ? 1 : 0)
                        };
            }
            catch (Exception ex)
            {
                ex.ToString();
                v_resultado = -1;
                v_mensaje = ex.Message;
            }
            var jsonResult = Json(new
            {
                v_resultado = v_resultado,
                v_mensaje = v_mensaje,
                total = v_total,
                rows = query

            }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetPagosHabilitadoJson(detalle_planilla_resumen_dto v_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarByIdPlanilla(v_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        #region GENERAR EXCEL - COMISIONES
        public byte[] ReadFileToBytes(string sPathFile)
        {
            using (FileStream fileStream = new FileStream(sPathFile, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[(int)fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);
                fileStream.Close();
                return bytes;
            }
        }
        public void GenerarComisionExcel(int p_codigo_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            detalle_planilla_resumen_dto v_planilla = new detalle_planilla_resumen_dto();
            v_planilla.codigo_planilla = p_codigo_planilla;

            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            var FileName = $"Comision_{numRandom}.xlsx";

            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarByIdPlanilla(v_planilla);
                // string urlPlantilla = Server.MapPath("~/Plantilla/plantilla_comision_chiclayo.xlsx");
                string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_comision_chiclayo.xlsx");

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Add($"comision-15");
                        worksheet.Cell("A5").Value = "Tipo Contrato";
                        worksheet.Cell("B5").Value = "Tipo Servicio";
                        worksheet.Cell("C5").Value = "Tarifa Parcel";
                        worksheet.Cell("D5").Value = "Propuesta";
                        worksheet.Cell("E5").Value = "Fecha";
                        worksheet.Cell("F5").Value = "Total Venta";
                        worksheet.Cell("G5").Value = "Anticipo";
                        worksheet.Cell("H5").Value = "% de inicial Comisión";
                        worksheet.Cell("I5").Value = "% Comisión";


                        
                            workbook.SaveAs(Path.Combine(urlBase, Path.Combine("Temp", FileName)));
                        
                        
                    }
                }





            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }

        }
        #endregion


        [HttpPost]
        public ActionResult GetPagosComisionManualJson(detalle_planilla_resumen_dto v_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarComisionManualByIdPlanilla(v_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpPost]
        public ActionResult GetPagosExcluidoJson(detalle_planilla_resumen_dto v_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarDetallePlanillaExcluidoByIdPlanilla(v_planilla.codigo_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }


        [HttpPost]
        public ActionResult GetPagosDescuentoJson(int p_codigo_planilla)
        {
            List<lista_descuento_dto> lst = new List<lista_descuento_dto>();
            try
            {
                lst = DetallePlanillaSelBL.Instance.ListarDescuentoByIdPlanilla(p_codigo_planilla);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpGet]
        public PartialViewResult _Planilla(int p_codigo_planilla)
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            PlanillaViewModel vm = new PlanillaViewModel();

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

                var planilla = new Planilla_dto();


                if (p_codigo_planilla > 0)
                {
                    planilla = PlanillaSelBL.Instance.BuscarById(p_codigo_planilla);
                }
                else
                {
                    //beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                    planilla.fecha_apertura = DateTime.Now;
                    planilla.usuario_apertura = beanSesionUsuario.codigoUsuario;
                }

                vm.bean = bean;
                vm.planilla = planilla;
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

            return PartialView(vm);
        }

        [HttpGet]
        public PartialViewResult _Exclusion(int codigo_detalle_planilla, int codigo_planilla)
        {
            ExclusionViewModel vm = new ExclusionViewModel();

            vm.codigo_detalle_planilla = codigo_detalle_planilla;
            vm.codigo_planilla = codigo_planilla;

            return PartialView(vm);
        }

        [HttpGet]
        public PartialViewResult _Descuento(int p_codigo_planilla)
        {
            descuento_dto v_descuento = new descuento_dto();
            v_descuento.codigo_planilla = p_codigo_planilla;
            return PartialView(v_descuento);
        }

        #region ANALISIS DE CONTRATO


        [HttpGet]
        public ActionResult _Analisis_Contrato(int codigo_empresa, string nro_contrato)
        {
            analisis_contrato_dto v_entidad = new analisis_contrato_dto();
            try
            {
                int sede = System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString());
                v_entidad = ContratoSelBL.Instance.BuscarByEmpresaContrato(codigo_empresa, nro_contrato, sede);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                v_entidad.existe_registro = -1;

            }


            return PartialView(v_entidad);
        }



        [HttpPost]
        public ActionResult GetArticulosByContratoEmpresaJson(filtro_contrato_dto v_entidad)
        {
            var lst = new List<analisis_contrato_articulo_cronograma_dto>();
            try
            {

                int sede = System.Convert.ToInt32(ConfigurationManager.AppSettings["sede"].ToString());
                lst = ContratoSelBL.Instance.ListarArticuloByContrato_Empresa(v_entidad, sede);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpPost]
        public ActionResult GetCronogramaCuotasByContratoEmpresaJson(filtro_contrato_dto v_entidad)
        {
            var lst = new List<analisis_contrato_cronograma_cuotas_dto>();
            try
            {
                lst = ContratoSelBL.Instance.ListarCronogramaCuotasByContrato_Empresa(v_entidad);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        [HttpPost]
        public ActionResult GetDetalleCronogramaPagoByArticuloJson(filtro_contrato_dto v_entidad)
        {

            var lst = new List<detalle_cronograma_comision_dto>();
            try
            {
                lst = ContratoSelBL.Instance.ListarCronogramaPagoByArticuloContrato(v_entidad);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");



        }
        #endregion
        [HttpPost]
        public ActionResult Descuento_Desactivar(int p_codigo_descuento)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            long p_codigo_planilla = 0;
            try
            {
                descuento_dto v_descuento = new descuento_dto();
                v_descuento.codigo_descuento = p_codigo_descuento;

                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_descuento.usuario_registra = beanSesionUsuario.codigoUsuario;
                v_descuento.estado_registro = false;
                MensajeDTO mensaje = PlanillaBL.Instance.Desactivar_Descuento(v_descuento);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                p_codigo_planilla = mensaje.idRegistro;
                vMensaje = "Se desactiv&oacute; satisfactoriamente.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult Descuento_Registrar(List<descuento_dto> v_descuento)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            long p_codigo_planilla = 0;
            try
            {

                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                foreach (var item in v_descuento)
                {
                    if (item.monto <= 0)
                    {
                        throw new Exception("Ingrese el monto del descuento mayor que cero.");
                    }
                    if (item.codigo_personal <= 0)
                    {
                        throw new Exception("Código de vendedor no identificado.");
                    }
                    if (item.codigo_planilla <= 0)
                    {
                        throw new Exception("Código de planilla no identificado.");
                    }
                    item.usuario_registra = beanSesionUsuario.codigoUsuario;
                    item.estado_registro = true;
                    //item.motivo = "algo aaa";
                }


                MensajeDTO mensaje = PlanillaBL.Instance.Registrar_Descuento(v_descuento);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                p_codigo_planilla = mensaje.idRegistro;
                vMensaje = "Se registro satisfactoriamente.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Calcular_Comision_Persona(int p_codigo_planilla, int p_codigo_persona)
        {


            string vMensaje = string.Empty;


            int v_codigo_estado_en_proceso_pago = (int)SIGEES.Web.Areas.Comision.Utils.EstadoCuota.en_proceso;
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {
                if (p_codigo_persona <= 0)
                {
                    throw new Exception("Código de vendedor no identificado.");
                }
                if (p_codigo_planilla <= 0)
                {
                    throw new Exception("Código de planilla no identificado.");
                }
                lst = DetallePlanillaSelBL.Instance.ObtenerSaldoPersonalByPlanilla(p_codigo_planilla, p_codigo_persona, v_codigo_estado_en_proceso_pago);
            }
            catch (Exception ex)
            {
                vMensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
            // return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, v_comision_total = v_comision_total, v_igv = v_igv, v_comision_bruto = v_comision_bruto }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _Persona_Busqueda()
        {

            return PartialView();
        }

        #region Operaciones de Planilla

        [HttpPost]
        public ActionResult Registrar(Planilla_dto v_planilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            long p_codigo_planilla = 0;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;
                MensajeDTO mensaje = PlanillaBL.Instance.Generar(v_planilla);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                p_codigo_planilla = mensaje.idRegistro;

                try
                {
                    string[] parameters = new string[2];
                    parameters[0] = p_codigo_planilla.ToString();
                    parameters[1] = "";

                    Thread SendingThreads = new Thread(fnEnviarEmail);
                    SendingThreads.Start(parameters);
                }
                catch (Exception ex2)
                {

                }
                finally
                {
                    vMensaje = "Se generó satisfactoriamente la planilla, total de registros procesados es " + mensaje.total_registro_afectado;
                }
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Cerrar(Planilla_dto v_planilla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.Cerrar(v_planilla);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                string[] parameters = new string[2];
                parameters[0] = v_planilla.codigo_planilla.ToString();
                parameters[1] = (String.IsNullOrEmpty(v_planilla.numero_planilla) ? "" : v_planilla.numero_planilla.ToString());

                Thread SendingThreads = new Thread(fnEnviarEmail);
                SendingThreads.Start(parameters);

                vMensaje = "Se cerró satisfactoriamente.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Anular(int p_codigo_planilla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            long v_codigo_planilla = 0;
            try
            {
                Planilla_dto v_planilla = new Planilla_dto();
                v_planilla.codigo_planilla = p_codigo_planilla;
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.Anular(v_planilla);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                v_codigo_planilla = mensaje.idRegistro;
                vMensaje = "Se anuló satisfactoriamente la planilla";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public ActionResult Excluir(detalle_planilla_dto v_detalle_planilla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_detalle_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;
                //v_detalle_planilla.excluido = false;

                MensajeDTO mensaje = DetallePlanillaBL.Instance.Excluir(v_detalle_planilla);
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
        public ActionResult HabilitarPago(detalle_cronograma_dto v_detalle_cronograma_dto)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_detalle_cronograma_dto.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.HabilitarPago(v_detalle_cronograma_dto);
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

        //MYJ - 20171124
        [HttpPost]
        public ActionResult Incluir_Listar(string nro_contrato, string codigo_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();

            try
            {

                lst = DetallePlanillaSelBL.Instance.IncluirListar(nro_contrato, Convert.ToInt32(codigo_planilla));
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        //MYJ - 20171124
        [HttpPost]
        public ActionResult Incluir_Procesar(int codigo_planilla, List<detalle_planilla_inclusion_dto> lst_inclusion)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                MensajeDTO mensaje = DetallePlanillaSelBL.Instance.IncluirProcesar(codigo_planilla, lst_inclusion, beanSesionUsuario.codigoUsuario);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se incluyo satisfactoriamente a la planilla " + mensaje.idRegistro.ToString() + " pago(s).";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        #region SECCION-REPORTE

        public PartialViewResult _Reporte_Planilla(int p_codigo_planilla, int p_codigo_personal)
        {
            ReporteViewModel vm = new ReporteViewModel();
            try
            {
                string urlReporte = Url.Action("frm_reporte_planilla", "Areas/Comision/Reporte/Planilla/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=" + p_codigo_personal;
                vm.url = urlReporte;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);

        }

        public ActionResult _Reporte_Liquidacion(string p_codigo_planilla, string p_codigo_personal)
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

        public ActionResult _Reporte_Resumen_Liquidacion(string p_codigo_planilla)
        {

            ReporteViewModel vm = new ReporteViewModel();
            try
            {
                string urlReporte = Url.Action("frm_reporte_resumen_liquidacion", "Areas/Comision/Reporte/Planilla/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla;
                vm.url = urlReporte;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);
        }

        #endregion

        #region ENVIAR CORREO

        [HttpPost]
        public ActionResult _Enviar_Correo_Liquidacion(int p_codigo_planilla, string p_nro_pllanilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                List<reporte_detalle_liquidacion> dt_general = PlanillaSelBL.Instance.ReporteDetalleLiquidacionPlanilla(p_codigo_planilla);
                if (dt_general == null)
                {
                    throw new Exception("No se encontró registro de planillas para el envío.");
                }
                if (dt_general.Count == 0)
                {
                    throw new Exception("No se encontró registro de planillas para el envío.");
                }
                string[] parameters = new string[2];
                parameters[0] = p_codigo_planilla.ToString();
                parameters[1] = p_nro_pllanilla;

                Thread SendingThreads = new Thread(fnEnviarEmail);
                SendingThreads.Start(parameters);

                vMensaje = "El envío de correo se encuentra en proceso.";
                vResultado = 1;
            }
            catch (System.Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);

        }

        private void fnEnviarEmail(object parameters)
        {
            Array arrayParameters = new object[2];
            arrayParameters = (Array)parameters;
            string v_titulo = string.Empty;
            string contenidoCorreo = string.Empty;

            try
            {
                if (!DebeEnviarCorreo()) { return; }

                int p_codigo_planilla = int.Parse((string)arrayParameters.GetValue(0));
                string p_nro_pllanilla = (string)arrayParameters.GetValue(1);

                List<personal_correo> lst_personal = new List<personal_correo>();
                List<personal_jefatura_correo> lst_jefatura = new List<personal_jefatura_correo>();
                List<canal_jefatura_dto> lst_canal_jefatura = new CanalGrupoBL().ListarJefatura();
                List<reporte_detalle_liquidacion> dt_general = PlanillaSelBL.Instance.ReporteDetalleLiquidacionPlanilla(p_codigo_planilla);
                Planilla_dto planilla = PlanillaSelBL.Instance.BuscarById(p_codigo_planilla);

                if (planilla.envio_liquidacion == false)
                {
                    return;
                }

                var _item_usuario = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_usuario);
                var _item_passwor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_password);
                var _item_servidor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_servidor);
                var _item_puerto = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_puerto);
                var _item_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.porcentaje_detraccion);

                var _v_planilla = dt_general.FirstOrDefault();
                v_titulo = "CORRESPONDIENTE AL PERIODO DEL " + getNombreDia(_v_planilla.fecha_inicio) + " " + getDia(_v_planilla.fecha_inicio) + " de " + getNombreMes(_v_planilla.fecha_inicio) + " del " + getAnho(_v_planilla.fecha_inicio) +
                    " AL " + getNombreDia(_v_planilla.fecha_fin) + " " + getDia(_v_planilla.fecha_fin) + " de " + getNombreMes(_v_planilla.fecha_fin) + " del " + getAnho(_v_planilla.fecha_fin);

                p_nro_pllanilla = (String.IsNullOrEmpty(p_nro_pllanilla) ? planilla.numero_planilla : p_nro_pllanilla);

                string[] datosCorreo = new string[(int)TemplateCorreoParametros.total_enum];
                datosCorreo[(int)TemplateCorreoParametros.planilla] = planilla.nombre_regla_tipo_planilla;
                datosCorreo[(int)TemplateCorreoParametros.numero_planilla] = planilla.numero_planilla;
                datosCorreo[(int)TemplateCorreoParametros.cuenta] = _item_usuario.valor;
                datosCorreo[(int)TemplateCorreoParametros.fecha] = DateTime.Now.ToShortDateString() + " " + String.Format("{0:T}", DateTime.Now);

                if (rptLiquidacion == null)
                {
                    rptLiquidacion = new LocalReport();
                    rptLiquidacion.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Planilla/rdl/rpt_liquidacion" + _v_planilla.tipo_reporte + ".rdlc");
                }

                if (rptResumenLiquidacion == null)
                {
                    rptResumenLiquidacion = new LocalReport();
                    rptResumenLiquidacion.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Planilla/rdl/rpt_liquidacion_resumen" + _v_planilla.tipo_reporte + ".rdlc");
                }

                ReportParameter p_porcentaje_detraccion = new ReportParameter("p_porcentaje_detraccion", _item_detraccion.valor);
                ReportParameter p_titulo_reporte = new ReportParameter("p_titulo_reporte", v_titulo);

                rptLiquidacion.SetParameters(new ReportParameter[] { p_porcentaje_detraccion, p_titulo_reporte });
                rptResumenLiquidacion.SetParameters(new ReportParameter[] { p_porcentaje_detraccion, p_titulo_reporte });

                if (_v_planilla.codigo_tipo_planilla == 1)
                {
                    foreach (var item in dt_general)
                    {
                        if (!lst_personal.Exists(x => x.codigo_personal == item.codigo_personal_referencial))
                        {
                            lst_personal.Add(new personal_correo()
                            {
                                nombres = item.nombre_personal_referencial,
                                codigo_personal = item.codigo_personal_referencial,
                                email = item.email_personal_referencial,
                                nombre_envio_correo = item.nombre_envio_correo,
                                apellido_envio_correo = item.apellido_envio_correo,
                                nombre_grupo = item.canal_grupo_nombre
                            });
                        }
                    }
                }

                if (_v_planilla.codigo_tipo_planilla == 2)
                {
                    foreach (var item in dt_general)
                    {
                        if (!lst_personal.Exists(x => x.codigo_personal == item.codigo_personal))
                        {
                            lst_personal.Add(new personal_correo()
                            {
                                nombres = item.nombre_personal,
                                codigo_personal = item.codigo_personal,
                                email = item.email_personal,
                                nombre_envio_correo = item.nombre_envio_correo,
                                apellido_envio_correo = item.apellido_envio_correo,
                                nombre_grupo = item.canal_grupo_nombre
                            });
                        }
                    }
                }

                foreach (var item in dt_general)
                {
                    if (!lst_jefatura.Exists(x => x.codigo_canal == item.codigo_canal))
                    {
                        canal_jefatura_dto canal = lst_canal_jefatura.FindAll(x => x.codigo_canal == item.codigo_canal).FirstOrDefault();

                        lst_jefatura.Add(new personal_jefatura_correo()
                        {
                            codigo_canal = item.codigo_canal,
                            email = canal.email,
                            email_copia = canal.email_copia,
                            nombre_envio_correo = canal.nombre_canal,

                        });
                    }
                }

                /*************************************************************************************************/
                string v_estado_planilla = (_v_planilla.codigo_estado_planilla == (int)EstadoPlanilla.abierto ? "(borrador) " : "");
                /*************************************************************************************************/

                SmtpClient smtp = new SmtpClient(_item_servidor.valor);
                smtp.Port = int.Parse(_item_puerto.valor.ToString());
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_item_usuario.valor, _item_passwor.valor);
                MailMessage email = new MailMessage();

                /*Creacion de Correo para Supervisor de Grupo*/
                contenidoCorreo = getContenidoCorreo(TemplateCorreo.Supervisor, datosCorreo);
                foreach (var item in lst_personal)
                {
                    try
                    {
                        email = new MailMessage();

                        List<reporte_detalle_liquidacion> dt_detalle = new List<reporte_detalle_liquidacion>();
                        List<reporte_detalle_liquidacion> dt_detalle_resumen = new List<reporte_detalle_liquidacion>();

                        if (_v_planilla.codigo_tipo_planilla == 1)
                        {
                            dt_detalle = dt_general.FindAll(x => x.canal_grupo_nombre == item.nombre_grupo);
                            dt_detalle_resumen = dt_general.FindAll(x => x.canal_grupo_nombre == item.nombre_grupo);

                        }
                        else if (_v_planilla.codigo_tipo_planilla == 2)
                        {
                            dt_detalle = dt_general.FindAll(x => x.codigo_personal == item.codigo_personal);
                            dt_detalle_resumen = dt_general.FindAll(x => x.codigo_personal == item.codigo_personal);
                        }

                        ReportDataSource dsDet = new ReportDataSource("dsDetalleLiquidacion", dt_detalle);
                        rptLiquidacion.DataSources.Clear();
                        rptLiquidacion.DataSources.Add(dsDet);

                        ReportDataSource dsDetResumen = new ReportDataSource("dsDetalleLiquidacion", dt_detalle_resumen);
                        rptResumenLiquidacion.DataSources.Clear();
                        rptResumenLiquidacion.DataSources.Add(dsDetResumen);

                        byte[] Bytes = rptLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle = null;
                        MemoryStream strm = new MemoryStream(Bytes);

                        byte[] BytesResumen = rptResumenLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle_resumen = null;
                        MemoryStream strmResumen = new MemoryStream(BytesResumen);

                        string v_enviar_correo = (string.IsNullOrWhiteSpace(item.nombre_envio_correo) ? "" : item.nombre_envio_correo) + " " + (string.IsNullOrWhiteSpace(item.apellido_envio_correo) ? "" : item.apellido_envio_correo);

                        Attachment atach = new Attachment(strm, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla_tipo_planilla + "_" + p_nro_pllanilla, "Listado", "pdf"));
                        Attachment atachResumen = new Attachment(strmResumen, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla_tipo_planilla + "_" + p_nro_pllanilla, "Resumen", "pdf"));

                        email.Attachments.Add(atach);
                        email.Attachments.Add(atachResumen);

                        email.To.Add(new MailAddress(item.email));

                        email.From = new MailAddress(_item_usuario.valor);
                        email.Subject = "Envío de Liquidaciones " + v_estado_planilla + "– " + planilla.nombre_regla_tipo_planilla + " " + p_nro_pllanilla;// + " – " + DateTime.Now.ToShortDateString();// _item_asunto.valor;

                        email.Body = contenidoCorreo;
                        email.IsBodyHtml = true;
                        email.Priority = MailPriority.High;
                        smtp.Send(email);
                        log.Info("Se envío correo a : " + item.email);
                        email.Attachments.Clear();
                    }
                    catch (Exception ex)
                    {
                        string mensaje = ex.Message;
                        log.Error("Ocurrio error al enviar correo a : " + item.nombres + "(" + mensaje + ")");
                    }
                    finally
                    {
                        email = null;
                    }
                }

                /*Creacion de Correo para Jefatura de Canal*/
                contenidoCorreo = getContenidoCorreo(TemplateCorreo.Jefatura, datosCorreo);
                foreach (var item in lst_jefatura)
                {
                    try
                    {
                        email = new MailMessage();

                        List<reporte_detalle_liquidacion> dt_detalle = new List<reporte_detalle_liquidacion>();
                        List<reporte_detalle_liquidacion> dt_detalle_resumen = new List<reporte_detalle_liquidacion>();

                        dt_detalle = dt_general.FindAll(x => x.codigo_canal == item.codigo_canal);
                        dt_detalle_resumen = dt_general.FindAll(x => x.codigo_canal == item.codigo_canal);

                        ReportDataSource dsDet = new ReportDataSource("dsDetalleLiquidacion", dt_detalle);
                        ReportDataSource dsDetResumen = new ReportDataSource("dsDetalleLiquidacion", dt_detalle_resumen);

                        rptLiquidacion.DataSources.Clear();
                        rptLiquidacion.DataSources.Add(dsDet);

                        rptResumenLiquidacion.DataSources.Clear();
                        rptResumenLiquidacion.DataSources.Add(dsDetResumen);

                        byte[] Bytes = rptLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle = null;
                        MemoryStream strm = new MemoryStream(Bytes);

                        byte[] BytesResumen = rptResumenLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle_resumen = null;
                        MemoryStream strmResumen = new MemoryStream(BytesResumen);

                        string v_enviar_correo = (string.IsNullOrWhiteSpace(item.nombre_envio_correo) ? "" : item.nombre_envio_correo);

                        Attachment atach = new Attachment(strm, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla_tipo_planilla + "_" + p_nro_pllanilla, "Listado", "pdf"));
                        Attachment atachResumen = new Attachment(strmResumen, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla_tipo_planilla + "_" + p_nro_pllanilla, "Resumen", "pdf"));

                        email.Attachments.Add(atach);
                        email.Attachments.Add(atachResumen);

                        email.To.Add(new MailAddress(item.email));
                        email.CC.Add(new MailAddress(item.email_copia));

                        email.From = new MailAddress(_item_usuario.valor);
                        email.Subject = "Envío de Liquidaciones " + v_estado_planilla + "– " + planilla.nombre_regla_tipo_planilla + " " + planilla.numero_planilla;// + " – " + DateTime.Now.ToShortDateString();

                        email.Body = contenidoCorreo;
                        email.IsBodyHtml = true;
                        email.Priority = MailPriority.High;
                        smtp.Send(email);
                        //log.Info("Se envío correo a : " + item.email);
                        email.Attachments.Clear();
                    }
                    catch (Exception ex)
                    {
                        string mensaje = ex.Message;
                        log.Error("Ocurrio error al enviar correo a : " + item.nombre_envio_correo + "(" + mensaje + ")");
                    }
                    finally
                    {
                        email = null;
                    }
                }
                email.Dispose();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                log.Error("Ocurrio error en envio correo: " + mensaje);
            }
        }

        private bool DebeEnviarCorreo()
        {
            var envioCorreo = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.envio_correo_planilla);
            bool retorno = false;
            try
            {
                retorno = Convert.ToBoolean(envioCorreo.valor);
            }
            catch (Exception ex)
            {
                retorno = false;
            }
            return retorno;
        }

        private string getNombreMes(string p_fecha)
        {

            var fecha = DateTime.Parse(p_fecha);



            switch (fecha.Month)
            {
                case 1:
                    return "ENERO";

                case 2:
                    return "FEBRERO";
                case 3:
                    return "MARZO";
                case 4:
                    return "ABRIL";
                case 5:
                    return "MAYO";
                case 6:
                    return "JUNIO";
                case 7:
                    return "JULIO";
                case 8:
                    return "AGOSTO";
                case 9:
                    return "SEPTIEMBRE";
                case 10:
                    return "OCTUBRE";
                case 11:
                    return "NOVIEMBRE";
                case 12:
                    return "DICIEMBRE";
            }
            return "";

        }
        private string getNombreDia(string p_fecha)
        {

            var fecha = DateTime.Parse(p_fecha);
            return fecha.ToString("dddd", new System.Globalization.CultureInfo("es-ES")).ToUpper();

        }
        private string getDia(string p_fecha)
        {

            var fecha = DateTime.Parse(p_fecha);

            return fecha.Day.ToString().PadLeft(2, '0');

        }
        private string getAnho(string p_fecha)
        {

            var fecha = DateTime.Parse(p_fecha);

            return fecha.Year.ToString();

        }

        private string getContenidoCorreo(TemplateCorreo tipo, string[] valores)
        {
            StringBuilder contenido = new StringBuilder();
            List<template_correo_dto> lstParametros = TemplateCorreoBL.Instance.ListarParametroa((int)tipo);
            template_correo_dto detalle = lstParametros.FirstOrDefault();
            int indice = 0;

            try
            {
                using (StreamReader lector = new StreamReader(Server.MapPath("~/Areas/Comision/Templates/" + detalle.nombre), System.Text.Encoding.UTF8, true))
                {
                    while (lector.Peek() > -1)
                    {
                        string linea = lector.ReadLine();
                        if (!String.IsNullOrEmpty(linea))
                        {
                            indice = 0;
                            foreach (var parametro in lstParametros)
                            {
                                linea = linea.Replace(parametro.parametro, (String.IsNullOrEmpty(valores[indice]) ? parametro.parametro : valores[indice]));
                                indice++;
                            }
                        }
                        contenido.Append(linea);
                    }

                    lector.Close();
                }
            }
            catch (Exception ex)
            {
                log.Info("Ocurrio error : " + ex.Message);
            }
            return contenido.ToString();
        }

        #endregion

        #region SECCION MANTENIMIENTO DE CUOTA

        [HttpGet]
        public PartialViewResult _Cuota_Cronograma(int codigo_detalle_cronograma)
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

        [HttpPost]
        public ActionResult _Refinanciar_Cuota(detalle_cronograma_comision_dto v_cuota)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            try
            {

                if (v_cuota.nro_cuota <= 0 && v_cuota.importe_comision <= 0)
                {

                    throw new Exception("Ingrese monto a refinanciar o nro de cuota.");
                }

                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                if (beanSesionUsuario == null)
                    throw new Exception("Se expiró su sessión, vuelva logearse.");
                v_cuota.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.RefinanciarPagoComisionCuota(v_cuota);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                vMensaje = "Se registró correctamente la operación.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult _Anular_Cuota(detalle_cronograma_comision_dto v_cuota)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_cuota.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = DetalleCronogramaPagoBL.Instance.AnularCuotaComision(v_cuota);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                vMensaje = "Se anuló correctamente la cuota.";

            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region TXT

        [HttpGet]
        public PartialViewResult _TXTContabilidad(int codigo_planilla)
        {
            TxtContabilidadViewModel vm = new TxtContabilidadViewModel();

            try
            {
                vm.codigo_planilla = codigo_planilla;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult GetResumenPlanillaTxtJson(int codigo_planilla)
        {
            List<txt_contabilidad_resumen_planilla_dto> lst = new List<txt_contabilidad_resumen_planilla_dto>();
            try
            {
                lst = PlanillaSelBL.Instance.GetResumenPlanillaTxt_Contabilidad(codigo_planilla, true);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        [HttpGet]
        public FileResult GenerarTxt(int id)
        {

            string mensaje = string.Empty;
            string numero_planilla = string.Empty;
            try
            {
                var _ruta_archivo = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.ruta_archivo_txt);
                if (_ruta_archivo == null)
                {
                    throw new Exception("La ruta del archivo txt no esta definido en la configuración de parámetros.");
                }

                List<string> selectedfiles = new List<string>();
                List<string> nombreArchivos = new List<string>();
                string nombreZip = string.Empty;

                List<cabecera_txt_dto> lst = PlanillaSelBL.Instance.GetReportePlanillaParaTxt(id, true);

                if (lst == null)
                    throw new Exception("No existe registro de planilla.");

                if (lst.Count == 0)
                    throw new Exception("No existe registro de planilla.");

                numero_planilla = lst.FirstOrDefault().numero_planilla;
                string urlPdf = string.Empty;
                string string2 = _ruta_archivo.valor.Substring(_ruta_archivo.valor.Length - 1, 1);
                if (string2 == "//" || string2 == "\\")
                {
                    urlPdf = string.Format("{0}{1}", _ruta_archivo.valor, numero_planilla);
                }
                else
                {
                    urlPdf = string.Format("{0}/{1}", _ruta_archivo.valor, numero_planilla);
                }



                //string urlPdf = Server.MapPath(string.Format("~/Areas/Comision/Reporte/Planilla/txt/{0}/", numero_planilla));

                if (!System.IO.Directory.Exists(urlPdf))
                {
                    System.IO.Directory.CreateDirectory(urlPdf);
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(urlPdf);
                    directory.GetFiles().ToList().ForEach(f => f.Delete());

                }


                var queryCodigoEmpresa = (from cabecera_txt_dto in lst
                                          group cabecera_txt_dto by cabecera_txt_dto.codigo_agrupador into newGroup
                                          orderby newGroup.Key
                                          select newGroup).ToArray();
                foreach (var item in queryCodigoEmpresa)
                {
                    List<cabecera_txt_dto> resultado = item.ToList();
                    cabecera_txt_dto cabecera = resultado.FirstOrDefault();


                    string tipoPagoMasivo = cabecera.calcular_detraccion ? "P" : "H";
                    //string referencia = "";
                    string checksum = "*";// POR VALIDAR
                    string subTipoPagoMasivo = "0";// POR VALIDAR
                    string identificadorDividendio = " ";//POR VALIDAR
                    string indicadorNotacargo = "0"; // SIEMPRE SE PASA CERO
                    int totalregistroAbonar = resultado.Count();



                    BeanCabeceraCargoPlanillaHPD beanCabeceraCargoPlanillaHPD = new BeanCabeceraCargoPlanillaHPD(
                       tipoPagoMasivo,
                       cabecera.tipo_cuenta_desembolso,//cabecera.tipo_producto_cabecera,
                       cabecera.numero_cuenta_desembolso,
                       cabecera.moneda_cuenta_desembolso,
                       double.Parse(cabecera.importe_desembolso_empresa.ToString()),
                       cabecera.fecha_proceso.Replace("/", ""),//formateaar ddmmyyyy
                       cabecera.nombre_empresa,
                       cabecera.checksum,
                       totalregistroAbonar.ToString(),
                       subTipoPagoMasivo,
                       identificadorDividendio,
                       indicadorNotacargo);

                    ///PARA PROVEEDORES
                    if (cabecera.calcular_detraccion)
                    {
                        List<BeanDetallePP> listaDetallePP = new List<BeanDetallePP>();

                        foreach (var detalle in resultado)
                        {
                            BeanDetallePP bean = new BeanDetallePP(
                                detalle.tipo_cuenta_abono,//detalle.tipo_producto,
                                detalle.numero_cuenta_abono,
                                detalle.nombre_personal,
                                detalle.moneda_cuenta_abono,
                                 double.Parse(detalle.importe_abono.ToString()),
                                detalle.nombre_tipo_documento,
                                detalle.nro_documento,
                                "F",//detalle.tipo_documento_pagar,
                                "9",//detalle.numero_documento_pagar,
                                "1",//detalle.tipo_abono,
                                " ",//referencia
                                "0");
                            listaDetallePP.Add(bean);
                        }

                        FormatoReporteTXT formato = new FormatoReporteTXT();
                        string txt_archivo = cabecera.codigo_agrupador.ToString();

                        selectedfiles.Add(txt_archivo);
                        nombreArchivos.Add(cabecera.nombre_empresa);

                        string savePath = string.Format(@"{0}\{1}.txt", urlPdf, txt_archivo);
                        formato.generaFormatoPlanillaProveedor(savePath, System.Text.Encoding.ASCII, listaDetallePP, beanCabeceraCargoPlanillaHPD);
                    }

                    /// HABERES
                    if (!cabecera.calcular_detraccion)
                    {

                        StringBuilder ArchivoIntBanCuerpo = new StringBuilder();
                        List<BeanDetalleHA> listaDetalleHA = new List<BeanDetalleHA>();
                        foreach (var detalle in resultado)
                        {
                            BeanDetalleHA bean = new BeanDetalleHA(
                                detalle.tipo_cuenta_abono.Trim(),//detalle.tipo_producto,
                                detalle.numero_cuenta_abono.Trim(),
                                detalle.nombre_personal.Trim(),
                                detalle.moneda_cuenta_abono.Trim(),
                                double.Parse(detalle.importe_abono.ToString()),
                                 cabecera.nombre_empresa.Trim(),//referencia
                                detalle.nombre_tipo_documento.Trim(),
                                detalle.nro_documento.Trim(),
                                "0");

                            listaDetalleHA.Add(bean);
                        }

                        FormatoReporteTXT formato = new FormatoReporteTXT();
                        string txt_archivo = cabecera.codigo_agrupador;

                        selectedfiles.Add(txt_archivo);
                        nombreArchivos.Add(cabecera.nombre_empresa);

                        string savePath = string.Format(@"{0}\{1}.txt", urlPdf, txt_archivo);
                        formato.generaFormatoPlanillaHaberes(savePath, System.Text.Encoding.UTF8, listaDetalleHA, beanCabeceraCargoPlanillaHPD);
                    }
                }

                string name_zip = DateTime.Now.ToString("dd-mm-yyyy");
                string savePathZip = string.Format(@"{0}\{1}.zip", urlPdf, name_zip);
                savePathZip = savePathZip.Replace("\\\\", "\\");
                ZipArchive zip = ZipFile.Open(savePathZip, ZipArchiveMode.Create);
                string nombre_zip_download = EvaluarNombreZip(nombreArchivos);

                foreach (string file in selectedfiles)
                {
                    zip.CreateEntryFromFile(string.Format("{0}/{1}", urlPdf, string.Format("{0}.txt", file)), string.Format("{0}.txt", file));
                }
                zip.Dispose();

                return File(savePathZip, "application/zip", string.Format("{0}-{1}.zip", numero_planilla, nombre_zip_download));

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return null;


        }

        private string EvaluarNombreZip(List<string> archivos)
        {
            string retorno = string.Empty;

            foreach (string empresa in archivos)
            {
                if (retorno.IndexOf(empresa + "|") == -1)
                {
                    retorno = (string.IsNullOrWhiteSpace(retorno) ? "" : retorno + "-") + empresa + "|";
                }
            }
            return retorno.Replace("|", "");
        }

        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        [HttpGet]
        public FileResult GenerarTxtContabilidad(int codigo_planilla, int codigo_empresa)
        {

            string mensaje = string.Empty;
            string numero_planilla = codigo_planilla.ToString() + "_" + codigo_empresa.ToString();
            try
            {
                var _ruta_archivo = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.ruta_archivo_txt);
                if (_ruta_archivo == null)
                {
                    throw new Exception("La ruta del archivo txt no esta definido en la configuración de parámetros.");
                }
                List<string> selectedfiles = new List<string>();
                List<txt_contabilidad_planilla_dto> lst = PlanillaSelBL.Instance.GetPlanillaTxt_Contabilidad(codigo_planilla, codigo_empresa, true);

                if (lst == null)
                    throw new Exception("No existe registro de planilla.");

                if (lst.Count == 0)
                    throw new Exception("No existe registro de planilla.");

                string urlTXT = string.Empty;
                string string2 = _ruta_archivo.valor.Substring(_ruta_archivo.valor.Length - 1, 1);

                if (string2 == "//" || string2 == "\\")
                {
                    urlTXT = string.Format("{0}{1}", _ruta_archivo.valor, numero_planilla);
                }
                else
                {
                    urlTXT = string.Format("{0}/{1}", _ruta_archivo.valor, numero_planilla);
                }

                if (!System.IO.Directory.Exists(urlTXT))
                {
                    System.IO.Directory.CreateDirectory(urlTXT);
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(urlTXT);
                    directory.GetFiles().ToList().ForEach(f => f.Delete());
                }

                FormatoReporteTXT formato = new FormatoReporteTXT();
                string txt_archivo = "COMISIONES";

                selectedfiles.Add(txt_archivo);
                string savePath = string.Format(@"{0}\{1}.txt", urlTXT, txt_archivo);
                formato.generarArchivoContabilidad(savePath, System.Text.Encoding.Unicode, lst);

                return File(savePath, "application/txt", txt_archivo + ".txt");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
        #endregion

        /**/
        public ActionResult SetDataExcel(string p_codigo_planilla)
        {
            //Guid id = Guid.NewGuid();
            //string v_guid = id.ToString().Replace('-', '_');
            //Session[v_guid + "_data"] = ReporteGeneralBL.Instance.Finanzas(busqueda); ;
            //Session[v_guid + "_filtro"] = ReporteGeneralBL.Instance.FinanzasFiltro(busqueda); ;

            return Json(new { p_codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarExcel(string p_codigo_planilla)
        {
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";
            string rutaRDLC = string.Empty;
            string nombreEXCEL = string.Empty;
            string nombreTipoReporte = string.Empty;

            List<reporte_liquidacion_Supervisor> lst = new List<reporte_liquidacion_Supervisor>();
            //reporte_finanzas_filtro_dto filtro = new reporte_finanzas_filtro_dto();
            try
            {
                lst = PlanillaSelBL.Instance.ReporteDetalleLiquidacionPlanillaSupervisor(int.Parse(p_codigo_planilla)); //Session[id + "_data"] as List<reporte_finanzas_dto>;
                //filtro = Session[id + "_filtro"] as reporte_finanzas_filtro_dto;

                //reporte_finanzas_dto detalle = lst.FirstOrDefault();

                nombreEXCEL = "ReporteLiquidacionSupervisor_Comision"; //+(detalle.tipo == (int)Utils.ReporteFinanzas.comision ? "_Comision" : "_Bono");

                //if (detalle.resumen_detalle == (int)Utils.ReporteFinanzasTipoSumatoria.resumen)
                //{
                //    rutaRDLC = "~/Areas/Comision/Reporte/ReporteFinanzas/rdl/rpt_reporte_finanzas.rdlc";
                //    nombreEXCEL += ".xls";
                //}
                //else
                //{
                //    rutaRDLC = "~/Areas/Comision/Reporte/ReporteFinanzas/rdl/rpt_reporte_finanzas_detalle.rdlc";
                //    nombreEXCEL += "_Detalle.xls";
                //}

                rutaRDLC = "~/Areas/Comision/Reporte/Planilla/rdl/rpt_liquidacion_resumen_supervisor.rdlc";
                nombreEXCEL += ".xls";

                //if (detalle.tipo == (int)Utils.ReporteFinanzas.comision)
                //{
                //    if (detalle.tipo_reporte == (int)Utils.ReporteFinanzasTipo.generado)
                //    {
                //        nombreTipoReporte = " - " + Utils.ReporteFinanzasTipo.generado.ToString().ToUpper();
                //    }
                //    else
                //    {
                //        nombreTipoReporte = " - " + Utils.ReporteFinanzasTipo.pagado.ToString().ToUpper();
                //    }
                //}
                //else
                //{
                //    nombreTipoReporte = "";
                //}

                ReportDataSource dataSource = new ReportDataSource("dsLiquidacionSupervisor", lst);
                LocalReport rpt = new LocalReport
                {
                    ReportPath = Server.MapPath(rutaRDLC)
                };

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                //List<ReportParameter> parametros = new List<ReportParameter>();
                //ReportParameter prmtTipo = new ReportParameter("p_titulo_reporte", "hola");
                //ReportParameter prmtCanal = new ReportParameter("Canal", filtro.canal);
                //ReportParameter prmtTipoPlanilla = new ReportParameter("TipoPlanilla", filtro.tipo_planilla);
                //ReportParameter prmtTipoReporte = new ReportParameter("TipoReporte", filtro.tipo_reporte);
                //ReportParameter prmtPeriodo = new ReportParameter("Periodo", filtro.periodo);
                //ReportParameter prmtAnio = new ReportParameter("Anio", filtro.anio);

                //parametros.Add(prmtTipo);
                //parametros.Add(prmtCanal);
                //parametros.Add(prmtTipoPlanilla);
                //parametros.Add(prmtTipoReporte);
                //parametros.Add(prmtPeriodo);
                //parametros.Add(prmtAnio);

                //rpt.SetParameters(parametros);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, nombreEXCEL);


            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            finally
            {
                //Session.Remove(id);
            }
            return null;
        }
        /**/

    }
}
