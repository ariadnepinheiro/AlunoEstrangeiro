<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DadosEnsinoRemoto.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.DadosEnsinoRemoto" %>

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
    <asp:Panel runat="server" ID="pnlMensagem" Width="70%" GroupingText="Dados Ensino Remoto:"
        class="style1">
        <br />
        <p class="style1">
            <b>Desde o início da pandemia da Covid-19, a Secretaria de Estado de Educação do Rio
                de Janeiro (Seeduc-RJ) tem estabelecido metas e mecanismos para continuar com a
                oferta de educação de qualidade para a população fluminense. Diante dos desafios
                que a sociedade e o poder público têm enfrentado nos últimos meses, a Seeduc se
                mostra persistente na missão de fazer com que toda a rede de ensino se adapte ao
                &#8216;novo normal&#8217;, adequando as atividades pedagógicas, até então presenciais
                em sua maioria, para o ensino remoto, em diferentes vertentes e modalidades da Educação
                Básica.</b></p>
        <p class="style1">
            Neste sentido, faz-se necessário entender como se dá a dinâmica de acessos ao Applique-se
            e ao Google Classroom tanto pelos alunos, professores e gestores. Desta forma, será
            possível traçar estratégias para fomentar, cada vez mais, a utilização destes recursos
            didáticos.</p>
        <br />
        <div>
            <ul style="list-style-type:square">
                <li>
                    <asp:HyperLink ID="hpSeeducOnline" Font-Size="12px" runat="server" Target="_blank"
                        Text="Classroom - Acompanhamento de alunos e professores" NavigateUrl="http://aplicacoes.educacao.rj.gov.br/AcompanhamentoClassroom"></asp:HyperLink></li>
                        <br />
                <li>
                    <asp:HyperLink ID="hplPainelBI" Font-Size="12px" runat="server" Target="_blank" Text="Applique-se - Paineis de monitoramento de acessos"
                        NavigateUrl="https://sites.google.com/educa.rj.gov.br/appliqueseemnumeros"></asp:HyperLink></li></ul>
        </div>
        <br />
        <br />
        <p>
            <b>Em caso de Dúvidas, 
                <asp:HyperLink ID="HyperLink1" Font-Size="12px" runat="server" Target="_blank" Text="clique aqui"
                    NavigateUrl="http://aplicacoes.educacao.rj.gov.br/Arquivos/Guia_de_orientações.pdf"></asp:HyperLink>
                e visualize o guia de orientações.</b></p>
        <br />
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
