using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ICorporacionService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(corporacion instance);
        IResult Update(corporacion instance);
        corporacion GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson(bool isReadAll = false);
    }
}