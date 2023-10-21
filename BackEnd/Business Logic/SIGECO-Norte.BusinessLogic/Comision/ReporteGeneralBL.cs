using System;
using System.Collections.Generic;
using System.Data;

using SIGEES.Entidades;
using SIGEES.Entidades.BeanReporte;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{

    #region SECCION REPORTE
    public class ReporteGeneralBL : GenericBL<ReporteGeneralBL>
    {

        private readonly SIGEES.DataAcces.ReporteGeneralDA reporteDA = new SIGEES.DataAcces.ReporteGeneralDA();

        public DataTable ListarPersonaPorFechaRegistra(DateTime fechaInicio, DateTime fechaFin)
        {
            
            return reporteDA.ListarPersonaPorFechaRegistra(fechaInicio, fechaFin);
        }

        public DataTable ListarPersonaPorParametrosInterface(int codigoCanal, int codigoGrupo, int estadoRegistro)
        {
            return reporteDA.ListarPersonaPorParametrosInterface(codigoCanal, codigoGrupo, estadoRegistro);
        }

        public DataTable ListarCanalGrupoPorFechaRegistra(DateTime fechaInicio, DateTime fechaFin, int esCanalGrupo, int codigoPadre)
        {
            return reporteDA.ListarCanalGrupoPorFechaRegistra(fechaInicio, fechaFin, esCanalGrupo, codigoPadre);
        }
        public DataTable ListarCanalGrupoInterface(int esCanalGrupo, int codigoPadre)
        {
            return reporteDA.ListarCanalGrupoInterface(esCanalGrupo, codigoPadre);
        }

        #region Comun
        
        public DataTable Anio()
        {
            return ReporteGeneralDA.Instance.Anio();
        }

        #endregion

        #region representantes y gestores

        public List<reporte_resumen_comercial_dto> ReportePlanillaComisionVendedor(reporte_comercial_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.ReportePlanillaComisionVendedor(busqueda);
        }
        public List<reporte_detallado_vendedores_dto> Detalle(reporte_comercial_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.Detalle(busqueda);
        }

        public List<reporte_resumen_comercial_dto> Detalle_OrdenPago(reporte_comercial_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.Detalle_OrdenPago(busqueda);
        }

        #endregion

        #region SUPERVISORES

        public List<reporte_detallado_vendedores_dto> DetalleComisionPlanillaSupervisor(reporte_comercial_busqueda_dto busqueda)
        {
            //return new List<reporte_detallado_vendedores_dto>();
            return ReporteGeneralDA.Instance.DetalleComisionPlanillaSupervisor(busqueda);
        }

        public List<reporte_detallado_vendedores_dto> DetalleComisionPlanillaJefatura(reporte_comercial_busqueda_dto busqueda)
        {
            //return new List<reporte_detallado_vendedores_dto>();
            return ReporteGeneralDA.Instance.DetalleComisionPlanillaJefatura(busqueda);
        }

        public List<reporte_detallado_vendedores_dto> DetalleComisionContrato(reporte_comercial_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.DetalleComisionContrato(busqueda);
        }
        #endregion

        #region Finanzas

        public List<reporte_resumen_supervisores_dto> ReporteComisionSupervisores(reporte_comercial_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.ReporteComisionSupervisores(busqueda);
        }

        public reporte_finanzas_filtro_dto FinanzasFiltro(reporte_finanzas_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.FinanzasFiltro(busqueda);
        }

        #endregion

        #region "Migracion de Contratos"
        public List<reporte_migracion_contratos_dto> MigracionContratos(reporte_migracion_contratos_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.MigracionContratos(busqueda);
        }
        #endregion

        #region "Cuotas Iniciales Pendientes"
        public List<reporte_cuotas_iniciales_dto> CuotasIniciales(reporte_cuotas_iniciales_busqueda_dto busqueda)
        {
            return ReporteGeneralDA.Instance.CuotasIniciales(busqueda);
        }
        #endregion

    }

    #endregion
}