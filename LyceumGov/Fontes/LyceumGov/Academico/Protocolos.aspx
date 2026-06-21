<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Protocolos.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Protocolos" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxwgv:ASPxGridView ID="grdProtocolos" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdProtocolos" KeyFieldName="IdProtocoloNota" DataSourceID="odsProtocolos">
        <Columns>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="IdProtocoloNota" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data" FieldName="DtCadastro" VisibleIndex="2"
                Width="120">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="Codigo" VisibleIndex="3"
                Width="300">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="Tipo" VisibleIndex="4" Width="70">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="Ano" VisibleIndex="5" Width="50">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="Periodo" VisibleIndex="6"
                Width="50">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Bimestre/Trimestre" FieldName="Subperiodo" VisibleIndex="7"
                Width="100">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="Turma" VisibleIndex="8"
                Width="100">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="NomeDisciplina" VisibleIndex="9">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsProtocolos" runat="server" TypeName="Techne.Lyceum.RN.ProtocoloNota"
        SelectMethod="Listar">
        <SelectParameters>
            <asp:Parameter DbType="String" Size="8" Name="matricula" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
