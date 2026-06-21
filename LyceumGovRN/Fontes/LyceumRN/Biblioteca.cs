using Techne.Data;
using Techne.Lyceum.CR;
using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using Techne.Library;

namespace Techne.Lyceum.RN
{
    public class Biblioteca : RNBase
    {
        #region Tela Biblioteca
        public static QueryTable ConsultarEndereço(string unidadeFis)
        {
            string sql =
                @"SELECT ENDERECO,END_NUM,END_COMPL,BAIRRO,MUNICIPIO,CEP
                FROM LY_UNIDADE_FISICA 
                WHERE UNIDADE_FIS = ? ";
            return Consultar(sql, unidadeFis);
        }

        //Consultar
        public static Ly_bib_biblioteca.Row ConsultarUnidade(string biblioteca_id)
        {
            Ly_bib_biblioteca.Row consulta = Ly_bib_biblioteca.QueryFirstRow(Config.CreateWritableConnection(), "id = ?", biblioteca_id);
            return consulta;
        }

        //Incluir Biblioteca
        public static RetValue Incluir(ref Ly_bib_biblioteca.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    linha = Ly_bib_biblioteca.Row.Insert(connection, linha.Nome_bib, linha.Resp_bib, linha.Horario_func, linha.Tipo_bib, linha.Unid_fisica, linha.Dependencia, "Fone_bib, Fax_bib, Email", linha.Fone_bib, linha.Fax_bib, linha.Email);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    //inclui localização padrão da biblioteca

                    Ly_bib_localizacao.Row.Insert(connection, linha.Id, "DESCR_LOCAL, SIGLA_LOCAL", "Biblioteca", "Biblioteca");
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }

                    retorno = new RetValue(true, "Biblioteca incluída com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Alterar Binlioteca
        public static RetValue Alterar(Ly_bib_biblioteca.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    Ly_bib_biblioteca.Row.Update(connection, linha.Id, "Nome_bib, Resp_bib, Horario_func, Tipo_bib, Unid_fisica, Dependencia, Fone_bib, Fax_bib, Email", linha.Nome_bib, linha.Resp_bib, linha.Horario_func, linha.Tipo_bib, linha.Unid_fisica, linha.Dependencia, linha.Fone_bib, linha.Fax_bib, linha.Email);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Biblioteca alterada com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static decimal ObterLocalPadrao(decimal? id_biblioteca)
        {
            string sql = "select id from LY_BIB_LOCALIZACAO where id_bib_biblioteca = ?";
            string id = ConsultarCampo(sql, id_biblioteca);
            if (!string.IsNullOrEmpty(id))
                return Convert.ToDecimal(id);
            return 0M;
        }

        //Excluir Biblioteca
        public static RetValue Excluir(Ly_bib_biblioteca.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                //exclui local padrão
                decimal id_local = ObterLocalPadrao(linha.Id);
                if (id_local != 0)
                {
                    Ly_bib_localizacao.Row.Delete(connection, id_local);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return new RetValue(false, null, new ErrorList("Existem materiais cadastrados para esta biblioteca.")); ;
                    }
                }

                Ly_bib_biblioteca.Row.Delete(connection, linha.Id);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Biblioteca excluída com sucesso.", null);

            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Inlcuir pessoa e usuário da biblioteca
        public static RetValue IncluirPessoaUsuario(Ly_pessoa dtPessoa, Ly_bib_usuario.Row usuario)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                retorno = IncluirPessoa(connection, dtPessoa);
                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
                usuario.Pessoa = dtPessoa.Rows[0].Pessoa;
                retorno = IncluirUsuarioBib(connection, usuario);
                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Usuário incluído com sucesso.", null);
        }

        //Inlcuir Pessoa
        public static RetValue IncluirPessoa(TConnectionWritable connection, Ly_pessoa dtPessoa)
        {
            RetValue retorno = null;
            dtPessoa.Rows[0].Pessoa = Pessoa.GeraPessoa();
            if (dtPessoa != null)
            {
                if (dtPessoa.Rows != null)
                {
                    ColunasTable colunas = MontarParametros(dtPessoa.Columns, dtPessoa.Rows[0]);
                    string ret = null; // RN.Pessoa.PreInsertRESP(dtPessoa.Rows[0], connection); //Retirado, tela não existe mais
                    if (!string.IsNullOrEmpty(ret))
                    {
                        connection.Rollback();
                        return new RetValue(false, null, new ErrorList(ret));
                    }
                    Ly_pessoa.Row.Insert(connection, dtPessoa.Rows[0].Pessoa, dtPessoa.Rows[0].Nome_compl, dtPessoa.Rows[0].Endereco, dtPessoa.Rows[0].End_num, dtPessoa.Rows[0].End_municipio, dtPessoa.Rows[0].Cep, colunas.Colunas, colunas.ValorColuna);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                }
            }
            return retorno;
        }

        //Incluir usuário após pessoa
        public static RetValue IncluirUsuarioBib(TConnectionWritable connection, Ly_bib_usuario.Row linha)
        {
            RetValue retorno = null;
            if (linha != null)
            {
                Ly_bib_usuario.Row.Insert(connection, linha.Pessoa, linha.Id_bib_biblioteca, linha.Tipo);

                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }
            }
            return retorno;
        }

