using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.RESPOSTAASSUNTO", Nome = "InspecaoEscolar.RESPOSTAASSUNTO")]

    public class RespostaAssunto : IEntity
    {
        [AtributoCampo(Nome = "RESPOSTAASSUNTOID")]
        public int RespostaAssuntoId { get; set; }

        [AtributoCampo(Nome = "ASSUNTOID")]
        public int AssuntoId { get; set; }

        [AtributoCampo(Nome = "CAMPANHAESCOLAID")]
        public int CampanhaEscolaId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "OPCOESASSUNTOID")]
        public int? OpcoesAssuntoId { get; set; }

        [AtributoCampo(Nome = "ACAODIRECAOID")]
        public int? AcaoDirecaoId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}




