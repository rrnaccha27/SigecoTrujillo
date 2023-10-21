using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGEES.Web.Core;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Areas.Comision.Entity;

namespace SIGEES.Web.Areas.Comision.Services
{
    public class EstadoPlanillaService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<estado_planilla> _repository;

        public EstadoPlanillaService()
        {
            this._repository = new DataRepository<estado_planilla>(dbContext);
        }



        #region Metodos de Listado

        private IQueryable<estado_planilla> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        private estado_planilla GetSingle(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_estado_planilla == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_estado_planilla == codigo);
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
                        {"codigo_estado_planilla", item.codigo_estado_planilla.ToString()},
                        {"nombre_estado_planilla", item.nombre},
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
            var node = this.GetSingle(codigo);

            var jo = new JObject
            {
                {"codigo_estado_planilla", node.codigo_estado_planilla.ToString()},
                {"nombre_estado_planilla", node.nombre},
                {"estado", node.estado_registro.ToString()},
                {"fecha_registro", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"usuario_registro", node.usuario_registra},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public string GetAllComboJson()
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetAll();

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"id", item.codigo_estado_planilla.ToString()},
                        {"text", item.nombre},
                    };
                    jObjects.Add(root);
                }
            }

            return JsonConvert.SerializeObject(jObjects);
        }

        #endregion
    }
}