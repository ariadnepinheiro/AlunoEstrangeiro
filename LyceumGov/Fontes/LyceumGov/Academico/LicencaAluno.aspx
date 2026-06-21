<%@ Page Language="C#" CodeBehind="LicencaAluno.aspx.cs" MasterPageFile="~/Modulos/LyceumMaster.Master"
    AutoEventWireup="true" Inherits="Techne.Lyceum.Net.Academico.LicencaAluno" %>

<asp:Content ID="conLotacaoFuncionario" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnDisciplinaChanged(cmbDisciplina) {
            if (typeof cmbDisciplina != 'undefined' && cmbDisciplina != null) {
                if (typeof cmbDisciplina.GetValue() != 'undefined' && cmbDisciplina.GetValue() != null)
                    grdLicenca.GetEditor("turma").PerformCallback(cmbDisciplina.GetValue().toString());
            }
        }

    </script>
<asp:HiddenField ID="hdnDataInicio" runat="server" />
<asp:HiddenField ID="hdnDataFim" runat="server" />
        <asp:HiddenField ID="hdnDataMatricula" runat="server" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                        <QueryParameters>
                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagemFixa" runat="server" Visible="false" SkinID="lblMensagem"
        Text="Para casos de atendimento domiciliar ou hospitalar (AEDH), favor utilizar a NOVA 'ABA AEDH - ESCOLARIZAÇÃO EM OUTROS ESPAÇOS', no cadastro do aluno."></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsLicenca" TypeName="Techne.Lyceum.Net.Academico.LicencaAluno"
        runat="server" SelectMethod="Lista" DeleteMethod="Delete" UpdateMethod="Update"
        OnInserting="odsLicenca_Inserting" InsertMethod="Insert" OnUpdating="odsLicenca_Updating"
        OnDeleting="odsLicenca_Deleting">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="tseAluno" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsDisciplinas" runat="server" SelectMethod="ConsultarDisciplinas"
        TypeName="Techne.Lyceum.RN.Disciplina">
        <SelectParameters>
            <asp:Parameter Name="aluno" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMatricula" runat="server" SelectMethod="ConsultarMatricula"
        TypeName="Techne.Lyceum.RN.Matricula">
        <SelectParameters>
            <asp:Parameter Name="aluno" Type="String" />
            <asp:Parameter Name="disciplina" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAnoSemestre" runat="server" SelectMethod="ConsultarAnoSemestre"
        TypeName="Techne.Lyceum.RN.Matricula">
        <SelectParameters>
            <asp:Parameter Name="aluno" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTurma" runat="server" SelectMethod="ConsultarTurma"
        TypeName="Techne.Lyceum.RN.Matricula">
        <SelectParameters>
            <asp:Parameter Name="aluno" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMotivo" runat="server" SelectMethod="ListaAtivoPor"
        TypeName="Techne.Lyceum.RN.RecursosHumanos.JustificativaFalta"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdLicenca" runat="server" ClientInstanceName="grdLicenca"
        Visible="false" AutoGenerateColumns="False" DataSourceID="odsLicenca" KeyFieldName="id_aluno_licenca"
        Width="85%" OnCellEditorInitialize="grdLicenca_CellEditorInitialize" OnInitNewRow="grdLicenca_InitNewRow"
        OnStartRowEditing="grdLicenca_StartRowEditing" OnAfterPerformCallback="grdLicenca_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="4%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdLicenca.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn FieldName="id_aluno_licenca" VisibleIndex="1" Caption="ID"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="aluno" VisibleIndex="2" Caption="Aluno"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Componente Curricular*" HeaderStyle-Font-Bold="true"
                FieldName="disciplina" VisibleIndex="3" Width="150px">
                <PropertiesComboBox DataSourceID="odsDisciplinas" TextField="nome" ValueField="disciplina"
                    ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a disciplina." IsRequired="True" />
                    </ValidationSettings>
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnDisciplinaChanged(s);} " />
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Turma*" HeaderStyle-Font-Bold="true" FieldName="turma"
                VisibleIndex="4" Width="120px">
                <PropertiesComboBox DataSourceID="odsTurma" TextField="turma" ValueField="turma"
                    ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a turma." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano Letivo*" HeaderStyle-Font-Bold="true"
                FieldName="ano" VisibleIndex="5" Width="110px">
                <PropertiesComboBox DataSourceID="odsAnoSemestre" TextField="ano" ValueField="ano"
                    ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o ano letivo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Período Letivo*" HeaderStyle-Font-Bold="true"
                FieldName="semestre" VisibleIndex="6" Width="100px">
                <PropertiesComboBox DataSourceID="odsAnoSemestre" TextField="semestre" ValueField="semestre"
                    ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o período letivo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Motivo da Justificativa*" HeaderStyle-Font-Bold="true"
                FieldName="justificativafaltaid" VisibleIndex="7" Width="100px">
                <PropertiesComboBox DataSourceID="odsMotivo" TextField="descricao" ValueField="justificativafaltaid"
                    ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o motivo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn FieldName="observacao" VisibleIndex="8" Caption="Observação"
                HeaderStyle-Font-Bold="true" Width="300px">
                <PropertiesTextEdit MaxLength="2000">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn FieldName="dt_inicio" VisibleIndex="9" Caption="Data Início*"
                HeaderStyle-Font-Bold="true" Width="110px">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data de início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn FieldName="dt_fim" VisibleIndex="10" Caption="Data Fim*"
                HeaderStyle-Font-Bold="true" Width="110px">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy"
                    EditFormat="Date">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data de fim." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
