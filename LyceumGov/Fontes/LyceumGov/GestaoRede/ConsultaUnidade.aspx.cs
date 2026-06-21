using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Controls;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Text;
using DevExpress.Web.ASPxTabControl;
using System.Data;
using Techne.Lyceum.RN;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.Data;
using System.IO;

namespace Techne.Lyceum.Net.GestaoRede
{
    [NavUrl("~/GestaoRede/ConsultaUnidade.aspx")]
    [ControlText("Consulta Dados Unidade")]
    [Title("Consulta Dados Unidade")]
    public partial class ConsultaUnidade : TPage
    {

        public enum TipoOperacao
        {
            Consultar,
            Inicial
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


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                MontarEquipamentos(null);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDiretor, "Equipe Técnico-Pedagógica");
        }

        protected void LimpaQuantitativos()
        {
            txtAlunos.Text = string.Empty;
            txtProfessores.Text = string.Empty;
            txtTurmas.Text = string.Empty;
            txtTurnos.Text = string.Empty;
        }
        protected void LimpaAbaCaracteristicasFisicas()
        {

            txtUnidadeFisica.Text = string.Empty;
            txtNomeComp.Text = string.Empty;
            txtCEP.Text = string.Empty;
            tsCEP.ResetValue();
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            ddlBairro.ClearSelection();
            ddlBairro.Items.Clear();
            txtEnd_Num.Text = string.Empty;
            txtEnd_Compl.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCGC.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtFone2.Text = string.Empty;
            txtFax.Text = string.Empty;
            chkAcessoDificil.Checked = false;
            chkRecursoAcessibilidade.ClearSelection();
            chkOcupacaoFormal.Checked = false;
            chkImovelCompartilhadoFisica.Checked = false;
            chkExtraclasse.Checked = false;
            rblAcessoNecEspecial.ClearSelection();
            pnlAcessibilidade.Visible = false;
            ddlTipo.ClearSelection();
            txtMunicipioFisica.Text = string.Empty;
            //chkAtivaDepen.Checked = true;
            ddlLocalFuncionamento.ClearSelection();
            rblLocalizacaoUF.ClearSelection();
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
            ddlDependenciaAdministrativa.ClearSelection();
            ddlDistritoFisica.ClearSelection();
            txtRegional.Text = string.Empty;
        }

        public void LimpaDadosInternet()
        {
            rblInternetBandaLarga.ClearSelection();
            chkAcessoInternet.ClearSelection();
            chkEquipamentoEscola.Checked = true;
            chkEquipamentoPessoal.Checked = false;
            chkRedeCabo.Checked = true;
            chkRedeWireless.Checked = false;
            chkSemRedeComputador.Checked = false;
        }

