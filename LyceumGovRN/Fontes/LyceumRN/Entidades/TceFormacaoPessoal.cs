namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceFormacaoPessoal : IEntity
    {
        public int IdFormacaoPessoal { get; set; }

        public int Pessoa { get; set; }

        public string Escolaridade { get; set; }

        public string SituacaoCurso { get; set; }

        public int IdCursoFormacaoPessoal { get; set; }

        public string FormacaoComplementacaoPedagogica { get; set; }

        public int AnoInicio { get; set; }

        public int AnoConclusao { get; set; }

        public string IdInstituicao { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public string Doc_comprobatorio { get; set; }
    }

    public class TceFormacaoEstudoAdicional : IEntity
    {
        public int FormacaoPessoalID { get; set; }

        public int EstudoAdicionalID { get; set; }
                
    }
}
