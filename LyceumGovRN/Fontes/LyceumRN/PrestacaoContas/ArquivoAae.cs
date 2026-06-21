using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ArquivoAae
    {
        public int RetornaArquivoAaeIdPor(DataContext contexto, int mandatoAae, out string tipoArquivo, out string nomeArquivo)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;
            tipoArquivo = String.Empty;
            nomeArquivo = string.Empty;

            try
            {
                contextQuery.Command = @"   SELECT ARQUIVOAAEID, TIPOARQUIVO,NOMEARQUIVO 
                                            FROM PrestacaoContas.ARQUIVOAAE
                                            WHERE MANDATOAAEID = @MANDATOAAE ";

                contextQuery.Parameters.Add("@MANDATOAAE", SqlDbType.Int, mandatoAae);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ARQUIVOAAEID"]);
                    tipoArquivo = Convert.ToString(reader["TIPOARQUIVO"]);
                    nomeArquivo = Convert.ToString(reader["NOMEARQUIVO"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        
        public byte[] ObtemArquivoPor(int arquivoAaeId)
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
                                            FROM   PrestacaoContas.ARQUIVOAAE (NOLOCK) 
											where ARQUIVOAAEID = @ARQUIVOAAEID ";

                contextQuery.Parameters.Add("@ARQUIVOAAEID", SqlDbType.Int, arquivoAaeId);

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

        public bool ExistePor(DataContext contexto, int mandatoAaeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   PrestacaoContas.ARQUIVOAAE (NOLOCK) 
                                        WHERE  MANDATOAAEID = @MANDATOAAEID ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAaeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Atualiza(DataContext contexto, Entidades.ArquivoAae arquivoAae)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.ARQUIVOAAE
                                       SET									    
										ARQUIVO = @ARQUIVO
										,TIPOARQUIVO = @TIPOARQUIVO
										,NOMEARQUIVO = @NOMEARQUIVO 
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE MANDATOAAEID = @MANDATOAAEID ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, arquivoAae.MandatoAaeId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, arquivoAae.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, arquivoAae.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, arquivoAae.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, arquivoAae.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.ArquivoAae arquivoAae)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.ARQUIVOAAE
                                           (MANDATOAAEID
                                           ,CHAVEARQUIVO
                                           ,ARQUIVO
                                           ,TIPOARQUIVO
                                           , NOMEARQUIVO                                          
                                           ,USUARIOID
                                           ,DATACADASTRO )
                                     VALUES
	                                       (@MANDATOAAEID
                                           ,NEWID()
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
										   ) 

                         SELECT IDENT_CURRENT('PrestacaoContas.ARQUIVOAAE') ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, arquivoAae.MandatoAaeId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, arquivoAae.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, arquivoAae.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, arquivoAae.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, arquivoAae.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            arquivoAae.ArquivoAaeId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void InsereAuditoria(DataContext contexto, Entidades.ArquivoAae arquivoAae, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.ARQUIVOAAE
                                               (ARQUIVOAAEID
                                               ,MANDATOAAEID
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
                                               (@ARQUIVOAAEID, 
                                               @MANDATOAAEID,
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

            contextQuery.Parameters.Add("@ARQUIVOAAEID", SqlDbType.Int, arquivoAae.ArquivoAaeId);
            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, arquivoAae.MandatoAaeId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, arquivoAae.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, arquivoAae.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, arquivoAae.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, arquivoAae.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorMandato(DataContext ctx, int mandatoAaeId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PrestacaoContas.ARQUIVOAAE
                            WHERE  MANDATOAAEID = @MANDATOAAEID  ";

            contextQuery.Parameters.Add("@MANDATOAAEID", SqlDbType.Int, mandatoAaeId);

            ctx.ApplyModifications(contextQuery);
        }
    }
}
