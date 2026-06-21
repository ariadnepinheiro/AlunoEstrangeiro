using System;
using System.Data;
using System.Web.UI;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Web;
using System.Collections.Generic;
using System.Reflection;
using Techne.Web;
using System.Configuration;
using System.Collections.Specialized;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Collections;

namespace Techne.Lyceum.Net.Menu
{
    public partial class Popup : System.Web.UI.UserControl
    {
        protected void Page_Init()
        {
            TPage.TituloGrid(this.grdVagas, string.Empty);
            TPage.TituloGrid(this.grdConvocacaoSemEmail, "Inscritos para efetuar contato.(Problema Envio de E-mail)");
        }

        protected void Page_Load()
        {

            try
            {
                RN.Lotacao rnLotacao = new Lotacao();
                RN.Docentes rnDocentes = new Docentes();
                RN.Transferencia rnTransferenciaAluno = new Transferencia();
                string setor = "";
                string escola = "";
                string[] unidadeEscolar = { "", "" };
                DataTable dtVagas = new DataTable();
                var temTransferenciaPendente = false;
                var temTransferenciaAlunoPendente = false;
                var temVagaDisponivel = false;
                var diretor = false;
                string matriculaUsuario;

                DataTable temAlocacao = new DataTable();
                DataTable temCargaHoraria = new DataTable();
                DataTable temPedenciaCadastro = new DataTable();

                var ExibeSAEB = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ExibeSAEB"]);

                var usuario = HttpContext.Current.User.Identity.Name;               


                if (!IsPostBack)
                {
                    if (usuario.Contains("/"))
                    {
                        matriculaUsuario = rnDocentes.ObtemMatriculaPorIdVinculo(usuario);
                    }
                    else
                    {
                        matriculaUsuario = usuario;
                    }

                    //verificar se agente responsável tem transferência pendente de patrimômio
                    var rnAgenteResponsavel = new RN.Patrimonio.AgenteResponsavel();
                    var rnTransferencia = new RN.Patrimonio.Transferencia();
                    var dtSetor = rnAgenteResponsavel.ListaPorMatricula(usuario);
                    if (dtSetor != null && dtSetor.Rows.Count > 0)
                        foreach (DataRow row in dtSetor.Rows)
                        {
                            setor = Convert.ToString(row["SETOR"]);
                            temTransferenciaPendente = rnTransferencia.VerificaSeTemTransferenciaPendentePor(setor);
                            if (temTransferenciaPendente)
                                break;
                        }

                    //verificar unidade do diretor
                    unidadeEscolar = rnLotacao.ObtemUnidadeDiretorPor(matriculaUsuario);
                    diretor = !unidadeEscolar[0].IsNullOrEmptyOrWhiteSpace(); //Marca caso seja diretor

                    if (unidadeEscolar[0].IsNullOrEmptyOrWhiteSpace())
                    { 
                        //Verifica unidade do adjunto ou secretraio
                        unidadeEscolar = rnLotacao.ObtemUnidadeAdjuntoSecretarioPor(matriculaUsuario);
                    }                

                    if (!unidadeEscolar[0].IsNullOrEmptyOrWhiteSpace())
                    {
                        //Caso seja diretor alimentar as sessoes
                        var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                        sessao.Escola = unidadeEscolar[0];
                        sessao.Municipio = unidadeEscolar[2];
                        sessao.Coordenadoria = unidadeEscolar[3];

                        //Verifica se tem solicitação de transferencia de aluno pendentes
                        temTransferenciaAlunoPendente = rnTransferenciaAluno.ExisteSolicitacaoPendentePor(unidadeEscolar[0].ToString());

                        hdnUnidade.Value = unidadeEscolar[0];
                        escola = unidadeEscolar[1];
                        odsVagas.Select();
                        odsVagas.DataBind();
                        grdVagas.DataBind();
                        odsConvocacaoSemEmail.Select();
                        odsConvocacaoSemEmail.DataBind();
                        grdConvocacaoSemEmail.DataBind();
                        if (grdVagas.VisibleRowCount > 0)
                            temVagaDisponivel = true;
                    }

                    //se quaisquer uma das afirmativas acima for positiva, mostrar popup
                    if (temTransferenciaPendente || temVagaDisponivel || temTransferenciaAlunoPendente)
                    {
                        plaTransferenciaAluno.Visible = temTransferenciaAlunoPendente;
                        plaTransferencia.Visible = temTransferenciaPendente;
                        plaVagas.Visible = temVagaDisponivel && diretor;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.setTimeout(function() { pcPopup.Show(); }, 1000);", true);
                    }
                    if (setor == "")
                    {
                        String[] dtfuncao = rnAgenteResponsavel.VerificaMatricula(usuario);
                        setor = dtfuncao[0];
                        escola = dtfuncao[1];
                    }
                    if (!(setor == null))
                    {

                        //Verifica se existem pendencias para a Carga horaria , Turma e Cadastro

                        //inicializa relatorio
                        var script = string.Empty;

                        IDictionary<string, string> pares = new Dictionary<string, string>();
                        pares.Add("setor", setor);
                        pares.Add("escola", escola);
                        btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=Pendencia_CHTC&grp=QHI&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=no, width=860, height=600, resizable=yes'); return false;");

                        //Turmas com Pendência de Alocação (Incompleta/Sem alocação)
                        temAlocacao = rnAgenteResponsavel.VerificaAlocacao(setor);

                        //Carga Horária Livre
                        temCargaHoraria = rnAgenteResponsavel.VerificaCargahoraria(setor);

                        //Pendência de Cadastro Docente
                        temPedenciaCadastro = rnAgenteResponsavel.VerificaPendenciaCadastro(setor);

                        if ((temAlocacao != null && temAlocacao.Rows.Count > 0) ||
                            (temCargaHoraria != null && temCargaHoraria.Rows.Count > 0) ||
                            temPedenciaCadastro != null && temPedenciaCadastro.Rows.Count > 0)
                        {
                            var textolbl = "";
                            if (temAlocacao != null && temAlocacao.Rows.Count > 0) { textolbl = " Alocação "; }
                            if (temCargaHoraria != null && temCargaHoraria.Rows.Count > 0) { if (textolbl == "") { textolbl = " Carga Horária Livre"; } else { textolbl = textolbl + " /Carga Horária Livre"; } }
                            if (temPedenciaCadastro != null && temPedenciaCadastro.Rows.Count > 0) { if (textolbl == "") { textolbl = " Cadastro "; } else { textolbl = textolbl + " /Cadastro "; } }

                            LblText.Text = "Caro Diretor, existe(m) pendência(s) de" + textolbl;
                            PlaCHTC.Visible = true;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.setTimeout(function() { pcPopup.Show(); }, 1000);", true);
                        }
                    }

                    if (!ExibeSAEB)
                    {
                        if (unidadeEscolar[0].IsNullOrEmptyOrWhiteSpace())
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.setTimeout(function() { pcPopup.Hide(); }, 1000);", true);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public object Listar(object unidade)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();

            var ue = Convert.ToString(unidade);

            if (!ue.IsNullOrEmptyOrWhiteSpace())
            {
                return rnControleVaga.ListaVagaMatriculaFacilPor(ue);
            }

            return null;
        }
        protected void MostraRelatorio(object sender, EventArgs e)
        {
            var script = string.Empty;

            IDictionary<string, string> pares = new Dictionary<string, string>();
            pares.Add("setor", "180777");
            pares.Add("escola", "teste ");
            btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=Pendencia_CHTC&grp=QHI&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");

        }

        public object ListarConvocacaoSemEmail(object unidade)
        {
            RN.Matriculas.ConvocacaoSemEmail rnConvocacaoSemEmail = new Techne.Lyceum.RN.Matriculas.ConvocacaoSemEmail();
            var ue = Convert.ToString(unidade);

            if (!ue.IsNullOrEmptyOrWhiteSpace())
            {
                return rnConvocacaoSemEmail.ListaConvocacaoSemEmailPor(ue);
            }

            return null;
        }
    }
}