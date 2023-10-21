using Newtonsoft.Json;
using SIGEES.BusinessLogic;
using SIGEES.Entidades;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.Areas.Comision.Services;
using SIGEES.Web.MemberShip.Filters;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class GestionComisionManualController : Controller
    {
        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly IParametroSistemaService _IParametroSistemaService;
        //private readonly EventoUsuarioService _eventoUsuarioService;

        private readonly TipoPagoService _tipoPagoService;
        private readonly EmpresaSIGECOService _empresaService;
        private readonly CanalGrupoService _canalService;

        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public GestionComisionManualController()
        {
            _IParametroSistemaService = new ParametroSistemaService();
            _tipoAccesoItemService = new TipoAccesoItemService();
            _tipoPagoService = new TipoPagoService();
            _empresaService = new EmpresaSIGECOService();
            _canalService = new CanalGrupoService();
        }

        public ActionResult GetTipoPagoJson()
        {
            string result = this._tipoPagoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetEmpresaJson()
        {
            string result = this._empresaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }

        
        public ActionResult ValidarReferencia(string codigo_comision_manual)
        {
            string result = ComisionManualBL.Instance.ValidarReferencia(Convert.ToInt32(codigo_comision_manual));
            return Content(JsonConvert.SerializeObject(new { mensaje = result}), "application/json");
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

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Registrar(int p_codigo_comision)
        {
            comision_manual_dto v_entidad = new comision_manual_dto();
            try
            {
                parametro_sistema v_parametro;

                if (p_codigo_comision < 0)
                {
                    v_parametro = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.comision_manual_tipo_venta);
                    v_entidad.nombre_tipo_venta = v_parametro.valor.Split(Convert.ToChar("|"))[1];
                    v_entidad.codigo_tipo_venta = Convert.ToInt32(v_parametro.valor.Split(Convert.ToChar("|"))[0]);
                    v_parametro = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.comision_manual_tipo_pago);
                    v_entidad.nombre_tipo_pago = v_parametro.valor.Split(Convert.ToChar("|"))[1];
                    v_entidad.codigo_tipo_pago = Convert.ToInt32(v_parametro.valor.Split(Convert.ToChar("|"))[0]);
                    
                }
                else 
                {
                    v_entidad = ComisionManualBL.Instance.Detalle(p_codigo_comision);
                }
                
               v_parametro = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.igv);
                v_entidad.igv = decimal.Parse(v_parametro.valor);
            }
            catch (Exception  ex)
            {

                string m = ex.Message;
            }

            return PartialView(v_entidad);
        }

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Detalle(int p_codigo_comision)
        {
            comision_manual_dto v_entidad = new comision_manual_dto();
            try
            {
                parametro_sistema v_parametro;
                v_entidad = ComisionManualBL.Instance.Detalle(p_codigo_comision);

                v_parametro = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.igv);
                v_entidad.igv = decimal.Parse(v_parametro.valor);
            }
            catch (Exception ex)
            {

                string m = ex.Message;
            }

            return PartialView(v_entidad);
        }

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Modificar(int p_codigo_comision)
        {
            comision_manual_dto v_entidad = new comision_manual_dto();
            try
            {
                parametro_sistema v_parametro;
                v_entidad = ComisionManualBL.Instance.Detalle(p_codigo_comision);

                v_parametro = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.igv);
                v_entidad.igv = decimal.Parse(v_parametro.valor);
            }
            catch (Exception ex)
            {

                string m = ex.Message;
            }

            return PartialView(v_entidad);
        }

        [HttpGet]        
        public ActionResult _Busqueda_Articulo()
        {
            //ParametroViewModel vm = new ParametroViewModel();
            //vm.codigo_empresa = p_codigo_empresa;
            //vm.numero_contrato = p_nro_contrato;
            //return PartialView(vm);
            return PartialView();
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

        [HttpPost]
        
        public ActionResult GetListarArticuloJson(string codigo_empresa, string nro_contrato, string nombre, string codigo_personal, string codigo_canal, string codigo_tipo_venta, string codigo_tipo_pago)
        {
            var lista = new List<articulo_comision_manual_listado_dto>();
            var busqueda = new articulo_comision_manual_busqueda_dto();
            string v_mensaje = string.Empty;
            int v_operacion = 1;
            int v_total = 0;
            try
            {
                busqueda.codigo_empresa = null;
                //busqueda.nro_contrato = null;

                if (codigo_empresa.Length > 0)
                    busqueda.codigo_empresa = Convert.ToInt32(codigo_empresa);
                if (nro_contrato.Length > 0)
                    busqueda.nro_contrato = nro_contrato;

                busqueda.nombre = nombre;
                busqueda.codigo_personal = Convert.ToInt32(codigo_personal);
                busqueda.codigo_canal = Convert.ToInt32(codigo_canal);
                busqueda.codigo_tipo_venta = Convert.ToInt32(codigo_tipo_venta); 
                busqueda.codigo_tipo_pago = Convert.ToInt32(codigo_tipo_pago);

                lista = ArticuloBL.Instance.ListarParaComisionManual(busqueda);
                v_total = lista.Count;
                if (v_total == 0)
                    throw new Exception("No se encontró artículos segun los filtros establecidos.");
            }
            catch (Exception ex)
            {
                v_mensaje = ex.Message;
                v_operacion = -1;
            }
            //{"total":0,"rows":[],"sucess":false,"message":"error message"}
            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = lista, sucess = v_operacion, message = v_mensaje }), "application/json");

        }

        [HttpPost]
        public ActionResult GetComisionPersonalArticulo(cronograma_pago_filtro p_cronograma_pago_filtro)
        {

            resumen_pago_comision_personal_dto _v_entidad = new resumen_pago_comision_personal_dto();
            string vMensaje = string.Empty;            
            try
            {
                _v_entidad.mensaje_operacion = string.Empty;
                _v_entidad = DetalleCronogramaPagoSelBL.Instance.GetPagoComisionByArticuloPersonal(p_cronograma_pago_filtro);
                _v_entidad.codigo_tipo_operacion = 1;

                if (_v_entidad.total_registro_encontrado>=1)
                {
                    _v_entidad.mensaje_operacion = "Total de registros encontrados " + _v_entidad.total_registro_encontrado;
                }
                if (_v_entidad.total_registro_encontrado == 0)
                {
                    _v_entidad.mensaje_operacion = "No se encontró comisión personal.";
                }

            }
            catch (Exception ex)
            {            
                _v_entidad.codigo_tipo_operacion = -1;
                _v_entidad.mensaje_operacion = ex.Message;
            }
            return Content(JsonConvert.SerializeObject(_v_entidad), "application/json");
            

        }

        [HttpPost]
        public ActionResult ListarAllJson(comision_manual_filtro_dto filtro)
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

            if (!(bean.estadoComisionManualListarAll != null && bean.estadoComisionManualListarAll.CompareTo("A") == 0))
            {
                filtro.usuario = beanSesionUsuario.codigoUsuario;
            }
            else{
                filtro.usuario = string.Empty;
            }
            
            var lista = new ComisionManualBL().Listar(filtro);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        [HttpPost]
        [RequiresAuthentication]
        public ActionResult Registrar_Comision(comision_manual_dto v_entidad)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                
                //v_entidad.estado_registro = false;

                MensajeDTO mensaje;

                mensaje = ComisionManualBL.Instance.Validar(v_entidad);

                if (mensaje.codigoError == 0)
                {
                    if (v_entidad.codigo_comision_manual == 0)
                    {
                        v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                        mensaje = ComisionManualBL.Instance.Insertar(v_entidad);
                    }
                    else
                    {
                        v_entidad.usuario_modifica = beanSesionUsuario.codigoUsuario;
                        mensaje = ComisionManualBL.Instance.Actualizar(v_entidad);
                    }

                    if (mensaje.idOperacion != 1)
                    {
                        throw new Exception(mensaje.mensaje);
                    }

                    vMensaje = (mensaje.mensaje.Length > 0 ? mensaje.mensaje : "Se guard&oacute; los datos satisfactoriamente.");
                }
                else {
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
        public ActionResult ActualizarLimitado(comision_manual_dto v_entidad)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;

            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                MensajeDTO mensaje;

                v_entidad.usuario_modifica = beanSesionUsuario.codigoUsuario;
                mensaje = ComisionManualBL.Instance.ActualizarLimitado(v_entidad);

                if (mensaje.idOperacion != 1)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = (mensaje.mensaje.Length > 0 ? mensaje.mensaje : "Se guard&oacute; los datos satisfactoriamente.");
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        [RequiresAuthentication]
        public ActionResult Desactivar(string codigo_comision_manual)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            var comision = new comision_manual_dto();
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                comision.usuario_modifica = beanSesionUsuario.codigoUsuario;
                comision.codigo_comision_manual = Convert.ToInt32(codigo_comision_manual);                

                MensajeDTO mensaje = ComisionManualBL.Instance.Desactivar(comision);

                if (mensaje.idOperacion != 1 || mensaje.codigoError != 0)
                {
                    throw new Exception(mensaje.mensaje);
                }

                vMensaje = "Se desactiv&oacute; satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }

            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SetFiltroGrilla(comision_manual_filtro_dto v_entidad)
        {
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
            Guid id = Guid.NewGuid();
            string v_guid = id.ToString().Replace('-', '_');
            v_entidad.usuario = beanSesionUsuario.codigoUsuario;

            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);

            if (!(bean.estadoComisionManualListarAll != null && bean.estadoComisionManualListarAll.CompareTo("A") == 0))
            {
                v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
            }
            else
            {
                v_entidad.usuario_registra = string.Empty;
            }

            Session[v_guid] = v_entidad;
            string urlReporte = Url.Action("frm_reporte_comision_manual", "Areas/Comision/Reporte/ComisionManual/frm", new { area = string.Empty }) + ".aspx?p_guid=" + v_guid;

            return Json(new { v_guid = v_guid, v_url = urlReporte }, JsonRequestBehavior.AllowGet);
        }


    }
}
