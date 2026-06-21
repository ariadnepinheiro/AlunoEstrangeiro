using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Techne
{
    internal class InputBox : Form
    {
        private readonly bool mandatory;

        private Button cmdCancel;

        private Button cmdOk;

        private Container components;

        private Label lblMessage;

        private TextBox txt;

        private InputBox(string message, string title, 
                         string initialValue, bool mandatory)
        {
            this.InitializeComponent();

            this.Text = title;
            this.lblMessage.Text = message;
            this.txt.Text = initialValue;
            this.cmdOk.Enabled = !mandatory || (message != null && message.Length > 0);
            this.mandatory = mandatory;
        }

        /// <summary>
        ///   Mostra formulário modal contendo um campo onde o usuário digita um valor.
        ///   Valores vazios (string vazia ou somente espaços) serão permitidos.
        /// </summary>
        /// <param name = "owner">Janela que conterá o formulário modal.</param>
        /// <param name = "message">Mensagem mostrada sobre o campo contendo instruções ao usuário (Ex: "Digite um valor").</param>
        /// <param name = "title">Texto mostrado no título do formulário.</param>
        /// <param name = "initialValue">Valor default mostrado inicialmente no campo.</param>
        public static string Show(IWin32Window owner, 
                                  string message, string title, 
                                  string initialValue)
        {
            return Show(owner, message, title, initialValue, false);
        }

        /// <summary>
        ///   Mostra formulário modal contendo um campo onde o usuário digita um valor.
        /// </summary>
        /// <param name = "owner">Janela que conterá o formulário modal.</param>
        /// <param name = "message">Mensagem mostrada sobre o campo contendo instruções ao usuário (Ex: "Digite um valor").</param>
        /// <param name = "title">Texto mostrado no título do formulário.</param>
        /// <param name = "initialValue">Valor default mostrado inicialmente no campo.</param>
        /// <param name = "mandatory">
        ///   Indica se valores vazios (string vazia ou somente espaços) será permitido.
        ///   O bloqueio é feito desabilitando-se o botão Ok.
        /// </param>
        public static string Show(IWin32Window owner, 
                                  string message, string title, 
                                  string initialValue, bool mandatory)
        {
            var frm = new InputBox(message, title, initialValue, mandatory);
            var result = frm.ShowDialog(owner);
            if (result == DialogResult.Cancel)
            {
                return null;
            }
            else
            {
                return frm.txt.Text;
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

        private void InitializeComponent()
        {
            this.lblMessage = new Label();
            this.txt = new TextBox();
            this.cmdOk = new Button();
            this.cmdCancel = new Button();
            this.SuspendLayout();

// lblMessage
            this.lblMessage.Anchor = ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                        | System.Windows.Forms.AnchorStyles.Left)
                                       | System.Windows.Forms.AnchorStyles.Right));
            this.lblMessage.Location = new Point(8, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(304, 56);
            this.lblMessage.TabIndex = 0;

// txt
            this.txt.Anchor = (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right));
            this.txt.Location = new Point(8, 88);
            this.txt.Name = "txt";
            this.txt.Size = new Size(304, 20);
            this.txt.TabIndex = 1;
            this.txt.Text = string.Empty;
            this.txt.TextChanged += this.txt_TextChanged;

// cmdOk
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Location = new Point(328, 8);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.TabIndex = 2;
            this.cmdOk.Text = "&Ok";

// cmdCancel
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new Point(328, 40);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "&Cancel";

// InputBox
            this.AcceptButton = this.cmdOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new Size(408, 119);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            this.cmdOk.Enabled = !this.mandatory || this.txt.Text.Trim().Length > 0;
        }
    }
}