using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IPisoPabellonService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(piso_pabellon instance);
        IResult Update(piso_pabellon instance);
        piso_pabellon GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson(bool isReadAll = false);
        string GetComboMayorAJson(bool isReadAll = false,int level=0);

        string GetComboMenorAJson(bool isReadAll=false, int level = 0);
    }
}