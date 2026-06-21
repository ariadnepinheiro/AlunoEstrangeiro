using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SRV.Common.Exceptions;
using System.Text;

namespace SRV.Common.Loader
{
    public class CsvFileLoader<EntityClass> where EntityClass : class, new()
    {
        public string FileName { get; set; }

        /// <summary>
        /// The list of failed records
        /// </summary>
        public List<string> ErrorRecords { get; set; }

        public bool HasError { get; set; }

        public CsvFileLoader(string fileName)
        {
            this.FileName = fileName;
            this.ErrorRecords = new List<string>();
        }

        /// <summary>
        /// Imports the CSV record and returns a list of objects
        /// </summary>
        /// <returns></returns>
        public List<EntityClass> Import()
        {
            string recordData;
            string[] dataElements;
            EntityClass fileRecord;
            List<EntityClass> result = new List<EntityClass>();

            ImportFileAttribute importFile = ReflectionHelper.GetImportFileAttribute(new EntityClass());		

			string[] linhas = File.ReadAllLines(this.FileName);
			if (linhas.Distinct().Count() != linhas.Count())
			{
				string erroMsg;
				var itens = linhas.Select((item, index) => new { Linha = index + 1, Valor = item }).GroupBy(p => p.Valor);

				foreach (var item in itens.Where(i => i.Count() > 1))
				{
					erroMsg = "Registro duplicado - Linhas: ";

					foreach (var i in item)
					{
						erroMsg += i.Linha + "; ";						
					}
					ErrorRecords.Add(erroMsg);
				}
			}
			else
			{
				linhas = null;
				StreamReader streamReader = new StreamReader(this.FileName, Encoding.Default);
				int line = 1;
				while (!streamReader.EndOfStream)
				{
					// Read in a line of the record data
					recordData = streamReader.ReadLine();

					try
					{
						// Remove trailing delimiter
						//recordData = recordData.TrimEnd(importFile.FieldDelimiter.ToCharArray());

						// Split the record data based on the column delimiter
						dataElements = recordData.Split(importFile.FieldDelimiter.ToCharArray());

						if (dataElements.Length != importFile.Columns)
						{
							throw new FieldValidationException("Quantidade de campos inválida");
						}

						// Populate the record data elements into the object
						// and add it to the list
						fileRecord = new EntityClass();

						// For every data elements we find
						for (int i = 0; i < dataElements.Length; i++)
						{
							ReflectionHelper.SetPropertyValue(fileRecord, dataElements[i], i);
						}

						result.Add(fileRecord);
					}
					catch (FieldValidationException e)
					{
						ErrorRecords.Add(String.Format("Linha {0}: {1}", line, e.Message));
					}

					line++;
				}

				streamReader.Close();				
			}

            if (ErrorRecords.Count > 0)
            {
                HasError = true;
            }

            return result;
        }
    }
}