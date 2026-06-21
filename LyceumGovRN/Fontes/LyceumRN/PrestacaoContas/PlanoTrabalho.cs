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
    public class PlanoTrabalho
    {
        public int ObtemFinalidadePor(DataContext contexto, int planoTrabalhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT FINALIDADEID
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE PLANOTRABALHOID = @PLANOTRABALHOID  ";

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["FINALIDADEID"]);
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

        public String[] ObtemIdentificadorPor(DataContext contexto, int planoTrabalhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            string[] retorno = {"",""};
            try
            {
                contextQuery.Command = @" SELECT IDENTIFICADOR,DESCRICAO
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE PLANOTRABALHOID = @PLANOTRABALHOID  ";

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno[0] = reader["IDENTIFICADOR"].ToString();
                    retorno[1] = reader["DESCRICAO"].ToString();
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
        public bool PermitePequenaDespesa(int planoTrabalhoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return PermitePequenaDespesa(ctx, planoTrabalhoId);
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
        }

        public bool PermitePequenaDespesa(DataContext contexto, int planoTrabalhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"   SELECT COUNT(*)
                                        FROM PrestacaoContas.PLANOTRABALHO PT (NOLOCK) 
                                        WHERE PEQUENADESPESA = 1
	                                        AND PLANOTRABALHOID = @PLANOTRABALHOID ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhPlanoTrabalhoMerenda(DataContext contexto, int planoTrabalhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"   SELECT COUNT(*)
                                        FROM PrestacaoContas.PLANOTRABALHO PT (NOLOCK) 
                                        WHERE FINALIDADEID = 2 --MERENDA
	                                        AND PLANOTRABALHOID = @PLANOTRABALHOID ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTipoContratacaoPor(DataContext contexto, int tipoContratacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE TIPOCONTRATACAOID = @TIPOCONTRATACAOID ";

            contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, tipoContratacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiIdentificadorCadastradoPor(DataContext ctx, string identificador)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                WHERE IDENTIFICADOR = @IDENTIFICADOR ";

            contextQuery.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar, identificador);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiSuperintendenciaPor(DataContext contexto, int superintendenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE SUPERINTENDENCIAID = @SUPERINTENDENCIAID ";

            contextQuery.Parameters.Add("@SUPERINTENDENCIAID", SqlDbType.Int, superintendenciaId);

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
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE FINALIDADEID = @FINALIDADEID ";

            contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiTipoDespesaPor(DataContext contexto, int tipoDespesaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE TIPODESPESAID = @TIPODESPESAID ";

            contextQuery.Parameters.Add("@TIPODESPESAID", SqlDbType.Int, tipoDespesaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string PesquisaPorId(string planoTrabalhoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            var retorno = "";
            contextQuery.Command = @" SELECT DESCRICAO
                                    FROM PrestacaoContas.PLANOTRABALHO (NOLOCK)
                                    WHERE PLANOTRABALHOID = @PLANOTRABALHOID ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = reader["DESCRICAO"].ToString();
            }

            return retorno;
        }

        public DataTable ListaTodos()
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  select pttc.DESCRICAO as Tipo_Descricao, pttd.DESCRICAO as Tipo_Despesa_Descricao, grs.DESCRICAO as Superintendendia_Descricao, pt.* ,
                                             case  when pt.periodicidade = 1 then 'Anual'
                                             when pt.periodicidade = 2 then 'Mensal'
                                             when pt.periodicidade = 3 then 'Eventual'
                                             end descricao_periodicidade, pcf.DESCRICAO as Finalidade_Descricao,
	                                           CASE
		                                         WHEN PT.PEQUENADESPESA = 1 THEN 'Sim'
                                                 WHEN PT.PEQUENADESPESA = 0 THEN 'Não'
                                               END DESCRICAO_PEQUENADESPESA,
											   ps.DESCRICAO as PROGRAMATRABALHO
                                             from prestacaocontas.PLANOTRABALHO pt
                                             join PrestacaoContas.TIPOCONTRATACAO pttc on pt.TIPOCONTRATACAOID = pttc.TIPOCONTRATACAOID
                                             join PrestacaoContas.TIPODESPESA pttd on pt.TIPODESPESAID = pttd.TIPODESPESAID
                                             join GestaoRede.SUPERINTENDENCIA grs on grs.SUPERINTENDENCIAID = pt.SUPERINTENDENCIAID
                                             join PrestacaoContas.FINALIDADE pcf on pcf.FINALIDADEID = pt.FINALIDADEID
											 join PrestacaoContas.PROGRAMATRABALHO pg on pt.programatrabalhoid = pg.programatrabalhoid
											 join PrestacaoContas.WSPROGRAMASEFAZ ps on pg.WSPROGRAMASEFAZID = ps.WSPROGRAMASEFAZID
                                            ";

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

        public DataTable ListaPorProgramaTrabalho(object PROGRAMATRABALHOID)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  select pttc.DESCRICAO as Tipo_Descricao, pttd.DESCRICAO as Tipo_Despesa_Descricao, grs.DESCRICAO as Superintendendia_Descricao, pt.* ,
                                             case  when pt.periodicidade = 1 then 'Anual'
                                             when pt.periodicidade = 2 then 'Mensal'
                                             when pt.periodicidade = 3 then 'Eventual'
                                             end descricao_periodicidade, pcf.DESCRICAO as Finalidade_Descricao,
	                                           CASE
		                                         WHEN PT.PEQUENADESPESA = 1 THEN 'Sim'
                                                 WHEN PT.PEQUENADESPESA = 0 THEN 'Não'
                                               END DESCRICAO_PEQUENADESPESA,
											   ps.DESCRICAO as PROGRAMATRABALHO
                                             from prestacaocontas.PLANOTRABALHO pt
                                             join PrestacaoContas.TIPOCONTRATACAO pttc on pt.TIPOCONTRATACAOID = pttc.TIPOCONTRATACAOID
                                             join PrestacaoContas.TIPODESPESA pttd on pt.TIPODESPESAID = pttd.TIPODESPESAID
                                             join GestaoRede.SUPERINTENDENCIA grs on grs.SUPERINTENDENCIAID = pt.SUPERINTENDENCIAID
                                             join PrestacaoContas.FINALIDADE pcf on pcf.FINALIDADEID = pt.FINALIDADEID
											 join PrestacaoContas.PROGRAMATRABALHO pg on pt.programatrabalhoid = pg.programatrabalhoid
											 join PrestacaoContas.WSPROGRAMASEFAZ ps on pg.WSPROGRAMASEFAZID = ps.WSPROGRAMASEFAZID
                                          where pt.programatrabalhoid = @programatrabalhoid
                                            ";

                contextQuery.Parameters.Add("@programatrabalhoid", SqlDbType.Int, PROGRAMATRABALHOID);

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

        public ValidacaoDados Valida(Entidades.PlanoTrabalho planoTrabalho, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ProgramaTrabalho rnProgramadeTrabalho = new ProgramaTrabalho();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (!cadastro)
            {
                if (planoTrabalho.PlanoTrabalhoId <= 0)
                {
                    mensagens.Add("O campo obrigatório código não foi preenchido.");
                }
            }

            if (planoTrabalho.ProgramaTrabalhoId <= 0)
            {
                mensagens.Add("O campo obrigatório Programa de Trabalho não foi preenchido.");
            }

            if (planoTrabalho.TipoContratacaoId <= 0)
            {
                mensagens.Add("O campo obrigatório Tipo Contratação não foi preenchido.");
            }

            if (planoTrabalho.TipoDespesaId <= 0)
            {
                mensagens.Add("O campo obrigatório Tipo Despesa não foi preenchido.");
            }

            if (planoTrabalho.SuperintendenciaId <= 0)
            {
                mensagens.Add("O campo obrigatório Superintendencia não foi preenchido.");
            }

            if (planoTrabalho.Periodicidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo obrigatório Periodicidade não foi preenchido.");
            }

            if (planoTrabalho.FinalidadeId <= 0)
            {
                mensagens.Add("O campo obrigatório Finalidade não foi preenchido.");
            }
            else
            {
                //Apenas planos com finalidade manutenção podem ser do tipo pequenas despesas
                if (planoTrabalho.PequenaDespesa && planoTrabalho.FinalidadeId != 1)
                {
                    mensagens.Add("Apenas planos com finalidade manutenção podem ser do tipo pequenas despesas.");
                }
            }

            if (String.IsNullOrEmpty(planoTrabalho.Descricao))
            {
                mensagens.Add("O campo obrigatório Descricao não foi preenchido.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (cadastro)
                    {
                        //Busca PTRES
                        string ptres = rnProgramadeTrabalho.ObtemPtresPor(contexto, planoTrabalho.ProgramaTrabalhoId);

                        //Gera IdentificadorSeq
                        planoTrabalho.IdentificadorSeq = this.ObtemIdentificadorSeqPor(contexto, ptres);

                        //O campo Identificador deve ter valor único na tabela. É composto pela concatenação dos últimos quatro dígitos do PTRES 
                        //do programa de trabalho com “-“ e com o valor seguinte ao máximo do agrupamento dos demais plano de programas de trabalho 
                        //que tenham o mesmo PTRES

                        //Gera Identificador
                        planoTrabalho.Identificador = string.Format("{0}-{1}", ptres.Substring(ptres.Length - 4, 4), planoTrabalho.IdentificadorSeq.ToString());

                        //Verifica se já existe o identificador cadastrado
                        if (this.PossuiIdentificadorCadastradoPor(contexto, planoTrabalho.Identificador))
                        {
                            mensagens.Add("Este IDENTIFICADOR já foi utilizado.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private int ObtemIdentificadorSeqPor(DataContext contexto, string ptres)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 1; //Caso nao exista nenhuma outro a resposta será 1
            try
            {
                contextQuery.Command = @" SELECT MAX(IDENTIFICADORSEQ) + 1 AS IDENTIFICADORSEQ
                                            FROM [LYCEUM].[PrestacaoContas].[PLANOTRABALHO] p (NOLOCK)
												INNER JOIN PRESTACAOCONTAS.PROGRAMATRABALHO PT (NOLOCK) ON P.PROGRAMATRABALHOID = PT.PROGRAMATRABALHOID
												INNER JOIN PRESTACAOCONTAS.WSPROGRAMASEFAZ PTS (NOLOCK) ON PT.WSPROGRAMASEFAZID = PTS.WSPROGRAMASEFAZID
                                            WHERE PTRES = @PTRES
                                            GROUP BY PTRES ";

                contextQuery.Parameters.Add("@PTRES", SqlDbType.VarChar, ptres);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["IDENTIFICADORSEQ"]);
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

        public void Insere(Entidades.PlanoTrabalho planoTrabalho)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(contexto, planoTrabalho);
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

        private void Insere(DataContext contexto, Entidades.PlanoTrabalho planoTrabalho)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO prestacaocontas.PLANOTRABALHO 
                                            (FINALIDADEID,
                                                   SUPERINTENDENCIAID,
                                                   TIPODESPESAID,
                                                   TIPOCONTRATACAOID,
                                                   PROGRAMATRABALHOID,
                                                   DESCRICAO,
                                                   IDENTIFICADOR,
                                                   IDENTIFICADORSEQ,
                                                   PERIODICIDADE,
                                                   PEQUENADESPESA,
                                                   USUARIOID,
                                                   DATACADASTRO,
                                                   DATAALTERACAO
                                                   
) 
                                VALUES      (@FINALIDADEID,
                                                 @SUPERINTENDENCIAID,
                                                  @TIPODESPESAID,
                                                   @TIPOCONTRATACAOID,
                                                   @PROGRAMATRABALHOID,
                                                   @DESCRICAO,
                                                   @IDENTIFICADOR,
                                                   @IDENTIFICADORSEQ,
                                                   @PERIODICIDADE,
                                                   @PEQUENADESPESA,
                                                   @USUARIOID,
                                                   @DATACADASTRO,
                                                   @DATAALTERACAO
                                                )
                                			
                                SELECT IDENT_CURRENT('prestacaocontas.PLANOTRABALHO') ";

            contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, planoTrabalho.FinalidadeId);
            contextQuery.Parameters.Add("@SUPERINTENDENCIAID", SqlDbType.Int, planoTrabalho.SuperintendenciaId);
            contextQuery.Parameters.Add("@TIPODESPESAID", SqlDbType.Int, planoTrabalho.TipoDespesaId);
            contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, planoTrabalho.TipoContratacaoId);
            contextQuery.Parameters.Add("@PROGRAMATRABALHOID", SqlDbType.Int, planoTrabalho.ProgramaTrabalhoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, planoTrabalho.Descricao);
            contextQuery.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar, planoTrabalho.Identificador);
            contextQuery.Parameters.Add("@IDENTIFICADORSEQ", SqlDbType.VarChar, planoTrabalho.IdentificadorSeq);
            contextQuery.Parameters.Add("@PERIODICIDADE", SqlDbType.VarChar, planoTrabalho.Periodicidade);
            contextQuery.Parameters.Add("@PEQUENADESPESA", SqlDbType.Bit, planoTrabalho.PequenaDespesa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, planoTrabalho.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);


            planoTrabalho.PlanoTrabalhoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.PlanoTrabalho planoTrabalho)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" UPDATE prestacaocontas.PLANOTRABALHO                                             
                                               SET  FINALIDADEID = @FINALIDADEID,           
                                               SUPERINTENDENCIAID = @SUPERINTENDENCIAID,                                                
                                               TIPODESPESAID = @TIPODESPESAID,
                                               TIPOCONTRATACAOID = @TIPOCONTRATACAOID, 
                                               PROGRAMATRABALHOID = @PROGRAMATRABALHOID,
                                               DESCRICAO = @DESCRICAO,    
                                               PERIODICIDADE = @PERIODICIDADE,  
                                               PEQUENADESPESA = @PEQUENADESPESA,  
                                               USUARIOID = @USUARIOID,
                                               DATAALTERACAO = @DATAALTERACAO
                                        WHERE  PLANOTRABALHOID = @PLANOTRABALHOID
                                			
                                 ";

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalho.PlanoTrabalhoId);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, planoTrabalho.FinalidadeId);
                contextQuery.Parameters.Add("@SUPERINTENDENCIAID", SqlDbType.Int, planoTrabalho.SuperintendenciaId);
                contextQuery.Parameters.Add("@TIPODESPESAID", SqlDbType.Int, planoTrabalho.TipoDespesaId);
                contextQuery.Parameters.Add("@TIPOCONTRATACAOID", SqlDbType.Int, planoTrabalho.TipoContratacaoId);
                contextQuery.Parameters.Add("@PROGRAMATRABALHOID", SqlDbType.Int, planoTrabalho.ProgramaTrabalhoId);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, planoTrabalho.Descricao);
                contextQuery.Parameters.Add("@PERIODICIDADE", SqlDbType.VarChar, planoTrabalho.Periodicidade);
                contextQuery.Parameters.Add("@PEQUENADESPESA", SqlDbType.Bit, planoTrabalho.PequenaDespesa);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, planoTrabalho.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public bool PossuiPlanilhaOrcamentaria(DataContext contexto, int planotrabalhoid)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PRESTACAOCONTAS.PLANILHAORCAMENTARIA (NOLOCK)
                                    WHERE PLANOTRABALHOID = @PLANOTRABALHOID ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planotrabalhoid);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaRemocao(int planoTrabalhoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            SaldoInicial rnSaldoInicial = new SaldoInicial();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (planoTrabalhoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiPlanilhaOrcamentaria(contexto, planoTrabalhoId))
                    {
                        mensagens.Add("Não é possível excluir um Projeto / Programa que esteja vinculado a uma Programação Orçamentária.");
                    }

                    if (rnSaldoInicial.PossuiPlanoTrabalhoPor(contexto, planoTrabalhoId))
                    {
                        mensagens.Add("Não é possível excluir um Projeto / Programa que possuia saldo inicial.");
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

        public void Remove(int planoTrabalhoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Remove Plano
                this.Remove(contexto, planoTrabalhoId);
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

        private void Remove(DataContext contexto, int planoTrabalhooId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" delete from prestacaocontas.PLANOTRABALHO
                                        where planotrabalhoid = @planotrabalhoid ";

            contextQuery.Parameters.Add("@planotrabalhoid", SqlDbType.Int, planoTrabalhooId);

            contexto.ApplyModifications(contextQuery);
        }

        //        public int ObtemFinalidadeTotalPor(DataContext contexto, int censo)
        //        {
        //            ContextQuery contextQuery = new ContextQuery();
        //            SqlDataReader reader = null;
        //            int retorno = 0;
        //            try
        //            {
        //                contextQuery.Command = @" SELECT P.IDENTIFICADOR, PTS.PT, P.DESCRICAO, F.DESCRICAO AS FINALIDADE
        //                                          FROM PrestacaoContas.PLANOTRABALHO P
        //	                                      JOIN PrestacaoContas.FINALIDADE F
        //		                                      ON P.FINALIDADEID = F.FINALIDADEID
        //	                                      JOIN PrestacaoContas.PROGRAMATRABALHO PT
        //		                                      ON PT.PROGRAMATRABALHOID = p.PROGRAMATRABALHOID
        //	                                      JOIN PrestacaoContas.WSPROGRAMASEFAZ PTS
        //		                                      ON PTS.WSPROGRAMASEFAZID = PT.WSPROGRAMASEFAZID
        //	                                      JOIN PrestacaoContas.PLANILHAORCAMENTARIA PO
        //		                                      ON PO.PLANOTRABALHOID = P.PLANOTRABALHOID
        //	                                      JOIN PrestacaoContas.ITEMPLANILHAORCAMENTARIA IPO
        //		                                      ON IPO.PLANILHAORCAMENTARIAID = PO.PLANILHAORCAMENTARIAID
        //	                                      JOIN PrestacaoContas.LANCAMENTOREPASSE R
        //		                                        ON R.ITEMPLANILHAORCAMENTARIAID = IPO.ITEMPLANILHAORCAMENTARIAID
        //                                          WHERE R.CENSO = @CENSO ";

        //                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);

        //                reader = contexto.GetDataReader(contextQuery);

        //                while (reader.Read())
        //                {
        //                    retorno = Convert.ToInt32(reader["FINALIDADE"]);
        //                }
        //            }
        //            finally
        //            {
        //                if (reader != null)
        //                {
        //                    reader.Close();
        //                }
        //            }

        //            return retorno;
        //        }

        public DataTable ListaDados(object censo, object ano, object planoTrabalho)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  select query.TOTALSOLICITADO, query.TOTALENTRADAS, query.TOTALCREDITOS
                                           , (query.TOTALSOLICITADO-(query.TOTALENTRADAS - query.TOTALCREDITOS)) as SALDO , query.FINALIDADE --Saldo (A-(B-C)) 
                                           from (
                                            select ( SELECT ISNULL(SUM(R.VALORPAGO),0) AS TOTAL_SOLICITADO
                                            FROM PrestacaoContas.PLANOTRABALHO P	                                           
	                                            JOIN PrestacaoContas.PLANILHAORCAMENTARIA PO
		                                            ON PO.PLANOTRABALHOID = P.PLANOTRABALHOID
	                                            JOIN PrestacaoContas.ITEMPLANILHAORCAMENTARIA IPO
		                                            ON IPO.PLANILHAORCAMENTARIAID = PO.PLANILHAORCAMENTARIAID
	                                            JOIN PrestacaoContas.LANCAMENTOREPASSE R
		                                            ON R.ITEMPLANILHAORCAMENTARIAID = IPO.ITEMPLANILHAORCAMENTARIAID
	                                             JOIN PrestacaoContas.ANALISEREPASSE AR
		                                            ON AR.LANCAMENTOREPASSEID = R.LANCAMENTOREPASSEID
                                            WHERE R.CENSO = @CENSO -- Unidade de Ensino Selecionada
                                                AND PO.ANO = @ANO -- Ano Informado na Tela
                                                AND P.PLANOTRABALHOID = @PLANOTRABALHO -- Plano Selecionado
                                                AND AR.APROVADO = 1 -- Listar somente repasses que foram aprovados
                                                AND R.WSREPASSESEFAZID IS NOT NULL -- Listar repasses que foram integrados
                                                AND R.STATUSLISTA = 'F' -- Listar somente repasses que já foram faturados (Pagos)
                                            ) as TOTALSOLICITADO
                                            , 
                                            ( SELECT  ISNULL(SUM(E.VALORPAGAMENTO),0) AS TOTAL_ENTRADAS
                                            FROM PrestacaoContas.EVENTO E      
                                            WHERE E.CENSO = @CENSO -- Unidade de Ensino Selecionada
                                                AND DATEPART(YYYY, E.DATAPAGAMENTO) = @ANO -- Ano Informado na Tela
                                                AND E.PLANOTRABALHOID = @PLANOTRABALHO -- Plano Selecionado 
                                                AND E.APROVADO = 1
                                            ) as TOTALENTRADAS
                                            ,
                                            ( SELECT ISNULL(SUM(EC.VALOR),0) AS TOTAL_SAIDAS
                                                FROM PrestacaoContas.EVENTOCREDITO EC 
                                                WHERE EC.CENSO = @CENSO
	                                                AND DATEPART(YYYY, EC.DATAEVENTO) = @ANO 
	                                                AND EC.PLANOTRABALHOID = @PLANOTRABALHO
                                            ) AS TOTALCREDITOS
                                            , 
											(
											select f.DESCRICAO from PrestacaoContas.PLANOTRABALHO pt
											join PrestacaoContas.FINALIDADE f 
											on f.FINALIDADEID = pt.FINALIDADEID
											where pt.PLANOTRABALHOID = @PLANOTRABALHO
											) as FINALIDADE
                                            ) query
                                                                                        ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, !String.IsNullOrEmpty(censo.ToString()) ? censo : null);
                contextQuery.Parameters.Add("@ANO", SqlDbType.VarChar, ano != null ? ano : null);
                contextQuery.Parameters.Add("@PLANOTRABALHO", SqlDbType.Int, !String.IsNullOrEmpty(planoTrabalho.ToString()) ? planoTrabalho : null);

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
    }
}
