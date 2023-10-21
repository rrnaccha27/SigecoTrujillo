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
    public class PersonalService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<personal> _repository;

        public PersonalService()
        {
            this._repository = new DataRepository<personal>(dbContext);
        }

        #region Metodos de Listado

        //Para seleccionar Supervisor de Canal/Grupo
        public string GetAllDisponiblebyNombreJson(string texto)
        {
            List<JObject> jObjects = new List<JObject>();

            var allNodes = new List<personal>().AsQueryable();

            allNodes = from e in dbContext.personal
                       where ((e.estado_registro == true) && (e.nombre + " " + (e.apellido_paterno == null ? "" : e.apellido_paterno) + " " + (e.apellido_materno == null ? "" : e.apellido_materno)).Contains(@texto)) //&& e.es_supervisor_canal == false && e.es_supervisor_grupo == false && e.codigo_canal_grupo == null && ((e.nombre.Contains(@texto)) )
                       orderby e.nombre + " " + (e.apellido_paterno == null ? "" : e.apellido_paterno) + " " + (e.apellido_materno == null ? "" : e.apellido_materno)
                    select e;

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_personal", item.codigo_personal.ToString()},
                        {"nro_documento", (item.nro_documento != null ? item.nro_documento.ToString() : "")},
                        {"nombre", item.nombre},
                        {"apellido_paterno", (item.apellido_paterno != null ? item.apellido_paterno.ToString() : "")},
                        {"apellido_materno", (item.apellido_materno != null ? item.apellido_materno.ToString() : "")},
                        {"fecha_registra", item.fecha_registra},
                        {"usuario_registra", item.usuario_registra},
                        {"nombre_completo", item.nombre + (item.apellido_paterno != null ? " " + item.apellido_paterno.ToString() : "") + (item.apellido_materno != null ? " " + item.apellido_materno.ToString() : "") },
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        //Para seleccionar Personal para un Canal/Grupo
        public string GetAllDisponibleJson()
        {
            List<JObject> jObjects = new List<JObject>();

            var allNodes = new List<personal>().AsQueryable();

            allNodes = from e in dbContext.personal
                       where e.estado_registro == true //&& e.es_supervisor_canal == false && e.es_supervisor_grupo == false && e.codigo_canal_grupo == null
                       select e;

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_personal", item.codigo_personal.ToString()},
                        {"nombre", item.nombre + " " + (item.apellido_paterno != null ? item.apellido_paterno.ToString() : "") + " " + (item.apellido_materno != null ? item.apellido_materno.ToString() : "")},
                        {"fecha_registra", item.fecha_registra},
                        {"usuario_registra", item.usuario_registra},
                        {"es_supervisor", false},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        //Para mostrar los asignados a un Canal/Grupo
        public string GetAllbyCanalGrupoJson(int codigo_canal_grupo)
        {
            List<JObject> jObjects = new List<JObject>();

            var allNodes = new List<personal>().AsQueryable();

            allNodes = from e in dbContext.personal
                       where e.estado_registro == true //&& e.codigo_canal_grupo == codigo_canal_grupo
                       select e;

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_personal", item.codigo_personal.ToString()},
                        {"nombre", item.nombre + " " + (item.apellido_paterno != null ? item.apellido_paterno.ToString() : "") + " " + (item.apellido_materno != null ? item.apellido_materno.ToString() : "")},
                        {"fecha_registra", item.fecha_registra},
                        {"usuario_registra", item.usuario_registra},
                        //{"es_supervisor_canal", item.es_supervisor_canal},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }
        #endregion

    }
}