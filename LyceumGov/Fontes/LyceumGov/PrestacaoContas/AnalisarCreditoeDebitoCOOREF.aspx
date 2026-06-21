<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalisarCreditoeDebitoCOOREF.aspx.cs" MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.AnalisarCreditoeDebitoCOOREF" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>

<%@ Register assembly="DevExpress.Web.v9.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dxtc" %>

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
    <asp:HiddenField runat="server" ID="hdnOperacaoExigenciaId" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid2" />
    
    <asp:HiddenField ID="hdnArquivoId" runat="server" />
    <asp:HiddenField ID="hdnQueryString" runat="server" />
    <asp:HiddenField ID="hdnOcorrenciaId" runat="server" />
    <asp:HiddenField ID="hdnPeriodoReferencia" runat="server" />
    <asp:HiddenField ID="hdnPerfil" runat="server" />
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Programa/Projeto" Width="800px">
            <table class="table-fixed" width="800">
            <tr>
                <td align="left">
                    <span>Período da Prestação de Contas: <span style="color: red">*</span></span>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tsePeriodoPrestacaoContas" runat="server" Key="periodoreferenciaid"
                        Argument="descricao" MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" 
                        SqlSelect="select periodoreferenciaid, mesinicial, mesfinal, referencia, datalimiteprestacaocontas, datalimiteanalise, descricao from prestacaocontas.vw_periodoreferencia"
                        GridWidth="850px" SqlOrder="periodoreferenciaid" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ID" FieldName="periodoreferenciaid" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Mês Inicial" FieldName="mesinicial" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Mês Final" FieldName="mesfinal" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Referência" FieldName="referencia" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim PContas" FieldName="datalimiteprestacaocontas" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim Análise" FieldName="datalimiteanalise" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
                        <tr>
                <td  align="left">
                    <span>Unidade de Ensino: </span>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns><tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
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
                <td align="left">
                    <span>Projeto / Programa: </span>
                </td>
                <td align="left">
                      <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="VarChar" AutoPostBack="true" 
                        SqlSelect=" select DISTINCT descricao, FINALIDADE,FINALIDADEID from [PrestacaoContas].[VW_PLANOTRABANHO_CENSO_COREF] "
                        OnChanged="tsePlanoTrabalho_Changed" SqlWhere=" CENSO = #tseUnidadeEnsino# ">
                        <GridColumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                        <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>                            
            </tr>
             <tr>
                <td align="left">
                    <span>Operação:</span>
                </td>
                <td align="left"> 
                   <tweb:TSearchBox ID="tseOperacao" runat="server" Caption="" Key="OPERACAOID" Argument="tipo"
                        MaxLength="20" ArgumentColumns="50" Columns="10" GridWidth="850px" DataType="Number" AutoPostBack="True"
                        SqlSelect="SELECT [CENSO],[PERIODOREFERENCIAID]      ,[PLANOTRABALHOID]      ,[tipo]      ,[plano]      ,[DATACADASTRO]      ,[status]      ,[VALOR]  FROM [LYCEUM].[PrestacaoContas].[VW_OPERACAO] "
                        OnChanged="tseOperacao_Changed" SqlWhere=" PERIODOREFERENCIAID = #tsePeriodoPrestacaoContas# and CENSO = #tseUnidadeEnsino# and PLANOTRABALHOID = #tsePlanoTrabalho#" Enabled="true">
                        <GridColumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="OPERACAOID" Width="5%" />
                        <tweb:TSearchBoxColumn Caption="Operação" FieldName="tipo" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Projeto/Programa" FieldName="plano" Width="15%" />
                        <tweb:TSearchBoxColumn Caption="Data Cadastro" FieldName="DATACADASTRO" Width="15%" />
                        <tweb:TSearchBoxColumn Caption="Status" FieldName="status" Width="30%" />
                        <tweb:TSearchBoxColumn Caption="Valor" FieldName="VALOR" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>    
            </tr>
               <tr>
                <td align="left">
                    <span>Status da Operação:</span>
                </td>
                <td  align="left"> 
                        <asp:RadioButtonList ID="rbFiltroOperacao" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" >
                            <asp:ListItem Selected=True Text="Todos" Value="9" />
                            <asp:ListItem Text="Lançado pela AAE" Value="0" />
                            <asp:ListItem Text="Enviado para Análise" Value="1" />
                            <asp:ListItem Text="Devolvido para Revisão" Value="2" />
                            <asp:ListItem Text="Aprovado" Value="3" />
                            <asp:ListItem Text="Reprovado" Value="4" />
                            <asp:ListItem Text="Com Exigência" Value="8" />
                        </asp:RadioButtonList>            
                </td>    
            </tr>
            <tr>
                <td align="right" colspan=2>
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
    
    
    
    
    
        <asp:Panel ID="pnlRegistro" runat="server" Width="100%" Visible="False">
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdRegistro" runat="server" KeyFieldName="OPERACAOID" ClientInstanceName="grdRegistro"
                        AutoGenerateColumns="False" OnAfterPerformCallback="grdRegistro_AfterPerformCallback"
                        Width="100%" OnPageIndexChanged="grdRegistro_PageIndexChanged">
                        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Styles CommandColumn-Wrap="False" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                                <SelectButton Text="Selecionar" Visible="True">
                                    <Image Url="~/img/bt_busca.png" />
                                </SelectButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Censo" VisibleIndex="1" Caption="CENSO" CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="nome_comp" VisibleIndex="1" Caption="Escola">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="OPERACAOID" VisibleIndex="3" Caption="Id" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="plano" VisibleIndex="1" Caption="Projeto/Programa">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="DATACADASTRO" VisibleIndex="1" Caption="Data do Envio">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Valor" FieldName="VALOR"  VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="40">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="status" VisibleIndex="3" Caption="Status" Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="qtd" VisibleIndex="3" Caption="Total de Exigências" Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    
    
    
    
    
    
    
    

                    
     
    
    
    
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableViewState="false"></asp:Label>
    <br />
    
    <asp:PlaceHolder ID="plaCreditoeDebito" runat="server" Visible="false">
    
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

        <tabpages>
            <dxtc:TabPage Text="Crédito/ Débito">
                            <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                           <table>
                            <tr>
                                <td>
                                  <asp:Label ID="Label2" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="Identificador"></asp:Label>
                                </td>
                                <td>
                                  <asp:Label ID="lblIdentificador" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text=""></asp:Label>
                                  <asp:Label ID="lblDescricao" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text=""></asp:Label>
                                </td>
                                  </tr>
                                    <tr> 
                                     <td style="text-align: right;">
                                     <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Operação:"></asp:Label>
                                     </td>
                                     <td>
                                        <asp:DropDownList ID="ddlOperacao" runat="server" Width="375px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlOperacao_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value="" />
                                        <asp:ListItem Text="Débito" Value="D" />
                                        <asp:ListItem Text="Crédito" Value="C" />
                                       </asp:DropDownList>
                                     </td>
                                   </tr>
                               <tr>
                                <td style="text-align: right;">
                                   <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Valor:"></asp:Label>
                                </td>
                                <td>
                                  <asp:TextBox ID="txtValor" runat="server"  Width="80px" MaxLength="6" SkinID="numerico"/>
                                </td>
                            </tr>
                                           <tr>
                            <td style="text-align: right;">
                                <asp:Label Font-Names="Verdana" ID="Label10" runat="server" Text="Justificativa:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificativa" runat="server" MaxLength="500" TextMode="MultiLine"
                                    onkeyup="mostrarResultado(this.value,500,'spHistorico');contarCaracteres(this.value,500,'spHistorico',this)"
                                    Height="75px" Width="600px" />
                                <br />
                                <span id="spHistorico" style="font-family: Georgia;">Limite de 500 caracteres</span><br />
                            </td>
                        </tr>
                        </table>
                  </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <dxtc:TabPage Text="Exigência Crédito/Débito">
               <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
