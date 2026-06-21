using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Common.Extension
{
    public static class ToolbarItemHelperExtension
    {

        public static MvcHtmlString ToolbarItemSubmit(this HtmlHelper helper, bool? render, string imageClass)
        {
            return ToolbarItemSubmit(helper, render, imageClass, null);
        }

        public static MvcHtmlString ToolbarItemSubmit(this HtmlHelper helper, bool? render, string imageClass, string tooltip)
        {
            if (render != null && render.Value)
            {
                var div = new TagBuilder("div");
                div.AddCssClass("toolbarItem");

                if (tooltip != null)
                    div.Attributes.Add("title", tooltip);

                var img = new TagBuilder("input");
                img.Attributes.Add("style", "vertical-align: bottom; border:0;");
                img.Attributes.Add("type", "image");
                img.Attributes.Add("value", "");
                img.Attributes.Add("src", VirtualPathUtility.ToAbsolute("~/Content/images/pix.gif"));
                img.Attributes.Add("class", imageClass);

                div.InnerHtml = img.ToString(TagRenderMode.SelfClosing);

                return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
            }
            else
                return new MvcHtmlString("");
        }

        public static MvcHtmlString ToolbarItemLink(this HtmlHelper helper, bool? render, string actionName, string controllerName, object routeValues, string imageClass)
        {
            return ToolbarItemLink(helper, render, actionName, controllerName, routeValues, null, imageClass);
        }

        public static MvcHtmlString ToolbarItemLink(this HtmlHelper helper, bool? render, string actionName, string controllerName, object routeValues, object htmlAttributes, string imageClass)
        {
            return ToolbarItemLink(helper, render, actionName, controllerName, routeValues, htmlAttributes, imageClass, null);
        }

        public static MvcHtmlString ToolbarItemLink(this HtmlHelper helper, bool? render, string actionName, string controllerName, object routeValues, object htmlAttributes, string imageClass, string tooltip)
        {
            if (render != null && render.Value)
            {
                var div = new TagBuilder("div");
                div.AddCssClass("toolbarItem");

                if (tooltip != null)
                    div.Attributes.Add("title", tooltip);

                var link = new TagBuilder("a");
                link.Attributes.Add("href", (new UrlHelper(HttpContext.Current.Request.RequestContext)).Action(actionName, controllerName, routeValues));
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

                link.AddCssClass(imageClass);

                div.InnerHtml = link.ToString(TagRenderMode.Normal);

                return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
            }
            else
                return new MvcHtmlString("");
        }

        public static MvcHtmlString ToolbarItemJS(this HtmlHelper helper, bool? render, string javascriptFunction, object htmlAttributes, string imageClass)
        {
            return ToolbarItemJS(helper, render, javascriptFunction, htmlAttributes, imageClass, null);
        }

        public static MvcHtmlString ToolbarItemJS(this HtmlHelper helper, bool? render, string javascriptFunction, object htmlAttributes, string imageClass, string tooltip)
        {
            if (render != null && render.Value)
            {
                var div = new TagBuilder("div");
                div.AddCssClass("toolbarItem");

                if (tooltip != null)
                    div.Attributes.Add("title", tooltip);

                var link = new TagBuilder("a");
                link.Attributes.Add("href", "#");
                link.Attributes.Add("onclick", javascriptFunction);
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

                link.AddCssClass(imageClass);

                div.InnerHtml = link.ToString(TagRenderMode.Normal);

                return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
            }
            else
                return new MvcHtmlString("");
        }


    }
}