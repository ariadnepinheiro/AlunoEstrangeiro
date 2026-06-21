namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class TabelaItem : IEntity
    {
        public string Descr { get; set; }

        public string Item { get; set; }
    }
}