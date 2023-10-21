using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IPermisoEmpresaService
    {
        IResult Create(permiso_empresa instance);
        IResult Update(permiso_empresa instance);
        string GetAllEmpresaByUsuarioJson(string codigoUsuario);
    }
}