using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
    public partial class Curriculos
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover (excluir) matrizes curriculares, seus anos de escolaridade e seus componentes curriculares.");

            help.Oper.Add("Para consultar, alterar ou remover (excluir) uma matriz curricular, seus anos de escolaridade e seus componentes curriculares é necessário “fazer uma busca por escolaridade e matriz curricular” iniciando pelo “Nível” e em cascata “Modalidade”, “Escolaridade”, “Turno” e “Matriz Curricular” obrigatoriamente em conjunto e nesta ordem clicando no botão ?I ao lado de sua respectiva grade de pesquisa. Todos são de seleção obrigatória.", "~/Images/bt_drop.png");
            help.Oper.Add("Ao clicar no botão ?I ao lado de cada linha da grade de pesquisa, os conteúdos existentes são apresentados em uma lista suspensa para a seleção do usuário.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição no filtro. Após definida a informação, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Após selecionar os conteúdos desejados deve-se selecionar a matriz curricular de interesse clicando na linha em que a matriz curricular aparece na lista suspensa.");
            help.Oper.Add("Obs.: Em quaisquer destas pesquisas, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de escolaridades para que só sejam exibidos registros contendo a palavra 'Ensino', digite %Ensino ou *Ensino na coluna 'Descrição'.");

            help.Oper.TitleAdd("Consultando uma matriz curricular");
            help.Oper.Add("A consulta é realizada automaticamente quando forem selecionados os filtros desejados na ordem em que se apresentam na grade de pesquisa.");
            help.Oper.Add("Os dados da matriz curricular selecionada serão exibidos e divididos em três abas:");
            help.Oper.Add("1 - Definição: Dados gerais da matriz curricular.");
            help.Oper.Add("2 - Ano de Escolaridade: Anos de escolaridade associados à matriz curricular.");
            help.Oper.Add("3 - Componentes curriculares: Componente curricular associadas à matriz curricular.");
            help.Oper.Add("Obs.: No modo de consulta de uma matriz curricular, é possível cadastrar, alterar ou remover (excluir) anos de escolaridade e componentes curriculares associados a ele.");

            help.Oper.TitleAdd("Cadastrando nova matriz curricular");
            help.Oper.Add("Para cadastrar uma nova matriz curricular deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Um formulário em branco é carregado para o preenchimento dos dados do novo registro na aba “Definição”. Na inclusão de uma nova matriz curricular as abas “Anos de Escolaridade” e “Componentes Curriculares” são desabilitadas.");
            help.Oper.Add("Deve-se preencher os campos com os dados da nova matriz curricular na aba “Definição”.");
            help.Oper.Add("Para salvar os dados da nova matriz curricular aba “Definições” deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da matriz curricular deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
            help.Oper.Add("Obs.: No modo de cadastro de uma matriz curricular, as abas “Ano de Escolaridade” e “Componente Curricular” ficam desativadas até que as informações da aba “Definições” sejam salvas com sucesso.");
            help.Oper.Add("Após salvar as informações da aba “Definições”, o sistema habilita as abas “Ano de Escolaridade” e “Componente Curricular” para preenchimento e salvamento dos dados na ordem que se segue.");


            help.Oper.TitleAdd("Alterando matriz curricular");
            help.Oper.Add("Para alterar uma matriz curricular é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para alterar os dados da matriz curricular selecionada deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
            help.Oper.Add("Um formulário é carregado com os dados da matriz curricular selecionada permitindo alteração nos campos.");
            help.Oper.Add("Para salvar os dados da matriz curricular deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a alteração nos dados da matriz curricular deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo (excluir) matriz curricular");
            help.Oper.Add("Para remover (excluir) uma matriz curricular, é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para remover (excluir) a matriz curricular selecionada deve-se clicar no botão ?I.", "~/Images/SmallDelete.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Cadastrando novo ano de escolaridade em uma matriz curricular");
            help.Oper.Add("Para cadastrar um novo ano de escolaridade é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para cadastrar um novo ano de escolaridade associado à matriz curricular selecionada a aba “Ano de Escolaridade” deve estar selecionada e deve-se clicar no botão ?I da grade de anos de escolaridade.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados do novo ano de escolaridade.");
            help.Oper.Add("Para salvar os dados do novo ano de escolaridade associado à matriz curricular selecionada deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão do novo ano de escolaridade na matriz curricular selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando ano de escolaridade de uma matriz curricular");
            help.Oper.Add("Para alterar um ano de escolaridade é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para alterar um ano de escolaridade associado à matriz curricular selecionada a aba “Ano de Escolaridade” deve estar selecionada e deve-se clicar no botão ?I da grade de anos de escolaridade.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados do ano de escolaridade serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados do ano de escolaridade deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados do ano de escolaridade deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo (excluindo) ano de escolaridade de uma matriz curricular");
            help.Oper.Add("Para remover (excluir) um ano de escolaridade é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para remover (excluir) um ano de escolaridade associado à matriz curricular selecionada a aba 'Ano de Escolaridade' deve estar selecionada e deve-se clicar no botão ?I da grade de anos de escolaridade e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Cadastrando nova componente curricular em uma matriz curricular");
            help.Oper.Add("Para cadastrar uma nova componente curricular é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para cadastrar uma nova componente curricular na matriz curricular selecionada a aba “Componente curricular” deve estar selecionada e deve-se clicar no botão ?I da grade de componente curricular.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados da nova componente curricular.");
            help.Oper.Add("Para salvar os dados da nova componente curricular na matriz curricular selecionada deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão da nova componente curricular na matriz curricular selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
            help.Oper.Add("Obs.: O modo de cadastro de componente curricular fica habilitado quando houver pelo menos um ano de escolaridade associado à matriz curricular em questão.");

            help.Oper.TitleAdd("Alterando componente curricular de uma matriz curricular");
            help.Oper.Add("Para alterar uma componente curricular é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para alterar uma componente curricular da matriz curricular selecionada a aba “Componente curricular” deve estar selecionada e deve-se clicar no botão ?I da grade de componente curricular.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados da componente curricular serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados da componente curricular deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados da componente curricular deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo (excluindo) componente curricular de uma matriz curricular");
            help.Oper.Add("Para remover (excluir) uma componente curricular, é necessário fazer a consulta da matriz curricular desejada. Ver “Consultando uma matriz curricular”.");
            help.Oper.Add("Para remover (excluir) uma componente curricular da matriz curricular selecionada a aba “Componente curricular” deve estar selecionada e deve-se clicar no botão ?I da grade de componente curricular e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Aba: Definição");
            help.Oper.Add("o Escolaridade*: Curso a que a matriz curricular se aplica.");
            help.Oper.Add("o Turno*: Turno a que a matriz curricular se aplica.");
            help.Oper.Add("o Especificação*: Especificação da matriz curricular.");
            help.Oper.Add("o Ano de Início*: Ano no qual terá início a validade da matriz curricular.");
            help.Oper.Add("o Período de Início*: Período no qual terá início a validade da matriz curricular.");
            help.Oper.Add("o Data de Publicação D.O.*: Data de publicação no Diário Oficial da matriz curricular.");
            help.Oper.Add("o Data Extinção: Data de extinção da matriz curricular.");
            help.Oper.Add("o Optativas:");
            help.Oper.Add("• Ensino Religioso  ");
            help.Oper.Add("•  Língua Estrangeira Facultativa");

            help.Oper.TitleAdd("•	Aba: Anos de Escolaridade");
            help.Oper.Add("o Ano/Série*: Identificação do ano/série em curso.");
            help.Oper.Add("o Descrição*: Descrição detalhada do ano de escolaridade.");
            help.Oper.Add("o Prefixo das Turmas: Prefixo de identificação das turmas.");
            help.Oper.Add("o Tempos de Aula: Tempos de aula referentes ao ano de escolaridade.");
            help.Oper.Add("o Idade Mínima de Ingresso: Idade mínima que um aluno deve possuir para ser alocado no ano de escolaridade em questão.");
            help.Oper.Add("o Dia Limite de Aniversário: Dia limite de aniversário (define os limites da idade mínima).");
            help.Oper.Add("o Mês Limite de Aniversário: Mês limite de aniversário (define os limites da idade mínima).");
            help.Oper.Add("o Desativado em: Indica data de desativação do ano de escolaridade.");
            help.Oper.Add("o Curso Seguinte*: O curso que vem após a conclusão.");
            help.Oper.Add("o Série Seguinte*: A série que vem após a série atual.");
            help.Oper.Add("o Ano/Série Concluinte??: Se é ou não o ano de conclusão do curso.");
            help.Oper.Add("o Emite Certificação??: Se o curso emite ou não Certificação.");
            help.Oper.Add("o Oferece Eletiva??: Se o curso oferece ou não eletiva.");

            help.Oper.TitleAdd("• Aba: Componentes Curriculares");
            help.Oper.Add("o Ano de Escolaridade: Ano de escolaridade ideal no qual a componente curricular deverá ser cursada.");
            help.Oper.Add("o Código: Código da componente curricular.");
            help.Oper.Add("o Componente Curricular: Nome descritivo da componente curricular.");
            help.Oper.Add("o Carga Horária: Carga horária total da componente curricular. (Apenas consulta)");
            help.Oper.Add("o Componente: Componente referente à componente curricular.");
            help.Oper.Add("o Macrocampo:");
            help.Oper.Add("o Área de Conhecimento: Área de conhecimento referente à componente curricular.");
            help.Oper.Add("o Obrigatória: Indica se a componente curricular é obrigatória no período. Quando estiver marcada como obrigatória, a componente curricular será apresentada automaticamente no momento em que o aluno efetuar a matrícula.");
            help.Oper.Add("o Permite GLP: Indica se a componente curricular permite alocação de docentes em GLP. Caso esta opção não esteja marcada, a página de turmas bloqueará a alocação de GLP para a componente curricular em questão.");

            help.Oper.TitleAdd("Botões");
            help.Oper.TitleAdd("Definição");
            help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
            help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
            help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
            help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");

            help.Oper.TitleAdd("Ano de Escolaridade/Disciplinas");
            help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
            help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
            help.Oper.Add("?I: Remove (excluir) a linha.", "~/img/bt_exclui2.png");
            help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");

            help.Oper.TitleAdd("Componente curricular");
            help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
            help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
            help.Oper.Add("?I: Remove (excluir) a linha.", "~/img/bt_exclui2.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
