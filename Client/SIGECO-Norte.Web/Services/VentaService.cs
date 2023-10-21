using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    
    public class VentaService:IVentaService
    {
        
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<venta> _repository;

        public VentaService()
        {
            this._repository = new DataRepository<venta>(dbContext);
        }
        string IVentaService.GetAllJson(bool isReadAll = false)
        {
            throw new NotImplementedException();
        }

        IResult Create(venta instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("Registro nulo.");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);
                result.IdRegistro = instance.codigo_venta.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        IResult Update(venta instance)
        {
            throw new NotImplementedException();
        }

        venta GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        IQueryable<venta> GetRegistro()
        {
            throw new NotImplementedException();
        }

        string GetSingleJSON(int id)
        {
            throw new NotImplementedException();
        }


        IResult IVentaService.Create(venta instance)
        {
            throw new NotImplementedException();
        }

        IResult IVentaService.Update(venta instance)
        {
            throw new NotImplementedException();
        }

        venta IVentaService.GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        IQueryable<venta> IVentaService.GetRegistro()
        {
            throw new NotImplementedException();
        }

        string IVentaService.GetSingleJSON(int id)
        {
            throw new NotImplementedException();
        }
    }
}