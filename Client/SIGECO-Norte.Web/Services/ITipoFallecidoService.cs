using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoFallecidoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_fallecido instance);
        IResult Update(tipo_fallecido instance);
        tipo_fallecido GetSingle(int id);
        string GetSingleJSON(int id);

    }
}