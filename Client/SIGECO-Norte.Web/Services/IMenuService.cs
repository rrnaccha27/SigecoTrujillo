using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IMenuService
    {
        IResult Create(menu instance);
        IResult Update(menu instance);
        IResult MoveDirection(int codigoMenu, Direction direction);
        bool IsExists(int codigoMenu);
        menu GetSingle(int codigoMenu);
        IQueryable<menu> GetNodes(bool isReadAll = false);
        List<string> GetListaRutaMenuPerfil(int codigoPerfil);
        string GetTreeJson();
        string GetSingleJSON(int codigoMenu);
        string GetAllJson(bool isReadAll = false);
        bool EsRoot(int codigoMenu);
        string GetTreeJsonByPerfil(int codigoPerfil);
        string GetMenuPrincipalByPerfilJson(int codigoPerfil);
        List<String> ObtenerCodigoMenuPadre(List<Nullable<int>> listaCodigos);
    }
}