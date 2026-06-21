using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
   public class RespostaDependenciaOpcao
   {
       public DataTable ListarRespostaDependenciaOpcao(int campanhaId, string unidadeEns)
       {
           DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable retorno = null;

           try
           {
               contextQuery.Command = @"
                            select * from InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO
                            where RESPOSTADEPENDENCIAID in (
                                select RESPOSTADEPENDENCIAID from InspecaoEscolar.RESPOSTADEPENDENCIA
                                where CAMPANHAESCOLAID in (
                                    select CAMPANHAESCOLAID from InspecaoEscolar.CAMPANHAESCOLA
                                    where CAMPANHAID = @CAMPANHAID
                                    and UNIDADE_ENS = @UNIDADE_ENS
                                )
                            )
                            ";

               contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
               contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.Int, unidadeEns);

               retorno = contexto.GetDataTable(contextQuery);
           }
           catch (Exception ex)
           {
               contexto.Abandon();
               string mensagem = string.Empty;
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
               contexto.Dispose();
           }

           return retorno;
       }

       public ICollection<Entidades.RespostaDependenciaOpcao> ListaRespostaDependenciaOpcaoPor(DataContext contexto, int respostaDependenciaId)
       {	
           ICollection<Entidades.RespostaDependenciaOpcao> lista = new List<Entidades.RespostaDependenciaOpcao>();
           ContextQuery contextQuery = new ContextQuery();

           contextQuery.Command = @" SELECT O.* 
                                          FROM [INSPECAOESCOLAR].[RESPOSTADEPENDENCIAOPCAO] O
                                          WHERE [RESPOSTADEPENDENCIAID] = @RESPOSTADEPENDENCIAID ";

           contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAID", SqlDbType.Int, respostaDependenciaId);

           lista = contexto.TryToBindEntities<Entidades.RespostaDependenciaOpcao>(contextQuery);

           return lista;
       }

       public int Insere(DataContext contexto, Entidades.RespostaDependenciaOpcao respostaDependenciaOpcao)
       {

           ContextQuery contextQuery = new ContextQuery();
           contextQuery.Command = @" INSERT INTO InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO
                                                (RESPOSTADEPENDENCIAID
                                                ,OPCOESASSUNTOID
                                                ,ACAODIRECAOID
                                                ,USUARIOID
                                                ,DATACADASTRO
                                                ,DATAALTERACAO)
                                    VALUES      (@RESPOSTADEPENDENCIAID
                                                ,@OPCOESASSUNTOID
                                                ,@ACAODIRECAOID
                                                ,@USUARIOID
                                                ,@DATACADASTRO
                                                ,@DATAALTERACAO)
                    
                    SELECT IDENT_CURRENT('InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO') ";

           contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAID", SqlDbType.Int, respostaDependenciaOpcao.RespostaDependenciaId);
           contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.VarChar, respostaDependenciaOpcao.OpcoesAssuntoId);
           contextQuery.Parameters.Add("@ACAODIRECAOID", SqlDbType.VarChar, respostaDependenciaOpcao.AcaoDirecaoId);
           contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, respostaDependenciaOpcao.UsuarioId);
           contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
           contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

           respostaDependenciaOpcao.RespostaDependenciaOpcaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

           return respostaDependenciaOpcao.RespostaDependenciaOpcaoId;
       }

       public int RetornaRespostaDependenciaOpcaoIdPor(DataContext contexto, int respostaDependenciaId, int opcoesAssuntoId)
       {
           ContextQuery contextQuery = new ContextQuery();
           SqlDataReader reader = null;
           int retorno = 0;
           try
           {
               contextQuery.Command = @" SELECT RESPOSTADEPENDENCIAOPCAOID
                                                FROM   InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO  (NOLOCK)
                                                WHERE  RESPOSTADEPENDENCIAID = @RESPOSTADEPENDENCIAID
                                                       AND OPCOESASSUNTOID = @OPCOESASSUNTOID ";

               contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAID", SqlDbType.Int, respostaDependenciaId);
               contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, opcoesAssuntoId);

               reader = contexto.GetDataReader(contextQuery);

               while (reader.Read())
               {
                   retorno = Convert.ToInt32(reader["RESPOSTADEPENDENCIAOPCAOID"]);
               }
           }
           finally
           {
               if (reader != null)
               {
                   reader.Close();
               }
           }

           return retorno;
       }

       public int RetornaQuantidadePor(DataContext contexto, int campanhaEscolaId, bool salaAula, bool Banheiro)
       {
           ContextQuery contextQuery = new ContextQuery();
           SqlDataReader reader = null;
           StringBuilder sql = new StringBuilder();
           int retorno = 0;
           try
           {
               sql.Append(@" SELECT COUNT(*) AS QUANTIDADE
                                FROM   INSPECAOESCOLAR.RESPOSTADEPENDENCIAOPCAO R 
                                       INNER JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD
                                                ON RD.RESPOSTADEPENDENCIAID = R.RESPOSTADEPENDENCIAID
                                       INNER JOIN INSPECAOESCOLAR.OPCOESASSUNTO O (NOLOCK) 
                                               ON R.OPCOESASSUNTOID = O.OPCOESASSUNTOID 
                                       INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                               ON O.ASSUNTOID = A.ASSUNTOID 
                                       INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                               ON G.GRUPOID = A.GRUPOID 
                                WHERE  RD.CAMPANHAESCOLAID = @CAMPANHAESCOLAID 
                                            ");

               if (salaAula) //--6  DEPENDÊNCIAS - SALA DE AULA
               {
                   sql.Append(@" AND  A.TIPOASSUNTOID = 6 ");
               }

               if (Banheiro) //--7  DEPENDÊNCIAS - BANHEIRO 
               {
                   sql.Append(@" AND  A.TIPOASSUNTOID = 7 ");
               }

               contextQuery.Command = sql.ToString();

               contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

               reader = contexto.GetDataReader(contextQuery);

               while (reader.Read())
               {
                   retorno = Convert.ToInt32(reader["QUANTIDADE"]);
               }
           }
           finally
           {
               if (reader != null)
               {
                   reader.Close();
               }
           }

           return retorno;
       }

       public List<string> ListaSalasSemOpcaoPor(DataContext contexto, int campanhaId, string censo)
       {
           List<string> lista = new List<string>();
           SqlDataReader reader = null;
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @"SELECT DISTINCT RD.DEPENDENCIA
                                        FROM   INSPECAOESCOLAR.OPCOESASSUNTO O (NOLOCK) 
                                                INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                        ON O.ASSUNTOID = A.ASSUNTOID 
	                                            INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK)
	                                                    ON G.GRUPOID = A.GRUPOID
			                                    INNER JOIN INSPECAOESCOLAR.CAMPANHAESCOLA ce (NOLOCK)
	                                                    ON ce.campanhaid = g.CAMPANHAID
					                                    and ce.UNIDADE_ENS = @CENSO
			                                    INNER JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK)
					                                    ON RD.CAMPANHAESCOLAID = CE.CAMPANHAESCOLAID
					                                    AND RD.PLACAIDENTIFICACAO IS NOT NULL --SALA DE AULA
			                                    LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIAOPCAO RO (NOLOCK)
					                                    ON RO.RESPOSTADEPENDENCIAID = RD.RESPOSTADEPENDENCIAID
					                                    AND RO.OPCOESASSUNTOID = O.OPCOESASSUNTOID
                                        WHERE G.CAMPANHAID = @CAMPANHAID
	                                            AND  A.TIPOASSUNTOID = 6
	                                            AND RO.RESPOSTADEPENDENCIAOPCAOID IS NULL  ";

               contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
               contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

               reader = contexto.GetDataReader(contextQuery);

               while (reader.Read())
               {
                   string sala = Convert.ToString(reader["DEPENDENCIA"]);

                   lista.Add(sala);
               }

               return lista;
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               if (reader != null)
               {
                   reader.Close();
               }
           }
       }

       public List<string> ListaBanheirosSemOpcaoPor(DataContext contexto, int campanhaId, string censo)
       {
           List<string> lista = new List<string>();
           SqlDataReader reader = null;
           ContextQuery contextQuery = new ContextQuery();

           try
           {
               contextQuery.Command = @"SELECT DISTINCT RD.DEPENDENCIA
                                        FROM   INSPECAOESCOLAR.OPCOESASSUNTO O (NOLOCK) 
                                                INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                        ON O.ASSUNTOID = A.ASSUNTOID 
	                                            INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK)
	                                                    ON G.GRUPOID = A.GRUPOID
			                                    INNER JOIN INSPECAOESCOLAR.CAMPANHAESCOLA ce (NOLOCK)
	                                                    ON ce.campanhaid = g.CAMPANHAID
					                                    and ce.UNIDADE_ENS = @CENSO
			                                    INNER JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK)
					                                    ON RD.CAMPANHAESCOLAID = CE.CAMPANHAESCOLAID
					                                    AND RD.IDENTIFICACAODEPENDENCIAID IS NOT NULL --BANHEIRO
			                                    LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIAOPCAO RO (NOLOCK)
					                                    ON RO.RESPOSTADEPENDENCIAID = RD.RESPOSTADEPENDENCIAID
					                                    AND RO.OPCOESASSUNTOID = O.OPCOESASSUNTOID
                                        WHERE G.CAMPANHAID = @CAMPANHAID
	                                            AND  A.TIPOASSUNTOID = 7
	                                            AND RO.RESPOSTADEPENDENCIAOPCAOID IS NULL  ";

               contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
               contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

               reader = contexto.GetDataReader(contextQuery);

               while (reader.Read())
               {
                   string sala = Convert.ToString(reader["DEPENDENCIA"]);

                   lista.Add(sala);
               }

               return lista;
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               if (reader != null)
               {
                   reader.Close();
               }
           }
       }

       public void Atualiza(DataContext contexto, Entidades.RespostaDependenciaOpcao respostaDependenciaOpcao)
       {
           ContextQuery contextQuery = new ContextQuery();

           contextQuery.Command = @" UPDATE InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO
                                       SET OPCOESASSUNTOID = @OPCOESASSUNTOID, 
	                                       ACAODIRECAOID = @ACAODIRECAOID,
	                                       USUARIOID = @USUARIOID, 
                                           DATAALTERACAO = @DATAALTERACAO
                                     WHERE RESPOSTADEPENDENCIAOPCAOID = @RESPOSTADEPENDENCIAOPCAOID ";

           contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAOPCAOID", SqlDbType.Int, respostaDependenciaOpcao.RespostaDependenciaOpcaoId);
           contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, respostaDependenciaOpcao.OpcoesAssuntoId);
           contextQuery.Parameters.Add("@ACAODIRECAOID", SqlDbType.Int, respostaDependenciaOpcao.AcaoDirecaoId);
           contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, respostaDependenciaOpcao.UsuarioId);
           contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

           contexto.ApplyModifications(contextQuery);
       }

       public void Remove(DataContext contexto, int respostaDependenciaOpcaoId)
       {
           ContextQuery contextQuery = new ContextQuery();

           contextQuery.Command = @" DELETE InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO
                                     WHERE RESPOSTADEPENDENCIAOPCAOID = @RESPOSTADEPENDENCIAOPCAOID ";

           contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAOPCAOID", SqlDbType.Int, respostaDependenciaOpcaoId);

           contexto.ApplyModifications(contextQuery);
       }

       public void RemovePorDependencia(DataContext contexto, int respostaDependenciaId)
       {
           ContextQuery contextQuery = new ContextQuery();

           contextQuery.Command = @" DELETE InspecaoEscolar.RESPOSTADEPENDENCIAOPCAO
                                     WHERE RESPOSTADEPENDENCIAID = @RESPOSTADEPENDENCIAID ";

           contextQuery.Parameters.Add("@RESPOSTADEPENDENCIAID", SqlDbType.Int, respostaDependenciaId);

           contexto.ApplyModifications(contextQuery);
       }
   }
}

