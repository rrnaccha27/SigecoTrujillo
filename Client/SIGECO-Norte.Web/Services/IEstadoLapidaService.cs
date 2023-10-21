using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEstadoLapidaService
    {
        string GetAllJson(bool isReadAll = false);
        List<estado_lapida> Lista();

        IResult Create(estado_lapida instance);
        IResult Update(estado_lapida instance);
        estado_lapida GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}