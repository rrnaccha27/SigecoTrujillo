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
    public class ParametroSistemaService : IParametroSistemaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<parametro_sistema> _repository;

        public ParametroSistemaService()
        {
            this._repository = new DataRepository<parametro_sistema>(dbContext);
        }

        public IResult Create(parametro_sistema instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_parametro_sistema.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(parametro_sistema instance)
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

        public IResult Delete(parametro_sistema instance)
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

        public parametro_sistema GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_parametro_sistema == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_parametro_sistema == id);
            }
            return null;
        }

        public parametro_sistema GetParametro(int id)
        {
            if (this._repository.IsExists(x => x.codigo_parametro_sistema == id))
            {
                parametro_sistema parametro_sistema = new parametro_sistema();
                parametro_sistema = this._repository.FirstOrDefault(x => x.codigo_parametro_sistema == id);

                if (parametro_sistema.tokenizar)
                {
                    parametro_sistema.valor = CifradoAES.DecryptStringAES(parametro_sistema.valor, Globales.llaveCifradoParametro);
                }

                return parametro_sistema;
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
                {"codigo_parametro_sistema", node.codigo_parametro_sistema.ToString()},
                {"nombre_parametro_sistema", node.nombre_parametro_sistema},
                {"valor", node.valor},
                {"tokenizar", node.tokenizar.ToString()},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<parametro_sistema> GetRegistros(bool isReadAll = false)
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
                        {"codigo_parametro_sistema", item.codigo_parametro_sistema.ToString()},
                        {"nombre_parametro_sistema", item.nombre_parametro_sistema},
                        {"valor", item.valor},
                        {"tokenizar", item.tokenizar.ToString()},
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

    }
}