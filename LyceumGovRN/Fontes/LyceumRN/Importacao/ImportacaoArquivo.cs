using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Seeduc.Infra.Entities;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Util.ImportacaoArquivo;

namespace Techne.Lyceum.RN.Importacao
{
    public abstract class ImportacaoArquivo<ClasseEntidadeImportacao> where ClasseEntidadeImportacao : class, new()
    {
        #region Propriedades

        private System.Web.UI.HtmlControls.HtmlInputFile fileUploadImportacao;
        private RN.HistoricoImportacao.TipoImportacaoEnum tipoImportacaoEnum;
        private string usuario;
        private string mensagem;

        public bool TemErro { get; set; }

        public int IdChave { get; set; }
        public int IdHistoricoImportacao { get; set; }

        #endregion

        /// <summary>
        /// Construtor da Classe
        /// </summary>
        /// <param name="idChave"></param>
        /// <param name="fileUploadImportacao"></param>
        /// <param name="enumTipoImportacao"></param>
        /// <param name="usuario"></param>
        public ImportacaoArquivo(int idChave, System.Web.UI.HtmlControls.HtmlInputFile fileUploadImportacao, RN.HistoricoImportacao.TipoImportacaoEnum enumTipoImportacao, string usuario)
        {
            this.TemErro = false;

            this.mensagem = string.Empty;
            this.fileUploadImportacao = fileUploadImportacao;
            this.tipoImportacaoEnum = enumTipoImportacao;
            this.usuario = usuario;

            this.IdChave = idChave;
            this.IdHistoricoImportacao = int.MinValue;
        }

