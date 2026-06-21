<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Campanha.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.Campanha" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 968px;">
        <asp:Label runat="server" ID="lblBloco" Text="Campanha" SkinID="BcTitulo" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
    </div>
    <div>
        <asp:Panel ID="pnlCampanha" runat="server" GroupingText="Caracteristicas da campanha selecionada">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label SkinID="lblObrigatorio" ID="lblAno" runat="server" Text="Ano:*"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ANO" DataValueField="ANO">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblPeriodo" SkinID="lblObrigatorio" runat="server" Text="Semestre:*"></asp:Label>
                        <asp:DropDownList ID="ddlperiodo" runat="server">
                            <asp:ListItem Selected="True" Value="0">Selecione</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblTitulo" SkinID="lblObrigatorio" runat="server" Text="Título*"></asp:Label>
                        <asp:HiddenField ID="HiddenID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtTitulo" runat="server" MaxLength="500" TextMode="MultiLine" Height="79px"
                            Width="1355px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblObjetivo" SkinID="lblObrigatorio" runat="server" Text="Objetivo*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtObjetivo" runat="server" MaxLength="8000" TextMode="MultiLine"
                            Height="79px" Width="1355px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblProcedimento" SkinID="lblObrigatorio" runat="server" Text="Procedimento*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txtProcedimento" runat="server" MaxLength="8000" TextMode="MultiLine"
                            Height="79px" Width="1355px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblExibeInspecaoEscolar" runat="server" Text="Exibe aba Inspeção Escola*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:RadioButtonList ID="rblExibeInspecaoEscolar" runat="server" RepeatDirection="Horizontal" Width="150px" >
                            <asp:ListItem Text="Sim" Value="true"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <dxwgv:ASPxGridView ID="grdCampanha" runat="server" AutoGenerateColumns="False" EnableCallBacks="False"
            ClientInstanceName="grdCampanha" DataSourceID="odsCamapanha" KeyFieldName="CAMPANHAID"
            OnAfterPerformCallback="grdCampanha_AfterPerformCallback" OnCustomButtonCallback="grdCampanha_CustomButtonCallback"
            OnRowDeleting="grdCampanha_RowDeleting">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <ClientSideEvents RowClick="function(s, e) {
                 doProcessClick = true;
                 visibleIndex = e.visibleIndex+1;
                 var key = s.GetRowKey(e.visibleIndex);
                 
                 window.setTimeout(ProcessClick(key),500);
                 
                 
            }" RowDblClick="function(s, e) {
	doProcessClick = false;
	var key = s.GetRowKey(e.visibleIndex);
	    
	alert('Here is the RowDoubleClick action in a row with the Key = '+descricao);
            }" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="60px">
                    <DeleteButton Text="Remover" Visible="True">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <UpdateButton>
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="Editar" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="CAMPANHAID" FieldName="CAMPANHAID" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" VisibleIndex="3"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Título" FieldName="TITULO" VisibleIndex="4"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Objetivo" FieldName="OBJETIVO" VisibleIndex="5"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Procedimento" FieldName="PROCEDIMENTO" VisibleIndex="6"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="Exibe Inspeção Escola" FieldName="EXIBEINSPECAOESCOLAR" VisibleIndex="7"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
        <asp:ObjectDataSource ID="odsCamapanha" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Campanha"
            runat="server" SelectMethod="ListarCampanha" DeleteMethod="Deletar"></asp:ObjectDataSource>
    </div>
</asp:Content>
