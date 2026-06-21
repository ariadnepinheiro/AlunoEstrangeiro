<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="AprovarLoteDebitoeCredito.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.AprovarLoteDebitoeCredito" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnAcaoChanged(ddlAcao, ddlMotivo) {
            if (ddlAcao.GetValue() == "N") //Reprovar
            {
                ddlMotivo.SetEnabled(true);
            }
            else {
                ddlMotivo.SetText("");
                ddlMotivo.SetEnabled(false);
            }
        }
       
    </script>
    <asp:HiddenField ID="hdnUnidadeEnsino" runat="server" />
    <asp:HiddenField ID="hdnPlanoTrabalho" runat="server" />
    <asp:HiddenField ID="hdnPeriodoPrestacaoContas" runat="server" />
    <asp:HiddenField ID="hdnStatus" runat="server" />
    
    
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe os dados da Pesquisa" Width="50%">
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Projeto / Programa:<span style="color: red">*</span></span>
                </td>
                <td width="600">
                      <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="VarChar" AutoPostBack="true" 
                        SqlSelect=" select DISTINCT descricao, FINALIDADE,FINALIDADEID from [PrestacaoContas].[VW_PLANOTRABANHO_CENSO] "
                        OnChanged="tsePlanoTrabalho_Changed" SqlWhere=" CENSO <> 9999999 ">
                        <GridColumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                        <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>                            
            </tr>
        </table>    
    
    
      <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Unidade de Ensino: </span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp"
                     OnChanged="tseUnidadeEnsino_Changed" 
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns><tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="11%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Período da Prestação de Contas:</span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tsePeriodoPrestacaoContas" runat="server" Key="periodoreferenciaid"
                        Argument="descricao" MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" 
                        OnChanged="tsePeriodoPrestacaoContas_Changed" 
                        SqlSelect="select periodoreferenciaid, mesinicial, mesfinal, referencia, datalimiteprestacaocontas, datalimiteanalise, descricao from prestacaocontas.vw_periodoreferencia"
                        GridWidth="850px" SqlOrder="periodoreferenciaid" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ID" FieldName="periodoreferenciaid" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Mês Inicial" FieldName="mesinicial" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Mês Final" FieldName="mesfinal" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Referência" FieldName="referencia" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim PContas" FieldName="datalimiteprestacaocontas" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim Análise" FieldName="datalimiteanalise" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
      
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:Label runat="server" ID="lblBloco" Text="Operações em lote"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnlObrigacoesFiscais" runat="server" Visible="false">
        <asp:ObjectDataSource ID="odsAprovarLoteDebitoCredito" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarLoteDebitoeCredito"
            runat="server" SelectMethod="ListaDadosGridPor" UpdateMethod="Update">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnUnidadeEnsino"          DefaultValue="" Name="UnidadeEnsino"          PropertyName="Value" />
                <asp:ControlParameter ControlID="hdnPlanoTrabalho"          DefaultValue="" Name="PlanoTrabalho"          PropertyName="Value" />
                <asp:ControlParameter ControlID="hdnPeriodoPrestacaoContas" DefaultValue="" Name="PeriodoPrestacaoContas" PropertyName="Value" />
                <asp:ControlParameter ControlID="hdnStatus"                 DefaultValue="" Name="Status"                 PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
                                    <table>
                                <tr>
                                <td>
                                <asp:Label ID="lblTotaldeOperacoesCreditotext" Text="Quantidade Total de Operações de Crédito:" runat="server" style="font-size: 10px;"></asp:Label>
                                <asp:Label ID="lblTotaldeOperacoesCredito" runat="server" style="font-size: 10px;"></asp:Label>
                                </td>
                                
                                    <td style="text-align: right;">
                                    <asp:Button ID="btnAprovar" runat="server" Text="Aprovar Todas" 
                                        OnClick="btnAprovar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                <td><asp:Label ID="lblSomaOperacoesCreditotext" Text="Valor Total Creditado no(s) projeto(s) selecionado(s), para aprovação em lote:" runat="server" style="font-size: 10px;"></asp:Label><p>
                                    <asp:Label ID="lblSomaOperacoesCredito" runat="server" style="font-size: 10px;" ></asp:Label></td>
                                    <td></td>
                                </tr>
                            </table>
        <dxwgv:ASPxGridView ClientInstanceName="grdAprovarDebitoCredito" ID="grdAprovarDebitoCredito"
            runat="server" Width="100%" DataSourceID="odsAprovarLoteDebitoCredito" KeyFieldName="OPERACAOID"
            SettingsPager-Mode="ShowAllRecords" EnableCallBacks="false">
           <settingsbehavior allowmultiselection="False" allowsort="False" />
            <settingscookies enabled="false" />
            <settingstext emptydatarow="Não existem dados." />
            <styles header-horizontalalign="Center" cell-horizontalalign="Center" cell-verticalalign="Middle" />
            <settingsediting mode="Inline" />
            <settingstext confirmdelete="Confirma a remoção?" emptydatarow="Não existem dados." />
            <settingsbehavior confirmdelete="true" />
            <columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                    <HeaderCaptionTemplate>
                    </HeaderCaptionTemplate>                   
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0" Visible="true" Caption="Cod.Operação"        FieldName="operacaoid"></dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" Visible="true" Caption="Operação"            FieldName="tipooperacao" ></dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" Visible="true" Caption="Projeto/Programa"    FieldName="planotrabalho"></dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" Visible="true" Caption="Data Envio p. Análise" FieldName="dataanalise"></dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" Visible="true" Caption="Valor da Operação"   FieldName="VALOR">
                                         <PropertiesTextEdit MaxLength="11" Width="150px" DisplayFormatString="c">
                                            <MaskSettings Mask="$&lt;0..9999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>     
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" Visible="true" Caption="Status da Operação"  FieldName="status"></dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" Visible="true" Caption="Justificativa"       FieldName="JUSTIFICATIVA"></dxwgv:GridViewDataTextColumn>                
            </columns>
        </dxwgv:ASPxGridView>
         
    </asp:Panel>
    

     
                
    <br />
   <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirma a aprovação deste lote?">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <div align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnConfirmar" runat="server" Text="Sim" OnClick="btnConfirmar_Click"
                                    OnClientClick="popup.Hide(); return true;" />
                            </td>
                            <td>
                                <asp:Button ID="btnNao" runat="server" Text="Não" OnClientClick="popup.Hide();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>

</asp:Content>
