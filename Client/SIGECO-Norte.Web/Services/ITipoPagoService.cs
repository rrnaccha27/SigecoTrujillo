using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoPagoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_pago instance);
        IResult Update(tipo_pago instance);
        tipo_pago GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}