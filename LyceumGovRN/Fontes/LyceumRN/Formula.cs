using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Library;
using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN
{
    public class Formula : RNBase
    {
        #region Validações de Fórmulas para LY_DISCIPLINA e LY_PROVA_DISCIP

        /// <summary>
        /// Valida a sintaxe da fórmula e verifica se os identificadores
        /// das provas existem na base de dados para determinada disciplina.
        /// Utilizado para validação dos critérios de avaliação da disciplina.
        /// </summary>        
        public static RetValue ValidaFormulaDisciplina(String formula, String disciplina, TConnectionWritable connection)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, disciplina, ref _sMsg, null, 0, 0, null, ref _sMedia, ref _sAprovacao, null, null, null, null, null, null, connection);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }


        /// <summary>
        /// Valida a fórmula de uma prova da disciplina (LY_PROVA_DISCIP), levando
        /// em consideração o nome e ordem da prova.
        /// </summary>        
        public static RetValue ValidaFormulaProvaDisciplina(String formula, String disciplina, String prova, decimal ordem)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, disciplina, ref _sMsg, null, 0, 0, null,
                ref _sMedia, ref _sAprovacao, prova, ordem, null, null, null, null);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }

        /// <summary>
        /// Valida a fórmula de uma prova da disciplina (LY_PROVA_DISCIP), levando
        /// em consideração o nome e ordem da prova.
        /// </summary> 
        public static RetValue ValidaFormulaProvaDisciplina(TConnectionWritable connection, String formula, String disciplina, String prova, decimal ordem)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, disciplina, ref _sMsg, null, 0, 0, null,
                ref _sMedia, ref _sAprovacao, prova, ordem, null, null, null, null, connection);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }

        #endregion
        
        #region Validações de Campos

        public static RetValue VerificaIdentificadorProva(String prova)
        {
            //Verifica se o nome da prova é igual a alguma palavra reservada
            String[] funcoes = new String[] { "MAX", "MIN", "TRUNCATE", "ROUND", "ISNULL" };
            for (int i = 0; i < funcoes.Count(); i++)
                if (prova.ToUpper().Equals(funcoes[i]))
                    return new RetValue(false, "", new ErrorList("O Instrumento não pode ser igual à palavra \"" + funcoes[i] + "\""));

            //Verifica se o nome do instrumento contém algum caractere não permitido
            if (prova.ToCharArray()
                .Where(c => c == ' ' || c == '.' || c == ',' || c == '-' ||
                           c == '+' || c == '*' || c == '/' || c == '(' || c == ')')
                .Count() > 0)
                return new RetValue(false, "", new ErrorList("O Instrumento contém caracteres não permitidos."));
            return null;
        }

        #endregion

        public static RetValue ValidaFormulaTurma(String formula, String disciplina, String turma, int ano, int semestre, String prova, decimal? ordem)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, disciplina, ref _sMsg, turma, ano, semestre, null, ref _sMedia, ref _sAprovacao, prova, ordem, null, null, null, null);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }

        /// <summary>
        /// Valida a sintaxe da fórmula e verifica se os identificadores
        /// das provas existem na base de dados para determinada serie.
        /// </summary>
        /// <param name="formula">Fórmula a ser validada.</param>
        /// <param name="disciplina">Código da serie.</param>
        /// <returns>Retorno contendo o resultado da validação da fórmula.</returns>
        public static RetValue ValidaFormulaSerie(String formula, String curso, String turno, String curriculo, decimal? serie, String prova, decimal? ordem)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, null, ref _sMsg, null, 0, 0, null, ref _sMedia, ref _sAprovacao, prova, ordem, serie, curso, turno, curriculo);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }

        public static RetValue ValidaFormulaSerie(TConnectionWritable connection, String formula, String curso, String turno, String curriculo, decimal? serie, String prova, decimal? ordem)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, null, ref _sMsg, null, 0, 0, null, ref _sMedia, ref _sAprovacao, prova, ordem, serie, curso, turno, curriculo, connection);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }
        /// <summary>
        /// Valida a sintaxe da fórmula e verifica se os identificadores
        /// das provas existem na base de dados para determinada serie.
        /// </summary>
        /// <param name="formula">Fórmula a ser validada.</param>
        /// <param name="disciplina">Código da serie.</param>
        /// <returns>Retorno contendo o resultado da validação da fórmula.</returns>
        public static RetValue ValidaFormulaSerie(String formula, String curso, String turno, String curriculo, decimal? serie)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, null, ref _sMsg, null, 0, 0, null, ref _sMedia, ref _sAprovacao, null, null, serie, curso, turno, curriculo);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }

        public static RetValue ValidaFormulaSerie(TConnectionWritable connection, String formula, String curso, String turno, String curriculo, decimal? serie)
        {
            String _sMedia = null, _sAprovacao = null, _sMsg = null;
            Boolean provaValida = ProcessaFormulaMedia(formula, null, ref _sMsg, null, 0, 0, null, ref _sMedia, ref _sAprovacao, null, null, serie, curso, turno, curriculo, connection);
            if (provaValida)
                return new RetValue(true, "Fórmula válida.", null);
            else
                return new RetValue(false, "", new ErrorList(_sMsg));
        }

        #region Cálculo de Conceito de Avaliação ProvaTurma de Alunos

        /// <summary>
        /// Calcula o conceito de um aluno para uma fórmula de uma ProvaTurma
        /// </summary>        
        public static RetValue CalculaConceitoProvaAluno(TConnectionWritable connection, String formula, String disciplina, String turma, int? ano, int? semestre, String aluno, out String conceito)
        {
            String sMsg = null, sAprovacao = null;
            conceito = null;
            Formula.ProcessaFormulaMedia(formula, disciplina, ref sMsg, turma, (int?)ano, (int?)semestre, aluno, ref conceito, ref sAprovacao,
                        null, null, null, null, null, null, connection);

            if (String.IsNullOrEmpty(sMsg)) return VerificarErro(connection.GetErrors());
            else return new RetValue(false, "", new ErrorList(sMsg));
        }

        /// <summary>
        /// Calcula o conceito de um aluno para uma fórmula de uma ProvaTurma
        /// </summary>        
        public static RetValue CalculaConceitoProvaAluno(String formula, String disciplina, String turma, int? ano, int? semestre, String aluno, out String conceito)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                return CalculaConceitoProvaAluno(connection, formula, disciplina, turma, ano, semestre, aluno, out conceito);
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Processamento de Fórmulas

        #region Tradução clsDisciplina.cls

        /// <summary>
        /// Verifica se uma fórmula de cálculo de média final é válida
        /// Cálculo da média de um aluno a partir da Fórmula
        /// Observacão:
        ///    - Para checar se a fórmula de uma disciplina é válida,
        ///      indicar somente a chave de LY_DISCIPLINA
        ///    - Para checar se a fórmula de uma turma é válida,
        ///      indicar somente a chave de LY_TURMA
        ///    - Para obter a média de um aluno a partir da fórmula de uma turma,
        ///      indicar a chave de LY_MATRICULA
        /// Os campos opcionais devem ser preenchidos de acordo com o resultado
        /// que se deseja obter, dos três resultados possíveis citados acima.
        /// </summary>
        /// <param name="sFormula">Fórmula</param>
        /// <param name="sDisciplina">Disciplina</param>
        /// <param name="sMsg">Mensagem de erro</param>
        /// <param name="_sTurma">Turma</param>
        /// <param name="_iAno">Ano</param>
        /// <param name="_iSemestre">Semestre</param>
        /// <param name="_sAluno">Aluno</param>
        /// <param name="_sMedia">Média do aluno</param>
        /// <param name="_sAprovacao"></param>
        /// <param name="connection">Conexão que será utilizada (deve estar aberta)</param>
        /// <returns>Fórmula é válida</returns>
        public static Boolean ProcessaFormulaMedia(String sFormula, String sDisciplina,
                                            ref String sMsg, String _sTurma,
                                            int? _iAno, int? _iSemestre,
                                            String _sAluno, ref String _sMedia,
                                            ref String _sAprovacao, String _novaProvaNome, 
                                            decimal? _novaProvaOrdem, decimal? sSerie,
                                            String sCurso, String sTurno, String sCurriculo,
                                            TConnectionWritable connection)
        {
            Boolean ProcessaFormulaMedia = false;

            ESTADO iEstado = (ESTADO)0;
            int iTam = 0;                           //'tamanho da string a ser analisada
            long iPerc = 0;
            String sAux = null;                     //'último identificador analisado
            int iParenteses = 0;                    //'número de abre parênteses não pareados
            int[] iParentesesFuncao = new int[20];  //'para cada função encontrada, contém
            //'o número de parênteses abertos
            //'depois da função
            int[] iVirgulaFuncao = new int[20];     //'para cada função encontrada, contém
            //'o número de vírgulas
            int iTopoAchou = 0;
            Boolean bNum = false;
            String sValor = null, sFormulaAritmetica = null;
            //Double dMedia = 0;
            Boolean[] bISNULL = new Boolean[20];
            Boolean[] bISFUNC = new Boolean[20];

            if (!StrFilled(sFormula))
            {
                sMsg = "Fórmula vazia";
                return false;
            }

            sFormula = StrTranslate(sFormula, vbCr.ToString(), " ");
            sFormula = StrTranslate(sFormula, vbLf.ToString(), " ");
            sFormula = StrTranslate(sFormula, vbTab.ToString(), " ");
            //' sFormula = StrTranslate(sFormula, ".", ",")
            sFormula = Trim(sFormula);
            //' sFormula = StrTranslate(sFormula, " ", "")   'não funciona a troca por delimitador Empty

            sFormulaAritmetica = sFormula;

            Boolean continua = true;
            while (continua)
            {
                switch (iEstado)
                {
                    #region ESTADO.EST_INICIO

                    case ESTADO.EST_INICIO:
                        iPerc = 1;
                        iTopoAchou = -1;
                        bNum = false;
                        iTam = Len(sFormula);
                        iParenteses = 0; //'    número de parênteses abertos
                        sMsg = "";
                        LimpaBranco(sFormula, ref iPerc, iTam);
                        sAux = "";
                        if (IsAlfa(sFormula, iPerc))
                            iEstado = ESTADO.EST_ALFANUM;
                        else if (IsNum(sFormula, iPerc))
                        {
                            bNum = true;
                            iEstado = ESTADO.EST_ALFANUM;
                        }
                        else if (Mid(sFormula, (int)iPerc, 1) == "(")
                            iEstado = ESTADO.EST_ABPARENT;
                        else
                        {
                            sMsg = "Fórmula deve começar com uma expressão ou função";
                            iEstado = ESTADO.EST_ERRO;
                        }
                        break;

                    #endregion

                    #region ESTADO.EST_ABPARENT

                    case ESTADO.EST_ABPARENT:
                        iParenteses = iParenteses + 1;
                        if (iTopoAchou >= 0)
                        {
                            iParentesesFuncao[iTopoAchou] = iParentesesFuncao[iTopoAchou] + 1;
                        }
                        iPerc = iPerc + 1;
                        LimpaBranco(sFormula, ref iPerc, iTam);
                        sAux = "";
                        bNum = false;
                        if (iPerc > iTam)
                        {
                            sMsg = "Fórmula incorreta: deve existir uma expressão ou abre parênteses depois de um abre parênteses";
                            iEstado = ESTADO.EST_ERRO;
                        }
                        else if (IsAlfa(sFormula, iPerc))
                        {
                            iEstado = ESTADO.EST_ALFANUM;
                        }
                        else if (IsNum(sFormula, iPerc))
                        {
                            bNum = true;
                            iEstado = ESTADO.EST_ALFANUM;
                        }
                        else if (Mid(sFormula, (int)iPerc, 1) != "(")
                        {
                            sMsg = "Fórmula incorreta: deve existir uma expressão ou abre parênteses depois de um abre parênteses";
                            iEstado = ESTADO.EST_ERRO;
                        }
                        break;

                    #endregion

                    #region ESTADO.EST_ALFANUM

                    case ESTADO.EST_ALFANUM:
                        sAux = sAux + Mid(sFormula, (int)iPerc, 1);
                        iPerc = iPerc + 1;

                        if (iPerc > iTam)
                        {
                            //FFS
                            if (!bNum)
                            {
                                if (ValidProva(connection, sAux, sDisciplina, _sTurma, _iAno.Value, _iSemestre.Value, _sAluno, ref sValor, ref sMsg, null, _novaProvaNome, _novaProvaOrdem, sSerie, sCurso, sTurno, sCurriculo))
                                {
                                    iEstado = ESTADO.EST_FIM; //'@
                                    ValTranslate(ref sFormulaAritmetica, sAux, sValor);
                                }
                                else
                                {
                                    iEstado = ESTADO.EST_ERRO;
    //                                sMsg += "Fórmula incorreta: identificador \"" + sAux + "\" de prova não cadastrado.";
                                }
                            }
                            else
                            {
                                iEstado = ESTADO.EST_FIM;
                            }
                        }
                        else if (IsNum(sFormula, iPerc) == true)
                        {
                            iEstado = ESTADO.EST_ALFANUM;
                        }
                        else if (IsAlfa(sFormula, iPerc) == true || Mid(sFormula, (int)iPerc, 1) == "_")
                        {
                            if (bNum == true)
                                iEstado = ESTADO.EST_ERRO;
                            else
                                iEstado = ESTADO.EST_ALFANUM;
                        }
                        else if (Mid(sFormula, (int)iPerc, 1) == "(")
                        {
                            if (UCase(sAux) == "MIN" ||
                                UCase(sAux) == "MAX" ||
                                UCase(sAux) == "ROUND" ||
                                UCase(sAux) == "TRUNCATE" ||
                                UCase(sAux) == "ISNULL")
                            {
                                iTopoAchou = iTopoAchou + 1;
                                iParentesesFuncao[iTopoAchou] = 0;

                                //Controlar o caso especial da função ISNULL que possui 3 param.
                                if (UCase(sAux) == "ISNULL")
                                    bISNULL[iTopoAchou] = true;
                                if (UCase(sAux) == "MAX" || UCase(sAux) == "MIN" || UCase(sAux) == "ROUND" || UCase(sAux) == "TRUNCATE")
                                    bISFUNC[iTopoAchou] = true;

                                iEstado = ESTADO.EST_ABPARENT;
                            }
                            else
                            {
                                sMsg = "Fórmula incorreta: funções permitidas MIN, MAX, TRUNCATE, ROUND, ISNULL";
                                iEstado = ESTADO.EST_ERRO;
                            }
                        }
                        else
                        {
                            LimpaBranco(sFormula, ref iPerc, iTam);
                            if (IsOper(sFormula, iPerc) == true)
                            {
                                iEstado = ESTADO.EST_OPER;
                                //FFS
                                if (!bNum)
                                {
                                    if (!ValidProva(connection, sAux, sDisciplina, _sTurma, _iAno.Value, _iSemestre.Value, _sAluno, ref sValor, ref sMsg, null, _novaProvaNome, _novaProvaOrdem, sSerie, sCurso, sTurno, sCurriculo))
                                    {
                                        iEstado = ESTADO.EST_ERRO;
     //                                   sMsg += "Fórmula incorreta: identificador de prova \"" + sAux + "\" não cadastrado.";
                                    }
                                    else
                                        ValTranslate(ref sFormulaAritmetica, sAux, sValor);
                                }

                                if (Mid(sFormula, (int)iPerc, 1) == ",")
                                {
                                    if (iTopoAchou >= 0)
                                    {
                                        if (iParentesesFuncao[iTopoAchou] > 1)
                                        {
                                            iEstado = ESTADO.EST_ERRO;
                                            sMsg = "Fórmula incorreta: ',' deve separar argumentos de função.";
                                        }
                                        //local virgula
                                        iVirgulaFuncao[iTopoAchou] = iVirgulaFuncao[iTopoAchou] + 1;
                                    }
                                }
                            }
                            //senao iEstado = EST_ERRO
                            else if (Mid(sFormula, (int)iPerc, 1) == ")")
                            {
                                //FFS
                                iEstado = ESTADO.EST_FCPARENT;
                                if (!bNum)
                                {
                                    if (!ValidProva(connection, sAux, sDisciplina, _sTurma, _iAno.Value, _iSemestre.Value, _sAluno, ref sValor, ref sMsg, null, _novaProvaNome, _novaProvaOrdem, sSerie, sCurso, sTurno, sCurriculo))
                                    {
                                        iEstado = ESTADO.EST_ERRO;
  //                                      sMsg += "Fórmula incorreta: identificador de prova \"" + sAux + "\" não cadastrado.";
                                    }
                                    else
                                    {
                                        ValTranslate(ref sFormulaAritmetica, sAux, sValor);
                                    }
                                }
                            }
                            else
                            {
                                sMsg = "Fórmula incorreta: uma expressão deve ser seguida por um fecha parênteses ou um operador";
                                iEstado = ESTADO.EST_ERRO;
                            }
                        }
                        break;

                    #endregion

                    #region ESTADO.EST_OPER

                    case ESTADO.EST_OPER:
                        iPerc = iPerc + 1;
                        LimpaBranco(sFormula, ref iPerc, iTam);
                        sAux = "";
                        bNum = false;
                        if (iPerc > iTam)
                            iEstado = ESTADO.EST_ERRO;
                        else if (IsAlfa(sFormula, iPerc))
                            iEstado = ESTADO.EST_ALFANUM;
                        else if (IsNum(sFormula, iPerc))
                        {
                            bNum = true;
                            iEstado = ESTADO.EST_ALFANUM;
                        }
                        else if (Mid(sFormula, (int)iPerc, 1) == "(")
                            iEstado = ESTADO.EST_ABPARENT;
                        else
                        {
                            sMsg = "Fórmula incorreta: operador deve ser seguido por uma expressão";
                            iEstado = ESTADO.EST_ERRO;
                        }
                        break;

                    #endregion

                    #region ESTADO.EST_FCPARENT

                    case ESTADO.EST_FCPARENT:
                        iParenteses = iParenteses - 1;
                        if (iTopoAchou >= 0)
                        {
                            iParentesesFuncao[iTopoAchou] = iParentesesFuncao[iTopoAchou] - 1;

                            if (iParentesesFuncao[iTopoAchou] == 0)
                            {
                                if (bISFUNC[iTopoAchou])
                                {
                                    if (iVirgulaFuncao[iTopoAchou] != 1)
                                    {
                                        sMsg = "Fórmula incorreta: as funções MAX, MIN, ROUND e TRUNCATE requerem dois parâmetros";
                                        iEstado = ESTADO.EST_ERRO;
                                        continue;
                                    }
                                    bISFUNC[iTopoAchou] = false;
                                }

                                //' iVirgulaFuncao(iTopoAchou) = iVirgulaFuncao(iTopoAchou) - 1
                                if (bISNULL[iTopoAchou])
                                {
                                    //Sempre vai ter uma virgula a mais no fechamento da ISNULL pois esta contem 3 parametros
                                    if (iVirgulaFuncao[iTopoAchou] != 2)
                                    {
                                        sMsg = "Fórmula incorreta: a função ISNULL requer três parâmetros";
                                        iEstado = ESTADO.EST_ERRO;
                                        continue;
                                    }
                                    //ZERAR O VETOR bISNULL para não dar erro quando aparecer outra função isnull na fórmula.
                                    bISNULL[iTopoAchou] = false;
                                }
                                iVirgulaFuncao[iTopoAchou] = 0;
                                iTopoAchou = iTopoAchou - 1;
                            }
                        }
                        iPerc = iPerc + 1;
                        LimpaBranco(sFormula, ref iPerc, iTam);
                        sAux = "";
                        bNum = false;
                        if (iParenteses < 0)
                        {
                            sMsg = "Fórmula incorreta: fecha parênteses sem abre parênteses correspondente";
                            iEstado = ESTADO.EST_ERRO;
                        }
                        else if (iPerc > iTam)
                        {
                            if (iParenteses > 0)
                            {
                                sMsg = "Fórmula incorreta: abre parênteses sem fecha parênteses correspondente";
                                iEstado = ESTADO.EST_ERRO;
                            }
                            else
                                iEstado = ESTADO.EST_FIM;
                        }
                        else if (IsOper(sFormula, iPerc))
                        {
                            iEstado = ESTADO.EST_OPER;
                            if (iTopoAchou >= 0)
                            {
                                if (iParentesesFuncao[iTopoAchou] > 1 && Mid(sFormula, (int)iPerc, 1) == ",")
                                {
                                    iEstado = ESTADO.EST_ERRO;
                                    sMsg = "Fórmula incorreta: operador deve ser seguido por expressão";
                                }
                                else if (Mid(sFormula, (int)iPerc, 1) == ",")
                                {
                                    iVirgulaFuncao[iTopoAchou] = iVirgulaFuncao[iTopoAchou] + 1;
                                }
                            }
                        }
                        break;

                    #endregion

                    #region ESTADO.EST_ERRO

                    case ESTADO.EST_ERRO:
                        ProcessaFormulaMedia = false;
                        continua = false;
                        break;

                    #endregion

                    #region ESTADO.EST_FIM

                    case ESTADO.EST_FIM:
                        if (iParenteses > 0)
                        {
                            sMsg = "Fórmula incorreta: abre parênteses sem fecha parênteses correspondente";
                            ProcessaFormulaMedia = false;
                        }
                        else if (iParenteses < 0)
                        {
                            sMsg = "Fórmula incorreta: fecha parênteses sem abre parênteses correspondente";
                            ProcessaFormulaMedia = false;
                        }
                        else
                        {
                            ProcessaFormulaMedia = true;
                        }
                        continua = false;
                        break;

                    #endregion
                }
            }

            if (ProcessaFormulaMedia && StrFilled(_sAluno))
            {
                //' sMedia = CalculoFormula(sFormulaAritmetica, sMsg)

                if (InStr(UCase(sFormulaAritmetica), "MAX") > 0 ||
                   InStr(UCase(sFormulaAritmetica), "MIN") > 0 ||
                   InStr(UCase(sFormulaAritmetica), "ROUND") > 0 ||
                   InStr(UCase(sFormulaAritmetica), "TRUNCATE") > 0 ||
                   InStr(UCase(sFormulaAritmetica), "ISNULL") > 0 ||
                   InStr(UCase(sFormulaAritmetica), "N") > 0) //Considerar valores nulos
                {
                    // Se houver "N" (notação para nota nula), DEVE haver o processamento
                    // para o "N" ser substituído por zero?
                    // A fórmula possui a função MAX,MIN aplicar a função antiga
                    // sFormulaAritmetica = Replace(sFormulaAritmetica, ",", ".")
//                    _sMedia = CalculoFormula(sFormulaAritmetica, ref sMsg);
                    _sMedia = Calculadora.CalcularExpressao(sFormulaAritmetica, ref sMsg);
                }                
                else
                {
                    //Precisa verificar se há vírgula na fórmula para mudar para ponto.
                    //Essa função somente deve ser utilizada para as 4 operaçoes(+-*/)
                    sFormulaAritmetica = Replace(sFormulaAritmetica, ",", ".");
                    //dMedia = Evaluate(sFormulaAritmetica);                    
//                    dMedia = Convert.ToDouble(CalculoFormula(sFormulaAritmetica, ref sMsg).Replace(".", ","));                    
//                    _sMedia = str(dMedia);
                    _sMedia = Calculadora.CalcularExpressao(sFormulaAritmetica, ref sMsg);
                }
                
                _sMedia = RN.Conceito.FormatarNota(sDisciplina, _sMedia);               

                //* FFS: Ou devo alterar ValidProva p/ não retornar mensagem "EOF" ?
                if (sMsg == "EOF")
                    sMsg = "Nota não preenchida";
                else
                    ProcessaFormulaMedia = !StrFilled(sMsg);
            }
            return ProcessaFormulaMedia;
        }

        /// <summary>
        /// Verifica se uma fórmula de cálculo de média final é válida
        /// Cálculo da média de um aluno a partir da Fórmula
        /// Observacão:
        ///    - Para checar se a fórmula de uma disciplina é válida,
        ///      indicar somente a chave de LY_DISCIPLINA
        ///    - Para checar se a fórmula de uma turma é válida,
        ///      indicar somente a chave de LY_TURMA
        ///    - Para obter a média de um aluno a partir da fórmula de uma turma,
        ///      indicar a chave de LY_MATRICULA
        /// Os campos opcionais devem ser preenchidos de acordo com o resultado
        /// que se deseja obter, dos três resultados possíveis citados acima.
        /// </summary>
        /// <param name="sFormula">Fórmula</param>
        /// <param name="sDisciplina">Disciplina</param>
        /// <param name="sMsg">Mensagem de erro</param>
        /// <param name="_sTurma">Turma</param>
        /// <param name="_iAno">Ano</param>
        /// <param name="_iSemestre">Semestre</param>
        /// <param name="_sAluno">Aluno</param>
        /// <param name="_sMedia">Média do aluno</param>
        /// <param name="_sAprovacao"></param>        
        /// <returns>Fórmula é válida</returns>
        public static Boolean ProcessaFormulaMedia(String sFormula, String sDisciplina,
                                            ref String sMsg, String _sTurma,
                                            int? _iAno, int? _iSemestre,
                                            String _sAluno, ref String _sMedia,
                                            ref String _sAprovacao, String _novaProvaNome, 
                                            decimal? _novaProvaOrdem, decimal? sSerie,
                                            String sCurso, String sTurno, String sCurriculo)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try 
	        {
                return ProcessaFormulaMedia(sFormula, sDisciplina, ref sMsg, _sTurma, _iAno, _iSemestre, 
                    _sAluno, ref _sMedia, ref _sAprovacao, _novaProvaNome, _novaProvaOrdem, sSerie, sCurso, sTurno, sCurriculo, connection);
	        }
	        finally
	        {
                connection.Close();
    	    }            
        }

        #endregion


        #region Tradução LyceumGlobal.bas

        /// <summary>
        /// OK. Função para o cálculo de uma expressão aritmética.
        /// SubFunções: Empilha, Desempilha, HeadExpression, TipoCalculo, Calcula
        /// </summary>
        /// <param name="sFormula">Fórmula aritmética</param>
        /// <param name="pMsg">Mensagem de erro</param>
        /// <returns>Resultado do Cálculo</returns>
        public static String CalculoFormula(String sFormula, ref String pMsg)
        {
            //Variaves utilizadas como estruturas auxiliares de pilha
            String[] aPilha = new String[30];
            int iPilha;

            //Variáveis utilizadas para a análise parcial dos componentes da Formula
            String StrHead;
            tTipoCalculo tType = (tTipoCalculo)0;
            String sExpressao;
            String sUnidade;
            String sArg1;
            String sArg2;
            String sArg3;

            try
            {
                //Inicialização das variáveis
                iPilha = 0;
                sExpressao = sFormula;
                sExpressao = sExpressao.Replace("*", " * ");

                while (Len(sExpressao) > 0 || iPilha > 1)
                {
                    sUnidade = Trim(HeadExpression(ref tType, ref sExpressao));

                    //'Debug.Print sUnidade
                    //'Análise do tipo de dado obtido pela expressao, para empilhamento ou cálculo
                    switch (tType)
                    {
                        #region TP_ABPARENT, TP_OPER, TP_NUMERO

                        case tTipoCalculo.TP_ABPARENT:
                        case tTipoCalculo.TP_OPER:
                        case tTipoCalculo.TP_NUMERO:
                            Empilha(sUnidade, ref aPilha, ref iPilha);
                            break;

                        #endregion

                        #region TP_OPERPRIOR

                        case tTipoCalculo.TP_OPERPRIOR:
                            StrHead = HeadExpression(ref tType, ref sExpressao);
                            if (tType == tTipoCalculo.TP_ABPARENT)
                                Empilha(sUnidade, ref aPilha, ref iPilha);
                            else
                            {
                                string temp = Desempilha(ref aPilha, ref iPilha);
                                string tempCalc = Calcula(StrHead, sUnidade, temp, null);
                                Empilha(tempCalc, ref aPilha, ref iPilha);
                            }                            
                            break;

                        #endregion

                        #region TP_FCPARENT

                        case tTipoCalculo.TP_FCPARENT:
                            while (TipoCalculo(aPilha[iPilha - 1]) == tTipoCalculo.TP_OPER)
                            {
                                string tmp1 = Desempilha(ref aPilha, ref iPilha);
                                string tmp2 = Desempilha(ref aPilha, ref iPilha);
                                string tmp3 = Desempilha(ref aPilha, ref iPilha);
                                string tempCalc = Calcula(tmp1, tmp2, tmp3, null);
                                Empilha(tempCalc, ref aPilha, ref iPilha);
                            }

                            if (TipoCalculo(aPilha[iPilha - 1]) == tTipoCalculo.TP_ABPARENT)
                            {
                                sUnidade = Desempilha(ref aPilha, ref iPilha);
                                Desempilha(ref aPilha, ref iPilha);  //'Desempilhando o "Abre Parenteses"
                                Empilha(sUnidade, ref aPilha, ref iPilha);
                            }
                            else if (TipoCalculo(aPilha[iPilha - 1]) == tTipoCalculo.TP_VIRGULA) //'Execução de Função (2param)
                            {
                                if (iPilha > 6)
                                {
                                    //Tratamento especial para o caso do ISNULL, que tem 3 parametros
                                    if (aPilha[iPilha - 6] == "ISNULL")
                                    {
                                        sArg1 = Desempilha(ref aPilha, ref iPilha);
                                        Desempilha(ref aPilha, ref iPilha); //Desempilhando a "Virgula"
                                        sArg2 = Desempilha(ref aPilha, ref iPilha);
                                        Desempilha(ref aPilha, ref iPilha); //Desempilhando a virgula
                                        sArg3 = Desempilha(ref aPilha, ref iPilha);
                                        Desempilha(ref aPilha, ref iPilha); //Desempilhando o "Abre Parenteses"
                                        Empilha(Calcula(sArg3, Desempilha(ref aPilha, ref iPilha), sArg2, sArg1), ref aPilha, ref iPilha);
                                    }
                                    else
                                    {
                                        sArg1 = Desempilha(ref aPilha, ref iPilha);
                                        Desempilha(ref aPilha, ref iPilha); //Desempilhando a "Virgula"
                                        sArg2 = Desempilha(ref aPilha, ref iPilha);
                                        Desempilha(ref aPilha, ref iPilha); //Desempilhando o "Abre Parenteses"
                                        Empilha(Calcula(sArg2, Desempilha(ref aPilha, ref iPilha), sArg1, null), ref aPilha, ref iPilha);
                                    }
                                }
                                else
                                {
                                    sArg1 = Desempilha(ref aPilha, ref iPilha);
                                    Desempilha(ref aPilha, ref iPilha); //Desempilhando a "Virgula"
                                    sArg2 = Desempilha(ref aPilha, ref iPilha);
                                    Desempilha(ref aPilha, ref iPilha); //Desempilhando o "Abre Parenteses"
                                    Empilha(Calcula(sArg2, Desempilha(ref aPilha, ref iPilha), sArg1, null), ref aPilha, ref iPilha);
                                }
                            }
                            else if (TipoCalculo(aPilha[iPilha - 1]) == tTipoCalculo.TP_OPERPRIOR) //'Execução de Função (2param)
                            {
                                Empilha(Calcula(Desempilha(ref aPilha, ref iPilha), Desempilha(ref aPilha, ref iPilha), Desempilha(ref aPilha, ref iPilha), null), ref aPilha, ref iPilha);
                            }
                            else
                            {
                                pMsg = "Erro de Sintaxe";
                                throw new Exception();
                            }
                            break;

                        #endregion

                        #region TP_VIRGULA

                        case tTipoCalculo.TP_VIRGULA:
                            while (TipoCalculo(aPilha[iPilha - 1]) == tTipoCalculo.TP_OPER)
                                Empilha(Calcula(Desempilha(ref aPilha, ref iPilha), Desempilha(ref aPilha, ref iPilha), Desempilha(ref aPilha, ref iPilha), null), ref aPilha, ref iPilha);
                            Empilha(sUnidade, ref aPilha, ref iPilha);
                            break;

                        #endregion

                        #region TP_VAZIO

                        case tTipoCalculo.TP_VAZIO:
                            Empilha(Calcula(Desempilha(ref aPilha, ref iPilha), Desempilha(ref aPilha, ref iPilha), Desempilha(ref aPilha, ref iPilha), null), ref aPilha, ref iPilha);
                            break;

                        #endregion

                        #region TP_FUNCAO

                        case tTipoCalculo.TP_FUNCAO:
                            //Funções extras reconhecidas pela rotina de cálculo
                            if (UCase(sUnidade) == "MAX" ||
                                UCase(sUnidade) == "MIN" ||
                                UCase(sUnidade) == "TRUNCATE" ||
                                UCase(sUnidade) == "ROUND" ||
                                UCase(sUnidade) == "ISNULL" ||
                                UCase(sUnidade) == "N")
                            {
                                //Se houver "N"(notação para nota nula), DEVE haver o processamento
                                // para o "N" ser substituído por zero
                                Empilha(UCase(sUnidade), ref aPilha, ref iPilha);
                            }                            
                            else
                            {
                                pMsg = "Operação desconhecida: " + sUnidade;
                                throw new Exception();
                            }
                            break;

                        #endregion
                    }
                }
                return Desempilha(ref aPilha, ref iPilha);
            }
            catch (Exception exc)
            {
                throw new ApplicationException("CalculoFormula: Erro de Sintaxe na fórmula", exc.InnerException);
            }
        }

        /// <summary>
        /// OK. Rotina para efetuar o cálculo das operações conhecidas
        /// </summary>        
        private static String Calcula(String sOperador2, String sOperacao, String sOperador1, String _sOperador3)
        {
            Double dValor = 0;            

            if (sOperacao != "ISNULL")
            {
                if (sOperador1 == "N") sOperador1 = "0";
                if (sOperador2 == "N") sOperador2 = "0";
            }
            else
            {
                if (sOperador1 == "N") sOperador1 = "0";
                if (_sOperador3 == "N") _sOperador3 = "0";
            }

            if(sOperador1 != null) sOperador1 = sOperador1.Replace(",", ".");
            if(sOperador2 != null) sOperador2 = sOperador2.Replace(",", ".");
            if(_sOperador3 != null) _sOperador3 = _sOperador3.Replace(",", ".");

            switch (sOperacao)
            {
                case "+":
                    dValor = Val(sOperador1) + Val(sOperador2);
                    break;
                case "-":
                    dValor = Val(sOperador1) - Val(sOperador2);
                    break;
                case "*":
                    dValor = Val(sOperador1) * Val(sOperador2);
                    break;
                case "/":
                    dValor = Val(sOperador1) / Val(sOperador2);
                    break;
                case "MAX":
                    dValor = (double)IIf(Val(sOperador1) > Val(sOperador2), Val(sOperador1), Val(sOperador2));
                    break;
                case "MIN":
                    dValor = (double)IIf(Val(sOperador1) < Val(sOperador2), Val(sOperador1), Val(sOperador2));
                    break;
                case "ROUND":
                    dValor = (double)TVal(cNullZeroFinanc(RoundTruncate(sOperador2, (int)Val(sOperador1), false)));
                    break;
                case "TRUNCATE":
                    dValor = (double)TVal(cNullZeroFinanc(RoundTruncate(sOperador2, (int)Val(sOperador1), true)));
                    break;
                case "ISNULL":
                    dValor = (double)IIf(sOperador2 == "N", Val(sOperador1), Val(_sOperador3));
                    break;
            }

            return Trim(dValor.ToString());
        }

        /// <summary>
        /// OK. Análise do tipo de parâmetro
        /// </summary>
        private static tTipoCalculo TipoCalculo(String sType)
        {
            sType = Mid(sType, 1, 1);

            switch (sType)
            {
                case "(": return tTipoCalculo.TP_ABPARENT;
                case "+":
                case "-":
                    return tTipoCalculo.TP_OPER;
                case "/":
                case "*":
                    return tTipoCalculo.TP_OPERPRIOR;
                case ",":
                    return tTipoCalculo.TP_VIRGULA;
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case ".":
                    return tTipoCalculo.TP_NUMERO;
                case ")":
                    return tTipoCalculo.TP_FCPARENT;
                case "":
                    return tTipoCalculo.TP_VAZIO;
                default:
                    return tTipoCalculo.TP_FUNCAO;
            }
        }

        /// <summary>
        /// OK. Função para controle de empilhamento no vetor parametrizado.
        /// </summary>
        private static void Empilha(String sDado, ref String[] aPilha, ref int iPilha)
        {
            iPilha = iPilha + 1;
            aPilha[iPilha] = sDado;
        }

        /// <summary>
        /// OK. Função para controle de desempilhamento no vetor parametrizado.
        /// </summary>
        private static String Desempilha(ref String[] aPilha, ref int iPilha)
        {
            String Desempilha = aPilha[iPilha];
            iPilha = iPilha - 1;
            return Desempilha;
        }

        /// <summary>
        /// OK OK. Analisador léxico da expressão parametrizada,
        /// retorna uma unidade lógica da expressão
        /// </summary>
        private static String HeadExpression(ref tTipoCalculo sType, ref String sExpressao)
        {
            String sAux;
            String HeadExpression;

            sExpressao = LTrim(sExpressao);
            sType = TipoCalculo(Mid(sExpressao, 1, 1));
            HeadExpression = Mid(sExpressao, 1, 1);
            sExpressao = Mid(sExpressao, 2, Len(sExpressao));

            while (TipoCalculo(HeadExpression) == tTipoCalculo.TP_NUMERO ||
                   TipoCalculo(HeadExpression) == tTipoCalculo.TP_FUNCAO)
            {
                //'Do While Not InStr(1, "+-*/(),", HeadExpression) > 0                
                sAux = Mid(sExpressao, 1, 1);
                if (!(InStr(1, "+-*/(),", sAux) > 0))
                {
                    sExpressao = Mid(sExpressao, 2, Len(sExpressao));
                    HeadExpression = HeadExpression + sAux;
                }
                else
                    break;
            }
            return HeadExpression;
        }

        #endregion


        #region Tradução LyceumGlobal.bas - Métodos Auxiliares

        /// <summary>
        /// OK. Limpa branco
        /// </summary>              
        private static void LimpaBranco(String sFormula, ref long iPos, int iTam)
        {
            while (iPos <= iTam)
            {
                if (Mid(sFormula, (int)iPos, 1) != " ")
                    return;
                iPos = iPos + 1;
            }
        }

        /// <summary>
        /// OK. Identifica se um caractere é um operador
        /// </summary>        
        private static Boolean IsOper(String sString, long _iPos)
        {
            return
                (Mid(sString, (int)_iPos, 1) == "+") ||
                (Mid(sString, (int)_iPos, 1) == "-") ||
                (Mid(sString, (int)_iPos, 1) == "/") ||
                (Mid(sString, (int)_iPos, 1) == "*") ||
                (Mid(sString, (int)_iPos, 1) == ",") ||
                (Mid(sString, (int)_iPos, 1) == ".");
        }

        /// <summary>
        /// OK. Identifica se um caractere é um operador
        /// </summary>        
        private static Boolean IsOper(String sString)
        {
            return IsOper(sString, 1);
        }

        /// <summary>
        /// OK. Identifica se o caractere é número
        /// </summary>
        private static Boolean IsNum(String sString, long _iPos)
        {
            return
                (Mid(sString, (int)_iPos, 1)[0] >= '0') &&
                (Mid(sString, (int)_iPos, 1)[0] <= '9');
        }

        /// <summary>
        /// OK. Identifica se o caractere é número
        /// </summary>
        private static Boolean IsNum(String sString)
        {
            return IsNum(sString, 1);
        }

        /// <summary>
        /// OK. Identifica se o caractere é letra
        /// </summary>
        private static Boolean IsAlfa(String sString, long iPos)
        {
            return ((Mid(sString, (int)iPos, 1)[0] >= 'A') && (Mid(sString, (int)iPos, 1)[0] <= 'Z')) ||
                   ((Mid(sString, (int)iPos, 1)[0] >= 'a') && (Mid(sString, (int)iPos, 1)[0] <= 'z')) ||
                   ((Trim(sString) != "") && (InStr(1, "_#$%&", sString) > 0));
        }

        /// <summary>
        /// OK. Identifica se o caractere é letra
        /// </summary>
        private static Boolean IsAlfa(String sString)
        {
            return IsAlfa(sString, 1);
        }

        /// <summary>
        /// OK. Identifica se uma prova é válida, e o respectivo valor        
        /// </summary>        
        private static Boolean ValidProva(TConnectionWritable connection, String sProva, String sDisciplina, String sTurma, int iAno,
                                  int iSemestre, String sAluno, ref String sValor, ref String sMSG,
                                  String _sGrupo, String _novaProva, decimal? _novaProvaOrdem, decimal? sSerie, String sCurso,
                                  String sTurno, String sCurriculo)
        {
            Boolean pExiste = false;
            String pRegistro = null;
            String sGrupoNota = null;

            if (StrFilled(_sGrupo)) //Validação da prova na Tabela LY_PROVAS_POR_GRUPO
            {
                if (!ExecutaExisteRegistro("SELECT PROVA_NAO_OFICIAL AS PROVA FROM LY_PROVAS_POR_GRUPO " +
                                           "WHERE PROVA_NAO_OFICIAL = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                           "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' AND GRUPO = '" + RN.RNBase.MudarAspas(_sGrupo) + "'",
                                           1, ref pExiste, ref pRegistro, ref sMSG, connection))
                    return false;
            }
            else if (StrFilled(sTurma)) //Validação da prova na Tabela LY_PROVA
            {
                if (String.IsNullOrEmpty(_novaProva) || !_novaProvaOrdem.HasValue)
                {
                    if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA " +
                                               "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                               "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' AND " +
                                               "TURMA = '" + RN.RNBase.MudarAspas(sTurma) + "' AND " +
                                               "ANO = " + iAno + " AND " +
                                               "SEMESTRE = " + iSemestre,
                                               1, ref pExiste, ref pRegistro, ref sMSG, connection))
                        return false;
                }
                //Validação de fórmula da LY_PROVA_UNIDADE_CURSO
                else
                {
                    if (_novaProva.ToUpper().Equals(sProva.ToUpper()))
                    {
                        sMSG = "A prova não pode fazer parte de sua própria fórmula.<br/>";
                        return false;
                    }

                    if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA " +
                                               "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                               "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' AND " +
                                               "TURMA = '" + RN.RNBase.MudarAspas(sTurma) + "' AND " +
                                               "ANO = " + iAno + " AND " +
                                               "SEMESTRE = " + iSemestre,
                                               1, ref pExiste, ref pRegistro, ref sMSG, connection))
                        return false;

                    if (pExiste)
                    {
                        Boolean pOrdemOk = false;
                        //Prova cadastrada
                        if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA " +
                                                   "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                                   "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' AND " +
                                                   "TURMA = '" + RN.RNBase.MudarAspas(sTurma) + "' AND " +
                                                   "ANO = " + iAno + " AND " +
                                                   "SEMESTRE = " + iSemestre + " AND " +
                                                   "ORDEM <= '" + RN.RNBase.MudarAspas(_novaProvaOrdem) + "'",
                                                   1, ref pOrdemOk, ref pRegistro, ref sMSG, connection))
                            return false;

                        if (!pOrdemOk)
                        {
                            sMSG = "Verifique os instrumentos que fazem parte da fórmula.<br/>Verifique se eles são de ordem inferior a ordem do instrumento.<br/>";
                            return false;
                        }
                    }
                }
            }
            else if (StrFilled(sSerie)) //Validação da prova na Tabela LY_PROVA_UNIDADE_CURSO
            {
                if (String.IsNullOrEmpty(_novaProva) || !_novaProvaOrdem.HasValue)
                {
                    if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA_UNIDADE_CURSO " +
                                               "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                               "SERIE = " + sSerie + " AND " +
                                               "CURSO = '" + RN.RNBase.MudarAspas(sCurso) + "' AND " +
                                               "TURNO = '" + RN.RNBase.MudarAspas(sTurno) + "' AND " +
                                               "CURRICULO = '" + RN.RNBase.MudarAspas(sCurriculo) + "'",
                                               1, ref pExiste, ref pRegistro, ref sMSG, connection))
                        return false;
                }
                //Validação de fórmula da LY_PROVA_UNIDADE_CURSO
                else
                {
                    if (_novaProva.ToUpper().Equals(sProva.ToUpper()))
                    {
                        sMSG = "O instrumento não pode fazer parte de sua própria fórmula.<br/>";
                        return false;
                    }

                    if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA_UNIDADE_CURSO " +
                                               "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                               "SERIE = " + sSerie + " AND " +
                                               "CURSO = '" + RN.RNBase.MudarAspas(sCurso) + "' AND " +
                                               "TURNO = '" + RN.RNBase.MudarAspas(sTurno) + "' AND " +
                                               "CURRICULO = '" + RN.RNBase.MudarAspas(sCurriculo) + "'",
                                               1, ref pExiste, ref pRegistro, ref sMSG, connection))
                        return false;

                    if (pExiste)
                    {
                        Boolean pOrdemOk = false;
                        //Prova cadastrada
                        if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA_UNIDADE_CURSO " +
                                                   "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                                   "SERIE = " + sSerie + " AND " +
                                                   "CURSO = '" + RN.RNBase.MudarAspas(sCurso) + "' AND " +
                                                   "TURNO = '" + RN.RNBase.MudarAspas(sTurno) + "' AND " +
                                                   "CURRICULO = '" + RN.RNBase.MudarAspas(sCurriculo) + "' AND " +
                                                   "ORDEM <= '" + RN.RNBase.MudarAspas(_novaProvaOrdem) + "'",
                                                   1, ref pOrdemOk, ref pRegistro, ref sMSG, connection))
                            return false;

                        if (!pOrdemOk)
                        {
                            sMSG = "Verifique os instrumentos que fazem parte da fórmula.<br/>Verifique se eles são de ordem inferior a ordem do instrumento.<br/>";
                            return false;
                        }
                    }
                }
            }
            else //Validação da prova na Tabela LY_PROVA_DISCIP
            {
                //Validação de fórmula da LY_DISCIPLINA
                if (String.IsNullOrEmpty(_novaProva) || !_novaProvaOrdem.HasValue)
                {
                    if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA_DISCIP " +
                                               "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                               "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "'",
                                               1, ref pExiste, ref pRegistro, ref sMSG, connection))
                        return false;
                }
                //Validação de fórmula da LY_PROVA_DISCIP
                else
                {
                    if (_novaProva.ToUpper().Equals(sProva.ToUpper()))
                    {
                        sMSG = "O instrumento não pode fazer parte de sua própria fórmula.<br/>";
                        return false;
                    }

                    if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA_DISCIP " +
                                               "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                               "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "'",
                                               1, ref pExiste, ref pRegistro, ref sMSG, connection))
                        return false;

                    if (pExiste)
                    {
                        Boolean pOrdemOk = false;
                        //Prova cadastrada
                        if (!ExecutaExisteRegistro("SELECT PROVA FROM LY_PROVA_DISCIP " +
                                                   "WHERE PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                                   "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' AND " +
                                                   "ORDEM <= '" + RN.RNBase.MudarAspas(_novaProvaOrdem) + "'",
                                                   1, ref pOrdemOk, ref pRegistro, ref sMSG, connection))
                            return false;

                        if (!pOrdemOk)
                        {
                            sMSG = "Verifique as provas que fazem parte da fórmula da prova e se elas são de ordem inferior a ordem da prova.";
                            return false;
                        }
                    }
                }
            }

            if (!pExiste)
            {
                if (StrFilled(_sGrupo))
                    sMSG = "Instrumento não cadastrado neste grupo.";
                else
                    sMSG = "Fórmula incorreta: identificador de instrumento \"" + sProva + "\" não cadastrado.";
                return false;
            }

            if (StrFilled(sAluno)) //Retorno do Valor
            {
                if (!ExecutaExisteRegistro("SELECT CONCEITO FROM LY_NOTA " +
                                           "WHERE ALUNO = '" + RN.RNBase.MudarAspas(sAluno) + "' AND " +
                                           "PROVA = '" + RN.RNBase.MudarAspas(sProva) + "' AND " +
                                           "DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' AND " +
                                           "TURMA = '" + RN.RNBase.MudarAspas(sTurma) + "' AND " +
                                           "ANO = " + iAno + " AND " +
                                           "SEMESTRE = " + iSemestre,
                                           1, ref pExiste, ref pRegistro, ref sMSG, connection))
                    return false;
                else
                {
                    if (!pExiste || cNull(pRegistro).ToString() == "")
                    {
                        sValor = "N"; //Apesar de ser uma prova válida, não há registro do conceito
                        //* Devo alterar ValidProva p/ não retornar mensagem "EOF" ?
                        sMSG = "";
                    }
                    else
                    {
                        if (StrFilled(pRegistro))
                        {
                            if (Left(pRegistro, 1) == "," || Left(pRegistro, 1) == ".")
                                pRegistro = "0" + pRegistro;
                        }

                        if (!IsNumeric(pRegistro))
                        {
                            //Descobre o Grupo para a disciplina
                            if (!ExecutaExisteRegistro("SELECT GRUPO_NOTA FROM LY_DISCIPLINA " +
                                                       "WHERE DISCIPLINA = '" + RN.RNBase.MudarAspas(sDisciplina) + "' ",
                                                       1, ref pExiste, ref sGrupoNota, ref sMSG, connection))
                                return false;

                            if (!ExecutaExisteRegistro("SELECT NOTA FROM LY_CONCEITO " +
                                                       "WHERE CONCEITO = '" + RN.RNBase.MudarAspas(pRegistro) + "' AND " +
                                                       "GRUPO = '" + RN.RNBase.MudarAspas(sGrupoNota) + "'",
                                                       1, ref pExiste, ref pRegistro, ref sMSG, connection))
                                return false;
                        }
                        //'sValor = TVal(cNullZeroFinanc(pRegistro))
                        if (StrFilled(pRegistro))
                            if (Left(pRegistro, 1) == "," || Left(pRegistro, 1) == ".")
                                pRegistro = "0" + pRegistro;

                        sValor = pRegistro.Replace(",", ".");
                    }
                }
            }
            return true;
        }        

        #endregion


        #region TechLibVB - Métodos

        /// <summary>
        /// OK. OK. Recebe uma variant e devolve Empty se a 
        /// variant for Null. Caso contrário devolve a variant.
        /// </summary>        
        private static object cNull(object v)
        {
            if (v == null)
                return String.Empty;
            else
                return v;
        }

        /// <summary>
        /// OK. OK. Recebe uma Variant e devolve '0' se não for número
        /// e não possuir conteúdo. Caso contrário, devolve a Variant.
        /// </summary>
        private static object cNullZeroFinanc(object v)
        {
            //*If TechLibVB.IsNumericString(s) Then
            if (IsNumeric(v))
            {
                return v;
                //AECJ em 18/10/2000 - a linha original foi comentada.
                //cNullZero = val(v)
            }
            else if (StrFilled(v))
            {
                return v;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// OK. [VERIFICAR][BYREF][BYVAL]. Troca todos os delimitadores presentes 
        /// em [De] pelos respectivos em [Para].
        /// </summary>                
        private static String StrTranslate(String Source, String De, String Para)
        {
            for (int cursor = 0; cursor < De.Length; cursor++)
                Source = Source.Replace(De[cursor], Para[cursor]);
            return Source;
        }

        /// <summary>
        /// OK. OK. ValTranslate
        /// </summary>        
        private static void ValTranslate(ref String sFormula, String sProva, String sValor)
        {
            int i;
            // 'variável de controle do i,
            // 'para não permitir que seja reprocessada uma substituição
            // 'de prova cujo nome é substring do nome de outra prova
            int PosIni;
            long Lenght;
            String sFormulaAux;

            sFormula = "(" + sFormula + ")";
            sFormulaAux = sFormula;
            i = InStr(1, sFormulaAux, sProva);

            Lenght = Len(sProva);

            //'Caso a formula seja somente a prova então substitui logo
            if (!StrFilled(sValor))
                sValor = "0";

            while (i > 0)
            {
                // 'Troca a prova pelo seu valor
                sFormulaAux = Mid(sFormula, 1, i - 1) + sValor + Mid(sFormula, i + (int)Lenght);

                if (sFormula == sProva || i == 1)
                {
                    sFormula = sFormulaAux;
                }
                // 'Verifica se o nome da prova não é substring do nome de outra prova
                else if (!IsAlfa(Mid(sFormula, i - 1, 1)) &&
                         !IsNum(Mid(sFormula, i - 1, 1)) &&
                         !IsAlfa(Mid(sFormula, i + (int)Lenght, 1)) &&
                         !IsNum(Mid(sFormula, i + (int)Lenght, 1)))
                {
                    sFormula = sFormulaAux;
                }

                // 'Caso a proxima ocorrência da prova seja em posição "i" já passada (menor de iAux)
                // ' então devemos evitar o reprocessamento desnecessário,
                // ' pois vai cair em um caso que não deu certo a substituição
                PosIni = i + Len(sValor);
                i = InStr(PosIni, sFormula, sProva);
            }
        }

        /// <summary>
        /// OK. OK. Converte um número para tipo numérico (Double).
        /// A conversão será feita somente se o mesmo estiver em uma String.
        /// O formato numérico da String deve ser o AMERICANO.
        /// Caso o número já seja um número ou seja Null ou Empty, nada é feito
        /// (o próprio será retornado).
        /// Caso seja outra coisa é gerado run-time error.
        private static object TVal(object Number)
        {
            if (Number is String)
                return Val(Replace(Number.ToString(), ",", "."));
            else
                return Number;
        }

        /// <summary>
        /// OK. OK. Executa um comando SELECT. Se o comando foi executado com sucesso, retorna
        /// TRUE. Devolve os valores dos campos do primeiro registro encontrado.
        /// Pode ser utilizado quando se deseja saber se um determinado registro existe,
        /// se existir, devolve os valores do campo do registro encontrado.
        /// </summary>
        /// <param name="pSQL">Comando SQL</param>
        /// <param name="Existe">Indica se existe algum registro devolvido pelo SELECT</param>
        /// <param name="pMsg">Mensagem de erro caso ocorra</param>
        /// <param name="pQuantCampos">Quantidade de campos do SELECT que devem ser devolvidos</param>
        /// <param name="pRegistro">Valores dos campos do registro encontrado(separados por TAB)</param>
        /// <returns>True se o comando SELECT foi executado com sucesso</returns>
        private static Boolean ExecutaExisteRegistro(String pSQL, int pQuantCampos, ref Boolean Existe,
            ref String _pRegistro, ref String _pMsg, TConnectionWritable connection)
        {            
            try
            {
                QueryTable qt = new QueryTable(pSQL);
                qt.Query(connection);

                if (qt.Rows.Count > 0)
                {
                    Existe = true;
                    _pRegistro = String.Empty;
                    for (int i = 0; i < pQuantCampos; i++)
                    {
                        if (qt.Rows[0][i].IsNull)
                            _pRegistro = _pRegistro + "\t";
                        else
                        {
                            _pRegistro = String.IsNullOrEmpty(_pRegistro) ?
                                (qt.Rows[0][i].ToString()) :
                                (_pRegistro + "\t" + qt.Rows[0][i]);
                        }
                    }
                }
                else
                {
                    Existe = false;
                }
            }
            catch (Exception e)
            {
                _pMsg = e.Message;
                return false;
            }            
            return true;
        }

        /// <summary>
        /// OK. Arredonda o número Decimal entrado como parâmetro, 
        /// de acordo com o número de casas decimais especificadas.
        /// </summary>        
        private static decimal RoundDecimal(object d, int casas)
        {
            return Math.Round(Convert.ToDecimal(d.ToString().Replace(".", ",")), casas);
        }

        /// <summary>
        /// OK. [VERIFICAR]
        /// </summary>        
        private static object RoundTruncate(String sVal, int iDec, Boolean bTruncate)
        {
            object RoundTruncate = 0;

            //    'sVal = ConverteStringFloat(sVal , ",", ".", ".", ",")
            //'    If bTruncate = True Then
            //'        RoundTruncate = RoundDecimal(TVal(TruncaMedia(sVal, iDec)), iDec)
            //'    Else
            //'        RoundTruncate = RoundDecimal(TVal(sVal), iDec)
            //'    End If
            sVal = Replace(sVal, ",", ".");
            if (bTruncate == true)
                RoundTruncate = RoundDecimal(TruncaMedia(TVal(sVal).ToString(), iDec), iDec);
            else
                RoundTruncate = RoundDecimal(TVal(sVal), iDec);

            return Replace(RoundTruncate.ToString(), ",", ".");
        }

        /// <summary>
        /// VB OK. TruncaMedia
        /// </summary>
        private static String TruncaMedia(String sValor, int dec)
        {
            String TruncaMedia;
            int posDec, lenDec;

            TruncaMedia = sValor; //'* resultado default *

            sValor = Trim(sValor);
            posDec = InStr(sValor, ".");
            if (posDec <= 0)
                posDec = InStr(sValor, ",");

            lenDec = Len(sValor);

            if (posDec == 0)
                TruncaMedia = sValor;
            else if (posDec + dec <= lenDec)
                TruncaMedia = Left(sValor, posDec + dec);

            return TruncaMedia;
        }

        #endregion


        #region Funções VB6
        /// <summary>
        /// VB OK. Retorna um Boolean indicando se a "expression" pode ser interpretada como um número
        /// </summary>
        private static Boolean IsNumeric(object expression)
        {
            return Microsoft.VisualBasic.Information.IsNumeric(expression);
            //decimal value;
            //return decimal.TryParse(expression.ToString(), out value);
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo um número específico de 
        /// caracteres, iniciando a partir do lado esquerdo da string
        /// </summary>        
        private static string Left(string str, int length)
        {
            return Microsoft.VisualBasic.Strings.Left(str, length);
            //return str.Substring(0, length);
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo um número específico de 
        /// caracteres, iniciando a partir do lado direito da string
        /// </summary>        
        private static string Right(string str, int length)
        {
            return Microsoft.VisualBasic.Strings.Right(str, length);
            //return (str.Length > length) ? str.Substring(str.Length - length) : str;
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo um número específico de caracteres da string
        /// </summary>        
        private static string Mid(string str, int startIndex, int length)
        {
            return Microsoft.VisualBasic.Strings.Mid(str, startIndex, length);
            //return str.Substring(startIndex - 1, length);
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo um número específico de caracteres da string
        /// </summary>       
        private static string Mid(string str, int startIndex)
        {
            return Microsoft.VisualBasic.Strings.Mid(str, startIndex);
            //return str.Substring(startIndex - 1);
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo uma cópia da string 
        /// especificada sem espaços em branco à direita
        /// </summary>    
        private static string LTrim(string param)
        {
            return Microsoft.VisualBasic.Strings.LTrim(param);
            //return param.TrimStart();
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo uma cópia da string 
        /// especificada sem espaços em branco à esquerda
        /// </summary>        
        private static string RTrim(string param)
        {
            return Microsoft.VisualBasic.Strings.RTrim(param);
            //return param.TrimEnd();
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo uma cópia da string 
        /// especificada sem espaços em branco à direita e à esquerda
        /// </summary>        
        private static string Trim(string param)
        {
            return Microsoft.VisualBasic.Strings.Trim(param);
            //return param.Trim();
        }

        /// <summary>
        /// VB OK. Retorna um inteiro contendo o número de caracteres na string ou 
        /// o número nominal de bytes necessários para armazenar a variável
        /// </summary>
        private static int Len(string param)
        {
            return Microsoft.VisualBasic.Strings.Len(param);
            //return param.Length;
        }

        /// <summary>
        /// VB OK. Retorna os números contidos na string como um valor numérico do tipo apropriado.
        /// </summary>
        private static double Val(String param)
        {
            return Microsoft.VisualBasic.Conversion.Val(param);
            //return double.Parse(param);
        }

        /// <summary>
        /// VB OK. Retorna um dos dois objetos, dependendo do resultado da expressão
        /// </summary>
        private static object IIf(bool expression, object TruePart, object FalsePart)
        {
            return Microsoft.VisualBasic.Interaction.IIf(expression, TruePart, FalsePart);
            //if (expression)
            //    return TruePart;
            //else
            //    return FalsePart;
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo a string convertida para caixa alta
        /// </summary>
        private static string UCase(string param)
        {
            return Microsoft.VisualBasic.Strings.UCase(param);
            //return param.ToUpper();
        }

        /// <summary>
        /// VB OK. Retorna uma string contendo o caractere convertido para caixa alta
        /// </summary>
        private static char UCase(char param)
        {
            return Microsoft.VisualBasic.Strings.UCase(param);
            //return param.ToString().ToUpper()[0];
        }

        /// <summary>
        /// OK. Retorna um Boolean informando se a string está vazia ou é nula
        /// </summary>
        private static bool StrFilled(object param)
        {
            if (param == null)
                return false;
            else
                return !String.IsNullOrEmpty(param.ToString());
        }

        /// <summary>
        /// VB OK. Retorna um string com todas as ocorrências da string especificada 
        /// substituídas pelo valor da outra string especificada
        /// </summary>
        private static String Replace(String string1, String oldValue, String newValue)
        {
            return string1.Replace(oldValue, newValue);
        }

        /// <summary>
        /// VB OK. Retorna o valor string do objeto
        /// </summary>
        private static String str(object string1)
        {
            return string1.ToString();
        }

        /// <summary>
        /// VB OK. Retorna o índice da primeira ocorrência 
        /// da string na string especificada
        /// </summary>
        private static int InStr(string value, string searchedStr)
        {
            return value.IndexOf(searchedStr) + 1;
        }

        /// <summary>
        /// VB OK. Retorna um inteiro especificando a posição da 
        /// primeira ocorrência de uma string na outra
        /// </summary>
        private static int InStr(int start, string str1, string str2)
        {
            return Microsoft.VisualBasic.Strings.InStr(start, str1, str2, Microsoft.VisualBasic.CompareMethod.Text);
            //return str1.IndexOf(str2, start) + 1;
        }

        /// <summary>
        /// OK. Calcula expressões matemáticas retornando o 
        /// resultado do cálculo da expressão.
        /// </summary>
        [Obsolete("COMO FAZER?")]
        private static double Evaluate(String expression)
        {
            expression = expression.Replace("\t", "").Replace("\n", "").Replace("\r", "");

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = "SELECT " + expression;
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection);

                if (qt.Rows.Count > 0)
                {
                    if (qt.Rows[0][0].IsNull)
                        return double.NaN;

                    object ovalue = qt.Rows[0][0];                    
                    double value;
                    if (double.TryParse(ovalue.ToString(), out value))
                        return value;
                    else
                        return double.NaN;
                }
                else
                    return double.NaN;                
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion


        #region VB ControlChars
        private static char vbCr = '\r';    //Microsoft.VisualBasic.ControlChars.Cr;
        private static char vbLf = '\n';    //Microsoft.VisualBasic.ControlChars.Lf;
        private static char vbTab = '\t'; //Microsoft.VisualBasic.ControlChars.Tab;
        #endregion


        #region Enumerações
        public enum ESTADO
        {
            EST_INICIO,
            EST_ERRO,
            EST_ABPARENT,
            EST_FCPARENT,
            EST_ALFANUM,
            EST_OPER,
            EST_FIM
        }

        public enum tTipoCalculo
        {
            TP_ABPARENT,
            TP_FCPARENT,
            TP_VIRGULA,
            TP_NUMERO,
            TP_ALPHA,
            TP_FUNCAO,
            TP_OPERPRIOR,
            TP_OPER,
            TP_VAZIO
        }
        #endregion

        #endregion
    }

    public abstract class Calculadora
    {
        /// <summary>
        /// Realiza o cálculo de uma expressão infixa.
        /// <para>&#160;</para>
        /// > Números devem utilizar ponto como separador decimal<para><br/></para>        
        /// > Operadores:<para><br/></para>
        ///     - Soma "+"<para><br/></para>
        ///     - Subtração "-"<para><br/></para>
        ///     - Multiplicação "*"<para><br/></para>
        ///     - Divisão "/"<para><br/></para>
        /// > Parênteses para alteração de prioridade de operador<para><br/></para>
        /// > Funções:<para><br/></para>
        ///     - MAX(arg1, arg2)<para><br/></para>
        ///     - MIN(arg1, arg2)<para><br/></para>
        ///     - ROUND(arg1, casas_decimais)<para><br/></para>
        ///     - TRUNCATE(arg1, casas_decimais)<para><br/></para>
        ///     - ISNULL(objeto, retorno_se_nulo, retorno_se_nao_nulo)<para><br/></para>
        ///
        /// </summary>
        /// <param name="expressao"></param>
        /// <returns></returns>
        public static String CalcularExpressao(String expressao, ref String erro)
        {
            try
            {
                List<Token> tokens = ConverterPosFixa(AnalisarExpressao(expressao));
                if (tokens.Count(t => t.TipoToken == TipoToken.Numero) == 0)
                {
                    return String.Empty;
                }

                Stack<String> pilha = new Stack<String>();

                for (int i = 0; i < tokens.Count; i++)
                {
                    Token token = tokens.ElementAt(i);
                    if (token.TipoToken == TipoToken.Identificador ||
                        token.TipoToken == TipoToken.Numero)
                    {
                        pilha.Push(token.Valor);
                    }
                    else if (token.TipoToken == TipoToken.Operacao ||
                        token.TipoToken == TipoToken.OperacaoPriori ||
                        token.TipoToken == TipoToken.Funcao2)
                    {
                        string dir = pilha.Pop();
                        string esq = pilha.Pop();
                        pilha.Push(Calcular(esq, dir, token.Valor, null));
                    }
                    else if (token.TipoToken == TipoToken.Funcao3)
                    {
                        string oper3 = pilha.Pop();
                        string oper2 = pilha.Pop();
                        string oper1 = pilha.Pop();
                        pilha.Push(Calcular(oper1, oper2, token.Valor, oper3));
                    }
                }
                return pilha.Pop().ToString().Replace(",", ".");
            }
            catch (Exception e)
            {
                erro = e.Message;
                return null;
            }
        }

        /// <summary>
        /// Separa a expressão em tokens.
        /// </summary>
        /// <param name="expressao">Expressão numérica. Pode ser utilizado o marcador N para valores nulos.</param>
        /// <returns>Lista ordenada contendo os tokens da expressão.</returns>
        private static List<Token> AnalisarExpressao(String expressao)
        {
            expressao = " ( " + expressao + " ) ";
            expressao = expressao
                .Replace(",", " , ")
                .Replace("(", " ( ")
                .Replace(")", " ) ")
                .Replace("+", " + ")
                .Replace("-", " - ")
                .Replace("*", " * ")
                .Replace("/", " / ")
                .Replace("MAX", " MAX ", true)
                .Replace("MIN", " MIN ", true)
                .Replace("ROUND", " ROUND ", true)
                .Replace("TRUNCATE", " TRUNCATE ", true)
                .Replace("ISNULL", " ISNULL ", true);

            while (expressao.Contains("  "))
                expressao = expressao.Replace("  ", " ");

            List<Token> lista = new List<Token>();
            String[] tokens = expressao.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String token in tokens)
            {
                Token t = new Token();
                t.Valor = token;
                t.TipoToken = ObterTipoToken(token);
                if (t.TipoToken == TipoToken.Numero)
                    t.Valor = t.Valor.Replace(".", ",");
                lista.Add(t);
            }
            return lista;
        }

        /// <summary>
        /// Converte uma lista ordenada de tokens de uma expressão infixa,
        /// para uma lista ordenada de tokens de uma expressão pós-fixada (RPN).
        /// </summary>
        /// <param name="tokens">Lista ordenada de tokens da expressão infixa.</param>
        /// <returns>Lista ordenada de tokens da expressão pós-fixada (RPN)</returns>
        private static List<Token> ConverterPosFixa(List<Token> tokens)
        {
            List<Token> saida = new List<Token>();
            Stack<Token> pilha = new Stack<Token>();
            int prioridade = 0;

            //Varre todos os elementos da expressão de entrada e, para cada elemento, verifica 
            //se é operador ou operando. Se for operando, já adicona a saída.
            while (tokens.Count > 0)
            {
                Token token = tokens[0];
                tokens.RemoveAt(0);

                if (token.TipoToken == TipoToken.Numero || token.TipoToken == TipoToken.Identificador)
                    saida.Add(token);
                else if (token.TipoToken == TipoToken.Operacao || token.TipoToken == TipoToken.OperacaoPriori)
                {
                    prioridade = ObterPrioridade(token.Valor);
                    while ((pilha.Count != 0) && (ObterPrioridade(pilha.Peek().Valor) >= prioridade))
                        saida.Add(pilha.Pop());
                    pilha.Push(token);
                }
                else if (token.TipoToken == TipoToken.AbreParenteses)
                    pilha.Push(token);
                else if (token.TipoToken == TipoToken.FechaParenteses)
                {
                    Token item = pilha.Pop();
                    while (item.TipoToken != TipoToken.AbreParenteses)
                    {
                        saida.Add(item);
                        item = pilha.Pop();
                    }
                }
                else if (token.TipoToken == TipoToken.Funcao2 || token.TipoToken == TipoToken.Funcao3)
                {
                    List<List<Token>> args = new List<List<Token>>();
                    args.Add(new List<Token>());
                    args.Add(new List<Token>());
                    if (token.TipoToken == TipoToken.Funcao3)
                        args.Add(new List<Token>());

                    tokens.RemoveAt(0); //remove abre parenteses
                    tokens.RemoveAt(tokens.Count - 1); //remove o fecha parenteses

                    int abre_parenteses = 1;
                    int virgulas = 0;
                    int limite = token.TipoToken == TipoToken.Funcao2 ? 1 : 2;

                    while (tokens.Count > 0)
                    {
                        Token t = tokens[0];
                        tokens.RemoveAt(0);

                        if (t.TipoToken == TipoToken.AbreParenteses)
                        {
                            abre_parenteses++;
                            args[virgulas].Add(t);
                        }
                        else if (t.TipoToken == TipoToken.Virgula)
                        {
                            if (abre_parenteses == 1)
                                virgulas++;
                            else
                                args[virgulas].Add(t);
                        }
                        else if (t.TipoToken == TipoToken.FechaParenteses)
                        {
                            abre_parenteses--;
                            args[virgulas].Add(t);

                            if (abre_parenteses == 0)
                            {
                                args[virgulas].RemoveAt(args[virgulas].Count - 1);
                                tokens.Add(t);
                                break;
                            }
                        }
                        else
                            args[virgulas].Add(t);
                    }

                    for (int i = 0; i < args.Count; i++)
                        saida.AddRange(ConverterPosFixa(args[i]));
                    saida.Add(token);
                }
            }

            while (pilha.Count != 0)
                saida.Add(pilha.Pop());

            return saida;
        }

        /// <summary>
        /// Realiza o cálculo de uma operação.
        /// Números decimais devem utilizar a vírgula como separador decimal
        /// Ex: 
        ///     -   Calcula("1", "2", "+", null)          => 1+2
        ///     -   Calcula("1", "2", "*", null)          => 1*2
        ///     -   Calcula("N", "2", "ISNULL", "3")      => ISNULL(N,2,3)
        ///     -   Calcula("1", "2", "MAX", null)        => MAX(1,2)
        ///     -   Calcula("1,2345", "2", "ROUND", null) => ROUND(1.2345,2)
        ///     etc...
        /// </summary>
        /// <param name="operadorEsquerda">Operador esquerda (primeiro)</param>
        /// <param name="operadorDireita">Operador direita (segundo)</param>
        /// <param name="operacao">Operação</param>
        /// <param name="operadorExtra">Operador extra (terceiro)</param>
        /// <returns>Retorna o valor numérico em uma string (vírgula como separador decimal)</returns>
        private static String Calcular(String operadorEsquerda, String operadorDireita, String operacao, String operadorExtra)
        {
            decimal dValor = 0M;

            //Se a operação não for ISNULL, substitui o "N" por zero para a realização do cálculo
            if (operacao != "ISNULL")
                if (operadorEsquerda == "N") operadorEsquerda = "0";

            if (operadorDireita == "N") operadorDireita = "0";
            if (!String.IsNullOrEmpty(operadorExtra) && operadorExtra == "N") operadorExtra = "0";

            switch (operacao)
            {
                case "+":
                    dValor = decimal.Parse(operadorEsquerda) + decimal.Parse(operadorDireita);
                    break;
                case "-":
                    dValor = decimal.Parse(operadorEsquerda) - decimal.Parse(operadorDireita);
                    break;
                case "*":
                    dValor = decimal.Parse(operadorEsquerda) * decimal.Parse(operadorDireita);
                    break;
                case "/":
                    if (decimal.Parse(operadorDireita) == 0)
                        throw new Exception("Erro: Tentativa de divisão por zero.");
                    dValor = decimal.Parse(operadorEsquerda) / decimal.Parse(operadorDireita);
                    break;
                case "MAX":
                    dValor = decimal.Parse(operadorEsquerda) > decimal.Parse(operadorDireita) ?
                        decimal.Parse(operadorEsquerda) : decimal.Parse(operadorDireita);
                    break;
                case "MIN":
                    dValor = decimal.Parse(operadorEsquerda) < decimal.Parse(operadorDireita) ?
                        decimal.Parse(operadorEsquerda) : decimal.Parse(operadorDireita);
                    break;
                case "ROUND":
                    dValor = Math.Round(decimal.Parse(operadorEsquerda), (int)decimal.Parse(operadorDireita));
                    break;
                case "TRUNCATE":
                    string[] decparts = operadorEsquerda.Split(new char[] { ',', '.' });
                    if (decparts.Length == 0)
                        throw new Exception("Parâmetro inválido na função Truncate: " + operadorEsquerda);
                    dValor = decimal.Parse(decparts[0]);                    
                    if(decparts.Length == 2)
                        decparts[1] = "0," + decparts[1].Substring(0, int.Parse(operadorDireita));
                    dValor = dValor + (decparts.Length == 2 ? decimal.Parse(decparts[1]) : 0M);
                    break;
                case "ISNULL":
                    dValor = operadorEsquerda == "N" ? decimal.Parse(operadorDireita) : decimal.Parse(operadorExtra);
                    break;
            }
            return dValor.ToString().Replace(".", ",");
        }

        /// <summary>
        /// Obtém a prioridade do operador.
        /// </summary>
        /// <param name="operador">String contendo o operador.</param>
        /// <returns>Prioridade do operador. Quanto maior o valor retornado, maior a prioridade.</returns>
        private static int ObterPrioridade(String operador)
        {
            switch (operador)
            {
                case "(": return 1;
                case "+":
                case "-": return 2;
                case "/":
                case "*": return 3;
                case "MAX":
                case "MIN":
                case "ROUND":
                case "TRUNCATE":
                case "ISNULL": return 4;
                default: return 5; //não deve chegar neste caso
            }
        }

        /// <summary>
        /// Identifica o tipo de token.
        /// </summary>
        /// <param name="value">Valor a ser analisado</param>
        /// <returns>TipoToken identificado</returns>
        private static TipoToken ObterTipoToken(String value)
        {
            value = value.Trim();

            if (value.Length == 0)
                return TipoToken.Vazio;
            else
            {
                switch (value)
                {
                    case "(": return TipoToken.AbreParenteses;
                    case ")": return TipoToken.FechaParenteses;
                    case "+":
                    case "-": return TipoToken.Operacao;
                    case "*":
                    case "/": return TipoToken.OperacaoPriori;
                    case ",": return TipoToken.Virgula;
                    case "ROUND":
                    case "TRUNCATE":
                    case "MAX":
                    case "MIN": return TipoToken.Funcao2;
                    case "ISNULL": return TipoToken.Funcao3;
                    default:
                        {
                            decimal value_dec;
                            if (decimal.TryParse(value.Replace(".", ","), out value_dec))
                                return TipoToken.Numero;
                            else
                                return TipoToken.Identificador;
                        }
                }
            }
        }

        public enum TipoToken
        {
            AbreParenteses,
            FechaParenteses,
            Virgula,
            Numero,
            Funcao2,
            Funcao3,
            OperacaoPriori,
            Operacao,
            Identificador,
            Vazio
        }

        private class Token
        {
            public String Valor { get; set; }
            public TipoToken TipoToken { get; set; }
        }
    }

    public static class ExtendedMethods
    {
        public static String Replace(this String original, String oldValue, String newValue, Boolean caseInsensitive)
        {
            if (!caseInsensitive)
                return original.Replace(oldValue, newValue);
            else
            {
                String regex = "";
                foreach (char c in oldValue)
                {
                    string cStr = c.ToString();
                    regex += "[" + cStr.ToUpper() + cStr.ToLower() + "]";
                }
                return System.Text.RegularExpressions.Regex.Replace(original, "(" + regex + ")", newValue);
            }
        }

        public static Boolean IsFilled(this String value)
        {
            return !String.IsNullOrEmpty(value);
        }
    }
}
