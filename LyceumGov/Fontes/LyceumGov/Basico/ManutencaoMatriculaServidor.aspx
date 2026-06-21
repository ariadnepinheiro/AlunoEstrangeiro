<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" 
    CodeBehind="ManutencaoMatriculaServidor.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ManutencaoMatriculaServidor" 
    Title="Manutenção de Matrícula de Servidor"%>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function ConfirmaAtualizacao() {
            var idFuncionalAtual = $("#<%=txtIdFuncionalAtual.ClientID %>").val();
            var idFuncionalNovo = $("#<%=txtIdFuncionalNovo.ClientID %>").val();
            var vinculoAtual = $("#<%=txtVinculoAtual.ClientID %>").val();
            var vinculoNovo = $("#<%=txtVinculoNovo.ClientID %>").val();
            var matriculaAtual = $("#<%=txtMatriculaAtual.ClientID %>").val();
            var matriculaNova = $("#<%=txtMatriculaNova.ClientID %>").val();

            if (confirm("Confirma as alterações?\ndo Id Funcional " + idFuncionalAtual + " para " + idFuncionalNovo + "\ndo Vínculo " + vinculoAtual + " para " + vinculoNovo + "\nda matrícula " + matriculaAtual + " para " + matriculaNova)) {
                return true;
            }
            return false;
        }
        function OnlyNumericEntry(e) {

            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function handlePaste(e) {
            e.preventDefault();
            var texto = (e.clipboardData || window.clipboardData).getData('text');
            texto = texto.replace(/\D/g, '');
            document.execCommand('insertText', false, texto);
        }
    </script>
    
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o Id/Vínculo ou o nome do servidor ou funcionário"
        Width="930px">

        <table>
            <tr>
                <td>
                    <asp:Label ID="lblVinculoTSearch" runat="server" Text="Id/Vínculo do Servidor/Funcionário:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseVinculo" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryManutencaoVinculo"
                        AutoPostBack="true" OnTextChanged="tseVinculo_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 968px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" AlternateText="Editar Matrícula"
            Visible="false" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" OnClientClick="return ConfirmaAtualizacao();" />
        <asp:Label runat="server" ID="Label1" Text="Servidor/Funcionário" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsVinculo" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <asp:Panel ID="pnAbas" runat="server" Width="1000px" Visible="false">
        <dxtc:ASPxPageControl ID="pcManutencaoVinculo" runat="server" ActiveTabIndex="0"
            Width="800px">
            <TabPages>
                <dxtc:TabPage Text="Situação Atual">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblIdFuncionalAtual" runat="server" Text="Id Funcional Atual:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIdFuncionalAtual" runat="server" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblIdFuncionalNovo" runat="server" Text="Id Funcional Novo:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIdFuncionalNovo" runat="server" MaxLength="10" onkeypress="return OnlyNumericEntry(event)"
                                            Visible="false" onpaste="handlePaste(event)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblVinculoAtual" runat="server" Text="Vinculo Atual:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVinculoAtual" runat="server" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVinculoNovo" runat="server" Text="Vinculo Novo:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVinculoNovo" runat="server" MaxLength="10" onkeypress="return OnlyNumericEntry(event)"
                                            Visible="false" onpaste="handlePaste(event)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMatriculaAtual" runat="server" Text="Matricula Atual:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMatriculaAtual" runat="server" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMatriculaNova" runat="server" Text="Matrícula Nova:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMatriculaNova" runat="server" MaxLength="10" onkeypress="return OnlyNumericEntry(event)"
                                            Visible="false" onpaste="handlePaste(event)"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lblAviso" runat="server" Text="IMPORTANTE: Caso o campo Matrícula Nova fique vazio, Matrícula ATUAL será atualizado para VAZIO."
                                SkinID="lblObrigatorio" Visible="false"></asp:Label>
                            <br />
                            <br />
                            <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblNome" runat="server" Text="Nome Completo:"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNome" runat="server" Enabled="false" Width="350px">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDtNasc" runat="server" Text="Data Nascimento:"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDtNasc" runat="server" MinDate="1901-01-01" Enabled="false">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCPF" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblSexo" runat="server" Text="Sexo:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                                <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                                                <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <dxwgv:ASPxGridView ID="grdResumoLotacoes" runat="server" AutoGenerateColumns="False"
                                AllowPaging="true" EnableCallBacks="true" ClientInstanceName="grdResumoLotacoes"
                                Width="900px" OnAfterPerformCallback="grdResumoLotacoes_AfterPerformCallback">
                                <SettingsPager PageSize="10" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ORDEM" Visible="false" VisibleIndex="0">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="IDVINCULO" VisibleIndex="1">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="2">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="DESCRICAO" VisibleIndex="3">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="U.A." FieldName="SETOR" Width="30px" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="5">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="6">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data da Nomeação" FieldName="DATA_NOMEACAO"
                                        VisibleIndex="7">
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data da Dispensa" FieldName="DATA_DESATIVACAO"
                                        VisibleIndex="8">
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIO" VisibleIndex="9">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Última Atualização" FieldName="DATA_ATUALIZACAO"
                                        VisibleIndex="10">
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            <br />
                            <br />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Histórico de Atualização">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">
                            <br />
                            <dxwgv:ASPxGridView ID="grdHistoricoAtualizacao" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="grdHistoricoAtualizacao" OnAfterPerformCallback="grdHistoricoAtualizacao_AfterPerformCallback">
                                <SettingsPager PageSize="10" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="Data/Hora Atualização" FieldName="DataCadastro"
                                        VisibleIndex="1" Width="150px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="UsuarioId" VisibleIndex="2"
                                        Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Id Funcional Anterior" FieldName="IdFuncionalAnterior"
                                        VisibleIndex="3" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Id Funcional Novo" FieldName="IdFuncionalNovo"
                                        VisibleIndex="4" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Vínculo Anterior" FieldName="VinculoAnterior"
                                        VisibleIndex="5" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Vínculo Novo" FieldName="VinculoNovo" VisibleIndex="6"
                                        Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matrícula Anterior" FieldName="MatriculaAnterior"
                                        VisibleIndex="7" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matrícula Nova" FieldName="MatriculaNova"
                                        VisibleIndex="8" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
</asp:Content>
