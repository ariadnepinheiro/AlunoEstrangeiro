using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace SRV.Common.Extension
{
    public static class ListBoxForExtension
    {
        /// <summary>
        /// Retorna um elemento select HTML para cada propriedade em um objeto que é representado
        /// por específica expressão usando específica lista de itens, option label, e atributos HTML
        /// Adiciona um elemento vazio em elemento select HTML and seleciona este elemento se outros
        /// elementos não estiverem selecionados.
        /// </summary>
        /// <param name="htmlHelper">Instância HTML helper que o método extende.</param>
        /// <param name="expression">Expressão que identifica que objeto que contem as propriedades para exibir.</param>
        /// <param name="selectList">Lista de objetos System.Web.Mvc.SelectListItem que são usados para popular o drop-down list.</param>
        /// <param name="optionLabel">Texto para um item vazio default. Esse parâmetro pode ser nulo.</param>
        /// <param name="htmlAttributes">Objecto que contém os atributos HTML para configurar o elemento.</param>
        /// <typeparam name="TModel">Tipo do modelo.</typeparam>
        /// <typeparam name="TProperty">Tipo do valor.</typeparam>
        /// <returns>Elemento select HTML para cada propriedade em um objeto que é representado pela expressão do parâmetro expression.</returns>
        /// <exceptions>System.ArgumentNullException: O parâmetro expression é nulo.</exceptions>
        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            IList<SelectListItem> items = selectList.ToList<SelectListItem>();

            //Adiciona elemento vazio e seleciona o mesmo se outros elementos não estiverem selecionados
            if (optionLabel != null)
                items.Insert(0, new SelectListItem { Value = "", Text = optionLabel, Selected = (selectList.Count(o => o.Selected == true) == 0) });

            selectList = items.ToArray<SelectListItem>();

            return SelectExtensions.ListBoxFor<TModel, TProperty>(htmlHelper, expression, selectList, htmlAttributes);
        }
    }
}