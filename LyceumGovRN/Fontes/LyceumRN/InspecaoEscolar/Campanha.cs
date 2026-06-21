using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;


namespace Techne.Lyceum.RN.InspecaoEscolar
{
    public class Campanha
    {

        #region Listar

        /// <summary>
        /// Lista as campanhas exibindo o Ano_Semestre_Titulo. Onde o Título serão exibidos apenas 100 caracteres.
        /// </summary>
        /// <returns></returns>
        public DataTable ListarCampanha_Grupo()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @"SELECT   C.CAMPANHAID,
                                        'ANO:'+CAST(C.ANO AS varchar(4))+'_'+
                                        'SEMESTRE:'+CAST(C.SEMESTRE AS varchar(2))+'_'+
                                        'TÍTULO:'+SUBSTRING(C.TITULO,0,100)	AS CAMPANHA  
                                        FROM   InspecaoEscolar.Campanha C
                                        ORDER BY C.ANO,C.SEMESTRE";

                //  contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

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

        public DataTable ListarCampanha()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT CAMPANHAID, 
												ANO, 
												SEMESTRE, 
												OBJETIVO, 
												PROCEDIMENTO, 
												CASE 
													WHEN EXIBEINSPECAOESCOLAR = 1 THEN 'Sim'
													ELSE 'Não'
												END EXIBEINSPECAOESCOLAR,
												TITULO, 
												USUARIOID, 
												DATACADASTRO, 
												DATAALTERACAO 
                                          FROM  InspecaoEscolar.Campanha ";

                //  contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

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

        public bool ExibeInspecaoEscolar(int campanhaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                          FROM InspecaoEscolar.CAMPANHA
                                          WHERE CAMPANHAID = @CAMPANHAID
                                                AND EXIBEINSPECAOESCOLAR = 1";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
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
                contexto.Dispose();
            }
        }


       /// <summary>
       /// Busca as campanhas cadastradas
       /// </summary>
       /// <param name="ano">Ano cadastrado</param>
       /// <param name="semestre">Semestre cadastrado</param>
        /// <returns>  C.CAMPANHAID,C.ANO,C.SEMESTRE,C.TITULO</returns>
        public DataTable ListarCampanha(int ano, int semestre)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT   C.CAMPANHAID,C.ANO,C.SEMESTRE,C.TITULO, C.EXIBEINSPECAOESCOLAR 
                                            FROM   INSPECAOESCOLAR.CAMPANHA C
                                            WHERE C.ANO=@ANO AND C.SEMESTRE=@SEMESTRE
                                                   ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

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
        /// Lista os anos das campanhas
        /// </summary>
        /// <returns> Retorna os anos de  campanhas cadastradas</returns>
        public DataTable ListarAno()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT C.ANO FROM INSPECAOESCOLAR.CAMPANHA C   ";


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
        /// Busca os semestres cadastrados pelo ano.
        /// </summary>
        /// <param name="ano">Ano a ser pesquisado</param>
        /// <returns>Retorna os semestres cadastrados, filtrando pelo ano.</returns>
        public DataTable ListarSemestreporAno(int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT C.SEMESTRE 
                                          FROM INSPECAOESCOLAR.CAMPANHA C 
                                          WHERE	C.ANO=@ANO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

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

        #endregion

        #region Inserir

        public bool Insere( DataContext contexto, Entidades.Campanha dadosCampanha)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            
            contextQuery.Command = @" INSERT INTO InspecaoEscolar.CAMPANHA
                                                (ANO
                                                ,SEMESTRE
                                                ,OBJETIVO
                                                ,PROCEDIMENTO
                                                ,TITULO
                                                ,EXIBEINSPECAOESCOLAR
                                                ,USUARIOID
                                                ,DATACADASTRO
                                                ,DATAALTERACAO) 
                                    VALUES      (@ANO
                                                ,@SEMESTRE
                                                ,@OBJETIVO
                                                ,@PROCEDIMENTO
                                                ,@TITULO
                                                ,@EXIBEINSPECAOESCOLAR
                                                ,@USUARIOID
                                                ,@DATACADASTRO
                                                ,@DATAALTERACAO)
                                       SELECT IDENT_CURRENT('InspecaoEscolar.CAMPANHA')";
                
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, dadosCampanha.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, dadosCampanha.Semestre);
                contextQuery.Parameters.Add("@OBJETIVO", SqlDbType.VarChar, dadosCampanha.Objetivo);
                contextQuery.Parameters.Add("@PROCEDIMENTO", SqlDbType.VarChar, dadosCampanha.Procedimento);
                contextQuery.Parameters.Add("@TITULO", SqlDbType.VarChar, dadosCampanha.Titulo);
                contextQuery.Parameters.Add("@EXIBEINSPECAOESCOLAR", SqlDbType.Bit, dadosCampanha.ExibeInspecaoEscolar);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosCampanha.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                
                dadosCampanha.CampanhaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
                
                
                retorno = true;
            
