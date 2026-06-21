using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
    public partial class ResetaSenhaUsuarios
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Resetar senha de alunos/docentes no portal ao Aluno online/Docente online.");
        
            help.Oper.TitleAdd("Resetando a senha de um aluno");
            help.Oper.Add("Para resetar a senha de um aluno selecione a aba 'Aluno online', consulte a matrícula do aluno e clique no botão 'Resetar'.");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");
            
            help.Oper.TitleAdd("Resetando a senha de um docente");
            help.Oper.Add("Para resetar a senha de um docente selecione a aba 'Docente online', consulte a matrícula do aluno e clique no botão 'Resetar'.");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");
        }
    }
}