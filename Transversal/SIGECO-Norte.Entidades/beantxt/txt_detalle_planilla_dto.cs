using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class cabecera_txt_dto
    {
        
        public string fecha_proceso { set; get; }
        public string numero_planilla { get; set; }
      //  public string simbolo_moneda { set; get; }

        public int codigo_empresa { set; get; }
        public string nombre_empresa { set; get; }
        public string numero_cuenta_desembolso { get; set; }
        public string moneda_cuenta_desembolso { get; set; }            
        public decimal importe_desembolso_empresa { get; set; }
        public string tipo_cuenta_desembolso { get; set; }

        public int codigo_personal { get; set; }
        public string nombre_personal { set; get; }       
        public string nombre_tipo_documento { set; get; }
        public string nro_documento { set; get; }        
        public string numero_cuenta_abono { get; set; }
        public string moneda_cuenta_abono { get; set; }
        public decimal importe_abono { set; get; }
        public string tipo_cuenta_abono { get; set; }

        /// <summary>
        /// SE REFIERE FACTURA, BOLETA, OTROS
        /// </summary>
        public string tipo_documento_pagar { get; set; }
        /// <summary>
        /// SE REFIERE NRO DE FACTURA, BOLETA, OTROS
        /// </summary>
        public string numero_documento_pagar { get; set; }
        public string tipo_abono { get; set; }
        public string tipo_producto { get; set; }
        public string tipo_producto_cabecera { get; set; }

        public bool calcular_detraccion { get; set; }

        public string checksum { get; set; }

        public string codigo_agrupador { get; set; }
    }

    //public partial class detalle_txt_dto
    //{
    //    public string fecha_proceso { set; get; }
    //    public string simbolo_moneda { set; get; }
    //    public string nombre_tipo_documento { set; get; }
    //    public string nro_documento { set; get; }
    //    public string nombre_personal { set; get; }
    //    public int neto_pagar_personal_x_empresa { set; get; }

    //}

}
