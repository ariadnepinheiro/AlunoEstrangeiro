using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    [AtributoTabela("FiscalizacaoLink.CONTRATOSETOR", Nome = "FiscalizacaoLink.CONTRATOSETOR")]
    public class ContratoSetor : IEntity
    {
        [AtributoCampo(Nome = "CONTRATOSETORID")]
        public int ContratoSetorId { get; set; }

        [AtributoCampo(Nome = "SETORID")]
        public string SetorId { get; set; }

        [AtributoCampo(Nome = "CONTRATOID")]
        public int ContratoId { get; set; }

        [AtributoCampo(Nome = "DATACONTRATACAO")]
        public DateTime? DataContratacao { get; set; }//TOOD: campo novo alimentar

        [AtributoCampo(Nome = "DATAIMPLANTACAO")]
        public DateTime? DataImplantacao { get; set; }

        [AtributoCampo(Nome = "DATAHOMOLOGACAO")]
        public DateTime? DataHomologacao { get; set; }//TOOD: nao vai mais existir

        [AtributoCampo(Nome = "DATATERMINO")]
        public DateTime? DataTermino { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
