<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DocumentoCertificacao.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.DocumentoCertificacao" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfRenovacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </br>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstancename="lblMensagem"></asp:Label>
    <asp:ObjectDataSource ID="odsTipoConclusao" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.DocumentoCertificacao"
        SelectMethod="ListarTipoConclusao">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoDocumentoCertifica" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.DocumentoCertificacao"
        SelectMethod="ListarTipoDocumentoCertifica"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsDocumentoCertificacao" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.DocumentoCertificacao"
        InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete" SelectMethod="listarDocumentoCertificacao">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </br> </br> </br> </br>
    <dxwgv:ASPxGridView ID="grdDocumentoCertificacao" runat="server" KeyFieldName="DOCUMENTOCERTID"
        AutoGenerateColumns="false" DataSourceID="odsDocumentoCertificacao" ClientInstanceName="grdDocumentoCertificacao"
        OnRowInserting="grdDocumentoCertificacao_RowInserting" OnRowUpdating="grdDocumentoCertificacao_RowUpdating"
        OnRowDeleting="grdDocumentoCertificacao_RowDeleting" OnCellEditorInitialize="grdDocumentoCertificacao_CellEditorInitialize"
        OnAfterPerformCallback="grdDocumentoCertificacao_AfterPerformCallback" Visible="true">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados.." />
        <SettingsPager PageSize="15" />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdDocumentoCertificacao.AddNewRow();" />
                    </div>
                </HeaderCaptionTemplate>
                <CancelButton Visible="true" Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <EditButton Visible="True" Text="Editar">
                    <Image Url="../img/bt_editar.png" />
                </EditButton>
                <DeleteButton Visible="True" Text="Remover">
                    <Image Url="../img/bt_exclui2.png" />
                </DeleteButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <UpdateButton Visible="true" Text="Salvar">
                    <Image Url="../img/bt_salvar.png" />
                </UpdateButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataColumn FieldName="DOCUMENTOCERTID" Visible="false" VisibleIndex="1"
                Width="10px">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="2" Caption="Tipo de Conclusão" Name="ddlTIPOCONCLUSAOID"
                FieldName="TIPOCONCLUSAOID" HeaderStyle-Font-Bold="true" Width="15px">
                <PropertiesComboBox DataSourceID="odsTipoConclusao" TextField="DESCRICAO" ValueField="TIPOCONCLUSAOID"
                    ValueType="System.String">
                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip"
                        RequiredField-ErrorText="Selecione o tipo de conclusão." EnableCustomValidation="true">
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aluno*" Name="ALUNO" VisibleIndex="3" FieldName="ALUNO"
                Width="30px" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="4" Caption="Tipo do Documento" Name="ddlTipoDocumentoCertificaID"
                FieldName="DOCUMENTOID" HeaderStyle-Font-Bold="true" Width="30px">
                <PropertiesComboBox DataSourceID="odsTipoDocumentoCertifica" TextField="DESCRICAO"
                    ValueField="DOCUMENTOID" ValueType="System.String">
                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip"
                        RequiredField-ErrorText="Selecione o tipo do documento." EnableCustomValidation="true">
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Número" Name="NUMERO" VisibleIndex="5" FieldName="NUMERO"
                Width="50px">
                <PropertiesTextEdit MaxLength="10">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Folha" Name="FOLHAS" VisibleIndex="6" FieldName="FOLHAS"
                Width="50px">
                <PropertiesTextEdit MaxLength="10">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Livro" FieldName="LIVRO" Name="LIVRO" VisibleIndex="7"
                Width="50px">
                <PropertiesTextEdit MaxLength="10">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Eixo" FieldName="EIXO" VisibleIndex="8" Width="550px">
                <PropertiesTextEdit MaxLength="255">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="9"
                Width="550px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
