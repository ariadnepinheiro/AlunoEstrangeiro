using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Common.Extension
{
    public static class ToolbarHelperExtension
    {

        /// <summary>
        /// Inicia a Toolbar, com a classe css padrão, "toolbar"
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <returns></returns>
        public static Toolbar BeginToolbar(this HtmlHelper htmlHelper)
        {
            // cria o <div> ... </div>
            return ToolbarHelper(htmlHelper, null);
        }

        /// <summary>
        /// Inicia a Toolbar, com classe css customizada
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static Toolbar BeginToolbar(this HtmlHelper htmlHelper, string cssClass)
        {
            // cria o <div> ... </div>
            return ToolbarHelper(htmlHelper, cssClass);
        }

        /// <summary>
        /// Inicia a Toolbar interna utilizada dentro de tab panels
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static Toolbar BeginInternalToolbar(this HtmlHelper htmlHelper)
        {
            // cria o <div> ... </div>
            return ToolbarHelper(htmlHelper, "toolbarInternal");
        }

        /// <summary>
        /// Finaliza a Toolbar.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        public static void EndToolbar(this HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div></div>");
        }

        /// <summary>
        /// Cria o HTML da toolbar. A classe css padrão, caso não informada, será "toolbar"
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="htmlAttributes">HTML attributes.</param>
        /// <returns></returns>
        private static Toolbar ToolbarHelper(this HtmlHelper htmlHelper, string cssClass)
        {
            if (cssClass == null)
                cssClass = "toolbar";

            string startTag = String.Format("<div class =\"{0}\"><div class =\"toolbarBorder\">", cssClass);

            htmlHelper.ViewContext.Writer.Write(startTag);
            Toolbar toolbar = new Toolbar(htmlHelper.ViewContext);

            return toolbar;
        }

    }
}