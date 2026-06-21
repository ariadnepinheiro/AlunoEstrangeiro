using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.RecursosHumanos.DTO
{
    public class DadosInscricaoMigracao
    {
        //dados de identificação
        public string IdVinculo { get; set; }
        public int NumFunc { get; set; }
        public int Pessoa { get; set; }
        public string Concurso { get; set; }
        public int? DocenteCandidatoId { get; set; }
        public string NumeroInscricao { get; set; }
        public string DescricaoSituacao { get; set; }
        public string Situacao { get; set; }

        //dados Lotacao
        public int? RegionalId { get; set; }
        public string RegionalDescricao { get; set; }
        public string MunicipioCodigo { get; set; }
        public string MunicipioDescricao { get; set; }
        public string Sede { get; set; }
        public string DisciplinaIngresso { get; set; }
        public string DisciplinaIngressoDescricao { get; set; }

        //dados do professor
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string EstadoCivil { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public string NaturalidadeId { get; set; }
        public string NaturalidadeDescricao { get; set; }
        public string NaturalidadeUf { get; set; }
        public int? NumeroDependentes { get; set; }

        //documentos pessoais
        public string Rg { get; set; }
        public string RgOrgao { get; set; }
        public DateTime? RgDataExpedicao { get; set; }
        public string RgUf { get; set; }
        public string Cpf { get; set; }
        public string PisPasep { get; set; }
        public string TituloEleitor { get; set; }
        public string TituloEleitorZona { get; set; }
        public string TituloEleitorSecao { get; set; }
        public string TituloEleitorUf { get; set; }
        public string Cnh { get; set; }
        public string CnhCategoria { get; set; }
        public DateTime? CnhValidade { get; set; }
        public string CnhUf { get; set; }
        public string CarteiraTrabalho { get; set; }
        public string CarteiraTrabalhoSerie { get; set; }
        public string CarteiraTrabalhoUf { get; set; }
        public string CertificadoReservista { get; set; }
        public string CertificadoReservistaSerie { get; set; }
        public string CertificadoReservistaUf { get; set; }

        //endereço
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public string EnderecoMunicipio { get; set; }
        public string EndMunicipioDescricao { get; set; }
        public string EnderecoUf { get; set; }

        //contato
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string EmailPessoal { get; set; }
        public string EmailInstitucional { get; set; }

        //outros
        public bool? Acumulacao { get; set; }
        public bool? FuncaoDiretor { get; set; }
        public int? QuantidadeAnosGlp { get; set; }
        public string Experiencia { get; set; }
        public string Titulacao { get; set; }
        public string TitulacaoDescricao { get; set; }
        public bool? UtilizaRubrica { get; set; }

        public DateTime DataMigracao { get; set; }

        public DateTime DataConvocacao { get; set; }

        public List<int> AnosGLP { get; set; }

        public bool ParticipouMigracaoAnterior { get; set; }


        public string UsuarioId { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

        //Anexos
        public ICollection<Entidades.DocenteCandidatoArquivo> Documentos { get; set; }
    }
}
