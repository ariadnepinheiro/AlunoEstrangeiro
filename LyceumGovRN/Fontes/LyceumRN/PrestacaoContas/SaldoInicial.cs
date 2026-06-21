using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class SaldoInicial
    {
        public bool PossuiPlanoTrabalhoPor(DataContext contexto, int planoTrabalhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PRESTACAOCONTAS.SALDOINICIAL (NOLOCK)
                                    WHERE PLANOTRABALHOID = @PLANOTRABALHOID ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.SaldoInicial saldoInicial, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Perfil rnPerfil = new Perfil();
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.PlanilhaOrcamentaria();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (saldoInicial == null)
            {
                return validacaoDados;
            }

            if (saldoInicial.PlanoTrabalhoId == 0)
            {
                mensagens.Add("O campo obrigatório Projeto / Programa não foi preenchido  ");
            }

            if (string.IsNullOrEmpty(saldoInicial.Censo))
            {
                mensagens.Add(" O campo obrigatório Unidade de Ensino não foi preenchido");
            }

            if (saldoInicial.ValorInicial == 0)
            {
                mensagens.Add(" O campo Saldo Inicial é obrigatório e deve ser maior que zero");
            }

            if (saldoInicial.ValorInicial < 0)
            {
                mensagens.Add(" O campo obrigatório Saldo Inicial não pode ser negativo");
            }
            else
            {
                decimal valorMaximo = Convert.ToDecimal(9999999999999.99);
                if (saldoInicial.ValorInicial > valorMaximo)
                {
                    mensagens.Add(string.Format(" O campo obrigatório Saldo Inicial não pode ser maior que {0}.", valorMaximo.ToString()));
                }
            }

            if (saldoInicial.DataReferenciaCalculo <= SqlDateTime.MinValue.Value)
            {
                mensagens.Add(" O campo obrigatório Data de Referência não foi preenchido ");
            }

            if (saldoInicial.DataInvalida)
            {
                mensagens.Add(" Insira uma data válida. ");
            }

            if (saldoInicial.DataReferenciaCalculo != null)
            {
                if (saldoInicial.DataReferenciaCalculo.Date != null)
                {
                    if (saldoInicial.DataReferenciaCalculo.Date > DateTime.Now.Date)
                    {
                        mensagens.Add(" A data de referência para cálculo de saldo não pode ser maior do que a data atual. ");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe o cadastro
                    if (cadastro && this.PossuiOutroCadastradoPor(contexto, saldoInicial.PlanoTrabalhoId, saldoInicial.Censo))
                    {
                        mensagens.Add("Saldo inicial já cadastrado.");
                    }

                    if (rnPlanilhaOrcamentaria.PossuiPlanilhaOrcamentariaPor(saldoInicial.PlanoTrabalhoId, saldoInicial.Censo))
                    {
                        if (!rnPerfil.PossuiPerfilSaldoInicialTotalPor(saldoInicial.UsuarioId) && !RN.Usuarios.UsuarioPrivilegiado(saldoInicial.UsuarioId))
                        {
                            mensagens.Add("Não é possível alterar saldos de Unidades de Ensino e Planos de Trabalho que já possuem planilhas orçamentárias e com solicitações de repasses.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Atualiza(Entidades.SaldoInicial saldoInicial)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE PrestacaoContas.SALDOINICIAL
                                       SET
									    PLANOTRABALHOID = @PLANOTRABALHOID
										,CENSO = @CENSO
										,VALORINICIAL = @VALORINICIAL
										,DATAREFERENCIACALCULO = @DATAREFERENCIACALCULO										
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO

                                     WHERE SALDOINICIALID = @SALDOINICIALID ";

                contextQuery.Parameters.Add("@SALDOINICIALID", SqlDbType.Int, saldoInicial.SaldoInicialId);
                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, saldoInicial.PlanoTrabalhoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, saldoInicial.Censo);
                contextQuery.Parameters.Add("@VALORINICIAL", SqlDbType.Decimal, saldoInicial.ValorInicial);
                contextQuery.Parameters.Add("@DATAREFERENCIACALCULO", SqlDbType.DateTime, saldoInicial.DataReferenciaCalculo);

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, saldoInicial.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public void Insere(Entidades.SaldoInicial saldoInicial)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {

                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  INSERT INTO [PrestacaoContas].[SALDOINICIAL]
                                           (PLANOTRABALHOID
                                           ,CENSO
                                           ,VALORINICIAL
                                           ,DATAREFERENCIACALCULO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           ( @PLANOTRABALHOID
                                           ,@CENSO
                                           ,@VALORINICIAL
                                           ,@DATAREFERENCIACALCULO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO )                                     
                                          
                         SELECT IDENT_CURRENT('PrestacaoContas.SALDOINICIAL') ";

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, saldoInicial.PlanoTrabalhoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, saldoInicial.Censo);
                contextQuery.Parameters.Add("@VALORINICIAL", SqlDbType.Decimal, saldoInicial.ValorInicial);
                contextQuery.Parameters.Add("@DATAREFERENCIACALCULO", SqlDbType.DateTime, saldoInicial.DataReferenciaCalculo);

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, saldoInicial.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                saldoInicial.SaldoInicialId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
            }

            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public ValidacaoDados ValidaRemocao(string censo, int planoTrabalhoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            RN.Perfil rnPerfil = new Perfil();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (planoTrabalhoId <= 0)
            {
                mensagens.Add("O campo PROJETO / PROGRAMA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                if (!rnPerfil.PossuiPerfilSaldoInicialTotalPor(usuarioId) && !RN.Usuarios.UsuarioPrivilegiado(usuarioId))
                {
                    mensagens.Add("Este usuário não possui permissão para excluir ou zerar saldo inicial.");
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

        public void Remove(string censo, int planoTrabalhoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @"  DELETE
                                           FROM [LYCEUM].[PrestacaoContas].[SALDOINICIAL] 
                                           WHERE CENSO = @CENSO
                                                AND PLANOTRABALHOID = @PLANOTRABALHOID ";

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                contexto.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        public DTOs.DadosSaldoInicial ObtemSaldoInicialPor(string censo, int planoTrabalhoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            DTOs.DadosSaldoInicial dadosSaldoInicial = new DTOs.DadosSaldoInicial();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT SALDOINICIALID, VALORINICIAL, DATAREFERENCIACALCULO from [PrestacaoContas].[SALDOINICIAL]
                                        where CENSO = @CENSO 
		                                AND PLANOTRABALHOID = @PLANOTRABALHOID";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@PLANOTRABALHOID", planoTrabalhoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosSaldoInicial.SaldoInicialID = Convert.ToInt32(reader["SALDOINICIALID"]);
                    dadosSaldoInicial.ValorInicial = Convert.ToDecimal(reader["VALORINICIAL"]);
                    dadosSaldoInicial.DataReferenciaVinculo = Convert.ToDateTime(reader["DATAREFERENCIACALCULO"]);

                }

                return dadosSaldoInicial;
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

        private bool PossuiOutroCadastradoPor(DataContext ctx, int planoTrabalhoId, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                FROM PRESTACAOCONTAS.SALDOINICIAL (NOLOCK)
                                WHERE CENSO = @CENSO 
                                    AND PLANOTRABALHOID = @PLANOTRABALHOID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

    }
}
