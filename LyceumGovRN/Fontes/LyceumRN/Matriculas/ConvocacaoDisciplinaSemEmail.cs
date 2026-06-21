using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class ConvocacaoDisciplinaSemEmail
    {
        public void Insere(int matriculaEspecialDisciplinaId, int matriculaEspecialId, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                this.Insere(contexto, matriculaEspecialDisciplinaId, matriculaEspecialId, usuarioId);
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

        public void Insere(DataContext contexto, int matriculaEspecialDisciplinaId, int matriculaEspecialId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.CONVOCACAODISCIPLINASEMEMAIL
                                               (MATRICULAESPECIALDISCIPLINAID
                                               ,MATRICULAESPECIALID
                                               ,USUARIOID
                                               ,DATAAVISO)
                                         VALUES
                                               (@MATRICULAESPECIALDISCIPLINAID, 
                                               @MATRICULAESPECIALID, 
                                               @USUARIOID, 
                                               @DATAAVISO)  ";

            contextQuery.Parameters.Add("@MATRICULAESPECIALDISCIPLINAID", SqlDbType.Int, matriculaEspecialDisciplinaId);
            contextQuery.Parameters.Add("@MATRICULAESPECIALID", SqlDbType.Int, matriculaEspecialId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAAVISO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
