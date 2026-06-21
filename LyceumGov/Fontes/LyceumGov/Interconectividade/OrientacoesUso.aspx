<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="OrientacoesUso.aspx.cs" Inherits="Techne.Lyceum.Net.Interconectividade.OrientacoesUso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <table>
        <tr>
            <td>
                <div class="mensagem">
                    <strong style="font-size: 12px; color: #0353AB;">Para saber mais sobre link:</strong>
                    <asp:HyperLink ID="SaibaMais" Font-Size="12px" runat="server" Target="_blank" Text="Clique Aqui."
                        NavigateUrl="http://aplicacoes.educacao.rj.gov.br/Arquivos/Manual_Fiscalizacao_link.pdf"></asp:HyperLink>
                    <br />
                    <div id="msg" runat="server" style="font-weight: bold; font-size: 12px; color: #0353AB;">
                        &nbsp;</div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
