using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.DTO
{
    public class UploadUnidadeAdministrativa
    {

        [Display(Name = "Tipo Unidade Administrativa")]
        public TipoUnidadeAdministrativa.Tipos_Upload? TipoUpload { get; set; }       
        public IEnumerable TiposUpload { get; set; }
                
        public IList<ArquivoImportacao> Arquivos { get; set; }
    }
}