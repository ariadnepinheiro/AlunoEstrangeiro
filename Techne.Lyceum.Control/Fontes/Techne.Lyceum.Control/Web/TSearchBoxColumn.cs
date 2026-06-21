using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI.WebControls;

namespace Techne.Web
{
    public class TSearchBoxColumn
    {
        private string _caption = string.Empty;

        public TSearchBoxColumn()
        {
            this.Visible = true;
        }

        [RefreshProperties(RefreshProperties.Repaint), Localizable(false), Description("Gets or sets the name of the database field assigned to the current column."), NotifyParentProperty(true), DefaultValue(""), Category("Data")]
        public virtual string FieldName { get; set; }

        [Category("Appearance"), Description("Gets or sets the text displayed within the column header."), NotifyParentProperty(true), Localizable(true), DefaultValue(""), RefreshProperties(RefreshProperties.Repaint)]
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

        [Description("Gets or sets the column's programmatic identifier. "), Browsable(false), Category("Behavior"), DefaultValue(""), Localizable(false), NotifyParentProperty(true)]
        public string Name
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

        // [Category("Appearance")]
        // [DefaultValue("")]
        // [Description("Gets or sets the column header's tooltip text. ")]
        // [NotifyParentProperty(true)]
        // [Localizable(true)]
        // public string ToolTip { get; set; }
        [NotifyParentProperty(true)]
        [Description("Gets or sets a value that specifies whether the column is visible.")]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        [Category("Appearance"), Description("Gets or sets the column's width. "), DefaultValue(typeof (Unit), ""), NotifyParentProperty(true)]
        public Unit Width { get; set; }
    }

    public class TSearchBoxColumnCollection : List<TSearchBoxColumn>
    {
    }

    public class TSearchBoxColumnCollectionEditor : CollectionEditor
    {
        public TSearchBoxColumnCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof (TSearchBoxColumn);
        }
    }
}