        protected void tseUnidade_Changed(object sender, EventArgs e)
        {
            try
            {
                pnlImprimir.Visible = false;
                divPrincipal.Visible = false;

                if (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
                {
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }
                else
                {
                    if (!tseUnidade.DBValue.IsNull)
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void MontaDadosInternet(string censo)
        {
            RN.DTOs.UnidadeDadosInternet unidadeDadosInternet = new UnidadeDadosInternet();
            RN.GestaoRede.UnidadeFisicaRedeInternet rnUnidadeFisicaRedeInternet = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaRedeInternet();

            unidadeDadosInternet = rnUnidadeFisicaRedeInternet.ObtemDadosInternetPor(censo);

            if (!unidadeDadosInternet.BandaLarga.IsNullOrEmptyOrWhiteSpace())
            {
                rblInternetBandaLarga.SelectedValue = unidadeDadosInternet.BandaLarga;
            }
            rblInternetBandaLarga_SelectedIndexChanged(null, null);

            foreach (var item in unidadeDadosInternet.AcessoInternet)
            {
                if (chkAcessoInternet.Items.FindByValue(item.ToString()) != null)
                {
                    chkAcessoInternet.Items.FindByValue(item.ToString()).Selected = true;
                }
            }

            chkRedeCabo.Checked = true;
            chkRedeWireless.Checked = false;
            chkSemRedeComputador.Checked = unidadeDadosInternet.RedeCabo == "N" && unidadeDadosInternet.RedeWireless == "N" ? true : false;
            chkSemRedeComputador_CheckedChanged(null, null);
            chkEquipamentoEscola.Checked = true;
            chkEquipamentoPessoal.Checked = false;
        }

        protected void rblInternetBandaLarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlDadosInternet.Visible = false;

                if (rblInternetBandaLarga.SelectedValue == "S")
                {
                    pnlDadosInternet.Visible = true;

                    //Volta para valores default
                    chkEquipamentoEscola.Checked = true;
                    chkEquipamentoPessoal.Checked = false;

                    chkRedeCabo.Checked = true;
                    chkRedeWireless.Checked = false;
                    chkSemRedeComputador.Checked = false;
                }
                else
                {
                    chkAcessoInternet.ClearSelection();

                    chkEquipamentoEscola.Checked = false;
                    chkEquipamentoPessoal.Checked = false;

                    chkRedeCabo.Checked = false;
                    chkRedeWireless.Checked = false;
                    chkSemRedeComputador.Checked = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void MontarEquipamentos(List<DadosEquipamentoUnidadeFisica> listaEquipamentos)
        {
            RN.EquipamentoUnidadeFisica rnEquipamentoUnidadeFisica = new EquipamentoUnidadeFisica();

            try
            {
                MasterPage ctl00 = FindControl("ctl00") as MasterPage;
                ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;
                // ASPxPageControl control = MainContent.FindControl("pcUnidade") as ASPxPageControl;
                Table dTable = MainContent.FindControl("tblEquipamentos") as Table;


                //Verifica se equipamentes já estão montados na tela
                if (dTable != null)
                {
                    return;
                }

                if ((listaEquipamentos == null || listaEquipamentos.Count == 0) && !tseUnidade.DBValue.IsNull)
                {
                    listaEquipamentos = rnEquipamentoUnidadeFisica.ObtemListaPor(tseUnidade.DBValue.ToString());
                }

                dTable = new Table();
                dTable.ID = "tblEquipamentos";

                if (listaEquipamentos != null)
                {
                    for (int i = 0; i <= listaEquipamentos.Count; i++)
                    {
                        TableRow dTRow = new TableRow();

                        for (int f = 0; f < 3; f++)
                        {
                            if ((i + f) < listaEquipamentos.Count)
                            {
                                DadosEquipamentoUnidadeFisica item = listaEquipamentos.ElementAt(i + f);
                                if (item.Descricao.ToUpper() != "INTERNET BANDA LARGA")
                                {
                                    TextBox txtQtdEquipamento = new TextBox();
                                    txtQtdEquipamento.Width = 20;
                                    txtQtdEquipamento.Enabled = false;
                                    txtQtdEquipamento.ID = "txtQtd" + Convert.ToString(item.IdEquipamento);
                                    txtQtdEquipamento.Text = Convert.ToString(item.Quantidade);
                                    txtQtdEquipamento.SkinID = "numerico";
                                    txtQtdEquipamento.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                    txtQtdEquipamento.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                    txtQtdEquipamento.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                    Label lblEquipamento = new Label();
                                    lblEquipamento.Enabled = false;
                                    lblEquipamento.Text = Convert.ToString(item.Descricao);

                                    HiddenField hdnEquipamento = new HiddenField();
                                    hdnEquipamento.Value = Convert.ToString(item.IdEquipamento);
                                    hdnEquipamento.ID = "hdn" + Convert.ToString(item.IdEquipamento);

                                    TableCell dTCell = new TableCell();
                                    dTCell.Controls.Add(txtQtdEquipamento);
                                    dTCell.Controls.Add(lblEquipamento);
                                    dTCell.Controls.Add(hdnEquipamento);
                                    dTRow.Controls.Add(dTCell);
                                }
                            }
                        }
                        i += 2;
                        dTable.Controls.Add(dTRow);
                    }
                    pnlEquipamentos.Controls.Add(dTable);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            RN.DTOs.UnidadeCaracteristicasFisicas infoFisicas = new UnidadeCaracteristicasFisicas();
            RN.UnidadeFisica rnUnidadeFisica = new Techne.Lyceum.RN.UnidadeFisica();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            DataTable dtDadosEscola = new DataTable();

            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        LimpaAbaCaracteristicasFisicas();
                        LimpaDadosInternet();
                        LimpaQuantitativos();
                        grdDiretor.DataBind();
                        CarregaAcessoInternet();
                        CarregaDependenciaAdm();
                        CarregaFormaOcupacao();
                        CarregaFormasAcessibilidade();
                        CarregaLocalFuncionamento();
                        pnlImprimir.Visible = false;
                        btnEncontraNoMapa.Visible = false;

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        LimpaAbaCaracteristicasFisicas();
                        LimpaDadosInternet();
                        LimpaQuantitativos();
                        btnEncontraNoMapa.Visible = true;

                        HabilitaDesabilitaCamposAbaCaracteristicasFisicas(false);
                        infoFisicas = rnUnidadeFisica.ObtemCaracteristicasFisicasPor(tseUnidade.DBValue.ToString());

                        if (!infoFisicas.UnidadeFisica.IsNullOrEmptyOrWhiteSpace())
                        {
                            txtUnidadeFisica.Text = infoFisicas.UnidadeFisica;
                            txtNomeComp.Text = !infoFisicas.NomeUnidadeFisica.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.NomeUnidadeFisica : string.Empty;
                            txtCEP.Text = !infoFisicas.Cep.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Cep : string.Empty;
                            tsCEP.DBValue = txtCEP.Text;
                            txtEndereco.Text = !infoFisicas.Endereco.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Endereco : string.Empty;
                            txtEnd_Num.Text = !infoFisicas.EnderecoNumero.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.EnderecoNumero : string.Empty;
                            txtEnd_Compl.Text = !infoFisicas.EnderecoComplemento.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.EnderecoComplemento : string.Empty;
                            txtMunicipioFisica.Text = !infoFisicas.MunicipioDescricao.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.MunicipioDescricao : string.Empty;
                            txtLatitude.Text = !infoFisicas.Latitude.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Latitude : string.Empty;
                            txtLongitude.Text = !infoFisicas.Longitude.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Longitude : string.Empty;
                            hdnCodMunicipioFisica.Value = !infoFisicas.Municipio.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Municipio : string.Empty;
                            txtRegional.Text = !infoFisicas.Regional.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Regional : string.Empty;

                            if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                this.CarregaBairro(hdnCodMunicipioFisica.Value);
                                string bairro = !infoFisicas.EnderecoBairro.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.EnderecoBairro : string.Empty;
                                if (ddlBairro.Items.FindByValue(bairro) != null)
                                {
                                    ddlBairro.SelectedValue = bairro;
                                }
                            }

                            txtEstado.Value = !infoFisicas.UF.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.UF : string.Empty;
                            txtEmail.Text = !infoFisicas.Email.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Email : string.Empty;
                            txtCGC.Text = !infoFisicas.Cnpj.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Cnpj : string.Empty;
                            txtFone.Text = !infoFisicas.Telefone1.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Telefone1 : string.Empty;
                            txtFone2.Text = !infoFisicas.Telefone2.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Telefone2 : string.Empty;
                            txtFax.Text = !infoFisicas.Fax.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Fax : string.Empty;
                            chkAcessoDificil.Checked = !infoFisicas.AcessoDificil.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AcessoDificil == "S" ? true : false) : false;
                            chkOcupacaoFormal.Checked = !infoFisicas.OcupacaoFormal.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.OcupacaoFormal == "S" ? true : false) : false;
                            chkImovelCompartilhadoFisica.Checked = !infoFisicas.ImovelCompartilhadoRede.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.ImovelCompartilhadoRede == "S" ? true : false) : false;
                            chkExtraclasse.Checked = !infoFisicas.Extraclasse.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.Extraclasse == "S" ? true : false) : false;
                            ddlTipo.SelectedValue = !infoFisicas.FormaOcupacaoTipo.IsNullOrEmptyOrWhiteSpace() ? (ddlTipo.Items.FindByValue(infoFisicas.FormaOcupacaoTipo) != null ? infoFisicas.FormaOcupacaoTipo : string.Empty) : string.Empty;
                            chkAreaAssentamento.Checked = !infoFisicas.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AreaAssentamento == "S" ? true : false) : false;
                            chkTerraIndigena.Checked = !infoFisicas.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.TerraIndigena == "S" ? true : false) : false;
                            chkQuilombos.Checked = !infoFisicas.AreaQuilombo.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AreaQuilombo == "S" ? true : false) : false;
                            ddlDependenciaAdministrativa.SelectedValue = !infoFisicas.DependenciaAdministrativa.IsNullOrEmptyOrWhiteSpace() ? (ddlDependenciaAdministrativa.Items.FindByValue(infoFisicas.DependenciaAdministrativa) != null ? infoFisicas.DependenciaAdministrativa : string.Empty) : string.Empty;
                            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked) ? true : false;

