using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class OcorrenciaEncaminhamento
    {
        public bool PossuiEncaminhamentoPor(int ocorrenciaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiEncaminhamentoPor(ctx, ocorrenciaId);
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

        public bool PossuiEncaminhamentoPor(DataContext contexto, int ocorrenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM Ocorrencias.OCORRENCIAENCAMINHAMENTO
                                        WHERE OCORRENCIAID = @OCORRENCIAID ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public DataTable ListaEncaminhamentoPor(int ocorrenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT E.OCORRENCIAENCAMINHAMENTOID, 
		                                        E.ENCAMINHAMENTO, 
		                                        A.NOMEARQUIVO, 
		                                        A.TIPOARQUIVO, 
		                                        A.OCORRENCIAENCAMINHAMENTOARQUIVOID
                                        FROM Ocorrencias.OCORRENCIAENCAMINHAMENTO E
	                                        LEFT JOIN Ocorrencias.OCORRENCIAENCAMINHAMENTOARQUIVO A 
				                                        ON E.OCORRENCIAENCAMINHAMENTOID = A.OCORRENCIAENCAMINHAMENTOID
                                        WHERE OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

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

        public ValidacaoDados Valida(Entidades.OcorrenciaEncaminhamento ocorrenciaEncaminhamento, Entidades.OcorrenciaEncaminhamentoAquivo ocorrenciaEncaminhamentoArquivo, string censo)
        {
            RN.Usuarios rnUsuarios = new Usuarios();
            Vitima rnVitima = new Vitima();
            Acusado rnAcusado = new Acusado();
            Perfil rnPerfil = new Perfil();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ocorrenciaEncaminhamento == null)
            {
                return validacaoDados;
            }

            if (ocorrenciaEncaminhamento.OcorrenciaId <= 0)
            {
                mensagens.Add("Campo OCORRENCIA é obrigatório.");
            }

            if (ocorrenciaEncaminhamento.Encaminhamento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENCAMINHAMENTO é obrigatório.");
            }
            else if (ocorrenciaEncaminhamento.Encaminhamento.Length > 5000)
            {
                mensagens.Add("Campo ENCAMINHAMENTO deve ter no máximo 5000 caracteres.");
            }

            //Verifica se tem arquivo
            if (ocorrenciaEncaminhamentoArquivo.Arquivo != null)
            {
                if (ocorrenciaEncaminhamentoArquivo.Arquivo == null || ocorrenciaEncaminhamentoArquivo.Arquivo.Count() <= 0)
                {
                    mensagens.Add("Campo ARQUIVO é obrigatório.");
                }
                else
                {
                    if (ocorrenciaEncaminhamentoArquivo.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
                    }
                    else
                    {
                        //Apenas aceitar pdf e imagem 
                        if (ocorrenciaEncaminhamentoArquivo.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                            && ocorrenciaEncaminhamentoArquivo.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                        {
                            mensagens.Add("Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                        }
                    }

                    //Verifica tamanho do arquivo - documentos com até 1 MB
                    int tamanhoByte = Buffer.ByteLength(ocorrenciaEncaminhamentoArquivo.Arquivo);
                    if (tamanhoByte > 1048576) //1MB
                    {
                        mensagens.Add("Os arquivos devem ter tamanho com até 1 MB.");
                    }

                    if (ocorrenciaEncaminhamentoArquivo.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
                    }
                    else if (ocorrenciaEncaminhamentoArquivo.NomeArquivo.Length > 500)
                    {
                        mensagens.Add("Campo NOME ARQUIVO deve conter no máximo por 500 caracteres.");
                    }
                }

                if (ocorrenciaEncaminhamentoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo USUARIO é obrigatório.");
                }
            }

            if (ocorrenciaEncaminhamento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutroEncaminhamentoCadastradoPor(contexto, ocorrenciaEncaminhamento.Encaminhamento, ocorrenciaEncaminhamento.OcorrenciaEncaminhamentoId,censo))
                    {
                        mensagens.Add("Este ENCAMINHAMENTO já foi utilizado.");
                    }

                    if (!rnUsuarios.EhPrivilegiado(contexto, ocorrenciaEncaminhamento.UsuarioId) && !rnPerfil.PossuiPerfilAdministradorRVEPor(contexto, ocorrenciaEncaminhamento.UsuarioId))
                    {
                        //verifica se o usuario tem perfil de adm
                        mensagens.Add("Apenas usuários com Perfil ADMINISTRADOR RVE podem cadastrar encaminhamentos.");
                    }

                    //Verifica se a ocorrencia possui vitima
                    if (!rnVitima.PossuiVitimaPor(contexto, ocorrenciaEncaminhamento.OcorrenciaId))
                    {
                        mensagens.Add("Esta ocorrência está incompleta pois não possui ALVO, com isso não pode ter encaminhamento cadastrado.");
                    }

                    //Verifica se a ocorrencia possui acusado
                    if (!rnAcusado.PossuiAcusadoPor(contexto, ocorrenciaEncaminhamento.OcorrenciaId))
                    {
                        mensagens.Add("Esta ocorrência está incompleta pois não possui AUTOR, com isso não pode ter encaminhamento cadastrado.");
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

        public void Insere(Entidades.OcorrenciaEncaminhamento ocorrenciaEncaminhamento, Entidades.OcorrenciaEncaminhamentoAquivo ocorrenciaEncaminhamentoArquivo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Ocorrencias.OcorrenciaEncaminhamentoArquivo rnOcorrenciaEncaminhamentoArquivo = new OcorrenciaEncaminhamentoArquivo();

            try
            {
                //Insere encaminhamento               
                this.Insere(contexto, ocorrenciaEncaminhamento);

                //Verifica se tem arquivo
                if (ocorrenciaEncaminhamentoArquivo.Arquivo != null)
                {
                    //Insere arquivo
                    ocorrenciaEncaminhamentoArquivo.OcorrenciaEncaminhamentoId = ocorrenciaEncaminhamento.OcorrenciaEncaminhamentoId;
                    rnOcorrenciaEncaminhamentoArquivo.Insere(contexto, ocorrenciaEncaminhamentoArquivo);

                    //Insere auditoria arquivo
                    rnOcorrenciaEncaminhamentoArquivo.InsereAuditoria(contexto, ocorrenciaEncaminhamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                }
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

        private void Insere(DataContext contexto, Entidades.OcorrenciaEncaminhamento ocorrenciaEncaminhamento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Ocorrencias.OCORRENCIAENCAMINHAMENTO
                                               (OCORRENCIAID
                                               ,ENCAMINHAMENTO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@OCORRENCIAID, 
                                               @ENCAMINHAMENTO, 
                                               @USUARIOID, 
                                               @DATACADASTRO,
                                               @DATAALTERACAO) 

                                    SELECT IDENT_CURRENT('Ocorrencias.OCORRENCIAENCAMINHAMENTO') ";


            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaEncaminhamento.OcorrenciaId);
            contextQuery.Parameters.Add("@ENCAMINHAMENTO", SqlDbType.VarChar, ocorrenciaEncaminhamento.Encaminhamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, ocorrenciaEncaminhamento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ocorrenciaEncaminhamento.OcorrenciaEncaminhamentoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados ValidaRemocao(int ocorrenciaEncaminhamentoId, string usuarioId)
        {
            Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ocorrenciaEncaminhamentoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (!rnUsuarios.EhPrivilegiado(contexto, usuarioId) && !rnPerfil.PossuiPerfilAdministradorRVEPor(contexto, usuarioId))
                    {
                        //verifica se o usuario tem perfil de adm
                        mensagens.Add("Apenas usuários com Perfil ADMINISTRADOR RVE podem excluir encaminhamentos.");
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

        public void Remove(int ocorrenciaEncaminhamentoId, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Ocorrencias.OcorrenciaEncaminhamentoArquivo rnOcorrenciaEncaminhamentoArquivo = new OcorrenciaEncaminhamentoArquivo();

            try
            {
                //Insere auditoria arquivo
                rnOcorrenciaEncaminhamentoArquivo.InsereAuditoria(contexto, ocorrenciaEncaminhamentoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioId);

                //Remove arquivo
                rnOcorrenciaEncaminhamentoArquivo.Remove(contexto, ocorrenciaEncaminhamentoId);

                //Remove encaminhamento
                this.Remove(contexto, ocorrenciaEncaminhamentoId);
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

        private void Remove(DataContext contexto, int ocorrenciaEncaminhamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Ocorrencias.OCORRENCIAENCAMINHAMENTO
                                    WHERE OCORRENCIAENCAMINHAMENTOID = @OCORRENCIAENCAMINHAMENTOID ";

            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOID", SqlDbType.Int, ocorrenciaEncaminhamentoId);

            contexto.ApplyModifications(contextQuery);
        }

        private bool PossuiOutroEncaminhamentoCadastradoPor(DataContext ctx, string encaminhamento, int id, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Ocorrencias.OCORRENCIAENCAMINHAMENTO (NOLOCK) OE
                                INNER JOIN Ocorrencias.OCORRENCIA O ON O.OCORRENCIAID = OE.OCORRENCIAID
                                WHERE ENCAMINHAMENTO = @ENCAMINHAMENTO
                                    AND O.CENSO = @CENSO
	                                AND OCORRENCIAENCAMINHAMENTOID <> @OCORRENCIAENCAMINHAMENTOID ";

            contextQuery.Parameters.Add("@ENCAMINHAMENTO", SqlDbType.VarChar, encaminhamento);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@OCORRENCIAENCAMINHAMENTOID", SqlDbType.Int, id);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
