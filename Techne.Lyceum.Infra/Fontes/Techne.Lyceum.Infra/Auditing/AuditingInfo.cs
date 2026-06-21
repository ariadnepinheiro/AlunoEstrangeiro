using System;

namespace Techne.Auditing
{
    public struct AuditingInfo
    {
        private readonly bool audita;

        private readonly string pagina;

        private readonly string usuario;

        public AuditingInfo(string usuario, string pagina, bool audita)
        {
            if (usuario == null || usuario.Trim().Length == 0)
            {
                throw new ArgumentException("Usuário não informado.", "usuario");
            }

            if (pagina == null || pagina.Trim().Length == 0)
            {
                throw new ArgumentException("Página não informada.", "pagina");
            }

            this.pagina = pagina;
            this.usuario = usuario;
            this.audita = audita;
        }

        public bool Audita
        {
            get
            {
                return this.audita;
            }
        }

        public string Pagina
        {
            get
            {
                return this.pagina;
            }
        }

        public string Usuario
        {
            get
            {
                return this.usuario;
            }
        }
    }
}