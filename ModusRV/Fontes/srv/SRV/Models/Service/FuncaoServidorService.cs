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

namespace SRV.Models.Service
{
    public class FuncaoServidorService : ArquivoImportacaoService
    {
		private readonly ModelStateDictionary modelState;

		public FuncaoServidorService()
		{		
		}

		public FuncaoServidorService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

		public Paging<FuncaoServidor> List(FiltroFuncaoServidor filtro, int currentPage, int pageSize)
        {
            Paging<FuncaoServidor> result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                FuncaoServidorMapper mapper = new FuncaoServidorMapper();
                mapper.connection = conn;

                result = mapper.List(filtro, currentPage, pageSize);
            }

            return result;
        }

		public void Delete(int idFuncaoServidor, UserState usuario)
		{
			this.Delete(idFuncaoServidor, usuario, null);
		}

        public void Delete(int idFuncaoServidor, UserState usuario,string numeroRecurso)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
				int? idUnidadeAdministrativaOcorrenciaServidor;

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    FuncaoServidorMapper mapper					  = new FuncaoServidorMapper();
					AlocacaoServidorMapper alocacaoServidorMapper = new AlocacaoServidorMapper();

					alocacaoServidorMapper.connection	= conn;
					alocacaoServidorMapper.transaction	= trans;
					mapper.connection					= conn;
                    mapper.transaction					= trans;

					OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService(this.modelState);

                    FuncaoServidor funcaoServidorOld = mapper.Find(idFuncaoServidor);

					if (!String.IsNullOrEmpty(numeroRecurso))
						funcaoServidorOld.Recurso = numeroRecurso;

					alocacaoServidorMapper.DeletePor(idFuncaoServidor);
					mapper.Delete(idFuncaoServidor);

					idUnidadeAdministrativaOcorrenciaServidor = ocorrenciaServidorService.PesquisarUnidadeAdministrativaPor(funcaoServidorOld.Servidor.IdServidor);

					if (funcaoServidorOld.UnidadeAdministrativa.IdUnidadeAdministrativa == idUnidadeAdministrativaOcorrenciaServidor)
					{
						idUnidadeAdministrativaOcorrenciaServidor = mapper.FindMaxUnidadeAdministrativaByServidor(funcaoServidorOld.Servidor.IdServidor);
						if (idUnidadeAdministrativaOcorrenciaServidor.HasValue)
						{
							ocorrenciaServidorService.UpdateUnidadeAdministrativaPor(funcaoServidorOld.Servidor.IdServidor, idUnidadeAdministrativaOcorrenciaServidor.Value, usuario, conn, trans);
						}
					}

					AuditDelete(funcaoServidorOld, usuario, trans);

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

                CsvFileLoader<ImportFuncaoServidor> loader = new CsvFileLoader<ImportFuncaoServidor>(filename);

                List<ImportFuncaoServidor> list = loader.Import();

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

                            FuncaoMapper funcaoMapper = new FuncaoMapper();
                            funcaoMapper.connection = conn;
                            funcaoMapper.transaction = trans;

                            UnidadeAdministrativaMapper unidadeAdministrativaMapper = new UnidadeAdministrativaMapper();
                            unidadeAdministrativaMapper.connection = conn;
                            unidadeAdministrativaMapper.transaction = trans;

