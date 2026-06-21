using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Domain;

namespace Techne.Lyceum.RN.CartaoEstudante.DTO
{
    [Serializable]
    public class RemessaDTO
    {
        public int RemessaId { get; set; }
        public int SolicitacaoId { get; set; }
        public int OperadoraId { get; set; }
        public int LoteRemessaId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string MatriculaAluno { get; set; }
        public string NomeAluno { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string Cpf { get; set; }
        public string NumeroRG { get; set; }
        public string UFRg { get; set; }
        public string OrgaoEmissorRG { get; set; }
        public DateTime DataExpedicaoRG { get; set; }
        public string Cep { get; set; }
        public int TipoLogradouroEndereco { get; set; }
        public string Endereco { get; set; }
        public string NumeroEndereco { get; set; }
        public string ComplementoEndereco { get; set; }
        public string Bairro { get; set; }
        public string EndMunicipio { get; set; }
        public string UnidadeEnsino { get; set; }
        public byte[] Foto { get; set; }
        public string Gratuidade { get; set; }
        public string ModalTrem { get; set; }
        public string ModalOnibus { get; set; }
        public string ModalMetro { get; set; }
        public string ModalBarcas { get; set; }
        public DateTime? DataUltimaAtualizacao { get; set; }
        public string Turno { get; set; }
        public string Turma { get; set; }
        public int? Serie { get; set; }
        public string EmailInterno { get; set; }
        public string LoginRioCard { get; set; }

        public DateTime? DataEnvioLoteRemessa { get; set; }

        public string NomeLoteRemessa{ get; set; }
        public OperadoraEnum Operadora { get { return (OperadoraEnum)this.OperadoraId; } }
    }
}
