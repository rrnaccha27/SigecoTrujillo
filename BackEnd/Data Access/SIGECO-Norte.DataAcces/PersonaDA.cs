using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
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
    public partial class PersonaDA : BaseDA<persona_dto>
    {
        public MensajeDTO Insertar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }

        public MensajeDTO Actualizar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }

        public MensajeDTO Eliminar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }
    }
    
     
    public partial class PersonaSelDA :SingletonDA<PersonaSelDA>, BaseSelDA<persona_dto>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(SIGEES.DataAcces.Helper.Conexion.cnSIGEES);
        public persona_dto BuscarById(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }

        public List<persona_dto> Listar()
        {
            string sql = "SELECT U.codigo_usuario,P.codigo_persona,P.apellido_materno, "
+"P.apellido_paterno,P.nombre_persona,P.numero_documento FROM usuario U INNER JOIN persona P ON  U.codigo_persona=P.codigo_persona WHERE P.estado_registro=1";



            
            DbCommand oDbCommand = oDatabase.GetSqlStringCommand(sql);
            var lst = new List<persona_dto>();
           
            try
            {
               
   

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    Int32 icodigo_usuario = oIDataReader.GetOrdinal("codigo_usuario");
                    Int32 icodigo_persona = oIDataReader.GetOrdinal("codigo_persona");
                    Int32 iapellido_materno = oIDataReader.GetOrdinal("apellido_materno");
                    Int32 iapellido_paterno = oIDataReader.GetOrdinal("apellido_paterno");
                    Int32 inombre_persona = oIDataReader.GetOrdinal("nombre_persona");
                    Int32 inumero_documento = oIDataReader.GetOrdinal("numero_documento");
                    
                    while (oIDataReader.Read())
                    {
                        persona_dto v_entidad = new persona_dto();
                        v_entidad.codigo_persona = DataUtil.DbValueToDefault<Int32>(oIDataReader[icodigo_persona]);
                        v_entidad.codigo_usuario = DataUtil.DbValueToDefault<string>(oIDataReader[icodigo_usuario]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader[iapellido_materno]);
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader[iapellido_paterno]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader[inombre_persona]);
                        v_entidad.numero_documento = DataUtil.DbValueToDefault<string>(oIDataReader[inumero_documento]);
                        lst.Add(v_entidad);
                    }

                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }

        public List<persona_dto> ListarVendedor()
        {
            string sql = "SELECT U.codigo_usuario,P.codigo_persona,P.apellido_materno, "
+ "P.apellido_paterno,P.nombre_persona,P.numero_documento FROM usuario U INNER JOIN persona P ON  U.codigo_persona=P.codigo_persona WHERE P.estado_registro=1";




            DbCommand oDbCommand = oDatabase.GetSqlStringCommand(sql);
            var lst = new List<persona_dto>();

            try
            {



                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    Int32 icodigo_usuario = oIDataReader.GetOrdinal("codigo_usuario");
                    Int32 icodigo_persona = oIDataReader.GetOrdinal("codigo_persona");
                    Int32 iapellido_materno = oIDataReader.GetOrdinal("apellido_materno");
                    Int32 iapellido_paterno = oIDataReader.GetOrdinal("apellido_paterno");
                    Int32 inombre_persona = oIDataReader.GetOrdinal("nombre_persona");
                    Int32 inumero_documento = oIDataReader.GetOrdinal("numero_documento");

                    while (oIDataReader.Read())
                    {
                        persona_dto v_entidad = new persona_dto();
                        v_entidad.codigo_persona = DataUtil.DbValueToDefault<Int32>(oIDataReader[icodigo_persona]);
                        v_entidad.codigo_usuario = DataUtil.DbValueToDefault<string>(oIDataReader[icodigo_usuario]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader[iapellido_materno]);
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader[iapellido_paterno]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader[inombre_persona]);
                        v_entidad.numero_documento = DataUtil.DbValueToDefault<string>(oIDataReader[inumero_documento]);
                        lst.Add(v_entidad);
                    }

                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }

        public List<persona_dto> Buscar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }

        public List<persona_dto> ListarCliente()
        {
            string sql = "select "
                        + " p.codigo_persona,"
                        +" c.codigo_cliente,"
                        +" p.apellido_paterno,"
                        +" p.apellido_materno,"
                        +" p.nombre_persona ,"
                        +" p.numero_documento"
+" from PERSONA p INNER JOIN cliente c ON  p.codigo_persona=c.codigo_persona"
+" where p.estado_registro=1 and c.estado_registro=1";




            DbCommand oDbCommand = oDatabase.GetSqlStringCommand(sql);
            var lst = new List<persona_dto>();

            try
            {



                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {
                    Int32 icodigo_cliente = oIDataReader.GetOrdinal("codigo_cliente");
                    Int32 icodigo_persona = oIDataReader.GetOrdinal("codigo_persona");
                    Int32 iapellido_materno = oIDataReader.GetOrdinal("apellido_materno");
                    Int32 iapellido_paterno = oIDataReader.GetOrdinal("apellido_paterno");
                    Int32 inombre_persona = oIDataReader.GetOrdinal("nombre_persona");
                    Int32 inumero_documento = oIDataReader.GetOrdinal("numero_documento");

                    while (oIDataReader.Read())
                    {
                        persona_dto v_entidad = new persona_dto();
                        v_entidad.codigo_persona = DataUtil.DbValueToDefault<Int32>(oIDataReader[icodigo_persona]);
                        v_entidad.codigo_cliente = DataUtil.DbValueToDefault<string>(oIDataReader[icodigo_cliente]);
                        v_entidad.apellido_materno = DataUtil.DbValueToDefault<string>(oIDataReader[iapellido_materno]);
                        v_entidad.apellido_paterno = DataUtil.DbValueToDefault<string>(oIDataReader[iapellido_paterno]);
                        v_entidad.nombre_persona = DataUtil.DbValueToDefault<string>(oIDataReader[inombre_persona]);
                        v_entidad.numero_documento = DataUtil.DbValueToDefault<string>(oIDataReader[inumero_documento]);
                        lst.Add(v_entidad);
                    }

                }


            }

            finally
            {
                if (oDbCommand != null) oDbCommand.Dispose();
                oDbCommand = null;

            }
            return lst;
        }
    }
}
