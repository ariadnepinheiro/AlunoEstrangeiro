<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConsultaProtocolo.aspx.cs" Inherits="Techne.Lyceum.Net.Protocolo.ConsultaProtocolo" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informe os dados para pesquisa."
        Width="600px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNomeTipo" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblTipoConsulta" runat="server" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="rblTipoConsulta_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Coordenadoria" Value="C"> </asp:ListItem>
                        <asp:ListItem Text="Regional" Value="R"> </asp:ListItem>
                        <asp:ListItem Text="Unidade de Ensino" Value="U"> </asp:ListItem>
                        <asp:ListItem Text="Situação" Value="S"> </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlCoordenadoria" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Text="Coordenadoria:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCoordenadoria" runat="server" Argument="descricao" ArgumentColumns="50"
                            GridWidth="500px" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                            OnChanged="tseCoordenadoria_Changed" SqlOrder="descricao" Key="nucleo" SqlSelect="SELECT nucleo, descricao FROM LY_NUCLEO"
                            DataType="VarChar">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="nucleo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="descricao" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlRegional" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                            GridWidth="500px" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                            OnChanged="tseRegional_Changed" SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                            DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlUnidade" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="500px"
                            OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                            SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="40%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />								 
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlSituacao" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="Situação da Prestação:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSituacao" runat="server" DataTextField="DESCRICAO" Height="20px"
                            AutoPostBack="true" DataValueField="SITUACAOPROTOCOLOID" AppendDataBoundItems="true"
                            OnSelectedIndexChanged="ddlSituacao_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdProtocolo" runat="server" KeyFieldName="PROTOCOLOPRESTACAOID"
            OnSelectionChanged="grdProtocolo_SelectionChanged" ClientInstanceName="grdProtocolo"
            AutoGenerateColumns="False" Width="100%" SkinID="NoConfirmDelete">
            <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                    <SelectButton Text="Selecionar" Visible="True">
                        <Image Url="~/img/bt_busca.png" />
                    </SelectButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="PROTOCOLOPRESTACAOID" VisibleIndex="0"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="UNIDADEENSINOID" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONALID" VisibleIndex="2"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Processo" FieldName="PROCESSO" VisibleIndex="5">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Número de Folhas" FieldName="NUMEROFOLHAS"
                    VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="SITUACAOPROTOCOLOID" FieldName="SITUACAOPROTOCOLOID"
                    VisibleIndex="7" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TIPOPROTOCOLOID" FieldName="TIPOPROTOCOLOID"
                    VisibleIndex="9" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo Prestação" FieldName="TIPO" VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Programa" FieldName="PROGRAMA" VisibleIndex="11">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="12">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data do Processo" FieldName="DATAPROCESSO"
                    VisibleIndex="13">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Cadastro" FieldName="DATACADASTRO" VisibleIndex="14">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Alteração" FieldName="DATAALTERACAO"
                    VisibleIndex="15" Visible="false">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIOID" VisibleIndex="16"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <asp:Panel ID="pnlGridCoordenadoria" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdProtocoloCoordenadoria" runat="server" KeyFieldName="PROTOLOCOPRESTACAO_COORDENADORIAID"
            ClientInstanceName="grdProtocoloCoordenadoria" AutoGenerateColumns="False" Width="100%"
            SkinID="NoConfirmDelete">
            <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="PROTOLOCOPRESTACAO_COORDENADORIAID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Coordenadoria" FieldName="COORDENADORIA" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Processo" FieldName="PROCESSO" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Número de Folhas" FieldName="NUMEROFOLHAS"
                    VisibleIndex="5">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo Prestação" FieldName="TIPO" VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Programa" FieldName="PROGRAMA" VisibleIndex="7">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data do Processo" FieldName="DATAPROCESSO"
                    VisibleIndex="9">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Detalhe" FieldName="DETALHE" VisibleIndex="11">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Exigência" FieldName="DATAEXIGENCIA"
                    VisibleIndex="12">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Tomada de Contas" FieldName="DATATOMADACONTAS"
                    VisibleIndex="13">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação Tomada de Contas" FieldName="SITUACAOTOMADACONTAS"
                    VisibleIndex="14">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Aprovação" FieldName="DATAAPROVACAO"
                    VisibleIndex="15">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Analisador" FieldName="ANALIADOR" VisibleIndex="16">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Cadastro" FieldName="DATACADASTRO" VisibleIndex="17">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Alteração" FieldName="DATAALTERACAO"
                    VisibleIndex="18" Visible="false">
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
