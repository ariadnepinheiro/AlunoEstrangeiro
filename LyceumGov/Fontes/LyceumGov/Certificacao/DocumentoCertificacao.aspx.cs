using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Techne.Lyceum.RN;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/DocumentoCertificacao.aspx"),
    ControlText("Cadastro de Informações do Livro"),
    Title("Cadastro de Informações do Livro"),]


    public partial class DocumentoCertificacao : TPage
    {
        public void Insert(object TIPOCONCLUSAOID, object DOCUMENTOID, object NUMERO, object FOLHAS, object LIVRO, object EIXO, object OBSERVACAO) { }

        public void Update(object TIPOCONCLUSAOID, object DOCUMENTOID, object NUMERO, object FOLHAS, object LIVRO, object EIXO, object OBSERVACAO, object DOCUMENTOCERTID) { }
        public void Delete(object DOCUMENTOCERTID) { }



        private readonly RN.Certificacao.DocumentoCertificacao rnDocumentoCertificacao;
        private readonly RN.Certificacao.TipoConclusao rnTipoConclusao;
        private readonly RN.Certificacao.TipoDocumentoCertifica rnTipoDocumentoCertifica;
        private readonly RN.Certificacao.Entidades.DocumentoCertificacao dadosDocumentoCertificacao;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    grdDocumentoCertificacao.Visible = false;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocumentoCertificacao, "Documento Certificação");

        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocumentoCertificacao);

        }

        private ValidacaoDados validacao;

        public DocumentoCertificacao()
        {
            dadosDocumentoCertificacao = new Techne.Lyceum.RN.Certificacao.Entidades.DocumentoCertificacao();
            rnDocumentoCertificacao = new Techne.Lyceum.RN.Certificacao.DocumentoCertificacao();
            rnTipoConclusao = new Techne.Lyceum.RN.Certificacao.TipoConclusao();
            rnTipoDocumentoCertifica = new Techne.Lyceum.RN.Certificacao.TipoDocumentoCertifica();
        }

        protected void grdDocumentoCertificacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                string aluno;

                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (this.tseAluno.IsValidDBValue)
                    {
                        aluno = tseAluno.Value.ToString();
                    }
                    else
                    {
                        throw new Exception("Erro ao obter o Aluno.");
                    }

                    dadosDocumentoCertificacao.DocumentoCertId = e.Keys["DOCUMENTOCERTID"] != null ? Convert.ToInt32(e.Keys["DOCUMENTOCERTID"]) : 0;


                    validacao = rnDocumentoCertificacao.ValidaRemocao(dadosDocumentoCertificacao.DocumentoCertId);


                    //validar os dados e depois fazer o insert

                    if (validacao.Valido)
                    {
                        rnDocumentoCertificacao.Remover(dadosDocumentoCertificacao);

                    }
                    else
                    {
                        e.Cancel = true;

                        throw new Exception(validacao.Mensagem);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        protected void grdDocumentoCertificacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            try
            {
                string aluno;

                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (this.tseAluno.IsValidDBValue)
                    {
                        aluno = tseAluno.Value.ToString();
                    }
                    else
                    {
                        throw new Exception("Erro ao obter o Aluno.");
                    }

                    dadosDocumentoCertificacao.DocumentoCertId = e.NewValues["DOCUMENTOCERTID"] != null ? Convert.ToInt32(e.NewValues["e.DOCUMENTOCERTID"]) : 0;
                    dadosDocumentoCertificacao.Aluno = aluno;
                    dadosDocumentoCertificacao.TipoConclusaoId = Convert.ToInt32(e.NewValues["TIPOCONCLUSAOID"]);
                    dadosDocumentoCertificacao.Numero = e.NewValues["NUMERO"] != null ? Convert.ToString(e.NewValues["NUMERO"]) : null;
                    dadosDocumentoCertificacao.Folhas = e.NewValues["FOLHAS"] != null ? Convert.ToString(e.NewValues["FOLHAS"]) : null;
                    dadosDocumentoCertificacao.Livro = e.NewValues["LIVRO"] != null ? Convert.ToString(e.NewValues["LIVRO"]) : null;
                    dadosDocumentoCertificacao.Eixo = e.NewValues["EIXO"] != null ? e.NewValues["EIXO"].ToString() : null;

                    dadosDocumentoCertificacao.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString() : null;
                    dadosDocumentoCertificacao.DocumentoId = Convert.ToInt32(e.NewValues["DOCUMENTOID"]);
                    dadosDocumentoCertificacao.UsuarioId = User.Identity.Name;
                    dadosDocumentoCertificacao.Pessoa = decimal.Parse(tseAluno["Pessoa"].ToString());

                    validacao = rnDocumentoCertificacao.ValidaInsercao(dadosDocumentoCertificacao);


                    //validar os dados e depois fazer o insert

                    if (validacao.Valido)
                    {
                        rnDocumentoCertificacao.Insere(dadosDocumentoCertificacao);

                    }
                    else
                    {
                        e.Cancel = true;

                        throw new Exception(validacao.Mensagem);
                    }

                }
                else
                {
                    throw new Exception("Erro ao obter o Aluno.");

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        protected void grdDocumentoCertificacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                string aluno;

                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (this.tseAluno.IsValidDBValue)
                    {
                        aluno = tseAluno.Value.ToString();
                    }
                    else
                    {
                        throw new Exception("Erro ao obter o Aluno.");
                    }

                    dadosDocumentoCertificacao.DocumentoCertId = e.Keys["DOCUMENTOCERTID"] != null ? Convert.ToInt32(e.Keys["DOCUMENTOCERTID"]) : 0;
                    dadosDocumentoCertificacao.Aluno = aluno;
                    dadosDocumentoCertificacao.TipoConclusaoId = Convert.ToInt32(e.NewValues["TIPOCONCLUSAOID"]);
                    dadosDocumentoCertificacao.Numero = e.NewValues["NUMERO"] != null ? Convert.ToString(e.NewValues["NUMERO"]) : null;
                    dadosDocumentoCertificacao.Folhas = e.NewValues["FOLHAS"] != null ? Convert.ToString(e.NewValues["FOLHAS"]) : null;
                    dadosDocumentoCertificacao.Livro = e.NewValues["LIVRO"] != null ? Convert.ToString(e.NewValues["LIVRO"]) : null;
                    // não pode mudar
                    dadosDocumentoCertificacao.DocumentoId = Convert.ToInt32(e.NewValues["DOCUMENTOID"]);

                    dadosDocumentoCertificacao.Eixo = e.NewValues["EIXO"] != null ? e.NewValues["EIXO"].ToString() : null;

                    dadosDocumentoCertificacao.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString() : null;

                    dadosDocumentoCertificacao.UsuarioId = User.Identity.Name;

                    dadosDocumentoCertificacao.Pessoa = decimal.Parse(tseAluno["Pessoa"].ToString());

                    validacao = rnDocumentoCertificacao.ValidaAtualizacao(dadosDocumentoCertificacao);


                    //validar os dados e depois fazer o insert

                    if (validacao.Valido)
                    {
                        tseAluno["pessoa"].ToString();

                        rnDocumentoCertificacao.Atualizar(dadosDocumentoCertificacao);

                    }
                    else
                    {
                        e.Cancel = true;

                        throw new Exception(validacao.Mensagem);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected void grdDocumentoCertificacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocumentoCertificacao);

        }

        public object listarDocumentoCertificacao(object aluno)
        {
            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnDocumentoCertificacao.Listar(aluno.ToString());
            }
            return null;
        }

        public object ListarTipoConclusao(object aluno)
        {
            if (!string.IsNullOrEmpty(aluno.ToString()))
            {
                return rnTipoConclusao.Listar(aluno.ToString());
            }
            return null;

        }

        public object ListarTipoDocumentoCertifica()
        {

            var lista = rnTipoDocumentoCertifica.Listar();
            lista.Rows.RemoveAt(0);
            return lista;


        }



        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (!this.tseAluno.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Aluno não encontrado.";
                        grdDocumentoCertificacao.Visible = false;

                    }
                    else
                    {

                        DataTable dt = rnDocumentoCertificacao.Listar(tseAluno.Value.ToString());

                        grdDocumentoCertificacao.Visible = true;

                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um aluno.";
                    grdDocumentoCertificacao.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDocumentoCertificacao_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (this.grdDocumentoCertificacao.IsNewRowEditing)
            {
                if (e.Column.FieldName == "DOCUMENTOID")
                {
                    e.Editor.Enabled = true;
                }
            }


            else if (this.grdDocumentoCertificacao.IsEditing)
            {
                if (e.Column.FieldName == "DOCUMENTOID")
                {
                    e.Editor.ReadOnly = true;
                }


                if (e.Column.FieldName == "TIPOCONCLUSAOID")
                {

                    if (rnDocumentoCertificacao.possuiDocumentoGerado(Convert.ToInt32(e.KeyValue.ToString())))

                        e.Editor.ReadOnly = true;

                }

            }

        }


        #region ControlaVisibilidade

        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Inicial,
            Consultar,
            Sucesso,
            Editar,
            Finalizar
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        private void RetiraVisibilidadeBotao()
        {

        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var img in imgBotoes)
            {
                img.Visible = true;
            }
            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        ControlarVisibilidadeControle(imgControles, controles);
                        grdDocumentoCertificacao.Visible = false;
                        break;

                    }
                    {

                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };


                        ControlarVisibilidadeControle(imgControles, controles);
                        grdDocumentoCertificacao.Visible = true;

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        ControlarVisibilidadeControle(imgControles, controles);

                        grdDocumentoCertificacao.Visible = false;

                        break;
                    }
                case TipoOperacao.Finalizar:
                    {

                        ImageButton[] imgControles = new ImageButton[] { };
                        Button[] controles = new Button[] { };
                        imgControles = new ImageButton[] { };

                        ControlarVisibilidadeControle(imgControles, controles);

                        break;
                    }
            }
        }


        #endregion


    }
}
