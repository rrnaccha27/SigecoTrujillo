using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
    public class ReglaRecuperoBL : GenericBL<ReglaRecuperoBL>
    {
        public List<regla_recupero_listado_dto> Listar(int codigo_tipo_planilla)
        {
            return ReglaRecuperoDA.Instance.Listar(codigo_tipo_planilla);
        }

        public regla_recupero_dto Unico(int codigo_regla_recupero)
        {
            return ReglaRecuperoDA.Instance.Unico(codigo_regla_recupero);
        }

        public MensajeDTO Insertar(regla_recupero_dto regla)
        {
            int codigo_regla_recupero = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_regla_recupero = ReglaRecuperoDA.Instance.Insertar(regla);
                    
                    v_mensaje.idRegistro = codigo_regla_recupero;
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

        public MensajeDTO Actualizar(regla_recupero_dto regla)
        {
            int codigo_regla_recupero = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_regla_recupero = regla.codigo_regla_recupero;
                        
                    ReglaRecuperoDA.Instance.Actualizar(regla);

                    v_mensaje.idRegistro = codigo_regla_recupero;
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

        public MensajeDTO Desactivar(regla_recupero_dto regla)
        {
            int codigo_regla_recupero = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_regla_recupero = regla.codigo_regla_recupero;

                    ReglaRecuperoDA.Instance.Desactivar(regla);

                    v_mensaje.idRegistro = codigo_regla_recupero;
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
