using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.HadesLyc.CR;
using Techne.Lyceum.CR;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class PadraoAcessoTurmas : RNBase
    {
        private static Hd_padusuario ConsultarPadUsuario(String usuario)
        {
            return Hd_padusuario.Query(Techne.HadesLyc.Config.CreateConnection(), "hd_padusuario.usuario = ?", usuario);
        }

        private static Ly_padaces_turma ConsultarPadAces(String curso, String[] padaces)
        {
            if (String.IsNullOrEmpty(curso) || padaces == null || padaces.Length == 0)
                return null;

            string sqlWhere = "curso = ? AND padaces in ( ";
            
            List<string> pars = new List<string>();
            pars.Add(curso);

            foreach (string padace in padaces)
            {
                sqlWhere += "?,";
                pars.Add(padace);
            }

            sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 1) + ")";

            return Ly_padaces_turma.Query(Techne.Lyceum.Config.CreateConnection(),
                sqlWhere, pars.ToArray());
        }

        private static Hd_usuario.Row ConsultarUsuario(String usuario)
        {
            return Hd_usuario.Row.Query(Techne.HadesLyc.Config.CreateConnection(), usuario);
        }

        public static DadosPadraoAcessoTurma ConsultarPadAcesTurma(String curso, String usuario)
        {
            if (String.IsNullOrEmpty(curso) || String.IsNullOrEmpty(usuario))
                return new DadosPadraoAcessoTurma();            

            Hd_usuario.Row rowUsuario = ConsultarUsuario(usuario);
            if (rowUsuario == null) return new DadosPadraoAcessoTurma();

            if(rowUsuario.Privilegiado == "S")
                return new DadosPadraoAcessoTurma(true, true,false);

            Hd_padusuario rowPadUsuario = ConsultarPadUsuario(usuario);
            if (rowPadUsuario == null) return new DadosPadraoAcessoTurma();

            Ly_padaces_turma padaces = ConsultarPadAces(curso, rowPadUsuario.Rows.Cast<Hd_padusuario.Row>().Select(p => p.Padaces).ToArray());
            if (padaces == null) return new DadosPadraoAcessoTurma();

            var padacesRows = padaces.Rows.Cast<Ly_padaces_turma.Row>();

            Boolean permissaoGeral = padacesRows.Count(p =>
                p.Operacao.ToUpper().Contains("GERAL")
                && DateTime.Today >= p.Dt_inicio
                && DateTime.Today <= p.Dt_fim) > 0;
            Boolean permissaoQHI = padacesRows.Count(p =>
                p.Operacao.ToUpper().Contains("QUADRO")
                && DateTime.Today >= p.Dt_inicio
                && DateTime.Today <= p.Dt_fim) > 0;
            Boolean permissaoParcial = padacesRows.Count(p =>
               p.Operacao.ToUpper().Contains("PARCIAL")
               && DateTime.Today >= p.Dt_inicio
               && DateTime.Today <= p.Dt_fim) > 0;

            return new DadosPadraoAcessoTurma(permissaoQHI, permissaoGeral, permissaoParcial);            
        }


        public static bool ConsultarPermissaoAlocacaoDocSemAula(String usuario)
        {
            TConnection conn = Config.CreateConnection();

            try
            {
                conn.Open();
                DbObject dbValue = TCommand.ExecuteScalar(conn,
                    @"SELECT TOP 1 1 
                      FROM padusuario padu
                      INNER JOIN itemtabela it ON it.item = padu.padaces AND it.tab = ?
                      WHERE padu.usuario = ?", "AlocacaoDocSemAula", usuario);

                if (dbValue == null) return false;
                else return !dbValue.IsNull;
            }
            catch
            {
                conn.Rollback();
            }
            finally
            {
                if(conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }
            return false;
        }

        public static bool EhPermissaoDiretorAlocacaoDocSemAula(String usuario)
        {
            TConnection conn = Config.CreateConnection();

            try
            {
                conn.Open();
                DbObject dbValue = TCommand.ExecuteScalar(conn,
                    @"SELECT TOP 1 1 
                      FROM padusuario padu
                      INNER JOIN itemtabela it ON it.item = padu.padaces AND it.tab = ?
                      WHERE padu.usuario = ?
                        AND PADACES = 'DIRETOR_UE'", "AlocacaoDocSemAula", usuario);

                if (dbValue == null) return false;
                else return !dbValue.IsNull;
            }
            catch
            {
                conn.Rollback();
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }
            return false;
        }
    }

    public class DadosPadraoAcessoTurma
    {
        private Boolean permissaoQHI = false;
        public Boolean PermissaoQHI { get { return permissaoQHI; } }

        private Boolean permissaoGeral = false;
        public Boolean PermissaoGeral { get { return permissaoGeral; } }

        private Boolean permissaoParcial = false;
        public Boolean PermissaoParcial { get { return permissaoParcial; } }

        public DadosPadraoAcessoTurma(Boolean permissaoQHI, Boolean permissaoGeral, Boolean permissaoParcial )
        {
            this.permissaoGeral = permissaoGeral;
            this.permissaoQHI = permissaoQHI;
            this.permissaoParcial = permissaoParcial;
        }

        public DadosPadraoAcessoTurma() { }
    }
}
