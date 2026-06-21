using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class ExecucaoQuery : QueryBase<ExecucaoQuery>
    {
        public Execucao Insere(Execucao execucao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO LYCEUM.CARTAOESTUDANTE.EXECUCAO 
                                                        (PROCESSOID, 
                                                         DATAEXECUCAOINICIO, 
                                                         DATAEXECUCAOFIM, 
                                                         SITUACAOEXECUCAO, 
                                                         DATAINCLUSAO) 
                                            VALUES      (@PROCESSOID, 
                                                         @DATAEXECUCAOINICIO, 
                                                         @DATAEXECUCAOFIM, 
                                                         @SITUACAOEXECUCAO, 
                                                         GETDATE())  
                                    
                        SELECT IDENT_CURRENT('CARTAOESTUDANTE.EXECUCAO') ";

                contextQuery.Parameters.Add("@PROCESSOID", SqlDbType.Int, execucao.ProcessoId);
                contextQuery.Parameters.Add("@DATAEXECUCAOINICIO", SqlDbType.DateTime, execucao.DataInicioExecucao);
                contextQuery.Parameters.Add("@DATAEXECUCAOFIM", SqlDbType.DateTime, execucao.DataFimExecucao);
                contextQuery.Parameters.Add("@SITUACAOEXECUCAO", SqlDbType.Bit, execucao.SituacaoExecucao);

                execucao.ExecucaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

                return execucao;
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

    }
}