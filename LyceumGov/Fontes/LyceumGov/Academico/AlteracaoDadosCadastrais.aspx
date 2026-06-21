<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AlteracaoDadosCadastrais.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.AlteracaoDadosCadastrais" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 
    <script type="text/javascript">
        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        };

        function CheckedChangedMae(objeto, objetoDeclaracao) {
            var check = window.document.getElementById(objeto).checked;

            if (check) {
                if (window.confirm('Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome da mãe.'))
                    window.document.getElementById(objetoDeclaracao).checked = check;
                else {
                    window.document.getElementById(objetoDeclaracao).checked = (!check);
                }
            }
            else {
                window.document.getElementById(objetoDeclaracao).visible = check;
            }
        }

        function CheckedChangedPai(objeto, objetoDeclaracao) {
            var check = window.document.getElementById(objeto).checked;

            if (check) {
                if (window.confirm('Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome do pai.'))
                    window.document.getElementById(objetoDeclaracao).checked = check;
                else {
                    window.document.getElementById(objetoDeclaracao).checked = (!check);
                }
            }
            else {
                window.document.getElementById(objetoDeclaracao).visible = check;
            }
        }



        function nomeSemNumComApost(b) {
            var a;           
            if (window.event) {
                a = window.event.keyCode
            }
            else {
                if (event) {
                    a = event.keyCode
                }
                else {
                    if (b) {
                        a = b.which
                    }
                    else {
                        return true
                    }
                }
            }

            if ((a >= 65 && a <= 90) || (a >= 97 && a <= 122) || (a >= 192 && a <= 255) || (a == 32) || (a == 45) || (a == 39)) {
                return true
            }
            else {
                return false
            }

            return true
        }


        function removeEspacosDuplicados(a) {
            a = a.replace(/\s{2,}/g, ' ');
        }

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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
   <script type="text/javascript">

       $(document).ready(function() {
       preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
               cep: '<%=txtCep.ClientID %>',
               nomeLogradouro: '<%=txtEndereco.ClientID %>',
               nomeBairro: '<%=txtBairro.ClientID %>',
               nomeMunicipio: '<%=txtMunicipio.ClientID %>',
               codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
               uf: '<%=txtEstado.ClientID %>'
           });
       });
    </script>


    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfRenovacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed" OnLoad="tseAluno_Load">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Alunos" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Panel ID="pntabDadosPessoais" runat="server" Visible="false">
        <asp:HiddenField ID="hddTxtEmail" runat="server" />
        <asp:HiddenField ID="hddDataAlteracaoEmail" runat="server" />
        <asp:Panel ID="pnPessoa" GroupingText="Dados Pessoais" runat="server">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblPessoa" runat="server" SkinID="lblObrigatorio" Text="Pessoa:* "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbltxtPessoa" runat="server" Text="Valor gerado após inclusão do aluno."
                                        Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtPessoa" runat="server" MaxLength="10" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNome" runat="server" SkinID="lblObrigatorio" Text="Nome:* "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNomeCompl" runat="server" MaxLength="100" onkeypress="return nomeSemNumComApost(event);"
                                        Width="600px" />
                                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="X-Small" ForeColor="red"
                                        Text="(Preencher sem abreviações)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNomeSocial" runat="server" Text="Nome Social: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNomeSocial" runat="server" MaxLength="100" onkeypress="return nomeSemNumComApost(event);"
                                        Width="600px" />
                                    <asp:LinkButton ID="hplLink" Font-Size="12px" Font-Bold="true" OnClick="hplLinkNomeSocial_Click"
                                        OnClientClick="window.document.forms[0].target='_blank';" runat="server">Saiba Mais</asp:LinkButton>
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
                                                <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
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
                                    <asp:Label ID="lblSexo" runat="server" SkinID="lblObrigatorio" Text="Sexo:* "></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" DataValueField="sexo"
                                        Width="150px">
                                        <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                                        <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblFilhos" runat="server" Text="Quantidade de Filhos: "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFilhos" runat="server" MaxLength="2" Width="50px" SkinID="numerico" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label runat="server" ID="lblEtnia" Text="Etnia:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEtnia" runat="server" DataTextField="NOME" DataValueField="TABELAITEMID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEst_Civil" runat="server" Text="Estado Civil:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label runat="server" ID="lblPaisNasc" Text="País de Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaisNasc" runat="server" DataTextField="nome" DataValueField="codigo"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaisNasc_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
                            OnSelectedIndexChanged="ddlNacionalidade_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblNaturalidadeUF" runat="server" Text="UF de Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUFNaturalidade" runat="server" DataTextField="uf_sigla"
                            DataValueField="uf_sigla" OnSelectedIndexChanged="ddlUFNaturalidade_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblNaturalidade" runat="server" Text="Naturalidade:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="5">
                        <tweb:TSearchBox ID="tseNaturalidade" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                            Columns="10" ArgumentColumns="30" AutoPostBack="true" Key="codigo" MaxLength="10"
                            OnChanged="tseNaturalidade_Changed" OnLoad="tseNaturalidade_Load">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                        <asp:TextBox ID="txtMunicipioNaturalidade" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlFiliacao" GroupingText="Filiação" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label2" runat="server" Text="Nome da Mãe:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeMae" runat="server" Width="250px" MaxLength="100" onkeypress="return nomeSemNumComApost(event); removeApostrofosDuplicados(event);" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" Width="140px"
                            AutoPostBack="true" OnCheckedChanged="chkNaoDeclarMae_CheckedChanged" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkFalecidaMae" Text="Falecida" Width="140px" AutoPostBack="true"
                            OnCheckedChanged="chkFalecidaMae_CheckedChanged" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPFMae" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                            onkeyup="formataCPF(this,event)" />
                    </td>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTelefoneMae" runat="server"
                            Text="Telefone:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtTelefoneMae" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                            Width="100px" MaxLength="14" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CheckBox runat="server" ID="chkDeclaroAusenciaMae" Text="Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome da mãe"
                            Visible="false" SkinID="lblObrigatorio" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label5" runat="server" Text="Nome do Pai:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomePai" runat="server" Width="250px" MaxLength="100" onkeypress="return nomeSemNumComApost(event); removeApostrofosDuplicados(event);" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" Width="140px"
                            AutoPostBack="true" OnCheckedChanged="chkNaoDeclarPai_CheckedChanged" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkFalecidoPai" Text="Falecido" Width="140px" AutoPostBack="true"
                            OnCheckedChanged="chkFalecidoPai_CheckedChanged" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label6" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPFPai" runat="server" Width="150px" MaxLength="20" SkinID="numerico"
                            onkeyup="formataCPF(this,event)" />
                    </td>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label7" runat="server" Text="Telefone:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtTelefonePai" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                            Width="100px" MaxLength="14" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CheckBox runat="server" ID="chkDeclaroAusenciaPai" Text="Declaro que foi conferida a certidão de nascimento/casamento e não consta o nome do pai"
                            Visible="false" SkinID="lblObrigatorio" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label8" runat="server" Text="Responsável Legal:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblResponsavel" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rblResponsavel_SelectedIndexChanged" AutoPostBack="true">
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
                        <asp:TextBox ID="txtNomeResponsavel" runat="server" Width="250px" Visible="false" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Visible="false" Font-Size="Smaller" ID="lblCPFResponsavel"
                            runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPFResponsavel" runat="server" Width="150px" SkinID="numerico"
                            onkeyup="formataCPF(this,event)" Visible="false" />
                    </td>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Visible="false" Font-Size="Smaller" ID="lblTelefoneResponsavel"
                            runat="server" Text="Telefone:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtTelefoneResp" Visible="false" onkeyup="formataTelefoneDDD(this,event)"
                            runat="server" Width="100px" MaxLength="14" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnEndereco" GroupingText="Endereço" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCEP" runat="server" Text="CEP:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" AutoPostBack="false" />
                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                            Modal="true" SkinID="CEP" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMunicipio" runat="server"
                                        Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
                                    <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="250px"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEndereco" runat="server"
                                        Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"
                                        Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Num" runat="server"
                                        Text="N.º:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15" />
                                </td>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEnd_Compl" runat="server"
                                        Text="Compl.:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndCompl" runat="server" MaxLength="50" onkeypress="return endereco(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblBairro" runat="server"
                                        Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                        Width="400px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblLocalZona" runat="server"
                                        Text="Localização/Zona<br> de Residência:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLocalZona" runat="server">
                                        <asp:ListItem Text="Urbana" Value="Urbana"> </asp:ListItem>
                                        <asp:ListItem Text="Rural" Value="Rural"> </asp:ListItem>
                                        <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblLocalizacaoDiferenciada" runat="server" Text="Localização Diferenciada: * "
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxCheckBox AutoPostBack="true" ID="chkNaoSeAplica" ValueChecked="S" ValueUnchecked="N"
                                                    ValueType="System.String" runat="server" Checked="true" Text="Não se aplica"
                                                    EnableViewState="false" OnCheckedChanged="chkNaoSeAplica_CheckedChanged">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkQuilombos" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Comunidade quilombola">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkAreaTradicional" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Área onde se localizam povos e comunidades tradicionais">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxCheckBox ID="chkAreaAssentamento" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Área de assentamento">
                                                </dxe:ASPxCheckBox>
                                                <dxe:ASPxCheckBox ID="chkTerraIndigena" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                    runat="server" Checked="false" Text="Terra indígena">
                                                </dxe:ASPxCheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <asp:Panel ID="pnContato" GroupingText="Contato" runat="server">
            <table>
                <tr>
                    <td style="text-align: right; width: 50px">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone ou Celular:"></asp:Label>
                    </td>
                    <td style="width: 415px">
                        <asp:TextBox ID="txtFone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                            MaxLength="14" Width="100px" />
                    </td>
                    <td style="text-align: right; width: 50px;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblCelular" runat="server"
                            Text="Celular:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)" runat="server"
                            MaxLength="14"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50px;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                            SkinID="lblObrigatorio" Text="E-mail:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" Style="text-transform: lowercase;" onchange="txtEmailTextChanged()"
                            onblur="txtEmailTextBlur()" runat="server" Width="400px" MaxLength="100" />
                    </td>
                    <td colspan="2">
                        <asp:Table ID="tblDadosEmail" runat="server" GridLines="None">
                            <asp:TableRow ID="rowDtAtualizacaoEmail">
                                <asp:TableCell>
                                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label25" runat="server" Text="Data de Atualização do E-mail:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtDataAtualizacaoEmail" runat="server" ReadOnly="true" Width="150px"
                                        MaxLength="100" Style="text-transform: lowercase" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnDocumento" GroupingText="Outros Documentos" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event)" runat="server" MaxLength="50"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Tipo" runat="server"
                            Text="Tipo:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRGTipoPessoa" runat="server" DataValueField="item" DatatTextField="descr"
                            Width="130px" OnSelectedIndexChanged="ddlRGTipoPessoa_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Num" runat="server"
                            Text="Número:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRGNum" runat="server" MaxLength="20" Width="160px" SkinID="numeroDocumento" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblComplIdentidade" runat="server"
                            Text="Complemento da identidade:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComplIdentidade" runat="server" MaxLength="20" Width="160px"
                            Visible="true" SkinID="numeroDocumento" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_UF" runat="server"
                            Text="Estado:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                            Width="130px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Emissor" runat="server"
                            Text="Órgão Emissor:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                            Width="160px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblRG_Data" runat="server"
                            Text="Data de Expedição:"></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtDataExped" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje" Width="140px">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnDocumentos_CertNasc" runat="server" GroupingText="Certidão Civil">
            <table>
                <tr>
                    <td style="text-align: left">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTipoCertidao" runat="server"
                            Text="Tipo Certidão Civil:*" SkinID="lblObrigatorio" Style="text-align: center"></asp:Label><asp:DropDownList
                                ID="ddlTipoCertidao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCertidao_SelectedIndexChanged">
                                <asp:ListItem Text="Não Informado" Value="Nenhum">
                                </asp:ListItem>
                                <asp:ListItem Text="Nascimento" Value="Nascimento"></asp:ListItem>
                                <asp:ListItem Text="Casamento" Value="Casamento"> </asp:ListItem>
                                <asp:ListItem Selected="True" Text="Selecione" Value=""> </asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTipoCertidaoCivil" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDeclaroCertidaoCivil" runat="server" Style="text-align: left"
                                            SkinID="lblObrigatorio" Text="Declaro que o aluno não apresentou a certidão de nascimento/casamento de acordo com o motivo descrito" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMotivoCertidao" runat="server"
                                            Text="Motivo:*" SkinID="lblObrigatorio"></asp:Label><asp:TextBox ID="txtMotivoCertidaoCivil"
                                                runat="server" Width="462px" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="lblCertCivil" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                            Text="Certidão Civil:*" Style="text-align: left"></asp:Label><asp:DropDownList ID="ddlCertidaoCivil"
                                runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCertidaoCivil_SelectedIndexChanged">
                                <asp:ListItem Text="Selecione" Value=""> </asp:ListItem>
                                <asp:ListItem Text="Modelo Antigo" Value="Modelo Antigo"> </asp:ListItem>
                                <asp:ListItem Text="Modelo Novo" Value="Modelo Novo"> </asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAntigo" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblUFCartorio" runat="server" Text="UF do Cartório: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUFCartorio" runat="server" AutoPostBack="true" DataTextField="UF"
                                            DataValueField="codigo_uf" OnSelectedIndexChanged="ddlUFCartorio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMunicipioCartorio" runat="server" Text="Município do Cartório: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlMunicipioCartorio" runat="server" AutoPostBack="true" DataTextField="municipio"
                                            DataValueField="codigo_municipio" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMunicipioCartorio_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCartorio" runat="server" Text="Cartório: "></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlCartorio" runat="server" DataTextField="nome_cartorio" DataValueField="cod_cartorio">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCertNasc" runat="server" Text="Número do Termo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Numero" runat="server" MaxLength="15" SkinID="numerico" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCertNascEmissao" runat="server" Text="Data de Emissão: "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dboDOC_CertNasc_DtEmissao" runat="server" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje" MinDate="1901-01-01">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="CertNascUF" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDOC_CertNasc_Uf" runat="server" DataTextField="sigla" DataValueField="sigla">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCertNascFolha" runat="server" Text="Folha: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Folha" runat="server" MaxLength="15" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNascLivro" runat="server" Text="Livro: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Livro" runat="server" MaxLength="15" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlNovo" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNumMatricula" runat="server" Text="Número da matrícula: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumMatriculaCertidao" Width="200px" runat="server" MaxLength="32"
                                            SkinID="numerico" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="true" ShowSizeGrip="False" HeaderText="Confirmação de Alteração de Dados"
        EnableAnimation="false" Width="500px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            <p style="text-align: justify;">
                                <b><font color='#FF0000'>ATENÇÃO:</font></b> Advertimos que toda alteração de dados
                                pessoais promovida no Sistema Conexão Educação deve ser respaldada por documentos
                                constantes na pasta física do aluno e que o Sistema Jurídico Brasileiro tipifica
                                como crime a inserção de dados falsos, alteração ou exclusão indevida de dados corretos
                                nos sistemas informatizados ou bancos de dados da Administração Pública com o fim
                                de obter vantagem indevida para si ou para outrem ou para causar dano, na forma
                                do art. 313-A do Código Penal.</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                Você está prestes a alterar dados pessoais de um (a) aluno(a) da Rede Estadual de
                                Educação com matrícula ativa. Você tem certeza na alteração proposta?</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
