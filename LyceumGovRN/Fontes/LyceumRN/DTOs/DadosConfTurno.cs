using System;
using System.Collections.Generic;
using System.Linq;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConfTurno
    {
        public int IdAgendaConfTurnoVaga { get; set; }

        public int AgendaId { get; set; }

        public DateTime DtInicioConfTurno { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public string Modalidade { get; set; }

        public string CodigoModalidade { get; set; }

        public string DescricaoTipo { get; set; }

        public string CodigoTipo { get; set; }

        public string Curso { get; set; }

        public string NomeCurso { get; set; }

        public int Serie { get; set; }

        public string DescricaoSerie { get; set; }

        public bool Manha { get; set; }

        public string ManhaCodigo { get; set; }

        public bool Tarde { get; set; }

        public string TardeCodigo { get; set; }

        public bool Noite { get; set; }

        public string NoiteCodigo { get; set; }

        public bool Ampliado { get; set; }

        public string AmpliadoCodigo { get; set; }

        public bool Integral { get; set; }

        public string IntegralCodigo { get; set; }

        public string Justificativa { get; set; }

        public bool ManhaNovo { get; set; }

        public string ManhaNovoCodigo { get; set; }

        public bool TardeNovo { get; set; }

        public string TardeNovoCodigo { get; set; }

        public bool NoiteNovo { get; set; }

        public string NoiteNovoCodigo { get; set; }

        public bool AmpliadoNovo { get; set; }

        public string AmpliadoNovoCodigo { get; set; }

        public bool IntegralNovo { get; set; }

        public string IntegralNovoCodigo { get; set; }

        public string JustificativaNovo { get; set; }

        public string Matricula { get; set; }

        public bool Finalizado { get; set; }

        public bool Encerrado { get; set; }

        public string PerfilResponsavel { get; set; }

        public string TurnosListaInicial { get; set; }

        public string TurnosIniciais { get; set; }
        
        public string TurnosIniciaisNovos { get; set; }

        public string Turnos
        {
            get
            {
                return string.Format(
                                     "{0}{1}{2}{3}{4}",
                                     this.Manha ? "1" : "0",
                                     this.Tarde ? "1" : "0",
                                     this.Noite ? "1" : "0",
                                     this.Integral ? "1" : "0",
                                     this.Ampliado ? "1" : "0");
            }
        }

        public string TurnosNovo
        {
            get
            {
                return string.Format(
                                     "{0}{1}{2}{3}{4}",
                                     this.ManhaNovo ? "1" : "0",
                                     this.TardeNovo ? "1" : "0",
                                     this.NoiteNovo ? "1" : "0",
                                     this.IntegralNovo ? "1" : "0",
                                     this.AmpliadoNovo ? "1" : "0");
            }
        }

        internal void CarregarTurnos(ICollection<string> codigoTurnos, ICollection<string> codigoTurnosNovo)
        {
            if (codigoTurnos == null || codigoTurnosNovo == null)
            {
                return;
            }

            foreach (var codigoTurno in codigoTurnos)
            {
                var dados = codigoTurno.Split(new[]
                                                  {
                                                      ":"
                                                  }, StringSplitOptions.RemoveEmptyEntries);
                var chave = string.Empty;

                if (dados.Length != 2)
                {
                    continue;
                }

                switch (dados[0])
                {
                    case "M":
                        chave = "Manha";
                        break;
                    case "N":
                        chave = "Noite";
                        break;
                    case "T":
                        chave = "Tarde";
                        break;
                    case "I":
                        chave = "Integral";
                        break;
                    case "A":
                        chave = "Ampliado";
                        break;
                }

                if (string.IsNullOrEmpty(chave))
                {
                    continue;
                }

                var properties = this.GetType()
                                     .GetProperties()
                                     .ToDictionary(x => x.Name, x => x);

                // Configurando o Turno
                var property = properties[chave];

                property.SetValue(this, true, null);

                // Configurando o código do Turno
                property = properties[string.Format("{0}Codigo", chave)];

                property.SetValue(this, dados[1], null);
            }

            foreach (var codigoTurno in codigoTurnosNovo)
            {
                var dados = codigoTurno.Split(new[]
                                                  {
                                                      ":"
                                                  }, StringSplitOptions.RemoveEmptyEntries);
                var chave = string.Empty;

                if (dados.Length != 2)
                {
                    continue;
                }

                switch (dados[0])
                {
                    case "M":
                        chave = "ManhaNovo";
                        break;
                    case "N":
                        chave = "NoiteNovo";
                        break;
                    case "T":
                        chave = "TardeNovo";
                        break;
                    case "I":
                        chave = "IntegralNovo";
                        break;
                    case "A":
                        chave = "AmpliadoNovo";
                        break;
                }

                if (string.IsNullOrEmpty(chave))
                {
                    continue;
                }

                var properties = this.GetType()
                                     .GetProperties()
                                     .ToDictionary(x => x.Name, x => x);

                // Configurando o Turno
                var property = properties[chave];

                property.SetValue(this, true, null);

                // Configurando o código do Turno
                property = properties[string.Format("{0}Codigo", chave)];

                property.SetValue(this, dados[1], null);
            }
        }
    }
}