                            pnlAcessibilidade.Visible = false;
                            if (infoFisicas.AcessoNecessidadeEspecial.IsNullOrEmptyOrWhiteSpace())
                            {
                                rblAcessoNecEspecial.ClearSelection();
                            }
                            else
                            {
                                rblAcessoNecEspecial.SelectedValue = infoFisicas.AcessoNecessidadeEspecial;

                                if (rblAcessoNecEspecial.SelectedValue == "S")
                                {
                                    pnlAcessibilidade.Visible = true;

                                    foreach (var linha in infoFisicas.FormasAcessibilidade)
                                    {
                                        if (chkRecursoAcessibilidade.Items.FindByValue(linha.ToString()) != null)
                                        {
                                            chkRecursoAcessibilidade.Items.FindByValue(linha.ToString()).Selected = true;
                                        }
                                    }
                                }
                            }

                            if (!infoFisicas.FormaOcupacaoLocalizacao.IsNullOrEmptyOrWhiteSpace())
                            {
                                if (rblLocalizacaoUF.Items.FindByValue(infoFisicas.FormaOcupacaoLocalizacao) != null)
                                {
                                    rblLocalizacaoUF.SelectedValue = infoFisicas.FormaOcupacaoLocalizacao;
                                }
                            }

                            if (!infoFisicas.LocalFuncionamento.IsNullOrEmptyOrWhiteSpace())
                            {
                                if (ddlLocalFuncionamento.Items.FindByValue(infoFisicas.LocalFuncionamento) != null)
                                {
                                    ddlLocalFuncionamento.SelectedValue = infoFisicas.LocalFuncionamento;
                                }
                            }

