using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class PainelFinanceiro
    {
        public DTOs.DadosReceitasRecebidas ObtemReceitasPor(string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId, int periodoReferenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemReceitasPor(contexto, censo, dataInicio, dataFim, finalidadeId, periodoReferenciaId);
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

        public DTOs.DadosReceitasRecebidas ObtemReceitasPor(string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId, int planoTrabalhoId, int periodoReferenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemReceitasPor(contexto, censo, dataInicio, dataFim, finalidadeId, null, planoTrabalhoId, periodoReferenciaId);
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
        public decimal ObtemCreditoDebitoPor(string censo, int periodoReferencia, int planoTrabalhoId, string tipo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemCreditoDebitoPor(contexto, censo, periodoReferencia, planoTrabalhoId, tipo);
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

        public List<DTOs.DadosReceitasRecebidas> ObtemReceitasProgramaPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            List<DTOs.DadosReceitasRecebidas> lista = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas>();
            
            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.RECEITASRECEBIDASPROGRAMA";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    PrestacaoContas.DTOs.DadosReceitasRecebidas dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
                    dados.SaldoAnterior = Convert.ToDecimal(reader["SALDOANTERIOR"]);
                    dados.Repasses = Convert.ToDecimal(reader["REPASESRECEBIDOS"]);
                    dados.Despesas = Convert.ToDecimal(reader["TOTALDESPESAS"]);
                    dados.Rendimentos = Convert.ToDecimal(reader["RENDIMENTOS"]);
                    dados.Devolucoes = Convert.ToDecimal(reader["DEVOLUCOES"]);
                    dados.ProgramaTrabalhoId = Convert.ToInt32(reader["PROGRAMATRABALHOID"]);
                    dados.Programa = Convert.ToString(reader["PROGRAMATRABALHO"]);
                    dados.Pt = Convert.ToString(reader["PT"]);
                    dados.PtRes = Convert.ToString(reader["PTRES"]);
                    lista.Add(dados);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DTOs.DadosReceitasProjetoRecebidas> ObtemReceitasProjetoPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            var lista = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasProjetoRecebidas>();

            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.RECEITASRECEBIDASPROJETO";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    var dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasProjetoRecebidas();
                    dados.SaldoAnterior = reader["SALDOANTERIOR"] == DBNull.Value? 0 : Convert.ToDecimal(reader["SALDOANTERIOR"]);
                    dados.Repasses = reader["REPASESRECEBIDOS"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["REPASESRECEBIDOS"]);
                    dados.Despesas = reader["TOTALDESPESAS"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TOTALDESPESAS"]);
                    dados.Rendimentos = reader["RENDIMENTOS"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["RENDIMENTOS"]);
                    dados.Devolucoes = reader["DEVOLUCOES"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DEVOLUCOES"]);
                    dados.ProgramaTrabalhoId = reader["PROGRAMATRABALHOID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PROGRAMATRABALHOID"]);
                    dados.Programa = Convert.ToString(reader["PROGRAMATRABALHO"]);
                    dados.PlanoTrabalhoId = reader["PLANOTRABALHOID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PLANOTRABALHOID"]);
                    dados.PlanoTrabalho = Convert.ToString(reader["PLANOTRABALHO"]);
                    dados.Pt = Convert.ToString(reader["PT"]);
                    dados.PtRes = Convert.ToString(reader["PTRES"]);
                    dados.CreditosDebitos = reader["CREDITOSDEBITOS"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CREDITOSDEBITOS"]);
                    lista.Add(dados);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DTOs.DadosReceitasRecebidas ObtemReceitasPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            PrestacaoContas.DTOs.DadosReceitasRecebidas dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.RECEITASRECEBIDAS";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);
                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.SaldoAnterior = Convert.ToDecimal(reader["SALDOANTERIOR"]);
                    dados.Repasses = Convert.ToDecimal(reader["REPASESRECEBIDOS"]);
                    dados.Despesas = Convert.ToDecimal(reader["TOTALDESPESAS"]);
                    dados.Rendimentos = Convert.ToDecimal(reader["RENDIMENTOS"]);
                    dados.Devolucoes = Convert.ToDecimal(reader["DEVOLUCOES"]);
                    dados.CreditosDebitos = Convert.ToDecimal(reader["CREDITOSDEBITOS"]);
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DTOs.DadosReceitasRecebidas ObtemReceitasPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId, string pt, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            PrestacaoContas.DTOs.DadosReceitasRecebidas dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.RECEITASRECEBIDAS";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);
                contextQuery.Parameters.Add("@PT", SqlDbType.VarChar, pt);
                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.SaldoAnterior = Convert.ToDecimal(reader["SALDOANTERIOR"]);
                    dados.Repasses = Convert.ToDecimal(reader["REPASESRECEBIDOS"]);
                    dados.Despesas = Convert.ToDecimal(reader["TOTALDESPESAS"]);
                    dados.Rendimentos = Convert.ToDecimal(reader["RENDIMENTOS"]);
                    dados.Devolucoes = Convert.ToDecimal(reader["DEVOLUCOES"]);
                    dados.CreditosDebitos = Convert.ToDecimal(reader["CREDITOSDEBITOS"]);
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DTOs.DadosReceitasRecebidas ObtemReceitasPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId, string pt, int planoTrabalhoId, int periodoReferenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            PrestacaoContas.DTOs.DadosReceitasRecebidas dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.RECEITASRECEBIDASTOTAL";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);
                contextQuery.Parameters.Add("@PT", SqlDbType.VarChar, pt);
                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, planoTrabalhoId);
                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.VarChar, periodoReferenciaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.SaldoAnterior = Convert.ToDecimal(reader["SALDOANTERIOR"]);
                    dados.Repasses = Convert.ToDecimal(reader["REPASESRECEBIDOS"]);
                    dados.Despesas = Convert.ToDecimal(reader["TOTALDESPESAS"]);
                    dados.Rendimentos = Convert.ToDecimal(reader["RENDIMENTOS"]);
                    dados.Devolucoes = Convert.ToDecimal(reader["DEVOLUCOES"]);
                    dados.CreditosDebitos = Convert.ToDecimal(reader["CREDITOSDEBITOS"]);
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public decimal ObtemCreditoDebitoPor(DataContext contexto, string censo, int periodoReferencia, int planoTrabalhoId, string tipo)
        {
            ContextQuery contextQuery = new ContextQuery();
            PrestacaoContas.DTOs.DadosReceitasRecebidas dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            SqlDataReader reader = null;
            decimal retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT sum(VALOR) as valor
                                          FROM PrestacaoContas.OPERACAO
                                          where censo = @CENSO
                                          and PERIODOREFERENCIAID = @PERIODOREFERENCIAID
                                          and PLANOTRABALHOID = @PLANOTRABALHOID
                                          and TIPOOPERACAO = @TIPO
                                          and STATUS = 3 ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoReferencia);
                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, planoTrabalhoId);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno =  reader["valor"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["valor"]) ;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public decimal ObtemValorSaldoAnterior(String censo, DateTime data, int finalidade)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;                
                contextQuery.Command = @"[PrestacaoContas].[SALDOANTERIOR]";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["VALOR"]);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (contexto != null)
                    contexto.Dispose();
            }

            return retorno;
        }

        public DataTable RetornaDadosMerenda(String censo, DateTime data, DateTime dataFim, int referenciaid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            int finalidade = 2;//Merenda

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.PAINELFINANCEIRO";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);
                contextQuery.Parameters.Add("@REFERENCIAID", referenciaid);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable RetornaDadosMerendaTotal(String censo, DateTime data, DateTime dataFim, int referenciaid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            int finalidade = 2;//Merenda

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.PAINELFINANCEIROTOTAL";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);
                contextQuery.Parameters.Add("@REFERENCIAID", referenciaid);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable RetornaDadosManutencao(String censo, DateTime data, DateTime dataFim, int referenciaid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            int finalidade = 1;//Manutenção

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.PAINELFINANCEIRO";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);
                contextQuery.Parameters.Add("@REFERENCIAID", referenciaid);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable RetornaDadosManutencaoTotal(String censo, DateTime data, DateTime dataFim, int referenciaid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            int finalidade = 1;//Manutenção

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.PAINELFINANCEIROTOTAL";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);
                contextQuery.Parameters.Add("@REFERENCIAID", referenciaid);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable RetornaDadosOutrosProjetos(String censo, DateTime data, DateTime dataFim, int referenciaid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            int finalidade = 3; //Outros

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.PAINELFINANCEIRO";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);
                contextQuery.Parameters.Add("@REFERENCIAID", referenciaid);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public DataTable RetornaDadosOutrosProjetosTotal(String censo, DateTime data, DateTime dataFim, int referenciaid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            int finalidade = 3; //Outros

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.PAINELFINANCEIROTOTAL";
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATAINICIO", data);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", finalidade);
                contextQuery.Parameters.Add("@REFERENCIAID", referenciaid);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public ValidacaoDados Valida(Entidades.PainelFinanceiro painelFinanceiro)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (painelFinanceiro == null)
            {
                return validacaoDados;
            }            

            if (painelFinanceiro.Censo == null)
            {
                mensagens.Add("Campo obrigatório Unidade Ensino não foi preenchido");
            }         

            if (painelFinanceiro.PeriodoReferencia == null)
            {
                mensagens.Add("Campo obrigatório Periodo Referência não foi preenchido");
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
    }
}
