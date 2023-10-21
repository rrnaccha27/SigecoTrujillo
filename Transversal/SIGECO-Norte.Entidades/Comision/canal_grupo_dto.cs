using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public partial class canal_grupo_listado_dto
    {
        public int codigo_canal_grupo { get; set; }
		public string nombre { get; set; }
		public int codigo_personal { get; set; }
        public string nombre_personal { get; set; }
		public bool s_percibe_comision { get; set; }
        public bool s_percibe_bono { get; set; }
		public bool p_percibe_comision { get; set; }
        public bool p_percibe_bono { get; set; }
        public bool administra_grupos { get; set; }
        public bool estado_registro { get; set; }
        public string codigo_equivalencia { get; set; }
    }

    public partial class canal_grupo_personal_dto
    {
        public int codigo_padre { get; set; }
        public int codigo_canal_grupo { get; set; }
        public string nombre { get; set; }
        public bool supervisor_percibe_comision { get; set; }
        public bool supervisor_percibe_bono { get; set; }
        public bool personal_percibe_comision { get; set; }
        public bool personal_percibe_bono { get; set; }
    }

    public class canal_grupo_dto
    {
        public Int32 codigo_canal_grupo { get; set; }
        public Boolean es_canal_grupo { get; set; }
        public String nombre { get; set; }
        public Int32 codigo_padre { get; set; }
        public Boolean estado { get; set; }
        public DateTime? fecha_registro { get; set; }
        public String usuario_registro { get; set; }
        public DateTime? fecha_modifica { get; set; }
        public String usuario_modifica { get; set; }
        public int id { get; set; }
        public string text { get; set; }

        public canal_grupo_dto()
        {
            codigo_canal_grupo = -1;
            es_canal_grupo = true;
            nombre = "";
            codigo_padre = -1;
            estado = true;
            fecha_registro = null;
            usuario_registro = "";
            fecha_modifica = null;
            usuario_modifica = "";

            id = -1;
            text = "";

        }
    }

    public class canal_grupo_detalle_dto
    {
        public int codigo_canal_grupo { get; set; }
        public string codigo_equivalencia { get; set; }
        public string nombre { get; set; }
        public int codigo_personal { get; set; }
        public bool administra_grupos { get; set; }
        public int s_percibe_comision { get; set; }
        public int s_percibe_bono { get; set; }
        public int p_percibe_comision { get; set; }
        public int p_percibe_bono { get; set; }
        public string s_c_empresa_planilla { get; set; }
        public string s_c_empresa_factura { get; set; }
        public string s_b_empresa_planilla { get; set; }
        public string s_b_empresa_factura { get; set; }
        public string p_c_empresa_planilla { get; set; }
        public string p_c_empresa_factura { get; set; }
        public string p_b_empresa_planilla { get; set; }
        public string p_b_empresa_factura { get; set; }
        public bool estado_registro { get; set; }
        
    }

    public class canal_grupo_combo_dto
    {
        public int id { get; set; }
        public string text { get; set; }
    }

    public class canal_jefatura_dto
    {
        public int codigo_canal { get; set; }
        public string email { get; set; }
        public string email_copia { get; set; }
        public string nombre_canal { get; set; }
    }
}
