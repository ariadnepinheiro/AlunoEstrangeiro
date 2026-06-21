<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ValorGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.ValorGLP" %>

<asp:Content ID="conPeriodoLetivo" ContentPlaceHolderID="cphFormulario" runat="server">
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlAno"
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
    <asp:ObjectDataSource ID="odsValores" TypeName="Techne.Lyceum.Net.Curriculo.ValorGLP"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" OnUpdating="odsValores_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsFuncao" TypeName="Techne.Lyceum.RN.Funcao" runat="server"
        SelectMethod="ObterFuncaoDoc"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdValores" runat="server" ClientInstanceName="grdValores"
        KeyFieldName="funcao" Visible="false" DataSourceID="odsValores" 
        OnRowValidating="grdValores_RowValidating" 
        oncelleditorinitialize="grdValores_CellEditorInitialize">
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" CellStyle-Wrap="False">
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
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
            <dxwgv:GridViewDataComboBoxColumn Caption="Função" FieldName="funcao" VisibleIndex="2" CellStyle-Wrap="False"
                Width="150px">
                <PropertiesComboBox DataSourceID="odsFuncao" ValueField="funcao" TextField="descricao">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Janeiro" FieldName="m1" VisibleIndex="3" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Fevereiro" FieldName="m2" VisibleIndex="4" CellStyle-Wrap="False"
                Width="50px">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Março" FieldName="m3" VisibleIndex="5" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Abril" FieldName="m4" VisibleIndex="6" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol"/>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Maio" FieldName="m5" VisibleIndex="7" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Junho" FieldName="m6" VisibleIndex="8" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Julho" FieldName="m7" VisibleIndex="9" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Agosto" FieldName="m8" VisibleIndex="10" Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Setembro" FieldName="m9" VisibleIndex="11"
                Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Outubro" FieldName="m10" VisibleIndex="12"
                Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Novembro" FieldName="m11" VisibleIndex="13"
                Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Dezembro" FieldName="m12" VisibleIndex="14"
                Width="60px" CellStyle-Wrap="False">
                <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic" MaxLength="9">
                    <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
