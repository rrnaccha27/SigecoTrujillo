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
    public class ReporteComercialController : Controller
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
        public ReporteComercialController()
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
            while (inicio<=DateTime.Now.Year)
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
            List<reporte_resumen_comercial_dto> lst = new List<reporte_resumen_comercial_dto>();
            if (busqueda.codigo_mes == 0 || busqueda.codigo_anio==0)
                return Content(JsonConvert.SerializeObject(lst), "application/json");


            busqueda.fecha_inicio = busqueda.codigo_tipo_reporte == 1 ? Convert.ToDateTime($"01/{busqueda.codigo_mes}/{busqueda.codigo_anio}"): Convert.ToDateTime($"16/{busqueda.codigo_mes}/{busqueda.codigo_anio}");
            busqueda.fecha_fin = busqueda.codigo_tipo_reporte == 1 ? Convert.ToDateTime($"15/{busqueda.codigo_mes}/{busqueda.codigo_anio}") : Convert.ToDateTime($"01/{busqueda.codigo_mes}/{busqueda.codigo_anio}").AddMonths(1).AddDays(-1);


            //busqueda.fecha_fin= busqueda.fecha_inicio.get
            var lista = ReporteGeneralBL.Instance.ReportePlanillaComisionVendedor(busqueda);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [RequiresAuthentication]
        public ActionResult ReporteResumenPlanilla(reporte_comercial_busqueda_dto busqueda)
        {

            List<reporte_resumen_comercial_dto> lst = new List<reporte_resumen_comercial_dto>();
            if (busqueda.codigo_mes == 0 || busqueda.codigo_anio == 0)
                return Content(JsonConvert.SerializeObject(lst), "application/json");

            busqueda.fecha_inicio = busqueda.codigo_tipo_reporte == 1 ? Convert.ToDateTime($"01/{busqueda.codigo_mes}/{busqueda.codigo_anio}") : Convert.ToDateTime($"16/{busqueda.codigo_mes}/{busqueda.codigo_anio}");
            busqueda.fecha_fin = busqueda.codigo_tipo_reporte == 1 ? Convert.ToDateTime($"15/{busqueda.codigo_mes}/{busqueda.codigo_anio}") : Convert.ToDateTime($"01/{busqueda.codigo_mes}/{busqueda.codigo_anio}").AddMonths(1).AddDays(-1);

            var lista = ReporteGeneralBL.Instance.ReportePlanillaComisionVendedor(busqueda);
            bool indicaQuincenal = busqueda.codigo_tipo_reporte == 1;
            string mes = GetMes(busqueda.codigo_mes);
            var (bytes, fileName) = GenerarComisionExcel(lista, indicaQuincenal, mes);
            //Session[v_guid] = v_entidad;
            return Json(new { archivo = Convert.ToBase64String(bytes), fileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        private string GetMes(int codigo) {
            string mes = "No definido";
            switch (codigo) {
                case 1:
                    mes="Enero";
                    break;
                case 2:
                    mes = "Febrero";
                    break;
                
                case 3:
                    mes = "Marzo";
                    break;
                case 4:
                    mes = "Abril";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Junio";
                    break;
                case 7:
                    mes = "Julio";
                    break;
                case 8:
                    mes = "Agosto";
                    break;
                case 9:
                    mes = "Septiembre";
                    break;
                case 10:
                    mes = "Octubre";
                    break;
                case 11:
                    mes = "Noviembre";
                    break;
                case 12:
                    mes = "Diciembre";
                    break;
            }
            return mes;
        }

        [RequiresAuthentication]
        public ActionResult ReporteDetallePlanilla(reporte_comercial_busqueda_dto busqueda)
        {
            List<reporte_resumen_comercial_dto> lst = new List<reporte_resumen_comercial_dto>();
            if (busqueda.codigo_mes == 0 || busqueda.codigo_anio == 0)
                return Content(JsonConvert.SerializeObject(lst), "application/json");

            busqueda.fecha_inicio = busqueda.codigo_tipo_reporte == 1 ? Convert.ToDateTime($"01/{busqueda.codigo_mes}/{busqueda.codigo_anio}") : Convert.ToDateTime($"16/{busqueda.codigo_mes}/{busqueda.codigo_anio}");

            busqueda.fecha_fin = busqueda.codigo_tipo_reporte == 1 ? Convert.ToDateTime($"15/{busqueda.codigo_mes}/{busqueda.codigo_anio}") : Convert.ToDateTime($"01/{busqueda.codigo_mes}/{busqueda.codigo_anio}").AddMonths(1).AddDays(-1);

            bool indicaQuincenal= busqueda.codigo_tipo_reporte==1;
            string mes = GetMes(busqueda.codigo_mes);

            //var lista = ReporteGeneralBL.Instance.Detalle(busqueda);
            if (busqueda.generar_orden_pago) {
                var lista = ReporteGeneralBL.Instance.Detalle_OrdenPago(busqueda);
                var (bytes, fileName) = GenerarOrdenPagoComisionExcel(lista, indicaQuincenal, mes);
                return Json(new { archivo = Convert.ToBase64String(bytes), fileName = fileName }, JsonRequestBehavior.AllowGet);
            } 
            else {
                var lista = ReporteGeneralBL.Instance.Detalle(busqueda);
                var (bytes, fileName) = GenerarDetalleComisionExcel(lista, indicaQuincenal, mes, busqueda.codigo_anio);
                return Json(new { archivo = Convert.ToBase64String(bytes), fileName = fileName }, JsonRequestBehavior.AllowGet);
            }
            
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

        private string GetDimension2(int codigoTipoArticulo) {
            string canal = "CANAL NO DEFINIDO";
            switch (codigoTipoArticulo) {
                case 1:// Espacio
                    canal = "CS";
                    break;
                case 2://Cremacion
                    canal = "CR";
                    break;
                case 3://Servicios Funerarios
                    canal = "SF";
                    break;
            }
            return canal;    
        }

        private string GetDimension5(int codigoCanal)
        {
            string canal = "CANAL NO DEFINIDO";
            switch (codigoCanal)
            {
                case 3:// oficina
                    canal = "NO DEFINIDO";
                    break;
                case 4://representantes
                    canal = "CON2041";
                    break;
                case 61://gestores
                    canal = "CON2042";
                    break;
            }
            return canal;
        }
        private string GetUnidadNegocio(int codigoTipoArticulo)
        {
            string canal = "CANAL NO DEFINIDO";
            switch (codigoTipoArticulo)
            {
                case 1:// Espacio
                    canal = "Camposanto";
                    break;
                case 2://Cremacion
                    canal = "Cremacion";
                    break;
                case 3://Servicios Funerarios
                    canal = "Funeraria";
                    break;
            }
            return canal;
        }
        private string GetDescripcion(int codigoTipoArticulo)
        {
            string canal = "Descripción no definido";
            switch (codigoTipoArticulo)
            {
                case 1:// Espacio
                    canal = "Comisión por Venta de espacio";
                    break;
                case 2://Cremacion
                    canal = "Comisión por Venta de cremación";
                    break;
                case 3://Servicios Funerarios
                    canal = "Comisión por Venta de servicio funerario";
                    break;
            }
            return canal;
        }
        public (byte[], string) GenerarOrdenPagoComisionExcel(List<reporte_resumen_comercial_dto> data,bool esQuincenal,string periodo)
        {

            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            string proveedor = data.Any() ? data.FirstOrDefault().nombre : "_sin_nombe";
            var FileName = $"TRU_COM_PLANTILLA_ORDEN_DE_SERVICIO_{DateTime.Now.Year}_{proveedor}_{numRandom}.xlsx";

            try
            {
                string periodoDescripcion =esQuincenal? $" 1ra quinc. {periodo}" : $" 2da quinc. {periodo}" ;

                //string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_comision_proveedores_orden_servicio.xlsx");
                string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/planilla_comision_vendedores_mantpower.xlsx");

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Where(x => x.Name == "orden_pago").FirstOrDefault();

                        var datos_personales = data.FirstOrDefault();
                        string nombres = $"{datos_personales.apellido_paterno} {datos_personales.apellido_materno} {datos_personales.nombre}";
                        worksheet.Cell(7, "C").Value = periodo;
                        worksheet.Cell(8, "C").Value = "2023";
                      
                        int rows = 12;
                        var montoComision = 0.00;

                        foreach (var item in data)
                        {
                            worksheet.Cell(rows, "B").Value = item.nro_documento;
                            worksheet.Cell(rows, "C").Value = item.nombre;
                            worksheet.Cell(rows, "D").Value = item.nombre_grupo;
                            worksheet.Cell(rows, "E").Value = item.nombre_supervisor;
                            worksheet.Cell(rows, "F").Value = item.monto_neto_espacio;
                            worksheet.Cell(rows, "G").Value = item.monto_neto_servicio;
                            worksheet.Cell(rows, "H").Value = item.bono_espacio;
                            worksheet.Cell(rows, "I").Value = item.bono_servicio;
                            worksheet.Cell(rows, "J").Value = (item.monto_neto_espacio + item.monto_neto_servicio + item.bono_espacio + item.bono_servicio);

                            montoComision = montoComision + double.Parse((item.monto_neto_espacio + item.monto_neto_servicio + item.bono_espacio + item.bono_servicio).ToString());

                            //worksheet.Cell(rows, "B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ////worksheet.Range($"D{rows}:F{rows}").Merge().Value =$"{GetDescripcion(item.Key)} {periodoDescripcion}";
                            ////worksheet.Cell(rows, "D").Value = GetDescripcion(item.Key); 
                            //worksheet.Cell(rows, "L").Value = montoComision;
                            //worksheet.Cell(rows, "G").Value = "COM";
                            //worksheet.Cell(rows, "G").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            //worksheet.Range($"I{rows}:J{rows}").Merge().Value= nombreCanal;
                            //worksheet.Cell(rows, "H").Value = GetUnidadNegocio(item.Key); ;
                            //worksheet.Cell(rows, "K").Value = "1";
                            //worksheet.Cell(rows, "M").Value = montoComision;
                            rows++;
                        }

                        //rows++;
                        //worksheet.Cell(rows, "C").Value = "TOTAL";
                        worksheet.Range($"B{rows}:I{rows}").Merge().Value = "TOTAL";
                        worksheet.Cell(rows, "B").Style.Font.Bold = true;
                        worksheet.Cell(rows, "J").Value = montoComision;
                        worksheet.Cell(rows, "J").Style.Font.Bold = true;

                        //worksheet.Cell(28, "C").Value = data.Sum(X=>X.monto_bruto);
                        //worksheet.Cell(28, "G").Value = data.Sum(X => X.monto_igv); ;
                        //worksheet.Cell(28, "K").Value = data.Sum(X => X.monto_neto); ;
                        //decimal? detracion = 0;
                        //decimal? porcentajeDetraccion = 10;
                        //decimal? monto_limite =Convert.ToDecimal(699.5);
                        //decimal? monto_neto = data.Sum(X => X.monto_neto);
                        //if (monto_neto >= monto_limite) {
                        //    detracion = monto_neto * (porcentajeDetraccion / 100);
                        //}                        
                        //worksheet.Cell(54, "K").Value = detracion.Value;

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
        public (byte[], string) GenerarDetalleComisionExcel(List<reporte_detallado_vendedores_dto> data, bool esQuincenal, string mes, int codigo_anio)
        {

            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            var FileName = $"TRU_COM_REPORTE_DE_COMISIONES_ACT_{DateTime.Now.Year}_{numRandom}.xlsx";

            try
            {


                string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/planilla_comision_representantes_gestores_chiclayo.xlsx");

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Where(x => x.Name == "REPORTE").FirstOrDefault();

                        IXLWorksheet worksheetFormat = workbook.Worksheets.Where(x => x.Name == "FORMATO").FirstOrDefault();


                        var HEADER_CANAL = worksheetFormat.Range("B6:D6");
                        var HEADER_GRUPO = worksheetFormat.Range("B7:D7");
                        var HEADER_PERSONA = worksheetFormat.Range("B9:D9");
                        var HEADER = worksheetFormat.Range("B10:K10");
                        var FORMATO_DATA = worksheetFormat.Range("B13:K13");
                        //var data_plantilla = worksheet.Range("B11:K11");
                        worksheet.Range("A2:J2").Value = $"COMISIONES PARQUE DEL NORTE S.A - TRUJILLO";
                        //worksheet.Range("A4:J4").Value = $"COMISIONES PARQUE DEL NORTE S.A - TRUJILLO";
                        if (esQuincenal)
                            worksheet.Range("A4:J4").Value = $" 1ra Quincena {mes} {codigo_anio}";
                        else
                            worksheet.Range("A4:J4").Value = $" 2da Quincena {mes}  {codigo_anio}";

                        int row = 7;                        
                        var canal = data.GroupBy(x=>x.codigo_canal);
                        foreach (var c in canal)
                        {
                            var data_canal = c.FirstOrDefault();
                            HEADER_CANAL.CopyTo(worksheet.Range($"B{row}:D{row}"));
                            worksheet.Cell(row, "B").Value = data_canal.nombre_canal;
                            var g = c.GroupBy(x=>x.codigo_grupo);
                            row++;
                            foreach (var gr in g)
                            {
                                var data_grupal = gr.FirstOrDefault();
                                HEADER_CANAL.CopyTo(worksheet.Range($"B{row}:D{row}"));
                                worksheet.Cell(row, "B").Value = data_grupal.nombre_grupo;
                                row=row+2;
                                var personas = gr.GroupBy(x => x.codigo_personal);
                                foreach (var key in personas)
                                {
                                    var datos_personales = key.FirstOrDefault().nombres;
                                    HEADER_PERSONA.CopyTo(worksheet.Range($"B{row}:D{row}"));
                                    worksheet.Range($"B{row}:D{row}").Value = datos_personales;
                                    row++;
                                    HEADER.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                    row++;

                                    #region SECCION-COMISION
                                    if (key.Any(x => x.id_tipo_comision == 1))
                                    {
                                        // row++;
                                        worksheet.Cell(row, "B").Value = "COMISIÓN";
                                        worksheet.Cell(row, "B").Style.Font.Bold = true;
                                        worksheet.Range($"B{row}:K{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        row++;
                                    }
                                    foreach (var item in key.Where(x => x.id_tipo_comision == 1).OrderBy(x => x.fecha_programado).ToList())
                                    {
                                        FORMATO_DATA.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                        //FORMATO_DATA.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                        worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo.ToUpper();
                                        worksheet.Cell(row, "C").Value = item.nombre_tipo_venta.ToUpper();
                                        worksheet.Cell(row, "D").Value = item.nombre_articulo;
                                        worksheet.Cell(row, "E").Value = item.nro_contrato;

                                        worksheet.Cell(row, "F").Value = item.fecha_programado.HasValue ? item.fecha_programado.Value.ToString("dd/MM/yyyy") : String.Empty;
                                        worksheet.Cell(row, "G").Value = item.monto_total_contrato;
                                        worksheet.Cell(row, "G").SetDataType(XLDataType.Number);

                                        worksheet.Cell(row, "H").Value = item.monto_total_cuota_inicial;
                                        worksheet.Cell(row, "I").Value = item.porcentaje_cuota_inicial;
                                        worksheet.Cell(row, "J").Value = item.porcentaje_regla_comision;

                                        worksheet.Cell(row, "K").Value = item.monto_neto;
                                        worksheet.Cell(row, "K").SetDataType(XLDataType.Number);

                                        worksheet.Range($"B{row}:K{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        worksheet.Range($"B{row}:K{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Range($"B{row}:K{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                        row++;
                                    }

                                    if (key.Any(x => x.id_tipo_comision == 2))
                                    {
                                        //row++;
                                        worksheet.Cell(row, "B").Value = "BONO";
                                        worksheet.Cell(row, "B").Style.Font.Bold = true;
                                        worksheet.Range($"B{row}:K{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        row++;
                                    }

                                    foreach (var item in key.Where(x => x.id_tipo_comision == 2).ToList())
                                    {
                                        FORMATO_DATA.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                        //FORMATO_DATA.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                        worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo.ToUpper();
                                        worksheet.Cell(row, "C").Value = item.nombre_tipo_venta.ToUpper();
                                        worksheet.Cell(row, "D").Value = item.nombre_articulo;
                                        worksheet.Cell(row, "E").Value = item.nro_contrato;

                                        worksheet.Cell(row, "F").Value = String.Empty;
                                        worksheet.Cell(row, "G").Value = item.monto_total_contrato;
                                        worksheet.Cell(row, "G").SetDataType(XLDataType.Number);

                                        worksheet.Cell(row, "H").Value = item.monto_total_cuota_inicial;
                                        worksheet.Cell(row, "I").Value = item.porcentaje_cuota_inicial;
                                        worksheet.Cell(row, "J").Value = item.porcentaje_regla_comision;

                                        worksheet.Cell(row, "K").Value = item.monto_neto;
                                        worksheet.Cell(row, "K").SetDataType(XLDataType.Number);

                                        worksheet.Range($"B{row}:K{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        worksheet.Range($"B{row}:K{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Range($"B{row}:K{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                        row++;
                                    }
                                    #endregion
                                    //foreach (var item in key)
                                    //{

                                    //    FORMATO_DATA.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                    //    //FORMATO_DATA.CopyTo(worksheet.Range($"B{row}:K{row}"));
                                    //    worksheet.Cell(row, "B").Value = item.nombre_tipo_articulo.ToUpper();
                                    //    worksheet.Cell(row, "C").Value = item.nombre_tipo_venta.ToUpper();
                                    //    worksheet.Cell(row, "D").Value = item.nombre_articulo;
                                    //    worksheet.Cell(row, "E").Value = item.nro_contrato;
                                    //    worksheet.Cell(row, "F").Value = item.fecha_programado.HasValue? item.fecha_programado.Value.ToString("dd/MM/yyyy"):String.Empty;
                                    //    worksheet.Cell(row, "G").Value = item.monto_total_contrato;
                                    //    worksheet.Cell(row, "G").SetDataType(XLDataType.Number);

                                    //    worksheet.Cell(row, "H").Value = item.monto_total_cuota_inicial;
                                    //    worksheet.Cell(row, "I").Value = item.porcentaje_cuota_inicial;
                                    //    worksheet.Cell(row, "J").Value = item.porcentaje_regla_comision;

                                    //    worksheet.Cell(row, "K").Value = item.monto_neto;
                                    //    worksheet.Cell(row, "K").SetDataType(XLDataType.Number);

                                    //    worksheet.Range($"B{row}:K{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    //    worksheet.Range($"B{row}:K{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    //    worksheet.Range($"B{row}:K{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    //    row++;
                                    //}
                                    worksheet.Cell(row, "B").Value = "TOTAL";
                                    worksheet.Cell(row, "B").Style.Font.Bold = true;
                                    worksheet.Range($"B{row}:J{row}").Merge();

                                    worksheet.Cell(row, "K").Value = key.Sum(x => x.monto_neto);
                                    worksheet.Cell(row, "K").Style.Fill.BackgroundColor = XLColor.FromArgb(209, 209, 255);
                                    worksheet.Range($"B{row}:K{row}").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range($"B{row}:K{row}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range($"B{row}:K{row}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                                    row=row+2;

                                   
                                }
                                row=row+2;

                            }
                            row++;

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

        public (byte[], string) GenerarComisionExcel(List<reporte_resumen_comercial_dto> data, bool indicaQuincenal, string mes)
        {

            Random random = new Random();
            int numRandom = random.Next(1000, 99999);
            var FileName = $"TRU_COM_RESUMEN_DE_COMISIONES_{DateTime.Now.Year}_{numRandom}.xlsx";

            try
            {


                string urlPlantilla = HostingEnvironment.MapPath("~/Plantilla/plantilla_resumen_representantes_gestores_chiclayo.xlsx");

                string urlBase = HostingEnvironment.MapPath("~/Plantilla/");
                var array = ReadFileToBytes(urlPlantilla);
                using (var stream = new MemoryStream(array))
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Where(x => x.Name == "REPORTE").FirstOrDefault();


                        int i = 1;
                        int row = 7;

                        worksheet.Range("B4:K4").Value = indicaQuincenal?$"1ERA QUINCENA {mes.ToUpper()}": $"2DA QUINCENA {mes.ToUpper()}";
                        worksheet.Name = indicaQuincenal ? $"1ERA QUINCENA {mes.ToUpper()}" : $"2DA QUINCENA {mes.ToUpper()}";
                        foreach (var item in data)
                        {

                            worksheet.Cell(row, 2).Value = i.ToString();
                            worksheet.Cell(row, 2).Style.Font.Bold = true;
                            worksheet.Cell(row, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            worksheet.Cell(row, 3).Style.NumberFormat.Format = "@";
                            worksheet.Cell(row, 3).Value = item.nombre;
                            worksheet.Cell(row, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            //worksheet.Cell(row, 4).Style.NumberFormat.Format = "@";
                            //worksheet.Cell(row, 4).SetDataType(XLDataType.Number);
                            worksheet.Cell(row, "D").Value = item.monto_bruto;
                            worksheet.Cell(row, "D").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                            //worksheet.Cell(row, 5).Style.NumberFormat.Format = "@";
                            worksheet.Cell(row, "E").Value = item.monto_igv;
                            worksheet.Cell(row, "E").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 5).SetDataType(XLDataType.Number);
                            //worksheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                            //worksheet.Cell(row, 6).Style.NumberFormat.Format = "@";
                            //worksheet.Cell(row, 6).SetDataType(XLDataType.Number);
                            worksheet.Cell(row, "F").Value = item.monto_neto;
                            worksheet.Cell(row, "F").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 6).Style.Font.Bold = true;
                            //worksheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                            //worksheet.Cell(row, 7).Style.NumberFormat.Format = "@";
                            worksheet.Cell(row, "G").Value = item.monto_neto_espacio;
                            worksheet.Cell(row, "G").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                            //worksheet.Cell(row, 8).Style.NumberFormat.Format = "@";
                            worksheet.Cell(row, "H").Value = item.monto_neto_cremacion;
                            worksheet.Cell(row, "H").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            // worksheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                            //worksheet.Cell(row, 9).Style.NumberFormat.Format = "@";
                            worksheet.Cell(row, "I").Value = item.monto_neto_servicio;
                            worksheet.Cell(row, "I").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                            worksheet.Cell(row, "J").Value = item.bono_espacio;
                            worksheet.Cell(row, "J").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(row, "K").Value = item.bono_cremacion;
                            worksheet.Cell(row, "K").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(row, "L").Value = item.bono_servicio;
                            worksheet.Cell(row, "L").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            worksheet.Cell(row, "M").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 10).Style.NumberFormat.Format = "@";
                            worksheet.Cell(row, "N").Value = item.monto_ir;
                            worksheet.Cell(row, "N").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            //worksheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                            //worksheet.Cell(row, 11).Style.NumberFormat.Format = "@";
                            //worksheet.Cell(row, 11).Value = item.monto_neto_otros;
                            //worksheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            i++;
                            row++;
                        }
                        // row++;

                        var bruto = data.Sum(x => x.monto_bruto);
                        var neto = data.Sum(x => x.monto_neto);
                        var igv = data.Sum(x => x.monto_igv);
                        var comision_campo = data.Sum(x => x.monto_neto_espacio);
                        var comision_cremacion = data.Sum(x => x.monto_neto_cremacion);
                        var comision_servicio = data.Sum(x => x.monto_neto_servicio);
                        var bono_campo = data.Sum(x => x.bono_espacio);
                        var bono_cremacion = data.Sum(x => x.bono_cremacion);
                        var bono_servicio = data.Sum(x => x.bono_servicio);
                        var ir = data.Sum(x => x.monto_ir);

                        worksheet.Range($"B{row}:C{row}").Merge().Value = "TOTAL";
                        worksheet.Cell(row, 2).Style.Font.Bold = true;
                        //worksheet.Cell(row, 2).Style.Fill.BackgroundColor = XLColor.FromArgb(180, 180, 180);

                        worksheet.Range($"B{row}:N{row}").Style.Font.FontColor = XLColor.FromArgb(255, 255, 255);
                        worksheet.Range($"B{row}:N{row}").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 0, 0);

                        worksheet.Cell(row, "D").Value = bruto;
                        worksheet.Cell(row, "D").Style.Font.Bold = true;

                        worksheet.Cell(row, "E").Value = igv;
                        worksheet.Cell(row, "E").Style.Font.Bold = true;

                        worksheet.Cell(row, "F").Value = neto;
                        worksheet.Cell(row, "F").Style.Font.Bold = true;

                        worksheet.Cell(row, "G").Value = comision_campo;
                        worksheet.Cell(row, "G").Style.Font.Bold = true;

                        worksheet.Cell(row, "H").Value = comision_cremacion;
                        worksheet.Cell(row, "H").Style.Font.Bold = true;

                        worksheet.Cell(row, "I").Value = comision_servicio;
                        worksheet.Cell(row, "I").Style.Font.Bold = true;

                        worksheet.Cell(row, "J").Value = bono_campo;
                        worksheet.Cell(row, "J").Style.Font.Bold = true;

                        worksheet.Cell(row, "K").Value = bono_cremacion;
                        worksheet.Cell(row, "K").Style.Font.Bold = true;

                        worksheet.Cell(row, "L").Value = bono_servicio;
                        worksheet.Cell(row, "L").Style.Font.Bold = true;

                        worksheet.Cell(row, "N").Value = ir;
                        worksheet.Cell(row, "N").Style.Font.Bold = true;

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
        #endregion

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

    }
}

