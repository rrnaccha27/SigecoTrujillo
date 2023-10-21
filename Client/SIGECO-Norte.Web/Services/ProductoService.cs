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
    public class ProductoService : IProductoService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<producto> _repository;

        public ProductoService()
        {
            this._repository = new DataRepository<producto>(dbContext);
        }

        public IResult Create(producto instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {

                this.dbContext.Entry(instance).State = EntityState.Added;

                foreach (var detalle_producto in instance.detalle_producto)
                {
                    detalle_producto.codigo_producto = instance.codigo_producto;
                    this.dbContext.Entry(detalle_producto).State = EntityState.Added;
                }

                this.dbContext.SaveChanges();

                result.IdRegistro = instance.codigo_producto.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(producto instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {

                var listaDetalleProductoEliminar = from d in dbContext.detalle_producto
                                                  where d.codigo_producto == instance.codigo_producto
                                                  select d;

                this.dbContext.detalle_producto.RemoveRange(listaDetalleProductoEliminar);

                this.dbContext.Entry(instance).State = EntityState.Modified;

                foreach (var detalle_producto in instance.detalle_producto)
                {
                    detalle_producto.codigo_producto = instance.codigo_producto;
                    this.dbContext.Entry(detalle_producto).State = EntityState.Added;
                }

                this.dbContext.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(producto instance)
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

        public producto GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_producto == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_producto == id);
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
                {"codigo_producto", node.codigo_producto.ToString()},
                {"nombre_producto", node.nombre_producto},
                {"codigo_tipo_producto", node.codigo_tipo_producto.ToString()},
                {"nombre_tipo_producto", node.tipo_producto.nombre_tipo_producto},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<producto> GetRegistros(bool isReadAll = false)
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
                        {"codigo_producto", item.codigo_producto.ToString()},
                        {"nombre_producto", item.nombre_producto},
                        {"nombre_tipo_producto", item.tipo_producto.nombre_tipo_producto},
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
        public string GetComboByTipoProductoJson(int codigoTipoProducto)
        {
            List<JObject> jObjects = new List<JObject>();

            var listaProducto = new List<producto>().AsQueryable();

            listaProducto = from p in dbContext.producto
                            where p.estado_registro == true && p.codigo_tipo_producto == codigoTipoProducto
                            select p;

            var allNodes = this.GetRegistros(false);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_producto.ToString()}, 
                        {"text", item.nombre_producto}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

    }
}