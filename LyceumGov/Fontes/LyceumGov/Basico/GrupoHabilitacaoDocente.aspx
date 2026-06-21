<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="GrupoHabilitacaoDocente.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.GrupoHabilitacaoDocente" %>

<asp:Content ID="conPrincipal" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }

        function Novo() {
            if (typeof grdDiscHabilit != 'undefined' && grdDiscHabilit != null) {
                var tSearch = document.getElementById("<%=tseDocentes.ClientID %>");
                if (typeof tSearch != 'undefined' && tSearch != null) {
                    if (typeof tSearch.value != 'undefined' && tSearch.value != null && tSearch.value != '')
                        grdDiscHabilit.AddNewRow();
                }
            }
        }

        function Novo2() {
            if (typeof grdDiscHabilitProv != 'undefined' && grdDiscHabilitProv != null) {
                var tSearch = document.getElementById("<%=tseDocentes.ClientID %>");
                if (typeof tSearch != 'undefined' && tSearch != null) {
                    if (typeof tSearch.value != 'undefined' && tSearch.value != null && tSearch.value != '')
                        grdDiscHabilitProv.AddNewRow();
                }
            }
        }

        function controlarObservacao(comboBox, limparTexto) {
            var txtJustificativa = $("#" + $(comboBox).attr("txtJustificativa"));
            var observacao = $(comboBox).val();

            alert(observacao);

            if (observacao == "TAD COM PROCESSO") {
                $(txtJustificativa).removeAttr("readonly");
                $(txtJustificativa).removeAttr("disabled");
                $(txtJustificativa).css("background-color", "");

                if (limparTexto) {
                    $(txtJustificativa).val("");
                }
            }
            else {
                $(txtJustificativa).attr("readonly", "readonly");
                $(txtJustificativa).attr("disabled", true);
                $(txtJustificativa).css("background-color", "Gainsboro");

                if (limparTexto) {
                    $(txtJustificativa).val("");
                }
            }
        }

        function controlarRecusa(radioButton, limparTexto) {
            var cmbObservacao = $("#" + $(radioButton).attr("cmbObservacao"));
            var txtJustificativa = $("#" + $(radioButton).attr("txtJustificativa"));

            $(txtJustificativa).attr("readonly", "readonly");
            $(txtJustificativa).attr("disabled", true);
            $(txtJustificativa).css("background-color", "Gainsboro");

            if (limparTexto) {
                $(txtJustificativa).val("");

                $(cmbObservacao).val("Selecione");
            }

            $(cmbObservacao).removeAttr("disabled");
        }

        function atualizarGrid() {

            $("select[id*='cmbObservacao']").each(function() {
                controlarObservacao(this, false);
            });
        }

        $(document).ready(function() {


            $("select[id*='cmbObservacao']").change(function() {
                controlarObservacao(this, true);
            });
        });


        function OnObservacaoChanged(cmbObservacao) {
            controlarObservacao(this, false);
            //grdSeries.GetEditor("serie_seguinte").PerformCallback(cmbCurso.GetValue().toString());
        }

        function OnAnoSerieConcluinteChanged() {
            if (typeof (chkAnoSerieConcluinte) != 'undefined' && chkAnoSerieConcluinte != null) {
                if (typeof chkAnoSerieConcluinte.GetValue() != 'undefined' && chkAnoSerieConcluinte.GetValue() != null) {
                    if (chkAnoSerieConcluinte.GetValue().toString() == "S") {
                        curso_seguinte.SetEnabled(false);
                        serie_seguinte.SetEnabled(false);
                        curso_seguinte.SetText('');
                        serie_seguinte.ClearItems();

                    }
                    else if (chkAnoSerieConcluinte.GetValue().toString() == "N") {
                        curso_seguinte.SetEnabled(true);
                        serie_seguinte.SetEnabled(true);
                    }
                }
                else {
                    curso_seguinte.SetEnabled(true);
                    serie_seguinte.SetEnabled(true);
                }
            }
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a ID/Vínculo ou o nome do docente"
        Width="680px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="ID/Vínculo do Docente:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocentes" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocente_Voluntario"
                        AutoPostBack="true" OnTextChanged="tseDocentes_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="hdnNumFunc" runat="server" />
    <asp:ObjectDataSource ID="odsDiscHabilitadas" TypeName="Techne.Lyceum.Net.Basico.GrupoHabilitacaoDocente"
        SelectMethod="ObterGruposDeHabilitacaoDocente" InsertMethod="InserirGrupoDeHabilitacaoDocente"
        DeleteMethod="RemoverGrupoDeHabilitacaoDocente" UpdateMethod="AtualizarGrupoDeHabilitacaoDocente"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnNumFunc" PropertyName="Value" DbType="String"
                Name="num_func" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsDiscHabilitadasProv" TypeName="Techne.Lyceum.Net.Basico.GrupoHabilitacaoDocente"
        SelectMethod="ObterGruposDeHabilitacaoDocenteProvisorios" runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnNumFunc" PropertyName="Value" DbType="String"
                Name="num_func" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsAgrupamentos" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_grupo_habilitacao"
        SqlOrder="descricao" SqlWhere=" ATIVO='S'">
    </techne:TTableDataSource>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <dxtc:ASPxPageControl ID="pcDicsHabilitacaoDoc" runat="server" ActiveTabIndex="1"
        Width="600px" Visible="false">
        <TabPages>
            <dxtc:TabPage Text="Grupos de Disciplinas Habilitadas">
                <ContentCollection>
                    <dxw:ContentControl runat="server">
                        <asp:Panel ID="pnlGrupoHab" runat="server" Visible="false">
                            <asp:HiddenField ID="hdnId" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Grupo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGrupo" runat="server" DataTextField="descricao" DataValueField="agrupamento">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkIngresso" Text="Grupo de Ingresso*" SkinID="lblObrigatorio" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkHabilitacao" Text="Habilitação Matrícula*" SkinID="lblObrigatorio" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkGLP" Text="Habilitação GLP*" SkinID="lblObrigatorio" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Tipo TAD*:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTipoTAD" OnSelectedIndexChanged="ddlTipoTAD_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Text="Selecione" Value="" Selected="True" />
                                            <asp:ListItem Text="DISCIPLINA SEM OBRIGATORIEDADE DE TAD" Value="DISCIPLINA SEM OBRIGATORIEDADE DE TAD" />
                                            <asp:ListItem Text="TAD SEM PROCESSO" Value="TAD SEM PROCESSO" />
                                            <asp:ListItem Text="TAD COM PROCESSO" Value="TAD COM PROCESSO" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Documentação*:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDocumentacao" runat="server" Width="150px" MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                            OnClick="btnSalvar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <dxwgv:ASPxGridView ID="grdDiscHabilit" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdDiscHabilit" DataSourceID="odsDiscHabilitadas" KeyFieldName="CompositeKey"
                            OnAfterPerformCallback="grdDiscHabilit_AfterPerformCallback" OnCustomUnboundColumnData="grdDiscHabilit_CustomUnboundColumnData"
                            OnRowDeleting="grdDiscHabilit_RowDeleting" OnRowUpdating="grdDiscHabilit_RowUpdating" EnableCallBacks="false"
                            OnRowInserting="grdDiscHabilit_RowInserting" OnCellEditorInitialize="grdDiscHabilit_CellEditorInitialize"
                            OnInitNewRow="grdDiscHabilit_InitNewRow" OnCustomButtonCallback="grdDiscHabilit_CustomButtonCallback"
                            OnStartRowEditing="grdDiscHabilit_StartRowEditing" Width="571px">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center" id="dvteste">
                                            <input type="image" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluir" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="num_func" ReadOnly="True" Visible="False"
                                    VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Grupo*" HeaderStyle-Font-Bold="true" FieldName="agrupamento"
                                    VisibleIndex="1" Width="380px">
                                    <PropertiesComboBox DataSourceID="tdsAgrupamentos" TextField="descricao" ValueField="agrupamento"
                                        EnableIncrementalFiltering="true" ValueType="System.String" Width="380px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor escolher um Grupo." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="provisorio" Visible="False" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="dt_limite" Visible="False" VisibleIndex="2">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Docente" FieldName="nome_compl" VisibleIndex="2"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="CompositeKey" UnboundType="String" Visible="False"
                                    VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Grupo de Ingresso*" HeaderStyle-Font-Bold="true"
                                    FieldName="agrupamento_ingresso" VisibleIndex="6" Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                                        ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Habilitação Matrícula*" HeaderStyle-Font-Bold="true"
                                    FieldName="campo_01" VisibleIndex="7" Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                                        ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Habilitação GLP*" HeaderStyle-Font-Bold="true"
                                    FieldName="campo_02" VisibleIndex="8" Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                                        ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Data do cadastro" FieldName="datacadastro"
                                    ReadOnly="true" VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Documentação" FieldName="documentacao" VisibleIndex="10">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Grupos de Disciplinas Habilitadas Provisoriamente" Visible="false">
                <ContentCollection>
                    <dxw:ContentControl runat="server">
                        <dxwgv:ASPxGridView ID="grdDiscHabilitProv" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdDiscHabilitProv" DataSourceID="odsDiscHabilitadasProv"
                            OnCellEditorInitialize="grdDiscHabilitProv_CellEditorInitialize" KeyFieldName="CompositeKeyProv"
                            OnCustomUnboundColumnData="grdDiscHabilitProv_CustomUnboundColumnData1" OnStartRowEditing="grdDiscHabilitProv_StartRowEditing"
                            OnInitNewRow="grdDiscHabilitProv_InitNewRow" OnAfterPerformCallback="grdDiscHabilitProv_AfterPerformCallback"
                            Width="580px">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_Limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="num_func" ReadOnly="True" Visible="False"
                                    VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Grupo*" HeaderStyle-Font-Bold="true" FieldName="agrupamento"
                                    VisibleIndex="1" Width="380px">
                                    <PropertiesComboBox DataSourceID="tdsAgrupamentos" TextField="descricao" ValueField="agrupamento"
                                        EnableIncrementalFiltering="true" ValueType="System.String" Width="380px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor escolher um Grupo." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="provisorio" Visible="False" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Limite*" HeaderStyle-Font-Bold="true"
                                    FieldName="dt_limite" Visible="True" VisibleIndex="2" Width="90px">
                                    <PropertiesDateEdit Width="90px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor preencher a Data Limite" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Docente" FieldName="nome_compl" VisibleIndex="2"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="CompositeKeyProv" UnboundType="String" Visible="False"
                                    VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Habilitação Matrícula*" HeaderStyle-Font-Bold="true"
                                    FieldName="campo_01" VisibleIndex="7" Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                                        ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Habilitação GLP*" HeaderStyle-Font-Bold="true"
                                    FieldName="campo_02" VisibleIndex="8" Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                                        ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
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
                            Confirma a exclusão do grupo de habilitação?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
