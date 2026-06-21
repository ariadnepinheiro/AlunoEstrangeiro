using System;
using System.Collections.Generic;
using System.Linq;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class ProvaSerie : RNBase
    {

        public static decimal ConsultarOrdem(string curso, string turno, string curriculo, string serie)
        {
            string sql = @"select MAX(ORDEM) from LY_PROVA_UNIDADE_CURSO where CURSO = ? AND TURNO = ? AND CURRICULO = ? AND SERIE = ?";

            decimal ordem = ExecutarFuncao(sql, curso, turno, curriculo, serie);

            return ordem + 1;
        }

        public static QueryTable CarregarProvasPorSerie()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select *
                                from LY_PROVA_UNIDADE_CURSO
                                where CURSO = '<curso>'
                                  and TURNO = '<turno>'
                                  and CURRICULO = '<currículo>'
                                  and SERIE = '<série>'
                                order by ORDEM";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        public static Ly_prova_unidade_curso.Row ConsultarProva(String curso, String turno, String curriculo, decimal? serie, String prova)
        {
            if (String.IsNullOrEmpty(curso)) return null;
            if (String.IsNullOrEmpty(turno)) return null;
            if (String.IsNullOrEmpty(curriculo)) return null;
            if (String.IsNullOrEmpty(Convert.ToString(serie))) return null;
            if (String.IsNullOrEmpty(prova)) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_prova_unidade_curso dt = Ly_prova_unidade_curso.Query(connection,
                    "curso = ? and turno = ? and curriculo = ? and serie = ? and prova = ?", curso, turno, curriculo, serie, prova);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Ly_prova_unidade_curso.Row ConsultarProva(TConnectionWritable connection, decimal id_prova_unidade_curso)
        {
            Ly_prova_unidade_curso dt = Ly_prova_unidade_curso.Query(connection,
                "id_prova_unidade_curso = ?", id_prova_unidade_curso);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

        public static Ly_prova_unidade_curso.RowCollection ConsultarProvas(TConnectionWritable connection, String curso, String turno, String curriculo, decimal? serie)
        {
            if (String.IsNullOrEmpty(curso)) return null;
            if (String.IsNullOrEmpty(turno)) return null;
            if (String.IsNullOrEmpty(curriculo)) return null;
            if (String.IsNullOrEmpty(Convert.ToString(serie))) return null;

            try
            {
                Ly_prova_unidade_curso dt = Ly_prova_unidade_curso.Query(connection,
                    "curso = ? and turno = ? and curriculo = ? and serie = ?", curso, turno, curriculo, serie);
                return dt.Rows;
            }
            catch
            {
                return null;
            }
        }

        public static Ly_prova_unidade_curso.Row ConsultarOrdem(String curso, String turno, String curriculo, decimal? serie, decimal? ordem)
        {
            if (!serie.HasValue) return null;
            if (!ordem.HasValue) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_prova_unidade_curso dt = Ly_prova_unidade_curso.Query(connection,
                    "curso = ? and turno = ? and curriculo = ? and serie = ? and ordem = ?", curso, turno, curriculo, serie, ordem.Value);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Inserir(Ly_prova_unidade_curso.Row row)
        {
            //Valida o valor do identificador/nome da Prova
            RetValue verificacaoProva = Formula.VerificaIdentificadorProva(row.Prova);
            if (verificacaoProva != null && !verificacaoProva.Ok)
                return verificacaoProva;

            //Verifica se já existe prova cadastrada com a mesma chave para o ano de escolaridade
            Ly_prova_unidade_curso.Row existeProvaSerie = ConsultarProva(row.Curso, row.Turno, row.Curriculo, row.Serie, row.Prova);
            if (existeProvaSerie != null)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento '{0}' já cadastrado para o ano de escolaridade '{1}'.",
                    row.Prova, row.Serie)));

            //Verifica se já existe alguma prova com a mesma ordem para o ano de escolaridade
            Ly_prova_unidade_curso.Row existeSerieOrdem = ConsultarOrdem(row.Curso, row.Turno, row.Curriculo, row.Serie, row.Ordem);
            if (existeSerieOrdem != null && existeSerieOrdem.Prova != row.Prova)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento de ordem '{0}' já cadastrado para o ano de escolaridade '{1}'.",
                    row.Ordem, row.Serie)));

            //Validação da fórmula
            if (!String.IsNullOrEmpty(row.Formula))
            {
                RetValue validacaoFormula = Formula.ValidaFormulaSerie(row.Formula, row.Curso, row.Turno, row.Curriculo, row.Serie, row.Prova, row.Ordem);
                if (validacaoFormula != null && !validacaoFormula.Ok)
                    return validacaoFormula;
            }

            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova_unidade_curso.Row.Insert(connection, row.Prova, row.Descricao, row.On_line, row.E_oficial, row.Ordem, row.Pode_alterar_formula,
                                                   row.E_recuperacao, row.E_prova_base_rec, "curso, turno, curriculo, serie, formula, subperiodo",
                                                   row.Curso, row.Turno, row.Curriculo, row.Serie, row.Formula, row.Subperiodo);

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

        public static RetValue Remover(decimal? id)
        {
            if(!id.HasValue)
                return new RetValue(false, "", new ErrorList("ID não informado."));

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova_unidade_curso.Row provaRemocao = ConsultarProva(connection, id.Value);
                if (provaRemocao == null)
                {
                    connection.Rollback();
                    return new RetValue(false, "", new ErrorList("Instrumento de avaliação não encontrado."));
                }

                Ly_prova_unidade_curso.Row.Delete(connection, id);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                {
                    connection.Rollback();
                    return ret;
                }                

                String curso = provaRemocao.Curso;
                String turno = provaRemocao.Turno;
                String curriculo = provaRemocao.Curriculo;
                decimal? serie = provaRemocao.Serie;

                //Obtém as fórmulas das provas das série e verifica se continuam válidas mesmo após a remoção da prova
                Ly_prova_unidade_curso.RowCollection dtProvas = ConsultarProvas(connection, curso, turno, curriculo, serie);
                foreach (Ly_prova_unidade_curso.Row row in dtProvas)
                {
                    if (String.IsNullOrEmpty(row.Formula))
                        continue;
                    if (row.Prova == provaRemocao.Prova)
                        continue;

                    RetValue retFormula = Formula.ValidaFormulaSerie(connection, row.Formula, curso, turno, curriculo, serie, row.Prova, row.Ordem.Value);
                    if (retFormula != null && !retFormula.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, "", new ErrorList("Não é possível remover o instrumento.\nExiste(m) fórmula(s) de instrumentos(s) referenciando o instrumento \"" + provaRemocao.Prova + "\"."));                        
                    }
                }

                //Obtém as fórmulas de critério da série e verifica se continuam válidas mesmo após a atualização da prova                
                Ly_criterio_aval_unidade_curso dtCriterio = ConsultarDadosCriterios(connection, curso, turno, curriculo, serie);
                if (dtCriterio.Rows.Count > 0)
                {
                    String[] formulas = new String[0];
                    Ly_criterio_aval_unidade_curso.Row row = dtCriterio.Rows[0];
                    formulas = new String[] {
                        row.Formula_ca1, row.Formula_ca2,
                        row.Formula_ca3, row.Formula_mf1,
                        row.Formula_mf2, row.Formula_mf3};

                    foreach (String formula in formulas.Where(f => !String.IsNullOrEmpty(f)))
                    {
                        RetValue retFormula = Formula.ValidaFormulaSerie(connection, formula, curso, turno, curriculo, serie);
                        if (retFormula != null && !retFormula.Ok)
                        {
                            connection.Rollback();
                            if (retFormula.Errors.Count == 0)
                                return retFormula;
                            else
                                return new RetValue(false, "", new ErrorList("Não é possível remover o instrumento.\nExiste(m) critério(s) de avaliação referenciando o instrumento \"" + provaRemocao.Prova + "\"."));
                        }
                    }
                }

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

        public static RetValue Atualizar(String curso, String turno, String curriculo, decimal? serie, String prova, Ly_prova_unidade_curso.Row newValues)
        {
            //Verifica se já existe alguma prova com a mesma ordem para a serie
            Ly_prova_unidade_curso.Row existeProvaOrdem = ConsultarOrdem(curso, turno, curriculo, serie, newValues.Ordem);
            if (existeProvaOrdem != null && prova != existeProvaOrdem.Prova)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento de ordem '{0}' já cadastrado para o ano de escolaridade '{1}'.",
                    newValues.Ordem, newValues.Serie)));

            //Validação da fórmula
            //if (!String.IsNullOrEmpty(newValues.Formula))
            //{
            //    RetValue validacaoFormula = Formula.ValidaFormulaSerie(newValues.Formula, curso, turno, curriculo, serie, prova, newValues.Ordem);
            //    if (validacaoFormula != null && !validacaoFormula.Ok)
            //        return validacaoFormula;
            //}

            //Atualização do registro
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova_unidade_curso.Row.Update(connection, newValues.Id_prova_unidade_curso,
                    "descricao, ordem, on_line, formula, subperiodo",
                    newValues.Descricao, newValues.Ordem, newValues.On_line,
                    newValues.Formula, newValues.Subperiodo);

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();

                //Obtém as fórmulas das provas das série e verifica se continuam válidas mesmo após a remoção da prova
                Ly_prova_unidade_curso.RowCollection dtProvas = ConsultarProvas(connection, curso, turno, curriculo, serie);
                foreach (Ly_prova_unidade_curso.Row row in dtProvas)
                {
                    if (String.IsNullOrEmpty(row.Formula))
                        continue;
                    if (row.Prova == prova)
                        continue;

                    RetValue retFormula = Formula.ValidaFormulaSerie(connection, row.Formula, curso, turno, curriculo, serie, row.Prova, row.Ordem.Value);
                    if (retFormula != null && !retFormula.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, "", new ErrorList("O instrumento de avaliação \"" + prova + "\" é referenciado na fórmula do instrumento de avaliação \"" + row.Prova + "\" e sua ordem é incompatível."));
                    }
                }

                //Obtém as fórmulas de critério da série e verifica se continuam válidas mesmo após a atualização da prova                
                Ly_criterio_aval_unidade_curso dtCriterio = ConsultarDadosCriterios(connection, curso, turno, curriculo, serie);
                if (dtCriterio.Rows.Count > 0)
                {
                    String[] formulas = new String[0];
                    Ly_criterio_aval_unidade_curso.Row row = dtCriterio.Rows[0];
                    formulas = new String[] {
                        row.Formula_ca1, row.Formula_ca2,
                        row.Formula_ca3, row.Formula_mf1,
                        row.Formula_mf2, row.Formula_mf3};

                    foreach (String formula in formulas.Where(f => !String.IsNullOrEmpty(f)))
                    {
                        RetValue retFormula = Formula.ValidaFormulaSerie(connection, formula, curso, turno, curriculo, serie);
                        if (retFormula != null && !retFormula.Ok)
                        {
                            connection.Rollback();
                            if (retFormula.Errors.Count == 0)
                                return retFormula;
                            else
                                return new RetValue(false, "", new ErrorList("Não é possível alterar o instrumento.\nExiste(m) critério(s) de avaliação referenciando o instrumento \"" + prova + "\"."));
                        }
                    }
                }

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

        public static Ly_criterio_aval_unidade_curso ConsultarDadosCriterios(TConnectionWritable connection, String curso, String turno, String curriculo, decimal? serie)
        {
            return Ly_criterio_aval_unidade_curso.Query(connection, "curso = ? AND turno = ? AND curriculo = ? AND serie = ?", curso, turno, curriculo, serie);
        }

        public static QueryTable CarregarID(String curso, String turno, String curriculo, decimal? serie, String prova)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select id_prova_unidade_curso
                                from LY_PROVA_UNIDADE_CURSO
                                where CURSO = ?
                                  and TURNO = ?
                                  and CURRICULO = ?
                                  and SERIE = ?
                                  and PROVA = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, curso, turno, curriculo, serie, prova);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        public static QueryTable CarregarOrdem(String curso, String turno, String curriculo, decimal? serie, String prova)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select ordem
                                from LY_PROVA_UNIDADE_CURSO
                                where CURSO = ?
                                  and TURNO = ?
                                  and CURRICULO = ?
                                  and SERIE = ?
                                  and PROVA = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, curso, turno, curriculo, serie, prova);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        #region Métodos para cópia de Provas/Critérios entre duas Séries

        /// <summary>
        /// Verifica se existem conflitos entre as provas das séries envolvidas.        
        /// </summary>
        /// <returns>RetValue == null: nenhum conflito
        ///          RetValue != null && RetValue.Ok: não há provas cadastradas na origem
        ///          RetValue != null && !RetValue.Ok: conflitos identificados.</returns>
        private static RetValue VerificaCopiaDeProvasECriterios(String cursoOrigem, String turnoOrigem, String curriculoOrigem, String serieOrigem,
            String cursoDestino, String turnoDestino, String curriculoDestino, String serieDestino)
        {
            //PRÉ-VERIFICAÇÃO: Validação do conteúdo e formato dos dados
            decimal d_serieOrigem, d_serieDestino;
            if (!Decimal.TryParse(serieOrigem, out d_serieOrigem))
                return new RetValue(false, "", new ErrorList("Série (origem cópia) inválida: \"" + serieOrigem + "\"."));
            if (!Decimal.TryParse(serieDestino, out d_serieDestino))
                return new RetValue(false, "", new ErrorList("Série (destino cópia) inválida: \"" + serieDestino + "\"."));
            Dictionary<String, String> chaves = new Dictionary<String, String>();
            chaves.Add("Curso (origem cópia)", cursoOrigem);
            chaves.Add("Curso (destino cópia)", cursoDestino);
            chaves.Add("Turno (origem cópia)", turnoOrigem);
            chaves.Add("Turno (destino cópia)", turnoDestino);
            chaves.Add("Currículo (origem cópia)", curriculoOrigem);
            chaves.Add("Currículo (destino cópia)", curriculoDestino);
            ErrorList errors = new ErrorList();
            foreach (var chave in chaves.Where(k => String.IsNullOrEmpty(k.Value)))
                errors.Add(chave.Key + " inválido: \"" + chave.Value + "\".");
            if (errors.Count > 0)
                return new RetValue(false, "", errors);

            //BUSCA DE DADOS - Busca as provas da origem e destino da cópia
            QueryTable qtProvasOrigem = CarregarProvasPorSerie(cursoOrigem, turnoOrigem, curriculoOrigem, d_serieOrigem);
            QueryTable qtProvasDestino = CarregarProvasPorSerie(cursoDestino, turnoDestino, curriculoDestino, d_serieDestino);

            if (qtProvasOrigem.Rows.Count == 0)
                return new RetValue(true, "Não existem instrumentos cadastrados para este ano de escolaridade.", null);

            //VERIFICAÇÃO DE CONFLITOS - Verifica conflitos de identificadores e ordem
            //                           entre provas da origem e destino da cópia
            Dictionary<decimal, String> provasOrigem = new Dictionary<decimal, String>();
            Dictionary<decimal, String> provasDestino = new Dictionary<decimal, String>();
            foreach (SimpleRow row in qtProvasOrigem.Rows)
                provasOrigem.Add((decimal)row["ordem"], (String)row["prova"]);
            foreach (SimpleRow row in qtProvasDestino.Rows)
                provasDestino.Add((decimal)row["ordem"], (String)row["prova"]);

            if (provasOrigem.Values.Where(v => provasDestino.Values.Contains(v)).Count() > 0)
                return new RetValue(false, "", new ErrorList("Foram encontrados conflitos de identificador entre os instrumentos de avaliação dos anos de escolaridade envolvidos. "));
            if (provasOrigem.Keys.Where(k => provasDestino.Keys.Contains(k)).Count() > 0)
                return new RetValue(false, "", new ErrorList("Foram encontrados conflitos de ordem entre os instrumentos de avaliação dos anos de escolaridade envolvidos. "));

            return null;
        }

        private static RetValue RemoverProvas(TConnectionWritable connection, String curso, String turno, String curriculo, String serie)
        {
            //Remove as provas associadas
            QueryTable qtDeleteProvas = new QueryTable(
                @"DELETE FROM ly_prova_unidade_curso WHERE
                    curso = ? AND turno = ? AND curriculo = ? AND serie = ?");
            qtDeleteProvas.Query(connection, curso, turno, curriculo, serie);
            RetValue retProvas = VerificarErro(connection.GetErrors());
            if (retProvas != null && !retProvas.Ok)
                return retProvas;       

            return null;
        }

        private static RetValue RemoverCriterios(TConnectionWritable connection, String curso, String turno, String curriculo, String serie)
        {
            //Remove os critérios de avaliação
            QueryTable qtDeleteCriterios = new QueryTable(
                @"DELETE FROM ly_criterio_aval_unidade_curso WHERE
                    curso = ? AND turno = ? AND curriculo = ? AND serie = ?");
            qtDeleteCriterios.Query(connection, curso, turno, curriculo, serie);
            RetValue retCriterios = VerificarErro(connection.GetErrors());
            if (retCriterios != null && !retCriterios.Ok)
                return retCriterios;
            return null;
        }

        private static RetValue CopiarProvas(TConnectionWritable connection, String cursoOrigem, String turnoOrigem, String curriculoOrigem, String serieOrigem,
            String cursoDestino, String turnoDestino, String curriculoDestino, String serieDestino)
        {
            QueryTable provasOrigem = new QueryTable(
                @"select * from LY_PROVA_UNIDADE_CURSO
                  where CURSO = ? and TURNO = ?
                  and CURRICULO = ? and SERIE = ?");
            provasOrigem.Query(connection, cursoOrigem, turnoOrigem, curriculoOrigem, serieOrigem);
            RetValue consultaOrigem = VerificarErro(connection.GetErrors());
            if (consultaOrigem != null && !consultaOrigem.Ok)
                return consultaOrigem;

            String e_oficial = "S";
            String pode_alterar_formula = "S";
            String e_recuperacao = "N";
            String e_prova_base_rec = "N";

            foreach (SimpleRow row in provasOrigem.Rows)
            {
                String prova = (String)row["prova"];
                String descricao = (String)row["descricao"];
                decimal ordem = (decimal)row["ordem"];
                decimal? subperiodo = (decimal?)row["subperiodo"];
                String on_line = (String)row["on_line"];
                String formula = (String)row["formula"];

                Ly_prova_unidade_curso.Row.Insert(connection, prova, descricao, on_line,
                    e_oficial, ordem, pode_alterar_formula, e_recuperacao, e_prova_base_rec,
                    "curso, turno, curriculo, serie, formula, subperiodo",
                    cursoDestino, turnoDestino, curriculoDestino, serieDestino, formula, subperiodo);

                RetValue retInsercao = VerificarErro(connection.GetErrors());
                if (retInsercao != null && !retInsercao.Ok)
                    return retInsercao;
            }
            return null;
        }

        private static RetValue CopiarCriterios(TConnectionWritable connection, String cursoOrigem, String turnoOrigem, String curriculoOrigem, String serieOrigem,
            String cursoDestino, String turnoDestino, String curriculoDestino, String serieDestino)
        {
            QueryTable qtCriteriosOrigem = new QueryTable(
                @"SELECT TOP 1 
                      curso, turno, curriculo, serie, 
                      formula_ca1, formula_mf1, conceito_min_1,
                      formula_ca2, formula_mf2, conceito_min_2,
                      formula_ca3, formula_mf3, conceito_min_3,
                      conceito_min_ex, conceito_min_ex_2
                  FROM ly_criterio_aval_unidade_curso 
                  WHERE curso = ? AND turno = ? AND curriculo = ? AND serie = ?");
            qtCriteriosOrigem.Query(connection, cursoOrigem, turnoOrigem, curriculoOrigem, serieOrigem);

            if (qtCriteriosOrigem.Rows.Count > 0)
            {
                SimpleRow rowOrigem = qtCriteriosOrigem.Rows[0];

                object curso = cursoDestino;
                object turno = turnoDestino;
                object curriculo = curriculoDestino;
                object serie = serieDestino;

                object formula_ca1 = rowOrigem["formula_ca1"];
                object formula_mf1 = rowOrigem["formula_mf1"];
                object conceito_min_1 = rowOrigem["conceito_min_1"];
                object formula_ca2 = rowOrigem["formula_ca2"];
                object formula_mf2 = rowOrigem["formula_mf2"];
                object conceito_min_2 = rowOrigem["conceito_min_2"];
                object formula_ca3 = rowOrigem["formula_ca3"];
                object formula_mf3 = rowOrigem["formula_mf3"];
                object conceito_min_3 = rowOrigem["conceito_min_3"];
                object conceito_min_ex = rowOrigem["conceito_min_ex"];
                object conceito_min_ex_2 = rowOrigem["conceito_min_ex_2"];

                Ly_criterio_aval_unidade_curso.Row.Insert(connection,
                    "curso, turno, curriculo, serie, " +
                    "formula_ca1, formula_mf1, conceito_min_1, " +
                    "formula_ca2, formula_mf2, conceito_min_2, " +
                    "formula_ca3, formula_mf3, conceito_min_3, " +
                    "conceito_min_ex, conceito_min_ex_2",
                    curso, turno, curriculo, serie,
                    formula_ca1, formula_mf1, conceito_min_1,
                    formula_ca2, formula_mf2, conceito_min_2,
                    formula_ca3, formula_mf3, conceito_min_3,
                    conceito_min_ex, conceito_min_ex_2);

                RetValue retInsercao = VerificarErro(connection.GetErrors());
                if (retInsercao != null && !retInsercao.Ok)
                    return retInsercao;

            }
            return null;
        }


        public static RetValue CopiarProvasECriterios(String cursoOrigem, String turnoOrigem, String curriculoOrigem, String serieOrigem,
            String cursoDestino, String turnoDestino, String curriculoDestino, String serieDestino, Boolean copiaForcada)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                //Se não for cópia forçada, verifica se há conflitos. Se há, não altera dados.
                //Se não há conflitos, realiza cópia.
                if (!copiaForcada)
                {
                    RetValue verificacao = VerificaCopiaDeProvasECriterios(cursoOrigem, turnoOrigem,
                        curriculoOrigem, serieOrigem, cursoDestino, turnoDestino, curriculoDestino,
                        serieDestino);
                    if (verificacao != null) return verificacao;
                }
                //Se for cópia forçada, não verifica e já remove os critérios e as provas
                else
                {
                    RetValue remocao = RemoverProvas(connection, cursoDestino, turnoDestino, curriculoDestino, serieDestino);
                    if (remocao != null && !remocao.Ok)
                    {
                        connection.Rollback();
                        return remocao;
                    }
                }

                RetValue remCriterios = RemoverCriterios(connection, cursoDestino, turnoDestino, curriculoDestino, serieDestino);
                if (remCriterios != null && !remCriterios.Ok)
                {
                    connection.Rollback();
                    return remCriterios;
                }

                //Cópia das provas e dos critérios. Ponto de execução atingido somente quando: 
                //(copiaForcada == false e não há conflitos) || (copiaForcada == true)                            
                RetValue copiaProvas = CopiarProvas(connection, cursoOrigem, turnoOrigem,
                        curriculoOrigem, serieOrigem, cursoDestino, turnoDestino, curriculoDestino,
                        serieDestino);
                if (copiaProvas != null && !copiaProvas.Ok)
                {
                    connection.Rollback();
                    return copiaProvas;
                }

                RetValue copiaCriterios = CopiarCriterios(connection, cursoOrigem, turnoOrigem, curriculoOrigem, serieOrigem, cursoDestino, turnoDestino, curriculoDestino, serieDestino);
                if (copiaCriterios != null && !copiaCriterios.Ok)
                {
                    connection.Rollback();
                    return copiaCriterios;
                }
                
                return null;
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

        #endregion

        #region ODS
        public static QueryTable CarregarProvasPorSerie(String curso, String turno, String curriculo, decimal? serie)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select id_prova_unidade_curso, prova, 
                               descricao, ordem, on_line, subperiodo,
                               formula, curso, turno, curriculo, serie
                                from LY_PROVA_UNIDADE_CURSO
                                where CURSO = ?
                                  and TURNO = ?
                                  and CURRICULO = ?
                                  and SERIE = ?
                                order by ORDEM";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, curso, turno, curriculo, serie);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        #region Métodos vazios para utilização pelo ObjectDataSource
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void InsertProvasPorSerie(object prova, object descricao, object subperiodo)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void DeleteProvasPorSerie(object id_prova_unidade_curso)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateProvasPorSerie(object prova, object descricao, object subperiodo, object id_prova_unidade_curso)
        {
            return;
        }
        #endregion

        #endregion
    }
}
