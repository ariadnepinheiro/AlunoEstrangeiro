using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.GestaoRede.Entidades
{
    [AtributoTabela("GestaoRede.REGIAOFINANCEIRAMUNICIPIO", Nome = "GestaoRede.REGIAOFINANCEIRAMUNICIPIO")]
    public class RegiaoFinanceiraMunicipio : IEntity
    {
        [AtributoCampo(Nome = "REGIAOFINANCEIRAMUNICIPIOID")]
        public int RegiaoFinanceiraMunicipioId { get; set; }

        [AtributoCampo(Nome = "REGIAOFINANCEIRAID")]
        public int RegiaoFinanceiraId { get; set; }

        [AtributoCampo(Nome = "MUNICIPIOID")]
        public string MunicipioId { get; set; }

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