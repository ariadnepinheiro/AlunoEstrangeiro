using System;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Controls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Data;
using Techne.Library.Sql.Structure;
using System.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/ConvocacaoMigracaoCH.aspx"),
    ControlText("ConvocacaoMigracaoCH"),
    Title("Convocação"),]
    public partial class ConvocacaoMigracaoCH : TPage
    {
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }

        public static string GetUrl()
        {
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        }

        private string BOTAO_SELECAO_CANDIDATOS = "Selecionar";
        private string BOTAO_NOVA_SELECAO = "Nova Seleção";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSelecao, "");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                Session["Aprovados"] = null;
                Session["privilegiado"] = RN.PadroesDeAcessos.VerificaPrivilegio(Convert.ToString(HttpContext.Current.User.Identity.Name));

                tseMunicipioProc.Enabled = false;
                tseDisciplina.Enabled = false;
            }
            if (!Convert.ToBoolean(Session["privilegiado"]))
            {
                bool bolEhContratoTempo = RN.PadroesDeAcessos.ConsultarPadacesContratoTempoPorUsuario(User.Identity.Name);

                if (!bolEhContratoTempo)
                {
                    tseRegional.DBValue = RN.Usuarios.RetornarRegionalUsuario(User.Identity.Name).ToString();
                    tseRegional.Enabled = false;
                }
                else
                {
                    tseRegional.Enabled = true;
                }
            }

            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull &&
                tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull &&
                tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull &&
                dtdataApresent != null && !String.IsNullOrEmpty(dtdataApresent.Text) &&
                dtHoraApresent != null && !dtHoraApresent.Text.Equals("__:__"))
            {
                IDictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("concurso", tseConcurso.DBValue.ToString());
                pares.Add("disciplina", tseDisciplina.DBValue.ToString());
                pares.Add("dt_apresentacao", ObtemDataHora());
                pares.Add("municipio", (tseMunicipioProc.DBValue.IsNull ? null : tseMunicipioProc.DBValue.ToString()));

                btnCorreios.ClientSideEvents.Click = @"function (s, e){ window.open('../Relatorio/Relatorios.aspx?report=rsconvocacaocorreios&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false; }";
                btnCoordenadoria.ClientSideEvents.Click = @"function (s, e){ window.open('../Relatorio/Relatorios.aspx?report=rsconvocacaocoordenadoria&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false; }";
            }
        }

        protected void tseConcurso_Changed(object sender, EventArgs args)
        {
            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull)
            {
                tseMunicipioProc.ResetValue();
                tseRegional.ResetValue();
                tseDisciplina.ResetValue();
                LimparCampos();
            }
            else
            {
                LimparCampos();
            }

            pnSelecao.Visible = false;
        }

        protected void tseDisciplina_Changed(object sender, EventArgs args)
        {
            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();

            pnSelecao.Visible = false;
            LimparCampos();
            BloquearBotoes();
            btnSelecionar.Text = BOTAO_SELECAO_CANDIDATOS;

            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull
                && tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull)
            {
                string concurso = tseConcurso.DBValue.ToString();
                string disciplina = tseDisciplina.DBValue.ToString();
                string municipio = tseMunicipioProc.DBValue.ToString();
                string regional = tseRegional.DBValue.ToString();

                txtDisponivel.Text = Convert.ToString(rnDocenteCandidato.ObtemInscricoesParaConvocacaoPor(disciplina, concurso, municipio, regional));
                int disponivel = Convert.ToInt32(txtDisponivel.Text);
                if (disponivel <= 0)
                {
                    btnSelecionar.Enabled = false;
                    lblMensagem.Text = "Atenção: Não existem inscrições disponíveis para o processo seletivo.";
                    return;
                }
                HabilitarCampos();
                pnCampos.Visible = true;
                btnSelecionar.Enabled = true;
                lblMensagem.Text = string.Empty;
            }
            else
            {
                lblMensagem.Text = "Favor selecionar processo seletivo, regional, municipio e disciplina válidos.";
                pnCampos.Visible = false;
                return;
            }
        }

        public object Listar(DbObject tseRegional, DbObject tseConcurso, DbObject tseDisciplina, string qtd, string dtHoraApresent, DateTime dtdataApresent, DbObject tseMunicipioProc)
        {
            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
            DataTable dadosGrid = null;

            if (!tseRegional.IsNull && !tseConcurso.IsNull && !tseDisciplina.IsNull && !string.IsNullOrEmpty(qtd))
            {
                if (Session["Aprovados"] == null)
                {
                    dadosGrid = rnDocenteCandidato.ListaInscritosParaConvocacaoPor(qtd, tseConcurso.ToString(), tseDisciplina.ToString(), tseMunicipioProc.ToString(), tseRegional.ToString());
                }
                else
                {
                    string hora = dtHoraApresent;
                    string[] horas_s = hora.Split(':');
                    int h = Convert.ToInt16(horas_s[0]);
                    int m = Convert.ToInt16(horas_s[1]);
                    DateTime data = dtdataApresent.Date;
                    data = data.AddHours(h);
                    data = data.AddMinutes(m);
                    dadosGrid = rnDocenteCandidato.ListaAprovadosPor(qtd, tseConcurso.ToString(), tseDisciplina.ToString(), data, tseMunicipioProc.ToString(), tseRegional.ToString());
                }
            }

            return dadosGrid;
        }

        protected void btnSelecionar_Click(object sender, EventArgs e)
        {
            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
            ValidacaoDados validacao = new ValidacaoDados();
            Session["Aprovados"] = null;

            try
            {
                string disciplina = Convert.ToString(tseDisciplina.DBValue);
                string qtd = txtQuantidade.Text;
                string munic = Convert.ToString(tseMunicipioProc.DBValue);
                string regional = Convert.ToString(tseRegional.DBValue);
                string concurso = tseConcurso.DBValue.ToString();
                DateTime dataPublicacaoConvocacao = !dtPublicacaoConvocacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtPublicacaoConvocacao.Date : DateTime.MinValue;


                if (btnSelecionar.Text == BOTAO_SELECAO_CANDIDATOS)
                {
                    DateTime data = DateTime.MinValue;
                    if (!dtHoraApresent.Text.Replace(":", string.Empty).Trim().IsNullOrEmptyOrWhiteSpace() && !dtdataApresent.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        string hora = dtHoraApresent.Text;
                        string[] horas_s = hora.Split(':');
                        int h = Convert.ToInt16(horas_s[0]);
                        int m = Convert.ToInt16(horas_s[1]);
                        data = dtdataApresent.Date;
                        data = data.AddHours(h);
                        data = data.AddMinutes(m);
                    }

                    DataTable qt = null;
                    validacao = rnDocenteCandidato.ValidaSelecaoConvocacao(qtd, dataPublicacaoConvocacao, data, concurso, disciplina, munic, regional, out qt);

                    if (validacao.Valido)
                    {
                        odsSelecao.Select();
                        odsSelecao.DataBind();
                        grdSelecao.DataBind();
                        pnSelecao.Visible = true;
                        DesbloquearBotoes();
                        btnSelecionar.Text = BOTAO_NOVA_SELECAO;
                        DesabilitarCampos();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
                else if (btnSelecionar.Text.Equals(BOTAO_NOVA_SELECAO))
                {
                    HabilitarCampos();
                    LimparCampos();
                    BloquearBotoes();
                    btnSelecionar.Text = BOTAO_SELECAO_CANDIDATOS;
                    txtDisponivel.Text = Convert.ToString(rnDocenteCandidato.ObtemInscricoesParaConvocacaoPor(disciplina, concurso, munic, regional));
                    pnSelecao.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConvocar_Click(object sender, EventArgs e)
        {
            RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                DateTime dataPublicacaoConvocacao = !dtPublicacaoConvocacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtPublicacaoConvocacao.Date : DateTime.MinValue;
                string concurso = tseConcurso.DBValue.ToString();
                string disciplina = tseDisciplina.DBValue.ToString();
                string municipio = tseMunicipioProc.DBValue.ToString();
                string qtd = txtQuantidade.Text;
                string hora = dtHoraApresent.Text;
                string[] horas_s = hora.Split(':');
                int h = Convert.ToInt16(horas_s[0]);
                int m = Convert.ToInt16(horas_s[1]);
                DateTime data = dtdataApresent.Date;
                data = data.AddHours(h);
                data = data.AddMinutes(m);
                string regional = Convert.ToString(tseRegional.DBValue);

                //Lista para inscritos que serão convocados
                DataTable convocar = null;

                validacao = rnDocenteCandidato.ValidaSelecaoConvocacao(qtd, dataPublicacaoConvocacao, data, concurso, disciplina, municipio, regional, out convocar);

                if (validacao.Valido)
                {
                    //Convoca
                    rnDocenteCandidato.Convoca(convocar, dataPublicacaoConvocacao, data, User.Identity.Name);

                    lblMensagem.Text = "Convocação realizada com sucesso";
                    btnCoordenadoria.Enabled = true;
                    btnConvocar.Enabled = false;
                    Session["Aprovados"] = "true";
                    odsSelecao.Select();
                    odsSelecao.DataBind();
                    grdSelecao.DataBind();

                    if (convocar.Rows.Count > 0)
                    {
                        foreach (DataRow item in convocar.Rows)
                        {
                            string email = item["E_MAIL"].ToString();

                            if (!email.IsNullOrEmptyOrWhiteSpace())
                            {
                                EnviaMalaDireta(concurso, email);
                            }

                            string emailInterno = item["E_MAIL_INTERNO"].ToString();

                            if (!email.IsNullOrEmptyOrWhiteSpace())
                            {
                                EnviaMalaDireta(concurso, emailInterno);
                            }
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                Session["Aprovados"] = null;
                lblMensagem.Text = ex.Message;
            }
        }

        private void EnviaMalaDireta(string concurso, string strEmail)
        {
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

            string endereco = rnProcessoSeletivo.RetornaEnderecoCoordenadoria(tseRegional.DBValue.ToString()).Rows[0]["ENDERECO"].ToString();

            RN.DTOs.DadosEmail email = new Techne.Lyceum.RN.DTOs.DadosEmail();
            email.Remetente = System.Configuration.ConfigurationManager.AppSettings["EmailContratoTemporario"].ToString();
            email.Login = System.Configuration.ConfigurationManager.AppSettings["EmailContratoTemporario_Login"].ToString();
            email.Senha = System.Configuration.ConfigurationManager.AppSettings["EmailContratoTemporario_Senha"].ToString();
            email.Assunto = "Convocação Processo Seletivo Interno";

            if (concurso == "2026-Migracao")
            {
                email.Texto = "<p>" +
                                    " Prezado(a) candidato(a), " +
                                    " <br />" +
                                    " <br />" +
                                    " Tendo em vista a publicação da convocação em Diário Oficial de " + dtPublicacaoConvocacao.Text + " , solicitamos seu comparecimento " +
                                    " à Coordenadoria Regional de Gestão de Pessoas " + tseRegional["REGIONAL"].ToString() + ", " +
                                    " situada à " + endereco + ", " +
                                    " no dia " + dtdataApresent.Text + ", às " + dtHoraApresent.Text + "h, para início dos procedimentos " +
                                    " concernentes à migração da carga horária para 30 horas semanais. " +
                                    " <br />" +
                                    " <br />" +
                                    " Nesta primeira etapa, será realizada a avaliação das documentações para conferência das informações prestadas na inscrição. Portanto, deverão ser apresentados os originais das documentações e declarações anexados ao sistema no ato da inscrição." +
                                    " <br />" +
                                    " <br />" +
                                    " Pedimos que compareça no dia e horário estabelecidos no edital de convocação. Porém, caso não seja possível, você terá até três dias para se apresentar, de acordo com o previsto na Resolução SEEDUC nº 6425 de 19 de fevereiro de 2026." +
                                    " <br />" +
                                    " <br />" +
                                    " Atenciosamente," +
                                    " <br />" +
                                    " <br />" +
                                    " Equipe SUPGP/SUBAD/SEEDUC" +
                                "</p>";
            }
            else if (concurso == "2025-Migracao")
            {
                email.Texto = "<p>" +
                                "Prezado(a) candidato(a), " +
                                " <br />" +
                                " <br />" +
                                " Tendo em vista a publicação da convocação em Diário Oficial de " + dtPublicacaoConvocacao.Text + " , solicitamos seu comparecimento " +
                                " à Coordenadoria Regional de Gestão de Pessoas " + tseRegional["REGIONAL"].ToString() + ", " +
                                " situada à " + endereco + ", " +
                                " no dia " + dtdataApresent.Text + ", às " + dtHoraApresent.Text + " h, para início dos procedimentos " +
                                " concernentes à migração da carga horária para 30 horas semanais. " +
                                " <br />" +
                                " <br />" +
                                " Nesta primeira etapa, será realizada a avaliação das documentações para conferência das informações prestadas na inscrição. Portanto, deverão ser apresentados os originais da documentação pessoal, da experiência profissional e da formação/titulação que foram anexados ao sistema." +
                                " <br />" +
                                " <br />" +
                                " Pedimos que compareça no dia e horário estabelecidos no edital de convocação. Porém, caso não seja possível, você terá até três dias para se apresentar, de acordo com o previsto na Resolução SEEDUC nº 6316 de 10 de janeiro de 2025." +
                                " <br />" +
                                " <br />" +
                                " Atenciosamente," +
                                " <br />" +
                                " <br />" +
                                " Equipe SUPGP" +
                            "</p>";
            }
            else
            {
                email.Texto = "<p>" +
                                "Prezado(a) candidato(a), " +
                                " <br />" +
                                " <br />" +
                                " Tendo em vista a publicação da convocação em Diário Oficial de " + dtPublicacaoConvocacao.Text + " , solicitamos seu comparecimento " +
                                " à Coordenadoria Regional de Gestão de Pessoas " + tseRegional["REGIONAL"].ToString() + ", " +
                                " situada à " + endereco + ", " +
                                " no dia " + dtdataApresent.Text + ", às " + dtHoraApresent.Text + " h, para início dos procedimentos " +
                                " concernentes à migração da carga horária para 30 horas semanais. " +
                                " <br />" +
                                " <br />" +
                                " Nesta primeira etapa, será realizada a avaliação das documentações para conferência das informações prestadas na inscrição. Portanto, deverão ser apresentados os originais da documentação pessoal, da experiência profissional e da formação/titulação que foram anexados ao sistema." +
                                " <br />" +
                                " <br />" +
                                " Pedimos que compareça no dia e horário estabelecidos no edital de convocação. Porém, caso não seja possível, você terá até três dias para se apresentar, de acordo com o previsto na Resolução SEEDUC nº 6254 de 19/04/2024." +
                                " <br />" +
                                " <br />" +
                                " Atenciosamente," +
                                " <br />" +
                                " <br />" +
                                " Equipe SUPGP" +
                            "</p>";
            }

            email.Destinatario = strEmail;

            try
            {
                RN.Util.Email.Envia(email);
            }
            catch (Exception)
            {
                ArquivaEmailNaoEnviado(email);
            }
        }

        protected void ArquivaEmailNaoEnviado(RN.DTOs.DadosEmail email)
        {
            RN.RecursosHumanos.Entidades.EmailNaoEnviado emailNaoEnviado = new RN.RecursosHumanos.Entidades.EmailNaoEnviado();
            RN.RecursosHumanos.EmailNaoEnviado rnRecursosHumanos = new Techne.Lyceum.RN.RecursosHumanos.EmailNaoEnviado();

            try
            {
                emailNaoEnviado.Projeto = "Convocação Migração CH";
                emailNaoEnviado.Remetente = email.Remetente;
                emailNaoEnviado.Destinatario = email.Destinatario;
                emailNaoEnviado.Assunto = email.Assunto;
                emailNaoEnviado.Texto = email.Texto;
                emailNaoEnviado.UsuarioId = User.Identity.Name;

                rnRecursosHumanos.Insere(emailNaoEnviado);
            }
            catch (Exception ex)
            {
                lblMensagem.Text += string.Format("<br/>E-mail: {0} não enviado - {1}", email.Destinatario, ex.Message);
            }
        }

        protected void LimparCampos()
        {
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            string concurso = tseConcurso.DBValue.ToString();
            string disciplina = tseDisciplina.DBValue.ToString();
            string municipio = tseMunicipioProc.DBValue.ToString();
            string regional = tseRegional.DBValue.ToString();
            txtQuantidade.Text = string.Empty;
            dtdataApresent.Text = string.Empty;
            dtHoraApresent.Text = string.Empty;
            dtPublicacaoConvocacao.Text = string.Empty;
        }

        protected void HabilitarCampos()
        {
            txtDisponivel.Enabled = true;
            txtQuantidade.Enabled = true;
            dtdataApresent.Enabled = true;
            dtHoraApresent.Enabled = true;
            dtPublicacaoConvocacao.Enabled = true;
        }

        protected void DesabilitarCampos()
        {
            txtDisponivel.Enabled = false;
            txtQuantidade.Enabled = false;
            dtdataApresent.Enabled = false;
            dtHoraApresent.Enabled = false;
            dtPublicacaoConvocacao.Enabled = false;
        }

        protected void BloquearBotoes()
        {
            btnConvocar.Enabled = false;
            btnCorreios.Enabled = false;
            btnCoordenadoria.Enabled = false;
        }

        protected void DesbloquearBotoes()
        {
            btnConvocar.Enabled = true;
        }

        private string ObtemDataHora()
        {
            DateTime data = DateTime.MinValue;

            if (!dtHoraApresent.Text.Replace(":", string.Empty).Trim().IsNullOrEmptyOrWhiteSpace() && !dtdataApresent.Text.IsNullOrEmptyOrWhiteSpace())
            {
                string hora = dtHoraApresent.Text;
                string[] horas_s = hora.Split(':');
                int h = Convert.ToInt16(horas_s[0]);
                int m = Convert.ToInt16(horas_s[1]);
                data = dtdataApresent.Date;
                data = data.AddHours(h);
                data = data.AddMinutes(m);
            }

            return data.ToString("yyyy-MM-dd HH:mm") + ":00";
        }
    }
}
