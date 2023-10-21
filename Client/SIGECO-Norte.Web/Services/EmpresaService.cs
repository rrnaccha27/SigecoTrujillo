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
    public class EmpresaService : IEmpresaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<empresa> _repository;

        public EmpresaService()
        {
            this._repository = new DataRepository<empresa>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(empresa instance)
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

        public IResult Update(empresa instance)
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

        public IResult Delete(empresa instance)
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

        public empresa GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_empresa == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_empresa == id);
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
                {"codigo_empresa", node.codigo_empresa.ToString()},
                {"nombre_empresa", node.nombre_empresa},
                {"nombre_corporacion", node.corporacion.nombre_corporacion},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<empresa> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }
        public IQueryable<empresa> GetRegistrosByCorporacion(int id)
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
                        {"codigo_empresa", item.codigo_empresa.ToString()},
                        {"nombre_empresa", item.nombre_empresa},
                        {"nombre_corporacion", item.corporacion.nombre_corporacion},
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

        public string GetComboByCorporacionJson(int codigoCorporacion)
        {
            List<JObject> jObjects = new List<JObject>();
            try
            {
                var allNodes = this.GetRegistrosByCorporacion(codigoCorporacion);

                if (allNodes.Any())
                {
                    foreach (var item in allNodes)
                    {
                        JObject root = new JObject{
                        {"id", item.codigo_empresa.ToString()}, 
                        {"text", item.nombre_empresa}
                    };
                        jObjects.Add(root);
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
                var lista = new List<empresa>().AsQueryable();

                lista = from p in dbContext.permiso_empresa join e in dbContext.empresa 
                        on p.codigo_empresa equals e.codigo_empresa
                        where p.codigo_usuario == codigoUsuario && p.estado_registro == true && e.estado_registro == true
                        select e;

                if (lista.Any())
                {
                    foreach (var item in lista)
                    {
                        JObject root = new JObject{
                        {"id", item.codigo_empresa.ToString()}, 
                        {"text", item.nombre_empresa}
                    };
                        jObjects.Add(root);
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