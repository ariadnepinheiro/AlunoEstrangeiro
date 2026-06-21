<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="AprovacaoDespesaEmLote.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.AprovacaoDespesaEmLote" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .qtd-despesa 
        {
            font-family: Tahoma, Geneva, sans-serif !important; 
            font-size: 12px !important; 
            font-weight: bold !important; 
            color: #0353ab !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisa." Width="620px">
        
        <asp:UpdatePanel ID="upFiltro" runat="server">
            <ContentTemplate>
            
                <table style="table-layout: fixed" width="580">
                    <tr>
                        <td width="120" align="right">
                            <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                        </td>
                        <td width="460">
                            <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                                SqlOrder="regional" Key="id_regional" 
                                SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                DataType="Number">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" 
                                SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
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
                        <td align="right">
                            <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio" runat="server" Text="Unidade de Ensino:*"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp" SqlWhere=" u.id_regional IS NOT NULL and u.id_regional = #tseRegional# and u.municipio = #tseMunicipio# "
                                SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo, u.municipio, u.id_regional,ua_atual,ua_antiga, r.regional from VW_UNIDADE_ENSINO_SITUACAO u left join TCE_REGIONAL r on u.ID_REGIONAL = r.ID_REGIONAL ">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                            
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="30%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            
            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>

    <asp:UpdatePanel ID="upMensagem" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="plaMensagem" runat="server" Visible="false" EnableViewState="false">
            <br />
            <br />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false" SkinID="lblMensagem"></asp:Label>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />
    <br />
    
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
            <div id="plaGrid" runat="server" style="display: none">
            
                <asp:ObjectDataSource ID="odsEvento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovacaoDespesaEmLote" SelectMethod="ListaDespesa">
                    <SelectParameters>
                        <asp:Parameter Name="censo" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                
                <div style="height: 25px; margin-left: 10px;" class="qtd-despesa">
                    <span>Qtd. de Despesas: </span><asp:Label ID="lblQtd" runat="server" Text="0" CssClass="qtd-despesa"></asp:Label>
                </div>
                <div style="height: 25px; margin-left: 10px;" class="qtd-despesa">
                    <span style="float: left; margin-top: 2px; margin-right: 20px;">
                        <span>Qtd. de Despesas Selecionadas: </span><asp:Label ID="lblQtdSel" runat="server" Text="0" CssClass="qtd-despesa"></asp:Label>
                    </span>
                    <dxe:ASPxComboBox ID="cmbFilterMode" runat="server" ValueType="System.String" SelectedIndex="0">
                        <Items>
                            <dxe:ListEditItem Text="Mostrar Todos" Value="All" Selected="true" />
                            <dxe:ListEditItem Text="Mostrar Selecionados" Value="Selected" />
                            <dxe:ListEditItem Text="Mostrar Não Selecionados" Value="UnSelected" />
                        </Items>
                        <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
                    </dxe:ASPxComboBox>
                </div>
                
                <dxwgv:ASPxGridView ID="grdDespesas" runat="server" DataSourceID="odsEvento"
                    ClientInstanceName="grdDespesas" AutoGenerateColumns="False"
                    Width="950px" KeyFieldName="EVENTOID" EnableCallBacks="false"
                    OnSelectionChanged="grdDespesas_SelectionChanged" OnCustomCallback="grdDespesas_CustomCallback">
                    
                    <SettingsBehavior AllowMultiSelection="true" AllowFocusedRow="false" ProcessSelectionChangedOnServer="true" />
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsPager PageSize="10" />
                    
                    <Columns>
                        <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" Caption="Selecionar" VisibleIndex="0"></dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Num. Evento" FieldName="NUMEROEVENTO" VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Finalidade" FieldName="FINALIDADE" VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Tipo Despesa" FieldName="TIPODESPESA" VisibleIndex="3">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Dt. Despesa" FieldName="DATAEVENTO" VisibleIndex="4">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Dt. NF" FieldName="DATANOTAFISCAL" VisibleIndex="5">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Dt. Pgto" FieldName="DATAPAGAMENTO" VisibleIndex="6">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Exigências" FieldName="TOTALEXIGENCIAS" VisibleIndex="7">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="" FieldName="FINALIDADEID" VisibleIndex="8" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                
                </dxwgv:ASPxGridView>
                
                <br /><br />
                
                <asp:Button ID="btnAprovarSelecionadas" runat="server" Text="Aprovar Selecionadas" OnClick="btnAprovarSelecionadas_Click" style="margin-right: 10px;" />
                <asp:Button ID="btnAprovarTodas" runat="server" Text="Aprovar Todas" OnClick="btnAprovarTodas_Click" />
            
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
        
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..." Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <script type="text/javascript">
        function OnSelectedIndexChanged(s, e) {
            grdDespesas.PerformCallback(s.GetValue());
        }
    </script>

</asp:Content>
