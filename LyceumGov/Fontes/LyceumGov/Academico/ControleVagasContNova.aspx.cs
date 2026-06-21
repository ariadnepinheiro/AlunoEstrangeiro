using System;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ControleVagasContNova.aspx"), ControlText("ControleVagasContNova"), Title("Controle de Vagas")]

    public partial class ControleVagasContNova : TPage
    {
        public object Listar(object unidade_ens, object ano, object periodo)
        {
            var ue = unidade_ens.ToString();

            if (ue != null
                && (ano != null
                    && ano.ToString() != "Selecione")
                && (periodo != null
                    && periodo.ToString() != "Selecione"))
            {
                return ControleVaga.Listar(ue, Convert.ToInt32(ano), Convert.ToInt32(periodo));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdControle, "Controle de Vagas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            this.ValidarCampos();

            if (!this.IsPostBack)
            {
                this.cmbAno.DataSource = PeriodoLetivo.ListarAnos();
                this.cmbAno.Items.Insert(0, "Selecione");
                this.cmbAno.DataBind();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int vagaUtilizada = 0;
                long resultado;

                if (!long.TryParse(this.txtVagasContinuidade.Text, out resultado))
                {
                    this.lblMensagem.Text = "O campo Vagas Continuidade deve ser composto por números.";

                    this.txtVagasContinuidade.Text = string.Empty;
                    this.txtVagasContinuidade.Focus();

                    return;
                }
                if (!long.TryParse(this.txtVagasNova.Text, out resultado))
                {
                    this.lblMensagem.Text = "O campo Vagas Nova deve ser composto por números.";

                    this.txtVagasNova.Text = string.Empty;
                    this.txtVagasNova.Focus();

                    return;
                }

                var liberadas = Convert.ToInt32(this.txtVagasContinuidade.Text) + Convert.ToInt32(this.txtVagasNova.Text);
                var controleVaga = new TceControleVaga
                {
                    Ano = Convert.ToInt32(this.cmbAno.SelectedValue),
                    Periodo = Convert.ToInt32(this.cmbPeriodo.SelectedValue),
                    Censo = this.tseUnidadeResponsavel.DBValue.ToString(),
                    Curso = this.cmbEscolaridade.SelectedValue,
                    Serie = Convert.ToInt32(this.cmbSerie.SelectedValue),
                    Turno = this.cmbTurno.SelectedValue == "Selecione" ? string.Empty : this.cmbTurno.SelectedValue,
                    VagasContinuidade = Convert.ToInt32(this.txtVagasContinuidade.Text),
                    VagasNovas = Convert.ToInt32(this.txtVagasNova.Text),
                    VagasLiberadas = liberadas,
                    ParticipaMatriculaFacil = chkParticipaMatriculaFacil.Checked,
                    VisualizaVaga = chkVisualizaVagas.Checked,
                    OfereceVagaFase1 = chkOfereceVagaFase1.Checked,
                    Matricula = this.User.Identity.Name,
                    IdControleVaga = string.IsNullOrEmpty(hdnIdControle.Value) ? 0 : int.Parse(hdnIdControle.Value)
                };

                var validacao = ControleVaga.Validar(controleVaga, out vagaUtilizada );

                if (validacao.Valido)
                {
                    if (controleVaga.IdControleVaga == 0)
                        ControleVaga.Inserir(controleVaga);
                    else
                        ControleVaga.Alterar(controleVaga);

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Controle de Vagas atualizado com sucesso.');", true);

                    this.cmbModalidade.ClearSelection();
                    this.cmbNivel.Items.Clear();
                    this.cmbEscolaridade.Items.Clear();
                    this.cmbSerie.Items.Clear();
                    this.cmbTurno.Items.Clear();

                    this.txtVagasContinuidade.Text = string.Empty;
                    this.txtVagasNova.Text = string.Empty;
                    this.txtVagasDisponiveis.Text = string.Empty;
                    this.txtVagasLiberadas.Text = string.Empty;
                    this.txtVagasUtilizadas.Text = string.Empty;
                    this.chkParticipaMatriculaFacil.Checked = false;
                    this.chkOfereceVagaFase1.Checked = false;
                    this.chkVisualizaVagas.Checked = false;
                    this.txtVagaPlanejada.Text = string.Empty;

                    this.odsControle.Select();
                    this.odsControle.DataBind();
                    this.grdControle.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        txtVagasLiberadas.Text = liberadas.ToString();

                        if (txtVagasUtilizadas.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            txtVagasUtilizadas.Text = "0";
                        }
                        txtVagasDisponiveis.Text = (liberadas - int.Parse(txtVagasUtilizadas.Text)).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbPeriodo.Items.Clear();
            if (this.cmbAno.SelectedValue != "Selecione" && !string.IsNullOrEmpty(cmbAno.SelectedValue))
            {
                this.cmbPeriodo.DataSource = PeriodoLetivo.ListarPeriodo(this.cmbAno.SelectedValue);
                this.cmbPeriodo.Items.Insert(0, "Selecione");
                this.cmbPeriodo.DataBind();
            }

            this.cmbModalidade.Items.Clear();
            this.cmbNivel.Items.Clear();
            this.cmbEscolaridade.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;            
            this.chkVisualizaVagas.Checked = false;
            this.txtVagaPlanejada.Text = string.Empty;
        }

        protected void cmbEscolaridade_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;            
            this.txtVagaPlanejada.Text = string.Empty;

            if (this.cmbEscolaridade.SelectedValue != "Selecione")
            {
                this.cmbSerie.DataSource = Serie.ListarSeriesPorUE(Convert.ToString(this.tseUnidadeResponsavel.DBValue), this.cmbEscolaridade.SelectedValue);
                this.cmbSerie.Items.Insert(0, "Selecione");
                this.cmbSerie.DataBind();
            }
        }

        protected void cmbModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbNivel.Items.Clear();
            this.cmbEscolaridade.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;            
            this.chkVisualizaVagas.Checked = false;
            this.txtVagaPlanejada.Text = string.Empty;

            if (this.cmbModalidade.SelectedValue != "Selecione")
            {
                this.cmbNivel.DataSource = Curso.ListarNivelPorUE(Convert.ToString(this.tseUnidadeResponsavel.DBValue));
                this.cmbNivel.Items.Insert(0, "Selecione");
                this.cmbNivel.DataBind();
            }
        }

        protected void cmbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbEscolaridade.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;
            this.chkVisualizaVagas.Checked = false;
            this.txtVagaPlanejada.Text = string.Empty;

            if ((this.cmbNivel.SelectedValue != "Selecione") && (this.cmbModalidade.SelectedValue != "Selecione"))
            {
                this.cmbEscolaridade.DataSource = Curso.ListarCursoPorUE(Convert.ToString(this.tseUnidadeResponsavel.DBValue), this.cmbModalidade.SelectedValue, this.cmbNivel.SelectedValue);
                this.cmbEscolaridade.Items.Insert(0, "Selecione");
                this.cmbEscolaridade.DataBind();
            }
        }

        protected void cmbPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbPeriodo.SelectedValue != "Selecione")
            {
                this.cmbModalidade.Items.Clear();
                this.cmbModalidade.DataSource = Curso.ListarModalidadePorUE(this.tseUnidadeResponsavel.DBValue.ToString());
                this.cmbModalidade.Items.Insert(0, "Selecione");
                this.cmbModalidade.DataBind();
            }

            this.cmbNivel.Items.Clear();
            this.cmbEscolaridade.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;
            this.chkVisualizaVagas.Checked = false;
            this.txtVagaPlanejada.Text = string.Empty;
        }

        protected void cmbSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbTurno.Items.Clear();
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.txtVagaPlanejada.Text = string.Empty;

            if (this.cmbSerie.SelectedValue != "Selecione")
            {
                this.cmbTurno.DataSource = Turno.ListarTurnosPor(Convert.ToString(this.tseUnidadeResponsavel.DBValue), this.cmbEscolaridade.SelectedValue, int.Parse(this.cmbSerie.SelectedValue), int.Parse(this.cmbAno.SelectedValue), int.Parse(this.cmbPeriodo.SelectedValue));
                this.cmbTurno.Items.Insert(0, "Selecione");
                this.cmbTurno.DataBind();
            }
        }

        protected void cmbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Matriculas.MatriculaEspecial rnMatriculaEspecial = new Techne.Lyceum.RN.Matriculas.MatriculaEspecial();
            txtVagasContinuidade.Text = string.Empty;
            txtVagasNova.Text = string.Empty;
            txtVagasDisponiveis.Text = string.Empty;
            txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;            
            this.chkVisualizaVagas.Checked = false;
            this.txtVagaPlanejada.Text = string.Empty;

            if (this.cmbTurno.SelectedValue != "Selecione")
            {                
                string curso = cmbEscolaridade.SelectedValue;
                int utilizadas = 0;

                if (curso == "9999.81" || curso == "9999.82" || curso == "9999.83" || curso == "9999.84"
                    || curso == "9999.85" || curso == "9999.86" || curso == "9999.87")
                {
                    //Busca de vagas utilizadas diferente para cursos de disciplinas da matricula especial
                    //9999.81	REGRESSEEDUC - GEOGRAFIA                   
                    //9999.82	REGRESSEEDUC - HISTÓRIA   
                    //9999.83	REGRESSEEDUC - BIOLOGIA                    
                    //9999.84	REGRESSEEDUC - FÍSICA                   
                    //9999.85	REGRESSEEDUC - QUÍMICA
                    //9999.86	REGRESSEEDUC - MATEMÁTICA
                    //9999.87	REGRESSEEDUC - LÍNGUA PORTUGUESA
                    utilizadas = rnMatriculaEspecial.RetornaQuantidadeUtilizadaPor(
                        curso, 
                        cmbTurno.SelectedValue, 
                        int.Parse(this.cmbAno.SelectedValue));
                }
                else
                {
                    utilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(
                        this.tseUnidadeResponsavel.DBValue.ToString(),
                        int.Parse(this.cmbAno.SelectedValue),
                        int.Parse(this.cmbPeriodo.SelectedValue),
                        int.Parse(this.cmbSerie.SelectedValue),
                        this.cmbEscolaridade.SelectedValue,
                        this.cmbTurno.SelectedValue);
                }

                this.txtVagasUtilizadas.Text = utilizadas.ToString();

                var dt = ControleVaga.RetornaVagasContinuidadeNova(this.tseUnidadeResponsavel.DBValue.ToString(),
                                                                   int.Parse(this.cmbAno.SelectedValue),
                                                                   int.Parse(this.cmbPeriodo.SelectedValue),
                                                                   int.Parse(this.cmbSerie.SelectedValue),
                                                                   this.cmbEscolaridade.SelectedValue,
                                                                   this.cmbTurno.SelectedValue);

                if (dt.Rows.Count > 0)
                {
                    chkParticipaMatriculaFacil.Checked = Convert.ToBoolean(dt.Rows[0]["PARTICIPAMATRICULAFACIL"]);
                    chkOfereceVagaFase1.Checked = Convert.ToBoolean(dt.Rows[0]["OFERECEVAGAFASE1"]);
                    chkVisualizaVagas.Checked = Convert.ToBoolean(dt.Rows[0]["VISUALIZAVAGA"]);
                    txtVagasContinuidade.Text = dt.Rows[0]["VAGAS_CONTINUIDADE"].ToString();
                    txtVagasNova.Text = dt.Rows[0]["VAGAS_NOVAS"].ToString();
                    txtVagasLiberadas.Text =
                        (Convert.ToInt32(dt.Rows[0]["VAGAS_CONTINUIDADE"]) + Convert.ToInt32(dt.Rows[0]["VAGAS_NOVAS"]))
                            .ToString();
                    hdnIdControle.Value = dt.Rows[0]["ID_CONTROLE_VAGA"].ToString();
                    txtVagaPlanejada.Text = dt.Rows[0]["VAGAPLANEJADA"].ToString();
                    btnSalvar.Visible = true;
                }
                else
                {
                    hdnIdControle.Value = "0";
                }

            }
        }

        protected void grdControle_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VAGAS_DISPONIVEIS")
            {
                var vagasLiberadas = e.GetListSourceFieldValue("VAGAS_LIBERADAS");
                var vagasUtilizadas = e.GetListSourceFieldValue("VAGAS_UTILIZADAS");

                e.Value = Convert.ToInt32(vagasLiberadas) - Convert.ToInt32(vagasUtilizadas);
            }
            //if (e.Column.FieldName == "VAGAS_DISPONIVEIS_CONTINUIDADE")
            //{
            //    var vagasLiberadasContinuidade = e.GetListSourceFieldValue("VAGAS_CONTINUIDADE");
            //    var vagasUtilizadasContinuidade = e.GetListSourceFieldValue("VAGAS_UTILIZADAS_CONTINUIDADE");

            //    e.Value = Convert.ToInt32(vagasLiberadasContinuidade) - Convert.ToInt32(vagasUtilizadasContinuidade);
            //}
            //if (e.Column.FieldName == "VAGAS_DISPONIVEIS_NOVAS")
            //{
            //    var vagasLiberadasNovas = e.GetListSourceFieldValue("VAGAS_NOVAS");
            //    var vagasUtilizadasNovas = e.GetListSourceFieldValue("VAGAS_UTILIZADAS_NOVAS");

            //    e.Value = Convert.ToInt32(vagasLiberadasNovas) - Convert.ToInt32(vagasUtilizadasNovas);
            //}
        }

        protected void grdControle_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["VAGAS_CONTINUIDADE"] == null)
            {
                e.RowError = "O campo Vagas Continuidade é de preenchimento obrigatório.";
            }
            if (e.NewValues["VAGAS_NOVAS"] == null)
            {
                e.RowError = "O campo Vagas Nova é de preenchimento obrigatório.";
            }
        }

        protected void odsControle_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.Matriculas.Agenda rnAgenda = new Techne.Lyceum.RN.Matriculas.Agenda();

            int vagasUtilizadas = 0;
            
            var controleVaga = new TceControleVaga
            {
                Ano = Convert.ToInt32(e.InputParameters["ANO"]),
                Periodo = Convert.ToInt32(e.InputParameters["PERIODO"]),
                Censo = Convert.ToString(e.InputParameters["CENSO"]),
                Curso = Convert.ToString(e.InputParameters["CURSO"]),
                Turno = Convert.ToString(e.InputParameters["TURNO"]),
                Serie = Convert.ToInt32(e.InputParameters["SERIE"]),
                IdControleVaga = Convert.ToInt32(e.InputParameters["ID_CONTROLE_VAGA"]),
                VagasContinuidade = int.Parse(e.InputParameters["VAGAS_CONTINUIDADE"].ToString()),
                VagasNovas = int.Parse(e.InputParameters["VAGAS_NOVAS"].ToString()),
                VagasLiberadas = int.Parse(e.InputParameters["VAGAS_CONTINUIDADE"].ToString()) + int.Parse(e.InputParameters["VAGAS_NOVAS"].ToString()),
                Matricula = this.User.Identity.Name,
                ParticipaMatriculaFacil = Convert.ToBoolean(e.InputParameters["PARTICIPAMATRICULAFACIL"]),
                VisualizaVaga = Convert.ToBoolean(e.InputParameters["VISUALIZAVAGA"]),
                OfereceVagaFase1 = Convert.ToBoolean(e.InputParameters["OFERECEVAGAFASE1"]),
                ParalisaMatriculaFacil = Convert.ToBoolean(e.InputParameters["PARALISAMATRICULAFACIL"])
            };

            var validacao = ControleVaga.Validar(controleVaga, out vagasUtilizadas);

            if (validacao.Valido)
            {
                if (rnAgenda.EhPeriodoFase1VigentePor(controleVaga.Ano))
                {
                    if (controleVaga.VagasLiberadas <= vagasUtilizadas && controleVaga.OfereceVagaFase1)
                    {
                        controleVaga.OfereceVagaFase1 = false;
                    }
                }

                ControleVaga.Alterar(controleVaga);

                e.Cancel = true;
                this.grdControle.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }
        public void Update(object ID_CONTROLE_VAGA, object CENSO, object ESCOLA, object MODALIDADE, object SEGMENTO, object NOME_CURSO, object SERIE, object NOME_TURNO, object VAGAS_CONTINUIDADE, object VAGAS_NOVAS, object VAGAS_LIBERADAS, object VAGAS_UTILIZADAS, object VAGAS_DISPONIVEIS, object PARTICIPAMATRICULAFACIL, object OFERECEVAGAFASE1, object PARALISAMATRICULAFACIL, object VISUALIZAVAGA, object DT_CADASTRO, object DT_ALTERACAO, object MATRICULA, object ANO, object PERIODO, object CURSO, object TURNO)
        {
        }
        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            var sessao = SessaoUsuario.GetSessaoUsuario();

            if (sessao != null)
            {
                if (!this.tseMunicipio.DBValue.IsNull)
                {
                    if (this.tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);

                        sessao.Escola = string.Empty;
                        this.tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    sessao.Municipio = string.Empty;
                    sessao.Escola = string.Empty;
                }
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            var sessao = SessaoUsuario.GetSessaoUsuario();

            if (!this.tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (this.tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                    {
                        sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                        this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                    }

                    this.grdControle.Visible = true;
                    this.lblMensagem.Text = string.Empty;
                    this.pnGrid.Visible = true;
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }

                    this.grdControle.Visible = false;
                    this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    this.pnGrid.Visible = false;
                }
            }
            else
            {
                if (sessao != null)
                {
                    sessao.Escola = string.Empty;
                    sessao.Municipio = string.Empty;
                    sessao.Coordenadoria = string.Empty;
                }

                this.grdControle.Visible = false;
                this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                this.pnGrid.Visible = false;

                this.LimparCampos();
            }
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;

            this.tseMunicipio.ResetValue();
            this.tseUnidadeResponsavel.ResetValue();

            this.cmbAno.SelectedIndex = 0;

            this.cmbPeriodo.Items.Clear();
            this.cmbModalidade.Items.Clear();
            this.cmbNivel.Items.Clear();
            this.cmbEscolaridade.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();

            this.txtVagasContinuidade.Text = string.Empty;
            this.txtVagasNova.Text = string.Empty;
            this.txtVagasDisponiveis.Text = string.Empty;
            this.txtVagasLiberadas.Text = string.Empty;
            this.txtVagasUtilizadas.Text = string.Empty;
            this.chkParticipaMatriculaFacil.Checked = false;
            this.chkOfereceVagaFase1.Checked = false;            
            this.chkVisualizaVagas.Checked = false;
            txtVagaPlanejada.Text = string.Empty;
            this.grdControle.Visible = false;
        }

        private void ValidarCampos()
        {
            this.txtVagasLiberadas.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtVagasLiberadas.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            this.txtVagasUtilizadas.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtVagasUtilizadas.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            this.txtVagasDisponiveis.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtVagasDisponiveis.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            this.txtVagasContinuidade.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtVagasContinuidade.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            this.txtVagasNova.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtVagasNova.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(grdControle);
        }

        protected void grdControle_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            this.ControlaAcesso(this.grdControle);
        }

        protected void grdControle_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "VAGAS_DISPONIVEIS")
            {
                var total = Convert.ToInt32(this.grdControle.GetRowValues(e.VisibleIndex, "TOTALALUNO"));
                var vagaUtilizada = Convert.ToInt32(this.grdControle.GetRowValues(e.VisibleIndex, "VAGAS_UTILIZADAS"));
                var vagaLiberada = Convert.ToInt32(this.grdControle.GetRowValues(e.VisibleIndex, "VAGAS_LIBERADAS"));
                var participaMF = Convert.ToBoolean(this.grdControle.GetRowValues(e.VisibleIndex, "PARTICIPAMATRICULAFACIL"));

                if (total > 0 && vagaLiberada <= vagaUtilizada && participaMF)
                {

                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
