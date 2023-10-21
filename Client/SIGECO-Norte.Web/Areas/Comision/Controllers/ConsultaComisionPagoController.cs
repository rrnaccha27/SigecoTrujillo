using Microsoft.Reporting.WebForms;
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
    public class ConsultaComisionPagoController : Controller
    {
        private readonly TipoPlanillaService _tipoPlanillaService;
        private readonly CanalGrupoService _canalService;
        //private readonly EstadoPlanillaService _estadoPlanillaService;
        private readonly EstadoCuotaService _estadoCuotaService;

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();

        private readonly ITipoAccesoItemService _tipoAccesoItemService;

        [RequiresAuthenticationAttribute]
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

        public ConsultaComisionPagoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
            _tipoPlanillaService = new TipoPlanillaService();
            //_estadoPlanillaService = new EstadoPlanillaService();
            _estadoCuotaService = new EstadoCuotaService();
        }

        public ActionResult GetRegistrosAllJson(grilla_comision_cronograma_filtro v_entidad)
        {
            List<grilla_comision_cronograma_dto> lst = new List<grilla_comision_cronograma_dto>();
            List<grilla_comision_cronograma_dto> lst_aux = new List<grilla_comision_cronograma_dto>();
            int v_total = 0;
            string mensaje = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(v_entidad.codigo_estado_cuota.ToString()))
                {

                    if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_habilitado_inicio))
                        v_entidad.fecha_habilitado_inicio = DateTime.Parse(v_entidad.str_fecha_habilitado_inicio);

                    if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_habilitado_fin))
                        v_entidad.fecha_habilitado_fin = DateTime.Parse(v_entidad.str_fecha_habilitado_fin);

                    if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_contrato_inicio))
                        v_entidad.fecha_contrato_inicio = DateTime.Parse(v_entidad.str_fecha_contrato_inicio);

                    if (!string.IsNullOrWhiteSpace(v_entidad.str_fecha_contrato_fin))
                        v_entidad.fecha_contrato_fin = DateTime.Parse(v_entidad.str_fecha_contrato_fin);

                    lst = DetalleCronogramaPagoSelBL.Instance.CronogramaPagoComisionListar(v_entidad);

                    v_total = lst.Count;

                    if (v_total == 0)
                    {
                        throw new Exception("No se encontraron registros de acuerdo a los filtros establecidos.");
                    }
                }
                return Content(JsonConvert.SerializeObject(new { total = v_total, sucess = true, rows = lst, message = mensaje }), "application/json");
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Content(JsonConvert.SerializeObject(new { total = 0, rows = lst_aux, sucess = false, message = mensaje }), "application/json");
        }

        [HttpPost]
        public ActionResult SetFiltroGrilla(grilla_comision_cronograma_filtro v_entidad)
        {
            Guid id = Guid.NewGuid();
            string v_guid = id.ToString().Replace('-','_');
            Session[v_guid] = v_entidad;
            string urlReporte = Url.Action("frm_reporte_consulta_comision", "Areas/Comision/Reporte/Comision/frm", new { area = string.Empty }) + ".aspx?p_guid=" + v_guid;

            return Json(new { v_guid = v_guid, v_url = urlReporte }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDataGrilla(List<grilla_comision_cronograma_dto> v_entidad)
        {
            Guid id = Guid.NewGuid();
            string v_guid = id.ToString().Replace('-', '_');
            Session[v_guid] = v_entidad;
            return Json(new { v_guid = v_guid}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Persona_Busqueda()
        {
            return PartialView();
        }

        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._tipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetGrupoJson(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Content("", "application/json");
            }
            else
            {
                string result = this._canalService.GetGrupoAllComboJson(int.Parse(id));
                return Content(result, "application/json");
            }
        }

        public ActionResult GetEstadoCuotaJson()
        {
            string result = this._estadoCuotaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetPersonalByNombreJson(string texto)
        {
            List<personal_comision_manual_listado_dto> lista = new List<personal_comision_manual_listado_dto>();

            if (texto != "")
            {
                lista = new PersonalBL().ListarParaComisionManual(texto);
            }

            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        #region Exportacion

        public ActionResult ExportarComisionExcel(string id)
        {
           
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";
            //DataTable lst = new DataTable();
            List<grilla_comision_cronograma_dto> lst = new List<grilla_comision_cronograma_dto>();
            try
            {
                lst = Session[id] as List<grilla_comision_cronograma_dto>;


                ReportDataSource dataSource = new ReportDataSource("dsComision", lst);
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Comision/rdl/rpt_comision.rdlc");

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, "Cronograma Pago Comision.xls");


                //return Exportar(FileType, ContentType, dataSource, "Fallecidos", "~/Reports/RFallecido.rdlc", parametros);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            finally {
                Session.Remove(id);
            }
            return null;


        }
        public ActionResult ExportarComisionPdf(string id)
        {
            grilla_comision_cronograma_filtro v_entidad = new grilla_comision_cronograma_filtro();
            string FileType = "pdf";
            string ContentType = "application/pdf";
            //DataTable lst = new DataTable();
            List<grilla_comision_cronograma_dto> lst = new List<grilla_comision_cronograma_dto>();
            try
            {
                lst = Session[id] as List<grilla_comision_cronograma_dto>;
                ReportDataSource dataSource = new ReportDataSource("dsComision", lst);
                LocalReport rpt = new LocalReport();
                rpt.ReportPath = Server.MapPath("~/Areas/Comision/Reporte/Comision/rdl/rpt_comision.rdlc");

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                string reportType = FileType;
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(renderedBytes, ContentType, string.Format("Cronograma Pago Comision.{0}", FileType));


                //return Exportar(FileType, ContentType, dataSource, "Fallecidos", "~/Reports/RFallecido.rdlc", parametros);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            finally {

                Session.Remove(id);
            }
            return null;
        }

        #endregion 

    }
}