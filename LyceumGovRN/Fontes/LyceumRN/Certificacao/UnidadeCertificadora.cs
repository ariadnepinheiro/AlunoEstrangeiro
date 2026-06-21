using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{
    public class UnidadeCertificadora
    {
        public bool PossuiGrupoUnidadeCertificadoraPor(DataContext contexto, int grupoUnidadeCertificadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM CertificacaoEscolar.UNIDADECERTIFICADORA (NOLOCK)
                                    WHERE GRUPOUNIDADECERTIFICADORAID = @GRUPOUNIDADECERTIFICADORAID ";

            contextQuery.Parameters.Add("@GRUPOUNIDADECERTIFICADORAID", SqlDbType.Int, grupoUnidadeCertificadoraId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public Entidades.UnidadeCertificadora ObtemPor(int unidadeCertificadoraId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.UnidadeCertificadora histFaculdade = new Entidades.UnidadeCertificadora();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT UR.UNIDADECERTIFICADORAID, 
	                                           UR.GRUPOUNIDADECERTIFICADORAID, 
	                                           UR.TIPO, 
	                                           UR.DESCRICAO, 
	                                           UR.DESCRICAOSITE, 
	                                           UR.ENDERECO, 
	                                           UR.NUMERO, 
	                                           UR.COMPLEMENTO, 
	                                           UR.BAIRRO, 
	                                           UR.CEP, 
	                                           UR.MUNICIPIO, 
	                                           UR.TELEFONE, 
	                                           UR.ATIVO, 
	                                           UR.USUARIOID, 
	                                           UR.DATACADASTRO, 
	                                           UR.DATAALTERACAO
                                        FROM [CertificacaoEscolar].[UNIDADECERTIFICADORA] UR
                                        WHERE UR.UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID ";

                contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

                histFaculdade = contexto.TryToBindEntity<Entidades.UnidadeCertificadora>(contextQuery);

                return histFaculdade;
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

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT UNIDADECERTIFICADORAID,
                                               UC.GRUPOUNIDADECERTIFICADORAID,
                                               G.DESCRICAO                           AS GRUPO,
                                               TIPO,
                                               UC.DESCRICAO                          AS UNIDADE,
                                               UC.DESCRICAOSITE,
                                               UC.ENDERECO,
                                               UC.NUMERO,
                                               UC.COMPLEMENTO,
                                               UC.BAIRRO,
                                               UC.CEP,
                                               M.CODIGO                              AS CODMUNICIPIO,
                                               M.NOME                                AS MUNICIPIO,
                                               UC.TELEFONE,
                                               UC.ATIVO
                                        FROM   CERTIFICACAOESCOLAR.UNIDADECERTIFICADORA UC (NOLOCK)
                                               INNER JOIN CERTIFICACAOESCOLAR.GRUPOUNIDADECERTIFICADORA G (NOLOCK)
                                                       ON UC.GRUPOUNIDADECERTIFICADORAID = G.GRUPOUNIDADECERTIFICADORAID
                                               LEFT JOIN MUNICIPIO M
                                                      ON UC.MUNICIPIO = M.CODIGO
                                        ORDER  BY UNIDADE ";

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

        public ValidacaoDados Valida(Entidades.UnidadeCertificadora unidadeCertificadora, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeCertificadora == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (unidadeCertificadora.UnidadeCertificadoraId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (unidadeCertificadora.GrupoUnidadeCertificadoraId <= 0)
            {
                mensagens.Add("Campo GRUPO é obrigatório.");
            }

            if (unidadeCertificadora.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }
            else
            {
                if (unidadeCertificadora.Tipo != "Ceja" && unidadeCertificadora.Tipo != "Polo")
                {
                    mensagens.Add("Campo TIPO deve ser Ceja ou Polo.");
                }

                if (unidadeCertificadora.Descricao.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo UNIDADE é obrigatório.");
                }
                else if (unidadeCertificadora.Descricao.Length > 200)
                {
                    mensagens.Add("Campo UNIDADE deve conter no máximo 200 caracteres!");
                }

                if (unidadeCertificadora.DescricaoSite.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo DESCRIÇÃO SITE é obrigatório.");
                }
                else if (unidadeCertificadora.DescricaoSite.Length > 200)
                {
                    mensagens.Add("Campo DESCRIÇÃO SITE deve conter no máximo 200 caracteres!");
                }

                if (unidadeCertificadora.Endereco.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ENDEREÇO é obrigatório.");
                }
                else if (unidadeCertificadora.Endereco.Length > 100)
                {
                    mensagens.Add("Campo ENDEREÇO deve conter no máximo 100 caracteres!");
                }

                if (unidadeCertificadora.Numero.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NÚMERO é obrigatório.");
                }
                else if (unidadeCertificadora.Numero.Length > 50)
                {
                    mensagens.Add("Campo NÚMERO deve conter no máximo 50 caracteres!");
                }

                if (!unidadeCertificadora.Complemento.IsNullOrEmptyOrWhiteSpace())
                {
                    if (unidadeCertificadora.Complemento.Length > 100)
                    {
                        mensagens.Add("Campo COMPLEMENTO deve conter no máximo 100 caracteres!");
                    }
                }

                if (unidadeCertificadora.Bairro.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo BAIRRO é obrigatório.");
                }
                else if (unidadeCertificadora.Bairro.Length > 100)
                {
                    mensagens.Add("Campo BAIRRO deve conter no máximo 100 caracteres!");
                }

                if (unidadeCertificadora.Cep.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo CEP é obrigatório!");
                }
                else
                {
                    var cep = Utils.RetirarMascara(unidadeCertificadora.Cep);

                    if (!Validacao.ValidarCEP(cep))
                    {
                        mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                    }
                }

                if (unidadeCertificadora.Municipio.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo MUNICIPIO é obrigatório.");
                }


                if (!unidadeCertificadora.Telefone.IsNullOrEmptyOrWhiteSpace())
                {
                    var telefone = Utils.RetirarMascara(unidadeCertificadora.Telefone);

                    if (!Validacao.ValidaTelefoneComDDD(telefone) && !Validacao.ValidaCelularComDDD(telefone))
                    {
                        mensagens.Add("Campo TELEFONE OU CELULAR é inválido.");
                    }
                }

            }

            if (unidadeCertificadora.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, unidadeCertificadora.Descricao, unidadeCertificadora.UnidadeCertificadoraId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int unidadeCertificadoraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM CertificacaoEscolar.UNIDADECERTIFICADORA (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND UNIDADECERTIFICADORAID <> @UNIDADECERTIFICADORAID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.UnidadeCertificadora unidadeCertificadora)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO CertificacaoEscolar.UNIDADECERTIFICADORA
                                               (GRUPOUNIDADECERTIFICADORAID
		                                       ,DESCRICAO
                                               ,DESCRICAOSITE
                                               ,TIPO
                                               ,ENDERECO
                                               ,NUMERO
                                               ,COMPLEMENTO
                                               ,BAIRRO
                                               ,CEP
                                               ,MUNICIPIO
                                               ,TELEFONE
                                               ,ATIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@GRUPOUNIDADECERTIFICADORAID,
		                                       @DESCRICAO,
		                                       @DESCRICAOSITE,
                                               @TIPO, 
                                               @ENDERECO, 
                                               @NUMERO, 
                                               @COMPLEMENTO, 
                                               @BAIRRO, 
                                               @CEP, 
                                               @MUNICIPIO, 
                                               @TELEFONE, 
                                               @ATIVO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) 

                                SELECT IDENT_CURRENT('CertificacaoEscolar.UNIDADECERTIFICADORA')";

                contextQuery.Parameters.Add("@GRUPOUNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadora.GrupoUnidadeCertificadoraId); 
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, unidadeCertificadora.Descricao);
                contextQuery.Parameters.Add("@DESCRICAOSITE", SqlDbType.VarChar, unidadeCertificadora.DescricaoSite);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, unidadeCertificadora.Tipo);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, unidadeCertificadora.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, unidadeCertificadora.Numero);
                contextQuery.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar, unidadeCertificadora.Complemento);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, unidadeCertificadora.Bairro);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, unidadeCertificadora.Cep);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, unidadeCertificadora.Municipio);
                contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, unidadeCertificadora.Telefone);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, unidadeCertificadora.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeCertificadora.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                unidadeCertificadora.UnidadeCertificadoraId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.UnidadeCertificadora unidadeCertificadora)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE CertificacaoEscolar.UNIDADECERTIFICADORA
                                           SET GRUPOUNIDADECERTIFICADORAID= @GRUPOUNIDADECERTIFICADORAID, 
                                              TIPO = @TIPO, 
                                              DESCRICAO = @DESCRICAO, 
                                              DESCRICAOSITE = @DESCRICAOSITE, 
                                              ENDERECO = @ENDERECO, 
                                              NUMERO = @NUMERO, 
                                              COMPLEMENTO = @COMPLEMENTO, 
                                              BAIRRO = @BAIRRO, 
                                              CEP = @CEP, 
                                              MUNICIPIO = @MUNICIPIO, 
                                              TELEFONE = @TELEFONE, 
                                              ATIVO = @ATIVO,
                                              USUARIOID = @USUARIOID,
                                              DATAALTERACAO= @DATAALTERACAO
                                         WHERE UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID ";

                contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadora.UnidadeCertificadoraId);
                contextQuery.Parameters.Add("@GRUPOUNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadora.GrupoUnidadeCertificadoraId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, unidadeCertificadora.Descricao);
                contextQuery.Parameters.Add("@DESCRICAOSITE", SqlDbType.VarChar, unidadeCertificadora.DescricaoSite);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, unidadeCertificadora.Tipo);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, unidadeCertificadora.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, unidadeCertificadora.Numero);
                contextQuery.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar, unidadeCertificadora.Complemento);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, unidadeCertificadora.Bairro);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, unidadeCertificadora.Cep);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, unidadeCertificadora.Municipio);
                contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, unidadeCertificadora.Telefone);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, unidadeCertificadora.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeCertificadora.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int unidadeCertificadoraId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            UsuarioUnidadeCertificadora rnUsuarioUnidadeCertificadora = new UsuarioUnidadeCertificadora();
            EnccejaRequerimento rnEEnccejaRequerimento = new EnccejaRequerimento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeCertificadoraId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnUsuarioUnidadeCertificadora.PossuiUnidadeCertificadoraPor(contexto, unidadeCertificadoraId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para um requerimento.");
                    }

                    if (rnEEnccejaRequerimento.PossuiUnidadeCertificadoraPor(contexto, unidadeCertificadoraId))
                    {
                        mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado por um aluno.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(int unidadeCertificadoraId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE CertificacaoEscolar.UNIDADECERTIFICADORA
                            WHERE  UNIDADECERTIFICADORAID = @UNIDADECERTIFICADORAID  ";

                contextQuery.Parameters.Add("@UNIDADECERTIFICADORAID", SqlDbType.Int, unidadeCertificadoraId);

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


        public DataTable ListaUnidadePor(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT UN.UNIDADECERTIFICADORAID, 
		                                            UN.DESCRICAO AS UNIDADE,
		                                            UN.TIPO
                                            FROM [CERTIFICACAOESCOLAR].UNIDADECERTIFICADORA UN (NOLOCK)
                                               INNER JOIN [CERTIFICACAOESCOLAR].[USUARIOUNIDADECERTIFICADORA] UUN (NOLOCK)
			                                            ON UN.UNIDADECERTIFICADORAID = UUN.UNIDADECERTIFICADORAID		
                                            where UUN.USUARIO = @USUARIO ";

                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

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
    }
}
