namespace Techne.Lyceum.RN
{
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using System;
    using Techne.Lyceum.RN.Util;
    using Techne.Lyceum.RN.DTOs;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class CompetenciaHabilidadeItem
    {
        public static TceCompetenciaHabilidadeItem Carregar(int IdCompetenciaHabilidadeItem)
        {
            try
            {
                TceCompetenciaHabilidadeItem CHI = new TceCompetenciaHabilidadeItem();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM TCE_COMPETENCIA_HABILIDADE_ITEM WHERE ID_COMPETENCIA_HABILIDADE_ITEM = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", IdCompetenciaHabilidadeItem);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            CHI.IdCompetenciaHabilidadeItem = (int)reader["ID_COMPETENCIA_HABILIDADE_ITEM"];
                            CHI.IdCompetenciaHabilidadeGrupo = (int)reader["ID_COMPETENCIA_HABILIDADE_GRUPO"];
                            CHI.CompetenciaHabilidade = (string)reader["COMPETENCIA_HABILIDADE"];
                            CHI.Ordem = (int)reader["ORDEM"];
                        }
                    }
                    return CHI;

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static int Inserir(TceCompetenciaHabilidadeItem competenciaHabilidadeItem)
        {
            try
            {

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO TCE_COMPETENCIA_HABILIDADE_ITEM(ID_COMPETENCIA_HABILIDADE_GRUPO,COMPETENCIA_HABILIDADE,ORDEM,MATRICULA) 
                                VALUES (@ID_COMPETENCIA_HABILIDADE_GRUPO,@COMPETENCIA_HABILIDADE,@ORDEM,@MATRICULA) "
                    };
                    contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_GRUPO", competenciaHabilidadeItem.IdCompetenciaHabilidadeGrupo );
                    contextQuery.Parameters.Add("@COMPETENCIA_HABILIDADE", competenciaHabilidadeItem.CompetenciaHabilidade);
                    contextQuery.Parameters.Add("@ORDEM", competenciaHabilidadeItem.Ordem);
                    contextQuery.Parameters.Add("@MATRICULA", competenciaHabilidadeItem.Matricula);

                    return ctx.ApplyModifications(contextQuery);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(TceCompetenciaHabilidadeItem competenciaHabilidadeItem)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE TCE_COMPETENCIA_HABILIDADE_ITEM SET 
                                 COMPETENCIA_HABILIDADE = @COMPETENCIA_HABILIDADE,
                                 ORDEM = @ORDEM,   
                                 MATRICULA = @matricula,
                                 DT_ALTERACAO = getdate()
                                 WHERE ID_COMPETENCIA_HABILIDADE_ITEM = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", competenciaHabilidadeItem.IdCompetenciaHabilidadeItem);
                    contextQuery.Parameters.Add("@COMPETENCIA_HABILIDADE", competenciaHabilidadeItem.CompetenciaHabilidade);
                    contextQuery.Parameters.Add("@ORDEM", competenciaHabilidadeItem.Ordem);
                    contextQuery.Parameters.Add("@MATRICULA", competenciaHabilidadeItem.Matricula);

                    return ctx.ApplyModifications(contextQuery);


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable Listar(int grupo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"select * from TCE_COMPETENCIA_HABILIDADE_ITEM I
                            INNER JOIN TCE_COMPETENCIA_HABILIDADE_GRUPO G ON I.ID_COMPETENCIA_HABILIDADE_GRUPO = G.ID_COMPETENCIA_HABILIDADE_GRUPO 
                            WHERE I.ID_COMPETENCIA_HABILIDADE_GRUPO = @grupo
                                                 "
                };

                contextQuery.Parameters.Add("@grupo", grupo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public List<DadosCompetenciaItem> ListaPor(DataContext contexto, int competenciaHabilidadeGrupoId)
        {
            List<DadosCompetenciaItem> itens = new List<DadosCompetenciaItem>();
            DadosCompetenciaItem dadosItem = new DadosCompetenciaItem();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT ID_COMPETENCIA_HABILIDADE_ITEM,
		                                            ID_COMPETENCIA_HABILIDADE_GRUPO,
		                                            COMPETENCIA_HABILIDADE,
		                                            ORDEM
                                            FROM TCE_COMPETENCIA_HABILIDADE_ITEM (NOLOCK)
                                            WHERE ID_COMPETENCIA_HABILIDADE_GRUPO = @ID_COMPETENCIA_HABILIDADE_GRUPO
                                            ORDER BY ORDEM   ";


                contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_GRUPO", SqlDbType.Int, competenciaHabilidadeGrupoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosItem = new DadosCompetenciaItem();

                    dadosItem.ItemId = Convert.ToInt32(reader["ID_COMPETENCIA_HABILIDADE_ITEM"]);
                    dadosItem.GrupoId = Convert.ToInt32(reader["ID_COMPETENCIA_HABILIDADE_GRUPO"]);
                    dadosItem.CompetenciaHabilidade = Convert.ToString(reader["COMPETENCIA_HABILIDADE"]);
                    dadosItem.Ordem = Convert.ToInt32(reader["ORDEM"]);

                    itens.Add(dadosItem);
                }

                return itens;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static ICollection<GrupoItemCompetenciaHabilidade> ListarComGrupos(FiltroGrupoCompetenciaHabilidade filtro)
        {
            var dic = new Dictionary<string, GrupoItemCompetenciaHabilidade>();

            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" select G.GRUPO, I.*
                                ,ISNULL((select 1 from dbo.TCE_COMPETENCIA_HABILIDADE_DOCENTE D 
                                where D.ID_COMPETENCIA_HABILIDADE_ITEM = I.ID_COMPETENCIA_HABILIDADE_ITEM 
                                AND D.MATRICULA = @matricula
                                AND D.DISCIPLINA = @disciplina
                                AND D.TURMA = @turma
                                AND D.ANO = @ano
                                AND D.PERIODO = @periodo
                                AND D.SUBPERIODO = @subperiodo),0) AS RESPOSTA

                        from TCE_COMPETENCIA_HABILIDADE_ITEM I
                        INNER JOIN TCE_COMPETENCIA_HABILIDADE_GRUPO G 
                        ON I.ID_COMPETENCIA_HABILIDADE_GRUPO = G.ID_COMPETENCIA_HABILIDADE_GRUPO 
                        WHERE G.DISCIPLINA = @disciplina
                        and G.CURSO = @curso
                        and G.MODALIDADE = @modalidade
                        and G.TIPO_CURSO = @tipoCurso
                        and G.ANO = @ano
                        and G.SERIE = @serie
                        and G.PERIODO = @periodo
                        and G.SUBPERIODO = @subperiodo 
                        ORDER BY G.ORDEM , I.ORDEM"
                    };

                    contextQuery.Parameters.Add("@disciplina", filtro.Disciplina);
                    contextQuery.Parameters.Add("@curso", filtro.Curso);
                    contextQuery.Parameters.Add("@modalidade", filtro.Modalidade);
                    contextQuery.Parameters.Add("@tipoCurso", filtro.TipoCurso);
                    contextQuery.Parameters.Add("@ano", filtro.Ano);
                    contextQuery.Parameters.Add("@serie", filtro.Serie);
                    contextQuery.Parameters.Add("@periodo", filtro.Periodo);
                    contextQuery.Parameters.Add("@subperiodo", filtro.Subperiodo);
                    contextQuery.Parameters.Add("@matricula", filtro.Matricula);
                    contextQuery.Parameters.Add("@turma", filtro.Turma);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            var grupo = (string)reader["GRUPO"];

                            var i = new RespostaItemCompetenciaHabilidade
                            {
                                IdCompetenciaHabilidadeItem = (int)reader["ID_COMPETENCIA_HABILIDADE_ITEM"],
                                IdCompetenciaHabilidadeGrupo = (int)reader["ID_COMPETENCIA_HABILIDADE_GRUPO"],
                                CompetenciaHabilidade = (string)reader["COMPETENCIA_HABILIDADE"],
                                Resposta = Convert.ToBoolean(reader["RESPOSTA"]),
                                DtCadastro = Convert.ToDateTime(reader["DT_CADASTRO"])
                            };

                            if (!dic.ContainsKey(grupo))
                            {
                                dic[grupo] = new GrupoItemCompetenciaHabilidade
                                {
                                    Grupo = grupo,
                                    IdCompetenciaHabilidadeGrupo = i.IdCompetenciaHabilidadeGrupo
                                };
                            }

                            dic[grupo].Itens.Add(i);
                        }
                    }
                }
                return dic.Values;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int Remover(int idCompetenciaHabilidadeItem)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "DELETE FROM TCE_COMPETENCIA_HABILIDADE_ITEM WHERE ID_COMPETENCIA_HABILIDADE_ITEM = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idCompetenciaHabilidadeItem);

