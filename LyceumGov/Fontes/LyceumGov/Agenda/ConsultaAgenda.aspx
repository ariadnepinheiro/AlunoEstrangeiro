<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConsultaAgenda.aspx.cs" Inherits="Techne.Lyceum.Net.Agenda.ConsultaAgenda"
    Title="Agenda de eventos" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar Agenda"
                Width="600px">
                <div>
                    <table width="600px" style="text-align: left">
                        <tr>
                            <td style="text-align: right; width: 30px;">
                                <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="a"
                                    Font-Bold="true">                                   
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 128px">
                                <asp:DropDownList Height="20px" ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                                    DataTextField="ANO" DataValueField="ANO" SkinID="a" Width="100px" ValidationGroup="ConfirmarForm">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; width: 70px">
                                <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Período:*" SkinID="a"
                                    Font-Bold="true">                                   
                                </asp:Label>
                            </td>
                            <td style="text-align: right; width: 100px">
                                <asp:CheckBoxList ID="cblperiodo" runat="server" DataTextField="PERIODO" DataValueField="ID_REDUZIDA"
                                    RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </td>
                            <td style="text-align: right;">
                                <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click"
                                    ImageUrl="Images/bot_buscar.png" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <asp:Label ID="lblMensagem" runat="server" Visible="true" SkinID="lblMensagem"></asp:Label>
            <br />
            <dxwgv:ASPxGridView ID="grdConsultaAgenda" runat="server" KeyFieldName="AGENDAID"
                Visible="False" ClientInstanceName="grdConsultaAgenda" AutoGenerateColumns="False"
                Width="90%">
                <SettingsBehavior AllowMultiSelection="False" AllowSort="true" ProcessSelectionChangedOnServer="true" />
                <Styles CommandColumn-Wrap="False">
                    <CommandColumn Wrap="False">
                    </CommandColumn>
                </Styles>
                <Columns>
                    <dxwgv:GridViewDataColumn VisibleIndex="2" Width="30" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <asp:ImageButton ID="imgBtnSelecionar" runat="server" CommandArgument='<%#Eval("AGENDAID")%>'
                                OnClick="SelecionaRow" ImageUrl="~/img/bt_busca.png" />
                        </DataItemTemplate>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Ano" FieldName="ANO" Width="90"
                        HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Período" FieldName="PERIODO"
                        Width="90" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Evento" FieldName="TIPOEVENTO" VisibleIndex="3"
                        Width="250px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn VisibleIndex="4" Caption="Descrição" FieldName="NOMEAGENDA"
                        Width="280" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn VisibleIndex="5" Caption="Data Início" FieldName="DATAINICIO"
                        Width="100" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                        <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn VisibleIndex="6" Caption="Data Fim" FieldName="DATAFIM"
                        Width="100" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                        <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left">
                        </CellStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                        Visible="False" VisibleIndex="7">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="AGENDAID" VisibleIndex="8" Caption="AGENDAID"
                        Visible="false" Width="0px">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Carregando..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
