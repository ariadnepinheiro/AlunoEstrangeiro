using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.VAGARESERVADA", Nome = "Matricula.VAGARESERVADA")]
    public class VagaReservada
    {
        [AtributoCampo(Nome = "CONTATOOPCAOINSCRICAOID")]
        public int VagaReservadaId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }
        
        [AtributoCampo(Nome = "CONTROLEVAGAID")]
        public int ControleVagaId { get; set; }

         [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }      
    }
}
