using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class ParametroBloqueioEncerramento
    {
        public Entidades.ParametroBloqueioEncerramento ObtemPor(DataContext contexto, int agendaId)
        {
            Entidades.ParametroBloqueioEncerramento entidade = new Techne.Lyceum.RN.Agenda.Entidades.ParametroBloqueioEncerramento();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                FROM AGENDA.PARAMETROBLOQUEIOENCERRAMENTO (NOLOCK)
                                WHERE AGENDAID = @AGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID", SqlDbType.Int, agendaId); 

                entidade = contexto.TryToBindEntity<Entidades.ParametroBloqueioEncerramento>(contextQuery);

                return entidade;
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
