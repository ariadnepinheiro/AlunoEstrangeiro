using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/InscricaoCompartilhadas.aspx")
    , ControlText("InscricaoCompartilhadas")
    , Title("Inscrição de Compartilhadas")]
    public partial class InscricaoCompartilhadas : TPage
    {
        #region Propriedades

        IList<String> mensagens;

        #endregion

        #region Funções da Tela

        public InscricaoCompartilhadas()
        {
            mensagens = new List<String>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DataTable dt = new DataTable();
                    dt = RN.Agenda.Agenda.ListaAnoPeriodoAgendaPorTipoEventoEDataEvento(Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.InscricaoAlunosCompartilhadas), DateTime.Now);

                    if (dt.Rows.Count > 0)
                    {
                        ddlAnoPeriodo.DataSource = dt;
                        ddlAnoPeriodo.DataBind();
                        ddlAnoPeriodo.Items.Insert(0, "Selecione");
                        ddlAnoPeriodo.Enabled = true;

                        CarregaComponentes();
                    }
                    else
                    {
                        lblMsg.Text = @"Não existe nenhuma inscrição de alunos das compartilhadas disponível no momento.";
                        btnInscricao.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        #endregion

        #region Methods Privates

        /// <summary>
        /// 
        /// </summary>
        private void CarregaComponentes()
        {
            //CarregaUnidadeEnsinoDestino();
            //CarregaUnidadeEnsinoOrigem();
            CarregaCursos();
        }

        /// <summary>
        /// 
        /// </summary>

        private void CarregaUnidadeEnsinoDestino()
        {
            if (Page.IsCallback)
                return;

            try
            {
                if (!tseUnidadeEnsinoDestino.DBValue.IsNull)
                {
                    lblMsg.Text = tseUnidadeEnsinoDestino.IsValidDBValue
                        ? string.Empty
                        : "Unidade de Ensino não ativa ou não cadastrada (favor verificar).";
                }
                else
                {
                    lblMsg.Text = "Unidade de Ensino não ativa ou não cadastrada (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Erro ao carregar unidades de ensino de destino.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CarregaUnidadeEnsinoOrigem()
        {
            if (tseUnidadeEnsinoDestino.DBValue.IsNull)
                return;

            try
            {
                string unidadeEnsino = tseUnidadeEnsinoDestino["unidade_ens"].ToString();
                using (DataTable dtEscolas = RN.UnidadeEnsino.RetornaUnidadeEnsinoAlunosOrigemDasCompartilhadas(unidadeEnsino))
                {
                    if (dtEscolas.Rows.Count > 0)
                    {
                        ddlUnidadeEnsinoOrigem.DataSource = dtEscolas;
                        ddlUnidadeEnsinoOrigem.DataBind();
                        ddlUnidadeEnsinoOrigem.Items.Insert(0, "Selecione");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Erro ao carregar unidades de ensino de origem.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CarregaCursos()
        {
            DataTable cursos_agenda = new DataTable();
            DataTable cursos = new DataTable();
            RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino agenda_Curso__Agenda_UnidadeEnsino
                = new RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino();
            RN.Agenda.Agenda rnAagenda = new Techne.Lyceum.RN.Agenda.Agenda();
            RN.Agenda.Agenda_Curso rnAgenda_Curso = new Techne.Lyceum.RN.Agenda.Agenda_Curso();
          
            if ((ddlAnoPeriodo.SelectedValue != "Selecione") && (this.tseUnidadeEnsinoDestino.IsValidDBValue && !this.tseUnidadeEnsinoDestino.DBValue.IsNull))
            {
                string[] anoperiodo = ddlAnoPeriodo.SelectedValue.ToString().Split('-');
                int ano = Convert.ToInt32(anoperiodo[0]);
                int periodo = Convert.ToInt32(anoperiodo[1]);

                int agendaId = rnAagenda.ObtemAgendaAbertaPor(Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.InscricaoAlunosCompartilhadas), DateTime.Now, ano, periodo);

                RN.Agenda.Agenda_UnidadeEnsino rnAgenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_UnidadeEnsino();

                if (rnAgenda_UnidadeEnsino.EhUnidadeParticipantePor(agendaId, (this.tseUnidadeEnsinoDestino.IsValidDBValue && !this.tseUnidadeEnsinoDestino.DBValue.IsNull) ? tseUnidadeEnsinoDestino.DBValue.ToString() : null))
                {
                    cursos_agenda = rnAgenda_Curso.ListaCursoPorAgendaEParticipacao(agendaId, (this.tseUnidadeEnsinoDestino.IsValidDBValue && !this.tseUnidadeEnsinoDestino.DBValue.IsNull) ? tseUnidadeEnsinoDestino.DBValue.ToString() : null);
                    
                }
                else
                {
                    cursos_agenda = new DataTable();
                }

                ddlCurso.Enabled = true;

                ddlCurso.DataSource = cursos_agenda;
                ddlCurso.DataBind();
                ddlCurso.Items.Insert(0, "Selecione");
            }
        }

        /// <summary>
        /// Função para tratar as mensagens de erro e listar na tela.
        /// </summary>
        /// <param name="mensagens"></param>
        /// <returns></returns>
        private string TrataMensagemErro(IList<string> mensagens)
        {
            string retorno = "";
            foreach (var msg in mensagens)
                retorno += "<li  style='color: Red;'>" + msg.Trim() + Environment.NewLine;

            return retorno;
        }

        private string MontarQueryString(int ano, int periodo, string unidadeDestino, string unidadeOrigem, string curso, string serie)
        {
            string queryString = string.Empty;
            queryString += "ano=" + ano;
            queryString += "&periodo=" + periodo;
            queryString += "&unidadeDestino=" + unidadeDestino;
            queryString += "&unidadeOrigem=" + unidadeOrigem;
            queryString += "&curso=" + curso;
            queryString += "&serie=" + serie;
            return queryString;
        }

        #endregion

        #region BTN OnClick

        protected void btnInscricao_Click(object sender, EventArgs e)
        {
            try
            {
                string queryString = string.Empty;

                #region Validação

                if (ddlAnoPeriodo.SelectedValue == "Selecione" || ddlAnoPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O Ano e Período é obrigatório.");
                }

                if (tseUnidadeEnsinoDestino.DBValue == null)
                {
                    mensagens.Add("A Unidade de Ensino de Destino é obrigatório.");
                }

                if (ddlUnidadeEnsinoOrigem.SelectedValue == "Selecione" || ddlUnidadeEnsinoOrigem.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("A Unidade de Ensino de Origem é obrigatório.");
                }

                if (ddlCurso.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O Curso é obrigatório.");
                }

                #endregion

                if (mensagens.Count > 0)
                {
                    lblMsg.Text = TrataMensagemErro(mensagens);
                }
                else
                {
                    string[] anoperiodo = ddlAnoPeriodo.SelectedValue.ToString().Split('-');
                    int ano = Convert.ToInt32(anoperiodo[0]);
                    int periodo = Convert.ToInt32(anoperiodo[1]);

                    queryString = MontarQueryString(
                        ano
                        , periodo
                        , Convert.ToString(tseUnidadeEnsinoDestino.DBValue)
                        , ddlUnidadeEnsinoOrigem.SelectedValue.ToString()
                        , Convert.ToString(ddlCurso.SelectedValue)
                        , ddlSerie.SelectedValue);

                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    Response.Redirect("Alunos.aspx?ChaveInscricaoCompartilhadas=" + Convert.ToBase64String(bytesToEncode), false);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void CarregaSerie()
        {
            DataTable cursos_agenda = new DataTable();
            DataTable series = new DataTable();
            RN.Agenda.Agenda_Curso rnAgenda_Curso = new Techne.Lyceum.RN.Agenda.Agenda_Curso();
            RN.Agenda.Agenda rnAagenda = new Techne.Lyceum.RN.Agenda.Agenda();
            //Carrega os cursos da agenda
            if (!ddlCurso.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                string[] anoperiodo = ddlAnoPeriodo.SelectedValue.ToString().Split('-');
                int ano = Convert.ToInt32(anoperiodo[0]);
                int periodo = Convert.ToInt32(anoperiodo[1]);

                int agendaId = rnAagenda.ObtemAgendaAbertaPor(Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.InscricaoAlunosCompartilhadas), DateTime.Now, ano, periodo);

                series = rnAgenda_Curso.ListaSeriePorAgendaEParticipacao(agendaId, Convert.ToString(tseUnidadeEnsinoDestino.DBValue), ddlCurso.SelectedValue);

                ListItem item = new ListItem("Selecione", string.Empty);
                ddlSerie.DataSource = series;
                ddlSerie.DataBind();
                ddlSerie.Items.Insert(0, item);
                ddlSerie.Enabled = true;
            }
        }

        #endregion

        #region Eventos DropDownList

        protected void ddlAnoPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerie.Items.Clear();
                CarregaCursos();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsinoDestino_Changed(object sender, Controls.ChangedEventArgs args)
        {
            try
            {
                ddlSerie.Items.Clear();
                CarregaCursos();
                CarregaUnidadeEnsinoOrigem();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerie.Items.Clear();
                CarregaSerie();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        #endregion
    }
}
