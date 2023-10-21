using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoNivelService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_nivel instance);
        IResult Update(tipo_nivel instance);
        tipo_nivel GetSingle(string id);
        string GetSingleJSON(string id);
        string GetComboJson();
        string GetComboCantidadJson();
    }
}