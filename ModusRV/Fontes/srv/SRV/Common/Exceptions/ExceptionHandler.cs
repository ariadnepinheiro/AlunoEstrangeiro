using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Common.View;
using System.Data.SqlClient;


namespace SRV.Common.Exceptions
{
    /// <summary>
    /// Reponsável pela manipulação de exceções nos Controllers
    /// </summary>
    public class ExceptionHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExceptionHandler));
        
        /// <summary>
        /// Método estático que manipula uma exceção e adiciona mensagem de feedback no ModelState.
        /// Se for BusinessException será exibida mensagem da exception. Se esta tiver InnerException (causa)
        /// a mensagem da causa será concatenada. Se for outra Exception será exibida mensagem default e concatenada 
        /// a mensagem da exeption.
        /// </summary>
        /// <param name="exception">Exception que foi lançada</param>
        /// <param name="defaultMessage">Mensagem de feedback padrão para a action em questão</param>
        /// <param name="modelState">ModelState do controller em execução</param>
        public static void Execute(Exception exception, string defaultMessage, ModelStateDictionary modelState)
        {
            if (exception is BusinessException)
            {
                defaultMessage = exception.Message;

                if (exception.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message))
                    defaultMessage += " (descrição: " + exception.InnerException.Message + ")";
            }
            else
            {
                log.Error(defaultMessage, exception);

                if (!string.IsNullOrEmpty(exception.Message))
                    defaultMessage += " (descrição: " + exception.Message + ")";
            }
            modelState.AddModelError(string.Empty, defaultMessage);
        }

        public static HttpCustomStatusCodeResult ExecuteAjax(Exception exception, string defaultMessage)
        {
            if (exception is BusinessException)
            {
                defaultMessage = exception.Message;

                if (exception.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message))
                    defaultMessage += " (descrição: " + exception.InnerException.Message + ")";
            }
            else if (exception is SqlException && ((SqlException)exception).Number == 547)
            {
                string msg = exception.Message;

                if (msg.IndexOf('.') >= 0)
                    msg = msg.Substring(0, msg.IndexOf('.'));

                msg = TratarMsg(msg);
                defaultMessage += " - Existe relação com a tabela " + msg;
            }
            else
            {
                log.Error(defaultMessage, exception);

                if (!string.IsNullOrEmpty(exception.Message))
                    defaultMessage += " (descrição: " + exception.Message + ")";
            }

            return new HttpCustomStatusCodeResult(400, defaultMessage);
        }

        private static string TratarMsg(string msgFK)
        {

            if (msgFK.Contains("FK_RV_SERVIDOR_RV_ALOC_SERV"))
                return "RV_ALOCACAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_ALOC_SERV"))
                return "RV_ALOCACAO_SERVIDOR";
            else if (msgFK.Contains("FK_ANO_REF_RV_ALOC_SERVIDOR"))
                return "RV_ALOCACAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_FUNC_SERV_RV_ALOC_SERV"))
                return "RV_ALOCACAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_USUARIO_RV_ARQUIVO_USR_IMP"))
                return "RV_ARQUIVO_IMPORTACAO";
            else if (msgFK.Contains("FK_RV_USUARIO_RV_ARQUIVO_USR_UPL"))
                return "RV_ARQUIVO_IMPORTACAO";
            else if (msgFK.Contains("FK_RV_ID_ARQUIVO_IMPORTACAO_ARQ_IMP_LOG"))
                return "RV_ARQUIVO_IMPORTACAO_LOG";
            else if (msgFK.Contains("FK_RV_AVAL_EXT_RV_AVAL_EXT_UNIDADM"))
                return "RV_AVALIACAO_EXTERNA_UNIDADM";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_AVAL_EXT_UNIDADM"))
                return "RV_AVALIACAO_EXTERNA_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANO_REF_RV_AVAL_EXT_UNIDADM"))
                return "RV_AVALIACAO_EXTERNA_UNIDADM";
            else if (msgFK.Contains("FK_RV_SERVIDOR_RV_CARGO_SERVIDOR"))
                return "RV_CARGO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_CARGO_SERVIDOR"))
                return "RV_CARGO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_CARGO_RV_CARGO_SERVIDOR"))
                return "RV_CARGO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_CRITERIO_UNIDADM"))
                return "RV_CRITERIO_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_CRITERIO_UNIDADM"))
                return "RV_CRITERIO_UNIDADM";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_ELEGIBILIDADE_UNIDADM"))
                return "RV_ELEGIBILIDADE_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_ELEG_UNIDADM"))
                return "RV_ELEGIBILIDADE_UNIDADM";
            else if (msgFK.Contains("FK_ID_USUARIO_EXECUCAO_CALCULO"))
                return "RV_EXECUCAO_CALCULO";
            else if (msgFK.Contains("FK_RV_GRUPO_FUNCAO_RV_FUNCAO"))
                return "RV_FUNCAO";
            else if (msgFK.Contains("FK_RV_SERVIDOR_RV_FUNCAO_SERVIDOR"))
                return "RV_FUNCAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_FUNCAO_SERVIDOR"))
                return "RV_FUNCAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_FUNCAO_RV_FUNCAO_SERVIDOR"))
                return "RV_FUNCAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_FUNCAO_SERVIDOR"))
                return "RV_FUNCAO_SERVIDOR";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_INDICADOR_UNIDADM"))
                return "RV_INDICADOR_UNIDADM";
            else if (msgFK.Contains("FK_RV_NIVEL_ENSINO_RV_INDIC_UNIDADM"))
                return "RV_INDICADOR_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_INDICADOR_UNIDADM"))
                return "RV_INDICADOR_UNIDADM";
            else if (msgFK.Contains("FK_RV_INDICADOR_RV_INDICADOR_UNIDADM"))
                return "RV_INDICADOR_UNIDADM";
            else if (msgFK.Contains("FK_ID_LOG_AUDITORIA"))
                return "RV_LOG_AUDITORIA_ITEM";
            else if (msgFK.Contains("FK_RV_AVAL_EXT_RV_META_AVAL_EXT_UNIDADM"))
                return "RV_META_AVAL_EXT_UNIDADM";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_META_AVAL_EXT_UNIDADM"))
                return "RV_META_AVAL_EXT_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANO_REF_RV_META_AVAL_EXT_UNIDADM"))
                return "RV_META_AVAL_EXT_UNIDADM";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_META_IGE_UNIDADM"))
                return "RV_META_IGE_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANO_REF_RV_META_IGE_UNIDADM"))
                return "RV_META_IGE_UNIDADM";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_META_UNIDADM"))
                return "RV_META_UNIDADM";
            else if (msgFK.Contains("FK_RV_NIVEL_ENSINO_RV_META_UNIDADM"))
                return "RV_META_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_META_UNIDADM"))
                return "RV_META_UNIDADM";
            else if (msgFK.Contains("FK_RV_INDICADOR_RV_META_UNIDADM"))
                return "RV_META_UNIDADM";
            else if (msgFK.Contains("FK_RV_MOTIVO_INELEG_UNIDADM_RV_ELEG_UNIDADM"))
                return "RV_MOTIVO_INELEG_UNIDADM";
            else if (msgFK.Contains("FK_RV_MOTIVO_INELEG_UNIDADM_RV_MOTIVO_INELEG"))
                return "RV_MOTIVO_INELEG_UNIDADM";
            else if (msgFK.Contains("FK_RV_NIVEL_ENSINO_RV_MODALIDADE"))
                return "RV_NIVEL_ENSINO";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_MODALIDADE_UNIDADM"))
                return "RV_NIVEL_ENSINO_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_MODALIDADE_UNIDADM"))
                return "RV_NIVEL_ENSINO_UNIDADM";
            else if (msgFK.Contains("FK_RV_NIVEL_ENSINO_RV_NIVEL_ENS_UNIDADM"))
                return "RV_NIVEL_ENSINO_UNIDADM";
            else if (msgFK.Contains("FK_ANO_REFERENCIA"))
                return "RV_NOTA";
            else if (msgFK.Contains("FK_RV_GRUPO_FUNC_RV_NOTA_FUNC_UNIDADM"))
                return "RV_NOTA_FUNCAO_UNIDADM";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_NOTA_FUNC_UNIDADM"))
                return "RV_NOTA_FUNCAO_UNIDADM";
            else if (msgFK.Contains("FK_RV_ANO_REF_RV_NOTA_FUNC_UNIDADM"))
                return "RV_NOTA_FUNCAO_UNIDADM";
            else if (msgFK.Contains("FK_RV_TIPO_OCORR_RV_OCORRENCIA"))
                return "RV_OCORRENCIA";
            else if (msgFK.Contains("FK_RV_OCORRENCIA_RV_OCOR_SERVIDOR"))
                return "RV_OCORRENCIA_SERVIDOR";
            else if (msgFK.Contains("FK_RV_SERVIDOR_RV_OCOR_SERVIDOR"))
                return "RV_OCORRENCIA_SERVIDOR";
            else if (msgFK.Contains("FK_RV_UNIDADM_RV_OCORR_SERV"))
                return "RV_OCORRENCIA_SERVIDOR";
            else if (msgFK.Contains("FK_RV_NOTA_RV_PARAM_CURVA_PREM"))
                return "RV_PARAMETRO_CURVA_PREMIACAO";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_PARAM_CURVA_PREM"))
                return "  RV_PARAMETRO_CURVA_PREMIACAO";
            else if (msgFK.Contains("FK_RV_GRUPO_FUNC_RV_CURV_PREMIACAO"))
                return "  RV_PARAMETRO_CURVA_PREMIACAO";
            else if (msgFK.Contains("FK_RV_INDICADOR_RV_PARAM_NOTA"))
                return "  RV_PARAMETRO_NOTA";
            else if (msgFK.Contains("FK_RV_NIVEL_ENSINO_RV_PARAM_NOTA"))
                return "  RV_PARAMETRO_NOTA";
            else if (msgFK.Contains("FK_RV_NOTA_RV_PARAM_NOTA"))
                return "  RV_PARAMETRO_NOTA";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_PARAM_NOTA"))
                return "RV_PARAMETRO_NOTA";
            else if (msgFK.Contains("FK_RV_TIPO_UNIDADM_RV_PARAM_PESO_UNIDADM"))
                return "RV_PARAMETRO_PESO";
            else if (msgFK.Contains("FK_RV_INDICADOR_RV_PARAM_PESO_UNIDADM"))
                return "RV_PARAMETRO_PESO";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_PARAM_PESO_UNIDADM"))
                return "RV_PARAMETRO_PESO";
            else if (msgFK.Contains("FK_RV_MODALIDADE_RV_PARAM_PESO_UNIDADM"))
                return "RV_PARAMETRO_PESO";
            else if (msgFK.Contains("FK_RV_GRUPO_FUNCAO_RV_PARAM_PESO"))
                return "RV_PARAMETRO_PESO";
            else if (msgFK.Contains("FK_RV_TIPO_UNIDADM_RV_TIPO_CRIT_ELEG"))
                return "RV_TIPO_CRITERIO_ELEGIBILIDADE";
            else if (msgFK.Contains("FK_RV_ANOREFERENCIA_RV_TIPO_CRIT_ELEG"))
                return "RV_TIPO_CRITERIO_ELEGIBILIDADE";
            else if (msgFK.Contains("FK_ID_USUARIO"))
                return "RV_TOKEN_ALTERACAO_SENHA";
            else if (msgFK.Contains("FK_RV_TIPO_UNIDADM_RV_UNIDADM"))
                return "RV_UNIDADE_ADMINISTRATIVA";
            else if (msgFK.Contains("FK_UNIDADE_REGIONAL"))
                return "RV_UNIDADE_ADMINISTRATIVA";
            else
                return " ";
            
        }

    }
}