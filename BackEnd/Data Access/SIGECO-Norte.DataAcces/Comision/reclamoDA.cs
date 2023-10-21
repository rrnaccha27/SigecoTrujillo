using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SIGEES.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using SIGEES.DataAcces.Helper;
using System.Data.Common;

namespace SIGEES.DataAcces
{
    public class reclamoDA
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);
        public string Registrar(reclamo_dto oBe)
        {
            string res = "";
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_INSERTAR", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_personal", SqlDbType.Int).Value = oBe.codigo_personal;
            cmd.Parameters.Add("@NroContrato", SqlDbType.VarChar, 20).Value = oBe.NroContrato;
            cmd.Parameters.Add("@codigo_articulo", SqlDbType.Int).Value = oBe.codigo_articulo;
            cmd.Parameters.Add("@codigo_empresa", SqlDbType.Int).Value = oBe.codigo_empresa;
            cmd.Parameters.Add("@Cuota", SqlDbType.Int).Value = oBe.Cuota;
            cmd.Parameters.Add("@Importe", SqlDbType.Decimal, 18).Value = oBe.Importe;
            cmd.Parameters.Add("@codigo_estado_reclamo", SqlDbType.Int).Value = oBe.codigo_estado_reclamo;
            cmd.Parameters.Add("@codigo_estado_resultado", SqlDbType.Int).Value = oBe.codigo_estado_resultado;
            cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 1000).Value = oBe.Observacion;
            cmd.Parameters.Add("@Respuesta", SqlDbType.VarChar, 1000).Value = oBe.Respuesta;
            cmd.Parameters.Add("@usuario_registra", SqlDbType.VarChar, 50).Value = oBe.usuario_registra;
            cmd.Parameters.Add("@fecha_registra", SqlDbType.DateTime).Value = oBe.fecha_registra;
            cmd.Parameters.Add("@es_contrato_migrado", SqlDbType.Bit).Value = oBe.es_contrato_migrado;
            try
            {
                oConexion.Open();
                res = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                res = "ERROR|NO SE PUDO GUARDAR";
            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return res;
        }

        public void Actualizar(reclamo_dto oBe)
        {
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_actualizar", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_reclamo", SqlDbType.Int).Value = oBe.codigo_reclamo;
            cmd.Parameters.Add("@codigo_personal", SqlDbType.Int).Value = oBe.codigo_personal;
            cmd.Parameters.Add("@NroContrato", SqlDbType.VarChar, 20).Value = oBe.NroContrato;

            cmd.Parameters.Add("@codigo_articulo", SqlDbType.Int).Value = oBe.codigo_articulo;
            cmd.Parameters.Add("@codigo_empresa", SqlDbType.Int).Value = oBe.codigo_empresa;
            cmd.Parameters.Add("@Cuota", SqlDbType.Int).Value = oBe.Cuota;
            cmd.Parameters.Add("@Importe", SqlDbType.Decimal, 18).Value = oBe.Importe;

            cmd.Parameters.Add("@atencion_codigo_articulo", SqlDbType.Int).Value = oBe.atencion_codigo_articulo;
            cmd.Parameters.Add("@atencion_codigo_empresa", SqlDbType.Int).Value = oBe.atencion_codigo_empresa;
            cmd.Parameters.Add("@atencion_Cuota", SqlDbType.Int).Value = oBe.atencion_Cuota;
            cmd.Parameters.Add("@atencion_Importe", SqlDbType.Decimal, 18).Value = oBe.atencion_Importe;

            cmd.Parameters.Add("@codigo_estado_reclamo", SqlDbType.Int).Value = oBe.codigo_estado_reclamo;
            cmd.Parameters.Add("@codigo_estado_resultado", SqlDbType.Int).Value = oBe.codigo_estado_resultado;
            cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 1000).Value = oBe.Observacion;
            cmd.Parameters.Add("@Respuesta", SqlDbType.VarChar, 1000).Value = oBe.Respuesta;
            cmd.Parameters.Add("@usuario_modifica", SqlDbType.VarChar, 50).Value = oBe.usuario_modifica;
            cmd.Parameters.Add("@fecha_modifica", SqlDbType.DateTime).Value = oBe.fecha_modifica;
            cmd.Parameters.Add("@TipoAfectaPlanilla", SqlDbType.VarChar, 20).Value = oBe.TipoAfectaPlanilla;
            try
            {
                oConexion.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
        }

        public int Eliminar(Int32 codigo_reclamo)
        {
            int res = 0;
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_ELIMINAR", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_reclamo", SqlDbType.Int).Value = codigo_reclamo;
            try
            {
                oConexion.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return res;
        }

        public reclamo_dto GetReg(Int32 codigo_reclamo)
        {
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_GETREG", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_reclamo", SqlDbType.Int).Value = codigo_reclamo;
            reclamo_dto entidad = null;
            try
            {
                oConexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    entidad = new reclamo_dto();

                    int icodigo_reclamo = reader.GetOrdinal("codigo_reclamo");
                    if (!reader.IsDBNull(icodigo_reclamo)) entidad.codigo_reclamo = reader.GetInt32(icodigo_reclamo);

                    int icodigo_personal = reader.GetOrdinal("codigo_personal");
                    if (!reader.IsDBNull(icodigo_personal)) entidad.codigo_personal = reader.GetInt32(icodigo_personal);

                    int iPersonalVentas = reader.GetOrdinal("PersonalVentas");
                    if (!reader.IsDBNull(iPersonalVentas)) entidad.PersonalVentas = reader.GetString(iPersonalVentas);

                    int iNroContrato = reader.GetOrdinal("NroContrato");
                    if (!reader.IsDBNull(iNroContrato)) entidad.NroContrato = reader.GetString(iNroContrato);

                    int icodigo_articulo = reader.GetOrdinal("codigo_articulo");
                    if (!reader.IsDBNull(icodigo_articulo)) entidad.codigo_articulo = reader.GetInt32(icodigo_articulo);

                    int iArticulo = reader.GetOrdinal("Articulo");
                    if (!reader.IsDBNull(iArticulo)) entidad.Articulo = reader.GetString(iArticulo);

                    int icodigo_empresa = reader.GetOrdinal("codigo_empresa");
                    if (!reader.IsDBNull(icodigo_empresa)) entidad.codigo_empresa = reader.GetInt32(icodigo_empresa);

                    int iCuota = reader.GetOrdinal("Cuota");
                    if (!reader.IsDBNull(iCuota)) entidad.Cuota = reader.GetInt32(iCuota);

                    int iImporte = reader.GetOrdinal("Importe");
                    if (!reader.IsDBNull(iImporte)) entidad.Importe = reader.GetDecimal(iImporte);

                    int iatencion_codigo_articulo = reader.GetOrdinal("atencion_codigo_articulo");
                    if (!reader.IsDBNull(iatencion_codigo_articulo)) entidad.atencion_codigo_articulo = reader.GetInt32(iatencion_codigo_articulo);

                    int iatencion_Articulo = reader.GetOrdinal("atencion_Articulo");
                    if (!reader.IsDBNull(iatencion_Articulo)) entidad.atencion_Articulo = reader.GetString(iatencion_Articulo);

                    int iatencion_codigo_empresa = reader.GetOrdinal("atencion_codigo_empresa");
                    if (!reader.IsDBNull(iatencion_codigo_empresa)) entidad.atencion_codigo_empresa = reader.GetInt32(iatencion_codigo_empresa);

                    int iatencion_Cuota = reader.GetOrdinal("atencion_Cuota");
                    if (!reader.IsDBNull(iatencion_Cuota)) entidad.atencion_Cuota = reader.GetInt32(iatencion_Cuota);

                    int iatencion_Importe = reader.GetOrdinal("atencion_Importe");
                    if (!reader.IsDBNull(iatencion_Importe)) entidad.atencion_Importe = reader.GetDecimal(iatencion_Importe);

                    int icodigo_estado_reclamo = reader.GetOrdinal("codigo_estado_reclamo");
                    if (!reader.IsDBNull(icodigo_estado_reclamo)) entidad.codigo_estado_reclamo = reader.GetInt32(icodigo_estado_reclamo);

                    int icodigo_estado_resultado = reader.GetOrdinal("codigo_estado_resultado");
                    if (!reader.IsDBNull(icodigo_estado_resultado)) entidad.codigo_estado_resultado = reader.GetInt32(icodigo_estado_resultado);

                    int iObservacion = reader.GetOrdinal("Observacion");
                    if (!reader.IsDBNull(iObservacion)) entidad.Observacion = reader.GetString(iObservacion);

                    int iRespuesta = reader.GetOrdinal("Respuesta");
                    if (!reader.IsDBNull(iRespuesta)) entidad.Respuesta = reader.GetString(iRespuesta);


                    int icodigo_planilla = reader.GetOrdinal("codigo_planilla");
                    if (!reader.IsDBNull(icodigo_planilla)) entidad.codigo_planilla = reader.GetInt32(icodigo_planilla);

                    int inumero_planilla = reader.GetOrdinal("numero_planilla");
                    if (!reader.IsDBNull(inumero_planilla)) entidad.numero_planilla = reader.GetString(inumero_planilla);


                    int iusuario_registra = reader.GetOrdinal("usuario_registra");
                    if (!reader.IsDBNull(iusuario_registra)) entidad.usuario_registra = reader.GetString(iusuario_registra);

                    int ifecha_registra = reader.GetOrdinal("fecha_registra");
                    if (!reader.IsDBNull(ifecha_registra)) entidad.fecha_registra = reader.GetDateTime(ifecha_registra);

                    int iusuario_modifica = reader.GetOrdinal("usuario_modifica");
                    if (!reader.IsDBNull(iusuario_modifica)) entidad.usuario_modifica = reader.GetString(iusuario_modifica);

                    int ifecha_modifica = reader.GetOrdinal("fecha_modifica");
                    if (!reader.IsDBNull(ifecha_modifica)) entidad.fecha_modifica = reader.GetDateTime(ifecha_modifica);

                    int iUsuarioAtencion = reader.GetOrdinal("UsuarioAtencion");
                    if (!reader.IsDBNull(iUsuarioAtencion)) entidad.UsuarioAtencion = reader.GetString(iUsuarioAtencion);

                    int iFechaAtencion = reader.GetOrdinal("FechaAtencion");
                    if (!reader.IsDBNull(iFechaAtencion)) entidad.FechaAtencion = reader.GetString(iFechaAtencion);

                    int ies_contrato_migrado = reader.GetOrdinal("es_contrato_migrado");
                    if (!reader.IsDBNull(ies_contrato_migrado)) entidad.es_contrato_migrado = reader.GetInt32(ies_contrato_migrado);

                    int iFechaRegistra = reader.GetOrdinal("FechaRegistra");
                    if (!reader.IsDBNull(iUsuarioAtencion)) entidad.FechaRegistra = reader.GetString(iFechaRegistra);

                    int iUsuarioRegistra = reader.GetOrdinal("UsuarioRegistra");
                    if (!reader.IsDBNull(iUsuarioRegistra)) entidad.UsuarioRegistra = reader.GetString(iUsuarioRegistra);

                    int inombre_estado_reclamo = reader.GetOrdinal("nombre_estado_reclamo");
                    if (!reader.IsDBNull(inombre_estado_reclamo)) entidad.nombre_estado_reclamo = reader.GetString(inombre_estado_reclamo);

                    int inombre_empresa = reader.GetOrdinal("nombre_empresa");
                    if (!reader.IsDBNull(inombre_empresa)) entidad.nombre_empresa = reader.GetString(inombre_empresa);

                    int inombre_estado_resultado_n1 = reader.GetOrdinal("nombre_estado_resultado_n1");
                    if (!reader.IsDBNull(inombre_estado_resultado_n1)) entidad.nombre_estado_resultado_n1 = reader.GetString(inombre_estado_resultado_n1);

                    int iobservacion_n1 = reader.GetOrdinal("observacion_n1");
                    if (!reader.IsDBNull(iobservacion_n1)) entidad.observacion_n1 = reader.GetString(iobservacion_n1);

                    int iusuario_n1 = reader.GetOrdinal("usuario_n1");
                    if (!reader.IsDBNull(iusuario_n1)) entidad.usuario_n1 = reader.GetString(iusuario_n1);

                    int ifecha_n1 = reader.GetOrdinal("fecha_n1");
                    if (!reader.IsDBNull(ifecha_n1)) entidad.fecha_n1 = reader.GetString(ifecha_n1);

                    int inombre_estado_resultado_n2 = reader.GetOrdinal("nombre_estado_resultado_n2");
                    if (!reader.IsDBNull(inombre_estado_resultado_n2)) entidad.nombre_estado_resultado_n2 = reader.GetString(inombre_estado_resultado_n2);

                    int iobservacion_n2 = reader.GetOrdinal("observacion_n2");
                    if (!reader.IsDBNull(iobservacion_n2)) entidad.observacion_n2 = reader.GetString(iobservacion_n2);

                    int iusuario_n2 = reader.GetOrdinal("usuario_n2");
                    if (!reader.IsDBNull(iusuario_n2)) entidad.usuario_n2 = reader.GetString(iusuario_n2);

                    int ifecha_n2 = reader.GetOrdinal("fecha_n2");
                    if (!reader.IsDBNull(ifecha_n2)) entidad.fecha_n2 = reader.GetString(ifecha_n2);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return entidad;
        }

        //public List<reclamo_dto> ListarAll(String NroContrato, Int32 codigo_estado_reclamo)
        //{
        //    SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
        //    SqlCommand cmd = new SqlCommand("up_reclamo_LISTAR_All", oConexion);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@NroContrato", SqlDbType.VarChar, 20).Value = NroContrato;
        //    cmd.Parameters.Add("@codigo_estado_reclamo", SqlDbType.Int).Value = codigo_estado_reclamo;
        //    List<reclamo_dto> lista = new List<reclamo_dto>();
        //    try
        //    {
        //        oConexion.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            reclamo_dto entidad = new reclamo_dto();

        //            int icodigo_reclamo = reader.GetOrdinal("codigo_reclamo");
        //            if (!reader.IsDBNull(icodigo_reclamo)) entidad.codigo_reclamo = reader.GetInt32(icodigo_reclamo);

        //            int icodigo_personal = reader.GetOrdinal("codigo_personal");
        //            if (!reader.IsDBNull(icodigo_personal)) entidad.codigo_personal = reader.GetInt32(icodigo_personal);

        //            int iNroContrato = reader.GetOrdinal("NroContrato");
        //            if (!reader.IsDBNull(iNroContrato)) entidad.NroContrato = reader.GetString(iNroContrato);

        //            int icodigo_articulo = reader.GetOrdinal("codigo_articulo");
        //            if (!reader.IsDBNull(icodigo_articulo)) entidad.codigo_articulo = reader.GetInt32(icodigo_articulo);

        //            int icodigo_empresa = reader.GetOrdinal("codigo_empresa");
        //            if (!reader.IsDBNull(icodigo_empresa)) entidad.codigo_empresa = reader.GetInt32(icodigo_empresa);

        //            int iCuota = reader.GetOrdinal("Cuota");
        //            if (!reader.IsDBNull(iCuota)) entidad.Cuota = reader.GetInt32(iCuota);

        //            int iImporte = reader.GetOrdinal("Importe");
        //            if (!reader.IsDBNull(iImporte)) entidad.Importe = reader.GetDecimal(iImporte);

        //            int icodigo_estado_reclamo = reader.GetOrdinal("codigo_estado_reclamo");
        //            if (!reader.IsDBNull(icodigo_estado_reclamo)) entidad.codigo_estado_reclamo = reader.GetInt32(icodigo_estado_reclamo);

        //            int icodigo_estado_resultado = reader.GetOrdinal("codigo_estado_resultado");
        //            if (!reader.IsDBNull(icodigo_estado_resultado)) entidad.codigo_estado_resultado = reader.GetInt32(icodigo_estado_resultado);

        //            int iObservacion = reader.GetOrdinal("Observacion");
        //            if (!reader.IsDBNull(iObservacion)) entidad.Observacion = reader.GetString(iObservacion);

        //            int iRespuesta = reader.GetOrdinal("Respuesta");
        //            if (!reader.IsDBNull(iRespuesta)) entidad.Respuesta = reader.GetString(iRespuesta);

        //            int iusuario_registra = reader.GetOrdinal("usuario_registra");
        //            if (!reader.IsDBNull(iusuario_registra)) entidad.usuario_registra = reader.GetString(iusuario_registra);

        //            int ifecha_registra = reader.GetOrdinal("fecha_registra");
        //            if (!reader.IsDBNull(ifecha_registra)) entidad.fecha_registra = reader.GetDateTime(ifecha_registra);

        //            int iusuario_modifica = reader.GetOrdinal("usuario_modifica");
        //            if (!reader.IsDBNull(iusuario_modifica)) entidad.usuario_modifica = reader.GetString(iusuario_modifica);

        //            int ifecha_modifica = reader.GetOrdinal("fecha_modifica");
        //            if (!reader.IsDBNull(ifecha_modifica)) entidad.fecha_modifica = reader.GetDateTime(ifecha_modifica);

        //            int ierror_contrato_migrado = reader.GetOrdinal("error_contrato_migrado");
        //            if (!reader.IsDBNull(ierror_contrato_migrado)) entidad.error_contrato_migrado = reader.GetString(ierror_contrato_migrado);
                    
        //            lista.Add(entidad);
        //            entidad = null;
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        if (oConexion.State == ConnectionState.Open) oConexion.Close();
        //        cmd.Dispose();
        //        oConexion.Dispose();
        //    }
        //    return lista;
        //}

        public List<reclamo_dto> Listar(reclamo_busqueda_dto oBE)
        {
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_LISTAR", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@NroContrato", SqlDbType.VarChar, 20).Value = oBE.nro_contrato;
            cmd.Parameters.Add("@PersonalVentas", SqlDbType.VarChar, 20).Value = oBE.personal_ventas;
            cmd.Parameters.Add("@codigo_estado_reclamo", SqlDbType.Int).Value = oBE.codigo_estado;
            cmd.Parameters.Add("@p_codigo_perfil", SqlDbType.Int).Value = oBE.codigo_perfil;
            cmd.Parameters.Add("@p_codigo_usuario", SqlDbType.VarChar).Value = oBE.usuario;

            List<reclamo_dto> lista = new List<reclamo_dto>();
            try
            {
                oConexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reclamo_dto entidad = new reclamo_dto();

                    int icodigo_reclamo = reader.GetOrdinal("codigo_reclamo");
                    if (!reader.IsDBNull(icodigo_reclamo)) entidad.codigo_reclamo = reader.GetInt32(icodigo_reclamo);

                    int icodigo_personal = reader.GetOrdinal("codigo_personal");
                    if (!reader.IsDBNull(icodigo_personal)) entidad.codigo_personal = reader.GetInt32(icodigo_personal);

                    int iPersonalVentas = reader.GetOrdinal("PersonalVentas");
                    if (!reader.IsDBNull(iPersonalVentas)) entidad.PersonalVentas = reader.GetString(iPersonalVentas);

                    int iNroContrato = reader.GetOrdinal("NroContrato");
                    if (!reader.IsDBNull(iNroContrato)) entidad.NroContrato = reader.GetString(iNroContrato);

                    int icodigo_articulo = reader.GetOrdinal("codigo_articulo");
                    if (!reader.IsDBNull(icodigo_articulo)) entidad.codigo_articulo = reader.GetInt32(icodigo_articulo);

                    int iArticulo = reader.GetOrdinal("Articulo");
                    if (!reader.IsDBNull(iArticulo)) entidad.Articulo = reader.GetString(iArticulo);

                    int icodigo_empresa = reader.GetOrdinal("codigo_empresa");
                    if (!reader.IsDBNull(icodigo_empresa)) entidad.codigo_empresa = reader.GetInt32(icodigo_empresa);

                    int iEmpresa = reader.GetOrdinal("Empresa");
                    if (!reader.IsDBNull(iEmpresa)) entidad.Empresa = reader.GetString(iEmpresa);

                    int iCuota = reader.GetOrdinal("Cuota");
                    if (!reader.IsDBNull(iCuota)) entidad.Cuota = reader.GetInt32(iCuota);

                    int iImporte = reader.GetOrdinal("Importe");
                    if (!reader.IsDBNull(iImporte)) entidad.Importe = reader.GetDecimal(iImporte);



                    int iatencion_codigo_articulo = reader.GetOrdinal("atencion_codigo_articulo");
                    if (!reader.IsDBNull(iatencion_codigo_articulo)) entidad.atencion_codigo_articulo = reader.GetInt32(iatencion_codigo_articulo);

                    int iatencion_Articulo = reader.GetOrdinal("atencion_Articulo");
                    if (!reader.IsDBNull(iatencion_Articulo)) entidad.atencion_Articulo = reader.GetString(iatencion_Articulo);

                    int iatencion_codigo_empresa = reader.GetOrdinal("atencion_codigo_empresa");
                    if (!reader.IsDBNull(iatencion_codigo_empresa)) entidad.atencion_codigo_empresa = reader.GetInt32(iatencion_codigo_empresa);

                    int iatencion_Empresa = reader.GetOrdinal("atencion_Empresa");
                    if (!reader.IsDBNull(iatencion_Empresa)) entidad.atencion_Empresa = reader.GetString(iatencion_Empresa);

                    int iatencion_Cuota = reader.GetOrdinal("atencion_Cuota");
                    if (!reader.IsDBNull(iatencion_Cuota)) entidad.atencion_Cuota = reader.GetInt32(iatencion_Cuota);

                    int iatencion_Importe = reader.GetOrdinal("atencion_Importe");
                    if (!reader.IsDBNull(iatencion_Importe)) entidad.atencion_Importe = reader.GetDecimal(iatencion_Importe);


                    int icodigo_estado_reclamo = reader.GetOrdinal("codigo_estado_reclamo");
                    if (!reader.IsDBNull(icodigo_estado_reclamo)) entidad.codigo_estado_reclamo = reader.GetInt32(icodigo_estado_reclamo);

                    int iEstado = reader.GetOrdinal("Estado");
                    if (!reader.IsDBNull(iEstado)) entidad.Estado = reader.GetString(iEstado);

                    int icodigo_estado_resultado = reader.GetOrdinal("codigo_estado_resultado");
                    if (!reader.IsDBNull(icodigo_estado_resultado)) entidad.codigo_estado_resultado = reader.GetInt32(icodigo_estado_resultado);

                    int iResultado = reader.GetOrdinal("Resultado");
                    if (!reader.IsDBNull(iResultado)) entidad.Resultado = reader.GetString(iResultado);

                    int iObservacion = reader.GetOrdinal("Observacion");
                    if (!reader.IsDBNull(iObservacion)) entidad.Observacion = reader.GetString(iObservacion);

                    int iRespuesta = reader.GetOrdinal("Respuesta");
                    if (!reader.IsDBNull(iRespuesta)) entidad.Respuesta = reader.GetString(iRespuesta);

                    int icodigo_planilla = reader.GetOrdinal("codigo_planilla");
                    if (!reader.IsDBNull(icodigo_planilla)) entidad.codigo_planilla = reader.GetInt32(icodigo_planilla);

                    int inumero_planilla = reader.GetOrdinal("numero_planilla");
                    if (!reader.IsDBNull(inumero_planilla)) entidad.numero_planilla = reader.GetString(inumero_planilla);

                    int iusuario_registra = reader.GetOrdinal("usuario_registra");
                    if (!reader.IsDBNull(iusuario_registra)) entidad.usuario_registra = reader.GetString(iusuario_registra);

                    int ifecha_registra = reader.GetOrdinal("fecha_registra");
                    if (!reader.IsDBNull(ifecha_registra)) entidad.fecha_registra = reader.GetDateTime(ifecha_registra);

                    int iusuario_modifica = reader.GetOrdinal("usuario_modifica");
                    if (!reader.IsDBNull(iusuario_modifica)) entidad.usuario_modifica = reader.GetString(iusuario_modifica);

                    int ifecha_modifica = reader.GetOrdinal("fecha_modifica");
                    if (!reader.IsDBNull(ifecha_modifica)) entidad.fecha_modifica = reader.GetDateTime(ifecha_modifica);

                    int ierror_contrato_migrado = reader.GetOrdinal("error_contrato_migrado");
                    if (!reader.IsDBNull(ierror_contrato_migrado)) entidad.error_contrato_migrado = reader.GetString(ierror_contrato_migrado);

                    int icodigo_estado_resultado_n1 = reader.GetOrdinal("codigo_estado_resultado_n1");
                    if (!reader.IsDBNull(icodigo_estado_resultado_n1)) entidad.codigo_estado_resultado_n1 = reader.GetInt32(icodigo_estado_resultado_n1);

                    int icodigo_estado_resultado_n2 = reader.GetOrdinal("codigo_estado_resultado_n2");
                    if (!reader.IsDBNull(icodigo_estado_resultado_n2)) entidad.codigo_estado_resultado_n2 = reader.GetInt32(icodigo_estado_resultado_n2);

                    int inombre_estado_resultado_n1 = reader.GetOrdinal("nombre_estado_resultado_n1");
                    if (!reader.IsDBNull(inombre_estado_resultado_n1)) entidad.nombre_estado_resultado_n1 = reader.GetString(inombre_estado_resultado_n1);

                    int inombre_estado_resultado_n2 = reader.GetOrdinal("nombre_estado_resultado_n2");
                    if (!reader.IsDBNull(inombre_estado_resultado_n2)) entidad.nombre_estado_resultado_n2 = reader.GetString(inombre_estado_resultado_n2);

                    int iestilo = reader.GetOrdinal("estilo");
                    if (!reader.IsDBNull(iestilo)) entidad.estilo = reader.GetString(iestilo);

                    lista.Add(entidad);
                    entidad = null;
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return lista;
        }

        public string ValidarRegistro(reclamo_dto oBe)
        {
            string res = "";
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_ValidaInsert", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_personal", SqlDbType.Int).Value = oBe.codigo_personal;
            cmd.Parameters.Add("@NroContrato", SqlDbType.VarChar, 20).Value = oBe.NroContrato;
            cmd.Parameters.Add("@codigo_articulo", SqlDbType.Int).Value = oBe.codigo_articulo;
            cmd.Parameters.Add("@codigo_empresa", SqlDbType.Int).Value = oBe.codigo_empresa;
            cmd.Parameters.Add("@Cuota", SqlDbType.Int).Value = oBe.Cuota;
            cmd.Parameters.Add("@Importe", SqlDbType.Decimal, 18).Value = oBe.Importe;
            try
            {
                oConexion.Open();
                res = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                res = "ERROR|NO SE PUDO GUARDAR";
            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return res;
        }
        public int ValidarContrato(int codigo_empresa, string nro_contrato, int personal)
        {
            int res = 0;
            DbCommand cmd = oDatabase.GetStoredProcCommand("up_reclamo_ValidaContrato");
            try
            {
                oDatabase.AddInParameter(cmd, "@codigo_empresa", DbType.Int32, codigo_empresa);
                oDatabase.AddInParameter(cmd, "@nro_contrato", DbType.String, nro_contrato);
                oDatabase.AddInParameter(cmd, "@p_codigo_personal", DbType.Int32, personal);

                res = (int)oDatabase.ExecuteScalar(cmd);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                cmd = null;
            }
            return res;
        }

        public string ValidarPago(reclamo_dto oBe)
        {
            string res = "";
            SqlConnection oConexion = new SqlConnection(ConexionData.SIGEES());
            SqlCommand cmd = new SqlCommand("up_reclamo_ValidaPago", oConexion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@codigo_personal", SqlDbType.Int).Value = oBe.codigo_personal;
            cmd.Parameters.Add("@nro_contrato", SqlDbType.VarChar, 20).Value = oBe.NroContrato;
            cmd.Parameters.Add("@codigo_empresa", SqlDbType.VarChar, 20).Value = oBe.atencion_codigo_empresa;
            cmd.Parameters.Add("@codigo_articulo", SqlDbType.Int).Value = oBe.atencion_codigo_articulo;
            cmd.Parameters.Add("@nro_cuota", SqlDbType.Int).Value = oBe.atencion_Cuota;
            cmd.Parameters.Add("@Importe", SqlDbType.Decimal, 18).Value = oBe.atencion_Importe;
            try
            {
                oConexion.Open();
                res = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oConexion.State == ConnectionState.Open) oConexion.Close();
                cmd.Dispose();
                oConexion.Dispose();
            }
            return res;
        }

        public reclamo_estado_contrato_dto EstadoContrato(int codigo_empresa, string nro_contrato)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reclamo_estado_contrato");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.Int32, codigo_empresa);
            oDatabase.AddInParameter(oDbCommand, "@p_nro_contrato", DbType.String, nro_contrato);

            reclamo_estado_contrato_dto contrato = new reclamo_estado_contrato_dto();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    contrato = new reclamo_estado_contrato_dto();
                    contrato.codigo_estado_proceso = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_estado_proceso"]);
                    contrato.nombre_estado_proceso = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_proceso"]);
                    contrato.observacion = DataUtil.DbValueToDefault<string>(oIDataReader["observacion"]);
                }
            }
            return contrato;
        }

        public bool AtenderContratoMigrado(reclamo_dto reclamo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reclamo_atencion_contrato_migrado");
            bool retorno = false;
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_reclamo", DbType.Int32, reclamo.codigo_reclamo);
                oDatabase.AddInParameter(oDbCommand, "@p_respuesta", DbType.String, reclamo.Respuesta);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, reclamo.usuario_modifica);

                reclamo_estado_contrato_dto contrato = new reclamo_estado_contrato_dto();

                oDatabase.ExecuteNonQuery(oDbCommand);
                retorno = true;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }

            return retorno;
        }

        public void AtenderN1(reclamo_atencion_n1_dto reclamo)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reclamo_atencion_n1");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_reclamo", DbType.Int32, reclamo.codigo_reclamo);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_estado_resultado", DbType.String, reclamo.codigo_estado_resultado);
                oDatabase.AddInParameter(oDbCommand, "@p_observacion", DbType.String, reclamo.observacion);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, reclamo.usuario);
    
                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return;
        }

        public int ObtenerPendientes(string usuario)
        {
            int retorno = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_reclamo_obtener_pendientes");
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_usuario", DbType.String, usuario);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    if (oIDataReader.Read())
                    {
                        retorno = DataUtil.DbValueToDefault<int>(oIDataReader["cantidad_pendientes"]);
                    }
                }
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return retorno;
        }

    }
}
