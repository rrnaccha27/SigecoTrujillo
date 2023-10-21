using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IPerfilUsuarioService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(perfil_usuario instance);
        IResult Update(perfil_usuario instance);
        perfil_usuario GetSingle(int id);
        string GetSingleJSON(int id);

        IResult Create_Multiple(perfil_usuario instance);
        IResult Update_Multiple(perfil_usuario instance);
        IResult Delete_Multiple(perfil_usuario instance);
        string GetComboJson(bool isReadAll = false);
    }
}