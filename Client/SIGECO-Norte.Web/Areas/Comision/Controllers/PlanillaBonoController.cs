using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.Areas.Comision.Utils;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using log4net;
using Microsoft.Reporting.WebForms;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class PlanillaBonoController : Controller
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
        public PlanillaBonoController() {
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

        #region Operaciones de Planilla

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
                MensajeDTO mensaje = PlanillaBL.Instance.GenerarPlanillaBono(v_planilla);
                
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
                
                p_codigo_planilla = mensaje.idRegistro;

                if (mensaje.total_registro_afectado < 1)
                {
                    vResultado = -1;
                    vMensaje = "Para el rango de fecha establecido no existe pagos habilitados.";
                }
                else
                {
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
                        ex2.ToString();
                    }

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

                try
                {
                    string[] parameters = new string[2];
                    parameters[0] = v_planilla.codigo_planilla.ToString();
                    parameters[1] = "";

                    Thread SendingThreads = new Thread(fnEnviarEmail);
                    SendingThreads.Start(parameters);
                }
                catch (Exception ex2)
                {
                    ex2.ToString();
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

        #endregion

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
                v_total = lista.Count();
            }
            catch (Exception ex)
            {
               string mensaje=ex.ToString();                
            }

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = lista }), "application/json");
        }

        #region Reportes
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

        public ActionResult _Reporte_Liquidacion_Resumen(int p_codigo_planilla)
        {
            ReporteViewModel vm = new ReporteViewModel();
            string vUrl = string.Empty;
            try
            {
                vUrl = Url.Action("frm_liquidacion_bono_resumen", "Areas/Comision/Reporte/PlanillaBono/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla;
                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView("_Reporte", vm);
        }

        #endregion

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

        #region ENVIAR CORREO

        LocalReport rptLiquidacion;
        LocalReport rptResumenLiquidacion;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PlanillaController));
        //ILog log = log4net.LogManager.GetLogger(typeof(PlanillaController));
        [HttpPost]
        public ActionResult _Enviar_Correo_Liquidacion(int p_codigo_planilla, string p_nro_pllanilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                List<reporte_detalle_liquidacion_bono> dt_general = PlanillaSelBL.Instance.ReporteLiquidacionBonoIndividual(p_codigo_planilla);
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
                int p_codigo_planilla = int.Parse((string)arrayParameters.GetValue(0));
                string p_nro_pllanilla = (string)arrayParameters.GetValue(1);

                List<personal_correo> lst_personal = new List<personal_correo>();
                List<personal_jefatura_correo> lst_jefatura = new List<personal_jefatura_correo>();
                List<canal_jefatura_dto> lst_canal_jefatura = new CanalGrupoBL().ListarJefatura();
                List<reporte_detalle_liquidacion_bono> dt_general = PlanillaSelBL.Instance.ReporteLiquidacionBonoIndividual(p_codigo_planilla);
                Planilla_bono_dto planilla = PlanillaSelBL.Instance.BuscarPlanillaBonoById(p_codigo_planilla);

                if (!planilla.envio_liquidacion)
                {
                    return;
                }

                var _item_usuario = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_usuario);
                var _item_passwor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_password);
                var _item_servidor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_servidor);
                var _item_puerto = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_puerto);
                var _item_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.porcentaje_detraccion);

                //var _v_planilla = dt_general.FirstOrDefault();
                //v_titulo = "CORRESPONDIENTE AL PERIODO DEL " + getNombreDia(_v_planilla.fecha_inicio) + " " + getDia(_v_planilla.fecha_inicio) + " de " + getNombreMes(_v_planilla.fecha_inicio) + " del " + getAnho(_v_planilla.fecha_inicio) +
                //    " AL " + getNombreDia(_v_planilla.fecha_fin) + " " + getDia(_v_planilla.fecha_fin) + " de " + getNombreMes(_v_planilla.fecha_fin) + " del " + getAnho(_v_planilla.fecha_fin);

                p_nro_pllanilla = (String.IsNullOrEmpty(p_nro_pllanilla) ? planilla.numero_planilla : p_nro_pllanilla);

                string[] datosCorreo = new string[(int)TemplateCorreoParametros.total_enum];
                datosCorreo[(int)TemplateCorreoParametros.planilla] = "Bono";
                datosCorreo[(int)TemplateCorreoParametros.numero_planilla] = planilla.numero_planilla;
                datosCorreo[(int)TemplateCorreoParametros.cuenta] = _item_usuario.valor;
                datosCorreo[(int)TemplateCorreoParametros.fecha] = DateTime.Now.ToShortDateString() + " " + String.Format("{0:T}", DateTime.Now);

                if (rptLiquidacion == null)
                {
                    rptLiquidacion = new LocalReport();
                    rptLiquidacion.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBono/rdl/rpt_liquidacion_bono_supervisor_individual.rdlc");
                }

                if (rptResumenLiquidacion == null)
                {
                    rptResumenLiquidacion = new LocalReport();
                    rptResumenLiquidacion.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBono/rdl/rpt_liquidacion_bono_resumen.rdlc");
                }

                //ReportParameter p_porcentaje_detraccion = new ReportParameter("p_porcentaje_detraccion", _item_detraccion.valor);
                //ReportParameter p_titulo_reporte = new ReportParameter("p_titulo_reporte", v_titulo);

                //rptLiquidacion.SetParameters(new ReportParameter[] { p_porcentaje_detraccion, p_titulo_reporte });
                //rptResumenLiquidacion.SetParameters(new ReportParameter[] { p_porcentaje_detraccion, p_titulo_reporte });

                foreach (var item in dt_general)
                {
                    if (!lst_personal.Exists(x => x.codigo_personal == item.codigo_grupo))
                    {
                        lst_personal.Add(new personal_correo()
                        {
                            nombres = item.nombre_supervisor,
                            codigo_personal = item.codigo_grupo,
                            email = item.email_supervisor,
                            nombre_envio_correo = item.nombre_supervisor,
                            apellido_envio_correo = "",
                            nombre_grupo = item.nombre_grupo
                        });
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
                string v_estado_planilla = (planilla.codigo_estado_planilla == (int)EstadoPlanilla.abierto ? "(borrador) " : "");
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

                        List<reporte_detalle_liquidacion_bono> dt_detalle = new List<reporte_detalle_liquidacion_bono>();
                        List<reporte_detalle_liquidacion_bono> dt_detalle_resumen = new List<reporte_detalle_liquidacion_bono>();

                        dt_detalle = dt_general.FindAll(x => x.nombre_grupo == item.nombre_grupo);
                        dt_detalle_resumen = dt_general.FindAll(x => x.nombre_grupo == item.nombre_grupo);

                        ReportDataSource dsDet = new ReportDataSource("dsLiquidacionBonoSupervisorIndividual", dt_detalle);
                        rptLiquidacion.DataSources.Clear();
                        rptLiquidacion.DataSources.Add(dsDet);

                        ReportDataSource dsDetResumen = new ReportDataSource("dsLiquidacionBonoSupervisorIndividual", dt_detalle_resumen);
                        rptResumenLiquidacion.DataSources.Clear();
                        rptResumenLiquidacion.DataSources.Add(dsDetResumen);

                        byte[] Bytes = rptLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle = null;
                        MemoryStream strm = new MemoryStream(Bytes);

                        byte[] BytesResumen = rptResumenLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle_resumen = null;
                        MemoryStream strmResumen = new MemoryStream(BytesResumen);

                        string v_enviar_correo = (string.IsNullOrWhiteSpace(item.nombre_envio_correo) ? "" : item.nombre_envio_correo);

                        Attachment atach = new Attachment(strm, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, "BONO" + "_" + p_nro_pllanilla, "Listado", "pdf"));
                        Attachment atachResumen = new Attachment(strmResumen, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, "BONO" + "_" + p_nro_pllanilla, "Resumen", "pdf"));

                        email.Attachments.Add(atach);
                        email.Attachments.Add(atachResumen);

                        email.To.Add(new MailAddress(item.email));

                        email.From = new MailAddress(_item_usuario.valor);
                        email.Subject = "Envío de Liquidaciones " + v_estado_planilla + "– " + "BONO" + " " + p_nro_pllanilla;// + " – " + DateTime.Now.ToShortDateString();// _item_asunto.valor;

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
                        log.Error("Ocurrio error al enviar  correo a : " + item.nombres + "(" + mensaje + ")");
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

                        List<reporte_detalle_liquidacion_bono> dt_detalle = new List<reporte_detalle_liquidacion_bono>();
                        List<reporte_detalle_liquidacion_bono> dt_detalle_resumen = new List<reporte_detalle_liquidacion_bono>();

                        dt_detalle = dt_general.FindAll(x => x.codigo_canal == item.codigo_canal);
                        dt_detalle_resumen = dt_general.FindAll(x => x.codigo_canal == item.codigo_canal);

                        ReportDataSource dsDet = new ReportDataSource("dsLiquidacionBonoSupervisorIndividual", dt_detalle);
                        ReportDataSource dsDetResumen = new ReportDataSource("dsLiquidacionBonoSupervisorIndividual", dt_detalle_resumen);

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

                        Attachment atach = new Attachment(strm, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, "BONO" + "_" + p_nro_pllanilla, "Listado", "pdf"));
                        Attachment atachResumen = new Attachment(strmResumen, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, "BONO" + "_" + p_nro_pllanilla, "Resumen", "pdf"));

                        email.Attachments.Add(atach);
                        email.Attachments.Add(atachResumen);

                        email.To.Add(new MailAddress(item.email));
                        email.CC.Add(new MailAddress(item.email_copia));

                        email.From = new MailAddress(_item_usuario.valor);
                        email.Subject = "Envío de Liquidaciones " + v_estado_planilla + "– " + "BONO" + " " + planilla.numero_planilla;// + " – " + DateTime.Now.ToShortDateString();

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
                        log.Error("Ocurrio error al enviar  correo a : " + item.nombre_envio_correo + "(" + mensaje + ")");
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
            }
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
                lst = PlanillaSelBL.Instance.GetResumenPlanillaBonoTxt_Contabilidad(codigo_planilla, true);
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
                List<cabecera_txt_dto> lst = PlanillaSelBL.Instance.GetPlanillaBonoParaTxt(id, true);

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
                List<txt_contabilidad_planilla_dto> lst = PlanillaSelBL.Instance.GetPlanillaBonoTxt_Contabilidad(codigo_planilla, codigo_empresa, true);

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

        #endregion

        /**/
        public ActionResult SetDataExcel(string p_codigo_planilla)
        {
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

            try
            {
                lst = PlanillaSelBL.Instance.ReporteLiquidacionPlanillaBonoSupervisor(int.Parse(p_codigo_planilla)); //Session[id + "_data"] as List<reporte_finanzas_dto>;

                nombreEXCEL = "ReporteLiquidacionSupervisor_Bono";

                rutaRDLC = "~/Areas/Comision/Reporte/Planilla/rdl/rpt_liquidacion_resumen_supervisor.rdlc";
                nombreEXCEL += ".xls";

                ReportDataSource dataSource = new ReportDataSource("dsLiquidacionSupervisor", lst);
                LocalReport rpt = new LocalReport
                {
                    ReportPath = Server.MapPath(rutaRDLC)
                };

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

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
