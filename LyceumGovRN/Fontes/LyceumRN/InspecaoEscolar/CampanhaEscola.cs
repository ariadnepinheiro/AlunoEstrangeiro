using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
    public class CampanhaEscola
    {

        #region Listar

        public DataTable ListarCampanhaEscola(int campanhaId, string unidadeEns)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT   C.CAMPANHAID,C.ANO,C.SEMESTRE,C.TITULO 
                                            FROM   INSPECAOESCOLAR.CAMPANHAESCOLA C
                                            WHERE C.campanhaId=@campanhaId AND C.unidadeEns=@unidadeEns
                                                   ";

                contextQuery.Parameters.Add("@campanhaId", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@unidadeEns", SqlDbType.Text, unidadeEns);

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

        public RN.InspecaoEscolar.DTOs.DadosCampanhaEscola ObtemCampanhaEscola(int campanhaId, string unidadeEns)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                var campanhaEscola = ObtemPor(campanhaId, unidadeEns);
                var respostasDependencias = new RN.InspecaoEscolar.RespostaDependencia().ListarRespostaDependencia(unidadeEns, campanhaId);
                var respostasDependenciasOpcoes = new RN.InspecaoEscolar.RespostaDependenciaOpcao().ListarRespostaDependenciaOpcao(campanhaId, unidadeEns);

                if (campanhaEscola != null)
                {
                    var dce = new RN.InspecaoEscolar.DTOs.DadosCampanhaEscola
                    {
                        CAMPANHAESCOLAID = campanhaEscola.CampanhaEscolaId,
                        CAMPANHAID = campanhaEscola.CampanhaId,
                        UNIDADE_ENS = campanhaEscola.Unidade_Ens,
                        USUARIOID = campanhaEscola.UsuarioId,
                        DATAALTERACAO = campanhaEscola.DataAlteracao,
                        DATACADASTRO = campanhaEscola.DataCadastro,
                        FINALIZADO = campanhaEscola.Finalizado,
                        DATAFINALIZACAO = campanhaEscola.DataFinalizacao,
                    };

                    foreach (DataRow rd in respostasDependencias.Rows)
                    {
                        var respostaDependencia = new RN.InspecaoEscolar.DTOs.DadosCampanhaEscola.DadosRespostaDependencia
                        {
                            RESPOSTADEPENDENCIAID = Convert.ToInt32(rd["RESPOSTADEPENDENCIAID"]),
                            CAMPANHAESCOLAID = Convert.ToInt32(rd["CAMPANHAESCOLAID"]),
                            DEPENDENCIA = Convert.ToString(rd["DEPENDENCIA"]),
                            FACULDADE = Convert.ToString(rd["FACULDADE"]),
                            PLACAIDENTIFICACAO = rd["PLACAIDENTIFICACAO"] != DBNull.Value ? Convert.ToBoolean(rd["PLACAIDENTIFICACAO"]) : (bool?)null,
                            IDENTIFICACAODEPENDENCIAID = rd["IDENTIFICACAODEPENDENCIAID"] != DBNull.Value ? Convert.ToInt32(rd["IDENTIFICACAODEPENDENCIAID"]) : (int?)null,
                            USUARIOID = Convert.ToString(rd["USUARIOID"]),
                            DATACADASTRO = Convert.ToDateTime(rd["DATACADASTRO"]),
                            DATAALTERACAO = Convert.ToDateTime(rd["DATAALTERACAO"]),
                        };

                        foreach (DataRow rdo in respostasDependenciasOpcoes.Rows)
                        {
                            if (respostaDependencia.RESPOSTADEPENDENCIAID != Convert.ToInt32(rdo["RESPOSTADEPENDENCIAID"]))
                                continue;

                            var respostaDependenciaOpcao = new RN.InspecaoEscolar.DTOs.DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao
                            {
                                RESPOSTADEPENDENCIAOPCAOID = Convert.ToInt32(rdo["RESPOSTADEPENDENCIAOPCAOID"]),
                                RESPOSTADEPENDENCIAID = Convert.ToInt32(rdo["RESPOSTADEPENDENCIAID"]),
                                OPCOESASSUNTOID = Convert.ToInt32(rdo["OPCOESASSUNTOID"]),
                                ACAODIRECAOID = Convert.ToInt32(rdo["ACAODIRECAOID"]),
                                USUARIOID = Convert.ToString(rdo["USUARIOID"]),
                                DATACADASTRO = Convert.ToDateTime(rdo["DATACADASTRO"]),
                                DATAALTERACAO = Convert.ToDateTime(rdo["DATAALTERACAO"]),
                            };

                            respostaDependencia.RespostasDependenciasOpcoes.Add(respostaDependenciaOpcao);
                        }

                        dce.RespostasDependencias.Add(respostaDependencia);
                    }

                    return dce;
                }

                return null;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public DataTable ListaSalaAulaPor(int campanhaId, string unidadeEns)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ListaSalaAulaPor(contexto, campanhaId, unidadeEns);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public DataTable ListaSalaAulaPor(DataContext contexto, int campanhaId, string unidadeEns)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DEP.DEPENDENCIA, 
                                               DEP.FACULDADE,
                                               @CAMPANHAID AS CAMPANHAID,
                                               EDIFICACAO, 
                                               PAVIMENTO, 
                                               CE.CAMPANHAESCOLAID, 
                                               RD.RESPOSTADEPENDENCIAID 
                                        FROM   LY_DEPENDENCIA DEP (NOLOCK) 
                                               LEFT JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK) 
                                                      ON DEP.FACULDADE = CE.UNIDADE_ENS 
                                                         AND CE.CAMPANHAID = @CAMPANHAID 
                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK) 
                                                      ON DEP.DEPENDENCIA = RD.DEPENDENCIA 
                                                         AND CE.CAMPANHAESCOLAID = RD.CAMPANHAESCOLAID 
                                        WHERE  CAD_SALA_AULA = 'S' 
                                               AND ATIVA = 'S' 
                                               AND TIPO_DEPEND = 'SALA' 
                                               AND DEP.FACULDADE = @CENSO  
                                        ORDER  BY DEP.DEPENDENCIA  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, unidadeEns);
                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

                return contexto.GetDataTable(contextQuery);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DataTable ListaBanheiroPor(int campanhaId, string unidadeEns)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ListaBanheiroPor(contexto, campanhaId, unidadeEns);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public int ObtemUltimaCampanhaFinalizadaPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @"  SELECT MAX(CAMPANHAID) as CAMPANHAID
                                                FROM   [INSPECAOESCOLAR].[CAMPANHAESCOLA]  (NOLOCK)                                                        
                                                WHERE  UNIDADE_ENS = @UNIDADE_ENS
												AND FINALIZADO = 1 ";

                contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["CAMPANHAID"] != DBNull.Value ? Convert.ToInt32(reader["CAMPANHAID"]) : 0;
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }

            return retorno;
        }

        public DataTable ListaBanheiroPor(DataContext contexto, int campanhaId, string unidadeEns)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DEP.DEPENDENCIA,TP.NOME TIPO_BANHEIRO, 
                                               DEP.FACULDADE,
                                               @CAMPANHAID AS CAMPANHAID,
                                               EDIFICACAO, 
                                               PAVIMENTO, 
                                               CE.CAMPANHAESCOLAID, 
                                               RD.RESPOSTADEPENDENCIAID 
                                        FROM   LY_DEPENDENCIA DEP (NOLOCK) 
                                               LEFT JOIN LY_TIPO_DEPENDENCIA TP 
										       ON DEP.TIPO_DEPEND=TP.TIPO_DEPEND
                                               LEFT JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK) 
                                                      ON DEP.FACULDADE = CE.UNIDADE_ENS 
                                                         AND CE.CAMPANHAID = @CAMPANHAID 
                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK) 
                                                      ON DEP.DEPENDENCIA = RD.DEPENDENCIA 
                                                         AND CE.CAMPANHAESCOLAID = RD.CAMPANHAESCOLAID 
                                        WHERE  -- CAD_SALA_AULA = 'S' AND
                                                ATIVA = 'S' 
                                                 AND DEP.TIPO_DEPEND LIKE 'BANHEIRO%' 
                                               AND DEP.FACULDADE = @CENSO  
                                        ORDER  BY DEP.DEPENDENCIA  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, unidadeEns);
                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

                return contexto.GetDataTable(contextQuery);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        //        public List<RN.DTOs.DadosRelatorioDependencia> ListaBanheiroPor(int campanhaId, string unidadeEns)
        //        {
        //            List<RN.DTOs.DadosRelatorioDependencia> lista = new List<RN.DTOs.DadosRelatorioDependencia>();
        //            RN.DTOs.DadosRelatorioDependencia dados = new RN.DTOs.DadosRelatorioDependencia();
        //            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
        //            SqlDataReader reader = null;
        //            ContextQuery contextQuery = new ContextQuery();

        //            try
        //            {
        //                contextQuery.Command = @" SELECT DEP.DEPENDENCIA,TP.NOME TIPO_BANHEIRO, 
        //                                               DEP.FACULDADE,
        //                                               @CAMPANHAID AS CAMPANHAID,
        //                                               EDIFICACAO, 
        //                                               PAVIMENTO, 
        //                                               CE.CAMPANHAESCOLAID, 
        //                                               RD.RESPOSTADEPENDENCIAID 
        //                                        FROM   LY_DEPENDENCIA DEP (NOLOCK) 
        //                                               LEFT JOIN LY_TIPO_DEPENDENCIA TP 
        //										       ON DEP.TIPO_DEPEND=TP.TIPO_DEPEND
        //                                               LEFT JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK) 
        //                                                      ON DEP.FACULDADE = CE.UNIDADE_ENS 
        //                                                         AND CE.CAMPANHAID = @CAMPANHAID 
        //                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTADEPENDENCIA RD (NOLOCK) 
        //                                                      ON DEP.DEPENDENCIA = RD.DEPENDENCIA 
        //                                                         AND CE.CAMPANHAESCOLAID = RD.CAMPANHAESCOLAID 
        //                                        WHERE  CAD_SALA_AULA = 'S' 
        //                                               AND ATIVA = 'S' 
        //                                                 AND DEP.TIPO_DEPEND LIKE 'BANHEIRO%' 
        //                                               AND DEP.FACULDADE = @CENSO  
        //                                        ORDER  BY DEP.DEPENDENCIA  ";

        //                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, unidadeEns);
        //                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

        //                reader = contexto.GetDataReader(contextQuery);

        //                while (reader.Read())
        //                {
        //                    dados = new Techne.Lyceum.RN.DTOs.DadosRelatorioDependencia();

        //                    dados.Dependencia = Convert.ToString(reader["DEPENDENCIA"]);
        //                    dados.UnidadeEnsino = Convert.ToString(reader["FACULDADE"]);
        //                    dados.CampanhaId = Convert.ToInt32(reader["CAMPANHAID"]);
        //                    dados.Edificacao = Convert.ToString(reader["EDIFICACAO"]);
        //                    dados.Pavimento = Convert.ToString(reader["PAVIMENTO"]);
        //                    dados.Tipo_Banheiro = Convert.ToString(reader["TIPO_BANHEIRO"]);
        //                    if (reader["CAMPANHAESCOLAID"] != DBNull.Value)
        //                    {
        //                        dados.CampanhaEscolaId = Convert.ToInt32(reader["CAMPANHAESCOLAID"]);
        //                    }

        //                    if (reader["RESPOSTADEPENDENCIAID"] != DBNull.Value)
        //                    {
        //                        dados.CampanhaEscolaId = Convert.ToInt32(reader["RESPOSTADEPENDENCIAID"]);
        //                    }


        //                    lista.Add(dados);
        //                }

        //                return lista;
        //            }
        //            catch (Exception ex)
        //            {
        //                string mensagem = string.Empty;
        //                contexto.Abandon();
        //                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
        //                {
        //                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
        //                        Environment.NewLine,
        //                        Convert.ToString(ex.Message));
        //                }
        //                else
        //                {
        //                    mensagem = Convert.ToString(ex.Message);
        //                }
        //                throw new Exception(mensagem);
        //            }
        //            finally
        //            {
        //                if (reader != null)
        //                {
        //                    reader.Close();
        //                }
        //                contexto.Dispose();
        //            }
        //        }

        public int ObtemCampanhaEscolaId(int campanhaId, string unidadeEns)
        {
            using (DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                return ObtemPor(contexto, campanhaId, unidadeEns).CampanhaEscolaId;
            }
        }

        public bool ExisteCampanhaEscola(int campanhaId, string unidadeEns)
        {
            using (DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                return ExisteCampanhaEscola(contexto, campanhaId, unidadeEns);
            }
        }

        public bool ExisteCampanhaEscola(DataContext contexto, int campanhaId, string unidadeEns)
        {
            ContextQuery contextQuery = new ContextQuery();
            Boolean retorno = false;

            contextQuery.Command = @" SELECT COUNT(0) EXISTE  
                                            FROM   INSPECAOESCOLAR.CAMPANHAESCOLA C
                                            WHERE C.CAMPANHAID=@CAMPANHAID 
                                                    AND C.UNIDADE_ENS=@UNIDADEENS
                                                   ";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
            contextQuery.Parameters.Add("@UNIDADEENS", SqlDbType.VarChar, unidadeEns);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool? RetornaPossuiAcervoPor(DataContext contexto, int campanhaEscolaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            bool? retorno = null;

            try
            {
                contextQuery.Command = @" SELECT POSSUIACERVO   
                                            FROM   INSPECAOESCOLAR.CAMPANHAESCOLA (NOLOCK)
                                            WHERE CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

                contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["POSSUIACERVO"] != DBNull.Value)
                    {
                        retorno = Convert.ToBoolean(reader["POSSUIACERVO"]);
                    }
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool EhCampanhaEscolaFinalizadaPor(DataContext contexto, int campanhaId, string unidadeEns)
        {
            ContextQuery contextQuery = new ContextQuery();
            Boolean retorno = false;

            contextQuery.Command = @" SELECT COUNT(0)   
                                            FROM   INSPECAOESCOLAR.CAMPANHAESCOLA (NOLOCK)
                                            WHERE CAMPANHAID = @CAMPANHAID 
                                                    AND UNIDADE_ENS = @UNIDADE_ENS
													AND FINALIZADO = 1 ";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
            contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.VarChar, unidadeEns);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }


            return retorno;
        }

        public bool EhCampanhaEscolaFinalizadaPor(DataContext contexto, int campanhaEscolaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            Boolean retorno = false;

            contextQuery.Command = @" SELECT COUNT(0)   
                                            FROM   INSPECAOESCOLAR.CAMPANHAESCOLA (NOLOCK)
                                            WHERE CAMPANHAESCOLAID = @CAMPANHAESCOLAID 
													AND FINALIZADO = 1 ";

            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }


            return retorno;
        }

        public List<string> ListaAssuntoSemRespostaPor(DataContext contexto, int campanhaId, string censo)
        {
            List<string> lista = new List<string>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT CASE 
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 1 AND a.ORDEM BETWEEN 3 AND 6 THEN 'Condições de Acesso 01' --//GRUPO 1 ABA1 - Assuntos 3 a 6
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 1 AND a.ORDEM BETWEEN 7 AND 10 THEN 'Condições de Acesso 02' --//GRUPO 1 ABA2 - Assuntos 7 a 10
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 1 AND a.ORDEM BETWEEN 10 AND 14 THEN 'Condições de Acesso 03' --//GRUPO 1 ABA3 - Assuntos 10 a 14
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 1 AND a.ORDEM BETWEEN 15 AND 18 THEN 'Condições de Acesso 04' --//GRUPO 1 ABA4 - Assuntos 15 a 18
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 1 AND a.ORDEM BETWEEN 19 AND 23 THEN 'Condições de Acesso 05' --//GRUPO 1 ABA5 - Assuntos 19 a 23
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 1 AND a.ORDEM >= 24 THEN 'Condições de Acesso 06' --//GRUPO 1 ABA6 - Assuntos da 24 até o final
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 2 THEN 'Alimentação Escolar'--//GRUPO 2 ABA1 - Assuntos todos
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 3 AND a.ORDEM BETWEEN 1 AND 11 THEN 'Tecnologia da Informação 01' --//GRUPO 3 ABA1 - Assuntos 1 a 11
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 3 AND a.ORDEM >= 12 THEN 'Tecnologia da Informação 02'--//GRUPO 3 ABA2 - Assuntos da 12 até o final
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 4 AND a.ORDEM BETWEEN 1 AND 13 THEN 'Situações Excepcionais 01' --//GRUPO 4 ABA1 - Assuntos 1 a 13
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 4 AND a.ORDEM >= 14 THEN 'Situações Excepcionais 02'--//GRUPO 4 ABA2 - Assuntos da 14 até o final
													WHEN a.TIPOASSUNTOID in (2, 3, 4) and g.ORDEM = 5 THEN 'Sala de Recursos Multifuncionais'--//GRUPO 5 ABA1 - Assuntos da 01 até o final
													when a.TIPOASSUNTOID in (8, 9, 10) then 'Considerações Finais'
												END ABA,
												G.DESCRICAO AS GRUPO, 
												A.DESCRICAO AS ASSUNTO
                                        FROM   INSPECAOESCOLAR.ASSUNTO A (NOLOCK)
                                               INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK)
                                                       ON A.GRUPOID = G.GRUPOID 
                                               INNER JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK)
                                                       ON G.CAMPANHAID = CE.CAMPANHAID 
                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTAASSUNTO R (NOLOCK)
                                                      ON A.ASSUNTOID = R.ASSUNTOID 
                                                         AND R.CAMPANHAESCOLAID = CE.CAMPANHAESCOLAID 
                                        WHERE  G.CAMPANHAID = @CAMPANHAID 
                                               AND CE.UNIDADE_ENS = @CENSO 
                                               AND R.RESPOSTAASSUNTOID IS NULL 
											   AND A.TIPOASSUNTOID IN ( 2, 3, 4, 8, 9, 10 ) 
										ORDER BY G.ORDEM, A.ORDEM ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    string aba = Convert.ToString(reader["ABA"]);
                    string grupo = Convert.ToString(reader["GRUPO"]);
                    string assunto = Convert.ToString(reader["ASSUNTO"]);

                    string mensagem = string.Format("Não é possivel finalizar, pois na aba {0} - {1}, a pergunta: '{2}' precisa estar respondida.", aba, grupo, assunto);
                    lista.Add(mensagem);
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

        //        public bool PossuiAssuntoSemRespostaPor(DataContext contexto, int campanhaId, string unidadeEns, bool consideracoesFinais, bool demais)
        //        {
        //            ContextQuery contextQuery = new ContextQuery();
        //            Boolean retorno = false;
        //            StringBuilder sql = new StringBuilder();

        //            sql.Append(@" SELECT COUNT(*) 
        //                                        FROM   INSPECAOESCOLAR.ASSUNTO A (NOLOCK)
        //                                               INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK)
        //                                                       ON A.GRUPOID = G.GRUPOID 
        //                                               INNER JOIN INSPECAOESCOLAR.CAMPANHAESCOLA CE (NOLOCK)
        //                                                       ON G.CAMPANHAID = CE.CAMPANHAID 
        //                                               LEFT JOIN INSPECAOESCOLAR.RESPOSTAASSUNTO R (NOLOCK)
        //                                                      ON A.ASSUNTOID = R.ASSUNTOID 
        //                                                         AND R.CAMPANHAESCOLAID = CE.CAMPANHAESCOLAID 
        //                                        WHERE  G.CAMPANHAID = @CAMPANHAID 
        //                                               AND CE.UNIDADE_ENS = @UNIDADE_ENS 
        //                                               AND R.RESPOSTAASSUNTOID IS NULL 
        //                                               ");

        //            if (consideracoesFinais)
        //            {
        //                sql.Append(@" AND A.TIPOASSUNTOID IN ( 8, 9, 10 ) 
        //                                         ");
        //            }

        //            if (demais)
        //            {
        //                sql.Append(@" AND A.TIPOASSUNTOID IN ( 2, 3, 4 )
        //                                         ");
        //            }

        //            contextQuery.Command = sql.ToString();

        //            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
        //            contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.Int, unidadeEns);

        //            if (contexto.GetReturnValue<int>(contextQuery) > 0)
        //            {
        //                retorno = true;
        //            }


        //            return retorno;
        //        }


        #endregion

        #region Inserir

        //Métodos para inserir
        //Método  public ValidacaoDados validaxxxx para Validar inserção
        //Método  bool para fazer a checagem no banco

        public int Insere(DataContext contexto, Entidades.CampanhaEscola campanhaEscola)
        {
            ContextQuery contextQuery = new ContextQuery();
            contextQuery.Command = @" INSERT INTO InspecaoEscolar.CAMPANHAESCOLA
                                                (CAMPANHAID
                                                ,UNIDADE_ENS
                                                ,POSSUIACERVO
                                                ,USUARIOID
                                                ,DATACADASTRO
                                                ,DATAALTERACAO) 
                                    VALUES      (@CAMPANHAID
                                                ,@UNIDADE_ENS
                                                ,@POSSUIACERVO
                                                ,@USUARIOID
                                                ,@DATACADASTRO
                                                ,@DATAALTERACAO)
                    
                    SELECT IDENT_CURRENT('InspecaoEscolar.CAMPANHAESCOLA') ";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaEscola.CampanhaId);
            contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.VarChar, campanhaEscola.Unidade_Ens);
            contextQuery.Parameters.Add("@POSSUIACERVO", SqlDbType.Bit, campanhaEscola.PossuiAcervo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, campanhaEscola.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            campanhaEscola.CampanhaEscolaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            return campanhaEscola.CampanhaEscolaId;
        }

        public void AtualizaPossuiAcervo(DataContext contexto, int campanhaEscolaId, bool possuiAcervo, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE InspecaoEscolar.CAMPANHAESCOLA
                                        SET POSSUIACERVO = @POSSUIACERVO,
                                            USUARIOID = @USUARIOID, 
                                            DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  CAMPANHAESCOLAID = @CAMPANHAESCOLAID  ";

            contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscolaId);
            contextQuery.Parameters.Add("@POSSUIACERVO", SqlDbType.Bit, possuiAcervo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
        #endregion

        public ValidacaoDados ValidaFinalizacao(Entidades.CampanhaEscola campanha, bool finalizacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Assunto rnAssunto = new Assunto();
            RespostaDependencia rnRespostaDependencia = new RespostaDependencia();
            RespostaDependenciaOpcao rnRespostaDependenciaOpcao = new RespostaDependenciaOpcao();
            OpcoesAssunto rnOpcoesAssunto = new OpcoesAssunto();
            ICollection<RN.InspecaoEscolar.Entidades.Assunto> assuntos = new List<RN.InspecaoEscolar.Entidades.Assunto>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (campanha == null)
            {
                return validacaoDados;
            }

            if (campanha.CampanhaEscolaId <= 0)
            {
                mensagens.Add("Não é possivel finalizar, pois nenhuma aba foi respondida.");
            }

            if (campanha.CampanhaId <= 0)
            {
                mensagens.Add("Campo CAMPANHA é obrigatório.");
            }

            if (campanha.Unidade_Ens.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }
            if (finalizacao)
            {
                if (campanha.DataFinalizacao == null || campanha.DataFinalizacao == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA FINALIZAÇÃO é obrigatório.");
                }

                if (campanha.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo USUÁRIO é obrigatório.");
                }
            }
            else
            {
                if (campanha.ConsideracaoFinal.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CONSIDERAÇÃO FINAL é obrigatório.");
                }

                if (campanha.ConsideracaoFinal.Length > 2000)
                {
                    mensagens.Add("Campo CONSIDERAÇÃO FINAL ultrapassou o limite de 2000 caracteres.");
                }

                if (campanha.Aceito == null)
                {
                    mensagens.Add("Campo O DIRETOR(A) RATIFICA O PREENCHIMENTO DO RT? é obrigatório.");
                }

                if (campanha.UsuarioAceiteId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo USUÁRIO é obrigatório.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verfica se é um aceite de diretor ou finalizacão
                    if (!finalizacao)
                    {
                        //Em caso de aceite de diretor verifica se a campanha já foi finalizada
                        if (!this.EhCampanhaEscolaFinalizadaPor(contexto, campanha.CampanhaEscolaId) && finalizacao)
                        {
                            mensagens.Add("Para efetuar a ratificação da CAMPANHA/ESCOLA precisa estar finalizada.");
                        }
                    }
                    else
                    {
                        //Em caso de finalização:

                        //Verifica se já está finalizada
                        if (this.EhCampanhaEscolaFinalizadaPor(contexto, campanha.CampanhaEscolaId))
                        {
                            mensagens.Add("Esta CAMPANHA / ESCOLA já foi finalizada.");
                        }
                        else
                        {
                            //Verifica quantidade de salas
                            DataTable salas = this.ListaSalaAulaPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens);

                            //Verifica quantidade de respostas
                            int respostasSalas = rnRespostaDependencia.RetornaQuantidadePor(contexto, campanha.CampanhaEscolaId, true, false);

                            List<string> problemasSalas = new List<string>();

                            //Verifica se não tem resposta para todas as dependencias
                            if (respostasSalas < salas.Rows.Count)
                            {
                                //Buscar salas sem respostas 
                                problemasSalas.AddRange(rnRespostaDependencia.ListaSalasSemRespostaPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens));
                            }

                            //Busca quantidade de oções
                            int opcoesSalas = rnOpcoesAssunto.RetornaQuantidadePor(contexto, campanha.CampanhaId, true, false);

                            //Verifica quantidade de respostas das opções
                            int respostasOpcaoSalas = rnRespostaDependenciaOpcao.RetornaQuantidadePor(contexto, campanha.CampanhaEscolaId, true, false);

                            //Verifica se não tem resposta opção para todas as dependencias
                            if (respostasOpcaoSalas < (salas.Rows.Count * opcoesSalas))
                            {
                                //Buscar salas sem respostas 
                                problemasSalas.AddRange(rnRespostaDependenciaOpcao.ListaSalasSemOpcaoPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens));
                            }

                            if (problemasSalas.Count > 0)
                            {
                                mensagens.Add(string.Format("Não é possivel finalizar, pois na aba SALA DE AULA a(s) sala(s) {0} precisa(m) estar totalmente respondida.", problemasSalas.Distinct().Aggregate((x, y) => x + ", " + y)));
                            }

                            //Verifica quantidade de banheiros
                            DataTable banheiros = this.ListaBanheiroPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens);

                            //Verifica quantidade de respostas
                            int respostasBanheiros = rnRespostaDependencia.RetornaQuantidadePor(contexto, campanha.CampanhaEscolaId, false, true);

                            List<string> problemasBanheiros = new List<string>();

                            //Verifica se não tem resposta para todas as dependencias
                            if (respostasBanheiros < banheiros.Rows.Count)
                            {
                                //Buscar banheiros sem respostas 
                                problemasBanheiros.AddRange(rnRespostaDependencia.ListaBanheirosSemRespostaPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens));
                            }

                            //Busca quantidade de oções
                            int opcoesBanheiros = rnOpcoesAssunto.RetornaQuantidadePor(contexto, campanha.CampanhaId, false, true);

                            //Verifica quantidade de respostas das opções
                            int respostasOpcaoBanheiros = rnRespostaDependenciaOpcao.RetornaQuantidadePor(contexto, campanha.CampanhaEscolaId, false, true);

                            //Verifica se não tem resposta opção para todas as dependencias
                            if (respostasOpcaoBanheiros < (banheiros.Rows.Count * opcoesBanheiros))
                            {
                                //Buscar baheiros sem respostas
                                problemasBanheiros.AddRange(rnRespostaDependenciaOpcao.ListaBanheirosSemOpcaoPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens));
                            }

                            if (problemasBanheiros.Count > 0)
                            {
                                mensagens.Add(string.Format("Não é possivel finalizar, pois na aba BANHEIROS o(s) banheiro(s) {0} precisa(m) estar totalmente respondido.", problemasBanheiros.Distinct().Aggregate((x, y) => x + ", " + y)));
                            }

                            //Verifica se a campanha / escola já possui informação de acervo
                            bool exibeAbaInspecaoEscolar = true; //TODO: Buscar na Campanha
                            if (exibeAbaInspecaoEscolar)
                            {
                                bool? possuiAcervo = this.RetornaPossuiAcervoPor(contexto, campanha.CampanhaEscolaId);
                                if (possuiAcervo == null)
                                {
                                    mensagens.Add("Não é possivel finalizar, pois a aba INSPEÇÃO ESCOLAR precisa estar totalmente respondida.");
                                }
                            }

                            //Busca assusutnos sem respostas
                            List<string> problemasAssuntos = this.ListaAssuntoSemRespostaPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens);

                            if (problemasAssuntos.Count() > 0)
                            {
                                mensagens.AddRange(problemasAssuntos);
                            }

                            ////Verifica respostas dos assuntos de demais dependencias
                            //if (this.PossuiAssuntoSemRespostaPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens, false, true))
                            //{
                            //    mensagens.Add("Não é possivel finalizar, pois as abas CONDIÇÕES DE ACESSO/ALIMENTAÇÃO ESCOLAR/TECNOLOGIA DA INFORMAÇÃO/SITUAÇÕES EXCEPCIONAIS precisam estar totalmente respondida.");
                            //}

                            ////Verifica respostas dos assuntos de consideraçoes finais
                            //if (this.PossuiAssuntoSemRespostaPor(contexto, campanha.CampanhaId, campanha.Unidade_Ens, true, false))
                            //{
                            //    mensagens.Add("Não é possivel finalizar, pois a aba CONSIDERAÇÕES FINAIS precisa estar totalmente respondida.");
                            //}

                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public bool Aceita(Entidades.CampanhaEscola campanhaEscola)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @" UPDATE InspecaoEscolar.CAMPANHAESCOLA
                                        SET CONSIDERACAOFINAL = @CONSIDERACAOFINAL,
                                            ACEITO = @ACEITO,
                                            DATAACEITE = @DATAACEITE,
                                            USUARIOACEITEID = @USUARIOACEITEID,
                                            FINALIZADO = @FINALIZADO, 
                                            DATAFINALIZACAO = @DATAFINALIZACAO   
                                        WHERE  CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

                contextQuery.Parameters.Add("@CONSIDERACAOFINAL", SqlDbType.VarChar, campanhaEscola.ConsideracaoFinal);
                contextQuery.Parameters.Add("@ACEITO", SqlDbType.Bit, campanhaEscola.Aceito);
                contextQuery.Parameters.Add("@DATAACEITE", SqlDbType.DateTime, campanhaEscola.DataAceite);
                contextQuery.Parameters.Add("@USUARIOACEITEID", SqlDbType.VarChar, campanhaEscola.UsuarioAceiteId);
                contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, campanhaEscola.DataFinalizacao);
                contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, campanhaEscola.Finalizado);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanhaEscola.CampanhaEscolaId);

                contexto.ApplyModifications(contextQuery);
                retorno = true;
            }
            catch (Exception ex)
            {

                string mensagem = string.Empty;
                contexto.Abandon();


                if (ex.Message == "Error executing ContextQuery!")
                {

                    if (Convert.ToString(ex.InnerException.Message).Contains("Violation of UNIQUE KEY constraint 'GrupoUnique'."))
                        mensagem = "Não é possível cadastrar uma ordem repetida para a mesma campanha.";


                    if (Convert.ToString(ex.InnerException.Message).Contains("Violation of UNIQUE KEY constraint 'GrupoDescricaoUnique'."))
                        mensagem = "Não é possível cadastrar uma descrição repetida para a mesma campanha.";

                }
                else if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        public void Finaliza(Entidades.CampanhaEscola campanha)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE InspecaoEscolar.CAMPANHAESCOLA SET    
                                               FINALIZADO = @FINALIZADO, 
                                               DATAFINALIZACAO = @DATAFINALIZACAO,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

                contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, campanha.DataFinalizacao);
                contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, campanha.Finalizado);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, campanha.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanha.CampanhaEscolaId);

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
                ctx.Dispose();
            }
        }

        public Entidades.CampanhaEscola ObtemPor(int campanhaId, string unidade)
        {
            Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                campanhaEscola = this.ObtemPor(contexto, campanhaId, unidade);
                return campanhaEscola;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public Entidades.CampanhaEscola ObtemPor(DataContext contexto, int campanhaId, string unidade)
        {
            Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                                FROM   [InspecaoEscolar].[CAMPANHAESCOLA]  (NOLOCK)                                                        
                                                WHERE  CAMPANHAID = @CAMPANHAID 
                                                       AND UNIDADE_ENS = @UNIDADE_ENS  ";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
            contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.VarChar, unidade);

            campanhaEscola = contexto.TryToBindEntity<Entidades.CampanhaEscola>(contextQuery);

            return campanhaEscola;
        }

        public List<DadosRelatorioInspecaoGrupo> ObtemListaConsideracoesFinaisPor(int campanhaId)
        {
            List<DadosRelatorioInspecaoGrupo> lista = new List<DadosRelatorioInspecaoGrupo>();
            DadosRelatorioInspecaoGrupo dado = new DadosRelatorioInspecaoGrupo();
            Grupo rnGrupo = new Grupo();
            Assunto rnAssunto = new Assunto();
            OpcoesAssunto rnOpcoesAssunto = new OpcoesAssunto();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                lista = rnGrupo.ObtemDadosGrupoConsideracoesFinaisPor(contexto, campanhaId);

                foreach (DadosRelatorioInspecaoGrupo grupo in lista)
                {
                    grupo.ListaAssunto = rnAssunto.ObtemDadosAssuntoPor(contexto, grupo.GrupoId, true, false, null, null);

                    foreach (DadosRelatorioInspecaoAssunto assunto in grupo.ListaAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(contexto, assunto.AssuntoId);
                    }
                }

                return lista;

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public List<DadosRelatorioInspecaoGrupo> ObtemListaDemaisDependenciasPor(int campanhaId)
        {
            List<DadosRelatorioInspecaoGrupo> lista = new List<DadosRelatorioInspecaoGrupo>();
            DadosRelatorioInspecaoGrupo dado = new DadosRelatorioInspecaoGrupo();
            Grupo rnGrupo = new Grupo();
            Assunto rnAssunto = new Assunto();
            OpcoesAssunto rnOpcoesAssunto = new OpcoesAssunto();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                lista = rnGrupo.ObtemDadosGrupoPor_DemaisDependencias(contexto, campanhaId);

                foreach (DadosRelatorioInspecaoGrupo grupo in lista)
                {
                    grupo.ListaAssunto = rnAssunto.ObtemDadosAssuntoPor(contexto, grupo.GrupoId, false, true, null, null);

                    foreach (DadosRelatorioInspecaoAssunto assunto in grupo.ListaAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(contexto, assunto.AssuntoId);
                    }
                }

                return lista;

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public List<DadosRelatorioInspecaoGrupo> ObtemListaDemaisDependenciasPor(int campanhaId, int ordemGrupo, int ordemAssuntoInicio, int ordemAssuntoFim)
        {
            List<DadosRelatorioInspecaoGrupo> lista = new List<DadosRelatorioInspecaoGrupo>();
            DadosRelatorioInspecaoGrupo dado = new DadosRelatorioInspecaoGrupo();
            Grupo rnGrupo = new Grupo();
            Assunto rnAssunto = new Assunto();
            OpcoesAssunto rnOpcoesAssunto = new OpcoesAssunto();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                lista = rnGrupo.ObtemDadosGrupoPor_DemaisDependencias(contexto, campanhaId, ordemGrupo);

                foreach (DadosRelatorioInspecaoGrupo grupo in lista)
                {
                    grupo.ListaAssunto = rnAssunto.ObtemDadosAssuntoPor(contexto, grupo.GrupoId, false, true, ordemAssuntoInicio, ordemAssuntoFim);

                    foreach (DadosRelatorioInspecaoAssunto assunto in grupo.ListaAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(contexto, assunto.AssuntoId);
                    }
                }

                return lista;

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public ValidacaoDados ValidaReabertura(Entidades.CampanhaEscola campanha, bool finalizacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Assunto rnAssunto = new Assunto();
            RespostaDependencia rnRespostaDependencia = new RespostaDependencia();
            RespostaDependenciaOpcao rnRespostaDependenciaOpcao = new RespostaDependenciaOpcao();
            OpcoesAssunto rnOpcoesAssunto = new OpcoesAssunto();
            ICollection<RN.InspecaoEscolar.Entidades.Assunto> assuntos = new List<RN.InspecaoEscolar.Entidades.Assunto>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (campanha == null)
            {
                return validacaoDados;
            }

            if (campanha.CampanhaId <= 0)
            {
                mensagens.Add("Campo CAMPANHA é obrigatório.");
            }

            if (campanha.Unidade_Ens.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verfica se é um aceite de diretor ou finalizacão
                    if (!finalizacao)
                    {
                        //Em caso de aceite de diretor verifica se a campanha já foi finalizada
                        if (!this.EhCampanhaEscolaFinalizadaPor(contexto, campanha.CampanhaEscolaId) && finalizacao)
                        {
                            mensagens.Add("Para efetuar a reabertura da CAMPANHA/ESCOLA precisa estar finalizada.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Reabri(Entidades.CampanhaEscola campanha)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE InspecaoEscolar.CAMPANHAESCOLA SET    
                                               FINALIZADO = @FINALIZADO, 
                                               DATAFINALIZACAO = @DATAFINALIZACAO,
                                               ACEITO = @ACEITO,
                                               DATAACEITE = @DATAACEITE,
                                               USUARIOACEITEID = @USUARIOACEITEID,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  CAMPANHAESCOLAID = @CAMPANHAESCOLAID ";

                contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, campanha.DataFinalizacao);
                contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, campanha.Finalizado);
                contextQuery.Parameters.Add("@ACEITO", SqlDbType.Bit, campanha.Aceito);
                contextQuery.Parameters.Add("@DATAACEITE", SqlDbType.DateTime, campanha.DataAceite);
                contextQuery.Parameters.Add("@USUARIOACEITEID", SqlDbType.VarChar, campanha.UsuarioAceiteId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, campanha.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@CAMPANHAESCOLAID", SqlDbType.Int, campanha.CampanhaEscolaId);

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
                ctx.Dispose();
            }
        }
    }
}
