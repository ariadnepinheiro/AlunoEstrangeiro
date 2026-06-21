<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MonitoramentoFrequencia.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MonitoramentoFrequencia" %>

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
    <asp:Panel runat="server" ID="pnlMensagem" Width="70%" GroupingText="Monitoramento da Frequência dos Alunos:"
        class="style1">
        <br />
        <p class="style1">
            <b>A SEEDUC/RJ está disponibilizando o painel de Monitoramento da Frequência dos Alunos,
                a fim de subsidiar o trabalho da escola nas estratégias administrativas e pedagógicas,
                a partir das informações lançadas pelas Unidades Escolares na tela Controle Frequência
                - Retorno Presencial.<br /><br />
                O monitoramento é apresentado através de um dashboard interativo e auto instrucional,
                com estatísticas que podem ser consultadas ao nível de ensino, modalidade, oferta,
                curso, etapa e turma.
                <br />
                
        </p>
        <br />
        <p>
            <div>
                <ul style="list-style-type: square">
                    <li runat="server" id="liDiretor" visible="false">
                        <asp:HyperLink ID="hplMonitoramentoDiretor" Font-Size="12px" runat="server" Target="_blank"
                            Visible="false" Text="Monitoramento Escolar" NavigateUrl="https://sites.google.com/educa.rj.gov.br/frequenciaescolaseeducrj/"></asp:HyperLink>
                    </li>
                    <li runat="server" id="liRegional" visible="false">
                        <asp:HyperLink ID="hplMonitoramentoRegional" Font-Size="12px" runat="server" Target="_blank"
                            Text="Monitoramento Escolar" Visible="false" NavigateUrl="https://sites.google.com/educa.rj.gov.br/frequenciaregionalseedurj/"></asp:HyperLink>
                    </li>
                </ul>
            </div>
            <br />
        </p>
        <br />
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
