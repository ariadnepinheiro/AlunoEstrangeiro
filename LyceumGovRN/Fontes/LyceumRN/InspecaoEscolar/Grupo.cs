using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
    public class Grupo
    {
        public DataTable ListarGrupo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT   * 
                                            FROM   InspecaoEscolar.Grupo
                                                   order by campanhaid,ordem";

                retorno = contexto.GetDataTable(contextQuery);
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

            return retorno;
        }

        /// <summary>
        ///  Lista de grupos por campanhaID
        /// </summary>
        /// <param name="campanhaID"></param>
        /// <returns>Retorna apenas GRUPOID e DESCRICAO</returns>
        public DataTable ListarGrupoporCampanha(int campanhaID)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT   G.GRUPOID,G.DESCRICAO 
                                            FROM   INSPECAOESCOLAR.GRUPO G
                                            WHERE G.CAMPANHAID =@CAMPANHAID
                                                   ORDER BY G.CAMPANHAID,G.ORDEM";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaID);

                retorno = contexto.GetDataTable(contextQuery);


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

            return retorno;
        }

        /// <summary>
        ///  Lista de grupos por campanhaID
        /// </summary>
        /// <param name="campanhaID"></param>
        /// <returns>Retorna todos os campos</returns>
        public DataTable ListarGrupoporCampanhaCompleto(int campanhaID)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT  G.* 
                                            FROM   INSPECAOESCOLAR.GRUPO G
                                            WHERE G.CAMPANHAID =@CAMPANHAID
                                                   ORDER BY G.CAMPANHAID,G.ORDEM";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaID);

                retorno = contexto.GetDataTable(contextQuery);


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

            return retorno;
        }
                
        #region Inserir

        public bool Insere(Entidades.Grupo dadosGrupo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {                              
                retorno = this.Insere(contexto,dadosGrupo);
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
            return retorno;
        }

        public bool Insere(DataContext contexto, Entidades.Grupo dadosGrupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"INSERT INTO INSPECAOESCOLAR.GRUPO 
                                        (CAMPANHAID,
                                        ORDEM,
                                        DESCRICAO,
                                        USUARIOID,
                                        DATACADASTRO,
                                        DATAALTERACAO)
                                VALUES (@CAMPANHAID,
                                        @ORDEM,
                                        @DESCRICAO,
                                        @USUARIOID,
                                        @DATACADASTRO,
                                        @DATAALTERACAO)
                                SELECT IDENT_CURRENT('INSPECAOESCOLAR.GRUPO')";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, dadosGrupo.CampanhaId);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosGrupo.Ordem);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosGrupo.Descricao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosGrupo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dadosGrupo.GrupoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
            
            retorno = true;
            return retorno;
        }

        public ValidacaoDados ValidaInsercaoAtualizacaoGrupo(Entidades.Grupo dadosGrupo)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            if (dadosGrupo == null)
            {
                mensagens.Add("Todos os campos são obrigatórios.");
            }

            if (dadosGrupo.Descricao == string.Empty)
            {
                mensagens.Add("Descrição é obrigatória.");
            }

            if (dadosGrupo.Ordem == 0)
            {
                mensagens.Add("Ordem é obrigatória.");
            }

            if (dadosGrupo.CampanhaId == 0)
            {
                mensagens.Add("Campanha é obrigatória.");
            }

            //if (dadosGrupo.GrupoId == 0)
            //{
            //    mensagens.Add("Grupo é obrigatória.");
            //}

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiDescricaoOrdemCampanha(contexto, dadosGrupo))
                    {
                        mensagens.Add("O GRUPO não pode ser inserido/atualizado pois possui ASSUNTOS e/ou ORDENS já cadastrados, e não podem ser repetidos.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }
        
        public ValidacaoDados ValidaAtualizacaoGrupo(Entidades.Grupo dadosGrupo)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosGrupo == null)
            {
                mensagens.Add("Todos os campos são obrigatórios.");
            }
            if (dadosGrupo.GrupoId == 0)
            {
                mensagens.Add("Grupo é obrigatório.");
            }

            if (dadosGrupo.Descricao == string.Empty)
            {
                mensagens.Add("Descrição é obrigatória.");
            }

            if (dadosGrupo.Ordem == 0)
            {
                mensagens.Add("Ordem é obrigatória.");
            }

            if (dadosGrupo.CampanhaId == 0)
            {
                mensagens.Add("Campanha é obrigatória.");
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

        private bool PossuiDescricaoOrdemCampanha(DataContext ctx, Entidades.Grupo dadosGrupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.GRUPO G
                                         WHERE G.CAMPANHAID=@CAMPANHAID AND
                                               (G.ORDEM=@ORDEM OR
                                               G.DESCRICAO=@DESCRICAO)";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, dadosGrupo.CampanhaId);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosGrupo.Ordem);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosGrupo.Descricao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        #endregion

        #region Atualizar

        public bool Atualiza(Entidades.Grupo dadosGrupo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @"update INSPECAOESCOLAR.GRUPO 
                                         set
                                        CAMPANHAID=@CAMPANHAID,
                                        ORDEM=@ORDEM,
                                        DESCRICAO=@DESCRICAO,
                                        USUARIOID=@USUARIOID,
                                        DATAALTERACAO=@DATAALTERACAO
                                        where
                                        GRUPOID=@GRUPOID
                                       ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, dadosGrupo.CampanhaId);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosGrupo.Ordem);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosGrupo.Descricao);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosGrupo.UsuarioId);
                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, dadosGrupo.GrupoId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
                retorno = true;
            }
            catch (Exception ex)
            {

                string mensagem = string.Empty;
                contexto.Abandon();


                if (ex.Message == "Error executing ContextQuery!")
                {

                    if (Convert.ToString(ex.InnerException.Message).Contains("Violation of UNIQUE KEY constraint 'GrupoUnique'."))
                        mensagem = "Não é possível cadastrar uma ordem repetida para a mesma campanha.";


                    if (Convert.ToString(ex.InnerException.Message).Contains("Violation of UNIQUE KEY constraint 'GrupoDescricaoUnique'."))
                        mensagem = "Não é possível cadastrar uma descrição repetida para a mesma campanha.";

                }
                else if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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
            return retorno;
        }

        #endregion

        #region Remover

        public void Remover(int grupoId, int campanhaID)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" delete InspecaoEscolar.GRUPO
                                                WHERE GRUPOID = @GRUPOID
                                                and CAMPANHAID = @CAMPANHAID ";

                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoId);
                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaID);

                contexto.ApplyModifications(contextQuery);
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
        
        public ValidacaoDados ValidaRemocaoGrupo(int grupoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };


            if (grupoId == 0 || grupoId == null)
            {
                mensagens.Add("Campo grupoId é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se JÁ FOI UTILLIZADO pelo ASSUNTO
                    if (this.PossuiAssuntoPorGrupoId(contexto, grupoId))
                    {
                        mensagens.Add("O GRUPO não pode ser excluido pois possui ASSUNTOS cadastrados.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + "</BR>" + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }
        
        private bool PossuiAssuntoPorGrupoId(DataContext ctx, int grupoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.ASSUNTO G
                                         WHERE G.GRUPOID=@GRUPOID";

            contextQuery.Parameters.Add("@GRUPOID", SqlDbType.VarChar, grupoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        #endregion

        public ICollection<RN.InspecaoEscolar.Entidades.Grupo> ObtemPor(DataContext contexto, int campanhaId)
        {
            ICollection<RN.InspecaoEscolar.Entidades.Grupo> grupos = new List<RN.InspecaoEscolar.Entidades.Grupo>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" select g.* from InspecaoEscolar.Grupo g 
                                     where g.campanhaid = @campanhaId ";

            contextQuery.Parameters.Add("@campanhaId", SqlDbType.Int, campanhaId);

            grupos = contexto.TryToBindEntities<RN.InspecaoEscolar.Entidades.Grupo>(contextQuery);

            return grupos;

        }

        public List<DadosRelatorioInspecaoGrupo> ObtemDadosGrupoConsideracoesFinaisPor(DataContext contexto, int campanhaId)
        {
            List<DadosRelatorioInspecaoGrupo> grupos = new List<DadosRelatorioInspecaoGrupo>();
            DadosRelatorioInspecaoGrupo dadosGrupo = new DadosRelatorioInspecaoGrupo();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT G.GRUPOID, 
                                           G.CAMPANHAID, 
                                           G.ORDEM, 
                                           G.DESCRICAO 
                                    FROM   INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                           INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                   ON G.GRUPOID = A.GRUPOID 
                                    WHERE  CAMPANHAID = @CAMPANHAID 
                                           AND TIPOASSUNTOID IN ( 8, 9, 10, 11 )
                                    ORDER BY G.ORDEM ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosGrupo = new DadosRelatorioInspecaoGrupo();

                    dadosGrupo.CampanhaId = Convert.ToInt32(reader["CAMPANHAID"]);
                    dadosGrupo.GrupoId = Convert.ToInt32(reader["GRUPOID"]);
                    dadosGrupo.Ordem = Convert.ToInt32(reader["ORDEM"]);
                    dadosGrupo.Descricao = Convert.ToString(reader["DESCRICAO"]);

                    grupos.Add(dadosGrupo);
                }

                return grupos;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosRelatorioInspecaoGrupo> ObtemDadosGrupoPor_DemaisDependencias(DataContext contexto, int campanhaId)
        {
            List<DadosRelatorioInspecaoGrupo> grupos = new List<DadosRelatorioInspecaoGrupo>();
            DadosRelatorioInspecaoGrupo dadosGrupo = new DadosRelatorioInspecaoGrupo();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT G.GRUPOID, 
                                           G.CAMPANHAID, 
                                           G.ORDEM, 
                                           G.DESCRICAO 
                                    FROM   INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                           INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                   ON G.GRUPOID = A.GRUPOID 
                                    WHERE  CAMPANHAID = @CAMPANHAID 
                                           AND TIPOASSUNTOID IN ( 2,3,4,5 )
                                    ORDER BY G.ORDEM ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosGrupo = new DadosRelatorioInspecaoGrupo();

                    dadosGrupo.CampanhaId = Convert.ToInt32(reader["CAMPANHAID"]);
                    dadosGrupo.GrupoId = Convert.ToInt32(reader["GRUPOID"]);
                    dadosGrupo.Ordem = Convert.ToInt32(reader["ORDEM"]);
                    dadosGrupo.Descricao = Convert.ToString(reader["DESCRICAO"]);

                    grupos.Add(dadosGrupo);
                }

                return grupos;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosRelatorioInspecaoGrupo> ObtemDadosGrupoPor_DemaisDependencias(DataContext contexto, int campanhaId, int ordem)
        {
            List<DadosRelatorioInspecaoGrupo> grupos = new List<DadosRelatorioInspecaoGrupo>();
            DadosRelatorioInspecaoGrupo dadosGrupo = new DadosRelatorioInspecaoGrupo();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT G.GRUPOID, 
                                           G.CAMPANHAID, 
                                           G.ORDEM, 
                                           G.DESCRICAO 
                                    FROM   INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                           INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                   ON G.GRUPOID = A.GRUPOID 
                                    WHERE  CAMPANHAID = @CAMPANHAID 
                                           AND TIPOASSUNTOID IN ( 2,3,4,5 )
                                           AND G.ORDEM = @ORDEM
                                    ORDER BY G.ORDEM ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, ordem);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosGrupo = new DadosRelatorioInspecaoGrupo();

                    dadosGrupo.CampanhaId = Convert.ToInt32(reader["CAMPANHAID"]);
                    dadosGrupo.GrupoId = Convert.ToInt32(reader["GRUPOID"]);
                    dadosGrupo.Ordem = Convert.ToInt32(reader["ORDEM"]);
                    dadosGrupo.Descricao = Convert.ToString(reader["DESCRICAO"]);

                    grupos.Add(dadosGrupo);
                }

                return grupos;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
