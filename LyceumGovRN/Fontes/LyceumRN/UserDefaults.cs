using System;
using System.Data;
using System.Web;

using Techne.Data;

namespace Techne.Lyceum
{
    public class LyceumUser
    {
        private string user;
        private bool privilegiado;
        private bool restricao_simnao;
        private bool restricao_unidade;
        private bool restricao_curso;
        private bool restricao_unid_fis;

        /// <summary>
        /// Obtém usuário.
        /// </summary>
        public string User
        {
            get { return user; }
        }

        /// <summary>
        /// Obtém privilegiado.
        /// </summary>
        public bool Privilegiado
        {
            get { return privilegiado; }
        }

        public bool Restricao_SimNao
        {
            get { return restricao_simnao; }
        }

        public bool Restricao_Unidade
        {
            get { return restricao_unidade; }
        }

        public bool Restricao_Curso
        {
            get { return restricao_curso; }
        }

        public bool Restricao_Unid_Fis
        {
            get { return restricao_unid_fis; }
        }


        private LyceumUser(string user, bool privilegiado, bool restricao_simnao, bool restricao_unidade, bool restricao_curso, bool restricao_unid_fis)
        {
            this.user = user;
            this.privilegiado = privilegiado;
            this.restricao_simnao = restricao_simnao;
            this.restricao_unidade = restricao_unidade;
            this.restricao_curso = restricao_curso;
            this.restricao_unid_fis = restricao_unid_fis;
        }

        public static LyceumUser Current
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                        return HttpContext.Current.Session["LyceumUserInfo"] as LyceumUser;
                    else
                        return null;
                }
                catch { return null; }
            }
        }

        public static LyceumUser Get(string usuario)
        {
            string sql =
              "SELECT hd_usuario.usuario, hd_usuario.privilegiado " +
                "FROM hd_usuario WHERE usuario = '" + RN.RNBase.MudarAspas(usuario) + "'";

            TConnection cn = ConnectionList.CreateWritableConnection("Hades");
            TDataAdapter da = new TDataAdapter(sql, cn);
            DataSet ds = new DataSet();

            cn.Open();
            try
            {
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw new Exception("Possível erro na sintaxe do SQL: " + sql + ". Connection string: " + cn.ConnectionString, e);
            }
            finally
            {
                if (cn.State == ConnectionState.Open) cn.Close();
            }

            string sql2 = "SELECT restricao_simnao, restricao_unidade, restricao_curso, restricao_unid_fis FROM ly_opcoes_acesso";

            TConnection cn2 = ConnectionList.CreateWritableConnection("Lyceum");
            TDataAdapter da2 = new TDataAdapter(sql2, cn2);
            DataSet ds2 = new DataSet();

            cn2.Open();
            try
            {
                da2.Fill(ds2);
            }
            catch (Exception e)
            {
                throw new Exception("Possível erro na sintaxe do SQL: " + sql2 + ". Connection string: " + cn2.ConnectionString, e);
            }
            finally
            {
                if (cn2.State == ConnectionState.Open) cn2.Close();
            }

            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                DataRow rowopt = ds2.Tables[0].Rows[0];
                return new LyceumUser(usuario, row["privilegiado"].ToString().ToUpper() == "S", rowopt["restricao_simnao"].ToString().ToUpper() == "S",
                                rowopt["restricao_unidade"].ToString().ToUpper() == "S", rowopt["restricao_curso"].ToString().ToUpper() == "S",
                                rowopt["restricao_unid_fis"].ToString().ToUpper() == "S");
            }
            else
                return new LyceumUser(usuario, false, false, false, false, false);
        }

        public static void SetCurrent(LyceumUser lyceumUser)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                HttpContext.Current.Session["LyceumUserInfo"] = lyceumUser;
        }
    }
}
