using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConfirmacaoCandidato
    {
        public DadosConfirmacaoCandidato()
        {
            PessoaAluno = new Techne.Lyceum.RN.Matriculas.Entidades.PessoaAluno();
            DadosIrmao = new DadosIrmao();
        }       

        public int Fase { get; set; }

        public int OpcaoInscricaoId { get; set; }

        public int InscricaoAlunoId { get; set; }

        public int NumeroInscricao { get; set; }

        public DadosIrmao DadosIrmao { get; set; } 

        public string IrmaoMatricula { get; set; }

        public int? IrmaoIdInscricao { get; set; }

        public int? IrmaoNumeroInscricao { get; set; } 

        public string Aluno { get; set; }

        public int PreCadastroAlunoId { get; set; }

        public int ControleVagaId { get; set; }

        public decimal? Pessoa { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public DateTime DataNascimento { get; set; }

        public string NomeMae { get; set; }

        public string NomePai { get; set; }

        public bool DeclaroAusenciaMae { get; set; }

        public bool DeclaroAusenciaPai { get; set; }

        public bool? DescFamilia { get; set; }

        public string Telefone { get; set; }

        public string Censo { get; set; }

        public string Escola { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Modalidade { get; set; }

        public string ModalidadeCodigo { get; set; }

        public string Segmento { get; set; }

        public string SegmentoCodigo { get; set; }

        public string Curso { get; set; }

        public string CursoDescricao { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public string UsuarioResponsavel { get; set; }

        public bool? Confirma { get; set; }

        public int? MotivoRejeicaoInscricaoId { get; set; }

        public string Email { get; set; }

        public string Municipio { get; set; }

        public string Curriculo { get; set; }

        public bool EnsinoReligioso { get; set; }

        public bool LinguaEstrangeiraFacultativa { get; set; }

        public string Turma { get; set; }

        public string SitAluno { get; set; }       

        public RN.Matriculas.Entidades.PessoaAluno PessoaAluno { get; set; }

        public byte[] FotoArquivo { get; set; }

        public string FotoTipoArquivo { get; set; }

        public string FotoNomeArquivo { get; set; }

        public string FotoSrc { get; set; }
    }
}
