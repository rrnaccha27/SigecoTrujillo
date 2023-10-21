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
    public class PermisoEmpresaService : IPermisoEmpresaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<permiso_empresa> _repository;

        public PermisoEmpresaService()
        {
            this._repository = new DataRepository<permiso_empresa>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(permiso_empresa instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_permiso_empresa.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(permiso_empresa instance)
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

        public IResult Delete(permiso_empresa instance)
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

        public string GetAllEmpresaByUsuarioJson(string codigoUsuario)
        {

            List<JObject> jObjects = new List<JObject>();

            var lista = new List<empresa>().AsQueryable();

            lista = from e in dbContext.empresa
                    where e.estado_registro == true
                    select e;

            if (!string.IsNullOrWhiteSpace(codigoUsuario))
            {
                var listaPermisoEmpresa = new List<permiso_empresa>().AsQueryable();

                listaPermisoEmpresa = from p in dbContext.permiso_empresa
                                      where p.estado_registro == true && p.codigo_usuario.CompareTo(codigoUsuario) == 0
                                      select p;

                if (lista.Any())
                {
                    foreach (var empresa in lista)
                    {
                        bool registrado = false;

                        foreach (var permisoEmpresa in listaPermisoEmpresa)
                        {
                            if (empresa.codigo_empresa.CompareTo(permisoEmpresa.codigo_empresa) == 0)
                            {
                                JObject obj = new JObject
                                {
                                    {"codigo_empresa", empresa.codigo_empresa.ToString()},
                                    {"nombre_empresa", empresa.nombre_empresa},
                                    {"nombre_corporacion", empresa.corporacion.nombre_corporacion},
                                    {"registrado", "true"}
                                };
                                registrado = true;
                                jObjects.Add(obj);
                                break;
                            }
                        }

                        if (!registrado)
                        {
                            JObject obj = new JObject
                                {
                                    {"codigo_empresa", empresa.codigo_empresa.ToString()},
                                    {"nombre_empresa", empresa.nombre_empresa},
                                    {"nombre_corporacion", empresa.corporacion.nombre_corporacion},
                                    {"registrado", "false"}
                                };
                            jObjects.Add(obj);
                        }
                    }

                }
            }
            else
            {
                if (lista.Any())
                {
                    foreach (var item in lista)
                    {
                        JObject root = new JObject
                    {
                        {"codigo_empresa", item.codigo_empresa.ToString()},
                        {"nombre_empresa", item.nombre_empresa},
                        {"nombre_corporacion", item.corporacion.nombre_corporacion},
                        {"registrado", "false"}
                    };
                        jObjects.Add(root);
                    }

                }
            }
            
            return JsonConvert.SerializeObject(jObjects);
        }
    }
}