using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Techne.Lyceum.RN.CartaoEstudante.Enum
{
    public enum SituacaoProcessamentoEnum
    {
        [Description("Aguardando entrada em processamento")]
        Aguardando = 0,
        [Description("Processado OK")]
        Processado = 1,
        [Description("Rejeitado")]
        Rejeitado = 2,
        [Description("Em Processamento (Gravado OK)")]
        EmProcessando = 9,
        [Description("Processado com nome ignorado")]
        ProcessadoNomeIgnorado = 10,
        [Description("ID da solicitação não localizado na operadora")]
        NaoLocalizado = 99
    }
}
