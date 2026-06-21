using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class CircuitoSetor
    {
        public DataTable ListaPor(int contratoSetorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT C.CIRCUITOSETORID, 
                                           C.CONTRATOSETORID, 
                                           C.DESIGNACAO, 
										   c.CUSTOMENSAL,
										   c.QUANTIDADEMESES,
										   (C.CUSTOMENSAL * C.QUANTIDADEMESES) AS CUSTOTOTAL,
                                           V.VELOCIDADEID, 
                                           CONVERT(VARCHAR, V.VALOR) + ' ' + U.DESCRICAO AS VELOCIDADE, 
                                           T.TECNOLOGIAID, 
                                           T.DESCRICAO AS TECNOLOGIA,
										   C.INICIO,
										   C.FIM 
                                    FROM   FISCALIZACAOLINK.CIRCUITOSETOR C (NOLOCK) 										   
                                           INNER JOIN FISCALIZACAOLINK.VELOCIDADE V (NOLOCK) 
                                                   ON C.VELOCIDADEID = V.VELOCIDADEID
										   INNER JOIN FISCALIZACAOLINK.UNIDADEVELOCIDADE U (NOLOCK)
												   ON V.UNIDADEVELOCIDADEID = U.UNIDADEVELOCIDADEID
                                           INNER JOIN FISCALIZACAOLINK.TECNOLOGIA T (NOLOCK) 
                                                   ON C.TECNOLOGIAID = T.TECNOLOGIAID 
                                    WHERE c.CONTRATOSETORID = @CONTRATOSETORID ";

                contextQuery.Parameters.Add("@CONTRATOSETORID", SqlDbType.Int, contratoSetorId);

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

        public ValidacaoDados Valida(RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor, bool cadastro, string setor)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.FiscalizacaoLink.Contrato rnContrato = new Contrato();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (circuitoSetor == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (circuitoSetor.CircuitoSetorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (circuitoSetor.ContratoSetorId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DO CONTRATO / UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (circuitoSetor.VelocidadeId <= 0)
            {
                mensagens.Add("Campo VELOCIDADE é obrigatório.");
            }

            if (circuitoSetor.TecnologiaId <= 0)
            {
                mensagens.Add("Campo TECNOLOGIA é obrigatório.");
            }                       

            if (circuitoSetor.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSAVEL é obrigatório.");
            }

            if (circuitoSetor.CustoMensal == null || circuitoSetor.CustoMensal <= 0)
            {
                mensagens.Add("Campo CUSTO MENSAL é obrigatório.");
            }

            if (circuitoSetor.QuantidadeMeses == null || circuitoSetor.QuantidadeMeses <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE MESES é obrigatório.");
            }

            if (circuitoSetor.Inicio == null || circuitoSetor.Inicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }

            if (circuitoSetor.Inicio == null || circuitoSetor.Inicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else if (circuitoSetor.Inicio != null && circuitoSetor.Inicio != DateTime.MinValue)
            {
                if (circuitoSetor.Inicio > circuitoSetor.Fim)
                {
                    mensagens.Add("Campo DATA INICIO deve ser menor ou igual a DATA FIM.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                    
                    //Verifica se existe outro circuito com msm designacao
                    if(this.PossuiOutroCircuitoSetorPor(contexto, circuitoSetor.Designacao, circuitoSetor.CircuitoSetorId,setor))
                    {
                        mensagens.Add("Já existe um link / circuito cadastrado com esta designação.");
                    }

                    //Verificar se existe outro no intervalo da data inicio
                    if (this.PossuiOutroCircuitoPor(contexto, circuitoSetor.CircuitoSetorId, circuitoSetor.ContratoSetorId, Convert.ToDateTime(circuitoSetor.Inicio)))
                    {
                        //só pode haver um link ativo para cada número de contrato por vez
                        mensagens.Add("Já existe outro link / circuito neste intervalo.");
                    }

                    //Verifica se não possui data fim (é ativo)
                    if (circuitoSetor.Fim == null || circuitoSetor.Fim == DateTime.MinValue)
                    {
                        //Verifica se possui outro sem data fim
                        if (this.PossuiOutroCircuitoAtivoPor(contexto, circuitoSetor.CircuitoSetorId, circuitoSetor.ContratoSetorId))
                        {
                            //So permitir incluir um registro para um contrato caso não exista registro vigente, 
                            mensagens.Add("Cada contrato só pode ter a associação de um link/circuito sem data fim por vez. Por favor, verifique se existe outro registro ainda vigente antes de incluir um novo.");
                        }                        
                    }
                    else
                    {
                        //Verificar se existe outro no intervalo da data fim
                        if (this.PossuiOutroCircuitoPor(contexto, circuitoSetor.CircuitoSetorId, circuitoSetor.ContratoSetorId, Convert.ToDateTime(circuitoSetor.Fim)))
                        {
                            mensagens.Add("Cada contrato só pode ter a associação de um link/circuito ativo por vez. Por favor, verifique as informações que está inserindo e ajuste os registros necessários.");
                        }
                    }

                    //Valida se a "data início" do registro desta aba está no período de vigência do contrato (aba "Dados Gerais" considerando 
                    //"Data Contratação" e "Data Término").
                    if (!rnContrato.PossuiContratoPor(contexto, circuitoSetor.ContratoSetorId, Convert.ToDateTime(circuitoSetor.Inicio)))
                    {
                        mensagens.Add("A DATA INICIO deve estar no período de vigência do contrato.");
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

        public RN.FiscalizacaoLink.Entidades.CircuitoSetor ObtemPor(int circuitoSetorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.CircuitoSetor();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                            FROM FISCALIZACAOLINK.CIRCUITOSETOR
                                            WHERE CIRCUITOSETORID = @CIRCUITOSETORID ";

                contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetorId); 

                circuitoSetor = contexto.TryToBindEntity<RN.FiscalizacaoLink.Entidades.CircuitoSetor>(contextQuery);

                return circuitoSetor;
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

        private bool PossuiOutroCircuitoAtivoPor(DataContext ctx, int circuitoSetorId, int contratoSetorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM FiscalizacaoLink.CIRCUITOSETOR(NOLOCK)
                                WHERE CIRCUITOSETORID <> @CIRCUITOSETORID
									AND CONTRATOSETORID = @CONTRATOSETORID							
									AND FIM IS NULL ";

            contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetorId);
            contextQuery.Parameters.Add("@CONTRATOSETORID", SqlDbType.Int, contratoSetorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }   

        private bool PossuiOutroCircuitoPor(DataContext ctx, int circuitoSetorId, int contratoSetorId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM FiscalizacaoLink.CIRCUITOSETOR(NOLOCK)
                                WHERE CIRCUITOSETORID <> @CIRCUITOSETORID
									AND CONTRATOSETORID = @CONTRATOSETORID							
									AND ((FIM IS NOT NULL AND @DATA BETWEEN INICIO
                                                   AND FIM) 
										OR (FIM IS NULL AND @DATA > INICIO)) ";

            contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetorId);
            contextQuery.Parameters.Add("@CONTRATOSETORID", SqlDbType.Int, contratoSetorId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCircuitoSetorPor(DataContext contexto, string desginacao, int circuitoSetorId, string setor)
        {
            ContextQuery contextQuery = new ContextQuery();            
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.CIRCUITOSETOR CI(NOLOCK)
                                    INNER JOIN FiscalizacaoLink.CONTRATOSETOR CS ON CS.CONTRATOSETORID = CI.CONTRATOSETORID
                                    WHERE CI.DESIGNACAO = @DESIGNACAO
                                        AND CS.SETORID = @SETORID
	                                    AND CI.CIRCUITOSETORID <> @CIRCUITOSETORID ";

            contextQuery.Parameters.Add("@DESIGNACAO", SqlDbType.VarChar, desginacao);
            contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);
            contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetorId);            

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiCircuitoSetorPor(DataContext contexto, string setor, int ano, int mes)
        {
            ContextQuery contextQuery = new ContextQuery();
            DateTime dataInicio;
            DateTime dataFim;
            bool existe = false;

            //Monta data para consulta
            dataInicio = new DateTime(ano, mes, 1);
            dataFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            contextQuery.Command = @" SELECT MIN(CS.DATAIMPLANTACAO) AS DATAIMPLANTACAO, 
                                                MAX(ISNULL(CS.DATATERMINO, C.DATATERMINO)) AS DATATERMINO 
                                        INTO   #DATALIMITE 
                                        FROM   FISCALIZACAOLINK.CONTRATOSETOR CS (NOLOCK) 
                                                INNER JOIN FISCALIZACAOLINK.CONTRATO C (NOLOCK) 
                                                        ON CS.CONTRATOID = C.CONTRATOID 
		                                        INNER JOIN FiscalizacaoLink.CIRCUITOSETOR CI (NOLOCK)
				                                        ON CI.CONTRATOSETORID = CS.CONTRATOSETORID
                                        WHERE SETORID = @SETORID 

                                        SELECT COUNT(*)
                                        FROM   #DATALIMITE 
                                        WHERE  @DATAINICIO BETWEEN CONVERT(DATE, DATAIMPLANTACAO) AND 
                                                                CONVERT(DATE, DATATERMINO)
		                                        OR  @DATAFIM BETWEEN CONVERT(DATE, DATAIMPLANTACAO) AND 
                                                                CONVERT(DATE, DATATERMINO)
                                        DROP TABLE #DATALIMITE ";

            contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO FiscalizacaoLink.CIRCUITOSETOR
                                                   (CONTRATOSETORID
                                                   ,VELOCIDADEID
                                                   ,TECNOLOGIAID
                                                   ,DESIGNACAO
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO
                                                   ,CUSTOMENSAL
                                                   ,QUANTIDADEMESES
                                                   ,INICIO
                                                   ,FIM)
                                             VALUES
                                                   (@CONTRATOSETORID, 
                                                   @VELOCIDADEID, 
                                                   @TECNOLOGIAID, 
                                                   @DESIGNACAO, 
                                                   @USUARIOID, 
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO, 
                                                   @CUSTOMENSAL,
                                                   @QUANTIDADEMESES,
                                                   @INICIO,
                                                   @FIM ) ";

                contextQuery.Parameters.Add("@CONTRATOSETORID", SqlDbType.Int, circuitoSetor.ContratoSetorId);
                contextQuery.Parameters.Add("@VELOCIDADEID", SqlDbType.Int, circuitoSetor.VelocidadeId);
                contextQuery.Parameters.Add("@TECNOLOGIAID", SqlDbType.Int, circuitoSetor.TecnologiaId);
                contextQuery.Parameters.Add("@DESIGNACAO", SqlDbType.VarChar, circuitoSetor.Designacao);
                contextQuery.Parameters.Add("@CUSTOMENSAL", SqlDbType.Decimal, circuitoSetor.CustoMensal);
                contextQuery.Parameters.Add("@QUANTIDADEMESES", SqlDbType.Int, circuitoSetor.QuantidadeMeses);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, circuitoSetor.UsuarioId);
                contextQuery.Parameters.Add("@INICIO", SqlDbType.DateTime, Convert.ToDateTime(circuitoSetor.Inicio).Date);
                if (circuitoSetor.Fim == null || circuitoSetor.Fim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@FIM", SqlDbType.DateTime, null);
                }
                else
                {
                    contextQuery.Parameters.Add("@FIM", SqlDbType.DateTime, Convert.ToDateTime(circuitoSetor.Fim).Date);
                }
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

        public void Atualiza(RN.FiscalizacaoLink.Entidades.CircuitoSetor circuitoSetor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE FISCALIZACAOLINK.CIRCUITOSETOR 
                                            SET    VELOCIDADEID = @VELOCIDADEID, 
                                                   TECNOLOGIAID = @TECNOLOGIAID,
                                                   DESIGNACAO = @DESIGNACAO,
                                                   CUSTOMENSAL = @CUSTOMENSAL,
                                                   QUANTIDADEMESES = @QUANTIDADEMESES,
                                                   INICIO = @INICIO,
                                                   FIM = @FIM,
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                            WHERE  CIRCUITOSETORID = @CIRCUITOSETORID   ";

                contextQuery.Parameters.Add("@CIRCUITOSETORID", SqlDbType.Int, circuitoSetor.CircuitoSetorId);
                contextQuery.Parameters.Add("@VELOCIDADEID", SqlDbType.Int, circuitoSetor.VelocidadeId);
                contextQuery.Parameters.Add("@TECNOLOGIAID", SqlDbType.Int, circuitoSetor.TecnologiaId);
                contextQuery.Parameters.Add("@DESIGNACAO", SqlDbType.VarChar, circuitoSetor.Designacao);
                contextQuery.Parameters.Add("@CUSTOMENSAL", SqlDbType.Decimal, circuitoSetor.CustoMensal);
                contextQuery.Parameters.Add("@QUANTIDADEMESES", SqlDbType.Int, circuitoSetor.QuantidadeMeses);
                contextQuery.Parameters.Add("@INICIO", SqlDbType.DateTime, Convert.ToDateTime(circuitoSetor.Inicio).Date);
                if (circuitoSetor.Fim == null || circuitoSetor.Fim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@FIM", SqlDbType.DateTime, null);
                }
                else
                {
                    contextQuery.Parameters.Add("@FIM", SqlDbType.DateTime, Convert.ToDateTime(circuitoSetor.Fim).Date);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, circuitoSetor.UsuarioId);
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

        public bool PossuiContratoSetorTecnologiaPor(DataContext contexto, int tecnologiaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.CIRCUITOSETOR 
                                    where TECNOLOGIAID = @TECNOLOGIAID ";

            contextQuery.Parameters.Add("@TECNOLOGIAID", tecnologiaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiContratoSetorVelocidadePor(DataContext contexto, int velocidadeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM FISCALIZACAOLINK.CIRCUITOSETOR 
                                    where VELOCIDADEID = @VELOCIDADEID ";

            contextQuery.Parameters.Add("@VELOCIDADEID", velocidadeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaRemocao(int circuitoSetorId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.FiscalizacaoLink.Avaliacao rnAvaliacao = new Avaliacao();
            RN.FiscalizacaoLink.ChamadoAnatel rnChamadoAnatel = new ChamadoAnatel();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (circuitoSetorId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se tem interrupção
                    if (rnAvaliacao.PossuiCircuitoInterrupcaoPor(contexto, circuitoSetorId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já possui avaliações cadastradas.");
                    }

                    //Verifica se tem chamado
                    if (rnChamadoAnatel.PossuiCircuitoInterrupcaoPor(contexto, circuitoSetorId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já possui chamados cdastrados.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(int circuitoSetorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FISCALIZACAOLINK.CIRCUITOSETOR
                            WHERE  CIRCUITOSETORID = @CIRCUITOSETORID  ";

                contextQuery.Parameters.Add("@CIRCUITOSETORID", circuitoSetorId);

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
