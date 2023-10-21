using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using Microsoft.Win32;

namespace SIGEES.DataAcces.Helper
{
    public class SqlHelp : IDisposable
    {
        private SqlConnection oConexion;
        private SqlCommand oComando;
        private Boolean disposedValue;


        public SqlHelp(CommandType pCommandType, String pCommandText, System.Data.SqlClient.SqlConnection pObjConexion, SqlTransaction pObjSqlTransaction)
        {
            oComando = new SqlCommand();
            oConexion = pObjConexion;
            oComando.Transaction = pObjSqlTransaction;
            oComando.Connection = pObjConexion;
            oComando.CommandText = pCommandText;
            oComando.CommandType = pCommandType;
        }

        public SqlHelp(CommandType pCommandType, String pCommandText, System.Data.SqlClient.SqlConnection pObjConexion, SqlTransaction pObjSqlTransaction, int pCommandTimeOut)
            : this(pCommandType, pCommandText, pObjConexion, pObjSqlTransaction)
        {
            oComando.CommandTimeout = pCommandTimeOut;
        }


        public int ExecuteNonQuery()
        {
            try
            {
                return oComando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oComando.Dispose();
            }
        }

        public SqlDataReader ExecuteDataReader()
        {
            try
            {
                return oComando.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecuteDataTable()
        {
            try
            {
                SqlDataAdapter oDa = new SqlDataAdapter(oComando);
                DataTable oDt = new DataTable();
                oDa.Fill(oDt);
                return oDt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oComando.Dispose();
            }
        }

        public Object ExecuteScalar()
        {
            try
            {
                return oComando.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oComando.Dispose();
            }
        }

        public void AgregarParametro(String pNombre, SqlDbType pTipo, ParameterDirection pDireccion, object pValor)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = pNombre;
            parametro.SqlDbType = pTipo;
            parametro.Direction = pDireccion;
            parametro.Value = pValor ?? (object)DBNull.Value;
            oComando.Parameters.Add(parametro);
            parametro = null;
        }

        public void AgregarParametro(String pNombre, SqlDbType pTipo, int pTamanio, ParameterDirection pDireccion, object pValor)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = pNombre;
            parametro.SqlDbType = pTipo;
            parametro.Size = pTamanio;
            parametro.Direction = pDireccion;
            parametro.Value = pValor ?? (object)DBNull.Value;
            oComando.Parameters.Add(parametro);
            parametro = null;
        }

        public void AgregarParametro(String pNombre, SqlDbType pTipo, ParameterDirection pDireccion)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = pNombre;
            parametro.SqlDbType = pTipo;
            parametro.Direction = pDireccion;
            oComando.Parameters.Add(parametro);
            parametro = null;
        }

        public Object strObtenerParametro(String nombre)
        {
            return oComando.Parameters[nombre].Value;

        }

        static internal T NullConvertFromDB<T>(object value)
        {
            if (value == null || value is DBNull)
            {
                return (T)(object)null;
            }
            return (T)value;
        }

        static internal object NullConvertToDB<T>(object value)
        {
            return value ?? DBNull.Value;
        }


        //IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (!(oComando == null))
                    {
                        oComando.Dispose();
                    }
                    oComando = null;

                    if (!(oConexion == null))
                    {
                        if (oConexion.State == ConnectionState.Open)
                        {
                            oConexion.Close();
                        }
                        oConexion = null;
                    }
                }
            }
            this.disposedValue = true;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            throw new NotImplementedException();
        }
    }
}
