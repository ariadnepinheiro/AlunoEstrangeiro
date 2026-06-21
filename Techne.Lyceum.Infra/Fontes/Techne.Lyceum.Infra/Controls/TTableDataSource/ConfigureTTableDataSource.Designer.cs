namespace Techne.Controls
{
  partial class ConfigureTTableDataSource
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.ddlTypes = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.ddlMethods = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.chkBusinessOnly = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // ddlTypes
      // 
      this.ddlTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ddlTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ddlTypes.FormattingEnabled = true;
      this.ddlTypes.Location = new System.Drawing.Point(12, 32);
      this.ddlTypes.Name = "ddlTypes";
      this.ddlTypes.Size = new System.Drawing.Size(393, 21);
      this.ddlTypes.TabIndex = 0;
      this.ddlTypes.SelectedIndexChanged += new System.EventHandler(this.ddlTypes_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(38, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Classe";
      // 
      // button1
      // 
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(125, 145);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(71, 26);
      this.button1.TabIndex = 2;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(202, 145);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(71, 26);
      this.button2.TabIndex = 3;
      this.button2.Text = "Cancelar";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // ddlMethods
      // 
      this.ddlMethods.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.ddlMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.ddlMethods.FormattingEnabled = true;
      this.ddlMethods.Location = new System.Drawing.Point(12, 79);
      this.ddlMethods.Name = "ddlMethods";
      this.ddlMethods.Size = new System.Drawing.Size(393, 21);
      this.ddlMethods.TabIndex = 4;
      this.ddlMethods.SelectedIndexChanged += new System.EventHandler(this.ddlMethods_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 63);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(43, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Método";
      // 
      // chkBusinessOnly
      // 
      this.chkBusinessOnly.AutoSize = true;
      this.chkBusinessOnly.Checked = true;
      this.chkBusinessOnly.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkBusinessOnly.Location = new System.Drawing.Point(12, 117);
      this.chkBusinessOnly.Name = "chkBusinessOnly";
      this.chkBusinessOnly.Size = new System.Drawing.Size(102, 17);
      this.chkBusinessOnly.TabIndex = 6;
      this.chkBusinessOnly.Text = "Somente do RN";
      this.chkBusinessOnly.UseVisualStyleBackColor = true;
      this.chkBusinessOnly.CheckedChanged += new System.EventHandler(this.chkBusinessOnly_CheckedChanged);
      // 
      // ConfigureTTableDataSource
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(417, 183);
      this.Controls.Add(this.chkBusinessOnly);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.ddlMethods);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ddlTypes);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ConfigureTTableDataSource";
      this.Text = "Selecione o Método de Consulta";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox ddlTypes;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.ComboBox ddlMethods;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox chkBusinessOnly;
  }
}