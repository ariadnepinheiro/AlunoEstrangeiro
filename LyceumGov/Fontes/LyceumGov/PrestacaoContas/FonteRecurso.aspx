<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FonteRecurso.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.FonteRecurso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsFonteRecurso" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.FonteRecurso"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdFonteRecurso" runat="server" DataSourceID="odsFonteRecurso"
        KeyFieldName="FONTERECURSOID" AutoGenerateColumns="false" ClientInstanceName="grdFonteRecurso"
        OnInitNewRow="grdFonteRecurso_InitNewRow" OnStartRowEditing="grdFonteRecurso_StartRowEditing"
        OnRowUpdating="grdFonteRecurso_RowUpdating" OnAfterPerformCallback="grdFonteRecurso_AfterPerformCallback"
        Width="60%" OnCellEditorInitialize="grdFonteRecurso_CellEditorInitialize">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <CancelButton Visible="true" Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <EditButton Visible="True" Text="Editar">
                    <Image Url="../img/bt_editar.png" />
                </EditButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <UpdateButton Visible="true" Text="Alterar">
                    <Image Url="../img/bt_salvar.png" />
                </UpdateButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="FONTERECURSOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataColumn Caption="Código SEFAZ" FieldName="CODIGOSEFAZ" VisibleIndex="2" Width="50px" ReadOnly="true">
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="3"
                FieldName="DESCRICAO" Width="300px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Ínicio*" FieldName="DATAINICIO" VisibleIndex="4"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="5"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
