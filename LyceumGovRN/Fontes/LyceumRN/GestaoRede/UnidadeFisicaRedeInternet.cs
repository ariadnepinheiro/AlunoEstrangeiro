using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class UnidadeFisicaRedeInternet
    {
        public ICollection<Entidades.UnidadeFisicaRedeInternet> ObtemPor(string unidadeFisica)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ICollection<Entidades.UnidadeFisicaRedeInternet> entidades = new List<Techne.Lyceum.RN.GestaoRede.Entidades.UnidadeFisicaRedeInternet>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                          FROM GESTAOREDE.UNIDADEFISICA_REDEINTERNET (NOLOCK)
                                          WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

                contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

                entidades = contexto.TryToBindEntities<Entidades.UnidadeFisicaRedeInternet>(contextQuery);

                return entidades;
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

        public DTOs.UnidadeDadosInternet ObtemDadosInternetPor(string censo)
        {
            DTOs.UnidadeDadosInternet entidade = new DTOs.UnidadeDadosInternet();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.GestaoRede.UnidadeFisicaAcessoInternet rnUnidadeFisicaAcessoInternet = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaAcessoInternet();
            
            try
            {
                //Busca caracteristicas fisicas
                entidade = this.ObtemDadosInternetPor(contexto, censo);

                //Busca Dados de Acesso a Internet
                entidade.AcessoInternet = rnUnidadeFisicaAcessoInternet.ListaAcessoInternetPor(contexto, censo);                           

                return entidade;
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

        private DTOs.UnidadeDadosInternet ObtemDadosInternetPor(DataContext contexto, string censo)
        {
            DTOs.UnidadeDadosInternet entidade = new DTOs.UnidadeDadosInternet();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT [UNIDADEFISICA_REDEINTERNETID]
                                                  ,[UNIDADEFISICAID]
                                                  ,[BANDALARGA]
                                                  ,[DISPOSITIVOESCOLA]
                                                  ,[DISPOSITIVOPESSOAL]
                                                  ,[REDECABO]
                                                  ,[REDEWIRELESS]                                                
                                            FROM GESTAOREDE.UNIDADEFISICA_REDEINTERNET (NOLOCK)
                                          WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

                contextQuery.Parameters.Add("@UNIDADEFISICAID", censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    entidade.Censo = Convert.ToString(reader["UNIDADEFISICAID"]);
                    entidade.BandaLarga = Convert.ToString(reader["BANDALARGA"]);
                    entidade.DispositivoEscola = Convert.ToString(reader["DISPOSITIVOESCOLA"]);
                    entidade.DispositivoPessoal = Convert.ToString(reader["DISPOSITIVOPESSOAL"]);
                    entidade.RedeCabo = Convert.ToString(reader["REDECABO"]);
                    entidade.RedeWireless = Convert.ToString(reader["REDEWIRELESS"]);                    
                }

                return entidade;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Insere(DataContext contexto, GestaoRede.Entidades.UnidadeFisicaRedeInternet unidadeFisicaRedeInternet)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO GestaoRede.UNIDADEFISICA_REDEINTERNET
                                               (UNIDADEFISICAID
                                               ,BANDALARGA        
                                               ,DISPOSITIVOESCOLA                        
		                                       ,DISPOSITIVOPESSOAL
		                                       ,REDECABO
		                                       ,REDEWIRELESS
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                              (@UNIDADEFISICAID,
                                               @BANDALARGA,
                                               @DISPOSITIVOESCOLA,                        
		                                       @DISPOSITIVOPESSOAL,
		                                       @REDECABO,
		                                       @REDEWIRELESS,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";


            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisicaRedeInternet.UnidadeFisicaId);
            contextQuery.Parameters.Add("@BANDALARGA", SqlDbType.VarChar, unidadeFisicaRedeInternet.BandaLarga);
            contextQuery.Parameters.Add("@DISPOSITIVOESCOLA", SqlDbType.VarChar, unidadeFisicaRedeInternet.DispositivoEscola);
            contextQuery.Parameters.Add("@DISPOSITIVOPESSOAL", SqlDbType.VarChar, unidadeFisicaRedeInternet.DispositivoPessoal);
            contextQuery.Parameters.Add("@REDECABO", SqlDbType.VarChar, unidadeFisicaRedeInternet.RedeCabo);
            contextQuery.Parameters.Add("@REDEWIRELESS", SqlDbType.VarChar, unidadeFisicaRedeInternet.RedeWireless);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeFisicaRedeInternet.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorUnidade(DataContext contexto, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE GestaoRede.UNIDADEFISICA_REDEINTERNET
                                      WHERE UNIDADEFISICAID = @UNIDADEFISICAID ";

            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeFisica);

            contexto.ApplyModifications(contextQuery);
        }


        public ValidacaoDados ValidaDadosInternet(DTOs.UnidadeDadosInternet unidadeDadosInternet)
        {
            List<string> mensagens = new List<string>();
            string cep = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeDadosInternet == null)
            {
                return validacaoDados;
            }

            if (unidadeDadosInternet.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (unidadeDadosInternet.BandaLarga.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo INTERNET BANDA LARGA é obrigatório.");
            }

            if (unidadeDadosInternet.BandaLarga.IsNullOrEmptyOrWhiteSpace()
               || (unidadeDadosInternet.BandaLarga != "N" && unidadeDadosInternet.BandaLarga != "S"))
            {
                mensagens.Add("Campo INTERNET BANDA LARGA é obrigatório.");
            }

            if (!unidadeDadosInternet.BandaLarga.IsNullOrEmptyOrWhiteSpace())
            {
                if (unidadeDadosInternet.BandaLarga == "S")
                {
                    if (unidadeDadosInternet.AcessoInternet == null || unidadeDadosInternet.AcessoInternet.Count == 0)
                    {
                        mensagens.Add("Campo ACESSO á INTERNET é obrigatório.");
                    }
                    else
                    {
                        //1	Nenhuma das opções
                        if (unidadeDadosInternet.AcessoInternet.Contains(5) && unidadeDadosInternet.AcessoInternet.Count > 1)
                        {
                            mensagens.Add("Quando a opção NÃO POSSUI estiver marcada não é possível acrescentar outra opção");
                        }
                    }

                    if ((unidadeDadosInternet.DispositivoEscola.IsNullOrEmptyOrWhiteSpace() && unidadeDadosInternet.DispositivoPessoal.IsNullOrEmptyOrWhiteSpace()) ||
                        (unidadeDadosInternet.DispositivoEscola == "N" && unidadeDadosInternet.DispositivoPessoal == "N"))
                    {
                        mensagens.Add("Campo EQUIPAMENTOS QUE OS ALUNOS USAM PARA ACESSAR A INTERNET DA ESCOLA é obrigatório.");
                    }

                    if ((unidadeDadosInternet.RedeCabo.IsNullOrEmptyOrWhiteSpace() && unidadeDadosInternet.RedeWireless.IsNullOrEmptyOrWhiteSpace() && unidadeDadosInternet.SemRedeComputador.IsNullOrEmptyOrWhiteSpace()) ||
                        (unidadeDadosInternet.RedeCabo == "N" && unidadeDadosInternet.RedeWireless == "N" && unidadeDadosInternet.SemRedeComputador == "N"))
                    {
                        mensagens.Add("Campo REDE LOCAL DE INTERLIGAÇÃO DE COMPUTADORES é obrigatório.");
                    }

                   
                }
            }
            if (unidadeDadosInternet.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
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

        public void SalvaDadosInternet(DTOs.UnidadeDadosInternet unidadeDadosInternet)
        {

            RN.GestaoRede.UnidadeFisicaAcessoInternet rnUnidadeFisicaAcessoInternet = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaAcessoInternet();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {

                //Atualiza ou insere dados 
                InsereOuAtualizaDadosInternet(contexto, unidadeDadosInternet);

                //Remove todas os acessos de internet
                rnUnidadeFisicaAcessoInternet.RemovePorUnidade(contexto, unidadeDadosInternet.Censo);

                if (unidadeDadosInternet.AcessoInternet != null && unidadeDadosInternet.AcessoInternet.Count > 0)
                {
                    foreach (int idAcessoInternet in unidadeDadosInternet.AcessoInternet)
                    {
                        //Inserir orgao colegiado
                        rnUnidadeFisicaAcessoInternet.Insere(contexto, unidadeDadosInternet.Censo, idAcessoInternet, unidadeDadosInternet.UsuarioResponsavel);
                    }
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
                contexto.Dispose();
            }
        }

        private void AtualizaDadosInternet(DataContext contexto, DTOs.UnidadeDadosInternet unidadeDadosInternet)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"UPDATE  [LYCEUM].[GestaoRede].[UNIDADEFISICA_REDEINTERNET]
                            SET        BANDALARGA = @BANDALARGA,
                                       DISPOSITIVOESCOLA = @DISPOSITIVOESCOLA,                     
                                       DISPOSITIVOPESSOAL = @DISPOSITIVOPESSOAL,
                                       REDECABO = @REDECABO,
                                       REDEWIRELESS = @REDEWIRELESS,
                                       USUARIOID = @USUARIOID,                                    
                                       DATAALTERACAO = @DATAALTERACAO
                            WHERE   UNIDADEFISICAID = @UNIDADEFISICAID ";

            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeDadosInternet.Censo);
            contextQuery.Parameters.Add("@BANDALARGA", SqlDbType.VarChar, unidadeDadosInternet.BandaLarga);
            contextQuery.Parameters.Add("@DISPOSITIVOESCOLA", SqlDbType.VarChar, unidadeDadosInternet.DispositivoEscola);
            contextQuery.Parameters.Add("@DISPOSITIVOPESSOAL", SqlDbType.VarChar, unidadeDadosInternet.DispositivoPessoal);
            contextQuery.Parameters.Add("@REDECABO", SqlDbType.VarChar, unidadeDadosInternet.RedeCabo);
            contextQuery.Parameters.Add("@REDEWIRELESS", SqlDbType.VarChar, unidadeDadosInternet.RedeWireless);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeDadosInternet.UsuarioResponsavel);            
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereOuAtualizaDadosInternet(DataContext dataContext, DTOs.UnidadeDadosInternet unidadeDadosInternet)
        {
            ContextQuery contextQuery = new ContextQuery(
                    @"DECLARE @TEMUNIDADE INT 

                        SELECT @TEMUNIDADE = COUNT(*)
                        FROM    [LYCEUM].[GestaoRede].[UNIDADEFISICA_REDEINTERNET]
                        WHERE  UNIDADEFISICAID = @UNIDADEFISICAID

                    IF ( @TEMUNIDADE = 0 ) 
                      BEGIN 
                       INSERT INTO GestaoRede.UNIDADEFISICA_REDEINTERNET
                                               (UNIDADEFISICAID
                                               ,BANDALARGA        
                                               ,DISPOSITIVOESCOLA                        
		                                       ,DISPOSITIVOPESSOAL
		                                       ,REDECABO
		                                       ,REDEWIRELESS
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                              (@UNIDADEFISICAID,
                                               @BANDALARGA,
                                               @DISPOSITIVOESCOLA,                        
		                                       @DISPOSITIVOPESSOAL,
		                                       @REDECABO,
		                                       @REDEWIRELESS,
                                               @USUARIOID,
                                               @DATACADASTRO,
                                               @DATAALTERACAO)
                      END 
                    ELSE 
                      BEGIN 
                            UPDATE  [LYCEUM].[GestaoRede].[UNIDADEFISICA_REDEINTERNET]
                            SET        BANDALARGA = @BANDALARGA,
                                       DISPOSITIVOESCOLA = @DISPOSITIVOESCOLA,                     
                                       DISPOSITIVOPESSOAL = @DISPOSITIVOPESSOAL,
                                       REDECABO = @REDECABO,
                                       REDEWIRELESS = @REDEWIRELESS,
                                       USUARIOID = @USUARIOID,                                    
                                       DATAALTERACAO = @DATAALTERACAO
                            WHERE   UNIDADEFISICAID = @UNIDADEFISICAID

                      END");


            contextQuery.Parameters.Add("@UNIDADEFISICAID", SqlDbType.VarChar, unidadeDadosInternet.Censo);
            contextQuery.Parameters.Add("@BANDALARGA", SqlDbType.VarChar, unidadeDadosInternet.BandaLarga);
            contextQuery.Parameters.Add("@DISPOSITIVOESCOLA", SqlDbType.VarChar, unidadeDadosInternet.DispositivoEscola);
            contextQuery.Parameters.Add("@DISPOSITIVOPESSOAL", SqlDbType.VarChar, unidadeDadosInternet.DispositivoPessoal);
            contextQuery.Parameters.Add("@REDECABO", SqlDbType.VarChar, unidadeDadosInternet.RedeCabo);
            contextQuery.Parameters.Add("@REDEWIRELESS", SqlDbType.VarChar, unidadeDadosInternet.RedeWireless);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeDadosInternet.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dataContext.ApplyModifications(contextQuery);
        }
    }
}
