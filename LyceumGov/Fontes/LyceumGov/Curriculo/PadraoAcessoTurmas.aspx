<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadraoAcessoTurmas.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.PadraoAcessoTurmas" %>

<asp:Content ID="conPadraodeAcessoTurmas" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsPadaces" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padaces">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsPadacesTurma" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_padaces_turma"
        SqlWhere="curso=@curso">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseCurso" Name="curso" PropertyName="DBValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <br />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por escolaridade"
        Width="650px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" ColumnName="CURSO" MaxLength="20" SqlSelect="SELECT CURSO, NOME FROM ly_curso"
                        OnChanged="tseCurso_Changed" SqlOrder="nome">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CURSO" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="70%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <dxwgv:ASPxGridView runat="server" ID="grdPadaces" DataSourceID="tdsPadacesTurma"
        AutoGenerateColumns="False" KeyFieldName="CompositeKey" ClientInstanceName="grdPadaces"
        Width="800px" OnCellEditorInitialize="grdPadaces_CellEditorInitialize" OnCustomUnboundColumnData="grdPadaces_CustomUnboundColumnData"
        OnInitNewRow="grdPadaces_InitNewRow" OnRowDeleting="grdPadaces_RowDeleting" OnRowInserting="grdPadaces_RowInserting"
        OnRowUpdating="grdPadaces_RowUpdating" OnRowValidating="grdPadaces_RowValidating"
        OnStartRowEditing="grdPadaces_StartRowEditing" OnAfterPerformCallback="grdPadaces_AfterPerformCallback"
        Visible="false">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPadaces.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
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
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn FieldName="curso" VisibleIndex="0" Caption="Curso*"
                Visible="false">
                <PropertiesTextEdit MaxLength="20">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Padrão de Acesso*" HeaderStyle-Font-Bold="true"
                FieldName="padaces" VisibleIndex="1">
                <PropertiesComboBox MaxLength="14" TextField="padaces" ValueField="padaces" DataSourceID="tdsPadaces"
                    Width="200px" DropDownWidth="150px" EnableIncrementalFiltering="true">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Padrão de Acesso." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Operação*" HeaderStyle-Font-Bold="true"
                FieldName="operacao" VisibleIndex="2">
                <PropertiesComboBox MaxLength="20" Width="350px" DropDownWidth="330px" EnableIncrementalFiltering="true">
                    <Items>
                        <dxe:ListEditItem Text="Permite manipulação da turma" Value="Geral" />
                        <dxe:ListEditItem Text="Permite manipulação do quadro de horários" Value="Quadro de Horários" />
                        <dxe:ListEditItem Text="Permite manipulação do quadro de horários parcialmente" Value="Parcial" />
                    </Items>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Operação." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn FieldName="dt_inicio" VisibleIndex="4" Caption="Data de Início*"
                Width="120px" HeaderStyle-Font-Bold="true">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data de Início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn FieldName="dt_fim" VisibleIndex="5" Caption="Data de Término*"
                Width="120px" HeaderStyle-Font-Bold="true">
                <PropertiesDateEdit Width="120px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Data de Término." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="6" UnboundType="String"
                Visible="False">
            </dxwgv:GridViewDataColumn>
        </Columns>
        <Settings ShowFilterRow="false" />
    </dxwgv:ASPxGridView>
</asp:Content>
