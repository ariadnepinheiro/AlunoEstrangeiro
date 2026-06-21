using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Techne.Web;
using Techne.Lyceum.CR;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Controls;
using Techne.Library.Sql.Structure;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
	[NavUrl("~/ProcessoSeletivo/CandidatoClassificacao.aspx"),
   ControlText("CandidatoClassificacao"),
   Title("Classificação Candidato"),]

	public partial class CandidatoClassificacao : TPage
	{

		#region Enum

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
						return (TipoOperacao)ViewState["_tipoOperacao"];
				}

				return TipoOperacao.Inicial;
			}
			set { ViewState["_tipoOperacao"] = value; }
		}

		#endregion

		#region Constantes

		private string DOCI_TEXT = "Professor para atuar nos anos finais do ensino fundamental e/ou ensino médio";
		private string DOCI_VALUE = "DOC I";
		private string DOCII_TEXT = "Professor para atuar nos anos iniciais do ensino fundamental";
		private string DOCII_VALUE = "DOC II";
		private string AREA_INTEGRADA_DOCII = "039";

		#endregion

		#region Eventos

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdSelecao, "");

			if (!string.IsNullOrEmpty(QueryStringDecodificada["usuario"]))
			{
                tseConcursoBusca.SqlWhere = " tipo = 'Contrato'";
			}

		}

		protected void Page_Load(object sender, EventArgs e)
		{
            try
            {
                Session["Aprovados"] = null;

                if (!IsPostBack)
                {
                    if (!string.IsNullOrEmpty(QueryStringDecodificada["usuario"]))
                    {
                        string candCon = CandidatoDocente.ConsultaCandidato(QueryStringDecodificada["usuario"]);

                        tseConcursoBusca.DBValue = candCon.Split('|')[1];
                        if (!CandidatoDocente.ExisteConcursoConsulta(tseConcursoBusca.DBValue.ToString()))
                        {
                            Response.Write("Não existe concurso ativo!");
                            Response.Redirect("~/Manutencao/ConcursoExpirou.aspx");
                        }

                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
		}

    	protected void tseMunicipio_Changed(object sender, EventArgs args)
		{
            if ((tseConcursoBusca.DBValue.IsNull || !tseConcursoBusca.IsValidDBValue) ||
                (tseRegional.DBValue.IsNull || !tseRegional.IsValidDBValue) ||
                (tseMunicipio.DBValue.IsNull || !tseMunicipio.IsValidDBValue))
            {
                lblMensagem.Visible = true;
                lblMensagem.Text = "Favor preencher todos os campos.";
                _tipoOperacao = TipoOperacao.Inicial;
            }
            else
            {
                lblMensagem.Text = string.Empty;
                _tipoOperacao = TipoOperacao.Consultar;

                string concurso = tseConcursoBusca.DBValue.ToString();
                int regional = Convert.ToInt32(tseRegional.DBValue.ToString());
                string municipio = tseMunicipio.DBValue.ToString();

                int numeroCandidatos = RN.ProcessoSeletivo.ObtemTotalInscricoesDisponiveis(concurso, regional, municipio);

                if (numeroCandidatos <= 0)
                {
                    lblMensagem.Visible = true;
                    lblMensagem.Text = "Atenção: Não existem inscrições disponíveis para o processo seletivo.";
                    return;
                }
                lblMensagem.Text = string.Empty;
            }

            ControlarTipoOperacao();
		}

		protected void btnLimpar_Click(object sender, EventArgs e)
		{
			LimparTela();
		}

		#endregion

		#region Métodos Privados

		private void ControlarTipoOperacao()
		{
			switch (_tipoOperacao)
			{
				case TipoOperacao.Inicial:
					{
						pnlResultadoConsulta.Visible = false;
						tseConcursoBusca.SqlWhere = " tipo = 'Contrato' AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, CD.DT_INICIO) AND     CONVERT(DATE, CD.DT_FIM)";
						break;
					}
				case TipoOperacao.Sucesso:
					{
						

						break;
					}

				case TipoOperacao.Novo:
					{
					

						break;
					}
				case TipoOperacao.Excluir:
					{
						
						break;
					}
				case TipoOperacao.Alterar:
					{
						

						break;
					}
				case TipoOperacao.Consultar:
					{
                        RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

                        tseConcursoBusca.SqlWhere = " tipo = 'Contrato' AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, CD.DT_INICIO) AND     CONVERT(DATE, CD.DT_FIM)";

						string concurso = tseConcursoBusca.DBValue.ToString();
                        string regional = tseRegional.DBValue.ToString();
						string munic = tseMunicipio.DBValue.ToString();

                        DataTable qt = rnProcessoSeletivo.ObtemConvocadosEAprovadosPor(concurso, regional, munic);

                        if (qt != null)
						{
						    if (qt.Rows.Count > 0)
							{
                                pnlResultadoConsulta.Visible = true;
                                odsSelecao.Select();
                                odsSelecao.DataBind();
                                grdSelecao.DataBind();
                                lblMensagem.Text = string.Empty;
                            }
                        }
						 break;
					}
			}
		}

		private object CarregarDadosDrop(string idDrop)
		{
			QueryTable dadosDrop = null;

			try
			{
				
			}
			catch
			{
				throw;
			}
			finally
			{
				dadosDrop = null;
			}
			return dadosDrop;
		}

		private void CarregarDadosRegional()
		{
			QueryTable qt = RN.ProcessoSeletivo.ObterRegional();

			tseRegional.DBValue = Convert.ToString(qt.Rows[0]["ID_REGIONAL"]);
			tseRegional.DataBind();
		}

		private void LimparTela()
		{
			tseConcursoBusca.ResetValue();
			tseMunicipio.ResetValue();
			tseRegional.ResetValue();
			pnlResultadoConsulta.Visible = false;
			lblMensagem.Text = string.Empty;
		}




		#endregion

		#region Métodos Públicos

		public object Listar(DbObject tseRegional, DbObject tseConcursoBusca, DbObject tseMunicipio)
		{
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
			DataTable dadosGrid = null;

			if (!tseMunicipio.IsNull && !tseConcursoBusca.IsNull && !tseRegional.IsNull)
			{
				dadosGrid = rnProcessoSeletivo.ObtemConvocadosEAprovadosPor(tseConcursoBusca.ToString(), tseRegional.ToString(), tseMunicipio.ToString());

			}

			return dadosGrid;
		}

		#endregion

	}
}
