using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.TipoAssunto", Nome = "InspecaoEscolar.TipoAssunto")]

    public class TipoAssunto : IEntity
    {
        [AtributoCampo(Nome = "TipoAssuntoId")]
        public int TipoAssuntoId { get; set; }

        [AtributoCampo(Nome = "Descricao")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "Ativo")]
        public Boolean Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }


    }
}




