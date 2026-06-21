using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class TermoCompromissoDocente: RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT  ID_TERMO_DOCENTE, ANO, DT_INICIO, DT_FIM, ARQUIVO
                            FROM    dbo.TCE_TERMO_COMPROMISSO_DOCENTE "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Inserir(TceTermoCompromissoDocente termoCompromissoDocente)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.TCE_TERMO_COMPROMISSO_DOCENTE ( ANO, DT_INICIO, DT_FIM,
                                                ARQUIVO, MATRICULA )
                                    VALUES  ( @ANO, @DT_INICIO, @DT_FIM, @ARQUIVO, @MATRICULA ) "
                    };

                    contextQuery.Parameters.Add("@ANO", termoCompromissoDocente.Ano);
                    contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.Date, termoCompromissoDocente.DtInicio);
                    contextQuery.Parameters.Add("@DT_FIM", SqlDbType.Date, termoCompromissoDocente.DtFim);
                    contextQuery.Parameters.Add("@ARQUIVO", termoCompromissoDocente.Arquivo);
                    contextQuery.Parameters.Add("@MATRICULA", termoCompromissoDocente.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TceTermoCompromissoDocente termoCompromissoDocente)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE  dbo.TCE_TERMO_COMPROMISSO_DOCENTE
                                    SET     DT_INICIO = @DT_INICIO, 
		                                    DT_FIM = @DT_FIM, 
                                            DT_ALTERACAO = GETDATE(), 
                                            MATRICULA = @MATRICULA
                                    WHERE   ID_TERMO_DOCENTE = @ID_TERMO_DOCENTE "
                    };

                    contextQuery.Parameters.Add("@ID_TERMO_DOCENTE", termoCompromissoDocente.IdTermoDocente);
                    contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.Date, termoCompromissoDocente.DtInicio);
                    contextQuery.Parameters.Add("@DT_FIM", SqlDbType.Date, termoCompromissoDocente.DtFim);
                    contextQuery.Parameters.Add("@MATRICULA", termoCompromissoDocente.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
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
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE dbo.TCE_TERMO_COMPROMISSO_DOCENTE
                                        WHERE ID_TERMO_DOCENTE = @ID_TERMO_DOCENTE"
                    };

                    contextQuery.Parameters.Add("@ID_TERMO_DOCENTE", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceTermoCompromissoDocente termoCompromissoDocente)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (termoCompromissoDocente == null)
            {
                return validacaoDados;
            }

            //verificar se é inserção
            if (termoCompromissoDocente.IdTermoDocente <= 0)
            {
                //se for inserção verificar preenchimento do ANO
                if (termoCompromissoDocente.Ano <= 0)
                {
                    mensagens.Add("O campo ANO é obrigatório!");
                }
            }

            if (termoCompromissoDocente.DtInicio.Equals(DateTime.MinValue) || !Validacao.ValidouData(termoCompromissoDocente.DtInicio, Validacao.Tipo.data))
            {
                mensagens.Add("O campo DATA DE INÍCIO é obrigatório e deve ser uma data válida!");
            }

            if (termoCompromissoDocente.DtFim.Equals(DateTime.MinValue) || !Validacao.ValidouData(termoCompromissoDocente.DtInicio, Validacao.Tipo.data))
            {
                mensagens.Add("O campo DATA DE FIM é obrigatório e deve ser uma data válida!");
            }

            if (!termoCompromissoDocente.DtInicio.Equals(DateTime.MinValue) && !termoCompromissoDocente.DtFim.Equals(DateTime.MinValue))
            {
                if (termoCompromissoDocente.DtInicio > termoCompromissoDocente.DtFim)
                {
                    mensagens.Add("O campo DATA DE FIM deve ser maior que o campo DATA DE INÍCIO!");
                }
            }

            if (string.IsNullOrEmpty(termoCompromissoDocente.Matricula) || termoCompromissoDocente.Matricula.Length > 8)
            {
                mensagens.Add("O USUARIO RESPONSÁVEL é obrigatório e seu login deve ter no máximo 8 caracteres!");
            }

            if (string.IsNullOrEmpty(termoCompromissoDocente.Arquivo))
            {
                mensagens.Add("O campo ARQUIVO é obrigatório.");
            }
            else if (termoCompromissoDocente.Arquivo.Length > 500 || termoCompromissoDocente.Arquivo.Substring(termoCompromissoDocente.Arquivo.Length - 4, 4) != ".htm")
            {
                mensagens.Add("O campo ARQUIVO do tipo '.htm' é obrigatório e deve ter no máximo 500 caracteres!");

            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    DBO.TCE_TERMO_COMPROMISSO_DOCENTE
                            WHERE   ANO = @ANO
                                    AND ID_TERMO_DOCENTE <> @ID_TERMO_DOCENTE ");

                    contextQuery.Parameters.Add("@ANO", termoCompromissoDocente.Ano);
                    contextQuery.Parameters.Add("@ID_TERMO_DOCENTE", termoCompromissoDocente.IdTermoDocente);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe um termo de compromisso cadastrado com este ano.");
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

        public static ValidacaoDados ValidarRemover(int id)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id <= 0)
            {
                return validacaoDados;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  1
                        FROM    TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE
                        WHERE   ID_TERMO_DOCENTE = @ID_TERMO_DOCENTE ");

                contextQuery.Parameters.Add("@ID_TERMO_DOCENTE", id);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    mensagens.Add("Não é possível deletar este termo, pois ele já foi utilizado.");
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
    }
}
