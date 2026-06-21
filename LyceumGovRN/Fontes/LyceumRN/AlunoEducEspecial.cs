using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class AlunoEducEspecial : RNBase
    {
        public static TceAlunoEducEspecial Carregar(string aluno, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @" SELECT TOP 1
                                *
                         FROM   TCE_ALUNO_EDUC_ESPECIAL
                         WHERE  ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                         ORDER BY DT_CADASTRO DESC ");
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.TryToBindEntity<TceAlunoEducEspecial>(contextQuery);
            }
        }

        public static DataTable ListaPor(DbObject aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable educacaoEspecial = null;

            try
            {
                contextQuery.Command = @" SELECT  ae.ALUNO ,
                            ae.ANO ,
                            ae.PERIODO ,
                            ae.DT_OFERTA ,
                            ae.NEC_TEC_ASSISTIDA ,
                            ae.ACEITE ,
                            ae.CENSO ,
                            ae.OBSERVACAO,
                            ae.CUIDADOR,
                            ae.INTERPRETELIBRAS,
                            ae.LEDOR,
                            ae.TRANSPORTEADAPTADO
                    FROM    TCE_ALUNO_EDUC_ESPECIAL ae WITH ( NOLOCK )
                    WHERE   ae.ALUNO = @ALUNO
                    ORDER BY AE.DT_CADASTRO DESC ";

                contextQuery.Parameters.Add("@ALUNO", Convert.ToString(aluno));

                educacaoEspecial = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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

            return educacaoEspecial;
        }

        public ValidacaoDados Valida(TceAlunoEducEspecial alunoEducEspecial)
        {
            int censo = 0;
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunoEducEspecial == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(alunoEducEspecial.Aluno))
            {
                mensagens.Add("Favor informar o aluno.");
            }

            if (alunoEducEspecial.Ano <= 0)
            {
                mensagens.Add("Favor informar o ANO LETIVO do Atendimento Educacional Especializado.");
            }

            if (alunoEducEspecial.Periodo < 0)
            {
                mensagens.Add("Favor informar o PERIODO LETIVO do Atendimento Educacional Especializado.");
            }

            if (alunoEducEspecial.DtOferta <= DateTime.MinValue)
            {
                mensagens.Add("Favor informar a DATA DA OFERTA do Atendimento Educacional Especializado.");
            }
            else
            {
                if (alunoEducEspecial.DtOferta.Date > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA DA OFERTA do Atendimento Educacional Especializado não pode ser maior que a data atual.");
                }
            }

             if (alunoEducEspecial.Aceite)
            {
                if (string.IsNullOrEmpty(alunoEducEspecial.Censo))
                {
                    mensagens.Add("Favor informar a UNIDADE DE ENSINO do Atendimento Educacional Especializado.");
                }
                else if ((alunoEducEspecial.Censo.Length != 8) || !int.TryParse(alunoEducEspecial.Censo, out censo))
                {
                    mensagens.Add("Favor informar uma UNIDADE DE ENSINO válida para o Atendimento Educacional Especializado.");
                }
            }

            if (string.IsNullOrEmpty(alunoEducEspecial.Matricula))
            {
                mensagens.Add("Favor informar o usuário responsável.");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    //verifica se a aluno já possui a mesma escola / aceite / ano / periodo
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*)
                        FROM    DBO.TCE_ALUNO_EDUC_ESPECIAL
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND ACEITE = @ACEITE
                                AND CENSO = @CENSO ");

                    contextQuery.Parameters.Add("@ALUNO", alunoEducEspecial.Aluno);
                    contextQuery.Parameters.Add("@ANO", alunoEducEspecial.Ano);
                    contextQuery.Parameters.Add("@PERIODO", alunoEducEspecial.Periodo);
                    contextQuery.Parameters.Add("@ACEITE", alunoEducEspecial.Aceite);
                    contextQuery.Parameters.Add("@CENSO", alunoEducEspecial.Censo);

                    var aceites = ctx.GetReturnValue<int>(contextQuery);

                    if (aceites > 0)
                    {
                        mensagens.Add("Este aluno já possui atendimento especial para este ano / periodo / censo / tipo aceite");
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

        public void Insere(TceAlunoEducEspecial alunoEducEspecial)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.TCE_ALUNO_EDUC_ESPECIAL 
                                        (ALUNO, 
                                         ANO, 
                                         PERIODO, 
                                         DT_OFERTA, 
                                         NEC_TEC_ASSISTIDA, 
                                         ACEITE, 
                                         CENSO, 
                                         OBSERVACAO, 
                                         MATRICULA,
                                         CUIDADOR, 
                                         INTERPRETELIBRAS, 
                                         LEDOR,
                                         TRANSPORTEADAPTADO) 
                            VALUES     (@ALUNO, 
                                        @ANO, 
                                        @PERIODO, 
                                        @DT_OFERTA, 
                                        @NEC_TEC_ASSISTIDA, 
                                        @ACEITE, 
                                        @CENSO, 
                                        @OBSERVACAO, 
                                        @MATRICULA,
                                        @CUIDADOR, 
                                        @INTERPRETELIBRAS, 
                                        @LEDOR,
                                        @TRANSPORTEADAPTADO)  ";

                contextQuery.Parameters.Add("@ALUNO ", alunoEducEspecial.Aluno);
                contextQuery.Parameters.Add("@ANO", alunoEducEspecial.Ano);
                contextQuery.Parameters.Add("@PERIODO", alunoEducEspecial.Periodo);
                contextQuery.Parameters.Add("@DT_OFERTA", alunoEducEspecial.DtOferta);
                contextQuery.Parameters.Add("@NEC_TEC_ASSISTIDA", alunoEducEspecial.NecTecAssistida);
                contextQuery.Parameters.Add("@ACEITE", alunoEducEspecial.Aceite);
                contextQuery.Parameters.Add("@CENSO ", alunoEducEspecial.Censo);
                contextQuery.Parameters.Add("@OBSERVACAO", alunoEducEspecial.Observacao);
                contextQuery.Parameters.Add("@MATRICULA", alunoEducEspecial.Matricula);
                contextQuery.Parameters.Add("@CUIDADOR", alunoEducEspecial.Cuidador);
                contextQuery.Parameters.Add("@INTERPRETELIBRAS", alunoEducEspecial.InterpreteLibras);
                contextQuery.Parameters.Add("@LEDOR", alunoEducEspecial.Ledor);
                contextQuery.Parameters.Add("@TRANSPORTEADAPTADO", alunoEducEspecial.TransporteAdaptado);

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
    }
}
