using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Lyceum.CR;
using Techne.Web;


namespace Techne.Lyceum.Net.Curriculo
{
    [
     NavUrl("~/Curriculo/OutrasInstituicoes.aspx"),
      ControlText("OutrasInstituicoes"),
      Title("Outras Instituições"),
    ]
    public partial class OutrasInstituicoes : TPage
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


        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
                
                ddlTipoInstituicao.Items.Clear();
                ddlTipoInstituicao.DataSource = RN.Basico.ConsultaItemTabelaValDescr("TipoInstituicao"); 
                ddlTipoInstituicao.Items.Insert(0, "Selecione");
                ddlTipoInstituicao.DataBind();
            }
        }


        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }


        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
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

        #region Métodos

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        tseInstituicao.ResetValue();
                        pcInstituicao.Visible = false;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        pcInstituicao.Visible = true;
                        DesabilitaCampos();
                        tsCEP.ShowButton = false;

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        txtOutraFaculdade.Text = RN.Instituicao.GeraOutraFaculdade();

                        txtNome_Comp.ReadOnly = false;
                        tsCEP.ShowButton = true;
                        txtEstado.Visible = true;
                        LimparTela();
                        tseInstituicao.ResetValue();
                        tseInstituicao.Enabled = false;
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        lblMensagem.Text = string.Empty;
                        pcInstituicao.Visible = true;
                        HabilitaCampos();

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        Ly_instituicao dt = new Ly_instituicao();
                        Ly_instituicao.Row dados = dt.NewRow();

                        RN.RetValue retorno = null;
                        retorno = RN.Instituicao.Excluir(txtOutraFaculdade.Text);


                        if (retorno != null)
                        {
                            if (!retorno.Ok)
                            {
                                lblMensagem.Text = retorno.Errors.ToString();
                            }
                            else
                            {
                                LimparTela();
                                lblMensagem.Text = retorno.Message;
                                _tipoOperacao = TipoOperacao.Inicial;
                                ControlarTipoOperacao();
                            }
                        }
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseInstituicao.Enabled = false;

                        HabilitaCampos();

                        txtEstado.Visible = true;
                        tsCEP.ShowButton = true;
                        txtNome_Comp.ReadOnly = true;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tsCEP.ShowButton = false;
                        txtEstado.Visible = true;

                        lblMensagem.Text = string.Empty;

                        tseInstituicao.Enabled = true;

                        Ly_instituicao.Row dados = new Ly_instituicao().NewRow();

                        dados = RN.Instituicao.Consultar(tseInstituicao.DBValue.ToString());

                        if (dados != null)
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                            pcInstituicao.Visible = true;
                            PreencherDadosTela(dados);
                            DesabilitaCampos();
                        }
                        else
                        {
                            LimparTela();
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            pcInstituicao.Visible = false;
                            lblMensagem.Text = "Instituição não cadastrada.";
                        }
                        break;
                    }
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
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            txtNome_Comp.Text = string.Empty;
            txtEnd_Compl.Text = string.Empty;
            txtEnd_Num.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtCEP.Text = string.Empty;
            txtBairro.Text = string.Empty;
            tseMunicipio.ResetValue();
            ddlTipoInstituicao.Items.Clear();

            ddlTipoInstituicao.DataSource = RN.Basico.ConsultaItemTabelaValDescr("TipoInstituicao");
            ddlTipoInstituicao.Items.Insert(0, "Selecione");
            ddlTipoInstituicao.DataBind();
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            txtNome_Comp.ReadOnly = false;
            txtEnd_Compl.ReadOnly = false;
            txtEnd_Num.ReadOnly = false;
            txtEndereco.ReadOnly = false;
            txtCEP.ReadOnly = false;
            txtBairro.ReadOnly = false;
            tseMunicipio.Mode = ControlMode.Edit;
            //ddlTipoInstituicao.Enabled = false;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            txtNome_Comp.ReadOnly = true;
            txtEnd_Compl.ReadOnly = true;
            txtEnd_Num.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtCEP.ReadOnly = true;
            txtBairro.ReadOnly = true;
            tseMunicipio.Mode = ControlMode.View;
            //ddlTipoInstituicao.Enabled = true ;
        }

        /// <summary>
        /// Armazena uma nova linha com os dados da tela no datatable passado como parâmetro
        /// </summary>
        /// <param name="dtDocente">DataTable do docente que será adicionado uma nova linha</param>
        private void ObterDados(Ly_instituicao dt)
        {
            Techne.Lyceum.CR.Ly_instituicao.Row dados = dt.NewRow();

            if (!string.IsNullOrEmpty(txtNome_Comp.Text))
                dados.Nome_comp = txtNome_Comp.Text;

            if (!string.IsNullOrEmpty(txtEnd_Compl.Text))
                dados.End_compl = txtEnd_Compl.Text;

            if (!string.IsNullOrEmpty(txtEnd_Num.Text))
                dados.End_num = txtEnd_Num.Text;

            if (!string.IsNullOrEmpty(txtEndereco.Text))
                dados.Endereco = txtEndereco.Text;

            if (!string.IsNullOrEmpty(txtCEP.Text))
                dados.Cep = txtCEP.Text;

            if (!string.IsNullOrEmpty(txtBairro.Text))
                dados.Bairro = txtBairro.Text;

            if (tseMunicipio.IsValidDBValue)
                dados.Municipio = Convert.ToString(tseMunicipio["codigo"]);
            else
                tseMunicipio.ResetValue();

            if (ddlTipoInstituicao.SelectedValue != "Selecione")
                dados.Tipo_origem = ddlTipoInstituicao.SelectedValue ;

            //fixos:
            dados.Outra_faculdade = txtOutraFaculdade.Text;
            dados.Local_vest = "N";
            dados.Tipo_inst = "ENSINO_MEDIO";


            dt.Rows.Add(dados);
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTela(Ly_instituicao.Row dados)
        {
            txtOutraFaculdade.Text = Convert.ToString(dados.Outra_faculdade);
            txtNome_Comp.Text = Convert.ToString(dados.Nome_comp);
            txtEnd_Compl.Text = Convert.ToString(dados.End_compl);
            txtEnd_Num.Text = Convert.ToString(dados.End_num);
            txtEndereco.Text = Convert.ToString(dados.Endereco);
            txtCEP.Text = Convert.ToString(dados.Cep);
            txtBairro.Text = Convert.ToString(dados.Bairro);
            if(dados.Municipio != null)
                tseMunicipio.DBValue = Convert.ToString(dados.Municipio);

            if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
            {
                txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
            if (dados.Tipo_origem != null)
            {
                ddlTipoInstituicao.SelectedValue = Convert.ToString(dados.Tipo_origem);
            }

        }
        #endregion

        #region Eventos

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            RN.RetValue retorno = null;

            CR.Ly_instituicao dt = new Ly_instituicao();
            ObterDados(dt);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
                retorno = RN.Instituicao.Incluir(dt);
            else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                retorno = RN.Instituicao.Alterar(dt);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                }
                else
                {
                    lblMensagem.Text = retorno.Message;
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            txtEstado.Value = string.Empty;

            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Excluir;
            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            txtEstado.Value = string.Empty;
            _tipoOperacao = TipoOperacao.Inicial;
            ControlarTipoOperacao();
        }


        protected void tseInstituicao_Changed(object sender, EventArgs e)
        {
            if (tseInstituicao.IsValidDBValue && !tseInstituicao.DBValue.IsNull)
            {
                LimparTela();
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
                lblMensagem.Text = string.Empty;
                pcInstituicao.Visible = true;

            }
            else if (!tseInstituicao.DBValue.IsNull)
            {
                lblMensagem.Text = "Outra Instituição não cadastrada.";
                pcInstituicao.Visible = false;
            }
            else
            {
                lblMensagem.Text = "Favor consultar uma Outra Instituição.";
                pcInstituicao.Visible = false;
            }
        }
        #endregion

        #region endereço
        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                    txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
            else
            {
                txtEstado.Value = string.Empty;
            }
        }
        #endregion

    }
}