        //incluir apenas usuário para pessoa que já existe
        public static RetValue IncluirUsuarioBib(Ly_bib_usuario.Row linha)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                //verifica se usuário já está na biblioteca
                if (!VerificaExisteUsuarioBib(connection, linha.Pessoa, linha.Id_bib_biblioteca))
                {
                    if (linha != null)
                    {
                        Ly_bib_usuario.Row.Insert(connection, linha.Pessoa, linha.Id_bib_biblioteca, linha.Tipo);

                        retorno = VerificarErro(connection.GetErrors());
                        if (retorno != null)
                        {
                            connection.Rollback();
                            return retorno;
                        }
                    }
                }
                else
                {
                    return new RetValue(false, null, new Techne.Library.ErrorList("Usuário já cadastro nesta biblioteca."));
                }
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Usuário incluído com sucesso.", null);
        }

        public static bool VerificaExisteUsuarioBib(TConnectionWritable connection, decimal? pessoa, decimal? id_bib)
        {
            string sql = "select 1 from ly_bib_usuario where pessoa = ? and id_bib_biblioteca = ?";

            int retorno = ExecutarFuncao(sql, connection, pessoa, id_bib);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool VerificaExisteUsuarioBib(string pessoa, decimal? id_bib)
        {
            string sql = "select 1 from ly_bib_usuario where pessoa = ? and id_bib_biblioteca = ?";

            int retorno = ExecutarFuncao(sql, pessoa, id_bib);

            if (retorno == 1)
                return true;
            else
                return false;
        }
        #endregion

        #region Tela Registrar Material

        //verifica se existe código de barras
        public static bool VerificaCodBarras(string cod_barras)
        {
            string sql = @"select top 1 1 from ly_bib_material where COD_BARRAS_MATERIAL = ?";
            int retorno = ExecutarFuncao(sql, cod_barras);
            if (retorno == 1)
                return true;
            else
                return false;
        }


        //verifica se existe código de barras
        public static bool VerificaCodBarras(string cod_barras, string id)
        {
            string sql = @"select top 1 1 from ly_bib_material where COD_BARRAS_MATERIAL = ? and id <> ?";
            int retorno = ExecutarFuncao(sql, cod_barras, id);
            if (retorno == 1)
                return true;
            else
                return false;
        }

        //Incluir material biblioteca
        public static RetValue IncluirMaterialBib(Ly_bib_material.Row linha)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_bib_material.Row.Insert(connection, linha.Cod_barras_material, linha.Num_registro, "Id_bib_titulo, Id_bib_localizacao", linha.Id_bib_titulo, linha.Id_bib_localizacao);

                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Exemplar incluído com sucesso.", null);
        }

