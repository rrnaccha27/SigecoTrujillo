using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
	public class CanalGrupoBL
	{
        private readonly SIGEES.DataAcces.CanalGrupoDA oCanalGrupoDA = new DataAcces.CanalGrupoDA();

        public List<canal_grupo_dto> Listar_Canal()
        {
            return oCanalGrupoDA.Listar_Canal();
        }
        public List<canal_grupo_dto> Listar_Grupo(Int32 codigo_padre)
        {
            return oCanalGrupoDA.Listar_Grupo(codigo_padre);
        }

        public List<canal_grupo_listado_dto> Listar(bool esCanalGrupo, int codigoPadre)
        {
            return oCanalGrupoDA.Listar(esCanalGrupo, codigoPadre);
        }
        public List<canal_grupo_personal_dto> ListarPersonal(int es_canal_grupo)
        {
            return oCanalGrupoDA.ListarPersonal(es_canal_grupo);
        }
        public canal_grupo_detalle_dto Detalle(int codigo_canal_grupo)
        {
            return oCanalGrupoDA.Detalle(codigo_canal_grupo);
        }

        public void EliminarConfiguracion(int codigo_canal_grupo)
        {
            oCanalGrupoDA.EliminarConfiguracion(codigo_canal_grupo);
        }

        public bool ExisteCodigoEquivalencia(int codigo_canal_grupo, string codigo_equivalencia)
        {
            return oCanalGrupoDA.ExisteCodigoEquivalencia(codigo_canal_grupo, codigo_equivalencia);
        }

        public List<canal_grupo_combo_dto> Listar_Canal_Planilla_Bono(Boolean es_planilla_jn = false)
        {
            return oCanalGrupoDA.Listar_Canal_Planilla_Bono(es_planilla_jn);
        }

        public List<canal_jefatura_dto> ListarJefatura()
        {
            return oCanalGrupoDA.ListarJefatura();
        }


    }
}
