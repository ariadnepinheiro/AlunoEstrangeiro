<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AgendaConfTurnosVagas.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AgendaConfTurnosVagas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OngrdAgendaEndCallBack() {

            if (typeof (grdAgenda) != 'undefined' && grdAgenda != null) {
                var valor = 'grade';

                grdAgenda.PerformCallback(valor);
            }
        }
        function OnCursoChanged(cmbCurso) {

            grdAgenda.GetEditor("SERIE").PerformCallback(cmbCurso.GetValue().toString());
        }

        function OnAnoPeriodoChanged(cmbAnoPeriodo) {

            grdAgenda.GetEditor("anoperiodoreferencia").PerformCallback(cmbAnoPeriodo.GetValue().toString());
        }
        function OnEscolaridadeChanged() {
       
            var descricao = cmbCurso.GetText().toString().split('|');

            txtNivel.SetText(descricao[2]);
            txtModalidade.SetText(descricao[1]);
            txtCodigo.SetText(cmbCurso.GetValue().toString());

            grdAgenda.GetEditor("SERIE").PerformCallback(cmbCurso.GetValue().toString());
        }
       
    </script>

    <style type="text/css">
        .dxeListBox_Office2003_Blue .dxeHD
        {
            font-weight: bold;
        }
        .dxeListBox_Blue .dxeHD
        {
            font-weight: bold;
        }
    </style>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxwgv:ASPxGridView ID="grdAgenda" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdAgenda"
        Width="1200px" DataSourceID="odsAgenda" KeyFieldName="ID_AGENDA_CONF_TURNO_VAGA"
        OnCellEditorInitialize="grdAgenda_CellEditorInitialize" OnInitNewRow="grdAgenda_InitNewRow"
        OnStartRowEditing="grdAgenda_StartRowEditing" OnAfterPerformCallback="grdAgenda_AfterPerformCallback"
        OnHtmlRowCreated="grdAgenda_HtmlRowCreated" OnCommandButtonInitialize="grdAgenda_CommandButtonInitialize">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados" />
        <%--        <ClientSideEvents EndCallback="function(s, e) { OngrdAgendaEndCallBack(); }" />--%>
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdAgenda.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="true">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="true">
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
            <dxwgv:GridViewDataTextColumn FieldName="ID_AGENDA_CONF_TURNO_VAGA" ReadOnly="true"
                VisibleIndex="1" Caption="ID*" HeaderStyle-Font-Bold="true" Width="50px" Visible="false">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano Letivo*" HeaderStyle-Font-Bold="true"
                FieldName="anoperiodo" VisibleIndex="1" Width="80px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsAnoPeriodo" TextField="anoperiodo" ValueField="anoperiodo"
                    ValueType="System.String" ClientInstanceName="anoperiodo" DropDownWidth="120px"
                    Width="120px" MaxLength="4" EnableSynchronization="False" EnableIncrementalFiltering="True">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o ano letivo." IsRequired="True" />
                    </ValidationSettings>
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnAnoPeriodoChanged(s); }">
                    </ClientSideEvents>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <%--            <dxwgv:GridViewDataComboBoxColumn Caption="Escolaridade*" FieldName="CURSO" Name="curso"
                VisibleIndex="2" Width="200px" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsCurso" TextField="NOME" ValueField="curso" ValueType="System.String"
                    EnableSynchronization="False" EnableIncrementalFiltering="True" ClientInstanceName="CURSO">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Escolaridade." IsRequired="True" />
                    </ValidationSettings>
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCursoChanged(s); }"></ClientSideEvents>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>--%>
            <dxwgv:GridViewDataTextColumn Caption="Modalidade*" FieldName="modalidade" VisibleIndex="2"
                Width="110px" ReadOnly="true">
                <PropertiesTextEdit ClientInstanceName="txtModalidade">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Segmento*" FieldName="nivel" VisibleIndex="3"
                Width="150px" ReadOnly="true">
                <PropertiesTextEdit ClientInstanceName="txtNivel">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CURSO" ReadOnly="True"
                VisibleIndex="4">
                <PropertiesTextEdit ClientInstanceName="txtCodigo">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Curso*" HeaderStyle-Font-Bold="true" FieldName="NOME"
                VisibleIndex="5" Width="300px">
                <PropertiesComboBox DataSourceID="odsCurso" EnableDefaultAppearance="false" ListBoxStyle-CssClass="dxeListBox"
                    CssPostfix="Office2003_Blue" ClientInstanceName="cmbCurso" TextFormatString="{1}|{2}|{3}"
                    ValueField="curso" ValueType="System.String" DropDownWidth="800px" Width="350px"
                    MaxLength="300">
                    <Columns>
                        <dxe:ListBoxColumn Caption="Código" FieldName="curso" Name="curso" Width="100px" />
                        <dxe:ListBoxColumn Caption="Escolaridade" FieldName="nome" Name="nome" Width="270px" />
                        <dxe:ListBoxColumn Caption="Modalidade" FieldName="modalidade" Name="modalidade"
                            Width="260px" />
                        <dxe:ListBoxColumn Caption="Nível" FieldName="nivel" Name="nivel" Width="190px" />
                    </Columns>
                    <ClientSideEvents SelectedIndexChanged="function(s,e){ OnEscolaridadeChanged(); }" />
                    <ListBoxStyle CssClass="dxeListBox">
                    </ListBoxStyle>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="Favor escolher uma escolaridade." />
                    </ValidationSettings>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="6" Caption="Série *" FieldName="SERIE"
                Width="100px">
                <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                    ClientInstanceName="SERIE">
                </PropertiesComboBox>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano/Período de Referência*" HeaderStyle-Font-Bold="true"
                FieldName="anoperiodoreferencia" VisibleIndex="7" Width="120px">
                <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                    ClientInstanceName="anoperiodoreferencia">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Início Confirmação Turno*" FieldName="DT_INICIO_CONF_TURNO"
                VisibleIndex="8" Width="110px">
                <PropertiesDateEdit Width="110px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar a Data Início da Confirmação Turno." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Final Confirmação Turno*" FieldName="DT_FIM_CONF_TURNO"
                VisibleIndex="9" Width="110px">
                <PropertiesDateEdit Width="110px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar a Data Final da Confirmação Turno." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Início Confirmação Vagas*" FieldName="DT_INICIO_CONF_VAGAS"
                VisibleIndex="10" Width="110px">
                <PropertiesDateEdit Width="110px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar a Data Início da Confirmação Vagas." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Final Confirmação Vagas*" FieldName="DT_FIM_CONF_VAGAS"
                VisibleIndex="11" Width="110px">
                <PropertiesDateEdit Width="110px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar a Data Final da Confirmação Vagas." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Perfil" FieldName="PERFIL_RESPONSAVEL" Name="PERFIL_RESPONSAVEL"
                UnboundType="String" Visible="false">
            </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" Name="SITUACAO"
                UnboundType="String" >
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsAnoPeriodo" runat="server" SelectMethod="ConsultarAnoPeriodo"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsCurso" TypeName="Techne.Lyceum.Net.Basico.AgendaConfTurnosVagas"
        SelectMethod="ListarCurso" runat="server"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAgenda" TypeName="Techne.Lyceum.Net.Basico.AgendaConfTurnosVagas"
        runat="server" SelectMethod="Listar" OnDeleting="odsAgenda_Deleting" DeleteMethod="Delete"
        OnUpdating="odsAgenda_Updating" UpdateMethod="Update" OnInserting="odsAgenda_Inserting"
        InsertMethod="Insert"></asp:ObjectDataSource>
</asp:Content>
