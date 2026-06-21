using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;
using SRV.Common.Exceptions;
using SRV.Common;

namespace SRV.Models.Service
{
    public class UnidadeAdministrativaService : ArquivoImportacaoService
    {
        public IList<UnidadeAdministrativa> List()
        {
            IList<UnidadeAdministrativa> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                mapper.connection = conn;

                result = mapper.List();
            }

            return result;
        }

        public IList<UnidadeAdministrativa> List(int idAnoReferencia, int idRegional, UserState usuario)
        {
            IList<UnidadeAdministrativa> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                mapper.connection = conn;

                result = mapper.List(idAnoReferencia, idRegional, usuario);
            }

            return result;
        }

        public IList<UnidadeAdministrativa> ListRegional(int idAnoReferencia, UserState usuario)
        {
            IList<UnidadeAdministrativa> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                mapper.connection = conn;

                result = mapper.ListRegional(idAnoReferencia, usuario);
            }

            return result;
        }

        public Paging<UnidadeAdministrativa> ListPesquisa(FiltroUnidadeAdministrativa filtro, int currentPage, int pageSize)
        {
            Paging<UnidadeAdministrativa> unidades;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                mapper.connection = conn;

                unidades = mapper.ListPesquisa(filtro, currentPage, pageSize);
            }

            return unidades;
        }

        public void Delete(int idUnidadeAdministrativa, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    UnidadeAdministrativa unidadeAdministrativaOld = mapper.Find(idUnidadeAdministrativa);

                    mapper.Delete(idUnidadeAdministrativa);

                    AuditDelete(unidadeAdministrativaOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        public void ImportRegional(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportUnidadeAdministrativaRegional> loader = new CsvFileLoader<ImportUnidadeAdministrativaRegional>(filename);

                List<ImportUnidadeAdministrativaRegional> list = loader.Import();

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

                            //Insere ou atualiza registros de Unidade Administrativa
                            //Atualiza status do arquivo para CONCLUÍDO
                            UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                            mapper.connection = conn;
                            mapper.transaction = trans;

                            UnidadeAdministrativa unidadeAdministrativaRegional, unidadeAdministrativaRegionalOld;

                            foreach (var importUnidadeAdministrativa in list)
                            {
                                //Carrega os dados do servidor
                                unidadeAdministrativaRegional = new UnidadeAdministrativa();

                                unidadeAdministrativaRegional.IdUnidadeAdministrativa = importUnidadeAdministrativa.CodUnidadeAdministrativa;
                                unidadeAdministrativaRegional.DesUnidadeAdministrativa = importUnidadeAdministrativa.DesUnidadeAdministrativa;
                                unidadeAdministrativaRegional.IdCenso = importUnidadeAdministrativa.CodCenso.Length > 0 ? importUnidadeAdministrativa.CodCenso : null;
                                unidadeAdministrativaRegional.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
                                unidadeAdministrativaRegional.TipoUnidadeAdministrativa.IdTipoUnidAdm = Constants.TipoUnidAdmRegional;

                                if (mapper.ExisteUnidadeAdministrativa(unidadeAdministrativaRegional.IdUnidadeAdministrativa.Value))
                                {
                                    unidadeAdministrativaRegionalOld = mapper.Find(unidadeAdministrativaRegional.IdUnidadeAdministrativa.Value);

                                    //Atualiza a Unidade Administrativa Regional
                                    mapper.Update(unidadeAdministrativaRegional);

                                    AuditUpdate(unidadeAdministrativaRegional, unidadeAdministrativaRegionalOld, usuarioLogado, trans);

                                }
                                else
                                {
                                    //Insere a Unidade Administrativa Regional
                                    mapper.Insert(unidadeAdministrativaRegional);
                                    AuditInsert(unidadeAdministrativaRegional, usuarioLogado, trans);

                                }
                            }

                                arquivoImportacaoMapper.UpdateStatus(arquivoImportacao, StatusImportacao.Concluido);
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

        public void ImportEscolar(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportUnidadeAdministrativaEscolar> loader = new CsvFileLoader<ImportUnidadeAdministrativaEscolar>(filename);

                List<ImportUnidadeAdministrativaEscolar> list = loader.Import();

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

                            UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                            mapper.connection = conn;
                            mapper.transaction = trans;

                            //Valida a unidade regional
                            for (int i = 0; i < list.Count; i++)
                            {

                                if (!mapper.ExistebyTipo(list[i].CodUnidadeRegional, Constants.TipoUnidAdmRegional))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodUnidadeRegional'"));

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
                                //Insere ou atualiza registros de Unidade Administrativa
                                //Atualiza status do arquivo para CONCLUÍDO

                                UnidadeAdministrativa unidadeAdministrativaEscolar, unidadeAdministrativaEscolarOld;

                                foreach (var importUnidadeAdministrativa in list)
                                {
                                    //Carrega os dados do servidor
                                    unidadeAdministrativaEscolar = new UnidadeAdministrativa();

                                    unidadeAdministrativaEscolar.IdUnidadeAdministrativa = importUnidadeAdministrativa.CodUnidadeAdministrativa;
                                    unidadeAdministrativaEscolar.DesUnidadeAdministrativa = importUnidadeAdministrativa.DesUnidadeAdministrativa;

                                    unidadeAdministrativaEscolar.Regional = new UnidadeAdministrativa();
                                    unidadeAdministrativaEscolar.Regional.IdUnidadeAdministrativa = importUnidadeAdministrativa.CodUnidadeRegional;
                                    unidadeAdministrativaEscolar.IdCenso = importUnidadeAdministrativa.CodCenso;

                                    unidadeAdministrativaEscolar.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
                                    unidadeAdministrativaEscolar.TipoUnidadeAdministrativa.IdTipoUnidAdm = Constants.TipoUnidAdmEscolar;

                                    if (mapper.ExisteUnidadeAdministrativa(unidadeAdministrativaEscolar.IdUnidadeAdministrativa.Value))
                                    {
                                        unidadeAdministrativaEscolarOld = mapper.Find(unidadeAdministrativaEscolar.IdUnidadeAdministrativa.Value);

                                        //Atualiza a Unidade Administrativa Regional
                                        mapper.Update(unidadeAdministrativaEscolar);

                                        AuditUpdate(unidadeAdministrativaEscolar, unidadeAdministrativaEscolarOld, usuarioLogado, trans);

                                    }
                                    else
                                    {
                                        //Insere a Unidade Administrativa Regional
                                        mapper.Insert(unidadeAdministrativaEscolar);
                                        AuditInsert(unidadeAdministrativaEscolar, usuarioLogado, trans);

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

        public void ImportDemaisUnidades(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            List<string> ErrorRecords = new List<string>();

            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportUnidadeAdministrativaDemaisUnidades> loader = new CsvFileLoader<ImportUnidadeAdministrativaDemaisUnidades>(filename);

                List<ImportUnidadeAdministrativaDemaisUnidades> list = loader.Import();

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

                            UnidadeAdministrativaMapper mapper = new UnidadeAdministrativaMapper();
                            mapper.connection = conn;
                            mapper.transaction = trans;


                            TipoUnidadeAdministrativaMapper tipoUnidadeAdministrativaMapper = new TipoUnidadeAdministrativaMapper();
                            tipoUnidadeAdministrativaMapper.connection = conn;
                            tipoUnidadeAdministrativaMapper.transaction = trans;

                            //Valida a unidade regional
                            for (int i = 0; i < list.Count; i++)
                            {

                                if (!mapper.ExistebyTipo(list[i].CodUnidadeRegional, Constants.TipoUnidAdmRegional))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodUnidadeRegional'"));

                                if (list[i].CodTipoUnidadeAdministrativa == Constants.TipoUnidAdmRegional || list[i].CodTipoUnidadeAdministrativa == Constants.TipoUnidAdmEscolar)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodTipoUnidadeAdministrativa'"));
                                else if (!tipoUnidadeAdministrativaMapper.ExisteTipoUnidadeAdministrativa(list[i].CodTipoUnidadeAdministrativa))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodTipoUnidadeAdministrativa'"));

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
                                UnidadeAdministrativa unidadeAdministrativaDemaisUnidades, unidadeAdministrativaDemaisUnidadesOld;

                                foreach (var importUnidadeAdministrativa in list)
                                {
                                    //Carrega os dados do servidor
                                    unidadeAdministrativaDemaisUnidades = new UnidadeAdministrativa();

                                    unidadeAdministrativaDemaisUnidades.IdUnidadeAdministrativa = importUnidadeAdministrativa.CodUnidadeAdministrativa;
                                    unidadeAdministrativaDemaisUnidades.DesUnidadeAdministrativa = importUnidadeAdministrativa.DesUnidadeAdministrativa;
                                    unidadeAdministrativaDemaisUnidades.IdCenso = importUnidadeAdministrativa.CodCenso.Length > 0 ? importUnidadeAdministrativa.CodCenso : null;
                                    unidadeAdministrativaDemaisUnidades.Regional = new UnidadeAdministrativa();
                                    unidadeAdministrativaDemaisUnidades.Regional.IdUnidadeAdministrativa = importUnidadeAdministrativa.CodUnidadeRegional;
                                    unidadeAdministrativaDemaisUnidades.TipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();
                                    unidadeAdministrativaDemaisUnidades.TipoUnidadeAdministrativa.IdTipoUnidAdm = importUnidadeAdministrativa.CodTipoUnidadeAdministrativa;

                                    if (mapper.ExisteUnidadeAdministrativa(unidadeAdministrativaDemaisUnidades.IdUnidadeAdministrativa.Value))
                                    {
                                        unidadeAdministrativaDemaisUnidadesOld = mapper.Find(unidadeAdministrativaDemaisUnidades.IdUnidadeAdministrativa.Value);

                                        //Atualiza a Unidade Administrativa Regional
                                        mapper.Update(unidadeAdministrativaDemaisUnidades);

                                        AuditUpdate(unidadeAdministrativaDemaisUnidades, unidadeAdministrativaDemaisUnidadesOld, usuarioLogado, trans);

                                    }
                                    else
                                    {
                                        //Insere a Unidade Administrativa Regional
                                        mapper.Insert(unidadeAdministrativaDemaisUnidades);
                                        AuditInsert(unidadeAdministrativaDemaisUnidades, usuarioLogado, trans);

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