using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Library;

namespace Techne.Lyceum.RN
{
    public class ProvaDisciplina : RNBase
    {

        public static decimal ConsultarOrdem(string disciplina)
        {
            string sql = @"select MAX(ORDEM) from LY_PROVA_DISCIP WHERE DISCIPLINA = ?";

            decimal ordem = ExecutarFuncaoDec(sql, disciplina);

            return ordem + 1;
        }

        public static QueryTable CarregarOrdem(String prova, String disciplina)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select ordem
                                from LY_PROVA_DISCIP
                                where PROVA = ? AND DISCIPLINA = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, prova, disciplina);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        public static QueryTable Consultar(String disciplina)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT disciplina,
                                      prova,
                                      descricao,
                                      ordem,
                                      subperiodo,
                                      on_line,
                                      formula
                               FROM ly_prova_discip";

                QueryTable qtProvaDisciplina;
                if (!String.IsNullOrEmpty(disciplina))
                {
                    sql += " WHERE disciplina = ?";
                    qtProvaDisciplina = new QueryTable(sql);
                    qtProvaDisciplina.Query(connection, disciplina);
                }
                else
                {
                    qtProvaDisciplina = new QueryTable(sql);
                    qtProvaDisciplina.Query(connection);
                }
                return qtProvaDisciplina;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Ly_prova_discip Consultar(TConnectionWritable connection, String disciplina)
        {
            if (String.IsNullOrEmpty(disciplina)) return null;
            Ly_prova_discip dt = Ly_prova_discip.Query(connection,
                "disciplina = ?", disciplina);
            return dt;
        }

        public static Ly_prova_discip.Row Consultar(String disciplina, String prova)
        {
            if (String.IsNullOrEmpty(disciplina)) return null;
            if (String.IsNullOrEmpty(prova)) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_prova_discip dt = Ly_prova_discip.Query(connection,
                    "disciplina = ? and prova = ?", disciplina, prova);

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

        public static Ly_prova_discip.Row Consultar(String disciplina, decimal? ordem)
        {
            if (String.IsNullOrEmpty(disciplina)) return null;
            if (!ordem.HasValue) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_prova_discip dt = Ly_prova_discip.Query(connection,
                    "disciplina = ? and ordem = ?", disciplina, ordem.Value);

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

        public static RetValue Inserir(Ly_prova_discip.Row row)
        {
            //Verifica se o nome da prova é igual a alguma palavra reservada
            String[] funcoes = new String[] { "MAX", "MIN", "TRUNCATE", "ROUND", "ISNULL" };
            for (int i = 0; i < funcoes.Count(); i++)
                if (row.Prova.Trim().ToUpper().Equals(funcoes[i]))
                    return new RetValue(false, "", new ErrorList("O Instrumento não pode ser igual à palavra \"" + funcoes[i] + "\""));

            //Verifica se o nome da prova contém algum caractere não permitido
            if (row.Prova.ToCharArray()
                .Where(c => c == ' ' || c == '.' || c == ',' || c == '-' ||
                           c == '+' || c == '*' || c == '/' || c == '(' || c == ')')
                .Count() > 0)
                return new RetValue(false, "", new ErrorList("O Instrumento contém caracteres não permitidos."));

            //Verifica se já existe prova cadastrada com a mesma chave para a disciplina
            Ly_prova_discip.Row existeProvaDisciplina = Consultar(row.Disciplina, row.Prova);
            if (existeProvaDisciplina != null)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento '{0}' já cadastrado para a disciplina '{1}'.",
                    row.Prova, row.Disciplina)));

