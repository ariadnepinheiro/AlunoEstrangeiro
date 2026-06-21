using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.CURSOSERIEMENSAGENS", Nome = "Matricula.CURSOSERIEMENSAGENS")]
    public class CursoSerieMensagens
    {
        [AtributoCampo(Nome = "CURSOSERIEMENSAGENSID")]
        public int CursoSerieMensagensId { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Cor { get; set; }

        [AtributoCampo(Nome = "EXIBEPOPUP")]
        public bool ExibePopup { get; set; }

        public string Descricao { get; set; }

        public string Atencao { get; set; }

        public string Declaracao { get; set; }

        [AtributoCampo(Nome = "LINKTEXTO")]
        public string LinkTexto { get; set; }

        public string Link { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
