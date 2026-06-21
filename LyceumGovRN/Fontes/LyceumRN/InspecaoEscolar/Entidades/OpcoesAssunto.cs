using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.OpcoesAssunto", Nome = "InspecaoEscolar.OpcoesAssunto")]

    public class OpcoesAssunto : IEntity
    {
        [AtributoCampo(Nome = "OpcoesAssuntoId")]
        public int OpcoesAssuntoId { get; set; }

        [AtributoCampo(Nome = "Descricao")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "Ordem")]
        public int Ordem { get; set; }

        [AtributoCampo(Nome = "AssuntoId")]
        public int AssuntoId { get; set; }

        [AtributoCampo(Nome = "AcaodeDirecao")]
        public Boolean AcaodeDirecao { get; set; }

        [AtributoCampo(Nome = "Restritivo")]
        public Boolean Restritivo { get; set; }



        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }


    }
}




