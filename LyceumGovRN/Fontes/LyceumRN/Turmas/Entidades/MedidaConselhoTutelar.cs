using System;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.MEDIDACONSELHOTUTELAR", Nome = "Turma.MEDIDACONSELHOTUTELAR")]
    public class MedidaConselhoTutelar : IEntity
    {
        [AtributoCampo(Nome = "MEDIDACONSELHOTUTELARID")]
        public int MedidasConselhoTutelarId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
