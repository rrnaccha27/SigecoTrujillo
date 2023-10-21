using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class txt_contabilidad_resumen_planilla_dto
    {
        public int codigo_checklist { set; get; }
        public int codigo_empresa { set; get; }
        public int codigo_planilla { get; set; }
		public string nombre_empresa { get; set; }
        public string numero_planilla { get; set; }
		public int comisiones { get; set; }
    }
    public partial class txt_contabilidad_planilla_dto
    {
        public string COD_COMISION{get; set;}
        public string N_CUOTA{get; set;}
        public string IMP_PAGAR{get; set;}
        public string COD_EMPRESA_G{get; set;}
        public string NUM_CONTRATO{get; set;}
        public string IGV{get; set;}
        public string DES_TIPO_VENTA{get; set;}
        public string TIPO_VENTA{get; set;}
        public string FEC_HAVILITADO{get; set;}
        public string DES_FORMA_PAGO{get; set;}
        public string ID_FORMA_DE_PAGO{get; set;}
        public string DES_TIPO_COMISION{get; set;}
        public string TIPO_COMISION{get; set;}
        public string CUARTA{get; set;}
        public string IES{get; set;}
        public string TIPO_MONEDA{get; set;}
        public string TIPO_AGENTE_G{get; set;}
        public string C_AGENTE{get; set;}
        public string COD_GRUPO_VENTA_G{get; set;}
        public string NOMBRE_GRUPO{get; set;}
        public string DESCRIPCION_1{get; set;}
        public string COD_BIEN{get; set;}
        public string DESCRIPCION_2{get; set;}
        public string COD_CONCEPTO{get; set;}
        public string TIPO_CAMBIO{get; set;}
        public string SALDO_A_PAGAR{get; set;}
        public string FEC_PLANILLA{get; set;}
        public string USU_PLANILLA{get; set;}
        public string N_OPERACION{get; set;}
        public string FEC_CIERRE{get; set;}
        public string DESC_TIPO_DOCUM{get; set;}
        public string RUC{get; set;}
        public string NUM_DOC{get; set;}
        public string NOM_AGENTE{get; set;}
        public string NOM_EMPRESA{get; set;}
        public string DIR_EMPRESA{get; set;}
        public string RUC_EMPRESA{get; set;}
        public string FEC_INICIO{get; set;}
        public string FEC_TERMINO{get; set;}
        public string DSCTO_ESTUDIO_C{get; set;}
        public string DSCTO_DETRACCION{get; set;}
        public string PORC_IGV{get; set;}
        public string UNIDAD_NEGOCIO { get; set; }
        public string DIMENSION_3 { get; set; }
        public string DIMENSION_4 { get; set; }
        public string DIMENSION_5 { get; set; }

        public override string ToString()
        {
            string linea = string.Empty;

            linea = COD_COMISION + "\t" + N_CUOTA + "\t" + IMP_PAGAR + "\t" + COD_EMPRESA_G + "\t" + NUM_CONTRATO + "\t" + IGV + "\t" + DES_TIPO_VENTA + "\t" + TIPO_VENTA + "\t" + FEC_HAVILITADO + "\t" + DES_FORMA_PAGO + "\t" + ID_FORMA_DE_PAGO + "\t" + DES_TIPO_COMISION + "\t" + TIPO_COMISION + "\t" + CUARTA + "\t" + IES + "\t" + TIPO_MONEDA + "\t" + TIPO_AGENTE_G + "\t" + C_AGENTE + "\t" + COD_GRUPO_VENTA_G + "\t" + NOMBRE_GRUPO + "\t" + DESCRIPCION_1 + "\t" + COD_BIEN + "\t" + DESCRIPCION_2 + "\t" + COD_CONCEPTO + "\t" + TIPO_CAMBIO + "\t" + SALDO_A_PAGAR + "\t" + FEC_PLANILLA + "\t" + USU_PLANILLA + "\t" + N_OPERACION + "\t" + FEC_CIERRE + "\t" + DESC_TIPO_DOCUM + "\t" + RUC + "\t" + NUM_DOC + "\t" + NOM_AGENTE + "\t" + NOM_EMPRESA + "\t" + DIR_EMPRESA + "\t" + RUC_EMPRESA + "\t" + FEC_INICIO + "\t" + FEC_TERMINO + "\t" + DSCTO_ESTUDIO_C + "\t" + DSCTO_DETRACCION + "\t" + PORC_IGV + "\t" + UNIDAD_NEGOCIO + "\t" + DIMENSION_3 + "\t" + DIMENSION_4 + "\t" + DIMENSION_5;

            return linea;
        }
    }



}
