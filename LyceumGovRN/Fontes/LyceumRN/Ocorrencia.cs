using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class Ocorrencia : RNBase
    {
        public static QueryTable Consultar(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = @" SELECT DISTINCT oc.aluno,
                                            oc.data,
                                            oc.ordem,
                                            oc.tipo,
                                            oc.descricao,
                                            oc.usuario,
                                            oc.ano,
                                            oc.periodo,
                                            oc.disciplina,
                                            oc.turma,
                                            doc.num_func,
                                            pe.nome_compl
                            FROM   LY_OCORRENCIA OC
                                   LEFT JOIN LY_TURMA T
                                          ON OC.TURMA = T.TURMA
                                             AND OC.ANO = T.ANO
                                             AND OC.PERIODO = T.SEMESTRE
                                             AND T.DISCIPLINA = OC.DISCIPLINA
                                   LEFT JOIN LY_DOCENTE DOC
                                          ON T.NUM_FUNC = DOC.NUM_FUNC
                                   LEFT JOIN LY_PESSOA PE
                                          ON PE.PESSOA = DOC.PESSOA
                            WHERE  OC.ALUNO = ?
                            ORDER  BY OC.DATA DESC,
                                      OC.ORDEM DESC  ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public static QueryTable ConsultarOrdem(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT MAX(oc.ordem) AS ORDEM FROM ly_ocorrencia oc LEFT JOIN ly_turma t ON oc.turma=t.turma AND oc.ano = t.ano AND oc.periodo = t.semestre WHERE oc.ALUNO=?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public static QueryTable ConsultarTurma(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT DISTINCT turma FROM ly_matricula WHERE ALUNO=?";
            if (!string.IsNullOrEmpty(aluno))
            {
                try
                {
                    qt = new QueryTable(sql);

                    qt.Query(connection, aluno);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
                return qt;


            return qt;
        }

        public static RetValue Excluir(Ly_ocorrencia.Row dadosOcorrencia)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);

            try
            {
                //Consulta o datatable para obter todas ocorrencias com o código informado
                Ly_ocorrencia dtOcorrencia = Ly_ocorrencia.Query(connection, "aluno = ? AND data=? AND tipo=? AND ordem=?", dadosOcorrencia.Aluno, dadosOcorrencia.Data, dadosOcorrencia.Tipo, dadosOcorrencia.Ordem);

                //Verifica se o datatable da ocorrência não é nulo
                if (dtOcorrencia != null)
                {
                    //Verifica se existem linhas no datatable da ocorrencia
                    if (dtOcorrencia.Rows != null)
                    {
                        foreach (Ly_ocorrencia.Row linhaOcorrencia in dtOcorrencia.Rows)
                        {
                            //Marca que alinha deve ser excluida
                            linhaOcorrencia.Delete();
                        }
                        //Chamar Put para efetuar a exclusao
                        dtOcorrencia.Put(connection);
                        retorno = VerificarErro(dtOcorrencia);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Ocorrência removida com sucesso.", null);
                    }
                }

            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        /// <summary>
        /// Método usado para inclusão de ocorrencia
        /// </summary>
        /// <param name="connection">Conexão usada para incluir a ocorrencia</param>
        /// <param name="dtOcorrencia">DataTable de Ocorrencia com os dados da ocorrencia que será incluida</param>
        public static RetValue Incluir(Ly_ocorrencia dtOcorrencia)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();

            connection.Open(true);

            RetValue valorRetorno = null;

            try
            {
                //verifica se o datatable de ocorrencia não é nulo
                if (dtOcorrencia != null)
                {
                    //verifica se existem linhas no datatable de ocorrencia
                    if (dtOcorrencia.Rows != null)
                    {

                        //salva os dados e atualiza o objeto
                        dtOcorrencia.Put(connection);

                        valorRetorno = VerificarErro(dtOcorrencia);

                        if (valorRetorno != null && !valorRetorno.Ok)
                        {
                            connection.Rollback();
                            return valorRetorno;
                        }

                        valorRetorno = new RetValue(true, "Registro incluido com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return valorRetorno;
        }


        /// <summary>
        /// Método usado para atualização de ocorrencia
        /// </summary>
        /// <param name="connection">Conexão usada para atualizar a ocorrencia</param>
        /// <param name="dtOcorrencia">DataTable de Ocorrencia com os dados da ocorrencia que será atualizada</param>
        public static RetValue Atualizar(Ly_ocorrencia dtOcorrencia)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();

            connection.Open(true);
            RetValue valorRetorno = null;

            try
            {
                //verifica se o datatable de ocorrencia não é nulo
                if (dtOcorrencia != null)
                {
                    //verifica se existem linhas no datatable de ocorrencia
                    if (dtOcorrencia.Rows != null)
                    {
                        Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.Rows[0];

                        Ly_ocorrencia dtOcorrenciaAuxiliar = ConsultarAlt(connection, dtOcorrencia.Rows[0]);

                        if (dtOcorrenciaAuxiliar != null)
                        {
                            if (dtOcorrenciaAuxiliar.Rows != null)
                            {
                                foreach (Ly_ocorrencia.Row linhaOcorrencia in dtOcorrenciaAuxiliar.Rows)
                                {
                                    linhaOcorrencia.Data = dadosOcorrencia.Data;
                                    linhaOcorrencia.Tipo = dadosOcorrencia.Tipo;
                                    linhaOcorrencia.Ordem = dadosOcorrencia.Ordem;
                                    linhaOcorrencia.Usuario = dadosOcorrencia.Usuario;
                                    linhaOcorrencia.Descricao = dadosOcorrencia.Descricao;
                                    if (dadosOcorrencia.Ano == null)
                                    {
                                        linhaOcorrencia.Ano = null;
                                        linhaOcorrencia.Periodo = null;
                                        linhaOcorrencia.Disciplina = null;
                                        linhaOcorrencia.Turma = null;
                                    }
                                    else
                                    {
                                        linhaOcorrencia.Ano = dadosOcorrencia.Ano;
                                        linhaOcorrencia.Periodo = dadosOcorrencia.Periodo;
                                        linhaOcorrencia.Disciplina = dadosOcorrencia.Disciplina;
                                        linhaOcorrencia.Turma = dadosOcorrencia.Turma;
                                    }
                                }
                            }

                        }

                        dtOcorrenciaAuxiliar.Put(connection);

                        valorRetorno = VerificarErro(dtOcorrenciaAuxiliar);

                        if (valorRetorno != null && !valorRetorno.Ok)
                        {
                            connection.Rollback();
                            return valorRetorno;
                        }

                        valorRetorno = new RetValue(true, "Registro atualizado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return valorRetorno;
        }

        public static Ly_ocorrencia ConsultarEditar(string aluno, DateTime data, string tipo, decimal ordem)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            return Ly_ocorrencia.Query(connection, "aluno = ? AND data = ? AND tipo = ? AND ordem = ?", aluno, data, tipo, ordem);
        }

        public static Ly_ocorrencia ConsultarAlt(TConnection connection, Ly_ocorrencia.Row dadosOcorrencia)
        {
            object[] parametros = new object[] { dadosOcorrencia.Aluno, dadosOcorrencia.Data, dadosOcorrencia.Tipo, dadosOcorrencia.Ordem };

            return Ly_ocorrencia.Query(connection, "aluno = ? AND data = ? AND tipo = ? AND ordem = ?", parametros);
        }

        public static QueryTable ConsultarAno(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT DISTINCT pl.ano FROM ly_periodo_letivo pl INNER JOIN LY_MATRICULA m on pl.ANO = m.ANO WHERE m.ALUNO = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public static QueryTable ConsultarPeriodo(string ano, string aluno)
        {

            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "select DISTINCT pl.ID_REDUZIDA, pl.PERIODO from ly_periodo_letivo pl INNER JOIN LY_MATRICULA m on pl.PERIODO = m.SEMESTRE WHERE m.ANO = ? AND m.ALUNO = ? ORDER BY PERIODO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, ano, aluno);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarTurma(string aluno, string disciplina, string ano, string semestre)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT DISTINCT turma FROM ly_matricula WHERE aluno =? AND disciplina=? AND ano=? AND semestre=?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno, disciplina, ano, semestre);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public static QueryTable ConsultarDadosAluno(string aluno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "select a.ALUNO as aluno, c.NOME as curso, ue.NOME_COMP as unidade_ensino, uf.NOME_COMP as unidade_fisica, s.DESCRICAO as serie, t.DESCRICAO as turno " +
                        "from ly_aluno a  " +
                        "left join ly_curso c on a.CURSO = c.CURSO  " +
                        "left join LY_UNIDADE_ENSINO ue on a.UNIDADE_ENSINO = ue.UNIDADE_ENS " +
                        "left join LY_UNIDADE_FISICA uf on a.UNIDADE_FISICA = uf.UNIDADE_FIS " +
                        "left join LY_SERIE s on (a.SERIE = s.SERIE and a.TURNO = s.TURNO and a.CURRICULO = s.CURRICULO and a.CURSO = s.CURSO )" +
                        "left join LY_TURNO t on a.TURNO = t.TURNO " +
                        "where a.aluno = ? ";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public bool PossuiOcorrenciaPor(DataContext ctx, string aluno, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM   LY_OCORRENCIA (NOLOCK) 
                                WHERE  ALUNO = @ALUNO 
                                       AND TURMA = @TURMA 
                                       AND ANO = @ANO 
                                       AND PERIODO = @PERIODO  ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }
    }
}
