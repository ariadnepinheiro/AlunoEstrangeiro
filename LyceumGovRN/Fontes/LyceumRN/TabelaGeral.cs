using Techne.Data;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using System.Linq;

namespace Techne.Lyceum.RN
{
    public class TabelaGeral
    {
        [
        MethodDescription("Valida os dados de tabela geral para inserção e atualização"),
        ToolTip("Valida se a tabela existe"),
        ControlText("Validação"),
        ]

        public static bool ExisteTabela(string tabela)
        {
            if (!string.IsNullOrEmpty(tabela))
            {
                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();

                try
                {

                    string sql = " select 1 from HD_TABELA where TABELA = ? ";

                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, tabela);

                    if (!valorConsulta.IsNull)
                        return true;
                }
                finally
                {
                    connection.Close();
                }
            }

            return false;
        }

        public static bool ExisteItemTabela(string tabela)
        {
            if (!string.IsNullOrEmpty(tabela))
            {
                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();

                try
                {
                    string sql = " select 1 from Hd_tabelaitem where TABELA = ? ";

                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, tabela);

                    if (!valorConsulta.IsNull)
                        return true;
                }
                finally
                {
                    connection.Close();
                }
            }

            return false;
        }

        public static DataTable ConsultaItemTabelaValDescr(string tab)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"Select item, descr  from itemtabela where tab = @tab order by descr"
                };
                contextQuery.Parameters.Add("@tab", tab);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ConsultaItemTabelaValDescrFiltro(string tab,string filtro, string exceto, string excluso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"Select item, descr  
                                from itemtabela 
                                where tab = @tab and 
                                      descr like '%' + @filtro + '%' and
                                      ( (descr not like '%' + @exceto + '%') or (@exceto='-1') ) and
                                      item <> @excluso
                                order by descr"
                };
                contextQuery.Parameters.Add("@tab", tab);
                contextQuery.Parameters.Add("@filtro", filtro);
                contextQuery.Parameters.Add("@exceto", exceto );
                contextQuery.Parameters.Add("@excluso", excluso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable SelecionarItensAbaCompartilhadaComboRedeEnsino(string tabela, string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            var Complemento = string.Empty;
            DataTable dtListarComponentesDoPerfil = null;

            try
            {
                dtListarComponentesDoPerfil = Perfil.ListarPerfis(usuario, "UNIDADE DE ENSINO");

                if (dtListarComponentesDoPerfil.Rows.Count > 0)
                {
                    Complemento += "ITEM = '' ";

                    if (dtListarComponentesDoPerfil.Select("PERFIL = 'SUPLAN'").Any())
                    {
                        Complemento += "OR ITEM = 'Municipal' ";
                    }
                    else
                    {
                        Complemento += "OR ITEM <> 'Municipal' ";
                    }
                }

                contextQuery.Command = @"SELECT ITEM, DESCR FROM dbo.ITEMTABELA WHERE TAB = @TAB " +
                    (Complemento.Length > 0 ? "AND ( " + Complemento + " ) " : Complemento) +
                    " ORDER BY DESCR ";

                contextQuery.Parameters.Add("@TAB", tabela);

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


        public DataTable SelecionarItensLocalFuncionamento(string tabela)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            try
            {
                contextQuery.Command = @"SELECT ITEM, DESCR FROM dbo.ITEMTABELA 
                                        WHERE TAB = @TAB 
                                        AND ITEM NOT IN ('TemploIgreja','Unidade de Internação/Prisional','Casa do Professor','Salas de Empresa')
                                        ORDER BY DESCR ";

                contextQuery.Parameters.Add("@TAB", tabela);

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



        public DataTable Lista(string tabela)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            try
            {
                contextQuery.Command = @"SELECT '' AS ITEM, ' Selecione' as DESCR
                                        UNION ALL
                                        SELECT ITEM, DESCR FROM dbo.ITEMTABELA 
                                        WHERE TAB = @TAB 
                                        AND ITEM <> 'Selecione'
                                        ORDER BY DESCR ";

                contextQuery.Parameters.Add("@TAB", tabela);

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
