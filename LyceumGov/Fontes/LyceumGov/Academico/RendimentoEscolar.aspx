<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RendimentoEscolar.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.RendimentoEscolar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
            text-align: justify;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <%-- <asp:Panel runat="server" ID="pnlMensagem" Width="80%" GroupingText="Rendimento Escolar:"
        style = "border: none">
        <br />--%>
    <div style="width: 80%">
        <p class="style1">
            <b>Com vistas a disponibilizar informações e dados educacionais, em especial acerca
                do rendimento escolar dos nossos alunos, a Seeduc/RJ oferece o Painel de Rendimento
                Escolar às equipes escolares, coordenadores regionais e aos gestores da rede. Trata-se
                de um dashboard com as principais informações sobre o rendimento escolar em diversas
                agregações e níveis, apresentados prontamente ao término do lançamento de notas
                e frequência. Esse painel busca ser interativo e auto instrucional ao usuário na
                obtenção de dados acerca do rendimento dos alunos, reduzindo a necessidade do envio
                de planilhas estáticas e rígidas com tais dados.</b></p>
        <p>
            <b>
                <asp:Label ID="lblDiretor" runat="server" Style="font-family: Tahoma; font-size: Small;"
                    Text="Em caso de Dúvidas, para visualizar o guia de orientações da Unidade Escolar "
                    Visible="false" SkinID="lblObrigatorio"></asp:Label>
                <asp:HyperLink ID="hplGuiaDiretor" Font-Size="12px" runat="server" Target="_blank"
                    Text="clique aqui" Visible="false" NavigateUrl="http://aplicacoes.educacao.rj.gov.br/Arquivos/Modelo_de_guia_Escola.pdf"></asp:HyperLink>
            </b>
            <asp:Label ID="lblRegional" runat="server" Style="font-family: Tahoma; font-size: Small;"
                Text="Em caso de Dúvidas, para visualizar o guia de orientações " Visible="false" SkinID="lblObrigatorio"></asp:Label>
            <asp:HyperLink ID="hplGuiaRegional" Font-Size="12px" runat="server" Target="_blank"
                Visible="false" Text="clique aqui." NavigateUrl="http://aplicacoes.educacao.rj.gov.br/Arquivos/Modelo_de_guia_Regional.pdf"></asp:HyperLink>
        </p>
        <br />
    </div>
    <%--</asp:Panel>--%>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div id="IdentificacaoSuperior">
 <%--       <iframe id="frResultado" runat="server" width="80%" height="800px" frameborder="1"
            scrolling="auto"></iframe>--%>
            <a target="_blank" href="https://app.powerbi.com/Redirect?action=OpenApp&appId=e925459d-bcd1-44ab-8b5d-7feee4a6b29d&ctid=0c2829c9-41fa-4885-b057-a327fa5f37d4">
<img src="../Images/PAINDE.png" alt="Link para PAINDE">
</a>
    </div>
</asp:Content>
