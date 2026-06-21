using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Common
{
    public static class Constants
    {
        public const int gridPageSize = 15;
        public const int horasValidadeTokenSenha = 24;
        public const string emailTemplate = "~/emailTemplate.cshtml";
        public const int numCasasDecimaisDefault = 2;

        //Relatórios
        public const short row_data = 5;
        public const short column_data = 0;
        public const int max_row = 65536;
        public const string sheet_name = "Plan";

        //Tipo Unidade
        public const int TipoUnidAdmRegional = 1;
        public const int TipoUnidAdmEscolar = 2;
        public const int TipoUnidAdmRegionalPedagogica = 3;
        public const int TipoUnidAdmRegionalAdministrativa = 4;
        public const int TipoUnidAdmRegionalGestaoPessoas = 5;
        public const int TipoUnidAdmRegionalPedagogica_Administrativa = 6;
        public const int TipoUnidAdmUnidadeAdministrativaSemMeta = 7;

        //Modalidade
        public const int IdModalidadeER = 1;
        public const int IdModalidadeEJA = 2;

        //Nível de Ensino
        public const int IdNivelEnsinoEREF1 = 1;
        public const int IdNivelEnsinoEREF2 = 2;
        public const int IdNivelEnsinoEREM = 3;

        public const int IdNivelEnsinoEJAEF2 = 2;
        public const int IdNivelEnsinoEJAEM = 3;

        //Indicador
        public const int IdIndicadorIDERJ = 6;
        public const int IdIndicadorID = 51;
        public const int IdIndicadorIF = 52;
        public const int IdIndicadorIGE = 53;

        public const int IdAvalExternaSaerjinho1 = 1;
        public const int IdAvalExternaSaerjinho2 = 2;
        public const int IdAvalExternaSaerjinho3 = 3;
        public const int IdAvalExternaSaerj = 4;

        public const int IdCriterioCurriculoMin = 1;
    }
}