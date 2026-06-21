<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AbsorcaoUnidadeEnsino.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AbsorcaoUnidadeEnsino" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdateDadosUnidadeEnsino" runat="server">
        <ContentTemplate>
            <br />
            <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe Unidade de Ensino de Destino para Pesquisar"
                Width="750px">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblUnidadeTSearch" runat="server" Text="Unidade de Ensino Destino:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearch ID="tseUnidade_Ensino_Destino" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAbsorcaoUnidadeEnsino"
                                AutoPostBack="true" OnTextChanged="tseUnidade_Ensino_Destino_Changed" OnLoad="tseUnidade_Ensino_Destino_Load">
                            </tweb:TSearch>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel ID="PnDadosAbosorvido" runat="server" GroupingText="Dados Unidade de Ensino Absorvida"
                Width="750px" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>                           
                            <tweb:TSearch ID="tseUnidade_Ensino_Origem" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAbsorcaoUnidadeEnsinoOrigem"
                                AutoPostBack="true" OnTextChanged="tseUnidade_Ensino_Origem_TextChanged" OnChanged="tseUnidade_Ensino_Origem_Changed" >
                            </tweb:TSearch>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Nível de Absorcão:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropNivelAbsorcao" runat="server" DataValueField="NIVELABSORCAOID"
                                AutoPostBack="true" OnSelectedIndexChanged="DropNivelAbsorcao_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Curso Absorvido:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropCursoAbsorvido" runat="server" DataValueField="CURSO" DataTextField="NOME"
                                OnSelectedIndexChanged="DropCursoAbsorvido_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Turno Absorvido:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropTurno" runat="server" DataValueField="TURNO" DataTextField="DESCRICAO"
                                OnSelectedIndexChanged="DropTurno_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Série Absorvida:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropSerie" runat="server" DataValueField="SERIE" DataTextField="SERIE"
                                OnSelectedIndexChanged="DropSerie_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Data da Absorvição:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="calendario" runat="server" MinDate="1901-01-01" Width="120px"
                                EnableDefaultAppearance="true" ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td>
                                <td>
                                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                        OnClick="btnSalvar_Click" />
                                </td>
                            </td>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <dxwgv:ASPxGridView ID="grdAbosorcaoUnidadeEnsino" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdAbosorcaoUnidadeEnsino" Font-Size="Small" Width="942px"
                DataSourceID="grdAbsorcao" KeyFieldName="SERIEABSORVIDAID" Visible="false">
                <Columns>
                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn Caption="" FieldName="SERIEABSORVIDAID" Visible="false"
                        VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nível Absorção" FieldName="NIVELABSORCAOIDDESCR"
                        VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Curso Absorvido" FieldName="CURSOORIGEMID"
                        VisibleIndex="3">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Turno Absorvido" FieldName="TURNOORIGEMID"
                        VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Série Absorvida" FieldName="SERIEORIGEMID"
                        VisibleIndex="5">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Data Absorvido" FieldName="DATAABSORCAO" VisibleIndex="6">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                        Visible="False" VisibleIndex="7">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="true" />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="grdAbsorcao" runat="server" TypeName="Techne.Lyceum.RN.AbsorcaoUnidadeEnsino"
                SelectMethod="ConsultarDadosAbsorvidos" OnDeleting="grdAbsorcao_Deleting" DeleteMethod="delete">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseUnidade_Ensino_Destino" PropertyName="Value"
                        Name="UnidadeDestino" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" runat="server" CssClass="overlay">
                <asp:Panel ID="Panel2" runat="server" CssClass="loader">
                    <asp:Image ID="Image1" runat="server" AlternateText="Updating..." Height="48" ImageUrl="~/Images/updateProgress.gif"
                        Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
