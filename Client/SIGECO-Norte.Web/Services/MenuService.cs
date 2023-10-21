using SIGEES.Web.Core;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGEES.Web.Utils;
using SIGEES.Web.Models.Bean;

namespace SIGEES.Web.Services
{
    public class MenuService : IMenuService
    {
        private SIGEESEntities dbContext = new SIGEESEntities();

        private readonly IRepository<menu> _repository;

        public MenuService()
        {
            this._repository = new DataRepository<menu>(dbContext);
        }

        public IResult Create(menu instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("POR FAVOR INTRODUZCA UN NODO");
            }

            IResult result = new Result(false);

            try
            {

                this._repository.Add(instance);

                result.IdRegistro = instance.codigo_menu.ToString();
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public IResult Update(menu instance)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("POR FAVOR INTRODUZCA UN NODO");
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


        public IResult MoveDirection(int codigoMenu, Direction direction)
        {
            if (codigoMenu.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("沒有輸入TreeNode ID");
            }

            var instance = this.GetSingle(codigoMenu);

            if (instance == null)
            {
                throw new InvalidOperationException("TreeNode 不存在");
            }

            /*
            if (instance.IsRoot)
            {
                throw new InvalidOperationException("根節點無法上移");
            }*/

            IResult result = new Result(false);

            try
            {
                /*
                var sameParentNodes = instance.ParentNode.Children.ToList();
                int currentNodeIndex = sameParentNodes.Select(x => x.ID).ToList().IndexOf(instance.ID);

                var firstOrDefault = sameParentNodes.FirstOrDefault();
                var lastOrDefault = sameParentNodes.LastOrDefault();

                switch (direction)
                {
                    case Direction.Up:
                        if (firstOrDefault != null && !firstOrDefault.ID.Equals(instance.ID))
                        {
                            var preNodeID = sameParentNodes[currentNodeIndex - 1].ID;
                            var preNode = this.GetSingle(preNodeID);
                            preNode.orden += 1;
                            preNode.fecha_modifica = DateTime.Now;
                            this.Update(preNode);

                            instance.orden -= 1;
                        }
                        break;

                    case Direction.Down:
                        if (lastOrDefault != null && !lastOrDefault.ID.Equals(instance.ID))
                        {
                            var nextNodeID = sameParentNodes[currentNodeIndex + 1].ID;
                            var nextNode = this.GetSingle(nextNodeID);
                            nextNode.orden -= 1;
                            nextNode.fecha_modifica = DateTime.Now;
                            this.Update(nextNode);

                            instance.orden += 1;
                        }
                        break;
                }

                instance.fecha_modifica = DateTime.Now;

                this._repository.Update(instance);

                result.Success = true;*/
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public bool IsExists(int codigoMenu)
        {
            if (codigoMenu.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("沒有輸入TreeNode ID");
            }
            return this._repository.IsExists(x => x.codigo_menu == codigoMenu);
        }

        public menu GetRootNode()
        {
            if (this._repository.Count() > 0)
            {
                return this._repository.FirstOrDefault(x => !x.codigo_menu_padre.HasValue);
            }
            return null;
        }

        public menu GetSingle(int codigoMenu)
        {
            if (this._repository.IsExists(x => x.codigo_menu == codigoMenu))
            {
                return this._repository.FirstOrDefault(x => x.codigo_menu == codigoMenu);
            }
            return null;
        }

        public IQueryable<menu> GetNodes(bool isReadAll = false)
        {
            var query = this._repository.GetAll();
            if (!isReadAll)
            {
                return query.Where(x => x.estado_registro == true);
            }
            return query;
        }

        public string GetTreeJson()
        {
            List<JObject> jObjects = new List<JObject>();

            var allNodes = this.GetNodes(false);
            if (allNodes.Any())
            {
                var rootNode = this.GetRootNode();
                JObject root = new JObject
                {
                    {"id", rootNode.codigo_menu.ToString()}, 
                    {"text", rootNode.nombre_menu}
                };
                root.Add("children", this.GetChildJsonArray(rootNode, allNodes));
                jObjects.Add(root);
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        private JArray GetChildJsonArray(menu parentNode, IEnumerable<menu> nodes)
        {
            JArray childArray = new JArray();

            foreach (var node in nodes.Where(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden))
            {
                JObject subObject = new JObject
                {
                    {"id", node.codigo_menu.ToString()}, 
                    {"text", node.nombre_menu}
                };

                if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
                {
                    subObject.Add("children", this.GetChildJsonArray(node, nodes));
                }
                childArray.Add(subObject);
            }

            return childArray;
        }

        public string GetTreeJsonByPerfil(int codigoPerfil)
        {
            List<JObject> jObjects = new List<JObject>();


            var listaPermisoMenu = (from x in this.dbContext.permiso_menu
                                    where x.estado_registro == true && x.codigo_perfil_usuario == codigoPerfil
                                    select new
                                    {
                                        codigo_permiso_menu = x.codigo_permiso_menu,
                                        codigo_perfil_usuario = x.codigo_perfil_usuario,
                                        codigo_menu = x.codigo_menu
                                    }).AsEnumerable().Select(y => new permiso_menu
                                    {
                                        codigo_permiso_menu = y.codigo_permiso_menu,
                                        codigo_perfil_usuario = y.codigo_perfil_usuario,
                                        codigo_menu = y.codigo_menu
                                    }).ToList();

            var allNodes = this.GetNodes(false);
            if (allNodes.Any())
            {
                var rootNode = this.GetRootNode();
                JObject root = new JObject
                {
                    {"id", rootNode.codigo_menu.ToString()}, 
                    {"text", rootNode.nombre_menu},
                    {"checked", false}
                };
                root.Add("children", this.GetChildJsonArrayChecked(rootNode, allNodes, listaPermisoMenu));
                jObjects.Add(root);
            }
            return JsonConvert.SerializeObject(jObjects);
        }
        private JArray GetChildJsonArrayChecked(menu parentNode, IEnumerable<menu> nodes, IEnumerable<permiso_menu> listaPermisoMenu)
        {
            JArray childArray = new JArray();
            foreach (var node in nodes.Where(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden))
            {

                bool nodoExiste = false;
                foreach (var registro in listaPermisoMenu)
                {

                    if (node.codigo_menu.CompareTo(registro.codigo_menu) == 0)
                    {
                        JObject subObject = new JObject
                        {
                            {"id", node.codigo_menu.ToString()}, 
                            {"text", node.nombre_menu},
                            {"checked", true}
                        };
                        if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
                        {
                            subObject.Add("children", this.GetChildJsonArrayChecked(node, nodes, listaPermisoMenu));
                        }
                        childArray.Add(subObject);
                        nodoExiste = true;

                        break;
                    }
                }

                if (!nodoExiste)
                {
                    JObject subObject = new JObject
                    {
                        {"id", node.codigo_menu.ToString()}, 
                        {"text", node.nombre_menu},
                        {"checked", false}
                    };

                    if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
                    {
                        subObject.Add("children", this.GetChildJsonArrayChecked(node, nodes, listaPermisoMenu));
                    }
                    childArray.Add(subObject);
                }
                nodoExiste = false;
            }

            return childArray;
        }

        public string GetSingleJSON(int id)
        {

            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("沒有輸入TreeNode ID");
            }
            var node = this.GetSingle(id);

            var jo = new JObject
            {
                {"codigo_menu", node.codigo_menu.ToString()},
                {"codigo_menu_padre", node.codigo_menu_padre},
                {"nombre_menu", node.nombre_menu},
                {"orden", node.orden.ToString()},
                {"ruta_menu", node.ruta_menu},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica}
            };

            return JsonConvert.SerializeObject(jo);
        }

        public string GetAllJson(bool isReadAll = false)
        {
            List<JObject> jObjects = new List<JObject>();

            var allNodes = this.GetNodes(isReadAll);
            if (allNodes.Any())
            {
                var rootNode = this.GetRootNode();
                JObject root = this.GetNodeJsonObject(rootNode);
                root.Add("children", this.GetChildArray(allNodes, rootNode));
                jObjects.Add(root);
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        private JArray GetChildArray(IEnumerable<menu> nodes, menu parentNode)
        {
            JArray childArray = new JArray();

            foreach (var node in nodes.Where(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden))
            {
                JObject subObject = this.GetNodeJsonObject(node);

                if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
                {
                    subObject.Add("children", this.GetChildArray(nodes, node));
                }
                childArray.Add(subObject);
            }

            return childArray;
        }

        private JObject GetNodeJsonObject(menu node)
        {
            var jsonObject = new JObject
            {
                {"codigo_menu", node.codigo_menu.ToString()},
                {"codigo_menu_padre", node.codigo_menu_padre},
                {"nombre_menu", node.nombre_menu},
                {"orden", node.orden.ToString()},
                {"ruta_menu", node.ruta_menu},
                {"estado_registro", node.estado_registro.ToString()},
                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
                {"usuario_registra", node.usuario_registra},
                {"usuario_modifica", node.usuario_modifica}
            };

            return jsonObject;
        }

        public bool EsRoot(int codigoMenu)
        {
            bool esRoot = false;
            if (this._repository.IsExists(x => x.codigo_menu == codigoMenu))
            {
                menu menu = this._repository.FirstOrDefault(x => x.codigo_menu == codigoMenu);
                if (menu.codigo_menu_padre == null)
                {
                    esRoot = true;
                }
            }
            return esRoot;
        }
        public List<string> GetListaRutaMenuPerfil(int codigoPerfil)
        {
            List<JObject> jObjects = new List<JObject>();

            var listaPermisoMenu = (from x in this.dbContext.permiso_menu
                                    join m in this.dbContext.menu
                                        on x.codigo_menu equals m.codigo_menu
                                    where x.estado_registro == true && m.estado_registro == true
                                    && x.codigo_perfil_usuario == codigoPerfil
                                    select new
                                    {
                                        ruta_menu = m.ruta_menu,
                                    });

            List<string> listaCodigos = new List<string>();
            foreach (var nodo in listaPermisoMenu)
            {
                listaCodigos.Add(nodo.ruta_menu);
            }
            return listaCodigos;
        }
        public string GetMenuPrincipalByPerfilJson(int codigoPerfil)
        {
            List<JObject> jObjects = new List<JObject>();

            var listaPermisoMenu = (from x in this.dbContext.permiso_menu
                                    join m in this.dbContext.menu
                                        on x.codigo_menu equals m.codigo_menu
                                    where x.estado_registro == true && m.estado_registro == true
                                    && x.codigo_perfil_usuario == codigoPerfil
                                    select new
                                    {
                                        codigo_menu = m.codigo_menu,
                                    }).AsEnumerable().Select(y => new BeanPermisoMenu
                                    {
                                        codigoMenu = y.codigo_menu
                                    }).ToList();

            List<Nullable<int>> listaCodigos = new List<int?>();
            foreach (var nodo in listaPermisoMenu)
            {
                listaCodigos.Add(nodo.codigoMenu);
            }

            List<String> listaCodigosMenu = new List<string>();
            listaCodigosMenu = ObtenerCodigoMenuPadre(listaCodigos);

            var listaMenuPrincipal = (from m in this.dbContext.menu
                                      where m.estado_registro == true &&
                                      listaCodigosMenu.Contains(m.codigo_menu.ToString())
                                      select new
                                      {
                                          codigo_menu = m.codigo_menu,
                                          codigo_menu_padre = m.codigo_menu_padre,
                                          nombre_menu = m.nombre_menu,
                                          ruta_menu = m.ruta_menu,
                                          orden = m.orden
                                      }).AsEnumerable().Select(y => new BeanPermisoMenu
                                      {
                                          codigoMenu = y.codigo_menu,
                                          codigoMenuPadre = y.codigo_menu_padre,
                                          nombreMenu = y.nombre_menu,
                                          rutaMenu = y.ruta_menu,
                                          orden = y.orden
                                      }).ToList();

            if (listaMenuPrincipal.Any())
            {

                var rootNode = this.GetRootNode();
                BeanPermisoMenu beanPermisoMenu = new BeanPermisoMenu();
                beanPermisoMenu.codigoMenu = rootNode.codigo_menu;
                beanPermisoMenu.nombreMenu = rootNode.nombre_menu;

                JObject root = new JObject
                {
                    {"id", beanPermisoMenu.codigoMenu.ToString()}, 
                    {"text", beanPermisoMenu.nombreMenu},
                    {"title", beanPermisoMenu.nombreMenu},
                    {"state", "open"},                    
                    {"ruta", ""},
                    {"url", ""}
                };
                root.Add("children", this.GetChildMenuPrincipalJsonArray(beanPermisoMenu, listaMenuPrincipal, listaCodigos));
                jObjects.Add(root);
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        private JArray GetChildMenuPrincipalJsonArray(BeanPermisoMenu parentNode, IEnumerable<BeanPermisoMenu> nodes, List<Nullable<int>> listaCodigos)
        {
            JArray childArray = new JArray();
            foreach (var node in nodes.Where(x => x.codigoMenuPadre == parentNode.codigoMenu).OrderBy(x => x.orden))
            {

                JObject subObject = new JObject
                    {
                        {"id", node.codigoMenu.ToString()}, 
                        {"text", node.nombreMenu},
                        {"title", node.nombreMenu},
                        {"ruta", node.rutaMenu},
                        {"url", node.rutaMenu}
                    };
                if (string.IsNullOrWhiteSpace(node.rutaMenu))
                    subObject.Add("state", "closed");


                if (nodes.Where(y => y.codigoMenuPadre == node.codigoMenu).Any())
                {
                    subObject.Add("children", this.GetChildMenuPrincipalJsonArray(node, nodes, listaCodigos));
                }
                childArray.Add(subObject);
            }

            return childArray;
        }

        public List<String> ObtenerCodigoMenuPadre(List<Nullable<int>> listaCodigos)
        {
            List<String> listaRetorno = new List<String>();

            foreach (var codigo in listaCodigos)
            {
                listaRetorno.Add(codigo.ToString());
                listaRetorno.AddRange(ObtenerCodigoMenu(codigo));
            }

            listaRetorno = listaRetorno.Distinct().ToList();

            return listaRetorno;
        }
        public List<String> ObtenerCodigoMenu(Nullable<int> codigoMenu)
        {
            List<String> listaCodigos = new List<String>();

            if (codigoMenu.ToString().Length != 0)
            {
                menu menu = this._repository.FirstOrDefault(x => x.codigo_menu == codigoMenu);
                if (menu.codigo_menu_padre.ToString().Length != 0)
                {
                    listaCodigos.Add(menu.codigo_menu_padre.ToString());

                    listaCodigos.AddRange(ObtenerCodigoMenu(menu.codigo_menu_padre));
                }
            }

            return listaCodigos;
        }

    }
}

//using SIGEES.Web.Core;
//using SIGEES.Web.Models;
//using SIGEES.Web.Models.Repository;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using SIGEES.Web.Utils;
//using SIGEES.Web.Models.Bean;

//namespace SIGEES.Web.Services
//{
//    public class MenuService : IMenuService
//    {
//        private SIGEESEntities dbContext = new SIGEESEntities();

//        private readonly IRepository<menu> _repository;

//        public MenuService()
//        {
//            this._repository = new DataRepository<menu>(dbContext);
//        }

//        public IResult Create(menu instance)
//        {
//            if (instance == null)
//            {
//                throw new InvalidOperationException("POR FAVOR INTRODUZCA UN NODO");
//            }

//            IResult result = new Result(false);

//            try
//            {

//                this._repository.Add(instance);

//                result.IdRegistro = instance.codigo_menu.ToString();
//                result.Success = true;
                 
//            }
//            catch (Exception ex)
//            {
//                result.Exception = ex;
//            }
//            return result;
//        }

//        public IResult Update(menu instance)
//        {
//            if (instance == null)
//            {
//                throw new InvalidOperationException("POR FAVOR INTRODUZCA UN NODO");
//            }

//            IResult result = new Result(false);

//            try
//            {

//                this._repository.Update(instance);

//                result.Success = true;
//            }
//            catch (Exception ex)
//            {
//                result.Exception = ex;
//            }
//            return result;
//        }


//        public IResult MoveDirection(int codigoMenu, Direction direction)
//        {
//            if (codigoMenu.Equals(Guid.Empty))
//            {
//                throw new ArgumentNullException("沒有輸入TreeNode ID");
//            }

//            var instance = this.GetSingle(codigoMenu);

//            if (instance == null)
//            {
//                throw new InvalidOperationException("TreeNode 不存在");
//            }

//            /*
//            if (instance.IsRoot)
//            {
//                throw new InvalidOperationException("根節點無法上移");
//            }*/

//            IResult result = new Result(false);

//            try
//            {
//                /*
//                var sameParentNodes = instance.ParentNode.Children.ToList();
//                int currentNodeIndex = sameParentNodes.Select(x => x.ID).ToList().IndexOf(instance.ID);

//                var firstOrDefault = sameParentNodes.FirstOrDefault();
//                var lastOrDefault = sameParentNodes.LastOrDefault();

//                switch (direction)
//                {
//                    case Direction.Up:
//                        if (firstOrDefault != null && !firstOrDefault.ID.Equals(instance.ID))
//                        {
//                            var preNodeID = sameParentNodes[currentNodeIndex - 1].ID;
//                            var preNode = this.GetSingle(preNodeID);
//                            preNode.orden += 1;
//                            preNode.fecha_modifica = DateTime.Now;
//                            this.Update(preNode);

//                            instance.orden -= 1;
//                        }
//                        break;

//                    case Direction.Down:
//                        if (lastOrDefault != null && !lastOrDefault.ID.Equals(instance.ID))
//                        {
//                            var nextNodeID = sameParentNodes[currentNodeIndex + 1].ID;
//                            var nextNode = this.GetSingle(nextNodeID);
//                            nextNode.orden -= 1;
//                            nextNode.fecha_modifica = DateTime.Now;
//                            this.Update(nextNode);

//                            instance.orden += 1;
//                        }
//                        break;
//                }

//                instance.fecha_modifica = DateTime.Now;

//                this._repository.Update(instance);

//                result.Success = true;*/
//            }
//            catch (Exception ex)
//            {
//                result.Exception = ex;
//            }
//            return result;
//        }

//        public bool IsExists(int codigoMenu)
//        {
//            if (codigoMenu.Equals(Guid.Empty))
//            {
//                throw new ArgumentNullException("沒有輸入TreeNode ID");
//            }
//            return this._repository.IsExists(x => x.codigo_menu == codigoMenu);
//        }

//        public menu GetRootNode()
//        {
//            if (this._repository.Count() > 0)
//            {
//                return this._repository.FirstOrDefault(x => !x.codigo_menu_padre.HasValue);
//            }
//            return null;
//        }

//        public menu GetSingle(int codigoMenu)
//        {
//            if (this._repository.IsExists(x => x.codigo_menu == codigoMenu))
//            {
//                return this._repository.FirstOrDefault(x => x.codigo_menu == codigoMenu);
//            }
//            return null;
//        }

//        public IQueryable<menu> GetNodes(bool isReadAll = false)
//        {
//            var query = this._repository.GetAll();
//            if (!isReadAll)
//            {
//                return query.Where(x => x.estado_registro == true);
//            }
//            return query;
//        }

//        public string GetTreeJson()
//        {
//            List<JObject> jObjects = new List<JObject>();

//            var allNodes = this.GetNodes(false);
//            if (allNodes.Any())
//            {
//                var rootNode = this.GetRootNode();
//                JObject root = new JObject
//                {
//                    {"id", rootNode.codigo_menu.ToString()}, 
//                    {"text", rootNode.nombre_menu}
//                };
//                root.Add("children", this.GetChildJsonArray(rootNode, allNodes));
//                jObjects.Add(root);
//            }
//            return JsonConvert.SerializeObject(jObjects);
//        }

//        private JArray GetChildJsonArray(menu parentNode, IEnumerable<menu> nodes)
//        {
//            JArray childArray = new JArray();

//            foreach (var node in nodes.Where(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden))
//            {
//                JObject subObject = new JObject
//                {
//                    {"id", node.codigo_menu.ToString()}, 
//                    {"text", node.nombre_menu}
//                };

//                if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
//                {
//                    subObject.Add("children", this.GetChildJsonArray(node, nodes));
//                }
//                childArray.Add(subObject);
//            }

//            return childArray;
//        }

//        public string GetTreeJsonByPerfil(int codigoPerfil)
//        {
//            List<JObject> jObjects = new List<JObject>();


//            var listaPermisoMenu = (from x in this.dbContext.permiso_menu
//                                    where x.estado_registro == true && x.codigo_perfil_usuario == codigoPerfil
//                                    select new
//                                    {
//                                        codigo_permiso_menu = x.codigo_permiso_menu,
//                                        codigo_perfil_usuario = x.codigo_perfil_usuario,
//                                        codigo_menu = x.codigo_menu
//                                    }).AsEnumerable().Select(y => new permiso_menu {
//                                        codigo_permiso_menu = y.codigo_permiso_menu,
//                                        codigo_perfil_usuario = y.codigo_perfil_usuario,
//                                        codigo_menu = y.codigo_menu
//                                    }).ToList();

//            var allNodes = this.GetNodes(false);
//            if (allNodes.Any())
//            {
//                var rootNode = this.GetRootNode();
//                JObject root = new JObject
//                {
//                    {"id", rootNode.codigo_menu.ToString()}, 
//                    {"text", rootNode.nombre_menu},
//                    {"checked", false}
//                };
//                root.Add("children", this.GetChildJsonArrayChecked(rootNode, allNodes, listaPermisoMenu));
//                jObjects.Add(root);
//            }
//            return JsonConvert.SerializeObject(jObjects);
//        }
//        private JArray GetChildJsonArrayChecked(menu parentNode, IEnumerable<menu> nodes, IEnumerable<permiso_menu> listaPermisoMenu)
//        {
//            JArray childArray = new JArray();
//            foreach (var node in nodes.Where(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden))
//            {

//                bool nodoExiste = false;
//                foreach (var registro in listaPermisoMenu)
//                {
                    
//                    if (node.codigo_menu.CompareTo(registro.codigo_menu) == 0)
//                    {
//                        JObject subObject = new JObject
//                        {
//                            {"id", node.codigo_menu.ToString()}, 
//                            {"text", node.nombre_menu},
//                            {"checked", true}
//                        };
//                        if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
//                        {
//                            subObject.Add("children", this.GetChildJsonArrayChecked(node, nodes, listaPermisoMenu));
//                        }
//                        childArray.Add(subObject);
//                        nodoExiste = true;
                        
//                        break;
//                    }
//                }

//                if (!nodoExiste)
//                {
//                    JObject subObject = new JObject
//                    {
//                        {"id", node.codigo_menu.ToString()}, 
//                        {"text", node.nombre_menu},
//                        {"checked", false}
//                    };

//                    if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
//                    {
//                        subObject.Add("children", this.GetChildJsonArrayChecked(node, nodes, listaPermisoMenu));
//                    }
//                    childArray.Add(subObject);
//                }
//                nodoExiste = false;
//            }

//            return childArray;
//        }

//        public string GetSingleJSON(int id)
//        {

//            if (id.Equals(Guid.Empty))
//            {
//                throw new ArgumentNullException("沒有輸入TreeNode ID");
//            }
//            var node = this.GetSingle(id);

//            var jo = new JObject
//            {
//                {"codigo_menu", node.codigo_menu.ToString()},
//                {"codigo_menu_padre", node.codigo_menu_padre},
//                {"nombre_menu", node.nombre_menu},
//                {"orden", node.orden.ToString()},
//                {"ruta_menu", node.ruta_menu},
//                {"estado_registro", node.estado_registro.ToString()},
//                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
//                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
//                {"usuario_registra", node.usuario_registra},
//                {"usuario_modifica", node.usuario_modifica}
//            };

//            return JsonConvert.SerializeObject(jo);
//        }

//        public string GetAllJson(bool isReadAll = false)
//        {
//            List<JObject> jObjects = new List<JObject>();

//            var allNodes = this.GetNodes(isReadAll);
//            if (allNodes.Any())
//            {
//                var rootNode = this.GetRootNode();
//                JObject root = this.GetNodeJsonObject(rootNode);
//                root.Add("children", this.GetChildArray(allNodes, rootNode));
//                jObjects.Add(root);
//            }
//            return JsonConvert.SerializeObject(jObjects);
//        }

//        private JArray GetChildArray(IEnumerable<menu> nodes, menu parentNode)
//        {
//            JArray childArray = new JArray();

//            foreach (var node in nodes.Where(x => x.codigo_menu_padre == parentNode.codigo_menu).OrderBy(x => x.orden))
//            {
//                JObject subObject = this.GetNodeJsonObject(node);

//                if (nodes.Where(y => y.codigo_menu_padre == node.codigo_menu).Any())
//                {
//                    subObject.Add("children", this.GetChildArray(nodes, node));
//                }
//                childArray.Add(subObject);
//            }

//            return childArray;
//        }

//        private JObject GetNodeJsonObject(menu node)
//        {
//            var jsonObject = new JObject
//            {
//                {"codigo_menu", node.codigo_menu.ToString()},
//                {"codigo_menu_padre", node.codigo_menu_padre},
//                {"nombre_menu", node.nombre_menu},
//                {"orden", node.orden.ToString()},
//                {"ruta_menu", node.ruta_menu},
//                {"estado_registro", node.estado_registro.ToString()},
//                {"fecha_registra", Fechas.convertDateTimeToString(node.fecha_registra)},
//                {"fecha_modifica", Fechas.convertDateTimeToString(node.fecha_modifica)},
//                {"usuario_registra", node.usuario_registra},
//                {"usuario_modifica", node.usuario_modifica}
//            };

//            return jsonObject;
//        }

//        public bool EsRoot(int codigoMenu)
//        {
//            bool esRoot = false;
//            if (this._repository.IsExists(x => x.codigo_menu == codigoMenu))
//            {
//                menu menu = this._repository.FirstOrDefault(x => x.codigo_menu == codigoMenu);
//                if (menu.codigo_menu_padre == null)
//                {
//                    esRoot = true;
//                }
//            }
//            return esRoot;
//        }
//        public List<string> GetListaRutaMenuPerfil(int codigoPerfil)
//        {
//            List<JObject> jObjects = new List<JObject>();

//            var listaPermisoMenu = (from x in this.dbContext.permiso_menu
//                                    join m in this.dbContext.menu
//                                        on x.codigo_menu equals m.codigo_menu
//                                    where x.estado_registro == true && m.estado_registro == true
//                                    && x.codigo_perfil_usuario == codigoPerfil
//                                    select new
//                                    {
//                                        ruta_menu = m.ruta_menu,
//                                    });

//            List<string> listaCodigos = new List<string>();
//            foreach (var nodo in listaPermisoMenu)
//            {
//                listaCodigos.Add(nodo.ruta_menu);
//            }
//            return listaCodigos;
//        }
//        public string GetMenuPrincipalByPerfilJson(int codigoPerfil)
//        {
//            List<JObject> jObjects = new List<JObject>();

//            var listaPermisoMenu = (from x in this.dbContext.permiso_menu join m in this.dbContext.menu 
//                                    on x.codigo_menu equals m.codigo_menu
//                                    where x.estado_registro == true && m.estado_registro == true 
//                                    && x.codigo_perfil_usuario == codigoPerfil
//                                    select new
//                                    {
//                                        codigo_menu = m.codigo_menu,
//                                    }).AsEnumerable().Select(y => new BeanPermisoMenu
//                                    {
//                                        codigoMenu = y.codigo_menu
//                                    }).ToList();

//            List<Nullable<int>> listaCodigos = new List<int?>();
//            foreach (var nodo in listaPermisoMenu)
//            {
//                listaCodigos.Add(nodo.codigoMenu);
//            }

//            List<String> listaCodigosMenu = new List<string>();
//            listaCodigosMenu = ObtenerCodigoMenuPadre(listaCodigos);

//            var listaMenuPrincipal = (from m in this.dbContext.menu
//                                    where m.estado_registro == true &&
//                                    listaCodigosMenu.Contains(m.codigo_menu.ToString())
//                                    select new
//                                    {
//                                        codigo_menu = m.codigo_menu,
//                                        codigo_menu_padre = m.codigo_menu_padre,
//                                        nombre_menu = m.nombre_menu,
//                                        ruta_menu = m.ruta_menu,
//                                        orden = m.orden
//                                    }).AsEnumerable().Select(y => new BeanPermisoMenu
//                                    {
//                                        codigoMenu = y.codigo_menu,
//                                        codigoMenuPadre = y.codigo_menu_padre,
//                                        nombreMenu = y.nombre_menu,
//                                        rutaMenu = y.ruta_menu,
//                                        orden = y.orden
//                                    }).ToList();

//            if (listaMenuPrincipal.Any())
//            {

//                var rootNode = this.GetRootNode();
//                BeanPermisoMenu beanPermisoMenu = new BeanPermisoMenu();
//                beanPermisoMenu.codigoMenu = rootNode.codigo_menu;
//                beanPermisoMenu.nombreMenu = rootNode.nombre_menu;

//                JObject root = new JObject
//                {
//                    {"id", beanPermisoMenu.codigoMenu.ToString()}, 
//                    {"text", beanPermisoMenu.nombreMenu}
//                };
//                root.Add("children", this.GetChildMenuPrincipalJsonArray(beanPermisoMenu, listaMenuPrincipal, listaCodigos));
//                jObjects.Add(root);
//            }
//            return JsonConvert.SerializeObject(jObjects);
//        }

//        private JArray GetChildMenuPrincipalJsonArray(BeanPermisoMenu parentNode, IEnumerable<BeanPermisoMenu> nodes, List<Nullable<int>> listaCodigos)
//        {
//            JArray childArray = new JArray();
//            foreach (var node in nodes.Where(x => x.codigoMenuPadre == parentNode.codigoMenu).OrderBy(x => x.orden))
//            {

//                JObject subObject = new JObject
//                    {
//                        {"id", node.codigoMenu.ToString()}, 
//                        {"text", node.nombreMenu},
//                        {"ruta", node.rutaMenu}
//                    };

//                if (nodes.Where(y => y.codigoMenuPadre == node.codigoMenu).Any())
//                {
//                    subObject.Add("children", this.GetChildMenuPrincipalJsonArray(node, nodes, listaCodigos));
//                }
//                childArray.Add(subObject);
//            }

//            return childArray;
//        }

//        public List<String> ObtenerCodigoMenuPadre(List<Nullable<int>> listaCodigos)
//        {
//            List<String> listaRetorno = new List<String>();

//            foreach (var codigo in listaCodigos){
//                listaRetorno.Add(codigo.ToString());
//                listaRetorno.AddRange(ObtenerCodigoMenu(codigo));
//            }

//            listaRetorno = listaRetorno.Distinct().ToList();

//            return listaRetorno;
//        }
//        public List<String> ObtenerCodigoMenu(Nullable<int> codigoMenu)
//        {
//            List<String> listaCodigos = new List<String>();

//            if (codigoMenu.ToString().Length != 0)
//            {
//                menu menu = this._repository.FirstOrDefault(x => x.codigo_menu == codigoMenu);
//                if (menu.codigo_menu_padre.ToString().Length != 0)
//                {
//                    listaCodigos.Add(menu.codigo_menu_padre.ToString());

//                    listaCodigos.AddRange(ObtenerCodigoMenu(menu.codigo_menu_padre));
//                }
//            }

//            return listaCodigos;
//        }

//    }
//}