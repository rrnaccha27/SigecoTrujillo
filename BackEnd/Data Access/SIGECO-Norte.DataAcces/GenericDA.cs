using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.DataAcces
{
    public class GenericDA<T> where T : class
    {

        static GenericDA()
        {
        }

        public static readonly T Instance =
            typeof(T).InvokeMember(typeof(T).Name,
                                    BindingFlags.CreateInstance |
                                    BindingFlags.Instance |
                                    BindingFlags.Public |
                                    BindingFlags.NonPublic,
                                    null, null, null) as T;


    }
   
}
