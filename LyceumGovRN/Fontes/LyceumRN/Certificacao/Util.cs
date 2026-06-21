using System;
using System.Globalization;

namespace Techne.Lyceum.RN.Certificacao
{
    public class Util
    {

        public string DataporExtenso(DateTime data)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            string dia = (data.Day.ToString().Length == 1 ? "0" + data.Day.ToString() : data.Day.ToString());
            int ano = data.Year;
            string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(data.Month));
            string result = dia + " de " + mes + " de " + ano;

            return result;
        }
    }

    public class TipoDocumento
    {
        public const int HISTORICOESCOLAR = 1;
        public const int CERTIDAO = 2;
        public const int CERTIFICADO_ESCOLAR = 3;
        public const int DIPLOMA = 4;
    }

    public class TipoConclusao_
    {
        public const int FUNDAMENTAL = 1;
        public const int MEDIO = 2;
        public const int PROFISIONALIZANTE = 3;

        public static string RetornatipoConclusao(int cod)
        {
            string conclusao = string.Empty;
            switch (cod)
            {
                case 1:
                    conclusao = "Fundamental";
                    break;
                case 2:
                    conclusao = "Medio";
                    break;
                case 3:
                    conclusao = "Profisionalizante";
                    break;

                default:
                    conclusao = "INESISTENTE";
                    break;
            }
            return conclusao;
        }
    }

    public class infAluno
    {
        public string nome;
        public string matricula;
        public string tpConclusao;
        public string tpDocumento;
        public int documentoCertID;
    }
}
