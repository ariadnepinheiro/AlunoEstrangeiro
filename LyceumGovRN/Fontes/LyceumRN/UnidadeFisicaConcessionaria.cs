using System;
using System.Collections.Generic;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN
{
    public class UnidadeFisicaConcessionaria : RNBase
    {
        public static LyUnidadeFisicaConcessionaria Carregar(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT *
                                    FROM    LY_UNIDADE_FISICA_CONCESSIONARIA
                                    WHERE   UNIDADE_FIS = @UNIDADE_FIS ");
                contextQuery.Parameters.Add("@UNIDADE_FIS", censo);

                return ctx.TryToBindEntity<LyUnidadeFisicaConcessionaria>(contextQuery);
            }
        }

        public ValidacaoDados Valida(LyUnidadeFisicaConcessionaria unidadeFisicaConcessionaria, List<int> listaTratamentoLixo)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeFisicaConcessionaria == null)
            {
                return validacaoDados;
            }

            if (unidadeFisicaConcessionaria.UnidadeFis.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE FÍSICA é obrigatório.");
            }

            //Validações de Enegia Eletrica
            if (unidadeFisicaConcessionaria.EeTipoAbastecimento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Energia Elétrica: Campo TIPO ABASTECIMENTO é obrigatório.");
            }
            else
            {
                //Verifica se foi marcada a opção Inexistente
                if (unidadeFisicaConcessionaria.EeTipoAbastecimento.Contains("Inexistente"))
                {
                    if (unidadeFisicaConcessionaria.EeTipoAbastecimento != "Inexistente")
                    {
                        mensagens.Add("Energia Elétrica: A opção INEXISTENTE do TIPO ABASTECIMENTO não pode ser escolhida juntamente com outra opção.");
                    }
                    else
                    {
                        //Com opção Inexistente marcada nenhum campo de Enegia Eletrica pode estar preenchido
                        if (!unidadeFisicaConcessionaria.EeEmpresa.IsNullOrEmptyOrWhiteSpace()
                            || !unidadeFisicaConcessionaria.EeCodCliente.IsNullOrEmptyOrWhiteSpace()
                            || !unidadeFisicaConcessionaria.EeClasseFornecimento.IsNullOrEmptyOrWhiteSpace()
                            || !unidadeFisicaConcessionaria.EeContratoFornecimento.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Energia Elétrica: Com a opção INEXISTENTE marcada, nenhum outro campo pode ser preenchido.");
                        }
                    }
                }
                else if (unidadeFisicaConcessionaria.EeTipoAbastecimento.Contains("Rede Pública/Concessionária"))
                {
                    if (unidadeFisicaConcessionaria.EeEmpresa.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Energia Elétrica: Campo CONCESSIONÁRIA é obrigatório quando marcada a opção 'Rede Pública/Concessionária'.");
                    }                   
                }
                else
                {
                    //Sem opção Rede Pública/Concessionária marcada nenhum campo de Concessionária pode estar preenchido
                    if (!unidadeFisicaConcessionaria.EeEmpresa.IsNullOrEmptyOrWhiteSpace()
                        || !unidadeFisicaConcessionaria.EeCodCliente.IsNullOrEmptyOrWhiteSpace()
                        || !unidadeFisicaConcessionaria.EeClasseFornecimento.IsNullOrEmptyOrWhiteSpace()
                        || !unidadeFisicaConcessionaria.EeContratoFornecimento.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Energia Elétrica: Sem a opção 'Rede Pública/Concessionária' marcada, nenhum campo da 'Concessionária'pode ser preenchido.");
                    }
                }
            }

            //Validações de Suprimento de agua
            if (unidadeFisicaConcessionaria.AgOutrosAbastecimentos.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Suprimento de água: Campo TIPO ABASTECIMENTO é obrigatório.");
            }
            else
            {
                //Verifica se foi marcada a opção Inexistente
                if (unidadeFisicaConcessionaria.AgOutrosAbastecimentos.Contains("Inexistente"))
                {
                    if (unidadeFisicaConcessionaria.AgOutrosAbastecimentos != "Inexistente")
                    {
                        mensagens.Add("Suprimento de água: A opção INEXISTENTE do TIPO ABASTECIMENTO não pode ser escolhida juntamente com outra opção.");
                    }
                    else
                    {
                        //Com opção Inexistente marcada nenhum campo de Enegia Eletrica pode estar preenchido
                        if ( !unidadeFisicaConcessionaria.AgEmpresa.IsNullOrEmptyOrWhiteSpace()
                            || !unidadeFisicaConcessionaria.AgCodCliente.IsNullOrEmptyOrWhiteSpace()
                            || !unidadeFisicaConcessionaria.AgHidrometro.IsNullOrEmptyOrWhiteSpace()
                          )
                        {
                            mensagens.Add("Suprimento de água: Com a opção INEXISTENTE marcada, nenhum outro campo pode ser preenchido.");
                        }
                    }
                }
                else if (unidadeFisicaConcessionaria.AgOutrosAbastecimentos.Contains("Rede Pública/Concessionária"))
                {
                    if (unidadeFisicaConcessionaria.AgEmpresa.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Suprimento de água: Campo CONCESSIONÁRIA é obrigatório quando marcada a opção 'Rede Pública/Concessionária'.");
                    }                  
                }
                else
                {
                    //Sem opção Rede Pública/Concessionária marcada nenhum campo de Concessionária pode estar preenchido
                    if (!unidadeFisicaConcessionaria.AgEmpresa.IsNullOrEmptyOrWhiteSpace()
                        || !unidadeFisicaConcessionaria.AgCodCliente.IsNullOrEmptyOrWhiteSpace()
                        || !unidadeFisicaConcessionaria.AgHidrometro.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Suprimento de água: Sem a opção 'Rede Pública/Concessionária' marcada, nenhum campo da 'Concessionária'pode ser preenchido.");
                    }
                }
            }           

            //Validações de Esgoto / Destinação de Lixo
            if (unidadeFisicaConcessionaria.ElEsgotoSanitario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Esgoto/Destinação do Lixo: Campo ESGOTO SANITÁRIO é obrigatório.");
            }
            else
            {
                //Verifica se foi marcada a opção Inexistente para esgoto
                if (unidadeFisicaConcessionaria.ElEsgotoSanitario.Contains("Inexistente"))
                {
                    if (unidadeFisicaConcessionaria.ElEsgotoSanitario != "Inexistente")
                    {
                        mensagens.Add("Esgoto/Destinação do Lixo: A opção INEXISTENTE do Esgoto Sanitário não pode ser escolhida juntamente com outra opção.");
                    }
                }
            }

            if (unidadeFisicaConcessionaria.ElDestinacaoLixo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Esgoto/Destinação do Lixo: Campo DESTINAÇÃO DO LIXO é obrigatório.");
            }

            if (listaTratamentoLixo == null || listaTratamentoLixo.Count == 0)
            {
                mensagens.Add("Esgoto/Destinação do Lixo: Campo TRATAMENTO DO LIXO é obrigatório.");
            }

            if (unidadeFisicaConcessionaria.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSAVEL é obrigatório.");
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

        public void Salva(LyUnidadeFisicaConcessionaria unidadeFisicaConcessionaria, List<int> listaTratamentoLixo)
        {
            RN.GestaoRede.UnidadeFisicaTratamentoLixo rnUnidadeFisicaTratamentoLixo = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaTratamentoLixo();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            int idUnidadeFisicaConcessionaria = 0;
            try
            {
                //Verifica se já existe Concessionária para aquele unidade Fisca
                idUnidadeFisicaConcessionaria = this.ObtemIdUnidadeFisicaConcessionariaPor(contexto, unidadeFisicaConcessionaria.UnidadeFis);                

                if (idUnidadeFisicaConcessionaria > 0)
                {
                    unidadeFisicaConcessionaria.IdUnidadeFisicaConcessionaria = idUnidadeFisicaConcessionaria;
                    this.Atualiza(contexto, unidadeFisicaConcessionaria);
                }
                else
                {
                    this.Insere(contexto, unidadeFisicaConcessionaria);
                }

                //Remove as formas de tratamento do lixo
                rnUnidadeFisicaTratamentoLixo.RemovePorUnidade(contexto, unidadeFisicaConcessionaria.UnidadeFis);

                foreach (var idTratamentoLixo in listaTratamentoLixo)
                {
                    //Insere tratamento lixo
                    rnUnidadeFisicaTratamentoLixo.Insere(contexto, unidadeFisicaConcessionaria.UnidadeFis, idTratamentoLixo, unidadeFisicaConcessionaria.Matricula);
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

        private int ObtemIdUnidadeFisicaConcessionariaPor(DataContext contexto, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int idUnidadeFisicaConcessionaria = 0;
            try
            {
                contextQuery.Command = @" SELECT ID_UNIDADE_FISICA_CONCESSIONARIA 
                                    FROM LY_UNIDADE_FISICA_CONCESSIONARIA (NOLOCK)
                                    WHERE UNIDADE_FIS = @UNIDADE_FIS ";

                contextQuery.Parameters.Add("@UNIDADE_FIS", SqlDbType.VarChar, unidadeFisica); 

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    idUnidadeFisicaConcessionaria = Convert.ToInt32(reader["ID_UNIDADE_FISICA_CONCESSIONARIA"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return idUnidadeFisicaConcessionaria;
        }

        private void Insere(DataContext contexto, LyUnidadeFisicaConcessionaria unidadeFisicaConcessionaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_UNIDADE_FISICA_CONCESSIONARIA 
                                    (UNIDADE_FIS, 
                                     EE_TIPO_ABASTECIMENTO, 
                                     EE_EMPRESA, 
                                     EE_COD_CLIENTE, 
                                     EE_CLASSE_FORNECIMENTO, 
                                     EE_CONTRATO_FORNECIMENTO,
                                     AG_OUTROS_ABASTECIMENTOS, 
                                     AG_EMPRESA, 
                                     AG_COD_CLIENTE, 
                                     AG_HIDROMETRO, 
                                     AG_PC_ARTESIANO, 
                                     AG_PC_SEMI_ARTESIANO, 
                                     AG_PC_CACIMBA,
                                     AG_PC_CARRO_PIPA,
                                     AG_TIPO_AGUA_CONSUMIDA, 
                                     AG_PC_VAZAO,
                                     AG_PC_BOMBA_SUBMERSA, 
                                     AG_PC_PROFUNDIDADE, 
                                     GA_EMPRESA, 
                                     GA_COD_CLIENTE, 
                                     GA_TIPO, 
                                     EL_ESGOTO_SANITARIO, 
                                     EL_DESTINACAO_LIXO, 
                                     MATRICULA, 
                                     DT_CADASTRO, 
                                     DT_ALTERACAO) 
                        VALUES      ( @UNIDADE_FIS, 
                                      @EE_TIPO_ABASTECIMENTO, 
                                      @EE_EMPRESA, 
                                      @EE_COD_CLIENTE, 
                                      @EE_CLASSE_FORNECIMENTO, 
                                      @EE_CONTRATO_FORNECIMENTO,
                                      @AG_OUTROS_ABASTECIMENTOS, 
                                      @AG_EMPRESA, 
                                      @AG_COD_CLIENTE, 
                                      @AG_HIDROMETRO, 
                                      @AG_PC_ARTESIANO, 
                                      @AG_PC_SEMI_ARTESIANO, 
                                      @AG_PC_CACIMBA, 
                                      @AG_PC_CARRO_PIPA,
                                      @AG_TIPO_AGUA_CONSUMIDA, 
                                      @AG_PC_VAZAO,
                                      @AG_PC_BOMBA_SUBMERSA, 
                                      @AG_PC_PROFUNDIDADE, 
                                      @GA_EMPRESA, 
                                      @GA_COD_CLIENTE, 
                                      @GA_TIPO, 
                                      @EL_ESGOTO_SANITARIO, 
                                      @EL_DESTINACAO_LIXO, 
                                      @MATRICULA, 
                                      @DT_CADASTRO, 
                                      @DT_ALTERACAO ) 
                                      
                                      SELECT IDENT_CURRENT('DBO.LY_UNIDADE_FISICA_CONCESSIONARIA') ";

            contextQuery.Parameters.Add("@UNIDADE_FIS", SqlDbType.VarChar, unidadeFisicaConcessionaria.UnidadeFis);           
            contextQuery.Parameters.Add("@EE_TIPO_ABASTECIMENTO", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeTipoAbastecimento);
            contextQuery.Parameters.Add("@EE_EMPRESA", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeEmpresa);
            contextQuery.Parameters.Add("@EE_COD_CLIENTE", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeCodCliente);
            contextQuery.Parameters.Add("@EE_CLASSE_FORNECIMENTO", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeClasseFornecimento);
            contextQuery.Parameters.Add("@EE_CONTRATO_FORNECIMENTO", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeContratoFornecimento);
            contextQuery.Parameters.Add("@AG_OUTROS_ABASTECIMENTOS", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgOutrosAbastecimentos);
            contextQuery.Parameters.Add("@AG_EMPRESA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgEmpresa);
            contextQuery.Parameters.Add("@AG_COD_CLIENTE", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgCodCliente);
            contextQuery.Parameters.Add("@AG_HIDROMETRO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgHidrometro);
            contextQuery.Parameters.Add("@AG_PC_ARTESIANO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcArtesiano);
            contextQuery.Parameters.Add("@AG_PC_SEMI_ARTESIANO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcSemiArtesiano);
            contextQuery.Parameters.Add("@AG_PC_CACIMBA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcCacimba);
            contextQuery.Parameters.Add("@AG_PC_CARRO_PIPA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcCarroPipa);                                                 
            contextQuery.Parameters.Add("@AG_TIPO_AGUA_CONSUMIDA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgTipoAguaConsumida);
            contextQuery.Parameters.Add("@AG_PC_VAZAO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcVazao);
            contextQuery.Parameters.Add("@AG_PC_BOMBA_SUBMERSA ", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcBombaSubmersa);
            contextQuery.Parameters.Add("@AG_PC_PROFUNDIDADE", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcProfundidade);
            contextQuery.Parameters.Add("@GA_EMPRESA", SqlDbType.VarChar, unidadeFisicaConcessionaria.GaEmpresa);
            contextQuery.Parameters.Add("@GA_COD_CLIENTE", SqlDbType.VarChar, unidadeFisicaConcessionaria.GaCodCliente);
            contextQuery.Parameters.Add("@GA_TIPO", SqlDbType.VarChar, unidadeFisicaConcessionaria.GaTipo);
            contextQuery.Parameters.Add("@EL_ESGOTO_SANITARIO", SqlDbType.VarChar, unidadeFisicaConcessionaria.ElEsgotoSanitario);
            contextQuery.Parameters.Add("@EL_DESTINACAO_LIXO", SqlDbType.VarChar, unidadeFisicaConcessionaria.ElDestinacaoLixo);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeFisicaConcessionaria.Matricula);
            contextQuery.Parameters.Add("@DT_CADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DT_ALTERACAO", SqlDbType.DateTime, DateTime.Now);

            unidadeFisicaConcessionaria.IdUnidadeFisicaConcessionaria = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, LyUnidadeFisicaConcessionaria unidadeFisicaConcessionaria)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_FISICA_CONCESSIONARIA 
                                    SET    EE_TIPO_ABASTECIMENTO = @EE_TIPO_ABASTECIMENTO, 
                                           EE_EMPRESA = @EE_EMPRESA, 
                                           EE_COD_CLIENTE = @EE_COD_CLIENTE, 
                                           EE_CLASSE_FORNECIMENTO = @EE_CLASSE_FORNECIMENTO, 
                                           EE_CONTRATO_FORNECIMENTO = @EE_CONTRATO_FORNECIMENTO,
                                           AG_OUTROS_ABASTECIMENTOS = @AG_OUTROS_ABASTECIMENTOS, 
                                           AG_EMPRESA = @AG_EMPRESA, 
                                           AG_COD_CLIENTE = @AG_COD_CLIENTE, 
                                           AG_HIDROMETRO = @AG_HIDROMETRO, 
                                           AG_PC_ARTESIANO = @AG_PC_ARTESIANO, 
                                           AG_PC_SEMI_ARTESIANO = @AG_PC_SEMI_ARTESIANO, 
                                           AG_PC_CACIMBA = @AG_PC_CACIMBA, 
                                           AG_PC_CARRO_PIPA = @AG_PC_CARRO_PIPA,
                                           AG_TIPO_AGUA_CONSUMIDA = @AG_TIPO_AGUA_CONSUMIDA, 
                                           AG_PC_VAZAO = @AG_PC_VAZAO,
                                           AG_PC_BOMBA_SUBMERSA = @AG_PC_BOMBA_SUBMERSA, 
                                           AG_PC_PROFUNDIDADE = @AG_PC_PROFUNDIDADE, 
                                           GA_EMPRESA = @GA_EMPRESA, 
                                           GA_COD_CLIENTE = @GA_COD_CLIENTE, 
                                           GA_TIPO = @GA_TIPO, 
                                           EL_ESGOTO_SANITARIO = @EL_ESGOTO_SANITARIO, 
                                           EL_DESTINACAO_LIXO = @EL_DESTINACAO_LIXO, 
                                           MATRICULA = @MATRICULA, 
                                           DT_ALTERACAO = @DT_ALTERACAO 
                                    WHERE  ID_UNIDADE_FISICA_CONCESSIONARIA = @ID_UNIDADE_FISICA_CONCESSIONARIA ";

            contextQuery.Parameters.Add("@ID_UNIDADE_FISICA_CONCESSIONARIA", SqlDbType.VarChar, unidadeFisicaConcessionaria.IdUnidadeFisicaConcessionaria);           
            contextQuery.Parameters.Add("@EE_TIPO_ABASTECIMENTO", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeTipoAbastecimento);
            contextQuery.Parameters.Add("@EE_EMPRESA", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeEmpresa);
            contextQuery.Parameters.Add("@EE_COD_CLIENTE", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeCodCliente);
            contextQuery.Parameters.Add("@EE_CLASSE_FORNECIMENTO", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeClasseFornecimento);
            contextQuery.Parameters.Add("@EE_CONTRATO_FORNECIMENTO", SqlDbType.VarChar, unidadeFisicaConcessionaria.EeContratoFornecimento);
            contextQuery.Parameters.Add("@AG_OUTROS_ABASTECIMENTOS", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgOutrosAbastecimentos);
            contextQuery.Parameters.Add("@AG_EMPRESA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgEmpresa);
            contextQuery.Parameters.Add("@AG_COD_CLIENTE", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgCodCliente);
            contextQuery.Parameters.Add("@AG_HIDROMETRO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgHidrometro);
            contextQuery.Parameters.Add("@AG_PC_ARTESIANO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcArtesiano);
            contextQuery.Parameters.Add("@AG_PC_SEMI_ARTESIANO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcSemiArtesiano);
            contextQuery.Parameters.Add("@AG_PC_CACIMBA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcCacimba);
            contextQuery.Parameters.Add("@AG_PC_CARRO_PIPA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcCarroPipa);                                                
            contextQuery.Parameters.Add("@AG_TIPO_AGUA_CONSUMIDA", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgTipoAguaConsumida);
            contextQuery.Parameters.Add("@AG_PC_VAZAO", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcVazao);
            contextQuery.Parameters.Add("@AG_PC_BOMBA_SUBMERSA ", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcBombaSubmersa);
            contextQuery.Parameters.Add("@AG_PC_PROFUNDIDADE", SqlDbType.VarChar, unidadeFisicaConcessionaria.AgPcProfundidade);
            contextQuery.Parameters.Add("@GA_EMPRESA", SqlDbType.VarChar, unidadeFisicaConcessionaria.GaEmpresa);
            contextQuery.Parameters.Add("@GA_COD_CLIENTE", SqlDbType.VarChar, unidadeFisicaConcessionaria.GaCodCliente);
            contextQuery.Parameters.Add("@GA_TIPO", SqlDbType.VarChar, unidadeFisicaConcessionaria.GaTipo);
            contextQuery.Parameters.Add("@EL_ESGOTO_SANITARIO", SqlDbType.VarChar, unidadeFisicaConcessionaria.ElEsgotoSanitario);
            contextQuery.Parameters.Add("@EL_DESTINACAO_LIXO", SqlDbType.VarChar, unidadeFisicaConcessionaria.ElDestinacaoLixo);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeFisicaConcessionaria.Matricula);
            contextQuery.Parameters.Add("@DT_ALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
