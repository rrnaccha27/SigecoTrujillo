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
using Microsoft.Reporting.WebForms;
using System.Data;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class PlanillaBonoJNController : Controller
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
        public PlanillaBonoJNController() {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _IParametroSistemaService = new ParametroSistemaService();
        }

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            //bool acceso = false;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

                //List<menu_dto> _lst_menu = UsuarioSelBL.Instance.GetMenuPrincipalByPerfilJson(beanSesionUsuario.codigoPerfil);
                //List<menu_dto> _lst_permitido = _lst_menu.FindAll(x => x.tipo_orden == 1);
                
                //foreach (menu_dto _opcion in _lst_permitido)
                //{
                //    if ("/" + _opcion.ruta_menu.ToLower() == Request.FilePath.ToString().ToLower())
                //    {
                //        acceso = true;
                //    }
                //}
                //if (!acceso)
                //{
                //    return Redirect("/Home/Index");
                //}
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

            ///planilla bono
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            PlanillaViewModel vm = new PlanillaViewModel();
            
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            var tipo_planilla = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.planilla_bono_jn_tipo_planilla);
            var canales = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.planilla_bono_jn_canales);

            if (p_codigo_planilla > 0)
            {
                vm.planilla_bono = PlanillaSelBL.Instance.BuscarPlanillaBonoById(p_codigo_planilla);
            }
            else
            {
                var planilla = new Planilla_bono_dto();
                planilla.fecha_apertura = DateTime.Now;
                planilla.usuario_apertura = beanSesionUsuario.codigoUsuario;
                planilla.codigo_tipo_planilla = Convert.ToInt32(tipo_planilla.valor);
                planilla.codigos_canales = canales.valor;
                vm.planilla_bono = planilla;
            }

            vm.bean = bean;
            return PartialView(vm);
        }

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
        [RequiresAuthentication]
        public ActionResult Registrar(Planilla_bono_dto v_planilla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            long p_codigo_planilla = 0;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;                
                MensajeDTO mensaje = PlanillaBL.Instance.GenerarPlanillaBono(v_planilla, true);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                p_codigo_planilla = mensaje.idRegistro;

                if (mensaje.total_registro_afectado < 1)
                {
                    vResultado = -1;
                    vMensaje = "";
                }
                else
                { vMensaje = "Se generó satisfactoriamente la planilla, total de registros procesados es " + mensaje.total_registro_afectado; }
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Cerrar(Planilla_bono_dto v_planilla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.CerrarPlanillaBono(v_planilla);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

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
        [RequiresAuthentication]
        public ActionResult Anular(int p_codigo_planilla)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            long v_codigo_planilla = 0;
            try
            {
                Planilla_bono_dto v_planilla = new Planilla_bono_dto();
                v_planilla.codigo_planilla = p_codigo_planilla;
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.AnularPlanillaBono(v_planilla);
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

        [HttpPost]
        public ActionResult GetPagosHabilitadosJson(int codigo_planilla)
        {
            List<detalle_planilla_resumen_dto> lst = new List<detalle_planilla_resumen_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarByIdPlanillaBono(codigo_planilla, true);
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
                lista = PlanillaSelBL.Instance.ListarPlanillaBono(true);
                v_total = lista.Count();
            }
            catch (Exception ex)
            {
               string mensaje=ex.ToString();                
            }

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = lista }), "application/json");
        }

        [HttpGet]
        public ActionResult _Reporte_Planilla(int p_codigo_planilla)
        {
            ReporteViewModel vm = new ReporteViewModel();
            string vUrl = string.Empty;
            try
            {
                vUrl = Url.Action("frmPlanillaBonoJN", "Areas/Comision/Reporte/PlanillaBonoJN/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla; 
                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            
            return PartialView("_Reporte",vm);
        }

        public ActionResult _Reporte_Liquidacion(int p_codigo_planilla) 
        {
            ReporteViewModel vm = new ReporteViewModel();
             string vUrl = string.Empty;
            try
            {
                vUrl = Url.Action("frmliquidacionjn", "Areas/Comision/Reporte/PlanillaBonoJN/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla;
                
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
                lst = new CanalGrupoBL().Listar_Canal_Planilla_Bono(true);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        [HttpPost]
        public ActionResult GetResumenPlanillaTxtJson(int codigo_planilla)
        {
            List<txt_contabilidad_resumen_planilla_dto> lst = new List<txt_contabilidad_resumen_planilla_dto>();
            try
            {
                lst = PlanillaSelBL.Instance.GetResumenPlanillaBonoTxt_Contabilidad(codigo_planilla);
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
                List<cabecera_txt_dto> lst = PlanillaSelBL.Instance.GetPlanillaBonoParaTxt(id);

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
                                          group cabecera_txt_dto by cabecera_txt_dto.codigo_empresa into newGroup
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
                        string txt_archivo = cabecera.nombre_empresa;

                        selectedfiles.Add(txt_archivo);
                        string savePath = string.Format(@"{0}\{1}.txt", urlPdf, txt_archivo);
                        formato.generaFormatoPlanillaProveedor(savePath, System.Text.Encoding.UTF8, listaDetallePP, beanCabeceraCargoPlanillaHPD);

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
                        string txt_archivo = cabecera.nombre_empresa;

                        selectedfiles.Add(txt_archivo);
                        string savePath = string.Format(@"{0}\{1}.txt", urlPdf, txt_archivo);
                        formato.generaFormatoPlanillaHaberes(savePath, System.Text.Encoding.UTF8, listaDetalleHA, beanCabeceraCargoPlanillaHPD);

                    }

                }

                string name_zip = DateTime.Now.ToString("dd-mm-yyyy");
                string savePathZip = string.Format(@"{0}\{1}.zip", urlPdf, name_zip);
                savePathZip = savePathZip.Replace("\\\\", "\\");
                ZipArchive zip = ZipFile.Open(savePathZip, ZipArchiveMode.Create);
                string nombre_zip_download = string.Empty;
                foreach (string file in selectedfiles)
                {
                    nombre_zip_download = (string.IsNullOrWhiteSpace(nombre_zip_download) ? "" : nombre_zip_download + "-") + file;
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
                List<txt_contabilidad_planilla_dto> lst = PlanillaSelBL.Instance.GetPlanillaBonoTxt_Contabilidad(codigo_planilla, codigo_empresa);

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
                string txt_archivo = "BONOS";

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

        [HttpPost]
        public ActionResult Excluir_Pago(detalle_planilla_bono_exclusion_dto v_detalle)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_detalle.excluido_usuario= beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.AnularPagoBono(v_detalle);
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                vMensaje = "Se excluyó correctamente el pago.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarExcel(string id)
        {
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";

            try
            {
                DataTable dt_planilla = PlanillaSelBL.Instance.ReportePlanillaBonoJNContabilidad(Convert.ToInt32(id));
                DataTable dt_detalle = PlanillaSelBL.Instance.ReportePlanillaBonoJN(Convert.ToInt32(id));

                ReportDataSource planilla = new ReportDataSource("dsContabilidad", dt_planilla);
                ReportDataSource detalle = new ReportDataSource("dsDetalle", dt_detalle);
                
                LocalReport rpt = new LocalReport
                {
                    ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBonoJN/rdl/resumen_contabilidad.rdlc")
                };

                rpt.DataSources.Clear();
                rpt.DataSources.Add(planilla);
                rpt.DataSources.Add(detalle);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, "ResumenContabilidad.xls");
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            finally
            {
                Session.Remove(id);
            }
            return null;
        }

    }
}
