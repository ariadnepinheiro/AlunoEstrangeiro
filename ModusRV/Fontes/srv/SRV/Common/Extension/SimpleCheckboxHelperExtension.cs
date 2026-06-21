using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Common.Extension
{
    public static class SimpleCheckboxHelperExtension
    {

        public static MvcHtmlString SimpleCheckbox(this HtmlHelper helper, string name, bool isChecked, object htmlAttributes)
        {
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var check = new TagBuilder("input");

            check.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);
            check.Attributes.Add("type", "checkbox");
            check.Attributes.Add("name", fullName);

            if (isChecked)
                check.MergeAttribute("checked", "checked");

            check.GenerateId(fullName);

            return MvcHtmlString.Create(check.ToString(TagRenderMode.SelfClosing));

        }


        public static MvcHtmlString CheckboxCheckAll(this HtmlHelper helper, string name, string checkboxName, object htmlAttributes)
        {
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var check = new TagBuilder("input");

            check.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);
            check.Attributes.Add("type", "checkbox");
            check.Attributes.Add("name", fullName);
            check.Attributes.Add("onclick", String.Format("changeCheckboxAll('{0}', '{1}')", name, checkboxName));

            check.GenerateId(fullName);

            return MvcHtmlString.Create(BuildJavascript() + check.ToString(TagRenderMode.SelfClosing));

        }

        private static string BuildJavascript()
        {
            return @"<script type=""text/javascript"">
                        function changeCheckboxAll(checkbox, checkboxFor) {
                        
                            if($(""input[name='"" + checkbox + ""']"").attr(""checked""))
                                $(""input[name='"" + checkboxFor + ""']"").attr(""checked"", true);
                            else
                                $(""input[name='"" + checkboxFor + ""']"").attr(""checked"", false);
                        }
                    </script>";

        }

    }
}