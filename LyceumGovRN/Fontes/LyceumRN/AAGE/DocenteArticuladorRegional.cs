using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.AAGE
{
    public class DocenteArticuladorRegional
    {
        public DataTable ListaVinculoDocenteArticuladorRegionalPor(decimal docenteId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable vinculos = null;
            DataColumn colunaTipoFuncao = new DataColumn();
            string tipoFuncao = RN.TipoFuncao.EnumTipoFuncao.MediadorArticulador.GetStringValue();

            try
            {
                contextQuery.Command = @" SELECT  DA.DOCENTEARTICULADOR_REGIONAL_ID ,
                                                DA.DOCENTEID ,
                                                DA.REGIONALID ,
                                                [DATAINICIO_VINCULO] ,
                                                [DATAFIM_VINCULO] ,
                                                [USUARIOID] ,
                                                [DATACADASTRO] ,
                                                [DATAALTERACAO] ,
                                                R.REGIONAL
                                        FROM    AAGE.DOCENTEARTICULADOR_REGIONAL DA ( NOLOCK )
                                                INNER JOIN DBO.TCE_REGIONAL R ( NOLOCK ) ON DA.REGIONALID = R.ID_REGIONAL
                                        WHERE   DA.DOCENTEID = @DOCENTEID ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteId);

                vinculos = ctx.GetDataTable(contextQuery);

                colunaTipoFuncao.ColumnName = "TIPOFUNCAO";
                colunaTipoFuncao.AllowDBNull = false;
                colunaTipoFuncao.DataType = System.Type.GetType("System.String");
                colunaTipoFuncao.DefaultValue = tipoFuncao;
                vinculos.Columns.Add(colunaTipoFuncao);
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
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return vinculos;
        }

        public void Insere(AAGE.Entidades.DocenteArticuladorRegional docenteArticuladorRegional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO AAGE.DOCENTEARTICULADOR_REGIONAL
                                            ( DOCENTEID ,
                                              REGIONALID ,
                                              DATAINICIO_VINCULO ,
                                              DATAFIM_VINCULO ,
                                              USUARIOID ,
                                              DATACADASTRO ,
                                              DATAALTERACAO
                                            )
                                    VALUES  ( @DOCENTEID ,
                                              @REGIONALID ,
                                              @DATAINICIO_VINCULO ,
                                              @DATAFIM_VINCULO ,
                                              @USUARIOID ,
                                              @DATACADASTRO ,
                                              @DATAALTERACAO
                                            ) ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteArticuladorRegional.DocenteId);
                contextQuery.Parameters.Add("@REGIONALID", docenteArticuladorRegional.RegionalId);
                contextQuery.Parameters.Add("@DATAINICIO_VINCULO", docenteArticuladorRegional.DataInicioVinculo);
                contextQuery.Parameters.Add("@DATAFIM_VINCULO", docenteArticuladorRegional.DataFimVinculo);
                contextQuery.Parameters.Add("@USUARIOID", docenteArticuladorRegional.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public void Atualiza(AAGE.Entidades.DocenteArticuladorRegional docenteArticuladorRegional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  AAGE.DOCENTEARTICULADOR_REGIONAL
                                        SET     DOCENTEID = @DOCENTEID ,
                                                REGIONALID = @REGIONALID ,
                                                DATAINICIO_VINCULO = @DATAINICIO_VINCULO ,
                                                DATAFIM_VINCULO = @DATAFIM_VINCULO ,
                                                USUARIOID = @USUARIOID ,
                                                DATAALTERACAO = @DATAALTERACAO
                                        WHERE   DOCENTEARTICULADOR_REGIONAL_ID = @DOCENTEARTICULADOR_REGIONAL_ID ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteArticuladorRegional.DocenteId);
                contextQuery.Parameters.Add("@REGIONALID", docenteArticuladorRegional.RegionalId);
                contextQuery.Parameters.Add("@DATAINICIO_VINCULO", docenteArticuladorRegional.DataInicioVinculo);
                contextQuery.Parameters.Add("@DATAFIM_VINCULO", docenteArticuladorRegional.DataFimVinculo);
                contextQuery.Parameters.Add("@USUARIOID", docenteArticuladorRegional.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
                contextQuery.Parameters.Add("@DOCENTEARTICULADOR_REGIONAL_ID", docenteArticuladorRegional.DocenteArticuladorRegionalId);

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

        public void Remove(int docenteArticuladorRegionalId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE AAGE.DOCENTEARTICULADOR_REGIONAL
                                        WHERE  DOCENTEARTICULADOR_REGIONAL_ID = @DOCENTEARTICULADOR_REGIONAL_ID ";

                contextQuery.Parameters.Add("@DOCENTEARTICULADOR_REGIONAL_ID", docenteArticuladorRegionalId);

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

        public bool PossuiOutroVinculoAtivoPor(DataContext ctx, decimal docenteId, DateTime dataInicio, DateTime dataFim, int idAtual)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    AAGE.DOCENTEARTICULADOR_REGIONAL DR ( NOLOCK )
                                        WHERE   DR.DOCENTEID = @DOCENTEID
                                                AND ( ( @DATAINICIO BETWEEN DR.DATAINICIO_VINCULO
                                                                    AND     DR.DATAFIM_VINCULO )
                                                      OR ( @DATAFIM BETWEEN DR.DATAINICIO_VINCULO
                                                                    AND     DR.DATAFIM_VINCULO )
                                                    )
                                                AND DR.DOCENTEARTICULADOR_REGIONAL_ID <> @DOCENTEARTICULADOR_REGIONAL_ID ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteId);
                contextQuery.Parameters.Add("@DATAINICIO", dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@DOCENTEARTICULADOR_REGIONAL_ID", idAtual);

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

        public ValidacaoDados Valida(AAGE.Entidades.DocenteArticuladorRegional docenteArticuladorRegional)
        {
            RN.AAGE.DocenteAAGEUnidadeEnsino rnDocenteAAGEUnidadeEnsino = new DocenteAAGEUnidadeEnsino();
            RN.AAGE.DocenteMediadorUnidadeEnsino rnDocenteMediadorUnidadeEnsino = new DocenteMediadorUnidadeEnsino();
            RN.TipoFuncao rnTipoFuncao = new TipoFuncao();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int tipoFuncao = (int)RN.TipoFuncao.EnumTipoFuncao.MediadorArticulador;
            string tipoFuncaoDescricao = RN.TipoFuncao.EnumTipoFuncao.MediadorArticulador.GetStringValue();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteArticuladorRegional == null)
            {
                return validacaoDados;
            }

            if (docenteArticuladorRegional.DocenteId <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (docenteArticuladorRegional.RegionalId <= 0)
            {
                mensagens.Add("Campo REGIONAL é obrigatório.");
            }

            if (docenteArticuladorRegional.DataInicioVinculo == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }

            if (docenteArticuladorRegional.DataFimVinculo == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (docenteArticuladorRegional.DataFimVinculo < docenteArticuladorRegional.DataInicioVinculo)
            {
                mensagens.Add("Campo DATA FIM não pode ser menor que a DATA INÍCIO.");
            }

            if (string.IsNullOrEmpty(docenteArticuladorRegional.UsuarioId))
            {
                 mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o docente possui ao menos uma funcao do tipo MediadorDeTecnologia = 2
                    if (!rnTipoFuncao.PossuiTipoFuncaoEmLotacaoAtivaPor(contexto, tipoFuncao, docenteArticuladorRegional.DocenteId))
                    {
                        mensagens.Add(string.Format("Só é permitido o vínculo caso o docente possua uma função do tipo {0} nesta matrícula.", tipoFuncaoDescricao));
                    }

                    //Verifica se o docente já atua como Mediador Articulador no período vigente
                    if (this.PossuiOutroVinculoAtivoPor(contexto, docenteArticuladorRegional.DocenteId, docenteArticuladorRegional.DataInicioVinculo, docenteArticuladorRegional.DataFimVinculo, docenteArticuladorRegional.DocenteArticuladorRegionalId))
                    {
                        mensagens.Add("Este docente já atua como Mediador Articulador em uma regional no período vigente.");
                    }

                    //Verifica se o docente já atua como Mediador de tecnologia no período vigente
                    if (rnDocenteMediadorUnidadeEnsino.PossuiVinculoAtivoPor(contexto, docenteArticuladorRegional.DocenteId, docenteArticuladorRegional.DataInicioVinculo, docenteArticuladorRegional.DataFimVinculo))
                    {
                        mensagens.Add("Este docente já atua como Mediador de tecnologia no período vigente.");
                    }

                    //Verifica se o docente já atua como AAGE no período vigente
                    if (rnDocenteAAGEUnidadeEnsino.PossuiVinculoAtivoPor(contexto, docenteArticuladorRegional.DocenteId, docenteArticuladorRegional.DataInicioVinculo, docenteArticuladorRegional.DataFimVinculo))
                    {
                        mensagens.Add("Este docente já atua como AAGE no período vigente.");
                    }                   

                    //Verificar se a unidade ja esta vinculada a outro docente
                    if (this.PossuiOutroDocenteArticuladosAtivoPor(contexto, docenteArticuladorRegional.RegionalId, docenteArticuladorRegional.DataInicioVinculo, docenteArticuladorRegional.DataFimVinculo, docenteArticuladorRegional.DocenteId))
                    {
                        mensagens.Add("Esta Regional já possui um Mediador Articulador vigente.");
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

        public bool PossuiVinculoAtivoPor(DataContext ctx, decimal docenteId, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    AAGE.DOCENTEARTICULADOR_REGIONAL DU  ( NOLOCK )
                                            WHERE   DU.DOCENTEID = @DOCENTEID
                                                    AND ( ( @DATAINICIO BETWEEN DU.DATAINICIO_VINCULO
                                                                        AND     DU.DATAFIM_VINCULO )
                                                          OR ( @DATAFIM BETWEEN DU.DATAINICIO_VINCULO
                                                                        AND     DU.DATAFIM_VINCULO )
                                                        ) ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteId);
                contextQuery.Parameters.Add("@DATAINICIO", dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);

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

        public bool PossuiOutroDocenteArticuladosAtivoPor(DataContext ctx, int regionalId, DateTime dataInicio, DateTime dataFim, decimal docenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    AAGE.DOCENTEARTICULADOR_REGIONAL DU ( NOLOCK )
                                        WHERE   du.REGIONALID = @REGIONALID
                                                AND ( ( @DATAINICIO BETWEEN DU.DATAINICIO_VINCULO
                                                                    AND     DU.DATAFIM_VINCULO )
                                                      OR ( @DATAFIM BETWEEN DU.DATAINICIO_VINCULO
                                                                    AND     DU.DATAFIM_VINCULO )
                                                    )
                                                AND DU.DOCENTEID <> @DOCENTEID ";

                contextQuery.Parameters.Add("@REGIONALID", regionalId);
                contextQuery.Parameters.Add("@DATAINICIO", dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@DOCENTEID", docenteId);

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
    }
}
