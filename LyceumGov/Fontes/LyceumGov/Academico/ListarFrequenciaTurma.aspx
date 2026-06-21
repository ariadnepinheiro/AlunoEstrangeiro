<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarFrequenciaTurma.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ListarFrequenciaTurma" %>

<asp:Content ID="cListarTurma" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar turma">
        <div>
            <table width="80%">
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td width="35%">
                        <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                            DataTextField="ano" DataValueField="ano" Width="70px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblPeriodo" runat="server" Font-Names="Verdana" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="id_reduzida" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged" DataValueField="periodo"
                            Width="100px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td colspan="3">
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
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            SqlWhere=" id_regional = #tseRegional# " GridWidth="600px" ArgumentColumns="50"
                            OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
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
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,ua_atual,ua_antiga,municipio,id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# " GridWidth="850px"
                            OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="18%" Visible="false" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="18%" Visible="false" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdTurma" runat="server" KeyFieldName="ano;semestre;turma" ClientInstanceName="grdTurma"
        AutoGenerateColumns="False" OnAfterPerformCallback="grdTurma_AfterPerformCallback"
        Font-Names="Verdana" Font-Size="Small" Width="90%" OnSelectionChanged="grdTurma_SelectionChanged"
        SkinID="NoConfirmDelete">
        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="True" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="5%">
                <SelectButton Text="Selecionar" Visible="True">
                    <Image Url="~/img/bt_busca.png" />
                </SelectButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn FieldName="ano" VisibleIndex="1" Caption="Ano" Width="50px"
                CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit MaxLength="5">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="semestre" VisibleIndex="2" Caption="Período"
                CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit MaxLength="5">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="curso" VisibleIndex="3" Caption="Curso"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="nomeCurso" VisibleIndex="4" Caption="Escolaridade">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="turno" VisibleIndex="5" Caption="Turno"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="descricaoTurno" VisibleIndex="6" Caption="Turno">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="serie" VisibleIndex="7" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="descricaoSerie" VisibleIndex="8" Caption="Ano de Escolaridade">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="turma" VisibleIndex="9" Caption="Turma"
                Width="70px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="nomeUnidadeResponsavel" VisibleIndex="11"
                Caption="Unidade Ensino">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="capacidade" VisibleIndex="12" Caption="Capacidade">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="dependencia" VisibleIndex="13" Caption="Sala de Aula">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="curriculo" VisibleIndex="14" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="faculdade" VisibleIndex="15" Visible="false">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
