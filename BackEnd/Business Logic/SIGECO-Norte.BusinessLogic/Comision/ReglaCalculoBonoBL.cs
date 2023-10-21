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
    public class ReglaCalculoBonoBL : GenericBL<ReglaCalculoBonoBL>
    {
        public List<regla_calculo_bono_listado_dto> Listar(int codigo_tipo_planilla)
        {
            return ReglaCalculoBonoDA.Instance.Listar(codigo_tipo_planilla);
        }

        public regla_calculo_bono_dto Unico(int codigo_regla_calculo_bono)
        {
            return ReglaCalculoBonoDA.Instance.Unico(codigo_regla_calculo_bono);
        }

        public List<regla_calculo_bono_articulo_listado_dto> ArticuloListar(int codigo_regla_calculo_bono)
        {
            return ReglaCalculoBonoDA.Instance.ArticuloListar(codigo_regla_calculo_bono);
        }

        public List<regla_calculo_bono_matriz_listado_dto> MatrizListar(int codigo_regla_calculo_bono)
        {
            return ReglaCalculoBonoDA.Instance.MatrizListar(codigo_regla_calculo_bono);
        }

        public MensajeDTO Insertar(regla_calculo_bono_dto regla)
        {
            int codigo_regla_calculo_bono = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    codigo_regla_calculo_bono = ReglaCalculoBonoDA.Instance.Insertar(regla);
                    
                    if (regla.lista_matriz != null)
                    { 
                        foreach (var item in regla.lista_matriz)
                        {
                            item.codigo_regla_calculo_bono = codigo_regla_calculo_bono;
                            ReglaCalculoBonoDA.Instance.MatrizInsertar(item);
                        }
                    }

                    if (regla.lista_articulo != null)
                    { 
                        foreach (var item in regla.lista_articulo)
                        {
                            item.codigo_regla_calculo_bono = codigo_regla_calculo_bono;
                            ReglaCalculoBonoDA.Instance.ArticuloInsertar(item);
                        }
                    }

                    v_mensaje.idRegistro = codigo_regla_calculo_bono;
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

        public MensajeDTO Actualizar(regla_calculo_bono_dto regla)
        {
            int codigo_regla_calculo_bono = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    codigo_regla_calculo_bono = regla.codigo_regla_calculo_bono;
                        
                    ReglaCalculoBonoDA.Instance.Actualizar(regla);
                    ReglaCalculoBonoDA.Instance.MatrizEliminar(codigo_regla_calculo_bono);
                    ReglaCalculoBonoDA.Instance.ArticuloEliminar(codigo_regla_calculo_bono);

                    if (regla.lista_matriz != null)
                    {
                        foreach (var item in regla.lista_matriz)
                        {
                            item.codigo_regla_calculo_bono = codigo_regla_calculo_bono;
                            ReglaCalculoBonoDA.Instance.MatrizInsertar(item);
                        }
                    }

                    if (regla.lista_articulo != null)
                    { 
                        foreach (var item in regla.lista_articulo)
                        {
                            item.codigo_regla_calculo_bono = codigo_regla_calculo_bono;
                            ReglaCalculoBonoDA.Instance.ArticuloInsertar(item);
                        }
                    }

                    v_mensaje.idRegistro = codigo_regla_calculo_bono;
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

        public MensajeDTO Desactivar(regla_calculo_bono_dto regla)
        {
            int codigo_regla_calculo_bono = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_regla_calculo_bono = regla.codigo_regla_calculo_bono;

                    ReglaCalculoBonoDA.Instance.Desactivar(regla);

                    v_mensaje.idRegistro = codigo_regla_calculo_bono;
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

        public regla_calculo_bono_detalle_dto Detalle(int codigo_regla_calculo_bono)
        {
            return ReglaCalculoBonoDA.Instance.Detalle(codigo_regla_calculo_bono);
        }

        public int Validar(regla_calculo_bono_dto regla)
        {
            return ReglaCalculoBonoDA.Instance.Validar(regla);
        }

    }
}
