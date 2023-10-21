using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEncofradoService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(encofrado instance);
        IResult Update(encofrado instance);
        encofrado GetSingle(int id);
        encofrado GetByEspacio(string codigoEspacio);
        string GetSingleJSON(int id);
    }
}