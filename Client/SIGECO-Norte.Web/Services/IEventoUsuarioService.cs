using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface IEventoUsuarioService
    {
        IResult GenerarEvento(BeanSesionUsuario beanSesionUsuario, int codigoTipoEvento, bool estadoEvento, List<BeanEntidad> listaBeanEntidad);

        string GetAllEventoUsuarioJson(string codigoUsuario, DateTime fechaInicio, DateTime fechaFin);
        string GetAllDetalleEntidadByEventoUsuarioJson(int codigoEventoUsuario);
        string GetAllDetalleAtributoByDetalleEntidadJson(int codigoDetalleEntidad);
    }
}