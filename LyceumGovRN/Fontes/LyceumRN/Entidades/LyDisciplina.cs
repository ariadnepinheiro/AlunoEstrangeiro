using Seeduc.Infra.Entities;
using System;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyDisciplina : IEntity
    {
        public string Disciplina { get; set; }

        public string Faculdade { get; set; }

        public string Depto { get; set; }

        public string Nome { get; set; }

        public string NomeCompl { get; set; }

        public Decimal Creditos { get; set; }

        public string TemNota { get; set; }

        public string TipoNota { get; set; }

        public string TemFreq { get; set; }

        public string PriorizaFreq { get; set; }

        public string VerificaHorario { get; set; }

        public string Ativa { get; set; }

        public string FormulaMf1 { get; set; }

        public string FormulaMf2 { get; set; }

        public string FormulaMf3 { get; set; }

        public string FormulaCa1 { get; set; }

        public string FormulaCa2 { get; set; }

        public string FormulaCa3 { get; set; }

        public string ConceitoMin1 { get; set; }

        public string ConceitoMin2 { get; set; }

        public string ConceitoMin3 { get; set; }

        public string ConceitoMinEx { get; set; }

        public string ConceitoMinEx2 { get; set; }

        public Decimal? PercPresmin { get; set; }

        public string Servico { get; set; }

        public string Estagio { get; set; }

        public int? HorasAula { get; set; }

        public int? HorasLab { get; set; }

        public int? HorasAtiv { get; set; }

        public int? HorasEstagio { get; set; }

        public Decimal PrazoRevisao { get; set; }

        public string FormulaPrereq { get; set; }

        public string FormulaEquiv { get; set; }

        public string Tipo { get; set; }

        public string GrupoNota { get; set; }

        public string GrupoMedia { get; set; }

        public Decimal? NCasasDec { get; set; }

        public string NotaMax { get; set; }

        public Decimal NCasasDecMedia { get; set; }

        public string NotaMaxMedia { get; set; }

        public int? AulasSemanais { get; set; }

        public int? AulasSemAula { get; set; }

        public int? AulasSemLab { get; set; }

        public int? AulasSemAtiv { get; set; }

        public string TruncaMedia { get; set; }

        public string FaltaDiaria { get; set; }

        public Decimal? PrazoDivulgacao { get; set; }

        public string Pim { get; set; }

        public string CopiaNotaSubturma { get; set; }

        public string NomeFantasia { get; set; }

        public string AvalCompetencia { get; set; }

        public DateTime StampAtualizacao { get; set; }

        public string TemAvalDescritiva { get; set; }

        public string Componente { get; set; }

        public string AreaConhecimento { get; set; }

        public string PermiteManterHorario { get; set; }

        public string Multipla { get; set; }

        [AtributoCampo(Nome = "CAMPO_01")]
        public string Campo01 { get; set; }

        public string CategoriaEnturmacao { get; set; }

        public string ObsFormulaMf1 { get; set; }

        public string ObsFormulaMf2 { get; set; }

        public string ObsFormulaMf3 { get; set; }

        public string Eletiva { get; set; }

        public string Grupo { get; set; }
    }
}
