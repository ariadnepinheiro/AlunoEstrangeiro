using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class AlunoLicenca : RNBase
    {
        public DataTable ListaPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT ID_ALUNO_LICENCA, 
                                                ALUNO, 
                                                DISCIPLINA, 
                                                TURMA, 
                                                ANO, 
                                                SEMESTRE, 
                                                DT_INICIO, 
                                                DT_FIM, 
                                                JUSTIFICATIVAFALTAID, 
                                                OBSERVACAO, 
                                                STAMP_ATUALIZACAO
                                        FROM dbo.LY_ALUNO_LICENCA
                                        WHERE ALUNO = @ALUNO
	                                        AND ATIVO = 'S' ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

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

        public ValidacaoDados Valida(Entidades.LyAlunoLicenca alunoLicenca, bool incluirTodas, bool cadastro, DateTime dataMatricula)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunoLicenca == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (alunoLicenca.IdAlunoLicenca <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (alunoLicenca.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (incluirTodas)
            {
                if (cadastro)
                {
                    alunoLicenca.Disciplina = string.Empty;
                    alunoLicenca.Turma = string.Empty;
                }                
            }
            else
            {
                if (alunoLicenca.Disciplina.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo DISCIPLINA é obrigatório.");
                }

                if (alunoLicenca.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TURMA é obrigatório.");
                }
            }           

            if (alunoLicenca.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (alunoLicenca.Semestre < 0)
            {
                mensagens.Add("Campo SEMESTRE é obrigatório.");
            }

            //data não podem ser menores que 1900 e maiores que data atual
            DateTime milnov = new DateTime(1900, 1, 1);

            if (alunoLicenca.DtInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else
            {
                if (alunoLicenca.DtInicio < milnov)
                {
                    mensagens.Add("DATA INÍCIO não pode ser menor que 1900.");
                }

                if (alunoLicenca.DtInicio.Date > DateTime.Now)
                {
                    mensagens.Add("DATA INÍCIO não pode ser maior que a data atual.");
                }

                if (alunoLicenca.DtInicio.Year != alunoLicenca.Ano)
                {
                    mensagens.Add("DATA INÍCIO deve estar dentro do ano letivo.");
                }

                else if (alunoLicenca.DtInicio < dataMatricula)
                {
                    mensagens.Add("Campo DATA INÍCIO não pode ser menor que a data em que o aluno foi enturmado.");
                }
            }

            if (alunoLicenca.DtFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else
            {
                if (alunoLicenca.DtFim.Year != alunoLicenca.Ano)
                {
                    mensagens.Add("DATA FIM deve estar dentro do ano letivo.");
                }

                if (alunoLicenca.DtFim < milnov)
                {
                    mensagens.Add("DATA FIM não pode ser menor que 1900.");
                }
            }

            if (alunoLicenca.DtFim < alunoLicenca.DtInicio)
            {
                mensagens.Add("Campo DATA FIM não pode ser menor que a DATA INÍCIO.");
            }

            if (alunoLicenca.JustificativaFaltaId <= 0)
            {
                mensagens.Add("Campo MOTIVO DA JUSTIFICATIVA é obrigatório.");
            }

            if (!alunoLicenca.Observacao.IsNullOrEmptyOrWhiteSpace())          
            {
                if (alunoLicenca.Observacao.Length > 2000)
                {
                    mensagens.Add("Campo DESCRIÇÃO deve conter no máximo 2000 caracteres!");
                }
            }

            alunoLicenca.Ativo = "S";

            if (alunoLicenca.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //para o motivo: “ATESTADO MÉDICO” (1 - 1	ATESTADO MÉDICO) - Não pode ter prazo superior à 14 dias de licença do aluno.
                    if (alunoLicenca.JustificativaFaltaId == 1 && alunoLicenca.DtFim.Date >= alunoLicenca.DtInicio.AddDays(15))
                    {
                        mensagens.Add("Para ATESTADO MÉDICO com mais de 15 dias, será considerado atendimento domiciliar ou hospitalar (AEDH), favor utilizar a NOVA 'ABA AEDH - ESCOLARIZAÇÃO EM OUTROS ESPAÇOS', no cadastro do aluno.");
                    }

                    //Verifica se já existe a outra cadastrada
                    if (this.PossuiOutraCadastroPor(contexto, alunoLicenca.IdAlunoLicenca, alunoLicenca.Aluno, alunoLicenca.Turma, alunoLicenca.Disciplina, alunoLicenca.Ano, alunoLicenca.Semestre, alunoLicenca.DtInicio, alunoLicenca.DtFim))
                    {
                        mensagens.Add("Os campos 'Disciplina', 'Turma', 'Ano', 'Período', 'Data Início' e 'Data Fim' não podem ser duplicados para um mesmo aluno.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, alunoLicenca.IdAlunoLicenca, alunoLicenca.Aluno, alunoLicenca.Turma, alunoLicenca.Disciplina, alunoLicenca.Ano, alunoLicenca.Semestre, alunoLicenca.DtInicio))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outro periodo desse ALUNO / TURMA.");
                    }

                    //Verifica se a data de fim está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, alunoLicenca.IdAlunoLicenca, alunoLicenca.Aluno, alunoLicenca.Turma, alunoLicenca.Disciplina, alunoLicenca.Ano, alunoLicenca.Semestre, alunoLicenca.DtFim))
                    {
                        mensagens.Add("DATA FIM não pode estar dentro do intervalo de outro periodo desse ALUNO / TURMA.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro
                     if (this.PossuiOutroIntervaloDataPor(contexto, alunoLicenca.IdAlunoLicenca, alunoLicenca.Aluno, alunoLicenca.Turma, alunoLicenca.Disciplina, alunoLicenca.Ano, alunoLicenca.Semestre, alunoLicenca.DtInicio, alunoLicenca.DtFim))
                    {
                        mensagens.Add("Não podem haver, para uma mesma 'Disciplina', 'Turma', 'Ano' e 'Período', 'Data Início' e 'Data Fim' que se intercalem.");
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

        private bool PossuiOutraCadastroPor(DataContext ctx, decimal idAlunoLicenca, string aluno, string turma, string disciplina, decimal ano, decimal semestre, DateTime dataini, DateTime datafim)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            bool existe = false;

            sql.Append(@" SELECT COUNT(1) 
                                    FROM LY_ALUNO_LICENCA 
                                    WHERE ALUNO = @ALUNO                                         
                                        AND ANO = @ANO 
                                        AND SEMESTRE = @SEMESTRE 
                                        AND DT_INICIO = @DT_INICIO
                                        AND DT_FIM = @DT_FIM
                                        AND ATIVO = 'S'
                                        AND ID_ALUNO_LICENCA <> @ID_ALUNO_LICENCA ");

            if (!turma.IsNullOrEmptyOrWhiteSpace())
            {
                sql.Append(@" 
                                         AND TURMA = @TURMA ");
            }

            if (!disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                sql.Append(@" 
                                        AND DISCIPLINA = @DISCIPLINA ");
            }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, semestre);
            contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.DateTime, dataini.Date);
            contextQuery.Parameters.Add("@DT_FIM", SqlDbType.DateTime, datafim.Date);
            contextQuery.Parameters.Add("@ID_ALUNO_LICENCA", SqlDbType.Int, idAlunoLicenca);

            if (!turma.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            }

            if (!disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            }

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiDataEmOutroIntervaloPor(DataContext contexto, decimal idAlunoLicenca, string aluno, string turma, string disciplina, decimal ano, decimal semestre, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            bool possui = false;

             sql.Append(@" SELECT COUNT(*) 
                                    FROM  LY_ALUNO_LICENCA (NOLOCK)
                                    WHERE ID_ALUNO_LICENCA <> @ID_ALUNO_LICENCA
	                                    and ALUNO = @ALUNO
	                                    AND ANO = @ANO 
	                                    AND SEMESTRE = @SEMESTRE
	                                    AND @DATA BETWEEN DT_INICIO AND 
			                                    CONVERT(DATE, CONVERT(DATETIME, ISNULL(DT_FIM, GETDATE())) ) ");

             if (!turma.IsNullOrEmptyOrWhiteSpace())
             {
                 sql.Append(@" 
                                         AND TURMA = @TURMA ");
             }

             if (!disciplina.IsNullOrEmptyOrWhiteSpace())
             {
                 sql.Append(@" 
                                        AND DISCIPLINA = @DISCIPLINA ");
             }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, semestre);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);
            contextQuery.Parameters.Add("@ID_ALUNO_LICENCA", SqlDbType.Int, idAlunoLicenca);

            if (!turma.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            }

            if (!disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            }

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutroIntervaloDataPor(DataContext ctx, decimal idAlunoLicenca, string aluno, string turma, string disciplina, decimal ano, decimal semestre, DateTime dataini, DateTime datafim)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            bool existe = false;

            sql.Append(@" SELECT COUNT(1) 
                                    FROM LY_ALUNO_LICENCA 
                                    WHERE ID_ALUNO_LICENCA <> @ID_ALUNO_LICENCA 
	                                    and DT_INICIO <= @DT_INICIO 
	                                    and DT_FIM >= @DT_FIM
	                                    and ALUNO = @ALUNO
	                                    AND ANO = @ANO 
	                                    AND SEMESTRE = @SEMESTRE ");

            if (!turma.IsNullOrEmptyOrWhiteSpace())
            {
                sql.Append(@" 
                                         AND TURMA = @TURMA ");
            }

            if (!disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                sql.Append(@" 
                                        AND DISCIPLINA = @DISCIPLINA ");
            }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, semestre);
            contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.DateTime, dataini.Date);
            contextQuery.Parameters.Add("@DT_FIM", SqlDbType.DateTime, datafim.Date);
            contextQuery.Parameters.Add("@ID_ALUNO_LICENCA", SqlDbType.Int, idAlunoLicenca);

            if (!turma.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            }

            if (!disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            }

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void InsereTodos(Entidades.LyAlunoLicenca alunoLicenca)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO dbo.LY_ALUNO_LICENCA
                                               (ALUNO
                                               ,DISCIPLINA
                                               ,TURMA
                                               ,ANO
                                               ,SEMESTRE
                                               ,DT_INICIO
                                               ,DT_FIM
                                               ,STAMP_ATUALIZACAO
                                               ,JUSTIFICATIVAFALTAID
                                               ,OBSERVACAO
                                               ,ATIVO
                                               ,USUARIOID
                                               ,DATACADASTRO)
                                               
										SELECT @ALUNO, 
                                               DISCIPLINA, 
                                               TURMA, 
                                               @ANO, 
                                               @SEMESTRE, 
                                               @DT_INICIO, 
                                               @DT_FIM, 
                                               @STAMP_ATUALIZACAO, 
                                               @JUSTIFICATIVAFALTAID, 
                                               @OBSERVACAO, 
                                               'S',
                                               @USUARIOID, 
                                               @DATACADASTRO
									  FROM LY_MATRICULA 
									  WHERE ALUNO = @ALUNO  
										  AND SIT_MATRICULA = 'Matriculado'   
										  AND ANO = @ANO
										  AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, alunoLicenca.Aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, alunoLicenca.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, alunoLicenca.Semestre);
                contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.DateTime, alunoLicenca.DtInicio.Date);
                contextQuery.Parameters.Add("@DT_FIM", SqlDbType.DateTime, alunoLicenca.DtFim.Date);
                contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, alunoLicenca.JustificativaFaltaId);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, alunoLicenca.Observacao);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoLicenca.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

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

        public void Insere(Entidades.LyAlunoLicenca alunoLicenca)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.LY_ALUNO_LICENCA
                                               (ALUNO
                                               ,DISCIPLINA
                                               ,TURMA
                                               ,ANO
                                               ,SEMESTRE
                                               ,DT_INICIO
                                               ,DT_FIM
                                               ,STAMP_ATUALIZACAO
                                               ,JUSTIFICATIVAFALTAID
                                               ,OBSERVACAO
                                               ,ATIVO
                                               ,USUARIOID
                                               ,DATACADASTRO)
                                         VALUES
                                               (@ALUNO, 
                                               @DISCIPLINA, 
                                               @TURMA, 
                                               @ANO, 
                                               @SEMESTRE, 
                                               @DT_INICIO, 
                                               @DT_FIM, 
                                               @STAMP_ATUALIZACAO, 
                                               @JUSTIFICATIVAFALTAID, 
                                               @OBSERVACAO, 
                                               'S' ,
                                               @USUARIOID, 
                                               @DATACADASTRO) ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, alunoLicenca.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, alunoLicenca.Disciplina);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, alunoLicenca.Turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, alunoLicenca.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, alunoLicenca.Semestre);
                contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.DateTime, alunoLicenca.DtInicio.Date);
                contextQuery.Parameters.Add("@DT_FIM", SqlDbType.DateTime, alunoLicenca.DtFim.Date);
                contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, alunoLicenca.JustificativaFaltaId);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, alunoLicenca.Observacao);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoLicenca.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

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

        public void Atualiza(Entidades.LyAlunoLicenca alunoLicenca)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE dbo.LY_ALUNO_LICENCA
                                           SET ALUNO = @ALUNO, 
                                              DISCIPLINA = @DISCIPLINA, 
                                              TURMA = @TURMA, 
                                              ANO = @ANO, 
                                              SEMESTRE = @SEMESTRE,
                                              DT_INICIO = @DT_INICIO, 
                                              DT_FIM = @DT_FIM, 
                                              STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO, 
                                              JUSTIFICATIVAFALTAID = @JUSTIFICATIVAFALTAID, 
                                              OBSERVACAO = @OBSERVACAO, 
                                              USUARIOID = @USUARIOID
                                         WHERE ID_ALUNO_LICENCA = @ID_ALUNO_LICENCA ";

                contextQuery.Parameters.Add("@ID_ALUNO_LICENCA", SqlDbType.Int, alunoLicenca.IdAlunoLicenca);
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, alunoLicenca.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, alunoLicenca.Disciplina);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, alunoLicenca.Turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, alunoLicenca.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, alunoLicenca.Semestre);
                contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.DateTime, alunoLicenca.DtInicio.Date);
                contextQuery.Parameters.Add("@DT_FIM", SqlDbType.DateTime, alunoLicenca.DtFim.Date);
                contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, alunoLicenca.JustificativaFaltaId);
                contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, alunoLicenca.Observacao);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, alunoLicenca.UsuarioId);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);

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

        public ValidacaoDados ValidaRemocao(int idAlunoLicenca)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (idAlunoLicenca <= 0)
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

        public void Remove(int idAlunoLicenca)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_ALUNO_LICENCA
                            WHERE  ID_ALUNO_LICENCA = @ID_ALUNO_LICENCA  ";

                contextQuery.Parameters.Add("@ID_ALUNO_LICENCA", SqlDbType.Int, idAlunoLicenca);

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

        public bool PossuiJustificativaFaltaPor(DataContext contexto, int justificativaFaltaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" select COUNT(1)
                                    from [LY_ALUNO_LICENCA]
                                    where JUSTIFICATIVAFALTAID = @JUSTIFICATIVAFALTAID ";

            contextQuery.Parameters.Add("@JUSTIFICATIVAFALTAID", SqlDbType.Int, justificativaFaltaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiLicencaPor(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM LY_ALUNO_LICENCA (NOLOCK) 
                            WHERE  ALUNO = @ALUNO 
                                   AND TURMA = @TURMA 
                                   AND ANO = @ANO 
                                   AND SEMESTRE = @SEMESTRE  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiLicencaPor(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM LY_ALUNO_LICENCA (NOLOCK) 
                            WHERE  ALUNO = @ALUNO 
                                   AND TURMA = @TURMA 
                                   AND ANO = @ANO 
                                   AND SEMESTRE = @SEMESTRE
								  AND @DATA BETWEEN DT_INICIO AND DT_FIM  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DATA", data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public static QueryTable ConsultarLicencas(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = @"SELECT id_aluno_licenca, 
                                                aluno, 
                                                disciplina, 
                                                turma, 
                                                ano, 
                                                semestre, 
                                                dt_inicio, 
                                                dt_fim, 
                                                justificativafaltaid, 
                                                observacao, 
                                                stamp_atualizacao
                            FROM Ly_aluno_licenca WHERE ATIVO = 'S' AND ALUNO = ? ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }
    }
}
