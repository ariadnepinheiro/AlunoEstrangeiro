using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Common.Exceptions;
using SRV.Common;
using System.Web.Mvc;

namespace SRV.Models.Service
{
    public class TokenAlteracaoSenhaService : BaseService
    {
        public TokenAlteracaoSenha Find(string token)
        {
            TokenAlteracaoSenha result;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                TokenAlteracaoSenhaMapper mapper = new TokenAlteracaoSenhaMapper();
                mapper.connection = conn;

                result = mapper.Find(token);

                if (result == null)
                    throw new BusinessException("Link inválido");

                if (result.DtValidade < DateTime.Now)
                    throw new BusinessException("Link expirado. Faça nova solicitação");
            }

            return result;
        }

        public TokenAlteracaoSenha Insert(string login)
        {
            TokenAlteracaoSenha alteracaoSenha = null;

            if (login == null || String.IsNullOrWhiteSpace(login))
                throw new BusinessException("Matrícula é obrigatória");

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    UsuarioMapper usuarioMapper = new UsuarioMapper();
                    usuarioMapper.connection = conn;
                    usuarioMapper.transaction = trans;

                    Usuario usuario = usuarioMapper.FindByLogin(login);

                    if (usuario == null)
                        throw new BusinessException("Usuário não encontrado");

                    if (!usuario.Ativo)
                        throw new BusinessException("Usuário não está ativo");

                    if(usuario.Perfil != Perfil.Administrador && usuario.Perfil != Perfil.Secretaria)
                        throw new BusinessException("Sua senha é o número de seu CPF");

                    alteracaoSenha = new TokenAlteracaoSenha();

                    alteracaoSenha.Usuario = usuario;
                    alteracaoSenha.DesToken = CreateToken();
                    alteracaoSenha.DtValidade = DateTime.Now.AddHours(Constants.horasValidadeTokenSenha);

                    TokenAlteracaoSenhaMapper mapper = new TokenAlteracaoSenhaMapper();
                    mapper.connection = conn;
                    mapper.transaction = trans;

                    mapper.Insert(alteracaoSenha);

                    trans.Commit();
                }
            }

            return alteracaoSenha;
        }

        private string CreateToken()
        {
            return Guid.NewGuid().ToString();
        }

    }
}