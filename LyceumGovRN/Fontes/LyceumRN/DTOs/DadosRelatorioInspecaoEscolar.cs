using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.InspecaoEscolar.Entidades;

namespace Techne.Lyceum.RN.DTOs
{
    //Grupos
    public class DadosRelatorioInspecaoGrupo
    {
        public DadosRelatorioInspecaoGrupo()
        {
            ListaAssunto = new List<DadosRelatorioInspecaoAssunto>();
        }

        public int GrupoId { get; set; }

        public int CampanhaId { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public List<DadosRelatorioInspecaoAssunto> ListaAssunto { get; set; }
    }

    //Assuntos
    public class DadosRelatorioInspecaoAssunto
    {
        public DadosRelatorioInspecaoAssunto()
        {
            ListaOpcao = new List<DadosRelatorioInspecaoOpcao>();
        }

        public int AssuntoId { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public int GrupoId { get; set; }

        public int TipoAssuntoId { get; set; }

        public Boolean AcaodeDirecao { get; set; }

        public int? IdPaiAssuntoId { get; set; }

        public List<DadosRelatorioInspecaoOpcao> ListaOpcao { get; set; }
    }

    //Opções
    public class DadosRelatorioInspecaoOpcao
    {
        public int OpcoesAssuntoId { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public int AssuntoId { get; set; }

        public Boolean AcaodeDirecao { get; set; }

        public Boolean Restritivo { get; set; }

        public string valor { get; set; }
    }

}
