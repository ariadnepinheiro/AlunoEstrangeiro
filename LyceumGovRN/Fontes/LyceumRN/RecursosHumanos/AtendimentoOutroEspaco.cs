using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class AtendimentoOutroEspaco
    {
        public DataTable ListaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ATENDIMENTOOUTROESPACOID,
                                            RE.REGIONAL,
	                                        UE.NOME_COMP AS ESCOLA,
	                                        A.CENSO,
	                                        A.ANO,
	                                        A.PERIODO,
	                                        A.TURMA,
	                                        A.DATAINICIO,
	                                        A.DATAFIM,
	                                        A.TIPO,
	                                        A.LAUDO,
                                            CASE 
                                                WHEN A.LAUDO = 1 THEN 'Sim' 
                                                ELSE 'Não' 
                                            END AS DESCRLAUDO,
                                            
	                                        A.REQUERIMENTO,
                                            CASE 
                                                WHEN A.REQUERIMENTO = 1 THEN 'Sim' 
                                                ELSE 'Não' 
                                            END AS DESCRREQUERIMENTO,
	                                        A.PLANOESPECIAL, 
                                            CASE 
                                                WHEN A.PLANOESPECIAL = 1 THEN 'Sim' 
                                                ELSE 'Não' 
                                            END AS DESCRPLANOESPECIAL,
	                                        NUMEROSEI, 
                                            CASE 
                                                WHEN A.PRORROGACAO = 1 THEN 'Sim' 
                                                ELSE 'Não' 
                                            END AS DESCRPRORROGACAO,
	                                        PRORROGACAO, 
	                                        DESCRICAO
                                        FROM RecursosHumanos.ATENDIMENTOOUTROESPACO A (NOLOCK)
	                                        INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON A.CENSO = UE.UNIDADE_ENS    
                                            INNER JOIN TCE_REGIONAL RE (NOLOCK) ON RE.ID_REGIONAL = UE.ID_REGIONAL
                                        WHERE ALUNO = @ALUNO
                                        ORDER BY A.ANO, A.PERIODO, A.DATAINICIO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

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

        public ValidacaoDados Valida(Entidades.AtendimentoOutroEspaco atendimentoOutroEspaco, bool cadastro, DateTime dataMatricula)
        {
            List<string> mensagens = new List<string>();
            FlPessoa rnFlPessoa = new FlPessoa();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (atendimentoOutroEspaco == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (atendimentoOutroEspaco.AtendimentoOutroEspacoId <= 0)
                {
                    mensagens.Add("Campo CODIGO é obrigatório.");
                }
            }

            if (atendimentoOutroEspaco.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (atendimentoOutroEspaco.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (atendimentoOutroEspaco.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (atendimentoOutroEspaco.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (atendimentoOutroEspaco.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURMA é obrigatório.");
            }

            if (atendimentoOutroEspaco.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (atendimentoOutroEspaco.DataInicio.Date > DateTime.Now.Date)
            {
                mensagens.Add("Campo DATA INÍCIO não pode ser maior que a data atual.");
            }
            else if (atendimentoOutroEspaco.DataInicio.Date < dataMatricula)
            {
                mensagens.Add("Campo DATA INÍCIO não pode ser menor que a data em que o aluno foi enturmado.");
            }

            if (atendimentoOutroEspaco.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (atendimentoOutroEspaco.DataFim < atendimentoOutroEspaco.DataInicio)
            {
                mensagens.Add("Campo DATA FIM não pode ser menor que a DATA INÍCIO.");
            }

            if (atendimentoOutroEspaco.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }
            else if (atendimentoOutroEspaco.Tipo != "Hospitalar" && atendimentoOutroEspaco.Tipo != "Domiciliar")
            {
                mensagens.Add("Campo TIPO é deve ser Hospitalar ou Domiciliar.");
            }

            if (atendimentoOutroEspaco.Laudo == null)
            {
                mensagens.Add("Campo LAUDO é obrigatório.");
            }

            if (atendimentoOutroEspaco.Requerimento == null)
            {
                mensagens.Add("Campo REQUERIMENTO DE ATENDIMENTO ENTREGUE é obrigatório.");
            }

            if (atendimentoOutroEspaco.PlanoEspecial == null)
            {
                mensagens.Add("Campo PLANO ESPECIAL DE ESTUDO ENTREGUE é obrigatório.");
            }

            if (atendimentoOutroEspaco.Prorrogacao == null)
            {
                mensagens.Add("Campo HOUVE PRORROGAÇÃO é obrigatório.");
            }

            if (!atendimentoOutroEspaco.NumeroSei.IsNullOrEmptyOrWhiteSpace() && atendimentoOutroEspaco.NumeroSei.Length > 200)
            {
                mensagens.Add("O campo NÚMERO SEI só pode ter até 200 caracteres.");
            }

            if (!atendimentoOutroEspaco.Descricao.IsNullOrEmptyOrWhiteSpace() && atendimentoOutroEspaco.Descricao.Length > 500)
            {
                mensagens.Add("O campo DESCRIÇÃO só pode ter até 500 caracteres.");
            }

            if (atendimentoOutroEspaco.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Caso o período cadastrado seja abaixo de 15 dias, aparecerá a mensagem com a informação para acessar a tela de Faltas Justificadas.
                    if (atendimentoOutroEspaco.DataFim.Date < atendimentoOutroEspaco.DataInicio.AddDays(15))
                    {
                        mensagens.Add("Para afastamento com 15 dias ou menos, será considerado Faltas Justificadas. Favor utilizar a tela de Faltas Justificadas.");
                    }

                    // Verifica se já existe aluno/turma/datainicio
                    if (this.PossuiOutroCadastroPor(contexto, atendimentoOutroEspaco.Aluno, atendimentoOutroEspaco.Turma, atendimentoOutroEspaco.DataInicio, atendimentoOutroEspaco.AtendimentoOutroEspacoId))
                    {
                        mensagens.Add("Este ALUNO / TURMA / INÍCIO já foi cadastrado.");
                    }

                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, atendimentoOutroEspaco.Aluno, atendimentoOutroEspaco.Turma, atendimentoOutroEspaco.DataInicio, atendimentoOutroEspaco.AtendimentoOutroEspacoId))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outro periodo desse ALUNO / TURMA.");
                    }

                    //Verifica se a data de fim está intercalada com outro
                    if (this.PossuiDataEmOutroIntervaloPor(contexto, atendimentoOutroEspaco.Aluno, atendimentoOutroEspaco.Turma, atendimentoOutroEspaco.DataFim, atendimentoOutroEspaco.AtendimentoOutroEspacoId))
                    {
                        mensagens.Add("DATA FIM não pode estar dentro do intervalo de outro periodo desse ALUNO / TURMA.");
                    }

                    //Verifica se as datas de inicio e de fim estão intercalada com outro
                    if (this.PossuiOutraIntercaladaPor(contexto, atendimentoOutroEspaco.Aluno, atendimentoOutroEspaco.Turma, atendimentoOutroEspaco.DataInicio, atendimentoOutroEspaco.DataFim, atendimentoOutroEspaco.AtendimentoOutroEspacoId))
                    {
                        mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outro periodo desse ALUNO / TURMA.");
                    }

                    //Busca qual tipo de Escolarização em outros espaçoes o aluno possui ( 2 EM DOMICÍLIO / 1 EM HOSPITAL / 3 NÃO RECEBE)
                    string escolarizacaoExterna = rnFlPessoa.ObtemRecebeEscolarizacaoOutroEspacoPor(contexto, atendimentoOutroEspaco.Aluno);

                    //Valida o tipo
                    if (atendimentoOutroEspaco.Tipo == "Hospitalar" && escolarizacaoExterna != "1") //1 - EM HOSPITAL
                    {
                        mensagens.Add("O tipo Hospitalar não pode ser escolhido caso o campo 'Recebe Escolarização em outros Espaços (diferente da escola)' não esteja marcado com a opção 'EM HOSPITAL'.");
                    }
                    else if (atendimentoOutroEspaco.Tipo == "Domiciliar" && escolarizacaoExterna != "2") //2 - EM DOMICÍLIO
                    {
                        mensagens.Add("O tipo Domiciliar não pode ser escolhido caso o campo 'Recebe Escolarização em outros Espaços (diferente da escola)' não esteja marcado com a opção 'EM DOMICÍLIO'.");
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

        private bool PossuiDataEmOutroIntervaloPor(DataContext contexto, string aluno, string turma, DateTime data, int atendimentoOutroEspacoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM  RecursosHumanos.ATENDIMENTOOUTROESPACO (NOLOCK)
                                    WHERE ALUNO = @ALUNO
	                                    AND TURMA = @TURMA
                                        AND ATENDIMENTOOUTROESPACOID <> @ATENDIMENTOOUTROESPACOID
	                                    AND @DATA BETWEEN DATAINICIO AND 
			                                    CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())) ) ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);
            contextQuery.Parameters.Add("@ATENDIMENTOOUTROESPACOID", SqlDbType.Int, atendimentoOutroEspacoId);


            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraIntercaladaPor(DataContext contexto, string aluno, string turma, DateTime dataInicio, DateTime dataFim, int atendimentoOutroEspacoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   RecursosHumanos.ATENDIMENTOOUTROESPACO (NOLOCK)
                                    WHERE ALUNO = @ALUNO
	                                    AND TURMA = @TURMA
	                                    AND ATENDIMENTOOUTROESPACOID <> @ATENDIMENTOOUTROESPACOID
	                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
	                                    AND @DATAFIM >= CONVERT(DATE, DATAFIM) ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ATENDIMENTOOUTROESPACOID", SqlDbType.Int, atendimentoOutroEspacoId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutroCadastroPor(DataContext contexto, string aluno, string turma, DateTime dataInicio, int atendimentoOutroEspacoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RecursosHumanos.ATENDIMENTOOUTROESPACO (NOLOCK)
                                WHERE ALUNO = @ALUNO
                                    AND TURMA = @TURMA
                                    AND DATAINICIO = @DATAINICIO
	                                AND ATENDIMENTOOUTROESPACOID <> @ATENDIMENTOOUTROESPACOID ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);
            contextQuery.Parameters.Add("@ATENDIMENTOOUTROESPACOID", SqlDbType.Int, atendimentoOutroEspacoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.AtendimentoOutroEspaco atendimentoOutroEspaco)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.ATENDIMENTOOUTROESPACO
                                               (ALUNO
                                               ,ANO
                                               ,PERIODO
                                               ,CENSO
                                               ,TURMA
                                               ,DATAINICIO
                                               ,DATAFIM
                                               ,TIPO
                                               ,LAUDO
                                               ,REQUERIMENTO
                                               ,PLANOESPECIAL
                                               ,NUMEROSEI
                                               ,PRORROGACAO
                                               ,DESCRICAO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@ALUNO, 
                                               @ANO, 
                                               @PERIODO, 
                                               @CENSO, 
                                               @TURMA, 
                                               @DATAINICIO, 
                                               @DATAFIM,
                                               @TIPO, 
                                               @LAUDO, 
                                               @REQUERIMENTO, 
                                               @PLANOESPECIAL, 
                                               @NUMEROSEI, 
                                               @PRORROGACAO, 
                                               @DESCRICAO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, atendimentoOutroEspaco.Aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, atendimentoOutroEspaco.Ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, atendimentoOutroEspaco.Periodo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, atendimentoOutroEspaco.Censo);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, atendimentoOutroEspaco.Turma);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, atendimentoOutroEspaco.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, atendimentoOutroEspaco.DataFim.Date);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, atendimentoOutroEspaco.Tipo);
                contextQuery.Parameters.Add("@LAUDO", SqlDbType.Bit, atendimentoOutroEspaco.Laudo);
                contextQuery.Parameters.Add("@REQUERIMENTO", SqlDbType.Bit, atendimentoOutroEspaco.Requerimento);
                contextQuery.Parameters.Add("@PLANOESPECIAL", SqlDbType.Bit, atendimentoOutroEspaco.PlanoEspecial);
                contextQuery.Parameters.Add("@NUMEROSEI", SqlDbType.VarChar, atendimentoOutroEspaco.NumeroSei);
                contextQuery.Parameters.Add("@PRORROGACAO", SqlDbType.Bit, atendimentoOutroEspaco.Prorrogacao);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, atendimentoOutroEspaco.Descricao);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, atendimentoOutroEspaco.UsuarioId);
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

        public void Atualiza(Entidades.AtendimentoOutroEspaco atendimentoOutroEspaco)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RecursosHumanos.ATENDIMENTOOUTROESPACO
                                               SET ANO = @ANO, 
                                                  PERIODO = @PERIODO, 
                                                  CENSO = @CENSO, 
                                                  TURMA = @TURMA, 
                                                  DATAINICIO = @DATAINICIO, 
                                                  DATAFIM = @DATAFIM,
                                                  TIPO = @TIPO,
                                                  LAUDO = @LAUDO,
                                                  REQUERIMENTO = @REQUERIMENTO,
                                                  PLANOESPECIAL = @PLANOESPECIAL,
                                                  NUMEROSEI = @NUMEROSEI,
                                                  PRORROGACAO = @PRORROGACAO,
                                                  DESCRICAO = @DESCRICAO,
                                                  USUARIOID = @USUARIOID,
                                                  DATAALTERACAO = @DATAALTERACAO
                                             WHERE ATENDIMENTOOUTROESPACOID = @ATENDIMENTOOUTROESPACOID ";

                contextQuery.Parameters.Add("@ATENDIMENTOOUTROESPACOID", SqlDbType.Int, atendimentoOutroEspaco.AtendimentoOutroEspacoId);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, atendimentoOutroEspaco.Ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, atendimentoOutroEspaco.Periodo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, atendimentoOutroEspaco.Censo);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, atendimentoOutroEspaco.Turma);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, atendimentoOutroEspaco.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, atendimentoOutroEspaco.DataFim.Date);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, atendimentoOutroEspaco.Tipo);
                contextQuery.Parameters.Add("@LAUDO", SqlDbType.Bit, atendimentoOutroEspaco.Laudo);
                contextQuery.Parameters.Add("@REQUERIMENTO", SqlDbType.Bit, atendimentoOutroEspaco.Requerimento);
                contextQuery.Parameters.Add("@PLANOESPECIAL", SqlDbType.Bit, atendimentoOutroEspaco.PlanoEspecial);
                contextQuery.Parameters.Add("@NUMEROSEI", SqlDbType.VarChar, atendimentoOutroEspaco.NumeroSei);
                contextQuery.Parameters.Add("@PRORROGACAO", SqlDbType.Bit, atendimentoOutroEspaco.Prorrogacao);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, atendimentoOutroEspaco.Descricao);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, atendimentoOutroEspaco.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int atendimentoOutroEspacoId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            Perfil rnPerfil = new Perfil();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (atendimentoOutroEspacoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o usuario tem perfil para excluir 
                    if (!rnPerfil.PossuiPerfilExclusaoAEDHPor(usuarioResponsavel))
                    {
                        mensagens.Add("Este usuário não possui perfil para EXCLUIR ESCOLARIZAÇÃO EM OUTROS ESPAÇOS.");
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

        public void Remove(int atendimentoOutroEspacoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RecursosHumanos.ATENDIMENTOOUTROESPACO
                                        WHERE ATENDIMENTOOUTROESPACOID = @ATENDIMENTOOUTROESPACOID ";

                contextQuery.Parameters.Add("@ATENDIMENTOOUTROESPACOID", SqlDbType.Int, atendimentoOutroEspacoId);

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
