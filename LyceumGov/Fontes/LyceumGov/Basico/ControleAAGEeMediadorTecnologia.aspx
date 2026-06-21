<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ControleAAGEeMediadorTecnologia.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ControleAAGEeMediadorTecnologia"
    Title="Controle AAGE e Mediador Tecnologia" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o ID/Vínculo ou o nome do docente"
        Height="45px" Width="784px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="ID/Vínculo do Docente*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocentes" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocenteAAGE"
                        AutoPostBack="true" MaxLength="15" OnTextChanged="tseDocentes_Changed" ValueField="num_func">
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
    <div class="divEditBlock" style="width: 968px;">
        <asp:Label runat="server" ID="lblBloco" Text="Docentes" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <asp:HiddenField runat="server" ID="hdnPerfil" />
    <dxtc:ASPxPageControl ID="apcDocente" runat="server" Height="299px" Width="800px"
        ActiveTabIndex="0" TabIndex="0" Visible="false">        
        <TabPages>
            <dxtc:TabPage Name="DadosPessoais" Text="Dados Pessoais">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Docente:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDocente" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Nome Completo:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNome" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="CPF:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCPF" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Data de Nascimento:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDataNascimento" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Sexo:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSexo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Estado Civil:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEstadoCivil" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlEndereco" runat="server" GroupingText="Endereço">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Endereço:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEndereco" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Nº:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNumero" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Complemento:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblComplemento" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Bairro:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBairro" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="CEP:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCEP" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="Município:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMunicipio" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Telefone:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTelefone" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlLotacao" runat="server" GroupingText="Lotação">
                            <dxwgv:ASPxGridView ClientInstanceName="grdLotacao" ID="grdLotacao" runat="server"
                                AutoGenerateColumns="False" KeyFieldName="MATRICULA">
                                <Columns>
                                 <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="IDVINCULO" VisibleIndex="1">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="MATRICULA" VisibleIndex="1">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Lotação" FieldName="NOME_COMP" VisibleIndex="3"
                                        ReadOnly="True">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Tipo Função" FieldName="TIPOFUNCAO" VisibleIndex="4"
                                        ReadOnly="True">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="DESCRICAO" VisibleIndex="5"
                                        ReadOnly="True">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="UA Antiga" FieldName="SETOR" VisibleIndex="6" ReadOnly="True">
                                    </dxwgv:GridViewDataTextColumn>
                                     <dxwgv:GridViewDataTextColumn Caption="UA" FieldName="ua_atual" VisibleIndex="6" ReadOnly="True">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="7"
                                        ReadOnly="True">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="Vinculos" Text="Vínculos">
                <ContentCollection>
                    <dxw:ContentControl ID="ccVinculos" runat="server">
                        <dxwgv:ASPxGridView ID="grdMediador" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdMediador"
                            KeyFieldName="DOCENTEMEDIADOR_UNIDADEENSINO_ID" DataSourceID="odsMediador" OnCellEditorInitialize="grdMediador_CellEditorInitialize"
                            OnRowValidating="grdMediador_RowValidating" Width="800px" OnHtmlRowCreated="grdMediador_HtmlRowCreated"
                            OnRowDeleting="grdMediador_RowDeleting" OnRowInserting="grdMediador_RowInserting"
                            OnRowUpdating="grdMediador_RowUpdating" OnCommandButtonInitialize="grdMediador_CommandButtonInitialize"
                            OnStartRowEditing="grdMediador_StartRowEditing">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFuncao" runat="server" Text="Tipo Função:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="TIPOFUNCAO" ID="ASPxGridViewTemplateReplacement5"
                                                            ReplacementType="EditFormCellEditor" runat="server">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUnidade" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" AutoPostBack="false" SqlOrder="NOME_COMP"
                                                            ColumnName="UNIDADE_ENS" Caption="" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio, ua_atual  from VW_UNIDADE_ENSINO_SITUACAO "
                                                            SqlWhere=" situacao = 'ESTADUAL'" FieldName="UNIDADE_ENS" MaxLength="8" DataType="Varchar"
                                                            Value='<%# Bind("UNIDADEENSINOID") %>'>
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="UNIDADE_ENS" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="NOME_COMP" Width="80%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDataInicio" runat="server" Text="Data Início:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAINICIO_VINCULO" ID="ASPxGridViewTemplateReplacement3"
                                                            ReplacementType="EditFormCellEditor" runat="server">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDataFim" runat="server" Text="Data Fim:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAFIM_VINCULO" ID="ASPxGridViewTemplateReplacement4"
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
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <dxe:ASPxImage ID="btnNovoGrid" runat="server" ClientInstanceName="btnNovoGrid" Cursor="pointer"
                                            ImageUrl="../img/bt_novo.png" OnLoad="imgMediador_Load">
                                            <ClientSideEvents Click="function(s, e) { grdMediador.AddNewRow(); }" />
                                        </dxe:ASPxImage>
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
                                <dxwgv:GridViewDataTextColumn Caption="DOCENTEMEDIADOR_UNIDADEENSINO_ID" FieldName="DOCENTEMEDIADOR_UNIDADEENSINO_ID"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Função" FieldName="TIPOFUNCAO" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NumFunc" FieldName="DOCENTEID" VisibleIndex="3"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="UNIDADEENSINOID" FieldName="UNIDADEENSINOID"
                                    Visible="false" VisibleIndex="5" Width="150px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="6"
                                    Visible="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO_VINCULO"
                                    VisibleIndex="7" Width="90px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor selecionar a data de início." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM_VINCULO" VisibleIndex="8"
                                    Width="90px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor selecionar a data de fim." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <br />
                        <dxwgv:ASPxGridView ID="grdArticulador" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdArticulador" KeyFieldName="DOCENTEARTICULADOR_REGIONAL_ID"
                            DataSourceID="odsArticulador" OnCellEditorInitialize="grdArticulador_CellEditorInitialize"
                            OnRowValidating="grdArticulador_RowValidating" Width="800px" OnHtmlRowCreated="grdArticulador_HtmlRowCreated"
                            OnRowDeleting="grdArticulador_RowDeleting" OnRowInserting="grdArticulador_RowInserting"
                            OnRowUpdating="grdArticulador_RowUpdating" OnCommandButtonInitialize="grdArticulador_CommandButtonInitialize"
                            OnStartRowEditing="grdArticulador_StartRowEditing">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFuncao" runat="server" Text="Tipo Função:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="TIPOFUNCAO" ID="ASPxGridViewTemplateReplacement5"
                                                            ReplacementType="EditFormCellEditor" runat="server">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                                            FieldName="id_regional" MaxLength="20" Columns="10" AutoPostBack="False" Caption=""
                                                            SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                                            DataType="Number" Value='<%# Bind("REGIONALID") %>'>
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDataInicio" runat="server" Text="Data Início:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAINICIO_VINCULO" ID="ASPxGridViewTemplateReplacement3"
                                                            ReplacementType="EditFormCellEditor" runat="server">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDataFim" runat="server" Text="Data Fim:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAFIM_VINCULO" ID="ASPxGridViewTemplateReplacement4"
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
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <dxe:ASPxImage ID="btnNovoGrid" runat="server" ClientInstanceName="btnNovoGrid" Cursor="pointer"
                                            ImageUrl="../img/bt_novo.png" OnLoad="imgMediador_Load">
                                            <ClientSideEvents Click="function(s, e) { grdArticulador.AddNewRow(); }" />
                                        </dxe:ASPxImage>
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
                                <dxwgv:GridViewDataTextColumn Caption="DOCENTEARTICULADOR_REGIONAL_ID" FieldName="DOCENTEARTICULADOR_REGIONAL_ID"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Função" FieldName="TIPOFUNCAO" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NumFunc" FieldName="DOCENTEID" VisibleIndex="3"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="REGIONALID" FieldName="REGIONALID" Visible="false"
                                    VisibleIndex="5" Width="150px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="6"
                                    Visible="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO_VINCULO"
                                    VisibleIndex="7" Width="90px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor selecionar a data de início." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM_VINCULO" VisibleIndex="8"
                                    Width="90px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor selecionar a data de fim." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <br />
                        <dxwgv:ASPxGridView ID="grdAAGE" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdAAGE"
                            KeyFieldName="DOCENTEAAGE_UNIDADEENSINO_ID" DataSourceID="odsAAGE" OnCellEditorInitialize="grdAAGE_CellEditorInitialize"
                            OnRowValidating="grdAAGE_RowValidating" Width="800px" OnHtmlRowCreated="grdAAGE_HtmlRowCreated"
                            OnRowDeleting="grdAAGE_RowDeleting" OnCommandButtonInitialize="grdAAGE_CommandButtonInitialize"
                            OnRowInserting="grdAAGE_RowInserting" OnRowUpdating="grdAAGE_RowUpdating" OnStartRowEditing="grdAAGE_StartRowEditing">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFuncao" runat="server" Text="Tipo Função:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="TIPOFUNCAO" ID="ASPxGridViewTemplateReplacement5"
                                                            ReplacementType="EditFormCellEditor" runat="server">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUnidade" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" AutoPostBack="false" SqlOrder="NOME_COMP"
                                                            ColumnName="UNIDADE_ENS" Caption="" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio, ua_atual from VW_UNIDADE_ENSINO_SITUACAO "
                                                            SqlWhere=" situacao = 'ESTADUAL'" FieldName="UNIDADE_ENS" MaxLength="8" DataType="Varchar"
                                                            Value='<%# Bind("UNIDADEENSINOID") %>'>
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="UNIDADE_ENS" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="NOME_COMP" Width="80%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDataInicio" runat="server" Text="Data Início:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAINICIO_VINCULO" ID="ASPxGridViewTemplateReplacement3"
                                                            ReplacementType="EditFormCellEditor" runat="server">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDataFim" runat="server" Text="Data Fim:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAFIM_VINCULO" ID="ASPxGridViewTemplateReplacement4"
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
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <dxe:ASPxImage ID="btnNovoGrid" runat="server" ClientInstanceName="btnNovoGrid" Cursor="pointer"
                                            ImageUrl="../img/bt_novo.png" OnLoad="imgAAGE_Load">
                                            <ClientSideEvents Click="function(s, e) { grdAAGE.AddNewRow(); }" />
                                        </dxe:ASPxImage>
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
                                <dxwgv:GridViewDataTextColumn Caption="DOCENTEAAGE_UNIDADEENSINO_ID" FieldName="DOCENTEAAGE_UNIDADEENSINO_ID"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Função" FieldName="TIPOFUNCAO" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NumFunc" FieldName="DOCENTEID" VisibleIndex="3"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="UNIDADEENSINOID" FieldName="UNIDADEENSINOID"
                                    Visible="false" VisibleIndex="5" Width="150px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="6"
                                    Visible="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DATAINICIO_VINCULO"
                                    VisibleIndex="7" Width="90px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor selecionar a data de início." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM_VINCULO" VisibleIndex="8"
                                    Width="90px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor selecionar a data de fim." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <asp:ObjectDataSource ID="odsMediador" TypeName="Techne.Lyceum.Net.Basico.ControleAAGEeMediadorTecnologia"
        runat="server" SelectMethod="Listar" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseDocentes" DefaultValue="" Name="docente" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsArticulador" TypeName="Techne.Lyceum.Net.Basico.ControleAAGEeMediadorTecnologia"
        runat="server" SelectMethod="ListarArticulador" InsertMethod="InsertArticulador"
        UpdateMethod="UpdateArticulador" DeleteMethod="DeleteArticulador">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseDocentes" DefaultValue="" Name="docente" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAAGE" TypeName="Techne.Lyceum.Net.Basico.ControleAAGEeMediadorTecnologia"
        runat="server" SelectMethod="ListarAAGE" InsertMethod="InsertAAGE" UpdateMethod="UpdateAAGE"
        DeleteMethod="DeleteAAGE">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseDocentes" DefaultValue="" Name="docente" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
