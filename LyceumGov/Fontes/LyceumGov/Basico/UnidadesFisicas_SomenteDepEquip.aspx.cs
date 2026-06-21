using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Techne.Lyceum.RN.DTOs;
using System.Linq;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/UnidadesFisicas_SomenteDepEquip.aspx"), ControlText("UnidadesFisicas_SomenteDepEquip"), Title("Dependência e Equipamentos")]
    public partial class UnidadesFisicas_SomenteDepEquip : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    DesabilitaAbas();
                    CarregaMaterialPedagogico();
                    CarregaOrgaosColegiados();
                    CarregaAcessoInternet();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void DesabilitaAbas()
        {
            btnSalvarQtdDependencias.Visible = false;
            btnSalvarQtdEquipamentos.Visible = false;
            btnSalvarPedagogicos.Visible = false;
            btnSalvarInternet.Visible = false;
            pnlGeral.Visible = false;
        }

        protected void tseUnidadesFisicas_Changed(object sender, EventArgs e)
        {
            try
            {
                DesabilitaAbas();
                LimpaDadosPedagogico();
                LimpaDadosInternet();
                LimpaDadosCondicaoSala();

                if ((!tseUnidadesFisicas.DBValue.IsNull) && (tseUnidadesFisicas.IsValidDBValue))
                {
                    MontarDependencias(tseUnidadesFisicas.DBValue.ToString());
                    MontarEquipamentos(null);
                    MontarSalasAlternativa(tseUnidadesFisicas.DBValue.ToString());
                    MontaDadosPedagogico(tseUnidadesFisicas.DBValue.ToString());
                    MontaDadosInternet(tseUnidadesFisicas.DBValue.ToString());

                    btnSalvarQtdDependencias.Visible = true;
                    btnSalvarQtdEquipamentos.Visible = true;
                    btnSalvarPedagogicos.Visible = true;
                    btnSalvarInternet.Visible = true;
                    pnlGeral.Visible = true;

                    ControlaAcesso(btnSalvarQtdDependencias, AcaoControle.editar);
                    ControlaAcesso(btnSalvarQtdEquipamentos, AcaoControle.editar);
                    ControlaAcesso(btnSalvarPedagogicos, AcaoControle.editar);

                    txtQtdSalaAcessibilidade.Text = tseUnidadesFisicas["salaacessibilidade"].ToString();
                    txtQtdSalaClimatizada.Text = tseUnidadesFisicas["salaclimatizada"].ToString();
                    txtQtdCantinhoLeitura.Text = tseUnidadesFisicas["salaCantinhoLeitura"].ToString();

                }
                else
                {
                    lblMensagem.Text = "Favor informar uma unidade física válida.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void MontarDependencias(string unidadeFisica)
        {
            try
            {
                DataTable dtDependencia = new DataTable();
                Table dTable = new Table();
                dTable.ID = "tblDependencia";

                dtDependencia = RN.TipoDependenciaUnidadeFisica.Listar(unidadeFisica);

                int Coluna = 0;
                if (dtDependencia != null)
                {
                    for (int i = 0; i <= dtDependencia.Rows.Count; i++)
                    {
                        TableRow dTRow = new TableRow();
                        Coluna = dtDependencia.Rows.Count / 10;
                        if (dtDependencia.Rows.Count % 10 != 0)
                            Coluna += 1;

                        for (int f = 0; f < Coluna; f++)
                        {
                            if ((i + f) < dtDependencia.Rows.Count)
                            {
                                TextBox txtQtdDependencia = new TextBox();
                                txtQtdDependencia.Width = 20;
                                txtQtdDependencia.ID = "txtQt" + dtDependencia.Rows[i + f]["TIPO_DEPEND"].ToString();
                                txtQtdDependencia.Text = dtDependencia.Rows[i + f]["QUANTIDADE"].ToString();
                                txtQtdDependencia.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                txtQtdDependencia.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                txtQtdDependencia.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                Label lblTipoDependencia = new Label();
                                lblTipoDependencia.Text = dtDependencia.Rows[i + f]["NOME"].ToString();

                                HiddenField hdnDependecia = new HiddenField();
                                hdnDependecia.Value = dtDependencia.Rows[i + f]["TIPO_DEPEND"].ToString();
                                hdnDependecia.ID = "hd" + dtDependencia.Rows[i + f]["TIPO_DEPEND"].ToString();

                                if (!hdnDependecia.Value.Equals("SALA") && !hdnDependecia.Value.Equals("SALAAEE"))
                                {
                                    TableCell dTCell = new TableCell();
                                    dTCell.Controls.Add(txtQtdDependencia);
                                    dTCell.Controls.Add(lblTipoDependencia);
                                    dTCell.Controls.Add(hdnDependecia);
                                    dTRow.Controls.Add(dTCell);
                                }
                            }
                        }

                        i += Coluna - 1;

                        dTable.Controls.Add(dTRow);
                    }

                    pnlDependencia.Controls.Add(dTable);
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
                if (listaEquipamentos == null || listaEquipamentos.Count == 0)
                {
                    listaEquipamentos = rnEquipamentoUnidadeFisica.ObtemListaPor(tseUnidadesFisicas.DBValue.ToString());
                }

                Table dTable = new Table();
                dTable.ID = "tblEquipamentos";

                if (listaEquipamentos.Count > 0)
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
                                    txtQtdEquipamento.ID = "txtQtd" + Convert.ToString(item.IdEquipamento);
                                    txtQtdEquipamento.Text = Convert.ToString(item.Quantidade);
                                    txtQtdEquipamento.SkinID = "numerico";
                                    txtQtdEquipamento.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                    txtQtdEquipamento.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                    txtQtdEquipamento.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                    Label lblEquipamento = new Label();
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

        protected void btnSalvarQtdDependencias_Click(object sender, EventArgs e)
        {
            string mensagem = string.Empty;
            string text = string.Empty;
            string hidden = string.Empty;
            List<TceTipoDependenciaUnidadeFisica> listaTipoDependencia = new List<TceTipoDependenciaUnidadeFisica>();
            int salaClimatizada;
            int salaAcessibilidade;
            int salaCantinhoLeitura;
            ValidacaoDados validacao = new ValidacaoDados();

            if (tseUnidadesFisicas.DBValue.IsNull || !tseUnidadesFisicas.IsValidDBValue)
            {
                lblMensagem.Text = "Favor informar uma unidade física válida.";
                DesabilitaAbas();
                return;
            }

            string unidadeFisica = tseUnidadesFisicas.DBValue.ToString();

            try
            {
                foreach (DataRow row in RN.TipoDependenciaUnidadeFisica.Listar(unidadeFisica).Rows)
                {
                    TceTipoDependenciaUnidadeFisica dependencia = new TceTipoDependenciaUnidadeFisica();

                    text = Request.Params["ctl00$cphFormulario$pcUnidadesFisicas$txtQt" + row["Tipo_Depend"].ToString()];
                    hidden = Request.Params["ctl00$cphFormulario$pcUnidadesFisicas$hd" + row["Tipo_Depend"].ToString()];

                    dependencia.Matricula = User.Identity.Name;
                    dependencia.Quantidade = !string.IsNullOrEmpty(text) ? int.Parse(text) : 0;
                    dependencia.TipoDependencia = hidden;
                    dependencia.UnidadeFisica = unidadeFisica;

                    listaTipoDependencia.Add(dependencia);
                }

                foreach (DataRow row in RN.Dependencia.ListarQuantidadeTipoSala(unidadeFisica).Rows)
                {
                    TceTipoDependenciaUnidadeFisica dependencia = new TceTipoDependenciaUnidadeFisica();

                    text = Request.Params["ctl00$cphFormulario$pcUnidadesFisicas$txt" + row["Tipo_Depend"].ToString()];
                    hidden = Request.Params["ctl00$cphFormulario$pcUnidadesFisicas$hs" + row["Tipo_Depend"].ToString()];

                    dependencia.Matricula = User.Identity.Name;
                    dependencia.Quantidade = !string.IsNullOrEmpty(text) ? int.Parse(text) : 0;
                    dependencia.TipoDependencia = hidden;
                    dependencia.UnidadeFisica = unidadeFisica;

                    listaTipoDependencia.Add(dependencia);
                }

                var script = "";

                foreach (var tipo in listaTipoDependencia)
                {
                    var SalaAlter = new DadosSalaAlternativa
                    {
                        FACULDADE = Convert.ToString(tseUnidadesFisicas.DBValue),
                        TIPO_DEPEND = tipo.TipoDependencia,
                        quatidade = tipo.Quantidade,

                    };

                    ValidacaoDados validacaoSala = Dependencia.ValidaQuadroSalaAlternativa(SalaAlter);

                    if (!validacaoSala.Valido)
                    {
                        mensagem = "A quantidade de dependência é inferior a quantidade de registros ativos para este tipo de dependência na lista de salas alternativas.";
                        script = @"alert('" + mensagem + @"');";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                        return;
                    }
                }

                salaClimatizada = !txtQtdSalaClimatizada.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtQtdSalaClimatizada.Text) : -1;
                salaAcessibilidade = !txtQtdSalaAcessibilidade.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtQtdSalaAcessibilidade.Text) : -1;
                salaCantinhoLeitura = !txtQtdCantinhoLeitura.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtQtdCantinhoLeitura.Text) : -1;

                validacao = RN.TipoDependenciaUnidadeFisica.Validar(listaTipoDependencia, salaClimatizada, salaAcessibilidade, salaCantinhoLeitura);

                if (validacao.Valido)
                {
                    RN.TipoDependenciaUnidadeFisica.Alterar(listaTipoDependencia, salaClimatizada, salaAcessibilidade,salaCantinhoLeitura);
                    mensagem = "As quantidades de dependências foram atualizadas com sucesso.";
                    script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
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
            finally
            {
                tseUnidadesFisicas_Changed(sender, e);
            }
        }

        protected void btnSalvarQtdEquipamentos_Click(object sender, EventArgs e)
        {
            List<DadosEquipamentoUnidadeFisica> listaEquipamentos = new List<DadosEquipamentoUnidadeFisica>();
            List<DadosEquipamentoUnidadeFisica> listaMaximosAtingidos = new List<DadosEquipamentoUnidadeFisica>();
            List<DadosEquipamentoUnidadeFisica> listaVinculos = new List<DadosEquipamentoUnidadeFisica>();
            this.trMaximos.Visible = false;
            this.btnConfirmarEquipamentos.Visible = false;

            if (tseUnidadesFisicas.DBValue.IsNull || !tseUnidadesFisicas.IsValidDBValue)
            {
                lblMensagem.Text = "Favor informar uma unidade física válida.";
                DesabilitaAbas();
                return;
            }

            try
            {
                //Monta lista com equipamentos da tela
                listaEquipamentos = RetornaListaEquipamentos();
                if (listaEquipamentos == null)
                {
                    return;
                }

                //Busca equipamentos sem vinculados que ultrapassaram o maximo
                listaMaximosAtingidos = listaEquipamentos.Where(x => x.IdEquipamentoMaximoVinculado == null && x.Quantidade > x.QuantidadeMaximaSugerida).ToList();

                //Busca equipamentos com vinculos
                listaVinculos = listaEquipamentos.Where(x => x.IdEquipamentoMaximoVinculado != null).ToList();
                foreach (var equipamento in listaVinculos)
                {
                    //Para equipamentos com vincula a validação de quantidade deverá ser realizada em conjunto com o valor definido para o equipamento identificado por este campo
                    int quantidade = equipamento.Quantidade + listaVinculos.Where(x => x.IdEquipamento == equipamento.IdEquipamentoMaximoVinculado).Select(x => x.Quantidade).First();

                    if (quantidade > equipamento.QuantidadeMaximaSugerida)
                    {
                        listaMaximosAtingidos.Add(equipamento);
                    }
                }

                if (listaMaximosAtingidos.Count > 0)
                {
                    //Caso existam quantidades maximas ultrapassadas exibir popup
                    List<string> maximos = new List<string>();

                    this.btnConfirmarEquipamentos.Visible = true;
                    this.trMaximos.Visible = true;

                    foreach (DadosEquipamentoUnidadeFisica item in listaMaximosAtingidos)
                    {
                        string equipamentoMaximo = string.Format("{0} {1}?",
                            item.Quantidade.ToString(),
                            item.Descricao);

                        maximos.Add(equipamentoMaximo);
                    }

                    this.blMaximos.DataSource = maximos;
                    this.blMaximos.DataBind();

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
                else
                {
                    SalvarEquipamentos(listaEquipamentos);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            finally
            {
                MontarEquipamentos(listaEquipamentos);
                MontarDependencias(tseUnidadesFisicas.DBValue.ToString());
                MontarSalasAlternativa(tseUnidadesFisicas.DBValue.ToString());
            }
        }

        protected void btnConfirmarEquipamentos_Click(object sender, EventArgs e)
        {
            List<DadosEquipamentoUnidadeFisica> listaEquipamentos = new List<DadosEquipamentoUnidadeFisica>();
            this.pucConfirmarEquipamentos.ShowOnPageLoad = false;

            try
            {
                //Monta lista com equipamentos da tela
                listaEquipamentos = RetornaListaEquipamentos();
                SalvarEquipamentos(listaEquipamentos);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            finally
            {
                MontarEquipamentos(listaEquipamentos);
                MontarDependencias(tseUnidadesFisicas.DBValue.ToString());
                MontarSalasAlternativa(tseUnidadesFisicas.DBValue.ToString());
            }
        }

        protected List<DadosEquipamentoUnidadeFisica> RetornaListaEquipamentos()
        {
            string unidadeFisica = tseUnidadesFisicas.DBValue.ToString();
            RN.EquipamentoUnidadeFisica rnEquipamentoUnidadeFisica = new EquipamentoUnidadeFisica();
            List<DadosEquipamentoUnidadeFisica> listaInicial = rnEquipamentoUnidadeFisica.ObtemListaPor(unidadeFisica);
            List<DadosEquipamentoUnidadeFisica> listaEquipamentos = new List<DadosEquipamentoUnidadeFisica>();

            try
            {
                foreach (var item in listaInicial)
                {
                    DadosEquipamentoUnidadeFisica equipamento = new DadosEquipamentoUnidadeFisica();

                    //Busca Quantidade Escolhida pelo usuario
                    var qtde = Request.Params["ctl00$cphFormulario$pcUnidadesFisicas$txtQtd" + Convert.ToString(item.IdEquipamento)];
                    if (qtde == null)
                    {
                        qtde = "0";
                    }
                    else if (qtde.ToUpper() == "ON")
                    {
                        qtde = "1";
                    }
                    else
                    {
                        int numero;
                        if (!int.TryParse(qtde, out numero))
                        {
                            lblMensagem.Text = "As quantidades de equipamentos devem ser números inteiros.";
                            return null;
                        }
                    }

                    //Alimenta lista
                    equipamento.Matricula = User.Identity.Name;
                    equipamento.Quantidade = !string.IsNullOrEmpty(qtde) ? int.Parse(qtde) : 0;
                    equipamento.Descricao = item.Descricao;
                    equipamento.IdEquipamento = item.IdEquipamento;
                    equipamento.UnidadeFisica = unidadeFisica;
                    equipamento.QuantidadeMaximaSugerida = item.QuantidadeMaximaSugerida;
                    equipamento.IdEquipamentoMaximoVinculado = item.IdEquipamentoMaximoVinculado;

                    listaEquipamentos.Add(equipamento);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            return listaEquipamentos;
        }

        protected void SalvarEquipamentos(List<DadosEquipamentoUnidadeFisica> listaDadosEquipamento)
        {
            string mensagem = string.Empty;
            string unidadeFisica = tseUnidadesFisicas.DBValue.ToString();
            List<TceEquipamentoUnidadeFisica> listaEquipamento = new List<TceEquipamentoUnidadeFisica>();
            try
            {
                //Monta lista de entidades da tabela
                foreach (DadosEquipamentoUnidadeFisica item in listaDadosEquipamento)
                {
                    TceEquipamentoUnidadeFisica equipamento = new TceEquipamentoUnidadeFisica();
                    equipamento.IdEquipamento = item.IdEquipamento;
                    equipamento.UnidadeFisica = item.UnidadeFisica;
                    equipamento.Quantidade = item.Quantidade;
                    equipamento.Matricula = item.Matricula;

                    listaEquipamento.Add(equipamento);
                }

                RN.EquipamentoUnidadeFisica.Alterar(listaEquipamento);

                mensagem = "As quantidades de equipamentos foram atualizadas com sucesso.";

                var script = @"alert('" + mensagem + @"');";
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                lblMensagem.Text = mensagem;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message.Replace(Environment.NewLine, "<br />");
            }
        }

        private void MontarSalasAlternativa(string unidadeFisica)
        {
            try
            {
                DataTable dtSalaAlternativa = new DataTable();
                Table dTable = new Table();
                dTable.ID = "tblSalaAlternativa";
                int Coluna = 0;
                dtSalaAlternativa = RN.Dependencia.ListarQuantidadeTipoSala(unidadeFisica);

                if (dtSalaAlternativa != null)
                {
                    if (dtSalaAlternativa.Rows.Count != 0)
                    {
                        for (int i = 0; i <= dtSalaAlternativa.Rows.Count; i++)
                        {
                            TableRow dTRow = new TableRow();

                            Coluna = dtSalaAlternativa.Rows.Count / 10;
                            if (dtSalaAlternativa.Rows.Count % 10 != 0)
                                Coluna += 1;

                            for (int f = 0; f < Coluna; f++)
                            {
                                if ((i + f) < dtSalaAlternativa.Rows.Count)
                                {
                                    TextBox txtSalaAlternativa = new TextBox();
                                    txtSalaAlternativa.Width = 20;
                                    txtSalaAlternativa.ID = "txt" + dtSalaAlternativa.Rows[i + f]["Tipo_Depend"].ToString();
                                    txtSalaAlternativa.Text = dtSalaAlternativa.Rows[i + f]["Quantidade"].ToString();
                                    txtSalaAlternativa.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                    txtSalaAlternativa.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                    txtSalaAlternativa.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                    Label lblSalaAlternativa = new Label();
                                    lblSalaAlternativa.Text = dtSalaAlternativa.Rows[i + f]["Nome"].ToString();

                                    HiddenField hdnSalaAlternativa = new HiddenField();
                                    hdnSalaAlternativa.Value = dtSalaAlternativa.Rows[i + f]["Tipo_Depend"].ToString();
                                    hdnSalaAlternativa.ID = "hs" + dtSalaAlternativa.Rows[i + f]["Tipo_Depend"].ToString();

                                    if (!hdnSalaAlternativa.Value.Equals("SALA") && !hdnSalaAlternativa.Value.Equals("SALAAEE"))
                                    {
                                        TableCell dTCell = new TableCell();
                                        dTCell.Controls.Add(txtSalaAlternativa);
                                        dTCell.Controls.Add(lblSalaAlternativa);
                                        dTCell.Controls.Add(hdnSalaAlternativa);
                                        dTRow.Controls.Add(dTCell);
                                    }
                                }
                            }

                            i += Coluna - 1;

                            dTable.Controls.Add(dTRow);
                        }
                        pnlSalaAlternativa.Controls.Add(dTable);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaOrgaosColegiados()
        {
            RN.GestaoRede.OrgaoColegiado rnOrgaoColegiado = new Techne.Lyceum.RN.GestaoRede.OrgaoColegiado();

            chkOrgaoColegiado.Items.Clear();
            chkOrgaoColegiado.DataSource = rnOrgaoColegiado.ListaOrgaoColegiadoAtivo();
            chkOrgaoColegiado.DataTextField = "DESCRICAO";
            chkOrgaoColegiado.DataValueField = "ORGAOCOLEGIADOID";
            chkOrgaoColegiado.DataBind();
        }

        private void CarregaMaterialPedagogico()
        {
            RN.GestaoRede.MaterialPedagogico rnMaterialPedagogico = new Techne.Lyceum.RN.GestaoRede.MaterialPedagogico();

            chkMaterialPedagogico.Items.Clear();
            chkMaterialPedagogico.DataSource = rnMaterialPedagogico.ListaMaterialPedagogicoAtivo();
            chkMaterialPedagogico.DataTextField = "DESCRICAO";
            chkMaterialPedagogico.DataValueField = "MATERIALPEDAGOGICOID";
            chkMaterialPedagogico.DataBind();
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

        public void LimpaDadosPedagogico()
        {
            chkMaterialPedagogico.ClearSelection();
            chkOrgaoColegiado.ClearSelection();
            rblPossuiSite.ClearSelection();
            rblPossuiProjeto.ClearSelection();
            rblCumpriuProjetoPedagogico.ClearSelection();
            rblEspacoEquipamentoEntorno.ClearSelection();
            txtSiteBlog.Text = string.Empty;
            rblCompartilhaEspacoComunidade.ClearSelection();
        }

        private void LimpaDadosCondicaoSala()
        {
            txtQtdSalaAcessibilidade.Text = string.Empty;
            txtQtdSalaClimatizada.Text = string.Empty;
            txtQtdCantinhoLeitura.Text = string.Empty;
        }

        public void MontaDadosPedagogico(string censo)
        {
            RN.DTOs.UnidadeDadosPedagogicos unidadeDadosPedagogicos = new UnidadeDadosPedagogicos();
            RN.UnidadeEnsino rnUnidade = new UnidadeEnsino();

            unidadeDadosPedagogicos = rnUnidade.ObtemDadosPedagogicosPor(censo);

            foreach (var item in unidadeDadosPedagogicos.MaterialPedagogico)
            {
                chkMaterialPedagogico.Items.FindByValue(item.ToString()).Selected = true;
            }

            foreach (var item in unidadeDadosPedagogicos.OrgaoColegiado)
            {
                chkOrgaoColegiado.Items.FindByValue(item.ToString()).Selected = true;
            }

            if (!unidadeDadosPedagogicos.PossuiPaginaWeb.IsNullOrEmptyOrWhiteSpace())
            {
                rblPossuiSite.SelectedValue = unidadeDadosPedagogicos.PossuiPaginaWeb;
            }

            if (!unidadeDadosPedagogicos.PossuiProjetoPedagogico.IsNullOrEmptyOrWhiteSpace())
            {
                rblPossuiProjeto.SelectedValue = unidadeDadosPedagogicos.PossuiProjetoPedagogico;
            }

            if (!unidadeDadosPedagogicos.CumpriuProjetoPedagogico.IsNullOrEmptyOrWhiteSpace())
            {
                rblCumpriuProjetoPedagogico.SelectedValue = unidadeDadosPedagogicos.CumpriuProjetoPedagogico;
            }

            if (!unidadeDadosPedagogicos.EspacoEquipamentoEntorno.IsNullOrEmptyOrWhiteSpace())
            {
                rblEspacoEquipamentoEntorno.SelectedValue = unidadeDadosPedagogicos.EspacoEquipamentoEntorno;
            }

            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            if (!unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                rblCompartilhaEspacoComunidade.SelectedValue = unidadeDadosPedagogicos.EspacoEscolaComunidade;
            }
            txtSiteBlog.Text = !unidadeDadosPedagogicos.PaginaWeb.IsNullOrEmptyOrWhiteSpace() ? unidadeDadosPedagogicos.PaginaWeb : string.Empty;
        }

        protected void btnSalvarPedagogicos_Click(object sender, EventArgs e)
        {
            try
            {
                RN.UnidadeEnsino rnUnidade = new UnidadeEnsino();
                RN.DTOs.UnidadeDadosPedagogicos unidadeDadosPedagogicos = new UnidadeDadosPedagogicos();
                ValidacaoDados validacao = new ValidacaoDados();
                List<int> listMaterialDidatico = new List<int>();
                List<int> listOrgaoColegiado = new List<int>();
                string mensagem = string.Empty;
                string script = string.Empty;

                unidadeDadosPedagogicos.Censo = (!tseUnidadesFisicas.DBValue.IsNull && tseUnidadesFisicas.IsValidDBValue) ? tseUnidadesFisicas.DBValue.ToString() : null;
                unidadeDadosPedagogicos.PossuiPaginaWeb = !rblPossuiSite.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblPossuiSite.SelectedValue : null;
                if (!rblPossuiSite.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblPossuiSite.SelectedValue == "S")
                    {
                        unidadeDadosPedagogicos.PaginaWeb = !txtSiteBlog.Text.IsNullOrEmptyOrWhiteSpace() ? txtSiteBlog.Text.Trim() : null;
                    }
                }
                unidadeDadosPedagogicos.EspacoEquipamentoEntorno = !rblEspacoEquipamentoEntorno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblEspacoEquipamentoEntorno.SelectedValue : null;
                unidadeDadosPedagogicos.EspacoEscolaComunidade = !rblCompartilhaEspacoComunidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblCompartilhaEspacoComunidade.SelectedValue : null;

                unidadeDadosPedagogicos.PossuiProjetoPedagogico = !rblPossuiProjeto.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblPossuiProjeto.SelectedValue : null;

                if (!rblPossuiProjeto.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblPossuiProjeto.SelectedValue == "S")
                    {
                        unidadeDadosPedagogicos.CumpriuProjetoPedagogico = !rblCumpriuProjetoPedagogico.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblCumpriuProjetoPedagogico.SelectedValue : null;
                    }
                }

                foreach (ListItem item in chkMaterialPedagogico.Items)
                {
                    if (item.Selected)
                    {
                        listMaterialDidatico.Add(Convert.ToInt32(item.Value));
                    }
                }

                foreach (ListItem item in chkOrgaoColegiado.Items)
                {
                    if (item.Selected)
                    {
                        listOrgaoColegiado.Add(Convert.ToInt32(item.Value));
                    }
                }

                unidadeDadosPedagogicos.MaterialPedagogico = listMaterialDidatico;
                unidadeDadosPedagogicos.OrgaoColegiado = listOrgaoColegiado;
                unidadeDadosPedagogicos.UsuarioResponsavel = User.Identity.Name;

                validacao = rnUnidade.ValidaDadosPedagogicos(unidadeDadosPedagogicos);

                if (validacao.Valido)
                {
                    rnUnidade.SalvaDadosPedagogicos(unidadeDadosPedagogicos);

                    mensagem = "Informações Pedagógicas salvas com sucesso.";
                    script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
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
                chkAcessoInternet.Items.FindByValue(item.ToString()).Selected = true;
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
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblPossuiProjeto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlCumpriuProjeto.Visible = false;
                if (rblPossuiProjeto.SelectedValue == "S")
                {
                    pnlCumpriuProjeto.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblPossuiSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSiteBlog.Visible = false;
                lblSiteBlog.Visible = false;
                if (rblPossuiSite.SelectedValue == "S")
                {
                    txtSiteBlog.Visible = true;
                    lblSiteBlog.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkOrgaoColegiado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in chkOrgaoColegiado.Items)
                {
                    if (item.Selected && item.Value == "6") //Não há órgãos colegiados em funcionamento
                    {
                        chkOrgaoColegiado.ClearSelection();
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

        protected void chkMaterialPedagogico_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in chkMaterialPedagogico.Items)
                {
                    if (item.Selected && item.Text == "Nenhuma das opções") //Nenhuma das opções
                    {
                        chkMaterialPedagogico.ClearSelection();
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
        
        protected void btnSalvarInternet_Click(object sender, EventArgs e)
        {
            try
            {
                RN.GestaoRede.UnidadeFisicaRedeInternet rnUnidadeFisicaRedeInternet = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaRedeInternet();
                RN.DTOs.UnidadeDadosInternet unidadeDadosInternet = new UnidadeDadosInternet();
                ValidacaoDados validacao = new ValidacaoDados();
                List<int> listAcessoInternet = new List<int>();

                string mensagem = string.Empty;
                string script = string.Empty;

                unidadeDadosInternet.Censo = (!tseUnidadesFisicas.DBValue.IsNull && tseUnidadesFisicas.IsValidDBValue) ? tseUnidadesFisicas.DBValue.ToString() : null;
                unidadeDadosInternet.BandaLarga = !rblInternetBandaLarga.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblInternetBandaLarga.SelectedValue : null;

                foreach (ListItem item in chkAcessoInternet.Items)
                {
                    if (item.Selected)
                    {
                        listAcessoInternet.Add(Convert.ToInt32(item.Value));
                    }
                }

                unidadeDadosInternet.AcessoInternet = listAcessoInternet;
                unidadeDadosInternet.DispositivoEscola = (unidadeDadosInternet.BandaLarga == "S" && chkEquipamentoEscola.Checked) ? "S" : "N";
                unidadeDadosInternet.DispositivoPessoal = "N";
                unidadeDadosInternet.RedeCabo = (unidadeDadosInternet.BandaLarga == "S" && chkRedeCabo.Checked) ? "S" : "N";
                unidadeDadosInternet.RedeWireless = "N";
                unidadeDadosInternet.SemRedeComputador = "N";


                unidadeDadosInternet.UsuarioResponsavel = User.Identity.Name;

                validacao = rnUnidadeFisicaRedeInternet.ValidaDadosInternet(unidadeDadosInternet);

                if (validacao.Valido)
                {
                    rnUnidadeFisicaRedeInternet.SalvaDadosInternet(unidadeDadosInternet);

                    mensagem = "Informações de Internet salvas com sucesso.";
                    script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
        }


    }
}
