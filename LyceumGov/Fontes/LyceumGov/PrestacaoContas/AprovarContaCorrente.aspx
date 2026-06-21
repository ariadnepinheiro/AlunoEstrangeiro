<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="AprovarContaCorrente.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.AprovarContaCorrente" %>

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

        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }

        function Desseleciona() {
            $("#ctl00_cphFormulario_rbStatusSituacao_0").click(function() {

                if ($('#ctl00_cphFormulario_rbStatusSituacao_0').is(':checked') == true) {
                    $('#ctl00_cphFormulario_rbStatusSituacao_0').is(':checked') = false;
                }
            });
        }        

    </script>

    <asp:Panel runat="server" ID="pnlTipoFiltro" GroupingText="Informe os dados para pesquisa da conta corrente"
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
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label1" runat="server" Text="Situação:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblSituacao" runat="server" RepeatDirection="Horizontal"
                        Width="254px" AutoPostBack="true" OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Aprovado" Value="Aprovado"></asp:ListItem>
                        <asp:ListItem Text="Reprovado" Value="Reprovado"></asp:ListItem>
                        <asp:ListItem Text="Pendente" Value="Pendente"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:HiddenField ID="hdnContaCorrente" runat="server" />
    <br />
    <asp:ObjectDataSource ID="odsContaCorrente" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarContaCorrente"
        SelectMethod="ListaDados">
        <SelectParameters>
            <asp:ControlParameter ControlID="rblSituacao" Name="filtro" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="rblTipoFiltro" Name="tipo" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMotivo" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarContaCorrente"
        SelectMethod="ListaMotivo">
        <SelectParameters>
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdAprovarContaCorrente" runat="server" DataSourceID="odsContaCorrente"
        EnableCallBacks="false" Visible="true" KeyFieldName="CONTACORRENTEID" AutoGenerateColumns="false"
        ClientInstanceName="grdAprovarContaCorrente" OnInitNewRow="grdAprovarContaCorrente_InitNewRow"
        OnStartRowEditing="grdAprovarContaCorrente_StartRowEditing" OnAfterPerformCallback="grdAprovarContaCorrente_AfterPerformCallback"
        OnCustomButtonCallback="grdAprovarContaCorrente_CustomButtonCallback" OnCustomButtonInitialize="grdAprovarContaCorrente_CustomButtonInitialize"
        Width="850px">
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                <CancelButton Visible="true" Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <UpdateButton Visible="true" Text="Alterar">
                    <Image Url="../img/bt_salvar.png" />
                </UpdateButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CONTACORRENTEID"
                Visible="false" Width="700px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Regional" Name="REGIONAL" VisibleIndex="1"
                FieldName="REGIONAL" Width="300px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Censo" Name="CENSO" VisibleIndex="1" FieldName="CENSO"
                Width="700px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" Name="NOME_COMP" VisibleIndex="1"
                FieldName="NOME_COMP" Width="700px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Banco" Name="BANCO" VisibleIndex="2" FieldName="BANCO"
                Width="400px" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Agencia" Name="AGENCIA" VisibleIndex="3" FieldName="AGENCIA"
                Width="400px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Conta" Name="CONTA" VisibleIndex="3" FieldName="CONTA"
                Width="400px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn VisibleIndex="4" Caption="Data Início" Name="DATAINICIO"
                FieldName="DATAINICIO" Width="100px" Visible="true" ReadOnly="true">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                </PropertiesDateEdit>
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn VisibleIndex="5" Caption="Data Fim" Name="DATAFIM"
                FieldName="DATAFIM" Width="100px" Visible="true" ReadOnly="true">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                  
                </PropertiesDateEdit>
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Situação" Name="situacao" VisibleIndex="6"
                FieldName="situacao" Width="10px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Motivo Reprovação" Name="MOTIVOREPROVACAOCONTACORRENTEID"
                VisibleIndex="7" FieldName="MOTIVOREPROVACAOCONTACORRENTEID" Width="10px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="8" ButtonType="Link" Width="50px" Caption="Aceitar">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnAceitar" Text="Aceitar" Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="7" ButtonType="Link" Width="50px" Caption="Reprovar">
                <CustomButtons>
                    <dxwgv:GridViewCommandColumnCustomButton ID="btnReprovar" Text="Reprovar" Visibility="AllDataRows">
                    </dxwgv:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dxwgv:GridViewCommandColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="300px">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblConfirmar" runat="server" Text="Selecione o motivo da reprovação:"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:DropDownList ID="cmbMotivo" runat="server" DataValueField="MOTIVOREPROVACAOCONTACORRENTEID"
                                DataTextField="DESCRICAO" DataSourceID="odsMotivo">
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btConfirma" runat="server" Text="Confirma" OnClick="btConfirma_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide();" />
                            <asp:Label ID="hID" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
