using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ICaracteristicaLoteService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(caracteristica_lote instance);
        IResult Update(caracteristica_lote instance);
        caracteristica_lote GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}