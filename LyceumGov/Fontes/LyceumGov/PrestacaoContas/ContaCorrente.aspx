<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="ContaCorrente.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ContaCorrente" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script src="../Scripts/js/min/jquery.maskedinput.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function OnEndCallBack(source) {

        }

        $(document).ready(function() {


            $("#ctl00_cphFormulario_txtDataInicio").mask("99/99/9999", { placeholder: 'dd/mm/yyyy' });
            $("#ctl00_cphFormulario_txtDataFim").mask("99/99/9999", { placeholder: 'dd/mm/yyyy' });
            $("#txtDataInicio").mask("99/99/9999", { placeholder: 'dd/mm/yyyy' });
            $("#txtDataFim").mask("99/99/9999", { placeholder: 'dd/mm/yyyy' });

        });

        function Desseleciona() {
            $("#ctl00_cphFormulario_rbStatusSituacao_0").click(function() {

                if ($('#ctl00_cphFormulario_rbStatusSituacao_0').is(':checked') == true) {
                    $('#ctl00_cphFormulario_rbStatusSituacao_0').is(':checked') = false;
                }
            });
        }             
        

    </script>

    <asp:Panel runat="server" ID="pnlTipoFiltro" GroupingText="Informe os dados para pesquisar as contas corrente"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblFiltro" runat="server" Text="Filtros:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" RepeatDirection="Horizontal"
                        Width="254px" AutoPostBack="true" OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Por Regional" Value="R"></asp:ListItem>
                        <asp:ListItem Text="Por Unidade de Ensino" Value="U"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlFiltro" Width="800px" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseRegional" runat="server" SqlSelect="select distinct regional, descricao from (select distinct R.ID_REGIONAL as regional, R.REGIONAL as descricao from VW_UNIDADE_ENSINO_SITUACAO uuf
                                join LY_UNIDADE_ENSINO ue on uuf.UNIDADE_ENS = ue.UNIDADE_ENS
                                join TCE_REGIONAL R on R.ID_REGIONAL = ue.ID_REGIONAL) as tabela" GridWidth="600px"
                            ArgumentColumns="50" Columns="10" MaxLength="10" OnChanged="tseRegional_Changed"
                            OnLoad="tseRegional_Load" DataType="Number" Key="regional">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="descricao" Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <asp:Panel runat="server" ID="pnlUnidade" Width="800px" Visible="false">
                        <td style="text-align: right; width: 200px">
                            <asp:Label Font-Names="Verdana" ID="lblUA" runat="server" Text="Unidade Escolar:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="3">
                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                EnableViewState="true" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio, id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                                ArgumentColumns="75" Columns="10" OnChanged="tseUnidadeResponsavel_Changed" GridWidth="850px"
                                SqlOrder="nome_comp" SqlWhere=" id_regional = #tseRegional# " OnLoad="tseUnidadeResponsavel_Load">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="15%" />
                                    <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="15%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </asp:Panel>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
        <asp:Label runat="server" ID="lblBlocoConta" Text="Conta Corrente" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsContar" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:HiddenField ID="hdnContaCorrente" runat="server" />
    <asp:Panel ID="pnlContaCorrente" runat="server" GroupingText="Dados da Conta Corrente"
        Width="700px">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label9" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                        Text="Banco:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseBanco" runat="server" Key="banco" Argument="nome" SqlSelect="SELECT banco, nome from BANCOS (nolock)"
                        SqlOrder="nome" AutoPostBack="true" OnChanged="tseBanco_Changed" MaxLength="20"
                        OnLoad="tseBanco_Load" GridWidth="850px" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Banco" FieldName="banco" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="40%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label11" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                        Text="Agência:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseAgencia" runat="server" Key="agencia" Argument="nome" OnChanged="tseAgencia_Changed"
                        MaxLength="20" GridWidth="850px" SqlSelect="select agencia, banco, nome from AGENCIAS (nolock) "
                        SqlWhere=" banco = #tseBanco# " OnLoad="tseBanco_Load">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Agência" FieldName="agencia" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Banco" FieldName="banco" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblConta" runat="server" Text="Conta:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConta" runat="server" Width="100px" MaxLength="10" SkinID="numerico"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Data Inicio:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Data Fim:"></asp:Label>
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
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="odsContaCorrente" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ContaCorrente"
        SelectMethod="Lista" UpdateMethod="Update" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" DefaultValue="DBValue" Name="regionalId" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="DBValue" Name="censo" />
            <asp:ControlParameter ControlID="rblTipoFiltro" Name="filtro" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdContaCorrente" runat="server" DataSourceID="odsContaCorrente"
        EnableCallBacks="false" KeyFieldName="CONTACORRENTEID" AutoGenerateColumns="false"
        ClientInstanceName="grdContaCorrente" OnInitNewRow="grdContaCorrente_InitNewRow"
        OnStartRowEditing="grdContaCorrente_StartRowEditing" OnAfterPerformCallback="grdContaCorrente_AfterPerformCallback"
        OnCustomButtonCallback="grdContaCorrente_CustomButtonCallback" OnCustomButtonInitialize="grdContaCorrente_CustomButtonInitialize"
        Width="850px">
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Styles CommandColumn-Wrap="False" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarCustom" Visibility="AllDataRows"
                        Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                    </dxwgv:GridViewCommandColumnCustomButton>
                    <dxwgv:GridViewCommandColumnCustomButton Text="Deletar" ID="btnDeletar" Visibility="AllDataRows"
                        Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Deletar">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CONTACORRENTEID"
                Visible="false" Width="700px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nº do Banco" Name="BANCO" VisibleIndex="1"
                FieldName="BANCO" Width="300px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome do Banco" Name="NOMEBANCO" VisibleIndex="1"
                FieldName="NOMEBANCO" Width="700px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Agência" Name="AGENCIA" VisibleIndex="2" FieldName="AGENCIA"
                Width="400px" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Conta" Name="CONTA" VisibleIndex="3" FieldName="CONTA"
                Width="400px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Início" Name="DATAINICIO" VisibleIndex="3"
                FieldName="DATAINICIO" Width="400px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Fim" Name="DATAFIM" VisibleIndex="3"
                FieldName="DATAFIM" Width="400px">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Deseja executar a operação de exclusão da conta corrente?">
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
