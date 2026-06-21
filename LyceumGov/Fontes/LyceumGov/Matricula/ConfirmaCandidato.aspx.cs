using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ConfirmaCandidato.aspx")]
    [ControlText("Confirmação do Candidato")]
    [Title("Confirmação do Candidato")]

    public partial class ConfirmaCandidato : TPage
    {

        public object ListarContato(object opcaoInscricaoId)
        {
            RN.Matriculas.ContatoOpcaoInscricao contatoOpcao = new Techne.Lyceum.RN.Matriculas.ContatoOpcaoInscricao();

            if (opcaoInscricaoId != null)
            {
                return contatoOpcao.ListaPor(Convert.ToInt32(opcaoInscricaoId));
            }

            return null;
        }

        public object ListarOpcaoIrmaoForaRede(object inscricaoAlunoId)
        {
            RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();

            var inscricao = inscricaoAlunoId != null ? inscricaoAlunoId.ToString() : null;

            if (!inscricao.IsNullOrEmptyOrWhiteSpace())
            {
                return opcao.ListaPor(Convert.ToInt32(inscricao));

            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.Matriculas.OpcaoInscricao inscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
                RN.DTOs.DadosConfirmacaoCandidato dados = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoCandidato();

                this.lblMensagem.Text = string.Empty;

                if (!this.IsPostBack)
                {

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        if (Request.QueryString["Chave"] != null)
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            dados.OpcaoInscricaoId = ObterQueryString(decodedText).OpcaoInscricaoId;

                            dados = inscricao.ObtemDadosConfirmacaoCandidatoPor(dados.OpcaoInscricaoId);

                            if (dados.OpcaoInscricaoId > 0)
                            {
                                CarregaMotivo();
                                CarregarModais();
                                PreencherDados(dados);
                                divPrincipal.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Para visualizar os dados é necessario escolher um candidato.";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private RN.DTOs.DadosConfirmacaoCandidato ObterQueryString(string queryString)
        {
            RN.DTOs.DadosConfirmacaoCandidato dadosConfimacao = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoCandidato();
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("OpcaoInscricaoId") >= 0)
                {
                    dadosConfimacao.OpcaoInscricaoId = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("PreCadastroAlunoId") >= 0)
                {
                    dadosConfimacao.PreCadastroAlunoId = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("ControleVagaId") >= 0)
                {
                    dadosConfimacao.ControleVagaId = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("CPF") >= 0)
                {
                    dadosConfimacao.Cpf = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("Censo") >= 0)
                {
                    dadosConfimacao.Censo = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("Mae") >= 0)
                {
                    dadosConfimacao.NomeMae = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("DataNascimento") >= 0)
                {
                    dadosConfimacao.DataNascimento = Convert.ToDateTime(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("Pessoa") >= 0)
                {
                    dadosConfimacao.Pessoa = Convert.ToDecimal(dados.Substring(dados.LastIndexOf('=') + 1));
                }
            }

            return dadosConfimacao;
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdOpcoesIrmaoForaRede, "Lista de Escolas escolhidas pelo seu irmão");
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(hdnControleVagaId.Value);

                Response.Redirect("ConfirmacaoCandidato.aspx?ChaveConfirmacao=" + Convert.ToBase64String(bytesToEncode), false);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
                RN.DTOs.DadosConfirmacaoCandidato dados = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoCandidato();
                ValidacaoDados validacao = new ValidacaoDados();
                string matriculaAluno = string.Empty;
                LyFlPessoa flPessoa = new LyFlPessoa();
                LyFotoPessoa fotoPessoa = new LyFotoPessoa();
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                dados = ObterQueryString(decodedText);
                dados.Fase = Convert.ToInt32(hdnFase.Value);
                dados.Ano = Convert.ToInt32(txtAno.Text);
                dados.Periodo = Convert.ToInt32(txtPeriodo.Text);
                dados.Nome = txtNome.Text.Trim();
                dados.NumeroInscricao = Convert.ToInt32(txtNumeroInscricao.Text);
                dados.InscricaoAlunoId = Convert.ToInt32(hdnInscricao.Value);
                dados.Confirma = !rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblConfirmacao.SelectedValue == "Sim" ? true : false) : (bool?)null;
                dados.UsuarioResponsavel = User.Identity.Name;
                dados.MotivoRejeicaoInscricaoId = !ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivo.SelectedValue) : (int?)null;
                dados.Serie = Convert.ToInt32(txtSerie.Text);

                if (rblConfirmacao.SelectedValue == "Sim")
                {
                    dados.EnsinoReligioso = chkEnsReligioso.Checked;
                    dados.LinguaEstrangeiraFacultativa = chkLinguaEstrangeira.Checked;
                    dados.Curriculo = hdnCurriculo.Value;

                    string modais = string.Empty;
                    bool transporte = false;
                    bool onibus = false;
                    int cont_transp = 0;
                    string transp_rodov = string.Empty;
                    string transp_aquav = string.Empty;
                    string transp_onibus = string.Empty; 

                    foreach (ListItem item in chkModais.Items)
                    {
                        if (item.Selected
                            && item.Value != string.Empty)
                        {
                            modais += item.Value;
                            modais += ";";

                            if (item.Value == "5") //TRANSPORTE RURAL
                            {
                                transporte = true;
                            }
                        }

                        if (item.Selected && item.Value != string.Empty)
                        {
                            modais += item.Value;
                            modais += ";";

                            if (item.Value == "2") //OPERADORA ÔNIBUS
                            {
                                onibus = true;
                            }
                        }
                    }

                    if (transporte)
                    {
                        foreach (ListItem item in chkRodoviario.Items)
                        {
                            if (item.Selected)
                            {
                                cont_transp++;
                                transp_rodov += item.Value;
                                transp_rodov += ";";
                            }
                        }

                        foreach (ListItem item in chkAquaviario.Items)
                        {
                            if (item.Selected)
                            {
                                cont_transp++;
                                transp_aquav += item.Value;
                                transp_aquav += ";";
                            }
                        }
                    }

                    if (onibus)
                    {
                        foreach (ListItem item in chkOnibus.Items)
                        {
                            if (item.Selected)
                            {
                                cont_transp++;
                                transp_onibus += item.Value;
                                transp_onibus += ";";
                            }
                        }
                    }

                    flPessoa.FlField04 = !ddlGratuidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGratuidade.SelectedValue : null;
                    flPessoa.FlField05 = !modais.IsNullOrEmptyOrWhiteSpace() ? modais : null;
                    flPessoa.FlField10 = !ddlPoderPublicoTransp.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPoderPublicoTransp.SelectedValue : null;
                    flPessoa.FlField11 = !transp_rodov.IsNullOrEmptyOrWhiteSpace() ? transp_rodov : null;
                    flPessoa.FlField12 = !transp_aquav.IsNullOrEmptyOrWhiteSpace() ? transp_aquav : null;
                    flPessoa.FlField20 = !transp_onibus.IsNullOrEmptyOrWhiteSpace() ? transp_onibus : null;
                    flPessoa.FlField23 = !rblDescFamilia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblDescFamilia.SelectedValue : null;

                    if (bimgFotoPessoa.ContentBytes != null)
                    {
                        fotoPessoa.Foto = bimgFotoPessoa.ContentBytes;
                    }
                    else
                    {
                        byte[] imagemVazia = { 0 };
                        fotoPessoa.Foto = null;
                    }

                    dados.IrmaoMatricula = !txtMatriculaIrmao.Text.IsNullOrEmptyOrWhiteSpace() ? txtMatriculaIrmao.Text : null;
                    dados.IrmaoIdInscricao = !hdnIdIrmao.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdIrmao.Value) : (int?)null;
                }

                validacao = opcao.ValidaConfirmacao(dados, flPessoa, fotoPessoa);

                if (validacao.Valido)
                {
                    opcao.Confirma(dados, flPessoa, fotoPessoa, out matriculaAluno);

                    if (dados.Confirma.Value)
                    {
                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(matriculaAluno);

                        Response.Redirect("~/Academico/Alunos.aspx?ChaveConfirmacao=" + Convert.ToBase64String(bytesToEncode), false);
                    }
                    else
                    {
                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(dados.ControleVagaId.ToString());

                        Response.Redirect("ConfirmacaoCandidato.aspx?ChaveConfirmacao=" + Convert.ToBase64String(bytesToEncode), false);
                    }

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaMotivo()
        {
            RN.Matriculas.MotivoRejeicaoInscricao rnMotivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.MotivoRejeicaoInscricao();
            try
            {
                ddlMotivo.Items.Clear();
                ddlMotivo.DataSource = rnMotivoRejeicaoInscricao.ListaMotivoAtivoParaConfirmacao();
                ddlMotivo.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlMotivo.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblConfirmacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlMotivo.Visible = false;
                pnlDisciplinaOptativas.Visible = false;
                pnTransporte.Visible = false;
                pnlDescFamilia.Visible = true;
                ddlMotivo.ClearSelection();
                ddlGratuidade.ClearSelection();
                ddlPoderPublicoTransp.ClearSelection();
                chkModais.Enabled = true;
                chkModais.SelectedValue = null;
                chkAquaviario.ClearSelection();
                chkRodoviario.ClearSelection();
                chkOnibus.ClearSelection();
                lblRodoviario.Visible = false;
                chkRodoviario.Visible = false;
                lblAquaviario.Visible = false;
                chkAquaviario.Visible = false;
                lblOnibus.Visible = false;
                chkOnibus.Visible = false;
                rblDescFamilia.Visible = true;

                if (!rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    btnSalvar.Visible = true;
                    pnlMotivo.Visible = (rblConfirmacao.SelectedValue == "Nao");

                    if (rblConfirmacao.SelectedValue == "Sim")
                    {
                        pnlDisciplinaOptativas.Visible = true;
                        pnTransporte.Visible = true;
                        pnlDescFamilia.Visible = true;
                        VerificaDisciplinaOptativa();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void VerificaDisciplinaOptativa()
        {
            LyCurriculo curriculo = new LyCurriculo();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            hdnCurriculo.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!txtAno.Text.IsNullOrEmptyOrWhiteSpace()
                && !txtPeriodo.Text.IsNullOrEmptyOrWhiteSpace()
                && !txtTurno.Text.IsNullOrEmptyOrWhiteSpace()
                && !txtSerie.Text.IsNullOrEmptyOrWhiteSpace()
                && (!hdnCurso.Value.IsNullOrEmptyOrWhiteSpace()))
            {
                curriculo = rnCurriculo.ObtemPrimeiroAtivoPor(hdnCurso.Value, txtTurno.Text, Convert.ToInt32(txtSerie.Text), Convert.ToInt32(txtAno.Text), Convert.ToInt32(txtPeriodo.Text));
                hdnCurriculo.Value = curriculo.Curriculo;
                if (curriculo.EnsinoReligioso == "S")
                {
                    chkEnsReligioso.Enabled = true;
                }

                if (curriculo.LinguaEstrangeira == "S")
                {
                    chkLinguaEstrangeira.Enabled = true;
                }
            }
        }

        protected void ddlGratuidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPoderPublicoTransp.ClearSelection();
                ddlPoderPublicoTransp.Enabled = false;
                chkModais.ClearSelection();
                chkModais.Enabled = false;

                if (!ddlGratuidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (ddlGratuidade.SelectedValue == "S"))
                {
                    ddlPoderPublicoTransp.SelectedValue = "Estadual";
                    ddlPoderPublicoTransp.Enabled = true;
                    chkModais.Enabled = true;
                }
                if (!ddlGratuidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (ddlGratuidade.SelectedValue == "N"))
                {
                    chkAquaviario.ClearSelection();
                    chkRodoviario.ClearSelection();
                    chkOnibus.ClearSelection();
                    lblRodoviario.Visible = false;
                    chkRodoviario.Visible = false;
                    chkAquaviario.Visible = false;
                    lblAquaviario.Visible = false;
                    chkOnibus.Visible = false;
                    lblOnibus.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarModais()
        {
            Object objModal = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.TransporteModal);

            if (objModal != null)
            {
                chkModais.Items.Clear();
                chkModais.DataSource = objModal;
                chkModais.DataBind();
            }
        }

        protected void chkModais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //chkRodoviario.ClearSelection();
                //chkRodoviario.Visible = false;
                //lblRodoviario.Visible = false;

                //chkAquaviario.ClearSelection();
                //chkAquaviario.Visible = false;
                //lblAquaviario.Visible = false;

                //chkOnibus.ClearSelection();
                //chkOnibus.Visible = false;
                //lblOnibus.Visible = false;

                foreach (ListItem item in chkModais.Items)
                {
                    if (item.Selected && item.Text == "TRANSPORTE RURAL")
                    {
                        //chkModais.ClearSelection();
                        //item.Selected = true;

                        chkRodoviario.ClearSelection();
                        chkRodoviario.Visible = true;
                        lblRodoviario.Visible = true;

                        chkAquaviario.ClearSelection();
                        chkAquaviario.Visible = true;
                        lblAquaviario.Visible = true;

                        return;
                    }
                    else if (!item.Selected && item.Text == "TRANSPORTE RURAL")
                    {
                        chkRodoviario.ClearSelection();
                        chkRodoviario.Visible = false;
                        lblRodoviario.Visible = false;

                        chkAquaviario.ClearSelection();
                        chkAquaviario.Visible = false;
                        lblAquaviario.Visible = false;
                    }

                    //if (item.Selected && item.Text == "CARRO COM ACESSIBILIDADE (CADEIRANTE)")
                    //{
                    //    //chkModais.ClearSelection();
                    //    //item.Selected = true;
                    //}

                    if (item.Selected && item.Text == "ÔNIBUS")
                    {
                        //chkModais.ClearSelection();
                        //item.Selected = true;

                        chkOnibus.Visible = true;
                        lblOnibus.Visible = true;
                    }
                    else if (!item.Selected && item.Text == "ÔNIBUS")
                    {
                        chkOnibus.ClearSelection();
                        chkOnibus.Visible = false;
                        lblOnibus.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkAquaviario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in chkAquaviario.Items)
                {
                    if (item.Selected && item.Text == "Não utiliza transporte Aquaviário")
                    {
                        chkAquaviario.ClearSelection();
                        item.Selected = true;
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparTela()
        {
            txtNome.Text = string.Empty;
            hdnInscricao.Value = string.Empty;
            txtNumeroInscricao.Text = string.Empty;
            txtAno.Text = string.Empty;
            txtPeriodo.Text = string.Empty;
            txtModalidade.Text = string.Empty;
            txtSegmento.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtTurno.Text = string.Empty;
            rblConfirmacao.ClearSelection();
            rblDescFamilia.ClearSelection();
            ddlMotivo.ClearSelection();
            btnSalvar.Visible = false;
            hdnCurso.Value = string.Empty;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Checked = false;
            hdnCurriculo.Value = string.Empty;
            hdnControleVagaId.Value = string.Empty;
            ddlGratuidade.ClearSelection();
            ddlPoderPublicoTransp.ClearSelection();
            chkModais.SelectedValue = null;
            chkAquaviario.ClearSelection();
            chkRodoviario.ClearSelection();
            chkOnibus.ClearSelection();
            lblRodoviario.Visible = false;
            chkRodoviario.Visible = false;
            chkAquaviario.Visible = false;
            lblAquaviario.Visible = false;
            chkOnibus.Visible = false;
            lblOnibus.Visible = false;
        }

        private void PreencherDados(RN.DTOs.DadosConfirmacaoCandidato dados)
        {
            hdnInscricao.Value = dados.InscricaoAlunoId.ToString();
            hdnFase.Value = dados.Fase.ToString();
            txtNumeroInscricao.Text = dados.NumeroInscricao.ToString();
            txtNome.Text = dados.Nome;
            txtUnidade.Text = dados.Escola;
            txtAno.Text = dados.Ano.ToString();
            txtPeriodo.Text = dados.Periodo.ToString();
            txtModalidade.Text = dados.Modalidade;
            txtSegmento.Text = dados.Segmento;
            txtCurso.Text = dados.CursoDescricao;
            hdnCurso.Value = dados.Curso;
            txtSerie.Text = dados.Serie.ToString();
            txtTurno.Text = dados.Turno;
            hdnControleVagaId.Value = dados.ControleVagaId.ToString();

            if (dados.FotoArquivo == null || dados.FotoArquivo.Length <= 0)
            {
                bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                bimgFotoPessoa.EmptyImage.AlternateText = "Sem foto";
                bimgFotoPessoa.ContentBytes = null;
            }
            else
            {
                bimgFotoPessoa.ContentBytes = dados.FotoArquivo;
            }


            pnlIrmaoForaRede.Visible = false;
            pnlIrmaoRede.Visible = false;
            pnlNaoPossuiIrmao.Visible = false;

            txtNomePai.Text = dados.NomePai;
            txtNomeMae.Text = dados.NomeMae;
            chkNaoDeclarMae.Checked = dados.DeclaroAusenciaMae;
            chkNaoDeclarPai.Checked = dados.DeclaroAusenciaPai;

            if (dados.IrmaoNumeroInscricao == (int?)null && dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                pnlNaoPossuiIrmao.Visible = true;
                chkNaoPossuiIrmao.Checked = true;
            }

            if (dados.IrmaoNumeroInscricao != (int?)null)
            {
                pnlIrmaoForaRede.Visible = true;
                chkIrmaoForaRede.Checked = true;
                txtInscricaoIrmao.Text = dados.IrmaoNumeroInscricao > 0 ? dados.IrmaoNumeroInscricao.ToString() : string.Empty;
                txtNomeIrmaoForaRede.Text = !dados.DadosIrmao.NomeCompl.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.NomeCompl : string.Empty;
                txtDataNascIrmaoForaRede.Text = dados.DadosIrmao.DataNascimento.HasValue ? dados.DadosIrmao.DataNascimento.Value.ToShortDateString() : string.Empty;
                hdnIdIrmao.Value = dados.IrmaoIdInscricao > 0 ? dados.IrmaoIdInscricao.ToString() : string.Empty;

            }

            if (!dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                pnlIrmaoRede.Visible = true;
                chkIrmaoRede.Checked = true;
                txtMatriculaIrmao.Text = !dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace() ? dados.IrmaoMatricula : string.Empty;
                txtNomeIrmaoRede.Text = !dados.DadosIrmao.NomeCompl.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.NomeCompl : string.Empty;
                txtDataNascIrmaoRede.Text = dados.DadosIrmao.DataNascimento.HasValue ? dados.DadosIrmao.DataNascimento.Value.ToShortDateString() : string.Empty;
                txtUEIrmaoRede.Text = !dados.DadosIrmao.EscolaAtual.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.EscolaAtual : string.Empty;
                txtCursoIrmaoRede.Text = !dados.DadosIrmao.CursoDescricaoAtual.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.CursoDescricaoAtual : string.Empty;
                txtSerieIrmaoRede.Text = dados.DadosIrmao.SerieAtual > 0 ? dados.DadosIrmao.SerieAtual.ToString() : string.Empty;
                txtTurnoIrmaoRede.Text = !dados.DadosIrmao.TurnoDescricaoAtual.IsNullOrEmptyOrWhiteSpace() ? dados.DadosIrmao.TurnoDescricaoAtual : string.Empty;
            }

        }
    }
}
