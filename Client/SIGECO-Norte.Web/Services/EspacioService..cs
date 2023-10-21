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
    public class EspacioService : IEspacioService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<espacio> _repository;

        public EspacioService()
        {
            this._repository = new DataRepository<espacio>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(espacio instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_espacio.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(espacio instance)
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

        public IResult Delete(espacio instance)
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

        public espacio GetSingle(string id)
        {
            if (this._repository.IsExists(x => x.codigo_espacio == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_espacio == id);
            }
            return null;
        }
        
        public espacio GetRegistroById(string id)
        {
            if (this._repository.IsExists(x => x.codigo_espacio.ToString() == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_espacio.ToString() == id);
            }
            return null;
        }

        public IQueryable<espacio> GetRegistrosByIdPlataform(int id)
        {
            if (this._repository.IsExists(x => x.codigo_plataforma == id))
            {
                return this._repository.Find(x => x.codigo_plataforma == id);
            }
            return null;
        }

        public string GetAllJson(bool isReadAll = false)
        {
            throw new NotImplementedException();
        }


        public string GetAllEspacioByCampoSantoYCodigoJson(int codigoCampoSanto, string codigoEspacio)
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<espacio>().AsQueryable();

            lista = from e in dbContext.espacio
                    where e.codigo_campo_santo == codigoCampoSanto && e.codigo_espacio.Contains(codigoEspacio)
                    select e;

            if (lista.Any())
            {
                foreach (var espacio in lista)
                {

                    JObject obj = new JObject
                    {
                        {"codigo_espacio", espacio.codigo_espacio},
                        {"nombre_plataforma", espacio.plataforma.nombre_plataforma}
                    };
                    jObjects.Add(obj);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        public string GetAllEspacioByProductoJson(int codigoProducto)
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<espacio>().AsQueryable();

            lista = from d in dbContext.detalle_producto join e in dbContext.espacio 
                    on d.codigo_espacio equals e.codigo_espacio
                    where d.codigo_producto == codigoProducto
                    select e;

            if (lista.Any())
            {
                foreach (var espacio in lista)
                {
                    JObject obj = new JObject
                    {
                        {"codigo_espacio", espacio.codigo_espacio},
                        {"nombre_plataforma", espacio.plataforma.nombre_plataforma}
                    };
                    jObjects.Add(obj);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }


        public string GetAllEspacioByCampoSantoYNotInProductoJson(int codigoCampoSanto, int codigoProducto, string codigoEspacio)
        {
            List<JObject> jObjects = new List<JObject>();
            var lista = new List<espacio>().AsQueryable();

            var listaCodigoEspacioRegistrado = new List<String>().AsQueryable();

            listaCodigoEspacioRegistrado = from d in dbContext.detalle_producto
                                     join e in dbContext.espacio
                                         on d.codigo_espacio equals e.codigo_espacio
                                     where d.codigo_producto == codigoProducto
                                     select e.codigo_espacio;

            lista = from e in dbContext.espacio
                    where e.codigo_campo_santo == codigoCampoSanto && e.codigo_espacio.Contains(codigoEspacio) &&
                    !listaCodigoEspacioRegistrado.Contains(e.codigo_espacio)
                    select e;

            if (lista.Any())
            {
                foreach (var espacio in lista)
                {

                    JObject obj = new JObject
                    {
                        {"codigo_espacio", espacio.codigo_espacio},
                        {"nombre_plataforma", espacio.plataforma.nombre_plataforma}
                    };
                    jObjects.Add(obj);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

    }
}