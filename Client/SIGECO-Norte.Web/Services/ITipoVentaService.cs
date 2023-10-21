using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoVentaService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_venta instance);
        IResult Update(tipo_venta instance);
        tipo_venta GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}