using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class OperacaoDocumentos : IOperacaoDocumentos
    {
        public DataTable ListaPor(int operacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT o.OPERACAODOCUMENTOSID,
	                                            o.OPERACAOID,
                                                d.DESCRICAO as  DOCUMENTOSNECESSARIOSOPERACOESID,
	                                            o.NOMEARQUIVO,
                                                o.DATAENVIO,
	                                            o.TIPOARQUIVO,
	                                            CASE
		                                            WHEN o.TIPOARQUIVO = 'application/pdf' then 'PDF'
		                                            WHEN o.TIPOARQUIVO = 'image/jpeg' then 'IMAGEM'		
		                                            ELSE  ''
	                                            END TIPO,
	                                            o.DATACADASTRO
                                            FROM PrestacaoContas.OPERACAODOCUMENTOS o
											inner join PrestacaoContas.DOCUMENTOSNECESSARIOSOPERACOES d on o.DOCUMENTOSNECESSARIOSOPERACOESID = d.DOCUMENTOSNECESSARIOSOPERACOESID
                                            WHERE o.OPERACAOID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, operacaoId);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaExigenciasPor(int PeriodoReferencia, string status, string censo, string plano, string operacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT v.OPERACAOID , o.OPERACAOEXIGENCIAID,     ,v.CENSO      ,v.nome_comp      ,v.PERIODOREFERENCIAID      ,v.PLANOTRABALHOID  ,o.APROVADO  
                                               ,v.tipo      ,v.plano      ,v.DATACADASTRO      ,v.status      ,v.VALOR,o.TIPOEXIGENCIAOPERACAOID , o.NOTAEXPLICATIVA, o.JUSTIFICATIVA	  ,e.NOMEARQUIVO,count(o.OPERACAOID) as qtd  
                                         FROM PrestacaoContas.VW_OPERACAO v   
                                         left join PrestacaoContas.OPERACAOEXIGENCIA o on v.OPERACAOID = o.OPERACAOID  
                                         left join PrestacaoContas.OPERACAOEXIGENCIAARQUIVO e on o.OPERACAOEXIGENCIAID = e.OPERACAOEXIGENCIAID
                                         where  v.PERIODOREFERENCIAID= @PERIODOREFERENCIAID and 
                                               (v.CODSTATUS = @STATUS OR @STATUS =9) and
                                               (v.CENSO = @CENSO OR @CENSO= 99999) AND
                                               (v.PLANOTRABALHOID = @PLANO OR @PLANO= 99999) AND
                                               (v.tipo = @tipo OR @tipo= '9')
                                         group by  v.OPERACAOID      ,o.OPERACAOEXIGENCIAID, v.CENSO      ,v.nome_comp      ,v.PERIODOREFERENCIAID      ,v.PLANOTRABALHOID ,o.APROVADO     ,v.tipo      ,v.plano      ,v.DATACADASTRO , o.NOTAEXPLICATIVA , o.JUSTIFICATIVA     ,v.status      ,v.VALOR,e.NOMEARQUIVO,o.TIPOEXIGENCIAOPERACAOID ";

                var auxoperacao = "";
                if (operacao.IsNullOrEmptyOrWhiteSpace())
                    auxoperacao = "9";
                if (operacao == "C")
                    auxoperacao = "Crédito";
                else
                    auxoperacao = "Débito";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferencia);
                contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo == "" ? "99999" : censo);
                contextQuery.Parameters.Add("@PLANO", SqlDbType.Int, plano == "" ? "99999" : plano);
                contextQuery.Parameters.Add("@tipo", SqlDbType.VarChar, auxoperacao);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaExigenciasGridAnaliseAprRepPor(int PeriodoReferencia, string status, string censo, string plano, string operacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {

                if (status == "9")
                {
                    status = "v.CODSTATUS BETWEEN 3 and 4 and";
                }
                else { status = "v.CODSTATUS =" + status + " and "; }

                contextQuery.Command = @"SELECT R.unidade_ens as CENSO, v.OPERACAOID     ,v.nome_comp      ,v.PERIODOREFERENCIAID    ,v.plano    
                                               ,v.tipo       ,v.DATACADASTRO      ,v.status      ,v.VALOR ,count(o.OPERACAOID) as qtd  
                                         from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL R
                                         INNER JOIN PrestacaoContas.VW_OPERACAO v ON V.CENSO = R.unidade_ens
                                         left join PrestacaoContas.OPERACAOEXIGENCIA o on v.OPERACAOID = o.OPERACAOID  
                                         left join PrestacaoContas.OPERACAOEXIGENCIAARQUIVO e on o.OPERACAOEXIGENCIAID = e.OPERACAOEXIGENCIAID
                                         where  v.PERIODOREFERENCIAID= @PERIODOREFERENCIAID and 
                                               " + status + @"
                                               (R.unidade_ens = @CENSO OR @CENSO= 99999) AND
                                               (v.PLANOTRABALHOID = @PLANO OR @PLANO= 99999) AND
                                               (v.OPERACAOID = @operacao OR @operacao= '99999999')
                                         group by v.OPERACAOID      ,  R.unidade_ens  ,v.nome_comp      ,v.PERIODOREFERENCIAID   ,v.plano       ,v.tipo       ,v.DATACADASTRO ,v.VALOR   ,v.status     ";




                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferencia);
                //contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo == "" ? "99999" : censo);
                contextQuery.Parameters.Add("@PLANO", SqlDbType.Int, plano == "" ? "99999" : plano);
                contextQuery.Parameters.Add("@operacao", SqlDbType.VarChar, operacao == "" ? "99999999" : operacao);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaExigenciasGridAnalisePor(int PeriodoReferencia, string status, string censo, string plano, string operacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                var existentes = "";
                if (status == "8")
                {
                    existentes = "   having count(o.OPERACAOID) <> 0 ";
                    status = "9";
                }

                contextQuery.Command = @"SELECT R.unidade_ens as CENSO, v.OPERACAOID     ,v.nome_comp      ,v.PERIODOREFERENCIAID    ,v.plano    
                                               ,v.tipo       ,v.DATACADASTRO      ,v.status      ,v.VALOR ,count(o.OPERACAOID) as qtd  
                                         from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL R
                                         INNER JOIN PrestacaoContas.VW_OPERACAO v ON V.CENSO = R.unidade_ens
                                         left join PrestacaoContas.OPERACAOEXIGENCIA o on v.OPERACAOID = o.OPERACAOID  
                                         left join PrestacaoContas.OPERACAOEXIGENCIAARQUIVO e on o.OPERACAOEXIGENCIAID = e.OPERACAOEXIGENCIAID
                                         where  v.PERIODOREFERENCIAID= @PERIODOREFERENCIAID and 
                                               (v.CODSTATUS = @STATUS OR @STATUS =9) and
                                               (R.unidade_ens = @CENSO OR @CENSO= 99999) AND
                                               (v.PLANOTRABALHOID = @PLANO OR @PLANO= 99999) AND
                                               (v.OPERACAOID = @operacao OR @operacao= '99999999')
                                         group by v.OPERACAOID      ,  R.unidade_ens  ,v.nome_comp      ,v.PERIODOREFERENCIAID   ,v.plano       ,v.tipo       ,v.DATACADASTRO ,v.VALOR   ,v.status     " + @existentes;




                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferencia);
                contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo == "" ? "99999" : censo);
                contextQuery.Parameters.Add("@PLANO", SqlDbType.Int, plano == "" ? "99999" : plano);
                contextQuery.Parameters.Add("@operacao", SqlDbType.VarChar, operacao == "" ? "99999999" : operacao);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }
        
        public DataTable ListaExigenciasGridPor(int PeriodoReferencia, string status, string censo, string plano, string operacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                var existentes = "";
                if (status == "8")
                {
                    existentes = "   having count(o.OPERACAOID) <> 0 ";
                    status = "9";
                }

                contextQuery.Command = @"SELECT R.unidade_ens as CENSO, v.OPERACAOID     ,v.nome_comp      ,v.PERIODOREFERENCIAID    ,v.plano    
                                               ,v.tipo       ,v.DATACADASTRO      ,v.status      ,v.VALOR ,count(o.OPERACAOID) as qtd  
                                         from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL R
                                         INNER JOIN PrestacaoContas.VW_OPERACAO v ON V.CENSO = R.unidade_ens
                                         left join PrestacaoContas.OPERACAOEXIGENCIA o on v.OPERACAOID = o.OPERACAOID  
                                         left join PrestacaoContas.OPERACAOEXIGENCIAARQUIVO e on o.OPERACAOEXIGENCIAID = e.OPERACAOEXIGENCIAID
                                         where  v.PERIODOREFERENCIAID= @PERIODOREFERENCIAID and 
                                               (v.CODSTATUS = @STATUS OR @STATUS =9) and
                                               (R.unidade_ens = @CENSO OR @CENSO= 99999) AND
                                               (v.PLANOTRABALHOID = @PLANO OR @PLANO= 99999) AND
                                               (v.tipo = @tipo OR @tipo= '9')
                                         group by v.OPERACAOID      ,  R.unidade_ens  ,v.nome_comp      ,v.PERIODOREFERENCIAID   ,v.plano       ,v.tipo       ,v.DATACADASTRO ,v.VALOR   ,v.status     " + @existentes;

                var auxoperacao = "";
                if (operacao.IsNullOrEmptyOrWhiteSpace())
                    auxoperacao = "9";
                if (operacao == "C")
                    auxoperacao = "Crédito";
                if (operacao == "D")
                    auxoperacao = "Débito";


                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PeriodoReferencia);
                contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo == "" ? "99999" : censo);
                contextQuery.Parameters.Add("@PLANO", SqlDbType.Int, plano == "" ? "99999" : plano);
                contextQuery.Parameters.Add("@tipo", SqlDbType.VarChar, auxoperacao);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }
        
        public DataTable ListaExigenciasPor(int operacaoid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT v.OPERACAOID      , o.OPERACAOEXIGENCIAID, v.CENSO      ,v.nome_comp      ,v.PERIODOREFERENCIAID      ,v.PLANOTRABALHOID  ,o.APROVADO,e.TIPOARQUIVO,e.CHAVEARQUIVO  ,e.OPERACAOEXIGENCIAARQUIVOID
                                               ,v.tipo      ,v.plano      ,v.DATACADASTRO      ,v.status      ,v.VALOR,o.TIPOEXIGENCIAOPERACAOID , o.NOTAEXPLICATIVA, o.JUSTIFICATIVA	  ,e.NOMEARQUIVO,count(o.OPERACAOID) as qtd  
                                         FROM PrestacaoContas.VW_OPERACAO v   
                                         left join PrestacaoContas.OPERACAOEXIGENCIA o on v.OPERACAOID = o.OPERACAOID  
                                         left join PrestacaoContas.OPERACAOEXIGENCIAARQUIVO e on o.OPERACAOEXIGENCIAID = e.OPERACAOEXIGENCIAID
                                         where  v.operacaoid= @OPERACAOID
                                         group by  v.OPERACAOID      ,o.OPERACAOEXIGENCIAID,v.CENSO      ,v.nome_comp      ,v.PERIODOREFERENCIAID      ,v.PLANOTRABALHOID ,o.APROVADO     ,v.tipo,e.TIPOARQUIVO,e.CHAVEARQUIVO,e.OPERACAOEXIGENCIAARQUIVOID         ,v.plano      ,v.DATACADASTRO , o.NOTAEXPLICATIVA , o.JUSTIFICATIVA     ,v.status      ,v.VALOR,e.NOMEARQUIVO,o.TIPOEXIGENCIAOPERACAOID ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoid);
                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaExigenciasPorId(int operacaoid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                                         SELECT o.OPERACAOEXIGENCIAID, iif (o.APROVADO  is null, 'Não analisado', iif (o.APROVADO = 0, 'Reprovado', 'Aprovado' )  ) as APROVADO,e.TIPOARQUIVO,e.CHAVEARQUIVO  ,e.OPERACAOEXIGENCIAARQUIVOID
                                               , o.TIPOEXIGENCIAOPERACAOID , o.NOTAEXPLICATIVA, o.JUSTIFICATIVA	  ,e.NOMEARQUIVO
                                         from PrestacaoContas.OPERACAOEXIGENCIA o 
                                         left join PrestacaoContas.OPERACAOEXIGENCIAARQUIVO e on o.OPERACAOEXIGENCIAID = e.OPERACAOEXIGENCIAID
                                         where  o.operacaoid=  @OPERACAOID
                                        ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoid);


                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public DataTable ListaDocumentosPorId(int operacaoid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                                        SELECT  o.OPERACAOID,  o.STATUS
                                          FROM PrestacaoContas.OPERACAO o
                                          inner join PrestacaoContas.OPERACAODOCUMENTOS d on o.OPERACAOID = d.OPERACAOID
                                           where  d.OPERACAODOCUMENTOSID=  @OPERACAOID
                                        ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoid);


                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public ValidacaoDados Valida(Entidades.OperacaoDocumentos operacaoDocumento)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (operacaoDocumento == null)
            {
                return validacaoDados;
            }

            if (operacaoDocumento.OperacaoId <= 0)
            {
                mensagens.Add("É Necessário salvar a operação antes de incluir o documento.");
            }

            if (operacaoDocumento.Arquivo == null || operacaoDocumento.Arquivo.Count() <= 0)
            {
                mensagens.Add("Campo ARQUIVO é obrigatório.");
            }
            else
            {
                if (operacaoDocumento.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
                }
                else
                {
                    //Apenas aceitar pdf e imagem 
                    if (operacaoDocumento.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                    {
                        mensagens.Add("Apenas serão aceitos arquivos do tipo .pdf .");
                    }
                }

                //Verifica tamanho do arquivo - documentos com até 1 MB
                int tamanhoByte = Buffer.ByteLength(operacaoDocumento.Arquivo);
                if (tamanhoByte > 1048576) //1MB
                {
                    mensagens.Add("Os arquivos devem ter tamanho com até 1 MB.");
                }

                if (operacaoDocumento.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
                }
                else if (operacaoDocumento.NomeArquivo.Length > 500)
                {
                    mensagens.Add("Campo NOME ARQUIVO deve conter no máximo por 500 caracteres.");
                }
            }

            if (operacaoDocumento.DocumentosNecessariosOperacoesId <= 0)
            {
                mensagens.Add("Campo DOCUMENTO NECESSÁRIO é obrigatório.");
            }

            if (operacaoDocumento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaEnvioAnalise(int operacaoDocumento)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int arquivo = 0;

            contextQuery.Command = @" SELECT count( [OPERACAOID]) as total
                                          FROM [LYCEUM].[PrestacaoContas].[OPERACAODOCUMENTOS]
                                          where  OPERACAOID = @OperacaoDocumentoID ";

            contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, operacaoDocumento);

            reader = contexto.GetDataReader(contextQuery);

            while (reader.Read())
            {
                arquivo = (int)reader["total"];
            }


            // if (VerificaEnvioSEI(Convert.ToInt32(operacaoDocumento)))
            //  {
            //      mensagens.Add("Formulário não pode ser alterado, pois o Formulário SEI já foi gerado");
            //  }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }
            return validacaoDados;
        }

        public DataTable ObtemOperacaoPor(int eventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();
                query.Command = @"
                    select * from [LYCEUM].[PrestacaoContas].[OPERACAO]
                    where OPERACAOID = @OPERACAOID    ";

                query.Parameters.Add("@OPERACAOID", SqlDbType.Int, eventoId);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerificaEnvioSEI(int operacaoId)
        {
            var operacao = ObtemOperacaoPor(operacaoId);

            var row = operacao.Rows[0];
            var periodoreferencia = Convert.ToInt32(row["PERIODOREFERENCIAID"]);
            var censo = Convert.ToString(row["CENSO"]);

            return VerificaEnvioSEIPorEscolaPeriodo(censo, periodoreferencia);
        }


        public bool VerificaEnvioSEIPorEscolaPeriodo(string censo, int periodoreferencia)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"SELECT count([IMPORTACAOSEIID]) as existe
                                     FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                     where [CENSO] = @CENSO and
		                                    [PERIODOREFERENCIAID] = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }
        public void EnviaAnalise(int valor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere arquivo
                this.EnviaAnalise(ctx, valor);

                //Insere auditoria arquivo
                //  this.InsereAuditoria(ctx, OperacaoDocumento, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        public void Insere(Entidades.OperacaoDocumentos OperacaoDocumento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere arquivo
                this.Insere(ctx, OperacaoDocumento);

                //Insere auditoria arquivo
                //  this.InsereAuditoria(ctx, OperacaoDocumento, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        private void Insere(DataContext contexto, Entidades.OperacaoDocumentos operacaoDocumento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [PrestacaoContas].[OperacaoDocumentos]
                                           ([OPERACAOID]
                                           ,[CHAVEARQUIVO]
                                           ,DOCUMENTOSNECESSARIOSOPERACOESID
                                           ,[ARQUIVO]
                                           ,[TIPOARQUIVO]
                                           ,[NOMEARQUIVO]
                                           ,[USUARIOID]
                                           ,[DATAENVIO]
                                           ,[DATACADASTRO]
                                           ,[DATAALTERACAO])
                                     VALUES
                                           (@OCORRENCIAID 
                                           ,NEWID()
                                           ,@DOCUMENTOSNECESSARIOSOPERACOESID
                                           ,@ARQUIVO
                                           ,@TIPOARQUIVO
                                           ,@NOMEARQUIVO
                                           ,@USUARIOID
                                           ,@DATAENVIO
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO    )

                         SELECT IDENT_CURRENT('PrestacaoContas.OperacaoDocumentos') ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, operacaoDocumento.OperacaoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, operacaoDocumento.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, operacaoDocumento.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, operacaoDocumento.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, operacaoDocumento.UsuarioId);
            contextQuery.Parameters.Add("@DOCUMENTOSNECESSARIOSOPERACOESID", SqlDbType.Int, operacaoDocumento.DocumentosNecessariosOperacoesId);
            contextQuery.Parameters.Add("@DATAENVIO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            operacaoDocumento.OperacaoDocumentosId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void InsereAuditoria(DataContext contexto, Entidades.OperacaoDocumentos OperacaoDocumento, string operacao, string estacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Poseidon.PrestacaoContas.OperacaoDocumento
                                               (OperacaoDocumentoID
                                               ,OPERACAOID
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
                                               (@OperacaoDocumentoID, 
                                               @OCORRENCIAID,
                                               NEWID(), 
                                               @ARQUIVO,
                                               @TIPOARQUIVO, 
                                               @NOMEARQUIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO,
                                               @DATAAUDITORIA,
                                               @OPERACAO,
                                               @ESTACAO) ";

            contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, OperacaoDocumento.OperacaoDocumentosId);
            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, OperacaoDocumento.OperacaoId);
            contextQuery.Parameters.Add("@ARQUIVO", SqlDbType.VarBinary, OperacaoDocumento.Arquivo);
            contextQuery.Parameters.Add("@TIPOARQUIVO", SqlDbType.VarChar, OperacaoDocumento.TipoArquivo);
            contextQuery.Parameters.Add("@NOMEARQUIVO", SqlDbType.VarChar, OperacaoDocumento.NomeArquivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, OperacaoDocumento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereAuditoria(DataContext contexto, int OperacaoDocumentoId, string operacao, string estacao, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.PrestacaoContas.OperacaoDocumento 
                                                    (OPERACAODOCUMENTOSID, 
                                                     OPERACAOID, 
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
                                        SELECT OPERACAODOCUMENTOSID, 
                                               OPERACAOID, 
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
                                        FROM   LYCEUM.PrestacaoContas.OperacaoDocumento 
                                        WHERE  OperacaoDocumentoID = @OperacaoDocumentoID  ";

            contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, OperacaoDocumentoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereAuditoriaPorOperacao(DataContext contexto, int operacaoId, string operacao, string estacao, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO POSEIDON.PrestacaoContas.OperacaoDocumentos 
                                                    (OPERACAODOCUMENTOSID, 
                                                     OPERACAOID, 
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
                                        SELECT OPERACAODOCUMENTOSID, 
                                               OPERACAOID, 
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
                                        FROM   LYCEUM.PrestacaoContas.OperacaoDocumentos 
                                        WHERE  OPERACAOID = @OPERACAOID ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);
            contextQuery.Parameters.Add("@DATAAUDITORIA", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@OPERACAO", SqlDbType.VarChar, operacao);
            contextQuery.Parameters.Add("@ESTACAO", SqlDbType.VarChar, estacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveDocumentos(DataContext contexto, int operacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" delete [LYCEUM].[PrestacaoContas].[OPERACAODOCUMENTOS]
                          where OPERACAOID = @OPERACAOID ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int arquivoId)
        {
            List<string> mensagens = new List<string>();

            Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (arquivoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            //  if(usuarioId.IsNullOrEmptyOrWhiteSpace())
            //  {
            //      mensagens.Add("Campo USUARIO é obrigatório.");
            //  }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    var ocorrenciaId = this.ListaDocumentosPorId(arquivoId);

                    //Verificar se a operacao possui encaminhamento, caso seja alteracao
                    if (ocorrenciaId.Rows[0].ItemArray[1].ToString() == "3" ||
                        ocorrenciaId.Rows[0].ItemArray[1].ToString() == "4")
                    {
                        //Caso exita encaminhamento verifica se o usuario tem perfil de adm                        
                        //       if (!rnUsuarios.EhPrivilegiado(contexto, usuarioId) && !rnPerfil.PossuiPerfilAdministradorRVEPor(contexto, usuarioId))
                        //       {
                        //verifica se o usuario tem perfil de adm
                        mensagens.Add("Este registro não pode ser alterado pois já foram Aprovados/Reprovados.");
                        //       }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaArquivo(int operacaoId, int documentoid)
        {
            List<string> mensagens = new List<string>();
            //  OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new OcorrenciaEncaminhamento();
            Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (operacaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (documentoid <= 0)
            {
                mensagens.Add("Campo Documento é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    int ocorrenciaId = this.ObtemArquivoIdPor(contexto, operacaoId, documentoid);

                    if (ocorrenciaId != 0)
                    {
                        mensagens.Add("Este registro não pode ser alterado pois já existem documentos cadastrados.");
                    }
                    else { validacaoDados.Valido = true; }

                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Remove(int arquivoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere auditoria arquivo
                //  this.InsereAuditoria(ctx, arquivoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioId);

                //Remove arquivo
                this.Remove(ctx, arquivoId);
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

        private void Remove(DataContext contexto, int arquivoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PrestacaoContas.OPERACAODOCUMENTOS                                     
                                        WHERE  OPERACAODOCUMENTOSID = @OperacaoDocumentoID  ";

            contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, arquivoId);

            contexto.ApplyModifications(contextQuery);
        }

        private void EnviaAnalise(DataContext contexto, int arquivoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   update [LYCEUM].[PrestacaoContas].[OPERACAO]
                                        set status = 1
                                        where  OPERACAOID  = @OperacaoDocumentoID  ";

            contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, arquivoId);

            contexto.ApplyModifications(contextQuery);
        }

        public byte[] ObtemArquivoPor(int arquivoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" 	 SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM   PrestacaoContas.OPERACAODOCUMENTOS (NOLOCK) 
											where OPERACAODOCUMENTOSID = @OperacaoDocumentoID ";

                contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, arquivoId);

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

        public int ObtemArquivoIdPor(DataContext contexto, int operacaoId, int documentoId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int id = 0;

            try
            {
                contextQuery.Command = @" 	 SELECT OPERACAODOCUMENTOSID 
                                            FROM   PrestacaoContas.OperacaoDocumentos (NOLOCK) 
											where OPERACAOID                       = @OperacaoID and
                                                  DOCUMENTOSNECESSARIOSOPERACOESID = @OperacaoDocumentoID";

                contextQuery.Parameters.Add("@OperacaoID", SqlDbType.Int, operacaoId);
                contextQuery.Parameters.Add("@OperacaoDocumentoID", SqlDbType.Int, documentoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["OPERACAODOCUMENTOSID"]);
                }

                return id;
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
    }

    public interface IOperacaoDocumentos
    {
        byte[] ObtemArquivoPor(int id);
    }
}
