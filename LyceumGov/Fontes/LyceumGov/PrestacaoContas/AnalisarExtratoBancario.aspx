<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalisarExtratoBancario.aspx.cs" MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.AnalisarExtratoBancario" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>

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

    <asp:HiddenField runat="server" ID="hdnIdTermo" />
    <asp:HiddenField runat="server" ID="hdnDadosPublicacao" />
    <asp:HiddenField runat="server" ID="hdnAba" />
    <asp:HiddenField runat="server" ID="hdnprogramatrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid2" />
    
    <asp:Panel runat="server" ID="Panel1" GroupingText="Informações do Extrato Bancário" Width="800px">
        <table width="800">
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
                </td>
            </tr>
            <tr>
                <td width="110" align="right">
                    <span>Mês / Ano:</span>
                </td>
                <td width="690">
                    <asp:ObjectDataSource ID="odsAno" runat="server" SelectMethod="ListaAno" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarExtratoBancario">
                    </asp:ObjectDataSource>
                
                    <asp:DropDownList ID="ddlMes" runat="server" Width="50px">
                        <asp:ListItem></asp:ListItem>
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
                    <asp:DropDownList ID="ddlAno" runat="server" Width="80px" DataSourceID="odsAno" AppendDataBoundItems="true" DataTextField="ANO" DataValueField="ANO">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableViewState="false"></asp:Label>
    <br />
    
    <asp:PlaceHolder ID="plaGrid" runat="server" Visible="false">
    
    <asp:ObjectDataSource ID="odsExtratoBancario" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarExtratoBancario" SelectMethod="ListaExtratoBancario">
        <SelectParameters>
            <asp:Parameter Name="mes" />
            <asp:Parameter Name="ano" />
            <asp:Parameter Name="censo" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <dxwgv:ASPxGridView runat="server" ID="grdExtratoBancario"
        ClientInstanceName="grdExtratoBancario"
        AutoGenerateColumns="False"
        EnableCallBacks="false" 
        Width="1000"
        DataSourceID="odsExtratoBancario"
        KeyFieldName="EXTRATOBANCARIOID" OnFocusedRowChanged="grdExtratoBancario_FocusedRowChanged">
        
        <SettingsBehavior ProcessFocusedRowChangedOnServer="true" AllowFocusedRow="true" AllowMultiSelection="False" AllowSort="False" />
        
        <Columns>
            <dxwgv:GridViewDataColumn Caption="" VisibleIndex="0" Width="40px">
                <DataItemTemplate>
                    <asp:ImageButton ID="lnkVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXTRATOBANCARIOID") + "," + Eval("TIPOARQUIVO") %>'
                        OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px" AlternateText="Visualizar Extrato">
                    </asp:ImageButton>
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="Censo" FieldName="CENSO" VisibleIndex="1" Width="80px">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="NotSet" />
                <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="Escola" FieldName="NOME_COMP" VisibleIndex="1" Width="200px">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="NotSet" />
                <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="Mês" FieldName="MES" VisibleIndex="2" Width="80px">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="NotSet" />
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="Ano" FieldName="ANO" VisibleIndex="3" Width="80px">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="NotSet" />
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="Status" FieldName="STATUSDESCRICAO" VisibleIndex="4" Width="160px">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="NotSet" />
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
            <dxwgv:GridViewDataColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="4" Width="400px">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="NotSet" />
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dxwgv:GridViewDataColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    
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
    
    <asp:PlaceHolder ID="plaExtratoBancario" runat="server" Visible="false">
    
    <asp:HiddenField ID="hidExtratoBancarioId" runat="server" />
    <asp:HiddenField ID="hidMes" runat="server" />
    <asp:HiddenField ID="hidAno" runat="server" />
    <asp:HiddenField ID="hidUnidadeEnsino" runat="server" />
    
    <div class="divEditBlock" style="width: 800px;">
        <asp:Label runat="server" ID="lblTitulo" Text="Extrato Bancário" SkinID="BcTitulo" />
        <asp:ImageButton ID="btnAprovar" runat="server" Visible="true" ImageUrl="~/Images/smallAprovar.png" ImageAlign="right" OnClick="btnAprovar_Click" OnClientClick="return confirm('Confirma a aprovação deste extrato bancário?');" />
        <asp:ImageButton ID="btnReprovar" runat="server" Visible="true" ImageUrl="~/Images/smallReprovar.png" ImageAlign="right" OnClick="btnReprovar_Click" OnClientClick="return confirm('Confirma a reprovação deste extrato bancário?');" />
        <asp:ImageButton ID="btnDevolver" runat="server" Visible="true" ImageUrl="~/Images/smallDevolver.png" ImageAlign="right" OnClick="btnDevolver_Click" />
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
    
    <dxtc:ASPxPageControl ID="pcExtratoBancario" runat="server" ActiveTabIndex="0" OnTabClick="pcExtratoBancario_TabClick" Width="800px">
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
                                    <asp:LinkButton ID="lnkVisualizar" runat="server" Text="Visualizar Extrato Bancário" Visible="false" OnCommand="btnVisualizar_Command"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <span>Observação: <span style="color: Red">*</span></span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Rows="5" Width="685px" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        </asp:PlaceHolder>
                        
                        <asp:PlaceHolder ID="plaVazio" runat="server" Visible="false">
                        <p>Sem extrato bancário cadastrado</p>
                        </asp:PlaceHolder>
                    
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Exigências do Extrato" Name="tabExigenciasExtrato" Enabled="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
 
                        <asp:ObjectDataSource ID="odsTipoExigenciaExtrato" runat="server"
                            TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarExtratoBancario" 
                            SelectMethod="ListaTipoExigenciaExtrato">
                        </asp:ObjectDataSource>
                    
                        <asp:ObjectDataSource ID="odsExigenciaExtrato" runat="server" 
                            TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarExtratoBancario" 
                            SelectMethod="ListaExigenciaExtrato"
                            InsertMethod="InsertExigenciaExtrato"
                            UpdateMethod="UpdateExigenciaExtrato"
                            DeleteMethod="DeleteExigenciaExtrato">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hidExtratoBancarioId" PropertyName="Value" Name="extratoBancarioId" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    
                        <dxwgv:ASPxGridView runat="server" ID="grdExigenciaExtrato"
                            ClientInstanceName="grdExigenciaExtrato"
                            AutoGenerateColumns="False"
                            EnableCallBacks="false" 
                            Width="800"
                            DataSourceID="odsExigenciaExtrato"
                            KeyFieldName="EXIGENCIAEXTRATOID"
                            OnCommandButtonInitialize="grdExigenciaExtrato_CommandButtonInitialize"
                            OnCustomButtonCallback="grdExigenciaExtrato_CustomButtonCallback" 
                            OnCustomButtonInitialize="grdExigenciaExtrato_CustomButtonInitialize"
                            OnRowInserting="grdExigenciaExtrato_RowInserting"
                            OnRowUpdating="grdExigenciaExtrato_RowUpdating"
                            OnRowDeleting="grdExigenciaExtrato_RowDeleting">
                            
                            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                            
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png" onclick="grdExigenciaExtrato.AddNewRow();" />
                                            <%--<script language="javascript">
                                                $(document).ready(function() {
                                                    var status = $("#<%= hidStatus.ClientID %>").val();

                                                    if (status !== "1" && status !== "3")
                                                        $("#btnNovoGrid").css("display", "none");
                                                    else
                                                        $("#btnNovoGrid").css("display", "inline");
                                                });
                                            </script>--%>
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Visible="True" Text="Remover">
                                        <Image Url="../img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Exigência" FieldName="TIPOEXIGENCIAEXTRATOID" VisibleIndex="1" Width="200px">
                                    <PropertiesComboBox DataSourceID="odsTipoExigenciaExtrato" TextField="DESCRICAO" ValueField="TIPOEXIGENCIAEXTRATOID"></PropertiesComboBox>
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nota Explicativa" FieldName="NOTAEXPLICATIVA" VisibleIndex="2" Width="200px">
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" VisibleIndex="3" Width="250px">
                                    <EditItemTemplate><%# Eval("JUSTIFICATIVA") %></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                    <EditCellStyle CssClass="tab-cell" HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </EditCellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="5" Width="50px">
                                    <DataItemTemplate>
                                        <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXIGENCIAEXTRATOID") + "," + Eval("TIPOARQUIVO") %>'
                                            OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                            AlternateText="Visualizar Documento" Visible='<%# Eval("EXIGENCIAEXTRATOID") != DBNull.Value && Eval("EXIGENCIAEXTRATOARQUIVOID") != DBNull.Value %>'></asp:ImageButton>
                                    </DataItemTemplate>
                                    <EditItemTemplate></EditItemTemplate>
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
                                <dxwgv:GridViewCommandColumn VisibleIndex="7" ButtonType="Link" Caption="Ação" Width="50px">
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnAprovarExigencia" Text="Aprovar" Visibility="BrowsableRow"></dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnReprovarExigencia" Text="Reprovar" Visibility="BrowsableRow"></dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataColumn FieldName="ARQUIVO" Visible="false"></dxwgv:GridViewDataColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    
    </asp:PlaceHolder>
</asp:Content>