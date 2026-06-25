using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ProdutoServico
    {
        public bool PossuiTipoProdutoServicoPor(DataContext contexto, int tipoProdutoServicoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                    WHERE TIPOPRODUTOSERVICOID = @TIPOPRODUTOSERVICOID ";

            contextQuery.Parameters.Add("@TIPOPRODUTOSERVICOID", SqlDbType.Int, tipoProdutoServicoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiProdutoServicoGrupoPor(DataContext contexto, int produtoServicoGrupoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                    WHERE PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID ";

            contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServicoGrupoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiUnidadeMedidaPor(DataContext contexto, int unidadeMedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                    WHERE UNIDADEMEDIDAID = @UNIDADEMEDIDAID ";

            contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiFinalidadePor(DataContext contexto, int finalidadeId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                    WHERE FINALIDADEID = @FINALIDADEID ";

            contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public Entidades.ProdutoServico ObtemPor(int produtoServicoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.ProdutoServico produtoServico = new Entidades.ProdutoServico();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                        WHERE PRODUTOSERVICOID = @PRODUTOSERVICOID";

                contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);

                produtoServico = contexto.TryToBindEntity<Entidades.ProdutoServico>(contextQuery);

                return produtoServico;
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

        public ValidacaoDados Valida(Entidades.ProdutoServico produtoServico, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (produtoServico == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (produtoServico.ProdutoServicoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (produtoServico.ProdutoServicoGrupoId <= 0)
            {
                mensagens.Add("Campo GRUPO é obrigatório.");
            }

            if (produtoServico.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PRODUTO OU SERVIÇO é obrigatório.");
            }

            if (produtoServico.TipoProdutoServicoId <= 0)
            {
                mensagens.Add("Campo TIPO é obrigatório.");
            }

            if (produtoServico.UnidadeMedidaId <= 0)
            {
                mensagens.Add("Campo UNIDADE DE MEDIDA é obrigatório.");
            }

            if (produtoServico.FinalidadeId <= 0)
            {
                mensagens.Add("Campo FINALIDADE é obrigatório.");
            }

            if (produtoServico.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe o nome cadastrado
                    if (this.PossuiOutroNomeCadastradoPor(contexto, produtoServico.Nome, produtoServico.ProdutoServicoId, produtoServico.UnidadeMedidaId))
                    {
                        mensagens.Add("O NOME já foi cadastrado com esta UNIDADE DE MEDIDA.");
                    }

                    if (produtoServico.CodigoFgv != null && produtoServico.CodigoFgv > 0)
                    {
                        // Verifica se já existe o codigo FGV cadastrado
                        if (this.PossuiOutroCodigoFgvCadastradoPor(contexto, Convert.ToInt32(produtoServico.CodigoFgv), produtoServico.ProdutoServicoId, produtoServico.UnidadeMedidaId))
                        {
                            mensagens.Add("O CÓDIGO da TABELA DE PREÇOS DE VALORES MÁXIMOS já foi cadastrado com esta UNIDADE DE MEDIDA.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool PossuiOutroNomeCadastradoPor(DataContext ctx, string nome, int produtoServicoId, int unidadeMedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                WHERE NOME = @NOME
	                                AND UNIDADEMEDIDAID = @UNIDADEMEDIDAID
	                                AND PRODUTOSERVICOID <> @PRODUTOSERVICOID ";

            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutroCodigoFgvCadastradoPor(DataContext ctx, int codigoFgv, int produtoServicoId, int unidadeMedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.PRODUTOSERVICO (NOLOCK)
                                WHERE CODIGOFGV = @CODIGOFGV
	                                AND UNIDADEMEDIDAID = @UNIDADEMEDIDAID
	                                AND PRODUTOSERVICOID <> @PRODUTOSERVICOID ";

            contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.VarChar, codigoFgv);
            contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServicoId);
            contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.ProdutoServico produtoServico)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  INSERT INTO PrestacaoContas.PRODUTOSERVICO
                                                   (TIPOPRODUTOSERVICOID
                                                   ,FINALIDADEID
                                                   ,PRODUTOSERVICOGRUPOID
                                                   ,UNIDADEMEDIDAID
                                                   ,NOME
                                                   ,DETALHE
                                                   ,INVENTARIAVEL
                                                   ,PEQUENASDESPESAS
                                                   ,NECESSITAORCAMENTO
                                                   ,NCM
                                                   ,FLAGNCM
                                                   ,CODIGOFGV
                                                   ,ATIVO
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@TIPOPRODUTOSERVICOID,
                                                   @FINALIDADEID,
                                                   @PRODUTOSERVICOGRUPOID,
                                                   @UNIDADEMEDIDAID, 
                                                   @NOME, 
                                                   @DETALHE, 
                                                   @INVENTARIAVEL,
                                                   @PEQUENASDESPESAS, 
                                                   @NECESSITAORCAMENTO, 
                                                   @NCM, 
                                                   @FLAGNCM, 
                                                   @CODIGOFGV,
                                                   @ATIVO, 
                                                   @USUARIOID, 
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO)

                            SELECT IDENT_CURRENT('PrestacaoContas.PRODUTOSERVICO')";

                contextQuery.Parameters.Add("@TIPOPRODUTOSERVICOID", SqlDbType.Int, produtoServico.TipoProdutoServicoId);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, produtoServico.FinalidadeId);
                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServico.ProdutoServicoGrupoId);
                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, produtoServico.UnidadeMedidaId);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, produtoServico.Nome);
                contextQuery.Parameters.Add("@DETALHE", SqlDbType.VarChar, produtoServico.Detalhe);
                contextQuery.Parameters.Add("@INVENTARIAVEL", SqlDbType.Bit, produtoServico.Inventariavel);
                contextQuery.Parameters.Add("@NECESSITAORCAMENTO", SqlDbType.Bit, produtoServico.NecessitaOrcamento);
                contextQuery.Parameters.Add("@PEQUENASDESPESAS", SqlDbType.Bit, produtoServico.PequenasDespesas);
                contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, produtoServico.Ncm);
                contextQuery.Parameters.Add("@FLAGNCM", SqlDbType.Bit, produtoServico.FlagNcm);

                if (produtoServico.CodigoFgv == null || produtoServico.CodigoFgv <= 0)
                {
                    contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.Int, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.Int, produtoServico.CodigoFgv);
                }

                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, produtoServico.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, produtoServico.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                produtoServico.ProdutoServicoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));

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

        public void Atualiza(Entidades.ProdutoServico produtoServico)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.PRODUTOSERVICO
                                               SET TIPOPRODUTOSERVICOID = @TIPOPRODUTOSERVICOID, 
                                                  FINALIDADEID = @FINALIDADEID, 
                                                  PRODUTOSERVICOGRUPOID = @PRODUTOSERVICOGRUPOID, 
                                                  UNIDADEMEDIDAID = @UNIDADEMEDIDAID, 
                                                  NOME = @NOME, 
                                                  DETALHE = @DETALHE, 
                                                  INVENTARIAVEL = @INVENTARIAVEL, 
                                                  PEQUENASDESPESAS = @PEQUENASDESPESAS, 
                                                  NECESSITAORCAMENTO = @NECESSITAORCAMENTO, 
                                                  NCM = @NCM, 
                                                  FLAGNCM = @FLAGNCM, 
                                                  CODIGOFGV = @CODIGOFGV, 
                                                  USUARIOID = @USUARIOID, 
                                                  DATAALTERACAO = @DATAALTERACAO
                                             WHERE PRODUTOSERVICOID = @PRODUTOSERVICOID ";

                contextQuery.Parameters.Add("@PRODUTOSERVICOID", SqlDbType.Int, produtoServico.ProdutoServicoId);
                contextQuery.Parameters.Add("@TIPOPRODUTOSERVICOID", SqlDbType.Int, produtoServico.TipoProdutoServicoId);
                contextQuery.Parameters.Add("@PRODUTOSERVICOGRUPOID", SqlDbType.Int, produtoServico.ProdutoServicoGrupoId);
                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, produtoServico.UnidadeMedidaId);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, produtoServico.FinalidadeId);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, produtoServico.Nome);
                contextQuery.Parameters.Add("@DETALHE", SqlDbType.VarChar, produtoServico.Detalhe);
                contextQuery.Parameters.Add("@INVENTARIAVEL", SqlDbType.Bit, produtoServico.Inventariavel);
                contextQuery.Parameters.Add("@NECESSITAORCAMENTO", SqlDbType.Bit, produtoServico.NecessitaOrcamento);
                contextQuery.Parameters.Add("@PEQUENASDESPESAS", SqlDbType.Bit, produtoServico.PequenasDespesas);
                contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, produtoServico.Ncm);
                contextQuery.Parameters.Add("@FLAGNCM", SqlDbType.Bit, produtoServico.FlagNcm);

                if (produtoServico.CodigoFgv == null || produtoServico.CodigoFgv <= 0)
                {
                    contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.Int, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.Int, produtoServico.CodigoFgv);
                }

                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, produtoServico.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, produtoServico.UsuarioId);
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

        public DataTable ListaCodigoFgvPor(string ncm, int unidadeMedidaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CODIGOFGV,CONVERT(varchar, CODIGOFGV) + ' - ' + P.NOME AS DESCRICAO
                                            FROM PRESTACAOCONTAS.PRODUTOSERVICO p
                                            where NCM = @NCM
                                            and UNIDADEMEDIDAID = @UNIDADEMEDIDAID ";

                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);
                contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, ncm);

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

        public decimal? ObtemValorFgvMaximoPor(string codigoFgv, int unidadeMedidaId, int eventoId)
        {
            Evento rnEvento = new Evento();
            Entidades.Evento evento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Evento();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            decimal? retorno = null; 

            try
            {
                //Bsuca dados so evento
                evento = rnEvento.ObtemPor(contexto, eventoId);

                contextQuery.Command = @" select VM.VALORMAXIMO AS VALORFGV
                                        from PrestacaoContas.PRODUTOSERVICO P (NOLOCK)
                                        inner join PrestacaoContas.PRODUTOSERVICOVALORMAXIMO vm (nolock)
	                                         on VM.PRODUTOSERVICOID = P.PRODUTOSERVICOID
		                                        AND REGIAOFGVID = (SELECT REGIAOFGVID 
					                                        FROM PrestacaoContas.REGIAOFGV__MUNICIPIO rm
						                                        inner join LY_UNIDADE_ENSINO ue on rm.MUNICIPIOID = ue.MUNICIPIO
					                                        where ue.UNIDADE_ENS = @CENSO)
		                                        AND MONTH(@DATAPAGAMENTO) BETWEEN MONTH(VM.DATAINICIO) AND MONTH(VM.DATAFIM)
                                        where UNIDADEMEDIDAID = @UNIDADEMEDIDAID
	                                          and CODIGOFGV = @CODIGOFGV "; //Ajustar query para receber parametros

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, evento.Censo);
                contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, evento.DataPagamento);
                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);
                contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.VarChar, codigoFgv); 

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["VALORFGV"]);
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

        public bool EhCodigoFgvValidoPor(DataContext contexto, string ncm, int unidadeMedidaId, string codigoFgv)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" 	SELECT count(1)
                                            FROM PRESTACAOCONTAS.PRODUTOSERVICO (NOLOCK)
                                            WHERE CODIGOFGV = @CODIGOFGV
	                                            AND UNIDADEMEDIDAID = @UNIDADEMEDIDAID
												AND NCM = @NCM ";

            contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, ncm);
            contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.VarChar, codigoFgv);
            contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);  

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemProdutoServicoIdPor(DataContext contexto, string codigoFgv, int unidadeMedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {


                contextQuery.Command = @" SELECT PRODUTOSERVICOID
                                            FROM PRESTACAOCONTAS.PRODUTOSERVICO (NOLOCK)
                                            WHERE CODIGOFGV = @CODIGOFGV
	                                            AND UNIDADEMEDIDAID = @UNIDADEMEDIDAID ";

                contextQuery.Parameters.Add("@CODIGOFGV", SqlDbType.VarChar, codigoFgv);
                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, unidadeMedidaId);  

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PRODUTOSERVICOID"]);
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

        public int? ObtemProdutoServicoIdPor(DataContext contexto, string ncm)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int? retorno = null;
            try
            {


                contextQuery.Command = @" SELECT PRODUTOSERVICOID
                                            FROM PRESTACAOCONTAS.PRODUTOSERVICO (NOLOCK)
                                            WHERE NCM = @NCM
	                                            AND FLAGNCM = 0 ";

                contextQuery.Parameters.Add("@NCM", SqlDbType.VarChar, ncm);
                

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PRODUTOSERVICOID"]);
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