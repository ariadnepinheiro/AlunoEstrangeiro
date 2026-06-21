using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Ocorrencias.Entidades
{
    [AtributoTabela("Ocorrencias.ARQUIVOOCORRENCIA", Nome = "Ocorrencias.ARQUIVOOCORRENCIA")]
    public class Ocorrencia: IEntity
    {
        [AtributoCampo(Nome = "OCORRENCIAID")]
        public int OcorrenciaId { get; set; }

        [AtributoCampo(Nome = "NUMEROOCORRENCIA")]
        public string NumeroOcorrencia { get; set; }

        public string Censo { get; set; }

        [AtributoCampo(Nome = "DATAOCORRENCIA")]
        public DateTime DataOcorrencia { get; set; }

        [AtributoCampo(Nome = "CLASSEID")]
        public int ClasseId { get; set; }

        [AtributoCampo(Nome = "SUBCLASSEID")]
        public int? SubClasseId { get; set; }

        public string Relato { get; set; }

        [AtributoCampo(Nome = "MEIOID")]
        public int MeioId { get; set; }

        [AtributoCampo(Nome = "DELEGACIAID")]
        public int? DelegaciaId { get; set; }

        [AtributoCampo(Nome = "BATALHAOID")]
        public int? BatalhaoId { get; set; }

        [AtributoCampo(Nome = "REGISTROOCORRENCIA")]
        public string RegistroOcorrencia { get; set; }

        [AtributoCampo(Nome = "USOARMA")]
        public bool? UsoArma { get; set; }

        public string Observacao { get; set; }

        public bool Arquivada { get; set; }

        public bool Interrupcao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "MOTIVOCANCELAMENTOID")]
        public int? MotivoCancelamentoId { get; set; }

        [AtributoCampo(Nome = "USUARIOCADASTRO")]
        public string UsuarioCadastro { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
