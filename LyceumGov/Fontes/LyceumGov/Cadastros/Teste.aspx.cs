using System.Collections.Generic;
namespace Techne.Lyceum.Net.Cadastros
{
    public partial class Teste : System.Web.UI.Page
    {
        public object Lista()
        {
            return new List<Pessoa>
            {
                new Pessoa { Id = 1, Nome = "Fulano de Tal", RG = "11.111.111-1", CPF = "111.111.111-11" },
                new Pessoa { Id = 2, Nome = "Ciclano da Silva", RG = "22.222.222-2", CPF = "222.222.222-22" },
                new Pessoa { Id = 3, Nome = "Beltrano de Souza", RG = "33.333.333-3", CPF = "333.333.333-33" },
                new Pessoa { Id = 4, Nome = "aaa", RG = "444", CPF = "444444" },
                new Pessoa { Id = 5, Nome = "bbb", RG = "555", CPF = "555555" },
                new Pessoa { Id = 6, Nome = "ccc", RG = "666", CPF = "666666" },
                new Pessoa { Id = 7, Nome = "ddd", RG = "777", CPF = "777777" },
                new Pessoa { Id = 8, Nome = "eee", RG = "888", CPF = "888888" },
                new Pessoa { Id = 9, Nome = "fff", RG = "999", CPF = "999999" },
                new Pessoa { Id = 10, Nome = "ggg", RG = "101010", CPF = "101010101010" },
            };
        }

        public class Pessoa
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string RG { get; set; }
            public string CPF { get; set; }
        }
    }
}
