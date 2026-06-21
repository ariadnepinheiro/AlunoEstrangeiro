<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="VerbaPorCompetencia.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.VerbaPorCompetencia" %>

<asp:Content ID="conVerbas" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Selecione o ano:" Width="200px">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" CssClass="ReadOnlyField" AutoPostBack="True"
                        DataTextField="ano" DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsVerba" TypeName="Techne.Lyceum.Net.Curriculo.VerbaPorCompetencia"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" OnUpdating="odsVerba_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdVerba" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdVerba"
        Visible="false" OnAfterPerformCallback="grdVerba_AfterPerformCallback" KeyFieldName="mes"
        DataSourceID="odsVerba" OnCommandButtonInitialize="grdVerba_CommandButtonInitialize"
        OnRowValidating="grdVerba_RowValidating">
        <SettingsText EmptyDataRow="Não existem dados." />
        <SettingsEditing Mode="Inline" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="false">
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
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Mês" FieldName="mes" VisibleIndex="1"
                Width="50" ReadOnly="true">
                <PropertiesComboBox>
                    <DropDownButton Enabled="false" Visible="false">
                    </DropDownButton>
                    <Items>
                        <dxe:ListEditItem Text="Janeiro" Value="1" />
                        <dxe:ListEditItem Text="Fevereiro" Value="2" />
                        <dxe:ListEditItem Text="Março" Value="3" />
                        <dxe:ListEditItem Text="Abril" Value="4" />
                        <dxe:ListEditItem Text="Maio" Value="5" />
                        <dxe:ListEditItem Text="Junho" Value="6" />
                        <dxe:ListEditItem Text="Julho" Value="7" />
                        <dxe:ListEditItem Text="Agosto" Value="8" />
                        <dxe:ListEditItem Text="Setembro" Value="9" />
                        <dxe:ListEditItem Text="Outubro" Value="10" />
                        <dxe:ListEditItem Text="Novembro" Value="11" />
                        <dxe:ListEditItem Text="Dezembro" Value="12" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Valor Inicial" FieldName="valor" VisibleIndex="2"
                Width="150">
                <PropertiesTextEdit MaxLength="13" Width="150px" DisplayFormatString="c" >
                    <MaskSettings Mask="$&lt;0..9999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="GLP Aceita" FieldName="aceita" VisibleIndex="3"
                ReadOnly="true" Width="150" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit MaxLength="13" Width="150px" DisplayFormatString="c">
                    <MaskSettings Mask="$&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="GLP Alocada" FieldName="alocada" VisibleIndex="4"
                ReadOnly="true" Width="150" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit MaxLength="13" Width="150px" DisplayFormatString="c">
                    <MaskSettings Mask="$&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Saldo" FieldName="saldo" VisibleIndex="5"
                Width="150" ReadOnly="true" PropertiesTextEdit-ReadOnlyStyle-Border-BorderStyle="None">
                <PropertiesTextEdit MaxLength="13" Width="150px" DisplayFormatString="c">
                    <MaskSettings Mask="$&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="false" />
        <SettingsPager Mode="ShowAllRecords" />
    </dxwgv:ASPxGridView>
</asp:Content>
