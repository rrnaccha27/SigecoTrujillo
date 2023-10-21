using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IPersonaService
    {
        string GetAllJson(bool isReadAll = false);
        IResult Create(persona instance);
        IResult Update(persona instance);
        persona GetSingle(int id);
        string GetSingleJSON(int id);
        string GetAllJsonByFiltro(string tipo, string valor);

        IQueryable<persona> GetRegistros(bool isReadAll = false);
        string GetAllVendedorJsonByFiltro(string tipo, string valor);
         
    }
}