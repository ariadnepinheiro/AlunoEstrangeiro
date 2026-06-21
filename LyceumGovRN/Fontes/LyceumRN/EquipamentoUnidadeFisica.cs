using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class EquipamentoUnidadeFisica : RNBase
    {
        public static int RetornaQtdLaboratorio(string unidadeFisica)
        {
            var contextQuery = new ContextQuery(
                @"SELECT COUNT(*) 
                  FROM TCE_TIPO_DEPENDENCIA_UNIDADE_FISICA 
                  WHERE TIPO_DEPENDENCIA='LABORATIORIOINFO' and
	                    UNIDADE_FISICA=@UNIDADE_FISICA");

            contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);
            
            return ExecutarFuncao(contextQuery);
        }

        public static int RetornaQtdComputadorGeral(string unidadeFisica)
        {
            var contextQuery = new ContextQuery(
                @"SELECT COUNT(*)
                  FROM TCE_EQUIPAMENTO_UNIDADE_FISICA 
                  WHERE UNIDADE_FISICA=@UNIDADE_FISICA AND ID_EQUIPAMENTO in (7,12) AND ISNULL(QUANTIDADE,0) > 0");

            contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

            return ExecutarFuncao(contextQuery);
        }

        public List<DadosEquipamentoUnidadeFisica> ObtemListaPor(string unidadeFisica)
        {
            List<DadosEquipamentoUnidadeFisica> listaEquipamentos = new List<DadosEquipamentoUnidadeFisica>();
            DadosEquipamentoUnidadeFisica equipamento = new DadosEquipamentoUnidadeFisica();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                if (string.IsNullOrEmpty(unidadeFisica))
                {
                    return null;
                }

                contextQuery.Command = @" SELECT E.ID_EQUIPAMENTO, 
                                       E.DESCRICAO, 
                                       ISNULL(EUF.QUANTIDADE, 0) AS QUANTIDADE, 
                                       ISNULL(QUANTIDADEMAXIMASUGERIDA, 0) AS QUANTIDADEMAXIMASUGERIDA,  
                                       ID_EQUIPAMENTO_MAXIMOVINCULADO 
                                FROM   DBO.TCE_EQUIPAMENTO E 
                                       LEFT JOIN TCE_EQUIPAMENTO_UNIDADE_FISICA EUF 
                                              ON EUF.ID_EQUIPAMENTO = E.ID_EQUIPAMENTO 
                                                 AND EUF.UNIDADE_FISICA = @UNIDADE_FISICA 
                                ORDER  BY ( CASE 
                                              WHEN E.DESCRICAO = 'INTERNET BANDA LARGA' THEN( 'Z' ) 
                                              ELSE( E.DESCRICAO ) 
                                            END ) ";

                contextQuery.Parameters.Add("@UNIDADE_FISICA", unidadeFisica);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    equipamento = new DadosEquipamentoUnidadeFisica();

                    equipamento.IdEquipamento = Convert.ToInt32(reader["ID_EQUIPAMENTO"]);
                    equipamento.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    equipamento.Quantidade = Convert.ToInt32(reader["QUANTIDADE"]);
                    equipamento.QuantidadeMaximaSugerida = Convert.ToInt32(reader["QUANTIDADEMAXIMASUGERIDA"]);
                    if (reader["ID_EQUIPAMENTO_MAXIMOVINCULADO"] != DBNull.Value)
                    {
                        equipamento.IdEquipamentoMaximoVinculado = Convert.ToInt32(reader["ID_EQUIPAMENTO_MAXIMOVINCULADO"]);
                    }

                    listaEquipamentos.Add(equipamento);
                }

                return listaEquipamentos;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public static void Alterar(List<TceEquipamentoUnidadeFisica> tipos)
        {
            var validacaoDados = Validar(tipos);

            if (!validacaoDados.Valido)
            {
                throw new Exception(validacaoDados.Mensagem);
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE  TCE_EQUIPAMENTO_UNIDADE_FISICA
                                    WHERE   UNIDADE_FISICA = @UNIDADE_FISICA "
                    };
                    contextQuery.Parameters.Add("@UNIDADE_FISICA", tipos[0].UnidadeFisica);

                    ctx.ApplyModifications(contextQuery);

                    foreach (var tipo in tipos)
                    {
                        if (tipo.Quantidade > 0)
                        {
                            contextQuery = new ContextQuery
                            {
                                Command = @" INSERT  INTO dbo.TCE_EQUIPAMENTO_UNIDADE_FISICA ( UNIDADE_FISICA,
                                                        ID_EQUIPAMENTO, QUANTIDADE, MATRICULA)
                                                VALUES  ( @UNIDADE_FISICA, @ID_EQUIPAMENTO, @QUANTIDADE, @MATRICULA ) "
                            };
                            contextQuery.Parameters.Add("@UNIDADE_FISICA", tipo.UnidadeFisica);
                            contextQuery.Parameters.Add("@ID_EQUIPAMENTO", tipo.IdEquipamento);
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

        public static ValidacaoDados Validar(List<TceEquipamentoUnidadeFisica> tipos)
        {
            var nQtdComp07 = 0;
            var nQtdComp12 = 0;
            var unidadeFisica = string.Empty;
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tipos == null)
            {
                return validacaoDados;
            }

            if (tipos.Count == 0)
            {
                mensagens.Add("Erro ao montar equipamentos não encontrados, favor tentar novamente!");
            }
            else
            {
                unidadeFisica = tipos[0].UnidadeFisica;
            }           

            foreach (var tipo in tipos)
            {
                if (tipo.IdEquipamento == 7) { nQtdComp07 = tipo.Quantidade; }
                if (tipo.IdEquipamento == 12) { nQtdComp12 = tipo.Quantidade; }

                if (string.IsNullOrEmpty(tipo.UnidadeFisica))
                {
                    mensagens.Add("A UNIDADE FÍSICA é obrigatória em todas as linhas da lista!");
                }

                if (unidadeFisica != tipo.UnidadeFisica)
                {
                    mensagens.Add("Todas as UNIDADES FÍSICAS da lista devem ser iguais!");
                }

                if (tipo.IdEquipamento <= 0)
                {
                    mensagens.Add("O EQUIPAMENTO  é obrigatório em todas as linhas da lista!");
                    break;
                }

                if (string.IsNullOrEmpty(tipo.Matricula) || tipo.Matricula.Length > 20)
                {
                    mensagens.Add("A MATRÍCULA DO RESPONSÁVEL é obrigatória E deve ter 20 digitos em todas as linhas da lista!");
                    break;
                }                                

                if (tipo.IdEquipamento == 7 && tipo.Quantidade > 0)
                {
                    int qtd = RetornaQtdLaboratorio(tipo.UnidadeFisica);
                    
                    if (qtd == 0) 
                    {
                        mensagens.Add("Está unidade de ensino não possui laboratório de informática, portanto não é permitido Computador para uso dos alunos.");
                    }

                }

                if (tipo.IdEquipamento == 13 && tipo.Quantidade > 0)
                {
                    int qtd = RetornaQtdComputadorGeral(tipo.UnidadeFisica);

                    //07 - Comp. Aluno
                    //12 - Comp. Administrativo

                    if ( (nQtdComp07 + nQtdComp12) == 0 )
                    {
                        mensagens.Add("Está unidade de ensino não possui computadores cadastrados, com isso a opção Internet Banda Larga não é permitida.");
                    }

                }

                if (tipo.IdEquipamento == 13 && tipo.Quantidade > 0)
                {
                    int qtd = RetornaQtdLaboratorio(tipo.UnidadeFisica);

                    if (qtd == 0)
                    {
                        mensagens.Add("Está unidade não possuí computadores no laboratório de informática, com isso a opção Internet Banda Larga não é permitida.");
                    }
                }

                if (tipo.Quantidade > 1000)
                {
                    mensagens.Add("A quantidade de equipamento não pode ser maior que 1000.");
                    break;
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
                                FROM  TCE_EQUIPAMENTO_UNIDADE_FISICA 
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