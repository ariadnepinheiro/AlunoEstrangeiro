using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Collections;

namespace Techne.Lyceum.RN
{
    public class FeriadoNacionalEstadual : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT ID_FERIADO_NAC_EST, DATA, DESCRICAO, 
                        TIPO_EVENTO, MATRICULA, DT_CADASTRO, DT_ALTERACAO
                        FROM dbo.TCE_FERIADO_NAC_EST
                        WHERE YEAR(DATA) = YEAR(GETDATE())
                        order by DATA "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Inserir(TceFeriadoNacionalEstadual feriadoNacionalEstadual)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  INSERT INTO TCE_FERIADO_NAC_EST
                            (DATA, DESCRICAO, TIPO_EVENTO, MATRICULA)
                            VALUES
                            (@DATA, @DESCRICAO, @TIPO_EVENTO, @MATRICULA)"
                    };

                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoNacionalEstadual.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoNacionalEstadual.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoNacionalEstadual.TipoEvento);
                    contextQuery.Parameters.Add("@MATRICULA", feriadoNacionalEstadual.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void Alterar(TceFeriadoNacionalEstadual feriadoNacionalEstadual)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE TCE_FERIADO_NAC_EST 
                          SET DATA = @DATA,
                          DESCRICAO = @DESCRICAO,
                          TIPO_EVENTO = @TIPO_EVENTO,
                          MATRICULA = @MATRICULA,
                          DT_ALTERACAO = GETDATE()
                          WHERE ID_FERIADO_NAC_EST = @ID "
                    };
                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoNacionalEstadual.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoNacionalEstadual.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoNacionalEstadual.TipoEvento);
                    contextQuery.Parameters.Add("@MATRICULA", feriadoNacionalEstadual.Matricula);
                    contextQuery.Parameters.Add("@ID", feriadoNacionalEstadual.IdFeriadoNacionalEstadual);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static void Remover(int idFeriadoNacionalEstadual)
        {
            if (idFeriadoNacionalEstadual < 1)
            {
                return;
            }
            
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE TCE_FERIADO_NAC_EST
                            WHERE ID_FERIADO_NAC_EST = @ID_FERIADO_NAC_EST "
                    };
                    contextQuery.Parameters.Add("@ID_FERIADO_NAC_EST", idFeriadoNacionalEstadual);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        private static bool VerificarDuplicidade(TceFeriadoNacionalEstadual feriadoNacionalEstadual)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                try
                {

                    var contextQuery = new ContextQuery();

                    contextQuery.Command = @" SELECT 1 
                        FROM TCE_FERIADO_NAC_EST
                        WHERE DATA = @DATA
                        AND DESCRICAO = @DESCRICAO
                        AND TIPO_EVENTO = @TIPO_EVENTO ";

                    contextQuery.Parameters.Add("@DATA", SqlDbType.Date, feriadoNacionalEstadual.Data);
                    contextQuery.Parameters.Add("@DESCRICAO", feriadoNacionalEstadual.Descricao);
                    contextQuery.Parameters.Add("@TIPO_EVENTO", feriadoNacionalEstadual.TipoEvento);                    

                    object obj = ctx.GetReturnValue(contextQuery);

                    if (obj == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    ctx.Abandon();
                    throw e;
                }
            }
        }

        public static ValidacaoDados Validar(TceFeriadoNacionalEstadual feriadoNacionalEstadual)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (feriadoNacionalEstadual == null)
            {
                return validacaoDados;
            }

            if (feriadoNacionalEstadual.Data.Equals(DateTime.MinValue))
            {
                mensagens.Add("O campo Data é obrigatório!");
            }

            if (feriadoNacionalEstadual.Data < DateTime.Today)
            {
                mensagens.Add("O campo Data não pode ser anterior ao dia atual!");
            }

            if (string.IsNullOrEmpty(feriadoNacionalEstadual.Descricao)
                || (!string.IsNullOrEmpty(feriadoNacionalEstadual.Descricao)
                    && feriadoNacionalEstadual.Descricao.Length > 500))
            {
                mensagens.Add("O campo Descrição é obrigatório com o máximo de 500 caracteres!");
            }

            if (string.IsNullOrEmpty(feriadoNacionalEstadual.TipoEvento)
                || (!string.IsNullOrEmpty(feriadoNacionalEstadual.TipoEvento)
                    && feriadoNacionalEstadual.TipoEvento.Length > 100))
            {
                mensagens.Add("O campo Tipo é obrigatório com o máximo de 100 caracteres!");
            }

            if (VerificarDuplicidade(feriadoNacionalEstadual))
            {
                mensagens.Add("Já existe um feriado cadastrado com estes dados!");
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

        public static ValidacaoDados ValidarExclusao(TceFeriadoNacionalEstadual feriadoNacionalEstadual)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (feriadoNacionalEstadual == null)
            {
                return validacaoDados;
            }
           
            if (feriadoNacionalEstadual.Data < DateTime.Today)
            {
                mensagens.Add("O campo Data não pode ser anterior ao dia atual!");
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

        public static TceFeriadoNacionalEstadual Bind(IDictionary chaves, IDictionary valores)
        {
            return new TceFeriadoNacionalEstadual
            {
                IdFeriadoNacionalEstadual = chaves == null ? 0 : Convert.ToInt32(chaves["ID_FERIADO_NAC_EST"]),
                TipoEvento = Convert.ToString(valores["TIPO_EVENTO"]),
                Descricao = Convert.ToString(valores["DESCRICAO"]),
                Data = Convert.ToDateTime(valores["DATA"] ?? DateTime.MinValue)
            };
        }

    }
}
