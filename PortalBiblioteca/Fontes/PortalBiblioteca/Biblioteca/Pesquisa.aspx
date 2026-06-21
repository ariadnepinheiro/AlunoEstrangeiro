<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PublicMaster.Master"
    AutoEventWireup="true" CodeBehind="Pesquisa.aspx.cs" Inherits="Techne.Lyceum.Net.Biblioteca.Pesquisa" %>

<asp:Content ID="ctPesquisa" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsPesquisa" TypeName="Techne.Lyceum.Net.Biblioteca.Pesquisa"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="hddBusca" DefaultValue="" Name="busca" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HiddenField ID="hddBusca" runat="server" />
    <dxwgv:ASPxGridView ID="grdBusca" runat="server" AutoGenerateColumns="False" SkinID="grdBuscaAvancada"
        OnCustomCallback="grdBusca_CustomCallback" ClientInstanceName="grdBusca" Width="1020px"
        EnableCallBacks="true" KeyFieldName="id" Font-Names="Verdana" Font-Size="Small"
        DataSourceID="odsPesquisa" OnCustomUnboundColumnData="grdBusca_CustomUnboundColumnData">
        <Settings ShowPreview="true" ShowStatusBar="Visible" ShowTitlePanel="true" ShowGroupPanel="false"
            ShowColumnHeaders="false" />
        <SettingsPager Visible="false">
        </SettingsPager>
        <Styles>
            <TitlePanel BackColor="White" ForeColor="Black" Font-Size="Smaller">
            </TitlePanel>
        </Styles>
        <Templates>
            <TitlePanel>
                <div style="text-align: right;">
                    Livros por página:
                    <select onchange="grdBusca.PerformCallback(this.value);">
                        <option value="5" <%# WriteSelectedIndex(5) %>>5</option>
                        <option value="10" <%# WriteSelectedIndex(10) %>>10</option>
                        <option value="15" <%# WriteSelectedIndex(15) %>>15</option>
                    </select>&nbsp;
                    <%#GetShowingOnPage() %>&nbsp; <a title="Primeira" href="JavaScript:grdBusca.GotoPage(0);">
                        &lt;&lt;</a> &nbsp; <a title="Anterior" href="JavaScript:grdBusca.PrevPage();">&lt;</a>
                    &nbsp; Página
                    <input type="text" onchange="grdBusca.GotoPage(parseInt(this.value, 10) - 1)" onkeydown="if (event.keyCode == 13) { event.cancelBubble=true; event.returnValue = false; grdBusca.GotoPage(parseInt(this.value, 10) - 1); return false; }"
                        value="<%# grdBusca.PageIndex + 1 %>" style="width: 20px" />
                    de
                    <%# grdBusca.PageCount%>
                    &nbsp; <a title="Próximo" href="JavaScript:grdBusca.NextPage();">&gt;</a> &nbsp;
                    <a title="Último" href="JavaScript:grdBusca.GotoPage(<%# grdBusca.PageCount - 1 %>);">
                        &gt;&gt;</a> &nbsp;
                </div>
            </TitlePanel>
            <StatusBar>
                <div style="text-align: right;">
                    Livros por página:
                    <select onchange="grdBusca.PerformCallback(this.value);">
                        <option value="5" <%# WriteSelectedIndex(5) %>>5</option>
                        <option value="10" <%# WriteSelectedIndex(10) %>>10</option>
                        <option value="15" <%# WriteSelectedIndex(15) %>>15</option>
                    </select>&nbsp;
                    <%#GetShowingOnPage() %>&nbsp; <a title="Primeira" href="JavaScript:grdBusca.GotoPage(0);">
                        &lt;&lt;</a> &nbsp; <a title="Anterior" href="JavaScript:grdBusca.PrevPage();">&lt;</a>
                    &nbsp; Página
                    <input type="text" onchange="grdBusca.GotoPage(parseInt(this.value, 10) - 1)" onkeydown="if (event.keyCode == 13) { event.cancelBubble=true; event.returnValue = false; grdBusca.GotoPage(parseInt(this.value, 10) - 1); return false; }"
                        value="<%# grdBusca.PageIndex + 1 %>" style="width: 20px" />
                    de
                    <%# grdBusca.PageCount%>
                    &nbsp; <a title="Próximo" href="JavaScript:grdBusca.NextPage();">&gt;</a> &nbsp;
                    <a title="Último" href="JavaScript:grdBusca.GotoPage(<%# grdBusca.PageCount - 1 %>);">
                        &gt;&gt;</a> &nbsp;
                </div>
            </StatusBar>
            <EmptyDataRow>
                Nenhum livro foi encontrado.
            </EmptyDataRow>
            <PreviewRow>
                <table>
                    <tr>
                        <td>
                            <dxe:ASPxBinaryImage ID="bnImageLivro" runat="server" Value='<%# Bind("imagem") %>'
                                Height="100px">
                                <EmptyImage Url="~/Images/semfoto.jpg" />
                            </dxe:ASPxBinaryImage>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        Título:
                                    </td>
                                    <td>
                                        <dxe:ASPxLabel ID="lblTitulo" runat="server" Value='<%# Bind("titulo") %>'>
                                        </dxe:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Autor(es):
                                    </td>
                                    <td>
                                        <dxe:ASPxLabel ID="lblAutor" runat="server" Value='<%# Bind("nome_autor") %>'>
                                        </dxe:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Editora:
                                    </td>
                                    <td>
                                        <dxe:ASPxLabel ID="lblEditora" runat="server" Value='<%# Bind("editora") %>'>
                                        </dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </PreviewRow>
        </Templates>
        <Columns>
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="id" Visible="false" VisibleIndex="0"
                Width="200px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataHyperLinkColumn Caption="" FieldName="navurl" UnboundType="String">
                <PropertiesHyperLinkEdit Text="Consultar" NavigateUrlFormatString="~/Biblioteca/ReservaMaterial.aspx?{0}">
                </PropertiesHyperLinkEdit>
            </dxwgv:GridViewDataHyperLinkColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
