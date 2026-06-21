using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.ContratoTemporario.Entidades;

namespace Techne.Lyceum.RN
{
    public class CandidatoDocContrato : RNBase
    {
        public bool PossuiCandidatoDocContratoPor(string candidato, string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_CANDIDATO_DOC_CONTRATO 
                            WHERE  CONCURSO = @CONCURSO 
                                   AND CANDIDATO = @CANDIDATO  ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public void Insere(DataContext ctx, RN.Entidades.LyCandidatoDocContrato candidatoDocContrato)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.LY_CANDIDATO_DOC_CONTRATO
                                               (CONCURSO
                                               ,CANDIDATO
                                               ,STATUS
                                               ,DT_INICIO_CONTRATO
                                               ,DT_FIM_CONTRATO)
                                         VALUES
                                               (@CONCURSO
                                               ,@CANDIDATO
                                               ,@STATUS
                                               ,@DT_INICIO_CONTRATO
                                               ,@DT_FIM_CONTRATO) ";

                contextQuery.Parameters.Add("@CONCURSO", candidatoDocContrato.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidatoDocContrato.Candidato);
                contextQuery.Parameters.Add("@STATUS", candidatoDocContrato.Status);
                contextQuery.Parameters.Add("@DT_INICIO_CONTRATO", candidatoDocContrato.DtInicioContrato);
                contextQuery.Parameters.Add("@DT_FIM_CONTRATO", candidatoDocContrato.DtFimContrato);

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
        }

        public void AtualizaDadosProposta(DataContext ctx, RN.Entidades.LyCandidatoDocContrato candidatoDocContrato)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_CANDIDATO_DOC_CONTRATO 
                                        SET STATUS = @STATUS,
                                            DT_INICIO_CONTRATO = @DT_INICIO_CONTRATO
                                        WHERE  CONCURSO = @CONCURSO 
                                               AND CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", candidatoDocContrato.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidatoDocContrato.Candidato);
                contextQuery.Parameters.Add("@STATUS", candidatoDocContrato.Status);
                contextQuery.Parameters.Add("@DT_INICIO_CONTRATO", candidatoDocContrato.DtInicioContrato);

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
        }
    }
}
