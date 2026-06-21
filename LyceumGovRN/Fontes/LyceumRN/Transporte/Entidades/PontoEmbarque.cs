using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.PONTOEMBARQUE", Nome = "Transporte.PONTOEMBARQUE")]
    public class PontoEmbarque : IEntity
    {
        [AtributoCampo(Nome = "PONTOEMBARQUEID")]
        public int PontoEmbarqueId { get; set; }

        [AtributoCampo(Nome = "ROTATRAJETOID")]
        public int RotaTrajetoId { get; set; }

        public bool Primeiro { get; set; }

        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Municipio { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
