using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ICampoSantoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(campo_santo instance);
        IResult Update(campo_santo instance);
        campo_santo GetSingle(int id);
        string GetSingleJSON(int id);
        IQueryable<campo_santo> GetRegistrosByCorporacion(int id);
        string GetComboByEmpresaJson(int codigoEmpresa);
        string GetComboByUsuarioJson(string codigoUsuario);
    }
}