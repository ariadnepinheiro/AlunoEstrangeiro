using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class CtvAgendaConfTurnoVaga : RNBase
    {
        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM TCE_CTV_AGENDA_CONF_TURNO_VAGA
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  A.* ,
                            C.NOME ,
                            CONVERT(VARCHAR, ANO) + ' - ' + CONVERT(VARCHAR, PERIODO) AS anoperiodo ,
                            CONVERT(VARCHAR, ANO_REFERENCIA) + ' - '
                            + CONVERT(VARCHAR, PERIODO_REFERENCIA) AS anoperiodoreferencia ,
                            PE.DESCRICAO AS PERFIL_RESPONSAVEL ,
                            MC.modalidade ,
                            TC.DESCRICAO AS nivel ,
                            C.CURSO ,
                            CASE A.ENCERRADO
                              WHEN 1 THEN 'Encerrada'
                              ELSE ''
                            END SITUACAO
                    FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                            INNER JOIN LY_CURSO C ON A.CURSO = C.CURSO
                            INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                            INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                            LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                            LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                    ORDER BY ANO DESC ,
                            PERIODO DESC ,
                            C.NOME ,
                            SERIE "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarParaEncerramento(int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  A.* ,
                                    C.NOME ,
                                    CONVERT(VARCHAR, ANO) + ' - ' + CONVERT(VARCHAR, PERIODO) AS ANOPERIODO  
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN LY_CURSO C ON A.CURSO = C.CURSO        
                            WHERE   ENCERRADO = 0
                            AND A.ANO = @ANO
                            AND A.PERIODO = @PERIODO
                            ORDER BY ANO ,
                                    PERIODO ,
                                    C.NOME ,
                                    SERIE "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ICollection<DadosAgendaTurnos> Listar(List<int> idAgenda)
        {
            ICollection<DadosAgendaTurnos> listaAgendas = new Collection<DadosAgendaTurnos>();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                foreach (var id in idAgenda)
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT  ID_AGENDA_CONF_TURNO_VAGA ,
                                    MC.DESCRICAO AS NOME_MODALIDADE ,
                                    C.NOME AS NOME_CURSO ,
                                    A.SERIE ,
                                    MC.MODALIDADE ,
                                    TC.DESCRICAO AS NIVEL ,
                                    C.CURSO ,
                                    DT_INICIO_CONF_TURNO ,
                                    DT_FIM_CONF_TURNO
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN LY_CURSO C ON A.CURSO = C.CURSO
                                    INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                    INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                            WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA "
                    };

                    contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", id);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            var agenda = new DadosAgendaTurnos
                            {
                                IdAgendaConfTurnoVaga = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]),
                                NomeModalidade = Convert.ToString(reader["NOME_MODALIDADE"]),
                                NomeCurso = Convert.ToString(reader["NOME_CURSO"]),
                                Serie = Convert.ToInt32(reader["SERIE"]),
                                Modalidade = Convert.ToString(reader["MODALIDADE"]),
                                Nivel = Convert.ToString(reader["NIVEL"]),
                                Curso = Convert.ToString(reader["CURSO"]),
                                DtInicioConfTurno = Convert.ToDateTime(reader["DT_INICIO_CONF_TURNO"]),
                                DtFimConfTurno = Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"])
                            };

                            listaAgendas.Add(agenda);
                        }
                    }
                }
            }

            return listaAgendas;
        }

        public static ICollection<DadosEncerramentoConfVagas> ListarDadosEncerramento(int idAgenda)
        {
            ICollection<DadosEncerramentoConfVagas> listaAgendas = new Collection<DadosEncerramentoConfVagas>();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                    {
                        Command = @"  SELECT A.ID_AGENDA_CONF_TURNO_VAGA ,
                                A.ANO ,
                                A.PERIODO ,
                                A.CURSO ,
                                A.SERIE ,
                                V.ID_CONF_VAGA ,
                                V.CURRICULO ,
                                V.TURNO ,
                                V.SALA ,
                                V.CENSO ,
                                V.TURMA ,
                                V.VAGAS_NOVAS ,
                                V.VAGAS_CONTINUIDADE
                         FROM   DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN DBO.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
								LEFT JOIN TCE_CTV_RESTRICAO R ON A.ID_AGENDA_CONF_TURNO_VAGA = R.ID_AGENDA_CONF_TURNO_VAGA and v.CENSO = r.CENSO
                         WHERE  A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND A.ENCERRADO = 0
								AND R.ID_RESTRICAO IS NULL "
                    };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        var agenda = new DadosEncerramentoConfVagas
                        {
                            IdAgendaConfTurnoVaga = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]),
                            Ano = Convert.ToInt32(reader["ANO"]),
                            Periodo = Convert.ToInt32(reader["PERIODO"]),
                            Curso = Convert.ToString(reader["CURSO"]),
                            Serie = Convert.ToInt32(reader["SERIE"]),
                            IdConfVaga = Convert.ToInt32(reader["ID_CONF_VAGA"]),
                            Curriculo = Convert.ToString(reader["CURRICULO"]),
                            Turno = Convert.ToString(reader["TURNO"]),
                            Sala = Convert.ToString(reader["SALA"]),
                            Censo = Convert.ToString(reader["CENSO"]),
                            Turma = Convert.ToString(reader["TURMA"]),
                            VagasNovas = Convert.ToInt32(reader["VAGAS_NOVAS"]),
                            VagasContinuidade = Convert.ToInt32(reader["VAGAS_CONTINUIDADE"])
                        };

                        listaAgendas.Add(agenda);
                    }
                }
            }

            return listaAgendas;
        }

        public static TceCtvAgendaConfTurnoVaga Carregar(int id)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT  *
                        FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ");
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", id);

                return ctx.TryToBindEntity<TceCtvAgendaConfTurnoVaga>(contextQuery);
            }
        }

        public static TceCtvAgendaConfTurnoVaga Carregar(int ano, int periodo, string curso, int serie)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT  *
                            FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND CURSO = @CURSO
                                    AND SERIE = @SERIE ");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                return ctx.TryToBindEntity<TceCtvAgendaConfTurnoVaga>(contextQuery);
            }
        }

        public static ValidacaoDados Validar(TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ctvAgendaConfTurnoVaga == null)
            {
                return validacaoDados;
            }

            if (ctvAgendaConfTurnoVaga.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.Periodo < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.AgendaId < 0)
            {
                mensagens.Add("O campo AGENDA ID não foi encontrado!");
            }

            if (string.IsNullOrEmpty(ctvAgendaConfTurnoVaga.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.Serie < 0)
            {
                mensagens.Add("O campo SÉRIE é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.AnoReferencia <= 0)
            {
                mensagens.Add("O campo ANO REFERÊNCIA é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.PeriodoReferencia < 0)
            {
                mensagens.Add("O campo PERÍODO REFERÊNCIA é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.AnoReferencia == ctvAgendaConfTurnoVaga.Ano)
            {
                if (ctvAgendaConfTurnoVaga.Periodo == ctvAgendaConfTurnoVaga.PeriodoReferencia)
                {
                    mensagens.Add("O campo ANO/PERÍODO não pode ser igual ao ANO/PERÍODO REFERÊNCIA!");
                }

                if (ctvAgendaConfTurnoVaga.Periodo < ctvAgendaConfTurnoVaga.PeriodoReferencia)
                {
                    mensagens.Add("O campo ANO/PERÍODO não pode ser menor ao ANO/PERÍODO REFERÊNCIA!");
                }
            }
            else
            {
                if (ctvAgendaConfTurnoVaga.Ano < ctvAgendaConfTurnoVaga.AnoReferencia)
                {
                    mensagens.Add("O campo ANO/PERÍODO não pode ser menor ao ANO/PERÍODO REFERÊNCIA!");
                }
            }

            if (ctvAgendaConfTurnoVaga.DtInicioConfTurno == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA INICIO TURNO é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.DtFimConfTurno == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA FIM TURNO é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.DtInicioConfTurno > ctvAgendaConfTurnoVaga.DtFimConfTurno)
            {
                mensagens.Add("O campo DATA DE INICIO TURNO deve ser menor que a DATA FIM!");
            }

            if (ctvAgendaConfTurnoVaga.DtInicioConfVagas == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA INICIO VAGAS é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.DtFimConfVagas == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA FIM VAGAS é obrigatório!");
            }

            if (ctvAgendaConfTurnoVaga.DtInicioConfVagas > ctvAgendaConfTurnoVaga.DtFimConfVagas)
            {
                mensagens.Add("O campo DATA DE INICIO VAGAS deve ser menor que a DATA FIM!");
            }

            if (ctvAgendaConfTurnoVaga.DtInicioConfTurno > ctvAgendaConfTurnoVaga.DtInicioConfVagas)
            {
                mensagens.Add("O campo DATA DE INICIO TURNOS deve ser menor ou igual a DATA INICIO VAGAS!");
            }

            if (ctvAgendaConfTurnoVaga.DtFimConfTurno > ctvAgendaConfTurnoVaga.DtFimConfVagas)
            {
                mensagens.Add("O campo DATA FIM DE TURNOS deve ser menor ou igual a DATA FIM VAGAS!");
            }

            if (string.IsNullOrEmpty(ctvAgendaConfTurnoVaga.Matricula)
                || (!string.IsNullOrEmpty(ctvAgendaConfTurnoVaga.Matricula)
                    && ctvAgendaConfTurnoVaga.Matricula.Length > 20))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório com o máximo de 20 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery
                     {
                         //Verifica já existe agenda para aquele ano / periodo / curso / serie
                         Command =
                             @" SELECT  1
                        FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND CURSO = @CURSO
                                AND SERIE = @SERIE
                                AND ID_AGENDA_CONF_TURNO_VAGA <> @ID_AGENDA_CONF_TURNO_VAGA "
                     };

                    contextQuery.Parameters.Add("@ANO", ctvAgendaConfTurnoVaga.Ano);
                    contextQuery.Parameters.Add("@PERIODO", ctvAgendaConfTurnoVaga.Periodo);
                    contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
                    contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
                    contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("ANO / PERIODO / CURSO / SERIE já cadastrada anteriormente.");
                    }

                    if (ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga == 0)
                    {
                        contextQuery = new ContextQuery
                        {
                            //Verifica já existe controle de vagas para aquele ano / periodo / curso / serie
                            Command =
                                @" SELECT  1
                                FROM    TCE_CONTROLE_VAGA
                                WHERE   CURSO = @CURSO
                                        AND SERIE = @SERIE
                                        AND ANO = @ANO
                                        AND PERIODO = @PERIODO "
                        };

                        contextQuery.Parameters.Add("@ANO", ctvAgendaConfTurnoVaga.Ano);
                        contextQuery.Parameters.Add("@PERIODO", ctvAgendaConfTurnoVaga.Periodo);
                        contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
                        contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);

                        obj = ctx.GetReturnValue(contextQuery);

                        if (obj != null)
                        {
                            mensagens.Add("Já existe um Controle de Vagas para este ANO / PERIODO / CURSO / SERIE.");
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

        public static void Inserir(TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            CtvRestricao rnRestricao = new CtvRestricao();

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga = Inserir(context, ctvAgendaConfTurnoVaga);

                    //Insere a agenda e progressao
                    CtvConfTurnoInicial.Inserir(context, ctvAgendaConfTurnoVaga);
                    CtvConfTurnoInicial.InserirProgressao(context, ctvAgendaConfTurnoVaga);

                    //Insere todos os Censos/Turnos daquele, curso/serie
                    CtvConfTurno.InserirTurnoNovo(context, ctvAgendaConfTurnoVaga);
                    CtvConfTurno.InserirTurnoNovoProgressao(context, ctvAgendaConfTurnoVaga);
                    CtvConfTurno.InserirTurnoContinuidade(context, ctvAgendaConfTurnoVaga);
                    CtvConfTurno.InserirTurnoContinuidadeProgressao(context, ctvAgendaConfTurnoVaga);

                    //Insere restriçoes de terminalidade existentes ano/periodo referencia
                    rnRestricao.InsereRestricaoTerminalidadePor(context, ctvAgendaConfTurnoVaga);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        private static int Inserir(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO TCE_CTV_AGENDA_CONF_TURNO_VAGA
                                ( ANO ,
                                  PERIODO ,
                                  CURSO ,
                                  SERIE ,
                                  ANO_REFERENCIA ,
                                  PERIODO_REFERENCIA ,
                                  DT_INICIO_CONF_TURNO ,
                                  DT_FIM_CONF_TURNO ,
                                  DT_INICIO_CONF_VAGAS ,
                                  DT_FIM_CONF_VAGAS ,
                                  ENCERRADO ,
                                  MATRICULA,
                                  AGENDAID
                                )
                        VALUES  ( @ANO ,
                                  @PERIODO ,
                                  @CURSO ,
                                  @SERIE ,
                                  @ANO_REFERENCIA ,
                                  @PERIODO_REFERENCIA ,
                                  @DT_INICIO_CONF_TURNO ,
                                  @DT_FIM_CONF_TURNO ,
                                  @DT_INICIO_CONF_VAGAS ,
                                  @DT_FIM_CONF_VAGAS ,
                                  0 ,
                                  @MATRICULA,
                                  @AGENDAID
                                )

                            SELECT  ID_AGENDA_CONF_TURNO_VAGA
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND CURSO = @CURSO
                                    AND SERIE = @SERIE  "
            };

            contextQuery.Parameters.Add("@ANO", ctvAgendaConfTurnoVaga.Ano);
            contextQuery.Parameters.Add("@PERIODO", ctvAgendaConfTurnoVaga.Periodo);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);
            contextQuery.Parameters.Add("@DT_INICIO_CONF_TURNO", ctvAgendaConfTurnoVaga.DtInicioConfTurno);
            contextQuery.Parameters.Add("@DT_FIM_CONF_TURNO", ctvAgendaConfTurnoVaga.DtFimConfTurno);
            contextQuery.Parameters.Add("@DT_INICIO_CONF_VAGAS", ctvAgendaConfTurnoVaga.DtInicioConfVagas);
            contextQuery.Parameters.Add("@DT_FIM_CONF_VAGAS", ctvAgendaConfTurnoVaga.DtFimConfVagas);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@AGENDAID", ctvAgendaConfTurnoVaga.AgendaId);

            return Convert.ToInt32(context.GetReturnValue(contextQuery));
        }

        public static void Alterar(TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE  TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            SET     DT_INICIO_CONF_TURNO = @DT_INICIO_CONF_TURNO ,
                                    DT_FIM_CONF_TURNO = @DT_FIM_CONF_TURNO ,
                                    DT_INICIO_CONF_VAGAS = @DT_INICIO_CONF_VAGAS ,
                                    DT_FIM_CONF_VAGAS = @DT_FIM_CONF_VAGAS ,
                                    MATRICULA = @MATRICULA ,
                                    DT_ALTERACAO = GETDATE()
                            WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA "
                    };

                    contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
                    contextQuery.Parameters.Add("@DT_INICIO_CONF_TURNO", ctvAgendaConfTurnoVaga.DtInicioConfTurno);
                    contextQuery.Parameters.Add("@DT_FIM_CONF_TURNO", ctvAgendaConfTurnoVaga.DtFimConfTurno);
                    contextQuery.Parameters.Add("@DT_INICIO_CONF_VAGAS", ctvAgendaConfTurnoVaga.DtInicioConfVagas);
                    contextQuery.Parameters.Add("@DT_FIM_CONF_VAGAS", ctvAgendaConfTurnoVaga.DtFimConfVagas);
                    contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados ValidarRemover(int id)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id <= 0)
            {
                mensagens.Add("O campo ID é obrigatório!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //verificar se algum turno já foi lançado 
                    var contextQuery = new ContextQuery(
                        @" SELECT  1
                        FROM    TCE_CTV_CONF_TURNO C
                        WHERE   C.ID_AGENDA_CONF_TURNO_VAGA = @ID
                                AND CONFIRMADA = 1 ");

                    contextQuery.Parameters.Add("@ID", id);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possivel excluir pois já foram lançados turnos para este ano / periodo / curso.");
                    }

                    //verificar se já existe proposta
                    contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    DBO.TCE_CTV_PROPOSTA_SEEDUC P
                            WHERE   P.ID_AGENDA_CONF_TURNO_VAGA = @ID ");

                    contextQuery.Parameters.Add("@ID", id);

                    obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possivel excluir pois já foram lançados propostas para este ano / periodo / curso.");
                    }

                    //verificar se já exsite restricao
                    contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    DBO.TCE_CTV_RESTRICAO P
                            WHERE   P.ID_AGENDA_CONF_TURNO_VAGA = @ID ");

                    contextQuery.Parameters.Add("@ID", id);

                    obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possivel excluir pois já foram criadas restriçoes para este ano / periodo / curso.");
                    }

                    //verificar se já existe lançamento de vagas
                    contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    dbo.TCE_CTV_CONF_VAGA c
                            WHERE   C.ID_AGENDA_CONF_TURNO_VAGA = @ID ");

                    contextQuery.Parameters.Add("@ID", id);

                    obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Não é possivel excluir pois já foram lançadas vagas para este ano / periodo / curso.");
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

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    CtvConfTurnoInicial.Excluir(ctx, id);
                    CtvConfTurno.Excluir(ctx, id);
                    Excluir(ctx, id);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        private static void Excluir(DataContext context, int idAgenda)
        {
            var contextQuery = new ContextQuery(
                @" DELETE  TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ");

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

            context.ApplyModifications(contextQuery);
        }

        public static bool ConsultarPeriodoAtivoTurnos()
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT count(*)
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA
                        WHERE   CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO_CONF_TURNO
                                                         AND     DT_FIM_CONF_TURNO ");

                var resultado = ctx.GetReturnValue<int>(contextQuery);

                return (resultado > 0);
            }
        }

        public static DataTable ConsultarPeriodoAnteriorTurnos()
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT TOP 1
                            *
                    FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA
                    WHERE   DT_INICIO_CONF_TURNO > CONVERT(DATE, GETDATE())
                    ORDER BY DT_INICIO_CONF_TURNO ");

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ConsultarPeriodoPosteriorTurnos()
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT
                                MAX(DT_FIM_CONF_TURNO) AS DT_FIM_CONF_TURNO
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                        WHERE   NOT EXISTS ( SELECT *
                                             FROM   TCE_CTV_AGENDA_CONF_TURNO_VAGA A2
                                             WHERE  A2.DT_FIM_CONF_TURNO > CONVERT(DATE, GETDATE()) )
                                AND CONVERT(DATE, GETDATE()) BETWEEN A.DT_INICIO_CONF_TURNO
                                                             AND     A.DT_FIM_CONF_TURNO ");

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarAnos()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT distinct ANO 
                                    FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                     "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPeriodo(int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT distinct PERIODO 
                                    FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A                                    
                                    WHERE ANO=@ANO"
                };

                contextQuery.Parameters.Add("@ANO", ano);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarAgendasDataFim(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT C.NOME ,
                                    A.SERIE ,
                                    A.DT_FIM_CONF_VAGAS
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN LY_CURSO C ON C.CURSO = A.CURSO
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI
                                    ON A.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   A.ANO = @ANO
                                    AND A.PERIODO = @PERIODO
                                    AND TI.CENSO = @CENSO
                                    AND CONVERT(DATE, GETDATE()) BETWEEN A.DT_INICIO_CONF_VAGAS
                                                                 AND     A.DT_FIM_CONF_VAGAS "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable RetornaAnoPeriodo(int idAgenda)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ANO, PERIODO, SERIE 
                                    FROM TCE_CTV_AGENDA_CONF_TURNO_VAGA 
                                    WHERE ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA"
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static int RetornaIdAgenda(int ano, int periodo, string curso, int serie)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            int idAgenda = 0;

            try
            {
                idAgenda = (new RN.CtvAgendaConfTurnoVaga()).ObtemIdAgendaAbertaPor(contexto, ano, periodo, curso, serie);
                return idAgenda;
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

        public int ObtemIdAgendaAbertaPor(DataContext contexto, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT  ID_AGENDA_CONF_TURNO_VAGA
                            FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND CURSO = @CURSO
                                    AND SERIE = @SERIE 
                                    AND ENCERRADO = 0 ";

                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
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

        public static int RetornaPrimeiraIdAgenda(int ano, int periodo)
        {
            var idAgenda = 0;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT TOP 1 ID_AGENDA_CONF_TURNO_VAGA
                            FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND ENCERRADO = 0
                                    "
                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        idAgenda = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
                    }
                }
            }
            return idAgenda;
        }

        public static DadosTurmaAgenda VerificaDadosTurma(int ano, int periodo, string censo, string turma, string turno)
        {
            var dados = new DadosTurmaAgenda();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"  SELECT  DISTINCT
                                ID_AGENDA_CONF_TURNO_VAGA ,
                                a.DT_INICIO_CONF_VAGAS ,
                                a.DT_FIM_CONF_VAGAS ,
                                a.DT_FIM_CONF_TURNO ,
                                tab.CURRICULO ,
                                t.CURSO , 
                                cr.NOME  AS nome_curso ,
                                t.SERIE ,
                                cr.MODALIDADE
                         FROM   TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN DBO.LY_TURMA T ON T.ANO = A.ANO_REFERENCIA
                                                             AND T.SEMESTRE = A.PERIODO_REFERENCIA
                                                             AND t.CURSO = a.CURSO
                                                             AND t.SERIE = a.SERIE 
                                INNER JOIN dbo.LY_CURSO cr ON cr.curso = t.CURSO                                   
                                LEFT JOIN ( SELECT  c.* ,
                                                    s.SERIE
                                            FROM    dbo.LY_CURRICULO c
                                                    INNER JOIN dbo.LY_SERIE s ON c.CURSO = s.CURSO
                                                                                 AND c.TURNO = s.TURNO
                                                                                 AND c.CURRICULO = s.CURRICULO
                                            WHERE   ( c.DT_EXTINCAO IS NULL
                                                      OR c.DT_EXTINCAO > GETDATE()
                                                    )
                                          ) AS tab ON t.CURSO = tab.CURSO
                                                      AND tab.TURNO = @TURNO
                                                      AND tab.SERIE = t.SERIE
                                                      AND tab.ANO_INI = a.ANO
                                                      AND tab.SEM_INI = a.PERIODO
                         WHERE  A.ANO = @ANO
                                AND A.PERIODO = @PERIODO
                                AND t.FACULDADE = @CENSO
                                AND t.TURMA = @TURMA
                                AND t.SIT_TURMA <> 'Desativada' "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@TURNO", turno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.IdAgenda = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
                        dados.DtInicioConfVagas = Convert.ToDateTime(reader["DT_INICIO_CONF_VAGAS"]);
                        dados.DtFimConfVagas = Convert.ToDateTime(reader["DT_FIM_CONF_VAGAS"]);
                        dados.Curso = Convert.ToString(reader["CURSO"]);
                        dados.NomeCurso = Convert.ToString(reader["nome_curso"]);
                        dados.Curriculo = Convert.ToString(reader["CURRICULO"]);
                        dados.Serie = Convert.ToInt32(reader["SERIE"]);
                        dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                        dados.DtFimConfTurno = Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"]);
                    }
                }
            }
            return dados;
        }

        public static DadosTurmaAgenda VerificaDadosTurmaProvisoria(int ano, int periodo, string censo, string turma, string turno)
        {
            var dados = new DadosTurmaAgenda();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"  SELECT  DISTINCT
                                ID_AGENDA_CONF_TURNO_VAGA ,
                                a.DT_INICIO_CONF_VAGAS ,
                                a.DT_FIM_CONF_VAGAS ,
                                a.DT_FIM_CONF_TURNO ,
                                t.CURSO , 
                                cr.NOME AS nome_curso ,
                                t.SERIE ,
                                tab.CURRICULO ,
                                cr.MODALIDADE
                         FROM   TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN dbo.TCE_CTV_TURMA_PROVISORIA T ON T.ANO = A.ANO
                                                             AND T.PERIODO = A.PERIODO
                                                             AND t.CURSO = a.CURSO
                                                             AND t.SERIE = a.SERIE 
                                INNER JOIN dbo.LY_CURSO cr ON cr.curso = t.CURSO                                   
                                LEFT JOIN ( SELECT  c.* ,
                                                    s.SERIE
                                            FROM    dbo.LY_CURRICULO c
                                                    INNER JOIN dbo.LY_SERIE s ON c.CURSO = s.CURSO
                                                                                 AND c.TURNO = s.TURNO
                                                                                 AND c.CURRICULO = s.CURRICULO
                                            WHERE   ( c.DT_EXTINCAO IS NULL
                                                      OR c.DT_EXTINCAO > GETDATE()
                                                    )
                                          ) AS tab ON t.CURSO = tab.CURSO
                                                      AND tab.TURNO = @TURNO
                                                      AND tab.SERIE = t.SERIE
                                                      AND tab.ANO_INI = a.ANO
                                                      AND tab.SEM_INI = a.PERIODO
                         WHERE  A.ANO = @ANO
                                AND A.PERIODO = @PERIODO
                                AND t.CENSO = @CENSO
                                AND t.TURMA = @TURMA "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@TURNO", turno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.IdAgenda = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
                        dados.DtInicioConfVagas = Convert.ToDateTime(reader["DT_INICIO_CONF_VAGAS"]);
                        dados.DtFimConfVagas = Convert.ToDateTime(reader["DT_FIM_CONF_VAGAS"]);
                        dados.Curso = Convert.ToString(reader["CURSO"]);
                        dados.NomeCurso = Convert.ToString(reader["nome_curso"]);
                        dados.Curriculo = Convert.ToString(reader["CURRICULO"]);
                        dados.Serie = Convert.ToInt32(reader["SERIE"]);
                        dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                        dados.DtFimConfTurno = Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"]);
                    }
                }
            }
            return dados;
        }

        public static DadosTurmaAgenda RetornaDadosTurma(int ano, int periodo, string censo, string turma)
        {
            var dados = new DadosTurmaAgenda();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"  SELECT DISTINCT
                            ID_AGENDA_CONF_TURNO_VAGA ,
                            A.DT_INICIO_CONF_VAGAS ,
                            A.DT_FIM_CONF_VAGAS ,
                            A.DT_FIM_CONF_TURNO ,
                            T.CURSO ,
                            cr.NOME AS nome_curso ,
                            T.SERIE ,
                            CR.MODALIDADE ,
                            M.DESCRICAO AS NOME_MODALIDADE
                    FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                            INNER JOIN DBO.LY_TURMA T ON T.ANO = A.ANO_REFERENCIA
                                                         AND T.SEMESTRE = A.PERIODO_REFERENCIA
                                                         AND T.CURSO = A.CURSO
                                                         AND T.SERIE = A.SERIE
                            INNER JOIN DBO.LY_CURSO CR ON CR.CURSO = T.CURSO
                            INNER JOIN DBO.LY_MODALIDADE_CURSO M ON CR.MODALIDADE = M.MODALIDADE
                    WHERE   A.ANO = @ANO
                            AND A.PERIODO = @PERIODO
                            AND T.FACULDADE = @CENSO
                            AND T.TURMA = @TURMA "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURMA", turma);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.IdAgenda = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
                        dados.DtInicioConfVagas = Convert.ToDateTime(reader["DT_INICIO_CONF_VAGAS"]);
                        dados.DtFimConfVagas = Convert.ToDateTime(reader["DT_FIM_CONF_VAGAS"]);
                        dados.Curso = Convert.ToString(reader["CURSO"]);
                        dados.NomeCurso = Convert.ToString(reader["nome_curso"]);
                        dados.Serie = Convert.ToInt32(reader["SERIE"]);
                        dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                        dados.NomeModalidade = Convert.ToString(reader["NOME_MODALIDADE"]);
                        dados.DtFimConfTurno = Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"]);
                    }
                }
            }
            return dados;
        }

        public static DadosTurmaAgenda RetornaDadosTurmaProvisoria(int ano, int periodo, string censo, string turma)
        {
            var dados = new DadosTurmaAgenda();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"  SELECT  DISTINCT
                                ID_AGENDA_CONF_TURNO_VAGA ,
                                A.DT_INICIO_CONF_VAGAS ,
                                A.DT_FIM_CONF_VAGAS ,
                                A.DT_FIM_CONF_TURNO ,
                                T.CURSO ,
                                cr.NOME AS nome_curso ,
                                T.SERIE ,
                                CR.MODALIDADE ,
                                M.DESCRICAO AS NOME_MODALIDADE
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN DBO.TCE_CTV_TURMA_PROVISORIA T ON T.ANO = A.ANO
                                                                             AND T.PERIODO = A.PERIODO
                                                                             AND T.CURSO = A.CURSO
                                                                             AND T.SERIE = A.SERIE
                                INNER JOIN DBO.LY_CURSO CR ON CR.CURSO = T.CURSO
                                INNER JOIN DBO.LY_MODALIDADE_CURSO M ON CR.MODALIDADE = M.MODALIDADE
                        WHERE   A.ANO = @ANO
                                AND A.PERIODO = @PERIODO
                                AND T.CENSO = @CENSO
                                AND T.TURMA = @TURMA "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURMA", turma);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        dados.IdAgenda = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
                        dados.DtInicioConfVagas = Convert.ToDateTime(reader["DT_INICIO_CONF_VAGAS"]);
                        dados.DtFimConfVagas = Convert.ToDateTime(reader["DT_FIM_CONF_VAGAS"]);
                        dados.Curso = Convert.ToString(reader["CURSO"]);
                        dados.NomeCurso = Convert.ToString(reader["nome_curso"]);
                        dados.Serie = Convert.ToInt32(reader["SERIE"]);
                        dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                        dados.NomeModalidade = Convert.ToString(reader["NOME_MODALIDADE"]);
                        dados.DtFimConfTurno = Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"]);
                    }
                }
            }
            return dados;
        }

        public static void EncerrarLancamento(IList<int> idAgendas, string matricula)
        {
            PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            RN.Serie rnSerie = new Serie();
            RN.Curso rnCurso = new Curso();

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //-- Passos para serem feitos pelo processo de encerramento de turnos e vagas:
                    //-- 1. Carregar todos os dados da turma (normal ou provisória)
                    //-- 2. Encerrar a agenda
                    //-- 3. Fazer validações para inserção de novas turmas (gravar problemas no LOG)
                    //-- 4. Caso não aja restrições criar nova turma (levar em conta colunas com dados default da tela de Turma)
                    //-- 5. Levar resultados do lançamento para tabela de controle de vagas

                    foreach (var agenda in idAgendas)
                    {
                        //Carrega dados todos os dados de todas as turma lançadas para aquela agenda
                        var lancamentos = ListarDadosEncerramento(agenda);

                        foreach (var dadosEncerramentoConfVagas in lancamentos)
                        {
                            string[] dataInicioFim = new string[2];

                            //Busca datas de inicio e fim do ano letivo
                            dataInicioFim = rnPeriodoLetivo.ObtemDataInicioFimAulaPor(dadosEncerramentoConfVagas.Ano, dadosEncerramentoConfVagas.Periodo);

                            //Criar entidade de turma com dados Default vistos na tela de cadastro de turma
                            var turmaReal = new LyTurma
                            {
                                AulasDadas = 0,
                                MinAulas = 0,
                                DtUltalt = DateTime.Now,
                                DtInicio = Convert.ToDateTime(dataInicioFim[0]),
                                DtFim = Convert.ToDateTime(dataInicioFim[1]),
                                SitTurma = "Desativada",
                                Especial = "N",
                                UtilizaIndice = "N",
                                NivelPresenca = "Presencial",
                                ExibeSomenteListaSel = "N",
                                LancamentoHistorico = "N",
                                DtCriacao = DateTime.Now,
                                PermiteChoqueHorario = "S",
                                TipoGestao = "Estadual",
                                Classificacao = "PROJ",
                                EmElaboracao = null
                            };

                            var mensagens = new List<string>();

                            //Verifica já existe outra turma com o mesmo nome ano / periodo / censo
                            var contextQuery = new ContextQuery
                            {
                                Command =
                                    @" SELECT TOP 1
                                                                    1
                                                            FROM    DBO.LY_TURMA
                                                            WHERE   TURMA = @TURMA
                                                                    AND ANO = @ANO
                                                                    AND SEMESTRE = @SEMESTRE
                                                                    AND FACULDADE = @CENSO "
                            };

                            contextQuery.Parameters.Add("@TURMA", dadosEncerramentoConfVagas.Turma);
                            contextQuery.Parameters.Add("@ANO", dadosEncerramentoConfVagas.Ano);
                            contextQuery.Parameters.Add("@SEMESTRE", dadosEncerramentoConfVagas.Periodo);
                            contextQuery.Parameters.Add("@CENSO", dadosEncerramentoConfVagas.Censo);

                            var obj = context.GetReturnValue(contextQuery);

                            if (obj != null)
                            {
                                mensagens.Add("Já existe outra com este mesmo nome/ano/periodo/censo");
                            }

                            //Verifica já existe outra turma naquela sala / turno / ano / periodo / censo
                            contextQuery = new ContextQuery
                            {
                                Command =
                                    @" SELECT TOP 1
                                                                        1
                                                                FROM    DBO.LY_TURMA
                                                                WHERE   DEPENDENCIA = @SALA
                                                                        AND TURNO = @TURNO
                                                                        AND ANO = @ANO
                                                                        AND SEMESTRE = @SEMESTRE
                                                                        AND FACULDADE = @CENSO "
                            };

                            contextQuery.Parameters.Add("@SALA", dadosEncerramentoConfVagas.Sala);
                            contextQuery.Parameters.Add("@TURNO", dadosEncerramentoConfVagas.Turno);
                            contextQuery.Parameters.Add("@ANO", dadosEncerramentoConfVagas.Ano);
                            contextQuery.Parameters.Add("@SEMESTRE", dadosEncerramentoConfVagas.Periodo);
                            contextQuery.Parameters.Add("@CENSO", dadosEncerramentoConfVagas.Censo);

                            obj = context.GetReturnValue(contextQuery);

                            if (obj != null)
                            {
                                mensagens.Add("Já existe outra turma nesta sala/turno/ano/periodo/censo");
                            }

                            //Verifica se existe disciplinas na grade daquele curriculo / curso / turno / serie
                            contextQuery = new ContextQuery
                            {
                                Command =
                                    @" SELECT TOP 1
                                                                        1
                                                                FROM    dbo.LY_GRADE
                                                                WHERE   CURRICULO = @CURRICULO
                                                                        AND CURSO = @CURSO
                                                                        AND TURNO = @TURNO
                                                                        AND SERIE_IDEAL = @SERIE 
                                                                        AND OBRIGATORIA='S'"
                            };

                            contextQuery.Parameters.Add("@CURRICULO", dadosEncerramentoConfVagas.Curriculo);
                            contextQuery.Parameters.Add("@CURSO", dadosEncerramentoConfVagas.Curso);
                            contextQuery.Parameters.Add("@TURNO", dadosEncerramentoConfVagas.Turno);
                            contextQuery.Parameters.Add("@SERIE", dadosEncerramentoConfVagas.Serie);

                            obj = context.GetReturnValue(contextQuery);

                            if (obj == null)
                            {
                                mensagens.Add("Não existem disciplinas na grade para esta matriz curricular/curso/turno/serie");
                            }

                            if (mensagens.Count > 0)
                            {
                                //Quando houver restriçoes para criar turma nova elas devem ser cadastradas na tabela de LOG
                                foreach (var mensagem in mensagens)
                                {
                                    var log = new TceCtvLogEncerramento
                                                  {
                                                      IdAgendaConfTurnoVaga = dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga,
                                                      Censo = dadosEncerramentoConfVagas.Censo,
                                                      Turma = dadosEncerramentoConfVagas.Turma,
                                                      Restricao = mensagem,
                                                      Matricula = matricula
                                                  };

                                    CtvLogEncerramento.Inserir(context, log);
                                }
                            }
                            else
                            {
                                //Cria nova turma
                                RN.Turma.InserirTurmaLancamentoVaga(context, turmaReal, dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga, dadosEncerramentoConfVagas.IdConfVaga);

                                //cria grade_serie da turma
                                RN.Turma.InserirGradeSerieLancamentoVaga(context, turmaReal, dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga, dadosEncerramentoConfVagas.IdConfVaga, matricula);

                                //cria grade_turma da turma
                                RN.Turma.InserirGradeTurmaLancamentoVaga(context, dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga, dadosEncerramentoConfVagas.IdConfVaga);

                                //Verifica se o curso da agenda permite eletiva
                                if (rnCurso.PermiteEletivaPor(context, dadosEncerramentoConfVagas.Curso))
                                {
                                    //Verifica se a serie da matriz permite eletiva
                                    if (rnSerie.PermiteEletivaPor(context, dadosEncerramentoConfVagas.Curriculo, dadosEncerramentoConfVagas.Curso, dadosEncerramentoConfVagas.Turno, dadosEncerramentoConfVagas.Serie))
                                    {
                                        //Verifica quantidade de turmas no cenos / curso / serie / turno
                                        int qtdeTurmas = lancamentos.Where(x => x.Censo == dadosEncerramentoConfVagas.Censo && x.Turno.ToUpper() == dadosEncerramentoConfVagas.Turno.ToUpper()).Select(x => x.Turma).Distinct().Count();

                                        //Verifica se existe apenas 1
                                        if (qtdeTurmas == 1)
                                        {
                                            //Caso exista apenas 1, cria turmas eletivas
                                            RN.Dependencia rnDependencia = new Dependencia();
                                            
                                            //Busca sala alternativa para turmas eletivas
                                            string salaAlternativa = rnDependencia.ObtemSalaAlternativaVaziaPor(context, dadosEncerramentoConfVagas.Censo, dadosEncerramentoConfVagas.Turno, dadosEncerramentoConfVagas.Ano, dadosEncerramentoConfVagas.Periodo);

                                            //Cria Eletiva do Grupo 1
                                            CriarEletiva(context, turmaReal, dadosEncerramentoConfVagas, matricula, 1, salaAlternativa);

                                            //Cria Eletiva do Grupo 2
                                            CriarEletiva(context, turmaReal, dadosEncerramentoConfVagas, matricula, 2, salaAlternativa);

                                            //Cria Eletiva do Grupo 3
                                            CriarEletiva(context, turmaReal, dadosEncerramentoConfVagas, matricula, 3, salaAlternativa);
                                        }
                                    }
                                }
                            }
                        }

                        //Encerra a agenda
                        Encerrar(context, agenda);
                    }

                    //Inserir resultado final na tabela controle de vagas
                    RN.ControleVaga.Inserir(context, idAgendas, matricula);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        private static void CriarEletiva(DataContext context, LyTurma turmaReal, DadosEncerramentoConfVagas dadosEncerramentoConfVagas, string usuarioId, int grupo, string salaAlternativa)
        {
            List<string> mensagens = new List<string>();
            string turmaEletiva = string.Empty;
            RN.Curriculo rnCurriculo = new Curriculo();
            RN.Serie rnSerie = new Serie();
            RN.Turma rnTurma = new Turma();

            //Busca matriz curricular da eletiva (Curso 9999.80 e grupo, no mesmo turno, serie, ano e período da turma criada)
            string curriculoEletiva = rnCurriculo.ObtemPrimeiroCurriculoEletivaAtivoPor(context, grupo, dadosEncerramentoConfVagas.Turno, dadosEncerramentoConfVagas.Serie, dadosEncerramentoConfVagas.Ano, dadosEncerramentoConfVagas.Periodo);

            //Verifica se existe disciplinas na grade daquele curriculo / curso / turno / serie
            var contextQuery = new ContextQuery
            {
                Command =
                    @" SELECT TOP 1
                                1
                        FROM    dbo.LY_GRADE
                        WHERE   CURRICULO = @CURRICULO
                                AND CURSO = @CURSOELETIVA
                                AND TURNO = @TURNO
                                AND SERIE_IDEAL = @SERIE 
                                AND OBRIGATORIA= 'S' "
            };

            contextQuery.Parameters.Add("@CURRICULO", curriculoEletiva);
            contextQuery.Parameters.Add("@CURSOELETIVA", "9999.80");
            contextQuery.Parameters.Add("@TURNO", dadosEncerramentoConfVagas.Turno);
            contextQuery.Parameters.Add("@SERIE", dadosEncerramentoConfVagas.Serie);

            var obj = context.GetReturnValue(contextQuery);

            if (obj == null)
            {
                mensagens.Add(string.Format("ELETIVA: Não existe disciplina eletiva grupo {0} na matriz do curso/turno/serie", grupo));
            }
            else
            {
                //Busca Prefixo a turma referencia e da eletiva
                string prefixoTurma = rnSerie.ObtemPrefixoSeriePor(context, dadosEncerramentoConfVagas.Curso, dadosEncerramentoConfVagas.Turno, dadosEncerramentoConfVagas.Curriculo, dadosEncerramentoConfVagas.Serie);
                string prefixoEletiva = rnSerie.ObtemPrefixoSeriePor(context, "9999.80", dadosEncerramentoConfVagas.Turno, curriculoEletiva, dadosEncerramentoConfVagas.Serie);

                //Cria nome da turma eletiva
                string complemento = dadosEncerramentoConfVagas.Turma;
                turmaEletiva = prefixoEletiva + complemento;

                //Verifica já existe outra turma com o mesmo nome ano / periodo / censo
                contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                        1
                                FROM    DBO.LY_TURMA
                                WHERE   TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE "
                };

                contextQuery.Parameters.Add("@TURMA", turmaEletiva);
                contextQuery.Parameters.Add("@ANO", dadosEncerramentoConfVagas.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", dadosEncerramentoConfVagas.Periodo);

                obj = context.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    mensagens.Add(string.Format("ELETIVA: Já existe outra eletiva do grupo {0} com este mesmo nome/ano/periodo/censo", grupo));
                }
            }

            //Valida se já existe outra Eletiva do mesmo grupo para a turma referencia
            var turmasExistentes = rnTurma.ObtemTurmaEletivaPor(context, Convert.ToDecimal(dadosEncerramentoConfVagas.Ano), Convert.ToDecimal(dadosEncerramentoConfVagas.Periodo), dadosEncerramentoConfVagas.Turma, grupo);
            if (turmasExistentes != null && turmasExistentes.Count > 0)
            {
                mensagens.Add(string.Format("ELETIVA: Já existe outra eletiva do grupo {0} para esta turma referencia", grupo));
            }

            if(salaAlternativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add(string.Format("ELETIVA: Não existe Sala Alternativa vaga no turno para a turma eletiva", grupo));
            }

            if (mensagens.Count > 0)
            {
                //Quando houver restriçoes para criar turma nova elas devem ser cadastradas na tabela de LOG
                foreach (var mensagem in mensagens)
                {
                    var log = new TceCtvLogEncerramento
                    {
                        IdAgendaConfTurnoVaga = dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga,
                        Censo = dadosEncerramentoConfVagas.Censo,
                        Turma = dadosEncerramentoConfVagas.Turma,
                        Restricao = mensagem,
                        Matricula = usuarioId
                    };

                    CtvLogEncerramento.Inserir(context, log);
                }
            }
            else
            {
                //Cria nova turma
                RN.Turma.InserirTurmaLancamentoVagaEletiva(context, turmaReal, dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga, dadosEncerramentoConfVagas.IdConfVaga, turmaEletiva, curriculoEletiva, salaAlternativa);

                //cria grade_serie da turma
                RN.Turma.InserirGradeSerieLancamentoVagaEletiva(context, turmaReal, dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga, dadosEncerramentoConfVagas.IdConfVaga, usuarioId, turmaEletiva, curriculoEletiva, salaAlternativa);

                //cria grade_turma da turma
                RN.Turma.InserirGradeTurmaLancamentoVagaEletiva(context, dadosEncerramentoConfVagas.IdAgendaConfTurnoVaga, dadosEncerramentoConfVagas.IdConfVaga, turmaEletiva, curriculoEletiva);
            }
        }

        public static void Encerrar(DataContext context, int idAgenda)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                    SET     ENCERRADO = 1 ,
                            DT_ENCERRAMENTO = GETDATE()
                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ");

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

            context.ApplyModifications(contextQuery);
        }

        public static QueryTable ListarAno(string perfil)
        {
            var sql = new StringBuilder();

            sql.Append(
                @"SELECT DISTINCT
                                        ANO
                                FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                                --WHERE ENCERRADO = 0 --retirado a pedido da fernanda
                                WHERE 1 = 1");

            if (perfil == "DIRETOR_UE")
            {
                sql.Append(
                    @" AND (CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO_CONF_TURNO
                                                                   AND     DT_FIM_CONF_TURNO
                                          OR CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO_CONF_VAGAS
                                                                      AND     DT_FIM_CONF_VAGAS
                                        )");
            }
            sql.Append(" ORDER BY ANO DESC");

            return Consultar(sql.ToString());
        }

        public static QueryTable ListarPeriodos(string perfil, int ano)
        {
            var sql = new StringBuilder();

            sql.Append(
                @"SELECT DISTINCT
                                        PERIODO
                                FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                                --WHERE ENCERRADO = 0 --retirado a pedido da fernanda
                                WHERE ANO = ? ");

            if (perfil == "DIRETOR_UE")
            {
                sql.Append(
                    @" AND (CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO_CONF_TURNO
                                                                   AND     DT_FIM_CONF_TURNO
                                          OR CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO_CONF_VAGAS
                                                                      AND     DT_FIM_CONF_VAGAS
                                        )");
            }
            sql.Append(" ORDER BY PERIODO ");

            return Consultar(sql.ToString(), ano);
        }

        public DataTable ListaPeriodosParaHistoricoPor(int ano)
        {
            DataTable dtPeridos = new DataTable();
            List<int> periodos = new List<int>();
            int periodo = -1;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            dtPeridos.Columns.Add("Periodos");
            dtPeridos.Columns.Add("Descricao");

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT PERIODO
                            FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO ";

                contextQuery.Parameters.Add("@ANO", ano);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    periodo = Convert.ToInt32(reader["PERIODO"]);
                    periodos.Add(periodo);
                }

                if (periodos.Contains(0) && periodos.Contains(1))
                {
                    dtPeridos.Rows.Add("0, 1", "Anual e 1º Semestre");
                }

                if (periodos.Contains(2))
                {
                    dtPeridos.Rows.Add("2", "2º Semestre");
                }

                return dtPeridos;
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

        public static bool PossuiApenasAgendasEncerradas(int ano)
        {
            int total = 0;
            int encerradas = 0;

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT count(*)
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA 
                        WHERE ANO = @ANO ");

                contextQuery.Parameters.Add("@ANO", ano);

                total = ctx.GetReturnValue<int>(contextQuery);

            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT count(*)
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA 
                        WHERE ANO = @ANO 
                        AND ENCERRADO=1");

                contextQuery.Parameters.Add("@ANO", ano);

                encerradas = ctx.GetReturnValue<int>(contextQuery);

            }

            return (total == encerradas);
        }

        public bool EhEncerrada(int idAgendaConfTurnoVaga)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool encerrada = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  ENCERRADO
                                FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                                WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA "
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaConfTurnoVaga);

                encerrada = ctx.GetReturnValue<bool>(contextQuery);

                return encerrada;
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

        public List<int> ObtemListaIdsAgendaConfTurnoVagaPor(FiltroRestricaoTerminalidade filtro)
        {
            List<int> listaIdsAgendas = new List<int>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int idAgenda = 0;

            try
            {
                contextQuery.Command = @" SELECT  ID_AGENDA_CONF_TURNO_VAGA
                        FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND CURSO = ISNULL(@CURSO, CURSO)
                                AND ( SERIE = @SERIE
                                      OR @SERIE = -1
                                    ) ";

                contextQuery.Parameters.Add("@ANO", filtro.Ano);
                contextQuery.Parameters.Add("@PERIODO", filtro.Periodo);
                contextQuery.Parameters.Add("@CURSO", filtro.PorCurso ? filtro.Curso : null);
                contextQuery.Parameters.Add("@SERIE", filtro.PorSerie ? filtro.Serie : -1);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    idAgenda = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]);
                    listaIdsAgendas.Add(idAgenda);
                }

                return listaIdsAgendas;
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

        public bool EhCursoPermitidoPor(int idAgendaConfTurnoVaga, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool permitido = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN LY_UNIDADE_ENSINO_CURSOS UEC ON A.CURSO = UEC.CURSO
                        WHERE   A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND UEC.UNIDADE_ENS = @UNIDADE_ENS ";

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@UNIDADE_ENS", censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    permitido = true;
                }

                return permitido;
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

        public DataTable ObtemListaSeriesComAgendaPor(int ano, int periodo, string curso, string unidadeEnsino, string municipio, string regional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT
                                                A.SERIE
                                     FROM TCE_CTV_AGENDA_CONF_TURNO_VAGA A (NOLOCK)
                                     INNER JOIN LY_SERIE                 LS (NOLOCK) ON  LS.CURSO                      = A.CURSO 
                                                                                    AND (LS.DT_EXTINCAO               IS NULL
                                                                                     OR  CONVERT(DATE, LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                                     
                                     INNER JOIN LY_UNIDADE_ENSINO_CURSOS UC (NOLOCK) ON LS.CURSO       = UC.CURSO

                                    ");
                if (!string.IsNullOrEmpty(municipio) || !string.IsNullOrEmpty(regional))
                {
                    sql.Append(" INNER JOIN DBO.LY_UNIDADE_ENSINO UE (NOLOCK) ON UC.UNIDADE_ENS = UE.UNIDADE_ENS");
                }

                sql.Append(@"  WHERE   A.ANO = @ANO
                                                AND A.PERIODO = @PERIODO
                                                AND A.CURSO = @CURSO                                              
                                         ");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);

                if (!string.IsNullOrEmpty(unidadeEnsino))
                {
                    sql.Append(" AND UC.UNIDADE_ENS = @UNIDADE_ENS ");
                    contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);
                }

                if (!string.IsNullOrEmpty(municipio))
                {
                    sql.Append(" AND UE.MUNICIPIO = @MUNICIPIO ");
                    contextQuery.Parameters.Add("@MUNICIPIO", municipio);
                }

                if (!string.IsNullOrEmpty(regional))
                {
                    sql.Append(" AND UE.ID_REGIONAL = @ID_REGIONAL ");
                    contextQuery.Parameters.Add("@ID_REGIONAL", regional);
                }

                sql.Append(" ORDER BY A.SERIE ");

                contextQuery.Command = sql.ToString();

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public bool PossuiAgendaEncerradaPor(FiltroRestricaoTerminalidade filtro)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool existe = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND CURSO = ISNULL(@CURSO, CURSO)
                                AND ( SERIE = @SERIE
                                      OR @SERIE = -1
                                    ) 
                                AND ENCERRADO = 1 "
                };

                contextQuery.Parameters.Add("@ANO", filtro.Ano);
                contextQuery.Parameters.Add("@PERIODO", filtro.Periodo);
                contextQuery.Parameters.Add("@CURSO", filtro.PorCurso ? filtro.Curso : null);
                contextQuery.Parameters.Add("@SERIE", filtro.PorSerie ? filtro.Serie : -1);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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