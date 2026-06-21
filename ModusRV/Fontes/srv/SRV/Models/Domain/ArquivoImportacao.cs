using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SRV.Models.Domain
{
    public enum TipoImportacao
    {
        AvaliacaoExternaUnidadeAdmin = 1,
        CriterioUnidadeAdministrativa = 2,
        Funcao = 3,
        FuncaoServidor = 4,
        IndicadorUnidadeAdministrativa = 5,
        MetaIGEUnidadeAdministrativa = 6,
        MetaUnidadeAdministrativa = 7,
        NivelEnsinoUnidadeAdministrativa = 8,
        OcorrenciaServidor = 9,
        Servidor = 10,
        UnidadeAdministrativa = 11,
        LancamentoNotasDocente = 12,
        AplicacaoProvaAvaliacaoExterna = 13,
        DenunciaAvaliacaoExterna = 14
    }

    public enum StatusImportacao
    {
        [DescriptionAttribute("PENDENTE")]
        Pendente = 0,
        
        [DescriptionAttribute("EM EXECUÇÃO")]
        EmExecucao = 1,
        
        [DescriptionAttribute("CONCLUÍDO")]
        Concluido = 2,

        [DescriptionAttribute("FALHA")]
        Falha = 3
    }

    public class ArquivoImportacao
    {
        public int IdArquivoImportacao { get; set; }

        public TipoImportacao TipoImportacao { get; set; }
        public StatusImportacao StatusImportacao { get; set; }

        public string DesArquivoOriginal { get; set; }
        public string DesArquivo { get; set; }
        public int NmLinhas { get; set; }

        public DateTime DtUpload { get; set; }
        public Usuario UsuarioUpload { get; set; }

        public DateTime DtImportacao { get; set; }
        public Usuario UsuarioImportacao { get; set; }
    }
}