using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class CtvTurmaProvisoria : RNBase
    {
        public static void Inserir(TceCtvTurmaProvisoria ctvTurmaProvisoria)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO dbo.TCE_CTV_TURMA_PROVISORIA
                            ( ANO ,
                              PERIODO ,
                              CENSO ,
                              TURMA ,
                              CURSO ,
                              SERIE ,
                              MATRICULA
                            )
                    VALUES  ( @ANO ,
                              @PERIODO ,
                              @CENSO ,
                              @TURMA ,
                              @CURSO ,
                              @SERIE ,
                              @MATRICULA
                            )"
                    };

                    contextQuery.Parameters.Add("@ANO", ctvTurmaProvisoria.Ano);
                    contextQuery.Parameters.Add("@PERIODO", ctvTurmaProvisoria.Periodo);
                    contextQuery.Parameters.Add("@CENSO", ctvTurmaProvisoria.Censo);
                    contextQuery.Parameters.Add("@TURMA", ctvTurmaProvisoria.Turma);
                    contextQuery.Parameters.Add("@CURSO", ctvTurmaProvisoria.Curso);
                    contextQuery.Parameters.Add("@SERIE", ctvTurmaProvisoria.Serie);
                    contextQuery.Parameters.Add("@MATRICULA", ctvTurmaProvisoria.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public ValidacaoDados ValidaTurmaProvisoria(TceCtvTurmaProvisoria ctvTurmaProvisoria, int idAgendaConfTurnoVaga, int codPerfil, int totalSalasDisponiveis)
        {
            var mensagens = new List<string>();
            int qtdeTurmas = 0;
            Turma rnTurma = new Turma();
            int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);
            Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Techne.Lyceum.RN.Agenda.ParametroTurnoVaga();
            RN.Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new RN.Agenda.Entidades.ParametroTurnoVaga();
            int qtdeTurmasProvisorias = 0;
            double porcentagemMaxima = 0;

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ctvTurmaProvisoria == null)
            {
                return validacaoDados;
            }

            if (idAgendaConfTurnoVaga <= 0 && !string.IsNullOrEmpty(ctvTurmaProvisoria.Curso) && ctvTurmaProvisoria.Serie > 0)
            {
                mensagens.Add("A AGENDA para este curso/ano/período não foi criada!</br>");
            }

            if (ctvTurmaProvisoria.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!</br>");
            }

            if (ctvTurmaProvisoria.Periodo < 0)
            {
                mensagens.Add("O campo PERÍODO é obrigatório!</br>");
            }

            if (string.IsNullOrEmpty(ctvTurmaProvisoria.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!</br>");
            }

            if (string.IsNullOrEmpty(ctvTurmaProvisoria.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!</br>");
            }

            if (ctvTurmaProvisoria.Serie < 0)
            {
                mensagens.Add("O campo SÉRIE é obrigatório!</br>");
            }

            if (string.IsNullOrEmpty(ctvTurmaProvisoria.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!</br>");
            }

            if (string.IsNullOrEmpty(ctvTurmaProvisoria.Matricula)
                    || (!string.IsNullOrEmpty(ctvTurmaProvisoria.Matricula)
                        && ctvTurmaProvisoria.Matricula.Length > 20))
            {
                mensagens.Add("O campo MATRÍCULA é obrigatório com o máximo de 20 caracteres!</br>");
            }

            if (idAgendaConfTurnoVaga > 0)
            {
                //verificar se existe restrição para aquele censo / agenda
                var restricao = RN.CtvRestricao.VerificaRestricao(idAgendaConfTurnoVaga, ctvTurmaProvisoria.Censo);
                if (restricao)
                {
                    mensagens.Add("Este censo possiu uma restrição para o lançamento do CURSO: " + ctvTurmaProvisoria.Curso + " SÉRIE: " + Convert.ToString(ctvTurmaProvisoria.Serie));
                }

                //verificar se existe proposta seeduc para aquele censo / agenda
                var propostaSeeduc = CtvPropostaSeeduc.VerificaPropostaSeeducPorAgenda(idAgendaConfTurnoVaga, ctvTurmaProvisoria.Censo);
                if (!propostaSeeduc)
                {
                    mensagens.Add("Não existe proposta Seeduc para o curso / série.</br>");
                }
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //Verifica já existe outra turma com o mesmo nome ano / periodo / censo
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" SELECT  1
                                FROM    DBO.TCE_CTV_TURMA_PROVISORIA
                                WHERE   ANO = @ANO
                                        AND PERIODO = @PERIODO
                                        AND CENSO = @CENSO
                                        AND TURMA = @TURMA "
                    };

                    contextQuery.Parameters.Add("@ANO", ctvTurmaProvisoria.Ano);
                    contextQuery.Parameters.Add("@PERIODO", ctvTurmaProvisoria.Periodo);
                    contextQuery.Parameters.Add("@CENSO", ctvTurmaProvisoria.Censo);
                    contextQuery.Parameters.Add("@TURMA", ctvTurmaProvisoria.Turma);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma turma com este nome / censo cadastrada anteriormente.</br>");
                    }

                    //Verifica já existe outra turma com o mesmo nome ano referencia / periodo referencia / censo
                    contextQuery = new ContextQuery
                    {
                        Command =
                            @" SELECT  1
                                FROM    LY_TURMA T
                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ANO = A.ANO_REFERENCIA
                                                                                           AND T.SEMESTRE = A.PERIODO_REFERENCIA
                                WHERE   A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                        AND TURMA = @TURMA
                                        AND T.CURSO = @CURSO
                                        AND SIT_TURMA <> 'Desativada' 
                                        AND OPTATIVAREFORCO = 'N'
                                        AND ISNULL(T.ELETIVA,'N') = 'N'
                                        AND T.DEPENDENCIA IS NOT NULL "
                    };

                    contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaConfTurnoVaga);
                    contextQuery.Parameters.Add("@TURMA", ctvTurmaProvisoria.Turma);
                    contextQuery.Parameters.Add("@CURSO", ctvTurmaProvisoria.Curso);

                    obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe uma TURMA com este nome / censo cadastrada anteriormente.</br>");
                    }

                    //Carregar dados da agenda: ano e periodo referencia
                    var agenda = RN.CtvAgendaConfTurnoVaga.Carregar(ctvTurmaProvisoria.Ano, ctvTurmaProvisoria.Periodo, ctvTurmaProvisoria.Curso, ctvTurmaProvisoria.Serie);

                    if (agenda.Encerrado)
                    {
                        mensagens.Add("O lançamento de vagas para este ano/periodo/curso série já foi encerrado.</br>");
                    }

                    //Verifica se o usuario é privilegiado
                    if (codPerfil != 0)
                    {
                        //Carrega a quantidade de turmas provisorias para a escola no ano/periodo
                        qtdeTurmasProvisorias = RetornaQtdeTurmasProvisorias(agenda.Ano, agenda.Periodo, ctvTurmaProvisoria.Censo);

                        //Carrega a quantidade turma turmas para a escola no ano/periodo referencia
                        qtdeTurmas = rnTurma.RetornaQtdeTurmasConfTurnoVagaPor(agenda.Ano, ctvTurmaProvisoria.Censo, tipoEventoConfirmacaoVagas);

                        //Busca parametros do perfil logado
                        parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, agenda.AgendaId);

                        //Verifica se perfil pode criar turmas provisorias
                        if (!parametroTurnoVaga.PodeTurmaProvisoria)
                        {
                            mensagens.Add("Este perfil não pode criar turmas provisórias.</br>");
                        }
                        else
                        {
                            //Verifica se o perfil tem um limite para criação de turma provisoria
                            if (parametroTurnoVaga.PossuiLimiteTurmaProvisoria)
                            {
                                porcentagemMaxima = (double)(qtdeTurmas * parametroTurnoVaga.PercentualCriacaoTurma) / 100;

                                //Verificar se a criação de novas turmas já atingiu o limite.
                                if ((qtdeTurmasProvisorias + 1) > porcentagemMaxima)
                                {
                                    mensagens.Add("O limite para criação de turmas nesta unidade já foi alcançado, para obter informações é necessário entrar em contato através do seguinte endereço de email: coordenacaomatricula@educacao.rj.gov.br </br>");
                                }
                            }
                        }
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

        public static int RetornaQtdeTurmasProvisorias(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT  COUNT(DISTINCT TURMA)
                        FROM    DBO.TCE_CTV_TURMA_PROVISORIA
                        WHERE  CENSO = @CENSO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO "
                };
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        /// <summary>
        /// RemoveTurmaProvisoriaPor
        /// </summary>
        /// <param name="pId"></param>
        public static void RemoveTurmaProvisoriaPor(int pId)
        {
            var contextQuery = new ContextQuery(
                @" DELETE dbo.TCE_CTV_TURMA_PROVISORIA
                    WHERE id_turma_provisoria = @ID");

            contextQuery.Parameters.Add("@ID", pId);

            ExecutarAlteracao(contextQuery);
        }

        /// <summary>
        /// RetornaTurmasProvisoriasPor
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <param name="censo"></param>
        /// <returns></returns>
        public static DataTable RetornaTurmasProvisoriasPor(object ano, object periodo, object censo)
        {
            //IList<RN.Entidades.TceCtvTurmaProvisoria> listaTceCtvTurmaProvisoria = null;
            //RN.Entidades.TceCtvTurmaProvisoria objTceCtvTurmaProvisoria = null;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"
                                SELECT P.id_turma_provisoria AS ID, 
                                       P.ano                 AS ANO, 
                                       P.periodo             AS PERIODO, 
                                       P.censo               AS CENSO, 
                                       P.turma               AS TURMA, 
                                       P.serie               AS SERIE, 
                                       c.nome                AS CURSO, 
                                       isnull(v.sala, '-')   AS SALA
                                FROM   tce_ctv_turma_provisoria P 
                                       INNER JOIN tce_ctv_agenda_conf_turno_vaga A 
                                               ON P.ano = A.ano 
                                                  AND P.periodo = A.periodo 
                                                  AND P.curso = A.curso 
                                                  AND P.serie = A.serie 
                                       INNER JOIN ly_curso c 
                                               ON P.curso = c.curso 
                                       LEFT OUTER JOIN tce_ctv_conf_vaga v 
                                                    ON v.id_agenda_conf_turno_vaga = A.id_agenda_conf_turno_vaga 
                                                       AND P.censo = v.censo 
                                                       AND P.turma = v.turma 
                                WHERE  P.censo = @CENSO 
                                       AND P.ano = @ANO 
                                       AND P.periodo = @PERIODO "
                    };

                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", censo);

                    return ctx.GetDataTable(contextQuery);
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
        }

        /// <summary>
        /// VerificaSalaAssociada
        /// </summary>
        /// <param name="pCenso"></param>
        /// <param name="pTurma"></param>
        /// <param name="pAno"></param>
        /// <param name="pPeriodo"></param>
        /// <returns></returns>
        public static bool VerificaSalaAssociada(string pCenso, string pTurma, string pAno, string pPeriodo)
        {
            bool blnRetorno = false;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"
                            SELECT * 
                            FROM   tce_ctv_turma_provisoria P, 
                                   tce_ctv_conf_vaga C, 
                                   tce_ctv_agenda_conf_turno_vaga A 
                            WHERE  P.censo = @CENSO 
                                   AND P.turma = @TURMA 
                                   AND P.ano = @ANO 
                                   AND P.periodo = @PERIODO 
                                   AND C.id_agenda_conf_turno_vaga = A.id_agenda_conf_turno_vaga 
                                   AND P.ano = A.ano 
                                   AND P.periodo = A.periodo 
                                   AND P.curso = A.curso 
                                   AND P.serie = A.serie 
                                   AND P.censo = C.censo 
                                   AND P.turma = C.turma "
                    };

                    contextQuery.Parameters.Add("@CENSO", pCenso);
                    contextQuery.Parameters.Add("@TURMA", pTurma);
                    contextQuery.Parameters.Add("@ANO", pAno);
                    contextQuery.Parameters.Add("@PERIODO", pPeriodo);

                    var retorno = ctx.GetDataTable(contextQuery);

                    if (retorno.Rows.Count > 0)
                    {
                        blnRetorno = true;
                    }

                    return blnRetorno;
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
        }

        public DataTable ListaTurmaProvisoriaPor(string censo, string ano, string tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @"  SELECT P.ID_TURMA_PROVISORIA AS ID, 
                                       P.ANO                 AS ANO, 
                                       P.PERIODO             AS PERIODO, 
                                       P.CENSO               AS CENSO, 
                                       P.TURMA               AS TURMA, 
                                       P.SERIE               AS SERIE, 
                                       C.NOME                AS CURSO, 
                                       ISNULL(V.SALA, '-')   AS SALA
                                FROM   TCE_CTV_TURMA_PROVISORIA P 
                                       INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A 
                                               ON P.ANO = A.ANO 
                                                  AND P.PERIODO = A.PERIODO 
                                                  AND P.CURSO = A.CURSO 
                                                  AND P.SERIE = A.SERIE 
                                       INNER JOIN LY_CURSO C 
                                               ON P.CURSO = C.CURSO 
                                       LEFT OUTER JOIN TCE_CTV_CONF_VAGA V 
                                                    ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA 
                                                       AND P.CENSO = V.CENSO 
                                                       AND P.TURMA = V.TURMA 
                                WHERE  P.CENSO = @CENSO 
                                        AND P.ANO=@ANO
                                      AND ( A.PERIODO IN (SELECT PERIODO 
                                                               FROM   agenda.PERIODOLETIVOAGENDA P 
                                                                      INNER JOIN agenda.AGENDA AA 
                                                                              ON aa.AGENDAID = P.AGENDAID 
                                                                      INNER JOIN agenda.EVENTO AE 
                                                                              ON AA.AGENDAID = AE.AGENDAID 
                                                               WHERE  Getdate() BETWEEN DATAINICIO AND DATAFIM 
                                                                      AND AE.TIPOEVENTOID = @TIPOEVENTOID
                                                                      --TipoEvento Copnfirmação de Vagas  
                                                                      AND P.ANO = A.ANO) )
                                       ";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                turmas = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public DataTable ListaSalaAssociadaPor(int idTurmaProvisoria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable salas = null;

            try
            {
                contextQuery.Command = @"  SELECT  *
                                  FROM   TCE_CTV_TURMA_PROVISORIA P, 
                                           TCE_CTV_CONF_VAGA C, 
                                           TCE_CTV_AGENDA_CONF_TURNO_VAGA A 
                                    WHERE  P.ID_TURMA_PROVISORIA = @ID_TURMA_PROVISORIA                                 
                                           AND C.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA 
                                           AND P.ANO = A.ANO 
                                           AND P.PERIODO = A.PERIODO 
                                           AND P.CURSO = A.CURSO 
                                           AND P.SERIE = A.SERIE 
                                           AND P.CENSO = C.CENSO 
                                           AND P.TURMA = C.TURMA
                                       ";
                contextQuery.Parameters.Add("@ID_TURMA_PROVISORIA", idTurmaProvisoria);

                salas = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return salas;
        }
    }
}
