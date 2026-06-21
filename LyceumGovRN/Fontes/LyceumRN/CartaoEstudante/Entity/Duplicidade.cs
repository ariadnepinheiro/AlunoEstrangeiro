using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class Duplicidade: IEntity
    {
        public long NumeroRegistro { get; set; }

        [AtributoCampo(Nome = "DUPLICIDADEID")]
        public int DuplicidadeId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }

        [AtributoCampo(Nome = "ALUNO")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "IDBENEFICIARIO")]
        public int IdBeneficiario { get; set; }   

        [AtributoCampo(Nome = "FLAGMATRICULAPRINCIPAL")]
        public string FlagMatriculaPrincipal { get; set; }

        [AtributoCampo(Nome = "DATAFLAGMATRICULAPRINCIPAL")]
        public DateTime DataFlagMatriculaPrincipal { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [AtributoCampo(Nome = "DATAATUALIZACAO")]
        public DateTime DataAtualizacao { get; set; }

        public Duplicidade() { }
        public Duplicidade(int duplicidadeId, int operadoraId, int idBeneficiario, string aluno, string nome, string flagMatriculaPrincipal, DateTime dataFlagMatriculaPrincipal, DateTime dataInclusao, DateTime dataAtualizacao)
        {
            DuplicidadeId = duplicidadeId;
            OperadoraId = operadoraId;
            IdBeneficiario = idBeneficiario;
            Aluno = aluno;
            FlagMatriculaPrincipal = flagMatriculaPrincipal;
            DataFlagMatriculaPrincipal = dataFlagMatriculaPrincipal;
            DataInclusao = dataInclusao;
            DataAtualizacao = dataAtualizacao;            
        }
    }
}
