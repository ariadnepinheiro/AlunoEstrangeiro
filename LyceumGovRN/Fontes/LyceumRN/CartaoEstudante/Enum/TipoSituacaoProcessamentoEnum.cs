using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Techne.Lyceum.RN.CartaoEstudante.Enum
{
    public enum TipoSituacaoProcessamentoEnum
    {
        [Description("Todos (0,1,2,9,10,99)")]
        Todos = 0,
        [Description("Possui crítica (2) ")]
        ComCritica = 1,
        [Description("Não possui crítica (1,10)")]
        SemCritica = 2,
        [Description("Em Processamento (0,9,99)")]
        EmProcessamento = 3
    }
}
