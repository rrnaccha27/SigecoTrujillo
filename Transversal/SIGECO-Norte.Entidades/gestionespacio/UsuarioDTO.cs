using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
   public class UsuarioDTO
    {

        public string usuario { get; set; }
        public string clave { get; set; }

        public Nullable<int> codigoPersona { get; set; }
        public Nullable<int> codigoPerfilUsuario { get; set; }

        public string estadoRegistro { get; set; }
       /****************************************************************/
        public int iCodUsuario { get; set; }

       
        public string vUsuario { get; set; }

       
        public string vPassword { get; set; }

       
        public int iCodPersona { get; set; }

       
        public string vApePaterno { get; set; }

        
        public string vApeMaterno { get; set; }

        
        public string vNombres { get; set; }

        
        public int iCodPerfil { get; set; }


        
        public string vPerfil { get; set; }

        
        public int iCodModulo { get; set; }

        
        public string vNombreModulo { get; set; }

        
        public int iCodSistema { get; set; }


        public string vNombreSistema { get; set; }

        public string username { get; set; }
       
    }


    public partial class  datos_usuario
    {

        public string user_name { set; get; }
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombre { set; get; }
        public string correo { set; get; }

    
    }
}
