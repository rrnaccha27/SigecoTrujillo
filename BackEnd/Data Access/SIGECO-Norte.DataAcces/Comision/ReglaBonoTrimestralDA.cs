using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;

namespace SIGEES.DataAcces
{
    public class ReglaBonoTrimestralDA : GenericDA<ReglaBonoTrimestralDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<grilla_regla_bono_trimestral_dto> Listar()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_listar");
            grilla_regla_bono_trimestral_dto v_entidad;
            List<grilla_regla_bono_trimestral_dto> lst = new List<grilla_regla_bono_trimestral_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new grilla_regla_bono_trimestral_dto();
                    v_entidad.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    v_entidad.descripcion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]);
                    v_entidad.nombre_tipo_bono= DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_bono"]);
                    v_entidad.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                    v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    v_entidad.nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);
                    v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    v_entidad.vigencia = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia"]);
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public regla_bono_trimestral_dto BuscarById(int p_codigo_regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_obtener_by_id");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, p_codigo_regla);
            regla_bono_trimestral_dto v_entidad=new regla_bono_trimestral_dto();  
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    v_entidad = new regla_bono_trimestral_dto();
                    v_entidad.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    v_entidad.codigo_tipo_bono = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_bono"]);
                    v_entidad.nombre_tipo_bono = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_bono"]);
                    v_entidad.descripcion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]);
                    v_entidad.fecha_registra = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_registra"]);
                    v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    v_entidad.fecha_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["fecha_modifica"]);
                    v_entidad.usuario_modifica = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_modifica"]);
                    v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    v_entidad.nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);
                    v_entidad.vigencia_inicio= DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_inicio"]);
                    v_entidad.vigencia_fin = DataUtil.DbValueToDefault<string>(oIDataReader["vigencia_fin"]);
                }
            }
            return v_entidad;
        }

        public int Insertar(regla_bono_trimestral_dto v_entidad)
        {
            int codigo_regla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_descripcion", DbType.String, v_entidad.descripcion);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_bono", DbType.Int32, v_entidad.codigo_tipo_bono);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_inicio", DbType.String, v_entidad.vigencia_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, v_entidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);

                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla").ToString();
                codigo_regla = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            
            return codigo_regla;
        }

        public int InsertarDetalle(grilla_regla_bono_trimestral_detalle_dto v_entidad)
        {
            int codigo_regla_detalle = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_detalle_insertar");
            
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, v_entidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_canal", DbType.Int32, v_entidad.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_empresa", DbType.String, v_entidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_venta", DbType.String, v_entidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_detalle", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_detalle").ToString();
                codigo_regla_detalle = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_regla_detalle;
        }

        public List<grilla_regla_bono_trimestral_detalle_dto> ListarDetalle(int p_codigo_regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_detalle_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, p_codigo_regla);

            grilla_regla_bono_trimestral_detalle_dto v_entidad;
            List<grilla_regla_bono_trimestral_detalle_dto> lst = new List<grilla_regla_bono_trimestral_detalle_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new grilla_regla_bono_trimestral_detalle_dto();
                    v_entidad.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    v_entidad.codigo_regla_detalle = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_detalle"]);
                    v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                    v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_tipo_venta"]);
                    v_entidad.codigo_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_empresa"]);
                    v_entidad.nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public int InsertarMeta(grilla_regla_bono_trimestral_meta_dto v_entidad)
        {
            int codigo_regla_meta = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_meta_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, v_entidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@p_rango_inicio", DbType.Int32, v_entidad.rango_inicio);
                oDatabase.AddInParameter(oDbCommand, "@p_rango_fin", DbType.Int32, v_entidad.rango_fin);
                oDatabase.AddInParameter(oDbCommand, "@p_monto", DbType.Decimal, v_entidad.monto);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_meta", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_meta").ToString();
                codigo_regla_meta = int.Parse(resultado1);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_regla_meta;
        }

        public List<grilla_regla_bono_trimestral_meta_dto> ListarMeta(int p_codigo_regla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_meta_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, p_codigo_regla);

            grilla_regla_bono_trimestral_meta_dto v_entidad;
            List<grilla_regla_bono_trimestral_meta_dto> lst = new List<grilla_regla_bono_trimestral_meta_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new grilla_regla_bono_trimestral_meta_dto();
                    v_entidad.codigo_regla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    v_entidad.codigo_regla_meta = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_meta"]);
                    v_entidad.rango_inicio = DataUtil.DbValueToDefault<int>(oIDataReader["rango_inicio"]);
                    v_entidad.rango_fin = DataUtil.DbValueToDefault<int>(oIDataReader["rango_fin"]);
                    v_entidad.monto = DataUtil.DbValueToDefault<decimal>(oIDataReader["monto"]); 
                    v_entidad.nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public void Eliminar(regla_bono_trimestral_dto v_entidad)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_eliminar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, v_entidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);

                oDatabase.ExecuteNonQuery(oDbCommand);
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

        }

        public List<combo_regla_bono_trimestral_dto> GetAllComboJson()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_combo");
            combo_regla_bono_trimestral_dto v_entidad;
            List<combo_regla_bono_trimestral_dto> lst = new List<combo_regla_bono_trimestral_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new combo_regla_bono_trimestral_dto();
                    v_entidad.id = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla"]);
                    v_entidad.text= DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]);                    
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public int Actualizar(regla_bono_trimestral_dto v_entidad)
        {
            int codigo_regla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_bono_trimestral_actualizar");
            
            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla", DbType.Int32, v_entidad.codigo_regla);
                oDatabase.AddInParameter(oDbCommand, "@p_vigencia_fin", DbType.String, v_entidad.vigencia_fin);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_modifica", DbType.String, v_entidad.usuario_registra);

                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_regla = v_entidad.codigo_regla;
            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }

            return codigo_regla;
        }
    }
}
