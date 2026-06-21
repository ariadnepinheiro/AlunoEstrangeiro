using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using System.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Library;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class LicencaDocente : RNBase
    {
        public static QueryTable ConsultarLicencas(int num_func)
        {
            string sql = string.Empty;
            sql = @"SELECT d.num_func, d.matricula,
                    ld.motivo,
                    ld.dtini,
                    ld.dtfim,
                    l.DESCRICAO as 'MOTIVO_LICENCA' 
                FROM 
                    LY_PESSOA p
                    inner join ly_docente d on d.PESSOA = p.PESSOA
                    inner join ly_licenca_docente ld on ld.NUM_FUNC = d.num_func
                    inner join LY_LICENCAS l on l.MOTIVO = ld.MOTIVO  
                   	where d.NUM_FUNC = ?
                    AND (dtfim is null or convert(date,dtfim) >= convert(date,GETDATE()))
                    order by d.matricula, ld.DTINI desc";
            return Consultar(sql, num_func);
        }

        public DataTable ListaPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT D.NUM_FUNC, 
                                       D.MATRICULA, 
                                       D.MATRICULA AS MATRICULADESC, 
									   P.IDFUNCIONAL,
									   D.VINCULO,
									   ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,D.VINCULO)), D.MATRICULA) IDVINCULO,
                                       LD.MOTIVO, 
                                       CONVERT(VARCHAR(10), LD.DTINI, 103) AS DTINI, 
                                       CONVERT(VARCHAR(10), LD.DTFIM, 103) AS DTFIM 
                                FROM   LY_PESSOA P (NOLOCK)
                                       INNER JOIN LY_DOCENTE D (NOLOCK)
                                               ON D.PESSOA = P.PESSOA 
                                       INNER JOIN LY_LICENCA_DOCENTE LD (NOLOCK)
                                               ON LD.NUM_FUNC = D.NUM_FUNC 
                                WHERE  P.PESSOA = @PESSOA 
                                ORDER  BY D.MATRICULA, 
                                          LD.DTINI DESC ";

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

        public LyLicencaDocente ObtemLicencaAtivaPor(decimal numFunc)
        {
            Entidades.LyLicencaDocente licenca = new Entidades.LyLicencaDocente();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 LD.NUM_FUNC, 
                                                 LD.DTINI, 
                                                 LD.DTFIM, 
                                                 LD.MOTIVO, 
                                                 LD.DT_RETORNO, 
                                                 LD.STAMP_ATUALIZACAO 
                                    FROM   LY_LICENCA_DOCENTE (NOLOCK) LD 
                                           JOIN LY_LICENCAS L (NOLOCK) 
                                             ON L.MOTIVO = LD.MOTIVO 
                                    WHERE  ISNULL(LD.DTFIM, '') >= 
			                                    ( CASE L.POSSUI_DTFIM 
                                                    WHEN 'N' THEN '' 
                                                    ELSE CONVERT(DATE, GETDATE()) 
                                                 END ) 
                                           AND LD.NUM_FUNC = @NUM_FUNC 
                                    ORDER  BY LD.DTINI DESC  ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    licenca.NumFunc = Convert.ToDecimal(reader["NUM_FUNC"]);
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

        public LyLicencaDocente ObtemLicencaAtivaPor(DataContext contexto, string matricula)
        {
            Entidades.LyLicencaDocente licenca = new Entidades.LyLicencaDocente();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 LD.NUM_FUNC, 
                                                 LD.DTINI, 
                                                 LD.DTFIM, 
                                                 LD.MOTIVO, 
                                                 LD.DT_RETORNO, 
                                                 LD.STAMP_ATUALIZACAO 
                                    FROM   LY_LICENCA_DOCENTE (NOLOCK) LD 
										   inner join LY_DOCENTE d  (NOLOCK) 
												on LD.NUM_FUNC = d.NUM_FUNC
                                           inner JOIN LY_LICENCAS L (NOLOCK) 
												ON L.MOTIVO = LD.MOTIVO 
                                    WHERE  ISNULL(LD.DTFIM, '') >= 
			                                    ( CASE L.POSSUI_DTFIM 
                                                    WHEN 'N' THEN '' 
                                                    ELSE CONVERT(DATE, GETDATE()) 
                                                 END ) 
                                           AND D.MATRICULA = @MATRICULA
                                    ORDER  BY LD.DTINI DESC  ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    licenca.NumFunc = Convert.ToDecimal(reader["NUM_FUNC"]);
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

        public DataTable ObtemListaLicencaDocentePor(int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable cursos = null;

            try
            {
                contextQuery.Command = @" SELECT  D.NUM_FUNC ,
                                D.MATRICULA ,
							    CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D.VINCULO) AS IDVINCULO,
                                L.DESCRICAO ,
                                LD.DTINI ,
                                LD.DTFIM
                        FROM    LY_DOCENTE D ( NOLOCK )
							    INNER JOIN LY_PESSOA P ( NOLOCK ) ON D.PESSOA = P.PESSOA
                                INNER JOIN LY_LICENCA_DOCENTE LD ( NOLOCK ) ON LD.NUM_FUNC = D.NUM_FUNC
                                INNER JOIN LY_LICENCAS L ( NOLOCK ) ON L.MOTIVO = LD.MOTIVO
                        WHERE   D.PESSOA = @PESSOA
                        ORDER BY D.MATRICULA ,
                                LD.DTINI DESC
                         ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                cursos = ctx.GetDataTable(contextQuery);
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

            return cursos;
        }

        public bool PossuiLicencaAtivaMotivo43(DataContext contexto, decimal numfunc)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                SELECT TOP 1 1
                FROM LY_LICENCA_DOCENTE LP (NOLOCK)
                INNER JOIN LY_LICENCAS L (NOLOCK)
                    ON L.MOTIVO = LP.MOTIVO
                WHERE LP.NUM_FUNC = @NUMFUNC
                  AND LP.MOTIVO = '43'
                  AND ISNULL(LP.DTFIM, '') >=
                        (
                            CASE L.POSSUI_DTFIM
                                WHEN 'N' THEN ''
                                ELSE CONVERT(DATE, GETDATE())
                            END
                        )";

                contextQuery.Parameters.Add("@NUMFUNC", numfunc);

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

        public bool PossuiLicencaMotivo43(DataContext contexto, decimal numfunc, DateTime dtIni)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 1
                                        FROM LY_LICENCA_DOCENTE 
                                        WHERE NUM_FUNC = @NUMFUNC
                                          AND MOTIVO = '43'
                                          AND CONVERT(DATE, DTINI) = @DTINI ";

                contextQuery.Parameters.Add("@NUMFUNC", numfunc);
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

        //verifica se funcionatio tem situação "Saída da Unidade Administrativa" ativa
        public bool PossuiLicencaSaidaUAAtivaPor(DataContext ctx, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM  LY_LICENCA_DOCENTE (NOLOCK) 
                            WHERE  MOTIVO = '2' 
                                    AND (DTFIM IS NULL OR CONVERT(DATE,DTFIM) > CONVERT(DATE,GETDATE()))
                                    AND NUM_FUNC = @NUM_FUNC ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void DesativaLicencaSaidaUA(DataContext ctx, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_DOCENTE
                                SET    DTFIM = CONVERT(DATE,GETDATE()),
	                                   STAMP_ATUALIZACAO = GETDATE()
                                 WHERE  MOTIVO = '2' 
                                    AND (DTFIM IS NULL OR CONVERT(DATE,DTFIM) > CONVERT(DATE,GETDATE()))
                                    AND NUM_FUNC = @NUM_FUNC ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

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

        public ValidacaoDados ValidaInsercao(LyLicencaDocente licencaDocente, string matricula, decimal pessoa, out bool licencaPossuiDataFim, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoDatas = new List<string>();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Licencas rnLicenca = new Licencas();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.Docentes rnDocente = new Docentes();
            DataContext contexto = null;
            int totalAulasAlocadas = 0;
            bool possuiAulasGlp;
            int numeroMatriculas = 0;
            int cargaHorariaPermitidaFuncao = 0;
            licencaPossuiDataFim = false;
            LyLotacao lotacaoAtiva = new LyLotacao();
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
          
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("Favor informar a PESSOA.");
            }

            if (licencaDocente.NumFunc <= 0)
            {
                mensagens.Add("Favor informar o NÚMERO DO DOCENTE.");
            }

            if (licencaDocente.Dtini == null || licencaDocente.Dtini == DateTime.MinValue)
            {
                mensagens.Add("Favor informar a DATA DE INÍCIO DA SITUAÇÃO.");
            }
            else
            {
                //Verifica se a data incio da situacao é maior que a data atual
                if (licencaDocente.Dtini > hoje)
                {
                    mensagens.Add("DATA INÍCIO SITUAÇÃO não pode ser maior que hoje.");
                }

                if (licencaDocente.Dtfim != null && licencaDocente.Dtfim != DateTime.MinValue)
                {
                    if (licencaDocente.Dtfim < licencaDocente.Dtini)
                    {
                        mensagens.Add("DATA DE FIM DA SITUAÇÃO não pode ser menor que DATA DE INÍCIO DA SITUAÇÃO.");
                    }
                }
            }

            if (licencaDocente.Motivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a SITUAÇÃO.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se usuario pode incluir licença                    
                    if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel) && !rnPadroesDeAcessos.PodeIncluirExcluirSituacaoPor(contexto, usuarioResponsavel, licencaDocente.Motivo))
                    {
                        mensagens.Add("Usuário não tem permissão para incluir esta situação.");
                    }

                    //Verifica se licença possui data sim
                    licencaPossuiDataFim = rnLicenca.PossuiDataFimPor(contexto, licencaDocente.Motivo);                    
                    if (licencaPossuiDataFim)
                    {
                        if (licencaDocente.Dtfim == null || licencaDocente.Dtfim == DateTime.MinValue)
                        {
                            mensagens.Add("Favor informar a DATA DE FIM DA SITUAÇÃO.");
                        }
                    }
                    else
                    {
                        if (licencaDocente.Dtfim != null && licencaDocente.Dtfim != DateTime.MinValue)
                        {
                            mensagens.Add("Para esta situação não é permitido preenchimento da DATA DE FIM.");
                        }
                    }

                    //Verifica perido maxima da licença
                    int periodo = rnLicenca.ObtemPeriodoLimitePor(contexto, licencaDocente.Motivo);
                    if (periodo > 0)
                    {
                        DateTime datafim = Convert.ToDateTime(licencaDocente.Dtfim);
                        DateTime dataini = Convert.ToDateTime(licencaDocente.Dtini);
                        if ((datafim.Subtract(dataini).Days > periodo))
                        {
                            mensagens.Add("A diferença em dias entre Data Início Situação e Data Fim Situação não pode ser maior que o período limite de " + periodo + " dias.");
                        }
                    }

                    //Verificar se situação é diferente de "RETORNANDO DE AFASTAMENTO - AGUARDANDO ALOCAÇÃO" e "AGUARDANDO PERÍCIA" - que permitem mais de uma licença ativa
                    if (licencaDocente.Motivo != "100" && licencaDocente.Motivo != "46")
                    {
                        //Verifica se situacao e diferente de "CARGA HORARIA REDUZIDA" - que permitem mais de uma licença ativa
                        if(licencaDocente.Motivo != "43")
                        {
                            if (this.ExisteLicencaAtivaPor(contexto, licencaDocente.NumFunc))
                            {
                                mensagens.Add("Já existe situação ativa para esta matricula.");
                            }
                        }                     
                    }

                    bool existeLicenca = this.ExisteLicencaPor(
                        contexto,
                        licencaDocente.Dtini,
                        licencaDocente.NumFunc
                    );

                    if (existeLicenca) 
                    {
                        mensagens.Add("Já existe uma licença cadastrada para este funcionario com esta data de início.");
                    }


                    //Verifica se possui glp, se possui não pode reduzir carga horária
                    possuiAulasGlp = rnAulaDocenteTipo.PossuiGLP(contexto, licencaDocente.NumFunc);
                    if (possuiAulasGlp && licencaDocente.Motivo == "43")
                    {
                        mensagens.Add("Não é possível reduzir a carga horária, pois o docente possui aulas GLP vinculadas.");
                    }

                    //Busca lotação 
                    lotacaoAtiva = rnLotacao.ObtemLotacaoAtivaPor(contexto, matricula);

                    //Verifica se a situação é carga horaria reduzida
                    if (licencaDocente.Motivo == "43" && lotacaoAtiva.DataNomeacao != DateTime.MinValue)
                    {
                        //busca aulas Alocadas para o Docente
                        totalAulasAlocadas = rnAulaDocente.ObtemTotalAulasAlocadasPor(contexto, licencaDocente.NumFunc, DateTime.Now);

                        //Busca quantidade de matriculas que a pessoa possui
                        numeroMatriculas = rnLotacao.ObtemNumeroMatriculaDocentePor(contexto, pessoa, Convert.ToDateTime(lotacaoAtiva.DataNomeacao));

                        //Busca categoria
                        string categoria = rnDocente.ObtemCategoriaPor(contexto, matricula);

                        //Atualiza carga Horaria do Docente
                        cargaHorariaPermitidaFuncao = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(contexto, categoria, lotacaoAtiva.Funcao);

                        //Se a quantidade de tempos alocados para o docente exceder a carga horária total configurada para
                        //a nova função do docente, não permitir o salvamento.
                        if (totalAulasAlocadas > (cargaHorariaPermitidaFuncao / 2))
                        {
                            mensagens.Add("Não é possível colocar carga horária reduzida, devido o professor ter mais tempos alocado(" + totalAulasAlocadas + ")  do que o permitido(" + (cargaHorariaPermitidaFuncao / 2) + ").");
                        }
                    }

                    if (rnLicenca.PossuiValidacaoAlocacaoPor(licencaDocente.Motivo) && licencaPossuiDataFim)
                    {
                        if (licencaDocente.Dtfim == null || licencaDocente.Dtfim == DateTime.MinValue)
                        {
                            if (rnAulaDocente.ExisteAulaAlocadaPor(contexto, licencaDocente.NumFunc, licencaDocente.Dtini))
                            {
                                mensagens.Add("Existem aulas alocadas para o docente neste período, favor desalocar aulas antes de inserir a situação.");
                            }
                        }
                        else
                        {
                            if (licencaDocente.Dtfim == null || licencaDocente.Dtfim == DateTime.MinValue)
                            {
                                if (rnAulaDocente.ExisteAulaAlocadaPeriodoLotacaoPor(contexto, licencaDocente.NumFunc, licencaDocente.Dtini, Convert.ToDateTime(licencaDocente.Dtfim)))
                                {
                                    mensagens.Add("Existem aulas alocadas para o docente neste período, favor desalocar aulas antes de inserir a situação.");
                                }
                            }
                        }
                    }

                    //Validação de datas intercaladas padrao
                    validacaoDatas = this.ValidaIntercalacaoDatas(contexto, licencaDocente);
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

        public void Insere(LyLicencaDocente licencaDocente, string matricula, bool licencaPossuiDataFim, string usuarioResponsavel)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                this.Insere(ctx, licencaDocente);

                if (!licencaPossuiDataFim)
                {
                    //Para licenças definitivas desativa a lotação atualizada
                    rnLotacao.DesativaLotacao(ctx, licencaDocente.Dtini, usuarioResponsavel, matricula);
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

        public ValidacaoDados ValidaAlteracao(LyLicencaDocente licencaDocente, string matricula, decimal pessoa, out bool licencaPossuiDataFim, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            List<string> validacaoDatas = new List<string>();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Licencas rnLicenca = new Licencas();
            RN.Docentes rnDocente = new Docentes();
            RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Lotacao rnLotacao = new Lotacao();
            licencaPossuiDataFim = false;
            int totalAulasAlocadas = 0;
            int numeroMatriculas = 0;
            int cargaHorariaPermitidaFuncao = 0;
            DataContext contexto = null;
            LyLotacao lotacaoAtiva = new LyLotacao();
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("Favor informar a PESSOA.");
            }

            if (licencaDocente.NumFunc <= 0)
            {
                mensagens.Add("Favor informar o NÚMERO DO DOCENTE.");
            }

            if (licencaDocente.Dtini == null || licencaDocente.Dtini == DateTime.MinValue)
            {
                mensagens.Add("Favor informar a DATA DE INÍCIO DA SITUAÇÃO.");
            }
            else
            {
                if (licencaDocente.Dtfim != null && licencaDocente.Dtfim != DateTime.MinValue)
                {
                    if (licencaDocente.Dtfim < licencaDocente.Dtini)
                    {
                        mensagens.Add("DATA DE FIM DA SITUAÇÃO não pode ser menor que DATA DE INÍCIO DA SITUAÇÃO.");
                    }
                }
            }            

            if (licencaDocente.Motivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Favor informar a SITUAÇÃO.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    string padaces = rnPadroesDeAcessos.ObtemPadraoAcessoLicencaPor(usuarioResponsavel, licencaDocente.Motivo);

                    //Verifica se usuario alterar data fim licença                   
                    if (!rnUsuarios.EhPrivilegiado(contexto, usuarioResponsavel)
                        && !rnPadroesDeAcessos.ExistePadraoAcessoPor(contexto, padaces, "Techne.Lyceum.Net.Basico.AlteraDataFinalLicenca", "LyceumNet"))
                    {
                        mensagens.Add("Usuário não tem permissão para alterar data fim da licença.");
                    }

                    licencaPossuiDataFim = rnLicenca.PossuiDataFimPor(contexto, licencaDocente.Motivo);
                    if (licencaPossuiDataFim)
                    {
                        if (licencaDocente.Dtfim == null || licencaDocente.Dtfim == DateTime.MinValue)
                        {
                            mensagens.Add("Favor informar a DATA DE FIM DA SITUAÇÃO.");
                        }
                    }
                    else
                    {
                        if (licencaDocente.Dtfim != null && licencaDocente.Dtfim != DateTime.MinValue)
                        {
                            mensagens.Add("Para esta situação não é permitido preenchimento da DATA DE FIM.");
                        }
                    }

                    //Verifica perido maxima da licença
                    int periodo = rnLicenca.ObtemPeriodoLimitePor(contexto, licencaDocente.Motivo);
                    if (periodo > 0 && licencaDocente.Dtfim != null)
                    {
                        DateTime datafim = Convert.ToDateTime(licencaDocente.Dtfim);
                        DateTime dataini = Convert.ToDateTime(licencaDocente.Dtini);
                        if ((datafim.Subtract(dataini).Days > periodo))
                        {
                            mensagens.Add("A diferença em dias entre Data Início Situação e Data Fim Situação não pode ser maior que o período limite de " + periodo + " dias.");
                        }
                    }

                    //Verificar se situação é diferente de "RETORNANDO DE AFASTAMENTO - AGUARDANDO ALOCAÇÃO" e "AGUARDANDO PERÍCIA" - que permitem mais de uma licença ativa
                    if (licencaDocente.Dtfim == null || licencaDocente.Dtfim == DateTime.MinValue)
                    {
                        if (licencaDocente.Motivo != "100" && licencaDocente.Motivo != "46")
                        {
                            //Verifica se situacao e diferente de "CARGA HORARIA REDUZIDA" - que permitem mais de uma licença ativa
                            if (licencaDocente.Motivo != "43")
                            {
                                if (this.ExisteOutraLicencaAtivaPor(contexto, licencaDocente.NumFunc, licencaDocente.Dtini))
                                {
                                    mensagens.Add("Já existe situação ativa para esta matricula.");
                                }
                            }
                           
                        }
                    }

                    //Busca lotação 
                    lotacaoAtiva = rnLotacao.ObtemLotacaoAtivaPor(contexto, matricula);

                    //Verifica se a situação é carga horaria reduzida
                    if (licencaDocente.Motivo == "43" && lotacaoAtiva.DataNomeacao != DateTime.MinValue)
                    {
                        //busca aulas Alocadas para o Docente
                        totalAulasAlocadas = rnAulaDocente.ObtemTotalAulasAlocadasPor(contexto, licencaDocente.NumFunc, DateTime.Now);
                       
                        //Busca quantidade de matriculas que a pessoa possui
                        numeroMatriculas = rnLotacao.ObtemNumeroMatriculaDocentePor(contexto, pessoa, Convert.ToDateTime(lotacaoAtiva.DataNomeacao));

                        //Busca categoria
                        string categoria = rnDocente.ObtemCategoriaPor(contexto, matricula);

                        //Atualiza carga Horaria do Docente
                        cargaHorariaPermitidaFuncao = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(contexto, categoria, lotacaoAtiva.Funcao);

                        //Se a quantidade de tempos alocados para o docente exceder a carga horária total configurada para
                        //a nova função do docente, não permitir o salvamento.
                        if (totalAulasAlocadas > (cargaHorariaPermitidaFuncao / 2))
                        {
                            mensagens.Add("Não é possível colocar carga horária reduzida, devido o professor ter mais tempos alocado(" + totalAulasAlocadas + ")  do que o permitido(" + (cargaHorariaPermitidaFuncao / 2) + ").");
                        }
                    }

                    //Validação de datas intercaladas padrao
                    validacaoDatas = this.ValidaIntercalacaoDatas(contexto, licencaDocente);
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

        public void Altera(LyLicencaDocente licencaDocente)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();

            try
            {
                if (licencaDocente.Motivo != "61") //GREVE
                {
                    //Verifica se a licença é definitiva
                    if (licencaDocente.Dtfim != null && licencaDocente.Dtfim != DateTime.MinValue)
                    {
                        //Substitui aulas após a data inicio da licença definitiva por carência
                        rnAulaDocente.SubstituiPorCarenciaPor(ctx, licencaDocente.NumFunc, licencaDocente.Dtini);

                        //desaloca aulas
                        rnAulaDocente.DesalocaAulas(ctx, licencaDocente.NumFunc, licencaDocente.Dtini);

                        //desaloca aulas glp
                        rnAulaDocenteTipo.DesalocaAulasTipo(ctx, licencaDocente.NumFunc, licencaDocente.Dtini);

                        //Atualiza glps usadas
                        rnDocenteGLP.AtualizaGlpUsadaPor(ctx, licencaDocente.NumFunc, licencaDocente.Dtini);
                    }
                }

                this.AlteraDataFim(ctx, licencaDocente);
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

        private List<string> ValidaIntercalacaoDatas(DataContext contexto, LyLicencaDocente licencaDocente)
        {
            List<string> mensagens = new List<string>();

            if(licencaDocente.Motivo == "43") 
            {
                return mensagens;
            }
            

            if (this.PossuiDataEmOutroIntervaloLicencaPor(contexto, licencaDocente.NumFunc, licencaDocente.Dtini, licencaDocente.Dtini))
            {
                mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outra licença.");
            }

            if (licencaDocente.Dtfim != null && licencaDocente.Dtfim != DateTime.MinValue && licencaDocente.Motivo != "43")
            {
                if (this.PossuiDataEmOutroIntervaloLicencaPor(contexto, licencaDocente.NumFunc, licencaDocente.Dtini, Convert.ToDateTime(licencaDocente.Dtfim)))
                {
                    mensagens.Add("DATA FIM não pode estar dentro do intervalo de outra licença.");
                }

                if (this.PossuiOutraLicencaIntercaladaPor(contexto, licencaDocente.NumFunc, licencaDocente.Dtini, Convert.ToDateTime(licencaDocente.Dtfim)))
                {
                    mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outra licença.");
                }
            }

            return mensagens;
        }

        private bool PossuiDataEmOutroIntervaloLicencaPor(DataContext ctx, decimal numFunc, DateTime dataInicio, DateTime dataAnalisar)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM   LY_LICENCA_DOCENTE (NOLOCK) 
                                WHERE  NUM_FUNC = @NUM_FUNC
                                        AND MOTIVO <> '43' 
                                        AND CONVERT(DATE, DTINI) <> CONVERT(DATE, @DTINI) 
                                        AND @DATA BETWEEN CONVERT(DATE, DTINI) AND CONVERT(DATE, ISNULL(DTFIM, GETDATE())) ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@DTINI", dataInicio);
            contextQuery.Parameters.Add("@DATA", dataAnalisar.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiOutraLicencaIntercaladaPor(DataContext ctx, decimal numFunc, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_LICENCA_DOCENTE  (NOLOCK) 
                                WHERE  NUM_FUNC = @NUM_FUNC 
                                    AND CONVERT(DATE, DTINI) <> CONVERT(DATE, @DATA_INICIO)
                                    AND @DATA_INICIO <= CONVERT(DATE, DTINI) 
                                    AND @DATA_FIM >= CONVERT(DATE, ISNULL(DTFIM, GETDATE()))   
                                    AND MOTIVO <> '43' ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@DATA_INICIO", dataInicio);
            contextQuery.Parameters.Add("@DATA_FIM", dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public ValidacaoDados ValidaRemocao(decimal numFunc, DateTime dataInicio, string motivo, string usuarioResponsavel)
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

            if (numFunc <= 0)
            {
                mensagens.Add("O NUMERO DO DOCENTE é obrigatória.");
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

        public void Remove(decimal numFunc, DateTime dataInicio, string motivo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_LICENCA_DOCENTE 
                                            WHERE  NUM_FUNC = @NUM_FUNC
		                                            AND CONVERT(DATE, DTINI) = CONVERT(DATE, @DTINI)
                                                    AND MOTIVO = @MOTIVO ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@DTINI", dataInicio);
                contextQuery.Parameters.Add("@MOTIVO", dataInicio);

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

        public void RemoveLicenca(decimal numFunc, DateTime dataInicio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_LICENCA_DOCENTE 
                                            WHERE  NUM_FUNC = @NUM_FUNC
		                                            AND CONVERT(DATE, DTINI) = CONVERT(DATE, @DTINI)
		                                            AND MOTIVO <> '43' ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
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

        public void Insere(DataContext ctx, RN.Entidades.LyLicencaDocente licencaDocente)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" INSERT INTO dbo.LY_LICENCA_DOCENTE
                                            (
                                                NUM_FUNC,
                                                DTINI,
                                                DTFIM,
                                                MOTIVO,
                                                DT_RETORNO,
                                                STAMP_ATUALIZACAO
                                            )
                                            VALUES
                                            (
                                                @NUM_FUNC,
                                                @DTINI,
                                                @DTFIM,
                                                @MOTIVO,
                                                @DT_RETORNO,
                                                @STAMP_ATUALIZACAO
                                            );";

                contextQuery.Parameters.Add("@NUM_FUNC", licencaDocente.NumFunc);
                contextQuery.Parameters.Add("@DTINI", licencaDocente.Dtini);
                contextQuery.Parameters.Add("@MOTIVO", licencaDocente.Motivo);
                contextQuery.Parameters.Add("@DTFIM", licencaDocente.Dtfim);
                contextQuery.Parameters.Add("@DT_RETORNO", licencaDocente.DtRetorno);
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

        public void AlteraDataFim(DataContext ctx, Entidades.LyLicencaDocente licencaDocente)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_DOCENTE 
                            SET    DTFIM = @DTFIM, 
                                   STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                            WHERE  NUM_FUNC = @NUM_FUNC 
                                   AND DTINI = @DTINI ";

                contextQuery.Parameters.Add("@NUM_FUNC", licencaDocente.NumFunc);
                contextQuery.Parameters.Add("@DTINI", licencaDocente.Dtini);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

                if (licencaDocente.Dtfim != DateTime.MinValue && licencaDocente.Dtfim != null)
                {
                    contextQuery.Parameters.Add("@DTFIM", licencaDocente.Dtfim);
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

        public bool ExisteLicencaAtivaPor(DataContext ctx, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LICENCA_DOCENTE (NOLOCK)
                                    WHERE (DTFIM IS NULL OR CONVERT(DATE,DTFIM) >= CONVERT(DATE,GETDATE()))
                                        AND NUM_FUNC = @NUM_FUNC 
                                        AND MOTIVO <> 43";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool ExisteOutraLicencaAtivaPor(DataContext ctx, decimal numFunc, DateTime dataInicio)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LICENCA_DOCENTE (NOLOCK)
                                    WHERE (DTFIM IS NULL OR CONVERT(DATE,DTFIM) >= CONVERT(DATE,GETDATE()))
                                        AND NUM_FUNC = @NUM_FUNC
										AND CONVERT(DATE, @DTINI) <> CONVERT(DATE, DTINI)  ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@DTINI", dataInicio);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool ExisteLicencaPor(DataContext ctx, DateTime datainicio, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   LY_LICENCA_DOCENTE (NOLOCK)
                                    WHERE CONVERT(DATE, DTINI) = @DATAINICIO
                                         AND NUM_FUNC = @NUM_FUNC ";

            contextQuery.Parameters.Add("@DATAINICIO", datainicio.Date);
            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public ValidacaoDados ValidaRemocaoLicencaAtiva(decimal numFunc, DateTime dataInicio, string usuarioResponsavel)
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
                if (numFunc <= 0)
                {
                    mensagens.Add("O NUMERO DO DOCENTE é obrigatória.");
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
                    string motivo = this.ObtemMotivoLicencaAtivaPor(contexto, numFunc);

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

        public string ObtemMotivoLicencaAtivaPor(DataContext contexto, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 MOTIVO 
                                        FROM   LY_LICENCA_DOCENTE 
                                        WHERE  NUM_FUNC = @NUM_FUNC 
                                               AND ( DTFIM IS NULL 
                                                      OR CONVERT(DATE, DTFIM) >= CONVERT(DATE, GETDATE()) ) 
                                        ORDER  BY DTFIM DESC ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public bool PossuiLicencaAnteriorComAltaPor(DataContext ctx, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_LICENCA_DOCENTE  (NOLOCK) 
                             WHERE  NUM_FUNC = @NUM_FUNC 
                                    AND MOTIVO IN ('13','15','17')
                                    AND DTFIM = (SELECT MAX(DTFIM) FROM LY_LICENCA_DOCENTE WHERE  NUM_FUNC = @NUM_FUNC )
                           ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }


        public bool PossuiCargaHorariaReduzida(int numFunc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                return this.PossuiCargaHorariaReduzida(ctx, numFunc);
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

        private bool PossuiCargaHorariaReduzida(DataContext ctx, int numFunc)
        {
            bool possui = false;
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*)
                               FROM   LY_LICENCA_DOCENTE (NOLOCK) LD 
                                           JOIN LY_LICENCAS L (NOLOCK) 
                                             ON L.MOTIVO = LD.MOTIVO 
                                    WHERE  ISNULL(LD.DTFIM, '') >= 
			                                    ( CASE L.POSSUI_DTFIM 
                                                    WHEN 'N' THEN '' 
                                                    ELSE CONVERT(DATE, GETDATE()) 
                                                 END ) 
                                           AND LD.NUM_FUNC = @NUMFUNC 
                                           AND L.MOTIVO = '43' ";

            contextQuery.Parameters.Add("@NUMFUNC", numFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void AlteraReducaoCHAtiva(DataContext ctx, Entidades.LyLicencaDocente licencaDocente)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_DOCENTE 
                            SET    DTFIM = @DTFIM,                                    
                                   STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                            WHERE NUM_FUNC = @NUMFUNC
                                  AND MOTIVO = '43'
                                  AND DTFIM >= GETDATE()
                                       ";

                contextQuery.Parameters.Add("@NUMFUNC", licencaDocente.NumFunc);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

                if (licencaDocente.Dtfim != DateTime.MinValue && licencaDocente.Dtfim != null)
                {
                    contextQuery.Parameters.Add("@DTFIM", licencaDocente.Dtfim);
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

        public void AlteraReducaoCH(DataContext ctx, Entidades.LyLicencaDocente licencaDocente)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" UPDATE LY_LICENCA_DOCENTE 
                            SET    DTFIM = @DTFIM,                                    
                                   STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                            WHERE NUM_FUNC = @NUMFUNC
                                  AND MOTIVO = '43'
                                  AND CONVERT(DATE, DTINI) = @DTINI ";

                contextQuery.Parameters.Add("@NUMFUNC", licencaDocente.NumFunc);
                contextQuery.Parameters.Add("@DTINI", licencaDocente.Dtini.Date);

                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

                if (licencaDocente.Dtfim != DateTime.MinValue && licencaDocente.Dtfim != null)
                {
                    contextQuery.Parameters.Add("@DTFIM", licencaDocente.Dtfim);
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

        public void RemoveReducaoCHAtiva(DataContext ctx, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command = @" delete LY_LICENCA_DOCENTE 
                            WHERE NUM_FUNC = @NUMFUNC
                                  AND MOTIVO = '43'
                                  AND DTFIM >= GETDATE() ";

                contextQuery.Parameters.Add("@NUMFUNC", numFunc);              

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