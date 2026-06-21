using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    public partial class CandidatoPropostaContrato
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Efetuar proposta de contrato temporário.");

            help.Oper.Add("Para efetuar proposta de contrato temporário, é necessário fazer uma pesquisa pelo processo seletivo e pelo candidato clicando no botão ?I ao lado de sua grade de pesquisa.", "~/Images/bt_drop.png");
            help.Oper.Add("Ao clicar no botão ?I na grade de pesquisa de processo seletivo, os registros de processos seletivos são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada por Processo Seletivo e/ou Descrição. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar o processo seletivo de interesse clicando na linha em que o processo seletivo aparece na lista suspensa.");
            help.Oper.Add("Ao clicar no botão ?I na grade de pesquisa de candidato, os registros de candidatos são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada por Candidato e/ou Nome. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar o candidato de interesse clicando na linha em que o candidato aparece na lista suspensa.");
            help.Oper.Add("Após as duas pesquisas, a tela de proposta de contrato temporário irá ser carregada.");
            help.Oper.Add("Obs.: Nas pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' da grade de pesquisa de processos seletivos para que só sejam exibidos registros contendo a palavra 'História', digite %História na coluna 'Descrição'.");

            help.Oper.TitleAdd("Efetuando proposta de contrato temporário");
            help.Oper.Add("Depois de selecionado o Processo Seletivo e o Candidato uma nova tela sera carregada abaixo.");
            help.Oper.Add("As abas de 'Dados Candidato' e 'Documentos' estão travadas e só podem ser consultadas.");
            help.Oper.Add("Na aba de 'Folha de Pagamento' preencha os campos obrigatórios.");
            help.Oper.Add("Para efetuar proposta de contrato temporário deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a proposta de contrato temporário deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Área de Pesquisa: Processo Seletivo");
            help.Oper.Add("• Processo Seletivo: Código de identificação do processo seletivo.");
            help.Oper.Add("• Descrição: Descrição do processo seletivo.");
            help.Oper.TitleAdd("Área de Pesquisa: Candidato");
            help.Oper.Add("• Candidato: Código de identificação do candidato.");
            help.Oper.Add("• Nome: Nome do candidato.");

            help.Oper.TitleAdd("Aba: Dados Candidato");
            help.Oper.TitleAdd("• Dados Pessoais");
            help.Oper.Add("• Nome Completo: Nome completo do docente.");
            help.Oper.Add("• Data Nascimento: Data de nascimento do docente.");
            help.Oper.Add("• Sexo: Sexo do docente.");
            help.Oper.Add("• Nome da Mãe: Nome completo da mãe do docente.");
            help.Oper.Add("• Nome do Pai: Nome completo do pai do docente.");
            help.Oper.Add("• Necessidade Especial: Informar se o docente possui alguma necessidade especial. (Tabela Geral: Necessidade Especial)");
            help.Oper.Add("• Estado Civil: Estado civil do docente. (Tabela Geral: Estado civil)");
            help.Oper.Add("• País de Nascimento: País de nascimento do docente. (Tabela: Países)");
            help.Oper.Add("• Nacionalidade: Nacionalidade do docente. (Tabela: Nacionalidades)");
            help.Oper.Add("• Naturalidade: Naturalidade do docente.");
            help.Oper.Add("• Estado: Estado onde se localiza a residência do docente.");
            help.Oper.TitleAdd("• Endereço");
            help.Oper.Add("• País: País onde se localiza a residência  do docente. (Tabela: Países)");
            help.Oper.Add("• CEP: CEP da residência do docente.");
            help.Oper.Add("• Município: Município onde se localiza a residência do docente.");
            help.Oper.Add("• Estado: Estado onde se localiza a residência do docente.");
            help.Oper.Add("• Endereço: Endereço da residência do docente.");
            help.Oper.Add("• N.º: Número da residência do docente.");
            help.Oper.Add("• Complemento.: Complemento da residência do docente.");
            help.Oper.Add("• Bairro: Bairro onde se localiza a residência do docente.");
            help.Oper.TitleAdd("• Contato");
            help.Oper.Add("• Telefone: Telefone principal de contato do docente.");
            help.Oper.Add("• Celular: Celular de contato com o docente.");
            help.Oper.Add("• E-mail: Endereço eletrônico de contato com o docente.");

            help.Oper.TitleAdd("Aba: Documentos");
            help.Oper.TitleAdd("• PIS/PASEP");
            help.Oper.Add("• PIS/PASEP: Número do PIS/PASEP do docente.");
            help.Oper.TitleAdd("• Carteira Profissional");
            help.Oper.Add("• Número: Número da carteira de trabalho do docente.");
            help.Oper.Add("• Série: Série da carteira de trabalho do docente.");
            help.Oper.Add("• Data de Expedição: Data de expedição da carteira de trabalho do docente.");
            help.Oper.Add("• Estado: Estado onde foi emitida a carteira de trabalho do docente. (Tabela Fixa: UF)");

            help.Oper.TitleAdd("Aba: Folha de Pagamento");
            help.Oper.Add("• Coordenadoria: Código de identificação e descrição da coordenadoria.");
            help.Oper.Add("• Habilitação: Código de identificação e descrição da habilitação.");
            help.Oper.Add("• Disciplina de Ingresso: Código de identificação e descrição da disciplina.");
            help.Oper.Add("• Unidade de Ensino: Código de identificação e nome da disciplina.");
            help.Oper.Add("• Banco: Número do banco.");
            help.Oper.Add("• Agência: Número da agência.");
            help.Oper.Add("• Conta: Número da conta.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
