using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Text;
using SRV.Common.Extension;
using SRV.Common.Validation;

namespace SRV.Models.Mapper
{
    public class ServidorMapper : BaseMapper<Servidor>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_servidor, des_nome_servidor,
                            des_cpf_servidor, fl_elegivel,
							des_id_funcional, nm_vinculo
                       FROM rv_servidor
                      WHERE id_servidor = @idServidor";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        public string QueryFindServidor(int? idRegional, int? idUnidadeAdministrativa, int idAnoReferencia, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT id_servidor, des_nome_servidor
                           FROM rv_servidor
                          WHERE id_servidor = @idServidor");

            if (idRegional != null)
            {
                if (idUnidadeAdministrativa != null)
                {
                    sql.Append(@" AND EXISTS (SELECT id_servidor
                                                FROM rv_funcao_servidor
                                               WHERE id_servidor = @idServidor
                                                 AND id_unidade_administrativa = @idUnidadeAdministrativa
                                                 AND id_ano_referencia = @idAnoReferencia)");
                }
                else
                {
                    sql.Append(@" AND EXISTS (SELECT fs.id_servidor
                                                FROM rv_funcao_servidor fs,
                                                     rv_unidade_administrativa ua
                                               WHERE fs.id_unidade_administrativa = ua.id_unidade_administrativa
                                                 AND fs.id_servidor = @idServidor
                                                 AND ua.id_unidade_regional = @idRegional
                                                 AND fs.id_ano_referencia = @idAnoReferencia");

                    if (usuario.Perfil == Perfil.Escola)
                    {
                        sql.Append(@"            AND ua.id_unidade_administrativa IN (SELECT id_unidade_administrativa
                                                                                        FROM rv_funcao_servidor
                                                                                       WHERE id_servidor = @idServidorLogado
                                                                                         AND id_ano_referencia = @idAnoReferencia)");
                    }

                    sql.Append("             )");
                }
            }
            else
            {
                if (usuario.Perfil == Perfil.Regional || usuario.Perfil == Perfil.Escola)
                {

                    sql.Append(@" AND EXISTS (SELECT fs.id_servidor
                                                FROM rv_funcao_servidor fs,
                                                     rv_unidade_administrativa ua
                                               WHERE fs.id_unidade_administrativa = ua.id_unidade_administrativa
                                                 AND fs.id_servidor = @idServidor
                                                 AND fs.id_ano_referencia = @idAnoReferencia");

                    if (usuario.Perfil == Perfil.Regional)
                    {
                        sql.Append(@"           AND ua.id_unidade_regional IN (SELECT ua1.id_unidade_regional
                                                                                 FROM rv_unidade_administrativa ua1,
                                                                                      rv_funcao_servidor fs1
                                                                                WHERE ua1.id_unidade_administrativa = fs1.id_unidade_administrativa
                                                                                  AND fs1.id_servidor = @idServidorLogado
                                                                                  AND fs1.id_ano_referencia = @idAnoReferencia))");
                    }
                    else if (usuario.Perfil == Perfil.Escola)
                    {
                        sql.Append(@"           AND ua.id_unidade_administrativa IN (SELECT id_unidade_administrativa
                                                                                       FROM rv_funcao_servidor
                                                                                      WHERE id_servidor = @idServidorLogado
                                                                                        AND id_ano_referencia = @idAnoReferencia))");
                    }
                }
            }

