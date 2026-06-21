using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class LotacaoDocente
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover lotações e situações de docentes.");
            help.Oper.Add("Para consultar, cadastrar, alterar ou remover uma lotação/situação é necessário fazer uma pesquisa pelo docente de interesse.");
            help.Oper.Add("A pesquisa pode ser feita pela nome, matrícula, documento e/ou CPF do docente. Após definidas estas informações, deve-se clicar em 'Buscar'. O resultado da pesquisa aparecerá em uma lista suspensa.");
            help.Oper.Add("Deve-se selecionar o docente de interesse clicando na linha em que o docente aparece na lista suspensa.");

            help.Oper.TitleAdd("Consultando lotações/situações de um docente");
            help.Oper.Add("A consulta é realizada automaticamente quando o docente de interesse for selecionado");
            help.Oper.Add("Os dados das lotações/situações associadas ao docente selecionado serão exibidos na grade de lotações e na grade de situações.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando nova lotação/situação para um docente");
            help.Oper.Add("Para cadastrar uma nova lotação/situação é necessário fazer a consulta do docente desejado. Ver 'Consultando lotações de um docente'.");
            help.Oper.Add("Para cadastrar uma nova lotação/situação associada ao docente selecionado deve-se clicar no botão ?I da respectiva grade.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados da nova lotação/situação.");
            help.Oper.Add("Para salvar os dados da nova lotação/situação deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da nova lotação/situação deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a inclusão.");
            help.Oper.Add("Obs.: Também é possível incluir situação através da grade de lotações, tanto no modo de inclusão como no modo de alteração da lotação.");

            help.Oper.TitleAdd("Alterando lotação/situação de um docente");
            help.Oper.Add("Para alterar uma lotação/situação é necessário fazer a consulta do docente desejado. Ver 'Consultando lotações de um docente'.");
            help.Oper.Add("Para alterar os dados de uma lotação/situação associada ao docente selecionado deve-se clicar no botão ?I.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados da lotação/situação serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados da lotação/situação deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados da lotação/situação deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo lotação/situação de um docente");
            help.Oper.Add("Para remover uma lotação/situação é necessário fazer a consulta do docente desejado. Ver 'Consultando lotações de um docente'.");
            help.Oper.Add("Para remover uma lotação/situação associada ao docente selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Grade Lotações");
            help.Oper.Add("• Matrícula: Matrícula do docente. (Tabela: docentes/Funcionários)");
            help.Oper.Add("• Função: Função exercida pela lotação.");
            help.Oper.Add("• U.A.: Unidade administrativa referente à lotação.");
            help.Oper.Add("• Coordenaadoria: Coordenadoria referente à lotação.");
            help.Oper.Add("• Unidade Ensino: Unidade de ensino referente à lotação.");
            help.Oper.Add("• Data da Nomeação: Data da nomeação do docente.");
            help.Oper.Add("• Data da Publicação da Nomeação: Data da publicação da nomeação do docente.");
            help.Oper.Add("• Data da Dispensa: Data da dispensa do docente.");
            help.Oper.Add("• Documentação: Indica se o docente é responsável por documentação.");
            help.Oper.Add("• Data da Publicação da Dispensa: Data da publicação da dispensa do docente.");
            help.Oper.Add("• Ato oficial: Ato oficial da nomeação do docente.");
            help.Oper.Add("• Situação: Motivo da licença. (Tabela: Ly_licenca_docente)");
            help.Oper.Add("• Data Início Situação: Data de innício da licença.");
            help.Oper.Add("• Data Fim Situação: Data de fim da licença.");

            help.Oper.TitleAdd("Grade Situações");
            help.Oper.Add("• Matrícula: Matrícula do docente.");
            help.Oper.Add("• Situação: Motivo da licença. (Tabela: Ly_licenca_docente)");
            help.Oper.Add("• Data Início Situação: Data de innício da licença.");
            help.Oper.Add("• Data Fim Situação: Data de fim da licença.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
            help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
            help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
            help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}

