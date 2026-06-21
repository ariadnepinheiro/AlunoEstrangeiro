using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class TipoEvento : RNBase
    {
        #region Propriedades e Enum
        public enum TipoEventoAgenda
        {
            [StringValue("Confirmação de Matrícula")]
            ConfirmacaoMatricula = 1,
            [StringValue("Turnos e Vagas")]
            ConfirmacaoTurnosVagas = 2,
            [StringValue("Processo Seletivo de Alunos")]
            ProcessoSeletivoAluno = 3,
            [StringValue("Inscrições Processo Seletivo")]
            InscricaoProcessoSeletivo = 4,
            [StringValue("Renovação de Matrícula")]
            RenovacaoMatricula = 5,
            [StringValue("Inscrição de Alunos das Compartilhadas")]
            InscricaoAlunosCompartilhadas = 6,
            [StringValue("Matrícula Fácil (SISMATI)")]
            MatriculaFacil = 7,
            [StringValue("Fechamento Ano Letivo e Matrícula")]
            FechamentoAnoLetivoMatrícula = 8,
            [StringValue("Confirmação de Turnos")]
            ConfirmacaoTurnos = 10,
            [StringValue("Confirmação de Vagas")]
            ConfirmacaoVagas = 13,
            [StringValue("Reabertura de Matrícula")]
            ReaberturaMatricula = 15,
            [StringValue("Bloqueio de Novo Cadastro de Matrícula")]
            BloqueioCadastroMatricula = 16,
            [StringValue("Bloqueio de Tranferência de Turmas")]
            BloqueioTranferenciaTurma = 17,
            [StringValue("Análise de Turnos")]
            AnaliseTurnos = 18,
            [StringValue("Análise de Vagas")]
            AnaliseVagas = 19,
            [StringValue("Bloqueio de Tranferência de Unidades")]
            BloqueioTransferenciaUnidade = 20,
            [StringValue("Liberação de Confirmação de Matrícula")]
            LiberacaoConfirmacaoMatricula = 21,
            [StringValue("Bloqueio de Encerramento de Aluno")]
            BloqueioEncerramentoAluno = 22            
        }
        #endregion

        public static DataTable ListaTipoEvento()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT TIPOEVENTOID ,
                                                 SISTEMICO    ,
                                                 ATIVO        ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID    ,
                                                 NOME          
                                            FROM agenda.TIPOEVENTO     
                                        ORDER BY TIPOEVENTOID ";

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static DataTable ListaTipoEventoAtivoPorSistema(int Sistemico)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT TIPOEVENTOID ,
                                                 SISTEMICO    ,
                                                 ATIVO        ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID    ,
                                                 NOME          
                                            FROM agenda.TIPOEVENTO 
                                           WHERE SISTEMICO = @SISTEMICO  
                                             AND ATIVO     = 1
                                        ORDER BY TIPOEVENTOID ";

                contextQuery.Parameters.Add("@SISTEMICO", Sistemico);

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static DataTable ListaTipoEventoPaiAtivo(int Sistemico)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT TE.TIPOEVENTOID ,
                                                 TE.SISTEMICO    ,
                                                 TE.ATIVO        ,
                                                 TE.DATACADASTRO ,
                                                 TE.DATAALTERACAO,
                                                 TE.USUARIOID    ,
                                                 TE.NOME          
                                            FROM agenda.TIPOEVENTO TE
                                           WHERE TE.SISTEMICO      = @SISTEMICO  
                                             AND TE.ATIVO          = 1
                                             AND NOT EXISTS(SELECT 1
                                                              FROM agenda.TIPOEVENTOCORRELATO
                                                             WHERE TIPOEVENTOID = TE.TIPOEVENTOID)
                                        ORDER BY TE.TIPOEVENTOID ";

                contextQuery.Parameters.Add("@SISTEMICO", Sistemico);
                
                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static DataTable ListaTipoEventoAtivoPorTipoEventoPai(int Sistemico, int TipoEventoPaiId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT TE.TIPOEVENTOID ,
                                                 TE.SISTEMICO    ,
                                                 TE.ATIVO        ,
                                                 TE.DATACADASTRO ,
                                                 TE.DATAALTERACAO,
                                                 TE.USUARIOID    ,
                                                 TE.NOME          
                                            FROM agenda.TIPOEVENTOCORRELATO TEC
                                           INNER JOIN agenda.TIPOEVENTO     TE
                                              ON TE.TIPOEVENTOID             = TEC.TIPOEVENTOID
                                           WHERE TEC.TIPOEVENTOPAIID = @TIPOEVENTOPAIID
                                             AND TE.SISTEMICO        = @SISTEMICO  
                                             AND TE.ATIVO            = 1
                                        ORDER BY TE.TIPOEVENTOID ";

                contextQuery.Parameters.Add("@TIPOEVENTOPAIID", TipoEventoPaiId);
                contextQuery.Parameters.Add("@SISTEMICO", Sistemico);

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static void InsereTipoEvento(Entidades.TipoEvento TipoEvento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO agenda.TIPOEVENTO( 
                                        SISTEMICO    ,
                                        ATIVO        ,
                                        DATACADASTRO ,
                                        DATAALTERACAO,
                                        USUARIOID    ,
                                        NOME          
                                    ) VALUES ( @SISTEMICO    ,
                                               @ATIVO        ,
                                               @DATACADASTRO ,
                                               @DATAALTERACAO,
                                               @USUARIOID    ,
                                               @NOME             
                                             )"
                };

                contextQuery.Parameters.Add("@SISTEMICO", TipoEvento.Sistemico);
                contextQuery.Parameters.Add("@ATIVO", TipoEvento.Ativo);
                contextQuery.Parameters.Add("@DATACADASTRO", TipoEvento.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", TipoEvento.DataAlteracao);
                contextQuery.Parameters.Add("@USUARIOID", TipoEvento.UsuarioId.Trim());
                contextQuery.Parameters.Add("@NOME", TipoEvento.Nome.Trim());

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void AlteraTipoEvento(Entidades.TipoEvento TipoEvento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE agenda.TIPOEVENTO
                                   SET SISTEMICO     = @SISTEMICO    ,
                                       ATIVO         = @ATIVO        ,
                                       DATACADASTRO  = @DATACADASTRO ,
                                       DATAALTERACAO = @DATAALTERACAO,
                                       USUARIOID     = @USUARIOID    ,
                                       NOME          = @NOME         
                                 WHERE TIPOEVENTOID  = @TIPOEVENTOID "
                };

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEvento.TipoEventoId);
                contextQuery.Parameters.Add("@SISTEMICO", TipoEvento.Sistemico);
                contextQuery.Parameters.Add("@ATIVO", TipoEvento.Ativo);
                contextQuery.Parameters.Add("@DATACADASTRO", TipoEvento.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", TipoEvento.DataAlteracao);
                contextQuery.Parameters.Add("@USUARIOID", TipoEvento.UsuarioId.Trim());
                contextQuery.Parameters.Add("@NOME", TipoEvento.Nome.Trim());

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void RemoveTipoEvento(int TipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE agenda.TIPOEVENTO
                                  WHERE TIPOEVENTOID = @TIPOEVENTOID "
                };

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
    }
}
