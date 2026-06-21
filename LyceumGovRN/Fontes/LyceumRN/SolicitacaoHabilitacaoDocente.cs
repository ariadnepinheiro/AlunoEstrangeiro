namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Library;
    using Techne.Lyceum.RN.DTOs;
    using Techne.Lyceum.RN.Entidades;
    using Util;

    public class SolicitacaoHabilitacaoDocente : RNBase
    {
        private const string Aprovacao = "Aprovado";

        private const string Reprovacao = "Reprovado";

        public static RetValue Aprovar(IList<StatusSolicitacaoHabilitacaoDocente> statusSolicitacaoHabilitacaoDocentes)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();

            foreach (var statusSolicitacaoHabilitacaoDocente in statusSolicitacaoHabilitacaoDocentes)
            {
                statusSolicitacaoHabilitacaoDocente.Status = Aprovacao;
            }

            try
            {
                connection.Open(true);

                // Altera o status da solicitação
                retorno = AlterarStatus(connection, statusSolicitacaoHabilitacaoDocentes);

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                // Insere a habilitação
                var habilitacoesInseridas = RN.GrupoHabilitacaoDoc.IncluirGrupoDeHabilitacaoDocentePorSolicitacao(connection, statusSolicitacaoHabilitacaoDocentes.Select(x => x.IdSolicitacaoHabilitacaoDocente));

                if (habilitacoesInseridas != statusSolicitacaoHabilitacaoDocentes.Count())
                {
                    connection.Rollback();

                    retorno = new RetValue(false, null, new ErrorList("Falha na inserção das habilitações do docente."));
                }
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue Incluir(TceSolicitacaoHabilitacaoDocente solicitacaoHabilitacaoDocente)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();

            var sql = @"INSERT INTO [dbo].[TCE_SOLICITACAO_HABILITACAO_DOCENTE]
                               ([UNIDADE_ENS]
                               ,[NUM_FUNC]
                               ,[SEGMENTO_ATUACAO]
                               ,[AGRUPAMENTO]
                               ,[HABILITACAO_MATRICULA]
                               ,[HABILITACAO_GLP]
                               ,[NUM_FUNC_SUBSTITUIDO]
                               ,[TIPO_SUBSTITUICAO]
                               ,[STATUS])
                         VALUES
                               (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            try
            {
                connection.Open(true);

                TCommand.ExecuteNonQuery(
                            connection,
                            sql,
                            solicitacaoHabilitacaoDocente.UnidadeEns,
                            solicitacaoHabilitacaoDocente.NumFunc,
                            solicitacaoHabilitacaoDocente.SegmentoAtuacao,
                            solicitacaoHabilitacaoDocente.Agrupamento,
                            solicitacaoHabilitacaoDocente.HabilitacaoMatricula ? "S" : "N",
                            solicitacaoHabilitacaoDocente.HabilitacaoGLP ? "S" : "N",
                            solicitacaoHabilitacaoDocente.NumFuncSubstituido,
                            solicitacaoHabilitacaoDocente.TipoSubstituicao,
                            "Aguardando");

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                retorno = new RetValue(true, "Solicitação de Habilitação incluída com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static QueryTable ListarPorUA(string setor, string status)
        {
            if (string.IsNullOrEmpty(setor))
            {
                return null;
            }

            var sql = @"SELECT  shd.*,
                                n.REGIONAL AS 'REGIONAL',
                                m.NOME as 'MUNICIPIO',
                                ue.NOME_COMP AS 'ESCOLA',
                                PE.NOME_COMPL AS 'NOME',
                                d.MATRICULA AS 'MATRICULA',
                                d.CATEGORIA AS 'CARGO',
                                ISNULL(F.DESCRICAO, '') AS 'FUNCAO',
                                gh.DESCRICAO AS 'DISCIPLINA_INGRESSO',
                                case 
									when HABILITACAO_MATRICULA = 'N' AND HABILITACAO_GLP = 'S' then 'GLP'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'N' THEN 'Matricula'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'S' THEN 'Matricula / GLP'
									END 'HABILITACAO_MATRICULA_GLP',
                                d2.MATRICULA AS 'MATRICULA_SUBSTITUIDA',
                                gh2.DESCRICAO AS 'DISCIPLINA_HABILITAR',
                           case when (select distinct 1 as 'aula'  
                            from LY_AULA_DOCENTE ad WITH ( NOLOCK )  
                              join LY_TURMA t WITH ( NOLOCK ) on  
                                ad.ANO = t.ANO  
                                AND ad.SEMESTRE = t.SEMESTRE  
                                 and ad.DISCIPLINA = t.DISCIPLINA  
                                 and ad.FACULDADE = t.FACULDADE  
                                 and ad.TURMA = t.TURMA  
                                 and ad.TURNO = t.TURNO   
                                 and ad.DATA_FIM = t.DT_FIM  
                                join LY_CURSO c WITH ( NOLOCK ) on c.CURSO=t.CURSO  	
                                join LY_DOCENTE d WITH ( NOLOCK ) on d.NUM_FUNC = ad.NUM_FUNC  
                                 where ad.NUM_FUNC = shd.NUM_FUNC  
                                and t.SIT_TURMA = 'Aberta'  
                                and ad.DATA_FIM >= convert(date,GETDATE())    
                                 and t.ANO = YEAR(GETDATE())) = 1 then 'SIM'  
                             else 'NÃO' 
                        END em_aula,
                        (CONVERT (VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT (VARCHAR, D.VINCULO)) AS IDVINCULO
                        FROM    dbo.TCE_SOLICITACAO_HABILITACAO_DOCENTE shd WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON shd.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN dbo.TCE_REGIONAL n WITH ( NOLOCK )
                                                                ON ue.ID_REGIONAL = n.ID_REGIONAL
                                INNER JOIN dbo.MUNICIPIO M WITH ( NOLOCK )
                                                                ON ue.MUNICIPIO = m.CODIGO
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                 ON shd.NUM_FUNC = d.NUM_FUNC
                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                 ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_LOTACAO l WITH ( NOLOCK )
                                                                ON d.MATRICULA = l.MATRICULA
                                                                   AND (
                                                                         l.DATA_DESATIVACAO IS NULL
                                                                         OR l.DATA_DESATIVACAO > GETDATE()
                                                                       )
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO_DOC ghd WITH ( NOLOCK )
                                                                                 ON d.NUM_FUNC = ghd.NUM_FUNC
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh WITH ( NOLOCK )
                                                                                   ON ghd.AGRUPAMENTO = gh.AGRUPAMENTO
                                LEFT JOIN dbo.LY_FUNCAO F WITH ( NOLOCK )
                                                                                   ON F.FUNCAO = L.FUNCAO
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh2 WITH ( NOLOCK )
                                                                                   ON shd.AGRUPAMENTO = gh2.AGRUPAMENTO
                                LEFT JOIN dbo.LY_DOCENTE d2 WITH ( NOLOCK )
                                                                  ON shd.NUM_FUNC_SUBSTITUIDO = d2.NUM_FUNC
                        WHERE   AGRUPAMENTO_INGRESSO = 'S'AND ghd.PROVISORIO='N'
                                AND ue.SETOR = ? ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND [STATUS] = ? ";
                sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO ";

                return Consultar(sql, setor, status);
            }

            sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO ";

            return Consultar(sql, setor);
        }

        public static QueryTable ListarPorNucleo(string id_regional, string status)
        {
            if (string.IsNullOrEmpty(id_regional))
            {
                return null;
            }

            var sql = @"SELECT  shd.*,
                                n.REGIONAL AS 'REGIONAL',
                                m.NOME as 'MUNICIPIO',
                                ue.NOME_COMP AS 'ESCOLA',
                                PE.NOME_COMPL AS 'NOME',
                                d.MATRICULA AS 'MATRICULA',
                                d.CATEGORIA AS 'CARGO',
                                ISNULL(F.DESCRICAO, '') AS 'FUNCAO',
                                gh.DESCRICAO AS 'DISCIPLINA_INGRESSO',
                                case 
									when HABILITACAO_MATRICULA = 'N' AND HABILITACAO_GLP = 'S' then 'GLP'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'N' THEN 'Matricula'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'S' THEN 'Matricula / GLP'
									END 'HABILITACAO_MATRICULA_GLP',
                                d2.MATRICULA AS 'MATRICULA_SUBSTITUIDA',
                                gh2.DESCRICAO AS 'DISCIPLINA_HABILITAR',
                        case when (select distinct 1 as 'aula'  
                            from LY_AULA_DOCENTE ad WITH ( NOLOCK )  
                              join LY_TURMA t WITH ( NOLOCK ) on  
                                ad.ANO = t.ANO  
                                AND ad.SEMESTRE = t.SEMESTRE  
                                 and ad.DISCIPLINA = t.DISCIPLINA  
                                 and ad.FACULDADE = t.FACULDADE  
                                 and ad.TURMA = t.TURMA  
                                 and ad.TURNO = t.TURNO   
                                 and ad.DATA_FIM = t.DT_FIM  
                                join LY_CURSO c WITH ( NOLOCK ) on c.CURSO=t.CURSO  	
                                join LY_DOCENTE d WITH ( NOLOCK ) on d.NUM_FUNC = ad.NUM_FUNC  
                                 where ad.NUM_FUNC = shd.NUM_FUNC  
                                and t.SIT_TURMA = 'Aberta'  
                                and ad.DATA_FIM >= convert(date,GETDATE())    
                                 and t.ANO = YEAR(GETDATE())) = 1 then 'SIM'  
                             else 'NÃO' 
                        END em_aula,
                        n.REGIONAL AS 'COORDENADORIA',
                        (CONVERT (VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT (VARCHAR, D.VINCULO)) AS IDVINCULO
                        FROM    dbo.TCE_SOLICITACAO_HABILITACAO_DOCENTE shd WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON shd.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN dbo.TCE_REGIONAL n WITH ( NOLOCK )
                                                                ON ue.ID_REGIONAL = n.ID_REGIONAL
                                INNER JOIN dbo.MUNICIPIO M WITH ( NOLOCK )
                                                                ON ue.MUNICIPIO = m.CODIGO
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                 ON shd.NUM_FUNC = d.NUM_FUNC

                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                 ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_LOTACAO l WITH ( NOLOCK )
                                                                ON d.MATRICULA = l.MATRICULA
                                                                   AND (
                                                                         l.DATA_DESATIVACAO IS NULL
                                                                         OR l.DATA_DESATIVACAO > GETDATE()
                                                                       )
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO_DOC ghd WITH ( NOLOCK )
                                                                                 ON d.NUM_FUNC = ghd.NUM_FUNC
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh WITH ( NOLOCK )
                                                                                   ON ghd.AGRUPAMENTO = gh.AGRUPAMENTO
                                LEFT JOIN dbo.LY_FUNCAO F WITH ( NOLOCK )
                                                                                   ON F.FUNCAO = L.FUNCAO
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh2 WITH ( NOLOCK )
                                                                                   ON shd.AGRUPAMENTO = gh2.AGRUPAMENTO
                                LEFT JOIN dbo.LY_DOCENTE d2 WITH ( NOLOCK )
                                                                  ON shd.NUM_FUNC_SUBSTITUIDO = d2.NUM_FUNC
                        WHERE   AGRUPAMENTO_INGRESSO = 'S' 
                                AND ghd.PROVISORIO = 'N'
                                AND n.ID_REGIONAL = ? ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND [STATUS] = ?";
                sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO";

                return Consultar(sql, id_regional, status);
            }

            sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO";

            return Consultar(sql, id_regional);
        }

        public static QueryTable ListarPorMunicipio(string municipio, string status)
        {
            if (string.IsNullOrEmpty(municipio))
            {
                return null;
            }

            var sql = @"SELECT  shd.*,
                                n.REGIONAL AS 'REGIONAL',
                                m.NOME as 'MUNICIPIO',
                                ue.NOME_COMP AS 'ESCOLA',
                                PE.NOME_COMPL AS 'NOME',
                                d.MATRICULA AS 'MATRICULA',
                                d.CATEGORIA AS 'CARGO',
                                ISNULL(F.DESCRICAO, '') AS 'FUNCAO',
                                gh.DESCRICAO AS 'DISCIPLINA_INGRESSO',
                                case 
									when HABILITACAO_MATRICULA = 'N' AND HABILITACAO_GLP = 'S' then 'GLP'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'N' THEN 'Matricula'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'S' THEN 'Matricula / GLP'
									END 'HABILITACAO_MATRICULA_GLP',
                                d2.MATRICULA AS 'MATRICULA_SUBSTITUIDA',
                                gh2.DESCRICAO AS 'DISCIPLINA_HABILITAR',
                        case when (select distinct 1 as 'aula'  
                            from LY_AULA_DOCENTE ad WITH ( NOLOCK )  
                              join LY_TURMA t WITH ( NOLOCK ) on  
                                ad.ANO = t.ANO  
                                AND ad.SEMESTRE = t.SEMESTRE  
                                 and ad.DISCIPLINA = t.DISCIPLINA  
                                 and ad.FACULDADE = t.FACULDADE  
                                 and ad.TURMA = t.TURMA  
                                 and ad.TURNO = t.TURNO   
                                 and ad.DATA_FIM = t.DT_FIM  
                                join LY_CURSO c WITH ( NOLOCK ) on c.CURSO=t.CURSO  	
                                join LY_DOCENTE d WITH ( NOLOCK ) on d.NUM_FUNC = ad.NUM_FUNC  
                                 where ad.NUM_FUNC = shd.NUM_FUNC  
                                and t.SIT_TURMA = 'Aberta'  
                                and ad.DATA_FIM >= convert(date,GETDATE())    
                                 and t.ANO = YEAR(GETDATE())) = 1 then 'SIM'  
                             else 'NÃO' 
                        END em_aula,
                        (CONVERT (VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT (VARCHAR, D.VINCULO)) AS IDVINCULO
                        FROM    dbo.TCE_SOLICITACAO_HABILITACAO_DOCENTE shd WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON shd.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN dbo.TCE_REGIONAL n WITH ( NOLOCK )
                                                                ON ue.ID_REGIONAL = n.ID_REGIONAL
                                INNER JOIN dbo.MUNICIPIO M WITH ( NOLOCK )
                                                                ON ue.MUNICIPIO = m.CODIGO
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                 ON shd.NUM_FUNC = d.NUM_FUNC
                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                 ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_LOTACAO l WITH ( NOLOCK )
                                                                ON d.MATRICULA = l.MATRICULA
                                                                   AND (
                                                                         l.DATA_DESATIVACAO IS NULL
                                                                         OR l.DATA_DESATIVACAO > GETDATE()
                                                                       )
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO_DOC ghd WITH ( NOLOCK )
                                                                                 ON d.NUM_FUNC = ghd.NUM_FUNC
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh WITH ( NOLOCK )
                                                                                   ON ghd.AGRUPAMENTO = gh.AGRUPAMENTO
                                LEFT JOIN dbo.LY_FUNCAO F WITH ( NOLOCK )
                                                                                   ON F.FUNCAO = L.FUNCAO
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh2 WITH ( NOLOCK )
                                                                                   ON shd.AGRUPAMENTO = gh2.AGRUPAMENTO
                                LEFT JOIN dbo.LY_DOCENTE d2 WITH ( NOLOCK )
                                                                  ON shd.NUM_FUNC_SUBSTITUIDO = d2.NUM_FUNC
                        WHERE   AGRUPAMENTO_INGRESSO = 'S'
                                AND ghd.PROVISORIO = 'N'
                                AND ue.MUNICIPIO = ? ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND [STATUS] = ?";
                sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO";

                return Consultar(sql, municipio, status);
            }

            sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO";

            return Consultar(sql, municipio);
        }

        public static QueryTable ListarPorCenso(string censo, string status)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            var sql = @"SELECT  shd.*,
                                n.REGIONAL AS 'REGIONAL',
                                m.NOME as 'MUNICIPIO',
                                ue.NOME_COMP AS 'ESCOLA',
                                PE.NOME_COMPL AS 'NOME',
                                d.MATRICULA AS 'MATRICULA',
                                d.CATEGORIA AS 'CARGO',
                                ISNULL(F.DESCRICAO, '') AS 'FUNCAO',
                                gh.DESCRICAO AS 'DISCIPLINA_INGRESSO',
                                case 
									when HABILITACAO_MATRICULA = 'N' AND HABILITACAO_GLP = 'S' then 'GLP'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'N' THEN 'Matricula'
									WHEN HABILITACAO_MATRICULA = 'S' AND HABILITACAO_GLP = 'S' THEN 'Matricula / GLP'
									END 'HABILITACAO_MATRICULA_GLP',
                                d2.MATRICULA AS 'MATRICULA_SUBSTITUIDA',
                                gh2.DESCRICAO AS 'DISCIPLINA_HABILITAR',
                                 case when (select distinct 1 as 'aula'  
                            from LY_AULA_DOCENTE ad WITH ( NOLOCK )  
                              join LY_TURMA t WITH ( NOLOCK ) on  
                                ad.ANO = t.ANO  
                                AND ad.SEMESTRE = t.SEMESTRE  
                                 and ad.DISCIPLINA = t.DISCIPLINA  
                                 and ad.FACULDADE = t.FACULDADE  
                                 and ad.TURMA = t.TURMA  
                                 and ad.TURNO = t.TURNO   
                                 and ad.DATA_FIM = t.DT_FIM  
                                join LY_CURSO c WITH ( NOLOCK ) on c.CURSO=t.CURSO  	
                                join LY_DOCENTE d WITH ( NOLOCK ) on d.NUM_FUNC = ad.NUM_FUNC  
                                 where ad.NUM_FUNC = shd.NUM_FUNC  
                                and t.SIT_TURMA = 'Aberta'  
                                and ad.DATA_FIM >= convert(date,GETDATE())    
                                 and t.ANO = YEAR(GETDATE())) = 1 then 'SIM'  
                             else 'NÃO' 
                        END em_aula,
                        ue.ID_REGIONAL AS 'COORDENADORIA',
                        (CONVERT (VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT (VARCHAR, D.VINCULO)) AS IDVINCULO
                        FROM    dbo.TCE_SOLICITACAO_HABILITACAO_DOCENTE shd WITH ( NOLOCK )
                                INNER JOIN dbo.LY_UNIDADE_ENSINO ue WITH ( NOLOCK )
                                                                         ON shd.UNIDADE_ENS = ue.UNIDADE_ENS
                                INNER JOIN dbo.TCE_REGIONAL n WITH ( NOLOCK )
                                                                ON ue.ID_REGIONAL = n.ID_REGIONAL
                                INNER JOIN dbo.MUNICIPIO M WITH ( NOLOCK )
                                                                ON ue.MUNICIPIO = m.CODIGO
                                INNER JOIN dbo.LY_DOCENTE d WITH ( NOLOCK )
                                                                 ON shd.NUM_FUNC = d.NUM_FUNC
                                INNER JOIN dbo.LY_PESSOA PE WITH ( NOLOCK )
                                                                 ON PE.PESSOA = D.PESSOA
                                LEFT JOIN dbo.LY_LOTACAO l WITH ( NOLOCK )
                                                                ON d.MATRICULA = l.MATRICULA
                                                                   AND (
                                                                         l.DATA_DESATIVACAO IS NULL
                                                                         OR l.DATA_DESATIVACAO > GETDATE()
                                                                       )
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO_DOC ghd WITH ( NOLOCK )
                                                                                 ON d.NUM_FUNC = ghd.NUM_FUNC
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh WITH ( NOLOCK )
                                                                                   ON ghd.AGRUPAMENTO = gh.AGRUPAMENTO
                                INNER JOIN dbo.LY_GRUPO_HABILITACAO gh2 WITH ( NOLOCK )
                                                                                   ON shd.AGRUPAMENTO = gh2.AGRUPAMENTO
                                LEFT JOIN dbo.LY_FUNCAO F WITH ( NOLOCK )
                                                                                   ON F.FUNCAO = L.FUNCAO
                                LEFT JOIN dbo.LY_DOCENTE d2 WITH ( NOLOCK )
                                                                  ON shd.NUM_FUNC_SUBSTITUIDO = d2.NUM_FUNC
                        WHERE   AGRUPAMENTO_INGRESSO = 'S'
                                AND ghd.PROVISORIO = 'N'
                                AND ue.UNIDADE_ENS = ? ";

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND [STATUS] = ?";
                sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO";

                return Consultar(sql, censo, status);
            }

            sql += " ORDER BY n.REGIONAL,m.NOME,ue.NOME_COMP,PE.NOME_COMPL,DATA_CADASTRO";

            return Consultar(sql, censo);
        }

        public static RetValue Remover(int idSolicitacaoHabilitacaoDocente)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();
            var sql = @"DELETE
                        FROM    [dbo].[TCE_SOLICITACAO_HABILITACAO_DOCENTE]
                        WHERE   [ID_SOLICITACAO_HABILITACAO_DOCENTE] = ?
                                AND [STATUS] = 'Aguardando'";

            try
            {
                connection.Open(true);

                TCommand.ExecuteNonQuery(connection, sql, idSolicitacaoHabilitacaoDocente);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                }
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue Reprovar(IList<StatusSolicitacaoHabilitacaoDocente> statusSolicitacaoHabilitacaoDocentes)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();

            foreach (var statusSolicitacaoHabilitacaoDocente in statusSolicitacaoHabilitacaoDocentes)
            {
                statusSolicitacaoHabilitacaoDocente.Status = Reprovacao;
            }

            try
            {
                connection.Open(true);

                retorno = AlterarStatus(connection, statusSolicitacaoHabilitacaoDocentes);

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                }
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static ValidacaoDados ValidarAprovacao(string idSolicitacao)
        {
            var validacao = new ValidacaoDados();

            var sql = @" select DISTINCT 1 
                from ly_grupo_habilitacao_doc g 
                inner join LY_DOCENTE d 
                on g.NUM_FUNC = d.NUM_FUNC 
                inner join TCE_SOLICITACAO_HABILITACAO_DOCENTE h
                on h.AGRUPAMENTO = g.AGRUPAMENTO
                and h.NUM_FUNC = g.NUM_FUNC
                where PROVISORIO = 'N'
                and ID_SOLICITACAO_HABILITACAO_DOCENTE = ? ";

            var retorno = ExecutarFuncao(sql, idSolicitacao);

            if (retorno == 1)
            {
                validacao.Valido = false; //possui habilitação naquela disciplina
                validacao.Mensagem = "Esta disciplina já esta habilitada para este docente, favor reprovar";

                return validacao;
            }

            validacao.Valido = true;

            return validacao;
        }

        public static ValidacaoDados Validar(TceSolicitacaoHabilitacaoDocente solicitacaoHabilitacaoDocente)
        {
            var validacao = new ValidacaoDados();
            var sql = @"SELECT DISTINCT 1
                        FROM   [dbo].[TCE_SOLICITACAO_HABILITACAO_DOCENTE]
                        WHERE  [NUM_FUNC] = ?
                               AND [STATUS] <> 'Reprovado' 
                               AND [AGRUPAMENTO] = ?";

            var retorno = ExecutarFuncao(sql, solicitacaoHabilitacaoDocente.NumFunc, solicitacaoHabilitacaoDocente.Agrupamento);

            if (retorno == 1)
            {
                // Possui aulas alocadas no período
                validacao.Valido = false;
                validacao.Mensagem = "Já existe solicitação Pendente/Aprovada nesta disciplina.";

                return validacao;
            }

            //            if (!string.IsNullOrEmpty(solicitacaoHabilitacaoDocente.TipoSubstituicao))
            //            {
            //                sql = @"select distinct 1
            //                        from LY_AULA_DOCENTE ad
            //                        join LY_TURMA t on 
            //                        ad.ANO = t.ANO
            //                        AND ad.SEMESTRE = t.SEMESTRE
            //                        and ad.DISCIPLINA = t.DISCIPLINA
            //                        and ad.FACULDADE = t.FACULDADE
            //                        and ad.TURMA = t.TURMA
            //                        and ad.TURNO = t.TURNO 
            //                        and ad.DATA_FIM = t.DT_FIM
            //                        join LY_CURSO c on c.CURSO=t.CURSO 	
            //                        join LY_DOCENTE d on d.NUM_FUNC = ad.NUM_FUNC 	
            //                        where ad.NUM_FUNC = ?
            //                        and t.SIT_TURMA = 'Aberta' 
            //                        and ad.DATA_FIM >= convert(date,GETDATE())
            //                        and c.TIPO = 1
            //                        and MATRICULA in (
            //					                        select matricula from dbo.LY_LOTACAO l WITH ( NOLOCK )
            //					                        where FUNCAO in (108,109,10001)
            //					                        AND (l.DATA_DESATIVACAO IS NULL OR l.DATA_DESATIVACAO > GETDATE() ) )";

            //                retorno = ExecutarFuncao(sql, solicitacaoHabilitacaoDocente.NumFunc);

            //                if (retorno != 1)
            //                {
            //                    validacao.Valido = false; //possui aulas alocadas no período
            //                    validacao.Mensagem = "Somente poderá habilitar Matrícula os regentes em Anos Iniciais e está em função DOC II";
            //                    return validacao;
            //                }
            //            }

            validacao.Valido = true;

            return validacao;
        }

        private static RetValue AlterarStatus(TConnectionWritable connection, IEnumerable<StatusSolicitacaoHabilitacaoDocente> statusSolicitacaoHabilitacaoDocentes)
        {
            RetValue retorno = null;
            var sql = @"UPDATE  [dbo].[TCE_SOLICITACAO_HABILITACAO_DOCENTE]
                        SET     [STATUS] = ?,
                                [MOTIVO] = ?,
                                [DATA_ANALISE] = ?
                        WHERE   [ID_SOLICITACAO_HABILITACAO_DOCENTE] = ?";

            try
            {
                foreach (var statusSolicitacaoHabilitacaoDocente in statusSolicitacaoHabilitacaoDocentes)
                {
                    if (statusSolicitacaoHabilitacaoDocente.Status == Aprovacao)
                    {
                        TCommand.ExecuteNonQuery(
                                    connection,
                                    sql,
                                    statusSolicitacaoHabilitacaoDocente.Status,
                                    string.Empty,
                                    DateTime.Now,
                                    statusSolicitacaoHabilitacaoDocente.IdSolicitacaoHabilitacaoDocente);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null
                            && !retorno.Ok)
                        {
                            return retorno;
                        }
                    }

                    if (statusSolicitacaoHabilitacaoDocente.Status == Reprovacao)
                    {
                        TCommand.ExecuteNonQuery(
                                    connection,
                                    sql,
                                    statusSolicitacaoHabilitacaoDocente.Status,
                                    statusSolicitacaoHabilitacaoDocente.Motivo,
                                    DateTime.Now,
                                    statusSolicitacaoHabilitacaoDocente.IdSolicitacaoHabilitacaoDocente);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null
                            && !retorno.Ok)
                        {
                            connection.Rollback();

                            return retorno;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }

            return retorno;
        }
    }
}