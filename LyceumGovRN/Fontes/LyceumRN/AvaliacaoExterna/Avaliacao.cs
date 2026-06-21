using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class Avaliacao
    {
        public int ObtemAnoPorAvaliacaoId(int avaliacaoId)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select ANO from AvaliacaoExterna.Avaliacao where AVALIACAOID = @AVALIACAOID";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable.Rows.Count > 0 ? Convert.ToInt32(dataTable.Rows[0]["ANO"]) : 0;
        }

        public Entidades.Avaliacao ObtemPor(int avaliacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Avaliacao avaliacao = new Entidades.Avaliacao();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                        FROM AVALIACAOEXTERNA.AVALIACAO (NOLOCK)
                                        WHERE AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

                avaliacao = contexto.TryToBindEntity<Entidades.Avaliacao>(contextQuery);

                return avaliacao;
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

        public DataTable Lista()
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select * from AvaliacaoExterna.Avaliacao";

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public DataTable ListaAtivoPorAno(int ano)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select AVALIACAOID, DESCRICAO from AvaliacaoExterna.Avaliacao WHERE ANO = @ANO AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public DataTable ListaAnos()
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select distinct ANO from AvaliacaoExterna.Avaliacao ORDER BY ANO DESC";

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }
        
        public void Insere(Entidades.Avaliacao avaliacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"INSERT INTO AvaliacaoExterna.AVALIACAO
                                                        (TIPOAVALIACAOID,
                                                         DESCRICAO,
                                                         ANO, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO)
                                            VALUES      (@TIPOAVALIACAOID,
                                                         @DESCRICAO, 
                                                         @ANO,
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO)

                                            SELECT IDENT_CURRENT('AvaliacaoExterna.AVALIACAO') ";

                contextQuery.Parameters.Add("@TIPOAVALIACAOID", SqlDbType.Int, avaliacao.TipoAvaliacaoID);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, avaliacao.Descricao);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, avaliacao.Ano);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, avaliacao.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, avaliacao.UsuarioID);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                avaliacao.AvaliacaoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));

                //var numero = ctx.ApplyModifications(contextQuery);

                //ContextQuery contextReturnQuery = new ContextQuery();
                //contextReturnQuery.Command = "";
                //INSERTED inserted = ctx.ApplyModifications<INSERTED>(contextQuery, contextReturnQuery);

                //avaliacao.AvaliacaoId = inserted.ID;
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
                ctx.Dispose();
            }
        }

        public void Altera(Entidades.Avaliacao avaliacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE AvaliacaoExterna.AVALIACAO SET
                                                         TIPOAVALIACAOID = @TIPOAVALIACAOID,
                                                         DESCRICAO = @DESCRICAO,
                                                         ANO = @ANO, 
                                                         ATIVO = @ATIVO, 
                                                         USUARIOID = @USUARIOID, 
                                                         DATAALTERACAO = @DATAALTERACAO
                                          WHERE AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacao.AvaliacaoId);
                contextQuery.Parameters.Add("@TIPOAVALIACAOID", SqlDbType.Int, avaliacao.TipoAvaliacaoID);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, avaliacao.Descricao);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, avaliacao.Ano);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, avaliacao.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, avaliacao.UsuarioID);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);
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
                ctx.Dispose();
            }
        }

        public void Exclui(int avaliacaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE FROM AvaliacaoExterna.AVALIACAO WHERE AVALIACAOID = @AVALIACAOID ";

                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacaoId);

                ctx.ApplyModifications(contextQuery);
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
                ctx.Dispose();
            }
        }

        public ValidacaoDados Valida(Entidades.Avaliacao avaliacao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();

            if (avaliacao == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (avaliacao.AvaliacaoId <= 0)
                {
                    mensagens.Add("Campo AVALIACAOID é obrigatório.");
                }
            }

            //Verificar se a descrição está preenchida
            if (avaliacao.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            //Verificar se o ano está preenchido com um ano válido suportado pelo SQL Server
            try
            {
                var ano = new System.Data.SqlTypes.SqlDateTime(avaliacao.Ano, 12, 31);
            }
            catch
            {
                mensagens.Add("Campo ANO possui um ano inválido.");
            }

            //Verificar se esta descrição com este ano já foi utilizada
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(0) 
                                FROM [AvaliacaoExterna].[AVALIACAO] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
                                AND ANO = @ANO
	                            AND AVALIACAOID <> @AVALIACAOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, avaliacao.Descricao);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, avaliacao.Ano);
                contextQuery.Parameters.Add("@AVALIACAOID", SqlDbType.Int, avaliacao.AvaliacaoId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    mensagens.Add("Esta DESCRIÇÃO com este ANO já foi utilizada.");
                }
            }
            catch (Exception ex)
            {
                if (ctx != null)
                {
                    ctx.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }

            //Verificar se o objetivo da avaliação foi preenchido
            if (avaliacao.TipoAvaliacaoID <= 0)
            {
                mensagens.Add("O OBJETIVO DA AVALIAÇÃO precisa ser selecionado.");
            }

            //Verificar se o id do usuário foi preenchido
            if (avaliacao.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            //Se tiver alguma msg adicionada, invalidar a inserção/alteração
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

        public ValidacaoDados ValidaRemocao(int avaliacao)
        {
            List<string> mensagens = new List<string>();
            RN.AvaliacaoExterna.Prova rnProva = new Prova();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (avaliacao <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                try
                {
                    if (rnProva.PossuiAvaliacaoPor(ctx, avaliacao))
                    {
                        mensagens.Add("Esta avaliação não pode ser excluída porque possui provas vinculadas à ela. Exclua as provas antes.");
                    }
                }
                catch (Exception ex)
                {
                    if (ctx != null)
                    {
                        ctx.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (ctx != null)
                    {
                        ctx.Dispose();
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

        public bool PossuiTipoPor(DataContext contexto, int tipoAvaliacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM AvaliacaoExterna.AVALIACAO (NOLOCK)
                                        WHERE TIPOAVALIACAOID = @TIPOAVALIACAOID ";

            contextQuery.Parameters.Add("@TIPOAVALIACAOID", SqlDbType.Int, tipoAvaliacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}