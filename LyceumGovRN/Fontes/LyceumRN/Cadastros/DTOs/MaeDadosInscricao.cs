using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Cadastros.DTOs
{
    public class MaeDadosInscricao
    {
        public int MaeInscricaoId { get; set; }

        public string CPF { get; set; }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string Email { get; set; }

        public string Celular { get; set; }

        public string FixoCelular { get; set; }

        public string NumeroRG { get; set; }

        public string OrgaoRG { get; set; }

        public string UfRG { get; set; }

        public int EscolaridadeId { get; set; }

        public string EscolaridadeDescricao { get; set; }

        public string CEP { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public int BairroId { get; set; }

        public string BairroDescricao { get; set; }

        public string MunicipioId { get; set; }

        public string MunicipioDescricao { get; set; }

        public string UF { get; set; }

        public bool TomouVacina { get; set; }

        public bool DoseUnica { get; set; }

        public DateTime DataVacina1 { get; set; }

        public DateTime? DataVacina2 { get; set; }

        public bool Desempregado { get; set; }

        public bool SeguroDesemprego { get; set; }

        public bool RendaSuperior { get; set; }

        public bool CargaHorariaLivre { get; set; }

        public bool ExperienciaTrabalho { get; set; }

        //public int MaeInscricaoArquivoId { get; set; }

        //public string TipoArquivoCurriculo { get; set; }

        public bool? Habilitado { get; set; }

        public int? MaeMotivoNaoHabilitadoId { get; set; }

        public string MaeMotivoNaoHabilitadoDescricao { get; set; }

        public bool? Manha { get; set; }

        public bool? Tarde { get; set; }
    }
}