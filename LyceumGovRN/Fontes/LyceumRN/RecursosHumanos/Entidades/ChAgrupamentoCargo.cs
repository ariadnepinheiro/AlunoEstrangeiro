using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.CH_AGRUPAMENTOCARGO", Nome = "RecursosHumanos.CH_AGRUPAMENTOCARGO")]
    public class ChAgrupamentoCargo : IEntity
    {
        [AtributoCampo(Nome = "CH_AGRUPAMENTOCARGOID")]
        public int ChAgrupamentoCargoId { get; set; }

        [AtributoCampo(Nome = "AGRUPAMENTOCARGOSID")]
        public int AgrupamentoCargosId { get; set; }
       
        [AtributoCampo(Nome = "FUNCAO")]
        public string Funcao { get; set; }

        [AtributoCampo(Nome = "CARGAHORARIACOMPLEMENTACAO")]
        public int CargaHorariaComplementacao { get; set; }

        [AtributoCampo(Nome = "CARGAHORARIAREGENCIA")]
        public int CargaHorariaRegencia { get; set; }

        [AtributoCampo(Nome = "CARGAHORARIAPLANEJAMENTO")]
        public int CargaHorariaPlanejamento { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
