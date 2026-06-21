namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.MapeamentoAtributos;

    public class LyFuncao : IEntity
    {
        public string Funcao { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "FUNCAOBB")]
        public string FuncaoBB { get; set; }

        public string Tipo { get; set; }

        //Regente
        [AtributoCampo(Nome = "CAMPO_01")]
        public string Campo01 { get; set; }

        //Função Extra-Classe?
        [AtributoCampo(Nome = "CAMPO_02")]
        public string Campo02 { get; set; }

        //Libera GLP na 2ª Matrícula
        [AtributoCampo(Nome = "CAMPO_03")]
        public string Campo03 { get; set; }

        //Diretor
        [AtributoCampo(Nome = "CAMPO_04")]
        public string Campo04 { get; set; }

        //Secretário
        [AtributoCampo(Nome = "CAMPO_05")]
        public string Campo05 { get; set; }

        //Desaloca aulas
        [AtributoCampo(Nome = "CAMPO_06")]
        public string Campo06 { get; set; }

        //Permite GLP
        [AtributoCampo(Nome = "CAMPO_07")]
        public string Campo07 { get; set; }

        //Compatível com Ensino Médio - DOC I
        [AtributoCampo(Nome = "CAMPO_08")]
        public string Campo08 { get; set; }

        //Compatível com Ensino Fundamental - DOC II
        [AtributoCampo(Nome = "CAMPO_09")]
        public string Campo09 { get; set; }

        //Permite Contrato Temporário
        [AtributoCampo(Nome = "CAMPO_10")]
        public string Campo10 { get; set; }

        [AtributoCampo(Nome = "SEMCARGAHORARIAEFETIVA")]
        public string SemCargaHorariaEfetiva { get; set; }

        [AtributoCampo(Nome = "INGRESSODOCENTE")]
        public string IngressoDocente { get; set; }
    }
}
