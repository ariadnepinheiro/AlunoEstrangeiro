using System;
using System.Collections;
using System.Data;
using System.Web.UI.Design;

namespace Techne.Controls
{
    public class TTableDataViewDesigner : DesignerDataSourceView
    {
        protected TTableDataSourceDesigner _owner;

        public TTableDataViewDesigner(TTableDataSourceDesigner owner, string viewName)
            : base(owner, viewName)
        {
            this._owner = owner;
        }

        public override IDataSourceViewSchema Schema
        {
            get
            {
                // if a type and the select method have been specified, the schema information is available
                if (!String.IsNullOrEmpty(this._owner.DataTableClassName))
                {
                    return this._owner.DataSourceSchema;
                }
                else
                {
                    return null;
                }
            }
        }

        public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
        {
            var dt = this._owner.CreateTableInstance();
            if (dt != null)
            {
                try
                {
                    var zeros = 1;
                    int rowNum;
                    if (minimumRows > 0)
                    {
                        zeros = (int)Math.Ceiling(Math.Log10(minimumRows));
                    }

                    if (zeros < 1)
                    {
                        zeros = 1;
                    }

                    var stringFormat = "{0} {1:" + new string('0', zeros) + "}";
                    var rowDate = DateTime.Now;
                    rowDate = new DateTime(rowDate.Year, rowDate.Month, rowDate.Day, 11, 30, 0);
                    for (var i = 0; i < minimumRows; i++)
                    {
                        var row = dt.NewRow();
                        rowNum = i + 1;
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.DataType == typeof (string))
                            {
                                row[col.ColumnName] = string.Format(stringFormat, col.ColumnName, rowNum);
                            }
                            else if (col.DataType == typeof (DateTime))
                            {
                                row[col.ColumnName] = rowDate.AddDays(-1.0 * i);
                            }
                            else
                            {
                                try
                                {
                                    row[col.ColumnName] = (decimal)rowNum;
                                }
                                catch
                                {
                                }
                            }
                        }

                        dt.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                }
            }

            if (dt != null && dt.Rows.Count >= minimumRows)
            {
                isSampleData = false;
                return dt.DefaultView;
            }
            else
            {
                return base.GetDesignTimeData(minimumRows, out isSampleData);
            }
        }
    }
}