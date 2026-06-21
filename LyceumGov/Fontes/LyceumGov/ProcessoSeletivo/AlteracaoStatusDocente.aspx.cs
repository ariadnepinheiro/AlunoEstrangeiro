using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using Techne.Controls;
using Techne.Data;
using Seeduc.Infra.Data;
using Techne.Web;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.RecursosHumanos;
using Techne.Lyceum.RN.ContratoTemporario;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/AlteracaoStatusDocente.aspx"),
    ControlText("AlteracaoStatusDocente"),
    Title("Alteração Status Docente"),]
    public partial class AlteracaoStatusDocente : TPage
    {
        public class StringValue : Attribute
        {
            public string Value { get; private set; }

            public StringValue(string value)
            {
                this.Value = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull)
            {
                tseCandidato.Enabled = true;
            }
            else
            {
                tseCandidato.Enabled = false;
            }
        }

        protected void tseConcurso_Changed(object sender, EventArgs e)
        {
            try
            {
                pnGrupo.Visible = false;

                if (!tseConcurso.IsValidDBValue || tseConcurso.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor consultar processo seletivo.";
                    tseCandidato.Enabled = false;
                    return;
                }

                tseCandidato.Enabled = true;
                lblMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCandidato_Changed(object sender, EventArgs e)
        {
            if (!tseConcurso.IsValidDBValue || tseConcurso.DBValue.IsNull)
            {
                lblMensagem.Text = "Favor consultar processo seletivo primeiro.";
                return;
            }

            pnGrupo.Visible = true;

            DocenteCandidato rnDocenteCandidato = new DocenteCandidato();
            DataTable dtDadosCandidatoDocente = new DataTable();

            Ly_candidato_docente dtCandidatoDocente = new Ly_candidato_docente();

            Ly_candidato_docente.Row dadosCandidatoDocente = dtCandidatoDocente.NewRow();
            dadosCandidatoDocente.Concurso = tseConcurso.DBValue.ToString();
            dadosCandidatoDocente.Candidato = tseCandidato.DBValue.ToString();

            dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor(dadosCandidatoDocente.Candidato,dadosCandidatoDocente.Concurso);

            if (dtDadosCandidatoDocente.Rows.Count > 0)
            {
                CarregaDadosCandidatoDocenteNovo(dtDadosCandidatoDocente);
            }
        }

        private void CarregaDadosCandidatoDocenteNovo(DataTable dadosCandidatoDocente)
        {
            if (dadosCandidatoDocente == null || dadosCandidatoDocente.Rows.Count == 0)
            {
                lblMensagem.Text = "Nenhum candidato encontrado.";
                return;
            }

            lblMensagem.Text = "";
            txtObservacao.Text = "";
            tseCategoria.Value = null;
            tseFuncaoLotacao.Value = null;
            RN.ProcessoSeletivo rnProcessoSeletivo = new RN.ProcessoSeletivo();

            DataTable dtDocente = new DataTable();
            RN.Docentes rnDocente = new Docentes();

            txtIdvinculo.Text = dadosCandidatoDocente.Rows[0]["IDVINCULO"].ToString();

            txtDataApresentacao.Text = dadosCandidatoDocente.Rows[0]["DTAPRESENTACAO"].ToString();
            txtHoraApresentacao.Text = dadosCandidatoDocente.Rows[0]["HORAAPRESENTACAO"].ToString();

            int situacaoInt = Convert.ToInt32(dadosCandidatoDocente.Rows[0]["SITUACAO"]);
            var situacaoEnum = (DocenteCandidato.EnumSituacao)situacaoInt;
            txtSituacao.Text = GetStringValue(situacaoEnum);

            CarregarDropdownSituacao(situacaoInt);

            if (situacaoEnum == DocenteCandidato.EnumSituacao.Convocado)
            {
                trDataApresentacao.Visible = true;
                trHoraApresentacao.Visible = true;
                trPontuacao.Visible = true;
                trCargo.Visible = false;
                trFuncao.Visible = false;
                trChRegencia.Visible = false;
                trChNormal.Visible = false;
                trChGLP.Visible = false;

                trCargoAlterar.Visible = false;
                trFuncaoAlterar.Visible = false;
            }
            else if (situacaoEnum == DocenteCandidato.EnumSituacao.MigracaoConcluida)
            {
                trDataApresentacao.Visible = false;
                trHoraApresentacao.Visible = false;
                trPontuacao.Visible = false;
                trCargo.Visible = true;
                trFuncao.Visible = true;
                trChRegencia.Visible = true;
                trChNormal.Visible = true;
                trChGLP.Visible = true;

                trCargoAlterar.Visible = true;
                trFuncaoAlterar.Visible = true;
            }
            else
            {
                trDataApresentacao.Visible = false;
                trHoraApresentacao.Visible = false;
                trPontuacao.Visible = false;
                trCargo.Visible = true;
                trFuncao.Visible = true;
                trChRegencia.Visible = true;
                trChNormal.Visible = true;
                trChGLP.Visible = true;

                trCargoAlterar.Visible = false;
                trFuncaoAlterar.Visible = false;
            }

            decimal pontuacaotitulacao = DocenteCandidatoTitulacao.ConsultarPontuacaoTitulacaoCandidato(tseCandidato.DBValue.ToString(),tseConcurso.DBValue.ToString());

            decimal pontuacaoExperiencia = DocenteCandidatoExperiencia.ConsultarPontuacaoExperienciaCandidato(tseCandidato.DBValue.ToString(),tseConcurso.DBValue.ToString());

            if (dadosCandidatoDocente.Rows[0]["FUNCAODIRETOR"].ToString() == "True") 
                txtPontuacao.Text = (pontuacaotitulacao + pontuacaoExperiencia + 1 + Convert.ToDecimal(Convert.ToInt32(dadosCandidatoDocente.Rows[0]["QUANTIDADEANOSGLP"]) * 0.5)).ToString();

            else 
                txtPontuacao.Text = (pontuacaotitulacao  + pontuacaoExperiencia + Convert.ToDecimal(Convert.ToInt32(dadosCandidatoDocente.Rows[0]["QUANTIDADEANOSGLP"]) * 0.5)).ToString();

            dtDocente = rnDocente.ObtemDadosDocenteMigracaoPor(DateTime.Now.Year, dadosCandidatoDocente.Rows[0]["IDVINCULO"].ToString());

            if (dtDocente.Rows.Count == 0) lblMensagem.Text = "Informações do docente não encontrado ou não ativo.";

            else
            {
                txtCargo.Text = Convert.ToString(dtDocente.Rows[0]["cargo"]);
                txtFuncao.Text = Convert.ToString(dtDocente.Rows[0]["funcao"]);
                txtChRegencia.Text = Convert.ToString(dtDocente.Rows[0]["ch_regencia"]);
                txtChNormal.Text = Convert.ToString(dtDocente.Rows[0]["tol_normal"]);
                using (DataContext contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    RN.AulaDocente rnAulaDocente = new AulaDocente();

                    var qtdGlp = rnAulaDocente.ObtemQuantidadeAulasGLPsAtivasDocentePor(
                        contexto,
                        dadosCandidatoDocente.Rows[0]["MATRICULA"].ToString()
                    );

                    txtChGlp.Text = qtdGlp == null ? "0" : qtdGlp.ToString();
                }
            }
        }

        private void CarregarDropdownSituacao(int situacaoAtual)
        {
            var lista = new List<object>();

            foreach (DocenteCandidato.EnumSituacao item in Enum.GetValues(typeof(DocenteCandidato.EnumSituacao)))
            {
                int valor = (int)item;

                if (situacaoAtual == 8)
                {
                    if (valor == 2 || valor == 8)
                        continue;
                }
                else
                {
                    if (valor != 1)
                        continue;
                }

                lista.Add(new
                {
                    statusid = valor,
                    descricao = GetStringValue(item)
                });
            }

            ddlSituacao.DataSource = lista;
            ddlSituacao.DataValueField = "statusid";
            ddlSituacao.DataTextField = "descricao";
            ddlSituacao.DataBind();

            ddlSituacao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Selecione --", ""));
        }

        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            pucConfirmar.ShowOnPageLoad = true;
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            DocenteCandidato rnDocenteCandidato = new DocenteCandidato();

            string candidato = tseCandidato.DBValue.ToString();
            string concurso = tseConcurso.DBValue.ToString();

            DataTable dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor(candidato, concurso);

            if (dtDadosCandidatoDocente == null || dtDadosCandidatoDocente.Rows.Count == 0)
            {
                lblMensagem.Text = "Candidato não encontrado.";
                pucConfirmar.ShowOnPageLoad = false;
                return;
            }

            DataRow row = dtDadosCandidatoDocente.Rows[0];

            int numFunc = row["NUMFUNC"] != DBNull.Value? Convert.ToInt32(row["NUMFUNC"]): 0;

            if (Convert.ToInt32(txtChNormal.Text) > 12)
            {
                lblMensagem.Text = "O Docente possui mais de 12 tempos em sua matricula.";
                pucConfirmar.ShowOnPageLoad = false;
                return;
            }

            if (txtObservacao.Text.Length > 500)
            {
                lblMensagem.Text = "Observação acima do limite de 500 caracteres";
                pucConfirmar.ShowOnPageLoad = false;
                return;
            }

            if (ddlSituacao.SelectedValue != "")
            {
                if (txtSituacao.Text == "MigracaoConcluida")
                {
                    try
                    {
                        string categoria = tseCategoria.Value != null ? tseCategoria.Value.ToString() : "";
                        string funcao = tseFuncaoLotacao.Value != null ? tseFuncaoLotacao.Value.ToString() : "";

                        if (string.IsNullOrEmpty(categoria))
                        {
                            lblMensagem.Text = "Selecione o Cargo.";
                            pucConfirmar.ShowOnPageLoad = false;
                            return;
                        }

                        if (string.IsNullOrEmpty(funcao))
                        {
                            lblMensagem.Text = "Selecione a Função.";
                            pucConfirmar.ShowOnPageLoad = false;
                            return;
                        }

                        using (DataContext contexto = DataContextBuilder.FromLyceum.UsingLock())
                        {
                            RN.RecursosHumanos.DTO.DadosAlocacaoMigracao dados = new RN.RecursosHumanos.DTO.DadosAlocacaoMigracao();
                            List<RN.RecursosHumanos.DTO.DadosTurmaAlocacao> dadosAlocacao = new List<RN.RecursosHumanos.DTO.DadosTurmaAlocacao>();
                            RN.Docentes rnDocente = new Docentes();
                            RN.Lotacao rnLotacao = new Lotacao();
                            DataTable dtDocente = new DataTable();

                            dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor(candidato, concurso);

                            if (dtDadosCandidatoDocente == null || dtDadosCandidatoDocente.Rows.Count == 0)
                            {
                                lblMensagem.Text = "Candidato não encontrado.";
                                return;
                            }

                            dados.Pessoa = row["PESSOA"] != DBNull.Value ? Convert.ToInt32(row["PESSOA"]) : 0;
                            dados.Aulas = dadosAlocacao;
                            dados.Categoria = (!tseCategoria.DBValue.IsNull && tseCategoria.IsValidDBValue)? tseCategoria.DBValue.ToString(): null;
                            dados.CategoriaAnterior = row["CATEGORIA"] != DBNull.Value? row["CATEGORIA"].ToString(): null;
                            dados.DocenteCandidatoId = row["DOCENTECANDIDATOID"] != DBNull.Value? Convert.ToInt32(row["DOCENTECANDIDATOID"]): 0;
                            dados.Funcao = (!tseFuncaoLotacao.DBValue.IsNull && tseFuncaoLotacao.IsValidDBValue)? tseFuncaoLotacao.DBValue.ToString(): null;
                            dados.MatriculaDocente = row["MATRICULA"] != DBNull.Value? row["MATRICULA"].ToString(): null;
                            dados.NumFunc = row["NUMFUNC"] != DBNull.Value? Convert.ToInt32(row["NUMFUNC"]): 0;
                            dados.UsuarioId = User.Identity.Name;
                            dados.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace()? Convert.ToInt32(ddlAno.SelectedValue): -1;
                            dados.DataConvocacaoDO = null;
                            dados.Observacao = (txtObservacao.Text == null || txtObservacao.Text.Trim() == "")? null : txtObservacao.Text.Trim();

                            rnDocente.AtualizaCargo(contexto, dados.NumFunc, dados.Categoria, dados.UsuarioId);
                            RN.Entidades.LyLotacao lotacaoBanco = rnLotacao.ObtemLotacaoAtivaPor(contexto, dados.MatriculaDocente);

                            if (lotacaoBanco == null)
                            {
                                lblMensagem.Text = "Lotação ativa não encontrada.";
                                return;
                            }

                            RN.Entidades.LyLotacao lotacao = new RN.Entidades.LyLotacao();

                            lotacao.Ordem = rnLotacao.ObtemProximaOrdemPor(contexto, dados.MatriculaDocente);
                            lotacao.Pessoa = dados.Pessoa;
                            lotacao.Matricula = dados.MatriculaDocente;
                            lotacao.Funcao = dados.Funcao;
                            lotacao.DataNomeacao = DateTime.Now.Date;
                            lotacao.UnidadeEns = lotacaoBanco.UnidadeEns;
                            lotacao.UnidadeFis = lotacaoBanco.UnidadeFis;
                            lotacao.Setor = lotacaoBanco.Setor;
                            lotacao.Categoria = dados.Categoria;
                            lotacao.Nucleo = lotacaoBanco.Nucleo;
                            lotacao.Usuario = dados.UsuarioId;
                            lotacao.DataAtualizacao = DateTime.Now;

                            rnLotacao.DesativaLotacaoMigracao(contexto,lotacao.DataNomeacao.AddDays(-1),dados.UsuarioId,dados.MatriculaDocente);
                            rnLotacao.Insere(contexto, lotacao);

                            if (ddlSituacao.SelectedValue == "1")
                            {
                                rnDocenteCandidato.LimparConvocacao(
                                    dados.DocenteCandidatoId.ToString(),
                                    tseConcurso.DBValue.ToString(),
                                    ddlSituacao.SelectedValue,
                                    dados.Observacao
                                );
                            }

                            rnDocenteCandidato.AtualizaSituacaoComObservacao(contexto,dados.DocenteCandidatoId,Convert.ToInt32(ddlSituacao.SelectedValue),dados.UsuarioId,dados.DataConvocacaoDO,dados.Observacao);

                            dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor(
                                tseCandidato.DBValue.ToString(),
                                tseConcurso.DBValue.ToString()
                            );

                            CarregaDadosCandidatoDocenteNovo(dtDadosCandidatoDocente);

                            ScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "alerta",
                                "alert('Processo Realizado com Sucesso');",
                                true
                            );

                            string idVinculo = row["IDVINCULO"] != DBNull.Value? row["IDVINCULO"].ToString(): null;
                            if (string.IsNullOrEmpty(idVinculo))
                            {
                                lblMensagem.Text = "IDVINCULO não encontrado.";
                                return;
                            }

                            dtDocente = rnDocente.ObtemDadosDocenteMigracaoPor(DateTime.Now.Year, idVinculo);
                            if (dtDocente == null || dtDocente.Rows.Count == 0)
                            {
                                lblMensagem.Text = "Informações do docente não encontrado ou não ativo.";
                            }
                            else
                            {
                                DataRow rowDoc = dtDocente.Rows[0];
                                txtCargo.Text = rowDoc["cargo"] != DBNull.Value ? rowDoc["cargo"].ToString() : "";
                                txtFuncao.Text = rowDoc["funcao"] != DBNull.Value ? rowDoc["funcao"].ToString() : "";
                                txtChRegencia.Text = rowDoc["ch_regencia"] != DBNull.Value ? rowDoc["ch_regencia"].ToString() : "";
                                txtChNormal.Text = rowDoc["tol_normal"] != DBNull.Value ? rowDoc["tol_normal"].ToString() : "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro: " + ex.ToString();
                    }
                }
                else
                {
                    if (ddlSituacao.SelectedValue != "1") return;

                    Ly_candidato_docente dtCandidatoDocente = new Ly_candidato_docente();

                    Ly_candidato_docente.Row dadosCandidatoDocente = dtCandidatoDocente.NewRow();
                    dadosCandidatoDocente.Concurso = tseConcurso.DBValue.ToString();

                    dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor(
                        tseCandidato.DBValue.ToString(),
                        tseConcurso.DBValue.ToString()
                    );

                    if (dtDadosCandidatoDocente.Rows.Count > 0)
                    {
                        string observacao = (txtObservacao.Text == null || txtObservacao.Text.Trim() == "") ? null : txtObservacao.Text.Trim();
                        rnDocenteCandidato.LimparConvocacao(
                            dtDadosCandidatoDocente.Rows[0]["DOCENTECANDIDATOID"].ToString(),
                            tseConcurso.DBValue.ToString(),
                            ddlSituacao.SelectedValue,
                            observacao
                        );

                        dtDadosCandidatoDocente = rnDocenteCandidato.ObtemPor(
                            tseCandidato.DBValue.ToString(),
                            tseConcurso.DBValue.ToString()
                        );
                        CarregaDadosCandidatoDocenteNovo(dtDadosCandidatoDocente);

                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "alerta",
                            "alert('Processo Realizado com Sucesso');",
                            true
                        );
                    } 
                }
                
            }
            else
            {
                lblMensagem.Text = "Selecione uma situação";
            }
            pucConfirmar.ShowOnPageLoad = false;
        }

        public static string GetStringValue(Enum value)
        {
            var type = value.GetType();
            var field = type.GetField(value.ToString());

            var attrs = field.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            return (attrs != null && attrs.Length > 0)
                ? attrs[0].Value
                : value.ToString();
        }
    }
}