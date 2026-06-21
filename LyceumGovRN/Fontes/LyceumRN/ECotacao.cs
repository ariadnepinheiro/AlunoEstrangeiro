using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class ECotacao
    {
        public decimal? ID          { get; set; }        
        public decimal? IDITEM      { get; set; }  
        public String   DESCITEM    { get; set; }  
        public String   MARCA       { get; set; }  
        public DateTime DATA        { get; set; }  
        public decimal? PRECO       { get; set; }
        public String   USUARIO     { get; set; }  
        public decimal? IDFORNEC    { get; set; }
        public decimal? IDITEMDESP  { get; set; }  
    }
}
