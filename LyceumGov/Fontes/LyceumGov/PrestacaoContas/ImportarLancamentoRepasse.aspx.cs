using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Web;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Globalization;
using System.Linq;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
    NavUrl("~/PrestacaoContas/ImportarLancamentoRepasse.aspx"),
    ControlText("ImportarLancamentoRepasse"),
    Title("Importar Lançamento Repasse"),
    ]
    public partial class ImportarLancamentoRepasse : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        private readonly RN.PrestacaoContas.LancamentoRepasse rnLancamentoRepasse = new Techne.Lyceum.RN.PrestacaoContas.LancamentoRepasse();
        private readonly RN.PrestacaoContas.ContaCorrente rnContaCorrente = new Techne.Lyceum.RN.PrestacaoContas.ContaCorrente();
        private readonly RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ItemPlanilhaOrcamentaria();
        private readonly RN.UnidadeEnsino rnUnidadeEnsino = new RN.UnidadeEnsino();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePO_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tsePO.DBValue.IsNull)
                {
                    if (!tsePO.IsValidDBValue)
                    {

                        lblMensagem.Text = "Programação Orçamentária não ativa ou não cadastrada (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Programação Orçamentária não ativa ou não cadastrada (favor verificar).";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePPO_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tsePPO.DBValue.IsNull)
                {
                    if (!tsePPO.IsValidDBValue)
                    {

                        lblMensagem.Text = "Parcela da Programação Orçamentária não ativa ou não cadastrada (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Parcela da Programação Orçamentária não ativa ou não cadastrada (favor verificar).";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                //verificar se o filtro está todo preenchido
                if ((tsePO.DBValue.IsNull || !tsePO.IsValidDBValue) || (tsePPO.DBValue.IsNull || !tsePPO.IsValidDBValue))
                {
                    lblMensagem.Text = "Para efetuar a importação é necessário selecionar uma Programação Orçamentária e sua Parcela.";
                    return;
                }

                //obter o nome do arquivo
                string fileName = System.IO.Path.GetFileName(arquivo.PostedFile.FileName);
                if (fileName == "")
                {
                    lblMensagem.Text = "Arquivo não Selecionado!!!";
                    return;
                }

                //verificar a extensão do arquivo. eh obrigatório ser CSV
                if (!fileName.EndsWith(".csv"))
                {
                    lblMensagem.Text = "Arquivo tem que estar no formato CSV!!!";
                    return;
                }

                //salvar o arquivo com um nome aleatório, para não termos problemas de 2 usuários diferentes estarem submetendo o mesmo arquivo ao mesmo tempo
                var aleatorio = Guid.NewGuid();
                var arquivoServer = "C:/Logs/Gestao/PrestacaoContas/" + aleatorio + "-" + fileName;
                arquivo.PostedFile.SaveAs(arquivoServer);

                //Carregar CSV
                var linhasDaPlanilha = CarregarCSV(new FileInfo(arquivoServer), Encoding.UTF7);
                if (!linhasDaPlanilha.Any())
                {
                    lblMensagem.Text = "Não foram encontradas linhas no formato correto para importação.";
                    return;
                }

                //validação da planilha
                var itemPlanilhaOrcamentariaId = Convert.ToInt32(tsePPO.Value);
                var valida = ValidaPlanilha(linhasDaPlanilha, itemPlanilhaOrcamentariaId);

                //se a planilha estiver válida...
                if (valida.Valido)
                {
                    //obter e concatenar as validações das linhas
                    var validacoesDasLinhas = string.Empty;
                    if (linhasDaPlanilha.Any(q => q.MensagemValidacao != null)) 
                    {
                        validacoesDasLinhas = linhasDaPlanilha
                            .Where(q => q.MensagemValidacao != null)
                            .Select(s => s.MensagemValidacao)
                            .Aggregate((c, n) => c + Environment.NewLine + n);
                    }

                    //converter a linha da planilha na entidade LancamentoRepasse
                    var lancamentosRepasseValidos = linhasDaPlanilha.Where(q => q.MensagemValidacao == null).Select(s =>
                        new RN.PrestacaoContas.Entidades.LancamentoRepasse 
                        {
                            ItemPlanilhaOrcamentariaId = itemPlanilhaOrcamentariaId,
                            Censo = s.Codigo,
                            ContaCorrenteId = s.ContaCorrenteId.Value,
                            Valor = s.Valor.Value,
                            UsuarioId = User.Identity.Name,
                            DataCadastro = DateTime.Now,
                            DataAlteracao = DateTime.Now,
                        }
                    ).ToList();

                    //variáveis contadoras
                    var valorTotalImportado = (decimal)0;
                    var totalItensImportados = 0;

                    //fazer a importação do arquivo
                    rnLancamentoRepasse.ImportaArquivo(lancamentosRepasseValidos, out valorTotalImportado, out totalItensImportados);
                    
                    //mostrar a mensagem na tela
                    lblMensagem.Text = "Importação Finalizada!!!<br/><br/>Total de Linhas no arquivo da Programação Orçamentária:" + linhasDaPlanilha.Count() + "<br/>Total de Linhas no Arquivo da Programação Orçamentária importados: " + totalItensImportados + "<br/>Total de Erros: " + (linhasDaPlanilha.Count() - totalItensImportados);
                    
                    if (validacoesDasLinhas.Any())
                        lblMensagem.Text += "<br/><br/>" + validacoesDasLinhas.Replace(Environment.NewLine, "<br />");
                }
                else
                {
                    lblMensagem.Text = valida.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private class LinhaCSV
        {
            public int Row { get; set; }
            public int? Item { get; set; }
            public string Codigo { get; set; }
            public string Escola { get; set; }
            public string Cnpj { get; set; }
            public int? ContaCorrenteId { get; set; }
            public string Banco { get; set; }
            public string Agencia { get; set; }
            public string Conta { get; set; }
            public decimal? Valor { get; set; }
            public string MensagemValidacao { get; set; }
        }

        private IList<LinhaCSV> CarregarCSV(StreamReader sr, Encoding encoding)
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var posicao = 0;
            var posicaoCabecalho = 0;

            var linhas = new List<LinhaCSV>();

            try
            {
                //percorrer todas as linhas do stream
                var linhaDoStream = (string)null;
                while (!sr.EndOfStream)
                {
                    //posição a ser processada
                    posicao++;

                    //ler a linha do stream
                    linhaDoStream = sr.ReadLine();

                    //obter as colunas da linha
                    string[] colunas = Regex.Split(linhaDoStream, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                    //se a linha atender as especificações do IF abaixo, então é um cabeçalho.
                    if (
                        colunas.Length >= 6 &&
                        compareInfo.IndexOf(linhaDoStream, "ITEM", CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1 &&
                        compareInfo.IndexOf(linhaDoStream, "CODIGO", CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1 &&
                        compareInfo.IndexOf(linhaDoStream, "CNPJ", CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1 &&
                        compareInfo.IndexOf(linhaDoStream, "CONTA CORRENTE", CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1 &&
                        compareInfo.IndexOf(linhaDoStream, "VALOR", CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1
                    )
                    {
                        if (posicaoCabecalho == 0)
                            posicaoCabecalho = posicao;

                        continue;
                    }

                    //verificar se é uma linha a ser descartada por não ter censo preenchido
                    if (colunas[1].Trim() == "")
                        continue;

                    //já não precisa mais descartar linhas, então, adicionar as linhas a partir daqui
                    var linha = new LinhaCSV();

                    linha.Row = posicao;

                    try { linha.Item = Convert.ToInt32(colunas[0]); }
                    catch { linha.Item = null; };

                    linha.Codigo = Convert.ToString(colunas[1]);
                    linha.Escola = Convert.ToString(colunas[2]);
                    linha.Cnpj = Convert.ToString(colunas[3]);
                    linha.ContaCorrenteId = null;
                    linha.Banco = "237";

                    try
                    {
                        var agencia_e_conta = colunas[4].Split('-');
                        agencia_e_conta = agencia_e_conta.Select(s => s.Trim()).ToArray();
                        linha.Agencia = agencia_e_conta[0];
                        linha.Conta = agencia_e_conta[1];
                    }
                    catch
                    {
                        linha.Agencia = null;
                        linha.Conta = null;
                    }

                    try 
                    {
                        var valor = colunas[5];
                        valor = valor.Where(q => char.IsDigit(q) || q == '.' || q == ',').Select(s => s.ToString()).Aggregate((c, n) => c + n);
                        linha.Valor = Convert.ToDecimal(valor, CultureInfo.GetCultureInfo("pt-BR")); 
                    }
                    catch { linha.Valor = null; }

                    linha.MensagemValidacao = null;

                    linhas.Add(linha);
                }

                return linhas;
            }
            catch (Exception ex)
            {
                throw new Exception("Arquivo CSV incompatível ou mal-formado.");
            }
        }

        private IList<LinhaCSV> CarregarCSV(FileInfo file, Encoding encoding)
        {
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(file.FullName, encoding);
                return CarregarCSV(sr, encoding);
            }
            catch (IOException ioex)
            {
                throw ioex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
        }

        private ValidacaoDados ValidaPlanilha(IList<LinhaCSV> linhasDaPlanilha, int itemPlanilhaOrcamentariaId)
        {
            //registro de mensagens
            var mensagens = new List<string>();

            //lista de censos da planilha
            var codigos = linhasDaPlanilha
                .Where(q => q.Codigo != null)
                .Select(s => s.Codigo)
                .Distinct().ToList();

            //obtenção da lista de censos existentes
            var unidadesEnsinoExistentes = rnUnidadeEnsino.RetornaApenasAsUnidadesExistentes(codigos);

            //obtenção dos lançamentos de repasse existentes
            var lancamentosRepasseExistentes = rnLancamentoRepasse.RetornaApenasAsUnidadesExistentes(itemPlanilhaOrcamentariaId);

            //obtenção da lista de contas correntes existentes
            var contasCorrentesExistentes = rnContaCorrente.RetornaApenasAsContasCorrentesExistentes(codigos);

            //Obter o valor da parcela da planilha orçamentária
            var valorItemPlanilhaOrcamentaria = rnItemPlanilhaOrcamentaria.ObtemValorPor(itemPlanilhaOrcamentariaId);
            
            //Verificar se foi possível obter os valores de todas as linhas da planilha, e aproveitar pra fazer:
            //1º: verificar se o censo está preenchido com 8 caracteres numéricos
            //2º: verificar se o censo existe
            //3º: verificar se o censo já existe para o item da planilha orçamentária especificado
            //3º: verificar se agência e conta existem cadastrados para esta unidade
            //4º: verificar se o CNPJ é válido conforme o dígito verificador
            //5º: verificar se o CNPJ pertence a esta unidade
            //Por fim: somatório dos valores, para verificar se a soma ultrapassa o valor da parcela de programação orçamentária

            var codigosValidados = new List<string>();
            var somaValores = (decimal)0;
            foreach (var linha in linhasDaPlanilha)
            {
                //verificar se o censo está preenchido com 8 caracteres numéricos
                if (linha.Codigo.Length != 8 || !linha.Codigo.All(q => char.IsDigit(q)))
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": O Código da Escola não está no formato esperado - 8 algarismos";
                    continue;
                }

                //verificar se o censo existe
                var censoExiste = unidadesEnsinoExistentes.Any(q => q.UnidadeEns == linha.Codigo);
                if (!censoExiste)
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": O Código da Escola não existe no sistema";
                    continue;
                }

                //verificar se o censo já existe para o item da planilha orçamentária especificado
                var censoJaLancado = lancamentosRepasseExistentes.Any(q => q.Censo == linha.Codigo);
                if (censoJaLancado)
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": O Código da Escola informado já havia sido lançado anteriormente";
                    continue;
                }

                //verificar se agência e conta existem cadastrados para esta unidade
                var contaCorrente = contasCorrentesExistentes.FirstOrDefault(q => 
                    q.Censo == linha.Codigo &&
                    q.Banco == linha.Banco &&
                    q.Agencia == linha.Agencia &&
                    (q.Conta == linha.Conta || Convert.ToInt32(q.Conta) == Convert.ToInt32(linha.Conta) )
                ); 
                if (contaCorrente == null)
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": Agência e Conta Corrente não existem cadastrados para esta unidade";
                    continue;
                }
                else
                {
                    linha.ContaCorrenteId = contaCorrente.ContaCorrenteId;
                }

                //verificar se o CNPJ é válido conforme o dígito verificador
                if (!Techne.Lyceum.RN.Validacao.ValidaCnpj(linha.Cnpj))
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": CNPJ inválido";
                    continue;
                }

                //verificar se o CNPJ pertence a esta unidade
                var cnpjPertencente = unidadesEnsinoExistentes.Any(q => q.UnidadeEns == linha.Codigo && (q.Cgc.RetirarMascaraCNPJ() == linha.Cnpj.RetirarMascaraCNPJ() || (Convert.ToInt64(q.Cgc.RetirarMascaraCNPJ()) == Convert.ToInt64(linha.Cnpj.RetirarMascaraCNPJ()))));
                if (!cnpjPertencente)
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": CNPJ não pertence a esta unidade";
                    continue;
                }

                //unidade validada
                codigosValidados.Add(linha.Codigo);

                //somar o valor da linha e verificar se já ultrapassou
                somaValores += (linha.Valor ?? 0);
                var ultrapassou = somaValores > valorItemPlanilhaOrcamentaria;
                if (ultrapassou)
                {
                    linha.MensagemValidacao = "Linha " + linha.Row + ": A partir desta linha a soma dos valores ultrapassa o valor da parcela da planilha orçamentária.";
                    break;
                }
            }

            //Obter a soma dos valores dos lançamentos de repasse que já estão no banco de dados
            var jaLancadosAnteriormente = rnLancamentoRepasse.ListaLancamentoRepassePor(itemPlanilhaOrcamentariaId, null);
            var somaJaLancados = jaLancadosAnteriormente.Sum(s => s.Valor);

            //Verificar se a soma dos valores da planilha + soma dos valores dos itens já lançados no BD ultrapassa o valor da parcela da planilha orçamentária
            var somaValoresTotal = somaValores + somaJaLancados;
            if (somaValoresTotal > valorItemPlanilhaOrcamentaria)
            {
                mensagens.Add("A soma dos valores da planilha mais a soma do que já foi importado ultrapassou o valor da parcela da planilha orçamentária.");
            }

            //retornar erro ou sucesso
            return new ValidacaoDados
            {
                Valido = !mensagens.Any(),
                Mensagem = mensagens.Any() ? mensagens.Aggregate((x, y) => x + Environment.NewLine + y) + Environment.NewLine + "Nenhuma linha foi importada." : null,
            };
        }
    }
}