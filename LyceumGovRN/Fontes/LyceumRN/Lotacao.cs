using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Linq;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class Lotacao : RNBase
    {
        public static string ObterSetorUsuario(string matricula)
        {
            string sql = "select SETOR from ly_lotacao where (data_desativacao is null or convert(date, data_desativacao) > convert(date,getdate())) and matricula = ?";
            return ConsultarCampo(sql, matricula);
        }

        public bool PossuiLotacaoAtivaEscolaPor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM   LY_LOTACAO L (NOLOCK) 
                                           INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                                   ON L.SETOR = UE.SETOR 
                                    WHERE  (DATA_DESATIVACAO IS NULL 
                                              OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())) 
                                           AND L.MATRICULA = @MATRICULA ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_CODIGO, matricula);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiLotacaoAtivaEscolaEmAtividadePor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT  COUNT(1) 
                                    FROM   LY_LOTACAO L (NOLOCK) 
                                           INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                                   ON L.SETOR = UE.SETOR 
                                    WHERE  (DATA_DESATIVACAO IS NULL 
                                              OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())) 
                                           AND L.MATRICULA = @MATRICULA
										   AND UE.SIT_FUNCIONAMENTO = 'EmAtividade' ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_CODIGO, matricula);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static string ConsultarFuncao(string matricula)
        {
            string sql = @"select TOP 1 F.funcao from ly_lotacao L INNER JOIN ly_funcao F ON F.funcao = L.funcao where matricula = ? AND (data_desativacao is null OR convert(date,l.data_desativacao) > convert(date,GetDate()))";
            return ConsultarCampo(sql, matricula);
        }

        public static DataTable ConsultaDatas(string matricula)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"select matricula,MIN(data_nomeacao) as data_nomeacao
                                , (select top 1 DATA_DESATIVACAO from LY_LOTACAO LD where L.MATRICULA =LD.MATRICULA order by ORDEM desc) as DATA_DESATIVACAO
                                from LY_LOTACAO L
                                where MATRICULA = @MATRICULA
                                group by matricula
                            "
                };
                contextQuery.Parameters.Add("@MATRICULA", matricula);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool ExisteMatriculaExtraClasseAtivaPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                        FROM   LY_LOTACAO L (NOLOCK)
                               INNER JOIN LY_FUNCAO F (NOLOCK)
                                       ON F.FUNCAO = L.FUNCAO 
                        WHERE  MATRICULA = @MATRICULA 
                               AND CAMPO_02 = 'S' 
                               AND L.DATA_NOMEACAO <= CONVERT(DATE, GETDATE()) 
                               AND ( L.DATA_DESATIVACAO IS NULL 
                                      OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) ) ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        private bool PossuiLotacaoAtivaPor(DataContext ctx, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_LOTACAO
                                WHERE   MATRICULA = @MATRICULA
                                        AND (DATA_DESATIVACAO IS NULL
                                            OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()))
                                        AND CONVERT(DATE,DATA_NOMEACAO) <= CONVERT(DATE,GETDATE()) ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiLotacaoSemDataDesativacaoPor(DataContext ctx, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_LOTACAO
                                WHERE   MATRICULA = @MATRICULA
                                        AND (DATA_DESATIVACAO IS NULL
                                            OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiLotacaoFuturaPor(DataContext ctx, decimal pessoa, string matricula, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
		                    FROM LY_LOTACAO (NOLOCK)
		                    WHERE PESSOA = @PESSOA
		                        AND MATRICULA = @MATRICULA 
		                        AND ORDEM = @ORDEM
		                        AND CONVERT(DATE,DATA_NOMEACAO) > CONVERT(DATE,GETDATE()) ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraLotacaoAbertaPor(DataContext ctx, string matricula, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_LOTACAO
                                WHERE   MATRICULA = @MATRICULA
                                        AND ORDEM <> @ORDEM
                                        AND (DATA_DESATIVACAO IS NULL
                                            OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())) ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public DateTime? ObtemUltimaDataDesativacaoPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DateTime? retorno = null;

            try
            {
                contextQuery.Command = @" SELECT DATA_DESATIVACAO 
                                        FROM   LY_LOTACAO L (NOLOCK)
                                        WHERE  L.MATRICULA = @MATRICULA
	                                           AND DATA_NOMEACAO = (SELECT MAX(DATA_NOMEACAO) AS DATA_NOMEACAO
                                                        FROM   LY_LOTACAO (NOLOCK) 
                                                        WHERE  MATRICULA = @MATRICULA 
                                                        GROUP  BY MATRICULA) ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        retorno = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }
                }

                return retorno;
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

        public int ObtemNumeroMatriculaDocentePor(DataContext ctx, decimal pessoa, DateTime dataNomeacao)
        {
            int matriculas = 0;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT  COUNT(D.MATRICULA)
                                FROM    LY_DOCENTE D ( NOLOCK )
                                        INNER JOIN LY_LOTACAO L ( NOLOCK ) ON D.PESSOA = L.PESSOA
                                                                              AND D.MATRICULA = L.MATRICULA
                                WHERE   D.PESSOA = @PESSOA
                                        AND CONVERT(DATE,L.DATA_NOMEACAO) <= CONVERT(DATE, @DATA_NOMEACAO)
                                        AND ( L.DATA_DESATIVACAO IS NULL
                                              OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())
                                            ) "
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@DATA_NOMEACAO", dataNomeacao);

            matriculas = ctx.GetReturnValue<int>(contextQuery);

            return matriculas;
        }

        public int ObtemNumeroMatriculaAtivaPor(DataContext ctx, decimal pessoa)
        {
            int matriculas = 0;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(DISTINCT MATRICULA) 
                    FROM   LY_LOTACAO L (NOLOCK) 
                    WHERE  L.PESSOA = @PESSOA 
                           AND CONVERT(DATE, L.DATA_NOMEACAO) <= CONVERT(DATE, GETDATE()) 
                           AND ( L.DATA_DESATIVACAO IS NULL 
                                  OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) ) "
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            matriculas = ctx.GetReturnValue<int>(contextQuery);

            return matriculas;
        }

        private bool PossuiDataNomeacaoEmOutroIntervaloLotacaoPor(DataContext ctx, string matricula, DateTime data, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                    FROM   LY_LOTACAO (NOLOCK)
                    WHERE  MATRICULA = @MATRICULA 
                            AND ORDEM <> @ORDEM
                            AND @DATA BETWEEN CONVERT(DATE, DATA_NOMEACAO) AND 
                                                                   CONVERT(DATE, ISNULL(DATA_DESATIVACAO, GETDATE()) - 1) ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(DataContext ctx, string matricula, DateTime data, decimal ordem, decimal ordemProximaLotacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LOTACAO 
                                    WHERE  MATRICULA = @MATRICULA 
                                           AND ORDEM <> @ORDEM
                                           AND ORDEM <> @ORDEM_PROXIMA_LOTACAO
                                           AND @DATA BETWEEN 
                                               CONVERT(DATE, DATA_NOMEACAO + 1) AND CONVERT( 
                                               DATE, ISNULL(DATA_DESATIVACAO, GETDATE()))  ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@ORDEM_PROXIMA_LOTACAO", ordemProximaLotacao);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(DataContext ctx, string matricula, DateTime data, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LOTACAO 
                                    WHERE  MATRICULA = @MATRICULA 
                                            AND ORDEM <> @ORDEM
                                            AND @DATA BETWEEN 
                                                CONVERT(DATE, DATA_NOMEACAO + 1) AND CONVERT( 
                                                DATE, ISNULL(DATA_DESATIVACAO, GETDATE()))  ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraLotacaoIntercaladaPor(DataContext ctx, string matricula, DateTime dataNomeacao, DateTime dataDesativacao, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   LY_LOTACAO (NOLOCK)
                        WHERE  MATRICULA = @MATRICULA 
                               AND ORDEM <> @ORDEM
                               AND @DATA_NOMEACAO <= CONVERT(DATE, DATA_NOMEACAO) 
                               AND @DATA_DESATIVACAO >= CONVERT(DATE, ISNULL(DATA_DESATIVACAO, GETDATE())) ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@DATA_NOMEACAO", dataNomeacao);
            contextQuery.Parameters.Add("@DATA_DESATIVACAO", dataDesativacao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraLotacaoIntercaladaPor(DataContext ctx, string matricula, DateTime dataNomeacao, DateTime dataDesativacao, decimal ordem, decimal ordemProximaLotacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   LY_LOTACAO 
                        WHERE  MATRICULA = @MATRICULA 
                               AND ORDEM <> @ORDEM
                               AND ORDEM <> @ORDEMPROXIMALOTACAO
                               AND @DATA_NOMEACAO <= CONVERT(DATE, DATA_NOMEACAO) 
                               AND @DATA_DESATIVACAO >= CONVERT(DATE, ISNULL(DATA_DESATIVACAO, GETDATE())) ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);
            contextQuery.Parameters.Add("@ORDEMPROXIMALOTACAO", ordemProximaLotacao);
            contextQuery.Parameters.Add("@DATA_NOMEACAO", dataNomeacao);
            contextQuery.Parameters.Add("@DATA_DESATIVACAO", dataDesativacao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOrdemPor(DataContext ctx, string matricula, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM    LY_LOTACAO
                                    WHERE   MATRICULA = @MATRICULA
		                                    AND ORDEM = @ORDEM ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@ORDEM", ordem);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiLotacaoAtivaPor(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiLotacaoAtivaPor(ctx, matricula);
                return possui;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public ValidacaoDados ValidaInsercaoLotacaoFuncionario(LyLotacao lotacao, RN.Entidades.LyLicencaPessoa licencaPessoa, bool finalizarAnterior, out LyLotacao lotacaoAnterior, bool licencaPossuiDataFim)
        {
            List<string> mensagens = new List<string>();
            decimal proximaOrdem = 0;
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
            lotacaoAnterior = new LyLotacao();
            LyVinculo vinculo = new LyVinculo();
            RN.VinculoLy rnVinculoLy = new VinculoLy();
            DataContext contexto = null;
            ValidacaoDados validacaoLicenca = new ValidacaoDados();
            List<string> validacaoCamposLotacao = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lotacao == null)
            {
                return validacaoDados;
            }

            lotacao.DataAtualizacao = DateTime.Now;
            if (lotacao.Readaptado.IsNullOrEmptyOrWhiteSpace())
            {
                lotacao.Readaptado = "N";
            }

            //Valida campos obrigatorios gerais de lotação
            validacaoCamposLotacao = this.ValidaCamposObrigatorios(lotacao);
            if (validacaoCamposLotacao.Count > 0)
            {
                mensagens.AddRange(validacaoCamposLotacao);
            }

            //verifica se esta colocando licença para validar campos
            if (!licencaPessoa.Motivo.IsNullOrEmptyOrWhiteSpace() || licencaPessoa.Dtini != DateTime.MinValue)
            {
                //Realiza validações de situação (licença) 
                validacaoLicenca = rnLicencaPessoa.ValidaInsercao(licencaPessoa, lotacao.Matricula, out licencaPossuiDataFim, lotacao.Usuario);

                if (!validacaoLicenca.Valido)
                {
                    mensagens.Add(validacaoLicenca.Mensagem);
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    vinculo = rnVinculoLy.ObtemPrimeiroVinculoPor(lotacao.Matricula);

                    if (lotacao.DataNomeacao < vinculo.DataNomeacao)
                    {
                        mensagens.Add("A data de nomeação da lotação não pode ser menor que a data de nomeação de Ingresso.");
                    }

                    if (this.PossuiOrdemPor(contexto, lotacao.Matricula, lotacao.Ordem))
                    {
                        mensagens.Add("Esta ORDEM já foi utilizada para esta Matricula.");
                    }
                    else
                    {
                        proximaOrdem = lotacao.Ordem;

                        if (finalizarAnterior)
                        {
                            //Alimenta a lotacaoAnterior
                            //Caso a opção de finalizar anterior esteja marcada a lotação anterior será a lotação ativa
                            lotacaoAnterior = this.ObtemLotacaoAtivaPor(contexto, lotacao.Matricula);
                            if (lotacaoAnterior.Matricula.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("A lotação anterior não foi encontrada.");
                            }
                            else
                            {
                                lotacaoAnterior.DataAtualizacao = DateTime.Now;

                                //Data da desativação da lotação anterior, sera a data da nomeação - 1
                                lotacaoAnterior.DataDesativacao = Convert.ToDateTime(lotacao.DataNomeacao).AddDays(-1);

                                lotacao.Ordem = lotacaoAnterior.Ordem; //Usa ordem da lotação q será desativada para validação

                                //Validata data de dispensa da lotacao anterior    
                                if (lotacaoAnterior.DataDesativacao != null)
                                {
                                    if (Convert.ToDateTime(lotacaoAnterior.DataDesativacao).Date <= Convert.ToDateTime(lotacaoAnterior.DataNomeacao).Date)
                                    {
                                        mensagens.Add("Data da Dispensa da lotação anterior (" + Convert.ToDateTime(lotacaoAnterior.DataDesativacao).Date.ToShortDateString() + ") não pode ser menor ou igual a Data da Nomeação (" + Convert.ToDateTime(lotacaoAnterior.DataNomeacao).Date.ToShortDateString() + ").");
                                    }
                                }
                                if (lotacaoAnterior.DataDesativacaoDo != null)
                                {
                                    if (Convert.ToDateTime(lotacao.DataDesativacao).Date > Convert.ToDateTime(lotacao.DataDesativacaoDo).Date)
                                    {
                                        mensagens.Add("Data da Publicação da Dispensa deve ser maior ou igual a Data de Dispensa da lotação anterior (" + Convert.ToDateTime(lotacao.DataDesativacao).Date.ToShortDateString() + ").");
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Alimenta a lotacaoAnterior
                            //Caso a opção de finalizar anterior não esteja marcada a lotação anterior será a ultima (com maior data de nomeação)
                            lotacaoAnterior = this.ObtemUltimaLotacaoPor(contexto, lotacao.Matricula);
                            if (!lotacaoAnterior.Matricula.IsNullOrEmptyOrWhiteSpace())
                            {
                                //Verifica se a data da desativação da lotação anterior, é a data da nomeação - 1
                                if (lotacaoAnterior.DataDesativacao != null)
                                {
                                    if (Convert.ToDateTime(lotacaoAnterior.DataDesativacao).Date != lotacao.DataNomeacao.AddDays(-1).Date)
                                    {
                                        mensagens.Add("A DATA DE NOMEAÇÃO da lotação deve ser a data desativação da lotação anterior + 1.");
                                    }
                                }
                            }
                        }

                        //Validação de datas intercaladas padrao
                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacao);
                        if (validacaoCamposLotacao.Count > 0)
                        {
                            mensagens.AddRange(validacaoCamposLotacao);
                        }

                        lotacao.Ordem = proximaOrdem;

                        if (lotacao.DataDesativacao == null)
                        {
                            if (!finalizarAnterior && this.PossuiLotacaoSemDataDesativacaoPor(contexto, lotacao.Matricula))
                            {
                                mensagens.Add("Já existe uma LOTAÇÃO SEM DATA DE DISPENSA para essa matrícula.");
                            }
                        }

                        if (finalizarAnterior && mensagens.Count == 0)
                        {
                            if (this.PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(contexto, lotacaoAnterior.Matricula, Convert.ToDateTime(lotacaoAnterior.DataDesativacao), lotacaoAnterior.Ordem))
                            {
                                mensagens.Add("DATA DE DISPENSA da lotacao anterior não pode estar dentro do intervalo de outra lotação.");
                            }

                            if (this.PossuiOutraLotacaoIntercaladaPor(contexto, lotacaoAnterior.Matricula, lotacaoAnterior.DataNomeacao, Convert.ToDateTime(lotacaoAnterior.DataDesativacao), lotacaoAnterior.Ordem))
                            {
                                mensagens.Add("DATA DE NOMEAÇÃO E DISPENSA da lotacao anterior não podem intercalar com outra lotação.");
                            }
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

        public void InsereLotacaoFuncionario(LyLotacao lotacao, LyLicencaPessoa licencaPessoa, bool finalizarAnterior, LyLotacao lotacaoAnterior, bool licencaPossuiDataFim)
        {
            RN.LicencaPessoa rnLicencaPessoa = new RN.LicencaPessoa();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Verifica se será necessario finalizar a lotação anterior com data de desativação
                if (finalizarAnterior)
                {
                    //Desativa a lotação anterior
                    this.DesativaLotacao(ctx, Convert.ToDateTime(lotacaoAnterior.DataDesativacao), lotacaoAnterior.Usuario, lotacaoAnterior.Matricula);
                }

                lotacao.DtInicioReadaptacao = null;
                lotacao.DtFimReadaptacao = null;

                //insere lotação
                this.Insere(ctx, lotacao);

                //verifica se existe licença
                if (!licencaPessoa.Motivo.IsNullOrEmptyOrWhiteSpace() && licencaPessoa.Dtini != null)
                {
                    //verifica se já existe essa licença ou outra ativa
                    if (!rnLicencaPessoa.ExisteLicencaPor(ctx, licencaPessoa.Dtini, licencaPessoa.Pessoa, licencaPessoa.Ordem) && !rnLicencaPessoa.ExisteLicencaAtivaPor(ctx, licencaPessoa.Pessoa, licencaPessoa.Ordem))
                    {
                        rnLicencaPessoa.Insere(ctx, licencaPessoa);

                        if (!licencaPossuiDataFim && lotacao.DataDesativacao == null)
                        {
                            //Para licenças definitivas desativa a lotação atualizada
                            this.DesativaLotacao(ctx, licencaPessoa.Dtini, lotacao.Usuario, lotacao.Matricula);
                        }
                    }
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

        public ValidacaoDados ValidaAlteracaoLotacaoFuncionario(LyLotacao lotacao, out LyLotacao proximaLotacao)
        {
            List<string> mensagens = new List<string>();
            LyLotacao lotacaoBase = new LyLotacao();
            proximaLotacao = new LyLotacao();
            DataContext contexto = null;
            List<string> validacaoCamposLotacao = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lotacao == null)
            {
                return validacaoDados;
            }

            //Valida campos obrigatorios gerais de lotação
            validacaoCamposLotacao = this.ValidaCamposObrigatorios(lotacao);
            if (validacaoCamposLotacao.Count > 0)
            {
                mensagens.AddRange(validacaoCamposLotacao);
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiDataNomeacaoEmOutroIntervaloLotacaoPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, lotacao.Ordem))
                    {
                        mensagens.Add("DATA DE NOMEAÇÃO não pode estar dentro do intervalo de outra lotação.");
                    }

                    if (lotacao.DataDesativacao != null)
                    {
                        //Busca lotação como está na base hoje
                        lotacaoBase = this.ObtemLotacaoPor(contexto, lotacao.Pessoa, lotacao.Matricula, lotacao.Ordem);

                        //Verifica se a data de desativação estava nula e a lotacao passa a estar desativada
                        if (lotacao.DataDesativacao != null && lotacao.DataDesativacao <= DateTime.Now && lotacaoBase.DataDesativacao == null)
                        {
                            mensagens.Add("Não é possível desativar a lotação de uma matrícula sem que seja inserido um afastamento sem data fim.");
                        }

                        //Verifica se existe lotaçao posterior a lotação que está sendo alterada
                        proximaLotacao = this.ObtemProximaLotacaoPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, lotacao.Ordem);

                        if (!proximaLotacao.Matricula.IsNullOrEmptyOrWhiteSpace())
                        {
                            proximaLotacao.DataNomeacao = Convert.ToDateTime(lotacao.DataDesativacao).AddDays(1);
                            proximaLotacao.DataAtualizacao = DateTime.Now;
                            proximaLotacao.Usuario = lotacao.Usuario;

                            if (proximaLotacao.DataDesativacao != null)
                            {
                                if (Convert.ToDateTime(proximaLotacao.DataDesativacao).Date <= Convert.ToDateTime(proximaLotacao.DataNomeacao).Date)
                                {
                                    mensagens.Add("Data da Dispensa da PRÓXIMA LOTAÇÃO não pode ser menor ou igual a data da dispensa desta lotação + 1.");
                                }
                            }
                        }

                        if (this.PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(contexto, lotacao.Matricula, Convert.ToDateTime(lotacao.DataDesativacao), lotacao.Ordem, proximaLotacao.Ordem))
                        {
                            mensagens.Add("DATA DE DISPENSA não pode estar dentro do intervalo de outra lotação.");
                        }

                        if (this.PossuiOutraLotacaoIntercaladaPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, Convert.ToDateTime(lotacao.DataDesativacao), lotacao.Ordem, proximaLotacao.Ordem))
                        {
                            mensagens.Add("DATA DE NOMEAÇÃO E DISPENSA não podem intercalar com outra lotação.");
                        }
                    }
                    else
                    {
                        if (this.PossuiOutraLotacaoAbertaPor(contexto, lotacao.Matricula, lotacao.Ordem))
                        {
                            mensagens.Add("Já existe uma LOTAÇÃO SEM DATA DE DISPENSA para esta matricula.");
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

        public void AlteraLotacaoFuncionario(LyLotacao lotacao, LyLotacao proximaLotacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();

            try
            {
                //verifica se tem situação "Saída da Unidade Administrativa" e coloca data fim nela
                if (rnLicencaPessoa.PossuiLicencaSaidaUAAtivaPor(ctx, lotacao.Pessoa, lotacao.Ordem))
                {
                    rnLicencaPessoa.DesativaLicencaSaidaUA(ctx, lotacao.Pessoa, lotacao.Ordem);
                }

                //Verifica se existe lotaçao posterior para ser alterada
                if (!proximaLotacao.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    this.Altera(ctx, proximaLotacao);
                }

                //Altera lotacao
                this.Altera(ctx, lotacao);
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

        private void AlteraDataFimReaptacaoDesativacao(DataContext ctx, DateTime dtDataDesativacao, DateTime dtDataFimReadaptacao, string strUsuario, decimal pessoa, string strMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_LOTACAO
										SET DATA_DESATIVACAO = @DATA_DESATIVACAO, 
                                        DT_FIM_READAPTACAO = @DT_FIM_READAPTACAO,
                                        USUARIO = @USUARIO, 
                                        DATA_ATUALIZACAO = GETDATE()
										WHERE PESSOA = @PESSOA 
                                            AND MATRICULA = @MATRICULA 
                                            AND READAPTADO = 'S'
                                            AND (DATA_DESATIVACAO IS NULL
                                                OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())) ";

                contextQuery.Parameters.Add("@DATA_DESATIVACAO", dtDataDesativacao);
                contextQuery.Parameters.Add("@DT_FIM_READAPTACAO", dtDataFimReadaptacao);
                contextQuery.Parameters.Add("@USUARIO", strUsuario);
                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@MATRICULA", strMatricula);

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

        private void AlteraDataNomeacao(DataContext ctx, DateTime dtDataNomeacao, string strUsuario, string matricula, decimal pessoa, decimal ordem)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_LOTACAO 
                                SET    DATA_NOMEACAO = @DATA_NOMEACAO, 
                                       USUARIO = @USUARIO, 
                                       DATA_ATUALIZACAO = GETDATE(),
                                       DT_INICIO_READAPTACAO = case when READAPTADO = 'S' then  @DATA_NOMEACAO else DT_INICIO_READAPTACAO end
                                WHERE  MATRICULA = @MATRICULA                         
                                       AND PESSOA = @PESSOA
                                       AND ORDEM = @ORDEM ";

                contextQuery.Parameters.Add("@DATA_NOMEACAO", dtDataNomeacao);
                contextQuery.Parameters.Add("@USUARIO", strUsuario);
                contextQuery.Parameters.Add("@MATRICULA", matricula);
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

        public ValidacaoDados ValidaRemocao(decimal pessoa, string matricula, decimal ordem)
        {
            List<string> mensagens = new List<string>();
            LyLotacao lotacao = new LyLotacao();
            decimal numFunc;
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Docentes rnDocente = new Docentes();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("A PESSOA é obrigatória.");
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o MATRICULA.");
            }

            if (ordem <= 0)
            {
                mensagens.Add("A ORDEM é obrigatória.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca NumFunc
                    numFunc = rnDocente.ObtemNumFuncPor(contexto, matricula);

                    //Verifica se é docente
                    if (numFunc > 0)
                    {
                        //Buscar Lotacao
                        lotacao = this.ObtemLotacaoPor(contexto, pessoa, matricula, ordem);

                        //Busca NumFunc
                        numFunc = rnDocente.ObtemNumFuncPor(contexto, matricula);

                        //Verifica se a lotacao removida está ativa
                        if (lotacao.DataDesativacao != null)
                        {
                            if (rnAulaDocente.ExisteAulaAlocadaPeriodoLotacaoPor(contexto, numFunc, lotacao.DataNomeacao, lotacao.DataDesativacao.Value))
                            {
                                mensagens.Add("Existem aulas alocadas para o servidor neste período, não é possível excluir a lotação.");
                            }
                        }
                        else
                        {
                            if (rnAulaDocente.ExisteAulaAlocadaPeriodoLotacaoPor(contexto, numFunc, lotacao.DataNomeacao))
                            {
                                mensagens.Add("Existem aulas alocadas para o servidor neste período, não é possível excluir a lotação.");
                            }
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

        public void Remove(decimal pessoa, string matricula, decimal ordem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"DELETE LY_LOTACAO
                                        WHERE PESSOA = @PESSOA
	                                        AND MATRICULA = @MATRICULA
	                                        AND ORDEM = @ORDEM ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@MATRICULA", matricula);
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
            finally
            {
                ctx.Dispose();
            }
        }

        public ValidacaoDados ValidaAlteracaoFuncaoDocente(decimal pessoa, string matricula, string usuarioResponsavel, string novaFuncao, out LyLotacao lotacaoParaDesativar, out LyLotacao novaLotacao)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoCamposLotacao = new List<string>();
            DataContext contexto = null;
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Docentes rnDocentes = new Docentes();
            LyLotacao lotacaoBanco = new LyLotacao();
            lotacaoParaDesativar = new LyLotacao();
            novaLotacao = new LyLotacao();
            RN.PadraoAcessoFuncao rnPadraoAcessoFuncao = new PadraoAcessoFuncao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("A PESSOA é obrigatória.");
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o MATRICULA.");
            }

            if (novaFuncao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o NOVA FUNÇÃO.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o USUARIO RESPONSAVEL.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca Lotacao Ativa
                    lotacaoBanco = this.ObtemLotacaoAtivaPor(contexto, matricula);
                    if (lotacaoBanco == null || lotacaoBanco.Pessoa <= 0)
                    {
                        mensagens.Add("Servidor não possui lotacao ativa.");
                    }
                    else
                    {

                        //verifica se o usuário pode excluir a função
                        if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel) && !rnPadraoAcessoFuncao.PossuiAcessoExcluirFuncaoPor(contexto, usuarioResponsavel, lotacaoBanco.Funcao))
                        {
                            mensagens.Add("Usuário não tem permissão para excluir a função atual do docente.");
                        }

                        if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel) && !rnPadraoAcessoFuncao.PossuiAcessoExcluirFuncaoPor(contexto, usuarioResponsavel, novaFuncao))
                        {
                            mensagens.Add("Usuário não tem permissão para associar esta função.");
                        }

                        //Monta lotação que será desativada
                        lotacaoParaDesativar = lotacaoBanco;
                        lotacaoParaDesativar.DataDesativacao = DateTime.Today.AddDays(-1);
                        lotacaoParaDesativar.Usuario = usuarioResponsavel;
                        lotacaoParaDesativar.DataAtualizacao = DateTime.Now;

                        if (Convert.ToDateTime(lotacaoParaDesativar.DataDesativacao).Date <= Convert.ToDateTime(lotacaoParaDesativar.DataNomeacao).Date)
                        {
                            mensagens.Add("Data da Dispensa da lotação anterior (" + Convert.ToDateTime(lotacaoParaDesativar.DataDesativacao).Date.ToShortDateString() + ") não pode ser menor ou igual a Data da Nomeação (" + Convert.ToDateTime(lotacaoParaDesativar.DataNomeacao).Date.ToShortDateString() + ").");
                        }

                        //Validação de datas intercaladas padrao
                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacaoParaDesativar);
                        if (validacaoCamposLotacao.Count > 0)
                        {
                            mensagens.AddRange(validacaoCamposLotacao);
                        }

                        //Monta lotação que será inserida
                        novaLotacao.Pessoa = pessoa;
                        novaLotacao.Matricula = matricula;
                        novaLotacao.Ordem = lotacaoBanco.Ordem; //Comeca com ordem atual para validar data corretamenta
                        novaLotacao.UnidadeEns = lotacaoBanco.UnidadeEns;
                        novaLotacao.Nucleo = lotacaoBanco.Nucleo;
                        novaLotacao.UnidadeFis = lotacaoBanco.UnidadeFis;
                        novaLotacao.Setor = lotacaoBanco.Setor;
                        novaLotacao.Categoria = rnDocentes.ObtemCategoriaPor(contexto, matricula);
                        novaLotacao.Funcao = novaFuncao;
                        novaLotacao.Readaptado = "N";
                        novaLotacao.DataNomeacao = DateTime.Today;
                        novaLotacao.DataDesativacao = null;
                        novaLotacao.DtInicioReadaptacao = null;
                        novaLotacao.DtFimReadaptacao = null;
                        novaLotacao.Usuario = usuarioResponsavel;
                        novaLotacao.DataAtualizacao = DateTime.Now;
                        novaLotacao.DataNomeacaoDo = null;
                        novaLotacao.DataDesativacaoDo = null;
                        novaLotacao.AtoOficial = null;
                        novaLotacao.RespDocumentacao = null;

                        //Validação de datas intercaladas padrao
                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, novaLotacao);
                        if (validacaoCamposLotacao.Count > 0)
                        {
                            mensagens.AddRange(validacaoCamposLotacao);
                        }

                        novaLotacao.Ordem = this.ObtemProximaOrdemPor(contexto, matricula);

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

        public void AlteraFuncaoDocente(LyLotacao lotacaoParaDesativar, LyLotacao novaLotacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //desativará a lotacao atual e vai inserir outra com a nova função

                //Desativa lotacao autal
                this.DesativaLotacao(contexto, Convert.ToDateTime(lotacaoParaDesativar.DataDesativacao), lotacaoParaDesativar.Usuario, lotacaoParaDesativar.Matricula);

                //Insere nova lotacao
                this.Insere(contexto, novaLotacao);
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

        public ValidacaoDados ValidaRemocaoLotacaoFuncao(DadosExclusaoFuncaoLotacao dadosExclusaoFuncaoLotacao)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoCamposLotacao = new List<string>();
            DataContext contexto = null;
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Docentes rnDocentes = new Docentes();
            LyLotacao lotacaoBanco = new LyLotacao();
            LyLotacao lotacaoParaDesativar = new LyLotacao();
            LyLotacao lotacao = new LyLotacao();
            string futuraFuncao = null;
            RN.PadraoAcessoFuncao rnPadraoAcessoFuncao = new PadraoAcessoFuncao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosExclusaoFuncaoLotacao.Pessoa <= 0)
            {
                mensagens.Add("A PESSOA é obrigatória.");
            }

            if (dadosExclusaoFuncaoLotacao.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o MATRICULA.");
            }

            if (dadosExclusaoFuncaoLotacao.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o USUARIO RESPONSAVEL.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca Lotacao Ativa
                    lotacaoBanco = this.ObtemLotacaoAtivaPor(contexto, dadosExclusaoFuncaoLotacao.Matricula);
                    if (lotacaoBanco == null || lotacaoBanco.Pessoa <= 0)
                    {
                        mensagens.Add("Servidor não possui lotacao ativa.");
                    }

                    //verifica se o usuário pode excluir a função
                    if (!rnUsuarios.EhPrivilegiado(contexto, dadosExclusaoFuncaoLotacao.UsuarioResponsavel) && !rnPadraoAcessoFuncao.PossuiAcessoExcluirFuncaoPor(contexto, dadosExclusaoFuncaoLotacao.UsuarioResponsavel, lotacaoBanco.Funcao))
                    {
                        mensagens.Add("Usuário não tem permissão para excluir esta função.");
                    }

                    //Monta lotação que será desativada
                    lotacaoParaDesativar = lotacaoBanco;
                    lotacaoParaDesativar.DataDesativacao = DateTime.Today.AddDays(-1);
                    lotacaoParaDesativar.Usuario = dadosExclusaoFuncaoLotacao.UsuarioResponsavel;
                    lotacaoParaDesativar.DataAtualizacao = DateTime.Now;

                    if (Convert.ToDateTime(lotacaoParaDesativar.DataDesativacao).Date <= Convert.ToDateTime(lotacaoParaDesativar.DataNomeacao).Date)
                    {
                        mensagens.Add("Data da Dispensa da lotação anterior (" + Convert.ToDateTime(lotacaoParaDesativar.DataDesativacao).Date.ToShortDateString() + ") não pode ser menor ou igual a Data da Nomeação (" + Convert.ToDateTime(lotacaoParaDesativar.DataNomeacao).Date.ToShortDateString() + ").");
                    }

                    //Validação de datas intercaladas padrao
                    validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacaoParaDesativar);
                    if (validacaoCamposLotacao.Count > 0)
                    {
                        mensagens.AddRange(validacaoCamposLotacao);
                    }

                    dadosExclusaoFuncaoLotacao.LotacaoParaDesativar = lotacaoParaDesativar;

                    //Verifica se é docente
                    if (dadosExclusaoFuncaoLotacao.NumFunc > 0)
                    {
                        if (rnAulaDocente.ExisteAulaAlocadaPor(contexto, dadosExclusaoFuncaoLotacao.NumFunc, lotacaoBanco.DataDesativacao.Value))
                        {
                            mensagens.Add("Existem aulas alocadas para o servidor neste período, não é possível excluir a função.");
                        }

                        //função da categoria
                        futuraFuncao = rnDocentes.ObtemFuncaoCategoriaPor(contexto, dadosExclusaoFuncaoLotacao.Matricula);
                        if (string.IsNullOrEmpty(futuraFuncao))
                        {
                            mensagens.Add("Não existe função relacionada ao cargo do docente.");
                        }

                        //Monta lotação que será inserida
                        lotacao.Pessoa = dadosExclusaoFuncaoLotacao.Pessoa;
                        lotacao.Matricula = dadosExclusaoFuncaoLotacao.Matricula;
                        lotacao.Ordem = lotacaoBanco.Ordem; //Comeca com ordem atual para validar data corretamenta
                        lotacao.UnidadeEns = lotacaoBanco.UnidadeEns;
                        lotacao.Nucleo = lotacaoBanco.Nucleo;
                        lotacao.UnidadeFis = lotacaoBanco.UnidadeFis;
                        lotacao.Setor = lotacaoBanco.Setor;
                        lotacao.Categoria = rnDocentes.ObtemCategoriaPor(contexto, dadosExclusaoFuncaoLotacao.Matricula);
                        lotacao.Funcao = futuraFuncao;
                        lotacao.Readaptado = "N";
                        lotacao.DataNomeacao = DateTime.Today;
                        lotacao.DataDesativacao = null;
                        lotacao.DtInicioReadaptacao = null;
                        lotacao.DtFimReadaptacao = null;
                        lotacao.Usuario = dadosExclusaoFuncaoLotacao.UsuarioResponsavel;
                        lotacao.DataAtualizacao = DateTime.Now;
                        lotacao.DataNomeacaoDo = null;
                        lotacao.DataDesativacaoDo = null;
                        lotacao.AtoOficial = null;
                        lotacao.RespDocumentacao = null;

                        //Validação de datas intercaladas padrao
                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacao);
                        if (validacaoCamposLotacao.Count > 0)
                        {
                            mensagens.AddRange(validacaoCamposLotacao);
                        }

                        lotacao.Ordem = this.ObtemProximaOrdemPor(contexto, dadosExclusaoFuncaoLotacao.Matricula);
                        dadosExclusaoFuncaoLotacao.Lotacao = lotacao;
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

        public void RemoveLotacaoFuncao(DadosExclusaoFuncaoLotacao dadosExclusaoFuncaoLotacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Verifica se é docente
                if (dadosExclusaoFuncaoLotacao.NumFunc > 0)
                {
                    //Caso seja docente, desativará a lotacao atual e vai inserir outra com a função original de ly_docente

                    //Desativa lotacao autal
                    this.DesativaLotacao(contexto, Convert.ToDateTime(dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.DataDesativacao), dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Usuario, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Matricula);


                    //verifica se existe lotação com ordem + 1 e altera a data de inicio
                    if (this.PossuiLotacaoFuturaPor(contexto, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Pessoa, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Matricula, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Ordem + 1))
                    {
                        this.AlteraDataNomeacao(contexto, dadosExclusaoFuncaoLotacao.Lotacao.DataNomeacao, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Usuario, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Matricula, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Pessoa, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Ordem + 1);
                    }
                    else
                    {
                        //Insere nova lotacao
                        this.Insere(contexto, dadosExclusaoFuncaoLotacao.Lotacao);
                    }
                }
                else
                {
                    //Caso sejá servidor, desativará a lotacao atual
                    this.DesativaLotacao(contexto, Convert.ToDateTime(dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.DataDesativacao), dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Usuario, dadosExclusaoFuncaoLotacao.LotacaoParaDesativar.Matricula);
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

        public ValidacaoDados ValidaAlteracaoLotacaoSituacaoServidor(DTOs.DadosLotacaoDocenteFuncionario dadosLotacaoDocenteFuncionario)
        {
            List<string> mensagens = new List<string>();
            RN.Docentes rnDocente = new Docentes();
            RN.Licencas rnLicenca = new Licencas();
            RN.Funcao rnFuncao = new Funcao();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            RN.Usuarios rnUsuario = new Usuarios();
            RN.Licencas rnLicencas = new Licencas();
            LyLotacao lotacaoBanco = null;
            LyLotacao lotacaoAntiga = null;
            LyLotacao lotacao = null;
            LyLotacao lotacaoFutura = null;
            LyLicencaPessoa licencaPessoa = null;
            LyLicencaDocente licencaDocente = null;
            bool possuiAulasAlocadas = false;
            DataContext contexto = null;
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            decimal proximaOrdem = 0;
            string futuraFuncao = string.Empty;
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
            RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
            RN.VinculoLy rnVinculoLy = new VinculoLy();
            ValidacaoDados validacaoLicenca = new ValidacaoDados();
            List<string> validacaoCamposLotacao = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosLotacaoDocenteFuncionario == null)
            {
                return validacaoDados;
            }

            if (dadosLotacaoDocenteFuncionario.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o MATRÍCULA.");
            }

            if (dadosLotacaoDocenteFuncionario.Setor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a UNIDADE ADMINISTRATIVA.");
            }
            else
            {
                if (dadosLotacaoDocenteFuncionario.Regional.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Favor informar a REGIONAL.");
                }
            }

            if (dadosLotacaoDocenteFuncionario.Readaptado)
            {
                if (dadosLotacaoDocenteFuncionario.DataInicioReadaptacao == null || dadosLotacaoDocenteFuncionario.DataInicioReadaptacao == DateTime.MinValue)
                {
                    mensagens.Add("DATA INÍCIO READAPTAÇÃO é obrigatória quando Readaptado está marcado.");
                }
                else
                {
                    if (dadosLotacaoDocenteFuncionario.DataFimReadaptacao != null && dadosLotacaoDocenteFuncionario.DataFimReadaptacao != DateTime.MinValue)
                    {
                        if (dadosLotacaoDocenteFuncionario.DataFimReadaptacao <= dadosLotacaoDocenteFuncionario.DataInicioReadaptacao)
                        {
                            mensagens.Add("DATA INÍCIO READAPTAÇÃO não pode ser menor ou igual a DATA FIM READAPTAÇÃO.");
                        }
                    }
                }

                if (dadosLotacaoDocenteFuncionario.DataFimReadaptacao == null || dadosLotacaoDocenteFuncionario.DataFimReadaptacao == DateTime.MinValue)
                {
                    mensagens.Add("DATA FIM READAPTAÇÃO é obrigatória quando Readaptado está marcado.");
                }
                else if (dadosLotacaoDocenteFuncionario.DataFimReadaptacao > Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioReadaptacao).AddYears(5))
                {
                    mensagens.Add("DATA FIM READAPTAÇÃO deve ser no máximo 5 anos maior que Data Início Readaptação.");
                }
            }

            if (dadosLotacaoDocenteFuncionario.Usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o USUÁRIO.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca proxima Ordem
                    proximaOrdem = ObtemProximaOrdemPor(contexto, dadosLotacaoDocenteFuncionario.Matricula);

                    //Busca Lotacao como está hoje na base para verificaçoes
                    lotacaoBanco = this.ObtemLotacaoAtivaPor(contexto, dadosLotacaoDocenteFuncionario.Matricula);
                    if (lotacaoBanco == null || lotacaoBanco.Pessoa <= 0)
                    {
                        mensagens.Add("Está lotação não está mais ativa.");
                    }
                    else
                    {
                        dadosLotacaoDocenteFuncionario.FuncaoAnterior = lotacaoBanco.Funcao;
                        dadosLotacaoDocenteFuncionario.EraReadaptado = lotacaoBanco.Readaptado == "S" ? true : false;
                        dadosLotacaoDocenteFuncionario.DataInicioReadaptacaoAntiga = lotacaoBanco.DtInicioReadaptacao;
                    }

                    //Verifica se data da readaptacao foi alterada
                    if (dadosLotacaoDocenteFuncionario.Readaptado && dadosLotacaoDocenteFuncionario.EraReadaptado)
                    {
                        if (dadosLotacaoDocenteFuncionario.Funcao.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Não é possível alterar a readaptação.");
                        }

                        if (dadosLotacaoDocenteFuncionario.DataInicioReadaptacaoAntiga != dadosLotacaoDocenteFuncionario.DataInicioReadaptacao)
                        {
                            //Verifica se a data incio da situacao é maior que a data atual
                            if (dadosLotacaoDocenteFuncionario.DataInicioReadaptacao > hoje)
                            {
                                mensagens.Add("DATA INÍCIO READAPTAÇÃO não pode ser maior que hoje.");
                            }

                            if (dadosLotacaoDocenteFuncionario.DataInicioReadaptacao < hoje.AddYears(-5))
                            {
                                mensagens.Add("DATA INÍCIO READAPTAÇÃO deve ser no máximo 5 anos menor que hoje.");
                            }
                        }
                    }

                    if (dadosLotacaoDocenteFuncionario.Readaptado && !dadosLotacaoDocenteFuncionario.EraReadaptado)
                    {
                        if (dadosLotacaoDocenteFuncionario.Funcao.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("É necessário informar uma FUNÇÃO para readaptação.");
                        }

                        //Verifica se a data incio da situacao é maior que a data atual
                        if (dadosLotacaoDocenteFuncionario.DataInicioReadaptacao > hoje)
                        {
                            mensagens.Add("DATA INÍCIO READAPTAÇÃO não pode ser maior que hoje.");
                        }

                        if (dadosLotacaoDocenteFuncionario.DataInicioReadaptacao < hoje.AddYears(-5))
                        {
                            mensagens.Add("DATA INÍCIO READAPTAÇÃO deve ser no máximo 5 anos menor que hoje.");
                        }
                    }

                    if (this.PossuiOrdemPor(contexto, dadosLotacaoDocenteFuncionario.Matricula, proximaOrdem))
                    {
                        mensagens.Add("Não foi encontrada proxima ordem válida.");
                    }
                    else
                    {
                        //Buscar numero de docente
                        if (dadosLotacaoDocenteFuncionario.NumFunc > 0)
                        {
                            possuiAulasAlocadas = rnAulaDocente.ObtemTotalAulasAlocadasPor(contexto, dadosLotacaoDocenteFuncionario.NumFunc, DateTime.Now) > 0;
                        }

                        dadosLotacaoDocenteFuncionario.PossuiAulasAlocadas = possuiAulasAlocadas;

                        if (!possuiAulasAlocadas
                            && !rnFuncao.EhFuncaoDiretor(contexto, dadosLotacaoDocenteFuncionario.FuncaoAnterior)
                            && !rnFuncao.EhFuncaoSecretario(contexto, dadosLotacaoDocenteFuncionario.FuncaoAnterior)
                            && !dadosLotacaoDocenteFuncionario.EraReadaptado)
                        {
                            dadosLotacaoDocenteFuncionario.Funcao = dadosLotacaoDocenteFuncionario.Funcao.Replace(" ", "");
                        }
                        else
                        {
                            dadosLotacaoDocenteFuncionario.Funcao = dadosLotacaoDocenteFuncionario.FuncaoAnterior;
                        }

                        //lotação futura que será inserida se for docente
                        if (dadosLotacaoDocenteFuncionario.NumFunc > 0)
                        {
                            //função da categoria
                            futuraFuncao = rnDocente.ObtemFuncaoCategoriaPor(contexto, dadosLotacaoDocenteFuncionario.Matricula);
                            if (string.IsNullOrEmpty(futuraFuncao))
                            {
                                mensagens.Add("Não existe função relacionada ao cargo do docente.");
                            }
                        }

                        if (dadosLotacaoDocenteFuncionario.Readaptado)//Readaptado = S
                        {
                            if (possuiAulasAlocadas)
                            {
                                mensagens.Add("Não é possivel marcar readaptação quando o servidor possui aulas alocadas.");
                            }
                        }
                        else //Readaptado = N
                        {
                            if (dadosLotacaoDocenteFuncionario.FuncaoAnterior != dadosLotacaoDocenteFuncionario.Funcao && possuiAulasAlocadas)
                            {
                                mensagens.Add("Existem aulas alocadas para o servidor, não é possível alterar a função.");
                            }
                        }

                        if (mensagens.Count == 0)
                        {
                            #region Monta Entidades de lotação

                            //Monta lotação que será inserida
                            lotacao = new LyLotacao();
                            lotacao.Ordem = lotacaoBanco.Ordem; //Comeca com ordem atual para validar data corretamenta
                            lotacao.Pessoa = lotacaoBanco.Pessoa;
                            lotacao.Matricula = lotacaoBanco.Matricula;
                            lotacao.Funcao = Convert.ToString(dadosLotacaoDocenteFuncionario.Funcao); //pode ter trocado ou não 
                            lotacao.UnidadeEns = lotacaoBanco.UnidadeEns;
                            lotacao.UnidadeFis = lotacaoBanco.UnidadeFis;
                            lotacao.Setor = lotacaoBanco.Setor;
                            lotacao.Categoria = dadosLotacaoDocenteFuncionario.Categoria;
                            lotacao.Nucleo = lotacaoBanco.Nucleo;
                            lotacao.Usuario = dadosLotacaoDocenteFuncionario.Usuario;
                            lotacao.DataAtualizacao = DateTime.Now;
                            lotacao.DataNomeacaoDo = null;
                            lotacao.DataDesativacaoDo = null;
                            lotacao.AtoOficial = null;
                            lotacao.RespDocumentacao = null;

                            //Verifica se é Readaptacao
                            if (dadosLotacaoDocenteFuncionario.Readaptado)
                            {
                                if (!dadosLotacaoDocenteFuncionario.EraReadaptado)
                                {
                                    //Caso a lotação anteriormente não fosse readaptada desativa a lotação atual e vai inserir outra readaptada                        

                                    //Atualiza data da lotação que será desativada, com data do inicio da Readaptacao
                                    lotacaoAntiga = lotacaoBanco;
                                    lotacaoAntiga.Usuario = dadosLotacaoDocenteFuncionario.Usuario;
                                    lotacaoAntiga.DataAtualizacao = DateTime.Now;
                                    lotacaoAntiga.DataDesativacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioReadaptacao).AddDays(-1);

                                    //Validata data de dispensa da lotacao anterior    
                                    if (Convert.ToDateTime(lotacaoAntiga.DataDesativacao).Date <= Convert.ToDateTime(lotacaoAntiga.DataNomeacao).Date)
                                    {
                                        mensagens.Add("Data da Dispensa da lotação anterior (" + Convert.ToDateTime(lotacaoAntiga.DataDesativacao).Date.ToShortDateString() + ") não pode ser menor ou igual a Data da Nomeação (" + Convert.ToDateTime(lotacaoAntiga.DataNomeacao).Date.ToShortDateString() + ").");
                                    }

                                    //Validação de datas intercaladas padrao
                                    validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacaoAntiga);
                                    if (validacaoCamposLotacao.Count > 0)
                                    {
                                        mensagens.AddRange(validacaoCamposLotacao);
                                    }

                                    dadosLotacaoDocenteFuncionario.LotacaoParaDesativar = lotacaoAntiga;

                                    //Lotacao para ser inserida                                   
                                    lotacao.Readaptado = "S";
                                    lotacao.DataNomeacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioReadaptacao);
                                    lotacao.DtInicioReadaptacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioReadaptacao);
                                    lotacao.DtFimReadaptacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimReadaptacao);
                                    lotacao.DataDesativacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimReadaptacao);

                                    //Validação de datas intercaladas padrao
                                    validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacao);
                                    if (validacaoCamposLotacao.Count > 0)
                                    {
                                        mensagens.AddRange(validacaoCamposLotacao);
                                    }

                                    lotacao.Ordem = proximaOrdem;
                                    dadosLotacaoDocenteFuncionario.Lotacao = lotacao;

                                    //Cria lotação futura que será inserida se for docente                                  
                                    lotacaoFutura = new LyLotacao();
                                    lotacaoFutura.Pessoa = lotacaoBanco.Pessoa;
                                    lotacaoFutura.Matricula = lotacaoBanco.Matricula;
                                    lotacaoFutura.Ordem = lotacaoBanco.Ordem; //Comeca com ordem atual para validar data corretamenta
                                    lotacaoFutura.Funcao = lotacao.Funcao; //Manter funcao da readptação
                                    lotacaoFutura.UnidadeEns = lotacaoBanco.UnidadeEns;
                                    lotacaoFutura.UnidadeFis = lotacaoBanco.UnidadeFis;
                                    lotacaoFutura.Setor = lotacaoBanco.Setor;
                                    lotacaoFutura.Categoria = dadosLotacaoDocenteFuncionario.Categoria;
                                    lotacaoFutura.Nucleo = lotacaoBanco.Nucleo;
                                    lotacaoFutura.Readaptado = "N";
                                    lotacaoFutura.DataNomeacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimReadaptacao).AddDays(1);
                                    lotacaoFutura.DtInicioReadaptacao = null;
                                    lotacaoFutura.DtFimReadaptacao = null;
                                    lotacaoFutura.DataDesativacao = null;
                                    lotacaoFutura.Usuario = dadosLotacaoDocenteFuncionario.Usuario;
                                    lotacaoFutura.DataAtualizacao = DateTime.Now;
                                    lotacaoFutura.DataNomeacaoDo = null;
                                    lotacaoFutura.DataDesativacaoDo = null;
                                    lotacaoFutura.AtoOficial = null;
                                    lotacaoFutura.RespDocumentacao = null;

                                    //Validação de datas intercaladas padrao
                                    validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacaoFutura);
                                    if (validacaoCamposLotacao.Count > 0)
                                    {
                                        mensagens.AddRange(validacaoCamposLotacao);
                                    }

                                    lotacaoFutura.Ordem = proximaOrdem + 1;
                                    dadosLotacaoDocenteFuncionario.LotacaoFutura = lotacaoFutura;

                                }
                                else
                                {
                                    //Caso a opção readaptado seja marcada e o lotação anteriormente era readaptada
                                    //alterar a data desativação apenas da lotação ativa
                                    lotacaoAntiga = lotacaoBanco;
                                    lotacaoAntiga.Usuario = dadosLotacaoDocenteFuncionario.Usuario;
                                    lotacaoAntiga.DataAtualizacao = DateTime.Now;
                                    lotacaoAntiga.DataDesativacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimReadaptacao);
                                    lotacaoAntiga.DtFimReadaptacao = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimReadaptacao);
                                    dadosLotacaoDocenteFuncionario.LotacaoParaDesativar = lotacaoAntiga;
                                }
                            }
                            else
                            {
                                //Se Não possui aula e a função foi trocada, e não mudou o readaptado, desativa a antiga e insere nova lotação!
                                if (dadosLotacaoDocenteFuncionario.FuncaoAnterior != dadosLotacaoDocenteFuncionario.Funcao && !string.IsNullOrEmpty(dadosLotacaoDocenteFuncionario.Funcao))
                                {
                                    if (!dadosLotacaoDocenteFuncionario.PossuiAulasAlocadas)
                                    {
                                        //Desativa lotação e insere uma nova
                                        lotacaoAntiga = lotacaoBanco;
                                        lotacaoAntiga.Usuario = dadosLotacaoDocenteFuncionario.Usuario;
                                        lotacaoAntiga.DataAtualizacao = DateTime.Now;
                                        lotacaoAntiga.DataDesativacao = DateTime.Today.AddDays(-1);

                                        //Validata data de dispensa da lotacao anterior    
                                        if (Convert.ToDateTime(lotacaoAntiga.DataDesativacao).Date <= Convert.ToDateTime(lotacaoAntiga.DataNomeacao).Date)
                                        {
                                            mensagens.Add("Data da Dispensa da lotação anterior (" + Convert.ToDateTime(lotacaoAntiga.DataDesativacao).Date.ToShortDateString() + ") não pode ser menor ou igual a Data da Nomeação (" + Convert.ToDateTime(lotacaoAntiga.DataNomeacao).Date.ToShortDateString() + ").");
                                        }

                                        //Validação de datas intercaladas padrao
                                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacaoAntiga);
                                        if (validacaoCamposLotacao.Count > 0)
                                        {
                                            mensagens.AddRange(validacaoCamposLotacao);
                                        }

                                        dadosLotacaoDocenteFuncionario.LotacaoParaDesativar = lotacaoAntiga;

                                        //Lotacao para ser inserida
                                        lotacao.Ordem = proximaOrdem;
                                        lotacao.DataNomeacao = DateTime.Today;
                                        lotacao.Readaptado = "N";
                                        lotacao.DtInicioReadaptacao = null;
                                        lotacao.DtFimReadaptacao = null;
                                        lotacao.DataDesativacao = null;

                                        //Validação de datas intercaladas padrao
                                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacao);
                                        if (validacaoCamposLotacao.Count > 0)
                                        {
                                            mensagens.AddRange(validacaoCamposLotacao);
                                        }

                                        lotacao.Ordem = proximaOrdem;
                                        dadosLotacaoDocenteFuncionario.Lotacao = lotacao;
                                    }
                                }
                            }

                            #endregion

                            //verifica se houve inclusão de licenca
                            if (!dadosLotacaoDocenteFuncionario.Situacao.IsNullOrEmptyOrWhiteSpace())
                            {
                                string motivo = dadosLotacaoDocenteFuncionario.Situacao;
                                string padaces = rnPadroesDeAcessos.ObtemPadraoAcessoLicencaPor(dadosLotacaoDocenteFuncionario.Usuario, motivo);

                                //Insere nova licença  ou atualiza data fim da licença                            

                                //Verificar se é servidor
                                if (dadosLotacaoDocenteFuncionario.NumFunc == 0)//monta ly_licenca_pessoa
                                {
                                    //busca a licenca como está hoje na base
                                    LyLicencaPessoa licencaBanco = new LyLicencaPessoa();
                                    licencaBanco = rnLicencaPessoa.ObtemLicencaAtivaPor(dadosLotacaoDocenteFuncionario.Matricula);

                                    dadosLotacaoDocenteFuncionario.MotivoAntigo = licencaBanco.Motivo;
                                    dadosLotacaoDocenteFuncionario.SituacaoAntigaDataIni = licencaBanco.Dtini;

                                    if (motivo == "46" && rnPadroesDeAcessos.EhPadraoAcessoBloqueadoLancamentoPericiaPor(contexto, dadosLotacaoDocenteFuncionario.Usuario))
                                    {
                                        if (licencaBanco.Motivo.IsNullOrEmptyOrWhiteSpace())
                                        {
                                            if (rnLicencaPessoa.PossuiLicencaAnteriorComAltaPor(contexto, lotacaoBanco.Pessoa))
                                            {
                                                mensagens.Add("Lançamento de AGUARDANDO´PERÍCIA não permitido para os casos de licença anterior não ser COM ALTA.");
                                            }
                                        }
                                    }

                                    if (!licencaBanco.Motivo.IsNullOrEmptyOrWhiteSpace() && licencaBanco.Dtini != DateTime.MinValue)
                                    {
                                        //já existia licença ativa
                                        if (dadosLotacaoDocenteFuncionario.DataFimSituacao != null && dadosLotacaoDocenteFuncionario.DataFimSituacao != DateTime.MinValue)//verifica se houve alteração na data fim
                                        {
                                            DateTime dataini = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioSituacao);//continua a mesma
                                            DateTime datafim = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimSituacao);

                                            licencaPessoa = new LyLicencaPessoa();
                                            licencaPessoa.Pessoa = lotacaoBanco.Pessoa;
                                            licencaPessoa.Ordem = Convert.ToDecimal(licencaBanco.Ordem);
                                            licencaPessoa.Motivo = motivo;
                                            licencaPessoa.Dtini = dataini;
                                            licencaPessoa.Dtfim = datafim;

                                            if (motivo == licencaBanco.Motivo)
                                            {
                                                bool possuiDataFim = false;
                                                //Realiza validações de situação (licença)
                                                validacaoLicenca = rnLicencaPessoa.ValidaAlteracao(licencaPessoa, lotacao.Matricula, out possuiDataFim, dadosLotacaoDocenteFuncionario.Usuario);
                                                if (!validacaoLicenca.Valido)
                                                {
                                                    mensagens.Add(validacaoLicenca.Mensagem);
                                                }

                                                dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim = possuiDataFim;
                                            }
                                            else
                                            {
                                                //Realiza validações de situação (licença)
                                                bool possuiDataFim = false;
                                                validacaoLicenca = rnLicencaPessoa.ValidaInsercao(licencaPessoa, lotacao.Matricula, out possuiDataFim, dadosLotacaoDocenteFuncionario.Usuario);
                                                if (!validacaoLicenca.Valido)
                                                {
                                                    mensagens.Add(validacaoLicenca.Mensagem);
                                                }

                                                dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim = possuiDataFim;
                                            }

                                            if (!dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim)
                                            {
                                                if (licencaDocente.Dtini < lotacaoBanco.DataNomeacao)
                                                {
                                                    mensagens.Add("A data início da licença é menor que a data de nomeação");
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        //não existia licença ativa, apenas insere uma nova com os dados

                                        DateTime dataini = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioSituacao);
                                        DateTime datafim = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimSituacao);

                                        licencaPessoa = new LyLicencaPessoa();
                                        licencaPessoa.Pessoa = lotacaoBanco.Pessoa;
                                        licencaPessoa.Motivo = motivo;
                                        licencaPessoa.Dtini = dataini;
                                        licencaPessoa.Dtfim = datafim;

                                        //Realiza validações de situação (licença)
                                        bool possuiDataFim = false;
                                        validacaoLicenca = rnLicencaPessoa.ValidaInsercao(licencaPessoa, lotacao.Matricula, out possuiDataFim, dadosLotacaoDocenteFuncionario.Usuario);
                                        if (!validacaoLicenca.Valido)
                                        {
                                            mensagens.Add(validacaoLicenca.Mensagem);
                                        }

                                        dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim = possuiDataFim;

                                        if (!possuiDataFim)
                                        {
                                            if (licencaPessoa.Dtini < lotacaoBanco.DataNomeacao)
                                            {
                                                mensagens.Add("A data início da licença é menor que a data de nomeação");
                                            }
                                        }
                                    }
                                }
                                else //monta ly_licenca_docente
                                {
                                    //busca a licenca como está hoje na base
                                    LyLicencaDocente licencaBanco = new LyLicencaDocente();
                                    licencaBanco = rnLicencaDocente.ObtemLicencaAtivaPor(dadosLotacaoDocenteFuncionario.NumFunc);

                                    dadosLotacaoDocenteFuncionario.MotivoAntigo = licencaBanco.Motivo;
                                    dadosLotacaoDocenteFuncionario.SituacaoAntigaDataIni = licencaBanco.Dtini;

                                    if (motivo == "46" && rnPadroesDeAcessos.EhPadraoAcessoBloqueadoLancamentoPericiaPor(contexto, dadosLotacaoDocenteFuncionario.Usuario))
                                    {
                                        if (licencaBanco.Motivo.IsNullOrEmptyOrWhiteSpace())
                                        {
                                            if (rnLicencaDocente.PossuiLicencaAnteriorComAltaPor(contexto, dadosLotacaoDocenteFuncionario.NumFunc))
                                            {
                                                mensagens.Add("Lançamento de AGUARDANDO´PERÍCIA não permitido para os casos de licença anterior não ser COM ALTA.");
                                            }
                                        }
                                    }

                                    if (!licencaBanco.Motivo.IsNullOrEmptyOrWhiteSpace() && licencaBanco.Dtini != DateTime.MinValue)
                                    {
                                        //já existia licença ativa

                                        if (motivo == licencaBanco.Motivo)
                                        {
                                            if (dadosLotacaoDocenteFuncionario.DataFimSituacao != null && dadosLotacaoDocenteFuncionario.DataFimSituacao != DateTime.MinValue)//verifica se houve alteração na data fim
                                            {
                                                DateTime dataini = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioSituacao);//continua a mesma
                                                DateTime datafim = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimSituacao);

                                                licencaDocente = new LyLicencaDocente();
                                                licencaDocente.NumFunc = dadosLotacaoDocenteFuncionario.NumFunc;
                                                licencaDocente.Motivo = motivo;
                                                licencaDocente.Dtini = dataini;
                                                licencaDocente.Dtfim = datafim;

                                                bool possuiDataFim = false;
                                                //Realiza validações de situação (licença)
                                                validacaoLicenca = rnLicencaDocente.ValidaAlteracao(licencaDocente, lotacao.Matricula, lotacao.Pessoa, out possuiDataFim, dadosLotacaoDocenteFuncionario.Usuario);
                                                if (!validacaoLicenca.Valido)
                                                {
                                                    mensagens.Add(validacaoLicenca.Mensagem);
                                                }

                                                dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim = possuiDataFim;

                                                if (!possuiDataFim)
                                                {
                                                    if (licencaDocente.Dtini < lotacaoBanco.DataNomeacao)
                                                    {
                                                        mensagens.Add("A data início da licença é menor que a data de nomeação");
                                                    }
                                                }
                                            }
                                        }
                                        else //nova licença deve ser inserida
                                        {
                                            DateTime dataini = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioSituacao);
                                            DateTime datafim = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataFimSituacao);

                                            licencaDocente = new LyLicencaDocente();
                                            licencaDocente.NumFunc = dadosLotacaoDocenteFuncionario.NumFunc;
                                            licencaDocente.Motivo = motivo;
                                            licencaDocente.Dtini = dataini;
                                            licencaDocente.Dtfim = datafim;

                                            bool possuiDataFim = false;
                                            //Realiza validações de situação (licença)
                                            validacaoLicenca = rnLicencaDocente.ValidaInsercao(licencaDocente, lotacao.Matricula, lotacao.Pessoa, out possuiDataFim, dadosLotacaoDocenteFuncionario.Usuario);
                                            if (!validacaoLicenca.Valido)
                                            {
                                                mensagens.Add(validacaoLicenca.Mensagem);
                                            }

                                            dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim = possuiDataFim;

                                            if (!possuiDataFim)
                                            {
                                                if (licencaDocente.Dtini < lotacaoBanco.DataNomeacao)
                                                {
                                                    mensagens.Add("A data início da licença é menor que a data de nomeação");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //não existia licença ativa, apenas insere uma nova com os dados
                                        if (!dadosLotacaoDocenteFuncionario.Situacao.IsNullOrEmptyOrWhiteSpace())//verifica se houve inclusão de motivo e dataini
                                        {
                                            DateTime dataini = Convert.ToDateTime(dadosLotacaoDocenteFuncionario.DataInicioSituacao);
                                            DateTime? datafim = dadosLotacaoDocenteFuncionario.DataFimSituacao;

                                            licencaDocente = new LyLicencaDocente();
                                            licencaDocente.NumFunc = dadosLotacaoDocenteFuncionario.NumFunc;
                                            licencaDocente.Motivo = motivo;
                                            licencaDocente.Dtini = dataini;
                                            licencaDocente.Dtfim = datafim;

                                            bool possuiDataFim = false;

                                            //Realiza validações de situação (licença)
                                            validacaoLicenca = rnLicencaDocente.ValidaInsercao(licencaDocente, lotacao.Matricula, lotacao.Pessoa, out possuiDataFim, dadosLotacaoDocenteFuncionario.Usuario);

                                            if (!validacaoLicenca.Valido)
                                            {
                                                mensagens.Add(validacaoLicenca.Mensagem);
                                            }

                                            dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim = possuiDataFim;

                                            if (!possuiDataFim)
                                            {
                                                if (licencaDocente.Dtini < lotacaoBanco.DataNomeacao)
                                                {
                                                    mensagens.Add("A data início da licença é menor que a data de nomeação");
                                                }
                                            }
                                        }
                                    }
                                }

                                dadosLotacaoDocenteFuncionario.LicencaPessoa = licencaPessoa;
                                dadosLotacaoDocenteFuncionario.LicencaDocente = licencaDocente;
                            }

                            //Verifica se a situação é carga horaria reduzida
                            if (dadosLotacaoDocenteFuncionario.Reducaoch == "S" && dadosLotacaoDocenteFuncionario.NumFunc > 0)
                            {
                                //busca aulas Alocadas para o Docente
                                int totalAulasAlocadas = rnAulaDocente.ObtemTotalAulasAlocadasPor(contexto, dadosLotacaoDocenteFuncionario.NumFunc, DateTime.Now);

                                //Busca categoria
                                string categoria = rnDocente.ObtemCategoriaPor(contexto, dadosLotacaoDocenteFuncionario.Matricula);

                                //Atualiza carga Horaria do Docente
                                int cargaHorariaPermitidaFuncao = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(contexto, categoria, dadosLotacaoDocenteFuncionario.Funcao);

                                //Se a quantidade de tempos alocados para o docente exceder a carga horária total configurada para
                                //a nova função do docente, não permitir o salvamento.
                                if (totalAulasAlocadas > (cargaHorariaPermitidaFuncao / 2))
                                {
                                    mensagens.Add("Não é possível colocar carga horária reduzida, devido o professor ter mais tempos alocado(" + totalAulasAlocadas + ")  do que o permitido(" + (cargaHorariaPermitidaFuncao / 2) + ").");
                                }
                            }
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

        public void AlteraLotacaoSituacaoServidor(DTOs.DadosLotacaoDocenteFuncionario dadosLotacaoDocenteFuncionario)
        {
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
            RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
            RN.VinculoLy rnVinculoLy = new VinculoLy();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            List<string> mensagens = new List<string>();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();
            bool possuiAulasGlp;

            try
            {
                //Verifica se é Readaptacao
                if (dadosLotacaoDocenteFuncionario.Readaptado)
                {
                    if (!dadosLotacaoDocenteFuncionario.EraReadaptado)
                    {
                        //Caso a lotação anteriormente não fosse readaptada desativa a lotação atual e vai inserir outra readaptada                        

                        //Atualiza data da lotação que será desativada, com data do inicio da Readaptacao
                        this.DesativaLotacao(contexto, Convert.ToDateTime(dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.DataDesativacao), dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Usuario, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Matricula);

                        //Lotacao para ser inserida
                        this.Insere(contexto, dadosLotacaoDocenteFuncionario.Lotacao);

                        if (dadosLotacaoDocenteFuncionario.LotacaoFutura != null)
                        {
                            this.Insere(contexto, dadosLotacaoDocenteFuncionario.LotacaoFutura);
                        }
                    }
                    else
                    {
                        //Caso a opção readaptado seja marcada e o lotação anteriormente era readaptada altera a data desativação apenas da lotação ativa

                        this.AlteraDataFimReaptacaoDesativacao(contexto, Convert.ToDateTime(dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.DataDesativacao), Convert.ToDateTime(dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.DtFimReadaptacao), dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Usuario, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Pessoa, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Matricula);

                        //verifica se existe lotação com ordem + 1 e altera a data de inicio
                        if (this.PossuiLotacaoFuturaPor(contexto, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Pessoa, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Matricula, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Ordem + 1))
                        {
                            this.AlteraDataNomeacao(contexto, Convert.ToDateTime(dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.DtFimReadaptacao).AddDays(1), dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Usuario, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Matricula, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Pessoa, dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.Ordem + 1);
                        }
                    }
                }
                else
                {
                    //Se Não possui aula e a função foi trocada, e não mudou o readaptado, desativa a antiga e insere nova lotação!
                    if (dadosLotacaoDocenteFuncionario.FuncaoAnterior != dadosLotacaoDocenteFuncionario.Funcao && !string.IsNullOrEmpty(dadosLotacaoDocenteFuncionario.Funcao))
                    {
                        if (!dadosLotacaoDocenteFuncionario.PossuiAulasAlocadas)
                        {
                            //Desativa lotação e insere uma nova
                            //dataDesativacaoLotacaoAnterior = DateTime.Today;
                            this.DesativaLotacao(contexto, Convert.ToDateTime(dadosLotacaoDocenteFuncionario.LotacaoParaDesativar.DataDesativacao), dadosLotacaoDocenteFuncionario.Usuario, dadosLotacaoDocenteFuncionario.Matricula);

                            //Lotacao para ser inserida
                            this.Insere(contexto, dadosLotacaoDocenteFuncionario.Lotacao);
                        }
                    }
                }

                //verifica se houve inclusão de licenca
                if (!dadosLotacaoDocenteFuncionario.Situacao.IsNullOrEmptyOrWhiteSpace())
                {
                    //Insere nova licença  ou atualiza data fim da licença

                    //Verifica se não é docente
                    if (dadosLotacaoDocenteFuncionario.NumFunc == 0)
                    {
                        if (!string.IsNullOrEmpty(dadosLotacaoDocenteFuncionario.MotivoAntigo) && dadosLotacaoDocenteFuncionario.SituacaoAntigaDataIni != DateTime.MinValue)
                        {
                            //já existia licença ativa
                            if (dadosLotacaoDocenteFuncionario.DataFimSituacao != null && dadosLotacaoDocenteFuncionario.DataFimSituacao != DateTime.MinValue)//verifica se houve alteração na data fim
                            {
                                if (dadosLotacaoDocenteFuncionario.LicencaPessoa.Motivo == dadosLotacaoDocenteFuncionario.MotivoAntigo)
                                {
                                    //Apenas altera data fim
                                    rnLicencaPessoa.AlteraDataFim(contexto, dadosLotacaoDocenteFuncionario.LicencaPessoa);
                                }
                                else
                                {
                                    rnLicencaPessoa.Insere(contexto, dadosLotacaoDocenteFuncionario.LicencaPessoa);

                                    if (!dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim)
                                    {
                                        //Para licenças definitivas desativa a lotação atualizada
                                        this.DesativaLotacaoLicencaDefinitiva(contexto, dadosLotacaoDocenteFuncionario.LicencaPessoa.Dtini, dadosLotacaoDocenteFuncionario.Usuario, dadosLotacaoDocenteFuncionario.Matricula);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //não existia licença ativa, apenas insere uma nova com os dados
                            rnLicencaPessoa.Insere(contexto, dadosLotacaoDocenteFuncionario.LicencaPessoa);

                            if (!dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim)
                            {
                                //Para licenças definitivas desativa a lotação atualizada
                                this.DesativaLotacaoLicencaDefinitiva(contexto, dadosLotacaoDocenteFuncionario.LicencaPessoa.Dtini, dadosLotacaoDocenteFuncionario.Usuario, dadosLotacaoDocenteFuncionario.Matricula);
                            }
                        }
                    }
                    else //Era docente
                    {
                        if (!string.IsNullOrEmpty(dadosLotacaoDocenteFuncionario.MotivoAntigo) && dadosLotacaoDocenteFuncionario.SituacaoAntigaDataIni != DateTime.MinValue)
                        {
                            //já existia licença ativa

                            if (dadosLotacaoDocenteFuncionario.LicencaDocente.Motivo == dadosLotacaoDocenteFuncionario.MotivoAntigo)
                            {
                                if (dadosLotacaoDocenteFuncionario.DataFimSituacao != null && dadosLotacaoDocenteFuncionario.DataFimSituacao != DateTime.MinValue)//verifica se houve alteração na data fim
                                {
                                    rnLicencaDocente.AlteraDataFim(contexto, dadosLotacaoDocenteFuncionario.LicencaDocente);
                                }
                            }
                            else //nova licença deve ser inserida
                            {
                                rnLicencaDocente.Insere(contexto, dadosLotacaoDocenteFuncionario.LicencaDocente);

                                if (!dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim)
                                {
                                    //Para licenças definitivas desativa a lotação atualizada
                                    this.DesativaLotacaoLicencaDefinitiva(contexto, dadosLotacaoDocenteFuncionario.LicencaDocente.Dtini, dadosLotacaoDocenteFuncionario.Usuario, dadosLotacaoDocenteFuncionario.Matricula);
                                }
                            }
                        }
                        else
                        {
                            //não existia licença ativa, apenas insere uma nova com os dados
                            rnLicencaDocente.Insere(contexto, dadosLotacaoDocenteFuncionario.LicencaDocente);

                            if (!dadosLotacaoDocenteFuncionario.LicencaPossuiDataFim)
                            {
                                //Para licenças definitivas desativa a lotação atualizada
                                this.DesativaLotacaoLicencaDefinitiva(contexto, dadosLotacaoDocenteFuncionario.LicencaDocente.Dtini, dadosLotacaoDocenteFuncionario.Usuario, dadosLotacaoDocenteFuncionario.Matricula);
                            }
                        }
                    }
                }

                //Define tipo da licença antes de começar
                bool ehDocente = dadosLotacaoDocenteFuncionario.NumFunc != 0;

                if (dadosLotacaoDocenteFuncionario.Reducaoch == "S")
                {
                    DateTime dataInicio = dadosLotacaoDocenteFuncionario.dtinich ?? DateTime.Now;
                          
                    if (!ehDocente)
                    {
                        var licencaReducao = new Entidades.LyLicencaPessoa
                        {
                            Pessoa = Convert.ToDecimal(dadosLotacaoDocenteFuncionario.Pessoa),
                            Ordem = rnVinculoLy.ObtemOrdemVinculoAtivoPor(contexto,dadosLotacaoDocenteFuncionario.Matricula),
                            Motivo = "43",
                            Dtini = dataInicio,
                            Dtfim = dadosLotacaoDocenteFuncionario.dtfimch,
                            DtRetorno = null
                        };

                        //Verifica se tem redução CH
                        bool jaExisteAtiva = rnLicencaPessoa.PossuiLicencaAtivaMotivo43(contexto, Convert.ToDecimal(dadosLotacaoDocenteFuncionario.Pessoa));
                        bool jaExisteInicio = rnLicencaPessoa.PossuiLicencaMotivo43(contexto, Convert.ToDecimal(dadosLotacaoDocenteFuncionario.Pessoa), dataInicio);

                        if (!jaExisteInicio)                       
                        {
                            //////TODO:  Voltar caso posso mudar data inicio
                            ////Caso exista ativa com outro inicio apaga
                            //if (jaExisteAtiva)
                            //{
                            //    //Remove
                            //    rnLicencaPessoa.RemoveReducaoCHAtiva(contexto, Convert.ToDecimal(dadosLotacaoDocenteFuncionario.Pessoa));
                            //}

                            rnLicencaPessoa.Insere(contexto, licencaReducao);
                        }
                        else
                        {
                            rnLicencaPessoa.AlteraReducaoCH(contexto, licencaReducao);                        
                        }
                    }
                    else
                    {
                        var licencaReducao = new Entidades.LyLicencaDocente
                        {
                            NumFunc = Convert.ToDecimal(dadosLotacaoDocenteFuncionario.NumFunc),
                            Motivo = "43",
                            Dtini = dataInicio,
                            Dtfim = dadosLotacaoDocenteFuncionario.dtfimch,
                            DtRetorno = null
                        };

                        //Verifica se tem redução CH
                        bool jaExisteAtiva = rnLicencaDocente.PossuiLicencaAtivaMotivo43(contexto, Convert.ToDecimal(dadosLotacaoDocenteFuncionario.NumFunc)); 
                        bool jaExisteInicio = rnLicencaDocente.PossuiLicencaMotivo43(contexto, Convert.ToDecimal(dadosLotacaoDocenteFuncionario.NumFunc), dataInicio);

                        if (!jaExisteInicio)
                        {
                            ////TODO:  Voltar caso posso mudar data inicio
                            ////Caso exista ativa com outro inicio apaga
                            //if (jaExisteAtiva)
                            //{
                            //    //Remove
                            //    rnLicencaDocente.RemoveReducaoCHAtiva(contexto, Convert.ToDecimal(dadosLotacaoDocenteFuncionario.NumFunc));
                            //}

                            //Verifica se possui glp, se possui não pode reduzir carga horária
                            possuiAulasGlp = rnAulaDocenteTipo.PossuiGLP(contexto, licencaReducao.NumFunc);
                            if (possuiAulasGlp && licencaReducao.Motivo == "43")
                            {
                                throw new Exception("Não é possível reduzir a carga horária, pois o docente possui aulas GLP vinculadas.");
                            }
                            else
                            {
                                rnLicencaDocente.Insere(contexto, licencaReducao);
                            }                            
                        }
                        else
                        { 
                            rnLicencaDocente.AlteraReducaoCH(contexto, licencaReducao);
                        }
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

        public void Insere(DataContext ctx, LyLotacao lotacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO DBO.LY_LOTACAO 
                                (PESSOA, 
                                 MATRICULA, 
                                 ORDEM, 
                                 FUNCAO, 
                                 UNIDADE_FIS, 
                                 DATA_NOMEACAO, 
                                 UNIDADE_ENS, 
                                 NUCLEO, 
                                 SETOR, 
                                 READAPTADO, 
                                 DT_INICIO_READAPTACAO,
                                 DT_FIM_READAPTACAO,
                                 USUARIO, 
                                 DATA_ATUALIZACAO,
                                 DATA_NOMEACAO_DO,
                                 DATA_DESATIVACAO_DO,
                                 DATA_DESATIVACAO,
                                 ATO_OFICIAL,
                                 RESP_DOCUMENTACAO,
                                 CATEGORIA) 
                    VALUES      (@PESSOA, 
                                 @MATRICULA, 
                                 @ORDEM, 
                                 @FUNCAO, 
                                 @UNIDADE_FIS, 
                                 @DATA_NOMEACAO, 
                                 @UNIDADE_ENS, 
                                 @NUCLEO, 
                                 @SETOR, 
                                 @READAPTADO, 
                                 @DT_INICIO_READAPTACAO,
                                 @DT_FIM_READAPTACAO, 
                                 @USUARIO, 
                                 @DATA_ATUALIZACAO,
                                 @DATA_NOMEACAO_DO,
                                 @DATADESATIVACAODO,
                                 @DATADESATIVACAO,
                                 @ATOOFICIAL,
                                 @RESPDOCUMENTACAO,
                                 @CATEGORIA) ";

                contextQuery.Parameters.Add("@PESSOA", lotacao.Pessoa);
                contextQuery.Parameters.Add("@MATRICULA", lotacao.Matricula);
                contextQuery.Parameters.Add("@ORDEM", lotacao.Ordem);
                contextQuery.Parameters.Add("@FUNCAO", lotacao.Funcao);
                contextQuery.Parameters.Add("@UNIDADE_FIS", lotacao.UnidadeFis);
                contextQuery.Parameters.Add("@DATA_NOMEACAO", lotacao.DataNomeacao);
                contextQuery.Parameters.Add("@UNIDADE_ENS", lotacao.UnidadeEns);
                contextQuery.Parameters.Add("@NUCLEO", lotacao.Nucleo);
                contextQuery.Parameters.Add("@SETOR", lotacao.Setor);
                contextQuery.Parameters.Add("@CATEGORIA", lotacao.Categoria);
                contextQuery.Parameters.Add("@READAPTADO", lotacao.Readaptado);
                contextQuery.Parameters.Add("@USUARIO", lotacao.Usuario);
                contextQuery.Parameters.Add("@DATA_ATUALIZACAO", DateTime.Now);

                if (lotacao.DataNomeacaoDo != null && lotacao.DataNomeacaoDo != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATA_NOMEACAO_DO", lotacao.DataNomeacaoDo);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATA_NOMEACAO_DO", null);
                }

                if (lotacao.DataDesativacaoDo != null && lotacao.DataDesativacaoDo != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATADESATIVACAODO", lotacao.DataDesativacaoDo);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATADESATIVACAODO", null);
                }

                if (lotacao.DataDesativacao != null && lotacao.DataDesativacao != DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATADESATIVACAO", lotacao.DataDesativacao);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATADESATIVACAO", null);
                }

                if (lotacao.Readaptado == "S")
                {
                    contextQuery.Parameters.Add("@DT_INICIO_READAPTACAO", lotacao.DataNomeacao);
                    contextQuery.Parameters.Add("@DT_FIM_READAPTACAO", lotacao.DataDesativacao);
                }
                else
                {
                    contextQuery.Parameters.Add("@DT_INICIO_READAPTACAO", null);
                    contextQuery.Parameters.Add("@DT_FIM_READAPTACAO", null);
                }

                contextQuery.Parameters.Add("@ATOOFICIAL", lotacao.AtoOficial);
                contextQuery.Parameters.Add("@RESPDOCUMENTACAO", lotacao.RespDocumentacao);

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

        private void Altera(DataContext ctx, LyLotacao lotacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_LOTACAO 
                                        SET    DATA_NOMEACAO = @DATA_NOMEACAO, 
                                               DATA_NOMEACAO_DO = @DATA_NOMEACAO_DO, 
                                               DATA_DESATIVACAO = @DATADESATIVACAO, 
                                               DATA_DESATIVACAO_DO = @DATADESATIVACAODO, 
                                               UNIDADE_ENS = @UNIDADE_ENS, 
                                               UNIDADE_FIS = @UNIDADE_FIS, 
                                               SETOR = @SETOR, 
                                               NUCLEO = @NUCLEO, 
                                               ATO_OFICIAL = @ATOOFICIAL, 
                                               RESP_DOCUMENTACAO = @RESPDOCUMENTACAO, 
                                               USUARIO = @USUARIO, 
                                               DATA_ATUALIZACAO = @DATA_ATUALIZACAO,
                                               DT_INICIO_READAPTACAO = @DT_INICIO_READAPTACAO,
                                               DT_FIM_READAPTACAO = @DT_FIM_READAPTACAO,
                                               FUNCAO = @FUNCAO
                                        WHERE  PESSOA = @PESSOA  
                                               AND MATRICULA = @MATRICULA 
                                               AND ORDEM = @ORDEM";

                contextQuery.Parameters.Add("@PESSOA", lotacao.Pessoa);
                contextQuery.Parameters.Add("@MATRICULA", lotacao.Matricula);
                contextQuery.Parameters.Add("@ORDEM", lotacao.Ordem);
                contextQuery.Parameters.Add("@DATA_NOMEACAO", lotacao.DataNomeacao);
                contextQuery.Parameters.Add("@DATA_NOMEACAO_DO", lotacao.DataNomeacaoDo);
                contextQuery.Parameters.Add("@DATADESATIVACAO", lotacao.DataDesativacao);
                contextQuery.Parameters.Add("@DATADESATIVACAODO", lotacao.DataDesativacaoDo);
                contextQuery.Parameters.Add("@UNIDADE_ENS", lotacao.UnidadeEns);
                contextQuery.Parameters.Add("@UNIDADE_FIS", lotacao.UnidadeFis);
                contextQuery.Parameters.Add("@NUCLEO", lotacao.Nucleo);
                contextQuery.Parameters.Add("@SETOR", lotacao.Setor);
                contextQuery.Parameters.Add("@FUNCAO", lotacao.Funcao);
                contextQuery.Parameters.Add("@ATOOFICIAL", lotacao.AtoOficial);
                contextQuery.Parameters.Add("@RESPDOCUMENTACAO", lotacao.RespDocumentacao);
                contextQuery.Parameters.Add("@USUARIO", lotacao.Usuario);
                contextQuery.Parameters.Add("@DATA_ATUALIZACAO", DateTime.Now);

                if (lotacao.Readaptado == "S")
                {
                    contextQuery.Parameters.Add("@DT_INICIO_READAPTACAO", lotacao.DataNomeacao);
                    contextQuery.Parameters.Add("@DT_FIM_READAPTACAO", lotacao.DataDesativacao);
                }
                else
                {
                    contextQuery.Parameters.Add("@DT_INICIO_READAPTACAO", null);
                    contextQuery.Parameters.Add("@DT_FIM_READAPTACAO", null);
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

        public DataTable ObtemListaLotacaoDocentePor(int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  D.PESSOA ,
                            D.NUM_FUNC ,
                            L.MATRICULA ,
							CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D.VINCULO) AS IDVINCULO,
                            F.DESCRICAO ,
                            L.SETOR ,
                            R.REGIONAL ,
                            UE.NOME_COMP ,
                            L.DATA_NOMEACAO ,
                            L.DATA_DESATIVACAO ,
                            L.USUARIO ,
                            L.DATA_ATUALIZACAO,
                            L.ORDEM
                    FROM    LY_LOTACAO L ( NOLOCK )
                            INNER JOIN LY_DOCENTE D ( NOLOCK ) ON L.MATRICULA = D.MATRICULA
							INNER JOIN LY_PESSOA P ( NOLOCK ) ON D.PESSOA = P.PESSOA
                            INNER JOIN LY_FUNCAO F ( NOLOCK ) ON L.FUNCAO = F.FUNCAO
                            LEFT JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) ON L.UNIDADE_ENS = UE.UNIDADE_ENS
                            LEFT JOIN TCE_REGIONAL R ( NOLOCK ) ON R.ID_REGIONAL = UE.ID_REGIONAL
                    WHERE   L.PESSOA = @PESSOA
                            AND ( ( L.DATA_DESATIVACAO IS NOT NULL
                                    AND L.DATA_DESATIVACAO < GETDATE()
                                  )
                                  OR ( L.DATA_DESATIVACAO IS NULL )
                                )
                    ORDER BY L.MATRICULA ,
                            L.DATA_NOMEACAO DESC ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable ObtemListaLotacaoVinculoPor(int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  V.PESSOA ,
                            L.MATRICULA ,
							CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,V.VINCULO) AS IDVINCULO,
                            F.DESCRICAO ,
                            L.SETOR ,
                            R.REGIONAL ,
                            UE.NOME_COMP ,
                            L.DATA_NOMEACAO ,
                            L.DATA_DESATIVACAO ,
                            L.USUARIO ,
                            L.DATA_ATUALIZACAO,
                            L.ORDEM
                    FROM    LY_LOTACAO L ( NOLOCK )
                            INNER JOIN LY_VINCULO V ( NOLOCK ) ON L.MATRICULA = V.MATRICULA
							INNER JOIN LY_PESSOA P ( NOLOCK ) ON V.PESSOA = P.PESSOA
                            INNER JOIN LY_FUNCAO F ( NOLOCK ) ON L.FUNCAO = F.FUNCAO
                            LEFT JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) ON L.UNIDADE_ENS = UE.UNIDADE_ENS
                            LEFT JOIN TCE_REGIONAL R ( NOLOCK ) ON R.ID_REGIONAL = UE.ID_REGIONAL
                    WHERE   L.PESSOA = @PESSOA
                            AND ( ( L.DATA_DESATIVACAO IS NOT NULL
                                    AND L.DATA_DESATIVACAO < GETDATE()
                                  )
                                  OR ( L.DATA_DESATIVACAO IS NULL )
                                )
                    ORDER BY L.MATRICULA ,
                            L.DATA_NOMEACAO DESC ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable ObtemListaLotacaoDocenteAtivaPor(int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lotacoes = null;

            try
            {
                contextQuery.Command = @" SELECT  D.NUM_FUNC ,
                                                L.MATRICULA ,
                                                F2.TIPOFUNCAOID ,
                                                CASE 
                                                WHEN F2.TIPOFUNCAOID = 1 THEN 'AAGE'
                                                WHEN F2.TIPOFUNCAOID = 2 THEN 'MEDIADOR DE TECNOLOGIA'
                                                WHEN F2.TIPOFUNCAOID = 3 THEN 'MEDIADOR ARTICULADOR'
                                                WHEN F2.TIPOFUNCAOID = 4 THEN 'OUTROS'
                                                END TIPOFUNCAO, 
                                                F.DESCRICAO ,
                                                --L.SETOR ,
                                                SE.UA_ANTIGA AS SETOR ,
                                                R.REGIONAL ,
                                                UE.NOME_COMP,
                                                SE.ua_atual,
                                                (CONVERT(VARCHAR,PE.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D.VINCULO)) IDVINCULO
                                        FROM    LY_LOTACAO L ( NOLOCK )
                                                INNER JOIN LY_DOCENTE D ( NOLOCK ) ON L.MATRICULA = D.MATRICULA
                                                INNER JOIN LY_PESSOA PE ON PE.PESSOA = D.PESSOA
                                                INNER JOIN LY_FUNCAO F ( NOLOCK ) ON L.FUNCAO = F.FUNCAO
                                                --INNER JOIN HADES..HD_SETOR SE ON SE.SETOR =  L.SETOR
                                                INNER JOIN HADES..VW_SETOR SE ON SE.SETOR = L.SETOR
                                                LEFT JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) ON L.UNIDADE_ENS = UE.UNIDADE_ENS
                                                LEFT JOIN TCE_REGIONAL R ( NOLOCK ) ON R.ID_REGIONAL = UE.ID_REGIONAL
		                                        LEFT JOIN FUNCAO F2 ( NOLOCK ) ON F.FUNCAO = F2.FUNCAOID
                                        WHERE   L.PESSOA = @PESSOA
                                                AND ( L.DATA_DESATIVACAO IS NULL
                                                      OR L.DATA_DESATIVACAO >= GETDATE()
                                                    )
                                        ORDER BY L.MATRICULA ,
                                                L.DATA_NOMEACAO DESC  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                lotacoes = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return lotacoes;
        }

        public List<int> ListaOrdemPor(string matricula)
        {
            List<int> ordens = new List<int>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  ORDEM
                        FROM    LY_LOTACAO (NOLOCK)
                        WHERE   MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ordens.Add(Convert.ToInt32(reader["ORDEM"]));
                }

                return ordens;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public int ObtemProximaOrdemPor(string matricula)
        {
            //Gera código de ordem (chave) a partir do último do banco 
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            int retorno = 0;

            try
            {
                retorno = this.ObtemProximaOrdemPor(ctx, matricula);
                return retorno;
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

        public int ObtemProximaOrdemPor(DataContext ctx, string matricula)
        {
            //Gera código de ordem (chave) a partir do último do banco 
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT  MAX(ORDEM) AS ORDEM
                                            FROM    LY_LOTACAO (NOLOCK)
                                            WHERE   MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["ORDEM"] != DBNull.Value ? Convert.ToInt32(reader["ORDEM"]) + 1 : 1;
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void AlteraMatriculaLotacao(DataContext ctx, Entidades.LogAtualizacaoMatricula logAtualizacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            List<int> ordens = new List<int>();
            try
            {
                ordens = ListaOrdemPor(logAtualizacaoMatricula.MatriculaAnterior);

                foreach (var ordem in ordens)
                {
                    contextQuery.Parameters.Clear();

                    contextQuery.Command = @" UPDATE  LY_LOTACAO
                                SET     MATRICULA = @MATRICULANOVA
                                WHERE   MATRICULA = @MATRICULAANTERIOR
                                        AND ORDEM = @ORDEM ";

                    contextQuery.Parameters.Add("@ORDEM", ordem);
                    contextQuery.Parameters.Add("@MATRICULAANTERIOR", logAtualizacaoMatricula.MatriculaAnterior);
                    contextQuery.Parameters.Add("@MATRICULANOVA", logAtualizacaoMatricula.MatriculaNova);

                    ctx.ApplyModifications(contextQuery);
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
        }

        public void AlteraMatriculaServidorLotacao(DataContext ctx, Entidades.LogAtualizacaoMatriculaServidor logAtualizacaoMatriculaServidor)
        {
            ContextQuery contextQuery = new ContextQuery();
            List<int> ordens = new List<int>();
            try
            {
                ordens = ListaOrdemPor(logAtualizacaoMatriculaServidor.MatriculaAnterior);

                foreach (var ordem in ordens)
                {
                    contextQuery.Parameters.Clear();

                    contextQuery.Command = @" UPDATE  LY_LOTACAO
                                SET     MATRICULA = @MATRICULANOVA
                                WHERE   MATRICULA = @MATRICULAANTERIOR
                                        AND ORDEM = @ORDEM ";

                    contextQuery.Parameters.Add("@ORDEM", ordem);
                    contextQuery.Parameters.Add("@MATRICULAANTERIOR", logAtualizacaoMatriculaServidor.MatriculaAnterior);
                    contextQuery.Parameters.Add("@MATRICULANOVA", logAtualizacaoMatriculaServidor.MatriculaNova);

                    ctx.ApplyModifications(contextQuery);
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
        }

        public DataTable ObtemListaLotacaoPor(int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT  
                           ly_lotacao.pessoa, 
                           ly_lotacao.ordem, 
                           ly_lotacao.matricula, 
                           f.idvinculo,
                           --ly_lotacao.setor, 
                           se.ua_antiga as setor, 
                           se.ua_atual,
                           ly_funcao.descricao         as descricao02, 
                           ly_nucleo.descricao         as descricao03, 
                           ly_unidade_ensino.nome_comp as nomecomp02, 
                           ly_turno.turno, 
                           LY_LOTACAO.data_nomeacao, 
                           LY_LOTACAO.data_nomeacao_do, 
                           LY_LOTACAO.data_desativacao, 
                           data_desativacao_do, 
                           resp_documentacao, 
                           ato_oficial, 
                           LY_LOTACAO.unidade_fis  
                   FROM   LY_LOTACAO 
                   INNER JOIN LY_FUNCAO 
                           ON LY_FUNCAO.FUNCAO = LY_LOTACAO.FUNCAO 
                   INNER JOIN LY_PESSOA 
                           ON LY_PESSOA.PESSOA = LY_LOTACAO.PESSOA                     
				   LEFT JOIN VW_FUNCIONARIOS f
						   on LY_PESSOA.PESSOA = f.PESSOA and LY_LOTACAO.MATRICULA = f.MATRICULA
                   LEFT JOIN LY_TURNO 
                          ON LY_TURNO.TURNO = LY_LOTACAO.TURNO 
                   LEFT JOIN LY_NUCLEO 
                          ON LY_NUCLEO.NUCLEO = LY_LOTACAO.NUCLEO 
                   LEFT JOIN LY_UNIDADE_ENSINO 
                          ON LY_UNIDADE_ENSINO.UNIDADE_ENS = LY_LOTACAO.UNIDADE_ENS 
                   INNER JOIN HADES..VW_SETOR SE ON SE.SETOR = LY_LOTACAO.SETOR
                    WHERE   LY_LOTACAO.PESSOA = @PESSOA
                               AND CAMPO_02 <> 'S' 
                               AND CONVERT(DATE,LY_LOTACAO.DATA_NOMEACAO) <= CONVERT(DATE, GETDATE()) 
                               AND ( LY_LOTACAO.DATA_DESATIVACAO IS NULL 
                                      OR CONVERT(DATE, LY_LOTACAO.DATA_DESATIVACAO) > 
                                         CONVERT(DATE, GETDATE()) 
                                   )
                           
                    ORDER BY LY_LOTACAO.MATRICULA ,
                            LY_LOTACAO.DATA_NOMEACAO DESC
 ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public ValidacaoDados ValidaMovimentacaoServidor(DadosMovimentacaoServidor dadosMovimentacaoServidor)
        {
            List<string> mensagens = new List<string>();
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
            LyLotacao lotacao = new LyLotacao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            //Verifica campos obrigatórios
            if (dadosMovimentacaoServidor.SetorDestino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a Unidade Administrativa.");
            }

            if (dadosMovimentacaoServidor.Pessoa <= 0 || dadosMovimentacaoServidor.Matricula.IsNullOrEmptyOrWhiteSpace() || dadosMovimentacaoServidor.Ordem <= 0 || dadosMovimentacaoServidor.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campos pessoa e/ou matricula e/ou ordem e/ou usuarioResponsavel não encontrados.");
            }

            if (mensagens.Count == 0)
            {
                if (!RN.Setores.ExisteSetor(dadosMovimentacaoServidor.SetorDestino.ToString()))
                {
                    mensagens.Add("Unidade Administrativa não encontrada.");
                }

                lotacao = ObtemLotacaoAtivaPor(dadosMovimentacaoServidor.Matricula);

                //Verifica se setor foi alterado
                if (dadosMovimentacaoServidor.SetorDestino != lotacao.Setor)
                {
                    //só permite que o usuário troque funcionários da mesma coordenadoria que ele
                    if (!RN.Usuarios.UsuarioPrivilegiado(dadosMovimentacaoServidor.UsuarioResponsavel))
                    {
                        RN.Setores rnSetores = new Techne.Lyceum.RN.Setores();
                        string matricula_usuario = RN.Usuarios.ObterMatriculaUsuario(dadosMovimentacaoServidor.UsuarioResponsavel);
                        string setor_usuario = RN.Lotacao.ObterSetorUsuario(matricula_usuario);
                        string tipoSetor = rnSetores.ObtemTipoSetorPor(setor_usuario);

                        if (!string.IsNullOrEmpty(setor_usuario))
                        {
                            if (!tipoSetor.IsNullOrEmptyOrWhiteSpace())
                            {
                                //não permitir mudar o usuário para o setor da coordenadoria
                                if (setor_usuario == dadosMovimentacaoServidor.SetorDestino)
                                {
                                    mensagens.Add("Não é possível trocar a UA de um funcionário para a UA da " + tipoSetor + " .");
                                }
                            }
                        }
                    }

                    //Busca numero do docente
                    decimal num_func = rnDocentes.ObtemNumFuncPor(dadosMovimentacaoServidor.Matricula);

                    if (num_func != 0)//Verifica se é um docente
                    {
                        //verifica se tem aulas em outro setor diferente do de destino
                        if (RN.AulaDocente.VerificaAulasAlocadasFuturas(num_func, dadosMovimentacaoServidor.SetorDestino))
                        {
                            mensagens.Add("Docente possui aulas alocadas em setor diferente do selecionado, portanto, não é possível alterar a UA.");
                        }
                    }

                    if (dadosMovimentacaoServidor.DataMovimentacao.Date < lotacao.DataNomeacao.Date)
                    {
                        mensagens.Add("Não é possivel realizar movimentação de um servidor mais de uma vez no mesmo dia. Verifique na tela de Lotação se já existe movimentação para a data de hoje.");
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

        public DataTable ListaLotacaoDocentePor(int pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT MATRICULA, MAX(DATA_NOMEACAO) AS DATA_NOMEACAO
                                            INTO #PODEREMOVER
                                            FROM LY_LOTACAO (NOLOCK)
                                            WHERE PESSOA = @PESSOA
                                            GROUP BY MATRICULA;


                                            SELECT 
                                                D.NUM_FUNC, 
                                                L.MATRICULA,
                                                ISNULL((CONVERT(VARCHAR,LP.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D.VINCULO)),D.MATRICULA) IDVINCULO, 
                                                LP.IDFUNCIONAL,
                                                D.VINCULO,
                                                SE.UA_ANTIGA AS SETOR, 
                                                SE.UA_ATUAL,
                                                UE.ID_REGIONAL, 
                                                N.REGIONAL AS DESCRICAO_REGIONAL, 
                                                UE.NOME_COMP, 
                                                L.UNIDADE_ENS, 
                                                L.UNIDADE_FIS, 
                                                D.PESSOA, 
                                                D.NUM_FUNC, 
                                                L.ORDEM, 
                                                F.FUNCAO, 
                                                F.DESCRICAO, 
                                                CD.NOME AS CARGO,
                                                L.DATA_NOMEACAO, 
                                                L.DATA_NOMEACAO_DO, 
                                                L.DATA_DESATIVACAO, 
                                                L.DATA_DESATIVACAO_DO, 
                                                L.ATO_OFICIAL, 
                                                L.RESP_DOCUMENTACAO, 
                                                L.USUARIO,
                                                L.DATA_ATUALIZACAO,
                                                GH.DESCRICAO AS DISCIPLINA_INGRESSO, 

                                                CASE WHEN L.READAPTADO = 'S' THEN 'Sim' ELSE 'Não' END READAPTADO, 
                                                L.DT_INICIO_READAPTACAO,
                                                L.DT_FIM_READAPTACAO, 

                                                LIC.MOTIVO,
                                                LIC.DTINI,
                                                LIC.DTFIM,

                                                CASE 
                                                    WHEN LIC_CH.DTINI IS NOT NULL THEN 'Sim' 
                                                    ELSE 'Não' 
                                                END AS REDUCAOCH,

                                                LIC_CH.DTINI AS DTINICH,
                                                LIC_CH.DTFIM AS DTFIMCH,

                                                CASE
                                                    WHEN R.MATRICULA IS NULL THEN 'N'
                                                    ELSE 'S' 
                                                END PODE_REMOVER,

                                                FUNCAOBB AS TIPOFUNCAO

                                            FROM LY_LOTACAO L 

                                            LEFT JOIN #PODEREMOVER R 
                                                ON L.MATRICULA = R.MATRICULA 
                                               AND L.DATA_NOMEACAO = R.DATA_NOMEACAO

                                            INNER JOIN LY_DOCENTE D 
                                                ON L.MATRICULA = D.MATRICULA 

                                            LEFT JOIN ly_grupo_habilitacao_doc ghdoc 
                                                ON D.NUM_FUNC = ghdoc.NUM_FUNC 
                                               AND AGRUPAMENTO_INGRESSO = 'S'

                                            LEFT JOIN ly_grupo_habilitacao gh 
                                                ON ghdoc.agrupamento = gh.agrupamento 

                                            INNER JOIN LY_FUNCAO F 
                                                ON L.FUNCAO = F.FUNCAO                                                

                                            OUTER APPLY (
                                                SELECT TOP 1 
                                                    LD.MOTIVO,
                                                    LD.DTINI,
                                                    LD.DTFIM
                                                FROM LY_LICENCA_DOCENTE LD
                                                WHERE LD.NUM_FUNC = D.NUM_FUNC

                                                  AND LD.MOTIVO <> '43'

                                                  AND LD.DTINI <= ISNULL(L.DATA_DESATIVACAO, GETDATE())
                                                  AND (LD.DTFIM IS NULL OR LD.DTFIM >= L.DATA_NOMEACAO)

                                                  AND (LD.DTFIM IS NULL OR LD.DTFIM >= GETDATE())

                                                ORDER BY LD.DTINI DESC
                                            ) LIC

                                            OUTER APPLY (
                                                SELECT TOP 1 
                                                    LD.DTINI,
                                                    LD.DTFIM
                                                FROM LY_LICENCA_DOCENTE LD
                                                WHERE LD.NUM_FUNC = D.NUM_FUNC

                                                  AND LD.MOTIVO = '43'

                                                  AND LD.DTINI <= ISNULL(L.DATA_DESATIVACAO, GETDATE())
                                                  AND (LD.DTFIM IS NULL OR LD.DTFIM >= L.DATA_NOMEACAO)

                                                ORDER BY LD.DTINI DESC
                                            ) LIC_CH

                                            LEFT JOIN LY_UNIDADE_ENSINO UE 
                                                ON L.UNIDADE_ENS = UE.UNIDADE_ENS 

                                            LEFT JOIN TCE_REGIONAL N 
                                                ON UE.ID_REGIONAL = N.ID_REGIONAL 

                                            INNER JOIN LY_CATEGORIA_DOCENTE CD 
                                                ON L.CATEGORIA = CD.CATEGORIA

                                            INNER JOIN HADES..VW_SETOR SE 
                                                ON SE.SETOR = L.SETOR

                                            INNER JOIN LY_PESSOA LP 
                                                ON LP.PESSOA = L.PESSOA
                                                                                          
                                            WHERE L.PESSOA = @PESSOA

                                            ORDER BY L.MATRICULA, L.DATA_NOMEACAO DESC;

                                            DROP TABLE #PODEREMOVER;
                                            ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

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

        public DataTable ListaLicencaCHPor(int numfunc)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT * FROM dbo.LY_LICENCA_DOCENTE WHERE NUM_FUNC = @NUM_FUNC AND MOTIVO = '43' ";

                contextQuery.Parameters.Add("@NUM_FUNC", numfunc);

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

        public DataTable ListaLotacaoFuncionarioPor(int pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT MATRICULA, MAX(DATA_NOMEACAO) AS DATA_NOMEACAO
                                            INTO #PODEREMOVER
                                            FROM LY_LOTACAO (NOLOCK)
                                            WHERE PESSOA = @PESSOA
                                            GROUP BY MATRICULA

                                            SELECT L.MATRICULA,
                                                   ISNULL((convert(varchar,LP.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) idvinculo, 
                                                   LP.IDFUNCIONAL,
                                                   d.VINCULO,
                                                   SE.UA_ANTIGA AS SETOR,
                                                   SE.UA_ATUAL,
                                                   UE.ID_REGIONAL, 
                                                   N.REGIONAL AS DESCRICAO_REGIONAL,  
                                                   UE.NOME_COMP, 
                                                   L.UNIDADE_ENS, 
                                                   L.UNIDADE_FIS, 
                                                   D.PESSOA, 
                                                   L.ORDEM, 
                                                   F.FUNCAO, 
                                                   F.DESCRICAO, 
                                                   CD.NOME AS CARGO,
                                                   L.DATA_NOMEACAO, 
                                                   L.DATA_NOMEACAO_DO, 
                                                   L.DATA_DESATIVACAO, 
                                                   L.DATA_DESATIVACAO_DO, 
                                                   L.ATO_OFICIAL, 
                                                   L.RESP_DOCUMENTACAO, 
                                                   L.USUARIO, 
                                                   L.DATA_ATUALIZACAO,

                                                   CASE
                                                        WHEN L.READAPTADO = 'S' THEN 'Sim'
                                                        ELSE 'Não' 
                                                   END READAPTADO,

                                                   LIC.MOTIVO, 
                                                   LIC.DTINI, 
                                                   LIC.DTFIM,

                                                   CASE
                                                        WHEN LIC_CH.DTINI IS NOT NULL THEN 'Sim'
                                                        ELSE 'Não'
                                                   END AS REDUCAOCH,

                                                   LIC_CH.DTINI AS DTINICH,
                                                   LIC_CH.DTFIM AS DTFIMCH,

                                                   CASE
                                                        WHEN R.MATRICULA IS NULL THEN 'N'
                                                        ELSE 'S' 
                                                   END PODE_REMOVER,

                                                   FUNCAOBB as TIPOFUNCAO

                                            FROM LY_LOTACAO L 

                                            LEFT JOIN #PODEREMOVER R 
                                                   ON L.MATRICULA = R.MATRICULA 
                                                  AND L.DATA_NOMEACAO = R.DATA_NOMEACAO

                                            INNER JOIN LY_VINCULO D 
                                                    ON L.MATRICULA = D.MATRICULA 

                                            INNER JOIN LY_FUNCAO F 
                                                    ON L.FUNCAO = F.FUNCAO 

                                            OUTER APPLY (
                                                SELECT TOP 1
                                                       LD.MOTIVO,
                                                       LD.DTINI,
                                                       LD.DTFIM
                                                FROM LY_LICENCA_PESSOA LD
                                                INNER JOIN LY_LICENCAS LI
                                                        ON LI.MOTIVO = LD.MOTIVO
                                                WHERE LD.PESSOA = D.PESSOA
                                                  AND LD.ORDEM = D.ORDEM

                                                  AND LD.MOTIVO <> '43'

                                                  AND (
                                                        LD.DTFIM IS NULL
                                                        OR LD.DTFIM >= CONVERT(DATE, GETDATE())
                                                      )

                                                ORDER BY LD.DTINI DESC
                                            ) LIC

                                            OUTER APPLY (
                                                SELECT TOP 1
                                                       LD.DTINI,
                                                       LD.DTFIM
                                                FROM LY_LICENCA_PESSOA LD
                                                INNER JOIN LY_LICENCAS LI
                                                        ON LI.MOTIVO = LD.MOTIVO
                                                WHERE LD.PESSOA = D.PESSOA
                                                  AND LD.ORDEM = D.ORDEM

                                                  AND LD.MOTIVO = '43'

                                                  AND (
                                                        LD.DTFIM IS NULL
                                                        OR LD.DTFIM >= CONVERT(DATE, GETDATE())
                                                      )

                                                ORDER BY LD.DTINI DESC
                                            ) LIC_CH

                                            LEFT JOIN LY_UNIDADE_ENSINO UE 
                                                   ON L.UNIDADE_ENS = UE.UNIDADE_ENS 

                                            LEFT JOIN TCE_REGIONAL N 
                                                   ON UE.ID_REGIONAL = N.ID_REGIONAL 

                                            LEFT JOIN LY_CATEGORIA_DOCENTE CD 
                                                   ON L.CATEGORIA = CD.CATEGORIA

                                            INNER JOIN HADES..VW_SETOR SE 
                                                    ON SE.SETOR = L.SETOR

                                            INNER JOIN LY_PESSOA LP 
                                                    ON LP.PESSOA = L.PESSOA
                                                                                       
                                            WHERE L.PESSOA = @PESSOA 
                                              AND (
                                                    D.DATA_DESATIVACAO IS NULL 
                                                    OR CONVERT(DATE, D.DATA_DESATIVACAO) >= CONVERT(DATE, GETDATE())
                                                  ) 
                                              AND (
                                                    L.DATA_DESATIVACAO IS NULL 
                                                    OR L.DATA_DESATIVACAO >= GETDATE()
                                                  )

                                            UNION 

                                            SELECT L.MATRICULA,
                                                   ISNULL((convert(varchar,LP.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) idvinculo, 
                                                   LP.IDFUNCIONAL,
                                                   d.VINCULO,
                                                   SE.UA_ANTIGA,
                                                   SE.UA_ATUAL,
                                                   UE.ID_REGIONAL, 
                                                   N.REGIONAL AS DESCRICAO_REGIONAL,  
                                                   UE.NOME_COMP, 
                                                   L.UNIDADE_ENS, 
                                                   L.UNIDADE_FIS, 
                                                   D.PESSOA, 
                                                   L.ORDEM, 
                                                   F.FUNCAO, 
                                                   F.DESCRICAO, 
                                                   CD.NOME AS CARGO,
                                                   L.DATA_NOMEACAO, 
                                                   L.DATA_NOMEACAO_DO, 
                                                   L.DATA_DESATIVACAO, 
                                                   L.DATA_DESATIVACAO_DO, 
                                                   L.ATO_OFICIAL, 
                                                   L.RESP_DOCUMENTACAO, 
                                                   L.USUARIO, 
                                                   L.DATA_ATUALIZACAO,

                                                   CASE
                                                        WHEN L.READAPTADO = 'S' THEN 'Sim'
                                                        ELSE 'Não' 
                                                   END READAPTADO,

                                                   NULL AS MOTIVO, 
                                                   NULL AS DTINI, 
                                                   NULL AS DTFIM,

                                                   'Não' AS REDUCAOCH,
                                                   NULL AS DTINICH,
                                                   NULL AS DTFIMCH,

                                                   CASE
                                                        WHEN R.MATRICULA IS NULL THEN 'N'
                                                        ELSE 'S' 
                                                   END PODE_REMOVER,

                                                   FUNCAOBB as TIPOFUNCAO

                                            FROM LY_LOTACAO L 

                                            LEFT JOIN #PODEREMOVER R 
                                                   ON L.MATRICULA = R.MATRICULA 
                                                  AND L.DATA_NOMEACAO = R.DATA_NOMEACAO

                                            INNER JOIN LY_VINCULO D 
                                                    ON L.MATRICULA = D.MATRICULA 

                                            INNER JOIN LY_FUNCAO F 
                                                    ON L.FUNCAO = F.FUNCAO 

                                            LEFT JOIN LY_UNIDADE_ENSINO UE 
                                                   ON L.UNIDADE_ENS = UE.UNIDADE_ENS 

                                            LEFT JOIN TCE_REGIONAL N 
                                                   ON UE.ID_REGIONAL = N.ID_REGIONAL  

                                            LEFT JOIN LY_CATEGORIA_DOCENTE CD 
                                                   ON L.CATEGORIA = CD.CATEGORIA

                                            INNER JOIN HADES..VW_SETOR SE 
                                                    ON SE.SETOR = L.SETOR

                                            INNER JOIN LY_PESSOA LP 
                                                    ON LP.PESSOA = L.PESSOA

                                            WHERE L.PESSOA = @PESSOA 
                                              AND L.DATA_DESATIVACAO IS NOT NULL 
                                              AND L.DATA_DESATIVACAO < GETDATE()

                                            ORDER BY L.MATRICULA, 
                                                     L.DATA_NOMEACAO DESC

                                            DROP TABLE #PODEREMOVER";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

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

        public DataTable ListaLotacaoAtivaPor(int pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT F.MATRICULA, 
	                                        FU.DESCRICAO AS FUNCAO, 
	                                        S.UA_ATUAL AS UA, 
	                                        RE.REGIONAL,
	                                        MU.NOME AS MUNICIPIO,
	                                        F.UNIDADE_ENS AS CENSO,
	                                        ISNULL(UE.NOME_COMP, NOMESETOR) AS ESCOLA,
	                                        UE.FONE,
	                                        UE.E_MAIL,
	                                        CASE 
												WHEN UE.UNIDADE_ENS IS NULL THEN ''
												ELSE ISNULL(UE.ENDERECO, '') + ', ' + ISNULL(UE.END_NUM, '') + ' ' + ISNULL(UE.END_COMPL, '') + ' - ' + ISNULL(B.DESCRICAO, '') 
											END ENDERECOESCOLA,
                                            IDVINCULO
									    FROM VW_FUNCIONARIOS F
											LEFT JOIN HADES..VW_SETOR S ON F.SETOR = S.SETOR
	                                        LEFT JOIN LY_FUNCAO FU ON F.FUNCAO = FU.FUNCAO
	                                        LEFT JOIN LY_UNIDADE_ENSINO UE ON F.UNIDADE_ENS = UE.UNIDADE_ENS
	                                        LEFT JOIN TCE_REGIONAL RE ON UE.ID_REGIONAL = RE.ID_REGIONAL
	                                        LEFT JOIN HADES..HD_MUNICIPIO MU ON UE.MUNICIPIO = MU.MUNICIPIO
	                                        LEFT JOIN HADES..BAIRRO B ON UE.BAIRRO = B.CODIGO
                                        WHERE PESSOA = @PESSOA
	                                        AND DATA_DESATIVACAO IS NULL ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

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

        public DataTable ListaServidoresPor(string unidadeEnsino)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                                        SELECT DISTINCT d.num_func AS num_func,
                                               l.matricula AS matricula,
                                               d.pessoa AS pessoa,
                                               PE.nome_compl AS nome,
                                               f.funcao AS funcao,
                                               L.categoria AS cargo,
                                               l.setor,
                                               se.ua_atual,
                                               l.data_desativacao AS data_desativacao,
                                               l.readaptado,
                                               l.dt_inicio_readaptacao AS dt_inicio_readaptacao,
                                               l.dt_fim_readaptacao AS dt_fim_readaptacao,
                                               gh.descricao AS disciplina, 

                                               (SELECT (SELECT Count(1)
                                                        FROM ly_aula_docente ad
                                                        JOIN ly_turma t ON ad.ano = t.ano
                                                                       AND ad.semestre = t.semestre
                                                                       AND ad.disciplina = t.disciplina
                                                                       AND ad.faculdade = t.faculdade
                                                                       AND ad.turma = t.turma
                                                                       AND ad.turno = t.turno
                                                                       AND ad.data_fim = t.dt_fim
                                                        WHERE ad.num_func = d.num_func
                                                          AND t.sit_turma = 'Aberta'
                                                          AND ad.data_fim >= CONVERT(DATE, Getdate()))
                                                    -
                                                    (SELECT Count(1)
                                                     FROM ly_aula_docente_tipo ad
                                                     JOIN ly_turma t ON ad.ano = t.ano
                                                                    AND ad.semestre = t.semestre
                                                                    AND ad.disciplina = t.disciplina
                                                                    AND ad.faculdade = t.faculdade
                                                                    AND ad.turma = t.turma
                                                                    AND ad.turno = t.turno
                                                                    AND ad.data_fim = t.dt_fim
                                                     WHERE ad.num_func = d.num_func
                                                       AND t.sit_turma = 'Aberta'
                                                       AND ad.data_fim >= CONVERT(DATE, Getdate())
                                                       AND ad.data_inicio <= CONVERT(DATE, Getdate())
                                                       AND ad.tipo_aula = 'GLP')) AS aulas_alocadas,

                                               (SELECT Count(1)
                                                FROM ly_aula_docente_tipo ad
                                                JOIN ly_turma t ON ad.ano = t.ano
                                                               AND ad.semestre = t.semestre
                                                               AND ad.disciplina = t.disciplina
                                                               AND ad.faculdade = t.faculdade
                                                               AND ad.turma = t.turma
                                                               AND ad.turno = t.turno
                                                               AND ad.data_fim = t.dt_fim
                                                WHERE ad.num_func = d.num_func
                                                  AND t.sit_turma = 'Aberta'
                                                  AND ad.data_fim >= CONVERT(DATE, Getdate())
                                                  AND ad.data_inicio <= CONVERT(DATE, Getdate())
                                                  AND ad.tipo_aula = 'GLP') AS aulas_alocadas_glp,

                                               lic.motivo AS motivo,
                                               lic.dtini AS dataini,
                                               lic.dtfim AS datafim,
                                               
                                               CASE 
                                                   WHEN lic_ch.dtini IS NOT NULL THEN 'S'
                                                   ELSE 'N'
                                               END AS reducaoch,

                                               lic_ch.dtini AS dtinich,
                                               lic_ch.dtfim AS dtfimch,

                                               ISNULL((convert(varchar,PE.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)),d.matricula) idvinculo,
                                               0 AS ordemLicenca

                                        FROM ly_lotacao l
                                        JOIN ly_unidade_ensino ue ON l.unidade_ens = ue.unidade_ens
                                        JOIN ly_docente d ON l.matricula = d.matricula
                                        JOIN LY_PESSOA PE ON PE.PESSOA = D.PESSOA
                                        JOIN ly_funcao f ON l.funcao = f.funcao

                                        LEFT JOIN (
                                            SELECT ld.num_func,
                                                   ld.dtfim,
                                                   ld.dtini,
                                                   ld.motivo + '|' + li.possui_dtfim AS motivo,
                                                   0 AS ordemLicenca
                                            FROM ly_licenca_docente ld
                                            INNER JOIN ly_licencas li ON li.motivo = ld.motivo
                                            WHERE Isnull(ld.dtfim, '') >= 
                                                  (CASE li.possui_dtfim
                                                      WHEN 'N' THEN ''
                                                      ELSE CONVERT(DATE, Getdate())
                                                   END)
                                              AND ld.motivo <> '43'
                                        ) lic
                                        ON lic.num_func = d.num_func
                                        AND (
                                            lic.dtfim IS NULL
                                            OR lic.dtfim = (
                                                SELECT TOP 1 ld.dtfim
                                                FROM ly_licenca_docente ld
                                                WHERE ld.num_func = d.num_func
                                                  AND ld.motivo <> '43'
                                                ORDER BY ld.dtfim DESC
                                            )
                                        )

                                        OUTER APPLY (
                                            SELECT TOP 1 
                                                ld.dtini,
                                                ld.dtfim
                                            FROM ly_licenca_docente ld
                                            WHERE ld.num_func = d.num_func
                                              AND ld.motivo = '43'
                                                AND (
                                            ld.dtfim IS NULL
											or  ld.dtfim > GETDATE())
                                            ORDER BY ld.dtini DESC
                                        ) lic_ch

                                        LEFT JOIN ly_grupo_habilitacao_doc ghd
                                               ON d.num_func = ghd.num_func
                                              AND ghd.agrupamento_ingresso = 'S'
                                              AND ghd.provisorio = 'N'

                                        LEFT JOIN ly_grupo_habilitacao gh
                                               ON ghd.agrupamento = gh.agrupamento

                                        INNER JOIN HADES..VW_SETOR SE 
                                               ON SE.SETOR = L.SETOR

                                        WHERE CONVERT(DATE,l.data_nomeacao) <= CONVERT(DATE, Getdate())
                                          AND (l.data_desativacao IS NULL
                                               OR CONVERT(DATE, l.data_desativacao) > CONVERT(DATE, Getdate()))
                                          AND l.unidade_ens = @UNIDADE
                                          AND L.FUNCAO <> '10311'

                                        UNION

                                        SELECT DISTINCT 0 AS num_func,
                                               v.matricula AS matricula,
                                               v.pessoa AS pessoa,
                                               p.nome_compl AS nome,
                                               f.funcao AS funcao,
                                               (SELECT i.NOME 
                                                  FROM LY_CATEGORIA_DOCENTE i 
                                                 WHERE L.categoria = i.CATEGORIA) AS cargo,
                                               l.setor,
                                               SE.UA_ATUAL,
                                               l.data_desativacao,
                                               l.readaptado,
                                               l.dt_inicio_readaptacao,
                                               l.dt_fim_readaptacao,
                                               NULL,
                                               NULL,
                                               NULL,

                                               lic.motivo,
                                               lic.dtini,
                                               lic.dtfim,

                                               CASE 
                                                   WHEN lic_ch.dtini IS NOT NULL THEN 'S'
                                                   ELSE 'N'
                                               END AS reducaoch,

                                               lic_ch.dtini,
                                               lic_ch.dtfim,

                                               ISNULL((convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,v.VINCULO)),v.matricula),
                                               isnull(lic.ordem,0)

                                        FROM ly_lotacao l
                                        JOIN ly_unidade_ensino ue ON l.unidade_ens = ue.unidade_ens
                                        JOIN ly_vinculo v ON l.matricula = v.matricula
                                        JOIN ly_funcao f ON l.funcao = f.funcao
                                        JOIN ly_pessoa p ON v.pessoa = p.pessoa
                                        INNER JOIN HADES..VW_SETOR SE 
                                                ON SE.SETOR = L.SETOR

                                        LEFT JOIN (
                                            SELECT ld.pessoa,
                                                   ld.dtfim,
                                                   ld.dtini,
                                                   ld.motivo + '|' + li.possui_dtfim AS motivo,
                                                   ld.ordem
                                            FROM ly_licenca_pessoa ld
                                            INNER JOIN ly_licencas li 
                                                    ON li.motivo = ld.motivo
                                            WHERE Isnull(ld.dtfim, '') >= 
                                                  (CASE li.possui_dtfim
                                                      WHEN 'N' THEN ''
                                                      ELSE CONVERT(DATE, Getdate())
                                                   END)
                                              AND ld.motivo <> '43'
                                        ) lic
                                        ON lic.pessoa = p.pessoa
                                        AND (
                                            lic.dtfim IS NULL
                                            OR lic.dtfim = (
                                                SELECT TOP 1 ld.dtfim
                                                FROM ly_licenca_pessoa ld
                                                WHERE ld.pessoa = p.pessoa
                                                  AND ld.motivo <> '43'
                                                ORDER BY ld.dtfim DESC
                                            )
                                        )

                                        OUTER APPLY (
                                            SELECT TOP 1 
                                                ld.dtini,
                                                ld.dtfim
                                            FROM ly_licenca_pessoa ld
                                            WHERE ld.pessoa = p.pessoa
                                              AND ld.motivo = '43'
                                                AND (
                                            ld.dtfim IS NULL
											or  ld.dtfim > GETDATE())
                                            ORDER BY ld.dtini DESC
                                        ) lic_ch

                                        WHERE CONVERT(DATE,l.data_nomeacao) <= CONVERT(DATE, Getdate())
                                          AND (l.data_desativacao IS NULL
                                               OR CONVERT(DATE, l.data_desativacao) > CONVERT(DATE, Getdate()))
                                          AND l.unidade_ens = @UNIDADE
                                          AND v.data_desativacao IS NULL  
                                          AND L.FUNCAO <> '10311'";


                contextQuery.Parameters.Add("@UNIDADE", unidadeEnsino);

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

        public ValidacaoDados ValidaInsercaoLotacaoDocente(LyLotacao lotacao, RN.Entidades.LyLicencaDocente licencaDocente, bool finalizarAnterior, out LyLotacao lotacaoAnterior, bool licencaPossuiDataFim, decimal numFunc, bool desalocaOECP)
        {
            List<string> mensagens = new List<string>();
            lotacaoAnterior = new LyLotacao();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            RN.Docentes rnDocente = new Docentes();
            RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
            int aulasAlocadas = 0;
            int cargaHorariaPermitidaFuncao = 0;
            int numeroMatriculas = 0;
            decimal proximaOrdem = 0;
            bool desalocaAulas = false;
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();
            RN.Funcao rnFuncao = new RN.Funcao();
            DataContext contexto = null;
            ValidacaoDados validacaoLicenca = new ValidacaoDados();
            List<string> validacaoCamposLotacao = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lotacao == null)
            {
                return validacaoDados;
            }

            lotacao.DataAtualizacao = DateTime.Now;
            if (lotacao.Readaptado.IsNullOrEmptyOrWhiteSpace())
            {
                lotacao.Readaptado = "N";
            }

            if (numFunc <= 0 && !string.IsNullOrEmpty(lotacao.Matricula))
            {
                mensagens.Add("Favor informar o NUMERO DO DOCENTE.");
            }

            //Valida campos obrigatorios gerais de lotação
            validacaoCamposLotacao = this.ValidaCamposObrigatorios(lotacao);
            if (validacaoCamposLotacao.Count > 0)
            {
                mensagens.AddRange(validacaoCamposLotacao);
            }

            //verifica se esta colocando licença para validar campos
            if (!licencaDocente.Motivo.IsNullOrEmptyOrWhiteSpace() || licencaDocente.Dtini != DateTime.MinValue)
            {
                //Realiza validações de situação (licença)
                validacaoLicenca = rnLicencaDocente.ValidaInsercao(licencaDocente, lotacao.Matricula, lotacao.Pessoa, out licencaPossuiDataFim, lotacao.Usuario);

                if (!validacaoLicenca.Valido)
                {
                    mensagens.Add(validacaoLicenca.Mensagem);
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiOrdemPor(contexto, lotacao.Matricula, lotacao.Ordem))
                    {
                        mensagens.Add("Esta ORDEM já foi utilizada para esta Matricula.");
                    }
                    else
                    {
                        proximaOrdem = lotacao.Ordem;

                        if (finalizarAnterior)
                        {
                            //Alimenta a lotacaoAnterior
                            //Caso a opção de finalizar anterior esteja marcada a lotação anterior será a lotação ativa
                            lotacaoAnterior = this.ObtemLotacaoAtivaPor(contexto, lotacao.Matricula);
                            if (lotacaoAnterior.Matricula.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("A lotação anterior não foi encontrada.");
                            }
                            else
                            {
                                lotacaoAnterior.DataAtualizacao = DateTime.Now;

                                //Data da desativação da lotação anterior, sera a data da nomeação - 1
                                lotacaoAnterior.DataDesativacao = Convert.ToDateTime(lotacao.DataNomeacao).AddDays(-1);

                                lotacao.Ordem = lotacaoAnterior.Ordem; //Para validação

                                //Validata data de dispensa da lotacao anterior    
                                if (lotacaoAnterior.DataDesativacao != null)
                                {
                                    if (Convert.ToDateTime(lotacaoAnterior.DataDesativacao).Date <= Convert.ToDateTime(lotacaoAnterior.DataNomeacao).Date)
                                    {
                                        mensagens.Add("Data da Dispensa da lotação anterior (" + Convert.ToDateTime(lotacaoAnterior.DataDesativacao).Date.ToShortDateString() + ") não pode ser menor ou igual a Data da Nomeação (" + Convert.ToDateTime(lotacaoAnterior.DataNomeacao).Date.ToShortDateString() + ").");
                                    }
                                }
                                if (lotacaoAnterior.DataDesativacaoDo != null)
                                {
                                    if (Convert.ToDateTime(lotacao.DataDesativacao).Date > Convert.ToDateTime(lotacao.DataDesativacaoDo).Date)
                                    {
                                        mensagens.Add("Data da Publicação da Dispensa deve ser maior ou igual a Data de Dispensa da lotação anterior (" + Convert.ToDateTime(lotacao.DataDesativacao).Date.ToShortDateString() + ").");
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Alimenta a lotacaoAnterior
                            //Caso a opção de finalizar anterior não esteja marcada a lotação anterior será a ultima (com maior data de nomeação)
                            lotacaoAnterior = this.ObtemUltimaLotacaoPor(contexto, lotacao.Matricula);
                            if (!lotacaoAnterior.Matricula.IsNullOrEmptyOrWhiteSpace())
                            {
                                //Verifica se a data da desativação da lotação anterior, é a data da nomeação - 1
                                if (lotacaoAnterior.DataDesativacao != null)
                                {
                                    if (Convert.ToDateTime(lotacaoAnterior.DataDesativacao).Date != lotacao.DataNomeacao.AddDays(-1).Date)
                                    {
                                        mensagens.Add("A DATA DE NOMEAÇÃO da lotação deve ser a data desativação da lotação anterior + 1.");
                                    }
                                }
                            }
                        }

                        //Validação de datas intercaladas padrao
                        validacaoCamposLotacao = this.ValidaIntercalacaoDatas(contexto, lotacao);
                        if (validacaoCamposLotacao.Count > 0)
                        {
                            mensagens.AddRange(validacaoCamposLotacao);
                        }

                        lotacao.Ordem = proximaOrdem;

                        if (lotacao.DataDesativacao == null)
                        {
                            if (!finalizarAnterior && this.PossuiLotacaoSemDataDesativacaoPor(contexto, lotacao.Matricula))
                            {
                                mensagens.Add("Já existe uma LOTAÇÃO SEM DATA DE DISPENSA para essa matrícula.");
                            }
                        }

                        if (finalizarAnterior && mensagens.Count == 0)
                        {
                            if (this.PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(contexto, lotacaoAnterior.Matricula, Convert.ToDateTime(lotacaoAnterior.DataDesativacao), lotacaoAnterior.Ordem))
                            {
                                mensagens.Add("DATA DE DISPENSA da lotacao anterior não pode estar dentro do intervalo de outra lotação.");
                            }

                            if (this.PossuiOutraLotacaoIntercaladaPor(contexto, lotacaoAnterior.Matricula, lotacaoAnterior.DataNomeacao, Convert.ToDateTime(lotacaoAnterior.DataDesativacao), lotacaoAnterior.Ordem))
                            {
                                mensagens.Add("DATA DE NOMEAÇÃO E DISPENSA da lotacao anterior não podem intercalar com outra lotação.");
                            }

                            if (lotacao.Funcao == "11" || lotacao.Funcao == "18")
                            {
                                desalocaAulas = desalocaOECP;
                            }
                            else
                            {
                                //Verifica se a funcao escolhida deve desalocar aulas (campo_06 = S) e desaloca aulas
                                desalocaAulas = rnFuncao.VerificaDesalocacaoAulasPor(contexto, lotacao.Funcao);
                            }

                            if (!desalocaAulas && !rnFuncao.EhFuncaoRegente(contexto, lotacao.Funcao))
                            {
                                if (lotacao.DataDesativacao != null)
                                {
                                    //verifica aulas alocadas
                                    if (rnAulaDocente.ExisteAulaAlocadaPor(contexto, numFunc, Convert.ToDateTime(lotacao.DataDesativacao)))
                                    {
                                        mensagens.Add("Não é possível incluir data da dispensa, funcionário possui aulas alocadas após a data de desativação.");
                                    }
                                }                               
                            }

                            //Verifica se função atual é diferente da anterior
                            if (!desalocaAulas && lotacaoAnterior.Funcao != lotacao.Funcao)
                            {
                                //busca aulas Alocadas
                                aulasAlocadas = rnAulaDocente.ObtemTotalAulasAlocadasPor(contexto, numFunc, Convert.ToDateTime(lotacao.DataNomeacao));

                                //Busca quantidade de matricula que a pessoa possui
                                numeroMatriculas = this.ObtemNumeroMatriculaDocentePor(contexto, lotacao.Pessoa, lotacao.DataNomeacao);

                                if (aulasAlocadas > 0 && !desalocaAulas)
                                {
                                    //Se o docente possuir aulas alocadas e a nova função escolhida não for do tipo Regente impedir o salvamento.
                                    if (!rnFuncao.EhFuncaoRegente(contexto, lotacao.Funcao))
                                    {
                                        mensagens.Add("Não é possível alterar a função do funcionário, pois a nova função não é do TIPO REGENTE e o funcionário possui aulas alocadas.");
                                    }
                                }

                                //Busca categoria
                                string categoria = rnDocente.ObtemCategoriaPor(contexto, lotacao.Matricula);

                                //Atualiza carga Horaria do Docente
                                cargaHorariaPermitidaFuncao = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(contexto, categoria, lotacao.Funcao);

                                //Se a quantidade de tempos alocados para o docente exceder a carga horária total configurada para
                                //a nova função do docente, não permitir o salvamento.
                                if (aulasAlocadas > cargaHorariaPermitidaFuncao)
                                {
                                    mensagens.Add("Não é possível alterar a função do funcionário, pois a quantidade de tempos alocados é superior a CH da nova função.");
                                }
                            }
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

        public void InsereLotacaoDocente(LyLotacao lotacao, LyLicencaDocente licencaDocente, bool finalizarAnterior, LyLotacao lotacaoAnterior, bool licencaPossuiDataFim, decimal numFunc, bool desalocaOECP, string idVinculo, string nomeDocente)
        {
            RN.Funcao rnFuncao = new Funcao();
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();
            RN.LicencaDocente rnLicencaDocente = new RN.LicencaDocente();
            bool desalocaAulas = false;
            RN.Setores rnSetores = new Setores();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            int totalGLP = 0;

            try
            {
                //Verifica se a lotacao tem data de desativação
                if (lotacao.DataDesativacao == null || Convert.ToDateTime(lotacao.DataDesativacao) >= DateTime.Today)
                {

                    if (lotacao.Funcao == "11" || lotacao.Funcao == "18")
                    {
                        desalocaAulas = desalocaOECP;
                        totalGLP = rnAulaDocenteTipo.ObtemQuantidadeGlpsPor(numFunc, Convert.ToDateTime(lotacaoAnterior.DataDesativacao));
                    }
                    else
                    {
                        //Verifica se a funcao escolhida deve desalocar aulas (campo_06 = S) e desaloca aulas
                        desalocaAulas = rnFuncao.VerificaDesalocacaoAulasPor(ctx, lotacao.Funcao);
                    }

                    if (desalocaAulas && !rnFuncao.EhFuncaoRegente(ctx, lotacao.Funcao))
                    {

                        //Substitui aulas após a data de desativaçao da lotação por carência
                        rnAulaDocente.SubstituiPorCarenciaPor(ctx, numFunc, Convert.ToDateTime(lotacaoAnterior.DataDesativacao));

                        //desaloca aulas
                        rnAulaDocente.DesalocaAulas(ctx, numFunc, Convert.ToDateTime(lotacaoAnterior.DataDesativacao));

                        //desaloca aulas glp
                        rnAulaDocenteTipo.DesalocaAulasTipo(ctx, numFunc, Convert.ToDateTime(lotacaoAnterior.DataDesativacao));

                        //Atualiza glps usadas
                        rnDocenteGLP.AtualizaGlpUsadaPor(ctx, numFunc, Convert.ToDateTime(lotacaoAnterior.DataDesativacao));

                        if (totalGLP > 0 && (lotacao.Funcao == "11" || lotacao.Funcao == "18"))
                        {
                            string regional = rnSetores.ObtemRegionalPor(lotacao.Setor);

                            if (regional.IsNullOrEmptyOrWhiteSpace())
                                regional = rnUnidadeEnsino.ObtemRegionalPor(lotacao.Setor);

                            EnviaEmailLotacaoOE_CP(idVinculo, nomeDocente, rnFuncao.ObtemDescricaoPor(lotacao.Funcao), regional, totalGLP, lotacao.Usuario);
                        }
                    }
                }

                //Verifica se será necessario finalizar a lotação anterior com data de desativação
                if (finalizarAnterior)
                {
                    //Desativa a lotação anterior
                    this.DesativaLotacao(ctx, Convert.ToDateTime(lotacaoAnterior.DataDesativacao), lotacaoAnterior.Usuario, lotacaoAnterior.Matricula);
                }

                lotacao.DtInicioReadaptacao = null;
                lotacao.DtFimReadaptacao = null;

                //insere lotação
                this.Insere(ctx, lotacao);

                //verifica se existe licença
                if (!licencaDocente.Motivo.IsNullOrEmptyOrWhiteSpace() && licencaDocente.Dtini != null)
                {
                    //verifica se já existe essa licença ou outra ativa
                    if (!rnLicencaDocente.ExisteLicencaPor(ctx, licencaDocente.Dtini, licencaDocente.NumFunc) && !rnLicencaDocente.ExisteLicencaAtivaPor(ctx, licencaDocente.NumFunc))
                    {
                        rnLicencaDocente.Insere(ctx, licencaDocente);

                        if (!licencaPossuiDataFim && lotacao.DataDesativacao == null)
                        {
                            //Para licenças definitivas desativa a lotação atualizada
                            this.DesativaLotacao(ctx, licencaDocente.Dtini, lotacao.Usuario, lotacao.Matricula);
                        }
                    }
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

        public ValidacaoDados ValidaAlteracaoLotacaoDocente(LyLotacao lotacao, decimal numFunc, out LyLotacao proximaLotacao, out bool desalocaAulas)
        {
            List<string> mensagens = new List<string>();
            proximaLotacao = new LyLotacao();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            LyLotacao lotacaoBase = new LyLotacao();
            RN.Funcao rnFuncao = new Funcao();
            DataContext contexto = null;
            bool funcaoRegente = false;
            bool funcaoProximaRegente = false;
            List<string> validacaoCamposLotacao = new List<string>();
            desalocaAulas = false;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lotacao == null)
            {
                return validacaoDados;
            }

            if (numFunc <= 0)
            {
                mensagens.Add("Favor informar o NUMERO DO DOCENTE.");
            }

            //Valida campos obrigatorios gerais de lotação
            validacaoCamposLotacao = this.ValidaCamposObrigatorios(lotacao);
            if (validacaoCamposLotacao.Count > 0)
            {
                mensagens.AddRange(validacaoCamposLotacao);
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiDataNomeacaoEmOutroIntervaloLotacaoPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, lotacao.Ordem))
                    {
                        mensagens.Add("DATA DE NOMEAÇÃO não pode estar dentro do intervalo de outra lotação.");
                    }

                    if (lotacao.DataDesativacao != null)
                    {
                        //Busca lotação como está na base hoje
                        lotacaoBase = this.ObtemLotacaoPor(contexto, lotacao.Pessoa, lotacao.Matricula, lotacao.Ordem);

                        //Verifica se a data de desativação estava nula e a lotacao passa a estar desativada
                        if (lotacao.DataDesativacao != null && lotacao.DataDesativacao <= DateTime.Now && lotacaoBase.DataDesativacao == null)
                        {
                            mensagens.Add("Não é possível desativar a lotação de uma matrícula sem que seja inserido um afastamento sem data fim.");
                        }

                        //Verificar se a função da lotação atual é de regente
                        funcaoRegente = rnFuncao.EhFuncaoRegente(contexto, lotacao.Funcao);

                        //Verifica se existe lotaçao posterior a lotação que está sendo alterada
                        proximaLotacao = this.ObtemProximaLotacaoPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, lotacao.Ordem);

                        if (!proximaLotacao.Matricula.IsNullOrEmptyOrWhiteSpace())
                        {
                            proximaLotacao.DataNomeacao = Convert.ToDateTime(lotacao.DataDesativacao).AddDays(1);
                            proximaLotacao.DataAtualizacao = DateTime.Now;
                            proximaLotacao.Usuario = lotacao.Usuario;

                            //Verificar se mudou de função deste para a proxima lotação
                            if (proximaLotacao.Funcao != lotacao.Funcao)
                            {
                                //Verificar se a função da proxima lotação é de regente
                                funcaoProximaRegente = rnFuncao.EhFuncaoRegente(contexto, proximaLotacao.Funcao);

                                if (funcaoProximaRegente != funcaoRegente)
                                {
                                    //Para as lotações que não serão de regente, validar se existe aula nos intervalos
                                    if (!funcaoProximaRegente)
                                    {
                                        DateTime dataDesativacao = proximaLotacao.DataDesativacao == (DateTime?)null ? DateTime.Now : Convert.ToDateTime(proximaLotacao.DataDesativacao);

                                        //Valida se tem aulas do intervalo
                                        if (rnAulaDocente.ExisteAulaAlocadaPeriodoLotacaoPor(contexto, numFunc, proximaLotacao.DataNomeacao, dataDesativacao))
                                        {
                                            mensagens.Add("Não é possível alterar a data da dispensa, pois o funcionário possui aulas alocadas na data informada.");
                                        }
                                    }

                                    if (!funcaoRegente)
                                    {
                                        //Valida se tem aulas do intervalo
                                        if (rnAulaDocente.ExisteAulaAlocadaPeriodoLotacaoPor(contexto, numFunc, lotacao.DataNomeacao, Convert.ToDateTime(lotacao.DataDesativacao)))
                                        {
                                            mensagens.Add("Não é possível alterar a data da dispensa, pois o funcionário possui aulas alocadas na data informada.");
                                        }
                                    }
                                }
                            }

                            if (proximaLotacao.DataDesativacao != null)
                            {
                                if (Convert.ToDateTime(proximaLotacao.DataDesativacao).Date <= Convert.ToDateTime(proximaLotacao.DataNomeacao).Date)
                                {
                                    mensagens.Add("Data da Dispensa da PRÓXIMA LOTAÇÃO não pode ser menor ou igual a data da dispensa desta lotação + 1.");
                                }
                            }
                        }
                        else if (lotacao.DataDesativacao <= DateTime.Now)
                        {
                            //Caso a lotação seja a ultima, verifica se existe aulas apos a data de desativação para serem canceladas
                            if (rnAulaDocente.ExisteAulaAlocadaPor(contexto, numFunc, Convert.ToDateTime(lotacao.DataDesativacao)))
                            {
                                desalocaAulas = true;
                            }
                        }

                        if (this.PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(contexto, lotacao.Matricula, Convert.ToDateTime(lotacao.DataDesativacao), lotacao.Ordem, proximaLotacao.Ordem))
                        {
                            mensagens.Add("DATA DE DISPENSA não pode estar dentro do intervalo de outra lotação.");
                        }

                        if (this.PossuiOutraLotacaoIntercaladaPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, Convert.ToDateTime(lotacao.DataDesativacao), lotacao.Ordem, proximaLotacao.Ordem))
                        {
                            mensagens.Add("DATA DE NOMEAÇÃO e dispensa não podem intercalar com outra lotação.");
                        }
                    }
                    else
                    {
                        if (this.PossuiOutraLotacaoAbertaPor(contexto, lotacao.Matricula, lotacao.Ordem))
                        {
                            mensagens.Add("Já existe uma LOTAÇÃO SEM DATA DE DISPENSA para esta matricula.");
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

        public void AlteraLotacaoDocente(LyLotacao lotacao, decimal numFunc, LyLotacao proximaLotacao, bool desalocaAulas)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();

            try
            {
                //verifica se tem situação "Saída da Unidade Administrativa" e coloca data fim nela
                if (rnLicencaDocente.PossuiLicencaSaidaUAAtivaPor(ctx, numFunc))
                {
                    rnLicencaDocente.DesativaLicencaSaidaUA(ctx, numFunc);
                }

                //Verifica se existe lotaçao posterior para ser alterada
                if (!proximaLotacao.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    this.Altera(ctx, proximaLotacao);
                }
                else if (desalocaAulas)
                {
                    //Substitui aulas após a data de desativaçao da lotação por carência
                    rnAulaDocente.SubstituiPorCarenciaPor(ctx, numFunc, Convert.ToDateTime(lotacao.DataDesativacao));

                    //desaloca aulas
                    rnAulaDocente.DesalocaAulas(ctx, numFunc, Convert.ToDateTime(lotacao.DataDesativacao));

                    //desaloca aulas glp
                    rnAulaDocenteTipo.DesalocaAulasTipo(ctx, numFunc, Convert.ToDateTime(lotacao.DataDesativacao));

                    //Atualiza glps usadas
                    rnDocenteGLP.AtualizaGlpUsadaPor(ctx, numFunc, Convert.ToDateTime(lotacao.DataDesativacao));
                }

                this.Altera(ctx, lotacao);
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

        public void MovimentaServidor(DadosMovimentacaoServidor dadosMovimentacaoServidor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
            string unidadeEnsinoDestino = string.Empty;
            string nucleoDestino = string.Empty;
            string unidadeFisicaDestino = string.Empty;

            try
            {
                //Busca unidade de ensino do setor de destino
                unidadeEnsinoDestino = RN.Setores.ConsultaSetorUniEns(dadosMovimentacaoServidor.SetorDestino);

                if (!string.IsNullOrEmpty(unidadeEnsinoDestino))
                {
                    //Caso o setor seja unidade de ensino, busca nucleo e unidade fisica referentes
                    nucleoDestino = RN.Setores.ConsultaNucleoUniEns(unidadeEnsinoDestino);
                    unidadeFisicaDestino = RN.Setores.ConsultaNucleoUniFis(unidadeEnsinoDestino);
                }
                else
                {
                    //Caso não seja busca nucleo por ua
                    nucleoDestino = RN.Setores.ConsultaSetorNucleo(dadosMovimentacaoServidor.SetorDestino);
                    unidadeEnsinoDestino = null;
                    unidadeFisicaDestino = null;
                }

                //Insere nova lotação com dados da atual
                this.InsereLotacaoDestino(ctx, dadosMovimentacaoServidor, unidadeFisicaDestino, unidadeEnsinoDestino, nucleoDestino);

                //Atualiza lotação atual para data fim
                this.AlteraDataDesativacao(ctx, dadosMovimentacaoServidor);

                //verifica se tem situação "Saída da Unidade Administrativa" e coloca data fim nela
                rnLicencaPessoa.FinalizaLicencaSaidaUnidadeAdministrativaPor(ctx, dadosMovimentacaoServidor.Pessoa, dadosMovimentacaoServidor.Ordem, dadosMovimentacaoServidor.Matricula);
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

        private void InsereLotacaoDestino(DataContext ctx, DadosMovimentacaoServidor dadosMovimentacaoServidor, string unidadeFisicaDestino, string unidadeEnsinoDestino, string nucleoDestino)
        {
            ContextQuery contextQuery = new ContextQuery();
            int proximaOrdem = 0;

            //Busca proxima ordem para lotação
            proximaOrdem = ObtemProximaOrdemPor(dadosMovimentacaoServidor.Matricula);

            contextQuery.Command = @" INSERT INTO LY_LOTACAO 
                                    (PESSOA, 
                                     MATRICULA, 
                                     ORDEM, 
                                     FUNCAO, 
                                     TURNO, 
                                     DATA_DESATIVACAO, 
                                     ATO_OFICIAL, 
                                     RESP_DOCUMENTACAO, 
                                     UNIDADE_FIS, 
                                     DATA_NOMEACAO, 
                                     DATA_NOMEACAO_DO, 
                                     DATA_DESATIVACAO_DO, 
                                     TIPO_DESATIVACAO, 
                                     UNIDADE_ENS, 
                                     NUCLEO, 
                                     SETOR, 
                                     READAPTADO, 
                                     DT_INICIO_READAPTACAO, 
                                     DT_FIM_READAPTACAO, 
                                     MOTIVO_READAPTACAO, 
                                     USUARIO, 
                                     DATA_ATUALIZACAO,
                                     CATEGORIA) 
                        (SELECT PESSOA, 
                                MATRICULA, 
                                @PROXIMA_ORDEM, 
                                FUNCAO, 
                                TURNO, 
                                DATA_DESATIVACAO, 
                                ATO_OFICIAL, 
                                RESP_DOCUMENTACAO, 
                                @UNIDADE_FIS, 
                                @DATA_NOMEACAO, 
                                DATA_NOMEACAO_DO, 
                                DATA_DESATIVACAO_DO, 
                                TIPO_DESATIVACAO, 
                                @UNIDADE_ENS, 
                                @NUCLEO, 
                                @SETOR, 
                                READAPTADO, 
                                DT_INICIO_READAPTACAO, 
                                DT_FIM_READAPTACAO, 
                                MOTIVO_READAPTACAO, 
                                @USUARIO, 
                                @DATA_ATUALIZACAO,
                                CATEGORIA 
                         FROM   LY_LOTACAO 
                         WHERE  PESSOA = @PESSOA 
                                AND ORDEM = @ORDEM 
                                AND MATRICULA = @MATRICULA)  ";

            contextQuery.Parameters.Add("@PROXIMA_ORDEM", proximaOrdem);
            contextQuery.Parameters.Add("@UNIDADE_FIS", unidadeFisicaDestino);
            contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsinoDestino);
            contextQuery.Parameters.Add("@NUCLEO", nucleoDestino);
            contextQuery.Parameters.Add("@SETOR", dadosMovimentacaoServidor.SetorDestino);
            contextQuery.Parameters.Add("@USUARIO", dadosMovimentacaoServidor.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATA_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@DATA_NOMEACAO", DateTime.Now);
            contextQuery.Parameters.Add("@PESSOA", dadosMovimentacaoServidor.Pessoa);
            contextQuery.Parameters.Add("@ORDEM ", dadosMovimentacaoServidor.Ordem);
            contextQuery.Parameters.Add("@MATRICULA", dadosMovimentacaoServidor.Matricula);

            ctx.ApplyModifications(contextQuery);
        }

        private void AlteraDataDesativacao(DataContext ctx, DadosMovimentacaoServidor dadosMovimentacaoServidor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_LOTACAO 
                                SET    DATA_DESATIVACAO = @DATA_DESATIVACAO, 
                                       DT_FIM_READAPTACAO = case when READAPTADO = 'S' then  @DATA_DESATIVACAO else DT_FIM_READAPTACAO end,
                                       USUARIO = @USUARIO, 
                                       DATA_ATUALIZACAO = @DATA_ATUALIZACAO 
                                WHERE  PESSOA = @PESSOA 
                                       AND ORDEM = @ORDEM 
                                       AND MATRICULA = @MATRICULA ";

            contextQuery.Parameters.Add("@DATA_DESATIVACAO", dadosMovimentacaoServidor.DataMovimentacao);
            contextQuery.Parameters.Add("@USUARIO", dadosMovimentacaoServidor.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATA_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@PESSOA", dadosMovimentacaoServidor.Pessoa);
            contextQuery.Parameters.Add("@ORDEM ", dadosMovimentacaoServidor.Ordem);
            contextQuery.Parameters.Add("@MATRICULA", dadosMovimentacaoServidor.Matricula);

            ctx.ApplyModifications(contextQuery);
        }

        public LyLotacao ObtemLotacaoAtivaPor(string matricula)
        {
            LyLotacao lotacao = new LyLotacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {
                lotacao = this.ObtemLotacaoAtivaPor(ctx, matricula);

                return lotacao;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void DesativaLotacao(DataContext ctx, DateTime dtDataDesativacao, string strUsuario, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_LOTACAO 
                                        SET    DATA_DESATIVACAO = @DATA_DESATIVACAO, 
                                               DT_FIM_READAPTACAO = case when READAPTADO = 'S' then  @DATA_DESATIVACAO else DT_FIM_READAPTACAO end,
                                               USUARIO = @USUARIO, 
                                               DATA_ATUALIZACAO = GETDATE() 
                                        WHERE  MATRICULA = @MATRICULA                         
                                               AND ((DATA_DESATIVACAO IS NULL AND CONVERT(DATE, DATA_NOMEACAO) < CONVERT(DATE, @DATA_DESATIVACAO))
                                                    OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, @DATA_DESATIVACAO))   ";

                contextQuery.Parameters.Add("@DATA_DESATIVACAO", dtDataDesativacao);
                contextQuery.Parameters.Add("@USUARIO", strUsuario);
                contextQuery.Parameters.Add("@MATRICULA", matricula);

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

        private void DesativaLotacoesAnterioresPor(DataContext ctx, string usuario, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_LOTACAO 
                                            SET    DATA_DESATIVACAO = @DATA_DESATIVACAO, 
                                                   DT_FIM_READAPTACAO = case when READAPTADO = 'S' then  @DATA_DESATIVACAO else DT_FIM_READAPTACAO end,
                                                   USUARIO = @USUARIO, 
                                                   DATA_ATUALIZACAO = @DATA_ATUALIZACAO 
                                            WHERE  MATRICULA = @MATRICULA 
                                                   AND ( DATA_DESATIVACAO IS NULL 
                                                          OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) ) 
                                                   AND DATA_NOMEACAO <= CONVERT(DATE, GETDATE())  ";

                contextQuery.Parameters.Add("@DATA_DESATIVACAO", DateTime.Now);
                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@DATA_ATUALIZACAO", DateTime.Now);
                contextQuery.Parameters.Add("@MATRICULA", matricula);

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

        public LyLotacao ObtemLotacaoAtivaPor(DataContext ctx, string matricula)
        {
            LyLotacao lotacao = new LyLotacao();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  *
                        FROM    dbo.LY_LOTACAO
                        WHERE   (DATA_DESATIVACAO IS NULL
                                            OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()))
                                AND CONVERT(DATE,DATA_NOMEACAO) <= CONVERT(DATE,GETDATE())
                                AND MATRICULA = @MATRICULA ")
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lotacao.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    lotacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                    lotacao.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    lotacao.Funcao = Convert.ToString(reader["FUNCAO"]);
                    lotacao.Turno = reader["TURNO"] != DBNull.Value ? Convert.ToString(reader["TURNO"]) : null;
                    lotacao.AtoOficial = reader["ATO_OFICIAL"] != DBNull.Value ? Convert.ToString(reader["ATO_OFICIAL"]) : null;
                    lotacao.RespDocumentacao = reader["RESP_DOCUMENTACAO"] != DBNull.Value ? Convert.ToString(reader["RESP_DOCUMENTACAO"]) : null;
                    lotacao.UnidadeFis = reader["UNIDADE_FIS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_FIS"]) : null;
                    lotacao.TipoDesativacao = reader["TIPO_DESATIVACAO"] != DBNull.Value ? Convert.ToString(reader["TIPO_DESATIVACAO"]) : null;
                    lotacao.UnidadeEns = reader["UNIDADE_ENS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_ENS"]) : null;
                    lotacao.Nucleo = reader["NUCLEO"] != DBNull.Value ? Convert.ToString(reader["NUCLEO"]) : null;
                    lotacao.Setor = Convert.ToString(reader["SETOR"]);
                    lotacao.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    lotacao.Readaptado = reader["READAPTADO"] != DBNull.Value ? Convert.ToString(reader["READAPTADO"]) : null;
                    lotacao.MotivoReadaptacao = reader["MOTIVO_READAPTACAO"] != DBNull.Value ? Convert.ToString(reader["MOTIVO_READAPTACAO"]) : null;
                    lotacao.Usuario = Convert.ToString(reader["USUARIO"]);

                    if (reader["DATA_NOMEACAO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(reader["DATA_NOMEACAO"]);
                    }

                    if (reader["DATA_NOMEACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacaoDo = Convert.ToDateTime(reader["DATA_NOMEACAO_DO"]);
                    }

                    if (reader["DATA_DESATIVACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacaoDo = Convert.ToDateTime(reader["DATA_DESATIVACAO_DO"]);
                    }

                    if (reader["DT_INICIO_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtInicioReadaptacao = Convert.ToDateTime(reader["DT_INICIO_READAPTACAO"]);
                    }

                    if (reader["DT_FIM_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtFimReadaptacao = Convert.ToDateTime(reader["DT_FIM_READAPTACAO"]);
                    }

                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacao = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }

                    if (reader["DATA_ATUALIZACAO"] != DBNull.Value)
                    {
                        lotacao.DataAtualizacao = Convert.ToDateTime(reader["DATA_ATUALIZACAO"]);
                    }
                }

                return lotacao;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private LyLotacao ObtemLotacaoPor(DataContext ctx, decimal pessoa, string matricula, decimal ordem)
        {
            LyLotacao lotacao = new LyLotacao();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  *
                        FROM    dbo.LY_LOTACAO
                        WHERE   MATRICULA = @MATRICULA 
                                AND PESSOA = @PESSOA
                                AND ORDEM = @ORDEM ")
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);
                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lotacao.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    lotacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                    lotacao.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    lotacao.Funcao = Convert.ToString(reader["FUNCAO"]);
                    lotacao.Turno = reader["TURNO"] != DBNull.Value ? Convert.ToString(reader["TURNO"]) : null;
                    lotacao.AtoOficial = reader["ATO_OFICIAL"] != DBNull.Value ? Convert.ToString(reader["ATO_OFICIAL"]) : null;
                    lotacao.RespDocumentacao = reader["RESP_DOCUMENTACAO"] != DBNull.Value ? Convert.ToString(reader["RESP_DOCUMENTACAO"]) : null;
                    lotacao.UnidadeFis = reader["UNIDADE_FIS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_FIS"]) : null;
                    lotacao.TipoDesativacao = reader["TIPO_DESATIVACAO"] != DBNull.Value ? Convert.ToString(reader["TIPO_DESATIVACAO"]) : null;
                    lotacao.UnidadeEns = reader["UNIDADE_ENS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_ENS"]) : null;
                    lotacao.Nucleo = reader["NUCLEO"] != DBNull.Value ? Convert.ToString(reader["NUCLEO"]) : null;
                    lotacao.Setor = Convert.ToString(reader["SETOR"]);
                    lotacao.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    lotacao.Readaptado = reader["READAPTADO"] != DBNull.Value ? Convert.ToString(reader["READAPTADO"]) : null;
                    lotacao.MotivoReadaptacao = reader["MOTIVO_READAPTACAO"] != DBNull.Value ? Convert.ToString(reader["MOTIVO_READAPTACAO"]) : null;
                    lotacao.Usuario = Convert.ToString(reader["USUARIO"]);

                    if (reader["DATA_NOMEACAO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(reader["DATA_NOMEACAO"]);
                    }

                    if (reader["DATA_NOMEACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacaoDo = Convert.ToDateTime(reader["DATA_NOMEACAO_DO"]);
                    }

                    if (reader["DATA_DESATIVACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacaoDo = Convert.ToDateTime(reader["DATA_DESATIVACAO_DO"]);
                    }

                    if (reader["DT_INICIO_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtInicioReadaptacao = Convert.ToDateTime(reader["DT_INICIO_READAPTACAO"]);
                    }

                    if (reader["DT_FIM_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtFimReadaptacao = Convert.ToDateTime(reader["DT_FIM_READAPTACAO"]);
                    }

                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacao = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }

                    if (reader["DATA_ATUALIZACAO"] != DBNull.Value)
                    {
                        lotacao.DataAtualizacao = Convert.ToDateTime(reader["DATA_ATUALIZACAO"]);
                    }
                }

                return lotacao;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private List<string> ValidaCamposObrigatorios(LyLotacao lotacao)
        {
            List<string> mensagens = new List<string>();
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dataMinima = new DateTime(1899, 12, 31);

            if (lotacao.Pessoa <= 0)
            {
                mensagens.Add("Favor informar a PESSOA.");
            }

            if (lotacao.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o MATRÍCULA.");
            }

            if (lotacao.Funcao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a FUNÇÃO.");
            }
            else
            {
                if (lotacao.Ordem <= 0)
                {
                    mensagens.Add("Favor informar a ORDEM.");
                }
            }

            if (lotacao.Setor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a UNIDADE ADMINISTRATIVA.");
            }

            if (lotacao.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a CATEGORIA.");
            }

            if (lotacao.Usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar o USUÁRIO.");
            }

            if (lotacao.Readaptado.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a READAPTAÇÃO.");
            }
            else
            {
                if (lotacao.Readaptado != "S" && lotacao.Readaptado != "N")
                {
                    mensagens.Add("READAPTAÇÃO inválida.");
                }

                if (lotacao.Readaptado == "S")
                {
                    if (lotacao.DataDesativacao == null || lotacao.DataDesativacao == DateTime.MinValue)
                    {
                        mensagens.Add("Para readaptação a DATA DA DISPENSA deve ser informada.");
                    }
                }
            }

            //Validação padrão de datas

            if (lotacao.DataNomeacao == DateTime.MinValue)
            {
                mensagens.Add("Favor informar a DATA DE NOMEAÇÃO.");
            }
            else
            {
                if (lotacao.DataNomeacao < dataMinima)
                {
                    mensagens.Add("DATA DA NOMEAÇÃO não pode ser menor que 1900.");
                }
            }

            if (lotacao.DataDesativacao != null && lotacao.DataDesativacao != DateTime.MinValue)
            {
                if (lotacao.DataDesativacao < dataMinima)
                {
                    mensagens.Add("DATA DA DISPENSA não pode ser menor que 1900.");
                }
            }

            if ((lotacao.DataNomeacao != null && lotacao.DataNomeacao != DateTime.MinValue)
                && (lotacao.DataDesativacao != null && lotacao.DataDesativacao != DateTime.MinValue))
            {
                if (Convert.ToDateTime(lotacao.DataDesativacao).Date <= Convert.ToDateTime(lotacao.DataNomeacao).Date)
                {
                    mensagens.Add("DATA DA DISPENSA não pode ser menor ou igual a DATA DA NOMEAÇÃO.");
                }
            }

            if (lotacao.DataNomeacaoDo != null && lotacao.DataNomeacaoDo != DateTime.MinValue)
            {
                if (lotacao.DataNomeacaoDo < dataMinima)
                {
                    mensagens.Add("DATA DA PUBLICAÇÃO DA NOMEAÇÃO não pode ser menor que 1900.");
                }
            }

            if (lotacao.DataDesativacaoDo != null && lotacao.DataDesativacaoDo != DateTime.MinValue)
            {
                if (lotacao.DataDesativacaoDo < dataMinima)
                {
                    mensagens.Add("DATA DA PUBLICAÇÃO DA DISPENSA não pode ser menor que 1900.");
                }
            }

            if ((lotacao.DataNomeacaoDo != null && lotacao.DataNomeacaoDo != DateTime.MinValue)
                && (lotacao.DataDesativacaoDo != null && lotacao.DataDesativacaoDo != DateTime.MinValue))
            {
                if (Convert.ToDateTime(lotacao.DataDesativacaoDo).Date <= Convert.ToDateTime(lotacao.DataNomeacaoDo).Date)
                {
                    mensagens.Add("DATA DA PUBLICAÇÃO DA DISPENSA não pode ser menor ou igual a DATA DA PUBLICAÇÃO DA NOMEAÇÃO.");
                }
            }

            return mensagens;
        }

        private List<string> ValidaIntercalacaoDatas(DataContext contexto, LyLotacao lotacao)
        {
            List<string> mensagens = new List<string>();

            if (this.PossuiDataNomeacaoEmOutroIntervaloLotacaoPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, lotacao.Ordem))
            {
                mensagens.Add("DATA DE NOMEAÇÃO não pode estar dentro do intervalo de outra lotação.");
            }

            if (lotacao.DataDesativacao != null)
            {
                if (this.PossuiDataDesativacaoEmOutroIntervaloLotacaoPor(contexto, lotacao.Matricula, Convert.ToDateTime(lotacao.DataDesativacao), lotacao.Ordem))
                {
                    mensagens.Add("DATA DE DISPENSA não pode estar dentro do intervalo de outra lotação.");
                }

                if (this.PossuiOutraLotacaoIntercaladaPor(contexto, lotacao.Matricula, lotacao.DataNomeacao, Convert.ToDateTime(lotacao.DataDesativacao), lotacao.Ordem))
                {
                    mensagens.Add("DATA DE NOMEAÇÃO e dispensa não podem intercalar com outra lotação.");
                }
            }

            return mensagens;
        }

        private LyLotacao ObtemUltimaLotacaoPor(DataContext ctx, string matricula)
        {
            LyLotacao lotacao = new LyLotacao();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT TOP 1 L.* 
                                FROM   LY_LOTACAO L (NOLOCK)
                                       INNER JOIN (SELECT MAX(DATA_NOMEACAO) AS DATA_NOMEACAO, 
                                                          MATRICULA
                                                   FROM   LY_LOTACAO (NOLOCK) 
                                                   WHERE  MATRICULA = @MATRICULA
                                                   GROUP  BY MATRICULA) AS T 
                                               ON T.DATA_NOMEACAO = L.DATA_NOMEACAO 
                                                  AND T.MATRICULA = L.MATRICULA 
                                WHERE  L.MATRICULA = @MATRICULA
                                ORDER BY ORDEM DESC ")
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lotacao.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    lotacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                    lotacao.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    lotacao.Funcao = Convert.ToString(reader["FUNCAO"]);
                    lotacao.Turno = reader["TURNO"] != DBNull.Value ? Convert.ToString(reader["TURNO"]) : null;
                    lotacao.AtoOficial = reader["ATO_OFICIAL"] != DBNull.Value ? Convert.ToString(reader["ATO_OFICIAL"]) : null;
                    lotacao.RespDocumentacao = reader["RESP_DOCUMENTACAO"] != DBNull.Value ? Convert.ToString(reader["RESP_DOCUMENTACAO"]) : null;
                    lotacao.UnidadeFis = reader["UNIDADE_FIS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_FIS"]) : null;
                    lotacao.TipoDesativacao = reader["TIPO_DESATIVACAO"] != DBNull.Value ? Convert.ToString(reader["TIPO_DESATIVACAO"]) : null;
                    lotacao.UnidadeEns = reader["UNIDADE_ENS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_ENS"]) : null;
                    lotacao.Nucleo = reader["NUCLEO"] != DBNull.Value ? Convert.ToString(reader["NUCLEO"]) : null;
                    lotacao.Setor = Convert.ToString(reader["SETOR"]);
                    lotacao.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    lotacao.Readaptado = reader["READAPTADO"] != DBNull.Value ? Convert.ToString(reader["READAPTADO"]) : null;
                    lotacao.MotivoReadaptacao = reader["MOTIVO_READAPTACAO"] != DBNull.Value ? Convert.ToString(reader["MOTIVO_READAPTACAO"]) : null;
                    lotacao.Usuario = Convert.ToString(reader["USUARIO"]);

                    if (reader["DATA_NOMEACAO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(reader["DATA_NOMEACAO"]);
                    }

                    if (reader["DATA_NOMEACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacaoDo = Convert.ToDateTime(reader["DATA_NOMEACAO_DO"]);
                    }

                    if (reader["DATA_DESATIVACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacaoDo = Convert.ToDateTime(reader["DATA_DESATIVACAO_DO"]);
                    }

                    if (reader["DT_INICIO_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtInicioReadaptacao = Convert.ToDateTime(reader["DT_INICIO_READAPTACAO"]);
                    }

                    if (reader["DT_FIM_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtFimReadaptacao = Convert.ToDateTime(reader["DT_FIM_READAPTACAO"]);
                    }

                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacao = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }

                    if (reader["DATA_ATUALIZACAO"] != DBNull.Value)
                    {
                        lotacao.DataAtualizacao = Convert.ToDateTime(reader["DATA_ATUALIZACAO"]);
                    }
                }

                return lotacao;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private LyLotacao ObtemProximaLotacaoPor(DataContext ctx, string matricula, DateTime dataNomeacao)
        {
            LyLotacao lotacao = new LyLotacao();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT L.* 
                                FROM   LY_LOTACAO L (NOLOCK)
                                WHERE  L.MATRICULA = @MATRICULA
	                                    AND DATA_NOMEACAO = (SELECT MIN(DATA_NOMEACAO) AS DATA_NOMEACAO
                                                FROM   LY_LOTACAO (NOLOCK) 
                                                WHERE  MATRICULA = @MATRICULA 
					                                AND DATA_NOMEACAO > @DATA_NOMEACAO
                                                GROUP  BY MATRICULA) ")
                };

                contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_ALFALARGE, matricula);
                contextQuery.Parameters.Add("@DATA_NOMEACAO", TechneDbType.T_DATA, dataNomeacao);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lotacao.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    lotacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                    lotacao.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    lotacao.Funcao = Convert.ToString(reader["FUNCAO"]);
                    lotacao.Turno = reader["TURNO"] != DBNull.Value ? Convert.ToString(reader["TURNO"]) : null;
                    lotacao.AtoOficial = reader["ATO_OFICIAL"] != DBNull.Value ? Convert.ToString(reader["ATO_OFICIAL"]) : null;
                    lotacao.RespDocumentacao = reader["RESP_DOCUMENTACAO"] != DBNull.Value ? Convert.ToString(reader["RESP_DOCUMENTACAO"]) : null;
                    lotacao.UnidadeFis = reader["UNIDADE_FIS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_FIS"]) : null;
                    lotacao.TipoDesativacao = reader["TIPO_DESATIVACAO"] != DBNull.Value ? Convert.ToString(reader["TIPO_DESATIVACAO"]) : null;
                    lotacao.UnidadeEns = reader["UNIDADE_ENS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_ENS"]) : null;
                    lotacao.Nucleo = reader["NUCLEO"] != DBNull.Value ? Convert.ToString(reader["NUCLEO"]) : null;
                    lotacao.Setor = Convert.ToString(reader["SETOR"]);
                    lotacao.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    lotacao.Readaptado = reader["READAPTADO"] != DBNull.Value ? Convert.ToString(reader["READAPTADO"]) : null;
                    lotacao.MotivoReadaptacao = reader["MOTIVO_READAPTACAO"] != DBNull.Value ? Convert.ToString(reader["MOTIVO_READAPTACAO"]) : null;
                    lotacao.Usuario = Convert.ToString(reader["USUARIO"]);

                    if (reader["DATA_NOMEACAO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(reader["DATA_NOMEACAO"]);
                    }

                    if (reader["DATA_NOMEACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacaoDo = Convert.ToDateTime(reader["DATA_NOMEACAO_DO"]);
                    }

                    if (reader["DATA_DESATIVACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacaoDo = Convert.ToDateTime(reader["DATA_DESATIVACAO_DO"]);
                    }

                    if (reader["DT_INICIO_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtInicioReadaptacao = Convert.ToDateTime(reader["DT_INICIO_READAPTACAO"]);
                    }

                    if (reader["DT_FIM_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtFimReadaptacao = Convert.ToDateTime(reader["DT_FIM_READAPTACAO"]);
                    }

                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacao = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }

                    if (reader["DATA_ATUALIZACAO"] != DBNull.Value)
                    {
                        lotacao.DataAtualizacao = Convert.ToDateTime(reader["DATA_ATUALIZACAO"]);
                    }
                }

                return lotacao;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiLotacaoPor(string matricula, decimal pessoa, decimal ordem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_LOTACAO
                                        WHERE MATRICULA = @MATRICULA
	                                        AND PESSOA = @PESSOA 
                                            AND ORDEM = @ORDEM";

                contextQuery.Parameters.Add("@MATRICULA", matricula);
                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public string[] ObtemUnidadeDiretorPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string[] retorno = new string[4];
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT LO.UNIDADE_ENS, UE.NOME_COMP, UE.NOME_COMP, UE.MUNICIPIO, UE.ID_REGIONAL
                                        FROM   LY_LOTACAO LO
                                        INNER JOIN	LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = LO.UNIDADE_ENS
                                        WHERE  LO.MATRICULA = @MATRICULA
                                               AND FUNCAO = 14
                                               AND ( LO.DATA_DESATIVACAO IS NULL
                                                      OR CONVERT(DATE, LO.DATA_DESATIVACAO) > CONVERT(DATE, Getdate()) ) ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                dt = contexto.GetDataTable(contextQuery);

                foreach (DataRow row in dt.Rows)
                {
                    retorno[0] = Convert.ToString(row["UNIDADE_ENS"]);
                    retorno[1] = Convert.ToString(row["NOME_COMP"]);
                    retorno[2] = Convert.ToString(row["MUNICIPIO"]);
                    retorno[3] = Convert.ToString(row["ID_REGIONAL"]);
                }
                return retorno;

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

        public string[] ObtemUnidadeAdjuntoSecretarioPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string[] retorno = new string[4];
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT LO.UNIDADE_ENS, UE.NOME_COMP, UE.MUNICIPIO, UE.ID_REGIONAL
                                        FROM   LY_LOTACAO LO
                                        INNER JOIN	LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = LO.UNIDADE_ENS
                                        WHERE  LO.MATRICULA = @MATRICULA
                                               AND FUNCAO in (13, 26) --13	DIRETOR ADJUNTO / 26	SECRETÁRIO
                                               AND ( LO.DATA_DESATIVACAO IS NULL
                                                      OR CONVERT(DATE, LO.DATA_DESATIVACAO) > CONVERT(DATE, Getdate()) ) ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                dt = contexto.GetDataTable(contextQuery);

                foreach (DataRow row in dt.Rows)
                {
                    retorno[0] = Convert.ToString(row["UNIDADE_ENS"]);
                    retorno[1] = Convert.ToString(row["NOME_COMP"]);
                    retorno[2] = Convert.ToString(row["MUNICIPIO"]);
                    retorno[3] = Convert.ToString(row["ID_REGIONAL"]);
                }
                return retorno;

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


        public string ObtemSegundaMatriculaAtivaPor(DataContext contexto, string matricula, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 MATRICULA AS SEGUNDAMATRICULA 
                                    FROM  LY_LOTACAO L (NOLOCK)                                                    
                                    WHERE  PESSOA = @PESSOA 
                                           AND MATRICULA <> @MATRICULA
                                           AND ( L.DATA_DESATIVACAO IS NULL 
                                                  OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) )  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void DesativaLotacaoLicencaDefinitiva(DataContext ctx, DateTime dtDataDesativacao, string strUsuario, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_LOTACAO 
                                        SET    DATA_DESATIVACAO = @DATA_DESATIVACAO, 
                                               DT_FIM_READAPTACAO = case when READAPTADO = 'S' then  @DATA_DESATIVACAO else DT_FIM_READAPTACAO end,
                                               USUARIO = @USUARIO, 
                                               DATA_ATUALIZACAO = GETDATE() 
                                        WHERE  MATRICULA = @MATRICULA                         
                                               AND (DATA_DESATIVACAO IS NULL 
                                                    OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, @DATA_DESATIVACAO))   ";

                contextQuery.Parameters.Add("@DATA_DESATIVACAO", dtDataDesativacao);
                contextQuery.Parameters.Add("@USUARIO", strUsuario);
                contextQuery.Parameters.Add("@MATRICULA", matricula);

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

        public string ObtemNumFuncSegundaMatriculaAtivaPor(DataContext contexto, string matricula, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 v.NUM_FUNC AS NUM_FUNCSEGUNDAMATRICULA 
                                    FROM  LY_LOTACAO L (NOLOCK)    
                                    inner join VW_FUNCIONARIOS v on v.MATRICULA=l.MATRICULA                                               
                                    WHERE  l.PESSOA = @PESSOA 
                                           AND l.MATRICULA <> @MATRICULA
                                           AND ( L.DATA_DESATIVACAO IS NULL 
                                                  OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) )  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void AlteraCategoria(DataContext ctx, string cargo, string strUsuario, string strMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_LOTACAO
										SET CATEGORIA = @CATEGORIA,
                                        USUARIO = @USUARIO, 
                                        DATA_ATUALIZACAO = GETDATE()
										WHERE MATRICULA = @MATRICULA 
                                            AND (DATA_DESATIVACAO IS NULL
                                                OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE())) ";

                contextQuery.Parameters.Add("@CATEGORIA", cargo);
                contextQuery.Parameters.Add("@USUARIO", strUsuario);
                contextQuery.Parameters.Add("@MATRICULA", strMatricula);

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


        private void EnviaEmailLotacaoOE_CP(string IdVinculo, string nome, string funcao, string regional, int totalGLP, string usuario)
        {
            RN.DTOs.DadosEmail email = new Techne.Lyceum.RN.DTOs.DadosEmail();
            email.Remetente = System.Configuration.ConfigurationManager.AppSettings["EmailComunica"].ToString();
            email.Login = System.Configuration.ConfigurationManager.AppSettings["EmailComunica_Login"].ToString();
            email.Senha = System.Configuration.ConfigurationManager.AppSettings["EmailComunica_Senha"].ToString();
            email.Assunto = "Lotação Função CP e OE";
            email.Texto = "<p>" +
                                "Prezado(a) , " +
                                " <br />" +
                                " <br />" +
                                " O(A) professor(a) " + nome + " , com ID/Vínculo " + IdVinculo + ", foi designado para fazer parte da equipe " +
                                " pedagógica em uma unidade escolar no âmbito da Regional " + regional + ". " +
                                " Como resultado, os " + totalGLP.ToString() + " tempos de GLP foram excluídos nesta data. " +

                                " <br />" +
                                " <br />" +
                                " Atenciosamente," +
                                " <br />" +
                                " <br />" +
                            "</p>";

            email.Destinatario = System.Configuration.ConfigurationManager.AppSettings["EmailComunica_Lotacao"].ToString();

            try
            {
                RN.Util.Email.Envia(email);
            }
            catch (Exception)
            {
                ArquivaEmailNaoEnviado(email, usuario);
            }
        }

        protected void ArquivaEmailNaoEnviado(RN.DTOs.DadosEmail email, string usuario)
        {
            RN.RecursosHumanos.Entidades.EmailNaoEnviado emailNaoEnviado = new RN.RecursosHumanos.Entidades.EmailNaoEnviado();
            RN.RecursosHumanos.EmailNaoEnviado rnRecursosHumanos = new Techne.Lyceum.RN.RecursosHumanos.EmailNaoEnviado();

            try
            {
                emailNaoEnviado.Projeto = "Lotação";
                emailNaoEnviado.Remetente = email.Remetente;
                emailNaoEnviado.Destinatario = email.Destinatario;
                emailNaoEnviado.Assunto = email.Assunto;
                emailNaoEnviado.Texto = email.Texto;
                emailNaoEnviado.UsuarioId = usuario;

                rnRecursosHumanos.Insere(emailNaoEnviado);
            }
            catch (Exception ex)
            {
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
        }

        public void DesativaLotacaoMigracao(DataContext ctx, DateTime dtDataDesativacao, string strUsuario, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_LOTACAO 
                                        SET    DATA_DESATIVACAO = CASE WHEN CONVERT(DATE, DATA_NOMEACAO) > CONVERT(DATE, @DATA_DESATIVACAO) THEN CONVERT(DATE, DATA_NOMEACAO) 
                                                                      ELSE CONVERT(DATE, @DATA_DESATIVACAO) END, 
                                               DT_FIM_READAPTACAO = case when READAPTADO = 'S' then  @DATA_DESATIVACAO else DT_FIM_READAPTACAO end,
                                               USUARIO = @USUARIO, 
                                               DATA_ATUALIZACAO = GETDATE() 
                                        WHERE  MATRICULA = @MATRICULA                         
                                               AND (DATA_DESATIVACAO IS NULL OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, @DATA_DESATIVACAO)) ";

                contextQuery.Parameters.Add("@DATA_DESATIVACAO", dtDataDesativacao);
                contextQuery.Parameters.Add("@USUARIO", strUsuario);
                contextQuery.Parameters.Add("@MATRICULA", matricula);

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

        private LyLotacao ObtemProximaLotacaoPor(DataContext ctx, string matricula, DateTime dataNomeacao, decimal ordem)
        {
            LyLotacao lotacao = new LyLotacao();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT L.* 
                                FROM   LY_LOTACAO L (NOLOCK)
                                WHERE  L.MATRICULA = @MATRICULA                                       
	                                    AND DATA_NOMEACAO = (SELECT MIN(DATA_NOMEACAO) AS DATA_NOMEACAO
                                                FROM   LY_LOTACAO (NOLOCK) 
                                                WHERE  MATRICULA = @MATRICULA 
					                                AND DATA_NOMEACAO > @DATA_NOMEACAO
                                                    AND ORDEM <> @ORDEM
                                                GROUP  BY MATRICULA) ")
                };

                contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_ALFALARGE, matricula);
                contextQuery.Parameters.Add("@DATA_NOMEACAO", TechneDbType.T_DATA, dataNomeacao);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lotacao.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    lotacao.Matricula = Convert.ToString(reader["MATRICULA"]);
                    lotacao.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    lotacao.Funcao = Convert.ToString(reader["FUNCAO"]);
                    lotacao.Turno = reader["TURNO"] != DBNull.Value ? Convert.ToString(reader["TURNO"]) : null;
                    lotacao.AtoOficial = reader["ATO_OFICIAL"] != DBNull.Value ? Convert.ToString(reader["ATO_OFICIAL"]) : null;
                    lotacao.RespDocumentacao = reader["RESP_DOCUMENTACAO"] != DBNull.Value ? Convert.ToString(reader["RESP_DOCUMENTACAO"]) : null;
                    lotacao.UnidadeFis = reader["UNIDADE_FIS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_FIS"]) : null;
                    lotacao.TipoDesativacao = reader["TIPO_DESATIVACAO"] != DBNull.Value ? Convert.ToString(reader["TIPO_DESATIVACAO"]) : null;
                    lotacao.UnidadeEns = reader["UNIDADE_ENS"] != DBNull.Value ? Convert.ToString(reader["UNIDADE_ENS"]) : null;
                    lotacao.Nucleo = reader["NUCLEO"] != DBNull.Value ? Convert.ToString(reader["NUCLEO"]) : null;
                    lotacao.Setor = Convert.ToString(reader["SETOR"]);
                    lotacao.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    lotacao.Readaptado = reader["READAPTADO"] != DBNull.Value ? Convert.ToString(reader["READAPTADO"]) : null;
                    lotacao.MotivoReadaptacao = reader["MOTIVO_READAPTACAO"] != DBNull.Value ? Convert.ToString(reader["MOTIVO_READAPTACAO"]) : null;
                    lotacao.Usuario = Convert.ToString(reader["USUARIO"]);

                    if (reader["DATA_NOMEACAO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(reader["DATA_NOMEACAO"]);
                    }

                    if (reader["DATA_NOMEACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataNomeacaoDo = Convert.ToDateTime(reader["DATA_NOMEACAO_DO"]);
                    }

                    if (reader["DATA_DESATIVACAO_DO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacaoDo = Convert.ToDateTime(reader["DATA_DESATIVACAO_DO"]);
                    }

                    if (reader["DT_INICIO_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtInicioReadaptacao = Convert.ToDateTime(reader["DT_INICIO_READAPTACAO"]);
                    }

                    if (reader["DT_FIM_READAPTACAO"] != DBNull.Value)
                    {
                        lotacao.DtFimReadaptacao = Convert.ToDateTime(reader["DT_FIM_READAPTACAO"]);
                    }

                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        lotacao.DataDesativacao = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }

                    if (reader["DATA_ATUALIZACAO"] != DBNull.Value)
                    {
                        lotacao.DataAtualizacao = Convert.ToDateTime(reader["DATA_ATUALIZACAO"]);
                    }
                }

                return lotacao;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}