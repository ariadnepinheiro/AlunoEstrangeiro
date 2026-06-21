<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RelatoriosContratos.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.RelatoriosContratos" %>

<asp:Content ID="conRelatoriosContratos" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe os dados do contrato:"
        Width="850px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblAnoBusca" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAnoBusca" runat="server" DataValueField="ano" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlAnoBusca_SelectedIndexChanged" Width="150px">
                        <asp:ListItem Text="Nenhum" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="2008" Value="2008"></asp:ListItem>
                        <asp:ListItem Text="2009" Value="2009"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Ano: Preenchimento obrigatório." ID="rfAnoBusca"
                        runat="server" ControlToValidate="ddlAnoBusca" InitialValue="" ValidationGroup="Imprimir"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblContratosBusca" runat="server" Text="Contratos:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlContratosBusca" runat="server" AutoPostBack="false" Width="550px">
                        <asp:ListItem Text="Nenhum" Value="" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Contrato: Preenchimento obrigatório." ID="rfContratoBusca"
                        runat="server" ControlToValidate="ddlContratosBusca" InitialValue="" ValidationGroup="Imprimir"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblConcursoBusca" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Key="concurso" Argument="descricao"
                        MaxLength="20" SqlSelect="select concurso, descricao, ano from LY_CONCURSO_DOCENTE"
                        SqlWhere="tipo = 'Contrato'" ArgumentColumns="90" Columns="30" GridWidth="850px">
                        <gridcolumns>
							<tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
							<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
						</gridcolumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Processo Seletivo: Preenchimento obrigatório."
                        ID="rfConcursoBusca" runat="server" ControlToValidate="tseConcursoBusca" InitialValue=""
                        ValidationGroup="Imprimir"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCandidatoBusca" runat="server" Text="Número de Inscrição:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCandidatoBusca" runat="server" Key="candidato" Argument="nome"
                        SqlSelect="SELECT candidato, nome FROM LY_CANDIDATO_DOCENTE" SqlWhere="concurso = #tseConcursoBusca#"
                        ArgumentColumns="90" Columns="30" MaxLength="20" SqlOrder="nome" GridWidth="850px">
                        <gridcolumns>
							<tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
							<tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
						</gridcolumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ErrorMessage="Número de Inscrição: Preenchimento obrigatório."
                        ID="rfCandidatoBusca" runat="server" ControlToValidate="tseCandidatoBusca" InitialValue=""
                        ValidationGroup="Imprimir"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <br />
                    <asp:ImageButton ID="btnImprimir" runat="server" ValidationGroup="Imprimir" SkinID="Imprimir"
                        OnClick="btnImprimir_Click" />
                    <asp:ValidationSummary ID="vsImprimir" runat="server" EnableClientScript="true" ShowMessageBox="true"
                        ValidationGroup="Imprimir" ShowSummary="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnRelatorio" runat="server" GroupingText="" Visible="false" Width="700px">
    </asp:Panel>
</asp:Content>
