using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class cronograma_pago_comision_dto
    {

    }

    public partial class cronograma_pago_filtro
    {
        //public int codigo_cronogrma { set; get; }
        public int codigo_articulo { set; get; }
        public int codigo_empresa { set; get; }
        public string nro_contrato { set; get; }
        public int codigo_personal { get; set; }
    }
    public partial class resumen_pago_comision_personal_dto : Auditoria_dto
    {
        public int codigo_cronograma { set; get; }
        public int codigo_articulo { set; get; }
        public int codigo_empresa { set; get; }
        public string nro_contrato { set; get; }
        public int total_registro_encontrado { set; get; }

        public decimal monto_comision_articulo { set; get; }
        public decimal monto_bruto { set; get; }
        public decimal monto_igv { set; get; }
        public decimal monto_neto { set; get; }
        public decimal monto_neto_pendiente { set; get; }
        public decimal monto_neto_en_proceso_pago { set; get; }
        public decimal monto_neto_pagado { set; get; }
        public decimal monto_neto_excluido { set; get; }
        public decimal monto_neto_anulado { set; get; }

    }


    #region  SECCION LISTADO DE PAGO DE COMISION

    public partial class grilla_comision_cronograma_filtro
    {

        public int? codigo_estado_cuota { set; get; }
        public int? codigo_grupo { set; get; }
        public int? codigo_canal { set; get; }
        public int? codigo_tipo_planilla { get; set; }
        public int? codigo_personal { get; set; }


        public string str_fecha_habilitado_inicio { get; set; }
        public string str_fecha_habilitado_fin { get; set; }

        public string str_fecha_contrato_inicio { get; set; }
        public string str_fecha_contrato_fin { get; set; }
 


        public DateTime? fecha_habilitado_inicio { get; set; }
        public DateTime? fecha_habilitado_fin { get; set; }

        public DateTime? fecha_contrato_inicio { get; set; }
        public DateTime? fecha_contrato_fin { get; set; }
    }
    public partial class grilla_comision_cronograma_dto
    {

        public string numero_planilla { get; set; }

        public string nombre_regla { get; set; }

        public string nombre_articulo { get; set; }

        public string nombre_canal { get; set; }

        public string nombre_grupo { get; set; }

        public string datos_vendedor { get; set; }

        public string nombre_empresa { get; set; }

        public string nombre_tipo_venta { get; set; }

        public string nombre_tipo_pago { get; set; }

        public string nro_contrato { get; set; }

        public string fecha_programada { get; set; }

        public decimal monto_bruto { get; set; }
        public decimal monto_neto { get; set; }
        public decimal igv { get; set; }


        public string str_monto_bruto
        {
            get
            {

                return monto_bruto.ToString("N2");
            }
        }
        public string str_monto_neto
        {
            get
            {

                return monto_neto.ToString("N2");
            }
        }
        public string str_igv
        {
            get
            {

                return igv.ToString("N2");
            }
        }


        public int nro_cuota { get; set; }



        public string nombre_estado_cuota { get; set; }

        public bool estado_registro { get; set; }

        public string nombre_estado_registro
        {
            get
            {
                return estado_registro ? "Activo" : "Inactivo";
            }
        }

    }
    #endregion


}
