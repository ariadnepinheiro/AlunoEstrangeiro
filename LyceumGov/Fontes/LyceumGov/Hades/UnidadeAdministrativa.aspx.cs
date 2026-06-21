using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Controls;
using Techne.Web;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.RN;
using Techne.HadesLyc.CR;
using System.Web.SessionState;
using System.Collections;
using Techne.Lyceum.CR;
using System.IO;

using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Hades
{
    [
     NavUrl("~/Hades/UnidadeAdministrativa.aspx"),
      ControlText("UnidadeAdministrativa"),
      Title("Unidades Administrativas"),
    ]

    public partial class Setores : TPage
    {

        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        public object Lista(object setor)
        {
            RN.GestaoRede.HistoricoUnidadeAdministrativa rnHistorico = new Techne.Lyceum.RN.GestaoRede.HistoricoUnidadeAdministrativa();

            if (setor != null)
                return rnHistorico.ListaPor(setor.ToString());


            return null;

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdContato, "Contatos");
            TituloGrid(grdHistoricoUA, string.Empty);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            CarregarGridContato();
        }

        private void CarregarGridContato()
        {
            if (!string.IsNullOrEmpty(lblSetor.Text.Trim()))
            {
                QueryTable qt = RN.Setores.ConsultarContato(lblSetor.Text.Trim());
                grdContato.DataSource = qt;
                grdContato.DataBind();
            }
        }

        private void CarregaTipo()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlTipo.Items.Clear();
            ddlTipo.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.TipoUA, RN.Basico.QueryListaTipoUA);
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, item);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _tipoOperacao = TipoOperacao.Inicial;
                pcSetor.Visible = false;

                ImageButton[] controles = new ImageButton[] { btnNovo };
                ControlarVisibilidadeControle(controles);
                CarregaTipo();
            }

            lblRetorno.Text = string.Empty;

        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion

        #region Métodos de Obter e Carregar Dados do Aluno e da Pessoa
        private void ObterDadosSetor(Hd_setor dtSetor)
        {
            Hd_setor.Row dadosSetor = dtSetor.NewRow();

            //Verifica se já tem codigo, caso não tenha foi informada uma UA Antiga para ser utilizada como codigo
            dadosSetor.Setor = lblSetor.Text.IsNullOrEmptyOrWhiteSpace() ? txtAntigaUA.Text : lblSetor.Text;
            dadosSetor.Nome = txtNome.Text;
            dadosSetor.Setorpai = null;
            dadosSetor.Tipo_setor = ddlTipo.SelectedValue;
            dadosSetor.Bairro = txtBairro.Text;
            dadosSetor.Logradouro = txtEndereco.Value;
            dadosSetor.Observacao = txtObs.Text;

            if (tseMunicipio.IsValidDBValue)
                dadosSetor.Municipio = Convert.ToString(tseMunicipio.DBValue);

            if (!string.IsNullOrEmpty(txtNumero.Text))
                dadosSetor.Numerolog = Convert.ToDecimal(txtNumero.Text);
            dadosSetor.Complemento = txtCompl.Text;
            dadosSetor.Fone = txtTelefone.Text;
            dadosSetor.Fax = txtFax.Text;
            if (!string.IsNullOrEmpty(rblAtivo.Text))
                dadosSetor.Ativo = Convert.ToString(rblAtivo.Text);

            dadosSetor.Dtini = dtIni.Date;

            if (!string.IsNullOrEmpty(dtFim.Text))
                dadosSetor.Dtfim = dtFim.Date;

            dadosSetor.Cep = txtCep.Text;
            dadosSetor.Pais = RN.Endereco.ObterCodigoPais("BRASIL");

            if (!string.IsNullOrEmpty(txtCNPJ.Text))
                dadosSetor.Cnpj = txtCNPJ.Text;

            if (!txtNovaUA.Text.IsNullOrEmptyOrWhiteSpace())
            {
                dadosSetor.novosetor = txtNovaUA.Text.Trim();
            }

            //dadosSetor.tipoComando = "Insert";

            dtSetor.Rows.Add(dadosSetor);
        }

        private void CarregaDadosSetor(Hd_setor.Row dadosSetor)
        {
            lblSetor.Text = dadosSetor.Setor;

            //Verifica se setor é uma UA antiga (uas reais tem 6 digitos, são numeros e menores que 200000
            long setor;
            if (long.TryParse(dadosSetor.Setor, out setor))
            {
                if (setor < 200000)
                {
                    txtAntigaUA.Text = dadosSetor.Setor;
                }
            }

            //Campo Unidade Administrativa, caso exista nova setor deve ser exibido o novo setor, caso não exista deve ser exibido o setor
            txtNovaUA.Text = dadosSetor.novosetor.IsNullOrEmptyOrWhiteSpace() ? dadosSetor.Setor : dadosSetor.novosetor;
            txtNome.Text = dadosSetor.Nome;
            ddlTipo.SelectedValue = dadosSetor.Tipo_setor;
            txtNumero.Text = dadosSetor.Numerolog.ToString();
            txtCompl.Text = dadosSetor.Complemento;
            txtObs.Text = dadosSetor.Observacao;

            int resul;
            if (int.TryParse(dadosSetor.Fone, out resul))
                txtTelefone.Text = string.Format("{0:(00)0000-0000}", resul);
            else
                txtTelefone.Text = dadosSetor.Fone;

            resul = 0;

            if (int.TryParse(dadosSetor.Fax, out resul))
                txtFax.Text = string.Format("{0:(00)0000-0000}", resul);
            else
                txtFax.Text = dadosSetor.Fax;

            if (dadosSetor.Dtini.HasValue)
                dtIni.Date = dadosSetor.Dtini.Value;
            if (dadosSetor.Dtfim.HasValue)
                dtFim.Date = dadosSetor.Dtfim.Value;
            rblAtivo.Text = dadosSetor.Ativo;

            Int64 result;
            if (Int64.TryParse(dadosSetor.Cnpj, out result))
            {
                txtCNPJ.Text = string.Format(@"{0:00\.000\.000\/0000-00}", result);
            }
            else
                txtCNPJ.Text = dadosSetor.Cnpj;


            if (!string.IsNullOrEmpty(dadosSetor.Municipio))
            {
                tseMunicipio.DBValue = dadosSetor.Municipio;

                if (tseMunicipio.IsValidDBValue)
                    txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
            if (!string.IsNullOrEmpty(dadosSetor.Cep))
                txtCep.Text = dadosSetor.Cep;

            DadosEndereco dadosEndereco = RN.Endereco.ConsultarDescricaoEndereco(dadosSetor.Bairro, dadosSetor.Municipio, dadosSetor.Logradouro);
            if (dadosEndereco != null)
            {
                if (!string.IsNullOrEmpty(dadosEndereco.DescricaoLogradouro))
                    txtEndereco.Value = dadosEndereco.DescricaoLogradouro;

                if (!string.IsNullOrEmpty(dadosEndereco.DescricaoBairro))
                    txtBairro.Text = dadosEndereco.DescricaoBairro;
            }
        }

        #endregion

        #region Eventos Botões
        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            pcSetor.TabPages[1].Enabled = false;
            tseUnidadeAdministrativa.ResetValue();
            LimpaTela();
            LimparEndereco();
            pcSetor.Visible = true;
            HabilitaCampos();
            ImageButton[] controles = new ImageButton[] { btnSalvar, btnCancel };
            ControlarVisibilidadeControle(controles);
            tseUnidadeAdministrativa.Enabled = false;
            rblAtivo.Items[0].Selected = true;
            CarregaTipo();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            HabilitaCampos();
            tseUnidadeAdministrativa.Enabled = false;
            pcSetor.TabPages[1].Enabled = true;
            txtAntigaUA.ReadOnly = true;
            ImageButton[] controles = new ImageButton[] { btnSalvar, btnCancel };
            ControlarVisibilidadeControle(controles);
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Inicial;

            pcSetor.Visible = false;
            tseUnidadeAdministrativa.Enabled = true;
            tseUnidadeAdministrativa.ResetValue();
            LimpaTela();
            LimparEndereco();

            //DesabilitaCampos();
            ImageButton[] controles = new ImageButton[] { btnNovo };
            ControlarVisibilidadeControle(controles);
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Excluir;

            lblRetorno.Text = "Confirma a remoção?";
            ImageButton[] controles = new ImageButton[] { btnConfirmar, btnCancel };
            ControlarVisibilidadeControle(controles);
            tseMunicipio.Mode = ControlMode.View;
            tsCEP.ShowButton = false;
        }

        protected void btnConfirmar_Click(object sender, ImageClickEventArgs e)
        {
            RetValue retorno = RN.Setores.Excluir(lblSetor.Text.ToString());

            _tipoOperacao = TipoOperacao.Inicial;

            if (!retorno.Ok)
            {
                lblRetorno.Text = retorno.Errors.ToString();
                ImageButton[] controles = new ImageButton[] { btnCancel };
                ControlarVisibilidadeControle(controles);
            }
            else
            {
                lblRetorno.Text = retorno.Message;
                LimpaTela();
                LimparEndereco();
                pcSetor.Visible = false;
                ImageButton[] controles = new ImageButton[] { btnNovo };
                ControlarVisibilidadeControle(controles);
                tseUnidadeAdministrativa.ResetValue();
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            Hd_setor dtSetor = new Hd_setor();
            RN.Setores rnSetor = new Techne.Lyceum.RN.Setores();
            bool erro = false;
            ObterDadosSetor(dtSetor);

            //verifica o endereço usado
            RN.DadosEndereco dadosEndereco = ControlarEndereco();
            if (dadosEndereco.Error == null || dadosEndereco.Error.Count == 0)
            {
                dtSetor.Rows[0].Municipio = dadosEndereco.Municipio;
                if (!string.IsNullOrEmpty(dadosEndereco.Bairro))
                    dtSetor.Rows[0].Bairro = dadosEndereco.Bairro;

                if (!string.IsNullOrEmpty(dadosEndereco.Logradouro))
                    dtSetor.Rows[0].Logradouro = dadosEndereco.Logradouro;

                if (!string.IsNullOrEmpty(dadosEndereco.TrechoLogradouro))
                    dtSetor.Rows[0].Trecho_logr = dadosEndereco.TrechoLogradouro;
            }

            //Verifica se a UA nova já existe
            if (rnSetor.ExisteNovaUaPor(dtSetor.Rows[0].novosetor, dtSetor.Rows[0].Setor))
            {
                lblRetorno.Text = "Já existe essa unidade Administrativa cadastrada.";
                erro = true;
            }

            //Verifica se é uma alteração
            if (_tipoOperacao == TipoOperacao.Alterar)
            {
                if (lblSetor.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    lblRetorno.Text += " Codigo da Unidade Não encontrado.";
                    erro = true;
                }
            }
            else
            {
                //Caso se um cadastro 

                //Verifica se foi digitado valor de antiga ua
                if (!dtSetor.Rows[0].Setor.IsNullOrEmptyOrWhiteSpace())
                {
                    //Verifica se é realmente um valor de Antiga UA (regras implemtadas para visualizar coluna Antiga UA na view e trigger)
                    long setor;
                    if (!long.TryParse(dtSetor.Rows[0].Setor, out setor))
                    {
                        lblRetorno.Text += " U.A. Antiga deve ser composta apenas de números.";
                        erro = true;
                    }
                    else if (setor >= 200000)
                    {
                        lblRetorno.Text += " U.A. Antiga deve ser menor que 200000.";
                        erro = true;
                    }

                    //Verifica se a Antiga UA já foi cadastrada
                    if (!erro && RN.Setores.ExisteSetor(dtSetor.Rows[0].Setor))
                    {
                        lblRetorno.Text += " Já existe uma unidade Administrativa com essa U.A. Antiga.";
                        erro = true;
                    }
                }
            }

            if (erro)
            {
                ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                ControlarVisibilidadeControle(controles);
            }
            else
            {
                if (_tipoOperacao != TipoOperacao.Alterar)
                {
                    tseUnidadeAdministrativa.Enabled = true;
                    //Verificação das datas
                    string verifica_data = "ok";
                    if (!string.IsNullOrEmpty(dtFim.Text) && Convert.ToDateTime(dtFim.Value) < Convert.ToDateTime(dtIni.Value))
                        verifica_data = "no";
                    else
                        verifica_data = "ok";

                    //incluir setor selecionado
                    dtSetor.Rows[0].Setorpai = null;
                    if (verifica_data == "ok")
                    {
                        RetValue retorno = RN.Setores.Incluir(dtSetor);
                        if (!retorno.Ok)
                        {
                            lblRetorno.Text = retorno.Errors.ToString();
                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);
                            pcSetor.TabPages[1].Enabled = false;
                        }
                        else
                        {
                            lblRetorno.Text = retorno.Message;
                            lblSetor.Text = dtSetor.Rows[0].Setor;
                            tseUnidadeAdministrativa.Value = dtSetor.Rows[0].novosetor;
                            grdContato.DataBind();
                            DesabilitaCampos();
                            ImageButton[] controles = new ImageButton[] { btnEditar, btnNovo, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                            pcSetor.TabPages[1].Enabled = true;
                        }
                    }
                    else
                    {
                        lblRetorno.Text = "Data Fim não pode ser menor que Data Início";
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                    }
                }
                else
                {
                    //Verificação das datas
                    string verifica_data = "ok";
                    if (!string.IsNullOrEmpty(dtFim.Text.ToString()) && Convert.ToDateTime(dtFim.Value) < Convert.ToDateTime(dtIni.Value))
                        verifica_data = "no";
                    else
                        verifica_data = "ok";

                    if (verifica_data == "ok")
                    {
                        RetValue retorno = RN.Setores.Alterar(dtSetor);
                        if (!retorno.Ok)
                        {
                            lblRetorno.Text = retorno.Errors.ToString();
                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);
                            pcSetor.TabPages[1].Enabled = false;
                        }
                        else
                        {
                            lblRetorno.Text = retorno.Message;
                            grdContato.DataBind();
                            DesabilitaCampos();
                            ImageButton[] controles = new ImageButton[] { btnEditar, btnNovo, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                            tseUnidadeAdministrativa.Enabled = true;
                            pcSetor.TabPages[1].Enabled = true;
                        }
                    }
                    else
                    {
                        lblRetorno.Text = "Data Fim não pode ser menor que Data Início";
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                    }
                }
            }
        }

        protected void cmbPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEndereco();
        }
        #endregion

        private RN.DadosEndereco ControlarEndereco()
        {
            RN.RetValue retorno = null;

            RN.DadosEndereco dadosEndereco = new Techne.Lyceum.RN.DadosEndereco();
            dadosEndereco.DescricaoBairro = txtBairro.Text;
            dadosEndereco.Cep = txtCep.Text.RetirarCaracteres();
            dadosEndereco.DescricaoLogradouro = txtEndereco.Value;
            dadosEndereco.DescricaoPais = "BRASIL"; //esta tela deve considerar sempre o BRASIL como valor da descricao do pais
            dadosEndereco.UF = txtEstado.Value;

            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                {
                    dadosEndereco.Municipio = Convert.ToString(tseMunicipio.DBValue);
                    dadosEndereco.DescricaoMunicipio = Convert.ToString(tseMunicipio["nome"]);
                }
            }

            DadosEndereco dadosEnderecoConsulta = RN.Endereco.ObterDadosEndereco(dadosEndereco.Cep, dadosEndereco.DescricaoMunicipio, dadosEndereco.DescricaoBairro, dadosEndereco.DescricaoLogradouro, dadosEndereco.UF);

            if (dadosEnderecoConsulta != null)
                return dadosEnderecoConsulta;

            retorno = RN.Endereco.ControlarEndereco(dadosEndereco);
            if (retorno != null)
            {
                if (!retorno.Ok)
                    lblRetorno.Text = retorno.Errors.ToString();
            }

            return dadosEndereco;
        }

        private void HabilitaCampos()
        {
            txtAntigaUA.ReadOnly = false;
            txtNome.ReadOnly = false;
            ddlTipo.Enabled = true;
            txtNumero.ReadOnly = false;
            txtCompl.ReadOnly = false;
            txtCep.ReadOnly = false;
            txtTelefone.ReadOnly = false;
            txtFax.ReadOnly = false;
            dtIni.Enabled = true;
            dtFim.Enabled = true;
            rblAtivo.Enabled = true;
            txtObs.ReadOnly = false;
            txtCNPJ.ReadOnly = false;

            tsCEP.ShowButton = true;
            txtNumero.ReadOnly = false;
            txtBairro.ReadOnly = false;

            txtEndereco.Attributes.Remove("readonly");
            tseMunicipio.Mode = ControlMode.Edit;

            txtNovaUA.ReadOnly = false;
        }

        private void DesabilitaCampos()
        {
            txtAntigaUA.ReadOnly = true;
            txtNovaUA.ReadOnly = true;
            txtNome.ReadOnly = true;
            ddlTipo.Enabled = false;
            txtCompl.ReadOnly = true;
            txtTelefone.ReadOnly = true;
            txtFax.ReadOnly = true;
            dtIni.Enabled = false;
            dtFim.Enabled = false;
            rblAtivo.Enabled = false;
            txtCNPJ.ReadOnly = true;
            txtObs.ReadOnly = true;

            txtCep.ReadOnly = true;
            txtNumero.ReadOnly = true;

            tsCEP.ShowButton = false;
            txtEndereco.Attributes["readonly"] = "readonly";
            txtBairro.ReadOnly = true;

            tseMunicipio.Mode = ControlMode.View;
        }

        private void LimpaTela()
        {
            lblSetor.Text = string.Empty;
            txtAntigaUA.Text = string.Empty;
            txtNovaUA.Text = string.Empty;
            txtNome.Text = string.Empty;
            ddlTipo.ClearSelection();
            txtCNPJ.Text = string.Empty;
            txtEndereco.Value = string.Empty;
            txtNumero.Text = string.Empty;
            txtCompl.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtObs.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtBairro.Text = string.Empty;
            dtIni.Date = DateTime.Now;
            dtFim.Text = string.Empty;
            rblAtivo.SelectedIndex = -1;
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            btnConfirmar.Visible = false;
        }

        private void LimparEndereco()
        {
            txtEndereco.Value = string.Empty;
            txtCep.Text = string.Empty;
            txtEndereco.Value = string.Empty;
            txtNumero.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtEstado.Value = string.Empty;
            tseMunicipio.ResetValue();
        }

        protected void tseUnidadeAdministrativa_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (tseUnidadeAdministrativa.IsValidDBValue && !tseUnidadeAdministrativa.DBValue.IsNull)
            {
                //exibe os dados do nó selecionado
                Hd_setor dtSetor = new Hd_setor();
                Hd_setor.Row dadosSetor = dtSetor.NewRow();
                dadosSetor = RN.Setores.Consultar(tseUnidadeAdministrativa["setor"].ToString());
                pcSetor.Visible = true;
                LimpaTela();
                LimparEndereco();
                CarregaDadosSetor(dadosSetor);
                DesabilitaCampos();
                ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                ControlarVisibilidadeControle(controles);
                pcSetor.TabPages[1].Enabled = true;
            }
            else
            {
                pcSetor.Visible = false;
                ImageButton[] controles = new ImageButton[] { btnNovo };
                ControlarVisibilidadeControle(controles);
            }
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                    txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
        }

        protected void grdContato_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "FONE" && e.Value != null)
            {
                decimal fone = 0;
                if (decimal.TryParse(e.Value.ToString().Replace(" ", ""), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = "";
                }
            }
            if (e.Column.FieldName == "CELULAR" && e.Value != null)
            {
                decimal fone = 0;
                if (decimal.TryParse(e.Value.ToString().Replace(" ", ""), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = "";
                }
            }
        }

    }//classe
}//namespace
