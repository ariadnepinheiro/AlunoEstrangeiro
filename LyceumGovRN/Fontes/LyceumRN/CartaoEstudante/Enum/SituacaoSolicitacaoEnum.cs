using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Techne.Lyceum.RN.CartaoEstudante.Enum
{
    [Serializable]
    public enum SituacaoSolicitacaoEnum
    {
        /// <summary>
        /// 
        /// Inclusão de cadastro.
        /// </summary>
        [Description("Criada")]
        Criada = 0,
        /// <summary>
        /// 
        /// Alteração de cadastro
        /// </summary>
        [Description("Gerada")]        
        Gerada = 1,
        /// <summary>
        /// 
        /// Alteração de login na Riocard
        /// </summary>
        [Description("Enviada")]
        Enviada = 2,
        /// <summary>
        /// 
        /// Cancelamento de matrícula por bloqueio
        /// </summary>
        [Description("Reenviada")]
        Reenviada = 3
    }
}
