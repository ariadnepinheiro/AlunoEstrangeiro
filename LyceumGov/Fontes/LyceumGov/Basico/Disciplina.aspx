<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Disciplina.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Disciplina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function limpaNotaMax() {
            var txNotaMax = document.getElementById("<%=txNota_Max.ClientID %>");
            var ddlGrupo = document.getElementById("<%=ddlGrupo_Nota.ClientID %>");

            var campo = ddlGrupo.value;

            if (campo == "<Nenhum>") {
                txNotaMax.value = '';
            }

        }

        function validaDecimais(campo) {
            if (campo.value != '') {
                var regra = /^\d{1,3}(,\d{1,4})?$/i;
                var valida = regra.exec(campo.value);
                if (!valida) {
                    campo.value = '';
                    //campo.setFocus();
                    alert('O campo deve ter no máximo 3 números inteiros e 4 casas decimais.');
                }
            }
        }

        function validaDecimais8(campo) {
            //if (campo.value != '') {
            var regra = /^\d{1,8}(,\d{1,2})?$/i;
            var valida = regra.exec(campo.value);
            if (!valida) {
                campo.value = '0';
                //campo.setFocus();
                alert('O campo deve ter no máximo 8 números inteiros e 2 casas decimais.');
            }
            //}
        }

        function validaDecimal3(campo, e) {
            var regra1 = /^\d{1,3}(,\d{1,4})?$/i;
            var regra2 = /^\d{1,3},(\d|\n)?/i;
            var valida1 = regra1.exec(campo.value);
            var valida2 = regra2.exec(campo.value);
            if (e.keyCode != 13 && (!valida1 && !valida2)) {
                //alert('Não é válida.');
            }
        }

        function validaDecimal8(campo, e) {
            var regra1 = /^\d{1,8}(,\d{1,2})?$/i;
            var regra2 = /^\d{1,8},(\d|\n)?/i;
            var valida1 = regra1.exec(campo.value);
            var valida2 = regra2.exec(campo.value);
            if (e.keyCode != 13 && (!valida1 && !valida2)) {
                //alert('Não é válida.');
            }
        }

        //Usado para permitir apenas número ou vírgula
        function NumeroComVirgula(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            //alert(charCode);
            if ((charCode > 47 && charCode <= 57) || (charCode == 44) || (charCode == 8))
                return true;

            return false;
        }    
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Componente Curricular"
        Width="610px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDisciplinaTSearch" runat="server" Text="Componente Curricular:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseDisciplina" runat="server" Argument="nome" Key="disciplina"
                        SqlOrder="disciplina" SqlSelect="SELECT disciplina, nome,tem_nota, tem_freq FROM LY_DISCIPLINA"
                        OnChanged="tseDisciplina_Changed" MaxLength="20">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="disciplina" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="tem_nota" FieldName="tem_nota" Width="10%" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="tem_freq" FieldName="tem_freq" Width="10%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 900px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Componente Curricular" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDisciplina" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcDisciplina" runat="server" ActiveTabIndex="3" Height="550px"
        Width="900px" Visible="false">
        <TabPages>
            <dxtc:TabPage Name="DadosGerais" Text="Dados Gerais">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <table cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnDisciplina" runat="server" GroupingText="Componente Curricular">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblDisciplina" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Código:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDisciplina" runat="server" ColumnName="disciplina" MaxLength="20"
                                                        FieldName="Código" Width="200px">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDisciplina" runat="server" ControlToValidate="txtDisciplina"
                                                        InitialValue="" ErrorMessage="Código: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblNome" runat="server" Font-Names="Verdana" Font-Size="Smaller" Text="Componente Curricular:* "
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td align="left" valign="bottom">
                                                    <asp:TextBox ID="txNome" runat="server" ColumnName="nome" FieldName="Disciplina"
                                                        MaxLength="100" Width="680px" RequiredFieldValidation="True">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txNome"
                                                        Display="Dynamic" InitialValue="" ErrorMessage="Componente Curricular: Preenchimento obrigatório."
                                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblComponente" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Componente:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlComponente" runat="server" DataValueField="item" DataTextField="descr"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlComponente_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvComponente" runat="server" ControlToValidate="ddlComponente"
                                                        InitialValue="" ErrorMessage="Componente: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblAreaConhecimento" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Área de Conhecimento:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAreaConhecimento" runat="server" DataValueField="item" DataTextField="descr">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvAreaConhecimento" runat="server" ControlToValidate="ddlAreaConhecimento"
                                                        Enabled="false" InitialValue="" ErrorMessage="Área de Conhecimento: Preenchimento obrigatório."
                                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblNomeFantasia" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Código SARE:* " Visible="true" SkinID="lblObrigatorio">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txNome_Fantasia" runat="server" Caption="" ColumnName="nome_fantasia"
                                                        MaxLength="3" Width="35px" Visible="true" RequiredFieldValidator="true" SkinID="numerico">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNomeFantasia" runat="server" ControlToValidate="txNome_Fantasia"
                                                        InitialValue="" ErrorMessage="Código SARE: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                                                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnChecks" runat="server" GroupingText="Detalhes">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblAtiva" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Ativa:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckAtiva" runat="server" Caption="" ColumnName="ativa" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblMultipla" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Disciplina múltipla:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkMultipla" runat="server" Caption="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblVerHorario" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Verifica no Q.H.I.:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckVerifica_Horario" runat="server" Caption="" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblAtividadeCompl" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Atividade Complementar:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckAtividadeCompl" runat="server" Caption="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblEstagio" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Estágio:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckEstagio" runat="server" Caption="" ColumnName="estagio" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="Smaller" Text="Eletiva:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkEletiva" runat="server" Caption="" AutoPostBack="true" OnCheckedChanged="chkEletiva_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td valign="top">
                                    <asp:Panel ID="pnlGrupoEletiva" runat="server" GroupingText="Grupo Eletiva" Enabled="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList runat="server" ID="rblGrupoEletiva" RepeatDirection="Vertical">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnHoras" runat="server" GroupingText="Carga Horária">
                                        <table id="tblCargaHoraria" runat="server">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblHorasAula" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Hora de Aula Total:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox Width="60px" ID="txHoras_Aula" runat="server" Caption="" ColumnName="horas_aula"
                                                        DataType="Number" MaxLength="4" onkeypress="return NumeroComVirgula(event);"
                                                        onChange="validaDecimais8(this);">
                                                    </asp:TextBox>
                                                </td>
                                                <td rowspan="3">
                                                    <asp:CustomValidator ID="cvHoras" SetFocusOnError="true" ErrorMessage="Um dos campos Hora de Aula Total, Hora de Estágio Total ou Hora de Atividade Total deve ser informado."
                                                        ToolTip="Um dos campos Hora de Aula Total, Hora de Estágio Total ou Hora de Atividade Total deve ser informado."
                                                        OnServerValidate="ValidaHoras_ServerValidate" ValidationGroup="SalvarForm" runat="server">
                                                        <table>
                                                            <tr><td><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></td></tr>
                                                            <tr><td><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></td></tr>
                                                            <tr><td><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></td></tr>
                                                        </table>                                                        
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblHorasEstagio" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Hora de Estágio Total: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txHoras_Estagio" Width="60px" runat="server" Caption="" ColumnName="horas_estagio"
                                                        DataType="Number" MaxLength="4" onkeypress="return NumeroComVirgula(event);"
                                                        onChange="validaDecimais8(this);">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblHorasAtividade" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Hora de Atividade Total:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHorasAtividade" Width="60px" runat="server" onkeypress="return NumeroComVirgula(event);"
                                                        onChange="validaDecimais8(this);" DataType="Number" MaxLength="4">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblHorasSemanais" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Total de Aulas Semanais:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txHorasSemanais" Width="60px" runat="server" DataType="Number" MaxLength="3"
                                                        onkeypress="return NumeroComVirgula(event);" onChange="validaDecimais8(this);">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvHorasSemanais" runat="server" ControlToValidate="txHorasSemanais"
                                                        InitialValue="" ErrorMessage="Total de Aulas Semanais: Preenchimento obrigatório."
                                                        ValidationGroup="SalvarForm" Display="Dynamic">
                                                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td valign="top">
                                    <asp:Panel ID="pnAvaliacao" runat="server" GroupingText="Tipo de Avaliação">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblTemNota" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Pontuação: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckTem_Nota" runat="server" Caption="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblTemFreq" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Frequência: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckTem_Freq" runat="server" Caption="" ColumnName="tem_freq" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblRelatorio" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Relatório: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRelatorio" runat="server" Caption="" ColumnName="tem_aval_descritiva" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="DadosComplementares" Text="Dados Complementares">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnNotas" runat="server" GroupingText="Nota">
                                        <table id="tblNota" runat="server">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblGrupo" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Grupo: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGrupo_Nota" runat="server" Caption="" ColumnName="grupo_nota"
                                                        CssClass="ReadOnlyField" DataValueField="grupo" AutoPostBack="True" Width="200px"
                                                        OnSelectedIndexChanged="ddlGrupo_Nota_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblNotaMaxima" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Nota Máxima:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txNota_Max" runat="server" Caption="" ColumnName="nota_max" CssClass="ReadOnlyField"
                                                        FieldName="Nota Máxima" MaxLength="8">
                                                    </asp:TextBox>
                                                    <asp:DropDownList ID="ddlConceitoNota" Visible="false" runat="server" DataTextField="descricao"
                                                        DataValueField="conceito">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ErrorMessage="Nota Máxima: Preenchimento obrigatório."
                                                        ID="rfvNotaMax" runat="server" ControlToValidate="txNota_Max" InitialValue=""
                                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ErrorMessage="Nota Máxima: Preenchimento obrigatório."
                                                        ID="rfvConceitoNota" runat="server" ControlToValidate="ddlConceitoNota" InitialValue=""
                                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblCasasDecNota" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Casas Decimais: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txN_Casas_Dec" runat="server" Caption="" ColumnName="n_casas_dec"
                                                        CssClass="ReadOnlyField" DataType="Number" MaxLength="1" SkinID="numerico">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Panel ID="pnReprovaPrimeiro" runat="server" GroupingText="Reprova Primeiro"
                                        Visible="false">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblPriorizaFreq" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Por frequência: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckPrioriza_Freq" runat="server" Caption="" ColumnName="prioriza_freq"
                                                        CssClass="ReadOnlyField" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnFormulas" runat="server" GroupingText="Critérios para Aprovação/ Média"
                                        Visible="false">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFAprov1" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Fórmula de Aprovação 1: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txFormula_Ca1" runat="server" Caption="" ColumnName="formula_ca1"
                                                        Columns="100" CssClass="ReadOnlyField" MaxLength="200" Width="720px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFMediaFin1" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Fórmula da Média Final 1: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txFormula_Mf1" runat="server" Caption="" ColumnName="formula_mf1"
                                                        Columns="100" CssClass="ReadOnlyField" MaxLength="200" Width="720px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblConcMin1" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Conceito Mínimo 1: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txConceito_Min1" runat="server" Caption="" ColumnName="conceito_min_1"
                                                        Columns="30" CssClass="ReadOnlyField" MaxLength="15" Width="150px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFAprov2" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Fórmula de Aprovação 2: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txFormula_Ca2" runat="server" Caption="" ColumnName="formula_ca2"
                                                        Columns="100" CssClass="ReadOnlyField" MaxLength="200" Width="720px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFMedia2" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Fórmula da Média Final 2: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txFormula_Mf2" runat="server" Caption="" ColumnName="formula_mf2"
                                                        Columns="100" CssClass="ReadOnlyField" MaxLength="200" Width="720px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblConcMin2" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Conceito Mínimo 2: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txConceito_Min2" runat="server" Caption="" Columns="30" CssClass="ReadOnlyField"
                                                        MaxLength="15" Width="150px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFAprov3" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Fórmula de Aprovação 3:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txFormula_Ca3" runat="server" Caption="" ColumnName="formula_ca3"
                                                        Columns="100" CssClass="ReadOnlyField" MaxLength="200" Width="720px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblFMedia3" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Fórmula da Média 3: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txFormula_Mf3" runat="server" Caption="" ColumnName="formula_mf3"
                                                        Columns="100" CssClass="ReadOnlyField" MaxLength="200" Width="720px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblConcMin3" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Conceito Mínimo 3: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txConceito_Min3" runat="server" Caption="" ColumnName="conceito_min_3"
                                                        Columns="30" CssClass="ReadOnlyField" MaxLength="15" Width="150px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblConcMinEx1" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Conceito Min. Exame 1: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txConceito_Min_Ex" runat="server" Caption="" ColumnName="conceito_min_ex"
                                                        Columns="30" CssClass="ReadOnlyField" MaxLength="15" Width="150px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lblConcMinEx2" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                        Text="Conceito Min. Exame 2: "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txConceito_Min_Ex_2" runat="server" Caption="" ColumnName="conceito_min_ex_2"
                                                        Columns="30" CssClass="ReadOnlyField" MaxLength="15" Width="150px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="GrupoHabilitacoes" Text="Grupos de Habilitações">
                <ContentCollection>
                    <dxw:ContentControl ID="ccGrupoHabilitacao" runat="server">
                        <dxwgv:ASPxGridView ID="grdGrupoHabilitacoes" runat="server" AutoGenerateColumns="False"
                            DataSourceID="odsGrupoHabilitacao" OnRowDeleting="grdGrupoHabilitacoes_RowDeleting"
                            Font-Names="Verdana" Font-Size="Small" OnInitNewRow="grdGrupoHabilitacoes_InitNewRow"
                            OnCustomUnboundColumnData="grdGrupoHabilitacoes_CustomUnboundColumnData" KeyFieldName="CompositeKey">
                            <SettingsText EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="agrupamento" VisibleIndex="1" Caption="Grupo"
                                    Visible="true" Width="400px">
                                    <PropertiesTextEdit Width="400px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="descricao" VisibleIndex="2" Caption="Descrição"
                                    Visible="true" Width="400px">
                                    <PropertiesTextEdit Width="400px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    VisibleIndex="5" Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="DisciplinaMultipla" Text="Disciplinas Múltiplas">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <dxwgv:ASPxGridView ID="grdDisciplinaMultipla" runat="server" ClientInstanceName="grdDisciplinaMultipla"
                            AutoGenerateColumns="False" Font-Names="Verdana" Font-Size="Small" KeyFieldName="CompositeKey"
                            DataSourceID="odsGridDisciplinaMultipla" OnInitNewRow="grdDisciplinaMultipla_InitNewRow"
                            OnAfterPerformCallback="grdDisciplinaMultipla_AfterPerformCallback" OnRowValidating="grdDisciplinaMultipla_RowValidating"
                            OnRowInserting="grdDisciplinaMultipla_RowInserting" OnRowDeleting="grdDisciplinaMultipla_RowDeleting"
                            OnCustomUnboundColumnData="grdDisciplinaMultipla_CustomUnboundColumnData">
                            <SettingsText EmptyDataRow="Não existem dados." />
                            <SettingsEditing Mode="EditForm" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="50px" CellStyle-Wrap="False">
                                    <CellStyle Wrap="False">
                                    </CellStyle>
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdDisciplinaMultipla.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton>
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="disciplina" VisibleIndex="1" Caption="Componente Curricular"
                                    Visible="false" Width="400px">
                                    <PropertiesTextEdit Width="400px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="disciplina_multipla" VisibleIndex="1" Caption="Código"
                                    Width="100px">
                                    <PropertiesTextEdit Width="100px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn FieldName="disciplina_multipla" VisibleIndex="2"
                                    Caption="Disciplina Múltipla" Visible="true" Width="300px" Name="disciplina_multipla">
                                    <PropertiesComboBox DataSourceID="odsDisciplinaMultipla" ValueField="disciplina"
                                        TextField="nome_compl" ValueType="System.String" Width="300px" DropDownWidth="300px">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="HORAS_AULA" VisibleIndex="3" Caption="Carga Horária"
                                    Width="100px">
                                    <PropertiesTextEdit Width="100px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    VisibleIndex="5" Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="color: #FF0000">
                                                        <dxe:ASPxTextBox runat="server" ID="txtDisciplina" Text='<%# Bind("disciplina") %>'
                                                            Width="150" Enabled="false" Visible="false">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDisciplina" runat="server" Text="Componente Curricular:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <tweb:TSearchBox ID="tseDisciplina" runat="server" Argument="nome" Key="disciplina"
                                                            SqlOrder="disciplina" SqlSelect="SELECT disciplina, nome FROM ly_disciplina"
                                                            Value='<%# Bind("disciplina_multipla") %>' MaxLength="20" AutoPostBack="false">
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="disciplina" Width="30%" />
                                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                        </dxwgv:ASPxGridView>
                        <asp:ObjectDataSource ID="odsDisciplinaMultipla" runat="server" TypeName="Techne.Lyceum.Net.Basico.Disciplina"
                            SelectMethod="ListarDisciplinaMultipla">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseDisciplina" PropertyName="DBValue" Name="disciplina" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsGridDisciplinaMultipla" runat="server" TypeName="Techne.Lyceum.Net.Basico.Disciplina"
                            SelectMethod="Listar">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseDisciplina" PropertyName="DBValue" Name="disciplina" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsGrupoHabilitacao" runat="server" TypeName="Techne.Lyceum.Net.Basico.Disciplina"
                            SelectMethod="ListarGrupohabilitacaoDisciplina" DeleteMethod="DeleteGrupoHabilitacao">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseDisciplina" PropertyName="DBValue" Name="disciplina" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
