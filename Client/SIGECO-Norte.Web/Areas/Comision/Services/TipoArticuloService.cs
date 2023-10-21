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
    public class TipoArticuloService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<tipo_articulo> _repository;

        public TipoArticuloService()
        {
            this._repository = new DataRepository<tipo_articulo>(dbContext);
        }

        #region Metodos de Mantenimiento
        public IResult Create(tipo_articulo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_tipo_articulo.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Hace de Update y Delete (logico)
        public IResult Update(tipo_articulo instance)
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

        private IQueryable<tipo_articulo> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        private tipo_articulo GetFirst(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_tipo_articulo == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_tipo_articulo == codigo);
            }
            return null;
        }
 

        public string GetAllJson(bool isReadAll = false)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetAll(isReadAll);

            if (allNodes.Any())
            {
                foreach (var node in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_tipo_venta", node.codigo_tipo_articulo.ToString()},
                        {"nombre", node.nombre},
                        
                        {"estado_registro", node.estado_registro.ToString()},
                        {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                        {"usuario_registra", node.usuario_registra},
                        {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                        {"usuario_modifica", node.usuario_modifica},
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
                {"codigo_tipo_venta", node.codigo_tipo_articulo.ToString()},
                {"nombre", node.nombre},
                
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"usuario_registra", node.usuario_registra},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public tipo_articulo GetSingle(int codigo)
        {
            return GetFirst(codigo);
        }

        public string GetAllComboJson()
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetAll();

            if (allNodes.Any())
            {
                foreach (var node in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"id", node.codigo_tipo_articulo.ToString()},
                        {"text", node.nombre},
                    };
                    jObjects.Add(root);
                }
            }

            return JsonConvert.SerializeObject(jObjects);
        }

        #endregion

    }
}