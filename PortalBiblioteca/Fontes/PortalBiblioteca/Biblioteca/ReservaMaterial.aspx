<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PublicMaster.Master"
    AutoEventWireup="true" CodeBehind="ReservaMaterial.aspx.cs" Inherits="Techne.Lyceum.Net.Biblioteca.ReservaMaterial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnReservas" GroupingText="Dados do Título" Width="700">
        <table cellspacing="15">
            <tr>
                <td>
                    <dxe:ASPxBinaryImage ID="bnImageLivro" runat="server" Height="100px" StoreContentBytesInViewState="true">
                        <EmptyImage Url="~/Images/semfoto.jpg" />
                    </dxe:ASPxBinaryImage>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <b>Título:</b>
                            </td>
                            <td>
                                <dxe:ASPxLabel ID="lblTitulo" runat="server">
                                </dxe:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Autor(es):</b>
                            </td>
                            <td>
                                <dxe:ASPxLabel ID="lblAutor" runat="server">
                                </dxe:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Editora:</b>
                            </td>
                            <td>
                                <dxe:ASPxLabel ID="lblEditora" runat="server">
                                </dxe:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <dxe:ASPxLabel ID="lblReserve" runat="server" Text="Reserve esse livro em uma das bibliotecas:">
        </dxe:ASPxLabel>
    </asp:Panel>
    <asp:HiddenField ID="hddTitulo" runat="server" />
    <asp:ObjectDataSource ID="odsReserva" TypeName="Techne.Lyceum.Net.Biblioteca.ReservaMaterial"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="hddTitulo" DefaultValue="" Name="titulo" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdReserva" runat="server" AutoGenerateColumns="False" Width="700"
        Visible="false" ClientInstanceName="grdReserva" DataSourceID="odsReserva" KeyFieldName="codigo"
        OnCustomButtonCallback="grdReserva_CustomButtonCallback" EnableCallBacks="false">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="codigo" VisibleIndex="0"
                CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Biblioteca" FieldName="id_bib_biblioteca"
                VisibleIndex="2" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Biblioteca" FieldName="biblioteca" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade" FieldName="unidade" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="status" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="5" ButtonType="Link" Width="50px" Caption="Reservar">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnReservar" Text="Reservar" Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
