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
    public class ComisionManualBL : GenericBL<ComisionManualBL>
    {
        public List<comision_manual_listado_dto> Listar(comision_manual_filtro_dto filtro)
        {
            return ComisionManualDA.Instance.Listar(filtro);
        }

        public comision_manual_dto Detalle(int codigo_comision_manual)
        {
            return ComisionManualDA.Instance.Detalle(codigo_comision_manual);
        }

        public MensajeDTO Insertar(comision_manual_dto comision)
        {
            MensajeDTO v_mensaje = new MensajeDTO();

			try
			{
                v_mensaje.mensaje = ComisionManualDA.Instance.Insertar(comision);
				
                v_mensaje.idOperacion = (v_mensaje.mensaje.Length > 0?-1:1);
                 
			}
			catch (Exception ex)
			{
				v_mensaje.mensaje = ex.Message;
				v_mensaje.idOperacion = -1;
			}

            return v_mensaje;
        }

        public MensajeDTO Actualizar(comision_manual_dto comision)
        {
            MensajeDTO v_mensaje = new MensajeDTO();

			try
			{
                v_mensaje.mensaje = ComisionManualDA.Instance.Actualizar(comision);
                v_mensaje.idOperacion = (v_mensaje.mensaje.Length > 0 ? -1 : 1);
            }
			catch (Exception ex)
			{
				v_mensaje.mensaje = ex.Message;
				v_mensaje.idOperacion = -1;
			}

            return v_mensaje;
        }

        public MensajeDTO ActualizarLimitado(comision_manual_dto comision)
        {
            MensajeDTO v_mensaje = new MensajeDTO();

            try
            {
                ComisionManualDA.Instance.ActualizarLimitado(comision);
                v_mensaje.mensaje = string.Empty;
                v_mensaje.idOperacion = 1;
            }
            catch (Exception ex)
            {
                v_mensaje.mensaje = ex.Message;
                v_mensaje.idOperacion = -1;
            }

            return v_mensaje;
        }

        public MensajeDTO Desactivar(comision_manual_dto comision)
        {
            MensajeDTO v_mensaje = new MensajeDTO();

			try
			{
                v_mensaje = ComisionManualDA.Instance.Desactivar(comision);

				v_mensaje.idOperacion = 1;
			}
			catch (Exception ex)
			{
				v_mensaje.mensaje = ex.Message;
				v_mensaje.idOperacion = -1;
			}

            return v_mensaje;
        }

        public MensajeDTO Validar(comision_manual_dto comision)
        {
            MensajeDTO v_mensaje = new MensajeDTO();

            try
            {
                v_mensaje = ComisionManualDA.Instance.Validar(comision);

            }
            catch (Exception ex)
            {
                v_mensaje.mensaje = ex.Message;
                v_mensaje.idOperacion = -1;
                v_mensaje.codigoError = -1;
            }

            return v_mensaje;
        }

        public List<comision_manual_reporte_dto> Reporte(comision_manual_filtro_dto filtro)
        {
            return ComisionManualDA.Instance.Reporte(filtro);
        }
        public comision_manual_reporte_param_dto ReporteParam(comision_manual_filtro_dto filtro)
        {
            return ComisionManualDA.Instance.ReporteParam(filtro);
        }

        public string ValidarReferencia(int codigo_comision_manual)
        {
            return ComisionManualDA.Instance.ValidarReferencia(codigo_comision_manual);
        }

    }
}
