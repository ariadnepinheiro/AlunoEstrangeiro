<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DuplicidadePessoa.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.DuplicidadePessoa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function Letras(b) {
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
        
        function ConfirmaAlteracao() {

            var selectResposta = $("[id$='rblSituacao']").find(":checked").val();

            if (typeof selectResposta == 'undefined') {

                return true;
            }

            if (selectResposta == 'Ativo') {
                if (confirm("Ao confirmar a alteração com os dados finais da matricula escolhida para situação de ''Ativo'', as demais matrículas serão ''Canceladas''.\nDeseja realmente continuar?")) {
                    return true;
                }
                return false;
            }
            if (selectResposta == 'Cancelado') {
                if (confirm("Ao confirmar a alteração todas as matrículas serão ''Canceladas''.\nDeseja realmente continuar?")) {
                    return true;
                }
                return false;
            }
            return true;
        }
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Aluno" Width="80%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="Label10" SkinID="lblObrigatorio" Text="Aluno*:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstanceName="lblMensagem"></asp:Label>
    <asp:Panel ID="pnDuplicidade" runat="server" Visible="false" Width="40%">
        <br />
        <asp:Panel ID="pnlDados" runat="server" Width="80%" GroupingText="Dados de identificação">
            <asp:Label ID="Label2" runat="server" Style="font-family: arial; color: black; font-size: 11px;
                font-weight: bold;" Text="Caso necessite alterar a identificação do aluno, ajuste através dos campos abaixo:"></asp:Label>
            <br />
            <br />
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNome" runat="server" SkinID="lblObrigatorio" Text="Nome do aluno:* "></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtNomeCompl" runat="server" MaxLength="100" onkeypress="return Letras(event);"
                            Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label3" runat="server" Text="Nome da Mãe:*" onkeypress="return Letras(event);"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNomeMae" runat="server" Width="350px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkNaoDeclarMae" Text="Não Declarada" Width="140px"
                            AutoPostBack="true" OnCheckedChanged="chkNaoDeclarMae_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblDtNasc" runat="server" SkinID="lblObrigatorio" Text="Data Nascimento:* "></asp:Label>
                    </td>
                    <td colspan="2">
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
                    <td align="center" colspan="2">
                        <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlGrid" runat="server" Visible="false">
            <dxwgv:ASPxGridView ID="grdDuplicidade" runat="server" AutoGenerateColumns="False"
                Width="100%" Visible="true" ClientInstanceName="grdDuplicidade" KeyFieldName="PESSOA"
                EnableCallBacks="false" OnCustomUnboundColumnData="grdDuplicidade_CustomUnboundColumnData">
                <SettingsBehavior ConfirmDelete="True" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsEditing Mode="Inline" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataCheckColumn Caption="" FieldName="" Name="Utilizar" VisibleIndex="1">
                        <DataItemTemplate>
                            <asp:CheckBox ID="ckUtilizar" runat="server" />
                        </DataItemTemplate>
                    </dxwgv:GridViewDataCheckColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="PESSOA" VisibleIndex="2"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="ALUNO" VisibleIndex="3"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="3"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome mãe" FieldName="NOME_MAE" VisibleIndex="4"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome pai" FieldName="NOME_PAI" VisibleIndex="4"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Data nascimento" FieldName="DT_NASC" VisibleIndex="5"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="6" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="FONE" VisibleIndex="7">
                        <PropertiesTextEdit MaxLength="10">
                            <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" />
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULAR" VisibleIndex="8"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULARFORMATADO" VisibleIndex="8"
                        UnboundType="String">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Endereço" FieldName="ENDERECO" VisibleIndex="9"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Numero" FieldName="END_NUM" VisibleIndex="10"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Bairro" FieldName="BAIRRO" VisibleIndex="11"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="E_MAIL" VisibleIndex="12"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Possui Foto" FieldName="POSSUI_FOTO" VisibleIndex="13"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Possui Solicitação Gratuidade" FieldName="GRATUIDADE"
                        VisibleIndex="14" Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SIT_ALUNO" VisibleIndex="15"
                        Visible="true">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsDuplicidade" TypeName="Techne.Lyceum.Net.Academico.DuplicidadePessoa"
                runat="server" SelectMethod="ListaDuplicidade">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
                    <asp:ControlParameter ControlID="txtNomeCompl" Name="nomeAluno" PropertyName="Text" />
                    <asp:ControlParameter ControlID="txtNomeMae" Name="nomeMae" PropertyName="Text" />
                    <asp:ControlParameter ControlID="dtDataNasc" Name="dataNascimento" PropertyName="Value" />
                    <asp:ControlParameter ControlID="hdnMatriculas" Name="matricula" PropertyName="Value" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlInformacao" runat="server" GroupingText="Dados Final para a Matrícula Escolhida">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblSituacao" runat="server" SkinID="lblObrigatorio" Text="Situação:* "></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblSituacao" runat="server" RepeatDirection="Horizontal"
                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="rblSituacao_SelectedIndexChanged">
                            <asp:ListItem Text="Ativa" Value="Ativo"></asp:ListItem>
                            <asp:ListItem Text="Cancelada" Value="Cancelado"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblMensagemSituacao" runat="server" SkinID="lblMensagem" ClientInstanceName="lblMensagem"></asp:Label>
            <br />
            <asp:Panel ID="pnlDadosAluno" runat="server" Visible="false">
                <asp:HiddenField runat="server" ID="hdnMatriculas" />
                <table>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label5" runat="server" Text="Dados da Confirmação Confirmada:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ddlConfirmacaoMatricula" runat="server" ValueField="ID_CONFIRMACAO_MATRICULA"
                                TextFormatString="{0} - {1}|{2} - {3}" DropDownWidth="700px" Width="480px" Height="5px"
                                ClientInstanceName="ddlConfirmacaoMatricula">
                                <Columns>
                                    <dxe:ListBoxColumn Caption="Código" FieldName="ID_CONFIRMACAO_MATRICULA" Width="15%" />
                                    <dxe:ListBoxColumn Caption="Ano" FieldName="ANO" Width="10%" />
                                    <dxe:ListBoxColumn Caption="Periodo" FieldName="PERIODO" Width="10%" />
                                    <dxe:ListBoxColumn Caption="Modalidade/Segmento/Curso" FieldName="MOD_SEG_CURSO"
                                        Width="60%" />
                                    <dxe:ListBoxColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO" Width="60%" />
                                    <dxe:ListBoxColumn Caption="Série" FieldName="SERIE" Width="10%" />
                                    <dxe:ListBoxColumn Caption="Turno" FieldName="NOME_TURNO" Width="20%" />
                                    <dxe:ListBoxColumn Caption="Turno" FieldName="TURNO" Width="20%" Visible="false" />
                                    <dxe:ListBoxColumn Caption="Curso" FieldName="CURSO" Width="20%" Visible="false" />
                                    <dxe:ListBoxColumn Caption="TipoVaga" FieldName="TIPOVAGAOCUPADA" Width="20%" Visible="false" />
                                </Columns>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                            OnClientClick=" Bloqueio();return ConfirmaAlteracao();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
