using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEstadoEspacioService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(estado_espacio instance);
        IResult Update(estado_espacio instance);
        estado_espacio GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}