<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AcompanhamentoTransferenciaPatrimonio.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.AcompanhamentoTransferenciaPatrimonio" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">

        function desabilitarBotoes() {
            $("#<%= this.btnSalvar.ClientID %>").attr("disabled", true);
            $("#<%= this.btnSalvar.ClientID %>").val("Aguarde...");

            $("#<%= this.btnAceitarTodas.ClientID %>").attr("disabled", true);
            $("#<%= this.btnAceitarTodas.ClientID %>").val("Aguarde...");

            $("#<%= this.btnRecusarTodas.ClientID %>").attr("disabled", true);
            $("#<%= this.btnRecusarTodas.ClientID %>").val("Aguarde...");
        }
        
        function controlarRecusa(radioButton, limparTexto) {
            var txtJustificativa = $("#" + $(radioButton).attr("txtJustificativa"));

            $(txtJustificativa).removeAttr("readonly");
            $(txtJustificativa).removeAttr("disabled");
            $(txtJustificativa).css("background-color", "");

            if (limparTexto) {
                $(txtJustificativa).val("");
            }
        }

        function controlarAceite(radioButton) {
            var txtJustificativa = $("#" + $(radioButton).attr("txtJustificativa"));

            $(txtJustificativa).attr("readonly", "readonly");
            $(txtJustificativa).attr("disabled", true);
            $(txtJustificativa).css("background-color", "Gainsboro");
            $(txtJustificativa).val("");

        }

        function abrirPopup() {

            window.setTimeout(function() {
                window.scrollTo(0, 0);
                pucConfirmar.Show();
            }, 1000);
        }

        function abrirPopupRecusados() {

            window.setTimeout(function() {
                pucConfirmarRecusados.Show();
            }, 1000);
        }

        function atualizarGrid() {
            $("input[id*='rbAceitar']:checked").each(function() {
                controlarAceite(this, true, false);
            });

            $("input[id*='rbRecusar']:checked").each(function() {
                controlarRecusa(this, false);
            });
        }

        $(document).ready(function() {

            $("input[id*='rbAceitar']").click(function() {
                controlarAceite(this);
            });

            $("input[id*='rbRecusar']").click(function() {
                controlarRecusa(this, true);
            });
        });
    </script>

    <dxtc:ASPxPageControl ID="pcTransferencia" runat="server" ActiveTabIndex="1" OnTabClick="pcTransferencia_TabClick"
        Width="90%">
        <TabPages>
            <dxtc:TabPage Text="Solicitação de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Acompanhamento de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Unidade Administrativa:" Width="100%">
                            <table>
                                <tr>
                                    <td style="text-align: right; width: 10%">
                                        <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                                            Text="Unidade Administrativa:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearch ID="tseUA" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                                            AutoPostBack="true" OnTextChanged="tseUA_Changed" Width="575px">
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        <asp:Panel ID="pnlGrid" runat="server" Width="100%">
                            <asp:Panel ID="pnSolicitacao" runat="server" GroupingText="Transferências realizadas pela Unidade">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right; width: 110px">
                                            <asp:Label ID="lblStatusSolic" runat="server" Text="Exibir transferências:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatusSolicPelaUE" runat="server" AppendDataBoundItems="True"
                                                AutoPostBack="True" Height="16px" DataTextField="SITUACAO" DataValueField="SITUACAO">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ObjectDataSource ID="odsAcompanhamentoOrigem" TypeName="Techne.Lyceum.Net.Patrimonio.AcompanhamentoTransferenciaPatrimonio"
                                                runat="server" SelectMethod="ListaOrigem">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="tseUA" DefaultValue="" Name="setor" PropertyName="DBValue" />
                                                    <asp:ControlParameter ControlID="ddlStatusSolicPelaUE" PropertyName="SelectedValue"
                                                        Name="situacao" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <dxwgv:ASPxGridView ID="grdPatrimonioSolicPelaUE" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdPatrimonioSolicPelaUE" DataSourceID="odsAcompanhamentoOrigem"
                                                KeyFieldName="TRANSFERENCIAITEMID" OnCustomUnboundColumnData="grdPatrimonioSolicPelaUE_CustomUnboundColumnData">
                                                <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                                <Columns>
                                                    <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="TRANSFERENCIAITEMID" ReadOnly="true"
                                                        VisibleIndex="0" Visible="false">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Código Bem" FieldName="BEMID" ReadOnly="true"
                                                        VisibleIndex="0" Visible="false">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Lote" FieldName="TRANSFERENCIAID" ReadOnly="true"
                                                        VisibleIndex="1" Width="10px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Número" UnboundType="String" FieldName="NUMERO"
                                                        ReadOnly="true" VisibleIndex="2" Width="10px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="BEM" Caption="Patrimônio" ReadOnly="true"
                                                        VisibleIndex="3">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Código Classificação" FieldName="CONTA" ReadOnly="true"
                                                        VisibleIndex="3">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Classificação" FieldName="CLASSIFICACAO" ReadOnly="true"
                                                        VisibleIndex="4">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Conservação" FieldName="ESTADOCONSERVACAO"
                                                        ReadOnly="true" VisibleIndex="5">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Destino" FieldName="SETORDESTINODESCRICAO"
                                                        ReadOnly="true" VisibleIndex="6">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="SIGLA" ReadOnly="true" Visible="false" VisibleIndex="7">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="VALOR" ReadOnly="true" VisibleIndex="8"
                                                        Visible="false" Caption="Valor " Width="20px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALORCOMSIGLA" UnboundType="String"
                                                        VisibleIndex="8" Name="Valor">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="SITUACAO" ReadOnly="true" Caption="Situação"
                                                        VisibleIndex="9">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" ReadOnly="true"
                                                        Visible="true" VisibleIndex="10">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="DATAMOVIMENTACAO" ReadOnly="true" Caption="Data da Transferência"
                                                        VisibleIndex="11">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data de Solicitação" FieldName="DATASOLICITACAO"
                                                        ReadOnly="true" Visible="true" VisibleIndex="12">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data Andamento" FieldName="DATAANDAMENTO"
                                                        ReadOnly="true" Visible="true" VisibleIndex="13">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="Panel2" runat="server" GroupingText="Transferências realizadas para Unidade">
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2" style="text-align: left;">
                                            <asp:Label ID="lblMensagemLoteDisponível" runat="server" SkinID="lblMensagem"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 110px">
                                            <asp:Label ID="lblStatusSolicParaUE" runat="server" Text="Exibir transferências:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatusSolicParaUE" runat="server" AppendDataBoundItems="True"
                                                OnSelectedIndexChanged="ddlStatusSolicParaUE_SelectedIndexChanged" AutoPostBack="True"
                                                Height="16px" DataTextField="SITUACAO" DataValueField="SITUACAO">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlLote" runat="server" Visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="text-align: right; width: 110px">
                                                <asp:Label Font-Names="Verdana" ID="lblLote" runat="server"
                                                    Text="Lote:"></asp:Label>
                                            </td>
                                            <td>
                                                <tweb:TSearchBox ID="tseLote" runat="server" SqlSelect="select transferenciaid , (s.nome + ' - ' + convert(varchar,t.datasolicitacao,103)) as descricao  from patrimonio.transferencia t inner join hades.dbo.hd_setor s on s.setor=t.setororigem "
                                                    SqlOrder="transferenciaid" ColumnName="transferenciaid" Key="transferenciaid"
                                                    Argument="descricao" Caption="" MaxLength="15" SqlWhere=" dataandamento is nulL and setordestino is null "
                                                    DataType="Number" OnChanged="tseLote_Changed">
                                                    <GridColumns>
                                                        <tweb:TSearchBoxColumn Caption="Lote" FieldName="transferenciaid" Width="20%" />
                                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                                    </GridColumns>
                                                </tweb:TSearchBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label Font-Names="Verdana" ID="lblLoteDescricao" SkinID="lblObrigatorio" runat="server"
                                                    Text="(Número do Lote - Unidade Administrativa - Data Solicitação Transferência)"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label Font-Names="Verdana" ID="lblDataTransferencia" SkinID="lblObrigatorio"
                                                    runat="server" Text="Data da Transferência:*"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtDataTransferencia" runat="server" Width="100px" Enabled="true"
                                                    EnableDefaultAppearance="true" ClientInstanceName="dtDataTransferencia" CalendarProperties-ClearButtonText="Limpar"
                                                    CalendarProperties-TodayButtonText="Hoje" OnPreRender="dtDataTransferencia_PreRender">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ObjectDataSource ID="odsAcompanhamentoDestino" TypeName="Techne.Lyceum.Net.Patrimonio.AcompanhamentoTransferenciaPatrimonio"
                                                runat="server" SelectMethod="ListaDestino">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="tseUA" DefaultValue="" Name="setor" PropertyName="DBValue" />
                                                    <asp:ControlParameter ControlID="ddlStatusSolicParaUE" PropertyName="SelectedValue"
                                                        Name="situacao" />
                                                    <asp:ControlParameter ControlID="tseLote" DefaultValue="" Name="lote" PropertyName="DBValue" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <dxwgv:ASPxGridView ID="grdPatrimonioSolicParaUE" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdPatrimonioSolicParaUE" DataSourceID="odsAcompanhamentoDestino"
                                                KeyFieldName="TRANSFERENCIAITEMID" OnHtmlRowCreated="grdPatrimonioSolicParaUE_HtmlRowCreated"
                                                OnCustomUnboundColumnData="grdPatrimonioSolicParaUE_CustomUnboundColumnData">
                                                <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                                <SettingsPager Mode="ShowAllRecords" />
                                                <Columns>
                                                    <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="TRANSFERENCIAITEMID" ReadOnly="true"
                                                        VisibleIndex="1" Visible="false">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Lote" FieldName="TRANSFERENCIAID" ReadOnly="true"
                                                        VisibleIndex="1" Width="10px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Número" UnboundType="String" FieldName="NUMERO"
                                                        ReadOnly="true" VisibleIndex="2" Width="10px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="BEM" Caption="Patrimônio" ReadOnly="true"
                                                        VisibleIndex="3">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Código Classificação" FieldName="CONTA" ReadOnly="true"
                                                        VisibleIndex="4">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Classificação" FieldName="CLASSIFICACAO" ReadOnly="true"
                                                        VisibleIndex="5">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Conservação" FieldName="ESTADOCONSERVACAO"
                                                        ReadOnly="true" VisibleIndex="6">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Origem" FieldName="SETORORIGEMDESCRICAO"
                                                        ReadOnly="true" VisibleIndex="7">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="MOEDAID" ReadOnly="true" Visible="false"
                                                        VisibleIndex="7">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="SIGLA" ReadOnly="true" Visible="false" VisibleIndex="7">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="VALOR" ReadOnly="true" VisibleIndex="8"
                                                        Visible="false" Caption="Valor " Width="20px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALORCOMSIGLA" UnboundType="String"
                                                        VisibleIndex="8" Name="Valor">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Situação*" Name="ANDAMENTO" VisibleIndex="9" Width="100px">
                                                        <DataItemTemplate>
                                                            <asp:RadioButton ID="rbAceitar" runat="server" GroupName='<%# Bind("TRANSFERENCIAITEMID") %>'
                                                                Text="Aceitar" /><br />
                                                            <asp:RadioButton ID="rbRecusar" runat="server" GroupName='<%# Bind("TRANSFERENCIAITEMID") %>'
                                                                Text="Recusar" />
                                                        </DataItemTemplate>
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Justificativa" Name="JUSTIFICATIVA" FieldName="JUSTIFICATIVA"
                                                        VisibleIndex="10">
                                                        <DataItemTemplate>
                                                            <asp:TextBox ID="txtJustificativa" runat="server" Text='<%# Bind("JUSTIFICATIVA") %>' />
                                                        </DataItemTemplate>
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataTextColumn FieldName="DATAMOVIMENTACAO" ReadOnly="true" Caption="Data da Transferência"
                                                        VisibleIndex="11">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data Solicitação" FieldName="DATASOLICITACAO"
                                                        VisibleIndex="12">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data Andamento" FieldName="DATAANDAMENTO"
                                                        VisibleIndex="13">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                                OnClick="btnSalvar_Click" OnClientClick="desabilitarBotoes();"
                                                UseSubmitBehavior="false" />
                                            <asp:Button ID="btnAceitarTodas" runat="server" ValidationGroup="SalvarForm" Text="Aceitar Todas"
                                                OnClick="btnAceitarTodas_Click" OnClientClick="desabilitarBotoes();"
                                                UseSubmitBehavior="false" />
                                            <asp:Button ID="btnRecusarTodas" runat="server" ValidationGroup="SalvarForm" Text="Recusar Todas"
                                                OnClick="btnRecusarTodas_Click" OnClientClick="desabilitarBotoes();"
                                                UseSubmitBehavior="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Histórico de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        EnableViewState="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" ShowPageScrollbarWhenModal="false" Width="400px" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblMensagemPopup" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="trAceitos">
                        <td>
                            <asp:Label ID="lblAceitos" runat="server" Text="Aceitos:"></asp:Label>
                            <br />
                            <asp:BulletedList ID="blAceitos" runat="server">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr runat="server" id="trRecusados">
                        <td>
                            <asp:Label ID="lblRecusados" runat="server" Text="Recusados:"></asp:Label>
                            <br />
                            <asp:BulletedList ID="blRecusados" runat="server">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblConfirmar" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click"
                                OnClientClick="this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarRecusados" ClientInstanceName="pucConfirmarRecusados"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" EnableViewState="false" ShowFooter="false" ShowHeader="false"
        ShowSizeGrip="False" EnableAnimation="false" ShowPageScrollbarWhenModal="false"
        Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Justificativa:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtJustificativa" MaxLength="500" runat="server" TextMode="MultiLine"
                                Height="88px" Width="239px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnConfirmarRecusados" runat="server" Text="Confirmar" OnClick="btnConfirmarRecusados_Click"
                                OnClientClick="this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                            <asp:Button ID="btnCancelarRecusados" runat="server" Text="Cancelar" OnClientClick="pucConfirmarRecusados.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
