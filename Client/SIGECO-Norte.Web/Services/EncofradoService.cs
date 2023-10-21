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
    public class EncofradoService : IEncofradoService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<encofrado> _repository;

        public EncofradoService()
        {
            this._repository = new DataRepository<encofrado>(dbContext);
        }

        public IResult Create(encofrado instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_encofrado.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(encofrado instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {

                this._repository.Update(instance);

                result.IdRegistro = instance.codigo_encofrado.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(encofrado instance)
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

        public encofrado GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_encofrado == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_encofrado == id);
            }
            return null;
        }
        public encofrado GetByEspacio(string codigoEspacio)
        {
            var encofrado = from e in dbContext.encofrado
                                   where e.codigo_espacio == codigoEspacio && e.estado_registro == true
                                   select e;
            if (encofrado.Any())
            {
                return encofrado.FirstOrDefault();
            }
            else
            {
                return null;
            }
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
                {"codigo_agencia", node.codigo_encofrado.ToString()},
                {"codigo_espacio", node.codigo_espacio},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<encofrado> GetRegistros(bool isReadAll = false)
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
                        {"codigo_agencia", item.codigo_encofrado.ToString()},
                        {"codigo_espacio", item.codigo_espacio},
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

    }
}