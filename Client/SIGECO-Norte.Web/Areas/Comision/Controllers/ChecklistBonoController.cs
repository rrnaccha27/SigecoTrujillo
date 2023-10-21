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
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SIGEES.Web.Areas.Comision.Utils;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Entidades.planilla;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    [RequiresAuthenticationAttribute]
    public class ChecklistBonoController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly IParametroSistemaService _IParametroSistemaService;

        public ChecklistBonoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _IParametroSistemaService = new ParametroSistemaService();
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

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult _Buscar()
        {
            var query = new object();
            int v_resultado = 1;
            string v_mensaje = "";
            int v_total = 0;
            try
            {
                var lst = ChecklistBonoSelBL.Instance.Listar();
                v_total = lst.Count();
                query = from order in lst.AsEnumerable()
                        select new
                        {
                            codigo_checklist = order.codigo_checklist,
                            numero_checklist = order.numero_checklist,
                            nombre_estado_checklist = order.nombre_estado_checklist,
                            codigo_estado_checklist = order.codigo_estado_checklist,
                            fecha_apertura = order.fecha_apertura,
                            fecha_cierre = order.fecha_cierre,
                            fecha_anulacion = order.fecha_anulacion,
                            usuario_apertura = order.usuario_apertura,
                            usuario_cierre = order.usuario_cierre,
                            usuario_anulacion = order.usuario_anulacion,
                            estilo = order.estilo
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

        [HttpGet]
        public PartialViewResult _Planilla(int codigo_checklist)
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            ChecklistBonoViewModel vm = new ChecklistBonoViewModel();

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

                var checklist = ChecklistBonoSelBL.Instance.Detalle(codigo_checklist);

                vm.bean = bean;
                vm.checklist = checklist;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return PartialView(vm);
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult GetPlanillaJson()
        {
            List<planilla_checklist_bono_dto> lst = new List<planilla_checklist_bono_dto>();
            try
            {

                lst = PlanillaSelBL.Instance.ListarParaChecklistBono();
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult GetDetalleJson(int codigo_checklist, int validado)
        {
            List<checklist_bono_detalle_listado_dto> lst = new List<checklist_bono_detalle_listado_dto>();
            try
            {

                lst = ChecklistBonoSelBL.Instance.ListarDetalle(codigo_checklist, (validado==1?true:false));
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(lst), "application/json");
        }

        [HttpGet]
        public ActionResult _Listado()
        {
            return PartialView();
        }
        
        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Aperturar(checklist_bono_estado_dto planilla)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                planilla.usuario = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = ChecklistBonoBL.Instance.Aperturar(planilla);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
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
        public ActionResult Anular(checklist_bono_estado_dto checklist)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                checklist.usuario = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = ChecklistBonoBL.Instance.Anular(checklist);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
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
        public ActionResult Cerrar(checklist_bono_estado_dto checklist)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                checklist.usuario = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = ChecklistBonoBL.Instance.Cerrar(checklist);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                string[] parameters = new string[1];
                parameters[0] = checklist.codigo_checklist.ToString();

                Thread SendingThreads = new Thread(EnvioTxtporCorreo);
                SendingThreads.Start(parameters);
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
        public ActionResult Validar(List<checklist_bono_detalle_listado_dto> checklist_detalle)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            string usuario = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                usuario = beanSesionUsuario.codigoUsuario;

                MensajeDTO mensaje = ChecklistBonoBL.Instance.Validar(checklist_detalle, usuario);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        #region TXT

        private string urlTxt = string.Empty;
        private string savePathZip = string.Empty;
        private string savePath = string.Empty;
        private string por_correo = string.Empty;

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
        public FileResult GenerarTxtRRHH(int id)
        {
            string mensaje = string.Empty;
            string numero_planilla = string.Empty;
            string tipo_txt = "_RRHH";

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
                urlTxt = string.Empty;
                string string2 = _ruta_archivo.valor.Substring(_ruta_archivo.valor.Length - 1, 1);
                if (string2 == "//" || string2 == "\\")
                {
                    urlTxt = string.Format("{0}{1}{2}{3}", _ruta_archivo.valor, numero_planilla, tipo_txt, por_correo);
                }
                else
                {
                    urlTxt = string.Format("{0}/{1}{2}{3}", _ruta_archivo.valor, numero_planilla, tipo_txt, por_correo);
                }

                //string urlTxt = Server.MapPath(string.Format("~/Areas/Comision/Reporte/Planilla/txt/{0}/", numero_planilla));

                if (!System.IO.Directory.Exists(urlTxt))
                {
                    System.IO.Directory.CreateDirectory(urlTxt);
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(urlTxt);
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
                        string savePath = string.Format(@"{0}\{1}.txt", urlTxt, txt_archivo);
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
                        string savePath = string.Format(@"{0}\{1}.txt", urlTxt, txt_archivo);
                        formato.generaFormatoPlanillaHaberes(savePath, System.Text.Encoding.UTF8, listaDetalleHA, beanCabeceraCargoPlanillaHPD);

                    }

                }

                string name_zip = DateTime.Now.ToString("dd-MM-yyyy");
                savePathZip = string.Format(@"{0}\{1}.zip", urlTxt, name_zip);
                savePathZip = savePathZip.Replace("\\\\", "\\");
                ZipArchive zip = ZipFile.Open(savePathZip, ZipArchiveMode.Create);
                string nombre_zip_download = string.Empty;
                foreach (string file in selectedfiles)
                {
                    nombre_zip_download = (string.IsNullOrWhiteSpace(nombre_zip_download) ? "" : nombre_zip_download + "-") + file;
                    zip.CreateEntryFromFile(string.Format("{0}/{1}", urlTxt, string.Format("{0}.txt", file)), string.Format("{0}.txt", file));
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
        public FileResult GenerarTxtContabilidad(int codigo_checklist, int codigo_empresa, int codigo_planilla)
        {
            string mensaje = string.Empty;
            string numero_planilla = string.Empty;
            string tipo_txt = "_CONT";

            try
            {
                var _ruta_archivo = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.ruta_archivo_txt);
                if (_ruta_archivo == null)
                {
                    throw new Exception("La ruta del archivo txt no esta definido en la configuración de parámetros.");
                }
                List<string> selectedfiles = new List<string>();
                List<txt_contabilidad_planilla_dto> lst = PlanillaSelBL.Instance.GetPlanillaBonoTxt_Contabilidad(codigo_checklist, codigo_empresa, false, codigo_planilla);
                checklist_bono_listado_dto checklist = ChecklistBonoSelBL.Instance.Detalle(codigo_checklist);
                numero_planilla = checklist.numero_checklist;

                if (lst == null)
                    throw new Exception("No existe registro de planilla.");

                if (lst.Count == 0)
                    throw new Exception("No existe registro de planilla.");

                urlTxt = string.Empty;
                string string2 = _ruta_archivo.valor.Substring(_ruta_archivo.valor.Length - 1, 1);

                if (string2 == "//" || string2 == "\\")
                {
                    urlTxt = string.Format("{0}{1}{2}{3}", _ruta_archivo.valor, numero_planilla, tipo_txt, por_correo);
                }
                else
                {
                    urlTxt = string.Format("{0}/{1}{2}{3}", _ruta_archivo.valor, numero_planilla, tipo_txt, por_correo);
                }

                if (!System.IO.Directory.Exists(urlTxt))
                {
                    System.IO.Directory.CreateDirectory(urlTxt);
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(urlTxt);
                    directory.GetFiles().ToList().ForEach(f => f.Delete());
                }

                FormatoReporteTXT formato = new FormatoReporteTXT();
                string txt_archivo = "BONOS";

                selectedfiles.Add(txt_archivo);
                savePath = string.Format(@"{0}\{1}.txt", urlTxt, txt_archivo);
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

        #region Envio de Txt por Correo

        private void EnvioTxtporCorreo(object parameters)
        {
            Array arrayParameters = new object[2];
            arrayParameters = (Array)parameters;

            int p_codigo_checklist = int.Parse((string)arrayParameters.GetValue(0));
            var _item_usuario = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_usuario);
            var _item_passwor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_password);
            var _item_servidor = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_servidor);
            var _item_puerto = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.correo_puerto);
            checklist_bono_listado_dto checklist = ChecklistBonoSelBL.Instance.Detalle(p_codigo_checklist);

            string[] datosCorreo = new string[(int)TemplateTxtParametros.total_enum];
            datosCorreo[(int)TemplateTxtParametros.tipo_checklist] = "Bono";
            datosCorreo[(int)TemplateTxtParametros.numero_checklist] = checklist.numero_checklist;
            datosCorreo[(int)TemplateTxtParametros.cuenta] = _item_usuario.valor;
            datosCorreo[(int)TemplateCorreoParametros.fecha] = DateTime.Now.ToShortDateString() + " " + String.Format("{0:T}", DateTime.Now);

            List<canal_jefatura_dto> lst_canal_jefatura = new CanalGrupoBL().ListarJefatura();
            canal_jefatura_dto cuentas = null;

            SmtpClient smtp = new SmtpClient(_item_servidor.valor);
            smtp.Port = int.Parse(_item_puerto.valor.ToString());
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_item_usuario.valor, _item_passwor.valor);
            MailMessage email = new MailMessage();

            string contenidoCorreo = getContenidoCorreo(TemplateCorreo.TxtChecklist, datosCorreo);

            por_correo = "_email";

            //Envio de TXT de RRHH
            savePathZip = string.Empty;
            GenerarTxtRRHH(p_codigo_checklist);
            cuentas = lst_canal_jefatura.FindAll(x => x.codigo_canal == (int)CuentasCorreo.recursos_humanos).FirstOrDefault();
            try
            {
                email = new MailMessage();

                Attachment atach = new Attachment(savePathZip);

                email.Attachments.Add(atach);

                email.To.Add(new MailAddress(cuentas.email));
                email.To.Add(new MailAddress(cuentas.email_copia));

                email.From = new MailAddress(_item_usuario.valor);
                email.Subject = "Envío de TXT de CheckList Bono " + datosCorreo[(int)TemplateTxtParametros.numero_checklist];

                email.Body = contenidoCorreo;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.High;
                smtp.Send(email);

                email.Attachments.Clear();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                //log.Error("Ocurrio error al enviar  correo a : " + item.nombres + "(" + mensaje + ")");
            }
            finally
            {
                email = null;
            }

            //Envio de TXT de Contabilidad
            cuentas = lst_canal_jefatura.FindAll(x => x.codigo_canal == (int)CuentasCorreo.contabilidad).FirstOrDefault();
            try
            {
                email = new MailMessage();

                List<txt_contabilidad_resumen_planilla_dto> lst = PlanillaSelBL.Instance.GetResumenPlanillaTxt_Contabilidad(p_codigo_checklist);
                string[] archivos = new string[lst.Count()];

                foreach (txt_contabilidad_resumen_planilla_dto elemento in lst)
                {
                    savePath = string.Empty;
                    GenerarTxtContabilidad(p_codigo_checklist, elemento.codigo_empresa, elemento.codigo_planilla);
                    Attachment atach = new Attachment(savePath);
                    email.Attachments.Add(atach);
                }

                email.To.Add(new MailAddress(cuentas.email));
                email.To.Add(new MailAddress(cuentas.email_copia));

                email.From = new MailAddress(_item_usuario.valor);
                email.Subject = "Envío de TXT de CheckList Bono " + datosCorreo[(int)TemplateTxtParametros.numero_checklist];

                email.Body = contenidoCorreo;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.High;
                smtp.Send(email);

                email.Attachments.Clear();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                //log.Error("Ocurrio error al enviar  correo a : " + item.nombres + "(" + mensaje + ")");
            }
            finally
            {
                email = null;
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
                ex.Message.ToString();
            }
            return contenido.ToString();
        }

        #endregion

    }
}
