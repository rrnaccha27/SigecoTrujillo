using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class descuento_dto : Auditoria_dto
    {
        public descuento_dto()
        {
        }
        public int codigo_descuento { set; get; }
        public int codigo_planilla { set; get; }
        public int codigo_personal { set; get; }
        public decimal monto { set; get; }
        public string motivo { set; get; }

        public int codigo_empresa { get; set; }
    }

    public partial class lista_descuento_dto : Auditoria_dto
    {

        public int codigo_descuento { set; get; }
        public int codigo_planilla { set; get; }
        public int codigo_personal { set; get; }
        public decimal monto { set; get; }
        public string motivo { set; get; }

        public string nombre_grupo_canal { set; get; }
        public string apellido_materno { set; get; }
        public string apellido_paterno { set; get; }
        public string nombre { set; get; }
        public string apellidos_nombres { set; get; }



        public string nombre_empresa { get; set; }

        public string nombre_grupo { get; set; }

        

        public int indica_estado_registro
        {
            get
            {
                return estado_registro ? 1 : 0;
            }
        }



    }
}
