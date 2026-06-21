using System;


namespace Seeduc.Infra.MapeamentoAtributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ParamentrosQueryString : Attribute
    {
        public string Nome { get; set; }
        public bool Disponivel { get; set; }

        public ParamentrosQueryString()
        {
            
            Nome = string.Empty;
            Disponivel = true;
        }
    }
}
