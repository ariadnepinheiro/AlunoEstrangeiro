<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DetalheConsultaProtocolo.aspx.cs" Inherits="Techne.Lyceum.Net.Protocolo.DetalheConsultaProtocolo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="Panel1" runat="server" Width="800px">
        <asp:Panel ID="pnlDados" runat="server" Width="800px" GroupingText="Dados Gerais Protocolo">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Tipo de Prestação:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTipo" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblCNPJTexto" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                            Font-Bold="true" Text="CNPJ:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCNPJ" runat="server"></asp:Label>
                        <asp:Label ID="lblIdProtocolo" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label23" runat="server" Text="Ano:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAno" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="Semestre:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSemestre" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Número do Processo:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNumeroProcesso" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="Label12" runat="server" Text="Data do Processo:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDataProcesso" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="Smaller" Font-Bold="true"
                            Text="Folha:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblFolha" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Programa:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblProgramaProtocolo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label6" runat="server" Text="Observação: "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtObservacao" runat="server" MaxLength="100" Width="300" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlAnalise" runat="server" Width="800px" GroupingText="Análise">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblAnaliseId" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="Situação da Prestação:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSituacao" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label8" runat="server" Text="Data:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDataSituacao" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label13" runat="server" Text="Descrição:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescricao" TextMode="MultiLine" runat="server" Height="300px"
                            Enabled="false" Width="650px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:ObjectDataSource ID="odsProtocoloAnalise" TypeName="Techne.Lyceum.Net.Protocolo.DetalheConsultaProtocolo"
            runat="server" SelectMethod="ListarProtocoloAnalise">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblIdProtocolo" DefaultValue="" Name="idProtocolo"
                    PropertyName="Text" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdAnalise" runat="server" AutoGenerateColumns="False" EnableCallBacks="False"
                        DataSourceID="odsProtocoloAnalise" Width="900PX" ClientInstanceName="grdAnalise"
                        KeyFieldName="ANALISEID" OnCustomButtonCallback="grdAnalise_CustomButtonCallback"
                        OnAfterPerformCallback="grdAnalise_AfterPerformCallback">
                        <SettingsPager PageSize="5" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Caption=" " Width="50PX"
                                ShowInCustomizationForm="true">
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                                <CustomButtons>
                                    <dxwgv:GridViewCommandColumnCustomButton Text="Selecionar" ID="btnSelecionar" Visibility="AllDataRows"
                                        Image-Url="~/img/bt_busca.png" Image-Height="15px" Image-AlternateText="Selecionar">
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ANALISEID" VisibleIndex="0"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="IDPROGRAMA" FieldName="PROGRAMAPROTOCOLOID"
                                VisibleIndex="0" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="PROGRAMA" FieldName="PROGRAMA" VisibleIndex="0"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="OBSERVACAO" FieldName="OBSERVACAO" VisibleIndex="0"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="SITUACAOPROTOCOLOID" FieldName="SITUACAOPROTOCOLOID"
                                VisibleIndex="6" Width="100px" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="7"
                                Width="100px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data Situação" FieldName="DATASITUACAO" VisibleIndex="11"
                                Width="80px">
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAO" VisibleIndex="13"
                                Width="450px" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DESCRICAOPARCIAL" VisibleIndex="13"
                                Width="450px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIOANALISADOR" VisibleIndex="21"
                                Width="70px" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Analisador" FieldName="NOMEUSUARIOANALISADOR"
                                VisibleIndex="21" Width="100px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Revisor" FieldName="NOMEUSUARIOREVISOR" VisibleIndex="21"
                                Width="100px">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table>
        <tr>
            <td align="left">
                <asp:ImageButton ID="btnVoltar" runat="server" SkinID="Voltar" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
