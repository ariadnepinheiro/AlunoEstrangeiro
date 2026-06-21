using System;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using System.Data;
using System.Web;
using Seeduc.Infra.Data;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class DocenteGLP : RNBase
    {
        public DataTable ListaSolicitacaoPor(string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT id_docente_funcao_glp,
                                                        df.matricula,
                                                        IDFUNCIONAL,
                                                        D.VINCULO,
                                                        (CONVERT (VARCHAR, IDFUNCIONAL) + '/' + CONVERT (VARCHAR, D.VINCULO)) AS IDVINCULO,
                                                        df.prazo,
                                                        pe.nome_compl,
                                                        ue.nome_comp,
                                                        di.descricao,
                                                        funcao_glp,
                                                        f.descricao                                 as segmento,
                                                        ano,
                                                        mes,
                                                        data,
                                                        status,
                                                        df.unidade_ens,
                                                        df.agrupamento,
                                                        convert(integer, isnull(glp_solicitada, 0)) glp_solicitada,
                                                        convert(integer, isnull(glp_usada, 0))      glp_usada,
                                                        convert(integer, isnull(glp_cancelada, 0))  glp_cancelada
                                        FROM   LY_DOCENTE_FUNCAO_GLP DF 
                                               INNER JOIN LY_DOCENTE D 
                                                       ON( D.MATRICULA = DF.MATRICULA ) 
                                               INNER JOIN LY_PESSOA PE (NOLOCK) 
                                                       ON ( PE.PESSOA = D.PESSOA ) 
                                               INNER JOIN LY_FUNCAO F 
                                                       ON( F.FUNCAO = DF.FUNCAO_GLP ) 
                                               INNER JOIN LY_GRUPO_HABILITACAO DI 
                                                       ON( DI.AGRUPAMENTO = DF.AGRUPAMENTO ) 
                                               INNER JOIN LY_UNIDADE_ENSINO UE 
                                                       ON( UE.UNIDADE_ENS = DF.UNIDADE_ENS ) 
                                        WHERE  DF.STATUS = 'Aguardando' 
                                               AND UE.SETOR = @SETOR 
                                        ORDER  BY DATA ASC  ";

                contextQuery.Parameters.Add("@SETOR", TechneDbType.T_CODIGO, setor);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

            return dt;
        }

        public ValidacaoDados ValidaSolicitacao(Entidades.LyDocenteFuncaoGlp docenteFuncaoGlp, List<RecursosHumanos.Entidades.DocenteFuncaoGlpTurma> listaDocenteFuncaoGlpTurma, decimal numFunc, decimal pessoa)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            bool ehDocente;
            int cargaHorariaTotal = 0;
            int numeroMatriculas = 0;
            int cargaHorariaSemanal = 0;
            int cargaHorariaEmAula = 0;
            string categoria = string.Empty;
            string categoriaSegundaMatricula = string.Empty;
            string segundaMatricula = string.Empty;
            int agrupamentoCargo = 0;
            int agrupamentoCargoSegundaMatricula = 0;
            int pedidosPendentes = 0;
            AulaDocente rnAulaDocente = new AulaDocente();
            LyFuncao funcao = new LyFuncao();
            CategoriaDocente rnCategoriaDocente = new CategoriaDocente();
            RecursosHumanos.Entidades.ChGlp chGlp = new RecursosHumanos.Entidades.ChGlp();
            RecursosHumanos.ChGlp rnChGlp = new Techne.Lyceum.RN.RecursosHumanos.ChGlp();
            Docentes rnDocentes = new Docentes();
            Funcao rnFuncao = new Funcao();
            Lotacao rnLotacao = new Lotacao();
            RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo = new Techne.Lyceum.RN.RecursosHumanos.ChAgrupamentoCargo();
            LyLicencaDocente licenca = new LyLicencaDocente();
            LyLicencaPessoa licencaPessoa = new LyLicencaPessoa();
            LyLotacao lotacao = new LyLotacao();
            LyLotacao lotacaoSegundaMatricula = new LyLotacao();
            LicencaDocente rnLicencaDocente = new LicencaDocente();
            LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
            VinculoLy rnVinculoLy = new VinculoLy();
            int glpMatricula = 0;
            int glpSegundaMatricula = 0;

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteFuncaoGlp == null)
            {
                return validacaoDados;
            }

            if (listaDocenteFuncaoGlpTurma == null)
            {
                return validacaoDados;
            }

            if (docenteFuncaoGlp.Matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo DOCENTE é obrigatório.");
            }

            if (docenteFuncaoGlp.Agrupamento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo GRUPO DE DISCIPLINAS é obrigatório.");
            }

            if (docenteFuncaoGlp.UnidadeEns.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (docenteFuncaoGlp.FuncaoGlp.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo FUNÇÃO é obrigatório.");
            }

            if (docenteFuncaoGlp.GlpSolicitada < 0)
            {
                mensagens.Add("O campo CH SOLICITADA é obrigatório.");
            }

            if (docenteFuncaoGlp.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório..");
            }

            if (numFunc <= 0)
            {
                mensagens.Add("O campo CODIGO DO DOCENTE é obrigatório..");
            }

            if (pessoa <= 0)
            {
                mensagens.Add("O campo PESSOA é obrigatório..");
            }

            if (docenteFuncaoGlp.Mes <= 0)
            {
                mensagens.Add("O campo MÊS é obrigatório.");
            }

            if (docenteFuncaoGlp.Status.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo SITUAÇÃO é obrigatório.");
            }

            if (listaDocenteFuncaoGlpTurma.Count == 0)
            {
                mensagens.Add("É obrigatório informar ao menos 1 TURMA.");
            }
            else
            {
                foreach (RecursosHumanos.Entidades.DocenteFuncaoGlpTurma turma in listaDocenteFuncaoGlpTurma)
                {
                    if (turma.Turma.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo TURMA é obrigatório.");
                    }

                    if (turma.Disciplina.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo DISCIPLINA é obrigatório.");
                    }

                    if (turma.Ano <= 0)
                    {
                        mensagens.Add("O campo ANO é obrigatório.");
                    }
                    else if (turma.Ano != docenteFuncaoGlp.Ano)
                    {
                        mensagens.Add("Apenas podem ser selecionadas turmas do ano atual.");
                    }

                    if (turma.Periodo < 0)
                    {
                        mensagens.Add("O campo PERÍODO é obrigatório.");
                    }

                    if (turma.NumFuncCarencia <= 0)
                    {
                        mensagens.Add("O campo TIPO DE CARÊNCIA é obrigatório.");
                    }

                    if (turma.CargaHoraria <= 0)
                    {
                        mensagens.Add("O campo CARGA HORÁRIA é obrigatório.");
                    }
                    else
                    {
                        cargaHorariaTotal += turma.CargaHoraria;
                    }
                }
            }

            if (cargaHorariaTotal != docenteFuncaoGlp.GlpSolicitada)
            {
                mensagens.Add("O campo CH SOLICITADA deve ser igual a soma das cargas horárias das turmas.");
            }

            if (docenteFuncaoGlp.UsuarioSolicitacaoId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    docenteFuncaoGlp.Prazo = 0; //Não possui prazo final

                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se é matricula de contrato
                    if (rnDocentes.EhMatriculaContratoPor(contexto, numFunc.ToString()))
                    {
                        mensagens.Add("Não é permitido GLP para Contrato Temporário.");
                    }

                    foreach (RecursosHumanos.Entidades.DocenteFuncaoGlpTurma turma in listaDocenteFuncaoGlpTurma)
                    {
                        //valida se já existe pedido igual pendente
                        if (this.ExistePedidoPendentePor(contexto, Convert.ToDecimal(docenteFuncaoGlp.Ano), Convert.ToDecimal(docenteFuncaoGlp.Mes), docenteFuncaoGlp.UnidadeEns, turma.Turma, turma.Disciplina))
                        {
                            mensagens.Add(string.Format("Pedido já foi realizado anteriormente para a turma {0} - Disciplina {1}, aguarde a resposta.", turma.Turma, turma.Disciplina));
                        }
                    }

                    //Busca quantidade de matrículas do docente
                    numeroMatriculas = rnLotacao.ObtemNumeroMatriculaAtivaPor(contexto, pessoa);

                    //Busca lotação ativa
                    lotacao = rnLotacao.ObtemLotacaoAtivaPor(contexto, docenteFuncaoGlp.Matricula);

                    if (lotacao.Pessoa == 0)
                    {
                        mensagens.Add("Este pedido não pode ser feito pois o docente não possui lotação ativa.");
                    }

                    if (numeroMatriculas != 1 && numeroMatriculas != 2)
                    {
                        mensagens.Add("O Docente deve ter no mínimo 1 e no máximo 2 matriculas.");
                    }

                    //Verifica se o segmento de atuação permite glp
                    if (!rnFuncao.PermiteGlpPor(contexto, docenteFuncaoGlp.FuncaoGlp))
                    {
                        mensagens.Add("Este pedido não pode ser feito pois o Segmento de Atuação não permite GLP.");
                    }

                    if (mensagens.Count == 0)
                    {
                        //Busca licença Ativa
                        licenca = rnLicencaDocente.ObtemLicencaAtivaPor(contexto, docenteFuncaoGlp.Matricula);

                        if (numeroMatriculas == 1)
                        {
                            // Não permitir professores docentes com uma matrícula: Em atividade extraclasse, exceto a função NAPES. 
                            //(I. Tela de função, através dos campos “Permite GLP”)
                            if (!rnFuncao.PermiteGlpPor(contexto, lotacao.Funcao))
                            {
                                mensagens.Add("Este pedido não pode ser feito pois a função da lotação não permite GLP.");
                            }

                            // Não permitir professores docentes com uma matrícula: lotados em Unidades Administrativas;
                            //(II.	Tela de lotação e situação de servidores e funcionários)
                            if (!rnLotacao.PossuiLotacaoAtivaEscolaPor(contexto, docenteFuncaoGlp.Matricula))
                            {
                                mensagens.Add("Este pedido não pode ser feito pois o docente não está lotada em uma Escola.");
                            }

                            // Não permitir professores docentes com uma matrícula: lotados em Unidades de Ensino não ativas;
                            //(III.	Tela de “Unidades de Ensino”)
                            if (!rnLotacao.PossuiLotacaoAtivaEscolaEmAtividadePor(contexto, docenteFuncaoGlp.Matricula))
                            {
                                mensagens.Add("Este pedido não pode ser feito pois o docente não está lotada em uma Escola Ativa.");
                            }

                            //Verifica se possui licença
                            if (licenca.NumFunc > 0)
                            {
                                // Não permitir professores docentes com uma matrícula: O professor que tiver qualquer afastamento.
                                //(IV.	Tela de lotação e situação de servidores e funcionários.)                                
                                mensagens.Add("Este pedido não pode ser feito pois o docente possui afastamento.");
                            }
                        }

                        if (mensagens.Count == 0)
                        {
                            //Busca categoria do docente
                            categoria = rnDocentes.ObtemCategoriaPor(contexto, docenteFuncaoGlp.Matricula);

                            //Busca Carga horaria semanal de regencia para a categoria e funcao
                            cargaHorariaSemanal = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(contexto, categoria, lotacao.Funcao);

                            if (cargaHorariaSemanal > 0)
                            {
                                //Busca Carga Horaria em aula
                                cargaHorariaEmAula = rnAulaDocente.ObtemQuantidadeAulasNormaisAtivasDocentePor(contexto, docenteFuncaoGlp.Matricula);

                                //Verifica se possui Carga horaria livre
                                if (cargaHorariaSemanal > cargaHorariaEmAula)
                                {
                                    //Não permitir professores docentes com uma ou duas matrículas e que possuem carga horária livre ou incompleta;
                                    //(I. Tela de turmas.)
                                    mensagens.Add("Este pedido não pode ser feito pois o docente possui carga horária livre ou incompleta.");
                                }
                            }

                            //Busca dados da funcao
                            funcao = rnFuncao.ObtemPor(contexto, lotacao.Funcao);

                            //Não permitir professores docentes com uma ou duas matrículas em função gratificada ou cargo em comissão, exceto as funções de Secretário, 
                            //Agente de Pessoal, Auxiliar de Agente de Pessoal, Coordenador Pedagógico, Orientador Educacional, Assistente Operacional Escolar, 
                            //que poderão ampliar a jornada de trabalho em regime de GLP na segunda matrícula desde que atuem como regente e não haja conflito de horários 
                            //e prejuízo ao desenvolvimento das atribuições inerentes à função
                            //(II. Tela de função, através dos campos “Tipo” e “Libera GLP na 2ª matrícula”.)
                            if (funcao.FuncaoBB.ToUpper() == "COMISSIONADA" && funcao.FuncaoBB.ToUpper() == "GRATIFICADA")
                            {
                                if (funcao.Campo03 != "S")
                                {
                                    mensagens.Add("Este pedido não pode ser feito pois o docente possui lotação em função gratificada ou cargo em comissão que não liberam GLP na 2ª matrícula .");
                                }
                            }

                            //Não permitir professores docentes com uma ou duas matrículas readaptados em outra função; 
                            //(III.	Tela de lotação e situação de servidores e funcionários;)
                            if (lotacao.Readaptado == "S")
                            {
                                mensagens.Add("Este pedido não pode ser feito pois o docente possui readaptação.");
                            }

                            //Verifica se docente possui licença de CH Reduzida
                            if (licenca.NumFunc > 0 && licenca.Motivo == "43")
                            {
                                //Não permitir professores docentes com uma ou duas matrículas com redução de carga horária;
                                //(IV. Tela de lotação e situação de servidores e funcionários;)
                                mensagens.Add("Este pedido não pode ser feito pois o docente possui redução de carga horária.");
                            }

                            //Não permitir professores docentes com uma ou duas matrículas regentes com complementação de carga horária em outras atividades;
                            //(V. Tela de função, através dos campos “Permite GLP”).           
                            if (funcao.Campo07 != "S")
                            {
                                mensagens.Add("Este pedido não pode ser feito pois o docente está lotado em um função que não Permite GLP.");
                            }

                            //Apenas continuar caso não exista erros anteriores
                            if (mensagens.Count == 0)
                            {
                                //Busca agrupamento do cargo
                                agrupamentoCargo = rnCategoriaDocente.ObtemAgrupamentoCargoPor(contexto, categoria);

                                if (numeroMatriculas == 2)
                                {
                                    //Buscar Segunda matricula do servidor
                                    segundaMatricula = rnLotacao.ObtemSegundaMatriculaAtivaPor(contexto, docenteFuncaoGlp.Matricula, pessoa);

                                    ehDocente = rnDocentes.EhMatriculaDocentePor(contexto, segundaMatricula);

                                    //Busca lotação ativa da segunda matricula
                                    lotacaoSegundaMatricula = rnLotacao.ObtemLotacaoAtivaPor(contexto, segundaMatricula);

                                    if (ehDocente)
                                    {
                                        //Busca licença Ativa da segunda matricula
                                        licenca = rnLicencaDocente.ObtemLicencaAtivaPor(contexto, segundaMatricula);

                                        //Busca categoria do docente da segunda matricula
                                        categoriaSegundaMatricula = rnDocentes.ObtemCategoriaPor(contexto, segundaMatricula);

                                        //Busca Carga horaria semanal de regencia da segunda matricula
                                        cargaHorariaSemanal = rnChAgrupamentoCargo.ObtemCargaHorariaRegenciaPor(contexto, categoriaSegundaMatricula, lotacaoSegundaMatricula.Funcao);

                                        //Caso não tenha licença
                                        if (licenca.NumFunc == 0)
                                        {
                                            //Verifica se a segunda matricula tem carga horaria de regencia
                                            if (cargaHorariaSemanal > 0)
                                            {
                                                //Busca Carga Horaria em aula da segunda matricula
                                                cargaHorariaEmAula = rnAulaDocente.ObtemQuantidadeAulasNormaisAtivasDocentePor(contexto, segundaMatricula);

                                                //Verifica se possui Carga horaria livre na segunda matricula 
                                                if (cargaHorariaSemanal > cargaHorariaEmAula)
                                                {
                                                    //Não permitir professores docentes com uma ou duas matrículas e que possuem carga horária livre ou incompleta;
                                                    //(I. Tela de turmas.)
                                                    mensagens.Add("Este pedido não pode ser feito pois o docente possui carga horária livre ou incompleta na segunda matricula.");
                                                }
                                            }
                                        }

                                        //Verifica se docente possui licença de CH Reduzida
                                        if (licenca.NumFunc > 0 && licenca.Motivo == "43")
                                        {
                                            //Não permitir professores docentes com uma ou duas matrículas com redução de carga horária;
                                            //(IV. Tela de lotação e situação de servidores e funcionários;)
                                            mensagens.Add("Este pedido não pode ser feito pois o docente possui redução de carga horária na segunda matricula.");
                                        }
                                    }
                                    else
                                    {
                                        //Busca licença Ativa da segunda matricula
                                        licencaPessoa = rnLicencaPessoa.ObtemLicencaAtivaPor(contexto, pessoa);

                                        //Verifica se docente possui licença de CH Reduzida
                                        if (licencaPessoa.Pessoa > 0 && licencaPessoa.Motivo == "43")
                                        {
                                            //Não permitir professores docentes com uma ou duas matrículas com redução de carga horária;
                                            //(IV. Tela de lotação e situação de servidores e funcionários;)
                                            mensagens.Add("Este pedido não pode ser feito pois o servidor possui redução de carga horária na segunda matricula.");
                                        }

                                        //Busca categoria do docente da segunda matricula
                                        categoriaSegundaMatricula = rnVinculoLy.ObtemCategoriaPor(contexto, segundaMatricula);
                                    }

                                    if (lotacao.Pessoa > 0)
                                    {
                                        //Busca dados da funcao da segunda matricula
                                        funcao = rnFuncao.ObtemPor(contexto, lotacaoSegundaMatricula.Funcao);

                                        //Não permitir professores docentes com uma ou duas matrículas em função gratificada ou cargo em comissão, exceto as funções de Secretário, 
                                        //Agente de Pessoal, Auxiliar de Agente de Pessoal, Coordenador Pedagógico, Orientador Educacional, Assistente Operacional Escolar, 
                                        //que poderão ampliar a jornada de trabalho em regime de GLP na segunda matrícula desde que atuem como regente e não haja conflito de horários 
                                        //e prejuízo ao desenvolvimento das atribuições inerentes à função
                                        //(II. Tela de função, através dos campos “Tipo” e “Libera GLP na 2ª matrícula”.)
                                        if (!funcao.FuncaoBB.IsNullOrEmptyOrWhiteSpace())
                                        {
                                            if (funcao.FuncaoBB.ToUpper() == "COMISSIONADA" && funcao.FuncaoBB.ToUpper() == "GRATIFICADA")
                                            {
                                                if (funcao.Campo03 != "S")
                                                {
                                                    mensagens.Add("Este pedido não pode ser feito pois o docente possui lotação na segunda matricula em função gratificada ou cargo em comissão que não liberam GLP na 2ª matrícula.");
                                                }
                                            }
                                        }

                                        //Não permitir professores docentes com uma ou duas matrículas readaptados em outra função; 
                                        //(III.	Tela de lotação e situação de servidores e funcionários;)
                                        if (lotacaoSegundaMatricula.Readaptado == "S")
                                        {
                                            mensagens.Add("Este pedido não pode ser feito pois o docente possui readaptação na segunda matricula.");
                                        }

                                        //Não permitir professores docentes com uma ou duas matrículas regentes com complementação de carga horária em outras atividades;
                                        //(V. Tela de função, através dos campos “Permite GLP”).           
                                        if (funcao.Campo03 != "S")
                                        {
                                            mensagens.Add("Este pedido não pode ser feito pois a segunda matrícula do docente está lotada em uma função que não Libera GLP na 2ª matrícula.");
                                        }
                                    }



                                    //Busca agrupamento do cargo da segunda matricula
                                    agrupamentoCargoSegundaMatricula = rnCategoriaDocente.ObtemAgrupamentoCargoPor(contexto, categoriaSegundaMatricula);

                                    if (agrupamentoCargoSegundaMatricula == 0)
                                    {
                                        mensagens.Add("Este pedido não pode ser feito pois o cargo da segunda matrícula consta sem agrupamento associado.");
                                    }
                                }

                                //Busca quantidade de GLP permitida para as categorias + funçoes das matriculas do docente
                                chGlp = rnChGlp.ObtemPor(contexto, numeroMatriculas, agrupamentoCargo, lotacao.Funcao, agrupamentoCargoSegundaMatricula, lotacaoSegundaMatricula.Funcao);

                                //Busca quantidade de GLPs em pedidos pendentes
                                pedidosPendentes = this.ObtemQuantidadeTotalPendente(contexto, docenteFuncaoGlp.Matricula);
                                if (numeroMatriculas == 2)
                                {
                                    pedidosPendentes = pedidosPendentes + this.ObtemQuantidadeTotalPendente(contexto, segundaMatricula);
                                }

                                if (chGlp.ChGlpId == 0)
                                {
                                    mensagens.Add("Este pedido não pode ser feito pois não existe carga horaria de glp cadastrada para o grupo de categoria / função de suas matriculas.");
                                }
                                else
                                {
                                    glpMatricula = ObtemQuantidadeTotalPendenteEAceito(contexto, docenteFuncaoGlp.Matricula, Convert.ToInt32(docenteFuncaoGlp.Ano));

                                    if (!segundaMatricula.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        glpSegundaMatricula = ObtemQuantidadeTotalPendenteEAceito(contexto, segundaMatricula, Convert.ToInt32(docenteFuncaoGlp.Ano));
                                    }

                                    //Valida qtd máxima de pedidos de GPL
                                    if (chGlp.Ch_Glp == 0)
                                    {
                                        mensagens.Add("Este pedido não pode ser feito, pois a carga horária para o grupo de categoria/função de suas matrículas já atingiu o limite de 65h.");
                                    }
                                    else
                                    {
                                        //Atualiza id encontrado
                                        docenteFuncaoGlp.ChGlpId = chGlp.ChGlpId;

                                        //Verifica que a quantidade de glp solicitada + outros pedidos pendentes é maior que a ch de glp permitida
                                        if (docenteFuncaoGlp.GlpSolicitada + pedidosPendentes > chGlp.Ch_Glp)
                                        {
                                            mensagens.Add("GLP não permitida. Total solicitado não pode ultrapassar " + chGlp.Ch_Glp + " tempo(s). Já existe(m) " + pedidosPendentes + " tempo(s) pendente(s).");

                                        }

                                        if (docenteFuncaoGlp.GlpSolicitada + (glpMatricula + glpSegundaMatricula) > chGlp.Ch_Glp)
                                        {
                                            mensagens.Add("Esta solicitação não pode ser realizada, pois ultrapassou o total de GLP permitida.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Solicita(Entidades.LyDocenteFuncaoGlp docenteFuncaoGlp, List<RecursosHumanos.Entidades.DocenteFuncaoGlpTurma> listaDocenteFuncaoGlpTurma)
        {
            RN.RecursosHumanos.DocenteFuncaoGlpTurma rnDocenteFuncaoGlpTurma = new Techne.Lyceum.RN.RecursosHumanos.DocenteFuncaoGlpTurma();
            Entidades.LyDocenteFuncaoGlpDetalhe docenteFuncaoGlpDetalhe = new LyDocenteFuncaoGlpDetalhe();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                //Inclui na tabela LYDOCENTEFUNCAOGLP
                this.Insere(contexto, docenteFuncaoGlp);

                foreach (RecursosHumanos.Entidades.DocenteFuncaoGlpTurma turma in listaDocenteFuncaoGlpTurma)
                {
                    //Atualiza com id gerado
                    turma.IdDocenteFuncaoGlp = docenteFuncaoGlp.IdDocenteFuncaoGlp;

                    //Inclui na tabela docenteFuncaoGlpTurma
                    rnDocenteFuncaoGlpTurma.Insere(contexto, turma);
                }

                //Monta histórico
                docenteFuncaoGlpDetalhe.IdDocenteFuncaoGlp = docenteFuncaoGlp.IdDocenteFuncaoGlp;
                docenteFuncaoGlpDetalhe.Status = docenteFuncaoGlp.Status;
                docenteFuncaoGlpDetalhe.QtdGlp = docenteFuncaoGlp.GlpSolicitada;
                docenteFuncaoGlpDetalhe.Usuario = docenteFuncaoGlp.UsuarioSolicitacaoId;
                docenteFuncaoGlpDetalhe.Motivo = null;

                //Verifica se histórico já existe o historico 
                if (!this.ExisteHistoricoPor(contexto, docenteFuncaoGlpDetalhe))
                {
                    //Caso não exista inclui na tabela LYDOCENTEFUNCAOGLPSDETALHE
                    this.InsereHistorico(contexto, docenteFuncaoGlpDetalhe);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        private void Insere(DataContext contexto, Entidades.LyDocenteFuncaoGlp docenteFuncaoGlp)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_DOCENTE_FUNCAO_GLP 
                                            (MATRICULA, 
                                             FUNCAO_GLP, 
                                             ANO, 
                                             MES, 
                                             STATUS, 
                                             UNIDADE_ENS, 
                                             DATA, 
                                             GLP_SOLICITADA, 
                                             AGRUPAMENTO, 
                                             PRAZO, 
                                             USUARIOSOLICITACAOID, 
                                             DATASOLICITACAO,
                                             CH_GLPID) 
                                VALUES      (@MATRICULA, 
                                             @FUNCAO_GLP, 
                                             @ANO, 
                                             @MES, 
                                             @STATUS, 
                                             @UNIDADE_ENS, 
                                             @DATASOLICITACAO, 
                                             @GLP_SOLICITADA, 
                                             @AGRUPAMENTO, 
                                             @PRAZO, 
                                             @USUARIOSOLICITACAOID, 
                                             @DATASOLICITACAO,
                                             @CH_GLPID) 

                                SELECT IDENT_CURRENT('DBO.LY_DOCENTE_FUNCAO_GLP') ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_ALFALARGE, docenteFuncaoGlp.Matricula);
            contextQuery.Parameters.Add("@FUNCAO_GLP", TechneDbType.T_CODIGO, docenteFuncaoGlp.FuncaoGlp);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, docenteFuncaoGlp.Ano);
            contextQuery.Parameters.Add("@MES", TechneDbType.T_MES, docenteFuncaoGlp.Mes);
            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, docenteFuncaoGlp.Status);
            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, docenteFuncaoGlp.UnidadeEns);
            contextQuery.Parameters.Add("@GLP_SOLICITADA", TechneDbType.T_DECIMAL_MEDIO, docenteFuncaoGlp.GlpSolicitada);
            contextQuery.Parameters.Add("@AGRUPAMENTO", TechneDbType.T_ALFAMEDIUM, docenteFuncaoGlp.Agrupamento);
            contextQuery.Parameters.Add("@PRAZO", TechneDbType.T_NUMERO_PEQUENO, docenteFuncaoGlp.Prazo);
            contextQuery.Parameters.Add("@USUARIOSOLICITACAOID", SqlDbType.VarChar, docenteFuncaoGlp.UsuarioSolicitacaoId);
            contextQuery.Parameters.Add("@DATASOLICITACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@CH_GLPID", SqlDbType.Int, docenteFuncaoGlp.ChGlpId);

            docenteFuncaoGlp.IdDocenteFuncaoGlp = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private bool ExisteHistoricoPor(DataContext contexto, Entidades.LyDocenteFuncaoGlpDetalhe docenteFuncaoGlpDetalhe)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM   LY_DOCENTE_FUNCAO_GLP_DETALHE 
                                WHERE  ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP 
                                       AND DATA = @DATA 
                                       AND STATUS = @STATUS 
                                       AND USUARIO = @USUARIO ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", TechneDbType.T_NUMERO, docenteFuncaoGlpDetalhe.IdDocenteFuncaoGlp);
            contextQuery.Parameters.Add("@DATA", TechneDbType.T_DATA, DateTime.Now);
            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, docenteFuncaoGlpDetalhe.Status);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, docenteFuncaoGlpDetalhe.Usuario);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhPedidoPendentePor(DataContext contexto, decimal idDocenteFuncaoGlp)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM   LY_DOCENTE_FUNCAO_GLP 
                                WHERE  ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP 
                                       AND STATUS = 'Aguardando' ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", idDocenteFuncaoGlp);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemQuantidadeTotalPendente(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT SUM(GLP_SOLICITADA) as GLP_SOLICITADA
                            FROM LY_DOCENTE_FUNCAO_GLP (NOLOCK)
                            WHERE STATUS = 'Aguardando' 
									AND MATRICULA = @MATRICULA";

                contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_ALFALARGE, matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["GLP_SOLICITADA"] == DBNull.Value ? 0 : Convert.ToInt32(reader["GLP_SOLICITADA"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        private void InsereHistorico(DataContext contexto, Entidades.LyDocenteFuncaoGlpDetalhe docenteFuncaoGlpDetalhe)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_DOCENTE_FUNCAO_GLP_DETALHE 
                                                    (ID_DOCENTE_FUNCAO_GLP, 
                                                     DATA, 
                                                     STATUS, 
                                                     QTD_GLP, 
                                                     USUARIO, 
                                                     MOTIVO) 
                                        VALUES      (@ID_DOCENTE_FUNCAO_GLP,
                                                     @DATA,
                                                     @STATUS,
                                                     @QTD_GLP,
                                                     @USUARIO,
                                                     @MOTIVO ) ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", TechneDbType.T_NUMERO, docenteFuncaoGlpDetalhe.IdDocenteFuncaoGlp);
            contextQuery.Parameters.Add("@DATA", TechneDbType.T_DATA, DateTime.Now);
            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, docenteFuncaoGlpDetalhe.Status);
            contextQuery.Parameters.Add("@QTD_GLP", TechneDbType.T_DECIMAL_MEDIO, docenteFuncaoGlpDetalhe.QtdGlp);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, docenteFuncaoGlpDetalhe.Usuario);
            contextQuery.Parameters.Add("@MOTIVO", TechneDbType.T_ALFALARGE, docenteFuncaoGlpDetalhe.Motivo);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiAgrupamentoCargosPor(DataContext ctx, int chGlpId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM DBO.LY_DOCENTE_FUNCAO_GLP (NOLOCK)
                                WHERE CH_GLPID = @CH_GLPID ";

            contextQuery.Parameters.Add("@CH_GLPID", SqlDbType.Int, chGlpId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }


        public ValidacaoDados ValidaRemocaoSolicitacao(int idDocenteFuncaoGlp)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (idDocenteFuncaoGlp <= 0)
            {
                mensagens.Add("O CODIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o pedido continua aguardando
                    if (!this.EhPedidoPendentePor(contexto, idDocenteFuncaoGlp))
                    {
                        mensagens.Add("Este pedido já foi analisado.");
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void RemoveSolicitacao(int idDocenteFuncaoGlp)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RecursosHumanos.DocenteFuncaoGlpTurma rnDocenteFuncaoGlpTurma = new Techne.Lyceum.RN.RecursosHumanos.DocenteFuncaoGlpTurma();

            try
            {
                //Remove da docenteFuncaoGlpTurma 
                rnDocenteFuncaoGlpTurma.RemovePorIdDocenteFuncaoGlp(contexto, idDocenteFuncaoGlp);

                //Remove da LYDOCENTEFUNCAOGLPSDETALHE
                this.RemoveHistorico(contexto, idDocenteFuncaoGlp);

                //Remove da tabela LYDOCENTEFUNCAOGLP
                this.Remove(contexto, idDocenteFuncaoGlp);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        private void Remove(DataContext contexto, int idDocenteFuncaoGlp)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE DBO.LY_DOCENTE_FUNCAO_GLP 
                                            WHERE ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", TechneDbType.T_NUMERO, idDocenteFuncaoGlp);

            contexto.ApplyModifications(contextQuery);
        }

        private void RemoveHistorico(DataContext contexto, int idDocenteFuncaoGlp)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE DBO.LY_DOCENTE_FUNCAO_GLP_DETALHE 
                                            WHERE ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP ";

            contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", TechneDbType.T_NUMERO, idDocenteFuncaoGlp);

            contexto.ApplyModifications(contextQuery);
        }

        public static QueryTable ConsultarExclusaoDocenteGLP(string idregional, string unidade)
        {
            TConnection cn = Config.CreateConnection();

            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.Append(@"   SELECT DISTINCT ID_DOCENTE_FUNCAO_GLP,
                                            DF.MATRICULA,
                                            PE.NOME_COMPL,
                                            UE.NOME_COMP,
			                                IDFUNCIONAL,
                                            D.VINCULO,
                                            (CONVERT (VARCHAR, IDFUNCIONAL) + '/' + CONVERT (VARCHAR, D.VINCULO)) AS IDVINCULO,
                                            DI.DESCRICAO,
                                            FUNCAO_GLP,
                                            case when funcao_glp in (108,109) then 'Ensino Fundamental Anos Iniciais'  else 'Ensino Fundamental Anos Finais / Ensino Médio' end segmento_atuacao,
                                            ANO,
                                            MES,
                                            DATA,
                                            STATUS,
                                            DF.UNIDADE_ENS,
                                            DF.AGRUPAMENTO,
                                            CONVERT(INTEGER, ISNULL(GLP_SOLICITADA, 0)) glp_solicitada
                            FROM   LY_DOCENTE_FUNCAO_GLP DF
                                   INNER JOIN LY_DOCENTE D
                                           ON( D.MATRICULA = DF.MATRICULA )
	                               INNER JOIN LY_PESSOA PE 
				                            ON PE.PESSOA = D.PESSOA
                                   INNER JOIN LY_FUNCAO F
                                           ON( F.FUNCAO = DF.FUNCAO_GLP )
                                   INNER JOIN LY_GRUPO_HABILITACAO DI
                                           ON( DI.AGRUPAMENTO = DF.AGRUPAMENTO )
                                   INNER JOIN LY_UNIDADE_ENSINO UE
                                           ON( UE.UNIDADE_ENS = DF.UNIDADE_ENS )
                            WHERE  DF.STATUS = 'Aceita'
                                   AND UE.ID_REGIONAL = ?
                                   AND UE.UNIDADE_ENS = ?
                            ORDER  BY DATA ASC   ");

            QueryTable qt = new QueryTable(sql.ToString());

            if (!string.IsNullOrEmpty(idregional) && !string.IsNullOrEmpty(unidade))
                qt.Query(cn, idregional, unidade);

            return qt;
        }

        public DataTable ListaAnaliseDocenteGLPPor(int id_regional, string unidade)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new System.Text.StringBuilder();
            DataTable dt = null;

            try
            {
                sql.Append(@" SELECT DISTINCT ID_DOCENTE_FUNCAO_GLP,
                                            DF.MATRICULA,
											IDFUNCIONAL,
                                            DD.VINCULO,
                                            (CONVERT (VARCHAR, IDFUNCIONAL) + '/' + CONVERT (VARCHAR, DD.VINCULO)) AS IDVINCULO,
                                            DF.FUNCAO_GLP,
                                            PE.NOME_COMPL,
                                            UE.NOME_COMP,
                                            FUNCAO_GLP,
                                            ANO,
                                            MES,
                                            DATA,
                                            STATUS,
                                            UE.SETOR,
                                            DF.UNIDADE_ENS,
                                            DF.AGRUPAMENTO,
                                            DI.DESCRICAO                                DESCRICAO_DISCIPLINA,
                                            CONVERT(INTEGER, ISNULL(GLP_SOLICITADA, 0))
                                            GLP_SOLICITADA,
                                            ISNULL(CHLIVRE, 0)                          AS CH_LIVRE,
                                            ''                AS CH_LIVRE_MUNICIPIO,  --ISNULL(CHLIVREMUNICIPIO, 0) 
                                            RE.REGIONAL,
                                            MUN.NOME                                    AS MUNICIPIO
                                            ,F.DESCRICAO as SEGMENTO
                            FROM   LY_DOCENTE_FUNCAO_GLP DF (NOLOCK)
                                   JOIN LY_DOCENTE DD (NOLOCK)
                                     ON DD.MATRICULA = DF.MATRICULA
                                   INNER JOIN LY_PESSOA PE (NOLOCK)
                                           ON PE.PESSOA = DD.PESSOA
                                   JOIN LY_GRUPO_HABILITACAO DI (NOLOCK)
                                     ON DI.AGRUPAMENTO = DF.AGRUPAMENTO
                                   JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
                                     ON UE.UNIDADE_ENS = DF.UNIDADE_ENS
                                   JOIN MUNICIPIO MUN
                                     ON MUN.CODIGO = UE.MUNICIPIO
                                   JOIN TCE_REGIONAL RE
                                     ON RE.ID_REGIONAL = UE.ID_REGIONAL
                                     INNER JOIN LY_FUNCAO F 
                                        ON( F.FUNCAO = DF.FUNCAO_GLP ) 
                            WHERE  DF.STATUS = 'Aguardando'
                                   AND UE.ID_REGIONAL = @ID_REGIONAL
                                   AND DF.ANO = Datepart(YEAR, Getdate())
                                   AND DF.MES = Datepart(MONTH, Getdate())  ");

                if (!string.IsNullOrEmpty(unidade))
                {
                    sql.Append(" and ue.UNIDADE_ENS = @UNIDADE_ENS ");
                }

                sql.Append(" ORDER BY DATA ASC ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@ID_REGIONAL", id_regional);

                if (!string.IsNullOrEmpty(unidade))
                {
                    contextQuery.Parameters.Add("@UNIDADE_ENS", unidade);
                }

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

            return dt;
        }

        //Consulta agrupamentos de dispciplinas habilitadas para o docente pela matrícula
        public static QueryTable ConsultarAgrupamentosHabilitados(string numFunc)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = @"select DISTINCT gh.AGRUPAMENTO, gh.DESCRICAO
                            from LY_GRUPO_HABILITACAO gh
                            inner join ly_grupo_habilitacao_doc gd
                            on gd.AGRUPAMENTO = gh.AGRUPAMENTO
                            where exists (select 1 from ly_docente doc where doc.num_func = ? and doc.num_func = gd.NUM_FUNC 
                                    and ((provisorio = 'N') or (provisorio = 'S' and dt_limite >= convert(date,getdate()))) )";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, numFunc);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        public void AtualizaGlpUsadaPor(DataContext ctx, decimal numFunc, DateTime dataFimAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FG
                                SET    GLP_USADA = (SELECT Count(*)
                                                    FROM   LY_AULA_DOCENTE_TIPO AD
                                                    WHERE  AD.ID_DOCENTE_FUNCAO_GLP = FG.ID_DOCENTE_FUNCAO_GLP
                                                          AND DATA_FIM > CONVERT(DATE, @DATAFIMAULA) )
                               -- SELECT COUNT(*)
                                FROM   LY_DOCENTE_FUNCAO_GLP FG
	                                INNER JOIN  LY_AULA_DOCENTE_TIPO AD
                                                ON  AD.ID_DOCENTE_FUNCAO_GLP = FG.ID_DOCENTE_FUNCAO_GLP			                                    
                                WHERE DATA_FIM >= CONVERT(DATE, @DATAFIMAULA) 
	                                AND NUM_FUNC = @NUM_FUNC ";

            contextQuery.Parameters.Add("@DATAFIMAULA", dataFimAula);
            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

            ctx.ApplyModifications(contextQuery);
        }

        private bool ExistePedidoPendentePor(DataContext contexto, decimal ano, decimal mes, string unidadeEnsino, string turma, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM   LY_DOCENTE_FUNCAO_GLP  GLP (NOLOCK)
								       LEFT JOIN RECURSOSHUMANOS.DOCENTEFUNCAOGLP_TURMA GLPT  (NOLOCK) 
													ON GLP.ID_DOCENTE_FUNCAO_GLP = GLPT.ID_DOCENTE_FUNCAO_GLP
                                WHERE  GLP.ANO = @ANO 
                                       AND MES = @MES 
                                       AND UNIDADE_ENS = @UNIDADE_ENS 
									   AND GLPT.TURMA = @TURMA
									   AND GLPT.DISCIPLINA = @DISCIPLINA									  
                                       AND STATUS = 'Aguardando' ";

            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@MES", TechneDbType.T_MES, mes);
            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeEnsino);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        //verifica se o docente já está alocado para disciplina como GLP antes de reprová-la
        public static bool ExisteAlocacaoParaSolcicitacao(TConnection connection, decimal? id_docente_funcao_glp)
        {
            string sql = @"select 1 from LY_AULA_DOCENTE_TIPO where ID_DOCENTE_FUNCAO_GLP = ? and tipo_aula = 'GLP'";
            int retorno = ExecutarFuncao(sql, connection, id_docente_funcao_glp);

            if (retorno == 1)
                return false;
            else
                return true;
        }

        public static RetValue ReprovarGLPAceita(Ly_docente_funcao_glp.Row row)
        {
            RetValue retorno;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                //alocação ativa tipo_aula = 'GLP'
                if (ExisteAlocacaoParaSolcicitacao(connection, row.Id_docente_funcao_glp))
                {
                    Ly_docente_funcao_glp.Row.Update(connection, row.Id_docente_funcao_glp, "Status, Data, Ano, Mes, Glp_usada, Glp_cancelada", row.Status, row.Data, DateTime.Today.Year, DateTime.Today.Month, row.Glp_usada, row.Glp_cancelada);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null)
                    {
                        connection.Rollback();
                        return retorno;
                    }

                    #region Histórico
                    Techne.Lyceum.CR.Ly_docente_funcao_glp_detalhe.Row rowDetalhe = new Techne.Lyceum.CR.Ly_docente_funcao_glp_detalhe().NewRow();
                    rowDetalhe.Id_docente_funcao_glp = row.Id_docente_funcao_glp;
                    rowDetalhe.Status = "Cancelada";
                    rowDetalhe.Data = row.Data;
                    rowDetalhe.Qtd_glp = row.Glp_solicitada;
                    rowDetalhe.Usuario = Convert.ToString(HttpContext.Current.User.Identity.Name); ;

                    retorno = IncluiHistorico(connection, rowDetalhe);

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }
                    #endregion
                }
                else
                {
                    return new RetValue(false, "", new ErrorList("Solicitação não pode ser excluída pois existe alocação para essa solicitação."));
                }
            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Solicitação de GLP excluída com sucesso.", null);
        }

        public static RetValue ReprovarGLPEmAnalise(decimal? id, decimal? glp_solicitada, string motivo, string usuarioAnalise)
        {
            RetValue retorno;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_docente_funcao_glp.Row.Update(connection, id, "Status, Data, Ano, Mes,Usuarioanaliseid,Dataanalise", "Reprovado", DateTime.Today, DateTime.Today.Year, DateTime.Today.Month, usuarioAnalise, DateTime.Now);
                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }

                #region Histórico
                Techne.Lyceum.CR.Ly_docente_funcao_glp_detalhe.Row rowDetalhe = new Techne.Lyceum.CR.Ly_docente_funcao_glp_detalhe().NewRow();
                rowDetalhe.Id_docente_funcao_glp = id;
                rowDetalhe.Status = "Reprovado";
                rowDetalhe.Data = DateTime.Today;
                rowDetalhe.Qtd_glp = glp_solicitada;
                rowDetalhe.Usuario = Convert.ToString(HttpContext.Current.User.Identity.Name); ;
                rowDetalhe.Motivo = motivo;

                retorno = IncluiHistorico(connection, rowDetalhe);

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                #endregion
            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Solicitação de GLP reprovada com sucesso.", null);
        }

        //Verifica se existe saldo suficiente para aceitação de pedido de GLP
        public static bool ExisteSaldo(TConnectionWritable connection, decimal? ano, decimal? mes, decimal? qtd_glp, string funcao, Techne.DbObject tipo)
        {
            decimal? valor_incial = ObterValorInicialCompetencia(connection, ano, mes);

            if (valor_incial > 0)
            {
                decimal? total_disp = ConsultaTotalDisponibilizado(connection, ano, mes); //menos cancelado                
                decimal? valor_glp = ObterValorGLP(connection, ano, mes, funcao, tipo);
                decimal? saldo = valor_incial - total_disp;
                decimal? valor_solicitado = valor_glp * qtd_glp;

                if (valor_solicitado <= saldo)
                    return true;
            }
            return false;
        }

        public static bool ExisteSaldoValor(TConnectionWritable connection, decimal? ano, decimal? mes, decimal? qtd_glp, string funcao, Techne.DbObject tipo, out decimal? valor)
        {
            decimal? total_disp = ConsultaTotalDisponibilizado(connection, ano, mes); //menos cancelado
            valor = 0;

            if (total_disp > 0)
            {
                decimal? valor_incial = ObterValorInicialCompetencia(connection, ano, mes);
                decimal? valor_glp = ObterValorGLP(connection, ano, mes, funcao, tipo);
                decimal? saldo = valor_incial - total_disp;
                decimal? valor_solicitado = valor_glp * qtd_glp;

                valor = valor_glp;
                if (valor_solicitado <= saldo)
                    return true;
            }

            return false;

        }

        //Consulta valor da GLP para ano, mês e função
        public static decimal ObterValorGLP(TConnection connection, decimal? ano, decimal? mes, string funcao, Techne.DbObject tipo)
        {
            string sql = "select valor from LY_VALOR_GLP where ANO = ? and MES = ? and FUNCAO = ? and (TIPO = ? or TIPO is null)";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, ano, mes, funcao, tipo);
            if (!valorConsulta.IsNull)
                return Convert.ToDecimal(valorConsulta);
            else
                return 0M;
        }

        //Consulta valor da GLP para ano, mês e função
        public static decimal ObterValorInicialCompetencia(TConnection connection, decimal? ano, decimal? mes)
        {
            string sql = "select isnull(VALOR,0) from LY_VERBA_GLP_ANO_MES where ANO = ? and MES = ?";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, ano, mes);
            if (!valorConsulta.IsNull)
                return Convert.ToDecimal(valorConsulta);
            else
                return 0M;
        }

        //Consulta saldo disponível para ano e mês (valor inicial - total de pedidos ACEITOS)
        public static decimal ConsultaTotalDisponibilizado(TConnection connection, decimal? ano, decimal? mes)
        {
            string sql = @"DECLARE @ano int,
            @mes int 
            SET @ano = ?
            SET @mes = ?
                  
           DECLARE @tabelaValores AS TABLE        
            (        
             valor money,          
             funcao varchar(20),
             tipo varchar(20)    
            )             
                 
            INSERT INTO @tabelaValores select valor, funcao, tipo from LY_VALOR_GLP where ANO = @ano and MES = @mes       
                  
            select SUM(A.TOTAL_DISPONIBILIZADO) TOTAL_DISPONIBILIZADO
            from
            (
            select       
            df.ID_DOCENTE_FUNCAO_GLP,
            (SUM(isnull(df.GLP_SOLICITADA,0)) - SUM(isnull(df.GLP_CANCELADA,0))) * (select isnull(valor,0) from @tabelaValores where funcao = df.FUNCAO_GLP and (tipo = (select TIPO from LY_GRUPO_HABILITACAO gh where gh.AGRUPAMENTO = df.AGRUPAMENTO) or tipo is null)) TOTAL_DISPONIBILIZADO
            --from
            from LY_DOCENTE_FUNCAO_GLP df      
             inner join LY_DOCENTE_FUNCAO_GLP_DETALHE dfd      
             on (df.ID_DOCENTE_FUNCAO_GLP = dfd.ID_DOCENTE_FUNCAO_GLP)      
             where            
             dfd.STATUS = 'Aceita'      
             and df.STATUS = 'Aceita'
             and YEAR(dfd.data) = @ano
             and (convert(date,dfd.DATA+df.PRAZO) >= convert(date,GETDATE()) OR ISNULL(df.PRAZO, 0) = 0)

            group by df.FUNCAO_GLP, df.AGRUPAMENTO, df.ID_DOCENTE_FUNCAO_GLP
            ) A";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, ano, mes);
            if (!valorConsulta.IsNull)
                return Convert.ToDecimal(valorConsulta);
            else
                return 0M;
        }

        //aceitar solicitação de GLP
        public static RetValue AceitarGLP(Ly_docente_funcao_glp.Row row)
        {
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            RetValue retorno;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                //verifica se disciplina pode ser aceita
                if (IsDisciplinaBloqueada(connection, row.Agrupamento))
                {
                    return new RetValue(false, "", new ErrorList("Este pedido não pode ser aceito pois esta disciplina não pode ser aceita temporariamente."));
                }

                int carencia = ExisteCarencia(connection, row.Unidade_ens, row.Agrupamento);
                if (carencia < row.Glp_solicitada)
                {
                    //deborah
                    return new RetValue(false, "", new ErrorList("Este pedido não pode ser aceito pois a carência é de " + carencia + " tempo(s) para esta disciplina."));
                }

                string tipo = string.Empty;
                //obter o tipo do agrupamento pedido
                tipo = ObterTipoAgrupamento(connection, row.Agrupamento);

                if (!string.IsNullOrEmpty(tipo))
                {
                    if (!ExisteSaldo(connection, row.Ano, row.Mes, row.Glp_solicitada, row.Funcao_glp, tipo))
                    {
                        return new RetValue(false, "", new ErrorList("Este pedido não pode ser aceito pois não existe saldo disponível."));
                    }
                }
                else
                {
                    if (!ExisteSaldo(connection, row.Ano, row.Mes, row.Glp_solicitada, row.Funcao_glp, DBNull.Value))
                    {
                        return new RetValue(false, "", new ErrorList("Este pedido não pode ser aceito pois não existe saldo disponível."));
                    }
                }

                if (IsReadaptado(connection, row.Matricula))
                {
                    return new RetValue(false, "", new ErrorList("Este pedido não pode ser aceito pois o servidor é readaptado."));
                }

                //Verifica se a função da lotação ativa do docente permite GLP
                Ly_lotacao.Row lotacaoAtiva = RN.Turma.ObterLotacaoAtiva(connection, row.Matricula);
                if (lotacaoAtiva != null)
                {
                    if (!RN.Funcao.PermiteGLP(connection, lotacaoAtiva.Funcao))
                        return new RetValue(false, "", new ErrorList("Este pedido não pode ser aceito pois a função da lotação do servidor não permite GLP."));
                }

                if (rnDocenteGLP.ehSolicitacaoPendente(Convert.ToDecimal(row.Id_docente_funcao_glp)))
                {
                    Ly_docente_funcao_glp.Row.Update(connection, row.Id_docente_funcao_glp, "Status, Data, Ano, Mes  ,Usuarioanaliseid,Dataanalise", "Aceita", row.Data, row.Ano, row.Mes, row.Usuarioanaliseid, row.DataAnalise);

                }
                else
                {
                    return new RetValue(false, "", new ErrorList("Esta solicitação já se encontra analisada."));
                }

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }
                #region Histórico
                Techne.Lyceum.CR.Ly_docente_funcao_glp_detalhe.Row rowDetalhe = new Techne.Lyceum.CR.Ly_docente_funcao_glp_detalhe().NewRow();
                rowDetalhe.Id_docente_funcao_glp = row.Id_docente_funcao_glp;
                rowDetalhe.Status = "Aceita";
                rowDetalhe.Data = row.Data;
                rowDetalhe.Qtd_glp = row.Glp_solicitada;
                rowDetalhe.Usuario = Convert.ToString(HttpContext.Current.User.Identity.Name); ;

                retorno = IncluiHistorico(connection, rowDetalhe);

                if (retorno != null)
                {
                    connection.Rollback();
                    return retorno;
                }
                #endregion
            }
            catch (Exception ex)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Solicitação de GLP aceita com sucesso.", null);
        }

        public static string ObterTipoAgrupamento(TConnection connection, string agrupamento)
        {
            string sql = "select tipo from ly_grupo_habilitacao where agrupamento = ? ";
            return ConsultarCampo(connection, sql, agrupamento);
        }

        public static int ExisteCarencia(TConnection connection, string unidade_ens, string agrupamento)
        {
            // CONVERT(DATE, GETDATE()) >= CONVERT(DATE, t.dt_inicio) AND  Demanda 3471 14/02/2013
            string sql = @"
                            DECLARE @agrupamento T_ALFAMEDIUM = ?, 
                                    @ue          T_CODIGO = ? 

                            SELECT Count(*) AS CONTAGEM_CARENCIAS 
                            FROM   ly_aula_docente ad (nolock) 
                                   INNER JOIN ly_turma t (nolock) 
                                           ON t.turma = ad.turma 
                                              AND t.ano = ad.ano 
                                              AND t.semestre = ad.semestre 
                                              AND t.disciplina = ad.disciplina 
                                              AND t.dt_fim = ad.data_fim 
                                   INNER JOIN ly_grupo_habilitacao_disc ghdisc (nolock) 
                                           ON ghdisc.disciplina = Isnull(t.disciplina_multipla, 
                                                                  t.disciplina) 
                                   INNER JOIN ly_unidades_associadas uniass (nolock) 
                                           ON uniass.unidade_fis = ad.faculdade 
                            WHERE  ghdisc.agrupamento = @agrupamento 
                                   AND uniass.unidade_ens = @ue 
                                   AND ad.num_func IN ( 115460, 115451 ) 
                                   AND 
                                   
                                   CONVERT(DATE, Getdate()) <= CONVERT(DATE, t.dt_fim) 
                                   AND t.sit_turma = 'Aberta' 
                            ";


            //SELECT COUNT(*) AS CONTAGEM_CARENCIAS
            //     FROM ly_aula_docente ad (NOLOCK)
            //         INNER JOIN ly_turma t ON     
            //               t.turma = ad.turma AND 
            //               t.ano = ad.ano AND 
            //               t.semestre = ad.semestre AND 
            //               t.disciplina = ad.disciplina AND 
            //               t.dt_fim = ad.data_fim
            //         INNER JOIN ly_grupo_habilitacao_disc ghdisc (NOLOCK) ON 
            //               (t.disciplina_multipla IS NOT NULL AND ghdisc.disciplina = t.disciplina_multipla) OR
            //               (t.disciplina_multipla IS NULL AND ghdisc.disciplina = ad.disciplina)
            //         INNER JOIN ly_unidades_associadas uniass (NOLOCK) ON 
            //             uniass.unidade_fis = ad.faculdade
            //         INNER JOIN ly_docente doc (NOLOCK) ON 
            //             ad.num_func = doc.num_func	
            //     WHERE 
            //         ghdisc.agrupamento = @agrupamento AND 
            //         uniass.unidade_ens = @ue AND 
            //         doc.matricula IN ('99999999','00000000') AND 
            //         CONVERT(DATE, GETDATE()) >= CONVERT(DATE, t.dt_inicio) AND 
            //         CONVERT(DATE, GETDATE()) <= CONVERT(DATE, t.dt_fim) AND
            //         t.sit_turma = 'Aberta'

            int retorno = ExecutarFuncao(sql, connection, agrupamento, unidade_ens);
            return retorno;
        }

        //verifica se o servidor é readaptado
        public static bool IsReadaptado(TConnection connection, string matricula)
        {
            string sql = @"select 1 from ly_lotacao l
                        where
                        l.DATA_NOMEACAO <= convert(date,GetDate()) AND
                        (l.DATA_DESATIVACAO is null or convert(date,l.DATA_DESATIVACAO) > convert(date,getdate()))
                        and l.READAPTADO = 'S'
                        and (l.DT_FIM_READAPTACAO is null or convert(date,l.DT_FIM_READAPTACAO) > convert(date,getdate()))
                        and  l.MATRICULA = ? ";

            int retorno = ExecutarFuncao(sql, connection, matricula);

            if (retorno == 1)
                return true; //é readaptado
            else
                return false;
        }

        //verifica se o servidor é readaptado
        public static bool IsDisciplinaBloqueada(TConnection connection, string agrupamento)
        {
            string sql = @"select 1 from ly_evento_geral where 
                tipo_filtro = 'DisciplinasBloqueadas'
                and valor_filtro = ? 
                and CONVERT(date,getdate()) >= dt_inicio 
                and CONVERT(date,getdate()) <= dt_fim";

            int retorno = ExecutarFuncao(sql, connection, agrupamento);

            if (retorno == 1)
                return true; //é readaptado
            else
                return false;
        }

        //inclui histórico de fluxo solicitação de GLP
        public static RetValue IncluiHistorico(TConnectionWritable connection, Ly_docente_funcao_glp_detalhe.Row row)
        {
            RetValue retorno = null;
            if (row != null)
            {
                //Se histórico já existir, não insere registro duplicado e retorna Sucesso
                Ly_docente_funcao_glp_detalhe.Row jaExiste = Ly_docente_funcao_glp_detalhe.QueryFirstRow(connection,
                    "id_docente_funcao_glp = ? AND data = ? AND status = ? AND usuario = ?",
                    row.Id_docente_funcao_glp, row.Data, row.Status, row.Usuario);
                if (jaExiste == null)
                {
                    Ly_docente_funcao_glp_detalhe.Row.Insert(connection, row.Id_docente_funcao_glp, row.Data, row.Status, "Qtd_glp, Usuario, Motivo", row.Qtd_glp, row.Usuario, row.Motivo);
                    retorno = VerificarErro(connection.GetErrors());
                }
            }
            return retorno;
        }

        private static void EstruturaDT(System.Data.DataTable dt)
        {
            dt.Columns.Add("mes", typeof(int));
            dt.Columns.Add("total_disponibilizado", typeof(float));
            dt.Columns.Add("usada", typeof(float));
            dt.Columns.Add("cancelada", typeof(float));
            dt.Columns.Add("saldo", typeof(float));
        }

        private static void EstruturaDTCompetencia(System.Data.DataTable dt)
        {
            dt.Columns.Add("mes", typeof(int));
            dt.Columns.Add("valor", typeof(decimal));
            dt.Columns.Add("aceita", typeof(decimal));
            dt.Columns.Add("alocada", typeof(decimal));
            dt.Columns.Add("saldo", typeof(decimal));
        }

        public static DataTable CarregarVerbaUnidade(string unidade_ens, int ano)
        {
            DataTable dt = new DataTable();
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                TCommand cm = new TCommand("a_VerbaUnidade", connection);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.Add("@unidade_ens", unidade_ens);
                cm.Parameters.Add("@ano", ano);

                TDataReader dr = cm.ExecuteReader();

                EstruturaDT(dt);

                System.Data.DataRow dtRow = null;
                while (dr.Read())
                {
                    dtRow = dt.NewRow();

                    if (!dr["TOTAL_DISPONIBILIZADO"].IsNull)
                        dtRow["total_disponibilizado"] = Convert.ToString(dr["total_disponibilizado"]);

                    if (!dr["USADA"].IsNull)
                        dtRow["usada"] = Convert.ToString(dr["usada"]);

                    if (!dr["CANCELADA"].IsNull)
                        dtRow["cancelada"] = Convert.ToString(dr["cancelada"]);

                    if (!dr["SALDO"].IsNull)
                        dtRow["saldo"] = Convert.ToString(dr["saldo"]);


                    if (!dr["MES"].IsNull)
                        dtRow["mes"] = Convert.ToString(dr["mes"]);

                    dt.Rows.Add(dtRow);
                }

                dr.Close();

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable CarregarVerbaCoordenadoria(string nucleo, int ano)
        {
            DataTable dt = new DataTable();
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                TCommand cm = new TCommand("a_VerbaCoordenadoria", connection);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.Add("@nucleo", nucleo);
                cm.Parameters.Add("@ano", ano);

                TDataReader dr = cm.ExecuteReader();

                EstruturaDT(dt);

                System.Data.DataRow dtRow = null;
                while (dr.Read())
                {
                    dtRow = dt.NewRow();

                    if (!dr["TOTAL_DISPONIBILIZADO"].IsNull)
                        dtRow["total_disponibilizado"] = Convert.ToString(dr["total_disponibilizado"]);

                    if (!dr["USADA"].IsNull)
                        dtRow["usada"] = Convert.ToString(dr["usada"]);

                    if (!dr["CANCELADA"].IsNull)
                        dtRow["cancelada"] = Convert.ToString(dr["cancelada"]);

                    if (!dr["SALDO"].IsNull)
                        dtRow["saldo"] = Convert.ToString(dr["saldo"]);


                    if (!dr["MES"].IsNull)
                        dtRow["mes"] = Convert.ToString(dr["mes"]);

                    dt.Rows.Add(dtRow);
                }

                dr.Close();

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable CarregarVerbaCompetencia(int ano)
        {
            DataTable dt = new DataTable();
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                TCommand cm = new TCommand("a_VerbaCompetencia", connection);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.Add("@ano", ano);

                TDataReader dr = cm.ExecuteReader();

                EstruturaDTCompetencia(dt);

                System.Data.DataRow dtRow = null;
                while (dr.Read())
                {
                    dtRow = dt.NewRow();

                    if (!dr["VALOR"].IsNull)
                        dtRow["valor"] = Convert.ToString(dr["valor"]);

                    if (!dr["ACEITA"].IsNull)
                        dtRow["aceita"] = Convert.ToString(dr["aceita"]);

                    if (!dr["ALOCADA"].IsNull)
                        dtRow["alocada"] = Convert.ToString(dr["alocada"]);

                    if (!dr["SALDO"].IsNull)
                        dtRow["saldo"] = Convert.ToString(dr["saldo"]);


                    if (!dr["MES"].IsNull)
                        dtRow["mes"] = Convert.ToString(dr["mes"]);

                    dt.Rows.Add(dtRow);
                }

                dr.Close();

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static RetValue AtualizaVerbaCompetencia(Ly_verba_glp_ano_mes.Row row, decimal usada)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            try
            {
                connection.Open(true);
                //não deixar diminuir o valor se já tiver alocado
                if (row.Valor < usada)
                {
                    return new RetValue(false, "", new ErrorList("Não é possível alterar o valor para um valor inferior ao valor que já foi alocado."));
                }

                if (ExisteVerbaParaMes(connection, row.Ano, row.Mes))
                {
                    //update
                    Ly_verba_glp_ano_mes.Row.Update(connection, row.Ano, row.Mes, "Valor", row.Valor);
                    retorno = VerificarErro(connection.GetErrors());
                    if (retorno != null)
                        return retorno;
                }
                else
                {
                    //insert
                    Ly_verba_glp_ano_mes.Row.Insert(connection, row.Ano, row.Mes, row.Valor);
                    if (retorno != null)
                        return retorno;
                }
            }
            catch (Exception ex)
            {
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Valor da GLP alterado com sucesso.", null);
        }

        //verifica se já existe valor cadastrado para o ano e mes
        public static bool ExisteVerbaParaMes(TConnection cn, decimal? ano, decimal? mes)
        {
            string sql = @"select 1 from LY_VERBA_GLP_ANO_MES where ANO = ? and MES = ?";
            int retorno = ExecutarFuncao(sql, cn, ano, mes);
            if (retorno == 1)
                return true;
            return false;
        }

        public static bool ExisteValorMenorMesAnterior(TConnection cn, decimal? ano, decimal? mes, decimal? valor)
        {
            string sql = @"select 1 from LY_VERBA_GLP_ANO_MES 
                            where 
                            ANO = ? 
                            and MES < ?
                            and VALOR > ?";
            int retorno = ExecutarFuncao(sql, cn, ano, mes, valor);
            if (retorno == 1)
                return true;
            return false;
        }

        public static RetValue AtualizaValorGLP(Ly_valor_glp dtValorGLP)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            try
            {
                connection.Open(true);

                for (int i = 0; i < dtValorGLP.Rows.Count; i++)
                {

                    if (ExisteValorParaMesCat(connection, dtValorGLP.Rows[i].Ano, dtValorGLP.Rows[i].Mes, dtValorGLP.Rows[i].Funcao, dtValorGLP.Rows[i].Tipo))
                    { //update
                        dtValorGLP.Rows[i].Id_valor_glp = ConsultarID(connection, dtValorGLP.Rows[i].Ano, dtValorGLP.Rows[i].Mes, dtValorGLP.Rows[i].Funcao, dtValorGLP.Rows[i].Tipo); //consulta valor do ID para estes dados
                        if (string.IsNullOrEmpty(dtValorGLP.Rows[i].Tipo))
                        {
                            Ly_valor_glp.Row.Update(connection, dtValorGLP.Rows[i].Id_valor_glp, "Ano, Mes, Funcao, Valor", dtValorGLP.Rows[i].Ano, dtValorGLP.Rows[i].Mes, dtValorGLP.Rows[i].Funcao, dtValorGLP.Rows[i].Valor);
                        }
                        else
                        {
                            Ly_valor_glp.Row.Update(connection, dtValorGLP.Rows[i].Id_valor_glp, "Ano, Mes, Funcao, Tipo, Valor", dtValorGLP.Rows[i].Ano, dtValorGLP.Rows[i].Mes, dtValorGLP.Rows[i].Funcao, dtValorGLP.Rows[i].Tipo, dtValorGLP.Rows[i].Valor);
                        }


                        retorno = VerificarErro(connection.GetErrors());
                        if (retorno != null)
                            return retorno;
                    }
                    else
                    {
                        //insert
                        Ly_valor_glp.Row.Insert(connection, dtValorGLP.Rows[i].Ano, dtValorGLP.Rows[i].Mes, dtValorGLP.Rows[i].Funcao, dtValorGLP.Rows[i].Valor, "Tipo", dtValorGLP.Rows[i].Tipo);
                        if (retorno != null)
                            return retorno;
                    }
                }

            }
            catch (Exception ex)
            {
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Valor da GLP alterado com sucesso.", null);
        }

        //verifica se já existe valor cadastrado para o ano e mes
        public static bool ExisteValorParaMesCat(TConnection conn, decimal? ano, decimal? mes, string funcao, string tipo)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                string sql = @"select 1 from Ly_valor_glp where ANO = ? and MES = ? and FUNCAO = ?";
                int retorno = ExecutarFuncao(sql, conn, ano, mes, funcao);
                if (retorno == 1)
                    return true;
                return false;
            }
            else
            {
                string sql = @"select 1 from Ly_valor_glp where ANO = ? and MES = ? and FUNCAO = ? and TIPO = ?";
                int retorno = ExecutarFuncao(sql, conn, ano, mes, funcao, tipo);
                if (retorno == 1)
                    return true;
                return false;
            }

        }

        public static decimal ConsultarID(TConnection conn, decimal? ano, decimal? mes, string funcao, string tipo)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                string sql = @"select ID_VALOR_GLP from Ly_valor_glp where ANO = ? and MES = ? and FUNCAO = ?";
                string valorConsulta = ConsultarCampo(conn, sql, ano, mes, funcao);
                if (!string.IsNullOrEmpty(valorConsulta))
                    return Convert.ToDecimal(valorConsulta);
                else
                    return 0M;
            }
            else
            {
                string sql = @"select ID_VALOR_GLP from Ly_valor_glp where ANO = ? and MES = ? and FUNCAO = ? and TIPO = ?";
                string valorConsulta = ConsultarCampo(conn, sql, ano, mes, funcao, tipo);
                if (!string.IsNullOrEmpty(valorConsulta))
                    return Convert.ToDecimal(valorConsulta);
                else
                    return 0M;
            }
        }

        public static DataTable CarregarValoresGLP(int ano)
        {
            string sql = @"DECLARE @ano int 
set @ano = ?
select 
f.funcao, f.descricao,
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 1) 'm1',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 2) 'm2',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 3) 'm3',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 4) 'm4',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 5) 'm5',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 6) 'm6',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 7) 'm7',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 8) 'm8',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 9) 'm9',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 10) 'm10',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 11) 'm11',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 12) 'm12'
from LY_FUNCAO f 
where CAMPO_01 = 'S' and FUNCAO <> '108' and FUNCAO <> '109'
union
select 
'DOC II - Atividade Complementar', 'DOC II - Atividade Complementar',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 1 and tipo = 'Atividade') 'm1',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 2 and tipo = 'Atividade') 'm2',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 3 and tipo = 'Atividade') 'm3',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 4 and tipo = 'Atividade') 'm4',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 5 and tipo = 'Atividade') 'm5',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 6 and tipo = 'Atividade') 'm6',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 7 and tipo = 'Atividade') 'm7',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 8 and tipo = 'Atividade') 'm8',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 9 and tipo = 'Atividade') 'm9',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 10 and tipo = 'Atividade') 'm10',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 11 and tipo = 'Atividade') 'm11',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 12 and tipo = 'Atividade') 'm12'
from LY_FUNCAO f 
where FUNCAO = '108'
union
select 
'DOC II - Disciplina Integrada', 'DOC II - Disciplina Integrada',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 1 and tipo = 'Disciplina') 'm1',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 2 and tipo = 'Disciplina') 'm2',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 3 and tipo = 'Disciplina') 'm3',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 4 and tipo = 'Disciplina') 'm4',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 5 and tipo = 'Disciplina') 'm5',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 6 and tipo = 'Disciplina') 'm6',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 7 and tipo = 'Disciplina') 'm7',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 8 and tipo = 'Disciplina') 'm8',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 9 and tipo = 'Disciplina') 'm9',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 10 and tipo = 'Disciplina') 'm10',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 11 and tipo = 'Disciplina') 'm11',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 12 and tipo = 'Disciplina') 'm12'
from LY_FUNCAO f 
where FUNCAO = '108'
union
select 
'DOC II - 40 - Atividade Complementar', 'DOC II - 40 - Atividade Complementar',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 1 and tipo = 'Atividade') 'm1',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 2 and tipo = 'Atividade') 'm2',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 3 and tipo = 'Atividade') 'm3',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 4 and tipo = 'Atividade') 'm4',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 5 and tipo = 'Atividade') 'm5',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 6 and tipo = 'Atividade') 'm6',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 7 and tipo = 'Atividade') 'm7',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 8 and tipo = 'Atividade') 'm8',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 9 and tipo = 'Atividade') 'm9',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 10 and tipo = 'Atividade') 'm10',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 11 and tipo = 'Atividade') 'm11',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 12 and tipo = 'Atividade') 'm12'
from LY_FUNCAO f 
where FUNCAO = '109'
union
select 
'DOC II - 40 - Disciplina Integrada', 'DOC II - 40 - Disciplina Integrada',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 1 and tipo = 'Disciplina') 'm1',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 2 and tipo = 'Disciplina') 'm2',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 3 and tipo = 'Disciplina') 'm3',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 4 and tipo = 'Disciplina') 'm4',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 5 and tipo = 'Disciplina') 'm5',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 6 and tipo = 'Disciplina') 'm6',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 7 and tipo = 'Disciplina') 'm7',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 8 and tipo = 'Disciplina') 'm8',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 9 and tipo = 'Disciplina') 'm9',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 10 and tipo = 'Disciplina') 'm10',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 11 and tipo = 'Disciplina') 'm11',
(select VALOR from ly_valor_glp where FUNCAO = f.FUNCAO and ANO = @ano and MES = 12 and tipo = 'Disciplina') 'm12'
from LY_FUNCAO f 
where FUNCAO = '109'";
            return Consultar(sql, ano);
        }

        public static RetValue ExcluirValorGLP(Ly_valor_glp dtValorGLPDel)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            try
            {
                connection.Open(true);

                for (int i = 0; i < dtValorGLPDel.Rows.Count; i++)
                {
                    dtValorGLPDel.Rows[i].Id_valor_glp = ConsultarID(connection, dtValorGLPDel.Rows[i].Ano, dtValorGLPDel.Rows[i].Mes, dtValorGLPDel.Rows[i].Funcao, dtValorGLPDel.Rows[i].Tipo); //consulta valor do ID para estes dados
                    Ly_valor_glp.Row.Delete(connection, dtValorGLPDel.Rows[i].Id_valor_glp);
                }

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null)
                    return retorno;
            }
            catch (Exception ex)
            {
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                connection.Close();
            }
            return new RetValue(true, "Valor da GLP alterado com sucesso.", null);
        }

        public static DataTable CarregarVerbaCoordenadorias(int ano, int mes)
        {
            DataTable dt = new DataTable();
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                TCommand cm = new TCommand("a_VerbaCoordenadorias", connection);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.Add("@ano", ano);
                cm.Parameters.Add("@mes", mes);

                TDataReader dr = cm.ExecuteReader();

                dt.Columns.Add("nucleo", typeof(string));
                dt.Columns.Add("total_disponibilizado", typeof(float));
                dt.Columns.Add("usada", typeof(float));
                dt.Columns.Add("cancelada", typeof(float));
                dt.Columns.Add("saldo", typeof(float));

                System.Data.DataRow dtRow = null;
                while (dr.Read())
                {
                    dtRow = dt.NewRow();

                    if (!dr["TOTAL_DISPONIBILIZADO"].IsNull)
                        dtRow["total_disponibilizado"] = Convert.ToString(dr["total_disponibilizado"]);

                    if (!dr["USADA"].IsNull)
                        dtRow["usada"] = Convert.ToString(dr["usada"]);

                    if (!dr["CANCELADA"].IsNull)
                        dtRow["cancelada"] = Convert.ToString(dr["cancelada"]);

                    if (!dr["SALDO"].IsNull)
                        dtRow["saldo"] = Convert.ToString(dr["saldo"]);


                    if (!dr["NUCLEO"].IsNull)
                        dtRow["nucleo"] = Convert.ToString(dr["nucleo"]);

                    dt.Rows.Add(dtRow);
                }

                dr.Close();

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable CarregarVerbaUnidades(string nucleo, int ano, int mes)
        {
            DataTable dt = new DataTable();
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                TCommand cm = new TCommand("a_VerbaUnidades", connection);
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.Parameters.Add("@nucleo", nucleo);
                cm.Parameters.Add("@ano", ano);
                cm.Parameters.Add("@mes", mes);

                TDataReader dr = cm.ExecuteReader();

                dt.Columns.Add("unidade", typeof(string));
                dt.Columns.Add("total_disponibilizado", typeof(float));
                dt.Columns.Add("usada", typeof(float));
                dt.Columns.Add("cancelada", typeof(float));
                dt.Columns.Add("saldo", typeof(float));

                System.Data.DataRow dtRow = null;
                while (dr.Read())
                {
                    dtRow = dt.NewRow();

                    if (!dr["TOTAL_DISPONIBILIZADO"].IsNull)
                        dtRow["total_disponibilizado"] = Convert.ToString(dr["total_disponibilizado"]);

                    if (!dr["USADA"].IsNull)
                        dtRow["usada"] = Convert.ToString(dr["usada"]);

                    if (!dr["CANCELADA"].IsNull)
                        dtRow["cancelada"] = Convert.ToString(dr["cancelada"]);

                    if (!dr["SALDO"].IsNull)
                        dtRow["saldo"] = Convert.ToString(dr["saldo"]);

                    if (!dr["UNIDADE"].IsNull)
                        dtRow["unidade"] = Convert.ToString(dr["unidade"]);

                    dt.Rows.Add(dtRow);
                }

                dr.Close();

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static QueryTable CarregarAndamentos(string unidade, int ano, int mes)
        {
            TConnection cn = Config.CreateConnection();

            string sql = @"select distinct
                        s.ID_DOCENTE_FUNCAO_GLP,
                        IDFUNCIONAL,
                        doc.VINCULO,
                        (CONVERT (VARCHAR, IDFUNCIONAL) + '/' + CONVERT (VARCHAR, doc.VINCULO)) AS IDVINCULO,
                        s.Prazo as prazo,
                        doc.Matricula as matricula,
                        PE.NOME_COMPL as nome,
                        fun.DESCRICAO as segmento,
                        (select top 1 gh.DESCRICAO from LY_GRUPO_HABILITACAO gh
                        where AGRUPAMENTO = s.AGRUPAMENTO) as disciplina,
                        (select top 1 det.status from LY_DOCENTE_FUNCAO_GLP_DETALHE det where det.ID_DOCENTE_FUNCAO_GLP = s.ID_DOCENTE_FUNCAO_GLP order by det.ID_DOCENTE_FUNCAO_GLP_DETALHE desc) as status,
                        (select top 1 det.motivo from LY_DOCENTE_FUNCAO_GLP_DETALHE det where det.ID_DOCENTE_FUNCAO_GLP = s.ID_DOCENTE_FUNCAO_GLP order by det.ID_DOCENTE_FUNCAO_GLP_DETALHE desc) as motivo,
                        (select top 1 Convert(numeric,det.QTD_GLP) from LY_DOCENTE_FUNCAO_GLP_DETALHE det where det.ID_DOCENTE_FUNCAO_GLP = s.ID_DOCENTE_FUNCAO_GLP order by det.ID_DOCENTE_FUNCAO_GLP_DETALHE desc) as quantidade,
                        (select top 1 det.data from LY_DOCENTE_FUNCAO_GLP_DETALHE det where det.ID_DOCENTE_FUNCAO_GLP = s.ID_DOCENTE_FUNCAO_GLP order by det.ID_DOCENTE_FUNCAO_GLP_DETALHE desc) as data,
                        Convert(numeric,GLP_SOLICITADA) GLP_SOLICITADA, 
                        Convert(numeric,GLP_USADA) GLP_USADA, 
                        Convert(numeric,GLP_CANCELADA) GLP_CANCELADA, 
                        (select top 1 det.usuario from LY_DOCENTE_FUNCAO_GLP_DETALHE det where det.ID_DOCENTE_FUNCAO_GLP = s.ID_DOCENTE_FUNCAO_GLP order by det.ID_DOCENTE_FUNCAO_GLP_DETALHE) as usuario
                        from LY_DOCENTE_FUNCAO_GLP s
                        inner join LY_DOCENTE doc
                        on doc.MATRICULA = s.MATRICULA
                        inner join LY_FUNCAO fun
                        on(fun.FUNCAO = s.FUNCAO_GLP)

                        INNER JOIN LY_PESSOA PE ON PE.PESSOA=DOC.PESSOA
                        where 
                        UNIDADE_ENS = ?
                        and DATEPART(YEAR ,s.DATA) = ?
                        and DATEPART(MONTH ,s.DATA) = ?
                        order by PE.NOME_COMPL, data desc";

            QueryTable qt = new QueryTable(sql);

            qt.Query(cn, unidade, ano, mes);

            return qt;
        }

        public static QueryTable RecalcularCHLivreGLP(string regional, string unidade)
        {
            TConnection cn = Config.CreateConnection();
            cn.Open();

            System.Text.StringBuilder sql = new System.Text.StringBuilder();

            sql.Append(@" DECLARE @ID_DOCENTE_FUNCAO_GLP T_NUMERO
						DECLARE @CH_LIVRE INTEGER
						--DECLARE @CH_LIVRE_MUNICIPIO INTEGER

						SET ROWCOUNT 0  

 SELECT DISTINCT ID_DOCENTE_FUNCAO_GLP,
                (
                       SELECT CONVERT(INTEGER, ISNULL(Sum(CH_LIVRE), 0))
                       FROM   (
                                     SELECT CONVERT(INT,CARGAHORARIAREGENCIA)      CARGAHORARIAREGENCIA,
                                            ISNULL(ATUACAO, 0)                   AS ATUACAO,
                                            ISNULL(CH_LIVRE, CARGAHORARIAREGENCIA) AS CH_LIVRE,
                                            NUM_FUNC,
                                            ID_REGIONAL,
                                            SETOR,
                                            UNIDADE_ENS,
                                            DISCIPLINA_INGRESSO
                                     FROM   (
                                                            SELECT DISTINCT DD.NUM_FUNC,
                                                                            LT.UNIDADE_ENS,
                                                                            LT.SETOR,
                                                                            UE.ID_REGIONAL,
                                                                            CH.CARGAHORARIAREGENCIA,
                                                                            SS.ATUACAO,
                                                                            CONVERT(INT, CH.CARGAHORARIAREGENCIA) - SS.ATUACAO AS CH_LIVRE,
                                                                            (
                                                                                       SELECT TOP 1
                                                                                                                           HH.AGRUPAMENTO
                                                                                       FROM       LY_GRUPO_HABILITACAO     AS HH
                                                                                       INNER JOIN LY_GRUPO_HABILITACAO_DOC AS DF
                                                                                       ON         DF.AGRUPAMENTO = HH.AGRUPAMENTO
                                                                                       WHERE      DF.NUM_FUNC = DD.NUM_FUNC
                                                                                       AND        DF.AGRUPAMENTO_INGRESSO = 'S'
                                                                                       AND        DF.PROVISORIO='N') AS DISCIPLINA_INGRESSO
                                                            FROM            LY_DOCENTE DD WITH(NOLOCK)
                                                            INNER JOIN      LY_LOTACAO LT WITH(NOLOCK)
                                                            ON              DD.MATRICULA = LT.MATRICULA
                                                            AND             DD.PESSOA = LT.PESSOA
                                                            INNER JOIN      LY_UNIDADE_ENSINO EU
                                                            ON              LT.UNIDADE_ENS = EU.UNIDADE_ENS
                                                            INNER JOIN		LY_CATEGORIA_DOCENTE CD
															ON				CD.CATEGORIA = DD.CATEGORIA
                                                            INNER JOIN      RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH 
                                                            ON              CH.AGRUPAMENTOCARGOSID = cd.AGRUPAMENTOCARGOSID
                                                            AND             CH.FUNCAO = LT.FUNCAO                             
                                                            LEFT JOIN -- aulas do docente no ano corrente em turma aberta sem contar as GLPs
                                                                            (
                                                                                       SELECT     D.NUM_FUNC,
                                                                                                  Count(*) AS ATUACAO
                                                                                       FROM       LY_DOCENTE D WITH(NOLOCK)
                                                                                       INNER JOIN LY_AULA_DOCENTE AD WITH(NOLOCK)
                                                                                       ON         AD.NUM_FUNC = D.NUM_FUNC
                                                                                       INNER JOIN LY_TURMA TU WITH(NOLOCK)
                                                                                       ON         TU.TURMA = AD.TURMA
                                                                                       AND        TU.DISCIPLINA = AD.DISCIPLINA
                                                                                       AND        TU.SEMESTRE = AD.SEMESTRE
                                                                                       AND        TU.DT_FIM = AD.DATA_FIM
                                                                                       AND        TU.SIT_TURMA = 'ABERTA'
                                                                                       WHERE      AD.ANO = Year(CURRENT_TIMESTAMP)
                                                                                       AND        NOT EXISTS
                                                                                                  (
                                                                                                         SELECT 1
                                                                                                         FROM   LY_AULA_DOCENTE_TIPO T1
                                                                                                         WHERE  T1.ANO = AD.ANO
                                                                                                         AND    T1.SEMESTRE = AD.SEMESTRE
                                                                                                         AND    T1.FACULDADE = AD.FACULDADE
                                                                                                         AND    T1.DIA_SEMANA = AD.DIA_SEMANA
                                                                                                         AND    T1.AULA = AD.AULA
                                                                                                         AND    T1.NUM_FUNC = AD.NUM_FUNC
                                                                                                         AND    T1.TURMA = AD.TURMA
                                                                                                         AND    T1.TURNO = AD.TURNO
                                                                                                         AND    T1.DISCIPLINA = AD.DISCIPLINA
                                                                                                         AND    T1.TIPO_AULA='GLP')
                                                                                       GROUP BY   D.NUM_FUNC) AS SS
                                                            ON              SS.NUM_FUNC = DD.NUM_FUNC 															                                        
															 WHERE dd.REGIMECONTRATACAOID = 1 AND 
															--LOTAÇÃO EM FUNÇÃO DE REGÊNCIA
															EXISTS
															(
																   SELECT 1
																   FROM   LY_FUNCAO F1
																   WHERE  F1.FUNCAO=LT.FUNCAO
																   AND    F1.CAMPO_01 = 'S')
															--LOTAÇÃO ATIVA
															AND
															(
															  LT.DATA_NOMEACAO <= CONVERT(DATE, CURRENT_TIMESTAMP)
															  AND
															  (
																LT.DATA_DESATIVACAO > CONVERT(DATE,CURRENT_TIMESTAMP)
																OR
																LT.DATA_DESATIVACAO IS NULL
															  )
															)
															--TEM CARGA LIVRE SOBRANDO
															AND
															CH.CARGAHORARIAREGENCIA - ISNULL(SS.ATUACAO,0) > 0
															-- NÃO TEM LICENÇA DE CARGA HORÁRIA REDUZIDA nem outras
															AND
															NOT EXISTS
															(
																   SELECT 1
																   FROM   LY_LICENCA_DOCENTE WITH(NOLOCK)
																   WHERE  DD.NUM_FUNC = NUM_FUNC
																   AND    (
																				 DTFIM IS NULL
																		  OR     DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)))
															AND
															EU.SETOR = UE.SETOR 

															UNION   

															 SELECT DISTINCT DD.NUM_FUNC,
																			LT.UNIDADE_ENS,
																			LT.SETOR,
																			UE.ID_REGIONAL,
																			CH.CARGAHORARIAREGENCIA,
																			SS_1.ATUACAO,
																			CONVERT(INT, CH.CARGAHORARIAREGENCIA) / 2 - SS_1.ATUACAO AS
																			ch_livre,
																			(SELECT TOP 1 HH.AGRUPAMENTO
																			 FROM   LY_GRUPO_HABILITACAO AS HH
																					INNER JOIN LY_GRUPO_HABILITACAO_DOC AS DF
																							ON DF.AGRUPAMENTO = HH.AGRUPAMENTO
																			 WHERE  DF.NUM_FUNC = DD.NUM_FUNC
																					AND DF.AGRUPAMENTO_INGRESSO = 'S'
																					AND DF.PROVISORIO = 'N')                       AS
																			DISCIPLINA_INGRESSO
															                       
															FROM   LY_DOCENTE AS DD WITH(NOLOCK)
																   INNER JOIN LY_LOTACAO AS LT WITH(NOLOCK)
																		   ON DD.MATRICULA = LT.MATRICULA
																			  AND DD.PESSOA = LT.PESSOA
																   INNER JOIN LY_UNIDADE_ENSINO AS EU
																		   ON EU.SETOR = LT.SETOR
																			  AND LT.UNIDADE_ENS = EU.UNIDADE_ENS
																   INNER JOIN		LY_CATEGORIA_DOCENTE CD
															                ON	CD.CATEGORIA = DD.CATEGORIA
                                                                   INNER JOIN      RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH 
                                                                            ON  CH.AGRUPAMENTOCARGOSID = CD.AGRUPAMENTOCARGOSID
                                                                            AND CH.FUNCAO = LT.FUNCAO  
																   LEFT JOIN
																   -- aulas do docente no ano corrente em turma aberta sem contar as GLPs
																   (SELECT D.NUM_FUNC,
																		   Count(*) AS ATUACAO
																	FROM   LY_DOCENTE D WITH(NOLOCK)
																		   INNER JOIN LY_AULA_DOCENTE AD WITH(NOLOCK)
																				   ON AD.NUM_FUNC = D.NUM_FUNC
																		   INNER JOIN LY_TURMA TU WITH(NOLOCK)
																				   ON TU.TURMA = AD.TURMA
																					  AND TU.DISCIPLINA = AD.DISCIPLINA
																					  AND TU.SEMESTRE = AD.SEMESTRE
																					  AND TU.DT_FIM = AD.DATA_FIM
																					  AND TU.SIT_TURMA = 'ABERTA'
																	WHERE  AD.ANO = Year(CURRENT_TIMESTAMP)
																		   AND NOT EXISTS (SELECT 1
																						   FROM   LY_AULA_DOCENTE_TIPO T1
																						   WHERE  T1.ANO = AD.ANO
																								  AND T1.SEMESTRE = AD.SEMESTRE
																								  AND T1.FACULDADE = AD.FACULDADE
																								  AND T1.DIA_SEMANA = AD.DIA_SEMANA
																								  AND T1.AULA = AD.AULA
																								  AND T1.NUM_FUNC = AD.NUM_FUNC
																								  AND T1.TURMA = AD.TURMA
																								  AND T1.TURNO = AD.TURNO
																								  AND T1.DISCIPLINA = AD.DISCIPLINA
																								  AND T1.TIPO_AULA = 'GLP')
																	GROUP  BY D.NUM_FUNC) AS SS_1
																		  ON SS_1.NUM_FUNC = DD.NUM_FUNC  

                      
																	
																	WHERE
                                                                    dd.REGIMECONTRATACAOID = 1 AND 
																	--TEM CARGA LIVRE SOBRANDO
																	CH.CARGAHORARIAREGENCIA/2 - ISNULL(SS_1.ATUACAO,0) > 0
																	--LOTAÇÃO EM FUNÇÃO DE REGÊNCIA
																	AND
																	EXISTS
																	(
																		   SELECT 1
																		   FROM   LY_FUNCAO F1
																		   WHERE  F1.FUNCAO=LT.FUNCAO
																		   AND    F1.CAMPO_01='S')
																	--LOTAÇÃO ATIVA
																	AND
																	(
																	  LT.DATA_NOMEACAO <= CONVERT(DATE, CURRENT_TIMESTAMP)
																	  AND
																	  (
																		LT.DATA_DESATIVACAO > CONVERT(DATE,CURRENT_TIMESTAMP)
																		OR
																		LT.DATA_DESATIVACAO IS NULL
																	  )
																	)
																	--POSSUI LICENÇA DE CH REDUZIDA (43)
																	AND
																	EXISTS
																	(
																		   SELECT 1
																		   FROM   LY_LICENCA_DOCENTE WITH(NOLOCK)
																		   WHERE  MOTIVO = '43'
																		   AND    DTINI <= CONVERT(DATE, CURRENT_TIMESTAMP)
																		   AND    DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)
																		   AND    NUM_FUNC = DD.NUM_FUNC)
																	AND
																	NOT EXISTS
																	(
																		   SELECT 1
																		   FROM   LY_LICENCA_DOCENTE WITH(NOLOCK)
																		   WHERE  DD.NUM_FUNC = NUM_FUNC
																		   AND    MOTIVO <> '43'
																		   AND    (
																						 DTFIM IS NULL
																				  OR     DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)))
																	AND
																	EU.SETOR = UE.SETOR ) S WHERE DISCIPLINA_INGRESSO = DF.AGRUPAMENTO )A) AS CH_LIVRE,
																	--fim chlivre
/*
																	--INICIO CHLIVRE MUNICIPIO
																	(
                       SELECT CONVERT(INTEGER, ISNULL(Sum(CH_LIVRE), 0))
                       FROM   (
                                     SELECT CONVERT(INT,CARGAHORARIAREGENCIA)      CARGAHORARIAREGENCIA,
                                            ISNULL(ATUACAO, 0)                   AS ATUACAO,
                                            ISNULL(CH_LIVRE, CARGAHORARIAREGENCIA) AS CH_LIVRE,
                                            NUM_FUNC,
                                            ID_REGIONAL,
                                            SETOR,
                                            UNIDADE_ENS,
                                            DISCIPLINA_INGRESSO
                                     FROM   (
                                                            SELECT DISTINCT DD.NUM_FUNC,
                                                                            LT.UNIDADE_ENS,
                                                                            LT.SETOR,
                                                                            UE.ID_REGIONAL,
                                                                            CH.CARGAHORARIAREGENCIA,
                                                                            SS.ATUACAO,
                                                                            CONVERT(INT, CH.CARGAHORARIAREGENCIA) - SS.ATUACAO AS CH_LIVRE,
                                                                            (
                                                                                       SELECT TOP 1
                                                                                                                           HH.AGRUPAMENTO
                                                                                       FROM       LY_GRUPO_HABILITACAO     AS HH
                                                                                       INNER JOIN LY_GRUPO_HABILITACAO_DOC AS DF
                                                                                       ON         DF.AGRUPAMENTO = HH.AGRUPAMENTO
                                                                                       WHERE      DF.NUM_FUNC = DD.NUM_FUNC
                                                                                       AND        DF.AGRUPAMENTO_INGRESSO = 'S'
                                                                                       AND        DF.PROVISORIO='N') AS DISCIPLINA_INGRESSO
                                                            FROM            LY_DOCENTE DD WITH(NOLOCK)
                                                            INNER JOIN      LY_LOTACAO LT WITH(NOLOCK)
                                                            ON              DD.MATRICULA = LT.MATRICULA
                                                            AND             DD.PESSOA = LT.PESSOA
                                                            INNER JOIN      LY_UNIDADE_ENSINO EU
                                                            ON              LT.UNIDADE_ENS = EU.UNIDADE_ENS
                                                            INNER JOIN		LY_CATEGORIA_DOCENTE CD
															ON				CD.CATEGORIA = DD.CATEGORIA
                                                            INNER JOIN      RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH 
                                                            ON              CH.AGRUPAMENTOCARGOSID = CD.AGRUPAMENTOCARGOSID
                                                            AND             CH.FUNCAO = LT.FUNCAO  
                                                            LEFT JOIN -- aulas do docente no ano corrente em turma aberta sem contar as GLPs
                                                                            (
                                                                                       SELECT     D.NUM_FUNC,
                                                                                                  Count(*) AS ATUACAO
                                                                                       FROM       LY_DOCENTE D WITH(NOLOCK)
                                                                                       INNER JOIN LY_AULA_DOCENTE AD WITH(NOLOCK)
                                                                                       ON         AD.NUM_FUNC = D.NUM_FUNC
                                                                                       INNER JOIN LY_TURMA TU WITH(NOLOCK)
                                                                                       ON         TU.TURMA = AD.TURMA
                                                                                       AND        TU.DISCIPLINA = AD.DISCIPLINA
                                                                                       AND        TU.SEMESTRE = AD.SEMESTRE
                                                                                       AND        TU.DT_FIM = AD.DATA_FIM
                                                                                       AND        TU.SIT_TURMA = 'ABERTA'
                                                                                       WHERE      AD.ANO = Year(CURRENT_TIMESTAMP)
                                                                                       AND        NOT EXISTS
                                                                                                  (
                                                                                                         SELECT 1
                                                                                                         FROM   LY_AULA_DOCENTE_TIPO T1
                                                                                                         WHERE  T1.ANO = AD.ANO
                                                                                                         AND    T1.SEMESTRE = AD.SEMESTRE
                                                                                                         AND    T1.FACULDADE = AD.FACULDADE
                                                                                                         AND    T1.DIA_SEMANA = AD.DIA_SEMANA
                                                                                                         AND    T1.AULA = AD.AULA
                                                                                                         AND    T1.NUM_FUNC = AD.NUM_FUNC
                                                                                                         AND    T1.TURMA = AD.TURMA
                                                                                                         AND    T1.TURNO = AD.TURNO
                                                                                                         AND    T1.DISCIPLINA = AD.DISCIPLINA
                                                                                                         AND    T1.TIPO_AULA='GLP')
                                                                                       GROUP BY   D.NUM_FUNC) AS SS
                                                            ON              SS.NUM_FUNC = DD.NUM_FUNC 															                                        
															 WHERE
                                                            dd.REGIMECONTRATACAOID = 1 AND 
															--LOTAÇÃO EM FUNÇÃO DE REGÊNCIA
															EXISTS
															(
																   SELECT 1
																   FROM   LY_FUNCAO F1
																   WHERE  F1.FUNCAO=LT.FUNCAO
																   AND    F1.CAMPO_01 = 'S')
															--LOTAÇÃO ATIVA
															AND
															(
															  LT.DATA_NOMEACAO <= CONVERT(DATE, CURRENT_TIMESTAMP)
															  AND
															  (
																LT.DATA_DESATIVACAO > CONVERT(DATE,CURRENT_TIMESTAMP)
																OR
																LT.DATA_DESATIVACAO IS NULL
															  )
															)
															--TEM CARGA LIVRE SOBRANDO
															AND
															CH.CARGAHORARIAREGENCIA - ISNULL(SS.ATUACAO,0) > 0
															-- NÃO TEM LICENÇA DE CARGA HORÁRIA REDUZIDA nem outras
															AND
															NOT EXISTS
															(
																   SELECT 1
																   FROM   LY_LICENCA_DOCENTE WITH(NOLOCK)
																   WHERE  DD.NUM_FUNC = NUM_FUNC
																   AND    (
																				 DTFIM IS NULL
																		  OR     DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)))
															AND
															EU.MUNICIPIO = UE.MUNICIPIO 
															AND EU.ID_REGIONAL = UE.ID_REGIONAL

															UNION   

															 SELECT DISTINCT DD.NUM_FUNC,
																			LT.UNIDADE_ENS,
																			LT.SETOR,
																			UE.ID_REGIONAL,
																			CH.CARGAHORARIAREGENCIA,
																			SS_1.ATUACAO,
																			CONVERT(INT, CH.CARGAHORARIAREGENCIA) / 2 - SS_1.ATUACAO AS
																			ch_livre,
																			(SELECT TOP 1 HH.AGRUPAMENTO
																			 FROM   LY_GRUPO_HABILITACAO AS HH
																					INNER JOIN LY_GRUPO_HABILITACAO_DOC AS DF
																							ON DF.AGRUPAMENTO = HH.AGRUPAMENTO
																			 WHERE  DF.NUM_FUNC = DD.NUM_FUNC
																					AND DF.AGRUPAMENTO_INGRESSO = 'S'
																					AND DF.PROVISORIO = 'N')                       AS
																			DISCIPLINA_INGRESSO
															                       
															FROM   LY_DOCENTE AS DD WITH(NOLOCK)
																   INNER JOIN LY_LOTACAO AS LT WITH(NOLOCK)
																		   ON DD.MATRICULA = LT.MATRICULA
																			  AND DD.PESSOA = LT.PESSOA
																   INNER JOIN LY_UNIDADE_ENSINO AS EU
																		   ON EU.SETOR = LT.SETOR
																			  AND LT.UNIDADE_ENS = EU.UNIDADE_ENS
																  INNER JOIN   LY_CATEGORIA_DOCENTE CD
															                ON	CD.CATEGORIA = DD.CATEGORIA
                                                                  INNER JOIN    RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH 
                                                                            ON  CH.AGRUPAMENTOCARGOSID = cd.AGRUPAMENTOCARGOSID
                                                                                AND  CH.FUNCAO = LT.FUNCAO                             
																   LEFT JOIN
																   -- aulas do docente no ano corrente em turma aberta sem contar as GLPs
																   (SELECT D.NUM_FUNC,
																		   Count(*) AS ATUACAO
																	FROM   LY_DOCENTE D WITH(NOLOCK)
																		   INNER JOIN LY_AULA_DOCENTE AD WITH(NOLOCK)
																				   ON AD.NUM_FUNC = D.NUM_FUNC
																		   INNER JOIN LY_TURMA TU WITH(NOLOCK)
																				   ON TU.TURMA = AD.TURMA
																					  AND TU.DISCIPLINA = AD.DISCIPLINA
																					  AND TU.SEMESTRE = AD.SEMESTRE
																					  AND TU.DT_FIM = AD.DATA_FIM
																					  AND TU.SIT_TURMA = 'ABERTA'
																	WHERE  AD.ANO = Year(CURRENT_TIMESTAMP)
																		   AND NOT EXISTS (SELECT 1
																						   FROM   LY_AULA_DOCENTE_TIPO T1
																						   WHERE  T1.ANO = AD.ANO
																								  AND T1.SEMESTRE = AD.SEMESTRE
																								  AND T1.FACULDADE = AD.FACULDADE
																								  AND T1.DIA_SEMANA = AD.DIA_SEMANA
																								  AND T1.AULA = AD.AULA
																								  AND T1.NUM_FUNC = AD.NUM_FUNC
																								  AND T1.TURMA = AD.TURMA
																								  AND T1.TURNO = AD.TURNO
																								  AND T1.DISCIPLINA = AD.DISCIPLINA
																								  AND T1.TIPO_AULA = 'GLP')
																	GROUP  BY D.NUM_FUNC) AS SS_1
																		  ON SS_1.NUM_FUNC = DD.NUM_FUNC  

                      
																	
																	WHERE
                                                                    dd.REGIMECONTRATACAOID = 1 AND 
																	--TEM CARGA LIVRE SOBRANDO
																	CH.CARGAHORARIAREGENCIA/2 - ISNULL(SS_1.ATUACAO,0) > 0
																	--LOTAÇÃO EM FUNÇÃO DE REGÊNCIA
																	AND
																	EXISTS
																	(
																		   SELECT 1
																		   FROM   LY_FUNCAO F1
																		   WHERE  F1.FUNCAO=LT.FUNCAO
																		   AND    F1.CAMPO_01='S')
																	--LOTAÇÃO ATIVA
																	AND
																	(
																	  LT.DATA_NOMEACAO <= CONVERT(DATE, CURRENT_TIMESTAMP)
																	  AND
																	  (
																		LT.DATA_DESATIVACAO > CONVERT(DATE,CURRENT_TIMESTAMP)
																		OR
																		LT.DATA_DESATIVACAO IS NULL
																	  )
																	)
																	--POSSUI LICENÇA DE CH REDUZIDA (43)
																	AND
																	EXISTS
																	(
																		   SELECT 1
																		   FROM   LY_LICENCA_DOCENTE WITH(NOLOCK)
																		   WHERE  MOTIVO = '43'
																		   AND    DTINI <= CONVERT(DATE, CURRENT_TIMESTAMP)
																		   AND    DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)
																		   AND    NUM_FUNC = DD.NUM_FUNC)
																	AND
																	NOT EXISTS
																	(
																		   SELECT 1
																		   FROM   LY_LICENCA_DOCENTE WITH(NOLOCK)
																		   WHERE  DD.NUM_FUNC = NUM_FUNC
																		   AND    MOTIVO <> '43'
																		   AND    (
																						 DTFIM IS NULL
																				  OR     DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)))
																	AND
																	EU.MUNICIPIO = UE.MUNICIPIO AND EU.ID_REGIONAL = UE.ID_REGIONAL ) S WHERE DISCIPLINA_INGRESSO = DF.AGRUPAMENTO )A) AS CH_LIVRE_MUNICIPIO,
																	--FIM CHLIVRE MUNICPIO
*/
																	convert(varchar(10),'') AS CH_LIVRE_MUNICIPIO,DF.UNIDADE_ENS, DF.AGRUPAMENTO INTO #TAB_CH_LIVRE FROM LY_DOCENTE_FUNCAO_GLP DF (NOLOCK) JOIN LY_DOCENTE DD (NOLOCK) ON DD.MATRICULA = DF.MATRICULA JOIN LY_GRUPO_HABILITACAO DI (NOLOCK) ON DI.AGRUPAMENTO = DF.AGRUPAMENTO JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = DF.UNIDADE_ENS WHERE DF.STATUS = 'Aguardando'
																	AND
																	 UE.ID_REGIONAL = ?
																	AND
																	DF.ANO = DATEPART(YEAR ,GETDATE())
																	AND
																	DF.MES = DATEPART(MONTH ,GETDATE()) ");

            if (!string.IsNullOrEmpty(unidade))
                sql.Append(" and ue.UNIDADE_ENS = ? ");

            sql.Append(@"
                set rowcount 1 
                select @id_docente_funcao_glp = id_docente_funcao_glp from #tab_ch_livre 
                while @@rowcount <> 0 
                begin 
                    set rowcount 0
                     
                     select @ch_livre = ch_livre from #tab_ch_livre where id_docente_funcao_glp = @id_docente_funcao_glp      
                     update ly_docente_funcao_glp 
					 set chlivre = @ch_livre ,
					 CHLIVREMUNICIPIO = 0
					 where id_docente_funcao_glp = @id_docente_funcao_glp
                     delete #tab_ch_livre where id_docente_funcao_glp = @id_docente_funcao_glp 
                     set rowcount 1 
                     select @id_docente_funcao_glp = id_docente_funcao_glp from #tab_ch_livre
                end 
                set rowcount 0

                drop table #tab_ch_livre");

            QueryTable qt = new QueryTable(sql.ToString());

            try
            {
                qt.CommandTimeout = 900;
                if (!string.IsNullOrEmpty(unidade))
                    qt.Query(cn, regional, unidade);
                else
                    qt.Query(cn, regional);
            }
            finally
            {
                cn.Close();
            }

            return qt;
        }

        public static string VerificaValorGLP(string ano, string mes, string funcao, string tipo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"select valor from LY_VALOR_GLP where ANO = @ANO and MES = @MES and FUNCAO = @FUNCAO and (TIPO = @TIPO or TIPO is null) "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@MES", mes);
                contextQuery.Parameters.Add("@FUNCAO", funcao);
                contextQuery.Parameters.Add("@TIPO", tipo);

                var retorno = ctx.GetReturnValue(contextQuery);

                return (string)retorno;
            }
        }

        public static DataTable ConsultaDocente(string matricula)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT D.NUM_FUNC,D.MATRICULA,P.NOME_COMPL,P.PESSOA,P.FONE
                            FROM   ly_docente d 
                            inner join ly_pessoa p (NOLOCK) 
                                   on d.pessoa=p.pessoa
                            where D.MATRICULA = @MATRICULA
                          "
                };
                contextQuery.Parameters.Add("@MATRICULA", matricula);
                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool ehSolicitacaoPendente(decimal idDocenteFuncaoGlp)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool existe = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                                FROM    LY_DOCENTE_FUNCAO_GLP
                                WHERE   ID_DOCENTE_FUNCAO_GLP = @ID_DOCENTE_FUNCAO_GLP
                                        AND STATUS = 'Aguardando' "
                };

                contextQuery.Parameters.Add("@ID_DOCENTE_FUNCAO_GLP", idDocenteFuncaoGlp);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public int ObtemQuantidadeTotalPendenteEAceito(DataContext contexto, string matricula, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT (SUM(isnull(GLP_SOLICITADA,0)) - SUM(isnull(GLP_CANCELADA,0))) as GLP
                            FROM LY_DOCENTE_FUNCAO_GLP (NOLOCK)
                            WHERE STATUS in ('Aguardando','Aceita')
                                    AND ANO = @ANO
									AND MATRICULA = @MATRICULA";

                contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_ALFALARGE, matricula);
                contextQuery.Parameters.Add("@ANO", ano);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["GLP"] == DBNull.Value ? 0 : Convert.ToInt32(reader["GLP"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }
    }
}

