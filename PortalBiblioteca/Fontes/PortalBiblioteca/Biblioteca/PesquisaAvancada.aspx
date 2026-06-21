<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PublicMaster.Master"
    AutoEventWireup="true" CodeBehind="PesquisaAvancada.aspx.cs" Inherits="Techne.Lyceum.Net.Biblioteca.PesquisaAvancada" %>

<asp:Content ID="ctPesquisa" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function Abre(e) {
            $('#<%=pnFiltros.ClientID%>').slideToggle();
            e.processOnServer = false;
            var text = btnAbre.GetText();
            if (text == "Ocultar filtros")
                btnAbre.SetText("Exibir filtros");
            else
                btnAbre.SetText("Ocultar filtros");
        }
    
    </script>

    <asp:Panel ID="pnFiltros" runat="server" GroupingText="Selecione os filtros da pesquisa"
        Width="1020px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblPesquisar" runat="server" Text="Pesquisar por:" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlFiltro1" runat="server">
                        <asp:ListItem Text="Título" Value="titulo" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Autor" Value="autor"></asp:ListItem>
                        <asp:ListItem Text="Assunto" Value="assunto"></asp:ListItem>
                        <asp:ListItem Text="Editora" Value="editora"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtFiltro1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlFiltro2" runat="server">
                        <asp:ListItem Text="Título" Value="titulo"></asp:ListItem>
                        <asp:ListItem Text="Autor" Value="autor" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Assunto" Value="assunto"></asp:ListItem>
                        <asp:ListItem Text="Editora" Value="editora"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtFiltro2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlFiltro3" runat="server">
                        <asp:ListItem Text="Título" Value="titulo"></asp:ListItem>
                        <asp:ListItem Text="Autor" Value="autor"></asp:ListItem>
                        <asp:ListItem Text="Assunto" Value="assunto" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Editora" Value="editora"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtFiltro3" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblTipo" runat="server" Text="Limitar a busca a determinado tipo de material"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div id="tipos" runat="server">
        </div>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblLocal" runat="server" Text="Localização e Disponibilidade" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBiblioteca" runat="server" Text="Biblioteca:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseBiblioteca" runat="server" Key="codigo" Argument="nome" MaxLength="20"
                        DataType="Number" SqlSelect="SELECT id as codigo, nome_bib as nome from LY_BIB_BIBLIOTECA"
                        GridWidth="850px" AutoPostBack="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:CheckBox ID="chkTodosLocais" runat="server" Checked="false" Text="Todas as bibliotecas" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="chkDisponiveis" runat="server" Checked="false" Text="Somente itens disponíveis." />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:ImageButton ID="btnBuscarAvanc" OnClick="btnBuscarAvanc_Click" ImageUrl="~/Images/bot_buscar.png"
                        runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <dxe:ASPxButton ID="btnAbre" ClientInstanceName="btnAbre" runat="server" Text="Ocultar filtros"
        UseSubmitBehavior="false" Font-Size="Smaller" ClientSideEvents-Click="function(s,e){Abre(e);}"
        Height="20px" Width="1020px" ToolTip="Clique para abrir os filtros da pesquisa.">
    </dxe:ASPxButton>
    <dxwgv:ASPxGridView ID="grdBusca" runat="server" AutoGenerateColumns="False" SkinID="grdBuscaAvancada"
        OnCustomCallback="grdBusca_CustomCallback" ClientInstanceName="grdBusca" Width="1020px"
        EnableCallBacks="false" KeyFieldName="id" Font-Names="Verdana" Font-Size="Small"
        OnCustomUnboundColumnData="grdBusca_CustomUnboundColumnData">
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
