using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosOcorrencia
    {
        public int OcorrenciaId { get; set; }

        public string NumeroOcorrencia { get; set; }

        public string Censo { get; set; }

        public string Regional { get; set; }

        public string Escola { get; set; }

        public string Municipio { get; set; }

        public string Bairro { get; set; }

        public DateTime DataOcorrencia { get; set; }

        public int ClasseId { get; set; }

        public int? SubClasseId { get; set; }        

        public string Relato { get; set; }

        public int MeioId { get; set; }

        public int? DelegaciaId { get; set; }

        public int? BatalhaoId { get; set; }

        public string RegistroOcorrencia { get; set; }

        public bool? UsoArma { get; set; }

        public string Observacao { get; set; }

        public bool Arquivada { get; set; }

        public bool? Interrupcao { get; set; }        

        public List<int> TiposArma { get; set; } // 1 - Branca, 2 - Fogo, 3 - Artefato

        public bool Ativo { get; set; }

        public int? MotivoCancelamentoId { get; set; }

        public string UsuarioId { get; set; }

        public string UsuarioCadastro { get; set; }
    }
}
