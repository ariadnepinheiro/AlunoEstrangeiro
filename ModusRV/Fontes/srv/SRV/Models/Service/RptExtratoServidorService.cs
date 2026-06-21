using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Common;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SRV.Models.Domain;
using SRV.Common.Exceptions;

namespace SRV.Models.Service
{
    public class RptExtratoServidorService : BaseService
    {
        public RptExtratoServidor Find(int? idRegional, int MatServidor, int Ciclo)
        {
            RptExtratoServidor rptExtratoServidor;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                RptExtratoServidorMapper mapper = new RptExtratoServidorMapper();
                mapper.connection = conn;
                
                rptExtratoServidor = mapper.Find(MatServidor, Ciclo);

                if (rptExtratoServidor == null)
                    throw new BusinessException("Servidor não encontrado");

                if (rptExtratoServidor.ElegivelUnidade)
                {
                    RptRegionalExtratoServidorMapper rptRegionalExtratoServidorMapper = new RptRegionalExtratoServidorMapper();
                    rptRegionalExtratoServidorMapper.connection = conn;

                    rptExtratoServidor.Unidades = rptRegionalExtratoServidorMapper.List(idRegional, MatServidor, Ciclo);
                }
                else
                    return rptExtratoServidor;                                        

                foreach (var unidade in rptExtratoServidor.Unidades)
                {
                    rptExtratoServidor.Resultado += unidade.Resultado;
                }

                rptExtratoServidor.IdAnoReferencia = Ciclo;

            }

            return rptExtratoServidor;
        }

        public byte[] GeraExcel(string pathTemplate, int? idRegional, int MatServidor, int Ciclo)
        {
            byte[] result = new byte[] { };

            RptExtratoServidor rptExtratoServidor;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                RptExtratoServidorMapper mapper = new RptExtratoServidorMapper();
                mapper.connection = conn;

                rptExtratoServidor = mapper.Find(MatServidor, Ciclo);

                if (rptExtratoServidor.ElegivelUnidade)
                {
                    RptRegionalExtratoServidorMapper rptRegionalExtratoServidorMapper = new RptRegionalExtratoServidorMapper();
                    rptRegionalExtratoServidorMapper.connection = conn;

                    rptExtratoServidor.Unidades = rptRegionalExtratoServidorMapper.List(idRegional, MatServidor, Ciclo);

                    foreach (var unidade in rptExtratoServidor.Unidades)
                    {
                        rptExtratoServidor.Resultado += unidade.Resultado;
                    }
                }
                
                rptExtratoServidor.IdAnoReferencia = Ciclo;
                rptExtratoServidor.IdRegional = idRegional;
            }

            // Gera o relatório em Excel
            int sheetNumber = 1;
            int rowData = 10;

