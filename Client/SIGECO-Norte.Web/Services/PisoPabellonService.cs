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
    public class PisoPabellonService : IPisoPabellonService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<piso_pabellon> _repository;

        public PisoPabellonService()
        {
            this._repository = new DataRepository<piso_pabellon>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(piso_pabellon instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_piso_pabellon.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(piso_pabellon instance)
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

        public IResult Delete(piso_pabellon instance)
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

        public piso_pabellon GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_piso_pabellon == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_piso_pabellon == id);
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
                {"codigo_piso_pabellon", node.codigo_piso_pabellon.ToString()},
                {"nombre_piso_pabellon", node.nombre_piso_pabellon},
               
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<piso_pabellon> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        public IQueryable<piso_pabellon> GetRegistrosMayorACantidad(bool isReadAll = false, int level = 0)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0 && x.codigo_piso_pabellon >= level);
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
                    //JObject root = new JObject
                    //{
                    //    {"codigo_estado_civil", item.codigo_estado_civil.ToString()},
                    //    {"nombre_estado_civil", item.nombre_estado_civil},
                    //    {"estado_registro", item.estado_registro.ToString()},
                    //    {"fecha_registra", Fechas.convertDateTimeToString(item.fecha_registra)},
                    //    {"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                    //    {"usuario_registra", item.usuario_registra},
                    //    {"usuario_modifica", item.usuario_modifica},
                    //};
                    //jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }



        public string GetComboMayorAJson(bool isReadAll = false, int level = 0)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistrosMayorACantidad(isReadAll, level);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_piso_pabellon.ToString()}, 
                        {"text", item.nombre_piso_pabellon}
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
                        {"id", item.codigo_piso_pabellon.ToString()}, 
                        {"text", item.nombre_piso_pabellon}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }


        public string GetComboMenorAJson(bool isReadAll = false, int level = 0)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistrosMenorACantidad(isReadAll, level);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_piso_pabellon.ToString()}, 
                        {"text", item.nombre_piso_pabellon}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }
        public IQueryable<piso_pabellon> GetRegistrosMenorACantidad(bool isReadAll = false, int level = 0)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0 && x.codigo_piso_pabellon <= level);
            }
            return query;
        }
        
    }
}