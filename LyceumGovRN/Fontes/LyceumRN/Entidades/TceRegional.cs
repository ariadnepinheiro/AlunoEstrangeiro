using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceRegional: IEntity
    {
        public int IdRegional { get; set; }

        public string Regional { get; set; }

        public string Cep { get; set; }

        public string Municipio { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public string Uf { get; set; }

        public string NomeMunicipio { get; set; }


    }
}
