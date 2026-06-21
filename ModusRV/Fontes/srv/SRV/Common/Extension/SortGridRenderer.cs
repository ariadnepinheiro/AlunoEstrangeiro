using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.UI.Grid;
using System.Web.Mvc;
using MvcContrib.Sorting;
using System.Web.Routing;

namespace SRV.Common.Extension
{
    public class SortGridRenderer<T> : HtmlTableGridRenderer<T> where T: class
    {
        protected override void RenderGridStart()
        {
            RenderText(BuildJavascript());
            RenderText(BuildSortInputField(GridModel.SortOptions.Column));
            RenderText(BuildDirectionInputField(GridModel.SortOptions.Direction));

            base.RenderGridStart();
        }

        protected override void RenderHeaderText(GridColumn<T> column)
        {
            if (IsSortingEnabled && column.Sortable)
            {
                string sortColumnName = GenerateSortColumnName(column);

                bool isSortedByThisColumn = GridModel.SortOptions.Column == sortColumnName;

                var sortOptions = new GridSortOptions
                {
                    Column = sortColumnName
                };

                if (isSortedByThisColumn)
                {
                    sortOptions.Direction = (GridModel.SortOptions.Direction == SortDirection.Ascending)
                        ? SortDirection.Descending
                        : SortDirection.Ascending;
                }
                else //default sort order
                {
                    sortOptions.Direction = GridModel.SortOptions.Direction;
                }

                //var routeValues = CreateRouteValuesForSortOptions(sortOptions, GridModel.SortPrefix);
                var routeValues = new RouteValueDictionary();

                //Re-add existing querystring
                foreach (var key in Context.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
                {
                    if (!routeValues.ContainsKey(key))
                    {
                        routeValues[key] = Context.RequestContext.HttpContext.Request.QueryString[key];
                    }
                }

                //var link = HtmlHelper.GenerateLink(Context.RequestContext, RouteTable.Routes, column.DisplayName, null, null, null, routeValues, null);
                
                var link = "<a href=\"#\" onclick=\"sortGrid('" + sortOptions.Column + "','" + sortOptions.Direction +  "')\">" + column.DisplayName + "</a>";
                
                RenderText(link);
            }
            else
            {
                base.RenderHeaderText(column);
            }
        }



        private static string BuildJavascript()
        {
            return @"<script type=""text/javascript"">
                        function sortGrid(sortColumn, direction) {
                            $(""#Direction"").val(direction);
                            $(""#Column"").val(sortColumn);
                            $(""#Column"").closest(""form"").submit();
                        }
                    </script>";

        }

        private static string BuildSortInputField(string column)
        {
            var builder = new TagBuilder("input");
            builder.Attributes.Add("type", "hidden");
            builder.Attributes.Add("name", "Column");
            builder.Attributes.Add("id", "Column");

            builder.Attributes.Add("value", column);

            return builder.ToString(TagRenderMode.SelfClosing);
        }

        private static string BuildDirectionInputField(SortDirection direction)
        {
            var builder = new TagBuilder("input");
            builder.Attributes.Add("type", "hidden");
            builder.Attributes.Add("name", "Direction");
            builder.Attributes.Add("id", "Direction");

            builder.Attributes.Add("value", direction.ToString());

            return builder.ToString(TagRenderMode.SelfClosing);
        }


    }
}