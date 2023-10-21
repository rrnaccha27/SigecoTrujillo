using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SIGEES.Web.Areas.Comision
{
    public static class Listados
    {
        public static String GetLiquidadoJson(bool incluirNinguno = false)
        {
            List<JObject> jObjects = new List<JObject>();
            if (incluirNinguno)
            {
                JObject todos = new JObject
                {
                    {"id", "99"},
                    {"text", "TODOS"},
                };
                jObjects.Add(todos);
            }

            JObject elemento1 = new JObject
            {
                {"id", "0"},
                {"text", "PENDIENTE"},
            };
            jObjects.Add(elemento1);

            JObject elemento2 = new JObject
            {
                {"id", "1"},
                {"text", "LIQUIDADO"},
            };
            jObjects.Add(elemento2);

            return JsonConvert.SerializeObject(jObjects);
        }

    }
}