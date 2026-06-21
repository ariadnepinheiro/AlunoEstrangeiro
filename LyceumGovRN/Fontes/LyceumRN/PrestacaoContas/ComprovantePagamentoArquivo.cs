using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ComprovantePagamentoArquivo
    {
        public byte[] ObtemArquivoPor(int comprovantePagamentoArquivoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO (NOLOCK) 
											WHERE COMPROVANTEPAGAMENTOARQUIVOID = @COMPROVANTEPAGAMENTOARQUIVOID ";

                contextQuery.Parameters.Add("@COMPROVANTEPAGAMENTOARQUIVOID", SqlDbType.Int, comprovantePagamentoArquivoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
                }

                return arquivo;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public bool ExisteComprovanteArquivoPor(DataContext contexto, int eventoid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO (NOLOCK) 
                                         WHERE EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoid);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Atualiza(DataContext contexto, Entidades.ComprovantePagamentoArquivo comprovantePagamentoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO
                                       SET									    
										ARQUIVO = @ARQUIVO
										,TIPOARQUIVO = @TIPOARQUIVO
										,NOMEARQUIVO = @NOMEARQUIVO 
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, comprovantePagamentoArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, comprovantePagamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, comprovantePagamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, comprovantePagamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, comprovantePagamentoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.ComprovantePagamentoArquivo comprovantePagamentoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO
                                           (EVENTOID
                                           ,CHAVEARQUIVO
                                           ,ARQUIVO
                                           ,TIPOARQUIVO
                                           , NOMEARQUIVO                                          
                                           ,USUARIOID
                                           ,DATACADASTRO )
                                     VALUES
	                                       (@EVENTOID
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
										   ) 

                         SELECT IDENT_CURRENT('PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO') ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, comprovantePagamentoArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, comprovantePagamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, comprovantePagamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, comprovantePagamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, comprovantePagamentoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            comprovantePagamentoArquivo.ComprovantePagamentoArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void InsereAuditoria(DataContext contexto, Entidades.ComprovantePagamentoArquivo comprovantePagamentoArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO
                                               (COMPROVANTEPAGAMENTOARQUIVOID
                                               ,EVENTOID
                                               ,CHAVEARQUIVO
                                               ,ARQUIVO
                                               ,TIPOARQUIVO
                                               ,NOMEARQUIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO
                                               ,DATAAUDITORIA
                                               ,OPERACAO
                                               ,ESTACAO )
                                         VALUES
                                               (@COMPROVANTEPAGAMENTOARQUIVOID, 
                                               @EVENTOID,
                                               NEWID(), 
                                               @ARQUIVO,
                                               @TIPOARQUIVO, 
                                               @NOMEARQUIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO,
                                               @DATAAUDITORIA,
                                               @OPERACAO,
                                               @ESTACAO) 

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.FORNECEDORDOCUMENTO') ";

            contextQuery.Parameters.Add("@COMPROVANTEPAGAMENTOARQUIVOID", SqlDbType.Int, comprovantePagamentoArquivo.ComprovantePagamentoArquivoId);
            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, comprovantePagamentoArquivo.EventoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, comprovantePagamentoArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, comprovantePagamentoArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, comprovantePagamentoArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, comprovantePagamentoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoria(DataContext contexto, int eventoId, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.PRESTACAOCONTAS.COMPROVANTEPAGAMENTOARQUIVO 
                                                    (COMPROVANTEPAGAMENTOARQUIVOID, 
                                                     EVENTOID, 
                                                     CHAVEARQUIVO, 
                                                     ARQUIVO, 
                                                     TIPOARQUIVO, 
                                                     NOMEARQUIVO, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO, 
                                                     DATAAUDITORIA,
                                                     OPERACAO, 
                                                     ESTACAO) 
                                        SELECT COMPROVANTEPAGAMENTOARQUIVOID, 
                                               EVENTOID, 
                                               NEWID(), 
                                               ARQUIVO, 
                                               TIPOARQUIVO, 
                                               NOMEARQUIVO, 
                                               @USUARIOID, 
                                               DATACADASTRO, 
                                               DATAALTERACAO, 
                                               @DATAAUDITORIA,
                                               @OPERACAO, 
                                               @ESTACAO 
                                        FROM   LYCEUM.PRESTACAOCONTAS.COMPROVANTEPAGAMENTOARQUIVO 
                                        WHERE  EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, HttpContext.Current.User.Identity.Name);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PRESTACAOCONTAS.COMPROVANTEPAGAMENTOARQUIVO                                     
                                        WHERE  EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
