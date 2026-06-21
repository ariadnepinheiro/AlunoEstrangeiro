namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class CompartilhadaOferta : RNBase
    {
        public static void Inserir(TceCompartilhadaOferta compartilhadaOferta)
        {
            var contextQuery = new ContextQuery(
                @"INSERT  INTO TCE_COMPARTILHADA_OFERTA
                        (
                          ID_COMPARTILHADA,
                          CURSO,
                          TURNO,
                          MATRICULA 
                        )
                VALUES  (
                          @ID_COMPARTILHADA,
                          @CURSO,
                          @TURNO,
                          @MATRICULA 
                        )");

            contextQuery.Parameters.Add("@ID_COMPARTILHADA", compartilhadaOferta.IdCompartilhada);
            contextQuery.Parameters.Add("@CURSO", compartilhadaOferta.Curso);
            contextQuery.Parameters.Add("@TURNO", compartilhadaOferta.Turno);
            contextQuery.Parameters.Add("@MATRICULA", compartilhadaOferta.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static DataTable Listar(int idCompartilhada)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  co.id_compartilhada_oferta,
                            co.id_compartilhada,
                            co.curso,
                            co.turno,
                            co.matricula,
                            co.dt_cadastro,
                            co.dt_alteracao,
                            md.descricao + ' / ' + tc.descricao + ' / ' + c.nome AS mod_seg_curso,
                            cm.rede_ensino
                    FROM    TCE_COMPARTILHADA_OFERTA co
                            INNER JOIN dbo.LY_CURSO c ON co.CURSO = c.CURSO
                            INNER JOIN dbo.LY_MODALIDADE_CURSO md ON c.MODALIDADE = md.MODALIDADE
                            INNER JOIN dbo.LY_TIPO_CURSO tc ON c.TIPO = tc.TIPO
                            INNER JOIN dbo.TCE_COMPARTILHADA cm ON cm.ID_COMPARTILHADA = co.ID_COMPARTILHADA
                    WHERE   co.id_compartilhada = @ID_COMPARTILHADA", 
                    new ContextQueryParameter("@ID_COMPARTILHADA", idCompartilhada)));
        }

        public static DataTable ListarEstaduais(int idCompartilhada)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  mc.descricao + ' / ' + tp.descricao + ' / ' + c.nome AS 'mod_seg_curso',
                            t.descricao AS 'turno'
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            INNER JOIN LY_CURSO c ON uc.curso = c.curso
                            INNER JOIN LY_TURNO t ON uc.turno = t.turno
                            INNER JOIN dbo.TCE_COMPARTILHADA co ON co.censo_compartilhada = uc.unidade_ens
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.modalidade = mc.modalidade
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.tipo = c.tipo
                    WHERE   co.id_compartilhada = @ID_COMPARTILHADA
                    ORDER BY mc.descricao ASC,
                            tp.descricao ASC,
                            c.nome ASC,
                            t.descricao ASC", 
                    new ContextQueryParameter("@ID_COMPARTILHADA", idCompartilhada)));
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  TCE_COMPARTILHADA_OFERTA
                    WHERE   ID_COMPARTILHADA_OFERTA = @ID_COMPARTILHADA_OFERTA");

            contextQuery.Parameters.Add("@ID_COMPARTILHADA_OFERTA", id);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados Validar(TceCompartilhadaOferta compartilhadaOferta)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var mensagens = new List<string>();
                var validacaoDados = new ValidacaoDados();

                if (compartilhadaOferta == null)
                {
                    return validacaoDados;
                }

                if (string.IsNullOrEmpty(compartilhadaOferta.Curso))
                {
                    mensagens.Add("O campo CURSO é obrigatório!");
                }

                if (string.IsNullOrEmpty(compartilhadaOferta.Turno))
                {
                    mensagens.Add("O campo TURNO é obrigatório!");
                }

                var quantidade = ctx.GetReturnValue<int>(
                    new ContextQuery(
                        @"SELECT  COUNT(*)
                            FROM    TCE_COMPARTILHADA_OFERTA
                            WHERE   ID_COMPARTILHADA = @ID_COMPARTILHADA
                                    AND CURSO = @CURSO
                                    AND TURNO = @TURNO ",
                        new ContextQueryParameter("@ID_COMPARTILHADA", compartilhadaOferta.IdCompartilhada), 
                        new ContextQueryParameter("@CURSO", compartilhadaOferta.Curso), 
                        new ContextQueryParameter("@TURNO", compartilhadaOferta.Turno)));

                if (quantidade > 0)
                {
                    mensagens.Add(
                        string.Format(
                            "Já existe uma OFERTA cadastrada no Curso {0} do turno {1}.", 
                            compartilhadaOferta.Curso, 
                            compartilhadaOferta.Turno));
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
        }


        public bool PossuiOfertaPor(DataContext contexto, int id)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                 FROM    TCE_COMPARTILHADA_OFERTA
                    WHERE   ID_COMPARTILHADA = @ID_COMPARTILHADA ";

            contextQuery.Parameters.Add("@ID_COMPARTILHADA", id);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}