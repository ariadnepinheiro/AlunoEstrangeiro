using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.Data.SqlClient;
using SRV.Models.Mapper;
using SRV.Models.DTO;
using SRV.Common;
using System.Web.Mvc;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace SRV.Models.Service
{
    public class RptConsolidadoService : BaseService
    {
        private readonly ModelStateDictionary modelState;

        public RptConsolidadoService(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        private bool ValidaFiltroconsolidado(FiltroRptConsolidado filtro)
        {

            if (filtro.IdAnoReferencia == null)
                modelState.AddModelError("IdAnoReferencia", "Campo é obrigatório");

            if (filtro.IdRegional == null)
                modelState.AddModelError("IdRegional", "Campo é obrigatório");

            return modelState.IsValid;
        }


        public Dictionary<int, ConsolidadoUnidadeAdm> List(FiltroRptConsolidado filtro, UserState usuario)
        {
            Dictionary<int, ConsolidadoUnidadeAdm> result = null;

            if (ValidaFiltroconsolidado(filtro))
            {
                result = new Dictionary<int,ConsolidadoUnidadeAdm>();

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    UnidadeConsolidadoMapper unidadeMapper = new UnidadeConsolidadoMapper();
                    unidadeMapper.connection = conn;

                    ValorConsolidadoMapper valorMapper = new ValorConsolidadoMapper();
                    valorMapper.connection = conn;

                    IList<UnidadeConsolidado> listUnidade = unidadeMapper.ListUnidade(filtro, usuario);

                    foreach (var unidade in listUnidade)
                    {
                        ConsolidadoUnidadeAdm consolidado = new ConsolidadoUnidadeAdm();

                        consolidado.Unidade = unidade;

                        if (unidade.Regional)
                            consolidado.PercCurriculoMinimoRegional = valorMapper.FindCurriculoMinRegional(filtro);

                        result.Add(unidade.IdUnidade, consolidado);
                    }

                    //PRIMEIRA ABA

                    //EF
                    IList<ValorConsolidado> list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREF1, Constants.IdIndicadorIDERJ);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IderjERFundamental1 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREF2, Constants.IdIndicadorIDERJ);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IderjERFundamental2 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREM, Constants.IdIndicadorIDERJ);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IderjEREnsinoMedio = valor;

                    //EJA
                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeEJA, Constants.IdNivelEnsinoEJAEF2, Constants.IdIndicadorID);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IdEjaFundamental2 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeEJA, Constants.IdNivelEnsinoEJAEM, Constants.IdIndicadorID);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IdEjaEnsinoMedio = valor;

                    //Lanc. notas e Curriculo Min.
                    list = valorMapper.ListLancamentoNotas(filtro, usuario);
                    foreach (var valor in list)
                        result[valor.IdUnidade].PercLancamentoNotas = valor;

                    list = valorMapper.ListCurriculoMinimo(filtro, usuario);
                    foreach (var valor in list)
                        result[valor.IdUnidade].PercCurriculoMinimo = valor;

                    //Avaliação Externa
                    list = valorMapper.ListAvaliacaoExterna(filtro, usuario, Constants.IdAvalExternaSaerjinho1);
                    foreach (var valor in list)
                        result[valor.IdUnidade].Saerjinho1 = valor;

                    list = valorMapper.ListAvaliacaoExterna(filtro, usuario, Constants.IdAvalExternaSaerjinho2);
                    foreach (var valor in list)
                        result[valor.IdUnidade].Saerjinho2 = valor;

                    list = valorMapper.ListAvaliacaoExterna(filtro, usuario, Constants.IdAvalExternaSaerjinho3);
                    foreach (var valor in list)
                        result[valor.IdUnidade].Saerjinho3 = valor;

                    list = valorMapper.ListAvaliacaoExterna(filtro, usuario, Constants.IdAvalExternaSaerj);
                    foreach (var valor in list)
                        result[valor.IdUnidade].Saerj = valor;


                    //SEGUNDA ABA

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREF1, Constants.IdIndicadorID);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IdERFundamental1 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREF1, Constants.IdIndicadorIF);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IfERFundamental1 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREF2, Constants.IdIndicadorID);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IdERFundamental2 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREF2, Constants.IdIndicadorIF);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IfERFundamental2 = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREM, Constants.IdIndicadorID);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IdEREnsinoMedio = valor;

                    list = valorMapper.ListIndicadores(filtro, usuario, Constants.IdModalidadeER, Constants.IdNivelEnsinoEREM, Constants.IdIndicadorIF);
                    foreach (var valor in list)
                        result[valor.IdUnidade].IfEREnsinoMedio = valor;

                    //TERCEIRA ABA

                    list = valorMapper.ListIGE(filtro, usuario);
                    foreach (var valor in list)
                        result[valor.IdUnidade].Ige = valor;

                }
            }

            return result;
        }

        public byte[] GeraExcel(string pathTemplate, FiltroRptConsolidado filtro, UserState usuario)
        {
            byte[] result = new byte[] { };

            if (ValidaFiltroconsolidado(filtro))
            {
                const string sheetName1 = "Elegibilidade de Unidades Admin";
                const string sheetName2 = "Ensino Regular";
                const string sheetName3 = "EJA Presencial";

                Dictionary<int, ConsolidadoUnidadeAdm> listResult;

                // Carrega os dados do relatório
                listResult = List(filtro, usuario);

                // Gera o relatório em Excel
                int sheetNumber1 = 1;
                int sheetNumber2 = 2;
                int sheetNumber3 = 3;

                int rowData = Constants.row_data;

                using (FileStream fs = new FileStream(pathTemplate, FileMode.Open, FileAccess.Read))
                {
                    using (HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true))
                    {
                        HSSFSheet sheet1 = (HSSFSheet)templateWorkbook.GetSheet(sheetName1);
                        HSSFSheet sheet2 = (HSSFSheet)templateWorkbook.GetSheet(sheetName2);
                        HSSFSheet sheet3 = (HSSFSheet)templateWorkbook.GetSheet(sheetName3);

                        //Fonte célula elegível
                        Font fontElegivel = sheet1.Workbook.CreateFont();
                        fontElegivel.FontName = "Wingdings 2";
                        fontElegivel.FontHeightInPoints = 14;

                        //Estilo célula unidade administrativa
                        CellStyle cellStyleUnidadeAdm = sheet1.Workbook.CreateCellStyle();
                        cellStyleUnidadeAdm.WrapText = true;
                        cellStyleUnidadeAdm.BorderTop = CellBorderType.THIN;
                        cellStyleUnidadeAdm.BorderLeft = CellBorderType.THIN;
                        cellStyleUnidadeAdm.BorderBottom = CellBorderType.THIN;
                        cellStyleUnidadeAdm.BorderRight = CellBorderType.THIN;
                        cellStyleUnidadeAdm.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleUnidadeAdm.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleUnidadeAdm.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleUnidadeAdm.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;

                        //Estilo célula unidade administrativa quebra
                        CellStyle cellStyleUnidadeAdmQuebra = sheet1.Workbook.CreateCellStyle();
                        cellStyleUnidadeAdmQuebra.FillForegroundColor = IndexedColors.GREY_25_PERCENT.Index;
                        cellStyleUnidadeAdmQuebra.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        cellStyleUnidadeAdmQuebra.WrapText = true;
                        cellStyleUnidadeAdmQuebra.BorderTop = CellBorderType.THIN;
                        cellStyleUnidadeAdmQuebra.BorderLeft = CellBorderType.THIN;
                        cellStyleUnidadeAdmQuebra.BorderBottom = CellBorderType.THIN;
                        cellStyleUnidadeAdmQuebra.BorderRight = CellBorderType.THIN;
                        cellStyleUnidadeAdmQuebra.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleUnidadeAdmQuebra.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleUnidadeAdmQuebra.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleUnidadeAdmQuebra.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;

                        //Estilo célula elegível
                        CellStyle cellStyleElegivel = sheet1.Workbook.CreateCellStyle();
                        cellStyleElegivel.WrapText = true;
                        cellStyleElegivel.BorderTop = CellBorderType.THIN;
                        cellStyleElegivel.BorderLeft = CellBorderType.THIN;
                        cellStyleElegivel.BorderBottom = CellBorderType.THIN;
                        cellStyleElegivel.BorderRight = CellBorderType.THIN;
                        cellStyleElegivel.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivel.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivel.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivel.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivel.Alignment = HorizontalAlignment.CENTER;
                        cellStyleElegivel.VerticalAlignment = VerticalAlignment.CENTER;
                        cellStyleElegivel.SetFont(fontElegivel);


                        //Estilo célula elegível quebra
                        CellStyle cellStyleElegivelQuebra = sheet1.Workbook.CreateCellStyle();
                        cellStyleElegivelQuebra.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        cellStyleElegivelQuebra.FillForegroundColor = IndexedColors.GREY_25_PERCENT.Index;
                        cellStyleElegivelQuebra.WrapText = true;
                        cellStyleElegivelQuebra.BorderTop = CellBorderType.THIN;
                        cellStyleElegivelQuebra.BorderLeft = CellBorderType.THIN;
                        cellStyleElegivelQuebra.BorderBottom = CellBorderType.THIN;
                        cellStyleElegivelQuebra.BorderRight = CellBorderType.THIN;
                        cellStyleElegivelQuebra.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivelQuebra.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivelQuebra.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivelQuebra.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleElegivelQuebra.Alignment = HorizontalAlignment.CENTER;
                        cellStyleElegivelQuebra.VerticalAlignment = VerticalAlignment.CENTER;
                        cellStyleElegivelQuebra.SetFont(fontElegivel);

                        //Estilo célula quebra
                        CellStyle cellStyleQuebra = sheet1.Workbook.CreateCellStyle();
                        cellStyleQuebra.FillForegroundColor = IndexedColors.GREY_25_PERCENT.Index;
                        cellStyleQuebra.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        cellStyleQuebra.DataFormat = sheet1.Workbook.CreateDataFormat().GetFormat(" 0.00");
                        cellStyleQuebra.WrapText = true;
                        cellStyleQuebra.BorderTop = CellBorderType.THIN;
                        cellStyleQuebra.BorderLeft = CellBorderType.THIN;
                        cellStyleQuebra.BorderBottom = CellBorderType.THIN;
                        cellStyleQuebra.BorderRight = CellBorderType.THIN;
                        cellStyleQuebra.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleQuebra.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleQuebra.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleQuebra.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;

                        //Estilo célula amarela
                        CellStyle cellStyleYellow = sheet1.Workbook.CreateCellStyle();
                        cellStyleYellow.FillForegroundColor = IndexedColors.LIGHT_YELLOW.Index;
                        cellStyleYellow.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        cellStyleYellow.DataFormat = sheet1.Workbook.CreateDataFormat().GetFormat(" 0.00");
                        cellStyleYellow.WrapText = true;
                        cellStyleYellow.BorderTop = CellBorderType.THIN;
                        cellStyleYellow.BorderLeft = CellBorderType.THIN;
                        cellStyleYellow.BorderBottom = CellBorderType.THIN;
                        cellStyleYellow.BorderRight = CellBorderType.THIN;
                        cellStyleYellow.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleYellow.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleYellow.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleYellow.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;

                        //Estilo célula azul
                        CellStyle cellStyleBlue = sheet1.Workbook.CreateCellStyle();
                        cellStyleBlue.FillForegroundColor = IndexedColors.PALE_BLUE.Index;
                        cellStyleBlue.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        cellStyleBlue.DataFormat = sheet1.Workbook.CreateDataFormat().GetFormat(" 0.00");
                        cellStyleBlue.WrapText = true;
                        cellStyleBlue.BorderTop = CellBorderType.THIN;
                        cellStyleBlue.BorderLeft = CellBorderType.THIN;
                        cellStyleBlue.BorderBottom = CellBorderType.THIN;
                        cellStyleBlue.BorderRight = CellBorderType.THIN;
                        cellStyleBlue.TopBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleBlue.LeftBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleBlue.BottomBorderColor = IndexedColors.GREY_40_PERCENT.Index;
                        cellStyleBlue.RightBorderColor = IndexedColors.GREY_40_PERCENT.Index;

                        foreach(KeyValuePair<int, ConsolidadoUnidadeAdm> item in listResult)
                        {
                            ImprimeLinhaRptAbaElegibilidade(item.Value, rowData, sheetNumber1, templateWorkbook, sheet1, cellStyleUnidadeAdm, cellStyleUnidadeAdmQuebra, cellStyleElegivel, cellStyleElegivelQuebra, cellStyleQuebra, cellStyleYellow);
                            ImprimeLinhaRptAbaEnsinoRegular(item.Value, rowData, sheetNumber2, templateWorkbook, sheet2, cellStyleUnidadeAdm, cellStyleUnidadeAdmQuebra, cellStyleElegivel, cellStyleElegivelQuebra, cellStyleQuebra, cellStyleYellow, cellStyleBlue);
                            ImprimeLinhaRptAbaEja(item.Value, rowData, sheetNumber3, templateWorkbook, sheet3, cellStyleUnidadeAdm, cellStyleUnidadeAdmQuebra, cellStyleElegivel, cellStyleElegivelQuebra, cellStyleQuebra, cellStyleYellow, cellStyleBlue);

                            rowData++;
                        }

                        using (MemoryStream ms = new MemoryStream())
                        {
                            templateWorkbook.Write(ms);
                            result = ms.ToArray();
                        }
                    }
                }
            }

            return result;
        }

        private void ImprimeLinhaRptAbaElegibilidade(ConsolidadoUnidadeAdm item, int rowData, int sheetNumber, HSSFWorkbook templateWorkbook, HSSFSheet sheet, CellStyle cellStyleUnidadeAdm, CellStyle cellStyleUnidadeAdmQuebra, CellStyle cellStyleElegivel, CellStyle cellStyleElegivelQuebra, CellStyle cellStyleQuebra, CellStyle cellStyleYellow)
        {
            int column = Constants.column_data;

            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowData);

            //IDUNIDADE
            HSSFCell cell;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleUnidadeAdmQuebra : cellStyleUnidadeAdm;
            cell.SetCellValue(item.Unidade.IdUnidade);

            //NOME UNIDADE ADMINISTRATIVA
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = item.Unidade.Regional ? cellStyleUnidadeAdmQuebra : cellStyleUnidadeAdm;
            cell.SetCellValue(item.Unidade.DesUnidade);

            //ELEGÍVEL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = item.Unidade.Regional ? cellStyleElegivelQuebra : cellStyleElegivel;
            cell.SetCellValue(item.Unidade.Elegivel ? "R" : "T");


            //IDERJ ER FUNDAMENTAL 1

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if(item.IderjERFundamental1.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjERFundamental1.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjERFundamental1.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjERFundamental1.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjERFundamental1.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjERFundamental1.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //IDERJ ER FUNDAMENTAL 2

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjERFundamental2.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjERFundamental2.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjERFundamental2.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjERFundamental2.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjERFundamental2.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjERFundamental2.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //IDERJ ENSINO MÉDIO

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjEREnsinoMedio.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjEREnsinoMedio.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjEREnsinoMedio.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjEREnsinoMedio.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IderjEREnsinoMedio.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IderjEREnsinoMedio.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //ID EJA FUNDAMENTAL 2

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaFundamental2.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaFundamental2.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaFundamental2.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaFundamental2.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaFundamental2.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaFundamental2.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //ID EJA ENSINO MÉDIO

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaEnsinoMedio.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaEnsinoMedio.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaEnsinoMedio.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaEnsinoMedio.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaEnsinoMedio.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaEnsinoMedio.Percentual.Value, 2)));
            else
                cell.SetCellValue("");

            //% LANÇAMENTO DE NOTAS
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.PercLancamentoNotas.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.PercLancamentoNotas.Percentual.Value, 2)));
            else
                cell.SetCellValue("");

            //% CURRÍCULO MÍNIMO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.Unidade.Regional)
            {
                if (item.PercCurriculoMinimoRegional.Percentual != null)
                    cell.SetCellValue(decimal.ToDouble(decimal.Round(item.PercCurriculoMinimoRegional.Percentual.Value, 2)));
                else
                    cell.SetCellValue("");
            }
            else
            {
                if (item.PercCurriculoMinimo.Percentual != null)
                    cell.SetCellValue(decimal.ToDouble(decimal.Round(item.PercCurriculoMinimo.Percentual.Value, 2)));
                else
                    cell.SetCellValue("");
            }

            //SAERJINHO1
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.Saerjinho1.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Saerjinho1.Percentual.Value, 2)));
            else
                cell.SetCellValue("");

            //SAERJINHO2
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.Saerjinho2.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Saerjinho2.Percentual.Value, 2)));
            else
                cell.SetCellValue("");

            //SAERJINHO3
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.Saerjinho3.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Saerjinho3.Percentual.Value, 2)));
            else
                cell.SetCellValue("");

            //SAERJ
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.Saerj.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Saerj.Percentual.Value, 2)));
            else
                cell.SetCellValue("");
        }

        private void ImprimeLinhaRptAbaEnsinoRegular(ConsolidadoUnidadeAdm item, int rowData, int sheetNumber, HSSFWorkbook templateWorkbook, HSSFSheet sheet, CellStyle cellStyleUnidadeAdm, CellStyle cellStyleUnidadeAdmQuebra, CellStyle cellStyleElegivel, CellStyle cellStyleElegivelQuebra, CellStyle cellStyleQuebra, CellStyle cellStyleYellow, CellStyle cellStyleBlue)
        {
            int column = Constants.column_data;

            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowData);

            //IDUNIDADE
            HSSFCell cell;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleUnidadeAdmQuebra : cellStyleUnidadeAdm;
            cell.SetCellValue(item.Unidade.IdUnidade);

            //NOME UNIDADE ADMINISTRATIVA
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = item.Unidade.Regional ? cellStyleUnidadeAdmQuebra : cellStyleUnidadeAdm;
            cell.SetCellValue(item.Unidade.DesUnidade);

            //ELEGÍVEL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = item.Unidade.Regional ? cellStyleElegivelQuebra : cellStyleElegivel;
            cell.SetCellValue(item.Unidade.Elegivel ? "R" : "T");


            //ID FUNDAMENTAL 1

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdERFundamental1.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdERFundamental1.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdERFundamental1.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdERFundamental1.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdERFundamental1.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdERFundamental1.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //IF FUNDAMENTAL 1

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IfERFundamental1.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfERFundamental1.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IfERFundamental1.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfERFundamental1.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IfERFundamental1.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfERFundamental1.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //ID FUNDAMENTAL 2

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdERFundamental2.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdERFundamental2.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdERFundamental2.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdERFundamental2.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdERFundamental2.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdERFundamental2.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //IF FUNDAMENTAL 2

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IfERFundamental2.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfERFundamental2.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IfERFundamental2.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfERFundamental2.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IfERFundamental2.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfERFundamental2.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //ID ENSINO MÉDIO

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdEREnsinoMedio.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEREnsinoMedio.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdEREnsinoMedio.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEREnsinoMedio.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdEREnsinoMedio.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEREnsinoMedio.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //IF ENSINO MÉDIO

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IfEREnsinoMedio.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfEREnsinoMedio.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IfEREnsinoMedio.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfEREnsinoMedio.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IfEREnsinoMedio.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IfEREnsinoMedio.Percentual.Value, 2)));
            else
                cell.SetCellValue("");
        }

        private void ImprimeLinhaRptAbaEja(ConsolidadoUnidadeAdm item, int rowData, int sheetNumber, HSSFWorkbook templateWorkbook, HSSFSheet sheet, CellStyle cellStyleUnidadeAdm, CellStyle cellStyleUnidadeAdmQuebra, CellStyle cellStyleElegivel, CellStyle cellStyleElegivelQuebra, CellStyle cellStyleQuebra, CellStyle cellStyleYellow, CellStyle cellStyleBlue)
        {
            int column = Constants.column_data;

            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowData);

            //IDUNIDADE
            HSSFCell cell;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleUnidadeAdmQuebra : cellStyleUnidadeAdm;
            cell.SetCellValue(item.Unidade.IdUnidade);

            //NOME UNIDADE ADMINISTRATIVA
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = item.Unidade.Regional ? cellStyleUnidadeAdmQuebra : cellStyleUnidadeAdm;
            cell.SetCellValue(item.Unidade.DesUnidade);

            //ELEGÍVEL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.STRING);
            cell.CellStyle = item.Unidade.Regional ? cellStyleElegivelQuebra : cellStyleElegivel;
            cell.SetCellValue(item.Unidade.Elegivel ? "R" : "T");


            //ID FUNDAMENTAL 2

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdEjaFundamental2.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaFundamental2.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdEjaFundamental2.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaFundamental2.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.IdEjaFundamental2.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaFundamental2.Percentual.Value, 2)));
            else
                cell.SetCellValue("");

            //ID Ensino Médio

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaEnsinoMedio.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaEnsinoMedio.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaEnsinoMedio.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaEnsinoMedio.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleYellow;

            if (item.IdEjaEnsinoMedio.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.IdEjaEnsinoMedio.Percentual.Value, 2)));
            else
                cell.SetCellValue("");


            //IGE

            //PREVISTO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.Ige.Previsto != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Ige.Previsto.Value, 2)));
            else
                cell.SetCellValue("");

            //REALIZADO
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.Ige.Realizado != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Ige.Realizado.Value, 2)));
            else
                cell.SetCellValue("");

            //PERCENTUAL
            column++;
            cell = (HSSFCell)dataRow.CreateCell(column);
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle = item.Unidade.Regional ? cellStyleQuebra : cellStyleBlue;

            if (item.Ige.Percentual != null)
                cell.SetCellValue(decimal.ToDouble(decimal.Round(item.Ige.Percentual.Value, 2)));
            else
                cell.SetCellValue("");
        }
    }
}