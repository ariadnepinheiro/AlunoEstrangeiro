using System;

namespace Seeduc.Infra.MapeamentoAtributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AtributoCampo 
        : Attribute
    {
        public string Nome { get; set; }
        
        public AtributoCampo()
        {
            Nome = string.Empty;
        }
    }
}
