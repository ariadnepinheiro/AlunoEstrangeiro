using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.Util
{
    public class GridViewTemplate : ITemplate
    {
        //A variable to hold the type of ListItemType.
        ListItemType _templateType;

        //A variable to hold the column name.
        string _columnName;

        //A variable to hold the column value.
        string _value;

        //Constructor where we define the template type and column name.
        public GridViewTemplate(ListItemType type, string colname, string value)
        {
            //Stores the template type.
            _templateType = type;

            //Stores the column name.
            _columnName = colname;

            //Stores the column name.
            _value = value;
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templateType)
            {
                case ListItemType.Header:
                    //Creates a new label control and add it to the container.
                    Label lbl = new Label();            //Allocates the new label object.
                    lbl.Text = _columnName;             //Assigns the name of the column in the lable.
                    container.Controls.Add(lbl);        //Adds the newly created label control to the container.

                    break;
                case ListItemType.Item:

                    HtmlTable tbl = new HtmlTable();
                    HtmlTableRow row = new HtmlTableRow();
                    HtmlTableCell cell = new HtmlTableCell();
                    
                    row.Cells.Add(cell);
                    tbl.Rows.Add(row);
                    //Form1.controls.add(tbl);

                    //Creates a new text box control and add it to the container.
                    Label lblTurmaTurno = new Label();                            //Allocates the new text box object.
                    lblTurmaTurno.Text = _value;
                    container.Controls.Add(lblTurmaTurno);                            //Adds the newly created textbox to the container.

                    //Creates a new text box control and add it to the container.
                    TextBox txtVagasContinuidade = new TextBox();                            //Allocates the new text box object.
                    txtVagasContinuidade.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                    txtVagasContinuidade.Columns = 20;                                        //Creates a column with size 4.
                    container.Controls.Add(txtVagasContinuidade);                            //Adds the newly created textbox to the container.

                    //Creates a new text box control and add it to the container.
                    TextBox txtVagasNovas = new TextBox();                            //Allocates the new text box object.
                    txtVagasNovas.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                    txtVagasNovas.Columns = 20;                                        //Creates a column with size 4.
                    container.Controls.Add(txtVagasNovas);                            //Adds the newly created textbox to the container.
                    
                    break;
            }
        }

        /// <summary>
        /// This is the event, which will be raised when the binding happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void tb1_DataBinding(object sender, EventArgs e)
        {
            TextBox txtdata = (TextBox)sender;
            GridViewRow container = (GridViewRow)txtdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            if (dataValue != DBNull.Value)
            {
                txtdata.Text = dataValue.ToString();
            }
        }
    }
}
