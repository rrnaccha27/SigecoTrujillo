using SIGEES.DataAcces;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIGEES.BusinessLogic
{
    public partial class ContratoBL : GenericBL<ContratoBL>
    {


        public MensajeDTO Generar(analisis_contrato_dto v_entidad)
        {
            var v_mensaje = new MensajeDTO();
            //int v_codigo_planilla = 0;
            //int p_cantidad_registro_procesado;

            //
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
            //    try
            //    {

            //        v_codigo_planilla = PlanillaDA.Instance.Generar(v_planilla, out p_cantidad_registro_procesado);
                    
            //        v_mensaje.idRegistro = v_codigo_planilla;
            //        v_mensaje.idOperacion = 1;
            //        v_mensaje.total_registro_afectado = p_cantidad_registro_procesado;
            //        v_mensaje.mensaje = "Se generó correctamente";
            //        scope.Complete();
            //    }
            //    catch (Exception ex)
            //    {
            //        v_mensaje.mensaje = ex.Message;
            //        v_mensaje.idOperacion = -1;

            //    }
            //}

            return v_mensaje;
        }

        
    }

    public partial class ContratoSelBL : GenericBL<ContratoSelBL>
   {

        //public List<Planilla_dto> Listar(int pTamanioPage, int pNumeroPagina) 
        //{
        //    return PlanillaSelDA.Instance.Listar(pTamanioPage, pNumeroPagina);
        //}

        public analisis_contrato_dto BuscarByIdCronograma(int p_codigo_cronograma)
        {
            return ContratoSelDA.Instance.BuscarByIdCronograma(p_codigo_cronograma);
        }



        public List<analisis_contrato_articulo_cronograma_dto> ListarArticuloByContrato_Empresa(filtro_contrato_dto v_entidad,int sede)
        {
            return ContratoSelDA.Instance.ListarArticuloByContrato_Empresa(v_entidad, sede);
        }

        public List<detalle_cronograma_comision_dto> ListarCronogramaPagoByArticuloContrato(filtro_contrato_dto v_entidad)
        {
            return ContratoSelDA.Instance.ListarCronogramaPagoByArticuloContrato(v_entidad);
        }

        public analisis_contrato_dto BuscarByEmpresaContrato(int codigo_empresa, string nro_contrato, int codigo_sede)
        {
            return ContratoSelDA.Instance.BuscarByEmpresaContrato(codigo_empresa, nro_contrato, codigo_sede);
        }

        public List<analisis_contrato_cronograma_cuotas_dto> ListarCronogramaCuotasByContrato_Empresa(filtro_contrato_dto v_entidad)
        {
            return ContratoSelDA.Instance.ListarCronogramaCuotasByContrato_Empresa(v_entidad);
        }

        public List<analisis_contrato_combo_dto> ListarEmpresasByContrato(filtro_contrato_dto v_entidad)
        {
            return ContratoSelDA.Instance.ListarEmpresasByContrato(v_entidad);
        }

        public List<analisis_contrato_combo_dto> ListarTipoPlanillaByContrato(filtro_contrato_dto v_entidad)
        {
            return ContratoSelDA.Instance.ListarTipoPlanillaByContrato(v_entidad);
        }

    }
}
