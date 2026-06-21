using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public enum StatusExecucao
    {
        [DescriptionAttribute("EM EXECUÇÃO")]
        EmExecucao = 0,

        [DescriptionAttribute("CONCLUÍDO")]
        Concluido = 1,

        [DescriptionAttribute("Erro")]
        Erro = 2,
    }

    public class ExecucaoCalculo
    {
        public int IdExecucaoCalculo { get; set; }

        [Display(Name = "Data da execução")]
        public DateTime DtExecucao { get; set; }
        
        public StatusExecucao StatusExecucao { get; set; }

        [Display(Name = "Usuário")]
        public Usuario Usuario { get; set; }

        [Display(Name = "Mensagem de Erro")]
        public string MENSAGEM { get; set; }
    }
}