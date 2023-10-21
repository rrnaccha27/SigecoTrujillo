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

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ReglaCalculoBonoController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly TipoPlanillaService _TipoPlanillaService;
        private readonly PersonalCanalGrupoBL _reglaCanalGrupoBL;
        private readonly ArticuloService _ArticuloService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReglaCalculoBonoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _TipoPlanillaService = new TipoPlanillaService();
            _reglaCanalGrupoBL = new PersonalCanalGrupoBL();
            _ArticuloService = new ArticuloService();
        }
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }
        
        public ActionResult GetCanalGrupoJson(int es_canal_grupo)
        {
            var lista = new CanalGrupoBL().ListarPersonal(es_canal_grupo);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetAllJson(string codigo_tipo_planilla)
        {
            var lista = ReglaCalculoBonoBL.Instance.Listar(Convert.ToInt32(codigo_tipo_planilla));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetTipoPlanillaJson()
        {
            string result = this._TipoPlanillaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetArticulosBusquedaJson(string valor)
        {
            string result = this._ArticuloService.GetAllArticulobyNombreYGeneraBonoJson(valor);
            return Content(result, "application/json");
        }

        public ActionResult GetArticulosJson(string codigo_regla_calculo_bono)
        {
            var lista = ReglaCalculoBonoBL.Instance.ArticuloListar(Convert.ToInt32(codigo_regla_calculo_bono));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetMatrizJson(string codigo_regla_calculo_bono)
        {
            var lista = ReglaCalculoBonoBL.Instance.MatrizListar(Convert.ToInt32(codigo_regla_calculo_bono));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpGet]
        public ActionResult _Registro(int codigo_regla_calculo_bono)
        {
            regla_calculo_bono_dto item = new regla_calculo_bono_dto();
            try
            {
                if (codigo_regla_calculo_bono > 0)
                    item = ReglaCalculoBonoBL.Instance.Unico(codigo_regla_calculo_bono);
                else
                    item.codigo_regla_calculo_bono = codigo_regla_calculo_bono;
            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpGet]
        public ActionResult _Detalle(int codigo_regla_calculo_bono)
        {
            regla_calculo_bono_detalle_dto item = new regla_calculo_bono_detalle_dto();
            try
            {
                if (codigo_regla_calculo_bono > 0)
                    item = ReglaCalculoBonoBL.Instance.Detalle(codigo_regla_calculo_bono);
                else
                    item.codigo_regla_calculo_bono = codigo_regla_calculo_bono;
            }
            catch (Exception ex)
            {

                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpPost]
        public ActionResult Guardar(regla_calculo_bono_dto regla)
        {
            JObject jo = new JObject();
            bool esNuevo = regla.codigo_regla_calculo_bono == -1 ? true : false;
            MensajeDTO respuesta;
            int codigoReglaPrevio = 0;

            try
            {

                codigoReglaPrevio = ReglaCalculoBonoBL.Instance.Validar(regla);

                if (codigoReglaPrevio == 0)
                {
                    regla.usuario = beanSesionUsuario.codigoUsuario;
                    if (esNuevo)
                    {
                        respuesta = ReglaCalculoBonoBL.Instance.Insertar(regla);
                    }
                    else
                    {
                        respuesta = ReglaCalculoBonoBL.Instance.Actualizar(regla);
                    }

                    if (respuesta.idOperacion == 1)
                    {
                        jo.Add("Msg", "Success");
                    }
                    else
                    {
                        jo.Add("Msg", "NO SE PUDO GUARDAR EL REGISTRO.");
                    }
                }
                else
                {
                    jo.Add("Msg", "Existe otra regla con la misma configuración.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Desactivar(string codigo_regla_calculo_bono)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (string.IsNullOrWhiteSpace(codigo_regla_calculo_bono))
            {
                jo.Add("Msg", "POR FAVOR SELECCIONE UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                regla_calculo_bono_dto regla = new regla_calculo_bono_dto();
                regla.codigo_regla_calculo_bono = Convert.ToInt32(codigo_regla_calculo_bono);
                regla.usuario = beanSesionUsuario.codigoUsuario;
                
                respuesta = ReglaCalculoBonoBL.Instance.Desactivar(regla);
                
                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al eliminar");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult GetRegistro(string codigo_regla_calculo_bono)
        {
            //ReglaCalculoBonoBL oBL = new ReglaCalculoBonoBL();
            regla_calculo_bono_dto oBE = new regla_calculo_bono_dto();
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo_regla_calculo_bono))
            {
                jo.Add("Msg", "Codigo Vacio");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            int ID;
            if (!int.TryParse(codigo_regla_calculo_bono, out ID))
            {
                jo.Add("Msg", "Error al parsear codigo_regla_calculo_bono");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                //int codigo_regla_calculo_bono = Convert.ToInt32(codigo_regla_calculo_bono);
                //oBE = ReglaCalculoBonoBL.GetReg(codigo_regla_calculo_bono);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(oBE, JsonRequestBehavior.AllowGet);
        }


    }
}
