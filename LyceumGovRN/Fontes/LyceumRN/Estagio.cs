using System;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class Estagio : RNBase
    {
        //Gera código de ordem (chave) a partir do último do banco
        public static decimal GeraEstagio(TConnection connection, decimal? ano, decimal? semestre, string turma, string disciplina, string aluno)
        {

            decimal ordem;
            DbObject dbordem;

            dbordem = TCommand.ExecuteScalar(connection, "select max(estagio_trab) from ly_estagio where ano = ? and semestre = ? and turma = ? and disciplina = ? and aluno = ?", ano, semestre, turma, disciplina, aluno);

            if (!dbordem.IsNull)
            {
                ordem = Convert.ToDecimal(dbordem);
                return ordem + 1;
            }
            return 1;
        }

        public static QueryTable ConsultarEstagios(string ano, string semestre, string turma, string disciplina, string aluno)
        {
            TConnection cn = Config.CreateConnection();

            string sql = @"select 
                    e.ano,
                    e.semestre,
                    e.turma,
                    e.disciplina,
                    e.aluno,
                    e.estagio_trab,
                    e.descricao,
                    e.dtini,
                    e.dtfim,
                    e.num_func,
                    PE.nome_compl,
                    e.observacao,
                    ee.chave,
                    emp.razao_social as empresa,
                    ee.supervisor,
                    ee.funcao,
                    ee.cargo,
                    ee.email
                     from LY_ESTAGIO e
                    left join LY_ESTAGIO_EMPRESA ee
                    on(e.ALUNO = ee.ALUNO and e.DISCIPLINA = ee.DISCIPLINA 
                    and e.TURMA = ee.TURMA and e.ANO = ee.ANO 
                    and e.SEMESTRE = ee.SEMESTRE and e.ESTAGIO_TRAB = ee.ESTAGIO_TRAB)
                    left join ly_empresa emp on (ee.empresa = emp.empresa)
                    left join ly_docente d on d.num_func = e.num_func
                    LEFT JOIN Ly_PESSOA PE ON PE.PESSOA = D.PESSOA
                    where e.ANO = ?
                    and e.SEMESTRE = ?
                    and e.TURMA = ?
                    and e.DISCIPLINA = ?
                    AND e.ALUNO = ?";

            QueryTable qt = new QueryTable(sql);

            qt.Query(cn, ano, semestre, turma, disciplina, aluno);

            return qt;
        }

        //verifica se empresa já existe antes de cadastrar outra repetida
        public static string ConsultaExisteEmpresa(string empresa)
        {
            string sql = "select top 1 empresa from ly_empresa where razao_social = ? ";
            return ConsultarCampo(sql, empresa);
        }

        //Gera código de ordem (chave) a partir do último do banco
        public static decimal GeraEmpresa()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            decimal ordem;
            DbObject dbordem;

            try
            {
                dbordem = TCommand.ExecuteScalar(connection, "select max(empresa) from ly_empresa");
            }
            finally
            {
                connection.Close();
            }
            if (!dbordem.IsNull)
            {
                ordem = Convert.ToDecimal(dbordem);
                return ordem + 1;
            }
            return 1;
        }

        //inclui registro em ly_estagio, ly_empresa_estagio e ly_empresa
        public static RetValue IncluirEstagio(Ly_estagio.Row dtEstagio, Ly_estagio_empresa.Row dtEmpresa)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtEstagio != null)
                {
                    dtEstagio.Estagio_trab = Convert.ToString(GeraEstagio(connection, dtEstagio.Ano, dtEstagio.Semestre, dtEstagio.Turma, dtEstagio.Disciplina, dtEstagio.Aluno));
                    dtEmpresa.Estagio_trab = dtEstagio.Estagio_trab;
                    Ly_estagio.Row.Insert(connection, dtEstagio.Aluno, dtEstagio.Disciplina, dtEstagio.Turma, dtEstagio.Ano, dtEstagio.Semestre, dtEstagio.Estagio_trab, dtEstagio.Dtini, "observacao,descricao,num_func,dtfim", dtEstagio.Observacao, dtEstagio.Descricao, dtEstagio.Num_func, dtEstagio.Dtfim);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    else
                    {
                        if (dtEmpresa != null)
                        {
                            if (!String.IsNullOrEmpty(dtEmpresa.Empresa))
                            {//insere a empresa e gera cógido pra ela
                                dtEmpresa.Razao_social = dtEmpresa.Empresa;
                                string empresa = ConsultaExisteEmpresa(dtEmpresa.Razao_social);
                                if (!String.IsNullOrEmpty(empresa))
                                {
                                    dtEmpresa.Empresa = empresa;
                                }
                                else
                                {
                                    decimal codigo = GeraEmpresa();
                                    Ly_empresa.Row.Insert(connection, Convert.ToString(codigo), dtEmpresa.Razao_social);

                                    retorno = VerificarErro(connection.GetErrors());
                                    if (retorno != null && !retorno.Ok)
                                    {
                                        connection.Rollback();
                                        return retorno;
                                    }
                                    dtEmpresa.Empresa = Convert.ToString(codigo);
                                }
                            }
                            Ly_estagio_empresa.Row.Insert(connection, dtEmpresa.Aluno, dtEmpresa.Disciplina, dtEmpresa.Turma, dtEmpresa.Ano, dtEmpresa.Semestre, dtEmpresa.Estagio_trab, "empresa, supervisor, cargo, funcao, email", dtEmpresa.Empresa, dtEmpresa.Supervisor, dtEmpresa.Cargo, dtEmpresa.Funcao, dtEmpresa.Email);

                            retorno = VerificarErro(connection.GetErrors());

                            if (retorno != null && !retorno.Ok)
                            {
                                connection.Rollback();
                                return retorno;
                            }
                        }

                    }
                }
                return new RetValue(true, "Registro incluído com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        //inclui registro em ly_estagio, ly_empresa_estagio e ly_empresa
        public static RetValue IncluirEstagio(Ly_estagio.Row dtEstagio, Ly_estagio_empresa.Row dtEmpresa, TConnectionWritable connection)
        {
            RetValue retorno = null;
            try
            {
                if (dtEstagio != null)
                {
                    dtEstagio.Estagio_trab = Convert.ToString(GeraEstagio(connection, dtEstagio.Ano, dtEstagio.Semestre, dtEstagio.Turma, dtEstagio.Disciplina, dtEstagio.Aluno));
                    dtEmpresa.Estagio_trab = dtEstagio.Estagio_trab;
                    Ly_estagio.Row.Insert(connection, dtEstagio.Aluno, dtEstagio.Disciplina, dtEstagio.Turma, dtEstagio.Ano, dtEstagio.Semestre, dtEstagio.Estagio_trab, dtEstagio.Dtini, "observacao,descricao,num_func,dtfim", dtEstagio.Observacao, dtEstagio.Descricao, dtEstagio.Num_func, dtEstagio.Dtfim);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    else
                    {
                        if (dtEmpresa != null)
                        {
                            if (!String.IsNullOrEmpty(dtEmpresa.Empresa))
                            {//insere a empresa e gera cógido pra ela
                                dtEmpresa.Razao_social = dtEmpresa.Empresa;
                                string empresa = ConsultaExisteEmpresa(dtEmpresa.Razao_social);
                                if (!String.IsNullOrEmpty(empresa))
                                {
                                    dtEmpresa.Empresa = empresa;
                                }
                                else
                                {
                                    decimal codigo = GeraEmpresa();
                                    Ly_empresa.Row.Insert(connection, Convert.ToString(codigo), dtEmpresa.Razao_social);

                                    retorno = VerificarErro(connection.GetErrors());
                                    if (retorno != null && !retorno.Ok)
                                    {
                                        connection.Rollback();
                                        return retorno;
                                    }
                                    dtEmpresa.Empresa = Convert.ToString(codigo);
                                }
                            }
                            Ly_estagio_empresa.Row.Insert(connection, dtEmpresa.Aluno, dtEmpresa.Disciplina, dtEmpresa.Turma, dtEmpresa.Ano, dtEmpresa.Semestre, dtEmpresa.Estagio_trab, "empresa, supervisor, cargo, funcao, email", dtEmpresa.Empresa, dtEmpresa.Supervisor, dtEmpresa.Cargo, dtEmpresa.Funcao, dtEmpresa.Email);

                            retorno = VerificarErro(connection.GetErrors());

                            if (retorno != null && !retorno.Ok)
                            {
                                connection.Rollback();
                                return retorno;
                            }
                        }
                    }
                }
                return new RetValue(true, "Registro incluído com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
        }

        public static RetValue ExcluirEstagio(Ly_estagio_empresa.Row dtEmpresa)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtEmpresa != null)
                {
                    Ly_estagio_empresa.Row.Delete(connection, dtEmpresa.Aluno, dtEmpresa.Disciplina, dtEmpresa.Turma, dtEmpresa.Ano, dtEmpresa.Semestre, dtEmpresa.Estagio_trab, dtEmpresa.Chave);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    else
                    {
                        Ly_estagio.Row.Delete(connection, dtEmpresa.Aluno, dtEmpresa.Disciplina, dtEmpresa.Turma, dtEmpresa.Ano, dtEmpresa.Semestre, dtEmpresa.Estagio_trab);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }
                        return new RetValue(true, "Registro excluído com sucesso.", null);
                    }
                }
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue AtualizarEstagio(Ly_estagio.Row dtEstagio, Ly_estagio_empresa.Row dtEmpresa)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtEstagio != null)
                {
                    Ly_estagio.Row.Update(connection, dtEstagio.Aluno, dtEstagio.Disciplina, dtEstagio.Turma, dtEstagio.Ano, dtEstagio.Semestre, dtEstagio.Estagio_trab, "dtini,observacao,descricao,num_func,dtfim", dtEstagio.Dtini, dtEstagio.Observacao, dtEstagio.Descricao, dtEstagio.Num_func, dtEstagio.Dtfim);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    else
                    {
                        if (dtEmpresa != null)
                        {

                            if (!String.IsNullOrEmpty(dtEmpresa.Empresa))
                            {//insere a empresa e gera cógido pra ela
                                dtEmpresa.Razao_social = dtEmpresa.Empresa;
                                string empresa = ConsultaExisteEmpresa(dtEmpresa.Razao_social);
                                if (!String.IsNullOrEmpty(empresa))
                                {
                                    dtEmpresa.Empresa = empresa;
                                }
                                else
                                {
                                    decimal codigo = GeraEmpresa();
                                    Ly_empresa.Row.Insert(connection, Convert.ToString(codigo), dtEmpresa.Razao_social);

                                    retorno = VerificarErro(connection.GetErrors());
                                    if (retorno != null && !retorno.Ok)
                                    {
                                        connection.Rollback();
                                        return retorno;
                                    }
                                    dtEmpresa.Empresa = Convert.ToString(codigo);
                                }
                            }
                            Ly_estagio_empresa.Row.Update(connection, dtEmpresa.Aluno, dtEmpresa.Disciplina, dtEmpresa.Turma, dtEmpresa.Ano, dtEmpresa.Semestre, dtEmpresa.Estagio_trab, dtEmpresa.Chave, "empresa, supervisor, cargo, funcao, email", dtEmpresa.Empresa, dtEmpresa.Supervisor, dtEmpresa.Cargo, dtEmpresa.Funcao, dtEmpresa.Email);

                            retorno = VerificarErro(connection.GetErrors());

                            if (retorno != null && !retorno.Ok)
                            {
                                connection.Rollback();
                                return retorno;
                            }
                        }
                    }
                }
                return new RetValue(true, "Registro alterado com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message.ToString()));
            }
            finally
            {
                connection.Close();
            }
        }

        //verifica se a disciplina é de estágio
        public static bool VerificaDisciplinaEstagio(string disciplina)
        {
            string sql = "select 1 from ly_disciplina where ESTAGIO = 'S' and DISCIPLINA = ?";

            int retorno = ExecutarFuncao(sql, disciplina);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static QueryTable ConsultarDisciplinaEstagio(string aluno, string ano, string semestre)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT DISTINCT m.disciplina, d.nome FROM ly_matricula m inner join ly_disciplina d on d.disciplina = m.disciplina WHERE m.aluno = ? AND m.ano = ? AND m.semestre = ? AND d.ESTAGIO = 'S' ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, aluno, ano, semestre);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }
    }
}
