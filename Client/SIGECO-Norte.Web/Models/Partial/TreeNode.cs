using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models
{
    [MetadataType(typeof(TreeNodeMetadata))]
    public partial class TreeNode
    {
        public class TreeNodeMetadata
        {
            [Display(Name = "El numero de nodo")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public int ID { get; set; }

            [Display(Name = "El numero de nodo superior")]
            public int ParentID { get; set; }

            [Display(Name = "Nombre de nodo")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Longitud de 1 ~ 100")]
            public string Name { get; set; }

            [Display(Name = "Ordenar nodo")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public int Sort { get; set; }

            [Display(Name = "Esta habilitado")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public bool IsEnable { get; set; }

            [Display(Name = "Fecha de creacion")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public DateTime CreateDate { get; set; }

            [Display(Name = "Actualizado")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public DateTime UpdateDate { get; set; }
        }

        public TreeNode(bool isRoot = false)
        {
            //this.ID = Guid.NewGuid();
            this.Sort = 9999;
            this.IsEnable = false;
            this.createDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }

        //-----------------------------------------------------------------------------------------
        //Extend

        public bool HasParent
        {
            get
            {
                return this.ParentID.HasValue;
            }
        }

        public bool IsRoot
        {
            get
            {
                return !this.HasParent;
            }
        }

        private TreeNode _ParentNode = null;
        public TreeNode ParentNode
        {
            get
            {
                if (this._ParentNode == null && this.ParentID.HasValue)
                {
                    using (SIGEESEntities db = new SIGEESEntities())
                    {
                        this._ParentNode = db.TreeNode.SingleOrDefault(x => x.ID == this.ParentID.Value);
                    }
                }
                return this._ParentNode;
            }
        }

        public bool HasChildren
        {
            get
            {
                return this.Children.Any();
            }
        }

        private IQueryable<TreeNode> _Children;
        public IQueryable<TreeNode> Children
        {
            get
            {
                if (this._Children == null)
                {
                    SIGEESEntities db = new SIGEESEntities();
                    this._Children = db.TreeNode
                        .Where(x => x.ParentID == this.ID)
                        .OrderBy(x => x.Sort)
                        .ThenBy(x => x.createDate);
                }
                return this._Children;
            }
        }

    }
}