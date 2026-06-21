using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Techne.Lyceum.RN.CartaoEstudante.Enum
{
    public enum TipoSolicitacaoEnum
    {
        /// <summary>
        /// 
        /// Inclusão de cadastro.
        /// </summary>
        [Description("Inclusão de cadastro.")]
        Codigo1005 = 1,
        /// <summary>
        /// 
        /// Alteração de cadastro
        /// </summary>
        [Description("Alteração de cadastro")]        
        Codigo1006 = 2,
        /// <summary>
        /// 
        /// Alteração de login na Riocard
        /// </summary>
        [Description("Alteração de login na Riocard")]
        Codigo1007 = 3,
        /// <summary>
        /// 
        /// Cancelamento de matrícula por bloqueio
        /// </summary>
        [Description("Cancelamento de matrícula por bloqueio")]
        Codigo49 = 4,
        /// <summary>
        /// 
        /// Descancelar cartão
        /// </summary>
        [Description("Descancelar cartão")]
        Codigo99 = 5,
    }
}
