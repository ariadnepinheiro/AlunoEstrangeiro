<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ProgramasPorUnidade.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ProgramasPorUnidade" %>

<asp:Content ID="ctProgramasUnidade" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnAgenciaChanged(cmbAgencia) {
            if (typeof cmbAgencia != 'undefined' && cmbAgencia != null) {
                if (typeof cmbAgencia.GetValue() != 'undefined' && cmbAgencia.GetValue() != null)
                    cmbPrograma.PerformCallback(cmbAgencia.GetValue().toString());
            }
        }

    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a unidade de ensino"
        Width="800px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeEns" AutoPostBack="true" runat="server" Key="unidade_ens"
                        Argument="nome_comp" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                        Caption="" MaxLength="20" GridWidth="850px" SqlOrder="nome_comp" OnChanged="tseUnidadeEns_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                            
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdProgramasUnidade" runat="server" AutoGenerateColumns="False"
        Visible="false" ClientInstanceName="grdProgramasUnidade" DataSourceID="tdsProgramasUnidade"
        KeyFieldName="id_unidade_ensino_programas" Width="970px" OnCellEditorInitialize="grdProgramasUnidade_CellEditorInitialize"
        OnInitNewRow="grdProgramasUnidade_InitNewRow" OnStartRowEditing="grdProgramasUnidade_StartRowEditing"
        OnRowValidating="grdProgramasUnidade_RowValidating" OnRowInserting="grdProgramasUnidade_RowInserting"
        OnAfterPerformCallback="grdProgramasUnidade_AfterPerformCallback" OnCustomColumnDisplayText="grdProgramasUnidade_CustomColumnDisplayText">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdProgramasUnidade.AddNewRow();" alt="Novo" />
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
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID_UNIDADE_ENSINO_PROGRAMAS" FieldName="id_unidade_ensino_programas"
                VisibleIndex="3" ReadOnly="true" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="UnidadeEns" FieldName="unidade_ens" Visible="False"
                VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Agência de Fomento*" FieldName="agencia"
                Width="200px" VisibleIndex="1">
                <PropertiesComboBox DataSourceID="odsAgencia" TextField="nome_agencia" ValueField="agencia"
                    DropDownWidth="200px" ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a agência de fomento." IsRequired="True" />
                    </ValidationSettings>
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnAgenciaChanged(s);} " />
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Programa*" FieldName="programa" VisibleIndex="2"
                Width="200px">
                <PropertiesComboBox TextField="nome_programa" ValueField="programa" ClientInstanceName="cmbPrograma"
                    DropDownWidth="200px" ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o programa." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Tipo de Benefício*" FieldName="tipo_beneficio"
                Width="200px" VisibleIndex="3">
                <PropertiesComboBox ValueType="System.String" DataSourceID="odsTipoBeneficio" ValueField="tipo_beneficio"
                    Width="200px" TextField="descricao">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o tipo de benefícios." IsRequired="true" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano de Validade*" FieldName="ano_validade"
                Width="100px" VisibleIndex="4">
                <PropertiesTextEdit MaskSettings-Mask="####" Width="100px">
                    <MaskSettings Mask="####"></MaskSettings>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o ano de validade." IsRequired="True" />
                        <RegularExpression ErrorText="O campo Ano de Validade só aceita anos superiores a 1900."
                            ValidationExpression="^([1]+[9]+[0-9]+[0-9]|[2-9]+[0-9]+[0-9]+[0-9])*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data de Início*" FieldName="dt_inicio" VisibleIndex="5"
                Width="100px">
                <PropertiesDateEdit MinDate="1899-12-31" Width="100px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data de início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Final*" FieldName="dt_fim" VisibleIndex="6"
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
    <techne:TTableDataSource ID="tdsProgramasUnidade" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_unidade_ensino_programas"
        SqlWhere="Ly_unidade_ensino_programas.unidade_ens = @unidadeEns">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="tseUnidadeEns" Name="unidadeEns" PropertyName="DBValue" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsAgencia" runat="server" TypeName="Techne.Lyceum.RN.ProgramasUnidade"
        SelectMethod="ConsultarAgencias"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoBeneficio" runat="server" TypeName="Techne.Lyceum.RN.ProgramasUnidade"
        SelectMethod="ConsultarTipoBeneficio"></asp:ObjectDataSource>
</asp:Content>
