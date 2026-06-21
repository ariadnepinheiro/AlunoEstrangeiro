using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{ 
    [AtributoTabela("InspecaoEscolar.Acervo", Nome = "InspecaoEscolar.Acervo")]
    public class Acervo : IEntity
    { 
        [AtributoCampo(Nome = "ACERVOID")]
        public int AcervoId { get; set; }

        [AtributoCampo(Nome = "MEDIDAID")]
        public int MedidaId { get; set; }

        [AtributoCampo(Nome = "INSTITUICAOID")]
        public string InstituicaoId { get; set; }

        [AtributoCampo(Nome = "CAMPANHAESCOLAID")]
        public int CampanhaEscolaId { get; set; }

        [AtributoCampo(Nome = "SITUACAO")]
        public string Situacao { get; set; }

        [AtributoCampo(Nome = "ATO")]
        public string Ato { get; set; }

        [AtributoCampo(Nome = "VOLUME")]
        public int Volume { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
        
    }
}
