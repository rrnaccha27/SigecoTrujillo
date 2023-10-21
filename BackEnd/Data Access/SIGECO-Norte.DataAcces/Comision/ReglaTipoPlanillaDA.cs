using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SIGEES.DataAcces.Helper;
using SIGEES.DataAcces.Helper.Utils;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{
    public class ReglaTipoPlanillaDA : GenericDA<ReglaTipoPlanillaDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<grilla_regla_tipo_planilla_dto> Listar()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_tipo_planilla_listar");
            grilla_regla_tipo_planilla_dto v_entidad;
            List<grilla_regla_tipo_planilla_dto> lst = new List<grilla_regla_tipo_planilla_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new grilla_regla_tipo_planilla_dto();
                    v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                    v_entidad.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    v_entidad.nombre_tipo_planilla = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_tipo_planilla"]);
                    v_entidad.descripcion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]);
                    v_entidad.str_fecha_registra = Fechas.convertDateToString(DataUtil.DbValueToNullable<DateTime>(oIDataReader["fecha_registra"]));
                    v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    v_entidad.str_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);
                    v_entidad.afecto_doc_completa = DataUtil.DbValueToDefault<string>(oIDataReader["afecto_doc_completa"]);
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public regla_tipo_planilla_dto BuscarById(int p_codigo_regla_tipo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_tipo_planilla_obtener_by_id");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_tipo_planilla", DbType.Int32, p_codigo_regla_tipo_planilla);
            regla_tipo_planilla_dto v_entidad=new regla_tipo_planilla_dto();  
            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                if (oIDataReader.Read())
                {
                    v_entidad = new regla_tipo_planilla_dto();
                    v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                    v_entidad.codigo_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_planilla"]);
                    v_entidad.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                    v_entidad.descripcion = DataUtil.DbValueToDefault<string>(oIDataReader["descripcion"]);
                    v_entidad.fecha_registra = DataUtil.DbValueToDefault<DateTime>(oIDataReader["fecha_registra"]);
                    v_entidad.usuario_registra = DataUtil.DbValueToDefault<string>(oIDataReader["usuario_registra"]);
                    v_entidad.estado_registro = DataUtil.DbValueToDefault<bool>(oIDataReader["estado_registro"]);
                    v_entidad.afecto_doc_completa = DataUtil.DbValueToDefault<bool>(oIDataReader["afecto_doc_completa"]);
                    v_entidad.codigo_tipo_reporte = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_tipo_reporte"]);
                    v_entidad.detraccion_contrato = DataUtil.DbValueToDefault<bool>(oIDataReader["detraccion_por_contrato"]);
                    v_entidad.envio_liquidacion = DataUtil.DbValueToDefault<bool>(oIDataReader["envio_liquidacion"]);
                }
            }
            return v_entidad;
        }

        public int Insertar(regla_tipo_planilla_dto v_entidad)
        {
            int codigo_regla_tipo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_tipo_planilla_insertar");

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, v_entidad.nombre);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_planilla", DbType.Int32, v_entidad.codigo_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);
                oDatabase.AddInParameter(oDbCommand, "@p_afecto_doc_completa", DbType.Boolean, v_entidad.afecto_doc_completa);
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_tipo_reporte", DbType.Int32, v_entidad.codigo_tipo_reporte);
                oDatabase.AddInParameter(oDbCommand, "@p_detraccion_por_contrato", DbType.Boolean, v_entidad.detraccion_contrato);
                oDatabase.AddInParameter(oDbCommand, "@p_envio_liquidacion", DbType.Boolean, v_entidad.envio_liquidacion);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_regla_tipo_planilla", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_regla_tipo_planilla").ToString();
                codigo_regla_tipo_planilla = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_regla_tipo_planilla;
        }

        public int InsertarDetalle(grilla_detalle_regla_tipo_planilla_dto v_entidad)
        {
            int codigo_detalle__regla_tipo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_regla_tipo_planilla_insertar");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@codigo_regla_tipo_planilla", DbType.Int32, v_entidad.codigo_regla_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@codigo_canal", DbType.Int32, v_entidad.codigo_canal);
                oDatabase.AddInParameter(oDbCommand, "@codigo_empresa", DbType.String, v_entidad.codigo_empresa);
                oDatabase.AddInParameter(oDbCommand, "@codigo_tipo_venta", DbType.String, v_entidad.codigo_tipo_venta);
                oDatabase.AddInParameter(oDbCommand, "@codigo_campo_santo", DbType.String, v_entidad.codigo_campo_santo);

                oDatabase.AddInParameter(oDbCommand, "@usuario_registra", DbType.String, v_entidad.usuario_registra);
                oDatabase.AddOutParameter(oDbCommand, "@p_codigo_detalle__regla_tipo_planilla", DbType.Int32, 20);

                oDatabase.ExecuteNonQuery(oDbCommand);
                var resultado1 = oDatabase.GetParameterValue(oDbCommand, "@p_codigo_detalle__regla_tipo_planilla").ToString();
                codigo_detalle__regla_tipo_planilla = int.Parse(resultado1);

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle__regla_tipo_planilla;
        }

        public List<grilla_detalle_regla_tipo_planilla_dto> ListarDetalle(int p_codigo_regla_tipo_planilla)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_detalle_regla_tipo_planilla_listar");
            oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_tipo_planilla", DbType.Int32, p_codigo_regla_tipo_planilla);
            grilla_detalle_regla_tipo_planilla_dto v_entidad;
            List<grilla_detalle_regla_tipo_planilla_dto> lst = new List<grilla_detalle_regla_tipo_planilla_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new grilla_detalle_regla_tipo_planilla_dto();
                    v_entidad.codigo_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                    v_entidad.codigo_detalle_regla_tipo_planilla = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_detalle_regla_tipo_planilla"]);
                    v_entidad.codigo_canal = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_canal"]);
                    //v_entidad.codigo_tipo_venta = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_tipo_venta"]);
                    v_entidad.codigo_empresa = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_empresa"]);
                    //v_entidad.codigo_campo_santo = DataUtil.DbValueToDefault<string>(oIDataReader["codigo_campo_santo"]);
                    v_entidad.nombre_estado_registro = DataUtil.DbValueToDefault<string>(oIDataReader["nombre_estado_registro"]);                    
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public int Eliminar(regla_tipo_planilla_dto v_entidad)
        {
            int codigo_detalle__regla_tipo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_tipo_planilla_eliminar");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_tipo_planilla", DbType.Int32, v_entidad.codigo_regla_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);
                

                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_detalle__regla_tipo_planilla = v_entidad.codigo_regla_tipo_planilla;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_detalle__regla_tipo_planilla;
        }

        public List<combo_regla_tipo_planilla_dto> GetAllComboJson()
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_tipo_planilla_combo");
            combo_regla_tipo_planilla_dto v_entidad;
            List<combo_regla_tipo_planilla_dto> lst = new List<combo_regla_tipo_planilla_dto>();

            using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
            {
                while (oIDataReader.Read())
                {
                    v_entidad = new combo_regla_tipo_planilla_dto();
                    v_entidad.id = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_regla_tipo_planilla"]);
                    v_entidad.text= DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);                    
                    lst.Add(v_entidad);
                }
            }
            return lst;
        }

        public int Actualizar(regla_tipo_planilla_dto v_entidad)
        {
            int codigo_regla_tipo_planilla = 0;
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_regla_tipo_planilla_actualizar");
            try
            {

                oDatabase.AddInParameter(oDbCommand, "@p_codigo_regla_tipo_planilla", DbType.Int32, v_entidad.codigo_regla_tipo_planilla);
                oDatabase.AddInParameter(oDbCommand, "@p_nombre", DbType.String, v_entidad.nombre);
                oDatabase.AddInParameter(oDbCommand, "@p_usuario_registra", DbType.String, v_entidad.usuario_registra);


                oDatabase.ExecuteNonQuery(oDbCommand);

                codigo_regla_tipo_planilla = v_entidad.codigo_regla_tipo_planilla;

            }
            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;
            }
            return codigo_regla_tipo_planilla;
        }
    }
}
