<%@ Page Language="C#" MasterPageFile="~/Modulos/PublicMaster.Master" AutoEventWireup="true"
    CodeBehind="Confirmacao.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivoAluno.Confirmacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 904px;
        }
        .fonteNegrito
        {
            font-family: Verdana;
            color: #000000;
            font-size: 12px;
            font-weight: bold;
        }
        .fonteNormal
        {
            font-family: Verdana;
            color: #000000;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function printPage() {
            var printElement = document.getElementById("divForm");
            var frame1 = $('<iframe />');
            frame1[0].name = "frame1";
            frame1.css({ "position": "absolute", "top": "-1000000px" });
            $("body").append(frame1);
            var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
            frameDoc.document.open();
            //Create a new HTML document.
            frameDoc.document.write('<html><head><title></title>');
            frameDoc.document.write('</head><body>');
            //Append the external CSS file.
            //frameDoc.document.write('<link href="../Styles/BoletimImpressao.css" rel="stylesheet" type="text/css" />');
            //Append the DIV contents.
            frameDoc.document.write(printElement.innerHTML);
            frameDoc.document.write('</body></html>');
            frameDoc.document.close();
            setTimeout(function() {
                window.frames["frame1"].focus();
                window.frames["frame1"].print();
                frame1.remove();
            }, 500);
        };

        
    </script>

    <div id="divForm">
        <asp:Panel ID="pnlForm" runat="server" BorderWidth="1" Width="900px">
            <br />
            <br />
            <center>
                <div style="font-family: Verdana; color: #000000; font-size: large; font-weight: bold"
                    class="style1">
                    Confirmação de Inscrição Processo Seletivo
                    <asp:Label ID="lblNumeroProcessoSeletivo" runat="server" Style="font-family: Verdana;
                        color: #000000; font-size: large; font-weight: bold"></asp:Label>
                </div>
            </center>
            <br />
            <br />
            <table cellpadding="8" cellspacing="15">
                <tr>
                    <th align="left" class="fonteNegrito">
                        Número de inscrição:
                    </th>
                    <td>
                        <asp:Label ID="lblNumeroInscricao" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Nome do candidato:
                    </th>
                    <td>
                        <asp:Label ID="lblNomeCandidato" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Data de Nascimento:
                    </th>
                    <td>
                        <asp:Label ID="lblDataNascimento" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Nome do Mãe:
                    </th>
                    <td>
                        <asp:Label ID="lblNomeMae" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Tipo de deficiência:
                    </th>
                    <td>
                        <asp:Label ID="lblTipoDeficiencia" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Recurso necessário para prova:
                    </th>
                    <td>
                        <asp:Label ID="lblRecursoNecessarioProva" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Unidade de ensino:
                    </th>
                    <td>
                        <asp:Label ID="lblUnidadeEnsino" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Curso:
                    </th>
                    <td>
                        <asp:Label ID="lblCurso" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        Efetuada em:
                    </th>
                    <td>
                        <asp:Label ID="lblDataInscricao" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="fonteNegrito">
                        IP:
                    </th>
                    <td>
                        <asp:Label ID="lblIP" runat="server" class="fonteNormal"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
    <br />
    <br />
    <div style="text-align: right; width: 900px">
        <input id="btnImprimir" style="width: 100px; margin-right: inherit;" type="button"
            name="btnImprimir" title="Imprimir" value="Imprimir" onclick="printPage();" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" runat="server" CssClass="overlay">
                <asp:Panel ID="Panel2" runat="server" CssClass="loader">
                    <asp:Image ID="Image1" runat="server" AlternateText="Updating..." Height="48" ImageUrl="~/Images/updateProgress.gif"
                        Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
