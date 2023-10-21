using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEspacioService
    {
        string GetAllJson(bool isReadAll = false);

        IResult Create(espacio instance);
        IResult Update(espacio instance);
        espacio GetSingle(string id);
        //string GetSingleJSON(int id);
        espacio  GetRegistroById(string id);
        IQueryable<espacio> GetRegistrosByIdPlataform(int id);
        string GetAllEspacioByCampoSantoYCodigoJson(int codigoCampoSanto, string codigoEspacio);
        string GetAllEspacioByProductoJson(int codigoProducto);
        string GetAllEspacioByCampoSantoYNotInProductoJson(int codigoCampoSanto, int codigoProducto, string codigoEspacio);
    }
}