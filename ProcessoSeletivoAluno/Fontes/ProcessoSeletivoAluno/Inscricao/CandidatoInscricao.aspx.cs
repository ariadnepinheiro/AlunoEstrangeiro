using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN;
using Proderj.Framework.Common;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Servicos;
using System.Linq;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.Net.ProcessoSeletivoAluno
{
    public partial class CandidatoInscricao : TPage
    {
        Techne.Lyceum.Net.ProcessoSeletivoAluno.Sessao.CandidatoProcessoSeletivoSessao sessaoCandidato = new Techne.Lyceum.Net.ProcessoSeletivoAluno.Sessao.CandidatoProcessoSeletivoSessao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!sessaoCandidato.CandidatoAcordoEdital)
                {
                    Response.Redirect("Edital.aspx");
                }

                if (!sessaoCandidato.CandidatoLogado)
                {
                    Response.Redirect("Identificacao.aspx");
                }

                if (!IsPostBack)
                {
                    lblTipoRedeEnsino.Text = string.Format("Concluiu ou irá concluir até o término do ano letivo de {0} o Ensino Fundamental na Rede de Ensino*:", DateTime.Now.Year.ToString());
                    
                    txtNomeCompletoAluno.Text = sessaoCandidato.NomeCandidato;
                    txtNomeMae.Text = sessaoCandidato.NomeMae;
                    dtDataNasc.Date = sessaoCandidato.DataNascimento;

                    this.CarregarDadosDrop(ddlUFCartorio.ID);
                    this.CarregarDadosDrop(ddlNecessidadeEspecial.ID);
                    this.CarregarDadosDrop(ddlUnidadeEnsino.ID);

                    VerificaNecessidadeEspecial();

                    trBolsaParticular.Visible = false;
                    trCertidaoAntigaCartorio.Visible = false;
                    trCertidaoAntigaDados.Visible = false;
                    trCertidaoAntigaDadosExpedicao.Visible = false;
                    trCertidaoAntigaFiltro.Visible = false;
                    trMatriculaCertidao.Visible = false;
                    trRedeEstadualSeeduc.Visible = false;
                    txtMatriculaSeeduc.Text = string.Empty;

                    CarregaRecursoAplicacaoProva();
                    CarregaDadosCandidatoInscrito();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaRecursoAplicacaoProva()
        {
            try
            {
                DataTable dtRecursoAplicacaoProva = new DataTable();
                dtRecursoAplicacaoProva = Techne.Lyceum.RN.NecessidadeEspecial.RecursoAplicacaoProva.Listar();

                chkRecursoAplicacaoProva.Items.Clear();
                chkRecursoAplicacaoProva.DataSource = dtRecursoAplicacaoProva.Select("EXCLUSIVO = 0").CopyToDataTable();
                chkRecursoAplicacaoProva.DataTextField = "NOME";
                chkRecursoAplicacaoProva.DataValueField = "RECURSOAPLICACAOPROVAID";
                chkRecursoAplicacaoProva.DataBind();
                //chkRecursoAplicacaoProva.Items.Add(new ListItem("Nenhum", "0"));

                rblRecursoAplicaProvaExclusivo.Items.Clear();
                rblRecursoAplicaProvaExclusivo.DataSource = dtRecursoAplicacaoProva.Select("EXCLUSIVO = 1").CopyToDataTable();
                rblRecursoAplicaProvaExclusivo.DataTextField = "NOME";
                rblRecursoAplicaProvaExclusivo.DataValueField = "RECURSOAPLICACAOPROVAID";
                rblRecursoAplicaProvaExclusivo.DataBind();

                chkRecursoAplicacaoProva.Attributes.Add("onclick", "retiraSelecaoCheckNenhumRecursoAplicacaoProva('" + chkNenhumRecursoAplicacaoProva.ClientID + "', '" + chkRecursoAplicacaoProva.ClientID + "', '" + rblRecursoAplicaProvaExclusivo.ClientID + "')");
                rblRecursoAplicaProvaExclusivo.Attributes.Add("onclick", "retiraSelecaoCheckNenhumRecursoAplicacaoProva('" + chkNenhumRecursoAplicacaoProva.ClientID + "', '" + chkRecursoAplicacaoProva.ClientID + "', '" + rblRecursoAplicaProvaExclusivo.ClientID + "')");
                chkNenhumRecursoAplicacaoProva.Attributes.Add("onclick", "retiraSelecaoCheckRecursoAplicacaoProva('" + chkNenhumRecursoAplicacaoProva.ClientID + "', '" + chkRecursoAplicacaoProva.ClientID + "', '" + rblRecursoAplicaProvaExclusivo.ClientID + "')");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlUFCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMunicipioCartorio.Items.Clear();
            ddlCartorio.Items.Clear();

            if (!string.IsNullOrEmpty(ddlUFCartorio.SelectedValue))
            {
                CarregarDadosDrop(ddlMunicipioCartorio.ID);
                ddlMunicipioCartorio.Visible = true;
                ddlMunicipioCartorio.Focus();
            }
            else
            {
                ddlMunicipioCartorio.Visible = false;
                ddlCartorio.Visible = false;
            }
        }

        protected void ddlMunicipioCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCartorio.Items.Clear();

            if (!string.IsNullOrEmpty(ddlMunicipioCartorio.SelectedValue))
            {
                CarregarDadosDrop(ddlCartorio.ID);
                ddlCartorio.Visible = true;
                ddlCartorio.Focus();
            }
            else
            {
                ddlCartorio.Visible = false;
            }
        }

        protected void ddlCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCartorio.SelectedValue))
            {
                txtNumeroTermo.Focus();
            }
        }

        private object CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;
            try
            {
                switch (idDrop)
                {
                    case "ddlUnidadeEnsino":
                        {
                            dadosDrop = Techne.Lyceum.RN.Agenda.UnidadeEnsinoProcessoSeletivo.ListaUnidadeEnsinoCursoTurnoProcessoSeletivo(sessaoCandidato.AgendaID);

                            DataTable unidadeensino = dadosDrop.DefaultView.ToTable(true, "UNIDADEENSINOID", "NOMEUNIDADEENSINO");
                            CarregarDropDownList(ddlUnidadeEnsino, unidadeensino, "");
                            ddlUnidadeEnsino.SelectedValue = string.Empty;
                            break;
                        }
                    case "ddlNecessidadeEspecial":
                        {
                            ddlNecessidadeEspecial.Items.Clear();
                            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
                            int tipoProcessoId = (int)RN.NecessidadeEspecial.NecessidadeEspecial.FiltroProcesso.ProcessoSeletivoAluno;
                            ddlNecessidadeEspecial.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtivaHabilitadaPor(tipoProcessoId);
                            ddlNecessidadeEspecial.DataBind();

                            ListItem item = new ListItem("Não possui.", "Não possui.");
                            ddlNecessidadeEspecial.Items.Insert(0, item);

                            break;
                        }
                    case "ddlUFCartorio":
                        {

                            dadosDrop = Techne.Lyceum.RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddlUFCartorio, dadosDrop, "");
                            break;

                        }
                    case "ddlMunicipioCartorio":
                        {
                            if (!string.IsNullOrEmpty(ddlUFCartorio.SelectedValue))
                            {
                                dadosDrop = Techne.Lyceum.RN.Basico.ConsultarMunicipioCartorio(RN.Basico.ObterCodigoUFCartorio(ddlUFCartorio.SelectedValue));
                                CarregarDropDownList(ddlMunicipioCartorio, dadosDrop, "");
                                break;
                            }
                            break;
                        }
                    case "ddlCartorio":
                        {
                            if (!string.IsNullOrEmpty(ddlMunicipioCartorio.SelectedValue))
                            {
                                dadosDrop = Techne.Lyceum.RN.Basico.ConsultarCartorio(RN.Basico.ObterCodigoUFCartorio(ddlUFCartorio.SelectedValue), ddlMunicipioCartorio.SelectedValue.ToString());
                                CarregarDropDownList(ddlCartorio, dadosDrop, "");
                                break;
                            }
                            break;
                        }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dadosDrop = null;
            }

            return dadosDrop;
        }

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();

            if (drop == ddlNecessidadeEspecial)
            {
                ListItem item = new ListItem("Não possui.", "Não possui.");
                drop.Items.Insert(0, item);
            }
            else
            {
                ListItem itemVazio = new ListItem("<SELECIONE>", "");
                drop.Items.Add(itemVazio);
                drop.SelectedValue = "";
            }
        }

        private void CarregaDadosRecursoAplicacaoProva(DataTable dadosRecursosAplicacaoProva)
        {
            foreach (DataRow linha in dadosRecursosAplicacaoProva.Rows)
            {
                ListItem liRbl = rblRecursoAplicaProvaExclusivo.Items.FindByValue(linha["RECURSOAPLICACAOPROVAID"].ToString());
                if (liRbl != null)
                {
                    liRbl.Selected = true;
                }

                ListItem liChk = chkRecursoAplicacaoProva.Items.FindByValue(linha["RECURSOAPLICACAOPROVAID"].ToString());
                if (liChk != null)
                {
                    liChk.Selected = true;
                }
            }

            if (rblRecursoAplicaProvaExclusivo.SelectedIndex == -1 && chkRecursoAplicacaoProva.SelectedIndex == -1)
                chkNenhumRecursoAplicacaoProva.Checked = true;
        }

        protected void ddlNecessidadeEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificaNecessidadeEspecial();
        }

        private void VerificaNecessidadeEspecial()
        {
            trRecursosNecessarioProva.Visible = !(ddlNecessidadeEspecial.SelectedValue == "Não possui." || string.IsNullOrEmpty(ddlNecessidadeEspecial.SelectedValue));
        }

        //Valida os dados da inscrição
        public List<string> ValidarDadosInscricao()
        {
            try
            {
                var mensagens = new List<string>();
                RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();

                #region Rede de Ensino

                if (rblTipoRedeEnsino.SelectedValue == "")
                {
                    mensagens.Add("Rede de ensino origem não informada.");
                }
                if (rblTipoRedeEnsino.SelectedValue == "particular")
                {
                    if (ddlTipoBolsaParticular.SelectedValue == "")
                    {
                        mensagens.Add("Tipo bolsa particular não informado.");
                    }
                }

                #endregion

                #region Dados Pessoais do Candidato

                if (txtNomeCompletoAluno.Text == string.Empty)
                {
                    mensagens.Add("Nome do candidato não informado.");
                }
                else
                {
                    RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaDadosNome(txtNomeCompletoAluno.Text, "Nome do Candidato", txtNomeCompletoAluno.Text, ref mensagens);
                }

                if (dtDataNasc.Text == string.Empty)
                {
                    mensagens.Add("Data de nascimento não informada.");
                }
                else
                {
                    if (!RN.Agenda.ProcessoSeletivo.VerificaDentroIntervaloDataNascimentoProcessoSeletivo(Convert.ToDateTime(dtDataNasc.Text), sessaoCandidato.AgendaID))
                    {
                        mensagens.Add("Idade não permitida para este processo seletivo.");
                    }
                }

                if (txtMatriculaSeeduc.Text != string.Empty)
                {
                    DataTable Matriculaseduc = RN.ProcessoSeletivoAluno.Candidato.ListaDadosRedeEstadual(txtMatriculaSeeduc.Text);
                    if (Matriculaseduc.Rows.Count == 0)
                    {
                        txtMatriculaSeeduc.Text = string.Empty;
                        mensagens.Add("Número de matrícula da Rede Estadual - SEEDUC não encontrada.");
                    }
                }

                if (rblSexo.SelectedValue == "")
                {
                    mensagens.Add("Sexo não informado.");
                }

                #endregion

                #region Dados Certidão

                if (ddlModeloCertidaoNascimento.SelectedValue == string.Empty)
                {
                    mensagens.Add("Certidão de Nascimento não informada.");
                }
                else if (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Novo")
                {
                    if (txtMatriculaCertidao.Text == string.Empty)
                    {
                        mensagens.Add("Número da Matrícula da Certidão não informado.");
                    }
                    else
                    {
                        if (txtMatriculaCertidao.Text.Length != 32)
                        {
                            mensagens.Add("Número de matrícula da Certidão deve ter 32 dígitos.");
                        }
                    }
                }
                else if (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Antigo")
                {
                    if (ddlUFCartorio.SelectedValue == string.Empty)
                    {
                        mensagens.Add("UF do cartório não informada.");
                    }

                    if (ddlMunicipioCartorio.SelectedValue == string.Empty)
                    {
                        mensagens.Add("Município do cartório não informado.");
                    }

                    if (ddlCartorio.SelectedValue == string.Empty)
                    {
                        mensagens.Add("Cartório não informado.");
                    }

                    if (txtNumeroTermo.Text == string.Empty)
                    {
                        mensagens.Add("Número do termo da certidão não informado.");
                    }
                    else
                    {
                        if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtNumeroTermo.Text) != txtNumeroTermo.Text)
                        {
                            mensagens.Add("Não é permitido mais de um espaço no campo Número do termo da Certidão.");
                        }
                    }

                    if (dtDataExped.Text == string.Empty)
                    {
                        mensagens.Add("Data de emissão da certidão não informada.");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dtDataNasc.Text))
                        {
                            if (Convert.ToDateTime(dtDataNasc.Value) > Convert.ToDateTime(dtDataExped.Value))
                            {
                                mensagens.Add("A data de Emissão da Certidão não pode ser inferior a data de nascimento.");
                            }
                        }
                        if (!string.IsNullOrEmpty(dtDataExped.Text))
                        {
                            if (Convert.ToDateTime(dtDataExped.Value) > Convert.ToDateTime("01/01/2010"))
                            {
                                mensagens.Add("A data de Emissão da Certidão não pode ser superior a 01/01/2010.");
                            }
                        }
                    }

                    if (txtfolha.Text == string.Empty)
                    {
                        mensagens.Add("Folha da certidão não informada.");
                    }
                    else
                    {
                        if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtfolha.Text) != txtfolha.Text)
                        {
                            mensagens.Add("Não é permitido mais de um espaço no campo Folha da Certidão.");
                        }
                    }

                    if (txtlivro.Text == string.Empty)
                    {
                        mensagens.Add("Livro da certidão não informado.");
                    }
                    else
                    {
                        if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtlivro.Text) != txtlivro.Text)
                        {
                            mensagens.Add("Não é permitido mais de um espaço no campo Livro da Certidão.");
                        }
                    }
                }

                #endregion

                #region Dados da Mãe

                if (txtNomeMae.Text == string.Empty)
                {
                    mensagens.Add("Nome da mãe não informado.");
                }
                else
                {
                    RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaDadosNome(txtNomeMae.Text, "Nome da Mãe", txtNomeCompletoAluno.Text, ref mensagens);
                }

                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaCampoCPF("Mãe", txtCPFMae.Text, ref mensagens);
                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaTelefone(txtTelefoneMae.Text, "Mãe", ref mensagens);

                #endregion

                #region Dados do Pai

                if (txtNomePai.Text == string.Empty)
                {
                    mensagens.Add("Nome da pai não informado.");
                }
                else
                {
                    RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaDadosNome(txtNomePai.Text, "Nome do Pai", txtNomeCompletoAluno.Text, ref mensagens);
                }

                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaCampoCPF("Pai", txtCPFPai.Text, ref mensagens);
                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaTelefone(txtTelefonePai.Text, "Pai", ref mensagens);

                if (!string.IsNullOrEmpty(txtNomeMae.Text) && !string.IsNullOrEmpty(txtNomePai.Text))
                {
                    if (txtNomeMae.Text == txtNomePai.Text)
                    {
                        mensagens.Add("O campo Nome do Pai não pode ser idêntico ao Nome da Mãe.");
                    }
                }

                #endregion

                #region Responsável Legal

                if (string.IsNullOrEmpty(chkResponsavel.SelectedValue))
                {
                    mensagens.Add("Responsável Legal não informado.");
                }

                #endregion

                #region Validações para Responsável Legal Selecionado

                int contTipoResponsavelSelecionado = 0;

                foreach (ListItem item in chkResponsavel.Items)
                {
                    if (item.Selected)
                    {
                        contTipoResponsavelSelecionado++;

                        if (item.Value == "Mãe")
                        {
                            RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaCamposResponsavel(item.Value, txtCPFMae.Text, chkNaoDeclarMae, ref mensagens);
                        }
                        if (item.Value == "Pai")
                        {
                            RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaCamposResponsavel(item.Value, txtCPFPai.Text, chkNaoDeclarPai, ref mensagens);
                        }
                        if (item.Value == "Outros")
                        {
                            if (txtNomeResponsavel.Text == string.Empty)
                            {
                                mensagens.Add("Responsável (Outros) não informado.");
                            }
                            else
                            {
                                if (txtNomeResponsavel.Text == txtNomeMae.Text || txtNomeResponsavel.Text == txtNomePai.Text)
                                {
                                    mensagens.Add("O campo Nome do Responsável (Outros) não pode ser igual ao Nome da(o) Mãe/Pai.");
                                }

                                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaDadosNome(txtNomeResponsavel.Text, "Nome do Responsável (Outros)", txtNomeCompletoAluno.Text, ref mensagens);
                                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaCamposResponsavel(item.Value, txtCPFResponsavel.Text, null, ref mensagens);
                            }

                            RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaCampoCPF("Responsável (Outros)", txtCPFResponsavel.Text, ref mensagens);
                            RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaTelefone(txtTelefoneResp.Text, "Responsável (Outros)", ref mensagens);
                        }
                    }
                }

                if (contTipoResponsavelSelecionado > 2)
                {
                    mensagens.Add("Somente poderá ter no máximo duas opções de Responsável Legal.");
                }

                #endregion

                #region Dados do Endereço

                if (txtCep.Text == string.Empty)
                {
                    mensagens.Add("Cep não informado.");
                }
                else
                {
                    var cep = Utils.RetirarMascara(txtCep.Text);

                    if (!Validacao.ValidarCEP(cep))
                    {
                        mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                    }
                }

                if (txtEndereco.Text == string.Empty)
                {
                    mensagens.Add("Endereço não informado.");
                }
                else
                {
                    if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtEndereco.Text) != txtEndereco.Text)
                    {
                        mensagens.Add("Não é permitido mais de um espaço no campo Endereço.");
                    }
                }

                if (txtEndNum.Text == string.Empty)
                {
                    mensagens.Add("Número do endereço não informado.");
                }
                else
                {
                    if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtEndNum.Text) != txtEndNum.Text)
                    {
                        mensagens.Add("Não é permitido mais de um espaço no campo Número do Endereço.");
                    }
                }

                if (txtEndCompl.Text != string.Empty)
                {
                    if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtEndCompl.Text) != txtEndCompl.Text)
                    {
                        mensagens.Add("Não é permitido mais de um espaço no campo Complemento do Endereço.");
                    }
                }

                if (txtBairro.Text == string.Empty)
                {
                    mensagens.Add("Bairro não informado.");
                }
                else
                {
                    if (!Validacao.Bairro(txtBairro.Text))
                    {
                        mensagens.Add("O campo Bairro é Obrigatório!");
                    }

                    if (RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaEspaco(txtBairro.Text) != txtBairro.Text)
                    {
                        mensagens.Add("Não é permitido mais de um espaço no campo Bairro do Endereço.");
                    }
                }

                var dadosEndereco = this.ControlarEndereco(mensagens);

                if (txtMunicipio.Value == string.Empty)
                {
                    mensagens.Add("Município  não informado.");
                }

                if (txtEstado.Value == string.Empty)
                {
                    mensagens.Add("Estado  não informado.");
                }
                #endregion

                #region Dados de Contato

                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaTelefone(txtFone.Text, "Candidato", ref mensagens);

                if (!string.IsNullOrEmpty(txtCelular.Text))
                {
                    var celular = Utils.RetirarMascara(txtCelular.Text);

                    if (!Validacao.ValidaCelularComDDD(celular))
                    {
                        mensagens.Add("O campo Celular é inválido.!");
                    }
                }

                if (!string.IsNullOrEmpty(txtEmail.Text))
                {
                    if (!Validacao.Email(txtEmail.Text))
                    {
                        mensagens.Add("O campo E-mail está em um formato incorreto!");
                    }
                }

                if (!string.IsNullOrEmpty(txtEmailConfirmacao.Text))
                {
                    if (!Validacao.Email(txtEmailConfirmacao.Text))
                    {
                        mensagens.Add("O campo E-mail de Confirmação está em um formato incorreto!");
                    }
                }

                if (txtEmail.Text != string.Empty && txtEmailConfirmacao.Text == string.Empty)
                {
                    mensagens.Add("E-mail de confirmação não informado.");
                }
                if (txtEmail.Text != string.Empty && txtEmailConfirmacao.Text != string.Empty && txtEmailConfirmacao.Text != txtEmail.Text)
                {
                    mensagens.Add("E-mail de confirmação não confere com o e-mail informado.");
                }

                #endregion

                #region Dados Unidade de Ensino / Curso / Turno

                if (ddlUnidadeEnsino.Text == string.Empty)
                {
                    mensagens.Add("Unidade de ensino pretendida não informada.");
                }
                if (ddlCurso.Text == string.Empty)
                {
                    mensagens.Add("Curso pretendido não informado.");
                }
                if (ddlTurno.Text == string.Empty)
                {
                    mensagens.Add("Turno pretendido não informado.");
                }

                #endregion

                #region Declaração

                if (checkConfirmacao.Checked == false)
                {
                    mensagens.Add("Favor declarar a veracidade de suas informações antes de continuar.");
                }

                #endregion

                #region Valida se os dados do Candidato já foram cadastrados para outro Candidato

                if (!(string.IsNullOrEmpty(txtNomeCompletoAluno.Text)) &&
                    !(string.IsNullOrEmpty(txtNomeMae.Text)) &&
                    !(dtDataNasc.Date == DateTime.MinValue))
                {
                    bool candidatoJaExistente = Techne.Lyceum.RN.ProcessoSeletivoAluno.Candidato.VerificaDadosCandidatoJaExistente(sessaoCandidato.CandidatoId, txtNomeCompletoAluno.Text, txtNomeMae.Text, dtDataNasc.Date);
                    if (candidatoJaExistente)
                    {
                        mensagens.Add("Dados já cadastrados para outro Candidato! Só é permitido 1 cadastro por Candidato.");
                    }
                }

                #endregion

                if (!string.IsNullOrEmpty(ddlNecessidadeEspecial.SelectedValue) && ddlNecessidadeEspecial.SelectedValue != "Não possui.")
                {
                    if (!rnNecessidadeEspecial.PossuiNecessidadeEspecialFiltroProcessoPor((int)RN.NecessidadeEspecial.NecessidadeEspecial.FiltroProcesso.ProcessoSeletivoAluno, ddlNecessidadeEspecial.SelectedValue))
                    {
                        mensagens.Add("A Necessidade Especial escolhida não pertence a este processo seletivo.Verifique.");
                        this.CarregarDadosDrop(ddlNecessidadeEspecial.ID);
                        ListItem itemVazio = new ListItem("<SELECIONE>", "");
                        ddlNecessidadeEspecial.Items.Add(itemVazio);
                        ddlNecessidadeEspecial.SelectedValue = "";
                        VerificaNecessidadeEspecial();
                    }
                }

                return mensagens;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return null;
            }
        }

        private DadosEndereco ControlarEndereco(List<string> mensagens)
        {
            RetValue retorno = null;
            var dadosEndereco = new DadosEndereco
            {
                DescricaoBairro = this.txtBairro.Text,
                Cep = this.txtCep.Text.RetirarCaracteres(),
                DescricaoLogradouro = this.txtEndereco.Text,
                Municipio = this.hdnCodMunicipio.Value,
                UF = this.txtEstado.Value,
                DescricaoMunicipio = txtMunicipio.Value
            };

            if (string.IsNullOrEmpty(hdnCodMunicipio.Value) && string.IsNullOrEmpty(txtMunicipio.Value))
            {
                dadosEndereco.Municipio = hdnCodMunicipio.Value;
                dadosEndereco.DescricaoMunicipio = txtMunicipio.Value;
            }

            retorno = Endereco.ControlarEndereco(dadosEndereco);

            if (retorno != null)
            {
                if (!retorno.Ok)
                    mensagens.Add(retorno.Errors.ToString());
            }

            return dadosEndereco;
        }

        protected void rblTipoRedeEnsino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtMatriculaSeeduc.Text)))
            {
                HabilitaCamposMatriculaInformada(true);
                chkNaoDeclarMae_CheckedChanged(null, EventArgs.Empty);
            }

            trBolsaParticular.Visible = (rblTipoRedeEnsino.SelectedValue == "Particular");
            trRedeEstadualSeeduc.Visible = (rblTipoRedeEnsino.SelectedValue == "Estadual");
            txtMatriculaSeeduc.Text = string.Empty;
            ddlTipoBolsaParticular.SelectedIndex = -1;
        }

        protected void ddlModeloCertidaoNascimento_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExibeCamposCertidaoModeloAntigo(ddlModeloCertidaoNascimento.SelectedValue == "Modelo Antigo");

            trMatriculaCertidao.Visible = (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Novo");

            txtMatriculaCertidao.Text = string.Empty;
            ddlUFCartorio.SelectedValue = string.Empty;
            ddlUFCartorio_SelectedIndexChanged(sender, e);
            txtNumeroTermo.Text = string.Empty;
            txtfolha.Text = string.Empty;
            txtlivro.Text = string.Empty;
            dtDataExped.Text = string.Empty;

            if (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Novo")
                txtMatriculaCertidao.Focus();
            if (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Antigo")
                ddlUFCartorio.Focus();
        }

        protected void chkNaoDeclarMae_CheckedChanged(object sender, EventArgs e)
        {
            txtNomeMae.ReadOnly = false;
            txtCPFMae.Enabled = true;
            txtTelefoneMae.Enabled = true;
            txtNomeMae.Text = string.Empty;
            chkFalecidaMae.Enabled = true;

            if (chkNaoDeclarMae.Checked)
            {
                txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                txtNomeMae.ReadOnly = true;
                txtCPFMae.Enabled = false;
                txtTelefoneMae.Enabled = false;
                txtCPFMae.Text = string.Empty;
                txtTelefoneMae.Text = string.Empty;
                chkFalecidaMae.Checked = false;
                chkFalecidaMae.Enabled = false;
                DesabilitaResponsavelLegal("H", "Mãe");
            }
            else
                DesabilitaResponsavelLegal("D", "Mãe");
        }

        protected void DesabilitaResponsavelLegal(string operacao, string filiacao)
        {
            foreach (ListItem item in chkResponsavel.Items)
            {
                if (item.Text == filiacao)
                {
                    if (operacao == "H")
                    {
                        item.Selected = false;
                        item.Enabled = false;
                        return;
                    }
                    if (operacao == "D")
                    {
                        item.Selected = false;
                        item.Enabled = true;
                        return;
                    }
                }
            }
        }

        protected void chkNaoDeclarPai_CheckedChanged(object sender, EventArgs e)
        {
            txtNomePai.ReadOnly = false;
            txtCPFPai.Enabled = true;
            txtTelefonePai.Enabled = true;
            txtNomePai.Text = string.Empty;
            chkFalecidoPai.Enabled = true;

            if (chkNaoDeclarPai.Checked)
            {
                txtNomePai.Text = chkNaoDeclarPai.Text.ToUpper();
                txtCPFPai.Text = string.Empty;
                txtTelefonePai.Text = string.Empty;
                txtNomePai.ReadOnly = true;
                txtCPFPai.Enabled = false;
                txtTelefonePai.Enabled = false;
                chkFalecidoPai.Checked = false;
                chkFalecidoPai.Enabled = false;
                DesabilitaResponsavelLegal("H", "Pai");
            }
            else
                DesabilitaResponsavelLegal("D", "Pai");
        }

        protected void chkFalecidaMae_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFalecidaMae.Checked)
                DesabilitaResponsavelLegal("H", "Mãe");
            else
                DesabilitaResponsavelLegal("D", "Mãe");
        }

        protected void chkFalecidoPai_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFalecidoPai.Checked)
                DesabilitaResponsavelLegal("H", "Pai");
            else
                DesabilitaResponsavelLegal("D", "Pai");
        }

        protected void chkResponsavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNomeResponsavel.Text = string.Empty;
            txtCPFResponsavel.Text = string.Empty;
            txtTelefoneResp.Text = string.Empty;

            foreach (ListItem item in chkResponsavel.Items)
            {
                if (item.Text == "Mãe")
                    ExibeCamposObrigatoriosResponsavelLegal(lblCPFMae, tableCPFMae, item.Selected);
                if (item.Text == "Pai")
                    ExibeCamposObrigatoriosResponsavelLegal(lblCPFPai, tableCPFPai, item.Selected);
                if (item.Text == "Outros")
                    ExibeCamposResponsavel(item.Selected);
            }
        }

        private void ExibeCamposObrigatoriosResponsavelLegal(Label lblCPFResponsavelLegal, System.Web.UI.HtmlControls.HtmlTable tableCPFResponsavelLegal, bool cpfObrigatorioResponsavelLegal)
        {
            lblCPFResponsavelLegal.Text = cpfObrigatorioResponsavelLegal ? "CPF:*" : "CPF:";
            lblCPFResponsavelLegal.Font.Bold = cpfObrigatorioResponsavelLegal;
            tableCPFResponsavelLegal.Visible = cpfObrigatorioResponsavelLegal;
        }

        protected void ddlUnidadeEnsino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                trMensagemUnidadeEnsino.Visible = false;

                if (ddlUnidadeEnsino.SelectedValue == string.Empty)
                {
                    ddlCurso.Items.Clear();
                }
                else
                {
                    QueryTable dadosDrop = null;
                    dadosDrop = Techne.Lyceum.RN.Agenda.UnidadeEnsinoProcessoSeletivo.ListaUnidadeEnsinoCursoTurnoProcessoSeletivo(sessaoCandidato.AgendaID);

                    string mensagemUnidadeEnsino = Techne.Lyceum.RN.Agenda.UnidadeEnsinoProcessoSeletivo.MensagemUnidadeEnsino(Convert.ToInt32(ddlUnidadeEnsino.SelectedValue), sessaoCandidato.ProcessoSeletivoID);
                    if (!(string.IsNullOrEmpty(mensagemUnidadeEnsino)))
                    {
                        lblMensagemUnidadeEnsino.Text = mensagemUnidadeEnsino;
                        trMensagemUnidadeEnsino.Visible = true;
                    }

                    DataTable unidade = dadosDrop.Select("UNIDADEENSINOID = " + ddlUnidadeEnsino.SelectedValue).CopyToDataTable();
                    DataTable curso = unidade.DefaultView.ToTable(true, "NOMECURSO", "CURSOID");
                    CarregarDropDownList(ddlCurso, curso, "");
                    ddlCurso.SelectedValue = string.Empty;
                    ddlCurso.Focus();
                }

                ddlTurno.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosCandidatoInscrito()
        {
            DataTable dadoscandidato = new DataTable();
            DataTable dtRecursoProva = new DataTable();
            DataTable buscaCandidatoExistente = new DataTable();

            try
            {
                if (txtMatriculaSeeduc.Text != string.Empty)
                {
                    dadoscandidato = RN.ProcessoSeletivoAluno.Candidato.ListaDadosRedeEstadual(txtMatriculaSeeduc.Text);
                    if (dadoscandidato.Rows.Count == 0)
                    {
                        txtMatriculaSeeduc.Text = string.Empty;
                        HabilitaCamposMatriculaInformada(true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Número de matrícula da Rede Estadual - SEEDUC não encontrada!')", true);
                        return;
                    }
                    else
                    {
                        sessaoCandidato.LimpaSessaoCandidatoInscrito();
                        buscaCandidatoExistente = RN.ProcessoSeletivoAluno.Candidato.VerificaCandidatoExistente(dadoscandidato.Rows[0]["NOMECOMPLETO"].ToString(), dadoscandidato.Rows[0]["NOMEMAE"].ToString(), Convert.ToDateTime(dadoscandidato.Rows[0]["DATANASCIMENTO"]), sessaoCandidato.AgendaID);
                        if (buscaCandidatoExistente.Rows.Count > 0)
                        {
                            sessaoCandidato.CandidatoId = Convert.ToInt32(buscaCandidatoExistente.Rows[0]["CANDIDATOID"].ToString());
                            if (buscaCandidatoExistente.Rows[0]["INSCRICAOID"] != DBNull.Value)
                                sessaoCandidato.InscricaoId = Convert.ToInt32(buscaCandidatoExistente.Rows[0]["INSCRICAOID"]);
                            if (buscaCandidatoExistente.Rows[0]["NUMEROINSCRICAO"] != DBNull.Value)
                                sessaoCandidato.NumeroInscricao = Convert.ToInt64(buscaCandidatoExistente.Rows[0]["NUMEROINSCRICAO"]);
                        }
                        LimpaCampos();
                    }
                }
                else
                {
                    txtMatriculaSeeduc.Text = string.Empty;
                    if (sessaoCandidato.CandidatoId != int.MinValue)
                    {
                        dadoscandidato = Techne.Lyceum.RN.ProcessoSeletivoAluno.Candidato.ListaDadosInscricaoCandidato(sessaoCandidato.CandidatoId);
                    }
                }

                if (dadoscandidato.Rows.Count > 0)
                {
                    //Rede de Ensino de Origem
                    if (txtMatriculaSeeduc.Text == string.Empty)
                    {
                        rblTipoRedeEnsino.SelectedValue = dadoscandidato.Rows[0]["REDEENSINOORIGEM"].ToString();
                        if (rblTipoRedeEnsino.SelectedValue == "Particular")
                        {
                            ddlTipoBolsaParticular.SelectedValue = dadoscandidato.Rows[0]["TIPOBOLSAPARTICULAR"].ToString();

                            #region Retirada da opção "Bolsista Parcial" (ler comentário)

                            // Opção "Bolsista Parcial" removida em agosto/2014 para Processo Seletivo 2015
                            // por solicitação da SEEDUC através da demanda 5150. No banco de dados, o valor
                            // original (1) será mantido e reservado, caso seja solicitado seu retorno no
                            // futuro. Assim, por hora o campo poderá receber apenas os valores 0 e 2.
                            // Caso algum candidato com valor 1 ("Bolsista Parcial") seja carregado, o valor
                            // da combobox será "reiniciado".

                            if (dadoscandidato.Rows[0]["TIPOBOLSAPARTICULAR"].ToString() == "1")
                                ddlTipoBolsaParticular.SelectedIndex = -1;

                            #endregion

                            trBolsaParticular.Visible = true;
                        }
                    }

                    HabilitaCamposMatriculaInformada(string.IsNullOrEmpty(txtMatriculaSeeduc.Text.Trim()));

                    //Dados Pessoais
                    txtNomeCompletoAluno.Text = dadoscandidato.Rows[0]["NOMECOMPLETO"].ToString();
                    dtDataNasc.Date = Convert.ToDateTime(dadoscandidato.Rows[0]["DATANASCIMENTO"].ToString());
                    rblSexo.SelectedValue = dadoscandidato.Rows[0]["SEXO"].ToString();

                    //Dados do Cartório
                    ddlModeloCertidaoNascimento.SelectedValue = dadoscandidato.Rows[0]["MODELOCERTIDAOCIVIL"].ToString();

                    if (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Novo")
                    {
                        txtMatriculaCertidao.Text = dadoscandidato.Rows[0]["CERTIDAONUMEROMATRICULA"].ToString();
                        trMatriculaCertidao.Visible = true;
                    }
                    if (ddlModeloCertidaoNascimento.SelectedValue == "Modelo Antigo")
                    {
                        ddlUFCartorio.SelectedValue = dadoscandidato.Rows[0]["CERTIDAOCARTORIOUF"].ToString();
                        ddlUFCartorio_SelectedIndexChanged(null, EventArgs.Empty);
                        ddlMunicipioCartorio.SelectedValue = dadoscandidato.Rows[0]["codigo_municipio"].ToString();
                        ddlMunicipioCartorio_SelectedIndexChanged(null, EventArgs.Empty);
                        ddlCartorio.SelectedValue = dadoscandidato.Rows[0]["CARTORIOID"].ToString();
                        ddlCartorio_SelectedIndexChanged(null, EventArgs.Empty);
                        txtNumeroTermo.Text = dadoscandidato.Rows[0]["CERTIDAONUMERO"].ToString();
                        if (dadoscandidato.Rows[0]["CERTIDAODATAEMISSAO"] != DBNull.Value)
                            dtDataExped.Date = Convert.ToDateTime(dadoscandidato.Rows[0]["CERTIDAODATAEMISSAO"].ToString());
                        txtfolha.Text = dadoscandidato.Rows[0]["CERTIDAOFOLHA"].ToString();
                        txtlivro.Text = dadoscandidato.Rows[0]["CERTIDAOLIVRO"].ToString();

                        ExibeCamposCertidaoModeloAntigo(true);
                    }

                    //Dados da Mãe
                    txtNomeMae.Text = dadoscandidato.Rows[0]["NOMEMAE"].ToString();
                    if (dadoscandidato.Rows[0]["NOMEMAE"].ToString().Replace("Ã", "A").Equals("NAO DECLARADA"))
                    {
                        chkNaoDeclarMae.Checked = true;
                    }
                    chkFalecidaMae.Checked = dadoscandidato.Rows[0]["MAEFALECIDA"].ToString() == "S" ? (Boolean)true : (Boolean)false;
                    txtCPFMae.Text = dadoscandidato.Rows[0]["MAECPF"].ToString();
                    txtTelefoneMae.Text = dadoscandidato.Rows[0]["MAETELEFONE"].ToString();

                    if (chkFalecidaMae.Checked || chkNaoDeclarMae.Checked)
                        DesabilitaResponsavelLegal("H", "Mãe");

                    if (chkNaoDeclarMae.Checked)
                    {
                        txtCPFMae.Text = string.Empty;
                        txtTelefoneMae.Text = string.Empty;
                        chkFalecidaMae.Checked = false;
                        txtNomeMae.ReadOnly = true;
                        txtCPFMae.Enabled = false;
                        txtTelefoneMae.Enabled = false;
                        chkFalecidaMae.Enabled = false;
                    }

                    //Dados do Pai
                    txtNomePai.Text = dadoscandidato.Rows[0]["NOMEPAI"].ToString();
                    if (dadoscandidato.Rows[0]["NOMEPAI"].ToString().Replace("Ã", "A").Equals("NAO DECLARADO"))
                    {
                        chkNaoDeclarPai.Checked = true;
                    }
                    chkFalecidoPai.Checked = dadoscandidato.Rows[0]["PAIFALECIDO"].ToString() == "S" ? (Boolean)true : (Boolean)false;
                    txtCPFPai.Text = dadoscandidato.Rows[0]["PAICPF"].ToString();
                    txtTelefonePai.Text = dadoscandidato.Rows[0]["PAITELEFONE"].ToString();

                    if (chkFalecidoPai.Checked || chkNaoDeclarPai.Checked)
                        DesabilitaResponsavelLegal("H", "Pai");

                    if (chkNaoDeclarPai.Checked)
                    {
                        txtCPFPai.Text = string.Empty;
                        txtTelefonePai.Text = string.Empty;
                        chkFalecidoPai.Checked = false;
                        txtNomePai.ReadOnly = true;
                        txtCPFPai.Enabled = false;
                        txtTelefonePai.Enabled = false;
                        chkFalecidoPai.Enabled = false;
                    }

                    //Responsável
                    if (!string.IsNullOrEmpty(dadoscandidato.Rows[0]["RESPONSAVEL"].ToString()))
                    {
                        string[] tipoResponsavel = dadoscandidato.Rows[0]["RESPONSAVEL"].ToString().Split(';');
                        foreach (String str in tipoResponsavel)
                        {
                            if (!string.IsNullOrEmpty(str) && !(str == "Próprio Aluno"))
                            {
                                chkResponsavel.Items.FindByValue(str).Selected = true;
                            }
                        }
                    }

                    chkResponsavel_SelectedIndexChanged(null, EventArgs.Empty);

                    if (!string.IsNullOrEmpty(dadoscandidato.Rows[0]["RESPONSAVELNOME"].ToString()))
                    {
                        txtNomeResponsavel.Text = dadoscandidato.Rows[0]["RESPONSAVELNOME"].ToString();
                        txtCPFResponsavel.Text = dadoscandidato.Rows[0]["RESPONSAVELCPF"].ToString();
                        txtTelefoneResp.Text = dadoscandidato.Rows[0]["RESPONSAVELTELEFONE"].ToString();
                    }

                    //Endereço
                    txtCep.Text = dadoscandidato.Rows[0]["ENDERECOCEP"].ToString();
                    hdnCodMunicipio.Value = dadoscandidato.Rows[0]["ENDERECOMUNICIPIO"].ToString();
                    txtEstado.Value = dadoscandidato.Rows[0]["UF_SIGLA"].ToString();
                    txtEndereco.Text = dadoscandidato.Rows[0]["ENDERECO"].ToString();
                    txtEndNum.Text = dadoscandidato.Rows[0]["ENDERECONUMERO"].ToString();
                    txtEndCompl.Text = dadoscandidato.Rows[0]["ENDERECOCOMPLEMENTO"].ToString();
                    txtBairro.Text = dadoscandidato.Rows[0]["ENDERECOBAIRRO"].ToString();

                    txtMunicipio.Value = Endereco.ObterDescricaoMunicipio(dadoscandidato.Rows[0]["ENDERECOMUNICIPIO"].ToString());
                    txtEstado.Value = Endereco.ObterUFMunicipio(dadoscandidato.Rows[0]["ENDERECOMUNICIPIO"].ToString());

                    //Contato
                    txtCelular.Text = dadoscandidato.Rows[0]["CELULAR"].ToString();
                    txtFone.Text = dadoscandidato.Rows[0]["TELEFONE"].ToString();
                    txtEmail.Text = dadoscandidato.Rows[0]["EMAIL"].ToString();

                    //Deficiência
                    if (dadoscandidato.Rows[0]["NECESSIDADEESPECIAL"] != DBNull.Value)
                    {
                        ListItem listItem = ddlNecessidadeEspecial.Items.FindByValue(dadoscandidato.Rows[0]["NECESSIDADEESPECIAL"].ToString());
                        if (listItem != null)
                        {
                            ddlNecessidadeEspecial.SelectedValue = dadoscandidato.Rows[0]["NECESSIDADEESPECIAL"].ToString();
                        }
                        else
                        {
                            ListItem itemVazio = new ListItem("<SELECIONE>", "");
                            ddlNecessidadeEspecial.Items.Add(itemVazio);
                            ddlNecessidadeEspecial.SelectedValue = "";
                        }
                    }

                    if (sessaoCandidato.CandidatoId != int.MinValue)
                    {
                        dtRecursoProva = Techne.Lyceum.RN.ProcessoSeletivoAluno.RecursoAplicacaoProvaCandidato.ListaRecursosNecessariosParaProva(sessaoCandidato.CandidatoId);
                    }
                    else if (dadoscandidato.Columns.Contains("PESSOA"))
                    {
                        dtRecursoProva = RN.PessoaRecursoAplicacaoProva.Listar(Convert.ToInt32(dadoscandidato.Rows[0]["PESSOA"].ToString()));
                    }

                    CarregaDadosRecursoAplicacaoProva(dtRecursoProva);

                    //if (txtMatriculaSeeduc.Text != string.Empty)
                    //{
                    //    DataTable dadosRecursosAplicacaoProva = RN.PessoaRecursoAplicacaoProva.Listar(Convert.ToInt32(dadoscandidato.Rows[0]["PESSOA"].ToString()));
                    //    CarregaDadosRecursoAplicacaoProva(dadosRecursosAplicacaoProva);
                    //}
                    //else
                    //{
                    //    CarregaDadosRecursoAplicacaoProva(Techne.Lyceum.RN.ProcessoSeletivoAluno.RecursoAplicacaoProvaCandidato.ListaRecursosNecessariosParaProva(sessaoCandidato.CandidatoId));
                    //}

                    VerificaNecessidadeEspecial();

                    if (rblTipoRedeEnsino.SelectedValue == "Estadual")
                    {
                        txtMatriculaSeeduc.Text = dadoscandidato.Rows[0]["ALUNOID"].ToString();
                        trRedeEstadualSeeduc.Visible = true;
                        HabilitaCamposMatriculaInformada(string.IsNullOrEmpty(txtMatriculaSeeduc.Text.Trim()));
                    }

                    if (sessaoCandidato.NumeroInscricao != Int64.MinValue)
                    {
                        DataTable unidadeCursoInscrito = RN.ProcessoSeletivoAluno.UnidadeEnsinoCursoTurnoInscricao.ListaUnidadeEnsinoCursoInscricao(sessaoCandidato.NumeroInscricao);
                        ddlUnidadeEnsino.SelectedValue = unidadeCursoInscrito.Rows[0]["UNIDADEENSINOID"].ToString();
                        ddlUnidadeEnsino_SelectedIndexChanged(null, EventArgs.Empty);

                        ddlCurso.SelectedValue = unidadeCursoInscrito.Rows[0]["CURSOID"].ToString();
                        ddlCurso_SelectedIndexChanged(null, EventArgs.Empty);

                        ddlTurno.SelectedValue = unidadeCursoInscrito.Rows[0]["TURNOID"].ToString();
                    }

                    txtNomeCompletoAluno.Focus();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void HabilitaCamposMatriculaInformada(bool habilita)
        {
            txtNomeCompletoAluno.Enabled = habilita;
            dtDataNasc.Enabled = habilita;
            txtNomeMae.Enabled = habilita;
            chkNaoDeclarMae.Enabled = habilita;
            chkFalecidaMae.Enabled = habilita;
        }

        private void ExibeCamposResponsavel(bool exibe)
        {
            tbNomeResponsavel.Visible = exibe;
            lblTelefoneResponsavel.Visible = exibe;
            txtTelefoneResp.Visible = exibe;
            lblCPFResponsavel.Visible = exibe;
            txtCPFResponsavel.Visible = exibe;
            tbValidacaoCPF.Visible = exibe;
        }

        private void ExibeCamposCertidaoModeloAntigo(bool exibeCampos)
        {
            trCertidaoAntigaCartorio.Visible = exibeCampos;
            trCertidaoAntigaDados.Visible = exibeCampos;
            trCertidaoAntigaDadosExpedicao.Visible = exibeCampos;
            trCertidaoAntigaFiltro.Visible = exibeCampos;
        }

        protected void btConfirmarDados_Click(object sender, EventArgs e)
        {
            DateTime dataCadastro = DateTime.Now;

            var tipoResponsavel = string.Empty;
            try
            {

                if (txtNomeMae.Text.Replace("Ã", "A").Equals("NAO DECLARADA"))
                {
                    chkNaoDeclarMae.Checked = true;
                    chkNaoDeclarMae_CheckedChanged(null, EventArgs.Empty);
                }

                if (txtNomePai.Text.Replace("Ã", "A").Equals("NAO DECLARADO"))
                {
                    chkNaoDeclarPai.Checked = true;
                    chkNaoDeclarPai_CheckedChanged(null, EventArgs.Empty);
                }

                foreach (ListItem item in chkResponsavel.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        tipoResponsavel += item.Value;
                        tipoResponsavel += ";";
                    }
                }

                var dadosCandidato = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.Candidato
                {
                    RedeEnsino = rblTipoRedeEnsino.SelectedValue,
                    TipoBolsaParticular = ddlTipoBolsaParticular.SelectedValue,
                    NomeCompleto = txtNomeCompletoAluno.Text.Trim(),
                    DataNascimento = !string.IsNullOrEmpty(dtDataNasc.Text.Trim())
                                                 ? dtDataNasc.Date
                                                 : (DateTime?)null,
                    Sexo = rblSexo.SelectedValue,
                    TipoCertidao = "Nascimento",
                    ModeloCertidaoCivil = !string.IsNullOrEmpty(ddlModeloCertidaoNascimento.SelectedValue)
                                                   ? ddlModeloCertidaoNascimento.SelectedValue
                                                   : string.Empty,
                    CertidaoNumeroMatricula = txtMatriculaCertidao.Text.Trim(),
                    CertidaoCartorioUF = Convert.ToString(ddlUFCartorio.SelectedValue),
                    CartorioID = !string.IsNullOrEmpty(ddlCartorio.SelectedValue)
                                                   ? int.Parse(ddlCartorio.SelectedValue)
                                                   : 0,
                    CertidaoCartorioExpedicao = !string.IsNullOrEmpty(ddlCartorio.SelectedValue)
                                                   ? ddlCartorio.SelectedItem.Text
                                                   : string.Empty,
                    CertidaoNumero = txtNumeroTermo.Text.Trim(),
                    CertidaoDataEmissao = !string.IsNullOrEmpty(dtDataExped.Text.Trim())
                                                 ? dtDataExped.Date
                                                 : (DateTime?)null,
                    CertidaoFolha = txtfolha.Text.Trim(),
                    CertidaoLivro = txtlivro.Text.Trim(),
                    NomeMãe = txtNomeMae.Text.Trim(),
                    MaeCPF = txtCPFMae.Text.Trim(),
                    MaeFalecida = (chkFalecidaMae.Checked == true) ? (string)"S" : (string)"N",
                    PaiFalecido = (chkFalecidoPai.Checked == true) ? (string)"S" : (string)"N",
                    MaeTelefone = txtTelefoneMae.Text.Trim(),
                    NomePai = txtNomePai.Text.Trim(),
                    PaiCPF = txtCPFPai.Text.Trim(),
                    PaiTelefone = txtTelefonePai.Text.Trim(),
                    ResponsavelCPF = txtCPFResponsavel.Text.Trim(),
                    ResponsavelNome = txtNomeResponsavel.Text.Trim(),
                    Responsavel = tipoResponsavel,
                    ResponsavelTelefone = txtTelefoneResp.Text.Trim(),
                    EnderecoCep = txtCep.Text.Trim(),
                    EnderecoMunicipio = hdnCodMunicipio.Value,
                    Endereco = txtEndereco.Text.Trim(),
                    EnderecoNumero = txtEndNum.Text.Trim(),
                    EnderecoCompleto = txtEndCompl.Text.Trim(),
                    EnderecoBairro = txtBairro.Text.Trim(),
                    Telefone = txtFone.Text.Trim(),
                    Celular = txtCelular.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NecessidadeEspecial = ddlNecessidadeEspecial.Text.Trim(),
                    IP = Request.UserHostAddress.ToString(),
                    DataCadastro = dataCadastro,
                    DataAlteracao = dataCadastro,
                    Candidatoid = Convert.ToString(sessaoCandidato.CandidatoId),
                    AlunoID = txtMatriculaSeeduc.Text,
                };

                var inscricao = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.Inscricao
                {
                    ProcessoSeletivoId = sessaoCandidato.ProcessoSeletivoID,
                    IP = dadosCandidato.IP,
                    DataCadastro = dataCadastro,
                    DataAlteracao = dataCadastro,
                    Situacao = Techne.Lyceum.RN.ProcessoSeletivoAluno.Inscricao.Situacao.Inscrito,
                };

                var unidadeEnsinoCursoTurnoInscricao = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.UnidadeEnsinoCursoTurnoInscricao
                {
                    UnidadeEnsinoId = ddlUnidadeEnsino.SelectedValue,
                    CursoId = ddlCurso.SelectedValue,
                    TurnoId = ddlTurno.SelectedValue,
                    DataCadastro = dataCadastro,
                    DataAlteracao = dataCadastro,
                    IP = dadosCandidato.IP,
                };

                var listCandidatoRecursoAplicacaoProva = new List<Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato>();

                if (ddlNecessidadeEspecial.SelectedValue != "Não possui." && !string.IsNullOrEmpty(ddlNecessidadeEspecial.SelectedValue))
                {
                    foreach (ListItem li in rblRecursoAplicaProvaExclusivo.Items)
                    {
                        if (li.Selected)
                        {
                            Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato recursoAplicacaoProvaCandidato = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato();
                            recursoAplicacaoProvaCandidato.RecursoAplicacaoProvaId = Convert.ToInt32(li.Value);
                            listCandidatoRecursoAplicacaoProva.Add(recursoAplicacaoProvaCandidato);
                        }
                    }

                    foreach (ListItem li in chkRecursoAplicacaoProva.Items)
                    {
                        if (li.Selected)
                        {
                            Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato recursoAplicacaoProvaCandidato = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato();
                            recursoAplicacaoProvaCandidato.RecursoAplicacaoProvaId = Convert.ToInt32(li.Value);
                            listCandidatoRecursoAplicacaoProva.Add(recursoAplicacaoProvaCandidato);
                        }
                    }
                }

                List<string> validacaoDadosMensagem = ValidarDadosInscricao();

                if (validacaoDadosMensagem.Count == 0)
                {
                    //Salva Candidato
                    Int64 numeroInscricao = 0;
                    int inscricaoId = sessaoCandidato.InscricaoId;
                    int candidatoId = sessaoCandidato.CandidatoId;
                    bool alteracaoDados = false;

                    if (candidatoId == int.MinValue)
                    {
                        RN.ProcessoSeletivoAluno.Candidato.SalvaDadosCandidato(dadosCandidato, listCandidatoRecursoAplicacaoProva, inscricao, unidadeEnsinoCursoTurnoInscricao, sessaoCandidato.AgendaID, out inscricaoId, out numeroInscricao);
                    }

                    else
                    {
                        RN.ProcessoSeletivoAluno.Candidato.AlteraDadosCandidato(dadosCandidato, listCandidatoRecursoAplicacaoProva, inscricao, unidadeEnsinoCursoTurnoInscricao, sessaoCandidato.AgendaID, candidatoId, ref inscricaoId, ref numeroInscricao);
                        alteracaoDados = true;
                    }

                    if (inscricaoId != int.MinValue)
                        sessaoCandidato.InscricaoId = inscricaoId;
                    if (numeroInscricao != Int64.MinValue)
                        sessaoCandidato.NumeroInscricao = numeroInscricao;

                    //Envia Email Confirmação
                    if (!string.IsNullOrEmpty(txtEmail.Text))
                    {
                        EnviaEmailConfirmacao(alteracaoDados);
                    }

                    Response.Redirect("Confirmacao.aspx");
                }
                else
                {
                    string MensagensErro = validacaoDadosMensagem.Aggregate((x, y) => x + "\\n" + y);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" + MensagensErro + "')", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void EnviaEmailConfirmacao(bool alteracaoDados)
        {
            try
            {
                RN.ProcessoSeletivoAluno.DTOs.ConfirmacaoProcessoSeletivoAluno confirmacaoProcessoSeletivoAluno = RN.ProcessoSeletivoAluno.Inscricao.ListaConfirmacao_ProcessoSeletivoAlunoPorInscricao(sessaoCandidato.NumeroInscricao);

                //Proderj.Framework.Common.Email email = new Proderj.Framework.Common.Email();

                string remetente = "procseletivo@educacao.rj.gov.br";
                string destinatario = txtEmail.Text;
                string assunto = "Inscrição Processo Seletivo 2014";

                if (alteracaoDados)
                    assunto = string.Concat("Alteração de Dados - ", assunto);

                string texto = "<p>" +
                                    " Confirmação de Inscrição Processo Seletivo " + confirmacaoProcessoSeletivoAluno.NumeroEdital +
                                    " <br />" +
                                    " <br />" +
                                    " Número Incrição: " + confirmacaoProcessoSeletivoAluno.NumeroInscricao +
                                    " <br />" +
                                    " Nome do Candidato: " + confirmacaoProcessoSeletivoAluno.NomeCandidato +
                                    " <br />" +
                                    " Data de Nascimento: " + confirmacaoProcessoSeletivoAluno.DataNascimento +
                                    " <br />" +
                                    " Nome da Mãe: " + confirmacaoProcessoSeletivoAluno.NomeMae +
                                    " <br />" +
                                    " Tipo de Deficiência: " + confirmacaoProcessoSeletivoAluno.NecessidadeEspecial +
                                    " <br />" +
                                    " Recurso Necessário para Prova: " + confirmacaoProcessoSeletivoAluno.RecursosNecessarioProva +
                                    " <br />" +
                                    " Unidade de Ensino: " + confirmacaoProcessoSeletivoAluno.UnidadeEnsino +
                                    " <br />" +
                                    " Curso: " + confirmacaoProcessoSeletivoAluno.Curso +
                                    " <br />" +
                                    " Efetuado em: " + confirmacaoProcessoSeletivoAluno.DataAlteracao +
                                    " <br />" +
                                    " Endereço IP: " + confirmacaoProcessoSeletivoAluno.IP +
                                    " <br />" +
                                    " <br />" +
                                    " <br />" +
                                    " <br />" +
                                    " <br />" +
                                    " ---" +
                                    " Esse e-mail foi gerado automaticamente, favor não responder" +
                                "</p>";

				Proderj.Framework.Common.Email.Enviar(remetente, assunto, texto, destinatario);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Não foi possível enviar o e-mail de confirmação.')", true);
            }
        }
        protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryTable dadosDrop = null;

            if (ddlCurso.SelectedValue == string.Empty)
            {
                ddlTurno.Items.Clear();
            }
            else
            {
                dadosDrop = Techne.Lyceum.RN.Agenda.UnidadeEnsinoProcessoSeletivo.ListaUnidadeEnsinoCursoTurnoProcessoSeletivo(sessaoCandidato.AgendaID);

                DataTable curso = dadosDrop.Select("CURSOID = '" + ddlCurso.SelectedValue + "'").CopyToDataTable();
                DataTable turno = curso.DefaultView.ToTable(true, "NOMETURNO", "TURNOID");
                CarregarDropDownList(ddlTurno, turno, "");
                ddlTurno.SelectedValue = string.Empty;
                ddlTurno.Focus();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMatriculaSeeduc.Text))
            {
                CarregaDadosCandidatoInscrito();
            }
            else
            {
                HabilitaCamposMatriculaInformada(true);
            }
        }

        private void LimpaCampos()
        {
            //Dados Pessoais
            txtNomeCompletoAluno.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
            rblSexo.SelectedIndex = -1;

            //Dados do Cartório
            ddlModeloCertidaoNascimento.SelectedValue = "";
            txtMatriculaCertidao.Text = string.Empty;
            trMatriculaCertidao.Visible = false;
            ddlUFCartorio.SelectedValue = "";
            ddlUFCartorio_SelectedIndexChanged(null, EventArgs.Empty);
            txtNumeroTermo.Text = string.Empty;
            dtDataExped.Text = string.Empty;
            txtfolha.Text = string.Empty;
            txtlivro.Text = string.Empty;
            ExibeCamposCertidaoModeloAntigo(false);

            //Dados da Mãe
            txtNomeMae.Text = string.Empty;
            txtNomeMae.ReadOnly = false;
            chkNaoDeclarMae.Checked = false;
            chkNaoDeclarMae.Enabled = true;
            chkFalecidaMae.Checked = false;
            chkFalecidaMae.Enabled = true;
            txtCPFMae.Text = string.Empty;
            txtCPFMae.Enabled = true;
            txtTelefoneMae.Text = string.Empty;
            txtTelefoneMae.Enabled = true;
            DesabilitaResponsavelLegal("D", "Mãe");

            //Dados do Pai
            txtNomePai.Text = string.Empty;
            txtNomePai.ReadOnly = false;
            chkNaoDeclarPai.Checked = false;
            chkNaoDeclarPai.Enabled = true;
            chkFalecidoPai.Checked = false;
            chkFalecidoPai.Enabled = true;
            txtCPFPai.Text = string.Empty;
            txtCPFPai.Enabled = true;
            txtTelefonePai.Text = string.Empty;
            txtTelefonePai.Enabled = true;
            DesabilitaResponsavelLegal("D", "Pai");

            //Dados do Responsável
            chkResponsavel.SelectedIndex = -1;
            txtNomeResponsavel.Text = string.Empty;
            txtCPFResponsavel.Text = string.Empty;
            txtTelefoneResp.Text = string.Empty;
            ExibeCamposResponsavel(false);

            //Endereço
            txtCep.Text = string.Empty;
            hdnCodMunicipio.Value = string.Empty;
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtMunicipio.Value = string.Empty;

            //Contato
            txtCelular.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtEmailConfirmacao.Text = string.Empty;

            //Deficiência
            ddlNecessidadeEspecial.SelectedValue = "Não possui.";
            VerificaNecessidadeEspecial();
            chkRecursoAplicacaoProva.SelectedIndex = -1;
            rblRecursoAplicaProvaExclusivo.SelectedIndex = -1;
            chkNenhumRecursoAplicacaoProva.Checked = false;

            //Unidade de Ensino
            ddlUnidadeEnsino.SelectedValue = string.Empty;
            ddlUnidadeEnsino_SelectedIndexChanged(null, EventArgs.Empty);
        }
    }
}
