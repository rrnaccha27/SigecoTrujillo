using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Threading;
using SIGEES.Web.Utils;
using SIGEES.Web.Models.Bean;

namespace SIGEES.Web.Services
{
    public class TipoAccesoItemService : ITipoAccesoItemService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<tipo_acceso_item> _repository;

        public TipoAccesoItemService()
        {
            this._repository = new DataRepository<tipo_acceso_item>(dbContext);
        }

        /// <summary>
        /// Creates the specified instance.
        public IResult Create(tipo_acceso_item instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_tipo_acceso_item.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(tipo_acceso_item instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Update(instance);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Delete(tipo_acceso_item instance)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("REGISTRO NULO");
            }

            IResult result = new Result(false);

            try
            {
                this._repository.Delete(instance);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public tipo_acceso_item GetSingle(int id)
        {
            if (this._repository.IsExists(x => x.codigo_tipo_acceso_item == id))
            {
                return this._repository.FirstOrDefault(x => x.codigo_tipo_acceso_item == id);
            }
            return null;
        }

        public string GetSingleJSON(int id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("ID  NULO");
            }
            var node = this.GetSingle(id);

            var jo = new JObject
            {
                {"codigo_tipo_acceso_item", node.codigo_tipo_acceso_item.ToString()},
                {"nombre_tipo_acceso_item", node.nombre_tipo_acceso_item},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica},
            };

            return JsonConvert.SerializeObject(jo);
        }

