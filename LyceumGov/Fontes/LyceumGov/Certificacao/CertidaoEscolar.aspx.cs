using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Techne.Lyceum.RN.Entidades;
using Image = System.Drawing.Image;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;

using iTextSharp.text.pdf;
using System.Net;
using Techne.Controls;
using System.Configuration;
using Techne.Web;


namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/CertidaoEscolar.aspx"),
 ControlText("Certidão Escolar"),
 Title("Certidão Escolar"),]
    public partial class CertidaoEscolar : TPage
    {
        RN.Certificacao.CertidaoEscolar rnCertidaoEscolar = new Techne.Lyceum.RN.Certificacao.CertidaoEscolar();
        RN.Certificacao.DocumentoGerado rnDocumentoGerado = new Techne.Lyceum.RN.Certificacao.DocumentoGerado();
        RN.Certificacao.DocumentoCertificacao rnDocumentoCertificao = new Techne.Lyceum.RN.Certificacao.DocumentoCertificacao();
        RN.Certificacao.CertificadoEscolar rnCertificadoEscolar = new Techne.Lyceum.RN.Certificacao.CertificadoEscolar();
        RN.Certificacao.Entidades.DocumentoGerado dadosDocumentoGerado = new Techne.Lyceum.RN.Certificacao.Entidades.DocumentoGerado();

        Boolean AutenticaCertificado = Convert.ToBoolean(ConfigurationManager.AppSettings["AutenticaCertificado"] ?? "false");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    this.ListarAnos();
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
            TituloGrid(grdMeusAlunos, "Alunos");
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnGerar);
        }

        public void ListarAnos()
        {
            ddlAnoEscolar.DataSource = rnCertidaoEscolar.ListarAnos();
            ddlAnoEscolar.DataBind();
            ddlAnoEscolar.Items.Insert(0, new ListItem("Selecione", "-1"));

        }

        public void ListarTurmas()
        {
            if ((!this.tseUnidadeEnsino.DBValue.IsNull) & (!this.tseCurso.DBValue.IsNull) & (ddlAnoEscolar.SelectedValue != "-1"))
            {
                if ((this.tseUnidadeEnsino.IsValidDBValue) & (this.tseCurso.IsValidDBValue))
                {

                    ddlTurma.DataSource = rnCertidaoEscolar.ListarTurmas(Convert.ToInt32(ddlAnoEscolar.SelectedValue), tseCurso.Value.ToString(), tseUnidadeEnsino.Value.ToString());
                    ddlTurma.DataBind();
                    ddlTurma.Items.Insert(0, new ListItem("Selecione", "-1"));
                }

            }

        }


        protected void tseUnidadeEnsino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            tseCurso.ResetValue();
            ddlAnoEscolar.ClearSelection();
            ddlTurma.ClearSelection();
            ddlTurma.Items.Clear();
            ListarTurmas();


        }
        protected void tseCurso2_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            ddlAnoEscolar.ClearSelection();
            ddlTurma.ClearSelection();
            ddlTurma.Items.Clear();
            ListarTurmas();

        }
        protected void ddlAnoEscolar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (ddlAnoEscolar.SelectedValue != "-1")
            ListarTurmas();

        }





        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            listarAlunos();

        }

        protected void listarAlunos()
        {
            try
            {

                int ano = Convert.ToInt32(ddlAnoEscolar.SelectedValue);
                string curso = tseCurso.Value.ToString();
                string unidade_ens = tseUnidadeEnsino.Value.ToString();
                string turma = ddlTurma.SelectedValue;


                if (ano != -1 & !string.IsNullOrEmpty(curso.ToString()) & !string.IsNullOrEmpty(unidade_ens.ToString()) & turma != "-1")
                {

                    var dt = rnCertidaoEscolar.ListarAlunosCertidaoEscolar(ano, curso.ToString(), unidade_ens.ToString(), turma);
                    if (dt.Rows.Count > 0)
                    {
                        grdMeusAlunos.DataSource = dt;
                        grdMeusAlunos.DataBind();
                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        lblMensagem.Text = "Nenhum aluno encontrado.";
                        grdMeusAlunos.DataSource = dt;
                        grdMeusAlunos.DataBind();
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                    }

                }
                else
                {
                    lblMensagem.Text = "É necessário preencher todos os campos para efetuar a consulta.";
                }

            }
            catch (Exception)
            {

                lblMensagem.Text = "Erro ao obter alunos.";
            }
        }

        private bool PodeGerarCertificadoSemAssinaturaDigital()
        {
            bool retorno = false;


            try
            {
                // busca informação do Web Config
                //sim, pode gerar certificado sem assinatura
                //Não, precisa de Token para gerar certificado com assinatura

                if (AutenticaCertificado)
                {
                    retorno = true;//Não precisa de autenticação

                }
                //else
                //{
                //    //busca o certificado
                //    if (rnCertificadoEscolar.ValidarCertificado())//Busca certificado localmente na máquina do usuário no usuário local
                //    {
                //        retorno = true;//achou
                //    }
                //    else
                //    {
                //        retorno = false;//não achou
                //    }
                //}
            }
            catch (Exception)
            {

                retorno = false;
            }

            return retorno;
        }

        protected void btnGerar_Click(object sender, EventArgs e)
        {

            // decimal pessoaCorreta = 0;
            List<decimal> pessoasParaRemover = new List<decimal>();

            List<int> selecionados = new List<int>();
            List<RN.Certificacao.infAluno> AlunosSelecionados = new List<Techne.Lyceum.RN.Certificacao.infAluno>();
            string matriculaCorreta = string.Empty;
            string nomeAluno = string.Empty;
            string tpConclusao = string.Empty;
            string usuario = User.Identity.Name;
            ExportaPdf pdf = new ExportaPdf();
            string html = string.Empty;
            string erro = string.Empty;
            byte[] Pdf_Assinado = null;
            byte[] Pdf_nao_Assinado = null;

            bool liberado = false;


            int tipoconclusao = 0;



            try
            {
                if (PodeGerarCertificadoSemAssinaturaDigital())
                { 
                    //pode gerar sem problemas
                    liberado = true;
                
                }
                else 
                {// precisa do certificado


                    //busca o certificado
                    if (rnCertificadoEscolar.ValidarCertificado())//Busca certificado localmente na máquina do usuário no usuário local
                    {
                        liberado = true;//achou, pode gerar
                    }
                    else
                    {
                        liberado = false;//não achou, deve parar o processo
                    }


                }





                if (liberado)
                {
# region PODE GERAR SEM ASSINATURA

                    for (int index = 0; index < grdMeusAlunos.VisibleRowCount; index++)

                    #region Busca Alunos Selecionados
                   
                    {
                        GridViewDataColumn col = (GridViewDataColumn)grdMeusAlunos.Columns["Selecionar"];
                        Control control = grdMeusAlunos.FindRowCellTemplateControl(index, col, "ckUtilizar");

                        // pegar o valor de tipo de conclusão

                        tipoconclusao = Convert.ToInt32(grdMeusAlunos.GetRowValues(0, "TIPOCONCLUSAOID").ToString());

                        if ((control is CheckBox))
                        {
                            CheckBox cb = ((CheckBox)control);
                            if (cb != null)
                            {
                                if (cb.Checked)
                                {
                                    selecionados.Add(index);

                                    matriculaCorreta = Convert.ToString(grdMeusAlunos.GetRowValues(index, "ALUNO"));
                                    nomeAluno = Convert.ToString(grdMeusAlunos.GetRowValues(index, "NOME_COMPL"));
                                    tpConclusao = Convert.ToString(grdMeusAlunos.GetRowValues(index, "TIPO_CONCLUSAO"));
                                    RN.Certificacao.infAluno aluno = new Techne.Lyceum.RN.Certificacao.infAluno();

                                    aluno.nome = nomeAluno;
                                    aluno.matricula = matriculaCorreta;
                                    aluno.tpConclusao = tpConclusao;
                                    AlunosSelecionados.Add(aluno);

                                }
                            }
                        }
                    }

                    #endregion

                    if (AlunosSelecionados.Count == 0)
                    {
                        lblMensagem.Text = "Selecione pelo menos um aluno.";
                    }
                    else
                    {
                        #region Processo de Gerar pdf

                        //Carrega o css que formata o html do PDF
                        string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("Css/CertificadoEscolar.css"));


                        //para cada aluno selecionado, gera um pdf assinado
                        foreach (var item in AlunosSelecionados)
                        {

                            string certificado = rnCertificadoEscolar.GerarCertificadoDiplomaEscolarCertidao(item.matricula, tipoconclusao, usuario, RN.Certificacao.TipoDocumento.CERTIDAO);

                            //nesse ponto o html está pronto

                            //se o certificado for gerado com erro anota o aluno e o erro
                            if (certificado.Substring(2, 6) != "<html>")
                            {
                                //erro += item.nome + " - " + item.tpConclusao + " - Erro encontrado: " + certificado + "<br>";
                                erro += "Erro ao gerar arquivo. Aluno: " + item.nome + " - Erro encontrado: " + certificado + "<br>";
                            }
                            //O html foi gerado com sucesso

                            else
                            {
                                //1ºpasso - Gera o pdf NÃO ASSINADO apartir do html e converte ele em byte[]

                                Pdf_nao_Assinado = pdf.gerapdfstream(certificado, false, cssText);


                                #region Assina Documento

                                if (!PodeGerarCertificadoSemAssinaturaDigital())
                                {
                                    //sim, pode gerar certificado sem assinatura
                                    //Não, precisa de Token para gerar certificado com assinatura

                                    //ASSINATUTA OCORRE AQUI

                                    //2ºpasso - Pega o código Thumbprint do certificado

                                    string tumb = RN.Certificacao.AssinaturaDigitalA3.BuscarCertificado().Thumbprint;

                                    //3ºpasso - Chama o método que irá assinar o documento  
                                    Pdf_Assinado = new RN.Certificacao.AssinaturaDigitalA3(tumb).ApplySignatureToPdf(Pdf_nao_Assinado);
                                }
                                #endregion

                                //4ºpasso - cria o arquivo localmente apartir do array assinado
                                //  File.WriteAllBytes(@"c:\fco\" + "Certificado_Em_Lote_" + DateTime.Now.ToShortDateString().Replace("/", "_") + "-" + item.matricula + ".pdf", signedArrPdf);


                                // nesse momento preciso transformar esse array para salvar no banco..

                                string nometipoconclusao = RN.Certificacao.TipoConclusao_.RetornatipoConclusao(tipoconclusao);

                                //busca as informações do documento certificação

                                #region Documento Gerado

                                var dtdocumento = rnDocumentoCertificao.Listar(item.matricula, tipoconclusao, RN.Certificacao.TipoDocumento.CERTIDAO);

                                var documentoCertID = dtdocumento.Rows[0]["DOCUMENTOCERTID"].ToString();


                                //pega os dados do documento gerado

                                //(DOCUMENTOGERADOID, DOCUMENTOCERTID,NUMEROGERADO,NOMEARQUIVO,USUARIOID,DATACADASTRO)

                                DataTable documentoGerado = rnDocumentoGerado.Listar(Int32.Parse(documentoCertID));


                                dadosDocumentoGerado.DocumentoGeradoID = Convert.ToInt32(documentoGerado.Rows[0]["DOCUMENTOGERADOID"].ToString());

                                dadosDocumentoGerado.DOCUMENTOCERTID = Convert.ToInt32(documentoCertID);
                                dadosDocumentoGerado.NUMEROGERADO = documentoGerado.Rows[0]["NUMEROGERADO"].ToString();
                                dadosDocumentoGerado.ChaveArquivo = documentoGerado.Rows[0]["ChaveArquivo"].ToString();

                               // dadosDocumentoGerado.NomeArquivo = "CERTIDAO__ESCOLAR_" + DateTime.Now.ToShortDateString().Replace("/", "_") + "-" + item.matricula + "_" + nometipoconclusao + ".pdf";
                                dadosDocumentoGerado.NomeArquivo = "CERTIDAO_ESCOLAR_" + item.matricula + "_" + DateTime.Now.ToShortDateString().Replace("/", "_") + "-" + nometipoconclusao + ".pdf";
                                dadosDocumentoGerado.TipoArquivo = "application/pdf";



                                if (PodeGerarCertificadoSemAssinaturaDigital())
                                {
                                    //aqui contém o arquivo não assinado
                                    dadosDocumentoGerado.Arquivo = Pdf_nao_Assinado;
                                }
                                else
                                { //aqui contém o arquivo assinado
                                    dadosDocumentoGerado.Arquivo = Pdf_Assinado;
                                }

                                dadosDocumentoGerado.UsuarioId = usuario;

                                dadosDocumentoGerado.DataCadastro = Convert.ToDateTime(documentoGerado.Rows[0]["DATACADASTRO"].ToString());
                                #endregion



                                //aqui farei o insert no documento gerado

                                bool atualizado = rnDocumentoGerado.Atualizar(dadosDocumentoGerado);

                                if (!atualizado)
                                {
                                    erro += " - Erro ao salvar/atualizar arquivo. Aluno: " + item.nome + " <br> ";
                                    //  lblMensagem.Text += item.nome + " - " + item.tpConclusao + " - Erro encontrado: Erro ao salvar/atualizar arquivo.<br> ";
                                }
                                else
                                {
                                    pdf.gerapdfStreamDownload(certificado, dadosDocumentoGerado.NomeArquivo, false, cssText);

                                    erro += " - Arquivo gerado/salvo com sucesso. Aluno: " + item.nome + " <br> ";
                                    //  lblMensagem.Text += item.nome + " - " + item.tpConclusao + ;

                                }

                            }

                        }

                        #endregion

                        lblMensagem.Text = erro;
                    }
               # endregion 
                }


                else
                {
                    lblMensagem.Text = "CERTIFICADO DIGITAL NECESSÁRIO NÃO FOI ENCONTRADO.";
                }
            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }
            _tipoOperacao = TipoOperacao.Inicial;
            ControlarTipoOperacao();

        }

       
        public byte[] ConverteStreamToByteArray(Stream stream)
        {
            byte[] byteArray = new byte[16 * 1024];
            using (MemoryStream mStream = new MemoryStream())
            {
                int bit;
                while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
                {
                    mStream.Write(byteArray, 0, bit);
                }
                return mStream.ToArray();
            }
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
            btnGerar.Visible = false;
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
                        btnGerar.Visible = false;
                        grdMeusAlunos.Visible = false;
                        break;

                    }

                case TipoOperacao.Consultar:
                    {

                        grdMeusAlunos.Visible = true;
                        Button[] controles = new Button[] { btnGerar };
                        ImageButton[] imgControles = new ImageButton[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        ControlaAcesso(btnGerar, AcaoControle.novo);
                        break;
                    }


            }
        }

        #endregion

    }
}
