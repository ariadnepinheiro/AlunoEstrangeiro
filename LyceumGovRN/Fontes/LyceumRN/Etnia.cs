using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class Etnia : RNBase
    {
        public QueryTable ConsultarEtnia()
        {
            string sql = "select etniaid, nome from HADES..ETNIA";
            return Consultar(sql);
        }

        public QueryTable ConsultarEtniaContratoTemporario()
        {
            string sql = "SELECT ETNIAID, NOME, TABELAITEMID FROM HADES.DBO.ETNIA E ( NOLOCK ) WHERE E.ATIVO = 1 AND E.TABELAITEMID <> 'NaoDeclarada' ";
            return Consultar(sql);
        }

        public DataTable ListaEtniaAtiva()
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable etnias = null;

            try
            {
                contextQuery.Command = @" SELECT	ETNIAID,
		                                        NOME, 
		                                        TABELAITEMID
                                        FROM	HADES.DBO.ETNIA E  ( NOLOCK )
                                        WHERE   E.ATIVO = 1 ";

                etnias = ctx.GetDataTable(contextQuery);
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

            return etnias;
        }

        public DataTable ListaEtniaAtivaContratoTemporario()
        {
            DataContext ctx = DataContextBuilder.FromHades.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable etniasdol = null;

            try
            {
                contextQuery.Command = @" SELECT	ETNIAID,
		                                        NOME, 
		                                        TABELAITEMID
                                        FROM	HADES.DBO.ETNIA E  ( NOLOCK )
                                        WHERE   E.ATIVO = 1 AND E.TABELAITEMID <> 'NaoDeclarada' ";

                etniasdol = ctx.GetDataTable(contextQuery);
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

            return etniasdol;
        }

        public int ObtemEtniaIdPor(string TabelaItemId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT	ETNIAID
                                            FROM	HADES.DBO.ETNIA E  ( NOLOCK )
                                            WHERE   TABELAITEMID = @TABELAITEMID  ";

                contextQuery.Parameters.Add("@TABELAITEMID", TabelaItemId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ETNIAID"]);
                }

                return retorno;
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
                ctx.Dispose();
            }
        }

        public string ObtemTabelaItemIPor(string EtniaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string retorno = null;

            try
            {
                contextQuery.Command = @" SELECT	TABELAITEMID
                                            FROM	HADES.DBO.ETNIA E  ( NOLOCK )
                                            WHERE   ETNIAID = @ETNIAID  ";

                contextQuery.Parameters.Add("@ETNIAID", EtniaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["TABELAITEMID"]);
                }

                return retorno;
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
                ctx.Dispose();
            }
        }
    }
}
