using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using Techne.Controls;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/AlteraDadosCandidato.aspx")]
    [ControlText("AlteraDadosCandidato")]
    [Title("Altera Dados do Candidato")]
    public partial class AlteraDadosCandidato : TPage
    {
        public object ListarFase1(object inscricaoAlunoId)
        {
            RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();

            var inscricao = inscricaoAlunoId != null ? inscricaoAlunoId.ToString() : null;
          
            if (!inscricao.IsNullOrEmptyOrWhiteSpace())
            {
                return opcao.ListaAlocacaoFase1Por(Convert.ToInt32(inscricao));

            }
            return null;
        }

        public object ListarOpcao(object inscricaoAlunoId)
        {
            RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();

            var inscricao = inscricaoAlunoId != null ? inscricaoAlunoId.ToString() : null;

            if (!inscricao.IsNullOrEmptyOrWhiteSpace())
            {
                return opcao.ListaPor(Convert.ToInt32(inscricao));

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

        public object ListarOpcaoHist(object inscricaoAlunoId)
        {
            RN.Matriculas.OpcaoInscricaoHist opcaoHist = new Techne.Lyceum.RN.Matriculas.OpcaoInscricaoHist();

            var inscricao = inscricaoAlunoId != null ? inscricaoAlunoId.ToString() : null;

            if (!inscricao.IsNullOrEmptyOrWhiteSpace())
            {
                return opcaoHist.ListaPor(Convert.ToInt32(inscricao));

            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    dvGeral.Visible = false;
                    dvFase1.Visible = false;
                    LimparDadosBusca();
                    LimparDadosCadastro();
                    CarregaAno();
                }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdOpcoesFase1, "Fase 1 - Opções Alocação");
            TituloGrid(grdOpcoes, "Opções Ativas");
            TituloGrid(grdOpcoesFinalizadas, "Histórico");
            TituloGrid(grdOpcoesIrmaoForaRede, "Lista de Escolas escolhidas pelo seu irmão");
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void LimparDadosBusca()
        {
            ddlAno.ClearSelection();
            tableBusca.Visible = false;
            rblTipoBusca.SelectedIndex = -1;
            dtBuscaDataNasc.Text = string.Empty;
            txtBuscaNumInscricao.Text = string.Empty;
            txtBuscaCPF.Text = string.Empty;
        }

        private void LimparDadosCadastro()
        {
            txtAlteraNome.Text = string.Empty;
            dtAlteraNascimento.Value = null;
            txtAlteraNomeMae.Text = string.Empty;
            ddlAlteraRedeOrigem.ClearSelection();

            hdnInscricaoAlunoId.Value = string.Empty;
            hdnPreCadastroAlunoId.Value = string.Empty;
            txtNomeCadastro.Text = string.Empty;
            txtRedeOrigem.Text = string.Empty;
            txtNumeroInscricao.Text = string.Empty;
            DtNascimentoCadastro.Text = string.Empty;
            ddlSexo.ClearSelection();
            ddlEstadoCivil.ClearSelection();
            ddlNacionalidade.ClearSelection();
            ddlUfNascimento.ClearSelection();
            ddlMunicipioNascimento.Items.Clear();
            txtNomePai.Text = string.Empty;
            txtNomeMaeCadastro.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chkNaoDeclarMaeCadastro.Checked = false;
            chkNaoDeclarPai.Checked = false;
            txtCelularFixo.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtLogradouro.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtMunicipio.Value = string.Empty;
            ddlBairro.Items.Clear();
            txtCPF.Text = string.Empty;
            txtRgNumero.Text = string.Empty;
            txtRgEmissor.Text = string.Empty;
            txtRgUf.Text = string.Empty;
            txtRgResponsavelEmissor.Text = string.Empty;
            txtRgResponsavelNumero.Text = string.Empty;
            txtRgResponsavelUf.Text = string.Empty;
            rblModeloCertidao.ClearSelection();
            rblTipoCertidao.ClearSelection();
            txtDOC_CertNasc_Numero.Text = string.Empty;
            txtDOC_CertNasc_Folha.Text = string.Empty;
            txtDOC_CertNasc_Livro.Text = string.Empty;
            txtNumMatriculaCertidao.Text = string.Empty;
            ddlDeficiencia.ClearSelection();
            rblResponsavel.ClearSelection();
            txtResponsavel.Text = string.Empty;
            txtCPFResponsavelCadastro.Value = string.Empty;
            txtCelularResponsavel.Text = string.Empty;

            hdnIdIrmao.Value = string.Empty;
            chkNaoPossuiIrmao.Checked = false;
            chkIrmaoForaRede.Checked = false;
            chkIrmaoRede.Checked = false;
            txtMatriculaIrmao.Text = string.Empty;
            txtInscricaoIrmao.Text = string.Empty;
            txtNomeIrmaoForaRede.Text = string.Empty;
            txtNomeIrmaoRede.Text = string.Empty;
            txtDataNascIrmaoForaRede.Text = string.Empty;
            txtDataNascIrmaoRede.Text = string.Empty;
            txtUEIrmaoRede.Text = string.Empty;
            txtCursoIrmaoRede.Text = string.Empty;
            txtSerieIrmaoRede.Text = string.Empty;
            txtTurnoIrmaoRede.Text = string.Empty;

        }

        protected void CarregaBairro(string municipioId)
        {
            RN.Bairro rnBairro = new Techne.Lyceum.RN.Bairro();
            ddlBairro.Items.Clear();
            ddlBairro.DataSource = rnBairro.ListaPor(municipioId);
            ddlBairro.DataBind();
            ddlBairro.Items.Insert(0, new ListItem("Selecione", string.Empty));

        }

        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);
            ListItem itemNaoPossui = new ListItem("Não Possui", "30");

            ddlDeficiencia.Items.Clear();
            ddlDeficiencia.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            ddlDeficiencia.DataBind();
            ddlDeficiencia.Items.Insert(0, itemVazio);
            ddlDeficiencia.Items.Insert(1, itemNaoPossui);
        }

        private void CarregaEstadoCivil()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEstadoCivil.Items.Clear();
            ddlEstadoCivil.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.EstadoCivil);
            ddlEstadoCivil.DataBind();
            ddlEstadoCivil.Items.Insert(0, item);
        }

        protected void CarregaMunicipio(string uf)
        {
            RN.Municipio rnMunicipio = new Techne.Lyceum.RN.Municipio();
            ddlMunicipioNascimento.Items.Clear();
            ddlMunicipioNascimento.DataSource = rnMunicipio.ListaPor(uf);
            ddlMunicipioNascimento.DataBind();
            ddlMunicipioNascimento.Items.Insert(0, new ListItem("Selecione", string.Empty));

        }

        private void CarregaNacionalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlNacionalidade.Items.Clear();
            ddlNacionalidade.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nacionalidade, RN.Basico.QueryListaNacionalidades);
            ddlNacionalidade.DataBind();
            ddlNacionalidade.Items.Insert(0, item);

            item = ddlNacionalidade.Items.FindByText("BRASILEIRA");
            if (item != null)
            {
                ddlNacionalidade.ClearSelection();
                item.Selected = true;
            }
        }

        private void CarregaUFNascimento()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlUfNascimento.Items.Clear();
            ddlUfNascimento.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.UfNaturalidade, RN.Basico.QueryListaUFNaturalidade);
            ddlUfNascimento.DataBind();
            ddlUfNascimento.Items.Insert(0, item);

        }



        protected void chkAlteraMaeNaoDeclarada_CheckedChange(object sender, EventArgs e)
        {
            var self = sender as CheckBox;

            txtAlteraNomeMae.Text = self.Checked ? "NÃO DECLARADA" : txtNomeMaeCadastro.Text;
            txtAlteraNomeMae.Enabled = !self.Checked;
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            var self = sender as DropDownList;

            LimparDadosCadastro();
            dvGeral.Visible = false;

            tableBusca.Visible = !self.SelectedValue.IsNullOrEmptyOrWhiteSpace();
            rblTipoBusca.SelectedIndex = -1;
            rblTipoBusca_SelectedIndexChanged(rblTipoBusca, null);
            
            txtBuscaNumInscricao.Text = string.Empty;
            dtBuscaDataNasc.Value = null;
            txtBuscaCPF.Text = string.Empty;
        }

        protected void chkAlteraSouAluno_CheckedChange(object sender, EventArgs e)
        {
            var self = sender as CheckBox;

            Label6.Visible = self.Checked;
            txtAlteraMatricula.Visible = self.Checked;

            if (self.Checked)
            {
                txtAlteraMatricula.Text = txtAlteraMatricula.Attributes["matricula"];
                txtAlteraMatricula.Attributes.Remove("matricula");
            }
            else
            {
                txtAlteraMatricula.Attributes.Add("matricula", txtAlteraMatricula.Text);
                txtAlteraMatricula.Text = string.Empty;
            }
        }

        protected void ddlAlteraRedeOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            var self = sender as DropDownList;

            if (self.SelectedValue != "Estadual")
            {
                chkAlteraSouAluno.Checked = false;
                chkAlteraSouAluno.Enabled = false;
                chkAlteraSouAluno_CheckedChange(chkAlteraSouAluno, null);
            }
            else
            {
                chkAlteraSouAluno.Checked = false;
                chkAlteraSouAluno.Enabled = true;
                chkAlteraSouAluno_CheckedChange(chkAlteraSouAluno, null);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var rnPreCadastroAluno = new RN.Matriculas.PreCadastroAluno();
                var rnInscricaoAluno = new RN.Matriculas.InscricaoAluno();

                var ano = Convert.ToInt32(ddlAno.SelectedValue);
                var inscricaoAlunoId = 0; int.TryParse(hdnInscricaoAlunoId.Value, out inscricaoAlunoId);
                var preCadastroAlunoId = 0; int.TryParse(hdnPreCadastroAlunoId.Value, out preCadastroAlunoId);
                var nome = txtAlteraNome.Text.ToUpper();
                var nomeMae = txtAlteraNomeMae.Text.ToUpper();
                var dataNascimento = Convert.ToDateTime(dtAlteraNascimento.Value);
                var redeOrigemInscricao = ddlAlteraRedeOrigem.SelectedValue;
                var aluno = txtAlteraMatricula.Text = chkAlteraSouAluno.Checked && txtAlteraMatricula.Text != "" ? txtAlteraMatricula.Text : null;
                var necessidadeEspecialId = ddlDeficiencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlDeficiencia.SelectedValue);

                if (inscricaoAlunoId != 0)
                {
                    var validacao = rnInscricaoAluno.ValidaAlteraDadosInscricao(inscricaoAlunoId, ano, nome, nomeMae, dataNascimento, redeOrigemInscricao, necessidadeEspecialId, aluno);
                    if (validacao.Valido)
                    {
                        rnInscricaoAluno.AlteraDadosInscricao(inscricaoAlunoId, nome, nomeMae, dataNascimento, redeOrigemInscricao, aluno, User.Identity.Name);
                        btnBuscarCandidato_Click(sender, null);

                        lblMensagem.Text = "Salvo com sucesso";

                        if (grdOpcoes.VisibleRowCount == 0)
                            lblMensagem.Text += ", porém atentar-se à data de nascimento, pois não foi validada porque o aluno não possui opções ativas.";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem;
                    }
                }
                else
                {
                    lblMensagem.Text = "A inscrição não foi localizada.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscarCandidato_Click(object sender, EventArgs e)
        {
            try
            {
                var dados = new DadosCandidato();
                var rnInscricao = new Techne.Lyceum.RN.Matriculas.InscricaoAluno();
                var rnAgenda = new Techne.Lyceum.RN.Matriculas.Agenda();
                
                //limpar a tela
                LimparDadosCadastro();
                dvGeral.Visible = false;
                dvFase1.Visible = false;

                //verificar se o ano está preenchido
                if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "É necessário informar o ANO.";
                    return;
                }

                //verifica se o ano é o ano corrente da matrícula
                var anoCorrente = rnAgenda.RetornaAnoCorrenteMatricula();
                var somenteLeitura = ddlAno.SelectedValue != anoCorrente.ToString();

                //verificar se o tipo de busca foi escolhido
                if (!new string[] { "1", "2" }.Contains(rblTipoBusca.SelectedValue))
                {
                    lblMensagem.Text = "É necessário informar o TIPO DE BUSCA.";
                    return;
                }

                //para o tipo de busca = num. de inscrição e dt. de nascimento...
                if (rblTipoBusca.SelectedValue == "1")
                {
                    //verificar se o num. de inscrição está preenchido
                    if (txtBuscaNumInscricao.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "É necessário informar o NÚMERO DA INSCRIÇÃO.";
                        return;
                    }

                    //verificar se a dt. de nasc. está preenchida
                    if (dtBuscaDataNasc.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "É necessário informar a DATA DE NASCIMENTO.";
                        return;
                    }
                }

                //para o tipo de busca = cpf...
                if (rblTipoBusca.SelectedValue == "2")
                {
                    //verificar se o cpf está preenchido
                    if (txtBuscaCPF.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "É necessário informar o CPF.";
                        return;
                    }
                }

                //todas as condições atendidas, obter os dados do candidato
                dados = rnInscricao.ObtemDadosCandidatoPor(
                    (!dtBuscaDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dtBuscaDataNasc.Date : (DateTime?)null),
                    Convert.ToInt32(ddlAno.SelectedValue),
                    txtBuscaNumInscricao.Text.Trim(),
                    txtBuscaCPF.Text.RetirarMascaraCPF()
                );

                //se o candidato não for localizado...
                if (dados.PreCadastroAlunoId <= 0)
                {
                    lblMensagem.Text = "Candidato não localizado com os dados informados.";
                    return;
                }

                //preparar a tela para exibir os dados do candidato
                CarregaNecessidadeEspecial();
                CarregaEstadoCivil();
                CarregaNacionalidade();
                CarregaUFNascimento();
                PreencheDadosCandidato(dados);

                Panel1.Visible = !somenteLeitura;
                lblMensagemSomenteLeitura.Visible = somenteLeitura;

                dvGeral.Visible = true;
                odsFase1.Select();
                odsFase1.DataBind();
                grdOpcoesFase1.DataBind();
                if (grdOpcoesFase1.VisibleRowCount > 0)
                {
                    dvFase1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblTipoBusca_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparDadosCadastro();
            dvGeral.Visible = false;
            txtBuscaCPF.Text = string.Empty;
            txtBuscaNumInscricao.Text = string.Empty;
            dtBuscaDataNasc.Value = null;

            if (rblTipoBusca.SelectedValue == "2")
            {
                trPorCPF.Visible = true;
                trPorNumInscricao.Visible = false;    
            }
            else if (rblTipoBusca.SelectedValue == "1")
            {
                trPorNumInscricao.Visible = true;
                trPorCPF.Visible = false;
            }
            else
            {
                trPorNumInscricao.Visible = false;
                trPorCPF.Visible = false;
            }
        }

        protected void rblModeloCertidao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNumMatriculaCertidao.Text = string.Empty;
                txtDOC_CertNasc_Folha.Text = string.Empty;
                txtDOC_CertNasc_Livro.Text = string.Empty;
                txtDOC_CertNasc_Numero.Text = string.Empty;

                if (rblModeloCertidao.SelectedValue == "Modelo Novo")
                {
                    pnlNovo.Visible = true;
                    pnlAntigo.Visible = false;
                }
                else if (rblModeloCertidao.SelectedValue == "Modelo Antigo")
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = true;
                }
                else
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblResponsavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtResponsavel.Text = string.Empty;

                foreach (ListItem item in rblResponsavel.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Text == "Outros")
                        {
                            txtResponsavel.Enabled = true;
                        }
                        if (item.Text == "Mãe")
                        {
                            txtResponsavel.Text = txtNomeMaeCadastro.Text.Trim();
                        }

                        if (item.Text == "Pai")
                        {
                            txtResponsavel.Text = txtNomePai.Text.Trim();
                        }

                        if (item.Text == "O próprio candidato")
                        {
                            txtResponsavel.Text = txtNomeCadastro.Text.Trim();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void PreencheDadosCandidato(RN.DTOs.DadosCandidato dadosCandidato)
        {
            txtAlteraNome.Text = !dadosCandidato.Nome.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.Nome : string.Empty;
            if (dadosCandidato.DataNascimento.HasValue)
            {
                dtAlteraNascimento.Value = dadosCandidato.DataNascimento.Value;
            }
            txtAlteraNomeMae.Text = !dadosCandidato.NomeMae.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.NomeMae : string.Empty;
            chkAlteraMaeNaoDeclarada.Checked = dadosCandidato.NomeMae.Trim().ToLower() == "não declarada";
            txtAlteraNomeMae.Enabled = !chkAlteraMaeNaoDeclarada.Checked;
            if (!dadosCandidato.RedeOrigem.IsNullOrEmptyOrWhiteSpace())
            {
                ddlAlteraRedeOrigem.SelectedValue = dadosCandidato.RedeOrigem.Capitaliza();
            }
            chkAlteraSouAluno.Checked = !dadosCandidato.Aluno.IsNullOrEmptyOrWhiteSpace();
            chkAlteraSouAluno_CheckedChange(chkAlteraSouAluno, null);
            txtAlteraMatricula.Text = dadosCandidato.Aluno;

            hdnInscricaoAlunoId.Value = dadosCandidato.InscricaoAlunoId.ToString();
            hdnPreCadastroAlunoId.Value = dadosCandidato.PreCadastroAlunoId.ToString();
            txtNomeCadastro.Text = !dadosCandidato.Nome.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.Nome : string.Empty;
            txtNumeroInscricao.Text = dadosCandidato.NumeroInscricao == null ? string.Empty : dadosCandidato.NumeroInscricao.ToString();
            txtRedeOrigem.Text = dadosCandidato.RedeOrigem;
            if (dadosCandidato.DataNascimento.HasValue)
            {
                DtNascimentoCadastro.Text = dadosCandidato.DataNascimento.Value.ToString("dd/MM/yyyy");
            }
            if (!dadosCandidato.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                if (ddlSexo.Items.FindByValue(dadosCandidato.Sexo) != null)
                {
                    ddlSexo.SelectedValue = ddlSexo.Items.FindByValue(dadosCandidato.Sexo).Value;
                }
            } 

            if (!dadosCandidato.EstadoCivil.IsNullOrEmptyOrWhiteSpace())
            {
                if (ddlEstadoCivil.Items.FindByValue(dadosCandidato.EstadoCivil) != null)
                {
                    ddlEstadoCivil.SelectedValue = ddlEstadoCivil.Items.FindByValue(dadosCandidato.EstadoCivil).Value;
                }
            } 

            if (!dadosCandidato.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                if (ddlNacionalidade.Items.FindByValue(dadosCandidato.Nacionalidade.ToUpper()) != null)
                {
                    ddlNacionalidade.SelectedValue = ddlNacionalidade.Items.FindByValue(dadosCandidato.Nacionalidade.ToUpper()).Value;
                }
            } 

            ddlUfNascimento.SelectedValue = !dadosCandidato.UfNascimento.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.UfNascimento : string.Empty;
            if (!ddlUfNascimento.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                ddlUfNascimento_SelectedIndexChanged(null, null);
            }

            if (!dadosCandidato.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace())
            {
                if (ddlMunicipioNascimento.Items.FindByValue(dadosCandidato.MunicipioNascimento) != null)
                {
                    ddlMunicipioNascimento.SelectedValue = ddlMunicipioNascimento.Items.FindByValue(dadosCandidato.MunicipioNascimento).Value;
                }
            } 

            txtNomeMaeCadastro.Text = !dadosCandidato.NomeMae.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.NomeMae : string.Empty;
            txtNomePai.Text = !dadosCandidato.NomePai.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.NomePai : string.Empty;
            chkNaoDeclarMaeCadastro.Checked = dadosCandidato.DeclaroAusenciaMae;
            chkNaoDeclarPai.Checked = dadosCandidato.DeclaroAusenciaPai;

            txtEmail.Text = !dadosCandidato.Email.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.Email : string.Empty;

            long resultado;
            var celular = dadosCandidato.Celular.RetirarMascaraTelefone();
            if (long.TryParse(celular, out resultado))
            {
                if (celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else if (celular.Length == 11)
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = resultado.ToString();
                }
            }

            long resultadoFixoCelular;
            var fixoCelular = dadosCandidato.FixoCelular.RetirarMascaraTelefone();

            if (long.TryParse(fixoCelular, out resultadoFixoCelular))
            {
                if (fixoCelular.Length == 10)
                {
                    txtCelularFixo.Text = string.Format("{0:(00)0000-0000}", resultadoFixoCelular);
                }
                else if (fixoCelular.Length == 11)
                {
                    txtCelularFixo.Text = string.Format("{0:(00)00000-0000}", resultadoFixoCelular);
                }
                else
                {
                    txtCelularFixo.Text = resultadoFixoCelular.ToString();

                }
            }


            Int64 resultadoCPF;

            if (Int64.TryParse(dadosCandidato.Cpf.RetirarMascaraCPF(), out resultadoCPF))
            {
                if (resultadoCPF != 0)
                {
                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", resultadoCPF);
                }
                else
                {
                    txtCPF.Text = resultadoCPF.ToString();
                }
            }

            Int64 resultadoCEP;
            var cep = !dadosCandidato.Cep.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.Cep.Replace("-", "") : string.Empty;
            if (Int64.TryParse(cep, out resultadoCEP))
            {
                if (resultadoCEP != 0)
                {
                    txtCep.Text = string.Format(@"{0:00000-000}", resultadoCEP);
                }
                else
                {
                    txtCep.Text = resultadoCEP.ToString();
                }
            }

            txtLogradouro.Text = !dadosCandidato.Endereco.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.Endereco : string.Empty;
            txtNumero.Text = !dadosCandidato.NumeroEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.NumeroEndereco : string.Empty;
            txtComplemento.Text = !dadosCandidato.ComplementoEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.ComplementoEndereco : string.Empty;
            txtEstado.Value = !dadosCandidato.UfEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.UfEndereco : string.Empty;
            hdnCodMunicipio.Value = !dadosCandidato.MunicipioEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.MunicipioEndereco : string.Empty;
            txtMunicipio.Value = !dadosCandidato.DescricaoMunicipioEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.DescricaoMunicipioEndereco : string.Empty;

            if (!hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace())
            {
                CarregaBairro(hdnCodMunicipio.Value);
                if (!dadosCandidato.Bairro.IsNullOrEmptyOrWhiteSpace())
                {
                    if (ddlBairro.Items.FindByText(dadosCandidato.Bairro) != null)
                    {
                        ddlBairro.SelectedValue = ddlBairro.Items.FindByText(dadosCandidato.Bairro).Value;
                    }
                }
            }

            if (!dadosCandidato.ModeloCertidao.IsNullOrEmptyOrWhiteSpace())
            {
                rblModeloCertidao.SelectedValue = dadosCandidato.ModeloCertidao;
                rblModeloCertidao_SelectedIndexChanged(null, null);
            }
            if (!dadosCandidato.TipoCertidao.IsNullOrEmptyOrWhiteSpace() && (dadosCandidato.TipoCertidao == "Nascimento" || dadosCandidato.TipoCertidao == "Casamento"))
            {
                rblTipoCertidao.SelectedValue = dadosCandidato.TipoCertidao;
            }

            txtNumMatriculaCertidao.Text = !dadosCandidato.MatriculaCertidao.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.MatriculaCertidao : string.Empty;
            txtDOC_CertNasc_Numero.Text = !dadosCandidato.TermoCertidao.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.TermoCertidao : string.Empty;
            txtDOC_CertNasc_Folha.Text = !dadosCandidato.FolhaCertidao.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.FolhaCertidao : string.Empty;
            txtDOC_CertNasc_Livro.Text = !dadosCandidato.LivroCertidao.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.LivroCertidao : string.Empty;

            if (dadosCandidato.NecessidadeEspecialId > 0)
            {
                if (ddlDeficiencia.Items.FindByValue(Convert.ToString(dadosCandidato.NecessidadeEspecialId)) != null)
                {
                    ddlDeficiencia.SelectedValue = ddlDeficiencia.Items.FindByValue(Convert.ToString(dadosCandidato.NecessidadeEspecialId)).Value;
                }
            }

            if (!dadosCandidato.Responsavel.IsNullOrEmptyOrWhiteSpace())
            {
                if (rblResponsavel.Items.FindByValue(dadosCandidato.Responsavel.Replace(';',' ').Trim()) != null)
                {
                    rblResponsavel.SelectedValue = dadosCandidato.Responsavel;
                    rblResponsavel_SelectedIndexChanged(null, null);
                }
            }
            txtResponsavel.Text = !dadosCandidato.ResponsavelNome.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.ResponsavelNome : string.Empty;

            Int64 resultadoCPFResponsavel;

            if (Int64.TryParse(dadosCandidato.ResponsavelCpf.RetirarMascaraCPF(), out resultadoCPFResponsavel))
            {
                if (resultadoCPFResponsavel != 0)
                {
                    txtCPFResponsavelCadastro.Value = string.Format(@"{0:000\.000\.000-00}", resultadoCPFResponsavel);
                }
                else
                {
                    txtCPFResponsavelCadastro.Value = resultadoCPFResponsavel.ToString();
                }
            }

            txtRgNumero.Text = dadosCandidato.NumeroRG;
            txtRgEmissor.Text = dadosCandidato.OrgaoRG;
            txtRgUf.Text = dadosCandidato.UFRG;
            txtRgResponsavelEmissor.Text = dadosCandidato.ResposanvelEmissorRG;
            txtRgResponsavelNumero.Text = dadosCandidato.ResponsavelNumeroRG;
            txtRgResponsavelUf.Text = dadosCandidato.ResposanvelUFRG;

            long resultadoTelResponsavel;
            var telResponsavel = dadosCandidato.ResponsavelFone.RetirarMascaraTelefone();

            if (long.TryParse(telResponsavel, out resultadoTelResponsavel))
            {
                if (telResponsavel.Length == 10)
                {
                    txtCelularResponsavel.Text = string.Format("{0:(00)0000-0000}", resultadoTelResponsavel);
                }
                else if (telResponsavel.Length == 11)
                {
                    txtCelularResponsavel.Text = string.Format("{0:(00)00000-0000}", resultadoTelResponsavel);
                }
                else
                {
                    txtCelularResponsavel.Text = resultadoTelResponsavel.ToString();
                }
            }

            pnlIrmaoForaRede.Visible = false;
            pnlIrmaoRede.Visible = false;
            pnlNaoPossuiIrmao.Visible = false;

            if (dadosCandidato.IrmaoNumeroInscricao == (int?)null && dadosCandidato.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                pnlNaoPossuiIrmao.Visible = true;
                chkNaoPossuiIrmao.Checked = true;
            }

            if (dadosCandidato.IrmaoNumeroInscricao != (int?)null)
            {
                pnlIrmaoForaRede.Visible = true;
                chkIrmaoForaRede.Checked = true;
                txtInscricaoIrmao.Text = dadosCandidato.IrmaoNumeroInscricao > 0 ? dadosCandidato.IrmaoNumeroInscricao.ToString() : string.Empty;
                txtNomeIrmaoForaRede.Text = !dadosCandidato.DadosIrmao.NomeCompl.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.DadosIrmao.NomeCompl : string.Empty;
                txtDataNascIrmaoForaRede.Text = dadosCandidato.DadosIrmao.DataNascimento.HasValue ? dadosCandidato.DadosIrmao.DataNascimento.Value.ToShortDateString() : string.Empty;
                hdnIdIrmao.Value = dadosCandidato.IrmaoIdInscricao > 0 ? dadosCandidato.IrmaoIdInscricao.ToString() : string.Empty;

                odsOpcaoIrmaoForaRede.DataBind();
            }

            if (!dadosCandidato.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                pnlIrmaoRede.Visible = true;
                chkIrmaoRede.Checked = true;
                txtMatriculaIrmao.Text = !dadosCandidato.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.IrmaoMatricula : string.Empty;
                txtNomeIrmaoRede.Text = !dadosCandidato.DadosIrmao.NomeCompl.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.DadosIrmao.NomeCompl : string.Empty;
                txtDataNascIrmaoRede.Text = dadosCandidato.DadosIrmao.DataNascimento.HasValue ? dadosCandidato.DadosIrmao.DataNascimento.Value.ToShortDateString() : string.Empty;
                txtUEIrmaoRede.Text = !dadosCandidato.DadosIrmao.EscolaAtual.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.DadosIrmao.EscolaAtual : string.Empty;
                txtCursoIrmaoRede.Text = !dadosCandidato.DadosIrmao.CursoDescricaoAtual.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.DadosIrmao.CursoDescricaoAtual : string.Empty;
                txtSerieIrmaoRede.Text = dadosCandidato.DadosIrmao.SerieAtual > 0 ? dadosCandidato.DadosIrmao.SerieAtual.ToString() : string.Empty;
                txtTurnoIrmaoRede.Text = !dadosCandidato.DadosIrmao.TurnoDescricaoAtual.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.DadosIrmao.TurnoDescricaoAtual : string.Empty;
            }           
            
        }

        protected void ddlUfNascimento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMunicipioNascimento.Items.Clear();
                if (!ddlUfNascimento.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaMunicipio(ddlUfNascimento.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
