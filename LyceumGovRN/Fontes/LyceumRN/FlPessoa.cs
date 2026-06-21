namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using Entidades;
    using Seeduc.Infra.Data;
    using System.Data;
    using System.Data.SqlClient;

    public class FlPessoa : RNBase
    {
        public static LyFlPessoa Carregar(decimal idPessoa)
        {
            try
            {
                var flpessoa = new LyFlPessoa();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT * FROM LY_FL_PESSOA
                            WHERE PESSOA = @ID "
                    };
                    contextQuery.Parameters.Add("@ID", idPessoa);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        if (reader.Read())
                        {
                            flpessoa.Pessoa = idPessoa;
                            flpessoa.FlField01 = Convert.ToString(reader["FL_FIELD_01"]);
                            flpessoa.FlField02 = Convert.ToString(reader["FL_FIELD_02"]);
                            flpessoa.FlField03 = Convert.ToString(reader["FL_FIELD_03"]);
                            flpessoa.FlField04 = Convert.ToString(reader["FL_FIELD_04"]);
                            flpessoa.FlField05 = Convert.ToString(reader["FL_FIELD_05"]);
                            flpessoa.FlField07 = Convert.ToString(reader["FL_FIELD_07"]);
                            flpessoa.FlField08 = Convert.ToString(reader["FL_FIELD_08"]);
                            flpessoa.FlField09 = Convert.ToString(reader["FL_FIELD_09"]);
                            flpessoa.FlField10 = Convert.ToString(reader["FL_FIELD_10"]);
                            flpessoa.FlField11 = Convert.ToString(reader["FL_FIELD_11"]);
                            flpessoa.FlField12 = Convert.ToString(reader["FL_FIELD_12"]);
                            flpessoa.FlField20 = Convert.ToString(reader["FL_FIELD_20"]);
                            flpessoa.FlField21 = Convert.ToString(reader["FL_FIELD_21"]);
                            flpessoa.FlField22 = Convert.ToString(reader["FL_FIELD_22"]);
                            flpessoa.FlField23 = Convert.ToString(reader["FL_FIELD_23"]);
                        }
                    }
                    return flpessoa;
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static void Inserir(LyFlPessoa lyFlPessoa, DataContext context)
        {
            var contextQuery = new ContextQuery
           {
               Command = @"INSERT INTO dbo.LY_FL_PESSOA
                                                (
                                                   PESSOA,
                                                    FL_FIELD_01,
                                                    FL_FIELD_02,
                                                    FL_FIELD_03,                                                                                                       
                                                    FL_FIELD_07,
                                                    FL_FIELD_08,
                                                    FL_FIELD_09,
                                                    FL_FIELD_21,
                                                    FL_FIELD_22,
                                                    FL_FIELD_23
                                                )
                                        VALUES  (
                                                   @PESSOA,
                                                    @FL_FIELD_01,
                                                    @FL_FIELD_02,
                                                    @FL_FIELD_03,                                                                                                    
                                                    @FL_FIELD_07,
                                                    @FL_FIELD_08,
                                                    @FL_FIELD_09,
                                                    @FL_FIELD_21,
                                                    @FL_FIELD_22,
                                                    @FL_FIELD_23
                                                )"
           };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, lyFlPessoa.Pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", lyFlPessoa.FlField01);
            contextQuery.Parameters.Add("@FL_FIELD_02", lyFlPessoa.FlField02);
            contextQuery.Parameters.Add("@FL_FIELD_03", lyFlPessoa.FlField03);          
            contextQuery.Parameters.Add("@FL_FIELD_07", lyFlPessoa.FlField07);
            contextQuery.Parameters.Add("@FL_FIELD_08", lyFlPessoa.FlField08);
            contextQuery.Parameters.Add("@FL_FIELD_09", lyFlPessoa.FlField09);
            contextQuery.Parameters.Add("@FL_FIELD_21", lyFlPessoa.FlField21);
            contextQuery.Parameters.Add("@FL_FIELD_22", lyFlPessoa.FlField22);
            contextQuery.Parameters.Add("@FL_FIELD_23", lyFlPessoa.FlField23);

            context.ApplyModifications(contextQuery);
        }

        public static void InserirTransporte(LyFlPessoa lyFlPessoa, DataContext context)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"INSERT INTO dbo.LY_FL_PESSOA
                                                (
                                                   PESSOA,                                                    
                                                    FL_FIELD_04,
                                                    FL_FIELD_05,
                                                    FL_FIELD_10,
                                                    FL_FIELD_11,
                                                    FL_FIELD_12,
                                                    FL_FIELD_20
                                                )
                                        VALUES  (
                                                   @PESSOA,                                                    
                                                    @FL_FIELD_04,
                                                    @FL_FIELD_05,                                                   
                                                    @FL_FIELD_10,
                                                    @FL_FIELD_11,
                                                    @FL_FIELD_12,
                                                    @FL_FIELD_20
                                                )"
            };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, lyFlPessoa.Pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_04", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField04);
            contextQuery.Parameters.Add("@FL_FIELD_05", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField05);
            contextQuery.Parameters.Add("@FL_FIELD_10", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField10);
            contextQuery.Parameters.Add("@FL_FIELD_11", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField11);
            contextQuery.Parameters.Add("@FL_FIELD_12", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField12);
            contextQuery.Parameters.Add("@FL_FIELD_20", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField20);

            context.ApplyModifications(contextQuery);
        }

        public void InsereFlPessoaPreCadastro(DataContext contexto, int preCadastroAlunoId, LyFlPessoa flPessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_FL_PESSOA 
                                                (PESSOA, 
                                                 FL_FIELD_02, 
                                                 FL_FIELD_09,
                                                 FL_FIELD_04,
                                                 FL_FIELD_05,
                                                 FL_FIELD_10,
                                                 FL_FIELD_11,
                                                 FL_FIELD_12,
                                                 FL_FIELD_20,
                                                 FL_FIELD_23
                                                ) 
                                    SELECT PESSOAID, 
                                           TIPOCERTIDAO, 
                                            MODELOCERTIDAO,
                                            @FL_FIELD_04,
                                            @FL_FIELD_05,
                                            @FL_FIELD_10,
                                            @FL_FIELD_11,
                                            @FL_FIELD_12,
                                            @FL_FIELD_20,
                                            @FL_FIELD_23
                                    FROM   MATRICULA.PRECADASTROALUNO 
                                    WHERE  PRECADASTROALUNOID = @PRECADASTROALUNOID  ";

            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);
            contextQuery.Parameters.Add("@FL_FIELD_04", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField04);
            contextQuery.Parameters.Add("@FL_FIELD_05", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField05);
            contextQuery.Parameters.Add("@FL_FIELD_10", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField10);
            contextQuery.Parameters.Add("@FL_FIELD_11", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField11);
            contextQuery.Parameters.Add("@FL_FIELD_12", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField12);
            contextQuery.Parameters.Add("@FL_FIELD_20", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField20);
            contextQuery.Parameters.Add("@FL_FIELD_23", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField23);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaFlPessoaPreCadastro(DataContext contexto, int preCadastroAlunoId, LyFlPessoa flPessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FL 
                                        SET    FL_FIELD_02 = PC.TIPOCERTIDAO , 
                                               FL_FIELD_09 = PC.MODELOCERTIDAO,
                                               FL_FIELD_04 = @FL_FIELD_04,
                                               FL_FIELD_05 = @FL_FIELD_05,
                                               FL_FIELD_10 = @FL_FIELD_10,
                                               FL_FIELD_11 = @FL_FIELD_11,
                                               FL_FIELD_12 = @FL_FIELD_12,
                                               FL_FIELD_20 = @FL_FIELD_20,
                                               FL_FIELD_23 = @FL_FIELD_23
                                        FROM   MATRICULA.PRECADASTROALUNO PC 
                                               INNER JOIN DBO.LY_FL_PESSOA FL 
                                                       ON PC.PESSOAID = FL.PESSOA 
                                        WHERE  PRECADASTROALUNOID = @PRECADASTROALUNOID  ";

            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);
            contextQuery.Parameters.Add("@FL_FIELD_04", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField04);
            contextQuery.Parameters.Add("@FL_FIELD_05", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField05);
            contextQuery.Parameters.Add("@FL_FIELD_10", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField10);
            contextQuery.Parameters.Add("@FL_FIELD_11", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField11);
            contextQuery.Parameters.Add("@FL_FIELD_12", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField12);
            contextQuery.Parameters.Add("@FL_FIELD_20", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField20);
            contextQuery.Parameters.Add("@FL_FIELD_23", TechneDbType.T_ALFAEXTRALARGE, flPessoa.FlField23);

            contexto.ApplyModifications(contextQuery);
        }

        public static void AlterarTransporte(LyFlPessoa lyFlPessoa, DataContext context)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"UPDATE  LY_FL_PESSOA
                                        SET    
                                            FL_FIELD_04 = @FL_FIELD_04,
                                            FL_FIELD_05 = @FL_FIELD_05,
                                            FL_FIELD_10 = @FL_FIELD_10,
                                            FL_FIELD_11 = @FL_FIELD_11,
                                            FL_FIELD_12 = @FL_FIELD_12,
                                            FL_FIELD_20 = @FL_FIELD_20
                                        WHERE   PESSOA = @PESSOA"
            };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, lyFlPessoa.Pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_04", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField04);
            contextQuery.Parameters.Add("@FL_FIELD_05", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField05);
            contextQuery.Parameters.Add("@FL_FIELD_10", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField10);
            contextQuery.Parameters.Add("@FL_FIELD_11", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField11);
            contextQuery.Parameters.Add("@FL_FIELD_12", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField12);
            contextQuery.Parameters.Add("@FL_FIELD_20", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField20);

            context.ApplyModifications(contextQuery);
        }

        public static void Alterar(LyFlPessoa lyFlPessoa, DataContext context)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"UPDATE  LY_FL_PESSOA
                                        SET    
                                            FL_FIELD_01= @FL_FIELD_01,
                                            FL_FIELD_02= @FL_FIELD_02,
                                            FL_FIELD_03= @FL_FIELD_03,                                          
                                            FL_FIELD_07= @FL_FIELD_07,
                                            FL_FIELD_08= @FL_FIELD_08,
                                            FL_FIELD_09= @FL_FIELD_09,
                                            FL_FIELD_21= @FL_FIELD_21,
                                            FL_FIELD_22= @FL_FIELD_22,
                                            FL_FIELD_23= @FL_FIELD_23
                                        WHERE   PESSOA = @PESSOA"
            };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, lyFlPessoa.Pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField01);
            contextQuery.Parameters.Add("@FL_FIELD_02", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField02);
            contextQuery.Parameters.Add("@FL_FIELD_03", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField03);           
            contextQuery.Parameters.Add("@FL_FIELD_07", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField07);
            contextQuery.Parameters.Add("@FL_FIELD_08", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField08);
            contextQuery.Parameters.Add("@FL_FIELD_09", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField09);
            contextQuery.Parameters.Add("@FL_FIELD_21", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField21);
            contextQuery.Parameters.Add("@FL_FIELD_22", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField22);
            contextQuery.Parameters.Add("@FL_FIELD_23", TechneDbType.T_ALFAEXTRALARGE, lyFlPessoa.FlField23);

            context.ApplyModifications(contextQuery);
        }

        public void AlteraDadosCadastrais(DataContext context, decimal pessoa, string flField01, string flField02, string flField07, string flField09)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"UPDATE  LY_FL_PESSOA
                                        SET    
                                            FL_FIELD_01= @FL_FIELD_01,
                                            FL_FIELD_02= @FL_FIELD_02,                                       
                                            FL_FIELD_07= @FL_FIELD_07,
                                            FL_FIELD_09= @FL_FIELD_09
                                        WHERE   PESSOA = @PESSOA"
            };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, flField01);
            contextQuery.Parameters.Add("@FL_FIELD_02", TechneDbType.T_ALFAEXTRALARGE, flField02);
            contextQuery.Parameters.Add("@FL_FIELD_07", TechneDbType.T_ALFAEXTRALARGE, flField07);
            contextQuery.Parameters.Add("@FL_FIELD_09", TechneDbType.T_ALFAEXTRALARGE, flField09);

            context.ApplyModifications(contextQuery);
        }

        public void InsereDadosCadastrais(DataContext context, decimal pessoa, string flField01, string flField02, string flField07, string flField09)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO dbo.LY_FL_PESSOA
                                                (
                                                   PESSOA,
                                                    FL_FIELD_01,
                                                    FL_FIELD_02,                                                                                                   
                                                    FL_FIELD_07,
                                                    FL_FIELD_09
                                                )
                                        VALUES  (
                                                   @PESSOA,
                                                    @FL_FIELD_01,
                                                    @FL_FIELD_02,                                                                                                  
                                                    @FL_FIELD_07,
                                                    @FL_FIELD_09,
                                                )"
            };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, flField01);
            contextQuery.Parameters.Add("@FL_FIELD_02", TechneDbType.T_ALFAEXTRALARGE, flField02);
            contextQuery.Parameters.Add("@FL_FIELD_07", TechneDbType.T_ALFAEXTRALARGE, flField07);
            contextQuery.Parameters.Add("@FL_FIELD_09", TechneDbType.T_ALFAEXTRALARGE, flField09);

            context.ApplyModifications(contextQuery);
        }

        public static bool VerificarFLPessoa(decimal pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT    1
                              FROM      LY_FL_PESSOA
                              WHERE     PESSOA = @PESSOA"
                };
                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ExistePor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM      LY_FL_PESSOA
                                        WHERE     PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static int Remover(decimal idPessoa, DataContext ctx)
        {
            try
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"DELETE  FROM dbo.LY_FL_PESSOA
                            WHERE   PESSOA = @PESSOA "
                };
                contextQuery.Parameters.Add("@Pessoa", TechneDbType.T_NUMERO, idPessoa);

                return ctx.ApplyModifications(contextQuery);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RetiraDadosTransportePublicoPor(string aluno, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery =
                new ContextQuery(@"UPDATE  FP
                                   SET     FL_FIELD_04 = 'N' ,
                                           FL_FIELD_05 = NULL
                                   FROM    dbo.LY_ALUNO A
                                           INNER JOIN dbo.LY_FL_PESSOA FP ON A.PESSOA = FP.PESSOA
                                   WHERE   ALUNO = @ALUNO
                                           AND FL_FIELD_04 = 'S'",
                new ContextQueryParameter("@ALUNO", aluno));

            listaContextQuery.Add(contextQuery);
        }

        public string ObtemZonaResidencialPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT FL_FIELD_01 
                                        FROM LY_FL_PESSOA (NOLOCK)
                                        WHERE   PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public string ObtemPovoIndigenaPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT FL_FIELD_21 
                                        FROM LY_FL_PESSOA (NOLOCK)
                                        WHERE   PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public string ObtemModaisPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT FL_FIELD_05 
                                        FROM LY_FL_PESSOA (NOLOCK)
                                        WHERE   PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado == null ? string.Empty : resultado;
        }

        public string ObtemTransporteRodoviarioPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT FL_FIELD_11 
                                        FROM LY_FL_PESSOA (NOLOCK)
                                        WHERE   PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemRecebeEscolarizacaoOutroEspacoPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT FL_FIELD_03
                                        FROM LY_FL_PESSOA F (NOLOCK)
											INNER JOIN LY_ALUNO A (NOLOCK) ON F.PESSOA = A.PESSOA
                                        WHERE   A.ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemTransporteAquaviarioPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT FL_FIELD_12
                                        FROM LY_FL_PESSOA (NOLOCK)
                                        WHERE   PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemTransporteOnibusPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT FL_FIELD_20 
                                        FROM LY_FL_PESSOA (NOLOCK)
                                        WHERE   PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void InsereZonaResidencial(DataContext ctx, decimal pessoa, string zonaResidencial)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LY_FL_PESSOA 
                                                (PESSOA, 
                                                 FL_FIELD_01) 
                                    VALUES      ( @PESSOA, 
                                                  @FL_FIELD_01 )  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, zonaResidencial);

            ctx.ApplyModifications(contextQuery);
        }

        public void InsereZonaResidencialPovoIndigena(DataContext ctx, decimal pessoa, string zonaResidencial, string povoIndigena)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LY_FL_PESSOA 
                                                (PESSOA, 
                                                 FL_FIELD_01,
                                                 FL_FIELD_21) 
                                    VALUES      ( @PESSOA, 
                                                  @FL_FIELD_01,
                                                  @FL_FIELD_21 )  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, zonaResidencial);
            contextQuery.Parameters.Add("@FL_FIELD_21", TechneDbType.T_ALFAEXTRALARGE, povoIndigena);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaZonaResidencial(DataContext ctx, decimal pessoa, string zonaResidencial)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_FL_PESSOA
                                        SET FL_FIELD_01 = @FL_FIELD_01
                                        WHERE   PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, zonaResidencial);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaZonaResidencialPovoIndigena(DataContext ctx, decimal pessoa, string zonaResidencial, string povoIndigenaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_FL_PESSOA
                                        SET FL_FIELD_01 = @FL_FIELD_01,
                                            FL_FIELD_21 = @FL_FIELD_21
                                        WHERE   PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@FL_FIELD_01", TechneDbType.T_ALFAEXTRALARGE, zonaResidencial);
            contextQuery.Parameters.Add("@FL_FIELD_21", TechneDbType.T_ALFAEXTRALARGE, povoIndigenaId);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaNumeroDependentes(DataContext contexto, int pessoa, int numeroDependentes)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_FL_PESSOA
	                                    SET FL_FIELD_13 = @NUMERODEPENDENTES
                                    WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@NUMERODEPENDENTES", numeroDependentes);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
