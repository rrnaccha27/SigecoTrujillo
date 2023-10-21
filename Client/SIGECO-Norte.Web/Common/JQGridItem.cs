using System;
using System.Collections.Generic;

namespace SIGEES.Web.Common
{
    public class JQGridItem
    {
        public JQGridItem(long pId, List<string> pRow)
        {
            ID = pId;
            Row = pRow;
        }


        public long ID { get; set; }
        public List<string> Row { get; set; }
    }
}
