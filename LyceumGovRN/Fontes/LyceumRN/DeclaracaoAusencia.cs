using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.RN
{
    public class DeclaracaoAusencia : RNBase
    {
        #region Enum
        public enum TipoDeclaracaoAusenciaId
        {
            [StringValue("Declaração de ausência da Mãe")]
            DeclaracaoAusenciaMae = 1,
            [StringValue("Declaração de ausência do Pai")]
            DeclaracaoAusenciaPai = 2,
            [StringValue("Declaração de ausência de certidão civil")]
            DeclaracaoCertidaoCivil = 3,
            [StringValue("Declaração de posse do laudo médico")]
            DeclaracaoNecessidadeEspecial = 4       
        }
        #endregion

        public static DataTable Listar(string alunoId)
        {
            try
            {
                var contextQuery = new ContextQuery(
                                @" SELECT  *
                                FROM    DBO.DECLARACAOAUSENCIA                                      
                                WHERE   ALUNOID = @ALUNOID  ");

                contextQuery.Parameters.Add("@ALUNOID", alunoId);

                return Consultar(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine,
                    Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }



        public static ValidacaoDados Validar(Entidades.DeclaracaoAusencia declaracaoAusencia)
        {
            try
            {
                var mensagens = new List<string>();
                var validacaoDados = new ValidacaoDados
                {
                    Valido = false
                };

                if (declaracaoAusencia == null)
                {
                    return validacaoDados;
                }

                if (string.IsNullOrEmpty(declaracaoAusencia.AlunoId))
                {
                    mensagens.Add("Aluno não encontrado.");
                }
                if (declaracaoAusencia.TipoDeclaracaoAusenciaId == 0)
                {
                    mensagens.Add("O campo Tipo de Declaração é obrigatório.");
                }

                if (declaracaoAusencia.TipoDeclaracaoAusenciaId == 3 && string.IsNullOrEmpty(declaracaoAusencia.Motivo))
                {
                    mensagens.Add("O campo Motivo é obrigatório para Certidão Civil.");
                }

                if (!string.IsNullOrEmpty(declaracaoAusencia.Motivo))
                {
                    if (declaracaoAusencia.Motivo.Length < 10)
                    {
                        mensagens.Add("O campo MOTIVO deve ter mais 10 caracteres.");
                    }

                    if (declaracaoAusencia.Motivo.Length > 200)
                    {
                        mensagens.Add("O campo MOTIVO é obrigatório com o máximo de 200 caracteres!");
                    }

                    var regex = new Regex(@"(\w)\1\1+");

                    if (regex.IsMatch(declaracaoAusencia.Motivo))
                    {
                        mensagens.Add("O campo MOTIVO não deve ter mais de 2 letras consecutivas repetidas.");
                    }
                }

                if (string.IsNullOrEmpty(declaracaoAusencia.Matricula)
                        || (!string.IsNullOrEmpty(declaracaoAusencia.Matricula)
                            && declaracaoAusencia.Matricula.Length > 12))
                {
                    mensagens.Add("O campo MATRICULA é obrigatório com o máximo de 12 caracteres!");
                }

                if (mensagens.Count == 0)
                {
                    //                    using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    //                    {
                    //                        //Verifica já existe outra inserção com o mesmo perfil / modalidade
                    //                        var contextQuery = new ContextQuery
                    //                        {
                    //                            Command =
                    //                                @" SELECT TOP 1
                    //                                    1
                    //                            FROM    DBO.DECLARACAOAUSENCIA
                    //                            WHERE   MODALIDADEID = @MODALIDADEID
                    //                                    AND PERFILMODALIDADEID <> @PERFILMODALIDADEID "
                    //                        };

                    //                        contextQuery.Parameters.Add("@MODALIDADEID", perfilModalidade.ModalidadeId);
                    //                        contextQuery.Parameters.Add("@PERFILMODALIDADEID", perfilModalidade.PerfilModalidadeId);

                    //                        var obj = ctx.GetReturnValue(contextQuery);

                    //                        if (obj != null)
                    //                        {
                    //                            mensagens.Add("Esta Modalidade já foi cadastrado anteriormente.");
                    //                        }
                    //                    }
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
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine,
                    Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        public static void Inserir(Entidades.DeclaracaoAusencia declaracaoAusencia, DataContext context)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"INSERT  INTO DBO.DECLARACAOAUSENCIA
                                            ( ALUNOID ,
                                              TIPODECLARACAOAUSENCIAID ,
                                              MATRICULA ,
                                              MOTIVO
                                            )
                                    VALUES  ( @ALUNOID ,
                                              @TIPODECLARACAOAUSENCIAID ,
                                              @MATRICULA  ,
                                              @MOTIVO               
                                            ) "
                };

                contextQuery.Parameters.Add("@ALUNOID", declaracaoAusencia.AlunoId);
                contextQuery.Parameters.Add("@TIPODECLARACAOAUSENCIAID", declaracaoAusencia.TipoDeclaracaoAusenciaId);
                contextQuery.Parameters.Add("@MATRICULA", declaracaoAusencia.Matricula);
                contextQuery.Parameters.Add("@MOTIVO", declaracaoAusencia.Motivo);

                context.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                       Environment.NewLine,
                       Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        public static void RemoverPorAluno(string aluno, DataContext context)
        {
            if (string.IsNullOrEmpty(aluno))
            {
                return;
            }

            var contextQuery = new ContextQuery
            {
                Command = @" DELETE  DBO.DECLARACAOAUSENCIA
                            WHERE   ALUNOID = @ALUNOID "
            };
            contextQuery.Parameters.Add("@ALUNOID", aluno);

            context.ApplyModifications(contextQuery);
        }

        public static void RemoverDadosCadastraisPorAluno(string aluno, DataContext context)
        {
            if (string.IsNullOrEmpty(aluno))
            {
                return;
            }

            var contextQuery = new ContextQuery
            {
                Command = @" DELETE  DBO.DECLARACAOAUSENCIA
                            WHERE   ALUNOID = @ALUNOID
                                and TIPODECLARACAOAUSENCIAID <> @TIPODECLARACAOAUSENCIAID "
            };
            contextQuery.Parameters.Add("@ALUNOID", aluno);
            contextQuery.Parameters.Add("@TIPODECLARACAOAUSENCIAID", Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoNecessidadeEspecial));

            context.ApplyModifications(contextQuery);
        }

        public static DataTable Carregar(string alunoId)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT  D.*, NOMEUSUARIO +' ('+ d.MATRICULA + ')' AS NOME_USUARIO
                                FROM    DECLARACAOAUSENCIA d
                                INNER JOIN dbo.USUARIO u ON d.MATRICULA=u.USUARIO
                                WHERE   d.alunoId = @aluno "
                };
                contextQuery.Parameters.Add("@aluno", alunoId);

                return ctx.GetDataTable(contextQuery);
            }
        }
    }
}