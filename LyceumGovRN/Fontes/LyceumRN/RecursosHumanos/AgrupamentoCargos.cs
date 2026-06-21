using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class AgrupamentoCargos
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT AGRUPAMENTOCARGOSID,
	                                       DESCRICAO,
	                                       CARGAHORARIA,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [RecursosHumanos].[AGRUPAMENTOCARGOS] (NOLOCK) ";

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT AGRUPAMENTOCARGOSID,
	                                       DESCRICAO,
	                                       CARGAHORARIA,
                                           (CONVERT(VARCHAR,AGRUPAMENTOCARGOSID) + '_' + CONVERT(VARCHAR,CARGAHORARIA)) as CHAVE,
                                           ATIVO,
                                           USUARIOID,
                                           DATACADASTRO,
                                           DATAALTERACAO
                                    FROM   [RecursosHumanos].[AGRUPAMENTOCARGOS] (NOLOCK)
                                            WHERE ATIVO = 1";

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public ValidacaoDados Valida(Entidades.AgrupamentoCargos agrupamentoCargos, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (agrupamentoCargos == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (agrupamentoCargos.AgrupamentoCargosId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (agrupamentoCargos.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (agrupamentoCargos.CargaHoraria <= 0)
            {
                mensagens.Add("Campo CARGA HORARIA é obrigatório.");
            }

            if (agrupamentoCargos.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, agrupamentoCargos.Descricao, agrupamentoCargos.AgrupamentoCargosId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int agrupamentoCargosId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM [RecursosHumanos].[AGRUPAMENTOCARGOS] (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND AGRUPAMENTOCARGOSID <> @AGRUPAMENTOCARGOSID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargosId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.AgrupamentoCargos agrupamentoCargos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.AGRUPAMENTOCARGOS
                                                        (DESCRICAO, 
                                                         CARGAHORARIA,
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @CARGAHORARIA,
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, agrupamentoCargos.Descricao);
                contextQuery.Parameters.Add("@CARGAHORARIA", SqlDbType.Int, agrupamentoCargos.CargaHoraria);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, agrupamentoCargos.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, agrupamentoCargos.UsuarioId);
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

        public void Atualiza(Entidades.AgrupamentoCargos agrupamentoCargos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RecursosHumanos.AGRUPAMENTOCARGOS
                                        SET    DESCRICAO = @DESCRICAO, 
                                               CARGAHORARIA = @CARGAHORARIA,
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, agrupamentoCargos.Descricao);
                contextQuery.Parameters.Add("@CARGAHORARIA", SqlDbType.Int, agrupamentoCargos.CargaHoraria);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, agrupamentoCargos.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, agrupamentoCargos.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargos.AgrupamentoCargosId);

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

        public ValidacaoDados ValidaRemocao(int agrupamentoCargosId)
        {
            List<string> mensagens = new List<string>();
            CategoriaDocente rnCategoriaDocente = new CategoriaDocente();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (agrupamentoCargosId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado 
                    if (rnCategoriaDocente.PossuiAgrupamentoCargosPor(contexto, agrupamentoCargosId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        public void Remove(int agrupamentoCargosId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RecursosHumanos.AGRUPAMENTOCARGOS
                            WHERE  AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID  ";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargosId);

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

        public int? ObtemCargaHorariaPor(int agrupamentoCargosId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = "SELECT CARGAHORARIA FROM RecursosHumanos.AGRUPAMENTOCARGOS WHERE AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargosId);

                return ctx.GetReturnValue<int?>(contextQuery);
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


    }
}
