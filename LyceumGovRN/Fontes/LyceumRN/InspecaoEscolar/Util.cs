using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.InspecaoEscolar
{
   public class Util
    {
    }
   public class TipoResposta
   {

       public const int MULTIPLA_ESCOLHA_COM_1_OPCAO = 2;
       public const int MULTIPLA_ESCOLHA_COM_VARIAS_OPCOES = 3;
       public const int DESCRITIVA = 4;
       public const int SEM_RESPOSTA = 5;
       public const int DEPENDÊNCIAS_SALADEAULA = 6;
       public const int DEPENDÊNCIAS_BANHEIRO = 7;
       public const int CONSIDERAÇÕES_FINAIS = 8;
   }
       
   public class dependencias_SaleBan
   {
       public string dependencia { get; set; }
       public string faculdade { get; set; }
       public bool placadeIdentificacao { get; set; }
       public int? identificacaodependencia { get; set; }

       public List<perguntas> l_perguntas = new List<perguntas>();
   }
   public class perguntas
   {
       public string campanhaid { get; set; }
       public string opassuntoid { get; set; }
       public string assuntoid { get; set; }
       public string grupoid { get; set; }
       public string respostaid { get; set; }
   }

   public class respostasPerguntas_SaleBan
   {
       public int campanhaEscolaid { get; set; }
       public int respostadependenciaid { get; set; } //ID DA DEPENDENCIA
       public string dependencia { get; set; }
       public bool placadeIdentificacao { get; set; }

       public List<respostas> l_respostas = new List<respostas>();  
   }
   public class respostas
   {
       public int opcoesassuntoid{ get; set; }//pergunta
       public string descricao{ get; set; }
       public int acaodirecaoid { get; set; }//resposta
       public int respostadependenciaopcaoid { get; set; }//ID DA RESPOSTA
   }

}
