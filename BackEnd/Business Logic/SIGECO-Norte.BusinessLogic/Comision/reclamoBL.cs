using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
	public class reclamoBL
	{
        private readonly SIGEES.DataAcces.reclamoDA oReclamoDA = new DataAcces.reclamoDA();

		public string Registrar(reclamo_dto oBe)
		{
			return oReclamoDA.Registrar(oBe);
		}

		public int Actualizar(reclamo_dto oBe)
		{
            int resultado = 0;

            try
            {
                oReclamoDA.Actualizar(oBe);
                resultado = 1;
            }
            catch (Exception ex)
            {
                resultado = -1;
            }

            return resultado;
		}

		public int Eliminar(Int32 codigo_reclamo)
		{
			return oReclamoDA.Eliminar(codigo_reclamo);
		}

		public reclamo_dto GetReg(Int32 codigo_reclamo)
		{
			return oReclamoDA.GetReg(codigo_reclamo);
		}

        //public List<reclamo_dto> ListarAll(String NroContrato, Int32 codigo_estado_reclamo)
        //{
        //    return oReclamoDA.ListarAll(NroContrato, codigo_estado_reclamo);
        //}

		public List<reclamo_dto> Listar(reclamo_busqueda_dto oBE)
		{
			return oReclamoDA.Listar(oBE);
		}

        public string ValidarRegistro(reclamo_dto oBe)
        {
            return oReclamoDA.ValidarRegistro(oBe);
        }

        public int ValidarContrato(int codigo_empresa, string nro_contrato, int personal)
        {
            return oReclamoDA.ValidarContrato(codigo_empresa, nro_contrato, personal);
        }

        public string ValidarPago(reclamo_dto oBe)
        {
            return oReclamoDA.ValidarPago(oBe);
        }

        public reclamo_estado_contrato_dto EstadoContrato(int codigo_empresa, string nro_contrato)
        {
            return oReclamoDA.EstadoContrato(codigo_empresa, nro_contrato);
        }

        public bool AtenderContratoMigrado(reclamo_dto reclamo)
        {
            return oReclamoDA.AtenderContratoMigrado(reclamo);
        }

        public int AtenderN1(reclamo_atencion_n1_dto reclamo)
        {
            int retorno = 1;
            try
            {
                oReclamoDA.AtenderN1(reclamo);
            }
            catch (Exception ex)
            {
                retorno = -1;
            }
            return retorno;
        }

        public int ObtenerPendientes(string usuario)
        {
            int retorno = 0;
            try
            {
                retorno = oReclamoDA.ObtenerPendientes(usuario);
            }
            catch (Exception ex)
            {
                retorno = -1;
            }
            return retorno;
        }

    }
}
