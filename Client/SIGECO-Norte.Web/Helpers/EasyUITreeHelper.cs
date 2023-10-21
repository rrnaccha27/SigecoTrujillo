using SIGEES.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIGEES.Web.Models;
using SIGEES.Web.Models.Repository;

namespace SIGEES.Web.Helpers
{
    public class EasyUITreeHelper
    {
        private readonly ITreeNodeService _service;

        public EasyUITreeHelper()
        {
            _service = new TreeNodeService();
        }

        /// Gets the root node.
        public TreeNode GetRootNode()
        {
            return this._service.GetRootNode();
        }

        /// Gets the nodes.
        /// <param name="isReadAll">if set to <c>true</c> [is read all].</param>
        public IQueryable<TreeNode> GetNodes(bool isReadAll = false)
        {
            return _service.GetNodes(isReadAll);
        }

        /// Gets the descendant.
        /// <param name="isReadAll">if set to <c>true</c> [is read all].</param>
        public List<TreeNode> GetDescendant(bool isReadAll = false)
        {
            var nodeList = new List<TreeNode>();

            TreeNode rootNode = this.GetRootNode();

            if (rootNode != null)
            {
                nodeList.Add(rootNode);
                var allNodes = this.GetNodes(isReadAll);
                this.GetChildNodes(allNodes.ToList(), rootNode, ref nodeList);
            }
            return nodeList;
        }

        /// Gets the child nodes.
        /// <param name="nodes">The nodes.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeList">The node list.</param>
        private void GetChildNodes(IEnumerable<TreeNode> nodes, TreeNode parentNode, ref List<TreeNode> nodeList)
        {
            if (!nodes.Any()) return;
            if (!parentNode.HasChildren) return;
            if (!nodes.Contains(parentNode)) return;

            var children = nodes.Where(x => x.ParentID == parentNode.ID)
                .OrderBy(x => x.Sort)
                .ThenBy(x => x.createDate);

            foreach (var item in children)
            {
                if (!nodeList.Contains(item))
                {
                    nodeList.Add(item);
                }
                this.GetChildNodes(nodes, item, ref nodeList);
            }
        }

        public string GetTreeJson()
        {
            List<JObject> jObjects = new List<JObject>();

            var allNodes = this._service.GetNodes(true);
            if (allNodes.Any())
            {
                var rootNode = this.GetRootNode();
                JObject root = new JObject
                {
                    {"id", rootNode.ID.ToString()}, 
                    {"text", rootNode.Name}
                };
                root.Add("children", this.GetChildArray(rootNode, allNodes));
                jObjects.Add(root);
            }
            return JsonConvert.SerializeObject(jObjects);
        }

        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodes">The nodes.</param>
        private JArray GetChildArray(TreeNode parentNode, IEnumerable<TreeNode> nodes)
        {
            JArray childArray = new JArray();

            foreach (var node in nodes.Where(x => x.ParentID == parentNode.ID).OrderBy(x => x.Sort))
            {
                JObject subObject = new JObject
                {
                    {"id", node.ID.ToString()}, 
                    {"text", node.Name}
                };

                if (nodes.Any(y => y.ParentID == node.ID))
                {
                    subObject.Add("children", this.GetChildArray(node, nodes));
                }
                childArray.Add(subObject);
            }

            return childArray;
        }

        /// Gets the nodes.
        /// <param name="parentID">The parent ID.</param>
        public string GetNodes(int? parentID)
        {
            List<JObject> jObjects = new List<JObject>();

            if (!parentID.HasValue)
            {
                var rootNode = this.GetRootNode();
                JObject node = new JObject
                {
                    {"id", rootNode.ID.ToString()}, 
                    {"text", rootNode.Name},
                    {"state", this._service.GetNodes().Any(x => x.ParentID == rootNode.ID) ? "closed" : "open" }
                };
                jObjects.Add(node);
            }
            else
            {
                var nodes = this._service.GetNodes().Where(x => x.ParentID == parentID).OrderBy(x => x.Sort);
                if (nodes.Any())
                {
                    foreach (var item in nodes)
                    {
                        JObject node = new JObject
                        {
                            {"id", item.ID.ToString()}, 
                            {"text", item.Name},
                            {"state", this._service.GetNodes().Any(x => x.ParentID == item.ID) ? "closed" : "open" }
                        };
                        jObjects.Add(node);
                    }
                }
            }
            return JsonConvert.SerializeObject(jObjects);
        }
    }
}