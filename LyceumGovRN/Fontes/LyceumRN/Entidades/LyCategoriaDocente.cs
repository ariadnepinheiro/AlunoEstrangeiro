using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyCategoriaDocente
    {
        public string Categoria { get; set; }

        public string Nome { get; set; }

        public string Funcao { get; set; }

        public string Ingresso { get; set; }

        public string NecessitaSuperior { get; set; }

        public string Funcionario { get; set; }

        public string Tipo { get; set; }

        public int AgrupamentoCargosId { get; set; }

        public int CargaHorariaRegencia { get; set; }

        public int CargaHorariaPlanejamento { get; set; }

        public string UsuarioId { get; set; }

        public string DataCadastro { get; set; }

        public string DataAlteracao { get; set; }
    }
}
