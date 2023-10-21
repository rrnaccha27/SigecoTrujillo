using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{
    public partial interface BaseSelDA<T> where T : class
    {
        
        T BuscarById(T pEntidad);
        List<T> Listar();
        List<T> Buscar(T pEntidad);
    }

    public partial interface BaseDA<T> where T : class
    {
        MensajeDTO Insertar(T pEntidad);

        MensajeDTO Actualizar(T pEntidad);
        MensajeDTO Eliminar(T pEntidad);
    }
}
