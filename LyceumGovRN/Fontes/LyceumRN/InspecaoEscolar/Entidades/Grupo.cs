using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.Grupo", Nome = "InspecaoEscolar.Grupo")]
    public  class Grupo:IEntity
    {
        [AtributoCampo(Nome = "GrupoId")]
        public int GrupoId { get; set; }

        [AtributoCampo(Nome = "CampanhaId")]
        public int CampanhaId { get; set; }

        [AtributoCampo(Nome = "Descricao")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "Ordem")]
        public int Ordem { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        public List<Assunto> listAssunto;

    }
}
