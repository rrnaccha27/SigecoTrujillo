using System;
using System.Reflection;

namespace SIGEES.BusinessLogic
{
    public class GenericBL<T> where T : class
    {

        static GenericBL()
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
