using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Common.Extension
{
    public static class ScrollerHelperExtension
    {
        /// <summary>
        /// Inicia o Scroller com largura padrão de 100%
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <returns></returns>
        public static Scroller BeginScroller(this HtmlHelper htmlHelper)
        {
            // cria o <div> ... </div>
            return ScrollerHelper(htmlHelper, null, null);
        }

        /// <summary>
        /// Inicia o Scroller, com largura customizada
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Scroller BeginScroller(this HtmlHelper htmlHelper, string width)
        {
            // cria o <div> ... </div>
            return ScrollerHelper(htmlHelper, width, null);
        }

        /// <summary>
        /// Inicia o Scroller, com largura e altura customizada
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Scroller BeginScroller(this HtmlHelper htmlHelper, string width, string height)
        {
            // cria o <div> ... </div>
            return ScrollerHelper(htmlHelper, width, height);
        }

        /// <summary>
        /// Finaliza o Scroller.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        public static void EndScroller(this HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div></div>");
        }

        /// <summary>
        /// Cria o HTML do scroller. A largura padrão, caso não informada, será "100%"
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private static Scroller ScrollerHelper(this HtmlHelper htmlHelper, string width, string height)
        {
            if (width == null)
                width = "100%";

            string heightAttr = "";
            string overflowYAttr = "hidden";

            if (height != null)
            {
                heightAttr = String.Format("height: {0};", height);
                overflowYAttr = "auto";
            }

            string startTag = String.Format(@"<div style='width: 100%; overflow: hidden;'>
                                                <div style='width: {0}; {1} overflow-x: scroll; overflow-y: {2};'>", width, heightAttr, overflowYAttr);

            htmlHelper.ViewContext.Writer.Write(startTag);
            Scroller scroller = new Scroller(htmlHelper.ViewContext);

            return scroller;
        }

    }
}