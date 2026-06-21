<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="InscricaoCompartilhadas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.InscricaoCompartilhadas"
    Title="Inscrição de Compartilhadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <fieldset>
        <legend>Unidades de Ensino Compartilhadas</legend>
        <table>
            <tr>
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Ano / Período:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAnoPeriodo" runat="server" AutoPostBack="true" DataTextField="ANOPERIODO"
                        Enabled="false" DataValueField="ANOPERIODO" OnSelectedIndexChanged="ddlAnoPeriodo_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Unidade de Ensino de Destino:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="left">
                    <tweb:TSearchBox ID="tseUnidadeEnsinoDestino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidadeEnsinoDestino_Changed" MaxLength="20" ArgumentColumns="50"
                        Columns="10" AutoPostBack="true" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="1050px" SqlOrder="nome_comp">
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
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Unidade de Ensino de Origem:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUnidadeEnsinoOrigem" runat="server" AutoPostBack="true"
                        DataTextField="unidade_ensino_origem" DataValueField="unidade_ens" Width="410px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label Font-Names="Verdana" ID="lblCurso" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCurso" runat="server" DataTextField="NOME" Enabled="false"
                        AutoPostBack="true" DataValueField="CURSO" Width="410px" OnSelectedIndexChanged="ddlCurso_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Série / Ano Escolar:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" Enabled="false"
                        DataValueField="serie" Width="410px" />
                </td>
            </tr>
        </table>
    </fieldset>
    <p>
        <asp:Button ID="btnInscricao" runat="server" Text="Inscrição" OnClick="btnInscricao_Click" />
    </p>
    <p>
        <asp:Label ID="lblMsg" runat="server" Text="" /></p>
</asp:Content>
