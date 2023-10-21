using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.Entidades;
using SIGEES.BusinessLogic.SAPSigecoWcf;

namespace SIGEES.BusinessLogic
{
    public class PersonalBL : GenericBL<PersonalBL>
    {

        private readonly SIGEES.DataAcces.PersonalDA oPersonalDA = new DataAcces.PersonalDA();

        public bool AsignarCanalGrupoMasivo(int codigo_canal_grupo, int codigo_personal, bool es_canal_grupo, string usuario_modifica, string xmlPersonal)
        {
            return oPersonalDA.AsignarCanalGrupoMasivo(codigo_canal_grupo, codigo_personal, es_canal_grupo, usuario_modifica, xmlPersonal);
        }

        public MensajeDTO Registrar(personal_dto personal, Boolean usarWCF = false)
        {
            int codigo_personal = 0;
            int codigo_registro = 0;
            string usuario = string.Empty;
            string codigo_equivalencia = string.Empty;
            bool es_supervisor = false;
            log_wcf_sap_dto log = new log_wcf_sap_dto();
            MensajeDTO respuesta = new MensajeDTO();
            List<int> grupos = new List<int>();
            List<string> grupos_equivalencia = new List<string>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_personal = oPersonalDA.Registrar(personal);
                    usuario = personal.usuario_registra;
                    PersonalCanalGrupoBL oCanalgrupoBL = new PersonalCanalGrupoBL();

                    foreach (var item in personal.lista_canal_grupo)
                    {
                        item.codigo_personal = codigo_personal;
                        item.usuario_registra = usuario;
                        codigo_registro = oCanalgrupoBL.Registrar(item);
                        grupos.Add(item.codigo_canal_grupo);
                        es_supervisor = (item.es_supervisor_canal || item.es_supervisor_grupo ? true : false);
                    }

                    oPersonalDA.ActivarValidado(codigo_personal);

                    codigo_equivalencia = oPersonalDA.ObtenerEquivalencia(codigo_personal);

                    respuesta.idRegistro = codigo_personal;
                    respuesta.idOperacion = 1;
                    respuesta.mensaje = codigo_equivalencia;

                    if (usarWCF) {
                        log.codigo_sigeco = codigo_personal;
                        log.codigo_equivalencia = codigo_equivalencia;
                        log.objeto = log_wcf_objeto.Personal.ToString();
                        log.usuario_registro = usuario;
                        log.tipo_operacion = log_wcf_operacion.Registrar.ToString();

                        try
                        {
                            canal_grupo_detalle_dto canal_grupo;
                            foreach (int codigo_cg in grupos) {
                                canal_grupo = new SIGEES.DataAcces.CanalGrupoDA().Detalle(codigo_cg);
                                grupos_equivalencia.Add(canal_grupo.codigo_equivalencia);
                            }
                            
                            SAPSigecoWcf.SAPSIGECO_MANT www = new SAPSIGECO_MANT();
                            EVendedor vendedor = new EVendedor();
                            SAPSIGECO_RES respuestaWcf;

                            vendedor.DNI = personal.nro_documento;
                            vendedor.Codigo = codigo_equivalencia;
                            vendedor.Cuenta = personal.nro_cuenta;
                            vendedor.Nombre = personal.nombre + " " + personal.apellido_paterno + " " + personal.apellido_materno;
                            vendedor.Grupos = grupos_equivalencia.ToArray();
                            vendedor.Estado = true;

                            respuestaWcf = www.RegistroVendSup(vendedor, es_supervisor, true);

                            if (respuestaWcf.ErrCode != 0)
                            {
                                www.Dispose();
                                throw new Exception(respuestaWcf.ErrMsg);
                            }

                            www.Dispose();
                            log.mensaje_excepcion = log_wcf_resultado.Success.ToString();
                        }
                        catch (Exception ex)
                        {
                            respuesta.mensaje_wcf = ex.Message;
                            log.mensaje_excepcion = ex.Message;
                            respuesta.idOperacion = -2;
                        }
                        new SIGEES.DataAcces.LogWCFSAPDA().Insertar(log);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    respuesta.mensaje = ex.Message;
                    respuesta.idOperacion = -1;
                }
            }

            return respuesta;
        }

