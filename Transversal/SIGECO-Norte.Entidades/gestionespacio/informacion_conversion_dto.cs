using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public class informacion_conversion_dto
    {
        public int codigo_conversion_espacio { set; get; }
        public int? codigo_Fallecido { set; get; }
        public DateTime? fecha_exhumacion_final { set; get; }
        public DateTime? fecha_encofrado_fin { set; get; }
        public DateTime? fecha_inhumacion_fin { set; get; }
        public DateTime? fecha_fin_conversion { set; get; }
        public DateTime? fecha_inicio_conversion { set; get; }

        #region EXHUMACION
        public DateTime? fecha_exhumacion { set; get; }
        public int? codigo_tipo_conversion_espacio { get; set; }
        public string nombre_tipo_conversion_espacio { get; set; }

        public string numero_informe_exhumacion { set; get; }
        public string observacion_exhumacion { set; get; }
        #endregion


        #region  ENCONFRADO
        public DateTime? fecha_inicio_encofrado { set; get; }
        public string numero_orden_encofrado { set; get; }
        public string observacion_encofrado { set; get; }

        #endregion

        #region  INHUMACION
        public DateTime? fecha_inhumacion { set; get; }
        public string numero_orden_inhumacion { set; get; }
        public string observacion_inhumacion { set; get; }

        #endregion

        #region  FINALIZAR CONVERSION
        public DateTime? fecha_final_ejecucion { set; get; }
        public string numero_orden_ejecucion { set; get; }
        public string observacion_ejecucion { set; get; }

        #endregion

        public DateTime? fecha_movimiento { set; get; }
        public string str_fecha_movimiento { set; get; }
        public string str_fecha_movimiento_2 { set; get; }
        
        public string observacion_movimiento { set; get; }
        public string numero_informe_movimiento { set; get; }

        #region ESTADO CONVERSION

        public int codigo_estado_conversion { set; get; }

        public string codigo_espacio { set; get; }
        #endregion



        public int indica_operacion { get; set; }

        public string usuario_registra { get; set; }

        public DateTime? fecha_registra { get; set; }



        public string usuario_modifica { get; set; }

        public int codigo_estado_espacio { get; set; }

        public DateTime? fecha_movimiento_2 { get; set; }

        public string observacion_conversion { get; set; }

        public string numero_informe_conversion { get; set; }
    }
}
