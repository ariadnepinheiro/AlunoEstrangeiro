using System;
using Techne.Data;
using Techne.Library;

namespace Techne.Lyceum.RN
{
    public class AssistenteEnsino : RNBase
    {
        #region LY_AUXILIAR

        /// <summary>
        /// Consulta auxiliares e seus dados.
        /// </summary>
        /// <returns></returns>
        public static QueryTable Consultar()
        {
            return Consultar(@"select a.auxiliar, a.pessoa, p.NOME_COMPL as nome from ly_auxiliar a
                    inner join LY_PESSOA p on p.PESSOA = a.PESSOA
                    order by a.auxiliar");
        }

        /// <summary>
        /// Consulta primeira linha para um auxiliar.
        /// </summary>
        /// <param name="auxiliar">auxiliar</param>
        /// <returns>linha do auxiliar</returns>
        public static CR.Ly_auxiliar.Row ConsultarAuxiliar(string auxiliar)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return CR.Ly_auxiliar.QueryFirstRow(connection, "auxiliar = ?", auxiliar);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Consulta auxiliar por pessoa.
        /// </summary>
        /// <param name="pessoa">código da pessoa</param>
        /// <returns>primeira linha do auxiliar para a pessoa</returns>
        public static CR.Ly_auxiliar.Row ConsultarAuxiliarPorPessoa(decimal? pessoa)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return CR.Ly_auxiliar.QueryFirstRow(connection, "pessoa = ?", pessoa);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Inlcui dados do auxiliar.
        /// </summary>
        /// <param name="auxiliar">auxiliar</param>
        /// <param name="pessoa">pessoa</param>
        /// <returns>sucesso ou mensagem de erro</returns>
        public static RetValue Inserir(String auxiliar, String pessoa)
        {
            if (String.IsNullOrEmpty(auxiliar))
                return new RetValue(false, "", new ErrorList("Favor informar o assistente."));
            if (String.IsNullOrEmpty(pessoa))
                return new RetValue(false, "", new ErrorList("Favor informar o aluno."));

            decimal d_pessoa;
            decimal? d2_pessoa = decimal.TryParse(pessoa, out d_pessoa) ? d_pessoa : (decimal?)null;
            if (!d2_pessoa.HasValue)
                return new RetValue(false, "", new ErrorList("Aluno inválido."));

            CR.Ly_auxiliar.Row row = ConsultarAuxiliar(auxiliar);
            if (row != null && row["auxiliar"].ToString().Equals(auxiliar.ToString()))
                return new RetValue(false, "", new ErrorList("Código de assistente de ensino já cadastrado."));

            CR.Ly_auxiliar.Row rowPessoa = ConsultarAuxiliarPorPessoa(d2_pessoa);
            if (rowPessoa != null && rowPessoa["pessoa"].ToString().Equals(pessoa.ToString()))
                return new RetValue(false, "", new ErrorList("Aluno já cadastrado como assistente de ensino."));

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                CR.Ly_auxiliar.Row.Insert(connection, auxiliar, d2_pessoa);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Atualiza auxiliar
        /// </summary>
        /// <param name="old_auxiliar">auxiliar</param>
        /// <param name="pessoa">pessoa</param>
        /// <returns>sucesso ou mensagem de erro</returns>
        public static RetValue Atualizar(String old_auxiliar, String pessoa)
        {
            if (String.IsNullOrEmpty(old_auxiliar))
                return new RetValue(false, "", new ErrorList("Favor informar o assistente."));
            if (String.IsNullOrEmpty(pessoa))
                return new RetValue(false, "", new ErrorList("Favor informar o aluno."));

            decimal d_pessoa;
            decimal? d2_pessoa = decimal.TryParse(pessoa, out d_pessoa) ? d_pessoa : (decimal?)null;
            if (!d2_pessoa.HasValue)
                return new RetValue(false, "", new ErrorList("Aluno inválido."));

            CR.Ly_auxiliar.Row rowPessoa = ConsultarAuxiliarPorPessoa(d2_pessoa);
            if (rowPessoa != null && !rowPessoa["auxiliar"].ToString().Equals(old_auxiliar.ToString()))
                return new RetValue(false, "", new ErrorList("Aluno já cadastrado como assistente de ensino."));

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                CR.Ly_auxiliar.Row.Update(connection, old_auxiliar, "auxiliar, pessoa", old_auxiliar, pessoa);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Deletar auxiliar
        /// </summary>
        /// <param name="auxiliar">auxiliar</param>
        public static void DeleteMethod(String auxiliar)
        {
            if (String.IsNullOrEmpty(auxiliar))
                return;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                CR.Ly_auxiliar.Row.Delete(connection, auxiliar);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                {
                    connection.Rollback();
                    throw new Exception(ret.Errors.ToString());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Obsolete("Não apagar: utilizado pelo ODS")]
        public static void InsertMethod(String auxiliar, decimal? pessoa) { }
        [Obsolete("Não apagar: utilizado pelo ODS")]
        public static void UpdateMethod(String auxiliar, decimal? pessoa) { }
        
        #endregion

        #region LY_AUXTURMA
        /// <summary>
        /// Consulta auxiliares da disciplina/turma/ano/semestre.
        /// </summary>
        /// <param name="disciplina">disciplina</param>
        /// <param name="turma">turma</param>
        /// <param name="ano">ano</param>
        /// <param name="semestre">semestre</param>
        /// <returns>querytable com auxiliares</returns>
        public static QueryTable ConsultarAuxsTurma(String disciplina, String turma, decimal ano, decimal semestre)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return Consultar(@"select aux.auxiliar, p.nome_compl as nome, at.disciplina, at.turma, at.ano, 
                        at.semestre, at.funcao, at.aluno, at.monitoria_paga from ly_auxturma at
                        inner join ly_auxiliar aux on at.auxiliar = aux.auxiliar
                        inner join ly_pessoa p on aux.pessoa = p.pessoa where
                        disciplina = ? and turma = ? and ano = ? and semestre = ?", disciplina, turma, ano, semestre);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Consulta auxiliar de disciplina/turma/ano/semestre.
        /// </summary>
        /// <param name="disciplina">disciplina</param>
        /// <param name="turma">turma</param>
        /// <param name="ano">ano</param>
        /// <param name="semestre">semestre</param>
        /// <param name="auxiliar">auxiliar</param>
        /// <returns>primeira linha do auxiliar</returns>
        public static CR.Ly_auxturma.Row ConsultarAuxTurma(String disciplina, String turma, decimal? ano, decimal? semestre, String auxiliar)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return CR.Ly_auxturma.QueryFirstRow(connection, "disciplina = ? AND turma = ? AND ano = ? AND semestre = ? AND auxiliar = ?",
                    disciplina, turma, ano, semestre, auxiliar);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Insere auxiliar na disciplina/turma/ano/semestre.
        /// </summary>
        /// <param name="row">linha de auxiliar ser inserida</param>
        /// <returns>sucesso ou mensagem de erro</returns>
        public static RetValue InserirAuxTurma(CR.Ly_auxturma.Row row)
        {
            if (String.IsNullOrEmpty(row.Disciplina)) return new RetValue(false, "", new ErrorList("Disciplina não informada."));
            if (String.IsNullOrEmpty(row.Turma)) return new RetValue(false, "", new ErrorList("Turma não informada."));
            if (!row.Ano.HasValue) return new RetValue(false, "", new ErrorList("Ano não informado."));
            if (!row.Semestre.HasValue) return new RetValue(false, "", new ErrorList("Semestre não informado."));
            if (String.IsNullOrEmpty(row.Auxiliar)) return new RetValue(false, "", new ErrorList("Auxiliar não informado."));
            if (String.IsNullOrEmpty(row.Funcao)) return new RetValue(false, "", new ErrorList("Função não informada."));
            if (String.IsNullOrEmpty(row.Monitoria_paga)) return new RetValue(false, "", new ErrorList("Monitoria paga não informada."));

            CR.Ly_auxturma.Row aux = ConsultarAuxTurma(row.Disciplina, row.Turma, row.Ano, row.Semestre, row.Auxiliar);
            QueryTable qtAux = Consultar(@"select aux.auxiliar, aux.pessoa, alu.aluno, alu.sit_aluno from ly_auxiliar aux
                                inner join LY_ALUNO alu on aux.PESSOA = alu.PESSOA where auxiliar = ?", row.Auxiliar);
           
            if (qtAux != null && qtAux.Rows.Count > 0 && !qtAux.Rows[0]["sit_aluno"].Equals("Ativo"))
                return new RetValue(false, "", new ErrorList("O assistente selecionado não está mais ativo."));
     
            if (aux == null)
            {
                TConnectionWritable connection = Config.CreateWritableConnection();
                connection.Open(true);
                try
                {
                    CR.Ly_auxturma.Row.Insert(connection, row.Disciplina, row.Turma, row.Ano, row.Semestre, row.Auxiliar, row.Funcao, row.Monitoria_paga);
                    RetValue ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, "", new ErrorList("Não foi possível inserir o registro."));
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
            else
                return new RetValue(false, "", new ErrorList("Auxiliar já cadastrado para a turma."));
        }

        /// <summary>
        /// Remove auxiliar de turma.
        /// </summary>
        /// <param name="disciplina">disciplina</param>
        /// <param name="turma">turma</param>
        /// <param name="ano">ano</param>
        /// <param name="semestre">semestre</param>
        /// <param name="auxiliar">auxiliar</param>
        /// <returns></returns>
        public static RetValue RemoverAuxTurma(String disciplina, String turma, decimal? ano, decimal? semestre, String auxiliar)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                CR.Ly_auxturma.Row aux = ConsultarAuxTurma(disciplina, turma, ano, semestre, auxiliar);
                if (aux != null)
                {
                    CR.Ly_auxturma.Row.Delete(connection, disciplina, turma, ano, semestre, auxiliar);
                    RetValue ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, "", new ErrorList("Não foi possível remover o registro."));
                    }
                    return null;
                }
                else
                    return new RetValue(false, "", new ErrorList("Registro não encontrado."));
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

        [Obsolete("Não apagar: utilizado pelo ODS")]
        public static void InsertMethodAuxTurma() { }
        [Obsolete("Não apagar: utilizado pelo ODS")]
        public static void DeleteMethodAuxTurma(string auxiliar) { }

        #endregion
    }
}
