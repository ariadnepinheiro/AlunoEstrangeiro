using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.PRECADASTROALUNO", Nome = "Matricula.PRECADASTROALUNO")]
    public class PreCadastroAluno : IEntity
    {
        [AtributoCampo(Nome = "PRECADASTROALUNOID")]
        public int PreCadastroAlunoId { get; set; }

        [AtributoCampo(Nome = "PESSOAID")]
        public decimal? PessoaId { get; set; } //PESSOA

        public string Nome { get; set; } //NOME_COMPL

        public string Email { get; set; } //E_MAIL

        [AtributoCampo(Nome = "DATANASCIMENTO")]
        public DateTime? DataNascimento { get; set; } //DT_NASC

        public string Responsavel { get; set; } //RESPONSAVEL

        [AtributoCampo(Nome = "NOMEMAE")]
        public string NomeMae { get; set; } //NOME_MAE

        [AtributoCampo(Nome = "MAECPF")]
        public string MaeCpf { get; set; } //MAE_CPF

        [AtributoCampo(Nome = "NOMEPAI")]
        public string NomePai { get; set; } //NOME_PAI

        [AtributoCampo(Nome = "PAICPF")]
        public string PaiCpf { get; set; } //PAI_CPF

        [AtributoCampo(Nome = "RESPONSAVELNOME")]
        public string ResponsavelNome { get; set; } //RESP_NOME_COMPL

        [AtributoCampo(Nome = "RESPONSAVELFONE")]
        public string ResponsavelFone { get; set; } //RESP_FONE

        [AtributoCampo(Nome = "RESPONSAVELCPF")]
        public string ResponsavelCpf { get; set; } //RESP_CPF

        [AtributoCampo(Nome = "RESPONSAVELNUMERORG")]
        public string ResponsavelNumeroRG { get; set; } //Não tem equivalente na pessoa

        [AtributoCampo(Nome = "RESPOSANVELEMISSORRG")]
        public string ResposanvelEmissorRG { get; set; } //Não tem equivalente na pessoa

        [AtributoCampo(Nome = "RESPOSANVELUFRG")]
        public string ResposanvelUFRG { get; set; } //TNão tem equivalente na pessoa

        public string Cpf { get; set; } //CPF

        public string Sexo { get; set; } //SEXO

        [AtributoCampo(Nome = "ESTADOCIVIL")]
        public string EstadoCivil { get; set; } //EST_CIVIL

        public string Nacionalidade { get; set; } //NACIONALIDADE

        [AtributoCampo(Nome = "MUNICIPIONASCIMENTO")]
        public string MunicipioNascimento { get; set; } //MUNICIPIO_NASC

        public string Celular { get; set; } //CELULAR

        [AtributoCampo(Nome = "FIXOCELULAR")]
        public string FixoCelular { get; set; } //FONE

        public string Cep { get; set; } //CEP

        public string Endereco { get; set; } //ENDERECO

        [AtributoCampo(Nome = "NUMEROENDERECO")]
        public string NumeroEndereco { get; set; } //END_NUM

        [AtributoCampo(Nome = "COMPLEMENTOENDERECO")]
        public string ComplementoEndereco { get; set; } //END_COMPL

        public string Bairro { get; set; } //BAIRRO

        [AtributoCampo(Nome = "MUNICIPIOENDERECO")]
        public string MunicipioEndereco { get; set; } //END_MUNICIPIO

        [AtributoCampo(Nome = "MATRICULACERTIDAO")]
        public string MatriculaCertidao { get; set; } //CERT_NUMERO_MATRICULA

        [AtributoCampo(Nome = "TERMOCERTIDAO")]
        public string TermoCertidao { get; set; } //CERT_NASC_NUM

        [AtributoCampo(Nome = "FOLHACERTIDAO")]
        public string FolhaCertidao { get; set; } //CERT_NASC_FOLHA

        [AtributoCampo(Nome = "LIVROCERTIDAO")]
        public string LivroCertidao { get; set; } //CERT_NASC_LIVRO

        [AtributoCampo(Nome = "NECESSIDADEESPECIALID")]
        public int? NecessidadeEspecialId { get; set; } //NECESSIDADEESPECIALID

        [AtributoCampo(Nome = "MODELOCERTIDAO")]
        public string ModeloCertidao { get; set; } //FLFIELD09 - LY_FL_PESSOA

        [AtributoCampo(Nome = "TIPOCERTIDAO")]
        public string TipoCertidao { get; set; } //FLFIELD02 - LY_FL_PESSOA

        [AtributoCampo(Nome = "NUMERORG")]
        public string NumeroRG { get; set; } //RG_NUM e RG_TIPO = "RG"

        [AtributoCampo(Nome = "ORGAORG")]
        public string OrgaoRG { get; set; } //RG_EMISSOR

        [AtributoCampo(Nome = "UFRG")]
        public string UFRG { get; set; } //RG_UF     

        [AtributoCampo(Nome = "REDEORIGEM")]
        public string RedeOrigem { get; set; } //LY_ALUNO - REDE_ENSINO_ORIGEM 

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
