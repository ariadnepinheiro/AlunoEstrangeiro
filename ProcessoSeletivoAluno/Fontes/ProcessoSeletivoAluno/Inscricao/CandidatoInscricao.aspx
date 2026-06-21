<%@ Page Language="C#" MasterPageFile="~/Modulos/PublicMaster.Master" AutoEventWireup="true"
    CodeBehind="CandidatoInscricao.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivoAluno.CandidatoInscricao" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCep.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });

        function executaPesquisaCEP() {
            document.getElementById('<%=tsCEP.ClientID%>_ctl00').click();
        }

        $(document).ready(function() {
            $("#<%= this.txtEmailConfirmacao.ClientID %>").bind("cut copy paste", function(e) {
                e.preventDefault();
            });
        });

        $(document).ready(function() {
            $("#<%= this.txtEmail.ClientID %>").bind("cut copy paste", function(e) {
                e.preventDefault();
            });
        });

        function RetiraParenteses(str) {
            str.value = str.value.replace("()", "");
            return (str);
        }

        function BuscaDadosMatricula(str) {
            document.getElementById('<%=btBuscar.ClientID%>').click();
        }

        function TextToUpper(str) {
            str.value = trim(str.value.toUpperCase().trim());
            return (str);
        }

        function TextToLower(str) {
            str.value = trim(str.value.toLowerCase());
            return (str);
        }

        function trim(str) {
            return str.replace(/^\s+|\s+$/g, "");
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if ((charCode > 31 && (charCode < 48 || charCode > 57)))
                return false;

            return true;
        }

        function nomeSemNum(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)
                || (charCode >= 192 && charCode <= 255) || (charCode == 32) || (charCode == 39) || (charCode == 8)) {
                return true;
            }
            else {
                return false;
            }
        }

        function retiraSelecaoCheckRecursoAplicacaoProva(strCheckNenhum, strRecursoAplicacaoProva, strRecursoAplicaProvaExclusivo) {
            var checkNenhum = document.getElementById(strCheckNenhum);

            if (checkNenhum.checked) {
                var chkRecursoAplicacaoProva = document.getElementById(strRecursoAplicacaoProva).getElementsByTagName("input");
                for (i = 0; i < chkRecursoAplicacaoProva.length; i++) {
                    if (chkRecursoAplicacaoProva[i].type == "checkbox") {
                        chkRecursoAplicacaoProva[i].checked = false;
                    }
                }

                var rblRecursoAplicaProvaExclusivo = document.getElementById(strRecursoAplicaProvaExclusivo).getElementsByTagName("input");
                for (i = 0; i < rblRecursoAplicaProvaExclusivo.length; i++) {
                    if (rblRecursoAplicaProvaExclusivo[i].type == "radio") {
                        rblRecursoAplicaProvaExclusivo[i].checked = false;
                    }
                }
            }
            else {
                retiraSelecaoCheckNenhumRecursoAplicacaoProva(strCheckNenhum, strRecursoAplicacaoProva, strRecursoAplicaProvaExclusivo);
            }
        }

        function retiraSelecaoCheckNenhumRecursoAplicacaoProva(strCheckNenhum, strRecursoAplicacaoProva, strRecursoAplicaProvaExclusivo) {
            var habilitaNenhum = true;

            var chkRecursoAplicacaoProva = document.getElementById(strRecursoAplicacaoProva).getElementsByTagName("input");
            for (i = 0; i < chkRecursoAplicacaoProva.length; i++) {
                if (chkRecursoAplicacaoProva[i].type == "checkbox") {
                    if (chkRecursoAplicacaoProva[i].checked) {
                        habilitaNenhum = false;
                        break;
                    }
                }
            }

            var rblRecursoAplicaProvaExclusivo = document.getElementById(strRecursoAplicaProvaExclusivo).getElementsByTagName("input");
            for (i = 0; i < rblRecursoAplicaProvaExclusivo.length; i++) {
                if (rblRecursoAplicaProvaExclusivo[i].type == "radio") {
                    if (rblRecursoAplicaProvaExclusivo[i].checked) {
                        habilitaNenhum = false;
                        break;
                    }
                }
            }

            document.getElementById(strCheckNenhum).checked = habilitaNenhum;
        }

        function checkResponsavel(sender, args) {
            var chkResponsavel = document.getElementById('<%=chkResponsavel.ClientID %>').getElementsByTagName("input");
            for (i = 0; i < chkResponsavel.length; i++) {
                if (chkResponsavel[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }

            args.IsValid = false;
        }
 
    </script>

    <div class="divEditBlock" style="width: 1210px;">
        <asp:Label runat="server" ID="ProcessoSeletivo" Text="Processo Seletivo" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlRedeEnsino" GroupingText="Rede de Ensino" runat="server" Width="1350px">
        <asp:UpdatePanel ID="UpdateRedeEnsino" runat="server">
            <ContentTemplate>
                <table cellpadding="5px" border="0" cellspacing="0">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblTipoRedeEnsino" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <table cellpadding="0" border="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblTipoRedeEnsino" runat="server" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rblTipoRedeEnsino_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="Federal">Federal</asp:ListItem>
                                            <asp:ListItem Value="Particular">Particular</asp:ListItem>
                                            <asp:ListItem Value="Estadual">Estadual</asp:ListItem>
                                            <asp:ListItem Value="Municipal">Municipal</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvTipoRedeEnsino" runat="server" ControlToValidate="rblTipoRedeEnsino"
                                            Display="Dynamic" ErrorMessage="Rede de Ensino de Origem: Preenchimento obrigatório."
                                            InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trBolsaParticular" runat="server">
                        <td align="right" style="width: 180px">
                            <asp:Label Style="font-size: 12px" ID="lbBolsaParticular" runat="server" Text="Tipo Bolsa Particular:"></asp:Label>
                        </td>
                        <td>
                            <!-- Opção "Bolsista Parcial" removida em agosto/2014 para Processo Seletivo 2015
                                     por solicitação da SEEDUC através da demanda 5150. No banco de dados, o valor
                                     original (1) será mantido e reservado, caso seja solicitado seu retorno no
                                     futuro. Assim, por hora o campo poderá receber apenas os valores 0 e 2.
                                     Caso algum candidato com valor 1 ("Bolsista Parcial") seja carregado, o valor
                                     da combobox será "reiniciado" (método CarregaDadosCandidatoInscrito). -->
                            <!-- <asp:ListItem Value="1">Bolsista Parcial</asp:ListItem> -->
                            <asp:DropDownList Style="padding: 3px" ID="ddlTipoBolsaParticular" runat="server">
                                <asp:ListItem Value="0">Não Bolsista</asp:ListItem>
                                <asp:ListItem Value="2">Bolsista Integral(100% de Bolsa)</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trRedeEstadualSeeduc" runat="server">
                        <td colspan="2">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbEstadual" runat="server" Text="Caso seja aluno da Rede Estadual - SEEDUC, informe aqui seu número de matrícula:"></asp:Label>
                                    </td>
                                    <td style="padding-left: 4px;">
                                        <asp:TextBox Height="18px" Style="text-transform: uppercase;" ID="txtMatriculaSeeduc"
                                            runat="server" SkinID="numerico" MaxLength="20" Width="200px" onblur="BuscaDadosMatricula(this)"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div style="display: none;">
        <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" UseSubmitBehavior="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlDadosPessoais" GroupingText="Dados Pessoais" runat="server" Width="1350px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="5px" border="0" cellspacing="0">
                    <tr>
                        <td align="right" style="width: 240px;">
                            <asp:Label ID="lblNomeCompletoAluno" runat="server" SkinID="lblObrigatorio" Text="Nome Completo*:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox Height="18px" ID="txtNomeCompletoAluno" runat="server" Width="400px"
                                MaxLength="100" onkeypress="return nomeSemNum(event);" Style="text-transform: uppercase;"
                                onblur="TextToUpper(this)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNomeComl" runat="server" ControlToValidate="txtNomeCompletoAluno"
                                Display="Dynamic" ErrorMessage="Nome: Preenchimento obrigatório." InitialValue=""
                                ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                            <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Font-Size="X-Small" ForeColor="red"
                                Text="(Preencher sem abreviações)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label5" runat="server" Text="Data Nascimento*:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <table cellpadding="0" border="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" ClientInstanceName="dtNasc" MinDate="1901-01-01"
                                            Width="120px" Paddings-Padding="1px">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ErrorMessage="Data Nascimento: Preenchimento obrigatório."
                                            ID="rfvDtNasc" runat="server" ControlToValidate="dtDataNasc" InitialValue=""
                                            ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                    <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" Text="Sexo*:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <table cellpadding="0" border="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="M">Masculino</asp:ListItem>
                                            <asp:ListItem Value="F">Feminino</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ErrorMessage="Sexo: Preenchimento obrigatório." ID="rfvSexo"
                                            runat="server" ControlToValidate="rblSexo" InitialValue="" ValidationGroup="SalvarForm"
                                            ToolTip="Campo Obrigatório!">
                                    <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlCertidaoNascimento" GroupingText="Certidão de Nascimento" runat="server"
        Width="1350px">
        <asp:UpdatePanel ID="UpdateCertidaoNascimento" runat="server">
            <ContentTemplate>
                <table cellpadding="5px" border="0" cellspacing="0">
                    <tr>
                        <td align="right" style="width: 240px;">
                            <asp:Label ID="Label2" runat="server" Text="Certidão de Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:DropDownList Style="padding: 3px" ID="ddlModeloCertidaoNascimento" runat="server"
                                OnSelectedIndexChanged="ddlModeloCertidaoNascimento_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="">&lt;Não informado&gt;</asp:ListItem>
                                <asp:ListItem Value="Modelo Novo">Modelo Novo</asp:ListItem>
                                <asp:ListItem Value="Modelo Antigo">Modelo Antigo</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvModeloCertidaoNascimento" runat="server" ControlToValidate="ddlModeloCertidaoNascimento"
                                Display="Dynamic" ErrorMessage="Certidão de Nascimento: Preenchimento obrigatório."
                                InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trMatriculaCertidao" runat="server">
                        <td align="right">
                            <asp:Label ID="lblMatriculaCertidao" runat="server" Text="Número de Matrícula da Certidão:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox Height="18px" ID="txtMatriculaCertidao" runat="server" SkinID="numerico"
                                Width="300px" MaxLength="32"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Número da Matrícula da Certidão: Preenchimento obrigatório."
                                ID="rfvMatriculaCertidao" runat="server" ControlToValidate="txtMatriculaCertidao"
                                InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                    <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trCertidaoAntigaFiltro" runat="server">
                        <td align="right">
                            <asp:Label ID="lbUFCartorio" runat="server" Text="UF do Cartório:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList Style="padding: 3px" ID="ddlUFCartorio" runat="server" AutoPostBack="true"
                                DataTextField="sigla" DataValueField="sigla" OnSelectedIndexChanged="ddlUFCartorio_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="UF do Cartório: Preenchimento obrigatório."
                                ID="rfvUfCartorio" runat="server" ControlToValidate="ddlUFCartorio" InitialValue=""
                                ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbMunicipioCartorio" runat="server" Text="Município do Cartório:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left" colspan="2">
                            <asp:DropDownList Style="padding: 3px" ID="ddlMunicipioCartorio" runat="server" AutoPostBack="true"
                                DataTextField="municipio" DataValueField="codigo_municipio" OnSelectedIndexChanged="ddlMunicipioCartorio_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="Município do Cartório: Preenchimento obrigatório."
                                ID="rfvMunicipioCartorio" runat="server" ControlToValidate="ddlMunicipioCartorio"
                                InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trCertidaoAntigaCartorio" runat="server">
                        <td align="right">
                            <asp:Label ID="lbCartorio" runat="server" Text="Cartório:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:DropDownList Style="padding: 3px" ID="ddlCartorio" runat="server" DataTextField="nome_cartorio"
                                DataValueField="cod_cartorio" OnSelectedIndexChanged="ddlCartorio_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ErrorMessage="Cartório: Preenchimento obrigatório." ID="rfvCartorio"
                                runat="server" ControlToValidate="ddlCartorio" InitialValue="" ValidationGroup="SalvarForm"
                                ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trCertidaoAntigaDadosExpedicao" runat="server">
                        <td align="right">
                            <asp:Label ID="lbNumeroTermo" runat="server" Text="Número do Termo:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                                ID="txtNumeroTermo" runat="server" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Número do Termo da Certidão: Preenchimento obrigatório."
                                ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNumeroTermo"
                                InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbDataExped" runat="server" Text="Data de Emissão:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataExped" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" Width="140px" Paddings-Padding="1px">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ErrorMessage="Data de Emissão da Certidão: Preenchimento obrigatório."
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="dtDataExped" InitialValue=""
                                            ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr id="trCertidaoAntigaDados" runat="server">
                        <td align="right">
                            <asp:Label ID="lbFolha" runat="server" Text="Folha:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                                ID="txtfolha" runat="server" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Folha do Livro da Certidão: Preenchimento obrigatório."
                                ID="rfvFolha" runat="server" ControlToValidate="txtfolha" InitialValue="" ValidationGroup="SalvarForm"
                                ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbLivro" runat="server" Text="Livro:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left" colspan="2">
                            <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                                ID="txtlivro" runat="server" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ErrorMessage="Livro da Certidão: Preenchimento obrigatório."
                                ID="rfvLivro" runat="server" ControlToValidate="txtlivro" InitialValue="" ValidationGroup="SalvarForm"
                                ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlFiliacao" GroupingText="Filiação" runat="server" Width="1350px">
        <asp:UpdatePanel ID="UpdateFiliacao" runat="server">
            <ContentTemplate>
                <table cellpadding="5px" border="0" cellspacing="0">
                    <tr>
                        <td align="right" style="width: 240px;">
                            <asp:Label ID="Label16" runat="server" Text="Nome da Mãe:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" Style="text-transform: uppercase;" ID="txtNomeMae" runat="server"
                                Width="420px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);"
                                onblur="TextToUpper(this);" />
                            <asp:RequiredFieldValidator ID="rfvNomeMae" runat="server" ControlToValidate="txtNomeMae"
                                Display="Dynamic" ErrorMessage="Nome da Mãe: Preenchimento obrigatório." InitialValue=""
                                ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" AutoPostBack="true"
                                OnCheckedChanged="chkNaoDeclarMae_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkFalecidaMae" Text="Falecida" AutoPostBack="true"
                                OnCheckedChanged="chkFalecidaMae_CheckedChanged" />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblCPFMae" runat="server" Text="CPF:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" ID="txtCPFMae" runat="server" Width="110px" MaxLength="20"
                                SkinID="numerico" onkeyup="formataCPF(this,event)" />
                        </td>
                        <td style="width: 20px; padding: 0px">
                            <table cellpadding="0" cellspacing="0" border="0" id="tableCPFMae" runat="server"
                                visible="false">
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvCPFMae" runat="server" ControlToValidate="txtCPFMae"
                                            Display="Dynamic" ErrorMessage="CPF da Mãe: Preenchimento obrigatório." InitialValue=""
                                            ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right; width: 50px">
                            <asp:Label ID="lblTelefoneMae" runat="server" Text="Telefone:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" ID="txtTelefoneMae" onkeyup="formataTelefoneDDD(this,event)"
                                runat="server" Width="100px" onblur="RetiraParenteses(this)" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTelefoneMae"
                                ErrorMessage="Telefone inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                ValidationExpression="\([1][1-9]\)[1-5]\d{3}[-]\d{4}\d*|\([2-9][0-9]\)[1-5]\d{3}[-]\d{4}\d*"
                                ToolTip="Telefone Inválido!">
                                <img src="../Images/AlertaMens.gif" alt="Telefone inválido" />
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label18" runat="server" Text="Nome do Pai:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" Style="text-transform: uppercase;" ID="txtNomePai" runat="server"
                                Width="420px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);"
                                onblur="TextToUpper(this);" />
                            <asp:RequiredFieldValidator ID="rfvNomePai" runat="server" ControlToValidate="txtNomePai"
                                Display="Dynamic" ErrorMessage="Nome do Pai: Preenchimento obrigatório." InitialValue=""
                                ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" AutoPostBack="true"
                                OnCheckedChanged="chkNaoDeclarPai_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkFalecidoPai" Text="Falecido" AutoPostBack="true"
                                OnCheckedChanged="chkFalecidoPai_CheckedChanged" />
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblCPFPai" runat="server" Text="CPF:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" ID="txtCPFPai" runat="server" Width="110px" MaxLength="20"
                                SkinID="numerico" onkeyup="formataCPF(this,event)" />
                        </td>
                        <td style="width: 20px; padding: 0px">
                            <table cellpadding="0" cellspacing="0" border="0" id="tableCPFPai" runat="server"
                                visible="false">
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvCPFPai" runat="server" ControlToValidate="txtCPFPai"
                                            Display="Dynamic" ErrorMessage="CPF do Pai: Preenchimento obrigatório." InitialValue=""
                                            ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right; width: 50px">
                            <asp:Label ID="Label20" runat="server" Text="Telefone:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" ID="txtTelefonePai" onkeyup="formataTelefoneDDD(this,event)"
                                runat="server" Width="100px" onblur="RetiraParenteses(this)" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtTelefonePai"
                                ErrorMessage="Telefone inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                ValidationExpression="\([1][1-9]\)[1-5]\d{3}[-]\d{4}\d*|\([2-9][0-9]\)[1-5]\d{3}[-]\d{4}\d*"
                                ToolTip="Telefone Inválido!">
                                <img src="../Images/AlertaMens.gif" alt="Telefone inválido" />
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="Label21" runat="server" Text="Responsável Legal:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="width: 165px">
                                                    <asp:CheckBoxList ID="chkResponsavel" runat="server" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="chkResponsavel_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="Mãe">Mãe</asp:ListItem>
                                                        <asp:ListItem Value="Pai">Pai</asp:ListItem>
                                                        <asp:ListItem Value="Outros">Outros</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>
                                                    <asp:CustomValidator Display="Dynamic" ID="cvCheckBoxListResponsavel" runat="server"
                                                        ClientValidationFunction="checkResponsavel" ErrorMessage="Responsável Legal: Campo Obrigatório!"
                                                        ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <table border="0" cellpadding="0" cellspacing="0" runat="server" id="tbNomeResponsavel"
                                            visible="false">
                                            <tr>
                                                <td style="text-align: right; padding-left: 5px; padding-right: 5px">
                                                    <asp:Label Names="Verdana" ID="lblNomeResponsavel" runat="server" Text="Nome do Responsável:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                                                        ID="txtNomeResponsavel" runat="server" Width="280px" MaxLength="100" />
                                                    <asp:RequiredFieldValidator ID="rfvNomeResponsavel" runat="server" ControlToValidate="txtNomeResponsavel"
                                                        Display="Dynamic" ErrorMessage="Nome do Responsável: Preenchimento obrigatório."
                                                        InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lblCPFResponsavel" Visible="false" runat="server" Text="CPF:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" ID="txtCPFResponsavel" runat="server" Width="110px" SkinID="numerico"
                                onkeyup="formataCPF(this,event)" Visible="false" />
                        </td>
                        <td style="width: 20px; padding: 0px">
                            <table border="0" cellpadding="0" cellspacing="0" runat="server" id="tbValidacaoCPF"
                                visible="false">
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvCPFResponsavel" runat="server" ControlToValidate="txtCPFResponsavel"
                                            Display="Dynamic" ErrorMessage="CPF do Responsável: Preenchimento obrigatório."
                                            InitialValue="" ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: right; width: 50px">
                            <asp:Label ID="lblTelefoneResponsavel" Visible="false" runat="server" Text="Telefone:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox Height="18px" ID="txtTelefoneResp" onkeyup="formataTelefoneDDD(this,event)"
                                runat="server" Width="100px" Visible="false" onblur="RetiraParenteses(this)" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtTelefoneResp"
                                ErrorMessage="Telefone inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                ValidationExpression="\([1][1-9]\)[1-5]\d{3}[-]\d{4}\d*|\([2-9][0-9]\)[1-5]\d{3}[-]\d{4}\d*"
                                ToolTip="Telefone Inválido!">
                                <img src="../Images/AlertaMens.gif" alt="Telefone inválido" />
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnEndereco" GroupingText="Endereço" runat="server" Width="1350px">
        <table cellpadding="5px" border="0" cellspacing="0">
            <tr>
                <td style="text-align: right; width: 240px;">
                    <asp:Label ID="lblCEP" runat="server" Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox Height="18px" ID="txtCep" runat="server" SkinID="numerico" MaxLength="8"
                        AutoPostBack="false" onblur="executaPesquisaCEP()" />
                    <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                        Modal="true" SkinID="CEP" />
                    <asp:RequiredFieldValidator ErrorMessage="CEP do Endereço: Preenchimento obrigatório."
                        ID="rfvCEP" runat="server" ControlToValidate="txtCEP" InitialValue="" ValidationGroup="SalvarForm"
                        ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revCEP" ControlToValidate="txtCEP" ValidationExpression="^.{8}$"
                        runat="server" ErrorMessage="CEP do Endereço: Preenchimento de oito números obrigatório."
                        ToolTip="Preenchimento de oito número obrigatório!" ValidationGroup="SalvarForm">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
                    <input id="txtMunicipio" runat="server" height="18px" maxlength="20" class="txtInput"
                        width="250px" readonly="readonly" />
                    <asp:RequiredFieldValidator ErrorMessage="Município do Endereço: Preenchimento obrigatório."
                        ID="rfvMunicipio" runat="server" ControlToValidate="txtMunicipio" InitialValue=""
                        ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                    </asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                    <asp:RequiredFieldValidator ErrorMessage="Estado do Endereço: Preenchimento obrigatório."
                        ID="rfvEstado" runat="server" ControlToValidate="txtEstado" InitialValue="" ValidationGroup="SalvarForm"
                        ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEndereco" runat="server" Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                        ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                        Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEndereco" runat="server" ControlToValidate="txtEndereco"
                        InitialValue="" ErrorMessage="Endereço: Preenchimento obrigatório." ValidationGroup="SalvarForm"
                        ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEnd_Num" runat="server" Text="N.º:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                        ID="txtEndNum" runat="server" MaxLength="15" />
                    <asp:RequiredFieldValidator ErrorMessage="Número do Endereço: Preenchimento obrigatório."
                        ID="rfvEndNum" runat="server" ControlToValidate="txtEndNum" InitialValue="" ValidationGroup="SalvarForm"
                        ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                    </asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblEnd_Compl" runat="server" Text="Compl.:"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                        ID="txtEndCompl" runat="server" MaxLength="50" onkeypress="return endereco(event);" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblBairro" runat="server" Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox Height="18px" Style="text-transform: uppercase;" onblur="TextToUpper(this);"
                        ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                        Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvBairro" runat="server" ControlToValidate="txtBairro"
                        InitialValue="" ErrorMessage="Bairro do Endereço: Preenchimento obrigatório."
                        ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="reBairro" runat="server" ControlToValidate="txtBairro"
                        ErrorMessage="Bairro inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                        ValidationExpression="^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ0-9 ]{3,50}$"
                        ToolTip="Bairro Inválido!">
                        <img src="../Images/AlertaMens.gif" alt="Bairro inválido!"/>
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnContato" GroupingText="Contato" runat="server" Width="1350px">
        <table cellpadding="5px" border="0" cellspacing="0">
            <tr>
                <td style="text-align: right; width: 240px;">
                    <asp:Label ID="lblFone" runat="server" Text="Telefone:"></asp:Label>
                </td>
                <td style="width: 150px">
                    <asp:TextBox Height="18px" ID="txtFone" onkeyup="formataTelefoneDDD(this,event)"
                        runat="server" Width="100px" onblur="RetiraParenteses(this)" />
                    <asp:RegularExpressionValidator ID="reFone" runat="server" ControlToValidate="txtFone"
                        ErrorMessage="Telefone inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                        ValidationExpression="\([1][1-9]\)[1-5]\d{3}[-]\d{4}\d*|\([2-9][0-9]\)[1-5]\d{3}[-]\d{4}\d*"
                        ToolTip="Telefone Inválido!">
                        <img src="../Images/AlertaMens.gif" alt="Telefone inválido" />
                    </asp:RegularExpressionValidator>
                </td>
                <td style="text-align: right; width: 50px">
                    <asp:Label ID="lblCelular" runat="server" Text="Celular:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox Height="18px" ID="txtCelular" onkeyup="formataTelefoneDDD(this,event)"
                        runat="server" Width="100px" onblur="RetiraParenteses(this)" />
                    <asp:RegularExpressionValidator ID="reCelular" runat="server" ControlToValidate="txtCelular"
                        ErrorMessage="Celular inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                        ValidationExpression="\([1][1-9]\)[6-9]\d{3}[-]\d{4}\d*|\([2-9][0-9]\)[6-9]\d{3}[-]\d{4}\d*"
                        ToolTip="Celular Inválido!">
                        <img src="../Images/AlertaMens.gif" alt="Celular inválido" />
                    </asp:RegularExpressionValidator>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEmail" runat="server" Text="E-mail:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox Height="18px" onblur="TextToLower(this);" Style="text-transform: lowercase;"
                        ID="txtEmail" runat="server" Width="400px" MaxLength="100" AutoComplete="Off"
                        AutoCompleteType="Disabled" />
                    <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="E-mail inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="E-mail Inválido!">
                        <img src="../Images/AlertaMens.gif" alt="E-mail inválido" />
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label22" runat="server" Text="Confirma E-mail:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox Height="18px" Style="text-transform: lowercase;" onblur="TextToLower(this);"
                        ID="txtEmailConfirmacao" runat="server" MaxLength="100" Width="400px" AutoComplete="Off"
                        AutoCompleteType="Disabled"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="reConfirmacaoEmail" runat="server" ControlToValidate="txtEmailConfirmacao"
                        ErrorMessage="E-mail de confirmação inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="E-mail de confirmação inválido!">
                        <img src="../Images/AlertaMens.gif" alt="E-mail de confirmação inválido" />
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pndeficiencia" GroupingText="Deficiência" runat="server" Width="1350px">
        <asp:UpdatePanel ID="UpdateDeficiencia" runat="server">
            <ContentTemplate>
                <table cellpadding="5px" border="0" cellspacing="0">
                    <tr>
                        <td align="right" style="width: 240px;">
                            <asp:Label ID="Label23" runat="server" Text="Deficiência:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList Style="padding: 3px" ID="ddlNecessidadeEspecial" runat="server"
                                DataValueField="item" AutoPostBack="true" DataTextField="DESCRICAO" Width="289px"
                                OnSelectedIndexChanged="ddlNecessidadeEspecial_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trRecursosNecessarioProva" runat="server">
                        <td>
                        </td>
                        <td>
                            <asp:Panel ID="pnRecursos" GroupingText="Recursos necessários para aplicação de provas"
                                runat="server" Style="width: 700px">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="chkRecursoAplicacaoProva" runat="server" RepeatColumns="2"
                                                RepeatDirection="Vertical" RepeatLayout="Table" Width="100%">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 3px">
                                            <asp:CheckBox ID="chkNenhumRecursoAplicacaoProva" Text="Nenhum" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblRecursoAplicaProvaExclusivo" runat="server" RepeatDirection="Horizontal"
                                                RepeatColumns="2" RepeatLayout="Table" Width="100%">
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnOpcaoProcessoSeletivo" GroupingText="Opção para o Processo Seletivo"
        runat="server" Width="1350px">
        <asp:UpdatePanel ID="UpdateOpcaoProcessoSeletivo" runat="server" EnableViewState="true">
            <ContentTemplate>
                <table cellpadding="5px" border="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="right" style="width: 240px;">
                            <asp:Label ID="Label25" runat="server" Text="Unidade Ensino Pretendida*:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList Style="padding: 3px" ID="ddlUnidadeEnsino" runat="server" DataValueField="UNIDADEENSINOID"
                                AutoPostBack="true" DataTextField="NOMEUNIDADEENSINO" Width="500px" OnSelectedIndexChanged="ddlUnidadeEnsino_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUnidadeEnsino" runat="server" ControlToValidate="ddlUnidadeEnsino"
                                InitialValue="" ErrorMessage="Unidade de Ensino Pretendida: Preenchimento obrigatório."
                                ValidationGroup="SalvarForm" ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trMensagemUnidadeEnsino" runat="server" visible="false">
                        <td>
                        </td>
                        <td>
                            <div style="border: solid 1px #dd3c10; background-color: #ffebe8; font-family: Verdana;
                                color: #FF0000; font-weight: bold; text-align: justify; padding: 20px; width: 500px">
                                <asp:Label ID="lblMensagemUnidadeEnsino" runat="server" Style="font-size: small"
                                    Text=""></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label26" runat="server" Text="Curso*:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList Style="padding: 3px" ID="ddlCurso" runat="server" DataValueField="CURSOID"
                                AutoPostBack="true" DataTextField="NOMECURSO" Width="400px" OnSelectedIndexChanged="ddlCurso_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCurso" runat="server" ControlToValidate="ddlCurso"
                                InitialValue="" ErrorMessage="Curso Pretendido: Preenchimento obrigatório." ValidationGroup="SalvarForm"
                                ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label27" runat="server" Text="Turno*:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList Style="padding: 3px" ID="ddlTurno" runat="server" DataValueField="TURNOID"
                                DataTextField="NOMETURNO" Width="250px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTurno" runat="server" ControlToValidate="ddlTurno"
                                InitialValue="" ErrorMessage="Turno Pretendido: Preenchimento obrigatório." ValidationGroup="SalvarForm"
                                ToolTip="Campo Obrigatório!">
                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/>
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <table width="1350px" cellpadding="5px" border="0" cellspacing="0">
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:CheckBox ID="checkConfirmacao" runat="server" Font-Bold="True" Text="Declaro para todos os fins de direito serem verídicas as informações aqui prestadas, estando ciente de ser crime o uso de informações falsas." />
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:UpdatePanel ID="UpdateConfirmarDados" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btConfirmarDados" runat="server" Text="Confirmar Dados" OnClick="btConfirmarDados_Click"
                            ValidationGroup="SalvarForm" OnClientClick="return confirm('Confirma os dados informados para a Inscrição do Processo Seletivo?');" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" runat="server" CssClass="overlay">
                <asp:Panel ID="Panel2" runat="server" CssClass="loader">
                    <asp:Image ID="Image1" runat="server" AlternateText="Updating..." Height="48" ImageUrl="~/Images/updateProgress.gif"
                        Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
