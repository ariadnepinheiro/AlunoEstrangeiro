using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class DadosGeraisDocente
    {
        // DADOS PESSOAIS
        public virtual string Num_func { get; set; }
        public virtual string Matricula { get; set; }
        public virtual string IDFuncional { get; set; }
        public virtual string IDVinculo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NomeSocial { get; set; }
        public virtual string DataNasc { get; set; }
        public virtual string CorRaca { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string NecessidadeEspecial { get; set; }
        public virtual string EstadoCivil { get; set; }
        public virtual string PaisNasc { get; set; }
        public virtual string Nacionalidade { get; set; }
        public virtual string Naturalidade { get; set; }
        public virtual string UFNascimento { get; set; }

        // ENDEREÇO
        public virtual string Pais { get; set; }
        public virtual string Cep { get; set; }
        public virtual string EndMunicipio { get; set; }
        public virtual string UFEndereco { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string ZonaResidencial { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Email { get; set; }

        // DOCUMENTO / IDENTIDADE
        public virtual string RGTipo { get; set; }
        public virtual string RGNumero { get; set; }
        public virtual string RGUF { get; set; }
        public virtual string RGEmissor { get; set; }
        public virtual string RGDtExp { get; set; }
        
        // DOCUMENTOS / CPF
        public virtual string CPF { get; set; }

        // adicionado por Felipe R. Gomes em 16/04/2021 - ref. ESP UC.01.0 Cadastro de Acompanhamento Google Classroom
        public virtual string EmailInterno { get; set; }
        public virtual string EmailGoogle { get; set; }
    }
}