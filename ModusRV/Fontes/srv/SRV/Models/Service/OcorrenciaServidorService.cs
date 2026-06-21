using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;
using System.Web.Mvc;
using SRV.Common;

namespace SRV.Models.Service
{
    public class OcorrenciaServidorService : ArquivoImportacaoService
    {
        private readonly ModelStateDictionary modelState;

		public OcorrenciaServidorService(){}

		public OcorrenciaServidorService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }


		public Paging<OcorrenciaServidor> List(FiltroOcorrenciaServidor filtro, int currentPage, int pageSize)
        {
            Paging<OcorrenciaServidor> list;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                OcorrenciaServidorMapper mapper = new OcorrenciaServidorMapper();
                mapper.connection = conn;

                list = mapper.List(filtro, currentPage, pageSize);
            }

            return list;
        }

        public void Delete(int idOcorrenciaServidor, UserState usuario)
        {
			this.Delete(idOcorrenciaServidor, usuario, null);
        }

		public void Delete(int idOcorrenciaServidor, UserState usuario, string numeroRecurso)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				using (SqlTransaction trans = conn.BeginTransaction())
				{
					OcorrenciaServidorMapper mapper = new OcorrenciaServidorMapper();
					mapper.connection = conn;
					mapper.transaction = trans;

					OcorrenciaServidor ocorrenciaServidorOld = mapper.Find(idOcorrenciaServidor);
					
					if (!String.IsNullOrEmpty(numeroRecurso))
						ocorrenciaServidorOld.Recurso = numeroRecurso;

					mapper.Delete(idOcorrenciaServidor);

					AuditDelete(ocorrenciaServidorOld, usuario, trans);

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

                CsvFileLoader<ImportOcorrenciaServidor> loader = new CsvFileLoader<ImportOcorrenciaServidor>(filename);

                List<ImportOcorrenciaServidor> list = loader.Import();

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
                            ServidorMapper servidorMapper = new ServidorMapper();
                            servidorMapper.connection = conn;
                            servidorMapper.transaction = trans;

                            OcorrenciaMapper ocorrenciaMapper = new OcorrenciaMapper();
                            ocorrenciaMapper.connection = conn;
                            ocorrenciaMapper.transaction = trans;

                            FuncaoServidorMapper funcaoServidorMapper = new FuncaoServidorMapper();
                            funcaoServidorMapper.connection = conn;
                            funcaoServidorMapper.transaction = trans;

                            //Validações
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (!servidorMapper.ExisteServidor(list[i].CodServidor))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodServidor'"));
                                else if (funcaoServidorMapper.FindMaxUnidadeAdministrativaByServidor(list[i].CodServidor) == null)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1} {2}", (i + 1), "Unidade administrativa não encontrada para o servidor de matrícula", list[i].CodServidor));

                                if (!ocorrenciaMapper.ExisteOcorrencia(list[i].CodOcorrencia))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodOcorrencia'"));                                

                                if(list[i].DataInicio.Year != usuarioLogado.Ciclo)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Data de início da ocorrência está fora do ciclo atual"));

                                if (list[i].DataFim.Year != usuarioLogado.Ciclo)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Data final da ocorrência está fora do ciclo atual"));

                                if(list[i].DataInicio.CompareTo(list[i].DataFim) > 0)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Data de início da ocorrência é maior que a data final da ocorrência"));
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
                                //Insere ou atualiza os registros
                                //Atualiza status do arquivo para CONCLUÍDO
                                OcorrenciaServidorMapper ocorrenciaServidorMapper = new OcorrenciaServidorMapper();
                                ocorrenciaServidorMapper.connection = conn;
                                ocorrenciaServidorMapper.transaction = trans;

                                //Limpa os registros que possuem ano de referência igual ao cliclo de gestão selecionado
                                ocorrenciaServidorMapper.DeleteAll(usuarioLogado.Ciclo);
                                AuditDeleteOcorrenciaServidor(new OcorrenciaServidor(), usuarioLogado, trans);

                                OcorrenciaServidor ocorrenciaServidor;

                                foreach (var importOcorrenciaServidor in list)
                                {
                                    //Carrega os dados
                                    ocorrenciaServidor = new OcorrenciaServidor();
                                    ocorrenciaServidor.Servidor = new Servidor() { IdServidor = importOcorrenciaServidor.CodServidor };
                                    ocorrenciaServidor.Ocorrencia = new Ocorrencia() { IdOcorrencia = importOcorrenciaServidor.CodOcorrencia };
                                    ocorrenciaServidor.DataInicioOcorrencia = importOcorrenciaServidor.DataInicio;
                                    ocorrenciaServidor.DataFimOcorrencia = importOcorrenciaServidor.DataFim;
                                    ocorrenciaServidor.UnidadeAdministrativa = new UnidadeAdministrativa() { IdUnidadeAdministrativa = funcaoServidorMapper.FindMaxUnidadeAdministrativaByServidor(importOcorrenciaServidor.CodServidor) };

                                    //Insere o registro
                                    ocorrenciaServidor = ocorrenciaServidorMapper.Insert(ocorrenciaServidor);
                                    AuditInsertOcorrenciaServidor(ocorrenciaServidor, usuarioLogado, trans);
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

        private void AuditInsertOcorrenciaServidor(OcorrenciaServidor model, UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = model.GetType().Name;
            logAuditoria.TipoOperacao = OperacaoAuditoria.Inclusao;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            int? idLogAuditoria = logMapper.Insert(logAuditoria);

            LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
            logItemMapper.connection = trans.Connection;
            logItemMapper.transaction = trans;

            LogAuditoriaItem item = new LogAuditoriaItem();

            item.DesAtributo = "IdOcorrenciaServidor";
            item.VlrAtual = model.IdOcorrenciaServidor.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "Servidor";
            item.VlrAtual = model.Servidor.IdServidor.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "Ocorrencia";
            item.VlrAtual = model.Ocorrencia.IdOcorrencia.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DataInicioOcorrencia";
            item.VlrAtual = model.DataInicioOcorrencia.ToString("dd/MM/yyyy");
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DataFimOcorrencia";
            item.VlrAtual = model.DataFimOcorrencia.Value.ToString("dd/MM/yyyy");
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "UnidadeAdministrativa";
            item.VlrAtual = model.UnidadeAdministrativa.IdUnidadeAdministrativa.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DataFimOriginal";
            item.VlrAtual = null;
            logItemMapper.Insert(item, idLogAuditoria.Value);
        }

        private void AuditDeleteOcorrenciaServidor(OcorrenciaServidor model, UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = model.GetType().Name;
            logAuditoria.TipoOperacao = OperacaoAuditoria.Exclusao;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            int? idLogAuditoria = logMapper.Insert(logAuditoria);

            LogAuditoriaItemMapper logItemMapper = new LogAuditoriaItemMapper();
            logItemMapper.connection = trans.Connection;
            logItemMapper.transaction = trans;

            LogAuditoriaItem item = new LogAuditoriaItem();

            item.DesAtributo = "TODOS OS REGISTROS";
            item.VlrAnterior = null;
            item.VlrAtual = null;
            logItemMapper.Insert(item, idLogAuditoria.Value);
        }

		internal OcorrenciaServidor Insert(OcorrenciaServidor ocorrenciaServidor, UserState usuario)
		{
			if (ValidaOcorrenciaServidor(ocorrenciaServidor, usuario))
			{
				using (SqlConnection conn = GetConnection())
				{
					conn.Open();

					using (SqlTransaction trans = conn.BeginTransaction())
					{
						OcorrenciaServidorMapper ocorrenciaServidorMapper = new OcorrenciaServidorMapper();
						ocorrenciaServidorMapper.connection = conn;
						ocorrenciaServidorMapper.transaction = trans;

						FuncaoServidorMapper funcaoServidorMapper = new FuncaoServidorMapper();
						funcaoServidorMapper.connection = conn;
						funcaoServidorMapper.transaction = trans;

						ocorrenciaServidor.UnidadeAdministrativa = new UnidadeAdministrativa();						

						int? idUnidadeAdministrativa = ocorrenciaServidorMapper.FindMaxUnidadeAdministrativaByServidor(ocorrenciaServidor.Servidor.IdServidor);
						
						if( idUnidadeAdministrativa != null )
						{
							ocorrenciaServidor.UnidadeAdministrativa.IdUnidadeAdministrativa = idUnidadeAdministrativa;
						}
						else
						{ 
							ocorrenciaServidor.UnidadeAdministrativa.IdUnidadeAdministrativa = funcaoServidorMapper.FindMaxUnidadeAdministrativaByServidor(ocorrenciaServidor.Servidor.IdServidor) ;
						}

						ocorrenciaServidor = ocorrenciaServidorMapper.Insert(ocorrenciaServidor);

						AuditInsert(ocorrenciaServidor, usuario, trans);

						trans.Commit();
					}
				}
			}

			return ocorrenciaServidor;
		}

		private bool ValidaOcorrenciaServidor(OcorrenciaServidor model, UserState usuarioLogado)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				
				ServidorMapper servidorMapper		= new ServidorMapper();
				OcorrenciaMapper ocorrenciaMapper	= new OcorrenciaMapper();			
				servidorMapper.connection			= conn;
				ocorrenciaMapper.connection			= conn;
				
				if (!servidorMapper.ExisteServidor(model.Servidor.IdServidor))
					modelState.AddModelError("OcorrenciaServidor.Servidor.IdServidor", "Valor inválido");

				if (!ocorrenciaMapper.ExisteOcorrencia(model.Ocorrencia.IdOcorrencia))
					modelState.AddModelError("OcorrenciaServidor.Ocorrencia.IdOcorrencia", "Valor inválido");

				if (model.DataInicioOcorrencia.Year != usuarioLogado.Ciclo)
					modelState.AddModelError("OcorrenciaServidor.DataInicioOcorrencia", "Data de início da ocorrência está fora do ciclo atual");

				if (model.DataFimOcorrencia.HasValue)
				{
					if (model.DataFimOcorrencia.Value.Year != usuarioLogado.Ciclo)
						modelState.AddModelError("OcorrenciaServidor.DataFimOcorrencia", "Data final da ocorrência está fora do ciclo atual");
				}
				else
				{
					modelState.AddModelError("OcorrenciaServidor.DataFimOcorrencia", "Campo Obrigatório");
				}

				if (model.DataInicioOcorrencia.CompareTo(model.DataFimOcorrencia) > 0)
					modelState.AddModelError("OcorrenciaServidor.DataInicioOcorrencia", "Data de início da ocorrência é maior que a data final da função");

				if (String.IsNullOrWhiteSpace(model.Recurso))
					modelState.AddModelError("OcorrenciaServidor.Recurso", "Campo Obrigatório");
			}

			return modelState.IsValid;
		}

		public OcorrenciaServidor PesquisarPor(int idOcorrenciaServidor)
		{
			OcorrenciaServidor ocorrenciaServidor;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				OcorrenciaServidorMapper mapper = new OcorrenciaServidorMapper();
				mapper.connection = conn;

				ocorrenciaServidor = mapper.Find(idOcorrenciaServidor);
			}

			return ocorrenciaServidor;
		}

		public void Update(OcorrenciaServidor ocorrenciaServidor, UserState usuario)
		{
			if (ValidaOcorrenciaServidor(ocorrenciaServidor, usuario))
			{
				using (SqlConnection conn = GetConnection())
				{
					conn.Open();

					using (SqlTransaction trans = conn.BeginTransaction())
					{
						OcorrenciaServidorMapper ocorrenciaServidorMapper = new OcorrenciaServidorMapper();
						ocorrenciaServidorMapper.connection = conn;
						ocorrenciaServidorMapper.transaction = trans;

						OcorrenciaServidor ocorrenciaServidorOld = PesquisarPor(ocorrenciaServidor.IdOcorrenciaServidor);
						ocorrenciaServidor.UnidadeAdministrativa = ocorrenciaServidorOld.UnidadeAdministrativa;

						ocorrenciaServidorMapper.Update(ocorrenciaServidor);

						AuditUpdateRecursoServidor(ocorrenciaServidor, ocorrenciaServidorOld, usuario, trans);

						trans.Commit();
					}
				}
			}
		}

		public void PreencherExtratoAfastamento(FiltroFuncaoOcorrenciaServidor filtro, UserState usuario)
		{
			OcorrenciaServidor ocorrenciaServidor;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				OcorrenciaServidorMapper mapper = new OcorrenciaServidorMapper();
				mapper.connection = conn;

				ocorrenciaServidor = mapper.CalculaTotalAfastamento(filtro.IdServidor.Value, usuario.Ciclo);

				filtro.PercentualTotalAfastamento = ocorrenciaServidor.PercentualTotalAlocado;
				filtro.TotalDiasAfastamento = ocorrenciaServidor.TotalDiasAlocado;
			}
		}

		public void UpdateUnidadeAdministrativaPor(int idServidor, int idUnidadeAdministrativa, UserState usuarioLogado, SqlConnection conn, SqlTransaction trans)
		{
			FiltroOcorrenciaServidor filtro					= new FiltroOcorrenciaServidor { IdServidor = idServidor };
			Paging<OcorrenciaServidor> listaOcorrencias		= List(filtro , 1, Constants.gridPageSize);
			UnidadeAdministrativa novaUnidadeAdministrativa = new UnidadeAdministrativa { IdUnidadeAdministrativa = idUnidadeAdministrativa };

			foreach (OcorrenciaServidor item in listaOcorrencias.Items)
			{
				this.AtualizaUnidadeAdministrativa(item, novaUnidadeAdministrativa, usuarioLogado, conn, trans);
			}
		}

		public void AtualizaUnidadeAdministrativa(OcorrenciaServidor ocorrenciaServidor, UnidadeAdministrativa novaUnidadeAdministrativa, UserState usuario, SqlConnection conn, SqlTransaction trans)
		{
			OcorrenciaServidorMapper ocorrenciaServidorMapper = new OcorrenciaServidorMapper();
			ocorrenciaServidorMapper.connection = conn;
			ocorrenciaServidorMapper.transaction = trans;

			ocorrenciaServidor.UnidadeAdministrativa = novaUnidadeAdministrativa;

			ocorrenciaServidorMapper.Update(ocorrenciaServidor);
		}

		public int? PesquisarUnidadeAdministrativaPor(int idServidor)
		{
			int? idUnidadeAdministrativaOcorrencias;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				OcorrenciaServidorMapper mapper = new OcorrenciaServidorMapper();
				mapper.connection = conn;

				idUnidadeAdministrativaOcorrencias = mapper.FindMaxUnidadeAdministrativaByServidor(idServidor);
			}

			return idUnidadeAdministrativaOcorrencias;
		}
	}
}