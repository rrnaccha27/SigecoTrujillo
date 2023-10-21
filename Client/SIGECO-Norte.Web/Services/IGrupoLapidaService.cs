using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IGrupoLapidaService
    {
        string GetAllJson(bool isReadAll = false);
        List<grupo_lapida> Lista();
        IResult Create(grupo_lapida instance);
        IResult UpdateMultiple(grupo_lapida instance, bool confeccionado, bool colocado, DateTime fechaModifica);
        IResult DeleteMultiple(grupo_lapida instance);
        IResult Update(grupo_lapida instance);
        grupo_lapida GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}