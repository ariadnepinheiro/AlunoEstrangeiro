namespace Techne.Lyceum.Net.Menu
{
    using Techne.Web;

    public partial class Config
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Funcionalidades disponíveis para os alunos:");
            help.Summary.Add("• Boletim;");

            help.Oper.Add("O aluno pode consultar as suas informações no sistema.");
        }
    }
}