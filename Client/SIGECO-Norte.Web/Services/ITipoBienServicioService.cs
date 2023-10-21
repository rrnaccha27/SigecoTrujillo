using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoBienServicioService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_bien_servicio instance);
        IResult Update(tipo_bien_servicio instance);
        tipo_bien_servicio GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}