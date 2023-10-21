using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IProductoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(producto instance);
        IResult Update(producto instance);
        producto GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboByTipoProductoJson(int codigoTipoProducto);
    }
}