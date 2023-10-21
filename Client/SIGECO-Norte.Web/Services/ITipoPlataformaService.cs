using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoPlataformaService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_plataforma instance);
        IResult Update(tipo_plataforma instance);
        tipo_plataforma GetSingle(int id);
        string GetSingleJSON(int id);
        string GetComboJson();
    }
}