using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosRota
    {
        //Dados Rota        
        public int RotaId { get; set; }

        public int Regional { get; set; }

        public string Situacao { get; set; }

        public string RegionalDescricao { get; set; }

        public string Municipio { get; set; }

        public string MunicipioDescricao { get; set; }

        public string Censo { get; set; }

        public bool Ativo { get; set; }

        public int RegiaoFinanceiraId { get; set; }

        public string RegiaoFinanceiraDescricao { get; set; }

        public string Cnpj { get; set; }

        public string Turno { get; set; }

        public int Ordem { get; set; }

        public string Codigo { get; set; }

        public int TipoCalculoPagamentoId { get; set; }

        public string TipoCalculoPagamento { get; set; }

        //Dados Trajeto Ida
        public int RotaTrajetoIdIda { get; set; }

        public int PrestadorIdIda { get; set; }

        public int CondutorIdIda { get; set; }

        public int VeiculoIdIda { get; set; }

        public int TipoContratacaoIdIda { get; set; }

        public string TipoContratacaoDescricaoIda { get; set; }

        public decimal ValorRotaIda { get; set; }

        public decimal QuantidadeKmIda { get; set; }

        public int? TempoIda { get; set; }

        //Dados Trajeto Volta
        public int RotaTrajetoIdVolta { get; set; }

        public int PrestadorIdVolta { get; set; }

        public int CondutorIdVolta { get; set; }

        public int VeiculoIdVolta { get; set; }

        public int TipoContratacaoIdVolta { get; set; }

        public string TipoContratacaoDescricaoVolta { get; set; }

        public decimal ValorRotaVolta { get; set; }

        public decimal QuantidadeKmVolta { get; set; }

        public int? TempoVolta { get; set; }

        public string UsuarioResponsavel { get; set; }

        public int QuantidadeAlunosIda { get; set; }

        public int QuantidadeAlunosVolta { get; set; }
    }
}
