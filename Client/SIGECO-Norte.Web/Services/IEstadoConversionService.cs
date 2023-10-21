using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEstadoConversionService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(estado_conversion instance);
        IResult Update(estado_conversion instance);
        estado_conversion GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}