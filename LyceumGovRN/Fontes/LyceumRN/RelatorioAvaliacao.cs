using System;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class RelatorioAvaliacao : RNBase
    {
        public static int VerificaRegistro(string curso, string turno, string curriculo, decimal? serie, string unidade_ens, decimal? ano, decimal? periodo)
        {
            string sql = @"select 1 from LY_RELATORIO_AVALIACAO where CURSO = ? 
                          AND TURNO = ? AND CURRICULO = ? AND SERIE = ? AND UNIDADE_ENS = ? 
                          AND ANO = ? AND PERIODO = ?";
            int num = ExecutarFuncao(sql, curso, turno, curriculo, serie, unidade_ens, ano, periodo);
            return num;
        }

        public static QueryTable Consultar(string curso, string turno, string curriculo, string serie)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select ra.ID_RELATORIO_AVALIACAO as id_relatorio_avaliacao,
                            ra.CURSO as curso, ra.TURNO as turno, ra.CURRICULO as curriculo,
                            ra.SERIE as serie, ra.UNIDADE_ENS as unidade_ens, ra.ANO as ano, ra.PERIODO as periodo,
                            ra.TEM_REPROVACAO_NOTA as tem_reprovacao_nota, ra.TEM_REPROVACAO_FREQ as tem_reprovacao_freq,
                            ra.LABEL_APROVACAO as label_aprovacao, ra.LABEL_REPROVACAO as label_reprovacao, 
                            ra.OBSERVACAO as observacao, null as dt_reclassificacao
                            from LY_RELATORIO_AVALIACAO ra
                            WHERE ra.CURSO = ? and ra.TURNO = ? and ra.CURRICULO = ? and ra.SERIE = ?";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, curso, turno, curriculo, serie);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Remover(decimal id)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_relatorio_avaliacao.Row.Delete(connection, id);

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Inserir(Ly_relatorio_avaliacao.Row row)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"curso, turno, curriculo, serie, unidade_ens, ano, periodo, tem_reprovacao_nota, tem_reprovacao_freq, 
                                   label_aprovacao, label_reprovacao, observacao";
                Ly_relatorio_avaliacao.Row.Insert(connection, colunas, row.Curso, row.Turno, row.Curriculo, row.Serie, row.Unidade_ens, row.Ano, row.Periodo,
                                                  row.Tem_reprovacao_nota, row.Tem_reprovacao_freq, row.Label_aprovacao, row.Label_reprovacao, row.Observacao);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Atualizar(Ly_relatorio_avaliacao.Row row)
        {
            //Atualização do registro
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"curso, turno, curriculo, serie, unidade_ens, ano, periodo, tem_reprovacao_nota, tem_reprovacao_freq, 
                                   label_aprovacao, label_reprovacao, observacao";
                Ly_relatorio_avaliacao.Row.Update(connection, row.Id_relatorio_avaliacao, colunas, row.Curso, row.Turno, row.Curriculo, row.Serie, row.Unidade_ens, row.Ano, row.Periodo,
                                                  row.Tem_reprovacao_nota, row.Tem_reprovacao_freq, row.Label_aprovacao, row.Label_reprovacao, row.Observacao); //conferir colunas

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());

            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal VerificaIdRelatorio(string curso, string turno, string curriculo, decimal serie, decimal ano, decimal periodo)
        {
            String sql_id = @"select ra.ID_RELATORIO_AVALIACAO as id_relatorio_avaliacao
                            from LY_RELATORIO_AVALIACAO ra
                            WHERE ra.CURSO = ? and ra.TURNO = ? and ra.CURRICULO = ? and ra.SERIE = ? and ra.ANO = ? and ra.PERIODO = ?";
            decimal id = ExecutarFuncao(sql_id, curso, turno, curriculo, serie, ano, periodo);
            return id;
        }

        public static QueryTable ConsultarDetalhes(string curso, string turno, string curriculo, string serie, int? ano, int? periodo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql_id = @"select ra.ID_RELATORIO_AVALIACAO as id_relatorio_avaliacao
                            from LY_RELATORIO_AVALIACAO ra
                            WHERE ra.CURSO = ? and ra.TURNO = ? and ra.CURRICULO = ? and ra.SERIE = ? and ra.ANO = ? and ra.PERIODO = ?";
                decimal? id = ExecutarFuncao(sql_id, curso, turno, curriculo, serie, ano, periodo);

                String sql = @"select id_relatorio_avaliacao_detalhe, id_relatorio_avaliacao, ordem, item, grupo_conceito, observacao
                            from LY_RELATORIO_AVALIACAO_DETALHE
                            WHERE ID_RELATORIO_AVALIACAO = ?";

                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, id);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarDetalhes(string id_relatorio_avaliacao)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select id_relatorio_avaliacao_detalhe, id_relatorio_avaliacao, ordem, item, grupo_conceito, observacao
                            from LY_RELATORIO_AVALIACAO_DETALHE
                            WHERE ID_RELATORIO_AVALIACAO = ?";

                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, id_relatorio_avaliacao);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue InserirDetalhes(Ly_relatorio_avaliacao_detalhe.Row row)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"grupo_conceito, observacao";
                Ly_relatorio_avaliacao_detalhe.Row.Insert(connection, row.Id_relatorio_avaliacao, row.Ordem, row.Item, colunas, row.Grupo_conceito, row.Observacao);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue AtualizarDetalhes(Ly_relatorio_avaliacao_detalhe.Row row)
        {
            //Atualização do registro
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"id_relatorio_avaliacao, ordem, item, grupo_conceito, observacao";
                Ly_relatorio_avaliacao_detalhe.Row.Update(connection, row.Id_relatorio_avaliacao_detalhe, colunas, row.Id_relatorio_avaliacao, row.Ordem, row.Item, row.Grupo_conceito, row.Observacao); //conferir colunas

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());

            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue RemoverDetalhes(decimal id)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_relatorio_avaliacao_detalhe.Row.Delete(connection, id);

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable CarregarID(decimal? id_relat, decimal? ordem_old, String item_old)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select id_relatorio_avaliacao_detalhe
                                from LY_RELATORIO_AVALIACAO_DETALHE
                                where ID_RELATORIO_AVALIACAO = ?
                                and ORDEM = ?
                                and ITEM = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, id_relat, ordem_old, item_old);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        public static QueryTable ConsultarItem()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select item, descricao from LY_ITEM_RELATORIO_AVALIACAO";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarGrupoConceito()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select grupo, descricao from LY_GRUPO_CONCEITO";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarMatriculas(string turma, decimal ano, decimal periodo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select distinct
	                        PE.nome_compl as nome_compl, 
	                        m.aluno as aluno, 
	                        m.sit_matricula as sit_matricula, 
	                        m.num_chamada as num_chamada,
	                        m.turma as turma, 
	                        m.ano as ano, 
	                        m.semestre as semestre
                       from ly_matricula m
						INNER JOIN ly_aluno a ON m.aluno = a.aluno
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        where 
                          m.turma = ?
                          and m.ano = ?
                          and m.semestre = ?                           
                        order by m.num_chamada";
                QueryTable qt = new QueryTable(sql);
                qt.CommandTimeout = 60;
                qt.Query(connection, turma, ano, periodo);

                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarRelatorioAvaliacaoAluno(string curso, string turno, string curriculo, string serie, string ano, string periodo, string unidade_ens, string aluno, string subperiodo)
        {
            QueryTable qt = null;
            string _query = @"select rad.ORDEM as ordem, rad.ITEM as item, ira.DESCRICAO as competencia, 
                                rad.GRUPO_CONCEITO as grupo_conceito , ard.CONCEITO as conceito, ar.ID_ALUNO_RELATORIO as id_aluno_relatorio,
                                ard.ID_ALUNO_RELATORIO_DETALHE as id_aluno_relatorio_detalhe, 
                                ard.SUBPERIODO as subperiodo, ard.OBSERVACAO as observacao, ar.SITUACAO as situacao, ar.DATA as data, 
                                ar.USUARIO as usuario, ra.ID_RELATORIO_AVALIACAO as id_relatorio_avaliacao, 
                                rad.ID_RELATORIO_AVALIACAO_DETALHE as id_relatorio_avaliacao_detalhe, ar.ALUNO as aluno
                                from LY_RELATORIO_AVALIACAO ra
                                inner join LY_RELATORIO_AVALIACAO_DETALHE rad on ra.ID_RELATORIO_AVALIACAO = rad.ID_RELATORIO_AVALIACAO
                                inner join LY_ITEM_RELATORIO_AVALIACAO ira on ira.ITEM = rad.ITEM
                                left join LY_ALUNO_RELATORIO ar on ar.ID_RELATORIO_AVALIACAO = ra.ID_RELATORIO_AVALIACAO
                                left join LY_ALUNO_RELATORIO_DETALHE ard on ard.ID_ALUNO_RELATORIO = ar.ID_ALUNO_RELATORIO
                                and ard.ID_RELATORIO_AVALIACAO_DETALHE = rad.ID_RELATORIO_AVALIACAO_DETALHE
                                WHERE ra.CURSO = ? AND ra.CURRICULO = ? and ra.TURNO = ? AND ra.SERIE = ?
                                and ra.ANO = ? and ra.PERIODO = ? and ra.UNIDADE_ENS = ? and ar.ALUNO = ? and ard.SUBPERIODO = ?";
            qt = Consultar(_query, curso, curriculo, turno, serie, ano, periodo, unidade_ens, aluno, subperiodo);
            if (qt.Rows.Count > 0)
            {
                return qt;
            }
            else
            {
                _query = @"select distinct rad.ORDEM as ordem, rad.ITEM as item, ira.DESCRICAO as competencia, 
                        rad.GRUPO_CONCEITO as grupo_conceito, null as conceito, null as id_aluno_relatorio,
                        null as id_aluno_relatorio_detalhe, 
                        null as subperiodo, null as observacao, null as situacao, null as data, 
                        null as usuario, ra.ID_RELATORIO_AVALIACAO as id_relatorio_avaliacao, 
                        rad.ID_RELATORIO_AVALIACAO_DETALHE as id_relatorio_avaliacao_detalhe,
                        null as aluno
                        from LY_RELATORIO_AVALIACAO ra
                        inner join LY_RELATORIO_AVALIACAO_DETALHE rad on ra.ID_RELATORIO_AVALIACAO = rad.ID_RELATORIO_AVALIACAO
                        left join LY_ITEM_RELATORIO_AVALIACAO ira on ira.ITEM = rad.ITEM
                        WHERE ra.CURSO = ? AND ra.CURRICULO = ? and ra.TURNO = ? AND ra.SERIE = ?
                        and ra.ANO = ? and ra.PERIODO = ? and ra.UNIDADE_ENS = ?";
                return Consultar(_query, curso, curriculo, turno, serie, ano, periodo, unidade_ens);
            }
        }

        public static QueryTable ConsultarRelatorioAvaliacaoAlunoImprimir(string curso, string turno, string curriculo, string serie, string ano, string periodo, string unidade_ens, string aluno)
        {
            string _query = @"select rad.ORDEM as ordem, rad.ITEM as item, ira.DESCRICAO as competencia, 
                rad.GRUPO_CONCEITO as grupo_conceito , ard.CONCEITO as conceito, ar.ID_ALUNO_RELATORIO as id_aluno_relatorio,
                ard.ID_ALUNO_RELATORIO_DETALHE as id_aluno_relatorio_detalhe, 
                sl.DESCRICAO as subperiodo, ard.OBSERVACAO as observacao, ar.SITUACAO as situacao, ar.DATA as data, 
                ar.USUARIO as usuario, ra.ID_RELATORIO_AVALIACAO as id_relatorio_avaliacao, 
                rad.ID_RELATORIO_AVALIACAO_DETALHE as id_relatorio_avaliacao_detalhe, ar.ALUNO as aluno
                from LY_RELATORIO_AVALIACAO ra
                inner join LY_RELATORIO_AVALIACAO_DETALHE rad on ra.ID_RELATORIO_AVALIACAO = rad.ID_RELATORIO_AVALIACAO
                inner join LY_ITEM_RELATORIO_AVALIACAO ira on ira.ITEM = rad.ITEM
                left join LY_ALUNO_RELATORIO ar on ar.ID_RELATORIO_AVALIACAO = ra.ID_RELATORIO_AVALIACAO
                left join LY_ALUNO_RELATORIO_DETALHE ard on ard.ID_ALUNO_RELATORIO = ar.ID_ALUNO_RELATORIO
                and ard.ID_RELATORIO_AVALIACAO_DETALHE = rad.ID_RELATORIO_AVALIACAO_DETALHE
                inner join LY_SUBPERIODO_LETIVO sl on sl.SUBPERIODO = ard.SUBPERIODO
                and ra.ANO = sl.ANO
                and ra.PERIODO = sl.PERIODO
                WHERE ra.CURSO = ? AND ra.CURRICULO = ? and ra.TURNO = ? AND ra.SERIE = ?
                and ra.ANO = ? and ra.PERIODO = ? and ra.UNIDADE_ENS = ? and ar.ALUNO = ?";
            return Consultar(_query, curso, curriculo, turno, serie, ano, periodo, unidade_ens, aluno);

        }

        public static string ConsultarGradeIDAluno(string aluno)
        {
            string _query = @"select DISTINCT gs.GRADE_ID as garde_id from LY_GRADE_SERIE gs inner join LY_MATRICULA ma on gs.GRADE = ma.TURMA
                              where ma.ALUNO = ?";

            return ConsultarCampo(_query, aluno);
        }

        public static string ConsultarGradeIDAluno(string aluno, decimal ano, decimal semestre)
        {
            string _query = @"select DISTINCT gs.GRADE_ID as grade_id from LY_MATRICULA ma inner join LY_GRADE_SERIE gs on gs.GRADE = ma.TURMA and ma.ANO = gs.ANO and ma.SEMESTRE = gs.SEMESTRE
                              where ma.ALUNO = ? and ma.ano = ? and ma.semestre = ?";

            return ConsultarCampo(_query, aluno, ano, semestre);
        }

        public static string ConsultarIdRelatAluno(string aluno, string id_relatorio_avaliacao)
        {
            string _query = @"select id_aluno_relatorio from LY_ALUNO_RELATORIO
                              where ALUNO = ? AND ID_RELATORIO_AVALIACAO = ?";

            return ConsultarCampo(_query, aluno, id_relatorio_avaliacao);
        }

        public static bool TemAprovacao(string curso, string turno, string curriculo, string serie, string ano, string periodo, string unidade_ens)
        {
            string sql = @"select TEM_REPROVACAO_NOTA from LY_RELATORIO_AVALIACAO where CURSO = ? and TURNO = ? and 
                           CURRICULO = ? and SERIE = ? and ANO = ? and PERIODO = ? and UNIDADE_ENS = ?";
            string aprovacao_nota = ConsultarCampo(sql, curso, turno, curriculo, serie, ano, periodo, unidade_ens);
            if (!string.IsNullOrEmpty(aprovacao_nota) && aprovacao_nota == "S")
                return true;
            else
                return false;
        }

        public static string ConsultaLabelAprovacao(string curso, string turno, string curriculo, string serie, string ano, string periodo, string unidade_ensino)
        {
            string sql = @"select LABEL_APROVACAO from LY_RELATORIO_AVALIACAO where CURSO = ? and TURNO = ? and 
                           CURRICULO = ? and SERIE = ? and ANO = ? and PERIODO = ? and UNIDADE_ENS = ?";
            return ConsultarCampo(sql, curso, turno, curriculo, serie, ano, periodo, unidade_ensino);
        }

        public static string ConsultaLabelReprovacao(string curso, string turno, string curriculo, string serie, string ano, string periodo, string unidade_ensino)
        {
            string sql = @"select LABEL_REPROVACAO from LY_RELATORIO_AVALIACAO where CURSO = ? and TURNO = ? and 
                           CURRICULO = ? and SERIE = ? and ANO = ? and PERIODO = ? and UNIDADE_ENS = ?";
            return ConsultarCampo(sql, curso, turno, curriculo, serie, ano, periodo, unidade_ensino);
        }

        public static string ConsultarSituacaoAluno(string aluno, string turma, string ano, string periodo)
        {
            String sql = @"select top(1)
	                        m.sit_matricula as sit_matricula
                       from ly_matricula m join ly_aluno a
	                        on m.aluno = a.aluno
                        where 
                          m.turma = ?
                          and m.ano = ?
                          and m.semestre = ?
                          and m.aluno = ? ";
            return ConsultarCampo(sql, turma, ano, periodo, aluno);
        }

        public static string ConsultarNomeAluno(string aluno, string turma, string ano, string periodo)
        {
            String sql = @"select distinct
	                        PE.nome_compl as nome_compl
                       from ly_matricula m join ly_aluno a
	                        on m.aluno = a.aluno
                            INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        where 
                          m.turma = ?
                          and m.ano = ?
                          and m.semestre = ?
                          and m.aluno = ? ";
            return ConsultarCampo(sql, turma, ano, periodo, aluno);
        }

        public static RetValue InserirRelatorioAvaliacaoAluno(Ly_aluno_relatorio.Row row, ref decimal? id_aluno_relatorio_ref)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"id_relatorio_avaliacao, num_func, usuario, situacao, data, observacao";
                row = Ly_aluno_relatorio.Row.Insert(connection, row.Aluno, row.Id_aluno_relatorio, colunas, row.Id_relatorio_avaliacao, row.Num_func, row.Usuario,
                                            row.Situacao, row.Data, row.Observacao);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                else
                    id_aluno_relatorio_ref = row.Id_aluno_relatorio;
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue AtualizarRelatorioAvaliacaoAluno(Ly_aluno_relatorio.Row row)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"aluno, id_relatorio_avaliacao, num_func, usuario, situacao, data, observacao";
                Ly_aluno_relatorio.Row.Update(connection, row.Id_aluno_relatorio, colunas, row.Aluno, row.Id_relatorio_avaliacao, row.Num_func, row.Usuario,
                                            row.Situacao, row.Data, row.Observacao);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue InserirRelatorioAvaliacaoAlunoDetalhe(Ly_aluno_relatorio_detalhe.Row row)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"subperiodo, ordem, id_relatorio_avaliacao_detalhe, conceito, observacao";
                Ly_aluno_relatorio_detalhe.Row.Insert(connection, row.Id_aluno_relatorio, row.Id_aluno_relatorio_detalhe, colunas, row.Subperiodo, row.Ordem,
                                                     row.Id_relatorio_avaliacao_detalhe, row.Conceito, row.Observacao);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue AtualizarRelatorioAvaliacaoAlunoDetalhe(Ly_aluno_relatorio_detalhe.Row row)
        {
            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                string colunas = @"id_aluno_relatorio, subperiodo, ordem, id_relatorio_avaliacao_detalhe, conceito, observacao";
                Ly_aluno_relatorio_detalhe.Row.Update(connection, row.Id_aluno_relatorio_detalhe, colunas, row.Id_aluno_relatorio, row.Subperiodo, row.Ordem,
                                                     row.Id_relatorio_avaliacao_detalhe, row.Conceito, row.Observacao);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        #region Métodos vazios para utilização pelo ObjectDataSource
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void InsertMethodODS(object ano, object periodo, object tem_reprovacao_nota, object tem_reprovacao_freq,
                                   object label_aprovacao, object label_reprovacao, object observacao)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void DeleteMethodODS(object id_relatorio_avaliacao)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateMethodODS(object ano, object periodo, object tem_reprovacao_nota, object tem_reprovacao_freq,
                                   object label_aprovacao, object label_reprovacao, object observacao, object id_relatorio_avaliacao)
        {
            return;
        }

        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void InsertDetalhesODS(object ordem, object item, object grupo_conceito, object observacao, object id_relatorio_avaliacao)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void DeleteDetalhesODS(object id_relatorio_avaliacao_detalhe)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateDetalhesODS(object ordem, object item, object grupo_conceito, object observacao, object id_relatorio_avaliacao, object id_relatorio_avaliacao_detalhe)
        {
            return;
        }
        #endregion


        public static string ConsultarDescrSerie(string curso, string turno, string curriculo, string serie)
        {
            string sql = @"select DESCRICAO from LY_SERIE where CURSO = ? and TURNO = ? and CURRICULO = ? and SERIE = ?";

            return ConsultarCampo(sql, curso, turno, curriculo, serie);
        }

        public static Ly_relatorio_avaliacao.Row ConsultarRelatorio(decimal id_relatorio)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                return Ly_relatorio_avaliacao.QueryFirstRow(connection, "id_relatorio_avaliacao = ?", id_relatorio);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarDadosSerie(decimal idRelat)
        {
            string sql = @"select CURSO, TURNO, CURRICULO, SERIE, UNIDADE_ENS from LY_RELATORIO_AVALIACAO WHERE ID_RELATORIO_AVALIACAO = ?";

            return Consultar(sql, idRelat);
        }

        public static bool VerificaDetalhe(decimal? ordem, string item, decimal? id_relat)
        {
            if (ordem != null && !string.IsNullOrEmpty(item))
            {
                string sql = @"select 1 from LY_RELATORIO_AVALIACAO_DETALHE where ORDEM = ? and ITEM = ? and ID_RELATORIO_AVALIACAO = ?";

                int existe = ExecutarFuncao(sql, ordem, item, id_relat);

                if (existe == 1)
                    return true;
                else
                    return false;
            }
            else if (ordem != null)
            {
                string sql = @"select 1 from LY_RELATORIO_AVALIACAO_DETALHE where ORDEM = ? and ID_RELATORIO_AVALIACAO = ?";

                int existe = ExecutarFuncao(sql, ordem, id_relat);

                if (existe == 1)
                    return true;
                else
                    return false;
            }
            else if (!string.IsNullOrEmpty(item))
            {
                string sql = @"select 1 from LY_RELATORIO_AVALIACAO_DETALHE where ITEM = ? and ID_RELATORIO_AVALIACAO = ?";

                int existe = ExecutarFuncao(sql, item, id_relat);

                if (existe == 1)
                    return true;
                else
                    return false;
            }
            return true;
        }
    }
}
