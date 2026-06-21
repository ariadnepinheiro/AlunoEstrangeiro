using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class Auditoria : RNBase
    {
        private static string bd_auditoria;
        public static string BD_AUDITORIA
        {
            get
            {
                if (String.IsNullOrEmpty(bd_auditoria))                
                    bd_auditoria = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["BancoAuditoria"]);                
                return bd_auditoria;
            }            
        }

        private static string bd_hades;
        public static string BD_HADES
        {
            get
            {
                if (String.IsNullOrEmpty(bd_hades))
                {
                    String hd_cs = HadesLyc.Config.ConnectionString.ToUpper();
                    string[] cs_split = hd_cs.Split(';');
                    for (int i = 0; i < cs_split.Length; i++)
                    {
                        if (cs_split[i].StartsWith("INITIAL CATALOG"))
                        {
                            bd_hades = cs_split[i].Replace("INITIAL CATALOG", "").Replace("=", "");
                            break;
                        }
                    }                    
                }
                return bd_hades;
            }
        }

        public static QueryTable ObterTransacoesAuditoria()
        {
            return Consultar(@"
                SELECT '<não selecionado>' AS trans UNION ALL
                SELECT trans FROM transacao (NOLOCK) WHERE logging = 'S' ORDER BY trans");
        }

        public static QueryTable ObterEstruturas()
        {
            try
            {
                return Consultar(@"
                SELECT s.id, s.name FROM sysobjects s (NOLOCK)
                WHERE 
                    s.type = 'U' AND
                    EXISTS(
	                    SELECT TOP 1 1 FROM " + BD_AUDITORIA + @"..sysobjects ps (NOLOCK)
	                    WHERE 
                            ps.name COLLATE Latin1_General_CI_AI = (s.name + '_audit') COLLATE Latin1_General_CI_AI AND
                            ps.type COLLATE Latin1_General_CI_AI = s.type COLLATE Latin1_General_CI_AI AND
                            ps.xtype COLLATE Latin1_General_CI_AI = s.xtype COLLATE Latin1_General_CI_AI)
                ORDER BY s.name");
            }
            catch
            {
                return null;
            }
        }

        public static QueryTable ObterCamposTabela(String tabela)
        {
            try
            {
                String sql = String.Format(@"                
                SELECT  sc.name AS ColumnName, st.name AS DataType, sc.name + ' (' + st.name + ')' AS Descricao
                FROM    sysobjects so (NOLOCK) INNER JOIN
                        syscolumns sc (NOLOCK) ON so.id = sc.id INNER JOIN
                        systypes st (NOLOCK) ON sc.xusertype = st.xusertype
                WHERE   so.xtype = 'U' AND so.id = OBJECT_ID('{0}') AND
                        EXISTS (
                            SELECT  TOP 1 1
                            FROM    {1}..sysobjects soa (NOLOCK) INNER JOIN
                                    {1}..syscolumns sca (NOLOCK) ON soa.id = sca.id INNER JOIN
                                    {1}..systypes sta (NOLOCK) ON sca.xtype = sta.xtype
                            WHERE   soa.xtype = 'U' AND soa.id = OBJECT_ID('{1}..{0}_audit') and CAST(sc.name AS VARCHAR(MAX)) COLLATE Latin1_General_CI_AI = CAST(sca.name AS VARCHAR(MAX))  COLLATE Latin1_General_CI_AI)
                ORDER BY ColumnName", tabela, BD_AUDITORIA);
                return Consultar(sql);
            }
            catch
            {
                return null;
            }
        }

        public static QueryTable ObterPaginas()
        {

            TConnection conn = Techne.HadesLyc.Config.CreateConnection();

            try
            {
                conn.Open();
                return Consultar(conn,
                    @"  SELECT DISTINCT paginaweb, REPLACE(REPLACE(paginaweb, '~/', ''), '/', ' - ') paginaweb_descr 
                        FROM hd_transacao (NOLOCK) WHERE SIS = 'LyceumNet' AND auditar = 'S' 
                        ORDER BY paginaweb");
            }
            catch { return null; }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }
        }

        public static String[] ObterPrimaryKeys(String tabela)
        {
            try
            {
                QueryTable qt = Consultar(@"
                select COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE a
                    inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS b
                    on a.CONSTRAINT_NAME = b.CONSTRAINT_NAME
                where a.table_name = ? and constraint_type = 'Primary key'", tabela);

                if (qt != null && qt.Rows.Count > 0)
                    return qt.Rows.Cast<SimpleRow>().Select(row => Convert.ToString(row[0])).ToArray();
                else
                    return new string[] { };
            }
            catch
            {
                return new string[] { };
            }
        }

        public static QueryTable ObterDadosAuditoria(String setor, String pagina, String usuario, String maquina, String estrutura, String[] campos, Dictionary<string, string> valores, DateTime? dtInicio, DateTime? dtFim, bool opInsert, bool opUpdate, bool opDelete, string histid, out Dictionary<string, string> pkValores)
        {
            pkValores = new Dictionary<string, string>();

            try
            {                
                if (String.IsNullOrEmpty(estrutura)) return Consultar("SELECT '<estrutura não selecionada>' AS mensagem");

                string[] pks = ObterPrimaryKeys(estrutura);
                String selectCampos = "tab.DATA_AUD Data, tab.USUARIO_AUD Usuário, hist.status Operação, trans.estacao Estação, trans.pagina Transação";

                selectCampos += ", " + pks.Select(pk => "tab." + pk).Aggregate((a, b) => a + ", " + b);

                if (campos != null && campos.Length > 0)
                    selectCampos += ", " + campos.Select(campo => "tab." + campo).Aggregate((a, b) => a + ", " + b);

                StringBuilder sqlSelect = new StringBuilder();
                sqlSelect.AppendLine(
                @"  SELECT {0}
                        from ZZCRO_HIST hist (NOLOCK)
                    INNER JOIN ZZCRO_HISTTRANS trans (NOLOCK) ON hist.TransId = trans.Id
                    INNER JOIN {1}..{2}_audit tab (NOLOCK) ON tab.HISTID = hist.Id
                    LEFT JOIN usuario u ON u.usuario = trans.usuario
                    LEFT JOIN transacao t ON t.trans = trans.pagina
                    LEFT JOIN {3}..hd_transacao hdt ON hdt.TRANS = trans.Pagina");
                string consulta = String.Format(sqlSelect.ToString(), selectCampos, BD_AUDITORIA, estrutura, BD_HADES);

                List<string> sqlWhere = new List<string>();

                List<string> statusSelecionados = new List<string>();
                if (opInsert) statusSelecionados.Add("'CADASTRADO'");
                if (opUpdate) statusSelecionados.Add("'ALTERADO'");
                if (opDelete) statusSelecionados.Add("'REMOVIDO'");
                if (statusSelecionados.Count > 0)
                    sqlWhere.Add(String.Format("hist.status IN ({0})", statusSelecionados.Aggregate((a, b) => a + ", " + b)));

                if (!String.IsNullOrEmpty(setor))
                    sqlWhere.Add(String.Format("u.setor = '{0}'", setor));

                if (!String.IsNullOrEmpty(usuario))
                    sqlWhere.Add(String.Format("u.usuario = '{0}'", usuario));

                if (!String.IsNullOrEmpty(pagina))
                    sqlWhere.Add(String.Format("hdt.paginaweb = '{0}'", pagina));

                if (!String.IsNullOrEmpty(maquina))
                    sqlWhere.Add(String.Format("trans.estacao = '{0}'", maquina));

                if (!String.IsNullOrEmpty(estrutura))
                    sqlWhere.Add(String.Format("hist.tabela = '{0}'", estrutura));

                if (dtInicio.HasValue)
                    sqlWhere.Add(String.Format("tab.data_aud >= '{0}'", dtInicio.Value.ToString("yyyy-MM-dd HH:mm:ss")));

                if (dtFim.HasValue)
                    sqlWhere.Add(String.Format("tab.data_aud <= '{0}'", dtFim.Value.ToString("yyyy-MM-dd HH:mm:ss")));

                if (!String.IsNullOrEmpty(histid))
                    sqlWhere.Add(String.Format("hist.id = '{0}'", histid));

                if (valores != null && valores.Count > 0)
                {
                    foreach (string pk in valores.Keys)
                    {
                        string pkValor = valores[pk];
                        sqlWhere.Add(String.Format("tab.{0} = '{1}'", pk, pkValor));
                    }
                }

                String sql = consulta;
                if (sqlWhere.Count > 0)
                    sql += " WHERE " + sqlWhere.Aggregate((a, b) => a + " AND " + b);

                String sqlOrder = " ORDER BY " + pks.Select(a => "tab." + a).Aggregate((a, b) => a + ", " + b) + ", hist.id desc";
                sql += sqlOrder;

                QueryTable qt = Consultar(sql);

                if (String.IsNullOrEmpty(histid) || qt.Rows.Count == 0)
                    return qt;
                else
                {
                    List<string> sqlWherePK = new List<string>();
                    for (int i = 0; i < pks.Length; i++)
                    {
                        if (qt.Rows[0][pks[i]].Type == DbType.Date)
                        {
                            sqlWherePK.Add(String.Format("tab.{0} = '{1}'", pks[i], Convert.ToDateTime(qt.Rows[0][pks[i]]).ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            pkValores.Add(pks[i], Convert.ToDateTime(qt.Rows[0][pks[i]]).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        }
                        else
                        {
                            sqlWherePK.Add(String.Format("tab.{0} = '{1}'", pks[i], qt.Rows[0][pks[i]]));
                            pkValores.Add(pks[i], Convert.ToString(qt.Rows[0][pks[i]]));
                        }
                    }
                    sqlWherePK.Add(String.Format("hist.id <= '{0}'", histid));

                    string sqlHistId = consulta;
                    if (sqlWherePK.Count > 0)
                        sqlHistId += " WHERE " + sqlWherePK.Aggregate((a, b) => a + " AND " + b);
                    sqlHistId += sqlOrder;
                    return Consultar(sqlHistId);
                }
            }
            catch
            {               
                return null;
            }
        }

        public static QueryTable ObterListaTransacoes(String setor, String usuario, string pagina, string maquina, DateTime? dataInicio, DateTime? dataFim, bool opInsert, bool opUpdate, bool opDelete, int maxReg)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(String.Format(@"
                SELECT TOP {0} h.id HistId, h.TransId, h.Stamp Data, h.Status Operacao, h.Tabela, ht.Estacao,  ht.Usuario, U.SETOR, U.NOMEUSUARIO, ht.Pagina, hdt.PAGINAWEB, REPLACE(hdt.paginaweb, '~/', '') paginaweb_descr FROM ZZCRO_Hist h (NOLOCK)
                INNER JOIN ZZCRO_HistTrans ht (NOLOCK) ON ht.id = h.TransId
                INNER JOIN {1}..HD_TRANSACAO hdt (NOLOCK) ON hdt.TRANS = ht.Pagina
                INNER JOIN usuario u (NOLOCK) ON u.USUARIO = ht.Usuario
                    WHERE hdt.SIS = 'LyceumNet'", maxReg, BD_HADES));

            if (!String.IsNullOrEmpty(setor))
                sql.AppendLine(String.Format("AND u.setor = '{0}'", setor));
            if (!String.IsNullOrEmpty(usuario))
                sql.AppendLine(String.Format("AND u.usuario = '{0}'", usuario));

            if (!String.IsNullOrEmpty(pagina))
                sql.AppendLine(String.Format("AND hdt.PaginaWeb = '{0}'", pagina));

            if (!String.IsNullOrEmpty(maquina))
                sql.AppendLine(String.Format("AND ht.Estacao = '{0}'", maquina));

            if (dataInicio.HasValue)
                sql.AppendLine(String.Format("AND h.Stamp >= '{0}'", dataInicio.Value.ToString("yyyy-MM-dd HH:mm:ss")));

            if (dataInicio.HasValue)
                sql.AppendLine(String.Format("AND h.Stamp <= '{0}'", dataFim.Value.ToString("yyyy-MM-dd HH:mm:ss")));

            List<string> operacoes = new List<string>();
            if (opDelete) operacoes.Add("'REMOVIDO'");
            if (opInsert) operacoes.Add("'CADASTRADO'");
            if (opUpdate) operacoes.Add("'ALTERADO'");

            if (operacoes.Count > 0)
                sql.AppendLine(String.Format("AND h.Status in ({0})", operacoes.Aggregate((a, b) => a + ", " + b)));

            sql.AppendLine(" ORDER BY h.id DESC");

            QueryTable qt = new QueryTable(sql.ToString());
            qt.CommandTimeout = 999;

            TConnection conn = Config.CreateConnection();
            conn.Open();
            try
            {
                qt.Query(conn);
                return qt;
            }
            catch { return null; }
            finally { conn.Close(); }
        }
    }
}