        public MensajeDTO Actualizar(personal_dto personal, string canalesEliminar, Boolean usarWCF = false)
        {
            int codigo_personal = 0;
            int codigo_registro = 0;
            string usuario = string.Empty;
            MensajeDTO respuesta = new MensajeDTO();
            log_wcf_sap_dto log = new log_wcf_sap_dto();
            List<int> grupos = new List<int>();
            List<string> grupos_equivalencia = new List<string>();
            Boolean es_supervisor = false;
            string codigo_equivalencia = string.Empty;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_personal = personal.codigo_personal;
                    usuario = personal.usuario_modifica;
                    oPersonalDA.Actualizar(personal);
                    PersonalCanalGrupoBL oCanalgrupoBL = new PersonalCanalGrupoBL();

                    foreach (var item in personal.lista_canal_grupo)
                    {
                        item.codigo_personal = codigo_personal;
                        if (item.codigo_registro < 0)
                        {
                            item.usuario_registra = usuario;
                            codigo_registro = oCanalgrupoBL.Registrar(item);
                        }
                        else if (item.confirmado){
                            item.usuario_modifica = usuario;
                            oCanalgrupoBL.Actualizar(item);
                        }
                        if (Convert.ToBoolean(item.estado_registro))
                        {
                            grupos.Add(item.codigo_canal_grupo);
                            es_supervisor = (item.es_supervisor_canal || item.es_supervisor_grupo ? true : false);
                        }
                    }

                    oPersonalDA.ActivarValidado(codigo_personal);

                    respuesta.idRegistro = codigo_personal;
                    respuesta.idOperacion = 1;

                    if (usarWCF)
                    {
                        codigo_equivalencia = oPersonalDA.ObtenerEquivalencia(codigo_personal);

                        log.codigo_sigeco = codigo_personal;
                        log.codigo_equivalencia = codigo_equivalencia;
                        log.objeto = log_wcf_objeto.Personal.ToString();
                        log.usuario_registro = usuario;
                        log.tipo_operacion = log_wcf_operacion.Modificar.ToString();

                        try
                        {
                            canal_grupo_detalle_dto canal_grupo;
                            foreach (int codigo_cg in grupos)
                            {
                                canal_grupo = new SIGEES.DataAcces.CanalGrupoDA().Detalle(codigo_cg);
                                grupos_equivalencia.Add(canal_grupo.codigo_equivalencia);
                            }

                            SAPSigecoWcf.SAPSIGECO_MANT www = new SAPSIGECO_MANT();
                            EVendedor vendedor = new EVendedor();
                            SAPSIGECO_RES respuestaWcf;

                            vendedor.DNI = personal.nro_documento;
                            vendedor.Codigo = codigo_equivalencia;
                            vendedor.Cuenta = personal.nro_cuenta;
                            vendedor.Nombre = personal.nombre + " " + personal.apellido_paterno + " " + personal.apellido_materno;
                            vendedor.Grupos = grupos_equivalencia.ToArray();
                            vendedor.Estado = true;
                            vendedor.EstadoSpecified = true;

                            respuestaWcf = www.RegistroVendSup(vendedor, es_supervisor, true);

                            if (respuestaWcf.ErrCode != 0)
                            {
                                www.Dispose();
                                throw new Exception(respuestaWcf.ErrMsg);
                            }

                            www.Dispose();
                            log.mensaje_excepcion = log_wcf_resultado.Success.ToString();
                        }
                        catch (Exception ex)
                        {
                            respuesta.mensaje_wcf = ex.Message;
                            log.mensaje_excepcion = ex.Message;
                            respuesta.idOperacion = -2;
                        }
                        new SIGEES.DataAcces.LogWCFSAPDA().Insertar(log);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    respuesta.mensaje = ex.Message;
                    respuesta.idOperacion = -1;

                }
            }

