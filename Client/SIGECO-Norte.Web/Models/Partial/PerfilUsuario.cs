using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEES.Web.Models
{
    [MetadataType(typeof(PerfilUsuarioMetadata))]
    public partial class PerfilUsuario
    {
        public class PerfilUsuarioMetadata
        {
            [Display(Name = "Codigo perfil")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public int codigoPerfilUsuario { get; set; }

            [Display(Name = "Nombre perfil")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            [StringLength(50, MinimumLength = 1, ErrorMessage = "Longitud de 1 ~ 50")]
            public string nombrePerfilUsuario { get; set; }

            [Display(Name = "Estado registro")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public string estadoRegistro { get; set; }

            [Display(Name = "Fecha registra")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            public DateTime fechaRegistra { get; set; }

            [Display(Name = "Fecha modifica")]
            public DateTime fechaModifica { get; set; }

            [Display(Name = "Usuario registra")]
            [Required(ErrorMessage = "Este es un campo obligatorio")]
            [StringLength(20, MinimumLength = 1, ErrorMessage = "Longitud de 1 ~ 20")]
            public string usuarioRegistra { get; set; }

            [Display(Name = "Usuario modifica")]
            [StringLength(20, MinimumLength = 1, ErrorMessage = "Longitud de 1 ~ 20")]
            public string usuarioModifica { get; set; }
        }

        public PerfilUsuario()
        {
        }

    }
}