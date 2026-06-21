using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class NecessidadeEspecial : RNBase
    {
        #region Propriedades e Enum
        public enum FiltroProcesso
        {
            [StringValue("Processo Seletivo do Contrato Temporário")]
            ProcessoSeletivoContratoTemporario = 1,
            [StringValue("Processo Seletivo Aluno")]
            ProcessoSeletivoAluno = 2
        }
        #endregion

        public DataTable ListaNecessidadeEspecialAtiva()
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable necessidadesEspeciais = null;

            try
            {
                contextQuery.Command = @" SELECT  NE.NECESSIDADEESPECIALID ,
                                                    ITEM ,
                                                    DESCRICAO
                                            FROM    HADES.DBO.NECESSIDADEESPECIAL NE ( NOLOCK )
                                            WHERE   NE.ATIVO = 1 
                                            ORDER BY CASE WHEN NECESSIDADEESPECIALID=30 THEN ' Não possui.' ELSE DESCRICAO END";

                necessidadesEspeciais = ctx.GetDataTable(contextQuery);
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

            return necessidadesEspeciais;
        }

        public DataTable ListaNecessidadeEspecialAtivaHabilitadaPor(int filtroProcessoId)
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable necessidadesEspeciais = null;

            try
            {
                contextQuery.Command = @" SELECT  NE.NECESSIDADEESPECIALID ,
                            ITEM ,
                            DESCRICAO
                    FROM    HADES.DBO.NECESSIDADEESPECIAL NE ( NOLOCK )
                            INNER JOIN HADES.DBO.NECESSIDADEESPECIAL_FILTROPROCESSO NF ( NOLOCK ) ON NE.NECESSIDADEESPECIALID = NF.NECESSIDADEESPECIALID
                                                                                  AND NF.FILTROPROCESSOID = @FILTROPROCESSOID
                    WHERE   NE.ATIVO = 1 ";

                contextQuery.Parameters.Add("@FILTROPROCESSOID", filtroProcessoId);

                necessidadesEspeciais = ctx.GetDataTable(contextQuery);
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

            return necessidadesEspeciais;
        }

        public DataTable ListaNecessidadeEspecialAtivaPor(DbObject filtroProcessoId)
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable necessidadesEspeciais = null;

            try
            {
                if (filtroProcessoId != null && !filtroProcessoId.IsNull)
                {
                    contextQuery.Command = @" SELECT  NE.NECESSIDADEESPECIALID ,
                                                ITEM ,
                                                DESCRICAO ,
                                                COALESCE(( NF.NECESSIDADEESPECIAL_FILTROPROCESSOID
                                                           / NF.NECESSIDADEESPECIAL_FILTROPROCESSOID ), 0) HABILITADO
                                        FROM    HADES.DBO.NECESSIDADEESPECIAL NE ( NOLOCK )
                                                LEFT JOIN HADES.DBO.NECESSIDADEESPECIAL_FILTROPROCESSO NF ( NOLOCK ) ON NE.NECESSIDADEESPECIALID = NF.NECESSIDADEESPECIALID
                                                                                                      AND NF.FILTROPROCESSOID = @FILTROPROCESSOID
                                        WHERE   NE.ATIVO = 1 ";

                    contextQuery.Parameters.Add("@FILTROPROCESSOID", int.Parse(filtroProcessoId.ToString()));

                    necessidadesEspeciais = ctx.GetDataTable(contextQuery);
                }
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

            return necessidadesEspeciais;
        }

        public ValidacaoDados ValidaNecessidadeEspecialFiltroProcesso(DTOs.DadosNecessidadeEspecialFiltroProcesso necessidadeEspecialFiltroProcesso)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            bool existeCadastrado = false;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (necessidadeEspecialFiltroProcesso == null)
            {
                return validacaoDados;
            }

            if (necessidadeEspecialFiltroProcesso.FiltroProcessoId <= 0 || necessidadeEspecialFiltroProcesso.NecessidadeEspecialId <= 0)
            {
                mensagens.Add("ID(s) não encontrado(s).");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromHades.ToFastReadingOnly();

                    //Verifica se a necessidade especial estava cadastrada para o processo
                    existeCadastrado = this.PossuiNecessidadeEspecialFiltroProcessoPor(contexto, necessidadeEspecialFiltroProcesso.FiltroProcessoId, necessidadeEspecialFiltroProcesso.NecessidadeEspecialId);

                    if (existeCadastrado && necessidadeEspecialFiltroProcesso.Habilitado)
                    {
                        mensagens.Add(string.Format("Atualização cancelada, a necessidade especial {0} já se encontra associada ao filtro {1}.", necessidadeEspecialFiltroProcesso.NecessidadeEspecialDescricao, necessidadeEspecialFiltroProcesso.FiltroProcessoDescricao));
                    }

                    if (!existeCadastrado && !necessidadeEspecialFiltroProcesso.Habilitado)
                    {
                        mensagens.Add(string.Format("Atualização cancelada, a necessidade especial {0} não se encontra associada ao filtro {1}.", necessidadeEspecialFiltroProcesso.NecessidadeEspecialDescricao, necessidadeEspecialFiltroProcesso.FiltroProcessoDescricao));
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiNecessidadeEspecialFiltroProcessoPor(DataContext ctx, int filtroProcessoId, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @"SELECT  COUNT(*)
                                        FROM    HADES.DBO.NECESSIDADEESPECIAL_FILTROPROCESSO (NOLOCK)
                                        WHERE   NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID
                                                AND FILTROPROCESSOID = @FILTROPROCESSOID ";

                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", necessidadeEspecialId);
                contextQuery.Parameters.Add("@FILTROPROCESSOID", filtroProcessoId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public void SalvaNecessidadeEspecialFiltroProcesso(DTOs.DadosNecessidadeEspecialFiltroProcesso necessidadeEspecialFiltroProcesso)
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingLock();

            try
            {
                if (necessidadeEspecialFiltroProcesso.Habilitado)
                {
                    this.InsereNecessidadeEspecialFiltroProcesso(ctx, necessidadeEspecialFiltroProcesso.FiltroProcessoId, necessidadeEspecialFiltroProcesso.NecessidadeEspecialId);
                }
                else
                {
                    this.RemoveNecessidadeEspecialFiltroProcesso(ctx, necessidadeEspecialFiltroProcesso.FiltroProcessoId, necessidadeEspecialFiltroProcesso.NecessidadeEspecialId);
                }
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

        private void RemoveNecessidadeEspecialFiltroProcesso(DataContext ctx, int filtroProcessoId, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  FROM HADES.DBO.NECESSIDADEESPECIAL_FILTROPROCESSO
                                            WHERE   NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID
                                                    AND FILTROPROCESSOID = @FILTROPROCESSOID ";

                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", necessidadeEspecialId);
                contextQuery.Parameters.Add("@FILTROPROCESSOID", filtroProcessoId);

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

        private void InsereNecessidadeEspecialFiltroProcesso(DataContext ctx, int filtroProcessoId, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO HADES.DBO.NECESSIDADEESPECIAL_FILTROPROCESSO
                                                ( NECESSIDADEESPECIALID ,
                                                  FILTROPROCESSOID
                                                )
                                        VALUES  ( @NECESSIDADEESPECIALID ,
                                                  @FILTROPROCESSOID
                                                ) ";

                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", necessidadeEspecialId);
                contextQuery.Parameters.Add("@FILTROPROCESSOID", filtroProcessoId);

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

        public bool PossuiNecessidadeEspecialFiltroProcessoPor(int filtroProcessoId, string necessidadeEspecial)
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @"SELECT  COUNT(*)
                                        FROM    HADES.DBO.NECESSIDADEESPECIAL_FILTROPROCESSO  NF(NOLOCK)
                                        INNER JOIN  HADES.DBO.NECESSIDADEESPECIAL NE ( NOLOCK ) ON NE.NECESSIDADEESPECIALID = NF.NECESSIDADEESPECIALID
                                                                                  AND NF.FILTROPROCESSOID = @FILTROPROCESSOID
                                        WHERE   ITEM = @ITEM
                                                AND ATIVO = 1
                                                ";

                contextQuery.Parameters.Add("@ITEM", necessidadeEspecial);
                contextQuery.Parameters.Add("@FILTROPROCESSOID", filtroProcessoId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public bool NecessitaLedorPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                            FROM   LY_ALUNO A (NOLOCK) 
                                                    INNER JOIN LY_PESSOA P (NOLOCK) 
                                                            ON A.PESSOA = P.PESSOA
		                                            INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                            ON P.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                            WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                            AND A.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool NecessitaCuidadorPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                            FROM   LY_ALUNO A (NOLOCK) 
                                                    INNER JOIN LY_PESSOA P (NOLOCK) 
                                                            ON A.PESSOA = P.PESSOA
		                                            INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                            ON P.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                            WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                            AND A.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool NecessitaInterpretePor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                            FROM   LY_ALUNO A (NOLOCK) 
                                                    INNER JOIN LY_PESSOA P (NOLOCK) 
                                                            ON A.PESSOA = P.PESSOA
		                                            INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                            ON P.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                            WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                            AND A.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool NecessitaInterpretePor(DataContext ctx, string turma, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                        FROM   LY_ALUNO A (NOLOCK) 
                                                INNER JOIN LY_PESSOA P (NOLOCK) 
                                                        ON A.PESSOA = P.PESSOA
		                                        INNER JOIN LY_MATRICULA M (NOLOCK)
				                                        ON M.ALUNO = A.ALUNO				
		                                        INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                        ON P.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                        WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                        AND M.SIT_MATRICULA = @SIT_MATRICULA
	                                        AND M.ANO = @ANO
	                                        AND M.SEMESTRE = @SEMESTRE
	                                        AND M.TURMA = @TURMA  ";

                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete);
                contextQuery.Parameters.Add("@SIT_MATRICULA", TechneDbType.T_SIT_MATRICULA, RN.Matricula.Matriculado);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool NecessitaLedorPor(DataContext ctx, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                    FROM   HADES.DBO.NECESSIDADEESPECIAL N (NOLOCK) 
		                                    INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                    ON N.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                    WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                    AND N.NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID  ";

                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, necessidadeEspecialId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool NecessitaCuidadorPor(DataContext ctx, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                    FROM   HADES.DBO.NECESSIDADEESPECIAL N (NOLOCK) 
		                                    INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                    ON N.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                    WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                    AND N.NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID  ";

                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, necessidadeEspecialId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool NecessitaInterpretePor(DataContext ctx, int necessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                    FROM   HADES.DBO.NECESSIDADEESPECIAL N (NOLOCK) 
		                                    INNER JOIN HADES.NecessidadeEspecial.NECESSIDADEESPECIAL__TIPORECURSONECESSIDADEESPECIAL NT (NOLOCK) 
				                                    ON N.NECESSIDADEESPECIALID = NT.NECESSIDADEESPECIALID
                                    WHERE NT.TIPORECURSONECESSIDADEESPECIALID = @TIPO
	                                    AND N.NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID  ";

                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, necessidadeEspecialId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.Int, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public int ObtemNecessidadeEspecialIdPor(DataContext contexto, string item)
        { 
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT NECESSIDADEESPECIALID 
                                        FROM HADES.DBO.NECESSIDADEESPECIAL (NOLOCK)
                                        WHERE ITEM = @ITEM ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, item); 

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }
    }
}