using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class EspacioDTO
    {
        public int NroColumnas { set; get; }
        public int NroFilas { set; get; }

        public int codigo_corporacion { set; get; }
        public int codigo_empresa { set; get; }
        public int codigo_campo_santo { set; get; }
        public int codigo_plataforma { set; get; }
        public string codigo_espacio { set; get; }
        public int tipo_espacio { set; get; }
        public int codigo_tipo_nivel { set; get; }
        public string eje_derecho { set; get; }
        public string eje_izquierdo { set; get; }
        public string je_superior { set; get; }
        public string eje_inferior { set; get; }
        public string codigo_plano { set; get; }
        public string numero_lote { set; get; }
    }
}
