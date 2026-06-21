using System.Collections.Generic;
using System.Data.SqlClient;
using SRV.Models.DTO;
using System;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class LoginMapper : BaseMapper<UserState>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT * 
                       FROM rv_usuario 
                      WHERE login = @login 
                        AND senha = @senha
                        AND fl_ativo = 'S'";
        }

        protected override string QueryListObjects()
        {
            return "";
        }

        public override UserState LoadObject(SqlDataReader reader)
        {
            UserState usuario = new UserState();

            usuario.Id = Convert.ToInt32(reader["id_usuario"]);
            usuario.Login = (string)reader["login"];
            usuario.Nome = (string)reader["des_nome_usuario"];
            usuario.Perfil = (Perfil)Enum.ToObject(typeof(Perfil), Convert.ToInt32(reader["id_perfil"]));
            usuario.AlterarSenha = "S".Equals(reader["fl_alterar_senha"]) ? true : false;

            return usuario;
        }

        public UserState Login(string noLogin, string senha)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("login", noLogin.ToUpper());
            param.Add("senha", senha);

            return FindObject(param);
        }


    }
}