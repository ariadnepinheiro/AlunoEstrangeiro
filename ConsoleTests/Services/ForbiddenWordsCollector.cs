namespace ConsoleTests.Services
{
    using System.Collections.Generic;
    using Seeduc.Infra.Services;

    public class ForbiddenWordsCollector : IForbiddenWordsCollector
    {
        public ICollection<string> GetWords()
        {
            return new[]
                   {
                       "inexistente", 
                       "não cadastrado", 
                       "não informado", 
                       "oculto", 
                       "inválido"
                   };
        }
    }
}