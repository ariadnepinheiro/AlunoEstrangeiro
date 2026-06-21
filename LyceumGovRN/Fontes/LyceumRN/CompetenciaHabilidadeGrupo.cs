using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class CompetenciaHabilidadeGrupo
    {
        public static TceCompetenciaHabilidadeGrupo Carregar(int IdCompetenciaHabilidadeGrupo)
        {
            try
            {
                TceCompetenciaHabilidadeGrupo CHG = new TceCompetenciaHabilidadeGrupo();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM TCE_COMPETENCIA_HABILIDADE_GRUPO WHERE ID_COMPETENCIA_HABILIDADE_GRUPO = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", IdCompetenciaHabilidadeGrupo);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            CHG.IdCompetenciaHabilidadeGrupo = (int)reader["ID_COMPETENCIA_HABILIDADE_GRUPO"];
                            CHG.Disciplina = (string)reader["DISCIPLINA"];
                            CHG.Curso = (string)reader["CURSO"];
                            CHG.Modalidade = (string)reader["MODALIDADE"];
                            CHG.TipoCurso = (string)reader["TIPO_CURSO"];
                            CHG.Ano = Convert.ToInt32(reader["ANO"]);
                            CHG.Serie = Convert.ToInt32(reader["SERIE"]);
                            CHG.Periodo = Convert.ToInt32(reader["PERIODO"]);
                            CHG.Subperiodo = Convert.ToInt32(reader["SUBPERIODO"]);
                            CHG.Grupo = (string)reader["GRUPO"];
                            CHG.Ordem = Convert.ToInt32(reader["ORDEM"]);
                            CHG.Tipo = (string)reader["TIPO"];
                        }
                    }
                    return CHG;

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(TceCompetenciaHabilidadeGrupo competenciaHabilidadeGrupo)
        {
            try
            {

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO TCE_COMPETENCIA_HABILIDADE_GRUPO(DISCIPLINA,CURSO,MODALIDADE,TIPO_CURSO,ANO,SERIE,PERIODO,SUBPERIODO,GRUPO,ORDEM,MATRICULA,TIPO) 
                                VALUES (@DISCIPLINA,@CURSO,@MODALIDADE,@TIPO_CURSO,@ANO,@SERIE,@PERIODO,@SUBPERIODO,@GRUPO,@ORDEM,@MATRICULA,@TIPO) "
                    };
                    contextQuery.Parameters.Add("@CURSO", competenciaHabilidadeGrupo.Curso);
                    contextQuery.Parameters.Add("@DISCIPLINA", competenciaHabilidadeGrupo.Disciplina);
                    contextQuery.Parameters.Add("@MODALIDADE", competenciaHabilidadeGrupo.Modalidade);
                    contextQuery.Parameters.Add("@TIPO_CURSO", competenciaHabilidadeGrupo.TipoCurso);
                    contextQuery.Parameters.Add("@ANO", competenciaHabilidadeGrupo.Ano);
                    contextQuery.Parameters.Add("@SERIE", competenciaHabilidadeGrupo.Serie);
                    contextQuery.Parameters.Add("@PERIODO", competenciaHabilidadeGrupo.Periodo);
                    contextQuery.Parameters.Add("@SUBPERIODO", competenciaHabilidadeGrupo.Subperiodo);
                    contextQuery.Parameters.Add("@GRUPO", competenciaHabilidadeGrupo.Grupo);
                    contextQuery.Parameters.Add("@ORDEM", competenciaHabilidadeGrupo.Ordem);
                    contextQuery.Parameters.Add("@MATRICULA", competenciaHabilidadeGrupo.Matricula);
                    contextQuery.Parameters.Add("@TIPO", competenciaHabilidadeGrupo.Tipo);

                    return ctx.ApplyModifications(contextQuery);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(TceCompetenciaHabilidadeGrupo competenciaHabilidadeGrupo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE TCE_COMPETENCIA_HABILIDADE_GRUPO SET 
                                 GRUPO = @GRUPO,
                                 ORDEM = @ORDEM,   
                                 MATRICULA = @matricula,
                                 DT_ALTERACAO = getdate()
                                 WHERE ID_COMPETENCIA_HABILIDADE_GRUPO = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", competenciaHabilidadeGrupo.IdCompetenciaHabilidadeGrupo);
                    contextQuery.Parameters.Add("@GRUPO", competenciaHabilidadeGrupo.Grupo);
                    contextQuery.Parameters.Add("@ORDEM", competenciaHabilidadeGrupo.Ordem);
                    contextQuery.Parameters.Add("@MATRICULA", competenciaHabilidadeGrupo.Matricula);

                    return ctx.ApplyModifications(contextQuery);


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable Listar(string disciplina, int ano, int bimestre, int serie, int periodo, string curso, string tipo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                                   {
                                       Command = @"SELECT *,d.disciplina + ' - ' + d.NOME_COMPL as NOME_DISCIPLINA, CHG.TIPO AS TIPO_CURRICULO 
                                                 FROM dbo.TCE_COMPETENCIA_HABILIDADE_GRUPO CHG
                                                inner join LY_DISCIPLINA D on CHG.DISCIPLINA=D.DISCIPLINA
                                                WHERE D.DISCIPLINA = @disciplina
                                                  AND ANO = @ano
                                                  AND SUBPERIODO = @subperiodo
                                                  AND SERIE = @serie
                                                  AND PERIODO = @periodo
                                                  AND CURSO = @curso
                                                  AND CHG.TIPO = @tipo"
                                   };

                contextQuery.Parameters.Add("@disciplina", disciplina);
                contextQuery.Parameters.Add("@ano", ano);
                contextQuery.Parameters.Add("@subperiodo", bimestre);
                contextQuery.Parameters.Add("@serie", serie);
                contextQuery.Parameters.Add("@periodo", periodo);
                contextQuery.Parameters.Add("@curso", curso);
                contextQuery.Parameters.Add("@tipo", tipo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public List<DadosCompetenciaCurriculo> ListaDadosCompetenciaCurriculoPor(string disciplina, int ano, int serie, int periodo, string curso, DateTime dataFrequencia, string tipo)
        {
            CompetenciaHabilidadeItem rnCompetenciaHabilidadeItem = new CompetenciaHabilidadeItem();
            List<DadosCompetenciaCurriculo> lista = new List<DadosCompetenciaCurriculo>();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                lista = this.ListaPor(contexto, disciplina, ano, serie, periodo, curso, dataFrequencia.Date, tipo);

                foreach (DadosCompetenciaCurriculo grupo in lista)
                {
                    grupo.ListaItem = rnCompetenciaHabilidadeItem.ListaPor(contexto, grupo.GrupoId);
                }

                return lista;
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

        private List<DadosCompetenciaCurriculo> ListaPor(DataContext contexto, string disciplina, int ano, int serie, int periodo, string curso, DateTime dataFrequencia, string tipo)
        {
            List<DadosCompetenciaCurriculo> grupos = new List<DadosCompetenciaCurriculo>();
            DadosCompetenciaCurriculo dadosGrupo = new DadosCompetenciaCurriculo();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT G.ID_COMPETENCIA_HABILIDADE_GRUPO, 
		                                            G.GRUPO, 
		                                            G.ORDEM,
		                                            G.SUBPERIODO,
                                                    G.TIPO
                                            FROM DBO.TCE_COMPETENCIA_HABILIDADE_GRUPO G (NOLOCK)
	                                            INNER JOIN LY_SUBPERIODO_LETIVO S (NOLOCK) ON G.ANO = S.ANO	
												                                            AND G.PERIODO = S.PERIODO
												                                            AND G.SUBPERIODO = S.SUBPERIODO
	                                            INNER JOIN TCE_COMPETENCIA_HABILIDADE_ITEM I (NOLOCK) ON G.ID_COMPETENCIA_HABILIDADE_GRUPO = I.ID_COMPETENCIA_HABILIDADE_GRUPO                                            		
                                            WHERE G.DISCIPLINA = @DISCIPLINA
		                                            AND G.ANO = @ANO		
		                                            AND G.SERIE = @SERIE
		                                            AND G.PERIODO = @PERIODO
		                                            AND G.CURSO = @CURSO
                                                    AND G.TIPO = @TIPO
		                                            AND @DATAFREQUENCIA BETWEEN S.DT_INICIO AND S.DT_FIM
                                            ORDER BY G.ORDEM ";

                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);
                contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosGrupo = new DadosCompetenciaCurriculo();

                    dadosGrupo.GrupoId = Convert.ToInt32(reader["ID_COMPETENCIA_HABILIDADE_GRUPO"]);
                    dadosGrupo.Grupo = Convert.ToString(reader["GRUPO"]);
                    dadosGrupo.Ordem = Convert.ToInt32(reader["ORDEM"]);
                    dadosGrupo.Subperiodo = Convert.ToInt32(reader["SUBPERIODO"]);
                    dadosGrupo.Tipo = Convert.ToString(reader["TIPO"]);

                    grupos.Add(dadosGrupo);
                }

                return grupos;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static int Remover(int idCompetenciaHabilidadeGrupo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM TCE_COMPETENCIA_HABILIDADE_GRUPO WHERE ID_COMPETENCIA_HABILIDADE_GRUPO = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idCompetenciaHabilidadeGrupo);

                    return ctx.ApplyModifications(contextQuery);
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static ValidacaoDados Validar(TceCompetenciaHabilidadeGrupo competenciaHabilidadeGrupo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (competenciaHabilidadeGrupo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (competenciaHabilidadeGrupo.IdCompetenciaHabilidadeGrupo <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (competenciaHabilidadeGrupo.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DISCIPLINA é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Grupo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo GRUPO é obrigatório.");
            }
            else
            {
                if (competenciaHabilidadeGrupo.Grupo.Length > 200)
                {
                    mensagens.Add("Campo DESCRIÇÃO deve conter no máximo 200 caracteres!");
                }
            }

            if (competenciaHabilidadeGrupo.Modalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MODALIDADE é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Ordem <= 0)
            {
                mensagens.Add("Campo ORDEM é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Serie <= 0)
            {
                mensagens.Add("Campo SERIE é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Subperiodo <= 0)
            {
                mensagens.Add("Campo SUBPERIODO é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.TipoCurso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO CURSO é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO CURRICULO é obrigatório.");
            }

            if (competenciaHabilidadeGrupo.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verfica se existe outro grupo com msm nome 
                    if (PossuiOutraDescricaoCadastradaPor(contexto, competenciaHabilidadeGrupo.Grupo, competenciaHabilidadeGrupo.Tipo, competenciaHabilidadeGrupo.Ano, competenciaHabilidadeGrupo.Periodo, competenciaHabilidadeGrupo.Subperiodo, competenciaHabilidadeGrupo.Curso, competenciaHabilidadeGrupo.Serie, competenciaHabilidadeGrupo.Disciplina, competenciaHabilidadeGrupo.IdCompetenciaHabilidadeGrupo))
                    {
                        mensagens.Add("Este GRUPO já foi cadastrado para este TIPO CURRICULO/ANO/PERIODO/BIMESTRE/CURSO/SERIE/DISPLINA.");
                    }

                    //Verfica se existe outro grupo com msm ordem 
                    if (PossuiOutraOrdemCadastradaPor(contexto, competenciaHabilidadeGrupo.Ordem, competenciaHabilidadeGrupo.Tipo, competenciaHabilidadeGrupo.Ano, competenciaHabilidadeGrupo.Periodo, competenciaHabilidadeGrupo.Subperiodo, competenciaHabilidadeGrupo.Curso, competenciaHabilidadeGrupo.Serie, competenciaHabilidadeGrupo.Disciplina, competenciaHabilidadeGrupo.IdCompetenciaHabilidadeGrupo))
                    {
                        mensagens.Add("Esta ORDEM já foi cadastrado para este TIPO CURRICULO/ANO/PERIODO/BIMESTRE/CURSO/SERIE/DISPLINA.");
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

        private static bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string grupo, string tipo, int ano, int periodo, int subperiodo, string curso, int serie, string disciplina, int idCompetenciaHabilidadeGrupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM TCE_COMPETENCIA_HABILIDADE_GRUPO (NOLOCK)
                                WHERE GRUPO = @GRUPO
	                                AND ANO = @ANO
	                                AND PERIODO = @PERIODO
	                                AND TIPO = @TIPO
	                                AND SUBPERIODO = @SUBPERIODO
	                                AND DISCIPLINA = @DISCIPLINA
	                                AND SERIE = @SERIE
	                                AND CURSO = @CURSO
	                                AND ID_COMPETENCIA_HABILIDADE_GRUPO <> @ID_COMPETENCIA_HABILIDADE_GRUPO ";

            contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_GRUPO", SqlDbType.Int, idCompetenciaHabilidadeGrupo);
            contextQuery.Parameters.Add("@GRUPO", SqlDbType.VarChar, grupo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);
            contextQuery.Parameters.Add("@SUBPERIODO", SqlDbType.Int, subperiodo);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private static bool PossuiOutraOrdemCadastradaPor(DataContext ctx, int ordem, string tipo, int ano, int periodo, int subperiodo, string curso, int serie, string disciplina, int idCompetenciaHabilidadeGrupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM TCE_COMPETENCIA_HABILIDADE_GRUPO (NOLOCK)
                                WHERE ORDEM = @ORDEM
	                                AND ANO = @ANO
	                                AND PERIODO = @PERIODO
	                                AND TIPO = @TIPO
	                                AND SUBPERIODO = @SUBPERIODO
	                                AND DISCIPLINA = @DISCIPLINA
	                                AND SERIE = @SERIE
	                                AND CURSO = @CURSO
	                                AND ID_COMPETENCIA_HABILIDADE_GRUPO <> @ID_COMPETENCIA_HABILIDADE_GRUPO ";

            contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_GRUPO", SqlDbType.Int, idCompetenciaHabilidadeGrupo);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, ordem);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);
            contextQuery.Parameters.Add("@SUBPERIODO", SqlDbType.Int, subperiodo);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static ValidacaoDados ValidarExclusao(TceCompetenciaHabilidadeGrupo competenciaHabilidadeGrupo)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT 1  FROM   [dbo].[TCE_COMPETENCIA_HABILIDADE_ITEM] 
                                        WHERE  ID_COMPETENCIA_HABILIDADE_GRUPO= @ID ";
                contextQuery.Parameters.Add("@ID", competenciaHabilidadeGrupo.IdCompetenciaHabilidadeGrupo);


                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Este grupo não pode ser excluído devido existir competências/habilidades vinculadas.";

                }
            }

            return validacao;
        }

        public DataTable ListaTipoGrupo()
        {
            DataTable table = new DataTable();
            table.Columns.Add("TIPO").DataType = typeof(string);
            table.Rows.Add("BÁSICO");
            table.Rows.Add("ESSENCIALIZADO");
            table.Rows.Add("RECOMPOSIÇÃO");

            return table;
        }
    }
}