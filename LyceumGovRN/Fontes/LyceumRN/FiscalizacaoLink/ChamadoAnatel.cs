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
    public class ChamadoAnatel
    {
        public DataTable ListaPor(int circuitoSetorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CHAMADOANATELID, 
		                                        CIRCUITOSETORID, 
		                                        NUMEROOPERADORA, 
		                                        DATAOPERADORA, 
		                                        NUMEROANATEL, 
		                                        DATAANATEL, 
		                                        DATARESOLUCAO, 
		                                        SEVERIDADE
                                        FROM FISCALIZACAOLINK.CHAMADOANATEL CA (NOLOCK)
                                        WHERE CIRCUITOSETORID = @CIRCUITOSETORID ";

                contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetorId);

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

        public bool PossuiCircuitoInterrupcaoPor(DataContext contexto, int circuitoSetorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.CHAMADOANATEL 
                                    where CIRCUITOSETORID = @CIRCUITOSETORID ";

            contextQuery.Parameters.Add("@CIRCUITOSETORID", circuitoSetorId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(RN.FiscalizacaoLink.Entidades.ChamadoAnatel chamadoAnatel, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new CircuitoSetor();
            RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor = new Entidades.CircuitoSetor();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (chamadoAnatel == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (chamadoAnatel.ChamadoAnatelId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (chamadoAnatel.CircuitoSetorId <= 0)
            {
                mensagens.Add("Campo CIRCUITO / CONTRATO é obrigatório.");
            }
            else
            {
                //Busca dados do circuito
                circuitoSetor = rnCircuitoSetor.ObtemPor(chamadoAnatel.CircuitoSetorId);

                if (circuitoSetor.CircuitoSetorId <= 0)
                {
                    mensagens.Add("Campo CIRCUITO / CONTRATO é inválido.");
                }
            }

            if (chamadoAnatel.NumeroOperadora.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO CHAMADO NA OPERADORA é obrigatório.");
            }
            else if (chamadoAnatel.NumeroOperadora.Length > 50)
            {
                mensagens.Add("Campo NÚMERO DO CHAMADO NA OPERADORA deve ser composto por no máximo 50 caracteres.");
            }

            if (chamadoAnatel.DataOperadora == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO CHAMADO NA OPERADORA é obrigatório.");
            }
            else
            {
                if (chamadoAnatel.DataOperadora != null && chamadoAnatel.DataOperadora != DateTime.MinValue)
                {
                    if (chamadoAnatel.DataAnatel != null && chamadoAnatel.DataAnatel != DateTime.MinValue)
                    {
                        if (Convert.ToDateTime(chamadoAnatel.DataOperadora) > chamadoAnatel.DataAnatel)
                        {
                            mensagens.Add("O Campo  DATA DO CHAMADO NA OPERADORA não pode ser superior a DATA DO CHAMADO NA ANATEL.");
                        }
                    }                    
                }

                //Validar se a data escolhida em "Data Chamado Operadora" está dentro do período de vigência do circuito escolhido
                if (Convert.ToDateTime(chamadoAnatel.DataOperadora).Date < circuitoSetor.Inicio ||
                    (circuitoSetor.Fim != null && circuitoSetor.Fim != DateTime.MinValue && Convert.ToDateTime(chamadoAnatel.DataOperadora).Date > Convert.ToDateTime(circuitoSetor.Fim).Date))
                {
                    mensagens.Add("A DATA DO CHAMADO NA OPERADORA não está no período em que o link/circuito indicado estava ativo. Por favor, verifique as informações e ajuste, caso necessário.");
                }
            }

            if (chamadoAnatel.NumeroAnatel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO CHAMADO NA ANATEL é obrigatório.");
            }
            else if (chamadoAnatel.NumeroAnatel.Length > 50)
            {
                mensagens.Add("Campo NÚMERO DO CHAMADO NA ANATEL deve ser composto por no máximo 50 caracteres.");
            }

            if (chamadoAnatel.DataAnatel == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO CHAMADO NA ANATEL é obrigatório.");
            }
            else if (chamadoAnatel.DataResolucao != null && chamadoAnatel.DataResolucao != DateTime.MinValue)
            {
                if (Convert.ToDateTime(chamadoAnatel.DataResolucao) < chamadoAnatel.DataAnatel)
                {
                    mensagens.Add("O Campo DATA DA RESOLUÇÃO não pode ser inferior a DATA DO CHAMADO NA ANATEL.");
                }
            }


            if (chamadoAnatel.Severidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SEVERIDADE é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o numero da chamado ja existe
                    if (this.PossuiOutroNumeroOperadoraPor(contexto, chamadoAnatel.NumeroOperadora, chamadoAnatel.ChamadoAnatelId))
                    {
                        mensagens.Add("Este NÚMERO CHAMADO NA OPERADORA já foi cadastrado.");
                    }

                    //Verifica se o numero da chamado ja existe
                    if (this.PossuiOutroNumeroAnatelPor(contexto, chamadoAnatel.NumeroAnatel, chamadoAnatel.ChamadoAnatelId))
                    {
                        mensagens.Add("Este NÚMERO DO CHAMADO NA ANATEL já foi cadastrado.");
                    }

                    //Data de Interrupção será a mesma data informada para data de abertura de chamado Anatel.
                    //Data de restabelecimento será a mesma data informada para data de resolução Anatel.
                    //O motivo do chamando deverá ser “Interrupção Anatel”

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

        private bool PossuiOutroNumeroAnatelPor(DataContext contexto, string numeroAnatel, int chamadoAnatelId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   FISCALIZACAOLINK.CHAMADOANATEL (NOLOCK) 
                                        WHERE  NUMEROANATEL = @NUMEROANATEL
	                                           and CHAMADOANATELID <> @CHAMADOANATELID ";

            contextQuery.Parameters.Add("@CHAMADOANATELID", SqlDbType.Int, chamadoAnatelId);
            contextQuery.Parameters.Add("@NUMEROANATEL", SqlDbType.VarChar, numeroAnatel);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroNumeroOperadoraPor(DataContext contexto, string numeroOperadora, int chamadoAnatelId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   FISCALIZACAOLINK.CHAMADOANATEL (NOLOCK) 
                                        WHERE  NUMEROOPERADORA = @NUMEROOPERADORA
	                                           and CHAMADOANATELID <> @CHAMADOANATELID ";

            contextQuery.Parameters.Add("@CHAMADOANATELID", SqlDbType.Int, chamadoAnatelId);
            contextQuery.Parameters.Add("@NUMEROOPERADORA", SqlDbType.VarChar, numeroOperadora);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(RN.FiscalizacaoLink.Entidades.ChamadoAnatel chamadoAnatel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO FiscalizacaoLink.CHAMADOANATEL
                                               (CIRCUITOSETORID
                                               ,NUMEROOPERADORA
                                               ,DATAOPERADORA
                                               ,NUMEROANATEL
                                               ,DATAANATEL
                                               ,DATARESOLUCAO
                                               ,SEVERIDADE
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@CIRCUITOSETORID, 
                                               @NUMEROOPERADORA, 
                                               @DATAOPERADORA, 
                                               @NUMEROANATEL, 
                                               @DATAANATEL, 
                                               @DATARESOLUCAO, 
                                               @SEVERIDADE,
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";


                contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, chamadoAnatel.CircuitoSetorId);
                contextQuery.Parameters.Add("@NUMEROOPERADORA", SqlDbType.VarChar, chamadoAnatel.NumeroOperadora);
                contextQuery.Parameters.Add("@DATAOPERADORA", SqlDbType.DateTime, chamadoAnatel.DataOperadora);
                contextQuery.Parameters.Add("@NUMEROANATEL", SqlDbType.VarChar, chamadoAnatel.NumeroAnatel);
                contextQuery.Parameters.Add("@DATAANATEL", SqlDbType.DateTime, chamadoAnatel.DataAnatel);
                contextQuery.Parameters.Add("@DATARESOLUCAO", SqlDbType.DateTime, chamadoAnatel.DataResolucao);
                contextQuery.Parameters.Add("@SEVERIDADE", SqlDbType.VarChar, chamadoAnatel.Severidade);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, chamadoAnatel.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public void Atualiza(RN.FiscalizacaoLink.Entidades.ChamadoAnatel chamadoAnatel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE FISCALIZACAOLINK.CHAMADOANATEL 
                                    SET    DATAANATEL = @DATAANATEL, 
                                           DATAOPERADORA = @DATAOPERADORA, 
                                           DATARESOLUCAO = @DATARESOLUCAO, 
                                           SEVERIDADE = @SEVERIDADE,
                                           USUARIOID = @USUARIOID, 
                                           DATAALTERACAO = @DATAALTERACAO 
                                    WHERE  CHAMADOANATELID = @CHAMADOANATELID  ";

                contextQuery.Parameters.Add("@CHAMADOANATELID", SqlDbType.Int, chamadoAnatel.ChamadoAnatelId);
                contextQuery.Parameters.Add("@DATAANATEL", SqlDbType.DateTime, chamadoAnatel.DataAnatel);
                contextQuery.Parameters.Add("@DATAOPERADORA", SqlDbType.DateTime, chamadoAnatel.DataOperadora);
                contextQuery.Parameters.Add("@DATARESOLUCAO", SqlDbType.DateTime, chamadoAnatel.DataResolucao);
                contextQuery.Parameters.Add("@SEVERIDADE", SqlDbType.VarChar, chamadoAnatel.Severidade);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, chamadoAnatel.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int chamadoAnatelId, DateTime? dataResolucao)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (chamadoAnatelId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            //Registro não pode ser excluído, pois esse chamado já se encontra com data de resolução informada.
            if (dataResolucao != null && dataResolucao != DateTime.MinValue)
            {
                mensagens.Add("Registro não pode ser excluído, pois esse chamado já se encontra com data de resolução.");
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

        public void Remove(int chamadoAnatelId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FISCALIZACAOLINK.CHAMADOANATEL
                            WHERE  CHAMADOANATELID = @CHAMADOANATELID  ";

                contextQuery.Parameters.Add("@CHAMADOANATELID", chamadoAnatelId);

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
