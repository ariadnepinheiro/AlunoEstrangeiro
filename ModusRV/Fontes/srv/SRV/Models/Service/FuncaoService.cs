using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Common.Loader;
using System.IO;
using System.Configuration;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class FuncaoService : ArquivoImportacaoService
    {
        public IList<Funcao> List()
        {
            IList<Funcao> funcoes = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                FuncaoMapper mapper = new FuncaoMapper();
                mapper.connection = conn;

                funcoes = mapper.List();
            }

            return funcoes;
        }

        public Paging<Funcao> List(FiltroFuncao filtro, int currentPage, int pageSize)
        {
            Paging<Funcao> funcoes = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                FuncaoMapper mapper = new FuncaoMapper();
                mapper.connection = conn;

                funcoes = mapper.List(filtro, currentPage, pageSize);
            }

            return funcoes;
        }

        public void Delete(string idFuncao, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    FuncaoMapper mapper = new FuncaoMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    Funcao funcaoOld = mapper.Find(idFuncao);

                    mapper.Delete(idFuncao);

                    AuditDelete(funcaoOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportFuncao> loader = new CsvFileLoader<ImportFuncao>(filename);

                List<ImportFuncao> list = loader.Import();

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        //Limpa log de erros anteriores
                        ArquivoImportacaoLogMapper arquivoImportacaoLogMapper = new ArquivoImportacaoLogMapper();
                        arquivoImportacaoLogMapper.connection = conn;
                        arquivoImportacaoLogMapper.transaction = trans;

                        arquivoImportacaoLogMapper.Delete(arquivoImportacao.IdArquivoImportacao);

                        ArquivoImportacaoMapper arquivoImportacaoMapper = new ArquivoImportacaoMapper();
                        arquivoImportacaoMapper.connection = conn;
                        arquivoImportacaoMapper.transaction = trans;

                        if (loader.HasError)
                        {
                            //Grava log de erros e atualiza status do arquivo
                            foreach (var error in loader.ErrorRecords)
                            {
                                arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                            }

                            arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                        }
                        else
                        {
                            GrupoFuncaoMapper grupoFuncaoMapper = new GrupoFuncaoMapper();
                            grupoFuncaoMapper.connection = conn;
                            grupoFuncaoMapper.transaction = trans;

                            //Valida grupo função
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i].CodGrupoFuncao != null && !grupoFuncaoMapper.ExisteGrupoFuncao(list[i].CodGrupoFuncao.Value))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodGrupoFuncao'"));
                            }

                            if (ErrorRecords.Count > 0)
                            {
                                //Grava log de erros e atualiza status do arquivo
                                foreach (var error in ErrorRecords)
                                {
                                    arquivoImportacaoLogMapper.Insert(arquivoImportacao.IdArquivoImportacao, error);
                                }

                                arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
                            }
                            else
                            {

                                //Insere ou atualiza registros da função
                                //Atualiza status do arquivo para CONCLUÍDO
                                FuncaoMapper funcaoMapper = new FuncaoMapper();
                                funcaoMapper.connection = conn;
                                funcaoMapper.transaction = trans;

                                Funcao funcao, funcaoOld;

                                foreach (var importFuncao in list)
                                {
                                    //Carrega os dados do função
                                    funcao = new Funcao();
                                    funcao.IdFuncao = importFuncao.CodFuncao;
                                    funcao.DesFuncao = importFuncao.DesFuncao;
                                    funcao.GrupoFuncao = new GrupoFuncao();
                                    funcao.GrupoFuncao.IdGrupoFuncao = importFuncao.CodGrupoFuncao;
                                    funcao.Gratificada = importFuncao.CodFuncao.Length <= 2 ? true : false;

                                    if (funcaoMapper.ExisteFuncao(importFuncao.CodFuncao))
                                    {
                                        funcaoOld = funcaoMapper.Find(importFuncao.CodFuncao);

                                        //Atualiza a função
                                        funcaoMapper.Update(funcao);
                                        AuditUpdate(funcao, funcaoOld, usuarioLogado, trans);
                                    }
                                    else
                                    {
                                        //Insere a função
                                        funcaoMapper.Insert(funcao);
                                        AuditInsert(funcao, usuarioLogado, trans);
                                    }
                                }

                                arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Concluido);
                            }
                        }

                        trans.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                DeleteLog(arquivoImportacao.IdArquivoImportacao);

                InsertLog(arquivoImportacao.IdArquivoImportacao, e.Message);

                UpdateStatus(arquivoImportacao, StatusImportacao.Falha);
            }
        }
    }
}