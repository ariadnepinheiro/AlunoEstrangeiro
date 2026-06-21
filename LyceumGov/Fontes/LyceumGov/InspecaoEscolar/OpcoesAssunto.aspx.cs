using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Collections.Specialized;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Data;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.Net;

namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/OpcoesAssunto.aspx"),
   ControlText("Opções de Resposta"),
   Title("Opções de Resposta"),]

    public partial class OpcoesAssunto : TPage
    {
        public void Insert(object DESCRICAO, object ORDEM, object ACAODEDIRECAO, object RESTRITIVO) { }
        public void Delete(object opcoesAssuntoId) { }

        public void Update(object DESCRICAO, object ORDEM, object ACAODEDIRECAO, object RESTRITIVO, object OPCOESASSUNTOID) { }

        private readonly RN.InspecaoEscolar.Campanha CampanhaRN;
        private readonly RN.InspecaoEscolar.Grupo GrupoRN;
        private readonly RN.InspecaoEscolar.TipoAssunto TipoAssuntoRN;
        private readonly RN.InspecaoEscolar.Assunto AssuntoRN;
        private readonly RN.InspecaoEscolar.OpcoesAssunto OpcoesAssuntoRN;



        private RN.InspecaoEscolar.Entidades.OpcoesAssunto OpcoesAssuntoDados;

        private ValidacaoDados validacao;



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ListarAno();
                    DropDownList[] dropdown = new DropDownList[] { ddlSemestre, ddlTituloCampanha, ddlGrupo, ddlAssunto };
                    LimpaDdl(dropdown);                 
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdOPAssunto);
        }

        public OpcoesAssunto()
        {
            CampanhaRN = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();
            GrupoRN = new Techne.Lyceum.RN.InspecaoEscolar.Grupo();
            AssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
            TipoAssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.TipoAssunto();
            OpcoesAssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.OpcoesAssunto();

            OpcoesAssuntoDados = new Techne.Lyceum.RN.InspecaoEscolar.Entidades.OpcoesAssunto();


            validacao = new ValidacaoDados();

        }

        #region Métodos_Grid

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdOPAssunto, "Opções de Resposta");
        }

        public object ListarAssuntoOpcoes(int assuntoid)
        {
            DataTable assunto = new DataTable();
            assunto = null;
            try
            {

                if (assuntoid == -1 || assuntoid == 0)
                {
                    return assunto;

                }
                else
                {                  
                    assunto = OpcoesAssuntoRN.ListaOpcaoAssuntoPor(assuntoid);
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return assunto;
        }

        protected void grdOPAssunto_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                int opAssuntoId = Convert.ToInt32(e.Keys["OPCOESASSUNTOID"]);


                if (opAssuntoId == 0)
                { throw new Exception("Erro ao obter o ASSUNTO."); }


                validacao = OpcoesAssuntoRN.ValidaRemocao(opAssuntoId);

                if (validacao.Valido)
                {
                    OpcoesAssuntoRN.Remover(opAssuntoId);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem);
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected void grdOPAssunto_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            try
            {
                int opcoesassuntoid = 0;
                int assuntoId = Convert.ToInt32(ddlAssunto.SelectedIndex > 0 ? ddlAssunto.SelectedValue : "-1");


                if (assuntoId == -1)
                { throw new Exception("Erro ao obter o ASSUNTO."); }

                //pegar os dados do panel
                PegaDadosnaTela(e.NewValues, opcoesassuntoid, assuntoId);

                validacao = OpcoesAssuntoRN.Valida(OpcoesAssuntoDados);
                //validar os dados e depois fazer o insert


                if (validacao.Valido)
                {

                    OpcoesAssuntoRN.Insere(OpcoesAssuntoDados);
                    ListarAssuntoOpcoes(Convert.ToInt32(ddlAssunto.SelectedItem.Value));
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem);
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        protected void grdOPAssunto_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                int opcoesassuntoid = Convert.ToInt32(e.Keys["OPCOESASSUNTOID"]);
                int assuntoId = Convert.ToInt32(ddlAssunto.SelectedIndex > 0 ? ddlAssunto.SelectedValue : "-1");


                if (assuntoId == -1)
                { throw new Exception("Erro ao obter o ASSUNTO."); }

                //pegar os dados do panel
                PegaDadosnaTela(e.NewValues, opcoesassuntoid, assuntoId);

                validacao = OpcoesAssuntoRN.Valida(OpcoesAssuntoDados);
                //validar os dados e depois fazer o insert


                if (validacao.Valido)
                {

                    OpcoesAssuntoRN.Atualiza(OpcoesAssuntoDados);
                    ListarAssuntoOpcoes(Convert.ToInt32(ddlAssunto.SelectedItem.Value));
                    //lblMensagem.Text = "Incluido com suceso!";
                }
                else
                {
                    e.Cancel = true;
                    // lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    throw new Exception(validacao.Mensagem);
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion


        #region Métodos_Panel

        public void ListarAno()
        {

            ddlAno.DataSource = CampanhaRN.ListarAno();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", "-1"));
            limpaGrid();

        }

        public void ListarSemestre(int ano)
        {
            DropDownList[] dropdown = new DropDownList[] { ddlSemestre, ddlTituloCampanha, ddlGrupo, ddlAssunto };
            LimpaDdl(dropdown);

            if (ano != -1)
            {

                ddlSemestre.DataSource = CampanhaRN.ListarSemestreporAno(ano);
                ddlSemestre.DataBind();
                ddlSemestre.Items.Insert(0, new ListItem("Selecione", "-1"));
                
            }



        }

        public void ListarCampanhaporSemestreAno(int ano, int semestre)
        {
            DropDownList[] dropdown = new DropDownList[] { ddlTituloCampanha, ddlGrupo, ddlAssunto };
            LimpaDdl(dropdown);
            if (ano != -1 && semestre != -1)
            {

                ddlTituloCampanha.DataSource = CampanhaRN.ListarCampanha(ano, semestre);
                ddlTituloCampanha.DataBind();
                ddlTituloCampanha.Items.Insert(0, new ListItem("Selecione", "-1"));
                
            }

        }

        public void ListaGrupoporCampanha(int campanhaId)
        {
            DropDownList[] dropdown = new DropDownList[] { ddlGrupo, ddlAssunto };
            LimpaDdl(dropdown);
            if (campanhaId != -1)
            {
                ddlGrupo.DataSource = GrupoRN.ListarGrupoporCampanha(campanhaId);
                ddlGrupo.DataBind();
                ddlGrupo.Items.Insert(0, new ListItem("Selecione", "-1"));
               
            }

        }

        public void ListarOpcoesAssuntoporAssunto(int assuntoId)
        {

            DropDownList[] dropdown = new DropDownList[] { ddlGrupo, ddlAssunto };
            LimpaDdl(dropdown);

            if (assuntoId != -1)
            {

                ddlAssunto.DataSource = GrupoRN.ListarGrupoporCampanha(assuntoId);
                ddlAssunto.DataBind();
                ddlAssunto.Items.Insert(0, new ListItem("Selecione", "-1"));
             
            }

        }

        public void ListaAssunto(int grupoId)
        {

            try
            {
                DropDownList[] dropdown = new DropDownList[] { ddlAssunto };
                LimpaDdl(dropdown);
                if (grupoId != -1)
                {

                    ddlAssunto.DataSource = AssuntoRN.ListarAssuntoSimples(grupoId);
                    ddlAssunto.DataBind();
                    ddlAssunto.Items.Insert(0, new ListItem("Selecione", "-1"));

                    if (ddlAssunto.Items.Count == 1)
                    {
                        lblobsResposta.Text = "Não existem perguntas cadastradas.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }


        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? ano = Convert.ToInt32(ddlAno.SelectedItem.Value);

                if (ano == null)
                {
                    ListarSemestre(Convert.ToInt32(-1));
                }
                else
                {
                    ListarSemestre(Convert.ToInt32(ano));
                  
                }
            }
            catch (Exception)
            {

                throw;
            }




        }

        protected void ddlSemestre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                int? ano, semestre;
                ano = Convert.ToInt32(ddlAno.SelectedItem.Value);
                semestre = Convert.ToInt32(ddlSemestre.SelectedItem.Value);

                if (ano == null || semestre == null)
                {
                    ListarCampanhaporSemestreAno(-1, -1);
                }
                else
                {
                    ListarCampanhaporSemestreAno(Convert.ToInt32(ano), Convert.ToInt32(semestre));
                }


            }
            catch (Exception)
            {

                throw new Exception("Erro ao listar campanha. ");
            }

        }

        protected void ddlTituloCampanha_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                int? campanhaid;
                campanhaid = Convert.ToInt32(ddlTituloCampanha.SelectedItem.Value);

                if (campanhaid == null)
                {
                    ListaGrupoporCampanha(-1);
                }
                else
                {
                    ListaGrupoporCampanha(Convert.ToInt32(campanhaid));
                }

            }
            catch (Exception)
            {

                throw new Exception("Erro ao listar grupo. ");
            }



        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int? grupoid;
                grupoid = Convert.ToInt32(ddlGrupo.SelectedItem.Value);

                if (grupoid == null)
                {
                    ListaAssunto(-1);
                }
                else
                {
                    ListaAssunto(Convert.ToInt32(grupoid));
                }

            }
            catch (Exception)
            {

                throw new Exception("Erro ao listar perguntas. ");
            }

        }

        protected void ddlAssunto_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                //chamar método que retorna o tipo da resposta
                int? assuntoId = Convert.ToInt32(ddlAssunto.SelectedItem.Value);

                if (assuntoId == null || assuntoId == -1)
                {
                    limpaGrid();
                }
                else
                {
                    int tipoAssuntoid = AssuntoRN.ListaTipoAssuntoId(Convert.ToInt32(assuntoId));

                    ProcessaOpcoes(tipoAssuntoid,Convert.ToInt32(assuntoId));
                }
               
            }
            catch (Exception)
            {

                throw new Exception("Erro ao carregar grid. ");
            }

        }

        #endregion

        #region Utilidades


        public void limpaGrid()
        {           
            lblobsResposta.Text = string.Empty;
            this.grdOPAssunto.DataBind();
            this.grdOPAssunto.Visible = false;
        }

        public void LimpaDdl(DropDownList[] ddl)
        {
            // passarei o nome do ddl que

            foreach (var nomeddl in ddl)
            {
                nomeddl.DataSource = null;
                nomeddl.DataBind();
                nomeddl.Items.Clear();
               
            }
            limpaGrid();

        }



        public void ProcessaOpcoes(int tipoAssuntoId, int assuntoid)
        {
            lblobsResposta.Text = string.Empty;
            string msg = string.Empty;

            //2	 MULTIPLA ESCOLHA COM 1 OPÇÃO
            //3	 MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES
            //4	 DESCRITIVA
            //5	 SEM RESPOSTA
            //6	 DEPENDÊNCIAS - SALA DE AULA
            //7	 DEPENDÊNCIAS - BANHEIRO
            //8	 CONSIDERAÇÕES FINAIS - DESCRITIVA
            //9	 CONSIDERAÇÕES FINAIS - MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES
            //10 CONSIDERAÇÕES FINAIS - MULTIPLA ESCOLHA COM 1 OPÇÃO
            //11 CONSIDERAÇÕES FINAIS - SEM RESPOSTA

            switch (tipoAssuntoId)
            {
                case 2:
                case 10:
                    msg = "O tipo da resposta selecionada é 'MULTIPLA ESCOLHA COM 1 OPÇÃO'.</br> ";
                    msg += "Cadastre uma ou mais opções de resposta informando a ordem desejada para ser apresentada no formulário de respostas.</br>";
                    msg += "Opções de resposta restritivas são opções únicas, ou seja, caso o usuário selecione essa opção no formulário de respostas, todas as outras serão desmarcadas.";

                    lblobsResposta.Text = msg;
                    ListarAssuntoOpcoes(assuntoid);
                    this.grdOPAssunto.Visible = true;

                    break;
                case 3:
                case 9:
                    msg = "O tipo da resposta selecionada é 'MULTIPLA ESCOLHA COM VÁRIAS OPÇÕES'.</br> ";
                    msg += "Cadastre uma ou mais opções de resposta informando a ordem desejada para ser apresentada no formulário de respostas.</br>";
                    msg += "Opções de resposta restritivas são opções únicas, ou seja, caso o usuário selecione essa opção no formulário de respostas, todas as outras serão desmarcadas.";

                    lblobsResposta.Text = msg;

                    ListarAssuntoOpcoes(assuntoid);
                    this.grdOPAssunto.Visible = true;

                    break;
                case 4:
                case 5:
                case 8:
                case 11:
                    msg = "O tipo da resposta selecionada é 'DESCRITIVA' ou 'SEM RESPOSTA', não necessita de resposta. ";

                    lblobsResposta.Text = msg;
                    this.grdOPAssunto.Visible = false;
                    break;

                case 6:
                case 7:

                    msg = "O tipo da resposta selecionada é 'DEPENDÊNCIAS - SALA DE AULA  OU BANHEIRO'.</br> ";
                    msg += "Cadastre uma ou mais opções de resposta informando a ordem desejada para ser apresentada no formulário de respostas.</br>";
                    msg += "Opções de resposta restritivas são opções únicas, ou seja, caso o usuário selecione essa opção no formulário de respostas, todas as outras serão desmarcadas.";

                    lblobsResposta.Text = msg;

                    ListarAssuntoOpcoes(assuntoid);
                    this.grdOPAssunto.Visible = true;
                    break;


                default:
                    ListarAssuntoOpcoes(assuntoid);
                    this.grdOPAssunto.Visible = false;
                    break;
            }



        }

        private void PegaDadosnaTela(OrderedDictionary od, int opcoesAssuntoId, int assuntoId)
        {
            try
            {
                OpcoesAssuntoDados.OpcoesAssuntoId = opcoesAssuntoId;
                OpcoesAssuntoDados.Descricao = od["DESCRICAO"] != null ? od["DESCRICAO"].ToString() : string.Empty;
                OpcoesAssuntoDados.Ordem = od["ORDEM"] != null ? Convert.ToInt32(od["ORDEM"]) : 0;
                OpcoesAssuntoDados.AssuntoId = assuntoId;
                OpcoesAssuntoDados.AcaodeDirecao = Convert.ToBoolean(od["ACAODEDIRECAO"]);
                OpcoesAssuntoDados.Restritivo = Convert.ToBoolean(od["RESTRITIVO"]);
                OpcoesAssuntoDados.UsuarioId = User.Identity.Name;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " PegaDadosnaTela ");
            }
        }

        #endregion

        protected void grdOPAssunto_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ListarAssuntoOpcoes(Convert.ToInt32(ddlAssunto.SelectedItem.Value));
        }
        protected void grdOPAssunto_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdOPAssunto.Settings.ShowFilterRow = false;
        } 


        protected void grdOPAssunto_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdOPAssunto.Settings.ShowFilterRow = false;
        }    

    }
}
