using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGEES.Web.Core;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Areas.Comision.Entity;

namespace SIGEES.Web.Areas.Comision.Services
{
    public class EmpresaSIGECOService
    {
        private SIGECOEntities dbContext = new SIGECOEntities();
        private readonly IRepository<empresa_sigeco> _repository;

        public EmpresaSIGECOService()
        {
            this._repository = new DataRepository<empresa_sigeco>(dbContext);
        }

        #region Metodos de Mantenimiento
        public IResult Create(empresa_sigeco instance)
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

        // Hace de Update y Delete (logico)
        public IResult Update(empresa_sigeco instance)
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
        #endregion

        #region Metodos de Listado

        private IQueryable<empresa_sigeco> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        private empresa_sigeco GetFirst(int codigo)
        {
            if (this._repository.IsExists(x => x.codigo_empresa == codigo))
            {
                return this._repository.FirstOrDefault(x => x.codigo_empresa == codigo);
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
                        {"codigo_empresa", item.codigo_empresa.ToString()},
                        {"nombre", item.nombre},
                        {"nombre_largo", item.nombre_largo},
                        {"nro_cuenta", item.nro_cuenta},
                        {"ruc", item.ruc},
						{"direccion_fiscal", item.direccion_fiscal},
                        {"estado", item.estado_registro?"1":"0"},
                        {"nombre_estado", item.estado_registro?"Activo":"Inactivo"},
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

            var node = this.GetFirst(codigo);

            var jo = new JObject
            {
                {"codigo_empresa", node.codigo_empresa.ToString()},
                {"nombre", node.nombre},
                {"ruc", node.ruc},
				{"direccion_fiscal", node.direccion_fiscal},
                {"estado", node.estado_registro.ToString()},
                {"fecha_registro", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"usuario_registro", node.usuario_registra},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public empresa_sigeco GetSingle(int codigo)
        {
            return GetFirst(codigo);
        }

      
        public string GetAllComboJson(bool isReadAll = false)
        {
            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetAll(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"id", item.codigo_empresa.ToString()},
                        {"text", item.nombre},
                    };
                    jObjects.Add(root);
                }
            }

            return JsonConvert.SerializeObject(jObjects);
        }
        public List<empresa_sigeco> GetAllByContrato(string p_numero_contrato)
        {


            var allNodes = (from pc in dbContext.cronograma_pago_comision
                            join e in dbContext.empresa_sigeco on pc.codigo_empresa equals e.codigo_empresa
                            where pc.nro_contrato == p_numero_contrato
                            select e).Distinct();
            if (allNodes.Any())
            {
                return allNodes.ToList();
            }
            else
            {
                return null;
            }
            
            
        }
        public string GetAllComboByContratoJson(string p_numero_contrato) {                                  

            List<JObject> jObjects = new List<JObject>();
            var allNodes = (from pc in dbContext.cronograma_pago_comision
                            join e in dbContext.empresa_sigeco on pc.codigo_empresa equals e.codigo_empresa
                            where pc.nro_contrato == p_numero_contrato
                            select e).Distinct();
            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                      {"id", item.codigo_empresa.ToString()},
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