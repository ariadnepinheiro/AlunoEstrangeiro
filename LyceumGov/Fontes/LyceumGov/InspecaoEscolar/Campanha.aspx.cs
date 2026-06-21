using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;



namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/Campanha.aspx"),
  ControlText("Campanha"),
  Title("Campanha"),]
    public partial class Campanha : TPage
    {
        RN.InspecaoEscolar.Campanha campanhaRN = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCampanha, string.Empty);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCampanha);
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdCampanha, AcaoControle.editar, "Editar");
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this._tipoOperacao = TipoOperacao.Cancelar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this._tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
                carregarAno();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.InspecaoEscolar.Entidades.Campanha campanhaDados = new RN.InspecaoEscolar.Entidades.Campanha();
                
                //preenche a entidade

                campanhaDados.Semestre = ddlperiodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlperiodo.SelectedValue);
                campanhaDados.Ano = ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlAno.SelectedValue);
                campanhaDados.Objetivo = txtObjetivo.Text;
                campanhaDados.Procedimento = txtProcedimento.Text;
                campanhaDados.Titulo = txtTitulo.Text;
                campanhaDados.ExibeInspecaoEscolar = !rblExibeInspecaoEscolar.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToBoolean(rblExibeInspecaoEscolar.SelectedValue) : (bool?)null;
                campanhaDados.UsuarioId = User.Identity.Name;
                campanhaDados.CampanhaId = HiddenID.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(HiddenID.Value);

                //chama o valida e verifica os erros
                var validacao = campanhaRN.Valida(campanhaDados);

                if (validacao.Valido) //validações foram OK
                {
                    if (campanhaDados.CampanhaId == 0)//inclusão
                    {
                        campanhaRN.Insere(campanhaDados);
                        this._tipoOperacao = TipoOperacao.Sucesso;
                        ControlarTipoOperacao();


                    }
                    else if (campanhaDados.CampanhaId != 0)//update
                    {
                        campanhaRN.Atualiza(campanhaDados);
                        this._tipoOperacao = TipoOperacao.Sucesso;
                        ControlarTipoOperacao();
                    }

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                }
            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }
        }

        public void Deletar(object CAMPANHAID)
        {


        }
        public object ListarCampanha()
        {
            RN.InspecaoEscolar.Campanha ListarCampanha = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();


            return ListarCampanha.ListarCampanha();

        }

        public void carregarAno()
        {
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", "0"));
        }

        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Inicial,
            Consultar,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pnlCampanha.Visible = false;
                        LimparTela();
                        break;

                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        grdCampanha.DataBind();
                        LimparTela();
                        pnlCampanha.Visible = false;
                        lblMensagem.Text = "Operação executada com Sucesso.";
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        pnlCampanha.Visible = true;
                        LimparTela();

                        break;
                    }
             
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };

                        ControlarVisibilidadeControle(controles);
                        pnlCampanha.Visible = false;
                        LimparTela();
                        grdCampanha.DataBind();
                        break;
                    }
            }
        }

        private void LimparTela()
        {
            ddlAno.ClearSelection();
            ddlperiodo.ClearSelection();
            txtObjetivo.Text = string.Empty;
            txtProcedimento.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            rblExibeInspecaoEscolar.ClearSelection();
            HiddenID.Value = null;
        
        }

        protected void grdCampanha_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCampanha);
        }

        protected void grdCampanha_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            if (e.ButtonID == "Editar")
            {
                this._tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
                carregarAno();

                string objetivo = Convert.ToString(grdCampanha.GetRowValues(e.VisibleIndex, "OBJETIVO"));
                string procedimento = Convert.ToString(grdCampanha.GetRowValues(e.VisibleIndex, "PROCEDIMENTO"));
                string titulo = Convert.ToString(grdCampanha.GetRowValues(e.VisibleIndex, "TITULO"));
                string exibeInspecaoEscolar = Convert.ToString(grdCampanha.GetRowValues(e.VisibleIndex, "EXIBEINSPECAOESCOLAR"));
                int ano = Convert.ToInt32(grdCampanha.GetRowValues(e.VisibleIndex, "ANO"));
                int semestre = Convert.ToInt32(grdCampanha.GetRowValues(e.VisibleIndex, "SEMESTRE"));
                int id = Convert.ToInt32(grdCampanha.GetRowValues(e.VisibleIndex, "CAMPANHAID"));

                txtObjetivo.Text = objetivo;
                txtProcedimento.Text = procedimento;
                txtTitulo.Text = titulo;
                rblExibeInspecaoEscolar.SelectedValue = exibeInspecaoEscolar.ToUpper() == "SIM" ? "true" : "false";
                ddlperiodo.SelectedValue = semestre.ToString();
                ddlAno.SelectedValue = ano.ToString();
                HiddenID.Value = id.ToString();
            }

            if (e.ButtonID == "Deletar")
            {
                int id = Convert.ToInt32(grdCampanha.GetRowValues(e.VisibleIndex, "CAMPANHAID"));
                var validacao = campanhaRN.ValidaRemocao(id);


                if (validacao.Valido) //validações foram OK
                {
                    //Remove
                    campanhaRN.Remove(id);
                    this._tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
            }

        }

        protected void grdCampanha_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            try
            {
                RN.InspecaoEscolar.Campanha rnInspecaoEscolar = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();
                int id = 0;

                id = Convert.ToInt32(e.Keys["CAMPANHAID"]);

                validacao = rnInspecaoEscolar.ValidaRemocao(id);

                if (validacao.Valido)
                {
                    rnInspecaoEscolar.Remove(id);
                    grdCampanha.DataBind();
                }
                else
                {
                    e.Cancel = true;
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
