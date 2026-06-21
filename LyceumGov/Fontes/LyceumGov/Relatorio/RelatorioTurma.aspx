<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatorioTurma.aspx.cs"
    Inherits="Techne.Lyceum.Net.Relatorio.RelatorioTurma" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SEEDUC - Governo do Rio de Janeiro - Quadro de Horários</title>
</head>
<link href="../Scripts/themes/basic/grid.css" rel="stylesheet" type="text/css" />
<link href="../Scripts/themes/jqModal.css" rel="stylesheet" type="text/css" />
<link href="../LyceumNet.css" type="text/css" rel="stylesheet" />
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
<style type="text/css">
    td
    {        
        border-bottom-color: inherit;
        border-left-color: #CDDDEE;
        border-right-color: #CDDDEE;
        border-top-color: #CDDDEE;
        border-width: 1.5px;        
    }    
    td.default
    {
        background-color: White;
        border-style: solid;        
        border-width: 2.5px;
        text-align: left;
    }   
    .bordaVermelha
    {
        border-style: solid;
        border-color: Red;
        border-width: 1.8px;
    }
    .bordaBranca
    {
        border-style: solid;
        border-color: White;
        border-width: 1.8px;
    }
    .bordaHeader
    {
        border-width: 0px;
        border-top-width: 5px;
        border-top-color: #6CA6EA;
        border-top-style: solid;
        background-color: #BFD7F3;
        color: Black;
        font-weight: bold;
    }
    .bordaHorario
    {
        border-width: 0px;
        color: Black;
        font-weight: bold;
        text-align: center;
    }
    .bordaFundo1
    {
        background-color: #CDDDEE;
        height: 8px;
        border-width: 0px;
    }
    .bordaFundo2
    {
        background-color: #0353AB;
        height: 2px;
        border-width: 0px;
    }
    .PopUp
    {
        display: none;
        position: absolute;
        width: 100px;
        font-size: 11px;
        font-weight: normal;
        font-family: verdana;
        color: #E7E6E1;
        background-color: #E7E6E1;
    }
    .PopUpErro
    {
        display: none;
        position: absolute;
        font-size: 11px;
        font-weight: bold;
        font-family: verdana;
        color: Black;
        background-color: #EDF4FF;
    }
    .Imagem
    {
        cursor: pointer;
    }
    .Controle
    {
        cursor: pointer;
        color: Black;
        width: 100%;
        text-decoration: none;
        text-align: left;
    }
    .Controle:hover
    {
        text-decoration: none;
        background-color: #003366;
        color: White;
    }
    .ControleDesabilitado:visited
    {
        cursor: pointer;
        color: Gray;
        width: 100%;
        text-decoration: none;
        text-align: left;
    }
    .ControleDesabilitado:link
    {
        cursor: pointer;
        color: Gray;
        width: 100%;
        text-decoration: none;
        text-align: left;
    }
    .ControleDesabilitado:active
    {
        cursor: pointer;
        color: Gray;
        width: 100%;
        text-decoration: none;
        text-align: left;
    }
    .ControleDesabilitado:hover
    {
        background-color: #003366;
        color: Gray;
        text-decoration: none;
    }
    .textoAzulEscuro
    {
        border: 1px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: #000080;
    }
    .textoAzul
    {
        border: 1px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: #0F6BFF;
    }
    .textoAzulClaro
    {
        border: 1px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: #7FC9FF;
    }
    .textoAmarelo
    {
        border: 1px #ccc solid;
        padding: 1px 3px;
        font-family: Verdana;
        font-size: smaller;
        background-color: #FFFD80;
    }
    .textoAmareloEscuro
    {
        border: 1px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: #FFD147;
    }
    .textoVermelho
    {
        border: 1px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: Red;
    }
    .textoLaranja
    {
        border: 1px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: #D7D7D7;
    }
    .txtInput
    {
        border: 0px #ccc solid;
        font-family: Verdana;
        font-size: smaller;
        background-color: Transparent;
    }
    .ImagemAviso
    {
        cursor: pointer;
    }
</style>
<script type="text/javascript">
    $(document).ready(function() {
        var maxHeight = 0;
        $("tr:has(td:has(input[type=hidden]))").each(function(i) {
            var height = $(this).height();            
            if (height > maxHeight)
                maxHeight = height;
        });        
        $("tr:has(td:has(input[type=hidden]))").each(function(i) {
            $(this).attr("height", maxHeight + 3);
        });
    });
</script>
<body >
    <form id="form1" runat="server" >
    <div>            
        <asp:Label Font-Names="Verdana" ID="lblMensagem" runat="server" SkinID="lblObrigatorio"></asp:Label>
        <asp:ImageButton ID="btnImprimir" runat="server" OnClientClick="javascript:window.print(); return false;"
            SkinID="Imprimir" />
        <table width="80%" >
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblTurma" runat="server" Font-Names="Verdana" Text="Turma:"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblTurmaValor" Text="<turma>" runat="server"></asp:Label>
                </td>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:"></asp:Label>
                </td>
                <td width="35%" >
                    <asp:Label Font-Names="Verdana" ID="lblValorAno" runat="server"></asp:Label>
                </td>                                
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCoordenadoria" runat="server" Font-Names="Verdana" Text="Coordenadoria:"></asp:Label>
                </td>
                <td >
                    <asp:Label Font-Names="Verdana" ID="lblValorCoordenadoria" runat="server"></asp:Label>
                </td>          
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Font-Names="Verdana" Text="Período:" ></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblValorPeriodo" runat="server"></asp:Label>
                </td>      
            </tr>
            <tr>
                <td style="text-align: right; width: 15%; white-space:nowrap">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:"></asp:Label>
                </td>
                <td style="width: 50%">
                    <asp:Label Font-Names="Verdana" ID="lblValorUnidadeResponsavel" runat="server"></asp:Label>
                </td>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblTurno" runat="server" Font-Names="Verdana" Text="Turno:" ></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblTurnoValor" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />        
        <asp:Table ID="tQuadroHorario" runat="server" GridLines="Both" Width="100%" EnableViewState="true" >
            <asp:TableHeaderRow ID="trCabecalho" runat="server">
                <asp:TableHeaderCell CssClass="bordaHeader" VerticalAlign="Middle" Text="Início/Término" Height="25px" Width="10%" ></asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="bordaHeader" Text="Segunda" Width="15%" ></asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="bordaHeader" Text="Terça" Width="15%" ></asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="bordaHeader" Text="Quarta" Width="15%" ></asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="bordaHeader" Text="Quinta" Width="15%" ></asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="bordaHeader" Text="Sexta" Width="15%" ></asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="bordaHeader" Text="Sábado" Width="15%" ></asp:TableHeaderCell>
            </asp:TableHeaderRow>            
        </asp:Table>
    </div>
    </form>
</body>
</html>
