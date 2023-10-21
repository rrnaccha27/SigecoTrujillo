using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.DataAcces;
using SIGEES.Entidades;


namespace SIGEES.BusinessLogic
{
    public partial class ChecklistComisionSelBL : GenericBL<ChecklistComisionSelBL>
    {
        public List<checklist_comision_listado_dto> Listar()
        {
            return ChecklistComisionSelDA.Instance.Listar();
        }

        public checklist_comision_listado_dto Detalle(int codigo_checklist)
        {
            return ChecklistComisionSelDA.Instance.Detalle(codigo_checklist);
        }

        public List<checklist_comision_detalle_listado_dto> ListarDetalle(int codigo_checklist, bool validado)
        {
            return ChecklistComisionSelDA.Instance.ListarDetalle(codigo_checklist, validado);
        }
    }

    public partial class ChecklistComisionBL : GenericBL<ChecklistComisionBL>
    {
        public MensajeDTO Aperturar(checklist_comision_estado_dto planilla)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistComisionDA.Instance.Aperturar(planilla);
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

        public MensajeDTO Anular(checklist_comision_estado_dto checklist)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistComisionDA.Instance.Anular(checklist);
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

        public MensajeDTO Cerrar(checklist_comision_estado_dto checklist)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistComisionDA.Instance.Cerrar(checklist);
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

        public MensajeDTO Validar(List<checklist_comision_detalle_listado_dto> checklist_detalle, string usuario_modifica)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistComisionDA.Instance.Validar(checklist_detalle, usuario_modifica);
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
    }


}
