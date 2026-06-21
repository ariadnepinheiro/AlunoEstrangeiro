using System;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class Servico : RNBase
    {
        public const string AlteracaoCadastral = "ALTERACAOCADASTRAL";

        public const string PrimeiraVia = "1VIACARTAO";

        public const string SegundaVia = "2VIACARTAO";

        public static bool AlterouDadosAluno(string aluno, string nome, string endereco, string numero, string cep, string municipio, string pais, string bairro, DateTime? data, string nome_mae)
        {
            var sql = @"select top 1 1 from LY_ALUNO a inner join LY_PESSOA p
                            on p.PESSOA = a.PESSOA
                            where (p.NOME_COMPL <> ?
                            or p.ENDERECO <> ?
                            or p.CEP <> ?
                            or p.END_NUM <> ?
                            or p.END_MUNICIPIO <> ?
                            or p.END_PAIS <> ?
                            or p.BAIRRO <> ?
                            or convert(date,p.DT_NASC) <> convert(date,?)
                            or p.nome_mae <> ?)
                            and ALUNO = ?";

            var retorno = nome_mae != null ? ExecutarFuncao(sql, nome, endereco, cep, numero, municipio, pais, bairro, data, nome_mae, aluno)
                              : ExecutarFuncao(sql, nome, endereco, cep, numero, municipio, pais, bairro, data, DBNull.Value, aluno);

            if (retorno == 1)
            {
                return true;
            }

            return false;
        }

        public static bool AlterouDadosNoCartaoDoAlunoMatriculado(string aluno, string nome, DateTime? data)
        {
            var sql = @"select top 1 1 from LY_ALUNO a 
                        inner join LY_PESSOA p
                            on p.PESSOA = a.PESSOA
                        inner join ly_matricula m
                            on a.aluno = m.aluno
                        where (p.NOME_COMPL <> ?
                        or convert(date,p.DT_NASC) <> convert(date,?))
                        and a.ALUNO = ?
                        and m.sit_matricula = 'Matriculado'";

            var retorno = ExecutarFuncao(sql, nome, data, aluno);

            return retorno == 1;
        }

        public static bool AlterouDadosNoCadastroDoAlunoMatriculado(string aluno, string mae, string endereco, string numero, string cep, string municipio, string pais, string bairro)
        {
            var sql = @"select top 1 1 from LY_ALUNO a inner join LY_PESSOA p WITH(NOLOCK)
                            on p.PESSOA = a.PESSOA
                            where (p.ENDERECO <> ?
                            or p.CEP <> ?
                            or p.END_NUM <> ?
                            or p.END_MUNICIPIO <> ?
                            or p.END_PAIS <> ?
                            or p.BAIRRO <> ?
                            or p.nome_mae <> ?)
                            and ALUNO = ?";

            var retorno = mae != null ? ExecutarFuncao(sql, endereco, cep, numero, municipio, pais, bairro, mae, aluno)
                              : ExecutarFuncao(sql, endereco, cep, numero, municipio, pais, bairro, DBNull.Value, aluno);

            return retorno == 1;
        }

        [MethodDescription("Preenchendo Grid de ANDAMENTO."),
         ToolTip("Andamento"),
         ControlText("Andamento de serviços"),
         Image("~/Images/Proc.gif")]
        public static QueryTable AndamentoImagem(Number solicitacao)
        {
            QueryTable qt;

            var cn = Config.CreateConnection();

            cn.Open();

            try
            {
                qt = new QueryTable("SELECT andamento, solicitacao, passo, comentario, data, status, setor, usuario, proximo_setor, proximo_usuario FROM ly_andamento WHERE solicitacao = ?");

                qt.Query(cn, solicitacao);

                return qt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        public static RetValue CancelarAndamento(decimal solicit, decimal item, string servico, string usuario, string comentario)
        {
            var qt = ConsultarPassoSetor(solicit, item, servico);

            if (!VerificaPassoCancelado(solicit, item))
            {
                if (qt != null && qt.Rows.Count > 0)
                {
                    var passo = qt.Rows[0]["passo"].ToString();
                    var setor = qt.Rows[0]["setor"].ToString();
                    var data = DateTime.Now;
                    try
                    {
                        var sql = "insert into ly_andamento (SOLICITACAO, ITEM_SOLICITACAO, SERVICO, PASSO ,Data, STATUS, SETOR, USUARIO, COMENTARIO) " +
                                  "values (? , ?, ?, ?, ?, ?, ?, ?,?)";
                        if (VerificaUsuario(usuario, setor))
                        {
                            IAE(sql, solicit, item, servico, passo, data, "Cancelado", setor, usuario, comentario);
                        }
                        else
                        {
                            return new RetValue(false, string.Empty, new ErrorList("Usuário não tem permissão para cancelar andamento."));
                        }
                    }
                    catch (Exception ex)
                    {
                        return new RetValue(false, string.Empty, new ErrorList(ex.Message));
                    }
                }
                else
                {
                    return new RetValue(false, string.Empty, new ErrorList("Não existe próximo passo para este serviço."));
                }
            }
            else
            {
                return new RetValue(false, string.Empty, new ErrorList("Existe passo anterior cancelado."));
            }

            return new RetValue(true, "Andamento cancelado com sucesso.", null);
        }

        public static QueryTable ConsultarAndamentos(decimal solicit, decimal item, string servico)
        {
            var cn = Config.CreateConnection();

            var qt = new QueryTable(@"select d.andamento, 
								    d.servico as servico,  
								    d.passo as passo,  
								    d.solicitacao as solicitacao,  
								    d.item as item ,  
								    d.data as data,  
								    d.status as status,  
								    d.setor as setor,  
								    d.usuario as usuario, d.comentario as comentario from  
								    (select a.andamento as andamento,  
								    a.servico as servico,  
								    a.passo as passo,  
								    a.solicitacao as solicitacao,  
								    a.item_solicitacao as item ,  
								    a.data as data,  
								    a.status as status,  
								    a.setor as setor,  
								    a.usuario as usuario,  
                                    a.comentario as comentario  
								    from ly_andamento a  
								    where a.solicitacao = ? 
								    and a.item_solicitacao = ? 
								    union all 
								    select null as andamento,  
								    b.servico as servico,  
								    b.passo as passo,  
								    null as solicitacao,  
								    null as item ,  
								    null as data,  
								    'Pendente' as status,  
								    b.setor as setor,  
								    null as usuario,  
                                    null as comentario  
								    from LY_FLUXO_DE_ANDAMENTO b 
								    where b.SERVICO = ? 
								    and not exists (select 1 from ly_andamento an where an.SOLICITACAO = ? and STATUS = 'Cancelado') 
								    and b.PASSO not IN  
								    (select c.passo as passo 
								    from ly_andamento c 
								    where c.solicitacao = ? 
								    and c.item_solicitacao = ? ))  d 
								    order by PASSO asc ");

            qt.Query(cn, solicit, item, servico, solicit, solicit, item);

            return qt;
        }

        public static QueryTable ConsultarPassoSetor(decimal solicit, decimal item, string servico)
        {
            var cn = Config.CreateConnection();
            var qt = new QueryTable("select passo, setor " +
                                    "from ly_fluxo_de_andamento f " +
                                    "where f.servico = ? " +
                                    "and not exists  " +
                                    "(select 1 " +
                                    "from ly_andamento a " +
                                    "where a.servico = f.servico " +
                                    "and a.passo = f.passo " +
                                    "and a.solicitacao = ? " +
                                    "and a.item_solicitacao = ?) " +
                                    "order by passo");

            qt.Query(cn, servico, solicit, item);

            return qt;
        }

        public static QueryTable ConsultarServicos(string aluno)
        {
            var cn = Config.CreateConnection();
            var qt = new QueryTable(@"Select s.solicitacao as solicitacao, s.aluno as aluno, s.data as data, i.item_solicitacao as item, i.servico as servico, t.descricao as descricao, i.qtd as qtd, i.obs as obs, 
                                    Case WHEN s.OPERADORABILHETAGEMID = 2 THEN 'SIM'
	                                     WHEN s.OPERADORABILHETAGEMID = 1 THEN 'NÃO'
		                                 ELSE 'NÃO INFORMADO'
	                                END as operadora,
				                    Case WHEN Convert(int, (select count(*) 
				                    from ly_andamento a 
				                    where a.solicitacao = s.solicitacao 
				                      and a.item_solicitacao = i.ITEM_SOLICITACAO 
				                      and exists (select 1 from ly_fluxo_de_andamento f where a.servico = f.servico having a.passo = max(f.passo))				      
				                      and a.status = 'Executado')) > 0 THEN 'Concluído'  
				                    WHEN Convert(varchar,(select count(*) 
				                    from ly_andamento a 
				                    where a.solicitacao = s.solicitacao 
				                     and a.item_solicitacao = i.ITEM_SOLICITACAO 
                                      and exists(select 1 from ly_fluxo_de_andamento f where a.servico = f.servico having a.passo = max(f.passo))				      
				                      and a.status = 'Executado')) +  
				                    Convert(varchar,(select count(*) 
				                    from ly_andamento a 
				                    where a.solicitacao = i.solicitacao 
				                      and a.item_solicitacao = i.item_solicitacao 
				                      and a.status = 'Cancelado')) = '00' THEN 'Pendente' 
				                    ELSE 'Cancelado' 
				                    END status   
				                      from ly_solicitacao_serv s join ly_itens_solicit_serv i 
				                    on s.solicitacao = i.solicitacao 
				                    join ly_tabela_servicos t 
				                     on i.servico = t.servico
				                    where s.aluno = ? 
                                      and t.servico <> 'ALTERACAOCADASTRAL'
				                    order by s.DATA desc ");

            qt.Query(cn, aluno);

            return qt;
        }

        public static RetValue DarAndamento(decimal solicit, decimal item, string servico, string usuario)
        {
            var qt = ConsultarPassoSetor(solicit, item, servico);

            if (!VerificaPassoCancelado(solicit, item))
            {
                if (qt != null && qt.Rows.Count > 0)
                {
                    var passo = qt.Rows[0]["passo"].ToString();
                    var setor = qt.Rows[0]["setor"].ToString();
                    var data = DateTime.Now;
                    try
                    {
                        var sql = "insert into ly_andamento (SOLICITACAO, ITEM_SOLICITACAO, SERVICO, PASSO ,Data, STATUS, SETOR, USUARIO) " +
                                  "values (? , ?, ?, ?, ?, ?, ?, ? )";

                        if (VerificaUsuario(usuario, setor))
                        {
                            IAE(sql, solicit, item, servico, passo, data, "Executado", setor, usuario);
                        }
                        else
                        {
                            return new RetValue(false, string.Empty, new ErrorList("Usuário não tem permissão para dar andamento."));
                        }
                    }
                    catch (Exception ex)
                    {
                        return new RetValue(false, string.Empty, new ErrorList(ex.Message));
                    }
                }
                else
                {
                    return new RetValue(false, string.Empty, new ErrorList("Não existe próximo passo para este serviço."));
                }
            }
            else
            {
                return new RetValue(false, string.Empty, new ErrorList("Existe passo anterior cancelado."));
            }

            return new RetValue(true, "Andamento realizado com sucesso.", null);
        }

        public static RetValue Excluir(decimal solicitacao)
        {
            var connection = Config.CreateWritableConnection();
            RetValue retorno = null;

            connection.Open(true);

            try
            {
                var dtI = Ly_itens_solicit_serv.Query(connection, " solicitacao = ? ", solicitacao);

                if (dtI != null)
                {
                    if (dtI.Rows != null)
                    {
                        foreach (Ly_itens_solicit_serv.Row linha in dtI.Rows)
                        {
                            linha.Delete();
                        }

                        dtI.Put(connection);
                        retorno = VerificarErro(dtI);

                        if (retorno != null)
                        {
                            return retorno;
                        }
                        else
                        {
                            var dtS = Ly_solicitacao_serv.Query(connection, " solicitacao = ?", solicitacao);

                            if (dtS != null)
                            {
                                if (dtS.Rows != null)
                                {
                                    foreach (Ly_solicitacao_serv.Row linha in dtS.Rows)
                                    {
                                        linha.Delete();
                                    }

                                    dtS.Put(connection);
                                    retorno = VerificarErro(dtI);

                                    if (retorno != null)
                                    {
                                        return retorno;
                                    }
                                }
                            }
                        }

                        retorno = new RetValue(true, "Solicitação removida com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static bool ExistePrimeiraViaCartao(string aluno)
        {
            const string Sql = @"SELECT  TOP 1 1
                                 FROM    Ly_solicitacao_serv s
                                         INNER JOIN Ly_itens_solicit_serv i ON s.SOLICITACAO = i.SOLICITACAO
                                 WHERE   ALUNO = ?
                                         AND SERVICO = '1VIACARTAO'
                                         AND NOT EXISTS ( SELECT 1
                                                      FROM   ly_fluxo_de_andamento f
                                                      WHERE  f.servico = i.SERVICO
                                                             AND NOT EXISTS ( SELECT 1
                                                                              FROM   ly_andamento a
                                                                              WHERE  a.servico = f.servico
                                                                                     AND a.passo = f.passo
                                                                                     AND a.solicitacao = s.SOLICITACAO
                                                                                    AND a.item_solicitacao = i.ITEM_SOLICITACAO ) )";

            var retorno = ExecutarFuncao(Sql, aluno);

            return retorno == 1;
        }

        public static bool ExisteSolicitacaoPendente(string aluno, string servico)
        {
            var sql = @"SELECT TOP 1
                                1
                        FROM    Ly_solicitacao_serv s
                                INNER JOIN LY_ITENS_SOLICIT_SERV i
                                    ON i.SOLICITACAO = s.SOLICITACAO
                        WHERE   ALUNO = ?
                                AND i.SERVICO = ?
                                AND EXISTS ( SELECT 1
                                             FROM   ly_fluxo_de_andamento f
                                             WHERE  f.servico = i.SERVICO
                                                    AND NOT EXISTS ( SELECT 1
                                                                     FROM   ly_andamento a
                                                                     WHERE  a.servico = f.servico
                                                                            AND a.passo = f.passo
                                                                            AND a.solicitacao = s.SOLICITACAO
                                                                            AND a.item_solicitacao = i.ITEM_SOLICITACAO ) )
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   ly_andamento a
                                                 WHERE  a.servico = i.SERVICO
                                                        AND a.solicitacao = s.SOLICITACAO
                                                        AND a.item_solicitacao = i.ITEM_SOLICITACAO
                                                        AND a.STATUS = 'Cancelado' )";

            var retorno = ExecutarFuncao(sql, aluno, servico);

            if (retorno == 1)
            {
                return true;
            }

            return false;
        }

        public static bool ExisteSolicitacaoPendente(string aluno)
        {
            const string Sql = @"SELECT TOP 1
                                         1
                                 FROM    Ly_solicitacao_serv s
                                         INNER JOIN LY_ITENS_SOLICIT_SERV i ON i.SOLICITACAO = s.SOLICITACAO
                                 WHERE   ALUNO = ?
                                         AND ( i.SERVICO = '2VIACARTAO'
                                               OR i.SERVICO = '1VIACARTAO'
                                             )
                                         AND EXISTS ( SELECT 1
                                                      FROM   ly_fluxo_de_andamento f
                                                      WHERE  f.servico = i.SERVICO
                                                             AND NOT EXISTS ( SELECT 1
                                                                              FROM   ly_andamento a
                                                                              WHERE  a.servico = f.servico
                                                                                     AND a.passo = f.passo
                                                                                     AND a.solicitacao = s.SOLICITACAO
                                                                                     AND a.item_solicitacao = i.ITEM_SOLICITACAO ) )
                                         AND NOT EXISTS ( SELECT 1
                                                          FROM   ly_andamento a
                                                          WHERE  a.servico = i.SERVICO
                                                                 AND a.solicitacao = s.SOLICITACAO
                                                                 AND a.item_solicitacao = i.ITEM_SOLICITACAO
                                                                 AND a.STATUS = 'Cancelado' )";

            var retorno = ExecutarFuncao(Sql, aluno);

            return retorno == 1;
        }

        public static RetVal GetGridAndamento(TConnectionWritable cn, Number solicitacao)
        {
            cn.Open(true);

            try
            {
                Ly_andamento.Row.Query(cn, solicitacao);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            return RetVal.Success(string.Empty);
        }

        public static RetValue IncluirSolicitacao(TConnectionWritable connection, string aluno, string servico, string obs, 
            ref string solicitacao, string operadoraId, string usuario)
        {
            var dtS = new Ly_solicitacao_serv();
            var dtI = new Ly_itens_solicit_serv();
            var dadosS = dtS.NewRow();
            var dadosI = dtI.NewRow();

            dadosS.Aluno = aluno;
            dadosS.Data = DateTime.Now;
            dadosS.OperadoraBilhetagemId = Convert.ToDecimal(operadoraId);
            dadosS.Usuario = usuario;

            dadosI.Item_solicitacao = 1;
            dadosI.Solicitacao = 0;
            dadosI.Servico = servico;
            dadosI.Obs = obs;
            dadosI.Qtd = 1;
            
            dtS.Rows.Add(dadosS);
            dtI.Rows.Add(dadosI);

            // Verifica se existe solicitação pendente
            if (ExisteSolicitacaoPendente(dtS.Rows[0].Aluno, dtI.Rows[0].Servico))
            {
                return new RetValue(false, null, new ErrorList("Aluno já possui uma solicitação pendente para o serviço."));
            }

            RetValue retorno = null;

            if (dtS.Rows != null)
            {
                dtS.Put(connection);
                retorno = VerificarErro(dtS);

                if (retorno != null)
                {
                    return retorno;
                }

                dtI.Rows[0].Solicitacao = dtS.Rows[0].Solicitacao;

                if (dtI.Rows != null)
                {
                    dtI.Put(connection);

                    retorno = VerificarErro(dtI);

                    if (retorno != null)
                    {
                        return retorno;
                    }

                    return new RetValue(true, "Solicitação incluida com sucesso.", null);
                }
            }

            if (dtI.Rows != null)
            {
                solicitacao = dtI.Rows[0].Solicitacao.ToString();
            }

            return retorno;
        }

        public static RetValue IncluirSolicitacao(string aluno, string servico, string obs, ref string solicitacao, string operadoraId, 
            string usuario)
        {
            var connection = Config.CreateWritableConnection();

            connection.Open(true);

            var retorno = IncluirSolicitacao(connection, aluno, servico, obs, ref solicitacao, operadoraId, usuario);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    connection.Rollback();

                    return retorno;
                }
            }

            connection.Close();

            return retorno;
        }

        // Excluir uma solicitação
        public static bool ValidaDadosAluno(string aluno)
        {
            var sql = @"select COUNT(*) from LY_ALUNO a inner join LY_PESSOA p
                            on p.PESSOA = a.PESSOA
                            where (p.NOME_COMPL is null
                            or p.ENDERECO is null
                            or p.CEP is null
                            or p.END_NUM is null
                            or p.END_MUNICIPIO is null
                            or p.BAIRRO is null
                            or p.DT_NASC is null)
                            and ALUNO = ?";

            var retorno = ExecutarFuncao(sql, aluno);

            if (retorno > 0)
            {
                return true;
            }

            return false;
        }

        public static bool VerificaPassoCancelado(decimal solicit, decimal item)
        {
            var sql = "select 1 from ly_andamento where STATUS = 'Cancelado' and SOLICITACAO = ? and ITEM_SOLICITACAO = ? ";
            var retorno = ExecutarFuncao(sql, solicit, item);

            if (retorno == 1)
            {
                return true;
            }

            return false;
        }

        public static bool VerificaMunicipioSalineiras(string aluno)
        {
            var sql = @"Select COUNT(1)
                          From LY_ALUNO          A 
                          Join LY_UNIDADE_ENSINO UE
                            On UE.UNIDADE_ENS    =  A.UNIDADE_ENSINO
                          Join MUNICIPIO         M
                            On M.CODIGO	         =  UE.MUNICIPIO
                         Where UE.ID_REGIONAL Is Not Null
                           And M.CODIGO       In ('00006848'
                                                 ,'00006851'
                                                 ,'00006873'
                                                 ,'00006932'
                                                 ,'00007078'
                                                 ,'00007085'
                                                 ,'00007091'
                                                 ,'00010237')
                           And A.ALUNO           = ?";

            var retorno = ExecutarFuncao(sql, aluno);

            if (retorno == 1)
            {
                return true;
            }

            return false;
        }

        public static bool VerificaUsuario(string usuario, string setor)
        {
            var sql = "select privil from USUARIO where usuario = ?";
            var privil = ConsultarCampo(sql, usuario);

            if (!string.IsNullOrEmpty(privil))
            {
                if (privil.ToUpper() != "S")
                {
                    sql = "select padaces from PADUSUARIO where usuario = ? ";

                    var qt = Consultar(sql, usuario);

                    if (qt != null && qt.Rows.Count > 0)
                    {
                        foreach (SimpleRow sr in qt.Rows)
                        {
                            if (Convert.ToString(sr["padaces"]) == setor)
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (privil.ToUpper() == "S")
                {
                    return true;
                }
            }

            return false;
        }

    }
}