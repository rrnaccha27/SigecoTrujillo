using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoProductoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_producto instance);
        IResult Update(tipo_producto instance);
        tipo_producto GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}