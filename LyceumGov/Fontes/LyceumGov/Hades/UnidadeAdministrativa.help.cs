using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
    public partial class Setores
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover (excluir) unidades administrativas.");

            help.Oper.Add("Para consultar, alterar ou remover (excluir) uma Unidade Administrativa é necessário fazer uma pesquisa pela Unidade Administrativa de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
            help.Oper.Add("Todas as unidades administrativas existentes são apresentadas em uma lista suspensa.");
            help.Oper.Add("A pesquisa pode ser filtrada pelo Código, U.A., Nome ou U.A. Antiga. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar a Unidade Administrativa de interesse clicando na linha em que a Unidade Administrativa aparece na lista suspensa.");
            help.Oper.Add("Obs.: Nesta pesquisa, utilize o caractere '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna “Descrição” para que só sejam exibidos registros contendo a palavra “História”, digite %história na coluna “Descrição”.");

            help.Oper.TitleAdd("Consultando uma unidade administrativa");
            help.Oper.Add("A consulta é realizada automaticamente quando a Unidade Administrativa de interesse for selecionada.");
            help.Oper.Add("Os dados da Unidade Administrativa selecionada serão destacados na página.");

            help.Oper.TitleAdd("Cadastrando nova unidade administrativa");
            help.Oper.Add("Para cadastrar uma nova Unidade Administrativa deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Será carregado um formulário em branco para preenchimento dos dados. Ver “Descrição dos Campos”.");
            help.Oper.Add("Deve-se preencher os campos com os dados da nova Unidade Administrativa.");
            help.Oper.Add("Para salvar os dados da nova Unidade Administrativa deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da Unidade Administrativa deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando unidade administrativa");
            help.Oper.Add("Para alterar uma Unidade Administrativa é necessário fazer a consulta da Unidade Administrativa desejada. Ver “Consultando uma Unidade Administrativa”.");
            help.Oper.Add("Para alterar os dados da Unidade Administrativa selecionada deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
            help.Oper.Add("Os dados da Unidade Administrativa serão carregados permitindo alterações nos campos. Ver “Descrição dos Campos”.");
            help.Oper.Add("Para salvar os dados da Unidade Administrativa deve-se clicar no botão  ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a alteração nos dados da Unidade Administrativa deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo unidade administrativa");
            help.Oper.Add("Para remover (excluir) uma Unidade Administrativa é necessário fazer a consulta da Unidade Administrativa desejada. Ver “Consultando uma Unidade Administrativa”.");
            help.Oper.Add("Para remover (excluir) a Unidade Administrativa selecionada deve-se clicar no botão ?I.", "~/Images/SmallDelete.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Aba: Geral (Consulta, Altera e Cadastro)");
            help.Oper.Add("• Dados da Unidade Administrativa");
            help.Oper.Add("o Código: Código da Unidade Administrativa.");
            help.Oper.Add("o Unidade Administrativa: Código da Unidade Administrativa.");
            help.Oper.Add("o U.A. Antiga: Código da Unidade Administrativa.");
            help.Oper.Add("o Nome: Nome descritivo da Unidade Administrativa.");
            help.Oper.Add("o Tipo: Tipo da Unidade Administrativa.");
            help.Oper.Add("o CNPJ: CNPJ da Unidade Administrativa.");
            help.Oper.Add("o Data início: Data de criação da Unidade Administrativa.");
            help.Oper.Add("o Data Fim: Data de extinção da Unidade Administrativa.");
            help.Oper.Add("o CEP: CEP da Unidade Administrativa.");
            help.Oper.Add("o Município: Município da Unidade Administrativa.");
            help.Oper.Add("o Estado: Estado da Unidade Administrativa..");
            help.Oper.Add("o Endereço: Endereço físico da Unidade Administrativa.");
            help.Oper.Add("o Nº: Número referente ao endereço.");
            help.Oper.Add("o Compl.: Complemento do endereço.");
            help.Oper.Add("o Bairro: Bairro da Unidade Administrativa.");
            help.Oper.Add("o Telefone: Número de telefone de contato da Unidade Administrativa.");
            help.Oper.Add("o Fax: Número de fax da Unidade Administrativa.");
            help.Oper.Add("o Ativo: Status da Unidade Administrativa.");
            help.Oper.Add("o Observação: Observação referente a Unidade Administrativa.");

            help.Oper.TitleAdd("• Aba: Contatos (Consulta)");
            help.Oper.Add("o Nome: Nome do contato do setor.");
            help.Oper.Add("o Matrícula: Matrícula do contato do setor.");
            help.Oper.Add("o Função: Função do contato do setor.");
            help.Oper.Add("o Telefone: Telefone do contato do setor.");
            help.Oper.Add("o Celular: Celular do contato do setor.");
            help.Oper.Add("o E-mail: E-mail do contato do setor.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?T: Carrega um formulário para novo registro.", btnNovo);
            help.Oper.Add("?T: Permite alteração no registro.", btnEditar);
            help.Oper.Add("?T: Remove o registro.", btnExcluir);
            help.Oper.Add("?T: Salva as alterações do registro.", btnSalvar);
            help.Oper.Add("?T: Cancela a operação corrente e retorna para página inicial.", btnCancel);
            help.Oper.Add("?T: Confirma a operação.", btnConfirmar);
            help.Oper.Add("A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
