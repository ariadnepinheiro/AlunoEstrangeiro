<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Consideracoes.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.Consideracoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidade_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp" AutoPostBack="true">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />                              
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCampanha" runat="server" Text="Campanha:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCampanha" runat="server" Key="campanhaid" Argument="titulo"
                        OnChanged="tseCampanha_Changed" MaxLength="8" SqlSelect="SELECT campanhaid ,titulo, Convert(varchar(4), ano) ano, Convert(varchar(1), semestre)semestre from inspecaoescolar.campanha"
                        GridWidth="850px" SqlOrder="titulo" DataType="Number" AutoPostBack="true">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="codigo" FieldName="campanhaid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Titulo" FieldName="titulo" Width="70%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ano" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Semestre" FieldName="semestre" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagemFinalizacao" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 750px;">
        <asp:Label runat="server" ID="lblBloco" Text="Considerações" SkinID="BcTitulo" />
        <asp:ImageButton ID="btnAceitar" runat="server" SkinID="BcSalvar" OnClick="btnAceitar_Click"
            ToolTip="Aceitar Campanha" OnClientClick="return confirm('Após a ratificação do Relatório de Trabalho de Infraestrutura não será possível efetuar alterações. Deseja prosseguir?');" />
    </div>
    <div>
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
        <asp:HiddenField ID="hdnCampanhaEscolaId" runat="server" />
        <asp:HiddenField ID="hdnFinalizado" runat="server" />
        <asp:HiddenField ID="hdnDataFinalizacao" runat="server" />
    </div>
    <br />
    <asp:Panel ID="pnlConsideracoes" runat="server" GroupingText="">
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Considerações Finais:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtConsideracoes" TextMode="MultiLine" runat="server" Width="600px"
                        MaxLength="2000" Style="overflow: auto" Height="120px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAceite" runat="server" SkinID="lblObrigatorio" Text="O Diretor(a) ratifica o preenchimento do RT?*"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblAceite" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
