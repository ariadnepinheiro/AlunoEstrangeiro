using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    public partial class CandidatoDocenteCH
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover ficha de inscrição.");

            help.Oper.TitleAdd("Consultando ficha de inscrição");
            help.Oper.Add("Para consultar uma ficha de inscrição, é necessário fazer uma pesquisa pelo processo seletivo e pelo candidato clicando no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("Ao clicar no botão ?I na grade de pesquisa de processo seletivo, os registros de processos seletivos são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada por Processo Seletivo e/ou Descrição. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar o processo seletivo de interesse clicando na linha em que o processo seletivo aparece na lista suspensa.");
            help.Oper.Add("Ao clicar no botão ?I na grade de pesquisa de candidato, os registros de candidatos são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada por Candidato e/ou Nome. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar o candidato de interesse clicando na linha em que o candidato aparece na lista suspensa.");
            help.Oper.Add("Após as duas pesquisas, a ficha do candidato irá ser carregada na tela.");
            help.Oper.Add("Obs.: Nas pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de processos seletivos para que só sejam exibidos registros contendo a palavra 'História', digite %História na coluna 'Descrição'.");

            help.Oper.TitleAdd("Cadastrando nova ficha de inscrição");
            help.Oper.Add("Para cadastrar um processo seletivo deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Um formulário em branco é carregado para o preenchimento dos dados do novo registro.");
            help.Oper.Add("Para salvar os dados da nova ficha de inscrição deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da nova ficha de inscrição deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
            help.Oper.Add("Obs.: No modo de cadastro de da nova ficha de inscrição, as abas 'Titulações/Experiências', 'Processo Seletivo', 'Situação da Avaliação' e 'Contrato Temporário' ficam desativadas.");

            help.Oper.TitleAdd("Alterando ficha de inscrição");
            help.Oper.Add("Para alterar uma ficha de inscrição é necessário fazer a consulta do processo seletivo e do candidato desejado. Ver 'Consultando ficha de inscrição'.");
            help.Oper.Add("Para alterar os dados da ficha de inscrição selecionada deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
            help.Oper.Add("Um formulário é carregado com os dados da ficha de inscrição selecionada permitindo alteração nos campos.");
            help.Oper.Add("Para salvar os dados da ficha de inscrição deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a alteração nos dados da ficha de inscrição deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo ficha de inscrição");
            help.Oper.Add("Para remover uma ficha de inscrição é necessário fazer a consulta do processo seletivo e do candidato desejado. Ver 'Consultando ficha de inscrição'.");
            help.Oper.Add("Para remover a ficha de inscrição selecionada deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/Images/SmallDelete.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Cadastrando novos registros nas abas 'Titulações/Experiências' e 'Situação de Avaliação'");
            help.Oper.Add("Para cadastrar uma titulação e/ou uma experiência e/ou situação é necessário fazer a consulta do processo seletivo e do candidato desejado. Ver 'Consultando ficha de inscrição'.");
            help.Oper.Add("Selecione a aba 'Titulações/Experiências' e/ou 'Situação de Avaliação' e depois clique no botão ?I da grade desejada.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados.");
            help.Oper.Add("Para salvar os dados da novo registro deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão do novo registro deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando registros nas abas 'Titulações/Experiências' e 'Situação de Avaliação");
            help.Oper.Add("Para alterar uma titulação e/ou uma experiência e/ou situação é necessário fazer a consulta do processo seletivo e do candidato desejado. Ver 'Consultando ficha de inscrição'.");
            help.Oper.Add("Selecione a aba 'Titulações/Experiências' e/ou 'Situação de Avaliação' e depois clique no botão ?I da grade desejada.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados do registro selecionado serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados do registro selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados do registro selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo registros nas abas 'Titulações/Experiências' e 'Situação de Avaliação");
            help.Oper.Add("Para remover uma titulação e/ou uma experiência e/ou situação é necessário fazer a consulta do processo seletivo e do candidato desejado. Ver 'Consultando ficha de inscrição'.");
            help.Oper.Add("Para remover o registro, a aba 'Titulações/Experiências' e/ou 'Situação de Avaliação' e depois clique no botão ?I da grade desejada e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Área de Pesquisa: Processo Seletivo");
            help.Oper.Add("• Processo Seletivo: Código de identificação do processo seletivo.");
            help.Oper.Add("• Descrição: Descrição do processo seletivo.");
            help.Oper.TitleAdd("Área de Pesquisa: Candidato");
            help.Oper.Add("• Candidato: Código de identificação do candidato.");
            help.Oper.Add("• Nome: Nome do candidato.");

            help.Oper.TitleAdd("Aba: Dados da Inscrição");
            help.Oper.Add("• Candidato: Código de identificação do candidato.");
            help.Oper.TitleAdd("• Área de Pesquisa: Processo Seletivo");
            help.Oper.Add("• Processo Seletivo: Código de identificação do processo seletivo.");
            help.Oper.Add("• Descrição: Descrição do processo seletivo.");
            help.Oper.TitleAdd("• Área de Pesquisa: Coordenadoria");
            help.Oper.Add("• Coordenadoria: Código de identificação da coordenadoria.");
            help.Oper.Add("• Descrição: Descrição da coordenadoria.");
            help.Oper.TitleAdd("• Área de Pesquisa: Habilitação");
            help.Oper.Add("• Categoria: Código de identificação da habilitação.");
            help.Oper.Add("• Descrição: Descrição da habilitação.");
            help.Oper.TitleAdd("• Área de Pesquisa: Disciplina de Ingresso");
            help.Oper.Add("• Disciplina: Código de identificação da disciplina.");
            help.Oper.Add("• Descrição: Descrição da disciplina.");

            help.Oper.TitleAdd("Aba: Dados Pessoais");
            help.Oper.Add("• Nome Completo: Nome completo do docente.");
            help.Oper.Add("• Data Nascimento: Data de nascimento do docente.");
            help.Oper.Add("• Sexo: Sexo do docente.");
            help.Oper.Add("• Necessidade Especial: Informar se o docente possui alguma necessidade especial. (Tabela Geral: Necessidade Especial)");
            help.Oper.Add("• Nome da Mãe: Nome completo da mãe do docente.");
            help.Oper.Add("• Nome do Pai: Nome completo do pai do docente.");
            help.Oper.Add("• Estado Civil: Estado civil do docente. (Tabela Geral: Estado civil)");
            help.Oper.Add("• País de Nascimento: País de nascimento do docente. (Tabela: Países)");
            help.Oper.Add("• Nacionalidade: Nacionalidade do docente. (Tabela: Nacionalidades)");
            help.Oper.Add("• Naturalidade: Naturalidade do docente.");
            help.Oper.Add("• País: País onde se localiza a residência  do docente. (Tabela: Países)");
            help.Oper.Add("• CEP: CEP da residência do docente.");
            help.Oper.Add("• Município: Município onde se localiza a residência do docente.");
            help.Oper.Add("• Endereço: Endereço da residência do docente.");
            help.Oper.Add("• N.º: Número da residência do docente.");
            help.Oper.Add("• Complemento.: Complemento da residência do docente.");
            help.Oper.Add("• Bairro: Bairro onde se localiza a residência do docente.");
            help.Oper.Add("• Telefone: Telefone principal de contato do docente.");
            help.Oper.Add("• Celular: Celular de contato com o docente.");
            help.Oper.Add("• E-mail: Endereço eletrônico de contato com o docente.");

            help.Oper.TitleAdd("Aba: Documentos");
            help.Oper.Add("• Tipo: Tipo do documento informado para a identificação do docente. (Tabela Geral: TIPO DOC)");
            help.Oper.Add("• Número: Número do documento informado.");
            help.Oper.Add("• Estado: Estado onde foi emitido o documento informado. (Tabela Fixa: UF)");
            help.Oper.Add("• Órgão Emissor: Órgão emissor do documento informado. (Tabela Geral: Orgao RG)");
            help.Oper.Add("• Data de Expedição: Data de expedição do documento informado.");
            help.Oper.Add("• CPF: Número do CPF do docente.");
            help.Oper.Add("• PIS/PASEP: Número do PIS/PASEP do docente.");
            help.Oper.TitleAdd("• Carteira Profissional");
            help.Oper.Add("• Número: Número da carteira de trabalho do docente.");
            help.Oper.Add("• Série: Série da carteira de trabalho do docente.");
            help.Oper.Add("• Data de Expedição: Data de expedição da carteira de trabalho do docente.");
            help.Oper.Add("• Estado: Estado onde foi emitida a carteira de trabalho do docente. (Tabela Fixa: UF)");

            help.Oper.TitleAdd("Aba: Titulações/Experiências");
            help.Oper.TitleAdd("• Grade: Titulações");
            help.Oper.Add("• Titulação: Titulação associada ao processo seletivo. (Tabela: Titulações para Processos Seletivos, cujo filtro é Processo Seletivo)");
            help.Oper.Add("• Pontuação: Pontuação referente à titulação.");
            help.Oper.TitleAdd("• Grade: Experiências");
            help.Oper.Add("• Experiência: Experiência requerida pelo processo seletivo. (Tabela: Experiências para Processos Seletivos, cujo filtro é Processo Seletivo)");
            help.Oper.Add("• Pontuação: Pontuação referente à experiência.");

            help.Oper.TitleAdd("Aba: Processo Seletivo");
            help.Oper.Add("• Status: Status do candidato.");
            help.Oper.Add("• Pontuação: Pontuação do candidato.");
            help.Oper.Add("• Data Apresentação: Data de apresentação do candidato.");
            help.Oper.Add("• Hora Apresentação: Hora de apresentação do candidato.");
			help.Oper.Add("• Avaliado pela COSEP: Avaliação do candidato pela COSEP.");
			help.Oper.Add("• Avaliado pela CDGP: Avaliação do candidato pela CDGP.");
            help.Oper.Add("• Remessa Documento: Remessa documento do candidato.");

            help.Oper.TitleAdd("Aba: Situação de Avaliação");
            help.Oper.Add("• Situação: Situação da avaliação do candidato. (Tabela Fixa: LY_CONCURSO_DOC_SIT_AVALIACAO)");

            help.Oper.TitleAdd("Aba: Contrato Temporário");
            help.Oper.Add("• Status: Status do contrato temporário do docente.");
            help.Oper.Add("• Data de Início: Data de início do contrato temporário.");
            help.Oper.Add("• Data de Término: Data de término do contrato temporário.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
            help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
            help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
            help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
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
