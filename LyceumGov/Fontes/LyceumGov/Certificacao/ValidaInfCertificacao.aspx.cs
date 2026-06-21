using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/ValidaInfCertificacao.aspx"),
    ControlText("Validação Informações do Documento"),
    Title("Validação Informações do Documento"),]
    public partial class ValidaInfCertificacao : TPage
    {
        RN.Certificacao.ValidaInfCertificacao rnValidaInfCertificado = new Techne.Lyceum.RN.Certificacao.ValidaInfCertificacao();
        RN.Certificacao.CertidaoEscolar rnCertidaoEscolar = new Techne.Lyceum.RN.Certificacao.CertidaoEscolar();
        RN.Certificacao.CertificadoEscolar rnCertificadoEscolar = new Techne.Lyceum.RN.Certificacao.CertificadoEscolar();
        RN.Certificacao.DocumentoGerado rnDocumentoGerado = new Techne.Lyceum.RN.Certificacao.DocumentoGerado();
        RN.Certificacao.DocumentoCertificacao rnDocumentoCertificao = new Techne.Lyceum.RN.Certificacao.DocumentoCertificacao();

        private ValidacaoDados validacao;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (grdMeusAlunos.VisibleRowCount > 0)
                //    grdMeusAlunos.Visible = true;
                //else
                //    grdMeusAlunos.Visible = false;

                if (!IsPostBack)
                {
                    this.ListarAnos();
                    CarregaPeriodoLetivo(ddlSemestre, Convert.ToInt32(ddlAnoEscolar.SelectedValue));
                    lblMensagem.Text = string.Empty;
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

            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnBaixar);
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnAutorizar);
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnsegundavia);
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btBuscar);
            // ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(ddlSemestre);

        }

        #region Botoes Popup Detalhes

        protected void btnBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                string Linha = txtRow.Value;
                if (Linha == null)
                    return;

                int DOCUMENTOCERTID = int.Parse(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DOCUMENTOCERTID").ToString());
                DataTable dtdocumentoGerado = rnDocumentoGerado.Listar(DOCUMENTOCERTID);
                byte[] arquivo = rnDocumentoGerado.ObtemArquivoPor(DOCUMENTOCERTID);
                string nomearquivo = dtdocumentoGerado.Rows[0]["NOMEARQUIVO"].ToString();

                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.ContentType = "application/pdf";
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + nomearquivo + "\"");

                response.BinaryWrite(arquivo);
                response.End();

                // response.Redirect("testing.aspx", false);
            }

            catch (Exception ex)
            {
                lblautorizadomsg.Text = "Houve um erro ao preparar o certificado para download.<br>" + ex.Message;
            }
        }

        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = User.Identity.Name;
                string Linha = txtRow.Value;
                if (Linha == null)
                    return;

                int DOCUMENTOCERTID = int.Parse(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DOCUMENTOCERTID").ToString());

                if (rnDocumentoCertificao.AutorizarDocumentoCertificacao(usuario, DOCUMENTOCERTID))
                {
                    //odsAlunos.DataBind();
                    // listarAlunos();
                    //CarregaTela();
                    //btnAutorizar.Visible = false;
                    //CarregaTela();
                    var id = txtRow.Value;
                    ScriptManager.RegisterStartupScript(this, GetType(), "teste", "grdMeusAlunos.Refresh();popupItens(" + id + ");", true);
                    lblautorizadomsg.Text = "Autorização da emissão do certificado feita com sucesso.";
                }
                else
                {
                    lblautorizadomsg.Text = "Houve um erro ao autorizar emissão do certificado. ";
                }
            }
            catch (Exception ex)
            {
                lblautorizadomsg.Text = ex.Message;
            }
        }

        protected void btnsegundavia_Click(object sender, EventArgs e)
        {
            string usuario = User.Identity.Name;
            //método para apagar o registro no  documento gerado
            RN.Certificacao.infAluno aluno = new Techne.Lyceum.RN.Certificacao.infAluno();

            string Linha = txtRow.Value;
            if (Linha == null)
                return;

            aluno.matricula = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "MATRICULA").ToString();
            aluno.tpConclusao = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_CONCLUSAO_ID").ToString();
            aluno.tpDocumento = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_DOCUMENTO_ID").ToString();
            aluno.documentoCertID = Convert.ToInt32(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DOCUMENTOCERTID").ToString());

            validacao = rnValidaInfCertificado.ValidaSegundaVia(aluno);

            if (validacao.Valido)
            {
                if (rnDocumentoGerado.GeraSegundaVia(aluno, usuario))
                {
                    //listarAlunos();
                    //listarAlunos(ddlAnoEscolar.SelectedValue, tseCurso2.Value, tseUnidadeEnsino.Value, ddlSemestre.SelectedValue);
                    //CarregaTela();

                    var id = txtRow.Value;
                    ScriptManager.RegisterStartupScript(this, GetType(), "SegundaVia", "grdMeusAlunos.Refresh();popupItens(" + id + ");", true);

                    lblautorizadomsg.Text = "Segunda via solicitada com sucesso.";
                    // btnBaixar.Visible = false;
                    // btnsegundavia.Visible = false;
                }
                else
                {
                    lblautorizadomsg.Text = "Erro ao solicitar Segunda via.";
                }
            }
            else
            {
                lblautorizadomsg.Text = validacao.Mensagem;
            }
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {

            if (!IsPostBack)
                return;

            CarregaTela();

        }

        public void CarregaTela()
        {
            LimparTela();

            string Linha = txtRow.Value;
            if (Linha == null)
                return;

            //informações do ALuno

            // (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "").ToString() : "não informado");

            txtNomeAluno.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NOMEDOALUNO").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NOMEDOALUNO").ToString() : " ");

            txtmatricula.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "MATRICULA").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "MATRICULA").ToString() : " ");

            txtNomemae.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NOME_MAE").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NOME_MAE").ToString() : " ");

            txtNomepai.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NOME_PAI").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NOME_PAI").ToString() : " ");

            txtNRg.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NUMERO_DOC").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NUMERO_DOC").ToString() : " ");

            txtorgaoemissor.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "ORGAO_EMISSOR").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "ORGAO_EMISSOR").ToString() : " ");

            txtufexpedicao.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UF_EXPEDICAO").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UF_EXPEDICAO").ToString() : " ");

            txtdtnascimento.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DATANASCIMENTO").ToString()) ? ((DateTime)grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DATANASCIMENTO")).ToString("dd/MM/yyyy") : " ");

            txtMunicipioNascimento.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "MUNICIPIO_NASC").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "MUNICIPIO_NASC").ToString() : " ");

            txtUFNascimento.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UF_NASC").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UF_NASC").ToString() : " ");

            txtPaisNascimento.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "PAIS_NASC").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "PAIS_NASC").ToString() : " ");

            //informações da UE
            txtNomeue.Text = tseUnidadeEnsino["NOME_COMP"].ToString();

            txtatocriacaoue.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UE_ATO").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UE_ATO").ToString() : "");

            txtdtatocriacaoue.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UE_DT").ToString()) ? ((DateTime)grdMeusAlunos.GetRowValuesByKeyValue(Linha, "UE_DT")).ToString("dd/MM/yyyy") : "");

            //informações do curso
            txtnomecurso.Text = tseCurso2["NOME"].ToString();

            txtnumerocurso.Text = tseCurso2.Value.ToString();

            txtatodecriacaocurso.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "ATO_CURSO").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "ATO_CURSO").ToString() : "");

            txtdtAtodecriacaocurso.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DT_DO_CURSO").ToString()) ? ((DateTime)grdMeusAlunos.GetRowValuesByKeyValue(Linha, "DT_DO_CURSO")).ToString("dd/MM/yyyy") : "");

            //informações do Livro
            txttipoconclusao.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_CONCLUSAO").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_CONCLUSAO").ToString() : "");

            txtTipoDocumento.Text = (!string.IsNullOrEmpty(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_DOCUMENTO").ToString()) ? grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_DOCUMENTO").ToString() : "");

            txtLivroDocumento.Text = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "LIVRO").ToString();

            txtFolhaDocumento.Text = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "FOLHAS").ToString();

            txtNumeroDocumento.Text = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "NUMERO").ToString();

            //informações do cadastro

            string cadCompleto = string.Empty;
            cadCompleto = grdMeusAlunos.GetRowValuesByKeyValue(Linha, "CADASTRO_COMPLETO").ToString().ToUpper();
            lblcadastroCompleto.Text += cadCompleto;

            Boolean autorizado;
            autorizado = Convert.ToBoolean(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "AUTORIZADO"));

            if (cadCompleto.ToUpper() == "NÃO")
            {
                btnAutorizar.Visible = false;
                btnBaixar.Visible = false;
                btnsegundavia.Visible = false;
                lblautorizado.Visible = false;
                lblmsgautorizado.Visible = true;
            }
            else
            {
                #region Autorizar

                if (autorizado)
                {
                    lblautorizado.Text += "SIM";
                    btnAutorizar.Visible = false;
                    lblmsgautorizado.Visible = false;
                }
                else
                {
                    lblautorizado.Text += "NÃO";
                    btnAutorizar.Visible = true;
                    ControlaAcesso(btnAutorizar, AcaoControle.editar);
                }

                #endregion

                #region Download /2ª via

                if ((grdMeusAlunos.GetRowValuesByKeyValue(Linha, "ARQUIVO").ToString() == "S"))
                {
                    btnBaixar.Visible = true;
                    ControlaAcesso(btnBaixar, AcaoControle.editar);
                    btnsegundavia.Visible = true;
                    ControlaAcesso(btnsegundavia, AcaoControle.novo);
                }
                else
                {
                    btnBaixar.Visible = false;
                    btnsegundavia.Visible = false;
                }

                #endregion
            }

            int TIPO_DOCUMENTO_ID = Convert.ToInt32(grdMeusAlunos.GetRowValuesByKeyValue(Linha, "TIPO_DOCUMENTO_ID").ToString().ToUpper());

            //tipo Documento
            //1		Histórico Escolar
            //2		Certidão
            //3		Certificado Escolar
            //4		Diploma

            //retira a visibilidade de acordo com o tipo do documento.
            switch (TIPO_DOCUMENTO_ID)
            {
                case 1: //1	Histórico Escolar

                    pnlivro.Visible = false;
                    pnlLiberacao.Visible = false;
                    break;

                case 2:  //2 Certidão

                    pnlivro.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "pnl", " $(\"#<%= pnlLiberacao.ClientID %>\").css(\"display\", \"block\"); ", true);
                    pnlLiberacao.Visible = true;
                    break;

                case 3: //3	Certificado Escolar
                case 4: //4	Diploma
                    btnBaixar.Visible = false;
                    btnsegundavia.Visible = false;
                    break;

                default:
                    break;
            }

            pucItemHistorico.ShowOnPageLoad = true;
            puccItemHistorico.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Overflow", " scroll()", true);
        }

        public void LimparTela()
        {
            txtNomeAluno.Text = string.Empty;
            txtmatricula.Text = string.Empty;
            txtNomemae.Text = string.Empty;
            txtNomepai.Text = string.Empty;
            txtNRg.Text = string.Empty;
            txtorgaoemissor.Text = string.Empty;
            txtufexpedicao.Text = string.Empty;
            txtdtnascimento.Text = string.Empty;
            txtMunicipioNascimento.Text = string.Empty;
            txtUFNascimento.Text = string.Empty;

            //informações da UE
            txtNomeue.Text = string.Empty;
            txtatocriacaoue.Text = string.Empty;
            txtdtatocriacaoue.Text = string.Empty;

            //informações do curso
            txtnomecurso.Text = string.Empty;
            txtnumerocurso.Text = string.Empty;
            txtatodecriacaocurso.Text = string.Empty;
            txtdtAtodecriacaocurso.Text = string.Empty;

            //informações do Livro
            txttipoconclusao.Text = string.Empty;
            txtTipoDocumento.Text = string.Empty;
            txtLivroDocumento.Text = string.Empty;
            txtFolhaDocumento.Text = string.Empty;
            txtNumeroDocumento.Text = string.Empty;

            //informações do cadastro

            lblcadastroCompleto.Text = "Cadastro Completo: ";
            lblautorizado.Text = "Emissão Autorizada: ";
            //rblAutorizado.ClearSelection();

            //informações da liberação
            btnsegundavia.Visible = false;
            btnBaixar.Visible = false;
            btnAutorizar.Visible = false;
            //  rblAutorizado.ClearSelection();

        }

        #endregion

        #region Painel de Busca

        //public void listarAlunos()
        //{
        //    string unidade_ens = tseUnidadeEnsino.Value == null ? "" : tseUnidadeEnsino.Value.ToString();
        //    string curso = tseCurso2.Value == null ? "" : tseCurso2.Value.ToString();
        //    string AnoEscolar = ddlAnoEscolar.SelectedValue;
        //    string semestre = ddlSemestre.SelectedValue == "" ? "-1" : ddlSemestre.SelectedValue.ToString();

        //    if (AnoEscolar.ToString() != "-1" & !string.IsNullOrEmpty(curso) & !string.IsNullOrEmpty(unidade_ens) & semestre != "-1")
        //    {
        //        DataTable dtAlunos = rnValidaInfCertificado.ListarAlunos(Convert.ToInt32(AnoEscolar), curso.ToString(), unidade_ens.ToString(), Convert.ToInt32(semestre));

        //        if (dtAlunos.Rows.Count > 0)
        //        {
        //            grdMeusAlunos.Visible = true;
        //            grdMeusAlunos.DataSource = dtAlunos;
        //            grdMeusAlunos.DataBind();
        //            //return dtAlunos;
        //        }
        //        else
        //        {
        //            lblMensagem.Text = "Nenhum aluno encontrado";
        //            grdMeusAlunos.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        lblMensagem.Text = "Todos os campos precisam estar preenchidos.";
        //        grdMeusAlunos.Visible = false;
        //    }
        //}

        public object listarAlunos(object AnoEscolar, object curso, object unidade_ens, object semestre)
        {

            string curso_ = curso == null ? "" : curso.ToString();
            string unidade_ens_ = unidade_ens == null ? "" : unidade_ens.ToString();
            string periodo = semestre == "" || semestre == null ? "-1" : semestre.ToString();

            if (AnoEscolar.ToString() != "-1" & !string.IsNullOrEmpty(curso_) & !string.IsNullOrEmpty(unidade_ens_) & periodo != "-1")
            {
                DataTable dtAlunos = rnValidaInfCertificado.ListarAlunos(Convert.ToInt32(AnoEscolar), curso.ToString(), unidade_ens.ToString(), Convert.ToInt32(semestre));

                if (dtAlunos.Rows.Count > 0)
                {
                    // grdMeusAlunos.Visible = true;
                    return dtAlunos;
                }
                else
                {
                    // grdMeusAlunos.Visible = false;
                    return null;
                }

            }
            return null;
        }

        protected void grdMeusAlunos_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (!grdMeusAlunos.IsEditing && !grdMeusAlunos.IsNewRowEditing)
            {
                if (e.Column.FieldName == "detalhes")
                {
                    e.Value = e.GetListSourceFieldValue("ID");
                    //e.Value = e.ListSourceRowIndex;
                }

                if (e.Column.FieldName == "CADASTRO_COMPLETO")
                {
                    //e.Value = e.GetListSourceFieldValue("CADASTRO_COMPLETO");
                    e.Value = e.ListSourceRowIndex;

                    string valor = grdMeusAlunos.GetRowValues(e.ListSourceRowIndex, "CADASTRO_COMPLETO").ToString();
                }
            }
        }

        public void ListarAnos()
        {
            ddlAnoEscolar.DataSource = rnCertidaoEscolar.ListarAnos();
            ddlAnoEscolar.DataBind();
            ddlAnoEscolar.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void ddlAnoEscolar_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaPeriodoLetivo(ddlSemestre, Convert.ToInt32(ddlAnoEscolar.SelectedValue));
        }

        private void CarregaPeriodoLetivo(DropDownList drop, int ano)
        {
            Techne.Lyceum.RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                drop.Items.Clear();

                if (ano != -1)
                {
                    drop.DataSource = rnPeriodoLetivo.ListaPeriodosletivosPor(ano);
                    drop.DataBind();
                    drop.Items.Insert(0, item);
                }
                else
                {
                    drop.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            listarAlunos(ddlAnoEscolar.SelectedValue, tseCurso2.Value, tseUnidadeEnsino.Value, ddlSemestre.SelectedValue);
            //listarAlunos();
        }

        #endregion

    }
}
