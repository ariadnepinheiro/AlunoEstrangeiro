using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    public class Veiculo: IEntity
    {
        [AtributoCampo(Nome = "VEICULOID")]
        public int VeiculoId { get; set; }
        
        [AtributoCampo(Nome = "TIPOVEICULOID")]
        public int TipoVeiculoId { get; set; }

        [AtributoCampo(Nome = "PLACA")]
        public string Placa { get; set; }

        [AtributoCampo(Nome = "ANOLICENCIAMENTO")]
        public int AnoLicenciamento { get; set; }

        [AtributoCampo(Nome = "NOME")]
        public string Nome { get; set; }

        [AtributoCampo(Nome = "ANOMODELO")]
        public int AnoModelo { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEASSENTOS")]
        public int QuantidadeAssentos { get; set; }

        [AtributoCampo(Nome = "OBSERVACAO")]
        public string Observacao { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
