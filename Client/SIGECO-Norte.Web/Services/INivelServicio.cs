using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface INivelServicio
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(nivel_servicio instance);
        IResult Update(nivel_servicio instance);
        nivel_servicio GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}