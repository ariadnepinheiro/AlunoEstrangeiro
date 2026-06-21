using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosFormulario1
    {
        //Filtros

        public string Censo { get; set; }

        public int PeriodoReferenciaId { get; set; }

        //Campos Formulario

        public string NomeAae{ get; set; }

        public string Cnpj{ get; set; }

        public string Endereco{ get; set; }

        public string Complemento{ get; set; }

        public string Bairro{ get; set; }

        public string Municipio{ get; set; }

        public string Cep{ get; set; }

        public string Ddd{ get; set; }

        public string Telefone{ get; set; }

        public string EmailInstituicional{ get; set; }

        public string DiretoriaRegional{ get; set; }

        public string FundamentalModalidade { get; set; }//X ou vazio

        public string FundamentalNumeroAlunos{ get; set; }

        public string FundamentalNumeroTurnos{ get; set; }

        public string FundamentalHorarioIntegralSim { get; set; }//X ou vazio

        public string FundamentalHorarioIntegralNao { get; set; }//X ou vazio

        public string MedioModalidade { get; set; }//X ou vazio

        public string MedioNumeroAlunos { get; set; }

        public string MedioNumeroTurnos { get; set; }

        public string MedioHorarioIntegralSim { get; set; }//X ou vazio

        public string MedioHorarioIntegralNao { get; set; }//X ou vazio

        public string EjaModalidade{ get; set; }//X ou vazio

        public string EjaNumeroAlunos{ get; set; }

        public string EjaNumeroTurnos{ get; set; }

        public string EjaHorarioIntegralSim { get; set; }//X ou vazio

        public string EjaHorarioIntegralNao { get; set; }//X ou vazio

        public string EducacaoEspecialModalidade{ get; set; }//X ou vazio

        public string EducacaoEspecialNumeroAlunos { get; set; }

        public string EducacaoEspecialNumeroTurnos { get; set; }

        public string EducacaoEspecialHorarioIntegralSim { get; set; }//X ou vazio

        public string EducacaoEspecialHorarioIntegralNao { get; set; }//X ou vazio

        public string DiretorInicioNome { get; set; }

        public string DiretorInicioCpf { get; set; }

        public string DiretorInicioMatricula { get; set; }

        public string DiretorInicioIdFuncional { get; set; }

        public string DiretorInicioDataDO { get; set; }

        public string DiretorFimNome { get; set; }

        public string DiretorFimCpf { get; set; }

        public string DiretorFimMatricula { get; set; }

        public string DiretorFimIdFuncional { get; set; }

        public string DiretorFimDataDO { get; set; }

        public string TesoureiroInicioNome { get; set; }

        public string TesoureiroInicioCpf { get; set; }

        public string TesoureiroInicioMatricula { get; set; }

        public string TesoureiroInicioIdFuncional { get; set; }

        public string TesoureiroFimNome { get; set; }

        public string TesoureiroFimCpf { get; set; }

        public string TesoureiroFimMatricula { get; set; }

        public string TesoureiroFimIdFuncional { get; set; }

        public string Banco{ get; set; }

        public string Agencia{ get; set; }

        public string ContaCorrente { get; set; }
    }
}
