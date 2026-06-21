using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosDocente
    {
        public decimal DocenteId { get; set; }

        public string Matricula { get; set; }

        public decimal Pessoa { get; set; }

        public string NomeCompleto { get; set; }

        public string Cpf { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string EstadoCivil { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Municipio { get; set; }

        public string Estado { get; set; }

        public string Etnia { get; set; }

        public string Email { get; set; }

        public string Nacionalidade { get; set; }

        public string Naturalidade { get; set; }

        public string NomeMae { get; set; }

        public string NomePai { get; set; }

        public string Identidade { get; set; }

        public string OrgaoIdentidade { get; set; }

        public string UFIdentidade { get; set; }

        public string TipoIdentidade { get; set; }

        public DateTime DataAdmissao { get; set; }

        public DateTime? DataDemissao { get; set; }

        public DateTime DataExpedicao { get; set; }

        public string TituloEleitor { get; set; }

        public string ZonaTitulo { get; set; }

        public string SecaoTitulo { get; set; }

        public string Certificado { get; set; }

        public string CategoriaCertificado { get; set; }

        public string SerieCertificado { get; set; }

        public string Pis { get; set; }

        public string Ctps { get; set; }

        public string CtpsSerie { get; set; }

        public DateTime? CtpsData { get; set; }

        public string CtpsUF { get; set; }

        public string AnoConcurso { get; set; }

        public int Acumulacao { get; set; }

        public string MatriculaAcumulacao { get; set; }

        public string OrgaoAcumulacao { get; set; }

        public string ProcessoAcumulacao { get; set; }

        public string Categoria { get; set; }

        public string ZonaResidencial { get; set; }

        public int? Banco { get; set; }

        public string Agencia { get; set; }

        public string Conta { get; set; }

        public string Concurso { get; set; }

        public string Candidato { get; set; }

        public string Voluntario { get; set; }

        public string RegimeTrabalho { get; set; }

        public int RegimeContratacaoId { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string SenhaAlterada { get; set; }

        public string SenhaDol { get; set; }

        public int? IdFuncional { get; set; }

        public int? Vinculo { get; set; }
    }
}

