using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Threading;

using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.Areas.Comision.Utils;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using log4net;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class PlanillaBonoTrimestralController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PlanillaBonoTrimestralController));
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly IParametroSistemaService _IParametroSistemaService;
        private readonly TipoPlanillaService _tipoPlanillaService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        
        //
        // GET: /Comision/PlanillaBono/
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public PlanillaBonoTrimestralController() {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _tipoPlanillaService = new TipoPlanillaService();
            _IParametroSistemaService = new ParametroSistemaService();
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

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult _Buscar()
        {
            int v_total = 0;
            var lista = new List<grilla_planilla_bono_trimestral>();
            try
            {
                lista = PlanillaSelBL.Instance.ListarPlanillaBonoTrimestral();
                v_total = lista.Count();
            }
            catch (Exception ex)
            {
                string mensaje = ex.ToString();
            }

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = lista }), "application/json");
        }

        #region COMBOS

        public ActionResult GetReglaBonoTrimestralJson()
        {
            List<combo_regla_bono_trimestral_dto> lst = new List<combo_regla_bono_trimestral_dto>();
            try
            {
                lst = new ReglaBonoTrimestralBL().GetAllComboJson();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        public ActionResult GetPeriodoTrimestralJson()
        {
            List<combo_periodo_trimestral_dto> lst = new List<combo_periodo_trimestral_dto>();
            try
            {
                lst = new PeriodoTrimestralBL().GetAllComboJson();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._tipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        #endregion

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
                vm.planilla_bono_trimestral = PlanillaSelBL.Instance.BuscarPlanillaBonoTrimestralById(p_codigo_planilla);
            }
            else
            {
                var planilla = new planilla_bono_trimestral_dto();
                planilla.fecha_apertura = DateTime.Now;
                planilla.usuario_apertura = beanSesionUsuario.codigoUsuario;
                vm.planilla_bono_trimestral = planilla;
            }

            vm.bean = bean;
            return PartialView(vm);
        }

        #region Opciones de Planilla

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Registrar(planilla_bono_trimestral_dto v_planilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            long p_codigo_planilla = 0;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_registra = beanSesionUsuario.codigoUsuario;                
                MensajeDTO mensaje = PlanillaBL.Instance.GenerarPlanillaBonoTrimestral(v_planilla);

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
                { 
                    vMensaje = "Se generó satisfactoriamente la planilla, total de registros procesados es " + mensaje.total_registro_afectado; 
                }

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
                    string mensaje2 = ex2.Message;
                }
            }
            catch (Exception ex)
            {
                vResultado = -2;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_planilla = p_codigo_planilla }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Cerrar(planilla_bono_trimestral_dto v_planilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_cierre = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.CerrarPlanillaBonoTrimestral(v_planilla);
                
                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se cerró satisfactoriamente la planilla.";

                try
                {
                    string[] parameters = new string[2];
                    parameters[0] = v_planilla.codigo_planilla .ToString();
                    parameters[1] = "";

                    Thread SendingThreads = new Thread(fnEnviarEmail);
                    SendingThreads.Start(parameters);
                }
                catch (Exception ex2)
                {
                    string mensaje2 = ex2.Message;
                }
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
                planilla_bono_trimestral_dto v_planilla = new planilla_bono_trimestral_dto();
                v_planilla.codigo_planilla = p_codigo_planilla;
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                v_planilla.usuario_anulacion = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = PlanillaBL.Instance.AnularPlanillaBonoTrimestral(v_planilla);
                
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
            List<detalle_planilla_bono_trimestral_dto> lst = new List<detalle_planilla_bono_trimestral_dto>();
            try
            {

                lst = DetallePlanillaSelBL.Instance.ListarByIdPlanillaBonoTrimestral(codigo_planilla);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");

        }

        [HttpGet]
        public ActionResult _Reporte(int p_codigo_planilla, int tipo_reporte)
        {
            ReporteViewModel vm = new ReporteViewModel();
            string vUrl = string.Empty;
            string formulario = string.Empty;

            switch (tipo_reporte)
            {
                case (int)ReporteBonoTrimestral.planilla:
                    formulario = "frmPlanillaBonoTrimestral";
                    break;
                case (int)ReporteBonoTrimestral.liquidacion:
                    formulario = "frmLiquidacionBonoTrimestral";
                    break;
                case (int)ReporteBonoTrimestral.resumen_liquidacion:
                    formulario = "frmResumenLiquidacionBonoTrimestral";
                    break;
            }

            try
            {
                vUrl = Url.Action(formulario, "Areas/Comision/Reporte/PlanillaBonoTrimestral/frm", new { area = string.Empty }) + ".aspx?p_codigo_planilla=" + p_codigo_planilla;
                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }

            return PartialView("_Reporte", vm);
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


        LocalReport rptLiquidacion;
        LocalReport rptResumenLiquidacion;

        public ActionResult _Enviar_Correo_Liquidacion(int p_codigo_planilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                string[] parameters = new string[2];
                parameters[0] = p_codigo_planilla.ToString();

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

                List<personal_correo> lst_personal = new List<personal_correo>();
                List<personal_jefatura_correo> lst_jefatura = new List<personal_jefatura_correo>();
                List<canal_jefatura_dto> lst_canal_jefatura = new CanalGrupoBL().ListarJefatura();
                List<detalle_liquidacion_planilla_bono_trimestral_dto> dt_general = DetallePlanillaSelBL.Instance.ReporteBonoTrimestralLiquidacionLst(p_codigo_planilla);
                planilla_bono_trimestral_dto planilla = PlanillaSelBL.Instance.BuscarPlanillaBonoTrimestralById(p_codigo_planilla);

                var _item_usuario = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_usuario);
                var _item_passwor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_password);
                var _item_servidor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_servidor);
                var _item_puerto = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_puerto);
                //var _item_detraccion = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.porcentaje_detraccion);

                var _v_planilla = dt_general.FirstOrDefault();
                string p_nro_pllanilla = planilla.numero_planilla;

                string[] datosCorreo = new string[(int)TemplateCorreoParametros.total_enum];
                datosCorreo[(int)TemplateCorreoParametros.planilla] = planilla.nombre_regla;//todo
                datosCorreo[(int)TemplateCorreoParametros.numero_planilla] = planilla.numero_planilla;
                datosCorreo[(int)TemplateCorreoParametros.cuenta] = _item_usuario.valor;
                datosCorreo[(int)TemplateCorreoParametros.fecha] = DateTime.Now.ToShortDateString() + " " + String.Format("{0:T}", DateTime.Now);

                if (rptLiquidacion == null)
                {
                    rptLiquidacion = new LocalReport();
                    rptLiquidacion.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBonoTrimestral/rdlc/rpt_liquidacion.rdlc");
                }

                if (rptResumenLiquidacion == null)
                {
                    rptResumenLiquidacion = new LocalReport();
                    rptResumenLiquidacion.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/PlanillaBonoTrimestral/rdlc/rpt_resumen_liquidacion.rdlc");
                }

                foreach (var item in dt_general)
                {
                    if (!lst_personal.Exists(x => x.codigo_personal == item.codigo_supervisor))
                    {
                        lst_personal.Add(new personal_correo()
                        {
                            nombres = item.nombre_supervisor,
                            codigo_personal = item.codigo_supervisor,
                            email = item.correo_supervisor,
                            nombre_envio_correo = item.nombre_supervisor,
                            apellido_envio_correo = string.Empty,
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

                        List<detalle_liquidacion_planilla_bono_trimestral_dto> dt_detalle = new List<detalle_liquidacion_planilla_bono_trimestral_dto>();
                        List<detalle_liquidacion_planilla_bono_trimestral_dto> dt_detalle_resumen = new List<detalle_liquidacion_planilla_bono_trimestral_dto>();

                        dt_detalle = dt_general.FindAll(x => x.nombre_grupo == item.nombre_grupo);
                        dt_detalle_resumen = dt_general.FindAll(x => x.nombre_grupo == item.nombre_grupo);


                        ReportDataSource dsDet = new ReportDataSource("dsLiquidacion", dt_detalle);
                        rptLiquidacion.DataSources.Clear();
                        rptLiquidacion.DataSources.Add(dsDet);

                        ReportDataSource dsDetResumen = new ReportDataSource("dsLiquidacion", dt_detalle_resumen);
                        rptResumenLiquidacion.DataSources.Clear();
                        rptResumenLiquidacion.DataSources.Add(dsDetResumen);

                        byte[] Bytes = rptLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle = null;
                        MemoryStream strm = new MemoryStream(Bytes);

                        byte[] BytesResumen = rptResumenLiquidacion.Render(format: "PDF", deviceInfo: "");
                        dt_detalle_resumen = null;
                        MemoryStream strmResumen = new MemoryStream(BytesResumen);

                        string v_enviar_correo = (string.IsNullOrWhiteSpace(item.nombre_envio_correo) ? "" : item.nombre_envio_correo);

                        Attachment atach = new Attachment(strm, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla + "_" + p_nro_pllanilla, "Listado", "pdf"));
                        Attachment atachResumen = new Attachment(strmResumen, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla + "_" + p_nro_pllanilla, "Resumen", "pdf"));

                        email.Attachments.Add(atach);
                        email.Attachments.Add(atachResumen);

                        email.To.Add(new MailAddress(item.email));

                        email.From = new MailAddress(_item_usuario.valor);
                        email.Subject = "Envío de Liquidaciones " + v_estado_planilla + "– " + planilla.nombre_regla + " " + p_nro_pllanilla;// + " – " + DateTime.Now.ToShortDateString();// _item_asunto.valor;

                        email.Body = contenidoCorreo;
                        email.IsBodyHtml = true;
                        email.Priority = MailPriority.High;
                        smtp.Send(email);
                        log.Info("Se envío correo a : " + item.email + " - " + planilla.nombre_regla + " " + p_nro_pllanilla);
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

                        List<detalle_liquidacion_planilla_bono_trimestral_dto> dt_detalle = new List<detalle_liquidacion_planilla_bono_trimestral_dto>();
                        List<detalle_liquidacion_planilla_bono_trimestral_dto> dt_detalle_resumen = new List<detalle_liquidacion_planilla_bono_trimestral_dto>();

                        dt_detalle = dt_general.FindAll(x => x.codigo_canal == item.codigo_canal);
                        dt_detalle_resumen = dt_general.FindAll(x => x.codigo_canal == item.codigo_canal);

                        ReportDataSource dsDet = new ReportDataSource("dsLiquidacion", dt_detalle);
                        ReportDataSource dsDetResumen = new ReportDataSource("dsLiquidacion", dt_detalle_resumen);

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

                        Attachment atach = new Attachment(strm, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla + "_" + p_nro_pllanilla, "Listado", "pdf"));
                        Attachment atachResumen = new Attachment(strmResumen, string.Format("{0}_{1}_{2}.{3}", v_enviar_correo, planilla.nombre_regla + "_" + p_nro_pllanilla, "Resumen", "pdf"));

                        email.Attachments.Add(atach);
                        email.Attachments.Add(atachResumen);

                        email.To.Add(new MailAddress(item.email));
                        email.CC.Add(new MailAddress(item.email_copia));

                        email.From = new MailAddress(_item_usuario.valor);
                        email.Subject = "Envío de Liquidaciones " + v_estado_planilla + "– " + planilla.nombre_regla + " " + planilla.numero_planilla;// + " – " + DateTime.Now.ToShortDateString();

                        email.Body = contenidoCorreo;
                        email.IsBodyHtml = true;
                        email.Priority = MailPriority.High;
                        smtp.Send(email);
                        log.Info("Se envío correo a : " + item.email + " – " + planilla.nombre_regla + " " + planilla.numero_planilla);
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


    }
}
