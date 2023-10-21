using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IVistaPabellonService
    {
        string GetAllJson(bool isReadAll = false);
        List<vista_pabellon> Lista();

        IResult Create(vista_pabellon instance);
        IResult Update(vista_pabellon instance);
        vista_pabellon GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}