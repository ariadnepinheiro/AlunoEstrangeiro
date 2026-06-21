<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Superintendencia.aspx.cs" Inherits="Techne.Lyceum.Net.GestaoRede.Superintendencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <div class="divEditBlock" style="width: 700px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Superintendencia" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsSuperintendencia" runat="server" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnDados" runat="server" GroupingText="Informe os dados da Superintendencia"
        Width="580px" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblSetorTSearch" runat="server" Text="Subsecretaria:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseSubsecretaria" runat="server" SqlSelect="SELECT SETOR FROM GESTAOREDE.SUBSECRETARIA"
                        AutoPostBack="false" Caption="" Key="SUBSECRETARIAID" SqlOrder="DESCRICAO" SqlWhere=" ATIVO = 1"
                        FollowContainerMode="false" MaxLength="15" DataType="Number" Argument="DESCRICAO"
                        ColumnName="SUBSECRETARIAID">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="SUBSECRETARIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="descricao" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="U.A.:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseSetor" runat="server" SqlSelect="SELECT ua_atual,nomesetor,ua_antiga,setor FROM hades..vw_setor"
                        AutoPostBack="false" SqlOrder="ua_atual" ColumnName="ua_atual" Caption="" Key="ua_atual"
                        FollowContainerMode="false" Connection="Hades" MaxLength="15" DataType="Varchar">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomesetor" Width="80%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Descrição:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescricao" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkAtivo" runat="server" Text="Ativo?" />
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        OnClick="btnSalvar_Click" />
                </td>
                <td>
                    <asp:Button ID="btnCancelarAtualizacao" runat="server" ValidationGroup="SalvarForm"
                        Text="Cancelar" OnClick="btnCancelarAtualizacao_Click" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <asp:HiddenField ID="hdnSuperintendenciaId" runat="server" />
    <br />
    <asp:ObjectDataSource ID="odsSuperintendencia" runat="server" TypeName="Techne.Lyceum.Net.GestaoRede.Superintendencia"
        SelectMethod="Lista"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdSuperintendencia" runat="server" DataSourceID="odsSuperintendencia"
        KeyFieldName="SUPERINTENDENCIAID" AutoGenerateColumns="false" ClientInstanceName="grdSuperintendencia"
        OnInitNewRow="grdSuperintendencia_InitNewRow" OnStartRowEditing="grdSuperintendencia_StartRowEditing"
        EnableCallBacks="false" OnAfterPerformCallback="grdSuperintendencia_AfterPerformCallback"
        OnCustomButtonCallback="grdSuperintendencia_CustomButtonCallback" Width="50%">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <CancelButton Visible="true" Text="Cancelar">
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="SUPERINTENDENCIAID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="SUBSECRETARIAID" Name="ID" VisibleIndex="1"
                FieldName="SUBSECRETARIAID" Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                FieldName="DESCRICAO" Width="250px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Subsecretaria*" FieldName="SUBSECRETARIA" Width="250px"
                VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="U.A.*" FieldName="UA_ATUAL" Width="50px" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="5"
                Width="50px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirma exclusão da superintendência?">
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
