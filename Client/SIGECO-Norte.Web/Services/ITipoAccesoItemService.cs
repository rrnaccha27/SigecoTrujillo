using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITipoAccesoItemService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(tipo_acceso_item instance);
        IResult Update(tipo_acceso_item instance);
        tipo_acceso_item GetSingle(int id);
        string GetSingleJSON(int id);
        string GetAllBeanByPerfilJson(int codigoPerfil, bool isReadAll = false);
        BeanItemTipoAcceso GetBeanItemTipoAcceso(int codigoPerfil);
    }
}