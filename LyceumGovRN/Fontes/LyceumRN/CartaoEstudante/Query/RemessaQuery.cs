using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.Enum;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class RemessaQuery : QueryBase<RemessaQuery>
    {
        private static readonly string TABELA_REMESSA = "LYCEUM.CartaoEstudante.REMESSA";
        private static readonly string TABELA_LOGREMESSA = "LYCEUM.CartaoEstudante.LOGREMESSA";
        private static readonly string TABELA_RETORNO = "LYCEUM.CartaoEstudante.RETORNO";
        private static readonly string PARAM_SITUACAO_PROCESSAMENTO_COM_CRITICA = " RET.SITUACAOPROCESSAMENTO = '2' ";
        private static readonly string PARAM_SITUACAO_PROCESSAMENTO_SEM_CRITICA = " RET.SITUACAOPROCESSAMENTO in ('1','10') ";
        private static readonly string PARAM_SITUACAO_PROCESSAMENTO_EM_PROCESSAMENTO = " ISNULL(RET.SITUACAOPROCESSAMENTO,'-1') not in ('1','2','10') ";

        RemessaQuery() { }

        public Remessa ObtemRemessaPor(int remessaId)
        {
            string SELECT_REMESSA =
                "SELECT * FROM " + TABELA_REMESSA + " " +
                "WHERE " +
                "    REMESSAID = @REMESSAID";

            Remessa remessa = ObtemPor<Remessa>(
                SELECT_REMESSA,
                remessaId
            );

            return remessa;
        }

        internal bool ExisteRemessaPara(string matricula)
        {
            string consulta = "select 1 FROM " + TABELA_REMESSA + " WHERE ALUNO = @MATRICULA";
            return Possui(consulta, matricula);
        }

        internal System.Data.DataTable ListaProcessamento(string aluno, string unidadeEnsino, DateTime? dataEnvioInicio, DateTime? dataEnvioFim, TipoSituacaoProcessamentoEnum tipoSituacaoProcessamento, string municipio, int idRegional)
        {
            var parameters = new List<string>();

            StringBuilder consulta = new StringBuilder(
                                                        @" SELECT T.NOMEOPERADORA
                                                                , T.REMESSAID
                                                                , T.ALUNO
                                                                , T.NOME_COMPL
                                                                , T.UNIDADE_ENS
                                                                , T.SOLICITACAOID
                                                                , T.CODSOLICITACAO
                                                                , T.DATAENVIO
                                                                , T.DATAINCLUSAOREMESSA        
                                                                , RET.DATAINCLUSAO AS DATAINCLUSAORETORNO
                                                                , RET.SITUACAOPROCESSAMENTO
                                                            FROM (
                                                                   SELECT O.NOMEOPERADORA
                                                                     , REM.REMESSAID
                                                                     , REM.ALUNO
                                                                     , REM.NOME_COMPL
                                                                     , REM.UNIDADE_ENS
                                                                     , REM.SOLICITACAOID
                                                                     , TS.CODSOLICITACAO
                                                                     , LLTR.DATAENVIO
                                                                     , REM.DATAINCLUSAO AS DATAINCLUSAOREMESSA
                                                                     , U.MUNICIPIO
                                                                     , U.ID_REGIONAL
                                                                     , RET.RETORNOID     
                                                                    FROM LYCEUM.CARTAOESTUDANTE.REMESSA REM (NOLOCK)
                                                                    LEFT JOIN CartaoEstudante.VW_RETORNO_ULTIMA_REMESSA RET  
                                                                      ON RET.REMESSAID       = REM.REMESSAID
                                                                         JOIN LYCEUM.CARTAOESTUDANTE.LOTEREMESSA LTR (NOLOCK)
                                                                      ON LTR.LOTEREMESSAID   = REM.LOTEREMESSAID   
                                                                    LEFT JOIN CartaoEstudante.VW_LOGLOTEREMESSA_MENOR_DATAENVIO LLTR
                                                                      ON LLTR.LOTEREMESSAID  = LTR.LOTEREMESSAID
                                                                         JOIN CARTAOESTUDANTE.SOLICITACAO S (NOLOCK)
                                                                      ON S.SOLICITACAOID     = REM.SOLICITACAOID
                                                                         JOIN CARTAOESTUDANTE.TIPOSOLICITACAO TS (NOLOCK)
                                                                      ON S.TIPOSOLICITACAOID = TS.TIPOSOLICITACAOID
                                                                         JOIN CARTAOESTUDANTE.OPERADORA O (NOLOCK)
                                                                      ON O.OPERADORAID       = REM.OPERADORAID
                                                                         JOIN DBO.LY_UNIDADE_ENSINO U (NOLOCK)
                                                                      ON REM.UNIDADE_ENS     = U.UNIDADE_ENS
                                                            ) AS T
                                                            LEFT JOIN LYCEUM.CARTAOESTUDANTE.RETORNO RET (NOLOCK)
                                                                ON T.RETORNOID = RET.RETORNOID ");
            #region Filtros

            if (!String.IsNullOrEmpty(aluno))
            {
                parameters.Add(string.Format(" T.ALUNO = '{0}' ", aluno));
            }

            if (!String.IsNullOrEmpty(unidadeEnsino))
            {
                parameters.Add(string.Format(" T.UNIDADE_ENS = {0} ", unidadeEnsino));
            }

            #region Critica
                       
            switch (tipoSituacaoProcessamento)
            {
                case TipoSituacaoProcessamentoEnum.Todos:
                    break;
                case TipoSituacaoProcessamentoEnum.ComCritica:
                    parameters.Add(PARAM_SITUACAO_PROCESSAMENTO_COM_CRITICA);
                    break;
                case TipoSituacaoProcessamentoEnum.SemCritica:
                    parameters.Add(PARAM_SITUACAO_PROCESSAMENTO_SEM_CRITICA);
                    break;
                case TipoSituacaoProcessamentoEnum.EmProcessamento:
                    parameters.Add(PARAM_SITUACAO_PROCESSAMENTO_EM_PROCESSAMENTO);
                    break;
                default:
                    break;
            }
            
            #endregion

            #region DataEnvio

            if (dataEnvioInicio.HasValue && dataEnvioInicio.Value != DateTime.MinValue && dataEnvioFim.HasValue && dataEnvioFim.Value != DateTime.MinValue)
            {
                parameters.Add(string.Format(" T.DATAENVIO BETWEEN CAST('{0}' as DATETIME) and CAST('{1}' as DATETIME) "
                                                , dataEnvioInicio.Value.ToString("yyyy-MM-dd 00:00")
                                                , dataEnvioFim.Value.ToString("yyyy-MM-dd 23:59:59.997")
                                            ));
            }
            else
            {
                if (dataEnvioInicio.HasValue && dataEnvioInicio.Value != DateTime.MinValue)
                {
                    parameters.Add(string.Format(" T.DATAENVIO >= CAST('{0}' as DATETIME) "
                                                    , dataEnvioInicio.Value.ToString("yyyy-MM-dd 00:00")
                                                ));
                }
                else
                {
                    if (dataEnvioFim.HasValue && dataEnvioFim.Value != DateTime.MinValue)
                        parameters.Add(string.Format(" T.DATAENVIO <= CAST('{0}' as DATETIME) "
                                                    , dataEnvioFim.Value.ToString("yyyy-MM-dd 23:59:59.997")
                                                    ));
                }
            }

            #endregion

            if (!String.IsNullOrEmpty(municipio))
            {
                parameters.Add(string.Format(" T.MUNICIPIO = {0} ", municipio));
            }

            if (idRegional != 0)
            {
                parameters.Add(string.Format(" T.ID_REGIONAL = {0} ", idRegional));
            }


            if (parameters.Count > 0)
            {
                consulta.Append(" WHERE ");
                consulta.Append(parameters.Aggregate((x, y) => string.Format("{0} AND {1}", x, y)));
            }

            consulta.Append(" ORDER BY T.REMESSAID ");

            #endregion

            return base.ObterDataTable(consulta.ToString());
        }
    }
}
