<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtratoBancario.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.ExtratoBancario" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateEditWithoutBorder
        {
            width: 100%;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
            padding-bottom: 1px;
        }
        .TSearchButton
        {
            border-width: 0px !important;
            vertical-align: top !important;
            position: relative;
            top: -1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="Panel1" GroupingText="Informações do Extrato Bancário" Width="800px">
        <table width="800" style="table-layout: fixed;">
            <tr>
                <td width="150" align="right">
                    <span>Mês / Ano: <span style="color: red">*</span></span>
                </td>
                <td width="650">
                    <asp:ObjectDataSource ID="odsAno" runat="server" SelectMethod="ListaAno" TypeName="Techne.Lyceum.Net.PrestacaoContas.ExtratoBancario">
                    </asp:ObjectDataSource>
                    <asp:DropDownList ID="ddlMes" runat="server" Width="50px">
                        <asp:ListItem Value="1">01</asp:ListItem>
                        <asp:ListItem Value="2">02</asp:ListItem>
                        <asp:ListItem Value="3">03</asp:ListItem>
                        <asp:ListItem Value="4">04</asp:ListItem>
                        <asp:ListItem Value="5">05</asp:ListItem>
                        <asp:ListItem Value="6">06</asp:ListItem>
                        <asp:ListItem Value="7">07</asp:ListItem>
                        <asp:ListItem Value="8">08</asp:ListItem>
                        <asp:ListItem Value="9">09</asp:ListItem>
                        <asp:ListItem Value="10">10</asp:ListItem>
                        <asp:ListItem Value="11">11</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlAno" runat="server" Width="80px" DataSourceID="odsAno" AppendDataBoundItems="true"
                        DataTextField="ANO" DataValueField="ANO">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <span>Unidade de Ensino: <span style="color: red">*</span></span>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp" AutoPostBack="false"
                        MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="11%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:PlaceHolder ID="plaExtratoBancario" runat="server" Visible="false">
        <asp:HiddenField ID="hidExtratoBancarioId" runat="server" />
        <asp:HiddenField ID="hidMes" runat="server" />
        <asp:HiddenField ID="hidAno" runat="server" />
        <asp:HiddenField ID="hidUnidadeEnsino" runat="server" />
        <div class="divEditBlock" style="width: 800px;">
            <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" Visible="false"
                OnClick="btnEditar_Click" />
            <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" Visible="false" OnClick="btnNovo_Click" />
            <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" Visible="false"
                OnClick="btnCancel_Click" />
            <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" Visible="false"
                OnClick="btnSalvar_Click" />
            <asp:ImageButton ID="btnEnviarParaAnalise" runat="server" Visible="true" ImageUrl="~/Images/smallEnviarParaAnalise.png" ImageAlign="right" OnClick="btnEnviarParaAnalise_Click" />
            <asp:Label runat="server" ID="lblTitulo" Text="Extrato Bancário" SkinID="BcTitulo" />
        </div>
        <br />
        <br />
        
        <asp:HiddenField ID="hidStatus" runat="server" />
        <asp:PlaceHolder ID="plaStatus" runat="server" Visible="false">
        <span style="font-weight: bold; font-size: 18px;">Status: </span>
        <asp:Label ID="lblStatus" runat="server" style="font-size: 16px;"></asp:Label>
        <br />
        <br />
        </asp:PlaceHolder>
        
        <dxtc:ASPxPageControl ID="pcExtratoBancario" runat="server" ActiveTabIndex="0" OnTabClick="pcExtratoBancario_TabClick"
            Width="800px">
            <TabPages>
                <dxtc:TabPage Text="Extrato Bancário">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:PlaceHolder ID="plaExistente" runat="server" Visible="false">
                                <table width="800">
                                    <tr>
                                        <td width="100" align="right">
                                            <span>Extrato: <span style="color: Red">*</span></span>
                                        </td>
                                        <td width="700" style="padding: 20px 0;">
                                            <asp:FileUpload ID="filExtratoBancario" runat="server" Width="450px" Visible="false" />
                                            <asp:LinkButton ID="lnkVisualizar" runat="server" Text="Visualizar Extrato Bancário"
                                                Visible="false" OnCommand="btnVisualizar_Command"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <span>Observação:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Rows="5" Width="685px"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plaVazio" runat="server" Visible="false">
                                <p>
                                    Sem extrato bancário cadastrado</p>
                            </asp:PlaceHolder>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Exigências do Extrato" Name="tabExigenciasExtrato" Enabled="false">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">
                            <asp:ObjectDataSource ID="odsExigenciaExtrato" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ExtratoBancario"
                                SelectMethod="ListaExigenciaExtrato" UpdateMethod="UpdateExigenciaExtrato">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hidExtratoBancarioId" PropertyName="Value" Name="extratoBancarioId" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView runat="server" ID="grdExigenciaExtrato" ClientInstanceName="grdExigenciaExtrato"
                                AutoGenerateColumns="False" Width="800" DataSourceID="odsExigenciaExtrato"
                                KeyFieldName="EXIGENCIAEXTRATOID" OnRowUpdating="grdExigenciaExtrato_RowUpdating"
                                OnHtmlRowCreated="grdExigenciaExtrato_HtmlRowCreated">
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                                <Columns>
                                    <%--<dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="" Width="50px">
                                    <HeaderCaptionTemplate></HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>--%>
                                    <dxwgv:GridViewDataTextColumn Caption="Tipo Exigência" FieldName="DESCRICAO_TIPOEXIGENCIAEXTRATO"
                                        VisibleIndex="1" Width="200px">
                                        <EditItemTemplate>
                                            <%# Eval("DESCRICAO_TIPOEXIGENCIAEXTRATO")%></EditItemTemplate>
                                        <EditCellStyle CssClass="tab-cell">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nota Explicativa" FieldName="NOTAEXPLICATIVA"
                                        VisibleIndex="2" Width="200px">
                                        <EditItemTemplate>
                                            <%# Eval("NOTAEXPLICATIVA")%></EditItemTemplate>
                                        <EditCellStyle CssClass="tab-cell">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" VisibleIndex="3"
                                        Width="250px">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Anexo" Name="btnDetalhes" VisibleIndex="4"
                                        Width="50px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnDetalhes" class="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXIGENCIAEXTRATOID") %>'
                                                OnCommand="btnDetalhes_Command" ImageUrl="~/img/upload.png" Height="15px" AlternateText="Importar Arquivo"
                                                Visible='<%# Eval("EXIGENCIAEXTRATOID") != DBNull.Value %>'></asp:ImageButton>
                                            <script language="javascript">
                                                $(document).ready(function() {
                                                    var status = $("#<%= hidStatus.ClientID %>").val();

                                                    if (status !== "2")
                                                        $(".btnDetalhes").each(function(index, el) { $(el).css("display", "none"); });
                                                    else
                                                        $(".btnDetalhes").each(function(index, el) { $(el).css("display", "inline"); });
                                                });
                                            </script>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="5" Width="50px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXIGENCIAEXTRATOID") + "," + Eval("TIPOARQUIVO") %>'
                                                OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                AlternateText="Visualizar Documento" Visible='<%# Eval("EXIGENCIAEXTRATOID") != DBNull.Value && Eval("EXIGENCIAEXTRATOARQUIVOID") != DBNull.Value %>'>
                                            </asp:ImageButton>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataColumn Caption="Aprovado" FieldName="APROVADO" VisibleIndex="6" Width="50px">
                                    <DataItemTemplate><%# Eval("APROVADO") == DBNull.Value ? "Não analisado" : (Convert.ToBoolean(Eval("APROVADO")) ? "Sim" : "Não") %></DataItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:CheckBox ID="chkAprovado" runat="server" Checked="false" Visible='<%# !Convert.ToString(Eval("JUSTIFICATIVA")).IsNullOrEmptyOrWhiteSpace() && (Eval("ARQUIVO") as byte[] ?? new byte[] { }).Any() %>'></asp:CheckBox>
                                    </EditItemTemplate>--%>
                                    <EditItemTemplate></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
        <dxpc:ASPxPopupControl ID="pucConfirmarArquivo" ClientInstanceName="pucConfirmarArquivo"
            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
            ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
            Width="580px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
            HeaderText="Upload de Arquivos">
            <HeaderStyle HorizontalAlign="Center" />
            <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <SizeGripImage Height="12px" Width="12px" />
            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
            <ContentCollection>
                <dxpc:PopupControlContentControl>
                    <table id="Table1" runat="server" width="100%">
                        <tr>
                            <td>
                                Documento:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Justificativa:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtJustificativa" runat="server" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnImportar" runat="server" Text="Importar" OnClick="btnImportar_Click"
                                    OnClientClick="pucConfirmarArquivo.Hide(); return true;" />
                            </td>
                        </tr>
                    </table>
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:ASPxPopupControl>
        <asp:HiddenField ID="hdnExigenciaExtratoId" runat="server" />
        <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
            ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Extrato Bancário">
            <HeaderStyle HorizontalAlign="Center" />
            <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
            <ContentCollection>
                <dxpc:PopupControlContentControl>
                    <dxe:ASPxBinaryImage ID="bimgArquivo" Width="350px" Height="350px" runat="server"
                        Visible="false" StoreContentBytesInViewState="True" AlternateText="sem foto"
                        ClientInstanceName="bimgArquivo">
                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                    </dxe:ASPxBinaryImage>
                    <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:ASPxPopupControl>
    </asp:PlaceHolder>
</asp:Content>
