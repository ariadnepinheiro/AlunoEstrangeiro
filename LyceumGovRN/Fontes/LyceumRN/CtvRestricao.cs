using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class CtvRestricao : RNBase
    {
        public static bool VerificaRestricao(int idAgenda, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    dbo.TCE_CTV_RESTRICAO
                            WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                    AND CENSO = @CENSO "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public DataTable ListaPor(FiltroRestricaoTerminalidade filtro)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retricoes = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                R.ID_RESTRICAO ,
                                R.ID_AGENDA_CONF_TURNO_VAGA ,
                                A.ANO ,
                                A.PERIODO ,
                                RE.REGIONAL ,
                                M.NOME AS MUNICIPIO ,
                                R.CENSO ,
                                R.CENSO + ' - ' + UE.NOME_COMP AS ESCOLA ,
                                MC.DESCRICAO AS 'MODALIDADE' ,
                                C.NOME AS 'CURSO' ,
                                C.CURSO AS 'CODCURSO' ,
                                S.SERIE AS 'SERIE' ,
                                 CASE R.TERMINALIDADE WHEN 1 THEN 'x' ELSE '' END TERMINALIDADE
                        FROM    TCE_CTV_RESTRICAO R
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON R.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN LY_CURSO C ON C.CURSO = A.CURSO
                                INNER JOIN LY_SERIE S ON S.SERIE = A.SERIE
                                                         AND S.CURSO = C.CURSO
                                INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                INNER JOIN LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = R.CENSO
                                INNER JOIN HADES.DBO.HD_MUNICIPIO M ON UE.MUNICIPIO = M.MUNICIPIO
                                INNER JOIN DBO.TCE_REGIONAL RE ON UE.ID_REGIONAL = RE.ID_REGIONAL
                        WHERE   A.ANO = @ANO
                                AND A.PERIODO = @PERIODO
                                AND A.CURSO = ISNULL(@CURSO, A.CURSO)
                                AND ( A.SERIE = @SERIE
                                      OR @SERIE = -1
                                    )
                                AND UE.UNIDADE_ENS = ISNULL(@UNIDADE_ENS, UE.UNIDADE_ENS)
                                AND UE.ID_REGIONAL = ISNULL(@ID_REGIONAL, UE.ID_REGIONAL)
                                AND UE.MUNICIPIO = ISNULL(@MUNICIPIO, UE.MUNICIPIO)";
                if (filtro.Terminalidade)
                {
                    contextQuery.Command = contextQuery.Command + " AND R.TERMINALIDADE = @TERMINALIDADE";
                    contextQuery.Parameters.Add("@TERMINALIDADE", filtro.Terminalidade);
                }
                contextQuery.Command = contextQuery.Command + @" ORDER BY MC.DESCRICAO ,
                                C.NOME ,
                                S.SERIE ";


                contextQuery.Parameters.Add("@ANO", filtro.Ano);
                contextQuery.Parameters.Add("@PERIODO", filtro.Periodo);
                contextQuery.Parameters.Add("@CURSO", filtro.PorCurso ? filtro.Curso : null);
                contextQuery.Parameters.Add("@SERIE", filtro.PorSerie ? filtro.Serie : -1);
                contextQuery.Parameters.Add("@UNIDADE_ENS", filtro.PorUnidadeEnsino ? filtro.UnidadeEnsino : null);
                contextQuery.Parameters.Add("@ID_REGIONAL", filtro.PorRegional ? filtro.Regional : null);
                contextQuery.Parameters.Add("@MUNICIPIO", filtro.PorMunicipio ? filtro.Municipio : null);

                retricoes = ctx.GetDataTable(contextQuery);
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

            return retricoes;
        }

        public ValidacaoDados ValidaEMontaListaRestricaoPor(FiltroRestricaoTerminalidade filtro, string matriculaResponsavel, out List<TceCtvRestricao> restricoes)
        {
            List<string> mensagens = new List<string>();
            Regional rnRegional = new Regional();
            TceCtvRestricao retricao = new TceCtvRestricao();
            CtvAgendaConfTurnoVaga rnCtvAgendaConfTurnoVaga = new CtvAgendaConfTurnoVaga();
            restricoes = new List<TceCtvRestricao>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(matriculaResponsavel))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório.");
            }

            if (!filtro.PorRegional && !filtro.PorCurso && !filtro.PorMunicipio && !filtro.PorUnidadeEnsino)
            {
                mensagens.Add("É necessário selecionar ao menos um tipo específico de FILTRO (Por Regional / Por Municipio / Por Escola / Por Curso).");
            }

            if (filtro.PorRegional && string.IsNullOrEmpty(filtro.Regional))
            {
                mensagens.Add("Caso a opção POR REGIONAL seja marcada, a REGIONAL é de preenchimento obrigatório.");
            }

            if (filtro.PorMunicipio && string.IsNullOrEmpty(filtro.Municipio))
            {
                mensagens.Add("Caso a opção POR MUNICIPIO seja marcada, o MUNICIPIO é de preenchimento obrigatório.");
            }

            //Para escolha de municpio a regional é obrigatoria
            if (filtro.PorMunicipio && string.IsNullOrEmpty(filtro.Regional))
            {
                mensagens.Add("Caso a opção POR MUNICIPIO seja marcada, a REGIONAL é de preenchimento obrigatório.");
            }

            if (filtro.PorUnidadeEnsino && string.IsNullOrEmpty(filtro.UnidadeEnsino))
            {
                mensagens.Add("Caso a opção POR UNIDADE ENSINO seja marcada, a UNIDADE ENSINO é de preenchimento obrigatório.");
            }

            if (filtro.PorCurso && string.IsNullOrEmpty(filtro.Curso))
            {
                mensagens.Add("Caso a opção POR CURSO seja marcada, o CURSO é de preenchimento obrigatório.");
            }

            //Para escolha de serie o curso é obrigatorio
            if (filtro.PorSerie && string.IsNullOrEmpty(filtro.Curso))
            {
                mensagens.Add("Caso a opção POR SERIE seja marcada, o CURSO é de preenchimento obrigatório.");
            }

            if (filtro.PorSerie && filtro.Serie <= 0)
            {
                mensagens.Add("Caso a opção POR SERIE seja marcada, a SERIE é de preenchimento obrigatório.");
            }

            if (rnCtvAgendaConfTurnoVaga.PossuiAgendaEncerradaPor(filtro))
            {
                mensagens.Add("Não é possível cadastrar restrição pois existe agenda(s) encerrada(s).");
            }

            if (mensagens.Count == 0)
            {
                if (!string.IsNullOrEmpty(filtro.Regional) && !string.IsNullOrEmpty(filtro.Municipio) && !rnRegional.EhMunicipioPertencentePor(Convert.ToInt32(filtro.Regional), filtro.Municipio))
                {
                    mensagens.Add("Este MUNICÍPIO não pertence a esta REGIONAL.");
                }     
                //Monta lista de restriçoes que serão necessarias a partir dos filtros escolhidos
                restricoes = this.MontaListaRestricoesNaoCadastradasPor(filtro, matriculaResponsavel);

                //Verifica se foram encontradas restriçoes para serem cadastradas para o conjunto de filtros escolhidos
                if (restricoes.Count <= 0)
                {
                    mensagens.Add("Não existe restrição a ser cadastrada ou agenda de turnos e vagas para este conjunto de filtro(s).");
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

        public List<TceCtvRestricao> MontaListaRestricoesNaoCadastradasPor(FiltroRestricaoTerminalidade filtro, string matriculaResponsavel)
        {
            List<TceCtvRestricao> listaRestricoes = new List<TceCtvRestricao>();
            List<int> listaIdsAgendaConfTurnoVaga = new List<int>();
            List<string> listaUnidadesEnsino = new List<string>();
            CtvAgendaConfTurnoVaga rnAgendaConfTurnoVaga = new CtvAgendaConfTurnoVaga();
            UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            TceCtvRestricao restricao = new TceCtvRestricao();

            //Busca lista de IdAgendaConfTurnoVaga o conjunto de filtros            
            listaIdsAgendaConfTurnoVaga = rnAgendaConfTurnoVaga.ObtemListaIdsAgendaConfTurnoVagaPor(filtro);

            //Busca lista de escolas para o conjunto de filtros
            listaUnidadesEnsino = rnUnidadeEnsino.ObtemListaUnidadesEnsinoPor(filtro);

            //Percorre as listas de IdAgendaConfTurnoVaga e de Escolas montando as entidades
            foreach (int idAgenda in listaIdsAgendaConfTurnoVaga)
            {
                foreach (string unidadeEnsino in listaUnidadesEnsino)
                {
                    //Verifica se a escola possui permissão para o curso
                    if (rnAgendaConfTurnoVaga.EhCursoPermitidoPor(idAgenda, unidadeEnsino))
                    {
                        restricao = new TceCtvRestricao
                        {
                            IdAgendaConfTurnoVaga = idAgenda,
                            Censo = unidadeEnsino,
                            Matricula = matriculaResponsavel,
                            DtCadastro = DateTime.Now,
                            Terminalidade = filtro.Terminalidade
                        };

                        //Verifica se já existe aquela restrição cadastrada
                        if (!CtvRestricao.VerificaRestricao(restricao.IdAgendaConfTurnoVaga, restricao.Censo))
                        {
                            //Caso não exista, adiciona na lista final de restriçoes não cadastradas
                            listaRestricoes.Add(restricao);
                        }
                    }
                }
            }

            return listaRestricoes;
        }

        public ResumoRestricoesParaCadastro ObtemResumoParaCadastroPor(List<TceCtvRestricao> restricoes)
        {
            ResumoRestricoesParaCadastro resumoRestricoesParaCadastro = new ResumoRestricoesParaCadastro();
            IEnumerable<string> listaUnidadesEnisno = new List<string>();
            IEnumerable<string> listaIdsAgendas = new List<string>();
            string censos = string.Empty;
            string agendas = string.Empty;

            try
            {
                //Obtem unidades de ensino que terão restriçoes cadastradas
                listaUnidadesEnisno = restricoes.Select(x => x.Censo).Distinct();

                //Obtem total de unidades de ensino que terão restriçoes cadastradas
                resumoRestricoesParaCadastro.UnidadesEnsino = listaUnidadesEnisno.Count();

                //Monta unidades de ensino para consulta
                censos = listaUnidadesEnisno.Aggregate((x, y) => x + "', '" + y);

                //Busca Total de Regionais e Municipios
                this.ObtemResumoRegionaisMunicipiosParaCadastroPor(resumoRestricoesParaCadastro, censos);

                //Obtem agendas que terão restriçoes cadastradas
                listaIdsAgendas = restricoes.Select(x => Convert.ToString(x.IdAgendaConfTurnoVaga)).Distinct();

                //Monta agendas para consulta
                agendas = listaIdsAgendas.Aggregate((x, y) => x + ", " + y);

                //Busca Total de Cursos e Series
                this.ObtemResumoCursosSereisParaCadastroPor(resumoRestricoesParaCadastro, agendas);

                return resumoRestricoesParaCadastro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ObtemResumoRegionaisMunicipiosParaCadastroPor(ResumoRestricoesParaCadastro resumo, string unidadesEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                //Busca Total de Regionais e Municipios
                contextQuery.Command = string.Format(@" SELECT  COUNT(DISTINCT ID_REGIONAL) AS QTDE_REGIONAIS ,
                                    COUNT(DISTINCT MUNICIPIO) AS QTDE_MUNICIPIOS
                            FROM    DBO.LY_UNIDADE_ENSINO
                            WHERE   UNIDADE_ENS IN ( '{0}' ) ", unidadesEnsino);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    resumo.Regionais = Convert.ToInt32(reader["QTDE_REGIONAIS"]);
                    resumo.Municipios = Convert.ToInt32(reader["QTDE_MUNICIPIOS"]);
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        private void ObtemResumoCursosSereisParaCadastroPor(ResumoRestricoesParaCadastro resumo, string idsAgendas)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                //Busca Total de Cursos e Serie
                contextQuery.Command = string.Format(@" SELECT  COUNT(DISTINCT CURSO) AS QTDE_CURSOS ,
                        COUNT(DISTINCT SERIE) AS QTDE_SERIES
                FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                WHERE   ID_AGENDA_CONF_TURNO_VAGA IN ( {0} ) ", idsAgendas);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    resumo.Cursos = Convert.ToInt32(reader["QTDE_CURSOS"]);
                    resumo.Series = Convert.ToInt32(reader["QTDE_SERIES"]);
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public void Insere(List<TceCtvRestricao> restricoes)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                foreach (TceCtvRestricao restricao in restricoes)
                {
                    this.Insere(ctx, restricao);
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
                } throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void Insere(DataContext ctx, TceCtvRestricao restricao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO dbo.TCE_CTV_RESTRICAO
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              CENSO ,
                              MATRICULA ,
                              DT_CADASTRO ,
                              TERMINALIDADE          
                            )
                    VALUES  ( @ID_AGENDA_CONF_TURNO_VAGA ,
                              @CENSO ,
                              @MATRICULA ,
                              @DT_CADASTRO,
                              @TERMINALIDADE 
                            ) ";

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", restricao.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", restricao.Censo);
                contextQuery.Parameters.Add("@MATRICULA", restricao.Matricula);
                contextQuery.Parameters.Add("@DT_CADASTRO", restricao.DtCadastro);
                contextQuery.Parameters.Add("@TERMINALIDADE", restricao.Terminalidade);

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

        public void Remove(List<int> idsRestricoes)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                foreach (int idRestricao in idsRestricoes)
                {
                    this.Remove(ctx, idRestricao);
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
                } throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void Remove(DataContext ctx, int idRestricao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE dbo.TCE_CTV_RESTRICAO
                    WHERE ID_RESTRICAO = @ID ";

                contextQuery.Parameters.Add("@ID", idRestricao);

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

        public void InsereRestricaoTerminalidadePor(DataContext ctx, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            TceCtvRestricao restricao = new TceCtvRestricao();
            RN.Serie rnSerie = new RN.Serie();
            List<string[]> listaCursoSerieAnterior = new List<string[]>();
            List<string> unidadesEnsino = new List<string>();

            //Busca terminalidades cadastradas para o curso/serie no ano/periodo referencia
            unidadesEnsino = this.ListaUnidadesEnsinoComRestricaoTerminalidadePor(ctvAgendaConfTurnoVaga.AnoReferencia, ctvAgendaConfTurnoVaga.PeriodoReferencia, ctvAgendaConfTurnoVaga.Curso, ctvAgendaConfTurnoVaga.Serie);

            //Busca cursos/series anteriores
            listaCursoSerieAnterior = rnSerie.ObtemCursosSeriesAnterioresPor(ctvAgendaConfTurnoVaga.Curso, ctvAgendaConfTurnoVaga.Serie);

            foreach (string[] cursoSerieAnterior in listaCursoSerieAnterior)
            {
                //Apenas criar restrição de terminalidade caso o curso seja o mesmo
                if (cursoSerieAnterior[0] == ctvAgendaConfTurnoVaga.Curso)
                {
                    //Busca terminalidades cadastradas para cada curso/serie ANTERIOR no ano/periodo referencia
                    unidadesEnsino.AddRange(this.ListaUnidadesEnsinoComRestricaoTerminalidadePor(ctvAgendaConfTurnoVaga.AnoReferencia, ctvAgendaConfTurnoVaga.PeriodoReferencia, cursoSerieAnterior[0], Convert.ToInt32(cursoSerieAnterior[1])));
                }
            }

            //Insere novas restriçoes para as unidades de ensino do ano/periodo referencia
            foreach (string censo in unidadesEnsino.Distinct())
            {
                restricao = new TceCtvRestricao
                {
                    IdAgendaConfTurnoVaga = ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga,
                    Censo = censo,
                    Matricula = ctvAgendaConfTurnoVaga.Matricula,
                    DtCadastro = DateTime.Now,
                    Terminalidade = true
                };

                this.Insere(ctx, restricao);
            }
        }

        private List<string> ListaUnidadesEnsinoComRestricaoTerminalidadePor(int ano, int periodo, string curso, int serie)
        {
            List<string> unidadesEnsino = new List<string>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  R.CENSO
                            FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN DBO.TCE_CTV_RESTRICAO R ON A.ID_AGENDA_CONF_TURNO_VAGA = R.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   A.ANO = @ANO
                                    AND A.PERIODO = @PERIODO
                                    AND A.CURSO = @CURSO
                                    AND A.SERIE = @SERIE
                                    AND R.TERMINALIDADE = 1 ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    unidadesEnsino.Add(Convert.ToString(reader["CENSO"]));
                }

                return unidadesEnsino;
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
               
    }
}
