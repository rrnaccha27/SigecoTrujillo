using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades.Common
{
   public class PaginacionDTO
    {
        
        public Int32 iPageSize { get; set; }

        
        public Int32 iCurrentPage { get; set; }

        
        public Int32 iTotalRows { get; set; }

        
        public string vSortColumn { get; set; }

        
        public string vSortOrder { get; set; }

        
        public Int32 iRecordCount { get; set; }

        
        public Int32 iPageCount { get; set; }
    }
}
