using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.Assunto", Nome = "InspecaoEscolar.Assunto")]

    public class Assunto : IEntity
    {
        [AtributoCampo(Nome = "AssuntoId")]
        public int AssuntoId { get; set; }

        [AtributoCampo(Nome = "Descricao")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "Ordem")]
        public int? Ordem { get; set; }

        [AtributoCampo(Nome = "GrupoId")]
        public int? GrupoId { get; set; }

        [AtributoCampo(Nome = "TipoAssuntoId")]
        public int? TipoAssuntoId { get; set; }

        [AtributoCampo(Nome = "AcaodeDirecao")]
        public Boolean AcaodeDirecao { get; set; }

        [AtributoCampo(Nome = "IdPaiAssuntoId")]
        public int? IdPaiAssuntoId { get; set; }

        [AtributoCampo(Nome = "RESTRICAO")]
        public int Restricao { get; set; } 

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}




