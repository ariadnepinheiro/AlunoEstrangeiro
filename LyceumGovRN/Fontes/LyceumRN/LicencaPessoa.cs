using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Library;
using System.Web;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class LicencaPessoa : RNBase
    {
        public DataTable ListaPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT V.MATRICULA, 
                                   V.MATRICULA AS MATRICULADESC, 
								   P.IDFUNCIONAL,
								   V.VINCULO,
								   ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,V.VINCULO)), V.MATRICULA) IDVINCULO,
                                   LP.MOTIVO, 
                                   convert(date,LP.DTINI) as DTINI, 
                                    convert(date,LP.DTFIM) as DTFIM, 
                                   LP.PESSOA, 
                                   LP.ORDEM 
                            FROM   LY_VINCULO V (NOLOCK)
                                   INNER JOIN LY_LICENCA_PESSOA LP (NOLOCK)
                                           ON LP.PESSOA = V.PESSOA 
                                            AND LP.ORDEM = V.ORDEM
								   INNER JOIN LY_PESSOA P (NOLOCK)
                                           ON LP.PESSOA = P.PESSOA 
                            WHERE  LP.PESSOA = @PESSOA 
                                   AND ( V.DATA_DESATIVACAO IS NULL 
                                          OR CONVERT(DATE, V.DATA_DESATIVACAO) >= CONVERT(DATE, GETDATE()) ) 
                            ORDER  BY V.MATRICULA, 
                                      LP.DTINI DESC  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

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

        public LyLicencaPessoa ObtemLicencaAtivaPor(string matricula)
        {
            Entidades.LyLicencaPessoa licenca = new Entidades.LyLicencaPessoa();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 LP.PESSOA, LP.ORDEM, LP.DTINI, LP.DTFIM, LP.MOTIVO, LP.DT_RETORNO, LP.STAMP_ATUALIZACAO 
                                    FROM   LY_LICENCA_PESSOA LP (NOLOCK) 
                                           JOIN LY_LICENCAS L (NOLOCK) 
                                             ON L.MOTIVO = LP.MOTIVO 
                                           JOIN LY_VINCULO V (NOLOCK) 
                                             ON LP.PESSOA = V.PESSOA 
                                                AND LP.ORDEM = V.ORDEM 
                                    WHERE  V.MATRICULA = @MATRICULA 
                                           AND V.DATA_DESATIVACAO IS NULL 
                                           AND ISNULL(CAST(LP.DTFIM AS DATE), '') >= 
		                                       ( CASE L.POSSUI_DTFIM 
				                                    WHEN 'N' THEN '' 
				                                    ELSE CAST(GETDATE() AS DATE) 
			                                    END ) 
                                    ORDER  BY LP.DTINI DESC ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    licenca.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    licenca.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    licenca.Dtini = Convert.ToDateTime(reader["DTINI"]);
                    licenca.Motivo  = Convert.ToString(reader["MOTIVO"]);

                    if (reader["DTFIM"] != DBNull.Value)
                    {
                        licenca.Dtfim = Convert.ToDateTime(reader["DTFIM"]);
                    }

                    if (reader["DT_RETORNO"] != DBNull.Value)
                    {
                        licenca.DtRetorno = Convert.ToDateTime(reader["DT_RETORNO"]);
                    }

                    if (reader["STAMP_ATUALIZACAO"] != DBNull.Value)
                    {
                        licenca.StampAtualizacao = Convert.ToDateTime(reader["STAMP_ATUALIZACAO"]);
                    }
                }

                return licenca;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public void FinalizaLicencaSaidaUnidadeAdministrativaPor(DataContext ctx, int pessoa, int ordem, string matricula)
        {
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
            ContextQuery contextQuery = new ContextQuery();

            //Busca numero de docente
            decimal num_func = rnDocentes.ObtemNumFuncPor(matricula);

            if (num_func == 0)
            {
                //Caso não seja docente finaliza licenças da pessoa
                contextQuery.Command = @" UPDATE LY_LICENCA_PESSOA 
                                            SET    DTFIM = CONVERT(DATE, GETDATE()) 
                                            WHERE  PESSOA = @PESSOA 
                                                   AND ORDEM = @ORDEM 
                                                   AND MOTIVO = @MOTIVO 
                                                   AND ( DTFIM IS NULL 
                                                          OR CONVERT(DATE, DTFIM) > CONVERT(DATE, GETDATE()) )  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM ", ordem);

            }
            else
            {
                //Caso seja docente finaliza licenças do docente
                contextQuery.Command = @" UPDATE LY_LICENCA_DOCENTE 
                                        SET    DTFIM = CONVERT(DATE, GETDATE()) 
                                        WHERE  NUM_FUNC = @NUM_FUNC 
                                               AND MOTIVO = @MOTIVO  
                                               AND ( DTFIM IS NULL 
                                                      OR CONVERT(DATE, DTFIM) > CONVERT(DATE, GETDATE()) ) ";

                contextQuery.Parameters.Add("@NUM_FUNC", num_func);
            }

            contextQuery.Parameters.Add("@MOTIVO", "2"); //"Saída da Unidade Administrativa"

            ctx.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaInsercao(Entidades.LyLicencaPessoa licencaPessoa, string matricula, out bool licencaPossuiDataFim, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoDatas = new List<string>();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.VinculoLy rnVinculo = new VinculoLy();
            LyVinculo vinculo = new LyVinculo();
            RN.Licencas rnLicenca = new Licencas();
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            DataContext contexto = null;
            decimal ordem = 0;
            licencaPossuiDataFim = false;
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (licencaPessoa.Pessoa <= 0)
            {
                mensagens.Add("A PESSOA é obrigatória.");
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("A MATRICULA é obrigatória.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("A USUARIO RESPONSAVEL é obrigatória.");
            }

            if (licencaPessoa.Dtini == null || licencaPessoa.Dtini == DateTime.MinValue)
            {
                mensagens.Add("A DATA DE INÍCIO DA SITUAÇÃO é obrigatória.");
            }
            else
            {
                //Verifica se a data incio da situacao é maior que a data atual
                if (licencaPessoa.Dtini > hoje)
                {
                    mensagens.Add("A DATA DE INÍCIO DA SITUAÇÃO não pode ser maior que hoje.");
                }

                if (licencaPessoa.Dtfim != null && licencaPessoa.Dtfim != DateTime.MinValue && licencaPessoa.Motivo != "43")
                {
                    if (licencaPessoa.Dtfim < licencaPessoa.Dtini)
                    {
                        mensagens.Add("A DATA DE FIM DA SITUAÇÃO não pode ser menor que DATA DE INÍCIO DA SITUAÇÃO.");
                    }
                }
            }            

            if (licencaPessoa.Motivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("A SITUAÇÃO é obrigatória.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Buscar Ordem do vinculo ativo da matricula
                    ordem = rnVinculo.ObtemOrdemVinculoAtivoPor(contexto, matricula);

                    //Verificar se servidor possui vinculo ativo                    
                    if (ordem <= 0)
                    {
                        mensagens.Add("Não é possível inserir situação. Servidor não possui vínculo ativo.");
                    }
                    else
                    {
                        licencaPessoa.Ordem = ordem;

                        //Verifica se usuario pode incluir licença                    
                        if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel) && !rnPadroesDeAcessos.PodeIncluirExcluirSituacaoPor(contexto, usuarioResponsavel, licencaPessoa.Motivo))
                        {
                            mensagens.Add("Usuário não tem permissão para incluir esta situação.");
                        }

                        licencaPossuiDataFim = rnLicenca.PossuiDataFimPor(contexto, licencaPessoa.Motivo);                        
                        if (licencaPossuiDataFim)
                        {
                            if (licencaPessoa.Dtfim == null || licencaPessoa.Dtfim == DateTime.MinValue)
                            {
                                mensagens.Add("A DATA DE FIM DA SITUAÇÃO é obrigatória.");
                            }
                        }
                        else
                        {
                            if (licencaPessoa.Dtfim != null && licencaPessoa.Dtfim != DateTime.MinValue)
                            {
                                mensagens.Add("Para esta SITUAÇÃO não é permitido preenchimento da DATA DE FIM.");
                            }
                        }

                        //Verificar se situação é diferente de "RETORNANDO DE AFASTAMENTO - AGUARDANDO ALOCAÇÃO" e "AGUARDANDO PERÍCIA" - que permitem mais de uma licença ativa
                        if (licencaPessoa.Motivo != "100" && licencaPessoa.Motivo != "46")
                        {
                            //Verifica se situacao e diferente de "CARGA HORARIA REDUZIDA" - que permitem mais de uma licença ativa
                            if (licencaPessoa.Motivo != "43")
                            {
                                if (this.ExisteLicencaAtivaPor(contexto, licencaPessoa.Pessoa, licencaPessoa.Ordem))
                                {
                                    mensagens.Add("Já existe situação ativa para esta matricula.");
                                }
                            }
                        }

                        bool existeLicenca = this.ExisteLicencaPor(
                            contexto,
                            licencaPessoa.Dtini,
                            licencaPessoa.Pessoa,
                            licencaPessoa.Ordem
                        );

                        if (existeLicenca)
                        {
                            mensagens.Add("Já existe uma licença cadastrada para este funcionario com esta data de início.");
                        }

                        //Verifica perido maxima da licença
                        int periodo = rnLicenca.ObtemPeriodoLimitePor(contexto, licencaPessoa.Motivo);
                        if (periodo > 0)
                        {
                            DateTime datafim = Convert.ToDateTime(licencaPessoa.Dtfim);
                            DateTime dataini = Convert.ToDateTime(licencaPessoa.Dtini);
                            if ((datafim.Subtract(dataini).Days > periodo))
                            {
                                mensagens.Add("A diferença em dias entre Data de Início da Situação e Data de Fim da Situação não pode ser maior que o período LIMITE DE " + periodo + " DIAS.");
                            }
                        }

                        //Validação de datas intercaladas padrao
                        validacaoDatas = this.ValidaIntercalacaoDatas(contexto, licencaPessoa);
                        if (validacaoDatas.Count > 0)
                        {
                            mensagens.AddRange(validacaoDatas);
                        }

                        vinculo = rnVinculo.ObtemPrimeiroVinculoPor(matricula);

                        if (licencaPessoa.Dtini < vinculo.DataNomeacao)
                        {
                            mensagens.Add("A data de início da licença não pode ser menor que a data de nomeação de Ingresso.");
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

        public void Insere(LyLicencaPessoa licencaPessoa, string matricula, bool licencaPossuiDataFim, string usuarioResponsavel)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(ctx, licencaPessoa);

                if (!licencaPossuiDataFim)
                {
                    //Para licenças definitivas desativa a lotação atualizada
                    rnLotacao.DesativaLotacao(ctx, licencaPessoa.Dtini, usuarioResponsavel, matricula);
                }
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

        public ValidacaoDados ValidaAlteracao(Entidades.LyLicencaPessoa licencaPessoa, string matricula, out bool licencaPossuiDataFim, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.VinculoTce rnVinculo = new VinculoTce();
            RN.Licencas rnLicenca = new Licencas();
            List<string> validacaoDatas = new List<string>();
            licencaPossuiDataFim = false;
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            DataContext contexto = null;
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (licencaPessoa.Pessoa <= 0)
            {
                mensagens.Add("A PESSOA é obrigatória.");
            }
            else if (licencaPessoa.Pessoa <= 0)
            {
                mensagens.Add("A ORDEM é obrigatória.");
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("A MATRICULA é obrigatória.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("A USUARIO RESPONSAVEL é obrigatória.");
            }

            if (licencaPessoa.Dtini == null || licencaPessoa.Dtini == DateTime.MinValue)
            {
                mensagens.Add("A DATA DE INÍCIO DA SITUAÇÃO é obrigatória.");
            }
            else
            {
                if (licencaPessoa.Dtfim != null && licencaPessoa.Dtfim != DateTime.MinValue)
                {
                    if (licencaPessoa.Dtfim < licencaPessoa.Dtini)
                    {
                        mensagens.Add("A DATA DE FIM DA SITUAÇÃO não pode ser menor que DATA DE INÍCIO DA SITUAÇÃO.");
                    }
                }
            }            

            if (licencaPessoa.Motivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("A SITUAÇÃO é obrigatória.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    string padaces = rnPadroesDeAcessos.ObtemPadraoAcessoLicencaPor(usuarioResponsavel, licencaPessoa.Motivo);

                    //Verifica se usuario pode alterar fim da licença
                    if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel)
                        && !rnPadroesDeAcessos.ExistePadraoAcessoPor(contexto, padaces, "Techne.Lyceum.Net.Basico.AlteraDataFinalLicenca", "LyceumNet"))
                    {
                        mensagens.Add("Usuário não tem permissão para alterar data fim da licença.");
                    }

                    licencaPossuiDataFim = rnLicenca.PossuiDataFimPor(contexto, licencaPessoa.Motivo);
                    if (licencaPossuiDataFim)
                    {
                        if (licencaPessoa.Dtfim == null || licencaPessoa.Dtfim == DateTime.MinValue)
                        {
                            mensagens.Add("A DATA DE FIM DA SITUAÇÃO é obrigatória.");
                        }
                    }
                    else
                    {
                        if (licencaPessoa.Dtfim != null && licencaPessoa.Dtfim != DateTime.MinValue)
                        {
                            mensagens.Add("Para esta SITUAÇÃO não é permitido preenchimento da DATA DE FIM.");
                        }
                    }

                    //Verificar se situação é diferente de "RETORNANDO DE AFASTAMENTO - AGUARDANDO ALOCAÇÃO" e "AGUARDANDO PERÍCIA" - que permitem mais de uma licença ativa
                    if (licencaPessoa.Dtfim == null || licencaPessoa.Dtfim == DateTime.MinValue)
                    {
                        if (licencaPessoa.Motivo != "100" && licencaPessoa.Motivo != "46")
                        {
                            //Verifica se situacao e diferente de "CARGA HORARIA REDUZIDA" - que permitem mais de uma licença ativa
                            if (licencaPessoa.Motivo != "43")
                            {
                                if (this.ExisteOutraLicencaAtivaPor(contexto, licencaPessoa.Pessoa, licencaPessoa.Ordem, licencaPessoa.Dtini))
                                {
                                    mensagens.Add("Já existe situação ativa para esta matricula.");
                                }
                            }
                        }
                    }

                    //Verifica perido maxima da licença
                    int periodo = rnLicenca.ObtemPeriodoLimitePor(contexto, licencaPessoa.Motivo);
                    if (periodo > 0)
                    {
                        DateTime datafim = Convert.ToDateTime(licencaPessoa.Dtfim);
                        DateTime dataini = Convert.ToDateTime(licencaPessoa.Dtini);
                        if ((datafim.Subtract(dataini).Days > periodo))
                        {
                            mensagens.Add("A diferença em dias entre Data de Início da Situação e Data de Fim da Situação não pode ser maior que o período LIMITE DE " + periodo + " DIAS.");
                        }
                    }

                    //Validação de datas intercaladas padrao
                    validacaoDatas = this.ValidaIntercalacaoDatas(contexto, licencaPessoa);
                    if (validacaoDatas.Count > 0)
                    {
                        mensagens.AddRange(validacaoDatas);
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

        public void Altera(LyLicencaPessoa licencaPessoa)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.AlteraDataFim(ctx, licencaPessoa);
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

        private List<string> ValidaIntercalacaoDatas(DataContext contexto, LyLicencaPessoa licencaPessoa)
        {
            List<string> mensagens = new List<string>();

            if (this.PossuiDataEmOutroIntervaloLicencaPor(contexto, licencaPessoa.Pessoa, licencaPessoa.Ordem, licencaPessoa.Dtini, licencaPessoa.Dtini))
            {
                mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outra licença.");
            }

            if (licencaPessoa.Dtfim != null && licencaPessoa.Dtfim != DateTime.MinValue)
            {
                if (this.PossuiDataEmOutroIntervaloLicencaPor(contexto, licencaPessoa.Pessoa, licencaPessoa.Ordem, licencaPessoa.Dtini, Convert.ToDateTime(licencaPessoa.Dtfim)))
                {
                    mensagens.Add("DATA FIM não pode estar dentro do intervalo de outra licença.");
                }

                if (this.PossuiOutraLicencaIntercaladaPor(contexto, licencaPessoa.Pessoa, licencaPessoa.Ordem, licencaPessoa.Dtini, Convert.ToDateTime(licencaPessoa.Dtfim)))
                {
                    mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outra licença.");
                }
            }

            return mensagens;
        }

        private bool PossuiDataEmOutroIntervaloLicencaPor(DataContext ctx, decimal pessoa, decimal ordem, DateTime dataInicio, DateTime dataAnalisar)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   LY_LICENCA_PESSOA (NOLOCK) 
                                        WHERE  PESSOA = @PESSOA
                                               AND ORDEM = @ORDEM
                                               AND CONVERT(DATE, DTINI) <> CONVERT(DATE, @DTINI) 
                                               AND @DATA BETWEEN CONVERT(DATE, DTINI) AND CONVERT(DATE, ISNULL(DTFIM, GETDATE()))
                                               AND MOTIVO <> '43'  ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@DTINI", dataInicio);
            contextQuery.Parameters.Add("@DATA", dataAnalisar.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraLicencaIntercaladaPor(DataContext ctx, decimal pessoa, decimal ordem, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_LICENCA_PESSOA  (NOLOCK) 
                             WHERE  PESSOA = @PESSOA 
                                    AND ORDEM = @ORDEM 
                                    AND CONVERT(DATE, DTINI) <> CONVERT(DATE, @DATA_INICIO)
                                    AND @DATA_INICIO <= CONVERT(DATE, DTINI) 
                                    AND @DATA_FIM >= CONVERT(DATE, ISNULL(DTFIM, GETDATE())) 
                                    AND MOTIVO <> '43'";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@DATA_INICIO", dataInicio);
            contextQuery.Parameters.Add("@DATA_FIM", dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public string ObtemMotivoLicencaAtivaPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 MOTIVO 
                                        FROM   LY_LICENCA_PESSOA 
                                        WHERE  PESSOA = @PESSOA 
                                               AND ( DTFIM IS NULL 
                                                      OR CONVERT(DATE, DTFIM) >= CONVERT(DATE, GETDATE()) ) 
                                        ORDER  BY DTFIM DESC ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public bool ExisteLicencaAtivaPor(DataContext ctx, decimal pessoa, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LICENCA_PESSOA (NOLOCK)
                                    WHERE (DTFIM IS NULL OR CONVERT(DATE,DTFIM) >= CONVERT(DATE,GETDATE()))
                                        AND PESSOA = @PESSOA
										AND ORDEM = @ORDEM
                                        AND MOTIVO <> 43";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ORDEM", ordem);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool ExisteOutraLicencaAtivaPor(DataContext ctx, decimal pessoa, decimal ordem, DateTime dataInicio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LICENCA_PESSOA (NOLOCK)
                                    WHERE (DTFIM IS NULL OR CONVERT(DATE,DTFIM) >= CONVERT(DATE,GETDATE()))
                                        AND PESSOA = @PESSOA
										AND ORDEM = @ORDEM
                                        AND CONVERT(DATE, DTINI) <> CONVERT(DATE, @DTINI) ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@DTINI", dataInicio);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool ExisteLicencaPor(DataContext ctx, DateTime datainicio, decimal pessoa, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LICENCA_PESSOA (NOLOCK)
                                    WHERE CONVERT(DATE, DTINI) = @DATAINICIO
                                        AND PESSOA = @PESSOA
										AND ORDEM = @ORDEM";

            contextQuery.Parameters.Add("@DATAINICIO", datainicio.Date);
            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ORDEM", ordem);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        //verifica se funcionatio tem situação "Saída da Unidade Administrativa" ativa
        public bool PossuiLicencaSaidaUAAtivaPor(DataContext ctx, decimal pessoa, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM  LY_LICENCA_PESSOA (NOLOCK) 
                            WHERE  MOTIVO = '2' 
                                    AND (DTFIM IS NULL OR CONVERT(DATE,DTFIM) > CONVERT(DATE,GETDATE()))
                                    AND PESSOA = @PESSOA
                                    AND ORDEM = @ORDEM ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ORDEM", ordem);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void DesativaLicencaSaidaUA(DataContext ctx, decimal pessoa, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_PESSOA
                                SET    DTFIM = CONVERT(DATE,GETDATE()),
	                                   STAMP_ATUALIZACAO = GETDATE()
                                 WHERE  MOTIVO = '2' 
                                    AND (DTFIM IS NULL OR CONVERT(DATE,DTFIM) > CONVERT(DATE,GETDATE()))
                                    AND PESSOA = @PESSOA
                                    AND ORDEM = @ORDEM ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM", ordem);

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
        }

        public void Insere(DataContext ctx, Entidades.LyLicencaPessoa licencaPessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" INSERT INTO dbo.LY_LICENCA_PESSOA
                                            (PESSOA
                                            ,ORDEM
                                            ,DTINI
                                            ,DTFIM
                                            ,MOTIVO
                                            ,DT_RETORNO
                                            ,STAMP_ATUALIZACAO)
                                            VALUES
                                            (@PESSOA
                                            ,@ORDEM
                                            ,@DTINI
                                            ,@DTFIM
                                            ,@MOTIVO
                                            ,@DTRETORNO
                                            ,@STAMP_ATUALIZACAO) ";

                contextQuery.Parameters.Add("@PESSOA", licencaPessoa.Pessoa);
                contextQuery.Parameters.Add("@ORDEM", licencaPessoa.Ordem);
                contextQuery.Parameters.Add("@MOTIVO", licencaPessoa.Motivo);
                contextQuery.Parameters.Add("@DTINI", licencaPessoa.Dtini);
                contextQuery.Parameters.Add("@DTFIM", licencaPessoa.Dtfim);
                contextQuery.Parameters.Add("@DTRETORNO", licencaPessoa.DtRetorno);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

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
        }

        public void AlteraDataFim(DataContext ctx, Entidades.LyLicencaPessoa licencaPessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_PESSOA 
                            SET    DTFIM = @DTFIM, 
                                   STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                            WHERE  PESSOA = @PESSOA 
                                   AND ORDEM = @ORDEM 
                                   AND DTINI = @DTINI ";

                contextQuery.Parameters.Add("@PESSOA", licencaPessoa.Pessoa);
                contextQuery.Parameters.Add("@ORDEM", licencaPessoa.Ordem);
                contextQuery.Parameters.Add("@DTINI", licencaPessoa.Dtini);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

                if (licencaPessoa.Dtfim != DateTime.MinValue && licencaPessoa.Dtfim != null)
                {
                    contextQuery.Parameters.Add("@DTFIM", licencaPessoa.Dtfim);
                }
                else
                {
                    contextQuery.Parameters.Add("@DTFIM", null);
                }

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
        }

        public ValidacaoDados ValidaRemocaoLicencaAtiva(decimal pessoa, DateTime dataInicio, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dataInicio == DateTime.MinValue)
            {
                mensagens.Add("Não existe LICENÇA ATIVA para este servidor");
            }
            else
            {
                if (pessoa <= 0)
                {
                    mensagens.Add("A PESSOA é obrigatória.");
                }

                if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O USUARIO é obrigatório.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Buscar motivo da licença
                    string motivo = this.ObtemMotivoLicencaAtivaPor(contexto, pessoa);

                    if (!motivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Verifica se usuario pode incluir licença                    
                        if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel) && !rnPadroesDeAcessos.PodeIncluirExcluirSituacaoPor(contexto, usuarioResponsavel, motivo))
                        {
                            mensagens.Add("Usuário não tem permissão para incluir esta situação.");
                        }
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

        public ValidacaoDados ValidaRemocao(decimal pessoa, decimal ordem, DateTime dataInicio, string motivo, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dataInicio == DateTime.MinValue)
            {
                mensagens.Add("A DATA INICIO é obrigatória.");
            }

            if (pessoa <= 0)
            {
                mensagens.Add("A PESSOA é obrigatória.");
            }

            if (ordem <= 0)
            {
                mensagens.Add("A ORDEM é obrigatória.");
            }

            if (motivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O MOTIVO é obrigatório.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se usuario pode incluir licença                    
                    if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel) && !rnPadroesDeAcessos.PodeIncluirExcluirSituacaoPor(contexto, usuarioResponsavel, motivo))
                    {
                        mensagens.Add("Usuário não tem permissão para incluir esta situação.");
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

        public void Remove(decimal pessoa, decimal ordem, DateTime dataInicio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_LICENCA_PESSOA 
                                            WHERE  PESSOA = @PESSOA 
                                                    AND ORDEM = @ORDEM
		                                            AND convert(date,DTINI) = @DTINI ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM", ordem);
                contextQuery.Parameters.Add("@DTINI", dataInicio);

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

        public void RemoveMatriculapor(string matricula, decimal pessoa, decimal ordem, DateTime dataInicio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LP FROM LY_LICENCA_PESSOA LP
                                           INNER JOIN LY_VINCULO V ON LP.PESSOA = V.PESSOA
                                            WHERE  LP.PESSOA = @PESSOA 
                                                    AND MATRICULA = @MATRICULA
                                                    AND LP.ORDEM = @ORDEM
		                                            AND convert(date,LP.DTINI) = @DTINI ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@MATRICULA", matricula);
                contextQuery.Parameters.Add("@ORDEM", ordem);
                contextQuery.Parameters.Add("@DTINI", dataInicio);

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

        public LyLicencaPessoa ObtemLicencaAtivaPor(DataContext contexto, decimal pessoa)
        {
            Entidades.LyLicencaPessoa licenca = new Entidades.LyLicencaPessoa();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 LP.PESSOA,
                                                 LP.ORDEM, 
                                                 LP.DTINI, 
                                                 LP.DTFIM, 
                                                 LP.MOTIVO, 
                                                 LP.DT_RETORNO, 
                                                 LP.STAMP_ATUALIZACAO 
                                    FROM   LY_LICENCA_PESSOA (NOLOCK) LP										  
                                           inner JOIN LY_LICENCAS L (NOLOCK) 
												ON L.MOTIVO = LP.MOTIVO 
                                    WHERE  ISNULL(LP.DTFIM, '') >= 
			                                    ( CASE L.POSSUI_DTFIM 
                                                    WHEN 'N' THEN '' 
                                                    ELSE CONVERT(DATE, GETDATE()) 
                                                 END ) 
                                           AND LP.PESSOA = @PESSOA
                                    ORDER  BY LP.DTINI DESC  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    licenca.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    licenca.Dtini = Convert.ToDateTime(reader["DTINI"]);
                    licenca.Motivo = Convert.ToString(reader["MOTIVO"]);

                    if (reader["DTFIM"] != DBNull.Value)
                    {
                        licenca.Dtfim = Convert.ToDateTime(reader["DTFIM"]);
                    }

                    if (reader["DT_RETORNO"] != DBNull.Value)
                    {
                        licenca.DtRetorno = Convert.ToDateTime(reader["DT_RETORNO"]);
                    }

                    if (reader["STAMP_ATUALIZACAO"] != DBNull.Value)
                    {
                        licenca.StampAtualizacao = Convert.ToDateTime(reader["STAMP_ATUALIZACAO"]);
                    }
                }

                return licenca;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiLicencaAtivaMotivo43(DataContext contexto, decimal pessoa)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
            SELECT TOP 1 1
            FROM LY_LICENCA_PESSOA LP (NOLOCK)
            INNER JOIN LY_LICENCAS L (NOLOCK)
                ON L.MOTIVO = LP.MOTIVO
            WHERE LP.PESSOA = @PESSOA
              AND LP.MOTIVO = '43'
              AND ISNULL(LP.DTFIM, '') >=
                    (
                        CASE L.POSSUI_DTFIM
                            WHEN 'N' THEN ''
                            ELSE CONVERT(DATE, GETDATE())
                        END
                    )";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                return reader.Read();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiLicencaMotivo43(DataContext contexto, decimal pessoa, DateTime dtIni)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 1
                                        FROM LY_LICENCA_PESSOA 
                                        WHERE PESSOA = @PESSOA
                                          AND MOTIVO = '43'
			                               AND CONVERT(DATE, DTINI) = @DTINI ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@DTINI", dtIni.Date);

                reader = contexto.GetDataReader(contextQuery);

                return reader.Read();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiLicencaAnteriorComAltaPor(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_LICENCA_PESSOA  (NOLOCK) 
                             WHERE  PESSOA = @PESSOA 
                                    AND MOTIVO IN ('13','15','17')
                                    AND DTFIM = (SELECT MAX(DTFIM) FROM LY_LICENCA_PESSOA  WHERE  PESSOA = @PESSOA )
                           ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void AlteraReducaoCH(DataContext ctx, Entidades.LyLicencaPessoa licencaPessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_PESSOA 
                            SET    DTFIM = @DTFIM, 
                                   STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                           WHERE PESSOA = @PESSOA
                              AND MOTIVO = '43'
                              AND CONVERT(DATE, DTINI) = @DTINI
                                     ";

                contextQuery.Parameters.Add("@PESSOA", licencaPessoa.Pessoa);
                contextQuery.Parameters.Add("@DTINI", licencaPessoa.Dtini.Date);

                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

                if (licencaPessoa.Dtfim != DateTime.MinValue && licencaPessoa.Dtfim != null)
                {
                    contextQuery.Parameters.Add("@DTFIM", licencaPessoa.Dtfim);
                }
                else
                {
                    contextQuery.Parameters.Add("@DTFIM", null);
                }

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
        }

        public void AlteraReducaoCHAtiva(DataContext ctx, Entidades.LyLicencaPessoa licencaPessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_PESSOA 
                            SET    DTFIM = @DTFIM, 
                                   STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                           WHERE PESSOA = @PESSOA
                              AND MOTIVO = '43'
                              AND DTFIM >= GETDATE()
                                     ";

                contextQuery.Parameters.Add("@PESSOA", licencaPessoa.Pessoa);                
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

                if (licencaPessoa.Dtfim != DateTime.MinValue && licencaPessoa.Dtfim != null)
                {
                    contextQuery.Parameters.Add("@DTFIM", licencaPessoa.Dtfim);
                }
                else
                {
                    contextQuery.Parameters.Add("@DTFIM", null);
                }

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
        }

        public void RemoveReducaoCHAtiva(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" DELETE LY_LICENCA_PESSOA 
                           WHERE PESSOA = @PESSOA
                              AND MOTIVO = '43'
                              AND DTFIM >= GETDATE() ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
               
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
        }
    }
}