using System;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ParalisaMatriculaFacil.aspx"), 
    ControlText("ParalisaMatriculaFacil"),
    Title("Paralisa Matricula Fácil")]
    public partial class ParalisaMatriculaFacil : TPage
    {
        public object Listar(object ano, object periodo, object curso)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();

            if ((ano != null
                    && ano.ToString() != "Selecione")
                && (periodo != null
                    && periodo.ToString() != "Selecione")
                && curso != null
                    && curso.ToString() != string.Empty)
            {
                return rnControleVaga.ListaParticipaMatriculaFacil(Convert.ToInt32(ano), Convert.ToInt32(periodo), Convert.ToString(curso));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdCurso, "* Listados apenas opções com marcação de Participa Matrícula Fácil");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (!this.IsPostBack)
            {
                this.ddlAno.DataSource = PeriodoLetivo.ListarAnos();
                this.ddlAno.Items.Insert(0, "Selecione");
                this.ddlAno.DataBind();
            }
        }

        protected void btnRetiraParalisar_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.ControleVaga rnControleVaga = new ControleVaga();

            try
            {
                int ano = ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt16(ddlAno.SelectedValue);
                int periodo = ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt16(ddlPeriodo.SelectedValue);
                string curso = (tseCurso.DBValue.IsNull || !tseCurso.IsValidDBValue) ? string.Empty : tseCurso.DBValue.ToString();
                string usuarioId = User.Identity.Name;

                validacao = rnControleVaga.ValidaRetiraParalisacao(ano, periodo, curso, usuarioId);

                if (validacao.Valido)
                {
                    rnControleVaga.RetiraParalisa(ano, periodo, curso, usuarioId);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                grdCurso.DataBind();
                CarregaQuantitativos();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnParalisar_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.ControleVaga rnControleVaga = new ControleVaga();

            try
            {
                int ano = ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt16(ddlAno.SelectedValue);
                int periodo = ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt16(ddlPeriodo.SelectedValue);
                string curso = (tseCurso.DBValue.IsNull || !tseCurso.IsValidDBValue) ? string.Empty : tseCurso.DBValue.ToString();
                int motivoRetiradaFila = ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlMotivo.SelectedValue);
                string usuarioId = User.Identity.Name;

                validacao = rnControleVaga.ValidaParalisacao(ano, periodo, curso, motivoRetiradaFila, usuarioId);

                if (validacao.Valido)
                {
                    rnControleVaga.Paralisa(ano, periodo, curso, motivoRetiradaFila, usuarioId);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                grdCurso.DataBind();
                CarregaQuantitativos();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.pnGrid.Visible = false;
                LimpaQuantitativos();

                if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace()
                    || tseCurso.DBValue.IsNull || !tseCurso.IsValidDBValue)
                {
                    lblMensagem.Text = "Para realizar a busca é necessário informar todos os campos.";
                }
                else
                {
                    CarregaQuantitativos();
                    this.pnGrid.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimpaQuantitativos()
        {
            lblQtdeCursos.Text = string.Empty;
            lblQtdeParticipaMatricula.Text = string.Empty;
            lblQtdeParalisado.Text = string.Empty;
            lblQtdeNãoParalisados.Text = string.Empty;
            lblFila.Text = string.Empty;
            odsControle.Select();
            odsControle.DataBind();
            grdCurso.DataBind();
        }

        public void Update(object MUNICIPIO_NOME, object CENSO, object ESCOLA, object MODALIDADE, object SEGMENTO, object NOME_CURSO, object SERIE, object NOME_TURNO, object VAGAS_CONTINUIDADE, object VAGAS_NOVAS, object VAGAS_LIBERADAS, object VAGAS_UTILIZADAS, object VAGAPLANEJADA, object PARTICIPAMATRICULAFACIL, object FILAESPERA, object OFERECEVAGAFASE1, object PARALISAMATRICULAFACIL, object VISUALIZAVAGA, object DT_ALTERACAO, object MATRICULA, object ID_CONTROLE_VAGA) { }

        protected void CarregaQuantitativos()
        {
            try
            {
                int ano = Convert.ToInt32(ddlAno.SelectedValue);
                int periodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
                string curso = Convert.ToString(tseCurso.DBValue);

                RN.ControleVaga rnControleVaga = new ControleVaga();
                DataTable resultato = rnControleVaga.QuantitativosParticipaMatriculaFacil(ano, periodo, curso);

                if (resultato.Rows.Count > 0)
                {
                    lblQtdeCursos.Text = resultato.Rows[0]["QUANTIDADE"].ToString();
                    lblQtdeParticipaMatricula.Text = resultato.Rows[0]["PARTICIPAMATRICULAFACIL"].ToString();
                    lblQtdeParalisado.Text = resultato.Rows[0]["PARALISADA"].ToString();
                    lblQtdeNãoParalisados.Text = resultato.Rows[0]["NAOPARALISADA"].ToString();
                    lblFila.Text = resultato.Rows[0]["FILA"].ToString();
                }
                else
                {
                    lblQtdeCursos.Text = "0";
                    lblQtdeParticipaMatricula.Text = "0";
                    lblQtdeParalisado.Text = "0";
                    lblQtdeNãoParalisados.Text = "0";
                    lblFila.Text = "0";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlPeriodo.Items.Clear();
            tseCurso.ResetValue();
            pnGrid.Visible = false;
            LimpaQuantitativos();

            if (this.ddlAno.SelectedValue != "Selecione" && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                this.ddlPeriodo.DataSource = PeriodoLetivo.ListarPeriodo(this.ddlAno.SelectedValue);
                this.ddlPeriodo.Items.Insert(0, "Selecione");
                this.ddlPeriodo.DataBind();
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            tseCurso.ResetValue();
            pnGrid.Visible = false;
            LimpaQuantitativos();
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            pnGrid.Visible = false;
            LimpaQuantitativos();
        }

        protected void grdCurso_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VAGAS_DISPONIVEIS")
            {
                var vagasLiberadas = e.GetListSourceFieldValue("VAGAS_LIBERADAS");
                var vagasUtilizadas = e.GetListSourceFieldValue("VAGAS_UTILIZADAS");

                e.Value = Convert.ToInt32(vagasLiberadas) - Convert.ToInt32(vagasUtilizadas);
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (pnGrid.Visible)
            {
                CarregaQuantitativos();
            }

            ControlaAcesso(btnParalisar, AcaoControle.editar);
            ControlaAcesso(btnRetiraParalisar, AcaoControle.editar);
            ControlaAcesso(grdCurso);
        }

        protected void grdCurso_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            int motivoRetiradaFila = ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(ddlMotivo.SelectedValue);

            int id = Convert.ToInt32(e.Keys["ID_CONTROLE_VAGA"]);
            string usuarioId = User.Identity.Name;
            bool paralisada = (e.NewValues["PARALISAMATRICULAFACIL"] == null || Convert.ToBoolean(e.NewValues["PARALISAMATRICULAFACIL"]) == false) ? false : true;

            if (paralisada && motivoRetiradaFila == 0)
            {
                e.Cancel = true;
                throw new Exception("Campo ALTERA SITUAÇÃO é obrigatório.");
            }
            else
            {
                rnControleVaga.Paralisa(id, paralisada, motivoRetiradaFila, usuarioId);
                grdCurso.DataBind();
                btnBuscar_Click(null, null);
            }
        }

        protected void grdCurso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            this.ControlaAcesso(this.grdCurso);
        }
    }
}