        /// <summary>
        /// Método que realiza a importação do Arquivo
        /// </summary>
        /// <returns>Mensagem para exibição.</returns>
        public string ImportarArquivo()
        {
            DataContext contexto = null;

            int idRelacionamentoHistoricoImportacao;

            RN.HistoricoImportacao historicoImportacao = new Techne.Lyceum.RN.HistoricoImportacao();
            RN.Entidades.HistoricoImportacao entidadeHistoricoImportacao = new Techne.Lyceum.RN.Entidades.HistoricoImportacao();

            Stream streamArquivoImportacao = null;
            FileInfo arquivoImportadoInfo = null;
            string nomeArquivoImportado = string.Empty;
            string nomeArquivoLog = string.Empty;

            ZipFileStream zipFileStream = new ZipFileStream();
            MemoryStream streamArquivoLog = new MemoryStream();
            byte[] bytesArquivoZipado = null;
            byte[] data = null;

            List<ClasseEntidadeImportacao> listaImportacao = null;

            if (ValidarArquivo())
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                    arquivoImportadoInfo = new FileInfo(fileUploadImportacao.PostedFile.FileName);
                    nomeArquivoImportado = arquivoImportadoInfo.Name;
                    streamArquivoImportacao = fileUploadImportacao.PostedFile.InputStream;

                    ImportaArquivoCsv<ClasseEntidadeImportacao> loader = new ImportaArquivoCsv<ClasseEntidadeImportacao>(streamArquivoImportacao);

                    //Zip do arquivo importado para gerar histórico de importação
                    zipFileStream.AdicionaFileStreamZip(nomeArquivoImportado, streamArquivoImportacao);

                    bytesArquivoZipado = zipFileStream.retornaBytesZipados();

                    //Inclusão de Histórico de Importação com a situação como “Em Execução”
                    entidadeHistoricoImportacao = historicoImportacao.AdicionaItemHistorico(arquivoImportadoInfo.Name, bytesArquivoZipado, RN.HistoricoImportacao.StatusProcessamento.EmExecucao, RN.HistoricoImportacao.TipoEntradaSistemaEnum.ImportacaoCSV, tipoImportacaoEnum, this.usuario);
                    IdHistoricoImportacao = historicoImportacao.Inserir(entidadeHistoricoImportacao, contexto);

                    //Inclui o relacionamento entre o Histórico de Importação e a tabela de relacionamento
                    idRelacionamentoHistoricoImportacao = InserirRelacionamentoHistoricoImportacao(contexto);

                    //Realiza leitura do arquivo e validação das informações
                    listaImportacao = loader.Importar();

                    if (loader.ErroSistema != string.Empty)
                    {
                        mensagem = loader.ErroSistema;

                        // Cancela a transação
                        contexto.Abandon();
                    }
                    else
                    {
                        var result = Importar(idRelacionamentoHistoricoImportacao, listaImportacao, loader.ErroRegistros, RN.HistoricoImportacao.TipoEntradaSistemaEnum.ImportacaoCSV, contexto);

                        if (!loader.TemErro && result == true)
                        {
                            mensagem = "Importação do arquivo concluída com sucesso.";

                            // Atualização do Registro de Histórico de Importação com a situação como “Concluído”.
                            historicoImportacao.AtualizarImportacaoConcluida(IdHistoricoImportacao, bytesArquivoZipado, RN.HistoricoImportacao.StatusProcessamento.Concluido, listaImportacao.Count, contexto);
                        }
                        else
                        {
                            mensagem = "Ocorreram erros durante a importação. Favor verificar o histórico de importação.";
                            nomeArquivoLog = "log_erros.csv";

                            streamArquivoLog = new MemoryStream();

                            foreach (string mensagemErro in loader.ErroRegistros)
                            {
                                data = System.Text.Encoding.Default.GetBytes(mensagemErro + Environment.NewLine);
                                streamArquivoLog.Write(data, 0, data.Length);
                            }

                            zipFileStream.AdicionaFileStreamZip(nomeArquivoLog, streamArquivoLog);
                            bytesArquivoZipado = zipFileStream.retornaBytesZipados();

                            // Sistema atualiza o registro de histórico de importação gerado para a situação como “FALHA”. 
                            historicoImportacao.Atualizar(IdHistoricoImportacao, bytesArquivoZipado, RN.HistoricoImportacao.StatusProcessamento.Falha, contexto);
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (contexto != null)
                        contexto.Abandon();

                    mensagem = ex.Message.Replace("'", "").Replace(Environment.NewLine, "").Replace(@"\", @"/");
                }
                finally
                {
                    if (contexto != null)
                        contexto.Dispose();

                    if (streamArquivoImportacao != null)
                    {
                        streamArquivoImportacao.Flush();
                        streamArquivoImportacao.Close();
                        streamArquivoImportacao.Dispose();
                    }
                }
            }
            else
            {
                this.TemErro = true;
            }

            return mensagem;
        }

        public abstract bool Importar(int idChaveRelacionamento, List<ClasseEntidadeImportacao> listaEntidadeImportacao, List<string> ErroRegistros, RN.HistoricoImportacao.TipoEntradaSistemaEnum tipoEntradaSistema, DataContext contexto);
        public abstract int InserirRelacionamentoHistoricoImportacao(DataContext contexto);

        /// <summary>
        /// Realiza as validações do arquivo
        /// </summary>
        /// <returns>Retorna false para validado com erros</returns>
        private bool ValidarArquivo()
        {
            if (string.IsNullOrEmpty(fileUploadImportacao.Value))
            {
                mensagem = "Ocorreu um erro ao tentar importar o arquivo. Tente Novamente!";
                return false;
            }

            if (fileUploadImportacao.PostedFile.ContentType != "application/vnd.ms-excel")
            {
                mensagem = "Tipo de arquivo não compatível para importação.";
                return false;
            }

            if (fileUploadImportacao.PostedFile.ContentLength == 0)
            {
                mensagem = "Arquivo de Importação não encontrado ou sem dados.";
                return false;
            }

            if (fileUploadImportacao.PostedFile.FileName == string.Empty)
            {
                mensagem = "Ocorreu um erro ao tentar importar o arquivo. Tente Novamente!";
                return false;
            }

            return true;
        }
    }
}
