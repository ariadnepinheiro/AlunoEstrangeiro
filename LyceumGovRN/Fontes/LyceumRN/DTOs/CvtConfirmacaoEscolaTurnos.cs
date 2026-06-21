namespace Techne.Lyceum.RN.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CvtConfirmacaoEscolaTurnos
    {
        public bool Ampliado { get; set; }

        public string AmpliadoCodigo { get; set; }

        public string Curso { get; set; }

        public int IdCvtConfirmacaoEscola { get; set; }

        public bool Integral { get; set; }

        public string IntegralCodigo { get; set; }

        public string Justificativa { get; set; }

        public bool Manha { get; set; }

        public string ManhaCodigo { get; set; }

        public string Matricula { get; set; }

        public string Modalidade { get; set; }

        public bool Noite { get; set; }

        public string NoiteCodigo { get; set; }

        public string NomeCurso { get; set; }

        public string Serie { get; set; }

        public bool Tarde { get; set; }

        public string TardeCodigo { get; set; }

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

        public string TurnosIniciais { get; set; }

        public string UnidadeEns { get; set; }

        internal void CarregarTurnos(ICollection<string> codigoTurnos)
        {
            if (codigoTurnos == null)
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
        }
    }
}