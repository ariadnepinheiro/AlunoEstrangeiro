using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    public class FonteRecurso
    {
        [AtributoCampo(Nome = "FONTERECURSOID")]
        public int FonteRecursoId { get; set; }

        [AtributoCampo(Nome = "CODIGOSEFAZ")]
        public string CodigoSefaz { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }
        
        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
