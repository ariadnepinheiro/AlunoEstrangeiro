<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalisareVerificarCreditoeDebitoCOOREF.aspx.cs" 
MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.AnalisareVerificarCreditoeDebitoCOOREF" %>

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
     function abrirPopup() {
         window.setTimeout(function() {
             pucConfirmar.Show();
         }, 1000);
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
    <asp:HiddenField runat="server" ID="hdnIdTermo" />
    <asp:HiddenField runat="server" ID="hdnDadosPublicacao" />
    <asp:HiddenField runat="server" ID="hdnAba" />
    <asp:HiddenField runat="server" ID="hdnprogramatrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid" />
    <asp:HiddenField runat="server" ID="hdnplanotrabalhoid2" />
    
    <asp:HiddenField ID="hdnArquivoId" runat="server" />
    <asp:HiddenField ID="hdnQueryString" runat="server" />
    <asp:HiddenField ID="hdnOcorrenciaId" runat="server" />
    <asp:HiddenField ID="hdnPeriodoReferencia" runat="server" />
    <asp:HiddenField ID="hdnPerfil" runat="server" />
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Programa/Projeto" Width="800px">
         <table class="table-fixed" width="100%">
            <tr>
                <td align="left"  width="20%">
                    <span>Projeto / Programa: </span>
                </td>
                <td>
                      <asp:Label ID="lblProjeto" runat="server" style="font-size: 12px;"></asp:Label>
                </td>                            
            </tr>
            
           <tr>
                <td  align="left" width="20%">
                    <span>Unidade de Ensino: </span>
                </td>
                <td  align="left">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" style="font-size: 12px;"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td align="left" width="20%">
                    <span>Tipo da Operação:</span>
                </td>
                <td  align="left"> 
                    <asp:Label ID="lblOperacao" runat="server" style="font-size: 12px;"></asp:Label>
                </td>    
            </tr> 
            
            <tr>
                <td align="left" width="20%">
                    <span>Código da Operação:</span>
                </td>
                <td  align="left"> 
                    <asp:Label ID="lblcodigooperacao" runat="server" style="font-size: 12px;"></asp:Label>
                </td>    
            </tr>
        </table>
    </asp:Panel>
    
    
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableViewState="true"></asp:Label>
    <br />
    
    <asp:PlaceHolder ID="plaCreditoeDebito" runat="server">

        <asp:HiddenField ID="hidExtratoBancarioId" runat="server" />
        <asp:HiddenField ID="hidMes" runat="server" />
        <asp:HiddenField ID="hidAno" runat="server" />
        <asp:HiddenField ID="hidUnidadeEnsino" runat="server" />
        <asp:HiddenField ID="hidStatus" runat="server" />
    
        <div class="divEditBlock" style="width: 800px;">    
            <asp:Label runat="server" ID="lblTitulo" Text="Análise de Crédito e Débito" SkinID="BcTitulo" />
            <asp:ImageButton ID="btnAprovar" runat="server" Visible="true" ImageUrl="~/Images/smallAprovar.png" ImageAlign="right" OnClick="btnAprovar_Click" OnClientClick="return confirm('Confirma a aprovação desta operação?');" />
            <asp:ImageButton ID="btnReprovar" runat="server" Visible="true" ImageUrl="~/Images/smallReprovar.png" ImageAlign="right" OnClick="btnReprovar_Click" OnClientClick="return confirm('Confirma a reprovação desta operação?');" />
            <asp:ImageButton ID="btnDevolver" runat="server" Visible="true" ImageUrl="~/Images/smallDevolverOperacao.png" ImageAlign="right" OnClick="btnDevolver_Click" />
            <asp:ImageButton ID="btnCancel" runat="server" ImageAlign="Right" SkinID="Voltar" OnClick="btnCancel_Click" />
        </div>
        <br />
        <br />    

        <asp:PlaceHolder ID="plaStatus" runat="server" Visible="true">
            <span style="font-weight: bold; font-size: 18px;">Status: </span>
            <asp:Label ID="lblStatus" runat="server" style="font-size: 16px;"></asp:Label>
            <br />
        </asp:PlaceHolder>
    
    <dxtc:ASPxPageControl ID="pcExtratoBancario" runat="server" ActiveTabIndex="0" OnTabClick="pcExtratoBancario_TabClick" Width="800px">

         <tabpages>
            <dxtc:TabPage Text="Informações da Operação">
                            <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                         <table>
                            <tr>
                                <td>
                                  <asp:Label ID="Label2" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text="CNPJ AAE"></asp:Label>
                                </td>
                                <td>
                                  <asp:Label ID="lblCNPJ" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                        Text=""></asp:Label>
                                </td>
                                  </tr>
                                    <tr> 
                                      <td style="text-align: right;">
                                         <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Data Envio:"></asp:Label>
                                      </td>
                                      <td>
                                         <asp:Label Font-Names="Verdana" ID="lblDataEnvio" runat="server" Text=""></asp:Label>
                                      </td>
                                   </tr>
                               <tr>
                                <td style="text-align: right;">
                                   <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Valor:"></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="txtValor" runat="server" MaxLength="10" Width="190px" CssClass="numeric"
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
              
                            <asp:ObjectDataSource ID="odsTipoExigencia" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisareVerificarCreditoeDebito"
                              SelectMethod="ListaTipoExigencias" >
                            </asp:ObjectDataSource>
                   
                        
                            <asp:ObjectDataSource ID="odsCreditoeDebito" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisareVerificarCreditoeDebito"
                                SelectMethod="ListaExigencias" InsertMethod="Insert" UpdateMethod="Update"
                                DeleteMethod="Delete" >  
                                <SelectParameters>
                                    <asp:ControlParameter Name="Id" ControlID="hdnOcorrenciaId" PropertyName="Value"
                                        Type="Int32" DefaultValue="1" />
                                </SelectParameters>            
                            </asp:ObjectDataSource>
                            
                            <dxwgv:ASPxGridView ID="grdExigencias" runat="server" DataSourceID="odsCreditoeDebito"
                                OnCellEditorInitialize="grdExigencias_CellEditorInitialize"
                                KeyFieldName="OPERACAOEXIGENCIAID" AutoGenerateColumns="false" ClientInstanceName="grdExigencias"
                                OnInitNewRow="grdExigencias_InitNewRow" OnStartRowEditing="grdExigencias_StartRowEditing"
                                OnRowInserting="grdExigencias_RowInserting" OnRowUpdating="grdExigencias_RowUpdating"
                                OnCustomButtonInitialize="grdExigencias_CustomButtonInitialize"
                                OnCustomButtonCallback="grdExigencias_CustomButtonCallback"
                                OnRowDeleting="grdExigencias_RowDeleting" Width="700px">
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                    onclick="grdExigencias.AddNewRow();" />
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
                                        <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnAprovarExigencia" Text="Aprovar Exigência" Visibility="Invisible">
                                            <Image Url="~/img/bt_ok.png" />
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnRejeitarExigencia" Text="Rejeitar Exigência" Visibility="Invisible">
                                            <Image Url="~/img/bt_desabilitar.png" />
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="OPERACAOEXIGENCIAID" Name="OPERACAOEXIGENCIAID" VisibleIndex="1" FieldName="OPERACAOEXIGENCIAID" Visible="false" Width="700px">
                                        <PropertiesTextEdit MaxLength="200"></PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="OPERACAOID" Name="OPERACAOID" VisibleIndex="2" FieldName="OPERACAOEXIGENCIAID" Visible="false" Width="700px">
                                        <PropertiesTextEdit MaxLength="200"></PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                              
                                                         
                                    
                                    
                                    
                                        <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Exigência*" HeaderStyle-Font-Bold="true"
                                                FieldName="TIPOEXIGENCIAOPERACAOID" VisibleIndex="3" Width="150px">
                                                <PropertiesComboBox DataSourceID="odsTipoExigencia" TextField="DESCRICAO" ValueField="TIPOEXIGENCIAOPERACAOID"
                                                    ValueType="System.String">
                                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Favor informar o Tipo Exigencia." IsRequired="True" />
                                                    </ValidationSettings>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                   
                                    <dxwgv:GridViewDataTextColumn Caption="Nota Explicativa*" Name="NOTAEXPLICATIVA" VisibleIndex="4" FieldName="NOTAEXPLICATIVA" Width="120px">
                                      <PropertiesTextEdit MaxLength="100">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Favor informar a Nota Explicativa." IsRequired="True" />
                                            </ValidationSettings>
                                      </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn> 
                                                                        
                                    
                                    <dxwgv:GridViewDataTextColumn Caption="Justificativa"    Name="JUSTIFICATIVA"  VisibleIndex="5" FieldName="JUSTIFICATIVA" Width="400px"  ReadOnly="True">
                                        <PropertiesTextEdit MaxLength="100"></PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>                                   

                                    
                                    <dxwgv:GridViewDataTextColumn Caption="Aprovado"  Name="APROVADO"   FieldName="APROVADO" VisibleIndex="6" ReadOnly="True">
                                            <PropertiesTextEdit MaxLength="10"></PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="7" Width="50px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("OPERACAOEXIGENCIAID") + "," + Eval("TIPOARQUIVO") %>'
                                                OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                AlternateText="Visualizar Documento" Visible='<%# Eval("OPERACAOEXIGENCIAID") != DBNull.Value && Eval("OPERACAOEXIGENCIAARQUIVOID") != DBNull.Value %>'>
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

            </dxtc:TabPage>

        </tabpages>
        



    
 </dxtc:ASPxPageControl>
     <asp:Panel ID="pnlDocumento" runat="server" GroupingText="Arquivos" Font-Names="Verdana"  Width="800px">
   
                 <ContentCollection>
                        <dxw:ContentControl ID="ContentControl4" runat="server">
                            <asp:ObjectDataSource ID="odsDocumento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisareVerificarCreditoeDebito"
                                SelectMethod="ListaDocumento" >
                                <SelectParameters>
                                    <asp:ControlParameter Name="Id" ControlID="hdnOcorrenciaId" PropertyName="Value"
                                        Type="Int32" DefaultValue="1" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView runat="server" ID="grdDocumento" ClientInstanceName="grdDocumento"
                                AutoGenerateColumns="False" EnableCallBacks="false" Width="800" DataSourceID="odsDocumento"
                                KeyFieldName="OPERACAODOCUMENTOSID" >
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                      
                                    <Columns>                                       
                                         <dxwgv:GridViewDataTextColumn Caption="ID"         Visible=false FieldName="OPERACAODOCUMENTOSID"             VisibleIndex="3" Width="1%" ><CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle></dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="OPERACAOID" Visible=false FieldName="OPERACAOID"                       VisibleIndex="2" Width="1%" ><CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle></dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="Descrição"                FieldName="DOCUMENTOSNECESSARIOSOPERACOESID" VisibleIndex="4" Width="30%"><CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle></dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="Data Envio"               FieldName="DATAENVIO"                        VisibleIndex="5" Width="20%"><CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle></dxwgv:GridViewDataTextColumn>
                                         <dxwgv:GridViewDataTextColumn Caption="Anexo"                    FieldName="NOMEARQUIVO"                      VisibleIndex="4" Width="30%"><CellStyle HorizontalAlign="Justify" VerticalAlign="Middle"></CellStyle></dxwgv:GridViewDataTextColumn>                                    
                                       
                                </Columns>
                            </dxwgv:ASPxGridView>
                
                        </dxw:ContentControl>
     </ContentCollection>
    </asp:Panel>
    
                      <asp:ObjectDataSource ID="odsMotivoReprovacao" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisareVerificarCreditoeDebito"
                              SelectMethod="ListaTipoReprovacao" >
                            </asp:ObjectDataSource>
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
                            <asp:DropDownList ID="cmbMotivo" runat="server" DataValueField="MOTIVOREPROVACAOOPERACAOID" DataTextField="DESCRICAO"
                                DataSourceID="odsMotivoReprovacao">
                            </asp:DropDownList>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btConfirma" runat="server" Text="Confirma" OnClick="btConfirmaReprovacao_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide();" />
                            <asp:Label ID="hID" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hQtde" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>

 <asp:Repeater ID="repCarrossel" runat="server" DataSourceID="odsDocumento" OnItemDataBound="repCarrossel_ItemDataBound">
                            <HeaderTemplate>
                                <div class="glider-contain" style="width: 800px;">
                                    <p style="text-align: center; color: red; font-size: 14px;">Clique nos círculos abaixo para navegar no carrossel de documentos abaixo</p><div role="tablist" class="dots"></div>
                                    <div class="glider">
                            </HeaderTemplate>
                           <ItemTemplate>
                                <div style="text-align: center">
                                    <div style="font-size: 16px; text-align: center; font-weight: bold; color: black;">
                                        <p><%# DataBinder.Eval(Container.DataItem, "NOMEARQUIVO")%></p>
                                    </div>
                                    
                                    <asp:PlaceHolder ID="plaTipoPDF" runat="server" Visible="false">
                                    <object data="FileCS.ashx?Tabela=OperacaoDocumentos&Id=<%# Eval("OPERACAODOCUMENTOSID") %>" type="application/pdf" width="800px" height="1170px">
	                                    <iframe src="FileCS.ashx?Tabela=OperacaoDocumentos&Id=<%# Eval("OPERACAODOCUMENTOSID") %>" width="100%" height="100%" style="border: none;">
		                                    <p>Your browser does not support PDFs.<a href="FileCS.ashx?Tabela=OperacaoDocumentos&Id=16">Download the PDF</a>.</p>
	                                    </iframe>
                                    </object>
                                    </asp:PlaceHolder>
                                    
                                    <asp:PlaceHolder ID="plaTipoImagem" runat="server" Visible="false">
                                    <img src="FileCS.ashx?Tabela=OperacaoDocumentos&Id=<%# Eval("OPERACAODOCUMENTOSID") %>" />
                                    </asp:PlaceHolder>
                                    
                                    <asp:PlaceHolder ID="plaSemArquivo" runat="server" Visible="false">
                                    <div style="font-size: 12px; text-align: center; font-weight: bold; color: black;">
                                        <p>(Nenhum arquivo enviado)</p>
                                    </div>
                                    </asp:PlaceHolder>
                                    
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </div>
                                </div>
                                
                                <script src="https://cdn.jsdelivr.net/npm/glider-js@1/glider.min.js"></script>
    
                                <script language="javascript">
                                    var glider = new Glider(document.querySelector('.glider'), {
                                      slidesToShow: 1,
                                      slidesToScroll: 1,
                                      draggable: true,
                                      dots: ".dots",
                                      arrows: {
                                        prev: ".prev",
                                        next: ".next",
                                      },
                                    });
                                </script>
                            </FooterTemplate>
                        </asp:Repeater>
                        
         
                        
    </asp:PlaceHolder>
    
     
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
</asp:Content>