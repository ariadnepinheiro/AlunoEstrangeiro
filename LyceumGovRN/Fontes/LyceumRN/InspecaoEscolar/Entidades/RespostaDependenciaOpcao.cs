using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.RespostaDependenciaOpcao", Nome = "InspecaoEscolar.RespostaDependenciaOpcao")]

    public class RespostaDependenciaOpcao : IEntity
    {
        [AtributoCampo(Nome = "RespostaDependenciaOpcaoId")]
        public int RespostaDependenciaOpcaoId { get; set; }

        [AtributoCampo(Nome = "RespostaDependenciaId")]
        public int RespostaDependenciaId { get; set; }

        [AtributoCampo(Nome = "OpcoesAssuntoId")]
        public int OpcoesAssuntoId { get; set; }

        [AtributoCampo(Nome = "AcaoDirecaoId")]
        public int AcaoDirecaoId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}




