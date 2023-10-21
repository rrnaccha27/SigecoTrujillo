using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    
    public interface IPlataformaService
    {
        string GetAllJson(bool isReadAll = false);
        IResult BulkInsertPlataforma(plataforma instance);
        IResult BulkInsertEspacio(int codigoPlataforma, List<espacio> listaEspacio);
        IResult CreateMultiple(plataforma instance);
        IResult Create(plataforma instance);
        IResult Update(plataforma instance);
        IResult ExisteIdentificador(string identificador);
        IResult Delete(plataforma instance);
        plataforma GetSingle(int id);
        IQueryable<plataforma> GetRegistrosByEmpresa(int pIdEmpresa);    

        string GetSingleJSON(int id);
        
    }
}