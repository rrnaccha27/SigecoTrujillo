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
    public class ReglaBonoTrimestralBL : GenericBL<ReglaBonoTrimestralBL>
    {
        public MensajeDTO Insertar(regla_bono_trimestral_dto v_entidad, List<grilla_regla_bono_trimestral_detalle_dto> lista_detalle_regla, List<grilla_regla_bono_trimestral_meta_dto> lst_meta)
        {
            int v_codigo_regla = 0;
            int v_codigo_regla_detalle = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                   
                   if (v_entidad.codigo_regla <= 0)
                   {
                       v_codigo_regla = ReglaBonoTrimestralDA.Instance.Insertar(v_entidad);
                   }
                   else
                   {
                       v_codigo_regla = ReglaBonoTrimestralDA.Instance.Actualizar(v_entidad);
                   }

                   foreach (var item in lista_detalle_regla)
                   {
                       item.codigo_regla = v_codigo_regla;
                       if(item.codigo_regla_detalle<=0)
                       {
                           v_codigo_regla_detalle = ReglaBonoTrimestralDA.Instance.InsertarDetalle(item);
                       }
                   }

                   foreach (var item in lst_meta)
                   {
                       item.codigo_regla = v_codigo_regla;
                       if (item.codigo_regla_meta <= 0)
                       {
                           v_codigo_regla_detalle = ReglaBonoTrimestralDA.Instance.InsertarMeta(item);
                       }
                   }

                    v_mensaje.idRegistro = v_codigo_regla;
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

        public List<grilla_regla_bono_trimestral_dto> Listar()
        {
            return ReglaBonoTrimestralDA.Instance.Listar();
        }

        public regla_bono_trimestral_dto BuscarById(int p_codigo_regla)
        {
            return ReglaBonoTrimestralDA.Instance.BuscarById(p_codigo_regla);
        }

        public List<grilla_regla_bono_trimestral_detalle_dto> ListarDetalle(int p_codigo_regla)
        {
            return ReglaBonoTrimestralDA.Instance.ListarDetalle(p_codigo_regla);
        }

        public List<grilla_regla_bono_trimestral_meta_dto> ListarMeta(int p_codigo_regla)
        {
            return ReglaBonoTrimestralDA.Instance.ListarMeta(p_codigo_regla);
        }

        public MensajeDTO Eliminar(regla_bono_trimestral_dto v_entidad)
        {
            int v_codigo_regla = 0;            
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ReglaBonoTrimestralDA.Instance.Eliminar(v_entidad);

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

        public List<combo_regla_bono_trimestral_dto> GetAllComboJson()
        {
            return ReglaBonoTrimestralDA.Instance.GetAllComboJson();
        }
    }
}


