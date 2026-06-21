using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using Techne.Data;

namespace Techne.Lyceum.RN.ContratoTemporario
{
    public class CandidatoDocente_GrupoHabilitacao : RNBase
    {
        public static QueryTable ListaDisciplinasHabilitacao(DbObject concurso, string candidato)
        {
            string sql = @"
                SELECT 
                    CG.CANDIDATODOCENTE_GRUPOHABILITACAOID,CG.HABILITADO,GH.AGRUPAMENTO,
                    GH.DESCRICAO 
                FROM LY_GRUPO_HABILITACAO GH 
                    INNER JOIN CONTRATOTEMPORARIO.CANDIDATODOCENTE_GRUPOHABILITACAO CG ON 
                        GH.AGRUPAMENTO = CG.AGRUPAMENTO
                WHERE 
                    CG.CONCURSO = ?  
                    AND CG.CANDIDATO = ?";

            return Consultar(sql, concurso, candidato);
        }
        
        public DataTable ListaDisciplinasHabilitacaoPor(string concurso, string municipio, string regional, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"SELECT DISTINCT GH.AGRUPAMENTO AS AGRUPAMENTO, GH.DESCRICAO AS DESCRICAO 
                FROM LY_GRUPO_HABILITACAO GH 
                   INNER JOIN LY_CONCURSO_DOC_HABILITACAO LDH ON 
                       GH.AGRUPAMENTO = LDH.AGRUPAMENTO 
                WHERE 
                   LDH.CONCURSO = @CONCURSO 
                   AND LDH.MUNICIPIO_PROC = @MUNICIPIO_PROC 
                   AND LDH.REGIONALID = @REGIONALID 
                   AND NOT EXISTS(
                       SELECT CG.CANDIDATODOCENTE_GRUPOHABILITACAOID 
                       FROM CONTRATOTEMPORARIO.CANDIDATODOCENTE_GRUPOHABILITACAO CG
                       WHERE 
                           CG.CONCURSO = @CONCURSO 
                           AND CG.CANDIDATO = @CANDIDATO 
                           AND CG.AGRUPAMENTO = GH.AGRUPAMENTO)
                        ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", municipio);
                contextQuery.Parameters.Add("@REGIONALID", regional);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public int Insere(RN.ContratoTemporario.Entidades.CandidatoDocente_GrupoHabilitacao dados)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"INSERT INTO CONTRATOTEMPORARIO.CANDIDATODOCENTE_GRUPOHABILITACAO
																	(CONCURSO,
																	CANDIDATO,
																	AGRUPAMENTO,
																	HABILITADO)
																	VALUES
																	(@CONCURSO,
																	 @CANDIDATO,
																	 @AGRUPAMENTO,
																	 @HABILITADO)";

                contextQuery.Parameters.Add("@CONCURSO", dados.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", dados.Candidato);
                contextQuery.Parameters.Add("@AGRUPAMENTO", dados.Agrupamento);
                contextQuery.Parameters.Add("@HABILITADO", 1);

                ret = ctx.ApplyModifications(contextQuery);
            }
            catch (SqlException exSql)
            {
                if (exSql.Number == 2601) // Cannot insert duplicate key row in object error
                {
                    exSql.Data.Add("error", "A disciplina " + dados.NomeDisciplina + " já havia sido cadastrada para o candidato.");
                    throw exSql;
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return ret;
        }

        public void AtualizaDisciplinasHabilitacao(int CANDIDATODOCENTE_GRUPOHABILITACAOID, bool habilitado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @"
                UPDATE ContratoTemporario.CANDIDATODOCENTE_GRUPOHABILITACAO SET
                    HABILITADO = @HABILITADO
                WHERE
                    CANDIDATODOCENTE_GRUPOHABILITACAOID = @CANDIDATODOCENTE_GRUPOHABILITACAOID";

                contextQuery.Parameters.Add("@CANDIDATODOCENTE_GRUPOHABILITACAOID", CANDIDATODOCENTE_GRUPOHABILITACAOID);
                contextQuery.Parameters.Add("@HABILITADO", habilitado);

                ctx.ApplyModifications(contextQuery);

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }
        }
    }
}
