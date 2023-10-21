using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models.Bean
{
    public class BeanItemTipoAcceso
    {
        public String estadoPermisoTotal { get; set; }
        public String estadoLectura { get; set; }
        public String estadoEscritura { get; set; }
        public String estadoModificacion { get; set; }
        public String estadoEliminacion { get; set; }
        public String estadoReporte { get; set; }
        public String estadoBusqueda { get; set; }

        public String estadoSeleccionEspacio { get; set; }
        public String estadoReservarEspacio { get; set; }
        public String estadoRenovarReserva { get; set; }
        public String estadoAnularReserva { get; set; }
        public String estadoVenderEspacio { get; set; }
        public String estadoAnularVenta { get; set; }
        public String estadoRegistrarFallecido { get; set; }
        public String estadoRegistrarLapida { get; set; }
        public String estadoConversionCinerario { get; set; }
        public String estadoModificarGestionEspacio { get; set; }
        public String estadoGenerarTXT_RRHH { get; set; }
        public String estadoGenerarTXT_Cont { get; set; }
        public String estadoRevisionPlanilla { get; set; }
        public String estadoEscrituraArticulo { get; set; }
        public String estadoEscrituraPersonal { get; set; }
        public String estadoEscrituraAnalisisComision { get; set; }
        public String estadoLecturaAnalisisComision { get; set; }
        public String estadoReclamoCrear { get; set; }
        public String estadoReclamoAtenderN1 { get; set; }
        public String estadoReclamoAtenderN2 { get; set; }
        public String estadoComisionManualListarAll { get; set; }
        public String estadoComisionManualEscritura{ get; set; }

        public String estadoChecklistEscritura { get; set; }

        public String estadoComisionInactivaNivel1 { get; set; }
        public String estadoComisionInactivaNivel2 { get; set; }
    }
}
