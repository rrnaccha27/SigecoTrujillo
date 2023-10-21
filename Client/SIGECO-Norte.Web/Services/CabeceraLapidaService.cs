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

namespace SIGEES.Web.Services
{
    public class CabeceraLapidaService : ICabeceraLapidaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<cabecera_lapida> _repository;

        public CabeceraLapidaService()
        {
            this._repository = new DataRepository<cabecera_lapida>(dbContext);
        }

        public IResult Create(cabecera_lapida instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);
                result.IdRegistro = instance.codigo_cabecera_lapida.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(cabecera_lapida instance)
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

        public IResult Delete(cabecera_lapida instance)
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

        public cabecera_lapida GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_cabecera_lapida == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_cabecera_lapida == id);
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
                {"codigo_cabecera_lapida", node.codigo_cabecera_lapida.ToString()},
                {"titulo", node.titulo},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<cabecera_lapida> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        public List<cabecera_lapida> Lista()
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
                        {"codigo_cabecera_lapida", item.codigo_cabecera_lapida.ToString()},
                        {"titulo", item.titulo},
                        {"estado_registro", item.estado_registro.ToString()},
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
                        {"id", item.codigo_cabecera_lapida.ToString()}, 
                        {"text", item.titulo}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllByEstadoLapidaJson(int codigoEstadoLapida)
        {

            List<JObject> jObjects = new List<JObject>();

            var lista = from c in dbContext.cabecera_lapida
                        where c.codigo_estado_lapida == codigoEstadoLapida && c.estado_registro == true
                        select c;

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_cabecera_lapida", item.codigo_cabecera_lapida.ToString()},
                        {"titulo", item.titulo},
                        {"nombre_estado_lapida", item.estado_lapida.nombre_estado_lapida},
                        {"estado_registro", item.estado_registro.ToString()},
                        {"fecha_registra", Fechas.convertDateTimeToString(item.fecha_registra)},
                        {"fecha_inicio_confeccion", Fechas.convertDateTimeToString(item.fecha_inicio_confeccion)},
                        {"fecha_fin_confeccion", Fechas.convertDateTimeToString(item.fecha_fin_confeccion)},
                        {"fecha_colocacion", Fechas.convertDateTimeToString(item.fecha_colocacion)},
                        {"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                        {"usuario_registra", item.usuario_registra},
                        {"usuario_modifica", item.usuario_modifica},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllByGrupoLapidaJson(int codigoGrupoLapida)
        {

            List<JObject> jObjects = new List<JObject>();

            var lista = from c in dbContext.cabecera_lapida join d in dbContext.detalle_grupo_lapida 
                        on c.codigo_cabecera_lapida equals d.codigo_cabecera_lapida
                        where c.estado_registro == true && d.estado_registro == true && d.codigo_grupo_lapida == codigoGrupoLapida
                        select c;

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_cabecera_lapida", item.codigo_cabecera_lapida.ToString()},
                        {"titulo", item.titulo},
                        {"nombre_estado_lapida", item.estado_lapida.nombre_estado_lapida},
                        {"estado_registro", item.estado_registro.ToString()},
                        {"fecha_registra", Fechas.convertDateTimeToString(item.fecha_registra)},
                        {"fecha_inicio_confeccion", Fechas.convertDateTimeToString(item.fecha_inicio_confeccion)},
                        {"fecha_fin_confeccion", Fechas.convertDateTimeToString(item.fecha_fin_confeccion)},
                        {"fecha_colocacion", Fechas.convertDateTimeToString(item.fecha_colocacion)},
                        {"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                        {"usuario_registra", item.usuario_registra},
                        {"usuario_modifica", item.usuario_modifica},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

    }
}