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
using SIGEES.Web.Areas.Comision.Entity;

using SIGEES.Web.Core;

using SIGEES.Web.Models.Bean;
using SIGEES.Web.Services;
using SIGEES.Web.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Areas.Comision.Models;
using SIGEES.Web.MemberShip.Filters;

namespace SIGEES.Web.Areas.Comision.Controllers
{
    public class GrupoController : Controller
    {
        //
        // GET: /Comision/CanalGrupo/

        private BeanSesionUsuario beanSesionUsuario = new BeanSesionUsuario();
        private readonly ITipoAccesoItemService _tipoAccesoItemService;
        private readonly EventoUsuarioService _eventoUsuarioService;
		
		private readonly CanalGrupoService _canalService = new CanalGrupoService();
        private readonly EmpresaSIGECOService _empresaService;
        private readonly PersonalService _personalService;

        private canal_grupo _canal_grupo = null;

        #region Inicializacion de Controller - Menu
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            beanSesionUsuario = Session[Common.Constante.session_name.sesionUsuario] as BeanSesionUsuario;
        }
        public GrupoController()
        {
            _tipoAccesoItemService = new TipoAccesoItemService();
            _eventoUsuarioService = new EventoUsuarioService();
            _canalService = new CanalGrupoService();
            _empresaService = new EmpresaSIGECOService();
            _personalService = new PersonalService();
        }
        #endregion

        [RequiresAuthentication]
        public ActionResult Index(string id)
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean = _tipoAccesoItemService.GetBeanItemTipoAcceso(beanSesionUsuario.codigoPerfil);
            
            canal_grupo grupo = _canalService.GetSingle(Convert.ToInt32(id));

            ViewBag.codigoPadre = grupo.codigo_canal_grupo;
            ViewBag.nombrePadre = grupo.nombre;
            ViewBag.estado = grupo.estado_registro ? "1" : "0";
            
