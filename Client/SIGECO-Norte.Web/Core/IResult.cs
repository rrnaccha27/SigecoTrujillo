using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace SIGEES.Web.Core
{
    public interface IResult
    {
        Guid ID
        {
            get;
        }

        bool Success
        {
            get;
            set;
        }

        string Message
        {
            get;
            set;
        }

        Exception Exception
        {
            get;
            set;
        }

        List<IResult> InnerResults
        {
            get;
        }

        string IdRegistro { get; set; }
    }
}