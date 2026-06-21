<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ProgramasDoAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ProgramasDoAluno" %>

<asp:Content ID="ctProgramasAluno" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="600px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdProgramasAluno" runat="server" AutoGenerateColumns="False"
        Visible="False" ClientInstanceName="grdProgramasAluno" DataSourceID="tdsProgramasAluno"
        KeyFieldName="id_aluno_programas" Width="970px" OnCellEditorInitialize="grdProgramasAluno_CellEditorInitialize"
        OnInitNewRow="grdProgramasAluno_InitNewRow" OnStartRowEditing="grdProgramasAluno_StartRowEditing"
        OnRowValidating="grdProgramasAluno_RowValidating" OnRowInserting="grdProgramasAluno_RowInserting"
        OnAfterPerformCallback="grdProgramasAluno_AfterPerformCallback" OnHtmlEditFormCreated="grdProgramasAluno_HtmlEditFormCreated">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="conProgramaAluno" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblProgramaUnidade" runat="server" Text="Programa Social:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseProgramaUnidade" AutoPostBack="false" runat="server" Key="id_unidade_ensino_programas"
                                        ArgumentColumns="120" Value='<%# Bind("id_unidade_ensino_programas") %>' Argument="descricao"
                                        DataType="Number" SqlSelect="SELECT distinct id_unidade_ensino_programas, ap.nome_agencia, ap.nome_programa, tb.descricao descricao_tipo_beneficio, ap.NOME_AGENCIA + ' - ' + ap.NOME_PROGRAMA +  ' - ' + tb.DESCRICAO as descricao, ano_validade from Ly_unidade_ensino_programas uep inner join LY_ALUNO a on a.UNIDADE_ENSINO = uep.UNIDADE_ENS inner join LY_AGENCIA_PROGRAMA ap on uep.AGENCIA = ap.AGENCIA and uep.PROGRAMA = ap.PROGRAMA inner join LY_TIPO_BENEFICIO tb on uep.TIPO_BENEFICIO = tb.TIPO_BENEFICIO"
                                        Caption="" MaxLength="20" GridWidth="850px">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_unidade_ensino_programas" Width="12%" />
                                            <tweb:TSearchBoxColumn Caption="descricao" FieldName="descricao" Visible="false" />
                                            <tweb:TSearchBoxColumn Caption="Agência de Fomento" FieldName="nome_agencia" Width="25%" />
                                            <tweb:TSearchBoxColumn Caption="Programa" FieldName="nome_programa" Width="25%" />
                                            <tweb:TSearchBoxColumn Caption="Tipo de Benefícios" FieldName="descricao_tipo_beneficio"
                                                Width="25%" />
                                            <tweb:TSearchBoxColumn Caption="Ano de Validade" FieldName="ano_validade" Width="13%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblDataIni" runat="server" Text="Data de Início:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ReplacementType="EditFormCellEditor"
                                        ColumnID="dt_inicio" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblDataFim" runat="server" Text="Data Final:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" ReplacementType="EditFormCellEditor"
                                        ColumnID="dt_fim" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
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
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdProgramasAluno.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID_ALUNO_PROGRAMAS" FieldName="id_aluno_programas"
                VisibleIndex="3" ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Agência" FieldName="id_unidade_ensino_programas"
                Visible="false" VisibleIndex="3">
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="aluno" Visible="False" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Agência de Fomento*" FieldName="nome_agencia"
                Width="170px" VisibleIndex="1" ReadOnly="True">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Programa*" FieldName="nome_programa" VisibleIndex="2"
                Width="170px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Tipo de Benefícios*" FieldName="descricao"
                VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano de Validade*" FieldName="ano_validade"
                Width="100px" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data de Início*" FieldName="dt_inicio" VisibleIndex="5"
                Width="100px">
                <PropertiesDateEdit MinDate="1899-12-31" Width="100px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data de início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data final*" FieldName="dt_fim" VisibleIndex="6"
                Width="100px">
                <PropertiesDateEdit MinDate="1899-12-31" Width="100px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data final." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsProgramasUnidade" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_unidade_ensino_programas">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsProgramasAluno" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_aluno_programas"
        SqlWhere="aluno = @aluno">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="aluno" PropertyName="DBValue" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsAgencia" runat="server" TypeName="Techne.Lyceum.RN.ProgramasUnidade"
        SelectMethod="ConsultarAgencias"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoBeneficio" runat="server" TypeName="Techne.Lyceum.RN.ProgramasUnidade"
        SelectMethod="ConsultarTipoBeneficio"></asp:ObjectDataSource>
</asp:Content>
