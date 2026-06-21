using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DataGrid = System.Web.UI.WebControls.DataGrid;

namespace Techne.Controls.Design
{
    internal class DataColumnList : System.Windows.Forms.Form
    {
        private Button cmdCancel;

        private Button cmdOK;

        private Container components;

        private CheckedListBox lst;

        private string pvChecked = string.Empty;

        private DataGrid pvGrid;

        public DataColumnList()
        {
            this.InitializeComponent();
        }

        public string Checked
        {
            get
            {
                try
                {
                    var result = new ArrayList();
                    foreach (string column in this.lst.CheckedItems)
                    {
                        result.Add(column.Substring(0, column.IndexOf(":")));
                    }

                    return StrLib.EnumerableToStr(result, ", ");
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                this.pvChecked = value;
            }
        }

        public DataGrid Grid
        {
            get
            {
                return this.pvGrid;
            }

            set
            {
                this.pvGrid = value;
                if (this.pvGrid == null)
                {
                    return;
                }

                var columns = new ArrayList();
                int i;
                string columndesc;
                for (i = 0; i < this.pvGrid.Columns.Count; i++)
                {
                    if (this.pvGrid.Columns[i].HeaderText.Trim() != string.Empty)
                    {
                        columndesc = this.pvGrid.Columns[i].HeaderText;
                    }
                    else
                    {
                        if (this.pvGrid.Columns[i] is System.Web.UI.WebControls.BoundColumn)
                        {
                            columndesc = ((System.Web.UI.WebControls.BoundColumn)this.pvGrid.Columns[i]).DataField;
                        }
                        else if (this.pvGrid.Columns[i] is System.Web.UI.WebControls.ButtonColumn)
                        {
                            columndesc = ((System.Web.UI.WebControls.ButtonColumn)this.pvGrid.Columns[i]).DataTextField;
                        }
                        else if (this.pvGrid.Columns[i] is System.Web.UI.WebControls.HyperLinkColumn)
                        {
                            columndesc = ((System.Web.UI.WebControls.HyperLinkColumn)this.pvGrid.Columns[i]).DataTextField;
                        }
                        else if (this.pvGrid.Columns[i] is System.Web.UI.WebControls.TemplateColumn)
                        {
                            columndesc = "<Template>";
                        }
                        else
                        {
                            columndesc = string.Empty;
                        }
                    }

                    columns.Add(i + ": " + columndesc);
                }

                this.lst.DataSource = columns.ToArray(typeof (string));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void DataColumnList_Load(object sender, EventArgs e)
        {
            int index;
            var s = string.Empty + this.pvChecked;
            foreach (var column in s.Split(','))
            {
                index = this.lst.FindString(column.Trim() + ":");
                if (index >= 0)
                {
                    this.lst.SetItemChecked(index, true);
                }
            }
        }

        /// <summary>
        ///   Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lst = new CheckedListBox();
            this.cmdCancel = new Button();
            this.cmdOK = new Button();
            this.SuspendLayout();

// lst
            this.lst.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                               | System.Windows.Forms.AnchorStyles.Left)
                              | System.Windows.Forms.AnchorStyles.Right;
            this.lst.CheckOnClick = true;
            this.lst.IntegralHeight = false;
            this.lst.Name = "lst";
            this.lst.Size = new Size(200, 115);
            this.lst.TabIndex = 0;

// cmdCancel
            this.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new Point(77, 120);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(60, 20);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";

// cmdOK
            this.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new Point(140, 120);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new Size(60, 20);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";

// DataColumnList
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(200, 141);
            this.ControlBox = false;
            this.Controls.AddRange(new Control[]
                                   {
                                       this.cmdOK, 
                                       this.cmdCancel, 
                                       this.lst
                                   });
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataColumnList";
            this.Text = "Colunas Selecionáveis";
            this.Load += this.DataColumnList_Load;
            this.ResumeLayout(false);
        }
    }
}