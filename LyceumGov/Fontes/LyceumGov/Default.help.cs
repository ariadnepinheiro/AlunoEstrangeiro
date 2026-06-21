using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net
{
    public partial class _default
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Esta referência dos menus descreve cada item de menu da janela principal do LyceumNet Seeduc assim como sua utilização. ");

            help.Oper.TitleAdd("Navegação pelo menu");
            help.Oper.Add("Para navegar no menu basta posicionar o mouse sobre a aba desejada - Básico, Currículo, Acadêmico, Hades - e os itens do menu contendo o nome da página aparecerão.");
            help.Oper.Add("Clique sobre a página desejada e será redirecionado automaticamente para esta página.");
            help.Oper.Add("Obs.: Os itens exibidos nos menus variam de acordo com a permissão do usuário ativo no sistema.");

            help.Oper.TitleAdd("Descrição das funcionalidades das páginas divididas por menu e dispostas em ordem alfabética:");
            help.Oper.TitleAdd("1 - Menu Básico");
            help.Oper.Add("• Alteração Data Lotação: Permite alterar a data de lotação dos alunos em determinada unidade.");
            help.Oper.Add("• Capacitação: Permite consultar, cadastrar, alterar ou excluir capacitações de uma pessoa.");
            help.Oper.Add("• Conceitos: Permite consultar, cadastrar, alterar e excluir grupos de conceitos e conceitos.");
            help.Oper.Add("• Coordenadoria: Permite consultar, cadastrar, alterar ou excluir coordenadorias.");
            help.Oper.Add("• Disciplina: Permite consultar, cadastrar, alterar e remover disciplinas.");
            help.Oper.Add("• Docentes: Permite consultar, cadastrar, alterar e remover docentes.");
            help.Oper.Add("• Formação Pessoal: Permite consultar, cadastrar, alterar e excluir as formações de uma pessoa.");
            help.Oper.Add("• Função do Docente em GLP: Permite consultar, incluir ou excluir uma função de um docente em GLP.");
            help.Oper.Add("• Funções: Permite consultar, cadastrar, alterar e excluir funções.");
            help.Oper.Add("• Grupo de Habilitações: Permite consultar, cadastrar, alterar e excluir grupos de habilitações");
            help.Oper.Add("• Grupo de Habilitações do Docente: Permite consultar, cadastrar, alterar e excluir grupos de habilitações de um docente.");
            help.Oper.Add("• Grupo de Habilitações por Disciplina: Permite consultar, cadastrar, alterar e excluir grupos de habilitações por disciplinas.");
            help.Oper.Add("• Lotação: Permite consultar, cadastrar, alterar ou excluir lotações de uma pessoa.");
            help.Oper.Add("• Lotação Docente: Permite consultar, cadastrar, alterar ou excluir lotações de um docente.");
            help.Oper.Add("• Lotação Funcionário: Permite consultar lotação de funcionários por unidade física.");
            help.Oper.Add("• Matrículas do Docente: Permite consultar o número de matrícula de um docente.");
            help.Oper.Add("• Papéis da Pessoa: Permite consultar, cadastrar, alterar e remover papéis que podem ser desempenhados pelas pessoas nos relacionamentos.");
            help.Oper.Add("• Período Letivo: Permite consultar, cadastrar, alterar e excluir períodos letivos.");
            help.Oper.Add("• Restrição de Acesso por Usuários: Permite consultar, cadastrar e excluir o acesso de um usuário do sistema aos dados de unidades físicas.");
            help.Oper.Add("• Sistema de Avaliação: Permite consultar, cadastrar, alterar e excluir sistemas de avaliação.");
            help.Oper.Add("• Subnível: Permite consultar, cadastrar, alterar e remover subníveis.");
            help.Oper.Add("• Subperíodo Letivo: Permite consultar, cadastrar, alterar e remover subperíodos letivos.");
            help.Oper.Add("• Tipos de Dependência: Permite consultar, cadastrar, alterar e remover tipos de dependência.");
            help.Oper.Add("• Tipos de Disciplina: Permite consultar, cadastrar, alterar e remover tipos de disciplina.");
            help.Oper.Add("• Tipos de Ingresso: Permite consultar, cadastrar, alterar e remover tipos de ingresso.");
            help.Oper.Add("• Titulação: Permite consultar, cadastrar, alterar e remover titulações.");
            help.Oper.Add("• Turno: Permite consultar, cadastrar, alterar e remover turnos.");
            help.Oper.Add("• Unidades de Ensino: Permite consultar, cadastrar, alterar e remover Unidades de Ensino.");
            help.Oper.Add("• Unidades Físicas: Permite consultar, cadastrar, alterar e remover Unidades Físicas.");

            help.Oper.TitleAdd("2 - Menu Currículo");
            help.Oper.Add("• Carga Horária das Categorias: Permite consultar, cadastrar, alterar e excluir a carga horária de uma categoria.");
            help.Oper.Add("• Currículos: Permite consultar, cadastrar, alterar e excluir currículos, seus anos escolares e unidades físicas.");
            help.Oper.Add("• Horário Operacional: Permite consultar, cadastrar, alterar e excluir horários operacionais.");
            help.Oper.Add("• Turmas:");

            help.Oper.TitleAdd("3 - Menu Acadêmico");
            help.Oper.Add("• Alunos: Permite consultar, cadastrar, alterar e remover alunos.");
            help.Oper.Add("• Alunos Cedidos: Permite consultar, cadastrar, alterar e remover alunos cedidos.");
            help.Oper.Add("• Carteirinhas: Permite consultar, cadastrar, alterar e remover carteirinhas.");
            help.Oper.Add("• Cursos: Permite consultar, cadastrar, alterar e remover cursos.");
            help.Oper.Add("• Frequências: Permite consultar frequências das turmas.");
            help.Oper.Add("• Controle de Gratuidades: Permite consultar gratuidades por aluno.");
            help.Oper.Add("• Controle de Faltas Justificadas: Permite consultar, cadastrar, alterar e remover faltas justificadas dos alunos.");
            help.Oper.Add("• Ocorrências: Permite consultar ocorrências de alunos.");
            help.Oper.Add("• Matrícula: Permite consultar, efetuar e excluir matrícula de alunos em turma/ano/período.");
            help.Oper.Add("• Matrícula por Disciplina: Permite consultar, efetuar e excluir matrículas do aluno por disciplina.");
            help.Oper.Add("• Controle de Merendas: Permite consultar retirada de merenda pelos alunos.");
            help.Oper.Add("• Pessoas: Permite consultar, cadastrar, alterar e remover pessoas.");
            help.Oper.Add("• Controle de Presença sem Cartão: Permite consultar alunos presentes sem apresentação do cartão e respectivas datas.");


            help.Oper.TitleAdd("4 - Menu Hades");
            help.Oper.Add("• Padrões de Acesso: Permite consultar, cadastrar, alterar e remover padrões de acesso ao sistema.");
            help.Oper.Add("\tConsultar, definir, alterar e remover transações permitidas por cada padrão de acesso.");
            help.Oper.Add("\tConsultar, definir e remover usuários para casa padrão de acesso.");
            help.Oper.Add("• Setores: Permite consultar, cadastrar, alterar e remover setores e subsetores apresentados de forma hierárquica.");
            help.Oper.Add("• Sistemas: Permite consultar o sistema, sua versão e suas transações. Além de permitir a definição de nomes dos menus, transações públicas e transações de auditoria.");
            help.Oper.Add("• Tabela Geral:");
            help.Oper.Add("• Usuários: Consultar, cadastrar e remover usuários e seus padrões de acesso.");

        }
    }
}