                    return ctx.ApplyModifications(contextQuery);
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static ValidacaoDados Validar(TceCompetenciaHabilidadeItem competenciaHabilidadeItem)
        {
            
            List<string> mensagens = new List<string>();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (competenciaHabilidadeItem == null)
            {
                return validacaoDados;
            }

            if (competenciaHabilidadeItem.Ordem <= 0)
            {
                mensagens.Add("Campo ORDEM é obrigatório.");
            }

            if (competenciaHabilidadeItem.CompetenciaHabilidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo COMPETÊNCIA/HABILIDADE é obrigatório.");
            }

            if (mensagens.Count == 0)
            {

                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @"SELECT 1 FROM [TCE_COMPETENCIA_HABILIDADE_ITEM] WHERE  
                                    ID_COMPETENCIA_HABILIDADE_GRUPO = @ID_COMPETENCIA_HABILIDADE_GRUPO
                                     and (COMPETENCIA_HABILIDADE = @COMPETENCIA_HABILIDADE
                                    or ORDEM = @ORDEM)";

                    if (competenciaHabilidadeItem.IdCompetenciaHabilidadeItem != 0)
                        contextQuery.Command += " AND ID_COMPETENCIA_HABILIDADE_ITEM <> @ID_COMPETENCIA_HABILIDADE_ITEM ";

                    contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_GRUPO", competenciaHabilidadeItem.IdCompetenciaHabilidadeGrupo);
                    contextQuery.Parameters.Add("@COMPETENCIA_HABILIDADE", competenciaHabilidadeItem.CompetenciaHabilidade);
                    contextQuery.Parameters.Add("@ORDEM", competenciaHabilidadeItem.Ordem);
                    contextQuery.Parameters.Add("@ID_COMPETENCIA_HABILIDADE_ITEM", competenciaHabilidadeItem.IdCompetenciaHabilidadeItem);


                    object obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)                   
                    {                        
                        mensagens.Add("Já existe uma Competência/Habilidade com este nome e/ou ordem.");
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static ValidacaoDados ValidarExclusao(int idCompetenciaHabilidadeItem)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT 1  FROM   [dbo].[TCE_COMPETENCIA_HABILIDADE_DOCENTE] 
                                        WHERE  ID_COMPETENCIA_HABILIDADE_ITEM= @ID ";
                contextQuery.Parameters.Add("@ID", idCompetenciaHabilidadeItem);


                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Esta competência/habilidade não pode ser excluída devido existir currículos minimo vinculados.";

                }
            }

            return validacao;
        }

        public int ObtemQuantidadeTipoPor(DataContext contexto, List<int> listaItensId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                string ids = string.Empty;

                foreach (int id in listaItensId)
                {
                    if (!ids.IsNullOrEmptyOrWhiteSpace())
                    {
                        ids += ", ";
                    }

                    ids += id.ToString();
                }               

                contextQuery.Command = string.Format(@" SELECT COUNT(DISTINCT TIPO) TIPOS
                                        FROM  TCE_COMPETENCIA_HABILIDADE_ITEM I
	                                        INNER JOIN TCE_COMPETENCIA_HABILIDADE_GRUPO G ON I.ID_COMPETENCIA_HABILIDADE_GRUPO = G.ID_COMPETENCIA_HABILIDADE_GRUPO
                                        WHERE ID_COMPETENCIA_HABILIDADE_ITEM IN ({0}) ", ids);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TIPOS"]);
                }

                return retorno;
            }           
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                
            }
        }
    }
}