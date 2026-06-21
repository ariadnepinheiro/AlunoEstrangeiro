<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false"  CodeBehind="IncluirCreditoeDebito.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.IncluirCreditoeDebito" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
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
        .table-fixed
        {
            table-layout: fixed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server"> 
 <script type="text/javascript">

     $(document).ready(function() {
         $("input.dinheiro")
                 .maskMoney({
                     decimal: ",",
                     thousands: "."
                 })
     });

     function OnlyNumericEntry(e) {

         var charCode = (e.which) ? e.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;
         return true;
     }

     function mostrarResultado(box, num_max, spContador) {
         var contagem_carac = box.length;
         if (contagem_carac != 0) {
             document.getElementById(spContador).innerHTML = contagem_carac + " caracteres digitados";
             if (contagem_carac == 1) {
                 document.getElementById(spContador).innerHTML = contagem_carac + " caracter digitado";
             }
             if (contagem_carac >= num_max) {
                 document.getElementById(spContador).innerHTML = "Limite de caracteres excedido!";
             }
         } else {
             document.getElementById(spContador).innerHTML = "Limite de " + num_max + " caracteres";
         }
     }
     function contarCaracteres(box, valor, spContador, campoMult) {

         var conta = valor - box.length;
         document.getElementById(spContador).innerHTML = "Você ainda pode digitar " + conta + " caracteres";
         if (box.length >= valor) {
             document.getElementById(spContador).innerHTML = "Limite excedido.";
             campoMult.value = campoMult.value.substr(0, valor);
         }
     }
     function Bloqueio() {

         var divBloqueio = document.getElementById("dvbloqueio");
         divBloqueio.className = "Bloqueado";
     }

     function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e, Tamanho) {

         var sep = 0;
         var key = '';
         var i = j = 0;
         var len = len2 = 0;
         var strCheck = '0123456789';
         var aux = aux2 = '';
         var whichCode = (window.Event) ? e.which : e.keyCode;
         if (whichCode == 13) return true;
         if (whichCode == 8) return true;

         key = String.fromCharCode(whichCode); // Valor para o código da Chave
         if (strCheck.indexOf(key) == -1) return false; // Chave inválida
         if (Tamanho < objTextBox.value.length) return false; // Tamanho
         len = objTextBox.value.length;

         for (i = 0; i < len; i++)
             if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
         aux = '';
         for (; i < len; i++)
             if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
         aux += key;
         len = aux.length;
         if (len == 0) objTextBox.value = '';
         if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
         if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
         if (len > 2) {
             aux2 = '';
             for (j = 0, i = len - 3; i >= 0; i--) {
                 if (j == 3) {
                     aux2 += SeparadorMilesimo;
                     j = 0;
                 }
                 aux2 += aux.charAt(i);
                 j++;
             }
             objTextBox.value = '';
             len2 = aux2.length;
             for (i = len2 - 1; i >= 0; i--)
                 objTextBox.value += aux2.charAt(i);
             objTextBox.value += SeparadorDecimal + aux.substr(len - 2, len);
         }
         return false;
     }

     function moeda(a, e, r, t) {

         var n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
         if (13 == o || 8 == o)
             return !0;
         if (n = String.fromCharCode(o),
    -1 == "0123456789".indexOf(n))
             return !1;
         for (u = a.value.length,
    h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
             ;
         for (l = ""; h < u; h++)
-1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
         if (l += n,
    0 == (u = l.length) && (a.value = ""),
    1 == u && (a.value = "0" + r + "0" + l),
    2 == u && (a.value = "0" + r + l),
    u > 2) {
             for (ajd2 = "",
        j = 0,
        h = u - 3; h >= 0; h--)
                 3 == j && (ajd2 += e,
            j = 0),
            ajd2 += l.charAt(h),
            j++;
             for (a.value = "",
        tamanho2 = ajd2.length,
        h = tamanho2 - 1; h >= 0; h--)
                 a.value += ajd2.charAt(h);
             a.value += r + l.substr(u - 2, u)
         }
         return !1
     }


    </script>
    <asp:HiddenField ID="hdnArquivoId" runat="server" />
    <asp:HiddenField ID="hdnQueryString" runat="server" />
    <asp:HiddenField ID="hdnOcorrenciaId" runat="server" />
    <asp:HiddenField ID="hidStatus" runat="server" />    
    <asp:HiddenField ID="hdnOperacaoExigenciaId" runat="server" />
    <asp:HiddenField ID="hdnPerfil" runat="server" />
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Programa/Projeto" Width="800px">
         <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Unidade de Ensino: <span style="color: red">*</span></span>
                </td>
                <td width="600">
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
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Projeto / Programa: <span style="color: red">*</span></span>
                </td>
                <td width="600">
                      <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="VarChar" AutoPostBack="true" 
                        SqlSelect=" select DISTINCT descricao, FINALIDADE,FINALIDADEID from [PrestacaoContas].[VW_PLANOTRABANHO_CENSO] "
                        OnChanged="tsePlanoTrabalho_Changed" SqlWhere=" CENSO = #tseUnidadeEnsino# ">
                        <GridColumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                        <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>                            
            </tr>
        </table>        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Período da Prestação de Contas: <span style="color: red">*</span></span>
                </td>
                <td width="600">
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
        </table>
   

        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Operação:</span>
                </td>
                <td width="600"> 
                   <tweb:TSearchBox ID="tseOperacao" runat="server" Caption="" Key="CODOPERACAO" Argument="tipo"
                        MaxLength="20" ArgumentColumns="50" Columns="10" GridWidth="850px" DataType="VarChar" AutoPostBack="True"
                        SqlSelect="SELECT [CENSO],[PERIODOREFERENCIAID]      ,[PLANOTRABALHOID]      ,[tipo]      ,[plano]      ,[DATACADASTRO]      ,[status]      ,[VALOR]  FROM [LYCEUM].[PrestacaoContas].[VW_OPERACAO] "
                        OnChanged="tseOperacao_Changed" SqlWhere=" PERIODOREFERENCIAID = #tsePeriodoPrestacaoContas# and CENSO = #tseUnidadeEnsino# and PLANOTRABALHOID = #tsePlanoTrabalho#" Enabled="true">
                        <GridColumns>
                        <tweb:TSearchBoxColumn Caption="Código" FieldName="CODOPERACAO" Width="5%" />
                        <tweb:TSearchBoxColumn Caption="Operação" FieldName="tipo" Width="10%" />
                        <tweb:TSearchBoxColumn Caption="Projeto/Programa" FieldName="plano" Width="15%" />
                        <tweb:TSearchBoxColumn Caption="Data Cadastro" FieldName="DATACADASTRO" Width="15%" />
                        <tweb:TSearchBoxColumn Caption="Status" FieldName="status" Width="30%" />
                        <tweb:TSearchBoxColumn Caption="Valor" FieldName="VALOR" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>    
            </tr>
        </table>

    </asp:Panel>
    <br />
    
    
    
    
    <asp:Label ID="lblMensagem" runat="server" ForeColor="Red"></asp:Label>
    <br />

    
    
    
    <asp:Label ID="lblTxtStatus" runat="server" 
        EnableViewState="False"></asp:Label>  
    
    <asp:Label ID="lblStatus" runat="server" ForeColor="Black" 
        EnableViewState="False" Font-Bold="True"></asp:Label>
    <br />

    
    <br />
    <asp:PlaceHolder ID="plaVisibilidadeGrid" runat="server" >
    <div class="divEditBlock" style="width: 800px;">
        <asp:ImageButton ID="btnEditar"        runat="server" SkinID="BcNovoEditar" OnClick="btnEditar_Click" OnClientClick="" />
        <asp:ImageButton ID="btnNovo"          runat="server" SkinID="BcNovoNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnSalvar"        runat="server" SkinID="BcNovoSalvar" OnClick="btnSalvar_Click" />
        <asp:ImageButton ID="btnEnviarAnalise" runat="server" ImageUrl="~/Images/bot_EnviarAnalise.png"   align="right" OnClick="btnEnviarAnalise_Click" ToolTip="Enviar para Análise" SkinID="BcEnviar" />
        <asp:ImageButton ID="btnAnalisar"      runat="server" align="right" OnClientClick="pucConfirmar.Show(); return false;" OnPreRender="btnAnalisar_PreRender" ToolTip="Analisar" SkinID="BcAnalisar" />
        <asp:ImageButton ID="btnCancel"        runat="server" SkinID="BcNovoCancelar" OnClick="btnCancel_Click" />            
        <asp:ImageButton ID="btnExcluir"       runat="server" SkinID="BcNovoExcluir" OnClick="btnExcluir_Click" />   
        <asp:Label runat="server" ID="lblBlocoFornecedor" Text="Operações" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"  ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />        
    </div>
        <br />
    <dxtc:ASPxPageControl ID="pcCreditoDebito" runat="server" ActiveTabIndex="0" 
        Width="800px">
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
                                   <asp:TextBox ID="txtValor" runat="server" MaxLength="10"  Width="190px" CssClass="numeric" 
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
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
          
                            <asp:ObjectDataSource ID="odsTipoExigencia" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.IncluirCreditoeDebito"
                              SelectMethod="ListaTipoExigencias" >
                            </asp:ObjectDataSource>
                                           
                            <asp:ObjectDataSource ID="odsCreditoeDebito" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.IncluirCreditoeDebito"
                                SelectMethod="ListaExigencias"  UpdateMethod="Update">  
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnOcorrenciaId"      PropertyName="Value" Name="Id" />
                                </SelectParameters>            
                            </asp:ObjectDataSource>
                            
                            <dxwgv:ASPxGridView runat="server" ID="grdExigencias" ClientInstanceName="grdExigencias"
                                AutoGenerateColumns="False" Width="800" DataSourceID="odsCreditoeDebito"
                                KeyFieldName="OPERACAOEXIGENCIAID" OnRowUpdating="grdExigencias_RowUpdating"
                                OnCellEditorInitialize="grdExigencias_CellEditorInitialize"
                                OnHtmlRowCreated="grdExigencias_HtmlRowCreated">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                                <Columns>
                                         <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Exigência*" HeaderStyle-Font-Bold="true"
                                                FieldName="TIPOEXIGENCIAOPERACAOID" VisibleIndex="2" Width="150px">
                                                <PropertiesComboBox DataSourceID="odsTipoExigencia" TextField="DESCRICAO" ValueField="TIPOEXIGENCIAOPERACAOID"
                                                    ValueType="System.String">
                                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Favor informar o Tipo Exigencia." IsRequired="True" />
                                                    </ValidationSettings>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
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
                                            <asp:ImageButton ID="btnDetalhes" class="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("OPERACAOEXIGENCIAID") %>'
                                                OnCommand="btnDetalhes_Command" ImageUrl="~/img/upload.png" Height="15px" AlternateText="Importar Arquivo"
                                                Visible='<%# (Eval("JUSTIFICATIVA") == DBNull.Value) && (Eval("TIPOEXIGENCIAOPERACAOID") != DBNull.Value)  %>'></asp:ImageButton>
                    
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="5" Width="50px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("OPERACAOEXIGENCIAID") + "," + Eval("TIPOARQUIVO") %>'
                                                OnCommand="btnVisualizarExigencia_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                AlternateText="Visualizar Documento" Visible='<%# Eval("OPERACAOEXIGENCIAID") != DBNull.Value && Eval("OPERACAOEXIGENCIAARQUIVOID") != DBNull.Value %>'>
                                            </asp:ImageButton>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Aprovado"  Name="APROVADO"   FieldName="APROVADO" VisibleIndex="6" ReadOnly="True">
                                            <PropertiesTextEdit MaxLength="10"></PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                         
                                </Columns>
                            </dxwgv:ASPxGridView>
                           
                
                   </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>

        </tabpages>
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
                                <asp:FileUpload ID="FileUpload2" runat="server" />
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
                                <asp:TextBox ID="TxtJustificativaExigencia" runat="server" Width="100%"></asp:TextBox>
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
         <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
            runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
            ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
            CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Operação">
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
    
    
        
        
     <asp:Panel ID="pnlDocumento" runat="server" GroupingText="Arquivos" Font-Names="Verdana"
                Width="800px">
   

                    <table width="100%">
                                 <tr>
                                    <td>
                                     <asp:Label ID="lblDocumentoInserir" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="Documento"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <tweb:TSearchBox ID="TSeDocNecessario" runat="server" Argument="descricao" Key="DOCUMENTOSNECESSARIOSOPERACOESID"
                                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="Number" AutoPostBack="false"
                                        SqlSelect=" select descricao from PrestacaoContas.DOCUMENTOSNECESSARIOSOPERACOES">
                                           <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="DOCUMENTOSNECESSARIOSOPERACOESID" Width="10%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="90%" />
                                           </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="Javascript: VerificaTamanhoArquivo();" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td style="text-align: right;">
                                    <asp:Button ID="btnAnexar" runat="server" Text="Adicionar" OnClientClick="Bloqueio()"
                                        OnClick="btnAnexar_Click" />
                                    </td>
                                </tr>
                            </table>





                 <ContentCollection>
                        <dxw:ContentControl ID="ContentControl4" runat="server">
                            <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.IncluirCreditoeDebito"
                                SelectMethod="ListaDocumento"  OnDeleting="odsDocumento_Deleting" UpdateMethod="UpdateOperacaoDocumento" DeleteMethod="DeleteOperacaoDocumento">
                                <SelectParameters>
                                    <asp:ControlParameter Name="Id" ControlID="hdnOcorrenciaId" PropertyName="Value"
                                        Type="Int32" DefaultValue="1" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView runat="server" ID="grdDocumento" ClientInstanceName="grdDocumento"
                                AutoGenerateColumns="False" EnableCallBacks="true" Width="800" DataSourceID="odsDocumento"
                                KeyFieldName="OPERACAODOCUMENTOSID"            
                                OnInitNewRow="grdDocumento_InitNewRow"
                                OnRowDeleting="grdDocumento_RowDeleting"
                                OnHtmlRowCreated="grdDocumento_HtmlRowCreated">
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />    
                                  <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <SettingsBehavior ConfirmDelete="true" />                
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <HeaderCaptionTemplate>               
                                            </HeaderCaptionTemplate>
                                            <DeleteButton Visible="true" Text="Remover">
                                                <Image Url="../img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <CancelButton Visible="true" Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <ClearFilterButton Text="Limpar" Visible="True">
                                                <Image Url="~/img/bt_limpa.png" />
                                            </ClearFilterButton>
                                        </dxwgv:GridViewCommandColumn>                                
                                         <dxwgv:GridViewDataTextColumn Caption="ID" Visible=false FieldName="OPERACAODOCUMENTOSID" VisibleIndex="1"  Width="10%">
                                                <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle>
                                         </dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="DOCUMENTOSNECESSARIOSOPERACOESID" VisibleIndex="2" Width="30%">
                                                <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"> </CellStyle>
                                         </dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="Data Envio" FieldName="DATAENVIO" VisibleIndex="3" Width="20%">
                                                <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle>
                                         </dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="Anexo" FieldName="NOMEARQUIVO" VisibleIndex="4" Width="30%">
                                                <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle>
                                         </dxwgv:GridViewDataTextColumn> 
                                        <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="5" Width="50px">
                                            <DataItemTemplate>
                                                <asp:ImageButton ID="lnkVisualizar" runat="server" CommandArgument='<%# Eval("OPERACAODOCUMENTOSID") + "," + Eval("TIPOARQUIVO") %>'
                                                    OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px" AlternateText="Visualizar Documento">
                                                </asp:ImageButton>
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                            </dxwgv:ASPxGridView>
                
                        </dxw:ContentControl>
                    </ContentCollection>
   
        </asp:Panel>

    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="410px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Analisar Fornecedor">
        <headerstyle horizontalalign="Center" />
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <contentstyle verticalalign="Top">
        </contentstyle>
        <sizegripimage height="12px" width="12px" />
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <contentcollection>
            <dxpc:PopupControlContentControl>
                <asp:Panel ID="pnlConfirmação" runat="server" GroupingText="Confirmação" Width="80%">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblConfirmacao" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="false" Width="201px">
                                    <asp:ListItem Text="Aprovado" Value="Aprovado"></asp:ListItem>
                                    <asp:ListItem Text="Reprovado" Value="Reprovado"></asp:ListItem>
                                </asp:RadioButtonList>
                                
                                <script language="javascript">
                                    $(() => {
                                        $(document.getElementsByName("<%= rblConfirmacao.UniqueID %>")).on("click", (evt) => {
                                            let pnlMotivo = $("#<%= pnlMotivo.ClientID %>");
                                            if (evt.target.value === "Reprovado") {
                                                pnlMotivo.css("display", "block");
                                            }
                                            else {
                                                pnlMotivo.css("display", "none");
                                            }
                                        });
                                    });
                                </script>
                            </td>
                            <td>
                                <asp:Panel ID="pnlMotivo" runat="server" Visible="true" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Motivo Reprovação Fornecedor:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlMotivoReprovacaoFornecedor" runat="server" DataTextField="DESCRICAO"
                                                    DataValueField="MOTIVOREPROVACAOFORNECEDORID" AppendDataBoundItems="true" Width="201px">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hidMotivosSelecionados" runat="server" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnAdicionarMotivo" value="+" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="overflow: auto;height: 100px;width: 100%;">
                                                    <table id="tableMotivos">
                                                        <tbody></tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <script language="javascript">
                                        $(() => {
                                            $("#btnAdicionarMotivo").on("click", (evt) => {
                                                let text = $("#<%= ddlMotivoReprovacaoFornecedor.ClientID %>").find(":selected").text();
                                                let value = $("#<%= ddlMotivoReprovacaoFornecedor.ClientID %>").find(":selected").val();
                                                if (value != '') {
                                                    if ($('#tableMotivos > tbody > tr[data-value=\'' + value + '\']').length == 0) {
                                                        $("#tableMotivos > tbody").append('<tr data-value="' + value + '"><td><input type="button" class="btnRemoverMotivo" value="X" data-value=' + value + '></td><td>' + text + '</td></tr>');
                                                    }
                                                }
                                                
                                                $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                if ($('#tableMotivos > tbody > tr').length > 0) {
                                                    $('#tableMotivos > tbody > tr').each((index, el) => {
                                                        if (index > 0) {
                                                            $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + ";");
                                                        }
                                                        $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + $(el).attr("data-value"));
                                                    });
                                                }
                                                else {
                                                    $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                }
                                            });
                                        
                                            $("#tableMotivos > tbody").delegate(".btnRemoverMotivo", "click", (evt) => {
                                                let value = $(evt.target).attr("data-value");
                                                $('#tableMotivos > tbody > tr[data-value=\'' + value + '\']').remove();
                                                
                                                $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                if ($('#tableMotivos > tbody > tr').length > 0) {
                                                    $('#tableMotivos > tbody > tr').each((index, el) => {
                                                        if (index > 0) {
                                                            $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + ";");
                                                        }
                                                        $("#<%= hidMotivosSelecionados.ClientID %>").val($("#<%= hidMotivosSelecionados.ClientID %>").val() + $(el).attr("data-value"));
                                                    });
                                                }
                                                else {
                                                    $("#<%= hidMotivosSelecionados.ClientID %>").val("");
                                                }
                                            });
                                        });
                                    </script>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <br />
                <table id="Table2" runat="server">
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnConfirmarAnalise" runat="server" Text="Analisar" OnClick="btnConfirmarAnalise_Click"
                                OnClientClick="pucConfirmar.Hide(); return true;" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </contentcollection>
    </dxpc:ASPxPopupControl>
    
    <script language="javascript">
        var dtePeriodoPagamentoInicio, dtePeriodoPagamentoFim, dxeCalendar, tseUnidadeEnsino, tseUnidadeEnsinoGrid, tsePlanoTrabalho, tsePlanoTrabalhoGrid;
        $(document).ready(function() {
            
            tsePeriodoPrestacaoContas = $("#<%= tsePeriodoPrestacaoContas.ClientID %>");
            tsePeriodoPrestacaoContasGrid = $("#<%= tsePeriodoPrestacaoContas.ClientID %>_grid");
            tseUnidadeEnsino = $("#<%= tseUnidadeEnsino.ClientID %>");
            tseUnidadeEnsinoGrid = $("#<%= tseUnidadeEnsino.ClientID %>_grid");
            tsePlanoTrabalho = $("#<%= tsePlanoTrabalho.ClientID %>");
            tsePlanoTrabalhoGrid = $("#<%= tsePlanoTrabalho.ClientID %>_grid");

            tsePeriodoPrestacaoContas.on("change", filtrarEvento);
            tsePeriodoPrestacaoContasGrid.on("click", filtrarEvento);
            tseUnidadeEnsino.on("change", filtrarEvento);
            tseUnidadeEnsinoGrid.on("click", filtrarEvento);
            tsePlanoTrabalho.on("change", filtrarEvento);
            tsePlanoTrabalhoGrid.on("click", filtrarEvento);
        
        });

        function filtrarEvento(lock) {
            try {
                if (lock != "lock") {
                    window.setTimeout(() => filtrarEvento("lock"), 100);
                    return;
                }
                
                console.log("verificando filtragem...");
                
                if (tsePeriodoPrestacaoContas.val() == undefined || tsePeriodoPrestacaoContas.val() == null || tsePeriodoPrestacaoContas.val() == "")
                    return;

                if (tseUnidadeEnsino.val() == undefined || tseUnidadeEnsino.val() == null || tseUnidadeEnsino.val() == "")
                    return;
                    
                if (tsePlanoTrabalho.val() == undefined || tsePlanoTrabalho.val() == null || tsePlanoTrabalho.val() == "")
                    return;
                
                console.log("filtragem ok, filtrando...");
                    
                __doPostBack("FiltraEvento", null);
            }
            catch (ex) {
            }
        }
    </script>

</asp:Content>
