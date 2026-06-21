<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AnaliseRecursosRecebidos.aspx.cs" Inherits="Techne.Lyceum.Net.Protocolo.AnaliseRecursosRecebidos" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div class="divEditBlock" style="width: 800px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancelarAnalise_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvarAnalise_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Análise" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAnalise" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
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
                        <asp:Label ID="lblTipoProtocoloId" runat="server" Visible="false"></asp:Label>
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
                        <asp:TextBox ID="txtFolha" runat="server" MaxLength="4" Width="90px" SkinID="numerico" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Programa:* " SkinID="lblObrigatorio"></asp:Label>
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
                        <asp:DropDownList ID="ddlSituacao" runat="server" DataTextField="DESCRICAO" Height="20px"
                            DataValueField="SITUACAOPROTOCOLOID" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label8" runat="server" Text="Data:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="data" runat="server" Width="150px" Enabled="true" EnableDefaultAppearance="true"
                            ClientInstanceName="data" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblAnalisador" runat="server" Font-Names="Verdana" Text="Quem Analisou:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearch ID="tseAnalisador" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryFuncionario"
                            AutoPostBack="true" OnTextChanged="tseAnalisador_Changed">
                        </tweb:TSearch>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRevisor" runat="server" Font-Names="Verdana" Text="Quem Revisou:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearch ID="tseRevisor" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryFuncionario"
                            AutoPostBack="true" OnTextChanged="tseRevisor_Changed">
                        </tweb:TSearch>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label13" runat="server" Text="Descrição:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescricao" TextMode="MultiLine" runat="server" Height="300px"
                            Width="650px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:ObjectDataSource ID="odsProtocoloAnalise" TypeName="Techne.Lyceum.Net.Protocolo.AnaliseRecursosRecebidos"
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
                                    <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarAnalise" Visibility="AllDataRows"
                                        Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ANALISEID" VisibleIndex="0"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="IDPROGRAMA" FieldName="PROGRAMAPROTOCOLOID"
                                VisibleIndex="0" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="OBSERVACAO" FieldName="OBSERVACAO" VisibleIndex="0"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="NUMEROFOLHAS" FieldName="NUMEROFOLHAS" VisibleIndex="0"
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
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIOSISTEMA" VisibleIndex="21"
                                Width="70px" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="NOMEUSUARIOSISTEMA" VisibleIndex="21"
                                Width="100px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIOANALISADOR" VisibleIndex="21"
                                Width="70px" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Analisador" FieldName="NOMEUSUARIOANALISADOR" VisibleIndex="21"
                                Width="100px">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIOREVISOR" VisibleIndex="21"
                                Width="70px" Visible="false">
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
