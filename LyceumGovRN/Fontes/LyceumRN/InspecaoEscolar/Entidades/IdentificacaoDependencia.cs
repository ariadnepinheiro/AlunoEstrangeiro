using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.IdentificacaoDependencia", Nome = "InspecaoEscolar.IdentificacaoDependencia")]

    public class IdentificacaoDependencia : IEntity
    {
        [AtributoCampo(Nome = "IdentificacaoDependenciaId")]
        public int IdentificacaoDependenciaId { get; set; }

        [AtributoCampo(Nome = "Descricao")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "Sigla")]
        public string Sigla { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }


    }
}




