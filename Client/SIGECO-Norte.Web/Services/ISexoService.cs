using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ISexoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(sexo instance);
        IResult Update(sexo instance);
        sexo GetSingle(string id);
        string GetSingleJSON(string id);
        string GetComboJson(bool isReadAll = false);
    }
}