using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEmpresaSigecoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(empresa_sigeco instance);
        IResult Update(empresa_sigeco instance);
        empresa_sigeco GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}