using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoConversionEspacioService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_conversion_espacio instance);
        IResult Update(tipo_conversion_espacio instance);
        tipo_conversion_espacio GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
        string GetComboConversionByEspacioJson(int pCantidadMaximaConversion );
    }
}