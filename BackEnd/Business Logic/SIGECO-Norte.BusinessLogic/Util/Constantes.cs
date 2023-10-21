using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.BusinessLogic
{
    public enum log_wcf_objeto : int
    {
        Personal = 1,
        Articulo = 2,
        Canal = 3,
        Grupo = 4
    }

    public enum log_wcf_operacion : int
    {
        Registrar = 1,
        Modificar = 2,
        Desactivar = 3,
        Activar = 4
    }

    public enum log_wcf_resultado : int
    {
        Success = 1,
        Failure = 2
    }

}