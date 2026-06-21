<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarEncerramentoAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ListarEncerramentoAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" OnTextChanged="tseAluno_Changed" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnCompartilhada" runat="server" />
    <asp:HiddenField ID="hdnAno" runat="server" />
    <asp:HiddenField ID="hdnPeriodo" runat="server" />
    <asp:HiddenField ID="hdnCurso" runat="server" />
    <asp:HiddenField ID="hdnSerie" runat="server" />
    <br />
    <br />
    <asp:Panel ID="pnDados" GroupingText="" runat="server">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblUniEnsino" runat="server" Text="Unidade de Ensino:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtUniEnsino" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSituacao" runat="server" MaxLength="15" Width="600px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCurso" runat="server" Text="Escolaridade:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCurso" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeCurso" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTurno" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeTurno" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCurriculo" runat="server" Text="Currículo:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCurriculo" runat="server" MaxLength="20" Width="600px" ReadOnly="true"
                        Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSerie" runat="server" Text="Ano de Escolaridade:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSerie" runat="server" MaxLength="3" Width="50px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeSerie" runat="server" MaxLength="3" Width="600px" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdEncerramentos" runat="server" AutoGenerateColumns="False"
        SkinID="NoConfirmDelete" ClientInstanceName="grdEncerramentos" KeyFieldName="CompositeKey"
        OnCustomUnboundColumnData="grdEncerramentos_CustomUnboundColumnData" OnAfterPerformCallback="grdEncerramentos_AfterPerformCallback"
        OnSelectionChanged="grdEncerramentos_SelectionChanged">
        <SettingsBehavior ProcessSelectionChangedOnServer="True" AllowSort="false"/>
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <SelectButton Text="Selecionar" Visible="True">
                    <Image Url="~/img/bt_busca.png" />
                </SelectButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="aluno" VisibleIndex="1"
                Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="curso" VisibleIndex="2"
                ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="nome_curso" VisibleIndex="2"
                ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="turno" VisibleIndex="3"
                ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="nome_turno" VisibleIndex="3"
                ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Currículo" FieldName="curriculo" VisibleIndex="4"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano Ingresso" FieldName="ano_ingresso" VisibleIndex="5"
                ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Período Ingresso" FieldName="periodo_ingresso"
                VisibleIndex="6" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Encerramento" FieldName="dt_encerramento"
                VisibleIndex="7" ReadOnly="true">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Reabertura" FieldName="dt_reabertura"
                VisibleIndex="8" ReadOnly="true">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="motivo" VisibleIndex="9"
                ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="nome_motivo" VisibleIndex="9"
                ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Instituição" FieldName="instituicao" VisibleIndex="10"
                ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Instituição" FieldName="nome_instituicao"
                VisibleIndex="10" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Colação" FieldName="dt_colacao" VisibleIndex="11"
                ReadOnly="true">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Diploma" FieldName="dt_diploma" VisibleIndex="12"
                ReadOnly="true">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano Encerramento" FieldName="ano_encerramento"
                VisibleIndex="13" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Período Encerramento" FieldName="periodo_encerramento"
                VisibleIndex="14" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Causa" FieldName="causa" VisibleIndex="15"
                ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Causa" FieldName="nome_causa" VisibleIndex="15"
                ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn Caption="Motivo Reabertura" FieldName="motivoreabertura" VisibleIndex="16"
                ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                Visible="False" VisibleIndex="17">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <table>
        <tr>
            <td>
                <dxe:ASPxButton ID="btnEncerrar" runat="server" Text="Novo Encerramento" OnClick="btnEncerrar_Click">
                </dxe:ASPxButton>
            </td>
            <td>
                <dxe:ASPxButton ID="btnReabrir" runat="server" Text="Reabrir" Visible="false">
                </dxe:ASPxButton>
            </td>
        </tr>
    </table>
</asp:Content>
