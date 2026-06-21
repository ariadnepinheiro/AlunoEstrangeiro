<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="EncerrarConfirmacaoVagas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.EncerrarConfirmacaoVagas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnFiltro" GroupingText="Informe os dados para pesquisar as agendas"
        Width="600px">
        <div>
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                            Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                            AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                            ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                            ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" DataValueField="periodo"
                            AppendDataBoundItems="true" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPeriodo" runat="server" ControlToValidate="ddlPeriodo"
                            ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvPeriodoPesquisa" runat="server" ControlToValidate="ddlPeriodo"
                            ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        &nbsp;
                    </td>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdEncerrar" runat="server" AutoGenerateColumns="False" Visible="False"
                        ClientInstanceName="grdEncerrar" DataSourceID="odsAgenda" KeyFieldName="ID_AGENDA_CONF_TURNO_VAGA"
                        OnAfterPerformCallback="grdEncerrar_AfterPerformCallback">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grdEncerrar.SelectAllRowsOnPage(this.checked);" title="Select/Unselect all rows on the page" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_AGENDA_CONF_TURNO_VAGA"
                                ReadOnly="true" VisibleIndex="1" Width="60">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano/Período" FieldName="ANOPERIODO" ReadOnly="true"
                                Visible="true" VisibleIndex="2" Width="90">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="NOME" ReadOnly="true" Visible="true"
                                VisibleIndex="2">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" VisibleIndex="3"
                                Visible="true" Width="90">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                    <asp:Button ID="btnEncerrar" Visible="false" runat="server" Text="Encerrar" OnClick="btnEncerrar_Click" />
                    <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right" ToolTip="Imprimir Log de Erros do Encerramento"
                        Visible="false" OnClick="btnImprimir_Click" />
                </asp:Panel>
                <asp:ObjectDataSource ID="odsAgenda" TypeName="Techne.Lyceum.RN.CtvAgendaConfTurnoVaga"
                    runat="server" SelectMethod="ListarParaEncerramento">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