        public IQueryable<tipo_acceso_item> GetRegistros(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro.CompareTo(true) == 0);
            }
            return query;
        }

        public string GetAllJson(bool isReadAll = false)
        {

            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistros(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    JObject root = new JObject
                    {
                        {"codigo_tipo_acceso_item", item.codigo_tipo_acceso_item.ToString()},
                        {"nombre_tipo_acceso_item", item.nombre_tipo_acceso_item},
                        {"estado_registro", item.estado_registro.ToString()},
                        {"fecha_registra", Fechas.convertDateTimeToString(item.fecha_registra)},
                        {"fecha_modifica", Fechas.convertDateTimeToString(item.fecha_modifica)},
                        {"usuario_registra", item.usuario_registra},
                        {"usuario_modifica", item.usuario_modifica},
                    };
                    jObjects.Add(root);
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }


        public string GetAllBeanByPerfilJson(int codigoPerfil, bool isReadAll = false)
        {

            var listaCodigoTipoAccesoItem = (from i in this.dbContext.item_tipo_acceso
                                             where i.estado_registro == true && 
                                             i.codigo_perfil_usuario == codigoPerfil
                                             group i by i.codigo_tipo_acceso_item into grupo
                                             select new { codigo_tipo_acceso_item = grupo.Key });

            List<JObject> jObjects = new List<JObject>();
            var allNodes = this.GetRegistros(isReadAll);

            if (allNodes.Any())
            {
                foreach (var item in allNodes)
                {
                    bool registrado = false;
                    foreach (var tipoAcceso in listaCodigoTipoAccesoItem)
                    {
                        if (tipoAcceso.codigo_tipo_acceso_item.CompareTo(item.codigo_tipo_acceso_item) == 0)
                        {
                            JObject root = new JObject
                            {
                                {"codigo_tipo_acceso_item", item.codigo_tipo_acceso_item.ToString()},
                                {"nombre_tipo_acceso_item", item.nombre_tipo_acceso_item},
                                {"registrado", "true"}
                            };
                            jObjects.Add(root);
                            registrado = true;
                            break;
                        }
                    }

                    if (!registrado)
                    {
                        JObject root = new JObject
                        {
                            {"codigo_tipo_acceso_item", item.codigo_tipo_acceso_item.ToString()},
                            {"nombre_tipo_acceso_item", item.nombre_tipo_acceso_item},
                            {"registrado", "false"}
                        };
                        jObjects.Add(root);
                    }
                    
                }

            }

            return JsonConvert.SerializeObject(jObjects);
        }

        private List<int> GetMenuPrincipalByPerfilJson(int codigoPerfil)
        {
            List<int> listaCodigos = new List<int>();
            try
            {
                var listaCodigoTipoAcceso = (from i in this.dbContext.item_tipo_acceso
                                             where i.estado_registro == true &&
                                             i.codigo_perfil_usuario == codigoPerfil
                                             select new
                                             {
                                                 codigo_tipo_acceso_item = i.codigo_tipo_acceso_item
                                             }).AsEnumerable().Select(y => new tipo_acceso_item
                                             {
                                                 codigo_tipo_acceso_item = y.codigo_tipo_acceso_item
                                             }).ToList();

                foreach (var registro in listaCodigoTipoAcceso)
                {
                    listaCodigos.Add(registro.codigo_tipo_acceso_item);
                }
            }
            catch (Exception ex)
            {

            }
            
            return listaCodigos;
        }

        public BeanItemTipoAcceso GetBeanItemTipoAcceso(int codigoPerfil)
        {
            BeanItemTipoAcceso beanItemTipoAcceso = new BeanItemTipoAcceso();
            beanItemTipoAcceso.estadoPermisoTotal = "I";
            beanItemTipoAcceso.estadoLectura = "I";
            beanItemTipoAcceso.estadoEscritura = "I";
            beanItemTipoAcceso.estadoModificacion = "I";
            beanItemTipoAcceso.estadoEliminacion = "I";
            beanItemTipoAcceso.estadoReporte = "I";

            beanItemTipoAcceso.estadoBusqueda = "I";
            beanItemTipoAcceso.estadoSeleccionEspacio = "I";
            beanItemTipoAcceso.estadoReservarEspacio = "I";
            beanItemTipoAcceso.estadoRenovarReserva = "I";
            beanItemTipoAcceso.estadoAnularReserva = "I";
            beanItemTipoAcceso.estadoVenderEspacio = "I";
            beanItemTipoAcceso.estadoAnularVenta = "I";
            beanItemTipoAcceso.estadoRegistrarFallecido = "I";
            beanItemTipoAcceso.estadoRegistrarLapida = "I";
            beanItemTipoAcceso.estadoConversionCinerario = "I";
            beanItemTipoAcceso.estadoModificarGestionEspacio = "I";
            beanItemTipoAcceso.estadoGenerarTXT_RRHH = "I";
            beanItemTipoAcceso.estadoGenerarTXT_Cont = "I";
            beanItemTipoAcceso.estadoRevisionPlanilla = "I";
            beanItemTipoAcceso.estadoEscrituraArticulo = "I";
            beanItemTipoAcceso.estadoEscrituraPersonal = "I";
            beanItemTipoAcceso.estadoEscrituraAnalisisComision = "I";
            beanItemTipoAcceso.estadoLecturaAnalisisComision = "I";
            beanItemTipoAcceso.estadoReclamoCrear = "I";
            beanItemTipoAcceso.estadoReclamoAtenderN1 = "I";
            beanItemTipoAcceso.estadoReclamoAtenderN2 = "I";
            beanItemTipoAcceso.estadoComisionManualListarAll = "I";
            beanItemTipoAcceso.estadoComisionManualEscritura = "I";
            beanItemTipoAcceso.estadoChecklistEscritura = "I";
            beanItemTipoAcceso.estadoComisionInactivaNivel1 = "I";
            beanItemTipoAcceso.estadoComisionInactivaNivel2 = "I";

            try
            {
                List<int>  listaCodigoTipoAccesoItem = GetMenuPrincipalByPerfilJson(codigoPerfil);
                foreach (var codigos in listaCodigoTipoAccesoItem)
                {
                    if (codigos.CompareTo(1) == 0)
                    {
                        beanItemTipoAcceso.estadoPermisoTotal = "A";
                        break;
                    }
                    else if (codigos.CompareTo(2) == 0)
                    {
                        beanItemTipoAcceso.estadoLectura = "A";
                    }
                    else if (codigos.CompareTo(3) == 0)
                    {
                        beanItemTipoAcceso.estadoEscritura = "A";
                    }
                    else if (codigos.CompareTo(4) == 0)
                    {
                        beanItemTipoAcceso.estadoModificacion = "A";
                    }
                    else if (codigos.CompareTo(5) == 0)
                    {
                        beanItemTipoAcceso.estadoEliminacion = "A";
                    }
                    else if (codigos.CompareTo(6) == 0)
                    {
                        beanItemTipoAcceso.estadoReporte = "A";
                    }
                    else if (codigos.CompareTo(7) == 0)
                    {
                        beanItemTipoAcceso.estadoBusqueda = "A";
                    }
                    else if (codigos.CompareTo(8) == 0)
                    {
                        beanItemTipoAcceso.estadoSeleccionEspacio = "A";
                    }
                    else if (codigos.CompareTo(9) == 0)
                    {
                        beanItemTipoAcceso.estadoReservarEspacio = "A";
                    }
                    else if (codigos.CompareTo(10) == 0)
                    {
                        beanItemTipoAcceso.estadoRenovarReserva = "A";
                    }
                    else if (codigos.CompareTo(11) == 0)
                    {
                        beanItemTipoAcceso.estadoAnularReserva = "A";
                    }
                    else if (codigos.CompareTo(12) == 0)
                    {
                        beanItemTipoAcceso.estadoVenderEspacio = "A";
                    }
                    else if (codigos.CompareTo(13) == 0)
                    {
                        beanItemTipoAcceso.estadoAnularVenta = "A";
                    }
                    else if (codigos.CompareTo(14) == 0)
                    {
                        beanItemTipoAcceso.estadoRegistrarFallecido = "A";
                    }
                    else if (codigos.CompareTo(15) == 0)
                    {
                        beanItemTipoAcceso.estadoRegistrarLapida = "A";
                    }
                    else if (codigos.CompareTo(16) == 0)
                    {
                        beanItemTipoAcceso.estadoConversionCinerario = "A";
                    }
                    else if (codigos.CompareTo(17) == 0)
                    {
                        beanItemTipoAcceso.estadoModificarGestionEspacio = "A";
                    }
                    else if (codigos.CompareTo(18) == 0)
                    {
                        beanItemTipoAcceso.estadoGenerarTXT_RRHH = "A";
                    }
                    else if (codigos.CompareTo(19) == 0)
                    {
                        beanItemTipoAcceso.estadoGenerarTXT_Cont = "A";
                    }
                    else if (codigos.CompareTo(20) == 0)
                    {
                        beanItemTipoAcceso.estadoRevisionPlanilla = "A";
                    }
                    else if (codigos.CompareTo(21) == 0)
                    {
                        beanItemTipoAcceso.estadoEscrituraArticulo = "A";
                    }
                    else if (codigos.CompareTo(22) == 0)
                    {
                        beanItemTipoAcceso.estadoEscrituraPersonal = "A";
                    }
                    else if (codigos.CompareTo(23) == 0)
                    {
                        beanItemTipoAcceso.estadoEscrituraAnalisisComision = "A";
                    }
                    else if (codigos.CompareTo(24) == 0)
                    {
                        beanItemTipoAcceso.estadoLecturaAnalisisComision = "A";
                    }
                    else if (codigos.CompareTo(25) == 0)
                    {
                        beanItemTipoAcceso.estadoReclamoCrear = "A";
                    }
                    else if (codigos.CompareTo(26) == 0)
                    {
                        beanItemTipoAcceso.estadoReclamoAtenderN1= "A";
                    }
                    else if (codigos.CompareTo(27) == 0)
                    {
                        beanItemTipoAcceso.estadoReclamoAtenderN2 = "A";
                    }
                    else if (codigos.CompareTo(28) == 0)
                    {
                        beanItemTipoAcceso.estadoComisionManualListarAll = "A";
                    }
                    else if (codigos.CompareTo(29) == 0)
                    {
                        beanItemTipoAcceso.estadoComisionManualEscritura = "A";
                    }
                    else if (codigos.CompareTo(30) == 0)
                    {
                        beanItemTipoAcceso.estadoChecklistEscritura = "A";
                    }
                    else if (codigos.CompareTo(31) == 0)
                    {
                        beanItemTipoAcceso.estadoComisionInactivaNivel1 = "A";
                    }
                    else if (codigos.CompareTo(32) == 0)
                    {
                        beanItemTipoAcceso.estadoComisionInactivaNivel2 = "A";
                    }
                }

                if (beanItemTipoAcceso.estadoPermisoTotal.CompareTo("A") == 0)
                {
                    beanItemTipoAcceso.estadoLectura = "A";
                    beanItemTipoAcceso.estadoEscritura = "A";
                    beanItemTipoAcceso.estadoModificacion = "A";
                    beanItemTipoAcceso.estadoEliminacion = "A";
                    beanItemTipoAcceso.estadoReporte = "A";

                    beanItemTipoAcceso.estadoBusqueda = "A";
                    beanItemTipoAcceso.estadoSeleccionEspacio = "A";
                    beanItemTipoAcceso.estadoReservarEspacio = "A";
                    beanItemTipoAcceso.estadoRenovarReserva = "A";
                    beanItemTipoAcceso.estadoAnularReserva = "A";
                    beanItemTipoAcceso.estadoVenderEspacio = "A";
                    beanItemTipoAcceso.estadoAnularVenta = "A";
                    beanItemTipoAcceso.estadoRegistrarFallecido = "A";
                    beanItemTipoAcceso.estadoRegistrarLapida = "A";
                    beanItemTipoAcceso.estadoConversionCinerario = "A";
                    beanItemTipoAcceso.estadoModificarGestionEspacio = "A";
                    beanItemTipoAcceso.estadoGenerarTXT_RRHH = "A";
                    beanItemTipoAcceso.estadoGenerarTXT_Cont = "A";
                    beanItemTipoAcceso.estadoRevisionPlanilla = "A";
                    beanItemTipoAcceso.estadoEscrituraArticulo = "A";
                    beanItemTipoAcceso.estadoEscrituraPersonal = "A";
                    beanItemTipoAcceso.estadoEscrituraAnalisisComision = "A";
                    beanItemTipoAcceso.estadoLecturaAnalisisComision = "A";
                    beanItemTipoAcceso.estadoReclamoCrear = "A";
                    beanItemTipoAcceso.estadoReclamoAtenderN1 = "A";
                    beanItemTipoAcceso.estadoReclamoAtenderN2 = "A";
                    beanItemTipoAcceso.estadoComisionManualListarAll = "A";
                    beanItemTipoAcceso.estadoComisionManualEscritura = "A";
                    beanItemTipoAcceso.estadoChecklistEscritura = "A";
                    beanItemTipoAcceso.estadoComisionInactivaNivel1 = "A";
                    beanItemTipoAcceso.estadoComisionInactivaNivel2 = "A";
                }
            }
            catch (Exception ex)
            {

            }

            return beanItemTipoAcceso;
        }
    }
}