using System;
using System.Collections.Generic;
using Seeduc.Infra.Data;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.RN.Importacao
{
    public class ImportaResultadoProcessoSeletivoAluno : ImportacaoArquivo<RN.Entidades.Importacao.ImportaResultadoProcessoSeletivoAluno>
    {
        public ImportaResultadoProcessoSeletivoAluno(int idChave, HtmlInputFile fileUploadImportacao, RN.HistoricoImportacao.TipoImportacaoEnum enumTipoImportacao, string usuario) 
            : base(idChave, fileUploadImportacao, enumTipoImportacao, usuario) { }

        /// <summary>
        /// Método Override para realizar o processo de importação e validação necessária da classe
        /// </summary>
        /// <param name="idChaveRelacionamento"></param>
        /// <param name="listaEntidadeImportacao"></param>
        /// <param name="ErroRegistros"></param>
        /// <param name="tipoEntradaSistema"></param>
        /// <param name="contexto"></param>
        /// <returns>True importadado com sucesso e False quando um erro é encontrado.</returns>
        public override bool Importar(int idChaveRelacionamento, List<RN.Entidades.Importacao.ImportaResultadoProcessoSeletivoAluno> listaEntidadeImportacao, List<string> ErroRegistros, RN.HistoricoImportacao.TipoEntradaSistemaEnum tipoEntradaSistema, DataContext contexto)
        {
            bool ImportadoComSucesso = true;
            bool numeroInscricaoValido = false;
            bool numeroInscricaoImportadoOutraChamada = false;
            DateTime horaInicial = DateTime.Now;

            RN.ProcessoSeletivoAluno.Inscricao rnInscricao = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Inscricao();
            string strSQL = string.Empty;
            int numeroLinhas = 1;

            try
            {
                foreach (RN.Entidades.Importacao.ImportaResultadoProcessoSeletivoAluno importaResultadoProcessoSeletivoAluno in listaEntidadeImportacao)
                {
                    numeroLinhas++;

                    numeroInscricaoValido = rnInscricao.VerificaNumeroInscricaoValido(this.IdChave, importaResultadoProcessoSeletivoAluno.NumeroInscricao, contexto);
                    if (!numeroInscricaoValido)
                    {
                        ErroRegistros.Add(string.Format("Linha {0}: Número de Inscrição inválido!", numeroLinhas));
                        ImportadoComSucesso = false;
                    }

                    numeroInscricaoImportadoOutraChamada = rnInscricao.VerificaNumeroInscricaoImportadoOutraChamada(this.IdChave, importaResultadoProcessoSeletivoAluno.NumeroInscricao, contexto);
                    if (numeroInscricaoImportadoOutraChamada)
                    {
                        ErroRegistros.Add(string.Format("Linha {0}: Número de Inscrição já importado para outra chamada!", numeroLinhas));
                        ImportadoComSucesso = false;
                    }
                }

                if (ImportadoComSucesso)
                {
                    //Query para Limpar os Candidatos Classificados Importados para a mesma chamada
                    rnInscricao.ExcluiCandidatosImportados(this.IdChave, contexto);

                    //Atualizar a situação do Candidato para Classificado
                    foreach (RN.Entidades.Importacao.ImportaResultadoProcessoSeletivoAluno importaResultadoProcessoSeletivoAluno in listaEntidadeImportacao)
                    {
                        rnInscricao.AtualizaSituacaoCandidatoParaClassificado(importaResultadoProcessoSeletivoAluno, idChaveRelacionamento, contexto);
                    }
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            DateTime horaFinal = DateTime.Now;

            return ImportadoComSucesso;
        }

        /// <summary>
        /// Método Override para gravar relacionamento de Histórico de Importação
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns>Id do relacionamento gerado</returns>
        public override int InserirRelacionamentoHistoricoImportacao(DataContext contexto)
        {
            int idProcessoSeletivo_HistoricoImportacao = int.MinValue;

            RN.ProcessoSeletivoAluno.ProcessoSeletivo_HistoricoImportacao rnProcessoSeletivo_HistoricoImportacao = new Techne.Lyceum.RN.ProcessoSeletivoAluno.ProcessoSeletivo_HistoricoImportacao();
            Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.ProcessoSeletivo_HistoricoImportacao processoSeletivo_HistoricoImportacao = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.ProcessoSeletivo_HistoricoImportacao();

            //Inclusão do Relacionamento entre Histórico de Importação e Processo Seletivo
            processoSeletivo_HistoricoImportacao = rnProcessoSeletivo_HistoricoImportacao.CriaEntidadeProcessoSeletivo_HistoricoImportacao(this.IdChave, this.IdHistoricoImportacao);
            idProcessoSeletivo_HistoricoImportacao = rnProcessoSeletivo_HistoricoImportacao.InsereProcessoSeletivo_HistoricoImportacao(processoSeletivo_HistoricoImportacao, contexto);

            return idProcessoSeletivo_HistoricoImportacao;
        }
    }
}
