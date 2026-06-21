<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Transcricao.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.Transcricao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    
    @media print {
        #divBotoes, #divMensagem, .top_inf {
            display: none;
        }
    }
    
</style>

<script language="javascript" type="text/javascript">

    function numberToLetter(number) {
        switch (number) {
            case 0: return "S";
            case 1: return "A";
            case 2: return "B";
            case 3: return "C";
            case 4: return "D";
            case 5: return "E";
            default: return null;
        }
    }

    function letterToNumber(letter, limit) {
        if (letter.length != 1)
            return;

        letter = letter.toUpperCase();
        
        switch (letter) {
            case "S": return 0;
            case "A": return 1;
            case "B": return 2;
            case "C": return (limit >= 3 ? 3 : null);
            case "D": return (limit >= 4 ? 4 : null);
            case "E": return (limit >= 5 ? 5 : null);
            default: return null;
        }
    }

    function validateLetter(evt, obj) {
        evt = evt || window.event;
        var charCode = evt.keyCode || evt.which;
        var charStr = String.fromCharCode(charCode);
        return letterToNumber(charStr, obj.dataset.qtdAlternativas) != null;
    }

    function keypress(evt, obj) {
        return validateLetter(evt, obj);
    }

    function keyup(evt, obj) {
        var isValid = validateLetter(evt, obj);
        if (!isValid)
            return;

        obj.value = obj.value.toUpperCase();
        var inputs = $(obj).closest('form').find(':input[type="text"]');
        inputs.eq(inputs.index(obj) + 1).focus();
    }

    function keydown(evt, obj) {
        //return false;
    }

    function validaData(evt, obj) {
        var dataArray = evt.target.value.split('/');
        var data = new Date(dataArray[1] + "/" + dataArray[0] + "/" + dataArray[2] + " 00:00:00");
        var dataMinima = new Date(obj.dataset.minDate + " 00:00:00");
        var dataMaxima = new Date(obj.dataset.maxDate + " 00:00:00");

        if (data < dataMinima) {
            alert("Data informada menor que a data mínima: " + dataMinima.toLocaleDateString("pt-BR"));
            evt.target.focus();
            evt.target.value = "";
        }

        if (data > dataMaxima) {
            alert("Data informada maior que a data máxima: " + dataMaxima.toLocaleDateString("pt-BR"));
            evt.target.focus();
            evt.target.value = "";
        }
    }

    function confirmacaoFinalizacao() {
        if (confirm("Confirma a finalização da transcrição?")) {
            __doPostBack('ctl00$cphFormulario$btnConcluir', '');
        } else {
            return false;
        }
    }

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" OnPreRender="UpdatePanel1_PreRender" >
        <ContentTemplate>
    
            <asp:HiddenField ID="hdnTranscricaoId" runat="server" />
    
            <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Dados da turma." Width="775px">
                <table width="750px">
                    <tr>
                        <td style="width: 100px;"></td>
                        <td style="width: 100px;"></td>
                        <td style="width: 100px;"></td>
                        <td style="width: 100px;"></td>
                        <td style="width: 100px;"></td>
                        <td style="width: 100px;"></td>
                        <td style="width: 100px;"></td>
                        <td style="width: 50px;"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblAvaliacao" runat="server" Font-Names="Verdana" Text="Avaliação:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="txtAvaliacao" runat="server" Font-Names="Verdana"><%= AvaliacaoDescricao %></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblProva" runat="server" Font-Names="Verdana" Text="Prova:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="txtProva" runat="server" Font-Names="Verdana"><%= ProvaDescricao %></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="txtRegional" runat="server" Font-Names="Verdana"><%= Unidade_Regional %></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblEtapa" runat="server" Font-Names="Verdana" Text="Etapa:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="txtEtapa" runat="server" Font-Names="Verdana"><%= Etapa %></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="txtMunicipio" runat="server" Font-Names="Verdana"><%= Unidade_Municipio %></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label Font-Names="Verdana" ID="lblTurma" runat="server" Text="Turma:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="txtTurma" runat="server" Font-Names="Verdana"><%= Turma %></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="txtUnidadeResponsavel" runat="server" Font-Names="Verdana"><%= Unidade_Nome %></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <br /><br />
            
            <div id="divBotoes" style="width: 775px; text-align: right;">
                <b>Legenda:</b>&nbsp;&nbsp;&nbsp;
                <img src="../img/transcricao-verde.png" />&nbsp;Resposta Certa&nbsp;&nbsp;&nbsp;
                <img src="../img/transcricao-vermelha.png" />&nbsp;Resposta Errada
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar Parcialmente" OnClick="btnSalvar_Click" UseSubmitBehavior="false" />
                <asp:Button ID="btnConcluir" runat="server" Text="Salvar e Finalizar" OnClick="btnConcluir_Click" UseSubmitBehavior="false" OnClientClick="return confirmacaoFinalizacao();" />
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar para Lista" OnClick="btnVoltar_Click" />
                <asp:Button ID="btnExportar" runat="server" Text="Exportar" OnClick="btnExportar_Click" Visible="false" />
            </div>
            
            <div id="divMensagem">
                <br /><br />
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Text=""></asp:Label>
            </div>
            
            <dxwgv:ASPxGridView ID="grdTranscricao" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdTranscricao" 
                KeyFieldName="ALUNO" Width="1565px" EnableViewState="false" OnPreRender="grdTranscricao_PreRender">
                
                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" UseFixedTableLayout="false"
                ShowHorizontalScrollBar="true" ShowVerticalScrollBar="false" ShowColumnHeaders="true" />
                <SettingsEditing Mode="Inline"  />
                <SettingsBehavior AllowDragDrop="false" ProcessSelectionChangedOnServer="false" AllowMultiSelection="false"
                AllowGroup="false" AllowSort="false" />
                <SettingsPager Mode="ShowAllRecords" />
                
                <Columns>
                    <%--<dxwgv:GridViewDataColumn Caption="ALUNOPARTICIPANTEID" FieldName="ALUNOPARTICIPANTEID" VisibleIndex="0" Width="0px" Visible="false" Settings-AllowSort="False" />--%>
                    <dxwgv:GridViewDataColumn Caption="Código" FieldName="ALUNO" VisibleIndex="0" Width="100px" Settings-AllowSort="False" />
                    <dxwgv:GridViewDataColumn Caption="Aluno" FieldName="NOME_COMPL" VisibleIndex="1" Width="150px" Settings-AllowSort="False" />
                    <dxwgv:GridViewDataColumn Caption="Sx" FieldName="SEXO" VisibleIndex="2" Width="30px" Settings-AllowSort="False" />
                    <dxwgv:GridViewDataColumn Caption="Dt. Nasc." FieldName="DT_NASC" VisibleIndex="3" Width="70px" Settings-AllowSort="False" />
                </Columns>
                
            </dxwgv:ASPxGridView>
            
            <script language="javascript" type="text/javascript">
                function addBands() {
                    
                    /*
                    Obs.: o ID da coluna no HTML de resposta da página muda quando se altera quaisquer propriedades do grdTranscrição.
                    Ex.: "#ctl00_cphFormulario_grdTranscricao_DXTcol" -> o "DXT" do nome, que vem antes do "col", é conforme as propriedades
                    do grid, então essa parte do nome pode mudar.
                    */
                
                    if (grdTranscricao.pageRowCount == 0)
                        return;
                
                    $("#ctl00_cphFormulario_grdTranscricao_DXHeadersRow")
                        .clone()
                        .attr("id", "topHeader")
                        .insertBefore("#ctl00_cphFormulario_grdTranscricao_DXHeadersRow");

                    for (i = 0; i <= 4; i++)
                        $("#topHeader #ctl00_cphFormulario_grdTranscricao_DXTcol" + i).html("&nbsp;");

                    var ceId = [];
                    var ceDescricao = [];
                    var ceColspan = [];
                    var ceStartIndex = [];
                    var oldCeId = 0;
                    var ceIndex = -1;
                    for (i = 5; i <= grdTranscricao.columns.length - 1; i++) {
                        var control = $(grdTranscricao.GetRow(0).cells[i]).find(":input");
                        var componenteId = control.attr("data-componente-id");
                        var descricaoComponente = control.attr("data-componente-descricao");
                        if (componenteId) {
                            if (oldCeId == componenteId) {
                                ceColspan[ceIndex]++;
                            } else {
                                oldCeId = componenteId;
                                ceId.push(componenteId);
                                ceDescricao.push(descricaoComponente);
                                ceColspan.push(1);
                                ceStartIndex.push(i);
                                ceIndex = ceId.length - 1;
                            }
                        }
                    }

                    for (i = 0; i < ceId.length; i++) {
                        ceColspan[i] += 2;
                    }

                    var colspan = 4;
                    for (i = 0; i < ceId.length; i++) {
                        for (j = ceStartIndex[i]; j < ceStartIndex[i] + ceColspan[i]; j++) {
                            if (j == ceStartIndex[i]) {
                                $("#topHeader #ctl00_cphFormulario_grdTranscricao_DXTcol" + j)
                                    .attr("colspan", ceColspan[i])
                                    .html(ceDescricao[i]);
                            }
                            else
                                $("#topHeader #ctl00_cphFormulario_grdTranscricao_DXTcol" + j).remove();
                        }
                    }
                }
            </script>
            
        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportar" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
</asp:Content>