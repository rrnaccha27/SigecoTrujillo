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
    public class PersonaService : IPersonaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<persona> _repository;

        public PersonaService()
        {
            this._repository = new DataRepository<persona>(dbContext);
        }

        public IResult Create(persona instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_persona.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(persona instance)
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

        public IResult Delete(persona instance)
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

        public persona GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_persona == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_persona == id);
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
                {"codigo_persona", node.codigo_persona.ToString()},
                {"nombre_persona", node.nombre_persona},
                {"apellido_paterno", node.apellido_paterno},
                {"apellido_materno", node.apellido_materno},
                {"numero_documento", node.numero_documento},
                {"fecha_nacimiento", Fechas.convertDateToString(node.fecha_nacimiento)},
                {"codigo_sexo", node.codigo_sexo},
                {"nombre_sexo", node.sexo.nombre_sexo},
                {"nombre_corporacion", node.corporacion.nombre_corporacion},
                {"codigo_estado_civil", node.estado_civil.codigo_estado_civil.ToString()},
                {"nombre_estado_civil", node.estado_civil.nombre_estado_civil},
                {"codigo_tipo_documento", node.tipo_documento.codigo_tipo_documento.ToString()},
                {"nombre_tipo_documento", node.tipo_documento.nombre_tipo_documento},
                {"es_vendedor", node.es_vendedor.ToString()},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }
        
        public IQueryable<persona> GetRegistros(bool isReadAll = false)
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
                        {"codigo_persona", item.codigo_persona.ToString()},
                        {"nombre_persona", item.nombre_persona},
                        {"apellido_paterno", item.apellido_paterno},
                        {"apellido_materno", item.apellido_materno},
                        {"numero_documento", item.numero_documento},
                        {"nombre_corporacion", item.corporacion.nombre_corporacion},
                        {"es_vendedor", item.es_vendedor.ToString()},
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

        public string GetAllJsonByFiltro(string tipo, string valor)
        {

            List<JObject> jObjects = new List<JObject>();
            var lista = new List<persona>().AsQueryable();
            if (tipo.CompareTo("1") == 0)
            {
                lista = from p in dbContext.persona
                        where (p.numero_documento).Contains(valor) && p.estado_registro == true
                        select p;
            }
            else if (tipo.CompareTo("2") == 0)
            {
                lista = from p in dbContext.persona
                        where (p.nombre_persona + " " + p.apellido_paterno + " " + p.apellido_materno).Contains(valor) && p.estado_registro == true
                        select p;
            }

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_persona", item.codigo_persona.ToString()},
                        {"persona", item.nombre_persona + " " + item.apellido_paterno + " " + item.apellido_materno},
                        {"numero_documento", item.numero_documento}
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllVendedorJsonByFiltro(string tipo, string valor)
        {

            List<JObject> jObjects = new List<JObject>();
            var lista = new List<persona>().AsQueryable();
            if (tipo.CompareTo("1") == 0)
            {
                lista = from p in dbContext.persona
                        where (p.numero_documento).Contains(valor) && p.estado_registro == true && p.es_vendedor == true
                        select p;
            }
            else if (tipo.CompareTo("2") == 0)
            {
                lista = from p in dbContext.persona
                        where (p.nombre_persona + " " + p.apellido_paterno + " " + p.apellido_materno).Contains(valor) && p.estado_registro == true && p.es_vendedor == true
                        select p;
            }
            else {
                lista = from p in dbContext.persona
                        where p.estado_registro == true && p.es_vendedor == true
                        select p;
            }

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_persona", item.codigo_persona.ToString()},
                        {"persona", item.nombre_persona + " " + item.apellido_paterno + " " + item.apellido_materno},
                        {"numero_documento", item.numero_documento}
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }
    }
}