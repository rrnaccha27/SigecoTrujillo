using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoTrasladoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_traslado instance);
        IResult Update(tipo_traslado instance);
        tipo_traslado GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}