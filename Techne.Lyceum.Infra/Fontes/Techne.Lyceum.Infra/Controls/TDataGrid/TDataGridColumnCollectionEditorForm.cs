using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using Label = System.Windows.Forms.Label;

namespace Techne.Controls
{
    internal class TDataGridColumnCollectionEditorForm : Form
    {
        private readonly DataGridColumnCollection pvColumns;

        private Button bAdd;

        private Button bAddMenu;

        private Button bCancel;

        private Button bDec;

        private Button bInc;

        private Button bOK;

        private Button bRemove;

        private ColumnHeader column;

        private IContainer components;

        private GroupBox groupBox1;

        private ImageList imgColumns;

        private Label label1;

        private Label label2;

        private ListView lvColumns;

        private ContextMenu mnuColumns;

        private ColumnHeader ord;

        private PropertyGrid pgColumn;

        private StatusBarPanel statusBarPanel1;

        public TDataGridColumnCollectionEditorForm(DataGridColumnCollection columns)
        {
            this.pvColumns = columns;
            this.InitializeComponent();
        }

        private Type[] itemTypes
        {
            get
            {
                return new[]
                       {
                           typeof (ButtonColumn), 
                           typeof (CheckBoxColumn), 
                           typeof (DropDownListColumn), 
                           typeof (HyperLinkColumn), 
                           typeof (LinkMethodColumn), 
                           typeof (MarkColumn), 
                           typeof (TextBoxColumn), 
                           null, 
                           typeof (TemplateColumn), 
                       };
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.PopulateListView();
            this.FillMenu();
        }

        private void AddItem(Type t)
        {
            if (this.lvColumns.Items.Count > 99)
            {
                return;
            }

            var obj = System.Activator.CreateInstance(t);
            var item = this.lvColumns.Items.Add(this.lvColumns.Items.Count.ToString("00"));
            item.SubItems.Add(this.ColumnText((DataGridColumn)obj));
            item.ImageIndex = 0;
            item.ImageIndex = this.ColumnImageIndex(t);
            item.Tag = obj;
        }

        private int ColumnImageIndex(Type t)
        {
            for (var i = 0; i < this.itemTypes.Length; i++)
            {
                if (this.itemTypes[i] == t)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private string ColumnText(DataGridColumn column)
        {
            var flags = BindingFlags.InvokeMethod | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.IgnoreCase;

            // Case 1. Neither argument coercion nor member selection is needed.
            if (column.HeaderText != null && column.HeaderText.Trim() != string.Empty && column.HeaderText != "?")
            {
                return column.HeaderText;
            }
            else
            {
                var memberName = column is TGridColumn ? "ColumnName" : "DataField";
                object[] members = column.GetType().GetMember(memberName, flags);
                object result = null;

                if (members.Length == 1)
                {
                    result = column.GetType().InvokeMember(memberName, flags, null, column, new object[0]);
                }

                if (result is string && ((string)result).Trim() != string.Empty)
                {
                    return (string)result;
                }
                else
                {
                    return column.GetType().Name;
                }
            }
        }

        private void FillMenu()
        {
            this.mnuColumns.MenuItems.Clear();

            for (var i = 0; i < this.itemTypes.Length; i++)
            {
                if (this.itemTypes[i] == null)
                {
                    this.mnuColumns.MenuItems.Add("-");
                }
                else
                {
                    this.mnuColumns.MenuItems.Add(this.itemTypes[i].Name, this.mnuColumns_Click);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            var listViewItem1 = new ListViewItem(new[]
                                                 {
                                                     new ListViewItem.ListViewSubItem(null, "Hi", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.ControlLight, new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (0))), 
                                                     new ListViewItem.ListViewSubItem(null, "Hey", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (0)))
                                                 }, -1);
            var resources = new ResourceManager(typeof (TDataGridColumnCollectionEditorForm));
            this.pgColumn = new PropertyGrid();
            this.statusBarPanel1 = new StatusBarPanel();
            this.lvColumns = new ListView();
            this.ord = new ColumnHeader();
            this.column = new ColumnHeader();
            this.imgColumns = new ImageList(this.components);
            this.label1 = new Label();
            this.label2 = new Label();
            this.bAdd = new Button();
            this.bRemove = new Button();
            this.bAddMenu = new Button();
            this.mnuColumns = new ContextMenu();
            this.groupBox1 = new GroupBox();
            this.bOK = new Button();
            this.bCancel = new Button();
            this.bDec = new Button();
            this.bInc = new Button();
            ((System.ComponentModel.ISupportInitialize)this.statusBarPanel1).BeginInit();
            this.SuspendLayout();

// pgColumn
            this.pgColumn.Anchor = ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                      | System.Windows.Forms.AnchorStyles.Left)
                                     | System.Windows.Forms.AnchorStyles.Right));
            this.pgColumn.CommandsVisibleIfAvailable = true;
            this.pgColumn.LargeButtons = false;
            this.pgColumn.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pgColumn.Location = new Point(240, 24);
            this.pgColumn.Name = "pgColumn";
            this.pgColumn.Size = new Size(300, 308);
            this.pgColumn.TabIndex = 0;
            this.pgColumn.Text = "propertyGrid1";
            this.pgColumn.ViewBackColor = System.Drawing.SystemColors.Window;
            this.pgColumn.ViewForeColor = System.Drawing.SystemColors.WindowText;
            this.pgColumn.PropertyValueChanged += this.pgColumn_PropertyValueChanged;

// statusBarPanel1
            this.statusBarPanel1.Text = "statusBarPanel1";

// lvColumns
            this.lvColumns.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvColumns.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                      | System.Windows.Forms.AnchorStyles.Left));
            this.lvColumns.Columns.AddRange(new[]
                                            {
                                                this.ord, 
                                                this.column
                                            });
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvColumns.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.UseItemStyleForSubItems = false;
            this.lvColumns.Items.AddRange(new[]
                                          {
                                              listViewItem1
                                          });
            this.lvColumns.Location = new Point(8, 24);
            this.lvColumns.MultiSelect = false;
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new Size(192, 276);
            this.lvColumns.SmallImageList = this.imgColumns;
            this.lvColumns.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvColumns.TabIndex = 1;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.SelectedIndexChanged += this.lvColumns_SelectedIndexChanged;

// ord
            this.ord.Width = 18;

// column
            this.column.Width = 168;

// imgColumns
            this.imgColumns.ImageSize = new Size(16, 16);
            this.imgColumns.ImageStream = (System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgColumns.ImageStream"));
            this.imgColumns.TransparentColor = System.Drawing.Color.Transparent;

// label1
            this.label1.Location = new Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(100, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Columns";

// label2
            this.label2.Location = new Point(240, 4);
            this.label2.Name = "label2";
            this.label2.Size = new Size(100, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Properties:";

// bAdd
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.bAdd.Location = new Point(8, 308);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new Size(76, 24);
            this.bAdd.TabIndex = 4;
            this.bAdd.Text = "Add";
            this.bAdd.Click += this.bAdd_Click;

// bRemove
            this.bRemove.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.bRemove.Location = new Point(116, 308);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new Size(84, 24);
            this.bRemove.TabIndex = 5;
            this.bRemove.Text = "Remove";
            this.bRemove.Click += this.bRemove_Click;

// bAddMenu
            this.bAddMenu.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.bAddMenu.Image = (System.Drawing.Image)(resources.GetObject("bAddMenu.Image"));
            this.bAddMenu.Location = new Point(84, 308);
            this.bAddMenu.Name = "bAddMenu";
            this.bAddMenu.Size = new Size(12, 24);
            this.bAddMenu.TabIndex = 6;
            this.bAddMenu.MouseDown += this.bAddMenu_MouseDown;

// groupBox1
            this.groupBox1.Anchor = (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                      | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox1.Location = new Point(8, 340);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(532, 4);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;

// bOK
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new Point(384, 356);
            this.bOK.Name = "bOK";
            this.bOK.Size = new Size(68, 24);
            this.bOK.TabIndex = 8;
            this.bOK.Text = "OK";
            this.bOK.Click += this.bOK_Click;

// bCancel
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new Point(464, 356);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new Size(72, 24);
            this.bCancel.TabIndex = 9;
            this.bCancel.Text = "Cancel";
            this.bCancel.Click += this.bCancel_Click;

// bDec
            this.bDec.Image = (System.Drawing.Image)(resources.GetObject("bDec.Image"));
            this.bDec.Location = new Point(204, 24);
            this.bDec.Name = "bDec";
            this.bDec.Size = new Size(20, 24);
            this.bDec.TabIndex = 10;
            this.bDec.Click += this.bDec_Click;

// bInc
            this.bInc.Image = (System.Drawing.Image)(resources.GetObject("bInc.Image"));
            this.bInc.Location = new Point(204, 56);
            this.bInc.Name = "bInc";
            this.bInc.Size = new Size(20, 24);
            this.bInc.TabIndex = 11;
            this.bInc.Click += this.bInc_Click;

// TDataGridColumnCollectionEditorForm
            this.AcceptButton = this.bOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.CancelButton = this.bCancel;
            this.ClientSize = new Size(548, 393);
            this.ControlBox = false;
            this.Controls.Add(this.bInc);
            this.Controls.Add(this.bDec);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bAddMenu);
            this.Controls.Add(this.bRemove);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvColumns);
            this.Controls.Add(this.pgColumn);
            this.Name = "TDataGridColumnCollectionEditorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editor de Colunas Techne";
            ((System.ComponentModel.ISupportInitialize)this.statusBarPanel1).EndInit();
            this.ResumeLayout(false);
        }

        private void PopulateCollection()
        {
            this.pvColumns.Clear();
            for (var i = 0; i < this.lvColumns.Items.Count; i++)
            {
                this.pvColumns.Add((DataGridColumn)this.lvColumns.Items[i].Tag);
            }
        }

        private void PopulateListView()
        {
            this.lvColumns.Items.Clear();

            for (var i = 0; i < this.pvColumns.Count; i++)
            {
                var item = this.lvColumns.Items.Add(i.ToString("00"));
                item.SubItems.Add(this.ColumnText(this.pvColumns[i]));
                item.ImageIndex = this.ColumnImageIndex(this.pvColumns[i].GetType());
                item.Tag = this.pvColumns[i];
            }
        }

        private void bAddMenu_MouseDown(object sender, MouseEventArgs e)
        {
            this.mnuColumns.Show(this.bAdd, new Point(0, this.bAdd.Height));
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            this.AddItem(typeof (TextBoxColumn));
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bDec_Click(object sender, EventArgs e)
        {
            if (this.lvColumns.SelectedItems.Count != 1 || this.lvColumns.SelectedItems[0].Index <= 0)
            {
                return;
            }

            var i = this.lvColumns.SelectedItems[0].Index;
            var item = this.lvColumns.Items[i];
            var itemant = this.lvColumns.Items[i - 1];
            itemant.SubItems[0].Text = i.ToString("00");
            item.SubItems[0].Text = (i - 1).ToString("00");
            this.lvColumns.Sort();
        }

        private void bInc_Click(object sender, EventArgs e)
        {
            if (this.lvColumns.SelectedItems.Count != 1 || this.lvColumns.SelectedItems[0].Index >= this.lvColumns.Items.Count - 1)
            {
                return;
            }

            var i = this.lvColumns.SelectedItems[0].Index;
            var item = this.lvColumns.Items[i];
            var itempos = this.lvColumns.Items[i + 1];
            itempos.SubItems[0].Text = i.ToString("00");
            item.SubItems[0].Text = (i + 1).ToString("00");
            this.lvColumns.Sort();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.PopulateCollection();
            this.Close();
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (this.lvColumns.SelectedItems.Count != 1)
            {
                return;
            }

            var pos = this.lvColumns.SelectedItems[0].Index;
            this.lvColumns.Items[pos].Remove();

            for (var i = 0; i < this.lvColumns.Items.Count; i++)
            {
                this.lvColumns.Items[i].SubItems[0].Text = i.ToString("00");
            }

            if (this.lvColumns.Items.Count > 0)
            {
                if (pos >= this.lvColumns.Items.Count)
                {
                    pos = this.lvColumns.Items.Count - 1;
                }
                else if (pos < 0)
                {
                    pos = 0;
                }

                this.lvColumns.Items[pos].Selected = true;
            }
        }

        private void lvColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pgColumn.SelectedObject = null;
            if (this.lvColumns.SelectedItems.Count > 0)
            {
                this.pgColumn.SelectedObject = this.lvColumns.Items[this.lvColumns.SelectedItems[0].Index].Tag;
            }
        }

        private void mnuColumns_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < this.itemTypes.Length; i++)
            {
                if (this.itemTypes[i] != null && ((System.Windows.Forms.MenuItem)sender).Text == this.itemTypes[i].Name)
                {
                    this.AddItem(this.itemTypes[i]);
                    break;
                }
            }
        }

        private void pgColumn_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (string.Compare(e.ChangedItem.Label, "DataField", true) == 0 ||
                string.Compare(e.ChangedItem.Label, "HeaderText", true) == 0)
            {
                this.lvColumns.SelectedItems[0].SubItems[1].Text = this.ColumnText((DataGridColumn)this.pgColumn.SelectedObject);
            }
        }
    }
}