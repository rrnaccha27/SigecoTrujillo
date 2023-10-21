using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoEventoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_evento instance);
        IResult Update(tipo_evento instance);
        tipo_evento GetSingle(int id);
        string GetSingleJSON(int id);

    }
}