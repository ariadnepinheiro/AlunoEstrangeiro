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
    using System.Data;
    using Seeduc.Infra.Data;

    public class FormacaoPessoal : RNBase
    {
        /// Método usado para verificar se um registro pode ser incluido
        public static QueryTable ConsultarInclusao(string pessoa, string titulacao, string tipo, string grande_area, string outra_faculdade, DateTime dtini)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT 1 FROM Ly_curriculo_pessoa WHERE PESSOA =? AND TITULACAO= ? AND TIPO =? AND GRANDE_AREA=? AND OUTRA_FACULDADE=? AND DTINI=?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pessoa, titulacao, tipo, grande_area, outra_faculdade, dtini);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }


        public static QueryTable ConsultarGraduacaoConcluida(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @"SELECT ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO
                               FROM tce_formacao_pessoal 
                               WHERE pessoa = ? and 
                                     escolaridade like 'Superior%' and 
                                     situacao_curso='Concluído' and
                                     id_formacao_pessoal <> ?   
                               ORDER BY situacao_curso";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value,identityDescarte.Value );
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarGraduacaoAndamento(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @"SELECT ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO
                               FROM tce_formacao_pessoal 
                               WHERE pessoa = ? and 
                                     escolaridade='Superior' and 
                                     situacao_curso!='Concluído' and
                                     id_formacao_pessoal <> ?   
                               ORDER BY situacao_curso";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value, identityDescarte.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarPosGraduacao(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @"SELECT ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO
                               FROM tce_formacao_pessoal 
                               WHERE pessoa = ? and 
                                     substring(escolaridade,1,8)='Pós-Grad' and
                                     id_formacao_pessoal <> ?    
                               ORDER BY situacao_curso";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value, identityDescarte.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarEnsinoMedio(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @"SELECT ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO
                               FROM tce_formacao_pessoal 
                               WHERE pessoa = ? and 
                                     substring(escolaridade,1,12)='Ensino Médio' and
                                     id_formacao_pessoal <> ?    
                               ORDER BY situacao_curso";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value, identityDescarte.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarGraduacaoConcluidaOUEnsinoMedio(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @" select ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO 
                                        from tce_formacao_pessoal 
                                        where SITUACAO_CURSO = 'concluido' 
                                        and (ESCOLARIDADE like '%superior%' or ESCOLARIDADE like '%ensino médio%') 
                                        and pessoa = ? and id_formacao_pessoal <> ?
                                        order by situacao_curso";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value, identityDescarte.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarEnsinoMedioConcluido(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @"  select ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO 
                                        from tce_formacao_pessoal 
                                        where SITUACAO_CURSO = 'concluido' 
                                        and  eSCOLARIDADE like '%ensino médio%'
                                        and pessoa = ?
                                        and id_formacao_pessoal <> ?
                                        order by situacao_curso ";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value, identityDescarte.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarGraduacaoAndamentoENaoEnsinoMedio(decimal? codigoPessoa, decimal? identityDescarte)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                String sql = @"SELECT ID_FORMACAO_PESSOAL,PESSOA,ESCOLARIDADE,SITUACAO_CURSO
                                   FROM tce_formacao_pessoal 
                                       WHERE pessoa = ? and 
                                       ((escolaridade like '%Superior%' and 
                                       situacao_curso='Em Andamento') and 
                                       (substring(escolaridade,1,12)!='Ensino Médio')) and
                                       id_formacao_pessoal <> ?
                                       ORDER BY situacao_curso";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, codigoPessoa.Value, identityDescarte.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        //retorna a chave de um registro
        public static string ConsultarChave(string pessoa, string titulacao, string tipo, string grande_area, string outra_faculdade, DateTime dtini)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            string chave;

            try
            {
                chave = Convert.ToString(TCommand.ExecuteScalar(connection, "SELECT chave FROM Ly_curriculo_pessoa WHERE PESSOA =? AND TITULACAO= ? AND TIPO =? AND GRANDE_AREA=? AND OUTRA_FACULDADE=? AND DTINI=?", pessoa, titulacao, tipo, grande_area, outra_faculdade, dtini));
            }
            finally
            {
                connection.Close();
            }

            return chave;
        }

        public static TceFormacaoPessoal Carregar(int IdFormacaoPessoal)
        {
            try
            {
                TceFormacaoPessoal FP = new TceFormacaoPessoal();

                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM TCE_FORMACAO_PESSOAL WHERE ID_FORMACAO_PESSOAL = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", IdFormacaoPessoal);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            FP.IdCursoFormacaoPessoal = (int)reader["ID_FORMACAO_PESSOAL"];

                            var cPessoa = reader["PESSOA"].ToString();

                            FP.Pessoa = int.Parse(cPessoa);
                            FP.Escolaridade = (string)reader["ESCOLARIDADE"];
                            FP.SituacaoCurso = (string)reader["SITUACAO_CURSO"];
                            FP.IdCursoFormacaoPessoal = (int)reader["ID_CURSO_FORMACAO_PESSOAL"];
                            FP.FormacaoComplementacaoPedagogica = (string)reader["FORMACAO_COMPLEMENTACAO_PEDAGOGICA"];
                            FP.AnoInicio = (int)reader["ANO_INICIO"];
                            FP.AnoConclusao = (int)reader["ANO_CONCLUSAO"];
                            FP.IdInstituicao = (string)reader["ID_INSTITUICAO"];
                            FP.Matricula = (string)reader["MATRICULA"];
                            FP.DtCadastro = (DateTime)reader["DT_CADASTRO"];
                        }
                    }
                    return FP;
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Inserir(TceFormacaoPessoal formacaopessoal)
        {
            try
            {

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "insert into dbo.TCE_FORMACAO_PESSOAL (PESSOA, ESCOLARIDADE, SITUACAO_CURSO, ID_CURSO_FORMACAO_PESSOAL, FORMACAO_COMPLEMENTACAO_PEDAGOGICA, ANO_INICIO, ANO_CONCLUSAO, ID_INSTITUICAO, MATRICULA,DOC_COMPROBATORIO) values (@PESSOA, @ESCOLARIDADE, @SITUACAO_CURSO, @ID_CURSO_FORMACAO_PESSOAL, @FORMACAO_COMPLEMENTACAO_PEDAGOGICA, @ANO_INICIO, @ANO_CONCLUSAO, @ID_INSTITUICAO, @MATRICULA, @DOC_COMPROBATORIO) "
                    };
                    contextQuery.Parameters.Add("@PESSOA", formacaopessoal.Pessoa);
                    contextQuery.Parameters.Add("@ESCOLARIDADE", formacaopessoal.Escolaridade);
                    contextQuery.Parameters.Add("@SITUACAO_CURSO", formacaopessoal.SituacaoCurso);
                    contextQuery.Parameters.Add("@ID_CURSO_FORMACAO_PESSOAL", formacaopessoal.IdCursoFormacaoPessoal);
                    contextQuery.Parameters.Add("@FORMACAO_COMPLEMENTACAO_PEDAGOGICA", formacaopessoal.FormacaoComplementacaoPedagogica);
                    contextQuery.Parameters.Add("@ANO_INICIO", formacaopessoal.AnoInicio);
                    contextQuery.Parameters.Add("@ANO_CONCLUSAO", formacaopessoal.AnoConclusao);
                    contextQuery.Parameters.Add("@ID_INSTITUICAO", formacaopessoal.IdInstituicao);
                    contextQuery.Parameters.Add("@MATRICULA", formacaopessoal.Matricula);
                    contextQuery.Parameters.Add("@DOC_COMPROBATORIO", formacaopessoal.Doc_comprobatorio);
                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int Alterar(TceFormacaoPessoal formacaopessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"update dbo.TCE_FORMACAO_PESSOAL
                                    set 
                                    ESCOLARIDADE = @ESCOLARIDADE,
                                    SITUACAO_CURSO = @SITUACAO_CURSO,
                                    ID_CURSO_FORMACAO_PESSOAL = @ID_CURSO_FORMACAO_PESSOAL,
                                    FORMACAO_COMPLEMENTACAO_PEDAGOGICA = @FORMACAO_COMPLEMENTACAO_PEDAGOGICA,
                                    ANO_INICIO = @ANO_INICIO,
                                    ANO_CONCLUSAO = @ANO_CONCLUSAO,
                                    ID_INSTITUICAO = @ID_INSTITUICAO,
                                    MATRICULA = @MATRICULA,
                                    DOC_COMPROBATORIO = @DOC_COMPROBATORIO,
                                    DT_ALTERACAO = GETDATE()
                                    where ID_FORMACAO_PESSOAL = @ID_FORMACAO_PESSOAL "
                    };
                    contextQuery.Parameters.Add("@ID_FORMACAO_PESSOAL", formacaopessoal.IdFormacaoPessoal);
                    contextQuery.Parameters.Add("@ESCOLARIDADE", formacaopessoal.Escolaridade);
                    contextQuery.Parameters.Add("@SITUACAO_CURSO", formacaopessoal.SituacaoCurso);
                    contextQuery.Parameters.Add("@ID_CURSO_FORMACAO_PESSOAL", formacaopessoal.IdCursoFormacaoPessoal);
                    contextQuery.Parameters.Add("@FORMACAO_COMPLEMENTACAO_PEDAGOGICA", formacaopessoal.FormacaoComplementacaoPedagogica);
                    contextQuery.Parameters.Add("@ANO_INICIO", formacaopessoal.AnoInicio);
                    contextQuery.Parameters.Add("@ANO_CONCLUSAO", formacaopessoal.AnoConclusao);
                    contextQuery.Parameters.Add("@ID_INSTITUICAO", formacaopessoal.IdInstituicao);
                    contextQuery.Parameters.Add("@MATRICULA", formacaopessoal.Matricula);
                    contextQuery.Parameters.Add("@DOC_COMPROBATORIO", formacaopessoal.Doc_comprobatorio);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static int DeletarFormacaoPessoalAdicional(TceFormacaoEstudoAdicional formacaopessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"delete from dbo.FORMACAOPESSOAL_ESTUDOADICIONAL
                                    where FORMACAOPESSOALID=@FORMACAOPESSOALID and 
                                          ESTUDOADICIONALID=@ESTUDOADICIONALID"
                    };
                    contextQuery.Parameters.Add("@FORMACAOPESSOALID", formacaopessoal.FormacaoPessoalID );
                    contextQuery.Parameters.Add("@ESTUDOADICIONALID", formacaopessoal.EstudoAdicionalID);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int DeletarFormacaoPessoalAdicional(int FormacaoPessoalID)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"delete from dbo.FORMACAOPESSOAL_ESTUDOADICIONAL
                                    where FORMACAOPESSOALID=@FORMACAOPESSOALID"
                    };
                    contextQuery.Parameters.Add("@FORMACAOPESSOALID", FormacaoPessoalID);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int ObtemIdentityFormacao()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            int ID;
            DbObject dbID;
            try
            {
                dbID = TCommand.ExecuteScalar(connection, "select max(id_formacao_pessoal) from TCE_FORMACAO_PESSOAL", 1);
            }
            finally
            {
                connection.Close();
            }
            if (!dbID.IsNull)
            {
                ID = (int)dbID;
                return ID ;
            }
            return 1;
        }


        public static int IncluirFormacaoPessoalAdicional(TceFormacaoEstudoAdicional formacaopessoal)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO FORMACAOPESSOAL_ESTUDOADICIONAL(FORMACAOPESSOALID,ESTUDOADICIONALID)
                                    VALUES(@FORMACAOPESSOALID,@ESTUDOADICIONALID)                                    
                                  "
                    };
                    contextQuery.Parameters.Add("@ESTUDOADICIONALID", formacaopessoal.EstudoAdicionalID);
                    contextQuery.Parameters.Add("@FORMACAOPESSOALID", formacaopessoal.FormacaoPessoalID);

                    return ctx.ApplyModifications(contextQuery);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }






























        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT * FROM TCE_FORMACAO_PESSOAL "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPessoa(string PESSOA)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock() )
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT FP.*, CURSO, AREA, NOME_COMP, TIPO_ORIGEM , AFP.AREA + ' - ' + CFP.CURSO AS AREA_CURSO
                              , CAST(CFP.ID_AREA_FORMACAO_PESSOAL AS VARCHAR) + '-' + CAST(CFP.ID_CURSO_FORMACAO_PESSOAL AS VARCHAR) AS CODIGO,
                            AFP.ID_AREA_FORMACAO_PESSOAL AS CODIGOAREA, CFP.ID_CURSO_FORMACAO_PESSOAL AS CODIGOCURSO,
                            I.TIPO_ORIGEM AS TIPOINSTITUICAO
                            FROM TCE_FORMACAO_PESSOAL FP
                            INNER JOIN TCE_CURSO_FORMACAO_PESSOAL CFP ON FP.ID_CURSO_FORMACAO_PESSOAL = CFP.ID_CURSO_FORMACAO_PESSOAL
                            INNER JOIN dbo.TCE_AREA_FORMACAO_PESSOAL AFP ON AFP.ID_AREA_FORMACAO_PESSOAL = CFP.ID_AREA_FORMACAO_PESSOAL
                            INNER JOIN LY_INSTITUICAO I ON I.OUTRA_FACULDADE = FP.ID_INSTITUICAO 
                            WHERE PESSOA = @PESSOA"
                };
                contextQuery.Parameters.Add("@PESSOA", PESSOA);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPessoaDocentes(string PESSOA)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT FP.*, CURSO, AREA, NOME_COMP, TIPO_ORIGEM , AFP.AREA + ' - ' + CFP.CURSO AS AREA_CURSO
                              , CAST(CFP.ID_AREA_FORMACAO_PESSOAL AS VARCHAR) + '-' + CAST(CFP.ID_CURSO_FORMACAO_PESSOAL AS VARCHAR) AS CODIGO,
                            AFP.ID_AREA_FORMACAO_PESSOAL AS CODIGOAREA
                            FROM TCE_FORMACAO_PESSOAL FP
                            INNER JOIN TCE_CURSO_FORMACAO_PESSOAL CFP ON FP.ID_CURSO_FORMACAO_PESSOAL = CFP.ID_CURSO_FORMACAO_PESSOAL
                            INNER JOIN dbo.TCE_AREA_FORMACAO_PESSOAL AFP ON AFP.ID_AREA_FORMACAO_PESSOAL = CFP.ID_AREA_FORMACAO_PESSOAL
                            INNER JOIN LY_INSTITUICAO I ON I.OUTRA_FACULDADE = FP.ID_INSTITUICAO 
                            WHERE PESSOA = @PESSOA"
                };
                contextQuery.Parameters.Add("@PESSOA", PESSOA);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarDisciplinaAdicional(string formacaopessoalid)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  CONVERT(VARCHAR,FEA.FORMACAOPESSOALID) + '-' + CONVERT(VARCHAR,FEA.ESTUDOADICIONALID) AS ID_COMPOSTO,
		                                FEA.FORMACAOPESSOALID,
		                                EA.ESTUDOADICIONALID,
                                        EA.NOME AS NOME_DISCIPLINA_ADICIONAL  
                                FROM FORMACAOPESSOAL_ESTUDOADICIONAL FEA 
                                INNER JOIN estudoadicional EA on FEA.ESTUDOADICIONALID=EA.ESTUDOADICIONALID
                                WHERE FEA.FORMACAOPESSOALID = @FORMACAOPESSOALID"
                };
                contextQuery.Parameters.Add("@FORMACAOPESSOALID", formacaopessoalid);

                return ctx.GetDataTable(contextQuery);
            }
        }

        //public static int Remover(int idFormacaoPessoal)
        //{
        //    try
        //    {
        //        using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
        //        {
        //            var contextQuery = new ContextQuery
        //            {
        //                Command = "DELETE FROM TCE_FORMACAO_PESSOAL WHERE ID_FORMACAO_PESSOAL = @ID "
        //            };
        //            contextQuery.Parameters.Add("@ID", idFormacaoPessoal);

        //            return ctx.ApplyModifications(contextQuery);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public static void ExcluirFormacaoPessoal(DataContext context, int idFormacaoPessoal)
        {
            var contextQuery = new ContextQuery(
                @" DELETE FROM TCE_FORMACAO_PESSOAL WHERE ID_FORMACAO_PESSOAL = @ID ");

            contextQuery.Parameters.Add("@ID", idFormacaoPessoal);

            context.ApplyModifications(contextQuery);
        }

        public static void ExcluirEstudoAdicional(DataContext context, int idEstudoAdicional)
        {
            var contextQuery = new ContextQuery(
                @" DELETE FROM FORMACAOPESSOAL_ESTUDOADICIONAL WHERE FORMACAOPESSOALID = @ID ");

            contextQuery.Parameters.Add("@ID", idEstudoAdicional);

            context.ApplyModifications(contextQuery);
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
                    //CtvConfTurnoInicial.Excluir(ctx, id);
                    //CtvConfTurno.Excluir(ctx, id);
                    ExcluirEstudoAdicional(ctx, id);
                    ExcluirFormacaoPessoal(ctx, id);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }
                
        public static ValidacaoDados ValidarPreRequisito(TceFormacaoPessoal formacaopessoal)
        {
            var validacao = new ValidacaoDados();
            var idPessoa = formacaopessoal.Pessoa; 
            var idContador = formacaopessoal.IdFormacaoPessoal; 
            
            var cEscolaridade = formacaopessoal.Escolaridade.ToString();
            var cSituacaoCurso = formacaopessoal.SituacaoCurso.ToString();

            validacao.Valido = true;

            Techne.Data.QueryTable qtGraduacaoConcluida = RN.FormacaoPessoal.ConsultarGraduacaoConcluida(idPessoa , idContador  );
            Techne.Data.QueryTable qtGraduacaoAndamento = RN.FormacaoPessoal.ConsultarGraduacaoAndamento(idPessoa,idContador);
            Techne.Data.QueryTable qtPosGraduacao = RN.FormacaoPessoal.ConsultarPosGraduacao(idPessoa,idContador);
            Techne.Data.QueryTable qtEnsinoMedioConcluido = RN.FormacaoPessoal.ConsultarEnsinoMedioConcluido(idPessoa, idContador);
            Techne.Data.QueryTable qtGraduacaoConcluidaOUEnsinoMedio = RN.FormacaoPessoal.ConsultarGraduacaoConcluidaOUEnsinoMedio(idPessoa, idContador);

            if (!string.IsNullOrEmpty(cEscolaridade))
            {
                if (cEscolaridade.Trim().Substring(0, 8) == "Pós-Grad")
                {
                    if (qtGraduacaoConcluida.Rows.Count == 0)
                    {
                        validacao.Mensagem = @"Não se permite inclusão de Pós-Graduação sem uma graduação completada!";

                        validacao.Valido = false;
                    }
                }
                if (cEscolaridade.Trim().Substring(0, 8) == "Superior" && cSituacaoCurso.Trim().Substring(0, 9) != "Concluído")
                {
                    if (qtGraduacaoConcluidaOUEnsinoMedio.Rows.Count == 0)
                    {
                        validacao.Mensagem = @"Não se permite inclusão de Graduação incompleta sem uma graduação completada ou ensino médio!";
                        validacao.Valido = false;
                    }
                }
                if (cEscolaridade.Trim().Substring(0, 8).Contains("Superior"))
                {
                    if (qtEnsinoMedioConcluido.Rows.Count == 0)
                    {
                        validacao.Mensagem = @"Não se permite inclusão de Graduação sem o ensino médio completo!";
                        validacao.Valido = false;
                    }
                }
            }
            else
            {
                validacao.Mensagem = @"Escolaridade não preenchida.";

                validacao.Valido = false;
            }
            return validacao;
        }


        public static ValidacaoDados Validar(TceFormacaoPessoal formacaopessoal)
        {
            var validacao = new ValidacaoDados();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT 1 FROM   [dbo].[TCE_FORMACAO_PESSOAL] WHERE  
                                    PESSOA = @PESSOA AND
                                    ESCOLARIDADE = @ESCOLARIDADE  AND
                                    SITUACAO_CURSO = @SITUACAO_CURSO  AND
                                    ID_CURSO_FORMACAO_PESSOAL = @ID_CURSO_FORMACAO_PESSOAL  AND
                                    FORMACAO_COMPLEMENTACAO_PEDAGOGICA = @FORMACAO_COMPLEMENTACAO_PEDAGOGICA  AND
                                    ANO_INICIO = @ANO_INICIO  AND
                                    ANO_CONCLUSAO = @ANO_CONCLUSAO  AND
                                    ID_INSTITUICAO = @ID_INSTITUICAO  AND
                                    DOC_COMPROBATORIO = @DOC_COMPROBATORIO  AND
                                    ID_FORMACAO_PESSOAL <> @ID_FORMACAO_PESSOAL";

                contextQuery.Parameters.Add("@PESSOA", formacaopessoal.Pessoa);
                contextQuery.Parameters.Add("@ESCOLARIDADE", formacaopessoal.Escolaridade);
                contextQuery.Parameters.Add("@SITUACAO_CURSO", formacaopessoal.SituacaoCurso);
                contextQuery.Parameters.Add("@ID_CURSO_FORMACAO_PESSOAL", formacaopessoal.IdCursoFormacaoPessoal);
                contextQuery.Parameters.Add("@FORMACAO_COMPLEMENTACAO_PEDAGOGICA", formacaopessoal.FormacaoComplementacaoPedagogica);
                contextQuery.Parameters.Add("@ANO_INICIO", formacaopessoal.AnoInicio);
                contextQuery.Parameters.Add("@ANO_CONCLUSAO", formacaopessoal.AnoConclusao);
                contextQuery.Parameters.Add("@ID_INSTITUICAO", formacaopessoal.IdInstituicao);
                contextQuery.Parameters.Add("@DOC_COMPROBATORIO", formacaopessoal.Doc_comprobatorio);
                contextQuery.Parameters.Add("@ID_FORMACAO_PESSOAL", formacaopessoal.IdFormacaoPessoal);


                object obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    validacao.Valido = true;
                }
                else
                {
                    validacao.Valido = false;
                    validacao.Mensagem = "Já existe uma Formação Pessoal com estas informações.";
                }
            }
            return validacao;
        }


    }
}
