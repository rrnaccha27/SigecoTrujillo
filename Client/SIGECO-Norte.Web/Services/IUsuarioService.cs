using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IUsuarioService
    {
        string GetAllJson();

        IResult Create(usuario instance);
        IResult Update(usuario instance);
        IResult ResetPassword(clave_usuario instance);
        usuario GetSingle(string id);
        string GetSingleJSON(string id);
        IQueryable<usuario> GetRegistros();
        string GetAllJsonByFiltro(string valor);
        BeanUser Login(string codigoUsuario);
        BeanUser LoginAD(string codigoUsuario);
    }
}