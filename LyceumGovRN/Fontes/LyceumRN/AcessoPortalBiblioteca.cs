using System;
using System.Web.Security;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class AcessoPortalBiblioteca : RNBase
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string NovaSenha { get; set; }
        public int Biblioteca { get; set; }
        public string Nome { get; set; }
        public bool AlteraSenha { get; set; }
        public bool Valido { get; set; }

        /// <summary>
        /// Seta usuário e senha de acesso ao portal.
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <param name="senha">senha</param>
        public AcessoPortalBiblioteca(string usuario, string senha)
        {
            this.Usuario = usuario;
            this.Senha = senha;
            this.Valido = false;
            this.AlteraSenha = false;
        }

        /// <summary>
        /// Seta usuário, senha e nova senha de acesso ao porta
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="senha"></param>
        /// <param name="novaSenha"></param>
        public AcessoPortalBiblioteca(string usuario, string senha, string novaSenha)
        {
            this.Usuario = usuario;
            this.Senha = senha;
            this.NovaSenha = novaSenha;
            this.Valido = false;
            this.AlteraSenha = false;
        }

        /// <summary>
        /// Efetua login no portal.
        /// </summary>
        public void LoginPortalBiblioteca()
        {
            if (!string.IsNullOrEmpty(this.Usuario))
            {
                QueryTable qt = new QueryTable("Select p.senha_alterada, p.senha_tac, p.nome_compl, convert(int,bu.id_bib_biblioteca) id_bib_biblioteca from LY_BIB_USUARIO bu join LY_PESSOA p on p.PESSOA = bu.PESSOA where bu.PESSOA = ?");
                TConnection cn = Config.CreateConnection();
                qt.Query(cn, this.Usuario);

                string senhaCript = FormsAuthentication.HashPasswordForStoringInConfigFile(this.Senha, "SHA1");
                if (qt.Rows.Count > 0)
                {
                    for (int i = 0; i < qt.Rows.Count; i++)
                    {
                        if (qt.Rows[i]["senha_tac"].ToString().Length > 15)
                        {
                            if (qt.Rows[i]["senha_tac"].ToString() == senhaCript)
                            {
                                if (qt.Rows[i]["senha_alterada"].ToString() == "N")
                                    this.AlteraSenha = true;
                                this.Valido = true;
                                this.Biblioteca = Convert.ToInt32(qt.Rows[i]["id_bib_biblioteca"]);
                                this.Nome = qt.Rows[i]["nome_compl"].ToString();
                                return;
                            }
                        }
                        else
                        {
                            if (qt.Rows[i]["senha_tac"].ToString() == this.Senha)
                            {
                                if (qt.Rows[i]["senha_alterada"].ToString() == "N")
                                    this.AlteraSenha = true;
                                this.Valido = true;
                                this.Biblioteca = Convert.ToInt32(qt.Rows[i]["id_bib_biblioteca"]);
                                this.Nome = qt.Rows[i]["nome_compl"].ToString();
                                return;
                            }
                        }
                    }
                    this.Valido = false;
                }
            }
        }



    }
}