                            if (!infoFisicas.Distrito.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtEnd_Num_TextChanged(null, null);
                                if (ddlDistritoFisica.Items.FindByValue(infoFisicas.Distrito) != null)
                                {
                                    ddlDistritoFisica.SelectedValue = infoFisicas.Distrito;
                                }
                            }
                        }

                        List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupoConsideracao = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                        RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();

                        int campanha = rnCampanhaEscola.ObtemUltimaCampanhaFinalizadaPor(tseUnidade.DBValue.ToString());

                        if (campanha > 0)
                        {
                            //GRUPO 3 ABA1 
                            lsGrupoConsideracao.Clear();
                            lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(campanha, 3, 1, 11);

                            rpdemaisdependenciasGrupoG3A1.DataSource = lsGrupoConsideracao;
                            rpdemaisdependenciasGrupoG3A1.DataBind();

                            //GRUPO 3 ABA2 - pego da 12 até o final
                            lsGrupoConsideracao.Clear();
                            lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(campanha, 3, 12, 0);

                            rpdemaisdependenciasGrupoG3A2.DataSource = lsGrupoConsideracao;
                            rpdemaisdependenciasGrupoG3A2.DataBind();


                            ManterTecnologia(campanha);
                        }

                        MontaDadosInternet(tseUnidade.DBValue.ToString());
                        dtDadosEscola = rnUnidadeEnsino.ObtemDadosEscolaPor(tseUnidade.DBValue.ToString());

                        if (dtDadosEscola.Rows.Count > 0)
                        {
                            txtAlunos.Text = dtDadosEscola.Rows[0]["TOTAL_ALUNOS"].ToString();
                            txtProfessores.Text = dtDadosEscola.Rows[0]["TOTAL_DOCENTES"].ToString();
                            txtTurmas.Text = dtDadosEscola.Rows[0]["TOTAL_TURMAS"].ToString();
                            txtTurnos.Text = dtDadosEscola.Rows[0]["TURNOS_COM_TURMA_ATIVA"].ToString();
                        }
                        divPrincipal.Visible = true;
                        pnlImprimir.Visible = true;


                        break;
                    }

            }
        }

        protected void txtEnd_Num_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlDistritoFisica.Items.Clear();
                    ddlDistritoFisica.DataSource = RN.Distrito.Listar(hdnCodMunicipioFisica.Value);
                    ddlDistritoFisica.DataBind();
                    ddlDistritoFisica.Items.Insert(0, new ListItem("Selecione", string.Empty));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void ManterTecnologia(int campanha)
        {
            #region GRUPO3

            RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
            List<RN.InspecaoEscolar.Entidades.RespostaAssunto> lsRespostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();


            //GRUPO3 - ABA1
            foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG3A1.Items)
            {
                Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG3A1");

                foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                {
                    lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(campanha, tseUnidade.DBValue.ToString());

                    if (lsRespostaAssunto.Count > 0)
                    {

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG3A1");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        int assunto = Convert.ToInt32(valores[0]);
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);

                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG3A1");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG3A1");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG3A1");

                                string acao = lsRespostaAssunto.Where(x => x.OpcoesAssuntoId == Convert.ToUInt32(hdnOPTipoAssunto.Value)).Select(x => x.AcaoDirecaoId.Value).FirstOrDefault().ToString();
                                drop.SelectedValue = acao;
                            }

                        }
                        else
                        {
                            //2	MULTIPLA ESCOLHA COM 1 OPÇÃO
                            //3	MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES
                            //4	DESCRITIVA
                            //5	SEM RESPOSTA
                            switch (Convert.ToInt32(tipo))
                            {
                                case 2:

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG3A1");

                                    foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                    {
                                        rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                    }

                                    break;

                                case 3:

                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG3A1");

                                    foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                    {
                                        var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                        chk.Items.FindByValue(valor).Selected = true;
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG3A1");
                                    txtResposta.Text = lsRespostaAssunto.Where(x => x.AssuntoId == assunto).Select(x => x.Descricao).FirstOrDefault();
                                    break;

                                case 5:
                                default:
                                    break;
                            }
                        }
                    }

                }

            }
            //GRUPO3 -  ABA1 

            //GRUPO3 - ABA2
            foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG3A2.Items)
            {
                Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG3A2");

                foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                {
                    lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(campanha, tseUnidade.DBValue.ToString());

                    if (lsRespostaAssunto.Count > 0)
                    {

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG3A2");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        int assunto = Convert.ToInt32(valores[0]);
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);

                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG3A2");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG3A2");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG3A2");

                                string acao = lsRespostaAssunto.Where(x => x.OpcoesAssuntoId == Convert.ToUInt32(hdnOPTipoAssunto.Value)).Select(x => x.AcaoDirecaoId.Value).FirstOrDefault().ToString();
                                drop.SelectedValue = acao;
                            }

                        }
                        else
                        {
                            //2	MULTIPLA ESCOLHA COM 1 OPÇÃO
                            //3	MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES
                            //4	DESCRITIVA
                            //5	SEM RESPOSTA
                            switch (Convert.ToInt32(tipo))
                            {
                                case 2:

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG3A2");

                                    foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                    {
                                        rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                    }

                                    break;

                                case 3:

                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG3A2");

                                    foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                    {
                                        var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                        chk.Items.FindByValue(valor).Selected = true;
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG3A2");
                                    txtResposta.Text = lsRespostaAssunto.Where(x => x.AssuntoId == assunto).Select(x => x.Descricao).FirstOrDefault();
                                    break;

                                case 5:
                                default:
                                    break;
                            }
                        }
                    }

                }

            }
            //GRUPO3 -  ABA2 



            #endregion


        }

        protected void rpdemaisdependenciasGrupoG3A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG3A1");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG3A1");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 1, 8);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG3A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;


            try
            {

                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;


                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {

                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG3A1");



                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG3A1");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG3A1");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG3A1");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG3A1");

                        lbl.Visible = true;
                        txt.Visible = false;
                        rbl.Visible = false;
                        chk.Visible = false;

                        switch (Convert.ToInt32(tipo))
                        {
                            case 2:
                                rbl.Visible = true;
                                txt.Visible = false;
                                chk.Visible = false;
                                break;

                            case 3:
                                chk.Visible = true;
                                txt.Visible = false;
                                rbl.Visible = false;
                                break;

                            case 4:
                                txt.Visible = true;
                                rbl.Visible = false;
                                chk.Visible = false;
                                break;

                            case 5:
                                lbl.Visible = true;
                                break;
                        }

                        if (acodirecao)
                        {
                            txt.Visible = false;
                            rbl.Visible = false;
                            chk.Visible = false;

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG3A1");

                            acaoRepeater.DataSource = assunto2.ListaOpcao;
                            acaoRepeater.DataBind();


                        }

                        if (chk != null)
                        {
                            var ds = chk.DataSource as List<RN.DTOs.DadosRelatorioInspecaoOpcao>;
                            var restritivo = ds.FirstOrDefault(q => q.Restritivo);
                            if (restritivo != null)
                            {
                                var id = restritivo.valor;
                                foreach (ListItem lst in chk.Items)
                                {
                                    if (lst.Value == id)
                                    {
                                        lst.Attributes["onclick"] = "desmarcarTodosEmDemaisDependenciasParaCheckbox(this);";
                                        lst.Attributes["data-nao-se-aplica"] = "1";
                                    }
                                    else
                                    {
                                        lst.Attributes["onclick"] = "desmarcarNaoSeAplicaEmCheckbox(this);";
                                        lst.Attributes["data-nao-se-aplica"] = "0";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lblMensagem.Text = "Erro ao salvar!";
            }
        }

        protected void rpAcaodeDirecaoG3A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG3A1");
            if (drop == null)
                return;

            RN.DTOs.DadosRelatorioInspecaoOpcao dataItem = e.Item.DataItem as RN.DTOs.DadosRelatorioInspecaoOpcao;
            if (dataItem == null)
                return;

            if (dataItem.Restritivo)
            {
                drop.Attributes["onchange"] = "desmarcarTodosEmDemaisDependencias(this)";
                drop.Attributes["data-nao-se-aplicaa"] = "1";
                drop.Items.Clear();
                drop.Items.Add(new ListItem { Text = "Selecione", Value = "-1" });
                drop.Items.Add(new ListItem { Text = "Não se Aplica", Value = "5" });
                drop.Items.Add(new ListItem { Text = "Espaço Inexistente", Value = "6" });
            }
            else
            {
                drop.Attributes["onchange"] = "desmarcarNaoSeAplicaEmDemaisDependencias(this)";

            }
        }

        protected void chkRespostaDemaisG3A1_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void rpdemaisdependenciasGrupoG3A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG3A2");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG3A2");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 9, 0);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG3A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;


            try
            {

                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;


                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {

                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG3A2");



                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG3A2");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG3A2");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG3A2");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG3A2");

                        lbl.Visible = true;
                        txt.Visible = false;
                        rbl.Visible = false;
                        chk.Visible = false;

                        switch (Convert.ToInt32(tipo))
                        {
                            case 2:
                                rbl.Visible = true;
                                txt.Visible = false;
                                chk.Visible = false;
                                break;

                            case 3:
                                chk.Visible = true;
                                txt.Visible = false;
                                rbl.Visible = false;
                                break;

                            case 4:
                                txt.Visible = true;
                                rbl.Visible = false;
                                chk.Visible = false;
                                break;

                            case 5:
                                lbl.Visible = true;
                                break;
                        }

                        if (acodirecao)
                        {
                            txt.Visible = false;
                            rbl.Visible = false;
                            chk.Visible = false;

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG3A2");

                            acaoRepeater.DataSource = assunto2.ListaOpcao;
                            acaoRepeater.DataBind();


                        }

                        if (chk != null)
                        {
                            var ds = chk.DataSource as List<RN.DTOs.DadosRelatorioInspecaoOpcao>;
                            var restritivo = ds.FirstOrDefault(q => q.Restritivo);
                            if (restritivo != null)
                            {
                                var id = restritivo.valor;
                                foreach (ListItem lst in chk.Items)
                                {
                                    if (lst.Value == id)
                                    {
                                        lst.Attributes["onclick"] = "desmarcarTodosEmDemaisDependenciasParaCheckbox(this);";
                                        lst.Attributes["data-nao-se-aplica"] = "1";
                                    }
                                    else
                                    {
                                        lst.Attributes["onclick"] = "desmarcarNaoSeAplicaEmCheckbox(this);";
                                        lst.Attributes["data-nao-se-aplica"] = "0";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lblMensagem.Text = "Erro ao salvar!";
            }
        }

        protected void rpAcaodeDirecaoG3A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG3A2");
            if (drop == null)
                return;

            RN.DTOs.DadosRelatorioInspecaoOpcao dataItem = e.Item.DataItem as RN.DTOs.DadosRelatorioInspecaoOpcao;
            if (dataItem == null)
                return;

            if (dataItem.Restritivo)
            {
                drop.Attributes["onchange"] = "desmarcarTodosEmDemaisDependencias(this)";
                drop.Attributes["data-nao-se-aplicaa"] = "1";
                drop.Items.Clear();
                drop.Items.Add(new ListItem { Text = "Selecione", Value = "-1" });
                drop.Items.Add(new ListItem { Text = "Não se Aplica", Value = "5" });
                drop.Items.Add(new ListItem { Text = "Espaço Inexistente", Value = "6" });
            }

            else
            {
                drop.Attributes["onchange"] = "desmarcarNaoSeAplicaEmDemaisDependencias(this)";

            }
        }

        protected void chkRespostaDemaisG3A2_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;

        }

        protected void HabilitaDesabilitaCamposAbaCaracteristicasFisicas(bool habilita)
        {
            txtUnidadeFisica.Enabled = false;
            txtNomeComp.Enabled = habilita;
            txtCEP.Enabled = habilita;
            tsCEP.ShowButton = habilita;
            txtEstado.Disabled = !habilita;
            txtEndereco.Enabled = habilita;
            txtLatitude.Enabled = habilita;
            txtLongitude.Enabled = habilita;
            ddlBairro.Enabled = habilita;
            txtEnd_Num.Enabled = habilita;
            txtEnd_Compl.Enabled = habilita;
            txtEmail.Enabled = habilita;
            txtCGC.Enabled = habilita;
            txtFone.Enabled = habilita;
            txtFone2.Enabled = habilita;
            txtFax.Enabled = habilita;
            chkAcessoDificil.Enabled = habilita;
            chkRecursoAcessibilidade.Enabled = habilita;
            chkOcupacaoFormal.Enabled = habilita;
            chkImovelCompartilhadoFisica.Enabled = habilita;
            chkExtraclasse.Enabled = habilita;
            rblAcessoNecEspecial.Enabled = habilita;
            ddlTipo.Enabled = habilita;
            txtMunicipioFisica.Enabled = habilita;
            ddlLocalFuncionamento.Enabled = habilita;
            rblLocalizacaoUF.Enabled = habilita;
            chkAreaAssentamento.Enabled = habilita;
            chkTerraIndigena.Enabled = habilita;
            chkQuilombos.Enabled = habilita;
            chkNaoSeAplica.Enabled = habilita;
            ddlDependenciaAdministrativa.Enabled = habilita;
            ddlDistritoFisica.Enabled = habilita;
            txtRegional.Enabled = habilita;
        }

        protected void chkSemRedeComputador_CheckedChanged(object sender, EventArgs e)
        {
            ValidaRedeInterligacao();
        }

        private void ValidaRedeInterligacao()
        {
            if (chkSemRedeComputador.Checked)
            {
                chkRedeCabo.Checked = !chkSemRedeComputador.Checked;
                chkRedeWireless.Checked = !chkSemRedeComputador.Checked;

                chkRedeCabo.Enabled = !chkSemRedeComputador.Checked;
                chkRedeWireless.Enabled = !chkSemRedeComputador.Checked;

            }
            else
            {
                HabilitaRedeInterligacao();
            }

        }

        private void HabilitaRedeInterligacao()
        {
            if (!chkSemRedeComputador.Checked)
            {
                Util.Utils.HabilitaDesabilitaControlesWeb(
                    new WebControl[] {
                        chkRedeCabo, chkRedeWireless
                    }, true
                );
            }

            chkSemRedeComputador.Enabled = true;
        }

        protected void CarregaBairro(string municipioId)
        {
            RN.Bairro rnBairro = new Techne.Lyceum.RN.Bairro();
            ddlBairro.Items.Clear();
            ddlBairro.DataSource = rnBairro.ListaPor(municipioId);
            ddlBairro.DataBind();
            ddlBairro.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void ddlLocalFuncionamento_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocalFuncionamento.SelectedValue == "PredioEscolar")
            {
                lblTipo.Text = "Forma de Ocupação*:";
                lblTipo.Font.Bold = true;
            }
            else
            {
                lblTipo.Text = "Forma de Ocupação:";
                lblTipo.Font.Bold = false;
            }
        }

        protected void rblAcessoNecEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!rblAcessoNecEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() && rblAcessoNecEspecial.SelectedValue == "S")
            {
                pnlAcessibilidade.Visible = true;
                chkRecursoAcessibilidade.Enabled = true;
            }
            else
            {
                pnlAcessibilidade.Visible = false;
                chkRecursoAcessibilidade.Enabled = false;
                chkRecursoAcessibilidade.ClearSelection();
            }
        }
        protected void chkAcessoInternet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in chkAcessoInternet.Items)
                {
                    if (item.Selected && item.Value == "5") //Não possui
                    {
                        chkAcessoInternet.ClearSelection();
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
        protected void chkNaoSeAplica_CheckedChanged(object sender, EventArgs e)
        {
            ValidaLocalizacaoDiferenciada();
        }

        private void ValidaLocalizacaoDiferenciada()
        {
            if (chkNaoSeAplica.Checked)
            {
                chkQuilombos.Checked = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Checked = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Checked = !chkNaoSeAplica.Checked;

                chkQuilombos.Enabled = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Enabled = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Enabled = !chkNaoSeAplica.Checked;
            }
            else
            {
                HabilitaLocalizacaoDiferenciada();
            }
        }
        private void HabilitaLocalizacaoDiferenciada()
        {
            if (!chkNaoSeAplica.Checked)
            {
                Util.Utils.HabilitaDesabilitaControlesWeb(
                    new WebControl[] {
                        chkQuilombos, chkTerraIndigena, chkAreaAssentamento
                    }, true
                );
            }

            chkNaoSeAplica.Enabled = true;
        }

        protected void grdDiretor_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "fone" && e.Value != null)
            {
                decimal fone;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }

            if (e.Column.FieldName == "celular" && e.Value != null)
            {
                decimal celular;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out celular))
                {
                    if (e.Value.ToString().Replace(" ", string.Empty).Length == 10)
                    {
                        e.DisplayText = string.Format("{0:(00)0000-0000}", celular);
                    }
                    if (e.Value.ToString().Replace(" ", string.Empty).Length == 11)
                    {
                        e.DisplayText = string.Format("{0:(00)00000-0000}", celular);
                    }

                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        private void CarregaFormasAcessibilidade()
        {
            RN.GestaoRede.FormasAcessibilidade rnFormasAcessibilidade = new Techne.Lyceum.RN.GestaoRede.FormasAcessibilidade();

            chkRecursoAcessibilidade.Items.Clear();
            chkRecursoAcessibilidade.DataSource = rnFormasAcessibilidade.ListaFormasAcessibilidadeAtivo();
            chkRecursoAcessibilidade.DataTextField = "DESCRICAO";
            chkRecursoAcessibilidade.DataValueField = "FORMASACESSIBILIDADEID";
            chkRecursoAcessibilidade.DataBind();
        }

        protected void CarregaLocalFuncionamento()
        {
            Techne.Lyceum.RN.TabelaGeral rnTabelaGeral = new TabelaGeral();

            ddlLocalFuncionamento.Items.Clear();
            ddlLocalFuncionamento.DataSource = rnTabelaGeral.SelecionarItensLocalFuncionamento("LocalFuncionamentoUF");
            ddlLocalFuncionamento.DataBind();
            ddlLocalFuncionamento.Items.Insert(0, new ListItem("Selecione", string.Empty));

        }

        protected void CarregaFormaOcupacao()
        {
            ddlTipo.Items.Clear();
            ddlTipo.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("Tipo Unidade Física");
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void CarregaDependenciaAdm()
        {
            ddlDependenciaAdministrativa.Items.Clear();
            ddlDependenciaAdministrativa.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("Dependência Adm");
            ddlDependenciaAdministrativa.DataBind();
            ddlDependenciaAdministrativa.Items.Insert(0, new ListItem("Selecione", string.Empty));

        }
        private void CarregaAcessoInternet()
        {
            RN.GestaoRede.AcessoInternet rnAcessoInternet = new Techne.Lyceum.RN.GestaoRede.AcessoInternet();

            chkAcessoInternet.Items.Clear();
            chkAcessoInternet.DataSource = rnAcessoInternet.ListaAcessoInternetAtivo();
            chkAcessoInternet.DataTextField = "DESCRICAO";
            chkAcessoInternet.DataValueField = "ACESSOINTERNETID";
            chkAcessoInternet.DataBind();
        }
    }
}
