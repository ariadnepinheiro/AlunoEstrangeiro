<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DisciplinasOptativasConfirmacao.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.DisciplinasOptativasConfirmacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="700px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfirmacaoMatricula"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                        <QueryParameters>
                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblAno_Ingresso" runat="server"
                        Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsConfirmacao" runat="server" TypeName="Techne.Lyceum.Net.Academico.DisciplinasOptativasConfirmacao"
        SelectMethod="Lista" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <table>
        <tr>
            <td>
                <dxwgv:ASPxGridView ClientInstanceName="grdConfirmacao" ID="grdConfirmacao" runat="server" DataSourceID="odsConfirmacao"
                    EnableCallBacks="false" AutoGenerateColumns="False" KeyFieldName="ID_CONFIRMACAO_MATRICULA" Width="100%"                     
                    OnCellEditorInitialize="grdConfirmacao_CellEditorInitialize" OnStartRowEditing="grdConfirmacao_StartRowEditing"
                    OnRowUpdating="grdConfirmacao_RowUpdating">
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                            <CancelButton Visible="true" Text="Cancelar">
                                <Image Url="~/img/bt_cancelar.png" />
                            </CancelButton>
                            <EditButton Visible="True" Text="Editar">
                                <Image Url="../img/bt_editar.png" />
                            </EditButton>
                            <ClearFilterButton Text="Limpar" Visible="True">
                                <Image Url="~/img/bt_limpa.png" />
                            </ClearFilterButton>
                            <UpdateButton Visible="true" Text="Alterar">
                                <Image Url="../img/bt_salvar.png" />
                            </UpdateButton>
                        </dxwgv:GridViewCommandColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_CONFIRMACAO_MATRICULA"
                            Name="ID_CONFIRMACAO_MATRICULA" VisibleIndex="0" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="ALUNO" FieldName="ALUNO" Name="ALUNO" VisibleIndex="2"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Ano Letivo" FieldName="ANO" VisibleIndex="3">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Período Letivo" FieldName="PERIODO" VisibleIndex="4">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO"
                            VisibleIndex="5">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Modalidade / Segmento / Curso" FieldName="MOD_SEG_CURSO"
                            VisibleIndex="6">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Série/Ano Escolar" FieldName="SERIE" VisibleIndex="7">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="8">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Data Sugerida" FieldName="DT_SUGERIDA_FORMATADA"
                            VisibleIndex="9">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Ensino Religioso" FieldName="ENSINO_RELIGIOSO"
                            Name="ENSINO_RELIGIOSO" VisibleIndex="10">
                            <DataItemTemplate>
                                <asp:CheckBox ID="chkEnsinoReligioso" runat="server" Enabled="false" Checked='<%# this.VerificarCheck(Eval("ENSINO_RELIGIOSO")) %>' />
                            </DataItemTemplate>
                        </dxwgv:GridViewDataCheckColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Língua Estrangeira Facultativa" FieldName="LINGUA_ESTRANGEIRA_FACULTATIVA"
                            Name="LINGUA_ESTRANGEIRA_FACULTATIVA" VisibleIndex="11">
                            <DataItemTemplate>
                                <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Enabled="false" Checked='<%# this.VerificarCheck(Eval("LINGUA_ESTRANGEIRA_FACULTATIVA")) %>' />
                            </DataItemTemplate>
                        </dxwgv:GridViewDataCheckColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Data Situação" FieldName="DT_ALTERACAO_FORMATADA"
                            VisibleIndex="13">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="STATUS" VisibleIndex="14">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Ensino Religioso" FieldName="PODE_ENSINO_RELIGIOSO"
                            VisibleIndex="15" visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Lingua Estrangueira Facultativa" FieldName="PODE_LINGUA_ESTRANGEIRA"
                            VisibleIndex="16" visible="false">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </td>
        </tr>
    </table>
</asp:Content>
