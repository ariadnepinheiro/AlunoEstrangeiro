using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.InspecaoEscolar.Entidades;

namespace Techne.Lyceum.RN.DTOs
{
    //Grupos
    public class DadosCompetenciaCurriculo
    {
        public DadosCompetenciaCurriculo()
        {
            ListaItem = new List<DadosCompetenciaItem>();
        }

        public int GrupoId { get; set; }

        public string Grupo { get; set; }

        public int Ordem { get; set; }

        public int Subperiodo { get; set; }

        public string Tipo { get; set; }

        public List<DadosCompetenciaItem> ListaItem { get; set; }
    }

    //Itens
    public class DadosCompetenciaItem
    {
        public int Ordem { get; set; }

        public int GrupoId { get; set; }

        public int ItemId { get; set; }

        public string CompetenciaHabilidade { get; set; }    
    }   

}
