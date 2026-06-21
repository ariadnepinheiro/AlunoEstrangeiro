using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class MacroCampos : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    //Lista todas os macros cadastrados no sistema
                    Command =
                        @" SELECT  ID_MACRO_CAMPOS, NOME, OBRIGATORIO, MATRICULA, 
                                    DT_CADASTRO, DT_ALTERACAO
                            FROM    DBO.TCE_MACRO_CAMPOS
                            ORDER BY DT_CADASTRO "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static ValidacaoDados Validar(TceMacroCampos macroCampos)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (macroCampos == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(macroCampos.Nome) || macroCampos.Nome.Length > 100)
            {
                mensagens.Add("O NOME é obrigatório e deve ter no máximo 100 caracteres!");
            }

            if (string.IsNullOrEmpty(macroCampos.Matricula) || macroCampos.Matricula.Length > 8)
            {
                mensagens.Add("O USUARIO RESPONSÁVEL é obrigatório e seu login deve ter no máximo 8 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                //verifica se existe outro macro com o mesmo nome cadastrado anteriormente
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    DBO.TCE_MACRO_CAMPOS
                            WHERE   NOME = @NOME
                                    AND ID_MACRO_CAMPOS <> @ID_MACRO_CAMPOS ");

                    contextQuery.Parameters.Add("@NOME", macroCampos.Nome);
                    contextQuery.Parameters.Add("@ID_MACRO_CAMPOS", macroCampos.IdMacroCampos);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("Já existe um macro campo cadastrado com este nome.");
                    }

                    //verificar se é inserção ou alteração
                    if (macroCampos.IdMacroCampos <= 0)
                    {
                        //Se for alteração, verifica se a macro esta relacionada com uma disciplina
                        contextQuery = new ContextQuery(
                        @" SELECT  1
                            FROM    LY_GRADE
                            WHERE   MACRO = @ID_MACRO_CAMPOS ");

                        contextQuery.Parameters.Add("@ID_MACRO_CAMPOS", macroCampos.IdMacroCampos);

                        obj = ctx.GetReturnValue(contextQuery);

                        if (obj != null)
                        {
                            mensagens.Add("Este Macro não pode ser alterado pois está vinculado a uma disciplina.");
                        }
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Inserir(TceMacroCampos macroCampos)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" INSERT  INTO dbo.TCE_MACRO_CAMPOS ( NOME, OBRIGATORIO, MATRICULA )
                                     VALUES  ( @NOME, @OBRIGATORIO, @MATRICULA ) "
                    };

                    contextQuery.Parameters.Add("@NOME", macroCampos.Nome);
                    contextQuery.Parameters.Add("@OBRIGATORIO", macroCampos.Obrigatorio);
                    contextQuery.Parameters.Add("@MATRICULA", macroCampos.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(TceMacroCampos macroCampos)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" UPDATE  DBO.TCE_MACRO_CAMPOS
                                    SET     NOME = @NOME, 
		                                    OBRIGATORIO = @OBRIGATORIO, 
		                                    MATRICULA = @MATRICULA,
                                            DT_ALTERACAO = GETDATE()
                                    WHERE   ID_MACRO_CAMPOS = @ID_MACRO_CAMPOS "
                    };

                    contextQuery.Parameters.Add("@NOME", macroCampos.Nome);
                    contextQuery.Parameters.Add("@OBRIGATORIO", macroCampos.Obrigatorio);
                    contextQuery.Parameters.Add("@MATRICULA", macroCampos.Matricula);
                    contextQuery.Parameters.Add("@ID_MACRO_CAMPOS", macroCampos.IdMacroCampos);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static ValidacaoDados ValidarRemover(int id)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (id <= 0)
            {
                return validacaoDados;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //Verifica se o macro já foi utilizado por uma matriz curricular
                var contextQuery = new ContextQuery(
                    @" SELECT  1
                        FROM    LY_GRADE
                        WHERE   MACRO = @ID_MACRO_CAMPOS ");

                contextQuery.Parameters.Add("@ID_MACRO_CAMPOS", id);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    mensagens.Add("Não é possível excluir este macro, pois ele já foi utilizado por uma matriz curricular.");
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" DELETE  dbo.TCE_MACRO_CAMPOS
                                     WHERE   ID_MACRO_CAMPOS = @ID_MACRO_CAMPOS "
                    };

                    contextQuery.Parameters.Add("@ID_MACRO_CAMPOS", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static bool ValidaObrigatorioGrade(int idMacro)
        {
            if (idMacro != 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    // se tiver não pode ser alterado
                    var contextQuery = new ContextQuery(
                    @"   SELECT  1
                            FROM    LY_GRADE
                            WHERE   MACRO in (
                                        SELECT ID_MACRO_CAMPOS 
                                            from TCE_MACRO_CAMPOS 
                                            where ID_MACRO_CAMPOS = @ID_MACRO_CAMPOS and OBRIGATORIO = 1) ");

                    contextQuery.Parameters.Add("@ID_MACRO_CAMPOS", idMacro);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static object GetID_Nome(string nome)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" select ID_MACRO_CAMPOS from TCE_MACRO_CAMPOS where nome = @NOME");

                contextQuery.Parameters.Add("@NOME", nome);

                return ctx.GetReturnValue(contextQuery);
            }
        }
    }
}
