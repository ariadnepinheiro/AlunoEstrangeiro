<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarOcorrencias.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ListarOcorrencias" %>

<asp:Content ID="conListarOcorrencias" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function Novo() {
            if (typeof grdOcorrencias != 'undefined' && grdOcorrencias != null) {
                var tSearch = document.getElementById("<%=tseAluno.ClientID %>");
                if (typeof tSearch != 'undefined' && tSearch != null) {
                    if (typeof tSearch.value != 'undefined' && tSearch.value != null && tSearch.value != '')
                        grdOcorrencias.AddNewRow();
                }
            }
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" 
                        SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno" AutoPostBack="true"
                        OnTextChanged="tseAluno_Changed">
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
    <techne:TTableDataSource ID="tdsDisciplina" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_matricula"
        SqlWhere="aluno = @tseAluno">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="tseAluno" DefaultValue="null" Name="tseAluno" PropertyName="DBValue" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsTurma" runat="server" SelectMethod="ConsultarTurma"
        TypeName="Techne.Lyceum.RN.Ocorrencia">
        <SelectParameters>
            <asp:Parameter Name="aluno" Type="String" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsTipo" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
        SqlColumns="ITEM, DESCR" SqlWhere="TAB = 'TIPO OCORRENCIA'" SqlOrder="DESCR">
    </techne:TTableDataSource>
    <div>
        <asp:Panel ID="pnDadosAcademicos" runat="server" GroupingText="Dados Escolares" Width="500px"
            Height="150px">
            <table id="tbAluno" runat="server">
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblCurso" runat="server" Text="Escolaridade: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCurso" Width="350" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblTurno" runat="server" Text="Turno: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTurno" Width="350" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblSerie" runat="server" Text="Ano Escolar: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSerie" Width="350" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeEnsino" runat="server" Text="Unidade Ensino: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnidadeEnsino" Width="350" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeFisica" runat="server" Text="Unidade Física: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnidadeFisica" Width="350" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
    <div>
            <dxwgv:ASPxGridView ID="grdOcorrencias" runat="server" AutoGenerateColumns="False"
                SkinID="NoConfirmDelete" ClientInstanceName="grdOcorrencias" OnRowDeleting="grdOcorrencias_RowDeleting"
                KeyFieldName="OcorrenciaKey" OnCustomUnboundColumnData="grdOcorrencias_CustomUnboundColumnData"
                OnAfterPerformCallback="grdOcorrencias_AfterPerformCallback" OnSelectionChanged="grdOcorrencias_SelectionChanged"
                OnInitNewRow="grdOcorrencias_InitNewRow" OnStartRowEditing="grdOcorrencias_StartRowEditing">
                <SettingsBehavior ProcessSelectionChangedOnServer="True" />
                <SettingsEditing Mode="Inline" />
                <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
                <Columns>
                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdOcorrencias.AddNewRow();" alt="Novo" />
                            </div>
                        </HeaderCaptionTemplate>
                        <EditButton Text="Editar" Visible="True">
                            <Image Url="~/img/bt_editar.png" />
                        </EditButton>
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                        <SelectButton Text="Selecionar" Visible="True">
                            <Image Url="~/img/bt_busca.png" />
                        </SelectButton>
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
                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="aluno" VisibleIndex="1"
                        Visible="False">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Tipo" FieldName="tipo" VisibleIndex="1"
                        Width="400px">
                        <PropertiesComboBox ValueType="System.String" DataSourceID="tdsTipo" TextField="DESCR"
                            ValueField="ITEM" Width="400px">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="data" VisibleIndex="2" Width="200px">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ordem" VisibleIndex="3"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="usuario" VisibleIndex="4"
                        Width="150px">
                        <PropertiesTextEdit MaxLength="20" Width="150px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="5"
                        Width="400px">
                        <PropertiesTextEdit MaxLength="5000" Width="400px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Ano Letivo" FieldName="ano" VisibleIndex="6"
                        Width="80px">
                        <PropertiesTextEdit MaxLength="4" Width="80px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Período Letivo" FieldName="periodo" VisibleIndex="7"
                        Width="50px">
                        <PropertiesTextEdit MaxLength="2" Width="50px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Disciplina" FieldName="disciplina" VisibleIndex="8"
                        Width="400px">
                        <PropertiesComboBox ValueType="System.String" DataSourceID="tdsDisciplina" TextField="nomedisciplina"
                            ValueField="disciplina" Width="400px">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Turma" FieldName="turma" VisibleIndex="9"
                        Width="500px">
                        <PropertiesComboBox ValueType="System.String" DataSourceID="odsTurma" TextField="turma"
                            ValueField="turma" Width="500px">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Código Docente" FieldName="num_func" VisibleIndex="10"
                        Visible="False">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Docente" FieldName="nome_compl" VisibleIndex="11"
                        Width="400px">
                        <PropertiesTextEdit Width="400px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="OcorrenciaKey" FieldName="OcorrenciaKey" UnboundType="String"
                        Visible="False" VisibleIndex="11">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
    </div>
</asp:Content>
