using System;

namespace Techne.Web
{
    /// <summary>
    ///   Página destino da navegação.
    ///   É recomendado utilizar o path completo, onde a raiz é representada por '~'.
    /// </summary>
    [
        AttributeUsage(AttributeTargets.Class), 
    ]
    public class NavUrlAttribute : Attribute
    {
        private readonly string url;

        public NavUrlAttribute(string url)
        {
            this.url = url;
        }

        public string Url
        {
            get
            {
                return this.url;
            }
        }
    }

    // Inicialmente o atributo NavParameter não poderá ser utilizado, pois ele pode "estragar"
    // a formalização dos parâmetros de uma página se não for bem utilizado.
    // Ele foi criado com a idéia inicial de se passar o parâmetro "manager". Assim, para substituí-lo
    // foi criado o atributo NavManager.
    [
        AttributeUsage(AttributeTargets.Method, AllowMultiple = true), 
    ]
    internal class NavParameterAttribute : Attribute
    {
        private readonly string parameter;

        private readonly string value;

        internal NavParameterAttribute(string parameter, string value)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException();
            }

            this.parameter = parameter;
            this.value = value;
        }

        public string Parameter
        {
            get
            {
                return this.parameter;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
        }
    }

    [
        AttributeUsage(AttributeTargets.Method), 
    ]
    internal class NavReturnAttribute : Attribute
    {
        private readonly bool canReturn;

        public NavReturnAttribute(bool canReturn)
        {
            this.canReturn = canReturn;
        }

        public bool Return
        {
            get
            {
                return this.canReturn;
            }
        }
    }

    [
        AttributeUsage(AttributeTargets.Method, AllowMultiple = false), 
    ]
    internal class NavManagerAttribute : NavParameterAttribute
    {
        public NavManagerAttribute(string managerId) : base("manager", managerId)
        {
        }
    }
}