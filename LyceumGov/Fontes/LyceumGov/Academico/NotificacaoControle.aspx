<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="NotificacaoControle.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.NotificacaoControle" %>

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

    <script type="text/javascript">

        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tseCEPOficioConselho.ClientID %>',
                cep: '<%=txtCEPOficioConselho.ClientID %>',
                nomeLogradouro: '<%=txtEnderecoOficioConselho.ClientID %>',
                codigoMunicipio: '<%=hdnMunicipioOficioConselho.ClientID %>',
                nomeMunicipio: '<%=txtMunicipioOficioConselho.ClientID %>',
                uf: '<%=txtEstadoOficioConselho.ClientID %>'
            });

            preencherDadosPorCEP2({ tscep: '<%=tseCEPOficioMPRJ.ClientID %>',
                cep: '<%=txtCEPOficioMPRJ.ClientID %>',
                nomeLogradouro: '<%=txtEnderecoOficioMPRJ.ClientID %>',
                nomeMunicipio: '<%=txtMunicipioOficioMPRJ.ClientID %>',
                codigoMunicipio: '<%=hdnMunicipioOficioMPRJ.ClientID %>',
                uf: '<%=txtEstadoOficioMPRJ.ClientID %>',
                numero: '<%=txtEndNumOficioMPRJ.ClientID %>',
                bairro: '<%=txtBairroOficioMPRJ.ClientID %>'
            });


            AddEvents();
        });
        function AddEvents() {

        }

 
    </script>

    <script type="text/javascript">

        $(function() {
            $("#btnImprimirConselho").click(function() {

                var nav = navigator.userAgent.toLowerCase();
                var printContent = document.getElementById("<%=divPrincipalConselho.ClientID %>");
                var title = document.title;

                if (nav.indexOf("chrome") != -1) {
                    var frame1 = $('<iframe />');
                    frame1[0].name = "frame1";
                    frame1.css({ "position": "absolute", "top": "-1000000px" });
                    $("body").append(frame1);
                    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                    frameDoc.document.open();
                    frameDoc.document.write('<html><head><title>' + title + '</title>');
                    frameDoc.document.write('</head><body>');
                    frameDoc.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    frameDoc.document.write(printContent.innerHTML);
                    frameDoc.document.write('</body></html>');
                    frameDoc.document.close();
                    setTimeout(function() {
                        window.frames["frame1"].focus();
                        window.frames["frame1"].print();
                        frame1.remove();
                    }, 500);
                }
                else {

                    var windowUrl = 'about:blank';
                    var windowName = 'Impressão';
                    var printWindow = window.open(windowUrl, windowName, 'width=1850,height=800');

                    printWindow.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    printWindow.document.write(printContent.innerHTML);
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                }
            });
        });

        $(function() {
            $("#btnImprimirMP").click(function() {

                var nav = navigator.userAgent.toLowerCase();
                var printContent = document.getElementById("<%=divPrincipalMP.ClientID %>");
                var title = document.title;

                if (nav.indexOf("chrome") != -1) {
                    var frame1 = $('<iframe />');
                    frame1[0].name = "frame1";
                    frame1.css({ "position": "absolute", "top": "-1000000px" });
                    $("body").append(frame1);
                    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                    frameDoc.document.open();
                    frameDoc.document.write('<html><head><title>' + title + '</title>');
                    frameDoc.document.write('</head><body>');
                    frameDoc.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    frameDoc.document.write(printContent.innerHTML);
                    frameDoc.document.write('</body></html>');
                    frameDoc.document.close();
                    setTimeout(function() {
                        window.frames["frame1"].focus();
                        window.frames["frame1"].print();
                        frame1.remove();
                    }, 500);
                }
                else {

                    var windowUrl = 'about:blank';
                    var windowName = 'Impressão';
                    var printWindow = window.open(windowUrl, windowName, 'width=1850,height=800');

                    printWindow.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    printWindow.document.write(printContent.innerHTML);
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                }
            });
        });   
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="" Height="70px" Width="580px">
        <table>
            <tr>
                <td>
                    Aluno:
                </td>
                <td>
                    <asp:TextBox ID="txtAluno" runat="server" Width="150px" ReadOnly="true" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    Nome:
                </td>
                <td>
                    <asp:TextBox ID="lblAlunoTSearch" runat="server" ReadOnly="true" Enabled="false"
                        Width="500px" />
                </td>
            </tr>
            <tr>
                <td>
                    Idade:
                </td>
                <td>
                    <asp:TextBox ID="lblIdade" runat="server" Width="150px" ReadOnly="true" Enabled="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="hdnIdade" runat="server" />
    <asp:HiddenField ID="hdnCurriculo" runat="server" />
    <asp:HiddenField ID="hdnNotificacaoId" runat="server" />
    <asp:HiddenField ID="hdnDataCadastro" runat="server" />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Alunos" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcNotificaControle" runat="server" ActiveTabIndex="0" Width="90%">
        <TabPages>
            <dxtc:TabPage Text="Dados Pessoais">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNome" runat="server" SkinID="lblObrigatorio" Text="Nome:* "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeCompl" runat="server" Width="600px" ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNomeSocial" runat="server" Text="Nome Social: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeSocial" runat="server" ReadOnly="true" Enabled="false" Width="600px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtNasc" runat="server" SkinID="lblObrigatorio" Text="Data Nascimento:* "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" Width="120px" Enabled="false" EnableDefaultAppearance="true"
                                                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblNomeMae" runat="server"
                                            Text="Nome da Mãe:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeMae" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" Width="140px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkFalecidaMae" Text="Falecida" Width="140px" ReadOnly="true"
                                            Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCPFMae" runat="server"
                                            Text="CPF:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPFMae" runat="server" Width="150px" ReadOnly="true" Enabled="false"
                                            onkeyup="formataCPF(this,event)" />
                                    </td>
                                    <td style="text-align: right; width: 50px">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTelefoneMae" runat="server"
                                            Text="Telefone:"></asp:Label>
                                    </td>
                                    <td style="width: 415px">
                                        <asp:TextBox ID="txtTelefoneMae" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                                            ReadOnly="true" Enabled="false" Width="100px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblNomePai" runat="server"
                                            Text="Nome do Pai:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomePai" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" Width="140px"
                                            ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkFalecidoPai" Text="Falecido" Width="140px" ReadOnly="true"
                                            Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPFPai" runat="server" Width="150px" ReadOnly="true" Enabled="false"
                                            onkeyup="formataCPF(this,event)" />
                                    </td>
                                    <td style="text-align: right; width: 50px">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTelefone" runat="server"
                                            Text="Telefone:"></asp:Label>
                                    </td>
                                    <td style="width: 415px">
                                        <asp:TextBox ID="txtTelefonePai" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                                            Width="100px" ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblResponsavel" runat="server"
                                            Text="Responsável Legal:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblResponsavel" runat="server" RepeatDirection="Horizontal"
                                            ReadOnly="true" Enabled="false">
                                            <asp:ListItem>Mãe</asp:ListItem>
                                            <asp:ListItem>Pai</asp:ListItem>
                                            <asp:ListItem>Próprio Aluno</asp:ListItem>
                                            <asp:ListItem>Outros</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Visible="false" Names="Verdana" Font-Size="Smaller" ID="lblNomeResponsavel"
                                            runat="server" Text="Nome do Responsável:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeResponsavel" runat="server" Width="250px" Visible="false"
                                            ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Visible="false" Font-Size="Smaller" ID="lblCPFResponsavel"
                                            runat="server" Text="CPF:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPFResponsavel" runat="server" Width="150px" ReadOnly="true"
                                            Enabled="false" onkeyup="formataCPF(this,event)" Visible="false" />
                                    </td>
                                    <td style="text-align: right; width: 50px">
                                        <asp:Label Font-Names="Verdana" Visible="false" Font-Size="Smaller" ID="lblTelefoneResponsavel"
                                            runat="server" Text="Telefone:"></asp:Label>
                                    </td>
                                    <td style="width: 415px">
                                        <asp:TextBox ID="txtTelefoneResp" Visible="false" onkeyup="formataTelefoneDDD(this,event)"
                                            ReadOnly="true" Enabled="false" runat="server" Width="100px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlEndereco" runat="server" GroupingText="Endereço" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                            Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" ReadOnly="true"
                                            Enabled="false" Width="400px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                            Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNum" runat="server" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
                                            Text="Compl.:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndCompl" runat="server" ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCep" runat="server" ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
                                            Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtBairro" runat="server" ReadOnly="true" Enabled="false" Width="400px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                            Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
                                        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="250px" onkeypress="return nomeSemNum(event);"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstado" runat="server" maxlength="2" class="txtInput" readonly="readonly" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Dados Escolares">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:Panel ID="pnlEscolaridade" runat="server" GroupingText="Escolaridade" Width="900px">
                            <table>
                                <tr>
                                    <td style="text-align: right; width: 15%">
                                        <asp:Label ID="lblRegional" runat="server" Text="Diretoria Regional:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseRegional" runat="server" SqlOrder="regional" SqlSelect=" select distinct  id_regional ,regional from tce_regional m "
                                            GridWidth="600px" ArgumentColumns="50" Columns="10" MaxLength="10" DataType="Number"
                                            Key="id_regional">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="60%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUnidade" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                                                MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                                                GridWidth="850px" SqlOrder="nome_comp">
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
                                    <td align="right">
                                        <asp:Label ID="lblNivelTSearch" runat="server" Text="Nível/Segmento:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNivel" runat="server" DataTextField="DESCRICAO" DataValueField="TIPO"
                                            ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlModalidade" runat="server" DataTextField="DESCRICAO" DataValueField="MODALIDADE"
                                            ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 120px">
                                        <asp:Label ID="lblCursoTSearch" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCurso" runat="server" DataTextField="NOME" DataValueField="CURSO"
                                            ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 120px">
                                        <asp:Label ID="lbltseTurno" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="DESCRICAO" DataValueField="TURNO"
                                            ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSeorie" runat="server"
                                            Text="Série/Ano Escolar:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="descricao" DataValueField="serie"
                                            ReadOnly="true" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblSituacao" runat="server"
                                            Text="Situação:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtSituacao" runat="server" ReadOnly="true" Enabled="false" Width="400px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="FICAI">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <asp:Panel ID="pnlDadosFrequencia" runat="server" GroupingText="Dados da Frequência"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblQtdFaltas" runat="server" Text="Quantidade de Faltas:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdFaltas" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtInicioFaltas" runat="server" SkinID="lblObrigatorio" Text="Data Início das Faltas:* "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInicioFaltas" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlDadosFICAI" runat="server" GroupingText="Dados FICAI" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNumFICAI" runat="server" Text="Número FICAI:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumFICAI" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtComunicacao" runat="server" SkinID="lblObrigatorio" Text="Data da Comunicação:* "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtComunicacao" runat="server" Width="120px" Enabled="true"
                                            EnableDefaultAppearance="true" ClientInstanceName="dtComunicacao" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblObsEstudante" runat="server" Text="Observações do Estudante:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObsEstudante" runat="server" Width="100%" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblFormaContato1" runat="server" Text="1ᵃ Forma de Contato:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFormaContato1" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="FORMACONTATOID">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtData1" runat="server" SkinID="lblObrigatorio" Text="1ᵃ Data:* "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtData1" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtData1" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblFormaContato2" runat="server" Text="2ᵃ Forma de Contato:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFormaContato2" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="FORMACONTATOID">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtData2" runat="server" Text="2ᵃ Data:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtData2" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtData2" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblFormaContato3" runat="server" Text="3ᵃ Forma de Contato:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFormaContato3" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="FORMACONTATOID">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtData3" runat="server" Text="3ᵃ Data:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtData3" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtData3" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlSituacaoFamiliar" runat="server" GroupingText="Situação Familiar/Necessidades Verificadas"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlSituacaoFamiliarFICAI" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="SITUACAOFAMILIARID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlAlegacaoFaltasFICAI" runat="server" GroupingText="Alegação para as faltas*"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtAlegacaofaltasFICAI" runat="server" Width="100%" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlEncaminhamentosUnidade" runat="server" GroupingText="Encaminhamentos feitos pela Unidade Escolar"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEncaminhaUE" runat="server" Text="Encaminhamento UE:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEncaminhaUE" runat="server" DataTextField="DESCRICAO" DataValueField="TIPOENCAMINHAMENTOID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtEncaminha" runat="server" Text="Data de Encaminhamento:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtEncaminha" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtEncaminha" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtRetorno" runat="server" Text="Data de retorno à Unidade Escolar:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtRetorno" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtRetorno" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEquipamento" runat="server" Text="Equipamento(s) Usado(s):"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEquipamento" runat="server" Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblProtocoloConTutelar" runat="server" Text="Protocolo Conselho Tutelar:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProtocoloConTutelar" runat="server" Width="250px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlMedidasTutelar" runat="server" GroupingText="Medidas adotadas pelo Conselho Tutelar"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMedidasTutelar" runat="server" Text="Medidas adotadas pelo Conselho Tutelar:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMedidasTutelar" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="MEDIDACONSELHOTUTELARID">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtEncaminhaTutelar" runat="server" Text="Data de Encaminhamento:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtEncaminhaTutelar" runat="server" Width="120px" Enabled="true"
                                            EnableDefaultAppearance="true" ClientInstanceName="dtEncaminhaTutelar" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblConselheiro" runat="server" Text="Nome do Conselheiro:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConselheiro" runat="server" Width="250px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlAtuacaoMP" runat="server" GroupingText="Atuação Ministério Público"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMedidasMinisterio" runat="server" Text="Medidas adotadas pelo Ministério Público:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMedidasMinisterio" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="MEDIDAMPRJID">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtEncaminhaMinisterio" runat="server" Text="Data de Encaminhamento:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtEncaminhaMinisterio" runat="server" Width="120px" Enabled="true"
                                            EnableDefaultAppearance="true" ClientInstanceName="dtEncaminhaMinisterio" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblPromotor" runat="server" Text="Promotor de Justiça:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPromotor" runat="server" Width="250px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSalvarFICAI" runat="server" Text="Salvar" OnClick="btnSalvarFICAI_Click" />
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="FAMI">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl4" runat="server">
                        <asp:Panel ID="pnlDadosFAMI" runat="server" GroupingText="Dados FAMI" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNumFAMI" runat="server" Text="Número FAMI:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumFAMI" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtComunicacaoFAMI" runat="server" SkinID="lblObrigatorio" Text="Data de Comunicação:* "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtComunicacaoFAMI" runat="server" Width="120px" Enabled="true"
                                            EnableDefaultAppearance="true" ClientInstanceName="dtComunicacaoFAMI" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlDadosFreqFAMI" runat="server" GroupingText="Dados Frequência" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNumFaltas" runat="server" Text="Número de Faltas:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumFaltasFAMI" runat="server" Width="250px" ReadOnly="true" Enabled="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtInicioFaltasFAMI" runat="server" SkinID="lblObrigatorio" Text="Data Início de Faltas:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInicioFaltasFAMI" runat="server" Width="250px" ReadOnly="true"
                                            Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlAlegacaoFaltasFAMI" runat="server" GroupingText="Alegação para as Faltas"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtAlegacaoFaltasFAMI" runat="server" Width="100%" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlEncaminhamentosFAMI" runat="server" GroupingText="Encaminhamentos Realizados"
                            Width="900px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEncaminhamentosFAMI" runat="server" Width="100%" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSalvarFAMI" runat="server" Text="Salvar" OnClick="btnSalvarFAMI_Click" />
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Ofício Conselho Tutelar">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl5" runat="server">
                        <asp:Panel ID="pnlOficioConselho" runat="server" Width="70%" GroupingText="">
                            <asp:HiddenField ID="hdnOficioConselhoId" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNumeroFICAIConselho" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="Ofício Ao Conselho Tutelar"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="Ao Conselho Tutelar:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAoConselho" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                            Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCEPOficioConselho" runat="server" MaxLength="8" SkinID="numerico">
                                        </asp:TextBox>
                                        <tweb:TSearch ID="tseCEPOficioConselho" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEP"
                                            Modal="true" SkinID="CEP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label3" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMunicipioOficioConselho" runat="server" MaxLength="20" Width="380px"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnMunicipioOficioConselho" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblUF" runat="server" Text="UF: "></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstadoOficioConselho" runat="server" maxlength="20" class="txtInput"
                                            readonly="readonly" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label5" runat="server" Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnderecoOficioConselho" runat="server" MaxLength="50" Width="380px"
                                            onkeypress="return endereco(event);" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label7" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNumOficioConselho" runat="server" MaxLength="15" SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label9" runat="server" Text="Bairro:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBairroOficioConselho" runat="server" Width="380px" MaxLength="50" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table>
                                <tr align="center">
                                    <td align="center">
                                        <asp:Button ID="btnSalvarConselho" runat="server" Text="Salvar" OnClick="btnSalvarConselho_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <div id="divPrincipalConselho" runat="server" visible="false" style="font-family: Tahoma, Geneva, sans-serif;
                                font-size: 13px; color: Black;">
                                <table align="center">
                                    <tr align="center">
                                        <td align="center">
                                            <img src="~/Images/logo_govrj.jpg" id="Image1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                            border-color: #000000; font-weight: bold;">
                                            GOVERNO DO ESTADO DO RIO DE JANEIRO
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <p>
                                    <u>
                                        <asp:Label ID="lblNumOficioConselho" runat="server" class="geral"></asp:Label></u></p>
                                <p>
                                    <strong><u>Ao Conselho Tutelar</u></strong></p>
                                <p>
                                    <u>
                                        <asp:Label ID="lblNomeConselho" runat="server" class="geral"></asp:Label></u></p>
                                <p>
                                    <u>
                                        <asp:Label ID="lblEnderecoConselho" runat="server" class="geral"></asp:Label></u></p>
                                <p>
                                    <strong>Assunto: Comunicação de Infrequência Escolar –
                                        <asp:Label ID="lblNomeAlunoConselhoTitulo" runat="server" class="geral"></asp:Label></strong></p>
                                <p>
                                    Senhor(a) Conselheiro(a),</p>
                                <p>
                                    Cumprimentando-os cordialmente, a <u>
                                        <asp:Label ID="lblNomeEscolaConselho" runat="server" class="geral"></asp:Label></u>
                                    , unidade da rede estadual de ensino, vem informar a situação de infrequência escolar
                                    do(a) estudante
                                    <asp:Label ID="lblNomeAlunoConselho" runat="server" class="geral"></asp:Label>,
                                    matrícula nº
                                    <asp:Label ID="lblMatriculaAlunoConselho" runat="server" class="geral"></asp:Label>,
                                    regularmente matriculado(a) no
                                    <asp:Label ID="lblSerieAlunoConselho" runat="server" class="geral"></asp:Label>,
                                    conforme registros no sistema Conexão Educação.</p>
                                <p>
                                    De acordo com o acompanhamento realizado, o(a) estudante acumula <strong><u>
                                        <asp:Label ID="lblQtdeDiasConselho" runat="server" class="geral"></asp:Label></u>
                                        dias consecutivos de ausência não justificada</strong>, situação que configura
                                    infrequência grave e potencial risco de evasão escolar.</p>
                                <p>
                                    Em conformidade com os procedimentos estabelecidos na <strong>Minuta de Resolução sobre
                                        Frequência Escolar</strong> e na Lei nº 10.376/2024, as seguintes ações foram
                                    adotadas pela unidade escolar:</p>
                                <br />
                                <p>
                                    1. <strong>Contato com a família/responsáveis:</strong> Foram realizadas tentativas
                                    de contato por <strong><u>
                                        <asp:Label ID="lblTipoContatoConselho" runat="server" class="geral"></asp:Label></u></strong>
                                    nas seguintes datas: <u>
                                        <asp:Label ID="lblDatasContatoFICAIConselho" runat="server" class="geral"></asp:Label></u>,
                                    sem sucesso ou sem justificativa plausível para as ausências.</p>
                                <br />
                                <p>
                                    2.<strong> Acionamento da rede de apoio:</strong> O caso foi encaminhado ao <u>Conselho
                                        Tutelar</u>, conforme registrado no sistema.</p>
                                <br />
                                <p>
                                    3. <strong>Registro e envio da FICAI:</strong> Em <strong><u>
                                        <asp:Label ID="lblDataRegistroFICAI" runat="server" class="geral"></asp:Label></u></strong>,
                                    foi registrada e enviada a <strong><u>Ficha de Comunicação do Aluno Infrequente (FICAI)</u></strong>
                                    a este Conselho Tutelar, conforme <strong><u>Protocolo Nº
                                        <asp:Label ID="lblProtocoloFICAI" runat="server" class="geral"></asp:Label></u></strong>.</p>
                                <br />
                                <p>
                                    4. <strong>Registro no Sistema Conexão Educação:</strong> Em <u>
                                        <asp:Label ID="lblDataRegistroConexaoConselho" runat="server" class="geral"></asp:Label></u>
                                    , a matrícula do(a) estudante foi alterada para<strong> "MATRÍCULA EM SUSPENSÃO"</strong>,
                                    conforme previsão normativa, garantindo a manutenção da vaga e assegurando seu direito
                                    ao retorno.</p>
                                <br />
                                <p>
                                    Diante da persistência da infrequência, <strong>solicitamos a intervenção deste Conselho
                                        Tutelar</strong>, visando a adoção das providências cabíveis para garantir o
                                    direito à educação do(a) estudante, conforme preconizado<strong> no Estatuto da Criança
                                        e do Adolescente (Lei nº 8.069/1990, arts. 4º, 53 e 129)</strong>.</p>
                                <br />
                                <br />
                                <br />
                                <br />
                                <table align="center" width="100%">
                                    <tr align="left">
                                        <td align="left">
                                            <asp:Label ID="lblIDUsuarioConselho" runat="server" class="geral"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblNomeUsuarioConselho" runat="server" class="geral"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <br />
                            <asp:Panel runat="server" ID="pnlImprimirConselho" Visible="false" Width="500px">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <input type="button" id="btnImprimirConselho" style="background-image: url(../Images/bot_imprimir.png);
                                                width: 100px; height: 27px; background-color: transparent!important;" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnExportarPDFConselho" runat="server" ImageAlign="Right" ToolTip="Export"
                                                OnClick="btnExportarPDFConselho_Click" ImageUrl="~/Images/bot_PDF.png" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Ofício ao MPRJ">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl6" runat="server">
                        <asp:Panel ID="pnlOficioMPRJ" runat="server" Width="60%" GroupingText="">
                            <asp:HiddenField ID="hdnOficioMPRJId" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNumeroFICAIMP" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label2" runat="server" Text="Ofício Ao Ministério Público do Rio de Janeiro"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label6" runat="server" Text="Promotoria da Infância e Juventude:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPromotoria" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEPOficioMPRJ" runat="server"
                                            Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCEPOficioMPRJ" runat="server" SkinID="numerico" MaxLength="8"
                                            AutoPostBack="false" />
                                        <tweb:TSearch ID="tseCEPOficioMPRJ" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEP"
                                            Modal="true" SkinID="CEP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipioOficioMPRJ" runat="server"
                                            Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hdnMunicipioOficioMPRJ" />
                                        <asp:TextBox ID="txtMunicipioOficioMPRJ" runat="server" MaxLength="20" Width="250px"
                                            onkeypress="return nomeSemNum(event);"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstadoOficioMPRJ" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstadoOficioMPRJ" runat="server" maxlength="2" class="txtInput" readonly="readonly" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnderecoOficioMPRJ" runat="server"
                                            Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnderecoOficioMPRJ" runat="server" MaxLength="50" Columns="50"
                                            onkeypress="return endereco(event);" Width="250px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndNumOficioMPRJ" runat="server"
                                            Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNumOficioMPRJ" runat="server" MaxLength="15" Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairroOficioMPRJ" runat="server"
                                            Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBairroOficioMPRJ" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                            Width="250px" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSalvarMP" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                            OnClick="btnSalvarMP_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <div id="divPrincipalMP" runat="server" visible="false" style="font-family: Tahoma, Geneva, sans-serif;
                                font-size: 13px; color: Black;">
                                <table align="center">
                                    <tr align="center">
                                        <td align="center">
                                            <img src="~/Images/logo_govrj.jpg" id="Img1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                            border-color: #000000; font-weight: bold;">
                                            GOVERNO DO ESTADO DO RIO DE JANEIRO
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <p>
                                    <strong><u>Ao Ministério Público do Estado do Rio de Janeiro</u></strong></p>
                                <p>
                                    <u>
                                        <asp:Label ID="lblNomePromotoriaMP" runat="server"></asp:Label></u></p>
                                <p>
                                    <u>
                                        <asp:Label ID="lblEnderecoMP" runat="server"></asp:Label></u></p>
                                <p>
                                    <strong>Assunto: Comunicação de Infrequência Escolar –
                                        <asp:Label ID="lblNomeAlunoMPTitulo" runat="server"></asp:Label></strong></p>
                                <br />
                                <p>
                                    Senhor(a) Promotor(a),</p>
                                <br />
                                <p>
                                    Cumprimentando-os cordialmente, a <u>
                                        <asp:Label ID="lblNomeEscolaMP" runat="server"></asp:Label></u> , unidade da
                                    rede estadual de ensino, vem informar a situação de infrequência escolar do(a) estudante
                                    <asp:Label ID="lblNomeAlunoMP" runat="server"></asp:Label>, matrícula nº
                                    <asp:Label ID="lblMatriculaAlunoMP" runat="server"></asp:Label>, regularmente matriculado(a)
                                    no
                                    <asp:Label ID="lblSerieAlunoMP" runat="server"></asp:Label>, conforme registros
                                    no sistema Conexão Educação.</p>
                                <p>
                                    De acordo com o acompanhamento realizado, o(a) estudante acumula <strong><u>
                                        <asp:Label ID="lblQtdeDiasMP" runat="server"></asp:Label></u> dias consecutivos
                                        de ausência não justificada</strong>, situação que configura infrequência grave
                                    e potencial risco de evasão escolar.</p>
                                <p>
                                    Em conformidade com os procedimentos estabelecidos na <strong>Minuta de Resolução sobre
                                        Frequência Escolar</strong> e na Lei nº 10.376/2024, as seguintes ações foram
                                    adotadas pela unidade escolar:</p>
                                <p>
                                    1. <strong>Contato com a família/responsáveis:</strong> Foram realizadas tentativas
                                    de contato por <strong><u>
                                        <asp:Label ID="lblTipoContatoMP" runat="server"></asp:Label></u></strong> nas
                                    seguintes datas: <u>
                                        <asp:Label ID="lblDatasContatoFICAIMP" runat="server"></asp:Label></u>, sem
                                    sucesso ou sem justificativa plausível para as ausências.</p>
                                <p>
                                    2.<strong> Acionamento da rede de apoio:</strong> O caso foi encaminhado ao <u>Conselho
                                        Tutelar</u>, conforme registrado no sistema.</p>
                                <p>
                                    3. <strong>Encaminhamento ao Conselho Tutelar:</strong> Diante da continuidade da
                                    infrequência, foi formalizado ofício ao <strong><u>
                                        <asp:Label ID="lblNomeConselhoTutelar" runat="server"></asp:Label></u></strong>,
                                    em
                                    <asp:Label ID="lblDataEncaminhamentoConselho" runat="server"></asp:Label>, que adotou
                                    como medida o <u>encaminhamento de ofício ao Ministério Público</u>.</p>
                                <p>
                                    4. <strong>Registro no Sistema Conexão Educação:</strong> Em <u>
                                        <asp:Label ID="lblDataRegistroConexaoMP" runat="server"></asp:Label></u> , a
                                    matrícula do(a) estudante foi alterada para<strong> "MATRÍCULA EM SUSPENSÃO"</strong>,
                                    conforme previsão normativa, garantindo a manutenção da vaga e assegurando seu direito
                                    ao retorno.</p>
                                <p>
                                    Diante da ausência de retorno à escola e da persistência da infrequência, <strong>solicitamos
                                        a intervenção deste Ministério Público</strong>, visando a adoção das providências
                                    cabíveis para garantir o direito à educação do(a) estudante, conforme preconizado<strong>
                                        no Estatuto da Criança e do Adolescente (Lei nº 8.069/1990, arts. 4º, 53 e 129)</strong>.</p>
                                <br />
                                <br />
                                <br />
                                <br />
                                <table align="center">
                                    <tr align="left">
                                        <td align="left">
                                            <asp:Label ID="lblIdUsuarioMP" runat="server"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblNomeUsuarioMP" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <br />
                            <asp:Panel runat="server" ID="pnlImprimirMP" Visible="false" Width="500px">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <input type="button" id="btnImprimirMP" style="background-image: url(../Images/bot_imprimir.png);
                                                width: 100px; height: 27px; background-color: transparent!important;" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnExportarPDFMP" runat="server" ImageAlign="Right" ToolTip="Export"
                                                OnClick="btnExportarPDFMP_Click" ImageUrl="~/Images/bot_PDF.png" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <asp:ImageButton ID="btnVoltar" runat="server" SkinID="Voltar" OnClick="btnCancelarVoltar_Click" />
</asp:Content>
