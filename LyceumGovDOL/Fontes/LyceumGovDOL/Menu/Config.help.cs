namespace Techne.Lyceum.Net.Menu
{
    using Techne.Web;

    public partial class Config
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;

            help.Summary.Add("Divisão do sistema que organiza as funcionalidades disponíveis para os docentes:");
            help.Summary.Add("• Lançamento de Notas;");
            help.Summary.Add("• Cadastramento para GLP;");
            help.Summary.Add("• Avaliação Currículo Mínimo;");
            help.Summary.Add("• Currículo Mínimo;");
            help.Summary.Add("• Protocolos.");

            help.Oper.Add("As alterações dos docentes são realizadas para uma determinada turma.");
        }
    }
}