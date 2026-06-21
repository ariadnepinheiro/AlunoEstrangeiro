using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;

namespace Techne.Lyceum.RN.Util.ImportacaoArquivo
{
    public class ImportaArquivoCsv<ClasseEntidadeImportacao> where ClasseEntidadeImportacao : class, new()
    {
        public Stream StreamArquivo { get; set; }

        /// <summary>
        /// Lista de Erros
        /// </summary>
        public List<string> ErroRegistros { get; set; }

        /// <summary>
        /// Erro de validação do arquivo que finaliza a rotina de importação
        /// </summary>
        public string ErroSistema { get; set; }

        public bool TemErro { get; set; }

        public ImportaArquivoCsv(Stream streamArquivo)
        {
            this.StreamArquivo = streamArquivo;
            this.ErroRegistros = new List<string>();
            this.ErroSistema = string.Empty;
        }

        /// <summary>
        /// Imports the CSV record and returns a list of objects
        /// </summary>
        /// <returns></returns>
        public List<ClasseEntidadeImportacao> Importar()
        {
            string linhaRegistro;
            string[] elementosDados;
            ClasseEntidadeImportacao registroEntidade;
            List<ClasseEntidadeImportacao> listaRegistros = new List<ClasseEntidadeImportacao>();

            ImportaAtributoArquivo importaArquivo = ReflectionHelper.CarregaAtributosImportacaoArquivo(new ClasseEntidadeImportacao());

            StreamReader streamReader = new StreamReader(this.StreamArquivo, Encoding.Default);

            List<PropriedadeAtributoCampo> listAtributos = ReflectionHelper.GetListProperties(new ClasseEntidadeImportacao());

            int numeroLinhas = 1;
            while (!streamReader.EndOfStream)
            {
                // Read in a line of the record data
                linhaRegistro = streamReader.ReadLine();

                try
                {
                    // Split the record data based on the column delimiter
                    elementosDados = linhaRegistro.Split(importaArquivo.CaracterDelimitador.ToCharArray());

                    if (elementosDados.Length != importaArquivo.Colunas)
                    {
                        ErroSistema = "Quantidade de campos inválida no arquivo";
                        break;
                    }
                    else
                    {
                        if ((!importaArquivo.CabecalhoPrimeiraLinha) || (importaArquivo.CabecalhoPrimeiraLinha && numeroLinhas != 1))
                        {
                            // Populate the record data elements into the object
                            // and add it to the list
                            registroEntidade = new ClasseEntidadeImportacao();

                            // For every data elements we find
                            for (int i = 0; i < importaArquivo.Colunas; i++)
                            {
                                string Erro = string.Empty;
                                ReflectionHelper.SetPropertyValue(listAtributos, registroEntidade, elementosDados[i], i, ref Erro);
                                if (!string.IsNullOrEmpty(Erro))
                                    AdicionaErros(numeroLinhas, Erro);
                            }

                            listaRegistros.Add(registroEntidade);
                        }
                    }
                }
                catch (Exception e)
                {
                    ErroRegistros.Add(String.Format("Linha {0}: {1}", numeroLinhas, e.Message));
                }

                numeroLinhas++;
            }

            if (ErroRegistros.Count > 0)
            {
                TemErro = true;
            }

            //streamReader.Close();

            return listaRegistros;
        }

        private void AdicionaErros(int numeroLinhas, string erro)
        {
            ErroRegistros.Add(String.Format("Linha {0}: {1}", numeroLinhas, erro));
        }
    }
}