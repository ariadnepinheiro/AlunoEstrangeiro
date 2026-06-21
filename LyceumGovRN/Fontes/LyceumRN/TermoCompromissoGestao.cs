using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class TermoCompromissoGestao : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @"SELECT  tg.ID_TERMO_GESTAO, tg.ANO, p.NOME AS 'PADRAO_ACESSO', TG.DT_INICIO,
                                    TG.DT_FIM, tg.ARQUIVO, tg.DT_ALTERACAO
                            FROM    dbo.TCE_TERMO_COMPROMISSO_GESTAO tg
                                    INNER JOIN HADES.dbo.HD_PADACES p ON tg.PADRAO_ACESSO = p.PADACES "
                                       };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Inserir(TceTermoCompromissoGestao termoCompromissoGestao)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT  INTO dbo.TCE_TERMO_COMPROMISSO_GESTAO ( ANO, PADRAO_ACESSO, DT_INICIO,
                                                     DT_FIM, ARQUIVO, MATRICULA )
                                        VALUES  ( @ANO, @PADRAO_ACESSO, @DT_INICIO, @DT_FIM, @ARQUIVO, @MATRICULA ) "
                    };

                    contextQuery.Parameters.Add("@ANO", termoCompromissoGestao.Ano);
                    contextQuery.Parameters.Add("@PADRAO_ACESSO", termoCompromissoGestao.PadraoAcesso);
                    contextQuery.Parameters.Add("@DT_INICIO",SqlDbType.Date, termoCompromissoGestao.DtInicio);
                    contextQuery.Parameters.Add("@DT_FIM",SqlDbType.Date, termoCompromissoGestao.DtFim);
                    contextQuery.Parameters.Add("@ARQUIVO", termoCompromissoGestao.Arquivo);
                    contextQuery.Parameters.Add("@MATRICULA", termoCompromissoGestao.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TceTermoCompromissoGestao termoCompromissoGestao)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"UPDATE  dbo.TCE_TERMO_COMPROMISSO_GESTAO
                                    SET    
		                                    DT_INICIO = @DT_INICIO,
                                            DT_FIM = @DT_FIM, 		                                   
		                                    DT_ALTERACAO = GETDATE(),
                                            MATRICULA = @MATRICULA
                                    WHERE   ID_TERMO_GESTAO = @ID_TERMO_GESTAO "
                    };

                    contextQuery.Parameters.Add("@ID_TERMO_GESTAO", termoCompromissoGestao.IdTermoGestao);
                    contextQuery.Parameters.Add("@DT_INICIO", SqlDbType.Date, termoCompromissoGestao.DtInicio);
                    contextQuery.Parameters.Add("@DT_FIM", SqlDbType.Date, termoCompromissoGestao.DtFim);
                    contextQuery.Parameters.Add("@MATRICULA", termoCompromissoGestao.Matricula);

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
                        Command = @"DELETE dbo.TCE_TERMO_COMPROMISSO_GESTAO
                                        WHERE ID_TERMO_GESTAO = @ID_TERMO_GESTAO"
                    };

                    contextQuery.Parameters.Add("@ID_TERMO_GESTAO", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados Validar(TceTermoCompromissoGestao termoCompromissoGestao)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (termoCompromissoGestao == null)
            {
                return validacaoDados;
            }

            //verificar se é inserção
            if (termoCompromissoGestao.IdTermoGestao <= 0)
            {
                //se for inserção verificar preenchimento do ANO
                if (termoCompromissoGestao.Ano <= 0)
                {
                    mensagens.Add("O campo ANO é obrigatório!");
                }
            }

            if (string.IsNullOrEmpty(termoCompromissoGestao.PadraoAcesso))
            {
                mensagens.Add("O campo PADRÃO DE ACESSO é obrigatório!");
            }

            if (termoCompromissoGestao.DtFim.Equals(DateTime.MinValue) || !Validacao.ValidouData(termoCompromissoGestao.DtInicio, Validacao.Tipo.data))
            {
                mensagens.Add("O campo DATA DE FIM é obrigatório e deve ser uma data válida!");
            }

            if (termoCompromissoGestao.DtInicio.Equals(DateTime.MinValue) || !Validacao.ValidouData(termoCompromissoGestao.DtInicio, Validacao.Tipo.data))
            {
                mensagens.Add("O campo DATA DE INÍCIO é obrigatório e deve ser uma data válida!");
            }

            if (!termoCompromissoGestao.DtInicio.Equals(DateTime.MinValue) && !termoCompromissoGestao.DtFim.Equals(DateTime.MinValue))
            {
                if (termoCompromissoGestao.DtInicio > termoCompromissoGestao.DtFim)
                {
                    mensagens.Add("O campo DATA DE FIM deve ser maior que o campo DATA DE INÍCIO!");
                }
            }

            if (string.IsNullOrEmpty(termoCompromissoGestao.Matricula) || termoCompromissoGestao.Matricula.Length > 8)
            {
                mensagens.Add("O USUARIO RESPONSÁVEL é obrigatório e seu login deve ter no máximo 8 caracteres!");
            }

            if (string.IsNullOrEmpty(termoCompromissoGestao.Arquivo))
            {
                mensagens.Add("O campo ARQUIVO é obrigatório.");
            }
            else if(termoCompromissoGestao.Arquivo.Length > 500 || termoCompromissoGestao.Arquivo.Substring(termoCompromissoGestao.Arquivo.Length - 4, 4) != ".htm")
            {
                mensagens.Add("O campo ARQUIVO do tipo '.htm' é obrigatório e deve ter no máximo 500 caracteres!");
               
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    DBO.TCE_TERMO_COMPROMISSO_GESTAO
                            WHERE   ANO = @ANO
                                    AND PADRAO_ACESSO = @PADRAO
                                    AND ID_TERMO_GESTAO <> @ID_TERMO_GESTAO ");

                    contextQuery.Parameters.Add("@ANO", termoCompromissoGestao.Ano);
                    contextQuery.Parameters.Add("@ID_TERMO_GESTAO", termoCompromissoGestao.IdTermoGestao);
                    contextQuery.Parameters.Add("@PADRAO", termoCompromissoGestao.PadraoAcesso);


                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe um termo de compromisso cadastrado para este ano/padrão de acesso.");
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
                        FROM    TCE_ACEITE_TERMO_COMPROMISSO_GESTAO
                        WHERE   ID_TERMO_GESTAO = @ID_TERMO_GESTAO ");

                contextQuery.Parameters.Add("@ID_TERMO_GESTAO", id);

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
