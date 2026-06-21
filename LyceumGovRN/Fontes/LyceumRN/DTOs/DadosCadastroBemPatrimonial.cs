using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosCadastroBemPatrimonial
    {
        public int BemId { get; set; }

        public int OperacaoId { get; set; }

        public int ClassificacaoId { get; set; }

        public string Descricao { get; set; }

        public string Setor { get; set; }

        public DateTime DataAquisicao { get; set; }

        public DateTime DataIncorporacao { get; set; }

        public string DocumentoHabil { get; set; }

        public decimal? ValorMercado { get; set; }

        public int? VidaUtil { get; set; }

        public int? VidaUtilizada { get; set; }

        public int? VidaFutura { get; set; }

        public string Historico { get; set; }

        public int MoedaId { get; set; }

        public int EstadoconservacaoId { get; set; }

        public decimal Valor { get; set; }        

        public string UsuarioId { get; set; }

        public string Numero { get; set; }

        public string NumeroInicial { get; set; }

        public int Quantidade { get; set; }
    }
}
