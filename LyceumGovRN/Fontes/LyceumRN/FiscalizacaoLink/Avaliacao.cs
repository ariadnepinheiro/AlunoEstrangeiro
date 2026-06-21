using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class Avaliacao
    {
        public RN.FiscalizacaoLink.Entidades.Avaliacao ObtemPor(int ano, int mes, string setor, int circuitoSetorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao = new RN.FiscalizacaoLink.Entidades.Avaliacao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                            FROM   FISCALIZACAOLINK.AVALIACAO (NOLOCK)
                                             WHERE  ANO = @ANO 
                                               AND MES = @MES 
                                               AND SETORID = @SETORID
                                               AND CIRCUITOSETORID = @CIRCUITOSETORID";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
                contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);
                contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetorId);

                avaliacao = contexto.TryToBindEntity<FiscalizacaoLink.Entidades.Avaliacao>(contextQuery);

                return avaliacao;
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

        public ValidacaoDados ValidaInsercao(RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao, RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoCamposGerais = new List<string>();
            RN.FiscalizacaoLink.ContratoSetor rnContratoSetor = new ContratoSetor();
            RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.CircuitoSetor();
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new CircuitoSetor();
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Interrupcao();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (avaliacao == null)
            {
                return validacaoDados;
            }

            //Valida campos obrigatorios gerais
            validacaoCamposGerais = this.ValidaCamposGerais(avaliacao, interrupcao);
            if (validacaoCamposGerais.Count > 0)
            {
                mensagens.AddRange(validacaoCamposGerais);
            }

            if ((avaliacao.Interrupcao == null || Convert.ToBoolean(avaliacao.Interrupcao)) &&
               (interrupcao == null || interrupcao.DataInterrupcao == DateTime.MinValue))
            {
                mensagens.Add("É obrigatório cadastar uma INTERRUPÇÃO para a opção 'Declaro que HOUVE interrupção do serviço'.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe uma avaliação para o ano mes setor  
                    if (this.ExisteAvaliacaoPor(contexto, avaliacao.Ano, avaliacao.Mes, avaliacao.SetorId, avaliacao.CircuitoSetorId))
                    {
                        mensagens.Add("Já existe uma avaliação cadastrada para o ano e mês e unidade administrativa.");
                    }

                    //Busca dados do circuito
                    circuitoSetor = rnCircuitoSetor.ObtemPor(avaliacao.CircuitoSetorId);

                    bool anoMesCorreto = false;

                    //Verifica se o circuito é do ano / mes
                    if (Convert.ToDateTime(circuitoSetor.Inicio).Year == avaliacao.Ano
                        || (circuitoSetor.Fim != null && circuitoSetor.Fim != DateTime.MinValue && Convert.ToDateTime(circuitoSetor.Fim).Month == avaliacao.Mes)
                        || ((circuitoSetor.Fim == null || circuitoSetor.Fim == DateTime.MinValue) && Convert.ToDateTime(circuitoSetor.Inicio).Month <= avaliacao.Mes))
                    {
                        anoMesCorreto = true;
                    }
                    else 
                    {
                        mensagens.Add("O circuito selecionada não é do ano / mês escolhido.");
                    }

                    if (Convert.ToBoolean(avaliacao.Interrupcao))
                    {
                        //Verifica se existe outro interrupcao com msm chamado
                        if (rnInterrupcao.PossuiInterrupcaoPor(contexto, interrupcao.Chamado))
                        {
                            mensagens.Add("Já existe uma interrupção cadastrada com este número de chamado.");
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

        public bool PossuiCircuitoInterrupcaoPor(DataContext contexto, int circuitoSetorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.AVALIACAO 
                                    where CIRCUITOSETORID = @CIRCUITOSETORID ";

            contextQuery.Parameters.Add("@CIRCUITOSETORID", circuitoSetorId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool ExisteAvaliacaoPor(DataContext contexto, int ano, int mes, string setor, int circuitoSetorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   FISCALIZACAOLINK.AVALIACAO (NOLOCK) 
                                    WHERE  ANO = @ANO 
                                           AND MES = @MES 
                                           AND SETORID = @SETORID
                                           AND CIRCUITOSETORID = @CIRCUITOSETORID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);
            contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);
            contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, mes);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private List<string> ValidaCamposGerais(RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao, RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Interrupcao();
            List<string> validacaoCamposGeraisInterrupcao = new List<string>();
            List<string> mensagens = new List<string>();
            DateTime dataMinimaEnvio;

            if (avaliacao.SetorId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (avaliacao.CircuitoSetorId <= 0)
            {
                mensagens.Add("Campo CIRCUITO é obrigatório.");
            }

            if (avaliacao.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (avaliacao.Mes <= 0)
            {
                mensagens.Add("Campo MÊS é obrigatório.");
            }

            if (avaliacao.Interrupcao == null)
            {
                mensagens.Add("Campo RESPOSTA é obrigatório.");
            }
            else if (!Convert.ToBoolean(avaliacao.Interrupcao) && interrupcao.DataInterrupcao != DateTime.MinValue)
            {
                mensagens.Add("Para cadastar uma INTERRUPÇÃO é necessário marcar a opção 'Declaro que HOUVE interrupção do serviço'.");
            }

            if (avaliacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            //Valida campos da Interrupcao
            if (Convert.ToBoolean(avaliacao.Interrupcao))
            {
                interrupcao.UsuarioId = avaliacao.UsuarioId;

                //Valida campos obrigatorios gerais da Interrupcao
                validacaoCamposGeraisInterrupcao = rnInterrupcao.ValidaCamposGerais(interrupcao, avaliacao.Ano, avaliacao.Mes, avaliacao.CircuitoSetorId);
                if (validacaoCamposGeraisInterrupcao.Count > 0)
                {
                    mensagens.AddRange(validacaoCamposGeraisInterrupcao);
                }

                if (interrupcao.Chamado.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NÚMERO DO CHAMADO é obrigatório.");
                }
            }

            if (avaliacao.EnvioFaturamento)
            {
                //Monta data minima para envio
                dataMinimaEnvio = new DateTime(avaliacao.Ano, avaliacao.Mes, DateTime.DaysInMonth(avaliacao.Ano, avaliacao.Mes));

                //Valida data envio
                if (DateTime.Now.Date < dataMinimaEnvio.Date)
                {
                    mensagens.Add(string.Format("A avaliação deste Ano/Mês apenas poderá ser enviada para faturamento a partir do dia {0}.", dataMinimaEnvio.ToString("dd/MM/yyyy")));
                }
            }

            return mensagens;
        }

        public void Insere(RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao, RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Interrupcao();
            try
            {
                //Insere avaliacao
                this.Insere(contexto, avaliacao);

                if (Convert.ToBoolean(avaliacao.Interrupcao))
                {
                    interrupcao.AvaliacaoId = avaliacao.AvaliacaoId;
                    rnInterrupcao.Insere(contexto, interrupcao);
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

        private void Insere(DataContext contexto, RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO FISCALIZACAOLINK.AVALIACAO 
                                                    (SETORID, 
                                                     CIRCUITOSETORID,
                                                     ANO, 
                                                     MES, 
                                                     INTERRUPCAO, 
                                                     ENVIOFATURAMENTO, 
                                                     DATAENVIOFATURAMENTO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@SETORID, 
                                                     @CIRCUITOSETORID,
                                                     @ANO, 
                                                     @MES, 
                                                     @INTERRUPCAO, 
                                                     @ENVIOFATURAMENTO, 
                                                     @DATAENVIOFATURAMENTO, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO) 
                                
                                SELECT IDENT_CURRENT('FISCALIZACAOLINK.AVALIACAO') ";

            contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, avaliacao.SetorId);
            contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, avaliacao.CircuitoSetorId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, avaliacao.Ano);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, avaliacao.Mes);
            contextQuery.Parameters.Add("@INTERRUPCAO", SqlDbType.Bit, avaliacao.Interrupcao);
            contextQuery.Parameters.Add("@ENVIOFATURAMENTO", SqlDbType.Bit, avaliacao.EnvioFaturamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, avaliacao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (avaliacao.EnvioFaturamento)
            {
                contextQuery.Parameters.Add("@DATAENVIOFATURAMENTO", SqlDbType.DateTime, DateTime.Now);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAENVIOFATURAMENTO", SqlDbType.DateTime, null);
            }

            avaliacao.AvaliacaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados ValidaAtualizacao(RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao, RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            List<string> validacaoCamposGerais = new List<string>();
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Interrupcao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (avaliacao == null)
            {
                return validacaoDados;
            }

            if (avaliacao.AvaliacaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            //Valida campos obrigatorios gerais
            validacaoCamposGerais = this.ValidaCamposGerais(avaliacao, interrupcao);
            if (validacaoCamposGerais.Count > 0)
            {
                mensagens.AddRange(validacaoCamposGerais);
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida se já foi enviado para faturamento
                    if (this.PossuiEnvioFaturamentoPor(contexto, avaliacao.AvaliacaoId))
                    {
                        mensagens.Add("Esta avaliação não pode ser editada pois já foi enviada para faturamento.");
                    }

                    if (!Convert.ToBoolean(avaliacao.Interrupcao))
                    {
                        //Valida se ainda existe alguma Interrupcao cadastrada
                        if (rnInterrupcao.ExisteInterrupcaoPor(contexto, avaliacao.AvaliacaoId))
                        {
                            mensagens.Add("Para marcar 'Declaro que NÃO HOUVE interrupção do serviço' é necessário que não exista nenhuma interrupção cadastrada.");
                        }
                    }
                    else
                    {
                        //Verifica se existe interrupcao com msm chamado
                        if (rnInterrupcao.PossuiInterrupcaoPor(contexto, interrupcao.Chamado))
                        {
                            mensagens.Add("Já existe uma interrupção cadastrada com este número de chamado.");
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

        public void Atualiza(RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao, RN.FiscalizacaoLink.Entidades.Interrupcao interrupcao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Interrupcao();
            try
            {
                //atualiza avaliacao
                this.Atualiza(contexto, avaliacao);

                if (Convert.ToBoolean(avaliacao.Interrupcao))
                {
                    //insere interrupcao
                    interrupcao.AvaliacaoId = avaliacao.AvaliacaoId;
                    rnInterrupcao.Insere(contexto, interrupcao);
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

        private void Atualiza(DataContext contexto, RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FISCALIZACAOLINK.AVALIACAO 
                                        SET    INTERRUPCAO = @INTERRUPCAO, 
                                               ENVIOFATURAMENTO = @ENVIOFATURAMENTO, 
                                               DATAENVIOFATURAMENTO = @DATAENVIOFATURAMENTO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  AVALIACAOID = @AVALIACAOID ";

            contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacao.AvaliacaoId);
            contextQuery.Parameters.Add("@INTERRUPCAO", SqlDbType.Bit, avaliacao.Interrupcao);
            contextQuery.Parameters.Add("@ENVIOFATURAMENTO", SqlDbType.Bit, avaliacao.EnvioFaturamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, avaliacao.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (avaliacao.EnvioFaturamento)
            {
                contextQuery.Parameters.Add("@DATAENVIOFATURAMENTO", SqlDbType.DateTime, DateTime.Now);
            }
            else
            {
                contextQuery.Parameters.Add("@DATAENVIOFATURAMENTO", SqlDbType.DateTime, null);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public void RetiraOpcaoInterrupcao(DataContext contexto, int avaliacaoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FISCALIZACAOLINK.AVALIACAO 
                                        SET    INTERRUPCAO = 0,                                                 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  AVALIACAOID = @AVALIACAOID ";

            contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiOpcaoInterrupcaoPor(DataContext contexto, int avaliacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            bool existe = false;

            try
            {

                contextQuery.Command = @" SELECT INTERRUPCAO
                                        FROM   FISCALIZACAOLINK.AVALIACAO (NOLOCK)
                                        WHERE  AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    existe = Convert.ToBoolean(reader["INTERRUPCAO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return existe;
        }

        public bool PossuiEnvioFaturamentoPor(DataContext contexto, int avaliacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            bool existe = false;

            try
            {

                contextQuery.Command = @" SELECT ENVIOFATURAMENTO
                                        FROM   FISCALIZACAOLINK.AVALIACAO (NOLOCK)
                                        WHERE  AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    existe = Convert.ToBoolean(reader["ENVIOFATURAMENTO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return existe;
        }
    }
}