using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;
using System.Text;
using System;

namespace Techne.Lyceum.RN
{
    public class Basico : RNBase
    {
        public const string QueryListaPaises = "SELECT UPPER(CODIGO) CODIGO, UPPER(NOME) NOME FROM PAISES ORDER BY NOME ";
        public const string QueryListaEtniaAtiva = "SELECT	ETNIAID, NOME, TABELAITEMID FROM HADES.DBO.ETNIA E ( NOLOCK ) WHERE E.ATIVO = 1 ";
        public const string QueryListaNacionalidades = "SELECT UPPER(NACIONALIDADE) NACIONALIDADE, UPPER(NOME) NOME FROM NACIONALIDADE ORDER BY NOME ";
        public const string QueryListaUF = "SELECT SIGLA, NOME FROM UF WHERE SIGLA <> '00' ORDER BY SIGLA";
        public const string QueryListaUFCartorio = "SELECT DISTINCT CODIGO_UF, UF FROM CARTORIO ORDER BY UF";
        public const string QueryListaMunicipioCartorio = "SELECT DISTINCT codigo_municipio,municipio FROM CARTORIO WHERE CODIGO_UF <> '00' and CODIGO_UF = ?";
        public const string QueryListaTipoIngresso = "Select tipo_ingresso, descricao From ly_tipo_ingresso ";
        public const string QueryListaUFNaturalidade = "SELECT distinct UF_SIGLA FROM    MUNICIPIO WHERE UF_SIGLA <> '00' order by UF_SIGLA";
        public const string QueryListaTipoUA = "Select tiposetorid, descricao From HADES.GestaoRede.TIPOSETOR";


        /// <summary>
        /// Consulta municípios e siglas.
        /// </summary>
        /// <returns>querytable com códigos de municípios, descrição e siglas</returns>
        public static QueryTable ConsultarMunicipio()
        {
            string sql = "SELECT CODIGO, (NOME + ' - ' + UF_SIGLA) AS NOME FROM MUNICIPIO ORDER BY NOME";
            return RNBase.Consultar(sql);
        }

        /// <summary>
        /// Consulta código de países e descrições.
        /// </summary>
        /// <returns>código de países e descrições</returns>
        public static QueryTable ConsultarPais()
        {
            string sql = "SELECT UPPER(CODIGO) CODIGO, UPPER(NOME) NOME FROM PAISES ORDER BY NOME";
            return RNBase.Consultar(sql);
        }

        public static string ObterCodigoUFCartorio(string SIGLA)
        {
            string sql = " SELECT TOP 1 CODIGO_UF FROM CARTORIO WHERE UF = ? ";

            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = RNBase.ExecutarFuncaoScalar(sql, SIGLA);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                connection.Close();
            }

            return string.Empty;
        }

        /// <summary>
        /// Consulta nacionalidades.
        /// </summary>
        /// <returns>nacionalidades</returns>
        public static QueryTable ConsultarNacionalidade()
        {
            string sql = "SELECT UPPER(NACIONALIDADE) NACIONALIDADE, UPPER(NOME) NOME FROM NACIONALIDADE ORDER BY NOME";
            return RNBase.Consultar(sql);
        }

        /// <summary>
        /// Consulta siglas de estados.
        /// </summary>
        /// <returns>siglas de estados</returns>
        public static QueryTable ConsultarUF()
        {
            string sql = "SELECT SIGLA, NOME FROM UF WHERE SIGLA <> '00' ORDER BY SIGLA";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ConsultarMunicipioCartorio(string COD_UF)
        {
            string sql = "SELECT DISTINCT codigo_municipio,municipio FROM CARTORIO WHERE CODIGO_UF <> '00' and CODIGO_UF = ?";
            return RNBase.Consultar(sql, COD_UF);
        }

        public static QueryTable ConsultarCartorio(string COD_UF, string CODIGO_MUNICIPIO)
        {
            string sql = "SELECT DISTINCT cod_cartorio,nome_cartorio FROM CARTORIO WHERE CODIGO_UF = ? AND CODIGO_MUNICIPIO = ?";
            return RNBase.Consultar(sql, COD_UF, CODIGO_MUNICIPIO);
        }

        public DataTable ObtemListaCartorioPor(string uf, string municipio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable cartorios = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                cod_cartorio ,
                                nome_cartorio
                        FROM    CARTORIO
                        WHERE   CODIGO_UF = @CODIGO_UF
                                AND CODIGO_MUNICIPIO = @CODIGO_MUNICIPIO ";

                contextQuery.Parameters.Add("@CODIGO_UF", uf);
                contextQuery.Parameters.Add("@CODIGO_MUNICIPIO", municipio);

                cartorios = ctx.GetDataTable(contextQuery);
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

            return cartorios;
        }

        public DataTable ObtemListaMunicipioCartorioPor(string uf)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable cartorios = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            codigo_municipio ,
                            municipio
                    FROM    CARTORIO
                    WHERE   CODIGO_UF <> '00'
                            AND CODIGO_UF = @CODIGO_UF
                    ORDER BY municipio ";

                contextQuery.Parameters.Add("@CODIGO_UF", uf);

                cartorios = ctx.GetDataTable(contextQuery);
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

            return cartorios;
        }

