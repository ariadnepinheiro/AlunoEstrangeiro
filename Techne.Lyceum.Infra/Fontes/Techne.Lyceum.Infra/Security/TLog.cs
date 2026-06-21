using System;
using System.Data;
using System.Threading;
using System.Web;
using Techne.Data;

namespace Techne
{
    public enum TLogEntryType
    {
        Error, 
        Warning, 
        Information
    }

    public enum TLogCategory
    {
        None, 
        Runtime, 
        Process, 
        Report, 
        Security, 
        Query
    }

    public class TLog
    {
        // Chama InsereAuditEvento()

        /// <summary>
        ///   Grava evento no log
        /// </summary>
        /// <param name = "source">Origem do evento. Pode ser nome da transação, relatório, processo, etc.</param>
        /// <param name = "category">Categoria da Origem. Por exemplo:'TRANSACAO','RELATORIO','PROCESSO','SEGURANCA'.</param>
        /// <param name = "sis">Sistema que gerou o evento.</param>
        /// <param name = "message">Texto descritivo do evento.</param>
        /// <param name = "type">Tipo do evento (Erro, Aviso, Informação, etc)</param>
        /// <returns>Retorna número identificador do registro de auditoria</returns>
        public static decimal WriteEntry(string source, TLogCategory category, string sis, string message, TLogEntryType type, 
                                         string[] paramNames, string[] paramValues)
        {
            var log = GetNew();
            if (log == null)
            {
                return 0;
            }

            return log.DoWriteEntry(source, category, sis, message, type, paramNames, paramValues);
        }

        /// <summary>
        ///   Grava evento no log
        /// </summary>
        /// <param name = "source">Origem do evento. Pode ser nome da transação, relatório, processo, etc.</param>
        /// <param name = "category">Categoria da Origem. Por exemplo:'TRANSACAO','RELATORIO','PROCESSO','SEGURANCA'.</param>
        /// <param name = "sis">Sistema que gerou o evento.</param>
        /// <param name = "message">Texto descritivo do evento.</param>
        /// <param name = "type">Tipo do evento (Erro, Aviso, Informação, etc)</param>
        /// <returns>Retorna número identificador do registro de auditoria</returns>
        public static decimal WriteEntry(string source, TLogCategory category, string sis, string message, TLogEntryType type)
        {
            return WriteEntry(source, category, sis, message, type, null, null);
        }

        internal static decimal LogWebSession()
        {
            var log = GetNew();
            if (log == null)
            {
                return 0;
            }

            return log.DoLogWebSession();
        }

        protected virtual decimal DoWriteEntry(string source, TLogCategory category, string sys, string message, 
                                               TLogEntryType type, string[] paramNames, string[] paramValues)
        {
            // Pega caractere correspondente ao tipo
            string tipo;
            switch (type)
            {
                case TLogEntryType.Information:
                    tipo = "I";
                    break;
                case TLogEntryType.Warning:
                    tipo = "A";
                    break;
                case TLogEntryType.Error:
                    tipo = "E";
                    break;
                default:
                    tipo = "I";
                    break;
            }

            // Pega texto correspondente à categoria
            string categoria;
            switch (category)
            {
                case TLogCategory.None:
                    categoria = "Nenhuma";
                    break;
                case TLogCategory.Runtime:
                    categoria = "Execução";
                    break;
                case TLogCategory.Process:
                    categoria = "Processo";
                    break;
                case TLogCategory.Report:
                    categoria = "Relatório";
                    break;
                case TLogCategory.Security:
                    categoria = "Segurança";
                    break;
                case TLogCategory.Query:
                    categoria = "Consulta";
                    break;
                default:
                    categoria = "Outra";
                    break;
            }

            // Pega usuário corrente
            string usuario;
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                usuario = string.Empty + HttpContext.Current.User.Identity.Name;
            }
            else
            {
                usuario = (string.Empty + Thread.CurrentPrincipal.Identity.Name).Trim();
            }

            // Pega máquina
            var machine = GetMachine();

            // Pega sessão corrente
            decimal sessao;
            if (HttpContext.Current != null &&
                HttpContext.Current.Session != null &&
                HttpContext.Current.Session["__HadesLogWebSession"] is decimal)
            {
                sessao = (decimal)HttpContext.Current.Session["__HadesLogWebSession"];
            }
            else
            {
                sessao = 0;
            }

