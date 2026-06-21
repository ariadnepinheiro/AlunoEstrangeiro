using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.InspecaoEscolar.Entidades
{
    [AtributoTabela("InspecaoEscolar.RESPOSTADEPENDENCIA", Nome = "InspecaoEscolar.RESPOSTADEPENDENCIA")]

    public class RespostaDependencia : IEntity
    {
        [AtributoCampo(Nome = "RESPOSTADEPENDENCIAID")]
        public int RespostaDependenciaId { get; set; }
        
        [AtributoCampo(Nome = "CAMPANHAESCOLAID")]
        public int CampanhaEscolaId { get; set; }

        [AtributoCampo(Nome = "DEPENDENCIA")]
        public string Dependencia { get; set; }

        [AtributoCampo(Nome = "FACULDADE")]
        public string Faculdade { get; set; }

        [AtributoCampo(Nome = "PLACAIDENTIFICACAO")]
        public Boolean? PlacaIdentificacao { get; set; }

        [AtributoCampo(Nome = "IDENTIFICACAODEPENDENCIAID")]
        public int? IdentificacaoDependenciaId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }


    }
}




