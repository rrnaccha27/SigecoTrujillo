using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ICabeceraLapidaService
    {
        string GetAllJson(bool isReadAll = false);
        List<cabecera_lapida> Lista();

        IResult Create(cabecera_lapida instance);
        IResult Update(cabecera_lapida instance);
        cabecera_lapida GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();

        string GetAllByEstadoLapidaJson(int codigoEstadoLapida);
        string GetAllByGrupoLapidaJson(int codigoGrupoLapida);
    }
}