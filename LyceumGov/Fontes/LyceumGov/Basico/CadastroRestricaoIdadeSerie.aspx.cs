namespace Techne.Lyceum.Net.Basico
{
    using System;
    using System.Web.UI.WebControls;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Web;
    using DevExpress.Web.ASPxGridView;

    [NavUrl("~/Basico/CadastroRestricaoIdadeSerie.aspx"), ControlText("Restrição Idade/Série"), Title("Restrição Idade/Série")]
    public partial class CadastroRestricaoIdadeSerie : TPage
    {
        public void Delete(object ID_RESTRICAO_IDADE_SERIE)
        {
        }

        public object Listar()
        {
            return RestricaoIdadeSerie.Listar();
        }

        public void Update(object ID_RESTRICAO_IDADE_SERIE, object MODALIDADE, object SEGMENTO, object CODIGO_CURSO, object NOME_CURSO, object SERIE, object IDADE_MINIMA, object IDADE_MAXIMA)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdRestricao, "Restrição Idade/Série");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
            this.ValidarCampos();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(grdRestricao);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            int resultado;
            var restricaoIdadeSerie = new TceRestricaoIdadeSerie
                                      {
                                          Curso = tseCurso.DBValue.ToString(),
                                          Matricula = this.User.Identity.Name,
                                          Serie = -1,
                                          IdadeMinima = -1,
                                          IdadeMaxima = -1
                                      };

            if (int.TryParse(this.cmbSerie.SelectedValue, out resultado))
            {
                restricaoIdadeSerie.Serie = resultado;
            }
            else
            {
                this.lblMensagem.Text = "O campo SÉRIE é OBRIGATÓRIO.";

                return;
            }

            if (int.TryParse(this.txtIdadeMinima.Text, out resultado))
            {
                restricaoIdadeSerie.IdadeMinima = resultado;
            }
            else
            {
                this.lblMensagem.Text = "Por favor, informe uma idade válida para o campo IDADE MÍNIMA.";

                return;
            }

            if (int.TryParse(this.txtIdadeMaxima.Text, out resultado))
            {
                restricaoIdadeSerie.IdadeMaxima = resultado;
            }
            else
            {
                this.lblMensagem.Text = "Por favor, informe uma idade válida para o campo IDADE MÁXIMA.";

                return;
            }

            var validacao = RestricaoIdadeSerie.Validar(restricaoIdadeSerie);

            if (validacao.Valido)
            {
                RestricaoIdadeSerie.Inserir(restricaoIdadeSerie);

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Restrição Idade/Série incluída com sucesso.');", true);

                this.LimparCampos();

                this.odsRestricao.Select();
                this.odsRestricao.DataBind();
                this.grdRestricao.DataBind();
            }
            else
            {
                this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }

        protected void tseModalidade_Changed(object sender, EventArgs e)
        {
            tseNivel.ResetValue();
            tseCurso.ResetValue();
            this.cmbSerie.Items.Clear();
            this.txtIdadeMaxima.Text = string.Empty;
            this.txtIdadeMinima.Text = string.Empty;
        }

        protected void tseNivel_Changed(object sender, EventArgs e)
        {
            tseCurso.ResetValue();
            tseCurso.Enabled = true;
            this.cmbSerie.Items.Clear();
            this.txtIdadeMaxima.Text = string.Empty;
            this.txtIdadeMinima.Text = string.Empty;
        }

        protected void tseCurso_Changed(object sender, EventArgs e)
        {
            this.cmbSerie.Items.Clear();
            this.txtIdadeMaxima.Text = string.Empty;
            this.txtIdadeMinima.Text = string.Empty;

            if (tseCurso.DBValue.ToString() != "Selecione")
            {
                this.cmbSerie.DataSource = Serie.ListarSeries(tseCurso.DBValue.ToString());
                this.cmbSerie.Items.Insert(0, "Selecione");
                this.cmbSerie.DataBind();
            }
        }

        protected void cmbSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtIdadeMaxima.Text = string.Empty;
            this.txtIdadeMinima.Text = string.Empty;
        }

        protected void grdRestricao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRestricao);
        }

        protected void odsRestricao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_RESTRICAO_IDADE_SERIE"].ToString();

            RestricaoIdadeSerie.Remover(int.Parse(id));
        }

        protected void odsRestricao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e.InputParameters["IDADE_MINIMA"] == null)
            {
                throw new Exception("O campo IDADE MÍNIMA é de preenchimento obrigatório.");
            }

            if (e.InputParameters["IDADE_MAXIMA"] == null)
            {
                throw new Exception("O campo IDADE MÁXIMA é de preenchimento obrigatório.");
            }

            if (int.Parse(e.InputParameters["IDADE_MINIMA"].ToString()) <= 0)
            {
                throw new Exception("O campo IDADE MÍNIMA é obrigatório e deve ser maior que zero!");
            }

            if (int.Parse(e.InputParameters["IDADE_MAXIMA"].ToString()) <= 0 || int.Parse(e.InputParameters["IDADE_MAXIMA"].ToString()) <= int.Parse(e.InputParameters["IDADE_MINIMA"].ToString()))
            {
                throw new Exception("O campo IDADE MÁXIMA é obrigatório e deve ser maior que a idade mínima!");
            }

            var restricaoIdadeSerie = new TceRestricaoIdadeSerie
                                      {
                                          IdRestricaoIdadeSerie = int.Parse(e.InputParameters["ID_RESTRICAO_IDADE_SERIE"].ToString()),
                                          IdadeMinima = int.Parse(e.InputParameters["IDADE_MINIMA"].ToString()),
                                          IdadeMaxima = int.Parse(e.InputParameters["IDADE_MAXIMA"].ToString()),
                                          Matricula = this.User.Identity.Name
                                      };

            RestricaoIdadeSerie.Alterar(restricaoIdadeSerie);
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            tseModalidade.ResetValue();
            tseNivel.ResetValue();
            tseCurso.ResetValue();
            this.cmbSerie.Items.Clear();
            this.txtIdadeMinima.Text = string.Empty;
            this.txtIdadeMaxima.Text = string.Empty;
        }

        private void ValidarCampos()
        {
            this.txtIdadeMaxima.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtIdadeMaxima.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            this.txtIdadeMinima.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtIdadeMinima.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }
    }
}