using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proderj.DOL.WebApp.Models
{
    public class DadosGeraisDocenteViewModel : ViewModelPadrao
    {
        public DadosGeraisDocenteViewModel(DocenteLogadoBindModel docenteLogadoModelo) : base(docenteLogadoModelo) { }

        // DADOS PESSOAIS
        public string Matricula { get { return CabecalhoModelo.DocenteLogadoModelo.Matricula; } }
        public string IDFuncional { get; set; }
        public string Nome { get { return CabecalhoModelo.DocenteLogadoModelo.Nome; } }
        public string NomeSocial { get; set; }
        public string DataNasc { get; set; }
        public string CorRaca { get; set; }
        public string Sexo { get; set; }
        public string NecessidadeEspecial { get; set; }
        public string EstadoCivil { get; set; }
        public string PaisNasc { get; set; }
        public string Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
        public string UFNascimento { get; set; }

        // ENDEREÇO
        public string Pais { get; set; }
        public string Cep { get; set; }
        public string EndMunicipio { get; set; }
        public string UFEndereco { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string ZonaResidencial { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }

        // DOCUMENTO / IDENTIDADE
        public string RGTipo { get; set; }
        public string RGNumero { get; set; }
        public string RGUF { get; set; }
        public string RGEmissor { get; set; }
        public string RGDtExp { get; set; }

        // DOCUMENTOS / CPF
        public string CPF { get; set; }

        // adicionado por Felipe R. Gomes em 16/04/2021 - ref. ESP UC.01.0 Cadastro de Acompanhamento Google Classroom
        public string EmailInterno { get; set; }
        public string EmailGoogle { get; set; }
    }
}