<ContentCollection>
                        <dxw:ContentControl ID="ContentControl5" runat="server">
                            <asp:ObjectDataSource ID="odsDocumento2" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.IncluirCreditoeDebito"
                                SelectMethod="ListaExigencias" >
                                <SelectParameters>
                                    <asp:ControlParameter Name="Id" ControlID="hdnOcorrenciaId" PropertyName="Value"
                                        Type="Int32" DefaultValue="1" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView runat="server" ID="grdExigencias" ClientInstanceName="grdExigencias"
                                AutoGenerateColumns="False" EnableCallBacks="false" Width="800" DataSourceID="odsDocumento2"
                                KeyFieldName="OPERACAOEXIGENCIAARQUIVOID" >
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <SettingsBehavior ConfirmDelete="true" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <HeaderCaptionTemplate>
                                            </HeaderCaptionTemplate>                       
                                        </dxwgv:GridViewCommandColumn>                                
                                         <dxwgv:GridViewDataTextColumn Caption="ID" Visible=false FieldName="OPERACAOEXIGENCIAID" VisibleIndex="3"
                                        Width="1%">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Tipo Exigência" FieldName="TIPOEXIGENCIAOPERACAOID"
                                        VisibleIndex="4" Width="30%">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nota Explicativa" FieldName="NOTAEXPLICATIVA"
                                        VisibleIndex="5" Width="20%">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="" FieldName="JUSTIFICATIVA "
                                        VisibleIndex="5" Width="20%">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>                                    
                                    <dxwgv:GridViewDataTextColumn Caption="Anexo" FieldName="NOMEARQUIVO"
                                        VisibleIndex="4" Width="30%">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>                                    
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="7" Width="20%">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizar" runat="server" 
                                               ImageUrl="~/img/bt_busca.png" Height="15px"
                                               AlternateText="Visualizar Documento" >
                                            </asp:ImageButton>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>    
                                    <dxwgv:GridViewDataTextColumn Caption="" FieldName="APROVADO "
                                        VisibleIndex="5" Width="20%">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>                                 
                                </Columns>
                            </dxwgv:ASPxGridView>
                
                        </dxw:ContentControl>
                    </ContentCollection>
                   </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>

        </tabpages>

 
   
    </dxtc:ASPxPageControl>
    
    </asp:PlaceHolder>
    
</asp:Content>