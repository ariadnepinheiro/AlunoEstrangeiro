using System;
using System.Configuration;
using System.Web.Security;

namespace Techne.Lyceum.Net.Modulos
{
    public partial class ProcessoSeletivoMaster : System.Web.UI.MasterPage
    {
        public bool ExibirMensagemModal
        {
            get
            {
                return ppcMensagemCompatilidadeIE.ShowOnPageLoad;
            }
            set
            {
                ppcMensagemCompatilidadeIE.ShowOnPageLoad = value;
                ppcMensagemCompatilidadeIE.Enabled = value;
            }
        }
        
        public string MensagemModal
        {
            get
            {
                return lblMensagem.Text;
            }
            set
            {
                lblMensagem.Text = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ppcMensagemCompatilidadeIE.ShowOnPageLoad = false;
            AtribuirMensagemCompatibilidadeIE(MensagemModal);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hlHelp.Attributes.Add("OnClick", "__Help(); return(false);");
            hlHelp.Style.Add("cursor", "pointer");

            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;

            lblVersao.Text = versao + sufixo;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            ppcMensagemCompatilidadeIE.ShowOnPageLoad = false;
        }

        public virtual void AtribuirMensagemCompatibilidadeIE(string mensagem)
        {
            lblMensagem.Text = mensagem;
        }

        public virtual void AtribuirModalOKClick(EventHandler modalOKClick)
        {
            btnOk.Click += modalOKClick;
        }       
    }
}