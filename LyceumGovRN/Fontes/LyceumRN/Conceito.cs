using System;
using System.Data;
using Seeduc.Infra.Data;
using Seeduc.Infra.Entities;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class Conceito : RNBase
    {
        /// <summary>
        /// Consulta conceitos de um grupo.
        /// </summary>
        /// <param name="codigoGrupo">código do grupo de conceito</param>
        /// <returns>lista de conceitos</returns>
        public static QueryTable ConsultarPorGrupo(string codigoGrupo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = "select conceito, descricao from LY_CONCEITO where GRUPO = ? order by nota desc";
            try
            {
                qt = new QueryTable(sql);
                qt.Query(connection, codigoGrupo);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static QueryTable ConsultarDisciplinaAdicional()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = "select estudoadicionalid,nome as NOME_DISCIPLINA_ADICIONAL from ESTUDOADICIONAL order by nome";
            try
            {
                qt = new QueryTable(sql);
                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        /// <summary>
        /// Consulta conceitos e descrições.
        /// </summary>
        /// <returns></returns>
        public static QueryTable Consultar()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = "select conceito, descricao from LY_CONCEITO";
            try
            {
                qt = new QueryTable(sql);
                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        /// <summary>
        /// Verifica se existe grupo de conceito.
        /// </summary>
        /// <param name="grupo">grupo de conceito</param>
        /// <returns>true se existe, false se não existe</returns>
        public static bool VerificarGrupo(string grupo)
        {
            if (!string.IsNullOrEmpty(grupo))
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();
                try
                {
                    string sql = " select 1 from Ly_grupo_conceito where GRUPO = ? ";
                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, grupo);
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

        /// <summary>
        /// Verifica se existe conceitos no grupo.
        /// </summary>
        /// <param name="grupo">grupo de conceitos</param>
        /// <returns>rue se existe, false se não existe</returns>
        public static bool VerificarConceito(string grupo)
        {
            if (!string.IsNullOrEmpty(grupo))
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();

                try
                {

                    string sql = " select 1 from Ly_conceito where GRUPO = ? ";

                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, grupo);

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

        /// <summary>
        /// Converte uma nota numérica para um conceito da disciplina ou,
        /// caso a disciplina não trabalhe com conceito, arredonda o valor numérico
        /// para o número de casas decimais que a disciplina trabalha.
        /// </summary>
        /// <param name="disciplina">Código da disciplina.</param>
        /// <param name="nota">Nota numérica.</param>
        /// <returns>Se retorno for nulo, esta disciplina não trabalha com Conceito (GRUPO_NOTA == NULL)</returns>
        public static String FormatarNota(String disciplina, String nota)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            if (String.IsNullOrEmpty(disciplina)) return null;
            if (String.IsNullOrEmpty(nota)) return null;

            try
            {
                RN.Disciplina.NotasDisciplina dadosNotas = Disciplina.ConsultarDisciplinaConceitos(disciplina);
                String grupoNota = dadosNotas.GrupoNota;
                int nCasasDecimais = dadosNotas.CasasDecimais;
                //decimal notaMax = decimal.Parse(dadosNotas.NotaMax.Replace(".", ","));
                decimal notaAux = decimal.Parse(nota.Replace(".", ","));

                //if (notaAux > notaMax)
                //    notaAux = notaMax;
                //else if (notaAux < 0)
                //    notaAux = 0M;

                if (String.IsNullOrEmpty(grupoNota))
                    return Math.Round(notaAux, nCasasDecimais).ToString().Replace(".", ",");

                decimal notaConceitoMaximo, notaConceitoMinimo;
                String conceitoMaximo, conceitoMinimo;

                QueryTable qtMaximo = new QueryTable(
                    @"SELECT TOP 1 valor_maximo, conceito FROM ly_conceito
                      WHERE grupo = ? ORDER BY valor_maximo DESC");
                qtMaximo.Query(connection, grupoNota);
                notaConceitoMaximo = decimal.Parse(qtMaximo.Rows[0]["valor_maximo"].ToString().Replace(".", ","));
                conceitoMaximo = qtMaximo.Rows[0]["conceito"].ToString();

                QueryTable qtMinimo = new QueryTable(
                    @"SELECT TOP 1 valor_minimo, conceito FROM ly_conceito
                      WHERE grupo = ? ORDER BY valor_minimo ASC");
                qtMinimo.Query(connection, grupoNota);
                notaConceitoMinimo = decimal.Parse(qtMinimo.Rows[0]["valor_minimo"].ToString().Replace(".", ","));
                conceitoMinimo = qtMinimo.Rows[0]["conceito"].ToString();

                if (notaAux > notaConceitoMaximo)
                    return conceitoMaximo;
                else if (notaAux < notaConceitoMinimo)
                    return conceitoMinimo;

                String sql = @"SELECT conceito FROM ly_conceito 
                               WHERE grupo = ? AND
                               ? >= valor_minimo AND ? <= valor_maximo";

                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, grupoNota, nota.Replace(",", "."), nota.Replace(",", "."));
                if (qt.Rows.Count > 0)
                    return qt.Rows[0][0].ToString();
                else
                    return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable ListaGrupoNotas()
        {
            DataContext ctx = Seeduc.Infra.Data.DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable grupos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                grupo
                        FROM    LY_CONCEITO ";

                grupos = ctx.GetDataTable(contextQuery);
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

            return grupos;
        }

        public DataTable ListaConceitosPor(string codigoGrupo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable conceitos = null;

            try
            {
                contextQuery.Command = @" SELECT  conceito ,
                                    descricao
                            FROM    LY_CONCEITO
                            WHERE   GRUPO = @GRUPO
                            ORDER BY NOTA DESC ";

                contextQuery.Parameters.Add("@GRUPO", codigoGrupo);

                conceitos = ctx.GetDataTable(contextQuery);
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

            return conceitos;
        }

        public bool EhConceitoCadastradoPor(string grupo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM    LY_CONCEITO
                                    WHERE   GRUPO = @GRUPO ";

                contextQuery.Parameters.Add("@GRUPO", grupo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public bool EhConceitoCadastradoPor(string grupo, string conceito)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    LY_CONCEITO
                        WHERE   GRUPO = @GRUPO
                                AND CONCEITO = @CONCEITO ";

                contextQuery.Parameters.Add("@GRUPO", grupo);
                contextQuery.Parameters.Add("@CONCEITO", conceito);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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
    }
}