                            //Validações
                            for (int i = 0; i < list.Count; i++)
                            {
                                if(list[i].Alocado >= 10000000) // 7 Casas antes da vírgula
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'Alocado'"));

                                if (list[i].Livre >= 10000000) // 7 Casas antes da vírgula
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'Livre'"));

                                if (list[i].Total >= 10000000) // 7 Casas antes da vírgula
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'Total'"));

                                if (!servidorMapper.ExisteServidor(list[i].CodServidor))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodServidor'"));

                                if (!funcaoMapper.ExisteFuncao(list[i].CodFuncao))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodFuncao'"));

                                if (!unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(list[i].CodUnidadeAdministrativa))
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Valor inválido para o campo 'CodUnidadeAdministrativa'"));

                                if (list[i].DataInicio.Year != usuarioLogado.Ciclo)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Data de início da função está fora do ciclo atual"));

                                if (list[i].DataFim.Year != usuarioLogado.Ciclo)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Data final da função está fora do ciclo atual"));

                                if (list[i].DataInicio.CompareTo(list[i].DataFim) > 0)
                                    ErrorRecords.Add(String.Format("Linha {0}: {1}", (i + 1), "Data de início da função é maior que a data final da função"));
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
                                FuncaoServidorMapper funcaoServidorMapper = new FuncaoServidorMapper();
                                funcaoServidorMapper.connection = conn;
                                funcaoServidorMapper.transaction = trans;

                                AlocacaoServidorMapper alocacaoServidorMapper = new AlocacaoServidorMapper();
                                alocacaoServidorMapper.connection = conn;
                                alocacaoServidorMapper.transaction = trans;

                                //Limpa todos os registros de alocação servidor que possuem ano de referência igual ao ciclo de gestão selecionado
                                alocacaoServidorMapper.DeleteAll(usuarioLogado.Ciclo);
                                AuditDeleteAlocacaoServidor(new AlocacaoServidor(), usuarioLogado, trans);

                                //Limpa todos os registros de função servidor que possuem ano de referência igual ao ciclo de gestão selecionado
                                funcaoServidorMapper.DeleteAll(usuarioLogado.Ciclo);
                                AuditDeleteFuncaoServidor(new FuncaoServidor(), usuarioLogado, trans);

                                FuncaoServidor funcaoServidor;

                                foreach (var importFuncaoServidor in list)
                                {
                                    //Carrega os dados
                                    funcaoServidor = new FuncaoServidor();
                                    funcaoServidor.Servidor = new Servidor() { IdServidor = importFuncaoServidor.CodServidor };
                                    funcaoServidor.Funcao = new Funcao() { IdFuncao = importFuncaoServidor.CodFuncao };
                                    funcaoServidor.AnoReferencia = new AnoReferencia() { IdAnoReferencia = usuarioLogado.Ciclo };
                                    funcaoServidor.DataInicioFuncao = importFuncaoServidor.DataInicio;
                                    funcaoServidor.DataFimFuncao = importFuncaoServidor.DataFim;
									funcaoServidor.CargaHorariaAlocada = importFuncaoServidor.Alocado;
									funcaoServidor.CargaHorariaTotal = importFuncaoServidor.Total;
									funcaoServidor.CargaHorariaLivre = importFuncaoServidor.Livre;
                                    funcaoServidor.Proporcionalidade = Decimal.Round(((importFuncaoServidor.Alocado + importFuncaoServidor.Livre) / importFuncaoServidor.Total), 2);
                                    funcaoServidor.UnidadeAdministrativa = new UnidadeAdministrativa() { IdUnidadeAdministrativa = importFuncaoServidor.CodUnidadeAdministrativa };

                                    //Insere o registro
                                    funcaoServidor = funcaoServidorMapper.Insert(funcaoServidor);
                                    AuditInsertFuncaoServidor(funcaoServidor, usuarioLogado, trans);
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

        private void AuditInsertFuncaoServidor(FuncaoServidor model, UserState usuario, SqlTransaction trans)
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

            item.DesAtributo = "IdFuncaoServidor";
            item.VlrAtual = model.IdFuncaoServidor.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "Servidor";
            item.VlrAtual = model.Servidor.IdServidor.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "Funcao";
            item.VlrAtual = model.Funcao.IdFuncao.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "AnoReferencia";
            item.VlrAtual = model.AnoReferencia.IdAnoReferencia.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DataInicioFuncao";
            item.VlrAtual = model.DataInicioFuncao.ToShortDateString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DataFimFuncao";
            item.VlrAtual = model.DataFimFuncao != null ? model.DataFimFuncao.Value.ToString("dd/MM/yyyy") : null;
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "Proporcionalidade";
            item.VlrAtual = model.Proporcionalidade.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "UnidadeAdministrativa";
            item.VlrAtual = model.UnidadeAdministrativa.IdUnidadeAdministrativa.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DataFimOriginal";
            item.VlrAtual = null;
            logItemMapper.Insert(item, idLogAuditoria.Value);
        }

        private void AuditDeleteFuncaoServidor(FuncaoServidor model, UserState usuario, SqlTransaction trans)
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

        private void AuditDeleteAlocacaoServidor(AlocacaoServidor model, UserState usuario, SqlTransaction trans)
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

		public FuncaoServidor Insert(FuncaoServidor funcaoServidor, UserState usuario)			
		{
			if (ValidaFuncaoServidor(funcaoServidor, usuario))
			{
				using (SqlConnection conn = GetConnection())
				{
					conn.Open();

					using (SqlTransaction trans = conn.BeginTransaction())
					{
						FuncaoServidorMapper mapper = new FuncaoServidorMapper();
						mapper.connection = conn;
						mapper.transaction = trans;

						OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService(this.modelState);
						
						funcaoServidor.AnoReferencia = new AnoReferencia { IdAnoReferencia = usuario.Ciclo };
						funcaoServidor = mapper.Insert(funcaoServidor);

						AuditInsert(funcaoServidor, usuario, trans);

						ocorrenciaServidorService.UpdateUnidadeAdministrativaPor(funcaoServidor.Servidor.IdServidor, funcaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, usuario, conn, trans);

						trans.Commit();
					}
				}
			}

			return funcaoServidor;
		}

		private bool ValidaFuncaoServidor(FuncaoServidor model, UserState usuarioLogado)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				ServidorMapper servidorMapper							= new ServidorMapper();			
				FuncaoMapper funcaoMapper								= new FuncaoMapper();
				UnidadeAdministrativaMapper unidadeAdministrativaMapper = new UnidadeAdministrativaMapper();
				servidorMapper.connection								= conn;
				funcaoMapper.connection									= conn;
				unidadeAdministrativaMapper.connection					= conn;

				if (model.CargaHorariaAlocada.HasValue)
				{
					if (model.CargaHorariaAlocada == 0 || model.CargaHorariaAlocada >= 1000000) // 6 Casas antes da vírgula
						modelState.AddModelError("FuncaoServidor.CargaHorariaAlocada", "Valor inválido");					
				}
				else
				{
					modelState.AddModelError("FuncaoServidor.CargaHorariaAlocada", "Campo Obrigatório");
				}

				if (model.CargaHorariaTotal.HasValue)
				{
					if (model.CargaHorariaTotal == 0 || model.CargaHorariaTotal >= 1000000) // 6 Casas antes da vírgula
						modelState.AddModelError("FuncaoServidor.CargaHorariaTotal", "Valor inválido");
				}
				else
				{
					modelState.AddModelError("FuncaoServidor.CargaHorariaTotal", "Campo Obrigatório");
				}

				if (model.CargaHorariaLivre.HasValue)
				{
					if (model.CargaHorariaLivre >= 1000000) // 6 Casas antes da vírgula
						modelState.AddModelError("FuncaoServidor.CargaHorariaLivre", "Valor inválido");
				}
				else
				{
					modelState.AddModelError("FuncaoServidor.CargaHorariaLivre", "Campo Obrigatório");
				}

				if (modelState.IsValid)
				{
					if (model.CargaHorariaAlocada + model.CargaHorariaLivre > model.CargaHorariaTotal)
						modelState.AddModelError("FuncaoServidor.CargaHorariaTotal", "Carga Horária Total menor que o soma da Carga Horária Alocada com a Carga Horária Livre.");
				}

				if (!servidorMapper.ExisteServidor(model.Servidor.IdServidor))
					modelState.AddModelError("FuncaoServidor.Servidor.IdServidor", "Valor inválido");

				if (!funcaoMapper.ExisteFuncao(model.Funcao.IdFuncao))
					modelState.AddModelError("FuncaoServidor.Funcao.IdFuncao", "Valor inválido");

				if (model.UnidadeAdministrativa == null
					|| !model.UnidadeAdministrativa.IdUnidadeAdministrativa.HasValue
					|| !unidadeAdministrativaMapper.ExisteUnidadeAdministrativa(model.UnidadeAdministrativa.IdUnidadeAdministrativa.Value)
				)
				{
					modelState.AddModelError("FuncaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa", "Valor inválido");
				}
				
				if (model.DataInicioFuncao.Year != usuarioLogado.Ciclo)
					modelState.AddModelError("FuncaoServidor.DataInicioFuncao", "Data de início da função está fora do ciclo atual");

				if (model.DataFimFuncao.HasValue)
				{
					if (model.DataFimFuncao.Value.Year != usuarioLogado.Ciclo)
						modelState.AddModelError("FuncaoServidor.DataFimFuncao", "Data final da função está fora do ciclo atual");
				}
				else
				{
					modelState.AddModelError("FuncaoServidor.DataFimFuncao", "Campo Obrigatório");
				}

				if (model.DataInicioFuncao.CompareTo(model.DataFimFuncao) > 0)
					modelState.AddModelError("FuncaoServidor.DataInicioFuncao", "Data de início da função é maior que a data final da função");

				if (String.IsNullOrWhiteSpace(model.Recurso))
					modelState.AddModelError("FuncaoServidor.Recurso", "Campo Obrigatório");
			}

