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
    public class PrecioArticuloService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<precio_articulo> _repository;

        public PrecioArticuloService()
        {
            this._repository = new DataRepository<precio_articulo>(dbContext);
        }

        #region Metodos de Mantenimiento
        public IResult Create(precio_articulo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_precio.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        // Hace de Update y Delete (logico)
        public IResult Update(precio_articulo instance)
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

        private IQueryable<precio_articulo> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        private precio_articulo GetFirst(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_precio == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_precio == codigo);
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
                        {"codigo_precio", item.codigo_precio.ToString()},
                        {"codigo_articulo", item.codigo_articulo.ToString()},
                        {"codigo_empresa", item.empresa_sigeco.codigo_empresa.ToString()},
                        {"nombre_empresa", item.empresa_sigeco.nombre},
                        {"codigo_tipo_venta", item.tipo_venta.codigo_tipo_venta.ToString()},
                        {"nombre_tipo_venta", item.tipo_venta.nombre},
                        {"codigo_moneda", item.moneda.codigo_moneda.ToString()},
                        {"nombre_moneda", item.moneda.nombre},
                        {"precio", item.precio.ToString()},

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

        public string GetAllbyArticuloJson(int codigoArticulo)
        {
            List<JObject> jObjects = new List<JObject>();
            //var allNodes = this.GetAllbyArticulo(codigoArticulo);

            var allNodes = new List<precio_articulo>().AsQueryable();

            allNodes = from e in dbContext.precio_articulo
                       where e.codigo_articulo == codigoArticulo && e.estado_registro == true
                    select e;

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_precio", item.codigo_precio.ToString()},
                        {"codigo_articulo", item.codigo_articulo.ToString()},
                        {"codigo_empresa", item.empresa_sigeco.codigo_empresa.ToString()},
                        {"nombre_empresa", item.empresa_sigeco.nombre},
                        {"codigo_tipo_venta", item.tipo_venta.codigo_tipo_venta.ToString()},
                        {"nombre_tipo_venta", item.tipo_venta.nombre},
                        {"codigo_moneda", item.moneda.codigo_moneda.ToString()},
                        {"nombre_moneda", item.moneda.nombre},
                        {"precio", item.precio.ToString()},

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

        public string GetSingleJson(int codigo)
        {
            if (codigo.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("ID  NULO");
            }

            var node = this.GetFirst(codigo);

            var jo = new JObject
            {
                {"codigo_precio", node.codigo_precio.ToString()},
                {"codigo_articulo", node.codigo_articulo.ToString()},
                {"codigo_empresa", node.empresa_sigeco.codigo_empresa.ToString()},
                {"nombre_empresa", node.empresa_sigeco.nombre},
                {"codigo_tipo_venta", node.tipo_venta.codigo_tipo_venta.ToString()},
                {"nombre_tipo_venta", node.tipo_venta.nombre},
                {"codigo_moneda", node.moneda.codigo_moneda.ToString()},
                {"nombre_moneda", node.moneda.nombre},
                {"precio", node.precio.ToString()},

                {"estado", node.estado_registro.ToString()},
                {"fecha_registro", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"usuario_registro", node.usuario_registra},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public precio_articulo GetSingle(int codigo)
        {
            return GetFirst(codigo);
        }

        #endregion

    }
}