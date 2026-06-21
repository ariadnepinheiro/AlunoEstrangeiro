using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class EventoCredito
    {
        public bool PossuiEventoCreditoPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 
                SELECT  count(*) 
                FROM PrestacaoContas.EVENTOCREDITO EC (NOLOCK)
                INNER JOIN PrestacaoContas.EXIGENCIAEVENTO EV ON EV.EXIGENCIAEVENTOID = EC.EXIGENCIAEVENTOID
                WHERE EV.EVENTOID = @EVENTOID                
            ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            return contexto.GetReturnValue<bool>(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.EventoCredito eventoCredito)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.EVENTOCREDITO
                                                       (PLANOTRABALHOID, 
                                                        CENSO, 
                                                        EXIGENCIAEVENTOID, 
                                                        EXIGENCIAEXTRATOID, 
                                                        VALOR, 
                                                        DATAEVENTO, 
                                                        USUARIOID, 
                                                        DATACADASTRO, 
                                                        DATAALTERACAO)
                                                 VALUES
                                                       (@PLANOTRABALHOID, 
                                                       @CENSO, 
                                                       @EXIGENCIAEVENTOID, 
                                                       @EXIGENCIAEXTRATOID, 
                                                       @VALOR, 
                                                       @DATAEVENTO, 
                                                       @USUARIOID, 
                                                       @DATACADASTRO, 
                                                       @DATAALTERACAO)
        
                                            SELECT IDENT_CURRENT('PrestacaoContas.EVENTOCREDITO') ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, eventoCredito.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, eventoCredito.Censo);
            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, eventoCredito.ExigenciaEventoId == null || eventoCredito.ExigenciaEventoId <= 0 ? (object)DBNull.Value : eventoCredito.ExigenciaEventoId);
            contextQuery.Parameters.Add("@EXIGENCIAEXTRATOID", SqlDbType.Int, eventoCredito.ExigenciaExtratoId == null || eventoCredito.ExigenciaExtratoId <= 0 ? (object)DBNull.Value : eventoCredito.ExigenciaExtratoId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, eventoCredito.Valor);
            contextQuery.Parameters.Add("@DATAEVENTO", SqlDbType.DateTime, eventoCredito.DataEvento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, eventoCredito.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            eventoCredito.EventoCreditoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void AtualizaNumeroEvento(DataContext contexto, int eventoCreditoId, string numeroEvento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTOCREDITO
                                               SET NUMEROEVENTO = @NUMEROEVENTO
                                             WHERE EVENTOCREDITOID = @EVENTOCREDITOID ";

            contextQuery.Parameters.Add("@EVENTOCREDITOID", SqlDbType.Int, eventoCreditoId);
            contextQuery.Parameters.Add("@NUMEROEVENTO", SqlDbType.VarChar, numeroEvento);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiCreditoExigenciaEventoPor(DataContext contexto, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.EVENTOCREDITO (NOLOCK)
                                    WHERE  EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Atualiza(DataContext contexto, Entidades.EventoCredito eventoCredito)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTOCREDITO
                                               SET VALOR = @VALOR, 
                                                   DATAEVENTO = @DATAEVENTO,
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO
                                             WHERE EVENTOCREDITOID = @EVENTOCREDITOID ";


            contextQuery.Parameters.Add("@EVENTOCREDITOID", SqlDbType.Int, eventoCredito.EventoCreditoId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, eventoCredito.Valor);
            contextQuery.Parameters.Add("@DATAEVENTO", SqlDbType.DateTime, eventoCredito.DataEvento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, eventoCredito.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorEvento(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE EEA
                                    FROM PrestacaoContas.EVENTOCREDITO  EEA
	                                    INNER JOIN LYCEUM.PRESTACAOCONTAS.EXIGENCIAEVENTO E ON EEA.EXIGENCIAEVENTOID = E.EXIGENCIAEVENTOID                                   
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
