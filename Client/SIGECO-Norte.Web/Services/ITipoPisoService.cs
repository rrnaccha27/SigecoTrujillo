using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoPisoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_piso instance);
        IResult Update(tipo_piso instance);
        tipo_piso GetSingle(int id);
        string GetSingleJSON(int id);

    }
}