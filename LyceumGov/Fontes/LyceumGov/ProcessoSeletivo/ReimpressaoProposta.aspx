<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ReimpressaoProposta.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.ReimpressaoProposta" %>

<asp:Content ID="conReimpressaoProposta" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por processo seletivo e número de inscrição"
        Width="650px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblConcursoTSearch" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcurso" runat="server" Key="concurso" Argument="descricao"
                        OnChanged="tseConcurso_Changed" MaxLength="20" SqlSelect="SELECT concurso, descricao from LY_CONCURSO_DOCENTE cd"
                        ArgumentColumns="50" Columns="30" GridWidth="850px" SqlWhere="tipo = 'Contrato' and cd.DT_INICIO <= getdate() and cd.DT_FIM >= getdate()">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblInscricaoTSearch" runat="server" Text="Número de Inscrição:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseInscricao" runat="server" Key="candidato" Argument="nome"
                        OnChanged="tseInscricao_Changed" MaxLength="20" SqlSelect="SELECT candidato, nome, cpf, nucleo FROM LY_CANDIDATO_DOCENTE cd"
                        ArgumentColumns="50" Columns="30" GridWidth="850px" SqlWhere="cd.CONCURSO = #tseConcurso# and convert(decimal,cd.STATUS) >= 2">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número de Inscrição" FieldName="candidato" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="nucleo" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right"
                        Visible="true"/>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
</asp:Content>
