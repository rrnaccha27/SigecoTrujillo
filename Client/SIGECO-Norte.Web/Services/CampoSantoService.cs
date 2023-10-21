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
    public class CampoSantoService : ICampoSantoService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<campo_santo> _repository;

        public CampoSantoService()
        {
            this._repository = new DataRepository<campo_santo>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(campo_santo instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_empresa.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(campo_santo instance)
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

        public IResult Delete(campo_santo instance)
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

        public campo_santo GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_campo_santo == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_campo_santo == id);
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
                {"codigo_campo_santo", node.codigo_campo_santo.ToString()},
                {"nombre_campo_santo", node.nombre_campo_santo},
                {"nombre_corporacion", node.corporacion.nombre_corporacion},
                {"nombre_empresa", node.empresa.nombre_empresa},
                {"anio_minimo_conversion", node.anio_minimo_conversion.ToString()},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<campo_santo> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }
        public IQueryable<campo_santo> GetRegistrosByCorporacion(int id)
        {
            var query = this._repository.GetAll();
            return query.Where(x => x.estado_registro.CompareTo(true) == 0 && x.codigo_corporacion == id);

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
                        {"codigo_campo_santo", item.codigo_campo_santo.ToString()},
                        {"nombre_campo_santo", item.nombre_campo_santo},
                        {"nombre_corporacion", item.corporacion.nombre_corporacion},
                        {"nombre_empresa", item.empresa.nombre_empresa},
                        {"anio_minimo_conversion", item.anio_minimo_conversion.ToString()},
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


        public string GetComboByEmpresaJson(int codigoEmpresa)
        {
            List<JObject> jObjects = new List<JObject>();
            try
            {
                var lista = new List<campo_santo>().AsQueryable();

                lista = from c in dbContext.campo_santo
                        where c.estado_registro == true && c.codigo_empresa == codigoEmpresa
                        select c;

                if (lista.Any())
                {
                    foreach (var campo_santo in lista)
                    {
                        JObject obj = new JObject{
                        {"id", campo_santo.codigo_campo_santo.ToString()}, 
                        {"text", campo_santo.nombre_campo_santo}
                    };
                        jObjects.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetComboByUsuarioJson(string codigoUsuario)
        {
            List<JObject> jObjects = new List<JObject>();
            try
            {
                var lista = new List<campo_santo>().AsQueryable();

                lista = from p in dbContext.permiso_empresa
                        join e in dbContext.empresa
                        on p.codigo_empresa equals e.codigo_empresa
                        join c in dbContext.campo_santo
                        on e.codigo_empresa equals c.codigo_empresa
                        where p.codigo_usuario == codigoUsuario && p.estado_registro == true && e.estado_registro == true &&
                        c.estado_registro == true
                        select c;

                if (lista.Any())
                {
                    foreach (var campo_santo in lista)
                    {
                        JObject obj = new JObject{
                        {"id", campo_santo.codigo_campo_santo.ToString()}, 
                        {"text", campo_santo.nombre_campo_santo}
                    };
                        jObjects.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return JsonConvert.SerializeObject(jObjects);
        }
    }
}