using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using System.IO;
using NPOI.SS.Util;

namespace Techne.Lyceum.RN.Util
{
    public class ExportaExcel
    {
        public void ExportaDataTablePor(DataTable consulta, string titulo, string nomeArquivo)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            //Cria a planilha com os dados da consulta
            workbook = this.GeraPlanilhaPor(consulta, titulo, nomeArquivo);

            HSSFWorkbook planilha = workbook;
            HttpResponse response = HttpContext.Current.Response;
            MemoryStream memoryStream = new MemoryStream();

            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", titulo));

            response.Clear();
            planilha.Write(memoryStream);
            response.BinaryWrite(memoryStream.ToArray());
        }

        public void ExportaDataTablePor_ColunasComCaption(DataTable consulta, string titulo, string nomeArquivo)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            //Cria a planilha com os dados da consulta
            workbook = this.GeraPlanilhaPor_ColunasComCaption(consulta, titulo, nomeArquivo);

            HSSFWorkbook planilha = workbook;
            HttpResponse response = HttpContext.Current.Response;
            MemoryStream memoryStream = new MemoryStream();

            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", titulo));

            response.Clear();
            planilha.Write(memoryStream);
            response.BinaryWrite(memoryStream.ToArray());
        }

        private HSSFWorkbook GeraPlanilhaPor(DataTable consulta, string tituloPlanilha, string nomeArquivo)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(nomeArquivo);
            NPOI.SS.UserModel.Cell celula;
            int indiceLinha = 0;
            int indiceColuna = 0;
            int quantidadeColunas = 0;

            //Busca quantidade de colunas
            quantidadeColunas = consulta.Columns.Count;

            //Cria linha Titulo
            Row titulo = (HSSFRow)sheet.CreateRow(indiceLinha);
            sheet.AddMergedRegion(new CellRangeAddress(indiceLinha, indiceLinha, 0, (quantidadeColunas - 1)));

            celula = titulo.CreateCell(0);
            celula.SetCellValue(tituloPlanilha.ToUpper());
            celula.CellStyle = this.StyleTitulo(workbook);

            indiceLinha = indiceLinha + 2;

            //Cria linha Cabeçalho
            Row cabecalho = (HSSFRow)sheet.CreateRow(2);

            //Monta colunas do cabecalho
            foreach (DataColumn coluna in consulta.Columns)
            {
                celula = cabecalho.CreateCell(indiceColuna);
                celula.SetCellValue(coluna.ColumnName);
                celula.CellStyle = this.StyleCabecalho(workbook);

                indiceColuna++;
            }

            indiceLinha++;

            //Cria demais linhas
            if (consulta.Rows.Count > 0)
            {
                foreach (DataRow item in consulta.Rows)
                {
                    Row dado = (HSSFRow)sheet.CreateRow(indiceLinha);
                    indiceColuna = 0;

                    foreach (DataColumn coluna in consulta.Columns)
                    {
                        celula = dado.CreateCell(indiceColuna);
                        celula.SetCellValue(Convert.ToString(item[coluna]));
                        //celula.CellStyle = this.StyleDemaisLinhas(workbook);

                        indiceColuna++;
                    }

                    indiceLinha++;
                }
            }

            //Ajusta largura das colunas
            for (int i = 0; i < quantidadeColunas; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            return workbook;
        }

        private HSSFWorkbook GeraPlanilhaPor_ColunasComCaption(DataTable consulta, string tituloPlanilha, string nomeArquivo)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(nomeArquivo);
            NPOI.SS.UserModel.Cell celula;
            int indiceLinha = 0;
            int indiceColuna = 0;
            int quantidadeColunas = 0;

            //Busca quantidade de colunas
            quantidadeColunas = consulta.Columns.Count;

            //Cria linha Titulo
            Row titulo = (HSSFRow)sheet.CreateRow(indiceLinha);
            sheet.AddMergedRegion(new CellRangeAddress(indiceLinha, indiceLinha, 0, (quantidadeColunas - 1)));

            celula = titulo.CreateCell(0);
            celula.SetCellValue(tituloPlanilha.ToUpper());
            celula.CellStyle = this.StyleTitulo(workbook);

            indiceLinha = indiceLinha + 2;

            //Cria linha Cabeçalho
            Row cabecalho = (HSSFRow)sheet.CreateRow(2);

            //Monta colunas do cabecalho
            foreach (DataColumn coluna in consulta.Columns)
            {
                celula = cabecalho.CreateCell(indiceColuna);
                celula.SetCellValue(coluna.Caption);
                celula.CellStyle = this.StyleCabecalho(workbook);

                indiceColuna++;
            }

            indiceLinha++;

            //Cria demais linhas
            if (consulta.Rows.Count > 0)
            {
                foreach (DataRow item in consulta.Rows)
                {
                    Row dado = (HSSFRow)sheet.CreateRow(indiceLinha);
                    indiceColuna = 0;

                    foreach (DataColumn coluna in consulta.Columns)
                    {
                        celula = dado.CreateCell(indiceColuna);
                        celula.SetCellValue(Convert.ToString(item[coluna]));
                        celula.CellStyle = this.StyleDemaisLinhas(workbook);

                        indiceColuna++;
                    }

                    indiceLinha++;
                }
            }

            //Ajusta largura das colunas
            for (int i = 0; i < quantidadeColunas; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            return workbook;
        }

        private CellStyle StyleTitulo(HSSFWorkbook workbook)
        {
            CellStyle style = workbook.CreateCellStyle();
            Font fonte = workbook.CreateFont();

            style.Alignment = HorizontalAlignment.CENTER;
            fonte.Boldweight = short.MaxValue;//Negrito
            style.SetFont(fonte);

            return style;
        }

        private CellStyle StyleCabecalho(HSSFWorkbook workbook)
        {
            CellStyle style = workbook.CreateCellStyle();
            Font fonte = workbook.CreateFont();

            //Bordas
            style.BorderBottom = CellBorderType.THIN;
            style.BorderLeft = CellBorderType.THIN;
            style.BorderRight = CellBorderType.THIN;
            style.BorderTop = CellBorderType.THIN;

            //Fundo
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.GREY_25_PERCENT.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;

            fonte.Boldweight = short.MaxValue;//Negrito
            style.SetFont(fonte);

            return style;
        }

        private CellStyle StyleDemaisLinhas(HSSFWorkbook workbook)
        {
            CellStyle style = workbook.CreateCellStyle();

            //Bordas
            style.BorderBottom = CellBorderType.THIN;
            style.BorderLeft = CellBorderType.THIN;
            style.BorderRight = CellBorderType.THIN;
            style.BorderTop = CellBorderType.THIN;

            return style;
        }
    }
}
