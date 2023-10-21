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
    public class PlanIntegralBL : GenericBL<PlanIntegralBL>
    {
        public List<plan_integral_listado_dto> Listar(int estado_registro)
        {
            return PlanIntegralDA.Instance.Listar(estado_registro);
        }

        public plan_integral_dto Unico(int codigo_plan_integral)
        {
            return PlanIntegralDA.Instance.Unico(codigo_plan_integral);
        }

        public MensajeDTO Insertar(plan_integral_dto plan)
        {
            int codigo_plan_integral = 0;
            bool validado = false;
            string usuario = string.Empty;
            MensajeDTO v_mensaje = new MensajeDTO();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_plan_integral = PlanIntegralDA.Instance.Insertar(plan);
                    usuario = plan.usuario;
                    foreach (var detalle in plan.plan_integral_detalle) {
                        detalle.codigo_plan_integral = codigo_plan_integral;
                        detalle.usuario = usuario;

                        PlanIntegralDetalleDA.Instance.Insertar(detalle);
                    }

                    validado = PlanIntegralDA.Instance.Validar(codigo_plan_integral);

                    if (validado)
                    {
                        v_mensaje.idRegistro = codigo_plan_integral;
                        v_mensaje.idOperacion = 1;
                        scope.Complete();
                    }
                    else
                    {
                        v_mensaje.mensaje = "Existe una configuracionq que ya esta registrada en otro Plan Integral.";
                        v_mensaje.idOperacion = -1;
                    }
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;
                }
            }

            return v_mensaje;
        }

        public MensajeDTO Actualizar(plan_integral_dto plan)
        {
            bool validado = false;
            int codigo_plan_integral = 0;
            string usuario = string.Empty;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_plan_integral = plan.codigo_plan_integral;

                    //PlanIntegralDA.Instance.Actualizar(plan);

                    usuario = plan.usuario;
                    foreach (var detalle in plan.plan_integral_detalle)
                    {
                        detalle.codigo_plan_integral = codigo_plan_integral;
                        detalle.usuario = usuario;

                        if (detalle.estado_registro == false) {
                            PlanIntegralDetalleDA.Instance.Desactivar(detalle);
                        }
                        else if (detalle.codigo_plan_integral_detalle < 0){
                            PlanIntegralDetalleDA.Instance.Insertar(detalle);
                        }
                    }

                    validado = PlanIntegralDA.Instance.Validar(codigo_plan_integral);

                    if (validado)
                    {
                        v_mensaje.idRegistro = codigo_plan_integral;
                        v_mensaje.idOperacion = 1;
                        scope.Complete();
                    }
                    else
                    {
                        v_mensaje.mensaje = "Existe una configuracionq que ya esta registrada en otro Plan Integral.";
                        v_mensaje.idOperacion = -1;
                    }
                }
                catch (Exception ex)
                {
                    v_mensaje.mensaje = ex.Message;
                    v_mensaje.idOperacion = -1;
                }
            }

            return v_mensaje;
        }

        public MensajeDTO Desactivar(plan_integral_dto regla)
        {
            int codigo_plan_integral = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_plan_integral = regla.codigo_plan_integral;

                    PlanIntegralDA.Instance.Desactivar(regla);

                    v_mensaje.idRegistro = codigo_plan_integral;
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

        public List<plan_integral_detalle_dto> DetalleListar(int codigo_plan_integral)
        {
            return PlanIntegralDetalleDA.Instance.Listar(codigo_plan_integral);
        }

        public MensajeDTO DetalleDesactivar(plan_integral_detalle_dto detalle)
        {
            int codigo_plan_integral_detalle = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    codigo_plan_integral_detalle = detalle.codigo_plan_integral_detalle;
                    PlanIntegralDetalleDA.Instance.Desactivar(detalle);

                    v_mensaje.idRegistro = codigo_plan_integral_detalle;
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
