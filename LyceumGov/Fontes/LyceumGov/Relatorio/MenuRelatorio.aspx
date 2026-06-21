<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MenuRelatorio.aspx.cs" Inherits="Techne.Lyceum.Net.Relatorio.MenuRelatorio" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<asp:ScriptManagerProxy ID="scManager" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel runat="server" ID="updPanel">
    <ContentTemplate>
    <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
        ShowTabs="False" Width="100%">
        <TabPages>
            <dxtc:TabPage>
                <ContentCollection>
                    <dxw:ContentControl runat="server">
                        <asp:TreeView ID="tree" runat="server" ForeColor="#0066FF" ImageSet="Arrows"
                            CollapseImageToolTip="Recolher {0}" ExpandImageToolTip="Expandir {0}" 
                            OnSelectedNodeChanged="tree_SelectedNodeChanged1">
                            <NodeStyle HorizontalPadding="10px" />
                            <SelectedNodeStyle CssClass="TreeViewSelected" />
                        </asp:TreeView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
        <table>
            <tr>
                <td><dxe:ASPxButton ID="btnExpand" runat="server" Text="Expandir" onclick="btnExpand_Click"></dxe:ASPxButton></td>
                <td><dxe:ASPxButton ID="btnCollapse" runat="server" Text="Recolher" onclick="btnCollapse_Click" 
                        style="height: 24px"></dxe:ASPxButton></td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
