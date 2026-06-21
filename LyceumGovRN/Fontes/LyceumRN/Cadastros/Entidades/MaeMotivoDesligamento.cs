using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Cadastros.Entidades
{
    [AtributoTabela("Cadastros.MAE_MOTIVODESLIGAMENTO", Nome = "Cadastros.MAE_MOTIVODESLIGAMENTO")]
    public class MaeMotivoDesligamento : IEntity
    {
        [AtributoCampo(Nome = "MAE_MOTIVODESLIGAMENTOID")]
        public int MaeMotivoDesligamentoId { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
