using SIGEES.Entidades.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{


    public partial class informacion_edificio_dto
    {

  
	public int  id_edificio {set;get;}
    public int id_edificio_padre { set; get; }
	public string codigo_edificio{set;get;}
	 
	public int secuencia_edificio{set;get;}
	
	public string  nombre_edificio {set;get;}
	public string  identificador_edificio{set;get;}
	public int numero_fila {set;get;}
	public int numero_columna{set;get;}
	 public bool es_seleccionado {set;get;}
	public bool estado {set;get;}
	
	public DateTime fecha_registra {set;get;}
	public DateTime? fecha_modifica {set;get;}
	public string usuario_registra {set;get;}
	public string usuario_modifica {set;get;}
    public string orientacion_eje_x { set; get; }
        

    /*ATRIBUTOS ADICIONALES*/
    public int cantidad_nicho { set; get; }
    public int id_sector { set; get; }	
    public int codigo_plataforma { set; get; }
    public int codigo_campo_santo { set; get; }

    public int codigo_empresa { set; get; }


    public informacion_edificio_dto[] lst_edificio { set; get; }

    public List<informacion_nicho_dto> lst_nichos { set; get; }
    public string identificador_pabellon { get; set; }

    public int codigo_piso_pabellon { get; set; }

    public string identificador_plataforma { get; set; }

    public int codigo_tipo_espacio { get; set; }

    public leyenda_dto[] leyenda_eje_y { set; get; }
    public leyenda_dto[] leyenda_eje_x { set; get; }
    }



    //[Serializable]
    //public partial class detalle_edificio {
    //    public int id_sector { set; get; }
    //    public int secuencia_edificio { set; get; }
    //    public int id_edificio { set; get; }
    //    public int id_edificio_padre { set; get; }


    //    public string usuario_registra { get; set; }

    //    public bool es_nuevo { get; set; }

    //    public bool estado { get; set; }
    //} 
}
