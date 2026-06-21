using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public enum OperacaoAuditoria
    {
        [DescriptionAttribute("INCLUSÃO")]
        [Display(Name = "Inclusão")]
        Inclusao = 1,

        [DescriptionAttribute("ATUALIZAÇÃO")]
        [Display(Name = "Atualização")]
        Atualizacao = 2,

        [DescriptionAttribute("EXCLUSÃO")]
        [Display(Name = "Exclusão")]
        Exclusao = 3,

        [DescriptionAttribute("INÍCIO CÁLCULO DE RV")]
        [Display(Name = "Início Cálculo de RV")]
        InicioCalculo = 4,

        [DescriptionAttribute("FIM CÁLCULO DE RV")]
        [Display(Name = "Fim Cálculo de RV")]
        FimCalculo = 5,

        [DescriptionAttribute("LOGIN")]
        [Display(Name = "Login")]
        Login = 6,

        [DescriptionAttribute("ALTERAÇÃO SENHA")]
        [Display(Name = "Alteração Senha")]
        AlteracaoSenha = 7
    }

    public class LogAuditoria
    {
        public int IdLogAuditoria { get; set; }

        public Usuario Usuario { get; set; }
        public DateTime DataLog { get; set; }
        public OperacaoAuditoria TipoOperacao { get; set; }
        public string DesObjeto { get; set; }
        public string IpCliente { get; set; }
    }
}