<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RecursosRecebidos.aspx.cs" Inherits="Techne.Lyceum.Net.Protocolo.RecursosRecebidos" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informe os dados para pesquisa."
        Width="600px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        GridWidth="500px" MaxLength="2" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                        DataType="Number">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="500px"
                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="8">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="8" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="500px"
                        OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio, id_regional , ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# ">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />							 
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="20%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="ConfirmarForm"
                        ImageUrl="~/Images/bot_buscar.png" OnClick="btnPesquisar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnPerfilCoord" runat="server" />
    <br />
    <br />
    <div class="divEditBlock" style="width: 700px;">
        <asp:Label runat="server" ID="lblBloco" Text="Prestação de Contas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsPrestacao" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlDados" runat="server" Width="900px" Visible="false">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label3" runat="server" Text="CNPJ:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblCNPJ" runat="server" Font-Names="Verdana" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Tipo de Prestação:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlTipoProtocolo" runat="server" DataTextField="descricao"
                        Height="20px" DataValueField="tipoprotocoloid" AppendDataBoundItems="true" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlTipoProtocolo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
             <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="server" Text="Programa:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlPrograma" runat="server" DataTextField="DESCRICAO" Height="20px"
                            DataValueField="PROGRAMAPROTOCOLOID" AppendDataBoundItems="true" Width="150px"
                            Enabled="false">
                        </asp:DropDownList>
                        <asp:Label ID="lblProgramaProtocoloId" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Height="20px" Width="100px" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rblSemestre" runat="server" RepeatDirection="Horizontal"
                        DataValueField="semestre" Width="200px">
                        <asp:ListItem Text="Anual" Value="Anual"></asp:ListItem>
                        <asp:ListItem Text="Trimestral" Value="Trimestral"></asp:ListItem>
                        <asp:ListItem Text="Semestral" Value="Semestral"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="tr2" runat="server">
                <td style="text-align: right;">
                    <asp:Label ID="Label7" runat="server" Text="Número do Processo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlInicialProcesso" runat="server">
                        <asp:ListItem Text="Selecione" Value="">
                        </asp:ListItem>
                        <asp:ListItem Text="E-03/" Value="E-03/">
                        </asp:ListItem>
                        <asp:ListItem Text="SEI-" Value="SEI-">
                        </asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtProcesso" runat="server" MaxLength="35" Width="300" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label12" runat="server" Text="Data do Processo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtProcesso" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtProcesso" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <calendarproperties clearbuttontext="Limpar" todaybuttontext="Hoje">
                        </calendarproperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label4" runat="server" Text="Situação da Prestação:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label16" runat="server" Text="Aguardando Análise"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="Folha:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFolha" runat="server" MaxLength="4" Width="90px" SkinID="numerico" />
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2">
                    <asp:TextBox ID="txtPrestacaoID" runat="server" Width="100px" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnSalvarPrestacao" runat="server" ValidationGroup="SalvarForm" Text="Incluir Protocolo"
                        OnClick="btnSalvarPrestacao_Click" />
                </td>
                <td>
                    <asp:Button ID="btnCancelarAtualizacao" runat="server" ValidationGroup="SalvarForm"
                        Text="Cancelar" OnClick="btnCancelarAtualizacao_Click" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <asp:ObjectDataSource ID="odsProtocolo" TypeName="Techne.Lyceum.Net.Protocolo.RecursosRecebidos"
        runat="server" SelectMethod="ListarProtocolo">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseRegional" DefaultValue="" Name="id_regional"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdPrestacao" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsProtocolo" ClientInstanceName="grdPrestacao" EnableCallBacks="false"
                        KeyFieldName="PROTOCOLOPRESTACAOID" OnAfterPerformCallback="grdPrestacao_AfterPerformCallback"
                        OnCustomButtonInitialize="grdPrestacao_CustomButtonInitialize" OnCustomButtonCallback="grdPrestacao_CustomButtonCallback">
                        <settingstext emptydatarow="Não existem dados." />
                        <columns>
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
                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="PROTOCOLOPRESTACAOID" VisibleIndex="2"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="UNIDADEENSINOID" VisibleIndex="3"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONALID" VisibleIndex="4"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Temporalidade" FieldName="TEMPORALIDADE" VisibleIndex="6">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Processo" FieldName="PROCESSO" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Número de Folhas" FieldName="NUMEROFOLHAS"
                                VisibleIndex="8">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="SITUACAOPROTOCOLOID" FieldName="SITUACAOPROTOCOLOID"
                                VisibleIndex="9" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="10">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="TIPOPROTOCOLOID" FieldName="TIPOPROTOCOLOID"
                                VisibleIndex="11" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="PROGRAMAPROTOCOLOID" FieldName="PROGRAMAPROTOCOLOID"
                                VisibleIndex="11" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tipo Prestação" FieldName="TIPO" VisibleIndex="12">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Programa" FieldName="PROGRAMA" VisibleIndex="13">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="14">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data do Processo" FieldName="DATAPROCESSO"
                                VisibleIndex="15">
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data Cadastro" FieldName="DATACADASTRO" VisibleIndex="16">
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data Alteração" FieldName="DATAALTERACAO"
                                VisibleIndex="17" Visible="false">
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIOID" VisibleIndex="18"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                        </columns>
                        <settings showfilterrow="true" showfilterrowmenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirma exclusão do processo de prestação de contas?">
        <headerstyle horizontalalign="Center" />
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <contentstyle verticalalign="Top">
        </contentstyle>
        <sizegripimage height="12px" width="12px" />
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <contentcollection>
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
        </contentcollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
