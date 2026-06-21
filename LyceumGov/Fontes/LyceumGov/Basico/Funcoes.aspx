<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Funcoes.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Funcoes" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxwgv:ASPxGridView ID="grdFuncoes" runat="server" DataSourceID="tdsFuncoes" AutoGenerateColumns="False"
        KeyFieldName="funcao" OnCellEditorInitialize="grdFuncoes_CellEditorInitialize"
        OnInitNewRow="grdFuncoes_InitNewRow" OnStartRowEditing="grdFuncoes_StartRowEditing"
        ClientInstanceName="grdFuncoes" OnRowInserting="grdFuncoes_RowInserting" OnRowUpdating="grdFuncoes_RowUpdating"
        OnCustomColumnDisplayText="grdFuncoes_CustomColumnDisplayText" OnAfterPerformCallback="grdFuncoes_AfterPerformCallback"
        OnRowValidating="grdFuncoes_RowValidating">
        <SettingsBehavior ConfirmDelete="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdFuncoes.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="Função*" HeaderStyle-Font-Bold="true" FieldName="funcao"
                VisibleIndex="1" Visible="false">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a função." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                VisibleIndex="2" Width="680px">
                <PropertiesTextEdit MaxLength="100">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Tipo*" HeaderStyle-Font-Bold="true" FieldName="funcaobb"
                Width="290px" VisibleIndex="3">
                <PropertiesComboBox DataSourceID="odsItemTabela" MaxLength="40" TextField="descricao"
                    ValueField="tipo" ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar o tipo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Regente?" FieldName="campo_01" VisibleIndex="4"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueType="System.String" ValueUnchecked="N" DisplayTextUndefined=" ">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Função Extra-Classe?" FieldName="campo_02"
                VisibleIndex="5" Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueType="System.String" ValueUnchecked="N">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Libera GLP na 2ª Matrícula?" FieldName="campo_03"
                VisibleIndex="6" Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Diretor?" FieldName="campo_04" VisibleIndex="7"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Secretário?" FieldName="campo_05" VisibleIndex="8"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Desaloca aulas?" FieldName="campo_06" VisibleIndex="9"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Permite GLP?" FieldName="campo_07" VisibleIndex="10"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S" 
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
              <dxwgv:GridViewDataCheckColumn Caption="Compatível com Ensino Médio - DOC I?" FieldName="campo_08" VisibleIndex="11"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S" 
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Compatível com Ensino Fundamental - DOC II?" FieldName="campo_09" VisibleIndex="12"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S" 
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Permite Contrato Temporário?" FieldName="campo_10" VisibleIndex="13"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S" 
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
              <dxwgv:GridViewDataCheckColumn Caption="Sem CH Efetiva?" FieldName="SEMCARGAHORARIAEFETIVA" VisibleIndex="14"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S" 
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
              <dxwgv:GridViewDataCheckColumn Caption="Ativa?" FieldName="ATIVO" VisibleIndex="14"
                Width="90px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S" 
                    ValueUnchecked="N" ValueType="System.String">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsFuncoes" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_funcao"
        SqlOrder="descricao">
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsItemTabela" runat="server" SelectMethod="ConsultaItemTabela"
        TypeName="Techne.Lyceum.RN.Basico">
        <SelectParameters>
            <asp:Parameter DefaultValue="Tipo Função" Name="Tab" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Button ID="Button1" runat="server" Text="Exportar" OnClick="Button1_Click_ExportarButton1_Click" />

      </asp:Content>
