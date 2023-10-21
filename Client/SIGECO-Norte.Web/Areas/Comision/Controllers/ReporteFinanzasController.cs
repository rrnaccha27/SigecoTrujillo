using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using SIGEES.BusinessLogic;
using SIGEES.Entidades;

using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.Areas.Comision.Utils;

using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.MemberShip.Filters;

using Microsoft.Reporting.WebForms;

using System.IO;
using System.Web.Hosting;
using ClosedXML.Excel;


namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ReporteFinanzasController : Controller
    {
        //
        // GET: /Comision/LogContratoSAP/

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly CanalGrupoService _canalService;
        private readonly TipoPlanillaService _tipoPlanillaService;

        // private canal_grupo _canal_grupo = null;

        #region Inicializacion de Controller - Menu
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReporteFinanzasController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
            _tipoPlanillaService = new TipoPlanillaService();
        }
        #endregion

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson(false);
            return Content(result, "application/json");
        }

        public ActionResult GetAnioReporteJson()
        {
            List<JObject> jObjects = new List<JObject>();
            int inicio = 2022;
            while (inicio <= DateTime.Now.Year)
            {
                jObjects.Add(new JObject { { "id", inicio.ToString() }, { "text", inicio.ToString() } });
                inicio++;
            }

            return Content(JsonConvert.SerializeObject(jObjects), "application/json");
        }
        public ActionResult GetMesJson()
        {
            List<JObject> jObjects = new List<JObject>();
            jObjects.Add(new JObject { { "id", "1" }, { "text", "Enero" } });
            jObjects.Add(new JObject { { "id", "2" }, { "text", "Febrero" } });
            jObjects.Add(new JObject { { "id", "3" }, { "text", "Marzo" } });
            jObjects.Add(new JObject { { "id", "4" }, { "text", "Abril" } });
            jObjects.Add(new JObject { { "id", "5" }, { "text", "Mayo" } });
            jObjects.Add(new JObject { { "id", "6" }, { "text", "Junio" } });
            jObjects.Add(new JObject { { "id", "7" }, { "text", "Julio" } });
            jObjects.Add(new JObject { { "id", "8" }, { "text", "Agosto" } });
            jObjects.Add(new JObject { { "id", "9" }, { "text", "Septiembre" } });
            jObjects.Add(new JObject { { "id", "10" }, { "text", "octubre" } });
            jObjects.Add(new JObject { { "id", "11" }, { "text", "Noviembre" } });
            jObjects.Add(new JObject { { "id", "12" }, { "text", "Diciembre" } });

            return Content(JsonConvert.SerializeObject(jObjects), "application/json");
        }

        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._tipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetAnioJson()
        {
            var result = ReporteGeneralBL.Instance.Anio();
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [RequiresAuthentication]
        public ActionResult GetAllJson(reporte_comercial_busqueda_dto busqueda)
        {
            var lista = ReporteGeneralBL.Instance.ReporteComisionSupervisores(busqueda);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        #region "Exportar Excel"

        [RequiresAuthentication]
        public ActionResult SetDataExcel(reporte_comercial_busqueda_dto busqueda)
        {
            Guid id = Guid.NewGuid();
            string v_guid = id.ToString().Replace('-', '_');
            Session[v_guid + "_data"] = ReporteGeneralBL.Instance.ReporteComisionSupervisores(busqueda);
           // Session[v_guid + "_filtro"] = ReporteGeneralBL.Instance.FinanzasFiltro(busqueda); ;
            
            return Json(new { v_guid = v_guid }, JsonRequestBehavior.AllowGet);
        }

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

        public byte[] ReadMemoryToBytes(XLWorkbook workbook)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[(int)ms.Length];
                buffer = ms.ToArray();
                return buffer;
            }

        }

        public ActionResult GenerarExcel(string id)
        {
            string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";
            string rutaRDLC = string.Empty;
            string nombreEXCEL = string.Empty;
            string nombreTipoReporte = string.Empty;

            List<reporte_finanzas_dto> lst = new List<reporte_finanzas_dto>();
            reporte_finanzas_filtro_dto filtro = new reporte_finanzas_filtro_dto();
            try
            {
                lst = Session[id + "_data"] as List<reporte_finanzas_dto>;
                filtro = Session[id + "_filtro"] as reporte_finanzas_filtro_dto;
                
                reporte_finanzas_dto detalle = lst.FirstOrDefault();

                nombreEXCEL = "ReporteFinanzas" + (detalle.tipo == (int)Utils.ReporteFinanzas.comision ? "_Comision" : "_Bono");

                if (detalle.resumen_detalle == (int)Utils.ReporteFinanzasTipoSumatoria.resumen)
                {
                    rutaRDLC = "~/Areas/Comision/Reporte/ReporteFinanzas/rdl/rpt_reporte_finanzas.rdlc";
                    nombreEXCEL += ".xls";
                }
                else 
                {
                    rutaRDLC = "~/Areas/Comision/Reporte/ReporteFinanzas/rdl/rpt_reporte_finanzas_detalle.rdlc";
                    nombreEXCEL += "_Detalle.xls";
                }

                if (detalle.tipo == (int)Utils.ReporteFinanzas.comision)
                {
                    if (detalle.tipo_reporte == (int)Utils.ReporteFinanzasTipo.generado)
                    {
                        nombreTipoReporte = " - " + Utils.ReporteFinanzasTipo.generado.ToString().ToUpper();
                    }
                    else
                    {
                        nombreTipoReporte = " - " + Utils.ReporteFinanzasTipo.pagado.ToString().ToUpper();
                    }
                }
                else
                {
                    nombreTipoReporte = "";
                }

                ReportDataSource dataSource = new ReportDataSource("dsReporteFinanzas", lst);
                LocalReport rpt = new LocalReport
                {
                    ReportPath = Server.MapPath(rutaRDLC)
                };

                rpt.DataSources.Clear();
                rpt.DataSources.Add(dataSource);

                List<ReportParameter> parametros = new List<ReportParameter>();
                ReportParameter prmtTipo = new ReportParameter("Tipo", filtro.tipo);
                ReportParameter prmtCanal = new ReportParameter("Canal", filtro.canal);
                ReportParameter prmtTipoPlanilla = new ReportParameter("TipoPlanilla", filtro.tipo_planilla);
                ReportParameter prmtTipoReporte = new ReportParameter("TipoReporte", filtro.tipo_reporte);
                ReportParameter prmtPeriodo = new ReportParameter("Periodo", filtro.periodo);
                ReportParameter prmtAnio = new ReportParameter("Anio", filtro.anio);

                parametros.Add(prmtTipo);
                parametros.Add(prmtCanal);
                parametros.Add(prmtTipoPlanilla);
                parametros.Add(prmtTipoReporte);
                parametros.Add(prmtPeriodo);
                parametros.Add(prmtAnio);

                rpt.SetParameters(parametros);

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
                Session.Remove(id);
            }
            return null;
        }

        [RequiresAuthentication]
        public ActionResult ReporteDetalleComisionSupervisores(reporte_comercial_busqueda_dto busqueda)
        {
            //LLAMANDO
            //OBJETO DE BUSINES LOGIC
            var obj = new ReporteGeneralBL();
            //var lista = obj.DetalleComisionPlanillaSupervisor(busqueda);
            if (busqueda.codigo_canal.Equals("4"))
            {
                var lista = obj.DetalleComisionPlanillaSupervisor(busqueda);
                var (bytes, fileName) = GenerarDetalleComisionSupervisorExcel(lista);
                return Json(new { archivo = Convert.ToBase64String(bytes), fileName = fileName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var lista = obj.DetalleComisionPlanillaJefatura(busqueda);
                var (bytes, fileName) = GenerarDetalleComisionJefaturaExcel(lista);
                return Json(new { archivo = Convert.ToBase64String(bytes), fileName = fileName }, JsonRequestBehavior.AllowGet);
            }
        }

        [RequiresAuthentication]
        public ActionResult ReporteContratoComisionSupervisores(reporte_comercial_busqueda_dto busqueda)
        {
            var lista = ReporteGeneralBL.Instance.DetalleComisionContrato(busqueda);
            var (bytes, fileName) = GenerarDetalleContratoComisionExcel(lista, busqueda.codigo_canal);
            return Json(new { archivo = Convert.ToBase64String(bytes), fileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public (byte[], string) GenerarDetalleComisionSupervisorExcel(List<reporte_detallado_vendedores_dto> data)
        {

            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            var FileName = $"CIX_COM_REPORTE_DE_COMISIONES_ACT_SUPERVISORES_{DateTime.Now.Year}_{numRandom}.xlsx";

            try
            {


                string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_comision_supervisor.xlsx");

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Where(x => x.Name == "REPORTE").FirstOrDefault();

                        var HEADER = worksheet.Range("B7:G7");
                        var UP_HEADER = worksheet.Range("B6:G6");
                        var HEADER_PERSONA = worksheet.Range("B5:G5");
                        worksheet.Range("B1:G1").Value = $"JARDINES DE LA PAZ - TRUJILLO";
                        int i = 1;
                        int j = 0;
                        int k = 0;
                        //int row = 8;
                        var personas = data.GroupBy(x => x.codigo_personal);

                        int row = 5;

                        foreach (var key in personas)
                        {
                            var total_comision_persona = 0.00;

                            var datos_personales = key.FirstOrDefault().nombre;
                            worksheet.Cell(row, "B").Value = datos_personales; worksheet.Cell(row, "B").Style.Font.Bold = true;
                            row++;
                            worksheet.Cell(row, "C").Value = "pptto"; worksheet.Cell(row, "C").Style.Font.Bold = true; worksheet.Cell(row, "C").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(row, "D").Value = "ejec"; worksheet.Cell(row, "D").Style.Font.Bold = true; worksheet.Cell(row, "D").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            row++;

                            var articulos = data.Where(x => x.codigo_personal.Equals(key.FirstOrDefault().codigo_personal)).GroupBy(x => x.codigo_tipo_articulo);

                            foreach (var key1 in articulos)
                            {
                                var dato_articulo = key1.FirstOrDefault().nombre_tipo_articulo;
                                var concepto_comision = key1.FirstOrDefault().concepto_comision_articulo;
                                var dato_tope_unidad = key1.FirstOrDefault().tope_unidad;
                                var dato_ejecutado = 0; //key1.FirstOrDefault().cantidad_contrato;
                                var dato_comision_venta = key1.FirstOrDefault().precio_unidad;

                                foreach (var item in key1)
                                {
                                    dato_ejecutado = dato_ejecutado + item.cantidad_contrato;
                                }

                                worksheet.Cell(row, "B").Value = dato_articulo; worksheet.Cell(row, "B").Style.Font.Bold = true; worksheet.Cell(row, "B").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(row, "C").Value = dato_tope_unidad; worksheet.Cell(row, "C").Style.Font.Bold = true; worksheet.Cell(row, "C").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(row, "D").Value = dato_ejecutado; worksheet.Cell(row, "D").Style.Font.Bold = true; worksheet.Cell(row, "D").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                row++;
                                worksheet.Cell(row, "B").Value = concepto_comision; worksheet.Cell(row, "B").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(row, "C").Style.Border.OutsideBorder = XLBorderStyleValues.Thin; worksheet.Cell(row, "D").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(row, "E").Value = dato_ejecutado; worksheet.Cell(row, "E").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(row, "F").Value = dato_comision_venta; worksheet.Cell(row, "F").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(row, "G").Value = dato_ejecutado * dato_comision_venta; worksheet.Cell(row, "G").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                total_comision_persona = total_comision_persona + double.Parse((dato_ejecutado * dato_comision_venta).ToString());
                                row++;

                                foreach (var item in key1)
                                {
                                    worksheet.Cell(row, "B").Value = item.concepto_comision_tipo_venta; worksheet.Cell(row, "B").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "C").Style.Border.OutsideBorder = XLBorderStyleValues.Thin; worksheet.Cell(row, "D").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "E").Value = item.monto_contrato; worksheet.Cell(row, "E").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "F").Value = item.porcentaje_regla_comision; worksheet.Cell(row, "F").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "G").Value = item.monto_neto; worksheet.Cell(row, "G").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    total_comision_persona = total_comision_persona + double.Parse(item.monto_neto.ToString());
                                    row++;
                                }

                                foreach (var item in key1)
                                {
                                    worksheet.Cell(row, "B").Value = item.concepto_bono_tipo_venta; worksheet.Cell(row, "B").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "C").Style.Border.OutsideBorder = XLBorderStyleValues.Thin; worksheet.Cell(row, "D").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "E").Value = item.meta_tipo_venta; worksheet.Cell(row, "E").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "F").Value = item.porcentaje_meta; worksheet.Cell(row, "F").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    worksheet.Cell(row, "G").Value = item.monto_prorrateo; worksheet.Cell(row, "G").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    total_comision_persona = total_comision_persona + double.Parse(item.monto_prorrateo.ToString());
                                    row++;
                                }

                                //foreach (var item in key1)
                                //{
                                //    worksheet.Cell(row, "B").Value = item.concepto_excedente_tipo_venta;
                                //    worksheet.Cell(row, "E").Value = item.excedente_tipo_venta;
                                //    worksheet.Cell(row, "F").Value = item.porcentaje_excedente;
                                //    worksheet.Cell(row, "G").Value = item.monto_prorrateo_excedente;
                                //    total_comision_persona = total_comision_persona + double.Parse(item.monto_prorrateo_excedente.ToString());
                                //    row++;
                                //}
                            }

                            worksheet.Cell(row, "E").Value = "TOTAL"; worksheet.Cell(row, "E").Style.Font.Bold = true;
                            worksheet.Cell(row, "G").Value = total_comision_persona; worksheet.Cell(row, "G").Style.Font.Bold = true;

                            row++; row++; row++;

                            //var datos_personales = key.FirstOrDefault().nombres;
                            //if (i == 1)
                            //{
                            //    worksheet.Cell(5, "B").Value = datos_personales;
                            //}
                            //if (i == 0)
                            //{
                            //    row++;
                            //    HEADER_PERSONA.CopyTo(worksheet.Range($"B{row}:D{row}"));
                            //    //worksheet.Range($"B{row}:D{row}").Value = datos_personales;
                            //    worksheet.Cell(row, "B").Value = datos_personales;
                            //    row++;
                            //    UP_HEADER.CopyTo(worksheet.Range($"B{row}:G{row}"));                                
                            //}

                            //foreach (var item in key)
                            //{
                            //    if (i == 1 && k == 0)
                            //    {
                            //        worksheet.Cell(7, "B").Value = item.nombre_tipo_articulo;
                            //        worksheet.Cell(7, "C").Value = item.tope_unidad;
                            //        worksheet.Cell(7, "D").Value = item.cantidad_contrato;
                            //        worksheet.Cell(7, "B").Style.Font.Bold = true;
                            //        j = item.codigo_tipo_articulo;
                            //    }
                            //    if (i == 0 && k == 0)
                            //    {
                            //        j = item.codigo_tipo_articulo;
                            //        worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo;
                            //        worksheet.Cell(row, "C").Value = item.tope_unidad;
                            //        worksheet.Cell(row, "D").Value = item.cantidad_contrato;
                            //        worksheet.Cell(row, "B").Style.Font.Bold = true;
                            //        worksheet.Range($"C{row}:G{row}").Style.Font.Bold = true;
                            //        row++;

                            //    }

                            //    if (j != item.codigo_tipo_articulo)
                            //    {
                            //        j = item.codigo_tipo_articulo;
                            //        worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo;
                            //        //CABECER
                            //        worksheet.Cell(row, "C").Value = item.tope_unidad;
                            //        worksheet.Cell(row, "D").Value = item.cantidad_contrato;
                            //        worksheet.Cell(row, "B").Style.Font.Bold = true;
                            //        worksheet.Range($"C{row}:G{row}").Style.Font.Bold = true;
                            //        row++;

                            //        worksheet.Cell(row, "B").Value = item.nombre_grupo;
                            //        if (item.codigo_detalle_regla_comision == 21)
                            //        {
                            //            worksheet.Cell(row, "E").Value = item.cantidad_contrato;
                            //        }

                            //        worksheet.Cell(row, "F").Value = item.precio_unidad;
                            //        worksheet.Cell(row, "G").Value = item.monto_neto;
                            //        worksheet.Cell(row, "G").SetDataType(XLDataType.Number);

                            //        row++;
                            //    }
                            //    else {
                            //        worksheet.Cell(row, "B").Value = item.nombre_grupo;
                            //        if (item.codigo_detalle_regla_comision == 21)
                            //        {
                            //            worksheet.Cell(row, "E").Value = item.cantidad_contrato;
                            //        }

                            //        worksheet.Cell(row, "F").Value = item.precio_unidad;
                            //        worksheet.Cell(row, "G").Value = item.monto_neto;
                            //        worksheet.Cell(row, "G").SetDataType(XLDataType.Number);

                            //        row++;
                            //        j = item.codigo_tipo_articulo;
                            //    }                                
                            //    k++;
                            //}
                            //worksheet.Cell(row, "B").Value = "TOTAL";
                            //worksheet.Cell(row, "B").Style.Font.Bold = true;
                            //worksheet.Range($"B{row}:F{row}").Merge();

                            //worksheet.Cell(row, "G").Value = key.Sum(x => x.monto_neto);
                            //worksheet.Cell(row, "G").Style.Fill.BackgroundColor = XLColor.FromArgb(209, 209, 255);
                            //worksheet.Range($"B{row}:G{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //worksheet.Range($"B{row}:G{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            //worksheet.Range($"B{row}:G{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                            //row++;

                            //i = 0;
                            //k = 0;
                            //j = 0;
                        }

                        workbook.SaveAs(Path.Combine(urlBase, Path.Combine("Temp", FileName)));
                        return (ReadMemoryToBytes(workbook), FileName);

                    }
                }





            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return (null, FileName);
        }

        public (byte[], string) GenerarDetalleComisionJefaturaExcel(List<reporte_detallado_vendedores_dto> data)
        {
            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            var FileName = $"CIX_COM_REPORTE_DE_COMISIONES_JEFATURA_{DateTime.Now.Year}_{numRandom}.xlsx";

            try
            {
                string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_comision_jefatura_trujillo.xlsx");

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Where(x => x.Name == "REPORTE").FirstOrDefault();

                        var HEADER = worksheet.Range("B6:E6");                        
                        var HEADER_PERSONA = worksheet.Range("B5:E5");
                        //worksheet.Range("B2:E2").Value = $"JARDINES DE LA PAZ - TRUJILLO";
                        int i = 1;
                        int j = 0;
                        int k = 0;
                        int row = 7;
                        //var personas = data.GroupBy(x => x.codigo_personal);

                        decimal? suma_venta = 0;
                        decimal? suma_monto = 0;

                        foreach (var item in data)
                        {
                            if (item.codigo_tipo_articulo == 1)
                            {
                                worksheet.Cell(7, "D").Value = item.cantidad_contrato;
                                worksheet.Cell(7, "E").Value = item.porcentaje_regla_comision;
                                //worksheet.Cell(7, "F").Value = item.porcentaje_excedente;
                                //row++;
                                worksheet.Cell(8, "D").Value = item.precio_unidad;
                                worksheet.Cell(8, "E").Value = item.monto_neto;
                                //worksheet.Cell(8, "F").Value = item.monto_prorrateo_excedente;
                                //row++;
                                worksheet.Cell(9, "D").Value = item.comision_x_meta;
                                worksheet.Cell(9, "E").Value = item.comision_excedente;
                                worksheet.Cell(9, "F").Value = item.comision_x_meta + item.comision_excedente;

                                suma_venta = suma_venta + item.comision_x_meta;
                                suma_monto = suma_monto + item.comision_excedente;
                            }

                            if (item.codigo_tipo_articulo == 3)
                            {
                                worksheet.Cell(10, "D").Value = item.cantidad_contrato;
                                worksheet.Cell(10, "E").Value = item.porcentaje_regla_comision;
                                //worksheet.Cell(7, "F").Value = item.porcentaje_excedente;
                                //row++;
                                worksheet.Cell(11, "D").Value = item.precio_unidad;
                                worksheet.Cell(11, "E").Value = item.monto_neto;
                                //worksheet.Cell(8, "F").Value = item.monto_prorrateo_excedente;
                                //row++;
                                worksheet.Cell(12, "D").Value = item.comision_x_meta;
                                worksheet.Cell(12, "E").Value = item.comision_excedente;
                                worksheet.Cell(12, "F").Value = item.comision_x_meta + item.comision_excedente;

                                suma_venta = suma_venta + item.comision_x_meta;
                                suma_monto = suma_monto + item.comision_excedente;
                            }

                            worksheet.Cell(13, "D").Value = suma_venta;
                            worksheet.Cell(13, "E").Value = suma_monto;
                            worksheet.Cell(13, "F").Value = suma_venta + suma_monto;

                            //if (item.codigo_tipo_articulo == 1)
                            //{
                            //    worksheet.Cell(row, "D").Value = item.porcentaje_regla_comision;
                            //    worksheet.Cell(row, "E").Value = item.porcentaje_meta;
                            //    worksheet.Cell(row, "F").Value = item.porcentaje_excedente;
                            //    row++;
                            //    worksheet.Cell(row, "D").Value = item.cantidad_contrato;
                            //    worksheet.Cell(row, "E").Value = item.monto_prorrateo;
                            //    worksheet.Cell(row, "F").Value = item.monto_prorrateo_excedente;
                            //    row++;
                            //    worksheet.Cell(row, "D").Value = item.precio_unidad;
                            //    worksheet.Cell(row, "E").Value = item.comision_x_meta;
                            //    worksheet.Cell(row, "F").Value = item.comision_excedente;
                            //    worksheet.Cell(row, "G").Value = item.precio_unidad + item.comision_x_meta + item.comision_excedente;
                            //}

                            /*
                            if (item.codigo_tipo_articulo == 2)
                            {
                                row++;
                                worksheet.Cell(row, "D").Value = item.porcentaje_regla_comision;
                                worksheet.Cell(row, "E").Value = item.porcentaje_meta;
                                worksheet.Cell(row, "F").Value = item.porcentaje_excedente;
                                row++;
                                worksheet.Cell(row, "D").Value = item.cantidad_contrato;
                                worksheet.Cell(row, "E").Value = item.monto_prorrateo;
                                worksheet.Cell(row, "F").Value = item.monto_prorrateo_excedente;
                                row++;
                                worksheet.Cell(row, "D").Value = item.precio_unidad;
                                worksheet.Cell(row, "E").Value = item.comision_x_meta;
                                worksheet.Cell(row, "F").Value = item.comision_excedente;
                                worksheet.Cell(row, "G").Value = item.precio_unidad + item.comision_x_meta + item.comision_excedente;
                            }
                            */
                            //if (item.codigo_tipo_articulo == 3)
                            //{
                            //    row++;
                            //    worksheet.Cell(row, "D").Value = item.porcentaje_regla_comision;
                            //    worksheet.Cell(row, "E").Value = item.porcentaje_meta;
                            //    worksheet.Cell(row, "F").Value = item.porcentaje_excedente;
                            //    row++;
                            //    worksheet.Cell(row, "D").Value = item.cantidad_contrato;
                            //    worksheet.Cell(row, "E").Value = item.monto_prorrateo;
                            //    worksheet.Cell(row, "F").Value = item.monto_prorrateo_excedente;
                            //    row++;
                            //    worksheet.Cell(row, "D").Value = item.precio_unidad;
                            //    worksheet.Cell(row, "E").Value = item.comision_x_meta;
                            //    worksheet.Cell(row, "F").Value = item.comision_excedente;
                            //    worksheet.Cell(row, "G").Value = item.precio_unidad + item.comision_x_meta + item.comision_excedente;
                            //}
                        }

                        //worksheet.Cell(16, "D").Value = double.Parse(worksheet.Cell(9, "D").Value.ToString()) + double.Parse(worksheet.Cell(12, "D").Value.ToString());
                        //worksheet.Cell(16, "E").Value = Math.Round(double.Parse(worksheet.Cell(9, "E").Value.ToString()) + double.Parse(worksheet.Cell(12, "E").Value.ToString()), 0);
                        //worksheet.Cell(16, "F").Value = double.Parse(worksheet.Cell(9, "F").Value.ToString()) + double.Parse(worksheet.Cell(12, "F").Value.ToString());
                        //worksheet.Cell(16, "G").Value = double.Parse(worksheet.Cell(9, "G").Value.ToString()) + double.Parse(worksheet.Cell(12, "G").Value.ToString());


                        /*
                        foreach (var key in personas)
                        {
                            var datos_personales = key.FirstOrDefault().nombres;
                            if (i == 1)
                            {
                                worksheet.Cell(5, "B").Value = datos_personales;
                                //worksheet.Range($"B{5}:D{5}").Value = datos_personales;
                            }
                            if (i == 0)
                            {
                                row++;
                                HEADER_PERSONA.CopyTo(worksheet.Range($"B{row}:D{row}"));
                                worksheet.Range($"B{row}:D{row}").Value = datos_personales;
                                //worksheet.Cell(row, "B").Value = datos_personales;
                                row++;                                
                            }

                            foreach (var item in key)
                            {
                                if (i == 1 && k == 0)
                                {
                                    worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo;
                                    worksheet.Cell(row, "B").Style.Font.Bold = true;
                                    j = item.codigo_tipo_articulo;
                                    row++;
                                }
                                if (i == 0 && k == 0)
                                {
                                    j = item.codigo_tipo_articulo;
                                    worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo;
                                    worksheet.Cell(row, "B").Style.Font.Bold = true;
                                    //worksheet.Range($"C{row}:E{row}").Style.Font.Bold = true;
                                    row++;

                                }

                                if (j != item.codigo_tipo_articulo)
                                {
                                    j = item.codigo_tipo_articulo;
                                    if (j == 0)
                                    {
                                        worksheet.Cell(row, "B").Value = "COMISIÓN POR MONTO INGRESADO";
                                    }
                                    else
                                    {
                                        worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo;
                                    }
                                    row++;

                                    worksheet.Cell(row, "B").Value = item.nombre_grupo;

                                    if (j != 0)
                                    {
                                        worksheet.Cell(row, "C").Value = item.cantidad_contrato;
                                        worksheet.Cell(row, "D").Value = "S/. " +item.precio_unidad.ToString();
                                    }
                                    else
                                    {
                                        worksheet.Cell(row, "D").Value = item.precio_unidad.ToString() + "%";
                                    }
                                    worksheet.Cell(row, "E").Value = item.monto_neto;
                                    worksheet.Cell(row, "E").SetDataType(XLDataType.Number);

                                    row++;
                                }
                                else
                                {
                                    worksheet.Cell(row, "B").Value = item.nombre_grupo;
                                    if (j != 0)
                                    {
                                        worksheet.Cell(row, "C").Value = item.cantidad_contrato;
                                        worksheet.Cell(row, "D").Value = "S/. " + item.precio_unidad.ToString();
                                    }
                                    else
                                    {
                                        worksheet.Cell(row, "D").Value = item.precio_unidad.ToString() + "%";
                                    }
                                    worksheet.Cell(row, "E").Value = item.monto_neto;
                                    worksheet.Cell(row, "E").SetDataType(XLDataType.Number);

                                    row++;
                                    j = item.codigo_tipo_articulo;
                                }
                                k++;
                            }
                            worksheet.Cell(row, "B").Value = "TOTAL";
                            worksheet.Cell(row, "B").Style.Font.Bold = true;
                            worksheet.Range($"B{row}:D{row}").Merge();
                            worksheet.Cell(row, "E").Value = key.Sum(x => x.monto_neto);

                            worksheet.Range($"B{row}:E{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"B{row}:E{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"B{row}:E{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                            row++;

                            i = 0;
                            k = 0;
                            j = 0;
                        }*/

                        workbook.SaveAs(Path.Combine(urlBase, Path.Combine("Temp", FileName)));
                        return (ReadMemoryToBytes(workbook), FileName);

                    }
                }
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return (null, FileName);
        }

        public (byte[], string) GenerarDetalleContratoComisionExcel(List<reporte_detallado_vendedores_dto> data, string codigo_canal)
        {

            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            var FileName = $"CIX_CONTRATOS_COMISION_{DateTime.Now.Year}_{numRandom}.xlsx";

            try
            {
                string urlPlantilla = string.Empty;
                if (codigo_canal.Equals("4"))
                {
                    urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_detalle_contratos_supervisor_chiclayo.xlsx");
                }
                else
                {
                    urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_detalle_contratos_jefatura_chiclayo.xlsx");
                }

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Where(x => x.Name == "REPORTE").FirstOrDefault();

                        var HEADER = worksheet.Range("B6:E6");
                        var HEADER_PERSONA = worksheet.Range("B5:E5");

                        int i = 1;                        
                        int row = 7;
                        var personas = data.GroupBy(x => x.codigo_personal);

                        foreach (var key in personas)
                        {
                            var datos_personales = key.FirstOrDefault().nombres;
                            if (i == 1)
                            {
                                worksheet.Cell(5, "B").Value = datos_personales;                                
                            }
                            if (i == 0)
                            {
                                row++;
                                HEADER_PERSONA.CopyTo(worksheet.Range($"B{row}:D{row}"));
                                worksheet.Cell(row, "B").Value = datos_personales;
                                //worksheet.Range($"B{row}:D{row}").Value = datos_personales;
                                row++;
                                HEADER.CopyTo(worksheet.Range($"B{row}:D{row}"));
                                row++;
                            }

                            foreach (var item in key)
                            {
                                worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo;
                                worksheet.Cell(row, "C").Value = item.nro_contrato;
                                worksheet.Cell(row, "D").Value = item.monto_total_contrato;
                                worksheet.Cell(row, "E").Value = item.monto_total_cuota_inicial;
                                worksheet.Cell(row, "E").SetDataType(XLDataType.Number);
                                worksheet.Range($"B{row}:E{row}").Style.Font.FontSize = 9;
                                worksheet.Range($"B{row}:E{row}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                worksheet.Range($"B{row}:E{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                worksheet.Range($"B{row}:E{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                worksheet.Range($"B{row}:E{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                row++;                                
                            }
                            worksheet.Cell(row, "B").Value = "TOTAL";
                            worksheet.Cell(row, "B").Style.Font.Bold = true;
                            worksheet.Range($"B{row}:C{row}").Merge();

                            worksheet.Cell(row, "D").Value = key.Sum(x => x.monto_total_contrato);
                            worksheet.Cell(row, "E").Value = key.Sum(x => x.monto_total_cuota_inicial);
                            worksheet.Cell(row, "D").SetDataType(XLDataType.Number);
                            worksheet.Cell(row, "E").SetDataType(XLDataType.Number);
                            worksheet.Range($"B{row}:E{row}").Style.Font.FontSize = 9;
                            worksheet.Range($"B{row}:E{row}").Style.Font.Bold = true;
                            worksheet.Range($"B{row}:E{row}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"B{row}:E{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"B{row}:E{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Range($"B{row}:E{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                            row++;

                            i = 0;
                        }

                        workbook.SaveAs(Path.Combine(urlBase, Path.Combine("Temp", FileName)));
                        return (ReadMemoryToBytes(workbook), FileName);

                    }
                }





            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            return (null, FileName);
        }


        public ActionResult ExportarExcel(string fileName)
        {


            //string FileType = "Excel";
            string ContentType = "application/vnd.ms-excel";

            List<reporte_comercial_dto> lst = new List<reporte_comercial_dto>();
            try
            {
                string urlPlantilla = HostingEnvironment.MapPath($"~/Plantilla/Temp/{fileName}");

                var array = ReadFileToBytes(urlPlantilla);
                //File.Delete(urlPlantilla);
                System.IO.File.Delete(urlPlantilla);
                //string reportType = FileType;
                //string mimeType;
                //string encoding;
                //string fileNameExtension;
                //Warning[] warnings;
                //string[] streams;
                //byte[] renderedBytes = rpt.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                return File(array, ContentType, fileName);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }
            finally
            {

            }
            return null;
        }

        #endregion 
    }
}
