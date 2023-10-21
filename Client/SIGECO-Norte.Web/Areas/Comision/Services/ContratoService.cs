using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace SIGEES.Web.Areas.Comision.Services
{
    public class ContratoService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();
        private readonly IRepository<contrato> _repository;

        public ContratoService()
        {
            this._repository = new DataRepository<contrato>(dbContext);
        }
        
        #region Metodos de Listado

        private IQueryable<contrato> GetAll(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            //if (!isReadAll)
            //{
            //    return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            //}
            return query;
        }

        private contrato ValidaSiExisteContratoJson(string codigo)
        {
            if (this._repository.IsExists(x => x.nro_contrato == codigo))
            {
                return this._repository.FirstOrDefault(x => x.nro_contrato == codigo);
            }
            return null;
        }
        
        #endregion

    }
}