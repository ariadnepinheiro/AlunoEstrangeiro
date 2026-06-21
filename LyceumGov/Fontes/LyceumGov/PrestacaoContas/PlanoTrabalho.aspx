<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanoTrabalho.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.PlanoTrabalho" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script src="../Scripts/jquery.maskedinput-1.2.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function OnEndCallBack(source) {
        }
        $(document).ready(function() {
        }); 
    </script>
    <asp:HiddenField runat="server" ID="hdnprogramatrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid2" />
    <asp:Panel runat="server" ID="Panel1" GroupingText="Informações do Programa de Trabalho"
        Width="70%">
        <table width="90%">
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Programa de Trabalho:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseProgramaTrabalho" runat="server" SqlSelect=" SELECT   PTS.PT, PTS.PTRES, PTS.UO FROM PRESTACAOCONTAS.PROGRAMATRABALHO PT  INNER JOIN PRESTACAOCONTAS.WSPROGRAMASEFAZ PTS  ON PT.WSPROGRAMASEFAZID = PTS.WSPROGRAMASEFAZID "
                        Key="PROGRAMATRABALHOID" GridWidth="600px" ArgumentColumns="50" OnChanged="tseProgramaTrabalho_Changed"
                        DataType="Number" MaxLength="10" Argument="DESCRICAO">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PROGRAMATRABALHOID" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="PT" FieldName="PT" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="PTRES" FieldName="PTRES" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="divEditBlock" style="width: 70%;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
    </div>
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Projeto / Programa" Width="70%"
        Visible="false">
        <table width="90%">
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Identificador:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblIdentificador" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblTipoContratacao" runat="server" Text="Tipo de Transferência:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbTipoDeContratacao" runat="server" DataTextField="DESCRICAO"
                        DataValueField="TIPOCONTRATACAOID" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label5" runat="server" Text="Tipo de Despesas:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbTipoDeDespesa" runat="server" DataTextField="DESCRICAO"
                        DataValueField="TIPODESPESAID" Width="457px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label6" runat="server" Text="Superintendências:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSuperintendencia" runat="server" DataTextField="DESCRICAO"
                        DataValueField="SUPERINTENDENCIAID" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Periodicidade:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbPeriodicidade" runat="server" Width="150px" AutoPostBack="True">
                        <asp:ListItem Value="0" Selected="True">Selecione</asp:ListItem>
                        <asp:ListItem Value="1">Anual</asp:ListItem>
                        <asp:ListItem Value="2">Mensal</asp:ListItem>
                        <asp:ListItem Value="3">Eventual</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Finalidade:" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbFinalidade" runat="server" DataTextField="DESCRICAO" DataValueField="FINALIDADEID"
                        Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Descrição:" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescricao" runat="server" MaxLength="800" TextMode="SingleLine"
                        Width="455px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label7" runat="server" Text="Pequenas Despesas:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPequenaDespesa" runat="server" Width="150px">
                        <asp:ListItem Value="0">Não</asp:ListItem>
                        <asp:ListItem Value="1">Sim</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
                    <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server"  SkinID="lblMensagem"></asp:Label>
    <asp:ObjectDataSource ID="odsPlanoTrabalho" TypeName="Techne.Lyceum.Net.PrestacaoContas.PlanoTrabalho"
        SelectMethod="Listar" UpdateMethod="ListaPorProgramaTrabalho" runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseProgramaTrabalho" DefaultValue="" Name="PROGRAMATRABALHOID"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnAbas" runat="server" Width="90%" Visible="true">
        <asp:Panel ID="pnlDocumentos" runat="server">
            <dxwgv:ASPxGridView ClientInstanceName="grdDocumento" ID="grdDocumento" EnableCallBacks="false"
                DataSourceID="odsPlanoTrabalho" Width="100%" KeyFieldName="ENCCEJAALUNOID" runat="server"
                OnCustomButtonCallback="grdDocumento_CustomButtonCallback" OnRowDataBound="grdDocumento_RowDataBound"
                OngrdDocumento_AfterPerformCallback="grdDocumento_AfterPerformCallback">
                <Settings ShowFilterRow="false" ShowFilterRowMenu="true" />
                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                <SettingsCookies Enabled="false" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                        <CancelButton Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                        <CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                                Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                            </dxwgv:GridViewCommandColumnCustomButton>
                            <dxwgv:GridViewCommandColumnCustomButton Text="Deletar" ID="btnDeletar" Visibility="AllDataRows"
                                Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Deletar">
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxwgv:GridViewCommandColumn>                 
                    <dxwgv:GridViewDataTextColumn Caption="PROGRAMATRABALHOID" FieldName="PROGRAMATRABALHOID"
                        VisibleIndex="1" Visible="false">
                    </dxwgv:GridViewDataTextColumn>                   
                    <dxwgv:GridViewDataTextColumn Caption="" FieldName="PLANOTRABALHOID" VisibleIndex="2"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Identificador" FieldName="IDENTIFICADOR" VisibleIndex="3"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Transferência" FieldName="TIPOCONTRATACAOID"
                        VisibleIndex="4" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Transferência" FieldName="Tipo_Descricao"
                        VisibleIndex="5" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de despesas" FieldName="TIPODESPESAID"
                        VisibleIndex="6" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de despesas" FieldName="Tipo_Despesa_Descricao"
                        VisibleIndex="7" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Programa de trabalho" FieldName="PROGRAMATRABALHO"
                        VisibleIndex="8" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Superintendência" FieldName="SUPERINTENDENCIAID"
                        VisibleIndex="9" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Superintendência" FieldName="Superintendendia_Descricao"
                        VisibleIndex="10" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAO" VisibleIndex="11"
                        Visible="true" Name="CPF">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Periodicidade" FieldName="PERIODICIDADE" VisibleIndex="12"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Periodicidade" FieldName="descricao_periodicidade"
                        VisibleIndex="13" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn VisibleIndex="7" Caption="Finalidade" Name="" FieldName="FINALIDADEID"
                        Width="100px" Visible="false" ReadOnly="true">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataDateColumn VisibleIndex="14" Caption="Finalidade" Name="" FieldName="Finalidade_Descricao"
                        Width="100px" Visible="true" ReadOnly="true">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Pequena Despesa" FieldName="PEQUENADESPESA"
                        VisibleIndex="15" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Pequena Despesa" FieldName="DESCRICAO_PEQUENADESPESA"
                        VisibleIndex="16" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </asp:Panel>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Deseja executar a operação de exclusão do Projeto / Programa?">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <div align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnExcluir" runat="server" Text="Sim" OnClick="btnExcluir_Click"
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
