using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class EmailNaoEnviado
    {
        public void Insere(Entidades.EmailNaoEnviado emailNaoEnviado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.EMAILNAOENVIADO
                                               (PROJETO
                                               ,REMETENTE
                                               ,DESTINATARIO
                                               ,ASSUNTO
                                               ,TEXTO
                                               ,ENVIADO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PROJETO, 
                                               @REMETENTE,
                                               @DESTINATARIO,
                                               @ASSUNTO, 
                                               @TEXTO,
                                               @ENVIADO,
                                               @USUARIOID, 
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@PROJETO", SqlDbType.VarChar, emailNaoEnviado.Projeto);
                contextQuery.Parameters.Add("@REMETENTE", SqlDbType.VarChar, emailNaoEnviado.Remetente);
                contextQuery.Parameters.Add("@DESTINATARIO", SqlDbType.VarChar, emailNaoEnviado.Destinatario);
                contextQuery.Parameters.Add("@ASSUNTO", SqlDbType.VarChar, emailNaoEnviado.Assunto);
                contextQuery.Parameters.Add("@TEXTO", SqlDbType.VarChar, emailNaoEnviado.Texto);
                contextQuery.Parameters.Add("@ENVIADO", SqlDbType.Bit, false);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, emailNaoEnviado.UsuarioId);
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
    }
}
