using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosBemPatrimonial
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

        public string Historico { get; set; }

        public int MoedaId { get; set; }

        public int EstadoconservacaoId { get; set; }

        public decimal UlimoValor { get; set; }

        public decimal ValorAtualizado { get; set; }

        public int VidaFutura { get; set; }

        public int? VidaUtilizada { get; set; }

        public bool Baixa { get; set; }

        public int? MotivoBaixaId { get; set; }

        public DateTime? DataBaixa { get; set; }

        public string ProcessoBaixa { get; set; }

        public string JustificativaBaixa { get; set; }

        public string BoletimOcorrencia { get; set; }

        public string InstituicaoDestino { get; set; }

        public string CnpjInstituicaoDestino { get; set; }

        public int Numero { get; set; }
    }
}
