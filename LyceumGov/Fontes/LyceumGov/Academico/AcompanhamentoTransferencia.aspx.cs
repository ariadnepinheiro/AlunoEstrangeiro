namespace Techne.Lyceum.Net.Academico
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxGridView;
    using DevExpress.Web.ASPxTabControl;
    using Seeduc.Infra.Helpers;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Web;
    using System.Linq;

    [NavUrl("~/Academico/AcompanhamentoTransferencia.aspx"), ControlText("Acompanhamento Transferencia Aluno"), Title("Acompanhamento Transferência Aluno")]
    public partial class AcompanhamentoTransferencia : TPage
    {
        public object ListarTransferenciasDeDestino(object unidade_ens, object status)
        {
            if (unidade_ens != null)
            {
                return Transferencia.ListarTransferenciasDeDestino(unidade_ens.ToString(), status.ToString());
            }

            return null;
        }

        public object ListarTransferenciasDeOrigem(object unidade_ens, object status)
        {
            if (unidade_ens != null)
            {
                return Transferencia.ListarTransferenciasDeOrigem(unidade_ens.ToString(), status.ToString());
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdAcompanhamento, string.Empty);
            TituloGrid(this.grdSolicitacao, string.Empty);
            ControlarVisibilidadeControle();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemPopupEletiva.Text = string.Empty;

                if (!IsPostBack)
                {
                    cmbStatusAcomp.Items.Clear();
                    cmbStatusAcomp.DataSource = Transferencia.ListarStatusDeTransferencias();
                    cmbStatusAcomp.DataBind();

                    cmbStatusSolic.Items.Clear();
                    cmbStatusSolic.DataSource = Transferencia.ListarStatusDeTransferencias();
                    cmbStatusSolic.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void ControlarVisibilidadeControle()
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
        }
        protected void pcTransferencia_TabClick(object source, TabControlCancelEventArgs e)
        {
            Response.Redirect("SolicitacaoTransferenciaAluno.aspx");
        }

        protected void tseMunicipio_Changed(object sender, Controls.ChangedEventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            tseUnidadeResponsavel.ResetValue();

            var sessao = SessaoUsuario.GetSessaoUsuario();

            if (sessao != null)
            {
                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);

                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    sessao.Municipio = string.Empty;
                    sessao.Escola = string.Empty;
                }
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Controls.ChangedEventArgs args)
        {
            var sessao = SessaoUsuario.GetSessaoUsuario();

            if (!tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        tseMunicipio.Value = tseUnidadeResponsavel["municipio"];
                    }

                    lblMensagem.Text = string.Empty;
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }

                    lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                }
            }
            else
            {
                if (sessao != null)
                {
                    sessao.Escola = string.Empty;
                    sessao.Municipio = string.Empty;
                    sessao.Coordenadoria = string.Empty;
                }

                lblMensagem.Text = "Favor consultar uma unidade de ensino.";
            }
        }

        protected void grdSolicitacao_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "MATRICULA_ALUNO")
            {
                var aluno = e.GetListSourceFieldValue("ALUNO");
                var nomealuno = e.GetListSourceFieldValue("NOME_ALUNO");

                e.Value = aluno + " - " + nomealuno;
            }

            if (e.Column.FieldName == "CENSO_ESCOLA")
            {
                var censo = e.GetListSourceFieldValue("CENSO");
                var nomeescola = e.GetListSourceFieldValue("NOME_ESCOLA");

                e.Value = censo + " - " + nomeescola;
            }
        }

        protected void grdAcompanhamento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdAcompanhamento.Visible || this.grdAcompanhamento.VisibleRowCount == 0)
            {
                return;
            }

            var status = (string)grdAcompanhamento.GetRowValues(e.VisibleIndex, "STATUS");
            var hfObservacao = DevExpressHelper.GetControl<HiddenField>(this.grdAcompanhamento, e.VisibleIndex, "OBSERVACAO", "hfObservacao");
            var cmbObservacao = DevExpressHelper.GetControl<DropDownList>(this.grdAcompanhamento, e.VisibleIndex, "OBSERVACAO", "cmbObservacao");
            var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdAcompanhamento, e.VisibleIndex, "JUSTIFICATIVA", "txtJustificativa");

            if (string.IsNullOrEmpty(status))
            {
                return;
            }

            var rbAceitar = DevExpressHelper.GetControl<RadioButton>(this.grdAcompanhamento, e.VisibleIndex, "ANDAMENTO", "rbAceitar");
            var rbRecusar = DevExpressHelper.GetControl<RadioButton>(this.grdAcompanhamento, e.VisibleIndex, "ANDAMENTO", "rbRecusar");

            if (rbAceitar == null
                || rbRecusar == null)
            {
                return;
            }

            rbAceitar.Enabled = false;
            rbRecusar.Enabled = false;
            cmbObservacao.Enabled = false;
            txtJustificativa.Enabled = false;
            txtJustificativa.BackColor = Color.Gainsboro;
            txtJustificativa.TabIndex = -1;

            if (status == Transferencia.Pendente)
            {
                rbAceitar.Enabled = true;
                rbRecusar.Enabled = true;
            }

            if (status == Transferencia.Aceita)
            {
                rbAceitar.Checked = true;
                rbRecusar.Checked = false;
            }

            if (status == Transferencia.Recusada)
            {
                rbAceitar.Checked = false;
                rbRecusar.Checked = true;
            }

            rbAceitar.InputAttributes.Add("cmbObservacao", cmbObservacao.ClientID);
            rbAceitar.InputAttributes.Add("txtJustificativa", txtJustificativa.ClientID);

            rbRecusar.InputAttributes.Add("cmbObservacao", cmbObservacao.ClientID);
            rbRecusar.InputAttributes.Add("txtJustificativa", txtJustificativa.ClientID);

            cmbObservacao.Attributes.Add("txtJustificativa", txtJustificativa.ClientID);

            cmbObservacao.ClearSelection();

            var observacao = string.IsNullOrEmpty(hfObservacao.Value) ? "Selecione" : hfObservacao.Value;

            cmbObservacao.SelectedValue = observacao;
        }

        protected void grdAcompanhamento_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "MATRICULA_ALUNO")
            {
                var aluno = e.GetListSourceFieldValue("ALUNO");
                var nomealuno = e.GetListSourceFieldValue("NOME_ALUNO");

                e.Value = aluno + " - " + nomealuno;
            }

            if (e.Column.FieldName == "CENSO_ESCOLA")
            {
                var censo = e.GetListSourceFieldValue("CENSO");
                var nomeescola = e.GetListSourceFieldValue("NOME_ESCOLA");

                e.Value = censo + " - " + nomeescola;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            var recusados = new List<string>();
            var aceitos = new List<string>();
            var escola = (string)null;
            lblMensagemPopupEletiva.Text = string.Empty;

            try
            {
                for (var rowIndex = 0; rowIndex < this.grdAcompanhamento.VisibleRowCount; rowIndex++)
                {
                    var id = (int)this.grdAcompanhamento.GetRowValues(rowIndex, "ID_TRANSFERENCIA");
                    var status = (string)this.grdAcompanhamento.GetRowValues(rowIndex, "STATUS");
                    var nomeAluno = this.grdAcompanhamento.GetRowValues(rowIndex, "MATRICULA_ALUNO").ToString();
                    var nomeEscola = this.grdAcompanhamento.GetRowValues(rowIndex, "CENSO_ESCOLA").ToString();
                    var rbAceitar = DevExpressHelper.GetControl<RadioButton>(this.grdAcompanhamento, rowIndex, "ANDAMENTO", "rbAceitar");
                    var rbRecusar = DevExpressHelper.GetControl<RadioButton>(this.grdAcompanhamento, rowIndex, "ANDAMENTO", "rbRecusar");

                    if (id > 0
                        && status.Equals("Pendente"))
                    {
                        if (rbRecusar.Checked)
                        {
                            escola = escola ?? nomeEscola;

                            recusados.Add(nomeAluno);
                        }

                        if (rbAceitar.Checked)
                        {
                            escola = escola ?? nomeEscola;

                            aceitos.Add(nomeAluno);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            this.trAceitos.Visible = false;
            this.trRecusados.Visible = false;
            this.btnConfirmar.Visible = false;

            if (aceitos.Count == 0
                && recusados.Count == 0)
            {
                this.lblMensagemPopup.Text = "Favor selecionar pelo menos um aluno para aceitar ou recusar a transferência!";

                this.btnCancelar.Text = "Fechar";

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);

                return;
            }

            var mensagens = new List<string>();
            string mensagem = string.Empty;

            mensagens.Add("O(s) aluno(s) abaixo possui enturmação em turmas eletivas. Caso confirme a transferência a enturmação nas turmas eletivas serão excluídas.");                           

            foreach (var item in aceitos)
            {
                string aluno = item.ToString().Substring(0, 15);

                //Verifica se o aluno possui enturmação em eletivas
                if (rnMatricula.PossuiMatriculaEletivaAtiva(aluno))
                {
                    mensagens.Add(item.ToString());
                }
            }

            if (mensagens.Count > 1)
            {
                lblMensagemPopupEletiva.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
            }

            this.btnConfirmar.Visible = true;
            this.btnCancelar.Text = "Cancelar";

            this.lblMensagemPopup.Text = string.Format(
                "As transferências para a unidade {0} serão processadas conforme abaixo:",
                escola);

            if (aceitos.Count > 0)
            {
                this.trAceitos.Visible = true;

                this.blAceitos.DataSource = aceitos;
                this.blAceitos.DataBind();
            }

            if (recusados.Count > 0)
            {
                this.trRecusados.Visible = true;

                this.blRecusados.DataSource = recusados;
                this.blRecusados.DataBind();
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
        }

        public void Delete(object ID_TRANSFERENCIA)
        {
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            RN.Transferencia rnTransferencia = new RN.Transferencia();
            this.pucConfirmar.ShowOnPageLoad = false;

            try
            {
                var transferencias = new List<TceTransferencia>();

                //Verifica situação atual das transferencias que serão recusados
                foreach (var item in blRecusados.Items)
                {
                    string aluno = item.ToString().Substring(0, 15);
                    if (!Transferencia.ExistePendenciaTranf(aluno))
                    {
                        this.lblMensagem.Text = "A(s) solicitação(s) de transferência(s) não se encontram mais pendentes, favor verificar novamente as modificaçoes necessarias.";
                        CarregaDadosGrid();
                        return;
                    }
                }

                //Verifica situação atual das transferencias que serão aceitas
                foreach (var item in blAceitos.Items)
                {
                    string aluno = item.ToString().Substring(0, 15);
                    if (!Transferencia.ExistePendenciaTranf(aluno))
                    {
                        this.lblMensagem.Text = "A(s) solicitação(s) de transferência(s) não se encontram mais pendentes, favor verificar novamente as modificaçoes necessarias.";
                        CarregaDadosGrid();
                        return;
                    }
                }
                
                for (var rowIndex = 0; rowIndex < this.grdAcompanhamento.VisibleRowCount; rowIndex++)
                {
                    var id = this.grdAcompanhamento.GetRowValues(rowIndex, "ID_TRANSFERENCIA").ToString();
                    var status = this.grdAcompanhamento.GetRowValues(rowIndex, "STATUS").ToString();
                    var aluno = this.grdAcompanhamento.GetRowValues(rowIndex, "ALUNO").ToString();
                    var motivo = this.grdAcompanhamento.GetRowValues(rowIndex, "MOTIVO").ToString();

                    var rbAceitar = DevExpressHelper.GetControl<RadioButton>(this.grdAcompanhamento, rowIndex, "ANDAMENTO", "rbAceitar");
                    var rbRecusar = DevExpressHelper.GetControl<RadioButton>(this.grdAcompanhamento, rowIndex, "ANDAMENTO", "rbRecusar");
                    var cmbObservacao = DevExpressHelper.GetControl<DropDownList>(this.grdAcompanhamento, rowIndex, "OBSERVACAO", "cmbObservacao");
                    var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdAcompanhamento, rowIndex, "JUSTIFICATIVA", "txtJustificativa");

                    if (id != null
                        && status.Equals("Pendente"))
                    {
                        if (rbRecusar != null)
                        {
                            if (rbRecusar.Checked)
                            {
                                var transferencia = new TceTransferencia
                                {
                                    IdTransferencia = int.Parse(id),
                                    Status = Transferencia.Recusada,
                                    MatriculaAndamento = User.Identity.Name,
                                    Observacao = cmbObservacao.SelectedValue == "Selecione" ? null : cmbObservacao.SelectedValue,
                                    Justificativa = string.IsNullOrEmpty(txtJustificativa.Text) ? null : txtJustificativa.Text
                                };

                                var validacao = Transferencia.ValidarRecusa(transferencia);

                                if (!validacao.Valido)
                                {
                                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                                    {
                                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                                    }

                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "atualizarGrid", "atualizarGrid();", true);
                                    return;
                                }

                                transferencias.Add(transferencia);
                            }
                        }

                        if (rbAceitar != null)
                        {
                            if (rbAceitar.Checked)
                            {
                                var transferencia = new TceTransferencia
                                {
                                    IdTransferencia = int.Parse(id),
                                    Status = Transferencia.Aceita,
                                    MatriculaAndamento = User.Identity.Name,
                                    Aluno = aluno,
                                    Motivo = motivo
                                };

                                var validacao = rnTransferencia.ValidarAceite(transferencia);

                                if (!validacao.Valido)
                                {
                                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                                    {
                                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                                    }

                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "atualizarGrid", "atualizarGrid();", true);
                                    return;
                                }

                                transferencias.Add(transferencia);
                            }
                        }
                    }
                }

                if (transferencias.Count > 0)
                {
                    //Realiza as transferências
                    rnTransferencia.ProcessarTransferencias(transferencias);

                    var mensagem = " Caro Diretor, transferências Aceitas/Recusadas com sucesso. \\n Caso o curso/série/turno do(s) aluno(s) transferido(s) seja(m) participante do Matrícula Fácil a vaga ficará reservada por 24 horas.";

                    var script = @"alert('" + mensagem + @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                    this.lblMensagem.Text = mensagem.Replace("\\n", "<br />");
                }
                else
                {
                    this.lblMensagem.Text = "Não existem transferências pendentes para a escola.";
                }

                CarregaDadosGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosGrid()
        {
            odsAcompanhamento.Select();
            odsAcompanhamento.DataBind();
            grdAcompanhamento.DataBind();

            odsSolicitacao.Select();
            odsSolicitacao.DataBind();
            grdSolicitacao.DataBind();
        }

        protected void odsSolicitacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_TRANSFERENCIA"].ToString();
            var validacao = Transferencia.ValidarRemover(Convert.ToInt32(id));

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }

            Transferencia.Remover(int.Parse(id));
        }

        protected string verificaradio(object resposta)
        {
            if (resposta is DBNull)
            {
                return string.Empty;
            }

            return (bool)resposta ? "1" : "0";
        }

        protected void grdSolicitacao_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var status = (string)grdSolicitacao.GetRowValues(e.VisibleIndex, "STATUS");

            if (!string.IsNullOrEmpty(status)
                && status != Transferencia.Pendente)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }
        }
    }
}