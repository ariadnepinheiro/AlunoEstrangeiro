using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Techne.Data;

namespace Techne.Controls.Design
{
    internal class TDataGridBaseDesigner : DataGridDesigner
    {
        public TDataGridBaseDesigner()
        {
            // Adiciona Verb
            DesignerVerb dvOutrasColunas;
            int i;
            for (i = 0; i < this.Verbs.Count; i++)
            {
                if (this.Verbs[i].Text == "Outras Colunas ...")
                {
                    break;
                }
            }

            if (i == this.Verbs.Count)
            {
                dvOutrasColunas = new DesignerVerb("Outras Colunas ...", 
                                                   this.dvOutrasColunas_Click);
                this.Verbs.Add(dvOutrasColunas);
                dvOutrasColunas.Enabled = true;
            }
        }

        public override string GetDesignTimeHtml()
        {
            var grid = (TDataGridBase)this.Component;

            // Troca gifs
            var sAsc = grid.SortAscImageUrl;
            var sDesc = grid.SortDescImageUrl;
            var sNoSort = grid.NoSortImageUrl;

            grid.SortAscImageUrl = TUtil.TranslateRelativeUrl(grid.SortAscImageUrl, grid);
            grid.SortDescImageUrl = TUtil.TranslateRelativeUrl(grid.SortDescImageUrl, grid);
            grid.NoSortImageUrl = TUtil.TranslateRelativeUrl(grid.NoSortImageUrl, grid);

            

            // Determina o DataTable associado à grid
            TDataTable tab = null;
            foreach (DataBinding dataBinding in this.DataBindings)
            {
                if (dataBinding.PropertyName == "DataSource")
                {
                    object obj2 = base.Component.Site.Container.Components[dataBinding.Expression];
                    if ((obj2 is TDataSet) && (grid.DataMember.Length > 0))
                    {
                        var set = (TDataSet)obj2;
                        if (!set.Tables.Contains(grid.DataMember))
                        {
                            continue;
                        }

                        tab = set.Tables[grid.DataMember] as TDataTable;
                        break;
                    }

                    if (obj2 is TDataTable)
                    {
                        tab = (TDataTable)obj2;
                        break;
                    }
                }
            }

            var originalTitle = grid.Title;
            var originalHeaders = new ArrayList();
            if (tab != null)
            {
                // Substitui o header de cada uma das colunas se possível
                foreach (DataGridColumn gridColumn in grid.Columns)
                {
                    originalHeaders.Add(gridColumn.HeaderText);
                    gridColumn.HeaderText = grid.GetCaption(gridColumn);
                }

                if (grid.Title == "?")
                {
                    var title = tab.Name;
                    if (title.Length == 0)
                    {
                        title = tab.TableName;
                    }

                    grid.Title = StrLib.ToProper(title);
                }
            }

            

            var html = base.GetDesignTimeHtml();

            grid.SortAscImageUrl = sAsc;
            grid.SortDescImageUrl = sDesc;
            grid.NoSortImageUrl = sNoSort;

            if (tab != null)
            {
                grid.Title = originalTitle;
                for (var i = 0; i < grid.Columns.Count; i++)
                {
                    grid.Columns[i].HeaderText = (string)originalHeaders[i];
                }
            }

            return html;
        }

        protected override string GetErrorDesignTimeHtml(Exception exc)
        {
            Trace.WriteLine("TDataGridBaseDesigner.GetErrorDesignTimeHtml(): " + exc.Message + "\r\n\r\n" + exc.StackTrace);
            return base.GetErrorDesignTimeHtml(exc);
        }

        protected void dvOutrasColunas_Click(object sender, EventArgs e)
        {
            try
            {
                IComponentChangeService changeService = null;

                if (this.Component.Site != null)
                {
                    changeService = (IComponentChangeService)this.Component.Site.GetService(typeof (IComponentChangeService));
                    if (changeService != null)
                    {
                        try
                        {
                            changeService.OnComponentChanging(this.Component, null);
                        }
                        catch (CheckoutException exc)
                        {
                            if (exc == CheckoutException.Canceled)
                            {
                                return;
                            }

                            throw exc;
                        }
                    }
                }

                DialogResult result;
                try
                {
                    var frm = new TDataGridColumnCollectionEditorForm(((System.Web.UI.WebControls.DataGrid)this.Component).Columns);
                    result = frm.ShowDialog();
                }
                finally
                {
                    if (changeService != null)
                    {
                        changeService.OnComponentChanged(this.Component, null, null, null);
                    }
                }

                if (result == DialogResult.OK)
                {
                    this.OnColumnsChanged();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}