			return modelState.IsValid;
		}

		public FuncaoServidor PesquisarPor(int idFuncaoServidor)
		{
			FuncaoServidor funcaoServidor;

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				FuncaoServidorMapper mapper = new FuncaoServidorMapper();
				mapper.connection = conn;

				funcaoServidor = mapper.Find(idFuncaoServidor);
			}

			return funcaoServidor;
		}

		public void Update(FuncaoServidor funcaoServidor, UserState usuario)
		{
			if (ValidaFuncaoServidor(funcaoServidor, usuario))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
						FuncaoServidorMapper mapper = new FuncaoServidorMapper();
						mapper.connection = conn;
						mapper.transaction = trans;

						OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService(this.modelState);

						FuncaoServidor funcaoServidorOld = PesquisarPor(funcaoServidor.IdFuncaoServidor);

						funcaoServidor.AnoReferencia = new AnoReferencia { IdAnoReferencia = usuario.Ciclo };
                        mapper.Update(funcaoServidor);

						AuditUpdateRecursoServidor(funcaoServidor, funcaoServidorOld, usuario, trans);

						ocorrenciaServidorService.UpdateUnidadeAdministrativaPor(funcaoServidor.Servidor.IdServidor, funcaoServidor.UnidadeAdministrativa.IdUnidadeAdministrativa.Value, usuario, conn, trans);

                        trans.Commit();
                    }
                }
            }
		}

		public void PreencherExtratoAlocacao(FiltroFuncaoOcorrenciaServidor filtro)
		{
			if (filtro.FuncoesServidores != null && filtro.FuncoesServidores.Items.Count > 0)
			{
				FuncaoServidor funcaoServidor;
				using (SqlConnection conn = GetConnection())
				{
					conn.Open();

					FuncaoServidorMapper mapper = new FuncaoServidorMapper();
					mapper.connection = conn;

					funcaoServidor = mapper.CalculaTotalAlocacao(filtro.FuncoesServidores.Items.FirstOrDefault());

					filtro.PercentualTotalAlocado = funcaoServidor.PercentualTotalAlocado;
					filtro.TotalDiasAlocado = funcaoServidor.TotalDiasAlocado;
				}
			}
		}
	}
}