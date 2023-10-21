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
using System.Configuration;
//using SIGEES.Web.Areas.Comision.Entity;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class LogContratoSAPController : Controller
    {
        //
        // GET: /Comision/LogContratoSAP/

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly CanalGrupoService _canalService;

        // private canal_grupo _canal_grupo = null;

        #region Inicializacion de Controller - Menu
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public LogContratoSAPController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _canalService = new CanalGrupoService();
        }
        #endregion

        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        public ActionResult GetAllJson(string fecha_inicio, string fecha_fin, string codigo_canal)
        {
            log_contrato_sap_fechas_dto busqueda = new log_contrato_sap_fechas_dto
            {
                fecha_inicio = fecha_inicio,
                fecha_fin = fecha_fin,
                codigo_canal = codigo_canal
            };
            int sede= int.Parse(ConfigurationManager.AppSettings["sede"].ToString());
            var lista = LogContratoSAPBL.Instance.Listar(busqueda, sede);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson(true);
            return Content(result, "application/json");
        }

        public ActionResult GetFechas()
        {
            var lista = LogContratoSAPBL.Instance.Fechas();
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpGet]
        public ActionResult _Detalle(string codigo_empresa, string nro_contrato)
        {
            log_contrato_sap_detalle_dto item = new log_contrato_sap_detalle_dto();
            try
            {
                item = LogContratoSAPBL.Instance.Detalle(codigo_empresa, nro_contrato);
            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpGet]
        public ActionResult _AnalisisComision(int codigo_empresa, string nro_contrato)
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

        public ActionResult GetSingleJson(string codigo_empresa, string nro_contrato)
        {
            var lista = LogContratoSAPBL.Instance.Detalle(codigo_empresa, nro_contrato);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
        public ActionResult HabilitarReproceso(List<log_contrato_sap_habilitar_dto> lstContratos)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (lstContratos == null)
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                StringBuilder xmlContratos = new StringBuilder();

                xmlContratos.Append("<contratos>");
                foreach (log_contrato_sap_habilitar_dto contrato in lstContratos)
                {
                    xmlContratos.Append("<contrato codigo_empresa='" + contrato.codigo_empresa.ToString() + "' nro_contrato='" + contrato.nro_contrato.ToString() + "' />");
                }
                xmlContratos.Append("</contratos>");
                string procesar = xmlContratos.ToString();

                respuesta = LogContratoSAPBL.Instance.HabilitarReproceso(procesar, beanSesionUsuario.codigoUsuario);

                if (respuesta.idOperacion != -1)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", respuesta.mensaje);
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

    }
}
