using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data;

namespace Techne.Lyceum.RN.AAGE
{
    public class DocenteAAGEUnidadeEnsino
    {
        public DataTable ListaVinculoDocenteAAGEUnidadeEnsinoPor(decimal docenteId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable vinculos = null;
            DataColumn colunaTipoFuncao = new DataColumn();
            string tipoFuncao = RN.TipoFuncao.EnumTipoFuncao.AAGE.GetStringValue();

            try
            {
                contextQuery.Command = @" SELECT  DOCENTEAAGE_UNIDADEENSINO_ID ,
                                                DOCENTEID ,
                                                UNIDADEENSINOID ,
                                                DATAINICIO_VINCULO ,
                                                DATAFIM_VINCULO ,
                                                USUARIOID ,
                                                DATACADASTRO ,
                                                DATAALTERACAO ,
                                                NOME_COMP AS ESCOLA
                                        FROM    AAGE.DOCENTEAAGE_UNIDADEENSINO DM ( NOLOCK )
                                                INNER JOIN LY_UNIDADE_ENSINO UE ( NOLOCK ) ON UE.UNIDADE_ENS = DM.UNIDADEENSINOID
                                        WHERE   DM.DOCENTEID = @DOCENTEID  ";

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

        public DataTable ListaDocentePor(object unidadeEnsinoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable docentes = null;

            try
            {
                contextQuery.Command = @" SELECT D.NUM_FUNC, 
                                               L.MATRICULA, 
                                               PE.NOME_COMPL, 
                                               LF.DESCRICAO, 
                                               PE.FONE, 
                                               PE.CELULAR, 
                                               PE.E_MAIL_INTERNO 
                                        FROM   LY_LOTACAO L ( NOLOCK )
                                               INNER JOIN LY_DOCENTE D ( NOLOCK )
                                                       ON L.MATRICULA = D.MATRICULA 
                                               INNER JOIN LY_PESSOA PE ( NOLOCK )
                                                       ON PE.PESSOA = D.PESSOA
                                               INNER JOIN AAGE.DOCENTEAAGE_UNIDADEENSINO DI ( NOLOCK )
                                                       ON D.NUM_FUNC = DI.DOCENTEID 
                                               INNER JOIN LY_FUNCAO LF ( NOLOCK )
                                                       ON L.FUNCAO = LF.FUNCAO 
                                               INNER JOIN FUNCAO F ( NOLOCK )
                                                       ON LF.FUNCAO = F.FUNCAOID 
                                        WHERE  F.TIPOFUNCAOID = 1 
                                               AND DI.UNIDADEENSINOID = @UNIDADEENSINOID 
                                               AND GETDATE() BETWEEN DI.DATAINICIO_VINCULO AND DI.DATAFIM_VINCULO 
                                               AND ( GETDATE() BETWEEN L.DATA_NOMEACAO AND L.DATA_DESATIVACAO 
                                                      OR L.DATA_DESATIVACAO IS NULL )   ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", Convert.ToString(unidadeEnsinoId));

                docentes = ctx.GetDataTable(contextQuery);
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

            return docentes;
        }

        public void Insere(AAGE.Entidades.DocenteAAGEUnidadeEnsino docenteAAGEUnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO AAGE.DOCENTEAAGE_UNIDADEENSINO
                                        ( DOCENTEID ,
                                          UNIDADEENSINOID ,
                                          DATAINICIO_VINCULO ,
                                          DATAFIM_VINCULO ,
                                          USUARIOID ,
                                          DATACADASTRO ,
                                          DATAALTERACAO
                                        )
                                VALUES  ( @DOCENTEID ,
                                          @UNIDADEENSINOID ,
                                          @DATAINICIO_VINCULO ,
                                          @DATAFIM_VINCULO ,
                                          @USUARIOID ,
                                          @DATACADASTRO ,
                                          @DATAALTERACAO
                                        ) ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteAAGEUnidadeEnsino.DocenteId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", docenteAAGEUnidadeEnsino.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@DATAINICIO_VINCULO", docenteAAGEUnidadeEnsino.DataInicioVinculo);
                contextQuery.Parameters.Add("@DATAFIM_VINCULO", docenteAAGEUnidadeEnsino.DataFimVinculo);
                contextQuery.Parameters.Add("@USUARIOID", docenteAAGEUnidadeEnsino.UsuarioId);
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

        public void Atualiza(AAGE.Entidades.DocenteAAGEUnidadeEnsino docenteAAGEUnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  AAGE.DOCENTEAAGE_UNIDADEENSINO
                                SET     DOCENTEID = @DOCENTEID ,
                                        UNIDADEENSINOID = @UNIDADEENSINOID ,
                                        DATAINICIO_VINCULO = @DATAINICIO_VINCULO ,
                                        DATAFIM_VINCULO = @DATAFIM_VINCULO ,
                                        USUARIOID = @USUARIOID ,
                                        DATAALTERACAO = @DATAALTERACAO
                                WHERE   DOCENTEAAGE_UNIDADEENSINO_ID = @DOCENTEAAGE_UNIDADEENSINO_ID ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteAAGEUnidadeEnsino.DocenteId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", docenteAAGEUnidadeEnsino.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@DATAINICIO_VINCULO", docenteAAGEUnidadeEnsino.DataInicioVinculo);
                contextQuery.Parameters.Add("@DATAFIM_VINCULO", docenteAAGEUnidadeEnsino.DataFimVinculo);
                contextQuery.Parameters.Add("@USUARIOID", docenteAAGEUnidadeEnsino.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
                contextQuery.Parameters.Add("@DOCENTEAAGE_UNIDADEENSINO_ID", docenteAAGEUnidadeEnsino.DocenteAAGEUnidadeEnsinoId);

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

        public void Remove(int docenteAAGEUnidadeEnsinoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE  AAGE.DOCENTEAAGE_UNIDADEENSINO
                                          WHERE   DOCENTEAAGE_UNIDADEENSINO_ID = @DOCENTEAAGE_UNIDADEENSINO_ID ";

                contextQuery.Parameters.Add("@DOCENTEAAGE_UNIDADEENSINO_ID", docenteAAGEUnidadeEnsinoId);

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

        //        public bool PossuiOutroVinculoAtivoPor(DataContext ctx, decimal docenteId, DateTime dataInicio, DateTime dataFim, int idAtual)
        //        {
        //            ContextQuery contextQuery = new ContextQuery();
        //            bool possui = false;

        //            try
        //            {
        //                contextQuery.Command = @" SELECT  COUNT(*)
        //                                        FROM    AAGE.DOCENTEAAGE_UNIDADEENSINO DU  ( NOLOCK )
        //                                        WHERE   DU.DOCENTEID = @DOCENTEID
        //                                                AND ( ( @DATAINICIO BETWEEN DU.DATAINICIO_VINCULO
        //                                                                    AND     DU.DATAFIM_VINCULO )
        //                                                      OR ( @DATAFIM BETWEEN DU.DATAINICIO_VINCULO
        //                                                                    AND     DU.DATAFIM_VINCULO )
        //                                                    )
        //                                                AND du.DOCENTEAAGE_UNIDADEENSINO_ID <> @DOCENTEAAGE_UNIDADEENSINO_ID ";

        //                contextQuery.Parameters.Add("@DOCENTEID", docenteId);
        //                contextQuery.Parameters.Add("@DATAINICIO", dataInicio);
        //                contextQuery.Parameters.Add("@DATAFIM", dataFim);
        //                contextQuery.Parameters.Add("@DOCENTEAAGE_UNIDADEENSINO_ID", idAtual);

        //                if (ctx.GetReturnValue<int>(contextQuery) > 0)
        //                {
        //                    possui = true;
        //                }

        //                return possui;
        //            }
        //            catch (Exception ex)
        //            {
        //                string mensagem = string.Empty;
        //                ctx.Abandon();
        //                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
        //                {
        //                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
        //                        Environment.NewLine,
        //                        Convert.ToString(ex.Message));
        //                }
        //                else
        //                {
        //                    mensagem = Convert.ToString(ex.Message);
        //                }
        //                throw new Exception(mensagem);
        //            }
        //        }

        public ValidacaoDados Valida(AAGE.Entidades.DocenteAAGEUnidadeEnsino docenteAAGEUnidadeEnsino)
        {
            RN.AAGE.DocenteMediadorUnidadeEnsino rnDocenteMediadorUnidadeEnsino = new DocenteMediadorUnidadeEnsino();
            RN.AAGE.DocenteArticuladorRegional rnDocenteArticuladorRegional = new DocenteArticuladorRegional();
            RN.TipoFuncao rnTipoFuncao = new TipoFuncao();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int censo = 0;
            int tipoFuncao = (int)RN.TipoFuncao.EnumTipoFuncao.AAGE;
            string tipoFuncaoDescricao = RN.TipoFuncao.EnumTipoFuncao.AAGE.GetStringValue();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteAAGEUnidadeEnsino == null)
            {
                return validacaoDados;
            }

            if (docenteAAGEUnidadeEnsino.DocenteId <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (string.IsNullOrEmpty(docenteAAGEUnidadeEnsino.UnidadeEnsinoId))
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }
            else if ((docenteAAGEUnidadeEnsino.UnidadeEnsinoId.Length != 8) || !int.TryParse(docenteAAGEUnidadeEnsino.UnidadeEnsinoId, out censo))
            {
                mensagens.Add("Campo UNIDADE DE ENSINO deve ser composto por 8 dígitos.");
            }

            if (docenteAAGEUnidadeEnsino.DataInicioVinculo == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }

            if (docenteAAGEUnidadeEnsino.DataFimVinculo == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (docenteAAGEUnidadeEnsino.DataFimVinculo < docenteAAGEUnidadeEnsino.DataInicioVinculo)
            {
                mensagens.Add("Campo DATA FIM não pode ser menor que a DATA INÍCIO.");
            }

            if (string.IsNullOrEmpty(docenteAAGEUnidadeEnsino.UsuarioId))
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o docente possui ao menos uma funcao do tipo AAGE = 1
                    if (!rnTipoFuncao.PossuiTipoFuncaoEmLotacaoAtivaPor(contexto, tipoFuncao, docenteAAGEUnidadeEnsino.DocenteId))
                    {
                        mensagens.Add(string.Format("Só é permitido o vínculo caso o docente possua uma função do tipo {0} nesta matrícula.", tipoFuncaoDescricao));
                    }

                    //Verifica se já existe o mesmo vinculo cadastrado
                    if (this.PossuiOutroVinculoPor(contexto, docenteAAGEUnidadeEnsino))
                    {
                        mensagens.Add("Vínculo já cadastrado anteriormente.");
                    }

                    ////Verifica se o docente já atua como AAGE no período vigente
                    //if (this.PossuiOutroVinculoAtivoPor(contexto, docenteAAGEUnidadeEnsino.DocenteId, docenteAAGEUnidadeEnsino.DataInicioVinculo, docenteAAGEUnidadeEnsino.DataFimVinculo, docenteAAGEUnidadeEnsino.docenteAAGEUnidadeEnsinoId))
                    //{
                    //    mensagens.Add("Este docente já atua como AAGE no período vigente.");
                    //}

                    //Verifica se o docente já atua como Mediador de tecnologia no período vigente
                    if (rnDocenteMediadorUnidadeEnsino.PossuiVinculoAtivoPor(contexto, docenteAAGEUnidadeEnsino.DocenteId, docenteAAGEUnidadeEnsino.DataInicioVinculo, docenteAAGEUnidadeEnsino.DataFimVinculo))
                    {
                        mensagens.Add("Este docente já atua como Mediador de tecnologia no período vigente.");
                    }                  

                    //Verifica se o docente já atua como Mediador Articulador no período vigente
                    if (rnDocenteArticuladorRegional.PossuiVinculoAtivoPor(contexto, docenteAAGEUnidadeEnsino.DocenteId, docenteAAGEUnidadeEnsino.DataInicioVinculo, docenteAAGEUnidadeEnsino.DataFimVinculo))
                    {
                        mensagens.Add("Este docente já atua como Mediador Articulador em uma regional no período vigente.");
                    }

                    //Verificar se a unidade ja esta vinculada a outro docente
                    if (this.PossuiOutroDocenteAAGEAtivoPor(contexto, docenteAAGEUnidadeEnsino.UnidadeEnsinoId, docenteAAGEUnidadeEnsino.DataInicioVinculo, docenteAAGEUnidadeEnsino.DataFimVinculo, docenteAAGEUnidadeEnsino.DocenteId))
                    {
                        mensagens.Add("A Unidade de Ensino já está vinculada a outro AAGE para o período vigente");
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
                                            FROM    AAGE.DOCENTEAAGE_UNIDADEENSINO DU  ( NOLOCK )
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

        public bool PossuiOutroVinculoPor(DataContext ctx, AAGE.Entidades.DocenteAAGEUnidadeEnsino docenteAAGEUnidadeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    AAGE.DOCENTEAAGE_UNIDADEENSINO DU  ( NOLOCK )
                                            WHERE   DU.DOCENTEID = @DOCENTEID
				                                    AND DU.UNIDADEENSINOID = @UNIDADEENSINOID
                                                    AND DU.DATAINICIO_VINCULO = @DATAINICIO 
                                                    AND DU.DATAFIM_VINCULO = @DATAFIM
                                                    AND DU.DOCENTEAAGE_UNIDADEENSINO_ID <> @DOCENTEAAGE_UNIDADEENSINO_ID ";

                contextQuery.Parameters.Add("@DOCENTEID", docenteAAGEUnidadeEnsino.DocenteId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", docenteAAGEUnidadeEnsino.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@DATAINICIO", docenteAAGEUnidadeEnsino.DataInicioVinculo);
                contextQuery.Parameters.Add("@DATAFIM", docenteAAGEUnidadeEnsino.DataFimVinculo);
                contextQuery.Parameters.Add("@DOCENTEAAGE_UNIDADEENSINO_ID", docenteAAGEUnidadeEnsino.DocenteAAGEUnidadeEnsinoId);

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

        public bool PossuiOutroDocenteAAGEAtivoPor(DataContext ctx, string unidadeEnsinoId, DateTime dataInicio, DateTime dataFim, decimal docenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    AAGE.DOCENTEAAGE_UNIDADEENSINO DU ( NOLOCK )
                                        WHERE   du.UNIDADEENSINOID = @UNIDADEENSINOID
                                                AND ( ( @DATAINICIO BETWEEN DU.DATAINICIO_VINCULO
                                                                    AND     DU.DATAFIM_VINCULO )
                                                      OR ( @DATAFIM BETWEEN DU.DATAINICIO_VINCULO
                                                                    AND     DU.DATAFIM_VINCULO )
                                                    )
                                                AND DU.DOCENTEID <> @DOCENTEID ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoId);
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
