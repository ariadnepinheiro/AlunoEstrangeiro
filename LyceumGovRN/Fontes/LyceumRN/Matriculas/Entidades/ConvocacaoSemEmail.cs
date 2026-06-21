using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.CONVOCACAOSEMEMAIL", Nome = "Matricula.CONVOCACAOSEMEMAIL")]
    public class ConvocacaoSemEmail
    {
        [AtributoCampo(Nome = "CONVOCACAOSEMEMAILID")]
        public int ConvocacaoSemEmailId { get; set; }

        [AtributoCampo(Nome = "INSCRICAOALUNOID")]
        public int InscricaoAlunoId { get; set; }

        [AtributoCampo(Nome = "OPCAOINSCRICAOID")]
        public int OpcaoInscricaoId { get; set; }

        [AtributoCampo(Nome = "USUARIORESPONSAVEL")]
        public string UsuarioResponsavel { get; set; }

        [AtributoCampo(Nome = "DATAAVISO")]
        public DateTime DataAviso { get; set; } 
    }
}
