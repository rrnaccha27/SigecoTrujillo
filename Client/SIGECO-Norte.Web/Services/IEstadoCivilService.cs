using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEstadoCivilService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(estado_civil instance);
        IResult Update(estado_civil instance);
        estado_civil GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson(bool isReadAll = false);
    }
}