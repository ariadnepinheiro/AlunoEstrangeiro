using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    public class AgenteResponsavel
    {
        public int AgenteResponsavelId { get; set; }

        public string Setor { get; set; }

        public string Matricula { get; set; }

        public DateTime DataNomeacao { get; set; }

        public DateTime? DataDispensa { get; set; }

        public DateTime? DataPublicacaoNomeacao { get; set; }

        public DateTime? DataPublicacaoDispensa { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
