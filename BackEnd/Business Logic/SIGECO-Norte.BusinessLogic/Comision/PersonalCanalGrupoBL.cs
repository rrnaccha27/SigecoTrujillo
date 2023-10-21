using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIGEES.Entidades;
using SIGEES.DataAcces;

namespace SIGEES.BusinessLogic
{
	public class PersonalCanalGrupoBL
	{
        private readonly SIGEES.DataAcces.PersonalCanalGrupoDA oPersonalCanalGrupoDA = new DataAcces.PersonalCanalGrupoDA();

        public List<personal_canal_grupo_listado_dto> Listar(int codigo_personal)
        {
            return oPersonalCanalGrupoDA.Listar(codigo_personal);
        }

        public int Registrar(personal_canal_grupo_dto canal_grupo)
        {
            return oPersonalCanalGrupoDA.Registrar(canal_grupo);
        }

        public void Actualizar(personal_canal_grupo_dto canal_grupo)
        {
            oPersonalCanalGrupoDA.Actualizar(canal_grupo);
        }

        public void EliminarPorPersonal(string canalesEliminar)
        {
            oPersonalCanalGrupoDA.EliminarPorPersonal(canalesEliminar);
        }

        public void AsignarSupervisor(int esCanalGrupo, personal_canal_grupo_dto canal_grupo)
        {
            oPersonalCanalGrupoDA.AsignarSupervisor(esCanalGrupo, canal_grupo);
        }

        public void AsignarPersonal(int esCanalGrupo, personal_canal_grupo_dto canal_grupo)
        {
            oPersonalCanalGrupoDA.AsignarPersonal(esCanalGrupo, canal_grupo);
        }

        public void DesasignarSupervisor(personal_canal_grupo_dto canal_grupo)
        {
            oPersonalCanalGrupoDA.DesasignarSupervisor(canal_grupo);
        }

        public void Transferir(int esCanalGrupo, int codigo_canal_crupo, personal_canal_grupo_dto canal_grupo)
        {
            oPersonalCanalGrupoDA.Transferir(esCanalGrupo, codigo_canal_crupo, canal_grupo);
        }

        public int GetOtrasSupervisiones(int codigo_personal, int codigo_canal_grupo)
        {
            return oPersonalCanalGrupoDA.GetOtrasSupervisiones(codigo_personal, codigo_canal_grupo);
        }

        public personal_canal_grupo_replicacion_dto BuscarParaReplicacion(int codigo_personal, int codigo_canal_grupo, string tipo_operacion)
        {
            return oPersonalCanalGrupoDA.BuscarParaReplicacion(codigo_personal, codigo_canal_grupo, tipo_operacion);
        }

	}
}
