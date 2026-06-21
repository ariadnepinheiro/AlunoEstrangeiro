<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupA.ascx.cs" Inherits="Techne.Lyceum.Net.Menu.PopupA" %>
<style type="text/css">
.conteudo {
    overflow: inherit;
}
</style>
<dxpc:ASPxPopupControl ID="pcPopup" runat="server" Modal="True" Width="600px" Height="350" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
    ClientInstanceName="pcPopup" HeaderText="Saeb" AllowDragging="True" EnableAnimation="False"
    EnableViewState="True" ShowHeader="True"
    ShowCloseButton="True" >   
      <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
     <ContentStyle>
        <Paddings Padding="0px" />
    </ContentStyle>   
    <ContentCollection>
        <dxpc:PopupControlContentControl ID="ppPopup" runat="server">
            <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <dxp:ASPxPanel ID="ASPxPanel2" runat="server"  >
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent3" runat="server">
                                <asp:PlaceHolder ID="PlaCHTC2" runat="server">
                                    <asp:Image ID="imgSaeb" runat="server" Width="100%"  ImageAlign="Middle" />
                                </asp:PlaceHolder>
                            </dxp:PanelContent>
                        </PanelCollection>
                    </dxp:ASPxPanel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </dxpc:PopupControlContentControl>
    </ContentCollection>
   
</dxpc:ASPxPopupControl>
