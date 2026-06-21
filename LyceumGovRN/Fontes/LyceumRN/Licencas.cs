using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class Licencas : RNBase
    {
        public static QueryTable ConsultarLicencas()
        {
            QueryTable qt = new QueryTable(@"
                SELECT 
                    motivo, descricao, possui_dtfim, periodo_limite, bloqueia_glp,participacontratotemporario,validaalocacao
                FROM 
                    ly_licencas 
                WHERE 
                    motivo <> ?
                ORDER BY
                    descricao");

            TConnection connection = Config.CreateConnection();
            connection.Open();
            qt.Query(connection, "43");
            return qt;
        }
        public static QueryTable ListarLicencas()
        {
            return Consultar("select motivo, descricao from ly_licencas order by descricao");
        }

        public static QueryTable PreencherComboLicenca(string usuario)
        {
            QueryTable qt = null;

            qt = Consultar(@"select (li.motivo + '|' + possui_dtfim) motivo, descricao 
                                    from LY_LICENCAS li 
                                    union 
                                    select '' as motivo, ' Nenhum ' as descricao 
                                    ORDER BY descricao");
            return qt;
        }

        [MethodDescription("Utilizado pelo ODS")]
        public static void DeleteMethod(string motivo) { }
        public static RetValue RemoverLicenca(string motivo)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_licencas.Row.Delete(connection, motivo);
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        [MethodDescription("Utilizado pelo ODS")]
        public static void InsertMethod(String motivo, String descricao, String possui_dtfim, decimal? periodo_limite, String bloqueia_glp, string participacontratotemporario, string validaalocacao) { }
        public static RetValue InserirLicenca(String motivo, String descricao, String possui_dtfim, String periodo_limite, String bloqueia_glp, string participacontratotemporario, string validaalocacao)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_licencas licencas = new Ly_licencas();
                Ly_licencas.Row licenca = licencas.NewRow();
                licenca.Motivo = motivo;
                licenca.Descricao = descricao;
                licenca.Possui_dtfim = possui_dtfim;
                licenca.Periodo_limite = periodo_limite.ToDecimal();
                licenca.Bloqueia_glp = bloqueia_glp;
                licenca.Participacontratotemporario = participacontratotemporario;
                licenca.Validaalocacao = validaalocacao;

                ColunasTable colunas = MontarParametros(licencas.Columns, licenca);
                Ly_licencas.Row.Insert(connection, motivo, descricao, colunas.Colunas, colunas.ValorColuna);
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        [MethodDescription("Utilizado pelo ODS")]
        public static void UpdateMethod(String motivo, String descricao, String possui_dtfim, String periodo_limite, String bloqueia_glp, string participacontratotemporario, string validaalocacao) { }
        public static RetValue AlterarLicenca(String oldMotivo, String motivo, String descricao, String possui_dtfim, String periodo_limite, String bloqueia_glp, string participacontratotemporario, string validaalocacao)
        {
            //oldMotivo, motivo, descricao, possui_dtfim, periodo_limite, bloqueia_glp
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_licencas licencas = new Ly_licencas();
                Ly_licencas.Row licenca = licencas.NewRow();
                licenca.Motivo = motivo;
                licenca.Descricao = descricao;
                licenca.Possui_dtfim = possui_dtfim;
                licenca.Periodo_limite = periodo_limite.ToDecimal();
                licenca.Bloqueia_glp = bloqueia_glp;
                licenca.Participacontratotemporario = participacontratotemporario;
                licenca.Validaalocacao = validaalocacao;

                ColunasTable colunas = MontarParametros(licencas.Columns, licenca);
                Ly_licencas.Row.Update(connection, oldMotivo, colunas.Colunas, colunas.ValorColuna);
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool PossuiDataFim(string motivo)
        {
            string sql = @"select 1 from LY_LICENCAS where MOTIVO = ? and POSSUI_DTFIM = 'S'";
            int qtd = ExecutarFuncao(sql, motivo);
            if (qtd == 1)
            {
                return true;
            }
            return false;
        }

        public bool PossuiValidacaoAlocacaoPor(string motivo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                    FROM    LY_LICENCAS
                    WHERE   MOTIVO = @MOTIVO
                            AND VALIDAALOCACAO ='S' ";

                contextQuery.Parameters.Add("@MOTIVO", motivo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public int ObtemPeriodoLimitePor(DataContext contexto, string situacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT ISNULL(PERIODO_LIMITE, 0) PERIODO_LIMITE 
                                        FROM   LY_LICENCAS (NOLOCK)
                                        WHERE  MOTIVO = @MOTIVO  ";

                contextQuery.Parameters.Add("@MOTIVO", situacao);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PERIODO_LIMITE"]);
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

        public bool PossuiDataFimPor(DataContext contexto, string motivo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_LICENCAS 
                                        WHERE MOTIVO = @MOTIVO 
                                        AND POSSUI_DTFIM = 'S' ";

            contextQuery.Parameters.Add("@MOTIVO", motivo);

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
                motivo AS LICENÇA,
                descricao as DESCRIÇÃO,
                possui_dtfim AS 'POSSUI DATA FINAL?',
                periodo_limite AS 'PERÍODO LIMITE (EM DIAS)',
                bloqueia_glp AS 'BLOQUEIA GLP?',
                participacontratotemporario AS 'PARTICIPA CONTRATO TEMPORÁRIO?',
                validaalocacao AS 'VALIDA ALOCAÇÃO?'
       
                FROM 
                    ly_licencas 
                WHERE 
                    motivo <> '43'
                ORDER BY
                    descricao
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
