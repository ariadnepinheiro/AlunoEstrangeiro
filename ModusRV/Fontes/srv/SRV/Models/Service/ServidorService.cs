using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SRV.Models.Domain;
using System.IO;
using System.Configuration;
using SRV.Common.Loader;
using SRV.Models.DTO;
using SRV.Common.Exceptions;
using SRV.Models.Mapper;
using SRV.Common.Validation;
using SRV.Security;
using SRV.Common;

namespace SRV.Models.Service
{
    public class ServidorService : ArquivoImportacaoService
    {
        public Servidor FindServidor(int idServidor)
        {
            Servidor servidor = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ServidorMapper mapper = new ServidorMapper();
                mapper.connection = conn;

                servidor = mapper.Find(idServidor);
            }

            return servidor;
        }

        public Servidor FindServidor(int idServidor, int? idRegional, int? idUnidadeAdministrativa, int idAnoReferencia, UserState usuario)
        {
            Servidor servidor = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ServidorMapper mapper = new ServidorMapper();
                mapper.connection = conn;

                servidor = mapper.FindServidor(idServidor, idRegional, idUnidadeAdministrativa, idAnoReferencia, usuario);
            }

            return servidor;
        }

        public Paging<Servidor> List(FiltroServidor filtro, int currentPage, int pageSize)
        {
            Paging<Servidor> servidores;

            if (filtro.Cpf != null)
            {
                if (!Cpf.ValidaCpf(filtro.Cpf))
                    throw new BusinessException("Cpf inválido");
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ServidorMapper mapper = new ServidorMapper();
                mapper.connection = conn;

                servidores = mapper.List(filtro, currentPage, pageSize);
            }

            return servidores;
        }

        public PesquisaServidor ListPesquisa(PesquisaServidor pesquisa, int currentPage, int pageSize, UserState usuario)
        {
            Paging<Servidor> servidores;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                ServidorMapper mapper = new ServidorMapper();
                mapper.connection = conn;

                if(pesquisa.Cpf != null)
                {
                    if (!Cpf.ValidaCpf(pesquisa.Cpf))
                        throw new BusinessException("Cpf inválido");
                }

                servidores = mapper.ListPesquisa(pesquisa, currentPage, pageSize, usuario);
            }

            pesquisa.PageServidores = servidores;

            return pesquisa;
        }

        public void Delete(int idServidor, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    ServidorMapper mapper = new ServidorMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    UsuarioMapper usuarioMapper = new UsuarioMapper();
                    usuarioMapper.connection = conn;
                    usuarioMapper.transaction = trans;

                    Usuario usuarioOld = new Usuario();
                    usuarioOld = usuarioMapper.FindByLogin(idServidor.ToString());

                    //Atualiza o usuário
                    usuarioMapper.Delete(usuarioOld.Id.Value);
                    AuditDelete(usuarioOld, usuario, trans);

                    Servidor servidorOld = mapper.Find(idServidor);

                    mapper.Delete(idServidor);

                    AuditDelete(servidorOld, usuario, trans);

                    trans.Commit();
                }
            }
        }

        public void Import(ArquivoImportacao arquivoImportacao, UserState usuarioLogado)
        {
            try
            {
                //Faz a leitura de processamento do arquivo
                string filename = Path.Combine(ConfigurationManager.AppSettings["PathUpload"], arquivoImportacao.DesArquivo);

                CsvFileLoader<ImportServidor> loader = new CsvFileLoader<ImportServidor>(filename);

                List<ImportServidor> list = loader.Import();

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
                            //Insere ou atualiza registros de servidor e usuário associado
                            //Atualiza status do arquivo para CONCLUÍDO
                            ServidorMapper servidorMapper = new ServidorMapper();
                            servidorMapper.connection = conn;
                            servidorMapper.transaction = trans;

                            UsuarioMapper usuarioMapper = new UsuarioMapper();
                            usuarioMapper.connection = conn;
                            usuarioMapper.transaction = trans;

                            Servidor servidor, servidorOld;
                            Usuario usuario, usuarioOld;

                            foreach (var importServidor in list)
                            {
                                //Carrega os dados do servidor
                                servidor = new Servidor();
                                servidor.IdServidor = importServidor.Matricula;
                                servidor.DesNomeServidor = importServidor.Nome.ToUpper();
                                servidor.DesCpfServidor = importServidor.Cpf;
								servidor.IdFuncional = importServidor.IdFuncional;
								servidor.Vinculo = importServidor.Vinculo;

                                //Carrega os dados do usuário
                                usuario = new Usuario();
                                usuario.Login = importServidor.Matricula.ToString();
                                usuario.Nome = importServidor.Nome.ToUpper();
                                usuario.CPF = String.Format(@"{0:00000000000}", importServidor.Cpf);
                                usuario.Senha = Crypt.Encrypt(usuario.CPF);
                                usuario.Perfil = Perfil.Servidor;
                                usuario.Ativo = true;

                                if (servidorMapper.ExisteServidor(importServidor.Matricula))
                                {
                                    servidorOld = servidorMapper.Find(importServidor.Matricula);

                                    usuarioOld = usuarioMapper.FindByLogin(servidor.IdServidor.ToString());
                                    usuarioOld.CPF = Cpf.RetiraMascaraCpf(usuarioOld.CPF);

                                    //Carrega o ID do usuário
                                    usuario.Id = usuarioOld.Id;

                                    usuario.Senha = null; //Valor não alterado (não gravar no log)

                                    servidor.Elegivel = servidorOld.Elegivel; //Valor não alterado (não gravar no log)

                                    if (!servidor.Equals(servidorOld))
                                    {
                                        //Atualiza o servidor
                                        servidorMapper.Update(servidor);

                                        AuditUpdateServidor(servidor, servidorOld, usuarioLogado, trans);
                                    }

                                    if (!usuario.Equals(usuarioOld))
                                    {
                                        //Atualiza o usuário
                                        usuarioMapper.Update(usuario);
                                    }
                                }
                                else
                                {
                                    //Insere o servidor
                                    servidorMapper.Insert(servidor);
                                    AuditInsertServidor(servidor, usuarioLogado, trans);

                                    //Insere o usuário
                                    usuarioMapper.Insert(usuario);

                                    //Limpa a senha para não gravar no log
                                    usuario.Senha = null; //Não gravar no log
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

        private void AuditInsertServidor(Servidor model, UserState usuario, SqlTransaction trans)
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

            item.DesAtributo = "IdServidor";
            item.VlrAtual = model.IdServidor.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "DesNomeServidor";
            item.VlrAtual = model.DesNomeServidor;
            logItemMapper.Insert(item, idLogAuditoria.Value);
            
            item.DesAtributo = "DesCpfServidor";
            item.VlrAtual = String.Format(@"{0:00000000000}", model.DesCpfServidor);
            logItemMapper.Insert(item, idLogAuditoria.Value);

            item.DesAtributo = "Elegivel";
            item.VlrAtual = null;
            logItemMapper.Insert(item, idLogAuditoria.Value);

			item.DesAtributo = "IdFuncional";
			item.VlrAtual = model.IdFuncional;
			logItemMapper.Insert(item, idLogAuditoria.Value);

			item.DesAtributo = "Vinculo";
			item.VlrAtual = model.Vinculo.HasValue ? model.Vinculo.ToString() : String.Empty;
			logItemMapper.Insert(item, idLogAuditoria.Value);

        }

        private void AuditUpdateServidor(Servidor model, Servidor modelOld, UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = model.GetType().Name;
            logAuditoria.TipoOperacao = OperacaoAuditoria.Atualizacao;
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

            item.DesAtributo = "IdServidor";
            item.VlrAnterior = model.IdServidor.ToString();
            item.VlrAtual = model.IdServidor.ToString();
            logItemMapper.Insert(item, idLogAuditoria.Value);

            if ((modelOld.DesNomeServidor != null ? modelOld.DesNomeServidor.ToUpper() : null) != (model.DesNomeServidor != null ? model.DesNomeServidor.ToUpper() : null))
            {
                item.DesAtributo = "DesNomeServidor";
                item.VlrAnterior = modelOld.DesNomeServidor;
                item.VlrAtual = model.DesNomeServidor;
                logItemMapper.Insert(item, idLogAuditoria.Value);
            }

            if (modelOld.DesCpfServidor != model.DesCpfServidor)
            {
                item.DesAtributo = "DesCpfServidor";
                item.VlrAnterior = String.Format(@"{0:00000000000}", modelOld.DesCpfServidor);
                item.VlrAtual = String.Format(@"{0:00000000000}", model.DesCpfServidor);
                logItemMapper.Insert(item, idLogAuditoria.Value);
            }

            if (modelOld.Elegivel != model.Elegivel)
            {
                item.DesAtributo = "Elegivel";
                item.VlrAnterior = modelOld.Elegivel.ToString();
                item.VlrAtual = model.Elegivel.ToString();
                logItemMapper.Insert(item, idLogAuditoria.Value);
            }

			if (modelOld.IdFuncional != model.IdFuncional)
			{
				item.DesAtributo = "IdFuncional";
				item.VlrAnterior = modelOld.IdFuncional;
				item.VlrAtual = model.IdFuncional;
				logItemMapper.Insert(item, idLogAuditoria.Value);
			}

			if (modelOld.Vinculo != model.Vinculo)
			{
				item.DesAtributo = "Vinculo";
				item.VlrAnterior = modelOld.Vinculo.HasValue ? modelOld.Vinculo.ToString() : String.Empty;
				item.VlrAtual = model.Vinculo.HasValue ? model.Vinculo.ToString() : String.Empty;
				logItemMapper.Insert(item, idLogAuditoria.Value);
			}
        }
    }
}