            return sql.ToString();
        }

        private string QueryList(FiltroServidor filtro)
        {
            StringBuilder sql = new StringBuilder();

			sql.Append(@"SELECT id_servidor, des_nome_servidor, des_cpf_servidor, des_id_funcional, nm_vinculo
                           FROM rv_servidor
                          WHERE 1 = 1");

            if (filtro.Nome != null)
                sql.Append(" AND des_nome_servidor LIKE @desNomeServidor");

            if (filtro.Cpf != null)
                sql.Append(" AND des_cpf_servidor = @desCpfServidor");

            sql.Append(" ORDER BY des_nome_servidor");

            return sql.ToString();
        }

        private string QueryListPesquisa(PesquisaServidor pesquisa, UserState usuario)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT id_servidor, des_nome_servidor, des_cpf_servidor
                           FROM rv_servidor
                          WHERE 1 = 1");

            if (pesquisa.Matricula != null)
                sql.Append(" AND id_servidor = @idServidor ");

            if (pesquisa.Nome != null)
                sql.Append(" AND UPPER(des_nome_servidor) LIKE @desNomeServidor");

            if (pesquisa.Cpf != null)
                sql.Append(" AND des_cpf_servidor = @desCpfServidor ");

            if (pesquisa.IdRegionalDefault != null)
            {
                if (pesquisa.IdUnidadeAdministrativaDefault != null)
                {
                    sql.Append(@" AND id_servidor IN (SELECT id_servidor
                                                        FROM rv_funcao_servidor
                                                       WHERE id_unidade_administrativa = @idUnidadeAdministrativa
                                                         AND id_ano_referencia = @idAnoReferencia)");
                }
                else
                {
                    sql.Append(@" AND id_servidor IN (SELECT fs.id_servidor
                                                        FROM rv_funcao_servidor fs,
                                                             rv_unidade_administrativa ua
                                                       WHERE fs.id_unidade_administrativa = ua.id_unidade_administrativa
                                                         AND ua.id_unidade_regional = @idRegional
                                                         AND fs.id_ano_referencia = @idAnoReferencia");

                    if (usuario.Perfil == Perfil.Escola)
                    {
                        sql.Append(@"                    AND ua.id_unidade_administrativa IN (SELECT id_unidade_administrativa
                                                                                                FROM rv_funcao_servidor
                                                                                               WHERE id_servidor = @idServidorLogado
                                                                                                 AND id_ano_referencia = @idAnoReferencia)");
                    }

                    sql.Append("                     )");
                }
            }
            else
            {
                if (usuario.Perfil == Perfil.Regional || usuario.Perfil == Perfil.Escola)
                {

                    sql.Append(@" AND id_servidor IN (SELECT fs.id_servidor
                                                        FROM rv_funcao_servidor fs,
                                                             rv_unidade_administrativa ua
                                                       WHERE fs.id_unidade_administrativa = ua.id_unidade_administrativa
                                                         AND fs.id_ano_referencia = @idAnoReferencia");

                    if (usuario.Perfil == Perfil.Regional)
                    {
                        sql.Append(@"                    AND ua.id_unidade_regional IN (SELECT ua1.id_unidade_regional
                                                                                          FROM rv_unidade_administrativa ua1,
                                                                                               rv_funcao_servidor fs1
                                                                                         WHERE ua1.id_unidade_administrativa = fs1.id_unidade_administrativa
                                                                                           AND fs1.id_servidor = @idServidorLogado
                                                                                           AND fs1.id_ano_referencia = @idAnoReferencia))");
                    }
                    else if (usuario.Perfil == Perfil.Escola)
                    {
                        sql.Append(@"                    AND ua.id_unidade_administrativa IN (SELECT id_unidade_administrativa
                                                                                                FROM rv_funcao_servidor
                                                                                               WHERE id_servidor = @idServidorLogado
                                                                                                 AND id_ano_referencia = @idAnoReferencia))");
                    }
                }
            }

            sql.Append(" ORDER BY des_nome_servidor");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_servidor
                                (id_servidor,
                                 des_nome_servidor,
                                 des_cpf_servidor,
								 des_id_funcional,
								 nm_vinculo)
                          VALUES(@idServidor,
                                 @desNomeServidor,
                                 @desCpfServidor,
								 @idFuncional,
								 @vinculo)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_servidor
                        SET des_nome_servidor = @desNomeServidor,
                            des_cpf_servidor = @desCpfServidor,
							des_id_funcional = @idFuncional,
							nm_vinculo = @vinculo
                      WHERE id_servidor = @idServidor";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_servidor
                           WHERE id_servidor = @idServidor";
        }

        private string QueryExisteServidor()
        {
            return @"SELECT id_servidor 
                       FROM rv_servidor
                      WHERE id_servidor = @idServidor";
        }
		
        public override Servidor LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Servidor servidor = new Servidor();

            servidor.IdServidor = Convert.ToInt32(reader["id_servidor"]);
            servidor.DesNomeServidor = (string)reader["des_nome_servidor"];

            if (reader.HasColumn("des_cpf_servidor"))
                servidor.DesCpfServidor = Convert.ToInt64(reader["des_cpf_servidor"]);

            if (reader.HasColumn("fl_elegivel"))
                servidor.Elegivel = reader["fl_elegivel"].ToString().Equals("S") ? true : false;

			if (reader.HasColumn("des_id_funcional"))
				servidor.IdFuncional = Convert.IsDBNull(reader["des_id_funcional"]) ? default(string) : (string)reader["des_id_funcional"];

			if (reader.HasColumn("nm_vinculo"))
				servidor.Vinculo = Convert.IsDBNull(reader["nm_vinculo"]) ? default(int?) : Convert.ToInt32(reader["nm_vinculo"]);

            return servidor;
        }

        public Servidor Find(int idServidor)
        {
            return FindObject("idServidor", idServidor);
        }

        public Servidor FindServidor(int idServidor, int? idRegional, int? idUnidadeAdministrativa, int idAnoReferencia, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", idServidor);

            if (idRegional != null)
                param.Add("idRegional", idRegional);

            if (idUnidadeAdministrativa != null)
                param.Add("idUnidadeAdministrativa", idUnidadeAdministrativa);

            if (usuario.Perfil != Perfil.Administrador && usuario.Perfil != Perfil.Secretaria)
                param.Add("idServidorLogado", Convert.ToInt32(usuario.Login));

            param.Add("idAnoReferencia", idAnoReferencia);

            return FindObject(QueryFindServidor(idRegional, idUnidadeAdministrativa, idAnoReferencia, usuario), param, LoadObject);
        }

        public Paging<Servidor> List(FiltroServidor filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.Nome != null)
                param.Add("desNomeServidor", String.Concat(filtro.Nome.ToUpper(), "%"));

            if (filtro.Cpf != null)
                param.Add("desCpfServidor", Convert.ToInt64(Cpf.RetiraMascaraCpf(filtro.Cpf)));

            return ListPagingObjects(QueryList(filtro), param, LoadObject, currentPage, pageSize);
        }

        public Paging<Servidor> ListPesquisa(PesquisaServidor pesquisa, int currentPage, int pageSize, UserState usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (pesquisa.Matricula != null)
                param.Add("idServidor", pesquisa.Matricula);

            if (pesquisa.Nome != null)
                param.Add("desNomeServidor", String.Concat(pesquisa.Nome.ToUpper(), "%"));

            if (pesquisa.Cpf != null)
                param.Add("desCpfServidor", Convert.ToInt64(Cpf.RetiraMascaraCpf(pesquisa.Cpf)));

            if(pesquisa.IdRegionalDefault != null)
                param.Add("idRegional", pesquisa.IdRegionalDefault);

            if (pesquisa.IdUnidadeAdministrativaDefault != null)
                param.Add("idUnidadeAdministrativa", pesquisa.IdUnidadeAdministrativaDefault);

            if(usuario.Perfil != Perfil.Administrador && usuario.Perfil != Perfil.Secretaria)
                param.Add("idServidorLogado", Convert.ToInt32(usuario.Login));

            param.Add("idAnoReferencia", pesquisa.IdAnoReferencia);
            
            return ListPagingObjects(QueryListPesquisa(pesquisa, usuario), param, LoadObject, currentPage, pageSize);
        }

        public void Insert(Servidor servidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", servidor.IdServidor);
            param.Add("desNomeServidor", servidor.DesNomeServidor);
            param.Add("desCpfServidor", servidor.DesCpfServidor);
			param.Add("idFuncional", servidor.IdFuncional);
			param.Add("vinculo", servidor.Vinculo);

            InsertObject(QueryInsert(), param);
        }

        public void Update(Servidor servidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", servidor.IdServidor);
            param.Add("desNomeServidor", servidor.DesNomeServidor);
            param.Add("desCpfServidor", servidor.DesCpfServidor);
			param.Add("idFuncional", servidor.IdFuncional);
			param.Add("vinculo", servidor.Vinculo);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idServidor", idServidor);

            DeleteObject(QueryDelete(), param);
        }

        public bool ExisteServidor(int idServidor)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idServidor", idServidor);

            object id = ExecuteScalarQuery(QueryExisteServidor(), param);

            return id != null ? true : false;
        }

    }
}