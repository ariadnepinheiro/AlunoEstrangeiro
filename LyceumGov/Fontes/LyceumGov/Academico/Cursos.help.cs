using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    public partial class Cursos
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover (excluir) escolaridades.");

            help.Oper.TitleAdd("Informando dados de pesquisa");
            help.Oper.Add("Para consultar uma escolaridade é necessário fazer uma pesquisa pela escolaridade de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
            help.Oper.Add("Todas as escolaridades existentes são apresentadas em uma lista suspensa.");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da escolaridade. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar a escolaridade de interesse clicando na linha em que a escolaridade aparece na lista suspensa.");
            help.Oper.Add("Obs.: Nesta pesquisa, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Descrição' para que só sejam exibidos registros contendo a palavra 'Fundamental', digite %Fundamental ou *Fundamental na coluna 'Descrição'.");

            help.Oper.TitleAdd("Consultando Escolaridades");
            help.Oper.Add("A consulta é realizada automaticamente quando a escolaridade de interesse for selecionada.");
            help.Oper.Add("Os dados da escolaridade selecionada serão apresentados na tela divididas em 3 abas: “Dados Gerais”, “Duração Aulas” e “Itinerário Formativo (Censo)”.");
            help.Oper.Add("Obs.: Esta consulta ficará desabilitada nos modos de inclusão e de alteração de um registro.");

            help.Oper.TitleAdd("Cadastrar Nova Escolaridade");
            help.Oper.Add("Para cadastrar uma escolaridade deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Deve-se preencher os campos referentes à escolaridade, iniciando pela aba “Dados Gerais”.");
            help.Oper.Add("Para salvar a nova escolaridade deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a inclusão deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("A operação retornará sucesso caso o registro tenha sido incluído com sucesso, ou uma mensagem de erro informando os erros ocorridos durante a inserção.");

            help.Oper.TitleAdd("Alterando escolaridade");
            help.Oper.Add("Para alterar uma escolaridade é necessário fazer a consulta da escolaridade desejada. Ver 'Consultando escolaridades'.");
            help.Oper.Add("Para alterar os dados da escolaridade deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
            help.Oper.Add("Os dados da escolaridade serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar as alterações deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a alteração deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("A operação retornará sucesso caso o registro tenha sido alterado com sucesso, ou uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo (excluindo) escolaridade");
            help.Oper.Add("Para remover (excluir) uma escolaridade, é necessário fazer a consulta da escolaridade desejada. Ver 'Consultando currículo'.");
            help.Oper.Add("Para remover (excluir) a escolaridade deve-se clicar no botão ?I e confirmar a exclusão.", "~/Images/SmallDelete.png");
            help.Oper.Add("A operação retornará sucesso caso o registro tenha sido removido (excluído) com sucesso, ou uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Aba: Dados Gerais");
            help.Oper.TitleAdd("Dados Gerais");
            help.Oper.Add("• Código: Código identificador único da escolaridade.");
            help.Oper.Add("• Sigla: Sigla da escolaridade.");
            help.Oper.Add("• Nome: Nome descritivo da escolaridade.");
            help.Oper.Add("• Modalidade: Modalidade referente à escolaridade.");
            help.Oper.Add("• Tipo: Tipo de Modalidade");
            help.Oper.Add("• Nível/Segmento: Nível exigido pela escolaridade.");
            help.Oper.Add("• Certificação: Certificação obtida pelo aluno na escolaridade.");
            help.Oper.Add("• Estrutura Curricular:");
            help.Oper.Add("o Formação geral básica");
            help.Oper.Add("o Itinerário formativo");
            help.Oper.Add("• Itinerário Formativo:	É um conjunto de atividades como disciplinas, projetos e oficinas, no Novo Ensino Médio.");
            help.Oper.Add("• Trilha de Aprendizagem: É um caminho estruturado, flexível e personalizado de conteúdos e atividades que guiam o desenvolvimento de competências e o alcance de objetivos educacionais ou profissionais.");
            help.Oper.Add("• Máximo Componente por Série: Geralmente se refere ao número máximo de alunos por sala de aula ou turma em uma série de ensino, que pode variar de acordo com a legislação e o nível de ensino.");
            help.Oper.Add("o Não se aplica");
            help.Oper.Add("• Ativo: Indica se a escolaridade está em estado ativo ou inativo. ");
            help.Oper.Add("• Tem reclassificação: Indica se curso possui reclassificação.");
            help.Oper.Add("• Oferta Eletiva: Consiste num conjunto de componentes curriculares de livre escolha do aluno, que visam aprofundar e enriquecer o seu repertório, de acordo com os seus interesses, necessidades e projeto de vida.");
            help.Oper.Add("• Concluintes Anteriores: Indica se a escolaridade possui concluintes anteriores.");
            help.Oper.Add("• Educação Profissional Concomitante: É o mesmo que curso técnico concomitante.");
            help.Oper.Add("• Sala Externa: É um espaço de convivência com dimensões generosas, projetado para proporcionar conforto e funcionalidade, e pode integrar diferentes ambientes como a sala de estar e de jantar ou até mesmo espaços exteriores. ");
            help.Oper.Add("• Confirmação de Turnos e Vagas:");
            help.Oper.Add("o Participa do cálculo de percentual da criação de turmas novas: ");
            help.Oper.Add("o Permite choque de turno integral:");

            help.Oper.Add("• Participa Fechamento Automático Ano Letivo: Se participa ou não do fechamento automático Ano Letivo.	");
            help.Oper.Add("• Permite transferência irrestrita: Se permite ou não transferência irrestrita.");
            help.Oper.Add("• Habilitação: Habilitações da formação na escolaridade.");
            help.Oper.Add("• Decreto: Decreto de lei que autoriza o funcionamento da escolaridade na instituição.");
            help.Oper.Add("• Data DO: Data de publicação do decreto da escolaridade no Diário Oficial.");
            help.Oper.Add("• Capacidade de Atendimento: Capacidade de atendimento disponível para a escolaridade.");

            help.Oper.TitleAdd("Aba: Duração Aulas");
            help.Oper.Add("• Duração das aulas por ano");
            help.Oper.Add("o Ano");
            help.Oper.Add("o Turno");
            help.Oper.Add("o Duração em minutos");

            help.Oper.TitleAdd("Aba: Itinerário Formativo (Censo) – os blocos abaixo listados serão exibidos de acordo com o que for selecionado, conforme a necessidade de cada escolaridade e previamente cadastrados.");
            help.Oper.Add("• Unidade curricular");
            help.Oper.Add("o Eletivas");
            help.Oper.Add("o Língua/Literatura estrangeira - Espanhol");
            help.Oper.Add("o Projeto de vida");
            help.Oper.Add("o Libras");
            help.Oper.Add("o Língua/Literatura estrangeira - Francês");
            help.Oper.Add("o Trilhas de aprofundamento/aprendizagens");
            help.Oper.Add("o Língua indígena");
            help.Oper.Add("o Língua/Literatura estrangeira - Outra");

            help.Oper.Add("• Área do itinerário formativo");
            help.Oper.Add("o Linguagens e suas tecnologias");
            help.Oper.Add("o Ciências da natureza e suas tecnologias");
            help.Oper.Add("o Formação técnica e profissional");
            help.Oper.Add("o Matemática e suas tecnologias");
            help.Oper.Add("o Ciências humanas e sociais aplicadas");
            help.Oper.Add("o Itinerário formativo integrado");

            help.Oper.Add("• Tipo de curso do itinerário de formação técnica e profissional");
            help.Oper.Add("o Curso técnico");
            help.Oper.Add("o Qualificação profissional técnica");

            help.Oper.Add("• Composição do itinerário formativo integrado");
            help.Oper.Add("o Linguagens e suas tecnologias");
            help.Oper.Add("o Ciências da natureza e suas tecnologias");
            help.Oper.Add("o Formação técnica e profissional");
            help.Oper.Add("o Matemática e suas tecnologias");
            help.Oper.Add("o Ciências humanas e sociais aplicadas");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
            help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Cancela a operação registro e retorna para página inicial.", "~/Images/SmallCancel.png");
            help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
            help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }

    }
}
