using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyUnidadeFisicaConcessionaria : IEntity
    {
        //Campos utilizados para nova tela:
        public int IdUnidadeFisicaConcessionaria { get; set; }

        public string UnidadeFis { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        //Enegia Eletrica
        public string EeTipoAbastecimento { get; set; }

        public string EeEmpresa { get; set; }

        public string EeCodCliente { get; set; }

        public string EeClasseFornecimento { get; set; }

        public string EeContratoFornecimento { get; set; }

        //Suprimento de agua
        public string AgOutrosAbastecimentos { get; set; }

        public string AgEmpresa { get; set; }

        public string AgCodCliente { get; set; }

        public string AgHidrometro { get; set; }

        public string AgPcArtesiano { get; set; }

        public string AgPcSemiArtesiano { get; set; }

        public string AgPcCacimba { get; set; }

        public string AgPcCarroPipa { get; set; }

        public string AgTipoAguaConsumida { get; set; }

        public string AgPcBombaSubmersa { get; set; }

        public string AgPcProfundidade { get; set; }

        public string AgPcVazao { get; set; }

        //Suprimento de gas
        public string GaEmpresa { get; set; }

        public string GaCodCliente { get; set; }

        public string GaTipo { get; set; }

        //Esgoto / Destinação de Lixo
        public string ElEsgotoSanitario { get; set; }

        public string ElDestinacaoLixo { get; set; }      
    }
}
