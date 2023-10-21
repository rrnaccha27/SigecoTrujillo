using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Core;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

using SIGEES.Entidades;
using SIGEES.Web.Areas.Comision.Services;
using System.Globalization;
using SIGEES.BusinessLogic;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class ReglaPagoComisionExcepcionController : Controller
    {
        //
        private readonly EmpresaSIGECOService _EmpresaSigecoService;
        private readonly CampoSantoSigecoService _CampoSantoSigecoService;
        private readonly ITipoAccesoItemService _TipoAccesoItemService;
        private readonly CanalGrupoService _CanalGrupoService;
        private readonly ArticuloService _ArticuloService;
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly IEventoUsuarioService _EventoUsuarioService;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public ReglaPagoComisionExcepcionController()
        {
            _EmpresaSigecoService = new EmpresaSIGECOService();
            _CampoSantoSigecoService = new CampoSantoSigecoService();
            _CanalGrupoService = new CanalGrupoService();
            _EventoUsuarioService = new EventoUsuarioService();
            _TipoAccesoItemService = new TipoAccesoItemService();
            _ArticuloService = new ArticuloService();
        }
        [RequiresAuthentication]
        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _TipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            return View(bean);
        }

        [HttpPost]
        public ActionResult Listar(regla_pago_comision_excepcion_dto parametros)
        {
            var query = new object();
            int total = 0;

            try
            {
                var lst = ReglaPagoComisionExcepcionBL.Instance.Listar(parametros);
                total = lst.Count;

                query = from order in lst.AsEnumerable()
                        select new
                        {
                            codigo = order.codigo_regla,
                            nombre = order.nombre,
                            nombre_empresa = order.nombre_empresa,
                            nombre_campo_santo = order.nombre_campo_santo,
                            nombre_canal_grupo = order.nombre_canal_grupo,
                            nombre_articulo = order.nombre_articulo,
                            valor_promocion = order.valor_promocion,
                            cuotas = order.cuotas,
                            estado_registro = order.estado_registro,
                            indica_estado = order.estado_registro?1:0,
                            estado_registro_nombre = order.estado_registro_nombre,
                            vigencia_inicio_str = order.vigencia_inicio_str,
                            vigencia_fin_str = order.vigencia_fin_str
                        };
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRegistro(int id)
        {
            regla_pago_comision_excepcion_dto regla_pago_comision_dto = new regla_pago_comision_excepcion_dto();
            bool existe = false;
            try
            {
                regla_pago_comision_dto = ReglaPagoComisionExcepcionBL.Instance.BuscarById(id);
                if (regla_pago_comision_dto != null)
                {
                    existe = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new { existe = existe, registro = regla_pago_comision_dto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Registrar(regla_pago_comision_excepcion_dto parametros)
        {
            int vResultado = 1;

            string vMensaje = string.Empty;
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(parametros.vigencia_inicio_str, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(parametros.vigencia_fin_str, "d/M/yyyy", CultureInfo.InvariantCulture);

                if (fechaInicio > fechaFin)
                {
                    throw new Exception("La fecha inicio debe ser menor a la fecha fin");
                }

                parametros.codigo_regla = 0;
                parametros.vigencia_inicio = fechaInicio;
                parametros.vigencia_fin = fechaFin;
                parametros.estado_registro = true;
                parametros.fecha_registra = DateTime.Now;
                parametros.usuario_registra = beanSesionUsuario.codigoUsuario;

                int existeRegla = ReglaPagoComisionExcepcionBL.Instance.Validar(parametros);

                if (existeRegla > 0)
                {
                    throw new Exception("Ya existe una regla para esta configuración de campo santo, empresa, canal y artículo.");
                }

                var v_mensaje = ReglaPagoComisionExcepcionBL.Instance.Insertar(parametros);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se registro satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Modificar(regla_pago_comision_excepcion_dto parametros)
        {
            int vResultado = 1;

            string vMensaje = string.Empty;
            try
            {
                DateTime fechaInicio = DateTime.ParseExact(parametros.vigencia_inicio_str, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(parametros.vigencia_fin_str, "d/M/yyyy", CultureInfo.InvariantCulture);

                if (fechaInicio > fechaFin)
                {
                    throw new Exception("La fecha inicio debe ser menor a la fecha fin");
                }

                int existeRegla = ReglaPagoComisionExcepcionBL.Instance.Validar(parametros);

                if (existeRegla > 0)
                {
                    throw new Exception("Ya existe una regla para esta configuración de campo santo, empresa y canal");
                }

                parametros.vigencia_inicio = fechaInicio;
                parametros.vigencia_fin = fechaFin;
                parametros.estado_registro = true;
                parametros.fecha_modifica = DateTime.Now;
                parametros.usuario_modifica = beanSesionUsuario.codigoUsuario;

                var v_mensaje = ReglaPagoComisionExcepcionBL.Instance.Actualizar(parametros);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se modifico satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Eliminar(regla_pago_comision_excepcion_dto parametros)
        {
            int vResultado = 1;

            string vMensaje = string.Empty;
            try
            {
                parametros.estado_registro = false;
                parametros.fecha_modifica = DateTime.Now;
                parametros.usuario_modifica = beanSesionUsuario.codigoUsuario;

                var v_mensaje = ReglaPagoComisionExcepcionBL.Instance.Eliminar(parametros);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                vMensaje = "Se desactivo satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpresaJSON()
        {
            string result = string.Empty;
            try
            {
                result = this._EmpresaSigecoService.GetAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }

        public ActionResult GetCanalGrupoJSON()
        {
            string result = string.Empty;
            try
            {
                result = this._CanalGrupoService.GetCanalAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }

        public ActionResult GetCampoSantoJSON()
        {
            string result = string.Empty;
            try
            {
                result = this._CampoSantoSigecoService.GetAllComboJson();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Content(result, "application/json");
        }

        public ActionResult GetArticulosJSON(string valor)
        {
            string result = this._ArticuloService.GetAllArticulobyNombreYGeneraBonoJson(valor);
            return Content(result, "application/json");
        }

    }
}
