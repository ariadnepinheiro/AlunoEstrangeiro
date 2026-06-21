using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.CartaoEstudante
{
    public class WsStatusFoto
    {
        public DataTable ObtemListaPor(string aluno, string censo)
        {
            DataTable retorno = null;

            if (!aluno.IsNullOrEmptyOrWhiteSpace())
            {
                //Busca id
                int idBeneficiario = this.ObtemIdBeneficiarioPor(aluno);
                retorno = this.ObtemListaPor(idBeneficiario);
            }
            else
            {
                retorno = this.ObtemListaUltimaSituacaoPor(censo);
            }

            return retorno;
        }

        private int ObtemIdBeneficiarioPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT IDBENEFICIARIO
					            FROM CARTAOESTUDANTE.WSSTATUSFOTO
					            WHERE MATRICULA = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno); 

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["IDBENEFICIARIO"]);
                }

                return retorno;
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

        private DataTable ObtemListaPor(int idBeneficiario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  WSSTATUSFOTOID, 
		                                        EXECUCAOINTEGRADORID, 
		                                        NUMEROREGISTRO, 
		                                        MATRICULA, 
                                                PE.NOME_COMPL AS NOME_ALUNO,
		                                        IDBENEFICIARIO, 
		                                        NUMEROCARTAO, 
		                                        NUMEROCHIP, 
		                                        GEROUREQUISICAO, 
		                                        WS.CRITICAFOTOID, 
		                                        C.DESCRICAO AS CRITICAFOTO,
		                                        IDREQUISICAO, 
		                                        CASE WHEN TIPOREQUISICAO = 1 THEN '1ª via' WHEN TIPOREQUISICAO = 2 THEN '2ª via' END TIPOREQUISICAO,  
		                                        DATAREQUISICAO, 
		                                        IDFOTO, 
		                                        WS.ORIGEMFOTOID, 
		                                        O.DESCRICAO AS ORIGEMFOTO,
		                                        DATAINCLUCAO, 
		                                        STATUSFOTO,
		                                        DATASTATUS, 
		                                        WS.MOTIVOREJEICAOFOTOID,
		                                        M.DESCRICAO AS MOTIVOREJEICAOFOTO
                                        FROM CARTAOESTUDANTE.WSSTATUSFOTO WS (nolock)
	                                        LEFT JOIN CARTAOESTUDANTE.ORIGEMFOTO O (nolock) 
			                                        ON WS.ORIGEMFOTOID = O.ORIGEMFOTOID
	                                        LEFT JOIN CARTAOESTUDANTE.CRITICAFOTO C (nolock)
			                                        ON WS.CRITICAFOTOID = C.CRITICAFOTOID
	                                        LEFT JOIN CARTAOESTUDANTE.MOTIVOREJEICAOFOTO M (nolock) 
			                                        ON WS.MOTIVOREJEICAOFOTOID = M.MOTIVOREJEICAOFOTOID
                                            INNER JOIN LY_ALUNO AL ON AL.ALUNO = WS.MATRICULA
											INNER JOIN LY_PESSOA PE ON PE.PESSOA = AL.PESSOA
                                        WHERE WS.IDBENEFICIARIO = @IDBENEFICIARIO
                                        ORDER BY WS.DATACADASTRO DESC ";

                contextQuery.Parameters.Add("@IDBENEFICIARIO", SqlDbType.Int, idBeneficiario);

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

        private DataTable ObtemListaUltimaSituacaoPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT WS.MATRICULA, CONVERT(INT, NULL) AS ULTIMOID
                                    INTO #MATRICULAS
                                    FROM CARTAOESTUDANTE.WSSTATUSFOTO WS
                                        INNER JOIN LY_ALUNO A ON WS.MATRICULA = A.ALUNO
                                    WHERE UNIDADE_ENSINO = @CENSO

                                    UPDATE #MATRICULAS
                                    SET ULTIMOID = (SELECT TOP 1 WSSTATUSFOTOID
				                                    FROM CARTAOESTUDANTE.WSSTATUSFOTO WS
				                                    WHERE WS.MATRICULA = #MATRICULAS.MATRICULA
				                                    ORDER BY WS.DATACADASTRO DESC)

                                    SELECT  WSSTATUSFOTOID, 
		                                    EXECUCAOINTEGRADORID, 
		                                    NUMEROREGISTRO, 
		                                    ws.MATRICULA, 
                                            PE.NOME_COMPL AS NOME_ALUNO,
		                                    IDBENEFICIARIO, 
		                                    NUMEROCARTAO, 
		                                    NUMEROCHIP, 
		                                    GEROUREQUISICAO, 
		                                    WS.CRITICAFOTOID, 
		                                    C.DESCRICAO AS CRITICAFOTO,
		                                    IDREQUISICAO, 
		                                    CASE WHEN TIPOREQUISICAO = 1 THEN '1ª via' WHEN TIPOREQUISICAO = 2 THEN '2ª via' END TIPOREQUISICAO, 
		                                    DATAREQUISICAO, 
		                                    IDFOTO, 
		                                    WS.ORIGEMFOTOID, 
		                                    O.DESCRICAO AS ORIGEMFOTO,
		                                    DATAINCLUCAO, 
		                                    STATUSFOTO,
		                                    DATASTATUS, 
		                                    WS.MOTIVOREJEICAOFOTOID,
		                                    MO.DESCRICAO AS MOTIVOREJEICAOFOTO
                                    FROM #MATRICULAS M
	                                    INNER JOIN CARTAOESTUDANTE.WSSTATUSFOTO WS
		                                    on  m.ULTIMOID = WS.WSSTATUSFOTOID
	                                    LEFT JOIN CARTAOESTUDANTE.ORIGEMFOTO O (nolock) 
			                                    ON WS.ORIGEMFOTOID = O.ORIGEMFOTOID
	                                    LEFT JOIN CARTAOESTUDANTE.CRITICAFOTO C (nolock)
			                                    ON WS.CRITICAFOTOID = C.CRITICAFOTOID
	                                    LEFT JOIN CARTAOESTUDANTE.MOTIVOREJEICAOFOTO MO (nolock) 
			                                    ON WS.MOTIVOREJEICAOFOTOID = Mo.MOTIVOREJEICAOFOTOID
                                        INNER JOIN LY_ALUNO AL ON AL.ALUNO = WS.MATRICULA
										INNER JOIN LY_PESSOA PE ON PE.PESSOA = AL.PESSOA
                                    ORDER BY WS.DATACADASTRO DESC

                                    DROP TABLE #MATRICULAS ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public void Insere(List<Techne.Lyceum.RN.CartaoEstudante.Entity.WsStatusFoto> listaFoto, out int cadastrados)
        {
            FotoPessoa rnFotoPessoa = new FotoPessoa();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            cadastrados = 0;

            try
            {
                foreach (Techne.Lyceum.RN.CartaoEstudante.Entity.WsStatusFoto item in listaFoto)
                {
                    //Insere Foto
                    this.Insere(contexto, item);

                    bool processado = true;
                    bool aprovado = false;

                    //Verifica se foto foi aprovada:
                    //1. caso foto tenha sido processada (caso a coluna GEROUREQUISICAO igual a 1 e STATUS igual a aprovado na tabela CartaoEstudando.WSSTATUSFOTO) 
                    //2. caso a critica foto seja igual a 4 (ALUNO JÁ TEM FOTO APROVADA) ou 5 (ALUNO JÁ TEM CARTÃO GERADO)
                    if ((item.GerouRequisicao && item.StatusFoto.ToUpper() == "APROVADO")
                        || (item.CriticaFotoId == 4 || item.CriticaFotoId == 5))
                    {
                        aprovado = true;
                    }                    
                   
                    //Atualiza tabela de controle de foto
                    rnFotoPessoa.AtualizaProcessamento(contexto, item, processado, aprovado);
                    
                    cadastrados++;
                };
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

        private void Insere(DataContext contexto, Entity.WsStatusFoto wsStatusFoto)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO CartaoEstudante.WSSTATUSFOTO
                                               (EXECUCAOINTEGRADORID,
                                               NUMEROREGISTRO,
                                               MATRICULA,
                                               IDBENEFICIARIO,
                                               NUMEROCARTAO,
                                               NUMEROCHIP,
                                               GEROUREQUISICAO,
                                               CRITICAFOTOID,
                                               IDREQUISICAO,
                                               IDSOLITACAO,
                                               TIPOREQUISICAO,
                                               DATAREQUISICAO,
                                               DATAINCLUCAOWS,
                                               DATAREQUISICAOWS,
                                               DATASTATUSWS,
                                               IDFOTO,
                                               ORIGEMFOTOID,
                                               DATAINCLUCAO,
                                               STATUSFOTO,
                                               DATASTATUS,
                                               MOTIVOREJEICAOFOTOID,
                                               USUARIOID,
                                               DATACADASTRO,
                                               DATAALTERACAO)
                                         VALUES
                                               (@EXECUCAOINTEGRADORID, 
                                               @NUMEROREGISTRO, 
                                               @MATRICULA, 
                                               @IDBENEFICIARIO, 
                                               @NUMEROCARTAO, 
                                               @NUMEROCHIP, 
                                               @GEROUREQUISICAO,
                                               @CRITICAFOTOID, 
                                               @IDREQUISICAO, 
                                               @IDSOLITACAO, 
                                               @TIPOREQUISICAO, 
                                               @DATAREQUISICAO, 
                                               @DATAINCLUCAOWS,
                                               @DATAREQUISICAOWS,
                                               @DATASTATUSWS,
                                               @IDFOTO, 
                                               @ORIGEMFOTOID, 
                                               @DATAINCLUCAO, 
                                               @STATUSFOTO, 
                                               @DATASTATUS, 
                                               @MOTIVOREJEICAOFOTOID, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@EXECUCAOINTEGRADORID", SqlDbType.Int, wsStatusFoto.ExecucaoIntegradorId);
            contextQuery.Parameters.Add("@NUMEROREGISTRO", SqlDbType.Int, wsStatusFoto.NumeroRegistro);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, wsStatusFoto.Matricula);
            contextQuery.Parameters.Add("@IDBENEFICIARIO", SqlDbType.Int, wsStatusFoto.IdBeneficiario);
            contextQuery.Parameters.Add("@NUMEROCARTAO", SqlDbType.VarChar, wsStatusFoto.NumeroCartao);
            contextQuery.Parameters.Add("@NUMEROCHIP", SqlDbType.VarChar, wsStatusFoto.NumeroChip);
            contextQuery.Parameters.Add("@GEROUREQUISICAO", SqlDbType.Bit, wsStatusFoto.GerouRequisicao);
            contextQuery.Parameters.Add("@CRITICAFOTOID", SqlDbType.Int, (wsStatusFoto.CriticaFotoId == null || wsStatusFoto.CriticaFotoId == 0) ? DBNull.Value : (object)wsStatusFoto.CriticaFotoId);
            contextQuery.Parameters.Add("@IDREQUISICAO", SqlDbType.Int, (wsStatusFoto.IdRequisicao == null || wsStatusFoto.IdRequisicao == 0) ? DBNull.Value : (object)wsStatusFoto.IdRequisicao);
            contextQuery.Parameters.Add("@IDSOLITACAO", SqlDbType.Int, (wsStatusFoto.IdSolitacao == null || wsStatusFoto.IdSolitacao == 0) ? DBNull.Value : (object)wsStatusFoto.IdSolitacao);
            contextQuery.Parameters.Add("@TIPOREQUISICAO", SqlDbType.Int, (wsStatusFoto.TipoRequisicao == null || wsStatusFoto.TipoRequisicao == 0) ? DBNull.Value : (object)wsStatusFoto.TipoRequisicao);
            contextQuery.Parameters.Add("@DATAREQUISICAO", SqlDbType.DateTime, wsStatusFoto.DataRequisicao == null ? DBNull.Value : (object)wsStatusFoto.DataRequisicao);
            contextQuery.Parameters.Add("@DATAINCLUCAOWS", SqlDbType.VarChar, wsStatusFoto.DataInclucaoWs);
            contextQuery.Parameters.Add("@DATAREQUISICAOWS", SqlDbType.VarChar, wsStatusFoto.DataRequisicaoWs);
            contextQuery.Parameters.Add("@DATASTATUSWS", SqlDbType.VarChar, wsStatusFoto.DataStatusWs);
            contextQuery.Parameters.Add("@IDFOTO", SqlDbType.Int, (wsStatusFoto.IdFoto == null || wsStatusFoto.IdFoto == 0) ? DBNull.Value : (object)wsStatusFoto.IdFoto);
            contextQuery.Parameters.Add("@ORIGEMFOTOID", SqlDbType.Int, (wsStatusFoto.OrigemFotoId == null || wsStatusFoto.OrigemFotoId == 0) ? DBNull.Value : (object)wsStatusFoto.OrigemFotoId);
            contextQuery.Parameters.Add("@DATAINCLUCAO", SqlDbType.DateTime, wsStatusFoto.DataInclucao == null ? DBNull.Value : (object)wsStatusFoto.DataInclucao);
            contextQuery.Parameters.Add("@STATUSFOTO", SqlDbType.VarChar, wsStatusFoto.StatusFoto);
            contextQuery.Parameters.Add("@DATASTATUS", SqlDbType.DateTime, wsStatusFoto.DataStatus == null ? DBNull.Value : (object)wsStatusFoto.DataStatus);
            contextQuery.Parameters.Add("@MOTIVOREJEICAOFOTOID", SqlDbType.Int, (wsStatusFoto.MotivoRejeicaoFotoId == null || wsStatusFoto.MotivoRejeicaoFotoId < 0) ? DBNull.Value : (object)wsStatusFoto.MotivoRejeicaoFotoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, wsStatusFoto.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
