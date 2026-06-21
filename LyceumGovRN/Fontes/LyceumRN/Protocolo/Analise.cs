using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Protocolo
{
    public class Analise
    {
        public DataTable ListaAnalisePor(int protocoloPrestacaoId)
        { 
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable resultado = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT  A.ANALISEID, 
                                                 CONVERT(VARCHAR(MAX),A.DESCRICAO) AS DESCRICAO, 
                                                A.SITUACAOPROTOCOLOID, 
                                                P.NUMEROFOLHAS,
                                                SUBSTRING(A.DESCRICAO,1,100) AS DESCRICAOPARCIAL, 
                                                S.DESCRICAO AS SITUACAO, 
		                                        a.DATASITUACAO,
                                                P.PROGRAMAPROTOCOLOID,
                                                PP.DESCRICAO AS PROGRAMA,
                                                A.USUARIOSISTEMA,
			                                    U.NOME AS NOMEUSUARIOSISTEMA,
                                                A.USUARIOANALISADOR,
			                                    FA.NOME_COMPL AS NOMEUSUARIOANALISADOR,
			                                    A.USUARIOREVISOR,
			                                    FR.NOME_COMPL AS NOMEUSUARIOREVISOR,
                                                P.OBSERVACAO            
                                    FROM       PROTOCOLO.PROTOCOLOPRESTACAO P (NOLOCK) 
                                    INNER JOIN PROTOCOLO.ANALISE A (NOLOCK) 
		                                    ON P.PROTOCOLOPRESTACAOID = A.PROTOCOLOPRESTACAOID 
                                    INNER JOIN PROTOCOLO.SITUACAOPROTOCOLO S (NOLOCK) 
		                                    ON A.SITUACAOPROTOCOLOID = S.SITUACAOPROTOCOLOID 
                                    LEFT JOIN  DBO.VW_FUNCIONARIOS FA (NOLOCK) 
		                                    ON A.USUARIOANALISADOR = FA.MATRICULA
                                    LEFT JOIN  DBO.VW_FUNCIONARIOS FR (NOLOCK) 
		                                    ON A.USUARIOREVISOR = FR.MATRICULA
                                    LEFT JOIN  HADES.DBO.HD_USUARIO U (NOLOCK) 
		                                    ON A.USUARIOSISTEMA = U.USUARIO 
                                    LEFT JOIN PROTOCOLO.PROGRAMAPROTOCOLO PP  (NOLOCK) 
                                            ON PP.PROGRAMAPROTOCOLOID=P.PROGRAMAPROTOCOLOID
                                    WHERE A.PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID
                                    ORDER BY A.DATASITUACAO DESC ";

                contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", protocoloPrestacaoId);

                resultado = contexto.GetDataTable(contextQuery);
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

            return resultado;
        }

        public ValidacaoDados Valida(Entidades.Analise analise, int? numeroFolhas, string observacao, DateTime dataProcesso, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new ProtocoloPrestacao();
            RN.Protocolo.Entidades.ProtocoloPrestacao protocoloPrestacao = new Techne.Lyceum.RN.Protocolo.Entidades.ProtocoloPrestacao();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (analise == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (analise.AnaliseId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }            

            if (numeroFolhas <= 0)
            {
                mensagens.Add("Campo NÚMERO DE FOLHAS é obrigatório.");
            }

            if (!observacao.IsNullOrEmptyOrWhiteSpace())
            {
                if (observacao.Length > 50)
                {
                    mensagens.Add("Campo OBSERVAÇÃO deve ter no máximo 50 caracteres.");
                }
            }

            if (analise.ProtocoloPrestacaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA PRESTAÇÃO é obrigatório.");
            }

            if (dataProcesso == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO PROCESSO é obrigatório.");
            }

            if (analise.SituacaoProtocoloId <= 0)
            {
                mensagens.Add("Campo SITUAÇÃO é obrigatório.");
            }

            if (analise.DataSituacao == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DA SITUAÇÃO é obrigatório.");
            }
            else
            {
                if (analise.DataSituacao < dataProcesso)
                {
                    mensagens.Add("A DATA DA SITUAÇÃO deve ser maior ou igual a DATA DO PROCESSO.");
                }

                if (analise.DataSituacao > DateTime.Now)
                {
                    mensagens.Add("Campo DATA DA SITUAÇÃO não pode ser maior que a data atual.");
                }
            }
            
            if (!analise.Descricao.IsNullOrEmptyOrWhiteSpace() && analise.Descricao.Length > 65535)
            {
                mensagens.Add("Campo DESCRIÇÃO DA SITUAÇÃO deve conter no máximo 65535 caracteres.");
            }

            if (analise.UsuarioAnalisador.IsNullOrEmptyOrWhiteSpace()) 
            {
                mensagens.Add("Campo USUÁRIO ANALISADOR é obrigatório.");
            }

            if (analise.UsuarioRevisor.IsNullOrEmptyOrWhiteSpace()) 
            {
                mensagens.Add("Campo USUÁRIO REVISOR é obrigatório.");
            }

            if (analise.UsuarioSistema.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Carrega prestação
                    protocoloPrestacao = rnProtocoloPrestacao.ObtemPor(contexto, analise.ProtocoloPrestacaoId);
                    
                    //Verifica se já existe mesma analise cadastrada
                    if (this.PossuiOutraAnaliseCadastradaPor(contexto, analise.DataSituacao, analise.SituacaoProtocoloId, analise.ProtocoloPrestacaoId, analise.AnaliseId))
                    {
                        mensagens.Add("Já existe outra análise cadastrada para esta mesma PRESTAÇÃO / DATA / SITUAÇÃO.");
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

        private bool PossuiOutraAnaliseCadastradaPor(DataContext contexto, DateTime dataSituacao, int situacaoId, int protocoloPrestacaoId, int analiseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PROTOCOLO.ANALISE (NOLOCK)
                                    WHERE DATASITUACAO = @DATASITUACAO
	                                    AND SITUACAOPROTOCOLOID = @SITUACAOPROTOCOLOID
	                                    AND PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID
	                                    AND ANALISEID <> @ANALISEID ";

            contextQuery.Parameters.Add("@DATASITUACAO", SqlDbType.Date, dataSituacao.Date);
            contextQuery.Parameters.Add("@SITUACAOPROTOCOLOID", SqlDbType.Int, situacaoId);
            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloPrestacaoId);
            contextQuery.Parameters.Add("@ANALISEID", SqlDbType.Int, analiseId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.Analise analise, int? numeroFolha, string observacao)
        {
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new ProtocoloPrestacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza dados Protocolo Prestacao
                rnProtocoloPrestacao.AtualizaDadosAnalise(contexto, numeroFolha, analise.ProtocoloPrestacaoId, observacao, analise.UsuarioSistema);

                this.Insere(contexto, analise);
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

        private void Insere(DataContext contexto, Entidades.Analise analise)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Protocolo.ANALISE
                                       (PROTOCOLOPRESTACAOID
                                       ,SITUACAOPROTOCOLOID
                                       ,DATASITUACAO
                                       ,DESCRICAO
                                       ,USUARIOANALISADOR
                                       ,USUARIOREVISOR
                                       ,USUARIOSISTEMA
                                       ,DATACADASTRO
                                       ,DATAALTERACAO)
                                 VALUES
                                       (@PROTOCOLOPRESTACAOID
                                       ,@SITUACAOPROTOCOLOID
                                       ,@DATASITUACAO
                                       ,@DESCRICAO
                                       ,@USUARIOANALISADOR
                                       ,@USUARIOREVISOR
                                       ,@USUARIOSISTEMA
                                       ,@DATACADASTRO
                                       ,@DATAALTERACAO) 

                            SELECT IDENT_CURRENT('Protocolo.ANALISE') ";

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, analise.ProtocoloPrestacaoId);
            contextQuery.Parameters.Add("@SITUACAOPROTOCOLOID", SqlDbType.Int, analise.SituacaoProtocoloId);
            contextQuery.Parameters.Add("@DATASITUACAO", SqlDbType.Date, analise.DataSituacao.Date);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.Text, analise.Descricao);
            contextQuery.Parameters.Add("@USUARIOANALISADOR", SqlDbType.VarChar, analise.UsuarioAnalisador);
            contextQuery.Parameters.Add("@USUARIOREVISOR", SqlDbType.VarChar, analise.UsuarioRevisor);
            contextQuery.Parameters.Add("@USUARIOSISTEMA", SqlDbType.VarChar, analise.UsuarioSistema);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            analise.AnaliseId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.Analise analise, int? numeroFolhas, string observacao)
        {
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new ProtocoloPrestacao();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza dados Protocolo Prestacao
                rnProtocoloPrestacao.AtualizaDadosAnalise(contexto, numeroFolhas, analise.ProtocoloPrestacaoId, observacao, analise.UsuarioSistema);

                this.Atualiza(contexto, analise);
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

        private void Atualiza(DataContext contexto, Entidades.Analise analise)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Protocolo.ANALISE
                               SET SITUACAOPROTOCOLOID = @SITUACAOPROTOCOLOID,
                                  DATASITUACAO = @DATASITUACAO,
                                  DESCRICAO = @DESCRICAO,
                                  USUARIOANALISADOR = @USUARIOANALISADOR,
                                  USUARIOSISTEMA = @USUARIOSISTEMA,
                                  USUARIOREVISOR = @USUARIOREVISOR,
                                  DATAALTERACAO = @DATAALTERACAO
                             WHERE ANALISEID = @ANALISEID ";

            contextQuery.Parameters.Add("@ANALISEID", SqlDbType.Int, analise.AnaliseId);
            contextQuery.Parameters.Add("@SITUACAOPROTOCOLOID", SqlDbType.Int, analise.SituacaoProtocoloId);
            contextQuery.Parameters.Add("@DATASITUACAO", SqlDbType.Date, analise.DataSituacao.Date);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, analise.Descricao);
            contextQuery.Parameters.Add("@USUARIOANALISADOR", SqlDbType.VarChar, analise.UsuarioAnalisador);
            contextQuery.Parameters.Add("@USUARIOREVISOR", SqlDbType.VarChar, analise.UsuarioRevisor);
            contextQuery.Parameters.Add("@USUARIOSISTEMA", SqlDbType.VarChar, analise.UsuarioSistema);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiAnalisePor(DataContext contexto, int protocoloPrestacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Protocolo.ANALISE
                                    where PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID ";

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", protocoloPrestacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiSituacaoProtocoloPor(DataContext contexto, int situacaoProtocoloId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Protocolo.ANALISE (NOLOCK)
                                    WHERE SITUACAOPROTOCOLOID = @SITUACAOPROTOCOLOID ";

            contextQuery.Parameters.Add("@SITUACAOPROTOCOLOID", situacaoProtocoloId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }


        public void RemoveAnalise(DataContext contexto, int protocoloPrestacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Protocolo.ANALISE
                                        WHERE PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID ";

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloPrestacaoId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