            return InsereAuditEvento(ConnectionList.CreateWritableConnectionWithoutPermission("Hades"), 
                                     tipo, source, categoria, sys, usuario, 
                                     machine, sessao, message, paramNames, paramValues);
        }

        // Chama GravaSessao() e inicializa a variável de sessão __HadesLogWebSession.
        protected decimal DoLogWebSession()
        {
            // Verifica se existem contexto Http, Sessão e TPrincipal
            if (HttpContext.Current == null ||
                HttpContext.Current.Session == null ||
                HttpContext.Current.User.Identity.Name + string.Empty == string.Empty)
            {
                return 0;
            }

            // Verifica se a sessão já foi logada
            Number sessionid;
            if (HttpContext.Current.Session["__HadesLogWebSession"] is decimal)
            {
                sessionid = (decimal)HttpContext.Current.Session["__HadesLogWebSession"];
            }
            else
            {
                sessionid = DBNull.Value;
            }

            var machine = GetMachine();
            var user = HttpContext.Current.User.Identity.Name;

            GravaSessao(ConnectionList.CreateWritableConnectionWithoutPermission("Hades"), ref sessionid, machine, user);

            HttpContext.Current.Session["__HadesLogWebSession"] = (decimal)sessionid;
            return (decimal)sessionid;
        }

        private static string GetMachine()
        {
            string machine;

            // Pega máquina. Se for request de página, pega o endereço da máquina cliente.
            // Em outros casos, pega o nome da máquina onde esta dll está rodando.
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                machine = HttpContext.Current.Request.UserHostAddress;

                if (HttpContext.Current.Request.UserHostName != null &&
                    HttpContext.Current.Request.UserHostName != string.Empty &&
                    HttpContext.Current.Request.UserHostName != machine)
                {
                    machine += "(" + HttpContext.Current.Request.UserHostName + ")";
                }

                if (machine == "127.0.0.1" || machine == "localhost")
                {
                    machine = Environment.MachineName;
                }
            }
            else
            {
                machine = Environment.MachineName;
            }

            if (machine.Length > 60)
            {
                machine = machine.Substring(0, 60);
            }

