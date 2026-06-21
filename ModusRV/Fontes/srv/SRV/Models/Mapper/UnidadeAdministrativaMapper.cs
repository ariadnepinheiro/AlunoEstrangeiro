using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Text;
using SRV.Common;
using SRV.Common.Extension;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public class UnidadeAdministrativaMapper : BaseMapper<UnidadeAdministrativa>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT * 
                       FROM rv_unidade_administrativa u, rv_tipo_unidadm t
                      WHERE u.id_tipo_unidadm = t.id_tipo_unidadm
                        AND u.id_unidade_administrativa = @idUnidade";
        }

        private string QueryFindByCenso()
        {
            return @"SELECT id_unidade_administrativa
                       FROM rv_unidade_administrativa
                      WHERE id_censo = @idCenso";
        }

        protected override string QueryListObjects()
        {
            return @"SELECT * 
                      FROM rv_unidade_administrativa
                     ORDER BY des_unidade_administrativa";
        }

        private string QueryListRegional(UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT id_tipo_unidadm, id_unidade_administrativa, des_unidade_administrativa, id_unidade_regional
                           FROM rv_unidade_administrativa
                          WHERE id_tipo_unidadm = @idTipoUnidadeRegional ");

            if (usuario.Perfil == Perfil.Escola)
                //sql.Append(@" AND id_unidade_administrativa IN (SELECT id_unidade_regional FROM rv_unidade_administrativa 
                //                                                 WHERE id_unidade_administrativa IN (SELECT id_unidade_administrativa 
                //                                                                                       FROM rv_funcao_servidor 
                //                                                                                      WHERE id_servidor = @idServidor
                //                                                                                        AND id_ano_referencia = @idAnoReferencia))");
                sql.Append(@" AND id_unidade_administrativa IN (SELECT ID_UNIDADE_REGIONAL FROM RV_UNIDADE_ADMINISTRATIVA WHERE ID_UNIDADE_ADMINISTRATIVA in (select ID_UNIDADE_ADMINISTRATIVA from RV_USUARIO where [LOGIN] = CAST(@idServidor as varchar)))");

            if (usuario.Perfil == Perfil.Regional)
                sql.Append(@" AND id_unidade_administrativa IN (select ID_REGIONAL from RV_USUARIO where [LOGIN] = CAST(@idServidor as varchar))");

            sql.Append(" ORDER BY des_unidade_administrativa");
            return sql.ToString();
        }


        private string QueryListPesquisa(int? idRegional, int? idTipoUnidadeAdm)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT a.id_unidade_administrativa, 
                                a.des_unidade_administrativa, 
	                            b.id_unidade_administrativa id_unidade_regional, 
                                b.des_unidade_administrativa des_regional,
	                            t.id_tipo_unidadm, 
                                t.des_tipo_unidadm
                           FROM rv_unidade_administrativa b RIGHT OUTER JOIN rv_unidade_administrativa a
                                                                          ON a.id_unidade_regional = b.id_unidade_administrativa
                                                                  INNER JOIN rv_tipo_unidadm t 
                                                                          ON a.id_tipo_unidadm = t.id_tipo_unidadm
                          WHERE 1=1");

            if (idRegional != null)
                sql.Append(@" AND a.id_unidade_regional = @idRegional ");

            if (idTipoUnidadeAdm != null)
                sql.Append(@" AND a.id_tipo_unidadm = @idTipoUnidadeAdm ");


            sql.Append(" ORDER BY des_unidade_administrativa");

            return sql.ToString();
        }

        private string QueryListUnidadeAdministrativa(UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT id_tipo_unidadm, id_unidade_administrativa, des_unidade_administrativa, id_unidade_regional
                           FROM rv_unidade_administrativa
                          WHERE id_unidade_regional = @regional 
                            AND id_tipo_unidadm IN (" + Constants.TipoUnidAdmEscolar + "," + Constants.TipoUnidAdmRegionalPedagogica + "," + Constants.TipoUnidAdmRegionalAdministrativa + "," + Constants.TipoUnidAdmRegionalGestaoPessoas + "," + Constants.TipoUnidAdmRegionalPedagogica_Administrativa + ") ");

            if (usuario.Perfil == Perfil.Escola)
                sql.Append(@" AND id_unidade_administrativa IN (SELECT ID_UNIDADE_ADMINISTRATIVA FROM RV_UNIDADE_ADMINISTRATIVA WHERE ID_UNIDADE_ADMINISTRATIVA in (select ID_UNIDADE_ADMINISTRATIVA from RV_USUARIO where [LOGIN] = CAST(@idServidor as varchar)))");

            sql.Append(" ORDER BY id_tipo_unidadm DESC, des_unidade_administrativa");

            return sql.ToString();
        }

        private string QueryExisteByTipo()
        {
            return @"SELECT COUNT(*)
                       FROM rv_unidade_administrativa
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                        AND id_tipo_unidadm = @idTipoUnidadeAdministrativa";
        }

        private string QueryExiste()
        {
            return @"SELECT COUNT(*)
                       FROM rv_unidade_administrativa
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_unidade_administrativa
                           WHERE id_unidade_administrativa = @idUnidadeAdministrativa";
        }
        private string QueryInsert()
        {
            return @"INSERT INTO rv_unidade_administrativa
                               (id_unidade_administrativa,
                                des_unidade_administrativa,
                                id_censo,
                                id_unidade_regional,
                                id_tipo_unidadm)
                        VALUES (@idUnidadeAdministrativa,
                                @desUnidadeAdministrativa,
                                @idCenso,
                                @idUnidadeRegional,
                                @idTipoUnidadeAdministrativa)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_unidade_administrativa
                        SET des_unidade_administrativa = @desUnidadeAdministrativa,
                            id_censo = @idCenso,
                            id_unidade_regional = @idUnidadeRegional,
                            id_tipo_unidadm = @idTipoUnidadeAdministrativa
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa";
        }

        public override UnidadeAdministrativa LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            UnidadeAdministrativa unidadeAdministrativa = new UnidadeAdministrativa();

            unidadeAdministrativa.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_administrativa"]);
            unidadeAdministrativa.DesUnidadeAdministrativa = (string)reader["des_unidade_administrativa"];

            if (reader.HasColumn("id_censo"))
            {
                if (reader["id_censo"] != DBNull.Value)
                    unidadeAdministrativa.IdCenso = (string)reader["id_censo"];
            }

            unidadeAdministrativa.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
            unidadeAdministrativa.TipoUnidadeAdministrativa.IdTipoUnidAdm = (int)reader["id_tipo_unidadm"];

            if (reader.HasColumn("des_tipo_unidadm"))
                unidadeAdministrativa.TipoUnidadeAdministrativa.DesTipoUnidAdm = (string)reader["des_tipo_unidadm"];

            if (reader["id_unidade_regional"] != DBNull.Value)
            {
                unidadeAdministrativa.Regional = new UnidadeAdministrativa();

                unidadeAdministrativa.Regional.IdUnidadeAdministrativa = Convert.ToInt32(reader["id_unidade_regional"]);

                if (reader.HasColumn("des_regional"))
                {
                    if (reader["des_regional"] != DBNull.Value)
                        unidadeAdministrativa.Regional.DesUnidadeAdministrativa = (string)reader["des_regional"];
                }
            }

            return unidadeAdministrativa;
        }

        public UnidadeAdministrativa Find(int idUnidadeAdministrativa)
        {
            return FindObject("idUnidade", idUnidadeAdministrativa);
        }

        public int? FindByCenso(string idCenso)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idCenso", idCenso);

            int idUnidadeAdministrativa = Convert.ToInt32(ExecuteScalarQuery(QueryFindByCenso(), param));

            if(idUnidadeAdministrativa == 0)
                return null;

            return idUnidadeAdministrativa;
        }

        public IList<UnidadeAdministrativa> List()
        {
            return ListObjects();
        }

        public IList<UnidadeAdministrativa> List(int idAnoReferencia, int regional, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("regional", regional);

            if (usuario.Perfil == Perfil.Escola)
            {
                param.Add("idServidor", Convert.ToInt32(usuario.Login));
                param.Add("idAnoReferencia", idAnoReferencia);
            }

            return ListObjects(QueryListUnidadeAdministrativa(usuario), param);
        }

        public Paging<UnidadeAdministrativa> ListPesquisa(FiltroUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.IdRegional != null)
                param.Add("idRegional", filtro.IdRegional);

            if (filtro.IdTipoUnidadeAdministrativa != null)
                param.Add("idTipoUnidadeAdm", filtro.IdTipoUnidadeAdministrativa);

            return ListPagingObjects(QueryListPesquisa(filtro.IdRegional, filtro.IdTipoUnidadeAdministrativa), param, LoadObject, currentPage, pageSize);
        }

        public IList<UnidadeAdministrativa> ListRegional(int idAnoReferencia, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idTipoUnidadeRegional", Constants.TipoUnidAdmRegional);

            if (usuario.Perfil == Perfil.Regional || usuario.Perfil == Perfil.Escola)
            {
                param.Add("idServidor", Convert.ToInt32(usuario.Login));
                param.Add("idAnoReferencia", idAnoReferencia);
            }

            return ListObjects(QueryListRegional(usuario), param);
        }

        public void Delete(int idUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteUnidadeAdministrativa(int idUnidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExiste(), param));

            return cont > 0 ? true : false;
        }


        public bool ExistebyTipo(int idUnidadeAdministrativa, int idTipoUnidade)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
            param.Add("idTipoUnidadeAdministrativa", idTipoUnidade);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExisteByTipo(), param));

            return cont > 0 ? true : false;
        }

        public void Insert(UnidadeAdministrativa unidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", unidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("desUnidadeAdministrativa", unidadeAdministrativa.DesUnidadeAdministrativa);
            param.Add("idCenso", unidadeAdministrativa.IdCenso);
            param.Add("idUnidadeRegional", unidadeAdministrativa.Regional != null ? unidadeAdministrativa.Regional.IdUnidadeAdministrativa : null);
            param.Add("idTipoUnidadeAdministrativa", unidadeAdministrativa.TipoUnidadeAdministrativa != null ? unidadeAdministrativa.TipoUnidadeAdministrativa.IdTipoUnidAdm : null);

            InsertObject(QueryInsert(), param);
        }

        public void Update(UnidadeAdministrativa unidadeAdministrativa)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUnidadeAdministrativa", unidadeAdministrativa.IdUnidadeAdministrativa);
            param.Add("desUnidadeAdministrativa", unidadeAdministrativa.DesUnidadeAdministrativa);
            param.Add("idCenso", unidadeAdministrativa.IdCenso);
            param.Add("idUnidadeRegional", unidadeAdministrativa.Regional != null ? unidadeAdministrativa.Regional.IdUnidadeAdministrativa : null);
            param.Add("idTipoUnidadeAdministrativa", unidadeAdministrativa.TipoUnidadeAdministrativa != null ? unidadeAdministrativa.TipoUnidadeAdministrativa.IdTipoUnidAdm : null);

            UpdateObject(QueryUpdate(), param);
        }

		public bool ExistePor(int idUnidadeAdministrativa, int idCenso)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();

			param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);
			param.Add("idCenso", idCenso);

			short cont = Convert.ToInt16(ExecuteScalarQuery(QueryExistePor(), param));

			return cont > 0 ? true : false;
		}

		private string QueryExistePor()
		{
			return @"SELECT COUNT(*)
                       FROM rv_unidade_administrativa
                      WHERE id_unidade_administrativa = @idUnidadeAdministrativa
						and id_censo = @idCenso";
		}

    }
}