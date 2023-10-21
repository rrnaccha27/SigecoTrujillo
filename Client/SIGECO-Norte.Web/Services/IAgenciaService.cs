using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IAgenciaService
    {
        string GetAllJson(bool isReadAll = false);
        List<agencia> Lista();

        IResult Create(agencia instance);
        IResult Update(agencia instance);
        agencia GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}