            return retorno;
        }
        
        public bool Insere(Entidades.Campanha dadosCampanha)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool retorno = false;
            try
            {                        
                retorno = this.Insere(contexto, dadosCampanha);
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
        
        #endregion

        #region Excluir

        public void Remove(int campanhaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" delete InspecaoEscolar.CAMPANHA
                                                WHERE CAMPANHAID = @CAMPANHAID ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

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

        #endregion

        #region Atualizar

        public bool Atualiza(Entidades.Campanha dadosCampanha)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {
                contextQuery.Command = @"UPDATE InspecaoEscolar.CAMPANHA SET
                                              ANO = @ANO,
                                              SEMESTRE=@SEMESTRE,
                                              OBJETIVO=@OBJETIVO,
                                              PROCEDIMENTO=@PROCEDIMENTO,
                                              TITULO=@TITULO,
                                              EXIBEINSPECAOESCOLAR=@EXIBEINSPECAOESCOLAR,
                                              USUARIOID=@USUARIOID,
                                              DATAALTERACAO=@DATAALTERACAO
                                         WHERE 
                                         CAMPANHAID=@CAMPANHAID;";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, dadosCampanha.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, dadosCampanha.Semestre);
                contextQuery.Parameters.Add("@OBJETIVO", SqlDbType.VarChar, dadosCampanha.Objetivo);
                contextQuery.Parameters.Add("@PROCEDIMENTO", SqlDbType.VarChar, dadosCampanha.Procedimento);
                contextQuery.Parameters.Add("@TITULO", SqlDbType.VarChar, dadosCampanha.Titulo);
                contextQuery.Parameters.Add("@EXIBEINSPECAOESCOLAR", SqlDbType.Bit, dadosCampanha.ExibeInspecaoEscolar);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosCampanha.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, dadosCampanha.CampanhaId);

                contexto.ApplyModifications(contextQuery);
                retorno = true;
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

        #endregion

        #region Validar

        private bool PossuiGrupoPor(DataContext ctx, int campanhaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.GRUPO G
                                         WHERE G.CAMPANHAID=@CAMPANHAID";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.VarChar, campanhaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool PossuiCampanhaEscolaPor(DataContext ctx, int campanhaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.CAMPANHAESCOLA G
                                         WHERE G.CAMPANHAID=@CAMPANHAID";

            contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.VarChar, campanhaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public ValidacaoDados Valida(Entidades.Campanha campanhaDados)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (campanhaDados == null)
            {
                return validacaoDados;
            }
            //verificar se insert ou update

            //if (campanhaDados.CampanhaId == null)
            //{
            //    //sei que é insert
            //    //se fosse update, usaria o próprio campanhaid
            //    campanhaDados.CampanhaId = 0;
            //    //Verifica se é alteração
            //}

            if (campanhaDados.Ano == 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (campanhaDados.Semestre == 0)
            {
                mensagens.Add("Campo SEMESTRE é obrigatório.");
            }

            if (campanhaDados.Titulo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TITULO é obrigatório.");
            }

            if (campanhaDados.Objetivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo OBJETIVO é obrigatório.");
            }

            if (campanhaDados.Procedimento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROCEDIMENTO é obrigatório.");
            }            

            if (campanhaDados.ExibeInspecaoEscolar == null)
            {
                mensagens.Add("Campo EXIBE ABA INSPEÇÃO ESCOLA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se existe outros títulos iguais
                    string titulo = this.verificaTituloExistente(contexto, campanhaDados.Titulo, campanhaDados.CampanhaId);

                    if (!titulo.IsNullOrEmptyOrWhiteSpace())// se não veio vazio
                    {
                        mensagens.Add(titulo);
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

        public ValidacaoDados ValidaRemocao(int campanhaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (campanhaId == 0)
            {
                mensagens.Add("Campo CODIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se JÁ FOI UTILLIZADO pelo grupo
                    if (this.PossuiGrupoPor(contexto, campanhaId))
                    {
                        mensagens.Add("A campanha não pode ser excluida pois possui grupos cadastrados.");
                    }

                    //Verifica se JÁ FOI UTILLIZADO pelA CAMPANHA DA ESCOLA
                    if (this.PossuiCampanhaEscolaPor(contexto, campanhaId))
                    {
                        mensagens.Add("A campanha não pode ser excluida pois já foi utlizada por uma escola.");
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

        private String verificaTituloExistente(DataContext ctx, string titulo, int campanhaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            //  List<string> escolas = new List<string>();
            string mensagem = string.Empty;


            try
            {
                contextQuery.Command = @"SELECT C.ANO,C.SEMESTRE,
                                            SUBSTRING ( C.titulo ,0 , 180 )TITULO,
                                            SUBSTRING ( C.OBJETIVO ,0 , 180 )OBJETIVO 
                                            FROM LYCEUM.INSPECAOESCOLAR.CAMPANHA C (NOLOCK)
                                            WHERE C.TITULO=@TITULO AND C.CAMPANHAID<>@CAMPANHAID";


                contextQuery.Parameters.Add("@TITULO", SqlDbType.VarChar, titulo);
                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.VarChar, campanhaId);

                reader = ctx.GetDataReader(contextQuery);



                while (reader.Read())
                {
                    mensagem = "Este TÍTULO já foi cadastrado para a campanha com  as informações abaixo: <br /><br />";
                    mensagem += "Título: " + Convert.ToString(reader["TITULO"]) + " <br /> ";
                    mensagem += "Ano: " + Convert.ToString(reader["ANO"]) + " <br /> ";
                    mensagem += "Semestre: " + Convert.ToString(reader["SEMESTRE"]) + " <br /> ";
                    mensagem += "Objetivo: " + Convert.ToString(reader["OBJETIVO"]) + " <br /> ";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();


            }

            return mensagem;
        }
        
        #endregion    
    }
}
