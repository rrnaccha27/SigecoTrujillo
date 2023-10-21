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
    public class PerfilUsuarioService : IPerfilUsuarioService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<perfil_usuario> _repository;

        public PerfilUsuarioService()
        {
            this._repository = new DataRepository<perfil_usuario>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(perfil_usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_perfil_usuario.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Create_Multiple(perfil_usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                
                this.dbContext.Entry(instance).State = EntityState.Added;

                foreach (var permisoMenu in instance.permiso_menu)
                {
                    permisoMenu.codigo_perfil_usuario = instance.codigo_perfil_usuario;
                    this.dbContext.Entry(permisoMenu).State = EntityState.Added;
                }

                foreach (var itemTipoAcceso in instance.item_tipo_acceso)
                {

                    itemTipoAcceso.codigo_perfil_usuario = instance.codigo_perfil_usuario;
                    this.dbContext.Entry(itemTipoAcceso).State = EntityState.Added;
                }

                this.dbContext.SaveChanges();

                result.IdRegistro = instance.codigo_perfil_usuario.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        

        public IResult Update(perfil_usuario instance)
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

        public IResult Delete_Multiple(perfil_usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                var permisoPorPerfil = from c in dbContext.permiso_menu
                          where c.codigo_perfil_usuario == instance.codigo_perfil_usuario
                          select c;

                var itemTipoAccesoPorPerfil = from it in dbContext.item_tipo_acceso
                                       where it.codigo_perfil_usuario == instance.codigo_perfil_usuario
                                       select it;

                this.dbContext.permiso_menu.RemoveRange(permisoPorPerfil);
                this.dbContext.item_tipo_acceso.RemoveRange(itemTipoAccesoPorPerfil);

                this.dbContext.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Update_Multiple(perfil_usuario instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                var permisoPorPerfil = from c in dbContext.permiso_menu
                                       where c.codigo_perfil_usuario == instance.codigo_perfil_usuario
                                       select c;

                var itemTipoAccesoPorPerfil = from it in dbContext.item_tipo_acceso
                                              where it.codigo_perfil_usuario == instance.codigo_perfil_usuario
                                              select it;

                this.dbContext.permiso_menu.RemoveRange(permisoPorPerfil);
                this.dbContext.item_tipo_acceso.RemoveRange(itemTipoAccesoPorPerfil);
                this.dbContext.Entry(instance).State = EntityState.Modified;

                foreach (var permisoMenu in instance.permiso_menu)
                {
                    permisoMenu.codigo_perfil_usuario = instance.codigo_perfil_usuario;
                    this.dbContext.Entry(permisoMenu).State = EntityState.Added;
                }

                foreach (var itemTipoAcceso in instance.item_tipo_acceso)
                {
                    itemTipoAcceso.codigo_perfil_usuario = instance.codigo_perfil_usuario;
                    this.dbContext.Entry(itemTipoAcceso).State = EntityState.Added;
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

        public IResult Delete(perfil_usuario instance)
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

        public perfil_usuario GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_perfil_usuario == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_perfil_usuario == id);
            }
            return null;
        }

        public string GetSingleJSON(int id)
        {
            //System.Diagnostics.Debug.WriteLine("id: " + id);
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("ID  NULO");
            }
            var node = this.GetSingle(id);

            var jo = new JObject
            {
                {"codigo_perfil_usuario", node.codigo_perfil_usuario.ToString()},
                {"nombre_perfil_usuario", node.nombre_perfil_usuario},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<perfil_usuario> GetRegistros(bool isReadAll = false)
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
                        {"codigo_perfil_usuario", item.codigo_perfil_usuario.ToString()},
                        {"nombre_perfil_usuario", item.nombre_perfil_usuario},
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

        public string GetComboJson(bool isReadAll = false)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistros(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_perfil_usuario.ToString()}, 
                        {"text", item.nombre_perfil_usuario}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }
    }
}