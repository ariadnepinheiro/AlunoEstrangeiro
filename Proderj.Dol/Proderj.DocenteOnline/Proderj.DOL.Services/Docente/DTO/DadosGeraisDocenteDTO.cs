using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    // Mantive "DTO" como sufixo e não prefixo como os demais, para seguir
    // o padrão adotado de <NomeDaClasse><AQueSeRefere>.
    
    // Descrito no documento "Padroes e Boas Praticas de Desenvolviment2.docx" 
    // (em http://10.12.18.240/ConexaoEducacao/Documentacao/Modelos%20e%20Manuais/Manuais/Padroes%20e%20Boas%20Praticas%20de%20Desenvolviment2.docx):
    // ----------------------------------------------------------------------------------------------------------------------------------------------
    // "Quando em seus nomes for feita menção à padrões de projeto, 
    // camadas ou nomenclaturas do .NET adotadas pela comunidade de TI 
    // (Exception, Colletion, Attribute, Controller) estas partes poderão ser mantidas em inglês e sufixadas ao nome da classe."
    // TODO: Renomear as demais classes DTO's a fim de seguinr o padrão definido.
    public class DadosGeraisDocenteDTO
    {
        // DADOS PESSOAIS
        public string Matricula { get; set; }
        public string IDFuncional { get; set; }
        public string Nome { get; set; }
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
        public virtual string EmailInterno { get; set; }
        public virtual string EmailGoogle { get; set; }
    }
}
