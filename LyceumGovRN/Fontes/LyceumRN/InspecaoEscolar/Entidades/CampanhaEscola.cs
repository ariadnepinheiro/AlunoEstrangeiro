using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.CampanhaEscola", Nome = "InspecaoEscolar.CampanhaEscola")]

    public class CampanhaEscola : IEntity
    {
        [AtributoCampo(Nome = "CAMPANHAESCOLAID")]
        public int CampanhaEscolaId { get; set; }

        [AtributoCampo(Nome = "CAMPANHAID")]
        public int CampanhaId { get; set; }

        [AtributoCampo(Nome = "UNIDADE_ENS")]
        public string Unidade_Ens { get; set; }

        [AtributoCampo(Nome = "FINALIZADO")]
        public Boolean? Finalizado { get; set; }

        [AtributoCampo(Nome = "POSSUIACERVO")]
        public Boolean? PossuiAcervo { get; set; }

        [AtributoCampo(Nome = "DATAFINALIZACAO")]
        public DateTime? DataFinalizacao { get; set; }

        [AtributoCampo(Nome = "CONSIDERACAOFINAL")]
        public string ConsideracaoFinal { get; set; }

        [AtributoCampo(Nome = "ACEITO")]
        public Boolean? Aceito { get; set; }

        [AtributoCampo(Nome = "DATAACEITE")]
        public DateTime? DataAceite { get; set; }

        [AtributoCampo(Nome = "USUARIOACEITEID")]
        public string UsuarioAceiteId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}




