using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class TTableCell : TableCell
    {
        private ITControl control;

        public string ColumnName
        {
            get
            {
                if (this.control == null)
                {
                    throw new InvalidOperationException();
                }

                return this.control.ColumnName;
            }

            set
            {
                if (this.control == null)
                {
                    throw new InvalidOperationException();
                }

                this.control.ColumnName = value;
            }
        }

        protected internal void SetTControl(ITControl control)
        {
            if (this.control != null)
            {
                throw new InvalidOperationException("O controle já está setado.");
            }

            if (control != null)
            {
                this.control = control;
                this.Controls.Add((Control)control);
            }
        }

        protected override void LoadViewState(object savedState)
        {
        }

        protected override object SaveViewState()
        {
            return null;
        }
    }
}