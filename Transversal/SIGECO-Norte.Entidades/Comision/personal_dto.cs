using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
    [Serializable]
    public class personal_dto
    {
        public Int32 codigo_personal { get; set; }
        public Int32 codigo_moneda { get; set; }
        public Int32 codigo_banco { get; set; }
        public Int32 codigo_tipo_cuenta { get; set; }
        public Int32 codigo_tipo_documento { get; set; }
        public string codigo_equivalencia { get; set; }
        public String nombre { get; set; }
        public String apellido_paterno { get; set; }
        public String apellido_materno { get; set; }
        public String nro_documento { get; set; }
        public string nro_ruc { get; set; }
        public String telefono_fijo { get; set; }
        public String telefono_celular { get; set; }
        public String correo_electronico { get; set; }
        public String nro_cuenta { get; set; }
        public String codigo_interbancario { get; set; }
        public Boolean es_persona_juridica { get; set; }
        public Boolean estado_registro { get; set; }
        public String usuario_registra { get; set; }
        public DateTime? fecha_registra { get; set; }
        public String usuario_modifica { get; set; }
        public DateTime? fecha_modifica { get; set; }

        public string fecha_registra_texto { get; set; }
        public string fecha_modifica_texto { get; set; }
        public string estado_registro_texto { get; set; }

        public int validado{ get; set; }
        public string validado_texto{ get; set; }
        public string fecha_validado{ get; set; }
        public string usuario_validado { get; set; }

        public string direccion { get; set; }
        public string contacto { get; set; }

        public int codigo_sede { get; set; }
        public List<personal_canal_grupo_dto> lista_canal_grupo { set; get; }
    }

    public class personal_listado_dto
    {
        public Int32 codigo_personal { get; set; }
        public String nombre { get; set; }
        public String apellidos { get; set; }
        public string nombre_tipo_documento { get; set; }
        public String nro_documento { get; set; }
        public bool estado_registro { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public bool es_supervisor_canal { get; set; }
        public bool es_supervisor_grupo { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre_completo { get; set; }
        public string validado { get; set; }
    }
    public class personal_planilla_listado_dto
    {
        public Int32 codigo_personal { get; set; }
        public Int32 codigo_planilla { get; set; }
        public String nombre { get; set; }
        public String apellido_paterno { get; set; }
        public String apellido_materno { get; set; }
        public String apellidos_nombres { get; set; }
        public string nombre_tipo_documento { get; set; }
        public String nro_documento { get; set; }
        public bool estado_registro { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public bool es_supervisor_canal { get; set; }
        public bool es_supervisor_grupo { get; set; }

        public string correo_electronico { get; set; }
        public string apellidos_nombres_vendedor
        {

            get
            {
                return nombre + " " + apellido_paterno + " " + apellido_materno;

            }
        }

        public string codigo_equivalencia { get; set; }
    }
    public class personal_x_canal_grupo_listado_dto
    {
        public int codigo_registro { get; set; }
        public int codigo_personal { get; set; }
        public String nombre { get; set; }
        public string fecha_creacion { get; set; }
        public string usuario_creacion { get; set; }
        public bool es_supervisor { get; set; }
    }

    public class personal_x_nombre_para_canal_grupo_dto
    {
        public int codigo_personal { get; set; }
        public String nombre { get; set; }
        public String apellido_paterno { get; set; }
        public String apellido_materno { get; set; }
        public string fecha_registra { get; set; }
        public string usuario_registra { get; set; }
    }

    public class personal_comision_manual_listado_dto
    {
        public int codigo_personal { get; set; }
        public string nombres { get; set; }
        public int codigo_canal { get; set; }
        public string nombre_canal { get; set; }
        public string codigo_equivalencia { get; set; }
    }

    public class personal_correo
    {
        public int codigo_personal { get; set; }
        public string nombres { get; set; }
        public string email { get; set; }

        public string nombre_envio_correo { get; set; }
        public string apellido_envio_correo { get; set; }
        public string nombre_grupo { get; set; }
    }

    public class personal_jefatura_correo
    {
        public int codigo_canal { get; set; }
        public string email { get; set; }
        public string email_copia { get; set; }
        public string nombre_envio_correo { get; set; }
    }

    public class personal_historico_validacion_dto
    {
        public int id { get; set; }
        public string texto_registra { get; set; }
        public string fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public string texto_modifica { get; set; }
        public string fecha_modifica{ get; set; }
        public string usuario_modifica { get; set; }
    }

    public class personal_historico_bloqueo_dto
    {
        public int codigo_personal { get; set; }
        public string descripcion { get; set; }
        public string numero_planilla { get; set; }
        public string descripcion_planilla { get; set; }
        public string estado_planilla { get; set; }
        public string fecha_registra { get; set; }
        public string fecha_modifica { get; set; }
        public string estado_registro_texto { get; set; }
    }


    public class personal_no_validado_dto
    {
        public int codigo_personal { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre_personal { get; set; }
        public string documento { get; set; }
        public string banco { get; set; }
        public string tipo_cuenta { get; set; }
        public string nro_cuenta { get; set; }
        public string fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }
        public string tipo_validacion { get; set; }
        public string canal_grupo { get; set; }
    }

    public class personal_type_dto
    {
        public int codigo_personal { get; set; }
    }

}
