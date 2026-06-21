namespace Techne.Lyceum.Net.Academico
{
    using Techne.Web;

    public partial class Boletim
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;

            help.Summary.Add("Consultar o boletim do aluno.");

            help.Oper.TitleAdd("Consultar Boletim");

            help.Oper.Add("Ao acessar a página de boletim, as informações do boletim serão exibidas.");

            help.Oper.TitleAdd("Descrição dos Campos");

            help.Oper.Add("• Código: Código da disciplina.");
            help.Oper.Add("• Disciplina: Descrição da disciplina.");
            help.Oper.Add("• Instrumentos: As próximas colunas listam os instrumentos de avaliação.");
            help.Oper.Add("• Total de Faltas: Total de faltas por disciplina.");
        }
    }
}