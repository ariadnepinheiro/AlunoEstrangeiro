using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SRV.Models.DTO;
using SRV.Models.Domain;
using SRV.Common;
using SRV.Common.Extension;
using System.Configuration;

namespace SRV.Models.Mapper
{
    public class UsuarioMapper : BaseMapper<Usuario>
    {
        protected override string QueryFindObject()
        {
            return @"SELECT id_usuario, 
                            login, 
                            des_nome_usuario, 
                            des_email_usuario, 
                            id_perfil, 
                            fl_ativo, 
                            dbo.fc_grid_formatar_cpf(cast(des_cpf_usuario as varchar)) as cpf,
                            fl_alterar_senha ,
                            ID_REGIONAL,
                            ID_UNIDADE_ADMINISTRATIVA
                       FROM rv_usuario
                      WHERE id_usuario = @idUsuario";
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryFindByLogin()
        {
            return @"SELECT id_usuario, 
                            login, 
                            des_nome_usuario, 
                            des_email_usuario, 
                            id_perfil, 
                            fl_ativo, 
                            dbo.fc_grid_formatar_cpf(cast(des_cpf_usuario as varchar)) as cpf,
                            fl_alterar_senha ,
                            ID_REGIONAL,
                            ID_UNIDADE_ADMINISTRATIVA
                       FROM rv_usuario
                      WHERE login = @login";
        }

        private string QueryAlterarSenha()
        {
            return @"UPDATE rv_usuario 
                        SET senha = @senha,
                            fl_alterar_senha = 'N'
                      WHERE id_usuario = @idUsuario";
        }

        private string QueryValidaSenha()
        {
            return @"SELECT COUNT(*) 
                       FROM rv_usuario 
                      WHERE id_usuario = @idUsuario
                        AND senha = @senha";
        }

        private string QueryListUsuario(FiltroUsuario filtro)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT id_usuario, 
                            login, 
                            des_nome_usuario, 
                            des_email_usuario, 
                            id_perfil, 
                            fl_ativo, 
                            dbo.fc_grid_formatar_cpf(cast(des_cpf_usuario as varchar)) as cpf,
                            fl_alterar_senha,
                            ID_REGIONAL,
                            ID_UNIDADE_ADMINISTRATIVA
                       FROM rv_usuario
                      WHERE 1 = 1 ");

            if (filtro.Nome != null)
                sql.Append(" AND UPPER(des_nome_usuario) LIKE @desNomeUsuario ");

            if (filtro.Matricula != null)
                sql.Append(" AND login = @login ");

            if (filtro.Perfil != Perfil.Vazio)
                sql.Append(" AND id_perfil = @id_perfil ");

            sql.Append(" ORDER BY des_nome_usuario, login");

            return sql.ToString();
        }

        private string QueryListPesquisaUsuario(PesquisaUsuario pesquisa)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT id_usuario, 
                            login, 
                            des_nome_usuario, 
                            des_email_usuario, 
                            id_perfil, 
                            fl_ativo, 
                            dbo.fc_grid_formatar_cpf(cast(des_cpf_usuario as varchar)) as cpf,
                            fl_alterar_senha,
                            ID_REGIONAL,
                            ID_UNIDADE_ADMINISTRATIVA
                       FROM rv_usuario
                      WHERE 1 = 1 ");

            if (pesquisa.Nome != null)
                sql.Append(" AND UPPER(des_nome_usuario) LIKE @desNomeUsuario ");

            if (pesquisa.Matricula != null)
                sql.Append(" AND login = @login ");

            sql.Append(" ORDER BY des_nome_usuario, login");

            return sql.ToString();
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_usuario 
                                (login,
                                 des_nome_usuario, 
                                 des_email_usuario, 
                                 id_perfil, 
                                 fl_ativo, 
                                 des_cpf_usuario,
                                 fl_alterar_senha,
                                 senha,
                                 ID_REGIONAL,
                                 ID_UNIDADE_ADMINISTRATIVA)
                         VALUES (@login,
                                 @nome, 
                                 @email, 
                                 @perfil, 
                                 @ativo, 
                                 @cpf,
                                 @fl_alterar_senha,
                                 @senha,
                                @ID_REGIONAL,
                                @ID_UNIDADE_ADMINISTRATIVA)";
        }

        private string QueryUpdate()
        {
            return @"UPDATE rv_usuario 
                        SET des_nome_usuario = @nome,
                            des_email_usuario = @email,
                            id_perfil = @perfil,
                            fl_ativo = @ativo,
                            des_cpf_usuario = @cpf,
                            ID_REGIONAL = @ID_REGIONAL,
                            ID_UNIDADE_ADMINISTRATIVA = @ID_UNIDADE_ADMINISTRATIVA
                      WHERE id_usuario = @id";
        }

        private string QueryDelete()
        {
            return @"DELETE FROM rv_usuario 
                           WHERE id_usuario = @id";
        }

