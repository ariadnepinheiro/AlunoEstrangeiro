using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceMunicipalizacao: IEntity
    {
        public int IdMunicipalizacao { get; set; }

        public string Processo { get; set; }

        public DateTime? DtPublicacaoDo { get; set; }

        public string PaginaDo { get; set; }

        public string NumAutorizoProvisorio { get; set; }

        public DateTime DtAutorizoProvisorio { get; set; }

        public DateTime DtValidadeAutorizo { get; set; }

        public string Censo { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? DtAlteracao { get; set; }

    }
}
