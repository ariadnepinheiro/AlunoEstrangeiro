namespace Techne.Lyceum.RN.Servicos
{
    using System.Collections.Generic;
    using Seeduc.Infra.Services;

    public class PalavrasProibidasEmNomes : IForbiddenWordsCollector
    {
        public ICollection<string> GetWords()
        {
            return new[]
                   {
                       "inexistente", 
                       "não cadastrado", 
                       "não informado", 
                       "oculto", 
                       "inválido",
                       "nada disso"                       
                   };
        }
    }
}