using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Techne.Lyceum.RN.Certificacao.Entidades;
using Techne.Lyceum.RN.Certificacao.DTOs;
using Image = System.Drawing.Image;
using Techne.Lyceum.RN.Util;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Entidades;

using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;
using Techne.Controls;
using Techne.Web;
using Techne.Lyceum.RN;


namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/HistoricoEscolar.aspx"),
     ControlText("Histórico Escolar"),
     Title("Histórico Escolar"),]

    public partial class HistoricoEscolar : TPage
    {
        private string flagSituacaoDisciplina;
        private string tipoConclusaoID;
        private string pessoaID;
        private string obs;
        private RN.Certificacao.Entidades.DocumentoCertificacao docCert;
        private RN.Certificacao.Entidades.DocumentoGerado docGerado;
        private List<HistoricoEscolarDTO> disciplinas = new List<HistoricoEscolarDTO>();
        private TceSituacaoFinalAluno situacaoFinalAluno = new TceSituacaoFinalAluno();
        private SituacaoFinalAluno rnSituacaoFinalAluno = new SituacaoFinalAluno();

        private List<HistoricoEscolarDTO> anoSemestre = new List<HistoricoEscolarDTO>();
        private List<HistoricoEscolarDTO> anosHistorico = new List<HistoricoEscolarDTO>();
        private List<HistoricoEscolarDTO> vidaPregressa = new List<HistoricoEscolarDTO>();
        private List<HistoricoEscolarDTO> rnDisciplinaProgressiva = new List<HistoricoEscolarDTO>();
        private RN.Certificacao.HistoricoEscolar rnHistorico = new Techne.Lyceum.RN.Certificacao.HistoricoEscolar();
        private List<HistoricoEscolarDTO> listarTodasAsDisciplinas = new List<HistoricoEscolarDTO>();



        //usado para armazenar as dependências do aluno
        private List<HistoricoEscolarDTO> listaDisciplinasProgressiva = new List<HistoricoEscolarDTO>();

        #region AcaoControle
        public enum AcaoControle
        {
            excluir,
            novo,
            editar
        }
        #endregion AcaoControle


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

        /// <summary>
        /// Método que recebe os 3 controles para restringir a visibilidade
        /// </summary>
        /// <param name="excluir">Controle de excluir</param>
        /// <param name="novo">Controle de novo</param>
        /// <param name="editar">Controle de editar</param>
        public void ControlaAcesso(Control control, AcaoControle ac)
        {
            if (ac == AcaoControle.excluir)
            {
                if (control.Visible)
                    control.Visible = Permission.AllowDelete;
            }
            else if (ac == AcaoControle.novo)
            {
                if (control.Visible)
                    control.Visible = Permission.AllowInsert;
            }
            else if (ac == AcaoControle.editar)
            {
                if (control.Visible)
                    control.Visible = Permission.AllowUpdate;
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnGerarHistorico.Visible = false;
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
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        btnGerarHistorico.Visible = false;
                        break;

                    }

                case TipoOperacao.Consultar:
                    {
                        Button[] controles = new Button[] { btnGerarHistorico };
                        ImageButton[] imgControles = new ImageButton[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        ControlaAcesso(btnGerarHistorico, AcaoControle.novo);
                        break;
                    }
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    this.listarTipos();
                    txtObservacao.Enabled = true;
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnGerarHistorico, AcaoControle.novo);
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            tseAluno.ResetValue();
            ddlTipoConclusao.ClearSelection();
            pnDadosAluno.Visible = false;
            lblMensagem.Text = string.Empty;
            btnGerarHistorico.Visible = false;
            btnCancelar.Visible = false;
        }



        /// <summary>
        /// Lista os tipos de conclusão Médio, Fundamental e profissionalizante
        /// </summary>
        private void listarTipos()
        {
            ddlTipoConclusao.DataSource = RN.Certificacao.HistoricoEscolar.listarTipoConclusao();
            ddlTipoConclusao.DataBind();
            ddlTipoConclusao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", "-1"));
        }



        /// <summary>
        ///     Lista os alunos informando a sua matrícula ou o nome
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tseAluno_Changed(object sender, EventArgs e)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (this.tseAluno.IsValidDBValue)
                    {
                        return;
                    }
                    else
                    {
                        this.lblMensagem.Text = "Aluno(a) não cadastrado(a), informe a matrícula ou o nome do aluno(a).";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Aluno(a) não cadastrado(a), informe a matrícula ou o nome do aluno(a).";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        /// <summary>
        /// Selecionar o tipo de conclusão do aluno selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoConclusao_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ano foi escolhido, logo limpa demais campos
            DropDownList[] dropdown = new DropDownList[] { ddlTipoConclusao };
            pnDadosAluno.Visible = false;
            btnGerarHistorico.Visible = false;
            btnCancelar.Visible = false;
        }



        /// <summary>
        ///     Busca as informações cadastrais do aluno selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            tipoConclusaoID = ddlTipoConclusao.SelectedValue;
            pessoaID = Convert.ToString(tseAluno["PESSOA"]);
            pnDadosAluno.Visible = false;
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Certificacao.HistoricoEscolar rnHistorico = new RN.Certificacao.HistoricoEscolar();
                validacao = rnHistorico.ValidaHistorico(pessoaID, Convert.ToInt32(tipoConclusaoID), null, "null");

                if (validacao.Valido)
                {
                    pnDadosAluno.Visible = true;
                    btnGerarHistorico.Visible = true;
                    btnCancelar.Visible = true;
                    txtObservacao.Enabled = true;
                    //carregar dados do aluno
                    this.carregarDadosAlunoPor(pessoaID, Convert.ToInt32(tipoConclusaoID));
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// Carrega foto do aluno para os dados cadastrais
        /// </summary>
        /// <param name="dadosFotoPessoa"></param>
        private void CarregaFoto(LyFotoPessoa dadosFotoPessoa)
        {
            if (dadosFotoPessoa == null)
            {
                bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                bimgFotoPessoa.EmptyImage.AlternateText = "Sem foto";
                bimgFotoPessoa.ContentBytes = null;
            }
            else
            {
                try
                {
                    // Tenta carregar array de bytes em objeto Image. 
                    // Em caso de exceção, a foto está em formato inválido
                    Image.FromStream(new MemoryStream(dadosFotoPessoa.Foto));
                    bimgFotoPessoa.ContentBytes = dadosFotoPessoa.Foto;
                }
                catch
                {
                    bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                    bimgFotoPessoa.EmptyImage.AlternateText = "Foto inválida";
                    bimgFotoPessoa.ContentBytes = null;
                }
            }
        }



        /// <summary>
        /// carrega dados cadastrais do aluno
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="tipoConclusaoID"></param>
        private void carregarDadosAlunoPor(string pessoaID, int tipoConclusaoID)
        {
            RN.Aluno rnAluno = new RN.Aluno();
            RN.DTOs.DadosFichaAluno dadosAluno = new Techne.Lyceum.RN.DTOs.DadosFichaAluno();

            RN.Certificacao.HistoricoEscolar rnHistorico = new Techne.Lyceum.RN.Certificacao.HistoricoEscolar();
            docCert = new Techne.Lyceum.RN.Certificacao.Entidades.DocumentoCertificacao();

            try
            {
                //Busca dados do aluno passando o parâmetro matrícula
                dadosAluno = rnAluno.ObtemFichaAlunoPorPessoa(pessoaID);

                //Retorna a observação do historico se existir
                docCert = rnHistorico.retornaObservacaoHistorico(dadosAluno.AlunoMatricula, pessoaID, tipoConclusaoID);

                if (!string.IsNullOrEmpty(dadosAluno.AlunoMatricula))
                {
                    pnDadosAluno.Visible = true;

                    //Dados pessoais
                    lblNome.Text = dadosAluno.NomeAluno.Trim();
                    lblDataNascimento.Text = dadosAluno.DataNascimento.ToString("dd/MM/yyyy");
                    lblSexo.Text = dadosAluno.Sexo; // == "M" ? "Masculino" : "Feminino");
                    lblQtdeFilhos.Text = (!string.IsNullOrEmpty(dadosAluno.QuantidadeFilhos.ToString()) ? dadosAluno.QuantidadeFilhos.ToString() : "não informado");
                    lblTipoSanguineo.Text = (!string.IsNullOrEmpty(dadosAluno.TipoSanguineo.ToString()) ? dadosAluno.TipoSanguineo : "Não declarado");
                    lblEtnia.Text = (!string.IsNullOrEmpty(dadosAluno.Etnia.ToString()) ? dadosAluno.Etnia : "Não declarado");
                    lblEstadoCivil.Text = (!string.IsNullOrEmpty(dadosAluno.EstadoCivil.ToString()) ? dadosAluno.EstadoCivil : "Não declarado");
                    lblNacionalidade.Text = (!string.IsNullOrEmpty(dadosAluno.Nacionalidade.ToString()) ? dadosAluno.Nacionalidade : "Não declarado");

                    if (!string.IsNullOrEmpty(dadosAluno.Naturalidade) && dadosAluno.Naturalidade.Trim() != "/")
                    {
                        // Brasileiro com naturalidade preenchida normalmente
                        lblNaturalidade.Text = dadosAluno.Naturalidade;
                        lblUFNasc.Text = dadosAluno.UfNascimento;
                        lblPaisNasc.Text = (!string.IsNullOrEmpty(dadosAluno.PaisNascimento) ? dadosAluno.PaisNascimento : "Não declarado");
                    }
                    else
                    {
                        try
                        {
                            string matriculaParaNat = dadosAluno.AlunoMatricula;
                            DataTable dtNatEst = rnAluno.ObtemNaturalidadeEstrangeiraPor(matriculaParaNat);
                            if (dtNatEst != null && dtNatEst.Rows.Count > 0)
                            {
                                string munExt = dtNatEst.Rows[0]["NOME_MUNICIPIO"].ToString();
                                string ufExt = dtNatEst.Rows[0]["NOME_ESTADO"].ToString();
                                string paisExt = dtNatEst.Rows[0]["NOME_PAIS"].ToString();

                                // Sempre mostra país de nascimento
                                lblPaisNasc.Text = RN.Util.Utils.Capitaliza(paisExt);

                                // Brasileiro nascido fora do Brasil: Município - Nome do Estado - País
                                if (!string.IsNullOrEmpty(dadosAluno.Nacionalidade))
                                {
                                    lblNaturalidade.Text = RN.Util.Utils.Capitaliza(munExt + "/" + ufExt + " - " + paisExt);
                                    lblUFNasc.Text = ufExt;
                                }
                            }
                            else
                            {
                                lblNaturalidade.Text = "Não informado";
                                lblUFNasc.Text = string.Empty;
                                lblPaisNasc.Text = "Não declarado";
                            }
                        }
                        catch
                        {
                            lblNaturalidade.Text = "Não informado";
                            lblUFNasc.Text = string.Empty;
                            lblPaisNasc.Text = "Não declarado";
                        }
                    }

                    lblCredo.Text = dadosAluno.Credo;
                    lblNecEspecial.Text = dadosAluno.NecessidadeEspecial;
                    lblMatricula.Text = dadosAluno.AlunoMatricula;
                    lblDataSit.Text = dadosAluno.DataSituacao.ToString();
                    lblNacionalidade.Text = dadosAluno.Nacionalidade.ToString();
                    this.CarregaFoto(dadosAluno.Foto);

                    //Dados Filiação
                    lblNomeMae.Text = dadosAluno.NomeMae;
                    lblCPFMae.Text = dadosAluno.CPFMae.AplicarMascaraCPF();

                    lblNomePai.Text = dadosAluno.NomePai;
                    lblCPFPai.Text = dadosAluno.CPFPai.AplicarMascaraCPF();

                    lblRespLegal.Text = dadosAluno.ResponsavelLegal;
                    lblNomeOutros.Text = dadosAluno.NomeOutros;
                    lblCPFOutros.Text = dadosAluno.CpfOutros.AplicarMascaraCPF();

                    lblTelMae.Text = dadosAluno.TelMae.AplicarMascaraTelefoneComDDD();
                    lblTelPai.Text = dadosAluno.TelPai.AplicarMascaraTelefoneComDDD();
                    lblTelResp.Text = dadosAluno.TelResponsavel.AplicarMascaraTelefoneComDDD();

                    //Endereço
                    lblEndereco.Text = dadosAluno.Endereco;
                    lblNumero.Text = dadosAluno.NumeroEndereco;
                    lblComplemento.Text = dadosAluno.ComplementoEndereco;
                    lblBairro.Text = dadosAluno.BairroEndereco;
                    lblMunicipio.Text = dadosAluno.MunicipioCartorio;
                    lblEstado.Text = dadosAluno.EstadoEndereco;
                    lblCEP.Text = dadosAluno.CepEndereco;
                    lblLocalizacao.Text = dadosAluno.LocalizacaoEndereco;

                    //Contatos
                    lblContatoTelefone.Text = dadosAluno.Telefone.AplicarMascaraTelefoneComDDD();
                    lblContatoCelular.Text = dadosAluno.Celular.AplicarMascaraCelularComDDD();
                    lblContatoEmail.Text = dadosAluno.Email;


                    obs = string.Empty;
                    if (!docCert.Observacao.IsNullOrEmptyOrWhiteSpace()) // Exibe a observação
                    {
                        obs = docCert.Observacao.ToString().ToUpper();
                        txtObservacao.Text = obs;
                    }
                    else if (docCert.Observacao.IsNullOrEmptyOrWhiteSpace()) // Informe uma nova observação
                    {
                        txtObservacao.Enabled = true;
                    }

                    btnGerarHistorico.Visible = true;
                    btnCancelar.Visible = true;
                }
                else
                {
                    //limpar a tela
                    pnDadosAluno.Visible = false;
                    btnGerarHistorico.Visible = false;
                    tseAluno.ResetValue();
                    ddlTipoConclusao.ClearSelection();
                    lblMensagem.Text = "Aluno não encontrado.";
                    return;
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }



        /// <summary>
        ///  Monta somente o corpo do ensino fundamental
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="heDto"></param>
        /// <param name="tipo"></param>
        /// <param name="obsHistorico"></param>
        public void corpoHistoricoFundamental(iTextSharp.text.Document doc, RN.Certificacao.DTOs.HistoricoEscolarDTO heDto, int tipo, string obsHistorico)
        {
            var helvetica_boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 7);
            var heveltica_Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7);
            var times_Assinatura_Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8);

            //ler pessoaID
            anoSemestre = rnHistorico.listarAnoSemestrePor(heDto.Pessoa, tipo);

            listarTodasAsDisciplinas = rnHistorico.listarTodasAsDisciplinasPor(heDto.Pessoa, tipo);

            disciplinas = rnHistorico.listarDisciplinasHistoricoEscolarPor(heDto.Pessoa, tipo);

            PdfPTable tblfundamental = new PdfPTable(disciplinas.Count() + 4);
            tblfundamental.TotalWidth = 560f;
            tblfundamental.LockedWidth = true;

            PdfPCell cell = new PdfPCell();
            cell = new PdfPCell(new Phrase("SÉRIE/ANO DE ESCOLARIDADE", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { Rotation = -90, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);

            //repetição que lê as disciplinas
            #region disciplinas
            foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO historico in disciplinas)
            {
                RN.Certificacao.DTOs.HistoricoEscolarDTO he = new HistoricoEscolarDTO();
                //disciplinas e o seu grupo 
                he.Disciplina = historico.grupoDisciplinas;
                he.Agrupamento = historico.Agrupamento;

                cell = new PdfPCell(new Phrase(he.Disciplina.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { Rotation = -90, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);
            }
            #endregion disciplinas

            cell = new PdfPCell(new Phrase("CARGA HORÁRIA TOTAL", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { Rotation = -90, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);
            cell = new PdfPCell(new Phrase("% FREQUENCIA ANUAL", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { Rotation = -90, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);
            cell = new PdfPCell(new Phrase("SITUAÇÃO FINAL", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { Rotation = -90, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);

            string nota_geral = string.Empty;
            int carga_horaria = 0;
            string situacao_hist = string.Empty;
            string anoSitucaoAluno = string.Empty;

            HistoricoEscolarDTO situacao_Final_Freq_total = new HistoricoEscolarDTO();
            HistoricoEscolarDTO anoPeriodo = new HistoricoEscolarDTO();

            #region tabela listarDisciplinas
            var novalista = listarTodasAsDisciplinas.Select(x => x.Serie).Distinct();

            anosHistorico = rnHistorico.RetornaAnosDeHistoricoPor(heDto.Pessoa);


            foreach (string serie in novalista)
            {
                cell = new PdfPCell(new Phrase(serie.ToString() + "º Ano", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);


                foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO hist in disciplinas)
                {
                    // mpetod que retorna ano e periodo que aquela pessoa fez naquela serie agrup. 
                    anoPeriodo = anoSemestre.Where(x => x.serie == serie).FirstOrDefault();

                    //Verifica se é agrupamento de OPTATIVAS
                    if (hist.Agrupamento == "LEOPT" || hist.Agrupamento == "REOPT")
                    {
                        //Verifica se a matriz curricular da turma principal oferece
                        bool oferece = rnHistorico.OfereceOptativaHabilitadaPor(heDto.Pessoa, anoPeriodo.Ano, anoPeriodo.Semestre, serie, tipo, hist.Agrupamento);

                        if (oferece)
                        {
                            HistoricoEscolarDTO NotaGeralCargahoraria = new HistoricoEscolarDTO();
                            NotaGeralCargahoraria = rnHistorico.obtemNotaPor(heDto.Pessoa, anoPeriodo.Ano, anoPeriodo.Semestre, serie, hist.Agrupamento, tipo);

                            //Caso a turma optativa seja ofertada, mesmo que o aluno não cumpra considerar carga horaria 40 caso não possua carga horaria
                            carga_horaria += Convert.ToInt32(NotaGeralCargahoraria.CargaHoraria != null ? NotaGeralCargahoraria.CargaHoraria : 40);

                            nota_geral = ((NotaGeralCargahoraria.Nota_geral == null || NotaGeralCargahoraria.Nota_geral.ToString() == "0.00" || NotaGeralCargahoraria.Nota_geral.ToString() == "0,00") ? "--" : NotaGeralCargahoraria.Nota_geral.Value.ToString("N2"));
                            anoSitucaoAluno = anoPeriodo.ano;

                            cell = new PdfPCell(new Phrase(nota_geral.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                        }
                        else
                        {
                            nota_geral = ("--");
                            cell = new PdfPCell(new Phrase(nota_geral.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                        }
                    }
                    else
                    {
                        HistoricoEscolarDTO NotaGeralCargahoraria = new HistoricoEscolarDTO();
                        NotaGeralCargahoraria = rnHistorico.obtemNotaPor(heDto.Pessoa, anoPeriodo.Ano, anoPeriodo.Semestre, serie, hist.Agrupamento, tipo);

                        carga_horaria += Convert.ToInt32(NotaGeralCargahoraria.CargaHoraria != null ? NotaGeralCargahoraria.CargaHoraria : 0);
                        nota_geral = ((NotaGeralCargahoraria.Nota_geral == null || NotaGeralCargahoraria.Nota_geral.ToString() == "0.00" || NotaGeralCargahoraria.Nota_geral.ToString() == "0,00") ? "--" : NotaGeralCargahoraria.Nota_geral.Value.ToString("N2"));
                        anoSitucaoAluno = anoPeriodo.ano;

                        if ((NotaGeralCargahoraria.Situacao_hist == "Rep Nota") || (NotaGeralCargahoraria.Situacao_hist == "REP NOTA"))
                        {
                            if (anoPeriodo.Ano != "2020" && anoPeriodo.Ano != "2021")
                            {
                                cell = new PdfPCell(new Phrase("(***)\n" + nota_geral.ToString() + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("(***)\n" + "--" + "", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }

                            flagSituacaoDisciplina = NotaGeralCargahoraria.Situacao_hist;
                        }
                        else
                        {
                            if (anoPeriodo.Ano != "2020" && anoPeriodo.Ano != "2021")
                            {
                                cell = new PdfPCell(new Phrase(nota_geral.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }
                        }
                    }

                    tblfundamental.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase(carga_horaria.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);

                //Buscar a freq anual e situação final do aluno

                situacao_Final_Freq_total = rnHistorico.situacaofinalDoAlunoPor(heDto.Pessoa, serie, anoSitucaoAluno, tipo);

                //FREQUENCIA TOTAL
                if (anoPeriodo.Ano != "2020" && anoPeriodo.Ano != "2021")
                {
                    cell = new PdfPCell(new Phrase(situacao_Final_Freq_total.Freqtotal.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                }
                else
                {
                    cell = new PdfPCell(new Phrase("--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                }

                tblfundamental.AddCell(cell);

                cell = new PdfPCell(new Phrase(situacao_Final_Freq_total.SituacaoFinal.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);

                PdfPTable tblEst = new PdfPTable(disciplinas.Count() + 4);

                vidaPregressa = rnHistorico.listarVidaPregressaDoAlunoPor(heDto.Pessoa, tipo);

                var escola = vidaPregressa.Where(x => (x.Serie == serie)).Select(e => e.Escola).FirstOrDefault();

                var municipio = vidaPregressa.Where(x => (x.Serie == serie)).Select(m => m.Municipio).FirstOrDefault();

                var ano = vidaPregressa.Where(x => (x.Serie == serie)).Select(a => a.Ano).FirstOrDefault();

                PdfPCell cellEst = new PdfPCell(new Phrase("Estabelecimento de Ensino: " + escola + "           Município: " + municipio + "           Ano: " + ano, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
                cellEst.Colspan = disciplinas.Count() + 4;
                tblEst.AddCell(cellEst);
                cell.Colspan = disciplinas.Count() + 4;
                cell.AddElement(tblEst);
                tblfundamental.AddCell(cell);
            }
            #endregion tabela listarDisciplinas


            #region Lista as disciplinas progressivas
            if (anosHistorico != null)
            {
                foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO listAnos in anosHistorico)
                {
                    RN.Certificacao.DTOs.HistoricoEscolarDTO anoD = new HistoricoEscolarDTO();
                    anoD.Ano = listAnos.Ano;
                    anoD.Pessoa = listAnos.Pessoa;
                    anoD.MatriculaAluno = listAnos.MatriculaAluno;
                    anoD.Semestre = listAnos.Semestre;

                    rnDisciplinaProgressiva = rnHistorico.listarAlunosQueRealizaramDepedenciaPor(anoD.Pessoa, anoD.Ano, anoD.Semestre);

                    if ((rnDisciplinaProgressiva != null) || (rnDisciplinaProgressiva.Count > 1))
                    {
                        foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO histDTO in rnDisciplinaProgressiva)
                        {
                            RN.Certificacao.DTOs.HistoricoEscolarDTO histDep = new HistoricoEscolarDTO();

                            histDep.MatriculaAluno = histDTO.MatriculaAluno;
                            histDep.Ano = histDTO.Ano;
                            histDep.Unidade_ens = histDTO.Unidade_ens;
                            histDep.Escola = histDTO.Escola;
                            histDep.Disciplina = histDTO.Disciplina;
                            histDep.Turma = histDTO.Turma;
                            histDep.NotaFinal = histDTO.NotaFinal;
                            histDep.Situacao_hist = histDTO.Situacao_hist;
                            histDep.Serie = histDTO.Serie;
                            histDep.Dependencia = histDTO.Dependencia;
                            histDep.DisciplinaReferencia = histDTO.DisciplinaReferencia;
                            histDep.SerieReferencia = histDTO.SerieReferencia;
                            histDep.Pessoa = histDTO.Pessoa;
                            //lista usada para adicionar as disciplinas pendentes do aluno
                            listaDisciplinasProgressiva.Add(histDep);
                        } // fim do foreach histDto
                    }
                }// fim do foreach listAnos                              
            }
            #endregion Lista as disciplinas progressivas


            doc.Add(tblfundamental);

            doc.Add(new iTextSharp.text.Chunk("\n"));

            PdfPTable tblInformacao = new PdfPTable(1);
            tblInformacao.TotalWidth = 560f;
            tblInformacao.LockedWidth = true;

            PdfPCell cellInformacao = new PdfPCell() { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 1 };

            cellInformacao = new PdfPCell(new Paragraph("OBSERVAÇÃO :" + obsHistorico + " ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            string legenda = "Legenda : C/H = Carga horária; F.A. = Frequencia Anual; APROV. = Aprovado***; APROV. C/DEP = Aprovado com dependência; DEP = Dependência REP NOTA = Reprovado por nota; REP FREQ = Reprovado por frequência; PROMOV = Promovido com Continuidade Escolar***; RETIDO****";
            cellInformacao = new PdfPCell(new Paragraph("\n" + legenda + " ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            //Demanda 8514            
            cellInformacao = new PdfPCell(new Paragraph("Histórico escolar anterior em anexo, conforme art. 5º da deliberação CEE nº 363/2017. Critérios de avaliação estabelecidos pela Portaria SEEDUC/SUGEN nº 419/2013", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            //fim Demanda 8514

            cellInformacao = new PdfPCell(new Paragraph("(*) Matrícula facultativa para o aluno ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            cellInformacao = new PdfPCell(new Paragraph("(**) Projeto definido pela unidade escolar obrigatória aos alunos que não optaram pela disciplinas de matrícula facultativa. ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            cellInformacao = new PdfPCell(new Paragraph("(***) Conforme os termos do artigo 8º da  Resolução SEEDUC Nº 5879 de 13 de outubro de 2020.", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            cellInformacao = new PdfPCell(new Paragraph("(****) Conforme os termos dos § 3º e 4º do artigo 12 da Resolução SEEDUC Nº 5879 de 13 de outubro de 2020. ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);

            #region Informação das disciplinas progressivas
            //Possui disciplinas com dependência progressiva
            if ((listaDisciplinasProgressiva != null) || (listaDisciplinasProgressiva.Count > 0))
            {
                foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO disciplinaProgressiva in listaDisciplinasProgressiva.Distinct().ToList())
                {
                    RN.Certificacao.DTOs.HistoricoEscolarDTO disProg = new HistoricoEscolarDTO();
                    disProg.Ano = disciplinaProgressiva.Ano;
                    disProg.Turma = disciplinaProgressiva.Turma;
                    disProg.Disciplina = disciplinaProgressiva.Disciplina;
                    disProg.Situacao_hist = disciplinaProgressiva.Situacao_hist;
                    disProg.SerieReferencia = disciplinaProgressiva.SerieReferencia;
                    string nota_geral2 = ((disciplinaProgressiva.NotaFinal == "null" || disciplinaProgressiva.NotaFinal.IsNullOrEmptyOrWhiteSpace()) ? "(não informada)" : disciplinaProgressiva.NotaFinal);
                    cellInformacao = new PdfPCell(new Paragraph(" (***) Em " + disProg.Ano + ", o(a) aluno(a) cumpriu dependência na turma " + disProg.Turma + " na disciplina " + disProg.Disciplina + " referente a " + disProg.SerieReferencia + " série obtendo nota final " + nota_geral2 + " , tendo sido " + disProg.Situacao_hist + " ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
                    tblInformacao.AddCell(cellInformacao);
                }
            }

            if ((listaDisciplinasProgressiva.Count == 0) && (flagSituacaoDisciplina == "Rep Nota"))
            {
                cellInformacao = new PdfPCell(new Paragraph(" \n(***) Este aluno não realizou a dependência desta disciplina ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
                tblInformacao.AddCell(cellInformacao);
            }


            doc.Add(tblInformacao);

            #endregion Informação das disciplinas progressivas



            //Rodapé
            #region
            iTextSharp.text.Paragraph txtRio = new iTextSharp.text.Paragraph("Rio de Janeiro,___/___/_____ ", times_Assinatura_Font);
            txtRio.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            txtRio.SpacingAfter = 12f;
            doc.Add(txtRio);

            iTextSharp.text.pdf.PdfPTable tblAssinatura = new PdfPTable(2);
            tblAssinatura.TotalWidth = 550f;
            tblAssinatura.LockedWidth = true;
            PdfPCell celLinha1 = new PdfPCell(new iTextSharp.text.Phrase("______________________________ \n     Secretário Escolar", times_Assinatura_Font));
            celLinha1.Colspan = 1;
            celLinha1.HorizontalAlignment = 1;
            celLinha1.BorderWidth = 0;
            tblAssinatura.AddCell(celLinha1);

            PdfPCell celLinha2 = new PdfPCell(new iTextSharp.text.Phrase("______________________________ \n      Diretor", times_Assinatura_Font));
            celLinha2.Colspan = 1;
            celLinha2.HorizontalAlignment = 1;
            celLinha2.BorderWidth = 0;
            tblAssinatura.AddCell(celLinha2);
            doc.Add(tblAssinatura);
            #endregion


        }



        /// <summary>
        ///     monta somente o corpo do histórico escolar do ensino médio
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="heDto"></param>
        /// <param name="tipo"></param>
        /// <param name="obsHistorico"></param>
        public void corpoHistoricoMedio(iTextSharp.text.Document doc, RN.Certificacao.DTOs.HistoricoEscolarDTO heDto, int tipo, string obsHistorico)
        {
            List<HistoricoEscolarDTO> listSitHistorico = new List<HistoricoEscolarDTO>();

            var helvetica_boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 7);
            var heveltica_Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7);
            var times_Assinatura_Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8);

            anoSemestre = rnHistorico.listarAnoSemestrePor(heDto.Pessoa, tipo);
            listarTodasAsDisciplinas = rnHistorico.listarTodasAsDisciplinasPor(heDto.Pessoa, tipo);
            disciplinas = rnHistorico.listarDisciplinasHistoricoEscolarPor(heDto.Pessoa, tipo);

            var series = listarTodasAsDisciplinas.Select(x => x.Serie).Distinct().ToList();

            int qtdCelulasPorLinha = ((series.Count() * 2) + 2);
            PdfPTable tblfundamental = new PdfPTable(qtdCelulasPorLinha);

            tblfundamental.TotalWidth = 550f;
            tblfundamental.LockedWidth = true;

            PdfPCell cell = new PdfPCell();
            float[] widths = new float[qtdCelulasPorLinha];
            int pos = 0;

            #region Monta as colunas de forma dinâmica
            while (pos < (qtdCelulasPorLinha - 1))
            {
                //Primeira coluna
                if (pos == 0)
                {
                    widths[pos] = 10f;
                    pos++;
                }
                //Preenchimento das séries
                while (pos < ((series.Count * 2) + 1))
                {
                    widths[pos] = 5f;
                    pos++;
                    //ùltima coluna
                    if (pos == (qtdCelulasPorLinha - 1))
                    {
                        widths[pos] = 10f;
                        break;
                    }
                }
            }
            #endregion

            tblfundamental.SetWidths(widths);

            int carga_horaria = 0;
            int carga_horaria_total = 0;
            string anoSituacaoAluno = string.Empty;
            string serieSituacaoAluno = string.Empty;
            HistoricoEscolarDTO situacao_Final_Freq_total = new HistoricoEscolarDTO();

            var totalCh = new List<decimal?>(series.Count());
            for (var i = 0; i < series.Count(); i++)
                totalCh.Add(0);


            for (var i = 0; i < qtdCelulasPorLinha * 3; i++)
            {
                if (i == 0)
                {
                    cell = new PdfPCell(new Paragraph("DISCIPLINAS", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Rowspan = 3 };
                    tblfundamental.AddCell(cell);
                }
            }

            #region cabeçalho disciplinas
            foreach (var notaCH in anoSemestre)
            {

                cell = new PdfPCell(new Paragraph("Ano: " + notaCH.Ano.ToString(), helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Colspan = 2 };
                tblfundamental.AddCell(cell);
            }


            cell = new PdfPCell(new Paragraph("CARGA HORÁRIA TOTAL", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Rowspan = 3 };
            tblfundamental.AddCell(cell);

            foreach (var notaCH in anoSemestre)
            {
                cell = new PdfPCell(new Paragraph(notaCH.Serie.ToString() + "ª Série", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Colspan = 2 };
                tblfundamental.AddCell(cell);
            }

            foreach (var notaCH in anoSemestre)
            {
                cell = new PdfPCell(new Paragraph("Nota/Conc.", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);

                cell = new PdfPCell(new Paragraph("C/H", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);
            }

            #endregion



            #region disciplinas
            anosHistorico = rnHistorico.RetornaAnosDeHistoricoPor(heDto.Pessoa);

            foreach (var disciplina in disciplinas)
            {
                //Disciplinas
                cell = new PdfPCell(new Phrase(disciplina.GrupoDisciplinas, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);

                foreach (string serie in series)
                {
                    HistoricoEscolarDTO sitHistorico = new HistoricoEscolarDTO();

                    // mpetod que retorna ano e periodo que aquela pessoa fez naquela serie agrup. 
                    var anoPeriodo = anoSemestre.Where(x => x.serie == serie).FirstOrDefault();


                    HistoricoEscolarDTO NotaGeralCargahoraria = new HistoricoEscolarDTO();

                    //Verifica se é agrupamento de OPTATIVAS
                    if (disciplina.Agrupamento == "LEOPT" || disciplina.Agrupamento == "REOPT")
                    {
                        //Verifica se a matriz curricular da turma principal oferece
                        bool oferece = rnHistorico.OfereceOptativaHabilitadaPor(heDto.Pessoa, anoPeriodo.Ano, anoPeriodo.Semestre, serie, tipo, disciplina.Agrupamento);

                        if (oferece)
                        {
                            NotaGeralCargahoraria = rnHistorico.obtemNotaPor(heDto.Pessoa, anoPeriodo.Ano, anoPeriodo.Semestre, serie, disciplina.Agrupamento, tipo);
                            cell = new PdfPCell(new Phrase(NotaGeralCargahoraria.Nota_geral.HasValue ? NotaGeralCargahoraria.Nota_geral.Value.ToString("N2") : "--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };

                            //Caso a turma optativa seja ofertada, mesmo que o aluno não cumpra considerar carga horaria 40 caso não possua carga horaria                           
                            if (NotaGeralCargahoraria.CargaHoraria == null || NotaGeralCargahoraria.CargaHoraria == 0)
                            {
                                NotaGeralCargahoraria.CargaHoraria = 40;
                            }
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase("--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                        }
                    }
                    else
                    {

                        NotaGeralCargahoraria = rnHistorico.obtemNotaPor(heDto.Pessoa, anoPeriodo.Ano, anoPeriodo.Semestre, serie, disciplina.Agrupamento, tipo);

                        //Nota 
                        if ((NotaGeralCargahoraria.Situacao_hist == "Rep Nota") || (NotaGeralCargahoraria.Situacao_hist == "REP NOTA"))
                        {
                            if (anoPeriodo.Ano != "2020" && anoPeriodo.Ano != "2021")
                            {
                                cell = new PdfPCell(new Phrase(NotaGeralCargahoraria.Nota_geral.HasValue ? "(***)" + NotaGeralCargahoraria.Nota_geral.Value.ToString("N2") + "" : "--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase(NotaGeralCargahoraria.Nota_geral.HasValue ? "(***)" + "--" + "" : "--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }

                            flagSituacaoDisciplina = NotaGeralCargahoraria.Situacao_hist;
                        }
                        else
                        {
                            if (anoPeriodo.Ano != "2020" && anoPeriodo.Ano != "2021")
                            {
                                cell = new PdfPCell(new Phrase(NotaGeralCargahoraria.Nota_geral.HasValue ? NotaGeralCargahoraria.Nota_geral.Value.ToString("N2") : "--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase(NotaGeralCargahoraria.Nota_geral.HasValue ? "--" : "--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                            }
                        }
                    }

                    tblfundamental.AddCell(cell);

                    //Carga Horária
                    cell = new PdfPCell(new Phrase(NotaGeralCargahoraria.CargaHoraria.HasValue ? NotaGeralCargahoraria.CargaHoraria.Value.ToString("N0") : "--", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                    tblfundamental.AddCell(cell);


                    //somatório de carga horária da série
                    int index = series.IndexOf(serie);
                    totalCh[index] += NotaGeralCargahoraria.CargaHoraria ?? 0;

                    //somatório de ch
                    carga_horaria += Convert.ToInt32(NotaGeralCargahoraria.CargaHoraria != null ? NotaGeralCargahoraria.CargaHoraria : 0);
                    carga_horaria_total += Convert.ToInt32(NotaGeralCargahoraria.CargaHoraria != null ? NotaGeralCargahoraria.CargaHoraria : 0);

                    sitHistorico = rnHistorico.situacaofinalDoAlunoPor(heDto.Pessoa, serie, anoPeriodo.Ano, tipo);
                    listSitHistorico.Add(sitHistorico);

                }
                //Carga Horária total
                cell = new PdfPCell(new Phrase(carga_horaria.ToString("N0"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);

                //zerando somatório de ch
                carga_horaria = 0;

            }


            #region Lista as disciplinas progressivas
            if (anosHistorico != null)
            {
                foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO listAnos in anosHistorico)
                {
                    RN.Certificacao.DTOs.HistoricoEscolarDTO anoD = new HistoricoEscolarDTO();
                    anoD.Ano = listAnos.Ano;
                    anoD.Pessoa = listAnos.Pessoa;
                    anoD.MatriculaAluno = listAnos.MatriculaAluno;
                    anoD.Semestre = listAnos.Semestre;

                    rnDisciplinaProgressiva = rnHistorico.listarAlunosQueRealizaramDepedenciaPor(anoD.Pessoa, anoD.Ano, anoD.Semestre);

                    if ((rnDisciplinaProgressiva != null) || (rnDisciplinaProgressiva.Count > 1))
                    {
                        foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO histDTO in rnDisciplinaProgressiva)
                        {
                            RN.Certificacao.DTOs.HistoricoEscolarDTO histDep = new HistoricoEscolarDTO();

                            histDep.MatriculaAluno = histDTO.MatriculaAluno;
                            histDep.Ano = histDTO.Ano;
                            histDep.Unidade_ens = histDTO.Unidade_ens;
                            histDep.Escola = histDTO.Escola;
                            histDep.Disciplina = histDTO.Disciplina;
                            histDep.Turma = histDTO.Turma;
                            histDep.NotaFinal = histDTO.NotaFinal;
                            histDep.Situacao_hist = histDTO.Situacao_hist;
                            histDep.Serie = histDTO.Serie;
                            histDep.Dependencia = histDTO.Dependencia;
                            histDep.DisciplinaReferencia = histDTO.DisciplinaReferencia;
                            histDep.SerieReferencia = histDTO.SerieReferencia;
                            histDep.Pessoa = histDTO.Pessoa;
                            //lista usada para adicionar as disciplinas pendentes do aluno
                            listaDisciplinasProgressiva.Add(histDep);
                        } // fim do foreach histDto
                    }
                }// fim do foreach listAnos                              
            }
            #endregion Lista as disciplinas progressivas


            #endregion disciplina



            #region base do histórico escolar (Freq.Anual, Situação e Total)
            //Linhas vazias
            for (int i = 0; i < qtdCelulasPorLinha; i++)
            {
                cell = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7f))) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);
            }

            //Linha Total
            cell = new PdfPCell(new Phrase(" TOTAL ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);
            //valores de total
            int ctCH = 0;
            foreach (var notaCH in anoSemestre)
            {
                cell = new PdfPCell(new Paragraph(" ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);

                var currentTotalCh = totalCh[ctCH];
                cell = new PdfPCell(new Paragraph(currentTotalCh.HasValue ? currentTotalCh.Value.ToString("N0") : "--", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblfundamental.AddCell(cell);
                ctCH++;
            }

            cell = new PdfPCell(new Phrase(carga_horaria_total.ToString("N0"), helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Rowspan = 3 };
            tblfundamental.AddCell(cell);


            //Freq. Anual
            cell = new PdfPCell(new Phrase(" % FREQUÊNCIA ANUAL ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);
            //valores de total
            int ctFreq = 0;
            foreach (var notaCH in anoSemestre)
            {
                if (notaCH.Ano != "2020" && notaCH.Ano != "2021")
                {
                    cell = new PdfPCell(new Paragraph(listSitHistorico[ctFreq].Freqtotal.ToString() + " %", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Colspan = 2 };
                }
                else
                {
                    cell = new PdfPCell(new Paragraph("--", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Colspan = 2 };

                }
                tblfundamental.AddCell(cell);
                ctFreq++;
            }


            //Situação Final
            cell = new PdfPCell(new Phrase(" SITUAÇÃO FINAL ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblfundamental.AddCell(cell);
            //valores de total
            int ctSit = 0;
            foreach (var notaCH in anoSemestre)
            {
                cell = new PdfPCell(new Paragraph(listSitHistorico[ctSit].SituacaoFinal.ToString(), helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1, Colspan = 2 };
                tblfundamental.AddCell(cell);
                ctSit++;
            }
            #endregion base do histórico escolar


            doc.Add(tblfundamental);

            //doc.Add(new iTextSharp.text.Paragraph("\n"));

            #region Vida Pregressa

            iTextSharp.text.pdf.PdfPTable tblEscolar = new PdfPTable(4);
            tblEscolar.TotalWidth = 550f;
            tblEscolar.LockedWidth = true;

            float[] widths_tblEscolar = new float[] { 10f, 20f, 10f, 10f };
            tblEscolar.SetWidths(widths_tblEscolar);

            PdfPCell cellEscolar = new PdfPCell();
            cellEscolar = new PdfPCell(new Phrase(" SÉRIE ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblEscolar.AddCell(cellEscolar);
            cellEscolar = new PdfPCell(new Phrase(" ESTABELECIMENTO DE ENSINO ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblEscolar.AddCell(cellEscolar);
            cellEscolar = new PdfPCell(new Phrase(" MUNICÍPIO/ESTADO ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblEscolar.AddCell(cellEscolar);
            cellEscolar = new PdfPCell(new Phrase("ANO ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            tblEscolar.AddCell(cellEscolar);

            vidaPregressa = rnHistorico.listarVidaPregressaDoAlunoPor(heDto.Pessoa, tipo);

            foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO historico in vidaPregressa)
            {
                RN.Certificacao.DTOs.HistoricoEscolarDTO he = new HistoricoEscolarDTO();
                he.Ano = historico.Ano;
                he.Escola = historico.Escola;
                he.Municipio = historico.Municipio;
                he.Serie = historico.Serie;

                cellEscolar = new PdfPCell(new Phrase(he.Serie + "ª ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblEscolar.AddCell(cellEscolar);
                cellEscolar = new PdfPCell(new Phrase(he.Escola, heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblEscolar.AddCell(cellEscolar);
                cellEscolar = new PdfPCell(new Phrase(he.Municipio, heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblEscolar.AddCell(cellEscolar);
                cellEscolar = new PdfPCell(new Phrase(he.Ano, heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
                tblEscolar.AddCell(cellEscolar);
            }

            doc.Add(tblEscolar);

            #endregion

            PdfPTable tblInformacao = new PdfPTable(1);
            tblInformacao.TotalWidth = 560f;
            tblInformacao.LockedWidth = true;

            PdfPCell cellInformacao = new PdfPCell() { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 1 };

            cellInformacao = new PdfPCell(new Paragraph("OBSERVAÇÃO :" + obsHistorico + " ", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            string legenda = "Legenda : C/H = Carga horária; APROV = Aprovado***; APROV. C/DEP = Aprovado com dependência; DEP = Dependência; REP NOTA = Reprovado por nota; REP FREQ = Reprovado por frequência; PROMOV = Promovido com Continuidade Escolar***; RETIDO****";
            cellInformacao = new PdfPCell(new Paragraph("\n" + legenda + " ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);

            //Demanda 8514            
            cellInformacao = new PdfPCell(new Paragraph("Histórico escolar anterior em anexo, conforme art. 5º da deliberação CEE nº 363/2017. Critérios de avaliação estabelecidos pela Portaria SEEDUC/SUGEN nº 419/2013", helvetica_boldFont)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            //fim Demanda 8514

            cellInformacao = new PdfPCell(new Paragraph("(*) Matrícula facultativa para o aluno ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            cellInformacao = new PdfPCell(new Paragraph("(**) Projeto definido pela unidade escolar obrigatória aos alunos que não optaram pelas disciplinas de matrícula facultativa. ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            cellInformacao = new PdfPCell(new Paragraph("(***) Conforme os termos do artigo 8º da  Resolução SEEDUC Nº 5879 de 13 de outubro de 2020. ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);
            cellInformacao = new PdfPCell(new Paragraph("(****) Conforme os termos dos § 3º e 4º do artigo 12 da Resolução SEEDUC Nº 5879 de 13 de outubro de 2020.", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
            tblInformacao.AddCell(cellInformacao);

            #region Informações

            //Possui disciplinas com dependência progressiva
            if ((listaDisciplinasProgressiva != null) || (listaDisciplinasProgressiva.Count > 0))
            {
                foreach (RN.Certificacao.DTOs.HistoricoEscolarDTO disciplinaProgressiva in listaDisciplinasProgressiva.Distinct().ToList())
                {
                    RN.Certificacao.DTOs.HistoricoEscolarDTO disProg = new HistoricoEscolarDTO();
                    disProg.Ano = disciplinaProgressiva.Ano;
                    disProg.Turma = disciplinaProgressiva.Turma;
                    disProg.Disciplina = disciplinaProgressiva.Disciplina;
                    disProg.Situacao_hist = disciplinaProgressiva.Situacao_hist;
                    disProg.SerieReferencia = disciplinaProgressiva.SerieReferencia;
                    string nota_geral2 = ((disciplinaProgressiva.NotaFinal == "null" || disciplinaProgressiva.NotaFinal.IsNullOrEmptyOrWhiteSpace()) ? "(não informada)" : disciplinaProgressiva.NotaFinal);
                    cellInformacao = new PdfPCell(new Paragraph("(***) Em " + disProg.Ano + ", o(a) aluno(a) cumpriu dependência na turma " + disProg.Turma + " na disciplina " + disProg.Disciplina + " referente a " + disProg.SerieReferencia + " série obtendo nota final " + nota_geral2 + " , tendo sido " + disProg.Situacao_hist + " ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
                    tblInformacao.AddCell(cellInformacao);
                }
            }

            if ((listaDisciplinasProgressiva.Count == 0) && (flagSituacaoDisciplina == "Rep Nota"))
            {
                cellInformacao = new PdfPCell(new iTextSharp.text.Paragraph("(***) Este aluno não realizou a dependência desta disciplina ", heveltica_Font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
                tblInformacao.AddCell(cellInformacao);
            }

            doc.Add(tblInformacao);

            #endregion Informações

            //Rodapé

            #region Rodape
            iTextSharp.text.Paragraph txtRio = new iTextSharp.text.Paragraph("Rio de Janeiro,___/___/_____ ", times_Assinatura_Font);
            txtRio.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            txtRio.SpacingAfter = 12f;
            doc.Add(txtRio);

            iTextSharp.text.pdf.PdfPTable tblAssinatura = new PdfPTable(2);
            tblAssinatura.TotalWidth = 550f;
            tblAssinatura.LockedWidth = true;
            PdfPCell celLinha1 = new PdfPCell(new iTextSharp.text.Phrase("______________________________ \n      Secretário Escolar", times_Assinatura_Font));
            celLinha1.Colspan = 1;
            celLinha1.HorizontalAlignment = 1;
            celLinha1.BorderWidth = 0;
            tblAssinatura.AddCell(celLinha1);

            PdfPCell celLinha2 = new PdfPCell(new iTextSharp.text.Phrase("______________________________ \n      Diretor", times_Assinatura_Font));
            celLinha2.Colspan = 1;
            celLinha2.HorizontalAlignment = 1;
            celLinha2.BorderWidth = 0;
            tblAssinatura.AddCell(celLinha2);
            doc.Add(tblAssinatura);
            #endregion Rodape

        }




        /// <summary>
        /// realiza a montagem dos históricos informando a matrícula do aluno e o seu tipo de conclusão
        /// </summary>
        /// <param name="heDto"></param>
        /// <param name="tipo"></param>
        /// <param name="nomeArquivo"></param>
        /// <param name="obsHistorico"></param> 
        public void montarHistoricoEscolar(RN.Certificacao.DTOs.HistoricoEscolarDTO heDto, int tipo, string nomeArquivo, string obsHistorico)
        {
            var helvetica_boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
            var heveltica_Font = FontFactory.GetFont(FontFactory.HELVETICA, 7);

            var times_Font9 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9);

            var times_Font7 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7);
            var times_Font7_negrito = FontFactory.GetFont(FontFactory.TIMES_BOLD, 7);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 40);
            //string imgCabecalho = HttpContext.Current.Server.MapPath("Img/brasaoPequeno.png");
            string imgCabecalho = HttpContext.Current.Server.MapPath("Img/Brasao.png");



            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/pdf";
            response.AddHeader("content-disposition", string.Format("attachment;filename={0}.pdf", nomeArquivo));
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            PdfWriter.GetInstance(doc, response.OutputStream);

            doc.Open();
            RN.Certificacao.DTOs.HistoricoEscolarDTO heEscola = new HistoricoEscolarDTO();
            heEscola = rnHistorico.obterDadosDaEscolaPor(heDto.Pessoa, tipo);


            #region cabeçalho
            //Logo             
            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imgCabecalho);
            png.ScaleAbsoluteHeight(80);
            png.ScaleAbsoluteWidth(64);
            png.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_LEFT;
            png.IndentationLeft = 9f;
            png.SpacingAfter = 5f;
            //png.BorderWidthTop = 10f;
            //png.BorderColorTop = iTextSharp.text.BaseColor.WHITE;



            PdfPTable tblCab = new PdfPTable(3) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
            PdfPCell cellCab = new PdfPCell();
            cellCab.Rowspan = 4;
            cellCab = new PdfPCell(new iTextSharp.text.Phrase("")) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
            tblCab.AddCell(cellCab);

            PdfPTable tblImg = new PdfPTable(1) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
            PdfPCell cellImg = new PdfPCell();
            cellImg = new PdfPCell(png) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
            tblImg.AddCell(cellImg);
            cellCab.AddElement(tblImg);
            tblCab.AddCell(cellCab);

            PdfPTable tblAto = new PdfPTable(1);
            PdfPCell cellAto = new PdfPCell() { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 };
            cellAto = new PdfPCell(new iTextSharp.text.Paragraph("Ato Autorizativo do Poder Público", times_Font9)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0, };
            tblAto.AddCell(cellAto);
            cellAto = new PdfPCell(new iTextSharp.text.Paragraph((heEscola.Decreto.ToString().IsNullOrEmptyOrWhiteSpace() ? "_______" : heEscola.Decreto.ToString()) + " ", times_Font9)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
            tblAto.AddCell(cellAto);
            cellAto = new PdfPCell(new Paragraph("D.O.:" + ((heEscola.DataDO.Value.ToString("dd/MM/yyyy").IsNullOrEmptyOrWhiteSpace() || heEscola.DataDO.Value.ToString("") == "01/01/0001 00:00:00") ? "______" : heEscola.DataDO.Value.ToString("dd/MM/yyyy")) + " ", times_Font9)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
            tblAto.AddCell(cellAto);
            cellAto = new PdfPCell(new Paragraph("U.A.: " + heEscola.Ua.ToString() + " ", times_Font9)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
            tblAto.AddCell(cellAto);
            cellAto = new PdfPCell(new Paragraph("Censo Escolar:" + heEscola.Unidade_ens.ToString() + " ", times_Font9)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
            tblAto.AddCell(cellAto);

            cellCab = new PdfPCell(tblAto) { Border = 1 };
            cellCab.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellCab.BorderWidth = 0;
            tblCab.AddCell(cellCab);
            doc.Add(tblCab);


            // adcionado por Francisco

            iTextSharp.text.Paragraph txtCabGov = new iTextSharp.text.Paragraph("Governo do Estado do Rio de Janeiro ", times_Font9);
            txtCabGov.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            txtCabGov.SpacingAfter = -2f;
            doc.Add(txtCabGov);

            iTextSharp.text.Paragraph txtCabSec = new iTextSharp.text.Paragraph("Secretaria de Estado de Educação ", times_Font9);
            txtCabSec.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(txtCabSec);

            doc.Add(new iTextSharp.text.Chunk("\n"));
            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
            doc.Add(seperator);

            var phraseEstabelecimento = new Phrase();

            iTextSharp.text.Paragraph Estabelecimento = new iTextSharp.text.Paragraph("ESTABELECIMENTO DE ENSINO: ", times_Font7_negrito);
            Estabelecimento.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseEstabelecimento.Add(Estabelecimento);
            iTextSharp.text.Paragraph NomeEscola = new iTextSharp.text.Paragraph(heEscola.Escola.ToUpper() + "\n", times_Font7);
            phraseEstabelecimento.Add(NomeEscola);
            doc.Add(phraseEstabelecimento);

            var phraseEndereco = new Phrase();
            //Endereço
            iTextSharp.text.Paragraph txtEndereco = new iTextSharp.text.Paragraph("ENDEREÇO: ", times_Font7_negrito);
            txtEndereco.SpacingAfter = 10f;
            txtEndereco.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseEndereco.Add(txtEndereco);
            iTextSharp.text.Paragraph enderecoEscolar = new iTextSharp.text.Paragraph(heEscola.Endereco.ToUpper() + " ", times_Font7);
            phraseEndereco.Add(enderecoEscolar);

            //Município
            iTextSharp.text.Paragraph txtMunicipio = new iTextSharp.text.Paragraph("   MUNICÍPIO: ", times_Font7_negrito);
            phraseEndereco.Add(txtMunicipio);
            iTextSharp.text.Paragraph municipioEscolar = new iTextSharp.text.Paragraph(heEscola.Municipio.ToUpper() + " ", times_Font7);
            phraseEndereco.Add(municipioEscolar);

            //CEP
            iTextSharp.text.Paragraph txtCEP = new iTextSharp.text.Paragraph("   CEP: ", times_Font7_negrito);
            phraseEndereco.Add(txtCEP);
            iTextSharp.text.Paragraph CEPEscolar = new iTextSharp.text.Paragraph(heEscola.Cep.FormataCep() + "\n", times_Font7);
            phraseEndereco.Add(CEPEscolar);

            doc.Add(phraseEndereco);

            doc.Add(new iTextSharp.text.Chunk("\n"));
            iTextSharp.text.Chunk txtH = new iTextSharp.text.Chunk("HISTÓRICO ESCOLAR", times_Font7_negrito);
            txtH.SetUnderline(0.5f, -1.5f);
            iTextSharp.text.Paragraph txtHistorico = new iTextSharp.text.Paragraph(txtH);
            txtHistorico.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(txtHistorico);

            //Tipo de conclusão

            string descricaoTipoConclusaoID = string.Empty;
            if (tipo == 1)
                descricaoTipoConclusaoID = "ENSINO FUNDAMENTAL";
            else if (tipo == 2)
                descricaoTipoConclusaoID = "ENSINO MÉDIO";
            else if (tipo == 3)
                descricaoTipoConclusaoID = "ENSINO PROFISSIONALIZANTE";

            //Nome do Curso
            var phraseCurso = new Phrase();
            iTextSharp.text.Paragraph txtCurso = new iTextSharp.text.Paragraph("CURSO: ", times_Font7_negrito);
            txtCurso.SpacingAfter = 5f;
            txtCurso.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseCurso.Add(txtCurso);
            iTextSharp.text.Paragraph CursoEscola = new iTextSharp.text.Paragraph(descricaoTipoConclusaoID.ToUpper() + "\n", times_Font7);
            phraseCurso.Add(CursoEscola);
            doc.Add(phraseCurso);

            //Nome do Aluno
            var phraseNome = new Phrase();
            iTextSharp.text.Paragraph txtNome = new iTextSharp.text.Paragraph("NOME DO ALUNO: ", times_Font7_negrito);
            phraseNome.Add(txtNome);

            iTextSharp.text.Paragraph nomeAluno = new iTextSharp.text.Paragraph(heDto.NomeAluno.Trim().ToUpper() + " ", times_Font7);
            txtNome.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseNome.Add(nomeAluno);
            doc.Add(phraseNome);

            //Matrícula do Aluno
            var phraseMatriculaAluno = new Phrase();
            string matriculaAluno = (heDto.MatriculaAluno.Trim() != null ? heDto.MatriculaAluno.Trim() : "NÃO INFORMADO");
            iTextSharp.text.Paragraph txtMatricula = new iTextSharp.text.Paragraph("    Nº MATRÍCULA NO SISTEMA CONEXÃO EDUCAÇÃO: ", times_Font7_negrito);
            txtMatricula.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            //txtMatricula.SpacingAfter = 18f;
            phraseMatriculaAluno.Add(txtMatricula);

            iTextSharp.text.Paragraph matriculaAlunoConexao = new iTextSharp.text.Paragraph(matriculaAluno + "\n", times_Font7);
            phraseMatriculaAluno.Add(matriculaAlunoConexao);

            doc.Add(phraseMatriculaAluno);

            //Nacionalidade
            var phraseNacionalidade = new Phrase();
            iTextSharp.text.Paragraph txtNacionalidade = new iTextSharp.text.Paragraph("NACIONALIDADE: ", times_Font7_negrito);
            txtNacionalidade.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseNacionalidade.Add(txtNacionalidade);

            iTextSharp.text.Paragraph nacionalidadeAluno = new iTextSharp.text.Paragraph(heDto.Nacionalidade.Trim().ToUpper(), times_Font7);
            phraseNacionalidade.Add(nacionalidadeAluno);

            //Naturalidade
            iTextSharp.text.Paragraph txtNaturalidade = new iTextSharp.text.Paragraph("   NATURALIDADE: ", times_Font7_negrito);
            txtNaturalidade.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseNacionalidade.Add(txtNaturalidade);

            iTextSharp.text.Paragraph naturalidadeAluno = new iTextSharp.text.Paragraph(heDto.Naturalidade.Trim().ToUpper() + " ", times_Font7);
            phraseNacionalidade.Add(naturalidadeAluno);

            //Data Nascimento
            string dt = ((heDto.DataNascimento.Value.ToString().IsNullOrEmptyOrWhiteSpace() ? "NÃO INFORMADO" : heDto.DataNascimento.Value.ToString("dd/MM/yyyy")));
            iTextSharp.text.Paragraph txtDataNascimento = new iTextSharp.text.Paragraph("   DATA DE NASCIMENTO: ", times_Font7_negrito);
            txtDataNascimento.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseNacionalidade.Add(txtDataNascimento);

            iTextSharp.text.Paragraph DataNascimentoAluno = new iTextSharp.text.Paragraph(dt + "\n", times_Font7);
            phraseNacionalidade.Add(DataNascimentoAluno);

            doc.Add(phraseNacionalidade);


            var phraseFiliacao = new Phrase();
            iTextSharp.text.Paragraph txtFiliacao = new iTextSharp.text.Paragraph("FILIAÇÃO: ", times_Font7_negrito);
            txtFiliacao.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            phraseFiliacao.Add(txtFiliacao);

            string nomeMae = heDto.FiliacaoMae.Trim().ToUpper();
            string nomePai = heDto.FiliacaoPai.Trim().ToUpper();
            iTextSharp.text.Paragraph filiacaoPaiMae = new iTextSharp.text.Paragraph(nomeMae + " e " + nomePai + "\n", times_Font7);
            phraseFiliacao.Add(filiacaoPaiMae);

            doc.Add(phraseFiliacao);

            if (tipo != 1)
            {
                string num_rg = (heDto.Num_rg.ToString() == "" || heDto.Num_rg.IsNullOrEmptyOrWhiteSpace() ? " NÃO INFORMADO " : heDto.num_rg.ToString());
                string orgaoExpedidor = (heDto.OrgaoExpedidor.ToString() == "" || heDto.OrgaoExpedidor.IsNullOrEmptyOrWhiteSpace() ? " NÃO INFORMADO " : heDto.OrgaoExpedidor.ToString());
                string dataExpedidor = (heDto.DataExpedicao == "" || heDto.DataExpedicao.IsNullOrEmptyOrWhiteSpace() ? " NÃO INFORMADO " : heDto.DataExpedicao);
                string estadoDoc = (heDto.EstadoDocumento == "" || heDto.EstadoDocumento.IsNullOrEmptyOrWhiteSpace() ? " NÃO INFORMADO " : heDto.EstadoDocumento);

                var phraseIdentidade = new Phrase();
                //nº da indentidade
                iTextSharp.text.Paragraph txtIdentidade = new iTextSharp.text.Paragraph("IDENTIDADE Nº: ", times_Font7_negrito);
                txtIdentidade.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                phraseIdentidade.Add(txtIdentidade);
                iTextSharp.text.Paragraph identidadeAluno = new iTextSharp.text.Paragraph(num_rg + " ", times_Font7);
                phraseIdentidade.Add(identidadeAluno);

                //Orgao Expedidor
                iTextSharp.text.Paragraph txtOrgaoExpedidor = new iTextSharp.text.Paragraph(" ORGÃO EXPEDIDOR: ", times_Font7_negrito);
                txtOrgaoExpedidor.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                phraseIdentidade.Add(txtOrgaoExpedidor);
                iTextSharp.text.Paragraph orgaoExpedidorAluno = new iTextSharp.text.Paragraph(orgaoExpedidor + " / " + estadoDoc + " ", times_Font7);
                phraseIdentidade.Add(orgaoExpedidorAluno);

                //Data Expedidor
                iTextSharp.text.Paragraph txtDataExpedidor = new iTextSharp.text.Paragraph(" DATA DE EXPEDIÇÃO: ", times_Font7_negrito);
                phraseIdentidade.Add(txtDataExpedidor);
                iTextSharp.text.Paragraph dataExpedidorAluno = new iTextSharp.text.Paragraph(dataExpedidor + "\n", times_Font7);
                phraseIdentidade.Add(dataExpedidorAluno);

                doc.Add(phraseIdentidade);
            }

            #endregion cabeçalho


            #region tipo do histórico escolar
            switch (tipo)
            {
                //Ensino Fundamental        
                case 1:
                    {
                        this.corpoHistoricoFundamental(doc, heDto, tipo, obsHistorico);
                        break;
                    }
                //Ensino Médio    
                case 2:
                    {
                        this.corpoHistoricoMedio(doc, heDto, tipo, obsHistorico);

                        //Verificar se exis te histórico do ensino fundamental
                        bool achou = false;
                        achou = rnHistorico.possuiNivelPor(heDto.Pessoa, 1);
                        if (achou)
                        {
                            doc.NewPage();

                            doc.Add(phraseNome);

                            doc.Add(new iTextSharp.text.Chunk("\n"));

                            iTextSharp.text.Chunk txtHistFundamental = new iTextSharp.text.Chunk("HISTÓRICO ESCOLAR DO ENSINO FUNDAMENTAL", times_Font7_negrito);
                            txtHistFundamental.SetUnderline(0.5f, -1.5f);
                            iTextSharp.text.Paragraph txtPrgHistFundamental = new iTextSharp.text.Paragraph(txtHistFundamental);
                            txtPrgHistFundamental.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            txtPrgHistFundamental.SpacingAfter = 10f;
                            doc.Add(txtPrgHistFundamental);

                            doc.Add(new iTextSharp.text.Chunk("\n"));

                            //retornar a observacao deste histórico fundamental
                            RN.Certificacao.Entidades.DocumentoCertificacao ObsHistFundamental = new Techne.Lyceum.RN.Certificacao.Entidades.DocumentoCertificacao();
                            ObsHistFundamental = rnHistorico.retornaObservacaoHistorico(heDto.MatriculaAluno, heDto.Pessoa, 1);

                            //Monta o histórico do ensino fundamental com essa observação
                            this.corpoHistoricoFundamental(doc, heDto, 1, ObsHistFundamental.Observacao);
                        }
                        break;
                    }
                //Ensino Profissionalizante        
                case 3:
                    {
                        this.corpoHistoricoMedio(doc, heDto, tipo, obsHistorico);
                        break;
                    }
                default:
                    break;
            }
            #endregion


            doc.Close();

        }




        /// <summary>
        ///  Nome do arquivo que será salvo no ato da geração do histórico escolar
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="matriculaAluno"></param>
        /// <returns></returns>
        private string nomeArquivoHistoricoEscolarPor(int tipo, string pessoaID)
        {
            string nomeArquivo = String.Empty;

            switch (tipo)
            {
                case 1:
                    nomeArquivo = "HEFundamental_" + pessoaID.ToString() + ".pdf";
                    break;
                case 2:
                    nomeArquivo = "HEMedio_" + pessoaID.ToString() + ".pdf";
                    break;
                case 3:
                    nomeArquivo = "HEProfissionalizante_" + pessoaID.ToString() + ".pdf";
                    break;
                default:
                    Console.WriteLine("Tipo de ensino inválido!!!");
                    break;
            }
            return nomeArquivo;
        }




        /// <summary>
        /// Ação que gera a criação do históricos escolar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGerarHistorico_Click(object sender, EventArgs e)
        {
            //Obter valores
            int tipoConclusaoID = Convert.ToInt32(ddlTipoConclusao.SelectedValue);
            pessoaID = Convert.ToString(tseAluno["PESSOA"]);
            string obsHistorico = txtObservacao.Text.Trim();

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.HistoricoEscolar rnHistorico = new Techne.Lyceum.RN.Certificacao.HistoricoEscolar();
            validacao = rnHistorico.ValidaHistorico(pessoaID, Convert.ToInt32(tipoConclusaoID), null, obsHistorico);

            if (validacao.Valido)
            {
                #region Dados da Ficha do Aluno(a)

                bool retorno = false;

                RN.Aluno rnAluno = new RN.Aluno();
                RN.DTOs.DadosFichaAluno dadosAluno = new Techne.Lyceum.RN.DTOs.DadosFichaAluno();
                RN.Certificacao.DocumentoGerado rnDocGerado = new Techne.Lyceum.RN.Certificacao.DocumentoGerado();

                docGerado = new RN.Certificacao.Entidades.DocumentoGerado();
                docCert = new RN.Certificacao.Entidades.DocumentoCertificacao();

                try
                {
                    if (tipoConclusaoID != -1)
                    {
                        //obtém dados pela pessoaID
                        dadosAluno = rnAluno.ObtemFichaAlunoPorPessoa(pessoaID);

                        //Adicionado por Ariadne em 03/06/2026
                        //Adicionado casos de brasileiros nascidos no exterior e estrangeiros
                        // Após: dadosAluno = rnAluno.ObtemFichaAlunoPorPessoa(pessoaID);

                        // --- INÍCIO: resolve naturalidade para estrangeiro/brasileiro no exterior ---
                        string naturalidadeResolvida;
                        if (!string.IsNullOrEmpty(dadosAluno.Naturalidade) && dadosAluno.Naturalidade.Trim() != "/")
                        {
                            naturalidadeResolvida = dadosAluno.Naturalidade;
                        }
                        else
                        {
                            try
                            {
                                DataTable dtNatEst = rnAluno.ObtemNaturalidadeEstrangeiraPor(dadosAluno.AlunoMatricula);
                                if (dtNatEst != null && dtNatEst.Rows.Count > 0)
                                {
                                    string munExt = dtNatEst.Rows[0]["NOME_MUNICIPIO"].ToString();
                                    string ufExt = dtNatEst.Rows[0]["NOME_ESTADO"].ToString();
                                    string paisExt = dtNatEst.Rows[0]["NOME_PAIS"].ToString();

                                    if (!string.IsNullOrEmpty(dadosAluno.Nacionalidade) &&
                                        dadosAluno.Nacionalidade.Trim().ToUpper() == "BRASILEIRA")
                                        naturalidadeResolvida = munExt + "/" + ufExt + " - " + paisExt;
                                    else
                                        naturalidadeResolvida = munExt + " - " + paisExt;
                                }
                                else
                                    naturalidadeResolvida = "NÃO INFORMADO";
                            }
                            catch
                            {
                                naturalidadeResolvida = "NÃO INFORMADO";
                            }
                        }
                        // --- FIM da resolução de naturalidade ---

                        RN.Certificacao.DTOs.HistoricoEscolarDTO heDto = new Techne.Lyceum.RN.Certificacao.DTOs.HistoricoEscolarDTO();
                        //Adcionado por Francisco em 15/01/2020
                        //Se algumas das informações abaixo for nula,vazia ou em branco o HISTÓRICO não poderá ser gerado.
                        //Para fins de teste deverá de ser comentado e retirá-lo assim que terminar

                        //if (
                        //    string.IsNullOrEmpty(dadosAluno.NomeAluno) ||
                        //    (dadosAluno.DataNascimento == null || dadosAluno.DataNascimento.ToString() == "") ||
                        //    (dadosAluno.Nacionalidade.IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.Naturalidade.IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.AlunoMatricula.IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.Pessoa.ToString().IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.NumeroDocumento.IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.DataExpedicaoDocumento == null || dadosAluno.DataExpedicaoDocumento.ToString() == "") ||
                        //    (dadosAluno.OrgaoEmissorDocumento.IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.EstadoDocumento.IsNullOrEmptyOrWhiteSpace()) ||
                        //    (dadosAluno.NomeMae.IsNullOrEmptyOrWhiteSpace() && dadosAluno.NomePai.IsNullOrEmptyOrWhiteSpace())
                        //    )
                        //{
                        //    throw new Exception("Existem dados obrigatórios do Aluno que não foram preenchidos.<BR>( DATA DE  NASCIMENTO, NACIONALIDADE, NATURALIDADE, NUMERO DO RG, DATA DE EXPEDICAO DO RG, UF DE EXPEDICAO DO RG, ORGÃO EMISSOR DO RG, NOME DO PAI E/OU NOME DA MÃE.)");
                        //}

                        //FIM

                        //Adicionado por Ariadne em 03/06/2026
                        // Valida naturalidade considerando estrangeiro/brasileiro no exterior
                        string naturalidadeValidacao = dadosAluno.Naturalidade;

                        bool naturalidadeVazia = string.IsNullOrEmpty(naturalidadeValidacao) ||
                                                 naturalidadeValidacao.Trim() == "/";

                        if (naturalidadeVazia)
                        {
                            try
                            {
                                DataTable dtNatEst = rnAluno.ObtemNaturalidadeEstrangeiraPor(dadosAluno.AlunoMatricula);
                                if (dtNatEst != null && dtNatEst.Rows.Count > 0)
                                {
                                    string munExt = dtNatEst.Rows[0]["NOME_MUNICIPIO"].ToString();
                                    string ufExt = dtNatEst.Rows[0]["NOME_ESTADO"].ToString();
                                    string paisExt = dtNatEst.Rows[0]["NOME_PAIS"].ToString();

                                    bool ehBrasileiro = !string.IsNullOrEmpty(dadosAluno.Nacionalidade) &&
                                                        dadosAluno.Nacionalidade.Trim().ToUpper() == "BRASILEIRA";

                                    naturalidadeValidacao = ehBrasileiro
                                        ? munExt + "/" + ufExt + " - " + paisExt
                                        : munExt + " - " + paisExt;
                                }
                            }
                            catch { /* validação abaixo tratará naturalidadeValidacao vazia */ }
                        }

                        // Determina se é estrangeiro (não-brasileiro)
                        bool ehEstrangeiro = !string.IsNullOrEmpty(dadosAluno.Nacionalidade) &&
                                             dadosAluno.Nacionalidade.Trim().ToUpper() != "BRASILEIRA";

                        // Para estrangeiros, RG não é obrigatório (podem não ter documento brasileiro)
                        bool rgObrigatorio = !ehEstrangeiro;

                        if (
                            string.IsNullOrEmpty(dadosAluno.NomeAluno) ||
                            (dadosAluno.DataNascimento == null || dadosAluno.DataNascimento.ToString() == "") ||
                            dadosAluno.Nacionalidade.IsNullOrEmptyOrWhiteSpace() ||
                            string.IsNullOrEmpty(naturalidadeValidacao) ||
                            dadosAluno.AlunoMatricula.IsNullOrEmptyOrWhiteSpace() ||
                            dadosAluno.Pessoa.ToString().IsNullOrEmptyOrWhiteSpace() ||
                            (rgObrigatorio && dadosAluno.NumeroDocumento.IsNullOrEmptyOrWhiteSpace()) ||
                            (rgObrigatorio && (dadosAluno.DataExpedicaoDocumento == null || dadosAluno.DataExpedicaoDocumento.ToString() == "")) ||
                            (rgObrigatorio && dadosAluno.OrgaoEmissorDocumento.IsNullOrEmptyOrWhiteSpace()) ||
                            (rgObrigatorio && dadosAluno.EstadoDocumento.IsNullOrEmptyOrWhiteSpace()) ||
                            (dadosAluno.NomeMae.IsNullOrEmptyOrWhiteSpace() && dadosAluno.NomePai.IsNullOrEmptyOrWhiteSpace())
                            )
                        {
                            throw new Exception("Existem dados obrigatórios do Aluno que não foram preenchidos.<BR>( DATA DE NASCIMENTO, NACIONALIDADE, NATURALIDADE, NOME DO PAI E/OU NOME DA MÃE.)");
                        }

                        heDto.NomeAluno = dadosAluno.NomeAluno.Trim().ToUpper();
                        heDto.DataNascimento = Convert.ToDateTime(dadosAluno.DataNascimento.ToString("dd/MM/yyyy"));
                        heDto.Nacionalidade = dadosAluno.Nacionalidade.ToString().ToUpper();
                        //heDto.Naturalidade = dadosAluno.Naturalidade.ToString().ToUpper();
                        heDto.Naturalidade = naturalidadeResolvida.ToUpper();
                        heDto.FiliacaoPai = dadosAluno.NomePai.ToString().ToUpper();
                        heDto.FiliacaoMae = dadosAluno.NomeMae.ToString().ToUpper();
                        heDto.MatriculaAluno = dadosAluno.AlunoMatricula.ToString();
                        heDto.Pessoa = dadosAluno.Pessoa.ToString();
                        heDto.UsuarioID = User.Identity.Name;


                        //dados que serão exibidos no médio e no profissionalizante
                        if (tipoConclusaoID != 1)
                        {
                            heDto.Num_rg = ((dadosAluno.NumeroDocumento == null || dadosAluno.NumeroDocumento == "") ? "nao informado" : dadosAluno.NumeroDocumento);
                            heDto.DataExpedicao = ((dadosAluno.DataExpedicaoDocumento == null || dadosAluno.DataExpedicaoDocumento.ToString() == "") ? "não informado" : dadosAluno.DataExpedicaoDocumento.Value.ToString("dd/MM/yyyy"));
                            heDto.OrgaoExpedidor = ((dadosAluno.OrgaoEmissorDocumento == null || dadosAluno.OrgaoEmissorDocumento == "") ? "não informado" : dadosAluno.OrgaoEmissorDocumento);
                            heDto.EstadoDocumento = ((dadosAluno.EstadoDocumento == null || dadosAluno.EstadoDocumento == "") ? "não informado" : dadosAluno.EstadoDocumento);
                        }

                        string nomeArquivo = string.Empty;
                        nomeArquivo = this.nomeArquivoHistoricoEscolarPor(tipoConclusaoID, pessoaID);

                        //Retorna a observação deste aluno, caso tenha.
                        docCert = rnHistorico.retornaObservacaoHistorico(heDto.MatriculaAluno, heDto.Pessoa, tipoConclusaoID);

                        docCert.TipoConclusaoId = tipoConclusaoID; //1-Fudamental 2-Médio 3-Profissionalizante
                        docCert.DocumentoId = 1; //1-histórico escolar
                        docCert.Aluno = dadosAluno.AlunoMatricula.ToString();
                        docCert.Livro = null;
                        docCert.Numero = null;
                        docCert.Folhas = null;
                        docCert.Observacao = (!obsHistorico.ToString().Trim().ToUpper().IsNullOrEmptyOrWhiteSpace() ? obsHistorico.ToString().Trim().ToUpper() : null);
                        docCert.UsuarioId = User.Identity.Name;
                        docCert.Eixo = null;
                        docCert.Pessoa = Convert.ToDecimal(dadosAluno.Pessoa.ToString());

                        //Recuperar o censo e a UA
                        #region Recupera censo e UA
                        RN.Certificacao.DTOs.HistoricoEscolarDTO heEscola = new HistoricoEscolarDTO();
                        heEscola = rnHistorico.obterDadosDaEscolaPor(heDto.Pessoa, tipoConclusaoID);

                        //Adcionado por Francisco em 15/01/2020
                        //Se algumas das informações abaixo for nula,vazia ou em branco o HISTÓRICO não poderá ser gerado. 
                        if (string.IsNullOrEmpty(heEscola.Modalidadenivel) || string.IsNullOrEmpty(heEscola.Ua) || string.IsNullOrEmpty(heEscola.Unidade_ens) || string.IsNullOrEmpty(heEscola.escola) || string.IsNullOrEmpty(heEscola.Endereco) || string.IsNullOrEmpty(heEscola.Municipio) || string.IsNullOrEmpty(heEscola.Cep) || string.IsNullOrEmpty(heEscola.DataDO.ToString()) || string.IsNullOrEmpty(heEscola.Decreto))
                        {

                            throw new Exception("Existem dados obrigatórios da Unidade de Ensino que não foram preenchidos.<BR>(UNIDADE ADMINISTRATIVA, DADOS DO ENDEREÇO OU ATO DE CRIAÇÃO DA UNIDADE)");

                        }

                        //FIM

                        string ua = string.Empty;
                        string unidade_Ens = string.Empty;

                        ua = heEscola.Ua.Trim();
                        unidade_Ens = heEscola.Unidade_ens.Trim();
                        #endregion Recupera censo e UA


                        #region DocCert

                        if (docCert.DocumentoCertId != 0) //realiza a atualização
                        {
                            //Atualiza dados Histórico Escolar na tabela DocumentoCertificacao
                            retorno = rnHistorico.AtualizaDadosHistoricoEscolar(docCert);
                            bool existe = false;

                            switch (tipoConclusaoID)
                            {
                                //Ensino Fundamental
                                case 1: if (retorno)
                                    {
                                        docGerado.NomeArquivo = nomeArquivo;
                                        docGerado.ChaveArquivo = null;
                                        docGerado.Arquivo = null;
                                        docGerado.TipoArquivo = null;
                                        docGerado.NUMEROGERADO = ua.ToString() + "/" + unidade_Ens.ToString() + "/" + DateTime.Now.Year.ToString() + " ";
                                        docGerado.UsuarioId = User.Identity.Name;
                                        docGerado.DOCUMENTOCERTID = docCert.DocumentoCertId;

                                        existe = rnHistorico.verificaSeHistoricoEscolarExisteNoDocGeradoPor(docCert.DocumentoCertId);

                                        if (existe)
                                            rnDocGerado.Atualizar(docGerado);
                                        else
                                            rnDocGerado.Insere(docGerado);
                                    }
                                    break;

                                //Ensino Médio
                                case 2: if (retorno)
                                    {
                                        docGerado.NomeArquivo = nomeArquivo;
                                        docGerado.ChaveArquivo = null;
                                        docGerado.Arquivo = null;
                                        docGerado.TipoArquivo = null;
                                        docGerado.NUMEROGERADO = ua.ToString() + "/" + unidade_Ens.ToString() + "/" + DateTime.Now.ToString() + " ";
                                        docGerado.UsuarioId = User.Identity.Name;
                                        docGerado.DOCUMENTOCERTID = docCert.DocumentoCertId;

                                        existe = rnHistorico.verificaSeHistoricoEscolarExisteNoDocGeradoPor(docCert.DocumentoCertId);

                                        if (existe)
                                            rnDocGerado.Atualizar(docGerado);
                                        else
                                            rnDocGerado.Insere(docGerado);

                                    }
                                    break;

                                //Ensino Profissionalizante
                                case 3: if (retorno)
                                    {
                                        docGerado.NomeArquivo = nomeArquivo;
                                        docGerado.ChaveArquivo = null;
                                        docGerado.Arquivo = null;
                                        docGerado.TipoArquivo = null;
                                        docGerado.NUMEROGERADO = ua.ToString() + "/" + unidade_Ens.ToString() + "/" + DateTime.Now.ToString() + " ";
                                        docGerado.UsuarioId = User.Identity.Name;
                                        docGerado.DOCUMENTOCERTID = docCert.DocumentoCertId;

                                        existe = rnHistorico.verificaSeHistoricoEscolarExisteNoDocGeradoPor(docCert.DocumentoCertId);

                                        if (existe)
                                            rnDocGerado.Atualizar(docGerado);
                                        else
                                            rnDocGerado.Insere(docGerado);
                                    }
                                    break;

                                default: Console.WriteLine("Tipo inexistente");
                                    break;
                            }

                        }
                        else if (docCert.DocumentoCertId == 0) // realiza uma nova inserção
                        {
                            retorno = rnHistorico.InsereDadosHistoricoEscolarPor(docCert);

                            switch (tipoConclusaoID)
                            {
                                //Ensino Fundamental
                                case 1: if (retorno)
                                    {
                                        docGerado.NomeArquivo = nomeArquivo;
                                        docGerado.ChaveArquivo = null;
                                        docGerado.Arquivo = null;
                                        docGerado.TipoArquivo = null;
                                        docGerado.NUMEROGERADO = ua.ToString() + "/" + unidade_Ens.ToString() + "/" + DateTime.Now.ToString() + " ";
                                        docGerado.UsuarioId = User.Identity.Name;
                                        docGerado.DOCUMENTOCERTID = docCert.DocumentoCertId;
                                        rnDocGerado.Insere(docGerado);
                                    }
                                    break;

                                //Ensino Médio
                                case 2: if (retorno)
                                    {
                                        docGerado.NomeArquivo = nomeArquivo;
                                        docGerado.ChaveArquivo = null;
                                        docGerado.Arquivo = null;
                                        docGerado.TipoArquivo = null;
                                        docGerado.NUMEROGERADO = ua.ToString() + "/" + unidade_Ens.ToString() + "/" + DateTime.Now.ToString() + " ";
                                        docGerado.UsuarioId = User.Identity.Name;
                                        docGerado.DOCUMENTOCERTID = docCert.DocumentoCertId;
                                        rnDocGerado.Insere(docGerado);
                                    }
                                    break;

                                //Ensino Profissionalizante
                                case 3: if (retorno)
                                    {
                                        docGerado.NomeArquivo = nomeArquivo;
                                        docGerado.ChaveArquivo = null;
                                        docGerado.Arquivo = null;
                                        docGerado.TipoArquivo = null;
                                        docGerado.NUMEROGERADO = ua.ToString() + "/" + unidade_Ens.ToString() + "/" + DateTime.Now.ToString() + " ";
                                        docGerado.UsuarioId = User.Identity.Name;
                                        docGerado.DOCUMENTOCERTID = docCert.DocumentoCertId;
                                        rnDocGerado.Insere(docGerado);
                                    }
                                    break;

                                default:
                                    Console.WriteLine("Tipo inexistente");
                                    break;
                            }
                        }
                        #endregion DocCert


                        #region montagem dos históricos escolares
                        switch (tipoConclusaoID)
                        {
                            case 1: // Ensino Fundamental
                                montarHistoricoEscolar(heDto, tipoConclusaoID, nomeArquivo, obsHistorico);
                                break;

                            case 2: //Ensino Médio
                                montarHistoricoEscolar(heDto, tipoConclusaoID, nomeArquivo, obsHistorico);
                                break;

                            case 3: // Profissionalizante
                                montarHistoricoEscolar(heDto, tipoConclusaoID, nomeArquivo, obsHistorico);
                                break;

                            default:
                                Console.WriteLine("Tipo inexistente");
                                break;
                        }
                        #endregion montagem dos históricos escolares

                    }
                    else
                    {
                        lblMensagem.Text = "Tipo de nível inválido ...";
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                    // throw new Exception(ex.Message);
                }
                #endregion
            }
            else
            {
                pnDadosAluno.Visible = false;
                btnGerarHistorico.Visible = false;
                btnCancelar.Visible = false;
                txtObservacao.Text = string.Empty;
                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }




    }
}
