<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Consulta.aspx.cs" Inherits="Techne.Lyceum.Net.Ocorrencia.Consulta" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/glider-js@1/glider.min.css" rel="stylesheet">
    <link href="../Styles/Ocorrencia.css" rel="stylesheet" type="text/css">
    <style>
        .cursorImagem
        {
            cursor: pointer;
        }
        .txtInput
        {
            background-color: White;
            font-family: Verdana;
            font-size: smaller;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnconsulta" runat="server" GroupingText="Informe os dados para pesquisar a ocorrência"
        Width="60%">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAno" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList Height="20px" ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        SkinID="a" Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDtOcorrencia" runat="server" Text="Data Ocorrência: "></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataOcorrencia" runat="server" Width="100px" Enabled="true"
                        EnableDefaultAppearance="true" ClientInstanceName="dtDataOcorrencia" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRegional" runat="server" Text="Regional: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                        Key="id_regional" SqlSelect="select distinct ID_REGIONAL, descricao from (select distinct ue.ID_REGIONAL, n.regional as descricao from VW_UNIDADE_ENSINO_SITUACAO uuf
                                                                 join LY_UNIDADE_ENSINO ue on uuf.UNIDADE_ENS = ue.UNIDADE_ENS
                                                                 join TCE_REGIONAL n on n.ID_REGIONAL = ue.ID_REGIONAL) as tabela"
                        SqlOrder="descricao, id_regional">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:Label ID="lblClasse" runat="server" Text="Classe: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseClasse" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                        OnChanged="tseClasse_Changed" Key="classeid" SqlSelect="select distinct CLASSEID, DESCRICAO,ORDEM from [Ocorrencias].[CLASSE]"
                        SqlWhere=" ATIVO = 1 " SqlOrder="ORDEM">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CLASSEID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Classe" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMunicipio" runat="server" Text="Municipio: "></asp:Label>
                </td>
                <td>
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
                <td>
                    <asp:Label ID="lblSubClasse" runat="server" Text="Sub Classe: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseSubClasse" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" Caption="" OnChanged="tseSubClasse_Changed"
                        Key="subclasseid" SqlSelect="select distinct SUBCLASSEID, DESCRICAO,ORDEM, CLASSEID from [Ocorrencias].[SUBCLASSE]"
                        SqlOrder="ORDEM" SqlWhere=" ATIVO = 1 AND CLASSEID = #tseClasse# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="SUBCLASSEID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Classe" FieldName="DESCRICAO" Width="80%" />
                            <tweb:TSearchBoxColumn Caption="Classe" FieldName="CLASSEID" Width="80%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual, bairro, ua_antiga    from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# " GridWidth="850px"
                        OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            <tweb:TSearchBoxColumn Caption="Bairro" FieldName="bairro" Width="18%" />
                            <tweb:TSearchBoxColumn Caption="id_regional" FieldName="id_regional" Width="18%"
                                Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:Label ID="lblTratamento" runat="server" Text="Tratamento: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseTratamento" runat="server" Argument="descricao" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" Caption="" OnChanged="tseTratamento_Changed"
                        Key="tratamentoid" SqlSelect="select distinct TRATAMENTOID, DESCRICAO,ORDEM from [Ocorrencias].[TRATAMENTO]"
                        SqlWhere=" ATIVO = 1 " SqlOrder="ORDEM">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="TRATAMENTOID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Tratamento" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Situação: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSituacao" runat="server">
                        <asp:ListItem Selected="True" Text="Selecione" Value=""> </asp:ListItem>
                        <asp:ListItem Text="Em aberto" Value="EmAberto"></asp:ListItem>
                        <asp:ListItem Text="Encaminhado" Value="Encaminhado"> </asp:ListItem>
                        <asp:ListItem Text="Arquivado" Value="Arquivado"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnBuscar" Text="Buscar" runat="server" OnClick="btnBuscar_Click"
                        OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
    <br />
    <table>
        <tr>
            <td align="left" colspan="4">
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="Panel2" runat="server" Width="100%">
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdRegistro" runat="server" KeyFieldName="OCORRENCIAID" ClientInstanceName="grdRegistro"
                        AutoGenerateColumns="False" OnAfterPerformCallback="grdRegistro_AfterPerformCallback"
                        Width="100%" OnPageIndexChanged="grdRegistro_PageIndexChanged">
                        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Styles CommandColumn-Wrap="False" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                                <SelectButton Text="Selecionar" Visible="True">
                                    <Image Url="~/img/bt_busca.png" />
                                </SelectButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="OCORRENCIAID" VisibleIndex="3" Caption="Id"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="REGIONAL" VisibleIndex="1" Caption="Regional"
                                 >
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="MUNICIPIO" VisibleIndex="1" Caption="Município"
                               >
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="ESCOLA" VisibleIndex="1" Caption="Escola"
                                 >
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="CENSO" VisibleIndex="1" Caption="Censo"
                                CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Data da Ocorrência" FieldName="DATAOCORRENCIA"
                                VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                Width="40">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SUBCLASSE" VisibleIndex="3" Caption="SubClasse"
                                Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="CLASSE" VisibleIndex="3" Caption="Classe"
                                Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SITUACAO" VisibleIndex="5" Caption="Situação"
                                Visible="true" Width="100" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
</asp:Content>
