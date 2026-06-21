using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.WebControls;

namespace Techne.Web
{
    public class TSearchColumn
    {
        private string _caption = string.Empty;

        public TSearchColumn()
        {
            this.Visible = true;
        }

        public TSearchColumn(string fieldName, string caption)
        {
            this.FieldName = fieldName;
            this.Caption = caption;
            this.Visible = true;
            this.Width = Unit.Empty;
        }

        public TSearchColumn(string fieldName, string caption, bool visible, Unit width)
        {
            this.FieldName = fieldName;
            this.Caption = caption;
            this.Visible = visible;
            this.Width = width;
        }

        [Category("Data"), Localizable(false), DefaultValue(""), NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint), Description("Nome da coluna")]
        public virtual string FieldName { get; set; }

        [DefaultValue(""), Category("Appearance"), RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), Localizable(true), Description("Texto do cabe\x00e7alho da coluna")]
        public string Caption
        {
            get
            {
                return this._caption;
            }

            set
            {
                this._caption = value == null ? string.Empty : value;
            }
        }

        [NotifyParentProperty(true)]
        [Description("Especifica se a coluna é visível ")]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        [NotifyParentProperty(true), DefaultValue(typeof (Unit), ""), Description("Largura da coluna"), Category("Appearance")]
        public Unit Width { get; set; }
    }

    public class TSearchColumnCollection : List<TSearchColumn>
    {
    }

    public class TSearchColumnCollectionEditor : CollectionEditor
    {
        public TSearchColumnCollectionEditor(Type type) : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof (TSearchColumn);
        }
    }
}