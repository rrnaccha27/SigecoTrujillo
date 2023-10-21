using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoEspacioService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_espacio instance);
        IResult Update(tipo_espacio instance);
        tipo_espacio GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson(bool isReadAll = false);
        string GetComboByTipoLoteJson(bool tipoLote);
    }
}