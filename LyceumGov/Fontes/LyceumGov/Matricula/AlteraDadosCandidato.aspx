<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AlteraDadosCandidato.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.AlteraDadosCandidato" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script type="text/javascript">


        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
    </script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnlBuscadados" runat="server" GroupingText="Preencha os dados abaixo para realizar a busca"
        Width="835px">
        <br />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        onchange="Bloqueio()" Width="70px" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tableBusca" runat="server" Visible="false">
            <tr>
                <td colspan="4" height="50px">
                    <asp:RadioButtonList ID="rblTipoBusca" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblTipoBusca_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Busca por Num. de Inscrição e Dt. Nasc." Value="1"></asp:ListItem>
                        <asp:ListItem Text="Busca por CPF" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trPorCPF" runat="server" visible="false">
                <td style="text-align: right">
                    <asp:Label ID="Label37" runat="server" Text="CPF:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBuscaCPF" MaxLength="14" runat="server" SkinID="numerico"></asp:TextBox>
                </td>
            </tr>
            <tr id="trPorNumInscricao" runat="server" visible="false">
                <td style="text-align: right">
                    <asp:Label ID="Label35" runat="server" Text="Nº Inscrição:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBuscaNumInscricao" runat="server" MaxLength="9" onkeypress="return onlyNumbers(event);"
                        Width="250px" />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label9" runat="server" Text="Data de Nascimento:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtBuscaDataNasc" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="40px" valign="bottom">
                    <asp:Button ID="btnBuscarCandidato" Text="Buscar Candidato" runat="server" OnClick="btnBuscarCandidato_Click"
                        OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnInscricaoAlunoId" runat="server" />
    <asp:HiddenField ID="hdnPreCadastroAlunoId" runat="server" />
    <asp:HiddenField runat="server" ID="hdnCodMunicipio" />
    <br />
    <div id="dvGeral" runat="server" visible="false">
    
        <asp:Panel ID="Panel1" runat="server" GroupingText="Alteração de Dados do Candidato" Width="835px" Visible="false">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label36" runat="server" Text="Nome do Candidato:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label50" runat="server" Text="Data de Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtAlteraNome" runat="server" MaxLength="100" onkeypress="return nomeSemNumMasComApostrofo(event);" Width="650px" />
                    </td>
                    <td>
                        <%--<asp:TextBox ID="dtAlteraNascimento" MaxLength="10" runat="server" placeholder="DD/MM/YYYY" Width="100px" />--%>
                        <dxe:ASPxDateEdit ID="dtAlteraNascimento" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                            ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje" EditFormatString="dd/MM/yyyy">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Nome da Mãe:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtAlteraNomeMae" runat="server" MaxLength="100" onkeypress="return nomeSemNumMasComApostrofo(event);" Width="650px" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkAlteraMaeNaoDeclarada" Text="Não Declarada" AutoPostBack="true" Width="100px" OnCheckedChanged="chkAlteraMaeNaoDeclarada_CheckedChange" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td width="150px">
                                    <asp:Label ID="Label52" runat="server" Text="Rede de Origem:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td width="100px">&nbsp;</td>
                                <td><asp:Label ID="Label6" runat="server" Text="Matrícula:" SkinID="lblObrigatorio" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlAlteraRedeOrigem" runat="server" OnSelectedIndexChanged="ddlAlteraRedeOrigem_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                        <asp:ListItem Text="ESTADUAL" Value="Estadual"></asp:ListItem>
                                        <asp:ListItem Text="FEDERAL" Value="Federal"></asp:ListItem>
                                        <asp:ListItem Text="MUNICIPAL" Value="Municipal"></asp:ListItem>
                                        <asp:ListItem Text="PARTICULAR" Value="Particular"></asp:ListItem>
                                        <asp:ListItem Text="AFASTADO" Value="Afastado"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAlteraSouAluno" runat="server" Text="Sou aluno" OnCheckedChanged="chkAlteraSouAluno_CheckedChange" AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAlteraMatricula" runat="server" Visible="false"></asp:TextBox>    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar Alterações" OnClick="btnSalvar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblMensagemSomenteLeitura" runat="server" SkinID="lblMensagem" Text="Ano selecionado não bate com o ano corrente da fase aberta da matrícula." Visible="false"></asp:Label>
        <br />
        <br />
        <asp:Panel ID="pnDados" runat="server" GroupingText="Dados do Candidato" Width="835px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label13" runat="server" SkinID="lblObrigatorio" Text="Número Inscrição:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Nome do Candidato:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:* "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNumeroInscricao" runat="server" Width="100px" Enabled="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeCadastro" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"
                            Enabled="false" Width="450px" />
                    </td>
                    <td>
                        <asp:TextBox ID="DtNascimentoCadastro" MaxLength="10" runat="server" placeholder="DD/MM/YYYY"
                            Width="100px" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label25" runat="server" SkinID="lblObrigatorio" Text="Rede de Origem:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label14" runat="server" SkinID="lblObrigatorio" Text="Estado Civil:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" SkinID="lblObrigatorio" Text="Sexo:* "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtRedeOrigem" runat="server" Width="100px" Enabled="false" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEstadoCivil" runat="server" AppendDataBoundItems="True"
                            Width="200px" DataValueField="item" DataTextField="descr" Enabled="false">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSexo" runat="server" AppendDataBoundItems="True" Enabled="false"
                            Width="100px">
                            <asp:ListItem Selected="True" Text="Selecione" Value=""></asp:ListItem>
                            <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                            <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label15" runat="server" SkinID="lblObrigatorio" Text="Nacionalidade:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label28" runat="server" SkinID="lblObrigatorio" Text="Município de Nascimento:* "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label26" runat="server" SkinID="lblObrigatorio" Text="UF de Nascimento:* "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlNacionalidade" runat="server" AppendDataBoundItems="True"
                            DataValueField="NACIONALIDADE" DataTextField="NOME" Enabled="false">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMunicipioNascimento" runat="server" AppendDataBoundItems="True"
                            Width="200px" DataValueField="CODIGO" DataTextField="NOME" Enabled="false">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUfNascimento" runat="server" AppendDataBoundItems="True"
                            Width="100px" DataValueField="UF_SIGLA" DataTextField="UF_SIGLA" Enabled="false">
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
                            Enabled="false" />
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
                            Width="450px" Enabled="false" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarPai" Text="Não Declarado" AutoPostBack="true"
                            Enabled="false" />
                    </td>
                </tr>
                <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
            </table>
            <asp:Panel ID="pnlNaoPossuiIrmao" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox runat="server" ID="chkNaoPossuiIrmao" Text="Não possui irmão na rede ou se inscrevendo."
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlIrmaoRede" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox runat="server" ID="chkIrmaoRede" Text="Possui irmão menor de 18 anos matriculado na Rede SEEDUC"
                                Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label41" runat="server" Text="Número da matricula do irmão:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtMatriculaIrmao" runat="server" Width="450px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label43" runat="server" Text="DADOS DO SEU IRMÃO" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label44" runat="server" Text="Nome:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label45" runat="server" Text="Data Nascimento:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNomeIrmaoRede" runat="server" Width="350px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataNascIrmaoRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label46" runat="server" Text="Unidade Escolar:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label47" runat="server" Text="Série:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUEIrmaoRede" runat="server" Width="350px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtSerieIrmaoRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label48" runat="server" Text="Curso:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label49" runat="server" Text="Turno:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCursoIrmaoRede" runat="server" Width="350px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTurnoIrmaoRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlIrmaoForaRede" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox runat="server" ID="chkIrmaoForaRede" Text="Possui irmão realizando simultaneamente inscrição no Matrícula Fácil"
                                AutoPostBack="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label42" runat="server" Text="Número da inscrição do irmão:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnIdIrmao" runat="server" />
                            <asp:TextBox ID="txtInscricaoIrmao" runat="server" Width="200px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label38" runat="server" Text="DADOS DO SEU IRMÃO" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label39" runat="server" Text="Nome:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label40" runat="server" Text="Data Nascimento:" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNomeIrmaoForaRede" runat="server" Width="200px" Enabled="false" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataNascIrmaoForaRede" runat="server" Width="150px" Enabled="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:ObjectDataSource ID="odsOpcaoIrmaoForaRede" TypeName="Techne.Lyceum.Net.Matricula.AlteraDadosCandidato"
                                runat="server" SelectMethod="ListarOpcaoIrmaoForaRede">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnIdIrmao" DefaultValue="" Name="inscricaoAlunoId"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdOpcoesIrmaoForaRede" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="grdOpcoesIrmaoForaRede" DataSourceID="odsOpcaoIrmaoForaRede"
                                KeyFieldName="OPCAOINSCRICAOID">
                                <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="OPCAOINSCRICAOID" FieldName="OPCAOINSCRICAOID"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" FieldName="DATACADASTRO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Opção" FieldName="OPCAO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="ESCOLA">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="DESCRICAOMODALIDADE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DESCRICAOTURNO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
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
                        <asp:TextBox ID="txtCep" runat="server" SkinID="numerico" MaxLength="8" AutoPostBack="false"
                            Enabled="false" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtLogradouro" runat="server" MaxLength="100" Width="450px" Enabled="false" />
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
                            Enabled="false" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtComplemento" runat="server" MaxLength="100" Width="250px" Enabled="false" />
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
                        <input id="txtEstado" runat="server" maxlength="20" readonly="readonly" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input id="txtMunicipio" runat="server" maxlength="20" readonly="readonly" />
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
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="Label23" runat="server" Text="CPF:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label29" runat="server" Text="Identidade:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label30" runat="server" Text="Orgão Emissor:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label31" runat="server" Text="UF Emissor:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCPF" MaxLength="14" runat="server" SkinID="numerico" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRgNumero" Style="width: 145px;" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRgEmissor" Style="width: 145px;" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRgUf" Style="width: 145px;" runat="server" Enabled="false" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <asp:Panel ID="Panel8" runat="server" GroupingText="Tipo Certidão*">
                            <div>
                                <asp:RadioButtonList ID="rblTipoCertidao" runat="server" RepeatDirection="Horizontal"
                                    Enabled="false">
                                    <asp:ListItem Text="Nascimento" Value="Nascimento"></asp:ListItem>
                                    <asp:ListItem Text="Casamento" Value="Casamento"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </asp:Panel>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 50%">
                        <asp:Panel ID="Panel9" runat="server" GroupingText="Modelo Certidão*">
                            <div>
                                <asp:RadioButtonList ID="rblModeloCertidao" runat="server" RepeatDirection="Horizontal"
                                    Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="rblModeloCertidao_SelectedIndexChanged">
                                    <asp:ListItem Text="Modelo Novo" Value="Modelo Novo"></asp:ListItem>
                                    <asp:ListItem Text="Modelo Antigo" Value="Modelo Antigo"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAntigo" runat="server" Visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCertNasc" runat="server" Text="Número do Termo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Numero" runat="server" MaxLength="15" SkinID="numerico"
                                            Enabled="false" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCertNascFolha" runat="server" Text="Folha:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Folha" runat="server" MaxLength="15" Enabled="false" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNascLivro" runat="server" Text="Livro:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_CertNasc_Livro" runat="server" MaxLength="15" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlNovo" runat="server" Visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNumMatricula" runat="server" Text="Número da matrícula:* " SkinID="lblObrigatorio"></asp:Label>
                                        <asp:TextBox ID="txtNumMatriculaCertidao" runat="server" MaxLength="32" Enabled="false"
                                            Width="200px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
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
                            Enabled="false" DataTextField="DESCRICAO" DataValueField="NECESSIDADEESPECIALID"
                            Style="width: 60%">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel3" runat="server" GroupingText="Contato" Width="835px">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <asp:Label ID="lblEmail" runat="server" Text="E-mail:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Telefone Celular:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Telefone Celular ou Fixo:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="450px" AutoComplete="Off" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelular" runat="server" Enabled="false" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelularFixo" runat="server" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel11" runat="server" GroupingText="Dados do Responsável" Width="835px">
            <table style="width: 100%">
                <tr>
                    <td colspan="4">
                        <div>
                            <asp:RadioButtonList ID="rblResponsavel" runat="server" RepeatDirection="Horizontal"
                                Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="rblResponsavel_SelectedIndexChanged">
                                <asp:ListItem Text="Pai" Value="Pai"></asp:ListItem>
                                <asp:ListItem Text="Mãe" Value="Mãe"></asp:ListItem>
                                <asp:ListItem Text="O próprio candidato" Value="Próprio Aluno"></asp:ListItem>
                                <asp:ListItem Text="Outros" Value="Outros"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label1" runat="server" Text="Nome do Responsável:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="Celular do Responsável:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtResponsavel" runat="server" MaxLength="100" Width="450px" Enabled="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelularResponsavel" runat="server" MaxLength="14" Enabled="false"
                            Style="width: 120px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label27" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label32" runat="server" Text="Identidade:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label33" runat="server" Text="Orgão Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label34" runat="server" Text="UF Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="text" id="txtCPFResponsavelCadastro" name="documentosCPF" runat="server"
                            readonly="readonly" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRgResponsavelNumero" Style="width: 120px;" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRgResponsavelEmissor" Style="width: 120px;" runat="server" Enabled="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRgResponsavelUf" Style="width: 120px;" runat="server" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:ObjectDataSource ID="odsOpcao" TypeName="Techne.Lyceum.Net.Matricula.AlteraDadosCandidato"
            runat="server" SelectMethod="ListarOpcao">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnInscricaoAlunoId" DefaultValue="" Name="inscricaoAlunoId"
                    PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdOpcoes" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdOpcoes"
            DataSourceID="odsOpcao" KeyFieldName="OPCAOINSCRICAOID">
            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="OPCAOINSCRICAOID" FieldName="OPCAOINSCRICAOID"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" FieldName="DATACADASTRO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Opção" FieldName="OPCAO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="ESCOLA">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="DESCRICAOMODALIDADE">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DESCRICAOTURNO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Posição na fila de espera" FieldName="POSICAOFILA">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
        <br />
        <div id="dvFase1" runat="server" visible="false">
            <asp:ObjectDataSource ID="odsFase1" TypeName="Techne.Lyceum.Net.Matricula.AlteraDadosCandidato"
                runat="server" SelectMethod="ListarFase1">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnInscricaoAlunoId" DefaultValue="" Name="inscricaoAlunoId"
                        PropertyName="Value" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <dxwgv:ASPxGridView ID="grdOpcoesFase1" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdOpcoes" DataSourceID="odsFase1" KeyFieldName="OPCAOINSCRICAOID">
                <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
                <Columns>
                    <dxwgv:GridViewDataTextColumn Caption="OPCAOINSCRICAOID" FieldName="OPCAOINSCRICAOID"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" FieldName="DATACADASTRO">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Opção" FieldName="OPCAO">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="ESCOLA">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="DESCRICAOMODALIDADE">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DESCRICAOTURNO">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </div>
        <br />
        <asp:ObjectDataSource ID="odsOpcoesFinalizadas" TypeName="Techne.Lyceum.Net.Matricula.AlteraDadosCandidato"
            runat="server" SelectMethod="ListarOpcaoHist">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnInscricaoAlunoId" DefaultValue="" Name="inscricaoAlunoId"
                    PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdOpcoesFinalizadas" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdOpcoesFinalizadas" DataSourceID="odsOpcoesFinalizadas"
            KeyFieldName="OPCAOINSCRICAOID">
            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s, e); }" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="OPCAOINSCRICAOID" FieldName="OPCAOINSCRICAOID"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" FieldName="DATACADASTRO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="ESCOLA">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="DESCRICAOMODALIDADE">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DESCRICAOTURNO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data da Situação" FieldName="DATASITUACAO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="MOTIVO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Fase 1 - Opção Alocação" FieldName="OPCAOFASE1">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
