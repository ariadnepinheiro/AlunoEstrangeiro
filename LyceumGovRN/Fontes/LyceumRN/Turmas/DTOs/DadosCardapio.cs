using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Turmas.DTOs
{
    public class DadosCardapio
    {
        public string Censo { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public int CardapioEletivaId { get; set; }

        public string DisciplinaManha1 { get; set; }

        public string DisciplinaManha2 { get; set; }

        public string DisciplinaTarde1 { get; set; }

        public string DisciplinaTarde2 { get; set; }

        public string DisciplinaNoite1 { get; set; }

        public string DisciplinaNoite2 { get; set; }

        public string DisciplinaIntegral1 { get; set; }

        public string DisciplinaIntegral2 { get; set; }

        public string DisciplinaAmpliado1 { get; set; }

        public string DisciplinaAmpliado2 { get; set; }

        public bool Validado { get; set; }

        public string UsuarioValidacao { get; set; }

        public string NomeUsuarioValidacao { get; set; }

        public DateTime? DataValidacao { get; set; }

        public bool Finalizado { get; set; }

        public string UsuarioFinalizacao { get; set; }

        public string NomeUsuarioFinalizacao { get; set; }

        public DateTime? DataFinalizacao { get; set; }

        public string UsuarioId { get; set; }
    }
}
