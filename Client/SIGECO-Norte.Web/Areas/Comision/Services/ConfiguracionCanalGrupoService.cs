using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGEES.Web.Core;
using SIGEES.Web.Areas.Comision.Entity;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SIGEES.Web.Areas.Comision.Services
{
    public class ConfiguracionCanalGrupoService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<configuracion_canal_grupo> _repository;

        public ConfiguracionCanalGrupoService()
        {
            this._repository = new DataRepository<configuracion_canal_grupo>(dbContext);
        }

        #region Metodos de Mantenimiento
        public IResult Create(configuracion_canal_grupo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_configuracion.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Hace de Update y Delete (logico)
        public IResult Update(configuracion_canal_grupo instance)
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
        #endregion

        #region Metodos de Listado

        private IQueryable<configuracion_canal_grupo> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            //if (!isReadAll)
            //{
            //    return query.Where(x => x.estado.CompareTo(true) == 0);
            //}
            return query;
        }

        private configuracion_canal_grupo GetFirst(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_configuracion == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_configuracion == codigo);
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
                        {"codigo_configuracion", item.codigo_configuracion.ToString()},
                        //{"estado", item.estado.ToString()},
                        //{"fecha_registro", Fechas.convertDateTimeToString(item.fecha_registro)},
                        //{"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                        //{"usuario_registro", item.usuario_registro},
                        //{"usuario_modifica", item.usuario_modifica},
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
                {"codigo_configuracion", node.codigo_configuracion.ToString()},
                //{"estado", node.estado.ToString()},
                //{"fecha_registro", Fechas.convertDateTimeToString(node.fecha_registro)},
                //{"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                //{"usuario_registro", node.usuario_registro},
                //{"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public configuracion_canal_grupo GetSingle(int codigo)
        {
            return GetFirst(codigo);
        }

        #endregion

    }
}