            //Verifica se já existe alguma prova com a mesma ordem para a disciplina
            Ly_prova_discip.Row existeDisciplinaOrdem = Consultar(row.Disciplina, row.Ordem);
            if (existeDisciplinaOrdem != null && existeDisciplinaOrdem.Prova != row.Prova)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento de ordem '{0}' já cadastrado para a disciplina '{1}'.",
                    row.Ordem, row.Disciplina)));

            //Validação da fórmula
            if (!String.IsNullOrEmpty(row.Formula))
            {                
                RetValue validacaoFormula = Formula.ValidaFormulaProvaDisciplina(row.Formula, row.Disciplina, row.Prova, row.Ordem.Value);
                if (validacaoFormula != null && !validacaoFormula.Ok)
                    return validacaoFormula;
            }

            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova_discip.Row.Insert(connection, row.Disciplina, row.Prova,
                    row.Descricao, row.Ordem, row.On_line, row.Pode_alterar_formula, row.E_recuperacao,
                    row.E_prova_base_rec, "e_oficial, formula, subperiodo", row.E_oficial, row.Formula,
                    row.Subperiodo);

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

        public static RetValue Remover(String disciplina, String prova)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                //Remove a prova da tabela
                Ly_prova_discip.Row.Delete(connection, disciplina, prova);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                {
                    connection.Rollback();
                    return ret;
                }


                //Obtém as fórmulas das provas das disciplinas e verifica se continuam válidas mesmo após a remoção da prova                
                Ly_prova_discip dtProvas = RN.ProvaDisciplina.Consultar(connection, disciplina);
                foreach (Ly_prova_discip.Row row in dtProvas.Rows)
                {
                    if (String.IsNullOrEmpty(row.Formula))
                        continue;

                    RetValue retFormula = Formula.ValidaFormulaProvaDisciplina(connection, row.Formula, disciplina, row.Prova, row.Ordem.Value);
                    if (retFormula != null && !retFormula.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, "", new ErrorList("Não é possível remover o instrumento.\nExiste(m) fórmula(s) de instrumento(s) referenciando o instrumento \"" + prova + "\"."));
                    }
                }


                //Obtém as fórmulas da disciplina e verifica se continuam válidas mesmo após a remoção da prova
                Ly_disciplina dtDisciplina = RN.Disciplina.ConsultarDadosDisciplina(disciplina);
                if (dtDisciplina.Rows.Count > 0)
                {
                    String[] formulasDisciplina = new String[0];
                    Ly_disciplina.Row row = dtDisciplina.Rows[0];
                    formulasDisciplina = new String[] {
                        row.Formula_ca1, row.Formula_ca2,
                        row.Formula_ca3, row.Formula_mf1,
                        row.Formula_mf2, row.Formula_mf3};

                    foreach (String formula in formulasDisciplina.Where(f => !String.IsNullOrEmpty(f)))
                    {
                        RetValue retFormula = Formula.ValidaFormulaDisciplina(formula, disciplina, connection);
                        if (retFormula != null && !retFormula.Ok)
                        {
                            connection.Rollback();
                            return new RetValue(false, "", new ErrorList("Não é possível remover o instrumento.\nExiste(m) critério(s) de avaliação referenciando o instrumento \"" + prova + "\"."));
                        }
                    }
                }

                return new RetValue(true, "Instrumento removido com sucesso.", null);
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

        public static RetValue Atualizar(String pk_disciplina, String pk_prova, Ly_prova_discip.Row newValues)
        {
            //Verifica se já existe alguma prova com a mesma ordem para a disciplina
            Ly_prova_discip.Row existeDisciplinaOrdem = Consultar(pk_disciplina, newValues.Ordem);
            if (existeDisciplinaOrdem != null && pk_prova != existeDisciplinaOrdem.Prova)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento de ordem '{0}' já cadastrado para a disciplina '{1}'.",
                    newValues.Ordem, newValues.Disciplina)));

            //Validação da fórmula
            if (!String.IsNullOrEmpty(newValues.Formula))
            {
                RetValue validacaoFormula = Formula.ValidaFormulaProvaDisciplina(newValues.Formula, pk_disciplina, pk_prova, newValues.Ordem.Value);
                if (validacaoFormula != null && !validacaoFormula.Ok)
                    return validacaoFormula;
            }

            //Atualização do registro
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova_discip.Row.Update(connection, pk_disciplina, pk_prova,
                    "descricao, ordem, on_line, formula, subperiodo",
                    newValues.Descricao, newValues.Ordem, newValues.On_line,
                    newValues.Formula, newValues.Subperiodo);

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();

                //Obtém as fórmulas das provas das disciplinas e verifica se continuam válidas mesmo após a atualização da prova                
                Ly_prova_discip dtProvas = RN.ProvaDisciplina.Consultar(connection, pk_disciplina);
                foreach (Ly_prova_discip.Row row in dtProvas.Rows)
                {
                    if (String.IsNullOrEmpty(row.Formula))
                        continue;
                    if (row.Prova == pk_prova)
                        continue;

                    RetValue retFormula = Formula.ValidaFormulaProvaDisciplina(connection, row.Formula, pk_disciplina, row.Prova, row.Ordem.Value);
                    if (retFormula != null && !retFormula.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, "", new ErrorList("O instrumento \"" + pk_prova + "\" é referenciada na fórmula do instrumento \"" + row.Prova + "\" e sua ordem é incompatível."));
                    }              
                }

                //Obtém as fórmulas da disciplina e verifica se continuam válidas mesmo após a remoção da prova
                Ly_disciplina dtDisciplina = RN.Disciplina.ConsultarDadosDisciplina(pk_disciplina);
                if (dtDisciplina.Rows.Count > 0)
                {
                    String[] formulasDisciplina = new String[0];
                    Ly_disciplina.Row row = dtDisciplina.Rows[0];
                    formulasDisciplina = new String[] {
                        row.Formula_ca1, row.Formula_ca2,
                        row.Formula_ca3, row.Formula_mf1,
                        row.Formula_mf2, row.Formula_mf3};

                    foreach (String formula in formulasDisciplina.Where(f => !String.IsNullOrEmpty(f)))
                    {
                        RetValue retFormula = Formula.ValidaFormulaDisciplina(formula, pk_disciplina, connection);
                        if (retFormula != null && !retFormula.Ok)
                        {
                            connection.Rollback();
                            return new RetValue(false, "", new ErrorList("Não é possível alterar o instrumento.\nExiste(m) critério(s) de avaliação referenciando o instrumento \"" + pk_prova + "\"."));
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

        public static QueryTable ConsultarSubPeriodosLetivos()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT 
                                   null AS subperiodo, 
                                   '<nenhum>' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarSubPeriodosLetivos(decimal? ano, decimal? semestre)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT 
                                   null AS subperiodo, 
                                   '<nenhum>' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo
                               WHERE ano = ? AND periodo = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, ano.Value, semestre.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        #region Métodos vazios para utilização pelo ObjectDataSource
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void InsertMethodODS(object prova, object descricao, object subperiodo)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void DeleteMethodODS(object disciplina, object prova)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateMethodODS(object prova, object descricao, object subperiodo, object disciplina)
        {
            return;
        }
        #endregion

        public static QueryTable ConsultarProvasDisciplina(object turma, string ano, string periodo, string disciplina, string aluno)
        {
            string _query = @"
                declare @disciplina T_CODIGO = ?
                declare @turma VARCHAR(50) = ?
                declare @ano T_ANO = ?
                declare @semestre T_SEMESTRE2 = ?
                declare @aluno T_CODIGO = ?

                select p.prova, p.ordem, p.subperiodo, pl.DESCRICAO as descr_subperiodo, n.conceito, isnull(p.formula,'') formula, p.nota_max as nota_max
                from LY_PROVA p 
                left join LY_NOTA n on  
	                p.DISCIPLINA = n.DISCIPLINA and p.TURMA = p.TURMA and p.ANO = n.ANO and p.SEMESTRE = n.SEMESTRE and p.PROVA = n.PROVA 
                left join LY_SUBPERIODO_LETIVO pl on 
	                pl.ANO = p.ANO and pl.PERIODO = p.SEMESTRE and pl.SUBPERIODO = p.SUBPERIODO
                where p.DISCIPLINA = @disciplina and p.TURMA = @turma and p.ANO = @ano and p.SEMESTRE = @semestre and aluno = @aluno

                UNION

                select distinct p.prova, p.ordem, p.subperiodo, pl.DESCRICAO as descr_subperiodo, '', isnull(p.formula,'') formula, p.nota_max as nota_max
                from LY_PROVA p 
                left join LY_SUBPERIODO_LETIVO pl on 
	                pl.ANO = p.ANO and pl.PERIODO = p.SEMESTRE and pl.SUBPERIODO = p.SUBPERIODO
                where p.DISCIPLINA = @disciplina and p.TURMA = @turma and p.ANO = @ano and p.SEMESTRE = @semestre and not exists
	                (select 1
		                from LY_PROVA p2 left join LY_NOTA n on  p2.DISCIPLINA = n.DISCIPLINA and p2.TURMA = p2.TURMA and p2.ANO = n.ANO  and p2.SEMESTRE = n.SEMESTRE and p2.PROVA = n.PROVA 
		                where p2.DISCIPLINA = @disciplina and p2.TURMA = @turma and p2.ANO = @ano and p2.SEMESTRE = @semestre and aluno = @aluno and p2.PROVA = p.PROVA)
                order by p.ORDEM";

            if (turma == null || ano == null || periodo == null || disciplina == null)
            {
                _query = _query.Replace("?", "null");
                return Consultar(_query);
            }
            else
            {
                return Consultar(_query, disciplina, turma.ToString(), ano, periodo, aluno);
            }
        }
    }
}