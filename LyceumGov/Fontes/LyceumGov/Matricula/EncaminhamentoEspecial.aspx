<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="EncaminhamentoEspecial.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.EncaminhamentoEspecial" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            preencherDadosPorCEP({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCep.ClientID %>',
                nomeLogradouro: '<%=txtLogradouro.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });

        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }
        function mostrarResultado(box, num_max, spContador) {
            var contagem_carac = box.length;
            if (contagem_carac != 0) {
                document.getElementById(spContador).innerHTML = contagem_carac + " caracteres digitados";
                if (contagem_carac == 1) {
                    document.getElementById(spContador).innerHTML = contagem_carac + " caracter digitado";
                }
                if (contagem_carac >= num_max) {
                    document.getElementById(spContador).innerHTML = "Limite de caracteres excedido!";
                }
            } else {
                document.getElementById(spContador).innerHTML = "Limite de " + num_max + " caracteres";
            }
        }
        function contarCaracteres(box, valor, spContador, campoMult) {

            var conta = valor - box.length;
            document.getElementById(spContador).innerHTML = "Você ainda pode digitar " + conta + " caracteres";
            if (box.length >= valor) {
                document.getElementById(spContador).innerHTML = "Limite excedido.";
                campoMult.value = campoMult.value.substr(0, valor);
            }
        }    
    </script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnlBuscadados" runat="server" GroupingText="Preencha os dados abaixo para realizar a busca"
        Width="835px" Height="131px">
        <br />
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="Nome:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomeBusca" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                        Width="450px" />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label9" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:* "></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label7" runat="server" Text="Nome da Mãe:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomeMae" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                        Width="450px" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkNaoDeclarMae" runat="server" AutoPostBack="true" OnCheckedChanged="chkNaoDeclarMae_CheckedChanged"
                        Text="Não Declarada" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    Para alterar os dados de Idenficação do aluno, faça as alterações nos campos acima
                    e inicie novamente.
                </td>
                <td>
                    <asp:Button ID="btnBuscarCandidato" Text="Iniciar Inclusão" runat="server" OnClick="btnBuscarCandidato_Click"
                        OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnPessoa" runat="server" />
    <asp:HiddenField ID="hdnPreCadastroAlunoId" runat="server" />
    <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
    <br />
    <div id="dvGeral" runat="server" visible="false">
        <asp:Panel ID="pnDados" runat="server" GroupingText="Dados do Candidato" Width="835px">
            <table>
                <tr>
                    <td style="width: 60%">
                        <asp:Label ID="Label2" runat="server" Text="Nome do Candidato:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Sexo:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNomeCadastro" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                            Enabled="false" Width="450px" />
                    </td>
                    <td>
                        <asp:TextBox ID="DtNascimentoCadastro" MaxLength="10" runat="server" placeholder="DD/MM/YYYY"
                            Enabled="false" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSexo" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem Text="Selecione" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                            <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlFiliacao" runat="server" GroupingText="Filiação" Width="835px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Nome da Mãe:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNomeMaeCadastro" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                            Enabled="false" Width="450px" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarMaeCadastro" Text="Não Declarada" AutoPostBack="true"
                            OnCheckedChanged="chkNaoDeclarMaeCadastro_CheckedChanged" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Nome do Pai:"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNomePai" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                            Width="450px" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" AutoPostBack="true"
                            OnCheckedChanged="chkNaoDeclarPai_CheckedChanged" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlCEP" runat="server" GroupingText="Endereço" Width="835px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="CEP:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label17" runat="server" SkinID="lblObrigatorio" Text="Logradouro:* "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" AutoPostBack="false" />
                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEPRioLimitrofes"
                            Modal="true" SkinID="CEP" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtLogradouro" runat="server" MaxLength="100" Width="450px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label18" runat="server" Text="Número:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label19" runat="server" Text="Complemento:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNumero" runat="server" AutoPostBack="true" MaxLength="9" SkinID="numerico"
                            OnTextChanged="txtNumero_TextChanged" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComplemento" runat="server" MaxLength="100" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" Text="UF:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label21" runat="server" SkinID="lblObrigatorio" Text="Município:*"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label22" runat="server" SkinID="lblObrigatorio" Text="Bairro:*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input id="txtMunicipio" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBairro" runat="server" AppendDataBoundItems="True" DataValueField="CODIGO"
                            DataTextField="DESCRICAO" Enabled="false">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel7" runat="server" GroupingText="Documento Candidato" Width="835px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label23" runat="server" Text="CPF:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCPF" name="txtCPFResponsavel" MaxLength="14" runat="server" SkinID="numerico"
                            placeholder="999.999.999-99" onkeyup="formataCPF(this,event)"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel10" runat="server" GroupingText="Deficiência" Width="835px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label24" runat="server" SkinID="lblObrigatorio" Text="Indique as deficiências que o candidato possui:*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlDeficiencia" runat="server" AppendDataBoundItems="True"
                            DataTextField="DESCRICAO" DataValueField="NECESSIDADEESPECIALID" Style="width: 60%">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlOpcao" runat="server" GroupingText="Dados da Matrícula" Width="835px">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label27" runat="server" Text="Ano:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAnoMatricula" runat="server" DataTextField="ano" DataValueField="ano"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlAnoMatricula_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label1" runat="server" Text="Período:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodoMatricula" runat="server" DataTextField="periodo"
                            DataValueField="periodo" OnSelectedIndexChanged="ddlPeriodoMatricula_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label4" runat="server" Text="Unidade de Ensino:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseUnidadeEnsinoMatricula" AutoPostBack="true" runat="server"
                            Key="unidade_ens" Argument="nome_comp" Caption="" MaxLength="20" GridWidth="850px"
                            OnChanged="tseUnidadeEnsinoMatricula_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="municipio" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label8" runat="server" Text="Nível/Segmento*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlNivelMatricula" runat="server" DataTextField="DESCRICAO"
                            DataValueField="TIPO" AutoPostBack="true" OnSelectedIndexChanged="ddlNivelMatricula_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label12" runat="server" Text="Modalidade*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlModalidadeMatricula" runat="server" DataTextField="DESCRICAO"
                            DataValueField="MODALIDADE" AutoPostBack="true" OnSelectedIndexChanged="ddlModalidadeMatricula_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label13" runat="server" Text="Curso:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlCursoMatricula" runat="server" DataTextField="NOME" DataValueField="CURSO"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCursoMatricula_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label14" runat="server" Text="Turno:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTurnoMatricula" runat="server" DataTextField="descricao"
                            DataValueField="turno" OnSelectedIndexChanged="ddlTurnoMatricula_SelectedIndexChanged"
                            AutoPostBack="true" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label25" runat="server" Text="Série/Ano Escolar:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSerieMatricula" runat="server" DataTextField="serie" DataValueField="serie"
                            AppendDataBoundItems="false">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel1" runat="server" GroupingText="Motivo da Encaminhamento Especial"
            Width="835px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label15" runat="server" SkinID="lblObrigatorio" Text="Indique o motivo do encaminhamento:*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlMotivo" runat="server" AppendDataBoundItems="True" DataTextField="DESCRICAO"
                            DataValueField="MOTIVOENCAMINHAMENTOESPECIALID" Style="width: 60%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Font-Names="Verdana" ID="Label26" runat="server" Text="Observação:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtObservacao" runat="server" MaxLength="500" TextMode="MultiLine"
                            name="txtObservacao" Height="75px" Width="600px" onkeyup="mostrarResultado(this.value,500,'spContador');contarCaracteres(this.value,500,'spContador',this)" />
                        <br />
                        <span id="spContador" style="font-family: Georgia;">Limite de 500 caracteres</span><br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table align="right" width="50%">
            <tr>
                <td>
                    <asp:Button ID="btnFinalizar" runat="server" Style="text-align: right" Text="Finalizar"
                        OnClick="btnFinalizar_Click" ValidationGroup="Finalizar" OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
