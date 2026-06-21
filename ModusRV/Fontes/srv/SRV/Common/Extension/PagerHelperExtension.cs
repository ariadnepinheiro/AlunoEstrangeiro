using System;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SRV.Models.DTO;

namespace SRV.Common.Extension
{
    public static class PagerHelperExtension
    {

        /// <summary>
        /// Cria um componente paginador utilizando o objeto Paging informado.
        /// </summary>
        /// <param name="helper">HTML Helper</param>
        /// <param name="pagination">Paging</param>
        /// <returns></returns>
        public static IHtmlString Pager<T>(this HtmlHelper helper, Paging<T> pagination)
        {
			return Pager<T>(helper, pagination, "page", "changePage");
        }

        private static string BuildCurrentPage(int currentPage, int totalPages)
        {
            var builder = new TagBuilder("span");
            builder.AddCssClass("currentPage");
            builder.SetInnerText((totalPages > 0 ? currentPage : 0) + " / " + totalPages);

            return builder.ToString(TagRenderMode.Normal);
        }

        private static string BuildTotalItems(int currentPage, int pageSize, int totalItems)
        {
            var builder = new TagBuilder("div");
            builder.AddCssClass("totalItems");

            if (totalItems > 0)
            {
                StringBuilder innerText = new StringBuilder();
                innerText.Append("Mostrando itens ");
                innerText.Append((currentPage - 1) * pageSize + 1);
                innerText.Append(" à ");

                if (currentPage * pageSize < totalItems)
                {
                    innerText.Append(currentPage * pageSize);
                }
                else
                {
                    innerText.Append(totalItems);
                }
                innerText.Append(" de ");
                innerText.Append(totalItems);

                builder.SetInnerText(innerText.ToString());
            }

            return builder.ToString(TagRenderMode.Normal);
        }

		public static IHtmlString Pager<T>(this HtmlHelper helper, Paging<T> pagination, string pageAttributeName, string jsFunction)
		{
			var builder = new TagBuilder("div");
			builder.AddCssClass("pager");

			if (pagination != null)
			{
				StringBuilder innerHtml = new StringBuilder();

				innerHtml.Append("<div class='pagerButtons'>");
				innerHtml.Append(BuildButtonFirst(1, pagination.CurrentPage > 1 ? true : false, jsFunction));
				innerHtml.Append(BuildButtonPrev(pagination.CurrentPage - 1, pagination.CurrentPage > 1 ? true : false, jsFunction));
				innerHtml.Append(BuildCurrentPage(pagination.CurrentPage, pagination.Pages));
				innerHtml.Append(BuildButtonNext(pagination.CurrentPage + 1, pagination.Pages > pagination.CurrentPage ? true : false, jsFunction));
				innerHtml.Append(BuildButtonLast(pagination.Pages, pagination.Pages > pagination.CurrentPage ? true : false, jsFunction));
				innerHtml.Append("</div>");

				innerHtml.Append(String.Format(@"<div class='pageTitle'>{0}", pagination.Title));
				innerHtml.Append("</div>");

				innerHtml.Append(BuildTotalItems(pagination.CurrentPage, pagination.PageSize, pagination.TotalItems));

				string Id = Guid.NewGuid().ToString("N");

				innerHtml.Append(BuildPageInputField(Id, pageAttributeName));
				innerHtml.Append(BuildJavascript(Id, jsFunction));

				builder.InnerHtml = innerHtml.ToString();
			}
			return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
		}

		private static string BuildPageInputField(string Id, string pageAttributeName)
		{
			var builder = new TagBuilder("input");
			builder.Attributes.Add("type", "hidden");
			builder.Attributes.Add("name", pageAttributeName);
			builder.Attributes.Add("id", "page_" + Id);

			return builder.ToString(TagRenderMode.SelfClosing);
		}

		private static string BuildJavascript(string Id, string jsFunction)
		{
			return String.Format(@"<script type=""text/javascript"">
                        function {1}(page) {{
                            $('#page_{0}').val(page);
                            $('#page_{0}').closest('form').submit();
                        }}
                    </script>", Id, jsFunction);
		}

		private static string BuildButtonFirst(int page, bool enabled, string jsFunction)
		{
			return BuildButton("Primeiro", "pageFirst", page, enabled, jsFunction);
		}

		private static string BuildButtonPrev(int page, bool enabled, string jsFunction)
		{
			return BuildButton("Anterior", "pagePrev", page, enabled, jsFunction);
		}

		private static string BuildButtonNext(int page, bool enabled, string jsFunction)
		{
			return BuildButton("Proximo", "pageNext", page, enabled, jsFunction);
		}

		private static string BuildButtonLast(int page, bool enabled, string jsFunction)
		{
			return BuildButton("Ultimo", "pageLast", page, enabled, jsFunction);
		}

		private static string BuildButton(string text, string cssClass, int page, bool enabled, string jsFunction)
		{
			var builderSpan = new TagBuilder("span");
			builderSpan.AddCssClass("pageButton");
			builderSpan.AddCssClass(cssClass + (enabled ? "" : "Disabled"));
			builderSpan.SetInnerText(text);

			if (enabled)
			{
				var builderLink = new TagBuilder("a");
				builderLink.AddCssClass("pageLink");
				builderLink.Attributes.Add("href", "#");
				builderLink.Attributes.Add("onclick", String.Format("{1}({0})", page, jsFunction));
				builderLink.InnerHtml = builderSpan.ToString(TagRenderMode.Normal);

				return builderLink.ToString(TagRenderMode.Normal);
			}

			return builderSpan.ToString(TagRenderMode.Normal);
		}
    }
}