using System;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Controls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Data;
using Techne.Library.Sql.Structure;
using System.Web;


namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
 NavUrl("~/ProcessoSeletivo/ConvocacaoDocente.aspx"),
  ControlText("ConvocacaoDocente"),
  Title("Convocação"),
]
    public partial class ConvocacaoDocente : TPage
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
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Propriedades e Enumeradores
        //private string DOCI_TEXT = "Professor para atuar nos anos finais do ensino fundamental e/ou ensino médio";
        //private string DOCI_VALUE = "DOC I";
        //private string DOCII_TEXT = "Professor para atuar nos anos iniciais do ensino fundamental";
        //private string DOCII_VALUE = "DOC II";
        //private string AREA_INTEGRADA_DOCII = "039";
        private string BOTAO_SELECAO_CANDIDATOS = "Selecionar";
        private string BOTAO_NOVA_SELECAO = "Nova Seleção";
        #endregion

        #region Eventos da Página

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSelecao, "");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Aprovados"] = null;
                Session["privilegiado"] = RN.PadroesDeAcessos.VerificaPrivilegio(Convert.ToString(HttpContext.Current.User.Identity.Name));

                tseMunicipioProc.Enabled = false;
                tseDisciplina.Enabled = false;
				CarregarListaCotas();
            }
            if (!Convert.ToBoolean(Session["privilegiado"]))
            {
                bool bolEhContratoTempo = RN.PadroesDeAcessos.ConsultarPadacesContratoTempoPorUsuario(User.Identity.Name);

                if (!bolEhContratoTempo)
                {
                    tseRegional.DBValue = RN.Usuarios.RetornarRegionalUsuario(User.Identity.Name).ToString();
                    tseRegional.Enabled = false;
                }
                else
                {
                    tseRegional.Enabled = true;
                }
            }
			

            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull &&
                tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull &&
                tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull &&
                dtdataApresent != null && !String.IsNullOrEmpty(dtdataApresent.Text) &&
                dtHoraApresent != null && !dtHoraApresent.Text.Equals("__:__"))
            {
                IDictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("concurso", tseConcurso.DBValue.ToString());
                //pares.Add("nucleo", tseCoordenadoria.DBValue.ToString());
                pares.Add("disciplina", tseDisciplina.DBValue.ToString());
                pares.Add("dt_apresentacao", ObtemDataHora());
                pares.Add("municipio", (tseMunicipioProc.DBValue.IsNull ? null : tseMunicipioProc.DBValue.ToString()));

                btnCorreios.ClientSideEvents.Click = @"function (s, e){ window.open('../Relatorio/Relatorios.aspx?report=rsconvocacaocorreios&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false; }";
                btnCoordenadoria.ClientSideEvents.Click = @"function (s, e){ window.open('../Relatorio/Relatorios.aspx?report=rsconvocacaocoordenadoria&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false; }";
            }
        }
        #endregion

        #region Eventos

        protected void tseConcurso_Changed(object sender, EventArgs args)
        {
			if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull)
			{
				tseMunicipioProc.ResetValue();
			    tseRegional.ResetValue();
				tseDisciplina.ResetValue();
				LimparCampos();
			}
			else
			{
				LimparCampos();
			}
			pnSelecao.Visible = false;
        }

        protected void tseCoordenadoria_Changed(object sender, EventArgs args)
        {
			//if (tseCoordenadoria.IsValidDBValue && !tseCoordenadoria.DBValue.IsNull)
			//{
			//    //cmbCargo.Enabled = true;
			//    //cmbCargo.Items.Clear();
			//    //cmbCargo.SelectedIndex = -1;
			//    QueryTable qtFuncao = RN.CandidatoDocente.ConsultarFuncao(tseConcurso.DBValue.ToString(), tseCoordenadoria.DBValue.ToString());
			//    if (qtFuncao != null && qtFuncao.Rows.Count > 0)
			//    {
			//        //foreach (SimpleRow funcao in qtFuncao.Rows)
			//        //{
			//        //    if (funcao["categoria"].ToString().Equals("DOC I"))
			//        //        cmbCargo.Items.Add(DOCI_TEXT, DOCI_VALUE);
			//        //    if (funcao["categoria"].ToString().Equals("DOC II"))
			//        //        cmbCargo.Items.Add(DOCII_TEXT, DOCII_VALUE);
			//        //}
			//        //cmbCargo.SelectedIndex = 0;
			//        tseDisciplina.SqlWhere = "ldh.Concurso = '" + tseConcurso.DBValue.ToString() +
			//            "' and ldh.nucleo = '" + tseCoordenadoria.DBValue.ToString() + "'";
			//        //"' and ldh.Categoria = '" + cmbCargo.SelectedItem.Value.ToString() + "'";
			//        tseDisciplina.DataBind();
			//    }
			//    tseDisciplina.ResetValue();
			//    pnSelecao.Visible = false;
			//    LimparCampos();
			//    BloquearBotoes();

			//    if (tseConcurso.DBValue.ToString() != "24º Processo")
			//    {
			//        tseMunicipioProc.Enabled = true;
			//        tseMunicipioProc.ResetValue();
			//    }
			//    else
			//    {
			//        tseMunicipioProc.Enabled = false;
			//    }
			//}
			//else
			//{
			//    //cmbCargo.Enabled = false;
			//    //cmbCargo.Items.Clear();
			//    //cmbCargo.SelectedIndex = -1;
			//    tseDisciplina.Mode = ControlMode.View;
			//    tseDisciplina.ResetValue();
			//    tseDisciplina.SqlWhere = string.Empty;
			//    tseDisciplina.DataBind();
			//    tseMunicipioProc.Enabled = false;

			//}
			//lblMensagem.Text = string.Empty;
			//pnCampos.Visible = false;
			//pnSelecao.Visible = false;
        }

     
        protected void tseDisciplina_Changed(object sender, EventArgs args)
        {
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

            pnSelecao.Visible = false;
            LimparCampos();
            BloquearBotoes();
            btnSelecionar.Text = BOTAO_SELECAO_CANDIDATOS;

            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull
				//&& tseCoordenadoria.IsValidDBValue && !tseCoordenadoria.DBValue.IsNull
                //&& !cmbCargo.SelectedItem.Value.Equals(string.Empty)
                && tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull)
            {
                string concurso = tseConcurso.DBValue.ToString();
                //string nucleo = tseCoordenadoria.DBValue.ToString();
                //string categoria = cmbCargo.SelectedItem.Value.ToString();
                string disciplina = tseDisciplina.DBValue.ToString();
                string municipio = tseMunicipioProc.DBValue.ToString();
				string regional = tseRegional.DBValue.ToString();

                txtDisponivel.Text = Convert.ToString(rnProcessoSeletivo.ObtemInscricoesDisponiveisPor(disciplina, concurso, municipio, regional));
                int disponivel = Convert.ToInt32(txtDisponivel.Text);
                if (disponivel <= 0)
                {
                    btnSelecionar.Enabled = false;
                    lblMensagem.Text = "Atenção: Não existem inscrições disponíveis para o processo seletivo.";
                    return;
                }
                HabilitarCampos();
                pnCampos.Visible = true;
                btnSelecionar.Enabled = true;
                lblMensagem.Text = string.Empty;
            }
            else
            {
                lblMensagem.Text = "Favor selecionar processo seletivo, coordenadoria, função e disciplina válidos.";
                pnCampos.Visible = false;
                return;
            }
        }

        protected void tseMunicipioProc_Changed(object sender, EventArgs args)
        {
			//if (Page.IsCallback)
			//    return;
			//if (!tseMunicipioProc.DBValue.IsNull && tseMunicipioProc.IsValidDBValue)
			//{
			//    //cmbCargo.Enabled = true;
			//    //cmbCargo.Items.Clear();
			//    //cmbCargo.SelectedIndex = -1;
			//    tseDisciplina.ResetValue();


			//    QueryTable qtFuncao = RN.CandidatoDocente.ConsultarFuncao(tseConcurso.DBValue.ToString(), tseCoordenadoria.DBValue.ToString());
			//    if (qtFuncao != null && qtFuncao.Rows.Count > 0)
			//    {

			//        if (tseMunicipioProc.DBValue.ToString() != "00007043")
			//        {
			//            tseDisciplina.SqlWhere = "ldh.Concurso = '" + tseConcurso.DBValue.ToString() +
			//                "' and ldh.nucleo = '" + tseCoordenadoria.DBValue.ToString() +
			//                //"' and ldh.Categoria = '" + cmbCargo.SelectedItem.Value.ToString() +
			//                "' and  ldh.Municipio_proc = '" + tseMunicipioProc.DBValue.ToString() + "'";
			//            tseDisciplina.DataBind();
			//        }
			//    }
			//    tseDisciplina.Mode = ControlMode.Edit;


			//}
			//else
			//{
			//    tseDisciplina.ResetValue();
			//    tseDisciplina.SqlWhere = "ldh.Municipio_proc is null";
			//    tseDisciplina.DataBind();
			//}

        }

        public object Listar(DbObject tseRegional, DbObject tseConcurso, DbObject tseDisciplina, string qtd, string dtHoraApresent, DateTime dtdataApresent, DbObject tseMunicipioProc, object cota)
        {
            QueryTable dadosGrid = null;

            if (!tseRegional.IsNull && !tseConcurso.IsNull && !tseDisciplina.IsNull && !string.IsNullOrEmpty(qtd))
            {
                if (Session["Aprovados"] == null)
                {
                    dadosGrid = RN.ProcessoSeletivo.SelecionarConvocadosPor(qtd, tseConcurso.ToString(), tseDisciplina.ToString(), tseMunicipioProc.ToString(), tseRegional.ToString(), cota.ToString());
                }
                else
                {
                    string hora = dtHoraApresent;
                    string[] horas_s = hora.Split(':');
                    int h = Convert.ToInt16(horas_s[0]);
                    int m = Convert.ToInt16(horas_s[1]);
                    DateTime data = dtdataApresent.Date;
                    data = data.AddHours(h);
                    data = data.AddMinutes(m);
                    dadosGrid = RN.ProcessoSeletivo.SelecionarAprovadosPor(qtd, tseConcurso.ToString(), tseDisciplina.ToString(), data, data, tseMunicipioProc.ToString(), tseRegional.ToString(), cota.ToString());
                }
            }

            return dadosGrid;
        }

        protected void btnSelecionar_Click(object sender, EventArgs e)
        {
            Session["Aprovados"] = null;

            #region Seleção de Candidatos
            if (btnSelecionar.Text == BOTAO_SELECAO_CANDIDATOS)
            {
                if ((tseConcurso.DBValue.ToString() != "24º Processo") && (tseMunicipioProc.DBValue.ToString() == string.Empty))
                {
                    lblMensagem.Text = "O campo Município é de preenchimento obrigatório para este processo seletivo.";
                    return;
                }
                int qtde = 0;
                if (!Int32.TryParse(txtQuantidade.Text, out qtde))
                {
                    lblMensagem.Text = "Quantidade inválida.";
                    return;
                }

                int disponivel = Convert.ToInt32(txtDisponivel.Text);

                if (disponivel == 0)
                {
                    lblMensagem.Text = "Não há inscrições disponíveis para o processo seletivo.";
                    return;
                }

                if (qtde > disponivel)
                {
                    lblMensagem.Text = "Quantidade não pode ser maior que o número de inscrições disponíveis.";
                    return;
                }

                if (qtde < 0)
                {
                    lblMensagem.Text = "Quantidade deve ser um número inteiro maior que zero.";
                    return;
                }

                string hora = dtHoraApresent.Text;
                string[] horas_s = hora.Split(':');
                int h = Convert.ToInt16(horas_s[0]);
                int m = Convert.ToInt16(horas_s[1]);
                DateTime data = dtdataApresent.Date;
                data = data.AddHours(h);
                data = data.AddMinutes(m);

                //valida horário e data maior que corrente
                if (data <= DateTime.Now)
                {
                    lblMensagem.Text = "Data e horário não podem ser menores que a data e horário correntes.";
                    return;
                }

                string concurso = tseConcurso.DBValue.ToString();
                if (!RN.ProcessoSeletivo.ValidaDataApresentacao(concurso, data))
                {
                    lblMensagem.Text = "Data de apresentação não pode ser menor que 48 horas após a data da convocação, desconsiderados sábados e domingos.";
                    return;
                }

				if (tseRegional.DBValue.IsNull)
				{
					lblMensagem.Text = "É necessário escolher uma regional";
					return;
				}

                QueryTable qt = null;
                string disciplina = tseDisciplina.DBValue.ToString();
                string qtd = txtQuantidade.Text;
                string munic = tseMunicipioProc.DBValue.ToString();
                string regional = tseRegional.DBValue.ToString();

                qt = RN.ProcessoSeletivo.SelecionarConvocadosPor(qtd, concurso, disciplina, munic, regional, ddlCotas.SelectedValue.ToString());

                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                    {
                        odsSelecao.Select();
                        odsSelecao.DataBind();
                        grdSelecao.DataBind();
                        pnSelecao.Visible = true;
                        DesbloquearBotoes();
                        btnSelecionar.Text = BOTAO_NOVA_SELECAO;
                        DesabilitarCampos();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Não existem candidatos para serem selecionados.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Não existem candidatos para serem selecionados.";
                }
            }
            #endregion

            #region Nova Seleção
            else if (btnSelecionar.Text.Equals(BOTAO_NOVA_SELECAO))
            {
                HabilitarCampos();
                LimparCampos();
                BloquearBotoes();
               // txtDisponivel.Text = Convert.ToString(RN.ProcessoSeletivo.ConsultarInscricoesDisponiveisPor(tseDisciplina.DBValue.ToString(), tseConcurso.DBValue.ToString(), tseMunicipioProc.DBValue.ToString(), tseRegional.DBValue.ToString()));
                btnSelecionar.Text = BOTAO_SELECAO_CANDIDATOS;
                pnSelecao.Visible = false;
            }
            #endregion
        }

        protected void btnConvocar_Click(object sender, EventArgs e)
        {
            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull
				//&& tseCoordenadoria.IsValidDBValue && !tseCoordenadoria.DBValue.IsNull
                //&& !cmbCargo.SelectedItem.Value.Equals(string.Empty)
                && tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull)
            {
                if ((tseConcurso.DBValue.ToString() != "24º Processo") && (tseMunicipioProc.DBValue.ToString() == string.Empty))
                {
                    lblMensagem.Text = "O campo Município é de preenchimento obrigatório para este processo seletivo.";
                    return;
                }

                string concurso = tseConcurso.DBValue.ToString();
                //string nucleo = tseCoordenadoria.DBValue.ToString();
                //string categoria = cmbCargo.SelectedItem.Value.ToString();
                string disciplina = tseDisciplina.DBValue.ToString();
                string municipio = tseMunicipioProc.DBValue.ToString();			
                string qtd = txtQuantidade.Text;
                string hora = dtHoraApresent.Text;
                string[] horas_s = hora.Split(':');
                int h = Convert.ToInt16(horas_s[0]);
                int m = Convert.ToInt16(horas_s[1]);
                DateTime data = dtdataApresent.Date;
                data = data.AddHours(h);
                data = data.AddMinutes(m);
				int regional = (int)tseRegional.DBValue;
				int intCota = Convert.ToInt32(ddlCotas.SelectedValue); ;
				
                RetValue retorno = null;
				QueryTable qt = null;
                retorno = Techne.Lyceum.RN.ProcessoSeletivo.ExecutaConvocaReprovaPor(qtd, concurso, disciplina, data, data, municipio, regional, intCota, ref qt);
                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        lblMensagem.Text = retorno.Errors.ToString();
                        Session["Aprovados"] = null;
                    }
                    else
                    {
                        lblMensagem.Text = retorno.Message;
                        //btnCorreios.Enabled = true;
                        btnCoordenadoria.Enabled = true;
                        btnConvocar.Enabled = false;
                        Session["Aprovados"] = "true";
                        odsSelecao.Select();
                        odsSelecao.DataBind();
                        grdSelecao.DataBind();

						if (qt.Rows.Count > 0)
						{
							foreach (DataRow item in qt.Rows)
							{
                                EnviaMalaDireta(CarregaEmail(item["candidato"].ToString(), concurso));
							}
						}
                    }
                }
            }
        }
        #endregion

        #region Métodos

		private void EnviaMalaDireta(string strEmail)
		{
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

            string endereco = rnProcessoSeletivo.RetornaEnderecoCoordenadoria(tseRegional.DBValue.ToString()).Rows[0]["ENDERECO"].ToString();

			RN.DTOs.DadosEmail email = new Techne.Lyceum.RN.DTOs.DadosEmail();
			//email.Remetente = "cosep@educacao.rj.gov.br";
            email.Remetente = System.Configuration.ConfigurationManager.AppSettings["EmailContratoTemporario"].ToString();
            email.Login = System.Configuration.ConfigurationManager.AppSettings["EmailContratoTemporario_Login"].ToString();
            email.Senha = System.Configuration.ConfigurationManager.AppSettings["EmailContratoTemporario_Senha"].ToString();
			email.Assunto = "Convocação Processo Seletivo";
			email.Texto = "<p>" +
								"PREZADO PROFESSOR, " +
								" <br />" +
								" <br />" +
								" Solicitamos seu comparecimento à Regional " + tseRegional["REGIONAL"].ToString() + ", " +
								" <br />" +
								" situado na " + endereco + ", " +
								" <br />" +
								" dia " + dtdataApresent.Text + ", às " + dtHoraApresent.Text + " horas , a fim de tratar de possível Contratação Temporária, " +
								" <br />" +
								" munido dos seguintes documentos (original e cópia): " +
								" <br />" +
								" <br />" +
								" • Carteira de Identidade; " +
								" <br />" +
								" • CPF; " +
								" <br />" +
								" • Título de Eleitor, comprovando a quitação com a Justiça Eleitoral; " +
								" <br />" +
								" • Certificado de Reservista; (no caso homem) " +
								" <br />" +
								" • PIS/PASEP; " +
								" <br />" +
								" • Carteira de Trabalho e Previdência Social – CTPS; " +
								" <br />" +
								" • Comprovante de Residência; " +
								" <br />" +
								" • Documento comprobatório onde consta o nº da Conta Bancária (BRADESCO), se possuir; " +
								" <br />" +
								" • Atestado de Saúde Ocupacional (ORIGINAL); " +
								" <br />" +
								" • Documentação comprobatória de experiência na área de atuação; " +
								" <br />" +
								" •	Documentação comprobatória da habilitação para a função relativa à contratação. " +
								" <br />" +
								" <br />" +
								" Rio de Janeiro, " + DateTime.Now.ToShortDateString() + "." +
								" <br />" +
								" <br />" +
                                " "+ tseRegional["REGIONAL"].ToString() + "" +								
								" <br />" +
								" <br />" +
								" " +
							"</p>";

			email.Destinatario = strEmail;
			//foreach (var item in Emails)
			//{
			//    email.Destinatario = item.ToString();
			//  //email.Destinatario = "chelena@educacao.rj.gov.br; rmoreira@educacao.rj.gov.br;";

			try
			{
                RN.Util.Email.EnviarMail(email);
			}
			catch (Exception)
			{
				throw;
			}
			//}

		}

		private string CarregaEmail(string candidato, string concurso)
		{
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

            return rnProcessoSeletivo.MontarMalaDireta(candidato, concurso);
		}

        protected void LimparCampos()
        {
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            string concurso = tseConcurso.DBValue.ToString();
            string disciplina = tseDisciplina.DBValue.ToString();
            string municipio = tseMunicipioProc.DBValue.ToString();
            string regional = tseRegional.DBValue.ToString();
            ddlCotas.SelectedIndex = 2;
            ddlCotas_SelectedIndexChanged(null, null);
            //lblMensagem.Text = string.Empty;
            //txtDisponivel.Text = Convert.ToString(rnProcessoSeletivo.ObtemInscricoesDisponiveisPor(disciplina, concurso, municipio, regional,ddlCotas.SelectedValue));
            //int disponivel = Convert.ToInt32(txtDisponivel.Text);
            txtQuantidade.Text = string.Empty;
            dtdataApresent.Text = string.Empty;
            dtHoraApresent.Text = string.Empty;
           
        }

        protected void HabilitarCampos()
        {
            txtDisponivel.Enabled = true;
            txtQuantidade.Enabled = true;
            dtdataApresent.Enabled = true;
            dtHoraApresent.Enabled = true;
        }

        protected void DesabilitarCampos()
        {
            txtDisponivel.Enabled = false;
            txtQuantidade.Enabled = false;
            dtdataApresent.Enabled = false;
            dtHoraApresent.Enabled = false;
        }

        protected void BloquearBotoes()
        {
            btnConvocar.Enabled = false;
            btnCorreios.Enabled = false;
            btnCoordenadoria.Enabled = false;
        }

        protected void DesbloquearBotoes()
        {
            btnConvocar.Enabled = true;
        }

        private string ObtemDataHora()
        {
            string hora = dtHoraApresent.Text;
            string[] horas_s = hora.Split(':');
            int h = Convert.ToInt16(horas_s[0]);
            int m = Convert.ToInt16(horas_s[1]);
            DateTime data = dtdataApresent.Date;
            data = data.AddHours(h);
            data = data.AddMinutes(m);
            return data.ToString("yyyy-MM-dd HH:mm") + ":00";
        }

		private void CarregarListaCotas()
		{
			QueryTable qtCotas = null;

			qtCotas = RN.ContratoTemporario.Cota.ListarCotas();
			ddlCotas.DataSource = qtCotas;
			ddlCotas.DataBind();
			ddlCotas.SelectedIndex = 2;
		}

		protected void ddlCotas_SelectedIndexChanged(object sender, EventArgs e)
		{
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

			if (ddlCotas.SelectedValue == "3")
                txtDisponivel.Text = Convert.ToString(rnProcessoSeletivo.ObtemInscricoesDisponiveisPor(tseDisciplina.DBValue.ToString(), tseConcurso.DBValue.ToString(), tseMunicipioProc.DBValue.ToString(), tseRegional.DBValue.ToString()));
			else
                txtDisponivel.Text = Convert.ToString(rnProcessoSeletivo.ObtemInscricoesDisponiveisPor(tseDisciplina.DBValue.ToString(), tseConcurso.DBValue.ToString(), tseMunicipioProc.DBValue.ToString(), tseRegional.DBValue.ToString(), ddlCotas.SelectedValue.ToString()));

		}

        #endregion
    }
}
