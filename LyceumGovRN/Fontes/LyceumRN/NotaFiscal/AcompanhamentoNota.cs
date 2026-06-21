using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;


namespace Techne.Lyceum.RN.NotaFiscal
{
    public class AcompanhamentoNota
    {
        public DataTable ListaPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ACOMPANHAMENTONOTAID, 
                                               CENSO, 
                                               PROCESSO, 
                                               DATAPROCESSO, 
                                               CHAVEACESSO, 
                                               VALIDO, 
                                               A.USUARIOID, 
                                               A.DATACADASTRO, 
                                               A.DATAALTERACAO, 
                                               U.NOME 
                                        FROM   PrestacaoContas.ACOMPANHAMENTONOTA A (NOLOCK) 
                                               LEFT JOIN HADES.DBO.HD_USUARIO U (NOLOCK) 
                                                      ON A.USUARIOID = U.USUARIO 
                                        WHERE  CENSO = @CENSO  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                dt = contexto.GetDataTable(contextQuery);
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

            return dt;
        }

        public ValidacaoDados Valida(Entidades.AcompanhamentoNota acompanhamentoNota, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (acompanhamentoNota == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (acompanhamentoNota.AcompanhamentoNotaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (acompanhamentoNota.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (acompanhamentoNota.Processo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROCESSO é obrigatório.");
            }

            if (acompanhamentoNota.DataProcesso == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO PROCESSO é obrigatório.");
            }

            if (acompanhamentoNota.ChaveAcesso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CHAVE DE ACESSO é obrigatório.");
            }
            else
            {
                // 44 dígitos presentes no DANFE (Documento Auxiliar de Nota Fiscal Eletrônica
                if (acompanhamentoNota.ChaveAcesso.Length != 44)
                {
                    mensagens.Add("Campo CHAVE DE ACESSO deve ser composto pelos 44 dígitos presentes no DANFE.");
                }
            }

            if (acompanhamentoNota.Valido == null)
            {
                mensagens.Add("Campo VÁLIDO é obrigatório.");
            }

            if (acompanhamentoNota.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se existe outras escolas
                    string escolas = this.PossuiOutroProcessoCadastradoPor(contexto, acompanhamentoNota.Processo, acompanhamentoNota.Censo);
                    if (!escolas.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add(string.Format("Este PROCESSO já foi cadastrado para a(s) escola(s) abaixo:<br />{0}", escolas));
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private string PossuiOutroProcessoCadastradoPor(DataContext ctx, string processo, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> escolas = new List<string>();
            string listaEscolas = string.Empty;

            try
            {

                contextQuery.Command = @"SELECT CENSO, UE.NOME_COMP
                                FROM PrestacaoContas.ACOMPANHAMENTONOTA A (NOLOCK)
								     INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON A.CENSO = UE.UNIDADE_ENS
                                WHERE PROCESSO = @PROCESSO
	                                AND CENSO <> @CENSO ";

                contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, processo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    escolas.Add(string.Format("{0} - {1}", Convert.ToString(reader["CENSO"]), Convert.ToString(reader["NOME_COMP"])));
                }

                if (escolas.Count > 0)
                {
                    listaEscolas = escolas.Aggregate((x, y) => x + "<br />" + y);
                }
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

            return listaEscolas;
        }

        private string BuscaOutrasEscolasPor(DataContext ctx, string chaveAcesso, int acompanhamentoNotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> escolas = new List<string>();
            string listaEscolas = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT CENSO, UE.NOME_COMP, PROCESSO 
                                        FROM PrestacaoContas.ACOMPANHAMENTONOTA A (NOLOCK)
								                INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON A.CENSO = UE.UNIDADE_ENS
                                        WHERE CHAVEACESSO = @CHAVEACESSO
	                                            AND ACOMPANHAMENTONOTAID <> @ACOMPANHAMENTONOTAID ";

                contextQuery.Parameters.Add("@CHAVEACESSO", SqlDbType.VarChar, chaveAcesso);
                contextQuery.Parameters.Add("@ACOMPANHAMENTONOTAID", SqlDbType.Int, acompanhamentoNotaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    escolas.Add(string.Format("{0} - {1} processo: {2}", Convert.ToString(reader["CENSO"]), Convert.ToString(reader["NOME_COMP"]), Convert.ToString(reader["PROCESSO"])));
                }

                if (escolas.Count > 0)
                {
                    listaEscolas = escolas.Aggregate((x, y) => x + "<br />" + y);
                }

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

            return listaEscolas;
        }

        public void Insere(Entidades.AcompanhamentoNota acompanhamentoNota, out string aviso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                //Verifica se existe outras escolas
                aviso = this.BuscaOutrasEscolasPor(ctx, acompanhamentoNota.ChaveAcesso, 0);
                if (!aviso.IsNullOrEmptyOrWhiteSpace())
                {
                    aviso = string.Format("Esta CHAVE DE ACESSO já esta sendo usada também pela(s) escola(s) abaixo:<br />{0}", aviso);
                }

                contextQuery.Command = @" INSERT INTO PrestacaoContas.ACOMPANHAMENTONOTA 
                                                            (CENSO, 
                                                             PROCESSO, 
                                                             DATAPROCESSO,
                                                             CHAVEACESSO, 
                                                             VALIDO, 
                                                             USUARIOID, 
                                                             DATACADASTRO, 
                                                             DATAALTERACAO) 
                                                VALUES      (@CENSO, 
                                                             @PROCESSO, 
                                                             @DATAPROCESSO,
                                                             @CHAVEACESSO, 
                                                             @VALIDO, 
                                                             @USUARIOID, 
                                                             @DATACADASTRO, 
                                                             @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, acompanhamentoNota.Censo);
                contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, acompanhamentoNota.Processo);
                contextQuery.Parameters.Add("@DATAPROCESSO", SqlDbType.DateTime, acompanhamentoNota.DataProcesso);
                contextQuery.Parameters.Add("@CHAVEACESSO", SqlDbType.VarChar, acompanhamentoNota.ChaveAcesso);
                contextQuery.Parameters.Add("@VALIDO", SqlDbType.Bit, acompanhamentoNota.Valido);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, acompanhamentoNota.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void Atualiza(Entidades.AcompanhamentoNota acompanhamentoNota, out string aviso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                //Verifica se existe outras escolas
                aviso = this.BuscaOutrasEscolasPor(ctx, acompanhamentoNota.ChaveAcesso, acompanhamentoNota.AcompanhamentoNotaId);
                if (!aviso.IsNullOrEmptyOrWhiteSpace())
                {
                    aviso = string.Format("Esta CHAVE DE ACESSO já esta sendo usada também pela(s) escola(s) {0}", aviso);
                }

                contextQuery.Command = @" UPDATE PrestacaoContas.ACOMPANHAMENTONOTA 
                                              SET PROCESSO = @PROCESSO, 
                                                    DATAPROCESSO = @DATAPROCESSO,
                                                    CHAVEACESSO = @CHAVEACESSO, 
                                                    VALIDO = @VALIDO, 
                                                    USUARIOID = @USUARIOID, 
                                                    DATAALTERACAO = @DATAALTERACAO
                                              WHERE ACOMPANHAMENTONOTAID = @ACOMPANHAMENTONOTAID ";

                contextQuery.Parameters.Add("@ACOMPANHAMENTONOTAID", SqlDbType.Int, acompanhamentoNota.AcompanhamentoNotaId);
                contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, acompanhamentoNota.Processo);
                contextQuery.Parameters.Add("@DATAPROCESSO", SqlDbType.DateTime, acompanhamentoNota.DataProcesso);
                contextQuery.Parameters.Add("@CHAVEACESSO", SqlDbType.VarChar, acompanhamentoNota.ChaveAcesso);
                contextQuery.Parameters.Add("@VALIDO", SqlDbType.Bit, acompanhamentoNota.Valido);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, acompanhamentoNota.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public ValidacaoDados ValidaRemocao(int acompanhamentoNotaId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (acompanhamentoNotaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int acompanhamentoNotaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.ACOMPANHAMENTONOTA
                            WHERE  ACOMPANHAMENTONOTAID = @ACOMPANHAMENTONOTAID  ";

                contextQuery.Parameters.Add("@ACOMPANHAMENTONOTAID", SqlDbType.Int, acompanhamentoNotaId);

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