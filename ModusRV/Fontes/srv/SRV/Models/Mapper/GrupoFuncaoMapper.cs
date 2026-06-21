using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class GrupoFuncaoMapper : BaseMapper<GrupoFuncao>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_grupo_funcao, des_grupo_funcao
                       FROM rv_grupo_funcao
                      WHERE id_grupo_funcao = @idGrupoFuncao";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_grupo_funcao, des_grupo_funcao
                       FROM rv_grupo_funcao
                      ORDER BY des_grupo_funcao";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_grupo_funcao
                               (des_grupo_funcao)
                        VALUES (@desGrupoFuncao)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_grupo_funcao
                        SET des_grupo_funcao = @desGrupoFuncao
                      WHERE id_grupo_funcao = @idGrupoFuncao";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_grupo_funcao
                           WHERE id_grupo_funcao = @idGrupoFuncao";
        }

        private string QueryExisteGrupoFuncao()
        {
            return @"SELECT COUNT(*)
                       FROM rv_grupo_funcao
                      WHERE id_grupo_funcao = @idGrupoFuncao";
        }

        public override GrupoFuncao LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            GrupoFuncao grupoFuncao = new GrupoFuncao();

            grupoFuncao.IdGrupoFuncao = (int)reader["id_grupo_funcao"];
            grupoFuncao.DesGrupoFuncao = (string)reader["des_grupo_funcao"];

            return grupoFuncao;
        }

        public GrupoFuncao Find(int idGrupoFuncao)
        {
            return FindObject("idGrupoFuncao", idGrupoFuncao);
        }

        public IList<GrupoFuncao> List()
        {
            return ListObjects();
        }

        public GrupoFuncao Insert(GrupoFuncao grupoFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desGrupoFuncao", grupoFuncao.DesGrupoFuncao.ToUpper());

            grupoFuncao.IdGrupoFuncao = InsertObjectWithIdentity(QueryInsert(), param);

            return grupoFuncao;
        }

        public void Update(GrupoFuncao grupoFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idGrupoFuncao", grupoFuncao.IdGrupoFuncao);
            param.Add("desGrupoFuncao", grupoFuncao.DesGrupoFuncao.ToUpper());

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idGrupoFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idGrupoFuncao", idGrupoFuncao);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteGrupoFuncao(int idGrupoFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idGrupoFuncao", idGrupoFuncao);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteGrupoFuncao(), param));

            return cont > 0 ? true : false;
        }
    }
}