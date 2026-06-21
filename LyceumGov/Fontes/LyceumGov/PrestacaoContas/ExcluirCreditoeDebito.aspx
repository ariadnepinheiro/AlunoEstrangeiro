<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcluirCreditoeDebito.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.ExcluirCreditoeDebito" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateEditWithoutBorder
        {
            width: 100%;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
            padding-bottom: 1px;
        }
        .TSearchButton
        {
            border-width: 0px !important;
            vertical-align: top !important;
            position: relative;
            top: -1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:HiddenField runat="server" ID="hdnIdTermo" />
    <asp:HiddenField runat="server" ID="hdnDadosPublicacao" />
    <asp:HiddenField runat="server" ID="hdnAba" />
    <asp:HiddenField runat="server" ID="hdnOperacaoExigenciaId" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid2" />
    <asp:HiddenField ID="hdnArquivoId" runat="server" />
    <asp:HiddenField ID="hdnQueryString" runat="server" />
    <asp:HiddenField ID="hdnOcorrenciaId" runat="server" />
    <asp:HiddenField ID="hdnPeriodoReferencia" runat="server" />
    <asp:HiddenField ID="hdnPerfil" runat="server" />
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Programa/Projeto" Width="800px">
        <table class="table-fixed" width="800">
            <tr>
                <td align="left">
                    <span>Período da Prestação de Contas: <span style="color: red">*</span></span>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tsePeriodoPrestacaoContas" runat="server" Key="periodoreferenciaid"
                        Argument="descricao" MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true"
                        SqlSelect="select periodoreferenciaid, mesinicial, mesfinal, referencia, datalimiteprestacaocontas, datalimiteanalise, descricao from prestacaocontas.vw_periodoreferencia"
                        OnChanged="tsePeriodoPrestacaoContas_Changed" GridWidth="850px" SqlOrder="periodoreferenciaid"
                        DataType="Number">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="ID" FieldName="periodoreferenciaid" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Mês Inicial" FieldName="mesinicial" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Mês Final" FieldName="mesfinal" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Referência" FieldName="referencia" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim PContas" FieldName="datalimiteprestacaocontas" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim Análise" FieldName="datalimiteanalise" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="15%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span>Unidade de Ensino: </span>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <gridcolumns><tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="11%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span>Projeto / Programa: </span>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="VarChar" AutoPostBack="true"
                        SqlSelect=" select DISTINCT descricao, FINALIDADE,FINALIDADEID from [PrestacaoContas].[VW_PLANOTRABANHO_CENSO] "
                        OnChanged="tsePlanoTrabalho_Changed" SqlWhere=" CENSO = #tseUnidadeEnsino# ">
                        <gridcolumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                        <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="90%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span>Operação:</span>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tseOperacao" runat="server" Caption="" Key="OPERACAOID" Argument="tipo"
                        MaxLength="20" ArgumentColumns="50" Columns="10" GridWidth="850px" DataType="Number"
                        AutoPostBack="True" SqlSelect="SELECT [CENSO],[PERIODOREFERENCIAID]      ,[PLANOTRABALHOID]      ,[tipo]      ,[plano]      ,[DATACADASTRO]      ,[status]      ,[VALOR]  FROM [LYCEUM].[PrestacaoContas].[VW_OPERACAO] "
                        OnChanged="tseOperacao_Changed" SqlWhere=" PERIODOREFERENCIAID = #tsePeriodoPrestacaoContas# and CENSO = #tseUnidadeEnsino# and PLANOTRABALHOID = #tsePlanoTrabalho#"
                        Enabled="true">
                        <gridcolumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="OPERACAOID" Width="5%" />
                        <tweb:TSearchBoxColumn Caption="Operação" FieldName="tipo" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Projeto/Programa" FieldName="plano" Width="15%" />
                        <tweb:TSearchBoxColumn Caption="Data Cadastro" FieldName="DATACADASTRO" Width="15%" />
                        <tweb:TSearchBoxColumn Caption="Status" FieldName="status" Width="30%" />
                        <tweb:TSearchBoxColumn Caption="Valor" FieldName="VALOR" Width="30%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span>Status da Operação:</span>
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rbFiltroOperacao" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true">
                        <asp:ListItem Selected="True" Text="Todos" Value="9" />
                        <asp:ListItem Text="Aprovado" Value="3" />
                        <asp:ListItem Text="Reprovado" Value="4" />
                        <asp:ListItem Text="Enviado para análise" Value="1" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
   <asp:ObjectDataSource ID="odsOperacao" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ExcluirCreditoeDebito"
            SelectMethod="ListaOperacao">
            <SelectParameters>
                <asp:ControlParameter ControlID="tsePeriodoPrestacaoContas" DefaultValue="" Name="periodo" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="rbFiltroOperacao" PropertyName="SelectedValue" Name="filtroOperacao" />
                <asp:ControlParameter ControlID="tseUnidadeEnsino" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tsePlanoTrabalho" DefaultValue="" Name="plano" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tseOperacao" DefaultValue="" Name="operacao" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
    <asp:Panel ID="pnlRegistro" runat="server" Width="100%" Visible="False">
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdRegistro" runat="server" KeyFieldName="OPERACAOID" ClientInstanceName="grdRegistro"
                        AutoGenerateColumns="False" OnAfterPerformCallback="grdRegistro_AfterPerformCallback" DataSourceID="odsOperacao"
                        Width="100%" OnPageIndexChanged="grdRegistro_PageIndexChanged">
                        <settingsbehavior allowmultiselection="False" processselectionchangedonserver="true" />
                        <settingstext emptydatarow="Não existem dados." />
                        <styles commandcolumn-wrap="False" />
                        <columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                                <SelectButton Text="Selecionar" Visible="True">
                                    <Image Url="~/img/bt_busca.png" />
                                </SelectButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Censo" VisibleIndex="1" Caption="CENSO" CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="nome_comp" VisibleIndex="1" Caption="Escola">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="OPERACAOID" VisibleIndex="3" Caption="Id" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="plano" VisibleIndex="1" Caption="Projeto/Programa">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="DATACADASTRO" VisibleIndex="1" Caption="Data do Envio">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Valor" FieldName="VALOR"  VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="40">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="status" VisibleIndex="3" Caption="Status" Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="qtd" VisibleIndex="3" Caption="Total de Exigências" Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                        </columns>
                        <settings showfilterrow="True" showfilterrowmenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableViewState="false"></asp:Label>
    <br />

</asp:Content>
