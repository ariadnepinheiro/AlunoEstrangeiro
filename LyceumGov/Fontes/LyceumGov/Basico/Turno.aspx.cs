using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Controls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/Turno.aspx"),
      ControlText("Turno"),
      Title("Turno"),
    ]

    public partial class Turno : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTurno, "Turnos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTurno);
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

        protected void grdTurno_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTurno);
        }

        protected void grdTurno_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTurno.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "turno")
                    e.Editor.Enabled = true;

            }
            else if (grdTurno.IsEditing)
            {
                if ((e.Column.FieldName) == "turno")
                    e.Editor.Enabled = false;

                if (e.Column.FieldName == "horainicio")
                {
                    e.Editor.Value = e.Editor.Value.ToString().Substring(0, 5);
                }

                if (e.Column.FieldName == "horafim")
                {
                    e.Editor.Value = e.Editor.Value.ToString().Substring(0, 5);
                }
                
            }

            

            

            
        }

        protected void grdTurno_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTurno.Settings.ShowFilterRow = false;

 
            
            
        }

        protected void grdTurno_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTurno.Settings.ShowFilterRow = false;
        }

        protected void grdTurno_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxGridView grd = (ASPxGridView)sender;

            int horaInicio = 0;
            int horaFim = 0;

            if (e.NewValues["horainicio"] != null) {
                horaInicio = int.Parse(e.NewValues["horainicio"].ToString().Replace(":", ""));
            }


            if (e.NewValues["horafim"] != null) {
                horaFim = int.Parse(e.NewValues["horafim"].ToString().Replace(":", ""));
            }

            if (horaInicio >= horaFim)
            {
                e.RowError = "Hora de início deve ser maior do que a hora final.";
                
            }

            //verifica na grid se esta editando, nao deve verificar se o turno existe caso seja uma alteracao.
            if (grd != null && grd.IsNewRowEditing == true)
            {
                int existe = RN.Turno.ConsultaTurno(e.NewValues["turno"].ToString());
                if (existe != 0)
                {
                    e.RowError = "Turno já existente.";
                }
            } 
        }

        protected void grdTurno_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            //RN.Turno.Replicar(Convert.ToString(e.NewValues["turno"].ToString()));
        }

        protected void btnSalvarGrade_Click(object sender, EventArgs e)
        {

            
            CR.Ly_turno dtTurno = new Techne.Lyceum.CR.Ly_turno();
            

            for (int i = 0; i < grdTurno.VisibleRowCount; i++)
            {
                ASPxTextBox txtHoraIni = (grdTurno.FindRowCellTemplateControl(i, (GridViewDataColumn)grdTurno.Columns["horainicio"], "txtBoxIni" ) as ASPxTextBox);
                ASPxTextBox txtHoraFim = (grdTurno.FindRowCellTemplateControl(i, (GridViewDataColumn)grdTurno.Columns["horafim"], "txtBoxFim" ) as ASPxTextBox);

                if (!string.IsNullOrEmpty(txtHoraIni.Text) && txtHoraIni.Text != "  :  " && txtHoraIni.Text != "  :" && !string.IsNullOrEmpty(txtHoraFim.Text))
                {
                    Techne.Lyceum.CR.Ly_turno.Row dadosTurno = dtTurno.NewRow();

                    
                    dadosTurno.Turno = Convert.ToString(grdTurno.GetRowValues(i, "turno")) ;
                    //dadosTurno.Horainicio = Convert.ToString(grdTurno.GetRowValues(i, "horainicio")).ToString().Substring(0, 5);
                    //dadosTurno.Horafim = Convert.ToString(grdTurno.GetRowValues(i, "horafim")).ToString().Substring(0, 5);

                    dadosTurno.Horainicio = Convert.ToString(txtHoraIni.Text.ToString().Substring(0, 5));
                    dadosTurno.Horafim = Convert.ToString(txtHoraFim.Text.ToString().Substring(0, 5));


                    dadosTurno.Descricao = Convert.ToString(grdTurno.GetRowValues(i, "descricao"));
                    dadosTurno.Mnemonico = Convert.ToString(grdTurno.GetRowValues(i,"mnemonico"));

                    
                    dtTurno.Rows.Add(dadosTurno);
                }

            }

            var retorno = RN.HorarioOperacional.SalvarTurno(dtTurno);
            //retorno = RN.Turno.s  .HorarioOperacional.Salvar(dtHorOper);

            //if (retorno != null)
            //{
             //   if (!retorno.Ok)
             //   {
             //       lblMensagem.Text = retorno.Errors.ToString();
              //      return;
             //   }

           // }
            ///if (dtHorOperDel.Rows.Count > 0)
            ///{
                //retorno = RN.HorarioOperacional.Delete(dtHorOperDel);
            //}

            ///if (retorno != null)
            //{
             //   if (!retorno.Ok)
               //     lblMensagem.Text = retorno.Errors.ToString();
            ///    else
            //        lblMensagem.Text = retorno.Message;
           // }

            ///Pesquisar();
        }

























    }
}

