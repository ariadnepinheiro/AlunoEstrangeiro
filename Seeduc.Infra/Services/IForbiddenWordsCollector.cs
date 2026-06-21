namespace Seeduc.Infra.Services
{
    using System.Collections.Generic;

    public interface IForbiddenWordsCollector
    {
        ICollection<string> GetWords();
    }
}