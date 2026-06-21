using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class ChAgrupamentoCargo
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CH.CH_AGRUPAMENTOCARGOID, 
                                   G.AGRUPAMENTOCARGOSID, 
                                   G.DESCRICAO AS DESCRICAO_AGRUPAMENTOCARGO, 
                                   CH.FUNCAO, 
                                   F.DESCRICAO AS DESCRICAOFUNCAO, 
                                   CARGAHORARIACOMPLEMENTACAO, 
                                   CARGAHORARIAPLANEJAMENTO, 
                                   CARGAHORARIAREGENCIA, 
                                   CH.USUARIOID, 
                                   CH.DATACADASTRO, 
                                   CH.DATAALTERACAO,
                                   G.CARGAHORARIA AS CARGAHORARIAGRUPO,   
                                   CARGAHORARIACOMPLEMENTACAO + CARGAHORARIAPLANEJAMENTO + CARGAHORARIAREGENCIA AS TOTAL,    
                                   (CONVERT(VARCHAR,G.AGRUPAMENTOCARGOSID) + '_' + CONVERT(VARCHAR,G.CARGAHORARIA)) as CHAVE
                            FROM   [RECURSOSHUMANOS].[CH_AGRUPAMENTOCARGO] CH (NOLOCK) 
                                   INNER JOIN LY_FUNCAO F (NOLOCK) 
                                           ON CH.FUNCAO = F.FUNCAO 
                                    INNER JOIN RECURSOSHUMANOS.AGRUPAMENTOCARGOS G (NOLOCK) 
                                           ON CH.AGRUPAMENTOCARGOSID = G.AGRUPAMENTOCARGOSID ";

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

        public int ObtemCargaTotalPor(int agrupamentoCargos, string funcao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT CARGAHORARIAREGENCIA, 
												CARGAHORARIACOMPLEMENTACAO, 
												CARGAHORARIAPLANEJAMENTO, 
												CARGAHORARIAREGENCIA + CARGAHORARIACOMPLEMENTACAO + CARGAHORARIAPLANEJAMENTO AS TOTAL
                                        FROM   RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO (NOLOCK) 											   
                                        WHERE  AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID
                                               AND FUNCAO = @FUNCAO ";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", agrupamentoCargos);
                contextQuery.Parameters.Add("@FUNCAO", funcao);


                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        public int ObtemCargaHorariaRegenciaPor(string categoria, string funcao)
        {	
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            int retorno = 0;

            try
            {
                retorno = this.ObtemCargaHorariaRegenciaPor(contexto, categoria, funcao);
                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public int ObtemCargaHorariaRegenciaPor(DataContext contexto, string categoria, string funcao)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT CH.CARGAHORARIAREGENCIA 
                                        FROM   RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH (NOLOCK) 
											   INNER JOIN LY_CATEGORIA_DOCENTE CD (NOLOCK) ON CH.AGRUPAMENTOCARGOSID = CD.AGRUPAMENTOCARGOSID
                                        WHERE  CD.CATEGORIA = @CATEGORIA
                                               AND CH.FUNCAO = @FUNCAO ";

                contextQuery.Parameters.Add("@CATEGORIA", TechneDbType.T_CODIGO, categoria);
                contextQuery.Parameters.Add("@FUNCAO", TechneDbType.T_CODIGO, funcao);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["CARGAHORARIAREGENCIA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CARGAHORARIAREGENCIA"]);
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

        public ValidacaoDados Valida(Entidades.ChAgrupamentoCargo chAgrupamentoCargo, int cargaHorariaGrupo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (chAgrupamentoCargo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (chAgrupamentoCargo.ChAgrupamentoCargoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (chAgrupamentoCargo.Funcao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo FUNÇÃO é obrigatório.");
            }

            if (chAgrupamentoCargo.AgrupamentoCargosId <= 0)
            {
                mensagens.Add("Campo GRUPO é obrigatório.");
            }

            if (chAgrupamentoCargo.CargaHorariaComplementacao < 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA COMPLEMENTAÇÃO é obrigatório.");
            }

            if (chAgrupamentoCargo.CargaHorariaRegencia < 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA REGÊNCIA é obrigatório.");
            }

            if (chAgrupamentoCargo.CargaHorariaPlanejamento < 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA PLANEJAMENTO é obrigatório.");
            }

            if (cargaHorariaGrupo <= 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA DO GRUPO é obrigatório.");
            }

            if (chAgrupamentoCargo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a carga horaria total é igual a carga horaria do grupo
                    //if ((chAgrupamentoCargo.CargaHorariaComplementacao + chAgrupamentoCargo.CargaHorariaRegencia + chAgrupamentoCargo.CargaHorariaPlanejamento) != cargaHorariaGrupo)
                    //{
                    //    mensagens.Add(string.Format("A soma das cargas horárias deve ser igual à carga horária do grupo ({0}).", cargaHorariaGrupo.ToString()));
                    //}

                    //Verifica se já existe a função + grupo cadastrado
                    if (this.PossuiOutraoCadastradaPor(contexto, chAgrupamentoCargo.Funcao, chAgrupamentoCargo.AgrupamentoCargosId, chAgrupamentoCargo.ChAgrupamentoCargoId))
                    {
                        mensagens.Add("Este CARGO / FUNÇÃO já foi cadastrado.");
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
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

        private bool PossuiOutraoCadastradaPor(DataContext ctx, string funcao, int agrupamentoCargosId, int chAgrupamentoCargoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM [RecursosHumanos].[CH_AGRUPAMENTOCARGO] (NOLOCK)
                                WHERE FUNCAO = @FUNCAO
                                    AND AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID
	                                AND CH_AGRUPAMENTOCARGOID <> @CH_AGRUPAMENTOCARGOID ";

            contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, funcao);
            contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargosId);
            contextQuery.Parameters.Add("@CH_AGRUPAMENTOCARGOID", SqlDbType.Int, chAgrupamentoCargoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.ChAgrupamentoCargo chAgrupamentoCargo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RecursosHumanos.CH_AGRUPAMENTOCARGO
                                                        (AGRUPAMENTOCARGOSID, 
                                                         FUNCAO,
                                                         CARGAHORARIACOMPLEMENTACAO, 
                                                         CARGAHORARIAPLANEJAMENTO,
                                                         CARGAHORARIAREGENCIA,
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@AGRUPAMENTOCARGOSID, 
                                                         @FUNCAO,
                                                         @CARGAHORARIACOMPLEMENTACAO, 
                                                         @CARGAHORARIAPLANEJAMENTO,
                                                         @CARGAHORARIAREGENCIA,
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, chAgrupamentoCargo.Funcao);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, chAgrupamentoCargo.AgrupamentoCargosId);
                contextQuery.Parameters.Add("@CARGAHORARIACOMPLEMENTACAO", SqlDbType.Int, chAgrupamentoCargo.CargaHorariaComplementacao);
                contextQuery.Parameters.Add("@CARGAHORARIAPLANEJAMENTO", SqlDbType.Int, chAgrupamentoCargo.CargaHorariaPlanejamento);
                contextQuery.Parameters.Add("@CARGAHORARIAREGENCIA", SqlDbType.Int, chAgrupamentoCargo.CargaHorariaRegencia);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, chAgrupamentoCargo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void Atualiza(Entidades.ChAgrupamentoCargo chAgrupamentoCargo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RecursosHumanos.CH_AGRUPAMENTOCARGO
                                        SET    CARGAHORARIACOMPLEMENTACAO = @CARGAHORARIACOMPLEMENTACAO, 
                                               CARGAHORARIAPLANEJAMENTO = @CARGAHORARIAPLANEJAMENTO,
                                               CARGAHORARIAREGENCIA = @CARGAHORARIAREGENCIA, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  CH_AGRUPAMENTOCARGOID = @CH_AGRUPAMENTOCARGOID ";

                contextQuery.Parameters.Add("@CARGAHORARIACOMPLEMENTACAO", SqlDbType.Int, chAgrupamentoCargo.CargaHorariaComplementacao);
                contextQuery.Parameters.Add("@CARGAHORARIAPLANEJAMENTO", SqlDbType.Int, chAgrupamentoCargo.CargaHorariaPlanejamento);
                contextQuery.Parameters.Add("@CARGAHORARIAREGENCIA", SqlDbType.Int, chAgrupamentoCargo.CargaHorariaRegencia);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, chAgrupamentoCargo.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@CH_AGRUPAMENTOCARGOID", SqlDbType.Int, chAgrupamentoCargo.ChAgrupamentoCargoId);

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

        public ValidacaoDados ValidaRemocao(int chAgrupamentoCargoId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (chAgrupamentoCargoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
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

        public void Remove(decimal chAgrupamentoCargoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO 
                                        WHERE  CH_AGRUPAMENTOCARGOID = @CH_AGRUPAMENTOCARGOID  ";

                contextQuery.Parameters.Add("@CH_AGRUPAMENTOCARGOID", chAgrupamentoCargoId);

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