            return machine;
        }

        private static TLog GetNew()
        {
            if (HttpContext.Current != null && HttpContext.Current.ApplicationInstance is TechneHttpApplication)
            {
                return ((TechneHttpApplication)HttpContext.Current.ApplicationInstance).GetTLog();
            }
            else
            {
                return null;
            }
        }

        // Insere registro em HD_AUDIT_SESSAO
        private static void GravaSessao(TConnectionWritable cn, ref Number sessao, 
                                        string estacao, string usuario)
        {
            

            cn.Open(true);
            try
            {
                

                var now = DateTime.Now;

                if (sessao.IsNull)
                {
                    if (cn.Rdbms == Rdbms.SQLServer)
                    {
                        sessao = (Number)TCommand.ExecuteScalar(cn, 
                                                                "INSERT INTO hd_audit_sessao(dtini, dtfim, estacao, usuario) " +
                                                                "VALUES(?, ?, ?, ?);" +
                                                                "SELECT SCOPE_IDENTITY()", 
                                                                now, now, estacao, usuario
                                             );
                    }
                    else if (cn.Rdbms == Rdbms.Oracle)
                    {
                        sessao = (Number)TCommand.ExecuteScalar(cn, "SELECT seq_audit_sessao.NEXTVAL FROM DUAL");
                        TCommand.ExecuteNonQuery(cn, 
                                                 "INSERT INTO hd_audit_sessao(sessao, dtini, dtfim, estacao, usuario) " +
                                                 "VALUES(?, ?, ?, ?, ?)", 
                                                 sessao, now, now, estacao, usuario
                            );
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    var row = SimpleRow.QueryFirstRow(cn, 
                                                      "SELECT usuario, estacao " +
                                                      "FROM hd_audit_sessao " +
                                                      "WHERE sessao = ?", 
                                                      sessao
                        );
                    TCommand.ExecuteNonQuery(cn, 
                                             "UPDATE hd_audit_sessao " +
                                             "SET dtfim = ?, usuario = ?, estacao = ? " +
                                             "WHERE sessao = ?", 
                                             now, row["usuario"].IsNull ? usuario : row["usuario"], 
                                             row["estacao"].IsNull ? estacao : row["estacao"], sessao
                        );
                }

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }

        // Insere registro em HD_ORIGEM_EVENTO, HD_PARAMETRO_EVENTO, HD_AUDIT_EVENTO, HD_AUDIT_EVENTO_PARAMETRO
        private static decimal InsereAuditEvento(TConnectionWritable cn, 
                                                 string tipo, string origem, string categoria, 
                                                 string sis, string usuario, string estacao, 
                                                 decimal sessao, VarChar descricao, 
                                                 string[] paramNames, string[] paramValues)
        {
            

            cn.Open(true);
            try
            {
                

                if (TCommand.ExecuteScalar(cn, 
                                           "SELECT COUNT(*) " +
                                           "FROM hd_origem_evento " +
                                           "WHERE sis = ? AND origem = ? AND categoria = ?", 
                                           sis, origem, categoria
                        ) == 0)
                {
                    TCommand.ExecuteNonQuery(cn, 
                                             "INSERT INTO hd_origem_evento(sis, categoria, origem) " +
                                             "VALUES(?, ?, ?)", 
                                             sis, categoria, origem
                        );
                }

                if (descricao.Length == 0)
                {
                    descricao = DBNull.Value;
                }

                if (sessao <= 0)
                {
                    sessao = 0;
                }

                // INSERE EVENTO
                decimal evento;
                if (cn.Rdbms == Rdbms.SQLServer)
                {
                    evento = (decimal)(Number)TCommand.ExecuteScalar(cn, 
                                                                     "INSERT INTO hd_audit_evento(tipo, data, origem, categoria, sis, usuario, estacao, sessao, descricao) " +
                                                                     "VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?);" +
                                                                     "SELECT SCOPE_IDENTITY()", 
                                                                     tipo, DateTime.Now, origem, categoria, sis, usuario.Length == 0 ? DBNull.Value : (DbObject)usuario, estacao, sessao == 0 ? DBNull.Value : (DbObject)sessao, descricao
                                                  );
                }
                else if (cn.Rdbms == Rdbms.Oracle)
                {
                    evento = (decimal)(Number)TCommand.ExecuteScalar(cn, "SELECT seq_audit_evento.NEXTVAL FROM DUAL");
                    TCommand.ExecuteNonQuery(cn, 
                                             "INSERT INTO hd_audit_evento(evento, tipo, data, origem, categoria, sis, usuario, estacao, sessao, descricao) " +
                                             "VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", 
                                             evento, tipo, DateTime.Now, origem, categoria, sis, usuario.Length == 0 ? DBNull.Value : (DbObject)usuario, estacao, sessao == 0 ? DBNull.Value : (DbObject)sessao, descricao
                        );
                }
                else
                {
                    throw new NotImplementedException();
                }

                // INSERE PARÂMETROS
                if (paramNames != null)
                {
                    for (var i = 0; i < paramNames.Length; i++)
                    {
                        var nome = paramNames[i];
                        var valor = paramValues[i];

                        if ((Number)TCommand.ExecuteScalar(cn, 
                                                           "SELECT COUNT(*) " +
                                                           "FROM hd_parametro_evento " +
                                                           "WHERE sis = ? AND origem = ? AND categoria = ? AND parametro = ?", 
                                                           sis, origem, categoria, nome
                                        ) == 0)
                        {
                            TCommand.ExecuteNonQuery(cn, 
                                                     "INSERT INTO hd_parametro_evento(sis, categoria, origem, parametro) " +
                                                     "VALUES(?, ?, ?, ?)", 
                                                     sis, categoria, origem, nome
                                );
                        }

                        TCommand.ExecuteNonQuery(cn, 
                                                 "INSERT INTO hd_audit_evento_parametro(evento, parametro, valor) " +
                                                 "VALUES(?, ?, ?)", 
                                                 evento, nome, valor
                            );
                    }
                }

                return evento;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }
    }
}