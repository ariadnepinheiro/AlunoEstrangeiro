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
    public class OpcoesAssunto
    {
        #region Listar

        public DataTable ListaOpcaoAssuntoPor(int assuntoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {

                contextQuery.Command = @" SELECT OP.OPCOESASSUNTOID, 
                                           OP.DESCRICAO, 
                                           SUBSTRING(DESCRICAO, 0, 30) + '...' AS DESCRICAOPARICAL, 
                                           OP.ORDEM, 
                                           OP.ACAODEDIRECAO, 
                                           OP.RESTRITIVO 
                                    FROM   INSPECAOESCOLAR.OPCOESASSUNTO OP (NOLOCK) 
                                    WHERE  OP.ASSUNTOID = @ASSUNTOID 
                                    ORDER  BY OP.ORDEM ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

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

        public DataTable ListarSaladeAulaTipoOpcaoResposta(int campanhaId, int tipoOpcaoResposta, string unidade_ens)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {

                contextQuery.Command = @" 
select 'CPID'+ CAST(g.CAMPANHAID AS VARCHAR(5))+
'&'+'ASTID'+CAST(a.ASSUNTOID AS VARCHAR(5))+
'&'+'GRPID'+ CAST(g.GRUPOID AS VARCHAR(5))+
'&'+'OPASTIDID'+CAST(op.OPCOESASSUNTOID AS VARCHAR(10)) AS IDENTIFICACAO
,g.CAMPANHAID,a.ASSUNTOID,g.GRUPOID,a.DESCRICAO DESCRICAOASSUNTO,
op.OPCOESASSUNTOID,op.DESCRICAO DESCRICAOOPCAO,op.ORDEM ORDEMASSUNTO, op.ACAODEDIRECAO,op.RESTRITIVO,g.DESCRICAO Grupo

    from InspecaoEscolar.CAMPANHA c 
    left join InspecaoEscolar.GRUPO g on (g.campanhaid= c.campanhaid)
    left join InspecaoEscolar.ASSUNTO a on( a.GRUPOID=g.GRUPOID)
    left join InspecaoEscolar.OPCOESASSUNTO op on(a.ASSUNTOID=op.ASSUNTOID)

    where c.campanhaid=@campanhaId
    and a.TIPOASSUNTOID=@tipoOpcaoResposta
    and op.acaodedirecao=1
    order by op.ordem
";


                contextQuery.Parameters.Add("@campanhaId", SqlDbType.Int, campanhaId);
                contextQuery.Parameters.Add("@tipoOpcaoResposta", SqlDbType.Int, tipoOpcaoResposta);
                contextQuery.Parameters.Add("@unidade_ens", SqlDbType.VarChar, unidade_ens);


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

        //Métodos para inserir
        //Método  public ValidacaoDados validaxxxx para Validar inserção
        //Método  bool para fazer a checagem no banco

        public bool Insere(Entidades.OpcoesAssunto dadosOpcoesAssunto)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();           
            bool retorno = false;
            try
            {
                retorno = this.Insere(contexto, dadosOpcoesAssunto);
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

        public bool Insere(DataContext contexto, Entidades.OpcoesAssunto dadosOpcoesAssunto)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" INSERT INTO InspecaoEscolar.opcoesassunto
                                                (DESCRICAO
                                               ,ORDEM
                                               ,ASSUNTOID
                                               ,ACAODEDIRECAO
                                               ,RESTRITIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                    VALUES     (@DESCRICAO
                                               ,@ORDEM
                                               ,@ASSUNTOID
                                               ,@ACAODEDIRECAO
                                               ,@RESTRITIVO
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO)

                                    SELECT IDENT_CURRENT('InspecaoEscolar.opcoesassunto')";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosOpcoesAssunto.Descricao);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosOpcoesAssunto.Ordem);
            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, dadosOpcoesAssunto.AssuntoId);
            contextQuery.Parameters.Add("@ACAODEDIRECAO", SqlDbType.Bit, dadosOpcoesAssunto.AcaodeDirecao);
            contextQuery.Parameters.Add("@RESTRITIVO", SqlDbType.Bit, dadosOpcoesAssunto.Restritivo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosOpcoesAssunto.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dadosOpcoesAssunto.OpcoesAssuntoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
            
            retorno = true;

            return retorno;
        }
        
        public ValidacaoDados Valida(Entidades.OpcoesAssunto OpAssunto)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (OpAssunto == null)
            {
                return validacaoDados;
            }

            if (OpAssunto.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo Descrição é obrigatório.");
            }

            if (OpAssunto.Ordem == null)
            {
                mensagens.Add("Campo ORDEM é obrigatório.");
            }

            if (OpAssunto.AssuntoId == null)
            {
                mensagens.Add("Campo ASSUNTOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a descrição ou a ordem já existem
                    if (this.PossuiDescricaoOrdemporAssuntoId(contexto, OpAssunto.AssuntoId, Convert.ToInt32(OpAssunto.Ordem), OpAssunto.Descricao, OpAssunto.OpcoesAssuntoId))
                    {
                        mensagens.Add("A ORDEM e/ou a DESCRIÇÃO informada já foi informada para outro registro neste grupo.");
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

        /// <summary>
        /// Utilizado para verificar se já existe Descrição ou Ordem já cadastrada pelo assunto
        /// </summary>
        /// <param name="ctx">Conexão</param>
        /// <param name="assuntoId">Id do assunto</param>
        /// <param name="ordem">Ordem as ser cadastrada</param>
        /// <param name="descricao">Descricção a ser cadastrada</param>
        /// <returns></returns>
        private bool PossuiDescricaoOrdemporAssuntoId(DataContext ctx, int assuntoId, int ordem, string descricao, int opcoesassuntoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;


            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.OPCOESASSUNTO OP
                                         WHERE (OP.ASSUNTOID = @ASSUNTOID AND (OP.ORDEM=@ORDEM OR OP.DESCRICAO=@DESCRICAO))
										 AND OP.OPCOESASSUNTOID<>@OPCOESASSUNTOID;
                                        
            ";

            contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);
            //contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, grupoid);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, ordem);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.VarChar, opcoesassuntoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }
        
        #endregion

        #region Update

        //Métodos para atualizar
        //Método  public ValidacaoDados validaxxxx para Validar atualizar
        //Método  bool para fazer a checagem no banco
        
        public bool Atualiza(Entidades.OpcoesAssunto dadosOpcoesAssunto)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @" UPDATE InspecaoEscolar.OPCOESASSUNTO SET
                                                DESCRICAO=@DESCRICAO
                                               ,ORDEM=@ORDEM
                                               ,ASSUNTOID=@ASSUNTOID
                                               ,ACAODEDIRECAO=@ACAODEDIRECAO
                                               ,RESTRITIVO=@RESTRITIVO
                                               ,USUARIOID=@USUARIOID
                                               ,DATAALTERACAO=@DATAALTERACAO
                                             WHERE OPCOESASSUNTOID = @OPCOESASSUNTOID
                                                ";

                contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, dadosOpcoesAssunto.OpcoesAssuntoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dadosOpcoesAssunto.Descricao);
                contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, dadosOpcoesAssunto.Ordem);
                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, dadosOpcoesAssunto.AssuntoId);
                contextQuery.Parameters.Add("@GRUPOID", SqlDbType.Int, dadosOpcoesAssunto.AssuntoId);
                contextQuery.Parameters.Add("@ACAODEDIRECAO", SqlDbType.Bit, dadosOpcoesAssunto.AcaodeDirecao);
                contextQuery.Parameters.Add("@RESTRITIVO", SqlDbType.Bit, dadosOpcoesAssunto.Restritivo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosOpcoesAssunto.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        #region Deletar

        //Métodos para Deletar
        //Método  public ValidacaoDados validaxxxx para Validar Deletar
        //Método  bool para fazer a checagem no banco

        public bool Remover(int opcoesAssuntoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            try
            {

                contextQuery.Command = @" DELETE FROM INSPECAOESCOLAR.OPCOESASSUNTO 
                                          WHERE OPCOESASSUNTOID = @OPCOESASSUNTOID
                                                ";

                contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, opcoesAssuntoId);


                contexto.ApplyModifications(contextQuery);
                retorno = true;
            }
            catch (Exception ex)
            {

                string mensagem = string.Empty;
                contexto.Abandon();


                if (Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        public ValidacaoDados ValidaRemocao(int opcoesAssuntoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };



            if (opcoesAssuntoId == 0)
            {
                mensagens.Add("Campo opcoesAssuntoId é obrigatório.");
            }


            if (mensagens.Count == 0)
            {

                try
                {

                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();



                    //Verifica se a descrição ou a ordem já existem
                    if (this.PossuiRespostaAssuntoporOpcaoAssuntoId(contexto, opcoesAssuntoId))
                    {
                        mensagens.Add("Não é possível excluir esta opção, pois a mesma possui uma resposta de campanhas.");
                    }

                    if (mensagens.Count == 0)
                    {

                        if (this.PossuiRespostaDependenciaporOpcaoAssuntoId(contexto, opcoesAssuntoId))
                        {
                            mensagens.Add("Não é possível excluir esta opção, pois a mesma possui uma resposta dependencia por opcao.");
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

        private bool PossuiRespostaDependenciaporOpcaoAssuntoId(DataContext ctx, int opcoesAssuntoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.RESPOSTADEPENDENCIAOPCAO RDO
                                         WHERE RDO.OPCOESASSUNTOID=@OPCOESASSUNTOID";

            contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, opcoesAssuntoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        private bool PossuiRespostaAssuntoporOpcaoAssuntoId(DataContext ctx, int opcoesAssuntoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"SELECT COUNT(0) EXISTE 
                                         FROM INSPECAOESCOLAR.RESPOSTAASSUNTO RA
                                         WHERE RA.OPCOESASSUNTOID=@OPCOESASSUNTOID";

            contextQuery.Parameters.Add("@OPCOESASSUNTOID", SqlDbType.Int, opcoesAssuntoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        #endregion

        public ICollection<RN.InspecaoEscolar.Entidades.OpcoesAssunto> ObtemPor(DataContext contexto, int campanhaId)
        {
            ICollection<RN.InspecaoEscolar.Entidades.OpcoesAssunto> opAssunto = new List<RN.InspecaoEscolar.Entidades.OpcoesAssunto>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" select op.*
                                    from InspecaoEscolar.OPCOESASSUNTO op
									 inner join InspecaoEscolar.ASSUNTO a on (op.ASSUNTOID=a.ASSUNTOID)
									 inner join InspecaoEscolar.GRUPO g on (a.GRUPOID=g.GRUPOID)
									 inner join InspecaoEscolar.CAMPANHA c on (c.CAMPANHAID = g.CAMPANHAID)
                                    where c.CAMPANHAID = @campanhaId ";

            contextQuery.Parameters.Add("@campanhaId", SqlDbType.Int, campanhaId);

            opAssunto = contexto.TryToBindEntities<RN.InspecaoEscolar.Entidades.OpcoesAssunto>(contextQuery);
            return opAssunto;

        }

        public List<DadosRelatorioInspecaoOpcao> ObtemDadosOpcaoPor(int assuntoId)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                return this.ObtemDadosOpcaoPor(contexto, assuntoId); ;
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

        public List<DadosRelatorioInspecaoOpcao> ObtemDadosOpcaoPor(DataContext contexto, int assuntoId)
        {
            List<DadosRelatorioInspecaoOpcao> lista = new List<DadosRelatorioInspecaoOpcao>();
            DadosRelatorioInspecaoOpcao dados = new DadosRelatorioInspecaoOpcao();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT OPCOESASSUNTOID, 
                                           DESCRICAO, 
                                           ORDEM, 
                                           ASSUNTOID, 
                                           ACAODEDIRECAO, 
                                           RESTRITIVO
                                    FROM   [INSPECAOESCOLAR].[OPCOESASSUNTO] (NOLOCK) 
                                    WHERE  ASSUNTOID = @ASSUNTOID
                                    ORDER BY ORDEM ";

                contextQuery.Parameters.Add("@ASSUNTOID", SqlDbType.Int, assuntoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados = new DadosRelatorioInspecaoOpcao();

                    dados.AssuntoId = Convert.ToInt32(reader["ASSUNTOID"]);
                    dados.OpcoesAssuntoId = Convert.ToInt32(reader["OPCOESASSUNTOID"]);
                    dados.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    dados.Ordem = Convert.ToInt32(reader["ORDEM"]);
                    dados.AcaodeDirecao = Convert.ToBoolean(reader["ACAODEDIRECAO"]);
                    dados.Restritivo = Convert.ToBoolean(reader["RESTRITIVO"]);
                    dados.valor = Convert.ToString(reader["OPCOESASSUNTOID"]) + "&" + Convert.ToInt32(reader["ASSUNTOID"]);

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

        public int RetornaQuantidadePor(DataContext contexto, int campanhaId, bool salaAula, bool Banheiro)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            StringBuilder sql = new StringBuilder();
            int retorno = 0;
            try
            {
                sql.Append(@" SELECT COUNT(*) AS QUANTIDADE
                                FROM   INSPECAOESCOLAR.OPCOESASSUNTO O (NOLOCK) 
                                       INNER JOIN INSPECAOESCOLAR.ASSUNTO A (NOLOCK) 
                                               ON O.ASSUNTOID = A.ASSUNTOID 
	                                   INNER JOIN INSPECAOESCOLAR.GRUPO G (NOLOCK)
	                                           ON G.GRUPOID = A.GRUPOID
                                WHERE G.CAMPANHAID = @CAMPANHAID
                                            ");

                if (salaAula) //--6  DEPENDÊNCIAS - SALA DE AULA
                {
                    sql.Append(@" AND  A.TIPOASSUNTOID = 6 ");
                }

                if (Banheiro) //--7  DEPENDÊNCIAS - BANHEIRO 
                {
                    sql.Append(@" AND  A.TIPOASSUNTOID = 7 ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CAMPANHAID", SqlDbType.Int, campanhaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
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
