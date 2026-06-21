using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class Contrato
    {
        public DataTable ListaContratoUnidadePor(string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT C.CONTRATOID, 
		                                        CS.CONTRATOSETORID, 
		                                        CO.CONTRATOOPERADORAID,
		                                        C.NUMERO,
		                                        C.DESCRICAO,
		                                        CS.DATACONTRATACAO,
		                                        CS.DATAIMPLANTACAO,
		                                        CS.DATATERMINO,
		                                        CO.OPERADORAID,
		                                        O.DESCRICAO AS OPERADORA,
												T.DESCRICAO AS TIPOLINK,
												T.TIPOLINKID,
												O.CNPJOPERADORA
                                        FROM FISCALIZACAOLINK.CONTRATO c (NOLOCK)
                                        inner join FiscalizacaoLink.CONTRATOSETOR cs (nolock) 
		                                        on c.CONTRATOID = cs.CONTRATOID
                                        inner join FiscalizacaoLink.CONTRATOOPERADORA co (nolock) 
		                                        on c.CONTRATOID = co.CONTRATOID
                                        inner join FiscalizacaoLink.OPERADORA o (nolock) 
		                                        on co.OPERADORAID = o.OPERADORAID
										inner join FiscalizacaoLink.TIPOLINK T (NOLOCK)
												ON c.TIPOLINKID = T.TIPOLINKID
                                        WHERE cs.SETORID = @SETORID
                                        ORDER BY CS.DATATERMINO DESC";

                contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);

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

        public ValidacaoDados ValidaContratoUnidade(DTOs.DadosContratoEscola dadosContratoEscola, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosContratoEscola == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (dadosContratoEscola.ContratoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO DO CONTRATO é obrigatório.");
                }

                if (dadosContratoEscola.ContratoSetorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO DO CONTRATO SETOR é obrigatório.");
                }

                if (dadosContratoEscola.ContratoOperadoraId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO DO CONTRATO OPERADORA é obrigatório.");
                }
            }

            if (dadosContratoEscola.UnidadeAdministrativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ADMINISTRATIVA é obrigatório.");
            }

            if (dadosContratoEscola.TipoLinkId <= 0)
            {
                mensagens.Add("Campo TIPO LINK é obrigatório.");
            }

            if (dadosContratoEscola.NumeroContrato.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO CONTRATO é obrigatório.");
            }
            else if (dadosContratoEscola.NumeroContrato.Length > 500)
            {
                mensagens.Add("Campo NÚMERO DO CONTRATO deve ser composto por no máximo 50 caracteres.");
            }

            if (dadosContratoEscola.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO DO CONTRATO é obrigatório.");
            }
            else if (dadosContratoEscola.Descricao.Length > 500)
            {
                mensagens.Add("Campo DESCRIÇÃO DO CONTRATO deve ser composto por no máximo 500 caracteres.");
            }

            if (dadosContratoEscola.DataContratacao == null || dadosContratoEscola.DataContratacao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DA CONTRATAÇÃO é obrigatório.");
            }

            if (dadosContratoEscola.DataImplantacao == null || dadosContratoEscola.DataImplantacao <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INSTALAÇÃO é obrigatório.");
            }
            else
            {
                if (dadosContratoEscola.DataContratacao != null && dadosContratoEscola.DataContratacao > DateTime.MinValue
                       && Convert.ToDateTime(dadosContratoEscola.DataImplantacao).Date < Convert.ToDateTime(dadosContratoEscola.DataContratacao).Date)
                {
                    mensagens.Add("A DATA INSTALAÇÃO deve ser maior ou igual a DATA DA CONTRATAÇÃO.");

                }
                if (dadosContratoEscola.DataTermino != null && dadosContratoEscola.DataTermino > DateTime.MinValue
                          && Convert.ToDateTime(dadosContratoEscola.DataTermino).Date < Convert.ToDateTime(dadosContratoEscola.DataImplantacao).Date)
                {
                    mensagens.Add("A DATA TERMINO deve ser maior ou igual a DATA INSTALAÇÃO.");
                }
            }

            if (dadosContratoEscola.DataTermino == null || dadosContratoEscola.DataTermino <= DateTime.MinValue)
            {
                mensagens.Add("Campo DATA TÉRMINO é obrigatório.");
            }

            if (dadosContratoEscola.OperadoraId <= 0)
            {
                mensagens.Add("Campo OPERADORA é obrigatório.");
            }            

            if (dadosContratoEscola.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o numero do contrato ja foi cadastrado para a unidade
                    if (this.PossuiOutroNumeroPor(contexto, dadosContratoEscola.ContratoId, dadosContratoEscola.NumeroContrato))
                    {
                        mensagens.Add("Este NÚMERO já foi cadastrado para outro contrato.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiOutroNumeroPor(DataContext contexto, int contratoId, string numero)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   FISCALIZACAOLINK.CONTRATO (NOLOCK) 
                                        WHERE  NUMERO = @NUMERO
	                                           and CONTRATOID <> @CONTRATOID ";

            contextQuery.Parameters.Add("@CONTRATOID", SqlDbType.Int, contratoId);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, numero);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTipoLinkPor(DataContext contexto, int tipoLinkId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM FISCALIZACAOLINK.CONTRATO (NOLOCK)
                                        WHERE TIPOLINKID = @TIPOLINKID";

            contextQuery.Parameters.Add("@TIPOLINKID", SqlDbType.VarChar, tipoLinkId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiContratoPor(DataContext ctx, int contratoSetorId, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                FROM FiscalizacaoLink.CONTRATOSETOR (NOLOCK)
                                WHERE CONTRATOSETORID = @CONTRATOSETORID
									AND ((DATATERMINO IS NOT NULL AND @DATA BETWEEN ISNULL(DATACONTRATACAO, DATAIMPLANTACAO)
                                                   AND DATATERMINO) 
										OR (DATATERMINO IS NULL AND @DATA > ISNULL(DATACONTRATACAO, DATAIMPLANTACAO))) ";

            contextQuery.Parameters.Add("@CONTRATOSETORID", SqlDbType.Int, contratoSetorId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void InsereContratoUnidade(DTOs.DadosContratoEscola dadosContratoEscola)
        {
            ContratoSetor rnContratoSetor = new ContratoSetor();
            ContratoOperadora rnContratoOperadora = new ContratoOperadora();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.Contrato contrato = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Contrato();
            Entidades.ContratoOperadora contratoOperadora = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ContratoOperadora();
            Entidades.ContratoSetor contratoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ContratoSetor();

            try
            {
                //Monta entidade Contrato
                contrato.Numero = dadosContratoEscola.NumeroContrato;
                contrato.Descricao = dadosContratoEscola.Descricao;
                contrato.DataTermino = dadosContratoEscola.DataTermino;
                contrato.UsuarioId = dadosContratoEscola.UsuarioId;
                contrato.TipoLinkId = dadosContratoEscola.TipoLinkId;

                //Insere Contrato
                this.Insere(contexto, contrato);

                //Atualiza com id gerado
                dadosContratoEscola.ContratoId = contrato.ContratoId;

                //Monta enidade contrato Operadora
                contratoOperadora.ContratoId = dadosContratoEscola.ContratoId;
                contratoOperadora.OperadoraId = dadosContratoEscola.OperadoraId;
                contratoOperadora.UsuarioId = dadosContratoEscola.UsuarioId;

                //Insere ContratoOperadora
                rnContratoOperadora.Insere(contexto, contratoOperadora);

                //Monta Entidade Contrato setor
                contratoSetor.ContratoId = dadosContratoEscola.ContratoId;
                contratoSetor.SetorId = dadosContratoEscola.UnidadeAdministrativa;
                contratoSetor.DataContratacao = dadosContratoEscola.DataContratacao;
                contratoSetor.DataImplantacao = dadosContratoEscola.DataImplantacao;
                contratoSetor.DataTermino = dadosContratoEscola.DataTermino;
                contratoSetor.UsuarioId = dadosContratoEscola.UsuarioId;

                //Insere Contratosetor
                rnContratoSetor.Insere(contexto, contratoSetor);
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

        public void AtualizaContratoUnidade(DTOs.DadosContratoEscola dadosContratoEscola)
        {
            ContratoSetor rnContratoSetor = new ContratoSetor();
            ContratoOperadora rnContratoOperadora = new ContratoOperadora();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.Contrato contrato = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Contrato();
            Entidades.ContratoOperadora contratoOperadora = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ContratoOperadora();
            Entidades.ContratoSetor contratoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.ContratoSetor();

            try
            {
                //Monta entidade Contrato
                contrato.ContratoId = dadosContratoEscola.ContratoId;
                contrato.Numero = dadosContratoEscola.NumeroContrato;
                contrato.Descricao = dadosContratoEscola.Descricao;
                contrato.DataTermino = dadosContratoEscola.DataTermino;
                contrato.UsuarioId = dadosContratoEscola.UsuarioId;
                contrato.TipoLinkId = dadosContratoEscola.TipoLinkId;

                //Atualiza Contrato
                this.Atualiza(contexto, contrato);

                //Monta enidade contrato Operadora
                contratoOperadora.ContratoOperadoraId = dadosContratoEscola.ContratoOperadoraId;
                contratoOperadora.ContratoId = dadosContratoEscola.ContratoId;
                contratoOperadora.OperadoraId = dadosContratoEscola.OperadoraId;
                contratoOperadora.UsuarioId = dadosContratoEscola.UsuarioId;

                //Atualiza ContratoOperadora por contrato
                rnContratoOperadora.Atualiza(contexto, contratoOperadora);

                //Monta Entidade Contrato setor
                contratoSetor.ContratoSetorId = dadosContratoEscola.ContratoSetorId;
                contratoSetor.ContratoId = dadosContratoEscola.ContratoId;
                contratoSetor.SetorId = dadosContratoEscola.UnidadeAdministrativa;
                contratoSetor.DataContratacao = dadosContratoEscola.DataContratacao;
                contratoSetor.DataImplantacao = dadosContratoEscola.DataImplantacao;
                contratoSetor.DataTermino = dadosContratoEscola.DataTermino;
                contratoSetor.UsuarioId = dadosContratoEscola.UsuarioId;

                //Atualiza Contratosetor por contrato
                rnContratoSetor.Atualiza(contexto, contratoSetor);
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

        private void Insere(DataContext contexto, Entidades.Contrato contrato)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO FiscalizacaoLink.CONTRATO
                                               (NUMERO
                                               ,TIPOLINKID
                                               ,DESCRICAO
                                               ,DATATERMINO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@NUMERO, 
                                               @TIPOLINKID,
                                               @DESCRICAO, 
                                               @DATATERMINO, 
                                               @USUARIOID, 
                                               @DATACADASTRO,
                                               @DATAALTERACAO)
                                     
                                        SELECT IDENT_CURRENT('FiscalizacaoLink.CONTRATO') ";

            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, contrato.Numero);
            contextQuery.Parameters.Add("@TIPOLINKID", SqlDbType.Int, contrato.TipoLinkId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, contrato.Descricao);            
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contrato.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (contrato.DataTermino == null || contrato.DataTermino <= DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, Convert.ToDateTime(contrato.DataTermino).Date);
            }

            contrato.ContratoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.Contrato contrato)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FiscalizacaoLink.CONTRATO
                                           SET NUMERO = @NUMERO,
                                              TIPOLINKID = @TIPOLINKID,
                                              DESCRICAO = @DESCRICAO, 
                                              DATATERMINO = @DATATERMINO, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE CONTRATOID = @CONTRATOID ";

            contextQuery.Parameters.Add("@CONTRATOID", SqlDbType.Int, contrato.ContratoId);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, contrato.Numero);
            contextQuery.Parameters.Add("@TIPOLINKID", SqlDbType.Int, contrato.TipoLinkId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, contrato.Descricao);           
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contrato.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (contrato.DataTermino == null || contrato.DataTermino <= DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, Convert.ToDateTime(contrato.DataTermino).Date);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public RN.FiscalizacaoLink.Entidades.Contrato ObtemPor(DataContext contexto, int contratoId)
        {
            RN.FiscalizacaoLink.Entidades.Contrato entidade = new RN.FiscalizacaoLink.Entidades.Contrato();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                            FROM   FISCALIZACAOLINK.CONTRATO (NOLOCK)
                                            WHERE  CONTRATOID = @CONTRATOID  ";

            contextQuery.Parameters.Add("@CONTRATOID", SqlDbType.Int, contratoId);

            entidade = contexto.TryToBindEntity<FiscalizacaoLink.Entidades.Contrato>(contextQuery);

            return entidade;
        }
    }
}