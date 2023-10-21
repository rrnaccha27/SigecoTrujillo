using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoBienService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_bien instance);
        IResult Update(tipo_bien instance);
        tipo_bien GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}