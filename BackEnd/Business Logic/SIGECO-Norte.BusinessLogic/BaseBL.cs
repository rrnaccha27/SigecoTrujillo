using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.BusinessLogic
{
    public partial interface BaseSelBL<T> where T : class
    {

        T BuscarById(T pEntidad);
        List<T> Listar();
        List<T> Buscar(T pEntidad);
    }

    public partial interface BaseBL<T> where T : class
    {
        MensajeDTO Insertar(T pEntidad);
        MensajeDTO Actualizar(T pEntidad);
        MensajeDTO Eliminar(T pEntidad);
    }

 
}
