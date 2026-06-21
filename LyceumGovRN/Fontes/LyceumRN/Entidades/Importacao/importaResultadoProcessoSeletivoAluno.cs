using System;
using Seeduc.Infra.Entities;
using Techne.Lyceum.RN.Util.ImportacaoArquivo;

namespace Techne.Lyceum.RN.Entidades.Importacao
{
    [ImportaAtributoArquivo(Colunas = 2, CabecalhoPrimeiraLinha = true)]
    public class ImportaResultadoProcessoSeletivoAluno : IEntity
    {
        public const int posicaoNumeroInscricao = 0;
        public const int posicaoOrdem = 1;

        [ImportaAtributoCampo(
            posicaoNumeroInscricao,
            NomeCampo = "NumeroInscricao",
            RetiraEspacos = true,
            ValidacaoRequirida = true,
            TamanhoMaximoCampo = 19,
            Requirido = true,
            TipoDado = DataType.Integer)]
        public Int64 NumeroInscricao { get; set; }

        [ImportaAtributoCampo(
            posicaoOrdem,
            NomeCampo = "Ordem",
            RetiraEspacos = true,
            ValidacaoRequirida = true,
            TamanhoMaximoCampo = 8,
            Requirido = true,
            TipoDado = DataType.Integer)]
        public int Ordem { get; set; }
    }
}

