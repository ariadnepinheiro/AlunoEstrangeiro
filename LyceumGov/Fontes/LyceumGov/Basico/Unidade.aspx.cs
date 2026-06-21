using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.Data;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/Unidade.aspx")]
    [ControlText("Unidades Ensino / Físicas")]
    [Title("Unidades Ensino / Físicas")]
    public partial class Unidade : TPage, IPostBackEventHandler

{

    #region Construtor e Enumerações

        public Unidade()
        {

        }

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

    #endregion


    #region Métodos Públicos de Listagem (ObjectDataSource)

        public object ListarCompartilhadaOferta(object idCompartilhada)
        {
            string id = idCompartilhada != null ? idCompartilhada.ToString() : null;

            if (!string.IsNullOrEmpty(id))
            {
                return CompartilhadaOferta.Listar(Convert.ToInt32(id));
            }
            return null;
        }

        public object ListarCompartilhadas(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return Compartilhada.Listar(unid);
            }
            return null;
        }

        public object ListarCompartilhadasETotalSalas(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return Compartilhada.ListarCompartilhadasETotalSalas(unidade, unidade);
            }
            return null;
        }

        public object ListarCursoUnidade(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return UnidadeEnsinoCursos.Listar(unid);
            }
            return null;
        }

        public object ListarSalasAlternativas(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return Dependencia.ListarSalasAlternativasPor(unid);
            }
            return null;
        }

        public object ListarSalaRecurso(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return Dependencia.ListarSalaRecursoPorUnidadeFisica(unid);
            }
            return null;
        }

        public object ListarBanheiro(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return Dependencia.ListarBanheiroPorUnidadeFisica(unid);
            }
            return null;
        }

        public object ListarEdifPav(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return UnidadeFisicaEdificacao.ListarPorUnidFisica(unid);
            }
            return null;
        }

        public object ListarDependencias(object unidade, object ativa)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return Dependencia.ListarPorUnidadeFisica(unid, ativa.ToString());
            }
            return null;
        }

        public object ListarDocumentos(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return DocCelebradoMunicipalizacao.Listar(unidade.ToString());
            }
            return null;
        }

        public object ListarSituacaoUnidade(object unidade)
        {
            string unid = unidade != null ? unidade.ToString() : null;

            if (!string.IsNullOrEmpty(unid))
            {
                return UnidadeEnsinoSituacao.Listar(unidade.ToString());
            }
            return null;
        }

    #endregion


    #region Eventos de Página

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    pcUnidade.Visible = false;
                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);
                    if (RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                    {
                        dtPerfil.Rows.Add(string.Empty, "privilegiado", "privilegiado", 0);
                    }

                    Session["perfis"] = dtPerfil;

                    //this.VerificaPerfil();

                    _tipoOperacao = TipoOperacao.Inicial;
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
            TituloGrid(grdCursoPorUnidade, "Implantação de Cursos/Modalidades");
            TituloGrid(grdEdificacoesPavimentos, string.Empty);
            TituloGrid(grdDependencias, "Dependências");
            TituloGrid(grdDemaisDependencias, "Salas de Recursos");
            TituloGrid(grdSalaAlternativa, "Sala Alternativa");
            TituloGrid(grdDiretor, "Equipe Técnico-Pedagógica");
            TituloGrid(grdMediador, "Mediador de Tecnologia");
            TituloGrid(grdAAGE, "AAGE");
            TituloGrid(grdCompartilhada, "Escolas Compartilhadas");
            TituloGrid(grdCompartilhadaOferta, "Ofertas");
            TituloGrid(grdCompartilhadaOfertaEstadual, "Ofertas");
            TituloGrid(grdSituacaoUnidade, "Histórico da Unidade");
            TituloGrid(grdDocumentosCelebrados, string.Empty);
            TituloGrid(grdBanheiroeVestiario, "Banheiros e Vestiários");
            odsAbaCompartilhadaComboRedeEnsino.SelectParameters["usuario"].DefaultValue = User.Identity.Name;
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                MontarDependencias(tseUnidade.DBValue.ToString());
                MontarSalasAlternativa(tseUnidade.DBValue.ToString());
                MontarEquipamentos(null);

                ControlaAcesso(grdEdificacoesPavimentos);
                ControlaAcesso(grdDependencias);
                ControlaAcesso(grdDemaisDependencias);
                ControlaAcesso(grdSalaAlternativa);

                if (_tipoOperacao == TipoOperacao.Alterar)
                {
                    tseUnidade.Mode = ControlMode.View;
                    tseUnidade.Enabled = false;
                }

                if (_tipoOperacao == TipoOperacao.Consultar)
                {
                    tsCEP.ShowButton = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    #endregion


    #region Controle de Operação e Perfis

        private void ControlarTipoOperacao()
        {
            RN.UnidadeEnsino rnUnidadeEnsino = new Techne.Lyceum.RN.UnidadeEnsino();
            RN.DTOs.UnidadeCaracteristicasFisicas infoFisicas = new UnidadeCaracteristicasFisicas();
            RN.DTOs.UnidadeInformacoesGerais infoGerais = new UnidadeInformacoesGerais();
            RN.UnidadeFisica rnUnidadeFisica = new Techne.Lyceum.RN.UnidadeFisica();
            TceMunicipalizacao municipalizacao = new TceMunicipalizacao();
            RN.UnidadeFisicaConcessionaria rnUnidadeFisicaConcessionaria = new Techne.Lyceum.RN.UnidadeFisicaConcessionaria();
            RN.Entidades.LyUnidadeFisicaConcessionaria concessionarias = new LyUnidadeFisicaConcessionaria();

            var dt = (DataTable)Session["perfis"];

            bool ehSuplan = false;
            bool ehSupie = false;
            bool ehSupadCnpj = false;
            bool ehPrivilegiado = false;
            bool ehDiretor = false;
            bool ehInspetor = false;
            bool ehUnidadeGeral = false;

            //Perfis Especificos Abas
            bool possuiInformacoesGerais = false;
            bool possuiCaracteristicasFisicas = false;
            bool possuiSalaAula = false;
            bool possuiCompartilhamento = false;
            bool possuiHistorico = false;
            bool possuiImplantacaoCursos = false;
            bool possuiConcessionarias = false;
            bool possuiDemaisDependenciaQuantidades = false;
            bool possuiDemaisDependenciaSalasRecursosAlternativas = false;
            bool possuiEquipamentosUnidade = false;
            bool possuiInternet = false;
            bool possuiPedagogico = false;
            bool possuiMunicipalizacao = false;

            ehSuplan = dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPLAN") + "'").Length > 0;
            ehSupie = dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPIE") + "'").Length > 0;
            ehSupadCnpj = dt.Select("perfil ='" + RN.RNBase.MudarAspas("CNPJ") + "'").Length > 0;
            ehPrivilegiado = dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length > 0;
            ehDiretor = dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIRETOR_UE") + "'").Length > 0;
            ehInspetor = dt.Select("perfil ='" + RN.RNBase.MudarAspas("INSPETOR ESCOLAR") + "'").Length > 0;
            ehUnidadeGeral = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE GERAL") + "'").Length > 0;

            possuiInformacoesGerais = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - INFORMAÇÕES GERAIS") + "'").Length > 0;
            possuiCaracteristicasFisicas = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - CARACTERÍSTICAS FÍSICAS") + "'").Length > 0;
            possuiSalaAula = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - SALA AULA") + "'").Length > 0;
            possuiCompartilhamento = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - COMPARTILHAMENTO") + "'").Length > 0;
            possuiHistorico = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - HISTÓRICO") + "'").Length > 0;
            possuiMunicipalizacao = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - MUNICIPALIZAÇÃO") + "'").Length > 0;
            possuiImplantacaoCursos = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - IMPLANTAÇÃO CURSOS") + "'").Length > 0;
            possuiConcessionarias = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - CONCESSIONÁRIAS") + "'").Length > 0;
            possuiDemaisDependenciaQuantidades = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - DEMAIS DEPENDÊNCIA QUANTIDADES") + "'").Length > 0;
            possuiDemaisDependenciaSalasRecursosAlternativas = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - DEMAIS DEPENDÊNCIA SALAS RECURSOS/ALTERNATIVAS") + "'").Length > 0;
            possuiEquipamentosUnidade = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - EQUIPAMENTOS NA UNIDADE") + "'").Length > 0;
            possuiInternet = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - INTERNET") + "'").Length > 0;
            possuiPedagogico = dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - PEDAGÓGICOS") + "'").Length > 0;


            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {

                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);

                        pcUnidade.Visible = false;
                        btnEncontraNoMapa.Visible = false;
                        LimparAbaInformacoesGerais();
                        LimpaAbaCaracteristicasFisicas();
                        LimpaDadosConcessionaria();
                        LimpaCamposMunicipalizacao();
                        LimpaCamposDocCelebrado();
                        LimpaCamposMunicipalizacaoVigente();
                        LimpaDadosCondicaoSala();
                        LimpaDadosInternet();
                        LimpaDadosPedagogico();
                        pnlEnderecoUF.Visible = false;
                        pnlDocCelebrado.Visible = false;
                        CarregaSituacaoFuncionamento();
                        CarregaClassificacao();
                        CarregaLocalFuncionamento();
                        CarregaFormaOcupacao();
                        CarregaDependenciaAdm();
                        CarregaFormasAcessibilidade();
                        CarregaTratamentoLixo();
                        CarregaMaterialPedagogico();
                        CarregaOrgaosColegiados();
                        CarregaAcessoInternet();
                        CarregaNumProcessoMunicipalizacao();
                        CarregaTipoDocCelebrado();
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                        ControlarVisibilidadeControle(controles);
                        btnEncontraNoMapa.Visible = false;

                        for (int i = 1; i < 11; i++)
                        {
                            pcUnidade.TabPages[i].Enabled = true;
                        }

                        if (!chkImovelCompartilhado.Checked && !chkImovelCompartilhadoFisica.Checked)
                        {
                            pcUnidade.TabPages[3].Enabled = false;
                        }
                        else
                        {
                            pcUnidade.TabPages[3].Enabled = true;
                        }

                        txtUnidadeFisica.Text = txtUnidadeEnsino.Text;
                        txtNomeComp.Text = txtNome_Comp.Text;
                        txtCEP.Text = txtCEPUF.Text;
                        tsCEP.DBValue = tseCEPUF.DBValue;
                        txtEndereco.Text = txtLogradouroUF.Text;

                        txtEnd_Num.Text = txtNumeroEndUF.Text;
                        txtEnd_Compl.Text = txtComplementoUF.Text;
                        txtMunicipioFisica.Text = txtMunicipio.Text;
                        hdnCodMunicipioFisica.Value = hdnCodMunicipio.Value;

                        if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            this.CarregaBairro(hdnCodMunicipioFisica.Value);
                            ddlBairro.SelectedValue = ddlBairroUF.SelectedValue;
                        }

                        txtEstado.Value = txtEstadoUF.Value;
                        if (ddlDistrito.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            txtEnd_Num_TextChanged(null, null);
                            if (ddlDistritoFisica.Items.FindByValue(ddlDistrito.SelectedValue) != null)
                            {
                                ddlDistritoFisica.SelectedValue = ddlDistrito.SelectedValue;
                            }
                        }

                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);
                        HabilitaDesabilitaCamposAbaCaracteristicasFisicas(false);
                        HabilitaDesabilitaCamposMunicipalizacao(false);
                        HabilitaDesabilitaCamposAbaInternet(false);
                        HabilitaDesabilitaCamposAbaPedagogico(false);
                        pnlEnderecoUF.Visible = false;

                        grdAAGE.Visible = true;
                        grdMediador.Visible = true;
                        grdDiretor.Visible = true;
                        grdDemaisDependencias.Columns[0].Visible = false;
                        grdCursoPorUnidade.Columns[0].Visible = false;
                        grdDependencias.Columns[0].Visible = false;
                        grdEdificacoesPavimentos.Columns[0].Visible = false;
                        grdSalaAlternativa.Columns[0].Visible = false;
                        grdSituacaoUnidade.Columns[0].Visible = false;
                        grdDocumentosCelebrados.Columns[0].Visible = false;
                        grdCompartilhada.Columns[0].Visible = false;
                        grdCompartilhadaOferta.Columns[0].Visible = false;
                        grdCompartilhadaOfertaEstadual.Columns[0].Visible = false;
                        grdBanheiroeVestiario.Columns[0].Visible = false;//banheiro
                        pnlEnergiaEletrica.Enabled = btnSalvarConcessionaria.Visible;
                        pnlEsgoto.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSuprimentoGas.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSuprimentoAgua.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSalaAlternativa.Enabled = btnSalvarQtdDependencias.Visible;
                        pnlCondicoesSala.Enabled = btnSalvarQtdDependencias.Visible;
                        pnlDependencia.Enabled = btnSalvarQtdDependencias.Visible;
                        pnlEquipamentos.Enabled = btnSalvarQtdEquipamentos.Visible;
                        pnlPedagogico.Enabled = btnSalvarPedagogicos.Visible;
                        pnlInternet.Enabled = btnSalvarInternet.Visible;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvarNovo };
                        ControlarVisibilidadeControle(controles);
                        btnEncontraNoMapa.Visible = true;

                        tseUnidade.ResetValue();
                        tseUnidade.Mode = ControlMode.View;
                        tseUnidade.Enabled = false;
                        pcUnidade.Visible = true;
                        pnlEnderecoUF.Visible = false;
                        LimparAbaInformacoesGerais();
                        LimpaAbaCaracteristicasFisicas();
                        LimpaCamposMunicipalizacao();
                        LimpaCamposMunicipalizacaoVigente();
                        LimpaDadosConcessionaria();
                        LimpaDadosCondicaoSala();
                        LimpaDadosPedagogico();
                        LimpaDadosInternet();

                        for (int i = 1; i < 11; i++)
                        {
                            pcUnidade.TabPages[i].Enabled = false;
                        }
                        HabilitaDesabilitaCamposAbaInformacoesGerais(ehSuplan || ehPrivilegiado || possuiInformacoesGerais);
                        RemoverSituacaoFuncionamentoRestrita();
                        grdAAGE.Visible = false;
                        grdMediador.Visible = false;
                        grdDiretor.Visible = false;
                        if (ehSuplan || ehPrivilegiado || possuiInformacoesGerais)
                        {
                            pnlEnderecoUF.Visible = true;
                        }
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);

                        tseUnidade.Mode = ControlMode.View;
                        tseUnidade.Enabled = false;
                        btnEncontraNoMapa.Visible = true;

                        for (int i = 1; i < 11; i++)
                        {
                            pcUnidade.TabPages[i].Enabled = true;
                        }
                        if (!chkImovelCompartilhado.Checked && !chkImovelCompartilhadoFisica.Checked)
                        {
                            pcUnidade.TabPages[3].Enabled = false;
                        }
                        else
                        {
                            pcUnidade.TabPages[3].Enabled = true;
                        }

                        //Aba Informações Gerais
                        HabilitaDesabilitaCamposAbaInformacoesGerais(ehSuplan || ehPrivilegiado || possuiInformacoesGerais);
                        btnSalvarInfoGerais.Visible = (ehSuplan || ehPrivilegiado || possuiInformacoesGerais);

                        double lat, lng;
                        bool latValida = double.TryParse(
                            txtLatitude.Text,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out lat);
                        bool lngValida = double.TryParse(
                            txtLongitude.Text,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out lng);

                        if (!latValida || lat < -90 || lat > 90)
                            txtLatitude.Text = string.Empty;

                        if (!lngValida || lng < -180 || lng > 180)
                            txtLongitude.Text = string.Empty;

                        //Aba Características Físicas/Localização
                        HabilitaDesabilitaCamposAbaCaracteristicasFisicas(ehSupie || ehPrivilegiado || ehUnidadeGeral || ehSupadCnpj || possuiCaracteristicasFisicas);
                        btnSalvarFisicas.Visible = (ehSupie || ehPrivilegiado || ehUnidadeGeral || ehSupadCnpj || possuiCaracteristicasFisicas);

                        //Aba Sala de aula
                        grdDependencias.Columns[0].Visible = (ehSupie || ehPrivilegiado || ehUnidadeGeral || possuiSalaAula);
                        grdEdificacoesPavimentos.Columns[0].Visible = (ehSupie || ehPrivilegiado || ehUnidadeGeral || possuiSalaAula);

                        //Aba Compartilhamento
                        grdCompartilhada.Columns[0].Visible = (ehSuplan || ehSupie || ehPrivilegiado || possuiCompartilhamento);
                        grdCompartilhadaOferta.Columns[0].Visible = (ehSuplan || ehSupie || ehPrivilegiado || possuiCompartilhamento);
                        grdCompartilhadaOfertaEstadual.Columns[0].Visible = (ehSuplan || ehSupie || ehPrivilegiado || possuiCompartilhamento);
                        btnVerOfertas.Visible = (ehSuplan || ehSupie || ehPrivilegiado || possuiCompartilhamento);

                        //Municipalizaçao
                        HabilitaDesabilitaCamposMunicipalizacao(possuiMunicipalizacao || ehPrivilegiado || possuiCompartilhamento);
                        grdDocumentosCelebrados.Columns[0].Visible = (possuiMunicipalizacao || ehPrivilegiado || possuiCompartilhamento);
                        btnSalvarMunic.Visible = (possuiMunicipalizacao || ehPrivilegiado || possuiCompartilhamento);
                        btnSalvarDocCelebrado.Visible = (possuiMunicipalizacao || ehPrivilegiado || possuiCompartilhamento);

                        //Aba Histórico da Unidade                       
                        grdSituacaoUnidade.Columns[0].Visible = (ehSuplan || ehPrivilegiado || possuiHistorico);

                        //Aba Implantação de Cursos/Modalidades                        
                        grdCursoPorUnidade.Columns[0].Visible = (ehSuplan || ehPrivilegiado || possuiImplantacaoCursos);

                        //Aba Concessionária
                        btnSalvarConcessionaria.Visible = (ehSupie || ehDiretor || ehPrivilegiado || ehUnidadeGeral || possuiConcessionarias);
                        pnlEnergiaEletrica.Enabled = btnSalvarConcessionaria.Visible;
                        pnlEsgoto.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSuprimentoGas.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSuprimentoAgua.Enabled = btnSalvarConcessionaria.Visible;

                        //Aba Demais Dependências
                        grdDemaisDependencias.Columns[0].Visible = (ehSupie || ehPrivilegiado || ehUnidadeGeral || possuiDemaisDependenciaSalasRecursosAlternativas); //Salas de recursos
                        grdSalaAlternativa.Columns[0].Visible = (ehSupie || ehPrivilegiado || ehUnidadeGeral || possuiDemaisDependenciaSalasRecursosAlternativas);
                        grdBanheiroeVestiario.Columns[0].Visible = (ehSupie || ehInspetor || ehUnidadeGeral || ehPrivilegiado || possuiDemaisDependenciaSalasRecursosAlternativas);//banheiro

                        //Botoes / painel quantitativo
                        pnlCondicoesSala.Enabled = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiDemaisDependenciaQuantidades);
                        pnlDependencia.Enabled = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiDemaisDependenciaQuantidades);
                        btnSalvarQtdDependencias.Visible = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiDemaisDependenciaQuantidades);
                        pnlSalaAlternativa.Enabled = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiDemaisDependenciaQuantidades);
                        if (ehPrivilegiado || ehSupie || ehInspetor || ehUnidadeGeral || possuiDemaisDependenciaQuantidades)
                        {
                            TceValidacaoTipoDependencia TVTP = ValidacaoTipoDependencia.RetornaUltimaValidacao(tseUnidade.DBValue.ToString());

                            if (TVTP != null)
                            {
                                if (TVTP.Status == ValidacaoTipoDependencia.Validado)
                                {
                                    btnValidarDependencias.Visible = false;
                                    btnInvalidarDependencias.Visible = false;
                                    btnReabrirValidacao.Visible = true;
                                }
                                else if (TVTP.Status == ValidacaoTipoDependencia.Reaberto)
                                {
                                    btnReabrirValidacao.Visible = false;
                                }
                                else if (TVTP.Status == ValidacaoTipoDependencia.NaoConfirmado)
                                {
                                    btnReabrirValidacao.Visible = false;
                                    btnInvalidarDependencias.Visible = true;
                                }
                            }
                        }

                        //Aba Equipamentos na Unidade
                        pnlEquipamentos.Enabled = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiEquipamentosUnidade);
                        btnSalvarQtdEquipamentos.Visible = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiEquipamentosUnidade);
                        btnCancelarEquipamentos.Visible = btnSalvarQtdEquipamentos.Visible;
                        btnConfirmarEquipamentos.Visible = btnSalvarQtdEquipamentos.Visible;

                        //Aba Internet
                        HabilitaDesabilitaCamposAbaInternet(ehSupie || ehPrivilegiado || ehUnidadeGeral || possuiInternet);
                        btnSalvarInternet.Visible = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiInternet);

                        //Aba Pedagógicos                        
                        HabilitaDesabilitaCamposAbaPedagogico(ehSupie || ehPrivilegiado || ehUnidadeGeral || possuiPedagogico);
                        btnSalvarPedagogicos.Visible = (ehPrivilegiado || ehSupie || ehInspetor || ehDiretor || ehUnidadeGeral || possuiPedagogico);
                        VerificaEducacaoAmbiental();
                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                        ControlarVisibilidadeControle(controles);
                        btnEncontraNoMapa.Visible = false;

                        for (int i = 1; i < 11; i++)
                        {
                            pcUnidade.TabPages[i].Enabled = true;
                        }

                        LimparAbaInformacoesGerais();
                        LimpaAbaCaracteristicasFisicas();
                        LimpaDadosConcessionaria();
                        LimpaCamposMunicipalizacao();
                        LimpaCamposMunicipalizacaoVigente();
                        LimpaDadosCondicaoSala();
                        LimpaDadosInternet();
                        LimpaDadosPedagogico();
                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);
                        HabilitaDesabilitaCamposAbaCaracteristicasFisicas(false);
                        HabilitaDesabilitaCamposMunicipalizacao(false);
                        HabilitaDesabilitaCamposAbaPedagogico(false);
                        pnlEnderecoUF.Visible = false;

                        double lat, lng;
                        bool latValida = double.TryParse(
                            txtLatitude.Text,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out lat);
                        bool lngValida = double.TryParse(
                            txtLongitude.Text,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out lng);

                        if (!latValida || lat < -90 || lat > 90)
                            txtLatitude.Text = string.Empty;

                        if (!lngValida || lng < -180 || lng > 180)
                            txtLongitude.Text = string.Empty;

                        grdAAGE.Visible = true;
                        grdMediador.Visible = true;
                        grdDiretor.Visible = true;
                        grdDemaisDependencias.Columns[0].Visible = false;
                        grdBanheiroeVestiario.Columns[0].Visible = false;//banheiro
                        grdCursoPorUnidade.Columns[0].Visible = false;
                        grdDependencias.Columns[0].Visible = false;
                        grdEdificacoesPavimentos.Columns[0].Visible = false;
                        grdSalaAlternativa.Columns[0].Visible = false;
                        grdSituacaoUnidade.Columns[0].Visible = false;
                        grdDocumentosCelebrados.Columns[0].Visible = false;
                        grdCompartilhada.Columns[0].Visible = false;
                        grdCompartilhadaOferta.Columns[0].Visible = false;
                        grdCompartilhadaOfertaEstadual.Columns[0].Visible = false;

                        infoGerais = rnUnidadeEnsino.ObtemInformacoesGeraisPor(tseUnidade.DBValue.ToString());

                        if (!infoGerais.Censo.IsNullOrEmptyOrWhiteSpace())
                        {
                            txtUnidadeEnsino.Text = infoGerais.Censo;
                            txtNome_Comp.Text = !infoGerais.NomeUnidade.IsNullOrEmptyOrWhiteSpace() ? infoGerais.NomeUnidade : string.Empty;
                            tseRegional.DBValue = infoGerais.RegionalId;
                            tseSetor.DBValue = infoGerais.UnidadeAdministrativa;
                            tseNucleo.DBValue = infoGerais.Coordenadoria;
                            tseCEPUF.Value = infoGerais.Cep;
                            ddlClassificacao.SelectedValue = !infoGerais.Classificacao.IsNullOrEmptyOrWhiteSpace() ? (ddlClassificacao.Items.FindByValue(infoGerais.Classificacao) != null ? infoGerais.Classificacao : string.Empty) : string.Empty;
                            ddlSitFuncionamento.SelectedValue = !infoGerais.SituacaoFuncionamento.IsNullOrEmptyOrWhiteSpace() ? (ddlSitFuncionamento.Items.FindByValue(infoGerais.SituacaoFuncionamento) != null ? infoGerais.SituacaoFuncionamento : string.Empty) : string.Empty;
                            txtLatitude.Text = !infoGerais.Latitude.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Latitude : string.Empty;
                            txtLongitude.Text = !infoGerais.Longitude.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Longitude : string.Empty;
                            txtLogradouroUF.Text = !infoGerais.Endereco.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Endereco : string.Empty;
                            chkImovelCompartilhado.Checked = !infoGerais.ImovelCompartilhado.IsNullOrEmptyOrWhiteSpace() ? (infoGerais.ImovelCompartilhado == "S" ? true : false) : false;
                            txtCEPUF.Text = infoGerais.Cep;
                            txtMunicipio.Text = !infoGerais.MunicipioDescricao.IsNullOrEmptyOrWhiteSpace() ? infoGerais.MunicipioDescricao : string.Empty;
                            hdnCodMunicipio.Value = !infoGerais.Municipio.IsNullOrEmptyOrWhiteSpace() ? infoGerais.Municipio : string.Empty;

                            if (!hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                this.CarregaBairroUF(hdnCodMunicipio.Value);
                                string bairro = !infoGerais.EnderecoBairro.IsNullOrEmptyOrWhiteSpace() ? infoGerais.EnderecoBairro : string.Empty;
                                if (ddlBairroUF.Items.FindByValue(bairro) != null)
                                {
                                    ddlBairroUF.SelectedValue = bairro;
                                }
                            }

                            txtComplementoUF.Text = !infoGerais.EnderecoComplemento.IsNullOrEmptyOrWhiteSpace() ? infoGerais.EnderecoComplemento : string.Empty;
                            txtEstadoUF.Value = !infoGerais.UF.IsNullOrEmptyOrWhiteSpace() ? infoGerais.UF : string.Empty;

                            if (!infoGerais.Distrito.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtNumeroEndUF_TextChanged(null, null);
                                if (ddlDistrito.Items.FindByValue(infoGerais.Distrito) != null)
                                {
                                    ddlDistrito.SelectedValue = infoGerais.Distrito;
                                }
                            }
                        }

                        infoFisicas = rnUnidadeFisica.ObtemCaracteristicasFisicasPor(tseUnidade.DBValue.ToString());

                        if (!infoFisicas.UnidadeFisica.IsNullOrEmptyOrWhiteSpace())
                        {
                            txtUnidadeFisica.Text = infoFisicas.UnidadeFisica;
                            txtNomeComp.Text = !infoFisicas.NomeUnidadeFisica.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.NomeUnidadeFisica : string.Empty;
                            txtCEP.Text = !infoFisicas.Cep.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Cep : string.Empty;
                            tsCEP.DBValue = txtCEP.Text;
                            txtEndereco.Text = !infoFisicas.Endereco.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Endereco : string.Empty;
                            txtEnd_Num.Text = !infoFisicas.EnderecoNumero.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.EnderecoNumero : string.Empty;
                            txtEnd_Compl.Text = !infoFisicas.EnderecoComplemento.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.EnderecoComplemento : string.Empty;
                            txtMunicipioFisica.Text = !infoFisicas.MunicipioDescricao.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.MunicipioDescricao : string.Empty;
                            hdnCodMunicipioFisica.Value = !infoFisicas.Municipio.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Municipio : string.Empty;

                            if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                this.CarregaBairro(hdnCodMunicipioFisica.Value);
                                string bairro = !infoGerais.EnderecoBairro.IsNullOrEmptyOrWhiteSpace() ? infoGerais.EnderecoBairro : string.Empty;
                                if (ddlBairro.Items.FindByValue(bairro) != null)
                                {
                                    ddlBairro.SelectedValue = bairro;
                                }
                            }

                            txtEstado.Value = !infoFisicas.UF.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.UF : string.Empty;
                            txtAreaTerreno.Text = infoFisicas.AreaTerreno > 0 ? infoFisicas.AreaTerreno.ToString() : string.Empty;
                            txtAreaTotalTerreno.Text = infoFisicas.AreaTotalTerreno > 0 ? infoFisicas.AreaTotalTerreno.ToString() : string.Empty;
                            txtAreaConstruida.Text = infoFisicas.AreaTotalConstruida > 0 ? infoFisicas.AreaTotalConstruida.ToString() : string.Empty;
                            txtEmail.Text = !infoFisicas.Email.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Email : string.Empty;
                            txtCGC.Text = !infoFisicas.Cnpj.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Cnpj : string.Empty;
                            txtFone.Text = !infoFisicas.Telefone1.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Telefone1 : string.Empty;
                            txtFone2.Text = !infoFisicas.Telefone2.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Telefone2 : string.Empty;
                            txtFax.Text = !infoFisicas.Fax.IsNullOrEmptyOrWhiteSpace() ? infoFisicas.Fax : string.Empty;
                            chkAcessoDificil.Checked = !infoFisicas.AcessoDificil.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AcessoDificil == "S" ? true : false) : false;
                            chkOcupacaoFormal.Checked = !infoFisicas.OcupacaoFormal.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.OcupacaoFormal == "S" ? true : false) : false;
                            chkImovelCompartilhadoFisica.Checked = !infoFisicas.ImovelCompartilhadoRede.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.ImovelCompartilhadoRede == "S" ? true : false) : false;
                            chkExtraclasse.Checked = !infoFisicas.Extraclasse.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.Extraclasse == "S" ? true : false) : false;
                            ddlTipo.SelectedValue = !infoFisicas.FormaOcupacaoTipo.IsNullOrEmptyOrWhiteSpace() ? (ddlTipo.Items.FindByValue(infoFisicas.FormaOcupacaoTipo) != null ? infoFisicas.FormaOcupacaoTipo : string.Empty) : string.Empty;
                            chkAreaAssentamento.Checked = !infoFisicas.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AreaAssentamento == "S" ? true : false) : false;
                            chkTerraIndigena.Checked = !infoFisicas.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.TerraIndigena == "S" ? true : false) : false;
                            chkQuilombos.Checked = !infoFisicas.AreaQuilombo.IsNullOrEmptyOrWhiteSpace() ? (infoFisicas.AreaQuilombo == "S" ? true : false) : false;
                            ddlDependenciaAdministrativa.SelectedValue = !infoFisicas.DependenciaAdministrativa.IsNullOrEmptyOrWhiteSpace() ? (ddlDependenciaAdministrativa.Items.FindByValue(infoFisicas.DependenciaAdministrativa) != null ? infoFisicas.DependenciaAdministrativa : string.Empty) : string.Empty;
                            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked) ? true : false;
                            txtQtdSalaClimatizada.Text = infoFisicas.SalaClimatizada > 0 ? infoFisicas.SalaClimatizada.ToString() : string.Empty;
                            txtQtdSalaAcessibilidade.Text = infoFisicas.SalaAcessibilidade > 0 ? infoFisicas.SalaAcessibilidade.ToString() : string.Empty;
                            txtQtdCantinhoLeitura.Text = infoFisicas.SalaCantinhoLeitura > 0 ? infoFisicas.SalaCantinhoLeitura.ToString() : string.Empty;

                            pnlAcessibilidade.Visible = false;
                            if (infoFisicas.AcessoNecessidadeEspecial.IsNullOrEmptyOrWhiteSpace())
                            {
                                rblAcessoNecEspecial.ClearSelection();
                            }
                            else
                            {
                                rblAcessoNecEspecial.SelectedValue = infoFisicas.AcessoNecessidadeEspecial;

                                if (rblAcessoNecEspecial.SelectedValue == "S")
                                {
                                    pnlAcessibilidade.Visible = true;

                                    foreach (var linha in infoFisicas.FormasAcessibilidade)
                                    {
                                        chkRecursoAcessibilidade.Items.FindByValue(linha.ToString()).Selected = true;
                                    }
                                }
                            }

                            if (!infoFisicas.FormaOcupacaoLocalizacao.IsNullOrEmptyOrWhiteSpace())
                            {
                                if (rblLocalizacaoUF.Items.FindByValue(infoFisicas.FormaOcupacaoLocalizacao) != null)
                                {
                                    rblLocalizacaoUF.SelectedValue = infoFisicas.FormaOcupacaoLocalizacao;
                                }
                            }

                            if (!infoFisicas.LocalFuncionamento.IsNullOrEmptyOrWhiteSpace())
                            {
                                if (ddlLocalFuncionamento.Items.FindByValue(infoFisicas.LocalFuncionamento) != null)
                                {
                                    ddlLocalFuncionamento.SelectedValue = infoFisicas.LocalFuncionamento;
                                }
                            }

                            if (!infoFisicas.Distrito.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtEnd_Num_TextChanged(null, null);
                                if (ddlDistritoFisica.Items.FindByValue(infoFisicas.Distrito) != null)
                                {
                                    ddlDistritoFisica.SelectedValue = infoFisicas.Distrito;
                                }
                            }
                        }
                        if (!chkImovelCompartilhado.Checked && !chkImovelCompartilhadoFisica.Checked)
                        {
                            pcUnidade.TabPages[3].Enabled = false;
                        }
                        else
                        {
                            pcUnidade.TabPages[3].Enabled = true;
                        }

                        CarregaMunicipalizacaoVigente();

                        pnlDadosMunicipalizacao.Visible = false;
                        pnlDocCelebrado.Visible = false;

                        CarregaClasseFornecimentoEnergia();
                        CarregaTipoSuprimentoGas();

                        concessionarias = RN.UnidadeFisicaConcessionaria.Carregar(tseUnidade.DBValue.ToString());
                        if (concessionarias.IdUnidadeFisicaConcessionaria > 0)
                        {
                            PreencherDadosConcessionaria(concessionarias);
                        }

                        MontaDadosPedagogico(tseUnidade.DBValue.ToString());
                        MontaDadosInternet(tseUnidade.DBValue.ToString());
                        HabilitaDesabilitaCamposAbaInternet(false);

                        pnlEnergiaEletrica.Enabled = btnSalvarConcessionaria.Visible;
                        pnlEsgoto.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSuprimentoGas.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSuprimentoAgua.Enabled = btnSalvarConcessionaria.Visible;
                        pnlSalaAlternativa.Enabled = btnSalvarQtdDependencias.Visible;
                        pnlCondicoesSala.Enabled = btnSalvarQtdDependencias.Visible;
                        pnlDependencia.Enabled = btnSalvarQtdDependencias.Visible;
                        PreencheInfoDependenciaEquipamento();
                        pnlEquipamentos.Enabled = btnSalvarQtdEquipamentos.Visible;
                        //pnlInternet.Enabled = btnSalvarInternet.Visible;
                        //pnlPedagogico.Enabled = btnSalvarPedagogicos.Visible;
                        break;

                    }
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
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
            btnSalvarNovo.Visible = false;
            btnSalvarInfoGerais.Visible = false;
            btnSalvarFisicas.Visible = false;
            btnCancelarEquipamentos.Visible = false;
            btnConfirmarEquipamentos.Visible = false;
            btnInvalidarDependencias.Visible = false;
            btnReabrirValidacao.Visible = false;
            btnSalvarConcessionaria.Visible = false;
            btnSalvarQtdDependencias.Visible = false;
            btnSalvarQtdEquipamentos.Visible = false;
            btnValidarDependencias.Visible = false;
            btnVerOfertas.Visible = false;
            btnSalvarMunic.Visible = false;
            btnSalvarPedagogicos.Visible = false;
            btnSalvarInternet.Visible = false;
        }

    #endregion


    #region Carregamento de Dropdowns e Controles

        protected void CarregaSituacaoFuncionamento()
        {
            ddlSitFuncionamento.Items.Clear();
            ddlSitFuncionamento.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("SitFuncionamentoUE");
            ddlSitFuncionamento.DataBind();
            ddlSitFuncionamento.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void CarregaBairroUF(string municipioId)
        {
            RN.Bairro rnBairro = new Techne.Lyceum.RN.Bairro();
            ddlBairroUF.Items.Clear();
            ddlBairroUF.DataSource = rnBairro.ListaPor(municipioId);
            ddlBairroUF.DataBind();
            ddlBairroUF.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void CarregaBairro(string municipioId)
        {

            RN.Bairro rnBairro = new Techne.Lyceum.RN.Bairro();
            ddlBairro.Items.Clear();
            ddlBairro.DataSource = rnBairro.ListaPor(municipioId);
            ddlBairro.DataBind();
            ddlBairro.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void CarregaClassificacao()
        {
            ddlClassificacao.Items.Clear();
            ddlClassificacao.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("ClassificaçãoUnidade");
            ddlClassificacao.DataBind();
            ddlClassificacao.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void CarregaLocalFuncionamento()
        {
            Techne.Lyceum.RN.TabelaGeral rnTabelaGeral = new TabelaGeral();

            ddlLocalFuncionamento.Items.Clear();
            ddlLocalFuncionamento.DataSource = rnTabelaGeral.SelecionarItensLocalFuncionamento("LocalFuncionamentoUF");
            ddlLocalFuncionamento.DataBind();
            ddlLocalFuncionamento.Items.Insert(0, new ListItem("Selecione", string.Empty));

        }

        protected void CarregaFormaOcupacao()
        {
            ddlTipo.Items.Clear();
            ddlTipo.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("Tipo Unidade Física");
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void CarregaDependenciaAdm()
        {
            ddlDependenciaAdministrativa.Items.Clear();
            ddlDependenciaAdministrativa.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("Dependência Adm");
            ddlDependenciaAdministrativa.DataBind();
            ddlDependenciaAdministrativa.Items.Insert(0, new ListItem("Selecione", string.Empty));

        }

        private void CarregaFormasAcessibilidade()
        {
            RN.GestaoRede.FormasAcessibilidade rnFormasAcessibilidade = new Techne.Lyceum.RN.GestaoRede.FormasAcessibilidade();

            chkRecursoAcessibilidade.Items.Clear();
            chkRecursoAcessibilidade.DataSource = rnFormasAcessibilidade.ListaFormasAcessibilidadeAtivo();
            chkRecursoAcessibilidade.DataTextField = "DESCRICAO";
            chkRecursoAcessibilidade.DataValueField = "FORMASACESSIBILIDADEID";
            chkRecursoAcessibilidade.DataBind();
        }

        private void CarregaTratamentoLixo()
        {
            RN.GestaoRede.TratamentoLixo rnTratamentoLixo = new Techne.Lyceum.RN.GestaoRede.TratamentoLixo();

            chkTratamentoLixo.Items.Clear();
            chkTratamentoLixo.DataSource = rnTratamentoLixo.ListaTratamentoLixoAtivo();
            chkTratamentoLixo.DataTextField = "DESCRICAO";
            chkTratamentoLixo.DataValueField = "TRATAMENTOLIXOID";
            chkTratamentoLixo.DataBind();
        }

        private void CarregaMunicipalizacaoVigente()
        {
            RN.Municipalizacao rnMunicipalizacao = new Municipalizacao();
            DataTable dt = new DataTable();

            dt = rnMunicipalizacao.ObtemDadosUltimaMunicipalizacaoPor(tseUnidade.DBValue.ToString());

            if (dt.Rows.Count > 0)
            {
                txtUltProcMunic.Text = dt.Rows[0]["PROCESSO"].ToString();
                txtUltDocVigente.Text = dt.Rows[0]["DOCVIGENTE"].ToString();
                txtUltDtAutorizo.Text = Convert.ToString(dt.Rows[0]["DATAAUTORIZO"]);
                txtUltDtVigFinal.Text = Convert.ToString(dt.Rows[0]["DATAVIGENCIAFINAL"]);
            }
        }

        private void CarregaClasseFornecimentoEnergia()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEE_ClasseFornecimento.Items.Clear();
            ddlEE_ClasseFornecimento.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.ClasseFornecimentoEnergia);
            ddlEE_ClasseFornecimento.DataBind();
            ddlEE_ClasseFornecimento.Items.Insert(0, item);
        }

        private void CarregaTipoSuprimentoGas()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlGas_Tipo.Items.Clear();
            ddlGas_Tipo.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.TipoSuprimentoGas);
            ddlGas_Tipo.DataBind();
            ddlGas_Tipo.Items.Insert(0, item);
        }

        private void CarregarPavimentos(ASPxComboBox cmbPavimentacao, string edificacao)
        {
            if (string.IsNullOrEmpty(edificacao))
            {
                return;
            }

            cmbPavimentacao.Items.Clear();
            cmbPavimentacao.TextField = "NOME_PAVIMENTO";
            cmbPavimentacao.ValueField = "PAVIMENTO";
            cmbPavimentacao.DataSource = UnidadeFisicaEdificacao.ConsultarPavimentos(tseUnidade.DBValue.ToString(), edificacao);
            cmbPavimentacao.DataBind();
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

        protected void CarregaNumProcessoMunicipalizacao()
        {
            RN.Municipalizacao rnMunicipalizacao = new Municipalizacao();

            ddlNumProcessoDocCelebrado.Items.Clear();
            ddlNumProcessoDocCelebrado.DataSource = rnMunicipalizacao.ListaPor(tseUnidade.DBValue.ToString());
            ddlNumProcessoDocCelebrado.DataBind();
            ddlNumProcessoDocCelebrado.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaTipoDocCelebrado()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlTipoDocCelebrado.Items.Clear();
            ddlTipoDocCelebrado.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.TipoDocCelebrado);
            ddlTipoDocCelebrado.DataBind();
            ddlTipoDocCelebrado.Items.Insert(0, item);
        }

    #endregion


    #region Handlers de Botões — Navegação

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
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
                _tipoOperacao = TipoOperacao.Alterar;

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!tseUnidade.DBValue.IsNull)
                {
                    _tipoOperacao = TipoOperacao.Consultar;
                }
                else
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    #endregion


    #region Handlers de Botões — Salvar Abas Principais

        protected void btnSalvarNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Barreira de segurança: "Pré-Ativa" nunca pode ser o status de criação
                if (ddlSitFuncionamento.SelectedItem != null &&
                    ddlSitFuncionamento.SelectedItem.Text.Trim()
                        .Equals("Pré-Ativa", StringComparison.OrdinalIgnoreCase))
                {
                    lblMensagem.Text = "O status 'Pré-Ativa' não pode ser selecionado na criação de uma unidade escolar.";
                    return;
                }
                
                ValidacaoDados validacao = new ValidacaoDados();
                RN.UnidadeEnsino rnUnidade = new Techne.Lyceum.RN.UnidadeEnsino();
                var dt = (DataTable)Session["perfis"];

                var infoGerais = new RN.DTOs.UnidadeInformacoesGerais
                                         {
                                             Censo = !txtUnidadeEnsino.Text.IsNullOrEmptyOrWhiteSpace() ? txtUnidadeEnsino.Text.Trim() : null,
                                             NomeUnidade = !txtNome_Comp.Text.IsNullOrEmptyOrWhiteSpace() ? txtNome_Comp.Text.Trim() : null,
                                             SituacaoFuncionamento = !ddlSitFuncionamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlSitFuncionamento.SelectedValue : null,
                                             RegionalId = (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue) ? Convert.ToInt32(tseRegional.DBValue.ToString()) : -1,
                                             Coordenadoria = (!tseNucleo.DBValue.IsNull && tseNucleo.IsValidDBValue) ? tseNucleo.DBValue.ToString() : null,
                                             UnidadeAdministrativa = (!tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue) ? tseSetor["setor"].ToString() : null,
                                             Classificacao = !ddlClassificacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlClassificacao.SelectedValue : null,
                                             ImovelCompartilhado = chkImovelCompartilhado.Checked ? "S" : "N",
                                             UsuarioResponsavel = User.Identity.Name,
                                             Latitude = !txtLatitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLatitude.Text.Trim() : null,
                                             Longitude = !txtLongitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLongitude.Text.Trim() : null,
                                             Cep = !txtCEPUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEPUF.Text.Trim() : null,
                                             Municipio = !hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipio.Value : null,
                                             Endereco = !txtLogradouroUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtLogradouroUF.Text.Trim() : null,
                                             EnderecoNumero = !txtNumeroEndUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumeroEndUF.Text.Trim() : null,
                                             EnderecoComplemento = !txtComplementoUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplementoUF.Text.Trim() : null,
                                             EnderecoBairro = !ddlBairroUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlBairroUF.SelectedValue : null,
                                             Distrito = !ddlDistrito.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDistrito.SelectedValue : null,

                                         };

                validacao = rnUnidade.ValidaInformacoesGeraisInsercao(infoGerais);

                if (validacao.Valido)
                {
                    rnUnidade.Insere(infoGerais, (dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length > 0));
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                   "alert('Unidade inserida com sucesso.');", true);
                    tseUnidade.ResetValue();
                    tseUnidade.DBValue = txtUnidadeEnsino.Text;

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

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
        }

        protected void btnSalvarInfoGerais_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.UnidadeEnsino rnUnidade = new Techne.Lyceum.RN.UnidadeEnsino();
                var infoGerais = new RN.DTOs.UnidadeInformacoesGerais
                {
                    Censo = !txtUnidadeEnsino.Text.IsNullOrEmptyOrWhiteSpace() ? txtUnidadeEnsino.Text.Trim() : null,
                    NomeUnidade = !txtNome_Comp.Text.IsNullOrEmptyOrWhiteSpace() ? txtNome_Comp.Text.Trim() : null,
                    SituacaoFuncionamento = !ddlSitFuncionamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlSitFuncionamento.SelectedValue : null,
                    RegionalId = (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue) ? Convert.ToInt32(tseRegional.DBValue.ToString()) : -1,
                    Coordenadoria = (!tseNucleo.DBValue.IsNull && tseNucleo.IsValidDBValue) ? tseNucleo.DBValue.ToString() : null,
                    UnidadeAdministrativa = (!tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue) ? tseSetor["setor"].ToString() : null,
                    Classificacao = !ddlClassificacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlClassificacao.SelectedValue : null,
                    ImovelCompartilhado = chkImovelCompartilhado.Checked ? "S" : "N",
                    UsuarioResponsavel = User.Identity.Name,
                    Latitude = !txtLatitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLatitude.Text.Trim() : null,
                    Longitude = !txtLongitude.Text.IsNullOrEmptyOrWhiteSpace() ? txtLongitude.Text.Trim() : null,
                    Cep = !tseCEPUF.DBValue.IsNull ? tseCEPUF.DBValue.ToString() : null,
                    Municipio = !hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipio.Value : null,
                    Endereco = !txtLogradouroUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtLogradouroUF.Text.Trim() : null,
                    EnderecoNumero = !txtNumeroEndUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumeroEndUF.Text.Trim() : null,
                    EnderecoComplemento = !txtComplementoUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplementoUF.Text.Trim() : null,
                    EnderecoBairro = !ddlBairroUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlBairroUF.SelectedValue : null,
                    Distrito = !ddlDistrito.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDistrito.SelectedValue : null,

                };

                validacao = rnUnidade.ValidaInformacoesGeraisAlteracao(infoGerais);

                if (validacao.Valido)
                {
                    rnUnidade.AtualizaInformacoesGerais(infoGerais);

                    if (!chkImovelCompartilhado.Checked && !chkImovelCompartilhadoFisica.Checked)
                    {
                        pcUnidade.TabPages[3].Enabled = false;
                    }
                    else
                    {
                        pcUnidade.TabPages[3].Enabled = true;
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                   "alert('Unidade atualizada com sucesso.');", true);
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

        }

        protected void btnSalvarFisicas_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.UnidadeFisica rnUnidadeFisica = new Techne.Lyceum.RN.UnidadeFisica();
                List<int> listRecursoAcessibilidade = new List<int>();

                var infoFisicas = new RN.DTOs.UnidadeCaracteristicasFisicas
                {
                    UnidadeFisica = !txtUnidadeFisica.Text.IsNullOrEmptyOrWhiteSpace() ? txtUnidadeFisica.Text.Trim() : null,
                    NomeUnidadeFisica = !txtNomeComp.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeComp.Text.Trim() : null,
                    FormaOcupacaoLocalizacao = !rblLocalizacaoUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblLocalizacaoUF.SelectedValue : null,
                    LocalFuncionamento = !ddlLocalFuncionamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlLocalFuncionamento.SelectedValue : null,
                    FormaOcupacaoTipo = !ddlTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipo.SelectedValue : null,
                    AreaAssentamento = chkAreaAssentamento.Checked ? "S" : "N",
                    TerraIndigena = chkTerraIndigena.Checked ? "S" : "N",
                    AreaQuilombo = chkQuilombos.Checked ? "S" : "N",
                    OcupacaoFormal = chkOcupacaoFormal.Checked ? "S" : "N",
                    ImovelCompartilhadoRede = chkImovelCompartilhadoFisica.Checked ? "S" : "N",
                    Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEP.Text : null,
                    Municipio = !hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipioFisica.Value : null,
                    Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim() : null,
                    EnderecoNumero = !txtEnd_Num.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnd_Num.Text.Trim() : null,
                    EnderecoComplemento = !txtEnd_Compl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnd_Compl.Text.Trim() : null,
                    EnderecoBairro = !ddlBairro.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlBairro.SelectedValue.Trim() : null,
                    UF = !txtEstadoUF.Value.IsNullOrEmptyOrWhiteSpace() ? txtEstadoUF.Value.Trim() : null,
                    Distrito = !ddlDistritoFisica.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDistritoFisica.SelectedValue : null,
                    AcessoNecessidadeEspecial = !rblAcessoNecEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblAcessoNecEspecial.SelectedValue : null,
                    AcessoDificil = chkAcessoDificil.Checked ? "S" : "N",
                    Extraclasse = chkExtraclasse.Checked ? "S" : "N",

                    DependenciaAdministrativa = !ddlDependenciaAdministrativa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDependenciaAdministrativa.SelectedValue : null,
                    Email = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null,
                    Cnpj = !txtCGC.Text.IsNullOrEmptyOrWhiteSpace() ? txtCGC.Text.Trim() : null,
                    Telefone1 = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.Trim() : null,
                    Telefone2 = !txtFone2.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone2.Text.Trim() : null,
                    Fax = !txtFax.Text.IsNullOrEmptyOrWhiteSpace() ? txtFax.Text.Trim() : null,
                    AreaTerreno = !txtAreaTerreno.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtAreaTerreno.Text.Trim()) : (decimal?)null,
                    UsuarioResponsavel = User.Identity.Name,
                    AreaTotalTerreno = !txtAreaTotalTerreno.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtAreaTotalTerreno.Text.Trim()) : (decimal?)null,
                    AreaTotalConstruida = !txtAreaConstruida.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtAreaConstruida.Text.Trim()) : (decimal?)null,
                };

                if (!rblAcessoNecEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() && rblAcessoNecEspecial.SelectedValue == "S")
                {
                    foreach (ListItem item in chkRecursoAcessibilidade.Items)
                    {
                        if (item.Selected)
                        {
                            listRecursoAcessibilidade.Add(Convert.ToInt32(item.Value));
                        }
                    }

                    infoFisicas.FormasAcessibilidade = listRecursoAcessibilidade;
                }

                validacao = rnUnidadeFisica.ValidaCaracteristicasFisicas(infoFisicas);

                if (validacao.Valido)
                {
                    rnUnidadeFisica.AtualizaCaracteristicasFisicas(infoFisicas);

                    if (!chkImovelCompartilhado.Checked && !chkImovelCompartilhadoFisica.Checked)
                    {
                        pcUnidade.TabPages[3].Enabled = false;
                    }
                    else
                    {
                        pcUnidade.TabPages[3].Enabled = true;
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                   "alert('Unidade atualizada com sucesso.');", true);

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

        }

    #endregion


    #region Handlers de Botões — Dependências e Validação

        protected void btnSalvarQtdDependencias_Click(object sender, EventArgs e)
        {
            List<TceTipoDependenciaUnidadeFisica> listaTipoDependencia = new List<TceTipoDependenciaUnidadeFisica>();
            string mensagem = string.Empty;
            string text = string.Empty;
            string hidden = string.Empty;
            int salaClimatizada;
            int salaAcessibilidade;
            int salaCantinhoLeitura;
            ValidacaoDados validacao = new ValidacaoDados();

            if (tseUnidade.DBValue.IsNull || !tseUnidade.IsValidDBValue)
            {
                lblMensagem.Text = "Favor informar uma unidade física válida.";
                return;
            }

            try
            {
                foreach (DataRow row in RN.TipoDependenciaUnidadeFisica.Listar(tseUnidade.DBValue.ToString()).Rows)
                {
                    TceTipoDependenciaUnidadeFisica dependencia = new TceTipoDependenciaUnidadeFisica();

                    text = Request.Params["ctl00$cphFormulario$pcUnidade$txtQt" + row["Tipo_Depend"].ToString()];
                    hidden = Request.Params["ctl00$cphFormulario$pcUnidade$hd" + row["Tipo_Depend"].ToString()];

                    dependencia.Matricula = User.Identity.Name;
                    dependencia.Quantidade = !string.IsNullOrEmpty(text) ? int.Parse(text) : 0;
                    dependencia.TipoDependencia = hidden;
                    dependencia.UnidadeFisica = tseUnidade.DBValue.ToString();

                    listaTipoDependencia.Add(dependencia);
                }

                foreach (DataRow row in RN.Dependencia.ListarQuantidadeTipoSala(tseUnidade.DBValue.ToString()).Rows)
                {
                    TceTipoDependenciaUnidadeFisica dependencia = new TceTipoDependenciaUnidadeFisica();

                    text = Request.Params["ctl00$cphFormulario$pcUnidade$txt" + row["Tipo_Depend"].ToString()];
                    hidden = Request.Params["ctl00$cphFormulario$pcUnidade$hs" + row["Tipo_Depend"].ToString()];

                    dependencia.Matricula = User.Identity.Name;
                    dependencia.Quantidade = !string.IsNullOrEmpty(text) ? int.Parse(text) : 0;
                    dependencia.TipoDependencia = hidden;
                    dependencia.UnidadeFisica = tseUnidade.DBValue.ToString();

                    listaTipoDependencia.Add(dependencia);
                }

                var script = "";
                foreach (var tipo in listaTipoDependencia)
                {
                    var SalaAlter = new DadosSalaAlternativa
                    {
                        FACULDADE = tseUnidade.DBValue.ToString(),
                        TIPO_DEPEND = tipo.TipoDependencia,
                        quatidade = tipo.Quantidade
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
                    RN.TipoDependenciaUnidadeFisica.Alterar(listaTipoDependencia, salaClimatizada, salaAcessibilidade, salaCantinhoLeitura);
                    mensagem = "As quantidades de dependências foram atualizadas com sucesso.";
                    script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    var data = TipoDependenciaUnidadeFisica.VerificaDataAlteracao(tseUnidade.DBValue.ToString());

                    if (!string.IsNullOrEmpty(data))
                    {
                        lblDataAlteracao.Text = "Data da última alteração: " + data.ToString();
                        lblDataAlteracao.Visible = true;
                    }
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
        }

        protected void btnValidarDependencias_Click(object sender, EventArgs e)
        {
            try
            {
                var TVTP = new TceValidacaoTipoDependencia()
                {
                    Matricula = User.Identity.Name,
                    Status = ValidacaoTipoDependencia.Validado.Trim(),
                    UnidadeFisica = tseUnidade.DBValue.ToString(),
                };
                var validacao = ValidacaoTipoDependencia.Validar(TVTP);

                if (validacao.Valido)
                {
                    ValidacaoTipoDependencia.Inserir(TVTP);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Validação de Dependência efetuada com sucesso.');", true);
                    btnValidarDependencias.Visible = false;
                    btnInvalidarDependencias.Visible = false;
                    btnReabrirValidacao.Visible = true;

                    //ControlarTipoOperacao(); TODO
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnInvalidarDependencias_Click(object sender, EventArgs e)
        {
            try
            {
                var TVTP = new TceValidacaoTipoDependencia()
                {
                    Matricula = User.Identity.Name,
                    Status = ValidacaoTipoDependencia.NaoConfirmado.Trim(),
                    UnidadeFisica = tseUnidade.DBValue.ToString(),
                };
                var validacao = ValidacaoTipoDependencia.Validar(TVTP);

                if (validacao.Valido)
                {
                    ValidacaoTipoDependencia.Inserir(TVTP);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não Validação de Dependência efetuada com sucesso.');", true);
                    btnValidarDependencias.Visible = true;
                    btnInvalidarDependencias.Visible = false;

                    //ControlarTipoOperacao(); TODO
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReabrirValidacao_Click(object sender, EventArgs e)
        {
            try
            {
                var TVTP = new TceValidacaoTipoDependencia()
                {
                    Matricula = User.Identity.Name,
                    Status = ValidacaoTipoDependencia.Reaberto.Trim(),
                    UnidadeFisica = tseUnidade.DBValue.ToString(),
                };
                var validacao = ValidacaoTipoDependencia.Validar(TVTP);

                if (validacao.Valido)
                {
                    ValidacaoTipoDependencia.Inserir(TVTP);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Dependência reaberta para alterações.');", true);
                    btnValidarDependencias.Visible = false;
                    btnInvalidarDependencias.Visible = false;
                    //ControlarTipoOperacao();
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    #endregion


    #region Handlers de Botões — Concessionárias

        protected void btnSalvarConcessionaria_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                LyUnidadeFisicaConcessionaria dadosConcessionarias = new LyUnidadeFisicaConcessionaria();
                RN.UnidadeFisicaConcessionaria rnUnidadeFisicaConcessionaria = new Techne.Lyceum.RN.UnidadeFisicaConcessionaria();
                StringBuilder tipo_abast_energia = new StringBuilder();
                StringBuilder outros_abast_agua = new StringBuilder();
                StringBuilder esgoto = new StringBuilder();
                StringBuilder lixo = new StringBuilder();
                List<int> listTratamentoLixo = new List<int>();

                foreach (ListItem item in chkTipoAbastecimentoEnergia.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                        tipo_abast_energia.Append(item.Value + ";");
                }

                foreach (ListItem item in chkOutrosAbastecimentos.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                        outros_abast_agua.Append(item.Value + ";");
                }

                foreach (ListItem item in chkEsgoto.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                        esgoto.Append(item.Value + ";");
                }

                foreach (ListItem item in chkLixo.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                        lixo.Append(item.Value + ";");
                }


                foreach (ListItem item in chkTratamentoLixo.Items)
                {
                    if (item.Selected)
                    {
                        listTratamentoLixo.Add(Convert.ToInt32(item.Value));
                    }
                }

                dadosConcessionarias.UnidadeFis = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? tseUnidade.DBValue.ToString() : null;
                dadosConcessionarias.IdUnidadeFisicaConcessionaria = !hdnCodigoConcessionaria.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnCodigoConcessionaria.Value) : 0;
                //ENERGIA
                dadosConcessionarias.EeTipoAbastecimento = (tipo_abast_energia.Length > 0) ? tipo_abast_energia.ToString(0, tipo_abast_energia.Length - 1) : null;
                dadosConcessionarias.EeEmpresa = (tseEE_Concessionaria.IsValidDBValue && !tseEE_Concessionaria.DBValue.IsNull) ? Convert.ToString(tseEE_Concessionaria["empresa"]) : null;
                dadosConcessionarias.EeCodCliente = !txtEE_NumeroCliente.Text.IsNullOrEmptyOrWhiteSpace() ? txtEE_NumeroCliente.Text.Trim() : null;
                dadosConcessionarias.EeClasseFornecimento = !ddlEE_ClasseFornecimento.Text.IsNullOrEmptyOrWhiteSpace() ? ddlEE_ClasseFornecimento.Text : null;
                if (chkEE_Contrato.Value != null)
                {
                    dadosConcessionarias.EeContratoFornecimento = (chkEE_Contrato.Checked) ? "S" : null;
                }

                //AGUA
                dadosConcessionarias.AgEmpresa = (tseAgua_Concessionaria.IsValidDBValue && !tseAgua_Concessionaria.DBValue.IsNull) ? Convert.ToString(tseAgua_Concessionaria["empresa"]) : null;
                dadosConcessionarias.AgCodCliente = !txtAgua_NumeroCliente.Text.IsNullOrEmptyOrWhiteSpace() ? txtAgua_NumeroCliente.Text.Trim() : null;
                dadosConcessionarias.AgPcVazao = !txtAgPoco_Vazao.Text.IsNullOrEmptyOrWhiteSpace() ? txtAgPoco_Vazao.Text.Trim() : null;
                dadosConcessionarias.AgPcProfundidade = !txtAgPoco_Profundidade.Text.IsNullOrEmptyOrWhiteSpace() ? txtAgPoco_Profundidade.Text.Trim() : null;
                dadosConcessionarias.AgHidrometro = chkAgua_Hidrometro.Checked ? chkAgua_Hidrometro.Value.ToString() : null;
                dadosConcessionarias.AgPcArtesiano = chkPoco.Items[0].Selected ? "S" : null;
                dadosConcessionarias.AgPcSemiArtesiano = chkPoco.Items[1].Selected ? "S" : null;
                dadosConcessionarias.AgPcCacimba = chkPoco.Items[2].Selected ? "S" : null;
                dadosConcessionarias.AgPcCarroPipa = chkPoco.Items[3].Selected ? "S" : null;
                dadosConcessionarias.AgPcBombaSubmersa = cbAgPoco_Bomba.Checked ? cbAgPoco_Bomba.Value.ToString() : null;
                dadosConcessionarias.AgTipoAguaConsumida = !rblTipoAguaConsumida.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTipoAguaConsumida.SelectedValue : null;
                dadosConcessionarias.AgOutrosAbastecimentos = (outros_abast_agua.Length > 0) ? outros_abast_agua.ToString(0, outros_abast_agua.Length - 1) : null;

                //GAS
                dadosConcessionarias.GaEmpresa = (tseGas_Concessionaria.IsValidDBValue && !tseGas_Concessionaria.DBValue.IsNull) ? Convert.ToString(tseGas_Concessionaria["empresa"]) : null;
                dadosConcessionarias.GaCodCliente = !txtGas_NumeroCliente.Text.IsNullOrEmptyOrWhiteSpace() ? txtGas_NumeroCliente.Text.Trim() : null;
                dadosConcessionarias.GaTipo = !ddlGas_Tipo.Text.IsNullOrEmptyOrWhiteSpace() ? ddlGas_Tipo.Text : null;

                //ESGOTO
                dadosConcessionarias.ElDestinacaoLixo = (lixo.Length > 0) ? lixo.ToString(0, lixo.Length - 1) : null;
                dadosConcessionarias.ElEsgotoSanitario = (esgoto.Length > 0) ? esgoto.ToString(0, esgoto.Length - 1) : null;

                dadosConcessionarias.Matricula = User.Identity.Name;

                validacao = rnUnidadeFisicaConcessionaria.Valida(dadosConcessionarias, listTratamentoLixo);

                if (validacao.Valido)
                {
                    rnUnidadeFisicaConcessionaria.Salva(dadosConcessionarias, listTratamentoLixo);
                    hdnCodigoConcessionaria.Value = dadosConcessionarias.IdUnidadeFisicaConcessionaria.ToString();

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                  "alert('Concessionárias atualizada com sucesso.');", true);
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
        }

    #endregion


    #region Handlers de Botões — Municipalização e Documentos

        protected void btnVerOfertas_Click(object sender, EventArgs e)
        {
            var index = grdCompartilhada.FocusedRowIndex;

            hdnCensoCompartilhada.Value = string.Empty;
            lblCensoCompartilhadoSelecionado.Text = string.Empty;
            grdCompartilhadaOferta.ClientVisible = false;
            grdCompartilhadaOfertaEstadual.ClientVisible = false;

            if (index > -1)
            {
                var idCompartilhada = Convert.ToInt32(grdCompartilhada.GetRowValues(index, "id_compartilhada"));
                var redeEnsino = Convert.ToString(grdCompartilhada.GetRowValues(index, "rede_ensino"));
                var censo = Convert.ToString(grdCompartilhada.GetRowValues(index, "censo"));
                var nome = Convert.ToString(grdCompartilhada.GetRowValues(index, "nome"));

                if (idCompartilhada <= 0 || string.IsNullOrEmpty(redeEnsino))
                {
                    lblMensagem.Text = "Para visualizar as ofertas, é necessário selecionar um compartilhamento!";

                    return;
                }

                lblMensagem.Text = string.Empty;
                hdnCensoCompartilhada.Value = idCompartilhada.ToString();
                lblCensoCompartilhadoSelecionado.Text = string.Format("{0} - {1}", censo, nome);

                if (redeEnsino == "Estadual")
                {
                    grdCompartilhadaOfertaEstadual.ClientVisible = true;
                    grdCompartilhadaOfertaEstadual.DataSource = CompartilhadaOferta.ListarEstaduais(idCompartilhada);
                    grdCompartilhadaOfertaEstadual.DataBind();
                }
                else
                {
                    grdCompartilhadaOferta.ClientVisible = true;
                    odsCompartilhadaOferta.Select();
                    grdCompartilhadaOferta.DataBind();
                }
            }
            else
            {
                lblMensagem.Text = "Para visualizar as ofertas, é necessário selecionar um compartilhamento!";
            }
        }

        protected void btnSalvarMunic_Click(object sender, EventArgs e)
        {
            try
            {
                Municipalizacao rnMunicipalizacao = new Municipalizacao();

                var dtMunic = new TceMunicipalizacao
                {
                    IdMunicipalizacao = !hdnMunicipalizacao.Value.IsNullOrEmptyOrWhiteSpace() ? int.Parse(hdnMunicipalizacao.Value) : 0,
                    Censo = tseUnidade.DBValue.ToString(),
                    Processo = txtProcessoMunic.Text.Trim(),
                    DtPublicacaoDo =
                        !string.IsNullOrEmpty(dtPublicacaoDO.Text)
                            ? dtPublicacaoDO.Date
                            : (DateTime?)null,
                    PaginaDo = txtPaginaDO.Text.Trim(),
                    NumAutorizoProvisorio = txtAutorizoProv.Text.Trim(),
                    DtAutorizoProvisorio = dtAutorizoProv.Date,
                    DtValidadeAutorizo = dtValidadeAutorizo.Date,
                    Matricula = User.Identity.Name
                };

                var validacao = Municipalizacao.Validar(dtMunic);

                if (validacao.Valido)
                {
                    string mensagem;

                    if (dtMunic.IdMunicipalizacao == 0)
                    {
                        var id = rnMunicipalizacao.Insere(dtMunic);
                        hdnMunicipalizacao.Value = id.ToString();
                        mensagem = "Municipalização incluída com sucesso.";
                    }
                    else
                    {
                        Municipalizacao.Alterar(dtMunic);
                        mensagem = "Municipalização alterada com sucesso.";
                    }

                    grdDocumentosCelebrados.DataBind();

                    var script = @"alert('" + mensagem + @"');";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", script, true);

                    var data = TipoDependenciaUnidadeFisica.VerificaDataAlteracao(tseUnidade.DBValue.ToString());

                    if (!string.IsNullOrEmpty(data))
                    {
                        lblDataAlteracao.Text = "Data da última alteração: " + data.ToString();
                        lblDataAlteracao.Visible = true;
                    }
                    CarregaNumProcessoMunicipalizacao();
                    LimpaCamposMunicipalizacaoVigente();
                    CarregaMunicipalizacaoVigente();
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
        }

        protected void btnSimDocCelebrado_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                DocCelebradoMunicipalizacao rnDocCelebradoMunicipalizacao = new DocCelebradoMunicipalizacao();

                int Id = 0;

                Id = Convert.ToInt32(hdnIdDocCelebrado.Value);

                validacao = rnDocCelebradoMunicipalizacao.ValidaRemocao(Id);

                if (validacao.Valido)
                {
                    rnDocCelebradoMunicipalizacao.Remover(Id);
                    grdDocumentosCelebrados.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdDocumentosCelebrados.CancelEdit();
                }

                this.pucConfirmarDocCelebrado.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNaoDocCelebrado_Click(object sender, EventArgs e)
        {
            this.pucConfirmarDocCelebrado.ShowOnPageLoad = false;
            grdDocumentosCelebrados.CancelEdit();
        }

        protected void btnSalvarDocCelebrado_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                DocCelebradoMunicipalizacao rnDocCelebradoMunicipalizacao = new DocCelebradoMunicipalizacao();
                TceDocCelebradoMunicipalizacao docCelebrado = new TceDocCelebradoMunicipalizacao();

                docCelebrado.IdDocCelebradoMunicipalizacao = !hdnIdDocCelebrado.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdDocCelebrado.Value) : -1;
                docCelebrado.Matricula = User.Identity.Name;
                docCelebrado.IdMunicipalizacao = !ddlNumProcessoDocCelebrado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNumProcessoDocCelebrado.SelectedValue) : -1;
                docCelebrado.Numero = !txtNumeroDocCelebrado.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumeroDocCelebrado.Text.Trim() : null;
                docCelebrado.Tipo = !ddlTipoDocCelebrado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipoDocCelebrado.SelectedValue : null;
                docCelebrado.DtCelebracao = !dtDataValidadeInicio.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataValidadeInicio.Date : DateTime.MinValue;
                docCelebrado.DtValidade = !dtDataValidadeFinal.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataValidadeFinal.Date : DateTime.MinValue;
                docCelebrado.Observacao = !txtObsDocCelebrado.Text.IsNullOrEmptyOrWhiteSpace() ? txtObsDocCelebrado.Text.Trim() : null;

                validacao = rnDocCelebradoMunicipalizacao.Validar(docCelebrado);

                if (validacao.Valido)
                {
                    if (docCelebrado.IdDocCelebradoMunicipalizacao == -1)
                    {
                        rnDocCelebradoMunicipalizacao.Inserir(docCelebrado);
                    }
                    else
                    {
                        rnDocCelebradoMunicipalizacao.Alterar(docCelebrado);
                    }

                    grdDocumentosCelebrados.DataBind();
                    LimpaCamposDocCelebrado();
                    LimpaCamposMunicipalizacao();
                    LimpaCamposMunicipalizacaoVigente();
                    pnlDocCelebrado.Visible = false;
                    pnlDadosMunicipalizacao.Visible = false;
                    CarregaMunicipalizacaoVigente();

                    lblMensagem.Text = "Documento Celebrado " + (docCelebrado.IdDocCelebradoMunicipalizacao == -1 ? "cadastrado" : "atualizado") + " com sucesso.";

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdDocumentosCelebrados.CancelEdit();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    #endregion


    #region Handlers de Botões — Equipamentos

        protected void btnSalvarQtdEquipamentos_Click(object sender, EventArgs e)
        {
            List<DadosEquipamentoUnidadeFisica> listaEquipamentos = new List<DadosEquipamentoUnidadeFisica>();
            List<DadosEquipamentoUnidadeFisica> listaMaximosAtingidos = new List<DadosEquipamentoUnidadeFisica>();
            List<DadosEquipamentoUnidadeFisica> listaVinculos = new List<DadosEquipamentoUnidadeFisica>();
            this.trMaximos.Visible = false;
            this.btnConfirmarEquipamentos.Visible = false;

            if (tseUnidade.DBValue.IsNull || !tseUnidade.IsValidDBValue)
            {
                lblMensagem.Text = "Favor informar uma unidade física válida.";
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

                    this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
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
            }
        }

    #endregion


    #region Handlers de Botões — Internet e Pedagógicos

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

                unidadeDadosPedagogicos.Censo = (!tseUnidade.DBValue.IsNull && tseUnidade.IsValidDBValue) ? tseUnidade.DBValue.ToString() : null;
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

                unidadeDadosPedagogicos.Educacaoambiental = !rblEducacaoambiental.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblEducacaoambiental.SelectedValue : null;
                unidadeDadosPedagogicos.ConteudoComponentes = !rblConteudoComponentes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblConteudoComponentes.SelectedValue : null;
                unidadeDadosPedagogicos.Componentecurricular = !rblComponenteCurricular.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblComponenteCurricular.SelectedValue : null;
                unidadeDadosPedagogicos.EixoEstuturante = !rblEixoEstuturante.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblEixoEstuturante.SelectedValue : null;
                unidadeDadosPedagogicos.EmEventos = !rblEmEventos.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblEmEventos.SelectedValue : null;
                unidadeDadosPedagogicos.ProjetosTransversais = !rblProjetosTransversais.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblProjetosTransversais.SelectedValue : null;
                unidadeDadosPedagogicos.NOL = !rblNOL.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblNOL.SelectedValue : null;


                /*              if (rblNOL.SelectedValue == "N")
                              {
                                 if( rblEducacaoambiental.SelectedValue = "S" ||
                                  rblConteudoComponentes.SelectedValue = "N";
                                  rblComponenteCurricular.SelectedValue = "N";
                                  rblEixoEstuturante.SelectedValue = "N";
                                  rblEmEventos.SelectedValue = "N";
                                  rblProjetosTransversais.SelectedValue = "N";
                              }
                              */

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

                unidadeDadosInternet.Censo = (!tseUnidade.DBValue.IsNull && tseUnidade.IsValidDBValue) ? tseUnidade.DBValue.ToString() : null;
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

    #endregion


    #region Handlers de Grid — Cursos por Unidade

        protected void odsCursoPorUnidade_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var unidade_ens = e.InputParameters["unidade_ens"].ToString();
            var curso = e.InputParameters["curso"].ToString();

            UnidadeEnsinoCursos.Remover(unidade_ens, curso);
        }

        protected void odsCursoPorUnidade_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var linhanova = new LyUnidadeEnsinoCursos
            {
                UnidadeEns = Convert.ToString(tseUnidade.DBValue),
                Curso = e.InputParameters["nome"].ToString(),
                Ato = Convert.ToString(e.InputParameters["ato"]),
                DtImplantacao = (e.InputParameters["dt_implantacao"] != null) ? Convert.ToDateTime(e.InputParameters["dt_implantacao"]) : (DateTime?)null,
                DtDo = (e.InputParameters["dt_do"] != null) ? Convert.ToDateTime(e.InputParameters["dt_do"]) : (DateTime?)null,
                Matricula = User.Identity.Name,
                Observacoes = Convert.ToString(e.InputParameters["OBSERVACOES"]),
                Processo = Convert.ToString(e.InputParameters["PROCESSO"])
            };

            var unidade = linhanova.UnidadeEns;
            var curso = linhanova.Curso;
            var qt = Curso.ConsultarCursoPorUnidade(unidade, curso);

            if (qt.Rows.Count <= 0)
            {
                var validacao = UnidadeEnsinoCursos.Validar(linhanova);

                if (validacao.Valido)
                {
                    UnidadeEnsinoCursos.Inserir(linhanova);
                }
                else
                {
                    throw new Exception(validacao.Mensagem);
                }
            }
            else
            {
                throw new Exception("Registro já existe. Favor escolher outro curso.");
            }
        }

        protected void odsCursoPorUnidade_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var linhanova = new LyUnidadeEnsinoCursos
            {
                UnidadeEns = e.InputParameters["unidade_ens"].ToString(),
                Curso = e.InputParameters["codigo"].ToString(),
                Ato = Convert.ToString(e.InputParameters["ato"]),
                DtImplantacao = (e.InputParameters["dt_implantacao"] != null) ? Convert.ToDateTime(e.InputParameters["dt_implantacao"]) : (DateTime?)null,
                DtDo = (e.InputParameters["dt_do"] != null) ? Convert.ToDateTime(e.InputParameters["dt_do"]) : (DateTime?)null,
                Matricula = User.Identity.Name,
                Observacoes = Convert.ToString(e.InputParameters["OBSERVACOES"]),
                Processo = Convert.ToString(e.InputParameters["PROCESSO"])
            };

            var unidade = linhanova.UnidadeEns;
            var curso = linhanova.Curso;

            var validacao = UnidadeEnsinoCursos.Validar(linhanova);

            if (validacao.Valido)
            {
                UnidadeEnsinoCursos.Alterar(linhanova);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdCursoPorUnidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCursoPorUnidade);
        }

        protected void grdCursoPorUnidade_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdCursoPorUnidade.IsNewRowEditing)
            {
                if (e.Column.FieldName == "nome")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdCursoPorUnidade.IsEditing)
            {
                if (e.Column.FieldName == "nome")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdCursoPorUnidade_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var unidade = Convert.ToString(e.GetListSourceFieldValue("unidade_ens"));
                var curso = Convert.ToString(e.GetListSourceFieldValue("codigo"));
                e.Value = unidade + "|" + curso;
            }
        }

        protected void grdCursoPorUnidade_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("unidade_ens", e.Values["unidade_ens"]);
            e.Keys.Add("curso", e.Values["codigo"]);
        }

        protected void grdCursoPorUnidade_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var chaves = e.Keys["CompositeKey"].ToString().Split('|');
            e.Keys.Clear();
            e.Keys.Add("unidade_ens", chaves[0]);
            e.Keys.Add("curso", chaves[1]);
        }

    #endregion


    #region Handlers de Grid — Edificações e Pavimentos

        public void DeleteEdifPav(object unidade_fis, object EDIFICACAO, object PAVIMENTO)
        {
        }

        public void InsertEdifPav(object EDIFICACAO, object NOME_EDIFICACAO, object PAVIMENTO, object NOME_PAVIMENTO)
        {
        }

        public void UpdateEdifPav(object NOME_EDIFICACAO, object NOME_PAVIMENTO, object UNIDADE_FIS, object EDIFICACAO, object PAVIMENTO)
        {
        }

        protected void odsEdificacoesPavimentos_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var edifpav = new LyUnidadeFisicaEdificacao
            {
                UnidadeFis = Convert.ToString(tseUnidade.DBValue),
                Edificacao = Convert.ToString(e.InputParameters["EDIFICACAO"]),
                NomeEdificacao = Convert.ToString(e.InputParameters["NOME_EDIFICACAO"]),
                Pavimento = Convert.ToString(e.InputParameters["PAVIMENTO"]),
                NomePavimento = Convert.ToString(e.InputParameters["NOME_PAVIMENTO"]),
                Matricula = User.Identity.Name,
            };

            var validacao = UnidadeFisicaEdificacao.Validar(edifpav, true);

            if (validacao.Valido)
            {
                RN.UnidadeFisicaEdificacao.Inserir(edifpav);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsEdificacoesPavimentos_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var edifpav = new LyUnidadeFisicaEdificacao
            {
                UnidadeFis = Convert.ToString(tseUnidade.DBValue),
                Edificacao = Convert.ToString(e.InputParameters["EDIFICACAO"].ToString()),
                NomeEdificacao = Convert.ToString(e.InputParameters["NOME_EDIFICACAO"].ToString()),
                Pavimento = Convert.ToString(e.InputParameters["PAVIMENTO"].ToString()),
                NomePavimento = Convert.ToString(e.InputParameters["NOME_PAVIMENTO"].ToString()),
                Matricula = User.Identity.Name
            };

            var validacao = UnidadeFisicaEdificacao.Validar(edifpav, false);

            if (validacao.Valido)
            {
                RN.UnidadeFisicaEdificacao.Alterar(edifpav);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsEdificacoesPavimentos_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var edifpav = new LyUnidadeFisicaEdificacao
            {
                UnidadeFis = Convert.ToString(tseUnidade.DBValue),
                Edificacao = Convert.ToString(e.InputParameters["EDIFICACAO"]),
                Pavimento = Convert.ToString(e.InputParameters["PAVIMENTO"]),
                Matricula = User.Identity.Name
            };

            var validacao = UnidadeFisicaEdificacao.ValidarRemover(edifpav.UnidadeFis, edifpav.Edificacao, edifpav.Pavimento);

            if (validacao.Valido)
            {
                UnidadeFisicaEdificacao.Remover(edifpav.UnidadeFis, edifpav.Edificacao, edifpav.Pavimento);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdEdificacoesPavimentos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEdificacoesPavimentos);
        }

        protected void grdEdificacoesPavimentos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdEdificacoesPavimentos.IsNewRowEditing)
            {
                if (e.Column.FieldName == "EDIFICACAO")
                    e.Editor.Enabled = true;
                if (e.Column.FieldName == "PAVIMENTO")
                    e.Editor.Enabled = true;
            }
            else
            {
                if (grdEdificacoesPavimentos.IsEditing)
                {
                    if (e.Column.FieldName == "EDIFICACAO")
                        e.Editor.Enabled = false;
                    if (e.Column.FieldName == "PAVIMENTO")
                        e.Editor.Enabled = false;
                }
            }
        }

        protected void grdEdificacoesPavimentos_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string faculdade = Convert.ToString(e.GetListSourceFieldValue("UNIDADE_FIS"));
                string dependencia = Convert.ToString(e.GetListSourceFieldValue("EDIFICACAO"));
                string pavimento = Convert.ToString(e.GetListSourceFieldValue("PAVIMENTO"));
                e.Value = faculdade + "-" + dependencia + "-" + pavimento;
            }
        }

        protected void grdEdificacoesPavimentos_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("UNIDADE_FIS", chaves[0]);
            e.Keys.Add("EDIFICACAO", chaves[1]);
            e.Keys.Add("PAVIMENTO", chaves[2]);
        }

        protected void grdEdificacoesPavimentos_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["unidade_fis"] = tseUnidade.DBValue.ToString();
        }

        protected void grdEdificacoesPavimentos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("UNIDADE_FIS", chaves[0]);
            e.Keys.Add("EDIFICACAO", chaves[1]);
            e.Keys.Add("PAVIMENTO", chaves[2]);
        }

    #endregion


    #region Handlers de Grid — Dependências (Sala de Aula)

        protected void grdDependencias_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDependencias);
        }

        protected void grdDependencias_AutoFilterCellEditorCreate(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "ativa")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Items.Clear();

                var check = e.Column as GridViewDataCheckColumn;

                if (check == null)
                {
                    return;
                }

                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }
        }

        protected void grdDependencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdDependencias.Settings.ShowFilterRow = false;
        }

        protected void grdDependencias_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdDependencias.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "dependencia")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.Value = "SL-";
                }
            }

            else if (grdDependencias.IsEditing)
            {
                if ((e.Column.FieldName) == "dependencia")
                    e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "tipo_depend")
            {
                var cmbTipoDependencia = e.Editor as ASPxComboBox;

                if (cmbTipoDependencia == null)
                {
                    return;
                }

                cmbTipoDependencia.Items.Clear();
                cmbTipoDependencia.DataSource = TipoDependencia.ListarSalaAula();
                cmbTipoDependencia.TextField = "NOME";
                cmbTipoDependencia.ValueField = "TIPO_DEPEND";
                cmbTipoDependencia.DataBind();

                var item = cmbTipoDependencia.Items.FindByText((string)e.Value);

                if (item != null)
                {
                    item.Selected = true;
                }
            }

            if (e.Column.FieldName == "pavimento")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += cmbPavimentacao_OnCallback;
            }

            if (grdDependencias.IsEditing && e.Column.FieldName == "pavimento" && e.KeyValue != DBNull.Value && e.KeyValue != null)
            {
                var val = grdDependencias.GetRowValuesByKeyValue(e.KeyValue, "edificacao");

                if (val == DBNull.Value)
                {
                    return;
                }

                var edificacao = (string)val;
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                CarregarPavimentos(combo, edificacao);
            }
        }

        protected void grdDependencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDependencias.Settings.ShowFilterRow = false;
        }

        public void Delete(object FACULDADE, object DEPENDENCIA)
        {
        }

        protected void odsDependencias_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Area = e.InputParameters["area"] == null ? (double?)null : Convert.ToDouble(e.InputParameters["area"]),
                Ativa = Convert.ToString(e.InputParameters["ativa"]),
                SalaAnexa = (e.InputParameters["sala_anexa"] == null || e.InputParameters["sala_anexa"] == string.Empty) ? "N" : Convert.ToString(e.InputParameters["sala_anexa"]),
                Faculdade = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? Convert.ToString(tseUnidade.DBValue) : null,
                Dependencia = e.InputParameters["dependencia"] != null ? Convert.ToString(e.InputParameters["dependencia"]) : null,
                Descricao = e.InputParameters["descricao"] != null ? Convert.ToString(e.InputParameters["descricao"]) : null,
                Obs = e.InputParameters["obs"] != null ? Convert.ToString(e.InputParameters["obs"]) : null,
                Edificacao = e.InputParameters["edificacao"] != null ? Convert.ToString(e.InputParameters["edificacao"]) : null,
                Pavimento = e.InputParameters["pavimento"] != null ? Convert.ToString(e.InputParameters["pavimento"]) : null,
                TipoDepend = e.InputParameters["tipo_depend"] != null ? Convert.ToString(e.InputParameters["tipo_depend"]) : null,
                Matricula = User.Identity.Name,
                NumAlunos = e.InputParameters["area"] != null ? Convert.ToInt32(Math.Round(Convert.ToDouble(e.InputParameters["area"]) * 0.8)) : -1
            };

            var validacao = Dependencia.ValidarInserir(dep);

            if (validacao.Valido)
            {
                Dependencia.InserirSalaDeAula(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsDependencias_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Area = e.InputParameters["area"] == null ? (double?)null : Convert.ToDouble(e.InputParameters["area"]),
                Ativa = Convert.ToString(e.InputParameters["ativa"]),
                SalaAnexa = (e.InputParameters["sala_anexa"] == null || e.InputParameters["sala_anexa"] == string.Empty) ? "N" : Convert.ToString(e.InputParameters["sala_anexa"]),
                Faculdade = e.InputParameters["faculdade"] != null ? Convert.ToString(e.InputParameters["faculdade"]) : null,
                Dependencia = e.InputParameters["dependencia"] != null ? Convert.ToString(e.InputParameters["dependencia"]) : null,
                Descricao = e.InputParameters["descricao"] != null ? Convert.ToString(e.InputParameters["descricao"]) : null,
                Obs = e.InputParameters["obs"] != null ? Convert.ToString(e.InputParameters["obs"]) : null,
                Edificacao = e.InputParameters["edificacao"] != null ? Convert.ToString(e.InputParameters["edificacao"]) : null,
                Pavimento = e.InputParameters["pavimento"] != null ? Convert.ToString(e.InputParameters["pavimento"]) : null,
                TipoDepend = e.InputParameters["tipo_depend"] != null ? Convert.ToString(e.InputParameters["tipo_depend"]) : null,
                Matricula = User.Identity.Name,
                NumAlunos = e.InputParameters["area"] != null ? Convert.ToInt32(Math.Round(Convert.ToDouble(e.InputParameters["area"]) * 0.8)) : -1
            };

            var validacao = Dependencia.ValidarAlterar(dep);

            if (validacao.Valido)
            {
                Dependencia.Alterar(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsDependencias_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Faculdade = Convert.ToString(e.InputParameters["faculdade"]),
                Dependencia = Convert.ToString(e.InputParameters["dependencia"]),
                Matricula = User.Identity.Name,
            };

            var validacao = Dependencia.ValidarRemover(dep);

            if (validacao.Valido)
            {
                Dependencia.Remover(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdDependencias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var chaves = e.Keys["CompositeKey"].ToString().Split('-');

            e.Keys.Clear();

            e.Keys.Add("faculdade", chaves[0]);

            string valores = string.Empty;
            string chavefinal = string.Empty;
            for (int i = 1; i < chaves.Length; i++)
            {
                valores = valores + chaves[i] + "-";
            }

            chavefinal = valores.Substring(0, valores.Length - 1);

            e.Keys.Add("dependencia", chavefinal);
        }

        protected void grdDependencias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();

            e.Keys.Add("faculdade", e.Values["faculdade"]);
            e.Keys.Add("dependencia", e.Values["dependencia"]);
        }

        protected void grdDependencias_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
            {
                e.NewValues["faculdade"] = tseUnidade.DBValue.ToString();
            }

            if (e.NewValues["ativa"] == null)
            {
                e.NewValues["ativa"] = "N";
            }
        }

        protected void grdDependencias_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "obs"
                && e.Value != null)
            {
                if (!string.IsNullOrEmpty(e.Value.ToString()))
                {
                    var obs = e.Value.ToString();

                    e.DisplayText = obs.Length > 100 ? obs.Substring(0, 100) : obs;
                }
            }
        }

        protected void grdDependencias_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var faculdade = Convert.ToString(e.GetListSourceFieldValue("faculdade"));
                var dependencia = Convert.ToString(e.GetListSourceFieldValue("dependencia"));

                e.Value = faculdade + "-" + dependencia;
            }
        }

    #endregion


    #region Handlers de Grid — Demais Dependências (Salas de Recursos)

        protected void grdDemaisDependencias_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDemaisDependencias);
        }

        protected void grdDemaisDependencias_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdDemaisDependencias.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "dependencia")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.Value = "SR-";
                }
            }

            else if (grdDemaisDependencias.IsEditing)
            {
                if ((e.Column.FieldName) == "dependencia")
                    e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "tipo_depend")
            {
                var cmbTipoDependencia = e.Editor as ASPxComboBox;

                if (cmbTipoDependencia == null)
                {
                    return;
                }

                cmbTipoDependencia.Items.Clear();
                cmbTipoDependencia.DataSource = TipoDependencia.ListarSalaRecurso();
                cmbTipoDependencia.TextField = "NOME";
                cmbTipoDependencia.ValueField = "TIPO_DEPEND";
                cmbTipoDependencia.DataBind();

                var item = cmbTipoDependencia.Items.FindByText((string)e.Value);

                if (item != null)
                {
                    item.Selected = true;
                }
            }

            if (e.Column.FieldName == "pavimento")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += cmbPavimentacao_OnCallback;
            }

            //if (this.grdDependencias.IsEditing && e.Column.FieldName == "pavimento" && e.KeyValue != DBNull.Value && e.KeyValue != null)
            //{
            //    var val = grdDependencias.GetRowValuesByKeyValue(e.KeyValue, "edificacao");

            //    if (val == DBNull.Value)
            //    {
            //        return;
            //    }

            //    var edificacao = (string)val;
            //    var combo = e.Editor as ASPxComboBox;

            //    if (combo == null)
            //    {
            //        return;
            //    }

            //    CarregarPavimentos(combo, edificacao);
            //}
        }

        protected void grdDemaisDependencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdDemaisDependencias.Settings.ShowFilterRow = false;
        }

        protected void grdDemaisDependencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDemaisDependencias.Settings.ShowFilterRow = false;
        }

        protected void odsDemaisDependencias_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Area = e.InputParameters["area"] == null ? (double?)null : Convert.ToDouble(e.InputParameters["area"]),
                Ativa = Convert.ToString(e.InputParameters["ativa"]),
                SalaAnexa = (e.InputParameters["sala_anexa"] == null || e.InputParameters["sala_anexa"] == string.Empty) ? "N" : Convert.ToString(e.InputParameters["sala_anexa"]),
                Faculdade = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? Convert.ToString(tseUnidade.DBValue) : null,
                Dependencia = e.InputParameters["dependencia"] != null ? Convert.ToString(e.InputParameters["dependencia"]) : null,
                Descricao = e.InputParameters["descricao"] != null ? Convert.ToString(e.InputParameters["descricao"]) : null,
                Obs = e.InputParameters["obs"] != null ? Convert.ToString(e.InputParameters["obs"]) : null,
                Edificacao = e.InputParameters["edificacao"] != null ? Convert.ToString(e.InputParameters["edificacao"]) : null,
                Pavimento = e.InputParameters["pavimento"] != null ? Convert.ToString(e.InputParameters["pavimento"]) : null,
                TipoDepend = e.InputParameters["tipo_depend"] != null ? Convert.ToString(e.InputParameters["tipo_depend"]) : null,
                Matricula = User.Identity.Name,
            };

            var validacao = Dependencia.ValidarInserir(dep);

            if (validacao.Valido)
            {
                Dependencia.InserirSalaRecurso(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsDemaisDependencias_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Area = e.InputParameters["area"] == null ? (double?)null : Convert.ToDouble(e.InputParameters["area"]),
                Ativa = Convert.ToString(e.InputParameters["ativa"]),
                SalaAnexa = (e.InputParameters["sala_anexa"] == null || e.InputParameters["sala_anexa"] == string.Empty) ? "N" : Convert.ToString(e.InputParameters["sala_anexa"]),
                Faculdade = e.InputParameters["faculdade"] != null ? Convert.ToString(e.InputParameters["faculdade"]) : null,
                Dependencia = e.InputParameters["dependencia"] != null ? Convert.ToString(e.InputParameters["dependencia"]) : null,
                Descricao = e.InputParameters["descricao"] != null ? Convert.ToString(e.InputParameters["descricao"]) : null,
                Obs = e.InputParameters["obs"] != null ? Convert.ToString(e.InputParameters["obs"]) : null,
                Edificacao = e.InputParameters["edificacao"] != null ? Convert.ToString(e.InputParameters["edificacao"]) : null,
                Pavimento = e.InputParameters["pavimento"] != null ? Convert.ToString(e.InputParameters["pavimento"]) : null,
                TipoDepend = e.InputParameters["tipo_depend"] != null ? Convert.ToString(e.InputParameters["tipo_depend"]) : null,
                Matricula = User.Identity.Name,
            };

            var validacao = Dependencia.ValidarAlterar(dep);

            if (validacao.Valido)
            {
                Dependencia.Alterar(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsDemaisDependencias_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Faculdade = Convert.ToString(e.InputParameters["faculdade"]),
                Dependencia = Convert.ToString(e.InputParameters["dependencia"]),
                Matricula = User.Identity.Name
            };

            var validacao = Dependencia.ValidarRemover(dep);

            if (validacao.Valido)
            {
                Dependencia.Remover(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

    #endregion


    #region Handlers de Grid — Sala Alternativa

        protected void grdSalaAlternativa_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (grdSalaAlternativa.IsNewRowEditing)
            {
                if (e.Column.FieldName == "NUM_ALUNOS")
                    e.Editor.ReadOnly = true;
                if (e.Column.FieldName == "DEPENDENCIA")
                    e.Editor.ReadOnly = true;
                if (e.Column.FieldName == "ATIVA")
                    e.Editor.Value = "S";
            }
            else
            {
                if (grdSalaAlternativa.IsEditing)
                {

                    if (e.Column.FieldName == "FACULDADE")
                        e.Editor.ReadOnly = false;
                    if (e.Column.FieldName == "DEPENDENCIA")
                        e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdSalaAlternativa_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            DataTable Dependencia = RN.Dependencia.BuscaMaiorDependencia(tseUnidade.DBValue.ToString());

            e.NewValues["NUM_ALUNOS"] = "45";
            e.NewValues["DEPENDENCIA"] = Dependencia.Rows[0][0].ToString();
        }

        protected void grdSalaAlternativa_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string faculdade = Convert.ToString(e.GetListSourceFieldValue("FACULDADE"));
                string dependencia = Convert.ToString(e.GetListSourceFieldValue("DEPENDENCIA"));

                e.Value = faculdade + "/" + dependencia;
            }
        }

        protected void grdSalaAlternativa_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["unidade_fis"] = tseUnidade.DBValue.ToString();
        }

        protected void grdSalaAlternativa_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('/');
            e.Keys.Clear();
            e.Keys.Add("FACULDADE", chaves[0]);
            e.Keys.Add("DEPENDENCIA", chaves[1]);

            e.Keys.Add("TIPO_DEPEND_ANTERIOR", e.OldValues["TIPO_DEPEND"]);
            e.Keys.Add("ATIVA_ANTERIOR", e.OldValues["ATIVA"]);
        }

        protected void grdSalaAlternativa_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("UNIDADE_FIS", chaves[0]);
            e.Keys.Add("EDIFICACAO", chaves[1]);
            e.Keys.Add("PAVIMENTO", chaves[2]);
        }

        protected void odsSalaAlternativas_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {

            DataTable Dependencia = RN.Dependencia.BuscaMaiorDependencia(tseUnidade.DBValue.ToString());
            var SalaAlter = new DadosSalaAlternativa
            {
                FACULDADE = tseUnidade.DBValue.ToString(),
                DEPENDENCIA = Dependencia.Rows[0][0].ToString(),
                DESCRICAO = Convert.ToString(e.InputParameters["DESCRICAO"]),
                TIPO_DEPEND = Convert.ToString(e.InputParameters["TIPO_DEPEND"]),
                NUM_ALUNOS = "45",
                ATIVA = Convert.ToString(e.InputParameters["ATIVA"]),
                SALA_ANEXA = Convert.ToString(e.InputParameters["SALA_ANEXA"]),
                MATRICULA = User.Identity.Name
            };

            var validacao = RN.Dependencia.Validar(SalaAlter);

            if (validacao.Valido)
            {
                RN.Dependencia.InserirSalaAlternativa(SalaAlter);

            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsSalaAlternativas_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var SalaAlter = new DadosSalaAlternativa
            {
                FACULDADE = Convert.ToString(tseUnidade.DBValue),
                DEPENDENCIA = Convert.ToString(e.InputParameters["DEPENDENCIA"]),
                DESCRICAO = Convert.ToString(e.InputParameters["DESCRICAO"]),
                TIPO_DEPEND = Convert.ToString(e.InputParameters["TIPO_DEPEND"]),
                NUM_ALUNOS = Convert.ToString(e.InputParameters["NUM_ALUNOS"]),
                ATIVA = Convert.ToString(e.InputParameters["ATIVA"]),
                SALA_ANEXA = Convert.ToString(e.InputParameters["SALA_ANEXA"]),
                TIPO_DEPEND_ANTERIOR = Convert.ToString(e.InputParameters["TIPO_DEPEND_ANTERIOR"]),
                ATIVA_ANTERIOR = Convert.ToString(e.InputParameters["ATIVA_ANTERIOR"]),
                MATRICULA = User.Identity.Name
            };

            var validacao = Dependencia.Validaralterar(SalaAlter);

            if (validacao.Valido)
            {
                RN.Dependencia.AlterarSalaAlternativa(SalaAlter);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

    #endregion


    #region Handlers de Grid — Compartilhamento

        public void DeleteCompartilhadaOferta(object id_compartilhada_oferta)
        {
        }

        public void DeleteXTotalSalas(object id_compartilhada)
        {
        }

        protected void odsCompartilhadaOferta_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var idCompartilhadaOferta = Convert.ToInt32(e.InputParameters["id_compartilhada_oferta"]);

            CompartilhadaOferta.Remover(idCompartilhadaOferta);
        }

        protected void odsCompartilhadaOferta_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var compartilhadaOferta = new TceCompartilhadaOferta
            {
                Curso = Convert.ToString(e.InputParameters["curso"]),
                Turno = Convert.ToString(e.InputParameters["turno"]),
                Matricula = User.Identity.Name,
                IdCompartilhada = Convert.ToInt32(hdnCensoCompartilhada.Value)
            };

            var validacao = CompartilhadaOferta.Validar(compartilhadaOferta);

            if (validacao.Valido)
            {
                CompartilhadaOferta.Inserir(compartilhadaOferta);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCompartilhada_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            var idCompartilhada = Convert.ToInt32(e.InputParameters["id_compartilhada"]);

            validacao = Compartilhada.ValidarRemover(idCompartilhada);

            if (validacao.Valido)
            {
                Compartilhada.Remover(idCompartilhada);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCompartilhada_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var compartilhada = new TceCompartilhada
            {
                UnidadedeEnsino = Convert.ToString(tseUnidade.Value),
                RedeEnsino = Convert.ToString(e.InputParameters["rede_ensino"]),
                Censo = tseUnidade.DBValue.ToString(),
                CensoCompartilhada = Convert.ToString(e.InputParameters["censo_compartilhada"]),
                Nome = Convert.ToString(e.InputParameters["nome"]),
                Matricula = User.Identity.Name,
                CedidasManha = !string.IsNullOrEmpty(Convert.ToString(e.InputParameters["cedidas_manha"])) ? int.Parse(Convert.ToString(e.InputParameters["cedidas_manha"])) : 0,
                CedidasTarde = !string.IsNullOrEmpty(Convert.ToString(e.InputParameters["cedidas_tarde"])) ? int.Parse(Convert.ToString(e.InputParameters["cedidas_tarde"])) : 0,
                CedidasNoite = !string.IsNullOrEmpty(Convert.ToString(e.InputParameters["cedidas_noite"])) ? int.Parse(Convert.ToString(e.InputParameters["cedidas_noite"])) : 0
            };

            var validacao = Compartilhada.Validar(compartilhada);

            if (validacao.Valido)
            {
                Compartilhada.Inserir(compartilhada);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCompartilhada_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var compartilhada = new TceCompartilhada
            {
                UnidadedeEnsino = Convert.ToString(tseUnidade.Value),
                IdCompartilhada = Convert.ToInt32(e.InputParameters["id_compartilhada"]),
                RedeEnsino = Convert.ToString(e.InputParameters["rede_ensino"]),
                Censo = tseUnidade.DBValue.ToString(),
                CensoCompartilhada = Convert.ToString(e.InputParameters["censo_compartilhada"]),
                Nome = Convert.ToString(e.InputParameters["nome"]),
                Matricula = User.Identity.Name,
                CedidasManha = !string.IsNullOrEmpty(Convert.ToString(e.InputParameters["cedidas_manha"])) ? int.Parse(Convert.ToString(e.InputParameters["cedidas_manha"])) : 0,
                CedidasTarde = !string.IsNullOrEmpty(Convert.ToString(e.InputParameters["cedidas_tarde"])) ? int.Parse(Convert.ToString(e.InputParameters["cedidas_tarde"])) : 0,
                CedidasNoite = !string.IsNullOrEmpty(Convert.ToString(e.InputParameters["cedidas_noite"])) ? int.Parse(Convert.ToString(e.InputParameters["cedidas_noite"])) : 0
            };

            var validacao = Compartilhada.ValidarAlterar(compartilhada);

            if (validacao.Valido)
            {
                Compartilhada.Alterar(compartilhada);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

        }

        public void cmbTempCurso_Callback(object source, CallbackEventArgsBase e)
        {
            var cmbSegmento = this.grdCompartilhadaOferta.FindEditFormTemplateControl("cmbTempSegmento") as ASPxComboBox;
            var cmbModalidade = this.grdCompartilhadaOferta.FindEditFormTemplateControl("cmbTempModalidade") as ASPxComboBox;
            var cmbCurso = source as ASPxComboBox;

            if (cmbSegmento == null
                || cmbModalidade == null
                || cmbCurso == null)
            {
                return;
            }

            cmbCurso.Items.Clear();

            if (cmbSegmento.SelectedIndex > -1
                && cmbModalidade.SelectedIndex > -1)
            {
                var dataTable = Curso.ListarEscolaridade((string)cmbSegmento.SelectedItem.Value, (string)cmbModalidade.SelectedItem.Value);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    cmbCurso.Items.Add(Convert.ToString(dataRow["NOME"]), Convert.ToString(dataRow["CURSO"]));
                }
            }
        }

        protected void grdCompartilhadaOferta_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCompartilhadaOferta);
        }

        protected void grdCompartilhadaOferta_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdCompartilhadaOferta.IsNewRowEditing)
            {
                if (e.Column.FieldName == "curso")
                {
                    var combo = e.Editor as ASPxComboBox;

                    if (combo == null)
                        return;

                    combo.Callback += cmbTempCurso_Callback;
                }
            }
        }

        protected void grdCompartilhada_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCompartilhada);
        }

        protected void grdCompartilhada_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "rede_ensino")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                    return;

                if (grdCompartilhada.IsNewRowEditing)
                {
                    combo.SelectedIndex = 0;
                }
            }
        }

        protected void grdCompartilhada_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var redeEnsino = (string)grdCompartilhada.GetRowValues(e.VisibleIndex, "rede_ensino");

            if (!string.IsNullOrEmpty(redeEnsino) && redeEnsino == "Estadual" && e.ButtonType == ColumnCommandButtonType.Edit)
            {
                e.Visible = false;
            }
        }

        protected void grdCompartilhada_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {
            grdCompartilhada.Settings.ShowFilterRow = false;

            if (e.NewValues["cedidas_manha"] == null)
                e.NewValues["cedidas_manha"] = 0;
            if (e.NewValues["cedidas_tarde"] == null)
                e.NewValues["cedidas_tarde"] = 0;
            if (e.NewValues["cedidas_noite"] == null)
                e.NewValues["cedidas_noite"] = 0;
        }

        protected void grdCompartilhada_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            var redeEnsino = Convert.ToString(e.NewValues["rede_ensino"]);

            if (redeEnsino == "Estadual")
            {
                e.NewValues["nome"] = null;
            }
        }

        protected void grdCompartilhada_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var redeEnsino = Convert.ToString(e.NewValues["rede_ensino"]);

            try
            {
                int cedidas_manha = (int)e.NewValues["cedidas_manha"];
                int cedidas_tarde = (int)e.NewValues["cedidas_tarde"];
                int cedidas_noite = (int)e.NewValues["cedidas_noite"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdCompartilhada_StartRowEditing(object sender, ASPxStartRowEditingEventArgs e)
        {
            grdCompartilhada.Settings.ShowFilterRow = false;
        }

    #endregion


    #region Handlers de Grid — Situação da Unidade (Histórico)

        protected void grdSituacaoUnidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSituacaoUnidade);
        }

        protected void grdSituacaoUnidade_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdSituacaoUnidade.IsNewRowEditing)
            {
                if (e.Column.FieldName == "SITUACAO")
                {
                    e.Editor.Enabled = true;
                }

                if (e.Column.FieldName == "ATO_OFICIAL")
                {
                    e.Editor.Enabled = true;
                }

                if (e.Column.FieldName == "DT_SITUACAO")
                {
                    e.Editor.Enabled = true;
                }
            }
        }

        protected void grdSituacaoUnidade_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var unidade = Convert.ToString(e.GetListSourceFieldValue("UNIDADE_ENS"));
                var ordem = Convert.ToString(e.GetListSourceFieldValue("ORDEM"));

                e.Value = unidade + "|" + ordem;
            }
        }

        protected void grdSituacaoUnidade_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            var unidadeEnsino = Convert.ToString(e.Values["UNIDADE_ENS"]);
            var ordem = Convert.ToInt32(e.Values["ORDEM"]);
            var ordemAtiva = UnidadeEnsinoSituacao.RetornaOrdemAtiva(unidadeEnsino);

            if (Convert.ToString(e.Values["SITUACAO"]) == "MUNICIPALIZADA")
            {
                if (Municipalizacao.VerificarMunicipalizacao(tseUnidade.DBValue.ToString()))
                {
                    throw new Exception("Esta situação não pode ser excluída devido existir vinculos com a Municipalização.");
                }
            }

            UnidadeEnsinoSituacao.Remover(unidadeEnsino, ordem);

            if (ordemAtiva == ordem)
            {
                var atoAtivo = UnidadeEnsinoSituacao.RetornaAtoAtivo(unidadeEnsino);

                if (atoAtivo == UnidadeEnsinoSituacao.Paralizacao)
                {
                    grdSituacaoUnidade.JSProperties["cpAtualizar"] = "Paralisada";
                }
                else if (atoAtivo == UnidadeEnsinoSituacao.Extincao)
                {
                    grdSituacaoUnidade.JSProperties["cpAtualizar"] = "Extinta";
                }
                else if (atoAtivo == UnidadeEnsinoSituacao.EmProcesso)
                {
                    grdSituacaoUnidade.JSProperties["cpAtualizar"] = "EmProcesso";
                }
                else
                {
                    grdSituacaoUnidade.JSProperties["cpAtualizar"] = "EmAtividade";
                }
            }

            e.Cancel = true;
            grdSituacaoUnidade.CancelEdit();
        }

        protected void grdSituacaoUnidade_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var unidadeEnsinoSituacao = UnidadeEnsinoSituacao.Bind(null, e.NewValues);

            unidadeEnsinoSituacao.UnidadeEns = tseUnidade.DBValue.ToString();
            unidadeEnsinoSituacao.Matricula = User.Identity.Name;

            UnidadeEnsinoSituacao.Inserir(unidadeEnsinoSituacao);

            var ordemativa = UnidadeEnsinoSituacao.RetornaOrdemAtiva(unidadeEnsinoSituacao.UnidadeEns);

            if (ordemativa == unidadeEnsinoSituacao.Ordem
                && unidadeEnsinoSituacao.AtoOficial == UnidadeEnsinoSituacao.Paralizacao)
            {
                grdSituacaoUnidade.JSProperties["cpAtualizar"] = "Paralisada";
            }
            else if (ordemativa == unidadeEnsinoSituacao.Ordem
                && unidadeEnsinoSituacao.AtoOficial == UnidadeEnsinoSituacao.Extincao)
            {
                grdSituacaoUnidade.JSProperties["cpAtualizar"] = "Extinta";
            }
            else if (ordemativa == unidadeEnsinoSituacao.Ordem
                && unidadeEnsinoSituacao.AtoOficial == UnidadeEnsinoSituacao.EmProcesso)
            {
                grdSituacaoUnidade.JSProperties["cpAtualizar"] = "EmProcesso";
            }

            e.Cancel = true;
            grdSituacaoUnidade.CancelEdit();
        }

        protected void grdSituacaoUnidade_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var chaves = e.Keys["CompositeKey"].ToString().Split('|');

            e.Keys.Clear();
            e.Keys.Add("UNIDADE_ENS", chaves[0]);
            e.Keys.Add("ORDEM", chaves[1]);

            var unidadeEnsinoSituacao = UnidadeEnsinoSituacao.Bind(e.Keys, e.NewValues);

            unidadeEnsinoSituacao.Matricula = User.Identity.Name;

            UnidadeEnsinoSituacao.Alterar(unidadeEnsinoSituacao);

            e.Cancel = true;
            grdSituacaoUnidade.CancelEdit();
        }

        protected void grdSituacaoUnidade_Validating(object sender, ASPxDataValidationEventArgs e)
        {
            var unidadeEnsinoSituacao = UnidadeEnsinoSituacao.Bind(null, e.NewValues);
            var validacao = UnidadeEnsinoSituacao.Validar(unidadeEnsinoSituacao);

            if (!validacao.Valido)
            {
                e.RowError = validacao.Mensagem;
            }
        }

    #endregion


    #region Handlers de Grid — Documentos Celebrados

        protected void grdDocumentosCelebrados_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocumentosCelebrados);
            ControlaAcesso(grdDocumentosCelebrados, AcaoControle.editar, "btnEditarDocCelebrado");
            ControlaAcesso(grdDocumentosCelebrados, AcaoControle.excluir, "btnExcluirDocCelebrado");
            AcessoGrid();
        }

        protected void AcessoGrid()
        {
            if (grdDocumentosCelebrados != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdDocumentosCelebrados.FindHeaderTemplateControl(grdDocumentosCelebrados.Columns[""], "btnNovoDocCelebrado");

                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;
                }
            }
        }

        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                pnlDadosMunicipalizacao.Visible = true;
                pnlDocCelebrado.Visible = true;
                LimpaCamposDocCelebrado();
                CarregaNumProcessoMunicipalizacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDocumentosCelebrados_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnEditarDocCelebrado")
                {
                    Municipalizacao rnMunicipalizacao = new Municipalizacao();
                    TceMunicipalizacao municipalizacao = new TceMunicipalizacao();
                    CarregaNumProcessoMunicipalizacao();

                    LimpaCamposMunicipalizacao();
                    LimpaCamposDocCelebrado();

                    int id = Convert.ToInt32(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "ID_MUNICIPALIZACAO"));

                    municipalizacao = rnMunicipalizacao.ObtemMunicipalizacaoPor(id);

                    if (municipalizacao != null)
                    {
                        hdnMunicipalizacao.Value = municipalizacao.IdMunicipalizacao.ToString();
                        txtAutorizoProv.Text = string.IsNullOrEmpty(municipalizacao.NumAutorizoProvisorio) ? string.Empty : municipalizacao.NumAutorizoProvisorio;
                        txtProcessoMunic.Text = string.IsNullOrEmpty(municipalizacao.Processo) ? string.Empty : municipalizacao.Processo;
                        dtAutorizoProv.Date = municipalizacao.DtAutorizoProvisorio;
                        dtValidadeAutorizo.Date = municipalizacao.DtValidadeAutorizo;

                        if (municipalizacao.DtPublicacaoDo.HasValue)
                        {
                            dtPublicacaoDO.Date = municipalizacao.DtPublicacaoDo.Value;
                        }

                        txtPaginaDO.Text = municipalizacao.PaginaDo;
                    }


                    hdnIdDocCelebrado.Value = Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "ID_DOC_CELEBRADO_MUNICIPALIZACAO"));

                    if (ddlNumProcessoDocCelebrado.Items.FindByValue(Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "ID_MUNICIPALIZACAO"))) != null)
                    {
                        ddlNumProcessoDocCelebrado.SelectedValue = Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "ID_MUNICIPALIZACAO"));
                    }

                    if (ddlTipoDocCelebrado.Items.FindByValue(Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "TIPO"))) != null)
                    {
                        ddlTipoDocCelebrado.SelectedValue = Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "TIPO"));
                    }
                    txtNumeroDocCelebrado.Text = Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "NUMERO"));
                    dtDataValidadeInicio.Date = Convert.ToDateTime(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "DT_CELEBRACAO"));
                    dtDataValidadeFinal.Date = Convert.ToDateTime(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "DT_VALIDADE"));
                    txtObsDocCelebrado.Text = Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "OBSERVACAO"));

                    pnlDocCelebrado.Visible = true;
                    pnlDadosMunicipalizacao.Visible = true;

                }

                if (e.ButtonID == "btnExcluirDocCelebrado")
                {
                    hdnIdDocCelebrado.Value = Convert.ToString(grdDocumentosCelebrados.GetRowValues(e.VisibleIndex, "ID_DOC_CELEBRADO_MUNICIPALIZACAO"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupDocCelebrado();", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    #endregion


    #region Handlers de Grid — Banheiros e Vestiários

        protected void grdBanheiroeVestiario_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var chaves = e.Keys["CompositeKey"].ToString().Split('-');

            e.Keys.Clear();

            e.Keys.Add("faculdade", chaves[0]);

            string valores = string.Empty;
            string chavefinal = string.Empty;
            for (int i = 1; i < chaves.Length; i++)
            {
                valores = valores + chaves[i] + "-";
            }

            chavefinal = valores.Substring(0, valores.Length - 1);

            e.Keys.Add("dependencia", chavefinal);
        }

        protected void grdBanheiroeVestiario_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();

            e.Keys.Add("faculdade", e.Values["faculdade"]);
            e.Keys.Add("dependencia", e.Values["dependencia"]);
        }

        protected void grdBanheiroeVestiario_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
            {
                e.NewValues["faculdade"] = tseUnidade.DBValue.ToString();
            }

            if (e.NewValues["ativa"] == null)
            {
                e.NewValues["ativa"] = "N";
            }
        }

        protected void grdBanheiroeVestiario_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "obs"
                && e.Value != null)
            {
                if (!string.IsNullOrEmpty(e.Value.ToString()))
                {
                    var obs = e.Value.ToString();

                    e.DisplayText = obs.Length > 100 ? obs.Substring(0, 100) : obs;
                }
            }
        }

        protected void grdBanheiroeVestiario_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var faculdade = Convert.ToString(e.GetListSourceFieldValue("faculdade"));
                var dependencia = Convert.ToString(e.GetListSourceFieldValue("dependencia"));

                e.Value = faculdade + "-" + dependencia;
            }
        }

        protected void grdBanheiroeVestiario_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdBanheiroeVestiario);
        }

        protected void grdBanheiroeVestiario_AutoFilterCellEditorCreate(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "ativa")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Items.Clear();

                var check = e.Column as GridViewDataCheckColumn;

                if (check == null)
                {
                    return;
                }

                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }
        }

        protected void grdBanheiroeVestiario_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdBanheiroeVestiario.Settings.ShowFilterRow = false;
        }

        protected void grdBanheiroeVestiario_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdBanheiroeVestiario.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "dependencia")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.Value = "BAN-";
                }
            }

            else if (grdBanheiroeVestiario.IsEditing)
            {
                if ((e.Column.FieldName) == "dependencia")
                    e.Editor.ReadOnly = true;
            }

            //if (e.Column.FieldName == "tipo_depend")
            //{
            //    var cmbTipoDependencia = e.Editor as ASPxComboBox;

            //    if (cmbTipoDependencia == null)
            //    {
            //        return;
            //    }

            //    cmbTipoDependencia.Items.Clear();
            //    cmbTipoDependencia.DataSource = TipoDependencia.ListarBanheiros();
            //    cmbTipoDependencia.TextField = "NOME";
            //    cmbTipoDependencia.ValueField = "TIPO_DEPEND";
            //    cmbTipoDependencia.DataBind();

            //    var item = cmbTipoDependencia.Items.FindByText((string)e.Value);

            //    if (item != null)
            //    {
            //        item.Selected = true;
            //    }
            //}

            if (e.Column.FieldName == "pavimento")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += cmbPavimentacao_OnCallback;
            }

            if (grdBanheiroeVestiario.IsEditing && e.Column.FieldName == "pavimento" && e.KeyValue != DBNull.Value && e.KeyValue != null)
            {
                var val = grdBanheiroeVestiario.GetRowValuesByKeyValue(e.KeyValue, "edificacao");

                if (val == DBNull.Value)
                {
                    return;
                }

                var edificacao = (string)val;
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                CarregarPavimentos(combo, edificacao);
            }
        }

        protected void grdBanheiroeVestiario_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdBanheiroeVestiario.Settings.ShowFilterRow = false;
        }

        protected void odsBanheiroeVestiario_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {

                Ativa = Convert.ToString(e.InputParameters["ativa"]),
                Faculdade = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? Convert.ToString(tseUnidade.DBValue) : null,
                Dependencia = e.InputParameters["dependencia"] != null ? Convert.ToString(e.InputParameters["dependencia"]) : null,
                Descricao = e.InputParameters["descricao"] != null ? Convert.ToString(e.InputParameters["descricao"]) : null,
                Edificacao = e.InputParameters["edificacao"] != null ? Convert.ToString(e.InputParameters["edificacao"]) : null,
                Pavimento = e.InputParameters["pavimento"] != null ? Convert.ToString(e.InputParameters["pavimento"]) : null,
                TipoDepend = e.InputParameters["tipo_depend"] != null ? Convert.ToString(e.InputParameters["tipo_depend"]) : null,
                Matricula = User.Identity.Name,
            };

            var validacao = Dependencia.ValidarInserirBanheiroeVestiario(dep);

            if (validacao.Valido)
            {
                Dependencia.InserirBanheiroeVestiario(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsBanheiroeVestiario_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Ativa = Convert.ToString(e.InputParameters["ativa"]),
                Faculdade = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? Convert.ToString(tseUnidade.DBValue) : null,
                Dependencia = e.InputParameters["dependencia"] != null ? Convert.ToString(e.InputParameters["dependencia"]) : null,
                Descricao = e.InputParameters["descricao"] != null ? Convert.ToString(e.InputParameters["descricao"]) : null,
                Edificacao = e.InputParameters["edificacao"] != null ? Convert.ToString(e.InputParameters["edificacao"]) : null,
                Pavimento = e.InputParameters["pavimento"] != null ? Convert.ToString(e.InputParameters["pavimento"]) : null,
                TipoDepend = e.InputParameters["tipo_depend"] != null ? Convert.ToString(e.InputParameters["tipo_depend"]) : null,
                Matricula = User.Identity.Name,
            };

            var validacao = Dependencia.ValidarAlterarBanheiroeVestiario(dep);

            if (validacao.Valido)
            {
                Dependencia.AlterarBanheiroeVestiario(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsBanheiroeVestiario_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dep = new LyDependencia
            {
                Faculdade = Convert.ToString(e.InputParameters["faculdade"]),
                Dependencia = Convert.ToString(e.InputParameters["dependencia"]),
                Matricula = User.Identity.Name
            };

            var validacao = Dependencia.ValidarRemoverBanheiroeVestiario(dep);

            if (validacao.Valido)
            {
                Dependencia.Remover(dep);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        public void Insert_banheiro(object dependencia, object descricao, object ativa, object tipo_depend, object edificacao, object pavimento, object faculdade) { }
        public void Update_banheiro(object dependencia, object descricao, object ativa, object tipo_depend, object edificacao, object pavimento, object faculdade) { }

    #endregion


    #region Handlers de Controles (TextChanged, SelectedIndexChanged etc.)

        protected void tseUnidade_Changed(object sender, EventArgs e)
        {
            try
            {
                LimpaDadosConcessionaria();
                LimpaDadosCondicaoSala();
                LimpaDadosInternet();
                LimpaDadosPedagogico();
                pcUnidade.Visible = false;

                if (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull)
                {
                    pcUnidade.Visible = true;
                    pcUnidade.ActiveTabIndex = 0;
                    txtUnidadeEnsino.Text = tseUnidade.DBValue.ToString();
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }
                else
                {
                    if (!tseUnidade.DBValue.IsNull)
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void txtNumeroEnd_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.Params.Get("__EVENTTARGET") == txtEnd_Num.UniqueID &&
                    Page.Request.Params.Get("__EVENTARGUMENT") == "Change")
                {
                    if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        this.CarregaBairro(hdnCodMunicipioFisica.Value);

                        ddlDistrito.Items.Clear();
                        ddlDistrito.DataSource = RN.Distrito.Listar(hdnCodMunicipioFisica.Value);
                        ddlDistrito.DataBind();
                        ddlDistrito.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void txtNumeroEndUF_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    this.CarregaBairroUF(hdnCodMunicipio.Value);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void txtNumeroEndUF_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (Page.Request.Params.Get("__EVENTTARGET") == txtNumeroEndUF.UniqueID &&
                    Page.Request.Params.Get("__EVENTARGUMENT") == "Change")
                {
                    if (!hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        this.CarregaBairroUF(hdnCodMunicipio.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlLocalFuncionamento_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocalFuncionamento.SelectedValue == "PredioEscolar")
            {
                lblTipo.Text = "Forma de Ocupação*:";
                lblTipo.Font.Bold = true;
            }
            else
            {
                lblTipo.Text = "Forma de Ocupação:";
                lblTipo.Font.Bold = false;
            }
        }

        protected void chkNaoSeAplica_CheckedChanged(object sender, EventArgs e)
        {
            ValidaLocalizacaoDiferenciada();
        }

        protected void rblAcessoNecEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!rblAcessoNecEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() && rblAcessoNecEspecial.SelectedValue == "S")
            {
                pnlAcessibilidade.Visible = true;
                chkRecursoAcessibilidade.Enabled = true;
            }
            else
            {
                pnlAcessibilidade.Visible = false;
                chkRecursoAcessibilidade.Enabled = false;
                chkRecursoAcessibilidade.ClearSelection();
            }
        }

        protected void txtEnd_Num_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlDistritoFisica.Items.Clear();
                    ddlDistritoFisica.DataSource = RN.Distrito.Listar(hdnCodMunicipioFisica.Value);
                    ddlDistritoFisica.DataBind();
                    ddlDistritoFisica.Items.Insert(0, new ListItem("Selecione", string.Empty));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkAtivaDepen_CheckedChanged(object sender, EventArgs e)
        {
            odsDependencias.Select();
            grdDependencias.DataBind();
        }

        protected void chkOutrosAbastecimentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int auxSelected = 0;

                foreach (ListItem item in chkOutrosAbastecimentos.Items)
                {
                    if (item.Selected)
                    {
                        auxSelected++;
                        if (item.Text == "Inexistente")
                        {
                            pnlConcessionariaAgua.Enabled = false;
                            tseAgua_Concessionaria.ResetValue();
                            tseAgua_Concessionaria.Mode = ControlMode.View;
                            txtAgua_NumeroCliente.Text = string.Empty;
                            chkAgua_Hidrometro.Checked = false;
                            chkOutrosAbastecimentos.ClearSelection();
                            item.Selected = true;
                            return;
                        }

                        if (item.Text == "Rede Pública/Concessionária")
                        {
                            pnlConcessionariaAgua.Enabled = true;
                            tseAgua_Concessionaria.Mode = ControlMode.Edit;
                        }
                    }
                }
                if (auxSelected == 0)
                {
                    pnlConcessionariaAgua.Enabled = false;
                    tseAgua_Concessionaria.ResetValue();
                    tseAgua_Concessionaria.Mode = ControlMode.View;
                    txtAgua_NumeroCliente.Text = string.Empty;
                    chkAgua_Hidrometro.Checked = false;
                    chkOutrosAbastecimentos.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkTipoAbastecimentoEnergia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int auxSelected = 0;

                pnlnformacoesEnergia.Enabled = false;
                tseEE_Concessionaria.Mode = ControlMode.View;

                foreach (ListItem item in chkTipoAbastecimentoEnergia.Items)
                {
                    if (item.Selected)
                    {
                        auxSelected++;
                        if (item.Text == "Inexistente")
                        {
                            chkEE_Contrato.Checked = false;
                            tseEE_Concessionaria.ResetValue();
                            txtEE_NumeroCliente.Text = string.Empty;
                            ddlEE_ClasseFornecimento.Items.Clear();
                            pnlnformacoesEnergia.Enabled = false;
                            chkTipoAbastecimentoEnergia.ClearSelection();
                            item.Selected = true;
                            return;
                        }

                        if (item.Text == "Rede Pública/Concessionária")
                        {
                            pnlnformacoesEnergia.Enabled = true;
                            tseEE_Concessionaria.Mode = ControlMode.Edit;
                            CarregaClasseFornecimentoEnergia();
                        }
                    }
                }
                if (auxSelected == 0)
                {
                    chkEE_Contrato.Checked = false;
                    tseEE_Concessionaria.ResetValue();
                    txtEE_NumeroCliente.Text = string.Empty;
                    ddlEE_ClasseFornecimento.ClearSelection();
                    chkTipoAbastecimentoEnergia.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkEsgoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in chkEsgoto.Items)
            {
                if (item.Selected && item.Text == "Inexistente")
                {
                    chkEsgoto.ClearSelection();
                    item.Selected = true;
                    return;
                }
            }
        }

        protected void chkTratamentoLixo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in chkTratamentoLixo.Items)
            {
                if (item.Selected && item.Text == "Não faz tratamento")
                {
                    chkTratamentoLixo.ClearSelection();
                    item.Selected = true;
                    return;
                }
            }
        }

        private void cmbPavimentacao_OnCallback(object source, CallbackEventArgsBase e)
        {
            CarregarPavimentos(source as ASPxComboBox, e.Parameter);
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

        protected void rblInternetBandaLarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlDadosInternet.Visible = false;

                if (rblInternetBandaLarga.SelectedValue == "S")
                {
                    pnlDadosInternet.Visible = true;

                    //Volta para valores default
                    chkEquipamentoEscola.Checked = true;
                    chkEquipamentoPessoal.Checked = false;

                    chkRedeCabo.Checked = true;
                    chkRedeWireless.Checked = false;
                    chkSemRedeComputador.Checked = false;
                }
                else
                {
                    chkAcessoInternet.ClearSelection();

                    chkEquipamentoEscola.Checked = false;
                    chkEquipamentoPessoal.Checked = false;

                    chkRedeCabo.Checked = false;
                    chkRedeWireless.Checked = false;
                    chkSemRedeComputador.Checked = true;
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

        protected void rblNOL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblNOL.SelectedValue == "S")
                {

                    rblConteudoComponentes.SelectedValue = "N";
                    rblComponenteCurricular.SelectedValue = "N";
                    rblEixoEstuturante.SelectedValue = "N";
                    rblEmEventos.SelectedValue = "N";
                    rblProjetosTransversais.SelectedValue = "N";
                    rblConteudoComponentes.Enabled = false;
                    rblComponenteCurricular.Enabled = false;
                    rblEixoEstuturante.Enabled = false;
                    rblEmEventos.Enabled = false;
                    rblProjetosTransversais.Enabled = false;
                }
                else
                {
                    rblConteudoComponentes.Enabled = true;
                    rblComponenteCurricular.Enabled = true;
                    rblEixoEstuturante.Enabled = true;
                    rblEmEventos.Enabled = true;
                    rblProjetosTransversais.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblEducacaoambiental_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificaEducacaoAmbiental();
        }

        private void VerificaEducacaoAmbiental()
        {
            try
            {
                if (rblEducacaoambiental.SelectedValue == "N")
                {
                    rblConteudoComponentes.SelectedValue = null;
                    rblComponenteCurricular.SelectedValue = null;
                    rblEixoEstuturante.SelectedValue = null;
                    rblEmEventos.SelectedValue = null;
                    rblProjetosTransversais.SelectedValue = null;
                    rblNOL.SelectedValue = null;
                    rblConteudoComponentes.Enabled = false;
                    rblComponenteCurricular.Enabled = false;
                    rblEixoEstuturante.Enabled = false;
                    rblEmEventos.Enabled = false;
                    rblProjetosTransversais.Enabled = false;
                    rblNOL.Enabled = false;
                }
                else
                {
                    rblConteudoComponentes.Enabled = true;
                    rblComponenteCurricular.Enabled = true;
                    rblEixoEstuturante.Enabled = true;
                    rblEmEventos.Enabled = true;
                    rblProjetosTransversais.Enabled = true;
                    rblNOL.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkRecursoAcessibilidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in chkRecursoAcessibilidade.Items)
                {
                    if (item.Selected && item.Value == "9") //Nenhum dos recursos de acessibilidade listados
                    {
                        chkRecursoAcessibilidade.ClearSelection();
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

    #endregion


    #region Limpar Campos

        private void LimparAbaInformacoesGerais()
        {
            hdnCodMunicipio.Value = string.Empty;
            txtUnidadeEnsino.Text = string.Empty;
            txtNome_Comp.Text = string.Empty;
            tseRegional.ResetValue();
            tseSetor.ResetValue();
            tseNucleo.ResetValue();
            tseCEPUF.ResetValue();
            ddlClassificacao.ClearSelection();
            ddlSitFuncionamento.ClearSelection();
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            txtLogradouroUF.Text = string.Empty;
            chkImovelCompartilhado.Checked = false;
            txtCEPUF.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtEstadoUF.Value = string.Empty;
            ddlBairroUF.ClearSelection();
            ddlBairroUF.Items.Clear();
            txtComplementoUF.Text = string.Empty;
            ddlDistrito.ClearSelection();
        }

        protected void LimpaAbaCaracteristicasFisicas()
        {
            hdnCodMunicipioFisica.Value = string.Empty;
            txtUnidadeFisica.Text = string.Empty;
            txtNomeComp.Text = string.Empty;
            txtCEP.Text = string.Empty;
            tsCEP.ResetValue();
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            ddlBairro.ClearSelection();
            ddlBairro.Items.Clear();
            txtEnd_Num.Text = string.Empty;
            txtEnd_Compl.Text = string.Empty;
            txtAreaTerreno.Text = string.Empty;
            txtAreaTotalTerreno.Text = string.Empty;
            txtAreaConstruida.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCGC.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtFone2.Text = string.Empty;
            txtFax.Text = string.Empty;
            chkAcessoDificil.Checked = false;
            chkRecursoAcessibilidade.ClearSelection();
            chkOcupacaoFormal.Checked = false;
            chkImovelCompartilhadoFisica.Checked = false;
            chkExtraclasse.Checked = false;
            rblAcessoNecEspecial.ClearSelection();
            pnlAcessibilidade.Visible = false;
            ddlTipo.ClearSelection();
            txtMunicipioFisica.Text = string.Empty;
            chkAtivaDepen.Checked = true;
            ddlLocalFuncionamento.ClearSelection();
            rblLocalizacaoUF.ClearSelection();
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkNaoSeAplica.Checked = false;
            ddlDependenciaAdministrativa.ClearSelection();
            ddlDistritoFisica.ClearSelection();
        }

        private void LimpaCamposMunicipalizacao()
        {
            hdnMunicipalizacao.Value = string.Empty;
            txtProcessoMunic.Text = "";
            dtPublicacaoDO.Text = string.Empty;
            txtPaginaDO.Text = string.Empty;
            txtAutorizoProv.Text = string.Empty;
            dtAutorizoProv.Text = string.Empty;
            dtValidadeAutorizo.Text = string.Empty;

        }

        private void LimpaCamposMunicipalizacaoVigente()
        {

            txtUltDocVigente.Text = string.Empty;
            txtUltDtAutorizo.Text = string.Empty;
            txtUltProcMunic.Text = string.Empty;
            txtUltDtVigFinal.Text = string.Empty;

        }

        protected void LimpaCamposDocCelebrado()
        {
            ddlNumProcessoDocCelebrado.ClearSelection();
            ddlTipo.ClearSelection();
            ddlTipo.SelectedIndex = -1;
            txtNumeroDocCelebrado.Text = string.Empty;
            dtDataValidadeInicio.Text = string.Empty;
            dtDataValidadeFinal.Text = string.Empty;
            txtObsDocCelebrado.Text = string.Empty;
            hdnIdDocCelebrado.Value = string.Empty;
        }

        private void LimpaDadosConcessionaria()
        {
            hdnCodigoConcessionaria.Value = string.Empty;
            Util.Utils.LimpaCampos(pnlEnergiaEletrica);
            Util.Utils.LimpaCampos(pnlSuprimentoAgua);
            Util.Utils.LimpaCampos(pnlSuprimentoGas);
            Util.Utils.LimpaCampos(pnlEsgoto);
        }

        private void LimpaDadosCondicaoSala()
        {
            txtQtdSalaAcessibilidade.Text = string.Empty;
            txtQtdSalaClimatizada.Text = string.Empty;
            txtQtdCantinhoLeitura.Text = string.Empty;
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

            rblEducacaoambiental.ClearSelection();
            rblConteudoComponentes.ClearSelection();
            rblComponenteCurricular.ClearSelection();
            rblEixoEstuturante.ClearSelection();
            rblEmEventos.ClearSelection();
            rblProjetosTransversais.ClearSelection();
            rblNOL.ClearSelection();

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

    #endregion


    #region Habilitar / Desabilitar Campos

        private void HabilitaDesabilitaCamposAbaInformacoesGerais(bool habilita)
        {
            txtUnidadeEnsino.Enabled = habilita;
            txtNome_Comp.Enabled = habilita;
            tseRegional.Mode = habilita ? ControlMode.Edit : ControlMode.View;
            tseSetor.Mode = habilita ? ControlMode.Edit : ControlMode.View;
            tseNucleo.Mode = habilita ? ControlMode.Edit : ControlMode.View;
            tseCEPUF.ShowButton = habilita;
            ddlClassificacao.Enabled = habilita;
            ddlSitFuncionamento.Enabled = habilita;
            txtLatitude.Enabled = habilita;
            txtLongitude.Enabled = habilita;
            txtLogradouroUF.Enabled = habilita;
            chkImovelCompartilhado.Enabled = habilita;
            txtCEPUF.Enabled = habilita;
            txtMunicipio.Enabled = habilita;
            txtEstadoUF.Disabled = habilita;
            ddlBairroUF.Enabled = habilita;
            txtComplementoUF.Enabled = habilita;
            ddlDistrito.Enabled = habilita;
        }

        protected void HabilitaDesabilitaCamposAbaCaracteristicasFisicas(bool habilita)
        {
            txtUnidadeFisica.Enabled = false;
            txtNomeComp.Enabled = habilita;
            txtCEP.Enabled = habilita;
            tsCEP.ShowButton = habilita;
            txtEstado.Disabled = !habilita;
            txtEndereco.Enabled = habilita;
            ddlBairro.Enabled = habilita;
            txtEnd_Num.Enabled = habilita;
            txtEnd_Compl.Enabled = habilita;
            txtAreaTerreno.Enabled = habilita;
            txtAreaTotalTerreno.Enabled = habilita;
            txtAreaConstruida.Enabled = habilita;
            txtEmail.Enabled = habilita;
            txtCGC.Enabled = habilita;
            txtFone.Enabled = habilita;
            txtFone2.Enabled = habilita;
            txtFax.Enabled = habilita;
            chkAcessoDificil.Enabled = habilita;
            chkRecursoAcessibilidade.Enabled = habilita;
            chkOcupacaoFormal.Enabled = habilita;
            chkImovelCompartilhadoFisica.Enabled = habilita;
            chkExtraclasse.Enabled = habilita;
            rblAcessoNecEspecial.Enabled = habilita;
            ddlTipo.Enabled = habilita;
            txtMunicipioFisica.Enabled = habilita;
            chkAtivaDepen.Enabled = habilita;
            ddlLocalFuncionamento.Enabled = habilita;
            rblLocalizacaoUF.Enabled = habilita;
            chkAreaAssentamento.Enabled = habilita;
            chkTerraIndigena.Enabled = habilita;
            chkQuilombos.Enabled = habilita;
            chkNaoSeAplica.Enabled = habilita;
            ddlDependenciaAdministrativa.Enabled = habilita;
            ddlDistritoFisica.Enabled = habilita;
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

        private void HabilitaDesabilitaCamposMunicipalizacao(bool habilita)
        {
            txtProcessoMunic.Enabled = habilita;
            dtPublicacaoDO.Enabled = habilita;
            txtPaginaDO.Enabled = habilita;
            txtAutorizoProv.Enabled = habilita;
            dtAutorizoProv.Enabled = habilita;
            dtValidadeAutorizo.Enabled = habilita;
        }

        private void HabilitaDesabilitaCamposAbaPedagogico(bool habilita)
        {
            chkMaterialPedagogico.Enabled = habilita;
            chkOrgaoColegiado.Enabled = habilita;
            rblPossuiSite.Enabled = habilita;
            rblPossuiProjeto.Enabled = habilita;
            rblCumpriuProjetoPedagogico.Enabled = habilita;
            rblEspacoEquipamentoEntorno.Enabled = habilita;
            txtSiteBlog.Enabled = habilita;
            rblCompartilhaEspacoComunidade.Enabled = habilita;
            rblEducacaoambiental.Enabled = habilita;
            rblConteudoComponentes.Enabled = habilita;
            rblComponenteCurricular.Enabled = habilita;
            rblEixoEstuturante.Enabled = habilita;
            rblEmEventos.Enabled = habilita;
            rblProjetosTransversais.Enabled = habilita;
            rblNOL.Enabled = habilita;
        }

        private void HabilitaDesabilitaCamposAbaInternet(bool habilita)
        {
            rblInternetBandaLarga.Enabled = habilita;
            chkAcessoInternet.Enabled = habilita;
            chkEquipamentoEscola.Enabled = habilita;
            chkEquipamentoPessoal.Enabled = habilita;
            chkRedeCabo.Enabled = habilita;
            chkRedeWireless.Enabled = habilita;
            chkSemRedeComputador.Enabled = habilita;

        }

    #endregion


    #region Montagem de Controles Dinâmicos e Helpers Privados

        private void PreencheInfoDependenciaEquipamento()
        {
            lblDataAlteracao.Text = string.Empty;
            lblDataAlteracaoEquip.Text = string.Empty;
            lblInfoValidacao.Text = string.Empty;
            TceValidacaoTipoDependencia TVTP = ValidacaoTipoDependencia.RetornaUltimaValidacao(tseUnidade.DBValue.ToString());

            string dataUltimaAlteracao = TipoDependenciaUnidadeFisica.VerificaDataAlteracao(tseUnidade.DBValue.ToString());

            if (!string.IsNullOrEmpty(dataUltimaAlteracao))
            {
                lblDataAlteracao.Text = "Data da última alteração: " + dataUltimaAlteracao.ToString();
                lblDataAlteracao.Visible = true;
            }
            else
            {
                lblDataAlteracao.Visible = false;
            }

            dataUltimaAlteracao = EquipamentoUnidadeFisica.VerificaDataAlteracao(tseUnidade.DBValue.ToString());

            if (!string.IsNullOrEmpty(dataUltimaAlteracao))
            {
                lblDataAlteracaoEquip.Text = "Data da última alteração: " + dataUltimaAlteracao.ToString();
                lblDataAlteracaoEquip.Visible = true;
            }
            else
            {
                lblDataAlteracaoEquip.Visible = false;
            }

            if (TVTP != null)
            {
                if (TVTP.Status == ValidacaoTipoDependencia.NaoConfirmado)
                {
                    lblInfoValidacao.Text = "Informações de dependência não validada pelo Inspetor Escolar. Favor verificar.";
                    lblInfoValidacao.Visible = true;

                    btnSalvarQtdDependencias.Visible = false;
                }
            }
        }

        private void PreencherDadosConcessionaria(LyUnidadeFisicaConcessionaria dadosConcessionaria)
        {
            hdnCodigoConcessionaria.Value = dadosConcessionaria.IdUnidadeFisicaConcessionaria.ToString();
            txtEE_NumeroCliente.Text = Convert.ToString(dadosConcessionaria.EeCodCliente);
            txtAgua_NumeroCliente.Text = Convert.ToString(dadosConcessionaria.AgCodCliente);
            txtAgPoco_Vazao.Text = Convert.ToString(dadosConcessionaria.AgPcVazao);
            txtAgPoco_Profundidade.Text = Convert.ToString(dadosConcessionaria.AgPcProfundidade);
            txtGas_NumeroCliente.Text = Convert.ToString(dadosConcessionaria.GaCodCliente);

            chkEE_Contrato.Value = Convert.ToString(dadosConcessionaria.EeContratoFornecimento);
            chkAgua_Hidrometro.Value = Convert.ToString(dadosConcessionaria.AgHidrometro);
            cbAgPoco_Bomba.Value = Convert.ToString(dadosConcessionaria.AgPcBombaSubmersa);

            var aux = Convert.ToString(dadosConcessionaria.AgPcArtesiano);
            if (aux == "S")
                chkPoco.Items[0].Selected = true;
            else
                chkPoco.Items[0].Selected = false;

            aux = Convert.ToString(dadosConcessionaria.AgPcSemiArtesiano);
            if (aux == "S")
                chkPoco.Items[1].Selected = true;
            else
                chkPoco.Items[1].Selected = false;

            aux = Convert.ToString(dadosConcessionaria.AgPcCacimba);
            if (aux == "S")
                chkPoco.Items[2].Selected = true;
            else
                chkPoco.Items[2].Selected = false;

            aux = Convert.ToString(dadosConcessionaria.AgPcCarroPipa);
            if (aux == "S")
                chkPoco.Items[3].Selected = true;
            else
                chkPoco.Items[3].Selected = false;

            PreencherDadoCombo(ddlEE_ClasseFornecimento, dadosConcessionaria.EeClasseFornecimento);
            PreencherDadoCombo(ddlGas_Tipo, dadosConcessionaria.GaTipo);

            tseEE_Concessionaria.Value = Convert.ToString(dadosConcessionaria.EeEmpresa);
            tseAgua_Concessionaria.Value = Convert.ToString(dadosConcessionaria.AgEmpresa);
            tseGas_Concessionaria.Value = Convert.ToString(dadosConcessionaria.GaEmpresa);

            if (!string.IsNullOrEmpty(dadosConcessionaria.AgTipoAguaConsumida))
            {
                rblTipoAguaConsumida.SelectedValue = dadosConcessionaria.AgTipoAguaConsumida;
            }

            if (!string.IsNullOrEmpty(dadosConcessionaria.EeTipoAbastecimento))
            {
                string[] tipo_abast = dadosConcessionaria.EeTipoAbastecimento.Split(';');
                foreach (String str in tipo_abast)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        chkTipoAbastecimentoEnergia.Items.FindByValue(str).Selected = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosConcessionaria.AgOutrosAbastecimentos))
            {
                string[] outros_abast = dadosConcessionaria.AgOutrosAbastecimentos.Split(';');
                foreach (String str in outros_abast)
                {
                    if (!string.IsNullOrEmpty(str) && chkOutrosAbastecimentos.Items.FindByValue(str) != null)
                    {
                        chkOutrosAbastecimentos.Items.FindByValue(str).Selected = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosConcessionaria.ElEsgotoSanitario))
            {
                string[] esgoto = dadosConcessionaria.ElEsgotoSanitario.Split(';');
                foreach (String str in esgoto)
                {
                    if (!string.IsNullOrEmpty(str) && chkEsgoto.Items.FindByValue(str) != null)
                    {
                        chkEsgoto.Items.FindByValue(str).Selected = true;
                    }
                }
            }
            if (!string.IsNullOrEmpty(dadosConcessionaria.ElDestinacaoLixo))
            {
                string[] lixo = dadosConcessionaria.ElDestinacaoLixo.Split(';');
                foreach (String str in lixo)
                {
                    if (!string.IsNullOrEmpty(str) && chkLixo.Items.FindByValue(str) != null)
                    {
                        chkLixo.Items.FindByValue(str).Selected = true;
                    }
                }
            }

            ICollection<RN.GestaoRede.Entidades.UnidadeFisicaTratamentoLixo> tratamento = new List<RN.GestaoRede.Entidades.UnidadeFisicaTratamentoLixo>();

            RN.GestaoRede.UnidadeFisicaTratamentoLixo rnUnidadeFisicaTratamentoLixo = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaTratamentoLixo();
            tratamento = rnUnidadeFisicaTratamentoLixo.ObtemPor(dadosConcessionaria.UnidadeFis);

            var listaTratamento = tratamento.Select(x => x.TratamentoLixoId).ToList();

            foreach (var linha in listaTratamento.ToList())
            {
                chkTratamentoLixo.Items.FindByValue(linha.ToString()).Selected = true;
            }
        }

        private void TrataAbasDemaisDependenciasEquipamentos()
        {
            string unidade = tseUnidade.DBValue.ToString();
            var dt = (DataTable)Session["perfis"];

            if (!Page.IsCallback)
            {
                MontarEquipamentos(null);
            }

        }

        private void MontarDependencias(string unidade)
        {
            try
            {
                DataTable dtDependencia = new DataTable();
                Table dTable = new Table();
                dTable.ID = "tblDependencia";
                var dt = (DataTable)Session["perfis"];
                bool habilita = dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIRETOR_UE") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPIE") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("INSPETOR ESCOLAR") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE GERAL") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - DEMAIS DEPENDÊNCIA QUANTIDADES") + "'").Length > 0;

                dtDependencia = RN.TipoDependenciaUnidadeFisica.Listar(unidade);

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
                                txtQtdDependencia.Enabled = habilita;
                                txtQtdDependencia.Width = 20;
                                txtQtdDependencia.ID = "txtQt" + dtDependencia.Rows[i + f]["TIPO_DEPEND"].ToString();
                                txtQtdDependencia.Text = dtDependencia.Rows[i + f]["QUANTIDADE"].ToString();
                                txtQtdDependencia.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                txtQtdDependencia.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                txtQtdDependencia.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                Label lblTipoDependencia = new Label();
                                lblTipoDependencia.Enabled = habilita;
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

        private void MontarSalasAlternativa(string unidade)
        {
            try
            {
                DataTable dtSalaAlternativa = new DataTable();
                Table dTable = new Table();
                dTable.ID = "tblSalaAlternativa";
                int Coluna = 0;
                var dt = (DataTable)Session["perfis"];
                dtSalaAlternativa = RN.Dependencia.ListarQuantidadeTipoSala(unidade);

                bool habilita = dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIRETOR_UE") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPIE") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("INSPETOR ESCOLAR") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE GERAL") + "'").Length > 0
    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - DEMAIS DEPENDÊNCIA QUANTIDADES") + "'").Length > 0;

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
                                    txtSalaAlternativa.Enabled = habilita;
                                    txtSalaAlternativa.Width = 20;
                                    txtSalaAlternativa.ID = "txt" + dtSalaAlternativa.Rows[i + f]["Tipo_Depend"].ToString();
                                    txtSalaAlternativa.Text = dtSalaAlternativa.Rows[i + f]["Quantidade"].ToString();
                                    txtSalaAlternativa.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                    txtSalaAlternativa.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                    txtSalaAlternativa.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                    Label lblSalaAlternativa = new Label();
                                    lblSalaAlternativa.Enabled = habilita;
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

        protected void MontarEquipamentos(List<DadosEquipamentoUnidadeFisica> listaEquipamentos)
        {
            RN.EquipamentoUnidadeFisica rnEquipamentoUnidadeFisica = new EquipamentoUnidadeFisica();

            try
            {
                MasterPage ctl00 = FindControl("ctl00") as MasterPage;
                ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;
                ASPxPageControl control = MainContent.FindControl("pcUnidade") as ASPxPageControl;
                Table dTable = control.FindControl("tblEquipamentos") as Table;
                var dt = (DataTable)Session["perfis"];
                bool habilita = dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIRETOR_UE") + "'").Length > 0
                    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPIE") + "'").Length > 0
                    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length > 0
                    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("INSPETOR ESCOLAR") + "'").Length > 0
                    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE GERAL") + "'").Length > 0
                    || dt.Select("perfil ='" + RN.RNBase.MudarAspas("UNIDADE - EQUIPAMENTOS NA UNIDADE") + "'").Length > 0;

                //Verifica se equipamentes já estão montados na tela
                if (dTable != null)
                {
                    return;
                }

                if ((listaEquipamentos == null || listaEquipamentos.Count == 0) && !tseUnidade.DBValue.IsNull)
                {
                    listaEquipamentos = rnEquipamentoUnidadeFisica.ObtemListaPor(tseUnidade.DBValue.ToString());
                }

                dTable = new Table();
                dTable.ID = "tblEquipamentos";

                if (listaEquipamentos != null)
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
                                    txtQtdEquipamento.Enabled = habilita;
                                    txtQtdEquipamento.ID = "txtQtd" + Convert.ToString(item.IdEquipamento);
                                    txtQtdEquipamento.Text = Convert.ToString(item.Quantidade);
                                    txtQtdEquipamento.SkinID = "numerico";
                                    txtQtdEquipamento.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
                                    txtQtdEquipamento.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
                                    txtQtdEquipamento.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

                                    Label lblEquipamento = new Label();
                                    lblEquipamento.Enabled = habilita;
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

        protected List<DadosEquipamentoUnidadeFisica> RetornaListaEquipamentos()
        {
            string unidadeFisica = tseUnidade.DBValue.ToString();
            RN.EquipamentoUnidadeFisica rnEquipamentoUnidadeFisica = new EquipamentoUnidadeFisica();
            List<DadosEquipamentoUnidadeFisica> listaInicial = rnEquipamentoUnidadeFisica.ObtemListaPor(unidadeFisica);
            List<DadosEquipamentoUnidadeFisica> listaEquipamentos = new List<DadosEquipamentoUnidadeFisica>();

            try
            {
                foreach (var item in listaInicial)
                {
                    DadosEquipamentoUnidadeFisica equipamento = new DadosEquipamentoUnidadeFisica();

                    //Busca Quantidade Escolhida pelo usuario
                    var qtde = Request.Params["ctl00$cphFormulario$pcUnidade$txtQtd" + Convert.ToString(item.IdEquipamento)];
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
            string unidadeFisica = tseUnidade.DBValue.ToString();
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

                var dataEquip = EquipamentoUnidadeFisica.VerificaDataAlteracao(tseUnidade.DBValue.ToString());

                if (!string.IsNullOrEmpty(dataEquip))
                {
                    lblDataAlteracaoEquip.Text = "Data da última alteração: " + dataEquip.ToString();
                    lblDataAlteracaoEquip.Visible = true;
                }

                //ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message.Replace(Environment.NewLine, "<br />");
            }
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

                rblPossuiSite_SelectedIndexChanged(null, null);
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

            if (!unidadeDadosPedagogicos.Educacaoambiental.IsNullOrEmptyOrWhiteSpace())
            {
                rblEducacaoambiental.SelectedValue = unidadeDadosPedagogicos.Educacaoambiental;
            }
            if (!unidadeDadosPedagogicos.ConteudoComponentes.IsNullOrEmptyOrWhiteSpace())
            {
                rblConteudoComponentes.SelectedValue = unidadeDadosPedagogicos.ConteudoComponentes;
            }
            if (!unidadeDadosPedagogicos.Componentecurricular.IsNullOrEmptyOrWhiteSpace())
            {
                rblComponenteCurricular.SelectedValue = unidadeDadosPedagogicos.Componentecurricular;
            }
            if (!unidadeDadosPedagogicos.EixoEstuturante.IsNullOrEmptyOrWhiteSpace())
            {
                rblEixoEstuturante.SelectedValue = unidadeDadosPedagogicos.EixoEstuturante;
            }
            if (!unidadeDadosPedagogicos.EmEventos.IsNullOrEmptyOrWhiteSpace())
            {
                rblEmEventos.SelectedValue = unidadeDadosPedagogicos.EmEventos;
            }
            if (!unidadeDadosPedagogicos.ProjetosTransversais.IsNullOrEmptyOrWhiteSpace())
            {
                rblProjetosTransversais.SelectedValue = unidadeDadosPedagogicos.ProjetosTransversais;
            }
            if (!unidadeDadosPedagogicos.NOL.IsNullOrEmptyOrWhiteSpace())
            {
                rblNOL.SelectedValue = unidadeDadosPedagogicos.NOL;
            }
            txtSiteBlog.Text = !unidadeDadosPedagogicos.PaginaWeb.IsNullOrEmptyOrWhiteSpace() ? unidadeDadosPedagogicos.PaginaWeb : string.Empty;
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

    #endregion


    #region IPostBackEventHandler

        public void RaisePostBackEvent(string eventArgument)
        {
            try
            {
                if (!hdnCodMunicipioFisica.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    this.CarregaBairro(hdnCodMunicipioFisica.Value);

                    ddlDistrito.Items.Clear();
                    ddlDistrito.DataSource = RN.Distrito.Listar(hdnCodMunicipioFisica.Value);
                    ddlDistrito.DataBind();
                    ddlDistrito.Items.Insert(0, new ListItem("Selecione", string.Empty));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    #endregion


    #region Outros

        protected void grdDiretor_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "fone" && e.Value != null)
            {
                decimal fone;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }

            if (e.Column.FieldName == "celular" && e.Value != null)
            {
                decimal celular;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out celular))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", celular);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        protected void grdMediador_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "fone" && e.Value != null)
            {
                decimal fone;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }

            if (e.Column.FieldName == "celular" && e.Value != null)
            {
                decimal celular;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out celular))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", celular);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        protected void grdAAGE_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "fone" && e.Value != null)
            {
                decimal fone;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out fone))
                {
                    e.DisplayText = string.Format("{0:(00)0000-0000}", fone);
                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }

            if (e.Column.FieldName == "celular" && e.Value != null)
            {
                decimal celular;

                if (decimal.TryParse(e.Value.ToString().Replace(" ", string.Empty), out celular))
                {
                    if (e.Value.ToString().Replace(" ", string.Empty).Length == 10)
                    {
                        e.DisplayText = string.Format("{0:(00)0000-0000}", celular);
                    }
                    if (e.Value.ToString().Replace(" ", string.Empty).Length == 11)
                    {
                        e.DisplayText = string.Format("{0:(00)00000-0000}", celular);
                    }

                }
                else
                {
                    e.DisplayText = string.Empty;
                }
            }
        }


        public void InsertModalidade(string modalidade, string nivel, string codigo, string nome, DateTime dt_implantacao, DateTime dt_do, string ato, string OBSERVACOES, string PROCESSO) { }


        public void UpdateModalidade(string modalidade, string nivel, string codigo, string nome, DateTime dt_implantacao, DateTime dt_do, string ato, string OBSERVACOES, string PROCESSO, string unidade_ens, decimal curso) { }


        public void DeleteModalidade(object unidade_ens, object curso) { }

        public void Update(object DESCRICAO, object ATIVA, object SALA_ANEXA, object TIPO_DEPEND, object AREA, object OBS, object EDIFICACAO, object PAVIMENTO, object FACULDADE, object DEPENDENCIA)
        {
        }

        public void Insert(object dependencia, object descricao, object ativa, object sala_anexa, object tipo_depend, object area, object edificacao, object pavimento, object obs, object faculdade)
        {
        }


        public void InsertCompartilhadaOferta(string curso, string turno) { }


        public void Insert(string rede_ensino, string censo_compartilhada, string nome, int cedidas_manha, int cedidas_tarde, int cedidas_noite) { }


        public void Insert(string rede_ensino, string censo, string censo_compartilhada, string nome) { }


        public void Update(object nome, object cedidas_manha, object cedidas_tarde, object cedidas_noite, object id_compartilhada) { }


        public void Update(object rede_ensino, object censo_compartilhada, object nome, object cedidas_manha, object cedidas_tarde, object cedidas_noite, object id_compartilhada) { }


        public static void InsertSalaAlternativa(object DEPENDENCIA, object DESCRICAO, object TIPO_DEPEND, object NUM_ALUNOS, object ATIVA, object SALA_ANEXA) { }


        public static void UpdateSalaAlternativa(object DEPENDENCIA, object DESCRICAO, object TIPO_DEPEND, object NUM_ALUNOS, object ATIVA, object SALA_ANEXA, object FACULDADE, object TIPO_DEPEND_ANTERIOR, object ATIVA_ANTERIOR) { }


        protected void btnCancelarMunicipalizacao_Click(object sender, EventArgs e)
        {
            LimpaCamposMunicipalizacao();
            LimpaCamposDocCelebrado();
            pnlDadosMunicipalizacao.Visible = false;
            pnlDocCelebrado.Visible = false;
        }

        private void RemoverSituacaoFuncionamentoRestrita()
        {
            var itensParaRemover = ddlSitFuncionamento.Items
                .Cast<ListItem>()
                .Where(i => !i.Text.Trim().Equals("Em Processo", StringComparison.OrdinalIgnoreCase)
                         && !i.Value.IsNullOrEmptyOrWhiteSpace()) // mantém o "Selecione"
                .ToList();
            foreach (var item in itensParaRemover)
                ddlSitFuncionamento.Items.Remove(item);
        }

    #endregion

}
}