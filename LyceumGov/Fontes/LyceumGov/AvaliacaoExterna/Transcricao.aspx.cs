using System;
using System.Data;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.WebControls;
using DevExpress.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN.AvaliacaoExterna.Entidades;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/Transcricao.aspx")]
    [ControlText("Transcrição das Respostas")]
    [Title("Transcrição das Respostas")]
    public partial class Transcricao : TPage
    {
        public readonly RN.UnidadeEnsino rnUnidadeEnsino;
        public readonly RN.AvaliacaoExterna.Etapa rnEtapa;
        public readonly RN.AvaliacaoExterna.TranscricaoTurma rnTranscricaoTurma;
        public readonly RN.AvaliacaoExterna.Questao rnQuestao;
        public readonly RN.AvaliacaoExterna.AlunoParticipante rnAluno;
        public readonly RN.AvaliacaoExterna.SituacaoParticipante rnSituacaoParticipante;
        public readonly RN.AvaliacaoExterna.Componente rnComponente;
        public readonly RN.AvaliacaoExterna.Prova rnProva;

        public DataTable dtAlunos;

        public Transcricao()
        {
            rnUnidadeEnsino = new Techne.Lyceum.RN.UnidadeEnsino();
            rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Etapa();
            rnTranscricaoTurma = new Techne.Lyceum.RN.AvaliacaoExterna.TranscricaoTurma();
            rnQuestao = new Techne.Lyceum.RN.AvaliacaoExterna.Questao();
            rnAluno = new Techne.Lyceum.RN.AvaliacaoExterna.AlunoParticipante();
            rnSituacaoParticipante = new Techne.Lyceum.RN.AvaliacaoExterna.SituacaoParticipante();
            rnComponente = new Techne.Lyceum.RN.AvaliacaoExterna.Componente();
            rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();
        }

        public int TranscricaoId 
        {
            get
            {
                //return rnAluno.ObtemTranscricaoId(EtapaId, Turma);

                int transcricaoId;
                int.TryParse(hdnTranscricaoId.Value, out transcricaoId);

                if (transcricaoId == 0)
                {
                    string key = this.QueryStringDecodificada["transcricaoId"];
                    int.TryParse(key ?? "0", out transcricaoId);
                    hdnTranscricaoId.Value = transcricaoId.ToString();
                }

                if (transcricaoId == 0)
                {
                    transcricaoId = rnTranscricaoTurma.ObtemIdPor(EtapaId, Turma, Ano, Semestre);
                    hdnTranscricaoId.Value = transcricaoId.ToString();
                }

                return transcricaoId;
            }
        }       

        public int EtapaId
        {
            get
            {
                int etapaId;
                string key = this.QueryStringDecodificada["etapaId"];
                int.TryParse(key ?? "0", out etapaId);
                return etapaId;
            }
        }

        public int Ano
        {
            get
            {
                int ano;
                string key = this.QueryStringDecodificada["ano"];
                int.TryParse(key ?? "0", out ano);
                return ano;
            }
        }

        public int Semestre
        {
            get
            {
                int semestre;
                string key = this.QueryStringDecodificada["semestre"]; 
                int.TryParse(key ?? "-1", out semestre);
                return semestre;
            }
        }

        public int ProvaId
        {
            get
            {
                int provaId;
                string key = this.QueryStringDecodificada["provaId"];
                int.TryParse(key ?? "0", out provaId);
                return provaId;
            }
        } 

        public string Curso
        {
            get
            {
                DataRow etapa = rnEtapa.ObtemPorEtapaId(EtapaId);
                if (etapa == null)
                    return null;
                if (etapa["CURSO"] == DBNull.Value)
                    return null;
                return etapa["CURSO"].ToString();
            }
        }

        public string NomeCurso
        {
            get
            {
                DataRow etapa = rnEtapa.ObtemPorEtapaId(EtapaId);
                if (etapa == null)
                    return null;
                if (etapa["NOMECURSO"] == DBNull.Value)
                    return null;
                return etapa["NOMECURSO"].ToString();
            }
        }
        
        public int Serie 
        {
            get
            {
                DataRow etapa = rnEtapa.ObtemPorEtapaId(EtapaId);
                if (etapa == null)
                    return 0;
                if (etapa["SERIE"] == DBNull.Value)
                    return 0;
                return Convert.ToInt32(etapa["SERIE"]);
            }
        }       

        public string Etapa
        {
            get
            {
                return string.Format("{2}º ano do curso {0} - {1}", Curso, NomeCurso, Serie);
            }
        }

        public int AvaliacaoId
        {
            get
            {
                int avaliacaoId;
                string key = this.QueryStringDecodificada["avaliacaoId"];
                int.TryParse(key ?? "0", out avaliacaoId);
                return avaliacaoId;
            }
        }

        public string AvaliacaoDescricao
        {
            get
            {
                string avaliacao = this.QueryStringDecodificada["avaliacao"];
                return Ano.ToString() + " - " + avaliacao.ToString();
            }
        }

        public string ProvaDescricao
        {
            get
            {
                return this.QueryStringDecodificada["prova"];
            }
        }

        public int Unidade_Ens
        {
            get
            {
                int unidade_ens;
                string key = this.QueryStringDecodificada["unidade_ens"];
                int.TryParse(key ?? "0", out unidade_ens);
                return unidade_ens;
            }
        }

        public string Unidade_Nome
        {
            get
            {
                DataRow unidadeEnsino = rnUnidadeEnsino.ObtemPorUnidadeEns(Unidade_Ens);
                if (unidadeEnsino == null)
                    return null;
                if (unidadeEnsino["NOME_COMP"] == DBNull.Value)
                    return null;
                return unidadeEnsino["NOME_COMP"].ToString();
            }
        }

        public string Unidade_Regional
        {
            get
            {
                DataRow unidadeEnsino = rnUnidadeEnsino.ObtemPorUnidadeEns(Unidade_Ens);
                if (unidadeEnsino == null)
                    return null;
                if (unidadeEnsino["REGIONAL"] == DBNull.Value)
                    return null;
                return unidadeEnsino["REGIONAL"].ToString();
            }
        }

        public string Unidade_Municipio
        {
            get
            {
                DataRow unidadeEnsino = rnUnidadeEnsino.ObtemPorUnidadeEns(Unidade_Ens);
                if (unidadeEnsino == null)
                    return null;
                if (unidadeEnsino["NOMEMUNICIPIO"] == DBNull.Value)
                    return null;
                return unidadeEnsino["NOMEMUNICIPIO"].ToString();
            }
        }

        public string Turma
        {
            get
            {
                return this.QueryStringDecodificada["turma"];
            }
        }

        private bool SomenteLeitura
        {
            get
            {
                return !(Permission.AllowDelete && Permission.AllowInsert && Permission.AllowUpdate);
            }
        }

        public DataTable ListaSituacaoParticipante()
        {
            DataTable lista = rnSituacaoParticipante.ListaAtivo();
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTranscricao, "Transcrição");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                /*
            
                ATENÇÃO: grid com colunas montadas dinamicamente, cada uma com DataItemTemplate definido
                - Para estes casos, o ideal é montar dinamicamente as colunas ANTES de qualquer evento de
                renderização (PreRender e PreRenderComplete não funcionarão para montagem), e também 
                desabilitar o ViewState do grid (EnableViewState = false).
                https://www.devexpress.com/Support/Center/Question/Details/T109412/aspxgridview-the-findrowcelltemplatecontrol-method-returns-null-value-for-dynamically
                
                - Como o ViewState do grid estará desabilitado, então para cada postback as colunas dinâmicas
                deverão ser montadas novamente. Por este motivo o Page_Load não deve ser pulado a cada postback.
                //if (IsPostBack)
                //    return;
                
                */

                //Obter uma lista de status de participação do aluno, para usarmos na montagem do grid dinâmico
                DataTable listaSituacaoParticipante = ListaSituacaoParticipante();

                //percorrer uma lista com todos os componentes da provaId especificada na querystring,
                //e para cada componente, adicionar uma lista de colunas ref. a ele próprio
                DataTable componentes = rnComponente.ListaPorProva(ProvaId); 
                IList<GridViewDataColumn> colunasDinamicas = new List<GridViewDataColumn>();
                for (int i = 0; i < componentes.Rows.Count; i++)
                {
                    //obter o id do componente da etapa
                    int componenteId = Convert.ToInt32(componentes.Rows[i]["COMPONENTEID"]);

                    //gerar a lista de respostas, com data de participação e status de participação, e agregá-la à outra lista de colunas dinâmicas
                    colunasDinamicas = AgregarColunasQuestoesPorComponente(colunasDinamicas, componenteId, listaSituacaoParticipante);
                }

                //com todas as colunas dinâmicas já geradas, adicioná-las 1 a 1 no gridview
                foreach (GridViewDataColumn col in colunasDinamicas)
                    grdTranscricao.Columns.Add(col);

                if (new string[] { "ctl00$cphFormulario$btnSalvar", "ctl00$cphFormulario$btnConcluir" }.Contains(Page.Request.Params.Get("__EVENTTARGET")))
                    return;

                grdTranscricao.DataSource = rnAluno.ListaAlunoPor(ProvaId, Turma, Semestre, Ano);
                grdTranscricao.DataBind();                               

                //montar os bands da transcrição
                ScriptManager.RegisterStartupScript(this, this.GetType(), "addBands", "addBands();", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (SomenteLeitura)
            {
                btnSalvar.Visible = false;
                btnConcluir.Visible = false;
            }
        }

        protected void grdTranscricao_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!rnTranscricaoTurma.TranscricaoExiste(EtapaId, Turma, Ano, Semestre))
                    return;

                int transcricaoId = TranscricaoId;
                if (transcricaoId <= 0)
                    return;

                IList<RN.AvaliacaoExterna.Entidades.AlunoParticipante> alunos;
                IList<RN.AvaliacaoExterna.Entidades.Questao> questoes;
                List<RN.AvaliacaoExterna.Entidades.Resposta> respostas;

                rnAluno.Carrega(ProvaId, Turma, Ano, Semestre, out alunos, out questoes, out respostas);

                if (alunos == null)
                    return;

                if (!alunos.Any() || !respostas.Any())
                    return;

                for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
                {
                    string ALUNO = Convert.ToString(grdTranscricao.GetRowValues(i, "ALUNO"));

                    foreach (GridViewDataColumn col in grdTranscricao.Columns)
                    {
                        int componenteId = 0;
                        int questaoId = 0;                        

                        if (col.Name.ToLower().StartsWith("dtp_"))
                        {
                            int.TryParse(col.Name.Split('_')[1], out componenteId);
                            if (componenteId == 0)
                                continue;

                            RN.AvaliacaoExterna.Entidades.AlunoParticipante aluno = alunos.FirstOrDefault(
                                q => q.Aluno == ALUNO &&
                                q.ComponenteId == componenteId
                            );
                            if (aluno == null)
                                continue;

                            ASPxDateEdit dt = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as ASPxDateEdit;
                            dt.Text = aluno.DataParticipacao.HasValue ? aluno.DataParticipacao.Value.ToString("dd/MM/yyyy") : "";
                        }

                        if (col.Name.ToLower().StartsWith("stp_"))
                        {
                            int.TryParse(col.Name.Split('_')[1], out componenteId);
                            if (componenteId == 0)
                                continue;

                            RN.AvaliacaoExterna.Entidades.AlunoParticipante aluno = alunos.FirstOrDefault(
                                q => q.Aluno == ALUNO &&
                                q.ComponenteId == componenteId /*&&
                            q.SituacaoParticipanteId.HasValue*/
                            );
                            if (aluno == null)
                                continue;

                            DropDownList cb = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                            cb.SelectedValue = aluno.SituacaoParticipanteId > 0 ? aluno.SituacaoParticipanteId.ToString() : null;

                            ViewState["ALUNOPARTICIPANTEID_" + grdTranscricao.GetRowValues(i, "ALUNO") + "_" + componenteId] = aluno.AlunoParticipanteId.ToString();
                        }

                        if (col.Name.ToLower().StartsWith("r"))
                        {
                            int.TryParse(col.Name.Substring(1), out questaoId);
                            if (questaoId == 0)
                                continue;

                            componenteId = rnQuestao.RetornaConponentePor(questaoId); 

                            var alunoParticipante = alunos.FirstOrDefault(r => r.Aluno == ALUNO && r.ComponenteId == componenteId);
                            if (alunoParticipante == null)
                                continue;

                            RN.AvaliacaoExterna.Entidades.Resposta resposta = respostas.FirstOrDefault(q =>
                                q.Aluno == ALUNO &&
                                q.QuestaoId == questaoId
                            );
                            if (resposta == null)
                                continue;

                            RN.AvaliacaoExterna.Entidades.Questao questao = questoes.FirstOrDefault(q => q.QuestaoId == questaoId);
                            if (questao == null)
                                continue;

                            bool? respostaCorreta = null;
                            if (resposta.resposta.HasValue)
                                respostaCorreta = questao.AlternativaCorreta == resposta.resposta.Value;

                            TextBox tb = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as TextBox;
                            tb.Text = resposta.resposta.HasValue ? NumberToLetter(resposta.resposta.Value).ToString() : "";
                            tb.Attributes["data-resposta-id"] = resposta.RespostaId.ToString();

                            //colorir a célula de acordo com acerto ou erro da questão. acerto: verde, erro: vermelho
                            if (respostaCorreta.HasValue && respostaCorreta.Value)
                                tb.BackColor = System.Drawing.Color.FromArgb(182, 226, 211); //#B6E2D3 - verde claro
                            else if (respostaCorreta.HasValue && !respostaCorreta.Value)
                                tb.BackColor = System.Drawing.Color.FromArgb(216, 167, 177); //#D8A7B1 - vermelho claro

                            ViewState["RESPOSTAID_" + ALUNO + "_" + questaoId] = resposta.RespostaId.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int ObterAlunoParticipanteId(string aluno, int componenteId)
        {
            return Convert.ToInt32(ViewState["ALUNOPARTICIPANTEID_" + aluno + "_" + componenteId]);
        }

        private int ObterRespostaId(string aluno, int questaoId)
        {
            return Convert.ToInt32(ViewState["RESPOSTAID_" + aluno + "_" + questaoId]);
        }

        protected void UpdatePanel1_PreRender(object sender, EventArgs e)
        {
            try
            {
                bool transcricaoFinalizada = rnTranscricaoTurma.TranscricaoFinalizada(EtapaId, Turma, Ano, Semestre);

                if (!transcricaoFinalizada)
                    return;

                btnSalvar.Visible = false;
                btnConcluir.Visible = false;
                btnExportar.Visible = true;

                if (transcricaoFinalizada)
                    lblMensagem.Text = "Transcrição finalizada<br /><br />";

                for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
                {
                    for (int j = 0; j < grdTranscricao.Columns.Count; j++)
                    {
                        WebControl ctrl = grdTranscricao.FindRowCellTemplateControl(i, grdTranscricao.Columns[j] as GridViewDataColumn, grdTranscricao.Columns[j].Name) as WebControl;
                        if (ctrl != null && (ctrl is TextBox || ctrl is ASPxDateEdit || ctrl is DropDownList))
                            ctrl.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private ValidacaoDados SalvaTranscricao(bool finaliza)
        {
            //Declaração das 3 variáveis que serão usadas para submeter os dados da transcrição para o banco de dados: 1 transcrição para N alunos, e 1 aluno para N respostas.
            //Prestar atenção:
            //- a qtd. de alunos é a qtd. de alunos da turma vezes a qtd. de componentes da etapa;
            //- a qtd. de respostas é a qtd. de questões de todos os componentes de etapa vezes a qtd. de alunos que participaram de todos os componentes da etapa;
            Techne.Lyceum.RN.AvaliacaoExterna.Entidades.TranscricaoTurma transcricao = new Techne.Lyceum.RN.AvaliacaoExterna.Entidades.TranscricaoTurma();
            List<RN.AvaliacaoExterna.Entidades.AlunoParticipante> alunos = new List<RN.AvaliacaoExterna.Entidades.AlunoParticipante>();
            List<RN.AvaliacaoExterna.Entidades.Resposta> respostas = new List<RN.AvaliacaoExterna.Entidades.Resposta>();
            List<string> alunosTotal = new List<string>();

            //Instanciar o validador de dados
            ValidacaoDados validacaoDados = new ValidacaoDados();
            
            //Prepara a transcrição para ser salva
            transcricao.TranscricaoTurmaId = TranscricaoId;
            transcricao.Ano = Ano;
            transcricao.Semestre = Semestre;
            transcricao.EtapaId = EtapaId;
            transcricao.Turma = Turma;
            transcricao.DataFinalizacao = finaliza ? (DateTime?)DateTime.Now : null;
            transcricao.UsuarioID = User.Identity.Name;

            //Para cada linha do grid...
            for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
            {
                //Listar, a partir do grid, os componentes da etapa que participam da transcrição
                IList<int> listaComponenteId = grdTranscricao.VisibleColumns
                    .Where(q => q.Name.StartsWith("stP_"))
                    .Select(s => Convert.ToInt32(s.Name.Split('_')[1]))
                    .ToList();

                //Para cada componente da etapa obtido...
                foreach (int componenteId in listaComponenteId)
                {
                    //Obter a data de participação do aluno para este componente de etapa
                    ASPxDateEdit txtDataParticipacao = grdTranscricao.FindRowCellTemplateControl(i, grdTranscricao.Columns["dtP_" + componenteId] as GridViewDataColumn, "dtP_" + componenteId) as ASPxDateEdit;
                    DateTime? dataParticipacao = !txtDataParticipacao.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataParticipacao.Text) as DateTime? : null;

                    //Obter o status de participação do aluno para este componente de etapa
                    DropDownList ddlStatusParticipacao = grdTranscricao.FindRowCellTemplateControl(i, grdTranscricao.Columns["stP_" + componenteId] as GridViewDataColumn, "stP_" + componenteId) as DropDownList;
                    int? statusParticipacao = !ddlStatusParticipacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlStatusParticipacao.SelectedValue) as int? : null;

                    if (finaliza && !statusParticipacao.HasValue)
                    {
                        validacaoDados.Mensagem = "É necessário preencher o campo STATUS PARTICIPAÇÃO de todos os alunos antes de finalizar a transcrição.";
                        return validacaoDados;
                    }

                    string codigoAluno = Convert.ToString(grdTranscricao.GetRowValues(i, "ALUNO"));
                    
                    //Adiciona aluno na lista geral para conferencia da tela
                    alunosTotal.Add(codigoAluno);

                    //Apenas adiciona na lista de alunos para gravar alunos com alguma informação
                    if (statusParticipacao != null || dataParticipacao != null)
                    {
                        //Instanciar o aluno para ser adicionado à lista de alunos
                        RN.AvaliacaoExterna.Entidades.AlunoParticipante aluno = new AlunoParticipante
                        {
                            AlunoParticipanteId = ObterAlunoParticipanteId(Convert.ToString(grdTranscricao.GetRowValues(i, "ALUNO")), componenteId),
                            ProvaId = ProvaId,
                            ComponenteId = componenteId,
                            Aluno = codigoAluno,
                            SituacaoParticipanteId = statusParticipacao == null ? 0: Convert.ToInt32(statusParticipacao),
                            DataParticipacao = !finaliza ? dataParticipacao : (new int?[] { 1, 5 }.Contains(statusParticipacao) ? dataParticipacao : null),
                            UsuarioID = User.Identity.Name,
                        };

                        //Adicionar o aluno à lista de alunos que serão inseridos na tabela ALUNOPARTICIPANTE
                        alunos.Add(aluno);
                    }
                }

                //Listar, a partir do grid, as questões que participam da transcrição
                IList<int> listaQuestaoId = grdTranscricao.VisibleColumns
                    .Where(q => q.Name.StartsWith("r"))
                    .Select(s => Convert.ToInt32(s.Name.Substring(1)))
                    .ToList();

                //E então, obter as questões do banco de dados
                IList<RN.AvaliacaoExterna.Entidades.Questao> questoes = rnQuestao.ListaPorQuestaoId(listaQuestaoId).ToList<RN.AvaliacaoExterna.Entidades.Questao>();

                //Para cada questão obtida...
                foreach (RN.AvaliacaoExterna.Entidades.Questao questao in questoes)
                {
                    //Obter a resposta dada pelo aluno para esta questão
                    TextBox txtResposta = grdTranscricao.FindRowCellTemplateControl(i, grdTranscricao.Columns["r" + questao.QuestaoId] as GridViewDataColumn, "r" + questao.QuestaoId) as TextBox;
                    int? _resposta = !txtResposta.Text.IsNullOrEmptyOrWhiteSpace() ? LetterToNumber(txtResposta.Text[0]) : null;

                    //Obter o código do aluno
                    string ALUNO = Convert.ToString(grdTranscricao.GetRowValues(i, "ALUNO"));

                    //Apenas adicionar quem teve resposta
                    if (_resposta != null)
                    {
                        //Instanciar a resposta para ser adicionada à lista de respostas
                        RN.AvaliacaoExterna.Entidades.Resposta resposta = new Resposta
                        {
                            RespostaId = ObterRespostaId(ALUNO, questao.QuestaoId),
                            Aluno = ALUNO,
                            QuestaoId = questao.QuestaoId,
                            resposta = _resposta,
                            UsuarioID = User.Identity.Name
                        };

                        //Adicionar a resposta à lista de respostas que serão inseridas na tabela RESPOSTA
                        respostas.Add(resposta);
                    }
                }
            }

            //Antes de salvar os dados, verificar se o grid está consistente com os alunos que estão no banco de dados
            // ******* NÃO FUNCIONOU ******* porque o grdTranscricao.VisibleRowCount atualiza a qtd. de registros do banco de dados sem renderizar as atualizações pro usuário
            bool estaConsistente = rnAluno.EstaConsistente(alunosTotal.Distinct().ToList(), ProvaId, Turma, Semestre, Ano);
            
            if (!estaConsistente)
            {
                btnSalvar.Enabled = false;
                btnConcluir.Enabled = false;
                validacaoDados.Valido = false;
                validacaoDados.Mensagem = "Houve movimentação(ões) com aluno(s) dessa turma. Por gentileza recarregar a tela antes de salvar os dados. Os botões \"" + btnSalvar.Text + "\" e \"" + btnConcluir.Text + "\" permanecerão desabilitados até que a tela seja carregada";
                return validacaoDados;
            }  

            //Se for salvar e finalizar...
            if (finaliza)
            {
                //Validar, salvar e finalizar a transcrição
                validacaoDados = rnTranscricaoTurma.ValidaFinalizacao(transcricao, alunos, respostas);
                transcricao.DataFinalizacao = validacaoDados.Valido ? DateTime.Now as DateTime? : null;
            }
            else
            {
                //Salvar a transcrição, sem finalizar e valida               
                transcricao.DataFinalizacao = null;
                validacaoDados = rnTranscricaoTurma.Valida(transcricao, alunos, respostas);
            }

            if (validacaoDados.Valido)
            {
                rnTranscricaoTurma.Salva(transcricao, alunos, respostas, alunosTotal);
                hdnTranscricaoId.Value = transcricao.TranscricaoTurmaId.ToString();
            }           

            return validacaoDados;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacaoDados = SalvaTranscricao(false);
                if (!validacaoDados.Valido)
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, validacaoDados.Mensagem);
                else
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "A transcrição foi salva com sucesso!");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, ex.Message);
            }
        }

        protected void btnConcluir_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacaoDados = SalvaTranscricao(true);
                if (!validacaoDados.Valido)
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, validacaoDados.Mensagem);
                else
                    lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, "A transcrição foi finalizada com sucesso!");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = String.Format("{0:dd/MM/yyyy HH:mm:ss} -> {1}<br /><br />", DateTime.Now, ex.Message);
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> qryPars = new Dictionary<string, string>();
            qryPars.Add("provaId", ProvaId.ToString());
            qryPars.Add("avaliacaoId", AvaliacaoId.ToString()); 
            qryPars.Add("etapaId", EtapaId.ToString()); 
            qryPars.Add("periodo", Semestre.ToString());
            qryPars.Add("unidade_ens", Unidade_Ens.ToString());
            string queryString = TPage.CodificaQueryString(qryPars);
            Response.Redirect(string.Format("EtapaTurma.aspx?{0}", queryString));
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            string titulo = this.ProvaDescricao + " - " + this.Etapa + " - " + this.Unidade_Nome + " - " + this.Turma;
            string nomeArquivo = this.ProvaId + "_" + this.EtapaId + "_" + this.Unidade_Ens + "_" + this.Turma;
            Techne.Lyceum.RN.Util.ExportaExcel exportaExcel = new Techne.Lyceum.RN.Util.ExportaExcel();

            if (!rnTranscricaoTurma.TranscricaoExiste(EtapaId, Turma, Ano, Semestre))
                return;

            //int transcricaoId = rnAluno.ObtemTranscricaoId(EtapaId, Turma);
            int transcricaoId = TranscricaoId;
            if (transcricaoId <= 0)
                return;

            IList<RN.AvaliacaoExterna.Entidades.AlunoParticipante> alunos;
            IList<RN.AvaliacaoExterna.Entidades.Questao> questoes;
            List<RN.AvaliacaoExterna.Entidades.Resposta> respostas;
            IList<RN.AvaliacaoExterna.Entidades.SituacaoParticipante> situacoes;
            rnAluno.Carrega(ProvaId, Turma, Ano, Semestre, out alunos, out questoes, out respostas, out situacoes); 
            if (!alunos.Any() || !respostas.Any() || !situacoes.Any())
                return;

            foreach (GridViewColumn column in grdTranscricao.VisibleColumns)
            {
                var col = column as GridViewDataColumn;
                DataColumn dataColumn = new DataColumn();

                if (col != null)
                    dataColumn = dataTable.Columns.Add(col.FieldName);

                if (col.Name.ToLower().StartsWith("dtp_"))
                {
                    dataColumn.Caption = "DIA DO TESTE";
                }
                if (col.Name.ToLower().StartsWith("stp_"))
                {
                    dataColumn.Caption = "STATUS PARTICIPAÇÃO";
                }
                if (col.Name.ToLower().StartsWith("r"))
                {
                    int questaoId = 0;
                    int.TryParse(col.Name.Substring(1), out questaoId);
                    if (questaoId == 0)
                        continue;

                    RN.AvaliacaoExterna.Entidades.Questao questao = questoes.FirstOrDefault(q => q.QuestaoId == questaoId);
                    if (questao == null)
                        continue;

                    dataColumn.Caption = questao.Numero.ToString();
                }
                else if (col.Name.IsNullOrEmptyOrWhiteSpace() && !col.FieldName.IsNullOrEmptyOrWhiteSpace())
                {
                    switch (col.FieldName)
                    {
                        case "ALUNO":
                            dataColumn.Caption = "CÓDIGO";
                            break;
                        case "NOME_COMPL":
                            dataColumn.Caption = "ALUNO";
                            break;
                        case "SEXO":
                            dataColumn.Caption = "SEXO";
                            break;
                        case "DT_NASC":
                            dataColumn.Caption = "DATA NASCIMENTO";
                            break;
                    }
                }
            }

            foreach (GridViewColumn column in grdTranscricao.VisibleColumns)
            {
                var col = column as GridViewDataColumn;
                DataColumn dataColumn = new DataColumn();

                if (col != null && col.Name.ToLower().StartsWith("r"))
                {
                    dataColumn = dataTable.Columns.Add(col.FieldName + "_resp");

                    int questaoId = 0;
                    int.TryParse(col.Name.Substring(1), out questaoId);
                    if (questaoId == 0)
                        continue;

                    RN.AvaliacaoExterna.Entidades.Questao questao = questoes.FirstOrDefault(q => q.QuestaoId == questaoId);
                    if (questao == null)
                        continue;

                    dataColumn.Caption = questao.Numero.ToString();
                }
                else
                {
                    continue;
                }
            }

            for (int i = 0; i < grdTranscricao.VisibleRowCount; i++)
            {
                DataRow row = dataTable.NewRow();
                string ALUNO = Convert.ToString(grdTranscricao.GetRowValues(i, "ALUNO"));

                foreach (GridViewColumn column in grdTranscricao.VisibleColumns)
                {
                    int componenteId = 0;
                    int questaoId = 0;
                   
                    var col = column as GridViewDataColumn;
                    string cellValue = "";

                    if (col.Name.ToLower().StartsWith("dtp_"))
                    {
                        int.TryParse(col.Name.Split('_')[1], out componenteId);
                        if (componenteId == 0)
                            continue;

                        RN.AvaliacaoExterna.Entidades.AlunoParticipante aluno = alunos.FirstOrDefault(
                            q => q.Aluno == ALUNO &&
                            q.ComponenteId == componenteId
                        );
                        if (aluno == null)
                            continue;

                        ASPxDateEdit dt = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as ASPxDateEdit;
                        dt.Text = aluno.DataParticipacao.HasValue ? aluno.DataParticipacao.Value.ToString("dd/MM/yyyy") : "";

                        if (!dt.Text.IsNullOrEmptyOrWhiteSpace())
                            cellValue = dt.Text;
                    }

                    if (col.Name.ToLower().StartsWith("stp_"))
                    {
                        int.TryParse(col.Name.Split('_')[1], out componenteId);
                        if (componenteId == 0)
                            continue;

                        RN.AvaliacaoExterna.Entidades.AlunoParticipante aluno = alunos.FirstOrDefault(
                            q => q.Aluno == ALUNO &&
                            q.ComponenteId == componenteId &&
                            q.SituacaoParticipanteId > 0
                        );
                        if (aluno == null)
                            continue;

                        RN.AvaliacaoExterna.Entidades.SituacaoParticipante situacao = situacoes.FirstOrDefault(q => q.SituacaoParticipanteId == aluno.SituacaoParticipanteId);
                        if (situacao == null)
                            continue;

                        DropDownList cb = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as DropDownList;
                        cb.SelectedValue = aluno.SituacaoParticipanteId.ToString();

                        if (!cb.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            cellValue = situacao.Descricao;
                    }

                    if (col.Name.ToLower().StartsWith("r"))
                    {
                        int.TryParse(col.Name.Substring(1), out questaoId);
                        if (questaoId == 0)
                            continue;

                        componenteId = rnQuestao.RetornaConponentePor(questaoId);

                        RN.AvaliacaoExterna.Entidades.Resposta resposta = respostas.FirstOrDefault(q =>
                            q.Aluno == ALUNO &&
                            q.QuestaoId == questaoId
                        );
                        if (resposta == null)
                            continue;
                        
                        RN.AvaliacaoExterna.Entidades.Questao questao = questoes.FirstOrDefault(q => q.QuestaoId == questaoId);
                        if (questao == null)
                            continue;

                        bool? respostaCorreta = null;
                        if (resposta.resposta.HasValue)
                            respostaCorreta = questao.AlternativaCorreta == resposta.resposta.Value;

                        TextBox tb = grdTranscricao.FindRowCellTemplateControl(i, col, col.Name) as TextBox;
                        tb.Text = resposta.resposta.HasValue ? NumberToLetter(resposta.resposta.Value).ToString() : "";

                        ////colorir a célula de acordo com acerto ou erro da questão. acerto: verde, erro: vermelho
                        //if (respostaCorreta.HasValue && respostaCorreta.Value)
                        //    tb.BackColor = System.Drawing.Color.FromArgb(182, 226, 211); //#B6E2D3 - verde claro
                        //else if (respostaCorreta.HasValue && !respostaCorreta.Value)
                        //    tb.BackColor = System.Drawing.Color.FromArgb(216, 167, 177); //#D8A7B1 - vermelho claro

                        if (!tb.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            cellValue = tb.Text;

                            if (respostaCorreta.HasValue && respostaCorreta.Value)
                                row[col.FieldName + "_resp"] = "1";
                            else if (respostaCorreta.HasValue && !respostaCorreta.Value)
                                row[col.FieldName + "_resp"] = "0";
                        }
                    }
                    else if (col.Name.IsNullOrEmptyOrWhiteSpace() && !col.FieldName.IsNullOrEmptyOrWhiteSpace())
                    {
                        switch (col.FieldName)
                        {
                            case "ALUNO":
                            case "NOME_COMPL":
                            case "SEXO":
                                cellValue = Convert.ToString(grdTranscricao.GetRowValues(i, col.FieldName));
                                break;
                            case "DT_NASC":
                                cellValue = Convert.ToDateTime(grdTranscricao.GetRowValues(i, col.FieldName)).ToString("dd/MM/yyyy");
                                break;
                        }
                    }

                    if (cellValue != null)
                        row[col.FieldName] = cellValue;
                    else
                        row[col.FieldName] = DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            exportaExcel.ExportaDataTablePor_ColunasComCaption(dataTable, titulo, nomeArquivo);
        }

        #region Helpers para o ASPxGridView

        private class FormatoNomeQuestao
        {
            public FormatoNomeQuestao(string nome)
            {
                Nome = nome;
            }

            public FormatoNomeQuestao(int etapaId, int questao)
            {
                EtapaId = etapaId;
                Questao = questao;
            }

            public int EtapaId { get; set; }
            public int Questao { get; set; }

            public string Nome
            {
                get
                {
                    if (EtapaId > 0 && Questao > 0)
                        return EtapaId.ToString() + "_" + Questao.ToString();
                    else
                        return null;
                }

                set
                {
                    if (value.IsNullOrEmptyOrWhiteSpace())
                        return;

                    string[] campos = value.Split('_');

                    if (campos.Length != 2)
                        return;

                    for (int i = 0; i < campos.Length; i++)
                    {
                        string campo = campos[i];

                        if (campo.IsNullOrEmptyOrWhiteSpace())
                            return;

                        int num;
                        if (!int.TryParse(campo, out num))
                            return;

                        switch (i)
                        {
                            case 0:
                                EtapaId = num;
                                break;

                            case 1:
                                Questao = num;
                                break;

                            default:
                                return;
                        }
                    }
                }
            }
        }

        private class FormatoNomeStatus
        {
            public FormatoNomeStatus(string nome)
            {
                Nome = nome;
            }

            public FormatoNomeStatus(int etapaId, DateTime data)
            {
                EtapaId = etapaId;
                AlunoId = 0;
                Data = data;
            }

            public int EtapaId { get; set; }
            public int AlunoId { get; set; }
            public DateTime Data { get; set; }
            public string DataFormatada
            {
                get
                {
                    string data = null;
                    if (Data >= SqlDateTime.MinValue)
                        data = Data.ToString("yyyyMMdd");
                    return data;
                }
            }

            public string Nome
            {
                get
                {
                    if (EtapaId > 0 && AlunoId > 0 && !DataFormatada.IsNullOrEmptyOrWhiteSpace())
                        return EtapaId.ToString() + "_" + AlunoId.ToString() + "_" + DataFormatada;
                    else
                        return null;
                }

                set
                {
                    if (value.IsNullOrEmptyOrWhiteSpace())
                        return;

                    string[] campos = value.Split('_');

                    if (campos.Length != 3)
                        return;

                    for (int i = 0; i < campos.Length; i++)
                    {
                        string campo = campos[i];

                        if (campo.IsNullOrEmptyOrWhiteSpace())
                            return;

                        if (new int[] { 0, 1 }.Contains(i))
                        {
                            int num;
                            if (!int.TryParse(campo, out num))
                                return;

                            switch (i)
                            {
                                case 0:
                                    EtapaId = num;
                                    break;

                                case 1:
                                    AlunoId = num;
                                    break;

                                default:
                                    return;
                            }
                        }
                        else if (i == 2)
                        {
                            DateTime dt;
                            if (!DateTime.TryParse(campo, out dt))
                                return;

                            switch (i)
                            {
                                case 2:
                                    Data = dt;
                                    break;

                                default:
                                    return;
                            }
                        }
                    }
                }
            }
        }
        
        private GridViewDataColumn CriaColunaResposta(int etapaId, int componenteId, string descricaoComponente, int questaoId, int numeroQuestao, int qtdAlternativas, int tamanhoColunaQuestoes)
        {
            GridViewDataColumn col = new GridViewDataColumn
            {
                Caption = numeroQuestao.ToString(),
                Name = "r" + questaoId,
                FieldName = "r" + questaoId,
                Width = Unit.Pixel(tamanhoColunaQuestoes),
                UnboundType = UnboundColumnType.String,
                DataItemTemplate = new TextBoxRespostaTemplate("r" + questaoId, componenteId, descricaoComponente, qtdAlternativas, SomenteLeitura),
            };

            col.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
            col.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);

            return col;
        }

        private GridViewDataColumn CriaColunaDataParticipacao(int componenteId, DateTime dataMinima, DateTime dataMaxima)
        {
            string nomeColuna = "dtP_" + componenteId;

            GridViewDataColumn col = new GridViewDataColumn
            {
                Caption = "Data Participação",
                Name = nomeColuna,
                FieldName = nomeColuna,
                Width = Unit.Pixel(100),
                UnboundType = UnboundColumnType.String,
                DataItemTemplate = new DateEditDataTemplate(nomeColuna, dataMinima, dataMaxima, SomenteLeitura),
            };

            col.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
            col.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);

            return col;
        }

        private GridViewDataColumn CriaColunaStatusParticipacao(int componenteId, DataTable listaSituacaoParticipante)
        {
            string nomeColuna = "stP_" + componenteId; 

            GridViewDataColumn col = new GridViewDataColumn
            {
                Caption = "Status Participação",
                Name = nomeColuna,
                FieldName = nomeColuna,
                Width = Unit.Pixel(100),
                UnboundType = UnboundColumnType.String,
                DataItemTemplate = new ComboBoxStatusTemplate(nomeColuna, listaSituacaoParticipante, SomenteLeitura),
            };

            col.Settings.AllowSort = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            col.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
            col.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);

            return col;
        }

        private IList<GridViewDataColumn> AgregarColunasQuestoesPorComponente(IList<GridViewDataColumn> colunasJaCriadas, int componenteId, DataTable listaSituacaoParticipante)
        {
            //não precisa rodar a função se o componente da etapa não tiver questões cadastradas, pois não terá colunas a serem criadas
            if (!rnQuestao.ExisteQuestaoPor(componenteId, ProvaId)) 
                return colunasJaCriadas;

            //obtem o componente da etapa para pegar as datas de inicio e fim de realização da prova
            RN.AvaliacaoExterna.Entidades.Etapa etapa = rnEtapa.ObtemPor(EtapaId);
            DateTime dataMinima = etapa.InicioRealizacao;
            DateTime dataMaxima = etapa.FimRealizacao;
            string descricaoComponente = rnComponente.RetornaDescricaoPor(componenteId);

            //obter a lista de questões através do componente da etapa especificado.
            DataTable questoes = rnQuestao.ListaPorComponente(componenteId, ProvaId); 

            //obter a qtd. de questões do componente da etapa especificado. esta será a qtd. de colunas a serem agregadas na lista de colunas dinâmicas
            //obs.: a validação acima já garante que qtdQuestoesPorComponente será maior do que zero
            int qtdQuestoesPorComponente = questoes.Rows.Count;

            //obter a qtd. de questões da etapa. é necessário porque precisaremos encaixar todas as questões da etapa no espaço reservado do grid às colunas dinâmicas
            int qtdQuestoesPorEtapa = rnProva.ObtemQtdQuestoesPorEtapaId(ProvaId); 

            //obter a qtd. de colunas fixas do grid. necessário para sabermos o tamanho do espaço reservado às colunas dinâmicas
            int qtdColunasFixasNoGrid = grdTranscricao.Columns.Count;

            //obter o tamanho total reservado para as colunas fixas do grid.
            //a diferença entre o tamanho do grid e este valor nos informará qto temos de tamanho no espaço das colunas dinâmicas
            int sumColunasFixasNoGrid = 0;
            foreach (GridViewDataColumn col in grdTranscricao.Columns)
                sumColunasFixasNoGrid += Convert.ToInt32(col.Width.Value);

            //eh a diferença entre o tamanho do grid, o tamanho reservado para as colunas fixas e o tamanho reservado para as colunas de status
            int espacoReservadoParaColunasDinamicas = Convert.ToInt32(grdTranscricao.Width.Value) - sumColunasFixasNoGrid - 340;

            //sabendo do tamanho do espaço reservado para colunas dinâmicas, e sabendo também da qtd. de colunas dinâmicas da ETAPA,
            //teremos como saber qual será o tamanho da coluna dinâmica.
            int tamanhoColunaQuestoes = (espacoReservadoParaColunasDinamicas / qtdQuestoesPorEtapa);

            //criar as colunas dinâmicas ref. as questões do componente da etapa e agregá-la à lista de colunas dinâmicas
            for (int q = 0; q < qtdQuestoesPorComponente; q++)
            {
                int questaoId = Convert.ToInt32(questoes.Rows[q]["QUESTAOID"]);
                int numeroQuestao = Convert.ToInt32(questoes.Rows[q]["NUMERO"]);
                int qtdAlternativas = Convert.ToInt32(questoes.Rows[q]["QUANTIDADEALTERNATIVAS"]);

                GridViewDataColumn colResposta = CriaColunaResposta(EtapaId, componenteId, descricaoComponente, questaoId, numeroQuestao, qtdAlternativas, tamanhoColunaQuestoes);
                colunasJaCriadas.Add(colResposta);
            }

            //criar a coluna ref. à data de participação e agregá-la à lista de colunas dinâmicas
            GridViewDataColumn colData = CriaColunaDataParticipacao(componenteId, dataMinima, dataMaxima);
            colunasJaCriadas.Add(colData);

            //criar a coluna ref. ao status de participação e agregá-la à lista de colunas dinâmicas
            GridViewDataColumn colStatus = CriaColunaStatusParticipacao(componenteId, listaSituacaoParticipante);
            colunasJaCriadas.Add(colStatus);

            //retornar a lista de colunas dinâmicas com as novas colunas já agregadas
            return colunasJaCriadas;
        }

        #endregion

        protected int? LetterToNumber(char letter)
        {
            letter = Convert.ToChar(letter.ToString().ToUpper());

            switch (letter)
            {
                case 'S': return 0;
                case 'A': return 1;
                case 'B': return 2;
                case 'C': return 3;
                case 'D': return 4;
                case 'E': return 5;
                default: return null;
            }
        }

        protected char? NumberToLetter(int number)
        {
            switch (number)
            {
                case 0: return 'S';
                case 1: return 'A';
                case 2: return 'B';
                case 3: return 'C';
                case 4: return 'D';
                case 5: return 'E';
                default: return null;
            }
        }
    }

    public class TextBoxRespostaTemplate : ITemplate
    {
        private readonly string id;
        private readonly int componenteId;
        private readonly string descricaoComponente;
        private readonly int qtdAlternativas;
        private readonly bool somenteLeitura;

        public TextBoxRespostaTemplate(string id, int componenteId, string descricaoComponente, int qtdAlternativas, bool somenteLeitura)
        {
            this.id = id;
            this.componenteId = componenteId;
            this.descricaoComponente = descricaoComponente;
            this.qtdAlternativas = qtdAlternativas;
            this.somenteLeitura = somenteLeitura;
        }

        public void InstantiateIn(Control container)
        {
            TextBox tb = new TextBox();
            tb.Width = Unit.Pixel(10);
            tb.ID = id;
            tb.Attributes["data-qtd-alternativas"] = qtdAlternativas.ToString();
            tb.Attributes["data-componente-id"] = componenteId.ToString();
            tb.Attributes["data-componente-descricao"] = descricaoComponente;
            tb.Attributes["onkeypress"] = "return keypress(event, this);";
            tb.Attributes["maxlength"] = "1";
            tb.Attributes["onkeyup"] = "return keyup(event, this);";
            tb.Attributes["onkeydown"] = "return keydown(event, this);";
            tb.Enabled = !somenteLeitura;
            tb.ReadOnly = somenteLeitura;
            container.Controls.Add(tb);
        }
    }

    public class DateEditDataTemplate : ITemplate
    {
        private readonly string id;
        private readonly DateTime dataMinima;
        private readonly DateTime dataMaxima;
        private readonly bool somenteLeitura;

        public DateEditDataTemplate(string id, DateTime dataMinima, DateTime dataMaxima, bool somenteLeitura)
        {
            this.id = id;
            this.dataMinima = dataMinima;
            this.dataMaxima = dataMaxima;
            this.somenteLeitura = somenteLeitura;
        }

        public void InstantiateIn(Control container)
        {
            //TextBox dt = new TextBox();
            var dt = new ASPxDateEdit();
            dt.ID = id;
            dt.MinDate = dataMinima;
            dt.MaxDate = dataMaxima;
            dt.UseMaskBehavior = false;
            dt.DropDownButton.Visible = false;
            dt.Width = Unit.Pixel(90);
            dt.Attributes["id"] = id;
            dt.Attributes["data-min-date"] = dataMinima.ToString("yyyy-MM-dd");
            dt.Attributes["data-max-date"] = dataMaxima.ToString("yyyy-MM-dd");
            dt.Attributes["onfocusout"] = "validaData(event, this)";
            dt.Enabled = !somenteLeitura;
            dt.ReadOnly = somenteLeitura;
            container.Controls.Add(dt);
        }
    }

    public class ComboBoxStatusTemplate : ITemplate
    {
        private readonly string id;
        private DataTable listaSituacaoParticipante;
        private readonly bool somenteLeitura;

        public ComboBoxStatusTemplate(string id, DataTable listaSituacaoParticipante, bool somenteLeitura)
        {
            this.id = id;
            this.listaSituacaoParticipante = listaSituacaoParticipante;
            this.somenteLeitura = somenteLeitura;
        }

        public void InstantiateIn(Control container)
        {
            DropDownList cb = new DropDownList();
            cb.ID = id;
            cb.Width = Unit.Pixel(90);
            cb.Attributes["data-aluno-participante-id"] = "";
            cb.DataTextField = "DESCRICAO";
            cb.DataValueField = "SITUACAOPARTICIPANTEID";
            cb.DataSource = listaSituacaoParticipante;
            cb.DataBind();
            cb.Enabled = !somenteLeitura;
            container.Controls.Add(cb);
        }
    }
}