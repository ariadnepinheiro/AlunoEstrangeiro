using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    public class CargaHNaoTrabMes
    {
        [AtributoCampo(Nome = "ID_CARGAHNAOTRABMES")]
        public int IdCargaHNaoTrabMes { get; set; }

        [AtributoCampo(Nome = "NUM_FUNC")]
        public decimal NumFunc { get; set; }

        [AtributoCampo(Nome = "UNIDADE_ENS")]
        public string UnidadeEns { get; set; }
        
        [AtributoCampo(Nome = "ANO")]
        public int Ano { get; set; }

        [AtributoCampo(Nome = "MES")]
        public int Mes { get; set; }

        [AtributoCampo(Nome = "CHMENSAL")]
        public int ChMensal { get; set; }

        [AtributoCampo(Nome = "CH_SEMANAL")]
        public int? ChSemanal { get; set; }

        [AtributoCampo(Nome = "CHNAOTRABALHADAMES")]
        public int ChNaoTrabalhadaMes { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
