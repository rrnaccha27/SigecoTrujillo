using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public class EventoUsuarioService : IEventoUsuarioService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<evento_usuario> _repository;

        public EventoUsuarioService()
        {
            this._repository = new DataRepository<evento_usuario>(dbContext);
        }

        public IResult GenerarEvento(BeanSesionUsuario beanSesionUsuario, int codigoTipoEvento, bool estadoEvento, List<BeanEntidad> listaBeanEntidad)
        {
            if (beanSesionUsuario == null || listaBeanEntidad == null)
            {
                throw new InvalidOperationException("PARAMETROS NULOS");
            }

            IResult result = new Result(false);

            try
            {
                DateTime fecha = DateTime.Now;
                evento_usuario evento_usuario = new evento_usuario();
                evento_usuario.fecha_suceso = fecha;
                evento_usuario.estado_evento = estadoEvento;
                evento_usuario.codigo_usuario = beanSesionUsuario.codigoUsuario;
                evento_usuario.codigo_tipo_evento = codigoTipoEvento;
                evento_usuario.codigo_menu = beanSesionUsuario.codigoMenu;

                this.dbContext.Entry(evento_usuario).State = EntityState.Added;
                
                foreach (var beanEntidad in listaBeanEntidad){
                    detalle_entidad detalle_entidad = new detalle_entidad();
                    detalle_entidad.codigo_evento_usuario = evento_usuario.codigo_evento_usuario;
                    detalle_entidad.codigo_entidad = beanEntidad.codigoEntidad;
                    detalle_entidad.nombre_entidad = beanEntidad.nombreEntidad;
                    this.dbContext.Entry(detalle_entidad).State = EntityState.Added;

                    foreach (var beanAtributo in beanEntidad.listaBeanAtributo)
                    {
                        detalle_atributo detalle_atributo = new detalle_atributo();
                        detalle_atributo.codigo_detalle_entidad = detalle_entidad.codigo_detalle_entidad;
                        detalle_atributo.nombre_atributo = beanAtributo.nombreAtributo;
                        detalle_atributo.valor_atributo = beanAtributo.valorAtributo;

                        this.dbContext.Entry(detalle_atributo).State = EntityState.Added;
                    }
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


        public string GetAllEventoUsuarioJson(string codigoUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<evento_usuario>().AsQueryable();

            lista = from e in dbContext.evento_usuario
                    where e.fecha_suceso >= fechaInicio && e.fecha_suceso <= fechaFin && e.codigo_usuario == codigoUsuario
                    select e;

            if (lista.Any())
            {
                foreach (var item in lista)
                {

                    JObject root = new JObject
                    {
                        {"codigo_evento_usuario", item.codigo_evento_usuario.ToString()},
                        {"codigo_usuario", item.codigo_usuario},
                        {"nombre_menu", item.menu.nombre_menu},
                        {"nombre_tipo_evento", item.tipo_evento.nombre_tipo_evento},
                        {"fecha_suceso", Fechas.convertDateTimeToString(item.fecha_suceso)},
                        {"estado_evento", item.estado_evento.ToString()}
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllDetalleEntidadByEventoUsuarioJson(int codigoEventoUsuario)
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<detalle_entidad>().AsQueryable();

            lista = from d in dbContext.detalle_entidad
                    where d.codigo_evento_usuario == codigoEventoUsuario
                    select d;

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_detalle_entidad", item.codigo_detalle_entidad.ToString()},
                        {"codigo_evento_usuario", item.codigo_evento_usuario.ToString()},
                        {"nombre_entidad", item.nombre_entidad},
                        {"codigo_entidad", item.codigo_entidad}
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllDetalleAtributoByDetalleEntidadJson(int codigoDetalleEntidad)
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<detalle_atributo>().AsQueryable();

            lista = from d in dbContext.detalle_atributo
                    where d.codigo_detalle_entidad == codigoDetalleEntidad
                    select d;

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject
                    {
                        {"codigo_detalle_atributo", item.codigo_detalle_atributo.ToString()},
                        {"codigo_detalle_entidad", item.codigo_detalle_entidad.ToString()},
                        {"nombre_atributo", item.nombre_atributo},
                        {"valor_atributo", item.valor_atributo}
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }
    }
}