using System;

namespace Techne.Web
{
    public class HdUsuario
    {
        private string _email = string.Empty;

        private string _nome = string.Empty;

        private string _usuario = string.Empty;

        public string Email
        {
            get
            {
                return this._email;
            }

            set
            {
                this._email = value == null ? string.Empty : value;
            }
        }

        public bool Habilitado { get; set; }

        public string Nome
        {
            get
            {
                return this._nome;
            }

            set
            {
                this._nome = value == null ? string.Empty : value;
            }
        }

        public DateTime? UltimoLogin { get; set; }

        public string Usuario
        {
            get
            {
                return this._usuario;
            }

            set
            {
                this._usuario = value == null ? string.Empty : value;
            }
        }
    }
}