            using (FileStream fs = new FileStream(pathTemplate, FileMode.Open, FileAccess.Read))
            {
                using (HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true))
                {
                    HSSFSheet sheet = (HSSFSheet)templateWorkbook.GetSheet("Extrato de RV");

                    //Estilo da linha
                    CellStyle cellStyleLinha = sheet.Workbook.CreateCellStyle();
                    cellStyleLinha.WrapText = true;
                    cellStyleLinha.BorderTop = CellBorderType.THIN;
                    cellStyleLinha.BorderLeft = CellBorderType.THIN;
                    cellStyleLinha.BorderBottom = CellBorderType.THIN;
                    cellStyleLinha.BorderRight = CellBorderType.THIN;
                    cellStyleLinha.TopBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinha.LeftBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinha.BottomBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinha.RightBorderColor = IndexedColors.GREY_25_PERCENT.Index;

                    //Estilo da linha decimal
                    CellStyle cellStyleLinhaDecimal = sheet.Workbook.CreateCellStyle();
                    cellStyleLinhaDecimal.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat(" 0.00");
                    cellStyleLinhaDecimal.WrapText = true;
                    cellStyleLinhaDecimal.BorderTop = CellBorderType.THIN;
                    cellStyleLinhaDecimal.BorderLeft = CellBorderType.THIN;
                    cellStyleLinhaDecimal.BorderBottom = CellBorderType.THIN;
                    cellStyleLinhaDecimal.BorderRight = CellBorderType.THIN;
                    cellStyleLinhaDecimal.TopBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaDecimal.LeftBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaDecimal.BottomBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaDecimal.RightBorderColor = IndexedColors.GREY_25_PERCENT.Index;

                    //Fonte célula elegível
                    Font fontElegivel = sheet.Workbook.CreateFont();
                    fontElegivel.FontName = "Wingdings 2";
                    fontElegivel.FontHeightInPoints = 14;

                    //Estilo da linha elegível
                    CellStyle cellStyleLinhaElegivel = sheet.Workbook.CreateCellStyle();
                    cellStyleLinhaElegivel.WrapText = true;
                    cellStyleLinhaElegivel.BorderTop = CellBorderType.THIN;
                    cellStyleLinhaElegivel.BorderLeft = CellBorderType.THIN;
                    cellStyleLinhaElegivel.BorderBottom = CellBorderType.THIN;
                    cellStyleLinhaElegivel.BorderRight = CellBorderType.THIN;
                    cellStyleLinhaElegivel.TopBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaElegivel.LeftBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaElegivel.BottomBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaElegivel.RightBorderColor = IndexedColors.GREY_25_PERCENT.Index;
                    cellStyleLinhaElegivel.SetFont(fontElegivel);
                    cellStyleLinhaElegivel.Alignment = HorizontalAlignment.CENTER;
                    cellStyleLinhaElegivel.VerticalAlignment = VerticalAlignment.CENTER;


                    ImprimeDadosServidor(rptExtratoServidor, ref sheetNumber, templateWorkbook, ref sheet, cellStyleLinha);

                    if (rptExtratoServidor.ElegivelUnidade)
                    {
                        foreach (RptRegionalExtratoServidor item in rptExtratoServidor.Unidades)
                        {
                            ImprimeLinhaRpt(item, ref rowData, ref sheetNumber, templateWorkbook, ref sheet, cellStyleLinha, cellStyleLinhaElegivel, cellStyleLinhaDecimal);
                        }
                    }

                    ImprimeRodape(rptExtratoServidor, ref rowData, ref sheetNumber, templateWorkbook, ref sheet);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        templateWorkbook.Write(ms);
                        result = ms.ToArray();
                    }
                }
            }            
            return result;
        }

        private void ImprimeDadosServidor(RptExtratoServidor rptExtratoServidor, ref int sheetNumber, HSSFWorkbook templateWorkbook, ref HSSFSheet sheet, CellStyle cellStyleLinha)
        {
            int column = Constants.column_data;

            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(5);

            HSSFCell cell;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptExtratoServidor.MatriculaServidor);

            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptExtratoServidor.Nome);

            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptExtratoServidor.CPF);

            HSSFRow dataRowMsg = (HSSFRow)sheet.CreateRow(7);

            HSSFCell cellResultado;

            if (!rptExtratoServidor.ElegivelUnidade)
            {
                //Fonte vermelha
                Font fontMsg = sheet.Workbook.CreateFont();
                fontMsg.Color = IndexedColors.RED.Index;
                fontMsg.Boldweight = 700;

                cellResultado = (HSSFCell)dataRowMsg.CreateCell(Constants.column_data);
                cellResultado.SetCellType(CellType.STRING);
                cellResultado.SetCellValue("Servidor Inelegível - SOMA dos períodos de lotação inferior a meta definida para o ano de referência.");
                cellResultado.CellStyle.SetFont(fontMsg);

            }
        }

        private void ImprimeLinhaRpt(RptRegionalExtratoServidor rptRegionalExtratoServidor, ref int row, ref int sheetNumber, HSSFWorkbook templateWorkbook, ref HSSFSheet sheet, CellStyle cellStyleLinha, CellStyle cellStyleLinhaElegivel, CellStyle cellStyleLinhaDecimal)
        {
            int column = Constants.column_data;

            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(row);

            HSSFCell cell;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptRegionalExtratoServidor.Regional);
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptRegionalExtratoServidor.Unidade);
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptRegionalExtratoServidor.FuncaoAtividade);
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptRegionalExtratoServidor.FuncaoBonificacao);
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinhaDecimal;
            cell.SetCellValue(decimal.ToDouble(decimal.Round(rptRegionalExtratoServidor.Proporcionalidade, 2)));
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = cellStyleLinha;
            cell.SetCellValue(rptRegionalExtratoServidor.Periodo);
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinhaDecimal;
            cell.SetCellValue(decimal.ToDouble(decimal.Round(rptRegionalExtratoServidor.CoeficienteBonificacao, 2)));
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinhaDecimal;
            if(rptRegionalExtratoServidor.Alocacao > 0)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(rptRegionalExtratoServidor.Alocacao.Value, 2)));
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = cellStyleLinhaElegivel;
            cell.SetCellValue(rptRegionalExtratoServidor.Eligibilidade ? "R" : "T");
            column++;

            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = cellStyleLinhaDecimal;
            cell.SetCellValue(decimal.ToDouble(decimal.Round(rptRegionalExtratoServidor.Resultado, 2)));

            row++;
        }

        private void ImprimeRodape(RptExtratoServidor rptExtratoServidor, ref int row, ref int sheetNumber, HSSFWorkbook templateWorkbook, ref HSSFSheet sheet)
        {
            int column = Constants.column_data;

            HSSFRow dataRowRodape = (HSSFRow)sheet.CreateRow(row);

            //Fonte Negrito
            Font fontRodape = sheet.Workbook.CreateFont();
            fontRodape.Color = IndexedColors.WHITE.Index;
            fontRodape.Boldweight = 700;

            CellStyle cellStyleRodape = sheet.Workbook.CreateCellStyle();
            cellStyleRodape.FillForegroundColor = IndexedColors.GREY_50_PERCENT.Index;
            cellStyleRodape.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyleRodape.SetFont(fontRodape);

            HSSFCell cellRodape;
            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.STRING);
            cellRodape.CellStyle = cellStyleRodape;
            cellRodape.SetCellValue("Total");
            column++;

            cellRodape = (HSSFCell)dataRowRodape.CreateCell(column);
            cellRodape.SetCellType(CellType.NUMERIC);
            cellRodape.CellStyle = cellStyleRodape;
            cellRodape.CellStyle.DataFormat = sheet.Workbook.CreateDataFormat().GetFormat(" 0.00");
            cellRodape.SetCellValue(decimal.ToDouble(decimal.Round(rptExtratoServidor.Resultado, 2)));
        }
    }
}