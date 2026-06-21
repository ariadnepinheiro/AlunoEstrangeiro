using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class DeclaracaoFiscalArquivo
    {
        public byte[] ObtemArquivoPor(int obrigacaoFiscalAaeId)
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
                                            FROM   PrestacaoContas.DECLARACAOFISCALARQUIVO (NOLOCK) 
											where OBRIGACAOFISCALAAEID = @OBRIGACAOFISCALAAEID ";

                contextQuery.Parameters.Add("@OBRIGACAOFISCALAAEID", SqlDbType.Int, obrigacaoFiscalAaeId);

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

        public void Insere(DataContext contexto, Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.DECLARACAOFISCALARQUIVO
                                               (OBRIGACAOFISCALAAEID 
                                               ,CHAVEARQUIVO
                                               ,ARQUIVO
                                               ,TIPOARQUIVO
                                               ,NOMEARQUIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@OBRIGACAOFISCALAAEID, 
                                               NEWID(), 
                                               @ARQUIVO,
                                               @TIPOARQUIVO, 
                                               @NOMEARQUIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) 

                                        SELECT IDENT_CURRENT('PRESTACAOCONTAS.DECLARACAOFISCALARQUIVO') ";

            contextQuery.Parameters.Add("@OBRIGACAOFISCALAAEID", SqlDbType.Int, declaracaoFiscalArquivo.ObrigacaoFiscalAaeId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, declaracaoFiscalArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, declaracaoFiscalArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, declaracaoFiscalArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, declaracaoFiscalArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            declaracaoFiscalArquivo.DeclaracaoFiscalArquivoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(DataContext ctx, Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE prestacaocontas.DECLARACAOFISCALARQUIVO 
                                            SET    ARQUIVO = @ARQUIVO, 
                                                   TIPOARQUIVO = @TIPOARQUIVO, 
                                                   NOMEARQUIVO = @NOMEARQUIVO, 
                                                   USUARIOID = @USUARIOID, 
                                                   DATAALTERACAO = @DATAALTERACAO 
                                            WHERE  OBRIGACAOFISCALAAEID = @OBRIGACAOFISCALAAEID ";

            contextQuery.Parameters.Add("@OBRIGACAOFISCALAAEID", SqlDbType.Int, declaracaoFiscalArquivo.ObrigacaoFiscalAaeId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, declaracaoFiscalArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, declaracaoFiscalArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, declaracaoFiscalArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, declaracaoFiscalArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public void InsereAuditoria(DataContext contexto, Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.DECLARACAOFISCALARQUIVO
                                               (DECLARACAOFISCALARQUIVOID
                                               ,OBRIGACAOFISCALAAEID
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
                                               (@DECLARACAOFISCALARQUIVOID, 
                                               @OBRIGACAOFISCALAAEID,
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

            contextQuery.Parameters.Add("@DECLARACAOFISCALARQUIVOID", SqlDbType.Int, declaracaoFiscalArquivo.DeclaracaoFiscalArquivoId);
            contextQuery.Parameters.Add("@OBRIGACAOFISCALAAEID", SqlDbType.Int, declaracaoFiscalArquivo.ObrigacaoFiscalAaeId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, declaracaoFiscalArquivo.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, declaracaoFiscalArquivo.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, declaracaoFiscalArquivo.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, declaracaoFiscalArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
