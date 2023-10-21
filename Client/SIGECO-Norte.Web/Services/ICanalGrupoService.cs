using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ICanalGrupoService
    {
        string GetAllJson(bool isReadAll = false);
        IResult Create(canal_grupo instance);
        IResult Update(canal_grupo instance);
        canal_grupo GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}