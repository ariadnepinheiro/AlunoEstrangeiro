using System;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/CadastroEstudosAdicionais.aspx")]
    [ControlText("Cadastro de Estudos Adicionais")]
    [Title("Cadastro de Estudos Adicionais")]
    public partial class CadastroEstudosAdicionais : TPage
    { 
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdEstudosAdicionais, "Estudos Adicionais");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.ControlaAcesso(this.grdEstudosAdicionais);
        }

        protected void grdEstudosAdicionais_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs asPxGridViewAfterPerformCallbackEventArgs)
        {
            this.ControlaAcesso(this.grdEstudosAdicionais);
        }

        protected void grdEstudosAdicionais_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            RN.EstudoAdicional rnEstudoAdicional = new RN.EstudoAdicional();
            ValidacaoDados validacao = new ValidacaoDados();

            int estudoAdicionalId = Convert.ToInt32(e.Values["ESTUDOADICIONALID"]);

            validacao = rnEstudoAdicional.ValidaRemocao(estudoAdicionalId);

            if (validacao.Valido)
            {
                rnEstudoAdicional.Remove(estudoAdicionalId);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }            

            e.Cancel = true;
            this.grdEstudosAdicionais.CancelEdit();
        }

        protected void grdEstudosAdicionais_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            RN.EstudoAdicional rnEstudoAdicional = new RN.EstudoAdicional();
            ValidacaoDados validacao = new ValidacaoDados();

            Techne.Lyceum.RN.Entidades.EstudoAdicional estudoAdicional = new Techne.Lyceum.RN.Entidades.EstudoAdicional
            {
                Nome =  e.NewValues["NOME"].ToString()
            };

            validacao = rnEstudoAdicional.Valida(estudoAdicional);

            if (validacao.Valido)
            {
                rnEstudoAdicional.Insere(estudoAdicional);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            e.Cancel = true;
            this.grdEstudosAdicionais.CancelEdit();
        }

        protected void grdEstudosAdicionais_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            RN.EstudoAdicional rnEstudoAdicional = new RN.EstudoAdicional();
            ValidacaoDados validacao = new ValidacaoDados();

            Techne.Lyceum.RN.Entidades.EstudoAdicional estudoAdicional = new Techne.Lyceum.RN.Entidades.EstudoAdicional
            {
                EstudoAdicionalId = Convert.ToInt32(e.Keys["ESTUDOADICIONALID"]),
                Nome = e.NewValues["NOME"].ToString()
            };

            validacao = rnEstudoAdicional.Valida(estudoAdicional);

            if (validacao.Valido)
            {
                rnEstudoAdicional.Altera(estudoAdicional);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            e.Cancel = true;
            this.grdEstudosAdicionais.CancelEdit();
        }

        protected void grdEstudosAdicionais_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            RN.EstudoAdicional rnEstudoAdicional = new RN.EstudoAdicional();
            ValidacaoDados validacao = new ValidacaoDados();

            Techne.Lyceum.RN.Entidades.EstudoAdicional estudoAdicional = new Techne.Lyceum.RN.Entidades.EstudoAdicional
            {
                EstudoAdicionalId = Convert.ToInt32(e.Keys["ESTUDOADICIONALID"]),
                Nome = e.NewValues["NOME"].ToString()
            };

            validacao = rnEstudoAdicional.Valida(estudoAdicional);

            if (!validacao.Valido)
            {
                e.RowError = validacao.Mensagem;
            }
        }

        public object Listar()
        {
            RN.EstudoAdicional rnEstudoAdicional = new RN.EstudoAdicional();
            DataTable listaEstudoAdicional = null;
            listaEstudoAdicional = rnEstudoAdicional.ListaEstudoAdicional();

            return listaEstudoAdicional;
        }
    }
}
