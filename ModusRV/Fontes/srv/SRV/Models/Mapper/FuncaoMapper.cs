using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;
using SRV.Common.Extension;

namespace SRV.Models.Mapper
{
    public class FuncaoMapper : BaseMapper<Funcao>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT *
                       FROM rv_funcao
                      WHERE id_funcao = @idFuncao";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT id_funcao, des_funcao
                       FROM rv_funcao
                      ORDER BY des_funcao";
        }

        private string QueryList(FiltroFuncao filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT f.id_funcao, f.des_funcao, g.des_grupo_funcao
                           FROM rv_funcao f LEFT OUTER JOIN rv_grupo_funcao g ON f.id_grupo_funcao = g.id_grupo_funcao
                          WHERE 1 = 1");

            if (filtro.IdFuncao != null)
                sql.Append(" AND f.id_funcao = @idFuncao");

            if (filtro.DesFuncao != null)
                sql.Append(" AND f.des_funcao LIKE @desFuncao");

            if (filtro.IdGrupoFuncao != null)
                sql.Append(" AND f.id_grupo_funcao = @idGrupoFuncao");

            sql.Append(" ORDER BY f.des_funcao, g.des_grupo_funcao");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_funcao
                               (id_funcao,
                                des_funcao,
                                nm_cargahoraria,
                                nm_cargahoraria_plan,
                                id_grupo_funcao,
                                fl_funcao_gratificada)
                        VALUES (@idFuncao,
                                @desFuncao,
                                @cargaHoraria,
                                @cargaHorariaPlan,
                                @grupoFuncao,
                                @gratificada)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_funcao
                        SET des_funcao = @desFuncao,
                            nm_cargahoraria = @cargaHoraria,
                            nm_cargahoraria_plan = @cargaHorariaPlan,
                            id_grupo_funcao = @grupoFuncao,
                            fl_funcao_gratificada = @gratificada
                      WHERE id_funcao = @idFuncao";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_funcao
                           WHERE id_funcao = @idFuncao";
        }

        private string QueryExisteFuncao()
        {
            return @"SELECT id_funcao 
                       FROM rv_funcao
                      WHERE id_funcao = @idFuncao";
        }

        public override Funcao LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Funcao funcao = new Funcao();

            funcao.IdFuncao = (string)reader["id_funcao"];

            if (reader["des_funcao"] != DBNull.Value)
                funcao.DesFuncao = (string)reader["des_funcao"];

            if (reader["nm_cargahoraria"] != DBNull.Value)
                funcao.CargaHoraria = (decimal)reader["nm_cargahoraria"];

            if (reader["nm_cargahoraria_plan"] != DBNull.Value)
                funcao.CargaHorariaPlan = (decimal)reader["nm_cargahoraria_plan"];

            funcao.Gratificada = reader["fl_funcao_gratificada"].ToString().Equals("S") ? true : false;

            funcao.GrupoFuncao = new GrupoFuncao();
            if (reader["id_grupo_funcao"] != DBNull.Value)
                funcao.GrupoFuncao.IdGrupoFuncao = (int)reader["id_grupo_funcao"];

            return funcao;
        }

        public Funcao LoadObjectSimple(System.Data.SqlClient.SqlDataReader reader)
        {
            Funcao funcao = new Funcao();

            funcao.IdFuncao = (string)reader["id_funcao"];
            funcao.DesFuncao = (string)reader["des_funcao"];

            if (reader.HasColumn("des_grupo_funcao"))
            {
                funcao.GrupoFuncao = new GrupoFuncao();

                if (reader["des_grupo_funcao"] != DBNull.Value)
                    funcao.GrupoFuncao.DesGrupoFuncao = (string)reader["des_grupo_funcao"];
            }

            return funcao;
        }

        public Funcao Find(string idFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idFuncao", idFuncao);

            return FindObject(QueryFindObject(), param);
        }

        public IList<Funcao> List()
        {
            return ListObjects(QueryListObjects(), null, LoadObjectSimple);
        }

        public Paging<Funcao> List(FiltroFuncao filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdFuncao != null)
                param.Add("idFuncao", filtro.IdFuncao);

            if (filtro.DesFuncao != null)
                param.Add("desFuncao", String.Concat("%", filtro.DesFuncao, "%"));

            if (filtro.IdGrupoFuncao != null)
                param.Add("idGrupoFuncao", filtro.IdGrupoFuncao);

            return ListPagingObjects(QueryList(filtro), param, LoadObjectSimple, currentPage, pageSize);
        }

        public void Insert(Funcao funcao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idFuncao", funcao.IdFuncao);
            param.Add("desFuncao", StringToUpper(funcao.DesFuncao));
            param.Add("cargaHoraria", funcao.CargaHoraria);
            param.Add("cargaHorariaPlan", funcao.CargaHorariaPlan);
            param.Add("grupoFuncao", funcao.GrupoFuncao.IdGrupoFuncao);
            param.Add("gratificada", funcao.Gratificada ? "S" : "N");

            InsertObject(QueryInsert(), param);
        }

        public void Update(Funcao funcao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idFuncao", funcao.IdFuncao);
            param.Add("desFuncao", StringToUpper(funcao.DesFuncao));
            param.Add("cargaHoraria", funcao.CargaHoraria);
            param.Add("cargaHorariaPlan", funcao.CargaHorariaPlan);
            param.Add("grupoFuncao", funcao.GrupoFuncao.IdGrupoFuncao);
            param.Add("gratificada", funcao.Gratificada ? "S" : "N");

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(string idFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idFuncao", idFuncao);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteFuncao(string idFuncao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idFuncao", idFuncao);

            object id = ExecuteScalarQuery(QueryExisteFuncao(), param);

            return id != null ? true : false;
        }
    }
}