<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LotacaoDocente.aspx.cs"
    Inherits="Techne.Lyceum.Net.Basico.LotacaoDocente" MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="conHeader" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Scripts/Themes/jquery-ui.css" />
</asp:Content>
<asp:Content ID="conLotacao" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script type="text/javascript">
        function OnSituacaoChanged(source) {
            if (typeof (cmbSituacaoLic) != 'undefined' && cmbSituacaoLic != null &&
                typeof (datafimLic) != 'undefined' && datafimLic != null) {
                var motivo = cmbSituacaoLic.GetValue();
                if (motivo != null && motivo != "") {
                    $.post("LotacaoDocente.aspx", { Motivo: motivo }, function(possui_dtfim) {
                        if (possui_dtfim == "S") {
                            datafimLic.SetEnabled(true);
                        } else if (possui_dtfim == "N") {
                            datafimLic.SetText("");
                            datafimLic.SetEnabled(false);
                        }
                    });
                }
            }

        }

        function OnMatriculaLicChanged() {
            var descricao = ddlMatriculaLic.GetText().toString().split('|');
            txtIdFuncional.SetText(descricao[1]);
            txtVinculo.SetText(descricao[2]);
        }


        function OnInit(dataInicio) {
            alert(dataInicio);
            if (typeof (dataInicio) != 'undefined' && dataInicio != null) {
                dataInicio.SetEnabled(false);
            }
        }
        $(function() {
            $("input[id*='dtini']").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
            });
            $("input[id*='dtfim']").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
            });
        });
        function abrirPopup() {
            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }

        function abrirPopupDesalocacao() {
            window.setTimeout(function() {
                pucConfirmarDesalocacao.Show();
            }, 1000);
        }

        function abrirPopupCPOE() {
            window.setTimeout(function() {
                pucConfirmarCPOE.Show();
            }, 1000);
        }

    </script>

    <asp:ObjectDataSource ID="odsLotacao" TypeName="Techne.Lyceum.Net.Basico.LotacaoDocente"
        runat="server" SelectMethod="Listar" DeleteMethod="Delete" UpdateMethod="Update"
        InsertMethod="Insert">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtPessoaHidden" Name="pessoa" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsFuncao" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_funcao">
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsMatricula" TypeName="Techne.Lyceum.Net.Basico.LotacaoDocente"
        runat="server" SelectMethod="ListaMatricula">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtPessoaHidden" Name="pessoa" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsSituacao" TypeName="Techne.Lyceum.RN.Licencas" SelectMethod="PreencherComboLicenca"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtUsuarioHidden" Name="usuario" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:TextBox ID="txtUsuarioHidden" runat="server" Visible="false" Text=" " />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o Id/Vínculo ou o nome do docente"
        Width="700px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="Id/Vínculo do Docente:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocente"
                        AutoPostBack="true" OnTextChanged="tseDocente_Changed" MaxLength="20">
                    </tweb:TSearch>
                    <asp:TextBox ID="txtPessoaHidden" Visible="false" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:HiddenField ID="hdnPadraoQHI" runat="server" />
    <asp:HiddenField ID="hdnPadraoCOCAC" runat="server" />
    <dxwgv:ASPxGridView ID="grdLotacao" runat="server" EnableCallBacks="false" ClientInstanceName="grdLotacao"
        AutoGenerateColumns="False" DataSourceID="odsLotacao" KeyFieldName="PESSOA;MATRICULA;ORDEM"
        OnInitNewRow="grdLotacao_InitNewRow" OnRowDeleting="grdLotacao_RowDeleting" OnRowUpdating="grdLotacao_RowUpdating"
        OnCellEditorInitialize="grdLotacao_CellEditorInitialize" OnRowInserting="grdLotacao_RowInserting"
        OnStartRowEditing="grdLotacao_StartRowEditing" OnAfterPerformCallback="grdLotacao_AfterPerformCallback"
        Width="1500px" Visible="false" OnHtmlRowCreated="grdLotacao_HtmlRowCreated" OnCancelRowEditing="grdLotacao_CancelRowEditing"
        OnCommandButtonInitialize="grdLotacao_CommandButtonInitialize">
        <SettingsEditing Mode="EditForm" />
        <Templates>
            <EditForm>
                <div style="padding: 4px 4px 3px 4px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMatricula" runat="server" Text="ID/Vínculo ou Matrícula:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cmbMatricula" Width="110px" ValueField="MATRICULA" TextField="IDVINCULO"
                                    DataSourceID="odsMatricula" runat="server" Value='<%# Bind("MATRICULA") %>' ClientInstanceName="cmbMatricula"
                                    AutoPostBack="true" OnSelectedIndexChanged="cmbMatricula_SelectedIndexChanged"
                                    EnableIncrementalFiltering="true">
                                    <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFuncao" runat="server" Text="Função:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <tweb:TSearchBox ID="tseFuncao" runat="server" Argument="descricao" ArgumentColumns="70"
                                    FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                    DataType="VarChar" Key="funcao" Value='<%# Bind("FUNCAO") %>' SqlOrder="descricao"
                                    SqlSelect="SELECT funcao, descricao FROM Ly_funcao">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSetor" runat="server" Text="Unidade Administrativa:*" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseSetor" runat="server" SqlSelect="SELECT ua_atual,nomesetor,ua_antiga,setor FROM hades..vw_setor"
                                    AutoPostBack="false" SqlOrder="ua_atual" ColumnName="ua_atual" Caption="" Key="ua_atual"
                                    FollowContainerMode="false" Connection="Hades" MaxLength="15" DataType="Varchar"
                                    Value='<%# Bind("ua_atual") %>'>
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomesetor" Width="80%" />
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataNomeacao" runat="server" Text="Data da Nomeação:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="DATA_NOMEACAO" Value='<%# Bind("DATA_NOMEACAO") %>'
                                    Width="110px">
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <asp:Label ID="lblDataNomeacaoDO" runat="server" Text="Data da Publicação da Nomeação: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATA_NOMEACAO_DO" ID="ASPxGridViewTemplateReplacement6"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataDesativacao" runat="server" Text="Data da Dispensa: "></asp:Label>
                            </td>
                            <td style="width: 80px">
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATA_DESATIVACAO" ID="ASPxGridViewTemplateReplacement8"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataDesativacaoDO" runat="server" Text="Data da Publicação da Dispensa: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATA_DESATIVACAO_DO" ID="ASPxGridViewTemplateReplacement7"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAtoOficial" runat="server" Text="Ato Oficial: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="ATO_OFICIAL" ID="ASPxGridViewTemplateReplacement9"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <dxe:ASPxCheckBox ID="chkReadaptado" runat="server" Value='<%# Bind("READAPTADO") %>'
                                    Text="Readaptado" ValueType="System.String" ValueChecked="Sim" ValueUnchecked="Não"
                                    ClientEnabled="false">
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDocumentacao" runat="server" Text="Documentação: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="RESP_DOCUMENTACAO" ID="ASPxGridViewTemplateReplacement10"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMotivo" runat="server" Text="Situação: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="MOTIVO" ID="ASPxGridViewTemplateReplacement11"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataInicio" runat="server" Text="Data Início Situação: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DTINI" ID="ASPxGridViewTemplateReplacement3"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataFim" runat="server" Text="Data Fim Situação: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DTFIM" ID="ASPxGridViewTemplateReplacement4"
                                    ReplacementType="EditFormCellEditor" runat="server">
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
                </div>
            </EditForm>
        </Templates>
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" alt="Novo" src="../img/bt_novo.png" onclick="grdLotacao.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="PESSOA" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Num_func" FieldName="NUM_FUNC" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ORDEM" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade Física" FieldName="unidade_fis" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Id Funcional" FieldName="IDFUNCIONAL" VisibleIndex="1"
                Width="100px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Vínculo" FieldName="VINCULO" VisibleIndex="2"
                Width="100px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Matrícula ou ID/Vínculo" FieldName="MATRICULA"
                VisibleIndex="3" Width="110px">
                <PropertiesComboBox DataSourceID="odsMatricula" TextField="MATRICULA" ValueField="MATRICULA"
                    Width="110px" ValueType="System.String" DropDownWidth="110px">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="FUNCAO" VisibleIndex="6"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cargo" FieldName="CARGO" VisibleIndex="7"
                Width="200px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="DESCRICAO" VisibleIndex="7"
                Width="200px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="U.A. Antiga" FieldName="SETOR" Width="30px"
                VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="U.A." FieldName="UA_ATUAL" Width="30px" VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID_Regional" FieldName="ID_REGIONAL" VisibleIndex="9"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="DESCRICAO_REGIONAL" VisibleIndex="10"
                Width="100" Visible="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENS"
                VisibleIndex="11" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="12"
                Width="150px" Visible="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina de Ingresso" FieldName="Disciplina_Ingresso"
                VisibleIndex="11" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina de Ingresso" FieldName="DISCIPLINA_INGRESSO"
                VisibleIndex="12" Width="150px" Visible="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Nomeação" FieldName="DATA_NOMEACAO"
                VisibleIndex="13" Width="110px">
                <PropertiesDateEdit Width="110px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn FieldName="DATA_NOMEACAO_DO" Caption="Data da Publicação da Nomeação"
                VisibleIndex="14" Width="110px">
                <PropertiesDateEdit Width="110px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Dispensa" FieldName="DATA_DESATIVACAO"
                VisibleIndex="15" Width="110px">
                <PropertiesDateEdit Width="110px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Publicação da Dispensa" FieldName="DATA_DESATIVACAO_DO"
                VisibleIndex="16" Width="110px">
                <PropertiesDateEdit Width="110px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Documentação" VisibleIndex="17" FieldName="RESP_DOCUMENTACAO"
                Width="70px">
                <PropertiesComboBox ValueType="System.String" Width="110px">
                    <Items>
                        <dxe:ListEditItem Text="Sim" Value="S" />
                        <dxe:ListEditItem Text="Não" Value="N" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ato Oficial" FieldName="ATO_OFICIAL" VisibleIndex="18"
                Width="100px">
                <PropertiesTextEdit MaxLength="100" Native="True" Width="100px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Readaptado" FieldName="READAPTADO" VisibleIndex="19">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Inicio Readaptação" FieldName="DT_INICIO_READAPTACAO"
                VisibleIndex="11" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Inicio Readaptação" FieldName="DT_INICIO_READAPTACAO"
                VisibleIndex="19" Width="150px" Visible="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Fim Readaptação" FieldName="DT_FIM_READAPTACAO"
                VisibleIndex="11" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Fim Readaptação" FieldName="DT_FIM_READAPTACAO"
                VisibleIndex="19" Width="150px" Visible="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Situação" FieldName="MOTIVO" VisibleIndex="20"
                Width="300px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="tdsMotivos" TextField="descricao"
                    ClientInstanceName="cmbSituacao" ValueField="motivo" Width="300px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnSituacaoChanged(cmbSituacao,datafim);}" />
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Situação" FieldName="DTINI" VisibleIndex="21"
                Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="datainicio">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Situação" FieldName="DTFIM" VisibleIndex="22"
                Width="110px">
                <PropertiesDateEdit Width="110px" ClientInstanceName="datafim">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            
            <dxwgv:GridViewDataTextColumn Caption="Redução de CH" FieldName="REDUCAOCH" VisibleIndex="23"
                Width="110px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Início Red de CH" FieldName="DTINICH" VisibleIndex="24"
                Width="110px">
                <PropertiesDateEdit Width="110px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Red de CH" FieldName="DTFIMCH" VisibleIndex="25"
                Width="110px">
                <PropertiesDateEdit Width="110px">
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>

            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIO" VisibleIndex="26">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Última Atualização" FieldName="DATA_ATUALIZACAO"
                VisibleIndex="26" Width="100px">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="PODE_REMOVER" FieldName="PODE_REMOVER" VisibleIndex="27"
                Width="100px" Visible="false">
            </dxwgv:GridViewDataDateColumn>
             <dxwgv:GridViewDataDateColumn Caption="TIPOFUNCAO" FieldName="TIPOFUNCAO" VisibleIndex="29"
                Width="100px" Visible="false">
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsLicenca" TypeName="Techne.Lyceum.Net.Basico.LotacaoDocente"
        runat="server" SelectMethod="ListarLic" DeleteMethod="DeleteLic" UpdateMethod="UpdateLic"
        InsertMethod="InsertLic">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtPessoaHidden" Name="pessoa" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsMotivos" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_licencas"
        SqlOrder="descricao">
    </techne:TTableDataSource>
    <asp:Label ID="lblMensagemLicenca" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxwgv:ASPxGridView ID="grdLicencas" runat="server" AutoGenerateColumns="False" Visible="false"
        ClientInstanceName="grdLicencas" DataSourceID="odsLicenca" KeyFieldName="NUM_FUNC;DTINI"
        EnableCallBacks="false" OnCellEditorInitialize="grdLicencas_CellEditorInitialize"
        OnRowDeleting="grdLicencas_RowDeleting" OnRowInserting="grdLicencas_RowInserting"
        OnRowUpdating="grdLicencas_RowUpdating" OnCancelRowEditing="grdLicencas_CancelRowEditing">
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdLicencas.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="Num_func" FieldName="NUM_FUNC" VisibleIndex="0"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Id Funcional" FieldName="IDFUNCIONAL" VisibleIndex="1"
                ReadOnly="true">
                <PropertiesTextEdit ClientInstanceName="txtIdFuncional">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Vínculo" FieldName="VINCULO" VisibleIndex="2"
                Name="VINCULO" ReadOnly="true">
                <PropertiesTextEdit ClientInstanceName="txtVinculo">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Matrícula*" HeaderStyle-Font-Bold="true"
                FieldName="MATRICULA" VisibleIndex="3" Width="40px">
                <PropertiesComboBox DataSourceID="odsMatricula" EnableDefaultAppearance="false" ListBoxStyle-CssClass="dxeListBox"
                    CssPostfix="Office2003_Blue" ClientInstanceName="ddlMatriculaLic" TextFormatString="{0}|{1}|{2}"
                    ValueField="MATRICULADESC" ValueType="System.String" DropDownWidth="800px" Width="350px"
                    MaxLength="300">
                    <Columns>
                        <dxe:ListBoxColumn Caption="Matrícula ou ID/Vínculo" FieldName="MATRICULA" Name="MATRICULA"
                            Width="260px" />
                        <dxe:ListBoxColumn Caption="Id Funcional" FieldName="IDFUNCIONAL" Name="IDFUNCIONAL"
                            Width="100px" />
                        <dxe:ListBoxColumn Caption="Vínculo" FieldName="VINCULO" Name="VINCULO" Width="270px" />
                    </Columns>
                    <ClientSideEvents SelectedIndexChanged="function(s,e){ OnMatriculaLicChanged(); }" />
                    <ListBoxStyle CssClass="dxeListBox">
                    </ListBoxStyle>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="Favor escolher uma MATRICULA." />
                    </ValidationSettings>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Situação" FieldName="MOTIVO" VisibleIndex="4"
                Width="300px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="tdsMotivos" TextField="descricao"
                    ClientInstanceName="cmbSituacaoLic" ValueField="motivo" Width="200px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnSituacaoChanged();}" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar uma situação." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Início Situação" FieldName="DTINI" VisibleIndex="5"
                Width="110px">
                <EditItemTemplate>
                    <dxe:ASPxTextBox ID="dtini" Width="120px" Text='<%#Bind("DTINI") %>' runat="server"
                        MaxLength="10" onkeyup="formataData(this,event)" OnLoad="dtIni_Load">
                    </dxe:ASPxTextBox>
                </EditItemTemplate>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim Situação" FieldName="DTFIM" VisibleIndex="6"
                Width="110px">
                <EditItemTemplate>
                    <dxe:ASPxTextBox ID="dtfim" Width="120px" Text='<%#Bind("DTFIM") %>' ClientInstanceName="datafimLic"
                        runat="server" MaxLength="10" onkeyup="formataData(this,event)">
                    </dxe:ASPxTextBox>
                </EditItemTemplate>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="false" />
    </dxwgv:ASPxGridView>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" EnableAnimation="false"
        Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Deseja terminar a lotação atual e inserir esta nova?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarDesalocacao" ClientInstanceName="pucConfirmarDesalocacao"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            O docente possui aulas alocadas após a data de desativação. Confirma a desalocação
                            das aulas?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSimDesalocacao" runat="server" Text="Sim" OnClick="btnSimDesalocacao_Click" />
                            <asp:Button ID="btnNaoDesalocacao" runat="server" Text="Não" OnClick="btnNaoDesalocacao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarCPOE" ClientInstanceName="pucConfirmarCPOE"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            O professor possui GLP alocada. Deseja efetuar a desalocação?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="Button1" runat="server" Text="Sim" OnClick="btnSimCPOE_Click" />
                            <asp:Button ID="Button2" runat="server" Text="Não" OnClick="btnNaoCPOE_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
