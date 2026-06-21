using System;
using System.Reflection;
using System.Web;
using System.Web.UI;

using Techne.Web;

namespace Techne.Lyceum.Net.Seguranca {
  [
  NavUrl("~/Seguranca/Principal.aspx")
  ]
  public partial class Principal : TPage {
    protected System.Web.UI.WebControls.Label PageUser;
    protected System.Web.UI.WebControls.Label PageDate;

    public static string GetUrl() {
      #region Código gerado Techne
      return Navigation.GetNavigation(MethodInfo.GetCurrentMethod()).GetUrl(new object[] {  });
      #endregion
    }

    protected void bt_Entrar_Click(object sender, ImageClickEventArgs e) {
      Autentica(txtnumero_matricula.Text, txtsenha.Text);
    }

    private void SetPanelLoginVisibility(bool visible) {
      PanelLogin.Visible = visible;
      PanelPassword.Visible = !visible;
    }

    private void Autentica(string usuario, string senha) {
      string msg;
      TechneAuthenticationResult result = TechneAuthentication.WebAuthenticate(usuario, senha, "~/default.aspx", out msg);

      if(result == TechneAuthenticationResult.ChangePassword) {
        cMsg.Text = string.Empty;
        ViewState.Add("USUARIO.ID", usuario);
        lblUsuarioSenha.Text=usuario;
        SetPanelLoginVisibility(false);
        lblMsgAlteracao.Text = msg + "<br>Digite uma nova senha";
      }
      else
        cMsg.Text = msg;
    }
  

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e) {
      InitializeComponent();
      base.OnInit(e);
    }

    private void InitializeComponent() {

	}
    #endregion

    protected void ib_ok_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
      if(ViewState["USUARIO.ID"] == null)
        SetPanelLoginVisibility(true);

      else if(txtSenhaNova1.Text != txtSenhaNova2.Text)
        lblErroSenha.Text = "As senhas digitadas não conferem";

      else {
        string msg;
        TechneAuthentication.ChangePassword(txtnumero_matricula.Text, txtSenhaAtual.Text, txtSenhaNova1.Text, out msg);
        if(msg != null)
          lblErroSenha.Text = msg;
        else
          Autentica(txtnumero_matricula.Text, txtSenhaNova1.Text);
      }

    }

    protected void ib_cancelar_Click(object sender, System.Web.UI.ImageClickEventArgs e) {
      SetPanelLoginVisibility(true);
    }

  	protected void Page_Load(object sender, System.EventArgs e)
	  {
	  
	  }

  }
}
