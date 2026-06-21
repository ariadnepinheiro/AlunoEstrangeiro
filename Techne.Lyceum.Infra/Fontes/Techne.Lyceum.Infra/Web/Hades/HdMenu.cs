using System.Collections.Generic;

namespace Techne.Web.Hades
{
    internal class HdMenu
    {
        private readonly List<HdMenuItem> _items = new List<HdMenuItem>();

        private string _descricao = string.Empty;

        private string _nomeMenu = string.Empty;

        private string _sistema = string.Empty;

        public string Descricao
        {
            get
            {
                return this._descricao;
            }

            set
            {
                this._descricao = value == null ? string.Empty : value;
            }
        }

        public List<HdMenuItem> Items
        {
            get
            {
                return this._items;
            }
        }

        public string NomeMenu
        {
            get
            {
                return this._nomeMenu;
            }

            set
            {
                this._nomeMenu = value == null ? string.Empty : value;
            }
        }

        public string Sistema
        {
            get
            {
                return this._sistema;
            }

            set
            {
                this._sistema = value == null ? string.Empty : value;
            }
        }
    }

    internal class HdMenuItem
    {
        public virtual string ID { get; set; }

        public virtual string ParentID { get; set; }

        public string ImageUrl { get; set; }

        public string[] Roles { get; set; }

        public string SisUrl { get; set; }

        public string Sistema { get; set; }

        public string Texto { get; set; }

        public string Trans { get; set; }

        public string Url { get; set; }
    }
}