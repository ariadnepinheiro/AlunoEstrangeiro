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
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/MovimentacaoServidores.aspx"),
ControlText("MovimentacaoServidores"),
Title("Movimentação de Servidores"),]
    public partial class MovimentacaoServidores : TPage
    {
        public void Update(object dtfim, object num_func, object dtini) { }

        public object Listar(string pessoa)
        {
            RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();

            if (!string.IsNullOrEmpty(pessoa))
                return rnLotacao.ObtemListaLotacaoPor(int.Parse(pessoa));

            return null;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdLotacao, "Movimentação de Servidores");
        }
        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdLotacao);
        }

        protected void tseServidor_Changed(object sender, EventArgs args)
        {
            RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();

            if (tseServidor.IsValidDBValue && !tseServidor.DBValue.IsNull)
            {
                grdLotacao.Visible = true;
                lblMensagem.Text = string.Empty;
                txtPessoaHidden.Text = tseServidor["pessoa"].ToString();
                if (rnLotacao.ExisteMatriculaExtraClasseAtivaPor(tseServidor["matricula"].ToString()))
                    lblMensagem.Text = "Esta id/Vinculo ou matrícula possui função extra-classe e não pode ser movimentada.";
            }
            else if (!tseServidor.DBValue.IsNull)
            {
                grdLotacao.Visible = false;
                lblMensagem.Text = "Servidor não cadastrado ou não está em função de regência com lotação ativa.";
                txtPessoaHidden.Text = string.Empty;
            }
            else
            {
                grdLotacao.Visible = false;
                lblMensagem.Text = "Favor consultar um servidor.";
                txtPessoaHidden.Text = string.Empty;
            }
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

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

        protected void grdLotacao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdLotacao);
        }
        protected void grdLotacao_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditFormEventArgs e)
        {
            //se o usuário for privilegiado não tem where, se ele for de um núcleo possuirá filtro
            if (RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
            {
                return;
            }
            else
            {
                RN.Setores rnSetores = new Techne.Lyceum.RN.Setores();
                //string matricula_usuario = RN.Usuarios.ObterMatriculaUsuario(HttpContext.Current.User.Identity.Name);
                //string setor_usuario = RN.Lotacao.ObterSetorUsuario(matricula_usuario);
                //string tipoSetor = rnSetores.ObtemTipoSetorPor(setor_usuario);
                //TSearchBox tseSetor = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseSetor");

                //if (tseSetor != null)
                //{
                //    if (!string.IsNullOrEmpty(setor_usuario))
                //    {
                //        if (!tipoSetor.IsNullOrEmptyOrWhiteSpace())
                //        {
                //            tseSetor.SqlWhere = "ue.UNIDADE_ENS is not null and USUARIO = '" + matricula_usuario + "'";
                //        }
                //        else
                //        {
                //            tseSetor.SqlWhere = " USUARIO = '" + matricula_usuario + "'";
                //        }
                //    }
                //}
            }
        }

        protected void odsLotacao_Movimentacao(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
            ValidacaoDados validacao = new ValidacaoDados();
            DadosMovimentacaoServidor dadosMovimentacao;

            TSearchBox tseSetor = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseSetor");

            dadosMovimentacao = new DadosMovimentacaoServidor
            {
                Pessoa = int.Parse(tseServidor["pessoa"].ToString()),
                SetorDestino = (!tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue) ? tseSetor["setor"].ToString(): null ,
                Ordem =  int.Parse(e.InputParameters["ordem"].ToString()),
                Matricula = (!tseServidor.DBValue.IsNull && tseServidor.IsValidDBValue) ? tseServidor["matricula"].ToString() : null,
                UsuarioResponsavel = User.Identity.Name,
                DataMovimentacao = DateTime.Now.AddDays(-1)
            };

            validacao = rnLotacao.ValidaMovimentacaoServidor(dadosMovimentacao);

            if (validacao.Valido)
            {
                rnLotacao.MovimentaServidor(dadosMovimentacao);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            e.Cancel = true;
            this.grdLotacao.CancelEdit();
        }
    }
}
