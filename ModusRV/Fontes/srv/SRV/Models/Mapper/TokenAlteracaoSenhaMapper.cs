using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class TokenAlteracaoSenhaMapper : BaseMapper<TokenAlteracaoSenha>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT s.id_token_alteracao_senha, 
                            s.des_token,
                            s.dt_validade,
                            u.id_usuario,
                            u.des_nome_usuario,
                            u.login
                       FROM rv_token_alteracao_senha s,
                            rv_usuario u
                      WHERE s.id_usuario = u.id_usuario
                        AND s.des_token = @desToken";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_token_alteracao_senha
                                (id_usuario,
                                 des_token,
                                 dt_validade)
                          VALUES (@idUsuario,
                                  @desToken,
                                  @dtValidade)";
        }

        public override TokenAlteracaoSenha LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            TokenAlteracaoSenha alteracaoSenha = new TokenAlteracaoSenha();

            alteracaoSenha.IdTokenAlteracaoSenha = Convert.ToInt32(reader["id_token_alteracao_senha"]);
            alteracaoSenha.DesToken = (string)reader["des_token"];
            alteracaoSenha.DtValidade = (DateTime)reader["dt_validade"];

            Usuario usuario = new Usuario();
            usuario.Id = Convert.ToInt32(reader["id_usuario"]);
            usuario.Nome = (string)reader["des_nome_usuario"];
            usuario.Login = (string)reader["login"];

            alteracaoSenha.Usuario = usuario;

            return alteracaoSenha;
        }

        public TokenAlteracaoSenha Find(string token)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("desToken", token);

            return FindObject(param);
        }

        public void Insert(TokenAlteracaoSenha alteracaoSenha)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUsuario", alteracaoSenha.Usuario.Id);
            param.Add("desToken", alteracaoSenha.DesToken);
            param.Add("dtValidade", alteracaoSenha.DtValidade);

            InsertObject(QueryInsert(), param);
        }

    }
}