            return respuesta;
        }

        public int Eliminar(Int32 codigo_personal)
        {
            return oPersonalDA.Eliminar(codigo_personal);
        }

        public MensajeDTO Desactivar(personal_dto personal, bool esSupervisor = false, string codigo_equivalencia = "", bool usarWCF = false)
        {
            MensajeDTO respuesta = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    oPersonalDA.Desactivar(personal);

                    respuesta.idOperacion = 1;
                    log_wcf_sap_dto log = new log_wcf_sap_dto();

                    if (usarWCF)
                    {
                        log.codigo_sigeco = personal.codigo_personal;
                        log.codigo_equivalencia = codigo_equivalencia;
                        log.objeto = log_wcf_objeto.Personal.ToString();
                        log.usuario_registro = personal.usuario_modifica;
                        log.tipo_operacion = log_wcf_operacion.Desactivar.ToString();

                        try
                        {
                            SAPSigecoWcf.SAPSIGECO_MANT www = new SAPSIGECO_MANT();
                            EVendedor vendedor = new EVendedor();
                            SAPSIGECO_RES respuestaWcf;

                            vendedor.Codigo = codigo_equivalencia;
                            vendedor.Estado = false;
                            vendedor.EstadoSpecified = true;

                            respuestaWcf = www.RegistroVendSup(vendedor, esSupervisor, true);

                            if (respuestaWcf.ErrCode != 0)
                            {
                                www.Dispose();
                                throw new Exception(respuestaWcf.ErrMsg);
                            }

                            www.Dispose();
                            log.mensaje_excepcion = log_wcf_resultado.Success.ToString();
                        }
                        catch (Exception ex)
                        {
                            respuesta.mensaje_wcf = ex.Message;
                            log.mensaje_excepcion = ex.Message;
                            respuesta.idOperacion = -2;
                        }
                        new SIGEES.DataAcces.LogWCFSAPDA().Insertar(log);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    respuesta.mensaje = ex.Message;
                    respuesta.idOperacion = -1;
                }
            }

            return respuesta;
        }

        public MensajeDTO Activar(personal_dto personal, bool esSupervisor = false, string codigo_equivalencia = "", bool usarWCF = false)
        {
            MensajeDTO respuesta = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    oPersonalDA.Activar(personal);

                    respuesta.idOperacion = 1;

                    log_wcf_sap_dto log = new log_wcf_sap_dto();

                    if (usarWCF)
                    {
                        log.codigo_sigeco = personal.codigo_personal;
                        log.codigo_equivalencia = codigo_equivalencia;
                        log.objeto = log_wcf_objeto.Personal.ToString();
                        log.usuario_registro = personal.usuario_modifica;
                        log.tipo_operacion = log_wcf_operacion.Activar.ToString();

                        try
                        {
                            SAPSigecoWcf.SAPSIGECO_MANT www = new SAPSIGECO_MANT();
                            EVendedor vendedor = new EVendedor();
                            SAPSIGECO_RES respuestaWcf;

                            vendedor.Codigo = codigo_equivalencia;
                            vendedor.Estado = true;
                            vendedor.EstadoSpecified = true;

                            respuestaWcf = www.RegistroVendSup(vendedor, esSupervisor, true);

                            if (respuestaWcf.ErrCode != 0)
                            {
                                www.Dispose();
                                throw new Exception(respuestaWcf.ErrMsg);
                            }

                            www.Dispose();
                            log.mensaje_excepcion = log_wcf_resultado.Success.ToString();
                        }
                        catch (Exception ex)
                        {
                            respuesta.mensaje_wcf = ex.Message;
                            log.mensaje_excepcion = ex.Message;
                            respuesta.idOperacion = -2;
                        }
                        new SIGEES.DataAcces.LogWCFSAPDA().Insertar(log);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    respuesta.mensaje = ex.Message;
                    respuesta.idOperacion = -1;
                }
            }

            return respuesta;
        }

        public MensajeDTO Validar(List<personal_no_validado_dto> lst_personal, string usuario_modifica)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    oPersonalDA.Validar(lst_personal, usuario_modifica);
                    v_mensaje.idOperacion = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;
                }
            }

            return v_mensaje;
        }

        #region LISTADOS

        public personal_dto GetReg(Int32 codigo_personal)
        {
            return oPersonalDA.GetReg(codigo_personal);
        }

        public System.Data.DataTable ListarDT(int codigoCanal, int codigoGrupo, int estadoRegistro)
        {
            return oPersonalDA.ListarDT(codigoCanal, codigoGrupo, estadoRegistro);
        }
        public List<personal_listado_dto> Listar(int codigoCanal, int codigoGrupo, int estadoPersonal, int sede, string nombre = "")
        {
            return oPersonalDA.Listar(codigoCanal, codigoGrupo, estadoPersonal, sede, nombre);
        }

        public List<personal_x_canal_grupo_listado_dto> ListarPorCanalGrupo(int codigo_canal_grupo)
        {
            return oPersonalDA.ListarPorCanalGrupo(codigo_canal_grupo);
        }

        public List<personal_planilla_listado_dto> ListarByPlanilla(personal_planilla_listado_dto v_entidad)
        {
            return oPersonalDA.ListarByPlanilla(v_entidad);
        }

        public List<personal_planilla_listado_dto> ListarByPlanillaEstado(int p_codigo_planilla, int p_codigo_estado_cuota)
        {
            return oPersonalDA.ListarByPlanillaEstado(p_codigo_planilla, p_codigo_estado_cuota);
        }

        public personal_dto CrearNuevo(int codigo_personal, int codigo_canal_grupo, string usuario)
        {
            return oPersonalDA.CrearNuevo(codigo_personal, codigo_canal_grupo, usuario);
        }

        public List<personal_x_nombre_para_canal_grupo_dto> ListarPorNombresCanalGrupo(string nombres)
        {
            return oPersonalDA.ListarPorNombresCanalGrupo(nombres);
        }
        public bool ExisteDocumento(personal_dto personal, bool tipo)
        {
            return oPersonalDA.ExisteDocumento(personal, tipo);
        }

        public bool ExisteSupervisor(int codigo_personal, int codigo_canal_grupo, ref string mensaje)
        {
            return oPersonalDA.ExisteSupervisor(codigo_personal, codigo_canal_grupo, ref mensaje);
        }

        public List<personal_comision_manual_listado_dto> ListarParaComisionManual(string nombre)
        {
            return oPersonalDA.ListarParaComisionManual(nombre);
        }

        public List<reclamo_personal_listado_dto> ListarParaReclamos(string codigo_usuario)
        {
            return oPersonalDA.ListarParaReclamos(codigo_usuario);
        }

        public List<personal_historico_validacion_dto> ListarHistoricoValidacion(int codigo_personal)
        {
            return oPersonalDA.ListarHistoricoValidacion(codigo_personal);
        }

        public List<personal_no_validado_dto> ListarNoValidados()
        {
            return oPersonalDA.ListarNoValidados();
        }

        public List<personal_historico_bloqueo_dto> GetHistorialBloqueoJson(int codigo_personal)
        {
            return oPersonalDA.GetHistorialBloqueoJson(codigo_personal);
        }

        public int GetCantidadBloqueo(int codigo_personal)
        {
            return oPersonalDA.GetCantidadBloqueo(codigo_personal);
        }

        #endregion
    }
}
