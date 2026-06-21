using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Web.UI.MobileControls;
using Techne.Lyceum.RN;
using System.IO;
using DevExpress.Web.ASPxClasses;


namespace Techne.Lyceum.Net.Academico
{
    [
     NavUrl("~/Academico/Ocorrencias.aspx"),
      ControlText("Ocorrências"),
      Title("Histórico de Ocorrências"),
    ]
    public partial class Ocorrencias : TPage
    {
        #region Propriedades e Enum
        public enum Operacao
        {
            Novo,
            NovoAlt,
            Excluir,
            Alterar,
            Consultar,
            ConsultarRetornaDados,
            Inicial,
            Confirmar
        }

        private Operacao _tipoOperacao
        {
            get { return (Operacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        [Serializable]
        private class DadosOcorrencia
        {
            public DadosOcorrencia()
            {

            }

            private string _aluno;
            private string _tipo;
            private string _data;
            private string _ordem;
            private string _usuario;
            private string _descricao;
            private string _ano;
            private string _periodo;
            private string _disciplina;
            private string _turma;

            public string Aluno
            {
                get { return _aluno; }
                set { _aluno = value; }
            }


            public string Tipo
            {
                get { return _tipo; }
                set { _tipo = value; }
            }

            public string Data
            {
                get { return _data; }
                set { _data = value; }
            }

            public string Ordem
            {
                get { return _ordem; }
                set { _ordem = value; }
            }

            public string Usuario
            {
                get { return _usuario; }
                set { _usuario = value; }
            }

            public string Descricao
            {
                get { return _descricao; }
                set { _descricao = value; }
            }

            public string Ano
            {
                get { return _ano; }
                set { _ano = value; }
            }

            public string Periodo
            {
                get { return _periodo; }
                set { _periodo = value; }
            }

            public string Disciplina
            {
                get { return _disciplina; }
                set { _disciplina = value; }
            }

            public string Turma
            {
                get { return _turma; }
                set { _turma = value; }
            }

        }

        private DadosOcorrencia ObjetoOcorrencia
        {
            get { return (DadosOcorrencia)ViewState["ObjetoOcorrencia"]; }
            set { ViewState["ObjetoOcorrencia"] = value; }
        }


        #endregion


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //verifica se não é post back da página
            if (!IsPostBack)
            {
                //para a primeira vez que a página é carregada o tipo de operação será inicial
                // no caso de ocorrencia não faz sentido já que ela é acionada diretamente para uma operação
                _tipoOperacao = Operacao.Inicial;

                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                    ObterDadosQueryString(decodedText);

                }
                else
                    Response.Redirect("ListarOcorrencias.aspx");

                ControlarTipoOperacao();
            }
        }

        protected void pnOcorrencias_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlarTipoOperacao();
                ControlarAtributosCampos();
            }

        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnEditar, AcaoControle.editar);
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = Operacao.Novo;
            LimparTela();
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = Operacao.Excluir;

            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = Operacao.Alterar;

            ControlarTipoOperacao();

        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            //_tipoOperacao = TipoOperacao.Inicial;
            //ControlarTipoOperacao();
            string aluno = ObjetoOcorrencia.Aluno.ToString();
            string queryString = MontarQueryString(aluno);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            //ASPxWebControl.RedirectOnCallback("ListarOcorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            Response.Redirect("ListarOcorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void btnConfirmar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = Operacao.Confirmar;

            ControlarTipoOperacao();

        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            CR.Ly_ocorrencia dtOcorrencia = new Techne.Lyceum.CR.Ly_ocorrencia();
            ObterDados(dtOcorrencia);

            RN.RetValue retorno = null;

            if (_tipoOperacao.Equals(Operacao.Novo))
                retorno = RN.Ocorrencia.Incluir(dtOcorrencia);
            else if (_tipoOperacao.Equals(Operacao.Alterar))
                RN.Ocorrencia.Atualizar(dtOcorrencia);


            if (retorno != null)
            {
                if (!retorno.Ok)
                    lblMensagem.Text = retorno.Errors.ToString();
                else
                    lblMensagem.Text = retorno.Message;
            }

