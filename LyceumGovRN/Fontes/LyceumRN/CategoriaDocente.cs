using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{ 
    public class CategoriaDocente
    {
        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CD.CATEGORIA, 
                                               CD.NOME,  
                                               CD.FUNCAO, 
	                                           F.DESCRICAO AS FUNCAODESCRICAO,                                             
                                               CD.INGRESSO, 
                                               CD.NECESSITA_SUPERIOR, 
                                               CD.FUNCIONARIO,
                                               CD.TIPO, 
                                               CD.USUARIOID, 
                                               CD.DATACADASTRO, 
                                               CD.DATAALTERACAO, 
                                               CD.AGRUPAMENTOCARGOSID, 
                                               A.DESCRICAO AS AGRUPAMENTO,                                              
                                               CD.CARGAHORARIAREGENCIA, 
                                               CD.CARGAHORARIAPLANEJAMENTO,
											   A.CARGAHORARIA AS CARGAHORARIAGRUPO,
                                              (CONVERT(VARCHAR,A.AGRUPAMENTOCARGOSID) + '_' + CONVERT(VARCHAR,A.CARGAHORARIA)) as CHAVE
                                        FROM   LY_CATEGORIA_DOCENTE CD (NOLOCK) 	   
                                                LEFT JOIN LY_FUNCAO F (NOLOCK)
			                                          ON F.FUNCAO = CD.FUNCAO                                     
                                               LEFT JOIN RECURSOSHUMANOS.AGRUPAMENTOCARGOS A (NOLOCK) 
                                                      ON CD.AGRUPAMENTOCARGOSID = A.AGRUPAMENTOCARGOSID   ";

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

        public DataTable ListaCategoriaFuncionario()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CD.CATEGORIA, 
                                               CD.NOME  
                                               FROM   LY_CATEGORIA_DOCENTE CD (NOLOCK) 	   
                                                WHERE FUNCIONARIO = 'S' ";

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

        public ValidacaoDados Valida(Entidades.LyCategoriaDocente lyCategoriaDocente, int cargaHorariaGrupo,  bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lyCategoriaDocente == null)
            {
                return validacaoDados;
            }

            if (lyCategoriaDocente.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CARGO é obrigatório.");
            }

            if (lyCategoriaDocente.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (lyCategoriaDocente.Funcao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo FUNÇÃO RELACIONADA é obrigatório.");
            }

            if (lyCategoriaDocente.Ingresso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo INGRESSO é obrigatório.");
            }
            else if (lyCategoriaDocente.Ingresso != "N" && lyCategoriaDocente.Ingresso != "S")
            {
                mensagens.Add("O campo INGRESSO inválido.");
            }

            if (lyCategoriaDocente.NecessitaSuperior.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NECESSITA SUPERIOR? é obrigatório.");
            }
            else if (lyCategoriaDocente.NecessitaSuperior != "N" && lyCategoriaDocente.NecessitaSuperior != "S")
            {
                mensagens.Add("O campo NECESSITA SUPERIOR? inválido.");
            }
            if (lyCategoriaDocente.Funcionario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo FUNCIONÁRIO? é obrigatório.");
            }
            else if (lyCategoriaDocente.NecessitaSuperior != "N" && lyCategoriaDocente.NecessitaSuperior != "S")
            {
                mensagens.Add("O campo FUNCIONÁRIO? inválido.");
            }

            if (lyCategoriaDocente.Tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }

            if (lyCategoriaDocente.AgrupamentoCargosId <= 0)
            {
                mensagens.Add("Campo GRUPO é obrigatório.");
            }          

            if (lyCategoriaDocente.CargaHorariaRegencia < 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA REGÊNCIA é obrigatório.");
            }

            if (lyCategoriaDocente.CargaHorariaPlanejamento < 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA PLANEJAMENTO é obrigatório.");
            }

            if (cargaHorariaGrupo <= 0)
            {
                mensagens.Add("Campo CARGA HORÁRIA DO GRUPO é obrigatório.");
            }

            if (lyCategoriaDocente.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }           

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (cadastro)
                    {
                        // Verifica se já existe a descrição cadastrada
                        if (this.PossuiCategoriaCadastradaPor(contexto, lyCategoriaDocente.Categoria))
                        {
                            mensagens.Add("Esta CATEGORIA já foi cadastrada.");
                        }
                    }

                    //Verifica se a carga horaria total é igual a carga horaria do grupo
                    if ((lyCategoriaDocente.CargaHorariaRegencia + lyCategoriaDocente.CargaHorariaPlanejamento) != cargaHorariaGrupo)
                    {
                        mensagens.Add(string.Format("A soma das cargas horárias deve ser igual à carga horária do grupo ({0}).", cargaHorariaGrupo.ToString()));
                    }

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutroNomeCadastradoPor(contexto, lyCategoriaDocente.Nome, lyCategoriaDocente.Categoria))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
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

        private bool PossuiCategoriaCadastradaPor(DataContext ctx, string categoria)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_CATEGORIA_DOCENTE (NOLOCK)
                                WHERE CATEGORIA = @CATEGORIA ";

            contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, categoria);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroNomeCadastradoPor(DataContext ctx, string nome, string categoria)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_CATEGORIA_DOCENTE (NOLOCK)
                                WHERE NOME = @NOME
                                       AND CATEGORIA <> @CATEGORIA ";

            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, categoria);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.LyCategoriaDocente lyCategoriaDocente)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO DBO.LY_CATEGORIA_DOCENTE 
                                                (CATEGORIA, 
                                                 NOME,
                                                 FUNCAO, 
                                                 INGRESSO, 
                                                 NECESSITA_SUPERIOR, 
                                                 FUNCIONARIO,
                                                 TIPO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO, 
                                                 AGRUPAMENTOCARGOSID,
                                                 CARGAHORARIAREGENCIA, 
                                                 CARGAHORARIAPLANEJAMENTO) 
                                    VALUES      (@CATEGORIA, 
                                                 @NOME,    
                                                 @FUNCAO,                                              
                                                 @INGRESSO, 
                                                 @NECESSITA_SUPERIOR, 
                                                 @FUNCIONARIO,
                                                 @TIPO, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO, 
                                                 @AGRUPAMENTOCARGOSID,                                                
                                                 @CARGAHORARIAREGENCIA, 
                                                 @CARGAHORARIAPLANEJAMENTO)  ";

                contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, lyCategoriaDocente.Categoria);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, lyCategoriaDocente.Nome);
                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, lyCategoriaDocente.Funcao);
                contextQuery.Parameters.Add("@INGRESSO", SqlDbType.VarChar, lyCategoriaDocente.Ingresso);
                contextQuery.Parameters.Add("@NECESSITA_SUPERIOR", SqlDbType.VarChar, lyCategoriaDocente.NecessitaSuperior);
                contextQuery.Parameters.Add("@FUNCIONARIO", SqlDbType.VarChar, lyCategoriaDocente.Funcionario);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, lyCategoriaDocente.Tipo);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, lyCategoriaDocente.AgrupamentoCargosId);               
                contextQuery.Parameters.Add("@CARGAHORARIAREGENCIA", SqlDbType.Int, lyCategoriaDocente.CargaHorariaRegencia);
                contextQuery.Parameters.Add("@CARGAHORARIAPLANEJAMENTO", SqlDbType.Int, lyCategoriaDocente.CargaHorariaPlanejamento);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, lyCategoriaDocente.UsuarioId);
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

        public void Atualiza(Entidades.LyCategoriaDocente lyCategoriaDocente)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_CATEGORIA_DOCENTE
                                        SET    NOME = @NOME,     
                                               FUNCAO = @FUNCAO,                                           
                                               INGRESSO = @INGRESSO, 
                                               NECESSITA_SUPERIOR = @NECESSITA_SUPERIOR, 
                                               FUNCIONARIO = @FUNCIONARIO,
                                               TIPO = @TIPO,                                              
                                               CARGAHORARIAREGENCIA = @CARGAHORARIAREGENCIA, 
                                               CARGAHORARIAPLANEJAMENTO = @CARGAHORARIAPLANEJAMENTO,                  
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  CATEGORIA = @CATEGORIA ";

                contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, lyCategoriaDocente.Categoria);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, lyCategoriaDocente.Nome);
                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, lyCategoriaDocente.Funcao);
                contextQuery.Parameters.Add("@INGRESSO", SqlDbType.VarChar, lyCategoriaDocente.Ingresso);
                contextQuery.Parameters.Add("@NECESSITA_SUPERIOR", SqlDbType.VarChar, lyCategoriaDocente.NecessitaSuperior);
                contextQuery.Parameters.Add("@FUNCIONARIO", SqlDbType.VarChar, lyCategoriaDocente.Funcionario);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, lyCategoriaDocente.Tipo);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, lyCategoriaDocente.AgrupamentoCargosId);               
                contextQuery.Parameters.Add("@CARGAHORARIAREGENCIA", SqlDbType.Int, lyCategoriaDocente.CargaHorariaRegencia);
                contextQuery.Parameters.Add("@CARGAHORARIAPLANEJAMENTO", SqlDbType.Int, lyCategoriaDocente.CargaHorariaPlanejamento);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, lyCategoriaDocente.UsuarioId);
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

        public string RetornaFuncaoPor(string categoria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT FUNCAO 
                            FROM LY_CATEGORIA_DOCENTE
                            WHERE CATEGORIA = @CATEGORIA ";

                contextQuery.Parameters.Add("@CATEGORIA", categoria); 

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public int ObtemAgrupamentoCargoPor(DataContext contexto, string categoria)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT AGRUPAMENTOCARGOSID 
                                        FROM   LY_CATEGORIA_DOCENTE (NOLOCK) 
                                        WHERE  CATEGORIA = @CATEGORIA ";

                contextQuery.Parameters.Add("@CATEGORIA", TechneDbType.T_CODIGO, categoria);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["AGRUPAMENTOCARGOSID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AGRUPAMENTOCARGOSID"]);
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

        public ValidacaoDados ValidaRemocao(string categoria)
        {
            List<string> mensagens = new List<string>();
            RN.Docentes rnDocente = new RN.Docentes();
            RN.ContratoTemporario.ConcursoDocente_CategoriaDocente rnConcursoDocente_CategoriaDocente = new Techne.Lyceum.RN.ContratoTemporario.ConcursoDocente_CategoriaDocente();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnDocente.PossuiCategoriaPor(contexto, categoria))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado por um docente.");
                    }

                    if (rnConcursoDocente_CategoriaDocente.PossuiCategoriaPor(contexto, categoria))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado por um processo seletivo.");
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

        public void Remove(string categoria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LY_CATEGORIA_DOCENTE
                            WHERE  CATEGORIA = @CATEGORIA  ";

                contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, categoria);

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

        public bool NecessitaCursoSuperior(string categoria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                    FROM LY_CATEGORIA_DOCENTE (NOLOCK)
                                    WHERE CATEGORIA = @CATEGORIA
                                    AND ISNULL(NECESSITA_SUPERIOR, 'N') = 'S' ";

                contextQuery.Parameters.Add("@CATEGORIA", TechneDbType.T_CODIGO, categoria);

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

        public bool PossuiAgrupamentoCargosPor(DataContext contexto, int agrupamentoCargosId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM LY_CATEGORIA_DOCENTE
                                    WHERE AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID ";

            contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargosId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaExcel()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT                                         
                                               CD.CATEGORIA AS CARGO, 
                                               CD.NOME AS DESCRIÇÃO,  
                                               CD.FUNCAO AS 'FUNÇÃO RELACIONADA', 
	                                           F.DESCRICAO AS FUNÇÃO,                                             
                                               CD.INGRESSO, 
                                               CD.NECESSITA_SUPERIOR AS 'NECESSITA SUPERIOR?', 
                                               CD.FUNCIONARIO AS 'FUNCIONÁRIO?',
                                               CD.TIPO,                                                
                                               A.DESCRICAO AS GRUPO,                                              
                                               CD.CARGAHORARIAREGENCIA AS 'C.H. REGÊNCIA', 
                                               CD.CARGAHORARIAPLANEJAMENTO AS 'C.H. PLANEJAMENTO',
											   A.CARGAHORARIA AS 'CARGA HORÁRIA TOTAL'
                                              
                                        FROM   LY_CATEGORIA_DOCENTE CD (NOLOCK) 	   
                                                LEFT JOIN LY_FUNCAO F (NOLOCK)
			                                          ON F.FUNCAO = CD.FUNCAO                                     
                                               LEFT JOIN RECURSOSHUMANOS.AGRUPAMENTOCARGOS A (NOLOCK) 
                                                      ON CD.AGRUPAMENTOCARGOSID = A.AGRUPAMENTOCARGOSID 
  ";

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


    }
}
