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
    public class ReglaTipoPlanillaBL : GenericBL<ReglaTipoPlanillaBL>
    {
        public MensajeDTO Insertar(regla_tipo_planilla_dto v_entidad, List<grilla_detalle_regla_tipo_planilla_dto> lista_detalle_regla)
        {
            int v_codigo_regla_tipo_planilla = 0;
            int v_codigo_detalle_regla_tipo_planilla = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                   
                   if (v_entidad.codigo_regla_tipo_planilla <= 0)
                   {
                       v_codigo_regla_tipo_planilla = ReglaTipoPlanillaDA.Instance.Insertar(v_entidad);
                   }
                   else
                   {
                       v_codigo_regla_tipo_planilla = ReglaTipoPlanillaDA.Instance.Actualizar(v_entidad);
                   }
                   foreach (var item in lista_detalle_regla)
                   {
                       item.codigo_regla_tipo_planilla = v_codigo_regla_tipo_planilla;
                       if(item.codigo_detalle_regla_tipo_planilla<=0)
                       {
                           v_codigo_detalle_regla_tipo_planilla = ReglaTipoPlanillaDA.Instance.InsertarDetalle(item);
                       }
                       
                   }
                    
                    v_mensaje.idRegistro = v_codigo_regla_tipo_planilla;
                    v_mensaje.idOperacion = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;

                }
            }

            return v_mensaje;
        }

        public List<grilla_regla_tipo_planilla_dto> Listar()
        {
            return ReglaTipoPlanillaDA.Instance.Listar();
        }

        public regla_tipo_planilla_dto BuscarById(int p_codigo_regla_tipo_planilla)
        {
            return ReglaTipoPlanillaDA.Instance.BuscarById(p_codigo_regla_tipo_planilla);
        }

        public List<grilla_detalle_regla_tipo_planilla_dto> ListarDetalle(int p_codigo_regla_tipo_planilla)
        {
            return ReglaTipoPlanillaDA.Instance.ListarDetalle(p_codigo_regla_tipo_planilla);
        }

        public MensajeDTO Eliminar(regla_tipo_planilla_dto v_entidad)
        {
            int v_codigo_regla_tipo_planilla = 0;            
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_regla_tipo_planilla = ReglaTipoPlanillaDA.Instance.Eliminar(v_entidad);
                    v_mensaje.idRegistro = v_codigo_regla_tipo_planilla;
                    v_mensaje.idOperacion = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;

                }
            }

            return v_mensaje;
        }

        public List<combo_regla_tipo_planilla_dto> GetAllComboJson()
        {
            return ReglaTipoPlanillaDA.Instance.GetAllComboJson();
        }
    }
}


