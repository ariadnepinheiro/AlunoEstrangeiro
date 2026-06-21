using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Certificacao
{
   public class ValidaInfCertificacao
    {
       private ValidacaoDados validacao;

       RN.Certificacao.DocumentoGerado rnDocumentoGerado = new RN.Certificacao.DocumentoGerado();

       public DataTable ListarAlunos(int ano, string Curso, string Faculdade, int SEMESTRE)
       {
           DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
           ContextQuery contextQuery = new ContextQuery();
           DataTable dt = null;

           try
           {
               contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
               contextQuery.Command = @"CertificacaoEscolar.VALIDAINFORMACAODOCUMENTO";
                            

               contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, SEMESTRE);
               contextQuery.Parameters.Add("@ano", SqlDbType.Int, ano);
               contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, Curso);
               contextQuery.Parameters.Add("@UNIDADEENSINO", SqlDbType.VarChar, Faculdade);
             
               dt = ctx.GetDataTable(contextQuery);
           }
           catch (Exception ex)
           {
               ctx.Abandon();
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
               ctx.Dispose();
           }

           return dt;
       }

       public ValidacaoDados ValidaSegundaVia(RN.Certificacao.infAluno aluno)
       {

           List<string> mensagens = new List<string>();
           DataContext contexto = null;
           ValidacaoDados validacaoDados = new ValidacaoDados
           {
               Valido = false
           };

           if (aluno.tpDocumento == string.Empty) //verifica se o tipo do documento é nulo
           {
               mensagens.Add("Tipo do Documento é obrigatório.");
           }
           if (aluno.tpDocumento == string.Empty) //verifica se o tipo da conclusão é nula
           {
               mensagens.Add("Tipo da Conclusao é obrigatório.");
           }

           if (Convert.ToInt32(aluno.tpDocumento) != 2) //verifica se é certidão escolar
           {
               mensagens.Add("O tipo do documento precisa ser Certidão Escolar.");
           }

           if (aluno.matricula.IsNullOrEmptyOrWhiteSpace())//verifica se a matricula é nula
           {
               mensagens.Add("Aluno é obrigatório.");
           }


           if (aluno.documentoCertID == 0)
           {
               mensagens.Add("Documento  certificação é obrigatório.");
           }

           
           if (mensagens.Count == 0)
           {
               try
               {
                   contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                   //Verifica se existe arquivo gerado para o tipo de Documento certificação


                   if (!rnDocumentoGerado.possuiArquivoGerado(contexto, aluno.documentoCertID))
                   {
                       mensagens.Add("O arquivo referente ao certificado, não foi encontrado. Não é possivel gerar 2ª via.");
                   }


               }
               catch (Exception ex)
               {
                   if (contexto != null)
                   {
                       contexto.Abandon();
                   }
                   throw new Exception("Não é possivel gerar 2ª via."+ ex.Message);
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
               validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
           }
           else
           {
               validacaoDados.Valido = true;
           }

           return validacaoDados;
       }
    }
}
