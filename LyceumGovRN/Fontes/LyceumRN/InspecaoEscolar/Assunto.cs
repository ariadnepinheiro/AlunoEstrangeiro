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
    public class Assunto : RNBase
    {
        public enum EnumRestricao
        {
            [StringValue("Sem Restricao")]
            SemRestricao = 0,
            [StringValue("Apenas Números")]
            ApenasNumero = 1,
            [StringValue("Data")]
            Data = 2
        }

        #region Listar

        public DataTable ListarAssunto(int grupoid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT A.ASSUNTOID,
                                               A.DESCRICAO      ASSUNTO,
                                               A.ORDEM,
                                               A.GRUPOID,
                                               G.DESCRICAO      GRUPO,
                                               A.TIPOASSUNTOID,
                                               TA.DESCRICAO     TIPOASSUNTO,
                                               A.ACAODEDIRECAO,
                                               Z.DESCRICAO      ASSUNTOPAI,
                                               A.IDPAIASSUNTOID ASSUNTOPAIID,
	                                           A.RESTRICAO
                                        FROM   InspecaoEscolar.ASSUNTO A
                                               INNER JOIN InspecaoEscolar.TIPOASSUNTO TA
                                                 ON ( A.TIPOASSUNTOID = TA.TIPOASSUNTOID )
                                               INNER JOIN InspecaoEscolar.GRUPO G
                                                 ON G.GRUPOID = A.GRUPOID
                                               LEFT JOIN InspecaoEscolar.ASSUNTO Z
                                                      ON Z.ASSUNTOID = A.IDPAIASSUNTOID
                                        WHERE  A.GRUPOID = @GRUPOID
                                        ORDER  BY A.ORDEM  ";

                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoid);

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

        public DataTable ListarAssuntoSimples(int grupoid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @"  
                                            SELECT   A.ASSUNTOID,A.DESCRICAO ASSUNTO 
                                            FROM INSPECAOESCOLAR.ASSUNTO A
		                                    WHERE A.GRUPOID=@GRUPOID
		                                    ORDER BY A.ORDEM;
                                        ";

                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoid);

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

        public DataTable ListarAssuntoPai(int grupoid)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @"  SELECT A.ASSUNTOID ASSUNTOPAIID, A.DESCRICAO ASSUNTOPAI

		                                         FROM INSPECAOESCOLAR.ASSUNTO A
		                                         
		                                         WHERE A.GRUPOID=@GRUPOID
                                                   AND A.TIPOASSUNTOID NOT IN(6,7)";

                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoid);

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

        public int ListaTipoAssuntoId(int assuntoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            int ID = 0;

            try
            {
                contextQuery.Command = @"  SELECT  A.TIPOASSUNTOID 
		                                         FROM INSPECAOESCOLAR.ASSUNTO A
		                                         
		                                         WHERE A.ASSUNTOID=@ASSUNTOID ;
                                        ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

                ID = contexto.GetReturnValue<int>(contextQuery);
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

            return ID;
        }

        public DataTable ListaAssuntoSalaAulaPor(int campanhaId)
        {
            int tipoAssuntoId = 6; //6	DEPENDÊNCIAS - SALA DE AULA
            return this.ListaAssuntoPor(campanhaId, tipoAssuntoId);
        }

        public DataTable ListaAssuntoBanheiroPor(int campanhaId)
        {
            int tipoAssuntoId = 7; //7	DEPENDÊNCIAS - BANHEIRO
            return this.ListaAssuntoPor(campanhaId, tipoAssuntoId);
        }

        private DataTable ListaAssuntoPor(int campanhaId, int tipoAssuntoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT ASSUNTOID, 
                                                        A.DESCRICAO, 
                                                        A.ORDEM, 
                                                        A.ACAODEDIRECAO 
                                        FROM   INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                               INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                                       ON A.GRUPOID = G.GRUPOID 
                                        WHERE  TIPOASSUNTOID = @TIPOASSUNTOID
                                               AND CAMPANHAID = @CAMPANHAID  ";

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@TIPOASSUNTOID", SqlDbType.Int, tipoAssuntoId);

                dt = contexto.GetDataTable(contextQuery);
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

            return dt;
        }

        #endregion

        #region Inserir

        //Métodos para inserir
        //Método  public ValidacaoDados validaxxxx para Validar inserção
        //Método  bool para fazer a checagem no banco

        public bool Insere(Entidades.Assunto dadosAssunto)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {
                retorno = this.Insere(contexto, dadosAssunto);
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

        public bool Insere(DataContext contexto, Entidades.Assunto dadosAssunto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" INSERT INTO InspecaoEscolar.assunto
                                                (DESCRICAO
                                               ,ORDEM
                                               ,GRUPOID
                                               ,TIPOASSUNTOID
                                               ,ACAODEDIRECAO
                                               ,IDPAIASSUNTOID
                                               ,RESTRICAO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                    VALUES     (@DESCRICAO
                                               ,@ORDEM
                                               ,@GRUPOID
                                               ,@TIPOASSUNTOID
                                               ,@ACAODEDIRECAO
                                               ,@IDPAIASSUNTOID
                                               ,@RESTRICAO
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO) 
                                    SELECT IDENT_CURRENT('InspecaoEscolar.assunto')";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosAssunto.Descricao);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosAssunto.Ordem);
            contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, dadosAssunto.GrupoId);
            contextQuery.Parameters.Add("@TIPOASSUNTOID", SqlDbType.Int, dadosAssunto.TipoAssuntoId);
            contextQuery.Parameters.Add("@ACAODEDIRECAO", SqlDbType.Bit, dadosAssunto.AcaodeDirecao);
            contextQuery.Parameters.Add("@IDPAIASSUNTOID", SqlDbType.Int, dadosAssunto.IdPaiAssuntoId);
            contextQuery.Parameters.Add("@RESTRICAO", SqlDbType.Int, dadosAssunto.Restricao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosAssunto.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dadosAssunto.AssuntoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

            retorno = true;
            return retorno;
        }
        #endregion

        #region Update

        //Métodos para atualizar
        //Método  public ValidacaoDados validaxxxx para Validar atualizar
        //Método  bool para fazer a checagem no banco

        public bool Atualiza(Entidades.Assunto dadosAssunto)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @" UPDATE InspecaoEscolar.ASSUNTO SET
                                               DESCRICAO=@DESCRICAO
                                               ,ORDEM=@ORDEM
                                               ,GRUPOID=@GRUPOID
                                               ,TIPOASSUNTOID=@TIPOASSUNTOID
                                               ,ACAODEDIRECAO=@ACAODEDIRECAO
                                               ,IDPAIASSUNTOID=@IDPAIASSUNTOID
                                               ,RESTRICAO=@RESTRICAO
                                               ,USUARIOID=@USUARIOID
                                               ,DATAALTERACAO=@DATAALTERACAO
                                            WHERE ASSUNTOID = @ASSUNTOID
                                                ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, dadosAssunto.AssuntoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosAssunto.Descricao);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosAssunto.Ordem);
                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, dadosAssunto.GrupoId);
                contextQuery.Parameters.Add("@TIPOASSUNTOID", SqlDbType.Int, dadosAssunto.TipoAssuntoId);
                contextQuery.Parameters.Add("@ACAODEDIRECAO", SqlDbType.Bit, dadosAssunto.AcaodeDirecao);
                contextQuery.Parameters.Add("@IDPAIASSUNTOID", SqlDbType.Int, dadosAssunto.IdPaiAssuntoId);
                contextQuery.Parameters.Add("@RESTRICAO", SqlDbType.Int, dadosAssunto.Restricao);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosAssunto.UsuarioId);
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

        #region Deletar

        //Métodos para Deletar
        //Método  public ValidacaoDados validaxxxx para Validar Deletar
        //Método  bool para fazer a checagem no banco
        public bool Remover(int assuntoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @" DELETE FROM INSPECAOESCOLAR.ASSUNTO
                                            WHERE ASSUNTOID = @ASSUNTOID
                                                ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);


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

        #region Validação

        public ValidacaoDados Valida(Entidades.Assunto Assunto, int grupo, int campanhaid)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (Assunto == null)
            {
                return validacaoDados;
            }

            if (Assunto.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo Descrição é obrigatório.");
            }

            if (Assunto.Ordem == null)
            {
                mensagens.Add("Campo ORDEM é obrigatório.");
            }

            if (Assunto.GrupoId == null)
            {
                mensagens.Add("Campo GRUPO é obrigatório.");
            }
            else
            {
                if (Assunto.GrupoId != grupo)
                {
                    mensagens.Add("Campo GRUPO deve ser do mesmo tipo.");
                }
            }

            if (Assunto.TipoAssuntoId == null)
            {
                mensagens.Add("Campo TIPOASSUNTO é obrigatório.");
            }
            else
            {
                if ((Assunto.TipoAssuntoId == 6) && (Assunto.IdPaiAssuntoId != null))
                {
                    mensagens.Add("SALA DE AULA não pode ser filho.");
                }
                if ((Assunto.TipoAssuntoId == 7) && (Assunto.IdPaiAssuntoId != null))
                {
                    mensagens.Add("BANHEIRO não pode ser filho.");
                }

                //Caso não seja Descritivo colocar sem restrição
                if (Assunto.TipoAssuntoId != 4)
                {
                    Assunto.Restricao = 0;
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {

                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //verificar é update ou se é inserção

                    // se for update , não pode alterar se já estiver utilizado como resposta assunto ou se tiver opções assunto

                    if (Assunto.AssuntoId != 0)// update
                    {
                        //Verifica se JÁ FOI UTILLIZADO pelo ASSUNTO
                        if (this.UtilizadocomoAssuntoPai(contexto, Assunto.AssuntoId))
                        {
                            mensagens.Add("O ASSUNTO não pode ser atualizado pois é utilizado como ASSUNTOPAI.");
                        }

                        if (mensagens.Count == 0)
                        {
                            if (this.UtilizadocomoRespostaAssunto(contexto, Assunto.AssuntoId))
                            {
                                mensagens.Add("O ASSUNTO não pode ser atualizado pois é utilizado como RESPOSTA ASSUNTO.");
                            }
                        }

                        if (mensagens.Count == 0)
                        {
                            if (this.UtilizadocomoOpcoesAssunto(contexto, Assunto.AssuntoId))
                            {
                                mensagens.Add("O ASSUNTO não pode ser atualizado pois é utilizado como OPÇÕES ASSUNTO.");
                            }
                        }
                    }


                    //Verifica se a descrição ou a ordem já existem
                    if (this.PossuiDescricaoOrdemporGrupo(contexto, Assunto.AssuntoId, Convert.ToInt32(Assunto.Ordem), Convert.ToInt32(Assunto.GrupoId), Assunto.Descricao))
                    {
                        mensagens.Add("O ASSUNTO não pode ser inserido/atualizado pois possui ORDEM e/ou DESCRIÇÃO já cadastrados, e não podem ser repetidos.");
                    }
                    //VERIFICA SE JÁ EXISTE UM BANHEIRO POR CAMPANHA
                    if (Assunto.TipoAssuntoId == 7)
                    {
                        if (this.PossuiBanheiroporCampanha(contexto, campanhaid))
                        {
                            mensagens.Add("O ASSUNTO não pode ser inserido/atualizado pois só poderá haver 1 tipo de pergunta  Banheiro por campanha.;");
                        }
                    }
                    if (Assunto.TipoAssuntoId == 6)
                    {
                        //VERIFICA SE JÁ EXISTE UM SALA DE AULA POR CAMPANHA
                        if (this.PossuiSaladeAulaporCampanha(contexto, campanhaid))
                        {
                            mensagens.Add("O ASSUNTO não pode ser inserido/atualizado pois só poderá haver 1 tipo de pergunta  Sala de aula por campanha.;");
                        }
                    }

                    if (Assunto.IdPaiAssuntoId != null)
                    {
                        //se não for nulo, verificar se está correto

                        // se o assunto pai é igual ao assuntoid
                        if (Assunto.AssuntoId == Assunto.IdPaiAssuntoId)
                        {
                            //mensagem de erro
                            mensagens.Add("O ASSUNTO PAI, não pode ser ele mesmo.");
                        }
                        else

                            //Verifica se a o assunto pai faz parte do Grupo
                            if (!this.PossuiAssuntoPaiCorreto(contexto, Assunto.IdPaiAssuntoId, Convert.ToInt32(Assunto.GrupoId)))
                            {
                                mensagens.Add("O ASSUNTO PAI não pode ser cadastrado, por não fazer parte do GRUPO.");
                            }
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

        private bool PossuiDescricaoOrdemporGrupo(DataContext ctx, int assuntoId, int ordem, int grupoid, string descricao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.ASSUNTO A
                                         WHERE (A.GRUPOID=@GRUPOID AND (A.ORDEM=@ORDEM OR A.DESCRICAO=@DESCRICAO))
                                         AND A.ASSUNTOID <> @ASSUNTOID
            ";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);
            contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoid);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, ordem);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool PossuiSaladeAulaporCampanha(DataContext ctx, int Campanhaid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE
		                                         FROM INSPECAOESCOLAR.ASSUNTO A
												 left join InspecaoEscolar.GRUPO g on a.GRUPOID= g.GRUPOID
												 where A.TIPOASSUNTOID=6 and g.CAMPANHAID=@CAMPANHAID
            ";

            contextQuery.Parameters.Add("@Campanhaid", SqlDbType.Int, Campanhaid);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool PossuiBanheiroporCampanha(DataContext ctx, int Campanhaid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE
		                                         FROM INSPECAOESCOLAR.ASSUNTO A
												 left join InspecaoEscolar.GRUPO g on a.GRUPOID= g.GRUPOID
												 where A.TIPOASSUNTOID=7 and g.CAMPANHAID=@CAMPANHAID
            ";

            contextQuery.Parameters.Add("@Campanhaid", SqlDbType.Int, Campanhaid);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool PossuiAssuntoPaiCorreto(DataContext ctx, int? idPaiAssuntoid, int grupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.ASSUNTO A
                                         WHERE A.ASSUNTOID=ASSUNTOID
										 AND A.GRUPOID=GRUPOID";



            contextQuery.Parameters.Add("@GRUPOID", SqlDbType.VarChar, grupo);
            //utilizo o idpaiassunto como assuntoid
            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.VarChar, idPaiAssuntoid);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public ValidacaoDados ValidaRemocaoAssunto(int assuntoID)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (assuntoID == 0 || assuntoID == null)
            {
                mensagens.Add("Erro ao obter o ASSUNTO.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se JÁ FOI UTILLIZADO pelo ASSUNTO
                    if (this.UtilizadocomoAssuntoPai(contexto, assuntoID))
                    {
                        mensagens.Add("O ASSUNTO não pode ser excluido pois é utilizado como ASSUNTOPAI.");
                    }
                    if (mensagens.Count == 0)
                    {
                        if (this.UtilizadocomoRespostaAssunto(contexto, assuntoID))
                        {
                            mensagens.Add("O ASSUNTO não pode ser excluido pois é utilizado como RESPOSTA ASSUNTO.");
                        }
                    }

                    if (mensagens.Count == 0)
                    {
                        if (this.UtilizadocomoOpcoesAssunto(contexto, assuntoID))
                        {
                            mensagens.Add("O ASSUNTO não pode ser excluido pois é utilizado como OPÇÕES ASSUNTO.");
                        }
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

        private bool UtilizadocomoAssuntoPai(DataContext ctx, int assuntoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.ASSUNTO G
                                         WHERE G.IDPAIASSUNTOID=@IDPAIASSUNTOID";

            contextQuery.Parameters.Add("@IDPAIASSUNTOID", SqlDbType.VarChar, assuntoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool UtilizadocomoRespostaAssunto(DataContext ctx, int assuntoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.RESPOSTAASSUNTO R
                                         WHERE R.ASSUNTOID=@ASSUNTOID";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.VarChar, assuntoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool UtilizadocomoOpcoesAssunto(DataContext ctx, int assuntoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.OPCOESASSUNTO O
                                         WHERE O.ASSUNTOID=@ASSUNTOID";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.VarChar, assuntoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        #endregion

        public ICollection<RN.InspecaoEscolar.Entidades.Assunto> ObtemPor(DataContext contexto, int campanhaid)
        {
            ICollection<RN.InspecaoEscolar.Entidades.Assunto> assunto = new List<RN.InspecaoEscolar.Entidades.Assunto>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"select a.*
                                    from InspecaoEscolar.ASSUNTO a
	                                    inner join InspecaoEscolar.GRUPO g on (g.GRUPOID = a.grupoid) 
                                    where g.campanhaid = @campanhaid ";

            contextQuery.Parameters.Add("@campanhaid", SqlDbType.Int, campanhaid);
            assunto = contexto.TryToBindEntities<RN.InspecaoEscolar.Entidades.Assunto>(contextQuery);
            return assunto;

        }

        public int RetornaIdPaiPor(DataContext contexto, int grupoIdDestino, int idPaiAssuntoOrigem)
        {
            int idPai = 0;
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @"SELECT DEST.ASSUNTOID
                                        FROM INSPECAOESCOLAR.ASSUNTO ORI
                                                INNER JOIN INSPECAOESCOLAR.ASSUNTO DEST
                                                        ON ORI.ORDEM = DEST.ORDEM 
                                                        AND ORI.DESCRICAO = DEST.DESCRICAO
                                                        AND DEST.GRUPOID = @GRUPOIDDESTINO
                                        WHERE ORI.ASSUNTOID = @IDPAIASSUNTOORIGEM ";

                contextQuery.Parameters.Add("@GRUPOIDDESTINO", SqlDbType.Int, grupoIdDestino);
                contextQuery.Parameters.Add("@IDPAIASSUNTOORIGEM", SqlDbType.Int, idPaiAssuntoOrigem);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    idPai = Convert.ToInt32(reader["ASSUNTOID"]);
                }
                if (idPai == 0)
                {
                    throw new Exception("ID DO ASSUNTO PAI NÃO IDENTIFICADO.");
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return idPai;
        }

        public Entidades.Assunto ObtemAssuntoPor(DataContext contexto, int assuntoId)
        {
            Entidades.Assunto assunto = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Assunto();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
                                            FROM INSPECAOESCOLAR.ASSUNTO (NOLOCK)
                                            WHERE ASSUNTOID = @ASSUNTOID  ";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

            assunto = contexto.TryToBindEntity<Entidades.Assunto>(contextQuery);

            return assunto;
        }

        public int ObtemRestricaoPor(int assuntoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT RESTRICAO
                                        FROM InspecaoEscolar.ASSUNTO
                                        WHERE ASSUNTOID = @ASSUNTOID ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["RESTRICAO"]);
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


        public DataRow ObtemPor(int assuntoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT * 
                                            FROM INSPECAOESCOLAR.ASSUNTO (NOLOCK)
                                            WHERE ASSUNTOID = @ASSUNTOID  ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

                DataTable assuntos = ctx.GetDataTable(contextQuery);

                if (assuntos.Rows.Count > 0)
                    return assuntos.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (ctx != null)
                {
                    ctx.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }
        }

        public bool TemOpcoesAssunto(int assuntoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" 
                                select count(oa.OPCOESASSUNTOID) as QTDOPCOESASSUNTO
                                from InspecaoEscolar.ASSUNTO a (nolock)
                                left join InspecaoEscolar.OPCOESASSUNTO oa (nolock) on a.ASSUNTOID = oa.ASSUNTOID
                                where a.ASSUNTOID = @ASSUNTOID
                                group by a.ASSUNTOID
                            ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
                if (ctx != null)
                {
                    ctx.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }
        }

        public int RetornaQuantidadeAssuntoPor_DemaisDependencias(int grupoId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                return this.RetornaQuantidadeAssuntoPor(contexto, grupoId, false, true);
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

        private int RetornaQuantidadeAssuntoPor(DataContext contexto, int grupoId, bool consideracoesFinais, bool demais)
        {
            int retorno = 0;
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT COUNT(DISTINCT A.ASSUNTOID) TOTAL 
                                        FROM   INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                        WHERE  GRUPOID = @GRUPOID 
                                               ");

                if (consideracoesFinais)
                {
                    sql.Append(@" AND TIPOASSUNTOID IN ( 8, 9, 10, 11 ) 
                                         ");
                }

                if (demais)
                {
                    sql.Append(@" AND TIPOASSUNTOID IN ( 2, 3, 4, 5 )
                                         ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int RetornaQuantidadeAssuntoPor_DemaisDependencias(int campanhaId, int ordemGrupo)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                return this.RetornaQuantidadeAssuntoPor(contexto, campanhaId, ordemGrupo, false, true);
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

        private int RetornaQuantidadeAssuntoPor(DataContext contexto, int campanhaId, int ordemGrupo, bool consideracoesFinais, bool demais)
        {
            int retorno = 0;
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT COUNT(DISTINCT A.ASSUNTOID) TOTAL 
                                       FROM   INSPECAOESCOLAR.GRUPO G (NOLOCK) 
                                           INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                                   ON G.GRUPOID = A.GRUPOID 
                                       WHERE  CAMPANHAID = @CAMPANHAID
											AND G.ORDEM = @ORDEM 
                                               ");

                if (consideracoesFinais)
                {
                    sql.Append(@" AND A.TIPOASSUNTOID IN ( 8, 9, 10, 11 ) 
                                         ");
                }

                if (demais)
                {
                    sql.Append(@" AND A.TIPOASSUNTOID IN ( 2, 3, 4, 5 )
                                         ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, ordemGrupo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosRelatorioInspecaoAssunto> ObtemDadosAssuntoPor(DataContext contexto, int grupoId, bool consideracoesFinais, bool demais, int? ordemInicio, int? ordemFim)
        {
            List<DadosRelatorioInspecaoAssunto> lista = new List<DadosRelatorioInspecaoAssunto>();
            DadosRelatorioInspecaoAssunto dados = new DadosRelatorioInspecaoAssunto();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT A.ASSUNTOID, 
                                                        A.DESCRICAO, 
                                                        A.ORDEM, 
                                                        A.GRUPOID, 
                                                        A.TIPOASSUNTOID, 
                                                        A.ACAODEDIRECAO, 
                                                        A.IDPAIASSUNTOID 
                                        FROM   INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                        WHERE  GRUPOID = @GRUPOID 
                                               ");

                if (consideracoesFinais)
                {
                    sql.Append(@" AND TIPOASSUNTOID IN ( 8, 9, 10, 11 ) 
                                         ");
                }

                if (demais)
                {
                    sql.Append(@" AND TIPOASSUNTOID IN ( 2, 3, 4, 5 )
                                         ");
                }

                if (ordemInicio != null && ordemInicio > 0)
                {
                    sql.Append(@" AND ORDEM >= @ORDEMINICIO
                                         ");

                    contextQuery.Parameters.Add("@ORDEMINICIO", SqlDbType.Int, ordemInicio);
                }

                if (ordemFim != null && ordemFim > 0)
                {
                    sql.Append(@" AND ORDEM <= @ORDEMFIM
                                         ");

                    contextQuery.Parameters.Add("@ORDEMFIM", SqlDbType.Int, ordemFim);
                }

                sql.Append(@" ORDER BY A.ORDEM  ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados = new DadosRelatorioInspecaoAssunto();

                    dados.AssuntoId = Convert.ToInt32(reader["ASSUNTOID"]);
                    dados.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    dados.GrupoId = Convert.ToInt32(reader["GRUPOID"]);
                    dados.Ordem = Convert.ToInt32(reader["ORDEM"]);
                    dados.TipoAssuntoId = Convert.ToInt32(reader["TIPOASSUNTOID"]);
                    dados.AcaodeDirecao = Convert.ToBoolean(reader["ACAODEDIRECAO"]);

                    if (reader["IDPAIASSUNTOID"] != DBNull.Value)
                    {
                        dados.IdPaiAssuntoId = Convert.ToInt32(reader["IDPAIASSUNTOID"]);
                    }

                    lista.Add(dados);
                }

                return lista;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosRelatorioInspecaoAssunto> ObtemDadosAssuntoPor_DemaisDependencias(int grupoId, int ordemAssuntoInicio, int ordemAssuntoFim)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                return this.ObtemDadosAssuntoPor(contexto, grupoId, false, true, ordemAssuntoInicio, ordemAssuntoFim);
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
    }
}