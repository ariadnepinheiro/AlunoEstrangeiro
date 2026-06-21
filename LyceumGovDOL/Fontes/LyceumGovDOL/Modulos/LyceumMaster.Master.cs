namespace Techne.Lyceum.Net.Modulos
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DevExpress.Web.ASPxEditors;
    using DevExpress.Web.ASPxGridView;
    using Techne.Lyceum.RN;

    public partial class LyceumMaster : MasterPage
    {
        private const string AvaliacaoCurriculoMinimo = "~/Academico/AvaliacaoCurriculoMinimo.aspx";

        private const string CurriculoMinimo = "~/Academico/CurriculoMinimo.aspx";

        private const string DisponibilidadeGLP = "~/Academico/DisponibilidadeGlp.aspx";

        private const string NotasTurma = "~/Academico/NotasTurma.aspx";

        private const string Protocolos = "~/Academico/Protocolos.aspx";

        private const string Config = "Config.aspx";

        public bool HabilitaLoading { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            TPage.TituloGrid(this.grdTurmaDisciplina, "Disciplinas do Docente");

            if (!this.IsPostBack)
            {
                if (this.Session["txtNomeUnidadeEns"] != null)
                {
                    this.txtNomeUnidadeEns.Value = this.Session["txtNomeUnidadeEns"].ToString();
                    this.lblUnidadeEns.Visible = true;
                }
                else
                {
                    this.pcPesquisa.ShowOnPageLoad = true;
                }

                if (this.Session["txtCodUnidadeEns"] != null)
                {
                    this.txtCodUnidadeEns.Value = this.Session["txtCodUnidadeEns"].ToString();
                }

                if (this.Session["txtAno"] != null)
                {
                    this.txtAno.Value = this.Session["txtAno"].ToString();
                    this.lblAno.Visible = true;
                }

                if (this.Session["txtTurma"] != null)
                {
                    this.txtTurma.Value = this.Session["txtTurma"].ToString();
                    this.lblTurma.Visible = true;
                }

                if (this.Session["txtPeriodo"] != null)
                {
                    this.txtPeriodo.Value = this.Session["txtPeriodo"].ToString();
                    this.lblPeriodo.Visible = true;
                }

                if (this.Session["txtNomeDisciplina"] != null)
                {
                    this.txtNomeDisciplina.Value = this.Session["txtNomeDisciplina"].ToString();
                    this.lblDisciplina.Visible = true;
                }

                if (this.Session["txtDisciplina"] != null)
                {
                    this.txtDisciplina.Value = this.Session["txtDisciplina"].ToString();
                }

                if (this.Session["txtMatricula"] != null)
                {
                    this.txtMatriculaProf.Value = this.Session["txtMatricula"].ToString();
                }

                if (this.Session["txtNomeProfessor"] != null)
                {
                    this.txtNomeProf.Value = this.Session["txtNomeProfessor"].ToString();
                }

                if (this.Session["txtCurso"] != null)
                {
                    this.txtCurso.Value = this.Session["txtCurso"].ToString();
                }

                if (this.Session["txtModalidade"] != null)
                {
                    this.txtModalidade.Value = this.Session["txtModalidade"].ToString();
                }

                if (this.Session["txtTipoCurso"] != null)
                {
                    this.txtTipoCurso.Value = this.Session["txtTipoCurso"].ToString();
                }

                if (this.Session["txtSerie"] != null)
                {
                    this.txtSerie.Value = this.Session["txtSerie"].ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.hlHelp.Attributes.Add("OnClick", "__Help(); return(false);");
            this.hlHelp.Style.Add("cursor", "pointer");

            if (this.Page is TPage)
            {
                this.lblTituloPagina.Text = ((TPage)this.Page).GetPageTitle();
            }
            else
            {
                this.lblTituloPagina.Text = string.Empty;
            }

            var email = string.Empty;

            if (this.Session["email"] != null
                && !string.IsNullOrEmpty(this.Session["email"].ToString()))
            {
                email = string.Format("Seu Email institucional é: {0}", this.Session["email"]);
            }

            var saudacao = string.Format(
                "(IP: {0}) {1}",
                Sistema.ObterIP(),
                email);

            this.lblMatricula.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            this.lblIP.Text = saudacao;

            Techne.Web.TSearchBox.RegisterTSearchBoxScript(this.Page);

            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;

            this.lblVersao.Text = versao + sufixo;

            //if (Session["aceite_termo"] == null)
            //{
            //    Response.Redirect("../Termos/AceiteTermoCompromissoDocente.aspx");
            //}

            if (this.HabilitaLoading)
            {
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered("managerMaster"))
                {
                    this.Page.ClientScript.RegisterStartupScript(
                        typeof(MasterPage),
                        "managerMaster",
                        @"<script type=""text/javascript"">
                        // Add event handlers to the search UpdatePanel
                        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

                        function startRequest(sender, e) {
                            pucLoading.Show();
                        }

                        function endRequest(sender, e) {
                            pucLoading.Hide();
                            AddEvents();
                        }
                        </script>
");
                }
            }

            this.lnk7.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/PageViewer.aspx?grp=DOL&report=chdocenteonline&matricula=" + HttpContext.Current.User.Identity.Name + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            RN.Docentes rnDocentes = new Docentes();
            var obj = (object[])this.grdTurmaDisciplina.GetRowValues(Convert.ToInt32(this.txtRow.Value), "nome_comp", "unidade_ens", "ano", "turma", "semestre", "nome_compl", "disciplina", "id", "curso", "modalidade", "tipo", "serie");

            if (obj[0] != null)
            {
                this.Session["txtNomeUnidadeEns"] = obj[0].ToString();
                this.Session["txtCodUnidadeEns"] = obj[1].ToString();
                this.Session["txtAno"] = obj[2].ToString();
                this.Session["txtTurma"] = obj[3].ToString();
                this.Session["txtPeriodo"] = obj[4].ToString();
                this.Session["txtNomeDisciplina"] = obj[5].ToString();
                this.Session["txtDisciplina"] = obj[6].ToString();
                this.Session["txtMatricula"] = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                this.Session["txtNomeProfessor"] = string.Empty; // rnDocentes.ObtemNomeDocentePor(System.Threading.Thread.CurrentPrincipal.Identity.Name);
                this.Session["txtNumFunc"] = rnDocentes.ObtemNumFuncPor(System.Threading.Thread.CurrentPrincipal.Identity.Name);
                this.Session["txtCurso"] = obj[8].ToString();
                this.Session["txtModalidade"] = obj[9].ToString();
                this.Session["txtTipoCurso"] = obj[10].ToString();
                this.Session["txtSerie"] = obj[11].ToString();
            }

            if (this.Session["txtCodUnidadeEns"] != null)
            {
                this.txtNomeUnidadeEns.Value = this.Session["txtNomeUnidadeEns"].ToString();
                this.txtCodUnidadeEns.Value = this.Session["txtCodUnidadeEns"].ToString();
                this.txtAno.Value = this.Session["txtAno"].ToString();
                this.txtTurma.Value = this.Session["txtTurma"].ToString();
                this.txtPeriodo.Value = this.Session["txtPeriodo"].ToString();
                this.txtNomeDisciplina.Value = this.Session["txtNomeDisciplina"].ToString();
                this.txtDisciplina.Value = this.Session["txtDisciplina"].ToString();
                this.txtMatriculaProf.Value = this.Session["txtMatricula"].ToString();
                this.txtNomeProf.Value = Session["txtNomeProfessor"].ToString();
                this.txtNumFunc.Value = this.Session["txtNumFunc"].ToString();
                this.txtCurso.Value = this.Session["txtCurso"].ToString();
                this.txtModalidade.Value = this.Session["txtModalidade"].ToString();
                this.txtTipoCurso.Value = this.Session["txtTipoCurso"].ToString();
                this.txtSerie.Value = this.Session["txtSerie"].ToString();
                this.lblUnidadeEns.Visible = true;
                this.lblAno.Visible = true;
                this.lblTurma.Visible = true;
                this.lblPeriodo.Visible = true;
                this.lblDisciplina.Visible = true;
            }
            else
            {
                this.lblUnidadeEns.Visible = false;
                this.lblAno.Visible = false;
                this.lblTurma.Visible = false;
                this.lblPeriodo.Visible = false;
                this.lblDisciplina.Visible = false;
            }

            if (this.Request.UrlReferrer == null)
            {
                return;
            }

            var referencia = this.Request.UrlReferrer.AbsoluteUri;

            if (referencia.EndsWith(Config))
            {
                this.Response.Redirect(NotasTurma);
            }
            else
            {
                this.Response.Redirect(referencia);
            }
        }

        protected void btnTrocar_Click(object sender, EventArgs e)
        {
            this.grdTurmaDisciplina.ExpandAll();
        }

        protected void chkMensagem_CheckedChanged(object sender, EventArgs e)
        {
            this.Session["msg"] = (sender as ASPxCheckBox).Checked;
            this.msg.Visible = (sender as ASPxCheckBox).Checked;
        }

        protected void grdTurmaDisciplina_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName.Equals("turma")
                || e.DataColumn.FieldName.Equals("nome_compl"))
            {
                var possuiNotasPendentes = this.grdTurmaDisciplina.GetDataRow(e.VisibleIndex)["possuiNotasPendentes"].ToString();
                e.Cell.ForeColor = possuiNotasPendentes == "S" ? Color.Red : Color.Green;

                var validoParaLancamento = this.grdTurmaDisciplina.GetDataRow(e.VisibleIndex)["validoParaLancamento"].ToString();
                e.Cell.Attributes.Add("validoParaLancamento", validoParaLancamento);
            }
        }

        protected void lbSair_Click(object sender, EventArgs e)
        {
            Session.Abandon();

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();

            this.Response.End();
        }

        protected void lnk10_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(Protocolos);
        }

        protected void lnk2_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(NotasTurma);
        }

        protected void lnk6_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(DisponibilidadeGLP);
        }

        protected void lnk8_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AvaliacaoCurriculoMinimo);
        }

        protected void lnk9_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(CurriculoMinimo);
        }

        protected void scrMng_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            this.scrMng.AsyncPostBackErrorMessage = e.Exception.Message;
        }
    }
}