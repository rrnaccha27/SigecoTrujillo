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
    public class TipoPagoService : ITipoPagoService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<tipo_pago> _repository;

        public TipoPagoService()
        {
            this._repository = new DataRepository<tipo_pago>(dbContext);
        }

        public IResult Create(tipo_pago instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_tipo_pago.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(tipo_pago instance)
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

        public IResult Delete(tipo_pago instance)
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

        public tipo_pago GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_tipo_pago == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_tipo_pago == id);
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
                {"codigo_tipo_pago", node.codigo_tipo_pago.ToString()},
                {"nombre", node.nombre},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<tipo_pago> GetRegistros(bool isReadAll = false)
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
                foreach (var node in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_tipo_pago", node.codigo_tipo_pago.ToString()},
                        {"nombre", node.nombre},
                        {"estado_registro", node.estado_registro.ToString()},
                        {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                        {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                        {"usuario_registra", node.usuario_registra},
                        {"usuario_modifica", node.usuario_modifica},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }
        public string GetComboJson()
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<tipo_pago>().AsQueryable();

            lista = from e in dbContext.tipo_pago
                    where e.estado_registro == true
                    select e;

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    JObject root = new JObject{
                        {"id", item.codigo_tipo_pago.ToString()}, 
                        {"text", item.nombre}
                    };
                    jObjects.Add(root);
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }

    }
}