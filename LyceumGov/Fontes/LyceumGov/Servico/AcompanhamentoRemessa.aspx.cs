using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.CartaoEstudante.Service;
using Techne.Lyceum.RN.CartaoEstudante.DTO.Filter;
using DevExpress.Web.ASPxGridView;
using DevExpress.Utils;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Image = System.Drawing.Image;
using System.IO;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.Servico
{
    [NavUrl("~/Servico/AcompanhamentoRemessa.aspx"), ControlText("AcompanhamentoRemessa"), Title("Remessas para Riocard")]

    public partial class AcompanhamentoRemessa : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemGridRetornoCritica.Text = string.Empty;

                if (!IsPostBack)
                {
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProcessamentoRemessa, "Situação de Processamento da Remessa");
            TituloGrid(grdRetornoCritica, "Críticas do Último Retorno");
        }

        #region Filtros

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            grdProcessamentoRemessa.Visible = false;

            if (!tseAluno.DBValue.IsNull)
            {
                if (tseAluno.IsValidDBValue)
                {
                    lblMensagem.Text = string.Empty;
                    tseRegional.ResetValue();
                    tseMunicipio.ResetValue();
                    tseUnidadeEnsino.ResetValue();
                }
                else
                {
                    lblMensagem.Text = "Matrícula inexistente.";
                    tseAluno.ResetValue();
                }
            }
            else
            {
                lblMensagem.Text = "Matrícula inexistente.";
                tseAluno.ResetValue();
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            grdProcessamentoRemessa.Visible = false;
            try
            {
                if (!tseRegional.DBValue.IsNull)
                {
                    if (tseRegional.IsValidDBValue)
                    {
                        tseUnidadeEnsino.ResetValue();
                        tseMunicipio.ResetValue();
                        tseAluno.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            grdProcessamentoRemessa.Visible = false;
            try
            {
                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (tseMunicipio.IsValidDBValue)
                    {
                        tseUnidadeEnsino.ResetValue();
                        tseRegional.ResetValue();
                        tseAluno.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            grdProcessamentoRemessa.Visible = false;
            try
            {
                if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                {
                    tseMunicipio.ResetValue();
                    tseRegional.ResetValue();
                    tseAluno.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblPossuiCriticas_IndexChanged(object sender, EventArgs e)
        {
            grdProcessamentoRemessa.Visible = false;
        }

        protected void dtEnvioInicial_ValueChanged(object sender, EventArgs e)
        {
            grdProcessamentoRemessa.Visible = false;
        }

        protected void dtEnvioFinal_ValueChanged(object sender, EventArgs e)
        {
            grdProcessamentoRemessa.Visible = false;
        }
                
        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            if (EscolheuFiltro())
            {
                grdProcessamentoRemessa.DataSource = null;
                grdProcessamentoRemessa.PageIndex = 0;
                grdProcessamentoRemessa.DataBind();
                grdProcessamentoRemessa.Visible = false;
                lblMensagem.Text = String.Empty;
                CarregaGrid();
            }
            else
            {
                lblMensagem.Text = "Escolha uma opção de filtro.";
            }
        }

        private bool EscolheuFiltro()
        {
            bool escolheuFiltro = false;

            if ((!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue)
                || (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                || (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                || (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                || (Convert.ToDateTime(dtEnvioInicial.Value) != DateTime.MinValue && Convert.ToDateTime(dtEnvioFinal.Value) != DateTime.MinValue)
                || !rblPossuiCriticas.SelectedValue.Equals("0"))
                escolheuFiltro = true;

            return escolheuFiltro;
        }

        #endregion

        protected void grdProcessamentoRemessa_PageIndexChanged(object sender, EventArgs e)
        {           
            CarregaGrid();
        }  

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int remessaId = Convert.ToInt32(e.CommandArgument);
                string codigoSolicitacao = grdProcessamentoRemessa.GetRowValuesByKeyValue(remessaId, "CodSolicitacao").ToString();

                ppcDetalhesRemessa.ShowOnPageLoad = true;
                LimpaDadosPopupDetalhes();
                PreencheDetalhesRemessa(remessaId, codigoSolicitacao);
                PreencheDetalhesRetorno(remessaId);                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void LimpaDadosPopupDetalhes()
        {
            #region tabRemessa
            bimgFotoPessoa.Value = null;
            txtCodigoRemessa.Text = String.Empty;
            txtLote.Text = String.Empty;
            txtSolicitacao.Text = String.Empty;
            txtOperadora.Text = String.Empty;
            txtDataInclusaoRemessa.Text = String.Empty;
            txtMatricula.Text = String.Empty;
            txtNomeAluno.Text = String.Empty;
            txtDataNascimento.Text = String.Empty;
            txtNomeMae.Text = String.Empty;
            txtNomePai.Text = String.Empty;
            txtCPF.Text = String.Empty;
            txtRGNum.Text = String.Empty;
            txtRGUF.Text = String.Empty;
            txtRGEmissor.Text = String.Empty;
            txtRGDataEmissao.Text = String.Empty;
            txtCep.Text = String.Empty;
            txtLogradouro.Text = String.Empty;
            txtEnderecoNumero.Text = String.Empty;
            txtComplemento.Text = String.Empty;
            txtBairro.Text = String.Empty;
            txtMunicipio.Text = String.Empty;
            txtEstado.Text = String.Empty;
            txtGratuidade.Text = String.Empty;
            txtTrem.Text = String.Empty;
            txtOnibus.Text = String.Empty;
            txtMetro.Text = String.Empty;
            txtBarcas.Text = String.Empty;
            txtUnidadeEnsino.Text = String.Empty;
            txtTurno.Text = String.Empty;
            txtTurma.Text = String.Empty;
            txtSerie.Text = String.Empty;
            txtEmailInterno.Text = String.Empty;
            txtLoginCartao.Text = String.Empty;
            txtDataEnvioLogRemessa.Text = String.Empty;
            txtDataUltimaAtualizacao.Text = String.Empty;
            #endregion

            #region tabRetorno
            txtBeneficiario.Text = String.Empty;
            txtDataProcessamentoRetorno.Text = String.Empty;
            txtSituacaoProcessamento.Text = String.Empty;
            txtDataInclusaoRetorno.Text = String.Empty;
            grdRetornoCritica.DataSource = null;
            grdRetornoCritica.DataBind();
            #endregion
        }

        private void PreencheDetalhesRetorno(int remessaId)
        {
            RetornoService retornoService = RetornoService.Instancia;

            RetornoDTO dtoRetorno = retornoService.ObtemDetalhesUltimoRetornoPor(remessaId);
            if (dtoRetorno != null)
            {
                txtBeneficiario.Text = dtoRetorno.IdBeneficiario == 0 ? string.Empty : dtoRetorno.IdBeneficiario.ToString();
                txtDataProcessamentoRetorno.Text = dtoRetorno.DataProcessamento.HasValue ? dtoRetorno.DataProcessamento.Value.ToShortDateString() : string.Empty;
                txtSituacaoProcessamento.Text = Util.Utils.GetEnumDescription((Techne.Lyceum.RN.CartaoEstudante.Enum.SituacaoProcessamentoEnum) Convert.ToInt32(dtoRetorno.SituacaoProcessamento));
                txtDataInclusaoRetorno.Text = dtoRetorno.DataInclusao == DateTime.MinValue ? string.Empty : dtoRetorno.DataInclusao.ToShortDateString();

                if (dtoRetorno.Criticas != null && dtoRetorno.Criticas.Count > 0)
                {
                    grdRetornoCritica.DataSource = dtoRetorno.Criticas;
                    grdRetornoCritica.DataBind();
                }
            }
        }

        private void PreencheDetalhesRemessa(int remessaId, string codigoSolicitacao)
        {
            RemessaService remessaService = RemessaService.Instancia;
            RemessaDTO remessaDTO = remessaService.ObtemDetalhesRemessaPor(remessaId);

            CarregaDadosFotoPessoa(remessaDTO.Foto);

            txtCodigoRemessa.Text = remessaId.ToString();
            txtLote.Text = remessaDTO.NomeLoteRemessa;
            txtSolicitacao.Text = codigoSolicitacao;
            txtOperadora.Text = remessaDTO.Operadora.ToString().ToUpper();
            txtDataInclusaoRemessa.Text = remessaDTO.DataInclusao == DateTime.MinValue ? string.Empty : remessaDTO.DataInclusao.ToShortDateString();

            txtMatricula.Text = remessaDTO.MatriculaAluno;
            txtNomeAluno.Text = remessaDTO.NomeAluno;
            txtDataNascimento.Text = remessaDTO.DataNascimento == DateTime.MinValue ? string.Empty : remessaDTO.DataNascimento.ToShortDateString();
            txtNomeMae.Text = remessaDTO.NomeMae;
            txtNomePai.Text = remessaDTO.NomePai;
            txtCPF.Text = remessaDTO.Cpf;
            txtRGNum.Text = remessaDTO.NumeroRG;
            txtRGUF.Text = remessaDTO.UFRg;
            txtRGEmissor.Text = remessaDTO.OrgaoEmissorRG;
            txtRGDataEmissao.Text = remessaDTO.DataExpedicaoRG == DateTime.MinValue ? string.Empty : remessaDTO.DataExpedicaoRG.ToShortDateString();
            txtCep.Text = remessaDTO.Cep;
            txtLogradouro.Text = remessaDTO.Endereco;
            txtEnderecoNumero.Text = remessaDTO.NumeroEndereco;
            txtComplemento.Text = remessaDTO.ComplementoEndereco;
            txtBairro.Text = remessaDTO.Bairro;
            txtGratuidade.Text = remessaDTO.Gratuidade;
            txtTrem.Text = remessaDTO.ModalTrem;
            txtOnibus.Text = remessaDTO.ModalOnibus;
            txtMetro.Text = remessaDTO.ModalMetro;
            txtBarcas.Text = remessaDTO.ModalBarcas;
            txtUnidadeEnsino.Text = remessaDTO.UnidadeEnsino;
            txtTurno.Text = remessaDTO.Turno;
            txtTurma.Text = remessaDTO.Turma;
            txtSerie.Text = remessaDTO.Serie.HasValue ? remessaDTO.Serie.Value.ToString() : string.Empty;
            txtEmailInterno.Text = remessaDTO.EmailInterno;
            txtLoginCartao.Text = remessaDTO.LoginRioCard;
            txtDataEnvioLogRemessa.Text = remessaDTO.DataEnvioLoteRemessa.HasValue && remessaDTO.DataEnvioLoteRemessa != DateTime.MinValue ? remessaDTO.DataEnvioLoteRemessa.Value.ToShortDateString() : string.Empty;
            txtDataUltimaAtualizacao.Text = remessaDTO.DataUltimaAtualizacao.HasValue && remessaDTO.DataUltimaAtualizacao != DateTime.MinValue ? remessaDTO.DataUltimaAtualizacao.Value.ToShortDateString() : string.Empty;

            if (!String.IsNullOrEmpty(remessaDTO.EndMunicipio))
            {
                TceMunicipio municipio = Municipio.ObtemPorDePara(remessaDTO.EndMunicipio);
                txtEstado.Text = municipio.UF ?? "";
                txtMunicipio.Text = municipio.Nome ?? "";
            }
        }

        private void CarregaGrid()
        {
            try
            {
                RemessaService rnRemessaService = RemessaService.Instancia;
                AcompanhamentoRemessaFilterDTO filtro = new AcompanhamentoRemessaFilterDTO();
                filtro.Aluno = tseAluno.Text;
                filtro.DataEnvioInicio = dtEnvioInicial.Date;
                filtro.DataEnvioFim = dtEnvioFinal.Date;
                filtro.UnidadeEnsino = Convert.ToString(tseUnidadeEnsino.Value);
                filtro.Municipio = Convert.ToString(tseMunicipio.Value);
                filtro.IdRegional = Convert.ToInt32(tseRegional.Value);

                filtro.TipoSituacaoProcessamento = (Techne.Lyceum.RN.CartaoEstudante.Enum.TipoSituacaoProcessamentoEnum) int.Parse(rblPossuiCriticas.SelectedValue);


                grdProcessamentoRemessa.DataSource = rnRemessaService.ListaProcessamento(filtro);
                grdProcessamentoRemessa.DataBind();

                if (grdProcessamentoRemessa.VisibleRowCount > 0)
                {
                    grdProcessamentoRemessa.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existem remessas para os parâmetros de pesquisa informados.";
                    grdProcessamentoRemessa.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosFotoPessoa(byte[] foto)
        {
            if (foto == null)
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
                    Image.FromStream(new MemoryStream(foto));
                    bimgFotoPessoa.ContentBytes = foto;
                }
                catch
                {
                    bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                    bimgFotoPessoa.EmptyImage.AlternateText = "Foto inválida";
                    bimgFotoPessoa.ContentBytes = null;
                }
            }
        }
    }
}
