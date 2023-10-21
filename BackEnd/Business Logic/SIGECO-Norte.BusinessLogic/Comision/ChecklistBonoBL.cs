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
    public partial class ChecklistBonoSelBL : GenericBL<ChecklistBonoSelBL>
    {
        #region BONO
        public List<checklist_bono_listado_dto> Listar()
        {
            return ChecklistBonoSelDA.Instance.Listar();
        }

        public checklist_bono_listado_dto Detalle(int codigo_checklist)
        {
            return ChecklistBonoSelDA.Instance.Detalle(codigo_checklist);
        }

        public List<checklist_bono_detalle_listado_dto> ListarDetalle(int codigo_checklist, bool validado)
        {
            return ChecklistBonoSelDA.Instance.ListarDetalle(codigo_checklist, validado);
        }
        #endregion

        #region BONO TRIMESTRAL
        public List<checklist_bono_listado_dto> ListarTrimestral()
        {
            return ChecklistBonoSelDA.Instance.ListarTrimestral();
        }

        public checklist_bono_listado_dto DetalleTrimestral(int codigo_checklist)
        {
            return ChecklistBonoSelDA.Instance.DetalleTrimestral(codigo_checklist);
        }

        public List<checklist_bono_detalle_listado_dto> ListarDetalleTrimestral(int codigo_checklist, bool validado)
        {
            return ChecklistBonoSelDA.Instance.ListarDetalleTrimestral(codigo_checklist, validado);
        }

        public List<cabecera_txt_dto> TXTParaRRHH(int codigo_checklist)
        {
            return ChecklistBonoSelDA.Instance.TXTParaRRHH(codigo_checklist);
        }
        public List<txt_contabilidad_resumen_planilla_dto> TXTResumenParaContabilidad(int codigo_checklist)
        {
            return ChecklistBonoSelDA.Instance.TXTResumenParaContabilidad(codigo_checklist);
        }

        public List<txt_contabilidad_planilla_dto> TXTParaContabilidad(int codigo_checklist, int codigo_empresa)
        {
            return ChecklistBonoSelDA.Instance.TXTParaContabilidad(codigo_checklist, codigo_empresa);
        }

        #endregion

    }

    public partial class ChecklistBonoBL : GenericBL<ChecklistBonoBL>
    {
        #region BONO
        public MensajeDTO Aperturar(checklist_bono_estado_dto planilla)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.Aperturar(planilla);
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

        public MensajeDTO Anular(checklist_bono_estado_dto checklist)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.Anular(checklist);
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

        public MensajeDTO Cerrar(checklist_bono_estado_dto checklist)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.Cerrar(checklist);
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

        public MensajeDTO Validar(List<checklist_bono_detalle_listado_dto> checklist_detalle, string usuario_modifica)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.Validar(checklist_detalle, usuario_modifica);
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
        #endregion

        #region BONO TRIMESTRAL

        public MensajeDTO AperturarTrimestral(checklist_bono_estado_dto planilla)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.AperturarTrimestral(planilla);
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

        public MensajeDTO AnularTrimestral(checklist_bono_estado_dto checklist)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.AnularTrimestral(checklist);
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

        public MensajeDTO CerrarTrimestral(checklist_bono_estado_dto checklist)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.CerrarTrimestral(checklist);
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

        public MensajeDTO ValidarTrimestral(List<checklist_bono_detalle_listado_dto> checklist_detalle, string usuario_modifica)
        {
            var v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ChecklistBonoDA.Instance.ValidarTrimestral(checklist_detalle, usuario_modifica);
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
        
        #endregion

    }


}
