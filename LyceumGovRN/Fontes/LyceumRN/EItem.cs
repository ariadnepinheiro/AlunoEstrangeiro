using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class EItem
    {
        public Int32 ID { get; set; }
        public string prdCodigo { get; set; }
        public string prdNome { get; set; }
        public string prdEspec { get; set; }
        public string prdTPatr { get; set; }
        public string prdNAntigo { get; set; }
        public string prdUnidad { get; set; }
        public Int32? prdCodPai { get; set; }
        public string prdDeuso { get; set; }
        public string prdPNAE { get; set; }


        public void OnValidateFieldValue()
        {
            if (prdCodigo.Length == 0)
                throw new Exception("O ITEMID é obrigatório");

            if (prdNome.Length == 0 || prdNome.Length > 200)
                throw new Exception("O Nome do Item deve ter o tamanho entre 1 e 200 caracteres");

            if (prdEspec.Length == 0 || prdEspec.Length > 2000)
                throw new Exception("A Especificação de ter o tamanho entre 1 e 2000 caracteres");

            if (prdTPatr.Length == 0 || prdTPatr.Length > 40)
                throw new Exception("O Patrimônio de ter o tamanho entre 1 e 40 caracteres");
        }
    }
}
