<%@ Page Language="C#" MasterPageFile="~/Modulos/ProcessoSeletivoMaster.Master" AutoEventWireup="true"
    CodeBehind="ReimpressaoFichaInscricao.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.ReimpressaoFichaInscricao" %>

<%@ MasterType VirtualPath="~/Modulos/ProcessoSeletivoMaster.Master" %>
<asp:Content ID="cReimpressaoFichaInscricao" ContentPlaceHolderID="cphFormulario"
    runat="server">
    <asp:ValidationSummary ID="vsReimpressao" runat="server" EnableClientScript="true"
        DisplayMode="List" ValidationGroup="ReimprimirForm" Font-Bold="true" Font-Size="Small" />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnImprimir" OnClick="Imprimir_Click" runat="server" SkinID="Imprimir"
            ImageAlign="Right" Visible="true" ValidationGroup="ReimprimirForm" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnBusca" GroupingText="Reimpressão da Ficha de Inscrição" runat="server"
        Width="885px">
        <table border="0">
            <tr>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblProcessoSeletivo" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Caption="" SqlSelect="SELECT CD.CONCURSO,CD.DESCRICAO,CD.DT_INICIO,CD.DT_FIM FROM LY_CONCURSO_DOCENTE CD WITH (NOLOCK)"
                        SqlWhere="tipo = 'Contrato' and CONVERT(DATE,GETDATE()) BETWEEN CONVERT(DATE,DT_INICIO) AND CONVERT(DATE,DT_FIM)"
                        ArgumentColumns="100" GridWidth="500">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tseConcursoBusca" ErrorMessage="Processo seletivo não informado."
                        ID="rfvConcursoBusca" InitialValue="" ValidationGroup="ReimprimirForm" SetFocusOnError="true"
                        Display="Dynamic">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" style="vertical-align:middle;" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCpf" runat="server" Text="CPF:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCpf" onkeyup="formataCPF(this,event)" runat="server" MaxLength="14"
                        Width="120px" />
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCpf" ErrorMessage="CPF não informado."
                        ID="rfvCpf" InitialValue="" ValidationGroup="ReimprimirForm" SetFocusOnError="true">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" style="vertical-align:middle;" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDataNasc" runat="server" Text="Data de Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="1901-01-01" Width="128px"
                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <asp:RequiredFieldValidator ErrorMessage="Data de nascimento não informada." ID="rfvDtNasc"
                        runat="server" ControlToValidate="dtDataNasc" InitialValue="" ValidationGroup="ReimprimirForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
