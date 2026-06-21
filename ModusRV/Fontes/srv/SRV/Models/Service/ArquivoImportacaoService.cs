using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using System.Configuration;
using System.IO;
using SRV.Common.Exceptions;
using System.Text;
using SRV.Models.DTO;
using SRV.Common.Loader;

namespace SRV.Models.Service
{
    public class ArquivoImportacaoService : BaseService
    {
        public ArquivoImportacao Find(int idArquivoImportacao)
        {
            ArquivoImportacao result = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ArquivoImportacaoMapper mapper = new ArquivoImportacaoMapper();
                mapper.connection = conn;

                result = mapper.Find(idArquivoImportacao);
            }

            return result;
        }

        public IList<ArquivoImportacao> ListByTipoImportacao(TipoImportacao tipoImportacao)
        {
            IList<ArquivoImportacao> result = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ArquivoImportacaoMapper mapper = new ArquivoImportacaoMapper();
                mapper.connection = conn;

                result = mapper.ListByTipoImportacao(tipoImportacao);
            }

            return result;
        }

        public void Insert(ArquivoImportacao arquivoImportacao)
        {
            arquivoImportacao.NmLinhas = GetLineCount(arquivoImportacao.DesArquivo);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ArquivoImportacaoMapper mapper = new ArquivoImportacaoMapper();
                mapper.connection = conn;

                mapper.Insert(arquivoImportacao);
            }
        }

        /// <summary>
        /// Apaga o arquivo físico e o registro associado no banco de dados. É permitida apenas a exclusão de arquivos
        /// com status diferente de CONCLUÍDO.
        /// </summary>
        /// <param name="idArquivoImportacao"></param>
        public void Delete(int idArquivoImportacao)
        {
            ArquivoImportacao arquivoImportacao = Find(idArquivoImportacao);

            if (arquivoImportacao.StatusImportacao != StatusImportacao.Concluido)
            {
                DeleteFile(arquivoImportacao.DesArquivo);

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        ArquivoImportacaoLogMapper logMapper = new ArquivoImportacaoLogMapper();
                        logMapper.connection = conn;
                        logMapper.transaction = trans;

                        logMapper.Delete(idArquivoImportacao);


                        ArquivoImportacaoMapper mapper = new ArquivoImportacaoMapper();
                        mapper.connection = conn;
                        mapper.transaction = trans;

                        mapper.Delete(idArquivoImportacao);

                        trans.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Atualiza o status do arquivo de importação. Se o novo status for EM EXECUÇÃO, também serão atualizados
        /// o usuário e data da importação.
        /// </summary>
        /// <param name="arquivoImportacao"></param>
        /// <param name="statusImportacao"></param>
        public void UpdateStatus(ArquivoImportacao arquivoImportacao, StatusImportacao statusImportacao)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ArquivoImportacaoMapper mapper = new ArquivoImportacaoMapper();
                mapper.connection = conn;

                if (statusImportacao == StatusImportacao.EmExecucao)
                    mapper.UpdateStatusEmExecucao(arquivoImportacao);
                else
                    mapper.UpdateStatus(arquivoImportacao, statusImportacao);
            }

        }

        protected void InsertLog(int idArquivoImportacao, string log)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ArquivoImportacaoLogMapper mapper = new ArquivoImportacaoLogMapper();
                mapper.connection = conn;

                mapper.Insert(idArquivoImportacao, log);
            }
        }

        protected void DeleteLog(int idArquivoImportacao)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                ArquivoImportacaoLogMapper mapper = new ArquivoImportacaoLogMapper();
                mapper.connection = conn;

                mapper.Delete(idArquivoImportacao);
            }
        }

        private int GetLineCount(string filename)
        {
            int lines = 0;

            string pathUpload = ConfigurationManager.AppSettings["PathUpload"];

            using (StreamReader sr = new StreamReader(Path.Combine(pathUpload, filename)))
            {
                String lineData;
                while ((lineData = sr.ReadLine()) != null)
                {
                    lines++;
                }
            }

            return lines;
        }

        private void DeleteFile(string filename)
        {
            string pathUpload = ConfigurationManager.AppSettings["PathUpload"];

            File.Delete(Path.Combine(pathUpload, filename));
        }

    }
}