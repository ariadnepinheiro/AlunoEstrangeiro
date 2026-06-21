<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Boletim.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Boletim" %>

<asp:Content ID="cListarTurma" ContentPlaceHolderID="cphFormulario" runat="server">
    <link rel="stylesheet" href="../Styles/Boletim.css" type="text/css" />

    <script type="text/javascript">

        $(function() {
            $("#btnPrint").click(function() {
                var contents = $("#<%= this.divGrdBoletim.ClientID %>").html();
                var contentsPrincipal = $("#<%= this.divPrincipal.ClientID %>").html();
                var contentsAssinatura = $("#<%= this.divAssinatura.ClientID %>").html();
                var contentsMSG = $("#<%= this.divmsg.ClientID %>").html();
                var contentsInformativo = $("#<%= this.divInformativo.ClientID %>").html();
                var contentsPortaria = $("#<%= this.divPortaria.ClientID %>").html();
                var frame1 = $('<iframe />');
                frame1[0].name = "frame1";
                frame1.css({ "position": "absolute", "top": "-1000000px" });
                $("body").append(frame1);
                var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                frameDoc.document.open();
                //Create a new HTML document.
                frameDoc.document.write('<html><head><title>Boletim do Aluno</title>');
                frameDoc.document.write('</head><body>');
                //Append the external CSS file.
                frameDoc.document.write('<link href="../Styles/BoletimImpressao.css" rel="stylesheet" type="text/css" />');
                //Append the DIV contents.
                frameDoc.document.write(contentsPrincipal);
                frameDoc.document.write(' <br /> <br /> <br />');
                frameDoc.document.write(contentsMSG);
                frameDoc.document.write(' <br /> <br /> <br />');
                frameDoc.document.write(contents);
                frameDoc.document.write(' <br /> <br /> <br />');
                frameDoc.document.write(contentsAssinatura);
                frameDoc.document.write(' <br /> <br /> <br />');
                frameDoc.document.write(contentsPortaria);
                frameDoc.document.write(' <br /> <br /> <br />');
                frameDoc.document.write(contentsInformativo);
                frameDoc.document.write('</body></html>');
                frameDoc.document.close();
                setTimeout(function() {
                    window.frames["frame1"].focus();
                    window.frames["frame1"].print();
                    frame1.remove();
                }, 500);
            });
        });

        function formataFixoCelularDDD(b, a) {
            vr = b.value = filtraNumeros(filtraCampo(b));
            tam = vr.length;
            if (tam < 10)
                return;

            if (tam == 11) {
                formataCelularDDD(b, a);
            }
            if (tam == 10) {
                formataTelefoneDDD(b, a);

            }
        }

        function formataCelularDDD(b, a) {

            a = getEvent(a);
            var c = getKeyCode(a);
            if (!teclaValida(c)) {
                return
            }
            vr = b.value = filtraNumeros(filtraCampo(b));
            if (vr.length > 11) {
                vr = vr.substr(0, 11)
            }
            tam = vr.length;

            if (tam <= 2) {
                b.value = "(" + vr + ")"

            }
            if (tam <= 6) {
                b.value = "(" + vr.substr(0, 2) + ")" + vr.substr(2, 5)

            }
            if (tam > 6) {
                b.value = "(" + vr.substr(0, 2) + ")" + vr.substr(2, 5) + "-" + vr.substr(7, 4)

            }
        }

        function formataTelefoneDDD(b, a) {
            a = getEvent(a);
            var c = getKeyCode(a);
            if (!teclaValida(c)) {
                return
            }
            vr = b.value = filtraNumeros(filtraCampo(b));
            if (vr.length > 10) {
                vr = vr.substr(0, 10)
            }
            tam = vr.length;
            if (tam <= 2) {
                b.value = "(" + vr + ")"
            }
            if (tam <= 6) {
                b.value = "(" + vr.substr(0, 2) + ")" + vr.substr(2, 4)
            }
            if (tam > 6) {
                b.value = "(" + vr.substr(0, 2) + ")" + vr.substr(2, 4) + "-" + vr.substr(6, 4)
            }
        }
    </script>

    <asp:ScriptManager ID="manager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAtualizar" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField ID="hdnAno" runat="server" />
            <asp:HiddenField ID="hdnSemestre" runat="server" />
            <asp:HiddenField ID="hdnMatriculaAtiva" runat="server" />
            <asp:HiddenField ID="hdnMatriculaHistorico" runat="server" />
            <asp:HiddenField ID="hdnQuantidadeBimestres" runat="server" />
            <div id="divPrincipal" runat="server">
                <table width="820px">
                    <tr>
                        <td style="width: 10px">
                            <asp:Image ID="Image1" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                                ImageUrl="~/Images/logo-educacao.png" Style="text-align: right" />
                        </td>
                        <td class="titulo">
                            BOLETIM DO ALUNO
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <table class="tablePrincipal">
                    <tr>
                        <td class="tdTituloPrincipal">
                            Aluno(a):
                        </td>
                        <td colspan="3" class="tdPrincipal">
                            <asp:Label ID="lblNome" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTituloPrincipal">
                            Unidade de Ensino:
                        </td>
                        <td colspan="3" class="tdPrincipal">
                            <asp:Label ID="lblUnidade" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTituloPrincipal">
                            Ano:
                        </td>
                        <td class="tdPrincipal">
                            <asp:Label ID="lblAno" runat="server"></asp:Label>
                        </td>
                        <td class="tdTituloPrincipal">
                            Periodo:
                        </td>
                        <td class="tdPrincipal">
                            <asp:Label ID="lblPeriodo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTituloPrincipal">
                            Escolaridade:
                        </td>
                        <td colspan="3" class="tdPrincipal">
                            <asp:Label ID="lblEscolaridade" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTituloPrincipal">
                            Ano de Escolaridade:
                        </td>
                        <td class="tdPrincipal">
                            <asp:Label ID="lblAnoEscolaridade" runat="server"></asp:Label>
                        </td>
                        <td class="tdTituloPrincipal">
                            Turno:
                        </td>
                        <td class="tdPrincipal">
                            <asp:Label ID="lblTurno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTituloPrincipal">
                            Turma Principal:
                        </td>
                        <td colspan="3" class="tdPrincipal">
                            <asp:Label ID="lblTurmaPrincipal" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="divSituacaoFinal" runat="server" visible="true">
                <table>
                    <tr>
                        <td>
                            Situação Final da Turma Principal:
                        </td>
                        <td>
                            <asp:Label ID="lblSituacaoFinal" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                           <asp:Label ID="lblAmparoResolucao" Text="Amparado pela Resolução SEEDUC/SUGEN nº 5.879/2020 D.O. 14/10/2020"
                           runat="server" Font-Bold="True" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="divMensagemSituacao2020" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblMensagemSituacao2020" Text="Prezado aluno(a) ou responsável, no momento, para  informações sobre o Boletim do Aluno, procure sua Unidade Escolar."
                                runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="divTelefone" runat="server">
                <table>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblFone" runat="server" Text="Telefone do Responsável:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                                MaxLength="14" Width="100px" placeholder="(99)99999-9999" />
                        </td>
                        <td>
                            <asp:Button ID="btnAtualizar" runat="server" Text="Atualizar" ValidationGroup="AtualizarFone"
                                OnClick="btnAtualizar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divmsg" runat="server" visible="true">
            <table>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblMensagem" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblUltimaAtualizacao" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <asp:Label ID="lblTipoMaricula" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            </div>
            <br />
            <br />
            <div runat="server" id="divGrdBoletim">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <div id="divAssinatura" runat="server" style="display: none;">
                <table>
                    <tr>
                        <td style="border-top: 2px  solid; width: 300px" align="center">
                            Assinatura do Responsável
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="border-top: 2px  solid; width: 300px" align="center">
                            Assinatura do Diretor ou Secretário
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <div style="width: 85%;" id="divImprimir" runat="server">
                            <input type="image" src="../Images/bot_imprimir.png" value="Print" id="btnPrint" />
                        </div>
                    </td>
                    <td style="color: Red;">
                        <div id="divPortaria" runat="server">
                            O boletim do aluno traz informações das notas, das faltas e o percentual de frequência
                            por disciplina a cada bimestre/trimestre.<br />
                            O percentual de frequência é contabilizado a partir do total de aulas dadas e do
                            total de faltas do aluno no bimestre/trimestre.<br />
                            O percentual de <b>Frequência Global do Aluno na Turma</b> é resultado das frequências
                            acumuladas bimestralmente/trimestralmente.<br />
                            Para maiores informações, consultar a
                            <asp:HyperLink ID="hpPortaria" runat="server" Target="_blank" NavigateUrl="http://download.rj.gov.br/documentos/10112/2298861/DLFE-78181.pdf/PORTARIASEEDUCSUGENN419DE27.09.2013DO30.09.2013NORMASDEAVALIACAO.pdf">Resolução SEEDUC nº 6303, de 08 de novembro de 2024</asp:HyperLink>.
                        </div>
                    </td>
                </tr>
            </table>
            <div id="divInformativo" runat="server">
                <table>
                    <tr>
                        <td colspan="2" style="color: Red;">
                            Documento para simples conferência, sua autenticidade está condicionada à validação
                            do Diretor ou Secretário de sua Unidade Escolar.
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
