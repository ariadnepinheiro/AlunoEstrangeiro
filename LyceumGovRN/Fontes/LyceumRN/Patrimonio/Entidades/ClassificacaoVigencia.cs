using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    public class ClassificacaoVigencia
    {
        public int ClassificacaoVigenciaId { get; set; }

        public int ClassificacaoId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public int VidaUtil { get; set; }

        public decimal TaxaValorResidual { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
