using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class VinculoTce : RNBase
    {
        /// <summary>
        /// Listar vinculo
        /// </summary>
        /// <returns>querytable com códigos de municípios, descrição e siglas</returns>
        public static DataTable Listar(string matricula)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"select distinct ue.UNIDADE_ENS + ' - ' + ue.NOME_COMP AS NOME_COMP, ue.UNIDADE_ENS, ID_VINCULO, DT_INICIO, DT_FIM, PRINCIPAL 
                            from tce_vinculo v
                                inner join LY_Docente d on d.MATRICULA = v.matricula
                                inner join LY_UNIDADE_ENSINO ue  on ue.UNIDADE_ENS = v.UNIDADE_ENS
                                 where v.matricula = @MATRICULA"
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados ValidarInsercao(TceVinculo vinculo)
        {
            return Validar(vinculo, true);
        }

        public static ValidacaoDados ValidarUpdate(TceVinculo vinculo)
        {
            return Validar(vinculo, false);
        }

        /// <summary>
        /// Validar Informações de vinculo
        /// </summary>
        /// <returns>querytable com códigos de municípios, descrição e siglas</returns>
        private static ValidacaoDados Validar(TceVinculo vinculo, bool insercao)
        {
            var mensagens = new List<string>();
            VinculoTce rnVinculo = new VinculoTce();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(vinculo.UnidadeEnsino))
            {
                mensagens.Add("O campo Unidade de Ensino é obrigatório não foi preenchido e/ou é inválido!");
            }

            if (string.IsNullOrEmpty(vinculo.Matricula))
            {
                mensagens.Add("O campo Matricula do voluntário não foi encontrado!");
            }

            if (!vinculo.Principal)
            {
                //Verifica se não existe outro vinculo principal cadastrado
                if (!rnVinculo.VerificaVinculoPrincipal(vinculo.Matricula))
                {
                    mensagens.Add("Esta matricula ainda não possui vinculo principal, favor adicionar primeiramente o vinculo principal!");
                }
            }

            if (vinculo.DtFim == default(DateTime) || vinculo.DtInicio == default(DateTime))
            {
                mensagens.Add("Os campos de data são obrigatórios");
            }

            if (vinculo.DtFim <= vinculo.DtInicio)
            {
                mensagens.Add("O campo de data final precisa ser posterior ao de data inicial");
            }

            if (mensagens.Count == 0)
            {
                if (vinculo.Principal)
                {
                    //Verifica se existe outro vinculo principal ativo
                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    {
                        var sql = new StringBuilder();
                        sql.Append(@" SELECT COUNT(*)
                                FROM    TCE_VINCULO V
                                WHERE   V.MATRICULA = @MATRICULA
                                        AND V.PRINCIPAL = 1
                                        AND ( ( @DATAINICIO <= V.DT_FIM
                                                AND @DATAINICIO >= V.DT_INICIO
                                              )
                                              OR ( @DATAINICIO <= V.DT_INICIO
                                                   AND @DATAFIM >= V.DT_INICIO
                                                 )
                                            )
                                         ");
                        if (!insercao)
                        {
                            sql.Append(" AND ID_VINCULO <> @ID_VINCULO ");
                        }

                        var contextQuery = new ContextQuery(sql.ToString());

                        contextQuery.Parameters.Add("@MATRICULA", vinculo.Matricula);
                        contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, vinculo.DtInicio);
                        contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, vinculo.DtFim);
                        contextQuery.Parameters.Add("@ID_VINCULO", vinculo.IdVinculo);

                        var obj = ctx.GetReturnValue(contextQuery);

                        if (ctx.GetReturnValue<int>(contextQuery) > 0)
                        {
                            mensagens.Add("Já existe Unidade de Ensino marcada como principal e ativa.");
                        }
                    }
                }

                //Verifica se existe vinculo ativo para a msm escola
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var sql = new StringBuilder();
                    sql.Append(@" SELECT COUNT(*)
                                FROM    TCE_VINCULO V
                                WHERE   V.MATRICULA = @MATRICULA
                                        AND V.UNIDADE_ENS = @UNIDADE_ENS
                                        AND ( ( @DATAINICIO <= V.DT_FIM
                                                AND @DATAINICIO >= V.DT_INICIO
                                              )
                                              OR ( @DATAINICIO <= V.DT_INICIO
                                                   AND @DATAFIM >= V.DT_INICIO
                                                 )
                                            )
                                         ");
                    if (!insercao)
                    {
                        sql.Append(" AND ID_VINCULO <> @ID_VINCULO ");
                    }

                    var contextQuery = new ContextQuery(sql.ToString());

                    contextQuery.Parameters.Add("@UNIDADE_ENS", vinculo.UnidadeEnsino);
                    contextQuery.Parameters.Add("@MATRICULA", vinculo.Matricula);
                    contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, vinculo.DtInicio);
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, vinculo.DtFim);
                    contextQuery.Parameters.Add("@ID_VINCULO", vinculo.IdVinculo);

                    if (ctx.GetReturnValue<int>(contextQuery) > 0)
                    {
                        mensagens.Add("Já existe esta Unidade de Ensino ativa.");
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

        public static void Inserir(TceVinculo vinculo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT  INTO tce_vinculo ( DT_INICIO, DT_FIM, PRINCIPAL, UNIDADE_ENS, MATRICULA )
                                        VALUES  ( @DT_INICIO, @DT_FIM, @PRINCIPAL, @UNIDADE_ENS, @MATRICULA ) "
                    };

                    contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.Date, vinculo.DtInicio);
                    contextQuery.Parameters.Add("@DT_FIM", SqlDbType.Date, vinculo.DtFim);
                    contextQuery.Parameters.Add("@PRINCIPAL", vinculo.Principal);
                    contextQuery.Parameters.Add("@UNIDADE_ENS", vinculo.UnidadeEnsino);
                    contextQuery.Parameters.Add("@MATRICULA", vinculo.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TceVinculo vinculo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE  tce_vinculo
                                    SET    
		                                    DT_INICIO = @DT_INICIO,
                                            DT_FIM = @DT_FIM,
                                            PRINCIPAL = @PRINCIPAL	,
                                            DT_ALTERACAO = GETDATE()                                   
                                    WHERE   ID_VINCULO = @ID_VINCULO "
                    };

                    contextQuery.Parameters.Add("@ID_VINCULO", vinculo.IdVinculo);
                    contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.Date, vinculo.DtInicio);
                    contextQuery.Parameters.Add("@PRINCIPAL", vinculo.Principal);
                    contextQuery.Parameters.Add("@DT_FIM", SqlDbType.Date, vinculo.DtFim);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void InserirLotacao(TceVinculo vinculo, decimal pessoa, string user)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO LY_LOTACAO ( PESSOA, MATRICULA, ORDEM, 
							FUNCAO, DATA_DESATIVACAO, UNIDADE_FIS,
								DATA_NOMEACAO, UNIDADE_ENS, READAPTADO, SETOR, USUARIO, DATA_ATUALIZACAO ) VALUES 
                                        (@PESSOA, @MATRICULA, (select isnull(max(ordem),0) + 1 as ORDEM from LY_LOTACAO WHERE MATRICULA = @MATRICULA),
                                            @FUNCAO, @DATA_DESATIVACAO, (SELECT UNIDADE_FIS FROM LY_UNIDADES_ASSOCIADAS WHERE UNIDADE_ENS = @UNIDADE_ENS),
                                                cast(GETDATE() as Date), @UNIDADE_ENS, 'N', (select top(1) SETOR from ly_unidade_ensino WHERE UNIDADE_ENS = @UNIDADE_ENS), 
                                                    @USUARIO, GETDATE())"
                    };

                    contextQuery.Parameters.Add("@PESSOA", pessoa);
                    contextQuery.Parameters.Add("@MATRICULA", vinculo.Matricula);
                    contextQuery.Parameters.Add("@FUNCAO", "10127");//REGENTE MAIS EDUCACAO
                    contextQuery.Parameters.Add("@DATA_DESATIVACAO", SqlDbType.Date, vinculo.DtFim);
                    contextQuery.Parameters.Add("@UNIDADE_ENS", vinculo.UnidadeEnsino);
                    contextQuery.Parameters.Add("@USUARIO", user);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void InserirOuAtualizarLotacao(TceVinculo vinculo, decimal pessoa, string user)
        {
            var principal = ConsultarCampo(
                new ContextQuery(
                    @"select principal from tce_vinculo where ID_VINCULO = @VINCULO",
                    new ContextQueryParameter("@VINCULO", vinculo.IdVinculo)));

            if (Convert.ToBoolean(principal))
                AtualizarLotacao(vinculo, pessoa);
            else
                InserirLotacao(vinculo, pessoa, user);
        }

        public static void AtualizarLotacao(TceVinculo vinculo, decimal pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE LY_LOTACAO set DATA_DESATIVACAO = @DATA_DESATIVACAO, DATA_ATUALIZACAO = GETDATE() 
                                    where PESSOA = @PESSOA and MATRICULA = @MATRICULA 
                                    and ORDEM = (select max(ordem) from ly_lotacao l where l.PESSOA = @PESSOA and l.MATRICULA = @MATRICULA )"
                    };

                    contextQuery.Parameters.Add("@PESSOA", pessoa);
                    contextQuery.Parameters.Add("@MATRICULA", vinculo.Matricula);
                    contextQuery.Parameters.Add("@DATA_DESATIVACAO", SqlDbType.Date, vinculo.DtFim);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public bool VerificaVinculoPrincipal(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool existe = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                    FROM    TCE_VINCULO
                    WHERE   PRINCIPAL = 1
                            AND MATRICULA = @MATRICULA "
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
    }
}
