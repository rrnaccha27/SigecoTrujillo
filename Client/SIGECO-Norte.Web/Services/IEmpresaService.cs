using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEmpresaService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(empresa instance);
        IResult Update(empresa instance);
        empresa GetSingle(int id);
        string GetSingleJSON(int id);
        IQueryable<empresa> GetRegistrosByCorporacion(int id);
        string GetComboByCorporacionJson(int codigoCorporacion);
        string GetComboByUsuarioJson(string codigoUsuario);
    }
}