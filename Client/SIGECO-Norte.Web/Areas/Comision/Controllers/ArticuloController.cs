using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Globalization;
using SIGEES.Web.Areas.Comision.Models;
using System.Text;
using SIGEES.Web.MemberShip.Filters;
using System.Configuration;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    //[RequiresAuthentication]
    public class ArticuloController : Controller
    {
        //
        // GET: /Comision/Articulo/


        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly EventoUsuarioService _eventoUsuarioService;
        private readonly ArticuloService _service;
        private readonly EmpresaSIGECOService _empresaService;
        private readonly TipoVentaService _tipoVentaService;
        private readonly MonedaService _monedaService;
        private readonly CanalGrupoService _canalService;
        private readonly TipoPagoService _tipoPagoService;
        private readonly TipoComisionService _tipoComisionService;
        private readonly TipoArticuloService _tipoArticuloService;

        private readonly UnidadNegocioService _unidadNegocioService;
        private readonly CategoriaService _categoriaService;

        private readonly IParametroSistemaService _IParametroSistemaService;


        // private articulo _articulo = null;

        #region Inicializacion de Controller - Menu
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        //}
        public ArticuloController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _eventoUsuarioService = new EventoUsuarioService();
            _service = new ArticuloService();
            _empresaService = new EmpresaSIGECOService();
            _tipoVentaService = new TipoVentaService();
            _monedaService = new MonedaService();
            _tipoPagoService = new TipoPagoService();
            _canalService = new CanalGrupoService();
            _tipoComisionService = new TipoComisionService();
            _tipoArticuloService = new TipoArticuloService();
            _unidadNegocioService = new UnidadNegocioService();
            _categoriaService = new CategoriaService();
            _IParametroSistemaService = new ParametroSistemaService();
        }
        #endregion

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

                string mens = ex.Message;
            }

            return View(bean);
        }

        public ActionResult GetAllJson(string nombre)
        {
            int v_total = 0;
            articulo_dto busqueda = new articulo_dto();
            busqueda.nombre = nombre;
            int sede = int.Parse(ConfigurationManager.AppSettings["sede"].ToString());
            var lista = ArticuloBL.Instance.Listar(busqueda, sede);
            v_total = lista.Count;

            return Content(JsonConvert.SerializeObject(new { total = v_total, rows = lista }), "application/json");
        }

        public ActionResult GetSingleJson(int codigo)
        {
            string result = this._service.GetSingleJson(codigo);
            return Content(result, "application/json");
        }

        public ActionResult GetEmpresaJson()
        {
            string result = this._empresaService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetTipoVentaJson()
        {
            string result = this._tipoVentaService.GetAllComboAbreaviaturaJson();
            return Content(result, "application/json");
        }


        public ActionResult GetTipoArticuloJson()
        {
            string result = this._tipoArticuloService.GetAllComboJson();
            return Content(result, "application/json");
        }


        public ActionResult GetMonedaJson()
        {
            string result = this._monedaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetCanalJson()
        {
            string result = this._canalService.GetCanalAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetTipoPagoJson()
        {
            string result = this._tipoPagoService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetUnidadNegocioJson()
        {
            string result = this._unidadNegocioService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetCategoriaJson()
        {
            string result = this._categoriaService.GetAllComboJson();
            return Content(result, "application/json");
        }
        public ActionResult GetComisionaPorJson()
        {
            string result = this._tipoComisionService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetTipoComisionSupervisorJson()
        {
            string result = TipoComisionSupervisorBL.Instance.Listar();
            return Content(result, "application/json");
        }

        public ActionResult GetPreciobyArticuloJson(int codigoArticulo)
        {
            List<precio_articulo_dto> lista = new List<precio_articulo_dto>();
            try
            {
                precio_articulo_dto busqueda = new precio_articulo_dto();
                busqueda.codigo_articulo = Convert.ToInt32(codigoArticulo);
                lista = Precio_ArticuloBL.Instance.Listar(busqueda);
            }
            catch (Exception ex)
            {

                string mensaje = ex.Message;
            }

            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetReglasbyPrecioJson(int codigoPrecio, List<regla_calculo_comision_dto> lst_regla_calculo_comision, List<regla_calculo_comision_dto> lst_eliminados)
        {
            var resumen = new List<regla_calculo_comision_dto>();

            if (lst_regla_calculo_comision == null)
            {
                lst_regla_calculo_comision = new List<regla_calculo_comision_dto>();
            }

            if (lst_eliminados == null)
            {
                lst_eliminados = new List<regla_calculo_comision_dto>();
            }
            
            var resultado = lst_regla_calculo_comision.FindAll(x => x.codigo_precio == codigoPrecio);
            var lista = ReglaCalculoComisionBL.Instance.ListarByPrecio(codigoPrecio);
            var validarEliminados = lst_eliminados.FindAll(x => x.codigo_precio == codigoPrecio);
            
            foreach (var item in lista)
            {
                if ( (!resultado.Exists(x => x.codigo_regla == item.codigo_regla)) && (!validarEliminados.Exists(x => x.codigo_regla == item.codigo_regla)) )
                {
                    resumen.Add(item);
                }
            }

            foreach (var item in resultado)
            {
                resumen.Add(item);
            }

            return Content(JsonConvert.SerializeObject(resumen), "application/json");
        }

        public ActionResult GetComisionPrecioSupervisorJson(int codigoPrecio, List<comision_precio_supervisor_dto> lst_comision_precio_supervisor, List<comision_precio_supervisor_dto> lst_eliminados)
        {
            var resumen = new List<comision_precio_supervisor_dto>();

            if (lst_comision_precio_supervisor == null)
            {
                lst_comision_precio_supervisor = new List<comision_precio_supervisor_dto>();
            }

            if (lst_eliminados == null)
            {
                lst_eliminados = new List<comision_precio_supervisor_dto>();
            }

            var resultado = lst_comision_precio_supervisor.FindAll(x => x.codigo_precio == codigoPrecio);
            var lista = ComisionPrecioSupervisorBL.Instance.ListarByPrecio(codigoPrecio);
            var validarEliminados = lst_eliminados.FindAll(x => x.codigo_precio == codigoPrecio);
            
            foreach (var item in lista)
            {
                if ((!resultado.Exists(x => x.codigo_comision == item.codigo_comision)) && (!validarEliminados.Exists(x => x.codigo_comision == item.codigo_comision)))
                {
                    resumen.Add(item);
                }
            }

            foreach (var item in resultado)
            {
                resumen.Add(item);
            }

            return Content(JsonConvert.SerializeObject(resumen), "application/json");
        }

        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Registrar(int p_codigo_articulo)
        {
            articulo_dto item = new articulo_dto();
            try
            {
                if (p_codigo_articulo > 0)
                {
                    item = ArticuloBL.Instance.BuscarById(p_codigo_articulo);
                }
                else
                {
                    item.codigo_articulo = p_codigo_articulo;
                    item.estado_registro = true;
                }
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }


        [HttpGet]
        [RequiresAuthentication]
        public ActionResult _Visualizar(int p_codigo_articulo)
        {
            articulo_dto item = new articulo_dto();
            try
            {
                item = ArticuloBL.Instance.BuscarById(p_codigo_articulo);
            }
            catch (Exception ex)
            {
                string v_mensaje = ex.Message;
            }

            return PartialView(item);
        }

        [HttpGet]
        public ActionResult _Comision(int p_codigo_precio, int p_estado_registro)
        {
            precio_articulo_dto pc = new precio_articulo_dto();
            pc.codigo_precio = p_codigo_precio;
            pc.estado_registro = p_estado_registro == 1;
            return PartialView(pc);
        }

        public ActionResult _VisualizarComision(int p_codigo_precio, int p_estado_registro)
        {
            precio_articulo_dto pc = new precio_articulo_dto();
            pc.codigo_precio = p_codigo_precio;
            pc.estado_registro = false;
            return PartialView(pc);
        }

        public ActionResult ValidarRangoFechaPrecio(precio_articulo_dto v_precio, List<precio_articulo_dto> lista_precio_articulo, precio_articulo_dto v_precio_original)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            decimal v_impuesto = 0;
            decimal v_precio_articulo = 0;
            decimal v_igv = 0;
            string validacion = string.Empty;
            try
            {
                parametro_sistema v_parametro = _IParametroSistemaService.GetParametro((int)SIGEES.Web.Areas.Comision.Utils.Parametro.igv);

                v_igv = decimal.Parse(v_parametro.valor);
                if (v_precio.precio_total <= 0)
                {
                    throw new Exception("El precio del artículo no es válido, vuelva intentar.");
                }

                v_impuesto = (v_precio.precio_total * v_igv) / (100 + v_igv);
                v_precio_articulo = v_precio.precio_total - v_impuesto;


                if (lista_precio_articulo == null)
                {
                    lista_precio_articulo = new List<precio_articulo_dto>();
                }

                var resultado = lista_precio_articulo.FindAll(x => x.codigo_empresa == v_precio.codigo_empresa &&
                    x.codigo_moneda == v_precio.codigo_moneda &&
                    x.codigo_tipo_venta == v_precio.codigo_tipo_venta
                    );
                
                if (resultado.Count > 0)
                {
                    var res_1 = resultado.Where(x => x.vigencia_inicio <= v_precio.vigencia_inicio && v_precio.vigencia_inicio <= x.vigencia_fin).ToList();
                    var res_2 = resultado.Where(x => x.vigencia_inicio <= v_precio.vigencia_fin && v_precio.vigencia_fin <= x.vigencia_fin).ToList();

                    if (res_1.Count > 0 || res_2.Count > 0)
                    {
                        throw new Exception("Rango de fecha de vigencia ya existe, vuelva intentar.");
                    }
                }

                if (v_precio.codigo_precio == v_precio_original.codigo_precio)
                {
                    if (!(
                        (v_precio.vigencia_inicio == v_precio_original.vigencia_inicio)
                        && (v_precio.codigo_empresa == v_precio_original.codigo_empresa)
                        && (v_precio.codigo_moneda == v_precio_original.codigo_moneda)
                        && (v_precio.codigo_tipo_venta == v_precio_original.codigo_tipo_venta)
                        && (v_precio.precio_total == v_precio_original.precio_total)
                        ))
                    {
                        validacion = ValidarContraPlanilla(v_precio.vigencia_inicio);
                    }
                    else
                    {
                        validacion = ValidarContraPlanilla(v_precio.vigencia_fin, false);
                    }
                }
                else
                { 
                    validacion = ValidarContraPlanilla(v_precio.vigencia_inicio);
                }

                if (validacion.Length > 0)
                {
                    throw new Exception(validacion);
                }

                vMensaje = "Se registró satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, v_impuesto = v_impuesto, v_precio_articulo = v_precio_articulo }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarRangoFechaComision(precio_articulo_dto v_precio, regla_calculo_comision_dto v_comision, List<regla_calculo_comision_dto> lst_regla_calculo_comision, regla_calculo_comision_dto v_comision_original, regla_calculo_comision_validacion_dto v_validacion)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            string validacion = string.Empty;

            try
            {
                if (lst_regla_calculo_comision == null)
                {
                    lst_regla_calculo_comision = new List<regla_calculo_comision_dto>();
                }

                //if (v_comision.valor <= 0)
                //{
                //    throw new Exception("El campo valor debe ser mayor que cero, vuelva intentar.");
                //}

                if (v_comision.codigo_tipo_comision == 2)
                {
                    if (v_comision.valor > 100)
                    {
                        throw new Exception("El campo valor debe ser menor o igual que 100, vuelva intentar.");
                    }
                }
                if (v_comision.codigo_tipo_comision == 1)
                {
                    if (v_comision.valor > v_precio.precio_total)
                    {
                        throw new Exception("El campo valor debe ser menor o igual del monto " + v_precio.precio_total + " , vuelva intentar.");
                    }
                }
                else if (v_comision.codigo_tipo_comision == 2)
                {
                    if (v_precio.precio_total == 0)
                    {
                        throw new Exception("No se puede porcentuar el valor de " + v_precio.precio_total + " , vuelva intentar.");
                    }
                }

                if (!(v_precio.vigencia_inicio <= v_comision.vigencia_inicio && v_comision.vigencia_fin <= v_precio.vigencia_fin))
                {
                    throw new Exception("Rango de fecha de vigencia comisión se encuentra fuera del rango " + v_precio.vigencia_inicio.ToShortDateString() + "  -  " + v_precio.vigencia_fin.ToShortDateString() + ", vuelva intentar.");
                }

                var resultado = lst_regla_calculo_comision.FindAll(x => x.codigo_canal == v_comision.codigo_canal &&
                    x.codigo_tipo_pago == v_comision.codigo_tipo_pago
                    //&& x.codigo_tipo_comision == v_comision.codigo_tipo_comision
                    );
                
                if (resultado.Count > 0)
                {
                    var res_1 = resultado.Where(x => x.vigencia_inicio <= v_comision.vigencia_inicio && v_comision.vigencia_inicio <= x.vigencia_fin).ToList();
                    var res_2 = resultado.Where(x => x.vigencia_inicio <= v_comision.vigencia_fin && v_comision.vigencia_fin <= x.vigencia_fin).ToList();

                    if (res_1.Count > 0 || res_2.Count > 0)
                    {
                        throw new Exception("Rango de fecha de vigencia de comisión ya existe, vuelva intentar.");
                    }
                }

                if (v_comision.codigo_regla == v_comision_original.codigo_regla && v_comision.codigo_regla > 0)
                {
                    if (!(
                        (v_comision.vigencia_inicio == v_comision_original.vigencia_inicio)
                        && (v_comision.codigo_canal == v_comision_original.codigo_canal)
                        && (v_comision.codigo_tipo_pago == v_comision_original.codigo_tipo_pago)
                        && (v_comision.codigo_tipo_comision == v_comision_original.codigo_tipo_comision)
                        && (v_comision.valor == v_comision_original.valor)
                        ))
                    {
                        v_validacion.fecha = v_comision.vigencia_inicio;
                        validacion = ValidarContraPlanilla(v_validacion);
                    }
                    else
                    {
                        v_validacion.fecha = v_comision.vigencia_fin;
                        validacion = ValidarContraPlanilla(v_validacion, false);
                    }
                }
                else
                {
                    v_validacion.fecha = v_comision.vigencia_inicio;
                    validacion = ValidarContraPlanilla(v_validacion);
                }

                if (validacion.Length > 0)
                {
                    throw new Exception(validacion);
                }

                vMensaje = "Se registró satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarRangoFechaComisionSupervisor(precio_articulo_dto v_precio, comision_precio_supervisor_dto v_comision, List<comision_precio_supervisor_dto> lst_regla_calculo_comision, comision_precio_supervisor_dto v_comision_original, regla_calculo_comision_validacion_dto v_validacion)
        {
            int vResultado = 1;
            string vMensaje = string.Empty;
            string validacion = string.Empty;

            try
            {
                if (lst_regla_calculo_comision == null)
                {
                    lst_regla_calculo_comision = new List<comision_precio_supervisor_dto>();
                }

                //if (v_comision.valor <= 0)
                //{
                //    throw new Exception("El campo valor debe ser mayor que cero, vuelva intentar.");
                //}

                if ((v_comision.codigo_tipo_comision_supervisor == 2) || (v_comision.codigo_tipo_comision_supervisor == 3))//% comision
                {
                    if (v_comision.valor > 100)
                    {
                        throw new Exception("El campo valor debe ser menor o igual que 100, vuelva intentar.");
                    }
                }
                if (v_comision.codigo_tipo_comision_supervisor == 1)//Monto Fijo
                {
                    if (v_comision.valor > v_precio.precio_total)
                    {
                        throw new Exception("El campo valor debe ser menor o igual del monto " + v_precio.precio_total + " , vuelva intentar.");
                    }
                }
                // else if (v_comision.codigo_tipo_comision == 2)
                // {
                // if (v_precio.precio_total == 0)
                // {
                // throw new Exception("No se puede porcentuar el valor de " + v_precio.precio_total + " , vuelva intentar.");
                // }
                // }

                if (!(v_precio.vigencia_inicio <= v_comision.vigencia_inicio && v_comision.vigencia_fin <= v_precio.vigencia_fin))
                {
                    throw new Exception("Rango de fecha de vigencia comisión se encuentra fuera del rango " + v_precio.vigencia_inicio.ToShortDateString() + "  -  " + v_precio.vigencia_fin.ToShortDateString() + ", vuelva intentar.");
                }


                var resultado = lst_regla_calculo_comision.FindAll(x => x.codigo_canal_grupo == v_comision.codigo_canal_grupo
                    && x.codigo_tipo_pago == v_comision.codigo_tipo_pago
                    //&& x.codigo_tipo_comision == v_comision.codigo_tipo_comision
                    );
                if (resultado.Count > 0)
                {
                    var res_1 = resultado.Where(x => x.vigencia_inicio <= v_comision.vigencia_inicio && v_comision.vigencia_inicio <= x.vigencia_fin).ToList();
                    var res_2 = resultado.Where(x => x.vigencia_inicio <= v_comision.vigencia_fin && v_comision.vigencia_fin <= x.vigencia_fin).ToList();

                    if (res_1.Count > 0 || res_2.Count > 0)
                    {
                        throw new Exception("Rango de fecha de vigencia de comisión ya existe, vuelva intentar.");
                    }
                }

                if (v_comision.codigo_comision == v_comision_original.codigo_comision && v_comision.codigo_comision > 0)
                {
                    if (!(
                        (v_comision.vigencia_inicio == v_comision_original.vigencia_inicio)
                        && (v_comision.codigo_canal_grupo == v_comision_original.codigo_canal_grupo)
                        && (v_comision.codigo_tipo_pago == v_comision_original.codigo_tipo_pago)
                        && (v_comision.codigo_tipo_comision_supervisor == v_comision_original.codigo_tipo_comision_supervisor)
                        && (v_comision.valor == v_comision_original.valor)
                        ))
                    {
                        v_validacion.fecha = v_comision.vigencia_inicio;
                        validacion = ValidarContraPlanilla(v_validacion);
                    }
                    else
                    {
                        v_validacion.fecha = v_comision.vigencia_fin;
                        validacion = ValidarContraPlanilla(v_validacion, false);
                    }
                }
                else
                {
                    v_validacion.fecha = v_comision.vigencia_inicio;
                    validacion = ValidarContraPlanilla(v_validacion);
                }

                if (validacion.Length > 0)
                {
                    throw new Exception(validacion);
                }

                vMensaje = "Se registró satisfactoriamente.";
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
        public ActionResult Registrar(articulo_dto v_entidad, List<regla_calculo_comision_dto> lst_regla_calcula_comision, List<comision_precio_supervisor_dto> lst_comision_precio_supervisor)
        {

            int vResultado = 1;
            Int64 v_codigo_articulo;
            string vMensaje = string.Empty;
            try
            {
                if (lst_regla_calcula_comision == null)
                {
                    lst_regla_calcula_comision = new List<regla_calculo_comision_dto>();
                }

                if (lst_comision_precio_supervisor == null)
                {
                    lst_comision_precio_supervisor = new List<comision_precio_supervisor_dto>();
                }

                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                if (beanSesionUsuario != null)
                    v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                else
                    v_entidad.usuario_registra = "root";

                foreach (var item in lst_regla_calcula_comision)
                {
                    item.vigencia_fin = DateTime.ParseExact(item.str_vigencia_fin, "d/M/yyyy", CultureInfo.InvariantCulture);
                    item.vigencia_inicio = DateTime.ParseExact(item.str_vigencia_inicio, "d/M/yyyy", CultureInfo.InvariantCulture);
                    item.usuario_registra = v_entidad.usuario_registra;
                    //item.estado_registro = true;
                }

                foreach (var item in lst_comision_precio_supervisor)
                {
                    item.vigencia_fin = DateTime.ParseExact(item.str_vigencia_fin, "d/M/yyyy", CultureInfo.InvariantCulture);
                    item.vigencia_inicio = DateTime.ParseExact(item.str_vigencia_inicio, "d/M/yyyy", CultureInfo.InvariantCulture);
                    item.usuario_registra = v_entidad.usuario_registra;
                    //item.estado_registro = true;
                }

                foreach (var item in v_entidad.lista_precio_articulo)
                {
                    item.usuario_registra = v_entidad.usuario_registra;
                    item.vigencia_fin = DateTime.ParseExact(item.str_vigencia_fin, "d/M/yyyy", CultureInfo.InvariantCulture);
                    item.vigencia_inicio = DateTime.ParseExact(item.str_vigencia_inicio, "d/M/yyyy", CultureInfo.InvariantCulture);

                    item.lst_regla_calcula_comision = lst_regla_calcula_comision.FindAll(x => x.codigo_precio == item.codigo_precio);
                    item.lst_comision_supervisor = lst_comision_precio_supervisor.FindAll(x => x.codigo_precio == item.codigo_precio);
                    //item.estado_registro = true;
                }

                MensajeDTO v_mensaje = ArticuloBL.Instance.Insertar(v_entidad);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }
                v_codigo_articulo = v_mensaje.idRegistro;
                vMensaje = "Se registró satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
                v_codigo_articulo = 0;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje, codigo_articulo = v_codigo_articulo }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [RequiresAuthentication]
        public ActionResult _Eliminar(int p_codigo_articulo)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                articulo_dto v_entidad = new articulo_dto();
                v_entidad.codigo_articulo = p_codigo_articulo;
                v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                MensajeDTO v_mensaje = ArticuloBL.Instance.Eliminar(v_entidad);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }

                vMensaje = "Se desactivó satisfactoriamente.";
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
        public ActionResult _Activar(int p_codigo_articulo)
        {

            int vResultado = 1;
            string vMensaje = string.Empty;
            try
            {
                beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
                articulo_dto v_entidad = new articulo_dto();
                v_entidad.codigo_articulo = p_codigo_articulo;
                v_entidad.usuario_registra = beanSesionUsuario.codigoUsuario;
                MensajeDTO v_mensaje = ArticuloBL.Instance.Activar(v_entidad);
                if (v_mensaje.idOperacion != 1)
                {
                    throw new Exception(v_mensaje.mensaje);
                }

                vMensaje = "Se activó satisfactoriamente.";
            }
            catch (Exception ex)
            {
                vResultado = -1;
                vMensaje = ex.Message;
            }
            return Json(new { v_resultado = vResultado, v_mensaje = vMensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult _Reporte(int p_codigo_articulo)
        {
            ReporteViewModel vm = new ReporteViewModel();
            StringBuilder sbReporte = new StringBuilder();
            string vUrl = Url.Action("frm_reporte_articulo", "Areas/Comision/Reporte/Articulo/frm", new { area = string.Empty }) + ".aspx?p_codigo_articulo=" + p_codigo_articulo;

            try
            {

                vm.url = vUrl;
            }
            catch (Exception ex)
            {
                string mensa = ex.Message;
            }
            return PartialView(vm);
        }

        private string ValidarContraPlanilla(DateTime fecha, bool inicio = true)
        {
            planilla_resumen_dto planilla = PlanillaSelBL.Instance.BuscarUltimoCerrado();
            string fecha_fin_planilla = Fechas.formatterStringDMYToStringCompare(planilla.fecha_fin);
            string fecha_inicio_rango = Fechas.convertDateToStringCompare(fecha);
            string retorno = string.Empty;

            if ((inicio && Convert.ToInt32(fecha_fin_planilla) >= Convert.ToInt32(fecha_inicio_rango)) || (!inicio && Convert.ToInt32(fecha_fin_planilla) > Convert.ToInt32(fecha_inicio_rango)))
            {
                retorno = "Rango de vigencia inválido, existe planilla con fecha " + planilla.fecha_fin + " y la fecha " + (inicio ? "Inicio" : "Fin") + " debe ser mayor" + (inicio?"":"/igual") + ".";
            }
            
            return retorno;
        }


        private string ValidarContraPlanilla(regla_calculo_comision_validacion_dto comision, bool inicio = true)
        {
            string retorno = string.Empty;
            planilla_resumen_dto planilla = PlanillaSelBL.Instance.BuscarUltimoAbiertoPorArticulo(comision);

            if (planilla.codigo_planilla != 0)
            {
                string fecha_fin_planilla = Fechas.formatterStringDMYToStringCompare(planilla.fecha_fin);
                string fecha_inicio_rango = Fechas.convertDateToStringCompare(comision.fecha);

                if ((inicio && Convert.ToInt32(fecha_fin_planilla) >= Convert.ToInt32(fecha_inicio_rango)) || (!inicio && Convert.ToInt32(fecha_fin_planilla) > Convert.ToInt32(fecha_inicio_rango)))
                {
                    retorno = "Rango de vigencia inválido por planilla " + planilla.nombre_planilla + " (" + planilla.estado_planilla + ") de fechas: " + planilla.fecha_inicio + " - " + planilla.fecha_fin + ".<br><br> La Vigencia " + (inicio ? "Inicio" : "Fin") + " debe ser mayor" + (inicio ? "" : "/igual") + " a: " + planilla.fecha_fin + ".";
                }
            }
            return retorno;
        }

    }
}
