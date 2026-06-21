using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
	public partial class ConcursoDocentes
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover processos seletivos, suas habilitações, suas experiências e suas titulações.");
            help.Oper.Add("Para consultar, alterar ou remover um processo seletivo, suas habilitações, suas experiências e suas titulações é necessário fazer uma pesquisa pelo processo seletivo de interesse clicando no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("Ao clicar no botão ?I, todos os processos seletivos existentes são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição do processo seletivo. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
			help.Oper.Add("Deve-se selecionar o processo seletivo de interesse clicando na linha em que o processo seletivo aparece na lista suspensa.");
			help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
			help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de processos seletivos para que só sejam exibidos registros contendo a palavra 'História', digite %História na coluna 'Descrição'.");

			help.Oper.TitleAdd("Consultando processo seletivo");
			help.Oper.Add("A consulta é realizada automaticamente quando o processo seletivo de interesse for selecionado.");
			help.Oper.Add("Os dados do processo seletivo selecionado serão exibidos e divididos em quatro abas:");
			help.Oper.Add("1 - Dados do Processo Seletivo: Dados gerais do processo seletivo.");
			help.Oper.Add("2 - Habilitações: Habilitações associadas ao processo seletivo.");
			help.Oper.Add("3 - Experiências: Experiências associadas ao processo seletivo.");
			help.Oper.Add("4 - Titulações: Titulações associadas ao processo seletivo.");
			help.Oper.Add("Obs.: No modo de consulta de um processo seletivo, é possível cadastrar, alterar ou remover habilitações, experiências e titulações associadas a ele.");

			help.Oper.TitleAdd("Cadastrando novo processo seletivo");
			help.Oper.Add("Para cadastrar um processo seletivo deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
			help.Oper.Add("Um formulário em branco é carregado para o preenchimento dos dados do novo registro.");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo processo seletivo na aba 'Dados do Processo Seletivo'.");
			help.Oper.Add("Para salvar os dados do novo processo seletivo deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a inclusão dos dados do processo seletivo deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
			help.Oper.Add("Obs.: No modo de cadastro de um processo seletivo, as abas 'Habilitações', 'Experiências' e 'Titulações' ficam desativadas.");

			help.Oper.TitleAdd("Alterando processo seletivo");
			help.Oper.Add("Para alterar um processo seletivo é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para alterar os dados do processo seletivo selecionado deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
			help.Oper.Add("Um formulário é carregado na aba 'Dados do Processo Seletivo' com os dados do processo seletivo selecionado permitindo alteração nos campos.");
			help.Oper.Add("Para salvar os dados do processo seletivo deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
			help.Oper.Add("Para cancelar a alteração nos dados do processo seletivo deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo processo seletivo");
			help.Oper.Add("Para remover um processo seletivo é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para remover o processo seletivo selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/Images/SmallDelete.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando nova habilitação em um processo seletivo");
			help.Oper.Add("Para cadastrar uma nova habilitação é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para cadastrar uma nova habilitação associada ao processo seletivo selecionado a aba 'Habilitações' deve estar selecionada e deve-se clicar no botão ?I da grade de habilitações.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova habilitação.");
			help.Oper.Add("Para salvar os dados da nova habilitação associada ao processo seletivo selecionado deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão da nova habilitação no processo seletivo selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando habilitação de um processo seletivo");
			help.Oper.Add("Para alterar uma habilitação é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para alterar uma habilitação associada ao processo seletivo selecionado a aba 'Habilitações' deve estar selecionada e deve-se clicar no botão ?I da grade de habilitações.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da habilitação serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da habilitação deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da habilitação deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo habilitação de um processo seletivo");
			help.Oper.Add("Para remover uma habilitação é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para remover uma habilitação associada ao processo seletivo selecionado a aba 'Habilitações' deve estar selecionada e deve-se clicar no botão ?I da grade de habilitações e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando nova experiência em um processo seletivo");
			help.Oper.Add("Para cadastrar uma nova experiência é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para cadastrar uma nova experiência no processo seletivo selecionado a aba 'Experiências' deve estar selecionada e deve-se clicar no botão ?I da grade de experiências.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova experiência.");
			help.Oper.Add("Para salvar os dados da nova experiência no processo seletivo selecionado deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão da nova experiência no processo seletivo selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando experiência de um processo seletivo");
			help.Oper.Add("Para alterar uma experiência é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para alterar uma experiência do processo seletivo selecionado a aba 'Experiências' deve estar selecionada e deve-se clicar no botão ?I da grade de experiências.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da experiência serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da experiência deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da experiência deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo experiência de um processo seletivo");
			help.Oper.Add("Para remover uma experiência é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para remover uma experiência do processo seletivo selecionado a aba 'Experiências' deve estar selecionada e deve-se clicar no botão ?I da grade de experiências e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Cadastrando nova titulação em um processo seletivo");
			help.Oper.Add("Para cadastrar uma nova titulação é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para cadastrar uma nova titulação no processo seletivo selecionado a aba 'Titulações' deve estar selecionada e deve-se clicar no botão ?I da grade de titulações.", "~/img/bt_novo.png");
			help.Oper.Add("Deve-se preencher os campos com os dados da nova titulação.");
			help.Oper.Add("Para salvar os dados da nova titulação no processo seletivo selecionado deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a inclusão da nova titulação no processo seletivo selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando titulação de um processo seletivo");
			help.Oper.Add("Para alterar uma titulação é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para alterar uma titulação do processo seletivo selecionado a aba 'Titulações' deve estar selecionada e deve-se clicar no botão ?I da grade de titulações.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados da titulação serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados da titulação deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados da titulação deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo titulação de um processo seletivo");
			help.Oper.Add("Para remover uma titulação é necessário fazer a consulta do processo seletivo desejado. Ver 'Consultando processo seletivo'.");
			help.Oper.Add("Para remover uma titulação do processo seletivo selecionado a aba 'Titulações' deve estar selecionada e deve-se clicar no botão ?I da grade de titulações e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Aba: Dados do processo seletivo");
			help.Oper.Add("• Processo Seletivo: Código de identificação do processo seletivo.");
			help.Oper.Add("• Descrição: Descrição do processo seletivo.");
			help.Oper.Add("• Ano: Ano do processo seletivo. (Tabela: Ano Letivo)");
			help.Oper.Add("• Período: Período do processo seletivo. (Tabela: Ano Letivo, cujo filtro é Ano)");
            help.Oper.Add("• Status: Status do processo seletivo. (Tabela Geral: StatusConcursoDoc)");
			help.Oper.Add("• Data Início: Data de início do processo seletivo.");
			help.Oper.Add("• Data Fim: Data de fim do processo seletivo.");
			help.Oper.Add("• Inscrição Início: Data de início da inscrição pela Internet do processo seletivo.");
			help.Oper.Add("• Inscrição Fim: Data de fim da inscrição pela Internet do processo seletivo.");
			help.Oper.Add("• Liberação Consulta Início*: Data de início da liberação de consulta à situação do candidato .");
			help.Oper.Add("• Liberação Consulta Fim*: Data de fim da liberação de consulta à situação do candidato.");
			help.Oper.Add("• Convocação Início*: Data de início da convocação.");
			help.Oper.Add("• Convocação Fim: Data de fim da convocação.");
			help.Oper.Add("• Ingresso Início: Data de início de ingresso do docente.");
			help.Oper.Add("• Ingresso Fim: Data de fim de ingresso do docente.");
			help.Oper.Add("• Dígítos para Número de Inscrição: Composição do número de inscrição dos candidatos referente ao processo seletivo.");
			help.Oper.Add("• Dias para a Apresentação: Quantidade de dias úteis para apresentação após convocação.");
			help.Oper.Add("• Resolução SEEDUC: Número da resolução SEEDUC referente ao processo seletivo.");
			help.Oper.Add("• Data de Publicação do DO: Data de publicação no Diário Oficial.");
			help.Oper.Add("• Observação: Observações para os candidatos. Este campo será exibido no site ao fazer a inscrição.");
            help.Oper.Add("* Se preencher data início é necessário preencher data fim.");

			help.Oper.TitleAdd("Aba: Habilitações");
			help.Oper.Add("• Coordenadoria: Coordenadoria referente ao processo seletivo.");
			help.Oper.Add("• Função: Função referente ao processo seletivo e à coordenadoria.");
			help.Oper.Add("• Disciplina: Disciplina referente ao processo seletivo e ao cargo.");
			help.Oper.Add("• Vagas: Quantidade de vagas disponíveis para o cargo.");

			help.Oper.TitleAdd("Aba: Experiências");
			help.Oper.Add("• Experiência: Experiência requerida pelo processo seletivo.");
			help.Oper.Add("• Pontuação: Pontuação referente à experiência.");

			help.Oper.TitleAdd("Aba: Titulações");
			help.Oper.Add("• Titulação: Titulação associada ao processo seletivo.");
			help.Oper.Add("• Pontuação: Pontuação referente à titulação.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
			help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
			help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
			help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
			help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");

			help.Oper.TitleAdd("Habilitações/Experiências/Titulações");
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
