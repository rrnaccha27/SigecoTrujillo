
using System;
using System.Collections.Generic;

namespace SIGEES.Web.Common
{
    public class JQGridJsonResponse
    {
        public List<JQGridButton> Buttons;
        public string FunctionDelete;
        public string FunctionDetails;
        public string FunctionEdit;
        public string FunctionFile;
        public string FunctionLink;
        public string Id;
        public string Row;
        public bool ShowDelete;
        public bool ShowDetails;
        public bool ShowEdit;
        public bool ShowFile;
        public bool ShowLink;
        public string TextDelete;
        public string TextDetails;
        public string TextEdit;
        public string TextFile;
        public string TextId;
        public string TextLink;
        public string TextUrl;

        public JQGridJsonResponse() { }

        public string Actions { get; set; }
        public int CurrentPage { get; set; }
        public bool Delete { get; set; }
        public bool Details { get; set; }
        public bool Edit { get; set; }
        public bool File { get; set; }
        public List<JQGridItem> Items { get; set; }
        public bool Link { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int State { get; set; }
        public string[] Values { get; set; }
    }
}
