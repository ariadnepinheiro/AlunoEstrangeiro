namespace Techne.Lyceum.Net.Hades
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Techne.Controls;
    using Techne.Data;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Web;
    using Techne.Lyceum.RN.Util;
    using Techne.Lyceum.CR;
    using DevExpress.Web.ASPxGridView;

    [NavUrl("~/Hades/Usuarios.aspx"), ControlText("Usuarios"), Title("Usuários")]
    public partial class Usuarios : TPage
    {
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

        public string Usuario
        {
            get { return (string)ViewState["usuario"]; }
            set { ViewState["usuario"] = value; }
        }

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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPadUsuario, "Padrões de Acesso");
            TituloGrid(grdUsuarioUnidade, "Unidades de Acesso Permitido do Usuário");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(grdPadUsuario);
            ControlaAcesso(grdUsuarioUnidade);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            btnResetarCpf.Visible = (tseUsuario.IsValidDBValue && !tseUsuario.DBValue.IsNull);

            if (!Permission.AllowDelete)
                btnRemoverTodasUnidades.Visible = false;
            if (!Permission.AllowInsert)
                btnAdicionarTodasUnidades.Visible = false;

            if (!IsPostBack)
            {
                // Abrir sempre na primeira aba
                pcUsuarios.ActiveTabIndex = 0;
                CarregaTipoExterno();
                this._tipoOperacao = TipoOperacao.Inicial;
                this.ControlarTipoOperacao();
                pcUsuarios.TabPages[1].Enabled = false;
                pcUsuarios.TabPages[2].Enabled = false;
            }

            //this.PermitePadrao();
        }

        private void PermitePadrao()
        {
            if (!string.IsNullOrEmpty(Usuario))
            {
                pcUsuarios.TabPages[1].Enabled = true;
                pcUsuarios.TabPages[2].Enabled = true;
            }
            else
            {
                pcUsuarios.TabPages[1].Enabled = false;
                pcUsuarios.TabPages[2].Enabled = false;
            }
        }

        #region Métodos
        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        tseUsuario.ResetValue();
                        pcUsuarios.Visible = false;
                        pcUsuarios.TabPages[1].Enabled = false;
                        pcUsuarios.TabPages[2].Enabled = false;
                        pcUsuarios.ActiveTabIndex = 0;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        if (RN.Usuarios.VerificaHabilitado(txtUsuario.Text))
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            ControlarVisibilidadeControle(controles);
                        }
                        else
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                            ControlarVisibilidadeControle(controles);
                        }

                        pcUsuarios.Visible = true;
                        pcUsuarios.TabPages[1].Enabled = true;
                        pcUsuarios.TabPages[2].Enabled = true;

                        lblSenha.Visible = true;
                        lblSenha2.Visible = true;
                        txtSenha.Visible = false;

                        tdsPadUsuario.SqlWhere = "hd_padusuario.usuario = '" + RN.RNBase.MudarAspas(txtUsuario.Text) + "'";
                        tdsPadUsuario.DataBind();
                        DesabilitaCampos();

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        pcUsuarios.ActiveTabIndex = 0;
                        tseFuncionario.Enabled = true;
                        rblTipoUsuario.Enabled = true;
                        tseUsuarioExterno.Enabled = true;
                        ddlTipoUsuarioExterno.Enabled = true;

                        rblTipoUsuario.SelectedValue = "Seeduc";
                        rblTipoUsuario_SelectedIndexChanged(null, null);

                        if (string.IsNullOrEmpty(txtUsuario.Text))
                            Usuario = Convert.ToString(txtUsuario.Text);
                        else
                            Usuario = string.Empty;

                        chkHabilitado.Checked = true;
                        lblSenha.Visible = true;
                        lblSenha2.Visible = false;
                        txtSenha.Visible = true;

                        LimparTela();
                        tseUsuario.ResetValue();
                        tseUsuario.Enabled = false;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        lblMensagem.Text = string.Empty;

                        pcUsuarios.Visible = true;

                        HabilitaCampos();

                        pcUsuarios.TabPages[1].Enabled = false;
                        pcUsuarios.TabPages[2].Enabled = false;
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        try
                        {
                            pcUsuarios.ActiveTabIndex = 0;
                            string usuario = txtUsuario.Text;
                            var dtUsuario = new RN.Entidades.HdUsuario();

                            if (rblTipoUsuario.SelectedValue == "Seeduc")
                            {
                                dtUsuario = new RN.Entidades.HdUsuario
                                {
                                    Matricula = txtMatricula.Text,
                                    IdVinculo = txtIdVinculo.Text,
                                    Pessoa = (!tseFuncionario.DBValue.IsNull && !string.IsNullOrEmpty(Convert.ToString(tseFuncionario["pessoa"]))) ? Convert.ToDecimal(tseFuncionario["pessoa"]) : (decimal?)null,
                                    Usuario = (!string.IsNullOrEmpty(txtUsuario.Text)) ? txtUsuario.Text : null,
                                    Nome = (!string.IsNullOrEmpty(txtNome.Text)) ? txtNome.Text : null,
                                    Senha = (!string.IsNullOrEmpty(txtSenha.Text)) ? RN.RNBase.HdPal(txtSenha.Text) : null,
                                    Setor = (tseSetor.IsValidDBValue) ? Convert.ToString(tseSetor["setor"]) : null,
                                    Habilitado = (chkHabilitado.Checked && !_tipoOperacao.Equals(TipoOperacao.Excluir)) ? "S" : "N",
                                    Privilegiado = (chkPrivilegiado.Checked) ? "S" : "N",
                                    PrivUnidadeEns = (chkPrivilegiadoUE.Checked) ? "S" : "N",
                                    Email = (!string.IsNullOrEmpty(txtEmail.Text)) ? txtEmail.Text : null,
                                    Grupousu = null
                                };
                            }
                            else
                            {
                                dtUsuario = new RN.Entidades.HdUsuario
                                {
                                    Matricula = null,
                                    IdVinculo = null,
                                    Pessoa = Convert.ToDecimal(tseUsuarioExterno["pessoa"]),
                                    Usuario = (!string.IsNullOrEmpty(txtUsuario.Text)) ? txtUsuario.Text : null,
                                    Nome = (!string.IsNullOrEmpty(txtNome.Text)) ? txtNome.Text : null,
                                    Senha = (!string.IsNullOrEmpty(txtSenha.Text)) ? RN.RNBase.HdPal(txtSenha.Text) : null,
                                    Setor = (tseSetor.IsValidDBValue) ? Convert.ToString(tseSetor["setor"]) : null,
                                    Habilitado = (chkHabilitado.Checked && !_tipoOperacao.Equals(TipoOperacao.Excluir)) ? "S" : "N",
                                    Privilegiado = (chkPrivilegiado.Checked) ? "S" : "N",
                                    PrivUnidadeEns = (chkPrivilegiadoUE.Checked) ? "S" : "N",
                                    Email = (!string.IsNullOrEmpty(txtEmail.Text)) ? txtEmail.Text : null,
                                    Grupousu = ddlTipoUsuarioExterno.SelectedValue
                                };
                            }

                            RN.Usuarios.Alterar(dtUsuario);

                            HabilitaCampos();
                            chkHabilitado.Checked = false;
                            var script = @"alert('Usuário excluído com sucesso');";

                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                            _tipoOperacao = TipoOperacao.Sucesso;
                            ControlarTipoOperacao();
                        }
                        catch (Exception ex)
                        {
                            lblMensagem.Text = ex.Message;

                            throw;
                        }


                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        pcUsuarios.ActiveTabIndex = 0;
                        Usuario = string.IsNullOrEmpty(txtUsuario.Text) ? Convert.ToString(txtUsuario.Text) : string.Empty;

                        tseUsuario.Enabled = false;
                        lblSenha.Visible = false;
                        lblSenha2.Visible = false;
                        txtSenha.Visible = false;
                        tseFuncionario.Enabled = false;
                        tseUsuario.Enabled = false;

                        HabilitaCampos();

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        pcUsuarios.TabPages[1].Enabled = true;
                        pcUsuarios.TabPages[2].Enabled = true;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        LimparTela();
                        pcUsuarios.ActiveTabIndex = 0;
                        lblMensagem.Text = string.Empty;

                        tseUsuario.Enabled = true;
                        lblSenha.Visible = true;
                        lblSenha2.Visible = true;
                        txtSenha.Visible = false;
                        var dadosUsuario = RN.Usuarios.Carregar(tseUsuario.DBValue.ToString());

                        if (dadosUsuario != null)
                        {
                            if (RN.Usuarios.VerificaHabilitado(tseUsuario.DBValue.ToString()))
                            {
                                ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                                ControlarVisibilidadeControle(controles);
                            }
                            else
                            {
                                ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                                ControlarVisibilidadeControle(controles);
                            }
                            pcUsuarios.Visible = true;
                            PreencherDadosTela(dadosUsuario);
                            DesabilitaCampos();

                            pcUsuarios.TabPages[1].Enabled = true;
                            pcUsuarios.TabPages[2].Enabled = true;
                        }
                        else
                        {
                            LimparTela();
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            pcUsuarios.Visible = false;
                            lblMensagem.Text = "Usuário não cadastrado.";
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
            tseFuncionario.ResetValue();
            tseUsuarioExterno.ResetValue();
            tseRegional.ResetValue();
            ddlTipoUsuarioExterno.ClearSelection();
            this.LimparDadosPessoa();
        }

        protected void LimparDadosPessoa()
        {
            //Limpa dados da tela
            txtIdVinculo.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtSenha.Text = string.Empty;
            tseSetor.ResetValue();
            chkPrivilegiado.Checked = false;
            chkHabilitado.Checked = false;
            chkPrivilegiadoUE.Checked = false;
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            txtNome.ReadOnly = true;
            txtMatricula.ReadOnly = true;
            txtIdVinculo.ReadOnly = true;
            txtTelefone.ReadOnly = true;
            txtSenha.ReadOnly = false;
            tseSetor.Mode = ControlMode.Edit;
            chkPrivilegiado.Enabled = true;
            chkHabilitado.Enabled = true;
            txtEmail.ReadOnly = false;
            chkPrivilegiadoUE.Enabled = true;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            tseFuncionario.Enabled = false;
            tseUsuarioExterno.Enabled = false;
            ddlTipoUsuarioExterno.Enabled = false;
            rblTipoUsuario.Enabled = false;
            txtNome.ReadOnly = true;
            txtIdVinculo.ReadOnly = true;
            txtMatricula.ReadOnly = true;
            txtTelefone.ReadOnly = true;
            txtSenha.ReadOnly = true;
            tseSetor.Mode = ControlMode.View;
            chkPrivilegiado.Enabled = false;
            chkHabilitado.Enabled = false;
            txtEmail.ReadOnly = true;
            chkPrivilegiadoUE.Enabled = false;
        }


        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTela(RN.Entidades.HdUsuario dadosUsuario)
        {
            RN.Setores rnSetores = new Techne.Lyceum.RN.Setores();
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.TipoUsuarioExterno();
            string chave = string.Empty;

            if (dadosUsuario.Grupousu.IsNullOrEmptyOrWhiteSpace())
            {
                rblTipoUsuario.SelectedValue = "Seeduc";
                rblTipoUsuario_SelectedIndexChanged(null, null);

                tseFuncionario.ResetValue();

                //verifica se a coluna matricula é realmente matricula, ou id/vinculo
                bool utilizaMatricula = !dadosUsuario.Usuario.Contains("/");
                QueryTable qt;

                //Busca Funcionario
                if (utilizaMatricula || dadosUsuario.IdVinculo == null)
                {
                    //Busca na vw_funcionarios por matricula
                    qt = RN.Usuarios.ConsultaServidorMatricula(dadosUsuario.Matricula);
                }
                else
                {
                    //Busca na ws_funcinarios por idvinculo
                    qt = RN.Usuarios.ConsultaServidorIdVinculo(dadosUsuario.IdVinculo);
                }

                if (qt != null && qt.Rows.Count > 0)
                {
                    chave = Convert.ToString(qt.Rows[0]["idvinculo_matricula"]);
                    tseFuncionario.Value = chave;
                    tseFuncionario.RaisePostBackEvent(chave);
                    tseFuncionario.DataBind();
                }

                if (dadosUsuario.Matricula != null || dadosUsuario.IdVinculo != null)
                {
                    if (!chave.IsNullOrEmptyOrWhiteSpace())
                    {
                        tseFuncionario.Value = chave;
                    }

                    string pessoa = Convert.ToString(dadosUsuario.Pessoa);
                    var dadosPessoa = RN.Pessoa.Carregar(Convert.ToInt32(pessoa));
                    if (dadosPessoa != null)
                        PreencherDadosPessoaConsulta(dadosPessoa);
                }
            }
            else
            {
                rblTipoUsuario.SelectedValue = "Externo";
                rblTipoUsuario_SelectedIndexChanged(null, null);

                ListItem listItem = ddlTipoUsuarioExterno.Items.FindByText(dadosUsuario.Grupousu);
                if (listItem == null)
                {
                    string tipoExterno = rnTipoUsuarioExterno.RetornaDescricaoPor(Convert.ToInt32(dadosUsuario.Grupousu));
                    ListItem item = new ListItem(tipoExterno, dadosUsuario.Grupousu);
                    ddlTipoUsuarioExterno.Items.Insert(0, item);
                }

                ddlTipoUsuarioExterno.SelectedValue = dadosUsuario.Grupousu;

                if (dadosUsuario.Pessoa != null && dadosUsuario.Pessoa != 0)
                {
                    string pessoa = Convert.ToString(dadosUsuario.Pessoa);
                    var dadosPessoa = RN.Pessoa.Carregar(Convert.ToInt32(pessoa));

                    if (dadosPessoa != null)
                    {
                        PreencherDadosPessoaConsulta(dadosPessoa);
                        tseUsuarioExterno.ResetValue();
                        tseUsuarioExterno.Value = dadosPessoa.Cpf;
                        tseUsuarioExterno.RaisePostBackEvent(dadosPessoa.Cpf);
                        tseUsuarioExterno.DataBind();
                    }
                }
            }

            txtUsuario.Text = Convert.ToString(dadosUsuario.Usuario);

            // Se a matrícula contiver '/' e o IdVinculo estiver nulo o valor da matrícula será utilizado como IdVinculo.
            if(dadosUsuario.Matricula.Contains("/") && Convert.ToString(dadosUsuario.IdVinculo) == null)
            {
                txtIdVinculo.Text = Convert.ToString(dadosUsuario.Matricula);
            }
            // Caso o valor retornado do banco contenha '/' será utilizado o valor de tseUsuario.DBValue.
            else if(Convert.ToString(tseUsuario.DBValue).Contains("/"))
            {
                txtIdVinculo.Text = Convert.ToString(tseUsuario.DBValue);
            }
            else
            {
                txtIdVinculo.Text = Convert.ToString(dadosUsuario.IdVinculo);
            }

            txtMatricula.Text = Convert.ToString(dadosUsuario.Matricula);
            txtNome.Text = Convert.ToString(dadosUsuario.Nome);
            txtSenha.Text = Convert.ToString(dadosUsuario.Senha);

            if (!string.IsNullOrEmpty(dadosUsuario.Setor))
            {
                tseSetor.DBValue = rnSetores.ObtemSetorAtualPor(Convert.ToString(dadosUsuario.Setor));
            }

            chkPrivilegiado.Checked = dadosUsuario.Privilegiado == "S";
            chkHabilitado.Checked = dadosUsuario.Habilitado == "S";
            chkPrivilegiadoUE.Checked = dadosUsuario.PrivUnidadeEns == "S";
        }

        private void CarregaTipoExterno()
        {
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.TipoUsuarioExterno();

            try
            {
                ListItem item = new ListItem("Nenhum", string.Empty);

                ddlTipoUsuarioExterno.Items.Clear();
                ddlTipoUsuarioExterno.DataSource = rnTipoUsuarioExterno.ListaTipoUsuarioExternoAtivo();
                ddlTipoUsuarioExterno.DataBind();
                ddlTipoUsuarioExterno.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherDadosPessoaConsulta(LyPessoa dadosPessoa)
        {
            txtNome.Text = dadosPessoa.Nome_compl;
            txtIdVinculo.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtTelefone.Text = dadosPessoa.Fone;
            txtCelular.Text = dadosPessoa.Celular;

            Int64 result;
            if (Int64.TryParse(dadosPessoa.Fone, out result))
            {
                txtTelefone.Text = string.Format("{0:(00)0000-0000}", result);
            }
            else
            {
                txtTelefone.Text = dadosPessoa.Fone;
            }

            if (Int64.TryParse(dadosPessoa.Celular, out result))
            {
                txtCelular.Text = string.Format("{0:(00)0000-0000}", result);
            }
            else
            {
                txtCelular.Text = dadosPessoa.Celular;
            }

            txtEmail.Text = Convert.ToString(dadosPessoa.E_mail_interno);
            txtSenha.Text = string.Empty;
        }

        private void CarregaDadosPessoa()
        {
            string chave = Convert.ToString(tseFuncionario.DBValue);
            bool utilizaMatricula = !chave.Contains("/");
            QueryTable qt;

            //Busca Funcionario
            if (utilizaMatricula)
            {
                //Busca na vw_funcionarios por matricula
                qt = RN.Usuarios.ConsultaServidorMatricula(chave);
            }
            else
            {
                //Busca na ws_funcinarios por idvinculo
                qt = RN.Usuarios.ConsultaServidorIdVinculo(chave);
            }

            if (qt.Rows.Count > 0)
            {
                txtNome.Text = Convert.ToString(qt.Rows[0]["nome_compl"]);
                txtIdVinculo.Text = Convert.ToString(qt.Rows[0]["idvinculo"]);
                txtMatricula.Text = Convert.ToString(qt.Rows[0]["matricula"]);
                Int64 result;
                if (Int64.TryParse(Convert.ToString(qt.Rows[0]["fone"]), out result))
                    txtTelefone.Text = string.Format("{0:(00)0000-0000}", result);
                else
                    txtTelefone.Text = Convert.ToString(qt.Rows[0]["fone"]);

                if (Int64.TryParse(Convert.ToString(qt.Rows[0]["celular"]), out result))
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", result);
                else
                    txtCelular.Text = Convert.ToString(qt.Rows[0]["celular"]);

                txtEmail.Text = Convert.ToString(qt.Rows[0]["e_mail_interno"]);
                if (!string.IsNullOrEmpty(Convert.ToString(qt.Rows[0]["setor"])))
                {
                    tseSetor.ResetValue();
                    tseSetor.DBValue = Convert.ToString(qt.Rows[0]["ua_atual"]);
                }
                else
                    tseSetor.ResetValue();

            }

            txtSenha.Text = string.Empty;
            txtUsuario.Text = chave.PadLeft(8, '0');
        }

        private void CarregaDadosExterno()
        {
            txtNome.Text = Convert.ToString(tseUsuarioExterno["nome"]);
            txtIdVinculo.Text = string.Empty;
            txtMatricula.Text = string.Empty;

            Int64 result;
            if (Int64.TryParse(Convert.ToString(tseUsuarioExterno["fone"]), out result))
            {
                txtTelefone.Text = string.Format("{0:(00)0000-0000}", result);
            }
            else
            {
                txtTelefone.Text = Convert.ToString(tseUsuarioExterno["fone"]);
            }

            if (Int64.TryParse(Convert.ToString(tseUsuarioExterno["celular"]), out result))
            {
                txtCelular.Text = string.Format("{0:(00)0000-0000}", result);
            }
            else
            {
                txtCelular.Text = Convert.ToString(tseUsuarioExterno["celular"]);
            }

            txtEmail.Text = Convert.ToString(tseUsuarioExterno["email"]);
            txtSenha.Text = string.Empty;
            //Monta Usuario
            txtUsuario.Text = string.Format("E{0}", Convert.ToString(tseUsuarioExterno["codigo"]).PadLeft(7, '0'));

            tseSetor.ResetValue();
        }

        #endregion

        #region Eventos

        protected void tseUsuario_Changed(object sender, EventArgs args)
        {
            tseFuncionario.ResetValue();
            LimparDadosPessoa();

            if (tseUsuario.IsValidDBValue && !tseUsuario.DBValue.IsNull)
            {
                pcUsuarios.Visible = true;
                pcUsuarios.TabPages[1].Enabled = true;
                pcUsuarios.TabPages[2].Enabled = true;
                pcUsuarios.ActiveTabIndex = 0;

                btnAdicionarTodasUnidades.Visible = true;
                btnRemoverTodasUnidades.Visible = true;
                grdUsuarioUnidade.Visible = true;
                lblMensagem.Text = string.Empty;

                this._tipoOperacao = TipoOperacao.Consultar;
                this.ControlarTipoOperacao();

                tdsPadUsuario.SqlWhere = "hd_padusuario.usuario = '" + RN.RNBase.MudarAspas(tseUsuario.DBValue.ToString()) + "'";
                tdsPadUsuario.DataBind();
                lblMensagem.Text = string.Empty;
            }
            else
            {
                btnAdicionarTodasUnidades.Visible = false;
                btnRemoverTodasUnidades.Visible = false;
                grdUsuarioUnidade.Visible = false;
                pcUsuarios.TabPages[1].Enabled = false;
                pcUsuarios.TabPages[2].Enabled = false;
                this._tipoOperacao = TipoOperacao.Inicial;
                this.ControlarTipoOperacao();
                lblMensagem.Text = "Usuário " + tseUsuario.DBValue.ToString() + " não cadastrado.";
                tseUsuario.ResetValue();
            }
        }

        protected void tseFuncionario_Changed(object sender, EventArgs args)
        {
            tseUsuarioExterno.ResetValue();
            LimparDadosPessoa();

            if (tseFuncionario.IsValidDBValue && !tseFuncionario.DBValue.IsNull)
            {
                this.CarregaDadosPessoa();
            }
            else if (tseFuncionario.Value != null)
            {
                lblMensagem.Text = "Funcionário não cadastrado.";
            }
            else if (tseFuncionario.Value == null)
            {
                lblMensagem.Text = "Favor selecionar um Funcionário.";
            }
        }

        protected void tseUsuarioExterno_Changed(object sender, EventArgs args)
        {
            //Limpa dados da tela
            tseFuncionario.ResetValue();
            txtTelefone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtIdVinculo.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtSenha.Text = string.Empty;
            tseSetor.ResetValue();
            chkPrivilegiado.Checked = false;
            chkHabilitado.Checked = false;
            chkPrivilegiadoUE.Checked = false;

            if (tseUsuarioExterno.IsValidDBValue && !tseUsuarioExterno.DBValue.IsNull)
            {
                this.CarregaDadosExterno();
            }
            else if (tseUsuarioExterno.Value != null)
            {
                lblMensagem.Text = "Usuário externo não cadastrado.";
            }
            else if (tseUsuarioExterno.Value == null)
            {
                lblMensagem.Text = "Favor selecionar um usuário externo.";
            }
        }

        protected void rblTipoUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Limpa dados da tela
            LimparTela();
            CarregaTipoExterno();

            if (rblTipoUsuario.SelectedValue == "Externo")
            {
                lblTipoUsuarioExterno.Visible = true;
                ddlTipoUsuarioExterno.Visible = true;
                lblUsuarioExterno.Visible = true;
                tseUsuarioExterno.Visible = true;

                lblFuncionario.Visible = false;
                tseFuncionario.Visible = false;
                lblIdVinculo.Visible = false;
                txtIdVinculo.Visible = false;
                lblMatricula.Visible = false;
                txtMatricula.Visible = false;
            }
            else
            {
                lblTipoUsuarioExterno.Visible = false;
                ddlTipoUsuarioExterno.Visible = false;
                lblUsuarioExterno.Visible = false;
                tseUsuarioExterno.Visible = false;

                lblFuncionario.Visible = true;
                tseFuncionario.Visible = true;
                lblIdVinculo.Visible = true;
                txtIdVinculo.Visible = true;
                lblMatricula.Visible = true;
                txtMatricula.Visible = true;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Entidades.HdUsuario dtUsuario = new RN.Entidades.HdUsuario();
                string mensagem = string.Empty;

                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();
                RN.RecursosHumanos.UsuarioExterno rnUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.UsuarioExterno();

                if (rblTipoUsuario.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "O campo Tipo é de preenchimento obrigatório.";
                    return;
                }

                if (rblTipoUsuario.SelectedValue == "Externo")
                {
                    if (ddlTipoUsuarioExterno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "O campo Tipo Externo é de preenchimento obrigatório.";
                        return;
                    }

                    if (!tseUsuarioExterno.IsValidDBValue || tseUsuarioExterno.DBValue.IsNull)
                    {
                        lblMensagem.Text = "O campo Usuário Externo é de preenchimento obrigatório.";
                        return;
                    }
                    else
                    {
                        if (!rnUsuarioExterno.EhAtivoPor(Convert.ToDecimal(tseUsuarioExterno["pessoa"]), Convert.ToInt32(ddlTipoUsuarioExterno.SelectedValue)))
                        {
                            lblMensagem.Text = "O usuário não pode ser criado pois este externo não está ativo.";
                            return;
                        }
                    }

                    dtUsuario = new RN.Entidades.HdUsuario
                    {
                        IdVinculo = null,
                        Matricula = null,
                        Pessoa = Convert.ToDecimal(tseUsuarioExterno["pessoa"]),
                        Usuario = (!string.IsNullOrEmpty(txtUsuario.Text)) ? txtUsuario.Text : null,
                        Nome = (!string.IsNullOrEmpty(txtNome.Text)) ? txtNome.Text : null,
                        Senha = (!string.IsNullOrEmpty(txtSenha.Text)) ? RNBase.HdPal(txtSenha.Text) : null,
                        Setor = (tseSetor.IsValidDBValue) ? Convert.ToString(tseSetor["setor"]) : null,
                        Habilitado = (chkHabilitado.Checked && !_tipoOperacao.Equals(TipoOperacao.Excluir)) ? "S" : "N",
                        Privilegiado = (chkPrivilegiado.Checked) ? "S" : "N",
                        PrivUnidadeEns = (chkPrivilegiadoUE.Checked) ? "S" : "N",
                        Email = (!string.IsNullOrEmpty(txtEmail.Text)) ? txtEmail.Text : null,
                        Grupousu = ddlTipoUsuarioExterno.SelectedValue
                    };
                }
                else
                {
                    if (!tseFuncionario.IsValidDBValue && tseFuncionario.DBValue.IsNull)
                    {
                        lblMensagem.Text = "O campo Funcionário é de preenchimento obrigatório.";
                        return;
                    }

                    dtUsuario = new RN.Entidades.HdUsuario
                    {
                        Matricula = txtMatricula.Text,
                        IdVinculo = txtIdVinculo.Text,
                        Pessoa = (!tseFuncionario.DBValue.IsNull && !string.IsNullOrEmpty(Convert.ToString(tseFuncionario["pessoa"]))) ? Convert.ToDecimal(tseFuncionario["pessoa"]) : (decimal?)null,
                        Usuario = (!string.IsNullOrEmpty(txtUsuario.Text)) ? txtUsuario.Text : null,
                        Nome = (!string.IsNullOrEmpty(txtNome.Text)) ? txtNome.Text : null,
                        Senha = (!string.IsNullOrEmpty(txtSenha.Text)) ? RNBase.HdPal(txtSenha.Text) : null,
                        Setor = (tseSetor.IsValidDBValue) ? Convert.ToString(tseSetor["setor"]) : null,
                        Habilitado = (chkHabilitado.Checked && !_tipoOperacao.Equals(TipoOperacao.Excluir)) ? "S" : "N",
                        Privilegiado = (chkPrivilegiado.Checked) ? "S" : "N",
                        PrivUnidadeEns = (chkPrivilegiadoUE.Checked) ? "S" : "N",
                        Email = (!string.IsNullOrEmpty(txtEmail.Text)) ? txtEmail.Text : null,
                        Grupousu = null
                    };

                    if (string.IsNullOrEmpty(Convert.ToString(dtUsuario.Pessoa)))
                    {
                        lblMensagem.Text = "Problemas ao carregar o Funcionário. Verificar lotação.";
                        return;
                    }
                }


                if (this._tipoOperacao.Equals(TipoOperacao.Novo))
                {
                    if (dtUsuario.Usuario.Length > 15)
                    {
                        lblMensagem.Text = "O campo Usuário deve conter no máximo 15 caracteres.";
                        return;
                    }

                    if (rnUsuarios.PossuiUsuarioCadastrado(dtUsuario.Usuario))
                    {
                        lblMensagem.Text = "Usuário já cadastrado.";
                        return;
                    }
                    else
                    {
                        //Verifica se o usuario possui matricula e não está utilizando ela como usuario
                        if (!dtUsuario.Matricula.IsNullOrEmptyOrWhiteSpace() && dtUsuario.Usuario != dtUsuario.Matricula)
                        {
                            string usuarioBase = RN.Usuarios.ObterUsuarioPorMatricula(dtUsuario.Matricula);
                            if (!usuarioBase.IsNullOrEmptyOrWhiteSpace())
                            {
                                lblMensagem.Text = string.Format("Este Funcionario já possui um usuario cadastrado com sua matricula: {0}.", dtUsuario.Matricula);
                                return;
                            }
                        }

                        //Verifica se o usuario possui idvinculo e não está utilizando como usuario
                        if (!dtUsuario.IdVinculo.IsNullOrEmptyOrWhiteSpace() && dtUsuario.Usuario != dtUsuario.IdVinculo)
                        {
                            string usuarioBase = RN.Usuarios.ObterUsuarioPorIdVinculo(dtUsuario.IdVinculo);
                            if (!usuarioBase.IsNullOrEmptyOrWhiteSpace())
                            {
                                lblMensagem.Text = string.Format("Este Funcionario já possui um usuario cadastrado com o id/Vinculo: {0}.", dtUsuario.IdVinculo);
                                return;
                            }
                        }
                    }

                    if (txtSenha.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "O campo Senha é de preenchimento obrigatório.";
                        return;
                    }

                    RN.Usuarios.Inserir(dtUsuario);
                    mensagem = "Usuário incluído com sucesso. ";

                    tseUsuario.ResetValue();
                    tseUsuario.DBValue = txtUsuario.Text;

                    grdUsuarioUnidade.DataBind();
                    grdUsuarioUnidade.Visible = true;

                    if (Permission.AllowDelete)
                    {
                        btnRemoverTodasUnidades.Visible = true;
                    }

                    if (Permission.AllowInsert)
                    {
                        btnAdicionarTodasUnidades.Visible = true;
                    }
                }
                else if (this._tipoOperacao.Equals(TipoOperacao.Alterar))
                {
                    RN.Usuarios.Alterar(dtUsuario);

                    mensagem = "Usuário alterado com sucesso. ";
                }

                if (!string.IsNullOrEmpty(txtEmail.Text))
                {
                    var retornoEmail = Pessoa.AlteraEmail(txtEmail.Text, Convert.ToDecimal(dtUsuario.Pessoa));
                    mensagem = mensagem + retornoEmail.Message;
                }

                var script = @"alert('" + mensagem + @"');";

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                this._tipoOperacao = TipoOperacao.Sucesso;

                this.ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnResetarCpf_Click(object sender, EventArgs e)
        {
            try
            {
                //RetValue retorno = null;
                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();

                string cpf = (tseFuncionario.IsValidDBValue) ? Convert.ToString(tseFuncionario["cpf"]) : null;

                if (cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Este usuário não possui CPF, favor utilizar a tela de Alteração de Senha do Usuário.";
                    return;
                }

                rnUsuarios.AlteraSenhaUsuario(tseUsuario.DBValue.ToString(), cpf);
                lblMensagem.Text = "Senha Alterada com sucesso.<br />A nova senha é: " + cpf + ".";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            pcUsuarios.TabPages[1].Enabled = false;
            pcUsuarios.TabPages[2].Enabled = false;

            this._tipoOperacao = TipoOperacao.Novo;

            this.ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            this._tipoOperacao = TipoOperacao.Excluir;

            this.ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            this._tipoOperacao = TipoOperacao.Alterar;

            this.ControlarTipoOperacao();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (tseUsuario.IsValidDBValue
                && !tseUsuario.DBValue.IsNull)
            {
                pcUsuarios.TabPages[1].Enabled = true;
                pcUsuarios.TabPages[2].Enabled = true;
            }
            else
            {
                pcUsuarios.TabPages[1].Enabled = false;
                pcUsuarios.TabPages[2].Enabled = false;
            }

            this._tipoOperacao = TipoOperacao.Inicial;

            this.ControlarTipoOperacao();
        }

        protected void btnAdicionarTodasUnidades_Click(object sender, EventArgs e)
        {
            if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
            {
                lblMensagem.Text = "Selecione um usuário válido.";
                return;
            }

            RetValue ret = RN.Usuarios.InserirAcessoTodasUnidadesFisicas(tseUsuario.DBValue.ToString());
            if (ret != null && !ret.Ok)
                lblMensagem.Text = "Não foi possível inserir todas as unidades físicas.<br/>Operação cancelada.";
            grdUsuarioUnidade.DataBind();

            this._tipoOperacao = TipoOperacao.Sucesso;
            this.ControlarTipoOperacao();
            pcUsuarios.ActiveTabIndex = 2;
        }

        protected void btnRemoverTodasUnidades_Click(object sender, EventArgs e)
        {
            RN.Usuarios rnUsuarios = new RN.Usuarios();

            try
            {
                if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
                {
                    lblMensagem.Text = "Selecione um usuário válido.";
                    return;
                }

                rnUsuarios.RemoveAcessoUnidadesFisicasPor(tseUsuario.DBValue.ToString());
                grdUsuarioUnidade.DataBind();

                this._tipoOperacao = TipoOperacao.Sucesso;
                this.ControlarTipoOperacao();
                pcUsuarios.ActiveTabIndex = 2;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnInserirRegional_Click(object sender, EventArgs e)
        {
            if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
            {
                lblMensagem.Text = "Selecione um usuário válido.";
                return;
            }

            if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
            {
                RetValue ret = RN.Usuarios.InserirAcessoTodasUnidadesFisicas(tseRegional.DBValue.ToString(), tseUsuario.DBValue.ToString());
                if (ret != null && !ret.Ok)
                    lblMensagem.Text = "Não foi possível incluir as unidades físicas da regional.<br/>Operação cancelada.";
                grdUsuarioUnidade.DataBind();

                this._tipoOperacao = TipoOperacao.Sucesso;
                this.ControlarTipoOperacao();
                pcUsuarios.ActiveTabIndex = 2;
                tseRegional.ResetValue();
            }
            else
            {
                lblMensagem.Text = "Selecione uma Regional válida.";
            }
        }

        protected void btnRemoverRegional_Click(object sender, EventArgs e)
        {
            if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
            {
                lblMensagem.Text = "Selecione um usuário válido.";
                return;
            }

            if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
            {
                RetValue ret = RN.Usuarios.RemoverAcessoUnidadesFisicas(tseRegional.DBValue.ToString(), tseUsuario.DBValue.ToString());
                if (ret != null && !ret.Ok)
                    lblMensagem.Text = "Não foi possível incluir as unidades físicas da regional.<br/>Operação cancelada.";
                grdUsuarioUnidade.DataBind();
                tseRegional.ResetValue();

                this._tipoOperacao = TipoOperacao.Sucesso;
                this.ControlarTipoOperacao();
                pcUsuarios.ActiveTabIndex = 2;
            }
            else
            {
                lblMensagem.Text = "Selecione uma regional válida.";
            }
        }

        #endregion

        #region Grid
        protected void grdUsuarioUnidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdUsuarioUnidade);
        }

        protected void grdUsuarioUnidade_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string usuario = Convert.ToString(e.GetListSourceFieldValue("usuario"));
                string faculdade = Convert.ToString(e.GetListSourceFieldValue("unidade_fis"));
                e.Value = usuario + "-" + faculdade;
            }
        }

        protected void grdUsuarioUnidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("usuario", e.Values["usuario"]);
            e.Keys.Add("unidade_fis", e.Values["unidade_fis"]);
        }

        protected void grdUsuarioUnidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["usuario"] = tseUsuario.IsValidDBValue && !tseUsuario.DBValue.IsNull ? tseUsuario.DBValue.ToString() : txtUsuario.Text;
        }

        protected void grdUsuarioUnidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("usuario", chaves[0]);
            e.Keys.Add("unidade_fis", chaves[1]);
        }

        protected void grdUsuarioUnidade_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdUsuarioUnidade.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "nome_comp03")
                    e.Editor.ReadOnly = false;

            }
            else if (grdUsuarioUnidade.IsEditing)
            {
                if ((e.Column.FieldName) == "nome_comp03")
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdUsuarioUnidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdUsuarioUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdUsuarioUnidade_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            TSearchBox tseUnidadeFisica = (TSearchBox)grdUsuarioUnidade.FindEditFormTemplateControl("tseUnidadeFisica");

            if (tseUnidadeFisica != null)
            {
                if (tseUnidadeFisica.DBValue.IsNull)
                    e.RowError = "Favor selecionar uma para unidade física.";

                if (!tseUnidadeFisica.IsValidDBValue)
                    e.RowError = "Favor selecionar uma unidade física válida.";
            }
        }

        protected void grdPadUsuario_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPadUsuario);
            if (!String.IsNullOrEmpty(txtUsuario.Text))
            {
                tdsPadUsuario.SqlWhere = "hd_padusuario.usuario = '" + RN.RNBase.MudarAspas(txtUsuario.Text) + "'";
                tdsPadUsuario.Select();
                grdPadUsuario.DataBind();
            }

        }
        protected void grdPadUsuario_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPadUsuario.Settings.ShowFilterRow = false;
        }

        protected void grdPadUsuario_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("usuario", chaves[0]);
            e.Keys.Add("sis", chaves[1]);
        }

        protected void grdPadUsuario_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUsuario.Text))
                e.NewValues["usuario"] = txtUsuario.Text;

            if (!String.IsNullOrEmpty(txtNome.Text))
                e.NewValues["nome"] = txtNome.Text;
        }

        protected void grdPadUsuario_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("padaces", e.Values["padaces"]);
            e.Keys.Add("usuario", e.Values["usuario"]);
        }

        protected void grdPadUsuario_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string padaces = Convert.ToString(e.GetListSourceFieldValue("padaces"));
                string usuario = Convert.ToString(e.GetListSourceFieldValue("usuario"));

                e.Value = padaces + "-" + usuario;
            }
        }

        protected void grdPadUsuario_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPadUsuario.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "usuario")
                    e.Editor.Enabled = true;
            }
            else if (grdPadUsuario.IsEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "usuario")
                    e.Editor.Enabled = false;
            }
        }
        #endregion
    }
}