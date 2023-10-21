using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    
    public interface IVentaService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(venta instance);
        IResult Update(venta instance);
        venta GetSingle(int id);        
        IQueryable<venta> GetRegistro();      

        string GetSingleJSON(int id);
        
    }
}