        //Incluir titulo
        public static RetValue IncluirTituloProvisorio(ref Ly_bib_titulo.Row linha_titulo, Ly_bib_material.Row linha_material)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                linha_titulo = Ly_bib_titulo.Row.Insert(connection, "Isbn, Titulo", linha_titulo.Isbn, linha_titulo.Titulo);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }

                Ly_bib_material.Row.Insert(connection, linha_material.Cod_barras_material, linha_material.Num_registro, "Id_bib_titulo, Id_bib_localizacao", linha_material.Id_bib_titulo, linha_material.Id_bib_localizacao);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Título incluído com sucesso.", null);
        }

        //Consultar Titulo
        public static QueryTable ConsultarTitulo(string titulo_id)
        {
            string sql = "SELECT ID,TITULO,ISBN,EDICAO,ANO,AREA,TEMA,GENERO,ID_BIB_EDITORA,ID_BIB_TIPO_MATERIAL,ID_BIB_ASSUNTO_CDD FROM Ly_bib_titulo WHERE id = ?";
            return Consultar(sql, titulo_id);
        }

        public static byte[] ConsultarImagemTitulo(string titulo_id)
        {
            QueryTable qt = new QueryTable("SELECT IMAGEM FROM Ly_bib_titulo WHERE id = ?");
            qt.Query(Config.CreateConnection(), titulo_id);

            try
            {
                if (qt.Rows.Count > 0)
                {
                    return (byte[])qt.Rows[0]["imagem"];
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        //Consultar Titulo
        public static QueryTable ConsultarMaterial(string material_id)
        {
            string sql = @"";
            return Consultar(sql, material_id);
        }

        //Incluir Título
        public static RetValue IncluirTitulo(ref decimal? Codigo, string Titulo, decimal Isbn, string Edicao, decimal Ano, string Area, string Tema, string Genero, decimal Id_bib_editora, decimal Id_bib_tipo_material, decimal Id_bib_assunto_cdd, byte[] Imagem)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Ly_bib_titulo.Row linha = Ly_bib_titulo.Row.Insert(connection, "Titulo, Isbn, Edicao, Ano, Area, Tema, Genero, Id_bib_editora, Id_bib_tipo_material, Id_bib_assunto_cdd, Imagem", Titulo, Isbn, Edicao, Ano, Area, Tema, Genero, Id_bib_editora, Id_bib_tipo_material, Id_bib_assunto_cdd, Imagem);

                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                Codigo = linha.Id;
                retorno = new RetValue(true, "Título incluído com sucesso.", null);
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Alterar Título
        public static RetValue AlterarTitulo(decimal? Codigo, string Titulo, decimal Isbn, string Edicao, decimal Ano, string Area, string Tema, string Genero, decimal Id_bib_editora, decimal Id_bib_tipo_material, decimal Id_bib_assunto_cdd, byte[] Imagem)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Ly_bib_titulo.Row.Update(connection, Codigo, "Titulo, Isbn, Edicao, Ano, Area, Tema, Genero, Id_bib_editora, Id_bib_tipo_material, Id_bib_assunto_cdd, Imagem", Titulo, Isbn, Edicao, Ano, Area, Tema, Genero, Id_bib_editora, Id_bib_tipo_material, Id_bib_assunto_cdd, Imagem);

                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Título alterado com sucesso.", null);
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Excluir Título
        public static RetValue ExcluirTitulo(Ly_bib_titulo.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Ly_bib_titulo.Row.Delete(connection, linha.Id);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Título excluído com sucesso.", null);

            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //consultar tipos de materiais
        public static QueryTable ConsultarTipoMaterial()
        {
            return Consultar("select id, sigla from ly_bib_tipo_material order by sigla");
        }

        //consultar tipos de materiais
        public static QueryTable ConsultarCDD()
        {
            return Consultar("select id, codigo_cdd + ' - ' + DESCRICAO_CDD as descricao from LY_BIB_ASSUNTO_CDD order by codigo_cdd");
        }


        //Gera código de ordem (chave) a partir do último do banco
        public static decimal GeraNum_Registro(string id_bib)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            decimal ordem;
            DbObject dbordem;

            try
            {
                dbordem = TCommand.ExecuteScalar(connection, @"select max(num_registro) from ly_bib_material m
                                                                INNER JOIN LY_BIB_LOCALIZACAO l on l.ID = m.ID_BIB_LOCALIZACAO 
                                                                where id_bib_biblioteca = ?", id_bib);
            }
            finally
            {
                connection.Close();
            }
            if (!dbordem.IsNull)
            {
                ordem = (decimal)dbordem;
                return ordem + 1;
            }
            return 1;
        }

        public static bool VerificaExisteIntervaloNum_registro(decimal inicio, decimal fim, string id_bib)
        {
            string sql = @"select 1 from LY_BIB_MATERIAL bm
                        where bm.NUM_REGISTRO >= ? and  bm.NUM_REGISTRO <= ?
                        and exists (select 1 from  LY_BIB_LOCALIZACAO bl where bl.ID_BIB_BIBLIOTECA = ? and bl.ID = bm.ID_BIB_LOCALIZACAO)";

            int retorno = ExecutarFuncao(sql, inicio, fim, id_bib);

            if (retorno == 1)
                return true;
            else
                return false;
        }
        #endregion

        public static QueryTable PesquisaLivro(string chaves)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select t.id, 
                        t.titulo, 
                        dbo.fn_Autores(t.ID) as nome_autor,
                        ed.NOME_EDITORA as editora,
                        t.imagem
                        from LY_BIB_TITULO t
                        left join LY_BIB_TIPO_MATERIAL tm on tm.ID = t.ID_BIB_TIPO_MATERIAL
                        left join LY_BIB_ASSUNTO_CDD acd on acd.ID = t.ID_BIB_ASSUNTO_CDD
                        left join LY_BIB_EDITORA ed on ed.ID = t.ID_BIB_EDITORA");

            sb.Append(" WHERE 1 = 1");
            string[] novas_chaves = chaves.Split();
            foreach (string s in novas_chaves)
            {
                if (s.Length > 2)
                    sb.Append(" and (t.titulo + ' ' + dbo.fn_Autores(t.ID)  + ' ' + ed.NOME_EDITORA) like '%" + s + "%'");
            }

            //verifica se tem usuario logado e filtra os títulos da biblioteca dele apenas

            string usuario = HttpContext.Current.User.Identity.Name;

            if (!string.IsNullOrEmpty(usuario))
            {
                sb.Append(@" and t.id in 
                        (select m.ID_BIB_TITULO from LY_BIB_MATERIAL m 
                        inner join LY_BIB_LOCALIZACAO l on m.ID_BIB_LOCALIZACAO = l.ID
                        inner join LY_BIB_BIBLIOTECA b on l.ID_BIB_BIBLIOTECA = b.ID
                        inner join LY_BIB_USUARIO u on u.ID_BIB_BIBLIOTECA = b.ID
                        where u.PESSOA = " + usuario + ")");
            }

            return RNBase.Consultar(sb.ToString());
        }

        public static QueryTable PesquisaAvancadaLivro(List<string> tipos, bool disponiveis, object biblioteca, string titulo, string editora, string autor, string assunto)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Select t.id, t.titulo, 
                        dbo.fn_Autores(t.ID) nome_autor,
                        t.imagem,
                        NOME_EDITORA as editora
                        from LY_BIB_TITULO t
                        left join LY_BIB_TIPO_MATERIAL tm on tm.ID = t.ID_BIB_TIPO_MATERIAL
                        left join LY_BIB_ASSUNTO_CDD acd on acd.ID = t.ID_BIB_ASSUNTO_CDD
                        left join LY_BIB_EDITORA ed on ed.ID = t.ID_BIB_EDITORA
                        where 1 = 1 ");

            if (biblioteca != null && biblioteca.ToString() != string.Empty)
                sb.Append(@" and exists 
                (select top 1 1 from LY_BIB_MATERIAL m where m.ID_BIB_TITULO = t.ID
                        and m.ID_BIB_LOCALIZACAO in 
                                (select ID from LY_BIB_LOCALIZACAO where ID_BIB_BIBLIOTECA = " + biblioteca + " ))");

            if (disponiveis)
            {
                sb.Append(@" and exists (select * from LY_BIB_MATERIAL m where m.ID_BIB_TITULO = t.ID
			                        and not exists 
					                (select top 1 1 from LY_BIB_EMPRESTIMO emp
					                where emp.ID_BIB_MATERIAL = m.ID
					                and emp.DT_DEVOLUCAO is null))");
            }
            if (tipos != null)
            {
                if (tipos.Count > 0)
                {
                    int i = 0;
                    foreach (string s in tipos)
                    {
                        if (i == 0)
                        {
                            sb.Append(" and (tm.ID = " + s);
                            i++;
                        }
                        else
                        {
                            sb.Append(" or tm.ID = " + s);
                        }
                    }
                    sb.Append(" )");
                }
            }


            if (titulo != string.Empty)
                sb.Append(AdicionaFiltrosWhere(titulo, "t.titulo"));
            if (assunto != string.Empty)
                sb.Append(AdicionaFiltrosWhere(assunto, "acd.descricao_cdd"));
            if (editora != string.Empty)
                sb.Append(AdicionaFiltrosWhere(editora, "ed.nome_editora"));
            if (autor != string.Empty)
                sb.Append(AdicionaFiltrosWhere(autor, "dbo.fn_Autores(t.ID)"));

            //verifica se tem usuario logado e filtra os títulos da biblioteca dele apenas

            string usuario = HttpContext.Current.User.Identity.Name;

            if (!string.IsNullOrEmpty(usuario))
            {
                sb.Append(@" and t.id in 
                        (select m.ID_BIB_TITULO from LY_BIB_MATERIAL m 
                        inner join LY_BIB_LOCALIZACAO l on m.ID_BIB_LOCALIZACAO = l.ID
                        inner join LY_BIB_BIBLIOTECA b on l.ID_BIB_BIBLIOTECA = b.ID
                        inner join LY_BIB_USUARIO u on u.ID_BIB_BIBLIOTECA = b.ID
                        where u.PESSOA = " + usuario + ")");
            }

            return RNBase.Consultar(sb.ToString());
        }

        private static string AdicionaFiltrosWhere(string valor, string campo)
        {
            StringBuilder sb = new StringBuilder();
            if (valor != string.Empty)
            {
                sb.Append(" and ( ");
                for (int i = 0; i < valor.Split('|').Length; i++)
                {
                    if (i > 0)
                        sb.Append(" or ");
                    sb.Append(campo + " like '%" + valor.Split('|')[i] + "%'");

                }
                sb.Append(" ) ");
            }
            return sb.ToString();
        }

        //Incluir Empréstimo
        public static RetValue EfetuarEmprestimo(ref Ly_bib_emprestimo.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    linha = Ly_bib_emprestimo.Row.Insert(connection, linha.Id_bib_usuario, linha.Id_bib_material, linha.Dt_emprestimo, linha.Dt_prev_devolucao);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Empréstimo efetuado com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Alterar data devolução  Empréstimo
        public static RetValue AlterarEmprestimo(Ly_bib_emprestimo.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    linha = Ly_bib_emprestimo.Row.Update(connection, linha.Id, "Dt_devolucao", linha.Dt_devolucao);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Empréstimo alterado com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Consultar reservas
        public static Ly_bib_reserva.Row ConsultarReserva(string material_id)
        {
            Ly_bib_reserva.Row consulta = Ly_bib_reserva.QueryFirstRow(Config.CreateWritableConnection(), "id_bib_material = ? and dt_validade >= convert(date,getdate())", material_id);
            return consulta;
        }
        //Consultar emprestimos
        public static Ly_bib_emprestimo.Row ConsultarEmprestimo(string material_id)
        {
            Ly_bib_emprestimo.Row consulta = Ly_bib_emprestimo.QueryFirstRow(Config.CreateWritableConnection(), "id_bib_material = ? and DT_DEVOLUCAO is null", material_id);
            return consulta;
        }

        //existe reserva para este usuário?
        public static bool ExisteReservaUsuario(string material, string usuario)
        {
            string sql = "select 1 from Ly_bib_reserva where id_bib_material = ? and id_bib_usuario = ? and dt_validade >= convert(date,getdate())";

            int retorno = ExecutarFuncao(sql, material, usuario);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        //existe reserva para este usuário?
        public static bool ExisteReservaOutroUsuario(string material, string usuario)
        {
            string sql = "select 1 from Ly_bib_reserva where id_bib_material = ? and id_bib_usuario <> ? and dt_validade >= convert(date,getdate())";

            int retorno = ExecutarFuncao(sql, material, usuario);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        //consultar usuário
        public static string ConsultarUsuario(decimal? id_usuario)
        {
            string sql = "select NOME_COMPL from LY_BIB_USUARIO u inner join LY_PESSOA p on p.PESSOA = u.PESSOA where ID = ?";
            return ConsultarCampo(sql, id_usuario);
        }

        //consultar autores
        public static string ConsultarAutores(string id_titulo)
        {
            string sql = @"select NOME_AUTOR from LY_BIB_AUTOR a 
                                inner join LY_BIB_AUTOR_TITULO at
                                on a.ID = at.ID_BIB_AUTOR
                                where at.ID_BIB_TITULO = ?";
            QueryTable qt = null;
            qt = Consultar(sql, id_titulo);
            string nomes = "";

            if (qt != null)
            {
                for (int i = 0; i < qt.Rows.Count; i++)
                {
                    nomes += qt.Rows[i]["NOME_AUTOR"].ToString();
                    nomes += " ; ";
                }
            }
            return nomes;
        }

        public static RetValue CancelarReserva(Ly_bib_reserva.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    Ly_bib_reserva.Row.Update(connection, linha.Id, "Dt_validade", linha.Dt_validade);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Reserva cancelada com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue RegistrarPenalidade(Ly_bib_penalidade.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    linha = Ly_bib_penalidade.Row.Insert(connection, linha.Id_bib_usuario, linha.Motivo, linha.Dt_penalidade, "Dt_baixa, Id_bib_emprestimo, Valor", linha.Dt_baixa, linha.Id_bib_emprestimo, linha.Valor);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Devolução efetuada com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //consulta multo
        public static QueryTable ConsultarvalorMulta(string id_emprestimo)
        {
            string sql =
                @"select 
        config.VALOR_DIA_MULTA,
        config.VALOR_FIXO_MULTA,
        emp.DT_PREV_DEVOLUCAO
        from LY_BIB_EMPRESTIMO emp
        inner join LY_BIB_MATERIAL mat
	        on emp.ID_BIB_MATERIAL = mat.ID 
        inner join LY_BIB_TITULO tit 
	        on tit.ID = mat.ID_BIB_TITULO
        inner join LY_BIB_LOCALIZACAO loc
	        on loc.ID = mat.ID_BIB_LOCALIZACAO
        inner join LY_BIB_CONFIGURACOES config
	        on config.ID_BIB_BIBLIOTECA = loc.ID_BIB_BIBLIOTECA
	        and config.ID_BIB_TIPO_MATERIAL = tit.ID_BIB_TIPO_MATERIAL
        where emp.ID = ?";
            return Consultar(sql, id_emprestimo);
        }

        #region Tela penalidades

        public static System.Data.DataTable CarregarPenalidades(string id_bib)
        {
            string sql = @"select p.valor as valor, p.ID as id, p.dt_penalidade, p.dt_baixa, p.id_bib_usuario, p.motivo, pes.NOME_COMPL, p.id_bib_emprestimo, t.titulo
                        from LY_BIB_PENALIDADE p 
                        inner join LY_BIB_USUARIO u
                         on p.ID_BIB_USUARIO = u.ID 
                         inner join LY_PESSOA pes
                         on pes.PESSOA = u.PESSOA
                        inner join LY_BIB_BIBLIOTECA b
                        on u.ID_BIB_BIBLIOTECA = b.ID 
                        left join LY_BIB_EMPRESTIMO e
                        on e.ID = p.ID_BIB_EMPRESTIMO
                        left join LY_BIB_MATERIAL m
                        on e.ID_BIB_MATERIAL = m.ID
                        left join LY_BIB_TITULO t
                        on t.ID = m.ID_BIB_TITULO
                        where b.ID = ?
                        order by p.dt_penalidade desc";
            return Consultar(sql, id_bib);
        }

        //Incluir Penalidade
        public static RetValue IncluirPenalidade(Ly_bib_penalidade.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    if (linha.Valor == 0 || linha.Valor == null)
                    {
                        //calcula o valor da penalidade
                        //se existe valor fixo usa ele, senão faz dias * valor por dia, se for null é 0.
                        if (linha.Id_bib_emprestimo != null)
                        {
                            Techne.Data.QueryTable qt = RN.Biblioteca.ConsultarvalorMulta(linha.Id_bib_emprestimo.ToString());
                            if (qt != null)
                            {
                                if (qt.Rows.Count != 0)
                                {
                                    if (qt.Rows[0]["VALOR_FIXO_MULTA"] != null)
                                    {
                                        decimal multa = Convert.ToDecimal(qt.Rows[0]["VALOR_FIXO_MULTA"]);
                                        linha.Valor = multa;
                                    }
                                    else if (qt.Rows[0]["VALOR_DIA_MULTA"] != null)
                                    {
                                        DateTime detprevdev = Convert.ToDateTime(qt.Rows[0]["DT_PREV_DEVOLUCAO"]);
                                        decimal dias = Convert.ToDecimal(linha.Dt_penalidade.Value.Date.Subtract(detprevdev).TotalDays);
                                        decimal multa = Convert.ToDecimal(qt.Rows[0]["VALOR_DIA_MULTA"]) * dias;
                                        linha.Valor = multa;
                                    }
                                }
                            }
                        }
                    }

                    linha = Ly_bib_penalidade.Row.Insert(connection, linha.Id_bib_usuario, linha.Motivo, linha.Dt_penalidade, "Dt_baixa, Id_bib_emprestimo, Valor", linha.Dt_baixa, linha.Id_bib_emprestimo, linha.Valor);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Penalidade incluída com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Alterar Penalidade
        public static RetValue AlterarPenalidade(Ly_bib_penalidade.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    Ly_bib_penalidade.Row.Update(connection, linha.Id, "Dt_baixa, Motivo, Id_bib_emprestimo, Valor", linha.Dt_baixa, linha.Motivo, linha.Id_bib_emprestimo, linha.Valor);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Penalidade alterada com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Excluir Penalidade
        public static RetValue ExcluirPenalidade(Ly_bib_penalidade.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Ly_bib_penalidade.Row.Delete(connection, linha.Id);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Penalidade excluída com sucesso.", null);

            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //existe reserva para este usuário?
        public static bool ExistePenalidade(string usuario, string emprestimo, string motivo, DateTime data, string id)
        {
            string sql = null;
            int retorno = 0;

            if (string.IsNullOrEmpty(id))
            {
                sql = "select top 1 1 from LY_BIB_PENALIDADE where ID_BIB_EMPRESTIMO = ? and ID_BIB_USUARIO = ? and MOTIVO = ? and DT_PENALIDADE = ?";
                retorno = ExecutarFuncao(sql, emprestimo, usuario, motivo, data);
            }
            else
            {
                sql = "select top 1 1 from LY_BIB_PENALIDADE where ID_BIB_EMPRESTIMO = ? and ID_BIB_USUARIO = ? and MOTIVO = ? and DT_PENALIDADE = ? and id <> ?";
                retorno = ExecutarFuncao(sql, emprestimo, usuario, motivo, data, id);
            }

            if (retorno == 1)
                return true;
            else
                return false;
        }
        #endregion

        public static QueryTable ConsultarTipos()
        {
            string sql = @"select ID, imagem, SIGLA from LY_BIB_TIPO_MATERIAL";
            return Consultar(sql);
        }


        public static QueryTable ConsultarTitulo2(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select t.id, 
            t.titulo, 
            dbo.fn_Autores(t.ID) as autores,
            ed.NOME_EDITORA as editora,
            t.imagem as imagem
            from LY_BIB_TITULO t
            left join LY_BIB_TIPO_MATERIAL tm on tm.ID = t.ID_BIB_TIPO_MATERIAL
            left join LY_BIB_ASSUNTO_CDD acd on acd.ID = t.ID_BIB_ASSUNTO_CDD
            left join LY_BIB_EDITORA ed on ed.ID = t.ID_BIB_EDITORA
            where t.ID = ?");

            return Consultar(sb.ToString(), id);
        }

        public static QueryTable ConsultarMateriais(string titulo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select m.ID codigo, 
            bib.NOME_BIB biblioteca,
            bib.ID id_bib_biblioteca,
            u.NOME_COMP unidade,
            (case when isnull((select top 1 1 from LY_BIB_RESERVA where ID_BIB_MATERIAL = m.ID and convert(date,DT_VALIDADE) >= GETDATE()),0) > 0 then 'Reservado' 
                  when isnull((select top 1 1 from LY_BIB_EMPRESTIMO where ID_BIB_MATERIAL = m.ID and convert(date,DT_DEVOLUCAO) is null ),0) > 0 then 'Emprestado' 
                            else 'Disponível' end) status
             FROM LY_BIB_MATERIAL m 
            inner join LY_BIB_LOCALIZACAO loc
            on m.ID_BIB_LOCALIZACAO = loc.ID
            inner join LY_BIB_BIBLIOTECA bib
            on bib.ID = loc.ID_BIB_BIBLIOTECA
            inner join LY_UNIDADE_FISICA u
            on u.UNIDADE_FIS = bib.UNID_FISICA
            WHERE m.ID_BIB_TITULO = ? and m.DT_BAIXA is null");

            string usuario = HttpContext.Current.User.Identity.Name;

            if (!string.IsNullOrEmpty(usuario))
            {
                sb.Append(@"  and  m.ID_BIB_LOCALIZACAO in 
                        (select l.ID from LY_BIB_LOCALIZACAO l
                        inner join LY_BIB_BIBLIOTECA b on l.ID_BIB_BIBLIOTECA = b.ID
                        inner join LY_BIB_USUARIO u on u.ID_BIB_BIBLIOTECA = b.ID
                        where u.PESSOA = " + usuario + ")");
            }



            return Consultar(sb.ToString(), titulo);
        }

        //consultar usuário
        public static decimal ConsultarIDUsuario(string pessoa)
        {
            string sql = @"select u.ID from ly_bib_usuario u inner join ly_pessoa p
                         on p.PESSOA = u.pessoa
                         where p.PESSOA = ?";
            return ExecutarFuncao(sql, pessoa);
        }

        //Incluir Título
        public static RetValue FazerReserva(Ly_bib_reserva.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    linha = Ly_bib_reserva.Row.Insert(connection, linha.Id_bib_usuario, linha.Dt_reserva, "Id_bib_material, Id_bib_titulo, Dt_validade", linha.Id_bib_material, linha.Id_bib_titulo, linha.Dt_validade);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Reserva efetuada com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Incluir sugestão
        public static RetValue IncluirSugestao(Ly_bib_sugestao.Row linha, string usuario_pessoa)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                QueryTable bibliotecas = null;
                bibliotecas = ConsultarBibliotecas(usuario_pessoa);
                if (bibliotecas != null)
                {
                    for (int i = 0; i < bibliotecas.Rows.Count; i++)
                    {
                        linha.Id_bib_biblioteca = Convert.ToDecimal(bibliotecas.Rows[i]["ID_BIB_BIBLIOTECA"]);
                        Ly_bib_sugestao.Row.Insert(connection, linha.Titulo, linha.Autor, linha.Id_bib_usuario, linha.Data, linha.Id_bib_biblioteca, " Editora, Ano, Observacoes", linha.Editora, linha.Ano, linha.Observacoes);
                        retorno = VerificarErro(connection.GetErrors());
                        if (retorno != null)
                        {
                            connection.Rollback();
                            return retorno;
                        }
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Sugestão enviada com sucesso.", null);
        }

        public static QueryTable ConsultarBibliotecas(string usuario_pessoa)
        {
            string sql = "select ID_BIB_BIBLIOTECA from LY_BIB_USUARIO where PESSOA = ? ";
            return Consultar(sql, usuario_pessoa);
        }

        //Consultar
        public static Ly_bib_projeto.Row ConsultarProjeto(string projeto_id)
        {
            Ly_bib_projeto.Row consulta = Ly_bib_projeto.QueryFirstRow(Config.CreateWritableConnection(), "id = ?", projeto_id);
            return consulta;
        }

        //Incluir Biblioteca
        public static RetValue Incluir(ref Ly_bib_projeto.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    linha = Ly_bib_projeto.Row.Insert(connection, linha.Nome_projeto, linha.Id_bib_biblioteca, "Objetivo, Responsavel, Resultado, Dt_inicial, Dt_final", linha.Objetivo, linha.Responsavel, linha.Resultado, linha.Dt_inicial, linha.Dt_final);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Projeto incluído com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Alterar Binlioteca
        public static RetValue Alterar(Ly_bib_projeto.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (linha != null)
                {
                    Ly_bib_projeto.Row.Update(connection, linha.Id, "Nome_projeto, Objetivo, Responsavel, Resultado, Dt_inicial, Dt_final, Id_bib_biblioteca", linha.Nome_projeto, linha.Objetivo, linha.Responsavel, linha.Resultado, linha.Dt_inicial, linha.Dt_final, linha.Id_bib_biblioteca);

                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    retorno = new RetValue(true, "Projeto alterado com sucesso.", null);
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //Excluir Biblioteca
        public static RetValue Excluir(Ly_bib_projeto.Row linha)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Ly_bib_projeto.Row.Delete(connection, linha.Id);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Projeto excluído com sucesso.", null);

            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static QueryTable PesquisaSugestoes(string biblioteca, DateTime inicio, DateTime fim)
        {
            string sql = @"select s.id, s.titulo, s.autor, s.editora, s.ano, s.observacoes, p.nome_compl 
from Ly_bib_sugestao s inner join ly_bib_usuario u on s.id_bib_usuario = u.id 
inner join LY_PESSOA p on p.PESSOA = u.pessoa
where s.id_bib_biblioteca = ? and s.data >= ? and s.data <= ?";

            return RNBase.Consultar(sql, biblioteca, inicio, fim);
        }

        public static string VerificaExisteConfig(decimal? material, decimal? bib)
        {
            QueryTable qt = null;
            string sql = "select 1 from LY_BIB_CONFIGURACOES where ID_BIB_BIBLIOTECA = ? and ID_BIB_TIPO_MATERIAL = ?";
            qt = RNBase.Consultar(sql, bib, material);
            if (qt.Rows.Count > 0)
            {
                return "Já existe configuração para este tipo de material.";
            }

            return "";
        }

        public static string VerificaExisteConfig_Update(decimal? material, decimal? bib, decimal? id)
        {
            QueryTable qt = null;
            string sql = "select 1 from LY_BIB_CONFIGURACOES where ID_BIB_BIBLIOTECA = ? and ID_BIB_TIPO_MATERIAL = ? and ID <> ?";
            qt = RNBase.Consultar(sql, bib, material, id);
            if (qt.Rows.Count > 0)
            {
                return "Já existe configuração para este tipo de material.";
            }
            return "";
        }
    }
}

