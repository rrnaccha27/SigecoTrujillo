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
    public class TipoConversionEspacioService : ITipoConversionEspacioService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<tipo_conversion_espacio> _repository;

        public TipoConversionEspacioService()
        {
            this._repository = new DataRepository<tipo_conversion_espacio>(dbContext);
        }

        public IResult Create(tipo_conversion_espacio instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);
                result.IdRegistro = instance.codigo_tipo_conversion_espacio.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(tipo_conversion_espacio instance)
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

        public IResult Delete(tipo_conversion_espacio instance)
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

        public tipo_conversion_espacio GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_tipo_conversion_espacio == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_tipo_conversion_espacio == id);
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
                {"codigo_tipo_conversion_espacio", node.codigo_tipo_conversion_espacio.ToString()},
                {"nombre_tipo_conversion_espacio", node.nombre_tipo_conversion_espacio},
                {"cantidad_conversion", node.cantidad_conversion.ToString()},
                {"codigo_tipo_nivel", node.codigo_tipo_nivel},
                {"nombre_tipo_nivel", node.tipo_nivel.nombre_tipo_nivel},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<tipo_conversion_espacio> GetRegistros(bool isReadAll = false)
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
                        {"codigo_tipo_conversion_espacio", item.codigo_tipo_conversion_espacio.ToString()},
                        {"nombre_tipo_conversion_espacio", item.nombre_tipo_conversion_espacio},
                        {"cantidad_conversion", item.cantidad_conversion.ToString()},
                        {"codigo_tipo_nivel", item.codigo_tipo_nivel},
                        {"nombre_tipo_nivel", item.tipo_nivel.nombre_tipo_nivel},
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
                        {"id", item.codigo_tipo_conversion_espacio.ToString()}, 
                        {"text", item.nombre_tipo_conversion_espacio}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        /*
        public IQueryable<tipo_conversion_espacio> GetRegistrosByCantidadMaximaConversion(int id)
        {
            var query = this._repository.GetAll();
            return query.Where(x => x.estado_registro.CompareTo(true) == 0 && x.cantidad_conversion<= id);

        }*/
        public string GetComboConversionByEspacioJson(int pCantidadMaximaConversion)
        {
            List<JObject> jObjects = new List<JObject>();
            try
            {
                var lista = new List<tipo_conversion_espacio>().AsQueryable();

                lista = from c in dbContext.tipo_conversion_espacio
                        where c.estado_registro == true && c.cantidad_conversion <= pCantidadMaximaConversion
                        select c;

                if (lista.Any())
                {
                    foreach (var campo_santo in lista)
                    {
                        JObject obj = new JObject{
                        {"id", campo_santo.codigo_tipo_conversion_espacio.ToString()}, 
                        {"text", campo_santo.nombre_tipo_conversion_espacio}
                    };
                        jObjects.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return JsonConvert.SerializeObject(jObjects);

            
        }
    }
}