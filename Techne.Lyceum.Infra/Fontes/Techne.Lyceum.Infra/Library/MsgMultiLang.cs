using System.Collections;
using System.Data;

namespace Techne.Library
{
    namespace MultiLang
    {
        /// <summary>
        ///   Nome de um objeto (tabela ou coluna) em um determinado idioma.
        ///   Contém também outras informações relacionadas ao nome no idioma.
        /// </summary>
        internal class NomeInfo
        {
            private readonly string pvIdioma;

            private readonly bool pvMasculino;

            private readonly string pvNome;

            private readonly bool pvSingular;

            public NomeInfo()
            {
                this.pvIdioma = "pt-BR"; // * Na verdade este idioma default deveria estar definido no banco!!!
                this.pvNome = string.Empty;
                this.pvMasculino = true;
                this.pvSingular = true;
            }

            public NomeInfo(DataRow NomeRow)
            {
                this.pvIdioma = NomeRow["idioma"].ToString();
                this.pvNome = NomeRow["nome"].ToString();
                this.pvMasculino = NomeRow["genero"].ToString() != "F";
                this.pvSingular = NomeRow["numero"].ToString() != "P";
            }

            public string Idioma
            {
                get
                {
                    return this.pvIdioma;
                }
            }

            public bool Masculino
            {
                get
                {
                    return this.pvMasculino;
                }
            }

            public string Nome
            {
                get
                {
                    return this.pvNome;
                }
            }

            public bool Singular
            {
                get
                {
                    return this.pvSingular;
                }
            }
        }

        /// <summary>
        ///   Array de NomeInfo.
        /// </summary>
        internal struct NomesInfo
        {
            private ArrayList pvNomesInfo;

            /// <summary>
            ///   Determina o nome do objeto (tabela ou coluna) dado o idioma.
            /// </summary>
            public NomeInfo this[string Idioma]
            {
                get
                {
                    if (this.pvNomesInfo != null)
                    {
                        foreach (NomeInfo n in this.pvNomesInfo)
                        {
                            if (n.Idioma == Idioma)
                            {
                                return n;
                            }
                        }
                    }

                    return new NomeInfo();
                }
            }

            public void Add(NomeInfo Nome)
            {
                if (this.pvNomesInfo == null)
                {
                    this.pvNomesInfo = new ArrayList();
                }

                this.pvNomesInfo.Add(Nome);
            }
        }

        /// <summary>
        ///   Uma mensagem em um determinado idioma.
        /// </summary>
        internal struct MensagemIdioma
        {
            private readonly string pvIdioma;

            private readonly string pvMensagem;

            public MensagemIdioma(string Idioma, string Mensagem)
            {
                this.pvIdioma = Idioma;
                this.pvMensagem = Mensagem;
            }

            public string Idioma
            {
                get
                {
                    return this.pvIdioma;
                }
            }

            public string Mensagem
            {
                get
                {
                    return this.pvMensagem;
                }
            }
        }

        /// <summary>
        ///   Array de uma mesma mensagem em diversos idiomas.
        /// </summary>
        internal class MensagemMultiLang : ArrayList
        {
            internal MensagemMultiLang(params MensagemIdioma[] Mensagens)
            {
                foreach (var m in Mensagens)
                {
                    this.Add(m);
                }
            }

            public string this[string Idioma]
            {
                get
                {
                    foreach (MensagemIdioma m in this)
                    {
                        if (m.Idioma == Idioma)
                        {
                            return m.Mensagem;
                        }
                    }

                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///   Um idioma (pode representar um registro da tabela 'IDIOMA' do Cronos).
        /// </summary>
        internal struct Idioma
        {
            private readonly string pvID;

            private readonly string pvNome;

            public Idioma(string IdiomaID, string Nome)
            {
                this.pvID = IdiomaID;
                this.pvNome = Nome;
            }

            public string IdiomaID
            {
                get
                {
                    return this.pvID;
                }
            }

            public string Nome
            {
                get
                {
                    return this.pvNome;
                }
            }
        }

        /// <summary>
        ///   Collection de idiomas (obtido da tabela 'IDIOMA' do Cronos).
        /// </summary>
        internal class Idiomas : ArrayList
        {
            public Idiomas(DataTable IdiomasTable)
            {
                foreach (DataRow r in IdiomasTable.Rows)
                {
                    this.Add(new Idioma(r["idioma"].ToString(), r["nome"].ToString()));
                }
            }
        }
    }
}