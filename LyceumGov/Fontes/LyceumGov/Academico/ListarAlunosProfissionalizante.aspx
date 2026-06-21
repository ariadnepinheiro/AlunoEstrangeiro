<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="ListarAlunosProfissionalizante.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ListarAlunosProfissionalizante" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="conAlunosProfissionalizante" ContentPlaceHolderID="cphFormulario"
    runat="server">

    <script type="text/javascript">
        function SelChanged(s, e) {
            if (e.isSelected)
                grdProfissional.GetRowValues(e.visibleIndex, 'ALUNO', OnGridSelectionComplete);
        }

        function OnGridSelectionComplete(values) {
            var str = 'ALUNO=' + values;
            str = Base64.encode(str);

            window.open("Matricula.aspx?Chave=" + str);
        }
        function ConfirmaCancelarMatricula() {

            var paraCancelar = $("#<%=grdProfissional.ClientID %> input[id*='chkCancelado']:checked").filter(
                function() {
                    var canceladoId = $(this).attr("cancelado");
                    var cancelado = $("#" + canceladoId).attr("checked");

            var status = $(this).attr("status");

            return !cancelado && status != "Cancelado";
                }
            ).length;


                if (paraCancelar == 0) {
                alert("Para cancelar é necessário selecionar pelo menos um aluno.");
                return false;
            }

            if (confirm("Esta operação cancelará a(s) " + paraCancelar + " matricula(s) selecionada(s). Deseja continuar?")) {
                return true;
            }
            return false;
        }

       
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="750px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidadeTSearch" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidade_Ensino_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="40%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlAlunosProfissionalizante" runat="server" GroupingText="Lista de Alunos de Ensino Profissionalizante"
        Width="800px">
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdProfissional" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grdProfissional" DataSourceID="odsProfissional" KeyFieldName="ID_ALUNO_CONCOMITANTE"
                        OnHtmlRowCreated="grdProfissional_HtmlRowCreated" OnCommandButtonInitialize="grdProfissional_CommandButtonInitialize"
                        OnAfterPerformCallback="grdProfissional_AfterPerformCallback" OnSelectionChanged="grdProfissional_SelectionChanged">
                        <SettingsEditing Mode="Inline" />
                        <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="false" />
                        <SettingsCookies Enabled="false" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Caption="Enturmar">                             
                                <SelectButton Text="Enturmar" Visible="True">
                                    <Image Url="../img/bt_copiar.png" />
                                </SelectButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_ALUNO_CONCOMITANTE"
                                ReadOnly="true" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" ReadOnly="true" VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="PERIODO" ReadOnly="true"
                                VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" ReadOnly="true" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_ALUNO" ReadOnly="true"
                                Width="150px" VisibleIndex="6">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Enturmado" FieldName="ENTURMADO" ReadOnly="true"
                                VisibleIndex="6" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" ReadOnly="true"
                                VisibleIndex="7" Name="Status" Visible="false">
                            </dxwgv:GridViewDataTextColumn>

                            <dxwgv:GridViewDataCheckColumn Caption="Cancelar" FieldName="CANCELADO" Name="CANCELAR"
                                VisibleIndex="10">
                                <DataItemTemplate>
                                    <asp:CheckBox ID="chkCancelado" runat="server" Checked='<%# this.VerificarCheck(Eval("CANCELADO")) %>' />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataCheckColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        <ClientSideEvents SelectionChanged="function(s, e) { SelChanged(s, e); }" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnCancelarMatricula" Text="Cancelar Matrícula" runat="server" OnClick="btnCancelarMatricula_Click"
                        ValidationGroup="ConfirmaMatriculaForm" OnClientClick="return ConfirmaCancelarMatricula();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsProfissional" TypeName="Techne.Lyceum.Net.Academico.ListarAlunosProfissionalizante"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidade_Ensino" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
