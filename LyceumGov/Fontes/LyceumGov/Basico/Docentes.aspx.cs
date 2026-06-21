


namespace Techne.Lyceum.Net.Basico
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxGridView;
    using Techne.Data;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using Techne.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using DevExpress.Web.ASPxEditors;
    using Techne.Controls;
    using DevExpress.Web.ASPxClasses;
    using System.Text;
    using System.Configuration;
    using System.Reflection;

    [NavUrl("~/Basico/Docentes.aspx")]
    [ControlText("Docentes")]
    [Title("Docentes")]

    public partial class Docentes : TPage
    {
        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Novo,
            Alterar,
            ConsultarDocente,
            ConsultarPessoa,
            Inicial,
            Sucesso
        }



        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        private TipoOperacao _tipoOperacaoRel
        {
            get
            {
                if (ViewState["_tipoOperacaoRel"] != null)
                {
                    if (ViewState["_tipoOperacaoRel"] is TipoOperacao)
                        return (TipoOperacao)ViewState["_tipoOperacaoRel"];
                }

                return TipoOperacao.Inicial;
            }
            set { ViewState["_tipoOperacaoRel"] = value; }
        }

        #endregion

        //usado no editar string ID_FORMACAO_PESSOAL
        private string ID_FORMACAO_PESSOAL = null;
        public static string GetUrl()
        {
            return Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        }

        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        #region Eventos

        //Eventos da Página:
        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdFormacaoPessoal, "Formação Pessoal");
            TituloGrid(grdCapacitacao, "Capacitação Profissional");
            TituloGrid(grdDisciplinaAdicional, "Disciplinas Adicionais");
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            var startIndexOnPage = this.grdFormacaoPessoal.PageIndex * this.grdFormacaoPessoal.SettingsPager.PageSize;
            var selectedRow = -1;

            for (var i = 0; i < this.grdFormacaoPessoal.VisibleRowCount; i++)
            {
                if (this.grdFormacaoPessoal.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    selectedRow = startIndexOnPage + i;

                    break;
                }
            }

            this.grdFormacaoPessoal.Selection.UnselectAll();
            return selectedRow;
        }

        private bool validaCheckAdicional()
        {
            bool tem = false;

            string nomeEscolaridade = ddlEscolaridade.SelectedValue.ToString().Trim();

            int posadicional = nomeEscolaridade.ToUpper().IndexOf("ADICIONAIS");

            if (posadicional >= 0)
            {
                for (int i = 0; i < chkListDisciplinaAdicional.Items.Count; i++)
                {
                    var checado = chkListDisciplinaAdicional.Items[i].Selected;
                    if (checado == true)
                    {
                        tem = true;
                    }
                }

                if (tem == false)
                {
                    lblMensValidacao.Text = "Estudo Adicional - Não foi escolhido nenhuma disciplina.";
                }
            }

            return tem;
        }

        protected void percorreIncluirCheckAdicional()
        {
            TceFormacaoEstudoAdicional tfea;
            List<TceFormacaoEstudoAdicional> selecao = new List<TceFormacaoEstudoAdicional>();

            if (string.IsNullOrEmpty(txtFormacaoPessoalID.Text) == false)
            {
                for (int i = 0; i < chkListDisciplinaAdicional.Items.Count; i++)
                {
                    var checado = chkListDisciplinaAdicional.Items[i].Selected;
                    if (checado == true)
                    {
                        tfea = new TceFormacaoEstudoAdicional();
                        tfea.EstudoAdicionalID = int.Parse(chkListDisciplinaAdicional.Items[i].Value);
                        tfea.FormacaoPessoalID = int.Parse(txtFormacaoPessoalID.Text);
                        selecao.Add(tfea);
                    }
                }
            }

            for (int j = 0; j < selecao.Count; j++)
            {
                int estudoAdicionalID = selecao[j].EstudoAdicionalID;

                if (RN.FormacaoPessoal.IncluirFormacaoPessoalAdicional(selecao[j]) > 0)
                {
                    // throw new Exception("Disciplina deletada com sucesso !");
                }
            }
        }

        protected void grdFormacaoPessoal_SelectionChanged(object sender, EventArgs e)
        {

        }
        protected void UpdatePanel1_Unload(object sender, EventArgs e)
        {
            MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page),
                new object[] { sender as UpdatePanel });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    AdicionaAtributosCampos();
                    rbtListOferecidoSEEDUC.SelectedValue = "1";
                    rbtListOferecidoSEEDUC_SelectedIndexChanged(sender, e);

                    _tipoOperacao = TipoOperacao.Inicial;
                    _tipoOperacaoRel = TipoOperacao.Inicial;
                    ControlarTipoOperacao();

                    ddlAreaCurso.Items.Clear();
                    ddlAreaCurso.DataSource = RN.AreaFormacaoPessoal.ListarAreas();
                    ddlAreaCurso.Items.Insert(0, "Selecione");
                    ddlAreaCurso.DataBind();

                    PreecherComboTabGeral(ddlTipoInstituicao, "TipoInstituicao");
                    PreecherComboTabGeralFiltro(ddlEscolaridade, "EscolaridadeFormacao", "", "Pós-", "Superior");
                    PreecherComboTabGeral(ddlSituacaoCurso, "SituacaoCursoForm");
                    PreecherComboTabGeral(ddlFormComplementPedag, "FormacaoComplement");

                    ddlAreaCursoPosGraduacao.Items.Clear();
                    ddlAreaCursoPosGraduacao.DataSource = RN.AreaFormacaoPessoal.ListarAreas();
                    ddlAreaCursoPosGraduacao.Items.Insert(0, "Selecione");
                    ddlAreaCursoPosGraduacao.DataBind();

                    PreecherComboTabGeral(ddlTipoInstituicaoPosGraduacao, "TipoInstituicao");
                    PreecherComboTabGeralFiltro(ddlEscolaridadePosGraduacao, "EscolaridadeFormacao", "Pós-", "-1", "Superior");
                    PreecherComboTabGeral(ddlSituacaoCursoPosGraduacao, "SituacaoCursoForm");
                    PreecherComboTabGeral(ddlFormComplementPedagPosGraduacao, "FormacaoComplement");

                }
                else
                {
                    if (_tipoOperacao == TipoOperacao.Novo)
                    {
                        tseCategoria.SqlWhere = " INGRESSO='S' and FUNCIONARIO = 'N'";
                    }
                    else if (_tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.ConsultarDocente
                        || (_tipoOperacao == TipoOperacao.Inicial && !tseCategoria.DBValue.IsNull))
                    {
                        CarregaCargo(Convert.ToString(tseCategoria.DBValue));
                    }
                }

                lblMensagem.Text = string.Empty;
                lblMensagemCapacitacao.Text = string.Empty;

                //tamanho maximo das datas
                DateTime dtAtual = DateTime.Now;

                dteRGDataExpPessoa.MaxDate = dtAtual.Date;
                dteDtDemissao.MaxDate = dtAtual.Date;
                dteDtAdmissao.MaxDate = dtAtual.Date;
                dteCProfDtExp.MaxDate = dtAtual.Date;
                dteDMIL_Alist_DtExp.MaxDate = dtAtual.Date;
                dteDMIL_Cr_DtExp.MaxDate = dtAtual.Date;
                dteDOC_Teleitor_DtExp.MaxDate = dtAtual.Date;
                dteDtNasc.MaxDate = dtAtual.Date.AddDays(-1);


                ddlEscolaridade_SelectedIndexChanged(sender, e);
                ddlEscolaridadePosGraduacao_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void AdicionaAtributosCampos()
        {
            txtCargaHorariaCapacitacao.Attributes.Add("onkeyPress", "return isNumberKey(event);");
            txtCargaHorariaCapacitacao.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtCargaHorariaCapacitacao.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            ControlarEnderecoPais();
        }

        protected void CarregaCombo()
        {
            CarregarDadosDrop(ddlPaisNasc.ID);
            CarregarDadosDrop(ddlNacionalidade.ID);
            CarregarDadosDrop(ddlEst_Civil.ID);
            CarregarDadosDrop(ddlPais.ID);
            CarregaNecessidadeEspecial();
            CarregarDadosDrop(ddlRGEmissorPessoa.ID);
            CarregarDadosDrop(ddlRGUFPessoa.ID);
            CarregarDadosDrop(txtCProfUF.ID);
            CarregarDadosDrop(ddlRGTipoPessoa.ID);
            CarregaEtnia();
            CarregarDadosDrop(cmbRegContratacao.ID);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);

            if (RN.PadroesDeAcessos.ConsultarPadacesParcial(HttpContext.Current.User.Identity.Name))
            {
                apcDocente.TabPages.FindByName("DadosLotacao").ClientVisible = false;
                btnNovo.Visible = false;
            }
            else
            {
                apcDocente.TabPages.FindByName("DadosLotacao").ClientVisible = true;
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEndereco();
        }

        protected void cmbRegContratacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString())
            {
                lblCargaHoraria.Visible = true;
                ddlCargaHoraria.Visible = true;
                lblAulasAlocadas.Visible = true;
                txtAulasAlocadas.Visible = true;
            }
            else
            {
                lblCargaHoraria.Visible = false;
                ddlCargaHoraria.Visible = false;
                lblAulasAlocadas.Visible = false;
                txtAulasAlocadas.Visible = false;
            }
        }

        protected void chkNaoPossuiIdFuncional_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtIdFuncional.Enabled = true;
                txtVinculo.Enabled = true;
                if (chkNaoPossuiIdFuncional.Checked)
                {
                    txtIdFuncional.Enabled = false;
                    txtVinculo.Enabled = false;
                    txtIdFuncional.Text = string.Empty;
                    txtVinculo.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void txtIdFuncional_TextChanged(object sender, EventArgs e)
        {
            if (!txtIdFuncional.Text.IsNullOrEmptyOrWhiteSpace())
            {
                chkNaoPossuiIdFuncional.Checked = false;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
            LyLotacao lotacao = new LyLotacao();
            LyCandidatoDocente candidato = new LyCandidatoDocente();
            LyGrupoHabilitacaoDoc habilitacao = new LyGrupoHabilitacaoDoc();
            RN.RecursosHumanos.Entidades.Acumulacao acumulacao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.Acumulacao();
            RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
            string zonaResidencial = null;
            bool possuiId = true;
            string naturalidade = string.Empty;
            string municipioEstrangeiro = string.Empty;

            try
            {

                if (!ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlPaisNasc.SelectedItem.Text.ToUpper() == "BRASIL")
                    {
                        naturalidade = (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue) ? Convert.ToString(tseNaturalidade.DBValue) : null;
                    }
                    else
                    {
                        // obtém o municipio estrangeiro
                        SimpleRow sr = Endereco.ObterCodigoMunicipioEstrangeiro(txtMunicipioNaturalidade.Text.Trim());

                        //verifica se a função retornou algum valor para a simplerow
                        if (sr != null)
                        {
                            //preenche os dados obtidos de municipio estrangeiro
                            if (!sr["municipio_estrangeiro"].IsNull)
                            {
                                municipioEstrangeiro = Convert.ToString(sr["municipio_estrangeiro"]);
                            }
                        }

                        naturalidade = !municipioEstrangeiro.IsNullOrEmptyOrWhiteSpace() ? municipioEstrangeiro : null;
                    }
                }

                var pessoa = new LyPessoa
                    {
                        Pessoa = !string.IsNullOrEmpty(txtPessoa.Text) ? Convert.ToDecimal(txtPessoa.Text) : 0m,
                        Nome_compl = txtNomeComplPessoa.Text,
                        Nome_social = TxtNomeSocial.Text,
                        Endereco = txtEnderecoPessoa.Text,
                        End_num = txtEndNumPessoa.Text,
                        End_municipio = Convert.ToString(tseMunicipio.DBValue),
                        Cep = txtCEP.Text.RetirarCaracteres(),
                        Rg_emissor = ddlRGEmissorPessoa.SelectedValue,
                        Rg_tipo = ddlRGTipoPessoa.SelectedValue,                       
                        Pais_nasc = ddlPaisNasc.SelectedValue,
                        Nacionalidade = ddlNacionalidade.SelectedValue,
                        Municipio_nasc = naturalidade,
                        Etnia = ddlRaca.SelectedValue,
                        Rg_uf = ddlRGUFPessoa.SelectedValue,
                        End_pais = ddlPais.SelectedValue,
                        NecessidadeEspecialId = !ddlNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?) null,
                        Est_civil = ddlEst_Civil.SelectedValue,
                        Cprof_uf = !txtCProfUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? txtCProfUF.SelectedValue : null,
                        Sexo = !string.IsNullOrEmpty(rblSexo.Text)
                                               ? Convert.ToString(rblSexo.Text)
                                               : string.Empty,
                        End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null,
                        Bairro = txtBairro.Text,
                        Fone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.Trim() : null,
                        Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text.Trim() : null,
                        E_mail_interno = !txtEmailInstitucional.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailInstitucional.Text.Trim() : null,
                        E_mail = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null,
                        Rg_num = !txtRGNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoa.Text.Trim() : null,
                        NomeMae = txtNomeMae.Text.TrimEnd(),
                        NomePai = txtNomePai.Text.TrimEnd(),
                        Cpf = txtCPF.Text.RetirarMascaraCPF().Trim(),
                        Cprof_num = !txtCProfNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfNum.Text.Trim() : null,
                        Cprof_serie = !txtCProfSerie.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfSerie.Text.Trim() : null,
                        Alist_num = !txtDMIL_Alist_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_Num.Text.Trim() : null,
                        Alist_rm = !txtDMIL_Alist_RM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_RM.Text.Trim() : null,
                        Alist_serie = !txtDMIL_Alist_Serie.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_Serie.Text.Trim() : null,
                        Alist_csm = !txtDMIL_Alist_CSM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Alist_CSM.Text.Trim() : null,
                        Cr_num = !txtDMIL_Cr_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_Num.Text.Trim() : null,
                        Cr_rm = !txtDMIL_Cr_RM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_RM.Text.Trim() : null,
                        Cr_serie = !txtDMIL_Cr_Serie.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_Serie.Text.Trim() : null,
                        Cr_csm = !txtDMIL_Cr_CSM.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_CSM.Text.Trim() : null,
                        Cr_cat = !txtDMIL_Cr_CAT.Text.IsNullOrEmptyOrWhiteSpace() ? txtDMIL_Cr_CAT.Text.Trim() : null,
                        Teleitor_num = !txtDOC_Teleitor_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Teleitor_Num.Text.Trim() : null,
                        Teleitor_zona = !txtDOC_Teleitor_Zona.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Teleitor_Zona.Text.Trim() : null,
                        Teleitor_secao = !txtDOC_Teleitor_Secao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_Teleitor_Secao.Text.Trim() : null,
                        Dt_nasc = !string.IsNullOrEmpty(dteDtNasc.Text) ? dteDtNasc.Date : (DateTime?)null,
                        Rg_dtexp = !string.IsNullOrEmpty(dteRGDataExpPessoa.Text) ? dteRGDataExpPessoa.Date : (DateTime?)null,
                        Cprof_dtexp = !string.IsNullOrEmpty(dteCProfDtExp.Text) ? dteCProfDtExp.Date : (DateTime?)null,
                        Alist_dtexp = !string.IsNullOrEmpty(dteDMIL_Alist_DtExp.Text) ? dteDMIL_Alist_DtExp.Date : (DateTime?)null,
                        Cr_dtexp = !string.IsNullOrEmpty(dteDMIL_Cr_DtExp.Text) ? dteDMIL_Cr_DtExp.Date : (DateTime?)null,
                        Teleitor_dtexp = !string.IsNullOrEmpty(dteDOC_Teleitor_DtExp.Text) ? dteDOC_Teleitor_DtExp.Date : (DateTime?)null,
                        Id_censo = null,                        
                        Pispasep = !txtPISPASEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtPISPASEP.Text.Trim() : null,
                        AreaAssentamento = chkAreaAssentamento.Checked ? "S" : "N",
                        TerraIndigena = chkTerraIndigena.Checked ? "S" : "N",
                        AreaQuilombos = chkQuilombos.Checked ? "S" : "N",
                        UsuarioId = User.Identity.Name
                    };              

                zonaResidencial = !rblLocalizacaoUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblLocalizacaoUF.SelectedValue : null;

                var docente = new LyDocente
                    {
                        Num_func = !string.IsNullOrEmpty(txtNumFunc.Text) ? Convert.ToDecimal(txtNumFunc.Text) : 0m,
                        Pessoa = pessoa.Pessoa,                        
                        Matricula = txtMatricula.Text.Trim(),
                        Dt_admissao = !dteDtAdmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dteDtAdmissao.Date : (DateTime?)null,
                        Dt_demissao = !dteDtDemissao.Text.IsNullOrEmptyOrWhiteSpace() ? dteDtDemissao.Date : (DateTime?)null,
                        Categoria = (!this.tseCategoria.DBValue.IsNull && this.tseCategoria.IsValidDBValue) ? tseCategoria.DBValue.ToString() : null,
                        RegimeContratacaoId = !cmbRegContratacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(cmbRegContratacao.SelectedValue) : (int?)null,
                        Ano_ingresso = !txtAnoConcurso.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtAnoConcurso.Text) : (decimal?)null,
                        Candidato = !txtCandidato.Text.IsNullOrEmptyOrWhiteSpace() ? txtCandidato.Text : null,
                        Concurso = !txtProcesso.Text.IsNullOrEmptyOrWhiteSpace() ? txtProcesso.Text : null,                       
                        Senha_alterada = "N",
                        Acumulacao = string.IsNullOrEmpty(rblAcumulacao.SelectedValue) ? (int)RN.Docentes.PossuiAcumulacao.NaoInformado : Convert.ToInt32(rblAcumulacao.SelectedValue),
                        Usuario = User.Identity.Name
                    };

                possuiId = !chkNaoPossuiIdFuncional.Checked;
                if (possuiId)
                {
                    pessoa.IdFuncional = !txtIdFuncional.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtIdFuncional.Text) : (int?)null;
                    docente.Vinculo = !txtVinculo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtVinculo.Text.Trim()) : (int?)null;
                }
                else
                {
                    pessoa.IdFuncional = 0;
                    docente.Vinculo = null;
                }

                long resultado;

                if (long.TryParse(txtCelular.Text.Trim().RetirarMascaraTelefone(), out resultado))
                {                   
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 10)
                    {
                        pessoa.Celular = string.Format("{0:(00)0000-0000}", resultado);                        
                    }
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 11)
                    {
                        pessoa.Celular = string.Format("{0:(00)00000-0000}", resultado);
                    }
                    txtCelular.Text = pessoa.Celular;
                }
                else
                {
                    pessoa.Celular = null;
                }
                //Verificação da Carga Horaria
                if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString())
                {
                    //Caso o docente seja de 'Contrato Temporario' utlizar ch escolhida na combo
                    docente.Regime_trabalho = !string.IsNullOrEmpty(ddlCargaHoraria.SelectedValue) ? ddlCargaHoraria.SelectedValue : null;
                }
                else
                {
                    docente.Regime_trabalho = txtCH.Text.Trim();
                }

                if (_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.ConsultarPessoa))
                {
                    docente.Senha_dol = txtCPF.Text.RetirarMascaraCPF().Trim();
                    lotacao.Pessoa = pessoa.Pessoa;
                    lotacao.Matricula = txtMatricula.Text.Trim();
                    lotacao.Ordem = 1;
                    lotacao.Funcao = (!this.tseFuncaoLotacao.DBValue.IsNull && this.tseFuncaoLotacao.IsValidDBValue) ? tseFuncaoLotacao.DBValue.ToString() : null;
                    if (!dtNomeacao.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(dtNomeacao.Text);
                    }
                    lotacao.UnidadeEns = (!this.tseUnidadeLotacao.DBValue.IsNull && this.tseUnidadeLotacao.IsValidDBValue) ? tseUnidadeLotacao.DBValue.ToString() : null;
                    lotacao.UnidadeFis = (!this.tseUnidadeLotacao.DBValue.IsNull && this.tseUnidadeLotacao.IsValidDBValue) ? tseUnidadeLotacao.DBValue.ToString() : null;
                    lotacao.Nucleo = (!this.tseUnidadeLotacao.DBValue.IsNull && this.tseUnidadeLotacao.IsValidDBValue) ? tseUnidadeLotacao["NUCLEO"].ToString() : null;
                    lotacao.Setor = (!this.tseUnidadeLotacao.DBValue.IsNull && this.tseUnidadeLotacao.IsValidDBValue) ? tseUnidadeLotacao["SETOR"].ToString() : null;
                    lotacao.Categoria = (!this.tseCategoria.DBValue.IsNull && this.tseCategoria.IsValidDBValue) ? tseCategoria.DBValue.ToString() : null;
                    lotacao.Usuario = User.Identity.Name;
                    lotacao.Readaptado = "N";

                    habilitacao.Agrupamento = (!this.tseDisciplinaIngresso.DBValue.IsNull && this.tseDisciplinaIngresso.IsValidDBValue) ? tseDisciplinaIngresso.DBValue.ToString() : null;
                    habilitacao.AgrupamentoIngresso = "S";
                    habilitacao.Provisorio = "N";
                    habilitacao.Campo01 = "S";
                    habilitacao.Campo02 = "S";
                }
                else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                {
                    lotacao.Pessoa = pessoa.Pessoa;
                    lotacao.Matricula = txtMatricula.Text.Trim();
                    lotacao.Categoria = (!this.tseCategoria.DBValue.IsNull && this.tseCategoria.IsValidDBValue) ? tseCategoria.DBValue.ToString() : null;
                    lotacao.Usuario = User.Identity.Name;

                }


                //Monta dados Acumulação 
                if (docente.Acumulacao == (int)RN.Docentes.PossuiAcumulacao.Sim)
                {
                    acumulacao.MatriculaOrgao = txtMatriculaAcumulacao.Text;
                    acumulacao.Orgao = txtOrgaoAcumulacao.Text;
                    acumulacao.NumeroProcesso = txtNumProcessoAcumulacao.Text;
                    acumulacao.UsuarioId = User.Identity.Name;
                }

                if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString())
                {
                    //Caso o regima de contratação seja contrato temporario, será necessario atualizar o candidato
                    candidato.Agrupamento_ingresso = (!this.tseDisciplinaIngresso.DBValue.IsNull && this.tseDisciplinaIngresso.IsValidDBValue) ? tseDisciplinaIngresso.DBValue.ToString() : null;
                    candidato.Bairro = txtBairro.Text.Trim();
                    candidato.Carga_Horaria = !string.IsNullOrEmpty(ddlCargaHoraria.SelectedValue) ? int.Parse(ddlCargaHoraria.SelectedValue) : -1;
                    candidato.Categoria = (!this.tseCategoria.DBValue.IsNull && this.tseCategoria.IsValidDBValue) ? tseCategoria.DBValue.ToString() : null;
                    candidato.Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text.Trim() : null;
                    candidato.Cep = txtCEP.Text.RetirarCaracteres();
                    candidato.Cpf = txtCPF.Text.RetirarMascaraCPF().Trim();
                    candidato.Cprof_dtexp = !dteCProfDtExp.Text.IsNullOrEmptyOrWhiteSpace() ? dteCProfDtExp.Date : (DateTime?)null;
                    candidato.Cprof_num = !txtCProfNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfNum.Text.Trim() : null;
                    candidato.Cprof_serie = !txtCProfSerie.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfSerie.Text.Trim() : null;
                    candidato.Cprof_uf = !txtCProfUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? txtCProfUF.SelectedValue : null;
                    candidato.Dt_nasc = !dteDtNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dteDtNasc.Date : (DateTime?)null;
                    candidato.E_mail = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;                    
                    candidato.End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null;
                    candidato.End_municipio = Convert.ToString(tseMunicipio.DBValue);
                    candidato.End_num = txtEndNumPessoa.Text.Trim();
                    candidato.End_pais = ddlPais.SelectedValue;
                    candidato.Endereco = txtEnderecoPessoa.Text.Trim();
                    candidato.Estado_civil = ddlEst_Civil.SelectedValue;
                    candidato.Fone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.Trim() : null;
                    candidato.Municipio_nasc = Convert.ToString(tseNaturalidade.DBValue);
                    candidato.Nacionalidade = ddlNacionalidade.SelectedValue;
                    candidato.NecessidadeEspecialId = !ddlNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?)null;
                    candidato.Nome = Utils.TiraBrancosConsec(txtNomeComplPessoa.Text.Trim());
                    candidato.Nome_mae = txtNomeMae.Text.TrimEnd();
                    candidato.Nome_pai = txtNomePai.Text.TrimEnd();
                    candidato.Pais_nasc = ddlPaisNasc.SelectedValue;
                    candidato.Pis_pasep = txtPISPASEP.Text.Trim();
                    candidato.Rg_dtexp = !dteRGDataExpPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? dteRGDataExpPessoa.Date : (DateTime?)null;
                    candidato.Rg_uf = !ddlRGUFPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGUFPessoa.SelectedValue : null;
                    candidato.Rg_emissor = !ddlRGEmissorPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGEmissorPessoa.SelectedValue : null;
                    candidato.Rg_tipo = ddlRGTipoPessoa.SelectedValue;
                    candidato.Rg_num = !txtRGNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoa.Text.Trim() : null;
                    candidato.Sexo = !string.IsNullOrEmpty(rblSexo.Text) ? Convert.ToString(rblSexo.Text) : string.Empty;
                    candidato.Dt_proposta = !dteDtAdmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dteDtAdmissao.Date : (DateTime?)null;
                    candidato.Aulas_Alocadas = !string.IsNullOrEmpty(txtAulasAlocadas.Text) ? Convert.ToInt32(txtAulasAlocadas.Text.Trim()) : -1;
                }
                else
                {
                    candidato = null;
                }

                googleEducation.Pessoa = pessoa.Pessoa;
                googleEducation.Email = !txtEmailGoogle.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailGoogle.Text.Trim() : null;
                googleEducation.UsuarioId = User.Identity.Name;

                if (_tipoOperacao.Equals(TipoOperacao.Novo) || _tipoOperacao.Equals(TipoOperacao.ConsultarPessoa))
                {
                    validacao = rnDocentes.Valida(pessoa, docente, acumulacao, habilitacao, lotacao, apcDocente.TabPages.FindByName("DadosLotacao").ClientVisible, candidato, zonaResidencial, true, possuiId, (chkNaoSeAplica.Checked ? "S" : "N"),googleEducation,ddlPovoIndigena.SelectedValue);
                    if (validacao.Valido)
                    {
                        rnDocentes.Insere(docente, pessoa, acumulacao, habilitacao, lotacao, zonaResidencial,ddlPovoIndigena.SelectedValue);
                        tseDocentes.DBValue = docente.Matricula;
                        txtPessoa.Text = pessoa.Pessoa.ToString();
                        txtNumFunc.Text = docente.Num_func.ToString();
                        txtMatricula.Text = docente.Matricula.ToString();
                        txtIdFuncional.Text = pessoa.IdFuncional.ToString();
                        txtVinculo.Text = docente.Vinculo.ToString();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
                else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                {
                    //Para Alteração valida ou salva lotação e grupo habilitacao
                    validacao = rnDocentes.Valida(pessoa, docente, acumulacao, null, null, apcDocente.TabPages.FindByName("DadosLotacao").ClientVisible, candidato, zonaResidencial, false, possuiId, (chkNaoSeAplica.Checked ? "S" : "N"),googleEducation,ddlPovoIndigena.SelectedValue);

                    if (validacao.Valido)
                    {
                        rnDocentes.Atualiza(docente, pessoa, acumulacao,lotacao, candidato, zonaResidencial,googleEducation,ddlPovoIndigena.SelectedValue);                      
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }


                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
                ControlarTSearchs();
                lblMensagem.Text = "Operação realizada com sucesso.";

                _tipoOperacaoRel = TipoOperacao.Inicial;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
            ControlarTSearchs();
            //Os dados do relacionamento devem ficar como tipo inicial
            _tipoOperacaoRel = TipoOperacao.Inicial;

        }

        protected void ddlRaca_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPovoIndigena.Visible = false;
                lblPovo.Visible = false;
                ddlPovoIndigena.ClearSelection();

                if (ddlRaca.SelectedValue == "Índigena")
                {
                    CarregaPovoIndigena();
                    ddlPovoIndigena.Visible = true;
                    lblPovo.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPovoIndigena()
        {

            RN.RecursosHumanos.PovoIndigena rnPovoIndigena = new RN.RecursosHumanos.PovoIndigena();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPovoIndigena.Items.Clear();
            ddlPovoIndigena.DataSource = rnPovoIndigena.ListaAtivoPor();
            ddlPovoIndigena.DataBind();
            ddlPovoIndigena.Items.Insert(0, item);

        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
            ControlarTSearchs();
            //Os dados do relacionamento devem ficar como tipo inicial
            _tipoOperacaoRel = TipoOperacao.Inicial;

        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            tseDocentes.ResetValue();
            tsePessoa.ResetValue();
            _tipoOperacao = TipoOperacao.Inicial;
            ControlarTipoOperacao();
            ControlarTSearchs();
            //Caso seja cancelado o formulario de Aluno, os dados do relacionamento devem ficar como tipo inicial
            _tipoOperacaoRel = TipoOperacao.Inicial;

        }       

        protected void tseDocentes_Changed(object sender, EventArgs args)
        {
            try
            {
                LimparTela();
                if (!string.IsNullOrEmpty(tseDocentes.DBValue.ToString()))
                {
                    if (tseDocentes.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.ConsultarDocente;
                        ControlarTipoOperacao();
                        ControlarTSearchs();
                    }
                    else
                    {
                        lblMensagem.Text = "Docente não cadastrado ou usuário sem permissão de visualização do Docente(Lotação).";
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                        ControlarTSearchs();
                    }
                }
                else
                {
                    lblMensagem.Text = "Docente não cadastrado ou usuário sem permissão de visualização do Docente(Lotação).";
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePessoa_Changed(object sender, EventArgs args)
        {
            if (!tsePessoa.DBValue.IsNull)
            {
                if (tsePessoa.IsValidDBValue)
                {
                    _tipoOperacao = TipoOperacao.ConsultarPessoa;
                    ControlarTipoOperacao();
                }
                else
                    lblMensagem.Text = "Pessoa não cadastrada.";
            }

        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (tseMunicipio.IsValidDBValue
                && !tseMunicipio.DBValue.IsNull)
            {
                txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
        }

        protected void tseNaturalidade_Changed(object sender, EventArgs args)
        {
            if (tseNaturalidade.IsValidDBValue
                && !tseNaturalidade.DBValue.IsNull)
            {
                txtEstadoNaturalidade.Value = tseNaturalidade["uf_sigla"].ToString();
            }
        }

        protected void grdPesquisaCEP_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string cep = Convert.ToString(e.GetListSourceFieldValue("cep"));
                string codigoLogradouro = Convert.ToString(e.GetListSourceFieldValue("codigoLogradouro"));
                string codigoBairro = Convert.ToString(e.GetListSourceFieldValue("codigoBairro"));
                string codigoMunicipio = Convert.ToString(e.GetListSourceFieldValue("codigoMunicipio"));
                string uf_sigla = Convert.ToString(e.GetListSourceFieldValue("uf_sigla"));
                e.Value = cep + "-" + codigoLogradouro + "-" + codigoBairro + "-" + codigoMunicipio + "-" + uf_sigla;
            }
        }

        protected void ddlPaisNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEnderecoNascimento();
        }

        protected void ddlRGTipoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRGTipoPessoa.SelectedValue == "RG")
            {
                ddlRGUFPessoa.Enabled = true;
                lblRGUFPessoa.Text = "Estado*: ";
                lblRGUFPessoa.Font.Bold = true;

                dteRGDataExpPessoa.Enabled = true;
                lblRGDataExpPessoa.Text = "Data de Expedição*: ";
                lblRGDataExpPessoa.Font.Bold = true;
            }
            else
            {
                ddlRGUFPessoa.Enabled = false;
                lblRGUFPessoa.Text = "Estado: ";
                lblRGUFPessoa.Font.Bold = false;

                dteRGDataExpPessoa.Enabled = false;
                lblRGDataExpPessoa.Text = "Data de Expedição: ";
                lblRGDataExpPessoa.Font.Bold = false;
            }
        }

        #endregion

        #region Métodos

        private RN.DadosEndereco ControlarEnderecoNascimento()
        {
            RN.RetValue retorno = null;

            RN.DadosEndereco dadosEndereco = new Techne.Lyceum.RN.DadosEndereco();
            dadosEndereco.DescricaoPais = ddlPaisNasc.SelectedItem.Text;
            dadosEndereco.UF = txtEstadoNaturalidade.Value;

            if (ddlPaisNasc.SelectedItem.Text.ToUpper() != "BRASIL")
            {
                dadosEndereco.DescricaoMunicipio = txtMunicipioNaturalidade.Text;
                retorno = RN.Endereco.ControlarEnderecoEstrangeiro(dadosEndereco);
            }
            else
            {
                if (!tseNaturalidade.DBValue.IsNull)
                {
                    if (tseNaturalidade.IsValidDBValue)
                        dadosEndereco.Municipio = Convert.ToString(tseNaturalidade.DBValue);
                }
            }
            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                    if (retorno.Errors != null)
                        dadosEndereco.Error = retorno.Errors;
                }
            }

            return dadosEndereco;
        }

        private RN.DadosEndereco ControlarEndereco()
        {
            RN.RetValue retorno = null;

            RN.DadosEndereco dadosEndereco = new Techne.Lyceum.RN.DadosEndereco();
            dadosEndereco.DescricaoBairro = txtBairro.Text;
            dadosEndereco.Cep = txtCEP.Text.RetirarCaracteres();
            dadosEndereco.DescricaoLogradouro = txtEnderecoPessoa.Text;
            dadosEndereco.DescricaoPais = ddlPais.SelectedItem.Text;
            dadosEndereco.UF = txtEstado.Value;

            if (ddlPais.SelectedItem.Text.ToUpper() != "BRASIL")
            {
                dadosEndereco.DescricaoMunicipio = txtMunicipio.Text;
                retorno = RN.Endereco.ControlarEnderecoEstrangeiro(dadosEndereco);
            }
            else
            {
                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (tseMunicipio.IsValidDBValue)
                    {
                        dadosEndereco.Municipio = Convert.ToString(tseMunicipio.DBValue);
                        dadosEndereco.DescricaoMunicipio = Convert.ToString(tseMunicipio["nome"]);
                    }
                }

                retorno = RN.Endereco.ControlarEndereco(dadosEndereco);
            }

            if (retorno != null)
            {
                if (!retorno.Ok)
                    lblMensagem.Text = retorno.Errors.ToString();
            }



            return dadosEndereco;
        }

        private void ControlarEnderecoPais()
        {
            if (ddlPais.SelectedItem != null)
            {
                //código 0 = BRASIL
                if (ddlPais.SelectedItem.Text.ToUpper() != "BRASIL")
                {
                    tsCEP.ShowButton = false;
                    revCEP.Enabled = false;

                    txtCEP.MaxLength = 9;

                    txtMunicipio.Visible = true;
                    //rfvMunicipio.ControlToValidate = txtMunicipio.ID;
                    tseMunicipio.Visible = false;
                    if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                        txtEstado.Attributes.Remove("readonly");
                }
                else
                {
                    if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                    {
                        tsCEP.ShowButton = true;
                        revCEP.Enabled = true;
                    }
                    else
                    {
                        tsCEP.ShowButton = false;
                        revCEP.Enabled = false;
                    }

                    txtCEP.MaxLength = 8;

                    txtMunicipio.Visible = false;
                    //rfvMunicipio.ControlToValidate = tseMunicipio.ID;
                    tseMunicipio.Visible = true;

                    txtEstado.Attributes.Add("readonly", "readonly");
                }

                if (ddlPaisNasc.SelectedItem != null)
                {
                    if (ddlPaisNasc.SelectedItem.Text.ToUpper() != "BRASIL")
                    {
                        txtMunicipioNaturalidade.Visible = true;
                        //rfvNaturalidade.ControlToValidate = txtMunicipioNaturalidade.ID;
                        tseNaturalidade.Visible = false;
                        if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                            txtEstadoNaturalidade.Attributes.Remove("readonly");
                    }
                    else
                    {
                        txtMunicipioNaturalidade.Visible = false;
                        //rfvNaturalidade.ControlToValidate = tseNaturalidade.ID;
                        tseNaturalidade.Visible = true;

                        txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
                    }
                }
            }
        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            tseMunicipio.ResetValue();

            txtEstado.Value = string.Empty;
            txtEnderecoPessoa.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtBairro.Text = string.Empty;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
        }

        private void LimparEnderecoNascimento()
        {
            txtMunicipioNaturalidade.Text = string.Empty;
            tseNaturalidade.ResetValue();
            txtEstadoNaturalidade.Value = string.Empty;
        }

        private void ControlarTipoOperacao()
        {
            apcDocente.ActiveTabIndex = 0;
            chkNaoPossuiIdFuncional.Visible = true;
            chkNaoPossuiIdFuncional.Enabled = false;
            lblMatricula.Text = "Matrícula ou ID/Vínculo:";

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        txtProcesso.Text = string.Empty;
                        txtCandidato.Text = string.Empty;
                        CarregaCargo("");
                        apcDocente.Visible = false;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = false;
                        btnImprimir.Visible = false;
                        ddlPovoIndigena.Visible = false;
                        lblPovo.Visible = false;
                        ddlPovoIndigena.ClearSelection();
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                        ControlarVisibilidadeControle(controles);
                        apcDocente.Visible = true;
                        apcDocente.TabPages[3].Enabled = true;
                        apcDocente.TabPages[4].Enabled = true;

                        DesabilitaCampos();
                        if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.Concursado).ToString())
                        {
                            btnImprimir.Visible = true;
                        }

                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = false;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        lbltxtPessoa.Visible = false;
                        lblPessoaTSearch.Visible = false;
                        txtPessoa.Visible = false;
                        btnImprimir.Visible = false;
                        tsePessoa.ResetValue();
                        apcDocente.Visible = true;
                        apcDocente.TabPages[3].Enabled = false;
                        apcDocente.TabPages[4].Enabled = false;
                        chkNaoPossuiIdFuncional.Visible = false;
                        lblMatricula.Text = "Matrícula:";
                        txtMatricula.ReadOnly = true;
                        txtIdFuncional.ReadOnly = false;
                        txtVinculo.ReadOnly = false;

                        LimparTela();
                        CarregaCombo();
                        CarregarDadosDrop(cmbRegContratacao.ID);
                        LimparEndereco();
                        HabilitaCampos();

                        pnlDisciplinaIngresso.Visible = true;
                        pnlLotacao.Visible = true;
                        tseFuncaoLotacao.Mode = ControlMode.View;
                        tseRegionalLotacao.Mode = ControlMode.Edit;
                        tseMunicipioLotacao.Mode = ControlMode.Edit;
                        tseUnidadeLotacao.Mode = ControlMode.Edit;
                        dtNomeacao.Enabled = true;
                        tseCategoria.SqlWhere = " INGRESSO='S'";
                        ddlCargaHoraria.Visible = false;
                        lblCargaHoraria.Visible = false;
                        lblAulasAlocadas.Visible = false;
                        txtAulasAlocadas.Visible = false;                        

                        tseDocentes.ResetValue();

                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = false;

                        break;
                    }                
                case TipoOperacao.Alterar:
                    {
                        txtPessoa.Visible = false;

                        lbltxtPessoa.Visible = false;
                        apcDocente.TabPages[3].Enabled = true;
                        apcDocente.TabPages[4].Enabled = true;
                        pnlLotacao.Visible = false;
                        pnlDisciplinaIngresso.Visible = false;
                        HabilitaCampos();
                        txtMatricula.ReadOnly = true; //matricula não alterado
                        txtIdFuncional.ReadOnly = true; //id não alterado
                        txtVinculo.ReadOnly = true; //vinculo não alterado
                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        if (RN.PadroesDeAcessos.ConsultarPadacesParcial(HttpContext.Current.User.Identity.Name))
                        {
                            apcDocente.TabPages.FindByName("DadosLotacao").ClientVisible = false;
                        }
                        else
                        {
                            apcDocente.TabPages.FindByName("DadosLotacao").ClientVisible = true;
                        }
                        if (!string.IsNullOrEmpty(txtCandidato.Text) && !string.IsNullOrEmpty(txtProcesso.Text))
                        {
                            cmbRegContratacao.Enabled = false;
                        }

                        //O CAMPO SÓ PODERA SER ALTERADDO CASO NAO TENHA INFORMAÇÃO(OU SEJA, ESTEJA VAZIO)
                        if (!txtEmailGoogle.Text.IsNullOrEmptyOrWhiteSpace())
                        {                          
                            txtEmailGoogle.Enabled = false;
                        }

                        break;
                    }

                case TipoOperacao.ConsultarDocente:
                    {

                        RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                        RN.AulaDocente rnAulaDocente = new AulaDocente();

                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = false;
                        apcDocente.TabPages[3].Enabled = true;
                        apcDocente.TabPages[4].Enabled = true;

                        RN.Entidades.LyDocente dadosDocente = new LyDocente();
                        dadosDocente = rnDocentes.CarregaPor(Convert.ToInt32(tseDocentes["num_func"].ToString()));

                        if ((dadosDocente != null) && (dadosDocente.Pessoa > 0))
                        {
                            txtPessoa.Text = Convert.ToString(dadosDocente.Pessoa);

                            //Verifica se foi alterado pelo Docente Online
                            DateTime dataAlteracao;
                            if (rnDocentes.AlteradoDocenteOnlinePor(dadosDocente.Pessoa, out dataAlteracao))
                            {
                                lblAvisoDol.Text = string.Format("Os dados do professor foram atualizados no dia {0} no Docente On Line.", dataAlteracao.ToString("dd/MM/yyyy"));
                            }
                            else
                            {
                                lblAvisoDol.Text = string.Empty;
                            }

                            CarregaCombo();

                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                            ControlarVisibilidadeControle(controles);
                            apcDocente.Visible = true;
                            PreencherDadosTelaDocente(dadosDocente);

                            RN.Entidades.LyPessoa dadosPessoa = new LyPessoa();

                            dadosPessoa = RN.Pessoa.Carregar(Convert.ToInt32(dadosDocente.Pessoa));
                            PreencherDadosTelaPessoa(dadosPessoa);
                            RN.RecursosHumanos.Entidades.Acumulacao acumulacao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.Acumulacao();
                            RN.RecursosHumanos.Acumulacao rnAcumulacao = new Techne.Lyceum.RN.RecursosHumanos.Acumulacao();

                            //Busca dados da acumulação
                            if (dadosDocente.Acumulacao == (int)RN.Docentes.PossuiAcumulacao.NaoInformado)
                            {
                                rblAcumulacao.ClearSelection();
                            }
                            else
                            {
                                acumulacao = rnAcumulacao.ObtemPor(Convert.ToDecimal(tseDocentes["num_func"].ToString()));
                                if (acumulacao.AcumulacaoId > 0)
                                {
                                    rblAcumulacao.SelectedValue = "1";
                                    rblAcumulacao_SelectedIndexChanged(null, null);
                                    txtMatriculaAcumulacao.Text = acumulacao.MatriculaOrgao;
                                    txtOrgaoAcumulacao.Text = acumulacao.Orgao;
                                    txtNumProcessoAcumulacao.Text = acumulacao.NumeroProcesso;
                                }
                                else
                                {
                                    rblAcumulacao.SelectedValue = "0";
                                    txtMatriculaAcumulacao.Text = string.Empty;
                                    txtOrgaoAcumulacao.Text = string.Empty;
                                    txtNumProcessoAcumulacao.Text = string.Empty;
                                }
                            }

                            RN.RecursosHumanos.Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
                            RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();

                            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new GrupoHabilitacaoDoc();
                            txtDisciplinadeIngresso.Text = rnGrupoHabilitacaoDoc.ObtemDisciplinadeIngressoPor(Convert.ToDecimal(tseDocentes["num_func"].ToString()));

                            googleEducation = rnGoogleEducation.ObtemPor(Convert.ToDecimal(dadosDocente.Pessoa));

                            if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtEmailGoogle.Text = googleEducation.Email.Trim();
                            }

                            if (!string.IsNullOrEmpty(txtCandidato.Text) && !string.IsNullOrEmpty(txtProcesso.Text))
                            {
                                cmbRegContratacao.SelectedValue = ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString();
                            }
                            else
                            {
                                ListItem listItem = cmbRegContratacao.Items.FindByValue(((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString());

                                if (listItem != null)
                                {
                                    cmbRegContratacao.Items.Remove(listItem);
                                }
                            }

                            if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString())
                            {
                                btnImprimir.Visible = false;
                                lblCargaHoraria.Visible = true;
                                ddlCargaHoraria.Visible = true;
                                lblAulasAlocadas.Visible = true;
                                txtAulasAlocadas.Visible = true;
                                txtAulasAlocadas.Text = rnAulaDocente.ObtemQuantidadeAulasAtivasDocentePor(tseDocentes.DBValue.ToString()).ToString();

                                if (!string.IsNullOrEmpty(txtProcesso.Text.Trim()))
                                {
                                    CarregaCargaHoraria(txtProcesso.Text.Trim(), dadosDocente.Categoria);

                                    ListItem listItem = ddlCargaHoraria.Items.FindByValue(dadosDocente.Regime_trabalho);

                                    if (listItem != null)
                                    {
                                        ddlCargaHoraria.SelectedValue = dadosDocente.Regime_trabalho;
                                    }

                                }

                            }
                            else
                            {
                                btnImprimir.Visible = true;
                                lblCargaHoraria.Visible = false;
                                ddlCargaHoraria.Visible = false;
                                lblAulasAlocadas.Visible = false;
                                txtAulasAlocadas.Visible = false;
                            }

                            pnlDisciplinaIngresso.Visible = false;
                            pnlLotacao.Visible = false;

                            DesabilitaCampos();
                            LimparCampos();
                        }
                        else
                        {
                            LimparTela();
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            apcDocente.Visible = false;
                            lblMensagem.Text = "Pessoa não cadastrada para este docente.";
                        }
                        break;
                    }

                case TipoOperacao.ConsultarPessoa:
                    {
                        //lbltxtNumFunc.Visible = true;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = false;
                        //txtNumFunc.Visible = false;
                        chkNaoPossuiIdFuncional.Visible = false;
                        LyPessoa dadosPessoa = new LyPessoa();

                        dadosPessoa = RN.Pessoa.Carregar(Convert.ToInt32(tsePessoa.DBValue.ToString()));
                        PreencherDadosTelaPessoa(dadosPessoa);
                        txtPessoa.Text = Convert.ToString(dadosPessoa.Pessoa);

                        //ControlarEnderecoPais();
                        break;
                    }
            }
        }

        private void CarregaCargo(string value)
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;
            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
            tseCategoria.SqlSelect = sqlSelect;
            tseCategoria.ResetValue();
            tseCategoria.SqlWhere = " FUNCIONARIO = 'N' ";

            if (!string.IsNullOrEmpty(txtProcesso.Text))
            {
                table = @" LY_CATEGORIA_DOCENTE CD
                           INNER JOIN CONTRATOTEMPORARIO.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD ON CD.CATEGORIA=CDCD.CATEGORIAID
                                        ";

                tseCategoria.SqlWhere = " FUNCIONARIO = 'N' AND CONCURSOID = '" + txtProcesso.Text.Trim() + "' ";
            }
            else
            {
                table = @" LY_CATEGORIA_DOCENTE";
                tseCategoria.SqlWhere = " FUNCIONARIO = 'N'";
            }

            coluna.Add("categoria");
            coluna.Add("nome");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseCategoria.SqlSelect = sqlSelect;
            tseCategoria.DBValue = value;
            tseCategoria.DataBind();
        }

        private void ControlarObrigatoriedadeDocumentos(string tipoDocumento)
        {
            if (ddlRGTipoPessoa.SelectedValue == "RG")
            {
                ddlRGUFPessoa.Enabled = true;
                lblRGUFPessoa.Text = "Estado*: ";
                lblRGUFPessoa.Font.Bold = true;

                dteRGDataExpPessoa.Enabled = true;
                lblRGDataExpPessoa.Text = "Data de Expedição*: ";
                lblRGDataExpPessoa.Font.Bold = true;
            }
            else
            {
                ddlRGUFPessoa.Enabled = false;
                lblRGUFPessoa.Text = "Estado: ";
                lblRGUFPessoa.Font.Bold = false;

                dteRGDataExpPessoa.Enabled = false;
                lblRGDataExpPessoa.Text = "Data de Expedição: ";
                lblRGDataExpPessoa.Font.Bold = false;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosPessoa">Linha com os dados da pessoa</param>
        private void PreencherDadosTelaPessoa(RN.Entidades.LyPessoa dadosPessoa)
        {
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            txtPessoa.Text = Convert.ToString(dadosPessoa.Pessoa);
            txtIDINEP.Text = string.Empty;

            if (dadosPessoa.IdFuncional != null && dadosPessoa.IdFuncional == 0)
            {
                chkNaoPossuiIdFuncional.Checked = true;
                txtIdFuncional.Text = string.Empty;
            }
            else
            {
                chkNaoPossuiIdFuncional.Checked = false;
                txtIdFuncional.Text = dadosPessoa.IdFuncional > 0 ? dadosPessoa.IdFuncional.ToString() : string.Empty;                
            }

            //Busca Zona Residencial
            string zonaResidencial =  rnFlPessoa.ObtemZonaResidencialPor(dadosPessoa.Pessoa);
            if (!zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                rblLocalizacaoUF.SelectedValue = zonaResidencial;
            }

            txtPISPASEP.Text = dadosPessoa.Pispasep;
            txtNomeComplPessoa.Text = dadosPessoa.Nome_compl;
            TxtNomeSocial.Text = dadosPessoa.Nome_social;
            if (dadosPessoa.Dt_nasc.HasValue)
            {
                dteDtNasc.Date = Convert.ToDateTime(dadosPessoa.Dt_nasc);
            }
            if (!string.IsNullOrEmpty(dadosPessoa.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosPessoa.Sexo) != null)
                {
                    rblSexo.Text = dadosPessoa.Sexo;
                }
            }
            ddlPovoIndigena.Visible = false;
            lblPovo.Visible = false;
            ddlPovoIndigena.ClearSelection();

            if (!dadosPessoa.Etnia.IsNullOrEmptyOrWhiteSpace())
            {
                if (ddlRaca.Items.FindByValue(dadosPessoa.Etnia) != null)
                {
                    ddlRaca.SelectedValue = dadosPessoa.Etnia;
                }

                if (ddlRaca.SelectedValue == "Índigena")
                {
                    CarregaPovoIndigena();
                    ddlPovoIndigena.Visible = true;
                    lblPovo.Visible = true;

                    string povoIndigena = rnFlPessoa.ObtemPovoIndigenaPor(dadosPessoa.Pessoa);
                    if (!povoIndigena.IsNullOrEmptyOrWhiteSpace())
                    {
                        ddlPovoIndigena.SelectedValue = povoIndigena;
                    }
                }
            }            

            txtEnderecoPessoa.Text = dadosPessoa.Endereco;
            txtEndNumPessoa.Text = dadosPessoa.End_num;
            txtEndCompl.Text = dadosPessoa.End_compl;

            if (!string.IsNullOrEmpty(dadosPessoa.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.Pais_nasc);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.Municipio_nasc);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipioNaturalidade.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstadoNaturalidade.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        //preenche os dados nos controles da tela
                        tseNaturalidade.DBValue = dadosPessoa.Municipio_nasc;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstadoNaturalidade.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.Municipio_nasc);
                    }
                    else
                    {
                        tseNaturalidade.ResetValue();
                        txtEstadoNaturalidade.Value = string.Empty;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.End_pais))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.End_pais);

                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.End_municipio);

                    if (sr != null)
                    {
                        if (!sr["nome"].IsNull)
                            txtMunicipio.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstado.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.End_municipio))
                    {
                        //preenche os dados nos controles da tela
                        tseMunicipio.DBValue = dadosPessoa.End_municipio;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.End_municipio);
                    }
                    else
                    {
                        tseMunicipio.ResetValue();
                        txtEstado.Value = string.Empty;
                    }
                }
            }

            txtBairro.Text = dadosPessoa.Bairro;
            txtCEP.Text = dadosPessoa.Cep;
            Int64 result;
            string fone = dadosPessoa.Fone.RetirarMascaraTelefone();
            if (Int64.TryParse(fone, out result))
                txtFone.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtFone.Text = dadosPessoa.Fone;

            long resultado;
            
            string celular = dadosPessoa.Celular.RetirarMascaraTelefone();
            if (long.TryParse(celular, out resultado))
            {
                if (dadosPessoa.Celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }
            else
            {
                txtCelular.Text = dadosPessoa.Celular;
            }

            txtEmailInstitucional.Text = dadosPessoa.E_mail_interno;
            txtEmail.Text = dadosPessoa.E_mail;
            txtRGNumPessoa.Text = dadosPessoa.Rg_num;

            if (dadosPessoa.Rg_dtexp.HasValue)
            {
                dteRGDataExpPessoa.Date = Convert.ToDateTime(dadosPessoa.Rg_dtexp);
            }

            txtDOC_Teleitor_Num.Text = dadosPessoa.Teleitor_num;
            txtDOC_Teleitor_Zona.Text = dadosPessoa.Teleitor_zona;
            txtDOC_Teleitor_Secao.Text = dadosPessoa.Teleitor_secao;
            if (dadosPessoa.Teleitor_dtexp.HasValue)
                dteDOC_Teleitor_DtExp.Date = dadosPessoa.Teleitor_dtexp.Value;
            txtDMIL_Alist_Num.Text = dadosPessoa.Alist_num;
            txtDMIL_Alist_RM.Text = dadosPessoa.Alist_rm;
            txtDMIL_Alist_Serie.Text = dadosPessoa.Alist_serie;
            txtDMIL_Alist_CSM.Text = dadosPessoa.Alist_csm;
            txtDMIL_Cr_Num.Text = dadosPessoa.Cr_num;
            txtDMIL_Cr_RM.Text = dadosPessoa.Cr_rm;
            txtDMIL_Cr_Serie.Text = dadosPessoa.Cr_serie;
            txtDMIL_Cr_CSM.Text = dadosPessoa.Cr_csm;
            txtDMIL_Cr_CAT.Text = dadosPessoa.Cr_cat;
            if (dadosPessoa.Cr_dtexp.HasValue)
                dteDMIL_Cr_DtExp.Date = dadosPessoa.Cr_dtexp.Value;
            if (dadosPessoa.Alist_dtexp.HasValue)
                dteDMIL_Alist_DtExp.Date = dadosPessoa.Alist_dtexp.Value;

            txtNomeMae.Text = dadosPessoa.NomeMae;
            txtNomePai.Text = dadosPessoa.NomePai;

            chkMaeNaoDeclarada.Checked = dadosPessoa.NomeMae == chkMaeNaoDeclarada.Text.ToUpper();
            chkPaiNaoDeclarado.Checked = dadosPessoa.NomePai == chkPaiNaoDeclarado.Text.ToUpper();

            if (chkMaeNaoDeclarada.Checked)
            {
                txtNomeMae.ReadOnly = true;
            }

            if (chkPaiNaoDeclarado.Checked)
            {
                txtNomePai.ReadOnly = true;
            }


            PreencherDadoCombo(ddlPaisNasc, Convert.ToString(dadosPessoa.Pais_nasc));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosPessoa.Rg_tipo));
            PreencherDadoCombo(ddlNecessidadeEspecial, Convert.ToString(dadosPessoa.NecessidadeEspecialId));
            PreencherDadoCombo(ddlPais, Convert.ToString(dadosPessoa.End_pais));
            PreencherDadoCombo(ddlRGUFPessoa, Convert.ToString(dadosPessoa.Rg_uf));
            PreencherDadoCombo(ddlRGEmissorPessoa, Convert.ToString(dadosPessoa.Rg_emissor));
            PreencherDadoCombo(txtCProfUF, Convert.ToString(dadosPessoa.Cprof_uf));
           
           
            if (!string.IsNullOrEmpty(dadosPessoa.Est_civil))
            {
                if (ddlEst_Civil.Items.FindByValue(dadosPessoa.Est_civil) != null)
                {
                    ddlEst_Civil.SelectedValue = dadosPessoa.Est_civil;
                }
                else
                {
                    ddlEst_Civil.SelectedValue = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Nacionalidade))
            {
                if (ddlNacionalidade.Items.FindByValue(dadosPessoa.Nacionalidade) != null)
                {
                    ddlNacionalidade.SelectedValue = dadosPessoa.Nacionalidade;
                }
            }     

            ControlarObrigatoriedadeDocumentos(ddlRGTipoPessoa.Text);

            if (Int64.TryParse(dadosPessoa.Cpf, out result))
                txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
            else
            {
                txtCPF.Text = dadosPessoa.Cpf;                
            }

            if (txtCPF.Text.IsNullOrEmptyOrWhiteSpace())
            {
                txtCPF.ReadOnly = false;
            }
            else
            {
                txtCPF.ReadOnly = true;
            }

            txtCProfNum.Text = dadosPessoa.Cprof_num;
            txtCProfSerie.Text = dadosPessoa.Cprof_serie;
            if (dadosPessoa.Cprof_dtexp.HasValue)
            {
                dteCProfDtExp.Date = Convert.ToDateTime(dadosPessoa.Cprof_dtexp);
            }

            chkAreaAssentamento.Checked = !dadosPessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaAssentamento == "S" ? true : false) : false;
            chkTerraIndigena.Checked = !dadosPessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.TerraIndigena == "S" ? true : false) : false;
            chkQuilombos.Checked = !dadosPessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaQuilombos == "S" ? true : false) : false;
            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked) ? true : false;

        }


        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTelaDocente(RN.Entidades.LyDocente dadosDocente)
        {
            txtNumFunc.Text = Convert.ToString(dadosDocente.Num_func);

            txtMatricula.Text = dadosDocente.Matricula;
            txtVinculo.Text = dadosDocente.Vinculo != null ? dadosDocente.Vinculo.ToString() : string.Empty;
            txtCH.Text = dadosDocente.Regime_trabalho;
            if (dadosDocente.Dt_admissao.HasValue)
            {
                dteDtAdmissao.Date = Convert.ToDateTime(dadosDocente.Dt_admissao);
            }
            else
            {
                dteDtAdmissao.Text = string.Empty;
            }

            if (dadosDocente.Dt_demissao.HasValue)
            {
                dteDtDemissao.Date = Convert.ToDateTime(dadosDocente.Dt_demissao);
            }

            txtProcesso.Text = dadosDocente.Concurso;
            txtCandidato.Text = dadosDocente.Candidato;            

            if (!dadosDocente.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                tseCategoria.DBValue = dadosDocente.Categoria.Trim();
            }

            if (dadosDocente.RegimeContratacaoId != null)
            {
                PreencherDadoCombo(cmbRegContratacao, Convert.ToString(dadosDocente.RegimeContratacaoId));

                if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString())
                {
                    btnImprimir.Visible = false;
                    lblCargaHoraria.Visible = true;
                    ddlCargaHoraria.Visible = true;
                    lblAulasAlocadas.Visible = true;
                    txtAulasAlocadas.Visible = true;
                    if (!string.IsNullOrEmpty(txtProcesso.Text.Trim()))
                    {
                        CarregaCargaHoraria(txtProcesso.Text.Trim(), dadosDocente.Categoria);

                        ListItem listItem = ddlCargaHoraria.Items.FindByValue(dadosDocente.Regime_trabalho);

                        if (listItem != null)
                        {
                            ddlCargaHoraria.SelectedValue = dadosDocente.Regime_trabalho;
                        }

                    }
                }
                else
                {
                    btnImprimir.Visible = true;
                    lblCargaHoraria.Visible = false;
                    ddlCargaHoraria.Visible = false;
                    lblAulasAlocadas.Visible = false;
                    txtAulasAlocadas.Visible = false;
                }
            }

            txtAnoConcurso.Text = Convert.ToString(dadosDocente.Ano_ingresso);   
        }       

        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            lblMensValidacao.Text = string.Empty;

            tseNaturalidade.ResetValue();
            txtEstadoNaturalidade.Value = string.Empty;
            txtMatricula.Text = string.Empty;
            txtPessoa.Text = string.Empty;
            txtIDINEP.Text = string.Empty;
            txtNomeComplPessoa.Text = string.Empty;
            TxtNomeSocial.Text = string.Empty;
            txtNomePai.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkMaeNaoDeclarada.Checked = false;
            chkPaiNaoDeclarado.Checked = false;
            dteDtNasc.Text = string.Empty;
            rblSexo.SelectedIndex = -1;
            rblLocalizacaoUF.SelectedIndex = -1;
            ddlEst_Civil.ClearSelection();
            ddlRaca.Items.Clear();
            ddlNecessidadeEspecial.Items.Clear();
            ddlPaisNasc.Items.Clear();
            ddlNacionalidade.Items.Clear();
            txtEnderecoPessoa.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            ddlPais.Items.Clear();
            txtMunicipio.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmailInstitucional.Text = string.Empty;
            txtEmailGoogle.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlRGTipoPessoa.Items.Clear();
            txtRGNumPessoa.Text = string.Empty;
            ddlRGUFPessoa.Items.Clear();
            dteRGDataExpPessoa.Text = string.Empty;
            ddlRGEmissorPessoa.Items.Clear();
            txtCPF.Text = string.Empty;
            txtPISPASEP.Text = string.Empty;
            txtCProfNum.Text = string.Empty;
            txtCProfSerie.Text = string.Empty;
            dteCProfDtExp.Text = string.Empty;
            txtCProfUF.Items.Clear();
            txtNumFunc.Text = string.Empty;
            dteDtAdmissao.Date = DateTime.Now;
            dteDtDemissao.Text = string.Empty;
            tseCategoria.ResetValue();
            cmbRegContratacao.Items.Clear();
            ddlCargaHoraria.ClearSelection();
            ddlCargaHoraria.Items.Clear();
            txtAulasAlocadas.Text = string.Empty;
            txtDMIL_Alist_Num.Text = string.Empty;
            txtDMIL_Alist_RM.Text = string.Empty;
            txtDMIL_Alist_Serie.Text = string.Empty;
            txtDMIL_Alist_CSM.Text = string.Empty;
            txtDMIL_Cr_Num.Text = string.Empty;
            txtDMIL_Cr_RM.Text = string.Empty;
            txtDMIL_Cr_Serie.Text = string.Empty;
            txtDMIL_Cr_CSM.Text = string.Empty;
            txtDMIL_Cr_CAT.Text = string.Empty;
            dteDMIL_Cr_DtExp.Text = string.Empty;
            dteDMIL_Alist_DtExp.Text = string.Empty;
            txtDOC_Teleitor_Num.Text = string.Empty;
            txtDOC_Teleitor_Zona.Text = string.Empty;
            txtDOC_Teleitor_Secao.Text = string.Empty;
            dteDOC_Teleitor_DtExp.Text = string.Empty;
            txtAnoConcurso.Text = string.Empty;
            rblAcumulacao.ClearSelection();
            txtMatriculaAcumulacao.Text = string.Empty;
            txtOrgaoAcumulacao.Text = string.Empty;
            txtNumProcessoAcumulacao.Text = string.Empty;
            tseFuncaoLotacao.ResetValue();
            tseRegionalLotacao.ResetValue();
            tseMunicipioLotacao.ResetValue();
            tseUnidadeLotacao.ResetValue();
            dtNomeacao.Text = string.Empty;
            tseDisciplinaIngresso.ResetValue();
            txtCH.Text = string.Empty;
            txtProcesso.Text = string.Empty;
            txtCandidato.Text = string.Empty;
            txtVinculo.Text = string.Empty;
            txtIdFuncional.Text = string.Empty;
            chkNaoPossuiIdFuncional.Checked = false;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            txtMunicipioNaturalidade.ReadOnly = false;
            txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");

            txtMunicipio.ReadOnly = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeComplPessoa.ReadOnly = false;
            TxtNomeSocial.ReadOnly = false;
            txtNomeMae.ReadOnly = false;
            txtNomePai.ReadOnly = false;
            chkPaiNaoDeclarado.Enabled = true;
            chkMaeNaoDeclarada.Enabled = true;
            dteDtNasc.Enabled = true;
            rblSexo.Enabled = true;
            rblLocalizacaoUF.Enabled = true;
            ddlRaca.Enabled = true;
            ddlPovoIndigena.Enabled = true;
            ddlNecessidadeEspecial.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlPaisNasc.Enabled = true;
            ddlNacionalidade.Enabled = true;
            ddlPais.Enabled = true;
            txtFone.ReadOnly = false;
            txtCelular.ReadOnly = false;
            txtEmailInstitucional.ReadOnly = false;
            txtEmailGoogle.Enabled = true;
            txtEmail.ReadOnly = false;
            ddlRGTipoPessoa.Enabled = true;
            txtRGNumPessoa.ReadOnly = false;
            ddlRGUFPessoa.Enabled = true;
            dteRGDataExpPessoa.Enabled = true;
            ddlRGEmissorPessoa.Enabled = true;
            txtCPF.ReadOnly = false;
            txtPISPASEP.ReadOnly = false;
            txtCProfNum.ReadOnly = false;
            txtCProfSerie.ReadOnly = false;
            dteCProfDtExp.Enabled = true;
            txtCProfUF.Enabled = true;
            dteDtAdmissao.Enabled = true;
            dteDtDemissao.Enabled = true;

            txtCEP.ReadOnly = false;
            tsCEP.ShowButton = true;
            txtEndCompl.ReadOnly = false;
            txtEndNumPessoa.ReadOnly = false;
            txtEnderecoPessoa.ReadOnly = false;
            txtBairro.ReadOnly = false;
            cmbRegContratacao.Enabled = true;
            ddlCargaHoraria.Enabled = true;
            txtAulasAlocadas.Enabled = true;

            txtDMIL_Alist_Num.ReadOnly = false;
            txtDMIL_Alist_RM.ReadOnly = false;
            txtDMIL_Alist_Serie.ReadOnly = false;
            txtDMIL_Alist_CSM.ReadOnly = false;
            txtDMIL_Cr_Num.ReadOnly = false;
            txtDMIL_Cr_RM.ReadOnly = false;
            txtDMIL_Cr_Serie.ReadOnly = false;
            txtDMIL_Cr_CSM.ReadOnly = false;
            txtDMIL_Cr_CAT.ReadOnly = false;
            dteDMIL_Cr_DtExp.Enabled = true;
            dteDMIL_Alist_DtExp.Enabled = true;
            txtDOC_Teleitor_Num.ReadOnly = false;
            txtDOC_Teleitor_Zona.ReadOnly = false;
            txtDOC_Teleitor_Secao.ReadOnly = false;
            dteDOC_Teleitor_DtExp.Enabled = true;

            txtAnoConcurso.ReadOnly = false;
            btnSalvarFormacao.Enabled = true;
            btnSalvarFormacaoPosGraduacao.Enabled = true;
            tseCategoria.Enabled = true;

            txtMatriculaAcumulacao.Enabled = true;
            txtOrgaoAcumulacao.Enabled = true;
            txtNumProcessoAcumulacao.Enabled = true;
            rblAcumulacao.Enabled = true;
            if (rblAcumulacao.SelectedValue == "1")
            {
                txtMatriculaAcumulacao.ReadOnly = false;
                txtOrgaoAcumulacao.ReadOnly = false;
                txtNumProcessoAcumulacao.ReadOnly = false;
            }
            else
            {
                txtMatriculaAcumulacao.ReadOnly = true;
                txtOrgaoAcumulacao.ReadOnly = true;
                txtNumProcessoAcumulacao.ReadOnly = true;
            }

            chkAreaAssentamento.Enabled = true;
            chkTerraIndigena.Enabled = true;
            chkQuilombos.Enabled = true;
            chkNaoSeAplica.Enabled = true;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            txtMunicipioNaturalidade.ReadOnly = true;
            txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
            txtMunicipio.ReadOnly = true;
            txtEstado.Attributes.Add("readonly", "readonly");

            txtMatricula.ReadOnly = true;
            txtVinculo.ReadOnly = true;
            txtIdFuncional.ReadOnly = true;

            txtNomeComplPessoa.ReadOnly = true;
            TxtNomeSocial.ReadOnly = true;
            txtNomeMae.ReadOnly = true;
            txtNomePai.ReadOnly = true;
            chkPaiNaoDeclarado.Enabled = false;
            chkMaeNaoDeclarada.Enabled = false;
            dteDtNasc.Enabled = false;
            rblSexo.Enabled = false;
            rblLocalizacaoUF.Enabled = false;
            ddlRaca.Enabled = false;
            ddlPovoIndigena.Enabled = false;
  
            ddlNecessidadeEspecial.Enabled = false;
            ddlEst_Civil.Enabled = false;
            ddlPaisNasc.Enabled = false;
            ddlNacionalidade.Enabled = false;
            txtFone.ReadOnly = true;
            txtCelular.ReadOnly = true;
            txtEmailInstitucional.ReadOnly = true;
            txtEmailGoogle.Enabled = false;
            txtEmail.ReadOnly = true;
            ddlRGTipoPessoa.Enabled = false;
            txtRGNumPessoa.ReadOnly = true;
            ddlRGUFPessoa.Enabled = false;
            dteRGDataExpPessoa.Enabled = false;
            ddlRGEmissorPessoa.Enabled = false;
            txtCPF.ReadOnly = true;
            txtPISPASEP.ReadOnly = true;
            txtCProfNum.ReadOnly = true;
            txtCProfSerie.ReadOnly = true;
            dteCProfDtExp.Enabled = false;
            txtCProfUF.Enabled = false;
            dteDtAdmissao.Enabled = false;
            dteDtDemissao.Enabled = false;

            ddlPais.Enabled = false;
            txtCEP.ReadOnly = true;
            tsCEP.ShowButton = false;

            txtEndCompl.ReadOnly = true;
            txtEndNumPessoa.ReadOnly = true;
            txtEnderecoPessoa.ReadOnly = true;
            txtBairro.ReadOnly = true;
            cmbRegContratacao.Enabled = false;
            ddlCargaHoraria.Enabled = false;
            txtAulasAlocadas.Enabled = false;

            txtDMIL_Alist_Num.ReadOnly = true;
            txtDMIL_Alist_RM.ReadOnly = true;
            txtDMIL_Alist_Serie.ReadOnly = true;
            txtDMIL_Alist_CSM.ReadOnly = true;
            txtDMIL_Cr_Num.ReadOnly = true;
            txtDMIL_Cr_RM.ReadOnly = true;
            txtDMIL_Cr_Serie.ReadOnly = true;
            txtDMIL_Cr_CSM.ReadOnly = true;
            txtDMIL_Cr_CAT.ReadOnly = true;
            dteDMIL_Cr_DtExp.Enabled = false;
            dteDMIL_Alist_DtExp.Enabled = false;
            txtDOC_Teleitor_Num.ReadOnly = true;
            txtDOC_Teleitor_Zona.ReadOnly = true;
            txtDOC_Teleitor_Secao.ReadOnly = true;
            dteDOC_Teleitor_DtExp.Enabled = false;

            txtAnoConcurso.ReadOnly = true;
            txtMatriculaAcumulacao.Enabled = false;
            txtOrgaoAcumulacao.Enabled = false;
            txtNumProcessoAcumulacao.Enabled = false;
            rblAcumulacao.Enabled = false;

            tseDisciplinaIngresso.Enabled = false;
            tseCategoria.Enabled = false;
            tseFuncaoLotacao.Enabled = false;
            tseRegionalLotacao.Enabled = false;
            tseMunicipioLotacao.Enabled = false;
            tseUnidadeLotacao.Enabled = false;
            dtNomeacao.Enabled = false;           
            chkNaoPossuiIdFuncional.Enabled = false;
            chkAreaAssentamento.Enabled = false;
            chkTerraIndigena.Enabled = false;
            chkQuilombos.Enabled = false;
            chkNaoSeAplica.Enabled = false;
        }


        /// <summary>
        /// Carrega os dados do banco na dropdownlist
        /// </summary>
        /// <param name="drop">DropDownList que será carregado</param>
        /// <param name="data">Dados que serão preenchidos</param>
        /// <param name="defaultValue">Valor padrão</param>
        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            //drop.SelectedIndex = -1;
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("<Nenhum>", "");
            drop.Items.Add(itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {               
                drop.SelectedValue = "";
                if (drop == ddlPais || drop == ddlPaisNasc)
                {
                    ListItem listItem = drop.Items.FindByText("BRASIL");
                    if (listItem != null)
                    {
                        drop.ClearSelection();
                        listItem.Selected = true;
                    }
                }
                if (drop == ddlNacionalidade)
                {
                    ListItem listItem = drop.Items.FindByText("BRASILEIRA");
                    if (listItem != null)
                    {
                        drop.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }
        }

        //Carrega combo "Necessidade Especial"
        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);

            ddlNecessidadeEspecial.Items.Clear();
            ddlNecessidadeEspecial.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            ddlNecessidadeEspecial.DataBind();
            ddlNecessidadeEspecial.Items.Insert(0, itemVazio);
        }

        //Carrega combo "Cor/Raça"
        private void CarregaEtnia()
        {
            RN.Etnia rnEtnia = new Etnia();
            ListItem item = new ListItem("<Nenhum>", string.Empty);

            ddlRaca.Items.Clear();
            ddlRaca.DataSource = rnEtnia.ListaEtniaAtiva();
            ddlRaca.DataBind();
            ddlRaca.Items.Insert(0, item);
        }

        /// <summary>
        /// Faz a consulta ao banco para cada DropDownList e chama o método para carregá-los
        /// </summary>
        /// <param name="idDrop">ID da DropDownList</param>
        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {
                    case "ddlEst_Civil":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
                            CarregarDropDownList(ddlEst_Civil, dadosDrop, "");
                            break;
                        }                   
                    case "ddlNacionalidade":
                        {
                            dadosDrop = RN.Basico.ConsultarNacionalidade();
                            CarregarDropDownList(ddlNacionalidade, dadosDrop, "");
                            break;
                        }
                    case "ddlPaisNasc":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaisNasc, dadosDrop, "");
                            break;
                        }
                    case "ddlRGTipoPessoa":
                        {
                            string param = "TIPO DOC";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGTipoPessoa, dadosDrop, "");
                            break;
                        }
                    case "ddlPais":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPais, dadosDrop, "");
                            break;
                        }

                    case "ddlRGEmissorPessoa":
                        {
                            string param = "ORGAO RG";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGEmissorPessoa, dadosDrop, "");
                            break;
                        }

                    case "ddlRGUFPessoa":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddlRGUFPessoa, dadosDrop, "");
                            break;
                        }
                    case "txtCProfUF":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(txtCProfUF, dadosDrop, "");
                            break;
                        }

                    case "cmbPaisNasc":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            //CarregarDropDownList(cmbPaisNasc, dadosDrop, "");
                            break;
                        }
                    case "cmbRegContratacao":
                        {
                            cmbRegContratacao.Items.Clear();
                            RN.RecursosHumanos.RegimeContratacao rnRegimeContratacao = new Techne.Lyceum.RN.RecursosHumanos.RegimeContratacao();

                            DataTable dtRegime = new DataTable();

                            if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                            {
                                dtRegime = rnRegimeContratacao.ListaRegimeContratacaoIngresso();
                            }
                            else
                            {
                                dtRegime = rnRegimeContratacao.ListaRegimeContratacao();
                            }
                            cmbRegContratacao.DataSource = dtRegime;
                            cmbRegContratacao.DataBind();
                            cmbRegContratacao.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            break;
                        }
                }
            }
            catch
            {
                throw;
            }
            return dadosDrop;
        }

        #endregion

        #region Controlar Visibilidade das TSearchs
        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                case TipoOperacao.Sucesso:
                    {
                        tseDocentes.Enabled = true;
                        lblPessoaTSearch.Visible = false;
                        tsePessoa.Visible = false;
                        //tseMunicipio.Mode = ControlMode.View;
                        //tseNaturalidade.Mode = ControlMode.View;

                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseDocentes.Enabled = false;
                        lblPessoaTSearch.Visible = true;
                        tsePessoa.Visible = true;
                        //tseMunicipio.Mode = ControlMode.Edit;
                        //tseNaturalidade.Mode = ControlMode.Edit;
                        break;
                    }               
                case TipoOperacao.Alterar:
                    {
                        tseDocentes.Enabled = false;
                        lblPessoaTSearch.Visible = false;
                        tsePessoa.Visible = false;
                        //tseMunicipio.Mode = ControlMode.Edit;
                        //tseNaturalidade.Mode = ControlMode.Edit;
                        break;
                    }
                case TipoOperacao.ConsultarDocente:
                    {
                        tseDocentes.Enabled = true;
                        lblPessoaTSearch.Visible = false;
                        tsePessoa.Visible = false;
                        //tseMunicipio.Mode = ControlMode.View;
                        //tseNaturalidade.Mode = ControlMode.View;
                        break;
                    }
                case TipoOperacao.ConsultarPessoa:
                    {
                        tseDocentes.Enabled = false;
                        lblPessoaTSearch.Visible = true;
                        tsePessoa.Visible = true;
                        //tseMunicipio.Mode = ControlMode.Edit;
                        //tseNaturalidade.Mode = ControlMode.Edit;
                        break;
                    }
            }
        }
        #endregion

        protected void tseDocentes_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseMunicipio_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tsePessoa_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseNaturalidade_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }


        protected void ddlEscolaridade_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nomeEscolaridade = ddlEscolaridade.SelectedValue.ToString().Trim();

            if (nomeEscolaridade == "Ensino Médio Normal/Magisterio" ||
               nomeEscolaridade == "Ensino Médio Normal/Magisterio Especifico Indigena" ||
               nomeEscolaridade == "Ensino Médio Normal/Magistério - Estudos Adicionais")
            {
                ddlSituacaoCurso.Text = "Concluído";
                ddlSituacaoCurso.Enabled = false;

            }
            else
            {
                ddlSituacaoCurso.Enabled = true;
            }

            if (nomeEscolaridade == "Superior Licenciatura")
            {
                ddlFormComplementPedag.Text = "Não";
                ddlFormComplementPedag.Enabled = false;
            }

            int posadicional = nomeEscolaridade.ToUpper().IndexOf("ADICIONAIS");

            if (posadicional >= 0)
            {
                pnDisciplinaAdicional.Enabled = true;
                chkListDisciplinaAdicional.Enabled = true;
                chkListDisciplinaAdicional.Visible = true;
                pnDisciplinaAdicional.Visible = true;

            }
            else
            {
                pnDisciplinaAdicional.Enabled = false;
                chkListDisciplinaAdicional.Enabled = false;
                chkListDisciplinaAdicional.Visible = false;
                pnDisciplinaAdicional.Visible = false;
            }

            string curso = "";
            int poslicenciatura = -1;
            if (string.IsNullOrEmpty(ddlCurso.SelectedValue) == false)
            {
                curso = ddlCurso.SelectedItem.Text.Trim().ToUpper();
                poslicenciatura = curso.IndexOf("LICENCIATURA");
            }


            if (nomeEscolaridade.Length >= 20)
            {
                string trecho = nomeEscolaridade.Substring(0, 20);


                if (string.IsNullOrEmpty(curso) == false)
                {


                    if (trecho == "Superior Bacharelado")
                    {
                        ddlFormComplementPedag.Enabled = true;
                    }
                    else
                    {
                        if (poslicenciatura > -1)
                        {
                            ddlFormComplementPedag.Enabled = false;
                            ddlFormComplementPedag.Text = "Não";
                        }
                    }

                }
            }



        }

        protected void ddlAreaCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAreaCurso.SelectedValue != "Selecione")
            {
                ddlCurso.Items.Clear();
                ddlCurso.DataSource = RN.CursoFormacaoPessoal.Listar(int.Parse(ddlAreaCurso.SelectedValue));
                ddlCurso.Items.Insert(0, "Selecione");
                ddlCurso.DataBind();
                ddlCurso.Enabled = true;

            }
            else
            {
                ddlCurso.Enabled = false;
            }

        }


        protected void ddlTipoInstituicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            tseInstituicao.ResetValue();

            if (ddlTipoInstituicao.SelectedValue != "Selecione")
            {
                tseInstituicao.DataBind();
            }
        }

        protected void tseInstituicao_Changed(object sender, EventArgs args)
        {
            if (tseInstituicao.IsValidDBValue && !tseInstituicao.DBValue.IsNull)
            {
                //  lblMensagem.Text = string.Empty;
            }
            else if (!tseDocentes.DBValue.IsNull)
            {
                //   lblMensagem.Text = "Instituição não cadastrada.";
            }
            else
            {
                //   lblMensagem.Text = "Favor consultar uma Instituição.";
            }
        }
        protected void btnSalvarFormacao_Click(object sender, EventArgs e)
        {
            blocoGravacaoFormacao(1); //Graduação
        }

        protected void blocoGravacaoFormacao(int x)
        {
            // x == 1 Graduação
            // x == 2 Pós-Graduação
            lblMensagem.Text = string.Empty;
            lblMensValidacao.Text = string.Empty;
            Techne.Lyceum.RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();

            if (ValidarPreenchimentoCap(x)) //Revisado
            {
                if (VerificarCamposCap(x))
                {
                    TceFormacaoPessoal TFP = new TceFormacaoPessoal();
                    var validacao = new ValidacaoDados();

                    TFP.Pessoa = int.Parse(txtPessoa.Text.ToString());
                    TFP.Escolaridade = x == 1 ? ddlEscolaridade.SelectedValue : ddlEscolaridadePosGraduacao.SelectedValue;
                    TFP.SituacaoCurso = x == 1 ? ddlSituacaoCurso.SelectedValue : ddlSituacaoCursoPosGraduacao.SelectedValue;
                    TFP.IdCursoFormacaoPessoal = int.Parse(x == 1 ? ddlCurso.SelectedValue : ddlCursoPosGraduacao.SelectedValue);
                    TFP.FormacaoComplementacaoPedagogica = x == 1 ? ddlFormComplementPedag.SelectedValue : ddlFormComplementPedagPosGraduacao.SelectedValue;
                    TFP.AnoInicio = int.Parse(x == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim());

                    if (!string.IsNullOrEmpty(x == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))
                    {
                        TFP.AnoConclusao = int.Parse(x == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim());
                    }
                    TFP.IdInstituicao = x == 1 ? tseInstituicao.DBValue.ToString() : tseInstituicaoPosGraduacao.DBValue.ToString();
                    TFP.Matricula = User.Identity.Name.ToString();
                    TFP.Doc_comprobatorio = (x == 1 ? ckDocComprob.Checked : ckDocComprobPosGraduacao.Checked) == true ? "Sim" : "Não";


                    if ((btnSalvarFormacao.Text != "Salvar Formação-Pessoal" && btnSalvarFormacaoPosGraduacao.Text == "Incluir Formação Pessoal - Pós-Graduação") ||
                        (btnSalvarFormacao.Text == "Incluir Formação Pessoal - Graduação" && btnSalvarFormacaoPosGraduacao.Text != "Salvar Formação-Pessoal Pós-Graduação"))
                    {
                        validacao = RN.FormacaoPessoal.Validar(TFP);

                        if (validacao.Valido == true)
                        {
                            validacao = RN.FormacaoPessoal.ValidarPreRequisito(TFP);
                        }
                        if (validacao.Valido)
                        {
                            if (RN.FormacaoPessoal.Inserir(TFP) > 0)
                            {
                                txtFormacaoPessoalID.Text = RN.FormacaoPessoal.ObtemIdentityFormacao().ToString();

                                if (validaCheckAdicional())
                                {
                                    percorreIncluirCheckAdicional();
                                }
                                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Formação Pessoal incluída com sucesso.');", true);
                                odsFormacaoPessoal.Select();
                                odsFormacaoPessoal.DataBind();
                                grdFormacaoPessoal.DataBind();
                                chkListDisciplinaAdicional.Visible = false;
                                chkListDisciplinaAdicional.Items.Clear();

                                if (rnCategoriaDocente.NecessitaCursoSuperior(tseCategoria.DBValue.ToString()))
                                {
                                    lblMensValidacao.Text = "Informativo: Para este cargo será necessário cadastrar o Ensino Superior";
                                }
                            }
                        }
                        else
                        {
                            lblMensValidacao.Text = validacao.Mensagem;
                        }
                    }
                    else
                    {
                        TFP.IdFormacaoPessoal = Convert.ToInt32(ViewState["idFormacaoPessoa"]);
                        validacao = RN.FormacaoPessoal.ValidarPreRequisito(TFP);
                        if (validacao.Valido)
                        {
                            if (RN.FormacaoPessoal.Alterar(TFP) > 0)
                            {
                                txtFormacaoPessoalID.Text = RN.FormacaoPessoal.ObtemIdentityFormacao().ToString();
                                string idFormacaoPessoal = ViewState["idFormacaoPessoa"].ToString();
                                DataTable dtDisciplinasAdicionais = RN.FormacaoPessoal.ListarDisciplinaAdicional(idFormacaoPessoal);
                                if (validaCheckAdicional())
                                {
                                    if (dtDisciplinasAdicionais.Rows.Count > 0)
                                    {
                                        if (RN.FormacaoPessoal.DeletarFormacaoPessoalAdicional(Convert.ToInt32(ViewState["idFormacaoPessoa"])) > 0)
                                        {

                                        }
                                    }
                                    percorreIncluirCheckAdicional();

                                    chkListDisciplinaAdicional.Items.Clear();
                                    pnDisciplinaAdicional.Enabled = false;
                                    chkListDisciplinaAdicional.Enabled = false;
                                    chkListDisciplinaAdicional.Visible = false;
                                    pnDisciplinaAdicional.Visible = false;
                                }
                                else
                                {
                                    lblMensValidacao.Text = validacao.Mensagem;
                                }

                                odsFormacaoPessoal.Select();
                                odsFormacaoPessoal.DataBind();
                                grdFormacaoPessoal.DataBind();

                                btnSalvarFormacao.Text = "Incluir Formação Pessoal - Graduação";
                                btnSalvarFormacaoPosGraduacao.Text = "Incluir Formação Pessoal - Pós-Graduação";
                                ddlCurso.Enabled = false;

                                if (rnCategoriaDocente.NecessitaCursoSuperior(Convert.ToString(tseCategoria.DBValue)))
                                {
                                    lblMensValidacao.Text = "Informativo: Para este cargo será necessário cadastrar o Ensino Superior";
                                }
                            }
                        }
                        else
                        {
                            lblMensValidacao.Text = validacao.Mensagem;
                        }
                    }
                }

                if (lblMensValidacao.Text == string.Empty)
                {
                    LimparCampos();
                }
            }
        }

        private bool ValidarPreenchimentoCap(int tipo)
        {
            Instituicao rnInstituicao = new Instituicao();
            string _message = "Não é possível deixar de preencher os seguintes campos.<br>Campos Necessários: ";
            bool Valido = true;

            if (_tipoOperacao.Equals(TipoOperacao.Sucesso) && string.IsNullOrEmpty(txtPessoa.Text))
            {
                _message += "- Pessoa ";
                Valido = false;
            }
            if (_tipoOperacao.Equals(TipoOperacao.Novo) && string.IsNullOrEmpty(txtPessoa.Text))
            {
                _message += "- Pessoa ";
                Valido = false;
            }

            if ((tipo == 1 ? ddlEscolaridade.SelectedValue : ddlEscolaridadePosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Escolaridade ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlSituacaoCurso.SelectedValue : ddlSituacaoCursoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Situação do Curso ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlAreaCurso.SelectedValue : ddlAreaCursoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Área do Curso ";
                Valido = false;
            }
            if (((tipo == 1 ? ddlCurso.SelectedValue : ddlCursoPosGraduacao.SelectedValue) == "Selecione") || ((tipo == 1 ? ddlCurso.SelectedValue : ddlCursoPosGraduacao.SelectedValue) == string.Empty))
            {
                _message += "- Curso ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlFormComplementPedag.SelectedValue : ddlFormComplementPedagPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Formação/Complementação Pedagógica ";
                Valido = false;
            }
            if (string.IsNullOrEmpty(tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim()))
            {
                _message += "- Ano de Início ";
                Valido = false;
            }
            if ((tipo == 1 ? ddlTipoInstituicao.SelectedValue : ddlTipoInstituicaoPosGraduacao.SelectedValue) == "Selecione")
            {
                _message += "- Tipo de Instituição ";
                Valido = false;
            }
            if (!(tipo == 1 ? tseInstituicao.IsValidDBValue : tseInstituicaoPosGraduacao.IsValidDBValue) || (tipo == 1 ? tseInstituicao.DBValue.IsNull : tseInstituicaoPosGraduacao.DBValue.IsNull))
            {
                _message += "- Nome da Instituição ";
                Valido = false;
            }
            else
            {
                if (tipo == 1 ? !rnInstituicao.ExisteInstituicao(tseInstituicao.Text) : !rnInstituicao.ExisteInstituicao(tseInstituicaoPosGraduacao.Text))
                {
                    _message += "- Instituição ";
                    Valido = false;
                    if (tipo == 1)
                    {
                        tseInstituicao.ResetValue();
                    }
                    else
                    {
                        tseInstituicaoPosGraduacao.ResetValue();
                    }
                }
            }

            if (!Valido)
            {
                //lblMensagem.Text = _message;
                lblMensValidacao.Text = _message;
            }

            return Valido;
        }

        private bool VerificarCamposCap(int tipo)
        {
            string _message = "Verificações:<br> ";
            bool Valido = true;

            if (((tipo == 1 ? ddlSituacaoCurso.SelectedValue : ddlSituacaoCursoPosGraduacao.SelectedValue) == "Concluído") && (string.IsNullOrEmpty((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))))
            {
                _message += "- O campo 'Ano de Conclusão' deve ser preenchido.";
                Valido = false;
            }
            if (!string.IsNullOrEmpty((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())))
            {
                if (int.Parse((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())) < 1930)
                {
                    _message += "- O campo 'Ano de Início' deve ser maior que 1930.";
                    Valido = false;
                }
                if (int.Parse((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())) == 0)
                {
                    _message += "- O campo 'Ano de Início' não pode ser igual a zero(0).";
                    Valido = false;
                }
                if ((tipo == 1 ? txtAnoInicio.Text.Length : txtAnoInicioPosGraduacao.Text.Length) != 4)
                {
                    _message += "- O campo 'Ano de Início' deve ter 4 dígitos.";
                    Valido = false;
                }
                if (int.Parse((tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim())) > DateTime.Now.Year)
                {
                    _message += "- O campo 'Ano de Início' não pode ser maior que ano vigente.";
                    Valido = false;
                }

            }
            if (!string.IsNullOrEmpty((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim())))
            {
                if (int.Parse((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim())) < 1930)
                {
                    _message += "- O campo 'Ano de Conclusão' deve ser maior que 1930.";
                    Valido = false;
                }
                if (int.Parse((tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim())) == 0)
                {
                    _message += "- O campo 'Ano de Conclusão' não pode ser igual a zero(0).";
                    Valido = false;
                }
                if ((tipo == 1 ? txtAnoConclusao.Text.Length : txtAnoConclusaoPosGraduacao.Text.Length) != 4)
                {
                    _message += "- O campo 'Ano de Conclusão' deve ter 4 dígitos.";
                    Valido = false;
                }

            }

            if (!string.IsNullOrEmpty(tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim()) && !string.IsNullOrEmpty(tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))
            {
                if (int.Parse(tipo == 1 ? txtAnoInicio.Text.Trim() : txtAnoInicioPosGraduacao.Text.Trim()) > int.Parse(tipo == 1 ? txtAnoConclusao.Text.Trim() : txtAnoConclusaoPosGraduacao.Text.Trim()))
                {
                    _message += "- Ano de Início não pode ser superior ao Ano de Conclusão ";
                    Valido = false;
                }
            }

            if (!Valido)
            {
                lblMensValidacao.Text = _message;
            }

            return Valido;
        }

        private void LimparCampos()
        {
            ddlEscolaridade.ClearSelection();
            ddlSituacaoCurso.ClearSelection();
            ddlAreaCurso.ClearSelection();
            ddlCurso.Items.Clear();
            ddlFormComplementPedag.ClearSelection();
            txtAnoConclusao.Text = string.Empty;
            txtAnoInicio.Text = string.Empty;
            ddlTipoInstituicao.ClearSelection();
            tseInstituicao.ResetValue();
            ckDocComprob.Checked = false;

            ddlEscolaridadePosGraduacao.ClearSelection();
            ddlSituacaoCursoPosGraduacao.ClearSelection();
            ddlAreaCursoPosGraduacao.ClearSelection();
            ddlCursoPosGraduacao.Items.Clear();
            ddlFormComplementPedagPosGraduacao.ClearSelection();
            txtAnoConclusaoPosGraduacao.Text = string.Empty;
            txtAnoInicioPosGraduacao.Text = string.Empty;
            ddlTipoInstituicaoPosGraduacao.ClearSelection();
            tseInstituicaoPosGraduacao.ResetValue();
            ckDocComprobPosGraduacao.Checked = false;

        }

        protected void ddlAreaCursoPosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAreaCursoPosGraduacao.SelectedValue != "Selecione")
            {
                ddlCursoPosGraduacao.Items.Clear();
                ddlCursoPosGraduacao.DataSource = RN.CursoFormacaoPessoal.Listar(int.Parse(ddlAreaCursoPosGraduacao.SelectedValue));
                ddlCursoPosGraduacao.Items.Insert(0, "Selecione");
                ddlCursoPosGraduacao.DataBind();
                ddlCursoPosGraduacao.Enabled = true;
            }
            else
            {
                ddlCursoPosGraduacao.Enabled = false;
            }
        }

        protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCurso.SelectedItem.Text.Trim().ToUpper().IndexOf("LICENCIATURA") > 0)
            {
                ddlFormComplementPedag.Enabled = false;
                ddlFormComplementPedag.Text = "Não";
            }
            else
            {
                ddlFormComplementPedag.Enabled = true;
                ddlFormComplementPedag.Text = "Selecione";
            }
        }

        protected void ddlCursoPosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCursoPosGraduacao.SelectedItem.Text.Trim().ToUpper().IndexOf("LICENCIATURA") > 0)
            {
                ddlFormComplementPedagPosGraduacao.Enabled = false;
                ddlFormComplementPedagPosGraduacao.Text = "Não";
            }
            else
            {
                ddlFormComplementPedagPosGraduacao.Enabled = true;
                ddlFormComplementPedagPosGraduacao.Text = "Selecione";
            }
        }

        protected void ddlTipoInstituicaoPosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            tseInstituicaoPosGraduacao.ResetValue();

            if (ddlTipoInstituicaoPosGraduacao.SelectedValue != "Selecione")
            {
                tseInstituicaoPosGraduacao.DataBind();
            }
        }

        protected void ddlEscolaridadePosGraduacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nomeEscolaridade = ddlEscolaridadePosGraduacao.SelectedValue.ToString().Trim();

            if (nomeEscolaridade == "Ensino Médio Normal/Magisterio" ||
               nomeEscolaridade == "Ensino Médio Normal/Magisterio Especifico Indigena" ||
               nomeEscolaridade == "Ensino Médio Normal/Magisterio Estudos Adicionais")
            {
                ddlSituacaoCursoPosGraduacao.Text = "Concluído";
                ddlSituacaoCursoPosGraduacao.Enabled = false;

            }
            else
            {
                ddlSituacaoCursoPosGraduacao.Enabled = true;
            }

            string curso = "";
            int poslicenciatura = -1;
            if (string.IsNullOrEmpty(ddlCursoPosGraduacao.SelectedValue) == false)
            {
                curso = ddlCursoPosGraduacao.SelectedItem.Text.Trim().ToUpper();
                poslicenciatura = curso.IndexOf("LICENCIATURA");
            }

            if (nomeEscolaridade.Length >= 20)
            {
                string trecho = nomeEscolaridade.Substring(0, 20);

                if (string.IsNullOrEmpty(curso) == false)
                {
                    if (trecho == "Superior Bacharelado")
                    {
                        ddlFormComplementPedagPosGraduacao.Enabled = true;
                    }
                    else
                    {
                        if (poslicenciatura > -1)
                        {
                            ddlFormComplementPedagPosGraduacao.Enabled = false;
                            ddlFormComplementPedagPosGraduacao.Text = "Não";
                        }
                    }
                }
            }
        }

        protected void btnSalvarFormacaoPosGraduacao_Click(object sender, EventArgs e)
        {
            blocoGravacaoFormacao(2); //Pós-Graduação
        }

        protected void grdFormacaoPessoal_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdFormacaoPessoal.Settings.ShowFilterRow = false;
        }

        protected void grdFormacaoPessoal_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            //report.Text = e.CommandArgument.ToString();
            if (e.ButtonID == "Editar")
            {
                string escolaridade = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ESCOLARIDADE"));
                string area = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "AREA"));
                ID_FORMACAO_PESSOAL = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ID_FORMACAO_PESSOAL"));
                ViewState["idFormacaoPessoa"] = ID_FORMACAO_PESSOAL;
                if (escolaridade.Contains("Superior") || escolaridade.Contains("Ensino Médio"))
                {
                    ddlEscolaridade.Text = string.Empty;
                    if (!string.IsNullOrEmpty(escolaridade))
                    {
                        if (ddlEscolaridade.Items.FindByText(escolaridade) != null)
                        {
                            ddlEscolaridade.Text = escolaridade;
                        }                       
                    }
                  
                    ddlEscolaridade_SelectedIndexChanged(sender, e);
                    DataTable dtDisciplinasAdicionais = RN.FormacaoPessoal.ListarDisciplinaAdicional(ID_FORMACAO_PESSOAL);
                    int posadicional = escolaridade.ToUpper().IndexOf("ADICIONAIS");

                    if (posadicional >= 0)
                    {
                        pnDisciplinaAdicional.Enabled = true;
                        chkListDisciplinaAdicional.Enabled = true;
                        chkListDisciplinaAdicional.Visible = true;
                        pnDisciplinaAdicional.Visible = true;
                        chkListDisciplinaAdicional.DataBind();
                    }
                    else
                    {
                        pnDisciplinaAdicional.Enabled = false;
                        chkListDisciplinaAdicional.Enabled = false;
                        chkListDisciplinaAdicional.Visible = false;
                        pnDisciplinaAdicional.Visible = false;
                    }
                    if (dtDisciplinasAdicionais.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtDisciplinasAdicionais.Rows)
                        {
                            foreach (DataColumn dc in dtDisciplinasAdicionais.Columns)
                            {
                                if (dc.Caption.Equals("NOME_DISCIPLINA_ADICIONAL"))
                                {
                                    foreach (ListItem item in chkListDisciplinaAdicional.Items)
                                    {
                                        string listaDiscAdicional = row["NOME_DISCIPLINA_ADICIONAL"].ToString();
                                        if (item.Text == listaDiscAdicional)
                                        {
                                            item.Selected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ddlSituacaoCurso.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "SITUACAO_CURSO"));
                    ddlAreaCurso.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOAREA"));
                    ddlAreaCurso_SelectedIndexChanged(sender, e);
                    ddlCurso.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOCURSO"));
                    ddlCurso_SelectedIndexChanged(sender, e);
                    ddlFormComplementPedag.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "FORMACAO_COMPLEMENTACAO_PEDAGOGICA"));
                    txtAnoInicio.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_INICIO"));
                    txtAnoConclusao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_CONCLUSAO"));

                    if (Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO")) != DBNull.Value.ToString())
                    {
                        ddlTipoInstituicao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO"));
                    }

                    tseInstituicao.DataBind();
                    tseInstituicao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ID_INSTITUICAO"));
                    string docComprobatorios = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "DOC_COMPROBATORIO"));

                    if (docComprobatorios.Equals("Sim"))
                    {
                        ckDocComprob.Checked = true;
                    }
                    else
                    {
                        ckDocComprob.Checked = false;
                    }

                    btnSalvarFormacao.Text = "Salvar Formação-Pessoal";
                }
                else if (escolaridade.Contains("Pós-Graduação"))
                {
                    ddlEscolaridadePosGraduacao.Text = string.Empty;
                    if (!string.IsNullOrEmpty(escolaridade))
                    {
                        if (ddlEscolaridadePosGraduacao.Items.FindByText(escolaridade) != null)
                        {
                            ddlEscolaridadePosGraduacao.Text = escolaridade;
                        }
                    }

                    ddlEscolaridadePosGraduacao_SelectedIndexChanged(sender, e);
                    ddlSituacaoCursoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "SITUACAO_CURSO"));
                    ddlAreaCursoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOAREA"));
                    ddlAreaCursoPosGraduacao_SelectedIndexChanged(sender, e);
                    ddlCursoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "CODIGOCURSO"));
                    ddlCursoPosGraduacao_SelectedIndexChanged(sender, e);
                    ddlFormComplementPedagPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "FORMACAO_COMPLEMENTACAO_PEDAGOGICA"));
                    txtAnoInicioPosGraduacao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_INICIO"));
                    txtAnoConclusaoPosGraduacao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ANO_CONCLUSAO"));

                    if (Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO")) != DBNull.Value.ToString())
                    {
                        ddlTipoInstituicaoPosGraduacao.SelectedValue = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "TIPOINSTITUICAO"));
                    }

                    tseInstituicaoPosGraduacao.DataBind();
                    tseInstituicaoPosGraduacao.Text = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "ID_INSTITUICAO"));
                    string docComprobatoriosPosGrad = Convert.ToString(grdFormacaoPessoal.GetRowValues(e.VisibleIndex, "DOC_COMPROBATORIO"));

                    if (docComprobatoriosPosGrad.Equals("Sim"))
                    {
                        ckDocComprobPosGraduacao.Checked = true;
                    }
                    else
                    {
                        ckDocComprobPosGraduacao.Checked = false;
                    }

                    btnSalvarFormacaoPosGraduacao.Text = "Salvar Formação-Pessoal Pós-Graduação";
                }
            }
        }

        protected void grdFormacaoPessoal_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdFormacaoPessoal.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PESSOA")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ID_INSTITUICAO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "AREA")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "NOME_COMP")
                    e.Editor.Enabled = false;
            }
            else if (grdFormacaoPessoal.IsEditing)
            {
                if ((e.Column.FieldName) == "PESSOA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ID_INSTITUICAO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "AREA")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "NOME_COMP")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "AREA_CURSO")
                {
                    ASPxComboBox cmbcurso = (e.Editor as ASPxComboBox);

                    cmbcurso.Items.Clear();
                    cmbcurso.DataSource = RN.CursoFormacaoPessoal.ListarCursoArea();
                    cmbcurso.TextField = "AREA_CURSO";
                    cmbcurso.ValueField = "CODIGO";
                    cmbcurso.DataBind();

                    var item = cmbcurso.Items.FindByText((string)e.Value);

                    if (item != null)
                    {
                        item.Selected = true;
                    }

                    // cmbcurso.Items.FindByText((string)e.Value).Selected = true;
                }

                if ((e.Column.FieldName) == "ESCOLARIDADE")
                {
                    ASPxComboBox cmbEscolaridade = (e.Editor as ASPxComboBox);
                    //  PreecherComboTabGeral(cmbEscolaridade, "EscolaridadeFormacao");
                }

                if ((e.Column.FieldName) == "SITUACAO_CURSO")
                {
                    ASPxComboBox cmbSituacaoCurso = (e.Editor as ASPxComboBox);
                    // PreecherComboTabGeral(cmbSituacaoCurso, "SituacaoCursoForm");
                }
                if ((e.Column.FieldName) == "FORMACAO_COMPLEMENTACAO_PEDAGOGICA")
                {
                    ASPxComboBox cmbFormacao = (e.Editor as ASPxComboBox);
                    //  PreecherComboTabGeral(cmbFormacao, "FormacaoComplement");
                }

            }
        }

        protected void grdFormacaoPessoal_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFormacaoPessoal.Settings.ShowFilterRow = false;
        }

        protected void grdFormacaoPessoal_Init(object sender, EventArgs e)
        {
            //GridViewDataComboBoxColumn cmbcurso = grdFormacaoPessoal.Columns["AREA_CURSO"] as GridViewDataComboBoxColumn;
            //cmbcurso.PropertiesComboBox.DataSource = RN.CursoFormacaoPessoal.ListarCursoArea();
            //cmbcurso.PropertiesComboBox.TextField = "AREA_CURSO";
            //cmbcurso.PropertiesComboBox.ValueField = "CODIGO";
        }

        protected void grdFormacaoPessoal_AutoFilterCellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "DOC_COMPROBATORIO")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }
        }

        protected void grdFormacaoPessoal_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["ESCOLARIDADE"])))
            {
                e.RowError = "Favor informar a Escolaridade.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["SITUACAO_CURSO"])))
            {
                e.RowError = "Favor informar a Situação do Curso.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["AREA_CURSO"])))
            {
                e.RowError = "Favor informar o Curso.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["FORMACAO_COMPLEMENTACAO_PEDAGOGICA"])))
            {
                e.RowError = "Favor informar a Formação/Complementação Pedagógica.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["ANO_INICIO"])))
            {
                e.RowError = "Favor informar o Ano de Início.";
            }

            if ((Convert.ToString(e.NewValues["SITUACAO_CURSO"]) == "Concluído") && e.NewValues["ANO_CONCLUSAO"] == null)
            {
                e.RowError = "O campo 'Ano de Conclusão' deve ser preenchido.";
            }

            if (!string.IsNullOrEmpty(e.NewValues["ANO_INICIO"].ToString()) && e.NewValues["ANO_CONCLUSAO"] != null)
            {
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) > int.Parse(e.NewValues["ANO_CONCLUSAO"].ToString()))
                {
                    e.RowError = "Ano de Início não pode ser superior ao Ano de Conclusão ";

                }
            }
            if (!string.IsNullOrEmpty(e.NewValues["ANO_INICIO"].ToString()))
            {
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) < 1930)
                {
                    e.RowError = " O campo 'Ano de Início' deve ser maior que 1930.";
                }
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) == 0)
                {
                    e.RowError = "O campo 'Ano de Início' não pode ser igual a zero(0).";
                }
                if (e.NewValues["ANO_INICIO"].ToString().Length != 4)
                {
                    e.RowError = "O campo 'Ano de Início' deve ter 4 dígitos.";
                }
                if (int.Parse(e.NewValues["ANO_INICIO"].ToString()) > DateTime.Now.Year)
                {
                    e.RowError = "O campo 'Ano de Início' não pode ser maior que ano vigente.";
                }
            }

            if (e.NewValues["ANO_CONCLUSAO"] != null)
            {
                if (int.Parse(e.NewValues["ANO_CONCLUSAO"].ToString()) < 1930)
                {
                    e.RowError = " O campo 'Ano de Conclusão' deve ser maior que 1930.";
                }
                if (int.Parse(e.NewValues["ANO_CONCLUSAO"].ToString()) == 0)
                {
                    e.RowError = "O campo 'Ano de Conclusão' não pode ser igual a zero(0).";
                }
                if (e.NewValues["ANO_CONCLUSAO"].ToString().Length != 4)
                {
                    e.RowError = "O campo 'Ano de Conclusão' deve ter 4 dígitos.";
                }
            }
        }

        protected void grdFormacaoPessoal_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFormacaoPessoal);
        }

        public object ListarPessoa(string pessoa)
        {
            if (!String.IsNullOrEmpty(pessoa))
                return RN.FormacaoPessoal.ListarPessoa(pessoa);

            return null;
        }

        protected void odsArea_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var id = e.InputParameters["ID_FORMACAO_PESSOAL"].ToString();
            lblMensagem.Text = "";
            lblMensValidacao.Text = "";

            var TFP = RN.FormacaoPessoal.Carregar(int.Parse(id));

            var cEscolaridade = TFP.Escolaridade.ToString();
            var cSituacaoCurso = TFP.SituacaoCurso.ToString();
            bool valido = true;

            Techne.Data.QueryTable qtGraduacaoConcluida = RN.FormacaoPessoal.ConsultarGraduacaoConcluida(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));
            Techne.Data.QueryTable qtGraduacaoAndamento = RN.FormacaoPessoal.ConsultarGraduacaoAndamento(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));
            Techne.Data.QueryTable qtPosGraduacao = RN.FormacaoPessoal.ConsultarPosGraduacao(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));
            Techne.Data.QueryTable qtGraduacaoAndamentoENaoEnsinoMedio = RN.FormacaoPessoal.ConsultarGraduacaoAndamentoENaoEnsinoMedio(int.Parse(txtPessoa.Text.ToString()), int.Parse(id));

            if (cEscolaridade.Trim().Substring(0, 8) == "Superior" && cSituacaoCurso.Trim() == "Concluído")
            {
                if (qtGraduacaoConcluida.Rows.Count == 1 && qtGraduacaoAndamento.Rows.Count >= 1)
                {
                    lblMensValidacao.Text = @"Não se pode excluir uma graduação completa se tiver uma graduação em andamento cadastrada !";
                    valido = false;
                }

                if (qtGraduacaoConcluida.Rows.Count == 0 && qtPosGraduacao.Rows.Count >= 1)
                {
                    lblMensValidacao.Text = @"Não se pode excluir uma graduação se tiver uma Pós-Graduação cadastrada !";
                    valido = false;
                    // btnTeste_Click(sender,e); 
                }

                if (qtGraduacaoAndamentoENaoEnsinoMedio.Rows.Count >= 1)
                {
                    lblMensValidacao.Text = @"Não se pode exluir uma graduação completa se tiver uma graduação em andamento e não existir ensino médio cadastrado !";
                    valido = false;
                }
            }
            if (valido)
            {
                RN.FormacaoPessoal.Remover(int.Parse(id));
                ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('Área excluída com sucesso!');</script>", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.opener.ExecutarPostBack();", true);
            }
        }

        protected void odsArea_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var TFP = new TceFormacaoPessoal
            {
                Pessoa = int.Parse(e.InputParameters["PESSOA"].ToString()),
                IdFormacaoPessoal = int.Parse(e.InputParameters["ID_FORMACAO_PESSOAL"].ToString()),
                Escolaridade = e.InputParameters["ESCOLARIDADE"].ToString(),
                SituacaoCurso = e.InputParameters["SITUACAO_CURSO"].ToString(),
                IdCursoFormacaoPessoal = int.Parse(e.InputParameters["AREA_CURSO"].ToString().Split('-')[1]),
                FormacaoComplementacaoPedagogica = e.InputParameters["FORMACAO_COMPLEMENTACAO_PEDAGOGICA"].ToString(),
                AnoInicio = int.Parse(e.InputParameters["ANO_INICIO"].ToString()),
                AnoConclusao = e.InputParameters["ANO_CONCLUSAO"] != null ? int.Parse(e.InputParameters["ANO_CONCLUSAO"].ToString()) : 0,
                IdInstituicao = e.InputParameters["ID_INSTITUICAO"].ToString(),
                Doc_comprobatorio = e.InputParameters["DOC_COMPROBATORIO"].ToString(),
                Matricula = User.Identity.Name
            };

            validacao = RN.FormacaoPessoal.Validar(TFP);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.FormacaoPessoal.Alterar(TFP) > 0)
                {

                    //throw new Exception("Área excluída com sucesso.");
                }
            }
        }

        protected void grdDisciplinaAdicional_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdDisciplinaAdicional.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "FORMACAOPESSOALID")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "ESTUDOADICIONALID")
                    e.Editor.Enabled = false;
            }

            else if (grdDisciplinaAdicional.IsEditing)
            {
                if ((e.Column.FieldName) == "FORMACAOPESSOALID")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "ESTUDOADICIONALID")
                    e.Editor.Enabled = false;
            }
        }

        private decimal CalcularOrdem(string pessoa)
        {
            decimal ordem = 0;

            //ordem = tdsCapacitacao.SqlColumns[1];
            QueryTable dadosCapacitacao = null;

            dadosCapacitacao = RN.Capacitacao.ConsultarOrdem(pessoa);

            string dados = dadosCapacitacao.Rows[0].ToString();
            char[] parametros = new char[] { ':' };
            string[] dadosOrdem = dados.Split(parametros, 2, StringSplitOptions.None);
            if (dadosOrdem[1].ToString() != " ")
                ordem = Convert.ToDecimal(dadosOrdem[1]);
            else
                ordem = 0;

            ordem = ordem + 1;

            return ordem;
        }

        protected void grdDisciplinaAdicional_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["FORMACAOPESSOALID"] = txtPessoa.Text.ToString();
            grdDisciplinaAdicional.Settings.ShowFilterRow = false;
        }

        protected void grdDisciplinaAdicional_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string pessoa = Convert.ToString(e.GetListSourceFieldValue("pessoa"));
                string ordem = Convert.ToString(e.GetListSourceFieldValue("ordem"));
                e.Value = pessoa + "-" + ordem;
            }
        }

        protected void grdDisciplinaAdicional_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("FORMACAOPESSOALID", e.Values["FORMACAOPESSOALID"]);
            e.Keys.Add("ESTUDOADICIONALID", e.Values["ESTUDOADICIONALID"]);
        }

        protected void grdDisciplinaAdicional_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //ComboBoxCallbackArgumentsReader cmbDisciplina = (grdDisciplinaAdicional.FindRowCellTemplateControl(i, (GridViewDataComboBoxColumn)grdHorarioOperacional.Columns["horaini_aula"], "txtBox") as ASPxTextBox);

            e.NewValues["estudoadicionalid"] = "6";

            //e.NewValues["estudoadicionalid"] = grdDisciplinaAdicional.getf  
            //    string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            //    e.Keys.Clear();
            //    e.Keys.Add("pessoa", chaves[0]);
            //    e.Keys.Add("ordem", chaves[1]);
        }

        protected void grdDisciplinaAdicional_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["FORMACAOPESSOALID"] = txtPessoa.Text.ToString();
        }

        protected void grdDisciplinaAdicional_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDisciplinaAdicional.Settings.ShowFilterRow = false;
        }

        protected void grdDisciplinaAdicional_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            /*  if (e.NewValues["data_conclusao"] != null)
              {
                  DateTime dataconcl = Convert.ToDateTime(e.NewValues["data_conclusao"]);
                  DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                  if (dataconcl > hoje)
                      e.RowError = "Data de conclusão não pode ser maior que a data atual.";

                  DateTime milnov = new DateTime(1900, 1, 1);

                  if (dataconcl < milnov)
                      e.RowError = "Data de conclusão não pode ser menor que 1900.";
              }
             
              if (e.NewValues["carga_horaria"] != null)
              {

                  int ch = int.Parse(e.NewValues["carga_horaria"].ToString());

                  if (ch < 4)
                      e.RowError = "Não é permitido cadastrar cursos/capacitações com carga horária inferior a 4 horas.";

              }*/
        }

        protected void grdDisciplinaAdicional_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDisciplinaAdicional);
        }

        public void PreecherComboTabGeral(DropDownList combo, string tabela)
        {
            combo.Items.Clear();
            combo.DataSource = RN.TabelaGeral.ConsultaItemTabelaValDescr(tabela);
            combo.DataBind();
            combo.Items.Insert(0, "Selecione");
        }

        public void PreecherComboTabGeralFiltro(DropDownList combo, string tabela, string filtro, string exceto, string excluso)
        {
            combo.Items.Clear();
            combo.DataSource = RN.TabelaGeral.ConsultaItemTabelaValDescrFiltro(tabela, filtro, exceto, excluso);
            combo.DataBind();            
            combo.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        #region Capacitação

        protected int minCargaHorariaCursoCapacitacao
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MinCargaHorariaCursoCapacitacao"]);
            }
        }

        private void AtualizaGridDocenteCursoCapacitacao()
        {
            odsDocenteCursoCapacitacao.Select();
            odsDocenteCursoCapacitacao.DataBind();
            grdCapacitacao.DataBind();
        }

        #region Eventos odsDocenteCursoCapacitacao

        public static void DeleteCursoCapacitacao(int PESSOAID, int CURSOCAPACITACAOID, string OFERECIDOSEEDUC, int TIPOCURSOCAPACITACAOID)
        {

        }

        protected void odsDocenteCursoCapacitacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            string idCursoCapacitacao = e.InputParameters["CURSOCAPACITACAOID"].ToString();
            string idPessoa = e.InputParameters["PESSOAID"].ToString();

            //Data: 11/04/2013 - Alterado por: Lucas Collina - Solicitado por: Wagner Medeiros
            //A validação por esse tipo de curso (EspecificoEduEspecial) já existia. 
            //Com a criação do campo 'TIPOCURSOCAPACITACAOID' na tabela 'LY_CAPACITACAO', 
            //foi alterado para tratar pelo ID deste tipo de curso == '6'
            if (e.InputParameters["TIPOCURSOCAPACITACAOID"].ToString() == "6")
            {
                if (RN.GrupoHabilitacao.PossuiGrupoCapacitacaoEdEspecial(idPessoa))
                {
                    throw new ApplicationException("Esta capacitação não pode ser excluída devido ter grupo de habilitação relacionado.");
                }
            }

            var validacao = new ValidacaoDados();
            bool cursoOferecidoSEEDUC = (e.InputParameters["OFERECIDOSEEDUC"].ToString() == "1");

            int retornoExclusaoCapacitacao = 0;

            if (cursoOferecidoSEEDUC)
            {
                retornoExclusaoCapacitacao = RN.DocenteCursoCapacitacao.RemoverCursoCapacitacao(int.Parse(idCursoCapacitacao), int.Parse(idPessoa));
            }
            else
            {
                retornoExclusaoCapacitacao = RN.Capacitacao.RemoverCursoCapacitacao(int.Parse(idCursoCapacitacao), int.Parse(idPessoa));
            }

            if (retornoExclusaoCapacitacao > 0)
            {
                AtualizaGridDocenteCursoCapacitacao();
            }
        }

        public static void AlteraCursoCapacitacao(string OFERECIDOSEEDUC, string NOMECURSO, int AREACONHECIMENTOID, int TIPOCURSOCAPACITACAOID, string NOMEINSTITUICAO, decimal CARGAHORARIA, DateTime DATACONCLUSAO, int PESSOAID, int CURSOCAPACITACAOID)
        {

        }

        protected void odsDocenteCursoCapacitacao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            int retornoAtualizacaoCapacitacao = 0;

            bool cursoOferecidoSEEDUC = (e.InputParameters["OFERECIDOSEEDUC"].ToString() == "1");

            if (!cursoOferecidoSEEDUC)
            {
                var entidadeLyCapacitacao = new RN.Entidades.LyCapacitacao
                {
                    AreaConhecimentoId = Convert.ToInt32(e.InputParameters["AREACONHECIMENTOID"]),
                    Capacitacao = e.InputParameters["NOMECURSO"].ToString(),
                    CargaHoraria = Convert.ToInt32(e.InputParameters["CARGAHORARIA"]),
                    DataConclusao = Convert.ToDateTime(e.InputParameters["DATACONCLUSAO"]),
                    NomeInstituicao = e.InputParameters["NOMEINSTITUICAO"].ToString(),
                    Ordem = Convert.ToInt32(e.InputParameters["CURSOCAPACITACAOID"]),
                    Pessoa = Convert.ToInt32(e.InputParameters["PESSOAID"]),
                    TipoCursoCapacitacaoId = Convert.ToInt32(e.InputParameters["TIPOCURSOCAPACITACAOID"])
                };

                validacao = RN.Capacitacao.Validar(entidadeLyCapacitacao);

                if (validacao.Valido)
                {
                    retornoAtualizacaoCapacitacao = RN.Capacitacao.AlterarCurso(entidadeLyCapacitacao);
                }
                else
                {
                    lblMensagemCapacitacao.Text = validacao.Mensagem.ToString();
                }
            }

            if (retornoAtualizacaoCapacitacao > 0)
            {
                AtualizaGridDocenteCursoCapacitacao();
                grdCapacitacao.Settings.ShowFilterRow = true;
            }

        }

        #endregion

        public object ListarDocenteCursoCapacitacao(string idPessoa)
        {
            if (!String.IsNullOrEmpty(idPessoa))
                return RN.DocenteCursoCapacitacao.Listar(Convert.ToInt32(idPessoa));

            return null;
        }

        public object ListarAreaConhecimento()
        {
            return RN.AreaConhecimento.Listar();
        }

        public object ListarTipoCurso()
        {
            return RN.TipoCursoCapacitacao.Listar();
        }

        public void PreencherComboTipoCursoCapacitacao(bool cursoOferecidoSeeduc)
        {
            ddlTipoCursoCapacitacao.Items.Clear();
            ddlTipoCursoCapacitacao.DataSource = RN.TipoCursoCapacitacao.Listar(cursoOferecidoSeeduc);
            ddlTipoCursoCapacitacao.DataBind();
            ddlTipoCursoCapacitacao.Items.Insert(0, "Selecione");
        }

        public void PreencherComboAreaConhecimentoCapacitacao(bool cursoOferecidoSEEDUC)
        {
            ddlAreaConhecimentoCapacitacao.Items.Clear();

            if (cursoOferecidoSEEDUC)
            {
                ddlAreaConhecimentoCapacitacao.DataSource = RN.AreaConhecimento.ListarAreaConhecimentoOferecidoSEEDUC(Convert.ToInt32(ddlTipoCursoCapacitacao.SelectedValue));
            }
            else
            {
                ddlAreaConhecimentoCapacitacao.DataSource = RN.AreaConhecimento.Listar();
            }

            ddlAreaConhecimentoCapacitacao.DataBind();
            ddlAreaConhecimentoCapacitacao.Items.Insert(0, "Selecione");
        }

        public void PreencherComboCursoCapacitacao(int idTipoCursoCapacitacao, int idAreaConhecimentoCapacitacao)
        {
            ddlCursoCapacitacao.Items.Clear();
            ddlCursoCapacitacao.DataSource = RN.CursoCapacitacao.ListarCursoCapacitacaoOferecidoSEEDUC(idTipoCursoCapacitacao, idAreaConhecimentoCapacitacao);
            ddlCursoCapacitacao.DataBind();
            ddlCursoCapacitacao.Items.Insert(0, "Selecione");
        }

        protected void rbtListOferecidoSEEDUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool cursoOferecidoSEEDUC = (rbtListOferecidoSEEDUC.SelectedValue == "1");

            LimpaTodosCamposCapacitacao();

            if (cursoOferecidoSEEDUC)
            {
                ddlTipoCursoCapacitacao.AutoPostBack = true;
                ddlAreaConhecimentoCapacitacao.AutoPostBack = true;
                ddlCursoCapacitacao.Visible = true;
                txtCursoCapacitacao.Visible = false;
                txtNomeInstituicaoCapacitacao.Enabled = false;
                txtCargaHorariaCapacitacao.Enabled = false;
                dteDataConclusaoCapacitacao.Enabled = false;

            }
            else
            {
                ddlTipoCursoCapacitacao.AutoPostBack = false;
                ddlAreaConhecimentoCapacitacao.AutoPostBack = false;
                ddlCursoCapacitacao.Visible = false;
                txtCursoCapacitacao.Visible = true;
                txtNomeInstituicaoCapacitacao.Enabled = true;
                txtCargaHorariaCapacitacao.Enabled = true;
                dteDataConclusaoCapacitacao.Enabled = true;

                PreencherComboAreaConhecimentoCapacitacao(cursoOferecidoSEEDUC);
            }

            PreencherComboTipoCursoCapacitacao(cursoOferecidoSEEDUC);
        }

        private void LimpaTodosCamposCapacitacao()
        {
            ddlTipoCursoCapacitacao.SelectedIndex = -1;
            ddlTipoCursoCapacitacao.Items.Clear();

            LimpaCamposCursoCapacitacao();
            LimpaComboCursoCapacitacao();
            LimpaComboAreaConhecimentoCapacitacao();
        }

        private void LimpaCamposCursoCapacitacao()
        {
            txtCursoCapacitacao.Text = string.Empty;
            txtNomeInstituicaoCapacitacao.Text = string.Empty;
            txtCargaHorariaCapacitacao.Text = string.Empty;
            dteDataConclusaoCapacitacao.Date = DateTime.MinValue;
        }

        private void LimpaComboCursoCapacitacao()
        {
            ddlCursoCapacitacao.SelectedIndex = -1;
            ddlCursoCapacitacao.Items.Clear();
        }

        private void LimpaComboAreaConhecimentoCapacitacao()
        {
            ddlAreaConhecimentoCapacitacao.SelectedIndex = -1;
            ddlAreaConhecimentoCapacitacao.Items.Clear();
        }

        protected void ddlTipoCursoCapacitacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool cursoOferecidoSEEDUC = (rbtListOferecidoSEEDUC.SelectedValue == "1");

            if (cursoOferecidoSEEDUC)
            {
                LimpaComboAreaConhecimentoCapacitacao();
                LimpaComboCursoCapacitacao();
                LimpaCamposCursoCapacitacao();

                if (ddlTipoCursoCapacitacao.SelectedIndex > 0)
                {
                    PreencherComboAreaConhecimentoCapacitacao(cursoOferecidoSEEDUC);
                }
            }
        }

        protected void ddlAreaConhecimentoCapacitacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtListOferecidoSEEDUC.SelectedValue == "1")
            {
                LimpaComboCursoCapacitacao();
                LimpaCamposCursoCapacitacao();

                if (ddlAreaConhecimentoCapacitacao.SelectedIndex > 0)
                {
                    PreencherComboCursoCapacitacao(Convert.ToInt32(ddlTipoCursoCapacitacao.SelectedValue), Convert.ToInt32(ddlAreaConhecimentoCapacitacao.SelectedValue));
                }
            }
        }

        protected void ddlCursoCapacitacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCursoCapacitacao.SelectedIndex > 0)
            {
                RN.Entidades.CursoCapacitacao entidadeCursoCapacitacao = new Techne.Lyceum.RN.Entidades.CursoCapacitacao();
                entidadeCursoCapacitacao = RN.CursoCapacitacao.Carregar(Convert.ToInt32(ddlCursoCapacitacao.SelectedValue));

                txtNomeInstituicaoCapacitacao.Text = entidadeCursoCapacitacao.NomeInstituicao;
                txtCargaHorariaCapacitacao.Text = entidadeCursoCapacitacao.CargaHoraria.ToString();
                dteDataConclusaoCapacitacao.Date = entidadeCursoCapacitacao.DataConclusao;
            }
            else
            {
                txtNomeInstituicaoCapacitacao.Text = "";
                txtCargaHorariaCapacitacao.Text = "";
                dteDataConclusaoCapacitacao.Date = DateTime.MinValue;
            }
        }

        protected void btnSalvarCapacitacao_Click(object sender, EventArgs e)
        {
            if (ValidarCursoCapacitacao())
            {
                var validacao = new ValidacaoDados();
                int retornoInserirCurso = 0;

                int idPessoa = Convert.ToInt32(txtPessoa.Text);
                bool cursoOferecidoSEEDUC = (rbtListOferecidoSEEDUC.SelectedValue == "1");

                if (cursoOferecidoSEEDUC)
                {
                    RN.Entidades.DocenteCursoCapacitacao entidadeDocenteCursoCapacitacao = new RN.Entidades.DocenteCursoCapacitacao();

                    entidadeDocenteCursoCapacitacao.CursoCapacitacaoId = Convert.ToInt32(ddlCursoCapacitacao.SelectedValue);
                    entidadeDocenteCursoCapacitacao.DataAtualizacao = DateTime.Now;
                    entidadeDocenteCursoCapacitacao.Matricula = User.Identity.Name.ToString();
                    entidadeDocenteCursoCapacitacao.PessoaId = idPessoa;

                    validacao = RN.DocenteCursoCapacitacao.Validar(entidadeDocenteCursoCapacitacao);

                    if (validacao.Valido)
                    {
                        retornoInserirCurso = RN.DocenteCursoCapacitacao.InserirCurso(entidadeDocenteCursoCapacitacao);
                    }
                }
                else
                {
                    RN.Entidades.LyCapacitacao entidadeLyCapacitacao = new RN.Entidades.LyCapacitacao();

                    entidadeLyCapacitacao.AreaConhecimentoId = Convert.ToInt32(ddlAreaConhecimentoCapacitacao.SelectedValue);
                    entidadeLyCapacitacao.Capacitacao = txtCursoCapacitacao.Text;
                    entidadeLyCapacitacao.CargaHoraria = Convert.ToInt32(txtCargaHorariaCapacitacao.Text);
                    entidadeLyCapacitacao.DataConclusao = dteDataConclusaoCapacitacao.Date;
                    entidadeLyCapacitacao.NomeInstituicao = txtNomeInstituicaoCapacitacao.Text;
                    entidadeLyCapacitacao.Pessoa = idPessoa;
                    entidadeLyCapacitacao.TipoCursoCapacitacaoId = Convert.ToInt32(ddlTipoCursoCapacitacao.SelectedValue);

                    validacao = RN.Capacitacao.Validar(entidadeLyCapacitacao);

                    if (validacao.Valido)
                    {
                        retornoInserirCurso = RN.Capacitacao.InserirCurso(entidadeLyCapacitacao);
                    }
                }

                if (retornoInserirCurso > 0)
                {
                    lblMensagemCapacitacao.Text = "Curso de Capacitação incluído com sucesso.";
                    AtualizaGridDocenteCursoCapacitacao();
                    LimpaTodosCamposCapacitacao();
                    rbtListOferecidoSEEDUC_SelectedIndexChanged(sender, EventArgs.Empty);
                }
                else
                {
                    lblMensagemCapacitacao.Text = validacao.Mensagem;
                }
            }
        }

        private bool ValidarCursoCapacitacao()
        {
            lblMensagemCapacitacao.Text = "";

            if (string.IsNullOrEmpty(rbtListOferecidoSEEDUC.SelectedValue))
            {
                lblMensagemCapacitacao.Text = "Favor informar se o Curso é oferecido pela SEEDUC.";
                rbtListOferecidoSEEDUC.Focus();
                return false;
            }

            if (ddlTipoCursoCapacitacao.SelectedIndex <= 0)
            {
                lblMensagemCapacitacao.Text = "Favor informar o Tipo de Curso.";
                ddlTipoCursoCapacitacao.Focus();
                return false;
            }

            if (ddlAreaConhecimentoCapacitacao.SelectedIndex <= 0)
            {
                lblMensagemCapacitacao.Text = "Favor informar a Área de Conhecimento.";
                ddlAreaConhecimentoCapacitacao.Focus();
                return false;
            }

            if (rbtListOferecidoSEEDUC.SelectedValue == "1")
            {
                if (ddlCursoCapacitacao.SelectedIndex <= 0)
                {
                    lblMensagemCapacitacao.Text = "Favor informar o Curso de Capacitação.";
                    ddlCursoCapacitacao.Focus();
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtCursoCapacitacao.Text.Trim()))
                {
                    lblMensagemCapacitacao.Text = "Favor digitar o Curso de Capacitação.";
                    txtCursoCapacitacao.Focus();
                    return false;
                }
                else
                {
                    if (txtCursoCapacitacao.Text.Trim().Length > 100)
                    {
                        lblMensagemCapacitacao.Text = "Não é permitido cadastrar Curso de Capacitação com mais de 100 caracteres.";
                        txtCursoCapacitacao.Focus();
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(txtNomeInstituicaoCapacitacao.Text.Trim()))
                {
                    lblMensagemCapacitacao.Text = "Favor digitar o Nome da Instituição.";
                    txtNomeInstituicaoCapacitacao.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(txtCargaHorariaCapacitacao.Text.Trim()))
                {
                    lblMensagemCapacitacao.Text = "Favor digitar a Carga Horária.";
                    txtCargaHorariaCapacitacao.Focus();
                    return false;
                }

                if (int.Parse(txtCargaHorariaCapacitacao.Text.Trim()) < minCargaHorariaCursoCapacitacao)
                {
                    lblMensagemCapacitacao.Text = "A Carga Horária não pode ser menor do que " + minCargaHorariaCursoCapacitacao + " hora(s).";
                    txtCargaHorariaCapacitacao.Focus();
                    return false;
                }

                if (dteDataConclusaoCapacitacao.Date == DateTime.MinValue)
                {
                    lblMensagemCapacitacao.Text = "Favor informar a Data de Conclusão.";
                    dteDataConclusaoCapacitacao.Focus();
                    return false;
                }
            }

            return true;
        }

        #region Eventos grdCapacitacao

        protected void grdCapacitacao_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string pessoa = Convert.ToString(e.GetListSourceFieldValue("PESSOAID"));
                string capacitacao = Convert.ToString(e.GetListSourceFieldValue("CURSOCAPACITACAOID"));
                string oferecidoSEEDUC = Convert.ToString(e.GetListSourceFieldValue("OFERECIDOSEEDUC"));
                e.Value = pessoa + "-" + capacitacao + "-" + oferecidoSEEDUC;
            }
        }

        protected void grdCapacitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCapacitacao);
        }

        protected void grdCapacitacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string[] chaves = e.EditingKeyValue.ToString().Split('-');

            //Array 2 = "Oferecido SEEDUC"
            //Se for igual a 1 ("Sim") não deixa editar
            if (chaves[2] == "1")
            {
                e.Cancel = true;
                throw new Exception("Não é possível alterar cursos oferecidos pela SEEDUC, apenas exclusão.");
            }
            else
            {
                grdCapacitacao.Settings.ShowFilterRow = false;
            }
        }

        protected void grdCapacitacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "OFERECIDOSEEDUC")
                e.Editor.ClientEnabled = false;
        }

        protected void grdCapacitacao_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCapacitacao.Settings.ShowFilterRow = true;
        }

        protected void grdCapacitacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("PESSOAID", Convert.ToInt32(chaves[0]));
            e.Keys.Add("CURSOCAPACITACAOID", Convert.ToInt32(chaves[1]));
            e.Keys.Add("OFERECIDOSEEDUC", chaves[2].ToString());
        }

        protected void grdCapacitacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("PESSOAID", Convert.ToInt32(chaves[0]));
            e.Keys.Add("CURSOCAPACITACAOID", Convert.ToInt32(chaves[1]));
            e.Keys.Add("OFERECIDOSEEDUC", chaves[2].ToString());

            e.Keys.Add("TIPOCURSOCAPACITACAOID", Convert.ToInt32(e.Values["TIPOCURSOCAPACITACAOID"]));
        }

        #endregion

        #endregion

        protected void rblAcumulacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMatriculaAcumulacao.Text = string.Empty;
            txtOrgaoAcumulacao.Text = string.Empty;
            txtNumProcessoAcumulacao.Text = string.Empty;

            if (rblAcumulacao.SelectedValue == "1")
            {
                txtMatriculaAcumulacao.ReadOnly = false;
                txtOrgaoAcumulacao.ReadOnly = false;
                txtNumProcessoAcumulacao.ReadOnly = false;
            }
            else
            {
                txtMatriculaAcumulacao.ReadOnly = true;
                txtOrgaoAcumulacao.ReadOnly = true;
                txtNumProcessoAcumulacao.ReadOnly = true;
            }
        }

        protected void tseCategoria_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseCategoria.DBValue.IsNull)
                {
                    if (this.tseCategoria.IsValidDBValue)
                    {
                        tseFuncaoLotacao.SqlWhere = " CAMPO_01='S' AND CATEGORIA='" + tseCategoria.DBValue + "'";
                        tseFuncaoLotacao.Mode = ControlMode.Edit;

                        if (cmbRegContratacao.SelectedValue == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario).ToString())
                        {
                            ddlCargaHoraria.ClearSelection();
                            ddlCargaHoraria.Items.Clear();

                            if (!string.IsNullOrEmpty(txtProcesso.Text.Trim()))
                            {
                                //Busca lista de carga horario de acordo com cargo
                                CarregaCargaHoraria(txtProcesso.Text.Trim(), tseCategoria.DBValue.ToString());
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Cargo não cadastrado.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar um cargo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void CarregaCargaHoraria(string concurso, string cargo)
        {
            ddlCargaHoraria.Items.Clear();

            try
            {
                if (!concurso.IsNullOrEmptyOrWhiteSpace() && !cargo.IsNullOrEmptyOrWhiteSpace())
                {
                    DataTable dtCarga = RN.ProcessoSeletivo.ConsultarCargaHorariaPor(concurso, cargo);
                    if (dtCarga.Rows.Count > 0)
                    {
                        int intMenorValor = Convert.ToInt32(dtCarga.Rows[0].ItemArray[1]);
                        int intCargaEfetiva = Convert.ToInt32(dtCarga.Rows[0].ItemArray[0]);

                        for (int i = intMenorValor; i <= intCargaEfetiva; i++)
                        {
                            ddlCargaHoraria.Items.Add(i.ToString());
                        }
                        ddlCargaHoraria.SelectedIndex = -1;
                        ddlCargaHoraria.DataBind();
                        ddlCargaHoraria.Items.Insert(0, new ListItem("<Selecione>", "-1"));
                        ddlCargaHoraria.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseFuncaoLotacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                string cpf = string.Empty;
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseFuncaoLotacao.DBValue.IsNull)
                {
                    if (this.tseFuncaoLotacao.IsValidDBValue)
                    {
                        if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                        {
                            if (!txtCPF.Text.IsNullOrEmptyOrWhiteSpace())
                            {
                                cpf = txtCPF.Text.RetirarMascaraCPF().Trim();
                                if (Utils.ValidarCpf(cpf))
                                {
                                    txtCH.Text = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(tseCategoria.DBValue.ToString(), tseFuncaoLotacao.DBValue.ToString()).ToString();

                                    if (txtCH.Text == "0")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('ATENÇÃO! Não existe Carga Horária associada ao cargo, função e número de matriculas do docente.');", true);
                                    }
                                }
                                else
                                {
                                    tseFuncaoLotacao.ResetValue();
                                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('CPF inválido.');", true);
                                }
                            }
                            else
                            {
                                tseFuncaoLotacao.ResetValue();
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('CPF é de preenchimento obrigatório.');", true);
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Função não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma função.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegionalLotacao_Changed(object sender, ChangedEventArgs args)
        {
            string municipio = string.Empty;
            try
            {
                tseRegionalLotacao.Visible = true;
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseRegionalLotacao.DBValue.IsNull)
                {
                    if (!this.tseRegionalLotacao.IsValidDBValue)
                    {
                        lblMensagem.Text = "Regional não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma regional.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipioLotacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseMunicipioLotacao.Visible = true;
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseMunicipioLotacao.DBValue.IsNull)
                {
                    if (!this.tseMunicipioLotacao.IsValidDBValue)
                    {
                        lblMensagem.Text = "Município não cadastrado.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar um município.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void tseUnidadeLotacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseUnidadeLotacao.Visible = true;
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseUnidadeLotacao.DBValue.IsNull)
                {
                    if (!this.tseUnidadeLotacao.IsValidDBValue)
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                    else
                    {
                        tseMunicipioLotacao.Value = tseUnidadeLotacao["Municipio"];
                        tseRegionalLotacao.Value = tseUnidadeLotacao["id_regional"];
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkMaeNaoDeclarada_CheckedChanged(object sender, EventArgs e)
        {
            txtNomeMae.ReadOnly = false;
            txtNomeMae.Text = string.Empty;
            if (chkMaeNaoDeclarada.Checked)
            {
                txtNomeMae.Text = chkMaeNaoDeclarada.Text.ToUpper();
                txtNomeMae.ReadOnly = true;
            }
        }

        protected void chkPaiNaoDeclarado_CheckedChanged(object sender, EventArgs e)
        {
            txtNomePai.ReadOnly = false;
            txtNomePai.Text = string.Empty;
            if (chkPaiNaoDeclarado.Checked)
            {
                txtNomePai.Text = chkPaiNaoDeclarado.Text.ToUpper();
                txtNomePai.ReadOnly = true;
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtNumFunc.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    //Abrir Ficha
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(txtNumFunc.Text.Trim());

                    Response.Write("<script type=text/javascript>");
                    Response.Write("pagina = " + @"'FichaImplantacao.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "'" + ";");
                    Response.Write("abriu = false;");
                    Response.Write("function abrir() {newWindow = window.open(pagina, 'nova', 'status=no, scrollbars=yes, resizable=yes, width=850, height=800'); if (newWindow) {abriu = true;   return false;}}");
                    Response.Write("abrir();");
                    Response.Write("</script>");
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
    }
}
