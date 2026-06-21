using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosEncaminhamentoEspecial
    {
        public DadosEncaminhamentoEspecial()
        {
            PessoaAluno = new Techne.Lyceum.RN.Matriculas.Entidades.PessoaAluno();
        }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string NomeMae { get; set; }

        public bool MaeNãoDeclarada { get; set; }

        public string Sexo { get; set; }

        public string NomePai { get; set; }

        public bool PaiNãoDeclarado { get; set; }

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string NumeroEndereco { get; set; }

        public string ComplementoEndereco { get; set; }

        public string MunicipioEndereco { get; set; }

        public string DescricaoMunicipioEndereco { get; set; }

        public string UfEndereco { get; set; }

        public string Bairro { get; set; }

        public string Cpf { get; set; }

        public int? NecessidadeEspecialId { get; set; }

        public string UsuarioResponsavel { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Censo { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Curriculo { get; set; }

        public int Serie { get; set; }

        public int? PreCadastroAlunoId { get; set; }

        public int ControleVagaId { get; set; }

        public decimal? Pessoa { get; set; }

        public string Turma { get; set; }

        public int MotivoEncaminhamentoEspecial { get; set; }

        public string Observacao { get; set; }

        public RN.Matriculas.Entidades.PessoaAluno PessoaAluno { get; set; }
    }
}
