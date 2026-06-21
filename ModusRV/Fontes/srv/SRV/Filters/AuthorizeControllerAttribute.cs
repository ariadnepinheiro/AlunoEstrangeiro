using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Filters
{
    /// <summary>
    /// Atributo que indica qual o controller que contém as permissões referente as funções da toolbar.
    /// Deve ser utilizado nos controllers referentes a telas de detalhe que não tem o cadastro de permissões no banco.
    /// O nome do controller deve ser informado sem o sufixo "Controller"
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AuthorizeControllerAttribute : Attribute
    {
        public string Controller { get; set; }

        public AuthorizeControllerAttribute(string controllerName)
        {
            Controller = controllerName;
        }
    }
}