using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Lyceum.RN.InspecaoEscolar.DTOs;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/Relatorio.aspx"),
    ControlText("Relatorio"),
    Title("Relatório"),]
    public partial class Relatorio : TPage
    {
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

        #region Variaveis_Globais

        private readonly RN.InspecaoEscolar.Campanha CampanhaRN;
        private readonly RN.InspecaoEscolar.CampanhaEscola CampanhaEscolaRN;
        private readonly RN.InspecaoEscolar.Grupo GrupoRN;
        private readonly RN.InspecaoEscolar.TipoAssunto TipoAssuntoRN;
        private readonly RN.InspecaoEscolar.Assunto AssuntoRN;
        private readonly RN.InspecaoEscolar.OpcoesAssunto OpcoesAssuntoRN;
        private readonly RN.InspecaoEscolar.AcaoDirecao AcaoDirecaoRN;
        private readonly RN.InspecaoEscolar.IdentificacaoDependencia IdentificacaoDependenciaRN;
        private readonly RN.InspecaoEscolar.RespostaDependencia RespostaDependenciaRN;

        private RN.InspecaoEscolar.Entidades.OpcoesAssunto OpcoesAssuntoDados;
        private RN.InspecaoEscolar.Entidades.CampanhaEscola CampanhaEscolaDados;

        private ValidacaoDados validacao;
        DataTable dtPerguntas2 = null;

        #endregion



        public Relatorio()
        {
            CampanhaRN = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();
            CampanhaEscolaRN = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
            GrupoRN = new Techne.Lyceum.RN.InspecaoEscolar.Grupo();
            AssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
            TipoAssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.TipoAssunto();
            OpcoesAssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
            OpcoesAssuntoDados = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.OpcoesAssunto();
            CampanhaEscolaDados = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
            AcaoDirecaoRN = new Techne.Lyceum.RN.InspecaoEscolar.AcaoDirecao();
            IdentificacaoDependenciaRN = new Techne.Lyceum.RN.InspecaoEscolar.IdentificacaoDependencia();
            RespostaDependenciaRN = new Techne.Lyceum.RN.InspecaoEscolar.RespostaDependencia();

            validacao = new ValidacaoDados();
        }

        public object ListarAcervo(object campanhaEscolaId)
        {
            RN.InspecaoEscolar.Acervo rnAcervo = new Techne.Lyceum.RN.InspecaoEscolar.Acervo();

            if (campanhaEscolaId != null)
            {
                if (!string.IsNullOrEmpty(campanhaEscolaId.ToString()))
                {
                    return rnAcervo.ListaPor(Convert.ToInt32(campanhaEscolaId));
                }
            }
            return null;
        }

        public object ListarMedida()
        {
            RN.InspecaoEscolar.Medida rnMedida = new Techne.Lyceum.RN.InspecaoEscolar.Medida();

            return rnMedida.ListaMedida();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAcervo, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                ControlaAcesso(btnSalvar, AcaoControle.novo);
                ControlaAcesso(btnSalvar_BanheiroeVestiarios, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG1A1, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG1A2, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG1A3, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG1A4, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG1A5, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG1A6, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG2A1, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG3A1, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG3A2, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG4A1, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG4A2, AcaoControle.novo);
                ControlaAcesso(btnDemaisDependenciasG5A1, AcaoControle.novo);
                ControlaAcesso(btnConsideracao, AcaoControle.novo);
                ControlaAcesso(btnEditar, AcaoControle.novo);
                ControlaAcesso(btnFinalizar, AcaoControle.novo);

                if (Page.IsPostBack)
                {
                    if ((tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
                        && (tseCampanha.IsValidDBValue && !tseCampanha.DBValue.IsNull))
                    {
                        CarregaConsideracoes();
                        ManterConsideracao();

                        CriaDemaisdependencias();
                        ManterDependencia();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCampanha_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                pcRelatorio.Visible = false;

                if (!this.tseCampanha.DBValue.IsNull)
                {
                    if (!this.tseCampanha.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Campanha não cadastrada.";

                    }
                    else
                    {
                        //Verifica se unidade foi preenchida 
                        if (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue)
                        {
                            _tipoOperacao = TipoOperacao.Consultar;
                            ControlarTipoOperacao();
                        }
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma campanha.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                tseCampanha.ResetValue();
                pcRelatorio.Visible = false;
                _tipoOperacao = TipoOperacao.Inicial;
                if (!this.tseUnidade.DBValue.IsNull)
                {
                    if (!this.tseUnidade.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";

                    }
                    else
                    {
                        //Verifica se Campanha foi preenchida 
                        if (!this.tseCampanha.DBValue.IsNull && this.tseCampanha.IsValidDBValue)
                        {
                            _tipoOperacao = TipoOperacao.Consultar;

                        }
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Editar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnFinalizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Finalizar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Cancelar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rpSalasdeAulas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                //a cada linha que eu coloco as salas de aula, eu crio todas as perguntas


                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    string guid = System.Guid.NewGuid().ToString();
                    DropDownList rblPlacaIdentificao = ((DropDownList)e.Item.FindControl("rblPlacaIdentificao"));
                    rblPlacaIdentificao.Attributes["data-guid"] = guid;

                    Repeater perguntas = (Repeater)e.Item.FindControl("rpPerguntas");

                    if (perguntas != null && dtPerguntas2 != null)
                    {
                        perguntas.DataSource = dtPerguntas2;
                        perguntas.DataBind();

                        foreach (RepeaterItem i in perguntas.Items)
                        {
                            guid = System.Guid.NewGuid().ToString();
                            DropDownList ddlPerguntas = ((DropDownList)i.FindControl("ddlPergunta"));
                            ddlPerguntas.Attributes["data-guid"] = guid;
                        }

                        //guid = System.Guid.NewGuid().ToString();
                        //RadioButtonList rblPlacaIdentificao = ((RadioButtonList)perguntas.Controls[0].FindControl("rblPlacaIdentificao"));
                        //rblPlacaIdentificao.Attributes["data-guid2"] = guid;
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
            }
            catch (Exception)
            {

                lblMensagem.Text = "Erro ao buscar as perguntas.";
            }

        }

        public List<RN.InspecaoEscolar.dependencias_SaleBan> BuscaPergunta()
        {
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;
            ASPxPageControl control = MainContent.FindControl("pcRelatorio") as ASPxPageControl;

            List<RN.InspecaoEscolar.dependencias_SaleBan> l_dependencias = new List<RN.InspecaoEscolar.dependencias_SaleBan>();

            Repeater saladeaulaRepeater = (Repeater)control.FindControl("rpSalasdeAulas");
            Repeater perguntaRepeater;

            //pega as linhas
            if (saladeaulaRepeater != null)
            {
                // para cada sala de aula(linha)
                for (int i = 0; i < saladeaulaRepeater.Items.Count; i++)
                {
                    RN.InspecaoEscolar.dependencias_SaleBan dependenciaRN = new RN.InspecaoEscolar.dependencias_SaleBan();

                    //pega todas as informações da dependencia

                    dependenciaRN.faculdade = tseUnidade.Value.ToString();

                    Label ldependencia = (Label)saladeaulaRepeater.Items[i].FindControl("lblNSala");
                    dependenciaRN.dependencia = ldependencia.Text;//AQUI TEM O NOME DA DEPENDENCIA

                    DropDownList rblPlacaIdentificao = (DropDownList)saladeaulaRepeater.Items[i].FindControl("rblPlacaIdentificao");
                    dependenciaRN.placadeIdentificacao = Request.Form[rblPlacaIdentificao.Attributes["data-guid"]] == "1" ? true : false;  //dependenciaRN.placadeIdentificacao = rblPlacaIdentificao.SelectedValue == "1" ? true : false;

                    //RadioButtonList rrblPlacaIdentificao = (RadioButtonList)saladeaulaRepeater.Items[i].FindControl("rblPlacaIdentificao");
                    //if (rrblPlacaIdentificao != null)
                    //{
                    //    if (rrblPlacaIdentificao.SelectedItem.Value == "1")
                    //    {
                    //        dependenciaRN.placadeIdentificacao = true;
                    //    }
                    //    else
                    //    {
                    //        dependenciaRN.placadeIdentificacao = false;
                    //    }
                    //}
                    //else
                    //{
                    //    throw new Exception("Erro ao obter placa de identificação.");
                    //}


                    dependenciaRN.identificacaodependencia = null;


                    if (saladeaulaRepeater.Items[i].ItemType == ListItemType.Item || saladeaulaRepeater.Items[i].ItemType == ListItemType.AlternatingItem)
                    {
                        // pego a pergunta
                        perguntaRepeater = (Repeater)saladeaulaRepeater.Items[i].FindControl("rpPerguntas");
                        if (perguntaRepeater != null)
                        {

                            for (int x = 0; x < perguntaRepeater.Items.Count; x++)
                            {

                                //criar uma lista com as respostas e os ids de cada pergunta para fazer um transaction e com insert ou update
                                DropDownList ddlpergunta = (DropDownList)perguntaRepeater.Items[x].FindControl("ddlPergunta");

                                //CPID17&ASTID25&GRPID32&OPASTIDID23
                                //CPID17&ASTID25&GRPID32&OPASTIDID23
                                string[] codigo = ddlpergunta.Attributes["data-nome"].Split('&');

                                //serve para cadastrar as perguntas de cada dependência
                                RN.InspecaoEscolar.perguntas pergunta = new RN.InspecaoEscolar.perguntas();

                                pergunta.campanhaid = codigo[0].ToString().Substring(4);
                                pergunta.assuntoid = codigo[1].ToString().Substring(5);
                                pergunta.grupoid = codigo[2].ToString().Substring(5);
                                pergunta.opassuntoid = codigo[3].ToString().Substring(9);
                                //pego a resposta da pergunta
                                pergunta.respostaid = Request.Form[ddlpergunta.Attributes["data-guid"]];  //pergunta.respostaid = ddlpergunta.SelectedItem.Value;

                                dependenciaRN.l_perguntas.Add(pergunta);
                            }
                        }

                    }
                    l_dependencias.Add(dependenciaRN);
                }
            }
            else
            {
                l_dependencias = null;
            }
            return l_dependencias;
        }

        #region ControlaVisibilidade

        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Inicial,
            Consultar,
            Sucesso,
            Editar,
            Finalizar
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

        private void RetiraVisibilidadeBotao()
        {
            btnEditar.Visible = false;
            btnCancelar.Visible = false;
            btnConsideracao.Visible = false;
            btnFinalizar.Visible = false;
            btnAcervo.Visible = false;
            btnSalvar.Visible = false;
            btnSalvar_BanheiroeVestiarios.Visible = false;
            btnDemaisDependenciasG1A1.Visible = false;
            btnDemaisDependenciasG1A2.Visible = false;
            btnDemaisDependenciasG1A3.Visible = false;
            btnDemaisDependenciasG1A4.Visible = false;
            btnDemaisDependenciasG1A5.Visible = false;
            btnDemaisDependenciasG1A6.Visible = false;
            btnDemaisDependenciasG2A1.Visible = false;
            btnDemaisDependenciasG3A1.Visible = false;
            btnDemaisDependenciasG3A2.Visible = false;
            btnDemaisDependenciasG4A1.Visible = false;
            btnDemaisDependenciasG4A2.Visible = false;
            btnDemaisDependenciasG5A1.Visible = false;


        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var img in imgBotoes)
            {
                img.Visible = true;
            }
            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void ControlarTipoOperacao()
        {
            RN.InspecaoEscolar.Campanha rnCampanha = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();

            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        pcRelatorio.Visible = false;
                        pcRelatorio.Enabled = false;
                        tseUnidade.Mode = ControlMode.Edit;
                        tseCampanha.Mode = ControlMode.Edit;
                        pnlConsideracao.Enabled = false;
                        pnlDemaisDependenciasG1A1.Enabled = false;
                        pnlDemaisDependenciasG1A2.Enabled = false;
                        pnlDemaisDependenciasG1A3.Enabled = false;
                        pnlDemaisDependenciasG1A4.Enabled = false;
                        pnlDemaisDependenciasG1A5.Enabled = false;
                        pnlDemaisDependenciasG1A6.Enabled = false;
                        pnlDemaisDependenciasG2A1.Enabled = false;
                        pnlDemaisDependenciasG3A1.Enabled = false;
                        pnlDemaisDependenciasG3A2.Enabled = false;
                        pnlDemaisDependenciasG4A1.Enabled = false;
                        pnlDemaisDependenciasG4A2.Enabled = false;
                        pnlDemaisDependenciasG5A1.Enabled = false;
                        pnlBanheiroVestiario.Enabled = false;
                        pnlSalaAula.Enabled = false;
                        pnlAcervo.Enabled = false;
                        rblPossuiAcervo.ClearSelection();
                        LimpaDadosAcervo();
                        PreecherComboTabGeral(ddlTipoInstituicao, RN.Util.Cache.TipoInstituicao);
                        CarregaMedida();
                        hdnCampanhaEscolaId.Value = string.Empty;
                        hdnAcervoId.Value = string.Empty;
                        odsAcervo.Select();
                        odsAcervo.DataBind();
                        grdAcervo.DataBind();
                        pcRelatorio.ActiveTabIndex = 0;
                        pcRelatorio.TabPages[13].ClientVisible = false; //Acervo
                        break;

                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] imgControles = new ImageButton[] { btnEditar, btnFinalizar };
                        Button[] controles = new Button[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        pcRelatorio.Visible = true;

                        break;
                    }
                case TipoOperacao.Editar:
                    {
                        ImageButton[] imgControles = new ImageButton[] { btnCancelar, btnFinalizar };
                        Button[] controles = new Button[] { btnConsideracao, btnAcervo, btnSalvar, btnSalvar_BanheiroeVestiarios,
                        btnDemaisDependenciasG1A1,
                        btnDemaisDependenciasG1A2,
                        btnDemaisDependenciasG1A3,
                        btnDemaisDependenciasG1A4,
                        btnDemaisDependenciasG1A5,
                        btnDemaisDependenciasG1A6,
                        btnDemaisDependenciasG2A1,
                        btnDemaisDependenciasG3A1,
                        btnDemaisDependenciasG3A2,
                        btnDemaisDependenciasG4A1,
                        btnDemaisDependenciasG4A2,
                        btnDemaisDependenciasG5A1
                        };
                        ControlarVisibilidadeControle(imgControles, controles);
                        pcRelatorio.Visible = true;
                        pcRelatorio.Enabled = true;
                        tseUnidade.Mode = ControlMode.View;
                        tseCampanha.Mode = ControlMode.View;
                        pnlConsideracao.Enabled = true;
                        pnlDemaisDependenciasG1A1.Enabled = true;
                        pnlDemaisDependenciasG1A2.Enabled = true;
                        pnlDemaisDependenciasG1A3.Enabled = true;
                        pnlDemaisDependenciasG1A4.Enabled = true;
                        pnlDemaisDependenciasG1A5.Enabled = true;
                        pnlDemaisDependenciasG1A6.Enabled = true;
                        pnlDemaisDependenciasG2A1.Enabled = true;
                        pnlDemaisDependenciasG3A1.Enabled = true;
                        pnlDemaisDependenciasG3A2.Enabled = true;
                        pnlDemaisDependenciasG4A1.Enabled = true;
                        pnlDemaisDependenciasG4A2.Enabled = true;
                        pnlDemaisDependenciasG5A1.Enabled = true;
                        pnlBanheiroVestiario.Enabled = true;
                        pnlAcervo.Enabled = true;
                        pnlSalaAula.Enabled = true;
                        grdAcervo.Columns[0].Visible = true;
                        pnlDadosAcervo.Visible = false;
                        //CriarSaladeAula();
                        //CriarBanheiro();

                        if (rblPossuiAcervo.SelectedValue == "S")
                        {
                            pnlDadosAcervo.Visible = true;
                        }

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                        RN.InspecaoEscolar.Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();
                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        imgControles = new ImageButton[] { btnEditar, btnFinalizar };
                        pcRelatorio.ActiveTabIndex = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            pcRelatorio.TabPages[i].Enabled = true;
                        }

                        hdnCampanhaEscolaId.Value = string.Empty;
                        rblPossuiAcervo.ClearSelection();
                        LimpaDadosAcervo();
                        odsAcervo.Select();
                        odsAcervo.DataBind();
                        grdAcervo.DataBind();

                        pnlGridAcervo.Visible = false;
                        pcRelatorio.TabPages[13].ClientVisible = false; //Acervo

                        campanhaEscola = rnCampanhaEscola.ObtemPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        hdnCampanhaEscolaId.Value = campanhaEscola.CampanhaEscolaId == null ? string.Empty : campanhaEscola.CampanhaEscolaId.ToString();

                        //Verifica se a campanha / escola já possui informação de acervo
                        bool exibeAbaInspecaoEscolar = rnCampanha.ExibeInspecaoEscolar(Convert.ToInt32(tseCampanha.DBValue));
                        if (exibeAbaInspecaoEscolar)
                        {
                            pcRelatorio.TabPages[13].ClientVisible = true; //Acervo
                        }

                        //Verifica se a campanha / escola já possui informação de acervo
                        if (campanhaEscola.CampanhaEscolaId != 0)
                        {
                            if (campanhaEscola.Finalizado != null && Convert.ToBoolean(campanhaEscola.Finalizado))
                            {
                                imgControles = new ImageButton[] { };
                                lblMensagem.Text = string.Format(@"Relatório de Trabalho de Infraestrutura foi finalizado em {0}", campanhaEscola.DataFinalizacao);
                            }

                            if (campanhaEscola.PossuiAcervo != null)
                            {
                                rblPossuiAcervo.SelectedValue = campanhaEscola.PossuiAcervo.Value ? "S" : "N";
                                rblPossuiAcervo_SelectedIndexChanged(null, null);
                            }

                            if (campanhaEscola.Aceito != null && Convert.ToBoolean(campanhaEscola.Aceito))
                            {
                                imgControles = new ImageButton[] { };
                                lblMensagem.Text = string.Format(@"Relatório de Trabalho de Infraestrutura foi aceito pela Direção da Unidade Escolar em {0} e não poderá mais ser alterado.", campanhaEscola.DataAceite);
                            }
                        }
                        else
                        {
                            tseUnidade.Mode = ControlMode.Edit;
                            tseCampanha.Mode = ControlMode.Edit;
                        }

                        ControlarVisibilidadeControle(imgControles, controles);

                        pcRelatorio.Visible = true;
                        pcRelatorio.Enabled = true;
                        pnlConsideracao.Enabled = false;
                        pnlDemaisDependenciasG1A1.Enabled = false;
                        pnlDemaisDependenciasG1A2.Enabled = false;
                        pnlDemaisDependenciasG1A3.Enabled = false;
                        pnlDemaisDependenciasG1A4.Enabled = false;
                        pnlDemaisDependenciasG1A5.Enabled = false;
                        pnlDemaisDependenciasG1A6.Enabled = false;
                        pnlDemaisDependenciasG2A1.Enabled = false;
                        pnlDemaisDependenciasG3A1.Enabled = false;
                        pnlDemaisDependenciasG3A2.Enabled = false;
                        pnlDemaisDependenciasG4A1.Enabled = false;
                        pnlDemaisDependenciasG4A2.Enabled = false;
                        pnlDemaisDependenciasG5A1.Enabled = false;
                        pnlBanheiroVestiario.Enabled = false;
                        pnlSalaAula.Enabled = false;
                        pnlAcervo.Enabled = false;                        

                        CarregaConsideracoes();
                        ManterConsideracao();

                        CriaDemaisdependencias();
                        ManterDependencia();

                        grdAcervo.Columns[0].Visible = false;

                        if (campanhaEscola.PossuiAcervo != null)
                        {
                            if (campanhaEscola.PossuiAcervo.Value)
                            {
                                pnlGridAcervo.Visible = true;
                            }
                        }

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] imgControles = new ImageButton[] { btnEditar, btnFinalizar };
                        Button[] controles = new Button[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        pcRelatorio.Visible = true;
                        pnlConsideracao.Enabled = false;
                        pnlDemaisDependenciasG1A1.Enabled = false;
                        pnlDemaisDependenciasG1A2.Enabled = false;
                        pnlDemaisDependenciasG1A3.Enabled = false;
                        pnlDemaisDependenciasG1A4.Enabled = false;
                        pnlDemaisDependenciasG1A5.Enabled = false;
                        pnlDemaisDependenciasG1A6.Enabled = false;
                        pnlDemaisDependenciasG2A1.Enabled = false;
                        pnlDemaisDependenciasG3A1.Enabled = false;
                        pnlDemaisDependenciasG3A2.Enabled = false;
                        pnlDemaisDependenciasG4A1.Enabled = false;
                        pnlDemaisDependenciasG4A2.Enabled = false;
                        pnlDemaisDependenciasG5A1.Enabled = false;
                        pnlBanheiroVestiario.Enabled = false;
                        pnlAcervo.Enabled = false;
                        pnlSalaAula.Enabled = false;
                        tseUnidade.Mode = ControlMode.Edit;
                        tseCampanha.Mode = ControlMode.Edit;


                        break;
                    }
                case TipoOperacao.Finalizar:
                    {
                        RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                        RN.InspecaoEscolar.Entidades.CampanhaEscola campanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.CampanhaEscola();

                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        imgControles = new ImageButton[] { btnEditar, btnFinalizar };

                        ControlarVisibilidadeControle(imgControles, controles);
                        pnlConsideracao.Enabled = false;
                        pnlDemaisDependenciasG1A1.Enabled = false;
                        pnlDemaisDependenciasG1A2.Enabled = false;
                        pnlDemaisDependenciasG1A3.Enabled = false;
                        pnlDemaisDependenciasG1A4.Enabled = false;
                        pnlDemaisDependenciasG1A5.Enabled = false;
                        pnlDemaisDependenciasG1A6.Enabled = false;
                        pnlDemaisDependenciasG2A1.Enabled = false;
                        pnlDemaisDependenciasG3A1.Enabled = false;
                        pnlDemaisDependenciasG3A2.Enabled = false;
                        pnlDemaisDependenciasG4A1.Enabled = false;
                        pnlDemaisDependenciasG4A2.Enabled = false;
                        pnlDemaisDependenciasG5A1.Enabled = false;


                        pnlBanheiroVestiario.Enabled = false;
                        pnlSalaAula.Enabled = false;
                        pnlAcervo.Enabled = false;

                        campanhaEscola.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                        campanhaEscola.CampanhaId = (!this.tseCampanha.DBValue.IsNull && this.tseCampanha.IsValidDBValue) ? Convert.ToInt32(tseCampanha.DBValue) : -1;
                        campanhaEscola.DataFinalizacao = DateTime.Now;
                        campanhaEscola.Finalizado = true;
                        campanhaEscola.Unidade_Ens = (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue) ? tseUnidade.DBValue.ToString() : null;
                        campanhaEscola.UsuarioId = User.Identity.Name;

                        validacao = rnCampanhaEscola.ValidaFinalizacao(campanhaEscola, true);

                        if (validacao.Valido)
                        {
                            rnCampanhaEscola.Finaliza(campanhaEscola);
                            imgControles = new ImageButton[] { };
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Finalização realizada com sucesso.')", true);
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                        ControlarVisibilidadeControle(imgControles, controles);

                        break;
                    }
            }
        }

        public void PreecherComboTabGeral(DropDownList combo, string tabela)
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            combo.Items.Clear();
            combo.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(tabela);
            combo.DataBind();
            combo.Items.Insert(0, item);
        }

        #endregion

        #region Consideracoes


        private void CarregaConsideracoes()
        {
            try
            {
                RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupoConsideracao = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();

                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaConsideracoesFinaisPor(Convert.ToInt32(tseCampanha.DBValue));

                rpGrupoConsideracao.DataSource = lsGrupoConsideracao;
                rpGrupoConsideracao.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rpGrupoConsideracao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem itemRep = e.Item;


            if ((itemRep.ItemType == ListItemType.Item) ||

                (itemRep.ItemType == ListItemType.AlternatingItem))
            {

            }
        }

        protected void rpAssuntoConsideracao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {

                HiddenField hdnTipoAssunto = (HiddenField)item.FindControl("hdnTipoAssuntoTipo");

                if (hdnTipoAssunto != null)
                {
                    var valores = hdnTipoAssunto.Value.Split('&');
                    string assunto = valores[0];
                    string tipo = valores[1];

                    TextBox txt = (TextBox)item.FindControl("txtResposta");
                    RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupo");
                    CheckBoxList chk = (CheckBoxList)item.FindControl("chkResposta");

                    txt.Visible = false;
                    rbl.Visible = false;
                    chk.Visible = false;

                    if (tipo == "9")
                    {
                        chk.Visible = true;
                        txt.Visible = false;
                        rbl.Visible = false;
                    }
                    else if (tipo == "8")
                    {
                        txt.Visible = true;
                        rbl.Visible = false;
                        chk.Visible = false;
                    }
                    else if (tipo == "10")
                    {
                        rbl.Visible = true;
                        txt.Visible = false;
                        chk.Visible = false;
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

        protected void btnConsideracao_Click(object sender, EventArgs e)
        {
            try
            {
                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();

                foreach (RepeaterItem itemG in rpGrupoConsideracao.Items)
                {
                    Repeater rpAssuntoConsideracao = (Repeater)itemG.FindControl("rpAssuntoConsideracao");

                    foreach (RepeaterItem item in rpAssuntoConsideracao.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)item.FindControl("hdnTipoAssuntoTipo");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];

                        if (tipo != "11")
                        {
                            resposta.AssuntoId = Convert.ToInt32(assunto);
                            resposta.AcaoDirecaoId = null;//Não se aplica
                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                            resposta.UsuarioId = User.Identity.Name;

                            if (tipo == "9")
                            {
                                CheckBoxList chk = (CheckBoxList)item.FindControl("chkResposta");

                                foreach (ListItem it in chk.Items)
                                {
                                    resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                    if (it.Selected)
                                    {
                                        var idOpcao = it.Value.Split('&');

                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                        resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                        resposta.Descricao = null;
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.AcaoDirecaoId = null;//Não se aplica
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;
                                        respostaAssunto.Add(resposta);
                                    }
                                }
                            }
                            else if (tipo == "10")
                            {
                                RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupo");
                                if (rbl.SelectedItem != null)
                                {
                                    var opcao = rbl.SelectedItem;
                                    var idOpcao = opcao.Value;
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                    resposta.Descricao = null;
                                    respostaAssunto.Add(resposta);
                                }
                            }
                            else if (tipo == "8") //resposta descritiva do considerações Gerais
                            {
                                TextBox txtResposta = (TextBox)item.FindControl("txtResposta");
                                resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                //Inserido por francisco Monteiro em 29/01/2020
                                //Solução de contorno para trazer o texto que o usuário digitou quando ultrapassa o 
                                //limite de caracteres e a função manter trazia o que estava no banco anteriormente.
                                //Pega o conteúdo digitado e joga em uma variável do tipo Hidden para usar no manter

                                hdnConsideracoesGerais.Value = resposta.Descricao;

                                resposta.OpcoesAssuntoId = null;

                                if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                {
                                    respostaAssunto.Add(resposta);
                                }
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    hdnCampanhaEscolaId.Value = resposta.CampanhaEscolaId.ToString();
                    CarregaConsideracoes();
                    ManterConsideracao();
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Considerações Finais foi salvo com sucesso!");

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Considerações realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }
            }
            catch (Exception ex)
            {
                ManterConsideracao();
                lblMensagem.Text = ex.Message;
            }
        }

        private void ManterConsideracao()
        {
            try
            {
                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> lsRespostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();

                lsRespostaAssunto = rnRespostaAssunto.ListaRespostaConsideracoesFinaisPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());


                foreach (RepeaterItem itemG in rpGrupoConsideracao.Items)
                {
                    Repeater rpAssuntoConsideracao = (Repeater)itemG.FindControl("rpAssuntoConsideracao");

                    if (lsRespostaAssunto.Count > 0)
                    {
                        foreach (RepeaterItem item in rpAssuntoConsideracao.Items)
                        {
                            HiddenField hdnTipoAssunto = (HiddenField)item.FindControl("hdnTipoAssuntoTipo");
                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];

                            if (tipo == "8")
                            {

                                TextBox txtResposta = (TextBox)item.FindControl("txtResposta");
                                txtResposta.Text = lsRespostaAssunto.Where(x => x.AssuntoId == assunto).Select(x => x.Descricao).FirstOrDefault();
                                //Inserido por francisco Monteiro em 29/01/2020
                                //Solução de contorno para trazer o texto que o usuário digitou quando ultrapassa o 
                                //limite de caracteres e a função manter trazia o que estava no banco anteriormente.
                                if (lblMensagem.Text.Contains("ultrapassou 500 caracteres"))
                                    txtResposta.Text = hdnConsideracoesGerais.Value.ToString();
                            }
                            else if (tipo == "9")
                            {
                                CheckBoxList chk = (CheckBoxList)item.FindControl("chkResposta");

                                foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                {
                                    var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                    chk.Items.FindByValue(valor).Selected = true;
                                }
                            }
                            else if (tipo == "10")
                            {
                                RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupo");

                                foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                {
                                    rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Demais Dependencias

        public void CriaDemaisdependencias()
        {

            try
            {
                RN.InspecaoEscolar.CampanhaEscola rnCampanhaEscola = new Techne.Lyceum.RN.InspecaoEscolar.CampanhaEscola();
                RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupoConsideracao = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();

                int ordemGrupo = 0;
                int total = 0;
                int media = 0;
                int ordemAssuntoInicio = 0;
                int ordemAssuntoFim = 0;
                int campanhaId = Convert.ToInt32(tseCampanha.Value) != null ? Convert.ToInt32(tseCampanha.Value) : -1;

                //lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue));

                //IMPORTANTE: SE MUDAR AS ORDENS DE INICIO E FIM DE QQ ABA, TEM Q MUDAR NA QUERY DE VALIDAÇÃO 
                //GRUPO 1 ABA1 - Assuntos 3 a 6
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 1, 3, 6);

                rpdemaisdependenciasGrupoG1A1.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG1A1.DataBind();

                //GRUPO 1 ABA2 - Assuntos 7 a 10
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 1, 7, 10);

                rpdemaisdependenciasGrupoG1A2.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG1A2.DataBind();

                //GRUPO 1 ABA3 - Assuntos 10 a 14
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 1, 11, 14);

                rpdemaisdependenciasGrupoG1A3.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG1A3.DataBind();

                //GRUPO 1 ABA4 - Assuntos 15 a 18
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 1, 15, 18);

                rpdemaisdependenciasGrupoG1A4.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG1A4.DataBind();

                //GRUPO 1 ABA5 - Assuntos 19 a 23
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 1, 19, 23);

                rpdemaisdependenciasGrupoG1A5.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG1A5.DataBind();

                //GRUPO 1 ABA6 - Assuntos da 24 até o final
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 1, 24, 0);

                rpdemaisdependenciasGrupoG1A6.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG1A6.DataBind();

                //GRUPO 2 ABA1 - Assuntos todos
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 2, 1, 0);

                rpdemaisdependenciasGrupoG2A1.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG2A1.DataBind();

                //GRUPO 3 
                //Verifica quantas questoes existem no grupo
                ordemGrupo = 3;
                total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(campanhaId, ordemGrupo);
                media = total / 2;

                //GRUPO 3 ABA1 - Assuntos 1 a media
                ordemAssuntoInicio = 1;
                ordemAssuntoFim = media;
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim);

                rpdemaisdependenciasGrupoG3A1.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG3A1.DataBind();

                //GRUPO 3 ABA2 - Assuntos da media + 1 até o final
                ordemAssuntoInicio = media + 1;
                ordemAssuntoFim = 0;
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim);

                rpdemaisdependenciasGrupoG3A2.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG3A2.DataBind();

                //GRUPO 4
                //Verifica quantas questoes existem no grupo
                ordemGrupo = 4;
                total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(campanhaId, ordemGrupo);
                media = total / 2;

                //GRUPO 4 ABA1 - Assuntos 1 a media
                ordemAssuntoInicio = 1;
                ordemAssuntoFim = media;
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim);

                rpdemaisdependenciasGrupoG4A1.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG4A1.DataBind();

                //GRUPO 4 ABA2 - Assuntos da media + 1 até o final
                ordemAssuntoInicio = media + 1;
                ordemAssuntoFim = 0;
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim);

                rpdemaisdependenciasGrupoG4A2.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG4A2.DataBind();

                //GRUPO 5 ABA1 - Assuntos todos
                lsGrupoConsideracao.Clear();
                lsGrupoConsideracao = rnCampanhaEscola.ObtemListaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), 5, 1, 0);

                rpdemaisdependenciasGrupoG5A1.DataSource = lsGrupoConsideracao;
                rpdemaisdependenciasGrupoG5A1.DataBind();
            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }
        }

        private void ManterDependencia()
        {
            // Método para trazer respostas da perguntas para tela
            try
            {

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> lsRespostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();

                lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());


                #region GRUPO1

                //GRUPO 1 - ABA1
                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //  lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A1");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A1");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A1");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A1");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A1");
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
                //GRUPO1 -  ABA1 

                //GRUPO 1 - ABA2

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A2.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A2");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //  lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A2");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A2");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A2");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A2");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A2");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A2");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A2");
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
                //GRUPO1 -  ABA2 

                //GRUPO 1 - ABA3
                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A3.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A3");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //  lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A3");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A3");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A3");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A3");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A3");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A3");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A3");
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
                //GRUPO1 -  ABA1 

                //GRUPO 1 - ABA4

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A4.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A4");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //    lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A4");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A4");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A4");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A4");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A4");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A4");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A4");
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
                //GRUPO1 -  ABA3 
                //GRUPO 1 - ABA5

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A5.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A5");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //       lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A5");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A5");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A5");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A5");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A5");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A5");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A5");
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
                //GRUPO1 -  ABA5 

                //GRUPO 1 - ABA6

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A6.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A6");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //   lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A6");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A6");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A6");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A6");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A6");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A6");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A6");
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
                //GRUPO1 -  ABA6



                #endregion

                #region GRUPO2


                //GRUPO 2 - ABA1

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG2A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG2A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //     lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG2A1");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG2A1");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG2A1");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG2A1");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG2A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG2A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG2A1");
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
                //GRUPO2 -  ABA1
                #endregion

                #region GRUPO3

                //GRUPO3 - ABA1
                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG3A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG3A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

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
                        // lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

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

                #region GRUPO4

                //GRUPO4 - ABA1
                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG4A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG4A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //     lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG4A1");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG4A1");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG4A1");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG4A1");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG4A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG4A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG4A1");
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
                //GRUPO4 -  ABA1 

                //GRUPO4 - ABA2
                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG4A2.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG4A2");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //  lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG4A2");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG4A2");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG4A2");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG4A2");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG4A2");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG4A2");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG4A2");
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
                //GRUPO4 -  ABA2 

                #endregion

                #region GRUPO5

                //GRUPO5 - ABA1
                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG5A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG5A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        //     lsRespostaAssunto = rnRespostaAssunto.ListaRespostaDemaisDependenciasPor(Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                        if (lsRespostaAssunto.Count > 0)
                        {

                            HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG5A1");

                            var valores = hdnTipoAssunto.Value.Split('&');
                            int assunto = Convert.ToInt32(valores[0]);
                            string tipo = valores[1];
                            bool acaodirecao = Convert.ToBoolean(valores[2]);

                            if (acaodirecao)//Ação de Direção
                            {
                                Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG5A1");

                                foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                                {
                                    HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG5A1");

                                    DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG5A1");

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

                                        RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG5A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            rbl.Items.FindByValue(linha.OpcoesAssuntoId.ToString()).Selected = true;
                                        }

                                        break;

                                    case 3:

                                        CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG5A1");

                                        foreach (var linha in lsRespostaAssunto.Where(x => x.AssuntoId == assunto))
                                        {
                                            var valor = linha.OpcoesAssuntoId.ToString() + "&" + linha.AssuntoId.ToString();
                                            chk.Items.FindByValue(valor).Selected = true;
                                        }

                                        break;

                                    case 4:

                                        TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG5A1");
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
                //GRUPO5 -  ABA1 

                #endregion
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #region GRUPO1

        //GRUPO1 - ABA1

        protected void rpdemaisdependenciasGrupoG1A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG1A1");

                if (assuntoRepeater != null)
                {
                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG1A1");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();

                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 3, 6);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);
                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();
                }
            }
        }

        protected void rpdemaisdependenciasAssuntoG1A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Aba Condições de Acesso 01
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG1A1");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG1A1");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG1A1");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG1A1");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG1A1");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG1A1");

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

        protected void rpAcaodeDirecaoG1A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG1A1");
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

        protected void chkRespostaDemaisG1A1_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG1A1_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 1;
                int? ordemAssuntoInicio = 3;
                int? ordemAssuntoFim = 6;

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A1");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A1");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A1");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A1");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();



                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }

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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A1");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A1");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A1");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Demais dependencias foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Demais dependências realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        //GRUPO1 - ABA2

        protected void rpdemaisdependenciasGrupoG1A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG1A2");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG1A2");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 7, 10);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG1A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Aba Condições de Acesso 02
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG1A2");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG1A2");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG1A2");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG1A2");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG1A2");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG1A2");

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

        protected void rpAcaodeDirecaoG1A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG1A2");
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

        protected void chkRespostaDemaisG1A2_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG1A2_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 1;
                int? ordemAssuntoInicio = 7;
                int? ordemAssuntoFim = 10;

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A2.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A2");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A2");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A2");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A2");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A2");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A2");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A2");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A2");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }


                    }

                }
                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Demais dependencias foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Demais dependências realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }



        //GRUPO1 - ABA3

        protected void rpdemaisdependenciasGrupoG1A3_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG1A3");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG1A3");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 11, 14);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG1A3_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Aba Condições de Acesso 03
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG1A3");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG1A3");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG1A3");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG1A3");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG1A3");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG1A3");

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

        protected void rpAcaodeDirecaoG1A3_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG1A3");
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

        protected void chkRespostaDemaisG1A3_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG1A3_Click(object sender, EventArgs e)
        {
            try
            {
                int? ordemGrupo = 1;
                int? ordemAssuntoInicio = 11;
                int? ordemAssuntoFim = 14;

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A3.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A3");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A3");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);

                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A3");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A3");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A3");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A3");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A3");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A3");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Demais dependencias foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Demais dependências realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        //GRUPO1 - ABA4

        protected void rpdemaisdependenciasGrupoG1A4_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG1A4");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG1A4");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 15, 18);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG1A4_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Aba Condições de Acesso 04
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {

                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG1A4");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG1A4");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG1A4");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG1A4");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG1A4");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG1A4");

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

        protected void rpAcaodeDirecaoG1A4_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG1A4");
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

        protected void chkRespostaDemaisG1A4_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG1A4_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 1;
                int? ordemAssuntoInicio = 15;
                int? ordemAssuntoFim = 18;

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A4.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A4");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A4");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A4");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A4");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A4");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A4");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A4");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A4");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Demais dependencias foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Demais dependências realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        //GRUPO1 - ABA5

        protected void rpdemaisdependenciasGrupoG1A5_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG1A5");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG1A5");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 19, 23);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG1A5_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Aba Condições de Acesso 05
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG1A5");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG1A5");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG1A5");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG1A5");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG1A5");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG1A5");

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

        protected void rpAcaodeDirecaoG1A5_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG1A5");
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

        protected void chkRespostaDemaisG1A5_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG1A5_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 1;
                int? ordemAssuntoInicio = 19;
                int? ordemAssuntoFim = 23;

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A5.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A5");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A5");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A5");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A5");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A5");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A5");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A5");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A5");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Condições de Acesso foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Condições de Acesso realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        //GRUPO1 - ABA6


        protected void rpdemaisdependenciasGrupoG1A6_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG1A6");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG1A6");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 24, 0);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG1A6_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Aba Condições de Acesso 06
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {

                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG1A6");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG1A6");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG1A6");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG1A6");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG1A6");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG1A6");

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

        protected void rpAcaodeDirecaoG1A6_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG1A6");
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

        protected void chkRespostaDemaisG1A6_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG1A6_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 1;
                int? ordemAssuntoInicio = 24;
                int? ordemAssuntoFim = 0;//o restante todo

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG1A6.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG1A6");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG1A6");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG1A6");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG1A6");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG1A6");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG1A6");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG1A6");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG1A6");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Condições de Acesso foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Condições de Acesso realizada com sucesso.')", true);
                }

                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }

        #endregion

        #region GRUPO2

        //GRUPO2 - ABA1

        protected void rpdemaisdependenciasGrupoG2A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG2A1");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG2A1");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();


                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 1, 13);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG2A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //ABa Alimentação Escolar
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {

                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG2A1");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG2A1");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG2A1");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG2A1");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG2A1");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG2A1");

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

        protected void rpAcaodeDirecaoG2A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG2A1");
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

        protected void chkRespostaDemaisG2A1_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG2A1_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 2;
                int? ordemAssuntoInicio = 1;
                int? ordemAssuntoFim = 0;//o restante todo

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG2A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG2A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG2A1");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG2A1");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG2A1");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG2A1");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG2A1");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG2A1");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG2A1");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Alimentação Escolar foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Alimentação Escolar realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        #endregion

        #region GRUPO3

        //GRUPO3 - ABA1


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

                    //Verifica quantas questoes existem no grupo
                    int grupo = Convert.ToInt32(hdnGrupo.Value);
                    int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(grupo);
                    int media = total / 2;
                    int ordemAssuntoInicio = 1;
                    int ordemAssuntoFim = media;

                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));
                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 1, 8);
                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(grupo, ordemAssuntoInicio, ordemAssuntoFim);

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
            //Aba Tecnologia da Informação 01
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

        protected void btnDemaisDependenciasG3A1_Click(object sender, EventArgs e)
        {
            RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
            RN.InspecaoEscolar.Assunto rnAssunto = new RN.InspecaoEscolar.Assunto();
            List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
            RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

            try
            {
                int ordemGrupo = 3;

                //Verifica quantas questoes existem no grupo
                int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(ordemGrupo);
                int media = total / 2;

                int? ordemAssuntoInicio = 1;
                int? ordemAssuntoFim = media;

                ValidacaoDados validacao = new ValidacaoDados();

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG3A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG3A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG3A1");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG3A1");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG3A1");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG3A1");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG3A1");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG3A1");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Tecnologia da Informação foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Tecnologia da Informação realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        //GRUPO3 - ABA2

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

                    //Verifica quantas questoes existem no grupo 1
                    int grupo = Convert.ToInt32(hdnGrupo.Value);
                    int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(grupo);
                    int media = total / 2;
                    int ordemAssuntoInicio = media + 1;
                    int ordemAssuntoFim = 0; //Até o final

                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));
                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 9, 0);
                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(grupo, ordemAssuntoInicio, ordemAssuntoFim);

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
            ////Aba Tecnologia da Informação 01
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

        protected void btnDemaisDependenciasG3A2_Click(object sender, EventArgs e)
        {
            RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
            RN.InspecaoEscolar.Assunto rnAssunto = new RN.InspecaoEscolar.Assunto();
            List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
            RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

            try
            {
                int ordemGrupo = 3;

                //Verifica quantas questoes existem no grupo
                int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(ordemGrupo);
                int media = total / 2;

                int? ordemAssuntoInicio = media + 1;
                int? ordemAssuntoFim = 0; //Até o fim               

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG3A2.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG3A2");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG3A2");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);

                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG3A2");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG3A2");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG3A2");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG3A2");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG3A2");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Tecnologia da Informação foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert(Tecnologia da Informação realizada com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }
        }


        #endregion

        #region GRUPO4


        //GRUPO4 - ABA1

        protected void rpdemaisdependenciasGrupoG4A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG4A1");

                if (assuntoRepeater != null)
                {
                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG4A1");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();

                    //Verifica quantas questoes existem no grupo
                    int grupo = Convert.ToInt32(hdnGrupo.Value);
                    int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(grupo);
                    int media = total / 2;
                    int ordemAssuntoInicio = 1;
                    int ordemAssuntoFim = media;

                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));
                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 1, 13);
                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(grupo, ordemAssuntoInicio, ordemAssuntoFim);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG4A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //ABA: Situações Excepcionais 01
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG4A1");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG4A1");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG4A1");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG4A1");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG4A1");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG4A1");

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

        protected void rpAcaodeDirecaoG4A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG4A1");
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

        protected void chkRespostaDemaisG4A1_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;
        }

        protected void btnDemaisDependenciasG4A1_Click(object sender, EventArgs e)
        {
            RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
            RN.InspecaoEscolar.Assunto rnAssunto = new RN.InspecaoEscolar.Assunto();
            List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
            RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

            try
            {
                int ordemGrupo = 4;

                //Verifica quantas questoes existem no grupo
                int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(ordemGrupo);
                int media = total / 2;

                int? ordemAssuntoInicio = 1;
                int? ordemAssuntoFim = media;

                ValidacaoDados validacao = new ValidacaoDados();

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG4A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG4A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG4A1");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);

                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG4A1");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG4A1");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG4A1");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG4A1");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG4A1");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG4A1");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Situações Excepcionais foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Situações Excepcionais realizado com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }

        }


        //GRUPO4 - ABA2

        protected void rpdemaisdependenciasGrupoG4A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG4A2");

                if (assuntoRepeater != null)
                {
                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG4A2");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();

                    //Verifica quantas questoes existem no grupo
                    int grupo = Convert.ToInt32(hdnGrupo.Value);
                    int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(grupo);
                    int media = total / 2;
                    int ordemAssuntoInicio = media + 1;
                    int ordemAssuntoFim = 0; //Até o final

                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));
                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 14, 0);
                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(grupo, ordemAssuntoInicio, ordemAssuntoFim);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);

                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();


                }


            }
        }

        protected void rpdemaisdependenciasAssuntoG4A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //ABa: Situações Excepcionais 02
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG4A2");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG4A2");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG4A2");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG4A2");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG4A2");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG4A2");

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

        protected void rpAcaodeDirecaoG4A2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG4A2");
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

        protected void chkRespostaDemaisG4A2_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;
        }

        protected void btnDemaisDependenciasG4A2_Click(object sender, EventArgs e)
        {
            RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
            RN.InspecaoEscolar.Assunto rnAssunto = new RN.InspecaoEscolar.Assunto();
            List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
            RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

            try
            {
                int ordemGrupo = 4;

                //Verifica quantas questoes existem no grupo
                int total = rnAssunto.RetornaQuantidadeAssuntoPor_DemaisDependencias(ordemGrupo);
                int media = total / 2;

                int? ordemAssuntoInicio = media + 1;
                int? ordemAssuntoFim = 0; //Até o fim

                ValidacaoDados validacao = new ValidacaoDados();

                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG4A2.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG4A2");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG4A2");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);

                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG4A2");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG4A2");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG4A2");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG4A2");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG4A2");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG4A2");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Situações Excepcionais foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Situações Excepcionais realizado com sucesso.')", true);
                }

                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region GRUPO5

        //GRUPO5 - ABA1

        protected void rpdemaisdependenciasGrupoG5A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if ((item.ItemType == ListItemType.Item) ||

                (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater assuntoRepeater = (Repeater)item.FindControl("rpdemaisdependenciasAssuntoG5A1");

                if (assuntoRepeater != null)
                {

                    //pega o grupo
                    HiddenField hdnGrupo = (HiddenField)item.FindControl("hdnGrupoIdG5A1");

                    RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
                    RN.InspecaoEscolar.OpcoesAssunto rnOpcoesAssunto = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();
                    List<RN.DTOs.DadosRelatorioInspecaoGrupo> lsGrupo = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoGrupo>();
                    List<RN.DTOs.DadosRelatorioInspecaoAssunto> lsAssunto = new List<Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto>();

                    //lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value));

                    lsAssunto = rnAssunto.ObtemDadosAssuntoPor_DemaisDependencias(Convert.ToInt32(hdnGrupo.Value), 14, 0);

                    foreach (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto in lsAssunto)
                    {
                        assunto.ListaOpcao = rnOpcoesAssunto.ObtemDadosOpcaoPor(assunto.AssuntoId);
                    }

                    assuntoRepeater.DataSource = lsAssunto;
                    assuntoRepeater.DataBind();
                }
            }
        }

        protected void rpdemaisdependenciasAssuntoG5A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //ABA: Sala de Recursos Multifuncionais
            RepeaterItem item = e.Item;
            RN.InspecaoEscolar.Assunto rnAssunto = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();

            try
            {
                Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto assunto2 = (Techne.Lyceum.RN.DTOs.DadosRelatorioInspecaoAssunto)e.Item.DataItem;

                if ((item.ItemType == ListItemType.Item) ||

                    (item.ItemType == ListItemType.AlternatingItem))
                {
                    HiddenField hdnAssuntoId = (HiddenField)item.FindControl("hdnAssuntoIdDemaisG5A1");

                    if (hdnAssuntoId != null)
                    {
                        var valores = hdnAssuntoId.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acodirecao = Convert.ToBoolean(valores[2]);

                        Label lbl = (Label)item.FindControl("lblAssuntoDemaisG5A1");
                        TextBox txt = (TextBox)item.FindControl("txtRespostaDemaisG5A1");
                        RadioButtonList rbl = (RadioButtonList)item.FindControl("rdGrupoDemaisG5A1");
                        CheckBoxList chk = (CheckBoxList)item.FindControl("chkRespostaDemaisG5A1");

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

                                int restricao = rnAssunto.ObtemRestricaoPor(Convert.ToInt32(assunto));

                                if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.ApenasNumero)// Apenas número
                                {
                                    txt.Attributes.Add("onkeyPress", "return somenteNumeros(event);");
                                }
                                else if (restricao == (int)RN.InspecaoEscolar.Assunto.EnumRestricao.Data)// Data
                                {
                                    txt.Attributes.Add("onkeyPress", "return mascaraData(this);");
                                    txt.Attributes.Add("onblur", "return validarCampoData(this);");
                                    txt.Attributes.Add("placeholder", "dd/mm/aaaa");
                                    txt.Attributes.Add("maxlength", "10");
                                }

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

                            Repeater acaoRepeater = (Repeater)item.FindControl("rpAcaodeDirecaoG5A1");

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

        protected void rpAcaodeDirecaoG5A1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Cria método, para não ir toda vez.
            DropDownList drop = (DropDownList)e.Item.FindControl("ddlPerguntaDemaisG5A1");
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

        protected void chkRespostaDemaisG5A1_PreRender(object sender, EventArgs e)
        {
            CheckBoxList s = sender as CheckBoxList;


        }

        protected void btnDemaisDependenciasG5A1_Click(object sender, EventArgs e)
        {

            try
            {
                int? ordemGrupo = 5;
                int? ordemAssuntoInicio = 0; //Com todos
                int? ordemAssuntoFim = 0;// com todo o resto

                RN.InspecaoEscolar.RespostaAssunto rnRespostaAssunto = new Techne.Lyceum.RN.InspecaoEscolar.RespostaAssunto();
                List<RN.InspecaoEscolar.Entidades.RespostaAssunto> respostaAssunto = new List<Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto>();
                RN.InspecaoEscolar.Entidades.RespostaAssunto resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                ValidacaoDados validacao = new ValidacaoDados();


                foreach (RepeaterItem itemGrupo in rpdemaisdependenciasGrupoG5A1.Items)
                {
                    Repeater rpGrupodemais = (Repeater)itemGrupo.FindControl("rpdemaisdependenciasAssuntoG5A1");

                    foreach (RepeaterItem itemAssunto in rpGrupodemais.Items)
                    {
                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                        HiddenField hdnTipoAssunto = (HiddenField)itemAssunto.FindControl("hdnAssuntoIdDemaisG5A1");

                        var valores = hdnTipoAssunto.Value.Split('&');
                        string assunto = valores[0];
                        string tipo = valores[1];
                        bool acaodirecao = Convert.ToBoolean(valores[2]);



                        if (acaodirecao)//Ação de Direção
                        {
                            Repeater rpAcaoDemais = (Repeater)itemAssunto.FindControl("rpAcaodeDirecaoG5A1");

                            foreach (RepeaterItem itemAcao in rpAcaoDemais.Items)
                            {
                                HiddenField hdnOPTipoAssunto = (HiddenField)itemAcao.FindControl("hdnOpAssuntoIdDemaisG5A1");

                                DropDownList drop = (DropDownList)itemAcao.FindControl("ddlPerguntaDemaisG5A1");

                                resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                if (drop.SelectedItem.Value != "-1")
                                {

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.OpcoesAssuntoId = Convert.ToInt32(hdnOPTipoAssunto.Value);
                                    resposta.AcaoDirecaoId = drop.SelectedItem.Value.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(drop.SelectedItem.Value);
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    respostaAssunto.Add(resposta);
                                }
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

                                    RadioButtonList rbl = (RadioButtonList)itemAssunto.FindControl("rdGrupoDemaisG5A1");
                                    if (rbl.SelectedItem != null)
                                    {
                                        var opcao = rbl.SelectedItem;
                                        var idOpcao = opcao.Value;
                                        resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao);
                                        resposta.AssuntoId = Convert.ToInt32(assunto);
                                        resposta.Descricao = null;
                                        resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                        resposta.UsuarioId = User.Identity.Name;


                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 3:
                                    CheckBoxList chk = (CheckBoxList)itemAssunto.FindControl("chkRespostaDemaisG5A1");

                                    foreach (ListItem it in chk.Items)
                                    {
                                        resposta = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.RespostaAssunto();

                                        if (it.Selected)
                                        {
                                            var idOpcao = it.Value.Split('&');

                                            resposta.OpcoesAssuntoId = Convert.ToInt32(idOpcao[0]);
                                            resposta.AssuntoId = Convert.ToInt32(idOpcao[1]);
                                            resposta.Descricao = null;
                                            resposta.AssuntoId = Convert.ToInt32(assunto);
                                            resposta.AcaoDirecaoId = null;//Não se aplica
                                            resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                            resposta.UsuarioId = User.Identity.Name;
                                            respostaAssunto.Add(resposta);
                                        }
                                    }

                                    break;

                                case 4:

                                    TextBox txtResposta = (TextBox)itemAssunto.FindControl("txtRespostaDemaisG5A1");
                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.Descricao = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? txtResposta.Text.Trim() : null;
                                    resposta.OpcoesAssuntoId = null;
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;


                                    if (!resposta.Descricao.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        respostaAssunto.Add(resposta);
                                    }

                                    break;

                                case 5:

                                    resposta.AssuntoId = Convert.ToInt32(assunto);
                                    resposta.AcaoDirecaoId = null;//Não se aplica
                                    resposta.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1;
                                    resposta.UsuarioId = User.Identity.Name;

                                    break;


                                default:

                                    break;
                            }
                        }
                    }
                }

                validacao = rnRespostaAssunto.Valida(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnRespostaAssunto.Salva(respostaAssunto, Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), ordemGrupo, ordemAssuntoInicio, ordemAssuntoFim, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Sala de Recursos Multifuncionais foi salvo com sucesso!");

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = respostaAssunto.Select(x => x.CampanhaEscolaId).First().ToString();
                    }

                    CriaDemaisdependencias();
                    ManterDependencia();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Sala de Recursos Multifuncionais realizado com sucesso.')", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ManterDependencia();
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion



        #endregion

        #region Sala de Aula


        private DataTable _acaoDirecao;

        public DataTable ListarAcaoDirecao()
        {
            DataTable lista = _acaoDirecao;

            if (lista == null)
            {
                lista = AcaoDirecaoRN.ListaAcaoDirecao();
                // lista.Rows.RemoveAt(5);
                _acaoDirecao = lista;
            }

            DataRow newRow = lista.NewRow();
            newRow["DESCRICAO"] = "Selecione";
            newRow["CODIGO"] = string.Empty;
            lista.Rows.InsertAt(newRow, 0);

            return lista;
        }

        public DataTable ListarAcaoDirecaoDemaisDependencias()
        {
            DataTable lista = null;
            RN.InspecaoEscolar.AcaoDirecao AcaoDirecao = new Techne.Lyceum.RN.InspecaoEscolar.AcaoDirecao();

            lista = AcaoDirecao.ListaAcaoDirecao();

            DataRow newRow = lista.NewRow();
            newRow["DESCRICAO"] = "Selecione";
            newRow["CODIGO"] = -1;
            lista.Rows.InsertAt(newRow, 0);

            return lista;

        }

        public void PegaEscolaCampanha()
        {//tseCampanha.Value != null ? Convert.ToInt32(tseCampanha.Value) : 0;

            try
            {
                CampanhaEscolaDados.CampanhaId = Convert.ToInt32(tseCampanha.Value) != null ? Convert.ToInt32(tseCampanha.Value) : -1;

                if (tseUnidade.Value.ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    CampanhaEscolaDados.Unidade_Ens = "Não Informado";
                }
                else
                {
                    CampanhaEscolaDados.Unidade_Ens = tseUnidade.Value.ToString();
                }

                CampanhaEscolaDados.UsuarioId = User.Identity.Name;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public int CampanhaId
        {
            get
            {
                int valor;
                int.TryParse((tseCampanha.Value ?? "0").ToString(), out valor);
                return valor;
            }
        }

        public string Unidade_Ens
        {
            get
            {
                return Convert.ToString(tseUnidade.Value);
            }
        }

        protected void CriarSaladeAula()
        {
            /*

            ATENÇÃO: grid com colunas montadas dinamicamente, cada uma com DataItemTemplate definido
            - Para estes casos, o ideal é montar dinamicamente as colunas ANTES de qualquer evento de
            renderização (PreRender e PreRenderComplete não funcionarão para montagem), e também 
            desabilitar o ViewState do grid (EnableViewState = false).
            https://www.devexpress.com/Support/Center/Question/Details/T109412/aspxgridview-the-findrowcelltemplatecontrol-method-returns-null-value-for-dynamically

            - Como o ViewState do grid estará desabilitado, então para cada postback as colunas dinâmicas
            deverão ser montadas novamente. Por este motivo o Page_Load não deve ser pulado a cada postback.
            //if (IsPostBack)
            //    return;

            */

            if (CampanhaId <= 0 || Unidade_Ens.IsNullOrEmptyOrWhiteSpace())
                return;

            //Obter uma lista de status de participação do aluno, para usarmos na montagem do grid dinâmico
            DataTable listaAcaoDirecaoSaladeAula = ListaAcaoDirecao();
            // retira a opção "X - Espaço Inexistente	6" da lista
            listaAcaoDirecaoSaladeAula.Rows.RemoveAt(6);

            //percorrer uma lista com todos os assuntos especificado não sei aonde (tem que verificar isso),
            //e para cada assunto, adicionar uma lista de colunas ref. a ele próprio
            DataTable listaAssunto = AssuntoRN.ListaAssuntoSalaAulaPor(CampanhaId);
            IList<GridViewDataColumn> colunasDinamicas = new List<GridViewDataColumn>();
            for (int i = 0; i < listaAssunto.Rows.Count; i++)
            {
                //obter o id do assunto
                int assuntoId = Convert.ToInt32(listaAssunto.Rows[i]["ASSUNTOID"]);
                string assuntoDescricao = Convert.ToString(listaAssunto.Rows[i]["DESCRICAO"]);

                //gerar a lista de respostas, com data de participação e status de participação, e agregá-la à outra lista de colunas dinâmicas
                colunasDinamicas = AgregarColunasQuestoesPorComponenteEtapa(colunasDinamicas, assuntoId, assuntoDescricao, listaAcaoDirecaoSaladeAula);
            }

            //com todas as colunas dinâmicas já geradas, adicioná-las 1 a 1 no gridview
            foreach (GridViewDataColumn col in colunasDinamicas)
                grdTranscricao.Columns.Add(col);

            //vincular os dados ao grid já montado
            grdTranscricao.DataSource = ListaSalaDeAula(CampanhaId, Unidade_Ens);
            grdTranscricao.DataBind();

            //montar os bands da transcrição
            // ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "addBands", "addBands();", true);
        }

        //protected void btnEditar_Click(object sender, EventArgs e)
        //{
        //    btnBuscar_Click(sender, null);

        //    btnEditar.Visible = false;
        //    btnSalvar.Visible = true;
        //    btnCancelar.Visible = true;

        //    pnlEditar.Enabled = true;
        //}


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var campanhaEscola = JogarDadosDoGridViewDeSalasDeAulaNaDTOParaSalvar();
                ValidacaoDados validacao = RespostaDependenciaRN.ValidaSalaAula(campanhaEscola);
                if (validacao.Valido)
                {
                    RespostaDependenciaRN.SalvaSalaAula(campanhaEscola, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = campanhaEscola.CAMPANHAESCOLAID.ToString();
                    }

                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório foi salvo com sucesso!");
                }
                else
                {
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, validacao.Mensagem);
                }

                //btnCancelar_Click(sender, e);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
            btnEditar.Visible = true;

            //CriarSaladeAula();
        }

        private DadosCampanhaEscola JogarDadosDoGridViewDeSalasDeAulaNaDTOParaSalvar()
        {
            ////tentar obter os dados da campanha escola. se não conseguir, instanciar uma nova campanha escola
            //var dto = CampanhaEscolaRN.ObtemCampanhaEscola(CampanhaId, Unidade_Ens);
            //if (dto == null)
            //    dto = new DadosCampanhaEscola();

            ////setar os valores do dto para nova campanha escola
            //if (dto.CAMPANHAESCOLAID == 0)
            //{
            //    dto.CAMPANHAID = CampanhaId;
            //    dto.UNIDADE_ENS = Unidade_Ens;
            //    dto.USUARIOID = User.Identity.Name;
            //    dto.DATACADASTRO = DateTime.Now;
            //    dto.DATAALTERACAO = DateTime.Now;

            //    //para cada linha montada no grid...
            //    for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
            //    {
            //        var rd = new DadosCampanhaEscola.DadosRespostaDependencia();
            //        var temOpcaoPreenchida = false;

            //        rd.DEPENDENCIA = Convert.ToString(grdTranscricao.GetRowValues(i, "DEPENDENCIA"));
            //        rd.FACULDADE = dto.UNIDADE_ENS;
            //        rd.IDENTIFICACAODEPENDENCIAID = null;

            //        //para cada coluna do grid...
            //        foreach (GridViewDataColumn col in grdTranscricao.Columns)
            //        {
            //            //se tratar-se da coluna de placa identificação...
            //            if (col.Name.ToLower().StartsWith("pi_"))
            //            {
            //                //obter o dropdown da placa identificação
            //                DropDownList pi = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
            //                if (pi == null)
            //                    continue;

            //                //se o valor do dropdown estiver nulo ou em branco...
            //                if (pi.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            //                    continue;

            //                temOpcaoPreenchida = true;

            //                //setar o valor da propriedade com o que veio do grid
            //                rd.PLACAIDENTIFICACAO = pi.SelectedValue == "1" ? true : false;

            //                continue;
            //            }

            //            //se tratar-se da coluna de resposta...
            //            if (col.Name.ToLower().StartsWith("r"))
            //            {
            //                var rdo = new DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao();

            //                //pegar o opcaoAssuntoId que está no nome da coluna, concatenado após o "r"
            //                int opcaoAssuntoId;
            //                int.TryParse(col.Name.Substring(1), out opcaoAssuntoId);
            //                if (opcaoAssuntoId == 0)
            //                    continue;

            //                //obter o dropdown da resposta
            //                DropDownList r = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
            //                if (r == null)
            //                    continue;

            //                //se o valor do dropdown estiver nulo ou em branco...
            //                if (r.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            //                    continue;

            //                temOpcaoPreenchida = true;

            //                //setar os valores das propriedades com o que veio do grid
            //                rdo.OPCOESASSUNTOID = opcaoAssuntoId;
            //                rdo.ACAODIRECAOID = Convert.ToInt32(r.SelectedValue);

            //                rd.RespostasDependenciasOpcoes.Add(rdo);

            //                continue;
            //            }
            //        }

            //        if (temOpcaoPreenchida)
            //            dto.RespostasDependencias.Add(rd);
            //    }

            //}
            //else
            //{
            //    dto.DATAALTERACAO = DateTime.Now;

            //    //para cada linha montada no grid...
            //    for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
            //    {
            //        var temOpcaoPreenchida = false;

            //        var rd = dto.RespostasDependencias.FirstOrDefault(q => q.RESPOSTADEPENDENCIAID == Convert.ToInt32(grdTranscricao.GetRowValues(i, "RESPOSTADEPENDENCIAID")));

            //        //para cada coluna do grid...
            //        foreach (GridViewDataColumn col in grdTranscricao.Columns)
            //        {
            //            //se tratar-se da coluna de placa identificação...
            //            if (col.Name.ToLower().StartsWith("pi_"))
            //            {
            //                //obter o dropdown da placa identificação
            //                DropDownList pi = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
            //                if (pi == null)
            //                    continue;

            //                //se o valor do dropdown estiver nulo ou em branco...
            //                if (pi.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            //                    continue;

            //                temOpcaoPreenchida = true;

            //                if (rd == null)
            //                    rd = new DadosCampanhaEscola.DadosRespostaDependencia();

            //                //setar o valor da propriedade com o que veio do grid
            //                rd.PLACAIDENTIFICACAO = pi.SelectedValue == "1" ? true : false;

            //                continue;
            //            }

            //            //se tratar-se da coluna de resposta...
            //            if (col.Name.ToLower().StartsWith("r"))
            //            {
            //                var rdo = new DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao();

            //                //pegar o opcaoAssuntoId que está no nome da coluna, concatenado após o "r"
            //                int opcaoAssuntoId;
            //                int.TryParse(col.Name.Substring(1), out opcaoAssuntoId);
            //                if (opcaoAssuntoId == 0)
            //                    continue;

            //                //obter o dropdown da resposta
            //                DropDownList r = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
            //                if (r == null)
            //                    continue;

            //                //se o valor do dropdown estiver nulo ou em branco...
            //                if (r.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            //                    continue;

            //                temOpcaoPreenchida = true;

            //                //setar os valores das propriedades com o que veio do grid
            //                rdo.OPCOESASSUNTOID = opcaoAssuntoId;
            //                rdo.ACAODIRECAOID = Convert.ToInt32(r.SelectedValue);

            //                if (rd == null)
            //                    rd = new DadosCampanhaEscola.DadosRespostaDependencia();

            //                rd.RespostasDependenciasOpcoes.Add(rdo);

            //                continue;
            //            }
            //        }

            //        if (temOpcaoPreenchida)
            //            dto.RespostasDependencias.Add(rd);
            //    }
            //}

            ////retornar a DTO
            //return dto;

            var dto = new DadosCampanhaEscola();

            dto.CAMPANHAID = CampanhaId;
            dto.UNIDADE_ENS = Unidade_Ens;
            dto.USUARIOID = User.Identity.Name;
            dto.DATACADASTRO = DateTime.Now;
            dto.DATAALTERACAO = DateTime.Now;

            //para cada linha montada no grid...
            for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
            {
                var rd = new DadosCampanhaEscola.DadosRespostaDependencia();
                var temOpcaoPreenchida = false;

                rd.DEPENDENCIA = Convert.ToString(grdTranscricao.GetRowValues(i, "DEPENDENCIA"));
                rd.FACULDADE = dto.UNIDADE_ENS;
                rd.IDENTIFICACAODEPENDENCIAID = null;

                //para cada coluna do grid...
                foreach (GridViewDataColumn col in grdTranscricao.Columns)
                {
                    //se tratar-se da coluna de placa identificação...
                    if (col.Name.ToLower().StartsWith("pi_"))
                    {
                        //obter o dropdown da placa identificação
                        DropDownList pi = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (pi == null)
                            continue;

                        //se o valor do dropdown estiver nulo ou em branco...
                        if (pi.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            continue;

                        temOpcaoPreenchida = true;

                        //setar o valor da propriedade com o que veio do grid
                        rd.PLACAIDENTIFICACAO = pi.SelectedValue == "1" ? true : false;

                        continue;
                    }

                    //se tratar-se da coluna de resposta...
                    if (col.Name.ToLower().StartsWith("r"))
                    {
                        var rdo = new DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao();

                        //pegar o opcaoAssuntoId que está no nome da coluna, concatenado após o "r"
                        int opcaoAssuntoId;
                        int.TryParse(col.Name.Substring(1), out opcaoAssuntoId);
                        if (opcaoAssuntoId == 0)
                            continue;

                        //obter o dropdown da resposta
                        DropDownList r = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (r == null)
                            continue;

                        //se o valor do dropdown estiver nulo ou em branco...
                        if (r.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            continue;

                        temOpcaoPreenchida = true;

                        //setar os valores das propriedades com o que veio do grid
                        rdo.OPCOESASSUNTOID = opcaoAssuntoId;
                        rdo.ACAODIRECAOID = Convert.ToInt32(r.SelectedValue);

                        rd.RespostasDependenciasOpcoes.Add(rdo);

                        continue;
                    }
                }

                if (temOpcaoPreenchida)
                    dto.RespostasDependencias.Add(rd);
            }

            return dto;
        }

        protected void grdTranscricao_Load(object sender, EventArgs e)
        {
            TituloGrid((ASPxGridView)sender, "Salas de Aula");

            CriarSaladeAula();
        }

        protected void grdBanheiro_Load(object sender, EventArgs e)
        {
            TituloGrid((ASPxGridView)sender, "Banheiros e Vestiários");

            CriarBanheiro();
        }

        protected void grdTranscricao_PreRender(object sender, EventArgs e)
        {
            //neste ponto, já tenho todo o grid montado com as sala de aula e as opções de assunto.
            //basta somente preencher os valores dos campos com o que está salvo (se tiver algo salvo).
            //senão, deixa tudo em branco mesmo

            //obter a DTO com todos os dados da campanhaescola, respostadependencia e respostadependenciaopcao
            var campanhaEscola = CampanhaEscolaRN.ObtemCampanhaEscola(CampanhaId, Unidade_Ens);
            if (campanhaEscola == null)
                return;

            //para cada linha montada no grid...
            for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
            {
                //obter a dependência especificada NA LINHA DO GRID. se não retornar nada, pula pra próxima linha
                string DEPENDENCIA = Convert.ToString(grdTranscricao.GetRowValues(i, "DEPENDENCIA"));
                if (DEPENDENCIA.IsNullOrEmptyOrWhiteSpace())
                    continue;

                //retornar a resposta-dependência (sala de aula) equivalente a linha em que está
                var respostaDependencia = campanhaEscola.RespostasDependencias.FirstOrDefault(q => q.DEPENDENCIA == DEPENDENCIA);
                if (respostaDependencia == null)
                    continue;

                //obter as colunas da linha. se não retornar nada, pula pra próxima coluna
                foreach (GridViewDataColumn col in grdTranscricao.Columns)
                {
                    //se tratar-se da coluna de placa identificação...
                    if (col.Name.ToLower().StartsWith("pi_"))
                    {
                        //obter o dropdown da placa identificação
                        DropDownList pi = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (pi == null)
                            continue;

                        //setar o valor da placa identificação com o que veio da DTO
                        if (respostaDependencia.PLACAIDENTIFICACAO.HasValue)
                            pi.SelectedValue = respostaDependencia.PLACAIDENTIFICACAO.Value ? "1" : "0";
                    }

                    //se tratar-se da coluna de respostas...
                    if (col.Name.ToLower().StartsWith("r"))
                    {
                        //pegar o opcaoAssuntoId que está no nome da coluna, concatenado após o "r"
                        int opcaoAssuntoId;
                        int.TryParse(col.Name.Substring(1), out opcaoAssuntoId);
                        if (opcaoAssuntoId == 0)
                            continue;

                        //obter a resposta dependência opção ref. ao opção assunto obtido no nome da coluna
                        var respostaDependenciaOpcao = respostaDependencia.RespostasDependenciasOpcoes.FirstOrDefault(q => q.OPCOESASSUNTOID == opcaoAssuntoId);
                        if (respostaDependenciaOpcao == null)
                            continue;

                        //obter o dropdown da resposta
                        DropDownList r = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (r == null)
                            continue;

                        //setar o valor da resposta com o que veio da DTO
                        r.SelectedValue = respostaDependenciaOpcao.ACAODIRECAOID.ToString();
                    }
                }
            }
        }

        #region Helpers para o ASPxGridView

        private class FormatoNomeQuestao
        {
            public FormatoNomeQuestao(string nome)
            {
                Nome = nome;
            }

            public FormatoNomeQuestao(int etapaId, int questao)
            {
                EtapaId = etapaId;
                Questao = questao;
            }

            public int EtapaId { get; set; }
            public int Questao { get; set; }

            public string Nome
            {
                get
                {
                    if (EtapaId > 0 && Questao > 0)
                        return EtapaId.ToString() + "_" + Questao.ToString();
                    else
                        return null;
                }

                set
                {
                    if (value.IsNullOrEmptyOrWhiteSpace())
                        return;

                    string[] campos = value.Split('_');

                    if (campos.Length != 2)
                        return;

                    for (int i = 0; i < campos.Length; i++)
                    {
                        string campo = campos[i];

                        if (campo.IsNullOrEmptyOrWhiteSpace())
                            return;

                        int num;
                        if (!int.TryParse(campo, out num))
                            return;

                        switch (i)
                        {
                            case 0:
                                EtapaId = num;
                                break;

                            case 1:
                                Questao = num;
                                break;

                            default:
                                return;
                        }
                    }
                }
            }
        }

        private class FormatoNomeStatus
        {
            public FormatoNomeStatus(string nome)
            {
                Nome = nome;
            }

            public FormatoNomeStatus(int etapaId, DateTime data)
            {
                EtapaId = etapaId;
                AlunoId = 0;
                Data = data;
            }

            public int EtapaId { get; set; }
            public int AlunoId { get; set; }
            public DateTime Data { get; set; }
            public string DataFormatada
            {
                get
                {
                    string data = null;
                    if (Data >= SqlDateTime.MinValue)
                        data = Data.ToString("yyyyMMdd");
                    return data;
                }
            }

            public string Nome
            {
                get
                {
                    if (EtapaId > 0 && AlunoId > 0 && !DataFormatada.IsNullOrEmptyOrWhiteSpace())
                        return EtapaId.ToString() + "_" + AlunoId.ToString() + "_" + DataFormatada;
                    else
                        return null;
                }

                set
                {
                    if (value.IsNullOrEmptyOrWhiteSpace())
                        return;

                    string[] campos = value.Split('_');

                    if (campos.Length != 3)
                        return;

                    for (int i = 0; i < campos.Length; i++)
                    {
                        string campo = campos[i];

                        if (campo.IsNullOrEmptyOrWhiteSpace())
                            return;

                        if (new int[] { 0, 1 }.Contains(i))
                        {
                            int num;
                            if (!int.TryParse(campo, out num))
                                return;

                            switch (i)
                            {
                                case 0:
                                    EtapaId = num;
                                    break;

                                case 1:
                                    AlunoId = num;
                                    break;

                                default:
                                    return;
                            }
                        }
                        else if (i == 2)
                        {
                            DateTime dt;
                            if (!DateTime.TryParse(campo, out dt))
                                return;

                            switch (i)
                            {
                                case 2:
                                    Data = dt;
                                    break;

                                default:
                                    return;
                            }
                        }
                    }
                }
            }
        }

        private GridViewDataColumn CriaColunaAcaoDirecao(int assuntoId, string descricao, int opcoesAssuntoId, int ordem, bool acaoDeDirecao, bool restritivo, int tamanhoColunaOpcoesAssunto, DataTable listaAcaoDirecao)
        {
            GridViewDataColumn col = new GridViewDataColumn
            {
                Caption = "<span style=\"white-space: nowrap; text-overflow: ellipsis; width: 90px; display: block; overflow: hidden\">" + ordem.ToString() + "-" + descricao + "</span>",
                Name = "r" + opcoesAssuntoId,
                FieldName = "r" + opcoesAssuntoId,
                Width = Unit.Pixel(tamanhoColunaOpcoesAssunto),
                UnboundType = UnboundColumnType.String,
                DataItemTemplate = new ComboBoxAcaoDirecaoTemplate("r" + opcoesAssuntoId, assuntoId, descricao, ordem, acaoDeDirecao, restritivo, listaAcaoDirecao),
            };

            col.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
            col.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
            col.ToolTip = ordem.ToString() + "-" + descricao;

            return col;
        }

        private GridViewDataColumn CriaColunaPlacaIdentificacao(int assuntoId, string assuntoDescricao)
        {
            string nomeColuna = "pi_" + assuntoId;

            GridViewDataColumn col = new GridViewDataColumn
            {
                Caption = "Placa Identificação",
                Name = nomeColuna,
                FieldName = nomeColuna,
                Width = Unit.Pixel(100),
                UnboundType = UnboundColumnType.String,
                DataItemTemplate = new ComboBoxPlacaIdentificacaoTemplate(nomeColuna, assuntoId, assuntoDescricao),
            };

            col.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
            col.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);

            return col;
        }

        private GridViewDataColumn CriaColunaIdentificacaoBanheiro(int assuntoId, string assuntoDescricao)
        {
            string nomeColuna = "ib_" + assuntoId;

            GridViewDataColumn col = new GridViewDataColumn
            {
                Caption = "Identificação Banheiro",
                Name = nomeColuna,
                FieldName = nomeColuna,
                Width = Unit.Pixel(100),
                UnboundType = UnboundColumnType.String,
                DataItemTemplate = new ComboBoxIdentificacaoBanheiroTemplate(nomeColuna, assuntoId, assuntoDescricao),
            };

            col.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
            col.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);

            return col;
        }

        private IList<GridViewDataColumn> AgregarColunasQuestoesPorComponenteEtapa(IList<GridViewDataColumn> colunasJaCriadas, int assuntoId, string assuntoDescricao, DataTable listaAcaoDirecao)
        {
            //não precisa rodar a função se o componente da etapa não tiver questões cadastradas, pois não terá colunas a serem criadas
            if (!AssuntoRN.TemOpcoesAssunto(assuntoId))
                return colunasJaCriadas;

            //obtem o assunto para pegar sua descrição
            DataRow assunto = AssuntoRN.ObtemPor(assuntoId);

            //obter a lista de opções assunto através do componente da etapa especificado.
            DataTable listaOpcoesAssunto = OpcoesAssuntoRN.ListaOpcaoAssuntoPor(assuntoId);

            //obter a qtd. de opções assunto especificado. esta será a qtd. de colunas a serem agregadas na lista de colunas dinâmicas
            int qtdQuestoesPorAssunto = listaOpcoesAssunto.Rows.Count;

            //setar o tamanho da coluna de opções assunto
            int tamanhoColunaOpcoesAssunto = 100;

            //criar a coluna ref. à plada de identificação e agregá-la à lista de colunas dinâmicas
            GridViewDataColumn colPlacaIdentificacao = CriaColunaPlacaIdentificacao(assuntoId, assuntoDescricao);
            colunasJaCriadas.Add(colPlacaIdentificacao);

            //criar as colunas dinâmicas ref. as opções assunto e agregá-la à lista de colunas dinâmicas
            for (int q = 0; q < qtdQuestoesPorAssunto; q++)
            {
                int opcoesAssuntoId = Convert.ToInt32(listaOpcoesAssunto.Rows[q]["OPCOESASSUNTOID"]);
                int ordem = Convert.ToInt32(listaOpcoesAssunto.Rows[q]["ORDEM"]);
                bool acaoDeDirecao = Convert.ToBoolean(listaOpcoesAssunto.Rows[q]["ACAODEDIRECAO"]);
                bool restritivo = Convert.ToBoolean(listaOpcoesAssunto.Rows[q]["RESTRITIVO"]);
                string descricao = Convert.ToString(listaOpcoesAssunto.Rows[q]["DESCRICAO"]);
                var colAcaoDirecao = CriaColunaAcaoDirecao(assuntoId, descricao, opcoesAssuntoId, ordem, acaoDeDirecao, restritivo, tamanhoColunaOpcoesAssunto, listaAcaoDirecao);
                colunasJaCriadas.Add(colAcaoDirecao);
            }

            //retornar a lista de colunas dinâmicas com as novas colunas já agregadas
            return colunasJaCriadas;
        }

        private IList<GridViewDataColumn> AgregarColunasQuestoesPorComponenteEtapa(IList<GridViewDataColumn> colunasJaCriadas, int assuntoId, string assuntoDescricao, DataTable listaAcaoDirecao, DataTable listaIdentificacaoDependencia)
        {
            //não precisa rodar a função se o componente da etapa não tiver questões cadastradas, pois não terá colunas a serem criadas
            if (!AssuntoRN.TemOpcoesAssunto(assuntoId))
                return colunasJaCriadas;

            //obtem o assunto para pegar sua descrição
            DataRow assunto = AssuntoRN.ObtemPor(assuntoId);

            //obter a lista de opções assunto através do componente da etapa especificado.
            DataTable listaOpcoesAssunto = OpcoesAssuntoRN.ListaOpcaoAssuntoPor(assuntoId);

            //obter a qtd. de opções assunto especificado. esta será a qtd. de colunas a serem agregadas na lista de colunas dinâmicas
            int qtdQuestoesPorAssunto = listaOpcoesAssunto.Rows.Count;

            //setar o tamanho da coluna de opções assunto
            int tamanhoColunaOpcoesAssunto = 100;

            //criar a coluna ref. à plada de identificação e agregá-la à lista de colunas dinâmicas
            //GridViewDataColumn colPlacaIdentificacao = CriaColunaPlacaIdentificacao(assuntoId, assuntoDescricao);
            GridViewDataColumn colIdentificacaoBanheiro = CriaColunaIdentificacaoBanheiro(assuntoId, assuntoDescricao);
            // colunasJaCriadas.Add(colPlacaIdentificacao);
            colunasJaCriadas.Add(colIdentificacaoBanheiro);

            //criar as colunas dinâmicas ref. as opções assunto e agregá-la à lista de colunas dinâmicas
            for (int q = 0; q < qtdQuestoesPorAssunto; q++)
            {
                int opcoesAssuntoId = Convert.ToInt32(listaOpcoesAssunto.Rows[q]["OPCOESASSUNTOID"]);
                int ordem = Convert.ToInt32(listaOpcoesAssunto.Rows[q]["ORDEM"]);
                bool acaoDeDirecao = Convert.ToBoolean(listaOpcoesAssunto.Rows[q]["ACAODEDIRECAO"]);
                bool restritivo = Convert.ToBoolean(listaOpcoesAssunto.Rows[q]["RESTRITIVO"]);
                string descricao = Convert.ToString(listaOpcoesAssunto.Rows[q]["DESCRICAO"]);
                var colAcaoDirecao = CriaColunaAcaoDirecao(assuntoId, descricao, opcoesAssuntoId, ordem, acaoDeDirecao, restritivo, tamanhoColunaOpcoesAssunto, listaAcaoDirecao);
                colunasJaCriadas.Add(colAcaoDirecao);
            }

            //retornar a lista de colunas dinâmicas com as novas colunas já agregadas
            return colunasJaCriadas;
        }

        #endregion

        public DataTable ListaAcaoDirecao()
        {
            DataTable lista = AcaoDirecaoRN.ListaAcaoDirecao();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public DataTable ListaIdentificacaoDependencia()
        {
            DataTable lista = IdentificacaoDependenciaRN.ListarIdentificacaoDependencia();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public DataTable ListaSalaDeAula(int CampanhaId, string Unidade_Ens)
        {
            return CampanhaEscolaRN.ListaSalaAulaPor(CampanhaId, Unidade_Ens);
        }


        public DataTable ListaBanheiro(int CampanhaId, string Unidade_Ens)
        {
            return CampanhaEscolaRN.ListaBanheiroPor(CampanhaId, Unidade_Ens);
        }

        public class ComboBoxAcaoDirecaoTemplate : ITemplate
        {
            private readonly string id;
            private readonly int assuntoId;
            private readonly string descricao;
            private readonly int ordem;
            private readonly bool acaoDeDirecao;
            private readonly bool restritivo;
            private readonly DataTable listaAcaoDirecao;

            public ComboBoxAcaoDirecaoTemplate(string id, int assuntoId, string descricao, int ordem, bool acaoDeDirecao, bool restritivo, DataTable listaAcaoDirecao)
            {
                this.id = id;
                this.assuntoId = assuntoId;
                this.descricao = descricao;
                this.ordem = ordem;
                this.acaoDeDirecao = acaoDeDirecao;
                this.restritivo = restritivo;
                this.listaAcaoDirecao = listaAcaoDirecao;
            }


            public void InstantiateIn(Control container)
            {
                DropDownList cb = new DropDownList();
                cb.ID = id;
                cb.Width = Unit.Pixel(90);
                cb.Attributes["data-assunto-id"] = assuntoId.ToString();
                cb.Attributes["data-descricao"] = descricao;
                cb.Attributes["data-ordem"] = ordem.ToString();
                cb.Attributes["data-acao-de-direcao"] = acaoDeDirecao.ToString();
                cb.Attributes["data-restritivo"] = restritivo.ToString();
                cb.Attributes["data-nao-se-aplica"] = "1";
                cb.DataTextField = "DESCRICAO";
                cb.DataValueField = "CODIGO";
                //cb.AppendDataBoundItems = true;
                cb.Items.Add(new ListItem { Text = "Selecione", Value = "" });
                cb.DataSource = listaAcaoDirecao;
                cb.DataBind();
                container.Controls.Add(cb);
            }
        }

        public class ComboBoxPlacaIdentificacaoTemplate : ITemplate
        {
            private readonly string id;
            private readonly int assuntoId;
            private readonly string assuntoDescricao;

            public ComboBoxPlacaIdentificacaoTemplate(string id, int assuntoId, string assuntoDescricao)
            {
                this.id = id;
                this.assuntoId = assuntoId;
                this.assuntoDescricao = assuntoDescricao;
            }

            public void InstantiateIn(Control container)
            {
                DropDownList cb = new DropDownList();
                cb.ID = id;
                cb.Width = Unit.Pixel(90);
                cb.Items.Add(new ListItem { Text = "Selecione", Value = "" });
                cb.Items.Add(new ListItem { Text = "SIM", Value = "1" });
                cb.Items.Add(new ListItem { Text = "NÃO", Value = "0" });
                cb.Attributes["data-assunto-id"] = assuntoId.ToString();
                cb.Attributes["data-assunto-descricao"] = assuntoDescricao;
                container.Controls.Add(cb);
            }
        }

        #endregion

        #region Outros

        protected void tseInstituicao_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseInstituicao.DBValue.IsNull)
                {
                    if (tseInstituicao.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Instituição não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Instituição não cadastrado (favor verificar).";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTipoInstituicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseInstituicao.ResetValue();

                if (!ddlTipoInstituicao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    tseInstituicao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDadosAcervo()
        {
            tseInstituicao.ResetValue();
            ddlSituacao.ClearSelection();
            txtAto.Text = string.Empty;
            txtVolume.Text = string.Empty;
            ddlMedida.ClearSelection();
            hdnAcervoId.Value = string.Empty;
        }

        private void CarregaMedida()
        {
            RN.InspecaoEscolar.Medida rnMedida = new Techne.Lyceum.RN.InspecaoEscolar.Medida();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);

            ddlMedida.Items.Clear();
            ddlMedida.DataSource = rnMedida.ListaMedidaAtiva();
            ddlMedida.DataBind();
            ddlMedida.Items.Insert(0, itemVazio);
        }

        protected void rblPossuiAcervo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlDadosAcervo.Visible = false;
                pnlGridAcervo.Visible = false;
                LimpaDadosAcervo();

                if (rblPossuiAcervo.SelectedValue == "S")
                {
                    pnlDadosAcervo.Visible = true;
                    pnlGridAcervo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAcervo_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.InspecaoEscolar.Acervo rnAcervo = new Techne.Lyceum.RN.InspecaoEscolar.Acervo();
                RN.InspecaoEscolar.Entidades.Acervo acervo = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Acervo();

                acervo.CampanhaEscolaId = !hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt16(hdnCampanhaEscolaId.Value) : -1;
                acervo.MedidaId = !ddlMedida.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMedida.SelectedValue) : -1;
                acervo.InstituicaoId = (tseInstituicao.IsValidDBValue && !tseInstituicao.DBValue.IsNull) ? tseInstituicao.DBValue.ToString() : null;
                acervo.Situacao = !ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlSituacao.SelectedValue : null;
                acervo.Ato = !txtAto.Text.IsNullOrEmptyOrWhiteSpace() ? txtAto.Text.Trim() : null;
                acervo.Volume = !txtVolume.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtVolume.Text) : -1;
                acervo.UsuarioId = User.Identity.Name;
                acervo.AcervoId = !hdnAcervoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnAcervoId.Value) : 0;

                validacao = rnAcervo.Valida(acervo, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null), (!hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1), Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), User.Identity.Name, acervo.AcervoId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (acervo.AcervoId == 0)
                    {
                        rnAcervo.Insere(acervo, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null), Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), User.Identity.Name);
                    }
                    else
                    {
                        rnAcervo.Atualiza(acervo, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null), Convert.ToInt32(tseCampanha.DBValue), User.Identity.Name);
                    }
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Inspeção Escolar foi salvo com sucesso!");

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Relatório Técnico", "alert('Inspeção Escolar incluído com sucesso.')", true);

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = acervo.CampanhaEscolaId.ToString();
                    }

                    odsAcervo.Select();
                    odsAcervo.DataBind();
                    grdAcervo.DataBind();

                    LimpaDadosAcervo();

                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAcervo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAcervo);
        }

        protected void grdAcervo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAcervo.Settings.ShowFilterRow = false;
        }

        protected void grdAcervo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAcervo.Settings.ShowFilterRow = false;
        }

        protected void grdAcervo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.InspecaoEscolar.Entidades.Acervo acervo = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Acervo();
            RN.InspecaoEscolar.Acervo rnAcervo = new Techne.Lyceum.RN.InspecaoEscolar.Acervo();

            int acervoId = 0;

            acervoId = Convert.ToInt32(e.Keys["ACERVOID"]);

            validacao = rnAcervo.ValidaRemocao(acervoId, User.Identity.Name);

            if (validacao.Valido)
            {
                rnAcervo.Remove(acervoId);
                grdAcervo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void Delete(object ACERVOID)
        { }

        public void Update(object TIPO_ORIGEM, object UNIDADE, object SITUACAO, object ATO, object VOLUME, object MEDIDAID, object ACERVOID)
        { }

        protected void grdAcervo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.InspecaoEscolar.Entidades.Acervo acervo = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.Acervo();
            RN.InspecaoEscolar.Acervo rnAcervo = new Techne.Lyceum.RN.InspecaoEscolar.Acervo();

            acervo.Situacao = e.NewValues["SITUACAO"] != null ? Convert.ToString(e.NewValues["SITUACAO"]) : null;
            acervo.Ato = e.NewValues["ATO"] != null ? Convert.ToString(e.NewValues["ATO"]) : null;
            acervo.MedidaId = e.NewValues["MEDIDAID"] != null ? Convert.ToInt32(e.NewValues["MEDIDAID"]) : -1;
            acervo.Volume = e.NewValues["VOLUME"] != null ? Convert.ToInt32(e.NewValues["VOLUME"]) : -1;
            acervo.UsuarioId = User.Identity.Name;
            acervo.AcervoId = Convert.ToInt32(e.Keys["ACERVOID"]);

            validacao = rnAcervo.Valida(acervo, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null), (!hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCampanhaEscolaId.Value) : -1), Convert.ToInt32(tseCampanha.DBValue), tseUnidade.DBValue.ToString(), User.Identity.Name, acervo.AcervoId == 0 ? true : false);

            if (validacao.Valido)
            {
                rnAcervo.Atualiza(acervo, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null), Convert.ToInt32(tseCampanha.DBValue), User.Identity.Name);
                odsAcervo.Select();
                odsAcervo.DataBind();
                grdAcervo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdAcervo.DataBind();
        }

        #endregion

        #region Banheiro

        protected void CriarBanheiro()
        {
            /*

            ATENÇÃO: grid com colunas montadas dinamicamente, cada uma com DataItemTemplate definido
            - Para estes casos, o ideal é montar dinamicamente as colunas ANTES de qualquer evento de
            renderização (PreRender e PreRenderComplete não funcionarão para montagem), e também 
            desabilitar o ViewState do grid (EnableViewState = false).
            https://www.devexpress.com/Support/Center/Question/Details/T109412/aspxgridview-the-findrowcelltemplatecontrol-method-returns-null-value-for-dynamically

            - Como o ViewState do grid estará desabilitado, então para cada postback as colunas dinâmicas
            deverão ser montadas novamente. Por este motivo o Page_Load não deve ser pulado a cada postback.
            //if (IsPostBack)
            //    return;

            */

            //var s = sender as ASPxGridView;

            if (CampanhaId <= 0 || Unidade_Ens.IsNullOrEmptyOrWhiteSpace())
                return;


            //Obter uma lista de status de participação do aluno, para usarmos na montagem do grid dinâmico
            DataTable listaAcaoDirecaoBanheiro = ListaAcaoDirecao();
            // retira a opção "X - Espaço Inexistente	6" da lista
            listaAcaoDirecaoBanheiro.Rows.RemoveAt(6);
            DataTable listaIdentificacao = ListaIdentificacaoDependencia();

            //Obter uma lista de dependências



            //Listar opções de assunto
            DataTable opcoesAssunto = OpcoesAssuntoRN.ListarSaladeAulaTipoOpcaoResposta(
                Convert.ToInt32(tseCampanha.Value ?? "0"),
                RN.InspecaoEscolar.TipoResposta.DEPENDÊNCIAS_BANHEIRO,
                Convert.ToString(tseUnidade.Value ?? "0")
            );

            //percorrer uma lista com todos os assuntos especificado não sei aonde (tem que verificar isso),
            //e para cada assunto, adicionar uma lista de colunas ref. a ele próprio
            DataTable listaAssunto = AssuntoRN.ListaAssuntoBanheiroPor(CampanhaId);
            IList<GridViewDataColumn> colunasDinamicas = new List<GridViewDataColumn>();
            for (int i = 0; i < listaAssunto.Rows.Count; i++)
            {
                //obter o id do assunto
                int assuntoId = Convert.ToInt32(listaAssunto.Rows[i]["ASSUNTOID"]);
                string assuntoDescricao = Convert.ToString(listaAssunto.Rows[i]["DESCRICAO"]);

                //gerar a lista de respostas, com data de participação e status de participação, e agregá-la à outra lista de colunas dinâmicas
                colunasDinamicas = AgregarColunasQuestoesPorComponenteEtapa(colunasDinamicas, assuntoId, assuntoDescricao, listaAcaoDirecaoBanheiro, listaIdentificacao);
            }

            //com todas as colunas dinâmicas já geradas, adicioná-las 1 a 1 no gridview
            foreach (GridViewDataColumn col in colunasDinamicas)
                grdBanheiro.Columns.Add(col);

            //vincular os dados ao grid já montado
            grdBanheiro.DataSource = ListaBanheiro(CampanhaId, Unidade_Ens);
            grdBanheiro.DataBind();

            //montar os bands da transcrição
            // ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "addBands", "addBands();", true);
        }

        protected void btnSalvar_BanheiroeVestiarios_Click(object sender, EventArgs e)
        {
            try
            {
                var campanhaEscola = JogarDadosDoGridViewBanheiroNaDTOParaSalvar();

                ValidacaoDados validacao = RespostaDependenciaRN.ValidaBanheiro(campanhaEscola);

                if (validacao.Valido)
                {
                    RespostaDependenciaRN.SalvaBanheiro(campanhaEscola, (!rblPossuiAcervo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblPossuiAcervo.SelectedValue == "S" ? true : false) : (bool?)null));

                    if (hdnCampanhaEscolaId.Value.IsNullOrEmptyOrWhiteSpace() || hdnCampanhaEscolaId.Value == "0")
                    {
                        hdnCampanhaEscolaId.Value = campanhaEscola.CAMPANHAESCOLAID.ToString();
                    }

                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "O relatório Banheiro e Vestiários foi salvo com sucesso!");
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, ex.Message);
            }
        }

        public class ComboBoxIdentificacaoBanheiroTemplate : ITemplate
        {
            private readonly string id;
            private readonly int assuntoId;
            private readonly string assuntoDescricao;

            public ComboBoxIdentificacaoBanheiroTemplate(string id, int assuntoId, string assuntoDescricao)
            {
                this.id = id;
                this.assuntoId = assuntoId;
                this.assuntoDescricao = assuntoDescricao;
            }

            public void InstantiateIn(Control container)
            {
                RN.InspecaoEscolar.IdentificacaoDependencia identificacao = new Techne.Lyceum.RN.InspecaoEscolar.IdentificacaoDependencia();

                DataTable identificacaodepenencia = identificacao.ListarIdentificacaoDependencia();
                DataRow newRow = identificacaodepenencia.NewRow();
                identificacaodepenencia.Rows.InsertAt(newRow, 0);


                DropDownList cb = new DropDownList();
                cb.ID = id;
                cb.Width = Unit.Pixel(90);

                //cb.Items.Add(new ListItem { Text = "Selecione", Value = "" });

                cb.DataSource = identificacaodepenencia;
                cb.DataTextField = "SIGLA";
                cb.DataValueField = "IDENTIFICACAODEPENDENCIAID";



                //cb.Items.Add(new ListItem { Text = "SIM", Value = "1" });
                //cb.Items.Add(new ListItem { Text = "NÃO", Value = "0" });
                cb.Attributes["data-assunto-id"] = assuntoId.ToString();
                cb.Attributes["data-assunto-descricao"] = assuntoDescricao;
                container.Controls.Add(cb);
            }
        }


        protected void grdBanheiro_PreRender(object sender, EventArgs e)
        {
            //neste ponto, já tenho todo o grid montado com as sala de aula e as opções de assunto.
            //basta somente preencher os valores dos campos com o que está salvo (se tiver algo salvo).
            //senão, deixa tudo em branco mesmo

            //obter a DTO com todos os dados da campanhaescola, respostadependencia e respostadependenciaopcao
            var campanhaEscola = CampanhaEscolaRN.ObtemCampanhaEscola(CampanhaId, Unidade_Ens);
            if (campanhaEscola == null)
                return;

            //para cada linha montada no grid...
            for (int i = 0; i < grdBanheiro.VisibleRowCount; i++)
            {
                //obter a dependência especificada NA LINHA DO GRID. se não retornar nada, pula pra próxima linha
                string DEPENDENCIA = Convert.ToString(grdBanheiro.GetRowValues(i, "DEPENDENCIA"));
                if (DEPENDENCIA.IsNullOrEmptyOrWhiteSpace())
                    continue;

                //retornar a resposta-dependência (sala de aula) equivalente a linha em que está
                var respostaDependencia = campanhaEscola.RespostasDependencias.FirstOrDefault(q => q.DEPENDENCIA == DEPENDENCIA);
                if (respostaDependencia == null)
                    continue;

                //obter as colunas da linha. se não retornar nada, pula pra próxima coluna
                foreach (GridViewDataColumn col in grdBanheiro.Columns)
                {
                    //se tratar-se da coluna de placa identificação...
                    if (col.Name.ToLower().StartsWith("ib_"))
                    {
                        //obter o dropdown da placa identificação (tipo do banheiro)
                        DropDownList pi = grdBanheiro.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (pi == null)
                            continue;

                        //setar o valor do tipo de banheiro que veio da dto
                        if (respostaDependencia.IDENTIFICACAODEPENDENCIAID != null)
                            pi.SelectedValue = respostaDependencia.IDENTIFICACAODEPENDENCIAID.Value.ToString()
                            ;
                        //setar o valor da placa identificação com o que veio da DTO
                        //  pi.SelectedValue = respostaDependencia.PLACAIDENTIFICACAO.HasValue && respostaDependencia.PLACAIDENTIFICACAO.Value ? "1" : "0";

                    }

                    //se tratar-se da coluna de respostas...
                    if (col.Name.ToLower().StartsWith("r"))
                    {
                        //pegar o opcaoAssuntoId que está no nome da coluna, concatenado após o "r"
                        int opcaoAssuntoId;
                        int.TryParse(col.Name.Substring(1), out opcaoAssuntoId);
                        if (opcaoAssuntoId == 0)
                            continue;

                        //obter a resposta dependência opção ref. ao opção assunto obtido no nome da coluna
                        var respostaDependenciaOpcao = respostaDependencia.RespostasDependenciasOpcoes.FirstOrDefault(q => q.OPCOESASSUNTOID == opcaoAssuntoId);
                        if (respostaDependenciaOpcao == null)
                            continue;

                        //obter o dropdown da resposta
                        DropDownList r = grdBanheiro.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (r == null)
                            continue;

                        //setar o valor da resposta com o que veio da DTO
                        r.SelectedValue = respostaDependenciaOpcao.ACAODIRECAOID.ToString();
                    }
                }
            }
        }

        private DadosCampanhaEscola JogarDadosDoGridViewBanheiroNaDTOParaSalvar()
        {
            var dto = new DadosCampanhaEscola();

            dto.CAMPANHAID = CampanhaId;
            dto.UNIDADE_ENS = Unidade_Ens;
            dto.USUARIOID = User.Identity.Name;
            dto.DATACADASTRO = DateTime.Now;
            dto.DATAALTERACAO = DateTime.Now;

            //para cada linha montada no grid...
            for (int i = 0; i < grdBanheiro.VisibleRowCount; i++)
            {
                var rd = new DadosCampanhaEscola.DadosRespostaDependencia();
                var temOpcaoPreenchida = false;

                rd.DEPENDENCIA = Convert.ToString(grdBanheiro.GetRowValues(i, "DEPENDENCIA"));
                rd.FACULDADE = dto.UNIDADE_ENS;
                //rd.IDENTIFICACAODEPENDENCIAID = null;
                rd.PLACAIDENTIFICACAO = null;


                //para cada coluna do grid...
                foreach (GridViewDataColumn col in grdBanheiro.Columns)
                {
                    //se tratar-se da coluna de placa identificação...
                    if (col.Name.ToLower().StartsWith("ib_"))
                    {
                        //obter o dropdown da placa identificação
                        DropDownList pi = grdBanheiro.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (pi == null)
                            continue;

                        //se o valor do dropdown estiver nulo ou em branco...
                        // if (pi.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        //     continue;

                        // temOpcaoPreenchida = true;


                        //se o valor do dropdown estiver nulo ou em branco...
                        if (pi.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            rd.IDENTIFICACAODEPENDENCIAID = null;
                            temOpcaoPreenchida = false;
                        }
                        else
                        {
                            rd.IDENTIFICACAODEPENDENCIAID = Convert.ToInt32(pi.SelectedItem.Value);
                            temOpcaoPreenchida = true;
                        }



                        //setar o valor da propriedade com o que veio do grid
                        // rd.PLACAIDENTIFICACAO = pi.SelectedValue == "1" ? true : false;

                        // continue;
                    }

                    //se tratar-se da coluna de resposta...
                    if (col.Name.ToLower().StartsWith("r"))
                    {
                        var rdo = new DadosCampanhaEscola.DadosRespostaDependencia.DadosRespostaDependenciaOpcao();

                        //pegar o opcaoAssuntoId que está no nome da coluna, concatenado após o "r"
                        int opcaoAssuntoId;
                        int.TryParse(col.Name.Substring(1), out opcaoAssuntoId);
                        if (opcaoAssuntoId == 0)
                            continue;

                        //obter o dropdown da resposta
                        DropDownList r = grdBanheiro.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        if (r == null)
                            continue;

                        //se o valor do dropdown estiver nulo ou em branco...
                        if (r.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            continue;

                        temOpcaoPreenchida = true;

                        //setar os valores das propriedades com o que veio do grid
                        rdo.OPCOESASSUNTOID = opcaoAssuntoId;
                        rdo.ACAODIRECAOID = Convert.ToInt32(r.SelectedValue);

                        rd.RespostasDependenciasOpcoes.Add(rdo);

                        continue;
                    }
                }

                if (temOpcaoPreenchida)
                    dto.RespostasDependencias.Add(rd);
            }

            return dto;
        }



        #endregion



    }

}
