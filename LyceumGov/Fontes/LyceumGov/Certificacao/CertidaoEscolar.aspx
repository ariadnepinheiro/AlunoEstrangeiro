<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CertidaoEscolar.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.CertidaoEscolar" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<script type="text/javascript">


        //add event handlers to the search UpdatePanel
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function startRequest(sender, e) {
            ppcLoading.Show();
        }

        function endRequest(sender, e) {
            ppcLoading.Hide();
        }

 
    </script>
    
     <asp:UpdatePanel ID="upnlMatriculas" runat="server" >
        <ContentTemplate>
   
    <asp:Panel ID="pnBusca" runat="server" Width="950px">
        <br />
        <div style="height: 1px; width: 100%; border-bottom: inset 1px Blue;">
        </div>
        <br />
        <table width="100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens" Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO" MaxLength="20" FieldName="Unidade de Ensino" GridWidth="850px" SqlOrder="nome_comp" AutoPostBack="true" OnChanged="tseUnidadeEnsino_Changed" >
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />										
                                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblTCurso" Text="Escolaridade:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseCurso" runat="server" Argument="nome" Caption="" Key="curso"
                                    SqlSelect="SELECT curso, nome FROM ly_curso" SqlOrder="nome" AutoPostBack="true"
                                      OnChanged="tseCurso2_Changed">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                            <td>
                               
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblAnoEscolar" Text="Ano Escolar:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAnoEscolar" AutoPostBack="true" runat="server" Width="130px"
                                    DataTextField="AnoEscolar" DataValueField="AnoEscolar" OnSelectedIndexChanged="ddlAnoEscolar_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblTurmaTse" runat="server" Text="Turma:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurma" runat="server" Width="130px"
                                    DataTextField="turma" DataValueField="turma" OnClick="btnPesquisar_Click">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:ImageButton ID="btnPesquisar" runat="server" ImageUrl="~/Images/bot_buscar.png"
                        OnClick="btnPesquisar_Click" ValidationGroup="Buscar" />
                </td>
                </tr>
                <tr>
                <td>
                    <br />
                    <asp:Label ID="lblMensagem" runat="server" ClientInstancename="lblMensagem" SkinID="lblMensagem"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
        <dxwgv:ASPxGridView ID="grdMeusAlunos" ClientInstanceName="grdMeusAlunos" AutoGenerateColumns="False"
            Visible="false" Width="950px" EnableViewState="false" runat="server" KeyFieldName="ALUNO"
            EnableCallBacks="false" >
            <SettingsBehavior AllowMultiSelection="TRUE" AllowFocusedRow="false" />
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsPager PageSize="2"
             />
            <Columns>
                <dxwgv:GridViewDataCheckColumn Caption="Selecionar" FieldName="" Name="Selecionar"
                    VisibleIndex="0" Width="10px">
                    <DataItemTemplate>
                        <asp:CheckBox ID="ckUtilizar" runat="server" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataCheckColumn>            
                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="ALUNO" VisibleIndex="1"
                    Visible="false" Width="900px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CódTurno" FieldName="TURNO" VisibleIndex="3"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CódAno Escolar" FieldName="SERIE" VisibleIndex="4"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TIPOCONCLUSAOID" FieldName="TIPOCONCLUSAOID"
                    VisibleIndex="5" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TURMA" FieldName="TURMA" VisibleIndex="6"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                
                 <dxwgv:GridViewDataTextColumn Caption="Tipo de Conclusão" FieldName="TIPO_CONCLUSAO" VisibleIndex="7"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            <ClientSideEvents SelectionChanged="function(s, e) { SelChanged(s, e); }" />
        </dxwgv:ASPxGridView>
       
      
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnGerar" runat="server" Text="Gerar Certidão Assinada" OnClick="btnGerar_Click"
                        AutoPostBack="false" />
                   </td>
            </tr>
        </table>
    </asp:Panel>
    
<dxpc:ASPxPopupControl ID="ppcLoading" ClientInstanceName="ppcLoading" runat="server"
                Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
                ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" DisappearAfter="60" Cursor="wait" EnableAnimation="false"
                Width="150px">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl>
                        <div style="text-align: center">
                            <asp:Image ID="Image2_" ImageUrl="~/App_Themes/Blue/GridView/gvLoadingOnStatusBar.gif"
                                runat="server" Visible="false" />
                            <asp:Label ID="Label2" Text="Carregando dados..." Font-Bold="true" runat="server" />
                            <br />
                            <br />
                            <asp:Image ID="Image1" ImageUrl="~/App_Themes/Blue/GridView/Loading.gif" runat="server" />
                        </div>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
      
    </asp:UpdatePanel>
</asp:Content>