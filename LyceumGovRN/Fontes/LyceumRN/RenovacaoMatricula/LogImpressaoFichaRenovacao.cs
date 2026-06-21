using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.RenovacaoMatricula
{
    public class LogImpressaoFichaRenovacao
    {
        public void Insere(RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao logImpressaoFichaRenovacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO RenovacaoMatricula.LOGIMPRESSAOFICHARENOVACAO
                            ( ALUNOID ,
                              RENOVACAOID ,
                              MATRICULA 
                            )
                    VALUES  ( @ALUNOID ,
                              @RENOVACAOID ,
                              @MATRICULA 
                            ) ";

                contextQuery.Parameters.Add("@ALUNOID", logImpressaoFichaRenovacao.AlunoId);
                contextQuery.Parameters.Add("@RENOVACAOID", logImpressaoFichaRenovacao.RenovacaoId);
                contextQuery.Parameters.Add("@MATRICULA", logImpressaoFichaRenovacao.Matricula);

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
