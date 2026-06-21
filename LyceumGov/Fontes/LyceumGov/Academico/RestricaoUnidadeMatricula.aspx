<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RestricaoUnidadeMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.RestricaoUnidadeMatricula" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 18%;
        }
        .style2
        {
            font-size: medium;
            font-weight: bold;
        }
        .style3
        {
            font-size: medium;
        }
    </style>

    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">
        function abrirPopup() {
            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }
        function fecharPopup() {
            window.setTimeout(function() {
                pucConfirmar.Hide();
            }, 1000);
        }
        function ConfirmaExclusaoRestricao() {
            if (confirm("Deseja excluir as restrições selecionadas?")) {
                return true;
            }
            return false;
        }
    </script>

    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlAno" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPeriodo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkRegional" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rblTipoFiltroRegional" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="tseRegional" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="chkMunicipio" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rblTipoFiltroMunicipio" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="tseMunicipio" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="chkUnidadeEnsino" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rblTipoFiltroUnidade" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="tseUnidade" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="chkCurso" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rblTipoFiltroCurso" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="tseCurso" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="chkSerie" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rblTipoFiltroSerie" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlSerie" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnExcluir" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdRestricao" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlFiltros" GroupingText="Informe os dados para pesquisar"
                Width="800">
                <table>
                    <tr>
                        <td style="text-align: left;" class="style1">
                            <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio" Style="text-align: left"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;" class="style1">
                            <asp:Label ID="lblPeriodo" runat="server" Text="Periodo:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                                AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:CheckBox ID="chkRegional" runat="server" Text="Regional" OnCheckedChanged="chkRegional_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTipoFiltroRegional" runat="server" AutoPostBack="true"
                                Visible="false" OnSelectedIndexChanged="rblTipoFiltroRegional_SelectedIndexChanged">
                                <asp:ListItem Text="Todas" Value="todasRegionais" Selected="True">
                                </asp:ListItem>
                                <asp:ListItem Text="Por Regional" Value="porRegionais">
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Panel ID="pnlRegional" runat="server" Visible="false">
                                <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                    MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                                    Key="id_regional" SqlSelect="SELECT DISTINCT u.id_regional,regional FROM VW_ZZCRO_UNIDADE_ENSINO u JOIN municipio m ON u.municipio = m.CODIGO JOIN dbo.TCE_REGIONAL r ON r.ID_REGIONAL=u.ID_REGIONAL"
                                    SqlOrder="regional" DataType="Number">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:CheckBox ID="chkMunicipio" runat="server" Text="Municipio" AutoPostBack="true"
                                OnCheckedChanged="chkMunicipio_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTipoFiltroMunicipio" runat="server" AutoPostBack="true"
                                Visible="false" OnSelectedIndexChanged="rblTipoFiltroMunicipio_SelectedIndexChanged">
                                <asp:ListItem Text="Todas" Value="todosMunicipio" Selected="True">
                                </asp:ListItem>
                                <asp:ListItem Text="Por Município" Value="porMunicipio">
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Panel ID="pnlMunicipio" runat="server" Visible="false">
                                <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                    SqlWhere="id_regional IS NOT NULL" GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed"
                                    Columns="10" MaxLength="10">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                        <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:CheckBox ID="chkUnidadeEnsino" runat="server" Text="Unidade de Ensino" AutoPostBack="true"
                                OnCheckedChanged="chkUnidadeEnsino_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTipoFiltroUnidade" runat="server" AutoPostBack="true"
                                Visible="false" OnSelectedIndexChanged="rblTipoFiltroUnidade_SelectedIndexChanged">
                                <asp:ListItem Text="Todas" Value="todasUnidade" Selected="True">
                                </asp:ListItem>
                                <asp:ListItem Text="Por Unidade de Ensino" Value="porUnidade">
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Panel ID="pnlUnidade" runat="server" Visible="false">
                                <tweb:TSearchBox ID="tseUnidade" runat="server" Caption="" Key="unidade_ens" MaxLength="20"
                                    ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                    SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# and situacao = 'ESTADUAL'"
                                    GridWidth="850px" OnChanged="tseUnidade_Changed" SqlOrder="nome_comp">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />                                        
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:CheckBox ID="chkCurso" runat="server" Text="Curso" AutoPostBack="true" OnCheckedChanged="chkCurso_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTipoFiltroCurso" runat="server" AutoPostBack="true" Visible="false"
                                OnSelectedIndexChanged="rblTipoFiltroCurso_SelectedIndexChanged">
                                <asp:ListItem Text="Todos" Value="todosCurso" Selected="True">
                                </asp:ListItem>
                                <asp:ListItem Text="Por Curso" Value="porCurso">
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Panel ID="pnlCurso" runat="server" Visible="false">
                                <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct  c.curso , c.nome , tc.tipo , tc.descricao, mc.modalidade as cod_modalidade, mc.descricao as modalidade FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO    inner join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE inner JOIN LY_UNIDADE_ENSINO_CURSOS uec ON c.CURSO = uec.CURSO inner JOIN dbo.LY_UNIDADE_ENSINO ue ON uec.UNIDADE_ENS = ue.UNIDADE_ENS INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA a ON uec.CURSO = a.CURSO "
                                    ArgumentColumns="60" Columns="10" MaxLength="20" ColumnName="curso" DataType="Varchar"
                                    SqlOrder="nome" GridWidth="800px" OnChanged="tseCurso_Changed">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                                        <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="20%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:CheckBox ID="chkSerie" runat="server" Text="Série" AutoPostBack="true" OnCheckedChanged="chkSerie_CheckedChanged"
                                Visible="false" />
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTipoFiltroSerie" runat="server" AutoPostBack="true" Visible="false"
                                OnSelectedIndexChanged="rblTipoFiltroSerie_SelectedIndexChanged">
                                <asp:ListItem Text="Todas" Value="todasSerie" Selected="True">
                                </asp:ListItem>
                                <asp:ListItem Text="Por Série" Value="porSerie">
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSerie" runat="server" AutoPostBack="True" DataTextField="serie"
                                DataValueField="serie" Visible="false" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkTerminalidade" runat="server" Text="Terminalidade" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Style="text-align: right"
                                Text="Buscar" ValidationGroup="SalvarForm" />
                            <asp:Button ID="btnCriarRestricao" runat="server" OnClick="btnBuscar_Click" Style="text-align: right"
                                Text="Criar Restrição" ValidationGroup="SalvarForm" />
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updatePanel3">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                                        <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                                                Height="48" Width="48" />
                                        </asp:Panel>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Visible="true"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="pnlRestricao" runat="server">
                <dxwgv:ASPxGridView ID="grdRestricao" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="grdRestricao" KeyFieldName="ID_RESTRICAO" OnAfterPerformCallback="grdRestricao_AfterPerformCallback"
                    OnPageIndexChanged="grdRestricao_PageIndexChanged">
                    <SettingsEditing Mode="Inline" />
                    <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                    <Columns>
                        <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="grdRestricao.SelectAllRowsOnPage(this.checked);"
                                    title="Select/Unselect all rows on the page" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_RESTRICAO" ReadOnly="true"
                            VisibleIndex="1" Width="60" Visible="false">
                            <PropertiesTextEdit>
                                <ReadOnlyStyle>
                                    <Border BorderStyle="None"></Border>
                                </ReadOnlyStyle>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" ReadOnly="true"
                            Visible="true" VisibleIndex="2" Width="90">
                            <PropertiesTextEdit>
                                <ReadOnlyStyle>
                                    <Border BorderStyle="None"></Border>
                                </ReadOnlyStyle>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Municipio" FieldName="MUNICIPIO" ReadOnly="true"
                            Visible="true" VisibleIndex="2" Width="90">
                            <PropertiesTextEdit>
                                <ReadOnlyStyle>
                                    <Border BorderStyle="None"></Border>
                                </ReadOnlyStyle>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" ReadOnly="true"
                            Visible="true" VisibleIndex="2" Width="90">
                            <PropertiesTextEdit>
                                <ReadOnlyStyle>
                                    <Border BorderStyle="None"></Border>
                                </ReadOnlyStyle>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" ReadOnly="true"
                            Visible="true" VisibleIndex="4">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="CURSO" ReadOnly="true"
                            Visible="true" VisibleIndex="5">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" Visible="true"
                            VisibleIndex="6">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Terminalidade" FieldName="TERMINALIDADE" ReadOnly="true"
                            Visible="true" VisibleIndex="7">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="CODCURSO" FieldName="CODCURSO" ReadOnly="true"
                            VisibleIndex="8" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                </dxwgv:ASPxGridView>
                <asp:Button ID="btnExcluir" Visible="false" runat="server" Text="Excluir Restrição"
                    OnClick="btnExcluir_Click" OnClientClick="return ConfirmaExclusaoRestricao();" />
            </asp:Panel>
            <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
                Modal="True" Width="300px" Height="180px" ShowPageScrollbarWhenModal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" HeaderStyle-HorizontalAlign="Center"
                HeaderText="Resumo de Criação de Restrição/Terminalidade" AllowDragging="True"
                EnableAnimation="False" EnableViewState="False">
                <HeaderStyle HorizontalAlign="Center" />
                <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Visible="true" Text="Total de Regionais: "
                                        CssClass="style2"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalRegional" runat="server" Visible="true" CssClass="style3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Visible="true" Text="Total de Município: "
                                        CssClass="style2"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalMunicipio" runat="server" CssClass="style3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Visible="true" Text="Total de Unidades: " CssClass="style2"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalUnidade" runat="server" CssClass="style3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Visible="true" Text="Total de Cursos: " CssClass="style2"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalCurso" runat="server" CssClass="style3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Visible="true" Text="Total de Séries: " CssClass="style2"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalSerie" runat="server" CssClass="style3"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click"
                                        OnClientClick="pucConfirmar.Hide(); return true;" />
                                </td>
                            </tr>
                        </table>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
