using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class ConvocacaoSemEmail
    {
        public void Insere(DataContext contexto, Entidades.ConvocacaoSemEmail convocacaoSemEmail)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO MATRICULA.CONVOCACAOSEMEMAIL 
                                                    (INSCRICAOALUNOID, 
                                                     OPCAOINSCRICAOID, 
                                                     USUARIORESPONSAVEL, 
                                                     DATAAVISO) 
                                        VALUES      (@INSCRICAOALUNOID, 
                                                     @OPCAOINSCRICAOID, 
                                                     @USUARIORESPONSAVEL, 
                                                     @DATAAVISO) 
                                          ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, convocacaoSemEmail.InscricaoAlunoId);
            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, convocacaoSemEmail.OpcaoInscricaoId);
            contextQuery.Parameters.Add("@USUARIORESPONSAVEL", SqlDbType.VarChar, convocacaoSemEmail.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATAAVISO", SqlDbType.DateTime, convocacaoSemEmail.DataAviso);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaConvocacaoSemEmailPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            string periodos = string.Empty;
            int ano = 0;

            try
            {
                ano = rnPeriodoLetivo.ObtemAnoAberto();

                if (ano > 0)
                {
                    periodos = rnPeriodoLetivo.ObtemPeriodoAbertoPor(ano);

                    contextQuery.Command = string.Format(@" SELECT I.NUMEROINSCRICAO, 
                                       PC.NOME, 
                                       PC.NOMEMAE, 
                                       PC.DATANASCIMENTO, 
                                       PC.CELULAR, 
                                       PC.FIXOCELULAR, 
                                       PC.EMAIL, 
                                       OP.DATACONVOCACAO, 
                                       OP.PRAZORESPOSTA
                                FROM   MATRICULA.CONVOCACAOSEMEMAIL SM (NOLOCK) 
                                       INNER JOIN MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
                                               ON SM.OPCAOINSCRICAOID = OP.OPCAOINSCRICAOID 
									   INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                               ON OP.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
                                       INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                               ON OP.INSCRICAOALUNOID = I.INSCRICAOALUNOID 
                                       INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                               ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID 
                                WHERE  I.ANO = @ANO 
									   AND CV.CENSO = @CENSO
									   AND ISNULL(SM.CONVOCADOSITE, 0) = 0
                                       AND I.PERIODO IN ( {0} ) ", periodos);

                    contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                    contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                    dt = contexto.GetDataTable(contextQuery);
                }
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
