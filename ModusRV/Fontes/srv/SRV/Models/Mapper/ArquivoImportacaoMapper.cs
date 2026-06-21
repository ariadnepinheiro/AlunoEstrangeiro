using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ArquivoImportacaoMapper : BaseMapper<ArquivoImportacao>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT * 
                       FROM rv_arquivo_importacao 
                      WHERE id_arquivo_importacao = @idArquivoImportacao";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryListByTipoImportacao()
        {
            return @"SELECT * 
                       FROM rv_arquivo_importacao 
                      WHERE tipo_importacao = @tipoImportacao
                        AND status_importacao <> @statusConcluido
                      UNION
                     SELECT * 
                       FROM rv_arquivo_importacao 
                      WHERE tipo_importacao = @tipoImportacao
                        AND status_importacao = @statusConcluido
                        AND id_arquivo_importacao = (SELECT MAX(id_arquivo_importacao) 
                                                       FROM rv_arquivo_importacao
                                                      WHERE tipo_importacao = @tipoImportacao
                                                        AND status_importacao = @statusConcluido)
                   ORDER BY dt_upload DESC";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_arquivo_importacao
                                (tipo_importacao,
                                 status_importacao,
                                 des_arquivo_original,
                                 des_arquivo,
                                 nm_linhas,
                                 dt_upload,
                                 id_usuario_upload)
                         VALUES (@tipoImportacao,
                                 @statusImportacao,
                                 @desArquivoOriginal,
                                 @desArquivo,
                                 @nmLinhas,
                                 GETDATE(),
                                 @idUsuarioUpload)";
        }

        private string QueryUpdateEmExecucao()
        {
            return @"UPDATE rv_arquivo_importacao
                        SET status_importacao = @statusEmExecucao,
                            dt_importacao = GETDATE(),
                            id_usuario_importacao = @idUsuarioImportacao
                      WHERE id_arquivo_importacao = @idArquivoImportacao";
        }

        private string QueryUpdateStatus()
        {
            return @"UPDATE rv_arquivo_importacao
                        SET status_importacao = @statusImportacao
                      WHERE id_arquivo_importacao = @idArquivoImportacao";
        }

        private string QueryDelete()
        {
            return @"DELETE 
                       FROM rv_arquivo_importacao
                      WHERE id_arquivo_importacao = @idArquivoImportacao
                        AND status_importacao <> @statusConcluido";
        }

        public override ArquivoImportacao LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            ArquivoImportacao arquivoImportacao = new ArquivoImportacao();

            arquivoImportacao.IdArquivoImportacao = (int)reader["id_arquivo_importacao"];
            arquivoImportacao.DesArquivoOriginal = (string)reader["des_arquivo_original"];
            arquivoImportacao.DesArquivo = (string)reader["des_arquivo"];
            arquivoImportacao.NmLinhas = (int)reader["nm_linhas"];
            arquivoImportacao.DtUpload = (DateTime)reader["dt_upload"];
            arquivoImportacao.TipoImportacao = (TipoImportacao)Enum.ToObject(typeof(TipoImportacao), Convert.ToInt32(reader["tipo_importacao"]));
            arquivoImportacao.StatusImportacao = (StatusImportacao)Enum.ToObject(typeof(StatusImportacao), Convert.ToInt32(reader["status_importacao"]));

            return arquivoImportacao;
        }

        public ArquivoImportacao Find(int idArquivoImportacao)
        {
            return FindObject("idArquivoImportacao", idArquivoImportacao);
        }

        public IList<ArquivoImportacao> ListByTipoImportacao(TipoImportacao tipoImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("tipoImportacao", (int)tipoImportacao);
            param.Add("statusConcluido", StatusImportacao.Concluido);

            return ListObjects(QueryListByTipoImportacao(), param);
        }

        public void Insert(ArquivoImportacao arquivoImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("tipoImportacao", arquivoImportacao.TipoImportacao);
            param.Add("statusImportacao", StatusImportacao.Pendente);
            param.Add("desArquivoOriginal", arquivoImportacao.DesArquivoOriginal);
            param.Add("desArquivo", arquivoImportacao.DesArquivo);
            param.Add("nmLinhas", arquivoImportacao.NmLinhas);
            param.Add("idUsuarioUpload", arquivoImportacao.UsuarioUpload.Id);

            InsertObject(QueryInsert(), param);
        }

        /// <summary>
        /// Atualiza o status da importação para EmExecucao e grava data e usuário que iniciou a tarefa
        /// </summary>
        /// <param name="arquivoImportacao"></param>
        public void UpdateStatusEmExecucao(ArquivoImportacao arquivoImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("statusEmExecucao", StatusImportacao.EmExecucao);
            param.Add("idArquivoImportacao", arquivoImportacao.IdArquivoImportacao);
            param.Add("idUsuarioImportacao", arquivoImportacao.UsuarioImportacao.Id);

            UpdateObject(QueryUpdateEmExecucao(), param);
        }

        /// <summary>
        /// Atualiza o status da importação
        /// </summary>
        /// <param name="arquivoImportacao"></param>
        /// <param name="statusImportacao"></param>
        public void UpdateStatus(ArquivoImportacao arquivoImportacao, StatusImportacao statusImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("statusImportacao", statusImportacao);
            param.Add("idArquivoImportacao", arquivoImportacao.IdArquivoImportacao);

            UpdateObject(QueryUpdateStatus(), param);
        }


        public void Delete(int idArquivoImportacao)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idArquivoImportacao", idArquivoImportacao);
            param.Add("statusConcluido", StatusImportacao.Concluido);

            DeleteObject(QueryDelete(), param);
        }

    }
}