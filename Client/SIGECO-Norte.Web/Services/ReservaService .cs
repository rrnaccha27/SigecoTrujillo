using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    
    public class ReservaService:IReservaService
    {
        
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<reserva> _repository;

        public ReservaService()
        {
            this._repository = new DataRepository<reserva>(dbContext);
        }



        public string GetAllJson(bool isReadAll = false)
        {
            throw new NotImplementedException();
        }


        

        public IResult Update(reserva instance)
        {
            throw new NotImplementedException();
        }

        public reserva GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<reserva> GetRegistro()
        {
            throw new NotImplementedException();
        }

        public string GetSingleJSON(int id)
        {
            throw new NotImplementedException();
        }


        IResult IReservaService.Create(reserva instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("Registro nulo.");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);
                result.IdRegistro = instance.codigo_reserva.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
    }
}