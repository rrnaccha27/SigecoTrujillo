using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
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
    public partial class TemplateCorreoSelDA : GenericDA<TemplateCorreoSelDA>
    {
        private Database oDatabase = EnterpriseLibraryContainer.Current.GetInstance<Database>(Conexion.cnSIGEES);

        public List<template_correo_dto> ListarParametroa(int codigo_template)
        {
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand("up_template_correo_listar_parametros");
            List<template_correo_dto> lst = new List<template_correo_dto>();

            try
            {
                oDatabase.AddInParameter(oDbCommand, "@p_codigo_template", DbType.Int32, codigo_template);

                using (IDataReader oIDataReader = oDatabase.ExecuteReader(oDbCommand))
                {

                    while (oIDataReader.Read())
                    {
                        var v_entidad = new template_correo_dto();

                        v_entidad.codigo_template = DataUtil.DbValueToDefault<int>(oIDataReader["codigo_template"]);
                        v_entidad.nombre = DataUtil.DbValueToDefault<string>(oIDataReader["nombre"]);
                        v_entidad.indice = DataUtil.DbValueToDefault<int>(oIDataReader["indice"]);
                        v_entidad.parametro = DataUtil.DbValueToDefault<string>(oIDataReader["parametro"]);

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
