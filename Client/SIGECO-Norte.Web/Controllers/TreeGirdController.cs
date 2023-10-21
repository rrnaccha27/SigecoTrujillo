using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEES.Web.Models;
using SIGEES.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Models.Bean;

namespace SIGEES.Web.Controllers
{
    public class TreeGirdController : Controller
    {

        private readonly ITreeNodeService _service;

        public TreeGirdController()
        {
            _service = new TreeNodeService();
        }

        public ActionResult Index()
        {
            BeanItemTipoAcceso bean = new BeanItemTipoAcceso();
            bean.estadoLectura = "A";
            bean.estadoEscritura = "A";
            bean.estadoModificacion = "A";
            bean.estadoEliminacion = "A";
            return View(bean);
        }

        public ActionResult HasRootNode()
        {
            JObject jo = new JObject();
            bool result = this._service.GetRootNode() != null;
            jo.Add("Msg", result.ToString());
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        public ActionResult LoadTreeNodeDDL()
        {
            var items = this._service.GetTreeJson();
            return Content(items, "application/json");
        }

        public ActionResult GetTreeNodeJSON()
        {
            string result = this._service.GetAllJson(true);
            return Content(result, "application/json");
        }

        [HttpPost]
        public ActionResult GetTreeNode(string id)
        {
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "需輸入TreeNode ID");
                return Content(JsonConvert.SerializeObject(jo));
            }

            //Guid nodeID;
            int nodeID;
            if (!int.TryParse(id, out nodeID))
            {
                jo.Add("Msg", "TreeNode ID 錯誤");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                string jsonContent = this._service.GetSingleJSON(nodeID);
                return Content(jsonContent, "application/json");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
        }

        [HttpPost]
        public ActionResult Create(string parentId, string name, bool enable)
        {
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(parentId) || string.IsNullOrWhiteSpace(name))
            {
                jo.Add("Msg", "POR FAVOR INTRODUZCA LA INFORMACION NECESARIO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            bool isRoot = parentId.Equals("root", StringComparison.OrdinalIgnoreCase);

            int parentNodeID;
            bool parseResult = int.TryParse(parentId, out parentNodeID);

            if (!isRoot && !parseResult)
            {
                jo.Add("Msg", "EL ERROR DE DATOS DE ID DEL NODO SUPERIOR");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            bool checkRoot = isRoot || this._service.IsExists(parentNodeID);

            if (!isRoot && !checkRoot)
            {
                jo.Add("Msg", "NO SE PUEDE ENCONTRAR EL PERFIL SUPERIOR ESPECIFICADO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                jo.Add("Msg", "DEBE INTRODUCIR EL NOMBRE DE NODO");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                TreeNode node = new TreeNode();

                //node.ID = Guid.NewGuid();
                if (!isRoot)
                {
                    node.ParentID = parentNodeID;
                }
                node.Name = name;
                node.IsEnable = enable;

                this._service.Create(node);

                jo.Add("Msg", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "未指定TreeNode ID");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            //Guid nodeID;
            int nodeID;
            if (!int.TryParse(id, out nodeID))
            {
                jo.Add("Msg", "TreeNode ID 錯誤");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            var node = this._service.GetSingle(nodeID);
            if (node == null)
            {
                jo.Add("Msg", "TreeNode不存在");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                //this._service.Delete(new Guid(id))
                this._service.Delete(nodeID);

                jo.Add("Msg", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult Update(string id, string parentId, string name, bool enable)
        {
            //System.Diagnostics.Debug.WriteLine("ENTRO MODIFICAR");
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "沒有輸入 TreeNode ID");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            //Guid nodeID;
            int nodeID;
            if (!int.TryParse(id, out nodeID))
            {
                jo.Add("Msg", "TreeNode ID 錯誤");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            if (!this._service.IsExists(nodeID))
            {
                jo.Add("Msg", "TreeNode 資料不存在");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            var node = this._service.GetSingle(nodeID);
            var originalParentID = node.ParentID.HasValue ? node.ParentID.Value : 0;

            //Guid parentNodeID = new Guid();
            int parentNodeID = 0;
            if (!node.IsRoot)
            {
                if (!int.TryParse(parentId, out parentNodeID))
                {
                    jo.Add("Msg", "Error de ID de nodo superior");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
                if (!this._service.IsExists(parentNodeID))
                {
                    jo.Add("Msg", "La informacion del nodo superior no existe");
                    return Content(JsonConvert.SerializeObject(jo), "application/json");
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                jo.Add("Msg", "No hay entrada en el nombre de nodo");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                if (!node.IsRoot)
                {
                    node.ParentID = parentNodeID;
                }
                node.Name = name;
                node.IsEnable = enable;

                this._service.Update(node, originalParentID);

                jo.Add("Msg", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult MoveUp(string id)
        {
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "沒有輸入TreeNode ID");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            //Guid nodeID;
            int nodeID;
            if (!int.TryParse(id, out nodeID))
            {
                jo.Add("Msg", "TreeNode ID 錯誤");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                this._service.MoveDirection(nodeID, Direction.Up);

                jo.Add("Msg", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        [HttpPost]
        public ActionResult MoveDown(string id)
        {
            JObject jo = new JObject();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("Msg", "沒有輸入TreeNode ID");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            //Guid nodeID;
            int nodeID;
            if (!int.TryParse(id, out nodeID))
            {
                jo.Add("Msg", "TreeNode ID 錯誤");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                this._service.MoveDirection(nodeID, Direction.Down);

                jo.Add("Msg", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Msg", ex.Message);
            }
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

    }
}