            //_tipoOperacao = Operacao.Inicial;

            string aluno = ObjetoOcorrencia.Aluno.ToString();
            string queryString = MontarQueryString(aluno);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            Response.Redirect("ListarOcorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void ddlDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(ddlAno.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue.ToString()))
            CarregarDadosDrop("ddlTurma");
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAno.SelectedValue.ToString() == "" || ddlAno.SelectedValue.ToString() == "Nulo")
            {
                ddlPeriodo.Items.Clear();
                ddlDisciplina.Items.Clear();
                ddlTurma.Items.Clear();
                CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
                CarregarDropDownList(ddlDisciplina, null, ObjetoOcorrencia.Disciplina);
                CarregarDropDownList(ddlTurma, null, ObjetoOcorrencia.Turma);
            }
            else
            {
                CarregarDadosDrop("ddlPeriodo");
                CarregarDadosDrop("ddlDisciplina");
                CarregarDadosDrop("ddlTurma");
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CarregarDadosDrop("ddlDisciplina");
        }

        #endregion


        #region Metodos

        private void ObterDadosQueryString(string queryString)
        {
            ObjetoOcorrencia = new DadosOcorrencia();
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno") >= 0)
                    ObjetoOcorrencia.Aluno = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("tipo") >= 0)
                    ObjetoOcorrencia.Tipo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("data") >= 0)
                    ObjetoOcorrencia.Data = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("ordem") >= 0)
                    ObjetoOcorrencia.Ordem = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("usuario") >= 0)
                    ObjetoOcorrencia.Usuario = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("descricao") >= 0)
                    ObjetoOcorrencia.Descricao = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("ano") >= 0)
                    ObjetoOcorrencia.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("periodo") >= 0)
                    ObjetoOcorrencia.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("disciplina") >= 0)
                    ObjetoOcorrencia.Disciplina = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("turma") >= 0)
                    ObjetoOcorrencia.Turma = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("Operacao") >= 0)
                {
                    string tipoOperacao = dados.Substring(dados.LastIndexOf('=') + 1);

                    if (tipoOperacao == "NOVO")
                        _tipoOperacao = Operacao.Novo;
                    else if (tipoOperacao == "ALTERAR")
                        _tipoOperacao = Operacao.Alterar;
                    else if (tipoOperacao == "EXCLUIR")
                        _tipoOperacao = Operacao.Excluir;
                    else if (tipoOperacao == "CONSULTAR")
                        _tipoOperacao = Operacao.Consultar;

                    lblTipoOperacao.Text = tipoOperacao;
                }
            }
        }

        /// <summary>
        /// Habilita a visibilidade dos botões de acordo com o tipo de operação
        /// </summary>
        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case Operacao.Inicial:
                    {

                        break;
                    }
                case Operacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);

                        break;
                    }
                case Operacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        tdData.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        break;
                    }
                case Operacao.Excluir:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnConfirmar };
                        ControlarVisibilidadeControle(controles);

                        break;
                    }
                case Operacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        break;
                    }
                case Operacao.Confirmar:
                    {
                        Ly_ocorrencia dtOcorrencia = new Ly_ocorrencia();
                        Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.NewRow();
                        dadosOcorrencia.Aluno = this.txtAluno.Text.ToString();
                        dadosOcorrencia.Data = Convert.ToDateTime(tdData.Date);
                        dadosOcorrencia.Tipo = this.ddlTipo.SelectedValue.ToString();
                        dadosOcorrencia.Ordem = Convert.ToDecimal(this.txtOrdem.Text.ToString());

                        RetValue retorno = null;
                        //retorno = Ocorrencia.Excluir(dadosOcorrencia);

                        string aluno = ObjetoOcorrencia.Aluno.ToString();
                        string queryString = MontarQueryString(aluno);

                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                        Response.Redirect("ListarOcorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

                        break;
                    }

            }
        }

        private void ControlarAtributosCampos()
        {
            switch (_tipoOperacao)
            {
                case Operacao.Inicial:
                    {

                        break;
                    }
                case Operacao.Consultar:
                    {
                        CarregarConsultar();

                        break;
                    }
                case Operacao.Novo:
                    {
                        CarregarNovo();

                        break;
                    }
                case Operacao.Excluir:
                    {
                        CarregarExcluir();

                        break;
                    }
                case Operacao.Alterar:
                    {
                        CarregarAlterar();

                        break;
                    }

            }
        }

        /// <summary>
        /// Carregar a tela para consulta
        /// </summary>
        private void CarregarConsultar()
        {
            lblTipoOperacao.Text = "Modo de consulta";
            string aluno = ObjetoOcorrencia.Aluno.ToString();
            this.txtAluno.Text = ObjetoOcorrencia.Aluno.ToString();
            this.txtNomeAluno.Text = RN.Aluno.ConsultarNome(ObjetoOcorrencia.Aluno.ToString());
            this.txtAluno.Enabled = false;
            this.txtNomeAluno.Enabled = false;
            DateTime data = Convert.ToDateTime(ObjetoOcorrencia.Data);
            this.tdData.Date = Convert.ToDateTime(ObjetoOcorrencia.Data);
            this.tdData.Enabled = false;
            string tipo = Convert.ToString(ObjetoOcorrencia.Tipo);
            decimal ordem = Convert.ToDecimal(ObjetoOcorrencia.Ordem);
            this.txtOrdem.Text = Convert.ToString(ObjetoOcorrencia.Ordem);
            this.txtOrdem.Enabled = false;

            Ly_ocorrencia dtOcorrencia = RN.Ocorrencia.ConsultarEditar(aluno, data, tipo, ordem);

            Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.Rows[0];

            this.txtUsuario.Text = Convert.ToString(dadosOcorrencia.Usuario);
            this.txtUsuario.ReadOnly = true;
            this.txtDescricao.Text = Convert.ToString(dadosOcorrencia.Descricao);
            this.txtDescricao.ReadOnly = true;
            if (!IsPostBack)
            {
                CarregarDadosDrop("ddlTipo");
                PreencherDadoCombo(ddlTipo, Convert.ToString(ObjetoOcorrencia.Tipo));
                //this.ddlTipo.SelectedValue = Convert.ToString(ObjetoOcorrencia.Tipo);
                this.ddlTipo.Enabled = false;
                if (!string.IsNullOrEmpty(dadosOcorrencia.Ano.ToString()))
                {
                    CarregarDadosDrop("ddlAno");
                    PreencherDadoCombo(ddlAno, Convert.ToString(dadosOcorrencia.Ano));
                    //this.ddlAno.SelectedValue = Convert.ToString(dadosOcorrencia.Ano);
                    this.ddlAno.Enabled = false;
                    CarregarDadosDrop("ddlPeriodo");
                    PreencherDadoCombo(ddlPeriodo, Convert.ToString(dadosOcorrencia.Periodo));
                    //this.ddlPeriodo.SelectedValue = Convert.ToString(dadosOcorrencia.Periodo);
                    this.ddlPeriodo.Enabled = false;
                    CarregarDadosDrop("ddlDisciplina");
                    PreencherDadoCombo(ddlDisciplina, Convert.ToString(dadosOcorrencia.Disciplina));
                    //this.ddlDisciplina.SelectedValue = Convert.ToString(dadosOcorrencia.Disciplina);
                    this.ddlDisciplina.Enabled = false;
                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue.ToString()))
                        CarregarDadosDrop("ddlTurma");
                    PreencherDadoCombo(ddlTurma, Convert.ToString(dadosOcorrencia.Turma));
                    //this.ddlTurma.SelectedValue = Convert.ToString(dadosOcorrencia.Turma);
                    this.ddlTurma.Enabled = false;
                }
                else
                {
                    CarregarDadosDrop("ddlAno");
                    CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
                    CarregarDropDownList(ddlDisciplina, null, ObjetoOcorrencia.Disciplina);
                    CarregarDropDownList(ddlTurma, null, ObjetoOcorrencia.Turma);
                    this.ddlAno.Enabled = false;
                    this.ddlPeriodo.Enabled = false;
                    this.ddlDisciplina.Enabled = false;
                    this.ddlTurma.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Carregar a tela para consulta
        /// </summary>
        private void CarregarExcluir()
        {
            lblTipoOperacao.Text = "Modo de exclusão";
            string aluno = ObjetoOcorrencia.Aluno.ToString();
            this.txtAluno.Text = ObjetoOcorrencia.Aluno.ToString();
            this.txtNomeAluno.Text = RN.Aluno.ConsultarNome(ObjetoOcorrencia.Aluno.ToString());
            this.txtAluno.Enabled = false;
            this.txtNomeAluno.Enabled = false;
            DateTime data = Convert.ToDateTime(ObjetoOcorrencia.Data);
            this.tdData.Date = Convert.ToDateTime(ObjetoOcorrencia.Data);
            this.tdData.Enabled = false;
            string tipo = Convert.ToString(ObjetoOcorrencia.Tipo);
            decimal ordem = Convert.ToDecimal(ObjetoOcorrencia.Ordem);
            this.txtOrdem.Text = Convert.ToString(ObjetoOcorrencia.Ordem);
            this.txtOrdem.Enabled = false;

            Ly_ocorrencia dtOcorrencia = RN.Ocorrencia.ConsultarEditar(aluno, data, tipo, ordem);

            Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.Rows[0];

            this.txtUsuario.Text = Convert.ToString(dadosOcorrencia.Usuario);
            this.txtUsuario.ReadOnly = true;
            this.txtDescricao.Text = Convert.ToString(dadosOcorrencia.Descricao);
            this.txtDescricao.ReadOnly = true;
            if (!IsPostBack)
            {
                CarregarDadosDrop("ddlTipo");
                PreencherDadoCombo(ddlTipo, Convert.ToString(ObjetoOcorrencia.Tipo));
                this.ddlTipo.Enabled = false;
                if (!string.IsNullOrEmpty(dadosOcorrencia.Ano.ToString()))
                {
                    CarregarDadosDrop("ddlAno");
                    PreencherDadoCombo(ddlAno, Convert.ToString(dadosOcorrencia.Ano));
                    //this.ddlAno.SelectedValue = Convert.ToString(dadosOcorrencia.Ano);
                    this.ddlAno.Enabled = false;
                    CarregarDadosDrop("ddlPeriodo");
                    PreencherDadoCombo(ddlPeriodo, Convert.ToString(dadosOcorrencia.Periodo));
                    //this.ddlPeriodo.SelectedValue = Convert.ToString(dadosOcorrencia.Periodo);
                    this.ddlPeriodo.Enabled = false;
                    CarregarDadosDrop("ddlDisciplina");
                    PreencherDadoCombo(ddlDisciplina, Convert.ToString(dadosOcorrencia.Disciplina));
                    //this.ddlDisciplina.SelectedValue = Convert.ToString(dadosOcorrencia.Disciplina);
                    this.ddlDisciplina.Enabled = false;
                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue.ToString()))
                        CarregarDadosDrop("ddlTurma");
                    PreencherDadoCombo(ddlTurma, Convert.ToString(dadosOcorrencia.Turma));
                    //this.ddlTurma.SelectedValue = Convert.ToString(dadosOcorrencia.Turma);
                    this.ddlTurma.Enabled = false;
                }
                else
                {
                    CarregarDadosDrop("ddlAno");
                    CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
                    CarregarDropDownList(ddlDisciplina, null, ObjetoOcorrencia.Disciplina);
                    CarregarDropDownList(ddlTurma, null, ObjetoOcorrencia.Turma);
                    this.ddlAno.Enabled = false;
                    this.ddlPeriodo.Enabled = false;
                    this.ddlDisciplina.Enabled = false;
                    this.ddlTurma.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Carregar a tela para novo registro
        /// </summary>
        private void CarregarNovo()
        {
            lblTipoOperacao.Text = "Modo de inclusão";
            string aluno = ObjetoOcorrencia.Aluno.ToString();
            this.txtAluno.Text = aluno;

            this.txtNomeAluno.Text = RN.Aluno.ConsultarNome(ObjetoOcorrencia.Aluno.ToString());
            this.txtAluno.Enabled = false;
            this.txtNomeAluno.Enabled = false;
            this.ddlTipo.Enabled = true;
            this.tdData.Enabled = true;
            decimal ordem = CalcularOrdem(aluno);
            this.txtOrdem.Text = Convert.ToString(ordem);
            this.txtUsuario.Text = Convert.ToString(HttpContext.Current.User.Identity.Name);
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Enabled = false;
            this.txtDescricao.ReadOnly = false;
            this.ddlAno.Enabled = true;
            this.ddlPeriodo.Enabled = true;
            this.ddlDisciplina.Enabled = true;
            this.ddlTurma.Enabled = true;
            if (!IsPostBack)
            {
                CarregarDadosDrop("ddlTipo");
                CarregarDadosDrop("ddlAno");
                CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
                CarregarDropDownList(ddlDisciplina, null, ObjetoOcorrencia.Disciplina);
                CarregarDropDownList(ddlTurma, null, ObjetoOcorrencia.Turma);
            }
        }

        /// <summary>
        /// Carregar a tela para atualização
        /// </summary>
        private void CarregarAlterar()
        {
            lblTipoOperacao.Text = "Modo de alteração";
            string aluno = ObjetoOcorrencia.Aluno.ToString();
            this.txtAluno.Text = ObjetoOcorrencia.Aluno.ToString();
            this.txtNomeAluno.Text = RN.Aluno.ConsultarNome(ObjetoOcorrencia.Aluno.ToString());
            this.txtAluno.Enabled = false;
            this.txtNomeAluno.Enabled = false;
            DateTime data = Convert.ToDateTime(ObjetoOcorrencia.Data);
            this.tdData.Date = Convert.ToDateTime(ObjetoOcorrencia.Data);
            this.tdData.Enabled = false;
            string tipo = Convert.ToString(ObjetoOcorrencia.Tipo);
            decimal ordem = Convert.ToDecimal(ObjetoOcorrencia.Ordem);
            this.txtOrdem.Text = Convert.ToString(ObjetoOcorrencia.Ordem);
            this.txtOrdem.Enabled = false;

            Ly_ocorrencia dtOcorrencia = RN.Ocorrencia.ConsultarEditar(aluno, data, tipo, ordem);

            Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.Rows[0];

            this.txtUsuario.Text = Convert.ToString(dadosOcorrencia.Usuario);
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Enabled = false;
            this.txtDescricao.Text = Convert.ToString(dadosOcorrencia.Descricao);
            this.txtDescricao.ReadOnly = false;
            this.ddlAno.Enabled = true;
            this.ddlPeriodo.Enabled = true;
            this.ddlDisciplina.Enabled = true;
            this.ddlTurma.Enabled = true;
            if (!IsPostBack)
            {
                CarregarDadosDrop("ddlTipo");
                this.ddlTipo.SelectedValue = Convert.ToString(ObjetoOcorrencia.Tipo);
                this.ddlTipo.Enabled = false;
                if (!string.IsNullOrEmpty(dadosOcorrencia.Ano.ToString()))
                {
                    CarregarDadosDrop("ddlAno");
                    PreencherDadoCombo(ddlAno, Convert.ToString(dadosOcorrencia.Ano));
                    //this.ddlAno.SelectedValue = Convert.ToString(dadosOcorrencia.Ano);
                    CarregarDadosDrop("ddlPeriodo");
                    PreencherDadoCombo(ddlPeriodo, Convert.ToString(dadosOcorrencia.Periodo));
                    //this.ddlPeriodo.SelectedValue = Convert.ToString(dadosOcorrencia.Periodo);
                    CarregarDadosDrop("ddlDisciplina");
                    PreencherDadoCombo(ddlDisciplina, Convert.ToString(dadosOcorrencia.Disciplina));
                    //this.ddlDisciplina.SelectedValue = Convert.ToString(dadosOcorrencia.Disciplina);
                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue.ToString()))
                        CarregarDadosDrop("ddlTurma");
                    PreencherDadoCombo(ddlTurma, Convert.ToString(dadosOcorrencia.Turma));
                    //this.ddlTurma.SelectedValue = Convert.ToString(dadosOcorrencia.Turma);
                }
                else
                {
                    CarregarDadosDrop("ddlAno");
                    CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
                    CarregarDropDownList(ddlDisciplina, null, ObjetoOcorrencia.Disciplina);
                    CarregarDropDownList(ddlTurma, null, ObjetoOcorrencia.Turma);
                }
            }
        }

        /// <summary>
        /// Habilita a visibilidade nos botões passados como parâmetro
        /// </summary>
        /// <param name="botoes">Array com os botões que serão visiveis</param>
        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
        }

        /// <summary>
        /// Retira a visibilidade de todos botões
        /// </summary>
        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnConfirmar.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        /// <summary>
        /// Armazena uma nova linha com os dados da tela no datatable passado como parâmetro
        /// </summary>
        /// <param name="dtOcorrencia">DataTable de ocorrencia que será adicionado uma nova linha</param>
        private void ObterDados(Ly_ocorrencia dtOcorrencia)
        {
            Techne.Lyceum.CR.Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.NewRow();

            dadosOcorrencia.Aluno = Convert.ToString(txtAluno.Text);
            dadosOcorrencia.Data = Convert.ToDateTime(tdData.Date);
            dadosOcorrencia.Tipo = ddlTipo.Text.ToString();
            dadosOcorrencia.Ordem = Convert.ToDecimal(txtOrdem.Text);
            dadosOcorrencia.Usuario = txtUsuario.Text.ToString();
            dadosOcorrencia.Descricao = txtDescricao.Text.ToString();
            //if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
            if (ddlAno.SelectedValue != "" && ddlAno.SelectedValue != "Nulo")
            {
                dadosOcorrencia.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
                dadosOcorrencia.Periodo = Convert.ToDecimal(ddlPeriodo.SelectedValue);
                dadosOcorrencia.Disciplina = Convert.ToString(ddlDisciplina.SelectedValue);
                dadosOcorrencia.Turma = Convert.ToString(ddlTurma.SelectedValue);
            }
            else
            {
                dadosOcorrencia.Ano = null;
                dadosOcorrencia.Periodo = null;
                dadosOcorrencia.Disciplina = null;
                dadosOcorrencia.Turma = null;
            }


            if (!_tipoOperacao.Equals(Operacao.Consultar))
            {
                //dadosTurma.Unidade_responsavel = ddlUnidadeResponsavel.SelectedValue;
                //dadosTurma.Dependencia = ddlDependencia.SelectedValue;

                //dadosTurma.Dt_inicio = dtIniAula.Date;
                //dadosTurma.Dt_fim = dtFimAula.Date;
                //dadosTurma.Num_alunos = Convert.ToDecimal(txtNumMaxAluno.Text);
            }


            dtOcorrencia.Rows.Add(dadosOcorrencia);
        }


        private void LimparTela()
        {
            ddlTipo.Items.Clear();
            tdData.Text = string.Empty;
            txtOrdem.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            ddlAno.Items.Clear();
            ddlPeriodo.Items.Clear();
            ddlDisciplina.Items.Clear();
            ddlTurma.Items.Clear();
            CarregarDadosDrop("ddlTipo");
            CarregarDadosDrop("ddlAno");
            CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
            CarregarDropDownList(ddlDisciplina, null, ObjetoOcorrencia.Disciplina);
            CarregarDropDownList(ddlTurma, null, ObjetoOcorrencia.Turma);
        }

        /// <summary>
        /// Calcula o campo Ordem
        /// </summary>
        /// <param name="aluno">Código do aluno</param>
        private decimal CalcularOrdem(string aluno)
        {
            decimal ordem = 0;

            QueryTable dadosOcorrencias = null;

            dadosOcorrencias = RN.Ocorrencia.ConsultarOrdem(aluno);

            string dados = dadosOcorrencias.Rows[0].ToString();
            char[] parametros = new char[] { ':' };
            string[] dadosOrdem = dados.Split(parametros, 2, StringSplitOptions.None);
            if (dadosOrdem[1].ToString() != " ")
                ordem = Convert.ToDecimal(dadosOrdem[1]);
            else
                ordem = 0;

            ordem = ordem + 1;

            return ordem;
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("<Lista Vazia>", "");
                drop.Items.Add(itemVazio);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                        drop.SelectedValue = defaultValue;
                    else
                    {
                        if (drop.ID == ddlAno.ID)
                        {
                            ListItem itemNulo = new ListItem("Nulo", null);
                            drop.Items.Add(itemNulo);
                            drop.SelectedValue = Convert.ToString(itemNulo);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
                }
            }
        }


        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {

                    case "DDLTIPO":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("TIPO OCORRENCIA");
                            if (_tipoOperacao.Equals(Operacao.Novo))
                                CarregarDropDownList(ddlTipo, dadosDrop, null);
                            else
                                CarregarDropDownList(ddlTipo, dadosDrop, ObjetoOcorrencia.Tipo);

                            break;
                        }
                    case "DDLANO":
                        {
                            string aluno = txtAluno.Text.ToString();
                            dadosDrop = RN.Ocorrencia.ConsultarAno(aluno);
                            if (_tipoOperacao.Equals(Operacao.Novo))
                                CarregarDropDownList(ddlAno, dadosDrop, null);
                            else
                                CarregarDropDownList(ddlAno, dadosDrop, ObjetoOcorrencia.Ano);

                            break;
                        }

                    case "DDLPERIODO":
                        {
                            string ano;
                            string aluno = txtAluno.Text.ToString();
                            ano = ddlAno.SelectedValue.ToString();
                            if (!string.IsNullOrEmpty(ano))
                            {
                                dadosDrop = RN.Ocorrencia.ConsultarPeriodo(ano, aluno);
                                //dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);
                                CarregarDropDownList(ddlPeriodo, dadosDrop, ObjetoOcorrencia.Periodo);
                            }
                            else
                            {
                                ano = ObjetoOcorrencia.Ano;
                                dadosDrop = RN.Ocorrencia.ConsultarPeriodo(ano, aluno);
                                CarregarDropDownList(ddlPeriodo, dadosDrop, ObjetoOcorrencia.Periodo);
                            }

                            break;
                        }
                    case "DDLDISCIPLINA":
                        {
                            string aluno;
                            string ano;
                            //decimal ano;
                            string semestre;
                            aluno = txtAluno.Text.ToString();
                            ano = ddlAno.SelectedValue.ToString();
                            semestre = ddlPeriodo.SelectedValue.ToString();
                            dadosDrop = RN.Disciplina.ConsultarPorAluno(aluno, ano, semestre);

                            CarregarDropDownList(ddlDisciplina, dadosDrop, ObjetoOcorrencia.Disciplina);

                            break;
                        }
                    case "DDLTURMA":
                        {
                            string aluno = null;
                            string disciplina = null;
                            string ano = null;
                            string semestre = null;

                            if (!string.IsNullOrEmpty(ddlDisciplina.SelectedValue))
                            {
                                aluno = txtAluno.Text.ToString();
                                disciplina = ddlDisciplina.SelectedValue;
                                ano = ddlAno.SelectedValue.ToString();
                                semestre = ddlPeriodo.SelectedValue.ToString();

                                dadosDrop = RN.Ocorrencia.ConsultarTurma(aluno, disciplina, ano, semestre);

                                CarregarDropDownList(ddlTurma, dadosDrop, ObjetoOcorrencia.Turma);
                            }

                            break;
                        }
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }

            return dadosDrop;
        }


        private string MontarQueryString(string aluno)
        {
            string queryString = string.Empty;

            queryString += "aluno=" + aluno;

            return queryString;
        }

        private void RedirecionarPagina()
        {
            HttpContext.Current.Items.Add("chave", "1000");

            Server.Transfer("ListarOcorrencias.aspx");
        }

        #endregion

    }
}