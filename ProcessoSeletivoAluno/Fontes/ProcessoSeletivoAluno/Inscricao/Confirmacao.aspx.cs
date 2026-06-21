using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.ProcessoSeletivoAluno;

namespace Techne.Lyceum.Net.ProcessoSeletivoAluno
{
    public partial class Confirmacao : System.Web.UI.Page
    {
        Sessao.CandidatoProcessoSeletivoSessao sessaoCandidato = new Sessao.CandidatoProcessoSeletivoSessao();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sessaoCandidato.CandidatoAcordoEdital == false)
            {
                Response.Redirect("Edital.aspx");
            }

            if (sessaoCandidato.CandidatoLogado == false)
            {
                Response.Redirect("Identificacao.aspx");
            }

            if (sessaoCandidato.InscricaoId == int.MinValue)
            {
                Response.Redirect("Identificacao.aspx");
            }

            listarInscricao();
            sessaoCandidato.EncerrarSessao();
        }

        public void listarInscricao()
        {
            RN.ProcessoSeletivoAluno.DTOs.ConfirmacaoProcessoSeletivoAluno confirmacaoProcessoSeletivoAluno = RN.ProcessoSeletivoAluno.Inscricao.ListaConfirmacao_ProcessoSeletivoAlunoPorInscricao(sessaoCandidato.NumeroInscricao);

            lblNumeroProcessoSeletivo.Text = confirmacaoProcessoSeletivoAluno.NumeroEdital;
            lblNumeroInscricao.Text = confirmacaoProcessoSeletivoAluno.NumeroInscricao;
            lblNomeCandidato.Text = confirmacaoProcessoSeletivoAluno.NomeCandidato;
            lblDataNascimento.Text = confirmacaoProcessoSeletivoAluno.DataNascimento;
            lblNomeMae.Text = confirmacaoProcessoSeletivoAluno.NomeMae;
            lblTipoDeficiencia.Text = confirmacaoProcessoSeletivoAluno.NecessidadeEspecial;
            lblRecursoNecessarioProva.Text = confirmacaoProcessoSeletivoAluno.RecursosNecessarioProva;
            lblUnidadeEnsino.Text = confirmacaoProcessoSeletivoAluno.UnidadeEnsino;
            lblCurso.Text = confirmacaoProcessoSeletivoAluno.Curso;
            lblDataInscricao.Text = confirmacaoProcessoSeletivoAluno.DataAlteracao;
            lblIP.Text = confirmacaoProcessoSeletivoAluno.IP;
        }
    }
}
