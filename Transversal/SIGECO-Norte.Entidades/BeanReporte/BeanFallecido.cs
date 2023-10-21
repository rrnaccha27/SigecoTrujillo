using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades.BeanReporte
{
    public class BeanFallecido
    {
        public int codigoFallecido { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public DateTime fechaDefuncion { get; set; }
        public string nombreSexo { get; set; }
    }
}
