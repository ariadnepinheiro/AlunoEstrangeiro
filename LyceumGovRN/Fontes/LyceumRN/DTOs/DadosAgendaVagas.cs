namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAgendaVagas
    {
        public int ID { get; set; }

        public int Periodo { get; set; }

        public int IDAgenda { get; set; }

        public int AgendaID { get; set; }

        public string Modalidade { get; set; }

        public string Curso { get; set; }

        public string Serie { get; set; }

        public string DescricaoSerie { get; set; }

        public int QuantContSeeduc { get; set; }

        public int QuantNovaSeeduc { get; set; }

        public int QuantCont { get; set; }

        public int QuantNovas { get; set; }

        public int QuantContVagasUtilizadas { get; set; }

        public int QuantNovasVagasUtilizadas { get; set; }

        public string TaxaReprovacao { get; set; }

        public string TaxaAprovacao { get; set; }

        public int QuantContSaldo
        {
            get { return QuantCont - QuantContVagasUtilizadas; }
            set { ;}
        }

        public int QuantNovaSaldo
        {
            get { return QuantNovas - QuantNovasVagasUtilizadas; }
            set { ;}
        }

        public string JustificativaContinuidade { get; set; }

        public string JustificativaNova { get; set; }

        public bool Delecao { get; set; }

        public bool Editavel { get; set; }

        public bool Finalizado { get; set; }

        public bool Encerrado { get; set; }

        public string NomeCurso { get; set; }

        public bool SerieEntrada { get; set; }
    }
}
