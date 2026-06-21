using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Transporte
{
    public class RotaAluno
    {
        public DataTable ListaPor(int rotaTrajetoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ROTAALUNOID, 
                                       ROTATRAJETOID, 
                                       A.PESSOA,
                                       RA.ALUNO, 
                                       P.NOME_COMPL AS NOME, 
                                       DATAINICIO, 
                                       DATAFIM, 
                                       RA.USUARIOID, 
                                       RA.DATACADASTRO, 
                                       RA.DATAALTERACAO 
                                FROM   [TRANSPORTE].[ROTAALUNO] RA (NOLOCK) 
                                       INNER JOIN LY_ALUNO A (NOLOCK) 
                                               ON RA.ALUNO = A.ALUNO 
                                       INNER JOIN LY_PESSOA P (NOLOCK) 
                                               ON A.PESSOA = P.PESSOA 
                                WHERE  ROTATRAJETOID = @ROTATRAJETOID 
                                        AND SIT_ALUNO = 'Ativo'
                                ORDER BY DATAFIM DESC, P.NOME_COMPL ";

                contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, DateTime.Now.Date);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaIdaPor(int rotaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ROTAALUNOID, 
                                       RA.ROTATRAJETOID, 
                                       A.PESSOA,
                                       RA.ALUNO, 
                                       P.NOME_COMPL AS NOME, 
                                       DATAINICIO, 
                                       DATAFIM, 
                                       RA.USUARIOID, 
                                       RA.DATACADASTRO, 
                                       RA.DATAALTERACAO 
                                FROM   [TRANSPORTE].[ROTAALUNO] RA (NOLOCK) 
									   INNER JOIN Transporte.ROTATRAJETO R (NOLOCK)
											   ON R.ROTATRAJETOID = RA.ROTATRAJETOID
											   AND R.IDA = 1
                                       INNER JOIN LY_ALUNO A (NOLOCK) 
                                               ON RA.ALUNO = A.ALUNO 
                                       INNER JOIN LY_PESSOA P (NOLOCK) 
                                               ON A.PESSOA = P.PESSOA 
                                WHERE  R.ROTAID = @ROTAID
                                ORDER BY DATAFIM DESC, P.NOME_COMPL";

                contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaVoltaPor(int rotaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ROTAALUNOID, 
                                       RA.ROTATRAJETOID, 
                                       A.PESSOA,
                                       RA.ALUNO, 
                                       P.NOME_COMPL AS NOME, 
                                       DATAINICIO, 
                                       DATAFIM, 
                                       RA.USUARIOID, 
                                       RA.DATACADASTRO, 
                                       RA.DATAALTERACAO 
                                FROM   [TRANSPORTE].[ROTAALUNO] RA (NOLOCK) 
									   INNER JOIN Transporte.ROTATRAJETO R (NOLOCK)
											   ON R.ROTATRAJETOID = RA.ROTATRAJETOID
											   AND R.IDA = 0
                                       INNER JOIN LY_ALUNO A (NOLOCK) 
                                               ON RA.ALUNO = A.ALUNO 
                                       INNER JOIN LY_PESSOA P (NOLOCK) 
                                               ON A.PESSOA = P.PESSOA 
                                WHERE  R.ROTAID = @ROTAID
                                ORDER BY DATAFIM DESC, P.NOME_COMPL";

                contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        private bool PossuiAlunoOutroIntervaloPor(DataContext ctx, string aluno, int rotaAlunoId, bool ida, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM   Transporte.ROTAALUNO ra (NOLOCK)
											INNER JOIN TRANSPORTE.ROTATRAJETO RT  (NOLOCK) ON RA.ROTATRAJETOID = RT.ROTATRAJETOID
                                    WHERE  ALUNO = @ALUNO
                                            AND ROTAALUNOID <> @ROTAALUNOID
											and RT.IDA = @IDA
                                            AND (
				                                    (@DATAINICIO <= CONVERT(DATE, DATAINICIO) AND @DATAFIM >= CONVERT(DATE, DATAFIM))
				                                    OR (@DATAINICIO BETWEEN  CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
			                                        OR (@DATAFIM BETWEEN CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
			                                     )  ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ROTAALUNOID", SqlDbType.Int, rotaAlunoId);
            contextQuery.Parameters.Add("@IDA", SqlDbType.Bit, ida);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiAlunoPor(DataContext contexto, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                    FROM Transporte.ROTAALUNO RA (NOLOCK)
										  INNER JOIN Transporte.ROTATRAJETO T (NOLOCK)
												ON RA.ROTATRAJETOID = T.ROTATRAJETOID
                                    WHERE T.ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiAlunoAtivoPor(DataContext contexto, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                    FROM Transporte.ROTAALUNO RA (NOLOCK)
										  INNER JOIN Transporte.ROTATRAJETO T (NOLOCK)
												ON RA.ROTATRAJETOID = T.ROTATRAJETOID
                                    WHERE T.ROTAID = @ROTAID 
                                          AND (@DATA BETWEEN RA.DATAINICIO  AND RA.DATAFIM) ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, DateTime.Now.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiAlunoAtivoPor(DataContext contexto, int rotaId, bool ida)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                    FROM Transporte.ROTAALUNO RA (NOLOCK)
										  INNER JOIN Transporte.ROTATRAJETO T (NOLOCK)
												ON RA.ROTATRAJETOID = T.ROTATRAJETOID
                                    WHERE T.ROTAID = @ROTAID 
                                          AND T.IDA = @IDA
                                          AND (@DATA BETWEEN RA.DATAINICIO  AND RA.DATAFIM) ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@IDA", SqlDbType.Bit, ida);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, DateTime.Now.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.RotaAluno rotaAluno, int tipoCalculoPagamentoId, string turno, int rotaId, bool cadastro, int pessoa, string censo)
        {
            List<string> mensagens = new List<string>();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            Pagamento rnPagamento = new Pagamento();
            RN.Aluno rnAluno = new Aluno();
            RN.Turno rnTurno = new Turno();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();
            int quantidadeAssentos;
            int quantidadeAlunos;
            int veiculoId;
            bool ida;
            Rota rnRota = new Rota();
            Veiculo rnVeiculo = new Veiculo();
            RN.Aluno.DadosAluno dadosAluno = new RN.Aluno.DadosAluno();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (rotaAluno == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (rotaAluno.RotaAlunoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (rotaAluno.RotaTrajetoId <= 0)
            {
                mensagens.Add("Campo ROTA ROJETO é obrigatório.");
            }

            if (pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (tipoCalculoPagamentoId <= 0)
            {
                mensagens.Add("Campo TIPO CALCULO PAGAMENTO é obrigatório.");
            }

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ROTA é obrigatório.");
            }

            if (rotaAluno.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (rotaAluno.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }
            else
            {
                if (rotaAluno.DataInicio.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA INICIO deve ser menor ou igual a data atual.");
                }
            }

            if (rotaAluno.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else
            {
                if (rotaAluno.DataInicio != DateTime.MinValue && rotaAluno.DataInicio.Date > rotaAluno.DataFim.Date)
                {
                    mensagens.Add("A DATA INICIO deve ser menor ou igual a DATA FIM.");
                }
            }

            if (rotaAluno.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a rota pode ser alterada
                    if (!rnRota.PodeEditarAlunoPor(contexto, rotaId))
                    {
                        mensagens.Add("Os alunos não podem ser alterados, pois o periodo para edição terminou.");
                    }
                    else
                    {
                        //Busca modal do aluno
                        string flField05 = rnFlPessoa.ObtemModaisPor(contexto, pessoa);
                        string[] modais = flField05.Split(';');

                        //Verifica o tipo da rota para associação de aluno 
                        if (tipoCalculoPagamentoId == 1) //1 - MARÍTIMO
                        {
                            //Se tipo for maritimo, o aluno precisa ter opção transporte rural com uma opção de Aquaviário/embarcação marcada
                            string aquaviario = rnFlPessoa.ObtemTransporteAquaviarioPor(contexto, pessoa);
                            if (modais.Contains("5") && aquaviario.IsNullOrEmptyOrWhiteSpace()) //Modal 5 - TRANSPORTE RURAL
                            {
                                mensagens.Add("Apenas podem ser associados a esta rota alunos com alguma opção de Transporte Rural - Aquaviário/embarcação marcada.");
                            }
                        }
                        else if (tipoCalculoPagamentoId == 2) //2 - PDE
                        {
                            //Se tipo for pne, aluno tem q ter opção transporte PNE marcada
                            if (modais.Contains("6")) //Modal 6	- PNE
                            {
                                mensagens.Add("Apenas podem ser associados a esta rota alunos com a opção de Transporte PCD marcada.");
                            }
                        }
                        else if (tipoCalculoPagamentoId == 3) //3 - RURAL
                        {
                            //Se tipo for maritimo, aluno tem q ter opção transporte rural com uma opção de Rodoviário marcada                        
                            string rodoviario = rnFlPessoa.ObtemTransporteRodoviarioPor(contexto, pessoa);
                            if (modais.Contains("5") && rodoviario.IsNullOrEmptyOrWhiteSpace()) //Modal 5 - TRANSPORTE RURAL
                            {
                                mensagens.Add("Apenas podem ser associados a esta rota alunos com alguma opção de Transporte Rural - Rodoviário marcada.");
                            }
                        }

                        //Busca tipo do trajeto do aluno
                        ida = rnRotaTrajeto.ObtemTipoTrajetoPor(contexto, rotaAluno.RotaTrajetoId);

                        //Aluno não pode estar associado mais de uma vez no msm periodo e trajeto (ida ou volta)
                        if (PossuiAlunoOutroIntervaloPor(contexto, rotaAluno.Aluno, rotaAluno.RotaAlunoId, ida, rotaAluno.DataInicio, rotaAluno.DataFim))
                        {
                            mensagens.Add("Este aluno já possui associação em outra rota neste periodo (INICIO / FIM).");
                        }

                        //Busca dados do aluno
                        dadosAluno = rnAluno.ObtemDadosAluno(contexto, rotaAluno.Aluno);

                        if (cadastro)
                        {
                            //Verifica se aluno é do mesmo turno da rota
                            if (dadosAluno.Turno != "I") //Alunos do integral podem ser associados em qualquer turno
                            {
                                if (dadosAluno.Turno != turno)
                                {
                                    mensagens.Add(string.Format("Este aluno está cadastrado para o turno {0}.", rnTurno.RetornaDescricaoTurno(dadosAluno.Turno)));
                                }
                            }
                            //Verifica se aluno é da escola da rota
                            if (dadosAluno.UnidadeResponsavel != censo)
                            {
                                mensagens.Add("Este aluno não é desta escola.");
                            }
                        }

                        if (dadosAluno.SitAluno != "Ativo")
                        {
                            mensagens.Add("Este aluno não está ativo.");
                        }

                        //para cadastrar/alterar datas, verificar se ja teve pagamento solicitado no periodo
                        if (rnPagamento.PossuiPagamentoPeriodoPor(contexto, rotaId, rotaAluno.DataInicio, rotaAluno.DataFim))
                        {
                            mensagens.Add("Já foi gerado pagamento, com isso não podem ser associados alunos neste periodo (INICIO / FIM).");
                        }

                        //Busca veiculo
                        veiculoId = rnRotaTrajeto.ObtemVeiculoPor(contexto, rotaAluno.RotaTrajetoId);

                        //Verifica se rota já tem veiculo
                        if (veiculoId > 0)
                        {
                            //Busca Alunos trajeto
                            quantidadeAlunos = this.ObtemAlunosAtivosPor(contexto, rotaAluno.RotaTrajetoId, DateTime.Now.Date);

                            //Busca Quantidade Assentos
                            quantidadeAssentos = rnVeiculo.ObtemQuantidadeAssentosPor(contexto, veiculoId);

                            if (cadastro)
                            {
                                //Caso seja cadastro considera mais 1 aluno que está sendo inserido
                                quantidadeAlunos++;
                            }
                            else
                            {
                                //Em caso de alteração verifica se aluno está ativo
                                if (rotaAluno.DataInicio.Date <= DateTime.Now.Date && rotaAluno.DataFim.Date >= DateTime.Now.Date)
                                {
                                    //Verifica se não era um aluno ativo
                                    if (!this.EhAtivoPor(contexto, rotaAluno.RotaAlunoId, DateTime.Now))
                                    {
                                        quantidadeAlunos++;
                                    }
                                }
                            }

                            //Veiculo tem q ter quantidade de acentos maior ou igual a quantidade de aluno + o motorista
                            if (quantidadeAssentos < (quantidadeAlunos + 1))
                            {
                                mensagens.Add("A quantidade de assentos do veiculo já foi atingida.");
                            }
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

        public void Insere(Entidades.RotaAluno rotaAluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Transporte.ROTAALUNO 
                                                    (ROTATRAJETOID, 
                                                     ALUNO, 
                                                     DATAINICIO, 
                                                     DATAFIM, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@ROTATRAJETOID, 
                                                     @ALUNO, 
                                                     @DATAINICIO, 
                                                     @DATAFIM, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaAluno.RotaTrajetoId);
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, rotaAluno.Aluno);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, rotaAluno.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, rotaAluno.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, rotaAluno.UsuarioId);
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

        public void Atualiza(Entidades.RotaAluno rotaAluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Transporte.ROTAALUNO 
                                            SET DATAINICIO = @DATAINICIO,
                                                DATAFIM = @DATAFIM,
                                                USUARIOID = @USUARIOID,
                                                DATAALTERACAO = @DATAALTERACAO
                                            WHERE ROTAALUNOID = @ROTAALUNOID ";

                contextQuery.Parameters.Add("@ROTAALUNOID", SqlDbType.Int, rotaAluno.RotaAlunoId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, rotaAluno.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, rotaAluno.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, rotaAluno.UsuarioId);
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

        public int ObtemAlunosAtivosPor(int rotaTrajetoId, DateTime data)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemAlunosAtivosPor(contexto, rotaTrajetoId, data);
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

        public int ObtemAlunosAtivosPor(DataContext contexto, int rotaTrajetoId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT COUNT(DISTINCT RA.ALUNO) AS ALUNOS
                                        FROM   TRANSPORTE.ROTAALUNO RA (NOLOCK)
											   INNER JOIN LY_ALUNO A (NOLOCK) 
											        ON A.ALUNO = RA.ALUNO
				                        WHERE ROTATRAJETOID = @ROTATRAJETOID
                                                AND SIT_ALUNO = 'Ativo'
				                                AND (@DATA BETWEEN DATAINICIO AND DATAFIM ) ";

                contextQuery.Parameters.Add("@ROTATRAJETOID", SqlDbType.Int, rotaTrajetoId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ALUNOS"]);
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

        private bool EhAtivoPor(DataContext contexto, int rotaAlunoId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) AS ALUNOS
                                        FROM   TRANSPORTE.ROTAALUNO RA (NOLOCK)
				                        WHERE ROTAALUNOID = @ROTAALUNOID
				                                AND (@DATA BETWEEN DATAINICIO AND DATAFIM ) ";

            contextQuery.Parameters.Add("@ROTAALUNOID", SqlDbType.Int, rotaAlunoId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaRemocao(int rotaAlunoId, int rotaId)
        {
            List<string> mensagens = new List<string>();
            Rota rnRota = new Rota();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (rotaAlunoId <= 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ROTA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a rota foi aprovada
                    if (rnRota.EhAprovadaPor(contexto, rotaId))
                    {
                        mensagens.Add("Os alunos não podem ser excluídos pois está rota já foi aprovada.");
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

        public void Remove(int rotaAlunoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.ROTAALUNO 
                                          WHERE ROTAALUNOID = @ROTAALUNOID ";

                contextQuery.Parameters.Add("@ROTAALUNOID", SqlDbType.Int, rotaAlunoId);

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