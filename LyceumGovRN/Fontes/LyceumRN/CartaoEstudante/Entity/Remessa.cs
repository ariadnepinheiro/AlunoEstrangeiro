using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class Remessa: IEntity
    {
        [AtributoCampo(Nome = "REMESSAID")]
        public int RemessaId { get; set; }
        [AtributoCampo(Nome = "SOLICITACAOID")]
        public int SolicitacaoId { get; set; }
        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }
        [AtributoCampo(Nome = "LOTEREMESSAID")]
        public int LoteRemessaId { get; set; }
        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }
        public string Aluno { get; set; }
        public string NomeCompl { get; set; }
        public DateTime DtNasc { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string Cpf { get; set; }
        public string RgNum { get; set; }
        [AtributoCampo(Nome = "RG_UF")]
        public string RgUF { get; set; }
        public string RgEmissor { get; set; }
        [AtributoCampo(Nome = "RG_DTEXP")]
        public DateTime RgDtExp { get; set; }
        public string Cep { get; set; }
        public int EndTpLogradouro { get; set; }
        public string Endereco { get; set; }
        public string EndNum { get; set; }
        public string EndCompl { get; set; }
        public string Bairro { get; set; }
        public string EndMunicipio { get; set; }
        public string UnidadeEns { get; set; }
        public byte[] Foto { get; set; }
        public string Gratuidade { get; set; }
        [AtributoCampo(Nome = "MODALTREM")]
        public string ModalTrem { get; set; }
        [AtributoCampo(Nome = "MODALONIBUS")]
        public string ModalOnibus { get; set; }
        [AtributoCampo(Nome = "MODALMETRO")]
        public string ModalMetro { get; set; }
        [AtributoCampo(Nome = "MODALBARCAS")]
        public string ModalBarcas { get; set; }
        public DateTime? StampAtualizacao { get; set; }
        [AtributoCampo(Nome = "QHITURNO")]
        public string Turno { get; set; }
        [AtributoCampo(Nome = "QHITURMA")]
        public string Turma { get; set; }
        public int? Serie { get; set; }
        [AtributoCampo(Nome = "E_MAIL_INTERNO")]
        public string EmailInterno { get; set; }
        [AtributoCampo(Nome = "LOGINRIOCARD")]
        public string LoginRioCard { get; set; }
        public string AssinaturaDigital { get; set; }
    }
}
