using System;
using System.Collections.Generic;
using System.Text;
using Techne.Data;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.RN
{
    public class Relatorio : RNBase
    {
        public static bool TemPermissao(string ReportHades, string GrupoRelat, string userID, ref string HadesUsu, ref string Msg)
        {

            #region Validações iniciais
            if (ReportHades == null)
            {
                Msg = "Relatório não especificado.";
                return false;
            }
            #endregion

            //Obtem usuário do Hades, e verifica sua permissão de acesso ao relatório através da procedure AcessoRelatorioRS

            TConnection connection = HadesLyc.Config.CreateConnection();
            connection.Open();

            QueryTable qt = new QueryTable(@"DECLARE @Usuario VARCHAR(100), 
                    @GrupoRelat VARCHAR(50),
                    @Relatorio VARCHAR(50),
                    @HadesUsu VARCHAR(15),
                    @Msg VARCHAR(200)
                      
                    SET @Usuario = ?
                    SET @GrupoRelat = ?
                    SET @Relatorio = ?

                    SET @Msg = ''

                    SELECT @HadesUsu = USUARIO FROM HD_USUARIO WHERE UPPER(USUARIO) = UPPER(@Usuario)
                    IF LEN(ISNULL(@HadesUsu, '')) = 0
                    SET @Msg = 'Acesso negado. Usuário windows não está associado a nenhum usuário no sistema Hades.' 
                    ELSE 
                    IF NOT EXISTS(SELECT RELATORIO
                                  FROM HD_RELATORIO
                                  WHERE UPPER(SIS) = 'LyceumNet' AND  UPPER(GRUPORELAT) = UPPER(@GrupoRelat) AND UPPER(RELATORIO) = UPPER(@Relatorio))
                      SET @Msg = 'Relatório não foi encontrado no cadastro de relatórios do sistema Hades.'
                    ELSE
                      IF NOT EXISTS(SELECT RELATORIO
                                    FROM HD_PADREL pr INNER JOIN HD_PADUSUARIO pu ON pr.PADACES = pu.PADACES
                                                      INNER JOIN HD_USUARIO u on pu.USUARIO = u.USUARIO
                                    WHERE (UPPER(pr.SIS) = 'LyceumNet' AND  UPPER(pr.GRUPORELAT) = UPPER(@GrupoRelat) AND
                                           pu.USUARIO = @HadesUsu AND UPPER(pr.RELATORIO) = UPPER(@Relatorio)) OR
                                          (pu.USUARIO = @HadesUsu AND u.PRIVILEGIADO = 'S'))
                        SET @Msg = 'Usuário não tem acesso ao relatório solicitado.'

                    SELECT @Msg Mensagem, @HadesUsu UsuarioHades ");

            qt.Query(connection, userID, GrupoRelat, ReportHades);

            connection.Close();

            if (qt.Rows.Count > 0)
            {
                Msg = qt.Rows[0]["Mensagem"].ToString();
                HadesUsu = qt.Rows[0]["UsuarioHades"].ToString();
                return (Msg.Length == 0);
            }
            else
            {
                return false;
            }

        }

        public static QueryTable Relatorios(string usuario)
        {
            TConnection cn = HadesLyc.Config.CreateConnection();

            QueryTable qt = new QueryTable(@"SELECT distinct r.gruporelat, r.relatorio, r.descricao
                                        FROM HD_PADREL pr INNER JOIN HD_PADUSUARIO pu ON pr.PADACES = pu.PADACES
	                                        INNER JOIN HD_USUARIO u on pu.USUARIO = u.USUARIO
	                                        inner join HD_RELATORIO r on pr.SIS = r.SIS and pr.GRUPORELAT = r.GRUPORELAT and pr.RELATORIO = r.RELATORIO
                                        WHERE (UPPER(pr.SIS) = 'LyceumNet' AND pu.USUARIO = ?) OR
                                        (pu.USUARIO = ? AND u.PRIVILEGIADO = 'S')");

            qt.Query(cn, usuario, usuario);

            return qt;
        }



        public static QueryTable GrupoRelatorio(string usuario)
        {
            TConnection cn = HadesLyc.Config.CreateConnection();

            QueryTable qt = new QueryTable(@"SELECT distinct g.gruporelat, g.descricao
            FROM HD_PADREL pr INNER JOIN HD_PADUSUARIO pu ON pr.PADACES = pu.PADACES
	            INNER JOIN HD_USUARIO u on pu.USUARIO = u.USUARIO
	            inner join HD_GRUPO_RELATORIOS g on pr.SIS = g.SIS and pr.GRUPORELAT = g.GRUPORELAT
            WHERE (UPPER(pr.SIS) = 'LyceumNet' AND pu.USUARIO = ?) OR
            (pu.USUARIO = ? AND u.PRIVILEGIADO = 'S')");

            qt.Query(cn, usuario, usuario);

            return qt;
        }

        public static bool PermiteRemoverGrupoRelatorio(string p)
        {
            string sql = "select 1 from HD_Relatorio where gruporelat = ? and sis = 'LyceumNet'";
            SimpleRow[] sr = new QueryTable(sql).Query(HadesLyc.Config.CreateConnection(), p);
            if (sr.Length > 0)
                return false;
            else
                return true;
        }

        public static bool ExisteGrupoRelatorio(string grupoRelatorio)
        {
            string sql = "select 1 from HD_GRUPO_RELATORIOS where gruporelat = ? and SIS = 'LyceumNet'";
            SimpleRow[] sr = new QueryTable(sql).Query(HadesLyc.Config.CreateConnection(), grupoRelatorio);
            if (sr.Length > 0)
                return true;
            else
                return false;
        }

        public static QueryTable ExecutaQuery(string query, List<object> list)
        {
            TConnection cn = Config.CreateConnection();
            QueryTable qt = new QueryTable(query);

            qt.Query(cn, list.ToArray());
            
            return qt;
        }
    }
}
