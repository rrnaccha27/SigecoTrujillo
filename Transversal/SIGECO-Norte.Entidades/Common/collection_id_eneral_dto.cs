using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
   [Serializable]
   public class tab_temporal_general_dto
   {
       public int codigo_registro { set; get; }

   }

   [Serializable]
   public class collection_id_exclusion_dto
   {
       public int codigo_exclusion{ set; get; }

   }
   [Serializable]
   public class collection_id_empres_dto
   {
       public int codigo_empresa { set; get; }

   }
}
