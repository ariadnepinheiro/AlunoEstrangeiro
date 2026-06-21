using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using SRV.Models.Domain;
using SRV.Models.Mapper;
using System.Web.Mvc;
using SRV.Common.Exceptions;
using SRV.Common.Validation;
using SRV.Security;
using System.Data.SqlClient;

namespace SRV.Models.Service
{
    public class UsuarioService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public UsuarioService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public UsuarioService()
        {

        }

        public UserState AlterarSenha(AlteracaoSenha alteracaoSenha, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string novaSenha = Crypt.Encrypt(alteracaoSenha.NovaSenha);

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    UsuarioMapper usuarioMapper = new UsuarioMapper();
                    usuarioMapper.connection = conn;
                    usuarioMapper.transaction = trans;

                    if (!usuarioMapper.ValidaSenha(usuario.Id, Crypt.Encrypt(alteracaoSenha.SenhaAtual)))
                    {
                        throw new BusinessException("Senha atual não confere");
                    }

                    usuarioMapper.AlterarSenha(usuario.Id, novaSenha);

                    usuario.AlterarSenha = false;

                    AuditAlteracaoSenha(usuario, trans);

                    trans.Commit();
                }
            }
            return usuario;
        }

        public void CriarNovaSenha(NovaSenha novaSenha, UserState usuario)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string novaSenhaCripto = Crypt.Encrypt(novaSenha.Senha);

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    UsuarioMapper usuarioMapper = new UsuarioMapper();
                    usuarioMapper.connection = conn;
                    usuarioMapper.transaction = trans;

                    usuarioMapper.AlterarSenha(usuario.Id, novaSenhaCripto);

                    AuditAlteracaoSenha(usuario, trans);

                    trans.Commit();
                }
            }
        }


        private void AuditAlteracaoSenha(UserState usuario, SqlTransaction trans)
        {
            LogAuditoria logAuditoria = new LogAuditoria();
            logAuditoria.DesObjeto = "Alteração Senha";
            logAuditoria.TipoOperacao = OperacaoAuditoria.AlteracaoSenha;
            logAuditoria.Usuario = new Usuario() { Id = usuario.Id };
            logAuditoria.IpCliente = usuario.IPCliente;

            LogAuditoriaMapper logMapper = new LogAuditoriaMapper();
            logMapper.connection = trans.Connection;
            logMapper.transaction = trans;

            logMapper.Insert(logAuditoria);
        }

        public Usuario Find(int id)
        {
            Usuario usuario;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                UsuarioMapper mapper = new UsuarioMapper();
                mapper.connection = conn;

                usuario = mapper.Find(id);
            }

            return usuario;
        }

        public Usuario FindByLogin(string login)
        {
            Usuario usuario;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                UsuarioMapper mapper = new UsuarioMapper();
                mapper.connection = conn;

                usuario = mapper.FindByLogin(login);
            }

            return usuario;
        }

        private bool ValidaFiltro(FiltroUsuario filtro)
        {

            if (filtro.Matricula == null && filtro.Nome == null && filtro.Perfil == Perfil.Vazio)
                throw new BusinessException("Deve ser informado pelo menos um campo para pesquisa");

            return true;
        }

        public Paging<Usuario> List(FiltroUsuario filtro, int currentPage, int pageSize)
        {

            Paging<Usuario> usuarios = null;
        
            if (ValidaFiltro(filtro))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    UsuarioMapper mapper = new UsuarioMapper();
                    mapper.connection = conn;

                    usuarios = mapper.List(filtro, currentPage, pageSize);
                }
            }

            return usuarios;
        }

        public Paging<Usuario> ListPesquisa(PesquisaUsuario pesquisa, int currentPage, int pageSize)
        {

            Paging<Usuario> usuarios = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                UsuarioMapper mapper = new UsuarioMapper();
                mapper.connection = conn;

                usuarios = mapper.ListPesquisa(pesquisa, currentPage, pageSize);
            }

            return usuarios;
        }

        private bool ValidaUsuario(Usuario usuario)
        {

            if (usuario.Login == null)
                modelState.AddModelError("Usuario.Login", "Campo é obrigatório");

            if (usuario.Nome == null)
                modelState.AddModelError("Usuario.Nome", "Campo é obrigatório");

            if ((usuario.Perfil == Perfil.Administrador || usuario.Perfil == Perfil.Secretaria) && (usuario.EmailUsuario == null))
                modelState.AddModelError("Usuario.EmailUsuario", "Campo é obrigatório");

			if (usuario.CPF == null)
			{
				modelState.AddModelError("Usuario.CPF", "Campo é obrigatório");
			}
			else
			{
				if (Cpf.RetiraMascaraCpf(usuario.CPF).Length > 11)
				{
					modelState.AddModelError("Usuario.CPF", "Campo não pode ter mais de 11 caracteres");
				}
				else if (!Cpf.ValidaCpf(usuario.CPF))
				{
					modelState.AddModelError("Usuario.CPF", "Cpf inválido");
				}
			}

            if (usuario.Perfil == null)
                modelState.AddModelError("Usuario.Perfil", "Campo é obrigatório");

            if (usuario.Perfil != null && usuario.Perfil == Perfil.Regional && !usuario.IdRegional.HasValue)
                modelState.AddModelError("Usuario.IdRegional", "Campo é obrigatório");

            if (usuario.Perfil != null && usuario.Perfil == Perfil.Escola && !usuario.IdUnidadeAdministrativa.HasValue)
                modelState.AddModelError("Usuario.IdUnidadeAdministrativa", "Campo é obrigatório");

            return modelState.IsValid;
        }

        public void Insert(Usuario usuario, UserState usuarioLogado)
        {
            if (ValidaUsuario(usuario))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        UsuarioMapper usuarioMapper = new UsuarioMapper();
                        usuarioMapper.connection = conn;
                        usuarioMapper.transaction = trans;

                        if (usuarioMapper.ValidaMatricula(usuario.Login))
                            throw new BusinessException("Matrícula já existe");

                        if (usuario.Perfil == Perfil.Regional && (!usuario.IdRegional.HasValue))
                            throw new BusinessException("Perfis do tipo Regional precisa informar a sua Regional correspondente.");

                        if (usuario.Perfil == Perfil.Escola && (!usuario.IdUnidadeAdministrativa.HasValue))
                            throw new BusinessException("Perfis do tipo Escola precisa informar a sua Escola correspondente.");

                        //Alterar a senha quando logar
                        usuario.AlterarSenha = true;

                        //Criptografa a Senha
                        usuario.CPF = Cpf.RetiraMascaraCpf(usuario.CPF);
                        usuario.Senha = Crypt.Encrypt(usuario.CPF);

                        usuarioMapper.Insert(usuario);

                        //Limpa a senha para não registrar no log
                        usuario.Senha = null;

                        AuditInsert(usuario, usuarioLogado, trans);

                        trans.Commit();
                    }
                }
            }
        }

        public void Update(Usuario usuario, UserState usuarioLogado)
        {
            if (ValidaUsuario(usuario))
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        UsuarioMapper usuarioMapper = new UsuarioMapper();
                        usuarioMapper.connection = conn;
                        usuarioMapper.transaction = trans;

                        usuario.CPF = Cpf.RetiraMascaraCpf(usuario.CPF);

                        Usuario usuarioOld = Find(usuario.Id.Value);
                        usuarioOld.CPF = Cpf.RetiraMascaraCpf(usuarioOld.CPF);
                        
                        //Limpa a senha para não registrar no log
                        usuarioOld.Senha = null;

                        usuarioMapper.Update(usuario);

                        AuditUpdate(usuario, usuarioOld, usuarioLogado, trans);

                        trans.Commit();
                    }
                }
            }
        }


    }
}