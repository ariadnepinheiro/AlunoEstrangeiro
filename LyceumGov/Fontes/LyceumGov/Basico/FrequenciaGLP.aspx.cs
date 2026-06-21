using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.DTOs;
using Techne.Web;
using Microsoft.Reporting.WebForms;
using Techne.Lyceum.RN.Relatorios;
using System.Configuration;
using DevExpress.Web.ASPxGridView;
using System.Diagnostics;
using Techne.Lyceum.RN.Util;
using Techne.Data;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/FrequenciaGLP.aspx"), ControlText("FrequenciaGLP"), Title("Frequência de GLP")]
    public partial class FrequenciaGLP : TPage
    {
        private readonly RN.RecursosHumanos.CargaHNaoTrabMes rnCargaHNaoTrabMes;

        public FrequenciaGLP()
        {
            rnCargaHNaoTrabMes = new RN.RecursosHumanos.CargaHNaoTrabMes();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdFrequencia, "Alocação de GLP");
            TituloGrid(grdTurma, "Detalhes GLP por Turma");
        }

        public void LimpaBusca()
        {
            pnlRatificar.Visible = false;
            lblMensagemFinalizacao.Text = string.Empty;
            plaFrequencia.Visible = false;
            btnImprimir.Visible = false;
            btnFinalizar.Enabled = false;
            chkTermoResponsabilidade.Enabled = true;
            chkTermoResponsabilidade.Checked = false;
            grdFrequencia.Visible = false;
            odsFrequencia.Select();
            odsFrequencia.DataBind();
            grdFrequencia.DataBind();
            hdnFinalizacao.Value = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimpaBusca();

                if (sessao != null)
                {

                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {

                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            
                            tseMunicipio.ResetValue();
                            tseSituaçaoFuncionamento.ResetValue();
                            tseUnidadeResponsavel.ResetValue();



                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;

                        tseSituaçaoFuncionamento.ResetValue();
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimpaBusca();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseSituaçaoFuncionamento.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;

                        tseSituaçaoFuncionamento.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                LimpaBusca();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];


                        }
                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseSituacaoFuncionamento_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpaBusca();
            ddlMes.ClearSelection();
            ddlPeriodoLancamento.Items.Clear();
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimpaBusca();

                if (!ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaPeriodoLancamento(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodoLancamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimpaBusca();
               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPeriodoLancamento(int ano,int mes)
        {
            try
            {

                RN.RecursosHumanos.PeriodoFrequenciaGlp rnPeriodoFrequenciaGlp = new Techne.Lyceum.RN.RecursosHumanos.PeriodoFrequenciaGlp();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlPeriodoLancamento.Items.Clear();
                ddlPeriodoLancamento.DataSource = rnPeriodoFrequenciaGlp.ListaPor(ano,mes);
                ddlPeriodoLancamento.DataBind();
                ddlPeriodoLancamento.Items.Insert(0, item);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                hdnFinalizacao.Value = string.Empty;
                btnImprimir.Visible = false;
                RN.RecursosHumanos.CargaHNaoTrabMesFinalizada rnCargaHFinalizada = new Techne.Lyceum.RN.RecursosHumanos.CargaHNaoTrabMesFinalizada();                

                string mensagemFinalizacao;
                plaFrequencia.Visible = false;
                btnFinalizar.Enabled = false;
                pnlRatificar.Visible = false;

                if (string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O campo \"ANO\" é obrigatório.";
                }

                if (string.IsNullOrEmpty(ddlMes.SelectedValue))
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O campo \"MÊS\" é obrigatório.";
                }

                if (string.IsNullOrEmpty(ddlPeriodoLancamento.SelectedValue))
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O campo \"PERÍODO\" é obrigatório.";
                }

                if (string.IsNullOrEmpty((tseRegional.Value ?? "").ToString()))
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O campo \"REGIONAL\" é obrigatório.";
                }

                if (string.IsNullOrEmpty((tseMunicipio.Value ?? "").ToString()))
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O campo \"MUNICÍPIO\" é obrigatório.";
                }

                if (string.IsNullOrEmpty((tseUnidadeResponsavel.Value ?? "").ToString()))
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O campo \"UNIDADE DE ENSINO\" é obrigatório.";
                }

                if (lblMensagem.Text.Length > 0)
                    return;

                DateTime mes_combo = new DateTime(int.Parse(ddlAno.SelectedValue), int.Parse(ddlMes.SelectedValue), 1);
                DateTime mes_anterior = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                DateTime mes_atual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                if (mes_combo > mes_atual)
                {
                    lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                    lblMensagem.Text += "O mês/ano informado deve ser menor ou igual ao mês/ano da data atual.";
                }

                if (lblMensagem.Text.Length > 0)
                    return;

                if (rnCargaHFinalizada.PossuiFinalizacaoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), out mensagemFinalizacao))
                {
                    this.lblMensagemFinalizacao.Text = mensagemFinalizacao;
                    hdnFinalizacao.Value = "S";
                    pnlRatificar.Visible = false;
                    btnImprimir.Visible = true;
                    btnFinalizar.Enabled = false;
                    chkTermoResponsabilidade.Enabled = false;
                }
                else
                {
                    pnlRatificar.Visible = true;
                }

                odsFrequencia.SelectParameters["ano"].DefaultValue = ddlAno.SelectedValue;
                odsFrequencia.SelectParameters["mes"].DefaultValue = ddlMes.SelectedValue;
                odsFrequencia.SelectParameters["periodo"].DefaultValue = ddlPeriodoLancamento.SelectedValue;
                odsFrequencia.SelectParameters["id_regional"].DefaultValue = (tseRegional.Value ?? "").ToString();
                odsFrequencia.SelectParameters["municipio"].DefaultValue = (tseMunicipio.Value ?? "").ToString();
                odsFrequencia.SelectParameters["faculdade"].DefaultValue = (tseUnidadeResponsavel.Value ?? "").ToString();

                grdFrequencia.DataBind();
                plaFrequencia.Visible = true;
                grdFrequencia.Visible = true;

                if ((mes_combo <= mes_anterior) && hdnFinalizacao.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    pnlRatificar.Visible = true;
                }

               
            }
            catch
            {
                throw;
            }
        }

        protected void btnSalvarCHNaoTrab_Click(object sender, EventArgs e)
        {
            int ano;
            int mes;
            decimal numFunc;
            string unidadeEns;
            int chMensal;
            int idCargaHNaoTrabMes;
            int chNaoTrabalhadaMes;

            try
            {
                if (!EhPrivilegiado && !PossuiPerfilAlteracao && !EhDiaPermitidoParaAlteracao)
                {
                    lblMensagemCHNaoTrab.Text = "Você não é usuário privilegiado, e seu perfil não possui permissão para alteração após o 6º dia útil do mês subsequente.";
                    return;
                }

                if (!int.TryParse(hidAno.Value ?? "", out ano))
                {
                    lblMensagemCHNaoTrab.Text = "Erro ao carregar o popup de edição de CH não trabalhada. ano inválido.";
                    return;
                }

                if (!int.TryParse(hidMes.Value ?? "", out mes))
                {
                    lblMensagemCHNaoTrab.Text = "Erro ao carregar o popup de edição de CH não trabalhada. mês inválido.";
                    return;
                }

                if (!decimal.TryParse(hidNumFunc.Value ?? "", out numFunc))
                {
                    lblMensagemCHNaoTrab.Text = "Erro ao carregar o popup de edição de CH não trabalhada. hidNumFunc inválido.";
                    return;
                }

                if (string.IsNullOrEmpty(hidUnidadeEns.Value))
                {
                    lblMensagemCHNaoTrab.Text = "Erro ao carregar o popup de edição de CH não trabalhada. hidUnidadeEns inválido.";
                    return;
                }
                else
                {
                    unidadeEns = hidUnidadeEns.Value;
                }               

                if (!int.TryParse(hidCHMensal.Value ?? "", out chMensal))
                {
                    lblMensagemCHNaoTrab.Text = "Erro ao carregar o popup de edição de CH não trabalhada. hidCHMensal inválido.";
                    return;
                }

                int.TryParse(hidIDCargaHNaoTrabMes.Value ?? "", out idCargaHNaoTrabMes);

                if (string.IsNullOrEmpty(User.Identity.Name))
                {
                    lblMensagemCHNaoTrab.Text = "Erro ao carregar o popup de edição de CH não trabalhada. usuario inválido.";
                    return;
                }

                if (!int.TryParse(txtCHNaoTrab.Text ?? "", out chNaoTrabalhadaMes))
                {
                    lblMensagemCHNaoTrab.Text = "É necessário preencher o campo \"CH Não Trabalhada Mês\" com um número.";
                    return;
                }

                if (chNaoTrabalhadaMes > chMensal)
                {
                    lblMensagemCHNaoTrab.Text = "A \"CH Não Trabalhada Mês\" não pode ser superior a \"CH Mensal\".";
                    return;
                }

                if (chNaoTrabalhadaMes < 0)
                {
                    lblMensagemCHNaoTrab.Text = "A \"CH Não Trabalhada Mês\" não pode ser negativa.";
                    return;
                }

                if (idCargaHNaoTrabMes == 0)
                    rnCargaHNaoTrabMes.Insere(numFunc, unidadeEns, ano, mes, chMensal, null, chNaoTrabalhadaMes, User.Identity.Name);
                else
                    rnCargaHNaoTrabMes.Edita(idCargaHNaoTrabMes, chNaoTrabalhadaMes, User.Identity.Name);

                hidAno.Value = "";
                hidMes.Value = "";
                hidNumFunc.Value = "";
                hidUnidadeEns.Value = "";
                hidCHMensal.Value = "";
                hidIDCargaHNaoTrabMes.Value = "";
                hidMatricula.Value = string.Empty;
                hidNome.Value = string.Empty;
                txtCHNaoTrab.Text = "";
                chkTermoResponsabilidade.Checked = false;

                grdFrequencia.DataBind();
            }
            catch
            {
                throw;
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                RN.RecursosHumanos.ProtocoloRelHorasTrabalhadaGlp rnProtocoloRelHorasTrabalhadaGlp = new Techne.Lyceum.RN.RecursosHumanos.ProtocoloRelHorasTrabalhadaGlp();
                int ano;
                int mes;
                decimal? id_regional;
                string municipio;
                string faculdade;

                int.TryParse(odsFrequencia.SelectParameters["ano"].DefaultValue ?? "", out ano);
                int.TryParse(odsFrequencia.SelectParameters["mes"].DefaultValue ?? "", out mes);

                try
                {
                    id_regional = decimal.Parse(odsFrequencia.SelectParameters["id_regional"].DefaultValue) as decimal?;
                }
                catch
                {
                    id_regional = null;
                }

                municipio = (odsFrequencia.SelectParameters["municipio"].DefaultValue ?? "").Trim() != "" ? odsFrequencia.SelectParameters["municipio"].DefaultValue : null;
                faculdade = (odsFrequencia.SelectParameters["faculdade"].DefaultValue ?? "").Trim() != "" ? odsFrequencia.SelectParameters["faculdade"].DefaultValue : null;

                int protocolo = 0;
                protocolo = rnProtocoloRelHorasTrabalhadaGlp.GeraProtocolo(ano, mes, faculdade, User.Identity.Name);

                if (protocolo == 0)
                    throw new Exception("Número de Protocolo retornou zero");

                grdFrequencia.DataBind();

                var serverPath = ConfigurationManager.AppSettings["ServerPath"];
                var user = ConfigurationManager.AppSettings["CredentialUser"];
                var pwd = ConfigurationManager.AppSettings["CredentialPwd"];
                var domain = ConfigurationManager.AppSettings["CredentialDomain"];

                rvwFrequenciaGLP.ProcessingMode = ProcessingMode.Remote;

                ServerReport serverReport = rvwFrequenciaGLP.ServerReport;

                serverReport.ReportServerUrl = new Uri(serverPath);
                serverReport.ReportServerCredentials = new TechneReportCredentials(user, pwd, domain);
                serverReport.ReportPath = "/LYCEUM/QHI/GLP_Frequencia";

                var paramList = new List<ReportParameter>();

                paramList.Add(new ReportParameter("usuario", RN.Usuarios.BuscaNome(User.Identity.Name), false));
                paramList.Add(new ReportParameter("PROTOCOLO", protocolo.ToString(), false));

                serverReport.SetParameters(paramList);

                serverReport.Refresh();

                plaTela.Visible = false;
                plaRelatorio.Visible = true;
            }
            catch
            {
                throw;
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            plaTela.Visible = true;
            plaRelatorio.Visible = false;
            btnBuscar_Click(null, null);
        }

        protected void grdFrequencia_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.CellType != GridViewTableCommandCellType.Data)
                return;

            switch (e.ButtonID)
            {
                case "btnEditar":
                    DateTime mes_combo = new DateTime(int.Parse(ddlAno.SelectedValue), int.Parse(ddlMes.SelectedValue), 1);
                    DateTime mes_anterior = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    if (mes_combo <= mes_anterior || EhDiaPermitidoParaAlteracao)
                    {
                        if (btnFinalizar.Enabled)
                        {
                            if ((PossuiPerfilAlteracao || EhPrivilegiado))
                            {
                                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                            }
                        }
                        else
                        {

                            if ((PossuiPerfilAlteracao || EhPrivilegiado) || (EhDiaPermitidoParaAlteracao && hdnFinalizacao.Value.IsNullOrEmptyOrWhiteSpace()))
                            {
                                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                            }
                            else
                            {
                                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                            }
                        }
                    }

                    break;
            }
        }

        public bool EhDiaPermitidoParaAlteracao
        {
            get
            {
                RN.RecursosHumanos.PeriodoLancamentoFreqGLP rnPeriodoLancamentoFreqGLP = new Techne.Lyceum.RN.RecursosHumanos.PeriodoLancamentoFreqGLP();
                RN.RecursosHumanos.Entidades.PeriodoLancamentoFreqGLP periodoLancamentoFreqGLP = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PeriodoLancamentoFreqGLP();
                int ano;
                int mes;
                DateTime data_do_combo;
                DateTime primeiro_dia_do_mes_subsequente_ao_do_combo;
                DateTime sexto_dia_util_do_mes_subsequente_ao_do_combo;
                bool retorno = false;

                int.TryParse(ddlAno.SelectedValue ?? "", out ano);
                if (ano == 0)
                    ano = 1900;

                int.TryParse(ddlMes.SelectedValue ?? "", out mes);
                if (mes == 0)
                    mes = 1;

                //Busca periodo para lancamento
                periodoLancamentoFreqGLP = rnPeriodoLancamentoFreqGLP.ObtemPor(ano, mes);

                //Verifica se exite periodo para lancamento
                if (periodoLancamentoFreqGLP.PeriodoLancamentoFreqGLPId > 0)
                { 
                    //Caso exista verifica se esta no periodo
                    retorno = (DateTime.Now.Date >= periodoLancamentoFreqGLP.DataInicio &&
                          DateTime.Now.Date <= periodoLancamentoFreqGLP.DataFim);
                }
                else
                {
                    //Caso nao existe segue regra anterior

                    data_do_combo = new DateTime(ano, mes, 1);
                    primeiro_dia_do_mes_subsequente_ao_do_combo = data_do_combo.AddMonths(1);
                    sexto_dia_util_do_mes_subsequente_ao_do_combo = AddWorkdays(data_do_combo.AddMonths(1).AddDays(-1), 6);

                    retorno = (DateTime.Now.Date >= primeiro_dia_do_mes_subsequente_ao_do_combo &&
                            DateTime.Now.Date <= sexto_dia_util_do_mes_subsequente_ao_do_combo);
                }
                return retorno;
            }
        }

        public bool EhPrivilegiado
        {
            get
            {
                return new Techne.Lyceum.RN.Usuarios().EhPrivilegiado(User.Identity.Name);
            }
        }

        public bool PossuiPerfilAlteracao
        {
            get
            {
                return new RN.Perfil().PossuiPerfilAlteracaoFrequenciaGLPPor(User.Identity.Name);
            }
        }
               
        public QueryTable ListaAno()
        {
            try
            {
                return RN.PeriodoLetivo.ConsultarAno();
            }
            catch
            {
                return null;
                throw;
            }
        }

        public List<int> ListaMes()
        {
            try
            {
                return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            }
            catch
            {
                throw;
            }
        }

        public List<DadosFrequenciaGLP> ListaFrequencia(int ano, int mes,string periodo, decimal? id_regional, string municipio, string faculdade)
        {
            try
            {
                List<DadosFrequenciaGLP> result;
                string[] data = null;
                if (periodo != null)
                {
                    data = periodo.Split('-');
                }

                result = rnCargaHNaoTrabMes.ListaFrequenciaGLP(ano, mes, (data != null ? Convert.ToDateTime(data[0].Trim()) : DateTime.MinValue), (data != null ? Convert.ToDateTime(data[1].Trim()) : DateTime.MinValue), id_regional, municipio, faculdade);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public List<DadosTurmaGLP> ListaTurma(int ano, int mes, string faculdade, decimal num_func)
        {
            try
            {
                List<DadosTurmaGLP> result;
                result = rnCargaHNaoTrabMes.ListaTurmaGLP(ano, mes, faculdade, num_func);
                return result;
            }
            catch
            {
                throw;
            }
        }

        //https://stackoverflow.com/questions/4604461/c-sharp-datetime-to-add-subtract-working-days/4604631
        #region AddWorkdays

        public DateTime AddWorkdays(DateTime originalDate, int workDays)
        {
            if (string.IsNullOrEmpty((tseMunicipio.Value ?? "").ToString()))
            {
                DateTime tmpDate = originalDate;
                while (workDays > 0)
                {
                    tmpDate = tmpDate.AddDays(1);
                    if (tmpDate.DayOfWeek < DayOfWeek.Saturday && tmpDate.DayOfWeek > DayOfWeek.Sunday)
                        workDays--;
                }
                return tmpDate;
            }
            else
            {
                return new RN.Matriculas.DiasNaoLetivos().RetornaDataFinalPor(originalDate, workDays, tseMunicipio.Value.ToString());
            }
        }

        #endregion

        protected void chkTermoResponsabilidade_CheckedChanged(object sender, EventArgs e)
        {
            btnFinalizar.Enabled = false;
            if (chkTermoResponsabilidade.Checked)
            {
                btnFinalizar.Enabled = true;
            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime mes_combo = new DateTime(int.Parse(ddlAno.SelectedValue), int.Parse(ddlMes.SelectedValue), 1);
                DateTime mes_anterior = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                if (!EhDiaPermitidoParaAlteracao)
                {
                    if (mes_combo > mes_anterior)
                    {
                        lblMensagem.Text += lblMensagem.Text.Length > 0 ? "<br />" : "";
                        lblMensagem.Text += "O mês/ano informado não pode ser finalizado, pois ainda não terminou.";
                    }
                }

                if (lblMensagem.Text.Length > 0)
                    return;

                RN.RecursosHumanos.CargaHNaoTrabMesFinalizada rnCargaHNaoTrabMesFinalizada = new Techne.Lyceum.RN.RecursosHumanos.CargaHNaoTrabMesFinalizada();
                RN.RecursosHumanos.Entidades.CargaHNaoTrabMesFinalizada cargaHFinalizada = new Techne.Lyceum.RN.RecursosHumanos.Entidades.CargaHNaoTrabMesFinalizada();
                RN.RecursosHumanos.Entidades.CargaHNaoTrabMes cargaHoraria = new Techne.Lyceum.RN.RecursosHumanos.Entidades.CargaHNaoTrabMes();
                RN.RecursosHumanos.CargaHNaoTrabMesFinalizada rnCargaHFinalizada = new Techne.Lyceum.RN.RecursosHumanos.CargaHNaoTrabMesFinalizada();

                var lista = new List<Techne.Lyceum.RN.RecursosHumanos.Entidades.CargaHNaoTrabMes>();
                var erros = new List<string>();

                cargaHFinalizada.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                cargaHFinalizada.Mes = !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMes.SelectedValue) : -1;
                cargaHFinalizada.Censo = (tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                cargaHFinalizada.DataFinalizacao = DateTime.Now;
                cargaHFinalizada.UsuarioId = User.Identity.Name;

                for (var rowIndex = 0; rowIndex < this.grdFrequencia.VisibleRowCount; rowIndex++)
                {
                    cargaHoraria = new Techne.Lyceum.RN.RecursosHumanos.Entidades.CargaHNaoTrabMes();

                    var numFunc = this.grdFrequencia.GetRowValues(rowIndex, "NUM_FUNC").ToString();
                    var idCargaHNaoTrabMes = this.grdFrequencia.GetRowValues(rowIndex, "ID_CARGAHNAOTRABMES");
                    var chNaoTrabalhada = this.grdFrequencia.GetRowValues(rowIndex, "CHNAOTRABALHADAMES").ToString();
                    var chMensal = this.grdFrequencia.GetRowValues(rowIndex, "CH_MENSAL").ToString();


                    cargaHoraria.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                    cargaHoraria.Mes = !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMes.SelectedValue) : -1;
                    cargaHoraria.UnidadeEns = (tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                    cargaHoraria.NumFunc = !numFunc.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(numFunc) : -1;
                    cargaHoraria.IdCargaHNaoTrabMes = idCargaHNaoTrabMes != DBNull.Value ? Convert.ToInt32(idCargaHNaoTrabMes) : -1;
                    cargaHoraria.ChSemanal = null;
                    cargaHoraria.ChNaoTrabalhadaMes = !chNaoTrabalhada.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(chNaoTrabalhada) : -1;
                    cargaHoraria.ChMensal = !chMensal.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(chMensal) : -1;

                    cargaHoraria.UsuarioId = User.Identity.Name;

                    lista.Add(cargaHoraria);
                }

                rnCargaHNaoTrabMesFinalizada.Finaliza(cargaHFinalizada, lista);
                lblMensagem.Text = "Finalização realizada com sucesso.";

                string mensagemFinalizacao = string.Empty;
                if (rnCargaHFinalizada.PossuiFinalizacaoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), out mensagemFinalizacao))
                {
                    this.lblMensagemFinalizacao.Text = mensagemFinalizacao;
                    pnlRatificar.Visible = false;
                    btnImprimir.Visible = true;
                    btnFinalizar.Enabled = false;
                    chkTermoResponsabilidade.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}