using System;

namespace Techne.Lyceum.Net.Seguranca {
  public partial class Sair : TPage {
    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e) {
      InitializeComponent();
      base.OnInit(e);
    }
    
    private void InitializeComponent() {
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e) {
      TechneAuthentication.SignOut();
			Response.Redirect(Seguranca.Identificacao.GetUrl());
    }

  }
}
