using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class OperacaoExclusao
    {
        public void InsereOperacaoHistorico(DataContext contexto, int operacaoId, string motivo, string usuarioId)
        {
            //TOOD: MONVER PARA CLASSE CORRETA
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [LYCEUM].[PrestacaoContas].[OPERACAOEXCLUSAO] ( [OPERACAOID]
                              ,[PERIODOREFERENCIAID]
                              ,[PLANOTRABALHOID]
                              ,[CENSO]
                              ,[TIPOOPERACAO]
                              ,[VALOR]
                              ,[JUSTIFICATIVA]
                              ,[STATUS]
                              ,[DATAANALISE]
                              ,[MOTIVOREPROVACAOOPERACAOID]
                              ,[CODOPERACAO]
                              ,[USUARIOIDCADASTRO]
                              ,[DATACADASTRO] ,
	                          [DATAALTERACAO] ,
	                          [MOTIVOEXCLUSAO],
							  [DATAEXCLUSAO],
							  [USUARIOIDEXCLUSAO])
                        SELECT [OPERACAOID]
                              ,[PERIODOREFERENCIAID]
                              ,[PLANOTRABALHOID]
                              ,[CENSO]
                              ,[TIPOOPERACAO]
                              ,[VALOR]
                              ,[JUSTIFICATIVA]
                              ,[STATUS]
                              ,[DATAANALISE]
                              ,[MOTIVOREPROVACAOOPERACAOID]
                              ,[CODOPERACAO]
                              ,[USUARIOID]
                              ,[DATACADASTRO]
                              ,[DATAALTERACAO],
							   @MOTIVO,
							    GETDATE(), 
								@USUARIO
                          FROM [LYCEUM].[PrestacaoContas].[OPERACAO]
                          where OPERACAOID = @OPERACAOID  ";

            contextQuery.Parameters.Add("@MOTIVO", SqlDbType.VarChar, motivo);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereOperacaoDocumentosHistorico(DataContext contexto, int operacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [LYCEUM].[PrestacaoContas].[OPERACAODOCUMENTOSEXCLUSAO] 
				SELECT * FROM [LYCEUM].[PrestacaoContas].[OPERACAODOCUMENTOS]
                where OPERACAOID = @OPERACAOID   ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereOperacaoExigenciaHistorico(DataContext contexto, int operacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO [LYCEUM].[PrestacaoContas].[OPERACAOEXIGENCIAEXCLUSAO] 
				SELECT * FROM [LYCEUM].[PrestacaoContas].[OPERACAOEXIGENCIA]
                where OPERACAOID = @OPERACAOID   ";

            contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

            contexto.ApplyModifications(contextQuery);
        }

    }
}
