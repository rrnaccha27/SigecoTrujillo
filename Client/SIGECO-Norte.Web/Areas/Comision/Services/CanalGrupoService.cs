using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGEES.Web.Core;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Areas.Comision.Entity;


namespace SIGEES.Web.Areas.Comision.Services
{
    public class CanalGrupoService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<canal_grupo> _repository;

        public CanalGrupoService()
        {
            this._repository = new DataRepository<canal_grupo>(dbContext);
        }

        #region Metodos de Mantenimiento
        public IResult Create(canal_grupo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_canal_grupo.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Hace de Update y Delete (logico)
        public IResult Update(canal_grupo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Update(instance);
                
                result.IdRegistro = instance.codigo_canal_grupo.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        #endregion

        #region Metodos de Listado

        private IQueryable<canal_grupo> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                //return query.Where(x => x.estado.CompareTo(true) == 0);
            }
            return query;
        }

        private canal_grupo GetFirst(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_canal_grupo == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_canal_grupo == codigo);
            }
            return null;
        }
 
        public string GetAllJson(bool isReadAll = false)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetAll(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_canal_grupo", item.codigo_canal_grupo.ToString()},

                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetSingleJson(int codigo)
        {
            if (codigo.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("ID  NULO");
            }

            var node = this.GetFirst(codigo);

            var jo = new JObject
            {
                {"codigo_canal_grupo", node.codigo_canal_grupo.ToString()},

            };

            return JsonConvert.SerializeObject(jo);
        }

        public canal_grupo GetSingle(int codigo)
        {
            return GetFirst(codigo);
        }

        public string GetCanalAllComboJson(bool incluirNinguno = false)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = new List<canal_grupo>().AsQueryable();

            allNodes = from e in dbContext.canal_grupo
                       where e.es_canal_grupo == true
                       select e;

            if (allNodes.Any())
            {
                if (incluirNinguno)
                {
                    JObject todos = new JObject
                    {
                        {"id", "99"},
                        {"text", "SIN CANAL"},
                    };
                    jObjects.Add(todos);
                }
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"id", item.codigo_canal_grupo.ToString()},
                        {"text", item.nombre},
                    };
                    jObjects.Add(root);
                }
            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetGrupoAllComboJson(int codigo_canal)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = new List<canal_grupo>().AsQueryable();
           
                allNodes = from e in dbContext.canal_grupo
                           where e.es_canal_grupo == false && e.codigo_padre==codigo_canal
                           select e;
            

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"id", item.codigo_canal_grupo.ToString()},
                        {"text", item.nombre},
                    };
                    jObjects.Add(root);
                }
            }

            return JsonConvert.SerializeObject(jObjects);
        }
        #endregion

    }
}