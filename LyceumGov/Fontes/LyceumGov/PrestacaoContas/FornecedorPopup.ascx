<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FornecedorPopup.ascx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.FornecedorPopup" %>

<style type="text/css">
.conteudo {
    overflow: inherit;
}
</style>

<dxpc:ASPxPopupControl ID="pcPopup" runat="server" Modal="True" Width="600" Height="350"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
    ClientInstanceName="pcPopup" HeaderText="Aviso" AllowDragging="True"
    EnableAnimation="False" EnableViewState="True">
    <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
    <ContentCollection>
        <dxpc:PopupControlContentControl ID="ppPopup" runat="server">
            <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <dxp:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnProsseguir">
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent3" runat="server">
                                
                                <%--<asp:PlaceHolder ID="plaDocumentosVencidos" runat="server" Visible="false">--%>
                                <table>
                                    <tr>
                                        <td valign="middle">
                                            <img src="../images/img_interrog.png" width="60" height="60" align="left" />
                                        </td>
                                        <td valign="middle">
                                            <strong style="font-size: large; color: #00f">Caro Diretor, atenção ao vencimento dos documentos.</strong><br />
                                            <%--<div>As vagas <b>RESERVADAS</b>, ficam temporariamente para uso exclusivo da escola.</div>--%>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:HiddenField ID="hdnFornecedorId" runat="server" />
                                            <dxwgv:ASPxGridView ID="grdDocumentos" runat="server" KeyFieldName="DOCUMENTOSFORNECEDORID"
                                                ClientInstanceName="grdDocumentos" EnableCallBacks="false" AutoGenerateColumns="False"
                                                Width="95%" Font-Names="Verdana" Font-Size="Small">
                                                <SettingsText EmptyDataRow="Não existem dados." />
                                                <SettingsPager PageSize="10" />
                                                <Columns>
                                                    <dxwgv:GridViewDataColumn FieldName="DESCRICAO" Caption="Descrição" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="PERIODICIDADE" Caption="Periodicidade" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="DATAINICIO" Caption="Dt. Ini" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="DATAFIM" Caption="Dt. Fim" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="EXPIRADO" Caption="Expirado?" VisibleIndex="0" />
                                                </Columns>
                                            </dxwgv:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                                <%--</asp:PlaceHolder>--%>
                                
                                <table>                                        
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnProsseguir" Width="100px" runat="server" Text="OK" OnClientClick="pcPopup.Hide(); return false;" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                
                            </dxp:PanelContent>
                        </PanelCollection>
                    </dxp:ASPxPanel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </dxpc:PopupControlContentControl>
    </ContentCollection>
    <ContentStyle>
        <Paddings PaddingBottom="5px" />
    </ContentStyle>
</dxpc:ASPxPopupControl>