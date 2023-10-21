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
    public class DescuentoComisionController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly EmpresaSIGECOService _empresaService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public DescuentoComisionController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _empresaService = new EmpresaSIGECOService();
        }
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }
        
        public ActionResult GetAllJson()
        {
            var lista = DescuentoComisionBL.Instance.Listar();
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetEmpresaJson()
        {
            string result = this._empresaService.GetAllComboJson();
            return Content(result, "application/json");
        }
		
        [HttpGet]
        public ActionResult _Registro(int codigo_descuento_comision)
        {
            descuento_comision_dto item = new descuento_comision_dto();
            try
            {
				item.codigo_descuento_comision = codigo_descuento_comision;
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpGet]
        public ActionResult _Detalle(int codigo_descuento_comision)
        {
            descuento_comision_dto busqueda = new descuento_comision_dto { codigo_descuento_comision = codigo_descuento_comision };
            descuento_comision_detalle_dto detalle = null;
            try
            {
                detalle = new DescuentoComisionBL().Detalle(busqueda);
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(detalle);
        }

        [HttpGet]
        public ActionResult _Busqueda_Personal()
        {
            return PartialView();
        }

        public ActionResult GetPersonalJson(string nombre)
        {
            var lista = new PersonalBL().ListarParaComisionManual(nombre);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetPlanillaJson(string codigo_descuento_comision)
        {
            descuento_comision_dto busqueda = new descuento_comision_dto { codigo_descuento_comision = Convert.ToInt32(codigo_descuento_comision) };

            var lista = new DescuentoComisionBL().DetallePlanilla(busqueda);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
        public ActionResult Guardar(descuento_comision_dto descuento_comision)
        {
            JObject jo = new JObject();
            bool esNuevo = descuento_comision.codigo_descuento_comision == -1 ? true : false;
            MensajeDTO respuesta = null;
            int codigoReglaPrevio = 0;

            try
            {

                codigoReglaPrevio = DescuentoComisionBL.Instance.Validar(descuento_comision);

                if (codigoReglaPrevio == 0)
                {
                    descuento_comision.usuario = beanSesionUsuario.codigoUsuario;
                    if (esNuevo)
                    {
                        respuesta = DescuentoComisionBL.Instance.Insertar(descuento_comision);
                    }
                    //else
                    //{
                    //    respuesta = DescuentoComisionBL.Instance.Actualizar(descuento_comision);
                    //}

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
                    jo.Add("Msg", "Existe un descuento activo para este vendedor.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.ToString());
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Desactivar(string codigo_descuento_comision)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (string.IsNullOrWhiteSpace(codigo_descuento_comision))
            {
                jo.Add("Msg", "NO SELECCIONO UN REGISTRO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                descuento_comision_dto descuento_comision = new descuento_comision_dto { 
                    codigo_descuento_comision = Convert.ToInt32(codigo_descuento_comision),
                    usuario = beanSesionUsuario.codigoUsuario
                };

                respuesta = DescuentoComisionBL.Instance.Desactivar(descuento_comision);
                
                if (respuesta.idOperacion == 1)
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

        [HttpPost]
        public ActionResult GenerarDescuento(string codigo_planilla)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (string.IsNullOrWhiteSpace(codigo_planilla))
            {
                jo.Add("Msg", "NO EXISTE PLANILLA.");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                descuento_comision_generar_dto descuento_comision = new descuento_comision_generar_dto {
                    codigo_planilla = Convert.ToInt32(codigo_planilla),
                    usuario = beanSesionUsuario.codigoUsuario
                };

                respuesta = DescuentoComisionBL.Instance.GenerarDescuento(descuento_comision);

                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                    jo.Add("Cantidad", respuesta.total_registro_afectado.ToString());
                }
                else
                {
                    jo.Add("Msg", "Error al generar descuentos.");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        [HttpPost]
        public ActionResult ValidarPlanilla(string codigo_planilla)
        {
            JObject jo = new JObject();
            MensajeDTO respuesta;

            if (string.IsNullOrWhiteSpace(codigo_planilla))
            {
                jo.Add("Msg", "NO EXISTE PLANILLA.");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            try
            {
                descuento_comision_generar_dto descuento_comision = new descuento_comision_generar_dto
                {
                    codigo_planilla = Convert.ToInt32(codigo_planilla),
                    usuario = beanSesionUsuario.codigoUsuario
                };

                respuesta = DescuentoComisionBL.Instance.ValidarPlanilla(descuento_comision);

                if (respuesta.idOperacion == 1)
                {
                    jo.Add("Msg", "Success");
                    jo.Add("Cantidad", respuesta.total_registro_afectado.ToString());
                }
                else
                {
                    jo.Add("Msg", "Error al procesar la validacion.");
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
