using System;
using System.Collections.Generic;

using System.Transactions;

using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
    public class ArticuloBL : GenericBL<ArticuloBL>
    {
        //private readonly SIGEES.DataAcces.ArticuloDA oArticuloDA = new DataAcces.ArticuloDA();
        public List<articulo_grilla_dto> Listar(articulo_dto busqueda,int sede)
        {
            return ArticuloDA.Instance.Listar(busqueda, sede);
        }

        
        public List<articulo_grilla_dto> ListarBySedeAndTipo(int tipoArticulo, int sede) 
        {
            return ArticuloDA.Instance.ListarBySedeAndTipo( tipoArticulo,  sede);
        }
        public List<articulo_dto> ListarByFiltro(string codigo_empresa, string nro_contrato, string nombre)
        {
            return ArticuloDA.Instance.ListarByFiltro(codigo_empresa, nro_contrato, nombre);
        }
        //public List<articulo_contrato_grilla_dto> ListarArticuloByContratoEmpresa(int codigo_empresa, string nro_contrato, string nombre)
        //{
        //    return ArticuloDA.Instance.ListarArticuloByContratoEmpresa(codigo_empresa, nro_contrato, nombre);
        //}
         
        public MensajeDTO Insertar(articulo_dto v_entidad)
        {
            int v_codigo_articulo = 0;
            int v_codigo_precio = 0;
            int v_codigo_regla = 0;
            int v_codigo_comision = 0;
            string tipoReplicacion = string.Empty;
            List<precio_articulo_replicacion_dto> listaReplicacion = new List<precio_articulo_replicacion_dto>();
            precio_articulo_replicacion_dto precio;

            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_codigo_articulo = ArticuloDA.Instance.Insertar(v_entidad);
                    foreach (var item in v_entidad.lista_precio_articulo)
                    {
                        item.codigo_articulo = v_codigo_articulo;
                        v_codigo_precio = item.codigo_precio;

                        //Un nuevo precio, nuevas comisiones
                        if (item.codigo_precio < 0)
                        { 
                            v_codigo_precio = Precio_ArticuloDA.Instance.Insertar(item);

                            foreach (var v_comision in item.lst_regla_calcula_comision)
                            {
                                v_comision.codigo_precio = v_codigo_precio;
                                v_codigo_regla = Regla_Calculo_ComisonDA.Instance.Insertar(v_comision);
                            }

                            foreach (var v_comision in item.lst_comision_supervisor)
                            {
                                v_comision.codigo_precio = v_codigo_precio;
                                v_codigo_comision = ComisionPrecioSupervisorDA.Instance.Insertar(v_comision);
                            }
                        }
                        else if (item.actualizado == 1)
                        {
                            v_codigo_precio = Precio_ArticuloDA.Instance.Clonar(item);
                        }
                        else
                        {
                            if (item.estado_registro == false)
                            {
                                v_codigo_precio = Precio_ArticuloDA.Instance.Insertar(item);
                            }
                            else
                            { 
                                foreach (var v_comision in item.lst_regla_calcula_comision)
                                {
                                    v_comision.codigo_precio = v_codigo_precio;

                                    if (v_comision.estado_registro)
                                    {
                                        v_codigo_regla = Regla_Calculo_ComisonDA.Instance.Clonar(v_comision);
                                    }
                                    else
                                    {
                                        Regla_Calculo_ComisonDA.Instance.Desactivar(v_comision);
                                    }
                                }

                                foreach (var v_comision in item.lst_comision_supervisor)
                                {
                                    v_comision.codigo_precio = v_codigo_precio;
                                    if (v_comision.estado_registro)
                                    {
                                        v_codigo_comision = ComisionPrecioSupervisorDA.Instance.Clonar(v_comision);
                                    }
                                    else
                                    {
                                        ComisionPrecioSupervisorDA.Instance.Desactivar(v_comision);
                                    }
                                }
                            }
                        }

                        ///Para replicacion en SAP
                        tipoReplicacion = (item.codigo_precio > 0 ? "E" : "N");
                        precio = Precio_ArticuloDA.Instance.BuscarParaReplicacion(v_codigo_precio, tipoReplicacion);
                        listaReplicacion.Add(precio);
                        ///
                    }

                    v_mensaje.idRegistro = v_codigo_articulo;
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

        public articulo_dto BuscarById(int p_codigo_articulo)
        {
            return ArticuloDA.Instance.BuscarById(p_codigo_articulo);
        }

        public System.Data.DataTable ReporteDetalladaByArticulo(int v_codigo_articulo)
        {
            return ArticuloDA.Instance.ReporteDetalladaByArticulo(v_codigo_articulo);
        }

        public List<articulo_dto> ListarParaReglaPagoComision(string nombre)
        {
            return ArticuloDA.Instance.ListarParaReglaPagoComision(nombre);
        }

        public MensajeDTO Eliminar(articulo_dto v_entidad)
        {
            int v_codigo_articulo = 0;            
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    v_codigo_articulo = ArticuloDA.Instance.Eliminar(v_entidad);  
                    
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

        public MensajeDTO Activar(articulo_dto v_entidad)
        {
            int v_codigo_articulo = 0;
            MensajeDTO v_mensaje = new MensajeDTO();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    v_codigo_articulo = ArticuloDA.Instance.Activar(v_entidad);

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
        
        public List<articulo_comision_manual_listado_dto> ListarParaComisionManual(articulo_comision_manual_busqueda_dto busqueda)
        {
            return ArticuloDA.Instance.ListarParaComisionManual(busqueda);
        }
    }
}
