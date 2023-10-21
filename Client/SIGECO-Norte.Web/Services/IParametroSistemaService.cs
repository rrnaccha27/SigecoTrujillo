using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IParametroSistemaService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(parametro_sistema instance);
        IResult Update(parametro_sistema instance);
        parametro_sistema GetSingle(int id);
        string GetSingleJSON(int id);
        parametro_sistema GetParametro(int id);
    }
}