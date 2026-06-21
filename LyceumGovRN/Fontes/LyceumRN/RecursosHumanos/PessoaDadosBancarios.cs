using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class PessoaDadosBancarios
    {
        public RecursosHumanos.Entidades.PessoaDadosBancarios ObtemAtivoPor(DataContext ctx, decimal pessoaId)
        {
            RecursosHumanos.Entidades.PessoaDadosBancarios pessoaDadosBancarios = new RecursosHumanos.Entidades.PessoaDadosBancarios();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT   * 
                                            FROM   RECURSOSHUMANOS.PESSOADADOSBANCARIOS 
                                            WHERE  PESSOAID = @PESSOAID
                                                   AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@PESSOAID", pessoaId);

            pessoaDadosBancarios = ctx.TryToBindEntity<RecursosHumanos.Entidades.PessoaDadosBancarios>(contextQuery);

            return pessoaDadosBancarios;
        }

        public void Insere(DataContext ctx, RecursosHumanos.Entidades.PessoaDadosBancarios pessoaDadosBancarios)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.PESSOADADOSBANCARIOS
                                       (PESSOAID
                                       ,BANCO
                                       ,AGENCIA
                                       ,CONTABANCO
                                       ,ATIVO
                                       ,USUARIOID
                                       ,DATACADASTRO
                                       ,DATAALTERACAO)
                                 VALUES
                                       (@PESSOAID
                                       ,@BANCO
                                       ,@AGENCIA
                                       ,@CONTABANCO
                                       ,1
                                       ,@USUARIOID
                                       ,@DATACADASTRO
                                       ,@DATAALTERACAO) ";

                contextQuery.Parameters.Add("@PESSOAID", pessoaDadosBancarios.PessoaId);
                contextQuery.Parameters.Add("@BANCO", pessoaDadosBancarios.Banco);
                contextQuery.Parameters.Add("@AGENCIA", pessoaDadosBancarios.Agencia);
                contextQuery.Parameters.Add("@CONTABANCO", pessoaDadosBancarios.ContaBanco);
                contextQuery.Parameters.Add("@USUARIOID", pessoaDadosBancarios.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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
        }

        public void Desativa(DataContext ctx, decimal pessoa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RECURSOSHUMANOS.PESSOADADOSBANCARIOS 
                                            SET    ATIVO = 0, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                           WHERE  PESSOAID = @PESSOAID
                                                  AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@PESSOAID", pessoa);
                contextQuery.Parameters.Add("@USUARIOID", usuarioResponsavel);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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
        }

        public void Remove(DataContext ctx, decimal pessoaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RECURSOSHUMANOS.PESSOADADOSBANCARIOS 
                                        WHERE  PESSOAID = @PESSOAID  ";

                contextQuery.Parameters.Add("@PESSOAID", pessoaId);

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
        }
    }
}
