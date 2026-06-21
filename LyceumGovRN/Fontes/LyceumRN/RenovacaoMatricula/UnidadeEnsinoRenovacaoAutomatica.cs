using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.RenovacaoMatricula
{
    public class UnidadeEnsinoRenovacaoAutomatica
    {
        public const string QueryListaUnidadesEnsinoRenovacaoAutomatica = " SELECT DISTINCT UNIDADEENSINOID FROM RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA (NOLOCK) ";

        public DataTable ObtemLista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT  UNIDADEENSINORENOVACAOAUTOMATICAID ,
                                UNIDADEENSINOID ,
                                DATACADASTRO ,
                                USUARIO ,
                                ( UNIDADEENSINOID + ' - ' + NOME_COMP ) AS ESCOLA
                        FROM    RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA(NOLOCK)
                                INNER JOIN LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = UNIDADEENSINOID ";

                lista = ctx.GetDataTable(contextQuery);
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

            return lista;
        }

        public DataTable ObtemListaPor(int idRegional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT  UNIDADEENSINORENOVACAOAUTOMATICAID ,
                                                UNIDADEENSINOID ,
                                                DATACADASTRO ,
                                                USUARIO ,
                                                ( UNIDADEENSINOID + ' - ' + NOME_COMP ) AS ESCOLA
                                        FROM    RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA(NOLOCK)
                                                INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = UNIDADEENSINOID
                                        WHERE   UE.ID_REGIONAL = @ID_REGIONAL ";

                contextQuery.Parameters.Add("@ID_REGIONAL", idRegional);

                lista = ctx.GetDataTable(contextQuery);
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

            return lista;
        }

        public DataTable ObtemListaPor(string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT  UNIDADEENSINORENOVACAOAUTOMATICAID ,
                                                UNIDADEENSINOID ,
                                                DATACADASTRO ,
                                                USUARIO ,
                                                ( UNIDADEENSINOID + ' - ' + NOME_COMP ) AS ESCOLA
                                        FROM    RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA(NOLOCK)
                                                INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = UNIDADEENSINOID
                                        WHERE   UNIDADEENSINOID = @UNIDADEENSINOID ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);

                lista = ctx.GetDataTable(contextQuery);
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

            return lista;
        }

        public ValidacaoDados Valida(DTOs.DadosRenovacaoAutomatica dadosRenovacaoAutomatica)
        {
            List<string> mensagens = new List<string>();
            List<string> unidadesEnsino = new List<string>();
            DataContext contexto = null;
            int resultado = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosRenovacaoAutomatica == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(dadosRenovacaoAutomatica.UsuarioResponsavel))
            {
                mensagens.Add("USUÁRIO RESPONSAVEL é obrigatório.");
            }

            if (dadosRenovacaoAutomatica.PorUnidadeEnsino == dadosRenovacaoAutomatica.PorRegional)
            {
                mensagens.Add("O tipo de renovação automatica deve ser escolhido.");
            }

            //Verifica se a criação da renovação automatica será por Escola
            if (dadosRenovacaoAutomatica.PorUnidadeEnsino)
            {
                if (string.IsNullOrEmpty(dadosRenovacaoAutomatica.UnidadeEnsino))
                {
                    mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
                }
                else if ((dadosRenovacaoAutomatica.UnidadeEnsino.Length != 8) || !int.TryParse(dadosRenovacaoAutomatica.UnidadeEnsino, out resultado))
                {
                    mensagens.Add("Campo UNIDADE DE ENSINO deve ser composto por 8 dígitos.");
                }
            }

            //Verifica se a criação da renovação automatica será por Regional
            if (dadosRenovacaoAutomatica.PorRegional)
            {
                if (dadosRenovacaoAutomatica.Regional <= 0)
                {
                    mensagens.Add("Campo REGIONAL é obrigatório.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a criação da renovação automatica será por Escola
                    if (dadosRenovacaoAutomatica.PorUnidadeEnsino)
                    {
                        //Verifica já existe Renovação Automatica para a unidade de ensino
                        if (this.ExisteRenovacaoAutomaticaPor(contexto, dadosRenovacaoAutomatica.UnidadeEnsino))
                        {
                            mensagens.Add("A UNIDADE DE ENSINO já possui renovação automática cadastrada");
                        }
                    }

                    //Verifica se a criação da renovação automatica será por Regional
                    if (dadosRenovacaoAutomatica.PorRegional)
                    {
                        dadosRenovacaoAutomatica.ListaUnidadesEnsino = new List<string>();

                        //Obtem lista com escolas daquela regional sem renovação automatica
                        unidadesEnsino = this.ObtemListaUnidadesEnsinoSemRenovacaoAutomaticaPor(contexto, dadosRenovacaoAutomatica.Regional);

                        //Verifica se exsite alguma escola para ser adicionada 
                        if (unidadesEnsino.Count == 0)
                        {
                            mensagens.Add("Todas as UNIDADES DE ENSINO desta regional já possuem renovação automática cadastrada");
                        }
                        else
                        {
                            dadosRenovacaoAutomatica.ListaUnidadesEnsino = unidadesEnsino;
                        }
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

        private bool ExisteRenovacaoAutomaticaPor(DataContext ctx, string unidadeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA(NOLOCK)
                                            WHERE   UNIDADEENSINOID = @UNIDADEENSINOID ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);

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

        public void Insere(DTOs.DadosRenovacaoAutomatica dadosRenovacaoAutomatica)
        {
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            RenovacaoMatricula.Entidades.UnidadeEnsinoRenovacaoAutomatica unidadeEnsinoRenovacaoAutomatica = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.UnidadeEnsinoRenovacaoAutomatica();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Verifica se a criação da renovação automatica será por Escola
                if (dadosRenovacaoAutomatica.PorUnidadeEnsino)
                {
                    unidadeEnsinoRenovacaoAutomatica.UnidadeEnsinoId = dadosRenovacaoAutomatica.UnidadeEnsino;
                    unidadeEnsinoRenovacaoAutomatica.Usuario = dadosRenovacaoAutomatica.UsuarioResponsavel;
                    unidadeEnsinoRenovacaoAutomatica.DataCadastro = DateTime.Now;

                    this.Insere(ctx, unidadeEnsinoRenovacaoAutomatica);
                }
                else if (dadosRenovacaoAutomatica.PorRegional)
                {
                    foreach (var unidadeEnsino in dadosRenovacaoAutomatica.ListaUnidadesEnsino)
                    {
                        unidadeEnsinoRenovacaoAutomatica = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.UnidadeEnsinoRenovacaoAutomatica();
                        unidadeEnsinoRenovacaoAutomatica.UnidadeEnsinoId = unidadeEnsino;
                        unidadeEnsinoRenovacaoAutomatica.Usuario = dadosRenovacaoAutomatica.UsuarioResponsavel;
                        unidadeEnsinoRenovacaoAutomatica.DataCadastro = DateTime.Now;

                        this.Insere(ctx, unidadeEnsinoRenovacaoAutomatica);
                    }
                }

                //Remove cache
                RN.Util.Cache.RemoveCache(RN.Util.Cache.UnidadesEnsinoRenovacaoAutomatica);
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

        private List<string> ObtemListaUnidadesEnsinoSemRenovacaoAutomaticaPor(DataContext ctx, int regional)
        {
            List<string> listaUnidadesEnsino = new List<string>();
            string unidadeEnsino = string.Empty;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT DISTINCT UNIDADE_ENS
                                 FROM   VW_UNIDADE_ENSINO_SITUACAO UE
                                 WHERE  UE.SITUACAO = 'ESTADUAL'
                                        AND UE.ID_REGIONAL = @ID_REGIONAL
                                        AND NOT EXISTS ( SELECT TOP 1
                                                                1
                                                         FROM   RENOVACAOMATRICULA.UNIDADEENSINORENOVACAOAUTOMATICA RA ( NOLOCK )
                                                         WHERE  RA.UNIDADEENSINOID = UE.UNIDADE_ENS ) ";

                contextQuery.Parameters.Add("@ID_REGIONAL", regional);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    unidadeEnsino = Convert.ToString(reader["UNIDADE_ENS"]);
                    listaUnidadesEnsino.Add(unidadeEnsino);
                }

                return listaUnidadesEnsino;
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
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private void Insere(DataContext ctx, RenovacaoMatricula.Entidades.UnidadeEnsinoRenovacaoAutomatica unidadeEnsinoRenovacaoAutomatica)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT  INTO RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA
                                                ( UNIDADEENSINOID ,
                                                  DATACADASTRO ,
                                                  USUARIO
                                                )
                                        VALUES  ( @UNIDADEENSINOID ,
                                                  @DATACADASTRO ,
                                                  @USUARIO
                                                )  ";

                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoRenovacaoAutomatica.UnidadeEnsinoId);
                contextQuery.Parameters.Add("@USUARIO", unidadeEnsinoRenovacaoAutomatica.Usuario);
                contextQuery.Parameters.Add("@DATACADASTRO", unidadeEnsinoRenovacaoAutomatica.DataCadastro);

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

        public void Remove(List<int> listaIds)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (int id in listaIds)
                {
                    this.Remove(ctx, id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void Remove(DataContext ctx, int unidadeEnsinioRenovacaoAutomaticaId)
        {
             ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RenovacaoMatricula.UNIDADEENSINORENOVACAOAUTOMATICA
                                          WHERE  UNIDADEENSINORENOVACAOAUTOMATICAID = @UNIDADEENSINORENOVACAOAUTOMATICAID ";

                contextQuery.Parameters.Add("@UNIDADEENSINORENOVACAOAUTOMATICAID", unidadeEnsinioRenovacaoAutomaticaId);

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