            return View(bean);
        }

        public ActionResult GetAllJson(string codigoPadre)
        {
            CanalGrupoBL canal_grupoBL = new CanalGrupoBL();

            var lista = canal_grupoBL.Listar(false, Convert.ToInt32(codigoPadre));
			
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }
        
        public ActionResult GetSingleJson(int codigo)
        {
            CanalGrupoBL canal_grupoBL = new CanalGrupoBL();

            var lista = canal_grupoBL.Detalle(codigo);

            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetEmpresaJson()
        {
            string result = this._empresaService.GetAllComboJson();
            return Content(result, "application/json");
        }

        public ActionResult GetPersonalByNombreJson(string texto)
        {
            //string result = _personalService.GetAllDisponiblebyNombreJson(texto);
            //return Content(result, "application/json");
            PersonalBL personal = new PersonalBL();
            var lista = personal.ListarPorNombresCanalGrupo(texto);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetPersonalDisponibleJson()
        {
            string result = _personalService.GetAllDisponibleJson();
            return Content(result, "application/json");
        }

        public ActionResult GetPersonalYaAsignadoJson(int codigoCanalGrupo)
        {
            PersonalBL personalBL = new PersonalBL();
            var lista = personalBL.ListarPorCanalGrupo(codigoCanalGrupo);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetOtrasSupervisioneJson(string codigo_personal, string codigo_canal_grupo)
        {
            PersonalCanalGrupoBL personalCanalGrupoBL = new PersonalCanalGrupoBL();
            var lista = personalCanalGrupoBL.GetOtrasSupervisiones(Convert.ToInt32(codigo_personal), Convert.ToInt32(codigo_canal_grupo));
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        public ActionResult GetCanalGrupoJson(int es_canal_grupo)
        {
            var lista = new CanalGrupoBL().ListarPersonal(es_canal_grupo);
            return Content(JsonConvert.SerializeObject(lista), "application/json");
        }

        //public ActionResult AsignarSupervisor(string codigo_personal, string codigo_canal_grupo, string es_canal_grupo, string percibe_comision, string percibe_bono)
        //{
        //    PersonalCanalGrupoBL personalCanalGrupoBL = new PersonalCanalGrupoBL();
        //    personal_canal_grupo_dto canal_grupo = new personal_canal_grupo_dto();

        //    canal_grupo.codigo_personal = Convert.ToInt32(codigo_personal);
        //    canal_grupo.codigo_canal_grupo = Convert.ToInt32(codigo_canal_grupo);
        //    canal_grupo.percibe_comision = (percibe_comision == "true");
        //    canal_grupo.percibe_bono = (percibe_bono == "true");
        //    canal_grupo.usuario_modifica = beanSesionUsuario.codigoUsuario;

        //    personalCanalGrupoBL.AsignarSupervisor(Convert.ToInt32(es_canal_grupo), canal_grupo);

        //    return Content(null);
        //}

        //public ActionResult DesasignarSupervisor(string codigo_personal, string codigo_canal_grupo, string percibe_comision, string percibe_bono)
        //{
        //    PersonalCanalGrupoBL personalCanalGrupoBL = new PersonalCanalGrupoBL();
        //    personal_canal_grupo_dto canal_grupo = new personal_canal_grupo_dto();

        //    canal_grupo.codigo_personal = Convert.ToInt32(codigo_personal);
        //    canal_grupo.codigo_canal_grupo = Convert.ToInt32(codigo_canal_grupo);
        //    canal_grupo.percibe_comision = (percibe_comision == "true");
        //    canal_grupo.percibe_bono = (percibe_bono == "true");
        //    canal_grupo.usuario_modifica = beanSesionUsuario.codigoUsuario;

        //    personalCanalGrupoBL.DesasignarSupervisor(canal_grupo);

        //    return Content(null);
        //}
        [RequiresAuthentication]
        public ActionResult Transferir(string codigo_canal_grupo_old, string es_canal_grupo, string codigo_personal, string codigo_canal_grupo, string percibe_comision, string percibe_bono)
        {
            JObject jo = new JObject();

            try
            {
                PersonalCanalGrupoBL personalCanalGrupoBL = new PersonalCanalGrupoBL();
                personal_canal_grupo_dto canal_grupo = new personal_canal_grupo_dto();

                canal_grupo.codigo_personal = Convert.ToInt32(codigo_personal);
                canal_grupo.codigo_canal_grupo = Convert.ToInt32(codigo_canal_grupo);
                canal_grupo.percibe_comision = (percibe_comision == "true");
                canal_grupo.percibe_bono = (percibe_bono == "true");
                canal_grupo.usuario_registra = beanSesionUsuario.codigoUsuario;

                personalCanalGrupoBL.Transferir(Convert.ToInt32(es_canal_grupo), Convert.ToInt32(codigo_canal_grupo_old), canal_grupo);
                //jo.Add("Msg", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [RequiresAuthentication]
        [HttpPost]
        public ActionResult Registrar(string codigoPadre, string codigo_cg, string nombre, string codigo_equivalencia, string codigoPersonalOriginal, string codigoPersonal, string administra_grupos, string dataConfiguracion, string accion)
        {
            bool estadoEvento = false;
            string codigo = "NULL";
            string auditoria = string.Empty;
            int codigo_canal_grupo = Convert.ToInt32(codigo_cg);
            string usuario = beanSesionUsuario.codigoUsuario;
            bool esNuevo = (codigo_canal_grupo <= 0);

            JObject jo = new JObject();

            _canal_grupo = new canal_grupo();

            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    jo.Add("Msg", "POR FAVOR INGRESE NOMBRE");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                else if (nombre.Length > 250)
                {
                    jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 250");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }

                CanalGrupoBL canalGrupoBL = new CanalGrupoBL();
                if (canalGrupoBL.ExisteCodigoEquivalencia(codigo_canal_grupo, codigo_equivalencia))
                {
                    throw new Exception("Ya existe el codigo ingresado, vuelva a ingresar otro.");
                }

                if (codigo_canal_grupo > 0)
                {
                    _canal_grupo = this._canalService.GetSingle(codigo_canal_grupo);
                    _canal_grupo.codigo_canal_grupo = codigo_canal_grupo;
                    _canal_grupo.fecha_modifica = DateTime.Now;
                    _canal_grupo.usuario_modifica = usuario;
                }
                else
                {
                    _canal_grupo.fecha_registra = DateTime.Now;
                    _canal_grupo.usuario_registra = usuario;
                }

                _canal_grupo.codigo_padre = Convert.ToInt32(codigoPadre);
                _canal_grupo.nombre = nombre;
                _canal_grupo.codigo_equivalencia = codigo_equivalencia;
                _canal_grupo.administra_grupos = administra_grupos == "true" ? true : false;
                _canal_grupo.es_canal_grupo = false;

                _canal_grupo.estado_registro = true;

                IResult respuesta = new Result();

                if (codigo_canal_grupo > 0)
                {
                    respuesta = this._canalService.Update(_canal_grupo);
                }
                else
                {
                    respuesta = this._canalService.Create(_canal_grupo);
                }

                if (respuesta.Success)
                {
                    estadoEvento = true;
                    codigo = respuesta.IdRegistro;

                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al registrar");
                }

            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            auditoria = RegistrarAuditoria(codigo, TipoAuditoria.Registrar, estadoEvento);
            if (auditoria != string.Empty)
            {
                jo.Add("Msg", auditoria);
            }

            if (estadoEvento)
            {

                ConfiguracionCanalGrupoService servicioConfiguracion = new ConfiguracionCanalGrupoService();
                configuracion_canal_grupo configuracion;
                EmpresaConfiguracionService servicioEmpresaConfiguracion = new EmpresaConfiguracionService();
                empresa_configuracion empresa;
                IResult respuesta;
                IResult respuesta2;
                string supervisorPecibeComision = "0";
                string supervisorPecibeBono = "0";
                string personalPecibeComision = "0";
                string personalPecibeBono = "0";
                string percibe = string.Empty;

                if (codigo_canal_grupo > 0)
                {
                    CanalGrupoBL canal_grupoBL = new CanalGrupoBL();
                    canal_grupoBL.EliminarConfiguracion(codigo_canal_grupo);
                }

                int codigoConfiguracion = 0;

                if (dataConfiguracion.Length > 0)
                { 
                    var listaConfiguraciones = dataConfiguracion.Split(Convert.ToChar("|"));

                    for (int indiceConfiguracion = 0; indiceConfiguracion < listaConfiguraciones.Length; indiceConfiguracion++)
                    {
                        var configuracionUnidad = listaConfiguraciones[indiceConfiguracion].Split(Convert.ToChar(","));

                        configuracion = new configuracion_canal_grupo();

                        configuracion.codigo_canal_grupo = (codigo_canal_grupo != -1) ? codigo_canal_grupo : Convert.ToInt32(codigo);
                        configuracion.supervisor_personal = configuracionUnidad[0] == "true" ? true : false;
                        configuracion.comision_bono = configuracionUnidad[1] == "true" ? true : false;
                        configuracion.percibe = configuracionUnidad[2] == "true" ? true : false;

                        if (configuracion.supervisor_personal && configuracion.comision_bono && configuracion.percibe)
                        {
                            supervisorPecibeComision = "1";
                        }

                        if (configuracion.supervisor_personal && !configuracion.comision_bono && configuracion.percibe)
                        {
                            supervisorPecibeBono = "1";
                        }

                        if (!configuracion.supervisor_personal && configuracion.comision_bono && configuracion.percibe)
                        {
                            personalPecibeComision = "1";
                        }

                        if (!configuracion.supervisor_personal && !configuracion.comision_bono && configuracion.percibe)
                        {
                            personalPecibeBono = "1";
                        }


                        respuesta = servicioConfiguracion.Create(configuracion);

                        if (respuesta.Success && configuracion.percibe)
                        {
                            codigoConfiguracion = Convert.ToInt32(respuesta.IdRegistro);


                            if (configuracionUnidad[3] == "true")
                            {
                                var empresas = configuracionUnidad[4].Split(Convert.ToChar("."));

                                for (int indiceEmpresas = 0; indiceEmpresas < empresas.Length; indiceEmpresas++)
                                {
                                    empresa = new empresa_configuracion();
                                    empresa.codigo_configuracion = codigoConfiguracion;
                                    empresa.planilla_factura = true;
                                    empresa.codigo_empresa = Convert.ToInt32(empresas[indiceEmpresas]);

                                    respuesta2 = servicioEmpresaConfiguracion.Create(empresa);
                                }
                            }

                            if (configuracionUnidad[5] == "true")
                            {

                                var empresas = configuracionUnidad[6].Split(Convert.ToChar("."));
                                for (int indiceEmpresas = 0; indiceEmpresas < empresas.Length; indiceEmpresas++)
                                {
                                    empresa = new empresa_configuracion();
                                    empresa.codigo_configuracion = codigoConfiguracion;
                                    empresa.planilla_factura = false;
                                    empresa.codigo_empresa = Convert.ToInt32(empresas[indiceEmpresas]);

                                    respuesta2 = servicioEmpresaConfiguracion.Create(empresa);
                                }
                            }
                        }

                    }
                }

                codigo_canal_grupo = (codigo_canal_grupo != -1) ? codigo_canal_grupo : Convert.ToInt32(codigo);
                PersonalBL personalBL = new PersonalBL();
                PersonalCanalGrupoBL personalCanalGrupoBL = new PersonalCanalGrupoBL();
                personal_dto nuevoPersonal;
                personal_canal_grupo_dto canal_grupo;
                personal_dto desactivar = new personal_dto();
                //personal_canal_grupo_replicacion_dto replicacion;
                List<personal_canal_grupo_replicacion_dto> listaReplicacion = new List<personal_canal_grupo_replicacion_dto>();

                if (esNuevo)
                {
                    if (codigoPersonal.Length > 0)
                    {
                        listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(Convert.ToInt32(codigoPersonal), codigo_canal_grupo, "E"));
                        
                        nuevoPersonal = personalBL.CrearNuevo(Convert.ToInt32(codigoPersonal), codigo_canal_grupo, usuario);

                        canal_grupo = new personal_canal_grupo_dto();

                        canal_grupo.codigo_personal = Convert.ToInt32(nuevoPersonal.codigo_personal);
                        canal_grupo.codigo_canal_grupo = Convert.ToInt32(codigo_canal_grupo);
                        canal_grupo.percibe_comision = (supervisorPecibeComision == "1");
                        canal_grupo.percibe_bono = (supervisorPecibeBono == "1");
                        canal_grupo.usuario_modifica = usuario;

                        personalCanalGrupoBL.AsignarSupervisor(0, canal_grupo);

                        listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(nuevoPersonal.codigo_personal, codigo_canal_grupo, "N"));
                    }
                }
                else
                {
                    if (codigoPersonal != codigoPersonalOriginal)
                    {

                        if (codigoPersonalOriginal.Length > 0)
                        {
                            var listaAcciones = accion.Split(Convert.ToChar(","));
                            
                            switch (Convert.ToInt32(listaAcciones[0]))
                            {
                                case 1://Transferencia como Vendedor
                                    listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(Convert.ToInt32(codigoPersonalOriginal), codigo_canal_grupo, "A"));
                                    nuevoPersonal = personalBL.CrearNuevo(Convert.ToInt32(codigoPersonalOriginal), codigo_canal_grupo, usuario);
                                    canal_grupo = new personal_canal_grupo_dto();

                                    canal_grupo.codigo_personal = Convert.ToInt32(nuevoPersonal.codigo_personal);
                                    canal_grupo.codigo_canal_grupo = Convert.ToInt32(listaAcciones[1]);
                                    canal_grupo.percibe_comision = (listaAcciones[2] == "1");
                                    canal_grupo.percibe_bono = (listaAcciones[3] == "1");
                                    canal_grupo.usuario_modifica = usuario;

                                    personalCanalGrupoBL.AsignarPersonal(1, canal_grupo);
                                    listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(nuevoPersonal.codigo_personal, codigo_canal_grupo, "N"));
                                    
                                    break;
                                case 2://Anulado
                                    listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(Convert.ToInt32(codigoPersonalOriginal), codigo_canal_grupo, "A"));
                                    desactivar.codigo_personal = Convert.ToInt32(codigoPersonalOriginal);
                                    desactivar.usuario_modifica = usuario;
                                    personalBL.Desactivar(desactivar, false);

                                    break;
                                case 3://Se le quita el registro del canal/grupo
                                    canal_grupo = new personal_canal_grupo_dto();

                                    canal_grupo.codigo_personal = Convert.ToInt32(codigoPersonalOriginal);
                                    canal_grupo.codigo_canal_grupo = Convert.ToInt32(codigo_canal_grupo);
                                    canal_grupo.usuario_modifica = usuario;

                                    personalCanalGrupoBL.DesasignarSupervisor(canal_grupo);
                                    
                                    break;
                            }
                        }

                        listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(Convert.ToInt32(codigoPersonal), codigo_canal_grupo, "A"));
                        nuevoPersonal = personalBL.CrearNuevo(Convert.ToInt32(codigoPersonal), codigo_canal_grupo, usuario);
                        
                        canal_grupo = new personal_canal_grupo_dto();

                        canal_grupo.codigo_personal = Convert.ToInt32(nuevoPersonal.codigo_personal);
                        canal_grupo.codigo_canal_grupo = Convert.ToInt32(codigo_canal_grupo);
                        canal_grupo.percibe_comision = (supervisorPecibeComision == "1") ;
                        canal_grupo.percibe_bono = (supervisorPecibeBono == "1");
                        canal_grupo.usuario_modifica = usuario;

                        personalCanalGrupoBL.AsignarSupervisor(0, canal_grupo);
                        listaReplicacion.Add(personalCanalGrupoBL.BuscarParaReplicacion(nuevoPersonal.codigo_personal, codigo_canal_grupo, "N"));
                    }
                }

                if (listaReplicacion.Count > 0)
                {
                    //TODO: Codigo para LLamar al Servicio SAP
                }
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        //[HttpPost]
        //public ActionResult Modificar(string codigo, string nombre, string administra_grupos, string dataConfiguracion)
        //{
        //    bool estadoEvento = false;
        //    string auditoria = string.Empty;

        //    JObject jo = new JObject();

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(codigo))
        //        {
        //            jo.Add("Msg", "CODIGO REGISTRO NULO");
        //            return Content(JsonConvert.SerializeObject(jo), "application/json");
        //        }
        //        else if (string.IsNullOrWhiteSpace(nombre))
        //        {
        //            jo.Add("Msg", "POR FAVOR INGRESE NOMBRE");
        //            return Content(JsonConvert.SerializeObject(jo), "application/json");
        //        }
        //        else if (nombre.Length > 250)
        //        {
        //            jo.Add("Msg", "NOMBRE, NUMERO MAXIMO DE CARACTERES 250");
        //            return Content(JsonConvert.SerializeObject(jo), "application/json");
        //        }

        //        _canal_grupo = this._canalService.GetSingle(int.Parse(codigo));

        //        _canal_grupo.nombre = nombre;
        //        _canal_grupo.administra_grupos = administra_grupos == "true" ? true : false;

        //        _canal_grupo.estado_registro = true;
        //        _canal_grupo.fecha_modifica = DateTime.Now;
        //        _canal_grupo.usuario_modifica = beanSesionUsuario.codigoUsuario;

        //        IResult respuesta = this._canalService.Update(_canal_grupo);
        //        if (respuesta.Success)
        //        {
        //            estadoEvento = true;
        //            jo.Add("Msg", "Success");
        //        }
        //        else
        //        {
        //            jo.Add("Msg", "Error al modificar");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        jo.Add("Msg", ex.Message);
        //    }

        //    auditoria = RegistrarAuditoria(codigo, TipoAuditoria.Modificar, estadoEvento);
        //    if (auditoria != string.Empty)
        //    {
        //        jo.Add("Msg", auditoria);
        //    }

        //    if (estadoEvento)
        //    {

        //        ConfiguracionCanalGrupoService servicioConfiguracion = new ConfiguracionCanalGrupoService();
        //        configuracion_canal_grupo configuracion;

        //        if (dataConfiguracion.Length > 0)
        //        { 
        //            var listaConfiguraciones = dataConfiguracion.Split(Convert.ToChar("|"));

        //            for (int indiceConfiguracion = 0; indiceConfiguracion < listaConfiguraciones.Length; indiceConfiguracion++)
        //            {
        //                var configuracionUnidad = listaConfiguraciones[indiceConfiguracion].Split(Convert.ToChar(","));
        //                bool nuevo = configuracionUnidad[0].Length == 0;
        //                configuracion = new configuracion_canal_grupo();

        //                configuracion = servicioConfiguracion.GetSingle(int.Parse(configuracionUnidad[0]));

        //                configuracion.codigo_canal_grupo = Convert.ToInt32(codigo);
        //                configuracion.supervisor_personal = configuracionUnidad[0] == "true" ? true : false;
        //                configuracion.comision_bono = configuracionUnidad[1] == "true" ? true : false;
        //                configuracion.percibe = configuracionUnidad[2] == "true" ? true : false;

        //                //configuracion.estado_registro = true;
        //                //configuracion.fecha_modifica = DateTime.Now;
        //                //configuracion.usuario_modifica = beanSesionUsuario.codigoUsuario;
						
        //                IResult respuesta = servicioConfiguracion.Update(configuracion);
        //            }
        //        }


        //    }
        //    return Content(JsonConvert.SerializeObject(jo), "application/json");
        //}

        [RequiresAuthentication]
        [HttpPost]
        public ActionResult Eliminar(string codigo)
        {
            bool estadoEvento = false;
            string auditoria = string.Empty;

            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                jo.Add("Msg", "CODIGO REGISTRO NULO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                _canal_grupo = this._canalService.GetSingle(int.Parse(codigo));
                _canal_grupo.estado_registro = false;
                _canal_grupo.fecha_modifica = DateTime.Now;
                _canal_grupo.usuario_modifica = beanSesionUsuario.codigoUsuario;

                IResult respuesta = this._canalService.Update(_canal_grupo);
                if (respuesta.Success)
                {
                    estadoEvento = true;
                    jo.Add("Msg", "Success");
                }
                else
                {
                    jo.Add("Msg", "Error al desactivar");
                }
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }

            auditoria = RegistrarAuditoria(codigo, TipoAuditoria.Eliminar, estadoEvento);
            if (auditoria != string.Empty)
            {
                jo.Add("Msg", auditoria);
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        private string RegistrarAuditoria(string codigo, TipoAuditoria auditoria, bool estadoEvento)
        {
            string mensaje = string.Empty;
            string titulo = auditoria.ToString().ToUpper();
            try
            {
                List<BeanEntidad> listaBeanEntidad = new List<BeanEntidad>();
                List<BeanAtributo> listaBeanAtributo = new List<BeanAtributo>();
                if (estadoEvento)
                {
                    switch (auditoria)
                    {
                        case TipoAuditoria.Registrar:
                            listaBeanAtributo.Add(new BeanAtributo("nombre_canal_grupo", _canal_grupo.nombre));
                            listaBeanAtributo.Add(new BeanAtributo("estado", _canal_grupo.estado_registro.ToString()));
                            listaBeanAtributo.Add(new BeanAtributo("fecha_registra", Fechas.convertDateTimeToString(_canal_grupo.fecha_registra)));
                            listaBeanAtributo.Add(new BeanAtributo("usuario_registra", _canal_grupo.usuario_registra));
                            break;
                        case TipoAuditoria.Modificar:
                            listaBeanAtributo.Add(new BeanAtributo("nombre_canal_grupo", _canal_grupo.nombre));
                            listaBeanAtributo.Add(new BeanAtributo("estado", _canal_grupo.estado_registro.ToString()));
                            listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(_canal_grupo.fecha_modifica)));
                            listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", _canal_grupo.usuario_modifica));
                            break;
                        case TipoAuditoria.Eliminar:
                            listaBeanAtributo.Add(new BeanAtributo("estado", _canal_grupo.estado_registro.ToString()));
                            listaBeanAtributo.Add(new BeanAtributo("fecha_modifica", Fechas.convertDateTimeToString(_canal_grupo.fecha_modifica)));
                            listaBeanAtributo.Add(new BeanAtributo("usuario_modifica", _canal_grupo.usuario_modifica));
                            break;
                    }

                    listaBeanEntidad.Add(new BeanEntidad(codigo, "canal_grupo", listaBeanAtributo));
                }
                else
                {
                    listaBeanAtributo.Add(new BeanAtributo("ERROR", "ERROR AL " + titulo));
                    listaBeanEntidad.Add(new BeanEntidad(codigo, "canal_grupo", listaBeanAtributo));
                }

                _eventoUsuarioService.GenerarEvento(beanSesionUsuario, (int)auditoria, estadoEvento, listaBeanEntidad);

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;
        }

    }
}
