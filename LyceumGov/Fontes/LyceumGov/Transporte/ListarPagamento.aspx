<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarPagamento.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.ListarPagamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="Panel1" runat="server" GroupingText="Informe os dados para pesquisar pagamento"
        Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                        OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" situacao = 'ESTADUAL' and id_regional = #tseRegional# AND municipio = #tseMunicipio# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />							
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 740px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Pagamento" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel ID="pnlDadosNovo" runat="server" GroupingText="Dados para lançamento de um novo pagamento"
        Visible="false" Width="50%">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Data Início:* "></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Text="Data Fim:* "></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataFim" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td align="left">
                    <asp:Button ID="btnProsseguir" runat="server" Text="Efetuar Lançamento" OnClick="btnProsseguir_Click"
                        OnClientClick="Bloqueio()" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsPagamento" runat="server" TypeName="Techne.Lyceum.Net.Transporte.ListarPagamento"
        SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdPagamento" runat="server" KeyFieldName="PAGAMENTOID" ClientInstanceName="grdPagamento"
            AutoGenerateColumns="False" DataSourceID="odsPagamento" Width="50%" SkinID="NoConfirmDelete">
            <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Styles CommandColumn-Wrap="False" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Selecionar" Name="btnDetalhes" VisibleIndex="0"
                    Width="30px" CellStyle-HorizontalAlign="Center" EditFormCaptionStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("PAGAMENTOID") %>'
                            OnCommand="btnDetalhes_Command" ImageUrl="~/img/bt_busca.png" Height="20px" AlternateText="Visualizar Detalhes do Pagamento">
                        </asp:ImageButton>
                    </DataItemTemplate>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="PAGAMENTOID" VisibleIndex="1" Caption="PAGAMENTOID"
                    CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="QUANTIDADEDIAS" VisibleIndex="1" Caption="Quantidade Dias"
                    CellStyle-HorizontalAlign="Center" Width="30">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="VALORTOTAL" VisibleIndex="2" Caption="Valor Total"
                    CellStyle-HorizontalAlign="Center" Width="30">
                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                        MaxLength="9">
                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATAINICIO" VisibleIndex="4" Caption="Data Início"
                    CellStyle-HorizontalAlign="Center" Width="100">
                    <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                    </PropertiesTextEdit>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATAFIM" VisibleIndex="5" Caption="Data Fim"
                    CellStyle-HorizontalAlign="Center" Width="100">
                    <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                    </PropertiesTextEdit>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATAGERACAOPAGAMENTO" VisibleIndex="8" Caption="Data Geraçao Pagamento"
                    CellStyle-HorizontalAlign="Center" Width="100">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
