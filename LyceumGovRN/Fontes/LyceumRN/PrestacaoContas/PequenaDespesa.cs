using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class PequenaDespesa
    {
        public Entidades.PequenaDespesa ObtemPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemPor(contexto, eventoId);
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

        public Entidades.PequenaDespesa ObtemPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select * from PrestacaoContas.PEQUENADESPESA (nolock)                    
                    where EVENTOID = @EVENTOID ";
                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
                return contexto.TryToBindEntity<Entidades.PequenaDespesa>(contextQuery);
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
        }

        public bool PossuiTipoTransportePor(DataContext contexto, int tipoTransporteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PEQUENADESPESA (NOLOCK)
                                    WHERE TIPOTRANSPORTEID = @TIPOTRANSPORTEID ";

            contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, tipoTransporteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
               existe = true;
            }

            return existe;
        }

        public void InsereEventoPequenasDespesasComComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.PEQUENADESPESA
                                               (EVENTOID
                                               ,TIPODESPESA
                                               ,FORMAPAGAMENTO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@EVENTOID, 
                                               @TIPODESPESA, 
                                               @FORMAPAGAMENTO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.VarChar, "PEQUENADESPESA");
            contextQuery.Parameters.Add("@FORMAPAGAMENTO", SqlDbType.VarChar, dados.FormaPagamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereEventoPequenasDespesasSemComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.PEQUENADESPESA
                                               (EVENTOID
                                               ,TIPODESPESA
                                               ,FORMAPAGAMENTO
                                               ,JUSTIFICATIVA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@EVENTOID, 
                                               @TIPODESPESA, 
                                               @FORMAPAGAMENTO, 
                                               @JUSTIFICATIVA,
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.VarChar, "SEMCOMPROVACAO");
            contextQuery.Parameters.Add("@FORMAPAGAMENTO", SqlDbType.VarChar, dados.FormaPagamento);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, dados.Justificativa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaEventoPequenasDespesasComComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE PrestacaoContas.PEQUENADESPESA
                                            SET FORMAPAGAMENTO = @FORMAPAGAMENTO,
                                                USUARIOID = @USUARIOID,
                                                DATAALTERACAO = @DATAALTERACAO
                                        WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@FORMAPAGAMENTO", SqlDbType.VarChar, dados.FormaPagamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaEventoPequenasDespesasSemComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE PrestacaoContas.PEQUENADESPESA
                                            SET FORMAPAGAMENTO = @FORMAPAGAMENTO,
                                                JUSTIFICATIVA = @JUSTIFICATIVA,
                                                USUARIOID = @USUARIOID,
                                                DATAALTERACAO = @DATAALTERACAO
                                        WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@FORMAPAGAMENTO", SqlDbType.VarChar, dados.FormaPagamento);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, dados.Justificativa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereEventoTransporte(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.PEQUENADESPESA
                                           (EVENTOID
                                           ,TIPOTRANSPORTEID
                                           ,TIPODESPESA
                                           ,JUSTIFICATIVA
                                           ,ORIGEM
                                           ,DESTINO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@EVENTOID, 
                                           @TIPOTRANSPORTEID, 
                                           @TIPODESPESA, 
                                           @JUSTIFICATIVA,
                                           @ORIGEM, 
                                           @DESTINO, 
                                           @USUARIOID, 
                                           @DATACADASTRO,
                                           @DATAALTERACAO) 

                                SELECT IDENT_CURRENT('PrestacaoContas.PEQUENADESPESA')";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, dados.TipoTransporteId);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.VarChar, "TRANSPORTE");
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, dados.Justificativa);
            contextQuery.Parameters.Add("@ORIGEM", SqlDbType.VarChar, dados.Origem);
            contextQuery.Parameters.Add("@DESTINO", SqlDbType.VarChar, dados.Destino);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


            dados.PequenaDespesaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

        }

        public void AtualizaEventoTransporte(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.PEQUENADESPESA
                                    SET TIPOTRANSPORTEID = @TIPOTRANSPORTEID,
                                        JUSTIFICATIVA = @JUSTIFICATIVA,
                                        ORIGEM = @ORIGEM,
                                        DESTINO = @DESTINO,
                                        USUARIOID = @USUARIOID,
                                        DATAALTERACAO = @DATAALTERACAO
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@TIPOTRANSPORTEID", SqlDbType.Int, dados.TipoTransporteId);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, dados.Justificativa);
            contextQuery.Parameters.Add("@ORIGEM", SqlDbType.VarChar, dados.Origem);
            contextQuery.Parameters.Add("@DESTINO", SqlDbType.VarChar, dados.Destino);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  insert into PrestacaoContas.PEQUENADESPESA_EXCLUIDO 
                                         SELECT *,GETDATE() FROM PrestacaoContas.PEQUENADESPESA                                    
                                                                            WHERE EVENTOID = @EVENTOID 

                                        DELETE PrestacaoContas.PEQUENADESPESA                                    
                                          WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiPequenaDespesaPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PEQUENADESPESA (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

    }
}
