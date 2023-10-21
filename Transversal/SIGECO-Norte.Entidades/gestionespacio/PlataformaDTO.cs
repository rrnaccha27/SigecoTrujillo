using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class PlataformaDTO
    {
        public int codigo_corporacion { set; get; }
        public int codigo_empresa { set; get; }
        public int codigo_campo_santo { set; get; }
        public int codigo_plataforma { set; get; }
        public string nombre_plataforma { set; get; }
        public string estado_registro { set; get; }
        public int numero_columnas { get; set; }
        public int numero_filas { get; set; }
    }
}
