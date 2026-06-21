namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.MapeamentoAtributos;

    public class HdUsuario : IEntity
    {
        public string Usuario { get; set; }

        public string Sis { get; set; }

        public string Nome { get; set; }

        public string Grupousu { get; set; }

        public string Senha { get; set; }

        public string Privilegiado { get; set; }

        public string Email { get; set; }

        public string Matricula { get; set; }

        [AtributoCampo(Nome = "IDVINCULO")]
        public string IdVinculo { get; set; }

        public string Setor { get; set; }

        public string Habilitado { get; set; }

        public string MotivoDesabilitado { get; set; }

        public DateTime? DataAlteracaoSenha { get; set; }

        public string AlterarSenha { get; set; }

        public string SenhasAnteriores { get; set; }

        public string SenhaTemp { get; set; }

        public DateTime? UltimoLogin { get; set; }

        public Decimal? TentativasIncorretas { get; set; }

        public string Winusuario { get; set; }

        public Decimal? Pessoa { get; set; }

        public string PrivUnidadeEns { get; set; }
    }
}
