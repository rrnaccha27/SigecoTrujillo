using SIGEES.Web.Core;
using SIGEES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Services
{
    public interface ITreeNodeService
    {
        IResult Create(TreeNode instance);
        IResult Update(TreeNode instance);
        IResult Update(TreeNode instance, int originalParentID);
        IResult MoveDirection(int id, Direction direction);
        IResult Delete(int id);
        IResult Delete(TreeNode instance);

        bool IsExists(int id);

        TreeNode GetRootNode();
        TreeNode GetSingle(int id);
        IQueryable<TreeNode> GetNodes(bool isReadAll = false);

        string GetTreeJson();
        string GetSingleJSON(int id);
        string GetAllJson(bool isReadAll = false);
    }
}