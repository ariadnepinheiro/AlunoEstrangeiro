using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class TipoDependenciaUnidadeFisica : RNBase
    {
        public static DataTable Listar(string unidadeFisica)
        {
            if (string.IsNullOrEmpty(unidadeFisica))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  DISTINCT d.TIPO_DEPEND, d.NOME, ISNULL(tuf.QUANTIDADE, 0) AS QUANTIDADE
                                     FROM    dbo.LY_TIPO_DEPENDENCIA d
                                LEFT JOIN TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA tuf ON tuf.TIPO_DEPENDENCIA = d.TIPO_DEPEND
                                AND tuf.UNIDADE_FISICA = @UNIDADE_FISICA
                                LEFT JOIN LYCURSO_LYTIPODEPENDENCIA DP ON  DP.LYTIPODEPENDENCIAID = D.TIPO_DEPEND 
                                WHERE d.TIPO_DEPEND <> 'SALA'
                                AND d.TIPO_DEPEND <> 'SALAAEE'
                                 AND  DP.LYTIPODEPENDENCIAID IS NULL   
                                ORDER BY d.NOME "
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Alterar(List<TceTipoDependenciaUnidadeFisica> tipos, int salaClimatizada, int salaAcessibilidade, int salaCantinhoLeitura)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE  TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA
                                     WHERE   UNIDADE_FISICA = @UNIDADE_FISICA "
                    };
                    contextQuery.Parameters.Add("@UNIDADE_FISICA", tipos[0].UnidadeFisica);

                    ctx.ApplyModifications(contextQuery);

                    contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE LY_UNIDADE_FISICA SET
                                         SALACLIMATIZADA = @SALACLIMATIZADA,
                                         SALAACESSIBILIDADE = @SALAACESSIBILIDADE,
                                         SALACANTINHOLEITURA = @SALACANTINHOLEITURA,
                                         MATRICULA = @MATRICULA,
                                         STAMP_ATUALIZACAO = GETDATE()
                                    WHERE   UNIDADE_FIS = @UNIDADE_FISICA
                                     "
                    };
                    contextQuery.Parameters.Add("@UNIDADE_FISICA", tipos[0].UnidadeFisica);
                    contextQuery.Parameters.Add("@SALACLIMATIZADA", salaClimatizada);
                    contextQuery.Parameters.Add("@SALAACESSIBILIDADE", salaAcessibilidade);
                    contextQuery.Parameters.Add("@SALACANTINHOLEITURA", salaCantinhoLeitura);
                    contextQuery.Parameters.Add("@MATRICULA", tipos[0].Matricula);

                    ctx.ApplyModifications(contextQuery);


                    foreach (var tipo in tipos)
                    {
                        if (tipo.Quantidade > 0)
                        {
                            contextQuery = new ContextQuery
                            {
                                Command = @" INSERT  INTO dbo.TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA ( UNIDADE_FISICA,
                                                    TIPO_DEPENDENCIA, QUANTIDADE, MATRICULA )
                                             VALUES  ( @UNIDADE_FISICA, @TIPO_DEPENDENCIA, @QUANTIDADE, @MATRICULA ) "
                            };
                            contextQuery.Parameters.Add("@UNIDADE_FISICA", tipo.UnidadeFisica);
                            contextQuery.Parameters.Add("@TIPO_DEPENDENCIA", tipo.TipoDependencia);
                            contextQuery.Parameters.Add("@QUANTIDADE", tipo.Quantidade);
                            contextQuery.Parameters.Add("@MATRICULA", tipo.Matricula);

                            ctx.ApplyModifications(contextQuery);
                        }
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(List<TceTipoDependenciaUnidadeFisica> tipos, int salaClimatizada, int salaAcessiblidade, int salaCantinhoLeitura)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipos == null)
            {
                return validacaoDados;
            }

            var unidadeFisica = tipos[0].UnidadeFisica;

            foreach (var tipo in tipos)
            {
                if (string.IsNullOrEmpty(tipo.UnidadeFisica))
                {
                    mensagens.Add("A UNIDADE FÍSICA é obrigatória em todas as linhas da lista!");
                }

                if (unidadeFisica != tipo.UnidadeFisica)
                {
                    mensagens.Add("Todas as UNIDADES FÍSICAS da lista devem ser iguais!");
                }

                if (string.IsNullOrEmpty(tipo.TipoDependencia))
                {
                    mensagens.Add("O TIPO DEPENDÊNCIA  é obrigatório em todas as linhas da lista!");
                    break;
                }

                if (string.IsNullOrEmpty(tipo.Matricula) || tipo.Matricula.Length > 20)
                {
                    mensagens.Add("A MATRÍCULA DO RESPONSÁVEL é obrigatória E deve ter 20 digitos em todas as linhas da lista!");
                    break;
                }

                if (tipo.Quantidade > 100)
                {
                    mensagens.Add("A quantidade da dependência não pode ser maior que 100.");
                    break;
                }
            }

            if (salaClimatizada < 0)
            {
                mensagens.Add("O campo SALAS DE AULA CLIMATIZADAS é de preenchimento obrigatório.");
            }

            if (salaAcessiblidade < 0)
            {
                mensagens.Add("O campo SALAS DE AULA COM ACESSIBILIDADE PARA PESSOAS COM DEFICIÊNCIA OU MOBILIDADE REDUZIDA é de preenchimento obrigatório.");
            }

            if (salaCantinhoLeitura < 0)
            {
                mensagens.Add("O campo SALAS DE AULA COM CANTINHO DE LEITURA é de preenchimento obrigatório.");
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

        public static string VerificaDataAlteracao(string unidadeFisica)
        {
            if (string.IsNullOrEmpty(unidadeFisica))
            {
                return null;
            }
            var data = string.Empty;
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT MAX(DT_ALTERACAO) AS DATA_ALTERACAO
                                FROM  TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA 
                               WHERE UNIDADE_FISICA = @UNIDADE_FISICA"
                };
                contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

                var dt = ctx.GetDataTable(contextQuery);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DATA_ALTERACAO"] != DBNull.Value)
                    {
                        data = dt.Rows[0]["DATA_ALTERACAO"].ToString();
                    }
                }
                return data;

            }
        }
    }
}