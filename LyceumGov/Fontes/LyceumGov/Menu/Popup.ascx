<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Popup.ascx.cs" Inherits="Techne.Lyceum.Net.Menu.Popup" %>

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
           
                                <asp:PlaceHolder ID="PlaCHTC" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td valign="middle">
                                            <img src="../images/img_interrog.png" width="60" height="60" align="left" />
                                        </td>
                                        <td valign="middle">
                                            <strong style="font-size: large; color: #00f">
                                            
                                             <asp:Label ID="LblText" runat="server" Text=""></asp:Label>
                                               <asp:LinkButton ID="btnImprimir" Font-Size="12px" style="color: red;" Font-Bold="true" OnClick="MostraRelatorio"
                        runat="server">CLIQUE AQUI</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                
                                </asp:PlaceHolder>
                                
                                <asp:PlaceHolder ID="plaTransferencia" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td valign="middle">
                                            <img src="../images/img_interrog.png" width="60" height="60" align="left" />
                                        </td>
                                        <td valign="middle">
                                            <strong style="font-size: large; color: #00f">Caro Agente Responsável, existem <span style="color: red;">TRANSFERÊNCIAS PENDENTES DE ACEITE</span> na área <a href="../Patrimonio/AcompanhamentoTransferenciaPatrimonio.aspx">Acompanhamento de Transferência</a>.</strong>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:PlaceHolder>
                                
                                 <asp:PlaceHolder ID="plaTransferenciaAluno" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td valign="middle">
                                            <img src="../images/img_interrog.png" width="60" height="60" align="left" />
                                        </td>
                                        <td valign="middle">
                                            <strong style="font-size: large; color: #00f">Caro Diretor/Secretário, existem <span style="color: red;">TRANSFERÊNCIAS PENDENTES DE ACEITE</span> na tela <a href="../Academico/SolicitacaoTransferenciaAluno.aspx">Solicitação de Transferência de Aluno > Acompanhamento de Solicitações</a>.</strong>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:PlaceHolder>
                                
                                <asp:PlaceHolder ID="plaVagas" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td valign="middle">
                                            <img src="../images/img_interrog.png" width="60" height="60" align="left" />
                                        </td>
                                        <td valign="middle">
                                            <strong style="font-size: large; color: #00f">Caro Diretor, existem vagas disponíveis a serem utilizadas.</strong><br />
                                            <div>As vagas <b>RESERVADAS</b>, ficam temporariamente para uso exclusivo da escola.</div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:HiddenField ID="hdnUnidade" runat="server" />
                                            <asp:ObjectDataSource ID="odsVagas" TypeName="Techne.Lyceum.Net.Menu.Popup" runat="server" SelectMethod="Listar">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnUnidade" Name="unidade" PropertyName="Value" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <dxwgv:ASPxGridView ID="grdVagas" runat="server" KeyFieldName="DESCRICAOCURSO;SERIE;DESCRICAOTURNO"
                                                ClientInstanceName="grdVagas" EnableCallBacks="false" AutoGenerateColumns="False"
                                                Width="95%" Font-Names="Verdana" Font-Size="Small" DataSourceID="odsVagas">
                                                <SettingsText EmptyDataRow="Não existem dados." />
                                                <SettingsPager PageSize="10" />
                                                <Columns>
                                                    <dxwgv:GridViewDataColumn FieldName="DESCRICAOCURSO" Caption="Curso" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="SERIE" Caption="Série" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="DESCRICAOTURNO" Caption="Código" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="VAGAS" Caption="Quantidade" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="RESERVADAS" Caption="Reservadas" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="TOTAL" Caption="Total" VisibleIndex="0" />
                                                </Columns>
                                            </dxwgv:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                 <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ObjectDataSource ID="odsConvocacaoSemEmail" TypeName="Techne.Lyceum.Net.Menu.Popup" runat="server" SelectMethod="ListarConvocacaoSemEmail">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnUnidade" Name="unidade" PropertyName="Value" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <dxwgv:ASPxGridView ID="grdConvocacaoSemEmail" runat="server" KeyFieldName="NUMEROINSCRICAO;DATACONVOCACAO"
                                                ClientInstanceName="grdConvocacaoSemEmail" EnableCallBacks="false" AutoGenerateColumns="False"
                                                Width="95%" Font-Names="Verdana" Font-Size="Small" DataSourceID="odsConvocacaoSemEmail">
                                                <SettingsText EmptyDataRow="Não existem dados." />
                                                <SettingsPager PageSize="10" />
                                                <Columns>
                                                    <dxwgv:GridViewDataColumn FieldName="NUMEROINSCRICAO" Caption="Inscrição" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="NOME" Caption="Nome" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="NOMEMAE" Caption="Mãe" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="DATANASCIMENTO" Caption="Data Nascimento" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="CELULAR" Caption="Celular" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="FIXOCELULAR" Caption="Telefone" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="EMAIL" Caption="E-mail" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="DATACONVOCACAO" Caption="Data Convocação" VisibleIndex="0" />
                                                    <dxwgv:GridViewDataColumn FieldName="PRAZORESPOSTA" Caption="Prazo Resposta" VisibleIndex="0" />
                                                </Columns>
                                            </dxwgv:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:PlaceHolder>
                                
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