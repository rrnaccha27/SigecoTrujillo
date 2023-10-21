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
using SIGEES.Web.Models.Bean;

namespace SIGEES.Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<usuario> _repository;

        public UsuarioService()
        {
            this._repository = new DataRepository<usuario>(dbContext);
        }

        public IResult Create(usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {

                this.dbContext.Entry(instance).State = EntityState.Added;

                foreach (var permisoEmpresa in instance.permiso_empresa)
                {
                    this.dbContext.Entry(permisoEmpresa).State = EntityState.Added;
                }

                foreach (var claveUsuario in instance.clave_usuario)
                {
                    this.dbContext.Entry(claveUsuario).State = EntityState.Added;
                }

                this.dbContext.SaveChanges();

                result.IdRegistro = instance.codigo_usuario.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {

                var listaPermisoEmpresaEliminar = from p in dbContext.permiso_empresa
                                              where p.codigo_usuario == instance.codigo_usuario
                                              select p;

                this.dbContext.permiso_empresa.RemoveRange(listaPermisoEmpresaEliminar);

                this.dbContext.Entry(instance).State = EntityState.Modified;

                foreach (var permisoEmpresa in instance.permiso_empresa)
                {
                    this.dbContext.Entry(permisoEmpresa).State = EntityState.Added;
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

        public IResult ResetPassword(clave_usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {

                var listaClaveUsuarioDesactivar = from c in dbContext.clave_usuario
                                                  where c.codigo_usuario == instance.codigo_usuario &&
                                                  c.estado_registro == true
                                                  select c;


                foreach (var claveUsuario in listaClaveUsuarioDesactivar)
                {
                    claveUsuario.estado_registro = false;
                    this.dbContext.Entry(claveUsuario).State = EntityState.Modified;
                }

                this.dbContext.Entry(instance).State = EntityState.Added;

                this.dbContext.SaveChanges();

                result.IdRegistro = instance.codigo.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(usuario instance)
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

        public usuario GetSingle(string id)
        {
            if (this._repository.IsExists(x => x.codigo_usuario == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_usuario == id);
            }
            return null;
        }
        public BeanUser Login(string codigoUsuario)
        {

            var lista = from u in dbContext.usuario
                    join c in dbContext.clave_usuario
                        on u.codigo_usuario equals c.codigo_usuario
                    where c.estado_registro == true && u.codigo_usuario == codigoUsuario
                    select new BeanUser { 
                        usuario = u.codigo_usuario,
                        clave = c.clave,
                        codigoPersona = u.codigo_persona,
                        codigoPerfilUsuario = u.codigo_perfil_usuario,
                        estadoRegistro = u.estado_registro
                    };

            if (lista.Any())
            {
                return lista.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public BeanUser LoginAD(string codigoUsuario)
        {
            var lista = from u in dbContext.usuario
                        where u.codigo_usuario == codigoUsuario
                        select new BeanUser
                        {
                            usuario = u.codigo_usuario,
                            codigoPerfilUsuario = u.codigo_perfil_usuario,
                            estadoRegistro = u.estado_registro
                        };

            if (lista.Any())
            {
                return lista.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public string GetSingleJSON(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("CODIGO REGISTRO NULO");
            }
            var node = this.GetSingle(id);

            var clave_usuaro = node.clave_usuario.ToList().Find(x=>x.estado_registro==true).clave;

            var str_clave = CifradoAES.DecryptStringAES(clave_usuaro, Globales.llaveCifradoClave);


            var jo = new JObject
            {
                {"codigo_usuario", node.codigo_usuario},
                {"codigo_perfil_usuario", node.codigo_perfil_usuario.ToString()},
                {"clave_usuario", str_clave},
                 
                {"nombre_perfil_usuario", node.perfil_usuario.nombre_perfil_usuario},
                {"persona", node.persona != null ? node.persona.nombre_persona + " " + node.persona.apellido_paterno + " " + node.persona.apellido_materno:""},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<usuario> GetRegistros()
        {
            var query = this._repository.GetAll();
            return query;
        }

        public string GetAllJson()
        {

            List<JObject> jObjects = new List<JObject>();
            //List<JObject> jObjects2 = new List<JObject>();

            var allNodes = this.GetRegistros();

            if (allNodes.Any())
            {
                allNodes = allNodes.OrderBy(x => x.estado_registro).ThenBy(n => n.codigo_usuario);

                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_usuario", item.codigo_usuario},
                        {"nombre_perfil_usuario", item.perfil_usuario.nombre_perfil_usuario},
                        {"persona", item.persona != null ? item.persona.nombre_persona + " " + item.persona.apellido_paterno + " " + item.persona.apellido_materno:""},
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


        public string GetAllJsonByFiltro(string valor)
        {

            List<JObject> jObjects = new List<JObject>();
            var lista = new List<usuario>().AsQueryable();

            lista = from u in dbContext.usuario
                    where (u.codigo_usuario).Contains(valor) && u.estado_registro == "A"
                    select u;

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_usuario", item.codigo_usuario},
                        {"persona", item.persona != null ? item.persona.nombre_persona + " " + item.persona.apellido_paterno + " " + item.persona.apellido_materno : ""}
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

    }
}