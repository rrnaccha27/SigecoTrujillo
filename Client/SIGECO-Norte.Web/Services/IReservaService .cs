using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    
    public interface IReservaService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(reserva instance);
        IResult Update(reserva instance);
        reserva GetSingle(int id);
        IQueryable<reserva> GetRegistro();      

        string GetSingleJSON(int id);
        
    }
}