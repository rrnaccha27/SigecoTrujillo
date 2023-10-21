using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Threading;
using SIGEES.Web.Utils;
using System.Data.Entity;

namespace SIGEES.Web.Services
{
    public class GrupoLapidaService : IGrupoLapidaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<grupo_lapida> _repository;

        public GrupoLapidaService()
        {
            this._repository = new DataRepository<grupo_lapida>(dbContext);
        }

        public IResult Create(grupo_lapida instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this.dbContext.Entry(instance).State = EntityState.Added;

                foreach (var detalle in instance.detalle_grupo_lapida)
                {
                    detalle.codigo_grupo_lapida = instance.codigo_grupo_lapida;
                    this.dbContext.Entry(detalle).State = EntityState.Added;

                    var cabeceraLapidaSelect = from c in dbContext.cabecera_lapida
                                         where c.codigo_cabecera_lapida == detalle.codigo_cabecera_lapida 
                                         && c.estado_registro == true
                                         select c;

                    if (cabeceraLapidaSelect.Any())
                    {
                        cabecera_lapida cabecera_lapida = new cabecera_lapida();
                        cabecera_lapida = cabeceraLapidaSelect.FirstOrDefault();

                        cabecera_lapida.codigo_estado_lapida = Globales.estadoLapidaEnConfeccion;
                        cabecera_lapida.fecha_inicio_confeccion = DateTime.Now;
                        cabecera_lapida.fecha_modifica = DateTime.Now;
                        this.dbContext.Entry(cabecera_lapida).State = EntityState.Modified;
                    }

                }

                this.dbContext.SaveChanges();

                result.IdRegistro = instance.codigo_grupo_lapida.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult UpdateMultiple(grupo_lapida instance, bool confeccionado, bool colocado, DateTime fechaModifica)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this.dbContext.Entry(instance).State = EntityState.Modified;

                var cabeceraLapidaSelect = from c in dbContext.cabecera_lapida join d in dbContext.detalle_grupo_lapida 
                                           on c.codigo_cabecera_lapida equals d.codigo_cabecera_lapida
                                           where d.codigo_grupo_lapida == instance.codigo_grupo_lapida 
                                           && c.estado_registro == true && d.estado_registro == true
                                           select c;

                DateTime fechaModificaSistema = DateTime.Now;

                foreach (cabecera_lapida cabecera in cabeceraLapidaSelect)
                {
                    if (confeccionado)
                    {
                        cabecera.codigo_estado_lapida = Globales.estadoLapidaConfeccionada;
                        cabecera.fecha_fin_confeccion = fechaModifica;
                        cabecera.fecha_modifica = fechaModificaSistema;
                    }
                    else if (colocado)
                    {
                        cabecera.codigo_estado_lapida = Globales.estadoLapidaColocada;
                        cabecera.fecha_colocacion = fechaModifica;
                        cabecera.fecha_modifica = fechaModificaSistema;

                        var espacioSelect = from e in dbContext.espacio
                                            where e.codigo_espacio == cabecera.codigo_espacio
                                            select e;

                        espacio espacio = espacioSelect.FirstOrDefault();
                        espacio.usuario_modifica = instance.usuario_modifica;
                        espacio.fecha_modifica = fechaModificaSistema;
                        espacio.lapida = true;
                        this.dbContext.Entry(espacio).State = EntityState.Modified;

                    }
                    this.dbContext.Entry(cabecera).State = EntityState.Modified;
                }

                this.dbContext.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult DeleteMultiple(grupo_lapida instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this.dbContext.Entry(instance).State = EntityState.Modified;

                var cabeceraLapidaSelect = from c in dbContext.cabecera_lapida
                                           join d in dbContext.detalle_grupo_lapida
                                               on c.codigo_cabecera_lapida equals d.codigo_cabecera_lapida
                                           where d.codigo_grupo_lapida == instance.codigo_grupo_lapida
                                           && c.estado_registro == true && d.estado_registro == true
                                           select c;

                var detalleGrupoSelect = from d in dbContext.detalle_grupo_lapida
                                         where d.codigo_grupo_lapida == instance.codigo_grupo_lapida
                                         && d.estado_registro == true
                                         select d;

                DateTime fechaModificaSistema = DateTime.Now;

                foreach (cabecera_lapida cabecera in cabeceraLapidaSelect)
                {
                    cabecera.codigo_estado_lapida = Globales.estadoLapidaPorConfeccionar;
                    cabecera.fecha_modifica = fechaModificaSistema;
                    cabecera.fecha_inicio_confeccion = null;
                    cabecera.fecha_fin_confeccion = null;
                    cabecera.fecha_colocacion = null;
                    cabecera.usuario_modifica = instance.usuario_modifica;
                    this.dbContext.Entry(cabecera).State = EntityState.Modified;

                    var espacioSelect = from e in dbContext.espacio
                                        where e.codigo_espacio == cabecera.codigo_espacio
                                        select e;

                    espacio espacio = espacioSelect.FirstOrDefault();
                    espacio.lapida = false;
                    espacio.usuario_modifica = instance.usuario_modifica;
                    espacio.fecha_modifica = fechaModificaSistema;
                    this.dbContext.Entry(espacio).State = EntityState.Modified;
                }

                foreach (detalle_grupo_lapida detalleGrupo in detalleGrupoSelect)
                {
                    detalleGrupo.estado_registro = instance.estado_registro;
                    this.dbContext.Entry(detalleGrupo).State = EntityState.Modified;
                }

                this.dbContext.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Update(grupo_lapida instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Update(instance);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(grupo_lapida instance)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Delete(instance);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public grupo_lapida GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_grupo_lapida == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_grupo_lapida == id);
            }
            return null;
        }

        public string GetSingleJSON(int id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("ID  NULO");
            }
            var node = this.GetSingle(id);

            var jo = new JObject
            {
                {"codigo_grupo_lapida", node.codigo_grupo_lapida.ToString()},
                {"nombre_grupo_lapida", node.nombre_grupo_lapida},
                {"codigo_asignacion", node.codigo_asignacion},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<grupo_lapida> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        public List<grupo_lapida> Lista()
        {
            var query = this._repository.GetAll().ToList();

            return query;
        }

        public string GetAllJson(bool isReadAll = false)
        {

            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistros(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_grupo_lapida", item.codigo_grupo_lapida.ToString()},
                        {"nombre_grupo_lapida", item.nombre_grupo_lapida},
                        {"codigo_asignacion", item.codigo_asignacion},
                        {"estado_registro", item.estado_registro == true ? "ACTIVO":"ELIMINADO"},
                        {"fecha_registra", Fechas.convertDateTimeToString(item.fecha_registra)},
                        {"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                        {"usuario_registra", item.usuario_registra},
                        {"usuario_modifica", item.usuario_modifica},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }
        public string GetComboJson()
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistros(false);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_grupo_lapida.ToString()}, 
                        {"text", item.nombre_grupo_lapida}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

    }
}