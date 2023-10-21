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
    public class TipoEspacioService : ITipoEspacioService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<tipo_espacio> _repository;

        public TipoEspacioService()
        {
            this._repository = new DataRepository<tipo_espacio>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(tipo_espacio instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_tipo_espacio.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(tipo_espacio instance)
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

        public IResult Delete(tipo_espacio instance)
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

        public tipo_espacio GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_tipo_espacio == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_tipo_espacio == id);
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
                {"codigo_tipo_espacio", node.codigo_tipo_espacio.ToString()},
                {"nombre_tipo_espacio", node.nombre_tipo_espacio},
                {"numero_columnas", node.numero_columnas.ToString()},
                {"numero_filas", node.numero_filas.ToString()},
                {"tipo_lote", node.tipo_lote},
                {"constante", node.constante},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<tipo_espacio> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
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
                        {"codigo_tipo_espacio", item.codigo_tipo_espacio.ToString()},
                        {"nombre_tipo_espacio", item.nombre_tipo_espacio},
                        {"numero_columnas", item.numero_columnas.ToString()},
                        {"numero_filas", item.numero_filas.ToString()},
                        {"tipo_lote", item.tipo_lote.ToString()},
                        {"constante", item.constante},
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

        public string GetComboJson(bool isReadAll = false)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistros(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_tipo_espacio.ToString()}, 
                        {"text", item.nombre_tipo_espacio}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetComboByTipoLoteJson(bool tipoLote)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = new List<tipo_espacio>().AsQueryable();
            allNodes = from p in dbContext.tipo_espacio
                       where p.estado_registro == true && p.tipo_lote == tipoLote
                        select p;

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_tipo_espacio.ToString()}, 
                        {"text", item.nombre_tipo_espacio}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }
    }
}