using SIGEES.DataAcces;
using SIGEES.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.BusinessLogic
{

    public partial class PersonaBL : GenericBL<PersonaBL>, BaseBL<persona_dto>
    {

        public MensajeDTO Insertar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }

        public MensajeDTO Actualizar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }

        public MensajeDTO Eliminar(persona_dto pEntidad)
        {
            throw new NotImplementedException();
        }
    }

   public partial class PersonaSelBL : GenericBL<PersonaSelBL>, BaseSelBL<persona_dto>
   {


       public persona_dto BuscarById(persona_dto pEntidad)
       {
           throw new NotImplementedException();
       }

       public List<persona_dto> Listar()
       {
           throw new NotImplementedException();
       }

       public List<persona_dto> ListarVendedor()
       {
           return PersonaSelDA.Instance.ListarVendedor();
       }

       public List<persona_dto> ListarCiente()
       {
           return PersonaSelDA.Instance.ListarCliente();
       }

       public List<persona_dto> Buscar(persona_dto pEntidad)
       {
           throw new NotImplementedException();
       }
   }
}
