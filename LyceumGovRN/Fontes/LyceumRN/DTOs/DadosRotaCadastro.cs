using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosRotaCadastro
    {
        //Dados Rota        
        public int RotaId { get; set; }

        public string Censo { get; set; }

        public string Turno { get; set; }

        public int Ordem { get; set; }

        public string Codigo { get; set; }

        public bool Ativo { get; set; }

        public int TipoCalculoPagamentoId { get; set; }

        //Dados Trajeto Ida
        public int RotaTrajetoIdIda { get; set; }

        public int TipoContratacaoIdIda { get; set; }

        public decimal ValorRotaIda { get; set; }

        public decimal QuantidadeKmIda { get; set; }

        public int? TempoIda { get; set; }

        //Dados Trajeto Volta
        public int RotaTrajetoIdVolta { get; set; }

        public int TipoContratacaoIdVolta { get; set; }

        public decimal ValorRotaVolta { get; set; }

        public decimal QuantidadeKmVolta { get; set; }

        public int? TempoVolta { get; set; }

        //Primeiro Ponto Embarque Ida
        public int PrimeiroPontoEmbarqueIdIda { get; set; }

        public string PrimeiroCepIda { get; set; }

        public string PrimeiroLogradouroIda { get; set; }

        public string PrimeiroNumeroIda { get; set; }

        public string PrimeiroBairroIda { get; set; }

        public string PrimeiroMunicipioIda { get; set; }

        public string PrimeiroLatitudeIda { get; set; }

        public string PrimeiroLongitudeIda { get; set; }

        //Primeiro Ponto Embarque Volta
        public int PrimeiroPontoEmbarqueIdVolta { get; set; }

        public string PrimeiroCepVolta { get; set; }

        public string PrimeiroLogradouroVolta { get; set; }

        public string PrimeiroNumeroVolta { get; set; }

        public string PrimeiroBairroVolta { get; set; }

        public string PrimeiroMunicipioVolta { get; set; }

        public string PrimeiroLatitudeVolta { get; set; }

        public string PrimeiroLongitudeVolta { get; set; }

        public string UsuarioResponsavel { get; set; }
    }
}
