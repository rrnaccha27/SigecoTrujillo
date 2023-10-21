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
    public class ArticuloService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<articulo> _repository;

        public ArticuloService()
        {
            this._repository = new DataRepository<articulo>(dbContext);
        }

        #region Metodos de Mantenimiento
        public IResult Create(articulo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_articulo.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Hace de Update y Delete (logico)
        public IResult Update(articulo instance)
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

        private IQueryable<articulo> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        private articulo GetFirst(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_articulo == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_articulo == codigo);
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
                        {"codigo_articulo", item.codigo_articulo.ToString()},
                        {"nombre", item.nombre},
                        {"abreviatura", item.abreviatura},
                        {"genera_comision", item.genera_comision.ToString()},
						{"genera_bono", item.genera_bono.ToString()},
                        {"estado", item.estado_registro.ToString()},
                        {"fecha_registro", Fechas.convertDateTimeToString(item.fecha_registra)},
                        {"usuario_registro", item.usuario_registra},
                        {"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                        {"usuario_modifica", item.usuario_modifica},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }
        public string GetAllArticulobyNombreJson(string texto)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = new List<articulo>().AsQueryable();
            allNodes = from e in dbContext.articulo
                       where e.estado_registro == true && (e.nombre.Contains(@texto)) //&& e.es_supervisor_canal == false && e.es_supervisor_grupo == false && e.codigo_canal_grupo == null && ((e.nombre.Contains(@texto)) )
                       select e;
            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_articulo", item.codigo_articulo.ToString()},
                        {"nombre", item.nombre},
                        {"abreviatura", item.abreviatura},
                        {"codigo_categoria", item.codigo_categoria},
                        {"fecha_registra", item.fecha_registra},
                        {"usuario_registra", item.usuario_registra},
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllArticulobyNombreYGeneraBonoJson(string texto)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = new List<articulo>().AsQueryable();
            allNodes = from e in dbContext.articulo
                       where e.estado_registro == true && (e.nombre.Contains(@texto)) && e.genera_bolsa_bono == true
                       select e;
            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_articulo", item.codigo_articulo.ToString()},
                        {"codigo_sku", item.codigo_sku.ToString()},
                        {"nombre", item.nombre},
                        {"abreviatura", item.abreviatura},
                        {"codigo_categoria", item.codigo_categoria},
                        {"fecha_registra", item.fecha_registra},
                        {"usuario_registra", item.usuario_registra},
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
                {"codigo_articulo", node.codigo_articulo.ToString()},
                {"nombre", node.nombre},
                {"abreviatura", node.abreviatura},
				{"genera_comision", node.genera_comision.ToString()},
				{"genera_bono", node.genera_bono.ToString()},
                {"estado", node.estado_registro.ToString()},
                {"fecha_registro", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"usuario_registro", node.usuario_registra},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public articulo GetSingle(int codigo)
        {
            return GetFirst(codigo);
        }

        public string GetAllComboJson()
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetAll();

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"id", item.codigo_articulo.ToString()},
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