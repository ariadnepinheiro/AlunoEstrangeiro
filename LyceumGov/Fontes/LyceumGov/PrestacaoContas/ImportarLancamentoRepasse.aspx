<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ImportarLancamentoRepasse.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ImportarLancamentoRepasse" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informações da Unidade de Ensino"
        Width="600px">
        <table>
       
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Programação Orçamentária:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                 <tweb:TSearchBox ID="tsePO" runat="server" MaxLength="9" DataType="Number" Argument="NOME" Key="planilhaorcamentariaid"
                        OnChanged="tsePO_Changed" SqlSelect="select PLANILHAORCAMENTARIAID,DESCRICAO,PROCESSO  from  PrestacaoContas.VW_PLANILHAORCAMENTARIA"
                        SqlWhere=" aprovada = 1 ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANILHAORCAMENTARIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="Processo" FieldName="PROCESSO" Width="40%" />
                        </GridColumns>
                    </tweb:TSearchBox>     
                  
                </td>
        
            </tr>
                  
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Text="Parcela da Programação Orçamentária:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>                  
                    
                    <tweb:TSearchBox ID="tsePPO" runat="server" Key="ITEMPLANILHAORCAMENTARIAID" Argument="ANO_MES"
                        SqlWhere=" PLANILHAORCAMENTARIAID = #tsePO# " MaxLength="9"  OnChanged="tsePPO_Changed"
                        DataType="Number" SqlSelect="select PLANILHA,ANO,MES,PLANILHAORCAMENTARIAID from  PrestacaoContas.VW_ITEMPLANILHAORCAMENTARIA ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="ITEMPLANILHAORCAMENTARIAID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="PLANILHA" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Mês Referência" FieldName="MES" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
        
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Text="Selecione o arquivo a ser importado"></asp:Label>
                    <input id="arquivo" type="file" runat="server" name="oFile">
                    <script language="javascript">
                        $(() => {
                            $("#<%= arquivo.ClientID %>").on("change", (evt) => {
                                let tamanhoCsv = evt.target.files[0].size;
                                if (tamanhoCsv > 4194304) { //4mb
                                    evt.target.value = "";
                                    alert("O CSV a ser importado não pode ter mais do que 4MB.");
                                    return false;
                                }
                            });
                        });
                    </script>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnImportar" runat="server" Text="Importar" OnClick="btnImportar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGridCoordenadoria" runat="server" Visible="false">
    </asp:Panel>
</asp:Content>