        /// <summary>
        /// Consulta sistemas.
        /// </summary>
        /// <returns>querytable de sistemas</returns>
        public static QueryTable ConsultarSistema()
        {
            string sql = "SELECT SIS FROM HD_SISTEMA";
            return RNBase.ConsultarHades(sql);
        }

        /// <summary>
        /// Consulta nome, versão, habilitado do sistema.
        /// </summary>
        /// <param name="sistema">sistema</param>
        /// <returns>querytable de dados do sistema</returns>
        public static QueryTable ConsultardDadosSistema(string sistema)
        {
            string sql = "SELECT NOME, VERSAO, HABILITADO FROM HD_SISTEMA WHERE SIS=?";
            return RNBase.ConsultarHades(sql, sistema);
        }

        /// <summary>
        /// Consulta itens da tabela geral.
        /// </summary>
        /// <param name="tab">tabela</param>
        /// <returns>querytable de itens da tabela</returns>
        public static QueryTable ConsultaItemTabela(string tab)
        {
            string sql = "Select item as tipo, descr as descricao from itemtabela where tab = ?";
            return RNBase.Consultar(sql, tab);
        }

        /// <summary>
        /// Consulta itens e descrições de uma tabela geral.
        /// </summary>
        /// <param name="tab">tabela</param>
        /// <returns>querytable de itens da tabela</returns>
        public static QueryTable ConsultaItemTabelaValDescr(string tab)
        {
            string sql = "Select item, descr  from itemtabela where tab = ? order by descr";
            return RNBase.Consultar(sql, tab);
        }

        public static DataTable ObtemListaTabelaGeralPor(string tab)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT  item ,
                                    descr
                            FROM    ITEMTABELA
                            WHERE   TAB = @TAB
                            ORDER BY DESCR ";

                contextQuery.Parameters.Add("@TAB", tab);

                retorno = ctx.GetDataTable(contextQuery);
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

            return retorno;
        }

        public static DataTable ObtemResultadoQueryPor(string query)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = query;
                retorno = ctx.GetDataTable(contextQuery);
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

            return retorno;
        }

        /// <summary>
        /// Consulta itens e descrições de uma tabela geral, ordenando numéricamente.
        /// </summary>
        /// <param name="tab">tabela</param>
        /// <returns>querytable de itens da tabela</returns>
        public static QueryTable ConsultaItemTabelaValDescrNumerico(string tab)
        {
            string sql = "Select item, descr  from itemtabela where tab = ? order by convert(decimal,descr)";
            return RNBase.Consultar(sql, tab);
        }

        /// <summary>
        /// Gera código de função a partir do último do banco.
        /// </summary>
        /// <returns>código de função decimal</returns>
        public static decimal GeraFuncao()
        {
            decimal funcao = RNBase.ExecutarFuncaoDec("SELECT MAX(CONVERT(DEC, FUNCAO)) FROM LY_FUNCAO WHERE FUNCAO BETWEEN '0' and '99999999999999999999'");
            return funcao + 1;
        }

        public static DataTable Listar()
        {
            var contextQuery = new ContextQuery(
                @" SELECT  [ID_MACRO_CAMPOS] ,[NOME],[OBRIGATORIO] ,[MATRICULA] ,[DT_CADASTRO],[DT_ALTERACAO]
                FROM    dbo.TCE_MACRO_CAMPOS
                ");


            return Consultar(contextQuery);
        }

        public static QueryTable ConsultaUnidadeEnsinoMaisEducacao()
        {
            var sql = new StringBuilder("SELECT DISTINCT ");
            sql.Append("e.NOME_COMP, ");
            sql.Append("e.UNIDADE_ENS ");
            sql.Append("FROM dbo.LY_UNIDADE_ENSINO_CURSOS c ");
            sql.Append("INNER JOIN dbo.LY_UNIDADE_ENSINO e ON c.UNIDADE_ENS = e.UNIDADE_ENS ");
            sql.Append("WHERE c.CURSO = '9999.92' ORDER BY e.NOME_COMP");

            return RNBase.Consultar(sql.ToString());
        }
    }
}