        private string QueryValidaMatricula()
        {
            return @"SELECT COUNT(*) 
                       FROM rv_usuario 
                      WHERE login = @matricula";
        }

        public override Usuario LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            Usuario usuario = new Usuario();

            usuario.Id = Convert.ToInt32(reader["id_usuario"]);
            usuario.Login = (string)reader["login"];
            usuario.Nome = (string)reader["des_nome_usuario"];

            if(reader["des_email_usuario"] != DBNull.Value)
                usuario.EmailUsuario = (string)reader["des_email_usuario"];

            if (reader["cpf"] != DBNull.Value)
                usuario.CPF = (string)reader["cpf"];
            if (reader["ID_REGIONAL"] != DBNull.Value)
                usuario.IdRegional = (int)reader["ID_REGIONAL"];
            if (reader["ID_UNIDADE_ADMINISTRATIVA"] != DBNull.Value)
                usuario.IdUnidadeAdministrativa = (int)reader["ID_UNIDADE_ADMINISTRATIVA"];

            usuario.AlterarSenha = reader["fl_alterar_senha"].Equals("S") ? true : false;
            usuario.Ativo = reader["fl_ativo"].Equals("S") ? true : false;
            usuario.Perfil = (Perfil)Enum.ToObject(typeof(Perfil), Convert.ToInt32(reader["id_perfil"]));

            return usuario;
        }

        public Usuario Find(int id)
        {
            return FindObject("idUsuario", id);
        }

        public Usuario FindByLogin(string login)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("login", login);

            return FindObject(QueryFindByLogin(), param);
        }

        public void AlterarSenha(int idUsuario, string senha)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idUsuario", idUsuario);
            param.Add("senha", senha);

            UpdateObject(QueryAlterarSenha(), param);
        }

        public bool ValidaSenha(int idUsuario, string senha)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("idUsuario", idUsuario);
            param.Add("senha", senha);

            short count = Convert.ToInt16(ExecuteScalarQuery(QueryValidaSenha(), param));

            return count > 0 ? true : false;
        }

        public Paging<Usuario> List(FiltroUsuario filtro, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (filtro.Nome != null)
                param.Add("desNomeUsuario", String.Concat("%", filtro.Nome.ToUpper(), "%"));

            if (filtro.Matricula != null)
                param.Add("login", filtro.Matricula);

            if (filtro.Perfil != Perfil.Vazio)
                param.Add("id_perfil", filtro.Perfil);

            return ListPagingObjects(QueryListUsuario(filtro), param, currentPage, pageSize);
        }

        public Paging<Usuario> ListPesquisa(PesquisaUsuario pesquisa, int currentPage, int pageSize)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            if (pesquisa.Nome != null)
                param.Add("desNomeUsuario", String.Concat("%", pesquisa.Nome.ToUpper(), "%"));

            if (pesquisa.Matricula != null)
                param.Add("login", pesquisa.Matricula);

            return ListPagingObjects(QueryListPesquisaUsuario(pesquisa), param, currentPage, pageSize);
        }

        public Usuario Insert(Usuario usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("login", usuario.Login.ToUpper());
            param.Add("nome", usuario.Nome.ToUpper());
            param.Add("email", usuario.EmailUsuario);
            param.Add("perfil", usuario.Perfil);
            param.Add("ativo", usuario.Ativo ? "S" : "N");
            param.Add("cpf", Convert.ToInt64(usuario.CPF));            
            param.Add("fl_alterar_senha", usuario.AlterarSenha ? "S" : "N");
            param.Add("senha", usuario.Senha);
            param.Add("ID_REGIONAL", usuario.IdRegional);
            param.Add("ID_UNIDADE_ADMINISTRATIVA", usuario.IdUnidadeAdministrativa);

            usuario.Id = InsertObjectWithIdentity(QueryInsert(), param);

            return usuario;
        }

        public void Update(Usuario usuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("id", usuario.Id);
            param.Add("nome", usuario.Nome.ToUpper());
            param.Add("email", usuario.EmailUsuario);
            param.Add("perfil", usuario.Perfil);
            param.Add("ativo", usuario.Ativo ? "S" : "N");
            param.Add("cpf", Convert.ToInt64(usuario.CPF));
            param.Add("ID_REGIONAL", usuario.IdRegional);
            param.Add("ID_UNIDADE_ADMINISTRATIVA", usuario.IdUnidadeAdministrativa);

            UpdateObject(QueryUpdate(), param);
        }

        public void Delete(int idUsuario)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("id", idUsuario);

            DeleteObject(QueryDelete(), param);
        }

        public bool ValidaMatricula(string matricula)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("matricula", matricula);

            short cont = Convert.ToInt16(ExecuteScalarQuery(QueryValidaMatricula(), param));

            return cont > 0 ? true : false;
        }

    }
}