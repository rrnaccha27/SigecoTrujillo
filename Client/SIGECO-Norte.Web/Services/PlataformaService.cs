using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using SIGEES.Web.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    
    public class PlataformaService : IPlataformaService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<plataforma> _repository;
        public PlataformaService()
        {
            this._repository = new DataRepository<plataforma>(dbContext);
        }
        public string GetAllJson(bool isReadAll = false)
        {
            throw new NotImplementedException();
        }

        public IResult Create(plataforma instance)
        {
            throw new NotImplementedException();
        }

        public IResult BulkInsertPlataforma(plataforma instance)
        {

            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                var campoSanto = from c in dbContext.campo_santo
                                 where c.codigo_campo_santo == instance.codigo_campo_santo
                                 select c;

                campo_santo campo_santo = new campo_santo();
                campo_santo = campoSanto.FirstOrDefault();

                instance.codigo_corporacion = campo_santo.codigo_corporacion;
                instance.codigo_empresa = campo_santo.codigo_empresa;
                
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["cnSIGEES"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
                {

                    conn.Open();

                    using (System.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction())
                    {

                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(
                        "INSERT INTO [dbo].[plataforma](nombre_plataforma, codigo_corporacion, codigo_empresa, codigo_campo_santo, estado_registro, identificador, codigo_tipo_plataforma, numero_columnas, numero_filas, fecha_registra, usuario_registra) " +
                        "VALUES (@nombre_plataforma, @codigo_corporacion, @codigo_empresa, @codigo_campo_santo, @estado_registro, @identificador, @codigo_tipo_plataforma, @numero_columnas, @numero_filas, @fecha_registra, @usuario_registra) " +
                        "SELECT CAST(scope_identity() AS int)"
                        , conn, tran);

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@nombre_plataforma", instance.nombre_plataforma));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_corporacion", instance.codigo_corporacion));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_empresa", instance.codigo_empresa));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_campo_santo", instance.codigo_campo_santo));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado_registro", instance.estado_registro));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@identificador", instance.identificador));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_tipo_plataforma", instance.codigo_tipo_plataforma));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_columnas", instance.numero_columnas));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_filas", instance.numero_filas));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_registra", instance.fecha_registra));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_registra", instance.usuario_registra));

                        cmd.CommandTimeout = 0;
                        instance.codigo_plataforma = (int)cmd.ExecuteScalar();

                        foreach (sector sector in instance.sector)
                        {
                            System.Data.SqlClient.SqlCommand cmdCuadrante = new System.Data.SqlClient.SqlCommand(
                            "INSERT INTO [dbo].[sector](codigo_sector, numero_sector, codigo_plataforma, eje_x, eje_x_superior, eje_y, eje_y_superior, es_nuevo, estado, numero_pisos_pabellon, numero_columna_pabellon, fecha_registra, usuario_registra) " +
                            "VALUES (@codigo_sector, @numero_sector, @codigo_plataforma, @eje_x, @eje_x_superior, @eje_y, @eje_y_superior, @es_nuevo, @estado, @numero_pisos_pabellon, @numero_columna_pabellon, @fecha_registra, @usuario_registra) " +
                            "SELECT CAST(scope_identity() AS int)"
                            , conn, tran);

                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_sector", sector.codigo_sector));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_sector", sector.numero_sector));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", instance.codigo_plataforma));
                            //cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_x_numero", sector.eje_x_numero));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_x", sector.eje_x));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_x_superior", sector.eje_x_superior));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_y", sector.eje_y));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_y_superior", sector.eje_y_superior));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@es_nuevo", sector.es_nuevo));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado", sector.estado));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_pisos_pabellon", sector.numero_pisos_pabellon));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_columna_pabellon", sector.numero_columna_pabellon));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_registra", sector.fecha_registra));
                            cmdCuadrante.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_registra", sector.usuario_registra));

                            cmdCuadrante.CommandTimeout = 0;
                            sector.id_sector = (int)cmdCuadrante.ExecuteScalar();
                            /*
                            foreach (espacio espacio in sector.espacio)
                            {
                                System.Data.SqlClient.SqlCommand cmdEspacio = new System.Data.SqlClient.SqlCommand(
                                "INSERT INTO [dbo].[espacio](codigo_espacio, codigo_corporacion, codigo_empresa, codigo_campo_santo, codigo_plataforma, id_sector, codigo_tipo_espacio, codigo_tipo_nivel, eje_derecho, eje_izquierdo, eje_superior, eje_inferior, codigo_plano, fecha_registra, usuario_registra, cantidad_nivel_espacio, cantidad_nivel_ocupado, en_conversion, cantidad_maxima_conversion, indica_espacio_definitivo, codigo_estado_espacio, habilitado, es_nicho_tipo_yumbo, numero_secuencia_sector) " +
                                "VALUES (@codigo_espacio, @codigo_corporacion, @codigo_empresa, @codigo_campo_santo, @codigo_plataforma, @id_sector, @codigo_tipo_espacio, @codigo_tipo_nivel, @eje_derecho, @eje_izquierdo, @eje_superior, @eje_inferior, @codigo_plano, @fecha_registra, @usuario_registra, @cantidad_nivel_espacio, @cantidad_nivel_ocupado, @en_conversion, @cantidad_maxima_conversion, @indica_espacio_definitivo, @codigo_estado_espacio, @habilitado, @es_nicho_tipo_yumbo, @numero_secuencia_sector ) "
                                , conn, tran);

                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_espacio", espacio.codigo_espacio));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_corporacion", instance.codigo_corporacion));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_empresa", instance.codigo_empresa));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_campo_santo", instance.codigo_campo_santo));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", instance.codigo_plataforma));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@id_sector", sector.id_sector));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_tipo_espacio", espacio.codigo_tipo_espacio));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_tipo_nivel", espacio.codigo_tipo_nivel));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_derecho", espacio.eje_derecho));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_izquierdo", espacio.eje_izquierdo));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_superior", espacio.eje_superior));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_inferior", espacio.eje_inferior));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plano", espacio.codigo_plano));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_registra", espacio.fecha_registra));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_registra", espacio.usuario_registra));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@cantidad_nivel_espacio", espacio.cantidad_nivel_espacio));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@cantidad_nivel_ocupado", espacio.cantidad_nivel_ocupado));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@en_conversion", espacio.en_conversion));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@cantidad_maxima_conversion", espacio.cantidad_maxima_conversion));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@indica_espacio_definitivo", espacio.indica_espacio_definitivo));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_estado_espacio", espacio.codigo_estado_espacio));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@habilitado", espacio.habilitado));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@es_nicho_tipo_yumbo", espacio.es_nicho_tipo_yumbo));
                                cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_secuencia_sector", espacio.numero_secuencia_sector));

                                cmdEspacio.CommandTimeout = 0;
                                cmdEspacio.ExecuteNonQuery();

                                System.Data.SqlClient.SqlCommand cmdDetalleEstadoEspacio = new System.Data.SqlClient.SqlCommand(
                                "INSERT INTO [dbo].[detalle_estado_espacio](codigo_espacio, estado_registro, fecha_registro, usuario_registro, codigo_estado_espacio) " +
                                "VALUES (@codigo_espacio, @estado_registro, @fecha_registro, @usuario_registro, @codigo_estado_espacio) "
                                , conn, tran);

                                cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_espacio", espacio.codigo_espacio));
                                cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado_registro", 1));
                                cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_registro", espacio.fecha_registra));
                                cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_registro", espacio.usuario_registra));
                                cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_estado_espacio", Common.Constante.estado_espacio.libre));

                                cmdDetalleEstadoEspacio.CommandTimeout = 0;
                                cmdDetalleEstadoEspacio.ExecuteNonQuery();
                            }
                            */
                        }

                        tran.Commit();

                        result.IdRegistro = instance.codigo_plataforma.ToString();
                        result.Success = true;
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                result.Exception = ex;
            }

            return result;
        }

        public IResult BulkInsertEspacio(int codigoPlataforma, List<espacio> listaEspacio)
        {

            IResult result = new Result(false);

            try
            {
                var getPlataforma = from p in dbContext.plataforma
                                 where p.codigo_plataforma == codigoPlataforma
                                 select p;

                plataforma plataforma = new plataforma();
                plataforma = getPlataforma.FirstOrDefault();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["cnSIGEES"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
                {

                    conn.Open();

                    using (System.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction())
                    {
                        foreach (espacio espacio in listaEspacio)
                        {

                            System.Data.SqlClient.SqlCommand cmdEspacio = new System.Data.SqlClient.SqlCommand(
                            "INSERT INTO [dbo].[espacio](codigo_espacio, codigo_espacio_visual, codigo_corporacion, codigo_empresa, codigo_campo_santo, codigo_plataforma, id_sector, codigo_tipo_espacio, codigo_tipo_nivel, eje_derecho, eje_izquierdo, eje_superior, eje_inferior, codigo_plano, fecha_registra, usuario_registra, cantidad_nivel_espacio, cantidad_nivel_ocupado, en_conversion, cantidad_maxima_conversion, indica_espacio_definitivo, codigo_estado_espacio, habilitado, es_nicho_tipo_yumbo, numero_secuencia_sector, numero_grupo) " +
                            "VALUES (@codigo_espacio, @codigo_espacio_visual, @codigo_corporacion, @codigo_empresa, @codigo_campo_santo, @codigo_plataforma, @id_sector, @codigo_tipo_espacio, @codigo_tipo_nivel, @eje_derecho, @eje_izquierdo, @eje_superior, @eje_inferior, @codigo_plano, @fecha_registra, @usuario_registra, @cantidad_nivel_espacio, @cantidad_nivel_ocupado, @en_conversion, @cantidad_maxima_conversion, @indica_espacio_definitivo, @codigo_estado_espacio, @habilitado, @es_nicho_tipo_yumbo, @numero_secuencia_sector, @numero_grupo ) "
                            , conn, tran);

                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_espacio", espacio.codigo_espacio));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_espacio_visual", espacio.codigo_espacio_visual));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_corporacion", plataforma.codigo_corporacion));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_empresa", plataforma.codigo_empresa));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_campo_santo", plataforma.codigo_campo_santo));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", plataforma.codigo_plataforma));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@id_sector", espacio.id_sector));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_tipo_espacio", espacio.codigo_tipo_espacio));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_tipo_nivel", espacio.codigo_tipo_nivel));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_derecho", espacio.eje_derecho));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_izquierdo", espacio.eje_izquierdo));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_superior", espacio.eje_superior));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@eje_inferior", espacio.eje_inferior));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plano", espacio.codigo_plano));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_registra", espacio.fecha_registra));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_registra", espacio.usuario_registra));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@cantidad_nivel_espacio", espacio.cantidad_nivel_espacio));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@cantidad_nivel_ocupado", espacio.cantidad_nivel_ocupado));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@en_conversion", espacio.en_conversion));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@cantidad_maxima_conversion", espacio.cantidad_maxima_conversion));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@indica_espacio_definitivo", espacio.indica_espacio_definitivo));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_estado_espacio", espacio.codigo_estado_espacio));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@habilitado", espacio.habilitado));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@es_nicho_tipo_yumbo", espacio.es_nicho_tipo_yumbo));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_secuencia_sector", espacio.numero_secuencia_sector));
                            cmdEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numero_grupo", espacio.numero_grupo));

                            cmdEspacio.CommandTimeout = 0;
                            cmdEspacio.ExecuteNonQuery();

                            System.Data.SqlClient.SqlCommand cmdDetalleEstadoEspacio = new System.Data.SqlClient.SqlCommand(
                            "INSERT INTO [dbo].[detalle_estado_espacio](codigo_espacio, estado_registro, fecha_registro, usuario_registro, codigo_estado_espacio) " +
                            "VALUES (@codigo_espacio, @estado_registro, @fecha_registro, @usuario_registro, @codigo_estado_espacio) "
                            , conn, tran);

                            cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_espacio", espacio.codigo_espacio));
                            cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado_registro", 1));
                            cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_registro", espacio.fecha_registra));
                            cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_registro", espacio.usuario_registra));
                            cmdDetalleEstadoEspacio.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_estado_espacio", Common.Constante.estado_espacio.libre));

                            cmdDetalleEstadoEspacio.CommandTimeout = 0;
                            cmdDetalleEstadoEspacio.ExecuteNonQuery();
                        }

                        tran.Commit();

                        result.IdRegistro = codigoPlataforma.ToString();
                        result.Success = true;
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                result.Exception = ex;
            }

            return result;
        }

        public IResult ExisteIdentificador(string identificador)
        {
            IResult result = new Result(false);

            try
            {
                var plataforma = from p in dbContext.plataforma
                                 where p.identificador.Contains(identificador) && p.estado_registro == true
                                 select p;

                if (plataforma.Any())
                {
                    result.IdRegistro = "0";
                    result.Success = true;
                }

            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Delete(plataforma instance)
        {

            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["cnSIGEES"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
                {

                    conn.Open();

                    using (System.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction())
                    {

                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(
                        "DELETE FROM detalle_estado_espacio WHERE codigo_espacio IN (SELECT e.codigo_espacio FROM espacio e WHERE e.codigo_plataforma = @codigo_plataforma)"
                        , conn, tran);

                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", instance.codigo_plataforma));
                        cmd.ExecuteNonQuery();

                        System.Data.SqlClient.SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand(
                        "DELETE FROM espacio WHERE codigo_plataforma = @codigo_plataforma"
                        , conn, tran);

                        cmd1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", instance.codigo_plataforma));
                        cmd1.ExecuteNonQuery();

                        System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand(
                        "DELETE FROM sector WHERE codigo_plataforma = @codigo_plataforma"
                        , conn, tran);

                        cmd2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", instance.codigo_plataforma));
                        cmd2.ExecuteNonQuery();

                        System.Data.SqlClient.SqlCommand cmd3 = new System.Data.SqlClient.SqlCommand(
                        "UPDATE FROM plataforma SET estado_registro = @estado_registro, fecha_modifica = @fecha_modifica, usuario_modifica = @usuario_modifica WHERE codigo_plataforma = @codigo_plataforma"
                        , conn, tran);

                        cmd3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado_registro", instance.estado_registro));
                        cmd3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_modifica", instance.fecha_modifica));
                        cmd3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@usuario_modifica", instance.usuario_modifica));
                        cmd3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@codigo_plataforma", instance.codigo_plataforma));
                        cmd3.ExecuteNonQuery();

                        tran.Commit();

                        result.IdRegistro = instance.codigo_plataforma.ToString();
                        result.Success = true;
                    }

                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
        public IResult CreateMultiple(plataforma instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                var campoSanto = from c in dbContext.campo_santo
                                           where c.codigo_campo_santo == instance.codigo_campo_santo
                                           select c;

                campo_santo campo_santo = new campo_santo();
                campo_santo = campoSanto.FirstOrDefault();

                instance.codigo_corporacion = campo_santo.codigo_corporacion;
                instance.codigo_empresa = campo_santo.codigo_empresa;

                this.dbContext.Entry(instance).State = EntityState.Added;

                foreach (sector sector in instance.sector)
                {
                    sector.codigo_plataforma = instance.codigo_plataforma;
                    this.dbContext.Entry(sector).State = EntityState.Added;

                    foreach (espacio espacio in sector.espacio)
                    {
                        espacio.codigo_plataforma = instance.codigo_plataforma;
                        espacio.codigo_campo_santo = instance.codigo_campo_santo;
                        espacio.codigo_empresa = instance.codigo_empresa;
                        espacio.codigo_corporacion = instance.codigo_corporacion;

                        this.dbContext.Entry(espacio).State = EntityState.Added;
                    }
                }

                dbContext.Database.CommandTimeout = 0;
                this.dbContext.SaveChanges();

                result.IdRegistro = instance.codigo_plataforma.ToString();
                result.Success = true;
                
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
        public IResult Update(plataforma instance)
        {
            throw new NotImplementedException();
        }

        public plataforma GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_plataforma == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_plataforma == id);
            }
            return null;
        }

        public IQueryable<plataforma> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }
        public IQueryable<plataforma> GetRegistrosByEmpresa(int pIdEmpresa)
        {
            if (this._repository.IsExists(x => x.codigo_empresa == pIdEmpresa))
            {
                return this._repository.Find(x => x.codigo_empresa == pIdEmpresa);
            }
            return null;
        }

        public string GetSingleJSON(int id)
        {
            throw new NotImplementedException();
        }


        
    }
}